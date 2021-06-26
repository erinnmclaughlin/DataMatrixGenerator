using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataMatrixGenerator
{
    public class DataMatrixEncoder
    {
        public static List<BitArray> Encode(string content, int size)
        {
            return GetGroups(content).SelectMany(x => EncodeGroup(x, size)).ToList();
        }

        private static List<BitArray> EncodeASCII(string content, int size)
        {
            var result = new List<BitArray>();

            foreach (var c in content)
            {
                var bytes = BitConverter.GetBytes(c);

                if (bytes.Length != size)
                    Array.Resize(ref bytes, size);

                result.Add(new BitArray(bytes));
            }

            return result;
        }

        private static List<BitArray> EncodeGroup(string content, int size)
        {
            if (content.Any(x => x.IsNumber()) && content.Any(x => !x.IsNumber()))
                throw new InvalidOperationException($"{nameof(content)} is in an invalid format.");

            if (int.TryParse(content, out var number))
            {
                return EncodeNumber(number, size);
            }
            else
            {
                return EncodeASCII(content, size);
            }
        }

        private static List<BitArray> EncodeNumber(int number, int size)
        {
            var result = new List<BitArray>();
            var groups = new List<int> { number };
            var maxSize = GetMaxValue(size);

            while (groups.Any(x => x > maxSize))
            {
                var str = number.ToString();
                var maxLength = groups.Count + 1;

                groups = SplitStringIntoSections(str, maxLength).Select(x => int.Parse(x)).ToList();
            }

            foreach (var group in groups)
            {
                var buffer = number <= 9 ? 0 : 130;
                var bytes = BitConverter.GetBytes(group + buffer);

                result.Add(new BitArray(bytes) { Length = size });
            }

            return result;
        }

        private static string[] GetGroups(string content)
        {
            string curr = "";
            var groups = new List<string>();

            foreach (var c in content)
            {
                if (curr == string.Empty || curr[^1].IsNumber() == c.IsNumber())
                {
                    curr += c;
                }
                else
                {
                    groups.Add(curr);
                    curr = c.ToString();
                }
            }

            groups.Add(curr);
            return groups.ToArray();
        }

        private static int GetMaxValue(int size)
        {
            int total = 0;
            int index = 1;

            for (int i = 0; i < size; i++)
            {
                total += index;
                index *= 2;
            }

            return total - 130;
        }

        private static IEnumerable<string> SplitStringIntoSections(string str, int maxSectionSize)
        {
            for(int i = 0; i < str.Length; i += maxSectionSize)
            {
                yield return str.Substring(i, Math.Min(maxSectionSize, str.Length - i));
            }
        }
    }
}
