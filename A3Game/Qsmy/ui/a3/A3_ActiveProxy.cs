using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;

namespace MuGame
{
    class A3_ActiveProxy : BaseProxy<A3_ActiveProxy>
    {
        public static uint EVENT_MLZDOPCUCCESS = 2;                 //魔物猎人操作完成
        public static uint EVENT_PVPSITE_INFO = 4;
        public static uint EVENT_PVPGETREW = 5;
        public static uint EVENT_MWLR_NEW = 1562; 
        public static uint EVENT_ONBLESS = 10000;                       //获得祝福

        public A3_ActiveProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_ACTIVE_MWSL, Active_MWSLOP);//
            addProxyListener(PKG_NAME.C2S_A3_ACTIVE_BOSS, Active_BOSSOP);
            addProxyListener(PKG_NAME.C2S_A3_FB_BLESSING, OnBlessing);
            addProxyListener(PKG_NAME.C2S_A3_ACTIVE_SWEEP, OnSweep);

        }

        public void SendLoadActivies()
        {
            SendGetBossInfo();
        }

        #region ====魔物猎人
        //1、获得狩猎信息
        public void SendGetHuntInfo()
        {
            Variant msg = new Variant();
            msg["op"] = 1;//op
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_MWSL, msg);
        }

        //2、请求搜索狩猎
        public void SendStartHunt()
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_MWSL, msg);
        }

        //3、放弃狩猎
        public void SendGiveUpHunt()
        {
            Variant msg = new Variant();
            msg["op"] = 3;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_MWSL, msg);
        }
        //4丶购买vip次数
        public void SendVipCount()
        {
            Variant msg = new Variant();
            msg["op"] = 4;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_MWSL, msg);
        }

        public void ActiveMWSLSearch(Variant data)
        {            
            A3_ActiveModel.getInstance().mwlr_map_info = data["map_info"];
            A3_ActiveModel.getInstance().mwlr_doubletime = data["double_limit"];
            A3_ActiveModel.getInstance().mwlr_totaltime = data["lose_tm"];
            A3_ActiveModel.getInstance().mwlr_map_id.Clear();
            A3_ActiveModel.getInstance().mwlr_mons_pos.Clear();
            for (int i = 0; i < data["map_info"].Count; i++)
                if (data["map_info"][i].ContainsKey("map_id"))
                {
                    int map_id = data["map_info"][i]["map_id"]._int;
                    Vector3 mon_pos = new Vector3(
                        x:data["map_info"][i]["x"] / GameConstant.PIXEL_TRANS_UNITYPOS,
                        y:0,
                        z:data["map_info"][i]["y"] / GameConstant.PIXEL_TRANS_UNITYPOS
                    );
                    A3_ActiveModel.getInstance().mwlr_map_id.Add(map_id);
                    A3_ActiveModel.getInstance().mwlr_mons_pos.Add(map_id, mon_pos);
                }
        }


        //4、魔物猎人操作
        public void Active_MWSLOP(Variant data)
        {
            debug.Log("收到魔物猎人消息" + data.dump());
            int op = data["res"];
            if (op < 0)
                Globle.err_output(op);
            A3_ActiveModel.getInstance().mwlr_charges = data["count"] ?? A3_ActiveModel.getInstance().mwlr_charges;
            //debug.Log(data.dump());
            switch (op)
            {
                case 1:
                    A3_ActiveModel.getInstance().vip_buy_count = data["vip_cnt"];
                    a3_active_mwlr_kill.initLoginData=data["map_info"];
                    ActiveMWSLSearch(data);
                    break;
                case 2:
                    ActiveMWSLSearch(data);
                    a3_active_mwlr_kill.Instance.Reset();
                    A3_ActiveModel.getInstance().mwlr_giveup = false;
                    dispatchEvent(GameEvent.Create(EVENT_MWLR_NEW, this, data));
                    break;
                case 3:
                    a3_active_mwlr_kill.initLoginData = data["map_info"];
                    ActiveMWSLSearch(data);
                    A3_ActiveModel.getInstance().mwlr_map_id.Clear();
                    A3_ActiveModel.getInstance().mwlr_mons_pos.Clear();
                    A3_ActiveModel.getInstance().mwlr_map_info?._arr.Clear();
                    A3_ActiveModel.getInstance().mwlr_giveup = true;
                    a3_active.MwlrIsDoing = false;
                    a3_active_mwlr_kill.Instance.Clear();
                    if(a3_expbar.instance)
                        a3_expbar.instance.DownTip();
                    break;
                case 4:
                    for (int i = 0; i < A3_ActiveModel.getInstance().mwlr_map_info.Count; i++)
                        if (A3_ActiveModel.getInstance().mwlr_map_info[i]["map_id"] == data["map_id"]._int)
                        {
                            A3_ActiveModel.getInstance().mwlr_map_info[i]["kill"]._bool = true;                            
                            if (!A3_ActiveModel.getInstance().listKilled.Contains(i))
                            A3_ActiveModel.getInstance().listKilled.Add(i);
                            break;
                        }
                    A3_ActiveModel.getInstance().mwlr_mon_killed++;
                    a3_active_mwlr_kill.Instance.Refresh();
                    if (SelfRole.fsm.Autofighting &&
                        PlayerModel.getInstance().task_monsterIdOnAttack.Remove(-1)) // 将-1作为monster hunter的任务id
                            SelfRole.fsm.Stop(); // 如果remove成功,说明玩家是导航过去专程打这个目标怪;
                                                 // 如果remove失败,则说明玩家是在自动挂机的过程中顺手把这个目标怪给打死了
                    A3_ActiveModel.getInstance().mwlr_on = false;                    
                    break;
                case 5:
                    a3_active.MwlrIsDoing = false;
                    a3_expbar.instance.DownTip();
                    a3_active_mwlr_kill.Instance.Clear();
                    break;
            }
            if (a3_liteMinimap.instance)
                a3_liteMinimap.instance.OnActiveInfoChange();

            dispatchEvent(GameEvent.Create(EVENT_MLZDOPCUCCESS, this, data));
        }
        #endregion
        #region ====世界Boss

        //
        //1、获得世界Boss信息
        public void SendGetBossInfo()
        {
            Variant msg = new Variant();
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_BOSS, msg);
        }
        //2、世界boss操作
        public void Active_BOSSOP(Variant data)
        {
            debug.Log("166世界boss信息:"+data.dump());
            if (data.ContainsKey("boss_status"))
            {
                for (int i = 0; i < A3_EliteMonsterModel.getInstance().bossid.Length; i++)
                {

                    if (data["boss_status"][i] != null)
                    {
                        A3_EliteMonsterModel.getInstance().bossid[i] = int.Parse(data["boss_status"][i]["index"].dump());
                        A3_EliteMonsterModel.getInstance().boss_status[i] = int.Parse(data["boss_status"][i]["status"].dump());
                        Debug.LogWarning("bossid" + A3_EliteMonsterModel.getInstance().bossid[i] + "+" + "boss_status" + A3_EliteMonsterModel.getInstance().boss_status[i]);


                        string name = "";
                        if (data["boss_status"][i].ContainsKey("killer_name"))
                            name = data["boss_status"][i]["killer_name"];
                        if (data["boss_status"][i].ContainsKey("dmg_list") && data["boss_status"][i]["dmg_list"].Count > 0)
                        {
                            debug.Log("伤害排行");
                            List<dmg_list> lst = new List<dmg_list>();
                            for (int j = 0; j < data["boss_status"][i]["dmg_list"].Count; j++)
                            {

                                int ranks = j;
                                dmg_list sl = new dmg_list();
                                int mid=0;
                                switch(data["boss_status"][i]["index"]._int)
                                {
                                    case 1:
                                        mid = 2012;
                                        break;
                                    case 2:
                                        mid = 3126;
                                        break;
                                    case 3:
                                        mid = 2000;
                                        break;
                                    default:
                                        break;
                                }
                                sl.mid = mid;
                                sl.cid = data["boss_status"][i]["dmg_list"][j]["cid"]._int;
                                sl.name = data["boss_status"][i]["dmg_list"][j]["name"]._str;
                                sl.dmg = data["boss_status"][i]["dmg_list"][j]["dmg"]._int;
                                sl.rank = ranks + 1;
                                sl.lat_name = name;
                                lst.Add(sl);
                                A3_EliteMonsterModel.getInstance().dic_dmg_lst[sl.mid] = lst;
                            }

                        }
                    }
                }
                dispatchEvent(GameEvent.Create(EliteMonsterProxy.EVENT_BOSSOPSUCCESS, this, data));
            }
            //收到boss死亡或者复活
            debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + data.dump());
            
            uint zhuan = PlayerModel.getInstance().up_lvl;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            bool can = true;
            if (data.ContainsKey("index"))
            {

                string xml = XMLMgr.instance.GetSXML("worldboss.boss", "id==" + data["index"]).getString("level_limit");
                string[] str = xml.Split(',');
                uint needzhuan = uint.Parse(str[0]);
                //1：复活
                if (data["status"] == 1)
                {
                    
                    if (zhuan >= needzhuan)
                    {
                        dic[data["index"]] = data["status"];
                        can = true;
                    }
                    else
                    {
                        if (dic.Count > 0)
                            can = true;
                        else
                        {
                            can = false;
                            EliteMonsterProxy.getInstance().SendProxy();
                        }

                    }
                    IconAddLightMgr.getInstance().showOrHideFires("shijieboss_Light_enterElite", can);
                }
                //2：死了
                else if(data["status"] == 2)
                {
                    if(dic.ContainsKey(data["index"]))
                        dic.Remove(data["index"]);
                    if (dic.Count > 0)
                        can = true;
                    else
                    {
                        can = false;
                        EliteMonsterProxy.getInstance().SendProxy();
                    }
                    IconAddLightMgr.getInstance().showOrHideFires("shijieboss_Light_enterElite", can);
                }
                //3：不存在
                else
                {

                }
            }
        }

        #endregion

        #region ====副本内部

        //副本内部
        public void SendGetBlessing(int type)
        {    //4绑钻 3钻石
            Variant msg = new Variant();
            msg["state_type"] = type;
            sendRPC(PKG_NAME.C2S_A3_FB_BLESSING, msg);
        }

        public void OnBlessing(Variant data)
        {
            //debug.Log(data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            A3_ActiveModel.getInstance().blessnum_yb = data["yb_cnt"];
            A3_ActiveModel.getInstance().blessnum_ybbd = data["bndyb_cnt"];

            flytxt.instance.fly(ContMgr.getCont("uilayer_a3_blessing_7"));
            dispatchEvent(GameEvent.Create(EVENT_ONBLESS, this, null));
        }
        #endregion

        public void  sendsweep(uint type) {
            Variant msg = new Variant();
            msg["op"] = 1;
            msg["sweep_type"] = type;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_SWEEP, msg);
            debug.Log(""+ msg.dump ());
        }

        public void sendget()
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_SWEEP, msg);
            debug.Log("" + msg.dump());
        }

        
        void OnSweep(Variant data)
        {
            Debug.Log("hhh"+ data.dump ());
            int res = data["res"];
            switch (res) {
                case 1://扫荡
                    bool isok = false;

                    if (A3_ActiveModel.getInstance().nowlvl != data["diff_floor"])
                    {
                        isok = true;
                    }

                    if (data.ContainsKey("diff_floor"))
                    {
                        A3_ActiveModel.getInstance().nowlvl = data["diff_floor"];

                    }
                    if (data.ContainsKey("sweep_type"))
                        A3_ActiveModel.getInstance().sweep_type = data["sweep_type"];


                    if (a3_active_mlzd.instans != null)
                    {
                        if(isok)
                            a3_active_mlzd.instans.onSweep();
                    }
                    break;
                case 2://领奖
                    int lvl = 0;
                    if (A3_ActiveModel.getInstance().nowlvl != data["diff_floor"])
                    {
                        lvl = A3_ActiveModel.getInstance().nowlvl;
                    }
                    if (data.ContainsKey("diff_floor"))
                    {
                        A3_ActiveModel.getInstance().nowlvl = data["diff_floor"];
                    }
                    if(data.ContainsKey("sweep_type"))
                        A3_ActiveModel.getInstance().sweep_type = data["sweep_type"];
                    if (data.ContainsKey("times"))
                        A3_ActiveModel.getInstance().count_mlzd = data["times"];
                    if (a3_active_mlzd.instans != null)
                    {
                        a3_active_mlzd.instans.onGetFit(lvl);
                    }
                    break;
            }

        }
        public void SendPVP(int val)
        {
            Variant msg = new Variant();
            msg["subcmd"] = val;
            sendRPC(PKG_NAME.S2C_AREAN_TOTAL_RES, msg);
        }
        public void OnPVP_site(Variant data)
        {
            int res = data["res"];
            debug.Log("PVP:" + data.dump());
            if (res < 0) {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                case 1://请求比武场信息结果
                    A3_ActiveModel.getInstance().grade = data["grade"];
                    A3_ActiveModel.getInstance().score = data["score"];
                    if (data["last_grade"] <= 0) { A3_ActiveModel.getInstance().lastgrage = 1; }
                    else
                    {
                        A3_ActiveModel.getInstance().lastgrage = data["last_grade"];
                    }
                    A3_ActiveModel.getInstance().pvpCount = data["cnt"];
                    A3_ActiveModel.getInstance().buyCount = data["buy_cnt"];
                    A3_ActiveModel.getInstance().Canget = data["rec_rwd"];
                    dispatchEvent(GameEvent.Create(EVENT_PVPSITE_INFO, this, data));
                    break;
                case 2://竞技场匹配
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.openFind();
                    break;
                case 3://取消匹配
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.CloseFind();
                    break;
                case 4://购买挑战次数
                    A3_ActiveModel.getInstance().buyCount = data["buy_cnt"];
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.refCount_buy(data["buy_cnt"]);
                    break;
                case 5:
                    if (data.ContainsKey ("rec_rwd")) { A3_ActiveModel.getInstance().Canget = data["rec_rwd"]; }
                    dispatchEvent(GameEvent.Create(EVENT_PVPGETREW, this, data));
                    
                    break;
                case 6:
                    bool b = false ;
                    if (data["open"] == 0)
                        b = false;
                    else if (data["open"] == 1)
                        b = true;
                    //if (a3_getJewelryWay.instance)
                    //{
                    //    a3_getJewelryWay.instance.setTxt_jjc(b);
                    //}
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.setbtn(b);
                    break;
                case 7: break;
                case 8://匹配成功，同步次数
                    A3_ActiveModel.getInstance().pvpCount = data["cnt"];
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.refCount(data["cnt"]);
                    break;
                case 9://战斗结束，同步分数段位
                    A3_ActiveModel.getInstance().grade = data["grade"];
                    A3_ActiveModel.getInstance().score = data["score"];
                    if (a3_active_pvp.instance != null)
                        a3_active_pvp.instance.refro_score();
                    break;
            }
        }
    }
}
