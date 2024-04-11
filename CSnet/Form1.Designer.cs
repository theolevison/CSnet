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
            this.OpenDevicesButton = new System.Windows.Forms.Button();
            this.cmdCloseDevice = new System.Windows.Forms.Button();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkHexFormat);
            this.GroupBox1.Controls.Add(this.OpenDevicesButton);
            this.GroupBox1.Controls.Add(this.cmdCloseDevice);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox1.Location = new System.Drawing.Point(4, 4);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Size = new System.Drawing.Size(987, 110);
            this.GroupBox1.TabIndex = 50;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Connecting";
            // 
            // chkHexFormat
            // 
            this.chkHexFormat.Location = new System.Drawing.Point(213, 26);
            this.chkHexFormat.Margin = new System.Windows.Forms.Padding(4);
            this.chkHexFormat.Name = "chkHexFormat";
            this.chkHexFormat.Size = new System.Drawing.Size(119, 35);
            this.chkHexFormat.TabIndex = 52;
            this.chkHexFormat.Text = "Raw Packet";
            this.chkHexFormat.CheckedChanged += new System.EventHandler(this.chkHexFormat_CheckedChanged);
            // 
            // OpenDevicesButton
            // 
            this.OpenDevicesButton.Location = new System.Drawing.Point(20, 26);
            this.OpenDevicesButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenDevicesButton.Name = "OpenDevicesButton";
            this.OpenDevicesButton.Size = new System.Drawing.Size(159, 26);
            this.OpenDevicesButton.TabIndex = 0;
            this.OpenDevicesButton.Text = "Open Devices";
            this.OpenDevicesButton.Click += new System.EventHandler(this.CmdOpenDevices_Click);
            // 
            // cmdCloseDevice
            // 
            this.cmdCloseDevice.Location = new System.Drawing.Point(20, 60);
            this.cmdCloseDevice.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCloseDevice.Name = "cmdCloseDevice";
            this.cmdCloseDevice.Size = new System.Drawing.Size(159, 27);
            this.cmdCloseDevice.TabIndex = 1;
            this.cmdCloseDevice.Text = "Close Devices";
            this.cmdCloseDevice.Click += new System.EventHandler(this.CmdCloseDevice_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(4, 122);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(987, 465);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.GroupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 591);
            this.tableLayoutPanel1.TabIndex = 52;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 591);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GroupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button OpenDevicesButton;
        internal System.Windows.Forms.Button cmdCloseDevice;
        internal System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        internal System.Windows.Forms.CheckBox chkHexFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

