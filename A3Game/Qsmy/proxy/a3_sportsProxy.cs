using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class a3_sportsProxy : BaseProxy<a3_sportsProxy>
    {


        public static uint EVENT_FINDING = 1;      // 寻找比赛中
        public static uint EVENT_FINDNOT = 2;      //取消匹配，匹配超时
        public static uint EVENT_FINDSCE = 20;      // 匹配成功
        public static uint EVENT_INFB_LOS = 21;     // 准备取消
        public static uint EVENT_TOSURE = 19;       // 准备情况
        public static uint EVENT_INFO = 25;         //占领信息
        public static uint EVENT_OWNER_INFO = 6;    //所有地图成员信息
        public static uint EVENT_MYZC_INFO = 4;     //我的战场积分信息
        public static uint EVENT_KILL_SCE = 22;       //击杀得分
        public static uint EVENT_KILL_INFO = 24;       //击杀广播
        public static uint EVENT_HELP_KILL = 26;      //自己助攻通知

        //jjc
        public static uint EVENT_PVPSITE_INFO = 104;
        public static uint EVENT_PVPGETREW = 105;
        public a3_sportsProxy() {
            addProxyListener(PKG_NAME.C2S_SPORTS,onsport);
            addProxyListener(PKG_NAME.S2C_AREAN_TOTAL_RES, OnPVP_site);
        }


        public void GotoJJCmap(int llid, int npcid = 0,bool bslvl=false )
        {
            Variant data = new Variant();
            data["llid"] = llid;
            data["npcid"] = npcid;
            data["bslvl"] = bslvl;
            sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
        }

        public List<OtherPlayerPos_jdzc> list_position = new List<OtherPlayerPos_jdzc>();

        public void find_game() {
            //开始匹配
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }

        public void cancel_game() {
            //取消匹配
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }
        public void toSure_game(bool sure) {
            //确定/取消准备
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["ready"] = sure;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }

        public void getPrestige_info() {
            //获取今天获得的声望点数
            Variant msg = new Variant();
            msg["op"] = 4;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }

        public void getAll_info() {
            //获取数据统计信息
            Variant msg = new Variant();
            msg["op"] = 5;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }
        public void getTeam_info()
        {
            //获取所有地图成员信息
            Variant msg = new Variant();
            msg["op"] = 6;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }

        public void getTeam_pos() {
            //获取所有人坐标
            Variant msg = new Variant();
            msg["op"] = 7;
            sendRPC(PKG_NAME.C2S_SPORTS, msg);
        }
        void onsport(Variant data)
        {
            int res = data["res"];
            debug.Log("Sport:" + data.dump());
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            switch (res) {
               
                case 1:
                    //正在匹配
                    a3_sportsModel.getInstance().sport_stage = Game_stage.find;
                    dispatchEvent(GameEvent.Create(EVENT_FINDING, this, data));
                    break;
                case 2:
                    //主动取消匹配  匹配超时通知 
                    a3_sportsModel.getInstance().sport_stage = Game_stage.nul;
                    dispatchEvent(GameEvent.Create(EVENT_FINDNOT, this, data));
                    break;
                case 3:break;
                case 4:
                    if (data.ContainsKey("point"))
                    {
                        a3_sportsModel.getInstance().Score_jdzc = data["point"];
                    }

                    if (data .ContainsKey ("rank")) {
                        a3_sportsModel.getInstance().Ranking_jdzc = data["rank"];
                    }
                    dispatchEvent(GameEvent.Create(EVENT_MYZC_INFO, this, data));
                    break;
                case 5:
                    //数据统计
                    Variant info = data["data"];
                    foreach (var one in info._arr)
                    {
                        int cid = one["cid"];
                        if (a3_sportsModel .getInstance ().GameInfo.ContainsKey (cid)) {
                            a3_sportsModel.getInstance().GameInfo[cid].kill_cnt = one["kill_cnt"];
                            a3_sportsModel.getInstance().GameInfo[cid].die_cnt  = one["die_cnt"];
                            a3_sportsModel.getInstance().GameInfo[cid].assists_cnt  = one["assists_cnt"];
                            a3_sportsModel.getInstance().GameInfo[cid].dmg  = one["dmg"];
                            a3_sportsModel.getInstance().GameInfo[cid].ach_point  = one["ach_point"];
                        }
                    }

                    if (a3_sportsModel.getInstance().GameInfo.ContainsKey((int)PlayerModel.getInstance().cid))
                    {
                        a3_sportsModel.getInstance().kill_count = a3_sportsModel.getInstance().GameInfo[(int)PlayerModel.getInstance().cid].kill_cnt;

                        a3_sportsModel.getInstance().die_count = a3_sportsModel.getInstance().GameInfo[(int)PlayerModel.getInstance().cid].die_cnt;

                        a3_sportsModel.getInstance().helpkill_count = a3_sportsModel.getInstance().GameInfo[(int)PlayerModel.getInstance().cid].assists_cnt;
                    }
                    if (a3_insideui_fb.instance != null && a3_insideui_fb.instance.open_zhanji)
                    {
                        a3_insideui_fb.instance.openzhanji(a3_sportsModel.getInstance().getGameInfo(true));
                    }

                    break;
                case 6:
                    dispatchEvent(GameEvent.Create(EVENT_OWNER_INFO, this, data));
                    //获取所有地图成员信息
                    break;
                case 7:
                    list_position.Clear();
                    List<Variant> l = data["data"]._arr;
                    foreach (var v in l)
                    {
                        OtherPlayerPos_jdzc  temp = new OtherPlayerPos_jdzc();
                        temp.lvlsideid = v["lvlsideid"];
                        temp.iid = v["iid"];
                        temp.x = (uint)((v["x"]) / GameConstant.PIXEL_TRANS_UNITYPOS);
                        temp.y = (uint)((v["y"]) / GameConstant.PIXEL_TRANS_UNITYPOS);
                        list_position.Add(temp);
                    }

                    //获取坐标
                    break;
                case 19:
                    //他人 / 自己准备通知
                    if (data.ContainsKey("cid") && data["cid"] == PlayerModel.getInstance().cid)
                    {
                        if (data.ContainsKey("ready") && data["ready"]  ==true) {
                            a3_sportsModel.getInstance().sport_stage = Game_stage.ture_game ;
                        }
                    }
                    dispatchEvent(GameEvent.Create(EVENT_TOSURE, this, data));
                    break;
                case 20:
                    //匹配成功通知
                    a3_sportsModel.getInstance().sport_stage = Game_stage.sure;
                    dispatchEvent(GameEvent.Create(EVENT_FINDSCE, this, data));
                    break;
                case 21:
                    //取消准备 重新匹配
                    if (data.ContainsKey("cid") && data["cid"] == PlayerModel.getInstance().cid)
                    {
                        a3_sportsModel.getInstance().sport_stage = Game_stage.nul;
                        flytxt.instance.fly(ContMgr.getCont("a3_sposrtsProxy_1"));
                    }
                    else if(data.ContainsKey("cid") && data["cid"] != PlayerModel.getInstance().cid) {
                        a3_sportsModel.getInstance().sport_stage = Game_stage.find;
                        flytxt.instance.fly(ContMgr.getCont("a3_sposrtsProxy_2"));
                    }
                    dispatchEvent(GameEvent.Create(EVENT_INFB_LOS, this, data));
                    break;
                case 22:
                    //同步击杀得分
                    dispatchEvent(GameEvent.Create(EVENT_KILL_SCE, this, data));
                    break;
                case 23:
                    //中途退出  下次能匹配的时间
                    break;
                case 24:
                    //击杀广播
                    dispatchEvent(GameEvent.Create(EVENT_KILL_INFO, this, data));
                    break;

                case 25:
                    dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    break;
                case 26://自己助攻通知
                    dispatchEvent(GameEvent.Create(EVENT_HELP_KILL, this, data));
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
            if (res < 0)
            {
                Globle.err_output(res); 
                return;
            }
            switch (res)
            {
                case 1://请求比武场信息结果
                    a3_sportsModel.getInstance().grade = data["grade"];
                    a3_sportsModel.getInstance().score = data["score"];
                    if (data["last_grade"] <= 0) { a3_sportsModel.getInstance().lastgrage = 1; }
                    else
                    {
                        a3_sportsModel.getInstance().lastgrage = data["last_grade"];
                    }
                    a3_sportsModel.getInstance().pvpCount = data["cnt"];
                    a3_sportsModel.getInstance().buyCount = data["buy_cnt"];
                    a3_sportsModel.getInstance().Canget = data["rec_rwd"];
                    dispatchEvent(GameEvent.Create(EVENT_PVPSITE_INFO, this, data));
                    break;
                case 2://竞技场匹配
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.openFind();
                    break;
                case 3://取消匹配
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.CloseFind();
                    break;
                case 4://购买挑战次数
                    a3_sportsModel.getInstance().buyCount = data["buy_cnt"];
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.refCount_buy(data["buy_cnt"]);
                    break;
                case 5:
                    if (data.ContainsKey("rec_rwd")) { a3_sportsModel.getInstance().Canget = data["rec_rwd"]; }
                    dispatchEvent(GameEvent.Create(EVENT_PVPGETREW, this, data));

                    break;
                case 6:
                    bool b = false;
                    if (data["open"] == 0)
                        b = false;
                    else if (data["open"] == 1)
                        b = true;
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.setbtn(b);
                    break;
                case 7: break;
                case 8://匹配成功，同步次数
                    a3_sportsModel.getInstance().pvpCount = data["cnt"];
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.refCount(data["cnt"]);
                    break;
                case 9://战斗结束，同步分数段位
                    a3_sportsModel.getInstance().grade = data["grade"];
                    a3_sportsModel.getInstance().score = data["score"];
                    if (a3_sports_jjc.instance != null)
                        a3_sports_jjc.instance.refro_score();
                    break;
                case 10://匹配成功，对手信息
                    ArrayList arr = new ArrayList();
                    arr.Add(data["cid"]._uint);
                    uint cid=PlayerModel.getInstance().cid;
                    arr.Add(data.ContainsKey("name") ? data["name"]._str : "");//机器人
                    arr.Add(data.ContainsKey("llid") ? data["llid"]._int :-1);//先进需要主动发242进地图

                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPORTS);
                    if (data.ContainsKey("llid"))
                        GotoJJCmap(data["llid"]._int);
                    //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_PKSHOW,arr);
                    


                    break;
            }
        }
    }
}
