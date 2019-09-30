using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_SlayDragonModel : ModelBase<A3_SlayDragonModel>
    {
        public Dictionary<int, string> dicDragonName;
        public Dictionary<string, DragonData> dicDragonData;
        public Dictionary<int, DragonInfo> dicDragonInfo;

        public A3_SlayDragonModel()
        {
            dicDragonName = new Dictionary<int, string>();
            dicDragonData = new Dictionary<string, DragonData>();
            dicDragonInfo = new Dictionary<int, DragonInfo>();
            ReadDrgnInfo(); 
        }

        void ReadDrgnInfo()
        {
            List<SXML> dragXml = XMLMgr.instance.GetSXMLList("clan.clan_boss");            
            for (int i = 0; i < dragXml.Count; i++)
            {
                DragonInfo drgnInfo = new DragonInfo();                
                drgnInfo.clan_lv = dragXml[i].getUint("clan_lv");
                drgnInfo.item_cost = dragXml[i].getInt("item_cost");
                drgnInfo.item_id = dragXml[i].getUint("item_id");
                drgnInfo.diff_lvl = dragXml[i].getInt("diff_lvl");
                drgnInfo.pre_min = dragXml[i].getInt("tm_ready");
                drgnInfo.level_min = dragXml[i].getInt("tm");
                List<SXML> rewardList = dragXml[i].GetNodeList("award");
                drgnInfo.dragonName = new Dictionary<uint,string>();
                for (int j = 0; j < rewardList.Count; j++)
                {
                    drgnInfo.rewardList.Add(new RewardInfo { dragonId = rewardList[j].getUint("level_id"), itemId = rewardList[j].getUint("item_id") });
                    drgnInfo.dragonName[rewardList[j].getUint("level_id")] = rewardList[j].getString("name");
                }                
                dicDragonInfo[drgnInfo.diff_lvl] = drgnInfo;
            }
        }

        public void SyncData(Variant data)
        {
            List<Variant> dragonDataList = data["tulong_lvl_ary"]._arr;
            for (int i = 0; i < dragonDataList.Count; i++)
            {
                Variant dragondata = dragonDataList[i];
                string dragonName = dicDragonName[i];
                DragonData dragonData = new DragonData();
                dragonData.isUnlcoked = dragondata["zhaohuan"];
                dragonData.isDead = dragondata["death"]._bool;
                dragonData.proc = dragondata["jindu"]._uint;
                dragonData.isCreated = dragondata["create_tm"]._bool;
                dragonData.isOpened = dragondata["open"]._bool;
                dragonData.dragonId = dragondata["lvl_id"]._uint;
                if (dicDragonData.ContainsKey(dragonName))
                    if (!dicDragonData[dragonName].isUnlcoked && dicDragonData[dragonName].isUnlcoked ^ dragonData.isUnlcoked)
                        flytxt.instance.fly(ContMgr.getCont("slaydragon_over"));
                //flytxt.instance.fly("已解除封印");
                dicDragonData[dragonName] = dragonData;
            }
        }

        public int GetPreMin() => dicDragonInfo[GetUnlockedDiffLv()].pre_min;
        public int GetKillingTime() => dicDragonInfo[GetUnlockedDiffLv()].level_min - dicDragonInfo[GetUnlockedDiffLv()].pre_min;
        public int GetLvMin() => dicDragonInfo[GetUnlockedDiffLv()].level_min;

        public DragonData GetDataById(uint tpid)
        {
            List<string> idx = new List<string>(dicDragonData.Keys);
            for (int i = 0; i < idx.Count; i++)
                if (dicDragonData[idx[i]].dragonId == tpid)
                    return dicDragonData[idx[i]];
            return null;
        }

        public uint GetIdByName(string dragonName)
        {
            List<string> idx = new List<string>(dicDragonData.Keys);
            for(int i = 0; i < idx.Count; i++)
                if (idx[i].Equals(dragonName))
                    return dicDragonData[idx[i]].dragonId;
            return 0;
        }

        public string GetCurrentDragonName()
        {
            if(dicDragonInfo[GetUnlockedDiffLv()].dragonName.ContainsKey(GetUnlockedDragonId()))
                return dicDragonInfo[GetUnlockedDiffLv()].dragonName[GetUnlockedDragonId()];
            return null;
        }

        public int GetCost(int diffLvl=0)
        {
            if (diffLvl == 0)
                diffLvl = GetUnlockedDiffLv();
            if (dicDragonInfo.ContainsKey(diffLvl))
                return dicDragonInfo[diffLvl].item_cost;
            return -1;
        }

        public uint GetUnlockedDragonId()
        {
            List<string> idx = new List<string>(dicDragonData.Keys);
            for (int i = 0; i < idx.Count; i++)
                if (dicDragonData[idx[i]].isUnlcoked && !dicDragonData[idx[i]].isDead)
                    return dicDragonData[idx[i]].dragonId;
            return 0;
        }

        public DragonData GetUnlockedDragonData()
        {
            List<string> idx = new List<string>(dicDragonData.Keys);
            for (int i = 0; i < idx.Count; i++)
                if (dicDragonData[idx[i]].isUnlcoked && !dicDragonData[idx[i]].isDead)
                    return dicDragonData[idx[i]];
            return null;
        }

        public int GetUnlockedDiffLv()
        {
            if (A3_LegionModel.getInstance().myLegion.id != 0)
            {
                List<int> idx = new List<int>(dicDragonInfo.Keys);
                for (int i = idx.Count - 1; i >= 0; i--)
                    if (dicDragonInfo[idx[i]].clan_lv <= A3_LegionModel.getInstance().myLegion.lvl)
                        return dicDragonInfo[idx[i]].diff_lvl;
            }
            return 0;
        }

        public uint GetDragonKeyId()
        {
            if (A3_LegionModel.getInstance().myLegion.id != 0)
            {
                List<int> idx = new List<int>(dicDragonInfo.Keys);
                for (int i = idx.Count - 1; i >= 0; i--)
                    if (dicDragonInfo[idx[i]].clan_lv <= A3_LegionModel.getInstance().myLegion.lvl)
                        return dicDragonInfo[idx[i]].item_id;
            }
            return 0;
        }

        public DragonInfo GetCurDragonLvInfo()
        {
            if (A3_LegionModel.getInstance().myLegion.id != 0)
            {
                List<int> idx = new List<int>(dicDragonInfo.Keys);
                for (int i = idx.Count - 1; i >= 0; i--)
                    if (dicDragonInfo[idx[i]].clan_lv <= A3_LegionModel.getInstance().myLegion.lvl)
                        return dicDragonInfo[idx[i]];
            }
            return null;
        }

        public uint GetRewardIdByDragonId(uint dragonId)
        {
            List<RewardInfo> rwdLst = GetCurDragonLvInfo().rewardList;
            for (int i = 0; i < rwdLst.Count; i++)
                if (rwdLst[i].dragonId == dragonId)
                    return rwdLst[i].itemId;
            return 0;
        }

        public bool IsAbleToUnlock()
        {
            return A3_LegionModel.getInstance().myLegion.clanc >= 3;
        }

        public string GetNameById(uint dragonId)
        {
            List<string> idx = new List<string>(dicDragonData.Keys);
            for (int i = 0; i < idx.Count; i++)
                if (dicDragonData[idx[i]].dragonId == dragonId)
                    return idx[i];
            return null;
        }
    }

    class DragonData
    {
        public uint dragonId;
        public bool isUnlcoked;
        public bool isDead;
        public uint proc;
        public bool isCreated;
        public bool isOpened;
        public long endTimeStamp;
    }

    class DragonInfo
    {       
        public uint clan_lv;
        public uint item_id;
        public int item_cost;
        public int diff_lvl;
        public int level_min;
        public int pre_min;
        public List<RewardInfo> rewardList = new List<RewardInfo>();
        public Dictionary<uint, string> dragonName;
    }

    class RewardInfo
    {
        public uint dragonId;
        public uint itemId;
    }
}
