using Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class A3_SevendayModel : ModelBase<A3_SevendayModel>
    {
       public    bool[] pointshow = new bool[4] {false,false,false,false };//积分奖励显示


        public int thisday = 1;           //哪一天
        public int have_point = 0;        //点数
        public int today_cost = 0;        //今日消费数量
        public int intensify_num = 0;     //今日强化次数 

        public int can_num = 0;//可以领但是没领的（提示功能）
        public bool havereware = false;//点数奖励是否有可领的

        public Dictionary<int, sevendayData> dic_data = new Dictionary<int, sevendayData>();

        public List<four_box> jifen_box = new List<four_box>();

        public A3_SevendayModel()
        {

            getSevendayData();

        }
        //读表存数据
        void getSevendayData()
        {
            List<SXML> xml = XMLMgr.instance.GetSXMLList("seven_days.seven_day");

            foreach (SXML x in xml)
            {
                int day = x.getInt("day");
                sevendayData sdd = new sevendayData();

                SXML xml_la = x.GetNode("login_awd");
                login_awd la = new login_awd();
                la.item_id = xml_la.getInt("awd_item");
                la.item_num = xml_la.getInt("awd_num");
                la.point = xml_la.getInt("point");
                la.tab = xml_la.getInt("tab");
                la.des = xml_la.getString("des");

                half_buy hb = new half_buy();
                SXML xml_hb = x.GetNode("half_buy");
                hb.shop_item = xml_hb.getInt("shop_item");
                hb.shop_num = xml_hb.getInt("shop_num");
                hb.point = xml_hb.getInt("point");
                hb.tab = xml_hb.getInt("tab");
                hb.des = xml_hb.getString("des");
                hb.cost = xml_hb.getInt("cost");

                Dictionary<int, award> dic_award = new Dictionary<int, award>();
                foreach (SXML y in x.GetNodeList("award"))
                {
                    award ad = new award();
                    ad.award_id = y.getInt("award_id");
                    ad.task_type = y.getInt("type");
                    ad.task_need = y.getString("param1");
                    ad.task_point = y.getInt("point");
                    ad.tab = y.getInt("tab");
                    ad.task_des = y.getString("des");
                    dic_award[ad.award_id] = ad;
                    ad.lst_ta = GetTaskAwards(day, ad.award_id);
                }
                sdd.loginaed = la;
                sdd.halfbuy = hb;
                sdd.task_award = dic_award;
                dic_data[day] = sdd;
            }

            List<SXML> p_xml = XMLMgr.instance.GetSXMLList("seven_days.point_awd");

            foreach (SXML z in p_xml)
            {
                four_box fb = new four_box();
                fb.id = z.getInt("award_id");
                fb.param1 = z.getInt("param1");
                task_awards ta = new task_awards();
                ta.id = z.GetNode("RewardItem").getInt("item_id");
                ta.value = z.GetNode("RewardItem").getInt("value");
                fb.lst_ta = ta;
                jifen_box.Add(fb);
            }
        }
        //对应的奖励
        List<task_awards> GetTaskAwards(int day, int award_id)
        {
            List<task_awards> lst = new List<task_awards>();
            List<SXML> xml = XMLMgr.instance.GetSXML("seven_days.seven_day", "day==" + day).GetNode("award", "award_id==" + award_id).GetNodeList("RewardItem");
            foreach (SXML x in xml)
            {
                task_awards ta = new task_awards();
                ta.id = x.getInt("item_id");
                ta.value = x.getInt("value");
                lst.Add(ta);
            }
            return lst;
        }
        //刷新数据（状态）
                /*前两种*/
        public void RefreshLg(int day,int state)
        {
            //dic_data[thisday].loginaed.isReceive = lg_isreceive>0?true:false;
            dic_data[day].loginaed.state = state;

        }
        public void RefreshHb(int hb_isreceive)
        {
            dic_data[thisday].halfbuy.isReceive = hb_isreceive > 0 ? true : false;
        }
               /*后一种*/
        public void Refreshs(int award_id, int state,int reach_num=-1)
        {


            foreach(int i in dic_data.Keys)
            {
                if (dic_data[i].task_award.ContainsKey(award_id))
                {
                    dic_data[i].task_award[award_id].state = state;
                    if(reach_num!=-1)
                      dic_data[i].task_award[award_id].task_have = reach_num;
                }
            }


        }
        //刷新点数奖励
        public void Refresh_fourbox(int id,int state)
        {
            for(int i=0;i< jifen_box.Count;i++)
            {
                if(jifen_box[i].id==id)
                {
                    jifen_box[i].state = state;
                }
            }

        }
        //刷新点数（半价和登陆自己加）
        public void RefreshPoint(int i)
        {
            if(i==0)
            {
                have_point += dic_data[thisday].loginaed.point;

            }
            else
            {
                have_point += dic_data[thisday].halfbuy.point;
            }
        }
        //刷新任务进度
        public void RefreshRach_num(int type, int rach_num)
        {
            //sevendayData data = dic_data[thisday];


            //foreach (award num in dic_data[thisday].task_award.Values)
            //{
            //    if (num.task_type == type)
            //    {
            //        num.task_have = rach_num;
            //    }
            //}
            foreach (int i in dic_data.Keys)
            {
                foreach (award num in dic_data[i].task_award.Values)

                {
                    if (num.task_type == type)
                    {
                        num.task_have = rach_num;
                    }
                }
            
            }
        }






        public void showOrHideFire()
        {

            debug.Log("七日目标提示：" + can_num);
           IconAddLightMgr.getInstance().showOrHideFire(can_num>0? "open_Light_sevenday":"close_Light_sevenday", null);
        }
    } 




    class sevendayData
    {
        public int whichday=1;  /*那一天*/
        public Dictionary<int,award> task_award;
        public login_awd loginaed;
        public half_buy halfbuy;        
    }
    //登陆
    class login_awd
    {
        public int item_id;
        public int item_num;
        public int point;
        public int tab;
        public string des;
        public int state = 2;//0没领，1领了，2不可领
    }
    //半价
    class half_buy
    {
        public int shop_item;
        public int shop_num;
        public int point;
        public int cost;
        public int tab;
        public string des;
        public bool isReceive = false;//是否购买
    }
    //其他:
    class award
    {
        public int tab;
        public int award_id;
        /*1: 当日消费
         *2：强化装备次数
         *3：升级宠物
         *4：升级技能 
         *5：升级翅膀 
         *6：镶嵌宝石
         *7：寻宝次数
         *8：角色等级
         *9：单人副本
         *10：组队副本
         *11：上架物品
         *12：合成宝石
         *13：首领击杀
         *14：竞技场
         *15：战场
         *16：圣器升级         
        */
        public int task_type;       
        public string task_des;
        public int task_point;
        public int state = 0;//0未完成，1已完成，2已领取
        public int task_have = 0;//任务进度
        public string task_need;//任务总需求
        public List<task_awards> lst_ta;
    }
    //奖励
    class task_awards
    {
        public int id;
        public int value;
    }

    //积分奖励
    class four_box
    {
        public int id;
        public int param1;
        public int state = 0;
        public task_awards lst_ta;
    }

}
