using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Biolab_DataAcquisition;

namespace SVIPT_BioLab
{
    public partial class Analisar : Form
    {
        private Session SessionFromFile;
        OpenFileDialog openFile;
        private Series chartSeries;
        private ChartArea chartArea;

        private Series SeriesB4;
        private ChartArea AreaB4;

        private Series SeriesB6;
        private ChartArea AreaB6;

        private Series SeriesB8;
        private ChartArea AreaB8;

        private Series SeriesB10;
        private ChartArea AreaB10;

        private Series SeriesB12;
        private ChartArea AreaB12;

        private List<double> cursorMotion;
        private int CursorWidth = 0;//usado na analize pro calculo dos intervalos de tempo
        private double CursorLeftLocationAtHome = 0;//tbm usado na analize
        private int[] RightSeqsPerBlock;
        private double[] ErrorRatePerBlock;
        private double[ , , ] ErrorSizeWholeExperiment;
        private double[ , , ] TimesWholeExperiment;
        private int[ , , ] AccuracyWholeExperiment;

        private Queue<int>[ , , ] pixelsEachTrial;

        private double[] SkillParameterPerBlock;
        private int[] OKsPerBlock;
        private int[] UndersPerBlock;
        private int[] OversPerBlock;
        private double[] TimesPerBlock;
        private double [,] TimesOfSequencePerBlock;
        private bool TemReativacao;
        private int NBlocos;
        //Limites dos alvos
        int[] leftLimits;
        int[] rightLimits;
        int[] widths;
        int larguraEmPixels;

