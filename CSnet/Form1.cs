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
            //TabControl2.SelectTab(0);
            //开启线程
            ThreadStart ts = new ThreadStart(UDPListener);
            Thread tc = new Thread(ts);
            tc.IsBackground = true;
            tc.Start();

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

        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Socket server1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Socket server2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Socket server3 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        private const byte BMSPACKET = 0;
        private const byte BMSTEMPPACKET = 1;
        private const byte PMSPACKET = 2;
        private const byte EMSPACKET = 3;
        private const byte ModuleVersion = 4;
        private const byte PackVersion = 5;
        private const byte DiagnosticVoltage = 6;
        private const byte Latency = 7;
        private const byte TotalPECErrors = 8;
        private const byte AverageUpdateRate = 9;
        private const byte PeakUpdateRate = 10;
        private const byte AverageRSSI = 11;
        private const byte PeakRSSI = 12;
        private const byte BEV = 13;
        private const byte BET = 14;
        private const byte OP90 = 15;

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
                     * Type of Packet, BMS 0, BMS Temperature 1, PMS 2, EMS 3, Setup command 13
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
                        if (uc1.Name.Equals(Encoding.ASCII.GetString(data.Take(6).ToArray())))
                        {
                            if (data[6] == BEV)
                            {
                                uc1.Setup(false, true);
                            }
                            else if (data[6] == BET)
                            {
                                uc1.Setup(true, true);
                            }else if (data[6] == OP90)
                            {
                                uc1.Setup(false, false);
                            } else if (data[1] == BMSPACKET)
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


                                byte[] tempPacket = BitConverter.GetBytes(uc1.managers[0].CD1V)
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSReferenceVoltage1())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSReferenceVoltage2())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSTemperature1())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSTemperature2())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSPressure1())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSPressure2())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSGas1())))
                                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(uc1.managers[0].EMSGas2())))

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
                            else if (data[1] == Latency)
                            {
                                //                         private const byte Latency = 7;
                                //private const byte TotalPECErrors = 8;
                                //private const byte AverageUpdateRate = 9;
                                //private const byte PeakUpdateRate = 10;
                                //private const byte AverageRSSI = 11;
                                //private const byte PeakRSSI = 12;
                                int data1 = uc1.modules[data[2]].Latency;
                                byte[] latency = BitConverter.GetBytes(data1);
                                server.SendTo(latency, remoteEnd);
                            }
                            else if (data[1] == TotalPECErrors)
                            {
                                int data1 = uc1.modules[data[2]].TotalPECErrors;
                                byte[] latency = BitConverter.GetBytes(data1);
                                server.SendTo(latency, remoteEnd);
                            }
                            else if (data[1] == AverageUpdateRate)
                            {
                                double data1 = uc1.modules[data[2]].AverageUpdateRate;
                                byte[] latency = BitConverter.GetBytes(data1);
                                server.SendTo(latency, remoteEnd);
                            }
                            else if (data[1] == PeakUpdateRate)
                            {
                                double data1 = uc1.modules[data[2]].PeakUpdateRate;
                                byte[] latency = BitConverter.GetBytes(data1);                                
                                server.SendTo(latency, remoteEnd);
                            }
                            else if (data[1] == AverageRSSI)
                            {
                                int data1 = uc1.modules[data[2]].AverageRSSI;
                                byte[] latency = BitConverter.GetBytes(data1);
                                server.SendTo(latency, remoteEnd);
                            }
                            else if (data[1] == PeakRSSI)
                            {
                                int data1 = uc1.modules[data[2]].PeakRSSI;
                                byte[] latency = BitConverter.GetBytes(data1);
                                server.SendTo(latency, remoteEnd);
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


        private void UDPListener1()
        {
            //server.Bind(new IPEndPoint(IPAddress.Any, 9001));
            server1.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9020));

            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[12];

            while (true)
            {
                try
                {
                    server1.ReceiveFrom(data, ref remoteEnd);

                    foreach (TabPage tabPage in TabControl1.TabPages)
                    {
                        OpenDeviceTab uc1 = (OpenDeviceTab)tabPage.Controls[0];

                        //check serial number against request                        
                        if (uc1.Name != "")//TODO: actually parse serial number
                        {
                            if (data[1] == BMSPACKET)
                            {
                                //send raw BMS packet to UDP client
                                if (uc1.modules.Length <= 12)//12个模组
                                {
                                    byte[] dataAll = new byte[uc1.modules[0].Packet0.Length * 12];

                                    // uc1.modules[0].Packet0.Length

                                    for (int i = 0; i < 12; i++)
                                    {
                                        Array.Copy(uc1.modules[i].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * i, uc1.modules[i].Packet0.Length);
                                    }


                                    //将所有的电压合在一起
                                    server1.SendTo(dataAll, remoteEnd);
                                }
                                else if (uc1.modules.Length >= 24)//24个模组{ //TODO: why is there a difference between 12 & 24?
                                {
                                    byte[] dataAll = new byte[uc1.modules[0].Packet0.Length * 24];

                                    // uc1.modules[0].Packet0.Length
                                    for(int i = 0; i <24; i++)
                                    {
                                        Array.Copy(uc1.modules[i].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * i, uc1.modules[i].Packet0.Length);
                                    }

                                    //将所有的电压合在一起
                                    server1.SendTo(dataAll, remoteEnd);
                                }
                                else//没有数据
                                {
                                    server1.SendTo(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, remoteEnd);
                                }
                            }
                            if (data[1] == BMSTEMPPACKET)
                            {
                                byte[] tempPacket = new byte[0];
                                if (uc1.modules.Length <= 12)//12个模组
                                {
                                    for(int i = 0; i < 12; i++){
                                        tempPacket = tempPacket.Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor1 * 100))
                                        .Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor2 * 100)).ToArray();
                                    }

                                    server1.SendTo(tempPacket, remoteEnd);
                                }
                                else if (uc1.modules.Length >= 24)//24个模组 //TODO: why is this here? Why does it matter if there are 12 or 24 modules??
                                {
                                    for (int i = 0; i < 24; i++)
                                    {
                                        tempPacket = tempPacket.Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor1 * 100))
                                        .Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor2 * 100)).ToArray();
                                    }

                                    server1.SendTo(tempPacket, remoteEnd);
                                }
                                else//没有连接的情况
                                {
                                    server1.SendTo(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, remoteEnd);
                                }
                            }

                            else
                            {
                                //request not recongised, send back FF
                                server1.SendTo(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, remoteEnd);
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