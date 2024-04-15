﻿using System;
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
        public double BMSMessageTimestamp { get; set; } = 0;


        //CMU Performance
        public Int16 Latency { get; set; }
        public Int16 TotalPECErrors { get; set; } = 0;
        public double AverageUpdateRate { get; set; }
        public double PeakUpdateRate { get; set; } = 0;
        public Int16 AverageRSSI { get; set; }
        public Int16 PeakRSSI { get; set; }
        public Int16 UpdateRateCount { get; set; } = 0;

        private void CalculatePECError(byte[] packetSlice) //TODO: implement for all data, not just READA
        {
            ushort uiReceivedPEC = (ushort)((packetSlice[6] << 8 | packetSlice[7]) & 0x03FF);

            if (adi_pec10_calc(true, 6, packetSlice) != uiReceivedPEC)
            {
                TotalPECErrors++;
            }
        }

        public void UpdateData(byte[] packet, uint packetID, double time)
        {
            if (packetID == 0)
            {
                Packet0 = packet;

                //ignore the initial value of 0
                if (BMSMessageTimestamp == 0)
                {
                    BMSMessageTimestamp = time;
                }

                double difference = Math.Abs(time - BMSMessageTimestamp);
                if (difference > PeakUpdateRate)
                {
                    PeakUpdateRate = difference;
                }
                //Debug.WriteLine($"Update {MacAddress} pu: {PeakUpdateRate} diff: {difference} pa: {AverageUpdateRate}");

                BMSMessageTimestamp = time; //time measured in seconds since device turned on, save for next iteration

                //calculate cumulative average for time between packets
                AverageUpdateRate = (difference + UpdateRateCount * AverageUpdateRate) / (UpdateRateCount + 1);
                UpdateRateCount++;

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
                CalculatePECError(packet.Skip(6).Take(8).ToArray());
            }
            else if (packetID == 1)
            {
                FillThermistorValues(packet.Skip(8).Take(4).ToArray());
                Packet1 = packet;
            }
        }

        public byte[] GetCGDV()
        {
            return DoubleArrayToBytes(CGDV);
        }

        public byte[] GetCGV()
        {
            return DoubleArrayToBytes(CGV);
        }

        private byte[] DoubleArrayToBytes(double[] array)
        {
            List<byte> byteList = new List<byte>();
            foreach (double d in array)
            {
                byteList.AddRange(Converter.DoubleToInt16Byte(d));
            }

            return byteList.ToArray();
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

        public static ushort adi_pec10_calc(bool rx_cmd, int len, byte[] data)
        {
            ushort remainder = 16; /* PEC_SEED;   0000010000 */
            ushort polynom = 0x48F; /* x10 + x7 + x3 + x2 + x + 1 <- the CRC10 polynomial         100 1000 1111   48F */

            /* Perform modulo-2 division, a byte at a time. */
            for (int pbyte = 0; pbyte < len; ++pbyte)
            {
                /* Bring the next byte into the remainder. */
                remainder ^= (ushort)(data[pbyte] << 2);

                /* Perform modulo-2 division, a bit at a time.*/
                for (int bit_ = 8; bit_ > 0; --bit_)
                {
                    /* Try to divide the current data bit. */
                    if ((remainder & 0x200) > 0) /* equivalent to remainder & 2^14 simply check for MSB */
                    {
                        remainder = (ushort)(remainder << 1);
                        remainder = (ushort)(remainder ^ polynom);
                    }
                    else
                    {
                        remainder = (ushort)(remainder << 1);
                    }
                }
            }

            if (rx_cmd == true)
            {
                remainder ^= (ushort)((data[len] & 0xFC) << 2);

                /* Perform modulo-2 division, a bit at a time */
                for (int bit_ = 6; bit_ > 0; --bit_)
                {
                    /* Try to divide the current data bit */
                    if ((remainder & 0x200) > 0) /* equivalent to remainder & 2^14 simply check for MSB */
                    {
                        remainder = (ushort)(remainder << 1);
                        remainder = (ushort)(remainder ^ polynom);
                    }
                    else
                    {
                        remainder = (ushort)(remainder << 1);
                    }
                }
            }
            return (ushort)(remainder & 0x3FF);
        }
    }
}
