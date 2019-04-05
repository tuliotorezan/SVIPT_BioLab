using System;
using System.IO.Ports;
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
    public partial class Conectar : Form
    {
        public Form_Main formMain;
        private string pName;
        //Objeto para comunicação serial
        private TeensyUsbCom serialPortObj;
        //Este objeto contem os diversos elementos associados ao equipamento,
        //firmware, status, dados de experimento etc.
        public Conectar(ref Session _session, ref TeensyUsbCom _serialPortObj,
                        ref TeensyData _dataObj)
        {
            string[] ports = SerialPort.GetPortNames();
            
            InitializeComponent();
            CBoxCOMs.Items.Clear();
            CBoxCOMs.Items.AddRange(ports);
            serialPortObj = _serialPortObj;
        }

        private void CBoxCOMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            pName = this.CBoxCOMs.Text;//coloca como opcao do combobox
            serialPortObj.CloseConn();//, todas as portas COM disponiveis
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            if (pName != null)
            {
                bool resultado = serialPortObj.OpenConnTeensy(pName);
                this.formMain.continua(resultado);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
