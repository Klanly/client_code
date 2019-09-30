using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace MuGame
{
    class ArenaRoom:BaseRoomItem
    {
        public override void onStart(Variant svr)
        {
            base.onStart(svr);

           
            //InterfaceMgr.getInstance().open(InterfaceMgr.FLOAT_ARENA);
            //herohead.instance.showOnlyHero();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_HEROHEAD);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIMAP);
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_MINIMAP);
            InterfaceMgr.getInstance().close(InterfaceMgr.BROADCASTING);
            debug.Log("!!ArenaRoom start!!");
            cdtime.show(doBgein);
        }


        private void doBgein()
        {
           // skillbar.setAutoFightType(1);
           
        }


        public override void onEnd()
        {
            base.onEnd();
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.OnDragOut();
            //InterfaceMgr.getInstance().close(InterfaceMgr.FLOAT_ARENA);
            debug.Log("!!ArenaRoom end!!");

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HEROHEAD);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LITEMINIMAP);

          //  InterfaceMgr.getInstance().open(InterfaceMgr.A3_MINIMAP);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.BROADCASTING);
        }

        //override public void onMonsterEnterView(GRAvatar gr)
        //{
            

        //    debug.Log("!!arenaRooom monster enter!!!");

        //    if (gr.m_nSex >= 0)
        //    {
        //        if (floatui_arena_hp.instance != null)
        //            floatui_arena_hp.instance.setEnemy(gr);
        //    }
        //}


        //override public bool onPrizeFinish(Variant msgData)
        //{
        //    int res = msgData["res"];
        //    if (res == 2)
        //    {
        //        int point = msgData["point"];
        //        int money = msgData["money"];

        //        arena_account.point = point;
        //        arena_account.money = money;

        //        InterfaceMgr.getInstance().open(InterfaceMgr.ARENA_ACCOUNT);
        //    }


        //    return true;
        //}

        //override public bool onLevelFinish(Variant msgData)
        //{
        //    if (floatui_arena_hp.instance)
        //        floatui_arena_hp.instance.endCD();

        //     bool win=false;
        //    if(msgData.ContainsKey("win"))
        //        win = msgData["win"]==1;
        //    arena_account.isWin = win;

        //    return true;
        //}
    }
}
