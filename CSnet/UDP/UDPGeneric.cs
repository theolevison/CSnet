using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CSnet
{
    public class UDPGeneric : UDPListener
    {
        public UDPGeneric(ISocket server, Dictionary<string, DeviceModel> devices) : base(server, devices)
        { }

        public override byte[] Action(byte[] data, DeviceModel device)
        {
            switch (data[6])
            {
                case BEV:
                    return device.Setup(false, true) ? new byte[] { } : ReturnFF();
                case BET:
                    return device.Setup(true, true) ? new byte[] { } : ReturnFF();
                case OP90:
                    return device.Setup(false, false) ? new byte[] {} : ReturnFF();
                case BMSPACKET:
                    //send raw BMS packet to UDP client
                    return device.modules[data[2]].Packet0;
                case BMSTEMPPACKET:
                    {
                        //send temperature data to UDP client
                        byte[] tempPacket = new byte[8];

                        byte[] raw1 = BitConverter.GetBytes(device.modules[data[2]].Thermistor1);
                        byte[] raw2 = BitConverter.GetBytes(device.modules[data[2]].Thermistor2);

                        //将温度数组组装到返回数组里面
                        tempPacket[0] = device.modules[data[2]].Thermistor1Raw[0];
                        tempPacket[1] = device.modules[data[2]].Thermistor1Raw[1];
                        tempPacket[2] = device.modules[data[2]].Thermistor2Raw[0];
                        tempPacket[3] = device.modules[data[2]].Thermistor2Raw[1];
                        tempPacket[4] = raw1[0];
                        tempPacket[5] = raw1[1];
                        tempPacket[6] = raw2[0];
                        tempPacket[7] = raw2[1];

                        double temp1 = device.modules[data[2]].Thermistor1;
                        double temp2 = device.modules[data[2]].Thermistor2;

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

                        return tempPacket;
                    }
                case PMSPACKET:
                    {
                        //for example:
                        device.managers[0].GetBEVDCFCContactorTemp();

                        byte[] tempPacket = BitConverter.GetBytes(device.managers[0].I1)
                            .Concat(BitConverter.GetBytes(device.managers[0].I2))
                            .Concat(BitConverter.GetBytes(device.managers[0].VBAT))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX1))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX2))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX3))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX4))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX5))
                            .Concat(BitConverter.GetBytes(device.managers[0].AUX6))
                            .ToArray();

                        return tempPacket;
                    }
                case EMSPACKET:
                    {
                        byte[] tempPacket = BitConverter.GetBytes(device.managers[0].CD1V)
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSReferenceVoltage1())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSReferenceVoltage2())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSTemperature1())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSTemperature2())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSPressure1())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSPressure2())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSGas1())))
                      .Concat(BitConverter.GetBytes(uconst.DoubleToInt(device.managers[0].EMSGas2())))

                      //.Concat(BitConverter.GetBytes(uc1.managers[0].GetBEVDCFCMinus()))
                      .ToArray();

                       return tempPacket;
                    }
                case ModuleVersion:
                    //send raw BMS packet to UDP client
                    byte[] moduleVersion = new byte[1];
                    moduleVersion[0] = (byte)device.modules[data[2]].version;

                    return moduleVersion;
                case PackVersion:
                    byte[] packVersion = new byte[1];
                    //firmwareLabel.Text
                    // string Addr1 = MyINI.GetIniKeyValueForStr("PACK", "version", Application.StartupPath + "\\config.ini");

                    packVersion[0] = byte.Parse(uconst.packVersion);
                    return packVersion;
                case DiagnosticVoltage:
                    {
                        int[] data1 = uconst.DoubleArrayToIntArray(device.modules[data[2]].CGDV);
                        return uconst.IntArrayToByteArray(data1);
                    }
                case Latency:
                    {
                        int data1 = device.modules[data[2]].Latency;
                        return BitConverter.GetBytes(data1);
                    }
                case TotalPECErrors:
                    {
                        int data1 = device.modules[data[2]].TotalPECErrors;
                        return BitConverter.GetBytes(data1);
                    }
                case AverageUpdateRate:
                    {
                        int data1 = uconst.DoubleToInt(device.modules[data[2]].AverageUpdateRate * 1000 * 100);
                        return BitConverter.GetBytes(data1);
                    }
                case PeakUpdateRate:
                    {
                        int data1 = uconst.DoubleToInt(device.modules[data[2]].PeakUpdateRate * 1000 * 100);
                        return BitConverter.GetBytes(data1);
                    }
                case AverageRSSI:
                    {
                        int data1 = device.modules[data[2]].AverageRSSI;
                        return BitConverter.GetBytes(data1);
                    }
                case PeakRSSI:
                    {
                        int data1 = device.modules[data[2]].PeakRSSI;
                        return BitConverter.GetBytes(data1);
                    }
                case SetACL:
                    device.SetACL();
                    return new byte[0];
                default:
                    {
                        return ReturnFF();
                    }
            }
        }
    }
}