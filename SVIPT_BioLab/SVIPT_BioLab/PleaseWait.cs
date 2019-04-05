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
    public partial class PleaseWait : Form
    {
        public PleaseWait(string msg)
        {
            InitializeComponent();

            label_wait.Text = msg;
        }

        private void PleaseWait_Load(object sender, EventArgs e)
        {

        }

    }
}
