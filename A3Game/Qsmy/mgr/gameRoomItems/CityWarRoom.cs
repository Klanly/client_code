using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class CityWarRoom : BaseRoomItem
    {
        public static bool isOpen;
        MapData data;
        double entertimer;
        Variant enterdata;
        public override void onStart(Variant svr)
        {
            base.onStart(svr);
            debug.Log("++++++++++++++++++进入城战");
            a3_insideui_fb.room = this;
            isOpen = true;
            data = MapModel.getInstance().getMapDta(8000);
            if (data == null)
            {
                data = new MapData();
            }
            MapModel.getInstance().AddMapDta(9000, data);
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.showbtnIcon", "ui/interfaces/low/a1_low_fightgame", false);
            InterfaceMgr.doCommandByLua("a1_high_fightgame.showbtnIcon", "ui/interfaces/high/a1_high_fightgame", false);
            entertimer = muNetCleint.instance.CurServerTimeStamp;
            a3_insideui_fb.begintime = entertimer;
            enterdata = muLGClient.instance.g_levelsCT.get_curr_lvl_info();
            a3_insideui_fb.ShowInUI(a3_insideui_fb.e_room.CITYWAR);
            A3_cityOfWarProxy.getInstance().sendGetFb_info();
            if (a3_expbar.instance)
            {
                a3_expbar.instance.showBtnIcon(false);
            }

            if (a3_liteMinimap.instance)
            {
                a3_liteMinimap.instance.showbtnIcon(false);
            }

            if (a1_gamejoy.inst_joystick)
            {
                a1_gamejoy.inst_joystick.show_btnIcon(false);
            }

            if (a3_herohead.instance)
            {
                a3_herohead.instance.showbtnIcon(false);
            }

            if (a3_equipup.instance)
            {
                a3_equipup.instance.showbtnIcom(false);
            }
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LITEMINIBASEMAP);
            if (data != null)
            {
                data.cycleCount++;
            }

        }
        public override void onEnd()
        {
            base.onEnd();
            isOpen = false;

            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            if (a3_expbar.instance)
            {
                a3_expbar.instance.showBtnIcon(true);
            }

            if (a3_liteMinimap.instance)
            {
                a3_liteMinimap.instance.showbtnIcon(true);
            }
            if (a3_herohead.instance)
            {
                a3_herohead.instance.showbtnIcon(true);
            }
            if (a3_equipup.instance)
            {
                a3_equipup.instance.showbtnIcom(true);
            }
            if (a1_gamejoy.inst_joystick)
            {
                a1_gamejoy.inst_joystick.show_btnIcon(true);
            }
            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.showbtnIcon", "ui/interfaces/low/a1_low_fightgame", true);
            InterfaceMgr.doCommandByLua("a1_high_fightgame.showbtnIcon", "ui/interfaces/high/a1_high_fightgame", true);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIBASEMAP);
            LevelProxy.getInstance().sendGet_lvl_cnt_info(1);
        }

        public override bool onLevelFinish(Variant msgData)
        {
            base.onLevelFinish(msgData);
            debug.Log("OVER" + msgData.dump());
            int waittime = 0;
            if (msgData.ContainsKey("win"))
            {
                int ct = msgData["win"];
                if (ct > 0) waittime = 3;
            }
            if (muNetCleint.instance.CurServerTimeStamp < enterdata["end_tm"] - 1)
            {
                waittime = 3;
            }
            new timersManager().addTimer(
                3,
                (object o) => {
                    MonsterMgr._inst.clear();
                }
            );
            new timersManager().addTimer(
                waittime,
            (Action<object>)((object o) =>
            {


                msgData["ltpid"] = enterdata["ltpid"];
                System.Collections.ArrayList al = new System.Collections.ArrayList();
                al.Add(msgData);
                double endtime = enterdata["end_tm"];
                double temp = (double)UnityEngine.Mathf.Min(muNetCleint.instance.CurServerTimeStamp, (int)endtime) - entertimer;
                al.Add(temp);
                al.Add(data.kmNum);
                //int t = (int)UnityEngine.Mathf.Max(0, PlayerModel.getInstance().exp - startExp);      
                // al.Add(t);
                InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_FB_FINISH, (System.Collections.ArrayList)al);
                // a3_insideui_fb.instance.setAct();
            })
            );
            return true;
        }
        override public void onGetMapMoney(int money) { }

    }
}
