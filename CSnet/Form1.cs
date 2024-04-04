using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LiuQF.Common;

namespace CSnet
{
    public partial class Form1 : Form
    {
        OptionsNeoEx neoDeviceOption = new OptionsNeoEx();
        Func<double, double> f;

        public Form1()
        {
            InitializeComponent();

            FindDevices();
        }

        private const int MAX_DEVICES = 255;

        private HashSet<String> openedDevicesSerials = new HashSet<String>();
        private int numberOfDevicesFound = MAX_DEVICES;
        private NeoDeviceEx[] ndNeoToOpenex = new NeoDeviceEx[MAX_DEVICES]; //Struct holding detected hardware information, TODO: this is limiting us to 16 devices

        private void CmdOpenDevices_Click(object sender, EventArgs e)
        {
            OpenDevices();
        }

        private void FindDevices()
        {
            //Search for connected hardware
            int iResult = icsNeoDll.icsneoFindDevices(ref ndNeoToOpenex[0], ref numberOfDevicesFound, 0, 0, ref neoDeviceOption, 0);

            if (iResult == 0)
            {
                MessageBox.Show("Problem finding devices");
                return;
            }

            if (numberOfDevicesFound < 1)
            {
                MessageBox.Show("No devices found, connect RAD wBMS");
                return;
            }
        }

        private void OpenDevices()
        {
            byte[] bNetwork = new byte[MAX_DEVICES];    //List of hardware IDs
            int iCount;		 //counter

            //File NetworkID array
            for (iCount = 0; iCount < MAX_DEVICES; iCount++)
            {
                bNetwork[iCount] = Convert.ToByte(iCount);
            }

            UInt32 StringSize = 6;
            byte[] bSN = new byte[6];

            OptionsOpenNeoEx OptionOpenNeoEX = new OptionsOpenNeoEx();

            for (int i = 0; i < numberOfDevicesFound; i++)
            {
                //Convert the Serial number to a String
                icsNeoDll.icsneoSerialNumberToString(Convert.ToUInt32(ndNeoToOpenex[i].neoDevice.SerialNumber), ref bSN[0], ref StringSize);
                string sConvertedSN = Encoding.ASCII.GetString(bSN);

                //keep track of devices that have already been opened
                if (openedDevicesSerials.Add(sConvertedSN))
                {
                    IntPtr anonymousPointer = IntPtr.Zero;
                    int iResult = icsNeoDll.icsneoOpenDevice(ref ndNeoToOpenex[i], ref anonymousPointer, ref bNetwork[0], 1, 0, ref OptionOpenNeoEX, 0);
                    if (iResult == 1)
                    {
                        MessageBox.Show("Device Opened OK!");
                    }
                    else
                    {
                        //MessageBox.Show("Problem Opening Port");
                        Debug.WriteLine("Device not opened");
                        return;
                    }

                    //create new tab per device
                    TabPage tabPage = new TabPage($"Device {sConvertedSN}");
                    tabPage.Controls.Add(new OpenDeviceTab(ref anonymousPointer) { Name = $"{sConvertedSN}" });
                    this.TabControl1.TabPages.Add(tabPage);
                }
            }
        }

        private void CmdCloseDevice_Click(object sender, EventArgs e)
        {
            int iResult;
            int iNumberOfErrors = 0;

            if (Timer1.Enabled == true)
            {
                Timer1.Enabled = false;
                //chkAutoRead.Checked = false;
            }

            foreach (TabPage tabPage in TabControl1.TabPages)
            {
                OpenDeviceTab uc1 = (OpenDeviceTab)tabPage.Controls[0];

                //close the port
                iResult = icsNeoDll.icsneoClosePort(uc1.m_hObject, ref iNumberOfErrors);
                //release resources used by the object
                icsNeoDll.icsneoFreeObject(uc1.m_hObject);

                if (iResult == 1)
                {
                    openedDevicesSerials.Remove(uc1.Name);
                    TabControl1.TabPages.Remove(tabPage);
                    MessageBox.Show("Port Closed OK!");
                }
                else
                {
                    MessageBox.Show("Problem ClosingPort");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //开启线程
            /*
            StartUDPServer(new UDPGeneric(new SocketWrapper("127.0.0.1", 9011), TabControl1));
            StartUDPServer(new UDPOP140(new SocketWrapper("192.168.3.100", 9020), TabControl1));
            StartUDPServer(new UDPOP140(new SocketWrapper("192.168.3.100", 9021), TabControl1));
            StartUDPServer(new UDPOP140(new SocketWrapper("192.168.3.100", 9022), TabControl1));
            */

            //Add the version number to the title of the application.  
            try
            {
                this.Text = "ADI box communication, API Version " + Convert.ToString(icsNeoDll.icsneoGetDLLVersion());
            }
            catch
            {
                this.Text = "DLL Missing or Not Accessable";
            }
        }

        private void StartUDPServer(UDPListener listener)
        {
            ThreadStart ts = new ThreadStart(listener.Start);
            Thread tc = new Thread(ts);
            tc.IsBackground = true;
            tc.Start();
        }
        
        private void chkHexFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHexFormat.Checked)
            {
                foreach (TabPage tabPage in TabControl1.TabPages)
                {
                    OpenDeviceTab uc1 = (OpenDeviceTab)tabPage.Controls[0];

                    uc1.chkHexFormat = true;
                }
            } else
            {
                foreach (TabPage tabPage in TabControl1.TabPages)
                {
                    OpenDeviceTab uc1 = (OpenDeviceTab)tabPage.Controls[0];

                    uc1.chkHexFormat = false;
                }
            }
        }
    }
}