using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace CSnet
{
    public class ModuleData
    {
        public Int16 version { get; set; }
        public string MacAddress { get; set; }
        public byte[] Packet0 { get; set; } = new byte[0];
        public byte[] Packet1 { get; set; } = new byte[0];
        public double[] CGV { get; set; } = new double[8];
        public double[] CGDV { get; set; } = new double[8];
        public double Thermistor1 { get; set; }
        public byte[] Thermistor1Raw { get; set; } = new byte[2];
        public double Thermistor2 { get; set; }
        public byte[] Thermistor2Raw { get; set; } = new byte[2];
        public double OldPktTimestamp { get; set; } = 0;


        //CMU Performance
        public Int16 Latency { get; set; }
        public Int16 TotalPECErrors { get; set; } = 0;
        public double AverageUpdateRate { get; set; }
        public double PeakUpdateRate { get; set; } = 0;
        public Int16 AverageRSSI { get; set; }
        public Int16 PeakRSSI { get; set; }
        public Int16 UpdateRateCount { get; set; } = 0;

        public void UpdateData(byte[] packet, uint packetID, double time)
        {
            if (packetID == 0)
            {
                Packet0 = packet;

                //ignore the initial value of 0
                if (OldPktTimestamp == 0)
                {
                    OldPktTimestamp = time;
                }

                double difference = Math.Abs(time - OldPktTimestamp);
                if (difference > PeakUpdateRate)
                {
                    PeakUpdateRate = difference;//all times are measured in seconds
                }               

                OldPktTimestamp = time; //time measured in seconds since device turned on, save for next iteration

                //calculate cumulative average for time between packets
                AverageUpdateRate = (difference + (UpdateRateCount * AverageUpdateRate)) / (UpdateRateCount + 1);
                UpdateRateCount++;

                Debug.WriteLine($"Update {MacAddress} time: {time} p: {PeakUpdateRate} diff: {difference} a: {AverageUpdateRate}");

                //modules[uiDeviceSource].AverageUpdateRate;
                //modules[uiDeviceSource].PeakUpdateRate;

                CGV[0] = BitConverter.ToUInt16(packet, 6) * 0.0001;
                CGV[1] = BitConverter.ToUInt16(packet, 8) * 0.0001;
                CGV[2] = BitConverter.ToUInt16(packet, 10) * 0.0001;
                CGV[3] = BitConverter.ToUInt16(packet, 14) * 0.0001;
                CGV[4] = BitConverter.ToUInt16(packet, 16) * 0.0001;
                CGV[5] = BitConverter.ToUInt16(packet, 18) * 0.0001;
                CGV[6] = BitConverter.ToUInt16(packet, 22) * 0.0001;
                CGV[7] = BitConverter.ToUInt16(packet, 24) * 0.0001;

                CGDV[0] = BitConverter.ToInt16(packet, 46) * 0.0002; //cell diagnostic registers are storing the result of ADSC command (see ADI datasheet)
                CGDV[1] = BitConverter.ToInt16(packet, 48) * 0.0002; //Vadsc = 0.5 * Vcell
                CGDV[2] = BitConverter.ToInt16(packet, 50) * 0.0002;
                CGDV[3] = BitConverter.ToInt16(packet, 54) * 0.0002;
                CGDV[4] = BitConverter.ToInt16(packet, 56) * 0.0002;
                CGDV[5] = BitConverter.ToInt16(packet, 58) * 0.0002;
                CGDV[6] = BitConverter.ToInt16(packet, 62) * 0.0002;
                CGDV[7] = BitConverter.ToInt16(packet, 64) * 0.0002;

                //Want C# 8 for packet[6..12] :(
                TotalPECErrors += Converter.CalculatePECError(packet.Skip(6).Take(8).ToArray()); 
                //TODO: use C packet definitions/structs to structure packets as they come in, and then iterate through each read & apply PEC method
            }
            else if (packetID == 1)
            {
                FillThermistorValues(packet.Skip(8).Take(4).ToArray());
                Packet1 = packet;
            }
        }

        public byte[] GetCGDV()
        {
            return Converter.DoubleArrayToBytes(CGDV);
        }

        public byte[] GetCGV()
        {
            return Converter.DoubleArrayToBytes(CGV);
        }

        public void UpdateMetadata(byte[] packet, uint packetID)
        {
            if (packetID == 0)
            {
                //Latency is little endian, so reverse byte order
                Latency = BitConverter.ToInt16(new byte[] { packet[25], packet[24] }, 0); //this should technically be uint16

                byte RSSI = packet[31];
                if (RSSI > PeakRSSI)
                {
                    PeakRSSI = RSSI;
                }
                //TODO: also calculate average RSSI here & compare to health report packet ave RSSI, as seen below
            }
        }

        public void UpdateHealthdata(byte[] packet, uint packetID)
        {
            if (packetID == 1)
            {
                //Get average RSSI to manager 0?
                AverageRSSI = packet[1]; //TODO: make sure I'm checking the correct average RSSI
            }
        }

        private void FillThermistorValues(byte[] rawTemps)
        {         
            //convert raw thermistor values to human readable celsius
            Thermistor1 = BMSTemp(BitConverter.ToUInt16(rawTemps,0));
            Thermistor2 = BMSTemp(BitConverter.ToUInt16(rawTemps, 2));
            Thermistor1Raw = rawTemps.Take(2).ToArray();
            Thermistor2Raw = rawTemps.Skip(2).Take(2).ToArray();
        }

        private double BMSTemp(ushort a)//温度计算
        {
            double temp;
            double b = a * 0.0001;

            //put voltage against graph to find thermistor temperature
            temp = (Math.Pow(b, 6) * 11.802) - (Math.Pow(b, 5) * 133.92) + (Math.Pow(b, 4) * 578.41) - (Math.Pow(b, 3) * 1207.8) + (Math.Pow(b, 2) * 1273.5) - b * 681.29 + 209.33;
            //ta = 98.977;
            //tb = -39.02;
            //tc = 3.4012;
            //td = -0.1862;
            //double temp1 = (ta + (tb * lnr) + (tc * (Math.Pow(lnr, 2))) + (td * (Math.Pow(lnr, 3))));
            return temp;
        }

        
    }
}
