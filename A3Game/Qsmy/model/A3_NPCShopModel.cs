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
    class A3_NPCShopModel : ModelBase<A3_NPCShopModel>
    {
        public A3_NPCShopModel()
        {
            local_dicNpcShop = new Dictionary<int, NpcShopData>();
            ReadXML();
        }
        public int alltimes;
        public Dictionary<uint, uint> float_list = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> float_list_last = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> float_list_num = new Dictionary<uint, uint>();//某商品剩余数量
        public Dictionary<uint, uint> all_float = new Dictionary<uint, uint>();//所有的float的id和价格
        public Dictionary<uint, uint> limit_num = new Dictionary<uint, uint>();//limit_num
        //public int npc_id;
        public List<SXML> listNPCShop = new List<SXML>();
        public Dictionary<int /*npc id*/, NpcShopData> local_dicNpcShop;//本地配置
        public Dictionary<int, NpcShopData> price = new Dictionary<int, NpcShopData>();
        private void ReadXML()
        {
            SXML sxmlFloat = XMLMgr.instance.GetSXML("npc_shop");
            List<SXML> listNpcShop = XMLMgr.instance.GetSXML("npc_shop").GetNodeList("npc_shop");
            if (listNpcShop != null)
            {
                for (int i = 0; i < listNpcShop.Count; i++)
                {
                    NpcShopData data = new NpcShopData();
                    data.shop_id = listNpcShop[i].getUint("shop_id");
                    data.npc_id = listNpcShop[i].getInt("npc_id");
                    data.shop_name = listNpcShop[i].getString("shop_name");
                    string[] floatList = listNpcShop[i].getString("float_list").Split(',');
                    string[] goodsList = listNpcShop[i].getString("goods_list").Split(',');
                    data.dicFloatList = new Dictionary<uint, uint>();
                    data.dicGoodsList = new Dictionary<uint, uint>();
                    uint id;
                    for (int j = 0; j < floatList.Length; j++)
                        if (uint.TryParse(floatList[j], out id))
                            data.dicFloatList.Add(id, sxmlFloat.GetNode("float_list", "id==" + id).getUint("item_id"));
                    for (int j = 0; j < goodsList.Length; j++)
                        if (uint.TryParse(goodsList[j], out id))
                            data.dicGoodsList.Add(id, sxmlFloat.GetNode("goods_list", "id==" + id).getUint("item_id"));
                    data.mapId = XMLMgr.instance.GetSXML("npcs").GetNode("npc", "id==" + data.npc_id).getInt("map_id");
                    local_dicNpcShop.Add(data.npc_id, data);
                }
            }
        }

        public NpcShopData GetDataByItemId(uint itemId)
        {

            //List<int> idx = local_dicNpcShop.Keys.ToList();
            //for (int i = 0; i < idx.Count; i++)
            //{
            //    for (int j = 0; j < local_dicNpcShop[idx[i]].dicGoodsList.Count; j++)
            //    {
            //        if (local_dicNpcShop[idx[i]].dicGoodsList.ContainsValue(itemId))
            //            return local_dicNpcShop[idx[i]];
            //    }
            //}
            // 现在改了，去读Shop_a3Model.getinstance().itemsdic_npcitem字典里面找（记得有两个id，自己看读哪一个）;
            return null;
        }
    }
    class NpcShopData
    {
        public uint shop_id;
        public int npc_id;
        public string shop_name;
        public Dictionary<uint, uint> dicFloatList;
        public Dictionary<uint, uint> dicGoodsList;
        public int mapId;
        public int lastprice;
        public int nowprice;
    }
}

