using System;
using System.Collections.Generic;
using System.Linq;

namespace MuGame
{
    enum SMITHY_PART
    {
        NOT_LEARNT = 0,
        WEAPON = 2,
        ARMOR = 1,
        JEWELRY = 3
    }
    class A3_SmithyModel:ModelBase<A3_SmithyModel>
    {
        //public static uint AvaiablePart { get; set; } = 0;
        public int rlrnDiamondCost, rlrnMoneyCost;
        public Dictionary<uint /* id */ , List<MatInfo>> dicSmithyInfo;
        public Dictionary<int /* type */ , List<MatInfo>> smithyInfoDicUseScroll;
        public Dictionary<uint /* id */, SmithyEqpInfo> dicSmithyItemInfo;
        public List<SmithyInfo> smithyLevelInfoList;

        //public List<int /* set */> expList;

        public int CurSmithyExp { set; get; }
        public int CurSmithyMaxExp { get; set; }
        public int CurSmithyLevel { set; get; }

        public SMITHY_PART CurSmithyType { set; get; } = SMITHY_PART.NOT_LEARNT;

        public A3_SmithyModel()
        {
            List<SXML> forgeItemList = XMLMgr.instance.GetSXMLList("forge.forge_eqp");
            SXML forgeCost = XMLMgr.instance.GetSXML("forge.cost_way", "way==1");
            dicSmithyInfo = new Dictionary<uint, List<MatInfo>>();
            dicSmithyItemInfo = new Dictionary<uint, SmithyEqpInfo>();
            for (int i = 0; i < forgeItemList.Count; i++)
            {
                uint eqpId = forgeItemList[i].getUint("eqp_id");
                int costListId = forgeItemList[i].getInt("forge_cost");
                List<SXML> itemCost = forgeCost.GetNode("cost", "forge_cost==" + costListId).GetNodeList("item_cost");

                dicSmithyItemInfo[eqpId] = new SmithyEqpInfo();
                dicSmithyItemInfo[eqpId].tpid = eqpId;
                dicSmithyItemInfo[eqpId].forgeCostId = costListId;
                dicSmithyItemInfo[eqpId].money = forgeItemList[i].getInt("money");
                dicSmithyItemInfo[eqpId].exp = forgeItemList[i].getInt("exp");
                dicSmithyItemInfo[eqpId].forgeLvl = forgeItemList[i].getInt("forge_lvl");

                dicSmithyInfo[eqpId] = new List<MatInfo>();
                for (int j = 0; j < itemCost.Count; j++)
                {
                    uint itemId = itemCost[j].getUint("item_id");
                    int num = itemCost[j].getInt("nums");
                    if (itemId > 0 && num > 0)
                        dicSmithyInfo[eqpId].Add(new MatInfo { tpid = itemId, num = num });
                }

                dicSmithyItemInfo[eqpId].matList = dicSmithyInfo[eqpId];
            }
            forgeCost = XMLMgr.instance.GetSXML("forge.cost_way", "way==2");
            smithyInfoDicUseScroll = new Dictionary<int, List<MatInfo>>();
            List<SXML> itemCostUseScroll = XMLMgr.instance.GetSXMLList("forge.forge_type");
            for (int i = 0; i < itemCostUseScroll.Count; i++)
            {
                int typeId = itemCostUseScroll[i].getInt("type");
                if (typeId > 0)
                {
                    if(!smithyInfoDicUseScroll.ContainsKey(typeId))
                        smithyInfoDicUseScroll[typeId] = new List<MatInfo>();
                    smithyInfoDicUseScroll[typeId].Add(new MatInfo { tpid = itemCostUseScroll[i].getUint("item_id"), num = itemCostUseScroll[i].getInt("nums") });
                    ScrollCostData scrlCostData = new ScrollCostData();
                    scrlCostData.money = itemCostUseScroll[i].getInt("money");
                    scrlCostData.costId = 1;                    
                    SmithyEqpInfo.dicScrollCost[typeId] = scrlCostData;
                }
            }
            itemCostUseScroll = forgeCost.GetNode("cost", "forge_cost==1").GetNodeList("item_cost");
            for (int i = 0; i < itemCostUseScroll.Count; i++)
            {                
                uint itemId = itemCostUseScroll[i].getUint("item_id");
                int num = itemCostUseScroll[i].getInt("nums");
                int type = itemCostUseScroll[i].getInt("type");
                
                if (itemId > 0 && num > 0)
                {
                    int j = 0;
                    for (List<int> idx = new List<int>(smithyInfoDicUseScroll.Keys); j < idx.Count; j++)
                    {
                        smithyInfoDicUseScroll[idx[j]].Add(new MatInfo { tpid = itemId, num = num });
                        SmithyEqpInfo.dicScrollCost[idx[j]].matList = smithyInfoDicUseScroll[idx[j]];
                    }
                }
               
            }
            List<SXML> forgeLv = XMLMgr.instance.GetSXMLList("forge.forge_lvl");
            smithyLevelInfoList = new List<SmithyInfo>();
            for (int i = 0; i < forgeLv.Count; i++)
            {
                int level = forgeLv[i].getInt("lvl");
                int expToNextLevel = forgeLv[i].getInt("exp");
                int maxAllowedSetLv = forgeLv[i].getInt("max_set_lv");
                if (!(maxAllowedSetLv > 0))
                    maxAllowedSetLv = (level + 1) / 2;
                if (level > 0)
                    smithyLevelInfoList.Add(new SmithyInfo { Level = level, ExpToNextLevel = expToNextLevel, MaxAllowedSetLv = maxAllowedSetLv });
            }
            rlrnDiamondCost = XMLMgr.instance.GetSXML("forge.relearn").getInt("gem");
            rlrnMoneyCost = XMLMgr.instance.GetSXML("forge.relearn").getInt("gold");
        }
        
