using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            string cipher_Text = cipherText.ToLower();
            string plain_text = plainText.ToLower();
            //  cipher_Text = "ctipscoeemrnuce"; plain_text = "computer science";
            cipher_Text = rmoveSpac(cipher_Text);
            plain_text = rmoveSpac(plain_text);
            List<List<int>> myList = new List<List<int>>();
            bool collectingonepart = false;
            int remembermyvalue_iami = 0;
            for (int i = 0; i < cipher_Text.Length; i++)
            {
                List<int> part = new List<int>();
                collectingonepart = false;
                remembermyvalue_iami = i;
                for (int j = 0; ((j < plain_text.Length) && (i < cipher_Text.Length)); j++)
                {

                    if (cipher_Text[i] == plain_text[j])
                    {
                        collectingonepart = true;
                        part.Add(j); i++;

                    }
                    /*  else if (collectingonepart)
                      { i--; break;}*/

                }//j
                myList.Add(part);
                i = remembermyvalue_iami + part.Count - 1;
            }

            decimal e = plain_text.Length, ee = myList.Count;
            decimal roundednumber = Math.Ceiling(e / ee);

            int rows = Decimal.ToInt32(roundednumber), columns = myList.Count, it = 0;
            char[,] table = new char[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; ((col < columns) && (it < plain_text.Length)); col++)
                {
                    table[row, col] = plain_text[it];
                    it++;
                }//columns;

            }//row
            List<List<char>> partofthecipher = new List<List<char>>();
            it = 0;
            for (int col = 0; col < columns; col++)
            {
                List<char> onepart = new List<char>();
                for (int row = 0; ((row < rows) && (it < plain_text.Length)); row++)
                {
                    if (table[row, col] == '\0')
                        continue;
                    onepart.Add(table[row, col]);
                    it++;
                }//r;
                partofthecipher.Add(onepart);
            }//c
            List<int> orders = new List<int>();
            for (int i = 0; i < partofthecipher.Count; i++)
            {
                orders.Add(findthispartorder(cipher_Text, partofthecipher[i], rows));
            }
            return orders;
        }

        public string Decrypt(string cipherText, List<int> key)
        {

            string cipher_Text = cipherText.ToLower();
            //       cipher_Text="ctipscoeemrnuce";// plain_text = "computer science";
            cipher_Text = rmoveSpac(cipher_Text);
            List<int> the_key = key;
            //       var the_key = new List<int>() { 1, 3, 4,       2,5 };
            //    var the_key = new List<int>() { 0,0,0,0,0 };

            //  List<int> the_key = new List<int>();// thekey;
            //    for (int i = 0; i < thekey.Count; i++) the_key.Add(0);
            //    for (int i = 0; i < thekey.Count; i++) the_key[thekey[i] - 1] = i + 1;

            decimal e = cipher_Text.Length, ee = the_key.Count;
            decimal roundednumber = Math.Ceiling(e / ee);
            int columns = the_key.Count, rows = Decimal.ToInt32(roundednumber), it = 0;

            char[,] table = new char[rows, columns];
            for (int col = 0; ((col < columns) && (it < cipher_Text.Length)); col++)
            {
                for (int row = 0; ((row < rows) && (it < cipher_Text.Length)); row++)

                {
                    table[row, col] = cipher_Text[it];
                    it++;
                }//columns;

            }//row
            ///read it
            char[] plaintext = new char[cipher_Text.Length];
            it = 0;
            for (int row = 0; ((row < rows) && (it < cipher_Text.Length)); row++)

            {
                for (int col = 0; col < columns; col++)
                {
                    if (table[row, the_key[col] - 1] == '\0')
                        continue;
                    plaintext[it] = table[row, the_key[col] - 1];
                    it++;
                }//r;

            }//c
            string finalword = new string(plaintext);
            return finalword;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            string plain_text = plainText.ToLower();
            //  plain_text = "computer science";
            plain_text = rmoveSpac(plain_text);
            List<int> thekey = key;
            //            var thekey = new List<int>() { 1, 3, 4,       2,5 };
            //    var the_key = new List<int>() { 0,0,0,0,0 };

            List<int> the_key = new List<int>();// thekey;
            for (int i = 0; i < thekey.Count; i++) the_key.Add(0);
            for (int i = 0; i < thekey.Count; i++) the_key[thekey[i] - 1] = i + 1;

            decimal e = plain_text.Length, ee = thekey.Count;
            decimal roundednumber = Math.Ceiling(e / ee);
            int columns = thekey.Count, rows = Decimal.ToInt32(roundednumber), it = 0;

            char[,] table = new char[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; ((col < columns) && (it < plain_text.Length)); col++)
                {
                    table[row, col] = plain_text[it];
                    it++;
                }//columns;

            }//row
            ///read it
            char[] ciphertext = new char[plain_text.Length];
            it = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; ((row < rows) && (it < plain_text.Length)); row++)
                {
                    if (table[row, the_key[col] - 1] == '\0')
                        continue;
                    ciphertext[it] = table[row, the_key[col] - 1];
                    it++;
                }//r;

            }//c
            string finalword = new string(ciphertext);
            return finalword;

        }
        public string rmoveSpac(string plainText)
        {
            char[] newstr = new char[plainText.Length];
            int counter = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == ' ')
                    continue;
                newstr[counter] = plainText[i];
                counter++;

            }//for
            char[] newsizedstr = new char[counter];
            for (int i = 0; i < counter; i++)
                newsizedstr[i] = newstr[i];

            string finalword = new string(newsizedstr);
            return finalword;
        }
        public int findthispartorder(string cipherText, List<char> onepart, int rows)
        { //List<char> onepart = new List<char>();
          //  char[] newstr = new char[plainText.Length];
            int counter = 0;
            bool inthrpro = false;
            int thebeg = 0;
            for (int i = 0; ((counter < onepart.Count) && (i < cipherText.Length)); i++)
            {

                if (cipherText[i] == onepart[counter])
                {
                    inthrpro = true;
                    counter++;
                    thebeg = i;
                }
                else if (inthrpro)
                {
                    i = thebeg; counter = 0; inthrpro = false;
                }


            }//for 
            thebeg++;
            return (thebeg / rows);
        }
    }
}
