using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using LiuQF.Common;

namespace CSnet
{
    public partial class OpenDeviceTab : UserControl
    {

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

        public const ulong ADI_WIL_DEV_NODE_0 = 0x0000000000000001u;     
        public const ulong ADI_WIL_DEV_NODE_1 = 0x0000000000000002u;     
        public const ulong ADI_WIL_DEV_NODE_2 = 0x0000000000000004u;     
        public const ulong ADI_WIL_DEV_NODE_3 = 0x0000000000000008u;     
        public const ulong ADI_WIL_DEV_NODE_4 = 0x0000000000000010u;     
        public const ulong ADI_WIL_DEV_NODE_5 = 0x0000000000000020u;     
        public const ulong ADI_WIL_DEV_NODE_6 = 0x0000000000000040u;     
        public const ulong ADI_WIL_DEV_NODE_7 = 0x0000000000000080u;     
        public const ulong ADI_WIL_DEV_NODE_8 = 0x0000000000000100u;     
        public const ulong ADI_WIL_DEV_NODE_9 = 0x0000000000000200u;     
        public const ulong ADI_WIL_DEV_NODE_10 = 0x0000000000000400u;    
        public const ulong ADI_WIL_DEV_NODE_11 = 0x0000000000000800u;    
        public const ulong ADI_WIL_DEV_NODE_12 = 0x0000000000001000u;    
        public const ulong ADI_WIL_DEV_NODE_13 = 0x0000000000002000u;    
        public const ulong ADI_WIL_DEV_NODE_14 = 0x0000000000004000u;    
        public const ulong ADI_WIL_DEV_NODE_15 = 0x0000000000008000u;    
        public const ulong ADI_WIL_DEV_NODE_16 = 0x0000000000010000u;    
        public const ulong ADI_WIL_DEV_NODE_17 = 0x0000000000020000u;    
        public const ulong ADI_WIL_DEV_NODE_18 = 0x0000000000040000u;    
        public const ulong ADI_WIL_DEV_NODE_19 = 0x0000000000080000u;    
        public const ulong ADI_WIL_DEV_NODE_20 = 0x0000000000100000u;    
        public const ulong ADI_WIL_DEV_NODE_21 = 0x0000000000200000u;    
        public const ulong ADI_WIL_DEV_NODE_22 = 0x0000000000400000u;    
        public const ulong ADI_WIL_DEV_NODE_23 = 0x0000000000800000u;    
        public const ulong ADI_WIL_DEV_NODE_24 = 0x0000000001000000u;    
        public const ulong ADI_WIL_DEV_NODE_25 = 0x0000000002000000u;    
        public const ulong ADI_WIL_DEV_NODE_26 = 0x0000000004000000u;    
        public const ulong ADI_WIL_DEV_NODE_27 = 0x0000000008000000u;    
        public const ulong ADI_WIL_DEV_NODE_28 = 0x0000000010000000u;    
        public const ulong ADI_WIL_DEV_NODE_29 = 0x0000000020000000u;    
        public const ulong ADI_WIL_DEV_NODE_30 = 0x0000000040000000u;    
        public const ulong ADI_WIL_DEV_NODE_31 = 0x0000000080000000u;    
        public const ulong ADI_WIL_DEV_NODE_32 = 0x0000000100000000u;    
        public const ulong ADI_WIL_DEV_NODE_33 = 0x0000000200000000u;    
        public const ulong ADI_WIL_DEV_NODE_34 = 0x0000000400000000u;    
        public const ulong ADI_WIL_DEV_NODE_35 = 0x0000000800000000u;    
        public const ulong ADI_WIL_DEV_NODE_36 = 0x0000001000000000u;    
        public const ulong ADI_WIL_DEV_NODE_37 = 0x0000002000000000u;    
        public const ulong ADI_WIL_DEV_NODE_38 = 0x0000004000000000u;    
        public const ulong ADI_WIL_DEV_NODE_39 = 0x0000008000000000u;    
        public const ulong ADI_WIL_DEV_NODE_40 = 0x0000010000000000u;    
        public const ulong ADI_WIL_DEV_NODE_41 = 0x0000020000000000u;    
        public const ulong ADI_WIL_DEV_NODE_42 = 0x0000040000000000u;    
        public const ulong ADI_WIL_DEV_NODE_43 = 0x0000080000000000u;    
        public const ulong ADI_WIL_DEV_NODE_44 = 0x0000100000000000u;    
        public const ulong ADI_WIL_DEV_NODE_45 = 0x0000200000000000u;    
        public const ulong ADI_WIL_DEV_NODE_46 = 0x0000400000000000u;    
        public const ulong ADI_WIL_DEV_NODE_47 = 0x0000800000000000u;    
        public const ulong ADI_WIL_DEV_NODE_48 = 0x0001000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_49 = 0x0002000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_50 = 0x0004000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_51 = 0x0008000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_52 = 0x0010000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_53 = 0x0020000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_54 = 0x0040000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_55 = 0x0080000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_56 = 0x0100000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_57 = 0x0200000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_58 = 0x0400000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_59 = 0x0800000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_60 = 0x1000000000000000u;    
        public const ulong ADI_WIL_DEV_NODE_61 = 0x2000000000000000u;

        public const ulong ADI_WIL_DEV_ALL_NODES = 0x3FFFFFFFFFFFFFFFu;
        
        public const ulong ADI_WIL_DEV_MANAGER_0 = 0x4000000000000000UL;
        public const ulong ADI_WIL_DEV_MANAGER_1 = 0x8000000000000000UL;
        public const ulong ADI_WIL_DEV_ALL_MANAGERS = 0xC000000000000000u;

        public bool chkHexFormat = false;

        private byte[] sListStringV1 = new byte[128]; //Node1 00数据
        private byte[] sListStringV2 = new byte[128]; //Node2 00数据

        public IntPtr m_hObject;		 //handle for device
        bool m_bPortOpen;	 //tells the port status of the device
        icsSpyMessage[] stMessages = new icsSpyMessage[20000];   //TempSpace for messages
        private byte currentMode = ADI_WIL_MODE_STANDBY;

        public ModuleData[] modules;
        public ManagerData[] managers = new ManagerData[2];

        private string sListString = "";//转中位机

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

        public OpenDeviceTab(ref IntPtr obj)
        {
            m_hObject = obj;
            m_bPortOpen = true;

            managers[0] = new ManagerData();
            managers[1] = new ManagerData();

            managers[0].Version = DeviceFirmwareVersion(62);
            managers[1].Version = DeviceFirmwareVersion(63); //this should throw an error if the pack is BEV & only has one manager

            getAclButton_Click(null, null);
            modules = new ModuleData[nodeMacAddresses.Count];

            for (int i = 0; i < nodeMacAddresses.Count; i++)
            {
                modules[i] = new ModuleData();
                modules[i].MacAddress = nodeMacAddresses[i];
                
            }

            InitializeComponent();

            //find firmware versions            
            Debug.WriteLine($"module count: {nodeMacAddresses.Count}");
        }

