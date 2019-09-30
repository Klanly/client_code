using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
 public  class NewbieTeachMgr
    {
        List<NewbieQueItem> lQue = new List<NewbieQueItem>();
        public NewbieTeachMgr()
        {
            NewbieTeachItem.initCommand();
        }

        public void add(string str,int id)
        {
            NewbieQueItem item = new NewbieQueItem(str, id);
          lQue.Add(item);
        }

        public void add(List<string> lstr, int id)
        {
            NewbieQueItem item = new NewbieQueItem(lstr, id);
            lQue.Add(item);
        }

        public void add(int type,int tpyeid,int id)
        {
            List<string> lstr = new List<string>();
            SXML ncpxml = XMLMgr.instance.GetSXML("newbie_fb");
            SXML xml = ncpxml.GetNode("newbie", "id==" + tpyeid);
            if (xml != null)
            {
                List<SXML> l = new List<SXML>();
                if (type == 1)
                    l = xml.GetNodeList("waitcode");
                else
                    l = xml.GetNodeList("code");
                foreach (SXML s in l)
                {
                    lstr.Add(s.getString("value"));
                }
            }
            NewbieQueItem item = new NewbieQueItem(lstr, id);
            lQue.Add(item);
        }


        private static NewbieTeachMgr _instance;
        public static NewbieTeachMgr getInstance()
        {
            if (_instance == null)
                _instance = new NewbieTeachMgr();

            return _instance;
        }
    }

    class NewbieQueItem
    {
        public int id;
        List<NewbieTeachItem> list=new List<NewbieTeachItem>();
        public NewbieQueItem(string str,int idid)
        {
            string[] arrStr = str.Split(';');
            NewbieTeachItem last = null;
            id = idid;
            for (int i = 0; i < arrStr.Length; i++)
            {
                if (arrStr[i] == "")
                    continue;

                NewbieTeachItem item = NewbieTeachItem.initWithStr(arrStr[i]);
                item.idx = i;
                item.id = id;
                if (last != null)
                    last.nextItem = item;

                last = item;
                list.Add(item);
            }

            list[0].doit();
        }

        public NewbieQueItem(List<string> lStr, int idid)
        {
            NewbieTeachItem last = null;
            id = idid;
            for (int i = 0; i < lStr.Count; i++)
            {
                if (lStr[i] == "")
                    continue;

                NewbieTeachItem item = NewbieTeachItem.initWithStr(lStr[i]);
                item.idx = i;
                item.id = id;
                if (last != null)
                    last.nextItem = item;

                last = item;
                list.Add(item);
            }

            list[0].doit();
        }

    }
}
