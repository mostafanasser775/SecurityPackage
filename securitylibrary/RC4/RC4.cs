using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        public static string check_Hex_Text(string word)
        {
            string pop = "";
            if (word[0] == '0' && word[1] == 'x')
            {
                for (int i = 2; i < word.Length - 1; i += 2)
                {
                    int Convint = Convert.ToInt32(word[i].ToString() + word[i + 1].ToString(), 16);
                    pop += (char)Convint;
                }
            }
            else
                pop = word;
            return pop;
        }

        public override string Decrypt(string cipherText, string key)
        {
            return Encrypt(cipherText, key);

            
        }

        public override  string Encrypt(string plainText, string key)
        {
            bool hex = false;
            if (plainText[0] == '0' && plainText[1] == 'x') hex = true;
            plainText = check_Hex_Text(plainText);
            key = check_Hex_Text(key);

            Console.WriteLine(plainText);
            Console.WriteLine(key);
            //now we have both key and plaintext with no issues
            // create the S and T
            int[] S = new int[256];
            int[] T = new int[256];
            int j = 0;
            int i = 0;
            for (i = 0; i < 256; i++)
            {
                S[i] = i;
                T[i] = key[i % key.Length];
            }
            //j=0
            //for 0 to 255
            //j=(j+S[i]+T[i])mod 255
            // swap(S[i] and S[J])

            int swap;
            for (i = 0; i < 256; i++)
            {
                swap = 0;
                j = (j + S[i] + T[i]) % 256;
                swap = S[i];
                S[i] = S[j];
                S[j] = swap;
            }
            //Now we generate 3-bits at a time, k, that we XOR with each 3-bits of plaintext to produce the 
            //ciphertext
            //i,j=0
            //for len of text
            // i = (i + 1) mod 256; 
            //j = (j + S[i]) mod 256;
            //Swap(S[i], S[j]);
            //t = (S[i] + S[j]) mod 256;
            //xor between plaintext and s[t]
            i = 0; j = 0;
            string cipher = "";
            for (int count = 0; count < plainText.Length; count++)
            {
                i = (i + 1) % 256;
                j = (j + S[i]) % 256;
                swap = S[i];
                S[i] = S[j];
                S[j] = swap;
                cipher += (char)(plainText[count] ^ (S[((S[i] + S[j]) % 256)]));
            }
            //now check if the input was hexadecimal we convert it again to its form
            if (hex==true) {
                byte[] bytes = Encoding.Default.GetBytes(cipher);
                string hexString = BitConverter.ToString(bytes);
                hexString = hexString.Replace("-", "");
                cipher = ("0x" + hexString.ToLower());

            }
           
            return cipher;
        }
    }
}
