using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CSnet
{
    public class ModuleData
    {
        public int version {  get; set; }
        public string MacAddress { get; set; }
        public byte[] Packet0 { get; set; } = new byte[0];
        public byte[] Packet1 { get; set; } = new byte[0];
        public double[] CGV { get; set; } = new double[8];
        public double[] CGDV { get; set; } = new double[8];
        public double Thermistor1 { get; set; }
        public byte[] Thermistor1Raw { get; set; } = new byte[2];
        public double Thermistor2 { get; set; }
        public byte[] Thermistor2Raw { get; set; } = new byte[2];
        public string Timestamp { get; set; }
        public string PacketTimestamp { get; set; }

        public void FillThermistorValues(byte[] rawTemps)
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
