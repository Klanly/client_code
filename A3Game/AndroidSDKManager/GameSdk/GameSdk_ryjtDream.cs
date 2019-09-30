using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class GameSdk_ryjtDream : GameSdk_base
    {
        public override void Pay(rechargeData data)
        {
            debug.Log("begin-pay");
            Variant v = new Variant();
            debug.Log("serverId:" + Globle.curServerD.sid);
            v["serverId"] = Globle.curServerD.sid;
            v["serverName"] = Globle.curServerD.server_name;
            v["serverDesc"] = Globle.curServerD.sid;
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["productId"] = data.payid;
            debug.Log("rechargeData:" + RechargeModel.getInstance().getRechargeDataById(data.id));

            debug.Log("name:" + data.name);
            v["productName"] = data.name;
            v["productPrice"] = data.golden;
            v["productCount"] = 1;
            v["productDesc"] = "description";
            v["change_rate"] = 0;
            v["roleLvl"] = PlayerModel.getInstance().lvl;

            debug.Log("end-pay");

            string LanPayInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("pay", "lanPay", LanPayInfoJsonString);
        }

        public override void record_quit()
        {
            if (Globle.Lan != "zh_cn")
                return;

            //string exitRoleInfoJsonString = "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"101\",\"roleGold\":\"300\",\"roleYb\":\"200\",\"roleServerId\":\"1\",\"roleServerName\":\"servername\"}";
            //AndroidJavaMethodInfoCall("lanRole", exitRoleInfoJsonString);
            //AndroidJavaMethodCall("exitPage");
            Variant v = new Variant();
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["roleLevel"] = PlayerModel.getInstance().lvl;
            v["roleGold"] = PlayerModel.getInstance().gold;
            v["roleleveluptime"] = "";
            v["rolecreatetime"] = "";
            v["rolevip"] = PlayerModel.getInstance().vip;
            v["roleYb"] = PlayerModel.getInstance().money;
            v["roleServerId"] = Globle.curServerD.sid;
            v["roleServerName"] = Globle.curServerD.server_name;
            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("exitPage", "lanRole", serverInfoJsonString, false);

            debug.Log("[record]quit:" + serverInfoJsonString);
        }
    }
}
