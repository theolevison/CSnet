using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CSnet
{
    public abstract class UDPListener
    {
        public const byte BMSPACKET = 0;
        public const byte BMSTEMPPACKET = 1;
        public const byte PMSPACKET = 2;
        public const byte EMSPACKET = 3;
        public const byte ModuleVersion = 4;
        public const byte PackVersion = 5;
        public const byte DiagnosticVoltage = 6;
        public const byte Latency = 7;
        public const byte TotalPECErrors = 8;
        public const byte AverageUpdateRate = 9;
        public const byte PeakUpdateRate = 10;
        public const byte AverageRSSI = 11;
        public const byte PeakRSSI = 12;
        public const byte SetACL = 13;
        public const byte BEV = 14;
        public const byte BET = 15;
        public const byte OP90 = 16;

        protected ISocket server;
        protected EndPoint remoteEnd;
        protected Dictionary<string, DeviceModel> devices;

        public UDPListener(ISocket server, Dictionary<string, DeviceModel> devices)//TODO: MVC this, so that I don't have to pass in a UI element & then hunt through it
        {
            this.server = server;
            this.devices = devices;
        }

        public void Start()
        {
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
                    foreach(DeviceModel device in devices.Values)
                    {                        
                        //check serial number against request
                        if (device.serialNumber.Equals(Encoding.ASCII.GetString(data.Take(6).ToArray())))
                        {
                            server.Send(Action(data, device));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    Console.WriteLine($"发送失败：{ex.Message}");
                }
            }
        }

        public abstract byte[] Action(byte[] data, DeviceModel device);

        protected byte[] ReturnFF()
        {
            //request not recongised, send back FF
            return new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
        }
    }
}