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
    public partial class MapSession : Form
    {
        Session session;

        public MapSession(ref Session _session)
        {
            InitializeComponent();
            session = _session;
        }

        private void MapSession_Load(object sender, EventArgs e)
        {
            textBox_Repetitions.Text = session.SVIPT_TaskRepetitions.ToString();
            textBox_ErrorPercent.Text = session.SVIPT_TaskOscilationPercent.ToString();
            if (session.SVIPT_Task == sviptConstant.sviptTaskOriginal)
                radioButton_SVIPToriginal.Checked = true;
            else if (session.SVIPT_Task == sviptConstant.sviptTaskModifed)
                radioButton_SVIPTmodificado.Checked = true;
            else if (session.SVIPT_Task == sviptConstant.sviptTaskModifed2)
                radioButton_SVIPTmodificado2.Checked = true;
            else if (session.SVIPT_Task == sviptConstant.sviptTaskLinear)
                radioButton_SVIPTB.Checked = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            //salvar alterações na sessão
            try
            {
                session.SVIPT_TaskRepetitions = Convert.ToInt32(textBox_Repetitions.Text);
                session.SVIPT_TaskOscilationPercent = Convert.ToInt32(textBox_ErrorPercent.Text);
                if (radioButton_SVIPToriginal.Checked)
                    session.SVIPT_Task = sviptConstant.sviptTaskOriginal;
                if (radioButton_SVIPTmodificado.Checked)
                    session.SVIPT_Task = sviptConstant.sviptTaskModifed;
                if (radioButton_SVIPTmodificado2.Checked)
                    session.SVIPT_Task = sviptConstant.sviptTaskModifed2;
                if (radioButton_SVIPTB.Checked)
                    session.SVIPT_Task = sviptConstant.sviptTaskLinear;

                if (radioButton_CopiarCorDoAlvo.Checked)
                    session.ColorChangeEnabled = true;
                else
                    session.ColorChangeEnabled = false;

                //Atualizar o número de repetições
                session.setTaskRepetitions(Convert.ToInt32(textBox_Repetitions.Text));
                
                //Definir o percentual de força máximo a ser usado na tarefa, dependendo
                //do tipo de tarefa selecionado.
                session.setTaskMaxForcePercentage();
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

        private void textBox_Repetitions_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox_ErrorPercent_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
