using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SVIPT_BioLab
{
    static class TeensyUsbComConstants
    {       
        //port serial. 
        public const string PortName = "COM6"; //PortName NÃO PODE SER "" (Gera ArgumentException: The PortName property was set to a value with a length of zero.)!!!!
        public const int BaudRate = 115200; //baud rate --- idêntica à definida no forware do Teensy
        public const int DataBits = 8; //data bits
        public const System.IO.Ports.StopBits StopBits = System.IO.Ports.StopBits.One; //one stop bit
        public const bool DtrEnable = false; //Data Terminal Ready signal desabilitado
        public const bool RtsEnable = false; //Request to Send signal disabled
        public const int Handshake = 0; //no handshake
        public const int Parity = 0; //no parity check
        public const int ParityReplace = 0; //Replace invalid bytes if parity error occurs.
                                            //Só efetivo se parity estiver habilitada!
        public const int ReadBufferSize = 1024; //Read Buffer 1KB
        public const int ReadTimeOut = 1000;     //Read timeout 1 seg
        public const int ReceivedBytesThreshold = TeensyComCommands.tCMDSize; //TODO PACOTE recebido é precedido do nome do comando
                                                    //"Mínima" quantidade de bytes no buffer 
                                                    //Rx que dispara um evento DataReceived. 
                                                    //The DataReceived event is also raised if an Eof
                                                    //character is received, regardless of the number of bytes 
                                                    //in the internal input buffer and the value of the 
                                                    //ReceivedBytesThreshold property.
        public const int WriteBufferSize = 1024; //Write Buffer 1KB
        public const int WriteTimeout = 1000;     //Write timeout 1 seg
        public const string Encoding = "us-ascii";  //Codificação usada para comunicação via TEXTO (strings/chars).
        public const string StringEndMsg = TeensyComCommands.terminatorChar; //Fim de string

        //Versao do Firmware com o qual este programa pode conversar 
        public const string Firmware = "Teensy SVIPT 1.0";

        //Descrição do PORT COM DO (DEVE SER IDENTICA à descrição 
        //do bus USB - ver control panel -> device manage)
        public const string Teensy_PortDescription = "USB Serial Device"; //"Teensy USB Serial" Para o pc do alcimar

    } //static class TeensyUsbComConstants

    static class TeensyComCommands
    {
        public const int tCMDSize = 4;     //Nomes dos comandos possuem sempre 4 caracteres
        public const string terminatorChar = "\0";    //Encerra mensagens que são STRINGS.
            //TODA comunicação no sentido Teensy -> Host (resposta a comandos) será precedida pelo comando
            //que a originou !
        public const string tCMD_SendFmw = "FMWR";  //Comando: Host solicita o envio do firmware
                                                    //Em resposta, Teensy envia o nome do comando seguido do firmware
                                                    //    "FMWRTeensy SVIPT 1.0'\0'"
        public const string tCMD_SendADrange = "ADRA";  //Comando: Host solicita o envio do range do conversor AD
                                                        //Em resposta, Teensy envia o nome do comando seguido do range em
                                                        //um string (range será enviado com uma casa decimal --- 5.0, 3.3 ...)
                                                        //    "ADRA5.0'\0'"                                                      
        public const string tCMD_SendADnbits = "ADNB"; //Comando: Host solicita o envio da quantidade de bits do conversor AD.
                                                       //Em resposta, Teensy envia o nome do comando seguido do nro de bits em
                                                       //um string (nbits - integer)
                                                       //    "ADNB10'\0'"
        public const string tCMD_SendADtxAmost = "ADTX"; //Comando: Host solicita o envio da taxa de amostragem usada na digitalização
                                                         //Em resposta, Teensy envia o nome do comando seguido da tx amostr em
                                                         //um string (TxAmostr - integer)
                                                         //    "ADNB200'\0'"
        public const string tCMD_StartAquis = "STAQ"; //Comando: Iniciar aquisição de dados e envio pela serial para o host.
                                                      //Host inicia aquisição de dados e envio para o host.
                                                      //Os dados serão enviados em pacotes com múltiplos de 2 bytes
                                                      //(ver abaixo).
                                                      //Cada pacote será o seguinte formato:
                                                      // | "STAQ" | #bytesPack | bytes pack |
                                                      // tCMDSize (atualmente 4) chars com nome do comando, seguido
                                                      // de 1 BYTE com a quantidade de bytes do pacote, seguido dos bytes do
                                                      //pacote. Cada amostra corresponde a 2 bytes (MSB first).  
        public const string tCMD_StopAquis = "SPAQ";  //Comando: Parar aquisição de dados e parar envio pela serial para o host


        //Este método retorna true se o string enviado é um dos possíveis comandos
        public static bool isValidCommand(string strCmd)
        {
            //Este array deve conter todos os comandos declarados acima
            string[] validCommand = {
                tCMD_SendFmw, tCMD_SendADrange, tCMD_SendADnbits, tCMD_SendADtxAmost,
                tCMD_StartAquis, tCMD_StopAquis
            };

            return validCommand.Contains(strCmd);
        }

    }

    /* TeensyData:
    Esta classe conterá os diversos elementos associados ao equipamento,
    firmware, status, dados de experimento etc. */
    public class TeensyData : IDisposable
    {

        #region Defines

        //Propriedades booleans
        public bool handShakeOk;        //Indica se o handshake foi realizado com sucesso.
        public bool ready;              //Teensy está pronto para receber comandos.

        //Propriedades string
        public string firmwareVersion;  //versão do firmware gravado no equipamento.

        //propriedades double
        public double ad_Range;    //Range (fundo de escala analógico) do AD deste Teensy.
        public double ad_nBits;    //Quantidade de bits do AD do teensy
        public double ad_TxAmost;  //taxa de amostragem da conversão AD no teensy 

        //Um buffer circular utilizado para armazenar dados recebidos 
        //em cada evento DataReceived do port usb-serial. 
        public CircularBufferBytes serialDataBC;

        //Uma lista para conter o sinal digitalizado (bytes).
        //Esta lista é preenchida à medida que novos dados digitalizados (sinal)
        //são disponíveis no serialDataBC.
        public List<byte> signalDataBytes;

        //A classe contém um timer para verificar timeout em determinadas ações (ver métodos
        //waitValueChange.
        private System.Timers.Timer timeOutTimer;
        //uma flag para indicar timeout
        private bool timeOut;

        /*
        //Um public enum para marcar cada propriedade acima agrupada em enums de cada tipo
        //Enum propriedades booleanas
        public enum tDataPropertiesBool
        {
            HandShakeOk,
            Ready
        }
        //Enum propriedades string
        public enum tDataPropertiesString
        {
            FirmwareVersion
        }
        //Enum propriedades double
        public enum tDataPropertiesDouble
        {
            AD_Range, AD_nBits,AD_TxAmost
        }
        */

        /*
        //Set - Propriedades booleanas
        public void setProperty(tDataPropertiesBool property, bool val)
        {
            switch (property)
            {
                case tDataPropertiesBool.HandShakeOk:
                    handShakeOk = val;
                    break;
                case tDataPropertiesBool.Ready:
                    ready = val;
                    break;
                default:
                    break;
            }

        }

        //Get - Propriedades booleanas.
        //Retorna o valor da propriedade.
        public bool getProperty(tDataPropertiesBool property)
        {
            bool res = false;

            switch (property)
            {
                case tDataPropertiesBool.HandShakeOk:
                    res = handShakeOk;
                    break;
                case tDataPropertiesBool.Ready:
                    res = ready;
                    break;
                default:
                    res = false;
                    break;
            }
            return res;
        }

        //Set - Propriedades string
        public void setProperty(tDataPropertiesString property, string val)
        {
            switch (property)
            {
                case tDataPropertiesString.FirmwareVersion:
                    firmwareVersion = val;
                    break;
                default:
                    break;
            }

        }

        //Get - Propriedades string.
        //Retorna o valor da propriedade.
        public string getProperty(tDataPropertiesString property)
        {
            string res = "";

            switch (property)
            {
                case tDataPropertiesString.FirmwareVersion:
                    res = firmwareVersion;
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        //Set - Propriedades double
        public void setProperty(tDataPropertiesDouble property, double val)
        {
            switch (property)
            {
                case tDataPropertiesDouble.AD_nBits:
                    ad_nBits = val;
                    break;
                case tDataPropertiesDouble.AD_Range:
                    ad_Range = val;
                    break;
                case tDataPropertiesDouble.AD_TxAmost:
                    ad_TxAmost = val;
                    break;
                default:
                    break;
            }
        }

        //Get - Propriedades double.
        //Retorna o valor da propriedade.
        public double getProperty(tDataPropertiesDouble property)
        {
            double res = -999999999;

            switch (property)
            {
                case tDataPropertiesDouble.AD_nBits:
                    res = ad_nBits;
                    break;
                case tDataPropertiesDouble.AD_Range:
                    res = ad_Range;
                    break;
                case tDataPropertiesDouble.AD_TxAmost:
                    res = ad_TxAmost;
                    break;
                default:
                    res = -999999999;
                    break;
            }
            return res;
        }
        */

        #endregion

        //construtor
        public TeensyData()
        {
            handShakeOk = false;
            ready = false;
            firmwareVersion = string.Empty;
            ad_nBits = -1; //NÃO deve ser igual ao real ainda... colocar outro valor qualquer
            ad_Range = -1;
            ad_TxAmost = -1;

            //Um buffer circular utilizado para armazenar dados recebidos 
            //em cada evento DataReceived do port usb-serial. 
            serialDataBC = new CircularBufferBytes(20480); //duas vezes o tamanho do buffer da USB - just in case

            //Uma lista para conter o sinal digitalizado (bytes).
            signalDataBytes = new List<byte>();

            //A classe contém um timer para verificar timeout em determinadas ações (ver métodos
            //waitValueChange.
            timeOutTimer = new System.Timers.Timer();
            timeOutTimer.Elapsed += OnTimeOutEvent; //handler
            timeOut = false;
        }

        //Dispose
        bool disposed;

        //Call Dispose to free resources explicitly, if you wish
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //dispose method -- chamado quando o objeto é destruído etc
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    //Parar timer se estiver rodadndo...
                    timeOutTimer.Stop();

                    //outras açoes de finalização necessárias.
                    //...
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        #region methods

        //Esta função, verifica se determinado dado chegou no buffer circular serialDataBC.
        //Se chegou, armazena o valor na propriedade correspondente (deste objeto).
        //Aguarda até timeOut milisegundos pela chegada.
        // AS PROPRIEDADES de que trata esta função são definidas pelos headers dos pacotes
        //enviados pelo Teensy: nome do comando que deu origem à informação enviada (TeensyComCommands).
        //Ou ainda: Os pacotes que chegam no buffer circular serialDataBC são SEMPRE precedidos
        //          pelo nome do comando que deu origem à informação enviada pelo Teensy.

        //waitValue.
        //Inputs: 
        //      header - String com nome do comando/header associado à mensagem esperada.
        //      _timeOutMs - limite de tempo (milisegundos) de espera.
        //      NewVal: referencia para o objeto onde será escrito o valor da propriedade recebida
        //              como retorno.
        //Retun:
        //      true: dado recebido e seu novo valor foi escrito na propriedade associada ao header.
        //      false: timeOut - dado não recebido.
        public bool waitValue(String header, int _timeOutMs)
        {

            //return true; //retirar
            bool fim = false;
            char[] auxChar;
            auxChar = new char[30];
            //eu poderia ter feito de forma a reconhecer o tamanho da mensagem e criar um array de chars do tamanho exato
            //entretanto, eu sei qual o tamanho maximo da mensagem, nao preciso economizar espaco de memoria e ficaria mais lento
            //entao uso esse array de tamanho maior que o maximo da mensagem, salvo a mensagem nele e depois passo ela para o local apropriado
            char[] packHeader;
            int i, indexPackHeader;
            bool wait = true;
            packHeader = new char[TeensyComCommands.tCMDSize];
            //inicializar packHeader com terminador de strings/header (importante no loop a seguir).
            for (i = 0; i < TeensyComCommands.tCMDSize; i++)
                packHeader[i] = '\0';

            indexPackHeader = 0;
            //iniciar timer timeout
            startTimeOutCounter(_timeOutMs);
            while (!timeOut && wait)
            {
                //aguardar chegada do header desejado no buffer serialDataBC.
                //NOTE QUE tudo que chegar antes será descartado do buffer circular.
                
                if (serialDataBC.count() > 0 && indexPackHeader < TeensyComCommands.tCMDSize) 
                {//no if acima, tava faltando o "&& indexPackHeader < TeensyComCommands.tCMDSize" entao era possivel o indice passar a posicao maxima do packHeader
                    packHeader[indexPackHeader] = (char)serialDataBC.read();
                    indexPackHeader++;
                    //chegou header completo?
                    string s = new string(packHeader);
                    if (string.Compare(s, header) == 0) // USAR switch (abaixo)
                    {
                        //chegou header - aguardar restante do pacote, conforme o header.
                        fim = false;
                        switch (s)
                        {
                            case TeensyComCommands.tCMD_SendFmw:
                                i = 0;
                                while (!fim && !timeOut)
                                {
                                    if (serialDataBC.count() > 0) //deveria receber "FMWRTeensy SVIPT 1.0", 
                                    { //ou seja, 4+16+1 bytes, portanto o count que chega aqui deve ser 17, dado que os 4 do cabecalho ja foram lidos
                                        auxChar[i] = (char)serialDataBC.read();
                                        if (auxChar[i] == '\0')
                                            fim = true;
                                        //checa o tamanho da mensagem e salva tal mensagem em um char auxiliar
                                        i++;
                                    }
                                }
                                if (timeOut)
                                    break;
                                string auxStrFmw = new string(auxChar,0,i-1);
                                //cria uma string nova e salva nela o novo char, ate a posicao onde acaba a mensagem
                                firmwareVersion = auxStrFmw;
                                wait = false;
                                break;

                            case TeensyComCommands.tCMD_SendADnbits:
                                i = 0;
                                while (!fim && !timeOut)
                                {
                                    if (serialDataBC.count() > 0) //deveria receber "ADNB10"
                                    {//ou seja, 4+2+1 bytes, entao o count quando chega aqui deve ser 3 pq ja leu o cabecalho
                                        auxChar[i] = (char)serialDataBC.read();
                                        if (auxChar[i] == '\0')
                                        {
                                            fim = true;
                                        } //checa o tamanho da mensagem e salva tal mensagem em um char auxiliar
                                        i++;
                                    }
                                }//passa para uma string auxiliar do tamanho exato da mensagem
                                if (timeOut)
                                    break;
                                string auxStrADnbits = new string(auxChar, 0, i - 1);
                                //por fim converte para double e atribui ao ad_nBits
                                try
                                {
                                    ad_nBits = Double.Parse(auxStrADnbits, System.Globalization.NumberFormatInfo.InvariantInfo);
                                }
                                catch
                                {
                                    ad_nBits = 1;
                                }

                                wait = false;
                                break;

                            case TeensyComCommands.tCMD_SendADrange: 
                                i = 0;
                                while (!fim && !timeOut)
                                {//deve receber "ADRA5.0" ou seja "65,68,82,65,53,46,48,0", 4+3+1bytes
                                    if (serialDataBC.count() > 0)//entao aqui o count deve ser 4, pq ja leu o cabecalho
                                    {
                                        auxChar[i] = (char)serialDataBC.read();
                                        if (auxChar[i] == '\0')
                                        {
                                            fim = true;
                                        } //checa o tamanho da mensagem e salva tal mensagem em um char auxiliar
                                        i++;
                                    }
                                }//passa para uma string auxiliar do tamanho exato da mensagem
                                if (timeOut)
                                    break;
                                string auxStrADrange = new string(auxChar, 0, i - 1);
                                //por fim converte para double e atribui ao ad_Range
                                try
                                {
                                    ad_Range = Double.Parse(auxStrADrange, System.Globalization.NumberFormatInfo.InvariantInfo);
                                }
                                catch
                                {
                                    ad_Range = -1;
                                }

                                wait = false;
                                break;

                            case TeensyComCommands.tCMD_SendADtxAmost:
                                i = 0;
                                while (!fim && !timeOut)
                                {//deveria receber "ADTX200" ou seja, "65,68,84,88,50,48,48" 4+3+1bytes
                                    if (serialDataBC.count() > 0)//entao aqui o count deve ser 4, pq ja leu o cabecalho
                                    {
                                        auxChar[i] = (char)serialDataBC.read();
                                        if (auxChar[i] == '\0')
                                        {
                                            fim = true;
                                        } //checa o tamanho da mensagem e salva tal mensagem em um char auxiliar
                                        i++;
                                    }
                                }//passa para uma string auxiliar do tamanho exato da mensagem
                                if (timeOut)
                                    break;
                                string auxStrADtxAmost = new string(auxChar, 0, i - 1);
                                //por fim converte para double e atribui ao ad_TxAmost
                                try
                                {
                                    ad_TxAmost = Double.Parse(auxStrADtxAmost, System.Globalization.NumberFormatInfo.InvariantInfo);
                                }
                                catch
                                {
                                    ad_TxAmost = 1;
                                }

                                wait = false;
                                break;
                                
                            case TeensyComCommands.tCMD_StartAquis:

                                break;

                            default:
                                //Header não reconhecido
                                break;
                        }

                        //se chegou o pacote completo, escreva o valor na propriedade associada ao header
                        //e termine o loop while.

                    }
                                 else
                                 {
                                     //Ainda não chegou header.
                                     //se packHeader já estiver cheio, vamos deslizar os chars para esquerda
                                     //e liberar a última posição para continuar tentando completar o header.
                                     if(indexPackHeader == TeensyComCommands.tCMDSize)
                                     {
                                         for (i = 1; i < TeensyComCommands.tCMDSize; i++)
                                             packHeader[i - 1] = packHeader[i];
                                         packHeader[i-1] = '\0'; //acho que packHeader[i] ta errado, to colocando packHeader[i-1]
                                         indexPackHeader--; //liberar uma posição (ultima) para próximo char do header
                                     }
                                 }

                             }

                         }
                         stopTimeOutCounter(); //toda vez que parava o contador, fazia timeout=false

                         if (timeOut) //nunca entrava nesse return
                             return false;
                         else
                         {
                             return true;
                         }

        }



        #endregion

        #region timer TimeOut

        //Iniciar timeOut counter com o time out enviado em mseg
        private void startTimeOutCounter(int timeOutMs)
        {
            timeOutTimer.Interval = timeOutMs;
            timeOut = false;
            timeOutTimer.Enabled = true;
        }

        //Finalizar timeOut counter
        private void stopTimeOutCounter()
        {
            //timeOut = false; //nao faz sentido fazer o timeout ser falso quando vc para ele
            timeOutTimer.Enabled = false;
        }

        //Timer event handler (para timeout)
        private void OnTimeOutEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            //se ocorreu evento, sinalize timeout
            timeOut = true;
        }

        #endregion

        
                /* PARSING DATA
            int indexHeader;

            string msgHeader = ""; //Note que as mensagens recebidas sempre possuem um header,
                                   //o comando que as originou (ver protocolo no início deste arquivo).

            //A mensagem deve conter pelo menos tCMDSize (nome de comando).           
            if (indexWritePackUSB < TeensyComCommands.tCMDSize)
                return "INCOMPLETE";

            //varre os dados contidos no pacote em busca de um header de mensagens.
            //A mensagem pode conter apenas o header.
            int maxIndex = indexWritePackUSB - TeensyComCommands.tCMDSize + 1;
            for (indexHeader = 0; indexHeader < maxIndex; indexHeader++)
            {
                msgHeader = encoding.GetString(pack, indexHeader, indexHeader + TeensyComCommands.tCMDSize);
                if (TeensyComCommands.isValidCommand(msgHeader))
                    break;
            }
            //se não localizou nenhuma mensagem ainda, ela deve estar incompleta.
            if (indexHeader == maxIndex)
                return "INCOMPLETE";

            //se o header não está no início, desloque todo o pacote para a esquerda.
            //isso pode acelerar novas buscas deste header, em uma nova chamada desta função,
            //caso sua mensagem não esteja completa.
            if (indexHeader > 0)
            {
                int auxindexWritePackUSB = indexWritePackUSB;
                for (int i = 0; i < auxindexWritePackUSB; i++)
                {
                    pack[i] = pack[i + indexHeader];
                    pack[i + indexHeader] = 0xff; //clear - não usar 0x00 pois o caracter null ('\0'= 0x00) é usado como terminado r de msgs.
                    indexWritePackUSB--;
                }
            }

            //localizou um header.
            //parse message - Header é nome de um comando
            switch (msgHeader)
            {
                case TeensyComCommands.tCMD_SendFmw:
                    //Enviou firmware?
                    //deve conter terminador de mensagens
                    if (!pack.Contains((byte)TeensyComCommands.terminatorChar[0]))
                        return "INCOMPLETE";

                    this.firmwareVersion = encoding.GetString(pack, TeensyComCommands.tCMDSize,
                                                              indexWritePackUSB - TeensyComCommands.tCMDSize - 1); //-1 retirar \0 da conversão
                    break;

                case TeensyComCommands.tCMD_SendADnbits:
                    //Enviou quantidade de bits (int) do AD do teensy
                    //deve conter terminador de mensagens
                    if (!pack.Contains((byte)TeensyComCommands.terminatorChar[0]))
                        return "INCOMPLETE";

                    this.ad_nBits = int.Parse(encoding.GetString(pack, TeensyComCommands.tCMDSize,
                                                         indexWritePackUSB - TeensyComCommands.tCMDSize - 1));
                    break;

                case TeensyComCommands.tCMD_SendADrange:
                    //Enviou o range do AD (double) do AD do teensy.
                    //deve conter terminador de mensagens
                    if (!pack.Contains((byte)TeensyComCommands.terminatorChar[0]))
                        return "INCOMPLETE";

                    //o string contendo o nro está no padrão inglês - '.' é o marcador
                    //de decimais. MAS, dependendo da cultura, deveria ser ','.
                    this.ad_Range = Double.Parse(encoding.GetString(pack, TeensyComCommands.tCMDSize,
                                                                    indexWritePackUSB - TeensyComCommands.tCMDSize - 1),
                                                 System.Globalization.NumberFormatInfo.InvariantInfo);
                    break;

                case TeensyComCommands.tCMD_SendADtxAmost:
                    //Enviou taxa de amostrage (int) do sinal 
                    //deve conter terminador de mensagens
                    if (!pack.Contains((byte)TeensyComCommands.terminatorChar[0]))
                        return "INCOMPLETE";

                    this.ad_TxAmost = int.Parse(encoding.GetString(pack, TeensyComCommands.tCMDSize,
                                                         indexWritePackUSB - TeensyComCommands.tCMDSize - 1));
                    break;

                case TeensyComCommands.tCMD_StartAquis:
                    //Enviou pacote de bytes da conversão AD do sinal.
                    //Guardar no buffer circular.
                    //Cada pacote será o seguinte formato:
                    // | "STAQ" | #bytesPack | bytes pack |

                    //tamanho do pacote devce ser igual a:
                    //  TeensyComCommands.tCMDSize + 1 + #bytesPack
                    //Chegou pelo o tamanho do pacote de dados?
                    if (indexWritePackUSB < (TeensyComCommands.tCMDSize + 1))
                        return "INCOMPLETE"; //não
                    //ok - chegaram todos os bytes do pacote de dados?
                    if (indexWritePackUSB < (TeensyComCommands.tCMDSize + 1 + (int)pack[TeensyComCommands.tCMDSize]))
                        return "INCOMPLETE"; //não

                    // tCMDSize (atualmente 4) chars com nome do comando, seguido
                    // de 1 BYTE com a quantidade de bytes do pacote, seguido dos bytes do
                    //pacote. Cada amostra corresponde a 2 bytes (MSB first). 
                    int nBytes = (int)pack[TeensyComCommands.tCMDSize];
                    for (int i = TeensyComCommands.tCMDSize + 1; i < nBytes; i++)
                        SVIPTdataBC.write(pack[i]);
                    break;

                default:
                    //Header não reconhecido
                    return "ERROR";
                    break;
            }
              */

    }


}
