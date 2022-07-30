using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {

        public int calculate(int alpha, int power,int q) {
            int val=1;
            for (int i = 0; i < power; i++)
                val = (val * alpha) % q;
            return val;
        }
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            List<int> algorithm = new List<int>();
            int ya=calculate(alpha, xa, q);
            int yb=calculate(alpha, xb, q);
            algorithm.Add(calculate(ya, xb, q)); //key1
            algorithm.Add(calculate(yb, xa, q)); //key2
            return algorithm;
        }
    }
}
