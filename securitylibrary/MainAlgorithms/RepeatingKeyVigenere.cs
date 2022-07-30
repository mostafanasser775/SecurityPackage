using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {

        private char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public string Analyse(string plainText, string cipherText)
        {
            string cipher_Text = cipherText.ToLower(), plain_Text = plainText.ToLower();
            //  cipher_Text = "jsxaiaic"; plain_Text = "computer";
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
            //key length
            bool make_sure = false;
            int extralength = 0, counter = 0, startofrepeating = 0;
            string finalkey = "";
            string finaldcword = new string(KeyArray);
            bool cutextra = recbool(cipher_Text, KeyArray);
            while (cutextra)
            {
                if (cutextra)
                { finalkey = rec(cipher_Text, KeyArray); cipher_Text = finalkey; }
                cutextra = recbool(finalkey, KeyArray);

            }
            if (finalkey != "")
                return finalkey;
            return rec(cipher_Text, KeyArray); ;
        }

        public string Decrypt(string cipherText, string key)
        {
            string cipher_Text = cipherText.ToLower(), keyy = key;
            Dictionary<Tuple<char, char>, char> table = GetTheTable();
            int size = cipher_Text.Length;
            char addToKey = 'x';

            char[] KeyArray = new char[cipher_Text.Length];
            for (int c = 0; c < cipher_Text.Length; c++)
            {

                foreach (var charac in alphabet)
                {
                    addToKey = table[Tuple.Create(keyy[(c) % keyy.Length], charac)];
                    if (cipher_Text[c] == addToKey)
                    {
                        addToKey = charac;
                        break;
                    }
                }//for each


                KeyArray[c] = addToKey;
            }
            string finalword = new string(KeyArray);
            return finalword;
        }

        public string Encrypt(string plainText, string key)
        {
            string plainTextt = plainText, keyy = key;
            Dictionary<Tuple<char, char>, char> table = GetTheTable();
            int size = plainTextt.Length;
            char addToKey;

            char[] KeyArray = new char[plainTextt.Length];
            for (int c = 0; c < plainTextt.Length; c++)
            {
                addToKey = table[Tuple.Create(keyy[(c) % keyy.Length], plainTextt[c])];
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
        public string rec(string cipher_Text, char[] KeyArray)
        {
            bool make_sure = false;
            int extralength = 0, counter = 0, startofrepeating = 111110;
            for (int i = 0; (i < cipher_Text.Length); i++)
            {
                // for (int ii = i + 1; (ii < cipher_Text.Length) && (startofrepeating + startofrepeating > ii); ii++)
                for (int ii = i + 1; (ii < cipher_Text.Length) && (ii + i < cipher_Text.Length); ii++)
                {

                    if (make_sure && KeyArray[i + ii] == KeyArray[counter])
                    {
                        extralength++;
                        counter++;
                    }
                    else if (KeyArray[i] == KeyArray[ii] && make_sure == false)
                    {
                        extralength++;
                        counter = i + 1;
                        make_sure = true;
                        startofrepeating = ii;
                    }


                }
                if (make_sure && (extralength + startofrepeating == cipher_Text.Length))
                {

                    break;
                }
                else if (make_sure)
                {
                    if ((KeyArray.Length > extralength + startofrepeating) && (KeyArray[extralength + startofrepeating] == KeyArray[0]))
                    {
                        char[] Key_Arrays = new char[KeyArray.Length - extralength];
                        int io = 0;
                        for (io = 0; io < startofrepeating; io++)
                            Key_Arrays[io] = KeyArray[io];
                        for (int ioo = io; ioo < KeyArray.Length; ioo++)
                        { Key_Arrays[io] = KeyArray[ioo]; io++; }
                        string finaldword = new string(Key_Arrays);
                        return finaldword;
                    }
                    break;
                }
                counter = 0;
                make_sure = false;
                extralength = 0;
            }
            if (make_sure)
            {
                char[] Key_Array = new char[cipher_Text.Length - extralength];

                for (int i = 0; i < cipher_Text.Length - extralength; i++)
                    Key_Array[i] = KeyArray[i];
                string finaldword = new string(Key_Array);
                return finaldword;
            }
            string finalword = new string(KeyArray);
            return finalword;
        }//rec
        public bool recbool(string cipher_Text, char[] KeyArray)
        {
            bool make_sure = false;
            int extralength = 0, counter = 0, startofrepeating = 0;
            for (int i = 0; (i < cipher_Text.Length); i++)
            {
                for (int ii = i + 1; (ii < cipher_Text.Length) && (ii + i < cipher_Text.Length); ii++)
                {
                    if (make_sure && KeyArray[i + ii] == KeyArray[counter])
                    {
                        extralength++;
                        counter++;
                    }
                    else if (KeyArray[i] == KeyArray[ii] && make_sure == false)
                    {
                        extralength++;
                        counter = i + 1;
                        make_sure = true;
                        startofrepeating = ii;
                    }


                }
                if (make_sure && (extralength + startofrepeating == cipher_Text.Length))
                {
                    make_sure = true;
                    break;
                }
                counter = 0;
                make_sure = false;
                extralength = 0;
            }
            if (make_sure)
            {
                char[] Key_Array = new char[cipher_Text.Length - extralength];

                for (int i = 0; i < cipher_Text.Length - extralength; i++)
                    Key_Array[i] = KeyArray[i];
                string finaldword = new string(Key_Array);
                return true;
            }
            string finalword = new string(KeyArray);
            return false;
        }//recbool
        public string reco(string cipher_Text)
        {
            char[] KeyArray;
            for (int i = 0; i < cipher_Text.Length; i++)
                i++;
            return "";

        }
    }
}