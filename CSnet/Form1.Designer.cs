namespace CSnet
{
    partial class Form1
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
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.chkHexFormat = new System.Windows.Forms.CheckBox();
            this.CmdOpenFirstDevice = new System.Windows.Forms.Button();
            this.cmdCloseDevice = new System.Windows.Forms.Button();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkHexFormat);
            this.GroupBox1.Controls.Add(this.CmdOpenFirstDevice);
            this.GroupBox1.Controls.Add(this.cmdCloseDevice);
            this.GroupBox1.Location = new System.Drawing.Point(1, -1);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Size = new System.Drawing.Size(1248, 108);
            this.GroupBox1.TabIndex = 50;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Connecting";
            // 
            // chkHexFormat
            // 
            this.chkHexFormat.Location = new System.Drawing.Point(205, 26);
            this.chkHexFormat.Margin = new System.Windows.Forms.Padding(4);
            this.chkHexFormat.Name = "chkHexFormat";
            this.chkHexFormat.Size = new System.Drawing.Size(133, 21);
            this.chkHexFormat.TabIndex = 52;
            this.chkHexFormat.Text = "Raw Packet";
            this.chkHexFormat.CheckedChanged += new System.EventHandler(this.chkHexFormat_CheckedChanged);
            // 
            // CmdOpenFirstDevice
            // 
            this.CmdOpenFirstDevice.Location = new System.Drawing.Point(20, 21);
            this.CmdOpenFirstDevice.Margin = new System.Windows.Forms.Padding(4);
            this.CmdOpenFirstDevice.Name = "CmdOpenFirstDevice";
            this.CmdOpenFirstDevice.Size = new System.Drawing.Size(160, 28);
            this.CmdOpenFirstDevice.TabIndex = 0;
            this.CmdOpenFirstDevice.Text = "Open Devices";
            this.CmdOpenFirstDevice.Click += new System.EventHandler(this.CmdOpenDevices_Click);
            // 
            // cmdCloseDevice
            // 
            this.cmdCloseDevice.Location = new System.Drawing.Point(20, 60);
            this.cmdCloseDevice.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCloseDevice.Name = "cmdCloseDevice";
            this.cmdCloseDevice.Size = new System.Drawing.Size(160, 28);
            this.cmdCloseDevice.TabIndex = 1;
            this.cmdCloseDevice.Text = "Close Devices";
            this.cmdCloseDevice.Click += new System.EventHandler(this.cmdCloseDevice_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Location = new System.Drawing.Point(1, 115);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(1248, 704);
            this.TabControl1.TabIndex = 51;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(0, 0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(200, 100);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "ISO15765";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 779);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.TabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button CmdOpenFirstDevice;
        internal System.Windows.Forms.Button cmdCloseDevice;
        internal System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        internal System.Windows.Forms.CheckBox chkHexFormat;
    }
}

