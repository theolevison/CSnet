using System;
using System.Collections.Generic;
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
    }
}
