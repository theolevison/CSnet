using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSnet
{
    public class DeviceModel
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

        /*
         ADI_WIL_ERR_SUCCESS Operation successful.
        ADI_WIL_ERR_FAIL The operation failed.
        ADI_WIL_ERR_API_IN_PROGRESS The operation could not be completed because another operation is currently in progress.
        ADI_WIL_ERR_TIMEOUT The operation has timed out.
        ADI_WIL_ERR_NOT_CONNECTED The operation could not be completed because there is no active connection to the network managers.
        ADI_WIL_ERR_INVALID_PARAMETER An invalid parameter has been specified.
        ADI_WIL_ERR_INVALID_STATE The system was in an invalid state when processing the request was made.
        ADI_WIL_ERR_NOT_SUPPORTED The requested feature is not supported.
        ADI_WIL_ERR_EXTERNAL An error has been returned from a HAL or OSAL component.
        ADI_WIL_ERR_INVALID_MODE The requested/current system mode is invalid.
        ADI_WIL_ERR_IN_PROGRESS Requesting a block of data from the user during the file loading process.
        ADI_WIL_ERR_CONFIGURATION_MISMATCH The two managers have mismatched configurations.
        ADI_WIL_ERR_CRC The operation encountered a CRC error.
        ADI_WIL_ERR_FILE_REJECTED The file was rejected by the end device.
        ADI_WIL_ERR_PARTIAL_SUCCESS Some devices failed complete the operation while others were successful.
         */

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

        public string serialNumber;

        public bool chkHexFormat = false;

        public IntPtr m_hObject;         //handle for device

        icsSpyMessage[] stMessages = new icsSpyMessage[20000];   //TempSpace for messages
        private byte currentMode = ADI_WIL_MODE_STANDBY;

        public ModuleData[] modules;
        public ManagerData[] managers = new ManagerData[2];

        public List<string> errors = new List<string>();

        public bool IsSetup = false;

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

        private bool BETLower = false;
        private bool BET = false;
        private bool BEV = false;


        public DeviceModel(ref IntPtr obj, string serialNumber)
        {
            this.serialNumber = serialNumber;
            m_hObject = obj;
        }

        public void CloseDevice()
        {
            IsSetup = false;
            int iNumberOfErrors = 0;            //close the port
            int iResult = icsNeoDll.icsneoClosePort(m_hObject, ref iNumberOfErrors);
            //release resources used by the object
            icsNeoDll.icsneoFreeObject(m_hObject);
        }

        private bool Reset()
        {
            ChangeADIboxSettings();

            managers[0] = new ManagerData();
            if (BET || BEV)
            {
                managers[1] = new ManagerData(); //this manager should be ignored for single manager uses, BEV & BETLower
            }

            //connect to managers first
            Connect(true, true);

            GetAcl();
            Debug.WriteLine($"module count: {nodeMacAddresses.Count}");

            modules = new ModuleData[nodeMacAddresses.Count];

            for (int i = 0; i < nodeMacAddresses.Count; i++)
            {
                modules[i] = new ModuleData();
                modules[i].MacAddress = nodeMacAddresses[i];
            }

            GetDeviceVersions();
            GPIO();

            IsSetup = true;
            return true;
        }

        public bool SetupBEV()
        {
            BET = false;
            BEV = true;
            BETLower = false;
            return Reset();
        }

        public bool SetupBET()
        {
            BET = true;
            BEV = false;
            BETLower = false;
            return Reset();
        }

        public bool SetupBETLower()
        {
            BET = false;
            BEV = false;
            BETLower = true;
            return Reset();
        }
        public bool SetupOP90()
        {
            BET = false;
            BEV = false;
            BETLower = false;
            return Reset();
        }

        private void ChangeADIboxSettings()
        {
            SRADBMSSettingsPack mysettings = new SRADBMSSettingsPack();

            icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref mysettings, Marshal.SizeOf(mysettings), 0);

            //set managers to external or onboard
            byte managerSetting = 0;
            if (BEV || BET || BETLower)
            {
                managerSetting = 129;
            }

            mysettings.Settings.spi_config.port_a.config.value = managerSetting;
            mysettings.Settings.spi_config.port_b.config.value = managerSetting;

            mysettings.Settings.wbms_wil_1.using_port_a = 1;
            if (BET)
            {
                mysettings.Settings.wbms_wil_1.using_port_b = 1;
            }
            else
            {
                mysettings.Settings.wbms_wil_1.using_port_b = 0;
            }

            icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref mysettings, Marshal.SizeOf(mysettings), 1, 0);  //write settings struct
        }

        private void OTAP(Stream fileStream)
        {
            SetMode(ADI_WIL_MODE_STANDBY);
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
            while (count != 0)
            { //not guaranteed to be 64 bytes long
                count = fileStream.Read(buffer, offset, 64);
                //what happens when we hit the end of the file?
                Debug.WriteLine($"reading: {count}");
                offset = SendOTAPFileChunk(offset, buffer);

                if (offset >= fileSize)//TODO: test if this will work
                {
                    //done
                    Debug.WriteLine("offset exceeds filesize");
                    break;
                }

                if (offset == -1)
                {
                    //error
                    Debug.WriteLine("Finished writing");
                    break;
                }
            }
        }

        private bool OTAPCheckStatus(byte uFunctionSelected)
        {
            byte callbackError = 0;
            if (CheckStatus(uFunctionSelected, out callbackError))
            {
                if (callbackError == ADI_WIL_ERR_IN_PROGRESS) //uFinishedProcessing == 1 && 
                {
                    //still writing file
                    Debug.WriteLine("file operation in progress");
                    return false;
                }
                else if (callbackError == ADI_WIL_ERR_SUCCESS)
                {
                    //written entire file
                    return true;
                }
                else
                {
                    //something went wrong

                    throw new Exception("error in otap");
                }
            }
            else
            {
                //something went wrong
                throw new Exception("error in otap");
            }
        }

        private int SendOTAPFileChunk(int offset, byte[] buffer)
        {
            byte[] parameters = new byte[67];

            byte function = ADI_WIL_API_LOAD_FILE;

            //typedef uint64_t adi_wil_device_t;
            //4 bytes enum for file type
            parameters[0] = 254;//ADI_WIL_DEV_MANAGER_0 = 240, ADI_WIL_DEV_MANAGER_1 = 241, ADI_WIL_DEV_ALL_MANAGERS= 254, ADI_WIL_DEV_ALL_NODES = 255
            parameters[1] = 0;//file type, ADI_WIL_FILE_TYPE_FIRMWARE = 0
            parameters[2] = 64;//file size, always send in 64 byte chunks
            Buffer.BlockCopy(buffer, offset, parameters, 3, 64); //copy chunk of file into parameters at position 3

            SendGenericCommand(function, parameters);

            if (!OTAPCheckStatus(function))
            {
                //in progress, read the latest offset

                byte[] pReturnedData = new byte[4];
                Marshal.Copy(ReadGenericCommand(function), pReturnedData, 0, pReturnedData.Length);

                //return offset;
                Debug.WriteLine($"Returned file position: {BitConverter.ToInt32(pReturnedData, 0)}");
                return BitConverter.ToInt32(pReturnedData, 0);
            }
            else
            {
                //finished
                return -1;
            }
        }

        public void GetConfigFile()
        {
            SetMode(ADI_WIL_MODE_STANDBY);

            //open file writer stream
            string filename = @"c:\Users\Public\Documents\configFile.txt";

            using (FileStream SourceStream = File.Open(filename, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.Begin);//go to the start of the file. I don't think this is necessary, unless we are writing over the top

                while (GetConfigurationFileChunk(SourceStream))
                {
                    //method appends to file
                }
                //close & save the file

            }

            //find version
        }

        private bool GetConfigurationFileChunk(FileStream SourceStream) //Current broken in icsNeoDLL, afaik
        {
            byte[] parameters = new byte[9];

            //typedef uint64_t adi_wil_device_t;
            //4 bytes enum for file type
            int deviceID = 2; //module 3
            //int deviceID = 62; //Manager 0
            BitConverter.GetBytes(Convert.ToUInt64(Math.Pow(2, deviceID))).CopyTo(parameters, 0); //add address to parameters

            parameters[8] = 2;//file type, ADI_WIL_FILE_TYPE_CONFIGURATION = 2

            byte function = ADI_WIL_API_GET_FILE;
            SendGenericCommand(function, parameters);

            if (!OTAPCheckStatus(function))
            {
                //in progress, read the latest offset                
                adi_wil_file_t fileChunk = (adi_wil_file_t)Marshal.PtrToStructure(ReadGenericCommand(function), typeof(adi_wil_file_t));

                //iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);
                //adi_wil_file_t fileChunk = (adi_wil_file_t)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), typeof(adi_wil_file_t));

                // wBMS Packet Payload is located in the ExtraDataPtr
                //IntPtr ptr = fileChunk.pData;
                Debug.WriteLine(fileChunk.pData);
                //Debug.WriteLine(Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 5));

                byte[] managedArray = new byte[fileChunk.iByteCount * 5];
                //Buffer.BlockCopy(pReturnedData, 5, managedArray, 0, fileChunk.iByteCount);

                //Debug.WriteLine(managedArray[0]);

                Marshal.Copy(fileChunk.pData, managedArray, 0, managedArray.Length - 40); //for some reason, this results in an access violation exception

                //append file chunk
                SourceStream.Write(managedArray, 0, managedArray.Length);

                return true;
            }
            else
            {
                MessageBox.Show("Succeeded"); //Either way close the filestream
                return false;
            }
        }

        private void Connect(bool portA, bool portB)
        {
            byte function = ADI_WIL_API_CONNECT;
            ushort bufferSize = 512;
            byte[] parameters = new byte[4];
            parameters[0] = 1;//portA
            parameters[1] = (byte)(!BEV || BET ? 1 : 0);//portB

            //parameters[2] = BitConverter.GetBytes((ushort)512)[0];
            //parameters[3] = BitConverter.GetBytes((ushort)512)[1];

            parameters[2] = (byte)(bufferSize & 0xFF);
            parameters[3] = (byte)((bufferSize >> 8) & 0xFF);

            SendGenericCommand(function, parameters);

            byte callbackError = 0;
            CheckStatus(function, out callbackError);

            ReadGenericCommand(function); //read, but we don't care about the output
        }

        public List<uint> GetFileCRC()
        {
            SetMode(ADI_WIL_MODE_STANDBY);

            byte[] parameters = new byte[9];
            ulong deviceID = ADI_WIL_DEV_ALL_NODES; //module 3
            //int deviceID = 62; //Manager 0
            //BitConverter.GetBytes(Convert.ToUInt64(Math.Pow(2, deviceID))).CopyTo(parameters, 0); //add address to parameters
            BitConverter.GetBytes(deviceID).CopyTo(parameters, 0);

            parameters[8] = 2;//file type, ADI_WIL_FILE_TYPE_CONFIGURATION = 2

            byte function = ADI_WIL_API_GET_FILE_CRC;
            SendGenericCommand(function, parameters);

            byte callbackError = 0;
            CheckStatus(function, out callbackError);

            adi_wil_file_crc_list_t crcList = (adi_wil_file_crc_list_t)Marshal.PtrToStructure(ReadGenericCommand(function), typeof(adi_wil_file_crc_list_t));

            //TODO: foreach file that exists, get crc and compare it against others (just first?) and if there's a mismatch, throw error

            string bitmapString = Convert.ToString((long)crcList.eFileExists, 2);
            bool[] bitmap = bitmapString.Select(x => x.Equals('1')).Reverse().ToArray();
            //bitmap = bitmap.Reverse().ToArray();

            List<uint> CRCs = new List<uint>();
            for (int i = 0; i < bitmap.Length; i++)
            {
                if (bitmap[i])
                {
                    CRCs.Add(crcList.iCRC[i]);
                }
            }

            return CRCs;
        }

        public void GPIO()
        {
            if (BEV || BETLower)
            {
                PMSGPIO(ADI_WIL_DEV_MANAGER_0, 1);
            }
            else if (BET)
            {
                PMSGPIO(ADI_WIL_DEV_MANAGER_0, 1);
                PMSGPIO(ADI_WIL_DEV_MANAGER_1, 1);
            }
                
        }

        private void PMSGPIO(ulong manager, byte highLow)
        {
            byte function = ADI_WIL_API_SET_GPIO; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[10];

            BitConverter.GetBytes(manager).CopyTo(parameters, 0);
            //parameters[0] = ADI_WIL_DEV_MANAGER_0; //target first manager //64 or 65
            parameters[8] = 4; // pin 9
            parameters[9] = highLow; // set to high

            SendGenericCommand(function, parameters);

            /*
                The GPIO pin on the target device is set to be an output pin before its value is set. 
                Note that the GPIO pin is set to output mode only, therefore it cannot be set to a value 
                then re-read back by using adi_wil_GetGPIO since adi_wil_GetGPIO will reconfigure the pin 
                to input mode in order to be read 
             */
        }

        public int DeviceFirmwareVersion(int deviceID)
        {
            SetMode(ADI_WIL_MODE_STANDBY);

            byte function = ADI_WIL_API_GET_DEVICE_VERSION; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[8];

            //node id's are converted to powers of 2, for some reason
            BitConverter.GetBytes(Convert.ToUInt64(Math.Pow(2, deviceID))).CopyTo(parameters, 0);

            SendGenericCommand(function, parameters);

            adi_wil_dev_version_t version = (adi_wil_dev_version_t)Marshal.PtrToStructure(ReadGenericCommand(function), typeof(adi_wil_dev_version_t));

            string temp = "";
            parameters.ToList().ForEach(x => temp += x);
            Debug.WriteLine($"node {deviceID} {temp} version: {version.MainProcSWVersion.iVersionMajor}.{version.MainProcSWVersion.iVersionMinor}.{version.MainProcSWVersion.iVersionPatch}");

            return version.MainProcSWVersion.iVersionMajor;
        }

        public ushort WilFirmwareVersion()
        {
            SetMode(ADI_WIL_MODE_STANDBY);

            byte function = ADI_WIL_API_GET_WIL_SOFTWARE_VERSION; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;

            SendGenericCommand(function, new byte[0]);

            adi_wil_dev_version_t version = (adi_wil_dev_version_t)Marshal.PtrToStructure(ReadGenericCommand(function), typeof(adi_wil_dev_version_t));

            Debug.WriteLine($"WIL version object {version}");

            return version.MainProcSWVersion.iVersionMajor;
        }

        private enum adi_wil_contextual_id_t
        {
            ADI_WIL_CONTEXTUAL_ID_0,             // Contextual data ID 0
            ADI_WIL_CONTEXTUAL_ID_1,             // Contextual data ID 1
            ADI_WIL_CONTEXTUAL_ID_WRITE_ONCE     // Write once contextual data
        }

        public void GetContextualData(int deviceID)
        {
            SetMode(ADI_WIL_MODE_STANDBY);

            byte function = 23;  //ADI_WIL_API_GET_CONTEXTUAL_DATA = 23
            byte[] parameters = new byte[9];

            //node id's are converted to powers of 2, for some reason
            BitConverter.GetBytes(Convert.ToUInt64(Math.Pow(2, deviceID))).CopyTo(parameters, 0);

            parameters[8] = Convert.ToByte(adi_wil_contextual_id_t.ADI_WIL_CONTEXTUAL_ID_0);

            SendGenericCommand(function, parameters);

            adi_wil_contextual_data_t contextualData = (adi_wil_contextual_data_t)Marshal.PtrToStructure(ReadGenericCommand(function), typeof(adi_wil_contextual_data_t));

            Debug.WriteLine(contextualData.iLength);

            //adi_wil_contextual_data_t
        }

        private IntPtr ReadGenericCommand(byte function)
        {
            byte uAPISelected, uInstanceSelected, uCurrentFunction;
            byte[] pReturnedData = new byte[512];

            uint uReturnedDataLength;
            //adi_wil_dev_version_t version = new adi_wil_dev_version_t();
            //IntPtr pointer = IntPtr.Zero;
            //TODO: use pointer instead of marshaling pReturnedData?

            uAPISelected = 1;
            uInstanceSelected = 0;

            int iResult = icsNeoDll.icsneoGenericAPIReadData(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0), out uReturnedDataLength);

            if (uCurrentFunction != function || iResult != 1) //1 is success
            {
                //Handle Error Here
                MessageBox.Show($"Read error {iResult}");
            }

            return Marshal.UnsafeAddrOfPinnedArrayElement(pReturnedData, 0);
        }
        private void SendGenericCommand(byte function, byte[] parameters)
        {
            byte functionError = 0;

            int iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), (uint)parameters.Length, out functionError);

            if (iResult != 1)
            {
                Debug.WriteLine("Couldn't send command");
            }

            byte callbackError = 0;
            CheckStatus(function, out callbackError);
        }

        public void SetACL()
        {
            List<string> macAddresses = ACLPage.GetStoredMacAddresses();

            List<byte> ACLList = new List<byte>
            {
                (byte)macAddresses.Count
            };

            for (int i = 0; i < macAddresses.Count; i++)
            {
                ACLList.AddRange(new byte[] { 0x64, 0xF9, 0xC0, 0x00, 0x00, Convert.ToByte(macAddresses[i].Substring(0, 2), 16), Convert.ToByte(macAddresses[i].Substring(2, 2), 16), Convert.ToByte(macAddresses[i].Substring(4, 2), 16) });
            }

            SetMode(ADI_WIL_MODE_STANDBY);

            SendGenericCommand(ADI_WIL_API_SET_ACL, ACLList.ToArray());

            Reset();
        }

        private bool CheckStatus(byte uFunctionSelected, out byte uCallbackError)
        {
            int timeOutCounter = 0, result = 0;

            byte uFinishedProcessing = 1, uCurrentFunction, uAPISelected = 1, uInstanceSelected = 0;

            do
            {
                result = icsNeoDll.icsneoGenericAPIGetStatus(m_hObject, uAPISelected, uInstanceSelected, out uCurrentFunction, out uCallbackError, out uFinishedProcessing);

                if (timeOutCounter == 200)
                {
                    throw new Exception($"Timeout whilst sending command to device {uCallbackError}");
                }
                timeOutCounter++;
                Thread.Sleep(100);
                //only breaks if command has been successfully sent, or timesout
            } while (uFinishedProcessing == 0);

            if (result == 1)
            {
                if (uCallbackError == ADI_WIL_ERR_SUCCESS)
                {
                    return true;
                }
                else if (uCallbackError == ADI_WIL_ERR_PARTIAL_SUCCESS)
                {
                    return true; //TODO: make it obvious that there was a partial success
                }
                else if (uCallbackError == ADI_WIL_ERR_IN_PROGRESS)
                {
                    Debug.WriteLine("In progress");
                    return true;
                }
                else if (uCallbackError == ADI_WIL_ERR_TIMEOUT)
                {
                    Debug.WriteLine("Timeout");
                    return false;
                }
                else
                {
                    Debug.WriteLine($"Operation not succesful, error: {uCallbackError}");
                    throw new Exception("unhandled status error");
                }
            }
            else
            {
                Debug.WriteLine("Command not sent");
                return false;
            }
            //TODO: add statements for all errors? Switch case?
        }

        private void SetMode(byte mode)
        {
            if (mode != currentMode)
            {
                byte[] parameters = new byte[1];

                parameters[0] = mode; //3 = active mode

                SendGenericCommand(ADI_WIL_API_SET_MODE, parameters);

                currentMode = mode;
            }
        }

        private Dictionary<int, string> nodeMacAddresses = new Dictionary<int, string>();

        public void GetAcl()//获取ACL
        {
            nodeMacAddresses.Clear();
            //get ACL is available in all system modes
            byte function = ADI_WIL_API_GET_ACL;

            SendGenericCommand(function, new byte[0]);

            byte[] returnedData = new byte[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES + 1];

            Marshal.Copy(ReadGenericCommand(function), returnedData, 0, returnedData.Length);

            byte uNodeCount = returnedData[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES];

            for (int i = 0; i < uNodeCount; i++)
            {
                byte[] byteMacAddress = new byte[ADI_WIL_MAC_SIZE];
                Buffer.BlockCopy(returnedData, i * ADI_WIL_MAC_SIZE, byteMacAddress, 0, ADI_WIL_MAC_SIZE);
                string stringMacAddress = "";
                foreach (byte x in byteMacAddress)
                {
                    stringMacAddress += $"{x:X2}";
                }
                if (!nodeMacAddresses.ContainsKey(i))
                {
                    nodeMacAddresses.Add(i, stringMacAddress);
                }
            }
        }

        public void OTAP()
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
        public void GetMessages()
        {
            SetMode(ADI_WIL_MODE_ACTIVE);

            long lResult;
            int lNumberOfMessages = 0;
            int lNumberOfErrors = 0;
            long lCount;

            double dTime = 0;

            // read the messages from the driver
            lResult = icsNeoDll.icsneoGetMessages(m_hObject, ref stMessages[0], ref lNumberOfMessages, ref lNumberOfErrors);

            // was the read successful?
            if (lResult == 1)
            {
                // for each message we read
                for (lCount = 1; lCount <= lNumberOfMessages; lCount++)
                {
                    lResult = icsNeoDll.icsneoGetTimeStampForMsg(m_hObject, ref stMessages[lCount - 1], ref dTime);

                    //Decode based on the protocol
                    switch (stMessages[lCount - 1].Protocol)
                    {
                        case (int)ePROTOCOL.SPY_PROTOCOL_WBMS:
                            //this is the only protocol we care about

                            int iNetId = (stMessages[lCount - 1].NetworkID2 << 8) | stMessages[lCount - 1].NetworkID;

                            if (iNetId == 532)
                            {
                                //wBMS 01;
                            }
                            else
                            {
                                //This is not a wBMS message
                            }
                            // Grab wBMS specific fields from the ArbIDOrHeader value
                            uint uiPacketType = GetPacketTypeFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);
                            uint uiPacketID = GetPacketIdFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);
                            uint uiDeviceSource = GetSourceFromArbId((uint)stMessages[lCount - 1].ArbIDOrHeader);

                            // Combine the wBMS message together
                            int uiPayloadLength = (stMessages[lCount - 1].NumberBytesHeader << 8) | stMessages[lCount - 1].NumberBytesData;

                            if (uiPayloadLength <= 514 && uiPayloadLength > 0)
                            {
                                byte[] packet = ConvertPacketToManaged(stMessages[lCount - 1], uiPayloadLength);

                                switch (uiPacketType)
                                {
                                    case API_PACKET_TYPE:
                                        //ignore
                                        break;
                                    case BMS_PACKET_TYPE:
                                        modules[uiDeviceSource].UpdateData(packet, uiPacketID, dTime);

                                        break;
                                    case PMS_PACKET_TYPE:
                                        Console.WriteLine($"PMS {uiDeviceSource}");
                                        managers[uiDeviceSource-240].UpdatePMSData(packet, uiPacketID);

                                        break;
                                    case EMS_PACKET_TYPE:
                                        managers[0].UpdateEMSData(packet, uiPacketID);

                                        break;
                                    case NETWORK_STATUS_PACKET_TYPE:
                                        //Network Metadata
                                        modules[uiDeviceSource].UpdateMetadata(packet, uiPacketID);

                                        break;
                                    case HEALTH_REPORTS_PACKET_TYPE:
                                        //Health Report
                                        if (uiDeviceSource < 64)
                                        {
                                            modules[uiDeviceSource].UpdateHealthdata(packet, uiPacketID);
                                        }

                                        break;
                                    case SPI_STATS_PACKET_TYPE:
                                        //SPI Port Statistics

                                        //TODO: decode payload
                                        break;
                                    case WIL_STATS_PACKET_TYPE:
                                        //WIL Statistics

                                        //TODO: decode payload
                                        break;
                                    case EVENT_PACKET_TYPE:
                                        //Event Notification

                                        //TODO: decode payload
                                        break;
                                    default:
                                        //Unknown
                                        break;
                                }
                            }
                            break;

                        case (int)ePROTOCOL.SPY_PROTOCOL_CAN:
                            break;
                        case (int)ePROTOCOL.SPY_PROTOCOL_CANFD:
                            break;
                        case (int)ePROTOCOL.SPY_PROTOCOL_ETHERNET:
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Problem Reading Messages");
            }
        }

        public void GetErrors()
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
                        //Get Text Description of the Error
                        iResult = icsNeoDll.icsneoGetErrorInfo(iErrors[iCount], sErrorShort, sErrorLong, ref iMaxLengthShort, ref iMaxLengthLong, ref iSeverity, ref lRestart);
                        errors.Add(sErrorShort + " - Description " + sErrorLong + " - Errornum: " + iErrors[iCount]);
                    }
                }
            }
        }

        private byte[] ConvertPacketToManaged(icsSpyMessage message, int payloadLength)
        {
            //wBMS Packet Payload is located in the ExtraDataPtr

            byte[] managedArray = new byte[payloadLength];
            Marshal.Copy(message.iExtraDataPtr, managedArray, 0, payloadLength);

            return managedArray;
        }
        
        public void GetDeviceVersions()
        {
            SetMode(ADI_WIL_MODE_ACTIVE);
            if (WaitForModulesToConnect())
            {
                Debug.WriteLine($"Getting versions");
                for (int i = 0; i < modules.Length; i++)
                {
                    modules[i].version = DeviceFirmwareVersion(i);
                }
                managers[0].Version = DeviceFirmwareVersion(62); //afaik, manager 0 is always the primary manager, responsible for EMS & PMS
                if (BET)
                {
                    managers[1].Version = DeviceFirmwareVersion(63);
                }
            }
        }

        public byte[] GetPMS()
        {
            if (BEV)
            {
                return managers[0].GetBEVPMS();
            } else if (BETLower)
            {
                return managers[0].GetBETBPMS();
            } else
            {
                return managers[0].GetBETAPMS().Concat(managers[1].GetBETBPMS()).ToArray();
            }
        }
        public string PrettyPrintPMS()
        {
            string outputText = "";
            if (BEV)
            {
                outputText = $"BEV I1:{managers[0].I1:0.00}A I2:{managers[0].I2:0.00}A DCFC+:{managers[0].GetBEVDCFCPlus():0.00}V DCFC-:{managers[0].GetBEVDCFCMinus():0.00}V ShuntTemp:{managers[0].GetBEVShuntTemp():0.00}C ContactorTemp:{managers[0].GetBEVDCFCContactorTemp():0.00}C MainContactorTemp:{managers[0].GetBEVMainContactorTemp():0.00}C VRef:{managers[0].GetBEVVREF():0.00}V";
            } else if (BETLower)
            {
                outputText = $"BETLower HVDC-:{managers[0].GetBETAHVDCMinus():0.00} SA1Temp:{managers[0].GetBETASA1Temp():0.00}C SA4Temp:{managers[0].GetBETASA4Temp():0.00}C SA3Temp:{managers[0].GetBETASA3Temp():0.00}C ShuntTemp:{managers[0].GetBETAShuntTemperature():0.00}C";
            } else
            {
                outputText = $"BETB HVDC-:{managers[1].GetBETBHVDCMinus():0.00}V Diff:{managers[1].GetBETBDCFCDifferential():0.00}V DCFC-:{managers[1].GetBETBDCFCMinus():0.00}V DCFC+:{managers[1].GetBETBDCFCPlus():0.00}V SB1Temp:{managers[1].GetBETBSB1Temp():0.00}C ShuntTemp:{managers[1].GetBETBShuntTemp():0.00}C";
                outputText += $"\nBETA HVDC-:{managers[0].GetBETAHVDCMinus():0.00}V SA1Temp:{managers[0].GetBETASA1Temp():0.00}C SA4Temp:{managers[0].GetBETASA4Temp():0.00}C SA3Temp:{managers[0].GetBETASA3Temp():0.00}C ShuntTemp:{managers[0].GetBETAShuntTemperature():0.00}C";
            }
            return outputText;
        }
        public bool WaitForModulesToConnect()
        {
            int timeoutCounter = 0;
            do
            {
                SendGenericCommand(ADI_WIL_API_GET_NETWORK_STATUS, new byte[0]);

                byte[] output = new byte[9];
                Marshal.Copy(ReadGenericCommand(ADI_WIL_API_GET_NETWORK_STATUS), output, 0, output.Length);

                //TODO: repeat this command until timeout (to one we pick) or until all nodes are checked against the bitmap returned from 

                BitArray bitArray = new BitArray(output.Skip(1).ToArray());

                int connectedCount = 0;
                foreach (bool b in bitArray)
                {
                    if (b)
                    {
                        connectedCount++;
                    }
                }
                Debug.WriteLine($"running count: {connectedCount}");

                if (connectedCount == output[0])
                {
                    return true;
                    //all modules have connected.
                }
                timeoutCounter++;
                Thread.Sleep(1000);
            } while (timeoutCounter < 8);
            MessageBox.Show("Connecting to modules timed-out, check ACL");
            return false;
        }

        /*
        iRetVal = WIL_RequestGetNetworkStatus(hObject, WIL_INSTANCE_0, &nwStatus);
        CU_ASSERT_INT_EQUAL(iRetVal, ADI_WIL_RETURN_SUCCESS);

        CU_ASSERT_INT_EQUAL(nwStatus.iCount, GLOBAL_TEST_CONFIG_NUMBER_NODES_UNDER_TEST); 
        iNodesConnected = 0;
        for (int i = 0; i < nwStatus.iCount; i++)
        {
            if((nwStatus.iConnectState >> i) & 0x1)
                iNodesConnected++;
        } 
        pStatus->iCount= bData[ADI_WIL_GET_NETWORK_STATUS_RETURN_NODE_COUNT_OFFSET];
        pStatus->iConnectState = *(uint64_t*)(bData + ADI_WIL_GET_NETWORK_STATUS_RETURN_NODE_CONNECTED_BITMASK_OFFSET);
         */
    }
}
