﻿using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSnet
{
    public class ManagerData
    {
        public string MacAddress { get; set; }
        public int Version { get; set; }
        public byte[] PMSPacket0 { get; set; } = new byte[0];
        public byte[] EMSPacket0 { get; set; } = new byte[0];
        public double I1 { get; set; }
        public double I2 { get; set; }
        public double VBAT { get; set; }
        public double AUX1 { get; set; } //BEV: FC+ Chassis, BETA: HVDC- Chassis, BETB: HVDC- Chassis
        public double AUX2 { get; set; } //BEV: FC- Chassis, BETA: Isolation switch monitoring, BETB:DCFC+ Chassis
        public double AUX3 { get; set; } //BEV: Shunt Thermistor, BETA: Shunt A temperature, BETB: DCFC differential
        public double AUX4 { get; set; } //BEV: DCFC relay Thermistor, BETA: SA1 temperature, BETB: DCFC- Chassis
        public double AUX5 { get; set; } //BEV: Main relay Thermistor, BETA: SA4 temperature, BETB: Isolation switch monitoring
        public double AUX6 { get; set; } //BEV: Vref external, BETA: SA3 temperature, BETB:Shunt B temperature
        public double AUX7 { get; set; } //BEV: 5V reference, BETA: input 12V sensing, BETB: SB1 temperature


        public double G1V { get; set; }
        public double G2V { get; set; }
        public double G3V { get; set; }
        public double G4V { get; set; }
        public double G5V { get; set; }
        public double G6V { get; set; }
        public double G7V { get; set; }
               
        public double C1V { get; set; }
               
        public double CD1V { get; set; }

        public string Timestamp { get; set; }
        public string PacketTimestamp { get; set; }

        public void UpdatePMSData(byte[] packet, uint packetID)
        {
            if (packetID == 0)
            {
                PMSPacket0 = packet;

                I1 = BitConverter.ToUInt16(packet, 11) * 0.00000760371;
                I2 = BitConverter.ToUInt16(packet, 13) * 0.00000760371;
                VBAT = BitConverter.ToUInt16(packet, 15) * 0.000375183;
                AUX1 = BitConverter.ToUInt16(packet, 19) * 0.000375183;
                AUX2 = BitConverter.ToUInt16(packet, 27) * 0.000375183;
                AUX3 = BitConverter.ToUInt16(packet, 40) * 0.000375183;
                AUX4 = BitConverter.ToUInt16(packet, 48) * 0.000375183;
                AUX5 = BitConverter.ToUInt16(packet, 61) * 0.000375183;
                AUX6 = BitConverter.ToUInt16(packet, 69) * 0.000375183;
            }
            else if (packetID == 2)
            {
                AUX7 = BitConverter.ToUInt16(packet, 69) * 0.000375183; //TODO: check if we can look in a different packet for AUX values? They are different sometimes
            }
        }

        public void UpdateEMSData(byte[] packet, uint packetID)
        {
            if (packetID == 0)
            {
                C1V = BitConverter.ToUInt16(packet, 6) * 0.0001;
                G1V = BitConverter.ToUInt16(packet, 24) * 0.0001;
                G2V = BitConverter.ToUInt16(packet, 26) * 0.0001;
                G3V = BitConverter.ToUInt16(packet, 30) * 0.0001;
                G4V = BitConverter.ToUInt16(packet, 32) * 0.0001;
                G5V = BitConverter.ToUInt16(packet, 34) * 0.0001;
                G6V = BitConverter.ToUInt16(packet, 38) * 0.0001;
                G7V = BitConverter.ToUInt16(packet, 40) * 0.0001;

                EMSPacket0 = packet;

                CD1V = BitConverter.ToUInt16(packet, 14) * 0.0002;
            }
        }

        //BEV measurements
        private double BEVTemp(double a)
        {
            double b = a * 0.001;//convert to correct decimal places TODO: check this is correct

            //put voltage against graph to find thermistor temperature
            return (Math.Pow(b, 3) * -4.0721) + (Math.Pow(b, 2) * 32.822) - b * 104.43 + 179.58;
        }
        public double GetBEVDCFCPlus()
        {
            //Debug.WriteLine($"aux1 {AUX1}, aux2 {AUX2}, aux3 {AUX3}, aux4 {AUX4}, aux5 {AUX5}, aux6 {AUX6}, aux7 {AUX7}");
            return AUX1 * 284.006; //TODO: confirm this is AUX 1, not AUX 0. Check VREF (AUX6) which should always be 3V
        }
        public double GetBEVDCFCMinus()
        {
            return -(AUX2 * 248.006);
        }
        public double GetBEVShuntTemp()
        {
            return BEVTemp(AUX3);
        }
        public double GetBEVDCFCContactorTemp()
        {
            return BEVTemp(AUX4);
        }
        public double GetBEVMainContactorTemp()
        {
            return BEVTemp(AUX5);
        }
        public double GetBEVVREF()
        {
            return AUX6;
        }        
        public byte[] GetBEVPMS()
        {            
            return BitConverter.GetBytes(I1)
                            .Concat(BitConverter.GetBytes(I2))
                            .Concat(BitConverter.GetBytes(VBAT))
                            .Concat(BitConverter.GetBytes(GetBEVDCFCPlus()))
                            .Concat(BitConverter.GetBytes(GetBEVDCFCMinus()))
                            .Concat(BitConverter.GetBytes(GetBEVShuntTemp()))
                            .Concat(BitConverter.GetBytes(GetBEVDCFCContactorTemp()))
                            .Concat(BitConverter.GetBytes(GetBEVMainContactorTemp()))
                            .Concat(BitConverter.GetBytes(GetBEVVREF()))
                            .ToArray();
        }
        
        //BET A
        public double GetBETAHVDCMinus()
        {
            return -(AUX1 * 701 + 3);
        }
        public double GetBETAIsolationSwitch()//not used in test
        {
            if (AUX2 < 25.51)
            {
                //Switch open
            } else if (AUX2 > 46.52)
            {
                //Switch closed
            }
            return 0;
        }
        public double GetBETAShuntTemperature()
        {
            double b = AUX3 * 0.0001;//convert to correct decimal places TODO: check this is correct to do

            //put voltage against graph to find thermistor temperature
            return ShuntNTCTransferFunction(b);
        }
        public double ShuntNTCTransferFunction(double x)
        {
            return (Math.Pow(x, 5) * -15.38) + (Math.Pow(x, 4) * 122.63) + (Math.Pow(x, 3) * -368.35) + (Math.Pow(x, 2) * 514.63) + (x * -362.57) + 156.2;
        }
        public double GetBETASA1Temp()
        {
            return LittleFuseTransferFunction(AUX4);
        }
        public double GetBETASA4Temp()
        {
            return LittleFuseTransferFunction(AUX5);
        }
        public double GetBETASA3Temp()
        {
            return LittleFuseTransferFunction(AUX6);
        }
        private double LittleFuseTransferFunction(double x)
        {
            return (Math.Pow(x, 6) * 8.2901) + (Math.Pow(x, 5) * -88.588) + (Math.Pow(x, 4) * 365.45) + (Math.Pow(x, 3) * -741.33) + (Math.Pow(x, 2) * 778.02) + (x * -436.16) + 162.46;
        }

        public byte[] GetBETAPMS()
        {
            return BitConverter.GetBytes(GetBETAHVDCMinus()) //Div says, there is no HVDC plus
                .Concat(BitConverter.GetBytes(GetBETAIsolationSwitch()))
                .Concat(BitConverter.GetBytes(GetBETAShuntTemperature()))
                .Concat(BitConverter.GetBytes(GetBETASA1Temp()))
                .Concat(BitConverter.GetBytes(GetBETASA3Temp()))
                .Concat(BitConverter.GetBytes(GetBETASA4Temp()))
                .ToArray();
        }


        //BET B
        public double GetBETBHVDCMinus()
        {
            return -(AUX1 * 701 +3);
        }
        public double GetBETBDCFCPlus()
        {
            return AUX2 * 584.489 + 1.2;
        }
        public double GetBETBDCFCMinus()
        {
            return -(AUX4 * 584.489 + 1.2);
        }
        public double GetBETBDCFCDifferential()
        {
            return AUX3;//TODO: add transfer function
        }
        public double GetBETBIsolationSwitch()
        {
            if (AUX5 < 25.51)
            {
                //Switch is open
            } else if (AUX5 > 46.52)
            {
                //Switch is closed
            }
            return 0;//TODO: make this actually work
        }
        public double GetBETBShuntTemp()
        {
            return ShuntNTCTransferFunction(AUX6); //TODO: transfer function
        }
        public double GetBETBSB1Temp()
        {
            return LittleFuseTransferFunction(AUX7); //TODO: transfer function
        }

        public byte[] GetBETBPMS()
        {
            return BitConverter.GetBytes(GetBETBHVDCMinus())
                .Concat(BitConverter.GetBytes(GetBETBDCFCPlus()))
                .Concat(BitConverter.GetBytes(GetBETBDCFCMinus()))
                .Concat(BitConverter.GetBytes(GetBETBDCFCDifferential()))
                .Concat(BitConverter.GetBytes(GetBETBIsolationSwitch()))
                .Concat(BitConverter.GetBytes(GetBETBShuntTemp()))
                .Concat(BitConverter.GetBytes(GetBETBSB1Temp()))
                .ToArray();
        }


        //EMS measurements
        private double NTC_COEF_A = 0.0026859917508292143;
        private double NTC_COEF_B = 0.0002879825290119797;
        private double NTC_COEF_C = 0.0000005413360154836909;
        public double EMSBDSBVoltage()
        {
            return CD1V;
        }
        public double EMSPressure1()
        {
            //Debug.WriteLine($"G1 {G1V}, G2 {G2V}, G3 {G3V}, G4 {G4V}, G5 {G5V}, G6 {G6V}, G7 {G7V}");
            return 10.0 + 250.0 * (G1V/ C1V);
        }
        public double EMSPressure2()
        {
            return 10.0 + 250.0 * (G3V/ G7V);
        }
        private double EMSThermistorTransferFunction(double thermistorVoltage, double referenceVoltage)
        {
            if (thermistorVoltage != 0)
            {
                double NTCResistance = ((10.0) / (thermistorVoltage / referenceVoltage - 1.0));
                return (1.0 / (NTC_COEF_A + (NTC_COEF_B * Math.Log(NTCResistance)) + (NTC_COEF_C * Math.Pow(Math.Log(NTCResistance), 3)))) - 273.0;
            } else
            {
                return 0;
            }
        }
        public double EMSTemperature1()
        {
            return EMSThermistorTransferFunction(C1V, G2V);
        }
        public double EMSTemperature2()
        {
            return EMSThermistorTransferFunction(G7V, G4V);
        }
        public double EMSGas1() 
        {
            return EMSGasTransferFunction(G5V, C1V);
        }
        public double EMSGas2()
        {
            return EMSGasTransferFunction(G6V, G7V);
        }
        private double EMSGasTransferFunction(double sensorVoltage, double referenceVoltage)
        {
            double temp = (sensorVoltage / referenceVoltage - 0.1) / 0.05; ;
            return (sensorVoltage / referenceVoltage - 0.1) / 0.05;
        }
        public double EMSReferenceVoltage1()
        {
            return C1V;
        }
        public double EMSReferenceVoltage2()
        {
            return G7V;
        }

        public byte[] GetEMS()
        {
            return BitConverter.GetBytes(CD1V)
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSReferenceVoltage1())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSReferenceVoltage2())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSTemperature1())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSTemperature2())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSPressure1())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSPressure2())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSGas1())))
                     .Concat(BitConverter.GetBytes(uconst.DoubleToInt(EMSGas2())))
                     .ToArray();
        }
    }
}
