using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    class a3_newActiveModel : ModelBase<a3_newActiveModel>
    {

        public bool CanGet = false;

        SXML xml;
        long activeing_time;
        long getRew_time;
        long active_time_over;
        public a3_newActiveModel() : base()
        {
            xml = XMLMgr.instance.GetSXML("new_server_activity");
            active_time_over = (xml.GetNode("tm").getInt("activity_days") - 1 )* 24 * 3600;
            getRew_time = xml.GetNode("tm").getInt("activity_days") * 24 * 3600;
            activeing_time = (xml.GetNode("tm").getInt("accept_days") + xml.GetNode("tm").getInt("activity_days")) * 24 * 3600;
        }
        //[boss_award]:<Array:0>
        //[boss_kill]:<Array:0>
        //public int new_recharge;
        //public int old_recharge;
        public int recharge;
        public int pay;
        public int zhuan;
        public int lvl;
        public int combpt;
        public int pk;
        public int wing;

        public bool pay_awd = false;
        public bool level_awd = false;
        public bool combpt_awd = false;
        public bool pk_awd = false;
        public bool recharge1_awd = false;
        public bool wing_awd = false;

        public List<int> boss_awd = new List<int>();
        public List<int> recharge_awd = new List<int>();

        public Dictionary<int, int> boosKill = new Dictionary<int, int>();
        public Dictionary<int, Rankinfo> level_rank = new Dictionary<int, Rankinfo>();
        public Dictionary<int, Rankinfo> combpt_rank = new Dictionary<int, Rankinfo>();
        public Dictionary<int, Rankinfo> pk_rank = new Dictionary<int, Rankinfo>();
        public Dictionary<int, Rankinfo> wing_rank = new Dictionary<int, Rankinfo>();
        public Dictionary<int, Rankinfo> boss_kill_rank = new Dictionary<int, Rankinfo>();
        public  Dictionary<int, Rankinfo> pay_rank = new Dictionary<int, Rankinfo>();
        public Dictionary<int, Rankinfo> recharge_rank = new Dictionary<int, Rankinfo>();

        public bool CanGetREW = false;
        public bool Show_active = false;
        public long kaifu_tm = 0 ;
        public long kaifu_tm_over = 0;
        public long s_get_tm = 0;
        public long e_get_tm = 0;
        public void setTime( long t)
        {
            kaifu_tm = t;
            kaifu_tm_over = t + active_time_over;
            s_get_tm = t + getRew_time;
            e_get_tm = t + activeing_time;
            long time = muNetCleint.instance.CurServerTimeStamp - t;
            if (time < activeing_time)
            {
                Show_active = true;
            }
            else { Show_active = false; }

            if (time >= getRew_time && Show_active)
            {
                CanGetREW = true;
            }
            else {
                CanGetREW = false;
            }
            InterfaceMgr.doCommandByLua("a1_low_fightgame.onshow_newact", "ui/interfaces/low/a1_low_fightgame", Show_active);

        }

        public bool setGet() {
            bool can = false;
            SXML sxml = XMLMgr.instance.GetSXML("new_server_activity");

            if (CanGetREW == false) return false;
            int myRank_lvl = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().level_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_lvl = info.rank;
                    break;
                }
            }
            List<SXML> listL = xml.GetNodeList("activity", "type==" + 1);
            foreach (SXML x in listL)
            {
                if (level_awd ) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    if (myRank_lvl == 0) continue;
                    string[] rank = x.getString("rank_limit").Split(',');
                  
                    if (a3_newActiveModel.getInstance().zhuan >= int.Parse(param[0]))
                    {
                        if (myRank_lvl >= int.Parse(rank[0]) && myRank_lvl <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else {
                    if (a3_newActiveModel.getInstance().zhuan > int.Parse(param[0]))
                    {
                        return true;
                    }
                    else if (a3_newActiveModel.getInstance().zhuan == int.Parse(param[0])) {
                        if (a3_newActiveModel.getInstance().lvl >= int.Parse(param[1])) {
                            return true;
                        }
                    }
                }
            }

            int myRank_combpt = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().combpt_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_combpt = info.rank;
                    break;
                }
            }

            List<SXML> listL1 = xml.GetNodeList("activity", "type==" + 2);
            foreach (SXML x in listL1) {
                if (combpt_awd) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    string[] rank = x.getString("rank_limit").Split(',');
                    if (myRank_combpt == 0) continue;
                    if (a3_newActiveModel.getInstance().combpt >= int.Parse(param[0]))
                    {
                        if (myRank_combpt >= int.Parse(rank[0]) && myRank_combpt <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (a3_newActiveModel.getInstance().combpt >= int.Parse(param[0]))
                    {
                        return true;
                    }
                }
            }


            int myRank_pk = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().pk_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_pk = info.rank;
                    break;
                }
            }
            List<SXML> listL2 = xml.GetNodeList("activity", "type==" + 3);
            foreach (SXML x in listL2)
            {
                if (pk_awd) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    string[] rank = x.getString("rank_limit").Split(',');
                    if (myRank_pk == 0) continue;
                    if (a3_newActiveModel.getInstance().pk >= int.Parse(param[0]))
                    {
                        if (myRank_pk >= int.Parse(rank[0]) && myRank_pk <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (a3_newActiveModel.getInstance().pk >= int.Parse(param[0]))
                    {
                        return true;
                    }
                }
            }



            int myRank_wing = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().wing_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_wing = info.rank;
                    break;
                }
            }
            List<SXML> listL3 = xml.GetNodeList("activity", "type==" + 4);
            foreach (SXML x in listL3)
            {
                if (wing_awd) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    string[] rank = x.getString("rank_limit").Split(',');
                    if (myRank_wing == 0) continue;
                    if (a3_newActiveModel.getInstance().wing >= int.Parse(param[0]))
                    {
                        if (myRank_wing >= int.Parse(rank[0]) && myRank_wing <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (a3_newActiveModel.getInstance().wing >= int.Parse(param[0]))
                    {
                        return true;
                    }
                }
            }


            int myRank_recharge = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().recharge_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_recharge = info.rank;
                    break;
                }
            }
            List<SXML> listL4 = xml.GetNodeList("activity", "type==" + 5);
            foreach (SXML x in listL4)
            {
                if (recharge1_awd) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    string[] rank = x.getString("rank_limit").Split(',');
                    if (myRank_recharge == 0) continue;
                    if (a3_newActiveModel.getInstance().recharge >= int.Parse(param[0]))
                    {
                        if (myRank_recharge >= int.Parse(rank[0]) && myRank_recharge <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (a3_newActiveModel.getInstance().recharge >= int.Parse(param[0]))
                    {
                        return true;
                    }
                }
            }

            int myRank_pay = 0;
            foreach (Rankinfo info in a3_newActiveModel.getInstance().pk_rank.Values)
            {
                if (info.cid == PlayerModel.getInstance().cid)
                {
                    myRank_pay = info.rank;
                    break;
                }
            }
            List<SXML> listL5 = xml.GetNodeList("activity", "type==" + 6);

            foreach (SXML x in listL5)
            {
                if (pay_awd) { break; }
                string[] param = x.getString("param_limit").Split(',');
                if (x.getString("rank_limit") != "null")
                {
                    string[] rank = x.getString("rank_limit").Split(',');
                    if (myRank_recharge == 0) continue;
                    if (a3_newActiveModel.getInstance().pay >= int.Parse(param[0]))
                    {
                        if (myRank_pay >= int.Parse(rank[0]) && myRank_pay <= int.Parse(rank[1]))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (a3_newActiveModel.getInstance().pay >= int.Parse(param[0]))
                    {
                        return true;
                    }
                }
            }

            return can;
        }





    }


    public class Rankinfo
    {
        public int rank;
        public int cid;
        public int carr;
        public int zhuan;
        public int lvl;
        public string name;
        public int total_win;
        public int combpt;
        public int mid;
        public int num;
        public int pay_num;
        public int recharge_num;
        public int stage_wing;
        public int level_wing;
    }
    public class rewInfo
    {
        public int rank;
        public int mid;
        public GameObject obj;
        public int pay_value;
        public uint rewid;
        public int type; // 1排行要求，2自身要求
        public int minRank;
        public int maxRank;
        public int minValue;
        public int maxValue;
    }

}
