using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>

    public class AES : CryptographicTechnique
    {
        string just_for_me_to_see;
        private int[,] table = new int[4, 4];
        private string[,] tableaftersubbyte = new string[4, 4];
        string[,] key_schedule = new string[4, 44];//keyschedule
        string[,] tableaftermix = new string[4, 4];
        string[] rcon = new string[] { "01", "02", "04", "08", "10", "20", "40", "80", "1b", "36" };
        private string[,] aes_s_box = new string[,] {
{"63","7c","77","7b","f2","6b","6f","c5","30","01","67","2b","fe","d7","ab","76"},
{"ca","82","c9","7d","fa","59","47","f0","ad","d4","a2","af","9c","a4","72","c0"},
{"b7","fd","93","26","36","3f","f7","cc","34","a5","e5","f1","71","d8","31","15"},
{"04","c7","23","c3","18","96","05","9a","07","12","80","e2","eb","27","b2","75"},
{"09","83","2c","1a","1b","6e","5a","a0","52","3b","d6","b3","29","e3","2f","84"},
{"53","d1","00","ed","20","fc","b1","5b","6a","cb","be","39","4a","4c","58","cf"},
{"d0","ef","aa","fb","43","4d","33","85","45","f9","02","7f","50","3c","9f","a8"},
{"51","a3","40","8f","92","9d","38","f5","bc","b6","da","21","10","ff","f3","d2"},
{"cd","0c","13","ec","5f","97","44","17","c4","a7","7e","3d","64","5d","19","73"},
{"60","81","4f","dc","22","2a","90","88","46","ee","b8","14","de","5e","0b","db"},
{"e0","32","3a","0a","49","06","24","5c","c2","d3","ac","62","91","95","e4","79"},
{"e7","c8","37","6d","8d","d5","4e","a9","6c","56","f4","ea","65","7a","ae","08"},
{"ba","78","25","2e","1c","a6","b4","c6","e8","dd","74","1f","4b","bd","8b","8a"},
{"70","3e","b5","66","48","03","f6","0e","61","35","57","b9","86","c1","1d","9e"},
{"e1","f8","98","11","69","d9","8e","94","9b","1e","87","e9","ce","55","28","df"},
{"8c","a1","89","0d","bf","e6","42","68","41","99","2d","0f","b0","54","bb","16"}};

        public override string Decrypt(string cipherText, string key)
        {

            /*string mainPlain2 = "0x00000000000000000000000000000001";
        string mainCipher2 = "0x58e2fccefa7e3061367f1d57a4e7455a";
             *                0x1CC88406EEAC7A06CDAE7004FA91259C"
        string mainKey2 = "0x00000000000000000000000000000000";*/
            keyschedule(key);
            //store the cipher text in table
            int result, colunmsize = 4, it_for_first_digit = 2;
            string mainPlain = cipherText;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)

                table[(it_for_first_digit / 2) - 1, 0] = Convert.ToInt32(mainPlain.Substring(it_for_first_digit, 2), 16);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                table[((it_for_first_digit / 2) - 1) - 4, 1] = Convert.ToInt32(mainPlain.Substring(it_for_first_digit, 2), 16);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                table[((it_for_first_digit / 2) - 1) - 8, 2] = Convert.ToInt32(mainPlain.Substring(it_for_first_digit, 2), 16);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                table[((it_for_first_digit / 2) - 1) - 12, 3] = Convert.ToInt32(mainPlain.Substring(it_for_first_digit, 2), 16);

            colunmsize = 4;
            //add round key
            for (int i = 0; i < 4; i++)
                for (int ii = 0; ii < 4; ii++)
                    table[i, ii] = table[i, ii] ^ Convert.ToInt32(key_schedule[i, ii + ((9 + 1) * 4)].Substring(0, key_schedule[i, ii + ((9 + 1) * 4)].Length), 16);
            //reverse shiftrows
            UNshiftrows();//table
            unsubyte();

            just_for_me_to_seefunction();
            just_for_me_to_see = "" + just_for_me_to_see;
            for (int it = 0; it < 9; it++)
            {
                //add round key
                for (int i = 0; i < 4; i++)
                    for (int ii = 0; ii < 4; ii++)
                        table[i, ii] = table[i, ii] ^ Convert.ToInt32(key_schedule[i, ii + ((8 - it + 1) * 4)].Substring(0, key_schedule[i, ii + ((8 - it + 1) * 4)].Length), 16);
                just_for_me_to_seefunction();
                //unmix columns
                hm();
                just_for_me_to_seefunction();
                UNshiftrows();//table
                just_for_me_to_seefunction();
                unsubyte();
                just_for_me_to_seefunction();
            }
            //add round key
            for (int i = 0; i < 4; i++)
                for (int ii = 0; ii < 4; ii++)
                    table[i, ii] = table[i, ii] ^ Convert.ToInt32(key_schedule[i, ii + ((0) * 4)].Substring(0, key_schedule[i, ii + ((0) * 4)].Length), 16);

            just_for_me_to_seefunction();
            return just_for_me_to_see;
        }
        public string ReverseString(string myStr)
        {
            char[] myArr = myStr.ToCharArray();
            Array.Reverse(myArr);
            return new string(myArr);
        }
        public string getremenderfromtwobinerynumbers(string a)
        {
            //100011011 ) 11110000101
            string elivenb = "100011011";
            string string_object;
            if (a.Length < elivenb.Length)
                return a;
            char[] res = new char[a.Length];
            int diffrence = a.Length - elivenb.Length;
            for (int i = 0; i < elivenb.Length; i++)
            {

                if (elivenb[i] == a[i])
                    res[i] = '0';
                else
                    res[i] = '1';


            }//i
            diffrence--;
            int counter = 1;
            for (int h = elivenb.Length; h < a.Length; h++)
            {
                res[h] = a[h];
            }
            //  res[elivenb.Length] = a[elivenb.Length];
            //if iam done with the divistion
            string_object = new string(res);
            string_object = getridofzreo(string_object);
            if (string_object.Length < 9)
                return getremenderfromtwobinerynumbers(string_object);
            //else
            while (diffrence >= 0)
            {

                for (int i = 0; i < elivenb.Length; i++)
                {

                    if (elivenb[i] == res[i + counter])
                        res[i + counter] = '0';
                    else
                        res[i + counter] = '1';


                }//i

                if (diffrence > 0)
                    res[elivenb.Length + counter] = a[elivenb.Length + counter];
                diffrence--;
                counter++;
                string_object = new string(res);
                string_object = getridofzreo(string_object);
                if (string_object.Length < 9)
                    return getremenderfromtwobinerynumbers(string_object);
            }//while
            string_object = new string(res);
            string_object = getridofzreo(string_object);
            if (string_object.Length > 9)
                return getremenderfromtwobinerynumbers(string_object);
            return string_object;
        }
        public void hm()
        {
            string[,] mat = new string[,] { { "0e", "0b", "0d", "09" },
            { "09", "0e", "0b", "0d" },
            { "0d", "09", "0e", "0b" },
            { "0b", "0d", "09", "0e" } };
            int summtion = 0, box, anotherBox;
            string[] allofthem = new string[64];
            int allofthemcounter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int foreverycell = 0; foreverycell < 4; foreverycell++)
                {
                    for (int ii = 0; ii < 4; ii++)
                    {
                        string matbin = ReverseString(Convert.ToString(Convert.ToInt64(mat[foreverycell, ii], 16), 2));
                        string normbin = ReverseString(Convert.ToString(table[ii, i], 2));
                        string strr = convertInttoHexMakeItTwoDigigt(table[ii, i]);
                        int[] poly = new int[matbin.Length + normbin.Length];

                        for (int z = matbin.Length - 1; z >= 0; z--)
                        {
                            if (matbin[z] == '0')
                                continue;

                            for (int zz = normbin.Length - 1; zz >= 0; zz--)
                            {
                                if (normbin[zz] == '0')
                                    continue;
                                poly[z + zz]++;
                            }//zz

                        }//z

                        for (int o = 0; o < poly.Length; o++)
                        {
                            if (poly[o] == 0)
                                continue;
                            if (poly[o] % 2 == 0)
                                poly[o] = 0;
                            else
                                poly[o] = 1;



                        }//o
                        poly[0] = poly[0];

                        allofthem[allofthemcounter] = string.Join("", poly);
                        allofthem[allofthemcounter] = getridofzreo(ReverseString(allofthem[allofthemcounter]));
                        allofthemcounter++;
                    }//ii
                }//foreverycell

            }//i
            allofthemcounter = allofthemcounter + 0;
            for (int i = 0; i < allofthemcounter; i++)
            {
                allofthem[i] = getridofzreo(getremenderfromtwobinerynumbers(allofthem[i]));

            }//i
            int row = 0, col = 0;
            for (int i = 0; i < allofthemcounter; i = i + 4)
            {
                string sd = ReverseString(xor_numbers(ReverseString(xor_numbers(allofthem[i], allofthem[i + 1])), ReverseString(xor_numbers(allofthem[i + 3], allofthem[i + 2]))));// = getremenderfromtwobinerynumbers(allofthem[i].Substring(1));
                                                                                                                                                                                   //   string sd = xor_numbers(xor_numbers(xor_numbers(allofthem[i], allofthem[i + 1]), allofthem[i + 2]), allofthem[i + 3]);// = getremenderfromtwobinerynumbers(allofthem[i].Substring(1));

                //     sd = sd.Substring(0, 8);
                int num = Convert.ToInt32(sd, 2);
                table[row % 4, col] = num;
                row++;
                if ((i + 4) % 16 == 0)
                    col++;
                sd = convertInttoHexMakeItTwoDigigt(num);
            }//i
        }
        public string getridofzreo(string a)
        {

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != '0')
                    return a.Substring(i);

            }
            return "";
        }
        public string xor_numbers(string a, string b)
        {


            int bigsize = a.Length, a_index = a.Length, b_index = b.Length, bigest = b.Length;
            if (b.Length < bigsize)
            {
                bigsize = b.Length;
                bigest = a.Length;
            }
            int[] digits = new int[bigest];
            for (int i = 0; i < bigsize; i++)
            {
                a_index--; b_index--;
                if (a[a_index] == b[b_index])
                    digits[i] = 0;
                else
                    digits[i] = 1;

            }
            if (bigest == bigsize)
                return string.Join("", digits);
            if (a.Length > b.Length)
            {
                for (int i = bigsize; i < bigest; i++)
                {
                    a_index--;// b_index--;
                    if (a[a_index] == '0')
                        digits[i] = 0;
                    else
                        digits[i] = 1;

                }
                return string.Join("", digits);
            }//a>b
            //else
            for (int i = bigsize; i < bigest; i++)
            {
                // a_index--;//
                b_index--;
                if (b[b_index] == '0')
                    digits[i] = 0;
                else
                    digits[i] = 1;

            }
            return string.Join("", digits);
        }
        public override string Encrypt(string plainText, string key)
        {
            keyschedule(key);
            //initial_round
            initial_s_round(plainText, key);
            for (int i = 0; i < 9; i++)
            {
                subbyte();
                shiftrows();//  tableaftersubbyte
                mix_columns();//tableaftermix
                addroundkey(i);//table
                just_for_me_to_seefunction();
            }
            subbyte();
            shiftrows();
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    table[i, ii] = Convert.ToInt32(tableaftersubbyte[i, ii].Substring(0, tableaftersubbyte[i, ii].Length), 16) ^ Convert.ToInt32(key_schedule[i, ii + ((9 + 1) * 4)].Substring(0, key_schedule[i, ii + ((9 + 1) * 4)].Length), 16);

                }
            }
            just_for_me_to_seefunction();
            Console.Write(just_for_me_to_see);
            return just_for_me_to_see;

            //  throw new NotImplementedException();
        }
        public Queue initial_s_round(string plainText, string key)
        {
            //initial_round
            string mainPlain = plainText;
            string mainCipher = "0x3925841D02DC09FBDC118597196A0B32";
            string mainKey = key;
            string keyPart, TextPart;
            int result, colunmsize = 4, it_for_first_digit = 2;

            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
            {
                TextPart = mainPlain.Substring(it_for_first_digit, 2); keyPart = mainKey.Substring(it_for_first_digit, 2);
                result = Convert.ToInt32(TextPart, 16) ^ Convert.ToInt32(keyPart, 16);

                table[(it_for_first_digit / 2) - 1, 0] = result;
            }
            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
            {
                TextPart = mainPlain.Substring(it_for_first_digit, 2); keyPart = mainKey.Substring(it_for_first_digit, 2);
                result = Convert.ToInt32(TextPart, 16) ^ Convert.ToInt32(keyPart, 16);

                table[((it_for_first_digit / 2) - 1) - 4, 1] = result;
            }
            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
            {
                TextPart = mainPlain.Substring(it_for_first_digit, 2); keyPart = mainKey.Substring(it_for_first_digit, 2);
                result = Convert.ToInt32(TextPart, 16) ^ Convert.ToInt32(keyPart, 16);

                table[((it_for_first_digit / 2) - 1) - 8, 2] = result;
            }
            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
            {
                TextPart = mainPlain.Substring(it_for_first_digit, 2); keyPart = mainKey.Substring(it_for_first_digit, 2);
                result = Convert.ToInt32(TextPart, 16) ^ Convert.ToInt32(keyPart, 16);

                table[((it_for_first_digit / 2) - 1) - 12, 3] = result;
            }
            colunmsize = 4;
            return null;
            //subbyte
            //shiftRows

            //mixcolumns

            //throw new NotImplementedException();
        }
        public void addroundkey(int roundNumber)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    table[i, ii] = Convert.ToInt32(tableaftermix[i, ii].Substring(0, tableaftermix[i, ii].Length), 16) ^ Convert.ToInt32(key_schedule[i, ii + ((roundNumber + 1) * 4)].Substring(0, key_schedule[i, ii + ((roundNumber + 1) * 4)].Length), 16);

                }
            }
            return;
        }
        public void subbyte()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    tableaftersubbyte[i, ii] = fromsbox(table[i, ii]);
                }
            }
            return;
        }
        public void shiftrows()
        {
            string holdThisRow;
            for (int ii = 1; ii < 4; ii++)
            {
                holdThisRow = shiftthisrow(ii, tableaftersubbyte[ii, 0], tableaftersubbyte[ii, 1], tableaftersubbyte[ii, 2], tableaftersubbyte[ii, 3]);
                for (int i = 0; i < 4; i++)
                {
                    tableaftersubbyte[ii, i] = holdThisRow.Substring(i * 2, 2);
                }
            }
            return;
        }
        public string convertInttoHexMakeItTwoDigigt(int number)
        {
            string str = number.ToString("X");
            if (str.Length == 1)
                return "0" + str;
            return str;
        }
        public void UNshiftrows()
        {
            string holdThisRow;
            for (int ii = 1; ii < 4; ii++)
            {
                holdThisRow = shiftthisrow(4 - ii
                   , convertInttoHexMakeItTwoDigigt(table[ii, 0])
                   , convertInttoHexMakeItTwoDigigt(table[ii, 1])
                   , convertInttoHexMakeItTwoDigigt(table[ii, 2])
                   , convertInttoHexMakeItTwoDigigt(table[ii, 3]));

                // holdThisRow = shiftthisrow(ii, tableaftersubbyte[ii, 0], tableaftersubbyte[ii, 1], tableaftersubbyte[ii, 2], tableaftersubbyte[ii, 3]);
                for (int i = 0; i < 4; i++)
                {
                    table[ii, i] = Convert.ToInt32(holdThisRow.Substring(i * 2, 2), 16);
                    //  tableaftersubbyte[ii, i] = holdThisRow.Substring(i * 2, 2);
                }
            }
            return;
        }
        public void mix_columns()
        {
            //    int sss=     zerotwo("87");
            string[,] mat = new string[,] { { "02", "03", "01", "01" },
            { "01", "02", "03", "01" },
            { "01", "01", "02", "03" },
            { "03", "01", "01", "02" } };
            int summtion = 0, box;
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    if (mat[0, ii] == "01")
                        summtion = summtion ^ Convert.ToInt32(tableaftersubbyte[ii, i], 16);
                    if (mat[0, ii] == "02")
                    {
                        box = zerotwo(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }
                    if (mat[0, ii] == "03")
                    {
                        box = zerothree(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }

                }
                tableaftermix[0, i] = summtion.ToString("X");
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {
                    if (mat[1, ii] == "01")
                        summtion = summtion ^ Convert.ToInt32(tableaftersubbyte[ii, i], 16);
                    if (mat[1, ii] == "02")
                    {
                        box = zerotwo(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }
                    if (mat[1, ii] == "03")
                    {
                        box = zerothree(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }

                }
                tableaftermix[1, i] = summtion.ToString("X");
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {
                    if (mat[2, ii] == "01")
                        summtion = summtion ^ Convert.ToInt32(tableaftersubbyte[ii, i], 16);
                    if (mat[2, ii] == "02")
                    {
                        box = zerotwo(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }
                    if (mat[2, ii] == "03")
                    {
                        box = zerothree(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }

                }
                tableaftermix[2, i] = summtion.ToString("X");
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {
                    if (mat[3, ii] == "01")
                        summtion = summtion ^ Convert.ToInt32(tableaftersubbyte[ii, i], 16);
                    if (mat[3, ii] == "02")
                    {
                        box = zerotwo(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }
                    if (mat[3, ii] == "03")
                    {
                        box = zerothree(tableaftersubbyte[ii, i]);
                        summtion = summtion ^ box;
                    }

                }
                tableaftermix[3, i] = summtion.ToString("X");
                summtion = 0;
            }

            return;
        }
        public void unmix_columns()
        {
            //    int sss=     zerotwo("87");
            string[,] mat = new string[,] { { "0e", "0b", "0d", "09" },
            { "09", "0e", "0b", "0d" },
            { "0d", "09", "0e", "0b" },
            { "0b", "0d", "09", "0e" } };
            int summtion = 0, box, anotherBox;
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {

                    if (mat[0, ii] == "0d")//13
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        //   box = zerotwo(table[ii, i].ToString("X"));
                        summtion = summtion ^ box;
                    }
                    if (mat[0, ii] == "0b")//11
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[0, ii] == "09")
                    {
                        box = zerothree(table[ii, i].ToString("X"));
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[0, ii] == "0e")//14
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }

                }
                //  tableaftermix[0, i] = summtion.ToString("X");
                table[0, i] = summtion;
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {

                    if (mat[1, ii] == "0d")//13
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        //   box = zerotwo(table[ii, i].ToString("X"));
                        summtion = summtion ^ box;
                    }
                    if (mat[1, ii] == "0b")//11
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[1, ii] == "09")
                    {
                        box = zerothree(table[ii, i].ToString("X"));
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[1, ii] == "0e")//14
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }


                }
                table[1, i] = summtion;
                //    tableaftermix[1, i] = summtion.ToString("X");
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {

                    if (mat[2, ii] == "0d")//13
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        //   box = zerotwo(table[ii, i].ToString("X"));
                        summtion = summtion ^ box;
                    }
                    if (mat[2, ii] == "0b")//11
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[2, ii] == "09")
                    {
                        box = zerothree(table[ii, i].ToString("X"));
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[2, ii] == "0e")//14
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }


                }
                table[2, i] = summtion;
                //tableaftermix[2, i] = summtion.ToString("X");
                summtion = 0;
                for (int ii = 0; ii < 4; ii++)
                {

                    if (mat[3, ii] == "0d")//13
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        //   box = zerotwo(table[ii, i].ToString("X"));
                        summtion = summtion ^ box;
                    }
                    if (mat[3, ii] == "0b")//11
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[3, ii] == "09")
                    {
                        box = zerothree(table[ii, i].ToString("X"));
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerothree(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }
                    if (mat[3, ii] == "0e")//14
                    {
                        box = zerotwo(table[ii, i].ToString("X"));
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        anotherBox = zerotwo(table[ii, i].ToString("X"));
                        box = box ^ anotherBox;
                        summtion = summtion ^ box;
                    }


                }
                table[3, i] = summtion;
                //tableaftermix[3, i] = summtion.ToString("X");
                summtion = 0;
            }

            return;
        }

        public int zerotwo(string number)
        {
            //    int hex=  ;
            string binary = Convert.ToString(Convert.ToInt32(number, 16), 2);
            if (binary.Length == 8)//most left is one
            {
                string s = binary.Substring(1, binary.Length - 1) + "0";
                int num = Convert.ToInt32(s, 2) ^ Convert.ToInt32("1B", 16);
                return num;
            }
            string ss = binary.Substring(0, binary.Length) + "0";
            //  int nums = Convert.ToInt32(ss, 2) ^ Convert.ToInt32("1B", 16);
            return Convert.ToInt32(ss, 2);

        }
        public void keyschedule(string mainKey)
        { //string mainKey = "0x2B7E151628AED2A6ABF7158809CF4F3C";
          // key_schedule

            int colunmsize = 4, it_for_first_digit = 2;

            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                key_schedule[(it_for_first_digit / 2) - 1, 0] = mainKey.Substring(it_for_first_digit, 2);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                key_schedule[((it_for_first_digit / 2) - 1) - 4, 1] = mainKey.Substring(it_for_first_digit, 2);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                key_schedule[((it_for_first_digit / 2) - 1) - 8, 2] = mainKey.Substring(it_for_first_digit, 2);

            colunmsize = 4;
            for (; colunmsize > 0; colunmsize--, it_for_first_digit = it_for_first_digit + 2)
                key_schedule[((it_for_first_digit / 2) - 1) - 12, 3] = mainKey.Substring(it_for_first_digit, 2);

            colunmsize = 4;

            //rotword
            /*   rotword(4);
               subbytekey(4);
               xor_key_from_rcon(4);
               xor_key_without_rcon(5);
               xor_key_without_rcon(6);
               xor_key_without_rcon(7);*/
            for (int i = 4; i < 44; i = i + 4)
            {
                rotword(i);
                subbytekey(i);
                xor_key_from_rcon(i);
                for (int ii = i + 1; ii < i + 4; ii++)
                {
                    xor_key_without_rcon(ii);
                }

            }
        }
        public void subbytekey(int index)
        {
            for (int i = 0; i < 4; i++)
            {
                // key_schedule[i, index] = fromsbox(key_schedule[i, index]);
                if (key_schedule[i, index].Length == 1)
                    key_schedule[i, index] = aes_s_box[0, Convert.ToInt32(key_schedule[i, index], 16)];

                else
                    key_schedule[i, index] = aes_s_box[Convert.ToInt32(key_schedule[i, index].Substring(0, 1), 16), Convert.ToInt32(key_schedule[i, index].Substring(1, 1), 16)];

            }

        }
        public void rotword(int index)
        {
            for (int i = 0; i < 4; i++)
            {
                key_schedule[i, index] = key_schedule[(i + 1) % 4, index - 1];
            }
        }
        public void unsubyte()
        {

            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    string str = unsubbytehelper(convertInttoHexMakeItTwoDigigt(table[i, ii]));
                    table[i, ii] = Convert.ToInt32(str, 16);

                }

            }
        }
        public string unsubbytehelper(string cell)
        {

            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    if (aes_s_box[row, col].ToUpper() == cell)
                    {
                        return row.ToString("X") + col.ToString("X");
                    }


                }//col


            }//row
            return "";

        }
        public int zerothree(string number)
        {
            int num = zerotwo(number);
            int n = num ^ Convert.ToInt32(number, 16);
            return n;

        }
        public void xor_key_from_rcon(int index)
        {//rcon
            int xorResult = Convert.ToInt32(key_schedule[0, index - 4].Substring(0, key_schedule[0, index - 4].Length), 16) ^ Convert.ToInt32(key_schedule[0, index].Substring(0, key_schedule[0, index].Length), 16) ^ Convert.ToInt32(rcon[(index / 4) - 1], 16);
            key_schedule[0, index] = xorResult.ToString("X");
            for (int i = 1; i < 4; i++)
            {

                xorResult = Convert.ToInt32(key_schedule[i, index - 4].Substring(0, key_schedule[i, index - 4].Length), 16) ^ Convert.ToInt32(key_schedule[i, index].Substring(0, key_schedule[i, index].Length), 16);
                key_schedule[i, index] = xorResult.ToString("X");

            }

        }
        public void xor_key_without_rcon(int index)
        {//rcon
            int xorResult;
            for (int i = 0; i < 4; i++)
            {

                xorResult = Convert.ToInt32(key_schedule[i, index - 4].Substring(0, key_schedule[i, index - 4].Length), 16) ^ Convert.ToInt32(key_schedule[i, index - 1].Substring(0, key_schedule[i, index - 1].Length), 16);
                key_schedule[i, index] = xorResult.ToString("X");

            }

        }
        public string fromsbox(int number)
        {
            string hex = number.ToString("X");
            if (hex.Length == 1)
                return aes_s_box[0, Convert.ToInt32(hex, 16)];


            return aes_s_box[Convert.ToInt32(hex.Substring(0, 1), 16), Convert.ToInt32(hex.Substring(1, 1), 16)];
        }
        public void just_for_me_to_seefunction()
        {
            just_for_me_to_see = "0x";
            for (int i = 0; i < 4; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    string d = table[ii, i].ToString("X");
                    if (d.Length == 1)
                        d = "0" + d;
                    just_for_me_to_see = just_for_me_to_see + d;

                }
            }

        }
        public string shiftthisrow(int number, string a, string b, string c, string d)
        {
            if (number == 1)
                return b + c + d + a;
            if (number == 2)
                return c + d + a + b;
            if (number == 3)
                return d + a + b + c;
            return "";
        }
    }
}
