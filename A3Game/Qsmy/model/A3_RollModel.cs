using Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    class A3_RollModel : ModelBase<A3_RollModel>
    {
        public  Dictionary<uint, ROllItem>  rollMapping
        {
             //set { rollItemMapping = value; }
             get { return rollItemMapping;}

         }
        public Dictionary<uint, Roll> rollReusultMapping
        {
            get { return rollResultMapping; }
        }

        private Dictionary<uint, ROllItem> rollItemMapping = null;

        private Dictionary<uint, Roll> rollResultMapping = null;

        public A3_RollModel()
        {
            if (rollItemMapping == null) rollItemMapping = new Dictionary<uint, ROllItem>();

            if (rollResultMapping == null) rollResultMapping = new Dictionary<uint, Roll>();

        }

        public void SetRollDropItem(List<Variant> s2cDataLst)
        {
            if ( s2cDataLst == null) return;

            for (int i = 0; i < s2cDataLst.Count; i++)
            {
                ROllItem rollItem = new ROllItem();
                rollItem.dpid = s2cDataLst[i]["dpid"];
                rollItem.tp = s2cDataLst[i]["tp"];
                rollItem.left_tm = s2cDataLst[i]["left_tm"];
                rollItem.teamid = s2cDataLst[i]["teamid"];
                rollItem.roll_tm = s2cDataLst[i]["roll_tm"];
                rollItem.roll_owner = s2cDataLst[i]["roll_owner"];

                if (s2cDataLst[i].ContainsKey("eqp"))
                {
                    Variant eqpary = s2cDataLst[i]["eqp"];

                    a3_BagItemData itemData = new a3_BagItemData();

                    if (eqpary.ContainsKey("tpid"))
                        itemData.tpid = eqpary["tpid"];

                    if (eqpary.ContainsKey("bnd"))
                        itemData.bnd = eqpary["bnd"];

                    a3_EquipModel.getInstance().equipData_read(itemData, eqpary);

                    itemData.confdata = a3_BagModel.getInstance().getItemDataById(itemData.tpid);

                    rollItem.eqpData = itemData;

                }

                if (s2cDataLst[i].ContainsKey("itm"))
                {
                    Variant itm = s2cDataLst[i]["itm"];
                    a3_BagItemData itemData = new a3_BagItemData();
                    itemData.tpid = itm["id"];
                    itemData.num = itm["cnt"];
                    itemData.confdata = a3_BagModel.getInstance().getItemDataById(itemData.tpid);
                    rollItem.itemData = itemData;
                }

               rollItemMapping[rollItem.dpid] = rollItem;

               if(a3_RollItem.single != null) a3_RollItem.single.AddRollItemGo(rollItem);

            }
        }

        public void RemoveRollDropItem(uint dropId, bool isClear = false)
        {
            if (isClear) {  rollItemMapping.Clear(); rollReusultMapping.Clear(); return; }

            if (rollItemMapping.ContainsKey(dropId))
            {
                rollItemMapping.Remove(dropId);

                if (rollReusultMapping.ContainsKey(dropId)) rollReusultMapping.Remove(dropId);

            }
            else
            {

                Debug.LogError("roll点中  没有该掉落id");

            }

        }

        public void SetRollResult(Variant data)
        {
            if (data.ContainsKey("roll_type")) {

                rollResultMapping[data["dpid"]._uint] = new Roll(data["dpid"]._uint, data["roll_type"]._uint, data["roll"]._uint);

            }else 
            {
                rollResultMapping[data["dpid"]._uint] = new Roll(data["dpid"]._uint , 0 , 0 );
            }

            if ( data.ContainsKey("roll_owner") && rollResultMapping.ContainsKey(data["dpid"]._uint)) {

                rollResultMapping[data["dpid"]._uint].SetRollOwner(data["roll_owner"]._uint, data["name"]._str, data["max_roll"]._uint);

            }

        }
    }

    public class ROllItem
    {
        public uint dpid = 0;
        public uint tp = 0;
        public uint left_tm = 0;
        public uint teamid = 0;
        public uint roll_tm = 0;
        public uint roll_owner = 0;
        public a3_BagItemData eqpData = null;
        public a3_BagItemData itemData = null;
        public bool isCanPick = false;
    }

    public class Roll {

        public uint dpid = 0;
        public uint roll_type = 0;   // 0  1  2 
        public uint roll = 0;

        public uint roll_owner = 0;
        public string name = string.Empty;
        public uint max_roll = 0;

        public Roll(uint dpid , uint roll_type , uint roll) {

            this.dpid = dpid;
            this.roll_type = roll_type;
            this.roll = roll;

        }

        public void SetRollOwner(uint roll_owner, string name, uint max_roll)
        {
            this.roll_owner = roll_owner;
            this.name = name;
            this.max_roll = max_roll;
        }

    }

}
