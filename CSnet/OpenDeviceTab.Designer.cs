namespace CSnet
{
    partial class OpenDeviceTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.BETLowerButton = new System.Windows.Forms.Button();
            this.GPIOHigh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SetupBEV = new System.Windows.Forms.Button();
            this.SetupOP90 = new System.Windows.Forms.Button();
            this.SetupBET = new System.Windows.Forms.Button();
            this.ButtonGetConfig = new System.Windows.Forms.Button();
            this.firmwareLabel = new System.Windows.Forms.Label();
            this.chkAutoRead = new System.Windows.Forms.CheckBox();
            this.ButtonSetACL = new System.Windows.Forms.Button();
            this.ButtonACLPage = new System.Windows.Forms.Button();
            this.ButtonGetACL = new System.Windows.Forms.Button();
            this.lblWaitForRxMessageWithTimeOutResult = new System.Windows.Forms.Label();
            this.ButtonOTAP = new System.Windows.Forms.Button();
            this.lblReadErrors = new System.Windows.Forms.Label();
            this.lblReadCount = new System.Windows.Forms.Label();
            this.ButtonGetMessages = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PMSBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EMSBox = new System.Windows.Forms.ListBox();
            this.cmdGetErrors = new System.Windows.Forms.Button();
            this.lstErrorHolder = new System.Windows.Forms.ListBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ButtonPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1381, 774);
            this.tableLayoutPanel1.TabIndex = 52;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.panel1);
            this.ButtonPanel.Controls.Add(this.GPIOHigh);
            this.ButtonPanel.Controls.Add(this.ButtonGetConfig);
            this.ButtonPanel.Controls.Add(this.firmwareLabel);
            this.ButtonPanel.Controls.Add(this.chkAutoRead);
            this.ButtonPanel.Controls.Add(this.ButtonSetACL);
            this.ButtonPanel.Controls.Add(this.ButtonACLPage);
            this.ButtonPanel.Controls.Add(this.ButtonGetACL);
            this.ButtonPanel.Controls.Add(this.lblWaitForRxMessageWithTimeOutResult);
            this.ButtonPanel.Controls.Add(this.ButtonOTAP);
            this.ButtonPanel.Controls.Add(this.lblReadErrors);
            this.ButtonPanel.Controls.Add(this.lblReadCount);
            this.ButtonPanel.Controls.Add(this.ButtonGetMessages);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPanel.Location = new System.Drawing.Point(13, 13);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(1355, 107);
            this.ButtonPanel.TabIndex = 49;
            // 
            // BETLowerButton
            // 
            this.BETLowerButton.Location = new System.Drawing.Point(8, 44);
            this.BETLowerButton.Margin = new System.Windows.Forms.Padding(4);
            this.BETLowerButton.Name = "BETLowerButton";
            this.BETLowerButton.Size = new System.Drawing.Size(62, 29);
            this.BETLowerButton.TabIndex = 48;
            this.BETLowerButton.Text = "Lower";
            this.BETLowerButton.Click += new System.EventHandler(this.BETLowerButton_Click);
            // 
            // GPIOHigh
            // 
            this.GPIOHigh.Location = new System.Drawing.Point(1161, 34);
            this.GPIOHigh.Margin = new System.Windows.Forms.Padding(4);
            this.GPIOHigh.Name = "GPIOHigh";
            this.GPIOHigh.Size = new System.Drawing.Size(136, 64);
            this.GPIOHigh.TabIndex = 47;
            this.GPIOHigh.Text = "Reset BDSB";
            this.GPIOHigh.Click += new System.EventHandler(this.GPIOHigh_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(61, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 29);
            this.label3.TabIndex = 46;
            this.label3.Text = "Select mode";
            // 
            // SetupBEV
            // 
            this.SetupBEV.Location = new System.Drawing.Point(78, 47);
            this.SetupBEV.Margin = new System.Windows.Forms.Padding(4);
            this.SetupBEV.Name = "SetupBEV";
            this.SetupBEV.Size = new System.Drawing.Size(51, 26);
            this.SetupBEV.TabIndex = 45;
            this.SetupBEV.Text = "BEV";
            this.SetupBEV.Click += new System.EventHandler(this.SetupBEV_Click);
            // 
            // SetupOP90
            // 
            this.SetupOP90.Location = new System.Drawing.Point(196, 44);
            this.SetupOP90.Margin = new System.Windows.Forms.Padding(4);
            this.SetupOP90.Name = "SetupOP90";
            this.SetupOP90.Size = new System.Drawing.Size(67, 33);
            this.SetupOP90.TabIndex = 44;
            this.SetupOP90.Text = "OP90";
            this.SetupOP90.Click += new System.EventHandler(this.SetupOP90_Click);
            // 
            // SetupBET
            // 
            this.SetupBET.Location = new System.Drawing.Point(137, 47);
            this.SetupBET.Margin = new System.Windows.Forms.Padding(4);
            this.SetupBET.Name = "SetupBET";
            this.SetupBET.Size = new System.Drawing.Size(51, 26);
            this.SetupBET.TabIndex = 43;
            this.SetupBET.Text = "BET";
            this.SetupBET.Click += new System.EventHandler(this.SetupBET_Click);
            // 
            // ButtonGetConfig
            // 
            this.ButtonGetConfig.Location = new System.Drawing.Point(903, 72);
            this.ButtonGetConfig.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonGetConfig.Name = "ButtonGetConfig";
            this.ButtonGetConfig.Size = new System.Drawing.Size(100, 31);
            this.ButtonGetConfig.TabIndex = 41;
            this.ButtonGetConfig.Text = "GET CONFIG";
            this.ButtonGetConfig.UseVisualStyleBackColor = true;
            this.ButtonGetConfig.Click += new System.EventHandler(this.Config_Click);
            // 
            // firmwareLabel
            // 
            this.firmwareLabel.Location = new System.Drawing.Point(1008, 37);
            this.firmwareLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.firmwareLabel.Name = "firmwareLabel";
            this.firmwareLabel.Size = new System.Drawing.Size(181, 20);
            this.firmwareLabel.TabIndex = 35;
            this.firmwareLabel.Text = "Firmware Version : ";
            // 
            // chkAutoRead
            // 
            this.chkAutoRead.Checked = true;
            this.chkAutoRead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoRead.Location = new System.Drawing.Point(156, 20);
            this.chkAutoRead.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoRead.Name = "chkAutoRead";
            this.chkAutoRead.Size = new System.Drawing.Size(107, 20);
            this.chkAutoRead.TabIndex = 34;
            this.chkAutoRead.Text = "AutoRead";
            this.chkAutoRead.CheckedChanged += new System.EventHandler(this.chkAutoRead_CheckedChanged);
            // 
            // ButtonSetACL
            // 
            this.ButtonSetACL.Location = new System.Drawing.Point(900, 30);
            this.ButtonSetACL.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonSetACL.Name = "ButtonSetACL";
            this.ButtonSetACL.Size = new System.Drawing.Size(100, 31);
            this.ButtonSetACL.TabIndex = 33;
            this.ButtonSetACL.Text = "SETACL";
            this.ButtonSetACL.UseVisualStyleBackColor = true;
            this.ButtonSetACL.Click += new System.EventHandler(this.SetACL_Click);
            // 
            // ButtonACLPage
            // 
            this.ButtonACLPage.Location = new System.Drawing.Point(792, 72);
            this.ButtonACLPage.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonACLPage.Name = "ButtonACLPage";
            this.ButtonACLPage.Size = new System.Drawing.Size(100, 32);
            this.ButtonACLPage.TabIndex = 32;
            this.ButtonACLPage.Text = "ACL PAGE";
            this.ButtonACLPage.UseVisualStyleBackColor = true;
            this.ButtonACLPage.Click += new System.EventHandler(this.ACLPage_Click);
            // 
            // ButtonGetACL
            // 
            this.ButtonGetACL.Location = new System.Drawing.Point(792, 30);
            this.ButtonGetACL.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonGetACL.Name = "ButtonGetACL";
            this.ButtonGetACL.Size = new System.Drawing.Size(100, 31);
            this.ButtonGetACL.TabIndex = 31;
            this.ButtonGetACL.Text = "GETACL";
            this.ButtonGetACL.UseVisualStyleBackColor = true;
            this.ButtonGetACL.Click += new System.EventHandler(this.GetAclButton_Click);
            // 
            // lblWaitForRxMessageWithTimeOutResult
            // 
            this.lblWaitForRxMessageWithTimeOutResult.Location = new System.Drawing.Point(263, 50);
            this.lblWaitForRxMessageWithTimeOutResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaitForRxMessageWithTimeOutResult.Name = "lblWaitForRxMessageWithTimeOutResult";
            this.lblWaitForRxMessageWithTimeOutResult.Size = new System.Drawing.Size(117, 49);
            this.lblWaitForRxMessageWithTimeOutResult.TabIndex = 27;
            this.lblWaitForRxMessageWithTimeOutResult.Text = "Status";
            // 
            // ButtonOTAP
            // 
            this.ButtonOTAP.Location = new System.Drawing.Point(1011, 72);
            this.ButtonOTAP.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonOTAP.Name = "ButtonOTAP";
            this.ButtonOTAP.Size = new System.Drawing.Size(66, 29);
            this.ButtonOTAP.TabIndex = 26;
            this.ButtonOTAP.Text = "OTAP";
            this.ButtonOTAP.Click += new System.EventHandler(this.OTAP_Click);
            // 
            // lblReadErrors
            // 
            this.lblReadErrors.Location = new System.Drawing.Point(263, 10);
            this.lblReadErrors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadErrors.Name = "lblReadErrors";
            this.lblReadErrors.Size = new System.Drawing.Size(181, 20);
            this.lblReadErrors.TabIndex = 24;
            this.lblReadErrors.Text = "Number Errors : ";
            // 
            // lblReadCount
            // 
            this.lblReadCount.Location = new System.Drawing.Point(263, 30);
            this.lblReadCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadCount.Name = "lblReadCount";
            this.lblReadCount.Size = new System.Drawing.Size(181, 20);
            this.lblReadCount.TabIndex = 23;
            this.lblReadCount.Text = "Number Read : ";
            // 
            // ButtonGetMessages
            // 
            this.ButtonGetMessages.Location = new System.Drawing.Point(9, 5);
            this.ButtonGetMessages.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonGetMessages.Name = "ButtonGetMessages";
            this.ButtonGetMessages.Size = new System.Drawing.Size(139, 49);
            this.ButtonGetMessages.TabIndex = 20;
            this.ButtonGetMessages.Text = "Get Messages";
            this.ButtonGetMessages.Click += new System.EventHandler(this.GetMessages_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lstMessage, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.PMSBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.EMSBox, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmdGetErrors, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lstErrorHolder, 0, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 126);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1355, 635);
            this.tableLayoutPanel2.TabIndex = 50;
            // 
            // lstMessage
            // 
            this.lstMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMessage.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMessage.HorizontalScrollbar = true;
            this.lstMessage.IntegralHeight = false;
            this.lstMessage.ItemHeight = 17;
            this.lstMessage.Location = new System.Drawing.Point(14, 14);
            this.lstMessage.Margin = new System.Windows.Forms.Padding(4);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.ScrollAlwaysVisible = true;
            this.lstMessage.Size = new System.Drawing.Size(1327, 176);
            this.lstMessage.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Location = new System.Drawing.Point(615, 200);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 38;
            this.label1.Text = "PMS data";
            // 
            // PMSBox
            // 
            this.PMSBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PMSBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PMSBox.HorizontalScrollbar = true;
            this.PMSBox.ItemHeight = 17;
            this.PMSBox.Location = new System.Drawing.Point(14, 228);
            this.PMSBox.Margin = new System.Windows.Forms.Padding(4);
            this.PMSBox.Name = "PMSBox";
            this.PMSBox.ScrollAlwaysVisible = true;
            this.PMSBox.Size = new System.Drawing.Size(1327, 145);
            this.PMSBox.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Location = new System.Drawing.Point(618, 382);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 39;
            this.label2.Text = "EMS data";
            // 
            // EMSBox
            // 
            this.EMSBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EMSBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EMSBox.HorizontalScrollbar = true;
            this.EMSBox.ItemHeight = 17;
            this.EMSBox.Location = new System.Drawing.Point(14, 411);
            this.EMSBox.Margin = new System.Windows.Forms.Padding(4);
            this.EMSBox.Name = "EMSBox";
            this.EMSBox.ScrollAlwaysVisible = true;
            this.EMSBox.Size = new System.Drawing.Size(1327, 84);
            this.EMSBox.TabIndex = 38;
            // 
            // cmdGetErrors
            // 
            this.cmdGetErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdGetErrors.Location = new System.Drawing.Point(14, 503);
            this.cmdGetErrors.Margin = new System.Windows.Forms.Padding(4);
            this.cmdGetErrors.Name = "cmdGetErrors";
            this.cmdGetErrors.Size = new System.Drawing.Size(1327, 22);
            this.cmdGetErrors.TabIndex = 22;
            this.cmdGetErrors.Text = "Get Errors";
            this.cmdGetErrors.Click += new System.EventHandler(this.GetErrors_Click);
            // 
            // lstErrorHolder
            // 
            this.lstErrorHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstErrorHolder.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstErrorHolder.HorizontalScrollbar = true;
            this.lstErrorHolder.ItemHeight = 17;
            this.lstErrorHolder.Location = new System.Drawing.Point(14, 533);
            this.lstErrorHolder.Margin = new System.Windows.Forms.Padding(4);
            this.lstErrorHolder.Name = "lstErrorHolder";
            this.lstErrorHolder.ScrollAlwaysVisible = true;
            this.lstErrorHolder.Size = new System.Drawing.Size(1327, 88);
            this.lstErrorHolder.TabIndex = 21;
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 70;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.BETLowerButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.SetupBEV);
            this.panel1.Controls.Add(this.SetupOP90);
            this.panel1.Controls.Add(this.SetupBET);
            this.panel1.Location = new System.Drawing.Point(486, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 89);
            this.panel1.TabIndex = 49;
            // 
            // OpenDeviceTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OpenDeviceTab";
            this.Size = new System.Drawing.Size(1381, 774);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonSetACL;
        private System.Windows.Forms.Button ButtonACLPage;
        private System.Windows.Forms.Button ButtonGetACL;
        internal System.Windows.Forms.Label lblWaitForRxMessageWithTimeOutResult;
        internal System.Windows.Forms.Button ButtonOTAP;
        internal System.Windows.Forms.Label lblReadErrors;
        internal System.Windows.Forms.Label lblReadCount;
        internal System.Windows.Forms.ListBox lstMessage;
        internal System.Windows.Forms.Button ButtonGetMessages;
        internal System.Windows.Forms.ListBox lstErrorHolder;
        internal System.Windows.Forms.Button cmdGetErrors;
        internal System.Windows.Forms.CheckBox chkAutoRead;
        public System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Label firmwareLabel;
        internal System.Windows.Forms.ListBox PMSBox;
        internal System.Windows.Forms.ListBox EMSBox;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonGetConfig;
        internal System.Windows.Forms.Button SetupBEV;
        internal System.Windows.Forms.Button SetupOP90;
        internal System.Windows.Forms.Button SetupBET;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button GPIOHigh;
        internal System.Windows.Forms.Button BETLowerButton;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
    }
}
