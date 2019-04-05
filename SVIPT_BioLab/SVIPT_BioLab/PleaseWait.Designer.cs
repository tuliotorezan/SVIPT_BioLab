namespace SVIPT_BioLab
{
    partial class PleaseWait
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
            this.label_wait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_wait
            // 
            this.label_wait.Location = new System.Drawing.Point(13, 20);
            this.label_wait.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_wait.Name = "label_wait";
            this.label_wait.Size = new System.Drawing.Size(565, 166);
            this.label_wait.TabIndex = 0;
            this.label_wait.Text = "Por favor, aguarde...";
            // 
            // PleaseWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 203);
            this.Controls.Add(this.label_wait);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PleaseWait";
            this.Text = "PleaseWait";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PleaseWait_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_wait;
    }
}