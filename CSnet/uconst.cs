using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSnet
{
    class uconst
    {

        //通用方法
        public static byte[] DoubleArrayToByteArray(double[] doubleArray)
        {
            byte[] byteArray = new byte[doubleArray.Length * sizeof(double)];
            Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static byte[] IntArrayToByteArray(int[] intArray)
        {
            byte[] byteArray = new byte[intArray.Length * sizeof(int)];
            Buffer.BlockCopy(intArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static byte[] FloatArrayToByteArray(float[] floatArray)
        {
            byte[] byteArray = new byte[floatArray.Length * sizeof(float)];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;

            //double[] doubleArray = { 1.0, 2.5, 3.6, 4.9 };
            //float[] floatArray = Array.ConvertAll(doubleArray, item => (float)item);
        }

        //public static byte[] DoubleArrayToByteArray(double[] doubleArray)
        //{
        //    byte[] byteArray = new byte[doubleArray.Length * sizeof(double)];
        //    Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);
        //    return byteArray;
        //}
        public static int[] DoubleArrayToIntArray(double[] doubleArray)
        {

            //byte[] byteArray = new byte[doubleArray.Length * sizeof(double)];
            // Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);
            //return byteArray;
            int[] doubleArray1 = new int[8];
            for (int i = 0; i < doubleArray.Length; i++)
            {
                doubleArray1[i] = (int)(doubleArray[i] * 10000);
            }

            return doubleArray1;
        }

        public static string packVersion = "";
    }
}