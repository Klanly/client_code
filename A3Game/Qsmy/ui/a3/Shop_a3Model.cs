using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;

namespace MuGame
{
    class Shop_a3Model:ModelBase<Shop_a3Model>
    {
        public Dictionary<int, shopDatas> itemsdic = new Dictionary<int, shopDatas>();//其他三个的
        public Dictionary<int, shopDatas> itemsdic_npcitem = new Dictionary<int, shopDatas>();//npc商店的
        public int selectnum = 0;
        public bool  toSelect = false;
        public int selectType = -1;
        //public Dictionary<int, iteminfoss> limiteddic = new Dictionary<int, iteminfoss>();//限时抢购的
        public Shop_a3Model()
        {
            Readxml();
        }
        void Readxml()
        {
            List<SXML> xml = XMLMgr.instance.GetSXMLList("golden_shop.golden_shop");
            foreach(SXML x in xml)
            {
                shopDatas infos = new shopDatas();
                infos.id = x.getInt("id");
                infos.type = x.getInt("type");
                infos.itemid = x.getInt("itemid");
                infos.money_type = x.getInt("money_type");
                infos.value = x.getInt("value");
                infos.itemName = x.getString("itemname");
                if (x.getInt("limit") != -1)
                {
                    infos.limiteD = x.getInt("limit");
                    infos.limiteNum = x.getInt("limit");
                }
                if (x.getInt("limit_w") != -1)
                {
                    infos.limiteD = x.getInt("limit_w");
                    infos.limiteNum = x.getInt("limit_w");
                }
                if ( x.getInt( "limit_f" ) != -1 )
                {
                    infos.limiteD = x.getInt( "limit_f" );
                    infos.limiteNum = x.getInt( "limit_f" );
                    infos.isover=true;
                }
                if (x.getInt("npc_id")!=-1)
                {
                    infos.npc_id = x.getInt("npc_id");
                }
                if(x.getInt("limit_day")!=-1)
               {
                    infos.day = x.getInt("limit_day");
                }
                itemsdic[infos.id] = infos;
                if (infos.npc_id != -1)
                    itemsdic_npcitem[infos.npc_id] = infos;
            }
        }
        public Dictionary<int, shopDatas> GetinfoByNPC_id(int npc_id)
        {
            Dictionary<int, shopDatas> dic = new Dictionary<int, shopDatas>();
            foreach(int i in itemsdic_npcitem.Keys)
            {
                if(npc_id==i)
                {
                    dic[i] = itemsdic_npcitem[i];
                }
            }
            return dic;
        }
        public void bundinggem(int id, int num , int left_num = -1 )
        {
            foreach (int ids in itemsdic.Keys)
            {
                if (id == ids)
                {
                    itemsdic[id].limiteD = itemsdic[id].limiteNum-num;

                    if ( left_num != -1 )
                    {
                        itemsdic[ id ].limiteD = itemsdic[ id ].limiteNum-left_num;
                    }

                    debug.Log("name:"+itemsdic[ids].itemName + "    num:" + itemsdic[id].limiteD);
                }
            } 
           
        }

        public shopDatas GetShopDataById(int itemid)
        {
            Dictionary<int,shopDatas>.Enumerator enumerator = itemsdic.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Current.Value.itemid == itemid)
                    return enumerator.Current.Value;
            return null;
        }


        //public Dictionary<int, iteminfoss> limited(int id, int num)
        //{

        //    foreach (int ids in limiteddic.Keys)
        //    {
        //        if (id == ids)
        //        {
        //            limiteddic[id].limiteD = num;
        //        }
        //    } 
        //    return limiteddic;
        //}


    }
    class shopDatas
    {
        public int id;
        public int type;
        public int itemid;
        public int money_type;
        public string itemName;
        public int value;
        public int limiteD;//剩余或限购数量
        public int limiteNum;//限购的总数
        public int discount;//折扣    
        public int npc_id=-1;
        public bool isover=false;
        public int day = -1;

    }

}