using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVIPT_BioLab
{
    public partial class Calibrar : Form
    {
        int CountFMax = 0;
        double[] PiPf = new double [4];
        private Session session;
        //Objeto para comunicação serial
        private TeensyUsbCom serialPortObj;
        //Este objeto contem os diversos elementos associados ao equipamento,
        //firmware, status, dados de experimento etc.
        private TeensyData dataObj;

        public Calibrar(ref Session _session, ref TeensyUsbCom _serialPortObj,
                        ref TeensyData _dataObj)
        {
            InitializeComponent();

            session = _session;
            serialPortObj = _serialPortObj;
            dataObj = _dataObj;
        }

        private void Calibrar_Load(object sender, EventArgs e)
        {
            label_ValorLinhaBase.Text = session.baseLineForceVal.ToString();
            textBox_FMaxFinal.Text = session.maxForceVal.ToString();
            textBox_LimiteMin.Text = session.minForcePercentage.ToString();
            textBox_LimiteMax.Text = session.maxForcePercentage.ToString();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            //salvar alterações na sessão
            try
            {
                //Salva os valores das TxtBox pro array PiPf , para caso teja usando o valor padrao
                try
                {
                    PiPf[0] = Double.Parse(TbInicialX.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    PiPf[0] = 0;
                }
                try
                {
                    PiPf[1] = Double.Parse(TbInicialY.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    PiPf[1] = 0;
                }
                try
                {
                    PiPf[2] = Double.Parse(TbFinalX.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    PiPf[2] = 0;
                }
                try
                {
                    PiPf[3] = Double.Parse(TbFinalY.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    PiPf[3] = 0;
                }

                //Garantir que limite Max > Limite Min
                if (Convert.ToInt32(textBox_LimiteMax.Text) <= Convert.ToInt32(textBox_LimiteMin.Text))
                {
                    MessageBox.Show("O limite de força deve ser MAIOR QUE o limite mínimo.");
                    return; //não sair se estiver errado.
                }
                if (dataObj.ad_Range <= PiPf[3] || dataObj.ad_Range <= PiPf[1])
                {
                    MessageBox.Show("A tensao da curva da célula de carga deve ser menor que o range do conversor AD.\nPor favor utiliza uma célula de carga adequada.");
                    return;
                }
                if (PiPf[3] <= PiPf[1])
                {
                    MessageBox.Show("A tensao do ponto final deve ser maior que a do ponto inicial.\nPor favor verifique se nao esta colocando Pi e Pf na ordem trocada");
                    return;
                }
                if (PiPf[2] <= PiPf[0])
                {
                    MessageBox.Show("A forca do ponto final deve ser maior que a do ponto inicial.\nPor favor verifique se nao esta colocando Pi e Pf na ordem trocada");
                    return;
                }

                try
                {
                    session.baseLineForceVal = Double.Parse(label_ValorLinhaBase.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    session.baseLineForceVal = 0;
                }
                try
                {
                    session.maxForceVal = Double.Parse(textBox_FMaxFinal.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    session.maxForceVal = 0;
                }
                try
                {
                    session.minForcePercentage = (int) Double.Parse(textBox_LimiteMin.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    session.minForcePercentage = 0;
                }
                try
                {
                    session.maxForcePercentage = (int)Double.Parse(textBox_LimiteMax.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                catch
                {
                    session.maxForcePercentage = 0;
                }

                //Definir o percentual de força máximo a ser usado na tarefa, dependendo
                //do tipo de tarefa selecionado, e dentro dos limites definidos em
                //MinForce_Percentage e MaxForce_Percentage
                session.A = (PiPf[0] - PiPf[2]) / (PiPf[1] - PiPf[3]);
                session.B = PiPf[2] - session.A * PiPf[3]; //encontra os valores para a curva Kgf = A*Volts+B
                session.setTaskMaxForcePercentage();
                session.CurvaConvertSetada = false;
            }
            catch
            {
                return;
            }
            //se as conversões ocorram com sucesso
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            //Simplemente feche a janela - não salvar informações
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox_LimiteMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
        private void textBox_LimiteMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                textBox_LimiteMax_Leave(sender, e);
            }
        }
        private void textBox_LimiteMax_Leave(object sender, EventArgs e)
        {
            //Garantir que limite Max > Limite Min
            if (Convert.ToInt32(textBox_LimiteMax.Text) <= Convert.ToInt32(textBox_LimiteMin.Text))
                MessageBox.Show("O limite de força deve ser MAIOR QUE o limite mínimo.");
        }

        private void textBox_LimiteMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
        private void textBox_LimiteMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                textBox_LimiteMin_Leave(sender, e);
            }
        }
        private void textBox_LimiteMin_Leave(object sender, EventArgs e)
        {
            //Garantir que limite Max > Limite Min
            if (Convert.ToInt32(textBox_LimiteMax.Text) <= Convert.ToInt32(textBox_LimiteMin.Text))
                MessageBox.Show("O limite de força deve ser MAIOR QUE o limite mínimo.");
        }

        private void button_CalibrarLinhaBase_Click(object sender, EventArgs e)
        {
            //           if(!dataObj.getProperty(TeensyData.tDataPropertiesBool.Ready))
            //           {
            //               MessageBox.Show("Hardware não conectado.");
            //               return;
            //           }

            //obter média do sinal de força com o sensor livre - baseline
            //session.baseLineForceVal = getAverageForce(3.0); //->funcao do alcimar que eu n entendi e vou tentar fazer a minha
            session.baseLineForceVal = ForcaMedia(3.0);

            label_ValorLinhaBase.Text = session.baseLineForceVal.ToString();

        }

        private void button_CalibForcaMax_Click(object sender, EventArgs e)
        {
 //           if (!dataObj.getProperty(TeensyData.tDataPropertiesBool.Ready))
 //           {
 //               MessageBox.Show("Hardware não conectado.");
//                return;
//            }

            //obter média do sinal de força com o sensor sob máxima força do usuário - MVF.
            session.maxForceVal = ForcaMedia(3.0);
            if (CountFMax == 0)
            {
                label_FMax1.Text = session.maxForceVal.ToString();
            }

            if (CountFMax == 1)
            {
                label_FMax2.Text = session.maxForceVal.ToString();
            }

            if (CountFMax == 2)
            {
                label_FMax3.Text = session.maxForceVal.ToString();
                double Aux = Double.Parse(label_FMax1.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                Aux = Aux + Double.Parse(label_FMax2.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                Aux = Aux + Double.Parse(label_FMax3.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
                Aux = Aux / 3;
                session.maxForceVal = Aux;
                textBox_FMaxFinal.Text = Aux.ToString();
            }
            CountFMax++;
            if (CountFMax == 3)
                CountFMax = 0;

        }

        //Obter média do sinal de força durante o tempo solicitado - em segundos.
        private double getAverageForce(double period)
        {
            double sumAvg = 0.0;
            int nSamplesReceived = 0;

            //Coletar period segundos de dados.
            //O valor retornado será a média de todas as leituras neste intervalo.
            int totalSamples = (int)(session.Sampling_Rate * period);

            //Iniciar coleta de dados
            //    Host envia query.
            byte[] CmdBytes = ASCIIEncoding.ASCII.GetBytes(TeensyComCommands.tCMD_StartAquis);
            serialPortObj.TransmitBytes(CmdBytes, TeensyComCommands.tCMDSize);

  /*          while (nSamplesReceived < totalSamples)
            {
                //os dados estão no buffer circular de tDataObj
                int nBytes = dataObj.countBufferData();
                if (nBytes > 0)
                    for (int i = 0; i < nBytes; i++)
                    {
                        //sample - 2 bytes (MSB first)
                        double val = dataObj.getByteBufferData() << 8;
                        i++;
                        val = val + dataObj.getByteBufferData();
                        //converter para volts
                        val = val * (dataObj.getProperty(TeensyData.tDataPropertiesDouble.AD_Range) /
                                 (Math.Pow(2, dataObj.getProperty(TeensyData.tDataPropertiesDouble.AD_nBits))));
                        //acumular para média
                        sumAvg = sumAvg + val;
                        nSamplesReceived++;
                        //se terminou,
                        //parar coleta de dados.
                        if (nSamplesReceived >= totalSamples)
                        {
                            CmdBytes = ASCIIEncoding.ASCII.GetBytes(TeensyComCommands.tCMD_StopAquis);
                            serialPortObj.TransmitBytes(CmdBytes, TeensyComCommands.tCMDSize);
                            //pare o loop
                            i = nBytes + 1;
                        }
                        label8.Text = "#Samples: " + i.ToString();// + nSamplesReceived.ToString();
                    }
            }
*/
            //para evitar deixar dados no buffer de leitura após esta fase:
            //flush input buffer
            serialPortObj.flushSerialInputBuffer();

            //retornar a média
            return sumAvg / nSamplesReceived;
        } //funcao inacabada que eu preferi fazer a minha propria

        private void TbInicialX_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbInicialX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                TbInicialX_Leave(sender, e);
            }

        }

        private void TbInicialX_Leave(object sender, EventArgs e)
        {
            try
            {
                PiPf[0] = Double.Parse(TbInicialX.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                PiPf[0] = 0;
            }
        }

        private void TbInicialY_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbInicialY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                TbInicialY_Leave(sender, e);
            }
        }

        private void TbInicialY_Leave(object sender, EventArgs e)
        {
            try
            {
                PiPf[1] = Double.Parse(TbInicialY.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                PiPf[1] = 0;
            }

            if (dataObj.ad_Range < PiPf[1])
            {
                MessageBox.Show("A tensao da curva da célula de carga deve ser menor que o range do conversor AD.\nPor favor utiliza uma célula de carga adequada.");
                TbInicialY.Text = "0";
                PiPf[1] = 0;
            }
        }

        private void TbFinalX_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbFinalX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                TbFinalX_Leave(sender, e);
            }
        }

        private void TbFinalX_Leave(object sender, EventArgs e)
        {
            try
            {
                PiPf[2] = Double.Parse(TbFinalX.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                PiPf[2] = 0;
            }

            if (PiPf[2] < PiPf[0])
            {
                MessageBox.Show("A forca do ponto final deve ser maior que a do ponto inicial.\nPor favor verifique se nao esta colocando Pi e Pf na ordem trocada");
                TbFinalY.Text = "0";
                PiPf[2] = 0;
            }
        }

        private void TbFinalY_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void TbFinalY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
                TbFinalY_Leave(sender, e);
            }
        }

        private void TbFinalY_Leave(object sender, EventArgs e)
        {
            try
            {
                PiPf[3] = Double.Parse(TbFinalY.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                PiPf[3] = 0;
            }

            if (dataObj.ad_Range < PiPf[3])
            {
                MessageBox.Show("A tensao da curva da célula de carga deve ser menor que o range do conversor AD.\nPor favor utiliza uma célula de carga adequada.");
                TbFinalY.Text = "0";
                PiPf[3] = 0;
            }
            if (PiPf[3]< PiPf[1])
            {
                MessageBox.Show("A tensao do ponto final deve ser maior que a do ponto inicial.\nPor favor verifique se nao esta colocando Pi e Pf na ordem trocada");
                TbFinalY.Text = "0";
                PiPf[3] = 0;
            }
        }


        //retorna a força media em volts
        private double ForcaMedia(double period)
        {
            //serialPortObj.flushSerialInputBuffer();
            //dataObj.serialDataBC.clear();
            double media = 0.0;
            byte[] oneNumbAsByte = new byte[2];
            int PackSize=0;
            int[] PacoteAtual = new int[10];
            int i, indicePackHeader = 0;
            for (i = 0; i < 10; i++)//inicializando o array
                PacoteAtual[i] = 0;
            i = 0;
            int AmostrasRestantes = (int)(period * session.Sampling_Rate);
            char[] packHeader;
            packHeader = new char[TeensyComCommands.tCMDSize];
            for (i = 0; i < TeensyComCommands.tCMDSize; i++)
                packHeader[i] = '\0'; //inicializando o array
            bool fim = false;
            bool gotPackSize=false;
            bool wait = true;
            serialPortObj.TransmitMsg(TeensyComCommands.tCMD_StartAquis);
            while (wait)
            {
                //aguardar chegada do header desejado no buffer serialDataBC.
                //NOTE QUE tudo que chegar antes será descartado do buffer circular.
                if (dataObj.serialDataBC.count() > 0 && indicePackHeader < TeensyComCommands.tCMDSize)
                {
                    packHeader[indicePackHeader] = (char)dataObj.serialDataBC.read();
                    indicePackHeader++;
                    //chegou header completo?
                    string s = new string(packHeader);
                    if (string.Compare(TeensyComCommands.tCMD_StartAquis, s) == 0 && indicePackHeader == TeensyComCommands.tCMDSize)
                    {//checa se a resposta do teensy eh pro pedido de startAquis, 
                        fim = false;
                        i = 0;
                        while (fim == false)//repete ate ter lido todos os bytes da mensagem
                        {                            
                            if (dataObj.serialDataBC.count() > 0 && i ==0 && gotPackSize==false)
                            { //apenas na primeira iteracao le o primeiro byte depois do "STAQ", que eh o byte que diz o tamanho do pacote enviado
                                oneNumbAsByte[i] = (byte)dataObj.serialDataBC.read();
                                PackSize = (int)oneNumbAsByte[i];
                                gotPackSize = true;
                            }
                            if(dataObj.serialDataBC.count() > 1 && i <PackSize && gotPackSize==true)
                            {//ira ler ate que i alcance o tamanho do pacote de dados inteiro
                                oneNumbAsByte[0] = (byte)dataObj.serialDataBC.read();
                                i++;
                                oneNumbAsByte[1] = (byte)dataObj.serialDataBC.read();
                                i++;
                                short num = (short)((oneNumbAsByte[0]<<8)|oneNumbAsByte[1]);
                                PacoteAtual[(i / 2)-1] = num;
                            }
                            if (i>=PackSize && gotPackSize==true)
                            {
                                for (i = 0; i < (PackSize / 2); i++)
                                {
                                    media = media + PacoteAtual[i];
                                }
                                AmostrasRestantes = AmostrasRestantes - (PackSize / 2);
                                fim = true;
                                indicePackHeader = 0;
                                gotPackSize = false;
                                for (i = 0; i < TeensyComCommands.tCMDSize; i++)
                                {
                                    packHeader[i] = '\0'; //reinicializa o array
                                    s= new string(packHeader);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Ainda não chegou header.
                        //se packHeader já estiver cheio, vamos deslizar os chars para esquerda
                        //e liberar a última posição para continuar tentando completar o header.
                        if (indicePackHeader == TeensyComCommands.tCMDSize)
                        {
                            for (i = 1; i < TeensyComCommands.tCMDSize; i++)
                                packHeader[i - 1] = packHeader[i];
                            packHeader[i - 1] = '\0'; //zera packHeader[ultimaPosicao]
                            indicePackHeader--; //liberar uma posição (ultima) para próximo char do header
                        }
                    }
                }
                if(AmostrasRestantes<=0)
                {
                    serialPortObj.TransmitMsg(TeensyComCommands.tCMD_StopAquis);
                    wait = false;
                }
            }
            //conversão para volts
            media = media / (period * session.Sampling_Rate);
            media = media * (dataObj.ad_Range / Math.Pow(2, dataObj.ad_nBits));
            return media;
        }

    }
}
