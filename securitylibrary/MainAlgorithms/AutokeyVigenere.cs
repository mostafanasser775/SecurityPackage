using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public string Analyse(string plainText, string cipherText)
        {

            string cipher_Text = cipherText.ToLower(), plain_Text = plainText.ToLower();
            //  string cipher_Text = "jsxaivsd"; string plain_Text = "computer";
            Dictionary<Tuple<char, char>, char> table = GetTheTable();
            int size = cipher_Text.Length;
            char addToKey = 'x';
            bool start_search = false;
            char[] KeyArray = new char[cipher_Text.Length];
            for (int c = 0; c < cipher_Text.Length; c++)
            {

                for (int ii = 0; ii < 26; ii++) //foreach (var charac in alphabet)
                {
                    if (plain_Text[c] == alphabet[ii])
                    {
                        start_search = true;
                        for (int i = 0; true; i++)
                        {
                            if (alphabet[(ii + i) % 26] == cipher_Text[c])
                            {
                                addToKey = alphabet[i];
                                break;
                            }//if
                        }
                    }
                    if (start_search)
                    {
                        start_search = false;
                        break;
                    }
                }//for each


                KeyArray[c] = addToKey;
            }
            string d = cutExtraLettersFromKey(plain_Text, KeyArray);
            if (d != "") return d;
            string finalword = new string(KeyArray);
            return finalword;
        }
        public string cutExtraLettersFromKey(string plainText, char[] KeyArray)
        {

            int extralength = 0;
            int counter; int ii;
            bool thereIsExtra = false;
            for (int i = 0; i < KeyArray.Length; i++)
            {
                extralength = 0;
                if (KeyArray[i] == plainText[0])
                {
                    counter = i;
                    for (ii = 0; counter < KeyArray.Length; ii++)
                    {

                        if (KeyArray[counter] == plainText[ii % plainText.Length])
                            extralength++;

                        else
                        { break; }//else
                        counter++;

                    }//ii
                    if (counter == KeyArray.Length)
                    {
                        char[] Keyfinal = new char[KeyArray.Length - extralength];
                        for (int p = 0; p < KeyArray.Length - extralength; p++)
                            Keyfinal[p] = KeyArray[p];
                        string finalword = new string(Keyfinal);
                        return finalword;
                    }
                }//if

            }
            return "";
        }
        public string Decrypt(string cipherText, string key)
        {

            //  string cipher_Text = "jsxaivsd", keyy = "hello";
            string cipher_Text = cipherText.ToLower(), keyy = key.ToLower();
            Dictionary<Tuple<char, char>, char> table = GetTheTable();
            int size = cipher_Text.Length;
            char addToKey = 'x';

            char[] KeyArray = new char[cipher_Text.Length];
            for (int c = 0; c < keyy.Length; c++)
            {

                foreach (var charac in alphabet)
                {
                    addToKey = table[Tuple.Create(keyy[(c)], charac)];
                    if (cipher_Text[c] == addToKey)
                    {
                        addToKey = charac;
                        break;
                    }
                }//for each


                KeyArray[c] = addToKey;
            }
            if (cipher_Text.Length > keyy.Length)
            {
                int theDiffrenceBetweenCTandKey = cipher_Text.Length - keyy.Length;
                for (int i = keyy.Length; i < cipher_Text.Length; i++)
                {
                    foreach (var charac in alphabet)
                    {
                        addToKey = table[Tuple.Create(KeyArray[(i - keyy.Length)], charac)];
                        if (cipher_Text[i] == addToKey)
                        {
                            addToKey = charac;
                            break;
                        }
                    }//for each


                    KeyArray[i] = addToKey;
                }

            }
            string finalword = new string(KeyArray);
            return finalword;

        }

        public string Encrypt(string plainText, string key)
        {
            string plainTextt = plainText.ToLower(), keyy = key.ToLower(), thekey;
            // string plainTextt = "computer", keyy = "hello", thekey;

            Dictionary<Tuple<char, char>, char> table = GetTheTable();
            int size = plainTextt.Length, indexforfillingtherestofthekey = 0;
            char addToKey;
            char[] Keywithextra = new char[plainTextt.Length];
            if (keyy.Length < plainTextt.Length)
            {
                for (int c = 0; c < keyy.Length; c++)
                { Keywithextra[c] = keyy[c]; }
                for (int c = keyy.Length; c < plainTextt.Length; c++)
                { Keywithextra[c] = plainTextt[indexforfillingtherestofthekey]; indexforfillingtherestofthekey++; }
            }
            keyy = new string(Keywithextra);
            char[] KeyArray = new char[plainTextt.Length];
            for (int c = 0; c < plainTextt.Length; c++)
            {
                addToKey = table[Tuple.Create(keyy[(c)], plainTextt[c])];
                // addToKey = table[Tuple.Create(plainTextt[c], key[(c)% key.Length])];

                KeyArray[c] = addToKey;
            }
            string finalword = new string(KeyArray);
            return finalword;
        }
        public Dictionary<Tuple<char, char>, char> GetTheTable()
        {

            Dictionary<string, string> phonebook = new Dictionary<string, string>();


            Tuple<string, int> t = new Tuple<string, int>("Hello", 4);
            Dictionary<Tuple<int, int>, string> the_modern_vigenere_tableau = new Dictionary<Tuple<int, int>, string>();
            the_modern_vigenere_tableau[Tuple.Create(1, 3)] = "Hello";
            string s = the_modern_vigenere_tableau[Tuple.Create(1, 3)];
            char[] alaphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Dictionary<Tuple<char, char>, char> the_modern_vigenere_tablea = new Dictionary<Tuple<char, char>, char>();
            int counter = 0;
            for (int row = 0; row < 26; row++)
            {
                for (int col = 0; col < 26; col++)
                {
                    the_modern_vigenere_tablea[Tuple.Create(alaphabet[row], alaphabet[col])] = alaphabet[(counter + col) % 26];

                }//c
                counter++;
            }//r
            return the_modern_vigenere_tablea;
        }
    }
}
