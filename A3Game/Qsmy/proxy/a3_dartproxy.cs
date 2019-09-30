using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace MuGame
{
    class a3_dartproxy : BaseProxy<a3_dartproxy>
    {
        public const int EVENT_GETINFO = 1;
        public const int LETSGO = 2;
        public const int EVENT_AWARDNUM = 3;
        public const int DARTHPNOW = 4;

        public bool gotoDart = true;
        public bool isme = false;
        public bool show2 = false;
        public bool dartHave = false;
        public bool canOpenDart = true;
        int per;
        public a3_dartproxy()
        {
            addProxyListener(PKG_NAME.S2C_CLAN_ESCORT, dartinfo);
        }

        //c2s
        public void sendDartGo()//查看镖车信息
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.C2S_CLAN_ESCORT, msg);
        }

        public void sendDartStart(uint line)//开始运镖
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            msg["line"] = line;
            sendRPC(PKG_NAME.C2S_CLAN_ESCORT, msg);
        }

        //s2c
        void dartinfo(Variant data)
        {
            debug.Log("镖车信息:" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                case 1: info(data); break;//运镖
                case 2: wannaGo(data); break;//通知开始
                case 3: itemNum(data); break;//完成奖励的数量
                case 4: dartHP(data); break;//刷新镖车的状态
                default:
                    break;
            }
        }
        void info(Variant data)
        {
            dispatchEvent(GameEvent.Create(EVENT_GETINFO, this, data));
            if (data["line"] > 0)
                A3_dartModel.getInstance().init(data["line"]);
            if (data["finish"])
            {
                a3_liteMinimap.instance?.getGameObjectByPath("goonDart").SetActive(false);
                dartHave = false;
            }
            canOpenDart = !data["finish"];
        }
        void wannaGo(Variant data)
        {
            if (data["line"] > 0)
                A3_dartModel.getInstance().init(data["line"]);
            if (!isme)
            {
                //flytxt.instance.fly("活动已开启");
                flytxt.instance.fly(ContMgr.getCont("starthuodong"));
                //if (data["line"] > 0)
                //    A3_dartModel.getInstance().init(data["line"]);
                if (GRMap.instance.m_nCurMapID >= 3333) return;//副本中不显示确认押镖界面
                show2 = true;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LEGION_DART);
                a3_legion_dart.instance?.getGameObjectByPath("candodart").SetActive(false);
                a3_legion_dart.instance?.getGameObjectByPath("cantdart").SetActive(true);
            }

            //dispatchEvent(GameEvent.Create(LETSGO, this, data));
        }
        void itemNum(Variant data)
        {
            List<SXML> list = XMLMgr.instance.GetSXMLList("item.item", "id==" + A3_dartModel.getInstance().item_id);
            string stri = list[0].getString("item_name");
            //flytxt.instance.fly(ContMgr.getCont("clan_11", data["item_num"]) + stri);
            dartHave = false;
            MsgBoxMgr.getInstance().showDartGetAwd(ContMgr.getCont("clan_11", data["item_num"]._uint.ToString()) + stri, A3_dartModel.getInstance().item_id, data["item_num"],per*1000);
            //dispatchEvent(GameEvent.Create(EVENT_AWARDNUM, this, data));
        }
        void yesHandle()
        {

        }
        bool showOne = false;
        void dartHP(Variant data)
        {
            per = data["hp_per"]._int;
            dispatchEvent(GameEvent.Create(DARTHPNOW, this, data));
            if (!gotoDart)
            {
                return;
            }
            if (data.ContainsKey("x"))
            {
                dartHave = true;
                float x = 0, z = 0;
                x = data["x"]._float / 53.3f;
                z = data["y"]._float / 53.3f;
                if (SelfRole._inst!=null)
                {
                    SelfRole.WalkToMap(data["map_id"], new Vector3(x, 2, z));
                }
                
            }
            if (data["hp_per"]._int <= 20 && !showOne)
            {
                flytxt.instance.fly(ContMgr.getCont("clan_9"));
                showOne = true;
            }
            
        }

    }
}