        /*
        private void test_wbms_settings()
        {
            CU_ASSERT_PTR_NOT_NULL_FATAL(hObject); // confirm hObject is not null

            icsNeoDll.SDeviceSettings mysettings = new SDeviceSettings();
            Array.Clear(mysettings.Settings.rad_bms.spi_config.port_a.config.padding, 0, mysettings.Settings.rad_bms.spi_config.port_a.config.padding.Length);
            Array.Clear(mysettings.Settings.rad_bms.spi_config.port_b.config.padding, 0, mysettings.Settings.rad_bms.spi_config.port_b.config.padding.Length);

            icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref mysettings, Marshal.SizeOf(typeof(SDeviceSettings)), PlasmaIonVnetChannelMain);  //read settings struct

            mysettings.Settings.rad_bms.spi_config.port_a.config.onboard_external = SPI_PORT_ONBOARD;  //force to use internal spi
            mysettings.Settings.rad_bms.spi_config.port_a.config.type = SPI_TYPE_WIL;
            mysettings.Settings.rad_bms.spi_config.port_a.config.mode = SPI_MODE_MASTER;

            mysettings.Settings.rad_bms.spi_config.port_b.config.onboard_external = SPI_PORT_ONBOARD;  //force to use internal spi
            mysettings.Settings.rad_bms.spi_config.port_b.config.type = SPI_TYPE_WIL;
            mysettings.Settings.rad_bms.spi_config.port_b.config.mode = SPI_MODE_MASTER;

            icsNeoDll.icsneoSetDeviceSettings(hObject, ref mysettings, Marshal.SizeOf(typeof(SDeviceSettings)), 1, PlasmaIonVnetChannelMain);  //write settings struct

            //Console.WriteLine("");
        }
        */

        private void OTAP(Stream fileStream)
        {
            setMode(ADI_WIL_MODE_STANDBY);
            /*The user is able to switch between what type of file they want to flash and WIL version
    
                opfw1x -  we need to have WIL 1.x flashed onto the RAD-wBMS and makes use the network is in standby

                fpa2x -  we need to have WIL 2.x flashed onto the RAD-wBMS and makes use the network is in standby

                opfw2x -  like fpa2x but the network needs to be in OTAP in order to flash operational firmware. Managers will trigger a reset after this so we must wait 
                            for that manager to reconnect before we continue through the application. Same goes for nodes.
            */

            byte[] buffer = new byte[64];
            int offset = 0;
                long fileSize = fileStream.Length;
            int count = -1;
            Debug.WriteLine($"filesize: {fileSize}");
            while (count != 0){ //not guaranteed to be 64 bytes long
                count = fileStream.Read(buffer, offset, 64);
                //what happens when we hit the end of the file?
                Debug.WriteLine($"reading: {count}");
                offset = SendOTAPFileChunk(offset, buffer);
                
                if (offset >= fileSize)
                {
                    //done
                    break;
                } else if (offset == -1)
                {
                    //error
                    Debug.WriteLine("OTAP problem high level");
                    break;
                }
            }
        }

        private bool OTAPCheckStatus(byte uAPISelected, byte uInstanceSelected, byte uFunctionSelected)
        {
            int timeout = 0, result = 0;

            byte uCallbackError, uFinishedProcessing, uCurrentFunction;

            do
            {
                Thread.Sleep(10);
                result = icsNeoDll.icsneoGenericAPIGetStatus(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, out uCallbackError, out uFinishedProcessing);
                if (timeout == 1000)
                {
                    Debug.WriteLine("OTAP timeout");
                    return false;                    
                }
                timeout++;
                //only breaks if command has been successfully sent, or timesout
            } while (uFinishedProcessing == 0);

            if (uCallbackError == ADI_WIL_ERR_IN_PROGRESS) //uFinishedProcessing == 1 && 
            {
                //still writing file
                return true;
            } else if (uCallbackError == ADI_WIL_ERR_SUCCESS)
            {
                //written entire file
                return true;
            } else
            {
                Debug.WriteLine($"OTAP bad response from get status, Callback: {uCallbackError} finishedProcessing: {uFinishedProcessing} result: {result}");
                return false;
            }
        }

        private int SendOTAPFileChunk(int offset, byte[] buffer)
        {
            int iResult;

            byte[] parameters = new byte[67];
            byte[] pReturnedData = new byte[4];

            byte uAPISelected = 1, uInstanceSelected = 0, uFunctionSelected = ADI_WIL_API_LOAD_FILE, uFunctionError, uCurrentFunction;

            //typedef uint64_t adi_wil_device_t;
            //4 bytes enum for file type
            parameters[0] = 254;//ADI_WIL_DEV_MANAGER_0 = 240, ADI_WIL_DEV_MANAGER_1 = 241, ADI_WIL_DEV_ALL_MANAGERS= 254, ADI_WIL_DEV_ALL_NODES = 255
            parameters[1] = 0;//file type, ADI_WIL_FILE_TYPE_FIRMWARE = 0
            parameters[2] = 64;//file size, always send in 64 byte chunks
            Buffer.BlockCopy(buffer, offset, parameters, 3, 64); //copy chunk of file into parameters at position 3

            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, uAPISelected, uInstanceSelected, uFunctionSelected, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), (uint)parameters.Length, out uFunctionError);

            if (iResult != 1 && uFunctionError != ADI_WIL_ERR_SUCCESS)
            {
                // Handle Error Here
                MessageBox.Show($"Send error {iResult} {uFunctionError}");
                return -1;
            }

            if(OTAPCheckStatus(uAPISelected, uInstanceSelected, uFunctionSelected))
            {
                //finished or in progress, either way read the latest offset
                uint uReturnedDataLength;

                iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);

                if (uCurrentFunction != uFunctionSelected || iResult != 1) //1 is success
                {
                    // Handle Error Here
                    MessageBox.Show($"Read error {iResult}");
                    return -1;
                }

