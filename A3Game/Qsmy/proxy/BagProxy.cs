using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;
namespace MuGame
{
    class BagProxy : BaseProxy<BagProxy>
    {
        public static uint EVENT_USE_ADD_HEROEXP = 0;
        public static uint EVENT_USE_GETGIFT = 1;
        public static uint EVENT_LOAD_BAG = 2;
        public static uint EVENT_ITME_SELL = 3;
        public static uint EVENT_ITEM_BUY = 4;
        public static uint EVENT_ITEM_CHANGE = 5;
        public static uint EVENT_OPEN_BAGLOCK = 6;
        public static uint EVENT_OPEN_HOUSELOCK = 7;
        public static uint EVENT_ROOM_TURN = 8;
        public static uint EVENT_USE_DYE = 9;
        public static uint EVENT_GET_SUM = 10;
        public BagProxy()
        {
            addProxyListener(PKG_NAME.S2C_GET_ITEMS_RES, onLoadItems);
            //addProxyListener(PKG_NAME.S2C_BUY_ITEM_RES, onBuyItems);
            addProxyListener(PKG_NAME.S2C_SELL_ITEM_RES, onSellItems);
            addProxyListener(PKG_NAME.S2C_USE_UITEM_RES, onUseItems);
            addProxyListener(PKG_NAME.S2C_ITEM_CHANGE, onItemChange);
            addProxyListener(PKG_NAME.S2C_BAGITEM_CDTIME, onItemCd);
        }
        void onItemChange(Variant data)
        {           
            debug.Log(data.dump() + ":::::::::::::::");
          //  Debug.LogError(a3_BagModel.getInstance().item_num[1540].num);
            if (data.ContainsKey("money") || data.ContainsKey("yb") || data.ContainsKey("bndyb"))
            {//金钱变化
                if (data.ContainsKey("money"))
                {
                    if (data["money"] > PlayerModel.getInstance().money )
                    {
                        //if(a3_insideui_fb.instance == null)
                        flytxt.instance.fly(ContMgr.getCont("BagProxy_money") + (data["money"] - PlayerModel.getInstance().money));

                        if (a3_insideui_fb.instance != null &&data.Count==1)
                        {
                            a3_insideui_fb.instance.SetInfMoney((int)(data["money"] - PlayerModel.getInstance().money));
                            //BaseRoomItem.instance.goldnum = (data["money"] - PlayerModel.getInstance().money);
                            //debug.Log("ssssssssssssssssssssssssddd+" + BaseRoomItem.instance.goldnum);
                        }
                    }
                    else
                    {
                        debug.Log("消耗金币" + (PlayerModel.getInstance().money - data["money"]));
                        skill_a3.upgold = (int)(PlayerModel.getInstance().money - data["money"]);
                    }
                    PlayerModel.getInstance().money = data["money"];
                }
                if (data.ContainsKey("yb"))//钻石
                {
                    uint lastgold = PlayerModel.getInstance().gold;
                    PlayerModel.getInstance().gold = data["yb"];
                    if (lastgold < data["yb"] && HttpAppMgr.instance != null && HttpAppMgr.instance.giftCard != null)
                    {
                        HttpAppMgr.instance.giftCard.getFirstRechangeCard();
                        HttpAppMgr.instance.giftCard.getRechangeCard();
                    }
                }
                if (data.ContainsKey("bndyb"))
                {
                    PlayerModel.getInstance().gift = data["bndyb"];
                }
                UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_MONEY_CHANGE, this, data));
            }
            bool new_itm = true;
            if (data.ContainsKey("new_itm"))
            {
                new_itm = data["new_itm"];
            }
            if (data.ContainsKey("add"))
            {
                //a3_BagModel.getInstance().removetype();
                int m=0;
                Variant info = data["add"];
                foreach (Variant item in info._arr)
                {
                    
                    a3_BagItemData itemData = new a3_BagItemData();
                    itemData.id = item["id"];
                    itemData.tpid = item["tpid"];
                    itemData.num = item["cnt"];
                    itemData.bnd = item["bnd"];

                    if (a3_BagModel.getInstance().Items.ContainsKey(item["id"]))
                        m = a3_BagModel.getInstance().Items[item["id"]].num;
                    if (item.ContainsKey("mark")) itemData.ismark = item["mark"];
                    itemData.isEquip = false;
                    if (new_itm)
                        itemData.isNew = true;
                    else
                        itemData.isNew = false;
                    if(item.ContainsKey("stone_att"))
                    {
                        itemData.isrunestone = true;
                        foreach (Variant i in item["stone_att"]._arr)
                        {
                            itemData.runestonedata.runeston_att = new Dictionary<int, int>();
                            int att_type = i["att_type"];
                            int att_value = i["att_value"];
                            itemData.runestonedata.runeston_att[att_type] = att_value;
                        }
                    }
                    else
                        itemData.isrunestone = false;
                    if (item.ContainsKey("intensify_lv"))
                    {
                        a3_EquipModel.getInstance().equipData_read(itemData, item);
                    }
                    if (item.ContainsKey ("talent")) {
                        itemData.isSummon = true;
                        itemData = A3_SummonModel.getInstance().GetSummonData(itemData, item);
                    }
                    a3_BagModel.getInstance().addItem(itemData);

                    if (off_line_exp.instance != null)
                    {
                       
                        if (off_line_exp.instance.offline == true)
                        {
                           
                            off_line_exp.instance.offline_item.Add(itemData);
                            if (off_line_exp.instance.fenjie.isOn == false)
                                flytxt.instance.fly(ContMgr.getCont("BagProxy_getequip") + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name"));
                            else
                            {
                                flytxt.instance.fly(ContMgr.getCont("BagProxy_geteitem") + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name") + "x" + (item["cnt"]-m));
                            }
                        }
                    }
                    if (A3_SummonProxy.getInstance().getNewSum) {
                        A3_SummonProxy.getInstance().getNewSum = false;
                        //dispatchEvent(GameEvent.Create(EVENT_GET_SUM, this, item));
                        if (a3_summon_tujian.instans != null) {
                            a3_summon_tujian.instans.ongetsum(itemData);
                        }

                    }

                    //添加魔晶
                    //if (itemData.tpid == 1540)
                    //{
                    //    if (a3_expbar.instance != null) a3_expbar.instance.CheckNewSkill();
                    //}
                }
                //if (off_line_exp.instance?.offline == true)
                //{
                //    if (off_line_exp.instance.offline_item != null)
                //    {
                //        foreach (var v in off_line_exp.instance.offline_item)
                //        {
                //            a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)v.tpid);
                //            GameObject go = IconImageMgr.getInstance().createA3ItemIconTip(itemid: (uint)v.tpid, num: v.num);
                //            flytxt.instance.fly(null, 6, showIcon: go);
                //        }
                //    }
                //    off_line_exp.instance.offline = false;
                //    off_line_exp.instance.offline_item.Clear();
                //}
              
                if (a3_role.instan != null)
                    dispatchEvent(GameEvent.Create(EVENT_USE_DYE, this, data));
            }
            if (data.ContainsKey("modcnts"))
            {
                int m = 0;
                Variant info = data["modcnts"];
                foreach (Variant item in info._arr)
                {
                  
                    a3_BagItemData itemData = new a3_BagItemData();
                    itemData.id = item["id"];
                    itemData.tpid = item["tpid"];
                    itemData.num = item["cnt"];
                    //itemData.bnd = item["bnd"];
                    itemData.isEquip = false;
                    itemData.isNew = false;
                    if (a3_BagModel.getInstance().Items.ContainsKey(item["id"]))
                    {
                        m = a3_BagModel.getInstance().Items[item["id"]].num;
                    }
                    // Debug.LogError(a3_BagModel.getInstance().Items[item["id"]].num + "sss" + a3_BagModel.getInstance().getItemNumByTpid(1540)+"ss"+ a3_BagModel.getInstance().getItems()[item["id"]].num);


                    if (item.ContainsKey("intensify_lv"))
                    {
                        a3_EquipModel.getInstance().equipData_read(itemData, item);
                    }
                    int n = 0;
                    if (a3_BagModel.getInstance().getItems().ContainsKey(itemData.id))
                    {
                        n = a3_BagModel.getInstance().getItems()[itemData.id].num;
                    }
                    a3_BagModel.getInstance().addItem(itemData);
                    if (item["tpid"]==1540)
                    {
                        skill_a3.upmojing = a3_BagModel.getInstance().getItemNumByTpid(1540);//魔晶剩余总量
                       
                    }
                  
                    if (off_line_exp.instance != null)
                    {
                     
                        if (off_line_exp.instance.offline == true)
                        {
                            
                            off_line_exp.instance.offline_item.Add(itemData);
                           
                            flytxt.instance.fly(ContMgr.getCont("BagProxy_geteitem") + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name") + "x" + (item["cnt"]-m));
                           

                        }
                    }
                   
                    //修改魔晶
                    //if (itemData.tpid == 1540 && n < itemData.num)
                    //{
                    //    if (a3_expbar.instance != null) a3_expbar.instance.CheckNewSkill();
                    //}
                }
               
            }
            if (data.ContainsKey("rmvids"))
            {
                //a3_BagModel.getInstance().addtype();
                Variant info = data["rmvids"];
                foreach (uint id in info._arr)
                {
                    a3_BagModel.getInstance().removeItem(id);
                }
            }

            if (data.ContainsKey("rmvids") || data.ContainsKey("add") || data.ContainsKey("modcnts"))
            {
                dispatchEvent(GameEvent.Create(EVENT_ITEM_CHANGE, this, data));
                //if (a3_bag.indtans) { a3_bag.indtans.onItemChange(data); }
                if (isRanse)
                { 
                    dispatchEvent(GameEvent.Create(EVENT_USE_DYE, this, data));
                    isRanse = false;
                }
            }

            if (data.ContainsKey("nobpt"))
            {//声望点
                if ((data["nobpt"] > PlayerModel.getInstance().nobpt)) {
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_nobpt") + (data["nobpt"] - PlayerModel.getInstance().nobpt));
                }
                PlayerModel.getInstance().nobpt = data["nobpt"];
            }
            if (data.ContainsKey("energy"))
            {//体力
                MapModel.getInstance().energy = data["energy"];
                //if (fb_3d.instance != null)
                //    fb_3d.instance.refreshEnergy();

                //if (fb_energy.instance != null)
                //    fb_energy.instance.refresh();

            }
          


        }

        public void onItemCd(Variant data)
        {
            if (data.ContainsKey("itemcds"))
            {
                foreach (Variant item in data["itemcd"]._arr)
                {
                    int cdtype = item["cdtp"];
                    float cdtm = item["cdtm"];
                }
            }
            if (data.ContainsKey("cd_type"))
            {
                int cdtype = data["cd_type"];
                float cdtm = data["cd"];
                if (cdtype == 4)
                {
                    MediaClient.instance.PlaySoundUrl("audio_common_use_hp", false, null);
                }
                a3_BagModel.getInstance().addItemCd(cdtype, cdtm);
            }
        }

        public void sendLoadItems(int val)
        {
            Variant msg = new Variant();
            msg["option"] = val;
            sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, msg);
        }

        public void sendMark(uint id)
        {
            Variant msg = new Variant();
            msg["option"] = 6;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, msg);
        }
        public void sendOpenLock(int type, int num, bool use_yb)
        {
            Variant msg = new Variant();
            msg["option"] = type;
            msg["unlock_num"] = num;
            msg["use_yb"] = use_yb;
            sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, msg);
        }
        public void sendRoomItems(bool pack_to_cangku, uint id, int num)
        {
            Variant msg = new Variant();
            msg["option"] = 4;
            msg["pack_to_cangku"] = pack_to_cangku;
            msg["item_id"] = id;
            msg["item_num"] = num;
            sendRPC(PKG_NAME.S2C_GET_ITEMS_RES, msg);
        }
        public void sendBuyItems(int tp, int id = 0, int num = 0)
        {
            Variant msg = new Variant();
            if (tp == 2)
            {
                msg["tp"] = 2;
                msg["id"] = id;
                msg["cnt"] = num;
            }
            if (tp == 1)
            {
                msg["tp"] = 1;
            }
            sendRPC(PKG_NAME.S2C_BUY_ITEM_RES, msg);
        }
        public void sendSellItems(uint id, int num)
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            msg["id"] = id;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_SELL_ITEM_RES, msg);
        }

        public void sendSellItems(List<Variant> id)
        {
            Variant msg = new Variant();
            msg["sell_items"] = new Variant();
            msg["op"] = 2;
            for (int i = 0; i < id.Count; i++)
            {
                msg["sell_items"].pushBack(id[i]);
            }
            sendRPC(PKG_NAME.S2C_SELL_ITEM_RES, msg);
        }


        public void sendUseItems(uint id, int num , string name = null)
        {
            Variant msg = new Variant();
            msg["id"] = id;
            msg["num"] = num;
            if (name != null)
            {
                msg["name"] = name;
            }
            sendRPC(PKG_NAME.S2C_USE_UITEM_RES, msg);
        }
        public void sendUseHeroExp(string itemid, int heroid)
        {
            Variant msg = new Variant();
            msg["tpid"] = itemid;
            msg["heroexp_hero_id"] = heroid;
            sendRPC(PKG_NAME.S2C_USE_UITEM_RES, msg);
        }

        public void onSellItems(Variant data)
        {
            int res = data["res"];
            if (res < 0)
            {
                // Globle.err_output(res);
                return;
            }
            else if (res == 1)
            {
                uint id = data["id"];
                uint earn = data["earn"];
                MediaClient.instance.PlaySoundUrl("audio_common_sold_coin", false, null);
            }
            else if (res == 2)
            {
                uint earn = data["earn"];
                MediaClient.instance.PlaySoundUrl("audio_common_sold_coin", false, null);
                if (piliang_chushou.instance != null)
                    piliang_chushou.instance.refresh_Sell();
                if (InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_SMITHY) && A3_Smithy.Instance != null)
                    A3_Smithy.Instance.refresh_Sell();
            }
            a3_BagModel.getInstance()?.SellItem();
            dispatchEvent(GameEvent.Create(EVENT_ITME_SELL, this, data));
        }
        public void onBuyItems(Variant data)
        {
            //if (data.ContainsKey("gift_shop"))
            //{//请求礼金商城数据
            //    UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_LOAD_GIFT_SHOP, this, data));
            //}
            //else
            //{
            //    int res = data["res"];
            //    if (res < 0)
            //    {
            //        Globle.err_output(res);
            //        return;
            //    }
            //    flytxt.instance.fly("购买成功！");
            //    Variant info = data["itms"];
            //    foreach (Variant item in info._arr)
            //    {
            //        BagItemData itemData = new BagItemData();
            //        itemData.id = item["tpid"];
            //        itemData.num = item["cnt"];
            //        BagModel.getInstance().addItem(itemData);
            //    }
            //    UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_BUY_ITEMS, this, data));
            //}
        }
        public void onLoadItems(Variant data)
        {
            debug.Log("包裹On_C#" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                case 0:
                    //a3_BagModel.getInstance().item_num.Clear();
                    //a3_BagModel.getInstance().getItems().Clear();
                    a3_BagModel.getInstance().curi = data["curi"];
                    Variant info = data["items"];
                    foreach (Variant item in info._arr)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        itemData.id = item["id"];
                        itemData.tpid = item["tpid"];
                        itemData.num = item["cnt"];
                        itemData.bnd = item["bnd"];
                        itemData.ismark = item["mark"];
                        itemData.isEquip = false;
                        itemData.isNew = false;
                        if (item.ContainsKey("intensify_lv"))
                        {
                            a3_EquipModel.getInstance().equipData_read(itemData, item);
                        }
                        if (item.ContainsKey("stone_att"))
                        {                          
                            itemData.isrunestone = true;
                            foreach (Variant i in item["stone_att"]._arr)
                            {
                                itemData.runestonedata.runeston_att = new Dictionary<int, int>();
                                int att_type = i["att_type"];
                                int att_value = i["att_value"];
                                itemData.runestonedata.runeston_att[att_type] = att_value;
                            }

                        }
                        else
                            itemData.isrunestone = false;

                        if(item.ContainsKey("talent"))
                            itemData = A3_SummonModel.getInstance().GetSummonData(itemData, item);
                        a3_BagModel.getInstance().addItem(itemData);
                        //if (a3_BagModel.getInstance().item_num.ContainsKey(itemData.tpid))
                        //{
                        //    itemData.num = a3_BagModel.getInstance().item_num[itemData.tpid].num + itemData.num;
                        //    a3_BagModel.getInstance().item_num.Remove(itemData.tpid);

                        //    a3_BagModel.getInstance().item_num.Add(itemData.tpid, itemData);
                        //}
                        //else
                        //{
                        //    a3_BagModel.getInstance().item_num.Add(itemData.tpid, itemData);
                        //}
                        if (a3_legion_info.mInstance != null)
                        {
                            a3_legion_info.mInstance.buff_up();
                        }
                    }
                    dispatchEvent(GameEvent.Create(EVENT_LOAD_BAG, this, null));
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.bag_Count", "ui/interfaces/low/a1_low_fightgame", a3_BagModel.getInstance().curi, a3_BagModel.getInstance().getItems().Count);
                    break;
                case 1:
                    a3_BagModel.getInstance().getHouseItems().Clear();
                    a3_BagModel.getInstance().house_curi = data["curi"];
                    Variant info1 = data["items"];
                    foreach (Variant item in info1._arr)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        itemData.id = item["id"];
                        itemData.tpid = item["tpid"];
                        itemData.num = item["cnt"];
                        itemData.bnd = item["bnd"];
                        itemData.ismark = item["mark"];
                        itemData.isEquip = false;
                        if (item.ContainsKey("intensify_lv"))
                        {
                            a3_EquipModel.getInstance().equipData_read(itemData, item);
                        }
                        a3_BagModel.getInstance().addHouseItem(itemData);
                    }
                   
                    //InterfaceMgr.getInstance().openUiFirstTime();
                    break;
                case 2:
                    a3_BagModel.getInstance().curi = data["unlock_num"];
                    dispatchEvent(GameEvent.Create(EVENT_OPEN_BAGLOCK, this, null));
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.bag_Count", "ui/interfaces/low/a1_low_fightgame", a3_BagModel.getInstance().curi, a3_BagModel.getInstance().getItems().Count);
                    if (a3_expbar.instance != null)
                    {
                        a3_expbar.instance.bag_Count();
                    }
                    break;
                case 3:
                    a3_BagModel.getInstance().house_curi = data["unlock_num"];
                    dispatchEvent(GameEvent.Create(EVENT_OPEN_HOUSELOCK, this, null));
                    break;
                case 4:
                    if (data.ContainsKey("add"))
                    {
                        Variant info2 = data["add"];
                        foreach (Variant item in info2._arr)
                        {
                            a3_BagItemData itemData = new a3_BagItemData();
                            itemData.id = item["id"];
                            itemData.tpid = item["tpid"];
                            itemData.num = item["cnt"];
                            itemData.bnd = item["bnd"];
                            itemData.isEquip = false;
                            if (item.ContainsKey("intensify_lv"))
                            {
                                a3_EquipModel.getInstance().equipData_read(itemData, item);
                            }
                            a3_BagModel.getInstance().addHouseItem(itemData);
                        }
                    }
                    if (data.ContainsKey("modcnts"))
                    {
                        Variant info3 = data["modcnts"];
                        foreach (Variant item in info3._arr)
                        {
                            a3_BagItemData itemData = new a3_BagItemData();
                            itemData.id = item["id"];
                            itemData.tpid = item["tpid"];
                            itemData.num = item["cnt"];
                            //itemData.bnd = item["bnd"];
                            itemData.isEquip = false;
                            if (item.ContainsKey("intensify_lv"))
                            {
                                a3_EquipModel.getInstance().equipData_read(itemData, item);
                            }
                            a3_BagModel.getInstance().addHouseItem(itemData);
                        }
                    }
                    if (data.ContainsKey("rmvids"))
                    {
                        Variant info4 = data["rmvids"];
                        foreach (uint id in info4._arr)
                        {
                            a3_BagModel.getInstance().removeHouseItem(id);
                        }
                    }

                    dispatchEvent(GameEvent.Create(EVENT_ROOM_TURN, this, data));
                    break;
                case 6:
                    uint itemId = data["id"];
                    a3_BagItemData one1 = a3_EquipModel.getInstance().getEquipByAll(itemId);
                    one1.ismark = data["mark"];
                    if (a3_BagModel.getInstance().getItems().ContainsKey(itemId))
                    {
                        a3_BagModel.getInstance().addItem(one1);
                    }
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(itemId))
                    {
                        a3_EquipModel.getInstance().addEquip(one1);
                    }
                    a3_BagModel.getInstance().isFirstMark = false;
                    if (a3_equiptip.instans)
                        a3_equiptip.instans.IsfirstMark();
                    if (a3_bag.indtans)
                        a3_bag.indtans.refreshMark(itemId);
                    break;
            }
        }

        bool isRanse = false;

        public void onUseItems(Variant data)
        {
            int res = data["res"];
            debug.Log("使用" + data.dump());
            if (res <= 0)
            {
                //Globle.err_output(res);
                if (res == -1101) flytxt.instance.fly(ContMgr.getCont("BagProxy_noplace"));
                if (res == -903) flytxt.instance.fly(ContMgr.getCont("BagProxy_notodo"));
                if (res == -539) flytxt.instance.fly(ContMgr.getCont("BagProxy_hanepet"));
                if (res == -969) flytxt.instance.fly(ContMgr.getCont("BagProxy_nonum"));
                if (res == -899) flytxt.instance.fly(ContMgr.getCont("BagProxy_nolv"));
                if (res == -2302) flytxt.instance.fly(ContMgr.getCont("BagProxy_expmax"));
                if (res == -9002) flytxt.instance.fly(ContMgr.getCont("BagProxy_lifemax"));
                if (res == -220) flytxt.instance.fly(ContMgr.getCont("BagProxy_nopoint"));
                if (res == -153) flytxt.instance.fly(ContMgr.getCont("HaveRoleName"));
                if (res == -153) flytxt.instance.fly(ContMgr.getCont("NotChangeName"));
                return;
            }

            if (res == 1)
            {
                if (data["tpid"] == 1528)
                {
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_addonetime"));
                }
                if (data["tpid"] >= 1511 && data["tpid"] <= 1515)
                {
                    int toid = data["tpid"];
                    string str = "";
                    switch (toid)
                    {
                        case 1511: str = ContMgr.getCont("BagProxy_liliang"); break;
                        case 1512: str = ContMgr.getCont("BagProxy_mingjie"); break;
                        case 1513: str = ContMgr.getCont("BagProxy_tili"); break;
                        case 1514: str = ContMgr.getCont("BagProxy_moli"); break;
                        case 1515: str = ContMgr.getCont("BagProxy_zhihui"); break;
                        default:
                            break;
                    }
                    // flytxt.instance.fly("使用成功，已经重置" + data["cnt"] + "点" + str);
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_newpoint",new List<string> { data["cnt"]._uint.ToString(),str}));
                }
                #region
                //if (data.ContainsKey("dress"))
                //{//时装礼包
                //    Variant dressInfo = data["dress"];
                //    ArrayList dresslist = new ArrayList();
                //    foreach (Variant dress in dressInfo._arr)
                //    {
                //        int id = dress["id"];
                //        int cnt = dress["cnt"];
                //        dresslist.Add(id);
                //        dresslist.Add(cnt);
                //    }
                //    InterfaceMgr.getInstance().open(InterfaceMgr.GETGIFT, dresslist);
                //}
                //if (data.ContainsKey("itms"))
                //{//物品礼包
                //    Variant info = data["itms"];
                //    ArrayList itemlist = new ArrayList();
                //    foreach (Variant item in info._arr)
                //    {
                //        int id = item["id"];
                //        int cnt = item["cnt"];
                //        itemlist.Add(id);
                //        itemlist.Add(cnt);
                //    }
                //    InterfaceMgr.getInstance().open(InterfaceMgr.GETGIFT, itemlist);            
                //}

                //BagItemData itemData = new BagItemData();
                //itemData.id = data["tpid"];
                //itemData.num = data["cnt"];
                //BagModel.getInstance().remove(itemData);
                //UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_USE_ITEMS, this, data));
                #endregion
            }
            if (res == 19)
            {
                uint id = data["id"];
                a3_BagItemData equip = a3_EquipModel.getInstance().getEquips()[id];
                equip.equipdata.color = data["colour"];
                a3_EquipModel.getInstance().addEquip(equip);
                a3_EquipModel.getInstance().equipColor_on(data["colour"]);
                if (equip.equipdata.color > 0)
                {
                    var str = a3_BagModel.getInstance().getItemXml((int)equip.equipdata.color).getString("item_name").Substring(0, 2);
                    //flytxt.instance.fly("使用成功，装备已染成漂亮的" + str + "!");
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_ranse", new List<string> { str}));
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_light"));
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_DYETIP);
                isRanse = true;
            }
            if (res == 2)
            {
                #region
                //HeroData one = new HeroData();
                //one.petId = data["heroexp_hero_id"];
                //one.exp = data["exp"];
                //HeroData old = HeroModel.getInstance().getHeros()[one.petId];
                //if (data.ContainsKey("hero_data"))
                //{
                //    Variant info = data["hero_data"];
                //    one.lv = info["level"];
                //    one.hp = info["hp"];
                //    one.strenglv = info["strengthen"];
                //    one.quality = info["quality"];
                //    one.growth = info["growth"];
                //    one.character = info["character"];
                //    one.attr = HeroModel.getInstance().getAttri(info);

                //    if (info.ContainsKey("combpt"))
                //    {
                //        one.combpt = info["combpt"];
                //    }
                //}
                //else
                //{
                //    one.lv = old.lv;
                //    one.hp = old.hp;
                //    one.strenglv = old.strenglv;
                //    one.quality = old.quality;
                //    one.growth = old.growth;
                //    one.character = old.character;
                //    one.attr = old.attr;
                //    one.combpt = old.combpt;
                //}
                //HeroModel.getInstance().addHero(one);


                //int oldexp = old.exp;
                //int oldlv = old.lv;
                //int newexp = one.exp;
                //int newlv = one.lv;

                //if (newlv == oldlv)
                //{
                //    string str = "经验增加：" + (newexp - oldexp);
                //    flytxt.instance.fly(str, 2);
                //}
                //else
                //{
                //    SXML xml;
                //    int addexp = 0;

                //    for (; oldlv < newlv; oldlv++)
                //    {
                //        if (old.xml.attackType == 0)
                //        {
                //            xml = XMLMgr.instance.GetSXML("hero_system.hero_level_0", "level==" + (oldlv+1));
                //        }
                //        else
                //        {
                //            xml = XMLMgr.instance.GetSXML("hero_system.hero_level_1", "level==" + (oldlv+1));
                //        }
                //        if (xml.getInt("cost_exp") < 0)
                //        {
                //            break;
                //        }
                //        addexp += xml.getInt("cost_exp");
                //    }    

                //    addexp = addexp - oldexp + newexp;

                //    string str = "经验增加：" + addexp;
                //    flytxt.instance.fly(str, 2);
                //}

                //dispatchEvent(GameEvent.Create(EVENT_USE_ADD_HEROEXP, this, data));
                #endregion
            }
            if (res == 13)
            {
                if (data.ContainsKey("money"))
                {
                    //flytxt.instance.fly("获得金币：" + data["money"]);
                }
                if (data.ContainsKey("itms"))
                {
                    Variant info = data["itms"];
                    foreach (Variant item in info._arr)
                    {
                        //flytxt.instance.fly("获得道具：" + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name") + "x" + item["cnt"]);
                        flytxt.instance.fly(ContMgr.getCont("BagProxy_geteitem") + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name") + "x" + item["cnt"]);
                    }
                }
                if (data.ContainsKey("eqps"))
                {
                    Variant info = data["eqps"];
                    foreach (Variant item in info._arr)
                    {
                        flytxt.instance.fly(ContMgr.getCont("BagProxy_getequip") + a3_BagModel.getInstance().getItemXml(item["tpid"]).getString("item_name"));
                    }
                }
            }
        }
    }
}
