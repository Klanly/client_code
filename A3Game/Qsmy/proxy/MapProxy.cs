using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class MapProxy : BaseProxy<MapProxy>
    {
        public static bool isyinsh = false;//是否隐身
        public List<int> drop = new List<int>();
        public string openWin = null;
        public string Win_uiData = null;
        public bool nul;
        public MapProxy()
            : base()
        {
            //   addProxyListener(PKG_NAME.S2C_ADD_NPCS, on_add_npcs);
            addProxyListener(PKG_NAME.S2C_TRIG_EFF, on_trig_eff);
            addProxyListener(PKG_NAME.S2C_MONSTER_SPAWN, on_monster_spawn);
            addProxyListener(PKG_NAME.S2C_PLAYER_RESPAWN, on_player_respawn);
            addProxyListener(PKG_NAME.S2C_SPRITE_INVISIBLE, on_sprite_invisible);
            addProxyListener(PKG_NAME.S2C_ON_SPRITE_HP_INFO_RES, on_sprite_hp_info_res);


            addProxyListener(PKG_NAME.S2C_PLAYER_ENTER_ZONE, on_player_enter_zone);
            addProxyListener(PKG_NAME.S2C_MONSTER_ENTER_ZONE, on_monster_enter_zone);
            addProxyListener(PKG_NAME.S2C_SPRITE_LEAVE_ZONE, on_sprite_leave_zone);
            addProxyListener(PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, on_begin_change_map_res);

            addProxyListener(PKG_NAME.S2C_MAP_CHANGE, onMapChange);
            addProxyListener(PKG_NAME.S2C_ON_OTHER_EQP_CHANGE, on_other_eqp_change);

            addProxyListener(PKG_NAME.S2C_PICK_DPITEM_RES, on_pickitem);

            addProxyListener(PKG_NAME.S2C_ITEM_DROPED, on_map_dpitem_res);

            addProxyListener(PKG_NAME.S2C_BEGIN_COLLECT, onbeginCollect);
            addProxyListener(PKG_NAME.S2C_A3_ACTIVE_GETCHEST, Collect_Box);

        }

        private void Collect_Box(Variant data)
        {
           
           // Debug.LogError(data.dump());
            if (data == null)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.CD);
                return;
            }
            int res = data["res"];
            if (res < 0)
            {
                flytxt.instance.fly(ContMgr.getError(res.ToString()));
            }
            switch (res)
            {
                case 2:
                    // CollectRole.instance.becollected = false;                  
                    //CollectBox.instance.m_curAni.SetBool("open", false);
                    InterfaceMgr.getInstance().close(InterfaceMgr.CD);
                    flytxt.instance.fly(ContMgr.getCont("MapProxy_openbad"));
                    break;
                case 1:
                    nul = false;
                    string str= "";
                    var info = data["itm_ary"]._arr;
                    //flytxt.instance.fly(ContMgr.getCont("MapProxy_openok"));
                    for (int i=0;i<info.Count;i++)
                    {
                        var p = info[i];
                       
                        if (p.ContainsKey("id") && p.ContainsKey("cnt"))
                        {
                            if (p["cnt"] != 0)
                            {
                                nul = true;
                                a3_ItemData dta = new a3_ItemData();
                                dta = a3_BagModel.getInstance().getItemDataById(p["id"]);
                                string color = "";
                                switch (dta.quality)
                                {
                                    case 1: color =/* "<color=#ffffff>" + */dta.item_name/* + "</color>"*/; break;
                                    case 2: color = /*"<color=#00FF00>" +*/ dta.item_name /*+ "</color>"*/; break;
                                    case 3: color = /*"<color=#66FFFF>" +*/ dta.item_name /*+ "</color>"*/; break;
                                    case 4: color = /*"<color=#FF00FF>" +*/ dta.item_name /*+ "</color>"*/; break;
                                    case 5: color =/* "<color=#FF7F0A>" + */dta.item_name /*+ "</color>"*/; break;
                                }
                              
                                str = ContMgr.getCont("BagProxy_geteitem") + color + "x" + p["cnt"];
                                flytxt.instance.fly(str);
                            }
                        }
                    }
                        if (nul == false)
                      //  {
                      //      flytxt.instance.fly(str,2);
                      //  }
                      //else
                        {
                            flytxt.instance.fly(ContMgr.getCont("MapProxy_null"));
                        }

                   
                    break;
                default:
                    InterfaceMgr.getInstance().close(InterfaceMgr.CD);
                    break;
                    //case 3:
                    //    break
            }

        }

        public bool changingMap = false;
        public void sendBeginChangeMap(int linkid, bool transmit = false, bool ontask = false, int lineid = -1)
        {
            if (FindBestoModel.getInstance().Canfly)//宝图活动战斗状态禁止传送
            {
                Variant v = new Variant();

                v["gto"] = linkid;
                v["transmit"] = transmit;
                v["ontask"] = ontask;
                if (lineid != -1)
                    v["line"] = lineid;
                debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>sendBeginChangeMap>" + v.dump());
                changingMap = true;
                sendRPC(PKG_NAME.C2S_ON_BEGIN_CHANGE_MAP_RES, v);
            }
            else
            {
                flytxt.instance.fly(FindBestoModel.getInstance().nofly_txt);
            }
        }

        public bool change_map = false;
        public void on_begin_change_map_res(Variant v)
        {

            debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>on_begin_change_map_res>" + v.dump());

            if (v["res"] == 1)
            {
                Variant var = new Variant();
                var["gto"] = v["gto"];
                var["gate"] = v["gate"];
                if (v.ContainsKey("line"))
                    var["line"] = v["line"];
                if (loading_cloud.instance == null)
                {
                    loading_cloud.showhandle = () =>
                    {
                        sendRPC(PKG_NAME.C2S_ONMAPCHANGE, var);
                    };
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.LOADING_CLOUD);
                }
                else
                {
                    sendRPC(PKG_NAME.C2S_ONMAPCHANGE, var);
                }
                change_map = true;

            } 
            else
            {
                InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.LOADING_CLOUD);
                changingMap = false;
                SelfRole.fsm.Stop();
                Globle.err_output(v["res"]);
            }
      }

        public void sendGoto_map(Variant data)
        {
            sendRPC(12, data);
        }


        public void sendShowMapObj()
        {
            debug.Log("!!sendShowMapObj!!" + " " + debug.count);
            sendRPC(PKG_NAME.C2S_ON_SHOW_MAP_OBJ, new Variant());
        }

        public void sendNpcTrans(uint npcid, uint trid)
        {
            Variant data = new Variant();
            data["npcid"] = npcid;
            data["trid"] = trid;
            sendRPC(19, data);
        }

        public void send_get_wrdboss_respawntm(bool islvl = false)
        {	//获取boss复活时间列表
            Variant data = new Variant();
            data["islvl"] = islvl;
            sendRPC(PKG_NAME.C2S_MONSTER_SPAWN, data);
        }


        public void sendRespawn(bool useGolden = false)
        {
            Variant data = new Variant();
            data["immediate"] = useGolden;
            sendRPC(PKG_NAME.C2S_PLAYER_RESPAWN, data);
        }

        public void sendGet_sprite_hp_info(uint iid)
        {
            Variant data = new Variant();
            data["iid"] = iid;
            sendRPC(PKG_NAME.C2S_ON_SPRITE_HP_INFO_RES, data);
        }

        public void sendChange_map(uint mapid)
        {
            Variant data = new Variant();
            data["gto"] = mapid;
            sendRPC(57, data);
        }
        public void sendEnd_change_map()
        {
            sendRPC(58, new Variant());
        }


        //private void on_add_npcs(Variant msgData)
        //{
        //    NetClient.instance.dispatchEvent(
        //           GameEvent.Create(PKG_NAME.S2C_ADD_NPCS, this, msgData)
        //       );
        //}
        private void on_trig_eff(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                  GameEvent.Create(PKG_NAME.S2C_TRIG_EFF, this, msgData)
              );

        }

        private void on_monster_spawn(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                  GameEvent.Create(PKG_NAME.S2C_MONSTER_SPAWN, this, msgData)
              );
        }

        private void on_player_respawn(Variant msgData)
        {
            debug.Log("PPPPP"+ msgData.dump ());
            //RoleMgr._instance.onRespawn(msgData);
            if (msgData.ContainsKey("back_town_tm"))
            {
                uint ms = msgData["back_town_tm"];
                long sec = ms - NetClient.instance.CurServerTimeStamp;
                if (sec < 0)
                    sec = 0;
                //int sec = (int)Math.Ceiling((float)ms / 1000);
                a3_relive.backtown_end_tm = (int)sec;
            }

            if (!msgData.ContainsKey("iid")) return;
            int max_hp = msgData["battleAttrs"]["max_hp"];
            uint iid = msgData["iid"]._uint;
            if (iid == SelfRole._inst.m_unIID)
            {
                SelfRole._inst.can_buff_move = true;
                SelfRole._inst.can_buff_skill = true;
                SelfRole._inst.can_buff_ani = true;
                SelfRole._inst.onRelive(max_hp);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RELIVE);
            }
            else
            {
                ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(iid);
                if (pr != null)
                {       
                    //播放伤害
                    pr.onRelive(max_hp);
                }
            }

            if (msgData.ContainsKey("x") && msgData.ContainsKey("y"))
            {
                float x = msgData["x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                float y = msgData["y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                Vector3 pos = new Vector3(x, 0f, y);
                if (iid == SelfRole._inst.m_unIID)
                {
                    SelfRole._inst.setPos(pos);
                }
                else {
                    ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(iid);
                    if (pr != null) {
                        pr.setPos(pos);
                    }
                }
            }
        }

        private void on_sprite_invisible(Variant msgData)
        {
            debug.Log("on_sprite_invisible::" + msgData.dump());

            uint iid = msgData["iid"]._uint;
            //uint m_unCID = 0;
            bool invisible = msgData["invisible"] > 0;
            if (iid == SelfRole._inst.m_unIID)
            {
                SelfRole._inst.invisible = invisible;
                if (a1_gamejoy.inst_skillbar != null)
                {
                    a1_gamejoy.inst_skillbar.forSkill_5008(invisible);
                }

                isyinsh = invisible;
                SelfRole._inst.refreshmapCount((int)PlayerModel.getInstance().treasure_num);
                SelfRole._inst.refreshVipLvl((uint)A3_VipModel.getInstance().Level);
                //m_unCID  = PlayerModel.getInstance().cid;

                if ( A3_SummonModel.getInstance().nowShowAttackID != 0 && invisible )
                {
                    A3_SummonProxy.getInstance().sendZhaohui();
                }
                else if ( A3_SummonModel.getInstance().nowShowAttackID != 0 && A3_SummonModel.getInstance().lastSummonID != 0 &&  invisible  == false) { 

                    if ( A3_SummonModel.getInstance().getSumCds().ContainsKey( ( int ) A3_SummonModel.getInstance().lastSummonID ) )
                    {
                        flytxt.instance.fly( ContMgr.getCont( "a3_summon10" ) );

                    }
                    else
                        A3_SummonProxy.getInstance().sendChuzhan( A3_SummonModel.getInstance().lastSummonID );

                } //隐身状态下 把召唤兽 收回
                
            }
            else
            {
                ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(iid);

                if (pr != null)
                {
                    pr.invisible = invisible;

                    if (invisible && SelfRole._inst.m_LockRole == pr)
                        SelfRole._inst.m_LockRole = null;

                    if ( pr.invisible == false &&  pr.dianjiTime == -1 && pr.isUp == false )
                    {
                        pr.ChangeRideState( true );
                    }
                }

                //m_unCID  = pr.m_unCID;

               
            }

            //uint monsterIID = 0;

            //if ( MonsterMgr._inst.roleSummonMapping.ContainsKey( m_unCID ) )
            //{
            //     monsterIID =  MonsterMgr._inst.roleSummonMapping[ m_unCID ]; // 召唤兽iid
            //}
            

            //if ( monsterIID != null && MonsterMgr._inst.m_mapMonster.ContainsKey( monsterIID ) )
            //{
            //    var monster =  MonsterMgr._inst.m_mapMonster[ monsterIID ];

            //    if ( monster is MS0000 )
            //    {
            //        ( monster as MS0000 ).invisibleState = invisible ;
            //    }

            //}
        }

        private void on_sprite_hp_info_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                  GameEvent.Create(PKG_NAME.S2C_ON_SPRITE_HP_INFO_RES, this, msgData)
              );

        }


        private void on_player_enter_zone(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //        GameEvent.Create(PKG_NAME.S2C_PLAYER_ENTER_ZONE, this, msgData)
            //    );

            //玩家进入视野
            debug.Log("玩家进入视野 " + msgData.dump());
            Variant info = msgData;
            foreach (Variant p in info["pary"]._arr)
            {
                OtherPlayerMgr._inst.AddOtherPlayer(p);
                //if (!p.ContainsKey("pet_food_last_time"))
                //    return;
                //if (p["pet_food_last_time"] == 0)
                //{
                //    debug.Log("对方玩家宠物没有饲料");
                //    OtherPlayerMgr._inst.PlayPetAvatarChange(p["iid"], 0, 0);//p["pet"]["id"]
                //}
            }
        }

        private void on_monster_enter_zone(Variant msgData)
        {
            debug.Log("++++++++++++++++++++++++++monster+" + msgData.dump());

            foreach (Variant m in msgData["monsters"]._arr)
            {
                if (m.ContainsKey("carr"))
                {
                    //OtherPlayerMgr._inst.AddOtherPlayer(m);
                    MonsterMgr._inst.AddMonster_PVP(m);
                }
                else if (m.ContainsKey("owner_cid"))
                {
                    MonsterMgr._inst.AddSummon(m);
                }
                else
                {
                    MonsterRole role;
                    if (m.ContainsKey("owner_name"))
                    {
                        string ownerName = m["owner_name"];
                        if (TeamProxy.getInstance().MyTeamData == null && !PlayerModel.getInstance().name.Equals(ownerName)) { }
                        //role = MonsterMgr._inst.AddMonster(m, invisible: false);
                        else if (TeamProxy.getInstance().MyTeamData != null && !TeamProxy.getInstance().MyTeamData.IsInMyTeam(ownerName)) { }
                        //role = MonsterMgr._inst.AddMonster(m, invisible: false);
                        else
                            role = MonsterMgr._inst.AddMonster(m/*, ownerName == PlayerModel.getInstance().name*/);
                    }
                    else
                        role = MonsterMgr._inst.AddMonster(m);
                }
                if (m.ContainsKey("escort_name"))
                {
                    if (PlayerModel.getInstance().up_lvl>=1)
                    {
                        MonsterMgr._inst.AddDartCar(m);
                    }
                    else
                        MonsterMgr._inst.RemoveMonster(m["iid"]);
                }                
            }
        }

        private void on_sprite_leave_zone(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //        GameEvent.Create(PKG_NAME.S2C_SPRITE_LEAVE_ZONE, this, msgData)
            //    );

            //debug.Log("玩家 怪物 等等离开视野 " + msgData.dump());
            //debug.Log("-------------------------------+" + msgData.dump());
            Variant data = msgData;
            foreach (uint iid in data["iidary"]._arr)
            {

                OtherPlayerMgr._inst.RemoveOtherPlayer(iid);
                MonsterMgr._inst.RemoveMonster(iid);
            }
        }

        // private void on_begin_change_map_res (Variant msgData)
        //{
        //    NetClient.instance.dispatchEvent(
        //            GameEvent.Create(PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, this, msgData)
        //        );
        //}

        private void onMapChange(Variant msgData)
        {
            if (a3_expbar.instance != null)
            {
                a3_expbar.instance.CloseAgainst();
            }

            debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>onMapChange>" + msgData.dump());

            if (A3_BuffModel.getInstance()?.BuffCd != null)
            {
                A3_BuffModel.getInstance().BuffCd.Clear();
                a3_buff.instance?.resh_buff();
            }
            if (msgData.ContainsKey("states"))
            {

                Variant buff = msgData["states"];
                foreach (Variant one in buff["state_par"]._arr)
                {
                    Variant data = one;
                    A3_BuffModel.getInstance().addBuffList(data);
                }

            }




            if (msgData.ContainsKey("pk_state"))
            {
                PlayerModel.getInstance().now_pkState = msgData["pk_state"];
                switch (PlayerModel.getInstance().now_pkState)
                {
                    case 0:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_PEACE;
                        break;
                    case 1:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_PKALL;
                        PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().cid;
                        PlayerModel.getInstance().m_unPK_Param2 = PlayerModel.getInstance().cid;
                        break;
                    case 2:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_TEAM;
                        PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().teamid;
                        PlayerModel.getInstance().m_unPK_Param2 = PlayerModel.getInstance().clanid;
                        break;
                    //case 3:
                    //    PlayerModel.getInstance().pk_state = PK_TYPE.PK_LEGION;
                    //    PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().clanid;
                    //    break;
                    //case 4:
                    //    PlayerModel.getInstance().pk_state = PK_TYPE.PK_HERO;
                    //    //？？？
                    //    break;

                    case 5:
                        PlayerModel.getInstance().pk_state = PK_TYPE.Pk_SPOET;

                        break;
                }
                if (a3_pkmodel._instance)
                    a3_pkmodel._instance.ShowThisImage(msgData["pk_state"]);

                InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modPkState", "model/PlayerModel", PlayerModel.getInstance().now_pkState, true);
            }
            GRMap.grmap_loading = true;
            if (a3_liteMiniBaseMap.instance)
                a3_liteMiniBaseMap.instance.clear();
            PlayerModel.getInstance().refreshByChangeMap(msgData);
            GRMap.curSvrMsg = msgData;
            NetClient.instance.dispatchEvent(
                    GameEvent.Create(PKG_NAME.S2C_MAP_CHANGE, this, msgData)
                );

            if (SelfRole.fsm.Autofighting)
                SelfRole.fsm.Stop();

            if (msgData["hp"] <= 0)
            {
                SelfRole._inst.onDead(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RELIVE);
            }

            change_map = false;
        }
        ///**
        // *其他人穿上、脱下装备结果 
        // * @param data
        // * 
        // */	
        private void on_other_eqp_change(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                  GameEvent.Create(PKG_NAME.S2C_ON_OTHER_EQP_CHANGE, this, msgData)
              );
        }




        //地图掉落金币
        private void on_map_dpitem_res(Variant msg_data)
        {
            // BaseRoomItem.instance.showDropItem();

            // DropItemdta 

            debug.Log("!!!!!!!!!!on_map_dpitem_res!" + msg_data.dump());


            float x = msg_data["x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
            float y = msg_data["y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;

            if (msg_data.ContainsKey("owner_see"))
            {
                if (msg_data["owner_see"] != PlayerModel.getInstance().cid)
                {
                    return;
                }
            }

            Vector3 pos = new Vector3(x, 0f, y);

            List<Variant> l = msg_data["itms"]._arr;
            List<DropItemdta> lDropItm = new List<DropItemdta>();
            long timer = NetClient.instance.CurServerTimeStampMS;
            foreach (Variant v in l)
            {
                DropItemdta d = new DropItemdta();
                d.init(v, timer);
                lDropItm.Add(d);
            }

            if (BaseRoomItem.instance != null)
                BaseRoomItem.instance.showDropItem(pos, lDropItm);
        }

        public void sendPickUpItem(uint dpid)
        {
            Variant d = new Variant();
            d["id"] = dpid;
            sendRPC(PKG_NAME.C2S_PICK_DPITEM_RES, d);
        }
        private void on_pickitem(Variant msg_data)
        {

            int res = msg_data["res"];
            debug.Log("dddddd-------------" + msg_data.dump());
            if (res == 1)
            {
                if (BaseRoomItem.instance != null)
                {
                    if (msg_data["cid"] == PlayerModel.getInstance().cid)
                    {
                        BaseRoomItem.instance.flyGetItmTxt(msg_data["id"], false);
                        //if (LevelProxy.getInstance().is_open == true)
                        //{
                        //    int aa = BaseRoomItem.instance.drop_id(msg_data["id"]);

                        //    drop.Add(aa);
                        //}

                    }

                    BaseRoomItem.instance.removeDropItm(msg_data["id"], false);
                }
            }
            else if (res == -824)
            {
                flytxt.instance.fly(ContMgr.getCont("worldmap_cangetitem"));
            }
            else if (res == -1101)
            {
                flytxt.instance.fly(ContMgr.getCont("worldmap_fullbag"));
                DropItem.cantGetTimer = NetClient.instance.CurServerTimeStampMS + 1500;
            }
            else if (res == -825)
            {
                int tm = msg_data["tm"];
                if (tm <= 0)
                    tm = 1;
                flytxt.instance.fly(ContMgr.getCont("worldmap_cangetitem_tm", new List<string> { tm.ToString() }));

            } else if (res == -827) {

                Globle.err_output(-825);

            }
        }

        public void sendCollectItem(uint dpid)
        {
            Variant d = new Variant();
            d["iid"] = dpid;
            // d["op"] = 1;
            sendRPC(PKG_NAME.C2S_BEGIN_COLLECT, d);
            //sendRPC(PKG_NAME.C2S_A3_ACTIVE_GETCHEST, d);
        }
        public void sendCollectBox(uint dpid)
        {
            Variant d = new Variant();
            d["op"] = 1;
            d["iid"] = dpid;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_GETCHEST, d);
        }

        public void sendStopCollectBox(bool forcestop)
        {
            Variant d = new Variant();
            d["op"] = forcestop ? 2 : 3;
            sendRPC(PKG_NAME.C2S_A3_ACTIVE_GETCHEST, d);
            // InterfaceMgr.getInstance().close(InterfaceMgr.CD);
        }
        //public void endCollectBox()
        //{
        //    Variant d = new Variant();
        //    d["op"] = 3;
        //    // d["iid"] = dpid;
        //    sendRPC(PKG_NAME.C2S_A3_ACTIVE_GETCHEST, d);
        //    // InterfaceMgr.getInstance().close(InterfaceMgr.CD);
        //}

        public void sendStopCollectItem(bool forcestop)
        {
            Variant d = new Variant();
            d["end_tp"] = forcestop ? 2 : 1;
            sendRPC(PKG_NAME.C2S_STOP_COLLECT, d);

            A3_TaskModel tkModel = A3_TaskModel.getInstance();
            if(forcestop == false)
                a3_task_auto.instance.RunTask(tkModel.curTask);
        }

        public void onbeginCollect(Variant msg)
        {
            debug.Log("onbeginCollect:::" + msg.dump());

            int res = msg["res"];

            if (res < 0)
            {

                flytxt.instance.fly(ContMgr.getError(res.ToString()));

                InterfaceMgr.getInstance().close(InterfaceMgr.CD);

                A3_TaskModel tkModel = A3_TaskModel.getInstance();

                if (tkModel.curTask != null) a3_task_auto.instance.RunTask(tkModel.curTask);
            }

        }
    }
}
