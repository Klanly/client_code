using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    class A3_cityOfWarProxy : BaseProxy<A3_cityOfWarProxy>
    {
        public static uint REFRESHINFO = 1;
        public static uint REFRESHAPPLY = 2;
        public static uint REFRESHPREPARE = 3;
        public static uint REFRESHFBINFO = 4;
        public A3_cityOfWarProxy()
        {
            addProxyListener(PKG_NAME.S2C_CLAN_CITYWAR, onInfo);


        }


        //1请求攻城战数据 
        //2进入攻城战
        //3提升防御
        //4 投标出价
        public void sendProxy(int op) {
            Variant msg = new Variant();
            msg["op"] = op;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void sendApply(uint count) {
            Variant msg = new Variant();
            msg["op"] = 4;
            msg["apply_num"] = count;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void sendPrepare(uint tp) {
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["type"] = tp;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void sendInfb() {
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void sendGetFb_info() {
            Variant msg = new Variant();
            msg["op"] = 7;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void sendPos_info()
        {
            Variant msg = new Variant();
            msg["op"] = 8;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }

        public void send_signal(uint tp,int x , int y)
        {
            Variant msg = new Variant();
            msg["op"] = 9;
            msg["type"] = tp;
            msg["x"] = x;
            msg["y"] = y;
            sendRPC(PKG_NAME.S2C_CLAN_CITYWAR, msg);
            debug.Log("City_send" + msg.dump());
        }


        public Dictionary<int , PlayerPos_cityWar> list_position = new Dictionary<int, PlayerPos_cityWar>();
        void onInfo(Variant data) {
            int res = data["res"];
            debug.Log("City" + data.dump());
            if (res <0) {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                case 1:

                    debug.Log("GGGG"+ A3_LegionModel.getInstance ().myLegion.id );
                    //请求攻城战数据
                    A3_cityOfWarModel.getInstance().last_type = data["last_type"];
                    A3_cityOfWarModel.getInstance().llid = data["llid"];
                    A3_cityOfWarModel.getInstance().def_clanid = data["def_clanid"];
                    A3_cityOfWarModel.getInstance().start_tm = data["start_tm"];
                    A3_cityOfWarModel.getInstance().clan_pcid = data["clan_pcid"];
                    A3_cityOfWarModel.getInstance().clan_lvl = data["clan_lvl"];

                    A3_cityOfWarModel.getInstance().clan_name = data["clan_name"];
                    A3_cityOfWarModel.getInstance().Castellan_name = data["name"];
                    A3_cityOfWarModel.getInstance().Castellan_zhuan = data["zhuan"];
                    A3_cityOfWarModel.getInstance().Castellan_lvl = data["lvl"];
                    A3_cityOfWarModel.getInstance().Castellan_combpt = data["combpt"];
                    A3_cityOfWarModel.getInstance().Castellan_carr = data["carr"];

                    if (data.ContainsKey ("def_info")) {
                        A3_cityOfWarModel.getInstance().deflist.Clear();
                        List<Variant> l = data["def_info"]._arr;
                        foreach (var v in l)
                        {
                            defInfo temp = new defInfo();
                            temp._type = v["type"];
                            temp._lvl = v["level"];
                            A3_cityOfWarModel.getInstance().deflist[v["type"]] = temp;
                        }
                    }

                    if (data.ContainsKey ("apply_list")) {
                        A3_cityOfWarModel .getInstance ().apply_list.Clear();
                        List<Variant> l = data["apply_list"]._arr;
                        List<Apply_Info> ApplyList = new List<Apply_Info>();
                        foreach (var v in l)
                        {
                            Apply_Info temp = new Apply_Info();
                            temp.clan_id = v["clan_id"];
                            temp.clan_name = v["clan_name"];
                            temp.apply_num  = v["apply_num"];
                            temp.apply_tm = v["apply_tm"];
                            temp.clan_lvl = v["clan_lvl"];
                            ApplyList.Add(temp);
                        }
                        ApplyList.Sort();
                        A3_cityOfWarModel.getInstance().apply_list = ApplyList;
                    }
                    dispatchEvent(GameEvent.Create(REFRESHINFO, this, data));
                    // A3_cityOfWarModel.getInstance().gg();
                    break;
                case 2:
                    //进入攻城战
                    break;
                case 3:
                    //提升防御
                    if (A3_cityOfWarModel.getInstance().deflist.ContainsKey(data["type"]))
                    {
                        A3_cityOfWarModel.getInstance().deflist[data["type"]]._lvl = data["level"];
                    }
                    else {
                        defInfo temp = new defInfo();
                        temp._type = data["type"];
                        temp._lvl = data["level"];
                        A3_cityOfWarModel.getInstance().deflist[data["type"]] = temp;
                    }
                    dispatchEvent(GameEvent.Create(REFRESHPREPARE, this, data));
                    break;
                case 4:
                    //投标出价
                    if (data.ContainsKey("apply_list"))
                    {
                        A3_cityOfWarModel.getInstance().apply_list.Clear();
                        List<Variant> l = data["apply_list"]._arr;
                        List<Apply_Info> ApplyList = new List<Apply_Info>();
                        foreach (var v in l)
                        {
                            Apply_Info temp = new Apply_Info();
                            temp.clan_id = v["clan_id"];
                            temp.clan_name = v["clan_name"];
                            temp.apply_num = v["apply_num"];
                            temp.apply_tm = v["apply_tm"];
                            temp.clan_lvl = v["clan_lvl"];
                            ApplyList.Add(temp);
                        }
                        ApplyList.Sort();
                        A3_cityOfWarModel.getInstance().apply_list = ApplyList;
                        dispatchEvent(GameEvent.Create(REFRESHAPPLY, this, data));
                    }
                    break;
                case 5:
                    //攻城战统计
                    break;
                case 6:
                    // 地图成员信息
                    break;
                case 8:
                    list_position.Clear();
                    List<Variant> ll = data["data"]._arr;
                    foreach (var v in ll)
                    {
                        PlayerPos_cityWar temp = new PlayerPos_cityWar();
                        temp.lvlsideid = v["lvlsideid"];
                        temp.iid = v["iid"];
                        temp.x = (uint)((v["x"]) / GameConstant.PIXEL_TRANS_UNITYPOS);
                        temp.y = (uint)((v["y"]) / GameConstant.PIXEL_TRANS_UNITYPOS);
                        list_position[v["iid"]] = temp;
                    }
                    break;
                case 9:
                    if (data["lvlsideid"] == PlayerModel .getInstance ().lvlsideid ) {
                        signalInfo info = new signalInfo();
                        info.signalType = data["type"];
                        info.x =(int) (data["x"]);
                        info. y= (int)(data["y"]);
                        info.cd = 5;
                        A3_cityOfWarModel.getInstance().signalList.Add(info);
                        a3_insideui_fb.instance.changesignal(info.signalType);
                        if (a3_liteMiniBaseMap2 .instance) {
                            a3_liteMiniBaseMap2.instance.SetSignal ();
                        }
                    }
                    break;
                case 20:
                    //怪物血量和人数
                    A3_cityOfWarModel.getInstance().atk_num = data["atk_num"];
                    A3_cityOfWarModel.getInstance().def_num = data["def_num"];
                    if (data["door_open"] == 1) {
                        A3_cityOfWarModel.getInstance().door_open = true ;
                    }else
                        A3_cityOfWarModel.getInstance().door_open = false;
                    if (data.ContainsKey ("mon_hpper")) {
                        List< Variant > l = data["mon_hpper"]._arr;
                        A3_cityOfWarModel.getInstance().SetMonInfo(l);
                    }
                    dispatchEvent(GameEvent.Create(REFRESHFBINFO, this, data));
                    break;
                case 21:
                    //击杀通知
                    break;
                case 22:
                    //助攻通知
                    break;
            }
        }
    }
}
