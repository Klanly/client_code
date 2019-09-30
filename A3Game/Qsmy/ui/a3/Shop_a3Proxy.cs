using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine.UI;

namespace MuGame
{
    class Shop_a3Proxy : BaseProxy<Shop_a3Proxy>
    {
        public static uint LIMITED = 0;
        public static uint CHANGELIMITED = 1;
        public static uint DELETELIMITED = 2;
        public static uint DONATECHANGE = 3;
        public Shop_a3Proxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_SHOP, onLoadShop);
        }
        public void sendinfo(int count, int id = -1, int num = 0, int activityId = -1)
        {
            Variant msg = new Variant();
            switch (count)
            {
                case 0:
                    msg["op"] = 0;
                    break;
                case 1:
                    msg["op"] = 1;
                    break;
                case 2:
                    msg["op"] = 2;
                    msg["id"] = id;
                    msg["item_num"] = num;
                    break;
                case 3:
                    msg["op"] = 3;
                    msg["id"] = id;
                    msg["item_num"] = num;
                    msg["shop_id"] = activityId;
                    break;
                case 5:
                    msg["op"] = 2;
                    msg["id"] = id;
                    msg["item_num"] = num;
                    break;
                case 6:
                    msg["op"] = 5;
                    msg["id"] = id;
                    msg["item_num"] = num;
                    break;
                case 7:
                    msg[ "op" ] = 2;
                    msg[ "id" ] = id;
                    msg[ "item_num" ] = num;
                    break;
            }
            sendRPC(PKG_NAME.C2S_A3_SHOP, msg);
            debug.Log(msg.dump());
        }

        //!--商店物品购买(金币购买)
        public void BuyStoreItems(uint tpid, uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 2;//原为4,4为特卖价格 
            shopDatas shopData = Shop_a3Model.getInstance().GetShopDataById((int)tpid);
            msg["id"] = shopData.id;
            msg["item_num"] = num;
            sendRPC(PKG_NAME.C2S_A3_SHOP, msg);

            //提示购买药水
            flytxt.instance.fly(ContMgr.getCont("shop_a3_buyself") + num + ContMgr.getCont("employer1") + shopData.itemName);//a3_BagModel.getInstance().getItemDataById(tpid).item_name);

        }

        public void onLoadShop(Variant data)
        {
            debug.Log("商城信息：" + data.dump());
            int res = data["res"];
            if (res == 0)
            {
                if (data["confs"].Length > 0)
                {
                    foreach (Variant v in data["confs"]._arr)
                    {
                        Shop_a3Model.getInstance().bundinggem(v["id"], v["item_num"]);
                    }
                }
                if (shop_a3.instance && shop_a3.instance.isbangding)
                    shop_a3.instance.tab3();
            }
            else if (res == 1)
            {
                debug.Log("收到的a3显示抢购刷新信息：" + data.dump());

                dispatchEvent(GameEvent.Create(LIMITED, this, data));

            }
            else if (res == 2)
            {
                debug.Log("收到的3购买信息：" + data.dump());
                if (a3_npc_shop.instance != null && a3_npc_shop.instance.isnpcshop)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_npcshop1"));
                }
                else
                {
                    if ( data[ "left_num" ] != null)
                    {
                        Shop_a3Model.getInstance().bundinggem( data[ "id" ] , data[ "item_num" ] , data[ "left_num" ] );
                    }
                    else {

                        Shop_a3Model.getInstance().bundinggem( data[ "id" ] , data[ "item_num" ] );

                    }

                    if (data.ContainsKey("donate"))
                        dispatchEvent(GameEvent.Create(DONATECHANGE, this, data));
                    if (shop_a3._instance != null)
                        shop_a3._instance.Refresh(data["id"], data["item_num"]);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_DYETIP);
                }



            }
            else if (res == 3)
            {
                debug.Log("收到的a3限时特卖购买信息：" + data.dump());
                shop_a3._instance.Refresh_limited(data["id"], data["shop_id"], data["left_num"]);

            }
            else if (res == 4)
            {
                debug.Log("收到的a3新增或变更限时特卖购买活动:" + data.dump());
                dispatchEvent(GameEvent.Create(CHANGELIMITED, this, data));
            }
            else if (res == 5)
            {
                debug.Log("收到的a3限时特卖更改活动信息:" + data.dump());
                dispatchEvent(GameEvent.Create(DELETELIMITED, this, data));
            }
            else if (res < 0)
            {
                Globle.err_output(data["res"]);
                return;
            }
        }
    }


}
