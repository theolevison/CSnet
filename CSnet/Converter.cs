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

        public static short CalculatePECError(byte[] packetSlice) //TODO: implement for all data, not just READA
        {
            ushort uiReceivedPEC = (ushort)((packetSlice[6] << 8 | packetSlice[7]) & 0x03FF);

            return (short)(adi_pec10_calc(true, 6, packetSlice) != uiReceivedPEC ? 1 : 0);
        }

        private static ushort adi_pec10_calc(bool rx_cmd, int len, byte[] data)
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
