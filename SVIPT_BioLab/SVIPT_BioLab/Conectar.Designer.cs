namespace SVIPT_BioLab
{
    partial class Conectar
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
            this.CBoxCOMs = new System.Windows.Forms.ComboBox();
            this.BtOk = new System.Windows.Forms.Button();
            this.LabelConectar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CBoxCOMs
            // 
            this.CBoxCOMs.FormattingEnabled = true;
            this.CBoxCOMs.Location = new System.Drawing.Point(12, 12);
            this.CBoxCOMs.Name = "CBoxCOMs";
            this.CBoxCOMs.Size = new System.Drawing.Size(618, 39);
            this.CBoxCOMs.TabIndex = 1;
            this.CBoxCOMs.SelectedIndexChanged += new System.EventHandler(this.CBoxCOMs_SelectedIndexChanged);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(958, 381);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(310, 88);
            this.BtOk.TabIndex = 2;
            this.BtOk.Text = "Ok";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // LabelConectar
            // 
            this.LabelConectar.AutoSize = true;
            this.LabelConectar.Location = new System.Drawing.Point(12, 390);
            this.LabelConectar.Name = "LabelConectar";
            this.LabelConectar.Size = new System.Drawing.Size(826, 32);
            this.LabelConectar.TabIndex = 3;
            this.LabelConectar.Text = "Por favor escolha a porta COM na qual o Teensy esta conectado";
            // 
            // Conectar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 481);
            this.Controls.Add(this.LabelConectar);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.CBoxCOMs);
            this.Name = "Conectar";
            this.Text = "Conectar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CBoxCOMs;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Label LabelConectar;
    }
}