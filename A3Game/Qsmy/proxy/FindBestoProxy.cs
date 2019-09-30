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
    class FindBestoProxy : BaseProxy<FindBestoProxy>
    {
        public static uint EVENT_INFO = 1;
        public FindBestoProxy()
        {
            addProxyListener(PKG_NAME.C2S_ON_MAP_ACTIVE, onMapCount);
            //addProxyListener(PKG_NAME.C2S_GET_RECT,onOtherplayerCount);
        }

        public void sendMap(int count )
        {//兑换奖励
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["cost"] = count;
            debug.Log("PPP"+ count);
            sendRPC(PKG_NAME.S2C_ATT_CHANGE, msg);
        }
        public void getinfo()
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.S2C_ATT_CHANGE, msg);
        }

        
        void onMapCount(Variant v)
        {
            debug.Log("onMapCount"+v.dump());
            int res = v["res"];
            if (res < 0 )
            {
                Globle.err_output(res);
                return;
            }
            if (res == 1)
            {
                uint iid = v["iid"];
                BaseRole Role = RoleMgr._instance.getRole(iid);
                if (Role != null && !(Role is MonsterRole) )
                {
                    if (Role.m_isMain)
                    {
                       // FindBestoModel.getInstance().mapCount = v["num"];
                        if (SelfRole._inst != null)
                        {
                            if (v["way"] == 2)
                            {
                                int count = v["num"];
                                if (count - PlayerModel.getInstance().treasure_num > 0)
                                {
                                    // flytxt.instance.fly("抢夺了" + (count - PlayerModel.getInstance().treasure_num) + "张藏宝图");
                                    flytxt.instance.fly(ContMgr.getCont("FindBestoProxy_add",new List<string>() { (count - PlayerModel.getInstance().treasure_num).ToString()}));
                                }
                                else if (count - PlayerModel.getInstance().treasure_num < 0)
                                {
                                    //flytxt.instance.fly("丢失了" + (PlayerModel.getInstance().treasure_num - count) + "张藏宝图");
                                    flytxt.instance.fly(ContMgr.getCont("FindBestoProxy_reduce", new List<string>() { (PlayerModel.getInstance().treasure_num - count).ToString() }));
                                }
                            }
                            PlayerModel.getInstance().treasure_num = v["num"];
                            SelfRole._inst.mapCount = v["num"];
                            SelfRole._inst.refreshmapCount(v["num"]);
                            if (PlayerModel.getInstance().treasure_num >= 50)
                            {
                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAOTUUI);
                            }
                        }
                    }
                    else
                    {
                        OtherPlayerMgr._inst.playerMapCountChange(iid, v["num"]);
                    }
                }
                if (A3_FindBesto.instan)
                    A3_FindBesto.instan.refreCount();
            }
            if (res == 3)
            {
                if (v["result"] == 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("FindBestoProxy_nook"));
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("FindBestoProxy_ok"));
                }
            }
            if (res == 4)
            {
                if (v["get_collect"] == false)
                {
                    flytxt.instance.fly(ContMgr.getCont("FindBestoProxy_nothing"));
                }
            }
            if (res == 5)
            {
                dispatchEvent(GameEvent.Create(EVENT_INFO, this, v));
            }
        }

        //void onOtherplayerCount(Variant v)
        //{
        //    debug.Log("onOtherplayerCount" + v.dump());
        //    OtherPlayerMgr._inst.playerMapCountChange(v["iid"], v["treasure_num"]);
        //}
    }
}
