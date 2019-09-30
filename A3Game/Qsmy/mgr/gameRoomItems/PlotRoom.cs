using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class PlotRoom : BaseRoomItem
    {
        Variant enterdata;
        int killnum;//击杀数量
        int moneynum;//获得金币
        double entertimer;
        public override void onStart(Variant svr)
        {
            base.onStart(svr);
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            enterdata = muLGClient.instance.g_levelsCT.get_curr_lvl_info();
            entertimer = muNetCleint.instance.CurServerTimeStamp;
            a3_insideui_fb.ShowInUI(a3_insideui_fb.e_room.Normal);
            killnum = 0;
            moneynum = 0;

        }

        public override void onEnd()
        {
            base.onEnd();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.updateUICseth();
            if (a3_liteMinimap.instance != null)
                a3_liteMinimap.instance.refreshByUIState();

            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            killnum = 0;
            moneynum = 0;

            a3_insideui_fb.CloseInUI();

            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
           // LevelProxy.getInstance().sendGet_lvl_cnt_info(1);
        }

        public override void onMonsterDied(MonsterRole monster)
        {
            killnum++;
        }

        public override void onPickMoney(int num)
        {
            moneynum+=num;
        }

        public override bool onLevelFinish(Variant msgData)
        {
          
            base.onLevelFinish(msgData);
            //CollectAllDrops1();
            if (a3_liteMiniBaseMap.instance != null) a3_liteMiniBaseMap.instance.clear();
            int waittime = 0;
            a3_insideui_fb.instance.transform.FindChild("normal/btn_quitfb").gameObject.SetActive(false);
            if (msgData.ContainsKey("win"))
            {
                int ct = msgData["win"];
                if (ct > 0) waittime = 3;
            }
            //else if (!SelfRole._inst.isDead) {
            //	waittime = 5;
            //} //由于结束副本在死亡之前发，所以改成副本时间来判断
            if (muNetCleint.instance.CurServerTimeStamp < enterdata["end_tm"] - 1)
            {
                waittime = 3;
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
                    al.Add(killnum);
                    al.Add(moneynum);
                    InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_FB_FINISH, (System.Collections.ArrayList)al);
                })
            );
            return true;
        }
    }
}
