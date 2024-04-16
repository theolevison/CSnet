﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using CSnet.UDP;

namespace CSnet
{
    public abstract class UDPListener : IUDPListener
    {
        protected const byte BMSPACKET = 0;
        protected const byte BMSTEMPPACKET = 1;

        protected ISocket server;
        protected Dictionary<string, DeviceModel> devices;

        public UDPListener(ISocket server, Dictionary<string, DeviceModel> devices)
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
                    server.ReceiveFrom(data);

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
                     * 
                     * Set settings, then set ACL. Then you can request whatever data you need                     * 
                     * 
                     * Turn Serial number from ascii -> byte
                     * It's case sensitive
                     * 
                     * e.g BS0232 00 01 = 4253303233320001
                     */

                    //iterate through each open device
                    foreach (DeviceModel device in devices.Values)
                    {
                        //check serial number against request
                        string tmy = (Encoding.ASCII.GetString(data.Take(6).ToArray()));
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