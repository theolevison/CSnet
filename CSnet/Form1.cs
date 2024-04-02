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

        IntPtr[] objects = new IntPtr[] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero }; //TODO: this is limiting us to 4 ADI boxes, must make IntPtr anonymous
        OptionsNeoEx neoDeviceOption = new OptionsNeoEx();
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Func<double, double> f;

        public Form1()
        {
            InitializeComponent();

            FindDevices();
        }

        private const byte ADI_WIL_MAC_SIZE = 8;
        private const byte ADI_WIL_MAX_NODES = 62;

        private const byte ADI_WIL_API_INITIALIZE = 0;
        private const byte ADI_WIL_API_TERMINATE = 1;
        private const byte ADI_WIL_API_CONNECT = 2;
        private const byte ADI_WIL_API_DISCONNECT = 3;
        private const byte ADI_WIL_API_SET_MODE = 4;
        private const byte ADI_WIL_API_GET_MODE = 5;
        private const byte ADI_WIL_API_SET_ACL = 6;
        private const byte ADI_WIL_API_GET_ACL = 7;
        private const byte ADI_WIL_API_QUERY_DEVICE = 8;
        private const byte ADI_WIL_API_GET_NETWORK_STATUS = 9;
        private const byte ADI_WIL_API_LOAD_FILE = 10;
        private const byte ADI_WIL_API_ERASE_FILE = 11;
        private const byte ADI_WIL_API_GET_DEVICE_VERSION = 12;
        private const byte ADI_WIL_API_GET_FILE_CRC = 13;
        private const byte ADI_WIL_API_SET_GPIO = 14;
        private const byte ADI_WIL_API_GET_GPIO = 15;
        private const byte ADI_WIL_API_SELECT_SCRIPT = 16;
        private const byte ADI_WIL_API_MODIFY_SCRIPT = 17;
        private const byte ADI_WIL_API_ENTER_INVENTORY_STATE = 18;
        private const byte ADI_WIL_API_GET_FILE = 19;
        private const byte ADI_WIL_API_GET_WIL_SOFTWARE_VERSION = 20;
        private const byte ADI_WIL_API_PROCESS_TASK = 21;
        private const byte ADI_WIL_API_SET_CONTEXTUAL_DATA = 22;
        private const byte ADI_WIL_API_GET_CONTEXTUAL_DATA = 23;
        private const byte ADI_WIL_API_RESET_DEVICE = 24;
        private const byte ADI_WIL_API_SET_STATE_OF_HEALTH = 25;
        private const byte ADI_WIL_API_GET_STATE_OF_HEALTH = 26;
        private const byte ADI_WIL_API_ENABLE_FAULT_SERVICING = 27;
        private const byte ADI_WIL_API_ROTATE_KEY = 28;
        private const byte ADI_WIL_API_SET_CUSTOMER_IDENTIFIER_DATA = 29;
        private const byte ADI_WIL_API_ENABLE_NETWORK_DATA_CAPTURE = 30;
        private const byte ADI_WIL_API_UPDATE_MONITOR_PARAMETERS = 31;
        private const byte ADI_WIL_API_GET_MONITOR_PARAMETERS_CRC = 32;
        private const byte ADI_WIL_API_ASSESS_NETWORK_TOPOLOGY = 33;
        private const byte ADI_WIL_API_APPLY_NETWORK_TOPOLOGY = 34;
        private const byte ADI_WIL_API_CONFIGURE_CELL_BALANCING = 35;
        private const byte ADI_WIL_API_GET_CELL_BALANCING_STATUS = 36;

        private const byte ADI_WIL_ERR_SUCCESS = 0;
        private const byte ADI_WIL_ERR_FAIL = 1;
        private const byte ADI_WIL_ERR_API_IN_PROGRESS = 2;
        private const byte ADI_WIL_ERR_TIMEOUT = 3;
        private const byte ADI_WIL_ERR_NOT_CONNECTED = 4;
        private const byte ADI_WIL_ERR_INVALID_PARAMETER = 5;
        private const byte ADI_WIL_ERR_INVALID_STATE = 6;
        private const byte ADI_WIL_ERR_NOT_SUPPORTED = 7;
        private const byte ADI_WIL_ERR_EXTERNAL = 8;
        private const byte ADI_WIL_ERR_INVALID_MODE = 9;
        private const byte ADI_WIL_ERR_IN_PROGRESS = 10;
        private const byte ADI_WIL_ERR_CONFIGURATION_MISMATCH = 11;
        private const byte ADI_WIL_ERR_CRC = 12;
        private const byte ADI_WIL_ERR_FILE_REJECTED = 13;
        private const byte ADI_WIL_ERR_PARTIAL_SUCCESS = 14;

        private const byte ADI_WIL_MODE_STANDBY = 1;
        private const byte ADI_WIL_MODE_COMMISSIONING = 2;
        private const byte ADI_WIL_MODE_ACTIVE = 3;
        private const byte ADI_WIL_MODE_MONITORING = 4;
        private const byte ADI_WIL_MODE_OTAP = 5;
        private const byte ADI_WIL_MODE_SLEEP = 6;

        private const int API_PACKET_TYPE = 0;
        private const int BMS_PACKET_TYPE = 1;
        private const int PMS_PACKET_TYPE = 2;
        private const int EMS_PACKET_TYPE = 3;
        private const int NETWORK_STATUS_PACKET_TYPE = 4;
        private const int HEALTH_REPORTS_PACKET_TYPE = 5;
        private const int SPI_STATS_PACKET_TYPE = 6;
        private const int WIL_STATS_PACKET_TYPE = 7;
        private const int EVENT_PACKET_TYPE = 8;
        private const int MAX_NODES = 64;
        private const int SOURCE_WIL = 224;
        private const int SOURCE_HOST = 225;
        private const int MANAGER_0_ID = 240;
        private const int MANAGER_1_ID = 241;
        private const int byte_number = 0;

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
            StartUDPServer(new UDP0("127.0.0.1", 9011, TabControl1));
            StartUDPServer(new UDP1("192.168.3.100", 9020, TabControl1));
            StartUDPServer(new UDP1("192.168.3.100", 9021, TabControl1));
            StartUDPServer(new UDP1("192.168.3.100", 9022, TabControl1));

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

        /* //TODO: understand generics
        private void StartUDPServer1<UDPListener>(string IPAdd, int port) where UDPListener : new()
        {
            //开启线程
            UDPListener uDP0 = new UDPListener("127.0.0.1", 9011, TabControl1);
            ThreadStart ts = new ThreadStart(uDP0.Start);
            Thread tc = new Thread(ts);
            tc.IsBackground = true;
            tc.Start();
        }
        */

        
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