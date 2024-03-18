using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
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

        IntPtr m_hObject;		 //handle for device
        IntPtr[] objects = new IntPtr[] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };

        bool m_bPortOpen;	 //tells the port status of the device
        icsSpyMessage[] stMessages = new icsSpyMessage[20000];   //TempSpace for messages
        byte[] NetworkIDConvert = new byte[15]; // Storage to convert listbox index to Network ID's 
        int iOpenDeviceType; //Storage for the device type that is open
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

        private HashSet<Int32> openedDevicesSerials = new HashSet<Int32>();
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

            //Set the number of devices to find, for this example look for 16.  This example will only work with the first.
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
                //keep track of devices that have already been opened
                if (openedDevicesSerials.Add(ndNeoToOpenex[i].neoDevice.SerialNumber))
                {
                    iResult = icsNeoDll.icsneoOpenDevice(ref ndNeoToOpenex[i], ref objects[i], ref bNetwork[0], 1, 0, ref OptionOpenNeoEX, 0);
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

                    //Convert the Serial number to a String
                    icsNeoDll.icsneoSerialNumberToString(Convert.ToUInt32(ndNeoToOpenex[i].neoDevice.SerialNumber), ref bSN[0], ref StringSize);
                    sConvertedSN = System.Text.ASCIIEncoding.ASCII.GetString(bSN);

                    //create new tab per device
                    TabPage tabPage = new TabPage($"Device {sConvertedSN}");
                    tabPage.Controls.Add(new OpenDeviceTab(ref objects[i]) { Name = $"{sConvertedSN}" });
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

            //close the port
            iResult = icsNeoDll.icsneoClosePort(m_hObject, ref iNumberOfErrors);
            if (iResult == 1)
            {
                MessageBox.Show("Port Closed OK!");
            }
            else
            {
                MessageBox.Show("Problem ClosingPort");
            }
        }

        private void cmdVersion_Click(object sender, EventArgs e)
        {
            //get version information and send to hardware.
            cmdVersion.Text = Convert.ToString(icsNeoDll.icsneoGetDLLVersion());
        }

        private uint GetPacketTypeFromArbId(uint arbId)
        {
            return (arbId >> 16) & 0xFF;
        }

        private uint GetPacketIdFromArbId(uint arbId)
        {
            return (arbId >> 8) & 0xFF;
        }

        private uint GetSourceFromArbId(uint arbId)
        {
            return arbId & 0xFF;
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
                            } else if (data[1] == PMSPACKET)
                            {
                               
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
                            } else if (data[1] == EMSPACKET)
                            {
                                byte[] tempPacket = BitConverter.GetBytes(uc1.managers[0].G1V)
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G2V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G3V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G4V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G5V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G6V))
                                    .Concat(BitConverter.GetBytes(uc1.managers[0].G7V))
                                    .ToArray();

                                server.SendTo(tempPacket, remoteEnd);
                            } else
                            {
                                //request not recongised, send back FF
                                server.SendTo(new byte[] {255,255,255,255,255,255,255,255,255,255,255}, remoteEnd);
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

        
        private void setACL_Click(object sender, EventArgs e)
        {
            //must set mode to standby before setting ACL
            setMode(ADI_WIL_MODE_STANDBY);

            int iResult;
            int iTimeOutCounter = 0;
            byte uAPISelected, uInstanceSelected, uFunctionSelected, uFunctionError, uCallbackError = 1, uCurrentFunction, uFinishedProcessing;
            byte[] pParameters = new byte[512];
            uint uTotalLength;
            uAPISelected = 1;
            uInstanceSelected = 0;
            uFunctionSelected = ADI_WIL_API_SET_ACL;
            byte uNodeCount = 1;
            byte[] pACLArray = { 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00 };
            pParameters[0] = uNodeCount;
            Buffer.BlockCopy(pACLArray, 0, pParameters, 1, pACLArray.Length);
            uTotalLength = (uint)(1 + pACLArray.Length);

            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, uAPISelected, uInstanceSelected, uFunctionSelected, Marshal.UnsafeAddrOfPinnedArrayElement(pParameters, 0), uTotalLength, out uFunctionError);

            if (iResult != 1 && uFunctionError != ADI_WIL_ERR_SUCCESS)
            {
                // Handle Error Here
                MessageBox.Show($"Command error {uFunctionError}");
                return;
            }

            while (iTimeOutCounter < 10)
            {
                iResult = icsNeoDll.icsneoGenericAPIGetStatus(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, out uCallbackError, out uFinishedProcessing);

                if (uCurrentFunction != uFunctionSelected || iResult != 1)
                {
                    // Handle Error Here
                    MessageBox.Show($"Status error {uFunctionError}");
                    return;
                }

                if (uFinishedProcessing == 1)
                {
                    break;
                }

                Thread.Sleep(100);
                iTimeOutCounter++;
            }

            if (uCallbackError != ADI_WIL_ERR_SUCCESS || iTimeOutCounter > 10)
            {
                // Handle Error Here
                MessageBox.Show($"Timeout/callback error {uCallbackError}");
                return;
            }

            MessageBox.Show("Apparently finished succesfully");
        }

        private void getACL_Click(object sender, EventArgs e)
        {
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return;  // do not read messages if we haven't opened neoVI yet
            }

            //get ACL is available in all system modes

            int iResult;

            byte uAPISelected, uInstanceSelected, uFunctionSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[513];
            byte[] pACL = new byte[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES];
            uint uParametersLength, uReturnedDataLength;

            //test value to see if the acl gets changed
            //pReturnedData[0] = 1;

            //MessageBox.Show(BitConverter.ToString(pReturnedData));
            MessageBox.Show(BitConverter.ToString(pACL));

            uAPISelected = 1;
            uInstanceSelected = 0;
            uFunctionSelected = ADI_WIL_API_GET_ACL;

            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, uAPISelected, uInstanceSelected, uFunctionSelected, IntPtr.Zero, 0, out uFunctionError);

            if (iResult != 1 && uFunctionError != ADI_WIL_ERR_SUCCESS)
            {
                // Handle Error Here
                MessageBox.Show($"Send error {iResult} {uFunctionError}");
                return;
            }

            if (!checkStatus(uAPISelected, uInstanceSelected, uFunctionSelected))
            {
                MessageBox.Show("Error");
            }

            iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);

            if (uCurrentFunction != uFunctionSelected || iResult != 1) //1 is success
            {
                // Handle Error Here
                MessageBox.Show($"Read error {iResult}");
                return;
            }

            if (uReturnedDataLength > 0)
            {
                uNodeCount = pReturnedData[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES + 1]; //497
                MessageBox.Show($"node count{uNodeCount}");

                if (ADI_WIL_MAX_NODES >= uNodeCount && uNodeCount >= 0)
                {
                    Buffer.BlockCopy(pReturnedData, 0, pACL, 0, uNodeCount * ADI_WIL_MAC_SIZE);
                }
                MessageBox.Show(BitConverter.ToString(pReturnedData));
                MessageBox.Show(BitConverter.ToString(pACL));
            }
        }

        private bool checkStatus(byte uAPISelected, byte uInstanceSelected, byte uFunctionSelected)
        {
            int iTimeOutCounter = 0;

            byte uCallbackError = 1, uFinishedProcessing, uCurrentFunction;

            while (iTimeOutCounter < 10)
            {
                int iResult = icsNeoDll.icsneoGenericAPIGetStatus(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, out uCallbackError, out uFinishedProcessing);

                if (uCurrentFunction != uFunctionSelected || iResult != 1) //1 is success
                {
                    // Handle Error Here
                    MessageBox.Show($"Error {iResult}");
                    return false;
                }

                if (uFinishedProcessing == 1)
                {
                    break;
                }

                System.Threading.Thread.Sleep(10);
                iTimeOutCounter++;
            }

            if (uCallbackError != ADI_WIL_ERR_SUCCESS || iTimeOutCounter > 10)
            {
                // Handle Error Here
                return false;
            } else
            {
                return true;
            }
        }

        private void setMode(byte mode)
        {
            //set mode to active
            //adi_wil_SetMode(m_hObject, 3);
            byte functionError = 0;
            byte function = ADI_WIL_API_SET_MODE; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[512];

            parameters[0] = mode; //3 = active mode

            icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 1, out functionError);

            if (!checkStatus(1, 0, function))
            {
                MessageBox.Show("Error");
            }
        }
    }
}
