using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;

namespace MuGame
{
    class PlayerInfoProxy : BaseProxy<PlayerInfoProxy>
    {
        public static uint EVENT_ADD_POINT = 0;
        public static uint EVENT_ON_EXP_CHANGE = 1;
        public static uint EVENT_ON_LV_CHANGE = 2;
        public static uint EVENT_SELF_ON_LV_CHANGE = 3;



        public static uint EVENT_ONGETPLAYERINFO = 211;
        public  bool look_player;

        public PlayerInfoProxy()
            : base()
        {
            addProxyListener(PKG_NAME.S2C_GETPLAYERINFO_FROMNAME, onGetPlayerInfo);

            #region Old
            addProxyListener(PKG_NAME.S2C_ATT_CHANGE, onAttChange);
            addProxyListener(PKG_NAME.S2C_SELF_ATT_CHANGE, onSelfAttchange);
            addProxyListener(PKG_NAME.S2C_DETAIL_INFO_CHANGE, onDetailInfoChange);
            addProxyListener(PKG_NAME.S2C_VIEW_AVATAR_CHANGE, onViewAvatar_Change);
            addProxyListener(PKG_NAME.S2C_SKEXP_CHANGE, onSkexpChange);
            addProxyListener(PKG_NAME.S2C_PLAYER_SHOW_INFO, onPlayerShowInfo);
            addProxyListener(PKG_NAME.S2C_PLAYER_DETAIL_INFO, onPlayerDetailInfo);
            addProxyListener(PKG_NAME.S2C_ADD_POINT, onPlayerAddPoint);
            addProxyListener(PKG_NAME.S2C_LVL_UP, onLvlUp);            
            addProxyListener(PKG_NAME.S2C_MODE_EXP, onModeExp);
            addProxyListener(PKG_NAME.S2C_Get_USER_CID_RES, onGetUserCidRes);
            //addProxyListener(PKG_NAME.S2C_QUERY_PLY_INFO_RES, onQueryPlyInfoRes);
            #endregion
        }

        public void SendGetPlayerFromName(string name)
        {
            look_player = false;
            Variant msg = new Variant();
            msg["name"] = name;
            sendRPC(PKG_NAME.S2C_GETPLAYERINFO_FROMNAME, msg);
        }

        void onGetPlayerInfo(Variant msgData)
        {
           
            if (msgData.ContainsKey("res"))
            {
                if (msgData["res"] < 0)
                {
                    Globle.err_output(msgData["res"]);
                }             
            }
            if (msgData.ContainsKey("cid"))
            {
                if (a3_legion_member.instance.IsInvite)
                {
                    a3_legion_member.instance.inviteNum = msgData["cid"];
                    a3_legion_member.instance.InviteBtn();
                }

                look_player = true;
            }
            dispatchEvent(GameEvent.Create(EVENT_ONGETPLAYERINFO, this, msgData));
        }

        #region Old

        public void sendLoadPlayerDetailInfo(uint cid)
        {
            //请求这个角色的详细数据
            Variant msg = new Variant();
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_PLAYER_DETAIL_INFO, msg);
        }

        public void sendAddPoint(int type, Dictionary<int, int> add_pt)
        {
            Variant msg = new Variant();
            msg["op"] = type;
            if (type == 0)
            {
                foreach (int key in add_pt.Keys)
                {
                    switch (key)
                    {
                        case 1:
                            msg["strpt"] = add_pt[key];
                            break;
                        case 2:
                            msg["intept"] = add_pt[key];
                            break;
                        case 3:
                            msg["agipt"] = add_pt[key];
                            break;
                        case 4:
                            msg["conpt"] = add_pt[key];
                            break;
                        case 5:
                            msg["wispt"] = add_pt[key];
                            break;
                    }
                }
            }

            if (type == 1)
            {

            }

            sendRPC(148, msg);
        }

