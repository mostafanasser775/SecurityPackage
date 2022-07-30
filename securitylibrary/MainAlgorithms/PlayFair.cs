using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public static string RemoveDuplicates(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }
        public string Decrypt(string cipherText, string key)
        {
            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            char[,] arr = new char[5, 5];
            string mainPlain = cipherText.ToLower();
            string mainKey = key.ToLower();
            mainKey = RemoveDuplicates(mainKey);
            string word = mainKey + engalphabet;
            word = RemoveDuplicates(word);
            Console.WriteLine(mainKey);
            Console.WriteLine(word);
            string plaintext = word;
            mainPlain = mainPlain.Replace('j', 'i');
            plaintext = plaintext.Replace('j'.ToString(), String.Empty);
            Console.WriteLine(plaintext);

            int kk = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    arr[i, j] = plaintext[kk];
                    kk++;

                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write("   " + arr[i, j]);

                }
                Console.WriteLine("");
            }


            Console.WriteLine(mainPlain.Count());
            string mmm = "";
            int count2 = 2;

            Console.WriteLine(mmm);


            int row1 = -1;
            int col1 = -1;
            int row2 = -1;
            int col2 = -1;
            string cipher = "";
            for (int i = 0; i < mainPlain.Length; i++)
            {

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (mainPlain[i] == arr[j, k])
                        {
                            row1 = j;
                            col1 = k;
                        }
                    }
                }
                i = i + 1;
                if (i < mainPlain.Length)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (mainPlain[i] == arr[j, k])
                            {
                                row2 = j;
                                col2 = k;
                            }
                        }
                    }
                }

                if (row1 == row2)
                {
                    if (col1 == 0) col1 = 5;
                    if (col2 == 0) col2 = 5;

                    cipher += arr[row1, (col1 - 1)];
                    cipher += arr[row2, (col2 - 1)];


                }
                else if (col1 == col2)
                {
                    if (row1 == 0) row1 = 5;
                    if (row2 == 0) row2 = 5;

                    cipher += arr[(row1 - 1), col1];
                    cipher += arr[(row2 - 1), col2];

                }
                else
                {
                    cipher += arr[row1, col2];
                    cipher += arr[row2, col1];

                }


            }

            if (cipher[cipher.Length - 1] == 'x') {
                cipher = cipher.Remove(cipher.Length - 1);
            }
            
            
            Console.WriteLine(cipher);

            int countx = 0;
            string Dword = cipher;
           
                for (int i = 0; i < cipher.Length; i++)
                {
                    if (i > 0 && i < cipher.Length + 1)
                    {
                        if (Dword[i] == 'x' && Dword[i - 1] == Dword[i + 1])
                        {
                            if ((i ) % 2 != 0)
                            {
                                cipher = cipher.Remove(i - countx, 1);
                                countx++;
                            
                            }
                        }
                    }
                    
                }

            return cipher;

        }

        public string Encrypt(string plainText, string key)
        {
            string engalphabet = "abcdefghijklmnopqrstuvwxyz";
            char[,] arr = new char[5, 5];
            string mainPlain = plainText.ToLower();
            string mainKey = key.ToLower();
            mainKey = RemoveDuplicates(mainKey);
            string word = mainKey + engalphabet;
            word = RemoveDuplicates(word);
            Console.WriteLine(mainKey);
            Console.WriteLine(word);
            string plaintext = word;
            mainPlain = mainPlain.Replace('j', 'i');
            plaintext = plaintext.Replace('j'.ToString(), String.Empty);
            Console.WriteLine(plaintext);

            int kk = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    arr[i, j] = plaintext[kk];
                    kk++;

                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write("   " + arr[i, j]);

                }
                Console.WriteLine("");
            }
            string newplain = "";
            int length = mainPlain.Length;
            int count = 2;

            for (int i = 0; i < length; i++)
            {

            }
            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    if (i + 1 < length)
                    {
                        if (mainPlain[i] == mainPlain[i + 1])
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                newplain += mainPlain[j];
                            }
                            newplain += "x";
                            if (i + 1 != length)
                            {
                                for (int j = i + 1; j < length; j++)
                                {
                                    newplain += mainPlain[j];
                                }
                            }
                            mainPlain = newplain;
                            newplain = "";
                            length = mainPlain.Length;
                            i = -1;
                        }
                    }
                    i++;
                }
            }

            Console.WriteLine(mainPlain.Count());
            if (mainPlain.Count() % 2 != 0)
            {
                mainPlain = mainPlain + 'x';
            }
            string mmm = "";
            int count2 = 2;
            for (int i = 0; i < mainPlain.Length; i++)
            {
                if (count2 == 0)
                {
                    mmm += "|";
                    count2 = 2;

                }
                count2--;

                mmm += mainPlain[i];
            }
            Console.WriteLine(mmm);


            int row1 = -1;
            int col1 = -1;
            int row2 = -1;
            int col2 = -1;
            string cipher = "";
            for (int i = 0; i < mainPlain.Length; i++)
            {

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (mainPlain[i] == arr[j, k])
                        {
                            row1 = j;
                            col1 = k;
                        }
                    }
                }
                i = i + 1;
                if (i < mainPlain.Length)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (mainPlain[i] == arr[j, k])
                            {
                                row2 = j;
                                col2 = k;
                            }
                        }
                    }
                }
                if (row1 == row2)
                {
                    cipher += arr[row1, (col1 + 1) % 5];
                    cipher += arr[row2, (col2 + 1) % 5];


                }
                else if (col1 == col2)
                {
                    cipher += arr[(row1 + 1) % 5, col1];
                    cipher += arr[(row2 + 1) % 5, col2];

                }
                else
                {
                    cipher += arr[row1, col2];
                    cipher += arr[row2, col1];

                }









            }



            Console.WriteLine(cipher);

            return cipher.ToLower();
        }
    }
}
