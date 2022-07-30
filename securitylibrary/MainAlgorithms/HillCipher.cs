using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    /// 


    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        public static int getinverse(int determent)
        {
            int rex = 0;
            for (int i = 2; i < 26; i++)
            {
                if (i != 25 || i != 1)
                {
                    if ((determent * i) % 26 == 1 && i != determent)
                    {
                        rex = i;
                        break;

                    }
                }
                if (determent == 1 || determent == 25)
                {
                    rex = determent;
                    break;
                }


            }
            return rex;

        }
        public static ulong GCD(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();

            List<int> plaincopy = new List<int>();
            List<int> samecipherpos = new List<int>();
            List<int> plainInvers = new List<int>();

            int determent = 0;
            int inverse = 0;
            int x = 0;
            int y = 0;
            for (x = 0; x < plainText.Count; x += 2)
            {
                for (y = 0; y < plainText.Count; y += 2)
                {

                    plaincopy.Clear();
                    plaincopy.Add(plainText[x]);
                    plaincopy.Add(plainText[y]);
                    plaincopy.Add(plainText[x + 1]);
                    plaincopy.Add(plainText[y + 1]);

                    determent = plaincopy[0] * plaincopy[3] - plaincopy[1] * plaincopy[2];

                    determent %= 26; if (determent < 0) determent += 26;

                    inverse = getinverse(determent);
                    if (inverse != 0)
                        break;

                }
                if (inverse != 0)
                    break;

            }
            Console.WriteLine(determent);
            if (inverse != 0)
            {
                int[,] planx = new int[2, 2];
                int c = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        planx[i, j] = (plaincopy[c] * inverse) % 26; if (planx[i, j] < 0) planx[i, j] += 26;
                        if ((i + j) % 2 != 0) planx[i, j] = -planx[i, j];
                        c++;
                    }
                }
                int swap = 0;
                swap = planx[0, 0];
                planx[0, 0] = planx[1, 1];
                planx[1, 1] = swap;

                samecipherpos.Add(cipherText[x]);
                samecipherpos.Add(cipherText[y]);
                samecipherpos.Add(cipherText[x + 1]);
                samecipherpos.Add(cipherText[y + 1]);
                int[,] cipher = new int[2, 2];
                c = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        cipher[i, j] = samecipherpos[c];
                        c++;
                    }
                }

                int[,] Matrix3 = new int[2, 2];
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < 2; k++)
                        {
                            Matrix3[i, j] += cipher[i, k] * planx[k, j];

                        }
                        Matrix3[i, j] = Matrix3[i, j] % 26; if (Matrix3[i, j] < 0) Matrix3[i, j] += 26;
                        key.Add(Matrix3[i, j]);
                    }
                }
                return key;
            }
            else
                throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plain = new List<int>();
            int keylen = 0;
            if (key.Count() % 2 == 0) keylen = 2;
            if (key.Count() % 3 == 0) keylen = 3;
            int[,] keyx = new int[keylen, keylen];
            int inverse = 0;
            int c = 0;

            if (keylen == 2)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[i, j] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { keyx[i, j] = key[c]; }

                        keyx[j, i] = key[c];
                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }
                Console.WriteLine("___________________");

                int determent = keyx[1, 1] * keyx[0, 0] - keyx[0, 1] * keyx[1, 0];
                determent = determent % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                inverse = getinverse(determent);
                Console.WriteLine(inverse);

                int[,] copykey = new int[keylen, keylen];
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        copykey[i, j] = keyx[i, j];

                    }
                }
                int swap = 0;
                copykey[1, 1] = keyx[0, 0]; copykey[0, 1] = -keyx[0, 1];
                copykey[0, 0] = keyx[1, 1]; copykey[1, 0] = -keyx[1, 0];
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        keyx[i, j] = (copykey[i, j] * inverse) % 26; if (keyx[i, j] < 0) keyx[i, j] += 26;
                        Console.Write(keyx[i, j] + "  ");
                    }
                    Console.WriteLine();
                }
                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * keyx[k, j];
                        }
                        Matrix3[i, j] = Matrix3[i, j] % 26; if (Matrix3[i, j] < 0) Matrix3[i, j] += 26;
                        plain.Add(Matrix3[i, j]);
                        Console.Write(Matrix3[i, j] % 26 + "  ");
                    }


                }
            }
            if (keylen == 3)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[i, j] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { keyx[j, i] = key[c]; }

                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }

                int e1 = keyx[0, 0] * (keyx[1, 1] * keyx[2, 2] - keyx[1, 2] * keyx[2, 1]);
                int e2 = keyx[0, 1] * (keyx[1, 0] * keyx[2, 2] - keyx[1, 2] * keyx[2, 0]);
                int e3 = keyx[0, 2] * (keyx[1, 0] * keyx[2, 1] - keyx[1, 1] * keyx[2, 0]);
                int determent = (e1 - e2 + e3) % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                inverse = getinverse(determent);
                Console.WriteLine(inverse);
                //copy the matrix to do operations on it
                int[,] copykey = new int[keylen, keylen];
                Console.WriteLine("______________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        copykey[i, j] = getdemat(i, j);
                        Console.Write(copykey[i, j] + "   ");
                    }
                    Console.WriteLine();
                }
                //calculate the inverse matrix 
                int getdemat(int row, int col)
                {
                    int value = 0;
                    List<int> mat = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {

                        for (int j = 0; j < 3; j++)
                        {
                            if (i == row || j == col)
                            {
                                continue;
                            }
                            else
                            {

                                mat.Add(keyx[i, j]);
                            }

                        }
                    }
                    value = mat[0] * mat[3] - mat[2] * mat[1];
                    if ((row + col) % 2 != 0)
                    {
                        value = -value;
                        return value;
                    }
                    else
                    {
                        return value;
                    }
                }

                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        keyx[i, j] = (copykey[j, i] * inverse) % 26;
                        if (keyx[i, j] < 0) keyx[i, j] += 26;
                    }

                }
                Console.WriteLine("_________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] + "     ");
                    }
                    Console.WriteLine();

                }

                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * keyx[k, j];

                        }
                        //plain.Add(Matrix3[j, i]);
                        //Console.Write(Matrix3[i, j] % 26+"  ");
                    }
                }
                int[,] Matrix4 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix4[i, j] = Matrix3[i, j] % 26;
                    }
                }
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(Matrix4[i, j] + "   ");
                        plain.Add(Matrix4[i, j]);
                    }
                }
            }
            if (inverse == 0)
            {
                throw new NotImplementedException();
            }
            else
            {
                return plain;
            }
        }
        public string Decrypt(string newCipher, string newKey)
        {
            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> chars = new Dictionary<char, int>();
            chars.Add('a', 0);
            chars.Add('b', 1);
            chars.Add('c', 2);
            chars.Add('d', 3);
            chars.Add('e', 4);
            chars.Add('f', 5);
            chars.Add('g', 6);
            chars.Add('h', 7);
            chars.Add('i', 8);
            chars.Add('j', 9);
            chars.Add('k', 10);
            chars.Add('l', 11);
            chars.Add('m', 12);
            chars.Add('n', 13);
            chars.Add('o', 14);
            chars.Add('p', 15);
            chars.Add('q', 16);
            chars.Add('r', 17);
            chars.Add('s', 18);
            chars.Add('t', 19);
            chars.Add('u', 20);
            chars.Add('v', 21);
            chars.Add('w', 22);
            chars.Add('x', 23);
            chars.Add('y', 24);
            chars.Add('z', 25);
            List<int> cipherText = new List<int>();
            List<int> key = new List<int>();
            newCipher = newCipher.ToLower();
            for (int i = 0; i < newCipher.Count(); i++)
            {
                cipherText.Add(chars[newCipher[i]]);
            }
            for (int i = 0; i < newKey.Count(); i++)
            {
                key.Add(chars[newKey[i]]);
            }
            List<int> plain = new List<int>();
            int keylen = 0;
            if (key.Count() % 2 == 0) keylen = 2;
            if (key.Count() % 3 == 0) keylen = 3;
            int[,] keyx = new int[keylen, keylen];
            int inverse = 0;
            int c = 0;

            if (keylen == 2)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[i, j] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { keyx[i, j] = key[c]; }

                        keyx[j, i] = key[c];
                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }
                Console.WriteLine("___________________");

                int determent = keyx[1, 1] * keyx[0, 0] - keyx[0, 1] * keyx[1, 0];
                determent = determent % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                inverse = getinverse(determent);
                Console.WriteLine(inverse);

                int[,] copykey = new int[keylen, keylen];
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        copykey[i, j] = keyx[i, j];

                    }
                }
                int swap = 0;
                copykey[1, 1] = keyx[0, 0]; copykey[0, 1] = -keyx[0, 1];
                copykey[0, 0] = keyx[1, 1]; copykey[1, 0] = -keyx[1, 0];
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        keyx[i, j] = (copykey[i, j] * inverse) % 26; if (keyx[i, j] < 0) keyx[i, j] += 26;
                        Console.Write(keyx[i, j] + "  ");
                    }
                    Console.WriteLine();
                }
                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * keyx[k, j];
                        }
                        Matrix3[i, j] = Matrix3[i, j] % 26; if (Matrix3[i, j] < 0) Matrix3[i, j] += 26;
                        plain.Add(Matrix3[i, j]);
                        Console.Write(Matrix3[i, j] % 26 + "  ");
                    }


                }
            }
            if (keylen == 3)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[i, j] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { keyx[j, i] = key[c]; }

                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }

                int e1 = keyx[0, 0] * (keyx[1, 1] * keyx[2, 2] - keyx[1, 2] * keyx[2, 1]);
                int e2 = keyx[0, 1] * (keyx[1, 0] * keyx[2, 2] - keyx[1, 2] * keyx[2, 0]);
                int e3 = keyx[0, 2] * (keyx[1, 0] * keyx[2, 1] - keyx[1, 1] * keyx[2, 0]);
                int determent = (e1 - e2 + e3) % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                inverse = getinverse(determent);
                Console.WriteLine(inverse);
                //copy the matrix to do operations on it
                int[,] copykey = new int[keylen, keylen];
                Console.WriteLine("______________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        copykey[i, j] = getdemat(i, j);
                        Console.Write(copykey[i, j] + "   ");
                    }
                    Console.WriteLine();
                }
                //calculate the inverse matrix 
                int getdemat(int row, int col)
                {
                    int value = 0;
                    List<int> mat = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {

                        for (int j = 0; j < 3; j++)
                        {
                            if (i == row || j == col)
                            {
                                continue;
                            }
                            else
                            {

                                mat.Add(keyx[i, j]);
                            }

                        }
                    }
                    value = mat[0] * mat[3] - mat[2] * mat[1];
                    if ((row + col) % 2 != 0)
                    {
                        value = -value;
                        return value;
                    }
                    else
                    {
                        return value;
                    }
                }

                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        keyx[i, j] = (copykey[j, i] * inverse) % 26;
                        if (keyx[i, j] < 0) keyx[i, j] += 26;
                    }

                }
                Console.WriteLine("_________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(keyx[i, j] + "     ");
                    }
                    Console.WriteLine();

                }

                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * keyx[k, j];

                        }
                        //plain.Add(Matrix3[j, i]);
                        //Console.Write(Matrix3[i, j] % 26+"  ");
                    }
                }
                int[,] Matrix4 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix4[i, j] = Matrix3[i, j] % 26;
                    }
                }
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(Matrix4[i, j] + "   ");
                        plain.Add(Matrix4[i, j]);
                    }
                }
            }
            if (inverse == 0)
            {
                throw new NotImplementedException();
            }
            else
            {
                string plz = "";
                for (int i = 0; i < plain.Count(); i++)
                {
                    plz += engalphabet[plain[i]];
                }
                return plz;
            }
        }
        public string Analyse(string plainText, string cipherText)
        {
            string newPlain = plainText;
            string newCipher = cipherText.ToLower();
            // string newKey = "frep";
            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> chars = new Dictionary<char, int>();
            chars.Add('a', 0);
            chars.Add('b', 1);
            chars.Add('c', 2);
            chars.Add('d', 3);
            chars.Add('e', 4);
            chars.Add('f', 5);
            chars.Add('g', 6);
            chars.Add('h', 7);
            chars.Add('i', 8);
            chars.Add('j', 9);
            chars.Add('k', 10);
            chars.Add('l', 11);
            chars.Add('m', 12);
            chars.Add('n', 13);
            chars.Add('o', 14);
            chars.Add('p', 15);
            chars.Add('q', 16);
            chars.Add('r', 17);
            chars.Add('s', 18);
            chars.Add('t', 19);
            chars.Add('u', 20);
            chars.Add('v', 21);
            chars.Add('w', 22);
            chars.Add('x', 23);
            chars.Add('y', 24);
            chars.Add('z', 25);
            List<int> plainotxt = new List<int>();
            List<int> ciphotxt = new List<int>();
            for (int i = 0; i < newCipher.Length; i++)
            {
                ciphotxt.Add(chars[newCipher[i]]);
            }
            for (int i = 0; i < newPlain.Length; i++)
            {
                plainotxt.Add(chars[newPlain[i]]);
            }
            List<int> key = new List<int>();

            List<int> plaincopy = new List<int>();
            List<int> samecipherpos = new List<int>();
            List<int> plainInvers = new List<int>();

            int determent = 0;
            int inverse = 0;
            int x = 0;
            int y = 0;
            for (x = 0; x < plainotxt.Count; x += 2)
            {
                for (y = 0; y < plainotxt.Count; y += 2)
                {

                    plaincopy.Clear();
                    plaincopy.Add(plainotxt[x]);
                    plaincopy.Add(plainotxt[y]);
                    plaincopy.Add(plainotxt[x + 1]);
                    plaincopy.Add(plainotxt[y + 1]);

                    determent = plaincopy[0] * plaincopy[3] - plaincopy[1] * plaincopy[2];

                    determent %= 26; if (determent < 0) determent += 26;

                    inverse = getinverse(determent);
                    if (inverse != 0)
                        break;

                }
                if (inverse != 0)
                    break;

            }
            Console.WriteLine(determent);
            if (inverse != 0)
            {
                int[,] planx = new int[2, 2];
                int c = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        planx[i, j] = (plaincopy[c] * inverse) % 26; if (planx[i, j] < 0) planx[i, j] += 26;
                        if ((i + j) % 2 != 0) planx[i, j] = -planx[i, j];
                        c++;
                    }
                }
                int swap = 0;
                swap = planx[0, 0];
                planx[0, 0] = planx[1, 1];
                planx[1, 1] = swap;

                samecipherpos.Add(ciphotxt[x]);
                samecipherpos.Add(ciphotxt[y]);
                samecipherpos.Add(ciphotxt[x + 1]);
                samecipherpos.Add(ciphotxt[y + 1]);
                int[,] cipher = new int[2, 2];
                c = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        cipher[i, j] = samecipherpos[c];
                        c++;
                    }
                }

                int[,] Matrix3 = new int[2, 2];
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < 2; k++)
                        {
                            Matrix3[i, j] += cipher[i, k] * planx[k, j];

                        }
                        Matrix3[i, j] = Matrix3[i, j] % 26; if (Matrix3[i, j] < 0) Matrix3[i, j] += 26;
                        key.Add(Matrix3[i, j]);
                    }
                }
                string kk = "";
                for (int i = 0; i < key.Count; i++)
                {
                    kk += engalphabet[key[i]];
                }
                return kk;
            }
            else
                throw new InvalidAnlysisException();
        }

        public string Encrypt(string plainText, string key)
        {

            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> chars = new Dictionary<char, int>();
            chars.Add('a', 0);
            chars.Add('b', 1);
            chars.Add('c', 2);
            chars.Add('d', 3);
            chars.Add('e', 4);
            chars.Add('f', 5);
            chars.Add('g', 6);
            chars.Add('h', 7);
            chars.Add('i', 8);
            chars.Add('j', 9);
            chars.Add('k', 10);
            chars.Add('l', 11);
            chars.Add('m', 12);
            chars.Add('n', 13);
            chars.Add('o', 14);
            chars.Add('p', 15);
            chars.Add('q', 16);
            chars.Add('r', 17);
            chars.Add('s', 18);
            chars.Add('t', 19);
            chars.Add('u', 20);
            chars.Add('v', 21);
            chars.Add('w', 22);
            chars.Add('x', 23);
            chars.Add('y', 24);
            chars.Add('z', 25);
            List<int> plain = new List<int>();
            List<int> keyasd = new List<int>();

            for (int i = 0; i < plainText.Length; i++)
            {
                plain.Add(chars[plainText[i]]);
                Console.WriteLine(plain[i]);
            }
            for (int i = 0; i < key.Length; i++)
            {
                keyasd.Add(chars[key[i]]);
            }

            int keylength = Convert.ToInt16(Math.Sqrt(keyasd.Count));
            Console.WriteLine(keylength);
            int[,] newkey = new int[keylength, keylength];

            int[,] newplain = new int[(plain.Count() / keylength), keylength];
            int c = 0;
            for (int i = 0; i < keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    newkey[j, i] = keyasd[c] % 26;
                    c++;
                }
            }
            c = 0;
            for (int i = 0; i < plain.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    newplain[i, j] = plain[c] % 26;
                    c++;
                }
            }
            List<int> cipherx = new List<int>(plain.Count);


            for (int i = 0; i < plain.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {

                    Console.Write("  " + newplain[i, j]);
                }
                Console.WriteLine();
            }

            int[,] Matrix3 = new int[plain.Count() / keylength, keylength];

            //divideinList(plain, Convert.ToInt32(Math.Sqrt(key.Count)));
            for (int i = 0; i < plain.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    Matrix3[i, j] = 0;
                    for (int k = 0; k < keylength; k++)
                    {
                        Matrix3[i, j] += newplain[i, k] * newkey[k, j];
                    }

                    Console.WriteLine(Matrix3[i, j] % 26);
                    cipherx.Add(Matrix3[i, j] % 26);
                }
            }

            string cipher = "";
            for (int i = 0; i < cipherx.Count(); i++)
            {
                cipher += engalphabet[cipherx[i]];
            }
            return (cipher.ToUpper());
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {

            int keylength = Convert.ToInt16(Math.Sqrt(key.Count));
            Console.WriteLine(keylength);
            int[,] newkey = new int[keylength, keylength];

            int[,] newplain = new int[(plainText.Count() / keylength), keylength];
            int c = 0;
            for (int i = 0; i < keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    newkey[j, i] = key[c] % 26;
                    c++;
                }
            }
            c = 0;
            for (int i = 0; i < plainText.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    newplain[i, j] = plainText[c] % 26;
                    c++;
                }
            }
            List<int> cipherx = new List<int>(plainText.Count);


            for (int i = 0; i < plainText.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {

                    Console.Write("  " + newplain[i, j]);
                }
                Console.WriteLine();
            }

            int[,] Matrix3 = new int[plainText.Count() / keylength, keylength];

            //divideinList(plain, Convert.ToInt32(Math.Sqrt(key.Count)));
            for (int i = 0; i < plainText.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {
                    Matrix3[i, j] = 0;
                    for (int k = 0; k < keylength; k++)
                    {
                        Matrix3[i, j] += newplain[i, k] * newkey[k, j];
                    }

                    Console.WriteLine(Matrix3[i, j] % 26);
                    cipherx.Add(Matrix3[i, j] % 26);
                }
            }
            for (int i = 0; i < plainText.Count() / keylength; i++)
            {
                for (int j = 0; j < keylength; j++)
                {


                }
            }
            return cipherx;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            string mainPlain = plain3;
            string cipherS3 = cipher3.ToLower();

            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> chars = new Dictionary<char, int>();
            chars.Add('a', 0);
            chars.Add('b', 1);
            chars.Add('c', 2);
            chars.Add('d', 3);
            chars.Add('e', 4);
            chars.Add('f', 5);
            chars.Add('g', 6);
            chars.Add('h', 7);
            chars.Add('i', 8);
            chars.Add('j', 9);
            chars.Add('k', 10);
            chars.Add('l', 11);
            chars.Add('m', 12);
            chars.Add('n', 13);
            chars.Add('o', 14);
            chars.Add('p', 15);
            chars.Add('q', 16);
            chars.Add('r', 17);
            chars.Add('s', 18);
            chars.Add('t', 19);
            chars.Add('u', 20);
            chars.Add('v', 21);
            chars.Add('w', 22);
            chars.Add('x', 23);
            chars.Add('y', 24);
            chars.Add('z', 25);
            List<int> key = new List<int>();
            List<int> plainText = new List<int>();
            List<int> cipherText = new List<int>();

            for (int i = 0; i < mainPlain.Length; i++)
            {
                plainText.Add(chars[mainPlain[i]]);
            }
            for (int i = 0; i < cipherS3.Length; i++)
            {
                cipherText.Add(chars[cipherS3[i]]);
            }


            int keylen = 0;
            if (plainText.Count() % 2 == 0) keylen = 2;
            if (plainText.Count() % 3 == 0) keylen = 3;
            int[,] plainx = new int[keylen, keylen];

            int c = 0;

            if (keylen == 3)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[j, i] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { plainx[j, i] = plainText[c]; }

                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(plainx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }

                int e1 = plainx[0, 0] * (plainx[1, 1] * plainx[2, 2] - plainx[1, 2] * plainx[2, 1]);
                int e2 = plainx[0, 1] * (plainx[1, 0] * plainx[2, 2] - plainx[1, 2] * plainx[2, 0]);
                int e3 = plainx[0, 2] * (plainx[1, 0] * plainx[2, 1] - plainx[1, 1] * plainx[2, 0]);
                int determent = (e1 - e2 + e3) % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                int inverse = 1;
                inverse = getinverse(determent);
                Console.WriteLine(inverse);
                //copy the matrix to do operations on it
                int[,] copykey = new int[keylen, keylen];
                Console.WriteLine("______________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        copykey[i, j] = getdemat(i, j);
                        Console.Write(copykey[i, j] + "   ");
                    }
                    Console.WriteLine();
                }
                //calculate the inverse matrix 
                int getdemat(int row, int col)
                {
                    int value = 0;
                    List<int> mat = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {

                        for (int j = 0; j < 3; j++)
                        {
                            if (i == row || j == col)
                            {
                                continue;
                            }
                            else
                            {
                                mat.Add(plainx[i, j]);
                            }

                        }
                    }
                    value = mat[0] * mat[3] - mat[2] * mat[1];
                    if ((row + col) % 2 != 0)
                    {
                        value = -value;
                        return value;
                    }
                    else
                    {
                        return value;
                    }
                }

                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        plainx[i, j] = (copykey[j, i] * inverse) % 26;
                        if (plainx[i, j] < 0) plainx[i, j] += 26;
                    }

                }
                Console.WriteLine("_________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(plainx[i, j] + "     ");
                    }
                    Console.WriteLine();

                }

                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * plainx[k, j];

                        }
                        //plain.Add(Matrix3[j, i]);
                        //Console.Write(Matrix3[i, j] % 26+"  ");
                    }
                }
                int[,] Matrix4 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix4[i, j] = Matrix3[i, j] % 26;
                    }
                }
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(Matrix4[i, j] + "   ");
                        key.Add(Matrix4[i, j]);
                    }
                }

            }
            string kk = "";
            for (int i = 0; i < key.Count(); i++)
            {
                kk += engalphabet[key[i]];
            }

            if (kk.Length > 1)
            {
                return kk;
            }
            else
                throw new InvalidAnlysisException();

        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();

            int keylen = 0;
            if (plainText.Count() % 3 == 0) keylen = 3;
            int[,] plainx = new int[keylen, keylen];

            int c = 0;

            if (keylen == 3)
            {
                int[,] cipherx = new int[cipherText.Count() / keylen, keylen];

                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        cipherx[j, i] = cipherText[c];
                        Console.Write(cipherx[i, j] % 26 + "    ");
                        c++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("_____________");
                c = 0;
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        if (keylen == 3) { plainx[j, i] = plainText[c]; }

                        c++;
                    }

                }
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Console.Write(plainx[i, j] % 26 + "   ");
                    }
                    Console.WriteLine();

                }

                int e1 = plainx[0, 0] * (plainx[1, 1] * plainx[2, 2] - plainx[1, 2] * plainx[2, 1]);
                int e2 = plainx[0, 1] * (plainx[1, 0] * plainx[2, 2] - plainx[1, 2] * plainx[2, 0]);
                int e3 = plainx[0, 2] * (plainx[1, 0] * plainx[2, 1] - plainx[1, 1] * plainx[2, 0]);
                int determent = (e1 - e2 + e3) % 26; if (determent < 0) determent += 26;
                Console.WriteLine(determent);
                int inverse = 1;
                inverse = getinverse(determent);
                Console.WriteLine(inverse);
                //copy the matrix to do operations on it
                int[,] copykey = new int[keylen, keylen];
                Console.WriteLine("______________________");
                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        copykey[i, j] = getdemat(i, j);
                        Console.Write(copykey[i, j] + "   ");
                    }
                    Console.WriteLine();
                }
                //calculate the inverse matrix 
                int getdemat(int row, int col)
                {
                    int value = 0;
                    List<int> mat = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {

                        for (int j = 0; j < 3; j++)
                        {
                            if (i == row || j == col)
                            {
                                continue;
                            }
                            else
                            {
                                mat.Add(plainx[i, j]);
                            }

                        }
                    }
                    value = mat[0] * mat[3] - mat[2] * mat[1];
                    if ((row + col) % 2 != 0)
                    {
                        value = -value;
                        return value;
                    }
                    else
                    {
                        return value;
                    }
                }

                for (int i = 0; i < keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        plainx[i, j] = (copykey[j, i] * inverse) % 26;
                        if (plainx[i, j] < 0) plainx[i, j] += 26;
                    }

                }
               

                int[,] Matrix3 = new int[cipherText.Count() / keylen, keylen];
                for (int i = 0; i < cipherText.Count() / keylen; i++)
                {
                    for (int j = 0; j < keylen; j++)
                    {
                        Matrix3[i, j] = 0;
                        for (int k = 0; k < keylen; k++)
                        {
                            Matrix3[i, j] += cipherx[i, k] * plainx[k, j];

                        }
                        Matrix3[i, j] = Matrix3[i, j] % 26;if (Matrix3[i, j] < 0) Matrix3[i, j] += 26;
                        key.Add(Matrix3[i, j]);
                    }
                }
              

            }
            return key;
        }


    }
}
