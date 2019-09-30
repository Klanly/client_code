using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;

namespace MuGame
{
    class WelfareModel : ModelBase<WelfareModel>
    {
        SXML itemsXMLList;
        Dictionary<a3_ItemData,uint> itemDataList;
        public WelfareModel():base()  
        {
            itemsXMLList = XMLMgr.instance.GetSXML("welfare", null);
            itemDataList = new Dictionary<a3_ItemData, uint>();
        }
        Dictionary<uint,uint> getFirstChargeXml()
        {
            Dictionary<uint,uint> firstRechangeList = new Dictionary<uint,uint>();
            List<SXML> itemFirstCharge = itemsXMLList.GetNodeList("firstcharge");
            for (int j = 0; j < itemFirstCharge.Count; j++)
            {
                SXML item = itemFirstCharge[j];
                List<SXML> itemIds = itemFirstCharge[j].GetNodeList("item", null);
                for (int i = 0; i < itemIds.Count; i++)
                {
                    uint id = itemIds[i].getUint("id");
                    uint carr = itemIds[i].getUint("carr");
                    firstRechangeList.Add(id, carr);
                }
            }
            return firstRechangeList;
        }
        public Dictionary<a3_ItemData, uint> getFirstChargeDataList()//首充奖励
        {
            itemDataList.Clear();

            Dictionary<uint,uint> itemIds = getFirstChargeXml();

            foreach (KeyValuePair<uint, uint> item in itemIds)
            {
                uint id = item.Key;
                a3_ItemData data = a3_BagModel.getInstance().getItemDataById(id);
                itemDataList.Add(data,item.Value);
            }
            return itemDataList;
        }
        public List<itemWelfareData> getDailyLogin()//每日登陆奖励
        {
            List<itemWelfareData> iwdList=new List<itemWelfareData>();
            List<SXML> itemDailyreward = itemsXMLList.GetNodeList("dailyreward");
            for (int j = 0; j < itemDailyreward.Count; j++)
            {
                SXML item = itemDailyreward[j];
                List<SXML> itemIds = itemDailyreward[j].GetNodeList("day", null);
                for (int i = 0; i < itemIds.Count; i++)
                {
                    uint id = itemIds[i].getUint("id");
                    uint itemId = itemIds[i].getUint("item_id");
                    uint num = itemIds[i].getUint("num");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;
        }
        public List<itemWelfareData> getOLReward()//在线奖励
        {
            List<itemWelfareData> iwdList = new List<itemWelfareData>();
            List<SXML> itemOLreward = itemsXMLList.GetNodeList("olreward");
            for (int j = 0; j < itemOLreward.Count; j++)
            {
                SXML item = itemOLreward[j];
                List<SXML> itemIds = itemOLreward[j].GetNodeList("times", null);
                for (int i = 0; i < itemIds.Count; i++)
                {
                    uint id = itemIds[i].getUint("id");
                    uint itemId = itemIds[i].getUint("need_time");
                    uint num = itemIds[i].getUint("num");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;
        }
        public List<itemWelfareData> getLevelReward()//角色升级奖励
        {
            List<itemWelfareData> iwdList = new List<itemWelfareData>();
            List<SXML> itemLevelReward = itemsXMLList.GetNodeList("level_reward");
            for (int j = 0; j < itemLevelReward.Count; j++)
            {
                SXML item = itemLevelReward[j];
                List<SXML> itemLVLs = itemLevelReward[j].GetNodeList("level", null);
                for (int i = 0; i < itemLVLs.Count; i++)
                {
                    uint id = itemLVLs[i].getUint("id");
                    uint zhuan = itemLVLs[i].getUint("zhuan");
                    uint lvl = itemLVLs[i].getUint("lvl");
                    uint itemId = itemLVLs[i].getUint("item_id");
                    uint num = itemLVLs[i].getUint("num");
                    uint award = itemLVLs[i].getUint("award");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.zhuan = zhuan;
                    iwd.lvl = lvl;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwd.award_num = award;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;
        }
        public List<itemWelfareData> getCumulateRechargeAward()//累积充值奖励
        {
            List<itemWelfareData> iwdList = new List<itemWelfareData>();
            List<SXML> itemLevelReward = itemsXMLList.GetNodeList("charge_cumulate");
            for (int j = 0; j < itemLevelReward.Count; j++)
            {
                SXML item = itemLevelReward[j];
                List<SXML> itemCharges = itemLevelReward[j].GetNodeList("charge", null);
                for (int i = 0; i < itemCharges.Count; i++)
                {
                    uint id = itemCharges[i].getUint("id");
                    uint cumulate = itemCharges[i].getUint("cumulate");
                    uint itemId = itemCharges[i].getUint("item_id");
                    uint num = itemCharges[i].getUint("num");
                    uint worth = itemCharges[i].getUint("worth");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.cumulateNum = cumulate;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwd.worth = worth;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;

        }
        public List<itemWelfareData> getCumulateConsumption()//累积消费奖励
        {
            List<itemWelfareData> iwdList = new List<itemWelfareData>();
            List<SXML> itemLevelReward = itemsXMLList.GetNodeList("consumption_cumulate");
            for (int j = 0; j < itemLevelReward.Count; j++)
            {
                SXML item = itemLevelReward[j];
                List<SXML> itemConsumption = itemLevelReward[j].GetNodeList("consumption", null);
                for (int i = 0; i < itemConsumption.Count; i++)
                {
                    uint id = itemConsumption[i].getUint("id");
                    uint cumulate = itemConsumption[i].getUint("cumulate");
                    uint itemId = itemConsumption[i].getUint("item_id");
                    uint num = itemConsumption[i].getUint("num");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.cumulateNum = cumulate;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;

        }
        public List<itemWelfareData> getDailyRecharge()//每日充值奖励
        {
            List<itemWelfareData> iwdList = new List<itemWelfareData>();
            List<SXML> itemLevelReward = itemsXMLList.GetNodeList("daily_charge");
            for (int j = 0; j < itemLevelReward.Count; j++)
            {
                SXML item = itemLevelReward[j];
                List<SXML> itemRcharge = itemLevelReward[j].GetNodeList("charge", null);
                for (int i = 0; i < itemRcharge.Count; i++)
                {
                    uint id = itemRcharge[i].getUint("id");
                    uint cumulate = itemRcharge[i].getUint("cumulate");
                    uint itemId = itemRcharge[i].getUint("item_id");
                    uint num = itemRcharge[i].getUint("num");
                    itemWelfareData iwd = new itemWelfareData();
                    iwd.id = id;
                    iwd.cumulateNum = cumulate;
                    iwd.itemId = itemId;
                    iwd.num = num;
                    iwdList.Add(iwd);
                }
            }
            return iwdList;
        }
        //写几个方法判断是否有可领却没领的奖励（福利的iconlight）

        public bool for_dengjilibao(List<Variant> lst_zhuan)
        {
            uint zhuan = PlayerModel.getInstance().up_lvl;
            //return common(zhuan, lst_zhuan, getLevelReward());           
            if(lst_zhuan.Count>0)
            {
                lst_zhuan.OrderBy(i => i.Values);
                return zhuan > lst_zhuan[lst_zhuan.Count - 1] ? true : false;
            }             
            else
              return zhuan >= getLevelReward()[0].zhuan? true : false;

        }
        public bool for_leijichongzhi(List<Variant> lst_leijichongzhi)
        {
            uint num = welfareProxy.totalRecharge;
            return common(num, lst_leijichongzhi, getCumulateRechargeAward());
        }
        public bool for_leixjixiaofei(List<Variant> lst_leijixiaofei)
        {
            uint num = welfareProxy.totalXiaofei;
            return common(num, lst_leijixiaofei, getCumulateConsumption());
        }
        public bool for_jinrichongzhi(List<Variant> lst_chongzhi_today)
        {
            uint num = welfareProxy.todayTotal_recharge;
            return common(num, lst_chongzhi_today, getDailyRecharge());
        }
        bool common(uint num,List<Variant> lst,List<itemWelfareData> data)
        {
            int id=0;
            if (lst.Count > 1)
            {
                lst.OrderBy(i=>i.Values);
                id = lst[lst.Count - 1];
            }
            if (id < 10)
                return num >= data[id + 1].cumulateNum ? true : false;               
            else
                return false;
        }
       public struct itemWelfareData
        {
            public string desc;
            public string strIcon;
            public string strToggle;
            public uint num;
            public uint id;
            public uint itemId;
            public uint cumulateNum;//当日累积充值的钻石量
            public uint timesId;
            public uint needTime;
            public uint rewardId;
            public uint stateId;
            public uint last;
            public uint zhuan;
            public uint lvl;
            public uint worth;//价值
            public string awardName;
            public uint award_num; //转生获取的砖石量
        }

    }
}
