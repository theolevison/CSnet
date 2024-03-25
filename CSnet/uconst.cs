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

        public static int DoubleToInt(double Double)
        {

            //byte[] byteArray = new byte[doubleArray.Length * sizeof(double)];
            // Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);
            //return byteArray;
            int Int;
            if (Double > 10000)
                Int = (int)(Double);
            else if (Double < 1000)
                Int = (int)(Double * 100);
            else
                Int = (int)(Double);


            return Int;
        }

        //新增最大值最小值平均值的计算
        public static double[] calulate(double[] voltage)
        {
            double[] numbers = new double[3];

            numbers[0] = voltage.Min();
            numbers[1] = voltage.Max();
            numbers[2] = voltage.Average();


            return numbers;
        }

        public static string packVersion = "";
    }
}