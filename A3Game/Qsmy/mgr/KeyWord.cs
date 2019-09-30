using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MuGame
{
    public class KeyWord
    {
        public static KeyWord instance = new KeyWord();

        public static string filter(string str)
        {

            return instance.Filter(str);
        }

        public static bool isContain(string str)
        {

            return instance.IsContain(str);
        }

        private static string[] l;
        public void init()
        {

            List<SXML> xmlList = XMLMgr.instance.GetSXMLList("keyword.k");
            l = new string[xmlList.Count];
            int idx = 0;
            foreach (SXML sxml in xmlList)
            {
                l[idx] = sxml.getString("s");
                idx++;
            }
            //do 
            //{
            //    l.Add(xml.getString("s"));
            //} while (xml.nextOne());
        }


        public string Filter(string str)
        {
            if (l == null)
                init();

            //   foreach (string  s in l)
            // {
            //  string a  =s;
            //     str = Regex.Replace(str, a, "*");

            string[] arr = str.Split(l, StringSplitOptions.None);
            //   string.Join("*", arr);
            // str = str.Replace(s, "*");
            //  }
            str = string.Join("*", arr);

            return str;
        }
        public bool IsContain(string str)
        {
            if (l == null) init();
            string[] arr = str.Split(l, StringSplitOptions.None);
            //  debug.Log(arr[0]+" "+ arr.Length);

            return arr.Length > 1;
        }




    }
}
