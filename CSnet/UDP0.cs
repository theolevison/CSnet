using System;
using System.Linq;
using System.Windows.Forms;

namespace CSnet
{
    public class UDP0 : UDPListener
    {
        public UDP0(string IPAdd, int Port, TabControl TabControl1) : base(IPAdd, Port, TabControl1)
        { }

        public override void Action(byte[] data)
        {
            switch (data[6])
            {
                case BEV:
                    uc1.Setup(false, true);
                    break;
                case BET:
                    uc1.Setup(true, true);
                    break;
                case OP90:
                    uc1.Setup(false, false);
                    break;
                case BMSPACKET:
                    //send raw BMS packet to UDP client
                    server.SendTo(uc1.modules[data[2]].Packet0, remoteEnd);
                    break;
                case BMSTEMPPACKET:
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
                        break;
                    }
                case PMSPACKET:
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
                        break;
                    }
                case EMSPACKET:
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
                        break;
                    }
                case ModuleVersion:
                    //send raw BMS packet to UDP client
                    byte[] modulesVersion = new byte[1];
                    modulesVersion[0] = (byte)uc1.modules[data[2]].version;

                    server.SendTo(modulesVersion, remoteEnd);
                    break;
                case PackVersion:
                    byte[] packVersion = new byte[1];
                    //firmwareLabel.Text
                    // string Addr1 = MyINI.GetIniKeyValueForStr("PACK", "version", Application.StartupPath + "\\config.ini");

                    packVersion[0] = byte.Parse(uconst.packVersion);
                    server.SendTo(packVersion, remoteEnd);
                    break;
                case DiagnosticVoltage:
                    {
                        int[] data1 = uconst.DoubleArrayToIntArray(uc1.modules[data[2]].CGDV);
                        byte[] CGDV1 = uconst.IntArrayToByteArray(data1);
                        server.SendTo(CGDV1, remoteEnd);
                        break;
                    }
                case Latency:
                    {
                        int data1 = uc1.modules[data[2]].Latency;
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                case TotalPECErrors:
                    {
                        int data1 = uc1.modules[data[2]].TotalPECErrors;
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                case AverageUpdateRate:
                    {
                        int data1 = uconst.DoubleToInt(uc1.modules[data[2]].AverageUpdateRate * 1000 * 100);
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                case PeakUpdateRate:
                    {
                        int data1 = uconst.DoubleToInt(uc1.modules[data[2]].PeakUpdateRate * 1000 * 100);
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                case AverageRSSI:
                    {
                        int data1 = uc1.modules[data[2]].AverageRSSI;
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                case PeakRSSI:
                    {
                        int data1 = uc1.modules[data[2]].PeakRSSI;
                        byte[] latency = BitConverter.GetBytes(data1);
                        server.SendTo(latency, remoteEnd);
                        break;
                    }
                default:
                    {
                        ReturnFF();
                        break;
                    }
            }
        }
    }
}