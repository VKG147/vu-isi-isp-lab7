using System;
using System.Collections;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static int[] pc1 = new int[56]
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        static int[] pc2 = new int[48]
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };

        static int[] rotate = new int[16]
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        static void Main(string[] args)
        {
            string key = "VladasZv";
            byte[] bytes = Encoding.ASCII.GetBytes(key);

            BitArray k = ReverseBytes(new BitArray(bytes));
            BitArray kPlus = GetKPlus(k);

            (BitArray c0, BitArray d0) = Split(kPlus);

            BitArray[] l = new BitArray[16];
            BitArray[] r = new BitArray[16];

            BitArray lx = new BitArray(c0);
            BitArray rx = new BitArray(d0);
            for (int i = 0; i < 16; ++i)
            {
                BitArray lx1 = new BitArray(lx);
                BitArray lx2 = new BitArray(lx);
                lx = lx1.RightShift(rotate[i]).Or(lx2.LeftShift(lx.Length - rotate[i]));

                BitArray rx1 = new BitArray(rx);
                BitArray rx2 = new BitArray(rx);
                rx = rx1.RightShift(rotate[i]).Or(rx2.LeftShift(rx.Length - rotate[i]));

                l[i] = new BitArray(lx);
                r[i] = new BitArray(rx);
            }

            BitArray[] keys = new BitArray[16];
            for (int i = 0; i < keys.Length; ++i)
            {
                BitArray kx = GetK(l[i], r[i]);
                keys[i] = new BitArray(kx);
            }


            BitArray answer = new BitArray(48);
            for (int i = 0; i < keys.Length; ++i)
            {
                answer = keys[i].Xor(answer);
            }

            for (int i = 0; i < answer.Length; ++i)
            {
                Console.Write(answer[i] ? "1" : "0");
            }
            Console.WriteLine();
        }

        //static 

        static (BitArray, BitArray) Split(BitArray bitArray)
        {
            BitArray l = new BitArray(bitArray.Length / 2);
            BitArray r = new BitArray(bitArray.Length / 2);

            for (int i = 0; i < l.Length; ++i)
            {
                l[i] = bitArray[i];
                r[i] = bitArray[i + bitArray.Length / 2];
            }

            return (l, r);
        }

        static BitArray Combine(BitArray a, BitArray b)
        {
            BitArray result = new BitArray(a.Length + b.Length);
            for (int i = 0; i < a.Length; ++i)
            {
                result[i] = a[i];
                result[i + a.Length] = b[i];
            }

            return result;
        }

        static BitArray ReverseBytes(BitArray bitArray)
        {
            BitArray reversed = new BitArray(bitArray);
            for (int i = 0; i < bitArray.Length / 8; i++)
            {
                for (int j = 0; j < 8; ++j)
                {
                    reversed[i * 8 + j] = bitArray[i * 8 + (7 - j)];
                }
            }
            return reversed;
        }

        static BitArray GetKPlus(BitArray k)
        {
            BitArray kPlus = new BitArray(56);

            for (int i = 0; i < pc1.Length; i++)
            {
                kPlus[i] = k[pc1[i] - 1];
            }

            return kPlus;
        }

        static BitArray GetK(BitArray l, BitArray r)
        {
            BitArray c = Combine(l, r);
            BitArray k = new BitArray(pc2.Length);

            for (int i = 0; i < k.Length; ++i)
            {
                k[i] = c[pc2[i] - 1];
            }

            return k;
        }
    }
}
