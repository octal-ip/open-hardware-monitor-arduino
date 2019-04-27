namespace ohm_arduino_gui
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.intervalComboBox1 = new System.Windows.Forms.ComboBox();
            this.comPortCombo = new System.Windows.Forms.ComboBox();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpuTempBox = new System.Windows.Forms.TextBox();
            this.gpuTempBox = new System.Windows.Forms.TextBox();
            this.cpuUtilBox = new System.Windows.Forms.TextBox();
            this.ramUtilBox = new System.Windows.Forms.TextBox();
            this.cpuTempLabel1 = new System.Windows.Forms.Label();
            this.gpuTempLabel1 = new System.Windows.Forms.Label();
            this.cpuUsageLabel1 = new System.Windows.Forms.Label();
            this.ramUsageLabel1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.baudComboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.outputComboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.adminLinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gradientTrackBar1 = new System.Windows.Forms.TrackBar();
            this.notifyIconMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTimer
            // 
            this.mainTimer.Interval = 1000;
            this.mainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // intervalComboBox1
            // 
            this.intervalComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.intervalComboBox1.FormattingEnabled = true;
            this.intervalComboBox1.Items.AddRange(new object[] {
            100,
            200,
            500,
            1000,
            2000});
            this.intervalComboBox1.Location = new System.Drawing.Point(415, 190);
            this.intervalComboBox1.Name = "intervalComboBox1";
            this.intervalComboBox1.Size = new System.Drawing.Size(89, 21);
            this.intervalComboBox1.TabIndex = 13;
            this.intervalComboBox1.SelectedIndexChanged += new System.EventHandler(this.intervalComboBox1_SelectedIndexChanged);
            // 
            // comPortCombo
            // 
            this.comPortCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comPortCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPortCombo.FormattingEnabled = true;
            this.comPortCombo.Location = new System.Drawing.Point(12, 17);
            this.comPortCombo.Name = "comPortCombo";
            this.comPortCombo.Size = new System.Drawing.Size(293, 21);
            this.comPortCombo.TabIndex = 1;
            this.comPortCombo.SelectedIndexChanged += new System.EventHandler(this.comPortCombo_SelectedIndexChanged);
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(12, 48);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.logBox.Size = new System.Drawing.Size(293, 218);
            this.logBox.TabIndex = 2;
            this.logBox.Text = "";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "OHM - Arduino";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(149, 48);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.showWindowToolStripMenuItem.Text = "Show window";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.showWindowToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cpuTempBox
            // 
            this.cpuTempBox.Location = new System.Drawing.Point(415, 106);
            this.cpuTempBox.Name = "cpuTempBox";
            this.cpuTempBox.ReadOnly = true;
            this.cpuTempBox.Size = new System.Drawing.Size(89, 20);
            this.cpuTempBox.TabIndex = 3;
            this.cpuTempBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gpuTempBox
            // 
            this.gpuTempBox.Location = new System.Drawing.Point(415, 135);
            this.gpuTempBox.Name = "gpuTempBox";
            this.gpuTempBox.ReadOnly = true;
            this.gpuTempBox.Size = new System.Drawing.Size(89, 20);
            this.gpuTempBox.TabIndex = 4;
            this.gpuTempBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cpuUtilBox
            // 
            this.cpuUtilBox.Location = new System.Drawing.Point(415, 51);
            this.cpuUtilBox.Name = "cpuUtilBox";
            this.cpuUtilBox.ReadOnly = true;
            this.cpuUtilBox.Size = new System.Drawing.Size(89, 20);
            this.cpuUtilBox.TabIndex = 5;
            this.cpuUtilBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ramUtilBox
            // 
            this.ramUtilBox.Location = new System.Drawing.Point(415, 77);
            this.ramUtilBox.Name = "ramUtilBox";
            this.ramUtilBox.ReadOnly = true;
            this.ramUtilBox.Size = new System.Drawing.Size(89, 20);
            this.ramUtilBox.TabIndex = 6;
            this.ramUtilBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cpuTempLabel1
            // 
            this.cpuTempLabel1.AutoSize = true;
            this.cpuTempLabel1.Location = new System.Drawing.Point(335, 109);
            this.cpuTempLabel1.Name = "cpuTempLabel1";
            this.cpuTempLabel1.Size = new System.Drawing.Size(62, 13);
            this.cpuTempLabel1.TabIndex = 7;
            this.cpuTempLabel1.Text = "CPU Temp:";
            this.cpuTempLabel1.Click += new System.EventHandler(this.label1_Click);
            // 
            // gpuTempLabel1
            // 
            this.gpuTempLabel1.AutoSize = true;
            this.gpuTempLabel1.Location = new System.Drawing.Point(334, 138);
            this.gpuTempLabel1.Name = "gpuTempLabel1";
            this.gpuTempLabel1.Size = new System.Drawing.Size(63, 13);
            this.gpuTempLabel1.TabIndex = 8;
            this.gpuTempLabel1.Text = "GPU Temp:";
            this.gpuTempLabel1.Click += new System.EventHandler(this.label2_Click);
            // 
            // cpuUsageLabel1
            // 
            this.cpuUsageLabel1.AutoSize = true;
            this.cpuUsageLabel1.Location = new System.Drawing.Point(331, 54);
            this.cpuUsageLabel1.Name = "cpuUsageLabel1";
            this.cpuUsageLabel1.Size = new System.Drawing.Size(66, 13);
            this.cpuUsageLabel1.TabIndex = 9;
            this.cpuUsageLabel1.Text = "CPU Usage:";
            // 
            // ramUsageLabel1
            // 
            this.ramUsageLabel1.AutoSize = true;
            this.ramUsageLabel1.Location = new System.Drawing.Point(329, 80);
            this.ramUsageLabel1.Name = "ramUsageLabel1";
            this.ramUsageLabel1.Size = new System.Drawing.Size(68, 13);
            this.ramUsageLabel1.TabIndex = 10;
            this.ramUsageLabel1.Text = "RAM Usage:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Baud Rate:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // baudComboBox1
            // 
            this.baudComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudComboBox1.FormattingEnabled = true;
            this.baudComboBox1.Items.AddRange(new object[] {
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.baudComboBox1.Location = new System.Drawing.Point(415, 217);
            this.baudComboBox1.Name = "baudComboBox1";
            this.baudComboBox1.Size = new System.Drawing.Size(89, 21);
            this.baudComboBox1.TabIndex = 14;
            this.baudComboBox1.SelectedIndexChanged += new System.EventHandler(this.baudComboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(329, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Update Interval:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // outputComboBox1
            // 
            this.outputComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputComboBox1.FormattingEnabled = true;
            this.outputComboBox1.Location = new System.Drawing.Point(415, 245);
            this.outputComboBox1.Name = "outputComboBox1";
            this.outputComboBox1.Size = new System.Drawing.Size(89, 21);
            this.outputComboBox1.TabIndex = 16;
            this.outputComboBox1.SelectedIndexChanged += new System.EventHandler(this.outputComboBox1_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(329, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Output:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // adminLinkLabel1
            // 
            this.adminLinkLabel1.AutoSize = true;
            this.adminLinkLabel1.Location = new System.Drawing.Point(12, 345);
            this.adminLinkLabel1.Name = "adminLinkLabel1";
            this.adminLinkLabel1.Size = new System.Drawing.Size(284, 13);
            this.adminLinkLabel1.TabIndex = 19;
            this.adminLinkLabel1.TabStop = true;
            this.adminLinkLabel1.Text = "Run as Administrator to enable temperature measurements.";
            this.adminLinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ohm_arduino_gui.Properties.Resources.gradient;
            this.pictureBox1.Location = new System.Drawing.Point(12, 316);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(492, 18);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // gradientTrackBar1
            // 
            this.gradientTrackBar1.LargeChange = 10;
            this.gradientTrackBar1.Location = new System.Drawing.Point(12, 285);
            this.gradientTrackBar1.Maximum = 100;
            this.gradientTrackBar1.Name = "gradientTrackBar1";
            this.gradientTrackBar1.Size = new System.Drawing.Size(492, 45);
            this.gradientTrackBar1.SmallChange = 2;
            this.gradientTrackBar1.TabIndex = 21;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 370);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gradientTrackBar1);
            this.Controls.Add(this.adminLinkLabel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.outputComboBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.baudComboBox1);
            this.Controls.Add(this.intervalComboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ramUsageLabel1);
            this.Controls.Add(this.cpuUsageLabel1);
            this.Controls.Add(this.gpuTempLabel1);
            this.Controls.Add(this.cpuTempLabel1);
            this.Controls.Add(this.ramUtilBox);
            this.Controls.Add(this.cpuUtilBox);
            this.Controls.Add(this.gpuTempBox);
            this.Controls.Add(this.cpuTempBox);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.comPortCombo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "OpenHardwareMonitor - Arduino";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.notifyIconMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientTrackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.ComboBox comPortCombo;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox cpuTempBox;
        private System.Windows.Forms.TextBox gpuTempBox;
        private System.Windows.Forms.TextBox cpuUtilBox;
        private System.Windows.Forms.TextBox ramUtilBox;
        private System.Windows.Forms.Label cpuTempLabel1;
        private System.Windows.Forms.Label gpuTempLabel1;
        private System.Windows.Forms.Label cpuUsageLabel1;
        private System.Windows.Forms.Label ramUsageLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox intervalComboBox1;
        private System.Windows.Forms.ComboBox baudComboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox outputComboBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel adminLinkLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar gradientTrackBar1;
    }
}

