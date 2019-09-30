using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class TlfbRoom111: BaseRoomItem
    {
        public static bool isOpen;
        MapData data;
        uint startExp;
        double entertimer;
        Variant enterdata;
        public override void onStart(Variant svr)
        {
            base.onStart(svr);
            a3_insideui_fb.room = this;
            isOpen = true;
            data = MapModel.getInstance().getMapDta(111);
            if (data == null)
            {
                data = new MapData();
            }
            MapModel.getInstance().AddMapDta(111, data);
            data.OnKillNumChange = (int i) => {
                if (a3_insideui_fb.instance != null)
                {
                    a3_insideui_fb.instance.SetKillNum(i);
                }
            };
            startExp = PlayerModel.getInstance().exp;
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
     
            entertimer = muNetCleint.instance.CurServerTimeStamp;
            a3_insideui_fb.begintime = entertimer;
            enterdata = muLGClient.instance.g_levelsCT.get_curr_lvl_info();
            a3_insideui_fb.ShowInUI(a3_insideui_fb.e_room.TLFB111);

            if (data != null)
            {
                data.cycleCount++;
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

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);

            LevelProxy.getInstance().sendGet_lvl_cnt_info(1);
            //InterfaceMgr.getInstance().open(InterfaceMgr.BROADCASTING);
        }

        public override bool onLevelFinish(Variant msgData)
        {
            base.onLevelFinish(msgData);
            CollectAllDrops();
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
            if (a3_liteMiniBaseMap.instance != null) a3_liteMiniBaseMap.instance.clear();
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
(Action<object>)((object o) => {


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
    a3_insideui_fb.instance.setAct();
                })
            );

            return true;
        }
        override public void onGetMapMoney(int money)
        {
            FightText.play(FightText.MONEY_TEXT, lgSelfPlayer.instance.grAvatar.getHeadPos(), money, false);
        }
    }
}
