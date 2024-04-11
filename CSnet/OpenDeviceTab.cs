using System;
using System.Linq;
using System.Windows.Forms;

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
            if (deviceModel.IsSetup)
            {
                deviceModel.GetMessages();
                OutputData();
            }
        }

        private void SetACL_Click(object sender, EventArgs e)
        {
            deviceModel.SetACL();
        }

        private void ACLPage_Click(object sender, EventArgs e)
        {
            ACLPage fw = new ACLPage();
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
                }
            }

            if (chkHexFormat)
            {
                //PMS data
                deviceModel.managers[0].PMSPacket0.ToList().ForEach(x => outputText += $"{x:X2} ");
                PMSBox.Items.Add(outputText);
                outputText = "";

                //EMS data
                deviceModel.managers[0].EMSPacket0.ToList().ForEach(x => outputText += $"{x:X2} ");
                EMSBox.Items.Add(outputText);
            }
            else //TODO: this is pretty tightly coupled, make it better
            {
                PMSBox.Items.Add(deviceModel.PrettyPrintPMS());

                //EMS data
                outputText = $"Manager 0 BDSB:{deviceModel.managers[0].EMSBDSBVoltage():0.00V} VREF1:{deviceModel.managers[0].EMSReferenceVoltage1():0.00}V VREF2:{deviceModel.managers[0].EMSReferenceVoltage2():0.00}V Temp1:{deviceModel.managers[0].EMSTemperature1():0.00}C Temp2:{deviceModel.managers[0].EMSTemperature2():0.00}C Pressure1:{deviceModel.managers[0].EMSPressure1():0.00}kPa Pressure2:{deviceModel.managers[0].EMSPressure2():0.00}kPa Gas1:{deviceModel.managers[0].EMSGas1():0.00}% Gas2:{deviceModel.managers[0].EMSGas2():0.00}%";

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

        private void Config_Click(object sender, EventArgs e)
        {
            //GetConfigFile();
            deviceModel.GetFileCRC();
            //GetContextualData(0);
        }

        private void SetupBEV_Click(object sender, EventArgs e)
        {
            deviceModel.SetupBEV();
        }

        private void SetupBET_Click(object sender, EventArgs e)
        {
            deviceModel.SetupBET();
        }

        private void SetupOP90_Click(object sender, EventArgs e)
        {
            deviceModel.SetupOP90();
        }

        private void GPIOHigh_Click(object sender, EventArgs e)
        {
            deviceModel.GPIO();
        }

        private void BETLowerButton_Click(object sender, EventArgs e)
        {
            deviceModel.SetupBET();
        }

    }
}
