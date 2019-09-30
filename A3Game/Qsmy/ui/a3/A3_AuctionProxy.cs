using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_AuctionProxy : BaseProxy<A3_AuctionProxy>
    {
        public static uint EVENT_LOADALL = 60;                      //读所有
        public static uint EVENT_SELLSCUCCESS = 1;                  //出售完成
        public static uint EVENT_LOADMYSHELF = 0;                   //读自己的货架
        public static uint EVENT_PUTOFFSUCCESS = 2;                 //下架完成
        public static uint EVENT_BUYSUCCESS = 3;                    //购买完成
        public static uint EVENT_MYGET = 010;                       //领取列表
        public static uint EVENT_GETMYGET = 4;                      //领取一个完成
        public static uint EVENT_NEWGET = 99;                       //提示是否有新物品可以领取

        public A3_AuctionProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_AUCTION, AuctionOP);
        }

        //查询自己上架信息和领取信息
        public void SendMyRackMsg()
        {
            Variant msg = new Variant();
            msg["op"] = 0;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //上架
        public void SendPutOnMsg(uint id, uint puttm, uint yb,uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            msg["id"] = id;
            msg["puttm"] = puttm;
            msg["yb"] = yb;
            msg["num"] = num;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //下架
        public void SendPutOffMsg(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            msg["id"] = id;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //买道具
        public void SendBuyMsg(uint id, uint cid,uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["id"] = id;
            msg["cid"] = cid;
            msg["num"] = num;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //领取道具
        public void SendGetMsg(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 4;
            msg["id"] = id;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //一键领取道具
        public void SendGetAllMsg()
        {
            Variant msg = new Variant();
            msg["op"] = 5;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //条件查询道具,从第0页开始
        public void SendSearchMsg(uint row = 0, uint up_cost = 0, uint job = 0, uint equip_type = 0, uint stage = 0, uint quality = 0, string name = null,uint prefix_name=0)
        {
            Variant msg = new Variant();
            msg["op"] = 6;
            msg["row"] = row * 8;
            msg["up_cost"] = up_cost;
            if (job > 0) msg["job"] = job;
            if (equip_type > 0) msg["equip_type"] = equip_type-1;
            if (stage > 0) msg["stage"] = stage - 1;
            if (quality > 0) msg["quality"] = quality;
            if (name != null) msg["name"] = name;
            if (prefix_name > 0 && prefix_name < 6) msg["prefix_name"] = prefix_name;
            sendRPC(PKG_NAME.C2S_A3_AUCTION, msg);
        }

        //操作回复
        void AuctionOP(Variant data)
        {
            debug.Log("拍卖行信息:"+data.dump());
            int res = data["res"];
            if (res < 0) Globle.err_output(res);
            Variant vd = new Variant();
            uint id = 0;
            switch (res)
            {
                case 0:
                    A3_AuctionModel.getInstance().AddMyItem(data);
                    dispatchEvent(GameEvent.Create(EVENT_LOADMYSHELF, this, data));
                    dispatchEvent(GameEvent.Create(EVENT_MYGET, this, data));
                    break;
                case 1:
                    dispatchEvent(GameEvent.Create(EVENT_SELLSCUCCESS, this, data));
                    break;
                case 2:
                    A3_AuctionModel.getInstance().UpToDown(data);
                    dispatchEvent(GameEvent.Create(EVENT_PUTOFFSUCCESS, this, data));
                    break;
                case 3:
                    A3_AuctionModel.getInstance().AddMyItem(data);
                    dispatchEvent(GameEvent.Create(EVENT_BUYSUCCESS, this, data));
                    break;
                case 4:
                    id = data["auc_id"];
                    if (A3_AuctionModel.getInstance().GetMyItems_down().ContainsKey(id))
                    {
                        var ddd = A3_AuctionModel.getInstance().GetMyItems_down()[id];
                        if (ddd.auctiondata.get_type != 3)
                        {
                            flytxt.instance.fly(ContMgr.getCont("A3_AuctionProxy",new List<string> { A3_AuctionModel.getInstance().myitems_down[id].num.ToString(), A3_AuctionModel.getInstance().myitems_down[id].confdata.item_name }));
                            // flytxt.instance.fly("领取了" + A3_AuctionModel.getInstance().myitems_down[id].num+"个"+ A3_AuctionModel.getInstance().myitems_down[id].confdata.item_name);
                        }
                        else
                        {
                            flytxt.instance.fly(ContMgr.getCont("A3_AuctionProxy1",new List<string> { ((int)(A3_AuctionModel.getInstance().myitems_down[id].auctiondata.cost * 0.8f)).ToString() }));
                            //flytxt.instance.fly("领取了" + (int)(A3_AuctionModel.getInstance().myitems_down[id].auctiondata.cost * 0.8f) + "钻石");
                        }
                        A3_AuctionModel.getInstance().GetMyItems_down().Remove(id);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_GETMYGET, this, data));
                    break;
                case 6:
                    A3_AuctionModel.getInstance().Clear();
                    vd = data["auc_data"];
                    foreach (Variant v in vd._arr)
                    {
                        //uint cid = v["cid"];
                        //if (PlayerModel.getInstance().cid != cid)//不为自己时候
                        //{
                        //    A3_AuctionModel.getInstance().AddItem(v);
                        //}
                        A3_AuctionModel.getInstance().AddItem(v);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_LOADALL, this, data));
                    break;
            }
            Variant nd = new Variant();
            if (A3_AuctionModel.getInstance().GetMyItems_down().Count > 0)
            {
                nd["new"] = true;
            }
            else nd["new"] = false;
            dispatchEvent(GameEvent.Create(EVENT_NEWGET, this, nd));
        }
    }
}
