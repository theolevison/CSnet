using System;
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
        public Int16 Version { get; set; }
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
        public double AUX8 { get; set; } //BEV: VREF2, BETA: VREF2, BETB: VREF2
        public double AUX9 { get; set; } //BEV: Dummy measurement, BETA: VREFA, BETB: VREFB
        public double AUX10 { get; set; } //BEV: Dummy measurement, BETA: Balance measure, BETB: Fast Charge CH
        public double AUX11 { get; set; } //BEV: Dummy measurement, BETA: Balance measure, BETB: Fast Charge FC


        public double G1V { get; set; }
        public double G2V { get; set; }
        public double G3V { get; set; }
        public double G4V { get; set; }
        public double G5V { get; set; }
        public double G6V { get; set; }
        public double G7V { get; set; }
               
        public double C1V { get; set; } //Reference voltage
               
        public double CD1V { get; set; } //BDSB power output

        public string Timestamp { get; set; }
        public string PacketTimestamp { get; set; }

        public void UpdatePMSData(byte[] packet, uint packetID)
        {
            PMSPacket0 = packet;
            Debug.WriteLine($"Is little endian: {BitConverter.IsLittleEndian}");
            switch (packetID)
            {
                case 0:
                    I1 = BitConverter.ToUInt16(packet, 11) * 0.00000760371;
                    I2 = BitConverter.ToUInt16(packet, 13) * 0.00000760371;
                    VBAT = BitConverter.ToUInt16(packet, 15) * 0.000375183;
                    AUX1 = BitConverter.ToUInt16(packet, 19) * 0.000375183;
                    AUX2 = BitConverter.ToUInt16(packet, 40) * 0.000375183;
                    AUX3 = BitConverter.ToUInt16(packet, 61) * 0.000375183;

                    break;
                case 1:
                    AUX4 = BitConverter.ToUInt16(packet, 11) * 0.000375183; //TODO: to check which manager is which, do a sanity check on the SAx thermistors, they should all be similar
                    AUX5 = BitConverter.ToUInt16(packet, 32) * 0.000375183;
                    AUX6 = BitConverter.ToUInt16(packet, 53) * 0.000375183;
                    break;
                case 2:
                    AUX7 = BitConverter.ToUInt16(packet, 6) * 0.000375183;
                    AUX8 = BitConverter.ToUInt16(packet, 27) * 0.000375183;
                    AUX9 = BitConverter.ToUInt16(packet, 48) * 0.000375183;
                    AUX10 = BitConverter.ToUInt16(packet, 68) * 0.000375183;
                    break;
                case 3:
                    AUX11 = BitConverter.ToUInt16(packet, 19) * 0.000375183;
                    break;
               
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
        private double BEVTemp(double b)
        {
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
            return Converter.DoubleToInt16Byte(I1)
                            .Concat(Converter.DoubleToInt16Byte(I2))
                            .Concat(Converter.DoubleToInt16Byte(VBAT))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVDCFCPlus()))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVDCFCMinus()))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVShuntTemp()))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVDCFCContactorTemp()))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVMainContactorTemp()))
                            .Concat(Converter.DoubleToInt16Byte(GetBEVVREF()))
                            .ToArray();
        }
        
        //BET A
        public double GetBETAHVDCMinus()
        {
            return -(AUX1 * 701 + 3);
        }
        public double GetBETAIsolationSwitch()
        {
            if (AUX2 < 25.51)
            {
                //Switch open
                return 0;
            } else if (AUX2 > 46.52)
            {
                //Switch closed
                return 1;
            } else
            {
                return 2; //Between thresholds
            }
        }
        public double GetBETAShuntTemperature()
        {
            //put voltage against graph to find thermistor temperature
            return ShuntNTCTransferFunction(AUX3);
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
        public double GetBETA12VInputSensing()
        {
            return AUX7 * 5.83 + 0.5;
        }
        public byte[] GetBETAPMS()
        {
            return Converter.DoubleToInt16Byte(GetBETAHVDCMinus()) //Div says, there is no HVDC plus
                .Concat(Converter.DoubleToInt16Byte(GetBETAIsolationSwitch()))
                .Concat(Converter.DoubleToInt16Byte(GetBETAShuntTemperature()))
                .Concat(Converter.DoubleToInt16Byte(GetBETASA1Temp()))
                .Concat(Converter.DoubleToInt16Byte(GetBETASA3Temp()))
                .Concat(Converter.DoubleToInt16Byte(GetBETASA4Temp()))
                .Concat(Converter.DoubleToInt16Byte(I1))
                .Concat(Converter.DoubleToInt16Byte(I2))
                .Concat(Converter.DoubleToInt16Byte(GetBETA12VInputSensing()))
                .Concat(Converter.DoubleToInt16Byte(GetBETAIsolationSwitch()))
                .ToArray();
        }

        //BET B
        public double GetBETBHVDCMinus()
        {
            return -(AUX1 * 701 + 3);
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
                return 0;
            } else if (AUX5 > 46.52)
            {
                //Switch is closed
                return 1;
            } else
            {
                return 2; //Between thresholds
            }
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
            return Converter.DoubleToInt16Byte(GetBETBHVDCMinus())
                .Concat(Converter.DoubleToInt16Byte(GetBETBDCFCPlus()))
                .Concat(Converter.DoubleToInt16Byte(GetBETBDCFCMinus()))
                .Concat(Converter.DoubleToInt16Byte(GetBETBDCFCDifferential()))
                .Concat(Converter.DoubleToInt16Byte(GetBETBIsolationSwitch()))
                .Concat(Converter.DoubleToInt16Byte(GetBETBShuntTemp()))
                .Concat(Converter.DoubleToInt16Byte(GetBETBSB1Temp()))
                .Concat(Converter.DoubleToInt16Byte(I1))
                .Concat(Converter.DoubleToInt16Byte(I2))
                .Concat(Converter.DoubleToInt16Byte(GetBETBIsolationSwitch()))
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
            return 10.0 + 250.0 * (G1V / C1V);
        }
        public double EMSPressure2()
        {
            return 10.0 + 250.0 * (G3V / G7V);
        }
        private double EMSThermistorTransferFunction(double thermistorVoltage, double referenceVoltage)
        {
            if (thermistorVoltage != 0)
            {
                double NTCResistance = ((10.0) / (thermistorVoltage / referenceVoltage - 1.0));
                return (1.0 / (NTC_COEF_A + (NTC_COEF_B * Math.Log(NTCResistance)) + (NTC_COEF_C * Math.Pow(Math.Log(NTCResistance), 3)))) - 273.0;
            }
            else
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
            return Converter.DoubleToInt16Byte(EMSBDSBVoltage())
                     .Concat(Converter.DoubleToInt16Byte(EMSReferenceVoltage1()))
                     .Concat(Converter.DoubleToInt16Byte(EMSReferenceVoltage2()))
                     .Concat(Converter.DoubleToInt16Byte(EMSTemperature1()))
                     .Concat(Converter.DoubleToInt16Byte(EMSTemperature2()))
                     .Concat(Converter.DoubleToInt16Byte(EMSPressure1()))
                     .Concat(Converter.DoubleToInt16Byte(EMSPressure2()))
                     .Concat(Converter.DoubleToInt16Byte(EMSGas1()))
                     .Concat(Converter.DoubleToInt16Byte(EMSGas2()))
                     .ToArray();
        }
    }
}
