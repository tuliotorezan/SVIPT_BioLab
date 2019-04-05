namespace SVIPT_BioLab
{
    partial class MapSession
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSession));
            this.radioButton_SVIPToriginal = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_SVIPTmodificado = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_ErrorPercent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Repetitions = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.radioButton_CopiarCorDoAlvo = new System.Windows.Forms.RadioButton();
            this.radioButton_SVIPTmodificado2 = new System.Windows.Forms.RadioButton();
            this.radioButton_SVIPTB = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton_SVIPToriginal
            // 
            this.radioButton_SVIPToriginal.AutoSize = true;
            this.radioButton_SVIPToriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SVIPToriginal.Location = new System.Drawing.Point(12, 27);
            this.radioButton_SVIPToriginal.Name = "radioButton_SVIPToriginal";
            this.radioButton_SVIPToriginal.Size = new System.Drawing.Size(361, 17);
            this.radioButton_SVIPToriginal.TabIndex = 1;
            this.radioButton_SVIPToriginal.TabStop = true;
            this.radioButton_SVIPToriginal.Text = "SVIPT original (mapeamento de força no centro dos limites)";
            this.radioButton_SVIPToriginal.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_SVIPTB);
            this.groupBox1.Controls.Add(this.radioButton_SVIPTmodificado2);
            this.groupBox1.Controls.Add(this.radioButton_SVIPTmodificado);
            this.groupBox1.Controls.Add(this.radioButton_SVIPToriginal);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 146);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mapeamento SVIPT";
            // 
            // radioButton_SVIPTmodificado
            // 
            this.radioButton_SVIPTmodificado.AutoSize = true;
            this.radioButton_SVIPTmodificado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SVIPTmodificado.Location = new System.Drawing.Point(12, 53);
            this.radioButton_SVIPTmodificado.Name = "radioButton_SVIPTmodificado";
            this.radioButton_SVIPTmodificado.Size = new System.Drawing.Size(416, 17);
            this.radioButton_SVIPTmodificado.TabIndex = 3;
            this.radioButton_SVIPTmodificado.TabStop = true;
            this.radioButton_SVIPTmodificado.Text = "SVIPT modificado (mapeamento de força aleatório dentro dos limites)";
            this.radioButton_SVIPTmodificado.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 229);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 188);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox_ErrorPercent);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_Repetitions);
            this.panel1.Location = new System.Drawing.Point(7, 159);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 41);
            this.panel1.TabIndex = 4;
            // 
            // textBox_ErrorPercent
            // 
            this.textBox_ErrorPercent.Location = new System.Drawing.Point(334, 9);
            this.textBox_ErrorPercent.Name = "textBox_ErrorPercent";
            this.textBox_ErrorPercent.Size = new System.Drawing.Size(68, 20);
            this.textBox_ErrorPercent.TabIndex = 3;
            this.textBox_ErrorPercent.Text = "5";
            this.textBox_ErrorPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_ErrorPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ErrorPercent_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Oscilação aceitavel (%)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Repetições da tarefa";
            // 
            // textBox_Repetitions
            // 
            this.textBox_Repetitions.Location = new System.Drawing.Point(119, 9);
            this.textBox_Repetitions.Name = "textBox_Repetitions";
            this.textBox_Repetitions.Size = new System.Drawing.Size(68, 20);
            this.textBox_Repetitions.TabIndex = 0;
            this.textBox_Repetitions.Text = "30";
            this.textBox_Repetitions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Repetitions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Repetitions_KeyPress);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(344, 339);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(95, 38);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(344, 285);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(95, 38);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // radioButton_CopiarCorDoAlvo
            // 
            this.radioButton_CopiarCorDoAlvo.AutoSize = true;
            this.radioButton_CopiarCorDoAlvo.Checked = true;
            this.radioButton_CopiarCorDoAlvo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_CopiarCorDoAlvo.Location = new System.Drawing.Point(20, 206);
            this.radioButton_CopiarCorDoAlvo.Name = "radioButton_CopiarCorDoAlvo";
            this.radioButton_CopiarCorDoAlvo.Size = new System.Drawing.Size(222, 17);
            this.radioButton_CopiarCorDoAlvo.TabIndex = 7;
            this.radioButton_CopiarCorDoAlvo.TabStop = true;
            this.radioButton_CopiarCorDoAlvo.Text = "Cursor copia a cor do proximo alvo";
            this.radioButton_CopiarCorDoAlvo.UseVisualStyleBackColor = true;
            // 
            // radioButton_SVIPTmodificado2
            // 
            this.radioButton_SVIPTmodificado2.AutoSize = true;
            this.radioButton_SVIPTmodificado2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SVIPTmodificado2.Location = new System.Drawing.Point(12, 76);
            this.radioButton_SVIPTmodificado2.Name = "radioButton_SVIPTmodificado2";
            this.radioButton_SVIPTmodificado2.Size = new System.Drawing.Size(427, 17);
            this.radioButton_SVIPTmodificado2.TabIndex = 4;
            this.radioButton_SVIPTmodificado2.TabStop = true;
            this.radioButton_SVIPTmodificado2.Text = "SVIPT modificado 2 (mapeamento de força aleatório dentro dos limites)";
            this.radioButton_SVIPTmodificado2.UseVisualStyleBackColor = true;
            // 
            // radioButton_SVIPTB
            // 
            this.radioButton_SVIPTB.AutoSize = true;
            this.radioButton_SVIPTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SVIPTB.Location = new System.Drawing.Point(12, 99);
            this.radioButton_SVIPTB.Name = "radioButton_SVIPTB";
            this.radioButton_SVIPTB.Size = new System.Drawing.Size(295, 17);
            this.radioButton_SVIPTB.TabIndex = 5;
            this.radioButton_SVIPTB.TabStop = true;
            this.radioButton_SVIPTB.Text = "SVIPT modificado (mapeamento de força linear)";
            this.radioButton_SVIPTB.UseVisualStyleBackColor = true;
            // 
            // MapSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 429);
            this.Controls.Add(this.radioButton_CopiarCorDoAlvo);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MapSession";
            this.Text = "Sessão";
            this.Load += new System.EventHandler(this.MapSession_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton_SVIPToriginal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_SVIPTmodificado;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Repetitions;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_ErrorPercent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton_CopiarCorDoAlvo;
        private System.Windows.Forms.RadioButton radioButton_SVIPTB;
        private System.Windows.Forms.RadioButton radioButton_SVIPTmodificado2;
    }
}