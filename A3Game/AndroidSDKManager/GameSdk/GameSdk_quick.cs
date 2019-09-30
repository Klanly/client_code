using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class GameSdk_quick : GameSdk_base
    {
        public override void Pay(rechargeData data)
        {
            // "{\"serverId\":\"300001\",\"serverName\":\"双线1区\",\"serverDesc\":\"s1\",
            // \"roleId\":\"1\",\"roleName\":\"haha\",\"productId\":\"1\",\"productName\":\"asdf\",
            // \"productPrice\":\"1\",\"productCount\":\"1\",\"productDesc\":\"description\",\"change_rate\":\"0\",\"productyb\":\"10\"}";

            debug.Log("begin-pay");
            Variant v = new Variant();
            debug.Log("serverId:" + Globle.curServerD.sid);
            v["gpuid"] = Globle.YR_srvlists__platuid;
            v["serverId"] = Globle.curServerD.sid;
            v["serverIds"] = Globle.curServerD.sids;
            v["serverName"] = Globle.curServerD.server_name;
            v["serverDesc"] = Globle.curServerD.sid;
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["productId"] = data.payid;
            v["roleCreateTime"] = PlayerModel.getInstance().crttm;
            v["roleLevel"] = getlv(PlayerModel.getInstance().up_lvl, PlayerModel.getInstance().lvl);

            if (Application.platform == RuntimePlatform.Android)
            {
                v["productShopId"] = data.pay_android_id;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                v["productShopId"] = data.pay_ios_id;
            }

            debug.Log("rechargeData:" + RechargeModel.getInstance().getRechargeDataById(data.id));

            debug.Log("name:" + data.name);
            v["productName"] = data.name;
            v["productPrice"] = data.golden;
            v["productCount"] = 1;
            v["productDesc"] = data.desc1;
            v["change_rate"] = 0;
            v["productyb"] = data.golden_value;

            debug.Log("end-pay");

            string LanPayInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("pay", "lanPay", LanPayInfoJsonString);
        }

        public override void record_createRole(Variant data)
        {
            // "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"1\",\"roleGold\":\"300\",\"roleYb\":\"200\",
            // \"roleServerId\":\"1\",\"roleServerName\":\"servername\",\"rolevip\":\"1\",\"rolePartyName\":\"哈利噶多\",
            // \"rolePartyId\":\"123\",\"rolePower\":\"1234\",\"rolePartyRoleId\":\"1231\",\"rolePartyRoleName\":\"哈利波特\",
            // \"roleProfessionId\":\"12\",\"roleProfession\":\"无敌法师\",\"roleFriendlist\":\" \"}";

            Globle.YR_role_enter_time = Globle.getStrTimeNomal(NetClient.instance.CurServerTimeStamp);

            int cid = data["cid"];
            string name = data["name"];

            if(name.Length > 2)
                name = name.Remove(name.Length - 2);

            uint zhuan = data["zhua"];
            uint lvl = data["lvl"];
            int professionId = data["carr"];

            Variant v = new Variant();
            v["roleId"] = cid;
            v["roleName"] = name;
            v["roleLevel"] = getlv(zhuan,lvl);
            v["roleGold"] = 0;
            v["roleYb"] = 0;
            v["roleCreateTime"] = NetClient.instance.CurServerTimeStamp;
            //v["roleleveluptime"] = NetClient.instance.CurServerTimeStamp;
            v["roleServerId"] = Globle.curServerD.sid;
            v["roleServerName"] = Globle.curServerD.server_name;
            v["rolevip"] = 0;
            v["rolePartyName"] = "";
            v["rolePartyId"] = "";
            v["rolePower"] = 0;
            v["rolePartyRoleId"] = "";
            v["rolePartyRoleName"] = "";
            v["roleProfessionId"] = professionId;
            v["roleProfession"] = "";
            v["roleFriendlist"] = "";
            v["gpuid"] = Globle.YR_srvlists__platuid;
            v["roleEnterTime"] = Globle.YR_role_enter_time;
            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("createRole", "lanRole", serverInfoJsonString, false);

            debug.Log("[record]createRole:" + serverInfoJsonString);
        }

        public Dictionary<uint, uint> zhuan_lv = new Dictionary<uint, uint>();
        public string getlv(uint up_lv, uint lv)
        {
            uint true_lv;
            if (!zhuan_lv.ContainsKey(up_lv))
            {
                SXML s_xml = XMLMgr.instance.GetSXML("carrlvl.lvl_limit", "zhuanzheng==" + up_lv);
                if (s_xml != null)
                {
                    uint one_lv = s_xml.getUint("level_limit");
                    zhuan_lv[up_lv] = one_lv;
                }
            }
            true_lv = zhuan_lv[up_lv] + lv;

            return true_lv.ToString();
        }

        public override void record_login()
        {
            // "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"1\",\"roleGold\":\"300\",\"roleYb\":\"200\",
            // \"roleServerId\":\"1\",\"roleServerName\":\"servername\",\"rolevip\":\"1\",\"rolePartyName\":\"哈利噶多\",
            // \"rolePartyId\":\"123\",\"rolePower\":\"1234\",\"rolePartyRoleId\":\"1231\",\"rolePartyRoleName\":\"哈利波特\",
            //\"roleProfessionId\":\"12\",\"roleProfession\":\"无敌法师\",\"roleFriendlist\":\" \"}";

            Globle.YR_role_enter_time = Globle.getStrTimeNomal(NetClient.instance.CurServerTimeStamp);


            Variant v = new Variant();
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["roleLevel"] = getlv(PlayerModel.getInstance().up_lvl,PlayerModel.getInstance().lvl);
            v["roleGold"] = PlayerModel.getInstance().money;
            v["roleYb"] = PlayerModel.getInstance().gold;
            v["roleCreateTime"] = PlayerModel.getInstance().crttm;
            //v["roleleveluptime"] = PlayerModel.getInstance().crttm;
            v["roleServerId"] = Globle.curServerD.sid;
            v["roleServerName"] = Globle.curServerD.server_name;
            v["rolevip"] = PlayerModel.getInstance().vip;
            v["rolePartyName"] = "";
            v["rolePartyId"] = PlayerModel.getInstance().clanid;
            v["rolePower"] = PlayerModel.getInstance().combpt;
            v["rolePartyRoleId"] = "";
            v["rolePartyRoleName"] = "";
            v["roleProfessionId"] = PlayerModel.getInstance().profession;
            v["roleProfession"] = "";
            v["roleFriendlist"] = "";
            v["gpuid"] = Globle.YR_srvlists__platuid;
            v["roleEnterTime"] = Globle.YR_role_enter_time;
            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("enterGame", "lanRole", serverInfoJsonString, false);

            debug.Log("[record]login:" + serverInfoJsonString);

        }

        public override void record_LvlUp()
        {
            // "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"1\",\"roleGold\":\"300\",
            // \"roleleveluptime\":\"1464537600\",\"roleCreateTime\":\"1464537600\",\"rolevip\":\"0\",
            // \"roleYb\":\"200\",\"roleServerId\":\"1\",\"roleServerName\":\"servername\"}";

            Variant v = new Variant();
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["roleLevel"] = getlv(PlayerModel.getInstance().up_lvl, PlayerModel.getInstance().lvl);
            v["roleGold"] = PlayerModel.getInstance().money;
            v["roleYb"] = PlayerModel.getInstance().gold;
            v["roleCreateTime"] = PlayerModel.getInstance().crttm;
            //v["roleleveluptime"] = PlayerModel.getInstance().crttm;
            v["roleServerId"] = Globle.curServerD.sid;
            v["roleServerName"] = Globle.curServerD.server_name;
            v["rolevip"] = PlayerModel.getInstance().vip;
            v["rolePartyName"] = "";
            v["rolePartyId"] = PlayerModel.getInstance().clanid;
            v["rolePower"] = PlayerModel.getInstance().combpt;
            v["rolePartyRoleId"] = "";
            v["rolePartyRoleName"] = "";
            v["roleProfessionId"] = PlayerModel.getInstance().profession;
            v["roleProfession"] = "";
            v["roleFriendlist"] = "";
            v["gpuid"] = Globle.YR_srvlists__platuid;
            v["roleEnterTime"] = Globle.YR_role_enter_time;
            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("roleUpgrade", "lanRole", serverInfoJsonString, false);

            debug.Log("[record]LvlUp:" + serverInfoJsonString);
        }

        public override void record_quit()
        {
            // "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"1\",\"roleGold\":\"300\",
            // \"roleleveluptime\":\"1464537600\",\"roleCreateTime\":\"1464537600\",\"rolevip\":\"0\",
            // \"roleYb\":\"200\",\"roleServerId\":\"1\",\"roleServerName\":\"servername\"}";

            Variant v = new Variant();
            v["roleId"] = PlayerModel.getInstance().cid;
            v["roleName"] = PlayerModel.getInstance().name;
            v["roleLevel"] = getlv(PlayerModel.getInstance().up_lvl, PlayerModel.getInstance().lvl);
            v["roleGold"] = PlayerModel.getInstance().money;
            v["roleYb"] = PlayerModel.getInstance().gold;
            v["roleCreateTime"] = PlayerModel.getInstance().crttm;
            //v["roleleveluptime"] = PlayerModel.getInstance().crttm;
            v["roleServerId"] = Globle.curServerD.sid;
            v["roleServerName"] = Globle.curServerD.server_name;
            v["rolevip"] = PlayerModel.getInstance().vip;
            v["rolePartyName"] = "";
            v["rolePartyId"] = PlayerModel.getInstance().clanid;
            v["rolePower"] = PlayerModel.getInstance().combpt;
            v["rolePartyRoleId"] = "";
            v["rolePartyRoleName"] = "";
            v["roleProfessionId"] = PlayerModel.getInstance().profession;
            v["roleProfession"] = "";
            v["roleFriendlist"] = "";

            string serverInfoJsonString = JsonManager.VariantToString(v);
            AnyPlotformSDK.Call_Cmd("exitPage", "lanRole", serverInfoJsonString, false);

            debug.Log("[record]exitPage:" + serverInfoJsonString);
        }
    }
}
