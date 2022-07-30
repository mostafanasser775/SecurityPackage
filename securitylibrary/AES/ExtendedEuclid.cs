using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int T, T1, T2;

            int A1 = 1;
            int A2 = 0;
            int B1 = 0;
            int B2 = 1;
            do
            {
                if (number == 0) return -1;
                else if (number == 1)
                {
                    if (B2 < 0) B2 += 26;
                    return B2;
                }
                T = A1 - (baseN / number) * B1;
                T1 = A2 - (baseN / number) * B2;
                T2 = baseN - (baseN / number) * number;
                A1 = B1;
                A2 = B2;
                baseN = number;
                number = int.Parse(T2.ToString());

                B1 = int.Parse(T.ToString());
                B2 = int.Parse(T1.ToString());
            } while (true);
        }
    }
}
