//using System.Text;
//using GameFramework;
//using Cross;
//using System.Collections.Generic;
//namespace MuGame
//{
//    class A3_NPCShopProxy : BaseProxy<A3_NPCShopProxy>
//    {

//        public static uint EVENT_NPCSHOP_BUY = 0;
//        public static uint EVENT_NPCSHOP_REFRESH = 1;
//        public static uint EVENT_NPCSHOP_TIME = 2;

//        public A3_NPCShopProxy()
//        {
//            addProxyListener(PKG_NAME.C2S_CARRCHIEF, onNPCShop);
//        }

//        //===========c2s=========

//        public void sendShowFloat(uint npcid)//获取NPC对应的浮动商品信息
//        {
//            Variant data = new Variant();
//            data["op"] = 1;
//            data["index"] = npcid;
//            sendRPC(PKG_NAME.C2S_CARRCHIEF, data);
//            debug.Log(data.dump());
//        }
//        public void sendBuy(uint npcid, uint shop_idx, uint shop_type, uint shop_num)/*购买NPC商城道具*/
//        {
//            Variant data = new Variant();
//            data["op"] = 2;
//            data["index"] = npcid;
//            data["shop_idx"] = shop_idx;
//            data["shop_type"] = shop_type; //浮动商品: 1  固定商品: 0
//            data["shop_num"] = shop_num;
//            sendRPC(PKG_NAME.C2S_CARRCHIEF, data);
//            debug.Log(data.dump());
//        }
//        public void sendShowAll()//所有道具价格
//        {
//            Variant data = new Variant();
//            data["op"] = 3;
//            sendRPC(PKG_NAME.C2S_CARRCHIEF, data);
//        }



//        //===========s2c=========
//        public void onNPCShop(Variant data)
//        {
//            debug.Log("NPCSHOP============" + data.dump());
//            int res = data["res"];
//            if (res < 0)
//            {
//                Globle.err_output(res);
//                //全服数量不足
//                //if (res==-5100)
//                //{
//                //    sendShowFloat((uint)A3_NPCShopModel.getInstance().listNPCShop[0].getInt("shop_id"));
//                //}
//                return;
//            }
//            switch (res)
//            {
//                case 1: onFloat(data); break;
//                case 2: onBuy(data); break;
//                case 3: onRefresh(data); break;
//                default:
//                    break;
//            }
//        }

//        void onFloat(Variant data)
//        {
//            A3_NPCShopModel.getInstance().alltimes = data["next_tm"];
//            A3_NPCShopModel.getInstance().price.Clear();
//            if (data.ContainsKey("float_list"))
//            {
//                Variant flist = data["float_list"];
//                A3_NPCShopModel.getInstance().float_list.Clear();
//                A3_NPCShopModel.getInstance().float_list_last.Clear();
//                A3_NPCShopModel.getInstance().float_list_num.Clear();
//                A3_NPCShopModel.getInstance().limit_num.Clear();
//                foreach (var item in flist._arr)
//                {
//                    if (!A3_NPCShopModel.getInstance().float_list.ContainsKey(item["item_id"]))
//                    {
//                        NpcShopData shopnc = new NpcShopData();
//                        shopnc.shop_id = item["shop_idx"]._uint;
//                        if (!A3_NPCShopModel.getInstance().float_list.ContainsKey(item["item_id"]))
//                        {
//                            A3_NPCShopModel.getInstance().float_list.Add(item["item_id"], item["cost"]);
//                            shopnc.nowprice=item["cost"]._int;
//                        }
//                        if (!A3_NPCShopModel.getInstance().float_list_last.ContainsKey(item["item_id"]))
//                        {
//                            A3_NPCShopModel.getInstance().float_list_last.Add(item["item_id"], item["last_cost"]);
//                            shopnc.lastprice = item["last_cost"]._int;
//                        }
//                        if (!A3_NPCShopModel.getInstance().float_list_num.ContainsKey(item["item_id"]))
//                        {
//                            A3_NPCShopModel.getInstance().float_list_num.Add(item["item_id"], item["self_limit"]);
//                        }
//                        A3_NPCShopModel.getInstance().price.Add((int)shopnc.shop_id,shopnc);
//                        A3_NPCShopModel.getInstance().limit_num.Add(item["item_id"], item["limit_num"]);
//                    }
//                }
//                dispatchEvent(GameEvent.Create(EVENT_NPCSHOP_TIME, this, data));
//            }

//        }
//        void onBuy(Variant data)
//        {
//            dispatchEvent(GameEvent.Create(EVENT_NPCSHOP_BUY, this, data));
//        }

//        void onRefresh(Variant data)
//        {
//            if (data["float_list"] != null)
//            {
//                Variant flist = data["float_list"];
//                //A3_NPCShopModel.getInstance().all_float.Clear();
//                foreach (var item in flist._arr)
//                {
//                    if (!A3_NPCShopModel.getInstance().all_float.ContainsKey(item["item_id"]))
//                        A3_NPCShopModel.getInstance().all_float.Add(item["item_id"], item["cost"]);
//                    else
//                        A3_NPCShopModel.getInstance().all_float[item["item_id"]] = item["cost"];
//                }
//            }
//            dispatchEvent(GameEvent.Create(EVENT_NPCSHOP_REFRESH, this, null));
//        }


//    }
//}
