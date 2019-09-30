using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class PVPRoom : BaseRoomItem
    {
        public static bool isOpen;
        uint startExp;
        MapData data;
        double entertimer;
        Variant enterdata;
        int Getach = 0;
        int GetExp = 0;
        public static PVPRoom instan;
        bool showHpBtn;
        public override void onStart(Variant svr)
        {
            base.onStart(svr);
            showHpBtn = a1_gamejoy.inst_skillbar.m_skillbar_hp_Add_btn.gameObject.activeSelf;
            a1_gamejoy.inst_skillbar.m_skillbar_hp_Add_btn.gameObject.SetActive(false);
            instan = this;
            a3_insideui_fb.room = this;
            isOpen = true;
            data = MapModel.getInstance().getMapDta(107);
            if (data == null)
            {
                data = new MapData();
            }
            MapModel.getInstance().AddMapDta(107, data);
            //data.OnKillNumChange = (int i) => {
            //    if (a3_insideui_fb.instance != null)
            //    {
            //        a3_insideui_fb.instance.SetKillNum(i);
            //    }
            //};
            startExp = PlayerModel.getInstance().exp;
            entertimer = muNetCleint.instance.CurServerTimeStamp;
            a3_insideui_fb.begintime = entertimer;
            enterdata = muLGClient.instance.g_levelsCT.get_curr_lvl_info();
            MapModel.getInstance().curLevelId = 107;
           // InterfaceMgr.doCommandByLua("MapModel:getInstance().getcurLevelId", "model/MapModel", 107);
            a3_insideui_fb.ShowInUI(a3_insideui_fb.e_room.PVP);
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();
            if (a1_gamejoy.inst_skillbar) { a1_gamejoy.inst_skillbar.clearCD(); }
            Variant v = new Variant();
            InterfaceMgr.doCommandByLua("MapModel:getInstance().getcurLevelId", "model/MapModel", MapModel.getInstance().curLevelId);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_high_fightgame.close_heroih_ani", "ui/interfaces/high/a1_high_fightgame");
            if (tragethead.instance != null)
            {
                tragethead.instance.inFB = true;
            }
            if (data != null)
            {
                data.cycleCount++;
            }

            if (a3_expbar.instance)
            {
                a3_expbar.instance.showBtnIcon(false);
            }
            if (a3_equipup.instance)
            {
                a3_equipup.instance.showbtnIcom(false);
            }
            if (a1_gamejoy.inst_joystick)
            {
                a1_gamejoy.inst_joystick.show_btnIcon(false);
            }
            if (a3_liteMinimap.instance)
            {
                a3_liteMinimap.instance.showbtnIcon(false);
            }
        }
        public override void onEnd()
        {
            base.onEnd();
            data.kmNum = 0;
            isOpen = false;
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
   

            //InterfaceMgr.openByLua("herohead2");
            InterfaceMgr.doCommandByLua("a1_low_fightgame.show_heroih_cs", "ui/interfaces/low/a1_low_fightgame");

            if (tragethead.instance != null)
            {
                tragethead.instance.inFB = false;
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
            LevelProxy.getInstance().sendGet_lvl_cnt_info(1);
            a1_gamejoy.inst_skillbar.m_skillbar_hp_Add_btn.gameObject.SetActive(showHpBtn);

            if (a3_expbar.instance)
            {
                a3_expbar.instance.showBtnIcon(true);
            }

            if (a3_equipup.instance)
            {
                a3_equipup.instance.showbtnIcom(true);
            }

            if (a1_gamejoy.inst_joystick)
            {
                a1_gamejoy.inst_joystick.show_btnIcon(true);
            }

            if (a3_liteMinimap.instance)
            {
                a3_liteMinimap.instance.showbtnIcon(true);
            }
        }


        public void refGet(int ach, int exp)
        {
            Getach = ach;
            GetExp = exp;
            getach = Getach;
            getExp = GetExp;
        }
        public override bool onLevelFinish(Variant msgData)
        {
            base.onLevelFinish(msgData);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
            if (a3_liteMiniBaseMap.instance != null) a3_liteMiniBaseMap.instance.clear();
            int waittime = 0;
            if (msgData.ContainsKey("win"))
            {
                int ct = msgData["win"];
                if (ct > 0) waittime = 1;
            }
            //else if (!SelfRole._inst.isDead) {
            //	waittime = 5;
            //} //由于结束副本在死亡之前发，所以改成副本时间来判断
            if (muNetCleint.instance.CurServerTimeStamp < enterdata["end_tm"] - 1)
            {
                waittime = 1;
            }
            new timersManager().addTimer(
            3,
            (object o) =>
            {
                MonsterMgr._inst.clear();
            }
            );
            new timersManager().addTimer(
                waittime,
(Action<object>)((object o) =>
                {
                    //CollectAllDrops1();

                    msgData["ltpid"] = enterdata["ltpid"];
                    System.Collections.ArrayList al = new System.Collections.ArrayList();
                    al.Add(msgData);
                    double endtime = enterdata["end_tm"];
                    double temp = (double)UnityEngine.Mathf.Min(muNetCleint.instance.CurServerTimeStamp, (int)endtime) - entertimer;
                    al.Add(temp);
                    al.Add(data.kmNum);
                    int t = (int)UnityEngine.Mathf.Max(0, PlayerModel.getInstance().exp - startExp);
                    al.Add(t);
                    InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_FB_FINISH, (System.Collections.ArrayList)al);
                })
            );

            return true;
        }
        public void onLevelFinish() { }
        override public void onGetMapMoney(int money)
        {
            FightText.play(FightText.MONEY_TEXT, lgSelfPlayer.instance.grAvatar.getHeadPos(), money, false);
        }
    }
}
