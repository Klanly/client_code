using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class a3_ActiveOnlineModel : ModelBase<a3_ActiveOnlineModel>
    {
        public bool hintreward = false;/*判断活跃奖励是否有提示*/
        public bool hintlottery = false;/*判断抽奖时候有提示*/


        public Dictionary<int, huoyue_infos> dic_huoyue = new Dictionary<int, huoyue_infos>();
        public Dictionary<int, reward_info> dic_reward = new Dictionary<int, reward_info>();
        public Dictionary<int, online_time> dic_online = new Dictionary<int, online_time>();
        public Dictionary<int, fund_infso> dic_funds = new Dictionary<int, fund_infso>();

        //当前活跃点数
        public int nowpoint = 0;
        //基金限制是否购买
        public bool zhanli_fund = false;
        public bool zuanshi_fund = false;
        public bool zhuansheng_fund = false;
        //当前可抽奖次数
        public int lottery_num = 0;
        //钻石基金当前进去（其他读playermodel）
        public int zuanshi_fundnow = 0;
        public int zhuansheng_fundnow = (int)PlayerModel.getInstance().up_lvl;
        public int zhanli_fundnow= PlayerModel.getInstance().combpt;


        public a3_ActiveOnlineModel()
        {
            getxml_infos1();
            getxml_infos();

        }
        void getxml_infos1()
        {
            List<SXML> xmlsss = XMLMgr.instance.GetSXMLList("huoyue.lotterytime");
            foreach (SXML x in xmlsss)
            {

                online_time ot = new online_time();
                ot.type = x.getInt("id");
                ot.time = x.getUint("time");
                dic_online[ot.type] = ot;
            }
            List<SXML> xmlssss = XMLMgr.instance.GetSXMLList("huoyue.foundation");
            foreach (SXML x in xmlssss)
            {
                fund_infso fi = new fund_infso();
                fi.type = x.getInt("type");
                fi.id = x.getInt("id");
                fi.des = x.getString("desc");
                fi.file = x.getString("icon_file");
                fi.zuanshi_num = x.getInt("yb");
                fi.bangzuan_num = x.getInt("bndyb");
                fi.need_paraml = x.getInt("param1");
                dic_funds[fi.id] = fi;
            }
        }
        void getxml_infos()
        {
            List<SXML> xmls = XMLMgr.instance.GetSXMLList("huoyue.active");
            foreach (SXML x in xmls)
            {
                huoyue_infos hy = new huoyue_infos();
                hy.id = x.getInt("id");
                hy.des = x.getString("des");
                hy.need_num = x.getInt("times");
                hy.point = x.getInt("ac_num");
                hy.type = x.getInt("type");
                hy.pram = x.getString("pram");
                hy.icon_frie = x.getString("icon_file");
                dic_huoyue[hy.id] = hy;
                
            }
            List<SXML> xmlss = XMLMgr.instance.GetSXMLList("huoyue.reward");
            foreach (SXML x in xmlss)
            {
                reward_info rd = new reward_info();
                rd.id = x.getInt("id");
                rd.item_id = x.getInt("item");
                rd.num = x.getInt("num");
                rd.ac = x.getInt("ac");
                dic_reward[rd.id] = rd;
            }
        }
        public void Refresh_huoyue(int id,int havenum)
        {
            if(dic_huoyue.ContainsKey(id))
            {
                dic_huoyue[id].have_num = havenum;
                if (havenum >= dic_huoyue[id].need_num)
                    dic_huoyue[id].receive_type = 2;
            }
        }

       
        public void Refresh_reward(int ac)
        {
            foreach (int i in dic_reward.Keys)
            {
                if (dic_reward[i].ac == ac)
                {
                    dic_reward[i].receive_type = 2;

                }
                else
                {
                    if(nowpoint>= dic_reward[i].ac&& dic_reward[i].receive_type!=2)
                    {
                        dic_reward[i].receive_type = 1;
                    }
                }
            }
        }


        public void Refresh_fund(int id, int receive_type )
        {
            if(dic_funds.ContainsKey(id))
            {
                dic_funds[id].receive_type = receive_type;
            }
        }


        public Dictionary<int, online_infos> GetActivelotteryItems(int id)
        {
            Dictionary<int, online_infos> dic = new Dictionary<int, online_infos>();
            List<SXML> xml = XMLMgr.instance.GetSXML("huoyue.item", "type=="+id).GetNodeList("item");
            foreach(SXML x in xml)
            {
                online_infos oi = new online_infos();
                oi.id = x.getInt("id");
                oi.item_id = x.getInt("item_id");
                oi.num = x.getInt("num");
                dic[oi.id] = oi; 
            }
            return dic;

        }



     public    void RefreshZuanshi_fundnow(int num)
        {
            zuanshi_fundnow = num;
        }


    }
    class huoyue_infos
    {
        public int id;
        public string des;
        public int point;
        public int need_num;
        public string icon_frie;
        public int have_num = 0;
        public int type;
        public string pram;
        public int receive_type = 0;   //0:未完成   1：已完成未领  2：已领
    }
    class reward_info
    {
        public int id;
        public int ac;
        public int item_id;
        public int num;
        public int receive_type = 0;
    }
    class online_time
    {
        public int type;
        public uint time;
        //ios上内存崩溃，可能是没有初始化 lst_info 的原因
        //public List<online_infos> lst_info;
    }

    class online_infos
    {
        public int id;
        public int item_id;
        public int num;
    }


    class fund_infso
    {
        public int type;
        public int id;
        public string des;
        public string file;
        public int zuanshi_num;
        public int bangzuan_num;
        public int need_paraml;
        public int have_paraml = 0;
        public int receive_type = 0;
    }
}