        public List<MatInfo> GetMatListById(uint tpid)
        {            
            a3_EquipData equip = a3_EquipModel.getInstance().getEquipByItemId(tpid);
            List<MatInfo> listMat = new List<MatInfo>();
            if (dicSmithyInfo.ContainsKey(tpid))
                for (int i = 0; i < dicSmithyInfo[tpid].Count; i++)
                    listMat.Add(new MatInfo { tpid = dicSmithyInfo[tpid][i].tpid, num = dicSmithyInfo[tpid][i].num });
            return listMat;
        }
        public List<MatInfo> GetMatListUseScroll()
        {
            List<MatInfo> listMat = new List<MatInfo>();
            if(smithyInfoDicUseScroll.ContainsKey((int)CurSmithyType))
                return smithyInfoDicUseScroll[(int)CurSmithyType];
            return null;
        }
        public int GetMaxAllowedSetLevel(int level)
        {
            for (int i = 0; i < smithyLevelInfoList.Count; i++)
                if (smithyLevelInfoList[i].Level == level)
                    return smithyLevelInfoList[i].MaxAllowedSetLv;
            return 0;
        }
        public int CalcMaxExp(int level)
        {
            CurSmithyMaxExp = smithyLevelInfoList[level-1].ExpToNextLevel;
            return CurSmithyMaxExp;
        }
        public int GetMoneyCostById(uint tpid,int num = 1)
        {
            if (dicSmithyItemInfo.ContainsKey(tpid))
                return dicSmithyItemInfo[tpid].money * num;
            else
                return 0;                            
        }
        public int GetMoneyCostByScroll(int num = 1) 
        {
            if (SmithyEqpInfo.dicScrollCost.ContainsKey((int)CurSmithyType))
                return SmithyEqpInfo.dicScrollCost[(int)CurSmithyType].money * num;
            else
                return 0;
        }
    }

    class SmithyInfo
    {
        public int Level;
        public int ExpToNextLevel;
        public int MaxAllowedSetLv;
    }
    class MatInfo
    {
        public uint tpid;
        public int num;
    }

    class SmithyEqpInfo
    {
        public uint tpid;
        public List<MatInfo> matList = new List<MatInfo>();
        public int money;
        public int exp;        
        public int forgeLvl;
        public int forgeCostId;

        public static Dictionary<int /* type <=> tpid */, ScrollCostData> dicScrollCost =new Dictionary<int, ScrollCostData>();
    }
    class ScrollCostData
    {
        public int costId;
        public int money;
        public List<MatInfo> matList= new List<MatInfo>();
    }
}
