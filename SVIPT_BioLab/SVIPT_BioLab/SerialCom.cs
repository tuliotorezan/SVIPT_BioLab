using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

using System.Threading;
using System.Windows.Forms;
using System.Timers;

namespace SVIPT_BioLab
{
    // Para PROTOCOLO DE COMUNICAÇÃO serial Teensy 2.0 <-> Host => Ver definições em TeensyData.cs

    /* TeensyUsbComPort:
     * 
     *      Esta classe implementa todas as ações para comunicação USB-CDC
     *      com o Teensy, via Serial (COM) Port:
     *          - TODOS os dados recebidos serão armazenados num objeto (buffer circular) da classe TeensyData.  
     */

    public class TeensyUsbCom : IDisposable
    {
        #region Defines - Variáveis, constantes e defines

        public System.IO.Ports.SerialPort serialPort; //O PORT COM

        TeensyData teensyDataObj;

        #endregion

        //Contrutores
        #region Constructors

        //Inicializa Objeto de comunicação USB.
        //Recebe uma referência ao objeto TeensyData para armazenamento das mensagens recebidas.
        //port usb-serial sem nome - utilizando configuração default para comunicação. 
        //As propriedades default estão definidas na classe estatica MipUsbComConstants.
        public TeensyUsbCom(ref TeensyData _TeensyDataObj)
        {
            teensyDataObj = _TeensyDataObj;
            serialPort = null;
            //criar uma instância default do port serial
            createDefaultComPort();

            //Adicionar um handler para manipulador de eventos 'DataReceived' 
            //(indica que uma certa quantidade de bytes foi recebida.
            //Atenção: O evento é gerado quando a serial receber PELO MENOS a quantidade de 
            //      bytes definida na propriedade ReceivedBytesThreshold do port.
            //      Nesta aplicação aquele limiar é setado para a mínima quantidade de bytes
            //      que deve ser enviada em cada transação. Aqui a propriedade ReceivedBytesThreshold
            //      do port é setada para a quantidade de bytes do cabeçalho das mesnagens,
            //      conforme o protocolo de comunicação Teensy<->HOST - ver comentários acima). 
            //      Desta forma, o evento DataReceived será disparado quando chegarem,
            //      PELO MENOS, a quantidade de bytes definida em serialPort.ReceivedBytesThreshold.
            //      Por fim: the DataReceived event is also raised if an Eof character 
            //      is received, regardless of the number of bytes in the internal input 
            //      buffer and the value of the ReceivedBytesThreshold property.
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort_DataReceived);

        }

        //Criar uma instância de um portSerial com as características default
        //definidas em MipUsbComConstants
        private void createDefaultComPort()
        {
            if (serialPort != null)
                return;

                //criar uma instancia da classe portSerial 
                serialPort = new System.IO.Ports.SerialPort();
                //ajustar propriedades default do port
                serialPort.PortName = TeensyUsbComConstants.PortName;
                serialPort.BaudRate = TeensyUsbComConstants.BaudRate;
                serialPort.DataBits = TeensyUsbComConstants.DataBits;
                serialPort.StopBits = TeensyUsbComConstants.StopBits;
                serialPort.DtrEnable = TeensyUsbComConstants.DtrEnable;
                serialPort.RtsEnable = TeensyUsbComConstants.RtsEnable;
                serialPort.Handshake = TeensyUsbComConstants.Handshake;
                serialPort.Parity = TeensyUsbComConstants.Parity;
                serialPort.ParityReplace = TeensyUsbComConstants.ParityReplace;
                serialPort.ReadBufferSize = TeensyUsbComConstants.ReadBufferSize;
                serialPort.ReadTimeout = TeensyUsbComConstants.ReadTimeOut;
                serialPort.ReceivedBytesThreshold = TeensyUsbComConstants.ReceivedBytesThreshold;
                serialPort.WriteBufferSize = TeensyUsbComConstants.WriteBufferSize;
                serialPort.WriteTimeout = TeensyUsbComConstants.WriteTimeout;
                serialPort.Encoding = Encoding.GetEncoding(TeensyUsbComConstants.Encoding);
                serialPort.NewLine = TeensyUsbComConstants.StringEndMsg;
        }

        #endregion

        //Get/set propriedades do objeto
        #region Properties

        //get/set Nome do port Com
        public string thePortName
        {
            get { return serialPort.PortName; }
            set
            {
                //Se PortName "" => Gera ArgumentException: The PortName property was set to a value with a length of zero.
                if (value != "" && value !=null)
                    serialPort.PortName = value;
            }
        }

        //get/set Baud rate
        public int theBaudRate
        {
            get { return serialPort.BaudRate; }
            set { serialPort.BaudRate = value; }
        }

        //Get String para identificar a conexão deste port
        public string ConnectionString
        {
            get
            {
                return String.Format("[Serial] Port: {0} | Baudrate: {1}",
                                     serialPort.PortName, serialPort.BaudRate.ToString());
            }
        }

