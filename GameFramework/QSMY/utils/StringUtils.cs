using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace GameFramework
{
    public class StringUtils
    {
        public static string unicodeToStr(string str)
        {

            string outStr = "";
            string a = "";

            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    a = str[i].ToString();
                    if (Regex.IsMatch(a, "u") && i + 5 <= str.Length)
                    {
                        a = str.Substring(i + 1, 4);
                        outStr += (char)int.Parse(a, System.Globalization.NumberStyles.HexNumber);
                        i += 4;
                    }
                    else
                    {
                        outStr += str[i];

                    }
                }
            }
            return outStr;
        }

        public static string FilterSpecial(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace("'","");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\\", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");
                return str;
            }
        }

        static public string formatText(string str)
        {
            string[] arr = str.Split(new string[] { "{n}" }, StringSplitOptions.None);
            str = string.Join("\n", arr);

            arr = str.Split('{');
            str = string.Join("<", arr);

            arr = str.Split('}');
            str = string.Join(">", arr);
            return str;
        }
        static public bool isValidName(string name)
        {
            bool isValid = true;
            string errChar = "\\/:*?\"<>|!@#$%^&*()_+={}[]<>\t\0\' ";
            if (string.IsNullOrEmpty(name))
            {
                isValid = false;
            }
            else
            {
                for (int i = 0; i < errChar.Length; i++)
                {
                    if (name.Contains(errChar[i].ToString()))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }

        //public static string unicodeToStr(string str)
        //{

        //    string outStr = "";  
        //    if (!string.IsNullOrEmpty(str))  
        //    {  
        //        string[] strlist = str.Replace("/","").Split('u');  
        //        try  
        //        {  
        //            for (int i = 1; i < strlist.Length; i++)  
        //            {  
        //                //将unicode字符转为10进制整数，然后转为char中文字符  
        //                outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);  
        //            }  
        //        }  
        //        catch (FormatException ex)  
        //        {  
        //            outStr = ex.Message;  
        //        }  
        //    }
        //    return outStr;
        //}

    }
}
