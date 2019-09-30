using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Cross;

namespace MuGame
{
    class A3_AuctionModel : ModelBase<A3_AuctionModel>
    {
        Dictionary<uint, a3_BagItemData> items = new Dictionary<uint, a3_BagItemData>();
        Dictionary<uint, a3_BagItemData> myitems_up = new Dictionary<uint, a3_BagItemData>();
        public Dictionary<uint, a3_BagItemData> myitems_down = new Dictionary<uint, a3_BagItemData>();

        public void AddMyItem(Variant data)
        {
            myitems_up.Clear();
            myitems_down.Clear();
            if (data.ContainsKey("auc_data"))
            {
                Variant vd = data["auc_data"];
                foreach (Variant v in vd._arr)
                {
                    var item = ReadItem(v);
                    RemoveItem(item.id);
                    if (myitems_up.ContainsKey(item.id))
                    {
                        myitems_up[item.id] = item;
                    }
                    else
                        myitems_up.Add(item.id,item);
                }
            }
            if (data.ContainsKey("get_list"))
            {
                Variant gl = data["get_list"];
                foreach (Variant v in gl._arr)
                {
                    var item = ReadItem(v);
                    RemoveItem(item.id);
                    myitems_down[item.id] = item;
                }
            }
        }

        public void UpToDown(Variant data)
        {
            if (data.ContainsKey("get_list"))
            {
                Variant list = data["get_list"];
                foreach (var v in list._arr)
                {
                    uint id = v["id"];
                    if (myitems_up.ContainsKey(id)) myitems_up.Remove(id);
                    var item = ReadItem(v);
                    myitems_down[item.id] = item;
                }
            }
        }

        public void AddItem(Variant data)
        {
            var item = ReadItem(data);
            items[item.id] = item;
        }

        a3_BagItemData ReadItem(Variant data)
        {

            uint id = data["id"];
            uint tpid = data["tpid"];
            a3_BagItemData item = new a3_BagItemData();
            item.id = id;
            item.tpid = tpid;
            if (data.ContainsKey("num"))
                item.num = data["num"];
            item.confdata = a3_BagModel.getInstance().getItemDataById(tpid);
            Variant eqpdata = data["itm"];
            a3_EquipModel.getInstance().equipData_read(item, eqpdata);
            item.auctiondata.cid = data["cid"];
            item.auctiondata.tm = data["tm"];
            item.auctiondata.pro_tm = data["puttm_type"];
            item.auctiondata.cost = data["cost"];
            if (data.ContainsKey("get_type"))
            {
                item.auctiondata.get_type = data["get_type"];
            }
            if (data.ContainsKey("get_tm"))
            {
                item.auctiondata.get_tm = data["get_tm"];
            }
            if (data.ContainsKey("seller_name"))
            {
                item.auctiondata.seller = data["seller_name"];
            }
            return item;
        }
        public a3_BagItemData GetDataById(uint id)
        {
            return items[id];
        }
        public void RemoveItem(uint id)
        {
            if (items.ContainsKey(id))
            {
                items.Remove(id);
            }
        }

        public void UpdateItem(Variant data)
        {
            uint id = data["id"];
            if (items.ContainsKey(id))
            {
                items.Remove(id);
            }
            AddItem(data);
        }

        public void Clear()
        {
            items.Clear();
        }

        public Dictionary<uint, a3_BagItemData> GetItems()
        {
            return items;
        }
        public Dictionary<uint, a3_BagItemData> GetMyItems_up()
        {
            return myitems_up;
        }
        public Dictionary<uint, a3_BagItemData> GetMyItems_down()
        {
            return myitems_down;
        }

        public string FromGetTypeToString(int get_type)
        {

            if (get_type == 1)
            {
                //return "物品下架";
                return ContMgr.getCont("A3_AuctionModel1");
            }
            else if (get_type == 2)
            {
                return ContMgr.getCont("A3_AuctionModel2");
                //return "物品购买";
            }
            else if (get_type == 3)
            {
                return ContMgr.getCont("A3_AuctionModel3");
                //return "物品卖出";
            }
            else
            {
                return ContMgr.getCont("A3_AuctionModel4");
                //return "特殊物品";
            }
        }
    }
}
