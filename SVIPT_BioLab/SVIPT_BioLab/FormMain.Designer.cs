using System.Threading;

namespace SVIPT_BioLab
{
    partial class Form_Main
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
            //Encerrando aquisição            
            tSerialPortObj.TransmitMsg(TeensyComCommands.tCMD_StopAquis);
            Thread.Sleep(100);
            tSerialPortObj.TransmitMsg(TeensyComCommands.tCMD_StopAquis);
            Thread.Sleep(100);
            tSerialPortObj.TransmitMsg(TeensyComCommands.tCMD_StopAquis);
            //aguarde alguns milisegundos para o Teensy parar qualquer transmissão
            Thread.Sleep(100);
            //esvaziar buffer de leitura da serial
            tSerialPortObj.flushSerialInputBuffer();
            tDataObj.serialDataBC.clear();

            //Matando as threads
            if (threadAcquisitionHandler.isRunning())
                threadAcquisitionHandler.Stop();
            
            if (threadGUIHandler.isRunning())
                threadGUIHandler.Stop();

            //aguarde alguns milisegundos para o Teensy parar qualquer transmissão
            Thread.Sleep(100);
 
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
            this.components = new System.ComponentModel.Container();
            this.panel_fdbkArea = new System.Windows.Forms.Panel();
            this.TbDebug = new System.Windows.Forms.TextBox();
            this.label_Sequencia = new System.Windows.Forms.Label();
            this.panel_GoStop = new System.Windows.Forms.Panel();
            this.pictureBox_GoStop = new System.Windows.Forms.PictureBox();
            this.label_GoStop = new System.Windows.Forms.Label();
            this.label_T5 = new System.Windows.Forms.Label();
            this.pictureBox_T5 = new System.Windows.Forms.PictureBox();
            this.label_T2 = new System.Windows.Forms.Label();
            this.pictureBox_T2_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_T2_Left = new System.Windows.Forms.PictureBox();
            this.label_T3 = new System.Windows.Forms.Label();
            this.label_T1 = new System.Windows.Forms.Label();
            this.pictureBox_T3_Right = new System.Windows.Forms.PictureBox();
            this.label_T4 = new System.Windows.Forms.Label();
            this.pictureBox_T3_Left = new System.Windows.Forms.PictureBox();
            this.pictureBox_T1_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_T1_Left = new System.Windows.Forms.PictureBox();
            this.pictureBox_T4_Right = new System.Windows.Forms.PictureBox();
            this.pictureBox_T4_Left = new System.Windows.Forms.PictureBox();
            this.pictureBox_Cursor = new System.Windows.Forms.PictureBox();
            this.label_Home = new System.Windows.Forms.Label();
            this.pictureBox_Thome = new System.Windows.Forms.PictureBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuConectar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTS_Sessao = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTS_SessaoNova = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTS_SessaoSalvar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTS_Calibrar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTS_MapearSessao = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTS_iniciarAquisi = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarAquisiçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pararToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTS_AnalizarSessao = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_fdbkArea.SuspendLayout();
            this.panel_GoStop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_GoStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T2_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T2_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T3_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T3_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T1_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T1_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T4_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T4_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cursor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Thome)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_fdbkArea
            // 
            this.panel_fdbkArea.BackColor = System.Drawing.Color.White;
            this.panel_fdbkArea.Controls.Add(this.TbDebug);
            this.panel_fdbkArea.Controls.Add(this.label_Sequencia);
            this.panel_fdbkArea.Controls.Add(this.panel_GoStop);
            this.panel_fdbkArea.Controls.Add(this.label_T5);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T5);
            this.panel_fdbkArea.Controls.Add(this.label_T2);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T2_Right);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T2_Left);
            this.panel_fdbkArea.Controls.Add(this.label_T3);
            this.panel_fdbkArea.Controls.Add(this.label_T1);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T3_Right);
            this.panel_fdbkArea.Controls.Add(this.label_T4);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T3_Left);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T1_Right);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T1_Left);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T4_Right);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_T4_Left);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_Cursor);
            this.panel_fdbkArea.Controls.Add(this.label_Home);
            this.panel_fdbkArea.Controls.Add(this.pictureBox_Thome);
            this.panel_fdbkArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_fdbkArea.Location = new System.Drawing.Point(0, 24);
            this.panel_fdbkArea.MinimumSize = new System.Drawing.Size(860, 450);
            this.panel_fdbkArea.Name = "panel_fdbkArea";
            this.panel_fdbkArea.Size = new System.Drawing.Size(860, 450);
            this.panel_fdbkArea.TabIndex = 0;
            this.panel_fdbkArea.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_fdbkArea_Paint);
            this.panel_fdbkArea.Resize += new System.EventHandler(this.panel_fdbkArea_Resize);
            // 
            // TbDebug
            // 
            this.TbDebug.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.TbDebug.Location = new System.Drawing.Point(15, 395);
            this.TbDebug.Margin = new System.Windows.Forms.Padding(1);
            this.TbDebug.Name = "TbDebug";
            this.TbDebug.Size = new System.Drawing.Size(617, 20);
            this.TbDebug.TabIndex = 24;
            this.TbDebug.Visible = false;
            // 
            // label_Sequencia
            // 
            this.label_Sequencia.AutoSize = true;
            this.label_Sequencia.BackColor = System.Drawing.Color.Transparent;
            this.label_Sequencia.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Sequencia.Location = new System.Drawing.Point(137, 25);
            this.label_Sequencia.Name = "label_Sequencia";
            this.label_Sequencia.Size = new System.Drawing.Size(682, 23);
            this.label_Sequencia.TabIndex = 23;
            this.label_Sequencia.Text = "Home - 1 - Home - 2 - Home - 3 - Home - 4 - Home - 5 - Home";
            this.label_Sequencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_GoStop
            // 
            this.panel_GoStop.Controls.Add(this.pictureBox_GoStop);
            this.panel_GoStop.Controls.Add(this.label_GoStop);
            this.panel_GoStop.Location = new System.Drawing.Point(321, 338);
            this.panel_GoStop.Name = "panel_GoStop";
            this.panel_GoStop.Size = new System.Drawing.Size(269, 46);
            this.panel_GoStop.TabIndex = 22;
            this.panel_GoStop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_GoStop_MouseClick);
            // 
            // pictureBox_GoStop
            // 
            this.pictureBox_GoStop.BackColor = System.Drawing.Color.LimeGreen;
            this.pictureBox_GoStop.Location = new System.Drawing.Point(3, 4);
            this.pictureBox_GoStop.Name = "pictureBox_GoStop";
            this.pictureBox_GoStop.Size = new System.Drawing.Size(44, 37);
            this.pictureBox_GoStop.TabIndex = 19;
            this.pictureBox_GoStop.TabStop = false;
            this.pictureBox_GoStop.Click += new System.EventHandler(this.pictureBox_GoStop_Click);
            // 
            // label_GoStop
            // 
            this.label_GoStop.AutoSize = true;
            this.label_GoStop.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GoStop.Location = new System.Drawing.Point(53, 8);
            this.label_GoStop.Name = "label_GoStop";
            this.label_GoStop.Size = new System.Drawing.Size(126, 29);
            this.label_GoStop.TabIndex = 18;
            this.label_GoStop.Text = "Go/Stop";
            this.label_GoStop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_GoStop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label_GoStop_MouseClick);
            // 
            // label_T5
            // 
            this.label_T5.AutoSize = true;
            this.label_T5.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_T5.Location = new System.Drawing.Point(584, 96);
            this.label_T5.Name = "label_T5";
            this.label_T5.Size = new System.Drawing.Size(30, 29);
            this.label_T5.TabIndex = 16;
            this.label_T5.Text = "5";
            // 
            // pictureBox_T5
            // 
            this.pictureBox_T5.BackColor = System.Drawing.Color.Maroon;
            this.pictureBox_T5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T5.Location = new System.Drawing.Point(573, 174);
            this.pictureBox_T5.Name = "pictureBox_T5";
            this.pictureBox_T5.Size = new System.Drawing.Size(50, 60);
            this.pictureBox_T5.TabIndex = 15;
            this.pictureBox_T5.TabStop = false;
            // 
            // label_T2
            // 
            this.label_T2.AutoSize = true;
            this.label_T2.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_T2.Location = new System.Drawing.Point(692, 96);
            this.label_T2.Name = "label_T2";
            this.label_T2.Size = new System.Drawing.Size(30, 29);
            this.label_T2.TabIndex = 14;
            this.label_T2.Text = "2";
            // 
            // pictureBox_T2_Right
            // 
            this.pictureBox_T2_Right.BackColor = System.Drawing.Color.Indigo;
            this.pictureBox_T2_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T2_Right.Location = new System.Drawing.Point(721, 132);
            this.pictureBox_T2_Right.Name = "pictureBox_T2_Right";
            this.pictureBox_T2_Right.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T2_Right.TabIndex = 13;
            this.pictureBox_T2_Right.TabStop = false;
            // 
            // pictureBox_T2_Left
            // 
            this.pictureBox_T2_Left.BackColor = System.Drawing.Color.Indigo;
            this.pictureBox_T2_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T2_Left.Location = new System.Drawing.Point(665, 132);
            this.pictureBox_T2_Left.Name = "pictureBox_T2_Left";
            this.pictureBox_T2_Left.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T2_Left.TabIndex = 12;
            this.pictureBox_T2_Left.TabStop = false;
            // 
            // label_T3
            // 
            this.label_T3.AutoSize = true;
            this.label_T3.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_T3.Location = new System.Drawing.Point(479, 96);
            this.label_T3.Name = "label_T3";
            this.label_T3.Size = new System.Drawing.Size(30, 29);
            this.label_T3.TabIndex = 11;
            this.label_T3.Text = "3";
            // 
            // label_T1
            // 
            this.label_T1.AutoSize = true;
            this.label_T1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_T1.Location = new System.Drawing.Point(349, 96);
            this.label_T1.Name = "label_T1";
            this.label_T1.Size = new System.Drawing.Size(30, 29);
            this.label_T1.TabIndex = 8;
            this.label_T1.Text = "1";
            // 
            // pictureBox_T3_Right
            // 
            this.pictureBox_T3_Right.BackColor = System.Drawing.Color.Green;
            this.pictureBox_T3_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T3_Right.Location = new System.Drawing.Point(507, 132);
            this.pictureBox_T3_Right.Name = "pictureBox_T3_Right";
            this.pictureBox_T3_Right.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T3_Right.TabIndex = 10;
            this.pictureBox_T3_Right.TabStop = false;
            // 
            // label_T4
            // 
            this.label_T4.AutoSize = true;
            this.label_T4.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_T4.Location = new System.Drawing.Point(210, 96);
            this.label_T4.Name = "label_T4";
            this.label_T4.Size = new System.Drawing.Size(30, 29);
            this.label_T4.TabIndex = 7;
            this.label_T4.Text = "4";
            // 
            // pictureBox_T3_Left
            // 
            this.pictureBox_T3_Left.BackColor = System.Drawing.Color.Green;
            this.pictureBox_T3_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T3_Left.Location = new System.Drawing.Point(451, 132);
            this.pictureBox_T3_Left.Name = "pictureBox_T3_Left";
            this.pictureBox_T3_Left.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T3_Left.TabIndex = 9;
            this.pictureBox_T3_Left.TabStop = false;
            // 
            // pictureBox_T1_Right
            // 
            this.pictureBox_T1_Right.BackColor = System.Drawing.Color.Tomato;
            this.pictureBox_T1_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T1_Right.Location = new System.Drawing.Point(377, 132);
            this.pictureBox_T1_Right.Name = "pictureBox_T1_Right";
            this.pictureBox_T1_Right.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T1_Right.TabIndex = 6;
            this.pictureBox_T1_Right.TabStop = false;
            // 
            // pictureBox_T1_Left
            // 
            this.pictureBox_T1_Left.BackColor = System.Drawing.Color.Tomato;
            this.pictureBox_T1_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T1_Left.Location = new System.Drawing.Point(321, 132);
            this.pictureBox_T1_Left.Name = "pictureBox_T1_Left";
            this.pictureBox_T1_Left.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T1_Left.TabIndex = 5;
            this.pictureBox_T1_Left.TabStop = false;
            // 
            // pictureBox_T4_Right
            // 
            this.pictureBox_T4_Right.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pictureBox_T4_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T4_Right.Location = new System.Drawing.Point(240, 132);
            this.pictureBox_T4_Right.Name = "pictureBox_T4_Right";
            this.pictureBox_T4_Right.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T4_Right.TabIndex = 4;
            this.pictureBox_T4_Right.TabStop = false;
            // 
            // pictureBox_T4_Left
            // 
            this.pictureBox_T4_Left.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.pictureBox_T4_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_T4_Left.Location = new System.Drawing.Point(182, 132);
            this.pictureBox_T4_Left.Name = "pictureBox_T4_Left";
            this.pictureBox_T4_Left.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_T4_Left.TabIndex = 3;
            this.pictureBox_T4_Left.TabStop = false;
            // 
            // pictureBox_Cursor
            // 
            this.pictureBox_Cursor.BackColor = System.Drawing.Color.Black;
            this.pictureBox_Cursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_Cursor.ErrorImage = null;
            this.pictureBox_Cursor.Location = new System.Drawing.Point(66, 198);
            this.pictureBox_Cursor.Name = "pictureBox_Cursor";
            this.pictureBox_Cursor.Size = new System.Drawing.Size(25, 30);
            this.pictureBox_Cursor.TabIndex = 2;
            this.pictureBox_Cursor.TabStop = false;
            // 
            // label_Home
            // 
            this.label_Home.AutoSize = true;
            this.label_Home.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Home.Location = new System.Drawing.Point(10, 96);
            this.label_Home.Name = "label_Home";
            this.label_Home.Size = new System.Drawing.Size(90, 29);
            this.label_Home.TabIndex = 1;
            this.label_Home.Text = "Home";
            this.label_Home.Visible = false;
            // 
            // pictureBox_Thome
            // 
            this.pictureBox_Thome.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox_Thome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Thome.Location = new System.Drawing.Point(35, 132);
            this.pictureBox_Thome.Name = "pictureBox_Thome";
            this.pictureBox_Thome.Size = new System.Drawing.Size(25, 150);
            this.pictureBox_Thome.TabIndex = 0;
            this.pictureBox_Thome.TabStop = false;
            this.pictureBox_Thome.Visible = false;
            // 
            // label_Status
            // 
            this.label_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label_Status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_Status.Location = new System.Drawing.Point(0, 425);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(659, 30);
            this.label_Status.TabIndex = 1;
            this.label_Status.Text = "Status";
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuConectar,
            this.menuTS_Sessao,
            this.menuTS_Calibrar,
            this.menuTS_MapearSessao,
            this.menuTS_iniciarAquisi,
            this.MenuTS_AnalizarSessao});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(659, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuConectar
            // 
            this.MenuConectar.Name = "MenuConectar";
            this.MenuConectar.Size = new System.Drawing.Size(67, 20);
            this.MenuConectar.Text = "Conectar";
            this.MenuConectar.Click += new System.EventHandler(this.MenuConectar_Click);
            // 
            // menuTS_Sessao
            // 
            this.menuTS_Sessao.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTS_SessaoNova,
            this.toolStripSeparator1,
            this.menuTS_SessaoSalvar});
            this.menuTS_Sessao.Name = "menuTS_Sessao";
            this.menuTS_Sessao.Size = new System.Drawing.Size(87, 20);
            this.menuTS_Sessao.Text = "Sessão SVIPT";
            // 
            // menuTS_SessaoNova
            // 
            this.menuTS_SessaoNova.Name = "menuTS_SessaoNova";
            this.menuTS_SessaoNova.Size = new System.Drawing.Size(130, 22);
            this.menuTS_SessaoNova.Text = "Criar Nova";
            this.menuTS_SessaoNova.Click += new System.EventHandler(this.menuTS_SessaoNova_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(127, 6);
            // 
            // menuTS_SessaoSalvar
            // 
            this.menuTS_SessaoSalvar.Name = "menuTS_SessaoSalvar";
            this.menuTS_SessaoSalvar.Size = new System.Drawing.Size(130, 22);
            this.menuTS_SessaoSalvar.Text = "Salvar";
            this.menuTS_SessaoSalvar.Click += new System.EventHandler(this.menuTS_SessaoSalvar_Click);
            // 
            // menuTS_Calibrar
            // 
            this.menuTS_Calibrar.Name = "menuTS_Calibrar";
            this.menuTS_Calibrar.Size = new System.Drawing.Size(92, 20);
            this.menuTS_Calibrar.Text = "Calibrar Força";
            this.menuTS_Calibrar.Click += new System.EventHandler(this.menuTS_Calibrar_Click);
            // 
            // menuTS_MapearSessao
            // 
            this.menuTS_MapearSessao.Name = "menuTS_MapearSessao";
            this.menuTS_MapearSessao.Size = new System.Drawing.Size(92, 20);
            this.menuTS_MapearSessao.Text = "Definir Sessão";
            this.menuTS_MapearSessao.Click += new System.EventHandler(this.menuTS_Mapear_Click);
            // 
            // menuTS_iniciarAquisi
            // 
            this.menuTS_iniciarAquisi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarAquisiçãoToolStripMenuItem,
            this.pararToolStripMenuItem});
            this.menuTS_iniciarAquisi.Name = "menuTS_iniciarAquisi";
            this.menuTS_iniciarAquisi.Size = new System.Drawing.Size(71, 20);
            this.menuTS_iniciarAquisi.Text = "Aquisição";
            this.menuTS_iniciarAquisi.Click += new System.EventHandler(this.menuTS_iniciarAquisi_Click);
            // 
            // iniciarAquisiçãoToolStripMenuItem
            // 
            this.iniciarAquisiçãoToolStripMenuItem.Name = "iniciarAquisiçãoToolStripMenuItem";
            this.iniciarAquisiçãoToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.iniciarAquisiçãoToolStripMenuItem.Text = "Iniciar";
            this.iniciarAquisiçãoToolStripMenuItem.Click += new System.EventHandler(this.iniciarAquisiçãoToolStripMenuItem_Click);
            // 
            // pararToolStripMenuItem
            // 
            this.pararToolStripMenuItem.Name = "pararToolStripMenuItem";
            this.pararToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.pararToolStripMenuItem.Text = "Parar";
            this.pararToolStripMenuItem.Click += new System.EventHandler(this.pararToolStripMenuItem_Click);
            // 
            // MenuTS_AnalizarSessao
            // 
            this.MenuTS_AnalizarSessao.Name = "MenuTS_AnalizarSessao";
            this.MenuTS_AnalizarSessao.Size = new System.Drawing.Size(99, 20);
            this.MenuTS_AnalizarSessao.Text = "Analisar Sessao";
            this.MenuTS_AnalizarSessao.Click += new System.EventHandler(this.MenuTS_AnalizarSessao_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 455);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.panel_fdbkArea);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(479, 307);
            this.Name = "Form_Main";
            this.Text = "SVIPT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_fdbkArea.ResumeLayout(false);
            this.panel_fdbkArea.PerformLayout();
            this.panel_GoStop.ResumeLayout(false);
            this.panel_GoStop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_GoStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T2_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T2_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T3_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T3_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T1_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T1_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T4_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_T4_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cursor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Thome)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_fdbkArea;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Label label_Home;
        private System.Windows.Forms.PictureBox pictureBox_Thome;
        private System.Windows.Forms.PictureBox pictureBox_T1_Left;
        private System.Windows.Forms.PictureBox pictureBox_T4_Right;
        private System.Windows.Forms.PictureBox pictureBox_T4_Left;
        private System.Windows.Forms.PictureBox pictureBox_Cursor;
        private System.Windows.Forms.Label label_T5;
        private System.Windows.Forms.PictureBox pictureBox_T5;
        private System.Windows.Forms.Label label_T2;
        private System.Windows.Forms.PictureBox pictureBox_T2_Right;
        private System.Windows.Forms.PictureBox pictureBox_T2_Left;
        private System.Windows.Forms.Label label_T3;
        private System.Windows.Forms.Label label_T1;
        private System.Windows.Forms.PictureBox pictureBox_T3_Right;
        private System.Windows.Forms.Label label_T4;
        private System.Windows.Forms.PictureBox pictureBox_T3_Left;
        private System.Windows.Forms.PictureBox pictureBox_T1_Right;
        private System.Windows.Forms.PictureBox pictureBox_GoStop;
        private System.Windows.Forms.Label label_GoStop;
        private System.Windows.Forms.Panel panel_GoStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuTS_Calibrar;
        private System.Windows.Forms.ToolStripMenuItem menuTS_MapearSessao;
        private System.Windows.Forms.ToolStripMenuItem menuTS_Sessao;
        private System.Windows.Forms.ToolStripMenuItem menuTS_SessaoNova;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuTS_SessaoSalvar;
        private System.Windows.Forms.ToolStripMenuItem MenuConectar;
        private System.Windows.Forms.ToolStripMenuItem menuTS_iniciarAquisi;
        private System.Windows.Forms.ToolStripMenuItem MenuTS_AnalizarSessao;
        private System.Windows.Forms.Label label_Sequencia;
        private System.Windows.Forms.ToolStripMenuItem iniciarAquisiçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pararToolStripMenuItem;
        private System.Windows.Forms.TextBox TbDebug;
    }
}

