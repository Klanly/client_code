using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_FashionShowModel : ModelBase<A3_FashionShowModel>
    {

        public Dictionary<int, fashionshow_date> Dic_AllData = new Dictionary<int, fashionshow_date>();

        //进游戏的时候时装的数据
        public int[] first_nowfs = new int[2];

        //当前身上穿的时装
        public int[] nowfs = new int[2];
        //当前拥有的
        public Dictionary<int, uint> dic_have_fs = new Dictionary<int, uint>();
        //是否显示
        public bool dress_show = true;
        public A3_FashionShowModel()
        {
            getalldata();
        }



        void getalldata()
        {
            List<SXML> xml= XMLMgr.instance.GetSXMLList("fashion_show.dress");
            foreach (SXML x in xml)
            {

                fashionshow_date fd = new fashionshow_date();
                fd.id = x.getInt("id");
                fd.name = x.getString("dress_name");
                fd.icon_file = x.getInt("icon_file");
                fd.dress_type = x.getInt("dress_type");
                fd.des = x.getString("desc");
                fd.carr = x.getInt("carr");
                List<fashionshow_att> lst_fa = new List<fashionshow_att>();
                List<fashionshow_unlock> lst_fu = new List<fashionshow_unlock>();
                foreach (SXML y in x.GetNodeList("att"))
                {
                    fashionshow_att fa = new fashionshow_att();
                    fa.type = y.getInt("att_type");
                    fa.value = y.getInt("att_value");
                    lst_fa.Add(fa);

                }
                foreach (SXML z in x.GetNodeList("unlock"))
                {
                    fashionshow_unlock fu = new fashionshow_unlock();
                    fu.type = z.getInt("type");
                    fu.need_num = z.getInt("need_num");
                    fu.need_id = z.getInt("need_item");
                    if (z.getInt("limit_day")!=-1)
                        fu.limit_day = z.getInt("limit_day");
                    if (z.getInt("forever") != -1)
                        fu.forever = z.getInt("forever");
                    lst_fu.Add(fu);
                }
                fd.lst_fa = lst_fa;
                fd.lst_fu = lst_fu;
                Dic_AllData[fd.id] = fd;
            }
        }

    }

    public class fashionshow_date
    {
        public int id;
        public bool isjihuo = false;
        public string name;
        public int carr;
        public int icon_file;
        public int dress_type;  //时装种类
        public string des;
        public List<fashionshow_att> lst_fa = new List<fashionshow_att>();
        public List<fashionshow_unlock> lst_fu = new List<fashionshow_unlock>();
    }
    public class fashionshow_att
    {
        public int type;
        public int value;
    }

    public class fashionshow_unlock
    {
        public int type;
        public int need_num;
        public int need_id;
        public int limit_day;
        public int forever;
    }
}