        public Analisar()
        {            
            InitializeComponent();
            cursorMotion = new List<double>();

            

            //Configurar o chart
            analysisChart.Series.Clear();
            analysisChart.ChartAreas.Clear();
            chartSeries = new Series("SkillParameter");
            chartSeries.ChartType = SeriesChartType.Line;
            chartSeries.BorderWidth = 3;
            analysisChart.Series.Add(chartSeries);
            //Cria uma nova chartarea
            chartArea = new ChartArea("areaSkillParameter");
            analysisChart.ChartAreas.Add(chartArea);
            NBlocos = 12;
            TemReativacao = false;

            SessionFromFile = new Session();
            //Usando a caixa de diálogo "OpenFileDialog" para selecionar o arquivo a ser analisado

            // inicializando o array dos target
            //Limites à esquerda
            leftLimits = new int[SessionFromFile.numberOfTrials];
            //Limites à direita
            rightLimits = new int[SessionFromFile.numberOfTrials];
            widths = new int[SessionFromFile.numberOfTrials];

            openFile = new OpenFileDialog();
            openFile.RestoreDirectory = false;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("A Sessao a ser carregada tem reativacao?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TemReativacao = true;
                    NBlocos++;
                }
                ErrorSizeWholeExperiment = new double[NBlocos, 32, 5];//5 alvos  32 sequencias  12blocos+1reativacao -> 5x32x12=1920
                TimesWholeExperiment = new double[NBlocos, 32, 5];
                AccuracyWholeExperiment = new int[NBlocos, 32, 5];

                pixelsEachTrial = new Queue<int>[NBlocos, 32, 5];

                SkillParameterPerBlock = new double[NBlocos];//12blocos ou 12+reativacao
                RightSeqsPerBlock = new int[NBlocos];
                ErrorRatePerBlock = new double[NBlocos];
                OKsPerBlock = new int[NBlocos];
                UndersPerBlock = new int[NBlocos];
                OversPerBlock = new int[NBlocos];
                TimesPerBlock = new double[NBlocos];
                TimesOfSequencePerBlock = new double[NBlocos, 32];
                CarregarDoArquivo(openFile.FileName);
                CalcularSkillParameter();
                GraficoSkillParameter();
            }
            else
                MessageBox.Show("Nenhum arquivo carregado...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        private void CarregarDoArquivo(string FileName)
        {
            string StrAux;
            string FileName2;
            string[] AuxArr;
            for (int NumDoBloco = 0; NumDoBloco < NBlocos; NumDoBloco++)
            {
                //Acabou o primeiro Bloco.
                //Agora vamos abrir o proximo bloco, sabendo que
                //o nome deles eh sempre arqv1: BLABLABLA_diaX_bY arquiv2: BLABLABLA_diaX_bY_results
                // para o proximo bloco  arqv3: BLABLABLA_diaX_b(Y+1) arquiv4: BLABLABLA_diaX_b(Y+1)_results
                // e se for o bloco 9, ai precisamos incrementar o X tambem


                //StrAux = FileName.Substring(FileName.Length - 6, FileName.Length - 4);
                if (NumDoBloco < 8)
                {
                    FileName = FileName.Substring(0, FileName.Length - 5);
                    FileName = FileName + Convert.ToString(NumDoBloco + 1) + ".txt";
                }
                else if (NumDoBloco == 8)
                {
                    FileName = FileName.Substring(0, FileName.Length - 5);
                    StrAux = FileName.Substring(FileName.Length-1);
                    FileName = FileName.Substring(0, FileName.Length - 3);
                    FileName = FileName + "2_" + StrAux + "9.txt";
                }
                else if (NumDoBloco == 9)
                {
                    FileName = FileName.Substring(0, FileName.Length - 5);
                    FileName = FileName + Convert.ToString(NumDoBloco + 1) + ".txt";
                }
                else if (NumDoBloco<NBlocos-1 || !TemReativacao)//se nao tiver reativacao, esse eh o ultimo else. se tiver reativacao esse vai ate o penultimo bloco (o ultimo vai ser a reativacao)
                {
                    FileName = FileName.Substring(0, FileName.Length - 6);
                    FileName = FileName + Convert.ToString(NumDoBloco + 1) + ".txt";
                }
                else
                {
                    FileName = FileName.Substring(0, FileName.Length - 12);
                    FileName = FileName + "Reativação.txt";
                }

                using (StreamReader sr = new StreamReader(FileName, Encoding.Default))
                {
                    StrAux = sr.ReadLine();
                    if (StrAux != "SVIPT Data File")
                    {//Verifica se a primeira linha eh a linha que todo arquivo gerado por esse programa tem como sua primeira linha
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }

                    //Seguindo a ordem, agora devemos ter: "Session: SessionName"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "Session" + " SessionName"
                    if (AuxArr[0] != "Session")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.id = AuxArr[1].Substring(1);//remove o espaco que ficou antes da palavra em questao

                    //Na sequencia, agora vem: "ID_Volunteer: VolunteerID"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "ID_Volunteer" + " VolunteerID"
                    if (AuxArr[0] != "ID_Volunteer")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.ID_Volunteer = AuxArr[1].Substring(1);//remove o espaco que ficou antes da palavra em questao


                    //Agora devemos ter: "Task: TaskType"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "Task" + " TaskType"
                    if (AuxArr[0] != "Task")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.SVIPT_Task = AuxArr[1].Substring(1);//remove o espaco que ficou antes da palavra em questao

                    //Agora devemos ter: "TaskRepetitions: NumberOfRepts"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "TaskRepetitions" + " NumberOfRepts"
                    if (AuxArr[0] != "TaskRepetitions")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.SVIPT_TaskRepetitions = Convert.ToInt32(AuxArr[1]);//Converte para inteiro

                    //Agora devemos ter: "Sampling_Rate: SampleRate"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "Sampling_Rate" + " SampleRate"
                    if (AuxArr[0] != "Sampling_Rate")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    try
                    {
                        SessionFromFile.Sampling_Rate = Double.Parse(AuxArr[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        SessionFromFile.Sampling_Rate = 1;
                    }

                    //Agora devemos ter: "Force_Unity: Volts"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "Force_Unity" + " Volts"
                    if (AuxArr[0] != "Force_Unity")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.Force_Unity = AuxArr[1].Substring(1);//remove o espaco que ficou antes da palavra em questao

                    //Agora devemos ter: "BaseLineForce_Val: BaseVal"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "BaseLineForce_Val" + " BaseVal"
                    if (AuxArr[0] != "BaseLineForce_Val")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    try
                    {
                        SessionFromFile.baseLineForceVal = Double.Parse(AuxArr[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        SessionFromFile.baseLineForceVal = 0;
                    }

                    //Agora devemos ter: "MaxVoluntForce_Val: MaxVal"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "MaxVoluntForce_Val" + " MaxVal"
                    if (AuxArr[0] != "MaxVoluntForce_Val")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    try
                    {
                        SessionFromFile.maxForceVal = Double.Parse(AuxArr[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        SessionFromFile.maxForceVal = 0;
                    }

                    //Agora devemos ter: "ConversionFunction: AX + B = Y"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', 'X', '=');//encontra o : e separa em "ConversionFunction" + " A" + " +B " + " Y"
                    if (AuxArr[0] != "ConversionFunction")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }//como queremos os valores de A e B, pegamos A da posicao [1] e para pegar B removemos " +" da posicao [2] e entao o pegamos
                    try
                    {
                        SessionFromFile.A = Double.Parse(AuxArr[1], System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        SessionFromFile.A = 0;
                    }
                    //Remove o " +" e entao converte para double
                    try
                    {
                        SessionFromFile.B = Double.Parse(AuxArr[2].Substring(2, AuxArr[2].Length - 2), System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        SessionFromFile.B = 0;
                    }

                    //Agora devemos ter: "MaxForce_Percentage: Max%"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "MaxForce_Percentage" + " Max%"
                    if (AuxArr[0] != "MaxForce_Percentage")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.maxForcePercentage = Convert.ToInt32(AuxArr[1]);//Converte para int

                    //Agora devemos ter: "MinForce_Percentage: Min%"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "MinForce_Percentage" + " Min%"
                    if (AuxArr[0] != "MinForce_Percentage")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.minForcePercentage = Convert.ToInt32(AuxArr[1]);//Converte para int

                    //Agora devemos ter: "TaskMaxForce_Percentage: Task%"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':');//encontra o : e separa em "TaskMaxForce_Percentage" + " Task%"
                    if (AuxArr[0] != "TaskMaxForce_Percentage")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.taskMaxForcePercentage = Convert.ToInt32(AuxArr[1]);//Converte para int


                    //Agora devemos ter os limites do cursor
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Limites de movimento do cursor" + " LimiteEsquerdo" + "LimiteDireito" 
                    if (AuxArr[0] != "Limites de movimento do cursor")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    SessionFromFile.cursorLimits[0] = Convert.ToInt32(AuxArr[1]);//Converte para int

                    SessionFromFile.cursorLimits[1] = Convert.ToInt32(AuxArr[2]);//Converte para int
                    larguraEmPixels = SessionFromFile.cursorLimits[1] - SessionFromFile.cursorLimits[0];

                    //Agora devemos ter tamanho e posicao do cursor
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    if (StrAux != "Cursor")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }

                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Location" + " Localizacao horizontal em pixels do lado esquerdo" + "Localizacao vertical em pixels do lado direito" 
                    if (AuxArr[0] != "Location")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }

                    CursorLeftLocationAtHome = Convert.ToInt32(AuxArr[1]);//Converte para int

                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Size" + " Largura em pixels" + "Altura em pixels" 
                    if (AuxArr[0] != "Size")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }

                    CursorWidth = Convert.ToInt32(AuxArr[1]);//Converte para int




                    //Agora devemos ter tamanho e posicao da box do home, entretanto nao usamos isso pra nada, entao vou s'o ler sem salvar essas linhas
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    if (StrAux != "Home")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }

                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Location" + " Localizacao horizontal em pixels do lado esquerdo" + "Localizacao vertical em pixels do lado direito" 
                    if (AuxArr[0] != "Location")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Size" + " Largura em pixels" + "Altura em pixels" 
                    if (AuxArr[0] != "Size")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    

                    //#######################################
                    for (int ixi = 0; ixi < SessionFromFile.numberOfTrials; ixi++)
                    {

                        if (ixi != SessionFromFile.numberOfTrials - 1)
                        {
                            StrAux = sr.ReadLine();//vai ler a linha toda

                            StrAux = sr.ReadLine();//vai ler a linha toda
                            AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Location" + " Localizacao horizontal em pixels do lado esquerdo" + "Localizacao vertical em pixels do lado direito" 
                            if (AuxArr[0] != "Location")
                            {//Verifica se a primeira parte esta correta
                                MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                                return; //parar execucao se estiver errado.
                            }

                            leftLimits[ixi] = Convert.ToInt32(AuxArr[1]);//Converte para int

                            StrAux = sr.ReadLine();//vai ler a linha toda
                            AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Size" + " Largura em pixels" + "Altura em pixels" 
                            if (AuxArr[0] != "Size")
                            {//Verifica se a primeira parte esta correta
                                MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                                return; //parar execucao se estiver errado.
                            }
                        }

                        StrAux = sr.ReadLine();//vai ler a linha toda

                        StrAux = sr.ReadLine();//vai ler a linha toda
                        AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Location" + " Localizacao horizontal em pixels do lado esquerdo" + "Localizacao vertical em pixels do lado direito" 
                        if (AuxArr[0] != "Location")
                        {//Verifica se a primeira parte esta correta
                            MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                            return; //parar execucao se estiver errado.
                        }

                        rightLimits[ixi] = Convert.ToInt32(AuxArr[1]);//Converte para int
                        if(ixi== SessionFromFile.numberOfTrials-1) leftLimits[ixi] = rightLimits[ixi];

                        StrAux = sr.ReadLine();//vai ler a linha toda
                        AuxArr = StrAux.Split(':', '\t');//encontra o : e separa em "Size" + " Largura em pixels" + "Altura em pixels" 
                        if (AuxArr[0] != "Size")
                        {//Verifica se a primeira parte esta correta
                            MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                            return; //parar execucao se estiver errado.
                        }


                        widths[ixi] = Convert.ToInt32(AuxArr[1]);//Converte para int
                        rightLimits[ixi] = rightLimits[ixi] + widths[ixi];
                    }


                    //Diminuindo a area que se considera acerto para os limites internos do alvo
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    for(int Trials = 0; Trials < SessionFromFile.numberOfTrials-1; Trials++)
                    {
                        rightLimits[Trials] -= widths[Trials];
                        leftLimits[Trials] += widths[Trials];
                    }
                    //#######################################

                    //agora, se esta for uma sessao modificada, vamos ter a lista das curvas usadas (n numeros entre 0e6, sendo n o numero de sequencias realizadas)
                    //se nao for, vamos ter a lista de posicoes do cursor na tela ao longo de todo o experimento
                    if (SessionFromFile.SVIPT_Task == sviptConstant.sviptTaskModifed || SessionFromFile.SVIPT_Task== sviptConstant.sviptTaskModifed2)
                    {
                        StrAux = sr.ReadLine();//vai ler a linha toda
                        while (StrAux != "Curvas Usadas: ")
                        {
                            if (sr.EndOfStream) //se tiver chegado no EOF sem ter encontrado "Curvas Usadas: "
                            {//retorna dizendo que o arquivo ta errado
                                MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                                return; //parar execucao se estiver errado.
                            }
                            StrAux = sr.ReadLine();//vai ler a linha toda
                        }
                        for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                        {

                            StrAux = sr.ReadLine();
                            try
                            {
                                SessionFromFile.ListaDeCurvas[i] = int.Parse(StrAux, System.Globalization.NumberFormatInfo.InvariantInfo);
                            }
                            catch
                            {
                                SessionFromFile.ListaDeCurvas[i] = 0;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                            SessionFromFile.ListaDeCurvas[i] = 0;
                    }
                    


                    //Agora, vamos ter todas as posicoes dos targets e dos cursores na tela
                    //uteis para recriar o experimento, porem desnecessarias para a analise
                    //portanto vou apenas ignorar o que foi lido, ate encontrarmos ForceSignal
                    //que eh o proximo dado relevante
                    //Agora devemos ter: "ForceSignal: \nSinal\nSinal\nSinal... ...\nSinal\nSinal"
                    StrAux = sr.ReadLine();//vai ler a linha toda
                    while (StrAux != "ForceSignal: ")
                    {
                        if (sr.EndOfStream) //se tiver chegado no EOF sem ter encontrado "ForceSignal: "
                        {//retorna dizendo que o arquivo ta errado
                            MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                            return; //parar execucao se estiver errado.
                        }
                        StrAux = sr.ReadLine();//vai ler a linha toda
                    }
                    SessionFromFile.data.Clear();
                    //quando encontrar o cabecalho certo, comeca a ler o sinal de forca
                    while (sr.Peek() != 'N' && sr.Peek() > -1) //> -1)//se for -1 eh pq chegou no EOF (end of file)
                    {
                        StrAux = sr.ReadLine();
                        try
                        {
                            SessionFromFile.data.Add(Double.Parse(StrAux, System.Globalization.NumberFormatInfo.InvariantInfo));
                        }
                        catch
                        {
                            SessionFromFile.data.Add(0);
                        }
                    }
                    //Acabou o primeiro arquivo.
                    //Agora vamos abrir o proximo arquivo, sabendo que
                    //o nome deles eh sempre arqv1: BLABLABLA arquiv2: BLABLABLA_results
                }
                FileName2 = FileName.Substring(0, FileName.Length - 4);
                FileName2 = FileName2 + "_results.txt";
                using (StreamReader sr2 = new StreamReader(FileName2, Encoding.Default))
                {
                    //Agora devemos ter: "Accuracy: \n0\n-1\n1... (0=ok 1=overshoot -1=undershoot) ...\n0\n0"
                    StrAux = sr2.ReadLine();//vai ler a linha toda
                    if (StrAux != "Accuracy: ")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                        for (int j = 0; j < SessionFromFile.numberOfTrials; j++)
                        {
                            StrAux = sr2.ReadLine();
                            AuxArr = StrAux.Split('\t');//encontra o tab que separa em "target num" + "0/1/-1"
                            SessionFromFile.accuracy[i, j] = Convert.ToInt32(AuxArr[1]);
                            AccuracyWholeExperiment[NumDoBloco, i, j] = 0;// SessionFromFile.accuracy[i,j];

                        }

                    //Agora devemos ter: "ErrorPercent: \n"
                    StrAux = sr2.ReadLine();//vai ler a linha toda
                    if (StrAux != "ErrorPercent: ")
                    {//Verifica se a primeira parte esta correta
                        MessageBox.Show("Arquivo incompativel!\nPor favor verifique se selecionou o arquivo correto.");
                        return; //parar execucao se estiver errado.
                    }
                    for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                        for (int j = 0; j < SessionFromFile.numberOfTrials; j++)
                        {
                            StrAux = sr2.ReadLine();
                            AuxArr = StrAux.Split('\t');//encontra o tab que separa em "target num" + "0/1/-1"
                            SessionFromFile.error[i, j] = Convert.ToDouble(AuxArr[1]);
                            ErrorSizeWholeExperiment[NumDoBloco, i, j] = SessionFromFile.error[i,j];

                        }
                }
                bool IsHome = true;
                double InstNormPos = 0;
                int NumOfSamples = 0;
                int count = 0;
                int k = 0;
                //bool leutudo = false;
                int PosNaTela = 0;
                double PosNaTelaPercent = 0;
                //VARIAVEIS PRA PARTE DE REFAZER O CALCULO DE ACERTO, ERRO E TAMANHO DE ERRO#############################################
                //Contador de alvos atingidos
                int targetCounter = 0;
                //Contador de trials (sequência de alvos)
                 int trialCounter = 0;
                //Variável que define o estado do cursor
                int cursorState = 0;
                //Definindo os possíveis estados do cursor
                const int cursorAtHome = 0; //Cursor localizado em Home
                const int cursorMovingTarget = 1; //Movendo em direção à algum alvo (esquerda pra direita)        
                const int cursorHitTarget = 2; //Cursor atingiu o centro do alvo
                const int cursorOvershoot = 3; //Cursor passou pelo alvo      
                const int cursorUndershoot = 4; //Cursor não atingiu o alvo  
                //Guarda a posição inicial do cursor que indica que ele está em Home
                int cursorHome = (int)CursorLeftLocationAtHome;
                
                //Percentual do comprimento total onde estao localizados cada um dos 5 alvos
                double[] tgtPercents;
                tgtPercents = new double[SessionFromFile.numberOfTrials];
                tgtPercents[3] = 0.2;
                tgtPercents[0] = 0.4;
                tgtPercents[2] = 0.6;
                tgtPercents[4] = 0.735;
                tgtPercents[1] = 0.875;
                //Variável para verificar oscilação no movimento do cursor
                double maxCursorPosition = 0;
                //Variável que armazena a posição do cursor no momento do undershoot
                double cursorPosUndershoot = 0;

        //#######################################################################################################################
        NumOfSamples = (int)SessionFromFile.Sampling_Rate * 50 / 1000;//50 eh o tempo do intervalo em milisegundos que tbm nao foi salvo no arquivo, 1000 eh pra converter pra segundos dnovo==> amostras/segundos  * segundos/1000  *  1000

                for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                    for (int j = 0; j < SessionFromFile.numberOfTrials; j++)
                    {
                        pixelsEachTrial[NumDoBloco, i, j] = new Queue<int>();
                        while (k < SessionFromFile.data.Count)
                        {
                            InstNormPos = 0;
                            for (int l = 0; l < NumOfSamples; l++)
                            {//soma e faz a media das proximas 10 amostras (igual o programa faz on-line pra determinar a posicao do cursor) 
                                InstNormPos = InstNormPos + SessionFromFile.data[k];
                                k++;
                            }
                            InstNormPos = InstNormPos / NumOfSamples;
                            //transforma de 0-1023bits para 0-5V (igual o programa faz on-line pra determinar a posicao do cursor)
                            InstNormPos = InstNormPos * (5.0 / 1024.0);//5 eh o AD_range que nao foi salvo no txt e 10 eh o num de bits que tambem nao foi salvo, talvez seja necessario adicionar
                                                                       //transforma de 0-5V para 0-100% da tela (igual o programa faz on-line pra determinar a posicao do cursor)
                            if (InstNormPos <= 0)
                            {
                                InstNormPos = 0;
                            }

                            else if (SessionFromFile.SVIPT_Task == sviptConstant.sviptTaskModifed)
                            {
                                switch (SessionFromFile.ListaDeCurvas[i])
                                {
                                    case 0://curva original
                                        InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                                        break;
                                    case 1://-3variancia
                                        InstNormPos = 1.026 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.061);
                                        break;
                                    case 2://-2variancia
                                        InstNormPos = 1.017 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.038);
                                        break;
                                    case 3://-1variancia
                                        InstNormPos = 1.009 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.017);
                                        break;
                                    case 4://+1variancia
                                        InstNormPos = 0.9997 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.986);
                                        break;
                                    case 5://+2variancia
                                        InstNormPos = 0.9987 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.975);
                                        break;
                                    case 6://+3variancia
                                        InstNormPos = 0.9962 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.967);
                                        break;
                                    default:
                                        InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                                        break;
                                }
                            }
                            else if (SessionFromFile.SVIPT_Task == sviptConstant.sviptTaskModifed2)
                            {
                                switch (SessionFromFile.ListaDeCurvas[i])
                                {
                                    case 0://curva original
                                        InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                                        break;
                                    case 1://-3variancia
                                        InstNormPos = 1.018 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 6.068);
                                        break;
                                    case 2://-2variancia
                                        InstNormPos = 1.014 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.7);
                                        break;
                                    case 3://-1variancia
                                        InstNormPos = 1.008 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.344);
                                        break;
                                    case 4://+1variancia
                                        InstNormPos = 0.9891 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.673);
                                        break;
                                    case 5://+2variancia
                                        InstNormPos = 0.9745 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.366);
                                        break;
                                    case 6://+3variancia
                                        InstNormPos = 0.955 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 4.083);
                                        break;
                                    default:
                                        InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                                        break;
                                }
                            }
                            else if (SessionFromFile.SVIPT_Task == sviptConstant.sviptTaskLinear)
                            {
                                InstNormPos = (InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100));
                            }
                            else
                            {

                                InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                            }

                            //InstNormPos = 1.0 + (Math.Log(InstNormPos / (SessionFromFile.maxForceVal * SessionFromFile.taskMaxForcePercentage / 100)) / 5.0);
                            //transforma de 0-100% para a posicao em pixels onde o cursor deve estar  (igual o programa faz on-line pra determinar a posicao do cursor)
                            PosNaTelaPercent = InstNormPos;
                            PosNaTela = SessionFromFile.cursorLimits[0] + (int)((SessionFromFile.cursorLimits[1] - SessionFromFile.cursorLimits[0]) * InstNormPos);

                            //(devido ao uso de log, ou uma possivel forca maior que a forca maxima calibrada)
                            //verifica se nao obtivemos posicao negativa ou maior que a largura da tela  (igual o programa faz on-line pra determinar a posicao do cursor)
                            if (PosNaTela < SessionFromFile.cursorLimits[0]) PosNaTela = SessionFromFile.cursorLimits[0];
                            if (PosNaTela > SessionFromFile.cursorLimits[1]) PosNaTela = SessionFromFile.cursorLimits[1];

                            //a posicao eh pensada para ser a posicao do centro do cursor, mas a funcao pronta do c#
                            //define a posicao da lateral esquerda do cursor, entao tiramos metade da largura do cursor
                            //if (CursorWidth%2 ==1) PosNaTela = PosNaTela - (CursorWidth) / 2;
                            PosNaTela = PosNaTela - CursorWidth / 2;
                            pixelsEachTrial[NumDoBloco, i, j].Enqueue(PosNaTela);
                            //#############################################################################################################################################
                            //##############TENDO A POSIÇAO NA TELA, VOU REFAZER O EXPERIMENTO INTEIRO PRA DETERMINAR OS ACERTOS E ERROS NO CRITÉRIO NOVO##################                          

                            //###########TROCAR xn -> PosNaTela,     theSession -> SessionFromFile
                            //Configura o novo valor máximo do cursor
                            //Este valor será utilizado para calcular o erro em relação ao target
                            if (PosNaTelaPercent > maxCursorPosition)
                                maxCursorPosition = PosNaTelaPercent;
                            //Se o cursor retornar mais que 5% do seu máximo enquanto estiver se movendo ao target
                            //então ele oscilou demais e isso é considerado um undershoot
                            else if ((maxCursorPosition - PosNaTelaPercent) > (((double)SessionFromFile.SVIPT_TaskOscilationPercent) / 100.0) && cursorState == cursorMovingTarget)
                            {
                                //Salva a posição no undershoot
                                cursorPosUndershoot = PosNaTelaPercent;//arrumar dps
                                //Configura o status do cursor como undershoot
                                cursorState = cursorUndershoot;
                                //Atualiza o status para undershoot
                            }

                            //Atualiza o status do cursor
                            //Quando o cursor passa por home, ele está movendo em direção a algum target
                            //Se o cursor estava indo para algum target e chega em "home", então um target
                            //foi atingido
                            if ( (PosNaTela) >= (cursorHome + CursorWidth) && cursorState == cursorAtHome)
                            {
                                //Atualiza o status do cursor para indicar que ele está se movimentando
                                cursorState = cursorMovingTarget;
                            }
                            //Se o cursor está movendo em direção à um alvo e ultrapassou o seu limite esquerdo por pelo menos 50%
                            //Então o alvo foi atingido
                            else if (PosNaTela + CursorWidth/2 >= leftLimits[j] && cursorState == cursorMovingTarget)
                            {
                                cursorState = cursorHitTarget;
                                //label_Status.Invoke(new Action(() => label_Status.Text = "Alvo: " + (targetCounter + 1).ToString()));// + "  Acertou!"));
                                //if (targetCounter >= 4)
                                //{
                                //    pictureBox_GoStop.BackColor = Color.Red;
                                //    label_GoStop.Invoke(new Action(() => label_GoStop.Text = "STOP"));
                                //}
                            }
                            //Se o cursor ultrapassar o alvo desejado em até 50% do seu tamanho
                            //Então aconteceu um overshoot
                            else if (PosNaTela+CursorWidth-(CursorWidth/2) >= rightLimits[j] && cursorState == cursorHitTarget)
                            {
                                cursorState = cursorOvershoot;
                            }
                            else if (PosNaTela == cursorHome && cursorState != cursorAtHome)
                            {
                                //Calcula o erro do cursor em relação ao target                    
                                //SessionFromFile.error[i, j] = (maxCursorPosition/larguraEmPixels) - tgtPercents[j];
                                //Zera a variável que armazena o máximo deslocamento do cursor
                                maxCursorPosition = 0;

                                //Avalia o resultado do movimento
                                //Undershoot, overshoot?
                                //Se nenhum dos dois tiver ocorrido, então foi um acerto
                                //Se o cursor voltou à home sem atingir o target, então foi undershoot
                                //Se o cursor oscilou demais, então foi undershoot

                                //Ja zerei ali em cima //AccuracyWholeExperiment[NumDoBloco, i, j] = 0; //garantindo que vai estar limpo caso nao seja nem under nem over
                                if (cursorState == cursorMovingTarget || cursorState == cursorUndershoot)
                                {
                                    SessionFromFile.undershoot[i, j] = true;
                                    AccuracyWholeExperiment[NumDoBloco, i, j] = -1;
                                    //Calcula o erro do cursor em relação ao target
                                    //Se ocorreu um undershoot por oscilação, deve-se levar em consideração
                                    //a posição dada por "cursorPosUndershoot"     
                                    //##############desnecessario refazer isso, pq o calculo do erro ja tava certo      
                                    //if (cursorPosUndershoot > 0)
                                    //    SessionFromFile.error[i, j] = (cursorPosUndershoot/larguraEmPixels) - tgtPercents[j];

                                    //Zera a posição do undershoot
                                    cursorPosUndershoot = 0;
                                }
                                //Se o cursor ultrapassou o target, então foi overshoot
                                else if (cursorState == cursorOvershoot)
                                {
                                    SessionFromFile.overshoot[i, j] = true;
                                    AccuracyWholeExperiment[NumDoBloco, i, j] = 1;
                                }

                                //Incrementa o contador de targets
                                /////targetCounter++;
                                //Status do cursor: Está em Home
                                cursorState = cursorAtHome;
                                //Zera o contador de target, caso ultrapasse o máximo de targets
                                if (j >= SessionFromFile.numberOfTrials-1)
                                {
                                    /////targetCounter = 0; //Zera o contador de targets
                                    /////trialCounter++; //Incrementa o contador de repetições (trials)
                                    
                                    //Caso o total de repetições tenha sido realizada, a sessão está concluída
                                    //Assim, a aquisição pode ser encerrada
                                    if (i >= SessionFromFile.SVIPT_TaskRepetitions-1)
                                    {
                                        //Encerra a aquisição
                                        //this.StopAcquisition();
                                        //A sessão terminou, atualizando o label_Status
                                        //label_Status.Invoke(new Action(() => label_Status.Text = "Sessão encerrada!"));
                                        //Atualiza o status do cursor para parado
                                        cursorState = cursorAtHome;
                                        //Muda o label para STOP
                                        //label_GoStop.Invoke(new Action(() => label_GoStop.Text = "STOP"));
                                        //Salvar o arquivo de dados?
                                        //###############################################
                                        //SessionFromFile.saveAnaliseDataFile();//criar função pra fazer isso ai
                                    }
                                }
                            }

                            //#############################################################################################################################################
                            //#############################################################################################################################################

                            if ((IsHome == false && j==0) || (j != 0 && j != 4) || (j == 4 && !(cursorState == cursorHitTarget || cursorState == cursorUndershoot)))
                                count = count + NumOfSamples;//incrementa o contador de tempo
                            if (PosNaTela >= cursorHome + CursorWidth && IsHome)//Posicao na tela tem que ser menor do que o valor maximo que pode ser
                            {// dividir Cursor width por 2 ali em cima e no proximo -cursorwidth/2
                                //se estava em home, e agora a posicao do cursor saiu de home
                                IsHome = false;//nao esta mais em home
                            }
                            else if (PosNaTela == cursorHome && !IsHome)
                            {
                                //se nao estava em home, e agora a posicao do cursor chegou em home
                                //o cursor voltou pra home
                                IsHome = true;
                                //contabiliza o tempo da trial que acabou de ser marcado
                                
                                SessionFromFile.times[i, j] = count * (1 / SessionFromFile.Sampling_Rate);
                                TimesWholeExperiment[NumDoBloco, i, j] = SessionFromFile.times[i, j];
                                //reseta o contador
                                count = 0;
                                break;//acabou o movimento entao vamos contabilizar o tempo dessa trial e comecar a marcar o tempo de proxima
                            }
                        }
                        
                    }

                //chartSeries.Points.DataBindY(cursorMotion);
            }
        }

        private void CalcularSkillParameter()
        {
            int ErrorsPerBlock = 0; //se 1 ou mais trials de uma sequencia de 5 estiver errada, ja conta aquela sequencia como errada
            double AvrgMovTime = 0.0;
            double Time = 0.0;
            double b = 5.424; // b eh uma constante de valor bizarro(por isso b) definida no artigo
            bool NaoErrouNessaSequencia = true;
            int NumOfSeqsPerBlock = 32;

            //dentro desse for ja vou anotar o numero de overshoots, undershoots, acertos, sequencias certas,
            //alem do error rate e do skill parameter pra cada bloco, pra poder salvar em um txt de resultados depois.
            for(int i=0;i<NBlocos;i++)
            {
                RightSeqsPerBlock[i] = 0;//inicializando com zeros
                ErrorRatePerBlock[i] = 0.0;
                TimesPerBlock[i] = 0.0;
                OKsPerBlock[i] = 0;
                OversPerBlock[i] = 0;
                UndersPerBlock[i] = 0;
                if (i == NBlocos - 1 && TemReativacao)
                    NumOfSeqsPerBlock = NumOfSeqsPerBlock / 2; //se for a reativacao, ai o numero de sequencias do bloco eh so metade
                for(int j=0;j<NumOfSeqsPerBlock;j++)
                {
                    
                    Time = 0.0;
                    NaoErrouNessaSequencia = true;
                    for (int k=0;k<5;k++)
                    {
                        Time = Time + TimesWholeExperiment[i, j, k]; //time=time das 5 reps
                        if (AccuracyWholeExperiment[i, j, k] != 0)
                        {
                            NaoErrouNessaSequencia = false;
                            ErrorsPerBlock++;
                            if (AccuracyWholeExperiment[i, j, k] == 1)
                                OversPerBlock[i]++;
                            if (AccuracyWholeExperiment[i, j, k] == -1)
                                UndersPerBlock[i]++;
                        }
                        else
                            OKsPerBlock[i]++;
                    }
                    TimesPerBlock[i] = TimesPerBlock[i] + Time; //tempo do bloco = tempo de 5 reps + tempo das prox 5 reps
                    if (NaoErrouNessaSequencia)
                        RightSeqsPerBlock[i]++;//se nao errou na sequencia, aumenta o num de sequencias sem erros
                }
                AvrgMovTime = TimesPerBlock[i] / NumOfSeqsPerBlock; //tempo medio = tempo de todas as 32 series de 5 repeticoes/ 32 (num de seqs por bloco)
                ErrorRatePerBlock[i] = (double)ErrorsPerBlock / (NumOfSeqsPerBlock*5); //error rate=num de erros/num de tentativas
                if (ErrorRatePerBlock[i] == 0) ErrorRatePerBlock[i] = 0.001;
                SkillParameterPerBlock[i] = (1 - ErrorRatePerBlock[i]) / (ErrorRatePerBlock[i] * (Math.Pow(Math.Log(AvrgMovTime), b)));
                AvrgMovTime = 0;
                ErrorsPerBlock = 0;
            }
        }

        private void GraficoSkillParameter()
        {
            analysisChart.Series.Clear();
            analysisChart.ChartAreas.Clear();
            chartSeries = new Series("SkillParameter");
            chartSeries.ChartType = SeriesChartType.Line;
            chartSeries.BorderWidth = 3;
            analysisChart.Series.Add(chartSeries);
            //Cria uma nova chartarea
            chartArea = new ChartArea("areaSkillParameter");
            analysisChart.ChartAreas.Add(chartArea);
            for (int i=0; i< NBlocos;i++)
                chartSeries.Points.AddXY(i+1, SkillParameterPerBlock[i]);
        }

        private void GraficoErrorSize()
        {
            analysisChart.Series.Clear();
            analysisChart.ChartAreas.Clear();
            chartSeries = new Series("ErrorSize");
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.BorderWidth = 3;
            analysisChart.Series.Add(chartSeries);
            //Cria uma nova chartarea
            chartArea = new ChartArea("areaErrorSize");
            analysisChart.ChartAreas.Add(chartArea);
            int posicao = 0;
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 32; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        posicao++;
                        chartSeries.Points.AddXY(posicao, ErrorSizeWholeExperiment[i, j, k]);
                    }
        }
        private void GraficoAccuracy()
        {
            analysisChart.Series.Clear();
            analysisChart.ChartAreas.Clear();
            chartSeries = new Series("TimePerTrial");
            chartSeries.ChartType = SeriesChartType.Column;
            chartSeries.BorderWidth = 3;
            analysisChart.Series.Add(chartSeries);
            //Cria uma nova chartarea
            chartArea = new ChartArea("areaTimeXAccuracy");
            analysisChart.ChartAreas.Add(chartArea);
            int posicao = 0;
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 32; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        posicao++;
                        chartSeries.Points.AddXY(posicao, TimesWholeExperiment[i, j, k]);
                        if(AccuracyWholeExperiment[i,j,k] ==0)
                            chartSeries.Points[posicao-1].Color = Color.Green;
                        if (AccuracyWholeExperiment[i, j, k] == 1)
                            chartSeries.Points[posicao-1].Color = Color.Blue;
                        if (AccuracyWholeExperiment[i, j, k] == -1)
                            chartSeries.Points[posicao-1].Color = Color.Red;
                    }
        }

        private void errorSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraficoErrorSize();
        }

        private void accuracyPerTrialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraficoAccuracy();
        }

        private void skillParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraficoSkillParameter();
        }


        private void plotarTrajetoriaPorAlvo(int Target)
        {
            analysisChart.Series.Clear();
            analysisChart.ChartAreas.Clear();
            chartB4.Series.Clear();
            chartB4.ChartAreas.Clear();
            chartB6.Series.Clear();
            chartB6.ChartAreas.Clear();
            chartB8.Series.Clear();
            chartB8.ChartAreas.Clear();
            chartB10.Series.Clear();
            chartB10.ChartAreas.Clear();
            chartB12.Series.Clear();
            chartB12.ChartAreas.Clear();

            chartSeries = new Series("Trajetoria");
            chartSeries.ChartType = SeriesChartType.FastLine;
            chartSeries.BorderWidth = 2;
            SeriesB4 = new Series("Trajetoria");
            SeriesB4.ChartType = SeriesChartType.FastLine;
            SeriesB4.BorderWidth = 2;
            SeriesB6 = new Series("Trajetoria");
            SeriesB6.ChartType = SeriesChartType.FastLine;
            SeriesB6.BorderWidth = 2;
            SeriesB8 = new Series("Trajetoria");
            SeriesB8.ChartType = SeriesChartType.FastLine;
            SeriesB8.BorderWidth = 2;
            SeriesB10 = new Series("Trajetoria");
            SeriesB10.ChartType = SeriesChartType.FastLine;
            SeriesB10.BorderWidth = 2;
            SeriesB12 = new Series("Trajetoria");
            SeriesB12.ChartType = SeriesChartType.FastLine;
            SeriesB12.BorderWidth = 2;

            analysisChart.Series.Add(chartSeries);
            chartB4.Series.Add(SeriesB4);
            chartB6.Series.Add(SeriesB6);
            chartB8.Series.Add(SeriesB8);
            chartB10.Series.Add(SeriesB10);     
            chartB12.Series.Add(SeriesB12);
            //Cria uma nova chartarea
            chartArea = new ChartArea("areaErrorSize");
            analysisChart.ChartAreas.Add(chartArea);
            AreaB4 = new ChartArea("areaErrorSize");
            chartB4.ChartAreas.Add(AreaB4);
            AreaB6 = new ChartArea("areaErrorSize");
            chartB6.ChartAreas.Add(AreaB6);
            AreaB8 = new ChartArea("areaErrorSize");
            chartB8.ChartAreas.Add(AreaB8);
            AreaB10 = new ChartArea("areaErrorSize");
            chartB10.ChartAreas.Add(AreaB10);
            AreaB12 = new ChartArea("areaErrorSize");
            chartB12.ChartAreas.Add(AreaB12);
            int k = 30;
            chartArea.AxisX.Maximum = k;
            AreaB4.AxisX.Maximum = k;
            AreaB6.AxisX.Maximum = k;
            AreaB8.AxisX.Maximum = k;
            AreaB10.AxisX.Maximum = k;
            AreaB12.AxisX.Maximum = k;

            int posicao = 0;
            for (int i = 0; i < 32; i++)//plotando grafico do B1 a B12
            {
                for (int j = 0; j < pixelsEachTrial[0, i, Target].Count; j++)
                {
                    posicao++;
                    chartSeries.Points.AddXY(posicao, pixelsEachTrial[0, i, Target].ElementAt(j));
                    if(j< pixelsEachTrial[0, i, Target].Count-1)
                        if (pixelsEachTrial[0, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;

                for (int j = 0; j < pixelsEachTrial[3, i, Target].Count; j++)
                {
                    posicao++;
                    SeriesB4.Points.AddXY(posicao, pixelsEachTrial[3, i, Target].ElementAt(j));
                    if (j < pixelsEachTrial[3, i, Target].Count - 1)
                        if (pixelsEachTrial[3, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;
                for (int j = 0; j < pixelsEachTrial[5, i, Target].Count; j++)
                {
                    posicao++;
                    SeriesB6.Points.AddXY(posicao, pixelsEachTrial[5, i, Target].ElementAt(j));
                    if (j < pixelsEachTrial[5, i, Target].Count - 1)
                        if (pixelsEachTrial[5, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;
                for (int j = 0; j < pixelsEachTrial[7, i, Target].Count; j++)
                {
                    posicao++;
                    SeriesB8.Points.AddXY(posicao, pixelsEachTrial[7, i, Target].ElementAt(j));
                    if (j < pixelsEachTrial[7, i, Target].Count - 1)
                        if (pixelsEachTrial[7, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;
                for (int j = 0; j < pixelsEachTrial[9, i, Target].Count; j++)
                {
                    posicao++;
                    SeriesB10.Points.AddXY(posicao, pixelsEachTrial[9, i, Target].ElementAt(j));
                    if (j < pixelsEachTrial[9, i, Target].Count - 1)
                        if (pixelsEachTrial[9, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;
                for (int j = 0;j< pixelsEachTrial[11, i, Target].Count; j++)
                {
                    posicao++;
                    SeriesB12.Points.AddXY(posicao, pixelsEachTrial[11, i, Target].ElementAt(j));
                    if (j < pixelsEachTrial[11, i, Target].Count - 1)
                        if (pixelsEachTrial[11, i, Target].ElementAt(j+1) == CursorLeftLocationAtHome)
                        posicao--;
                }
                posicao = 0;
            }
        }



        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fn;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            StreamWriter writer;
            if (MessageBox.Show("Deseja salvar os dados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                string exePath = Application.ExecutablePath; //diretório do executável
                string fnConfig = Path.GetDirectoryName(exePath) + "\\";
                fn = fnConfig + SessionFromFile.ID_Volunteer + "_Overall_Results.txt";

                using (writer = new StreamWriter(@fn))
                {
                    //identificador do voluntário
                    writer.Write(sviptFileConstant.idVolunteer + SessionFromFile.ID_Volunteer);
                    //String descritor da tarefa executada na sessão. 
                    writer.Write("\r\n" + sviptFileConstant.sviptTask + SessionFromFile.SVIPT_Task);
                    //quantidade de repetições da tarefa durante a sessão
                    writer.Write("\r\n" + sviptFileConstant.taskRepetitions + SessionFromFile.SVIPT_TaskRepetitions.ToString());


                    // 1->Overshoot
                    // 0->Acerto
                    //-1->Undershoot
                    writer.Write("\r\n" + "Accuracy:");
                    int targetId = 1;
                    foreach (int val in AccuracyWholeExperiment)
                    {// loop over all data values
                        writer.Write("\r\n");
                        writer.Write(targetId.ToString() + "\t" + val.ToString("D"));
                        targetId++;
                        if (targetId > SessionFromFile.numberOfTrials)
                            targetId = 1;
                    }


                    writer.Write("\r\n" + "Localizacao:");
                    targetId = 1;
                    int aux = 0;
                    for (int NB = 0; NB < NBlocos; NB++)
                        for (int i = 0; i < SessionFromFile.SVIPT_TaskRepetitions; i++)
                            for (int j = 0; j < SessionFromFile.numberOfTrials; j++)
                                while (pixelsEachTrial[NB,i,j].Count>0)
                                {// loop over all data values
                                    writer.Write("\r\n");
                                    aux = pixelsEachTrial[NB, i, j].Dequeue();
                                    writer.Write(NB.ToString() + "\t" + i.ToString() + "\t" + j.ToString() + "\t" + aux.ToString("D"));
                                    
                                }



                    //numero de acertos por bloco
                    writer.Write("\r\n" + "Numero de acertos por bloco:");
                    for (int i = 0; i < 12; i++)
                    {
                        if(i== 4 && TemReativacao)
                            writer.Write("\r\n" + OKsPerBlock[12].ToString());
                        writer.Write("\r\n" + OKsPerBlock[i].ToString());
                    }

                    //numero de undershoots por bloco
                    writer.Write("\r\n" + "Numero de undershoots por bloco:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + UndersPerBlock[12].ToString());
                        writer.Write("\r\n" + UndersPerBlock[i].ToString());
                    }

                    //numero de overshoots por bloco
                    writer.Write("\r\n" + "Numero de overshoots por bloco:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + OversPerBlock[12].ToString());
                        writer.Write("\r\n" + OversPerBlock[i].ToString());
                    }

                    //numero de sequencias completas corretas
                    writer.Write("\r\n" + "Numero de sequencias completas por bloco:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + RightSeqsPerBlock[12].ToString());
                        writer.Write("\r\n" + RightSeqsPerBlock[i].ToString());
                    }

                    //valor do skill parameter para cada bloco
                    writer.Write("\r\n" + "Skill parameter value:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + SkillParameterPerBlock[12].ToString());
                        writer.Write("\r\n" + SkillParameterPerBlock[i].ToString());
                    }

                    //error rate
                    writer.Write("\r\n" + "Error rate value:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + ErrorRatePerBlock[12].ToString());
                        writer.Write("\r\n" + ErrorRatePerBlock[i].ToString());
                    }

                    //tempo gasto em cada bloco
                    writer.Write("\r\n" + "Tempo gasto por bloco:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                            writer.Write("\r\n" + TimesPerBlock[12].ToString());
                        writer.Write("\r\n" + TimesPerBlock[i].ToString());
                    }

                    //tempo de cada movimento home -> alvo -> home
                    writer.Write("\r\n" + "Tempo gasto por cada trial:");
                    for (int i = 0; i < 12; i++)
                    {
                        if(i==4 && TemReativacao)
                        {
                            i = 12;
                            for (int j = 0; j < 16; j++)
                                for (int k = 0; k < 5; k++)
                                    writer.Write("\r\n" + TimesWholeExperiment[i, j, k].ToString());
                            i = 4;
                        }
                        for (int j = 0; j < 32; j++)
                            for (int k = 0; k < 5; k++)
                                writer.Write("\r\n" + TimesWholeExperiment[i, j, k].ToString());
                    }

                    //errorpercent cada movimento home -> alvo -> home
                    writer.Write("\r\n" + "Tamanho percentual do erro em cada trial:");
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 4 && TemReativacao)
                        {
                            i = 12;
                            for (int j = 0; j < 16; j++)
                                for (int k = 0; k < 5; k++)
                                    writer.Write("\r\n" + Convert.ToString(k+1) + "\t" + ErrorSizeWholeExperiment[i, j, k].ToString());
                            i = 4;
                        }
                        for (int j = 0; j < 32; j++)
                            for (int k = 0; k < 5; k++)
                            {
                                writer.Write("\r\n" + Convert.ToString(k+1) + "\t" + ErrorSizeWholeExperiment[i, j, k].ToString());
                            }
                    }


                }
            }
        }

        private void target1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            plotarTrajetoriaPorAlvo(0);
        }

        private void target2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            plotarTrajetoriaPorAlvo(1);
        }

        private void target3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            plotarTrajetoriaPorAlvo(2);
        }

        private void target4ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            plotarTrajetoriaPorAlvo(3);
        }

        private void target5ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            plotarTrajetoriaPorAlvo(4);
        }
    }
}
