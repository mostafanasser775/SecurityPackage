using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        Dictionary<char, int> chars = new Dictionary<char, int>();
        public Ceaser()
        {
            chars.Add('a', 1);
            chars.Add('b', 2);
            chars.Add('c', 3);
            chars.Add('d', 4);
            chars.Add('e', 5);
            chars.Add('f', 6);
            chars.Add('g', 7);
            chars.Add('h', 8);
            chars.Add('i', 9);
            chars.Add('j', 10);
            chars.Add('k', 11);
            chars.Add('l', 12);
            chars.Add('m', 13);
            chars.Add('n', 14);
            chars.Add('o', 15);
            chars.Add('p', 16);
            chars.Add('q', 17);
            chars.Add('r', 18);
            chars.Add('s', 19);
            chars.Add('t', 20);
            chars.Add('u', 21);
            chars.Add('v', 22);
            chars.Add('w', 23);
            chars.Add('x', 24);
            chars.Add('y', 25);
            chars.Add('z', 26);

        }
        public string Encrypt(string plainText, int key)
        {
            int length = plainText.Length;
            string word = "";
            for (int i = 0; i < length; i++)
            {
                if (char.IsLetter(plainText[i]))
                {
                    int index;
                    index = chars[plainText[i]];
                    index = index + key;
                    if (index > 26)
                    {
                        index = index % 26;
                    }
                    foreach (var krp in chars)
                    {
                        if (index == krp.Value)
                        {
                            word += krp.Key;
                        }
                    }
                }
                else
                {
                    word += plainText[i];
                }

            }
            return word;

        }
        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.ToLower();
            int cindex;
            int Len = cipherText.Length;
            string Dword = ""; 
            for (int i = 0; i < Len; i++)
            {
                if (char.IsLetter(cipherText[i]))
                {
                    cindex = chars[cipherText[i]] - key;
                    if (cindex <= 0) cindex += 26;
                    foreach (var kvp in chars)
                    {
                        if (kvp.Value == cindex)
                        {
                            Dword += kvp.Key;
                        }
                    }
                }
                else
                {
                    Dword += cipherText[i];
                }
            }

            return Dword;
        }
        public int Analyse(string plainText, string cipherText)
        {
            plainText=plainText.ToLower();
            cipherText=cipherText.ToLower();
            

            if (plainText.Length != cipherText.Length) return -1;
            int plainindex = chars[plainText[0]];
            int cipherindex = chars[char.ToLower(cipherText[0])];
            if (cipherindex >= plainindex)
            {
                return cipherindex - plainindex;
            }
            else return ((26-plainindex)+cipherindex);
          
        }
    }
}
