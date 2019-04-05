using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace SVIPT_BioLab
{
    //Constantes
    public static class sviptFileConstant
    {
        public const string sviptFileID = "SVIPT Data File";
        public const string samplingRate = "Sampling_Rate: ";
        public const string forceUnity = "Force_Unity: ";
        public const string baseLineForceVal = "BaseLineForce_Val: ";
        public const string maxVoluntForceVal = "MaxVoluntForce_Val: ";
        public const string maxForcePercentage = "MaxForce_Percentage: ";
        public const string minForcePercentage = "MinForce_Percentage: ";
        public const string taskMaxForcePercentage = "TaskMaxForce_Percentage: ";
        public const string sviptSession = "Session: ";

        public const string SessionAccuracy = "Accuracy: ";//Overshoot = 1, acerto = 0, undershoot = -1
        public const string TrialErrors = "ErrorPercent: ";
        public const string TrialTimes = "Times: ";
        public const string ConversionFunction = "ConversionFunction: ";

        public const string sviptTask = "Task: ";
        public const string taskRepetitions = "TaskRepetitions: ";
        public const string idVolunteer = "ID_Volunteer: ";
        public const string forceSignal = "ForceSignal: ";

        public const string oscillationPercent = "OscillationPercent: ";

        //Filename do arquivo contendo a última configuração utilizada.
        //Este arquivo deve estar no mesmo diretório do executável
        public const string fileNameConfig = "SVIPTConfig.cfg";
        public const string stringIDfileConfig = "SVIPT ConfigFile";
    }

    //Modificação: 21/03/2017
    //Autor: Andrei
    //Removi os espaços, pois a função "getStringItemFromConfigFile" quebra a string em tokens utilizando
    //espaço e tabulação como parâmetros. Desta forma, a tarefa não era lida corretamente do arquivo com a
    //string da linha sendo quebrada em várias ao invés de apenas duas. Assim, sempre que a janela
    //"Definir sessão" é aberta, ela marca a tarefa modificada e não a original, independente do conteúdo
    //do arquivo.
    //Original abaixo
    //public const string sviptTaskOriginal = "Original [Home - 1 - Home - 2 - Home - 3 - Home - 4 - Home - 5]";
    //public const string sviptTaskModifed = "Modified [Home - 1 - Home - 2 - Home - 3 - Home - 4 - Home - 5]";
    public static class sviptConstant
    {
        public const string sviptTaskOriginal = "Original[Home-1-Home-2-Home-3-Home-4-Home-5]";
        public const string sviptTaskModifed = "Modified[Home-1-Home-2-Home-3-Home-4-Home-5]";
        public const string sviptTaskModifed2 = "Modified2[Home-1-Home-2-Home-3-Home-4-Home-5]";
        public const string sviptTaskLinear = "Linear[Home-1-Home-2-Home-3-Home-4-Home-5]";
    }

    // Esta classe (SVIPTmanager) gerencia a sessão SVIPT.

    /* Nesta versão, as configurações e os dados da sessão serão armazenados em um arquivo texto.
    * Formato do arquivo:
    * 
    * ----------------------------------------------------------------
    * SVIPT File
    * Session: string                   //String identificador da sessão
    * ID_Volunteer: string              //identificador do voluntário      
    * Task: string      //String descritor da tarefa executada na sessão: 
    *                   // "Original [Home-1-Home-2-Home-3-Home-4-Home-5]" ou 
    *                   // "Modified [Home-1-Home-2-Home-3-Home-4-Home-5]"
    * TaskRepetitions: int              //quantidade de repetições da tarefa durante a sessão
    * Sampling_Rate: double            //taxa de amostragem para coleta de sinais de força
    * Force_Unity: string               //unidade usada para força
    * baseLineForceVal = double         //valor de força sem carga (na unidade Force_Unity)
    * maxVoluntForceVal = double        //valor de força máxima do voluntário (MVF - na unidade Force_Unity)
    * maxForcePercentage = int       //Percentual de força máxima que pode ser usado para 
    *                                   //atingir deslocamento máximo numa dada tarefa.
    * MinForce_Percentage = int       //Percentual de força mínima que pode ser usado para 
    *                                   //atingir deslocamento máximo numa dada tarefa.
    * sessionMaxForcePercentage = int    //Percentual de força máxima usado nas TAREFAS desta sessão.
    *                                       //Este valor ajusta a curva log.
    * ForceSignal:
    * ... Série temporal com os valores de força coletados.
    * ... Cada amostra será armazenada na unidade Force_Unity.
    * ... Dados serão armazenados em uma coluna.

    /* Classe para uma sessão SVIPT */

    public class Session
    {
        public string id { get; set; }          //Identificador desta sessão
        public string SVIPT_Task { get; set; }  //String descritor da tarefa executada na sessão.
                                                // "Original [Home-1-Home-2-Home-3-Home-4-Home-5]" ou 
                                                // "Modified [Home-1-Home-2-Home-3-Home-4-Home-5]"
        public string ID_Volunteer { get; set; }   //identificador do voluntário 
        public int SVIPT_TaskRepetitions { get; set; } //quantidade de repetições da tarefa durante a sessão
        public int SVIPT_TaskOscilationPercent { get; set; }//porcentagem de oscilacao aceitavel para nao marcar como erro
        public double Sampling_Rate { get; set; }  //taxa de amostragem para coleta de sinais de força
        public string Force_Unity { get; set; }    //unidade usada para força
        public double baseLineForceVal { get; set; } //valor de força sem carga (na unidade Force_Unity)
        public double maxForceVal { get; set; }    //valor de força máxima do voluntário (na unidade Force_Unity)
        public int maxForcePercentage { get; set; } //Percentual de força máxima que pode ser usado para 
                                                    //atingir deslocamento máximo numa dada tarefa.
        public int minForcePercentage { get; set; }  //Percentual de força mínima que pode ser usado para 
                                                     //atingir deslocamento máximo numa dada tarefa.
        public int taskMaxForcePercentage { get; set; } //Percentual de força máxima usado nas TAREFAS desta sessão.
                                                        //Este valor ajusta a curva log.
        public double A { get; set; } //valor para a curva Volts = A*kgf+B
        public double B { get; set; } //valor para a curva Volts = A*kgf+B

        public bool CurvaConvertSetada { get; set; }

        public List<double> data;                  //Lista com a série temporal dos valores de força coletados.
        public int ListWriteIndex = 0; //indice de aonde na lista estamos escrevendo

        public Queue<double> JanelaMM; //sinal bruto dos ultimos 20 pra media movel


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
        public Queue<int> [,]PosPixels;
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
        public Queue<string> EpochQueue;//s'o pra debuggar


        public int TamanhoJanela { get; }

        public bool saved { get; set; }  //indica se a sessão foi salva em arquivo

        public int numberOfTrials { get; } //numero de alvos em cada repeticao

        public double[,] times { get; set; } //tempo em segundos decorrido em cada uma das trials(j) de cada serie(i) times[i,j]

        public double[,] error { get; set; } // valor entre -1 e 1, significando quantos % houve de erro entre o final do movimento do cursor e seu alvo
                                             //ex: -0.5 para o alvo 3 (localisado em 60% da trajetoria) significaria que o movimento foi ate 10% da trajetoria e ja voltou para home
        public bool[,] undershoot { get; set; } //true na posicao [i,j] se na trial j da repeticao i houve undershoot

        public bool[,] overshoot { get; set; } //true na posicao [i,j] se na trial j da repeticao i houve overshoot

        public int[,] accuracy { get; set; }//-1=undershoot, 0=acerto, 1=overshoot

        public int[] ListaDeCurvas { get; set; } //mostra a sequencia de A's usada

        public bool ColorChangeEnabled { get; set; }

        //Variáveis necessárias para análise offline do experimento
        //Limites de movimento
        public int[] cursorLimits { get; set; }
        //Representação do cursor
        public PictureBox cursor { get; set; }
        //Representação do home
        public PictureBox home { get; set; }
        //Representação dos targets
        public PictureBox[,] targets { get; set; }

        public Session()
        {
            ColorChangeEnabled = true;
            CurvaConvertSetada = false;
            A = 0;
            B = 0;
            id = "";
            SVIPT_Task = sviptConstant.sviptTaskOriginal;
            ID_Volunteer = "";
            baseLineForceVal = 0.001;
            maxForceVal = 4.0;
            maxForcePercentage = 45;
            minForcePercentage = 35;
            SVIPT_TaskRepetitions = 30;
            numberOfTrials = 5;
            Sampling_Rate = -1; //ainda não definida
            Force_Unity = "Volts";
            data = new List<double>();
            JanelaMM = new Queue<double>();
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
            PosPixels = new Queue<int>[32,5];
            for (int i = 0; i < 32; i++)
                for (int j = 0; j < 5; j++)
                    PosPixels[i, j] = new Queue<int>();
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
            EpochQueue = new Queue<string>();
            TamanhoJanela = 20;
            for (int i = 0; i < TamanhoJanela; i++)
            {
                JanelaMM.Enqueue(0);
            }
            saved = true; //como não existem dados ainda, não é necessário salvar os "dados" da sessão.

            //Inicializa os limites do cursor
            this.cursorLimits = new int[2]; //Mínimo e máximo

            //Inicializa o objeto que representa o cursor
            this.cursor = new PictureBox();

            //Inicializa o objeto que representa a posição do home
            this.home = new PictureBox();

            //Cria o vetor de PictureBox contendo detalhes dos targets
            targets = new PictureBox[numberOfTrials, 2];
            for (int i = 0; i < numberOfTrials; i++)
            {
                targets[i, 0] = new PictureBox();
                targets[i, 1] = new PictureBox();
            }

            //Configura o valor máximo de força
            setTaskMaxForcePercentage();

            //Tentar carregar arquivo contendo a última configuração utilizada.
            //Se existir, as configurações iniciais desta sessão serão iguais às
            //últimas.
            loadConfig();

            //Configura o número de repetições
            setTaskRepetitions(SVIPT_TaskRepetitions);

        }

        //Autor: Andrei
        //Data: 22/03/2017
        //Configura os limites máximos que o cursor pode atingir na tela
        public void setCursorLimits(int[] _limits)
        {
            this.cursorLimits = _limits;
        }

        //Autor: Andrei
        //Data: 22/03/2017
        //Atualiza as propriedades dos targets
        //Cada picturebox contém as informações necessárias para recriar o experimento
        //para análises offline.
        public void setGUIobjects(PictureBox _cursor, PictureBox _home, PictureBox[,] _targets)
        {
            //Atualiza o cursor
            this.cursor = _cursor;
            //Atualiza a objeto home
            this.home = _home;
            //Atualiza os objetos dos targets
            this.targets = _targets;
        }

        //Autor: Andrei
        //Data: 21/03/2017
        //Define a quantidade de repetições da tarefa.
        //Esta variável é importante, pois determina o tamanho dos vetores
        //que armazenam o processamento dos dados
        public void setTaskRepetitions(int taskRepetitions)
        {
            SVIPT_TaskRepetitions = SVIPT_TaskRepetitions;
            times = new double[SVIPT_TaskRepetitions, numberOfTrials];
            error = new double[SVIPT_TaskRepetitions, numberOfTrials];
            undershoot = new bool[SVIPT_TaskRepetitions, numberOfTrials];
            overshoot = new bool[SVIPT_TaskRepetitions, numberOfTrials];
            accuracy = new int[SVIPT_TaskRepetitions, numberOfTrials];
            ListaDeCurvas = new int[SVIPT_TaskRepetitions];
        }

        //Limpa as variáveis que armazenam os resultados das repetições
        public void clearMeasures()
        {
            times = new double[SVIPT_TaskRepetitions, numberOfTrials];
            error = new double[SVIPT_TaskRepetitions, numberOfTrials];
            undershoot = new bool[SVIPT_TaskRepetitions, numberOfTrials];
            overshoot = new bool[SVIPT_TaskRepetitions, numberOfTrials];
            accuracy = new int[SVIPT_TaskRepetitions, numberOfTrials];
        }

        //Definir o percentual de força máximo a ser usado na tarefa desta sessão, dependendo
        //do tipo de tarefa selecionado, e dentro dos limites definidos em
        //MinForce_Percentage e MaxForce_Percentage
        public void setTaskMaxForcePercentage()
        {
            //if (SVIPT_Task == sviptConstant.sviptTaskOriginal)
            //{
                //Tipo de tarefa - Original.
                //Limite de força da tarefa igual à metade dos limites impostos na fase de calibração.
                taskMaxForcePercentage = minForcePercentage + ((maxForcePercentage - minForcePercentage) / 2);
            /*}
            else
            {
                //Tipo de tarefa - Modified.
                //Limite de força da tarefa definido aleatoriamente entre os limites impostos na fase de calibração.
                //Para evitar ficar muito similar à original, vou definir uma distância mínima
                //da tarefa original. Ou seja, o limite de força aleatório será definido 
                //aleatóriamente, mas deve ter uma folga em relação ao nivel "original" de 
                //+-15% do range de força limite definido.
                int minDist = (int) (0.15 * (maxForcePercentage - minForcePercentage)); 
                //nivel médio (original)
                int original = minForcePercentage + ((maxForcePercentage - minForcePercentage) / 2);
                //gerar % aleatorio entre minForcePercentage e maxForcePercentage,
                //mas fora da faixa original +- minDist
                Random rnd = new Random();
                //Random.Next retorna um inteiro com sinal de 32 bits maior ou igual a minValue 
                //e menor que maxValue.
                int randVal1 = rnd.Next(minForcePercentage, (original - minDist) + 1); //na faixa inferior
                int randVal2 = rnd.Next((original + minDist), maxForcePercentage + 1); //na faixa superior
                //randVal1 contém um percentual aleatório na faixa superior (meio - maximo) dos limites 
                //de força, e randVal2 contém um valor na faixa inferior.
                //Aleatoriamente, pegamos um deles.
                if (rnd.Next(-100, 101) >= 0)
                    taskMaxForcePercentage = randVal1;
                else
                    taskMaxForcePercentage = randVal2;
            }*/
        }

        //Salvar dados da sessão em arquivo
        public void saveDataFile()
        {
            string fn;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            StreamWriter writer;

            //todos os doubles serão armezanados no arquivo txt com 6 casas decimais.

            //      string exePath = Application.ExecutablePath; //diretório do executável
            //      fnConfig = Path.GetDirectoryName(exePath) + "\\" +
            //                       bcFileConstant.fileNameConfig;

            /*SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            //saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            //ok
            fn = saveFileDialog1.FileName;*/

            if (MessageBox.Show("Deseja salvar os dados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                string exePath = Application.ExecutablePath; //diretório do executável
                string fnConfig = Path.GetDirectoryName(exePath) + "\\";
                fn = fnConfig + this.ID_Volunteer + "_" + this.id + ".txt";

                using (writer = new StreamWriter(@fn))
                {
                    // Write the contents.
                    //String identificador de arquivo SVIPT
                    writer.Write(sviptFileConstant.sviptFileID);
                    //String identificador da sessão
                    writer.Write("\r\n" + sviptFileConstant.sviptSession + id);
                    //identificador do voluntário
                    writer.Write("\r\n" + sviptFileConstant.idVolunteer + ID_Volunteer);
                    //String descritor da tarefa executada na sessão. 
                    writer.Write("\r\n" + sviptFileConstant.sviptTask + SVIPT_Task);
                    //quantidade de repetições da tarefa durante a sessão
                    writer.Write("\r\n" + sviptFileConstant.taskRepetitions + SVIPT_TaskRepetitions.ToString());
                    //taxa de amostragem para coleta de sinais de força
                    writer.Write("\r\n" + sviptFileConstant.samplingRate + Sampling_Rate.ToString("n6"));
                    //unidade usada para força
                    writer.Write("\r\n" + sviptFileConstant.forceUnity + Force_Unity);
                    //valor de força sem carga (na unidade Force_Unity)
                    writer.Write("\r\n" + sviptFileConstant.baseLineForceVal + baseLineForceVal.ToString("n6"));
                    //valor de força máxima do voluntário (MVF - na unidade Force_Unity)
                    writer.Write("\r\n" + sviptFileConstant.maxVoluntForceVal + maxForceVal.ToString("n6"));
                    //pontos A e B que definem a cuva da celula de carga do tipo A*X(Volts) + B = Y(Kgf)
                    writer.Write("\r\n" + sviptFileConstant.ConversionFunction + A.ToString("n6") + "X +" + B.ToString("n6") + " = Y");
                    //Percentual de força máxima que pode ser usado para  deslocamento máximo numa dada tarefa.
                    writer.Write("\r\n" + sviptFileConstant.maxForcePercentage + maxForcePercentage.ToString());
                    //Percentual de força mínima que pode ser usado para  deslocamento máximo numa dada tarefa.
                    writer.Write("\r\n" + sviptFileConstant.minForcePercentage + minForcePercentage.ToString());
                    //Percentual de força máxima usado nas TAREFAS desta sessão.
                    writer.Write("\r\n" + sviptFileConstant.taskMaxForcePercentage + taskMaxForcePercentage.ToString());

                    //Descrevendo os limites de movimento
                    writer.Write("\r\nLimites de movimento do cursor: " + this.cursorLimits[0].ToString() + "\t" + this.cursorLimits[1].ToString());

                    //Descrevendo o objeto do cursor
                    writer.Write("\r\nCursor");
                    writer.Write("\r\nLocation: " + this.cursor.Location.X.ToString() + "\t" + this.cursor.Location.Y.ToString());
                    writer.Write("\r\nSize: " + this.cursor.Size.Width.ToString() + "\t" + this.cursor.Size.Height.ToString());

                    //Descrevendo a posição Home
                    writer.Write("\r\nHome");
                    writer.Write("\r\nLocation: " + this.home.Location.X.ToString() + "\t" + this.home.Location.Y.ToString());
                    writer.Write("\r\nSize: " + this.home.Size.Width.ToString() + "\t" + this.home.Size.Height.ToString());

                    //Detalhes dos targets
                    for (int i = 0; i < numberOfTrials; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //Como o target 5 não possui limites, ele não é especificado por duas
                            //picturebox, portanto, a última iteração deste laço não deve ser 
                            //realizada
                            if (i == numberOfTrials - 1 & j == 1)
                                break;

                            //Escreve o identificador do target
                            //Ex: Target 1 - Left --> indicando os detalhes do limite esquerdo do target 1
                            string title = "Target " + (i + 1).ToString();
                            if (j == 0 && i < numberOfTrials - 1)
                                title += " - Left";
                            else if ((j == 1 && i < numberOfTrials - 1))
                                title += " - Right";

                            //Escreve o identificador do target
                            writer.Write("\r\n" + title);
                            //Escrevendo os detalhes necessários para redesenhar o target
                            //Localização
                            writer.Write("\r\nLocation: " + this.targets[i, j].Location.X.ToString() + "\t" + this.targets[i, j].Location.Y.ToString());
                            //Tamanho
                            writer.Write("\r\nSize: " + this.targets[i, j].Size.Width.ToString() + "\t" + this.targets[i, j].Size.Height.ToString());
                        }
                    }
                    //Ordem das curvas A'(1,2,3,4,5,6), caso seja sessao modificada
                    if(SVIPT_Task == sviptConstant.sviptTaskModifed || SVIPT_Task == sviptConstant.sviptTaskModifed2)
                    {
                        writer.Write("\r\nCurvas Usadas: ");
                        for(int i=0;i<SVIPT_TaskRepetitions;i++)
                        {
                            writer.Write("\r\n"+ ListaDeCurvas[i].ToString());
                        }
                    }


                    //Série temporal com os valores de força coletados.
                    writer.Write("\r\n" + sviptFileConstant.forceSignal);
                    // loop over all data values
                    foreach (double val in data)
                    {
                        writer.Write("\r\n");
                        writer.Write(val.ToString("n6"));
                        //writer.Write(val.ToString().PadLeft(4, '0'));//writer.Write(val.ToString("n6"));
                        //tava mostrando 6 digitos decimais, entretando, como estou salvando o valor de 0-1023, nao ha necessidade de decimais
                        // agora esta mostrando sempre 4 digitos. 50 -> 0050, 1011 -> 1011
                    }


                    //Serie dos numsamples e epoch40
                    writer.Write("\r\n" + "NumSamples tab Epoch40");
                    // loop over all data values
                    foreach (string val in EpochQueue)
                    {
                        writer.Write("\r\n");
                        writer.Write(val);
                        //writer.Write(val.ToString().PadLeft(4, '0'));//writer.Write(val.ToString("n6"));
                        //tava mostrando 6 digitos decimais, entretando, como estou salvando o valor de 0-1023, nao ha necessidade de decimais
                        // agora esta mostrando sempre 4 digitos. 50 -> 0050, 1011 -> 1011
                    }

                    writer.Write("\r\n" + "Localizacao:");
                    int auxili = 0;
                        for (int i = 0; i < SVIPT_TaskRepetitions; i++)
                            for (int j = 0; j < numberOfTrials; j++)
                                while (PosPixels[i, j].Count > 0)
                                {// loop over all data values
                                    writer.Write("\r\n");
                                    auxili = PosPixels[i, j].Dequeue();
                                    writer.Write( i.ToString() + "\t" + j.ToString() + "\t" + auxili.ToString("D"));

                                }


                    //Close writer
                    writer.Close();

                    //Autor: Andrei
                    //22/03/2017
                    //Modificação: Criando um novo arquivo para armazenar os resultados
                    //O nome do arquivo é igual ao nome do arquivo de dados seguido de "_results"               
                    string[] aux = fn.Split('.');
                    writer = new StreamWriter(aux[0] + "_results.txt");
                    //Sequencia informando se houve undershoot ou overshoot nas trials.
                    writer.Write(sviptFileConstant.SessionAccuracy);
                    for (int i = 0; i < SVIPT_TaskRepetitions; i++)
                        for (int j = 0; j < numberOfTrials; j++)
                        {//loop pra montar o array com overshoots, acertos e undershoots
                            if (overshoot[i, j])
                                accuracy[i, j] = 1;
                            else if (undershoot[i, j])
                                accuracy[i, j] = -1;
                            else
                                accuracy[i, j] = 0;
                        }
                    //Salvando a acurácia em cada tentativa identificando qual era o target
                    int targetId = 1;
                    foreach (int val in accuracy)
                    {// loop over all data values
                        writer.Write("\r\n");
                        writer.Write(targetId.ToString() + "\t" + val.ToString("D"));
                        targetId++;
                        if (targetId > numberOfTrials)
                            targetId = 1;
                    }

                    targetId = 1;
                    writer.Write("\r\n" + sviptFileConstant.TrialErrors);
                    foreach (double val in error)
                    {// loop over all data values
                        writer.Write("\r\n");
                        writer.Write(targetId.ToString() + "\t" + val.ToString("n6"));
                        targetId++;
                        if (targetId > numberOfTrials)
                            targetId = 1;
                    }

                    //Necessário verificar a melhor forma de calcular o tempo
                    //de duração, pois não sei se o StopWatch é a melhor opção
                    //considerando o uso de multi-threading
                    /*
                    writer.Write("\r\n" + sviptFileConstant.TrialTimes);
                    foreach (long val in times)
                    {// loop over all data values
                        writer.Write("\r\n");
                        writer.Write(val.ToString().PadLeft(5, '0'));
                    }
                    */
                    writer.Close();
                }
            }
        }

        //Armazena as configurações da sessão em um arquivo - na próxima sessão, esta
        //configuração será usada como default/inicial (sobrescreve o anterior).
        public void saveConfig()
        {
            // Create new file and open it for read and write, if the file exists overwrite it.

            //todos os doubles serão armezanados no arquivo txt com 6 casas decimais.

            string fnConfig;

            string exePath = Application.ExecutablePath; //diretório do executável
            fnConfig = Path.GetDirectoryName(exePath) + "\\" +
                             sviptFileConstant.fileNameConfig;

            using (StreamWriter writer = new StreamWriter(@fnConfig))
            {
                // Write the contents

                //String identificador de arquivo de configuração
                writer.Write(sviptFileConstant.stringIDfileConfig);
                //String descritor da tarefa executada na sessão. 
                writer.Write("\r\n" + sviptFileConstant.sviptTask + SVIPT_Task);
                //quantidade de repetições da tarefa durante a sessão
                writer.Write("\r\n" + sviptFileConstant.taskRepetitions + SVIPT_TaskRepetitions.ToString());
                //taxa de amostragem para coleta de sinais de força
                //writer.Write("\r\n" + sviptFileConstant.samplingRate + Sampling_Rate.ToString("n6"));
                //unidade usada para força
                writer.Write("\r\n" + sviptFileConstant.forceUnity + Force_Unity);
                //valor de força sem carga (na unidade Force_Unity)
                writer.Write("\r\n" + sviptFileConstant.baseLineForceVal + baseLineForceVal.ToString("n6"));
                //valor de força máxima do voluntário (MVF - na unidade Force_Unity)
                writer.Write("\r\n" + sviptFileConstant.maxVoluntForceVal + maxForceVal.ToString("n6"));
                //Percentual de força máxima que pode ser usado para  deslocamento máximo numa dada tarefa.
                writer.Write("\r\n" + sviptFileConstant.maxForcePercentage + maxForcePercentage.ToString());
                //Percentual de força mínima que pode ser usado para  deslocamento máximo numa dada tarefa.
                writer.Write("\r\n" + sviptFileConstant.minForcePercentage + minForcePercentage.ToString());
                //Percentual de força máxima usado nas TAREFAS desta sessão.
                writer.Write("\r\n" + sviptFileConstant.taskMaxForcePercentage + taskMaxForcePercentage.ToString());
                //Percentual de oscilação máximo aceitável para determinar undershoot
                writer.Write("\r\n" + sviptFileConstant.oscillationPercent + SVIPT_TaskOscilationPercent.ToString());

                //Close writer
                writer.Close();
            }
        }

        //Carrega a configuração da última sessão realizada - será usada como configuração
        //inicial desta sessão.
        //Sempre executa: Se o arquivo fileNameConfig não existir as listas estarão vazias.
        //                fileNameConfig deve estar no mesmo diretório do executável
        public void loadConfig()
        {
            char[] delimiters = new char[] { '\t', ' ' };
            string fnConfig;

            string exePath = Application.ExecutablePath; //diretório do executável
            fnConfig = Path.GetDirectoryName(exePath) + "\\" +
                             sviptFileConstant.fileNameConfig;

            //o Arquivo configuração existe?
            if (!File.Exists(fnConfig))
            {
                //Se não existir deve apenas zerar a lista de dados
                data.Clear();
                return;
            }

            // I am using the FileStream class to open the file and the StreamReader class to read from the file.

            // create a FileStream:
            FileStream file = new FileStream(@fnConfig, FileMode.Open, FileAccess.ReadWrite);
            //Wrap the FileStream in a StreamReader:
            StreamReader reader = new StreamReader(file);

            //Read file
            //The first Line must contain the string stringIDfileConfig
            string currentLine = reader.ReadLine();
            if (!currentLine.Equals(sviptFileConstant.stringIDfileConfig))
            {
                //arquivo não contém configurações válidas --- ele será sobrescrito quando sair (ver SaveConfig)
                data.Clear();
                return;
            }

            //String descritor da tarefa executada na sessão. 
            SVIPT_Task = getStringItemFromConfigFile(reader, sviptFileConstant.sviptTask);
            //quantidade de repetições da tarefa durante a sessão
            SVIPT_TaskRepetitions = getIntItemFromConfigFile(reader, sviptFileConstant.taskRepetitions);
            //taxa de amostragem para coleta de sinais de força
            //Sampling_Rate = getDoubleItemFromConfigFile(reader, sviptFileConstant.samplingRate);
            //unidade usada para força
            Force_Unity = getStringItemFromConfigFile(reader, sviptFileConstant.forceUnity);
            //valor de força sem carga (na unidade Force_Unity)
            baseLineForceVal = getDoubleItemFromConfigFile(reader, sviptFileConstant.baseLineForceVal);
            //valor de força máxima do voluntário (MVF - na unidade Force_Unity)
            maxForceVal = getDoubleItemFromConfigFile(reader, sviptFileConstant.maxVoluntForceVal);
            //Percentual de força máxima que pode ser usado para  deslocamento máximo numa dada tarefa.
            maxForcePercentage = getIntItemFromConfigFile(reader, sviptFileConstant.maxForcePercentage);
            //Percentual de força mínima que pode ser usado para  deslocamento máximo numa dada tarefa.
            minForcePercentage = getIntItemFromConfigFile(reader, sviptFileConstant.minForcePercentage);
            //Percentual de força máxima usado nas TAREFAS desta sessão.
            taskMaxForcePercentage = getIntItemFromConfigFile(reader, sviptFileConstant.taskMaxForcePercentage);
            //Oscilação máxima permitida para determinar undershoot
            SVIPT_TaskOscilationPercent = getIntItemFromConfigFile(reader, sviptFileConstant.oscillationPercent);

            //File will be closed during operation
            reader.Close();
            file.Close();
        }

        //Lê a próxima linha do streamfile de LEITURA, e retorna o valor string
        //associado ao item solicitado - se o tem não estiver na próxima linha,
        //mostra msg de erro e retorna "".
        private string getStringItemFromConfigFile(StreamReader reader, string item)
        {
            string line;
            char[] delimiters = new char[] { '\t', ' ' };
            //lê próxima linha do streamer de leitura
            line = reader.ReadLine();
            //separar os strings
            string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //o primeiro token deve ser o item buscado
            if (!item.ToLower().Trim().Equals(parts[0].ToLower()))
            {
                MessageBox.Show("Item " + item + " não localizado no arquivo de configuração.",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            return parts[1];
        }

        //Lê a próxima linha do streamfile de LEITURA, e retorna o valor inteiro
        //associado ao item solicitado - se o tem não estiver na próxima linha,
        //mostra msg de erro e retorna 0;
        private int getIntItemFromConfigFile(StreamReader reader, string item)
        {
            string line;
            char[] delimiters = new char[] { '\t', ' ' };
            //lê próxima linha do streamer de leitura
            line = reader.ReadLine();
            //separar os strings
            string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //o primeiro token deve ser o item buscado
            if (!(item.ToLower().Trim().Equals(parts[0].ToLower())))
            {
                MessageBox.Show("Item " + item + " não localizado no arquivo de configuração.",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            return Convert.ToInt16(parts[1]);
        }

        //Lê a próxima linha do streamfile de LEITURA, e retorna o valor inteiro
        //associado ao item solicitado - se o tem não estiver na próxima linha,
        //mostra msg de erro e retorna 0;
        private double getDoubleItemFromConfigFile(StreamReader reader, string item)
        {
            string line;
            char[] delimiters = new char[] { '\t', ' ' };
            //lê próxima linha do streamer de leitura
            line = reader.ReadLine();
            //separar os strings
            string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //o primeiro token deve ser o item buscado
            if (!item.ToLower().Trim().Equals(parts[0].ToLower()))
            {
                MessageBox.Show("Item " + item + " não localizado no arquivo de configuração.",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            double res5;
            try
            {
                res5 = Double.Parse(parts[1], System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                res5 = 0;
            }
            return res5;
        }
    }
}
