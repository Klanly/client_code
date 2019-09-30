using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;

namespace MuGame
{

  public  class ContMgr
    {
        static public Dictionary<string, string> dText;
        static public Dictionary<string, string> dError;
        static public Dictionary<string, Color> dColor;
        static public Dictionary<string, string> dColorStr;
        static public void init()
        {
            dText = new Dictionary<string, string>();
            dColor = new Dictionary<string, Color>();
            SXML xml = XMLMgr.instance.GetSXML("zh");

            xml.forEach((SXML x) =>
            {
                string str = x.getString("c"),title = x.getString("i");
                str = StringUtils.formatText(str);                
                dText[title] = str;

                if (x.hasValue("color"))
                {
                    string[] color = x.getString("color").Split(',');
                    if (color.Length == 3)
                    {
                        int r = 0, g = 0, b = 0;
                        int.TryParse(color[0], out r);
                        int.TryParse(color[1], out g);
                        int.TryParse(color[2], out b);
                        dColor[title] = new Color(r, g, b);
                        dColorStr[title] = string.Format(@"#{0}{1}{2}",r.ToString("x"),g.ToString("x"),b.ToString("x"));
                    }
                }
            });

            dError = new Dictionary<string, string>();
            xml = XMLMgr.instance.GetSXML("errorcode.l");

            string errorcodes = xml.getString("c");
            errorcodes = errorcodes.Replace(" ", "");
            errorcodes = errorcodes.Replace("//", "");
            //  errorcodes = Regex.Replace(errorcodes, @"\s", "");
            string[] codes = errorcodes.Split('\n');
            // dError[xml.getString("i")] = str;
            foreach (string str in codes)
            {
                if (str == "" || str == "\t")
                    continue;

                string[] temparr = str.Split(',');
                string[] temparr2 = temparr[0].Split('=');

                dError[temparr2[1]] = temparr[1];
            };
        }


        static Dictionary<string, string>  dOutGameText ;
        static public void initOutGame()
        {
            dOutGameText = new Dictionary<string, string>();
            string[] files = System.IO.File.ReadAllLines(Cross.LoaderBehavior.DATA_PATH+ "OutAssets/outGameTxt.txt");

            for(int i=0;i<files.Length;i++)
            {
                string[] keyValue = files[i].Split('|');

                if (keyValue.Length != 2)
                    continue;

                dOutGameText[keyValue[0]] = keyValue[1];
                debug.Log(keyValue[0] + "==" + keyValue[1]);
            }
        }
        static public void initOutGame(string path)
        {
            dOutGameText = new Dictionary<string, string>();
            string[] files = path.Split(new char[] { '\n' });

            for (int i = 0; i < files.Length; i++)
            {
                string[] keyValue = files[i].Split('|');

                if (keyValue.Length != 2)
                    continue;

                dOutGameText[keyValue[0]] = keyValue[1];
            }
        }
        static public string getOutGameCont(string id, params string[] vals)
        {
            if (dOutGameText == null)
                initOutGame();

            if (!dOutGameText.ContainsKey(id))
                return id;

            string str = dOutGameText[id];

            int idx = 0;
            foreach (string p in vals)
            {
                string[] stringSeparators = new string[] { "<" + idx + ">" };
                string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                str = string.Join(p, arr);
                idx++;
            }

            string[] str1 = new string[] { "<n>" };
            string[] arr2 = str.Split(str1, StringSplitOptions.None);
            str = string.Join("\n", arr2);
            return str;
        }



        static public string getError(string id)
        {
            if (dError == null)
                init();

            if (!dError.ContainsKey(id))
                return id;

            return dError[id];
        }


        static public string getCont(string id, params string[] vals)
        {
            if (dText == null)
                init();

            if (!dText.ContainsKey(id))
                return id;

            string str = dText[id];

            int idx = 0;
            foreach (string p in vals)
            {
                string[] stringSeparators = new string[] { "<" + idx + ">" };
                string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                str = string.Join(p, arr);
                idx++;
            }

            return str;
        }


        static public string getCont(string id, List<string> pram = null)
        {
            if (dText == null)
                init();

            if (!dText.ContainsKey(id))
                return id;

            string str = dText[id];

            if (pram != null)
            {
                int idx = 0;
                foreach (string p in pram)
                {
                    string[] stringSeparators = new string[] { "<" + idx + ">" };
                    string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                    str = string.Join(p, arr);
                    idx++;
                }
            }

            return str;
        }
        static public string getFollowContColorStr(string id)
        {
            if (dColorStr == null)
                init();

            if (!dColorStr.ContainsKey(id))
                return "#000000";

            return dColorStr[id];
        }

        static public Color getFollowContColor(string id)
        {
            if (dColor == null)
                init();

            if (!dColor.ContainsKey(id))
                return Color.white;

            return dColor[id];
        }
    }
}
