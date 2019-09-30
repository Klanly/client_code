using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace MuGame
{
    class DragonRoom : BaseRoomItem
    {
        public override void onStart(Variant svr)
        {
            base.onStart(svr);            
            a3_insideui_fb.begintime = muNetCleint.instance.CurServerTimeStamp;
            a3_insideui_fb.ShowInUI(a3_insideui_fb.e_room.DRAGON);
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.refreshByUIState();
            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            a3_liteMinimap.instance.SetTaskInfoVisible(false);
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_HEROHEAD);
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIMAP);
            //InterfaceMgr.getInstance().close(InterfaceMgr.BROADCASTING);  



        }

        public override void onEnd()
        {
            a3_insideui_fb.CloseInUI();
            base.onEnd();
            //if (a1_gamejoy.inst_joystick != null)
            //    a1_gamejoy.inst_joystick.OnDragOut();
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.refreshByUIState();
            Variant v = new Variant();
            v["curLevelId"] = MapModel.getInstance().curLevelId;            
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_map_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.refresh_btn_ByUIState", "ui/interfaces/low/a1_low_fightgame", v);
            a3_liteMinimap.instance.SetTaskInfoVisible(true);
            //InterfaceMgr.getInstance().open(InterfaceMgr.A3_HEROHEAD);
            //InterfaceMgr.getInstance().open(InterfaceMgr.A3_LITEMINIMAP);
            //InterfaceMgr.getInstance().open(InterfaceMgr.BROADCASTING);
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
        }


    }
}
