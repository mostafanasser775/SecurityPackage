using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        Dictionary<char, int> chars = new Dictionary<char, int>();
        public Monoalphabetic()
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

        public static string RemoveDuplicates(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }
        public string EngAlphabet = "abcdefghijklmnopqrstuvwxyz";
        public string Analyse(string plainText, string cipherText)
        {

            

            string EngAlphabet = "abcdefghijklmnopqrstuvwxyz";
            string EngAlphabet2 = " abcdefghijklmnopqrstuvwxyz";


            string mainPlain = plainText.ToLower();
            string mainCipher = cipherText.ToLower();


            string x = RemoveDuplicates(mainPlain);
            string y = RemoveDuplicates(mainCipher);

           
            Dictionary<char, char> encryptchars = new Dictionary<char, char>();
            for (int i = 0; i < x.Length; i++)
            {
                if (char.IsLetter(x[i]) && char.IsLetter(y[i]))
                {
                    encryptchars.Add(x[i], y[i]);
                }
                else
                    continue;
            }

            char[] charArray = new char[26];
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = '*';
            }
            Console.WriteLine(charArray.Count());

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    if (EngAlphabet[i] == x[j])
                    {
                        charArray[i] = y[j];
                    }
                }
            }
            int gett = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (i != 0 && charArray[i - 1] != '*' && charArray[i] == '*')
                {
                    gett = (chars[charArray[i - 1]]) + 1;
                    if (gett == 27) gett = 1;
                    for (int k = 0; k < 26; k++)
                    {
                        if (EngAlphabet2[gett] == charArray[k])
                        {
                            gett++;
                            if (gett == 27) gett = 1;
                            k = -1;
                        }
                    }
                    charArray[i] = EngAlphabet2[gett];

                    Console.WriteLine(gett);
                }
            }
            string word = "";
            for (int i = 0; i < 26; i++)
            {
                word += charArray[i];
            }
            Console.WriteLine(word);
            word = RemoveDuplicates(word);
            Console.WriteLine(word.Count());
            return word;
}

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string Dword = "";
            Dictionary<char, char> Decryption = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                if (char.IsLetter(EngAlphabet[i]) && char.IsLetter(key[i]))
                {
                    Decryption.Add(key[i], EngAlphabet[i]);
                }
            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                Dword += Decryption[cipherText[i]];
            }
            return Dword;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();
            string word = "";
            Dictionary<char, char> encryptchars = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++) {
                if (char.IsLetter(EngAlphabet[i]) && char.IsLetter(key[i]))
                {
                    encryptchars.Add(EngAlphabet[i], key[i]);
                }
            }
            for (int i = 0; i < plainText.Length; i++) {
                word += encryptchars[plainText[i]];
            }
            return word;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string EngAlphabet = "abcdefghijklmnopqrstuvwxyz";
            string wordfreq = "ETAOINSRHLDCUMFPGWYBVKXJQZ".ToLower();

            Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();
            int count = 0;
            string word = cipher.ToLower();
            for (int i = 0; i < 26; i++)
            {
                count = 0;
                char trim = EngAlphabet[i];
                for (int j = 0; j < word.Length; j++)
                {
                    if (trim == word[j])
                    {
                        count++;
                    }
                }
                foreach (char c in word)
                {
                    word = word.Replace(trim.ToString(), string.Empty);
                }
                keyValuePairs.Add(trim, count);
            }
            word = cipher.ToLower();
            int step = 0;


            char[] charArray = new char[cipher.Length];
            for (int i = 0; i < cipher.Length; i++)
            {
                charArray[i] = word[i];

            }


            foreach (KeyValuePair<char, int> author in keyValuePairs.OrderByDescending(key => key.Value))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == author.Key)
                    {
                        charArray[i] = wordfreq[step];
                    }
                }
                step++;
                Console.WriteLine("Key: {0}, Value: {1}", author.Key, author.Value);
            }

            string keykey = "";
            foreach (char c in charArray)
            {
                keykey += c;
            }
            return keykey;

        }
    }
}
