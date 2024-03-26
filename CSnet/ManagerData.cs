using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CSnet
{
    public class ManagerData
    {
        public string MacAddress { get; set; }
        public int Version { get; set; }
        public byte[] PMSPacket0 { get; set; } = new byte[0];
        public byte[] EMSPacket0 { get; set; } = new byte[0];
        public UInt16 I1 { get; set; }
        public UInt16 I2 { get; set; }
        public UInt16 VBAT { get; set; }
        public UInt16 AUX1 { get; set; } //BEV: FC+ Chassis, BETA: HVDC- Chassis, BETB: HVDC- Chassis
        public UInt16 AUX2 { get; set; } //BEV: FC- Chassis, BETA: Isolation switch monitoring, BETB:DCFC+ Chassis
        public UInt16 AUX3 { get; set; } //BEV: Shunt Thermistor, BETA: Shunt A temperature, BETB: DCFC differential
        public UInt16 AUX4 { get; set; } //BEV: DCFC relay Thermistor, BETA: SA1 temperature, BETB: DCFC- Chassis
        public UInt16 AUX5 { get; set; } //BEV: Main relay Thermistor, BETA: SA4 temperature, BETB: Isolation switch monitoring
        public UInt16 AUX6 { get; set; } //BEV: Vref external, BETA: SA3 temperature, BETB:Shunt B temperature
        public UInt16 AUX7 { get; set; } //BEV: 5V reference, BETA: input 12V sensing, BETB: SB1 temperature


        public UInt16 G1V { get; set; }
        public UInt16 G2V { get; set; }
        public UInt16 G3V { get; set; }
        public UInt16 G4V { get; set; }
        public UInt16 G5V { get; set; }
        public UInt16 G6V { get; set; }
        public UInt16 G7V { get; set; }

        public UInt16 C1V { get; set; }

        public UInt16 CD1V { get; set; }

        public string Timestamp { get; set; }
        public string PacketTimestamp { get; set; }

        public void UpdatePMSData(byte[] packet, uint packetID)
        {
            
            if (packetID == 0)
            {
                PMSPacket0 = packet;

                I1 = BitConverter.ToUInt16(packet, 11);
                I2 = BitConverter.ToUInt16(packet, 13);
                VBAT = BitConverter.ToUInt16(packet, 15);
                AUX1 = BitConverter.ToUInt16(packet, 19);
                AUX2 = BitConverter.ToUInt16(packet, 27);
                AUX3 = BitConverter.ToUInt16(packet, 40);
                AUX4 = BitConverter.ToUInt16(packet, 48);
                AUX5 = BitConverter.ToUInt16(packet, 61);
                AUX6 = BitConverter.ToUInt16(packet, 69);
            }
            else if (packetID == 2)
            {
                AUX7 = BitConverter.ToUInt16(packet, 69); //TODO: check if we can look in a different packet for AUX values? They are different sometimes
            }
        }

        public void UpdateEMSData(byte[] packet, uint packetID)
        {
            if (packetID == 0)
            {
                C1V = BitConverter.ToUInt16(packet, 6);
                G1V = BitConverter.ToUInt16(packet, 24);
                G2V = BitConverter.ToUInt16(packet, 26);
                G3V = BitConverter.ToUInt16(packet, 30);
                G4V = BitConverter.ToUInt16(packet, 32);
                G5V = BitConverter.ToUInt16(packet, 34);
                G6V = BitConverter.ToUInt16(packet, 38);
                G7V = BitConverter.ToUInt16(packet, 40);

                EMSPacket0 = packet;

                CD1V = BitConverter.ToUInt16(packet, 14);
            }
        }

        //BEV measurements
        private double BEVTemp(double a)
        {
            double b = a * 0.001;//convert to correct decimal places

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

        //BET A
        public double GetBETAHVCDMinus()
        {
            return -((double)AUX1 * 701 + 3);
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
            double b = AUX3 * 0.0001;//convert to correct decimal places

            //put voltage against graph to find thermistor temperature
            return (Math.Pow(b, 5) * -15.38) + (Math.Pow(b, 4) * 122.63) - (Math.Pow(b, 3) * 368.35) + (Math.Pow(b, 2) * 514.63) - b * 362.57 + 156.2;
        }
        public double GetBETASA1Temp()
        {
            return AUX4;
        }
        public double GetBETASA4Temp()
        {
            return AUX5;
        }
        public double GetBETASA3Temp()
        {
            return AUX6;
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
            return AUX6; //TODO: transfer function
        }
        public double GetBETBSB1Temp()
        {
            return AUX7; //TODO: transfer function
        }
    
        //EMS measurements
        private double NTC_COEF_A = 0.0026859917508292143;
        private double NTC_COEF_B = 0.0002879825290119797;
        private double NTC_COEF_C = 0.0000005413360154836909;
        public double EMSBDSBVoltage()
        {
            return CD1V * 0.0001;
        }
        public double EMSPressure1()
        {
            //Debug.WriteLine($"G1 {G1V}, G2 {G2V}, G3 {G3V}, G4 {G4V}, G5 {G5V}, G6 {G6V}, G7 {G7V}");
            return 10.0 + 250.0 * (G1V/ (double)C1V);
        }
        public double EMSPressure2()
        {
            return 10.0 + 250.0 * (G3V/ (double)G7V); //TODO: do we need to * 0.0001? To convert Voltage?
        }
        public double EMSTemperature1()
        {
            if (C1V != 0)
            {
                double NTCResistance = ((10.0) / (C1V / (double)G2V - 1.0));
                return (1.0 / (NTC_COEF_A + (NTC_COEF_B * Math.Log(NTCResistance)) + (NTC_COEF_C * Math.Pow(Math.Log(NTCResistance), 3)))) - 273.0;
            } else
            {
                return 0;
            }
        }
        public double EMSTemperature2()
        {
            if (G7V != 0)
            {
                double NTCResistance = ((10.0) / (G7V / (double)G4V - 1.0));
                return (1.0 / (NTC_COEF_A + (NTC_COEF_B * Math.Log(NTCResistance)) + (NTC_COEF_C * Math.Pow(Math.Log(NTCResistance), 3)))) - 273.0;
            } else
            {
                return 0;
            }
        }
        public double EMSGas1() 
        { 
            return (G5V / (double)C1V - 0.1) / 0.05;
        }
        public double EMSGas2()
        {
            return (G6V / (double)G7V - 0.1) / 0.05;
        }
        public double EMSReferenceVoltage1()
        {
            return C1V;
        }
        public double EMSReferenceVoltage2()
        {
            return G7V;
        }
    }
}
