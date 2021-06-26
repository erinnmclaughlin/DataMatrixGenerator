using System.Collections;
using System.Collections.Generic;

namespace DataMatrixGenerator
{
    public class DataMatrixDecoder
    {
        public static string Decode(List<BitArray> byteArrays)
        {
            string result = "";

            foreach (var arr in byteArrays)
            {
                int total = GetTotal(arr);
                result += (total > 130) ? (total - 130).ToString() : (char)total;
            }

            return result;
        }

        private static int GetTotal(BitArray arr)
        {
            int total = 0;
            int index = 1;

            foreach (bool bit in arr)
            {
                total += index * (bit ? 1 : 0);
                index *= 2;
            }

            return total;
        }
    }
}
