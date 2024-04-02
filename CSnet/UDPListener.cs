using System;
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
        public const byte BEV = 13;
        public const byte BET = 14;
        public const byte OP90 = 15;

        protected Socket server;
        protected EndPoint remoteEnd;
        protected OpenDeviceTab uc1;

        private string IPAdd;
        private int Port;
        private TabControl TabControl1;

        public UDPListener(string IPAdd, int Port, TabControl TabControl1)
        {
            this.IPAdd = IPAdd;
            this.Port = Port;
            this.TabControl1 = TabControl1;
        }

        public void Start()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            server.Bind(new IPEndPoint(IPAddress.Parse(IPAdd), Port));

            remoteEnd = new IPEndPoint(IPAddress.Any, 0);
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
                        uc1 = (OpenDeviceTab)tabPage.Controls[0];

                        //check serial number against request
                        if (uc1.Name.Equals(Encoding.ASCII.GetString(data.Take(6).ToArray())))
                        {
                            Action(data);
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

        public abstract void Action(byte[] data);

        public void ReturnFF()
        {
            //request not recongised, send back FF
            server.SendTo(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, remoteEnd);
        }
    }
}