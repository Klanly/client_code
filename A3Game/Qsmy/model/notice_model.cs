using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Policy;
using Cross;
using GameFramework;
namespace MuGame
{
    class notice_model : ModelBase<notice_model>
    {
        public List<noticeDate> notice = new List<noticeDate>();
      //  public List<float> keys = new List<float>();
      public   void xml_time()
        {

            List<SXML> xml = XMLMgr.instance.GetSXMLList("notice.func");

            for (int i = 0; i < xml.Count; i++)
            {
                noticeDate date = new noticeDate();
                date.id = xml[i].getInt("id");
                date.des = xml[i].getString("des");
                date.func_id = xml[i].getInt("func_id");
                date.last = xml[i].getInt("last");
                date.icon = xml[i].getInt("icon");
                date.zhuan = xml[i].getInt("zhuan");
                date.level = xml[i].getInt("level");
                 date.time = new Dictionary<float, float>();
                List<SXML> times = xml[i].GetNodeList("time");
                for (int j = 0; j < times.Count; j++)
                {
                    string t = times[j].getString("t");
                    string[] time_point = t.Split(',');
                    float a = float.Parse(time_point[0]);
                    float b = float.Parse(time_point[1]);
                    date.time[a] = b;
                   // keys.Add(a);
                }
                notice.Add(date);
            }

        }
    }
    public class noticeDate
    {
        public int id;
        public int func_id;
        public int last;
        public int icon;
        public string des;
        public int zhuan;
        public int level;
        public Dictionary<float, float>time;

    }
}
