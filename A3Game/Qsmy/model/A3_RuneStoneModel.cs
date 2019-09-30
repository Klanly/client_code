using System;
using System.Linq;
using System.Collections.Generic;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    class A3_RuneStoneModel : ModelBase<A3_RuneStoneModel>
    {

        public int nowstamina = 50; //体力值
        public int nowlv = 1;       //等级
        public int nowexp = 0;      //经验值

        public Dictionary<uint, a3_BagItemData> dressup_runestones = new Dictionary<uint, a3_BagItemData>();//初始化身上穿的
      
        public A3_RuneStoneModel()
        {
            
        }
        public Dictionary<uint, a3_BagItemData> getHaveRunestones()
        {
            return a3_BagModel.getInstance().getRunestonrs();
        }
        public a3_BagItemData getA3_BagItemDataById(uint id)
        {
            if (a3_BagModel.getInstance().getItems().ContainsKey(id))
            {
                return a3_BagModel.getInstance().getItems()[id];
            }else
                 return new a3_BagItemData();
        }
        public void initDressupInfos(List<Variant> arr)
        {
            foreach (Variant data in arr)
            {
                //DressupInfos(data);

                dressup_runestones[DressupInfos(data).id] = DressupInfos(data);
            }

        }
        public a3_BagItemData DressupInfos(Variant data)
        {
            a3_BagItemData itemData = new a3_BagItemData();
            itemData.id = data["fushi"]["id"];
            itemData.tpid = data["fushi"]["tpid"];
            itemData.isrunestone = true;
            if (data["fushi"].ContainsKey("mark"))
                itemData.ismark = data["fushi"]["mark"];
            if (data["fushi"].ContainsKey("stone_att"))
            {
                foreach (Variant i in data["fushi"]["stone_att"]._arr)
                {
                    itemData.runestonedata.runeston_att = new Dictionary<int, int>();
                    int att_type = i["att_type"];
                    int att_value = i["att_value"];
                    itemData.runestonedata.runeston_att[att_type] = att_value;
                }
            }
            return itemData;
            
        }







        public int getNeedMoney(int stone_id)
        {
            return XMLMgr.instance.GetSXML("item.rune_stone", "id==" + stone_id).getInt("money");
        }
        public int nowStamina(int v)
        {
            nowstamina = v;
            return Mathf.Clamp(nowstamina, 0, 50);
        }
        public int nowLv(int lv)
        {
            nowlv = lv;
            return Mathf.Clamp(nowlv, 1, 10);
        }
        public int nowExp(int exp)
        {
            nowexp = exp;
            return nowexp;
        }
    }
}