        #endregion

        #region Methods

        #region Port Control

        //Abrir port de comunicação entre Host e Teensy.
        //Para tal é feita uma busca no sistema para localizar o Port Com com a 
        //descrição do Por USB do Teensy.
        //Retorna true se conseguiu abrir o port corretamente
        public bool OpenConnTeensy()
        {
            if (serialPort == null)
                return false;

            //Localizar as portas com ativadas no sistema.
            string pName = GetTeensySerialPort();

            //Encontrou o PortCOM do MIP?
            if (pName == "")
                return false; //não

            //ok - abrir port
            thePortName = pName;
            serialPort.PortName = pName;
            return OpenConn();
        }

        public bool OpenConnTeensy(string pName)
        {
            if (serialPort == null)
                return false;

            //Localizar as portas com ativadas no sistema.

            //Encontrou o PortCOM do MIP?
            if (pName == "")
                return false; //não

            //ok - abrir port
            thePortName = pName;
            serialPort.PortName = pName;
            return OpenConn();
        }

        //Varre o sistema à procura do Port Com com a descrição do canal
        //USB CDC do Teensy.
        //Se encontrar retorna o nome do Port ("COMx"), caso contrário retorna um
        //string vazio (""). MAS, cuidado, serial.PortName Não deve instanciada
        //como "" senão => Gera ArgumentException: The PortName property was set to a value with a length of zero.
        private string GetTeensySerialPort()
        {
            try
            {
                var query = new ManagementObjectSearcher("root\\CIMV2",
                                                         "SELECT * FROM Win32_PnPEntity");

                query.Options.Timeout = new TimeSpan(0, 0, 1); //timeout => TimeSpan(horas, minutos, segundos);
                query.Options.ReturnImmediately = false;

                //log.Info("Query built");
                foreach (ManagementObject obj in query.Get())
                {
                    using (obj)
                    {
                        // var key = (uint)obj.GetPropertyValue("IDProcess");
                        if (obj["Caption"].ToString().Contains(TeensyUsbComConstants.Teensy_PortDescription))
                        {
                            string portDescription = obj["Caption"].ToString();
                            //Se encontrou, queryObj["Caption"] retorna um string com o friendly name do
                            //port COM. Algo do tipo "Teensy (COMx)"
                            //     COMx é o port com localizado x = 1, 2, 3...
                            //Assim, bastaria apenas separar o string (usando separadores ( e ) e pegar o 
                            //segundo token. MAS, se o usuário inserir a palavra COM ou outra entre parêntesis
                            //na descrição, esta estratégia iria falhar.
                            //Assim, a estratégia que usarei será, varrer TODOS os nomes de PORT COM
                            //disponíveis no sistema e verifica aquele que possui o nome
                            //como parte do Friendly name do port.

                            //GetPortNames retorna um array com os nomes de todos os ports com no computador.
                            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                            {
                                if (portDescription.Contains(s))
                                {
                                    return s;
                                }
                            }
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                return "";
            }
            return "";
        }

        //Abrir port Com.
            //Retorna true se conseguiu abrir o port corretamente
        private bool OpenConn()
            {
                try
                {
                    if (serialPort == null)
                        return false;

                    if (!serialPort.IsOpen)
                    {
                        serialPort.Open(); //abrir port
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }

        //Fechar port Com
        public void CloseConn()
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    //Fechar o port Com
                    serialPort.Close();
                }
            }

        //reset port com
        public bool ResetConn()
            {
                CloseConn();
                return OpenConn();
            }

        //Flush serial input buffer
        public void flushSerialInputBuffer()
        {
            if (serialPort.IsOpen)
            {
                while(serialPort.BytesToRead > 0)
                {
                    serialPort.ReadByte();
                }
            }
        }

        #endregion

        #region Transmit

        //transmite o conteúdo do vetor de bytes packet para a serial
        public void TransmitBytes(byte[] packet, int nBytes)
            {
                if (serialPort.IsOpen)
                {
                    //Atenção - Writes don’t usually block threads, AS LONG THERE IS ROOM in 
                    //the KERNEL BUFFER (hardware buffer). If there isn’t room in the kernel 
                    //buffer, it means you’re writing large amounts of data at a higher rate 
                    //than the port can drain it — but you configured the port baud rate. 
                    //If you’re worried about overfilling the buffer, use WriteAsync.
                    serialPort.Write(packet, 0, nBytes);
                }
            }

        //transmite a mensagem (string - exatamente como recebido) para a serial.
        //ATENÇÃO a mensagem deve ser um string codificado em "us-ascii" e terminado
        //com newLine '\0' - conforme definido nas propriedades do port serial.
        public void TransmitMsg(string msg)
            {
                if (serialPort.IsOpen)
                {
                    //Atenção - Writes don’t need a separate thread, because they don’t block 
                    //AS LONG THERE IS ROOM in the KERNEL BUFFER (hardware buffer).
                    //If there isn’t room in the kernel buffer, it means you’re writing large 
                    //amounts of data at a higher rate than the port can drain it — but you 
                    //configured the port baud rate. 
                    //If you’re worried about overfilling the buffer, use WriteAsync rather 
                    //than using a separate thread.

                    //MAIS: take care when using WriteLine, because it accepts a string rather 
                    //than a byte array, and THERE'S NO STANDARD LINE TERMINATION for all serial 
                    //devices. Be careful to SET YOUR encoding and the NewLine PROPERTIES.
                    //Nesta aplicação estou usando Encoding = "us-ascii" e StringEndMsg = "\0" nas
                    //mensagens a serem enviadas e recebidas, bem como assim são setadas aquelas
                    //propriedades do port serial (ver MipUsbComConstants).

                    serialPort.WriteLine(msg); //INCLUI newLine ao final !!!!
                }
        }

        #endregion

        #region serialport DataReceived event handler

        // O evento é gerado quando a serial receber PELO MENOS a quantidade de 
        //  bytes definida na propriedade ReceivedBytesThreshold do port.
        // Nesta aplicação aquele limiar é setado para a mínima quantidade de bytes
        //  que deve ser enviada em cada transação.
        // DataReceived event is also raised if an Eof character 
        //  is received, regardless of the number of bytes in the internal input 
        //  buffer and the value of the ReceivedBytesThreshold property.
        //
        // Os dados recebidos serão armazenados num buffer circular do objeto 
        //  teensyDataObj para tratamento futuro.
        //
        //NOTA: The DataReceived event is raised on a secondary thread when data is 
        //received from the SerialPort object.
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bytesRead;
            byte[] dataSerial;

            //Garantir que este evento foi gerado pelo port do Teensy que estamos usando.
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            if (sp.PortName != serialPort.PortName)
                return;

            //o buffer temporário deve ser capaz de armazenar todos os byter no receiving buffer da serial
            dataSerial = new byte[serialPort.ReadBufferSize];

            //buffer para conter dados sendo recebidos pela USB até a chegada completa do pacote.
            bytesRead = sp.Read(dataSerial, 0, sp.BytesToRead);

            //transferir todos os bytes recebidos para o buffer circular de teensyDataObj
            for (int i = 0; i < bytesRead; i++)
                teensyDataObj.serialDataBC.write(dataSerial[i]);

        }

        #endregion

        #region IDisposable Methods

        public void Dispose()
            {
                CloseConn();

                if (serialPort != null)
                {
                    serialPort.Dispose();
                    serialPort = null;
                }
            }

        #endregion

        #endregion

        #region Custom Events (OnMessageReceived and OnSerialErrorReceiving), event args, delegate Invoke Functions

        //Não estou usando estes evantos, mas deixo aqui sua definição para, se precisar, usar no futuro).

            #region Custom Event OnMessageReceived

            //Custom Event - Uma mensagem ( | TCOM/TACK_x | payLoad | TPACK_END | ) foi recebida.
            //Este evento possui argumentos para identificar a mensagem recebida.
            // Um string com o NOME DO HEADER da mensagem recebida (OnMessageReceived(string)).
            public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

            //Argumentos (EventArgs) para os eventos OnMessageReceived (string _nomeTCOM) 
            public class MessageReceivedEventArgs : EventArgs
            {
                //string
                private string headerMsg;

                //constructor
                public MessageReceivedEventArgs(string _headerMsg)
                {
                    headerMsg = _headerMsg;
                }

                //properties
                public string HeaderMsg
                {
                    get { return headerMsg; }
                    private set { headerMsg = value; }
                }
            }

            //Raise Custom Event para indicar que uma mensagem (sequencia de Bytes ASCII
            //      finalizada com o caracter terminador de mensagens do PIC) foi recebida.
            //  Parâmetro: string com o NOME do comando que precede a mensagem.
            private void RaiseOnMessageReceivedEvent(string nomeCOM)
            {
            //var handler = this.OnMessageReceived;
            //if (handler != null)
            // {
            //     handler(this, new MessageReceivedEventArgs(nomeCOM));
            // }

            //Ou, simplificando, se OnMessageReceived foi criado, chamar delegate para gerar evento
            OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(nomeCOM));
            }

            #endregion

            #region Custom Event onSerialErrorReceiving

            //Custom Event - ocorreu um erro ao ler dados do port serial.
            public event EventHandler OnSerialErrorReceiving;

            //Raise Custom Event para indicar que um erro ocorreu na leitura de dados
            //      serial.
            private void RaiseOnSerialErrorReceiving()
            {
                var handler = this.OnSerialErrorReceiving;

                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }

                //OU, simplificando:  se onErrorReceiving foi criado, chamar delegate para gerar evento
                //onErrorReceiving?.Invoke(this, EventArgs.Empty);

            }

            #endregion

            #endregion

    } //class MipUsbComPort

}
