using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSnet
{
    public static class Converter
    {
        public static byte[] DoubleToInt16Byte(double d)
        {
            Int16 temp = (Int16)(d * 100); //preserve 2 decimal places
            return BitConverter.GetBytes(temp);
        }

        public static byte[] DoubleArrayToBytes(double[] array)
        {
            List<byte> byteList = new List<byte>();
            foreach (double d in array)
            {
                Debug.WriteLine(d);
                byteList.AddRange(Converter.DoubleToInt16Byte(d*100)); //perserve 2 more decimal places, currently only used for cell voltages
            }

            return byteList.ToArray();
        }
    }
}
