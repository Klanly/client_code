using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    enum Game_stage {
        nul = 0, // 初始
        find = 1, //匹配中
        sure = 2, // 确认中
        ture_game  //接受比赛
    }
    class a3_sportsModel : ModelBase<a3_sportsModel>
    {

        public a3_sportsModel() : base()
        {

            SXML Xml = XMLMgr.instance.GetSXML("jjc.info");
            if (Xml == null) return;

            buy_cnt = A3_VipModel.getInstance().vip_exchange_num(7);

            callenge_cnt = Xml.getInt("callenge_cnt");

            buy_zuan_count = Xml.getInt("buy_cost");

        }

        public Dictionary<int, info_teamPlayer> GameInfo = new Dictionary<int, info_teamPlayer>();
        public int Score_jdzc;
        public int Ranking_jdzc;
        public float tosureTime = 20;

        public Game_stage sport_stage = Game_stage.nul;

        //竞技场
        public int pvpCount = 0; //匹配次数
        public int buyCount = 0; //购买次数
        public int callenge_cnt = 0;
        public int buy_cnt

        {
            get { return A3_VipModel.getInstance().vip_exchange_num( 7 ); }
            set
            {
                if ( buy_cnt == value )
                    return;

                buy_cnt = value;
              
            }
        }
        public int score = 0; //分数
        public int grade = 0; //段位
        public int lastgrage = 0;//上届段位
        public int buy_zuan_count = 0;
        public int Canget = 0;

        public Dictionary<int, info_teamPlayer> getGameInfo(bool sort = false) {
            if (!sort)
                return GameInfo;
            else {
                List<info_teamPlayer> itemList = new List<info_teamPlayer>();
                itemList.AddRange(GameInfo.Values);
                itemList.Sort();
                Dictionary<int, info_teamPlayer> SortItems = new Dictionary<int, info_teamPlayer>();
                foreach (info_teamPlayer data in itemList)
                {
                    SortItems[data.cid] = data;
                }
                return SortItems;
            }
        }

        //据点战场
        public int kill_count = 0;
        public int die_count = 0;
        public int helpkill_count = 0;


    }



    public class playerinfo
    {
        public int cid;
        public int carr;
        public GameObject conobj;
    }

    public class OtherPlayerPos_jdzc
    {
        public int iid;
        public int lvlsideid;
        public uint x;
        public uint y;
    }



    public class info_teamPlayer : IComparable<info_teamPlayer>
    {
        public int cid;
        public string name;
        public int carr;
        public int zhuan;
        public int lvl;
        public int lvlsideid;//阵营
        public int kill_cnt = 0;
        public int die_cnt = 0;
        public int assists_cnt = 0;
        public double dmg = 0;
        public int ach_point = 0;

        public int CompareTo(info_teamPlayer other)
        {
            if (ach_point > other.ach_point)
            {
                return -1;
            }
            else if (ach_point < other.ach_point) { return 1; }
            else {
                if (kill_cnt > other.kill_cnt) return -1;
                else if (kill_cnt < other.kill_cnt) return 1;
                else return 0;
            }

        }
    }
}
