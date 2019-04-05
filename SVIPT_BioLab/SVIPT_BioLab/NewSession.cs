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
    public partial class NewSession : Form
    {
        Session session;

        public NewSession(ref Session _session)
        {
            InitializeComponent();

            session = _session;
        }

        private void NewSession_Load(object sender, EventArgs e)
        {
            textBox_IdSession.Text = session.id;
            textBox_IdVolunteer.Text = session.ID_Volunteer;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            //Simplemente feche a janela - não salvar informações
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            //guardar informações no objeto da sessão
            session.id = textBox_IdSession.Text;
            session.ID_Volunteer = textBox_IdVolunteer.Text;
            //feche a janela
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
