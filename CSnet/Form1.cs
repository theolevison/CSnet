﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSnet
{
    public partial class Form1 : Form
    {

        IntPtr m_hObject;		 //handle for device
        bool m_bPortOpen;	 //tells the port status of the device
        icsSpyMessage[] stMessages = new icsSpyMessage[20000];   //TempSpace for messages
        byte[] NetworkIDConvert = new byte[15]; // Storage to convert listbox index to Network ID's 
        int iOpenDeviceType; //Storage for the device type that is open
        OptionsNeoEx neoDeviceOption = new OptionsNeoEx();

        public Form1()
        {
            InitializeComponent();
        }

        private void CmdOpenFirstDevice_Click(object sender, EventArgs e)
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

            //check if the port is already open
            if (m_bPortOpen == true) return;

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

            //Open the first found device
            iResult = icsNeoDll.icsneoOpenDevice(ref ndNeoToOpenex[0], ref m_hObject, ref bNetwork[0], 1, 0, ref OptionOpenNeoEX, 0);
            if (iResult == 1)
            {
                MessageBox.Show("Port Opened OK!");
            }
            else
            {
                MessageBox.Show("Problem Opening Port");
                return;
            }


            //Set the device type for later use
            iOpenDeviceType = ndNeoToOpenex[0].neoDevice.DeviceType;

            //Convert the Serial number to a String
            icsNeoDll.icsneoSerialNumberToString(Convert.ToUInt32(ndNeoToOpenex[0].neoDevice.SerialNumber), ref bSN[0], ref StringSize);
            sConvertedSN = System.Text.ASCIIEncoding.ASCII.GetString(bSN);

            //display the connect hardware type
            switch (ndNeoToOpenex[0].neoDevice.DeviceType)
            {
                case (int)eHardwareTypes.NEODEVICE_BLUE:
                    lblneoInfo.Text = "neoVI Blue SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_DW_VCAN:
                    lblneoInfo.Text = "Value CAN 2 SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE:
                    lblneoInfo.Text = "neoVI FIRE SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN3:
                    lblneoInfo.Text = "ValueCAN 3 SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE2:
                    lblneoInfo.Text = "neoVI FIRE 2 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_PLASMA:
                    lblneoInfo.Text = "neoVI PLASMA SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_ION:
                    lblneoInfo.Text = "neoVI ION SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCANRF:
                    lblneoInfo.Text = "ValueCAN.rf SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADGALAXY:
                    lblneoInfo.Text = "RadGalaxy SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN42:
                    lblneoInfo.Text = "ValueCAN 4-2 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN41:
                    lblneoInfo.Text = "ValueCAN 4-1 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN42_EL:
                    lblneoInfo.Text = "ValueCAN 4-2EL SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN4_IND:
                    lblneoInfo.Text = "ValueCAN 4 IND SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADPLUTO:
                    lblneoInfo.Text = "RAD Pluto SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADSUPERMOON:
                    lblneoInfo.Text = "RAD SuperMoon SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADMOON2:
                    lblneoInfo.Text = "RAD Moon2 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADMOON3:
                    lblneoInfo.Text = "RAD Moon3 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_GIGASTAR:
                    lblneoInfo.Text = "RAD Gigastar SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RED2:
                    lblneoInfo.Text = "neoVI RED2 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3:
                    lblneoInfo.Text = "neoVI FIRE3 SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3_FLEXRAY:
                    lblneoInfo.Text = "neoVI FIRE3 FR SN " + sConvertedSN;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADJUPITER:
                    lblneoInfo.Text = "RAD Jupiter SN " + sConvertedSN;
                    break;
                default:
                    lblneoInfo.Text = "Unknown neoVI SN " + Convert.ToString(ndNeoToOpenex[0].neoDevice.SerialNumber);
                    break;
            }

            //Set device open flag
            m_bPortOpen = true;
        }

        private void cmdCloseDevice_Click(object sender, EventArgs e)
        {
            int iResult;
            int iNumberOfErrors = 0;

            if (Timer1.Enabled == true)
            {
                Timer1.Enabled = false;
                chkAutoRead.Checked = false;
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

            lblneoInfo.Text = "Port Not Opened";

            //Clear device type and open flag
            iOpenDeviceType = 0;
            m_bPortOpen = false;
        }

        private void cmdVersion_Click(object sender, EventArgs e)
        {
            //get version information and send to hardware.
            cmdVersion.Text = Convert.ToString(icsNeoDll.icsneoGetDLLVersion());
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

        private void chkAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bPortOpen == false)
            {
                Timer1.Enabled = false;
                chkAutoRead.Checked = false;
                return;
            }
            else
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

        }

        private void cmdWaitForMessageWithTimeOut_Click(object sender, EventArgs e)
        {
            int iResult;
            UInt32 iTimeOut = 5000;


            lblWaitForRxMessageWithTimeOutResult.Text = "Status";
            Application.DoEvents();

            //This function will block until, A: A Message is received by the hardware, or B: the timeout is reached
            iResult = icsNeoDll.icsneoWaitForRxMessagesWithTimeOut(m_hObject, iTimeOut);
            if (iResult == 1)
            {
                //Message received before timeout
                lblWaitForRxMessageWithTimeOutResult.Text = "Message received";
                cmdReceive_Click(sender, e);
            }
            else
            {
                //Timeout reached and no messages received
                lblWaitForRxMessageWithTimeOutResult.Text = "Message Not received";
            }
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

        private void cmdReceive_Click(object sender, EventArgs e)
        {
            long lResult;
            int lNumberOfMessages = 0;
            int lNumberOfErrors = 0;
            long lCount;
            string sListString;
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            long iByteCount;
            int iLongMessageTotalByteCount;
            double dTime = 0;
            byte[] iDataBytes;

            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return;  // do not read messages if we haven't opened neoVI yet
            }

            /*
            //set mode to active
            //adi_wil_SetMode(m_hObject, 3);
            byte functionError = 0;
            byte function = 4; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[512];

            parameters[0] = 3; //3 = active mode

            icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 1, out functionError);1
            */
            setMode(ADI_WIL_MODE_ACTIVE);

            // read the messages from the driver
            lResult = icsNeoDll.icsneoGetMessages(m_hObject, ref stMessages[0], ref lNumberOfMessages, ref lNumberOfErrors);
            // was the read successful?
            if (lResult == 1)
            {
                // clear the previous list of messages
                lstMessage.Items.Clear();
                lblReadCount.Text = "Number Read : " + Convert.ToString(lNumberOfMessages);
                lblReadErrors.Text = "Number Errors : " + Convert.ToString(lNumberOfErrors);
                // for each message we read
                for (lCount = 1; lCount <= lNumberOfMessages; lCount++)
                {
                    // Calculate the messages timestamp
                    lResult = icsNeoDll.icsneoGetTimeStampForMsg(m_hObject, ref stMessages[lCount - 1], ref dTime);

                    sListString = "Time : " + Convert.ToString(dTime);  //Build String

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
                            Debug.WriteLine("Protocol: wbms");

                            //translate data from raw form, to data we can use
                            int iNetId = (stMessages[lCount - 1].NetworkID2 << 8) | stMessages[lCount - 1].NetworkID;

                            Debug.WriteLine(iNetId);

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

                            Debug.WriteLine($"{uiPacketType} {uiPacketID} {uiDeviceSource} ");

                            if (0 <= uiDeviceSource && uiDeviceSource < MAX_NODES)
                            {
                                sListString += $"Node {uiDeviceSource} ";
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

                            //TODO: need to go into active mode to get useful packets
                            //WIL_RequestSetMode??//ADI_WIL_API_SET_MODE
                            //adi_wil_SetMode()


                            switch (uiPacketType)
                            {
                                case API_PACKET_TYPE:
                                    sListString += "API ";

                                    //TODO: ignore
                                    break;
                                case BMS_PACKET_TYPE:
                                    sListString += "BMS ";

                                    //TODO: look in ICS WIL DLL Deliverables\wbmsapitester\wbmsapitester\containers for information on decoding bms payload
                                    break;
                                case PMS_PACKET_TYPE:
                                    sListString += "PMS ";

                                    //TODO: look in ICS WIL DLL Deliverables\wbmsapitester\wbmsapitester\containers for information on decoding pms payload
                                    break;
                                case EMS_PACKET_TYPE:
                                    sListString += "EMS ";

                                    //TODO: look in ICS WIL DLL Deliverables\wbmsapitester\wbmsapitester\containers for information on decoding ems payload
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


                            // Combine the wBMS message together
                            int uiPayloadLength = (stMessages[lCount - 1].NumberBytesHeader << 8) | stMessages[lCount - 1].NumberBytesData;
                            Debug.WriteLine($"Payload Length: {uiPayloadLength}");

                            if (uiPayloadLength <= 514)
                            {
                                // Print out the payload
                                Debug.Write("Payload: ");

                                // wBMS Packet Payload is located in the ExtraDataPtr
                                IntPtr ptr = stMessages[lCount - 1].iExtraDataPtr;

                                // Print out the entire payload
                                for (int uiDataIndex = 0; uiDataIndex < uiPayloadLength; uiDataIndex++)
                                {
                                    byte data = Marshal.ReadByte(ptr, uiDataIndex);
                                    Debug.Write($"{data:X2}\t");
                                    sListString += data;
                                }


                            }
                            else
                            {
                                Debug.WriteLine("Payload Length exceeds the Maximum size of 514!");
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

                                System.Runtime.InteropServices.Marshal.Copy(stMessages[lCount - 1].iExtraDataPtr, iDataBytes, 0, iLongMessageTotalByteCount);
                                System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(iDataBytes, System.Runtime.InteropServices.GCHandleType.Pinned);

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


                    if (stMessages[lCount - 1].NumberBytesData >= 1 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data1)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 2 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data2)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 3 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data3)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 4 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data4)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 5 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data5)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 6 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data6)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 7 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data7)) + " ";
                    if (stMessages[lCount - 1].NumberBytesData >= 8 && Convert.ToBoolean(stMessages[lCount - 1].ExtraDataPtrEnabled == 0)) sListString = sListString + icsNeoDll.ConvertToHex(Convert.ToString(stMessages[lCount - 1].Data8)) + " ";

                    if (stMessages[lCount - 1].Protocol == (int)ePROTOCOL.SPY_PROTOCOL_LIN)
                    {
                        sListString = sListString + "CS=" + stJMsg.ACK1.ToString("X2");
                    }

                    //Add the message to the list
                    lstMessage.Items.Add(sListString);

                }
            }
            else
            {
                MessageBox.Show("Problem Reading Messages");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            iOpenDeviceType = 0;

            TabControl1.SelectTab(0);
            TabControl2.SelectTab(0);

            //Fill dropdowns
            lstNetwork.Items.Add("DEVICE");
            lstNetwork.Items.Add("HSCAN");
            lstNetwork.Items.Add("MSCAN");
            lstNetwork.Items.Add("SWCAN");
            lstNetwork.Items.Add("LSFTCAN");
            lstNetwork.Items.Add("HSCAN2");
            lstNetwork.Items.Add("HSCAN3");
            lstNetwork.Items.Add("HSCAN4");
            lstNetwork.Items.Add("HSCAN5");
            lstNetwork.Items.Add("HSCAN6");
            lstNetwork.Items.Add("HSCAN7");
            lstNetwork.Items.Add("DWCAN9");
            lstNetwork.Items.Add("DWCAN10");
            lstNetwork.Items.Add("DWCAN11");
            lstNetwork.Items.Add("DWCAN12");
            lstNetwork.Items.Add("DWCAN13");
            lstNetwork.Items.Add("DWCAN14");
            lstNetwork.Items.Add("DWCAN15");
            lstNetwork.Items.Add("DWCAN16");


            lstISO15765Network.Items.Add("DEVICE");
            lstISO15765Network.Items.Add("HSCAN");
            lstISO15765Network.Items.Add("MSCAN");
            lstISO15765Network.Items.Add("SWCAN");
            lstISO15765Network.Items.Add("LSFTCAN");
            lstISO15765Network.Items.Add("HSCAN2");
            lstISO15765Network.Items.Add("HSCAN3");
            lstISO15765Network.Items.Add("HSCAN4");
            lstISO15765Network.Items.Add("HSCAN5");
            lstISO15765Network.Items.Add("HSCAN6");
            lstISO15765Network.Items.Add("HSCAN7");
            lstISO15765Network.Items.Add("DWCAN9");
            lstISO15765Network.Items.Add("DWCAN10");
            lstISO15765Network.Items.Add("DWCAN11");
            lstISO15765Network.Items.Add("DWCAN12");
            lstISO15765Network.Items.Add("DWCAN13");
            lstISO15765Network.Items.Add("DWCAN14");
            lstISO15765Network.Items.Add("DWCAN15");
            lstISO15765Network.Items.Add("DWCAN16");

            lstOtherNetwork.Items.Add("ISO");
            lstOtherNetwork.Items.Add("ISO2");
            lstOtherNetwork.Items.Add("ISO3");
            lstOtherNetwork.Items.Add("ISO4");

            lstLINNetwork.Items.Add("LIN1");
            lstLINNetwork.Items.Add("LIN2");
            lstLINNetwork.Items.Add("LIN3");
            lstLINNetwork.Items.Add("LIN4");
            lstLINNetwork.Items.Add("LIN5");
            lstLINNetwork.Items.Add("LIN6");
            lstLINNetwork.Items.Add("LIN7");
            lstLINNetwork.Items.Add("LIN8");

            lstCANFDNetwork.Items.Add("HSCAN");
            lstCANFDNetwork.Items.Add("MSCAN");
            lstCANFDNetwork.Items.Add("HSCAN2");
            lstCANFDNetwork.Items.Add("HSCAN3");
            lstCANFDNetwork.Items.Add("HSCAN4");
            lstCANFDNetwork.Items.Add("HSCAN5");
            lstCANFDNetwork.Items.Add("HSCAN6");
            lstCANFDNetwork.Items.Add("HSCAN7");
            lstCANFDNetwork.Items.Add("DWCAN9");
            lstCANFDNetwork.Items.Add("DWCAN10");
            lstCANFDNetwork.Items.Add("DWCAN11");
            lstCANFDNetwork.Items.Add("DWCAN12");
            lstCANFDNetwork.Items.Add("DWCAN13");
            lstCANFDNetwork.Items.Add("DWCAN14");
            lstCANFDNetwork.Items.Add("DWCAN15");
            lstCANFDNetwork.Items.Add("DWCAN16");

            cboEthernetNetworks.Items.Add("ETHERNET");
            cboEthernetNetworks.Items.Add("OP_ETHERNET1");
            cboEthernetNetworks.Items.Add("OP_ETHERNET2");
            cboEthernetNetworks.Items.Add("OP_ETHERNET3");
            cboEthernetNetworks.Items.Add("OP_ETHERNET4");
            cboEthernetNetworks.Items.Add("OP_ETHERNET5");
            cboEthernetNetworks.Items.Add("OP_ETHERNET6");
            cboEthernetNetworks.Items.Add("OP_ETHERNET7");
            cboEthernetNetworks.Items.Add("OP_ETHERNET8");
            cboEthernetNetworks.Items.Add("OP_ETHERNET9");
            cboEthernetNetworks.Items.Add("OP_ETHERNET10");
            cboEthernetNetworks.Items.Add("OP_ETHERNET11");
            cboEthernetNetworks.Items.Add("OP_ETHERNET12");

            lstNetwork.SelectedIndex = 1;
            lstLINNetwork.SelectedIndex = 0;
            lstOtherNetwork.SelectedIndex = 0;
            lstCANFDNetwork.SelectedIndex = 0;
            cboLINMessageType.SelectedIndex = 0;
            cboIDType.SelectedIndex = 0;
            lstNumberOfBytes.SelectedIndex = 8;
            lstISO15765Network.SelectedIndex = 1;
            lstNumberOfOtherBytes.SelectedIndex = 8;
            lstLINNumberOfBytes.SelectedIndex = 8;
            lstNumberOfCANFDBytes.SelectedIndex = 8;
            cboEthernetNetworks.SelectedIndex = 0;

            lstBaudRateToUse.Items.Add("20000");
            lstBaudRateToUse.Items.Add("33333");
            lstBaudRateToUse.Items.Add("50000");
            lstBaudRateToUse.Items.Add("62500");
            lstBaudRateToUse.Items.Add("83333");
            lstBaudRateToUse.Items.Add("100000");
            lstBaudRateToUse.Items.Add("125000");
            lstBaudRateToUse.Items.Add("250000");
            lstBaudRateToUse.Items.Add("500000");
            lstBaudRateToUse.Items.Add("800000");
            lstBaudRateToUse.Items.Add("1000000");
            lstBaudRateToUse.SelectedIndex = 0;

            lstFDBaudRateToUse.Items.Add("1000000");
            lstFDBaudRateToUse.Items.Add("2000000");
            lstFDBaudRateToUse.Items.Add("4000000");
            lstFDBaudRateToUse.Items.Add("5000000");
            lstFDBaudRateToUse.Items.Add("8000000");
            lstFDBaudRateToUse.Items.Add("10000000");
            lstFDBaudRateToUse.SelectedIndex = 0;

            lstNetworkBaudRate.Items.Add("HSCAN");
            lstNetworkBaudRate.Items.Add("MSCAN");
            lstNetworkBaudRate.Items.Add("SWCAN");
            lstNetworkBaudRate.Items.Add("LSFTCAN");
            lstNetworkBaudRate.Items.Add("HSCAN2");
            lstNetworkBaudRate.Items.Add("HSCAN3");
            lstNetworkBaudRate.Items.Add("HSCAN4");
            lstNetworkBaudRate.Items.Add("HSCAN5");
            lstNetworkBaudRate.Items.Add("HSCAN6");
            lstNetworkBaudRate.Items.Add("HSCAN7");
            lstNetworkBaudRate.Items.Add("DWCAN9");
            lstNetworkBaudRate.Items.Add("DWCAN10");
            lstNetworkBaudRate.Items.Add("DWCAN11");
            lstNetworkBaudRate.Items.Add("DWCAN12");
            lstNetworkBaudRate.Items.Add("DWCAN13");
            lstNetworkBaudRate.Items.Add("DWCAN14");
            lstNetworkBaudRate.Items.Add("DWCAN15");
            lstNetworkBaudRate.Items.Add("DWCAN16");
            lstNetworkBaudRate.SelectedIndex = 0;

            lstNetwork.SelectedIndex = 1;
            lstNumberOfBytes.SelectedIndex = 8;


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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (m_bPortOpen == true)
            {
                cmdReceive_Click(cmdReceive, null);
            }
        }

        private void cmdTransmit_Click(object sender, EventArgs e)
        {
            long lResult;
            icsSpyMessage stMessagesTx = new icsSpyMessage();
            long lNetworkID;
            string sTempString;


            // Has the uset open neoVI yet?;
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            // Read the Network we will transmit on (indicated by lstNetwork ListBox)
            sTempString = lstNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sTempString);

            // Is this a CAN network or a J1850/ISO one?
            // load the message structure

            if (chkExtendedID.Checked == true)
            {
                //Make id Extended
                stMessagesTx.StatusBitField = Convert.ToInt16(eDATA_STATUS_BITFIELD_1.SPY_STATUS_XTD_FRAME);
            }
            else
            {
                //Use Normal ID
                stMessagesTx.StatusBitField = 0;
            }
            stMessagesTx.ArbIDOrHeader = Convert.ToInt32(txtArbID.Text, 16);            // The ArbID
            stMessagesTx.NumberBytesData = Convert.ToByte(lstNumberOfBytes.SelectedIndex);         // The number of Data Bytes
            if (stMessagesTx.NumberBytesData > 8) stMessagesTx.NumberBytesData = 8; // You can only have 8 databytes with CAN
                                                                                    // Load all of the data bytes in the structure

            stMessagesTx.Data1 = Convert.ToByte(txtDataByte1.Text, 16);
            stMessagesTx.Data2 = Convert.ToByte(txtDataByte2.Text, 16);
            stMessagesTx.Data3 = Convert.ToByte(txtDataByte3.Text, 16);
            stMessagesTx.Data4 = Convert.ToByte(txtDataByte4.Text, 16);
            stMessagesTx.Data5 = Convert.ToByte(txtDataByte5.Text, 16);
            stMessagesTx.Data6 = Convert.ToByte(txtDataByte6.Text, 16);
            stMessagesTx.Data7 = Convert.ToByte(txtDataByte7.Text, 16);
            stMessagesTx.Data8 = Convert.ToByte(txtDataByte8.Text, 16);

            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTx, Convert.ToInt32(lNetworkID), 1);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }

        private void cboLINMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //0 = Commander  = ID and data section
            //1 = Header(only) = ID 
            //2 = Responder = Responder data

            //Show everything
            pnlDataP2.Visible = true;
            txtLINDataByte1.Visible = true;
            txtLINDataByte2.Visible = true;

            //Hide un-needed stuff
            switch (cboLINMessageType.SelectedIndex)
            {
                case 0:
                    // No need to hide anything
                    break;
                case 1:
                    //Header, no data bytes
                    pnlDataP2.Visible = false;
                    txtLINDataByte1.Visible = false;
                    txtLINDataByte2.Visible = false;
                    break;
                case 2:
                    //No need to hide anything
                    break;
                default:
                    //should be no way to get here.
                    break;
            }
        }


        private void SendCommanderFrame()
        {
            long lResult;
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            int lNetworkID;
            string sTempString;


            //This message type will send out the break, header, and Data.

            //Set message as Commander frame
            stJMsg.StatusBitField = Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_INIT_MESSAGE);

            // Set the network (LIN 1 in this case)
            sTempString = lstLINNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sTempString);

            // for all the header bytes
            if (cboIDType.SelectedIndex == 0)
            {
                //standard ID, need to convert!
                stJMsg.Header1 = GetProtectedIDFromID(Convert.ToByte(txtLINDataByteProtectedID.Text, 16));
            }
            else
            {
                //Protected ID, we can use it
                stJMsg.Header1 = Convert.ToByte(txtLINDataByteProtectedID.Text, 16); //The lin Protected ID goes into the First header bytes
            }

            //Copy the data
            stJMsg.Data1 = Convert.ToByte(txtLINDataByte3.Text, 16);
            stJMsg.Data2 = Convert.ToByte(txtLINDataByte4.Text, 16);
            stJMsg.Data3 = Convert.ToByte(txtLINDataByte5.Text, 16);
            stJMsg.Data4 = Convert.ToByte(txtLINDataByte6.Text, 16);
            stJMsg.Data5 = Convert.ToByte(txtLINDataByte7.Text, 16);
            stJMsg.Data6 = Convert.ToByte(txtLINDataByte8.Text, 16);

            switch (lstLINNumberOfBytes.SelectedIndex)
            {
                case 0:
                    //Make a header only type frame
                    stJMsg.NumberBytesHeader = 1;
                    stJMsg.NumberBytesData = 0;
                    break;
                case 1:
                    //Only 1 byte of data
                    stJMsg.NumberBytesHeader = 2;
                    stJMsg.Header2 = Convert.ToByte(txtLINDataByte1.Text, 16);
                    stJMsg.NumberBytesData = 0;
                    //checksum goes into header byte 3 for this instance
                    stJMsg.Header3 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    stJMsg.NumberBytesHeader = 3;
                    break;
                default:
                    //2 or more 
                    stJMsg.NumberBytesHeader = 3;
                    stJMsg.Header2 = Convert.ToByte(txtLINDataByte1.Text, 16);
                    stJMsg.Header3 = Convert.ToByte(txtLINDataByte2.Text, 16);
                    stJMsg.NumberBytesData = Convert.ToByte(lstLINNumberOfBytes.SelectedIndex - 2);

                    //header byte
                    if (lstLINNumberOfBytes.SelectedIndex == 2) stJMsg.Data1 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 3) stJMsg.Data2 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 4) stJMsg.Data3 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 5) stJMsg.Data4 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 6) stJMsg.Data5 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 7) stJMsg.Data6 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 8) stJMsg.Data7 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    stJMsg.NumberBytesData = Convert.ToByte(lstLINNumberOfBytes.SelectedIndex - 1);
                    break;
            }


            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stJMsg, lNetworkID, 1);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }

        private void SendHeaderOnlyFrame()
        {
            long lResult;
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            int lNetworkID;
            string sTempString;

            //This message type will send out the break and the header.  The data will come back from the Responder ECU (if connected)

            //Set message as Commander frame
            stJMsg.StatusBitField = Convert.ToInt32(eDATA_STATUS_BITFIELD_1.SPY_STATUS_INIT_MESSAGE);

            // Set the network (LIN 1 in this case)
            sTempString = lstLINNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sTempString);

            stJMsg.NumberBytesHeader = 1;
            // for all the header bytes
            if (cboIDType.SelectedIndex == 0)
            {
                //standard ID, need to convert!
                stJMsg.Header1 = GetProtectedIDFromID(Convert.ToByte(txtLINDataByteProtectedID.Text, 16));
            }
            else
            {
                //Protected ID, we can use it
                stJMsg.Header1 = Convert.ToByte(txtLINDataByteProtectedID.Text, 16); //The lin Protected ID goes into the First header bytes
            }

            //No data bytes for this type of message.  Also the Responder will have the checksum.
            stJMsg.NumberBytesData = 0;

            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stJMsg, lNetworkID, 1);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }

        private void SendResponderFrame()
        {
            long lResult;
            icsSpyMessageJ1850 stJMsg = new icsSpyMessageJ1850();
            int lNetworkID;
            string sTempString;

            //This message type will send out the break, header, and Data.

            //Set message as Save frame
            stJMsg.StatusBitField = 0;

            // Set the network (LIN 1 in this case)
            sTempString = lstLINNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sTempString);


            // for all the header bytes
            if (cboIDType.SelectedIndex == 0)
            {
                //standard ID, need to convert!
                stJMsg.Header1 = GetProtectedIDFromID(Convert.ToByte(txtLINDataByteProtectedID.Text, 16));
            }
            else
            {
                //Protected ID, we can use it
                stJMsg.Header1 = Convert.ToByte(txtLINDataByteProtectedID.Text, 16); //The lin Protected ID goes into the First header bytes
            }

            //Copy the data
            stJMsg.Data1 = Convert.ToByte(txtLINDataByte3.Text, 16);
            stJMsg.Data2 = Convert.ToByte(txtLINDataByte4.Text, 16);
            stJMsg.Data3 = Convert.ToByte(txtLINDataByte5.Text, 16);
            stJMsg.Data4 = Convert.ToByte(txtLINDataByte6.Text, 16);
            stJMsg.Data5 = Convert.ToByte(txtLINDataByte7.Text, 16);
            stJMsg.Data6 = Convert.ToByte(txtLINDataByte8.Text, 16);

            switch (lstLINNumberOfBytes.SelectedIndex)
            {
                case 0:
                    //Make a header only type frame
                    stJMsg.NumberBytesHeader = 1;
                    stJMsg.NumberBytesData = 0;
                    break;
                case 1:
                    //Only 1 byte of data
                    stJMsg.NumberBytesHeader = 2;
                    stJMsg.Header2 = Convert.ToByte(txtLINDataByte1.Text, 16);
                    stJMsg.NumberBytesData = 0;
                    //checksum goes into header byte 3 for this instance
                    stJMsg.Header3 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    stJMsg.NumberBytesHeader = 3;
                    break;
                default:
                    //2 or more 
                    stJMsg.NumberBytesHeader = 3;
                    stJMsg.Header2 = Convert.ToByte(txtLINDataByte1.Text, 16);
                    stJMsg.Header3 = Convert.ToByte(txtLINDataByte2.Text, 16);
                    stJMsg.NumberBytesData = Convert.ToByte(lstLINNumberOfBytes.SelectedIndex - 2);

                    //header byte
                    if (lstLINNumberOfBytes.SelectedIndex == 2) stJMsg.Data1 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 3) stJMsg.Data2 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 4) stJMsg.Data3 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 5) stJMsg.Data4 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 6) stJMsg.Data5 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 7) stJMsg.Data6 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    if (lstLINNumberOfBytes.SelectedIndex == 8) stJMsg.Data7 = CalculateLINCheckSum(chkEnhancedChecksum.Checked, ref stJMsg);
                    stJMsg.NumberBytesData = Convert.ToByte(lstLINNumberOfBytes.SelectedIndex - 1);
                    break;
            }


            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stJMsg, lNetworkID, 1);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }


        private byte CalculateLINCheckSum(bool UseEnhanced, ref icsSpyMessageJ1850 SpyMessageJ1850)
        {
            int ChecksumCalculation = 0;

            //Add a little error checking
            if (SpyMessageJ1850.NumberBytesHeader == 0 || SpyMessageJ1850.NumberBytesHeader == 1) return (0);

            //Get sum of all the bytes
            if (SpyMessageJ1850.NumberBytesHeader >= 2) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Header2;
            if (SpyMessageJ1850.NumberBytesHeader >= 3) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Header3;
            if (SpyMessageJ1850.NumberBytesData >= 1) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data1;
            if (SpyMessageJ1850.NumberBytesData >= 2) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data2;
            if (SpyMessageJ1850.NumberBytesData >= 3) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data3;
            if (SpyMessageJ1850.NumberBytesData >= 4) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data4;
            if (SpyMessageJ1850.NumberBytesData >= 5) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data5;
            if (SpyMessageJ1850.NumberBytesData >= 6) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Data6;

            //If enhanced add in the protected ID
            if (UseEnhanced == true) ChecksumCalculation = ChecksumCalculation + SpyMessageJ1850.Header1;

            //Get number of carries and add to LSB
            ChecksumCalculation = ((ChecksumCalculation / 0x100) + ChecksumCalculation) - (ChecksumCalculation & 0xff00);
            ChecksumCalculation = ((ChecksumCalculation / 0x100) + ChecksumCalculation) & 0xFF;

            //Invert to get checksum
            return Convert.ToByte(0xFF ^ ChecksumCalculation);
        }

        private byte GetProtectedIDFromID(byte ID)
        {
            bool[] bID = new bool[8];
            int iCntr;
            int iOutput;
            int iPower;


            //Get bit values
            for (iCntr = 0; iCntr <= 7; iCntr++)
            {
                iPower = Convert.ToInt32(Math.Pow(2, iCntr));
                bID[iCntr] = ((ID & iPower) != 0);
            }

            //Calculate ProtectedID
            bID[6] = bID[0] ^ bID[1] ^ bID[2] ^ bID[4];
            bID[7] = !(bID[1] ^ bID[3] ^ bID[4] ^ bID[5]);

            //Get byte value
            iOutput = 0;
            for (iCntr = 0; iCntr <= 7; iCntr++)
            {
                iOutput = iOutput + Convert.ToByte(bID[iCntr]) * Convert.ToInt32(Math.Pow(2, iCntr));
            }

            return Convert.ToByte(iOutput);
        }

        private void cmdOtherTransmit_Click(object sender, EventArgs e)
        {
            long lResult;
            icsSpyMessageJ1850 stMessagesTxJ1850 = new icsSpyMessageJ1850();
            long lNetworkID;
            string sTempString;

            // Has the uset open neoVI yet?;
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            // Read the Network we will transmit on (indicated by lstNetwork ListBox)
            sTempString = lstOtherNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sTempString);


            // load the message structure
            //copy headder bytes
            stMessagesTxJ1850.Header1 = Convert.ToByte(txtOtherPT.Text, 16);
            stMessagesTxJ1850.Header2 = Convert.ToByte(txtOtherTrgt.Text, 16);
            stMessagesTxJ1850.Header3 = Convert.ToByte(txtOtherSrc.Text, 16);
            stMessagesTxJ1850.NumberBytesHeader = 3;
            stMessagesTxJ1850.NumberBytesData = Convert.ToByte(lstNumberOfOtherBytes.SelectedIndex);         // The number of Data Bytes
                                                                                                             // Load all of the data bytes in the structure
            stMessagesTxJ1850.Data1 = Convert.ToByte(txtOtherData1.Text, 16);
            stMessagesTxJ1850.Data2 = Convert.ToByte(txtOtherData2.Text, 16);
            stMessagesTxJ1850.Data3 = Convert.ToByte(txtOtherData3.Text, 16);
            stMessagesTxJ1850.Data4 = Convert.ToByte(txtOtherData4.Text, 16);
            stMessagesTxJ1850.Data5 = Convert.ToByte(txtOtherData5.Text, 16);
            stMessagesTxJ1850.Data6 = Convert.ToByte(txtOtherData6.Text, 16);
            stMessagesTxJ1850.Data7 = Convert.ToByte(txtOtherData7.Text, 16);
            stMessagesTxJ1850.Data8 = Convert.ToByte(txtOtherData8.Text, 16);

            // Transmit the assembled message
            lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTxJ1850, Convert.ToInt32(lNetworkID), 1);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }

        private void cmdSetBitRate_Click(object sender, EventArgs e)
        {
            int iBitRateToUse = 0;
            int iNetworkID = 0;
            int iResult;

            //Get the network name index to set the baud rate of 
            switch (lstNetworkBaudRate.SelectedIndex)
            {
                case 0:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN;
                    break;
                case 1:
                    iNetworkID = (int)eNETWORK_ID.NETID_MSCAN;
                    break;
                case 2:
                    iNetworkID = (int)eNETWORK_ID.NETID_SWCAN;
                    break;
                case 3:
                    iNetworkID = (int)eNETWORK_ID.NETID_LSFTCAN;
                    break;
                case 4:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN2;
                    break;
                case 5:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN3;
                    break;
                case 6:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN4;
                    break;
                case 7:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN5;
                    break;
                case 8:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN6;
                    break;
                case 9:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN7;
                    break;
                case 10:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN7;
                    break;
                case 11:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_09;
                    break;
                case 12:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_10;
                    break;
                case 13:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_11;
                    break;
                case 14:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_12;
                    break;
                case 15:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_13;
                    break;
                case 16:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_14;
                    break;
                default:
                    MessageBox.Show("Incorrect Network selected");
                    return;
            }

            iBitRateToUse = Convert.ToInt32(lstBaudRateToUse.Text);

            //Set the bit rate
            iResult = icsNeoDll.icsneoSetBitRate(m_hObject, iBitRateToUse, iNetworkID);
            if (iResult != 1)
            {
                MessageBox.Show("Problem setting bit rate");
            }
            else
            {
                MessageBox.Show("Bit Rate Set");
            }
        }

        private void cmdSetFDBitRate_Click(object sender, EventArgs e)
        {
            int iBitRateToUse = 0;
            int iNetworkID = 0;
            int iResult;

            //Get the network name index to set the baud rate of 
            switch (lstNetworkBaudRate.SelectedIndex)
            {
                case 0:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN;
                    break;
                case 1:
                    iNetworkID = (int)eNETWORK_ID.NETID_MSCAN;
                    break;
                case 2:
                    iNetworkID = -1;
                    break;
                case 3:
                    iNetworkID = -1;
                    break;
                case 4:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN2;
                    break;
                case 5:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN3;
                    break;
                case 6:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN4;
                    break;
                case 7:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN5;
                    break;
                case 8:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN6;
                    break;
                case 9:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN7;
                    break;
                case 10:
                    iNetworkID = (int)eNETWORK_ID.NETID_HSCAN7;
                    break;
                case 11:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_09;
                    break;
                case 12:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_10;
                    break;
                case 13:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_11;
                    break;
                case 14:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_12;
                    break;
                case 15:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_13;
                    break;
                case 16:
                    iNetworkID = (int)eNETWORK_ID.NETID_DWCAN_14;
                    break;
                default:
                    MessageBox.Show("Incorrect Network selected");
                    return;
            }

            if (iNetworkID == -1)
            {
                MessageBox.Show("Not a Valid CAN FD Channel");
                return;
            }

            iBitRateToUse = Convert.ToInt32(lstFDBaudRateToUse.Text);

            //Set the bit rate
            iResult = icsNeoDll.icsneoSetFDBitRate(m_hObject, iBitRateToUse, iNetworkID);
            if (iResult != 1)
            {
                MessageBox.Show("Problem setting FD bit rate");
            }
            else
            {
                MessageBox.Show("FD Bit Rate Set");
            }
        }

        private void cmd3GGetSettings_Click(object sender, EventArgs e)
        {
            SVCAN3SettingsPack VcanReadSettings = new SVCAN3SettingsPack();
            SFireSettingsPack FireReadSettings = new SFireSettingsPack();
            SFire2SettingsPack Fire2ReadSettings = new SFire2SettingsPack();
            SRADGalaxySettingsPack RadGalaxyReadSettings = new SRADGalaxySettingsPack();
            SVCANRFSettingsPack VcanRFReadSettings = new SVCANRFSettingsPack();
            SVCAN412SettingsPack Vcan412ReadSettings = new SVCAN412SettingsPack();
            SRADPlutoSettingsPack RADPlutoSettings = new SRADPlutoSettingsPack();
            SVCAN4IndSettingsPack VCAN4IndSettings = new SVCAN4IndSettingsPack();
            SJupiterSettingsPack JupiterSettings = new SJupiterSettingsPack();
            SRED2SettingsPack RED2Settings = new SRED2SettingsPack();
            SFire3SettingsPack FIRE3Settings = new SFire3SettingsPack();
            SFire3FRSettingsPack FIRE3FlexSettings = new SFire3FRSettingsPack();

            CAN_SETTINGS HSCanSettings = new CAN_SETTINGS();
            int iNumberOfBytes = 0;
            int iResult;

            if (m_bPortOpen == false)  //Check to see if the port is opened first
            {
                MessageBox.Show("Device is Not Opened!");
                return;
            }


            //Get the settigns of the connected hardware
            switch (iOpenDeviceType)
            {
                case (int)eHardwareTypes.NEODEVICE_FIRE:          //FIRE
                                                                  //Get the settings
                    FireReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFireSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FireReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FireReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading FIRE configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FireReadSettings.FireSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN3:            //Vcan3
                                                                     //Get the setting
                    VcanReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN3SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VcanReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VcanReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCAN3 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VcanReadSettings.VCAN3Settings.Can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_FIRE2:            //FIRE2
                                                                     //Get the setting
                    Fire2ReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire2SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(Fire2ReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref Fire2ReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading FIRE2 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = Fire2ReadSettings.Fire2Settings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_RADGALAXY:            //RadGalaxy
                                                                         //Get the setting
                    RadGalaxyReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADGalaxySettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RadGalaxyReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RadGalaxyReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RADGalaxy configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RadGalaxyReadSettings.RADGalaxySettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCANRF:           //VcanRF
                                                                     //Get the setting
                    VcanRFReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCANRFSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VcanRFReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VcanRFReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCANRF configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VcanRFReadSettings.VCANRFSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN42:           //Vcan4-2
                case (int)eHardwareTypes.NEODEVICE_VCAN41:           //Vcan4-1  (Same steps for both)
                                                                     //Get the setting
                    Vcan412ReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN412SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(Vcan412ReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref Vcan412ReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCAN412 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = Vcan412ReadSettings.VCAN412Settings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_RADPLUTO:             //RAD Pluto
                                                                         //Get the setting
                    RADPlutoSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADPlutoSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RADPlutoSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RADPlutoSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RAD Pluto configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RADPlutoSettings.PlutoSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN4_IND:            //ValueCAN 4-Ind
                                                                         //Get the setting
                    VCAN4IndSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN4IndSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VCAN4IndSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VCAN4IndSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading ValueCAN4 Ind configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VCAN4IndSettings.VCAN4IndSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_RADJUPITER:           //RAD Jupiter
                                                                         //Get the setting
                    JupiterSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADJupiterSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(JupiterSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref JupiterSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RAD Jupiter configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = JupiterSettings.JupiterSettings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RED2:             //RAD Jupiter
                                                                     //Get the setting
                    RED2Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRed2SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RED2Settings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RED2Settings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RED 2 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RED2Settings.RED2Settings.can1;
                    break;


                case (int)eHardwareTypes.NEODEVICE_FIRE3:            //RAD Jupiter
                                                                     //Get the setting
                    FIRE3Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FIRE3Settings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FIRE3Settings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading Fire 3 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FIRE3Settings.Fire3Settings.can1;
                    break;


                case (int)eHardwareTypes.NEODEVICE_FIRE3_FLEXRAY:            //RAD Jupiter
                                                                             //Get the setting
                    FIRE3FlexSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3FlexraySettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FIRE3FlexSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FIRE3FlexSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading neoVI FIRE 3 Flexray configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FIRE3FlexSettings.Fire3FRSettings.can1;
                    break;

                default:
                    //Connected hardware does not support this command
                    MessageBox.Show("Problem reading configuration, unsupported device");
                    return;
            }

            //fill text boxes with data from sub struct
            txt3GPSeg1.Text = Convert.ToString(HSCanSettings.TqSeg1);
            txt3GPSeg2.Text = Convert.ToString(HSCanSettings.TqSeg2);
            txt3GPropDelay.Text = Convert.ToString(HSCanSettings.TqProp);
            txt3GSJumpW.Text = Convert.ToString(HSCanSettings.TqSync);
            txt3GBRP.Text = Convert.ToString(HSCanSettings.BRP);
        }

        private void cmd3GSetSettings_Click(object sender, EventArgs e)
        {
            SVCAN3SettingsPack VcanReadSettings = new SVCAN3SettingsPack();
            SFireSettingsPack FireReadSettings = new SFireSettingsPack();
            SFire2SettingsPack Fire2ReadSettings = new SFire2SettingsPack();
            SRADGalaxySettingsPack RadGalaxyReadSettings = new SRADGalaxySettingsPack();
            SVCANRFSettingsPack VcanRFReadSettings = new SVCANRFSettingsPack();
            SVCAN412SettingsPack Vcan412ReadSettings = new SVCAN412SettingsPack();
            SRADPlutoSettingsPack RADPlutoSettings = new SRADPlutoSettingsPack();
            SVCAN4IndSettingsPack VCAN4IndSettings = new SVCAN4IndSettingsPack();
            SJupiterSettingsPack JupiterSettings = new SJupiterSettingsPack();
            SRED2SettingsPack RED2Settings = new SRED2SettingsPack();
            SFire3SettingsPack FIRE3Settings = new SFire3SettingsPack();
            SFire3FRSettingsPack FIRE3FlexSettings = new SFire3FRSettingsPack();
            CAN_SETTINGS HSCanSettings = new CAN_SETTINGS();
            int iNumberOfBytes = 0;
            int iResult;

            if (m_bPortOpen == false)  //Check to see if the port is opened first
            {
                MessageBox.Show("Device is Not Opened!");
                return;
            }

            switch (iOpenDeviceType)
            {
                case (int)eHardwareTypes.NEODEVICE_FIRE:             //FIRE
                                                                     //Get the settings
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FireReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FireReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading FIRE configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FireReadSettings.FireSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN3:            //Vcan3
                                                                     //Get the setting
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VcanReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VcanReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCAN3 configuration");
                        return;
                    }

                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VcanReadSettings.VCAN3Settings.Can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_FIRE2:            //FIRE2
                                                                     //Get the setting
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(Fire2ReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref Fire2ReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading FIRE2 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = Fire2ReadSettings.Fire2Settings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_RADGALAXY:            //RadGalaxy
                                                                         //Get the setting
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RadGalaxyReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RadGalaxyReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RADGalaxy configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RadGalaxyReadSettings.RADGalaxySettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCANRF:           //VcanRF
                                                                     //Get the setting
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VcanRFReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VcanRFReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCANRF configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VcanRFReadSettings.VCANRFSettings.can1;
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN42:           //Vcan4-2
                case (int)eHardwareTypes.NEODEVICE_VCAN41:           //Vcan4-1 Both are the same
                                                                     //Get the setting
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(Vcan412ReadSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref Vcan412ReadSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading VCAN412 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = Vcan412ReadSettings.VCAN412Settings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADPLUTO:             //RAD Pluto
                                                                         //Get the setting
                    RADPlutoSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADPlutoSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RADPlutoSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RADPlutoSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RAD Pluto configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RADPlutoSettings.PlutoSettings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN4_IND:            //ValueCAN 4-Ind
                                                                         //Get the setting
                    VCAN4IndSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN4IndSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(VCAN4IndSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref VCAN4IndSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading ValueCAN4 Ind configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = VCAN4IndSettings.VCAN4IndSettings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADJUPITER:           //RAD Jupiter
                                                                         //Get the setting
                    JupiterSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADJupiterSettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(JupiterSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref JupiterSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading RAD Jupiter configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = JupiterSettings.JupiterSettings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_RED2:             //neoVI RED 2
                                                                     //Get the setting
                    RED2Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRed2SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(RED2Settings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref RED2Settings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading neoVI RED 2 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = RED2Settings.RED2Settings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3:            //neoVI FIRE 3
                                                                     //Get the setting
                    FIRE3Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3SettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FIRE3Settings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FIRE3Settings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading neoVI FIRE 3 configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FIRE3Settings.Fire3Settings.can1;
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3_FLEXRAY:            //neoVI Fire 3 Flexray
                                                                             //Get the setting
                    FIRE3FlexSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3FlexraySettingsType;
                    iNumberOfBytes = System.Runtime.InteropServices.Marshal.SizeOf(FIRE3FlexSettings);
                    iResult = icsNeoDll.icsneoGetDeviceSettings(m_hObject, ref FIRE3FlexSettings, iNumberOfBytes, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem reading neoVI FIRE 3 Flexray configuration");
                        return;
                    }
                    //Copy the HS CAN settings from the structure to sub struct
                    HSCanSettings = FIRE3FlexSettings.Fire3FRSettings.can1;
                    break;
                default:
                    //Connected hardware does not support this command
                    MessageBox.Show("Problem reading configuration, unsupported device");
                    return;
            }

            //fill text boxes with data from sub struct
            HSCanSettings.TqSeg1 = Convert.ToByte(txt3GPSeg1.Text);
            HSCanSettings.TqSeg2 = Convert.ToByte(txt3GPSeg2.Text);
            HSCanSettings.TqProp = Convert.ToByte(txt3GPropDelay.Text);
            HSCanSettings.TqSync = Convert.ToByte(txt3GSJumpW.Text);
            HSCanSettings.BRP = Convert.ToByte(txt3GBRP.Text);
            HSCanSettings.SetBaudrate = 1;

            //Check device to make sure correct structure goes to correct device
            switch (iOpenDeviceType)
            {
                case (int)eHardwareTypes.NEODEVICE_FIRE:          //FIRE
                                                                  //copy sub struct to main struct
                    FireReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFireSettingsType;
                    FireReadSettings.FireSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref FireReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending FIRE configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN3:            //Vcan3
                                                                     //copy sub struct to main struct
                    VcanReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN3SettingsType;
                    VcanReadSettings.VCAN3Settings.Can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref VcanReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending Vcan3 configuration");
                        return;
                    }
                    break;

                case (int)eHardwareTypes.NEODEVICE_FIRE2://FIRE2
                                                         //copy sub struct to main struct
                    Fire2ReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire2SettingsType;
                    Fire2ReadSettings.Fire2Settings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref Fire2ReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending FIRE2 configuration");
                        return;
                    }
                    break;

                case (int)eHardwareTypes.NEODEVICE_RADGALAXY://RAD Galaxy
                                                             //copy sub struct to main struct
                    RadGalaxyReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADGalaxySettingsType;
                    RadGalaxyReadSettings.RADGalaxySettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref RadGalaxyReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending RADGalaxy configuration");
                        return;
                    }
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCANRF://Vcanrf
                                                          //copy sub struct to main struct
                    VcanRFReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCANRFSettingsType;
                    VcanRFReadSettings.VCANRFSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref VcanRFReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending Vcanrf configuration");
                        return;
                    }
                    break;

                case (int)eHardwareTypes.NEODEVICE_VCAN42: //VCAN 4-2
                case (int)eHardwareTypes.NEODEVICE_VCAN41: //VCAN 4-1
                                                           //copy sub struct to main struct
                    Vcan412ReadSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN412SettingsType;
                    Vcan412ReadSettings.VCAN412Settings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref Vcan412ReadSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending VCAN 412 configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADPLUTO: //RAD Pluto
                                                             //copy sub struct to main struct
                    RADPlutoSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADPlutoSettingsType;
                    RADPlutoSettings.PlutoSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref RADPlutoSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending RAD Pluto configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_VCAN4_IND: //ValueCAN 4 Ind
                                                              //copy sub struct to main struct
                    VCAN4IndSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceVCAN4IndSettingsType;
                    VCAN4IndSettings.VCAN4IndSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref VCAN4IndSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending ValueCAN 4 Ind configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_RADJUPITER: //RAD Jupiter
                                                               //copy sub struct to main struct
                    JupiterSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRADJupiterSettingsType;
                    JupiterSettings.JupiterSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref JupiterSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending RAD Jupiter configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_RED2:             //neoVI RED 2
                                                                     //copy sub struct to main struct
                    RED2Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceRed2SettingsType;
                    RED2Settings.RED2Settings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref RED2Settings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending neoVI RED 2 configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3:            //neoVI FIRE 3
                                                                     //copy sub struct to main struct
                    FIRE3Settings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3SettingsType;
                    FIRE3Settings.Fire3Settings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref FIRE3Settings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending neoVI FIRE 3 configuration");
                        return;
                    }
                    break;
                case (int)eHardwareTypes.NEODEVICE_FIRE3_FLEXRAY:            //neoVI Fire 3 Flexray
                                                                             //copy sub struct to main struct
                    FIRE3FlexSettings.uiDevice = (UInt32)EDeviceSettingsType.DeviceFire3FlexraySettingsType;
                    FIRE3FlexSettings.Fire3FRSettings.can1 = HSCanSettings;
                    iResult = icsNeoDll.icsneoSetDeviceSettings(m_hObject, ref FIRE3FlexSettings, iNumberOfBytes, 1, 0);
                    if (iResult == 0)
                    {
                        MessageBox.Show("Problem Sending neoVI FIRE 3 configuration");
                        return;
                    }
                    break;
                default:
                    MessageBox.Show("Problem sending configuration, unsupported device");
                    return;
            }
            MessageBox.Show("Settings Changed");
        }

        private void cmdLINTransmit_Click(object sender, EventArgs e)
        {
            // Has the uset open neoVI yet?
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return;   // do not read messages if we haven't opened neoVI yet
            }

            //0 = Commander  = ID and data section
            //1 = Header(only) = ID 
            //2 = Responder = Responder data
            //Find out what type of messages to send

            switch (cboLINMessageType.SelectedIndex)
            {
                case 0:
                    // Commander Frame
                    SendCommanderFrame();
                    break;
                case 1:
                    //Header, no data bytes
                    SendHeaderOnlyFrame();
                    break;
                case 2:
                    //Responder Frame
                    SendResponderFrame();
                    break;
                default:
                    //Should never get here
                    break;
            }
        }

        private void cmdCANFDTransmit_Click(object sender, EventArgs e)
        {
            //Hardware must have CAN FD support for this tor work. 
            int lResult;
            icsSpyMessage stMessagesTx = new icsSpyMessage();
            int lNetworkID;
            uint iNumberTxed = 0;

            byte[] iDataBytes;
            int iCounter;

            //Read data from textbox to Byte arraylstNumberOfCANFDBytes
            String[] sSplitString = txtFDByteList.Text.Split(',');
            iDataBytes = new byte[sSplitString.GetUpperBound(0) + 1];


            for (iCounter = 0; iCounter < sSplitString.GetUpperBound(0) + 1; iCounter++)
            {
                iDataBytes[iCounter] = Convert.ToByte(sSplitString[iCounter], 16);
            }

            //Get pointer to data (could also be done with Unsafe code)
            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(iDataBytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr CanFDptr = gcHandle.AddrOfPinnedObject();

            //Send on HS CAN
            string sNetIDToUse = lstCANFDNetwork.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sNetIDToUse);

            //Set the ID
            stMessagesTx.ArbIDOrHeader = Convert.ToInt32(txtCANFDArbID.Text, 16);

            // The number of Data Bytes
            stMessagesTx.NumberBytesData = Convert.ToByte(lstNumberOfCANFDBytes.Text);
            stMessagesTx.NumberBytesHeader = 0;

            stMessagesTx.iExtraDataPtr = CanFDptr;
            stMessagesTx.Protocol = Convert.ToByte(CSnet.ePROTOCOL.SPY_PROTOCOL_CANFD);

            if (chkCANFDExtended.Checked == true)
            {
                //Make id Extended
                stMessagesTx.StatusBitField = Convert.ToInt16(eDATA_STATUS_BITFIELD_1.SPY_STATUS_XTD_FRAME);
            }
            else
            {
                //Use Normal ID
                stMessagesTx.StatusBitField = 0;
            }

            stMessagesTx.StatusBitField2 = 0;
            stMessagesTx.StatusBitField3 = 16; //Enable bitrate switch

            if (Convert.ToInt32(lstNumberOfCANFDBytes.Text) > 8)
            {
                //CAN FD More than 8 bytes
                //Enable Extra Data Pointer
                stMessagesTx.ExtraDataPtrEnabled = 1;
                lResult = icsNeoDll.icsneoTxMessagesEx(m_hObject, ref stMessagesTx, Convert.ToUInt32(lNetworkID), 1, ref iNumberTxed, 0);
            }
            else
            {
                //CAN FD 8 bytes or less
                stMessagesTx.ExtraDataPtrEnabled = 0;
                stMessagesTx.Data1 = Convert.ToByte(txtDataByte1.Text, 16);
                stMessagesTx.Data2 = Convert.ToByte(txtDataByte2.Text, 16);
                stMessagesTx.Data3 = Convert.ToByte(txtDataByte3.Text, 16);
                stMessagesTx.Data4 = Convert.ToByte(txtDataByte4.Text, 16);
                stMessagesTx.Data5 = Convert.ToByte(txtDataByte5.Text, 16);
                stMessagesTx.Data6 = Convert.ToByte(txtDataByte6.Text, 16);
                stMessagesTx.Data7 = Convert.ToByte(txtDataByte7.Text, 16);
                stMessagesTx.Data8 = Convert.ToByte(txtDataByte8.Text, 16);

                // Transmit the assembled message
                lResult = icsNeoDll.icsneoTxMessages(m_hObject, ref stMessagesTx, Convert.ToInt32(lNetworkID), 1);
            }


            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }

            gcHandle.Free();
        }

        private void cmdISO15765SendMessage_Click(object sender, EventArgs e)
        {
            int lResult;
            int iCounter;
            int iNetwork = 1;

            // Has the uset open neoVI yet?
            if (m_bPortOpen == false)  //Check to see if the port is opened first
            {
                MessageBox.Show("Device is Not Opened!");
                return;
            }

            //Create Message and adjust array to proper size
            stCM_ISO157652_TxMessage tx_msg = new stCM_ISO157652_TxMessage();
            tx_msg.data = new byte[4096];


            //enable ISO15765
            string sNetIDString = lstISO15765Network.Text;
            iNetwork = icsNeoDll.GetNetworkIDfromString(ref sNetIDString);
            lResult = icsNeoDll.icsneoISO15765_EnableNetworks(m_hObject, iNetwork);

            tx_msg.id = Convert.ToUInt32(txtISO15765ArbID.Text, 16);  //ArbID of the message
            tx_msg.vs_netid = Convert.ToUInt16(iNetwork);  //Network to use
            tx_msg.num_bytes = Convert.ToUInt32(nudISO15765NumberOfBytes.Value); //The number of data bytes to use
            tx_msg.padding = 0xAA; //Set Padding Byte
            tx_msg.flags = 0;



            int iFlags = 0;
            if (chkIS015765PaddingEnable.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.paddingEnable));
            if (chkISO15765ExtendedID.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.id_29_bit_enable));
            if (chkISO15765FCExtendedID.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.fc_id_29_bit_enable));
            if (chkISO15765CANFD.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.iscanFD));
            if (chkISO15765CANFD.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.isBSREnabled));
            tx_msg.flags = Convert.ToUInt16(iFlags);

            tx_msg.fc_id = Convert.ToUInt32(txtISO15765FlowControlArbID.Text, 16); //ArbID for the flow control Frame
            tx_msg.fc_id_mask = 0xFFF; //The flow control arb filter mask (response id from receiver)
            tx_msg.flowControlExtendedAddress = Convert.ToByte(chkISO15765ExtendedID.Checked); //Extended ID
            tx_msg.fs_timeout = 0x100;  //Flow Control Time out in ms
            tx_msg.fs_wait = 0x3000; //Flow Control FS=Wait Timeout ms
            tx_msg.blockSize = 0;//Block size (for sending flow Control)
            tx_msg.stMin = 0;

            //Fill data
            String[] SplitString = txtISO15765Data.Text.Split(',');
            for (iCounter = 0; iCounter < SplitString.GetUpperBound(0) + 1; iCounter++)
            {
                tx_msg.data[iCounter] = Convert.ToByte(SplitString[iCounter], 16);
            }

            lResult = icsNeoDll.icsneoISO15765_TransmitMessage(m_hObject, iNetwork, ref tx_msg, 3000);

            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }
        }

        private void cmdConFigISO15765Rx_Click(object sender, EventArgs e)
        {
            int lResult;
            String sTempNetwork;
            int iNetwork;
            stCM_ISO157652_RxMessage flow_cntrl_msg = new stCM_ISO157652_RxMessage();

            sTempNetwork = lstISO15765Network.Text;
            iNetwork = icsNeoDll.GetNetworkIDfromString(ref sTempNetwork);
            lResult = icsNeoDll.icsneoISO15765_EnableNetworks(m_hObject, iNetwork);

            //Build structure for message
            string sNetIDString = lstISO15765Network.Text;
            flow_cntrl_msg.vs_netid = Convert.ToUInt16(iNetwork);
            flow_cntrl_msg.padding = 0xAA; //Set Padding Byte
            flow_cntrl_msg.id = Convert.ToUInt32(txtISO15765ArbID.Text, 16);  //ArbID of the message
            flow_cntrl_msg.id_mask = 0xFFF; //The flow control arb filter mask (response id from receiver)
            flow_cntrl_msg.fc_id = Convert.ToUInt32(txtISO15765FlowControlArbID.Text, 16); //ArbID for the flow control Frame
            flow_cntrl_msg.blockSize = 100;
            flow_cntrl_msg.stMin = 100;
            flow_cntrl_msg.cf_timeout = 1000;

            //Set flags for Padding and ID information.
            flow_cntrl_msg.flags = 0;
            int iFlags = Convert.ToInt32(stCM_ISO157652_RxMessage_Flags.enableFlowControlTransmission);
            if (chkIS015765PaddingEnable.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.paddingEnable));
            if (chkISO15765ExtendedID.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.id_29_bit_enable));
            if (chkISO15765FCExtendedID.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.fc_id_29_bit_enable));
            if (chkISO15765CANFD.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.iscanFD));
            if (chkISO15765CANFD.Checked == true) iFlags = (iFlags | Convert.ToInt32(CSnet.stCM_ISO157652_TxMessage_Flags.isBSREnabled));
            flow_cntrl_msg.flags = Convert.ToUInt16(iFlags);

            //ulIndex set to 0 for setting up the first filter.
            lResult = icsNeoDll.icsneoISO15765_ReceiveMessage(m_hObject, 0, ref flow_cntrl_msg);
            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Setting up Message");
            }
        }

        private void cmdISO15765Disable_Click(object sender, EventArgs e)
        {
            int iResult;

            iResult = icsNeoDll.icsneoISO15765_DisableNetworks(m_hObject);
            // Test the returned result
            if (iResult != 1)
            {
                MessageBox.Show("Problem with disabling networks");
            }
        }

        private void cmdTxEthernet_Click(object sender, EventArgs e)
        {
            //Hardware must have ethernet support for this tor work. 
            int lResult;
            icsSpyMessage stMessagesTx = new icsSpyMessage();
            int lNetworkID;
            uint iNumberTxed = 0;

            byte[] iDataBytes;
            int iCounter;

            //Read data from textbox to Byte arraylstNumberOfCANFDBytes
            String[] sSplitString = txtEthernetData.Text.Split(',');
            iDataBytes = new byte[sSplitString.GetUpperBound(0) + 1];


            for (iCounter = 0; iCounter < sSplitString.GetUpperBound(0) + 1; iCounter++)
            {
                iDataBytes[iCounter] = Convert.ToByte(sSplitString[iCounter], 16);
            }

            //Get pointer to data (could also be done with Unsafe code)
            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(iDataBytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr Ethernetptr = gcHandle.AddrOfPinnedObject();

            //Send on Ethernet
            string sNetIDToUse = cboEthernetNetworks.Text;
            lNetworkID = icsNeoDll.GetNetworkIDfromString(ref sNetIDToUse);

            // The number of Data Bytes
            stMessagesTx.NumberBytesData = Convert.ToByte(Convert.ToInt32(nudEthernetNumberOfBytes.Value) & 0xFF);
            stMessagesTx.NumberBytesHeader = Convert.ToByte(Convert.ToInt32(nudEthernetNumberOfBytes.Value) / 0x100);
            stMessagesTx.NumberBytesData = Convert.ToByte(nudEthernetNumberOfBytes.Value);
            stMessagesTx.NumberBytesHeader = 0;

            stMessagesTx.iExtraDataPtr = Ethernetptr;
            stMessagesTx.Protocol = Convert.ToByte(CSnet.ePROTOCOL.SPY_PROTOCOL_ETHERNET);
            stMessagesTx.StatusBitField = 0;
            stMessagesTx.StatusBitField2 = 0;
            stMessagesTx.StatusBitField3 = 0;

            //Enable Extra Data Pointer
            stMessagesTx.ExtraDataPtrEnabled = 1;
            lResult = icsNeoDll.icsneoTxMessagesEx(m_hObject, ref stMessagesTx, Convert.ToUInt32(lNetworkID), 1, ref iNumberTxed, 0);

            // Test the returned result
            if (lResult != 1)
            {
                MessageBox.Show("Problem Transmitting Message");
            }

            gcHandle.Free();
        }

        private void cmdPhyRequest_Click(object sender, EventArgs e)
        {
            Int32 iResult;
            PhyRegPkt_t PhyRegPkt;
            int iTempFlags = 0;

            //Check for supported hardware
            if (iOpenDeviceType == (int)eHardwareTypes.NEODEVICE_RADSUPERMOON | iOpenDeviceType == (int)eHardwareTypes.NEODEVICE_RADMOON2 | iOpenDeviceType == (int)eHardwareTypes.NEODEVICE_RADPLUTO)
            {
                //Copy data to the structure

                PhyRegPkt.ClausePkt.phyAddrOrPort = Convert.ToByte(nudAddressOrPort.Value);
                PhyRegPkt.ClausePkt.pageOrDevice = Convert.ToByte(nudPhyPageOrDevice.Value);
                PhyRegPkt.ClausePkt.regAddr = Convert.ToUInt16(nudReqAddress.Value);
                PhyRegPkt.ClausePkt.regVal = Convert.ToUInt16(nudReqValue.Value);


                //Set the needed flags from the checkbox
                if (chkPhyEnabled.Checked == true) iTempFlags = iTempFlags | (int)PhyRegFlags.Enabled;     //Set Bitfield Enabled
                if (chkPhyWrite.Checked == true) iTempFlags = iTempFlags | (int)PhyRegFlags.WriteEnable;     //Set Bitfield WriteEnable = F,Clause45Enable 
                if (chkphyClause45Enable.Checked == true) iTempFlags = iTempFlags | (int)PhyRegFlags.Clause45Enable;     //Set Bitfield Clause45Enable 
                PhyRegPkt.Flags = (ushort)iTempFlags;

                //Get and cast the size of the structrue.
                iResult = icsNeoDll.icsneoReadWritePHYSettings(m_hObject, ref PhyRegPkt, (IntPtr)System.Runtime.InteropServices.Marshal.SizeOf(PhyRegPkt), (IntPtr)1);
                //If this is a read, dispaly the data
                if (chkPhyWrite.Checked == false)
                {
                    nudReqValue.Value = PhyRegPkt.ClausePkt.regVal;
                }
            }
            else
            {
                MessageBox.Show("Connected device not supported");
            }

        }

        private void cmdSetDoIPState_Click(object sender, EventArgs e)
        {
            Int32 lResult;

            // Has the uset open neoVI yet?;
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            //Call function to change the IO Pin
            lResult = icsNeoDll.icsneoEnableDOIPLine(m_hObject, chkDoIPState.Checked);

            //read Error state
            if (lResult != 1)
            {
                MessageBox.Show("Problem Setting DoIP Activation Line or hardware does not support this.");
            }
            else
            {
                MessageBox.Show("State Changed!");
            }
        }

        private void cmdGetGPTP_Click(object sender, EventArgs e)
        {
            Int32 iResult;
            GPTPStatus sgGPTPStatus = new GPTPStatus();
            double dTimeStamp;

            // Has the uset open neoVI yet?
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            //Call function 
            iResult = icsNeoDll.icsneoGetGPTPStatus(m_hObject, ref sgGPTPStatus);

            if (iResult == 0)
            {
                cmdGetGPTP.Text = "Call failed or not supported by hardware.";
            }
            else
            {
                //Calculate the time stamp
                dTimeStamp = ((Convert.ToUInt64(sgGPTPStatus.current_time.seconds_msb) << 32) + sgGPTPStatus.current_time.seconds_lsb) + (Convert.ToDouble(sgGPTPStatus.current_time.nanoseconds) / 1000000000);

                //Output the timestamp
                cmdGetGPTP.Text = Convert.ToString(dTimeStamp);
            }
        }


        private void cmdFirmwareCheck_Click_1(object sender, EventArgs e)
        {
            stAPIFirmwareInfo FirmwareInfo = new stAPIFirmwareInfo();
            int iResult;
            string sFWVerInfo;

            // Has the uset open neoVI yet?;
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            //Request DLL Firmware information.stAPIFirmwareInfo
            iResult = icsNeoDll.icsneoGetDLLFirmwareInfo(m_hObject, ref FirmwareInfo);
            if (iResult == 0)
            {
                MessageBox.Show("Problem getting the version of the firmware stored within the DLL API");
                return;
            }

            sFWVerInfo = "DLL FW Version = " + Convert.ToString(FirmwareInfo.iAppMajor) + "." + Convert.ToString(FirmwareInfo.iAppMinor) + "\r\n";

            //Request Hardware Firmware information.
            iResult = icsNeoDll.icsneoGetHWFirmwareInfo(m_hObject, ref FirmwareInfo);
            if (iResult == 0)
            {
                MessageBox.Show("Problem getting the neoVI's firmware information");
            }
            sFWVerInfo += "HW FW Version = " + Convert.ToString(FirmwareInfo.iAppMajor) + "." + Convert.ToString(FirmwareInfo.iAppMinor);

            txtFirmwareCheck.Text = sFWVerInfo;
        }

        private void cmdGetRTC_Click(object sender, EventArgs e)
        {
            Int32 lResult;
            icsSpyTime icsTime = new icsSpyTime();

            // Has the uset open neoVI yet?
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            //Call for RTC
            lResult = icsNeoDll.icsneoGetRTC(m_hObject, ref icsTime);

            //read Error state
            if (lResult != 1)
            {
                cmdGetRTC.Text = "Problem";
            }
            else
            {
                cmdGetRTC.Text = Convert.ToString(icsTime.hour) + ":" + Convert.ToString(icsTime.min) + ":" + Convert.ToString(icsTime.sec) + "\n" + Convert.ToString(icsTime.month) + "/" + Convert.ToString(icsTime.day) + "/" + Convert.ToString(icsTime.year);
            }
        }

        private void cmdSetRTC_Click(object sender, EventArgs e)
        {
            Int32 lResult;
            icsSpyTime icsTime = new icsSpyTime();
            DateTime dtDateTime = DateTime.Now;

            // Has the uset open neoVI yet?
            if (m_bPortOpen == false)
            {
                MessageBox.Show("neoVI not opened");
                return; // do not read messages if we haven't opened neoVI yet
            }

            icsTime.day = Convert.ToByte(dtDateTime.Day);
            icsTime.hour = Convert.ToByte(dtDateTime.Hour);
            icsTime.min = Convert.ToByte(dtDateTime.Minute);
            icsTime.month = Convert.ToByte(dtDateTime.Month);
            icsTime.sec = Convert.ToByte(dtDateTime.Second);
            icsTime.year = Convert.ToByte(dtDateTime.Year - 2000);

            //Call tos set RTC
            lResult = icsNeoDll.icsneoSetRTC(m_hObject, ref icsTime);

            //read Error state
            if (lResult != 1)
            {
                cmdSetRTC.Text = "Problem";
            }
            else
            {
                cmdSetRTC.Text = "RTC Set";
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
            /*
            //set mode to active
            //adi_wil_SetMode(m_hObject, 3);
            byte functionError = 0;
            byte function = 4; // adi_wil_SetMode ADI_WIL_API_SET_MODE = 4;
            byte[] parameters = new byte[512];

            parameters[0] = 3; //3 = active mode

            icsNeoDll.icsneoGenericAPISendCommand(m_hObject, 1, 0, function, Marshal.UnsafeAddrOfPinnedArrayElement(parameters, 0), 1, out functionError);
            */

            int iResult;

            byte uAPISelected, uInstanceSelected, uFunctionSelected, uFunctionError, uCurrentFunction, uNodeCount;
            byte[] pReturnedData = new byte[513];
            byte[] pACL = new byte[ADI_WIL_MAC_SIZE * ADI_WIL_MAX_NODES];
            uint uParametersLength, uReturnedDataLength;

            //test value to see if the acl gets changed
            pReturnedData[0] = 1;

            System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary shb = new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(pReturnedData);
            MessageBox.Show(shb.ToString());

            uAPISelected = 1;
            uInstanceSelected = 0;
            uFunctionSelected = ADI_WIL_API_GET_ACL;

            iResult = icsNeoDll.icsneoGenericAPISendCommand(m_hObject, uAPISelected, uInstanceSelected, uFunctionSelected, IntPtr.Zero, 0, out uFunctionError);

            if (iResult != 1 && uFunctionError != ADI_WIL_ERR_SUCCESS)
            {
                // Handle Error Here
                MessageBox.Show($"Send error {iResult}");
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
                uNodeCount = pReturnedData[512];

                if (ADI_WIL_MAX_NODES >= uNodeCount && uNodeCount >= 0)
                {
                    Buffer.BlockCopy(pReturnedData, 0, pACL, 0, uNodeCount * ADI_WIL_MAC_SIZE);
                }
                shb = new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(pACL);
                MessageBox.Show(shb.ToString());
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
