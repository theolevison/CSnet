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
                    return device.SetupBEV() ? new byte[] { } : ReturnFF();
                case BET:
                    return device.SetupBET() ? new byte[] { } : ReturnFF();
                case OP90:
                    return device.SetupOP90() ? new byte[] {} : ReturnFF();
                case BETLower:
                    return device.SetupBETLower() ? new byte[] { } : ReturnFF();
                case BMSPACKET:
                    return device.modules[data[7]].GetCGV();
                case BMSTEMPPACKET:
                    {
                        //send temperature data to UDP client
                        byte[] tempPacket = new byte[8];

                        byte[] raw1 = Converter.DoubleToInt16Byte(device.modules[data[7]].Thermistor1);
                        byte[] raw2 = Converter.DoubleToInt16Byte(device.modules[data[7]].Thermistor2);

                        //将温度数组组装到返回数组里面
                        tempPacket[0] = device.modules[data[7]].Thermistor1Raw[0];
                        tempPacket[1] = device.modules[data[7]].Thermistor1Raw[1];
                        tempPacket[2] = device.modules[data[7]].Thermistor2Raw[0];
                        tempPacket[3] = device.modules[data[7]].Thermistor2Raw[1];
                        tempPacket[4] = raw1[0];
                        tempPacket[5] = raw1[1];
                        tempPacket[6] = raw2[0];
                        tempPacket[7] = raw2[1];

                        double temp1 = device.modules[data[7]].Thermistor1;
                        double temp2 = device.modules[data[7]].Thermistor2;

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
                    return device.GetPMS();
                case EMSPACKET:                    
                    return device.managers[0].GetEMS(); //TODO: make this int
                case ModuleVersion:
                    return BitConverter.GetBytes(device.modules[data[7]].version);
                case PackVersion:
                    return BitConverter.GetBytes(device.managers[data[7]].Version);
                case DiagnosticVoltage:
                        return device.modules[data[7]].GetCGDV();
                case Latency:
                    {
                        int data1 = device.modules[data[7]].Latency;
                        return BitConverter.GetBytes(data1);
                    }
                case TotalPECErrors:
                    {
                        int data1 = device.modules[data[7]].TotalPECErrors;
                        return BitConverter.GetBytes(data1);
                    }
                case AverageUpdateRate:                    
                        return Converter.DoubleToInt16Byte(device.modules[data[7]].AverageUpdateRate);
                case PeakUpdateRate:                    
                        return Converter.DoubleToInt16Byte(device.modules[data[7]].PeakUpdateRate);
                case AverageRSSI:
                    {
                        int data1 = device.modules[data[7]].AverageRSSI;
                        return BitConverter.GetBytes(data1);
                    }
                case PeakRSSI:
                    {
                        int data1 = device.modules[data[7]].PeakRSSI;
                        return BitConverter.GetBytes(data1);
                    }
                case SetACL:
                    device.SetMESACL();
                    return new byte[0];
                case Timings:
                    return device.GetTimings();
                case VoltAndCurrent:
                    //protected const byte VoltAndCurrent = 19;
                    //protected const byte HVDC = 20;
                    return device.GetTimings();
                case HVDC:
                    return device.GetTimings();
                default:
                    return ReturnFF();
            }
        }
    }
}