        public void sendAttChange()
        {
            Variant msg = new Variant();
            sendRPC(26, msg);
        }
        public void sendSelfAttchange()
        {
            Variant msg = new Variant();
            sendRPC(32, msg);
        }
        public void sendDetailInfoChange()
        {
            Variant msg = new Variant();
            sendRPC(40, msg);
        }
        public void sendSkexpChange()
        {
            Variant msg = new Variant();
            sendRPC(41, msg);
        }
        public void sendPlayerShowInfo(Variant cids)
        {
            Variant msg = new Variant();
            msg["cidary"] = cids;
            sendRPC(51, msg);
        }
        public void sendPlayerDetailInfo(uint cid)
        {
            Variant msg = new Variant();
            msg["cid"] = cid;
            sendRPC(52, msg);
        }
        public void sendlvl_up()
        {
            Variant msg = new Variant();
            sendRPC(60, msg);
        }
        public void sendmod_exp()
        {
            Variant msg = new Variant();
            sendRPC(61, msg);
        }
        public void sendon_get_user_cid_res(string name, bool ol, uint func)
        {
            Variant msg = new Variant();
            msg["ol"] = ol;
            msg["name"] = name;
            msg["func"] = func;
            sendRPC(251, msg);
        }
        public void sendon_query_ply_info_res(Variant data)
        {
            sendRPC(253, data);
        }

        void onAttChange(Variant msgData)
        {
            RoleMgr._instance.onAttchange(msgData);
        }

        void onPlayerAddPoint(Variant msgData)
        {
            int res = msgData["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }

            dispatchEvent(GameEvent.Create(EVENT_ADD_POINT, this, msgData));
        }

        void onSelfAttchange(Variant msgData)
        {
            debug.Log("属性变换" + msgData.dump());
            if (msgData.ContainsKey("mpleft"))
            {
                PlayerModel.getInstance().modMp(msgData["mpleft"]);
            }
            if (msgData.ContainsKey("hp"))
            {
                PlayerModel.getInstance().modHp (msgData["hp"]);
            }

            if (msgData.ContainsKey ("vipcard_life")) {
                if (msgData["vipcard_life"] == 1) { A3_signProxy.getInstance().yueka = 2; }
                else {
                    if (msgData.ContainsKey ("vipcard_month_end_time")) {
                        if (msgData["vipcard_month_end_time"] <= NetClient.instance.CurServerTimeStamp)
                        {
                            A3_signProxy.getInstance().yueka = 0;
                        }
                        else {
                            A3_signProxy.getInstance().yueka = 1;
                        }
                    }
                }
                if (a3_Recharge .isshow) {
                    a3_Recharge.isshow.refre_recharge();
                }
                A3_signProxy.getInstance().sendproxy(1,0);
            }

            if (msgData.ContainsKey ("first_double")) {
                foreach ( int info in msgData["first_double"]._arr )
                {
                    if (RechargeModel .getInstance ().rechargeMenu.ContainsKey (info)) {

 
                        if (RechargeModel.getInstance().rechargeMenu[info].first_double >= 1)
                        {
                            if (!RechargeModel.getInstance().firsted.Contains(info)) { RechargeModel.getInstance().firsted.Add(info); }
                        }
                        if (a3_Recharge.isshow)
                        {
                            a3_Recharge.isshow.refre_recharge();
                        }
                    }
                }
            }

            PlayerModel.getInstance().attrChangeCheck(msgData);
            PlayerModel.getInstance().attPointCheck(msgData);
            if(msgData.ContainsKey("max_hp"))
            {
                InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modHp", "model/PlayerModel", PlayerModel.getInstance().hp, msgData["max_hp"]._int);
            }
            if(msgData.ContainsKey("max_mp"))
            {
                InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modMp", "model/PlayerModel", PlayerModel.getInstance().mp, msgData["max_mp"]._int);
            }
        }

        void onViewAvatar_Change(Variant v)
        {
            //debug.Log("有人在我旁边换装了！！！~~~~~~~~~~~~~~~~~~~~~~~~");
            //debug.Log("有人在我旁边换装了！！！~~~~~~~~~~~~~~~~~~~~~~~~");
            //debug.Log("有人在我旁边换装了！！！~~~~~~~~~~~~~~~~~~~~~~~~");

            debug.Log("周围人状态变化："+v.dump ());
            uint iid = v["iid"];
            if (v.ContainsKey("serial_kp"))
            {
                if (v["iid"] == PlayerModel.getInstance().iid)
                {
                    if (SelfRole._inst != null)
                    {
                        PlayerModel.getInstance().serial = v["serial_kp"];
                        SelfRole._inst.serial = v["serial_kp"];
                        PlayerNameUIMgr.getInstance().refreserialCount(SelfRole._inst, v["serial_kp"]);
                    }
                }
            }
            if (v.ContainsKey("strike_back_tm"))
            {
                //反击buff时间戳玩家自己
                if (v["iid"] == PlayerModel.getInstance().iid)
                {
                    if (v.ContainsKey("strike_back_tm"))
                    {
                        if (SelfRole._inst != null)
                        {
                            SelfRole._inst.hidbacktime = v["strike_back_tm"];
                            if (v["strike_back_tm"] == 0)
                            {
                                PlayerModel.getInstance().hitBack = 0;
                                PlayerNameUIMgr.getInstance().refresHitback(SelfRole._inst, 0, true);
                            }
                            else
                            {
                                PlayerModel.getInstance().hitBack = SelfRole._inst.hidbacktime - (uint)NetClient.instance.CurServerTimeStamp;
                                PlayerNameUIMgr.getInstance().refresHitback(SelfRole._inst, (int)(SelfRole._inst.hidbacktime - (uint)NetClient.instance.CurServerTimeStamp), true);
                            }
                            //debug.Log("时间1：" + SelfRole._inst.hidbacktime + "时间2：" + (uint)NetClient.instance.CurServerTimeStamp);
                            //debug.Log("玩家自己受到时间：" + (int)(SelfRole._inst.hidbacktime - (uint)NetClient.instance.CurServerTimeStamp));
                        }

                    }
                }
                else
                {
                    OtherPlayerMgr._inst.refreshPlayerInfo(v);
                }
            }
            else
            {
                OtherPlayerMgr._inst.refreshPlayerInfo( v ,false);
            }


            if ( v.ContainsKey( "title_sign" ) )
            {
                if ( v[ "iid" ] != PlayerModel.getInstance().iid )
                {
                    int title_sign = v["title_sign"]._int;
                    uint cid = v[ "cid" ]._uint;
                    bool ishow = false;
                    if ( v.ContainsKey( "title_sign_display" ) )
                    {
                        ishow = v[ "title_sign_display" ];
                    }
                            
                    PlayerNameUIMgr.getInstance().SetOtherTitle( cid, title_sign , ishow );
                }
            }

            //人物修改名字

            if (v.ContainsKey("name"))
            {
                if (v["iid"] == PlayerModel.getInstance().iid)
                {
                    PlayerModel.getInstance().name = v["name"];

                    if (SelfRole._inst != null)
                    {
                        SelfRole._inst.roleName = v["name"];

                        PlayerNameUIMgr.getInstance().refresName(SelfRole._inst);
						
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHANGE_NAME);
                    }
                }
                else {

                    ProfessionRole otherRole =  OtherPlayerMgr._inst.GetOtherPlayer(v["iid"]);

                    otherRole.roleName= v["name"];

                    PlayerNameUIMgr.getInstance().refresName(otherRole);


                }

            }

        }


