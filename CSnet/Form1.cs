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

        private HashSet<String> openedDevicesSerials = new HashSet<String>();
        private void CmdOpenDevices_Click(object sender, EventArgs e)
        {
            int iResult;
            NeoDeviceEx[] ndNeoToOpenex = new NeoDeviceEx[16];	//Struct holding detected hardware information
            OptionsOpenNeoEx OptionOpenNeoEX = new OptionsOpenNeoEx();
            byte[] bNetwork = new byte[255];    //List of hardware IDs
            int iNumberOfDevices;   //Number of hardware devices to look for 
            int iCount;		 //counter
            UInt32 StringSize = 6;
            string sConvertedSN = "      ";
            byte[] bSN = new byte[6];

            //File NetworkID array
            for (iCount = 0; iCount < 255; iCount++)
            {
                bNetwork[iCount] = Convert.ToByte(iCount);
            }

            //Set the number of devices to find, for this example look for 16
            iNumberOfDevices = 15;

            //Search for connected hardware
            //iResult = icsNeoDll.icsneoFindNeoDevices((uint)eHardwareTypes.NEODEVICE_ALL, ref ndNeoToOpen, ref iNumberOfDevices);
            iResult = icsNeoDll.icsneoFindDevices(ref ndNeoToOpenex[0], ref iNumberOfDevices, 0, 0, ref neoDeviceOption, 0);

            if (iResult == 0)
            {
                MessageBox.Show("Problem finding devices");
                return;
            }

            if (iNumberOfDevices < 1)
            {
                MessageBox.Show("No devices found");
                return;
            }

            for (int i = 0; i < iNumberOfDevices; i++)
            {
                //Convert the Serial number to a String
                icsNeoDll.icsneoSerialNumberToString(Convert.ToUInt32(ndNeoToOpenex[i].neoDevice.SerialNumber), ref bSN[0], ref StringSize);
                sConvertedSN = Encoding.ASCII.GetString(bSN);

                //keep track of devices that have already been opened
                if (openedDevicesSerials.Add(sConvertedSN))
                {
                    IntPtr anonymousPointer = IntPtr.Zero;
                    iResult = icsNeoDll.icsneoOpenDevice(ref ndNeoToOpenex[i], ref anonymousPointer, ref bNetwork[0], 1, 0, ref OptionOpenNeoEX, 0);
                    if (iResult == 1)
                    {
                        MessageBox.Show("Port Opened OK!");
                    }
                    else
                    {
                        //MessageBox.Show("Problem Opening Port");
                        Debug.WriteLine("Not opened");
                        return;
                    }

                    //create new tab per device
                    TabPage tabPage = new TabPage($"Device {sConvertedSN}");
                    tabPage.Controls.Add(new OpenDeviceTab(ref anonymousPointer) { Name = $"{sConvertedSN}" });
                    this.TabControl1.TabPages.Add(tabPage);
                }
            }
        }

        private void cmdCloseDevice_Click(object sender, EventArgs e)
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
            //TabControl2.SelectTab(0);
            //开启线程
            ThreadStart ts = new ThreadStart(UDPListener);
            Thread tc = new Thread(ts);
            tc.IsBackground = true;
            tc.Start();

            //Add the version number to the title of the application.  
            try
            {
                this.Text = "ICS API Example API Version " + Convert.ToString(icsNeoDll.icsneoGetDLLVersion());
            }
            catch
            {
                this.Text = "ICS API Example, DLL Missing or Not Accessable";
            }
        }

        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const byte BMSPACKET = 0;
        private const byte BMSTEMPPACKET = 1;
        private const byte PMSPACKET = 2;
        private const byte EMSPACKET = 3;
        private const byte ModuleVersion = 4;
        private const byte PackVersion = 5;
        private const byte DiagnosticVoltage = 6;

        private void UDPListener()
        {
            //server.Bind(new IPEndPoint(IPAddress.Any, 9001));
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9011));          

            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[12];

            while (true)
            {
                try
                {
                    server.ReceiveFrom(data, ref remoteEnd);

                    /*
                     * UDP request format:
                     * Serial Number from ADI box
                     * Type of Packet, BMS 0, BMS Temperature 1, PMS 2, EMS 3
                     * BMS -> Module number
                     * BMS Temperature -> Module number
                     * PMS -> 0
                     * EMS -> 0
                     * 
                     * e.g request for pms data
                     * BS0232 2 0
                     *  
                     * e.g request for bms data from module 24
                     * BS0232 0 24
                     */

                    //iterate through each open device
                    foreach (TabPage tabPage in TabControl1.TabPages)
                    {
                        OpenDeviceTab uc1 = (OpenDeviceTab)tabPage.Controls[0];

                        //check serial number against request                        
                        if (uc1.Name == "BS0226")//TODO: actually parse serial number
                        {
                            if (data[1] == BMSPACKET)
                            {
                                //send raw BMS packet to UDP client
                                server.SendTo(uc1.modules[data[2]].Packet0, remoteEnd);
                            }
                            else if (data[1] == BMSTEMPPACKET)
                            {
                                //send temperature data to UDP client
                                byte[] tempPacket = new byte[8];

                                byte[] raw1 = BitConverter.GetBytes(uc1.modules[data[2]].Thermistor1);
                                byte[] raw2 = BitConverter.GetBytes(uc1.modules[data[2]].Thermistor2);

                                //将温度数组组装到返回数组里面
                                tempPacket[0] = uc1.modules[data[2]].Thermistor1Raw[0];
                                tempPacket[1] = uc1.modules[data[2]].Thermistor1Raw[1];
                                tempPacket[2] = uc1.modules[data[2]].Thermistor2Raw[0];
                                tempPacket[3] = uc1.modules[data[2]].Thermistor2Raw[1];
                                tempPacket[4] = raw1[0];
                                tempPacket[5] = raw1[1];
                                tempPacket[6] = raw2[0];
                                tempPacket[7] = raw2[1];

                                double temp1 = uc1.modules[data[2]].Thermistor1;
                                double temp2 = uc1.modules[data[2]].Thermistor2;

                                short temp3 = (Int16)(temp1 * 100);
                                short temp4 = (Int16)(temp2 * 100);

                                // byte BitConverter.GetBytes(temp3);
                                // TemCal[]= BitConverter.GetBytes(temp3);
                                byte[] aaa = BitConverter.GetBytes(temp3);
                                byte[] bbb = BitConverter.GetBytes(temp4);
                                //将温度数组组装到返回数组里面
                                tempPacket[4] = aaa[0];
                                tempPacket[5] = aaa[1];
                                tempPacket[6] = bbb[0];
                                tempPacket[7] = bbb[1];

                                server.SendTo(tempPacket, remoteEnd);
                            }
                            else if (data[1] == PMSPACKET)
                            {
                                //for example:
                                uc1.managers[0].GetBEVDCFCContactorTemp();

                                byte[] tempPacket = BitConverter.GetBytes(uc1.managers[0].I1)
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].I2))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].VBAT))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX1))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX2))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX3))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX4))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX5))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].AUX6))
                                    .ToArray();

                                server.SendTo(tempPacket, remoteEnd);
                            }
                            else if (data[1] == EMSPACKET)
                            {


                                byte[] tempPacket = BitConverter.GetBytes(uc1.managers[0].G1V)
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G2V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G3V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G4V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G5V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G6V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G7V))
                                    //.Concat(BitConverter.GetBytes(uc1.managers[0].GetBEVDCFCMinus()))
                                    .ToArray();

                                server.SendTo(tempPacket, remoteEnd);
                            }
                            else if (data[1] == ModuleVersion)
                            {
                                //send raw BMS packet to UDP client
                                byte[] modulesVersion = new byte[1];
                                modulesVersion[0] = (byte)uc1.modules[data[2]].version;


                                server.SendTo(modulesVersion, remoteEnd);
                            }
                            else if (data[1] == PackVersion)
                            {
                                byte[] packVersion = new byte[1];
                                //firmwareLabel.Text
                                // string Addr1 = MyINI.GetIniKeyValueForStr("PACK", "version", Application.StartupPath + "\\config.ini");

                                packVersion[0] = byte.Parse(uconst.packVersion);
                                server.SendTo(packVersion, remoteEnd);
                            }
                            else if (data[1] == DiagnosticVoltage)
                            {
                                int[] data1 = uconst.DoubleArrayToIntArray(uc1.modules[data[2]].CGDV);
                                byte[] CGDV1 = uconst.IntArrayToByteArray(data1);
                                server.SendTo(CGDV1, remoteEnd);
                            }
                            else
                            {
                                //request not recongised, send back FF
                                server.SendTo(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, remoteEnd);
                            }
                        }
                    }

                    //Thread.Sleep(6);
                }
                catch (Exception ex)
                {
                    //throw ex;
                    Console.WriteLine($"发送失败：{ex.Message}");
                }
            }
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
