using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LiuQF.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace CSnet
{
    public partial class OpenDeviceTab : UserControl
    {
        DeviceModel deviceModel;
        public bool chkHexFormat = false;

        public OpenDeviceTab(DeviceModel deviceModel)
        {
            this.deviceModel = deviceModel;
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            deviceModel.GetMessages();
            OutputData();
        }

        private void SetACL_Click(object sender, EventArgs e)
        {
            deviceModel.SetACL();
        }

        private void ACLPage_Click(object sender, EventArgs e)
        {
            Form2 fw = new Form2();
            fw.WindowState = FormWindowState.Normal;
            fw.StartPosition = FormStartPosition.CenterScreen;
            fw.ShowDialog();
        }

        private void GetAclButton_Click(object sender, EventArgs e)//获取ACL
        {
            deviceModel.GetAcl();
        }

        private void OTAP_Click(object sender, EventArgs e)
        {
            deviceModel.OTAP();
        }
        private void GetMessages_Click(object sender, EventArgs e)
        {
            deviceModel.GetMessages();
            OutputData();
        }

        private void OutputData()
        {
            // clear the previous list of messages
            lstMessage.Items.Clear();
            PMSBox.Items.Clear();
            EMSBox.Items.Clear();

            string outputText = "";

            //iterate through each node, printing the latest packet in the requested format
            foreach (ModuleData module in deviceModel.modules)
            {
                if (chkHexFormat)
                {
                    module.Packet0.ToList().ForEach(x => outputText += $"{x:X2} ");
                    lstMessage.Items.Add(outputText);
                    outputText = "";
                }
                else
                {
                    outputText = $"Module {module.MacAddress} Version {module.version}";
                    foreach (double cgv in module.CGV)
                    {
                        outputText += $" {cgv:0.000}V";
                    }
                    outputText += $" {module.Thermistor1:0.00}C {module.Thermistor2:0.00}C";
                    lstMessage.Items.Add(outputText);
                    outputText = "";

                    //Debug.WriteLine($"Module {module.MacAddress} rssi: {module.PeakRSSI}");
                    //Debug.WriteLine($"Module {module.MacAddress} pu: {module.PeakUpdateRate}");
                }
            }

            if (chkHexFormat)
            {
                //PMS data
                deviceModel.managers[0].PMSPacket0.ToList().ForEach(x => outputText += $"{x:X2} ");
                PMSBox.Items.Add(outputText);
                outputText = "";
                /*
                managers[1].PMSPacket0.ToList().ForEach(x => temp += $"{x:X2} ");
                PMSBox.Items.Add(temp);
                temp = "";
                */

                //EMS data
                deviceModel.managers[0].EMSPacket0.ToList().ForEach(x => outputText += $"{x:X2} ");
                EMSBox.Items.Add(outputText);

                /*
                managers[1].EMSPacket0.ToList().ForEach(x => temp += $"{x:X2} "); //TODO: make this an option in packet reading
                EMSBox.Items.Add(temp);
                */
            }
            else //TODO: this is pretty tightly coupled, make it better
            {
                //PMS data
                outputText = $"Manager 0 BEV {deviceModel.managers[0].I1:0.00}A {deviceModel.managers[0].I2:0.00}A {deviceModel.managers[0].GetBEVDCFCPlus():0.00} {deviceModel.managers[0].GetBEVDCFCMinus():0.00} {deviceModel.managers[0].GetBEVShuntTemp():0.00} {deviceModel.managers[0].GetBEVDCFCContactorTemp():0.00} {deviceModel.managers[0].GetBEVMainContactorTemp():0.00} {deviceModel.managers[0].GetBEVVREF():0.00}";
                PMSBox.Items.Add(outputText);
                outputText = $"Manager 0 BETA {deviceModel.managers[0].GetBETAHVCDMinus()} {deviceModel.managers[0].GetBETASA1Temp()} {deviceModel.managers[0].GetBETASA4Temp()} {deviceModel.managers[0].GetBETASA3Temp()} {deviceModel.managers[0].GetBETAShuntTemperature()}";
                PMSBox.Items.Add(outputText);
                outputText = $"Manager 0 BETB {deviceModel.managers[0].GetBETBDCFCDifferential()} {deviceModel.managers[0].GetBETBDCFCMinus()} {deviceModel.managers[0].GetBETBDCFCPlus()} {deviceModel.managers[0].GetBETBSB1Temp()} {deviceModel.managers[0].GetBETBShuntTemp()} {deviceModel.managers[0].GetBETBHVDCMinus()}";
                PMSBox.Items.Add(outputText);
                //Debug.WriteLine(managers[0].PMSPacket0);
                //Debug.WriteLine(managers[0].AUX1);
                //Debug.WriteLine(managers[0].AUX2);

                //EMS data
                outputText = $"Manager 0 {deviceModel.managers[0].EMSBDSBVoltage():0.00V} {deviceModel.managers[0].CD1V:0.00V} {deviceModel.managers[0].EMSReferenceVoltage1():0.00} {deviceModel.managers[0].EMSReferenceVoltage2():0.00} {deviceModel.managers[0].EMSTemperature1():0.00} {deviceModel.managers[0].EMSTemperature2():0.00} {deviceModel.managers[0].EMSPressure1():0.00} {deviceModel.managers[0].EMSPressure2():0.00} {deviceModel.managers[0].EMSGas1():0.00} {deviceModel.managers[0].EMSGas2():0.00}";

                EMSBox.Items.Add(outputText);
            }
        }

        private void GetErrors_Click(object sender, EventArgs e)
        {
            deviceModel.GetErrors();
            lstErrorHolder.Items.Clear();
            lstErrorHolder.Items.Add(deviceModel.errors);
        }

        private void chkAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRead.Checked == true)
            {
                Timer1.Enabled = true;
            }
            else
            {
                Timer1.Enabled = false;
            }
        }

        private void GPIO_Click(object sender, EventArgs e)
        {
            deviceModel.GPIO();
            
        }
        private void Version_Click(object sender, EventArgs e)
        {
            deviceModel.GetDeviceVersions();
        }

        private void Config_Click(object sender, EventArgs e)
        {
            //GetConfigFile();
            deviceModel.GetFileCRC();
            //GetContextualData(0);
        }
    }
}