        void onDetailInfoChange(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //        GameEvent.Create(PKG_NAME.S2C_DETAIL_INFO_CHANGE, this, msgData)
            //    );

        }

        void onSkexpChange(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                      GameEvent.Create(PKG_NAME.S2C_SKEXP_CHANGE, this, msgData)
                  );
        }


        void onPlayerShowInfo(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_PLAYER_SHOW_INFO, this, msgData)
             );
        }

        void onPlayerDetailInfo(Variant msgData)
        {
            debug.Log("onPlayerDetailInfo" + msgData.dump());
            Variant data = msgData["pinfo"];
            if (msgData["res"]._int == 1)
                OtherPlayerMgr._inst.refreshPlayerInfo(msgData["pinfo"]);
            if (!data?.ContainsKey("pet_food_last_time") ?? true)
                return;
            if (data["pet_food_last_time"] == 0)
            {
                debug.Log("对方玩家宠物没有饲料");
                OtherPlayerMgr._inst.PlayPetAvatarChange(data["iid"], 0, 0);//p["pet"]["id"]
            }
            else
                OtherPlayerMgr._inst.PlayPetAvatarChange(data["iid"], data["pet"]["id"], 0);


        }

        void onLvlUp(Variant msgData)
        {
            if (a3_liteMinimap.instance!=null)
            a3_liteMinimap.instance.function_open(a3_liteMinimap.instance.fun_i);
            //NetClient.instance.dispatchEvent(
            //    GameEvent.Create(GAME_EVENT.ON_LVL_UP, this, msgData)
            //);
            //if (msgData.ContainsKey("pinfo"))
            //{
            //    NetClient.instance.dispatchEvent(
            //        GameEvent.Create(GAME_EVENT.R_PLAYER_INFO_CHANGED, this, null)
            //    );
            //    NetClient.instance.dispatchEvent(
            //        GameEvent.Create(GAME_EVENT.PLAYER_INFO_CHANGED, this, null)
            //    );
            //}

            //Variant plyInfo = new Variant();

            //NetClient.instance.dispatchEvent(
            //        GameEvent.Create(GAME_EVENT.MODIFY_TEAMMATE_DATA, this, msgData["lvlshare"])
            //    );
            debug.Log("收到升级或者转生的协议........." + msgData.dump());
            if (msgData.ContainsKey("cid"))
            {
                if (msgData["cid"] != PlayerModel.getInstance().cid)
                {
                    if (OtherPlayerMgr._inst != null && OtherPlayerMgr._inst.m_mapOtherPlayer.Count > 0)
                    {
                        if (OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(msgData["iid"]))
                        {
                            OtherPlayerMgr._inst.m_mapOtherPlayer[msgData["iid"]].zhuan = msgData["zhuan"];
                        }

                    }
                    //点击的角色在你身边升到1转以上了
                    if (SelfRole._inst != null && SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.m_unIID == msgData["iid"])
                    {
                        PkmodelAdmin.RefreshShow(SelfRole._inst.m_LockRole, true);
                    }
                    //   // (SelfRole._inst.m_LockRole as ProfessionRole).lvl = msgData["lvl"];
                    //    //(SelfRole._inst.m_LockRole as ProfessionRole).zhuan= msgData["zhuan"];                    
                }
                else
                {
                    PlayerModel.getInstance().lvUp(msgData);
                    dispatchEvent(GameEvent.Create(EVENT_SELF_ON_LV_CHANGE, this, msgData));
                    if(msgData.ContainsKey("pinfo"))
                        InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modExp", "model/PlayerModel", (uint)msgData["pinfo"]["exp"]);
                    if (msgData.ContainsKey("mod_exp"))
                    {
                        flytxt.instance.fly("EXP+" + msgData["mod_exp"], 3);
                    }

                }
            }            
            if (a3_QHmaster.instance != null)
            {
                a3_QHmaster.instance.refreshDashi();
            }
            dispatchEvent(GameEvent.Create(EVENT_ON_LV_CHANGE, this, msgData));
            ResetLvLProxy.getInstance().resetLvL();
        }

        void onGetUserCidRes(Variant msgData)
        {
            //(NetClient.instance.g_gameM as muLGClient).g_MgrPlayerInfoCT.on_get_user_cid_res(msgData);
        }
        void onModeExp(Variant msgData)
        {
            PlayerModel.getInstance().exp = PlayerModel.getInstance().exp + msgData["mod_exp"];

            var maxExp = PlayerModel.getInstance().GetCurrMaxExp();

            if ( maxExp != 0u )
            {
                PlayerModel.getInstance().exp = PlayerModel.getInstance().exp >= maxExp ? maxExp : PlayerModel.getInstance().exp;
            }

            debug.Log("经验增加："+ msgData["mod_exp"]);
            if(flytxt.instance)
                flytxt.instance.fly("EXP+" + msgData["mod_exp"], 3);
            if (a3_insideui_fb.instance != null) a3_insideui_fb.instance.SetInfExp(msgData["mod_exp"]);
            if (GameRoomMgr.getInstance().curRoom != null) GameRoomMgr.getInstance().curRoom.onAddExp(msgData["mod_exp"]);


            if (msgData.ContainsKey("cur_exp"))
            {
                PlayerModel.getInstance().exp = msgData["cur_exp"];
            }
            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modExp", "model/PlayerModel", PlayerModel.getInstance().exp);
            dispatchEvent(GameEvent.Create(EVENT_ON_EXP_CHANGE, this, msgData));
        }

        //void onQueryPlyInfoRes(Variant msgData)
        //{
        //    NetClient.instance.dispatchEvent(
        //       GameEvent.Create(PKG_NAME.S2C_QUERY_PLY_INFO_RES, this, msgData)
        //   );
        // ( NetClient.instance.g_gameM as muLGClient).g_MgrPlayerInfoCT.on_query_ply_info_res(msgData);
        //}
        #endregion


    }
}
