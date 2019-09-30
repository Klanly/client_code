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
    class a3_speedTeamProxy:BaseProxy<a3_speedTeamProxy>
    {
    //    public a3_speedTeamProxy()
    //    {
    //        addProxyListener(PKG_NAME.C2S_GET_OBJTEAM_INFO, getInfo);

    //    }
    //    private void getInfo(Variant data)
    //    {
    //        debug.Log("队伍信息" + data.dump());
    //        int res = data["res"];
    //        if (res < 0) { return; }
    //        switch (res)
    //        {
    //            case 0:
    //                break;
    //            case 1:
    //                break;
    //            case 2:
    //                break;
    //            case 3:
    //                break;
    //            case 4:
    //                break;
    //            default:
    //                break;

    //        }

    //    }
    //    public void send_objectteam(uint type)//根据队伍目的查看队伍
    //    {
    //        Variant msg = new Variant();
    //        msg["iid"] = 120;
    //        msg["ltpid"] = type;
    //        sendRPC(PKG_NAME.C2S_GET_OBJTEAM_INFO, msg);
    //    }
    //    public void send_obj_join(uint tpid)//选中申请加入
    //    {
    //        Variant msg = new Variant();
    //        msg["iid"] =120;
    //        msg["tpid"] = tpid;
    //        sendRPC(PKG_NAME.C2S_GET_OBJTEAM_INFO, msg);
    //    }

    //    internal void sendobject_change(int v)//改变队伍目的。
    //    {
    //        Variant msg = new Variant();
    //        msg["iid"] = 120;
    //        msg["ltpid"] = v;
    //        sendRPC(PKG_NAME.C2S_GET_OBJTEAM_INFO, msg);
    //    }
    }
}
