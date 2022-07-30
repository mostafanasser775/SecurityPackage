using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            string cipher_Text = cipherText.ToLower();
            string plain_Text = plainText.ToLower();
            int key = 1, countTheSimi = 0, ii, normalindex = 0, counter = 0;
            string hold_the_cipher = "", hold_the_plain = "";
            while (hold_the_plain.Substring(0, hold_the_plain.Length - counter) != plain_Text)
            {
                counter = 0;
                key++;
                hold_the_plain = Decrypt(cipher_Text, key);
                for (int i = hold_the_plain.Length - 1; i >= 0; i--)
                {
                    if (hold_the_plain[i] == 'x')
                        counter++;
                    else
                        break;
                }

            }
            return key;
            /* while (hold_the_cipher!=cipher_Text)
             {
                 key++;
              hold_the_cipher=Encrypt(plain_Text,key);

              }
             return key;*/
        }

        public string Decrypt(string cipherText, int key)
        {
            string cipher_Text = cipherText.ToLower();
            int thekey = key;
            //    plain_Text = "computer science"; thekey = 2;
            //  cipher_Text = "cmuesineoptrcec"; thekey = 2;
            cipher_Text = rmoveSpac(cipher_Text);
            int number_of_col = cipher_Text.Length / thekey, remainder = cipher_Text.Length % thekey;
            remainder = thekey - remainder;
            if ((cipher_Text.Length / thekey) != (number_of_col * 2))
                number_of_col++;

            char[] newstr = new char[cipher_Text.Length];
            int indexForMyNewWord = 0, counter = 0;


            char[,] array6 = new char[thekey, number_of_col];
            for (int ii = 0; (ii < thekey); ii++)
            {
                for (int i = 0; i < number_of_col; i++)
                {
                    if ((remainder != 0) && (thekey <= remainder + ii) && (i + 1 == number_of_col))
                        continue;
                    array6[ii, i] = cipher_Text[counter];
                    counter++;


                }//i
            }//ii
            counter = 0;
            for (int ii = 0; (ii < number_of_col); ii++)
            {
                for (int i = 0; ((i < thekey) && (counter < cipher_Text.Length)); i++)
                {
                    newstr[counter] = array6[i, ii];
                    counter++;

                }//i
            }//ii
             ///////////////////////////////////////////////////////

            string finalword = new string(newstr);
            return finalword;
        }

        public string Encrypt(string plainText, int key)
        {
            string plain_Text = plainText.ToLower();
            int thekey = key;
            //    plain_Text = "computer science"; thekey = 2;
            plain_Text = rmoveSpac(plain_Text);

            int number_of_col = plain_Text.Length / thekey;
            if ((plain_Text.Length / thekey) != (number_of_col * 2))
                number_of_col++;

            char[] newstr = new char[plain_Text.Length];
            int indexForMyNewWord = 0;
            for (int i = 0; i < thekey; i++)
            {
                for (int ii = i; ii < plain_Text.Length; ii = ii + thekey)
                {

                    newstr[indexForMyNewWord] = plain_Text[ii];
                    indexForMyNewWord++;

                }//forinner


            }//for

            string finalword = new string(newstr);
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
    }
}
