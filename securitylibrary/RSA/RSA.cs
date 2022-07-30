using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
  
    public class RSA
    {
        static int modInverse(int a, int m)
        {

            for (int x = 1; x < m; x++)
                if (((a % m) * (x % m)) % m == 1)
                    return x;
            return -1;
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q, val = 1;
            for (int i = 0; i < e; i++)
                val = (val * M) % n;
            return val;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int val = 1,n=p*q,phi=(p-1)*(q-1);
            
            int d = modInverse(e,phi);
            for (int i = 0; i < d; i++)
                val = (val*C) % n;
            Console.WriteLine(val);
            return val;
        }
    }
}
