namespace SVIPT_BioLab
{
    partial class Calibrar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calibrar));
            this.button_CalibrarLinhaBase = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_ValorLinhaBase = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_CalibForcaMax = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox_LimiteMax = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_LimiteMin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TbFinalY = new System.Windows.Forms.TextBox();
            this.TbInicialY = new System.Windows.Forms.TextBox();
            this.TbFinalX = new System.Windows.Forms.TextBox();
            this.TbInicialX = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label_FMax3 = new System.Windows.Forms.Label();
            this.label_FMax2 = new System.Windows.Forms.Label();
            this.label_FMax1 = new System.Windows.Forms.Label();
            this.textBox_FMaxFinal = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_CalibrarLinhaBase
            // 
            this.button_CalibrarLinhaBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_CalibrarLinhaBase.Location = new System.Drawing.Point(16, 227);
            this.button_CalibrarLinhaBase.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button_CalibrarLinhaBase.Name = "button_CalibrarLinhaBase";
            this.button_CalibrarLinhaBase.Size = new System.Drawing.Size(602, 103);
            this.button_CalibrarLinhaBase.TabIndex = 0;
            this.button_CalibrarLinhaBase.Text = "Calibrar Linha de Base";
            this.button_CalibrarLinhaBase.UseVisualStyleBackColor = true;
            this.button_CalibrarLinhaBase.Click += new System.EventHandler(this.button_CalibrarLinhaBase_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label_ValorLinhaBase);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_CalibrarLinhaBase);
            this.panel1.Location = new System.Drawing.Point(35, 31);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 452);
            this.panel1.TabIndex = 1;
            // 
            // label_ValorLinhaBase
            // 
            this.label_ValorLinhaBase.BackColor = System.Drawing.Color.Black;
            this.label_ValorLinhaBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ValorLinhaBase.ForeColor = System.Drawing.Color.Yellow;
            this.label_ValorLinhaBase.Location = new System.Drawing.Point(277, 346);
            this.label_ValorLinhaBase.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_ValorLinhaBase.Name = "label_ValorLinhaBase";
            this.label_ValorLinhaBase.Size = new System.Drawing.Size(341, 86);
            this.label_ValorLinhaBase.TabIndex = 2;
            this.label_ValorLinhaBase.Text = "0.00";
            this.label_ValorLinhaBase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 370);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 39);
            this.label3.TabIndex = 1;
            this.label3.Text = "Valor atual (V): ";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(610, 203);
            this.label1.TabIndex = 0;
            this.label1.Text = "Linha de Base\r\n\r\nSolicite que o voluntário não exerça pressão sobre o sensor até " +
    "o final da coleta.\r\n\r\nPressione o botão para iniciar.";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.textBox_FMaxFinal);
            this.panel2.Controls.Add(this.label_FMax1);
            this.panel2.Controls.Add(this.label_FMax2);
            this.panel2.Controls.Add(this.label_FMax3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.button_CalibForcaMax);
            this.panel2.Location = new System.Drawing.Point(683, 31);
            this.panel2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1007, 452);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 309);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(241, 39);
            this.label4.TabIndex = 1;
            this.label4.Text = "Valor final (V): ";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 12);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(987, 168);
            this.label5.TabIndex = 0;
            this.label5.Text = "Força Máxima\r\n\r\nSolicite que o voluntário pressione o sensor com força máxima e m" +
    "antenha.\r\n\r\nPressione o botão para iniciar.";
            // 
            // button_CalibForcaMax
            // 
            this.button_CalibForcaMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_CalibForcaMax.Location = new System.Drawing.Point(20, 187);
            this.button_CalibForcaMax.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button_CalibForcaMax.Name = "button_CalibForcaMax";
            this.button_CalibForcaMax.Size = new System.Drawing.Size(975, 103);
            this.button_CalibForcaMax.TabIndex = 0;
            this.button_CalibForcaMax.Text = "Calibrar Força Máxima";
            this.button_CalibForcaMax.UseVisualStyleBackColor = true;
            this.button_CalibForcaMax.Click += new System.EventHandler(this.button_CalibForcaMax_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.textBox_LimiteMax);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.textBox_LimiteMin);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(35, 503);
            this.panel3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(815, 503);
            this.panel3.TabIndex = 3;
            // 
            // textBox_LimiteMax
            // 
            this.textBox_LimiteMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LimiteMax.Location = new System.Drawing.Point(499, 442);
            this.textBox_LimiteMax.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox_LimiteMax.Name = "textBox_LimiteMax";
            this.textBox_LimiteMax.Size = new System.Drawing.Size(215, 45);
            this.textBox_LimiteMax.TabIndex = 4;
            this.textBox_LimiteMax.Text = "45";
            this.textBox_LimiteMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LimiteMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_LimiteMax_KeyDown);
            this.textBox_LimiteMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_LimiteMax_KeyPress);
            this.textBox_LimiteMax.Leave += new System.EventHandler(this.textBox_LimiteMax_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 449);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(410, 39);
            this.label2.TabIndex = 3;
            this.label2.Text = "Limite MÁXIMO (% Fmax)";
            // 
            // textBox_LimiteMin
            // 
            this.textBox_LimiteMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LimiteMin.Location = new System.Drawing.Point(499, 363);
            this.textBox_LimiteMin.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox_LimiteMin.Name = "textBox_LimiteMin";
            this.textBox_LimiteMin.Size = new System.Drawing.Size(215, 45);
            this.textBox_LimiteMin.TabIndex = 2;
            this.textBox_LimiteMin.Text = "35";
            this.textBox_LimiteMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LimiteMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_LimiteMin_KeyDown);
            this.textBox_LimiteMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_LimiteMin_KeyPress);
            this.textBox_LimiteMin.Leave += new System.EventHandler(this.textBox_LimiteMin_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(51, 373);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(399, 39);
            this.label6.TabIndex = 1;
            this.label6.Text = "Limite MÍNIMO (% Fmax)";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(11, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(795, 328);
            this.label7.TabIndex = 0;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(1341, 918);
            this.button_ok.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(355, 88);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "Ok";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(971, 918);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(355, 88);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "Cancelar";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.TbFinalY);
            this.panel4.Controls.Add(this.TbInicialY);
            this.panel4.Controls.Add(this.TbFinalX);
            this.panel4.Controls.Add(this.TbInicialX);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(875, 503);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(815, 400);
            this.panel4.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(544, 205);
            this.label13.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 39);
            this.label13.TabIndex = 9;
            this.label13.Text = "Tensão";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(328, 205);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 39);
            this.label12.TabIndex = 8;
            this.label12.Text = "Força";
            // 
            // TbFinalY
            // 
            this.TbFinalY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbFinalY.Location = new System.Drawing.Point(504, 341);
            this.TbFinalY.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TbFinalY.Name = "TbFinalY";
            this.TbFinalY.Size = new System.Drawing.Size(215, 45);
            this.TbFinalY.TabIndex = 7;
            this.TbFinalY.Text = "3.937";
            this.TbFinalY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbFinalY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbFinalY_KeyDown);
            this.TbFinalY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbFinalY_KeyPress);
            this.TbFinalY.Leave += new System.EventHandler(this.TbFinalY_Leave);
            // 
            // TbInicialY
            // 
            this.TbInicialY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbInicialY.Location = new System.Drawing.Point(504, 262);
            this.TbInicialY.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TbInicialY.Name = "TbInicialY";
            this.TbInicialY.Size = new System.Drawing.Size(215, 45);
            this.TbInicialY.TabIndex = 6;
            this.TbInicialY.Text = "0";
            this.TbInicialY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbInicialY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbInicialY_KeyDown);
            this.TbInicialY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbInicialY_KeyPress);
            this.TbInicialY.Leave += new System.EventHandler(this.TbInicialY_Leave);
            // 
            // TbFinalX
            // 
            this.TbFinalX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbFinalX.Location = new System.Drawing.Point(272, 341);
            this.TbFinalX.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TbFinalX.Name = "TbFinalX";
            this.TbFinalX.Size = new System.Drawing.Size(215, 45);
            this.TbFinalX.TabIndex = 5;
            this.TbFinalX.Text = "17.1361";
            this.TbFinalX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbFinalX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbFinalX_KeyDown);
            this.TbFinalX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbFinalX_KeyPress);
            this.TbFinalX.Leave += new System.EventHandler(this.TbFinalX_Leave);
            // 
            // TbInicialX
            // 
            this.TbInicialX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbInicialX.Location = new System.Drawing.Point(272, 262);
            this.TbInicialX.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TbInicialX.Name = "TbInicialX";
            this.TbInicialX.Size = new System.Drawing.Size(215, 45);
            this.TbInicialX.TabIndex = 4;
            this.TbInicialX.Text = "0.9334";
            this.TbInicialX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbInicialX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbInicialX_KeyDown);
            this.TbInicialX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbInicialX_KeyPress);
            this.TbInicialX.Leave += new System.EventHandler(this.TbInicialX_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(53, 343);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(190, 39);
            this.label11.TabIndex = 3;
            this.label11.Text = "Ponto Final";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(53, 267);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(204, 39);
            this.label10.TabIndex = 2;
            this.label10.Text = "Ponto Inicial";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(792, 153);
            this.label9.TabIndex = 0;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // label_FMax3
            // 
            this.label_FMax3.BackColor = System.Drawing.Color.Black;
            this.label_FMax3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_FMax3.ForeColor = System.Drawing.Color.Yellow;
            this.label_FMax3.Location = new System.Drawing.Point(677, 370);
            this.label_FMax3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_FMax3.Name = "label_FMax3";
            this.label_FMax3.Size = new System.Drawing.Size(316, 62);
            this.label_FMax3.TabIndex = 3;
            this.label_FMax3.Text = "Terceira";
            this.label_FMax3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_FMax2
            // 
            this.label_FMax2.BackColor = System.Drawing.Color.Black;
            this.label_FMax2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_FMax2.ForeColor = System.Drawing.Color.Yellow;
            this.label_FMax2.Location = new System.Drawing.Point(345, 370);
            this.label_FMax2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_FMax2.Name = "label_FMax2";
            this.label_FMax2.Size = new System.Drawing.Size(316, 62);
            this.label_FMax2.TabIndex = 4;
            this.label_FMax2.Text = "Segunda";
            this.label_FMax2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_FMax1
            // 
            this.label_FMax1.BackColor = System.Drawing.Color.Black;
            this.label_FMax1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_FMax1.ForeColor = System.Drawing.Color.Yellow;
            this.label_FMax1.Location = new System.Drawing.Point(13, 370);
            this.label_FMax1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_FMax1.Name = "label_FMax1";
            this.label_FMax1.Size = new System.Drawing.Size(316, 62);
            this.label_FMax1.TabIndex = 5;
            this.label_FMax1.Text = "Primeira";
            this.label_FMax1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_FMaxFinal
            // 
            this.textBox_FMaxFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_FMaxFinal.Location = new System.Drawing.Point(270, 306);
            this.textBox_FMaxFinal.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox_FMaxFinal.Name = "textBox_FMaxFinal";
            this.textBox_FMaxFinal.Size = new System.Drawing.Size(361, 45);
            this.textBox_FMaxFinal.TabIndex = 6;
            this.textBox_FMaxFinal.Text = "0.9334";
            this.textBox_FMaxFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Calibrar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1704, 1024);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Calibrar";
            this.Text = "Calibrar";
            this.Load += new System.EventHandler(this.Calibrar_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_CalibrarLinhaBase;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_ValorLinhaBase;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_CalibForcaMax;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_LimiteMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_LimiteMin;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox TbFinalY;
        private System.Windows.Forms.TextBox TbInicialY;
        private System.Windows.Forms.TextBox TbFinalX;
        private System.Windows.Forms.TextBox TbInicialX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_FMax1;
        private System.Windows.Forms.Label label_FMax2;
        private System.Windows.Forms.Label label_FMax3;
        private System.Windows.Forms.TextBox textBox_FMaxFinal;
    }
}