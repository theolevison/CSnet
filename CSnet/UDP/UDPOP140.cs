using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CSnet
{
    public class UDPOP140 : UDPListener
    {
        public UDPOP140(ISocket server, TabControl TabControl1) : base(server, TabControl1)
        { }

        public override void Action(byte[] data)
        {
            if (data[6] == BMSPACKET)
            {
                //send raw BMS packet to UDP client
                Thread.Sleep(6);
                if (uc1.modules.Length <= 12)//12个模组
                {
                    byte[] dataAll = new byte[240];

                    // uc1.modules[0].Packet0.Length

                    for (int i = 0; i < 12; i++)
                    {
                        Array.Copy(uc1.modules[i].Packet0, 6, dataAll, 20 * i, 20);
                    }

                    //将所有的电压合在一起
                    server.Send(dataAll);
                }
                else if (uc1.modules.Length >= 24)//24个模组{ //TODO: why is there a difference between 12 & 24?
                {
                    byte[] dataAll = new byte[480];

                    // uc1.modules[0].Packet0.Length
                    for (int i = 0; i < 24; i++)
                    {
                        Array.Copy(uc1.modules[i].Packet0, 6, dataAll, 20 * i, 20);
                    }

                    //将所有的电压合在一起
                    server.Send(dataAll);
                }
                else//没有数据
                {
                    ReturnFF();
                }
            }
            if (data[6] == BMSTEMPPACKET)
            {
                byte[] tempPacket = new byte[0];
                if (uc1.modules.Length <= 12)//12个模组
                {
                    for (int i = 0; i < 12; i++)
                    {
                        tempPacket = tempPacket.Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor1 * 100))
                        .Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor2 * 100)).ToArray();
                    }

                    server.Send(tempPacket);
                }
                else if (uc1.modules.Length >= 24)//24个模组 //TODO: why is this here? Why does it matter if there are 12 or 24 modules??
                {
                    for (int i = 0; i < 24; i++)
                    {
                        tempPacket = tempPacket.Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor1 * 100))
                        .Concat(BitConverter.GetBytes((Int16)uc1.modules[i].Thermistor2 * 100)).ToArray();
                    }

                    server.Send(tempPacket);
                }
                else//没有连接的情况
                {
                    ReturnFF();
                }
            }

            else
            {
                ReturnFF();
            }
        }
    }
}