                //return offset;
                Debug.WriteLine($"Returned file position: {BitConverter.ToInt32(pReturnedData, 0)}");
                return BitConverter.ToInt32(pReturnedData, 0);
            } else
            {
                MessageBox.Show("OTAP status error");
                return -1;
            }
        }

        private void PMSGPIO(ulong manager, byte highLow)
        {
            byte functionError = 0;
            byte function = ADI_WIL_API_SET_GPIO; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[512];


            //little endian?
            BitConverter.GetBytes(manager).CopyTo(parameters, 0);
            //parameters[0] = ADI_WIL_DEV_MANAGER_0; //target first manager //64 or 65
            parameters[8] = 4; // pin 9
            parameters[9] = highLow; // set to high

            int iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 10, out functionError);

            if (!checkStatus(1, 0, function) || iResult != 1)
            {
                MessageBox.Show("GPIO error");
            }

            /*
            //read GPIO to check it was done successfully 
            function = ADI_WIL_API_GET_GPIO;

            BitConverter.GetBytes(ADI_WIL_DEV_MANAGER_0).CopyTo(parameters, 0);
            //parameters[0] = ADI_WIL_DEV_MANAGER_0; //target first manager //64 or 65
            parameters[8] = 4; // pin 9
                               //parameters[9] = 1; // set to high


            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 9, out functionError);

            if (!checkStatus(1, 0, function) || iResult != 1)
            {
                MessageBox.Show("GPIO error");
            }

            byte uAPISelected, uInstanceSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[512];
            pReturnedData[0] = 4;

            uint uParametersLength, uReturnedDataLength;
            //adi_wil_dev_version_t version = new adi_wil_dev_version_t();

            uAPISelected = 1;
            uInstanceSelected = 0;

            iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);

            if (uCurrentFunction != function || iResult != 1) //1 is success
            {
                // Handle Error Here
                MessageBox.Show($"Read error {iResult}");
                return;
            }

            
            Debug.WriteLine($"GPIO pin output: {BitConverter.ToUInt64(pReturnedData, 0)}");

            
                The GPIO pin on the target device is set to be an output pin before its value is set. 
                Note that the GPIO pin is set to output mode only, therefore it cannot be set to a value 
                then re-read back by using adi_wil_GetGPIO since adi_wil_GetGPIO will reconfigure the pin 
                to input mode in order to be read 
             */

        }

        private int DeviceFirmwareVersion(int deviceID)
        {
            setMode(ADI_WIL_MODE_STANDBY);

            //Get Device version
            byte functionError = 0;
            byte function = ADI_WIL_API_GET_DEVICE_VERSION; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[8];


            //node id's are converted to powers of 2, for some reason
            BitConverter.GetBytes(Convert.ToUInt64(Math.Pow(2, deviceID))).CopyTo(parameters, 0);

            int iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), (uint)parameters.Length, out functionError);

            if (!checkStatus(1, 0, function) || iResult != 1)
            {
                MessageBox.Show("Find version error");
                return 0;
            }

            byte uAPISelected, uInstanceSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[512];

            uint uParametersLength, uReturnedDataLength;
            //adi_wil_dev_version_t version = new adi_wil_dev_version_t();

            uAPISelected = 1;
            uInstanceSelected = 0;

            iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);
            adi_wil_dev_version_t version = (adi_wil_dev_version_t)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), typeof(adi_wil_dev_version_t));

            if (uCurrentFunction != function || iResult != 1) //1 is success
            {
                // Handle Error Here
                MessageBox.Show($"Read error {iResult}");
                return 0;
            }
            string temp = "";
            parameters.ToList().ForEach(x => temp += x);
            Debug.WriteLine($"node {deviceID} {temp} version: {version.MainProcSWVersion.iVersionMajor}.{version.MainProcSWVersion.iVersionMinor}.{version.MainProcSWVersion.iVersionPatch}");

            return version.MainProcSWVersion.iVersionMajor;
        }

        private void WilFirmwareVersion()
        {
            setMode(ADI_WIL_MODE_STANDBY);

            //Get WIL version
            byte functionError = 0;
            byte function = ADI_WIL_API_GET_WIL_SOFTWARE_VERSION; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[512];
            int iResult;

            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 0, out functionError);

            if (!checkStatus(1, 0, function) || iResult != 1)
            {
                Debug.WriteLine($"Find version check status error");
                MessageBox.Show("Find version error");
            }


            byte uAPISelected, uInstanceSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[512];


            uint uParametersLength, uReturnedDataLength;
            //adi_wil_dev_version_t version = new adi_wil_dev_version_t();

            uAPISelected = 1;
            uInstanceSelected = 0;

            iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);
            adi_wil_dev_version_t version = (adi_wil_dev_version_t)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), typeof(adi_wil_dev_version_t));


            if (uCurrentFunction != function || iResult != 1) //1 is success
            {
                // Handle Error Here
                MessageBox.Show($"Read error {iResult}");
                return;
            }

            Debug.WriteLine($"version object {version}");

            firmwareLabel.Text = $"Firmware version: {version.MainProcSWVersion.iVersionMajor}";
        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            CmdReceive_Click(cmdReceive, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Get ACL flash
            string Addr1 = MyINI.GetIniKeyValueForStr("macAddress", "1", Application.StartupPath + "\\config.ini");
            string Addr2 = MyINI.GetIniKeyValueForStr("macAddress", "2", Application.StartupPath + "\\config.ini");
            string Addr3 = MyINI.GetIniKeyValueForStr("macAddress", "3", Application.StartupPath + "\\config.ini");
            string Addr4 = MyINI.GetIniKeyValueForStr("macAddress", "4", Application.StartupPath + "\\config.ini");
            string Addr5 = MyINI.GetIniKeyValueForStr("macAddress", "5", Application.StartupPath + "\\config.ini");
            string Addr6 = MyINI.GetIniKeyValueForStr("macAddress", "6", Application.StartupPath + "\\config.ini");
            string Addr7 = MyINI.GetIniKeyValueForStr("macAddress", "7", Application.StartupPath + "\\config.ini");
            string Addr8 = MyINI.GetIniKeyValueForStr("macAddress", "8", Application.StartupPath + "\\config.ini");
            string Addr9 = MyINI.GetIniKeyValueForStr("macAddress", "9", Application.StartupPath + "\\config.ini");
            string Addr10 = MyINI.GetIniKeyValueForStr("macAddress", "10", Application.StartupPath + "\\config.ini");
            string Addr11 = MyINI.GetIniKeyValueForStr("macAddress", "11", Application.StartupPath + "\\config.ini");
            string Addr12 = MyINI.GetIniKeyValueForStr("macAddress", "12", Application.StartupPath + "\\config.ini");
            string Addr13 = MyINI.GetIniKeyValueForStr("macAddress", "13", Application.StartupPath + "\\config.ini");
            string Addr14 = MyINI.GetIniKeyValueForStr("macAddress", "14", Application.StartupPath + "\\config.ini");
            string Addr15 = MyINI.GetIniKeyValueForStr("macAddress", "15", Application.StartupPath + "\\config.ini");
            string Addr16 = MyINI.GetIniKeyValueForStr("macAddress", "16", Application.StartupPath + "\\config.ini");
            int iResult;
            int iTimeOutCounter = 0;
            byte uAPISelected, uInstanceSelected, uFunctionSelected, uFunctionError, uCallbackError = 1, uCurrentFunction, uFinishedProcessing;
            byte[] pParameters = new byte[512];
            uint uTotalLength;
            uAPISelected = 1;
            uInstanceSelected = 0;
            uFunctionSelected = ADI_WIL_API_SET_ACL;
            byte uNodeCount = 16;

            //set 
            UInt64[] address1_1 = strToToHexByte(Addr1.Substring(0, 2));
            UInt64[] address1_2 = strToToHexByte(Addr1.Substring(2, 2));
            UInt64[] address1_3 = strToToHexByte(Addr1.Substring(4, 2));

            UInt64[] address2_1 = strToToHexByte(Addr2.Substring(0, 2));
            UInt64[] address2_2 = strToToHexByte(Addr2.Substring(2, 2));
            UInt64[] address2_3 = strToToHexByte(Addr2.Substring(4, 2));

            UInt64[] address3_1 = strToToHexByte(Addr3.Substring(0, 2));
            UInt64[] address3_2 = strToToHexByte(Addr3.Substring(2, 2));
            UInt64[] address3_3 = strToToHexByte(Addr3.Substring(4, 2));

            UInt64[] address4_1 = strToToHexByte(Addr4.Substring(0, 2));
            UInt64[] address4_2 = strToToHexByte(Addr4.Substring(2, 2));
            UInt64[] address4_3 = strToToHexByte(Addr4.Substring(4, 2));

            UInt64[] address5_1 = strToToHexByte(Addr5.Substring(0, 2));
            UInt64[] address5_2 = strToToHexByte(Addr5.Substring(2, 2));
            UInt64[] address5_3 = strToToHexByte(Addr5.Substring(4, 2));

            UInt64[] address6_1 = strToToHexByte(Addr6.Substring(0, 2));
            UInt64[] address6_2 = strToToHexByte(Addr6.Substring(2, 2));
            UInt64[] address6_3 = strToToHexByte(Addr6.Substring(4, 2));

            UInt64[] address7_1 = strToToHexByte(Addr7.Substring(0, 2));
            UInt64[] address7_2 = strToToHexByte(Addr7.Substring(2, 2));
            UInt64[] address7_3 = strToToHexByte(Addr7.Substring(4, 2));

            UInt64[] address8_1 = strToToHexByte(Addr8.Substring(0, 2));
            UInt64[] address8_2 = strToToHexByte(Addr8.Substring(2, 2));
            UInt64[] address8_3 = strToToHexByte(Addr8.Substring(4, 2));

            UInt64[] address9_1 = strToToHexByte(Addr9.Substring(0, 2));
            UInt64[] address9_2 = strToToHexByte(Addr9.Substring(2, 2));
            UInt64[] address9_3 = strToToHexByte(Addr9.Substring(4, 2));

            UInt64[] address10_1 = strToToHexByte(Addr10.Substring(0, 2));
            UInt64[] address10_2 = strToToHexByte(Addr10.Substring(2, 2));
            UInt64[] address10_3 = strToToHexByte(Addr10.Substring(4, 2));

            UInt64[] address11_1 = strToToHexByte(Addr11.Substring(0, 2));
            UInt64[] address11_2 = strToToHexByte(Addr11.Substring(2, 2));
            UInt64[] address11_3 = strToToHexByte(Addr11.Substring(4, 2));

            UInt64[] address12_1 = strToToHexByte(Addr12.Substring(0, 2));
            UInt64[] address12_2 = strToToHexByte(Addr12.Substring(2, 2));
            UInt64[] address12_3 = strToToHexByte(Addr12.Substring(4, 2));

            UInt64[] address13_1 = strToToHexByte(Addr13.Substring(0, 2));
            UInt64[] address13_2 = strToToHexByte(Addr13.Substring(2, 2));
            UInt64[] address13_3 = strToToHexByte(Addr13.Substring(4, 2));

            UInt64[] address14_1 = strToToHexByte(Addr14.Substring(0, 2));
            UInt64[] address14_2 = strToToHexByte(Addr14.Substring(2, 2));
            UInt64[] address14_3 = strToToHexByte(Addr14.Substring(4, 2));

            UInt64[] address15_1 = strToToHexByte(Addr15.Substring(0, 2));
            UInt64[] address15_2 = strToToHexByte(Addr15.Substring(2, 2));
            UInt64[] address15_3 = strToToHexByte(Addr15.Substring(4, 2));

            UInt64[] address16_1 = strToToHexByte(Addr16.Substring(0, 2));
            UInt64[] address16_2 = strToToHexByte(Addr16.Substring(2, 2));
            UInt64[] address16_3 = strToToHexByte(Addr16.Substring(4, 2));

            //set int[] to string
            string strAddr1_1 = "";
            string strAddr1_2 = "";
            string strAddr1_3 = "";

            for (int i = 0; i < address1_1.Length; i++)
            {
                strAddr1_1 += address1_1[i].ToString();
            }
            byte A1_1 = byte.Parse(strAddr1_1);

            for (int i = 0; i < address1_2.Length; i++)
            {
                strAddr1_2 += address1_2[i].ToString();
            }
            byte A1_2 = byte.Parse(strAddr1_2);

            for (int i = 0; i < address1_3.Length; i++)
            {
                strAddr1_3 += address1_3[i].ToString();
            }
            byte A1_3 = byte.Parse(strAddr1_3);

            string strAddr2_1 = "";
            string strAddr2_2 = "";
            string strAddr2_3 = "";

            for (int i = 0; i < address2_1.Length; i++)
            {
                strAddr2_1 += address2_1[i].ToString();
            }
            byte A2_1 = byte.Parse(strAddr2_1);

            for (int i = 0; i < address2_2.Length; i++)
            {
                strAddr2_2 += address2_2[i].ToString();
            }
            byte A2_2 = byte.Parse(strAddr2_2);

            for (int i = 0; i < address2_3.Length; i++)
            {
                strAddr2_3 += address2_3[i].ToString();
            }
            byte A2_3 = byte.Parse(strAddr2_3);

            string strAddr3_1 = "";
            string strAddr3_2 = "";
            string strAddr3_3 = "";

            for (int i = 0; i < address3_1.Length; i++)
            {
                strAddr3_1 += address3_1[i].ToString();
            }
            byte A3_1 = byte.Parse(strAddr3_1);

            for (int i = 0; i < address3_2.Length; i++)
            {
                strAddr3_2 += address3_2[i].ToString();
            }
            byte A3_2 = byte.Parse(strAddr3_2);

            for (int i = 0; i < address3_3.Length; i++)
            {
                strAddr3_3 += address3_3[i].ToString();
            }
            byte A3_3 = byte.Parse(strAddr3_3);

            string strAddr4_1 = "";
            string strAddr4_2 = "";
            string strAddr4_3 = "";

            for (int i = 0; i < address4_1.Length; i++)
            {
                strAddr4_1 += address4_1[i].ToString();
            }
            byte A4_1 = byte.Parse(strAddr4_1);

            for (int i = 0; i < address4_2.Length; i++)
            {
                strAddr4_2 += address4_2[i].ToString();
            }
            byte A4_2 = byte.Parse(strAddr4_2);

            for (int i = 0; i < address4_3.Length; i++)
            {
                strAddr4_3 += address4_3[i].ToString();
            }
            byte A4_3 = byte.Parse(strAddr4_3);

            string strAddr5_1 = "";
            string strAddr5_2 = "";
            string strAddr5_3 = "";

            for (int i = 0; i < address5_1.Length; i++)
            {
                strAddr5_1 += address5_1[i].ToString();
            }
            byte A5_1 = byte.Parse(strAddr5_1);

            for (int i = 0; i < address5_2.Length; i++)
            {
                strAddr5_2 += address5_2[i].ToString();
            }
            byte A5_2 = byte.Parse(strAddr5_2);

            for (int i = 0; i < address5_3.Length; i++)
            {
                strAddr5_3 += address5_3[i].ToString();
            }
            byte A5_3 = byte.Parse(strAddr5_3);


            string strAddr6_1 = "";
            string strAddr6_2 = "";
            string strAddr6_3 = "";

            for (int i = 0; i < address6_1.Length; i++)
            {
                strAddr6_1 += address6_1[i].ToString();
            }
            byte A6_1 = byte.Parse(strAddr6_1);

            for (int i = 0; i < address6_2.Length; i++)
            {
                strAddr6_2 += address6_2[i].ToString();
            }
            byte A6_2 = byte.Parse(strAddr6_2);

            for (int i = 0; i < address6_3.Length; i++)
            {
                strAddr6_3 += address6_3[i].ToString();
            }
            byte A6_3 = byte.Parse(strAddr6_3);

            string strAddr7_1 = "";
            string strAddr7_2 = "";
            string strAddr7_3 = "";

            for (int i = 0; i < address7_1.Length; i++)
            {
                strAddr7_1 += address7_1[i].ToString();
            }
            byte A7_1 = byte.Parse(strAddr7_1);

            for (int i = 0; i < address7_2.Length; i++)
            {
                strAddr7_2 += address7_2[i].ToString();
            }
            byte A7_2 = byte.Parse(strAddr7_2);

            for (int i = 0; i < address7_3.Length; i++)
            {
                strAddr7_3 += address7_3[i].ToString();
            }
            byte A7_3 = byte.Parse(strAddr7_3);

            string strAddr8_1 = "";
            string strAddr8_2 = "";
            string strAddr8_3 = "";

            for (int i = 0; i < address8_1.Length; i++)
            {
                strAddr8_1 += address8_1[i].ToString();
            }
            byte A8_1 = byte.Parse(strAddr8_1);

            for (int i = 0; i < address8_2.Length; i++)
            {
                strAddr8_2 += address8_2[i].ToString();
            }
            byte A8_2 = byte.Parse(strAddr8_2);

            for (int i = 0; i < address8_3.Length; i++)
            {
                strAddr8_3 += address8_3[i].ToString();
            }
            byte A8_3 = byte.Parse(strAddr8_3);

            string strAddr9_1 = "";
            string strAddr9_2 = "";
            string strAddr9_3 = "";

            for (int i = 0; i < address9_1.Length; i++)
            {
                strAddr9_1 += address9_1[i].ToString();
            }
            byte A9_1 = byte.Parse(strAddr9_1);

            for (int i = 0; i < address9_2.Length; i++)
            {
                strAddr9_2 += address9_2[i].ToString();
            }
            byte A9_2 = byte.Parse(strAddr9_2);

            for (int i = 0; i < address9_3.Length; i++)
            {
                strAddr9_3 += address9_3[i].ToString();
            }
            byte A9_3 = byte.Parse(strAddr9_3);

            string strAddr10_1 = "";
            string strAddr10_2 = "";
            string strAddr10_3 = "";

            for (int i = 0; i < address10_1.Length; i++)
            {
                strAddr10_1 += address10_1[i].ToString();
            }
            byte A10_1 = byte.Parse(strAddr10_1);

            for (int i = 0; i < address10_2.Length; i++)
            {
                strAddr10_2 += address10_2[i].ToString();
            }
            byte A10_2 = byte.Parse(strAddr10_2);

            for (int i = 0; i < address10_3.Length; i++)
            {
                strAddr10_3 += address10_3[i].ToString();
            }
            byte A10_3 = byte.Parse(strAddr10_3);


            string strAddr11_1 = "";
            string strAddr11_2 = "";
            string strAddr11_3 = "";

            for (int i = 0; i < address11_1.Length; i++)
            {
                strAddr11_1 += address11_1[i].ToString();
            }
            byte A11_1 = byte.Parse(strAddr11_1);

            for (int i = 0; i < address11_2.Length; i++)
            {
                strAddr11_2 += address11_2[i].ToString();
            }
            byte A11_2 = byte.Parse(strAddr11_2);

            for (int i = 0; i < address11_3.Length; i++)
            {
                strAddr11_3 += address11_3[i].ToString();
            }
            byte A11_3 = byte.Parse(strAddr1_3);

            string strAddr12_1 = "";
            string strAddr12_2 = "";
            string strAddr12_3 = "";

            for (int i = 0; i < address12_1.Length; i++)
            {
                strAddr12_1 += address12_1[i].ToString();
            }
            byte A12_1 = byte.Parse(strAddr12_1);

            for (int i = 0; i < address12_2.Length; i++)
            {
                strAddr12_2 += address12_2[i].ToString();
            }
            byte A12_2 = byte.Parse(strAddr12_2);

            for (int i = 0; i < address12_3.Length; i++)
            {
                strAddr12_3 += address12_3[i].ToString();
            }
            byte A12_3 = byte.Parse(strAddr12_3);

            string strAddr13_1 = "";
            string strAddr13_2 = "";
            string strAddr13_3 = "";

            for (int i = 0; i < address13_1.Length; i++)
            {
                strAddr13_1 += address13_1[i].ToString();
            }
            byte A13_1 = byte.Parse(strAddr13_1);

            for (int i = 0; i < address13_2.Length; i++)
            {
                strAddr13_2 += address13_2[i].ToString();
            }
            byte A13_2 = byte.Parse(strAddr13_2);

            for (int i = 0; i < address13_3.Length; i++)
            {
                strAddr13_3 += address13_3[i].ToString();
            }
            byte A13_3 = byte.Parse(strAddr13_3);

            string strAddr14_1 = "";
            string strAddr14_2 = "";
            string strAddr14_3 = "";

            for (int i = 0; i < address14_1.Length; i++)
            {
                strAddr14_1 += address14_1[i].ToString();
            }
            byte A14_1 = byte.Parse(strAddr14_1);

            for (int i = 0; i < address14_2.Length; i++)
            {
                strAddr14_2 += address14_2[i].ToString();
            }
            byte A14_2 = byte.Parse(strAddr14_2);

            for (int i = 0; i < address14_3.Length; i++)
            {
                strAddr14_3 += address14_3[i].ToString();
            }
            byte A14_3 = byte.Parse(strAddr14_3);

            string strAddr15_1 = "";
            string strAddr15_2 = "";
            string strAddr15_3 = "";

            for (int i = 0; i < address15_1.Length; i++)
            {
                strAddr15_1 += address15_1[i].ToString();
            }
            byte A15_1 = byte.Parse(strAddr15_1);

            for (int i = 0; i < address15_2.Length; i++)
            {
                strAddr15_2 += address15_2[i].ToString();
            }
            byte A15_2 = byte.Parse(strAddr15_2);

            for (int i = 0; i < address15_3.Length; i++)
            {
                strAddr15_3 += address15_3[i].ToString();
            }
            byte A15_3 = byte.Parse(strAddr15_3);


            string strAddr16_1 = "";
            string strAddr16_2 = "";
            string strAddr16_3 = "";

            for (int i = 0; i < address16_1.Length; i++)
            {
                strAddr16_1 += address16_1[i].ToString();
            }
            byte A16_1 = byte.Parse(strAddr16_1);

            for (int i = 0; i < address16_2.Length; i++)
            {
                strAddr16_2 += address16_2[i].ToString();
            }
            byte A16_2 = byte.Parse(strAddr16_2);

            for (int i = 0; i < address16_3.Length; i++)
            {
                strAddr16_3 += address16_3[i].ToString();
            }
            byte A16_3 = byte.Parse(strAddr16_3);

            byte[] pACLArray = { 0x64, 0xF9, 0xC0, 0x00, 0x00, A1_1, A1_2, A1_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A2_1, A2_2, A2_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A3_1, A3_2, A3_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A4_1, A4_2, A4_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A5_1, A5_2, A5_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A6_1, A6_2, A6_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A7_1, A7_2, A7_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A8_1, A8_2, A8_3 ,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A9_1, A9_2, A9_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A10_1, A10_2, A10_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A11_1, A11_2, A11_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A12_1, A12_2, A12_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A13_1, A13_2, A13_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A14_1, A14_2, A14_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A15_1, A15_2, A15_3,
                0x64, 0xF9, 0xC0, 0x00, 0x00, A16_1, A16_2, A16_3};


            //byte[] pACLArray = { 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0D, 0x16, 0xBB, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0A, 0x83, 0x89, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0A, 0xCC, 0xEF, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0E, 0xAD, 0x03, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x12, 0x9E, 0xFE, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0E, 0x99, 0xB1, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0B, 0x36, 0x72, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0A, 0xAC, 0xD4
            // ,0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0E, 0x92, 0x59, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0E, 0xAB, 0x2F, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0C, 0xCE, 0xDF, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0B, 0x91, 0x02, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0C, 0xCE, 0x4D, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0E, 0x99, 0xB1, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0A, 0xBF, 0x8D, 0x64, 0xF9, 0xC0, 0x00, 0x00, 0x0D, 0x29, 0xAB};
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

        private void button3_Click(object sender, EventArgs e)
        {
            //must set mode to standby before setting ACL

            Form2 fw = new Form2();
            fw.WindowState = FormWindowState.Normal;
            fw.StartPosition = FormStartPosition.CenterScreen;
            //fw.MdiParent = this;
            //fw.WindowState.
            //fw.Owner = this;
            fw.ShowDialog();

            setMode(ADI_WIL_MODE_STANDBY);
        }

        private bool checkStatus(byte uAPISelected, byte uInstanceSelected, byte uFunctionSelected)
        {
            int iTimeOutCounter = 0;

            byte uCallbackError = 1, uFinishedProcessing, uCurrentFunction;

            while (iTimeOutCounter < 1000)
            {
                int iResult = icsNeoDll.icsneoGenericAPIGetStatus(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, out uCallbackError, out uFinishedProcessing);

                if (uCurrentFunction != uFunctionSelected || iResult != 1) //1 is success
                {
                    // function mismatch, or command not sent to the device
                    return false;
                }

                if (uFinishedProcessing == 1)
                {
                    break;
                }

                Thread.Sleep(1);
                iTimeOutCounter++;
            }

            if (uCallbackError != ADI_WIL_ERR_SUCCESS)
            {
                // Handle Error Here
                return false;
            }
            else
            {
                return true;
            }
        }

        private void setMode(byte mode)
        {
            if (mode != currentMode)
            {
                //set mode to active
                //adi_wil_SetMode(m_hObject, 3);
                byte functionError = 0;
                byte function = ADI_WIL_API_SET_MODE; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
                byte[] parameters = new byte[512];

                parameters[0] = mode; //3 = active mode

                int result = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 1, out functionError);
                if (result != 1)
                {
                    Debug.WriteLine("Set mode command not sent to device");
                }

                if (!checkStatus(1, 0, function))
                {
                    MessageBox.Show("Set mode error");
                } else
                {
                    currentMode = mode;
                }
            }
        }

        private Dictionary<int, string> nodeMacAddresses = new Dictionary<int, string>();

        private void getAclButton_Click(object sender, EventArgs e)//获取ACL
        {
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return;  // do not read messages if we haven't opened neoVI yet
            }

            //get ACL is available in all system modes

            int iResult;

            byte uAPISelected, uInstanceSelected, uFunctionSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES + 1];

            uint uParametersLength, uReturnedDataLength;

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
                MessageBox.Show("ACL error");
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
                uNodeCount = pReturnedData[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES];

                for (int i = 0; i < uNodeCount; i++)
                {
                    byte[] macAddress = new byte[ADI_WIL_MAC_SIZE];
                    Buffer.BlockCopy(pReturnedData, i * ADI_WIL_MAC_SIZE, macAddress, 0, ADI_WIL_MAC_SIZE);
                    string temp = "";
                    foreach (byte x in macAddress)
                    {
                        temp += $"{x:X2}";
                    }
                    if (!nodeMacAddresses.ContainsKey(i))
                    {
                        nodeMacAddresses.Add(i, temp);

                    }
                }
                //MessageBox.Show(BitConverter.ToString(pReturnedData));

            }

        }

        private void cmdOTAP(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "OTAP files (*.OTAP3)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Read the contents of the file into a stream
                    using (Stream fileStream = openFileDialog.OpenFile())
                    {
                        OTAP(fileStream);
                    }                    
                }
            }

            
        }

        private void CmdReceive_Click(object sender, EventArgs e)
        {
            setMode(ADI_WIL_MODE_ACTIVE);

            long lResult;
            int lNumberOfMessages = 0;
            int lNumberOfErrors = 0;
            long lCount;
            // string sListString;
            //  server.Bind(new IPEndPoint(IPAddress.Parse("192.168.3.100"), 9011));
            // string sListString2 = "";//Node 号
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            long iByteCount;
            int iLongMessageTotalByteCount;
            double dTime = 0;
            byte[] iDataBytes;

            //if (m_bPortOpen == false)
            //{
            //    MessageBox.Show("neoVI not opened");
            //    return;  // do not read messages if we haven't opened neoVI yet
            //}

            // read the messages from the driver
            lResult = icsNeoDll.icsneoGetMessages(m_hObject, ref stMessages[0], ref lNumberOfMessages, ref lNumberOfErrors);

            // was the read successful?
            if (lResult == 1)
            {
                // clear the previous list of messages
                lstMessage.Items.Clear();
                PMSBox.Items.Clear();
                EMSBox.Items.Clear();
                lblReadCount.Text = "Number Read : " + Convert.ToString(lNumberOfMessages);
                lblReadErrors.Text = "Number Errors : " + Convert.ToString(lNumberOfErrors);
                // for each message we read
                for (lCount = 1; lCount <= lNumberOfMessages; lCount++)
                {
                    // Calculate the messages timestamp
                    lResult = icsNeoDll.icsneoGetTimeStampForMsg(m_hObject, ref stMessages[lCount - 1], ref dTime);

                    sListString = "Time : " + Convert.ToString(dTime);  //Build String
                    //sListString1 = "Time : " + Convert.ToString(dTime);  //Build String
                    //TODO: unpack raw data into correct format

                    // Was it a tx or rx message
                    if ((stMessages[lCount - 1].StatusBitField & Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_TX_MSG)) > 0)
                    {
                        sListString = sListString + " Tx ";
                    }
                    else
                    {
                        sListString = sListString + " Rx ";
                    }

                    //Add the network name to the string

                    sListString = sListString + "Network " + icsNeoDll.GetStringForNetworkID(Convert.ToInt16((stMessages[lCount - 1].NetworkID2 << 8) + stMessages[lCount - 1].NetworkID));

                    //Decode based on the protocol
                    switch (stMessages[lCount - 1].Protocol)
                    {
                        case (int)ePROTOCOL.SPY_PROTOCOL_WBMS:
                            //this is the only protocol we care about

                            //translate data from raw form, to data we can use
                            int iNetId = (stMessages[lCount - 1].NetworkID2 << 8) | stMessages[lCount - 1].NetworkID;

                            if (iNetId == 532)
                            {
                                sListString += "wBMS 01 ";
                            }
                            else
                            {
                                //This is not a wBMS message
                                sListString += "Unknown ";
                            }
                            // Grab wBMS specific fields from the ArbIDOrHeader value
                            uint uiPacketType = GetPacketTypeFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);
                            uint uiPacketID = GetPacketIdFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);
                            uint uiDeviceSource = GetSourceFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);

                            //Debug.WriteLine($" {uiPacketType} {uiPacketID} {uiDeviceSource} ");

                            if (0 <= uiDeviceSource && uiDeviceSource < MAX_NODES)
                            {
                                sListString += $"Node {uiDeviceSource} ";

                                /*
                                //16台设备00数据收集
                                if (uiPacketID == 0)
                                {
                                    modules[uiDeviceSource].Packet0[0] = (byte)uiDeviceSource;
                                }
                                else
                                {
                                    modules[uiDeviceSource].Packet1[0] = (byte)uiDeviceSource;
                                }
                                */
                            }
                            else if (uiDeviceSource == MANAGER_0_ID || uiDeviceSource == MANAGER_1_ID)
                            {
                                sListString += $"Manager {uiDeviceSource - MANAGER_0_ID} ";
                            }
                            else if (uiDeviceSource == SOURCE_WIL)
                            {
                                sListString += "WIL ";
                            }
                            else if (uiDeviceSource == SOURCE_HOST)
                            {
                                sListString += "HOST ";
                            }
                            else
                            {
                                sListString += "Unknown ";
                            }

                            // Combine the wBMS message together
                            int uiPayloadLength = (stMessages[lCount - 1].NumberBytesHeader << 8) | stMessages[lCount - 1].NumberBytesData;


                            switch (uiPacketType)
                            {
                                case API_PACKET_TYPE:
                                    sListString += "API ";

                                    //TODO: ignore
                                    break;
                                case BMS_PACKET_TYPE:
                                    {
                                        sListString += "BMS ";

                                        if (uiPayloadLength <= 514)
                                        {

                                            // wBMS Packet Payload is located in the ExtraDataPtr
                                            IntPtr ptr = stMessages[lCount - 1].iExtraDataPtr;

                                            // Copy each packet into the object arrays

                                            if (uiPacketID == 0 && uiPayloadLength > 0)
                                            {
                                                byte[] managedArray = new byte[uiPayloadLength];
                                                Marshal.Copy(ptr, managedArray, 0, uiPayloadLength);

                                                modules[uiDeviceSource].Packet0 = managedArray;

                                                double[] CGV = new double[8];
                                                CGV[0] = BitConverter.ToUInt16(managedArray, 6) * 0.0001;
                                                CGV[1] = BitConverter.ToUInt16(managedArray, 8) * 0.0001;
                                                CGV[2] = BitConverter.ToUInt16(managedArray, 10) * 0.0001;
                                                CGV[3] = BitConverter.ToUInt16(managedArray, 14) * 0.0001;
                                                CGV[4] = BitConverter.ToUInt16(managedArray, 16) * 0.0001;
                                                CGV[5] = BitConverter.ToUInt16(managedArray, 18) * 0.0001;
                                                CGV[6] = BitConverter.ToUInt16(managedArray, 22) * 0.0001;
                                                CGV[7] = BitConverter.ToUInt16(managedArray, 24) * 0.0001;

                                                modules[uiDeviceSource].CGV = CGV;

                                                double[] CGDV = new double[8];
                                                CGDV[0] = BitConverter.ToUInt16(managedArray, 46) * 0.0001;
                                                CGDV[1] = BitConverter.ToUInt16(managedArray, 48) * 0.0001;
                                                CGDV[2] = BitConverter.ToUInt16(managedArray, 50) * 0.0001;
                                                CGDV[3] = BitConverter.ToUInt16(managedArray, 54) * 0.0001;
                                                CGDV[4] = BitConverter.ToUInt16(managedArray, 56) * 0.0001;
                                                CGDV[5] = BitConverter.ToUInt16(managedArray, 58) * 0.0001;
                                                CGDV[6] = BitConverter.ToUInt16(managedArray, 62) * 0.0001;
                                                CGDV[7] = BitConverter.ToUInt16(managedArray, 64) * 0.0001;

                                                modules[uiDeviceSource].CGDV = CGDV;
                                            }
                                            else if (uiPacketID == 1 && uiPayloadLength > 0)
                                            {

                                                byte[] managedArray = new byte[uiPayloadLength];
                                                Marshal.Copy(ptr, managedArray, 0, uiPayloadLength);
                                                /*
                                                managedArray[0] = Marshal.ReadByte(ptr, 8);
                                                managedArray[1] = Marshal.ReadByte(ptr, 9);
                                                managedArray[2] = Marshal.ReadByte(ptr, 10);
                                                managedArray[3] = Marshal.ReadByte(ptr, 11);
                                                */

                                                modules[uiDeviceSource].FillThermistorValues(managedArray.Skip(8).Take(4).ToArray());
                                                modules[uiDeviceSource].Packet1 = managedArray;
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Payload Length exceeds the Maximum size of 514!");
                                        }
                                    }
                                    //TODO: look in ICS WIL DLL Deliverables\wbmsapitester\wbmsapitester\containers for information on decoding bms payload
                                    break;
                                case PMS_PACKET_TYPE:
                                    {
                                        sListString += "PMS ";

                                        if (uiPayloadLength <= 514 && uiPayloadLength > 0)
                                        {
                                            // wBMS Packet Payload is located in the ExtraDataPtr
                                            IntPtr ptr = stMessages[lCount - 1].iExtraDataPtr;

                                            // Copy each packet into the object arrays
                                            byte[] managedArray = new byte[uiPayloadLength];
                                            Marshal.Copy(ptr, managedArray, 0, uiPayloadLength);

                                            managers[0].PMSPacket0 = managedArray;

                                            if (uiPacketID == 0)
                                            {                                                
                                                managers[0].I1 = BitConverter.ToUInt16(managedArray, 11);
                                                managers[0].I2 = BitConverter.ToUInt16(managedArray, 13);
                                                managers[0].VBAT = BitConverter.ToUInt16(managedArray, 15);
                                                Debug.WriteLine($"{BitConverter.ToUInt16(managedArray, 19)} {managedArray[19]} {managedArray[20]}");
                                                Debug.WriteLine(BitConverter.ToUInt16(new byte[] {255,255}, 0));
                                                managers[0].AUX1 = BitConverter.ToUInt16(managedArray, 19);
                                                managers[0].AUX2 = BitConverter.ToUInt16(managedArray, 27);
                                                managers[0].AUX3 = BitConverter.ToUInt16(managedArray, 40);
                                                managers[0].AUX4 = BitConverter.ToUInt16(managedArray, 48);
                                                managers[0].AUX5 = BitConverter.ToUInt16(managedArray, 61);
                                                managers[0].AUX6 = BitConverter.ToUInt16(managedArray, 69);
                                            } else if (uiPacketID == 2)
                                            {
                                                managers[0].AUX7 = BitConverter.ToUInt16(managedArray, 69); //TODO: check if we can look in a different packet for AUX values? They are different sometimes
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Payload Length exceeds the Maximum size of 514!");
                                        }
                                    }

                                    //TODO: look in ICS WIL DLL Deliverables\wbmsapitester\wbmsapitester\containers for information on decoding pms payload
                                    break;
                                case EMS_PACKET_TYPE:
                                    sListString += "EMS ";

                                    if (uiPayloadLength <= 514)
                                    {
                                        // Print out the payload

                                        // wBMS Packet Payload is located in the ExtraDataPtr
                                        IntPtr ptr = stMessages[lCount - 1].iExtraDataPtr;

                                        // Copy each packet into the object arrays

                                        if (uiPacketID == 0 && uiPayloadLength > 0)
                                        {
                                            byte[] managedArray = new byte[uiPayloadLength];
                                            Marshal.Copy(ptr, managedArray, 0, uiPayloadLength);

                                            //Debug.WriteLine($"EMS {managedArray[5]}");

                                            managers[0].C1V = BitConverter.ToUInt16(managedArray, 6);
                                            managers[0].G1V = BitConverter.ToUInt16(managedArray, 24); //TODO: check this is the correct offset, should it be 16?
                                            managers[0].G2V = BitConverter.ToUInt16(managedArray, 26);
                                            managers[0].G3V = BitConverter.ToUInt16(managedArray, 30);
                                            managers[0].G4V = BitConverter.ToUInt16(managedArray, 32);
                                            managers[0].G5V = BitConverter.ToUInt16(managedArray, 34);
                                            managers[0].G6V = BitConverter.ToUInt16(managedArray, 38);
                                            managers[0].G7V = BitConverter.ToUInt16(managedArray, 40);
                                                     
                                            managers[0].EMSPacket0 = managedArray;

                                            managers[0].CD1V = BitConverter.ToUInt16(managedArray,14);
                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Payload Length exceeds the Maximum size of 514!");
                                    }

                                    break;
                                case NETWORK_STATUS_PACKET_TYPE:
                                    sListString += "Network Metadata ";

                                    //TODO: decode payload
                                    break;
                                case HEALTH_REPORTS_PACKET_TYPE:
                                    sListString += "Health Report ";

                                    //TODO: decode payload
                                    break;
                                case SPI_STATS_PACKET_TYPE:
                                    sListString += "SPI Port Statistics ";

                                    //TODO: decode payload
                                    break;
                                case WIL_STATS_PACKET_TYPE:
                                    sListString += "WIL Statistics ";

                                    //TODO: decode payload
                                    break;
                                case EVENT_PACKET_TYPE:
                                    sListString += "Event Notification ";

                                    //TODO: decode payload
                                    break;
                                default:
                                    sListString += "Unknown ";
                                    break;
                            }

                            break;

                        case (int)ePROTOCOL.SPY_PROTOCOL_CAN:
                            // list the arb id
                            sListString = sListString + " ArbID : " + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader)) + "  Data ";
                            break;
                        case (int)ePROTOCOL.SPY_PROTOCOL_CANFD:
                        case (int)ePROTOCOL.SPY_PROTOCOL_ETHERNET:
                            if (stMessages[lCount - 1].ExtraDataPtrEnabled == 0)
                            {
                                //8 bytes of data
                                sListString = sListString + " FD ArbID : " + Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader, 16) + "  Data ";
                            }
                            else
                            {
                                if (stMessages[lCount - 1].Protocol == Convert.ToByte(ePROTOCOL.SPY_PROTOCOL_CANFD))
                                {
                                    //Count for CAN FD
                                    iLongMessageTotalByteCount = stMessages[lCount - 1].NumberBytesData;
                                }
                                else
                                {
                                    //Count for Ethernet
                                    iLongMessageTotalByteCount = ((stMessages[lCount - 1].NumberBytesHeader * 0x100) + stMessages[lCount - 1].NumberBytesData);
                                }

                                //More than 8 bytes of data
                                iDataBytes = new byte[iLongMessageTotalByteCount];

                                Marshal.Copy(stMessages[lCount - 1].iExtraDataPtr, iDataBytes, 0, iLongMessageTotalByteCount);
                                GCHandle gcHandle = GCHandle.Alloc(iDataBytes, GCHandleType.Pinned);

                                if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_CANFD) sListString = sListString + " FD ArbID : " + Convert.ToString(stMessages[lCount - 1].ArbIDOrHeader, 16);
                                sListString = sListString + "  Data ";
                                for (iByteCount = 0; iByteCount < iLongMessageTotalByteCount; iByteCount++)
                                {
                                    sListString = sListString + Convert.ToString(iDataBytes[iByteCount], 16) + " ";
                                }
                                gcHandle.Free();
                            }
                            break;
                        default:
                            // list the headers bytes
                            icsNeoDll.ConvertCANtoJ1850Message(ref stMessages[lCount - 1], ref stJMsg);
                            sListString = sListString + " Data : ";

                            //add the data bytes
                            if (stJMsg.NumberBytesHeader >= 1) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stJMsg.Header1)) + " ";
                            if (stJMsg.NumberBytesHeader >= 2) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stJMsg.Header2)) + " ";
                            if (stJMsg.NumberBytesHeader >= 3) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stJMsg.Header3)) + " ";
                            sListString = sListString + "  ";
                            break;
                    }
                }

                string temp = "";

                //iterate through each node, printing the latest packet in the requested format
                foreach (ModuleData module in modules)
                {
                    if (chkHexFormat)
                    {
                        module.Packet0.ToList().ForEach(x => temp += $"{x:X2} ");
                        lstMessage.Items.Add(temp);
                        temp = "";
                    }
                    else
                    {
                        temp = $"Module {module.MacAddress} Version {module.version}";
                        foreach (double cgv in module.CGV)
                        {
                            temp += $" {cgv:0.000}V";
                        }
                        temp += $" {module.Thermistor1:0.00}C {module.Thermistor2:0.00}C";
                        lstMessage.Items.Add(temp);
                        temp = "";
                    }
                    //lstMessage.Items.Add(String.Join(" ", message.packet0));
                }

                if (chkHexFormat)
                {
                    //PMS data
                    managers[0].PMSPacket0.ToList().ForEach(x => temp += $"{x:X2} ");
                    PMSBox.Items.Add(temp);
                    temp = "";
                    /*
                    managers[1].PMSPacket0.ToList().ForEach(x => temp += $"{x:X2} ");
                    PMSBox.Items.Add(temp);
                    temp = "";
                    */

                    //EMS data
                    managers[0].EMSPacket0.ToList().ForEach(x => temp += $"{x:X2} ");
                    EMSBox.Items.Add(temp);

                    /*
                    managers[1].EMSPacket0.ToList().ForEach(x => temp += $"{x:X2} "); //TODO: make this an option in packet reading
                    EMSBox.Items.Add(temp);
                    */
                }
                else
                {
                    //PMS data
                    temp = $"Manager 0 BEV {managers[0].I1:0.00}A {managers[0].I2:0.00}A {managers[0].GetBEVDCFCPlus():0.00} {managers[0].GetBEVDCFCMinus():0.00} {managers[0].GetBEVShuntTemp():0.00} {managers[0].GetBEVDCFCContactorTemp():0.00} {managers[0].GetBEVMainContactorTemp():0.00} {managers[0].GetBEVVREF():0.00}";
                    PMSBox.Items.Add(temp);
                    temp = $"Manager 0 BETA {managers[0].GetBETAHVCDMinus()} {managers[0].GetBETASA1Temp()} {managers[0].GetBETASA4Temp()} {managers[0].GetBETASA3Temp()} {managers[0].GetBETAShuntTemperature()}";
                    PMSBox.Items.Add(temp);
                    temp = $"Manager 0 BETB {managers[0].GetBETBDCFCDifferential()} {managers[0].GetBETBDCFCMinus()} {managers[0].GetBETBDCFCPlus()} {managers[0].GetBETBSB1Temp()} {managers[0].GetBETBShuntTemp()} {managers[0].GetBETBHVDCMinus()}";
                    PMSBox.Items.Add(temp);
                    Debug.WriteLine(managers[0].PMSPacket0);
                    Debug.WriteLine(managers[0].AUX1);
                    Debug.WriteLine(managers[0].AUX2);

                    //EMS data
                    temp = $"Manager 0 {managers[0].CD1V:0.00V} {managers[0].EMSReferenceVoltage1():0.00} {managers[0].EMSReferenceVoltage2():0.00} {managers[0].EMSTemperature1():0.00} {managers[0].EMSTemperature2():0.00} {managers[0].EMSPressure1():0.00} {managers[0].EMSPressure2():0.00} {managers[0].EMSGas1():0.00} {managers[0].EMSGas2():0.00}";
                    
                    EMSBox.Items.Add(temp);
                }
            }
            else
            {
                MessageBox.Show("Problem Reading Messages");
            }
        }

        private void cmdGetErrors_Click(object sender, EventArgs e)
        {
            int iResult = 0;  //Storage for Result of Call
            int[] iErrors = new int[600];  //Array for Error Numbers
            int iNumberOfErrors = 0;  // Storage for number of errors
            int iCount = 0;   //Counter
            int iSeverity = 0;  //tells the Severity of Error
            int iMaxLengthShort = 0;  //Tells Max length of Error String
            int iMaxLengthLong = 0;	//Tells Max Length of Error String
            int lRestart = 0;  //tells if a restart is needed
            StringBuilder sErrorShort = new StringBuilder(256);  //String for Error
            StringBuilder sErrorLong = new StringBuilder(256);  //String for Error

            iMaxLengthShort = 255; //Set initial conditions
            iMaxLengthLong = 255; //Set initial conditions
            // Read Out the errors
            iResult = icsNeoDll.icsneoGetErrorMessages(m_hObject, ref iErrors[0], ref iNumberOfErrors);

            // Test the returned result
            if (iResult == 0)
            {
                MessageBox.Show("Problem Reading Errors");
            }
            else
            {
                if (iNumberOfErrors != 0)
                {
                    for (iCount = 0; iCount < iNumberOfErrors; iCount++)
                    {
                        lstErrorHolder.Items.Clear();

                        //Get Text Description of the Error
                        iResult = icsNeoDll.icsneoGetErrorInfo(iErrors[iCount], sErrorShort, sErrorLong, ref iMaxLengthShort, ref iMaxLengthLong, ref iSeverity, ref lRestart);
                        lstErrorHolder.Items.Add(sErrorShort + " - Description " + sErrorLong + " - Errornum: " + iErrors[iCount]);
                    }
                }
            }
        }

        public static UInt64[] strToToHexByte(string hexString, int len = 2, int mode = 0)
        {
            String s = "";
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % len) != 0)
                hexString += "0";
            if ((mode == 0) && (len > 2))
            {
                int l = hexString.Length;
                while (l > 0)
                {
                    s += hexString.Substring(l - 2, 2);
                    l -= 2;
                }
                hexString = s;
            }
            UInt64[] returnBytes = new UInt64[hexString.Length / len];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToUInt64(hexString.Substring(i * len, len), 16);

            return returnBytes;
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

        private void button5_Click(object sender, EventArgs e)
        {
            //WilFirmwareVersion(); //don't need to check this
            PMSGPIO(ADI_WIL_DEV_MANAGER_0, 1);
            PMSGPIO(ADI_WIL_DEV_MANAGER_1, 1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < modules.Length; i++)
            {
                
                modules[i].version = DeviceFirmwareVersion(i);
            }
        }
    }
}
