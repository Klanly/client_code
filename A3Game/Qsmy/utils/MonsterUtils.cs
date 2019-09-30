using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace MuGame
{

    class MonsterUtils
    {

        //{monsters:
        //   [mid:, iid:, x:, y:, level:怪物等级,speed:, face:, invisible:, lvlsideid:副本阵营id,hp:生命值,max_hp:最大生命值,
        //       moving:{start_tm:, to_x:, to_y:}, atking:{tar_iid:}, stats:{state_par:[{id:, par: , start_tm:, end_tm:}]}]
        //}
        static public Variant getMontserVar(int mid,uint iid,float x,float y)
        {
            Variant dta = new Variant();
            dta["mid"] = mid; dta["iid"] = iid; dta["x"] = x; dta["y"] = y;
            dta["level"] = 1; dta["speed"] = 10; dta["hp"] = 10000; dta["max_hp"] = 10000;
            return dta;
        }
    }
}
