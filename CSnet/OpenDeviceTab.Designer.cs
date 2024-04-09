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
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SetupBEV = new System.Windows.Forms.Button();
            this.SetupOP90 = new System.Windows.Forms.Button();
            this.SetupBET = new System.Windows.Forms.Button();
            this.ButtonGetConfig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.EMSBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PMSBox = new System.Windows.Forms.ListBox();
            this.firmwareLabel = new System.Windows.Forms.Label();
            this.chkAutoRead = new System.Windows.Forms.CheckBox();
            this.ButtonSetACL = new System.Windows.Forms.Button();
            this.ButtonACLPage = new System.Windows.Forms.Button();
            this.ButtonGetACL = new System.Windows.Forms.Button();
            this.lblWaitForRxMessageWithTimeOutResult = new System.Windows.Forms.Label();
            this.ButtonOTAP = new System.Windows.Forms.Button();
            this.lblReadErrors = new System.Windows.Forms.Label();
            this.lblReadCount = new System.Windows.Forms.Label();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.ButtonGetMessages = new System.Windows.Forms.Button();
            this.lstErrorHolder = new System.Windows.Forms.ListBox();
            this.cmdGetErrors = new System.Windows.Forms.Button();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.GroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.label3);
            this.GroupBox4.Controls.Add(this.SetupBEV);
            this.GroupBox4.Controls.Add(this.SetupOP90);
            this.GroupBox4.Controls.Add(this.SetupBET);
            this.GroupBox4.Controls.Add(this.ButtonGetConfig);
            this.GroupBox4.Controls.Add(this.label2);
            this.GroupBox4.Controls.Add(this.EMSBox);
            this.GroupBox4.Controls.Add(this.label1);
            this.GroupBox4.Controls.Add(this.PMSBox);
            this.GroupBox4.Controls.Add(this.firmwareLabel);
            this.GroupBox4.Controls.Add(this.chkAutoRead);
            this.GroupBox4.Controls.Add(this.ButtonSetACL);
            this.GroupBox4.Controls.Add(this.ButtonACLPage);
            this.GroupBox4.Controls.Add(this.ButtonGetACL);
            this.GroupBox4.Controls.Add(this.lblWaitForRxMessageWithTimeOutResult);
            this.GroupBox4.Controls.Add(this.ButtonOTAP);
            this.GroupBox4.Controls.Add(this.lblReadErrors);
            this.GroupBox4.Controls.Add(this.lblReadCount);
            this.GroupBox4.Controls.Add(this.lstMessage);
            this.GroupBox4.Controls.Add(this.ButtonGetMessages);
            this.GroupBox4.Controls.Add(this.lstErrorHolder);
            this.GroupBox4.Controls.Add(this.cmdGetErrors);
            this.GroupBox4.Location = new System.Drawing.Point(4, 4);
            this.GroupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox4.Size = new System.Drawing.Size(1229, 637);
            this.GroupBox4.TabIndex = 48;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Receive Message";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(570, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 29);
            this.label3.TabIndex = 46;
            this.label3.Text = "Select mode";
            // 
            // SetupBEV
            // 
            this.SetupBEV.Location = new System.Drawing.Point(570, 40);
            this.SetupBEV.Margin = new System.Windows.Forms.Padding(4);
            this.SetupBEV.Name = "SetupBEV";
            this.SetupBEV.Size = new System.Drawing.Size(51, 26);
            this.SetupBEV.TabIndex = 45;
            this.SetupBEV.Text = "BEV";
            this.SetupBEV.Click += new System.EventHandler(this.SetupBEV_Click);
            // 
            // SetupOP90
            // 
            this.SetupOP90.Location = new System.Drawing.Point(688, 40);
            this.SetupOP90.Margin = new System.Windows.Forms.Padding(4);
            this.SetupOP90.Name = "SetupOP90";
            this.SetupOP90.Size = new System.Drawing.Size(51, 26);
            this.SetupOP90.TabIndex = 44;
            this.SetupOP90.Text = "OP90";
            this.SetupOP90.Click += new System.EventHandler(this.SetupOP90_Click);
            // 
            // SetupBET
            // 
            this.SetupBET.Location = new System.Drawing.Point(629, 40);
            this.SetupBET.Margin = new System.Windows.Forms.Padding(4);
            this.SetupBET.Name = "SetupBET";
            this.SetupBET.Size = new System.Drawing.Size(51, 26);
            this.SetupBET.TabIndex = 43;
            this.SetupBET.Text = "BET";
            this.SetupBET.Click += new System.EventHandler(this.SetupBET_Click);
            // 
            // ButtonGetConfig
            // 
            this.ButtonGetConfig.Location = new System.Drawing.Point(904, 41);
            this.ButtonGetConfig.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonGetConfig.Name = "ButtonGetConfig";
            this.ButtonGetConfig.Size = new System.Drawing.Size(100, 31);
            this.ButtonGetConfig.TabIndex = 41;
            this.ButtonGetConfig.Text = "GET CONFIG";
            this.ButtonGetConfig.UseVisualStyleBackColor = true;
            this.ButtonGetConfig.Click += new System.EventHandler(this.Config_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 509);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 20);
            this.label2.TabIndex = 39;
            this.label2.Text = "EMS data";
            // 
            // EMSBox
            // 
            this.EMSBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EMSBox.HorizontalScrollbar = true;
            this.EMSBox.ItemHeight = 17;
            this.EMSBox.Location = new System.Drawing.Point(11, 529);
            this.EMSBox.Margin = new System.Windows.Forms.Padding(4);
            this.EMSBox.Name = "EMSBox";
            this.EMSBox.ScrollAlwaysVisible = true;
            this.EMSBox.Size = new System.Drawing.Size(1209, 89);
            this.EMSBox.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 393);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 20);
            this.label1.TabIndex = 38;
            this.label1.Text = "PMS data";
            // 
            // PMSBox
            // 
            this.PMSBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PMSBox.HorizontalScrollbar = true;
            this.PMSBox.ItemHeight = 17;
            this.PMSBox.Location = new System.Drawing.Point(11, 416);
            this.PMSBox.Margin = new System.Windows.Forms.Padding(4);
            this.PMSBox.Name = "PMSBox";
            this.PMSBox.ScrollAlwaysVisible = true;
            this.PMSBox.Size = new System.Drawing.Size(1209, 89);
            this.PMSBox.TabIndex = 37;
            // 
            // firmwareLabel
            // 
            this.firmwareLabel.Location = new System.Drawing.Point(1012, 11);
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
            this.chkAutoRead.Location = new System.Drawing.Point(160, 39);
            this.chkAutoRead.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoRead.Name = "chkAutoRead";
            this.chkAutoRead.Size = new System.Drawing.Size(107, 20);
            this.chkAutoRead.TabIndex = 34;
            this.chkAutoRead.Text = "AutoRead";
            this.chkAutoRead.CheckedChanged += new System.EventHandler(this.chkAutoRead_CheckedChanged);
            // 
            // ButtonSetACL
            // 
            this.ButtonSetACL.Location = new System.Drawing.Point(904, 4);
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
            this.ButtonACLPage.Location = new System.Drawing.Point(796, 41);
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
            this.ButtonGetACL.Location = new System.Drawing.Point(796, 4);
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
            this.lblWaitForRxMessageWithTimeOutResult.Location = new System.Drawing.Point(456, 19);
            this.lblWaitForRxMessageWithTimeOutResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaitForRxMessageWithTimeOutResult.Name = "lblWaitForRxMessageWithTimeOutResult";
            this.lblWaitForRxMessageWithTimeOutResult.Size = new System.Drawing.Size(117, 49);
            this.lblWaitForRxMessageWithTimeOutResult.TabIndex = 27;
            this.lblWaitForRxMessageWithTimeOutResult.Text = "Status";
            // 
            // ButtonOTAP
            // 
            this.ButtonOTAP.Location = new System.Drawing.Point(1015, 42);
            this.ButtonOTAP.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonOTAP.Name = "ButtonOTAP";
            this.ButtonOTAP.Size = new System.Drawing.Size(66, 29);
            this.ButtonOTAP.TabIndex = 26;
            this.ButtonOTAP.Text = "OTAP";
            this.ButtonOTAP.Click += new System.EventHandler(this.OTAP_Click);
            // 
            // lblReadErrors
            // 
            this.lblReadErrors.Location = new System.Drawing.Point(267, 20);
            this.lblReadErrors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadErrors.Name = "lblReadErrors";
            this.lblReadErrors.Size = new System.Drawing.Size(181, 20);
            this.lblReadErrors.TabIndex = 24;
            this.lblReadErrors.Text = "Number Errors : ";
            // 
            // lblReadCount
            // 
            this.lblReadCount.Location = new System.Drawing.Point(267, 49);
            this.lblReadCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadCount.Name = "lblReadCount";
            this.lblReadCount.Size = new System.Drawing.Size(181, 20);
            this.lblReadCount.TabIndex = 23;
            this.lblReadCount.Text = "Number Read : ";
            // 
            // lstMessage
            // 
            this.lstMessage.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMessage.HorizontalScrollbar = true;
            this.lstMessage.IntegralHeight = false;
            this.lstMessage.ItemHeight = 17;
            this.lstMessage.Location = new System.Drawing.Point(11, 79);
            this.lstMessage.Margin = new System.Windows.Forms.Padding(4);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.ScrollAlwaysVisible = true;
            this.lstMessage.Size = new System.Drawing.Size(1209, 181);
            this.lstMessage.TabIndex = 19;
            // 
            // ButtonGetMessages
            // 
            this.ButtonGetMessages.Location = new System.Drawing.Point(13, 24);
            this.ButtonGetMessages.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonGetMessages.Name = "ButtonGetMessages";
            this.ButtonGetMessages.Size = new System.Drawing.Size(139, 49);
            this.ButtonGetMessages.TabIndex = 20;
            this.ButtonGetMessages.Text = "Get Messages";
            this.ButtonGetMessages.Click += new System.EventHandler(this.GetMessages_Click);
            // 
            // lstErrorHolder
            // 
            this.lstErrorHolder.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstErrorHolder.HorizontalScrollbar = true;
            this.lstErrorHolder.ItemHeight = 17;
            this.lstErrorHolder.Location = new System.Drawing.Point(11, 300);
            this.lstErrorHolder.Margin = new System.Windows.Forms.Padding(4);
            this.lstErrorHolder.Name = "lstErrorHolder";
            this.lstErrorHolder.ScrollAlwaysVisible = true;
            this.lstErrorHolder.Size = new System.Drawing.Size(1209, 89);
            this.lstErrorHolder.TabIndex = 21;
            // 
            // cmdGetErrors
            // 
            this.cmdGetErrors.Location = new System.Drawing.Point(11, 264);
            this.cmdGetErrors.Margin = new System.Windows.Forms.Padding(4);
            this.cmdGetErrors.Name = "cmdGetErrors";
            this.cmdGetErrors.Size = new System.Drawing.Size(1211, 29);
            this.cmdGetErrors.TabIndex = 22;
            this.cmdGetErrors.Text = "Get Errors";
            this.cmdGetErrors.Click += new System.EventHandler(this.GetErrors_Click);
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 500;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // OpenDeviceTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBox4);
            this.Name = "OpenDeviceTab";
            this.Size = new System.Drawing.Size(1249, 645);
            this.GroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox4;
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
    }
}
