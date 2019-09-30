using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame;
using System.Collections;
using UnityEngine;


namespace MuGame
{
    class A3_EliteMonsterModel : ModelBase<A3_EliteMonsterModel>
    {
        public Dictionary<int, bossrankingdata> dic_bsk = new Dictionary<int, bossrankingdata>();


        public int kill_cnt = 0;                         /*已完成次数*/
        public int vip_buy_cnt = 0;                      /*已购买次数*/
        public Dictionary<int, List<dmg_list>> dic_dmg_lst = new Dictionary<int, List<dmg_list>>();/*排行数据*/
        #region elite monster
        public Dictionary<uint /* mon id */ , EliteMonsterInfo> dicEMonInfo;
        private List<uint> listSortedEMonId;
        public List<uint> ListSortedEMonId
        {
            set { listSortedEMonId = value; }
            get { return listSortedEMonId ?? GetSortedMonInfoIdList(); }
        }
        #endregion
        public A3_EliteMonsterModel()
        {
            MWLR_strf(1, ref s10, ref s11, ref s12, ref s13, ref i11, ref i12);
            MWLR_strf(2, ref s20, ref s21, ref s22, ref s23, ref i21, ref i22);
            MWLR_strf(3, ref s30, ref s31, ref s32, ref s33, ref i31, ref i32);
            dicEMonInfo = new Dictionary<uint, EliteMonsterInfo>();
            ListSortedEMonId = new List<uint>();
        }

        //↓世界Boss副本
        public string s10 = "", s11 = "", s12 = "", s20 = "", s21 = "", s22 = "", s30 = "", s31 = "", s32 = "";// ←背景信息
        public List<int> s13 = new List<int>(), s23 = new List<int>(), s33 = new List<int>();// ←奖励信息
        public int i11, i12, i21, i22, i31, i32;// ←刷新时间
        public int[] bossid = new int[20];// ←当前准备刷新的Boss(0没有,1,2,3)
        public int[] boss_status = new int[20];// ←1活着, 2死了, 3没有

        void MWLR_strf(int i, ref string s0, ref string s1, ref string s2, ref List<int> s3, ref int i1, ref int i2)
        {
            var xm = XMLMgr.instance.GetSXML("worldboss");
            var bx = xm.GetNode("droplist", "id==" + i);
            s0 = xm.GetNode("boss", "id==" + i).getString("name");
            string[] ss = xm.GetNode("boss", "id==" + i).getString("time").Split(',');
            i1 = int.Parse(ss[0]);
            i2 = int.Parse(ss[1]);
            s1 = bx.getString("intro1");
            s2 = bx.getString("intro2");
            var bli = bx.GetNodeList("equip");
            s3.Clear();
            foreach (var v in bli)
            {
                s3.Add(v.getInt("id"));
            }
        }

        public List<SXML> getxml_jingbi(int mid,int ranking)
        {
         return   XMLMgr.instance.GetSXML("worldboss.boss_awd", "mid==" + mid).GetNode("rank_awd", "rank==" + ranking).GetNodeList("RewardValue");
        }
        public List<SXML> getxml_item(int mid, int ranking)
        {
            return XMLMgr.instance.GetSXML("worldboss.boss_awd", "mid==" + mid).GetNode("rank_awd", "rank==" + ranking).GetNodeList("RewardItem");
        }
        public List<uint> GetSortedMonInfoIdList()
        {
            List<uint> listMonId = new List<uint>(dicEMonInfo.Keys);
            listMonId.Sort((first, second) =>
            {
                if (dicEMonInfo[first].upLv.Value > dicEMonInfo[second].upLv.Value)
                    return 1;
                if (dicEMonInfo[first].upLv.Value == dicEMonInfo[second].upLv.Value)
                {
                    if (dicEMonInfo[first].lv.Value > dicEMonInfo[second].lv.Value)
                        return 1;
                    if (dicEMonInfo[first].lv.Value == dicEMonInfo[second].lv.Value)
                        return 0;
                    if (dicEMonInfo[first].lv.Value < dicEMonInfo[second].lv.Value)
                        return -1;
                }
                return -1;
            });
            return ListSortedEMonId = listMonId;
        }

        public EliteMonsterInfo AddData(Variant data)
        {
            uint monId = data["mid"]._uint;
            EliteMonsterInfo monInfo;
            if (!dicEMonInfo.ContainsKey(monId))
            {
                monInfo = new EliteMonsterInfo(
                    lastKilledDate: data.ContainsKey("kill_tm") ? data["kill_tm"]._uint : 0,
                    respawnTime: data.ContainsKey("respawntm") ? data["respawntm"]._uint : 0,
                    killerName: data.ContainsKey("killer_name") ? data["killer_name"]._str : "",
                    mapId: data.ContainsKey("mapid") ? data["mapid"]._int : 0,
                    pos: data.ContainsKey("mon_x") && data.ContainsKey("mon_y") ? new Vector2(data["mon_x"]._int, data["mon_y"]._int) : Vector2.zero,
                    monId: monId);
                dicEMonInfo.Add(monId, monInfo);
                ReadRewardXml(monId);
            }
            else
            {
                monInfo = dicEMonInfo[monId];
            }
            return monInfo;
        }

        public void LoadReward(uint monId) => ReadRewardXml(monId);
        private void ReadRewardXml(uint monId)
        {
            List<uint> listReward = new List<uint>();
            SXML xml = XMLMgr.instance.GetSXML("worldboss");
            SXML rewardDrop = xml.GetNode("mdrop", "mid==" + monId);
            if (rewardDrop == null) return;

            listReward.Clear();
            List<SXML> listRewardNode;
            listRewardNode = rewardDrop.GetNodeList("item");
            for (int j = 0; j < listRewardNode.Count; j++)
                listReward.Add(listRewardNode[j].getUint("id"));
            if (!EliteMonsterInfo.poolItemReward.ContainsKey(monId))
                EliteMonsterInfo.poolItemReward.Add(monId, listReward);
        }
    }
}
class EliteMonsterInfo
{
    public static Dictionary<uint /*monId*/, List<uint /*item id*/> /*reward item list*/> poolItemReward = new Dictionary<uint, List<uint>>();
    public int mapId;
    public uint lastKilledTime;
    public uint unRespawnTime;
    public string date;
    public string killerName;
    public Vector2 pos;
    public uint? lv;
    public uint? upLv;
    public List<uint> rewardItem;
    private uint monId;
    public uint MonId
    {
        get { return monId; }
        set
        {
            monId = value;
            if (!lv.HasValue || !upLv.HasValue)
            {
                lv = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getUint("lv");
                upLv = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getUint("zhuan");
            }
        }
    }

    public EliteMonsterInfo(uint lastKilledDate, uint respawnTime, string killerName, int mapId, Vector2 pos, uint monId)
    {
        lastKilledTime = lastKilledDate;
        date = GetDateBySec(lastKilledDate);
        unRespawnTime = respawnTime;

        this.killerName = killerName;
        this.mapId = mapId;
        this.pos = pos;
        this.MonId = monId;
    }

    private string GetDateBySec(uint sec)
    {
        if (sec == 0)
            return null;
        return new DateTime(year: 1970, month: 1, day: 1, hour: 8, minute: 0, second: 0).AddSeconds(sec).ToString("yyyy-MM-dd HH:mm");
    }
}
class dmg_list
{
    public int mid;
    public int cid;
    public string name;
    public int dmg;
    public int rank;
    public string lat_name;
}

class bossrankingdata
{
    public int cid;
    public string name;
    public uint dmg;
}

