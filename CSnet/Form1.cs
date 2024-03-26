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
            NeoDeviceEx[] ndNeoToOpenex = new NeoDeviceEx[16];	//Struct holding detected hardware information, TODO: this is limiting us to 16 devices
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
                        if (uc1.Name != "")//TODO: actually parse serial number
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

                                    Array.Copy(uc1.modules[0].Packet0, 0, dataAll, 0, uc1.modules[0].Packet0.Length);//复制模组1
                                    Array.Copy(uc1.modules[1].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length, uc1.modules[1].Packet0.Length);//复制模组2
                                    Array.Copy(uc1.modules[2].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 2, uc1.modules[2].Packet0.Length);//复制模组3
                                    Array.Copy(uc1.modules[3].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 3, uc1.modules[3].Packet0.Length);//复制模组4
                                    Array.Copy(uc1.modules[4].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 4, uc1.modules[4].Packet0.Length);//复制模组5
                                    Array.Copy(uc1.modules[5].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 5, uc1.modules[5].Packet0.Length);//复制模组6
                                    Array.Copy(uc1.modules[6].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 6, uc1.modules[6].Packet0.Length);//复制模组7
                                    Array.Copy(uc1.modules[7].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 7, uc1.modules[7].Packet0.Length);//复制模组8
                                    Array.Copy(uc1.modules[8].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 8, uc1.modules[8].Packet0.Length);//复制模组9
                                    Array.Copy(uc1.modules[9].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 9, uc1.modules[9].Packet0.Length);//复制模组10
                                    Array.Copy(uc1.modules[10].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 10, uc1.modules[10].Packet0.Length);//复制模组11
                                    Array.Copy(uc1.modules[11].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 11, uc1.modules[11].Packet0.Length);//复制模组12

                                    //将所有的电压合在一起
                                    server1.SendTo(dataAll, remoteEnd);
                                }
                                else if (uc1.modules.Length >= 24)//24个模组{
                                {
                                    byte[] dataAll = new byte[uc1.modules[0].Packet0.Length * 24];

                                    // uc1.modules[0].Packet0.Length

                                    Array.Copy(uc1.modules[0].Packet0, 0, dataAll, 0, uc1.modules[0].Packet0.Length);//复制模组1
                                    Array.Copy(uc1.modules[1].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length, uc1.modules[1].Packet0.Length);//复制模组2
                                    Array.Copy(uc1.modules[2].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 2, uc1.modules[2].Packet0.Length);//复制模组3
                                    Array.Copy(uc1.modules[3].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 3, uc1.modules[3].Packet0.Length);//复制模组4
                                    Array.Copy(uc1.modules[4].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 4, uc1.modules[4].Packet0.Length);//复制模组5
                                    Array.Copy(uc1.modules[5].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 5, uc1.modules[5].Packet0.Length);//复制模组6
                                    Array.Copy(uc1.modules[6].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 6, uc1.modules[6].Packet0.Length);//复制模组7
                                    Array.Copy(uc1.modules[7].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 7, uc1.modules[7].Packet0.Length);//复制模组8
                                    Array.Copy(uc1.modules[8].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 8, uc1.modules[8].Packet0.Length);//复制模组9
                                    Array.Copy(uc1.modules[9].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 9, uc1.modules[9].Packet0.Length);//复制模组10
                                    Array.Copy(uc1.modules[10].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 10, uc1.modules[10].Packet0.Length);//复制模组11
                                    Array.Copy(uc1.modules[11].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 11, uc1.modules[11].Packet0.Length);//复制模组12
                                    Array.Copy(uc1.modules[12].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 12, uc1.modules[12].Packet0.Length);//复制模组13
                                    Array.Copy(uc1.modules[13].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 13, uc1.modules[13].Packet0.Length);//复制模组14
                                    Array.Copy(uc1.modules[14].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 14, uc1.modules[14].Packet0.Length);//复制模组15
                                    Array.Copy(uc1.modules[15].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 15, uc1.modules[15].Packet0.Length);//复制模组16
                                    Array.Copy(uc1.modules[16].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 16, uc1.modules[16].Packet0.Length);//复制模组17
                                    Array.Copy(uc1.modules[17].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 17, uc1.modules[17].Packet0.Length);//复制模组18
                                    Array.Copy(uc1.modules[18].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 18, uc1.modules[18].Packet0.Length);//复制模组19
                                    Array.Copy(uc1.modules[19].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 19, uc1.modules[19].Packet0.Length);//复制模组20
                                    Array.Copy(uc1.modules[20].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 20, uc1.modules[20].Packet0.Length);//复制模组21
                                    Array.Copy(uc1.modules[21].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 21, uc1.modules[21].Packet0.Length);//复制模组22
                                    Array.Copy(uc1.modules[22].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 22, uc1.modules[22].Packet0.Length);//复制模组23
                                    Array.Copy(uc1.modules[23].Packet0, 0, dataAll, uc1.modules[0].Packet0.Length * 23, uc1.modules[23].Packet0.Length);//复制模组24

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
                                byte[] tempPacket = new byte[48];
                                if (uc1.modules.Length <= 12)//12个模组
                                {
                                    double temp1 = uc1.modules[0].Thermistor1;
                                    double temp2 = uc1.modules[0].Thermistor2;
                                    double temp3 = uc1.modules[1].Thermistor1;
                                    double temp4 = uc1.modules[1].Thermistor2;
                                    double temp5 = uc1.modules[2].Thermistor1;
                                    double temp6 = uc1.modules[2].Thermistor2;
                                    double temp7 = uc1.modules[3].Thermistor1;
                                    double temp8 = uc1.modules[3].Thermistor2;
                                    double temp9 = uc1.modules[4].Thermistor1;
                                    double temp10 = uc1.modules[4].Thermistor2;
                                    double temp11 = uc1.modules[5].Thermistor1;
                                    double temp12 = uc1.modules[5].Thermistor2;
                                    double temp13 = uc1.modules[6].Thermistor1;
                                    double temp14 = uc1.modules[6].Thermistor2;
                                    double temp15 = uc1.modules[7].Thermistor1;
                                    double temp16 = uc1.modules[7].Thermistor2;
                                    double temp17 = uc1.modules[8].Thermistor1;
                                    double temp18 = uc1.modules[8].Thermistor2;
                                    double temp19 = uc1.modules[9].Thermistor1;
                                    double temp20 = uc1.modules[9].Thermistor2;
                                    double temp21 = uc1.modules[10].Thermistor1;
                                    double temp22 = uc1.modules[10].Thermistor2;
                                    double temp23 = uc1.modules[11].Thermistor1;
                                    double temp24 = uc1.modules[11].Thermistor2;
                                    short Temp1 = (Int16)(temp1 * 100);
                                    short Temp2 = (Int16)(temp2 * 100);
                                    short Temp3 = (Int16)(temp3 * 100);
                                    short Temp4 = (Int16)(temp4 * 100);
                                    short Temp5 = (Int16)(temp5 * 100);
                                    short Temp6 = (Int16)(temp6 * 100);
                                    short Temp7 = (Int16)(temp7 * 100);
                                    short Temp8 = (Int16)(temp8 * 100);
                                    short Temp9 = (Int16)(temp9 * 100);
                                    short Temp10 = (Int16)(temp10 * 100);
                                    short Temp11 = (Int16)(temp11 * 100);
                                    short Temp12 = (Int16)(temp12 * 100);
                                    short Temp13 = (Int16)(temp13 * 100);
                                    short Temp14 = (Int16)(temp14 * 100);
                                    short Temp15 = (Int16)(temp15 * 100);
                                    short Temp16 = (Int16)(temp16 * 100);
                                    short Temp17 = (Int16)(temp17 * 100);
                                    short Temp18 = (Int16)(temp18 * 100);
                                    short Temp19 = (Int16)(temp19 * 100);
                                    short Temp20 = (Int16)(temp20 * 100);
                                    short Temp21 = (Int16)(temp21 * 100);
                                    short Temp22 = (Int16)(temp22 * 100);
                                    short Temp23 = (Int16)(temp23 * 100);
                                    short Temp24 = (Int16)(temp24 * 100);

                                    //// byte BitConverter.GetBytes(temp3);
                                    //// TemCal[]= BitConverter.GetBytes(temp3);
                                    byte[] aaa1 = BitConverter.GetBytes(Temp1);
                                    byte[] aaa2 = BitConverter.GetBytes(Temp2);
                                    byte[] aaa3 = BitConverter.GetBytes(Temp3);
                                    byte[] aaa4 = BitConverter.GetBytes(Temp4);
                                    byte[] aaa5 = BitConverter.GetBytes(Temp5);
                                    byte[] aaa6 = BitConverter.GetBytes(Temp6);
                                    byte[] aaa7 = BitConverter.GetBytes(Temp7);
                                    byte[] aaa8 = BitConverter.GetBytes(Temp8);
                                    byte[] aaa9 = BitConverter.GetBytes(Temp9);
                                    byte[] aaa10 = BitConverter.GetBytes(Temp10);
                                    byte[] aaa11 = BitConverter.GetBytes(Temp11);
                                    byte[] aaa12 = BitConverter.GetBytes(Temp12);
                                    byte[] aaa13 = BitConverter.GetBytes(Temp13);
                                    byte[] aaa14 = BitConverter.GetBytes(Temp14);
                                    byte[] aaa15 = BitConverter.GetBytes(Temp15);
                                    byte[] aaa16 = BitConverter.GetBytes(Temp16);
                                    byte[] aaa17 = BitConverter.GetBytes(Temp17);
                                    byte[] aaa18 = BitConverter.GetBytes(Temp18);
                                    byte[] aaa19 = BitConverter.GetBytes(Temp19);
                                    byte[] aaa20 = BitConverter.GetBytes(Temp20);
                                    byte[] aaa21 = BitConverter.GetBytes(Temp21);
                                    byte[] aaa22 = BitConverter.GetBytes(Temp22);
                                    byte[] aaa23 = BitConverter.GetBytes(Temp23);
                                    byte[] aaa24 = BitConverter.GetBytes(Temp24);


                                    ////将温度数组组装到返回数组里面
                                    tempPacket[0] = aaa1[0];
                                    tempPacket[1] = aaa1[1];
                                    tempPacket[2] = aaa2[0];
                                    tempPacket[3] = aaa2[1];
                                    tempPacket[4] = aaa3[0];
                                    tempPacket[5] = aaa3[1];
                                    tempPacket[6] = aaa4[0];
                                    tempPacket[7] = aaa4[1];
                                    tempPacket[8] = aaa5[0];
                                    tempPacket[9] = aaa5[1];
                                    tempPacket[10] = aaa6[0];
                                    tempPacket[11] = aaa6[1];
                                    tempPacket[12] = aaa7[0];
                                    tempPacket[13] = aaa7[1];
                                    tempPacket[14] = aaa8[0];
                                    tempPacket[15] = aaa8[1];
                                    tempPacket[16] = aaa9[0];
                                    tempPacket[17] = aaa9[1];
                                    tempPacket[18] = aaa10[0];
                                    tempPacket[19] = aaa10[1];
                                    tempPacket[20] = aaa11[0];
                                    tempPacket[21] = aaa11[1];
                                    tempPacket[22] = aaa12[0];
                                    tempPacket[23] = aaa12[1];
                                    tempPacket[24] = aaa13[0];
                                    tempPacket[25] = aaa13[1];
                                    tempPacket[26] = aaa14[0];
                                    tempPacket[27] = aaa14[1];
                                    tempPacket[28] = aaa15[0];
                                    tempPacket[29] = aaa15[1];
                                    tempPacket[30] = aaa16[0];
                                    tempPacket[31] = aaa16[1];
                                    tempPacket[32] = aaa17[0];
                                    tempPacket[33] = aaa17[1];
                                    tempPacket[34] = aaa18[0];
                                    tempPacket[35] = aaa18[1];
                                    tempPacket[36] = aaa19[0];
                                    tempPacket[37] = aaa19[1];
                                    tempPacket[38] = aaa20[0];
                                    tempPacket[39] = aaa20[1];
                                    tempPacket[40] = aaa21[0];
                                    tempPacket[41] = aaa21[1];
                                    tempPacket[42] = aaa22[0];
                                    tempPacket[43] = aaa22[1];
                                    tempPacket[44] = aaa23[0];
                                    tempPacket[45] = aaa23[1];
                                    tempPacket[46] = aaa24[0];
                                    tempPacket[47] = aaa24[1];

                                    server1.SendTo(tempPacket, remoteEnd);
                                }
                                else if (uc1.modules.Length >= 24)//24个模组
                                {
                                    double temp1 = uc1.modules[0].Thermistor1;
                                    double temp2 = uc1.modules[0].Thermistor2;
                                    double temp3 = uc1.modules[1].Thermistor1;
                                    double temp4 = uc1.modules[1].Thermistor2;
                                    double temp5 = uc1.modules[2].Thermistor1;
                                    double temp6 = uc1.modules[2].Thermistor2;
                                    double temp7 = uc1.modules[3].Thermistor1;
                                    double temp8 = uc1.modules[3].Thermistor2;
                                    double temp9 = uc1.modules[4].Thermistor1;
                                    double temp10 = uc1.modules[4].Thermistor2;
                                    double temp11 = uc1.modules[5].Thermistor1;
                                    double temp12 = uc1.modules[5].Thermistor2;
                                    double temp13 = uc1.modules[6].Thermistor1;
                                    double temp14 = uc1.modules[6].Thermistor2;
                                    double temp15 = uc1.modules[7].Thermistor1;
                                    double temp16 = uc1.modules[7].Thermistor2;
                                    double temp17 = uc1.modules[8].Thermistor1;
                                    double temp18 = uc1.modules[8].Thermistor2;
                                    double temp19 = uc1.modules[9].Thermistor1;
                                    double temp20 = uc1.modules[9].Thermistor2;
                                    double temp21 = uc1.modules[10].Thermistor1;
                                    double temp22 = uc1.modules[10].Thermistor2;
                                    double temp23 = uc1.modules[11].Thermistor1;
                                    double temp24 = uc1.modules[11].Thermistor2;
                                    double temp25 = uc1.modules[12].Thermistor1;
                                    double temp26 = uc1.modules[12].Thermistor2;
                                    double temp27 = uc1.modules[13].Thermistor1;
                                    double temp28 = uc1.modules[13].Thermistor2;
                                    double temp29 = uc1.modules[14].Thermistor1;
                                    double temp30 = uc1.modules[14].Thermistor2;
                                    double temp31 = uc1.modules[15].Thermistor1;
                                    double temp32 = uc1.modules[15].Thermistor2;
                                    double temp33 = uc1.modules[16].Thermistor1;
                                    double temp34 = uc1.modules[16].Thermistor2;
                                    double temp35 = uc1.modules[17].Thermistor1;
                                    double temp36 = uc1.modules[17].Thermistor2;
                                    double temp37 = uc1.modules[18].Thermistor1;
                                    double temp38 = uc1.modules[18].Thermistor2;
                                    double temp39 = uc1.modules[19].Thermistor1;
                                    double temp40 = uc1.modules[19].Thermistor2;
                                    double temp41 = uc1.modules[20].Thermistor1;
                                    double temp42 = uc1.modules[20].Thermistor2;
                                    double temp43 = uc1.modules[21].Thermistor1;
                                    double temp44 = uc1.modules[21].Thermistor2;
                                    double temp45 = uc1.modules[22].Thermistor1;
                                    double temp46 = uc1.modules[22].Thermistor2;
                                    double temp47 = uc1.modules[23].Thermistor1;
                                    double temp48 = uc1.modules[23].Thermistor2;

                                    short Temp1 = (Int16)(temp1 * 100);
                                    short Temp2 = (Int16)(temp2 * 100);
                                    short Temp3 = (Int16)(temp3 * 100);
                                    short Temp4 = (Int16)(temp4 * 100);
                                    short Temp5 = (Int16)(temp5 * 100);
                                    short Temp6 = (Int16)(temp6 * 100);
                                    short Temp7 = (Int16)(temp7 * 100);
                                    short Temp8 = (Int16)(temp8 * 100);
                                    short Temp9 = (Int16)(temp9 * 100);
                                    short Temp10 = (Int16)(temp10 * 100);
                                    short Temp11 = (Int16)(temp11 * 100);
                                    short Temp12 = (Int16)(temp12 * 100);
                                    short Temp13 = (Int16)(temp13 * 100);
                                    short Temp14 = (Int16)(temp14 * 100);
                                    short Temp15 = (Int16)(temp15 * 100);
                                    short Temp16 = (Int16)(temp16 * 100);
                                    short Temp17 = (Int16)(temp17 * 100);
                                    short Temp18 = (Int16)(temp18 * 100);
                                    short Temp19 = (Int16)(temp19 * 100);
                                    short Temp20 = (Int16)(temp20 * 100);
                                    short Temp21 = (Int16)(temp21 * 100);
                                    short Temp22 = (Int16)(temp22 * 100);
                                    short Temp23 = (Int16)(temp23 * 100);
                                    short Temp24 = (Int16)(temp24 * 100);
                                    short Temp25 = (Int16)(temp25 * 100);
                                    short Temp26 = (Int16)(temp26 * 100);
                                    short Temp27 = (Int16)(temp27 * 100);
                                    short Temp28 = (Int16)(temp28 * 100);
                                    short Temp29 = (Int16)(temp29 * 100);
                                    short Temp30 = (Int16)(temp30 * 100);
                                    short Temp31 = (Int16)(temp31 * 100);
                                    short Temp32 = (Int16)(temp32 * 100);
                                    short Temp33 = (Int16)(temp33 * 100);
                                    short Temp34 = (Int16)(temp34 * 100);
                                    short Temp35 = (Int16)(temp35 * 100);
                                    short Temp36 = (Int16)(temp36 * 100);
                                    short Temp37 = (Int16)(temp37 * 100);
                                    short Temp38 = (Int16)(temp38 * 100);
                                    short Temp39 = (Int16)(temp39 * 100);
                                    short Temp40 = (Int16)(temp40 * 100);
                                    short Temp41 = (Int16)(temp41 * 100);
                                    short Temp42 = (Int16)(temp42 * 100);
                                    short Temp43 = (Int16)(temp43 * 100);
                                    short Temp44 = (Int16)(temp44 * 100);
                                    short Temp45 = (Int16)(temp45 * 100);
                                    short Temp46 = (Int16)(temp46 * 100);
                                    short Temp47 = (Int16)(temp47 * 100);
                                    short Temp48 = (Int16)(temp48 * 100);


                                    //// byte BitConverter.GetBytes(temp3);
                                    //// TemCal[]= BitConverter.GetBytes(temp3);
                                    byte[] aaa1 = BitConverter.GetBytes(Temp1);
                                    byte[] aaa2 = BitConverter.GetBytes(Temp2);
                                    byte[] aaa3 = BitConverter.GetBytes(Temp3);
                                    byte[] aaa4 = BitConverter.GetBytes(Temp4);
                                    byte[] aaa5 = BitConverter.GetBytes(Temp5);
                                    byte[] aaa6 = BitConverter.GetBytes(Temp6);
                                    byte[] aaa7 = BitConverter.GetBytes(Temp7);
                                    byte[] aaa8 = BitConverter.GetBytes(Temp8);
                                    byte[] aaa9 = BitConverter.GetBytes(Temp9);
                                    byte[] aaa10 = BitConverter.GetBytes(Temp10);
                                    byte[] aaa11 = BitConverter.GetBytes(Temp11);
                                    byte[] aaa12 = BitConverter.GetBytes(Temp12);
                                    byte[] aaa13 = BitConverter.GetBytes(Temp13);
                                    byte[] aaa14 = BitConverter.GetBytes(Temp14);
                                    byte[] aaa15 = BitConverter.GetBytes(Temp15);
                                    byte[] aaa16 = BitConverter.GetBytes(Temp16);
                                    byte[] aaa17 = BitConverter.GetBytes(Temp17);
                                    byte[] aaa18 = BitConverter.GetBytes(Temp18);
                                    byte[] aaa19 = BitConverter.GetBytes(Temp19);
                                    byte[] aaa20 = BitConverter.GetBytes(Temp20);
                                    byte[] aaa21 = BitConverter.GetBytes(Temp21);
                                    byte[] aaa22 = BitConverter.GetBytes(Temp22);
                                    byte[] aaa23 = BitConverter.GetBytes(Temp23);
                                    byte[] aaa24 = BitConverter.GetBytes(Temp24);
                                    byte[] aaa25 = BitConverter.GetBytes(Temp25);
                                    byte[] aaa26 = BitConverter.GetBytes(Temp26);
                                    byte[] aaa27 = BitConverter.GetBytes(Temp27);
                                    byte[] aaa28 = BitConverter.GetBytes(Temp28);
                                    byte[] aaa29 = BitConverter.GetBytes(Temp29);
                                    byte[] aaa30 = BitConverter.GetBytes(Temp30);
                                    byte[] aaa31 = BitConverter.GetBytes(Temp31);
                                    byte[] aaa32 = BitConverter.GetBytes(Temp32);
                                    byte[] aaa33 = BitConverter.GetBytes(Temp33);
                                    byte[] aaa34 = BitConverter.GetBytes(Temp34);
                                    byte[] aaa35 = BitConverter.GetBytes(Temp35);
                                    byte[] aaa36 = BitConverter.GetBytes(Temp36);
                                    byte[] aaa37 = BitConverter.GetBytes(Temp37);
                                    byte[] aaa38 = BitConverter.GetBytes(Temp38);
                                    byte[] aaa39 = BitConverter.GetBytes(Temp39);
                                    byte[] aaa40 = BitConverter.GetBytes(Temp40);
                                    byte[] aaa41 = BitConverter.GetBytes(Temp41);
                                    byte[] aaa42 = BitConverter.GetBytes(Temp42);
                                    byte[] aaa43 = BitConverter.GetBytes(Temp43);
                                    byte[] aaa44 = BitConverter.GetBytes(Temp44);
                                    byte[] aaa45 = BitConverter.GetBytes(Temp45);
                                    byte[] aaa46 = BitConverter.GetBytes(Temp46);
                                    byte[] aaa47 = BitConverter.GetBytes(Temp47);
                                    byte[] aaa48 = BitConverter.GetBytes(Temp48);

                                    ////将温度数组组装到返回数组里面
                                    tempPacket[0] = aaa1[0];
                                    tempPacket[1] = aaa1[1];
                                    tempPacket[2] = aaa2[0];
                                    tempPacket[3] = aaa2[1];
                                    tempPacket[4] = aaa3[0];
                                    tempPacket[5] = aaa3[1];
                                    tempPacket[6] = aaa4[0];
                                    tempPacket[7] = aaa4[1];
                                    tempPacket[8] = aaa5[0];
                                    tempPacket[9] = aaa5[1];
                                    tempPacket[10] = aaa6[0];
                                    tempPacket[11] = aaa6[1];
                                    tempPacket[12] = aaa7[0];
                                    tempPacket[13] = aaa7[1];
                                    tempPacket[14] = aaa8[0];
                                    tempPacket[15] = aaa8[1];
                                    tempPacket[16] = aaa9[0];
                                    tempPacket[17] = aaa9[1];
                                    tempPacket[18] = aaa10[0];
                                    tempPacket[19] = aaa10[1];
                                    tempPacket[20] = aaa11[0];
                                    tempPacket[21] = aaa11[1];
                                    tempPacket[22] = aaa12[0];
                                    tempPacket[23] = aaa12[1];
                                    tempPacket[24] = aaa13[0];
                                    tempPacket[25] = aaa13[1];
                                    tempPacket[26] = aaa14[0];
                                    tempPacket[27] = aaa14[1];
                                    tempPacket[28] = aaa15[0];
                                    tempPacket[29] = aaa15[1];
                                    tempPacket[30] = aaa16[0];
                                    tempPacket[31] = aaa16[1];
                                    tempPacket[32] = aaa17[0];
                                    tempPacket[33] = aaa17[1];
                                    tempPacket[34] = aaa18[0];
                                    tempPacket[35] = aaa18[1];
                                    tempPacket[36] = aaa19[0];
                                    tempPacket[37] = aaa19[1];
                                    tempPacket[38] = aaa20[0];
                                    tempPacket[39] = aaa20[1];
                                    tempPacket[40] = aaa21[0];
                                    tempPacket[41] = aaa21[1];
                                    tempPacket[42] = aaa22[0];
                                    tempPacket[43] = aaa22[1];
                                    tempPacket[44] = aaa23[0];
                                    tempPacket[45] = aaa23[1];
                                    tempPacket[46] = aaa24[0];
                                    tempPacket[47] = aaa24[1];
                                    tempPacket[48] = aaa25[0];
                                    tempPacket[49] = aaa25[1];
                                    tempPacket[50] = aaa26[0];
                                    tempPacket[51] = aaa26[1];
                                    tempPacket[52] = aaa27[0];
                                    tempPacket[53] = aaa27[1];
                                    tempPacket[54] = aaa28[0];
                                    tempPacket[55] = aaa28[1];
                                    tempPacket[56] = aaa29[0];
                                    tempPacket[57] = aaa29[1];
                                    tempPacket[58] = aaa30[0];
                                    tempPacket[59] = aaa30[1];
                                    tempPacket[60] = aaa31[0];
                                    tempPacket[61] = aaa31[1];
                                    tempPacket[62] = aaa32[0];
                                    tempPacket[63] = aaa32[1];
                                    tempPacket[64] = aaa33[0];
                                    tempPacket[65] = aaa33[1];
                                    tempPacket[66] = aaa34[0];
                                    tempPacket[67] = aaa34[1];
                                    tempPacket[68] = aaa35[0];
                                    tempPacket[69] = aaa35[1];
                                    tempPacket[70] = aaa36[0];
                                    tempPacket[71] = aaa36[1];
                                    tempPacket[72] = aaa37[0];
                                    tempPacket[73] = aaa37[1];
                                    tempPacket[74] = aaa38[0];
                                    tempPacket[75] = aaa38[1];
                                    tempPacket[76] = aaa39[0];
                                    tempPacket[77] = aaa39[1];
                                    tempPacket[78] = aaa40[0];
                                    tempPacket[79] = aaa40[1];
                                    tempPacket[80] = aaa41[0];
                                    tempPacket[80] = aaa41[1];
                                    tempPacket[81] = aaa41[1];
                                    tempPacket[82] = aaa42[0];
                                    tempPacket[83] = aaa42[1];
                                    tempPacket[84] = aaa43[0];
                                    tempPacket[85] = aaa43[1];
                                    tempPacket[86] = aaa44[0];
                                    tempPacket[87] = aaa44[1];
                                    tempPacket[88] = aaa45[0];
                                    tempPacket[89] = aaa45[1];
                                    tempPacket[90] = aaa46[0];
                                    tempPacket[91] = aaa46[1];
                                    tempPacket[92] = aaa47[0];
                                    tempPacket[93] = aaa47[1];
                                    tempPacket[94] = aaa48[0];
                                    tempPacket[95] = aaa48[1];

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
