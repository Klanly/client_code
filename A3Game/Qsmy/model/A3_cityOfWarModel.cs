using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    class A3_cityOfWarModel : ModelBase<A3_cityOfWarModel>
    {
        public int apply_tm_day;
        public int apply_tm_start_h;
        public int apply_tm_end_h;
        public int min_cost;
        public int max_cost ;
        public int open_tm_day;
        public int open_tm_start_h;
        public int open_tm_start_m;
        public int open_tm_end_h;
        public int open_tm_end_m;

        public int one_change_cost ;

        public int atk_num = 0;
        public int def_num = 0;

        public bool door_open = false;


        public A3_cityOfWarModel() : base() {

            SXML Xml = XMLMgr.instance.GetSXML("clanwar");
            if (Xml == null) return;

            apply_tm_day = Xml.GetNode("apply_tm").getInt("date");
            apply_tm_start_h = Xml.GetNode("apply_tm").getInt("start_h");
            apply_tm_end_h = Xml.GetNode("apply_tm").getInt("end_h");
            min_cost = Xml.GetNode("apply_tm").getInt("min_cost");
            max_cost = Xml.GetNode("apply_tm").getInt("max_cost");
            one_change_cost = Xml.GetNode("apply_tm").getInt("change_min");
            open_tm_day = int.Parse(Xml.GetNode("open_tm").getString("date").Split(',')[0]);

            open_tm_start_h = Xml.GetNode("open_tm").GetNode("time").getInt("start_h");
            open_tm_start_m = Xml.GetNode("open_tm").GetNode("time").getInt("start_m");
            open_tm_end_h = Xml.GetNode("open_tm").GetNode("time").getInt("end_h");
            open_tm_end_m = Xml.GetNode("open_tm").GetNode("time").getInt("end_m");




        }

        public Apply_Info checkMineClan()
        {
            if (A3_LegionModel.getInstance().myLegion != null)
            {
                foreach (Apply_Info info in apply_list)
                {
                    if (info.clan_id == A3_LegionModel.getInstance().myLegion.id)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        public void gg() { Debug.Log("LLLL" + apply_tm_day + "P" + apply_tm_start_h + "P" + apply_tm_end_h + "P" + min_cost + "p" + open_tm_day + "P" + open_tm_start_h + "P" + open_tm_start_m + "P" + open_tm_end_h + "P" + open_tm_end_m); }

        public string GetTimestr(int tm) {
            string str;
            if (tm <= 9)
                str = "0" + tm;
            else
                str = tm.ToString();

            return str;
        }

        public long starTime = 0;
        public long endTime = 0;



        //  1 进攻方胜利  2 防守方胜利
        public int last_type;

        public int llid;

        public int def_clanid;

        public int start_tm;

        public int clan_pcid;

        public int clan_lvl;

        public string clan_name;

        public string Castellan_name;

        public int Castellan_zhuan;

        public int Castellan_lvl;

        public int Castellan_combpt;

        public int Castellan_carr;

        public Dictionary<int, defInfo> deflist = new Dictionary<int, defInfo>();

        public List<Apply_Info> apply_list = new List<Apply_Info>();

        public Dictionary<int, MonInfo> moninfos = new Dictionary<int, MonInfo>();

        public List<signalInfo> signalList = new List<signalInfo>();

        public Apply_Info GetApplyInfo_One(int idx) {
            if (apply_list.Count > 0)
            {
                if (apply_list.Count == 1)
                {
                    if (idx == 1)
                        return apply_list[0];
                    else return null;
                }
                else if (apply_list.Count == 2)
                {
                    if (idx == 1)
                        return apply_list[0];
                    else if (idx == 2)
                        return apply_list[1];
                    else return null;
                }
                else
                {
                    if (idx == 1)
                        return apply_list[0];
                    else if (idx == 2)
                        return apply_list[1];
                    else
                        return apply_list[2];
                }


            }
            else return null;

        }

        public string GetStage_Str() {
            string str = "";
            switch (last_type)
            {
                case 0:
                    str = ContMgr.getCont("War_stage0");
                    break;
                case 1:
                    str = ContMgr.getCont("War_stage1");
                    break;
                case 2:
                    str = ContMgr.getCont("War_stage2");
                    break;
                case 3:
                    str = ContMgr.getCont("War_stage3");
                    break;
                case 4:
                    str = ContMgr.getCont("War_stage4");
                    break;
                case 5:
                    str = ContMgr.getCont("War_stage5");
                    break;
                case 6:
                    str = ContMgr.getCont("War_stage6");
                    break;


            }

            return str;
        }



        public void SetMonInfo(List<Variant> l)
        {
            foreach (Variant v in l) {
                int id = v["mid"];
                int per = v["per"];

                if (moninfos.ContainsKey(id))
                {
                    moninfos[id].per = per;
                }
                else {
                    SXML sxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + id);
                    MonInfo mon = new MonInfo();
                    mon.mid = id;
                    mon.per = per;
                    mon.name = sxml.getString("name");
                    moninfos[id] = mon;
                }
            }
        }


        public bool CanInFB() {

            if (A3_LegionModel.getInstance().myLegion != null)
            {
                if (checkTime() == TimeType.WarStart)
                {
                    if (A3_LegionModel.getInstance().myLegion.id == def_clanid) return true;
                    foreach (Apply_Info info in apply_list) {
                        if (info.clan_id == A3_LegionModel.getInstance().myLegion.id)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public TimeType checkTime1() {
            gg();
            DateTime dateTime = DateTime.Now;
            if (open_tm_day < 7 && open_tm_day > apply_tm_day) {
                if (dateTime.DayOfWeek == GetWeek(apply_tm_day)) {
                    //竞标日
                    if (dateTime.Hour >= apply_tm_start_h && dateTime.Hour < apply_tm_end_h)
                    {
                        //竞标时间
                        return TimeType.ApplyTime;
                    }
                    else if (dateTime.Hour >= apply_tm_end_h) {
                        return TimeType.PrepareTime;
                    }
                    else if (dateTime.Hour < apply_tm_start_h) {
                        return TimeType.WarOver;
                    }
                }
                else if (dateTime.DayOfWeek > GetWeek(apply_tm_day) && dateTime.DayOfWeek < GetWeek(open_tm_day))
                {
                    //备战
                    return TimeType.PrepareTime;
                }
                else if (dateTime.DayOfWeek == GetWeek(open_tm_day)) {
                    //攻城日
                    if (dateTime.Hour > open_tm_start_h && dateTime.Hour < open_tm_end_h) {
                        //攻城时间
                        if (llid != 0)
                            return TimeType.WarStart;
                        else
                            return TimeType.WarOver;
                    }
                    else if (dateTime.Hour < open_tm_start_h) return TimeType.PrepareTime;
                    else if (dateTime.Hour > open_tm_end_h) return TimeType.WarOver;
                    else if (dateTime.Hour == open_tm_start_h) {
                        if (dateTime.Minute >= open_tm_start_m)
                        {
                            if (llid != 0)
                                return TimeType.WarStart;
                            else
                                return TimeType.WarOver;
                        }
                        else
                            return TimeType.PrepareTime;
                    }
                    else if (dateTime.Hour == open_tm_end_h) {
                        if (dateTime.Minute < open_tm_end_m)
                        {
                            if (llid != 0)
                                return TimeType.WarStart;
                            else
                                return TimeType.WarOver;
                        }
                        else
                            return TimeType.WarOver;
                    }
                }
            }
            else {
                if (dateTime.DayOfWeek == GetWeek(apply_tm_day))
                {
                    //竞标日
                    if (dateTime.Hour >= apply_tm_start_h && dateTime.Hour < apply_tm_end_h)
                    {
                        //竞标时间
                        return TimeType.ApplyTime;
                    }
                    else if (dateTime.Hour >= apply_tm_end_h)
                    {
                        return TimeType.PrepareTime;
                    }
                    else if (dateTime.Hour < apply_tm_start_h)
                    {
                        return TimeType.WarOver;
                    }
                }
                else if (dateTime.DayOfWeek > GetWeek(apply_tm_day) && dateTime.DayOfWeek <= GetWeek(open_tm_day - 1))
                {
                    //备战
                    return TimeType.PrepareTime;
                }
                else if (dateTime.DayOfWeek == GetWeek(open_tm_day))
                {
                    //攻城日
                    if (dateTime.Hour > open_tm_start_h && dateTime.Hour < open_tm_end_h)
                    {
                        //攻城时间
                        if (llid != 0)
                            return TimeType.WarStart;
                        else
                            return TimeType.WarOver;
                    }
                    else if (dateTime.Hour < open_tm_start_h) return TimeType.PrepareTime;
                    else if (dateTime.Hour > open_tm_end_h) return TimeType.WarOver;
                    else if (dateTime.Hour == open_tm_start_h)
                    {
                        if (dateTime.Minute >= open_tm_start_m)
                        {
                            if (llid != 0)
                                return TimeType.WarStart;
                            else
                                return TimeType.WarOver;
                        }
                        else
                            return TimeType.PrepareTime;
                    }
                    else if (dateTime.Hour == open_tm_end_h)
                    {
                        if (dateTime.Minute < open_tm_end_m)
                        {
                            if (llid != 0)
                                return TimeType.WarStart;
                            else
                                return TimeType.WarOver;
                        }
                        else
                            return TimeType.WarOver;
                    }
                }
            }

            return TimeType.WarOver;
        }



        //竞标 备战 攻城 休战
        public TimeType checkTime()
        {
            if (Check_ApplyTime()) { return TimeType.ApplyTime; }
            if (Check_PrepareTime()) { return TimeType.PrepareTime; }
            if (Check_WarStart()) { return TimeType.WarStart; }
            return TimeType.WarOver;
        }
        bool Check_ApplyTime() {
            DateTime dateTime = DateTime.Now;
            if (GetNumForWeek(dateTime.DayOfWeek) == apply_tm_day)
            {
                if (dateTime.Hour >= apply_tm_start_h && dateTime.Hour < apply_tm_end_h)
                    return true;
            }
            return false;
        }

        bool Check_PrepareTime() {
            DateTime dateTime = DateTime.Now;
            if (GetNumForWeek(dateTime.DayOfWeek) == apply_tm_day)
            {
                if (GetNumForWeek(dateTime.DayOfWeek) == open_tm_day)
                {
                    if (dateTime.Hour < open_tm_start_h)
                        return true;
                    else if (dateTime.Hour == open_tm_start_h)
                    {
                        if (dateTime.Minute < open_tm_start_m)
                            return true;
                    }
                }
                else
                    if (dateTime.Hour >= apply_tm_end_h) return true;
            }
            else if (GetNumForWeek(dateTime.DayOfWeek) > apply_tm_day && GetNumForWeek(dateTime.DayOfWeek) < open_tm_day)
            {
                return true;
            }
            else if (GetNumForWeek(dateTime.DayOfWeek) == open_tm_day)
            {
                if (dateTime.Hour < open_tm_start_h)
                    return true;
                else if (dateTime.Hour == open_tm_start_h)
                {
                    if (dateTime.Minute < open_tm_start_m)
                        return true;
                }
            }
            return false;
        }

        bool Check_WarStart()
        {
            DateTime dateTime = DateTime.Now;
            if (GetNumForWeek(dateTime.DayOfWeek) == open_tm_day)
            {
                if (dateTime.Hour > open_tm_start_h && dateTime.Hour < open_tm_end_h)
                {
                    if (llid != 0)
                        return true;
                }
                else if (dateTime.Hour == open_tm_start_h)
                {
                    if (dateTime.Minute >= open_tm_start_m)
                    {
                        if (llid != 0)
                            return true;
                    }
                }
                else if (dateTime.Hour == open_tm_end_h)
                {
                    if (dateTime.Minute < open_tm_end_m)
                    {
                        if (llid != 0)
                            return true;
                    }
                }
            }
            return false;
        }


        public int GetNumForWeek(DayOfWeek e)
        {
            switch (e) {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
            }
            return 1;
        }



        public DayOfWeek GetWeek(int day) {
            switch (day) {
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
                case 5:
                    return DayOfWeek.Friday;
                case 6:
                    return DayOfWeek.Saturday;
                case 7:
                    return DayOfWeek.Sunday;
            }
            return DayOfWeek.Monday;
        }



    }

    class defInfo {
        public int _type;
        public int _lvl;
    }

    class MonInfo {
        public int mid;
        public int per;
        public string name;
    }

    class Apply_Info : IComparable<Apply_Info>
    {
        public int clan_id;
        public string clan_name;
        public int apply_num;
        public int apply_tm;
        public int clan_lvl;
        public int CompareTo(Apply_Info other) {

            if (apply_num > other.apply_num)
                return -1;
            else if (apply_num < other.apply_num)
                return 1;
            else
            {
                if (apply_tm > other.apply_tm)
                    return 1;
                else if (apply_tm < other.apply_tm)
                    return -1;
                else return 0;
            }
        }
    }
    public class PlayerPos_cityWar
    {
        public int iid;
        public int lvlsideid;
        public uint x;
        public uint y;
    }
    public enum TimeType {
        ApplyTime = 1,
        PrepareTime = 2,
        WarStart = 3,
        WarOver = 4
    }

    public class signalInfo
    {
        public int lvlsideid;
        public int signalType;
        public int x;
        public int y;
        public int cd;
        public GameObject signalObj;
    }
}
