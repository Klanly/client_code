using System;
using System.Collections.Generic;
using Cross;
using GameFramework;


namespace MuGame
{
    public class LGGDLevels : lgGDBase
    {
        //private SvrLevelConfig _svrLevelConf;

        private Variant _curr_lvl_conf;
        private Variant _cur_level;
        private bool _is_fin = false;
        private int _curlay = 0;
        protected int _lvlsideid;

        //通过NPC进入副本时，npc的id
        private uint _enter_lvl_npc = 0;

        public LGGDLevels(gameManager m)
            : base(m)
        {
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new LGGDLevels(m as gameManager);
        }
        override public void init()
        {
            this.g_mgr.g_netM.addEventListener(UI_EVENT.LGGD_LEVEL, switchFunc);


            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_GET_LVLMIS_RES, switchFunc);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_CLANTER_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_CARRCHIEF, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_BROADCAST, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_MOD_LVL_SELFPVPINFO, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_ERR_MSG, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_CHECK_IN_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_CREATE_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_ENTER_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_GET_LVL_INFO_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_FIN, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_GET_PRIZE_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_SIDE_INFO, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_CLOSE_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVL_KM, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LEAVE_LVL_RES, switchFunc);
            this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_ON_BATTLE_DO_RES, switchFunc);

        }
        private void switchFunc(GameEvent e)
        {
            Variant data = e.data;
            //DebugTrace.print(GameTools.GetMethodInfo(data));
            if (data.ContainsKey("case"))
                switch (data["case"]._str)
                {
                    case "enter_scritp"://点击进入按钮
                        data = data["data"];
                        enter_scritp(data["ltpid"], data["npcid"], data["diff"]);
                        break;
                    case "SetKillMonTrigs":
                        SetKillMonTrigs(data["data"]);
                        break;

                    case "get_lvlmis_res":
                        get_lvlmis_res(data["data"]);
                        break;
                    case "on_clanter_res":
                        on_clanter_res(data["data"]);
                        break;
                    case "on_lvl_res":
                        on_lvl_res(data["data"]);
                        break;

                    //case "on_npcshop_res":
                    //    on_npc_shop(data["data"]);
                    //    //A3_NPCShopProxy.getInstance().onNPCShop(data["data"]);
                    //    break;
                    case "on_lvl_broadcast_res":
                        on_lvl_broadcast_res(data["data"]);
                        break;
                    case "lvl_pvpinfo_board_msg":
                        lvl_pvpinfo_board_msg(data["data"]);
                        break;
                    case "mod_lvl_selfpvpinfo":
                        mod_lvl_selfpvpinfo(data["data"]);
                        break;
                    case "on_lvl_err_msg":
                        on_lvl_err_msg(data["data"]);
                        break;
                    case "on_check_in_lvl_res":
                        on_check_in_lvl_res(data["data"]);
                        break;
                    case "on_create_lvl_res":
                        on_create_lvl_res(data["data"]);
                        break;
                    case "on_enter_lvl_res":
                        on_enter_lvl_res(data["data"]);
                        break;
                    case "get_associate_lvls_res":
                        get_associate_lvls_res(data["data"]);
                        break;
                    //case "get_lvl_info_res":
                    //    get_lvl_info_res(data["data"]);
                    //    break;
                    //case "on_lvl_fin":
                    //    on_lvl_fin(data["data"]);
                    //    break;
                    //case "lvl_get_prize_res":
                    //    lvl_get_prize_res(data["data"]);
                    //    break;
                    case "on_lvl_side_info":
                        on_lvl_side_info(data["data"]);
                        break;
                    case "on_close_lvl_res":
                        on_close_lvl_res(data["data"]);
                        break;
                    case "on_lvl_km":
                        on_lvl_km(data["data"]);
                        break;
                    case "on_leave_lvl":
                        on_leave_lvl();
                        break;
                    case "on_battle_do_res":
                        on_battle_do_res(data["data"]);
                        break;
                    default:
                        GameTools.PrintNotice("switchFunc defanult");
                        break;

                }
            else GameTools.PrintNotice("switchFunc no case");
        }

        //------------------------------------------------当前副本信息
        //当前副本难度
        private uint _current_diff = 0;
        public Boolean in_level
        {
            get
            {
                return _cur_level != null;
            }
        }

        public int current_lvl
        {
            get
            {
                return _curltpid;
            }
        }

        public Boolean IsNeedLvl(Variant tpids)
        {
            return in_level && (tpids != null && tpids._arr.IndexOf(_curltpid) >= 0);
        }

        public Boolean InMultiLvl()
        {
            bool flag = _cur_level != null;
            if (flag)
            {
                flag = false;
                Variant multiLevel = muCCfg.svrLevelConf.GetMultiLevel();//this.lgInGame.lgClient.svrGameConfig.svrLevelConf.GetMultiLevel();
                foreach (Variant slevel in multiLevel._arr)
                {
                    if (slevel["tpid"] == _curltpid)
                    {
                        flag = true;
                    }
                }
            }

            return flag;
        }

        /**
         * 当前副本难度
         */
        public uint current_diff
        {
            get
            {
                return _current_diff;
            }
        }

        /**
         *是否在侠客行副本 
         */
        public Boolean in_travel_script()
        {
            if (_cur_level != null)
            {
                return muCCfg.svrLevelConf.get_levelmis_data(_cur_level["ltpid"]) != null;
            }
            return false;
        }

        public Variant get_curr_lvl_info()
        {
            return _cur_level;
        }

        public Variant get_curr_lvl_conf()
        {
            return _curr_lvl_conf;
        }

        public int get_lvlsideid()
        {
            return _lvlsideid;
        }
        public int GetKillCount()
        {
            int count = 0;
            if (_cur_level.ContainsKey("score_km"))
            {
                foreach (Variant obj in _cur_level["score_km"]._arr)
                {
                    Variant sonf = muCCfg.svrMonsterConf.get_monster_data(obj["mid"]);
                    if (sonf)
                    {
                        count += obj["cnt"];//上次退出前杀怪数	
                    }
                }
            }
            return count;
        }
        //忽略队友
        public Boolean is_currlvl_ignore_team()
        {
            if (_curr_lvl_conf)
            {
                if (_curr_lvl_conf.ContainsKey("pvp"))
                {
                    return _curr_lvl_conf["pvp"][0]["ignore_team"] == 1;
                }
            }
            return false;
        }
        //忽略阵营
        public Boolean is_currlvl_map_ignore_side(int mapid)
        {
            if (_curr_lvl_conf)
            {
                Variant maps = _curr_lvl_conf["map"];
                if (maps != null)
                {
                    foreach (Variant map in maps._arr)
                    {
                        if (mapid == map["id"])
                        {
                            return map["ignore_side"] == 1;
                        }
                    }
                }
            }
            return false;
        }
        public Boolean is_currlvl_ignore_clan()
        {
            if (_curr_lvl_conf)
            {
                if (_curr_lvl_conf.ContainsKey("pvp"))
                {
                    return _curr_lvl_conf["pvp"][0]["ignore_clan"] == 1;
                }
            }
            return false;
        }

        //------------------------------------------------副本 信息      

        public uint get_lvl_max_difflvl(Variant lvl_conf)
        {
            uint max_diff_lvl = 1;
            Variant diff_lvl_conf = lvl_conf["diff_lvl"] as Variant;
            if (diff_lvl_conf != null)
            {
                foreach (Variant conf in diff_lvl_conf._arr)
                {
                    if (conf["lv"] > max_diff_lvl)
                    {
                        max_diff_lvl = conf["lv"];
                    }
                }
            }
            return max_diff_lvl;
        }

        /// <summary>
        /// 进入副本或战场的方法 @data 数据  @_diff 难度。默认为1 
        /// </summary>
        /// <param name="ltpid"></param>
        /// <param name="npcid"></param>
        /// <param name="diff"></param>
        /// <param name="inroom"></param>
        /// <param name="cost_tpt"></param>
        public void enter_scritp(uint ltpid, int npcid = 0, int diff = 1, Boolean inroom = false, int cost_tp = 0)
        {
            //
            //if(!inroom && _lgInGame.lgGD_Team.IsInRoom())
            //{

            //    var str1:String = LanguagePack.getLanguageText("team_lvl","inteamlevel");
            //    var mainui:LGIUIMainUI = this._lgInGame.lgClient.uiClient.getLGUI(UIName.LGUIMainUIImpl) as LGIUIMainUI;
            //    mainui.systemmsg([str1],1024);
            //    return;
            //}
            Variant lvlconfig = (this.g_mgr.g_gameConfM as muCLientConfig).
                svrLevelConf.get_level_data(ltpid);//_svrLevelConf.get_level_data(ltpid);
            if (lvlconfig == null)
                return;

            _enter_lvl_npc = (uint)npcid;
            if (lvlconfig.ContainsKey("cltid"))
            {
                //check_in_clte( lvlconfig.cltid, ltpid );
                //DebugTrace.print(GameTools.GetMethodInfo("cltid"));
            }
            else if (lvlconfig.ContainsKey("cltwarid"))
            {
                //check_in_clte( lvlconfig.cltwarid, ltpid );
                //DebugTrace.print(GameTools.GetMethodInfo("cltwarid"));
            }
            else if (lvlconfig.ContainsKey("slvl_diff"))
            {
                //DebugTrace.print(GameTools.GetMethodInfo("slvl_diff"));
                check_in_slvl_diff(lvlconfig["slvl_diff"]._uint, (uint)diff, (uint)cost_tp);
            }
            else
            {
                if (lvlconfig["lctp"] == 1)
                {
                    Variant lvl = get_lvl_llid(ltpid);
                    if (lvl == null)
                    {//自主创建副本
                        create_lvl(ltpid, (uint)diff, (uint)cost_tp);
                    }
                    else
                    {
                        enter_lvl(lvl["llid"], lvl["ltpid"]);
                    }
                }
                else if (lvlconfig["lctp"] == 2)
                {
                    //服务器创建副本
                    if (lvlconfig.ContainsKey("carrchief"))
                    {
                        check_in_carrchief(lvlconfig["carrchief"]);
                    }
                    else
                    {
                        check_in_lvl(ltpid);
                    }
                }
                else
                {
                    //err
                    //DebugTrace.add(DebugTrace.DTT_ERR,("lctp err! lctp = " + lvlconfig["lctp"]));
                    //DebugTrace.print("lctp err! lctp = " + lvlconfig["lctp"]);
                }
            }
        }

        //----------------------------------- 签入副本---------------------------------------------
        //		1.	消息名称：check_in_lvl
        //		消息功能：报名加入副本；
        //		cmd_id：240
        //		参数：{
        //		  ltpid:可选，报名加入的副本配置id,
        //		  arenaid:可选，报名签入排队的竞技场配置id,
        //		  chlvl:可选，arenaid存在情况下必填，=true表示签入决赛副本,
        //		  cancel:可选，arenaid存在情况下必填，=true表示取消当前竞技场排队,
        //		  clteid:可选，待加入的帮派领地配置id,
        //			carrc: 可选，职业ID
        //		}
        //		对应消息：check_in_lvl_res	
        private void check_in_slvl_diff(uint slvl_diff, uint diff_lvl, uint cost_tp)
        {
            igLevelMsg.check_in_lvl(GameTools.createGroup(
                "slvl_diff", slvl_diff, "diff_lvl", diff_lvl, "cost_tp", cost_tp
            ));//{slvl_diff:slvl_diff,diff_lvl:diff_lvl,cost_tp:cost_tp} );
        }
        private void check_in_lvl(uint ltpid)
        {
            igLevelMsg.check_in_lvl(GameTools.createGroup("ltpid", ltpid));
        }
        private void check_in_carrchief(uint carrid)
        {
            igLevelMsg.check_in_lvl(GameTools.createGroup("carrc", carrid));
        }

        public void check_in_arena(uint arenaid, bool chlvl, bool cancel = false)
        {//报名参加竞技场
            igLevelMsg.check_in_lvl(GameTools.createGroup(
            "arenaid", arenaid, "chlvl", chlvl, "cancel", cancel));
        }
        public void check_in_arenaex(uint arenaexid, bool cancel = false)
        {//报名多人竞技场
            igLevelMsg.check_in_lvl(GameTools.createGroup(
            "arenaexid", arenaexid, "cancel", cancel));
        }

        private void check_in_clte(uint clteid, uint lvltpid)
        {//报名加入帮派领地
            Variant terrConfig = (this.g_mgr.g_gameConfM as muCLientConfig).
                svrLevelConf.get_clan_territory(clteid);//_svrLevelConf.get_clan_territory( clteid );
            if (terrConfig)
            {
                switch (terrConfig["tp"]._str)
                {
                    case "1"://帮派家园副本
                        {
                            igLevelMsg.check_in_lvl(GameTools.createGroup("clteid", clteid, "war", false));
                        }
                        break;
                    case "2":
                        {
                            if (terrConfig["warlvl"]["tpid"] == lvltpid)
                            {//进入领地争夺副本
                                float tmnow = muNClt.CurServerTimeStampMS;//this._lgInGame.lgClient.session.connection.cur_server_tm;		
                                if (ConfigUtil.check_tm(tmnow, terrConfig["warlvl"]["tmchk"][0]))
                                {
                                    igLevelMsg.check_in_lvl(GameTools.createGroup("clteid", clteid, "war", true));
                                }
                                else
                                {
                                    this.on_lvl_err_msg(GameTools.createGroup("res", -411));
                                }
                            }
                            else
                            {   //议事厅副本
                                Variant selfclan = null;//this.lgInGame.lgGD_clans.GetSelfInfo();
                                if (selfclan && (selfclan["clanid"] == IsClanHasClte(clteid)))
                                {
                                    igLevelMsg.check_in_lvl(GameTools.createGroup("clteid", clteid, "war", false));
                                }
                            }
                        }
                        break;
                }
            }
        }

        //		3.	消息名称：check_in_lvl_res
        //		消息功能：报名加入副本结果，收到该消息后可立即发送enter_lvl消息进入副本；
        //		cmd_id：240
        //		参数：{
        //		  arenaid:可选，当前签入等待队列的竞技场id,
        //		  tm:可选，预计队列等待实际，单位秒,
        //		  llid:可选，副本实例id, 
        //		  ltpid:可选，副本配置id, 
        //		  lmtp:可选，副本规模，=1为单人副本，=2为组队副本，=3为帮派副本（废弃），=4为多人副本},
        //		  match:可选，是否为匹配进入（匹配进入需弹提示框，而非直接发送enter_lvl消息进入），=true为匹配进入,
        //		  acancel:可选，=true表示取消了当前的排队等待}
        //		对应消息：check_in_lvl
        //			

        public void on_check_in_lvl_res(Variant data)
        {
            //_add_lvl_llid( data );


            //if(data.ContainsKey("acancel") && data["cancel"])
            //{//跨服竞技
            //    if(ui_crossWar.IsMyChange())
            //    {
            //        ui_crossWar.MatchTimeOut(true);
            //    }
            //    else
            //    {
            //        ui_crossWar.ClearWarTime();
            //        ui_crossWar.ChangePrepareGoto(true);
            //    }
            //    //_lgInGame.lgGD_Arena.acancel();
            //    //_lgInGame.lgGD_Arena.SetBattleMatch(false);//取消排队
            //}
            //else if(data.ContainsKey("arenaid") || data.ContainsKey("arenaexid"))
            //{//报名竞技场成功,开始等待匹配
            //    if(data.ContainsKey("arenaid"))
            //    {
            //        //_lgInGame.lgGD_Arena.Update_arena_info(data["arenaid"],data);
            //    }
            //    else
            //    {
            //        //_lgInGame.lgGD_Arena.Update_arenaex_info(data["arenaexid"],data);	
            //    }
            //    if(!ui_crossWar.IsMyChange())
            //    {
            //        //var btm:Number = _lgInGame.lgClient.session.connection.cur_server_tm;
            //        //_lgInGame.lgGD_Arena.SetWarStartTm(btm);
            //        ui_crossWar.ShowWarTime(0);
            //    }
            //    //_lgInGame.lgGD_Arena.SetBattleMatch(true);
            //    //_lgInGame.lgGD_Arena.ClearWarTime();
            //    ui_crossWar.ChangePrepareGoto(false);

            //}
            //else if(data.ContainsKey("match") && data["match"])
            //{//匹配进入
            //    _matchLvlInfo = GameTools.createGroup("iid",data["iid"]);// { iid:data.llid };
            //    if(data.ContainsKey("m_ply"))
            //    {
            //        _matchLvlInfo["m_ply"] = data["m_ply"];
            //    }
            //    if( uiLevel != null)
            //    {
            //        uiLevel.OnMetchLevel( _matchLvlInfo );
            //    }
            //}			
            //else
            //{
            //    uint diff_lvl = data.ContainsKey("diff_lvl") ? data["diff_lvl"]._uint : 0;
            //    //成功后直接进入副本
            //    enter_lvl(data["llid"]._uint, data["ltpid"]._uint, diff_lvl, false);
            //}
        }
        //----------------------------------- 创建副本 ---------------------------------------------
        private void create_lvl(uint ltpid, uint diff_lvl, uint cost_tp = 0)
        {
            Variant sendData = new Variant();//{npcid:_enter_lvl_npc, ltpid:ltpid, diff_lvl:diff_lvl};
            sendData["npcid"] = _enter_lvl_npc;
            sendData["ltpid"] = ltpid;
            sendData["diff_lvl"] = diff_lvl;
            if (cost_tp != 0)
            {
                sendData["cost_tp"] = cost_tp;
            }
            igLevelMsg.create_lvl(sendData);
        }

        public void Create_script(uint ltpid, int npcid = 0, int diff_lvl = 1, uint cost_tp = 0)
        {
            Variant sendData = new Variant();//{npcid:npcid,ltpid:ltpid,diff_lvl:diff_lvl};
            sendData["npcid"] = npcid;
            sendData["ltpid"] = ltpid;
            sendData["diff_lvl"] = diff_lvl;
            if (cost_tp != 0)
            {
                sendData["cost_tp"] = cost_tp;
            }
            Variant einfo = new Variant();
            einfo["npcid"] = npcid;
            einfo["ltpid"] = ltpid;
            einfo["diff_lvl"] = diff_lvl;
            igLevelMsg.create_lvl(einfo);
        }
        /// <summary>
        /// 创建副本成功
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>

        private int curmapid;
        public void on_create_lvl_res(Variant data)
        {
            //LGUIMainUIImpl_NEED_REMOVE.TO_LEVEL = true;
            _add_lvl_llid(data);

            if (data["lmtp"] != 2)
            {//不是组队副本
                if (data["creator_cid"] != null)//== this._lgInGame.selfPlayer.cid)
                {
                    uint llid = data.ContainsKey("llid") ? data["llid"]._uint : 0;
                    uint diff_lvl = data.ContainsKey("diff_lvl") ? data["diff_lvl"]._uint : 1;
                    uint cost_tp = data.ContainsKey("cost_tp") ? data["cost_tp"]._uint : 0;

                    Variant d = SvrLevelConfig.instacne.get_level_data(data["ltpid"]);
                    //curmapid = MapModel.getInstance().getMapDta(data["ltpid"]).mapid;
                    curmapid = d["map"][0]["id"];
                    enter_lvl(llid, data["ltpid"]._uint, diff_lvl, false, cost_tp);
                }
                else if (data["lmtp"] == 4)
                {
                    //  _lgInGame.lgGD_Team.GetSelfRoom()
                    int self = 0;// _lgInGame.lgGD_Team.GetCurRoomPar();
                    if (self == data["ltpid"])
                    {
                        enter_lvl(data["llid"], data["ltpid"]._uint);
                    }
                }
            }
            else
            {
                uint llid = data.ContainsKey("llid") ? data["llid"]._uint : 0;
                uint diff_lvl = data.ContainsKey("diff_lvl") ? data["diff_lvl"]._uint : 1;
                uint cost_tp = data.ContainsKey("cost_tp") ? data["cost_tp"]._uint : 0;
                Variant d = SvrLevelConfig.instacne.get_level_data(data["ltpid"]);
                curmapid = d["map"][0]["id"];
                enter_lvl(llid, data["ltpid"]._uint, diff_lvl, false, cost_tp);

            }

            //if (uiLevel)
            //{
            //    uiLevel.OnCreateLevel(data);
            //}
            this.g_mgr.dispatchEvent(GameEvent.Create(UI_EVENT.LGUI_LEVEL,
                this, GameTools.CreateSwitchData("OnCreateLevel", data)));
        }
        //----------------------------------- 进入副本 ---------------------------------------------
        private void enter_lvl(uint llid, uint ltpid, uint diff_lvl = 0, bool bslvl = false, uint cost_tp = 0)
        {
            Variant sendData = GameTools.createGroup("tp", 1, "npcid", _enter_lvl_npc, "llid", llid, "bslvl", bslvl);
            if (diff_lvl != 0)
            {
                sendData["diff_lvl"] = diff_lvl;
            }
            if (cost_tp != 0)
            {
                sendData["cost_tp"] = cost_tp;
            }

            sendData["mapid"] = curmapid;
            sendData["ltpid"] = ltpid;
            curmapid = 0;
            igLevelMsg.enter_lvl(sendData);
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        public void changeLvl()
        {
            Variant sendData = GameTools.createGroup("tp", 2);
            igLevelMsg.enter_lvl(sendData);
        }
        //        //		4.	消息名称：enter_lvl_res
        //        //		消息功能：进入副本结果；
        //        //		cmd_id：242
        //        //		参数：{llid:副本实例id, ltpid:副本配置id, diff_lvl:难度等级, end_tm:结束时间（服务器时间，unix时间戳，单位秒）,cur_round:可选，当前的回合,round_tm:可选，当前回合结束时间（UNIX时间戳，单位秒）,ghost_cnt:可选，离幽灵状态剩余复活次数,preptm:可选，准备时间（UNIX时间戳，单位秒）, kprec:可选，击杀信息字段{ kp:本次副本击杀玩家数, ckp:连续杀敌数, lvl_hexp:本次副本获得荣誉值, dc:本次副本死亡次数}win:可选，存在则表示副本已结束, close_tm:可选，若副本结束，则本字段出现，表示副本关闭时间（服务器时间，unix时间戳，单位秒）, km:杀怪统计数组，可选[{mid:怪物配置id, cntleft:剩余需杀数量}], sidekm:分阵营杀怪统计数组，可选[{sideid:阵营id,km:[{mid:怪物配置id, cntleft:剩余需杀怪数量}]}], sidept:分阵营积分信息数组，可选[{sideid:阵营id, pt:当前积分}]}
        //        //		对应消息：enter_lvl
        public void on_enter_lvl_res(Variant data)
        {

            PlayerModel.getInstance().inFb = true;
            //  MapProxy.getInstance().sendShowMapObj();



            _cur_level = data;
            _current_diff = data["diff_lvl"];
            Variant lvl_conf = muCCfg.svrLevelConf.get_level_data(_cur_level["ltpid"]);
            _curr_lvl_conf = lvl_conf;
            _curltpid = _cur_level["ltpid"];

            debug.Log("!!enter_lvl_res!! " + " _curltpid:" + _curltpid + " " + debug.count);

            //todo
            //_lgInGame.map.SetWorldBoss();
            //lgSelfPlayer player = _lgInGame.selfPlayer;
            // player.Stop();
            if (_lvlsideid != 0)
            {
                // player.SetLvlsideid( _lvlsideid );
                if (data.ContainsKey("bslvl"))
                {
                    // player.ShowSideInfo();	
                }
            }
            //  player.addEventListener( logicDataEvent.LDET_SELF_DIE, on_self_die);
            //string enterStr = muCCfg.localGeneral.GetCommonConf("enterDontStop")._str;
            //Variant strArr = GameTools.split(enterStr, ",");//enterStr.split(",");
            //if (strArr._arr.IndexOf(_curltpid) < 0)
            //{
            //    //_lgInGame.AIPly.stop();
            //}
            _update_level_ctnleft(lvl_conf["tpid"], (data.ContainsKey("cntleft") ? data["cntleft"]._int : 0));

            //添加至缓存中
            for (uint i = 0; i < this._associate_lvls.Count; i++)
            {
                Variant lvl = this._associate_lvls[(int)i];
                if (lvl["llid"]._int == _cur_level["llid"]._int)
                {
                    lvl["end_tm"] = data["end_tm"];
                    break;
                }
            }

            if (data.ContainsKey("cur_round"))
            {
                _curr_round_info["cur_round"] = data["cur_round"];
            }

            if (data.ContainsKey("round_tm"))
            {
                _curr_round_info["round_tm"] = data["round_tm"];
            }

            if (data.ContainsKey("preptm"))
            {
                _curr_round_info["preptm"] = data["preptm"];
            }

            if (data.ContainsKey("ghostcnt"))
            {
                _curr_round_info["ghostcnt"] = data["ghostcnt"];
            }

            //帮派副本数据
            if (data.ContainsKey("sideclan"))
            {
                _set_sideclan(data["sideclan"]);
                data.RemoveKey("sideclan");
            }

            if (_curr_lvl_conf.ContainsKey("pvp"))
            {
                if (_curr_lvl_conf.ContainsKey("arenaid") || _curr_lvl_conf.ContainsKey("arenaexid"))
                {
                    //					_main._gamescene.system_mgr.set_view_tp("normal","fast_skill");
                    if (data["bslvl"])
                    {   //进入跨服竞技场成功,客户端模拟将玩家数据显示到最大值（服务器已处理数据，未通知过来）						
                        //player.modHp( player.max_hp );
                        //player.modMp( player.max_mp );
                        //player.modDp( player.max_dp );
                        _curWaitTm = GameConstant.LAST_TM / 1000;   //20秒倒计时开始
                                                                    //ui_crossWar.SetCurWaitTm(_curWaitTm);
                                                                    //进入跨服服务器，移除身上有职业限制的状态(玩家属性改变服务器已通知，移除状态的消息涉及跨服服务器中iid，不做处理，客户端模拟)
                        _rmvCarrLimitState();
                    }
                }
            }
            if (data.ContainsKey("kumiteply"))
            {
                _set_kumite_plys(data["kumiteply"]);//更新玩家列表
                data.RemoveKey("kumiteply");
            }

            //攻城战
            if (data.ContainsKey("cltwar"))
            {
                _lvl_clanter_score = data["cltwar"];
                data.RemoveKey("cltwar");
            }
            //if (uiLevel != null)
            //{
            //    uiLevel.OnEnterLevel(lvl_conf, data);
            //}

            if (data.ContainsKey("kprec"))
            {
                _cur_level["kprec"] = data["kprec"];
            }
            else if (_cur_level != null && _curr_lvl_conf["lptp"] == 2)
            {
                _cur_level["kprec"] = GameTools.createGroup("kp", 0, "dc", 0, "ckp", 0, "mdc", 0, "mkp", 0);
            }
            if (_curr_lvl_conf.ContainsKey("map_lay"))
            {
                _curlay = 0;//array 从1起
            }
            if (_enters._arr.IndexOf(data["ltpid"]) == -1)
            {
                _enters._arr.Add(data["ltpid"]);
            }

            // player.AddLvlAvatar(lvl_conf.tpid);
            //player.ShowLevelTitle();
            //this.lgInGame.trigMgr.DoTrigger( TriggerManager.TRGT_ENTERLVL, lvl_conf.tpid );

            //组队副本
            if (InMultiLvl())
            {
                //todo
                //lgmainui.EnterMultilvl();
            }
        }

        //        //移除有职业限制的buff,客户端模拟
        private void _rmvCarrLimitState()
        {
            //uint selfiid = 0;//_lgInGame.selfPlayer.iid;
            uint carr = 0;//.selfPlayer.carr;
            Variant bstates = null;//_lgInGame.selfPlayer.GetBless();
            foreach (Variant bstate in bstates._arr)
            {
                if (muCCfg.svrGeneralConf.CarrBStateLimit(carr, bstate["id"]))
                {
                    // _lgInGame.selfPlayer.BlessChange({bstate rmvid.id});
                }
            }
            Variant removed_ids = new Variant();
            //Variant normalstates = null;//_lgInGame.selfPlayer.GetStates();
            //                    foreach (Variant state in normalstates._
            //)
            //                    {
            //                        if( muCCfg.svrGeneralConf.CarrStateLimit(carr, state["id"]))
            //                        {
            //                            removed_ids._arr.Add(state["id"]);
            //                        }
            //                    }
            if (removed_ids.Count > 0)
            {
                // _lgInGame.map.RemoveStates(GameTools.createGroup("iid",selfiid,"ids", removed_ids));
            }
        }

        //        //----------------------------------- 离开副本 ---------------------------------------------
        public void leave_lvl()
        {
            if (null == _cur_level)
                return;
            igLevelMsg.leave_lvl();
        }

        public void on_leave_lvl()
        {
            PlayerModel.getInstance().inFb = false;
            // lgInGame.selfPlayer.RemoveLvlAvatar(_curltpid);
            //lgInGame.selfPlayer.RemoveLevelTitle(_curltpid);
            // this._lgInGame.selfPlayer.Stop( {false clearAI} );	
            if (_lvlsideid != 0)
            {
                //_lgInGame.selfPlayer.UnshowSideInfo(_lvlsideid);
                _lvlsideid = 0;
                //_lgInGame.selfPlayer.SetLvlsideid( 0 );	
            }

            Variant multiLevel = muCCfg.svrLevelConf.GetMultiLevel();
            foreach (Variant slevel in multiLevel._arr)
            {
                if (slevel["tpid"] == _curltpid)
                {
                    //组队副本
                    //lgmainui.LeaveMultilvl();
                    for (int i = 0; i < _associate_lvls.Count; i++)
                    {
                        Variant lvl = _associate_lvls[i];
                        if (lvl["llid"] == _cur_level["llid"])
                        {
                            _associate_lvls._arr.RemoveAt(i);//.splice(i,1);
                            break;
                        }
                    }
                    break;
                }
            }

            _cur_level = null;
            //string leaveStr = muCCfg.localGeneral.GetCommonConf("leaveDontStop");
            //Variant strArr = GameTools.split(leaveStr, ",");
            //if (strArr._arr.IndexOf(_curltpid) < 0)
            //{
            //    //_lgInGame.AIPly.stop();
            //}
            //if (uiLevel != null)
            //{
            //    uiLevel.OnLeaveLevel(_curr_lvl_conf, _is_fin);
            //}
            _curr_lvl_conf = null;
            _kumiteply._arr.Clear();
            _kumrs = null;
            _kumre = null;
            _curr_round_info = new Variant();
            _sideclan = null;
            _kmtrigs = null;
            _matchLvlInfo = null;
            _hadCost = null;
            _is_fin = false;
            //退出房间
            //if (_lgInGame.lgGD_Team.IsInRoom())
            //{
            //    _lgInGame.lgGD_Team.LeaveRoom();
            //}

        }

        //        //----------------------------------- 副本结束 ---------------------------------------------
        //        //		5.	消息名称：lvl_fin
        //        //		消息功能：副本结束；
        //        //		cmd_id：245
        //        //		参数：{ win: >0时，若有阵营则为胜利阵营id，否则为玩家cid，<0为失败，=0则为所有玩家胜利，若自己阵营胜利，则可领取副本奖励, close_tm:关闭时间，到时间将强制关闭（服务器时间，unix时间戳，单位秒）, ply_res:可选，当副帮类型为单人或组队副本时该字段存在，该字段不存在时，则通过胜利失败来判断是否可领取奖励，副本用户结果数组[ {cid:角色id, has_prz:是否有奖励, score:得分,achives:获得的成就id数组[], diff_lvl:可选，新开放难度等级, besttm:可选，最佳通关时间，单位秒} ], winplycids:可选，胜利玩家cid数组，若该字段存在，则根据该字段判断自己胜利与否，忽略win字段[], aply_res:可选，竞技场角色信息、积分情况表{winer:胜利者数组[{ainfo:{pt:当前积分}, level:角色等级, carr:角色职业, sex:角色性别, ptchange:可选，积分变化值}], loser: 失败者数组[{ainfo:{pt:当前积分}, level:角色等级, carr:角色职业, sex:角色性别, ptchange:可选，积分变化值}]}}

        public void on_lvl_fin(Variant data)
        {
            _is_fin = true;

            //this._lgInGame.selfPlayer.removeEventListener(logicDataEvent.LDET_SELF_DIE, on_self_die);

            _rmv_lvl_llid(_cur_level["iid"]);
            //DebugTrace.print(GameTools.GetMethodInfo(_cur_level));

            //清空杀怪触发器数据
            SetKillMonTrigs(null);

            _lvl_fin(data);
        }

        private void _lvl_fin(Variant data)
        {
            Variant lvl_conf = _curr_lvl_conf;
            bool real_win = false;
            uint selfcid = 0;//this._lgInGame.selfPlayer.cid;

            for (int i = 0; i < _associate_lvls.Count; i++)
            {
                Variant lvl = _associate_lvls[i];
                if (lvl["llid"] == _cur_level["llid"])
                {
                    _associate_lvls._arr.RemoveAt(i);//.splice(i,1);
                    break;
                }
            }
            //跨服战
            if (data.ContainsKey("self_win") && data["self_win"] == true)
            {
                real_win = true;
            }
            if (data.ContainsKey("winplycids"))
            {
                foreach (uint plycid in data["winplycids"]._arr)
                {
                    if (plycid == selfcid)
                    {
                        real_win = true;
                        break;
                    }
                }
            }
            else
            {
                if (data["win"] == 0)
                {
                    real_win = true;
                }
                else if (data["win"] > 0)
                {
                    Variant pvp_conf = lvl_conf["pvp"][0];
                    if ((pvp_conf.ContainsKey("side"))
                         || ((pvp_conf.ContainsKey("death_match")) && (pvp_conf["death_match"][0].ContainsKey("side")))
                         || ((pvp_conf.ContainsKey("cltwar")) && (pvp_conf["cltwar"][0].ContainsKey("side")))
                         || ((pvp_conf.ContainsKey("clcqwar")) && (pvp_conf["clcqwar"][0].ContainsKey("side"))))
                    {
                        real_win = data["win"] == _lvlsideid;
                    }
                    else
                    {
                        real_win = selfcid == data["win"];
                    }
                }
            }

            if (real_win)
            {
                if (lvl_conf["lptp"] == 1)
                {
                    Variant self_res = null;
                    if (data.ContainsKey("ply_res"))
                    {
                        foreach (Variant ply_res in data["ply_res"]._arr)
                        {
                            if (selfcid == ply_res["cid"])
                            {
                                self_res = ply_res;
                                break;
                            }
                        }
                    }

                    if (self_res != null)
                    {
                        _update_lvlinfo_on_lvlfin(lvl_conf["tpid"], self_res);

                        //获得称号
                        if (self_res.ContainsKey("achives"))
                        {
                            foreach (Variant achData in self_res["achives"]._arr)
                            {
                                //this._lgInGame.lgGD_achives.OnAddAchive(achData);
                            }
                        }

                        if (self_res["has_prz"] == true)
                        {
                            this._add_lvlprize(GameTools.createGroup("ltpid", _cur_level["ltpid"], "diff_lvl", _cur_level["diff_lvl"]));// { _cur_level ltpid.ltpid, _cur_level diff_lvl.diff_lvl} );
                        }
                    }
                }
                else if (lvl_conf["lptp"] == 2)
                {
                    //战场
                    this._add_lvlprize(GameTools.createGroup("ltpid", _cur_level["ltpid"], "diff_lvl", _cur_level["diff_lvl"]));//{ _cur_level ltpid.ltpid,_cur_level diff_lvl.diff_lvl} );
                }
            }
            //if (uiLevel != null)
            //{
            //    data["real_win"] = real_win;
            //    uiLevel.OnLevelFinish(_curr_lvl_conf, data);
            //}
        }

        //        //S2C
        //        //关闭副本
        public void on_close_lvl_res(Variant data)
        {
            _rmv_lvl_llid(data["llid"]);
        }

        //        //		1.	消息名称：lvl_err_msg
        //        //		消息功能：返回副本相关错误信息
        //        //		cmd_id：239
        //        //		参数：{res:错误id}
        //        //		对应消息：副本相关消息
        public void on_lvl_err_msg(Variant data)
        {
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            //if (uiLevel != null)
            //{
            //    uiLevel.OnLevelError(data);
            //}
        }

        //        //		4.	消息名称：lvl_broadcas
        //        //		消息功能：副本广播消息
        //        //		cmd_id：236
        //        //		参数：{
        //        //		  tp:广播消息类型id，取值如下：
        //        //		
        //        //		  =1，玩家连续击杀消息，该值时消息结构如下：
        //        //		    cid:连续击杀角色id,
        //        //		    name:连续击杀角色名,
        //        //		    ckp:连续击杀数量,
        //        //		
        //        //		  =2，玩家连续击杀被终结消息，取该值时消息结构如下：
        //        //		    cid:击杀玩家角色id,
        //        //		    tcid:被终结玩家角色id,
        //        //		    name:击杀玩家角色id,
        //        //		    tname:被终结玩家角色id,
        //        //		    tkcp:被终结时连续击杀数量,
        //        //		  }
        //        //		对应消息：无
        public void on_lvl_broadcast_res(Variant data)
        {
            //if (uiLevel != null)
            //{
            //    this.uiLevel.OnLevelBroadcast(data);
            //}
        }


        //        //---------------------------------------------- pvp 排行榜 -------------------------------------------------
        private Variant _lvl_pvpinfo_board = null;
        private Variant _lvl_clanter_score = null;
        private Variant _transdata = null;
        private Variant _mapMonObj = new Variant();
        public Variant get_lvl_pvpinfo_board_buffer()
        {
            return _lvl_pvpinfo_board;
        }

        public Variant get_self_lvl_killInfo()
        {
            if (_lvl_pvpinfo_board != null)
            {
                uint selfcid = 0;//this.lgInGame.selfPlayer.cid;
                Variant board_info = _lvl_pvpinfo_board["infos"] as Variant;
                for (uint i = 0; i < board_info.Length; ++i)
                {
                    Variant board_t = board_info[i];
                    if (selfcid == board_t["cid"])
                    {
                        return GameTools.createGroup("rank", i, "info", board_t);//{ i rank, board_t info };
                    }
                }
            }
            return null;
        }

        public Variant get_clanter_score_buffer()
        {
            return _lvl_clanter_score;
        }

        //        //		1.	消息名称：get_lvl_pvpinfo_board
        //        //		消息功能：获取战场击杀排行信息；
        //        //		cmd_id：239
        //        //		参数：无
        //        //		对应消息：lvl_pvpinfo_board_res
        public void get_lvl_pvpinfo_board()
        {
            igLevelMsg.get_lvl_pvpinfo_board(GameTools.createGroup("tp", 1));
        }
        private void get_clanter_score()
        {
            igLevelMsg.get_lvl_pvpinfo_board(GameTools.createGroup("tp", 2));
        }
        public void get_lvl_pvpinfo_side()
        {
            igLevelMsg.get_lvl_pvpinfo_board(GameTools.createGroup("tp", 3));
        }
        public void get_lvl_towerinfo(uint mapid, uint mid, Variant transdata)
        {
            _transdata = transdata;
            igLevelMsg.get_lvl_pvpinfo_board(GameTools.createGroup("tp", 4, "mapid", mapid, "mid", mid));
        }

        //        //		5.	消息名称：lvl_pvpinfo_board_res
        //        //		消息功能：获得副本击杀排行信息，定时获取刷新，角色名称通过本地缓存获取，若没有则通过查询角色信息消息（query_ply_info）获取，获取成功后缓存至本地；
        //        //		cmd_id：237
        //        //  tp:排行积分信息类型，取值如下：
        //        //  =1，击杀排行榜，消息体如下：
        //        //    infos:排行信息数组[cid:角色id, sideid:阵营id, kp:击杀玩家数, dc:死亡次数],
        //        //  =2，帮派领地积分信息，只有在帮派领地争夺战副本中获取才有值，消息体如下：
        //        //    max_clanid:进攻方当前积分最高的帮派id,
        //        //    max_clanpt:进攻方当前最高帮派积分,
        //        //    self_clanpt:自己所在帮派的帮派积分,
        //        //		对应消息：get_lvl_pvpinfo_board
        //        //  =4, 副本剩余怪物情况
        //        //     cntleft: 怪物剩余数量

        public void lvl_pvpinfo_board_msg(Variant data)
        {
            if (data["tp"] == 1)
            {
                _lvl_pvpinfo_board = data;

                //if (uiLevel != null)
                //{
                //    uiLevel.UpdateLevelPvpinfo(data);
                //}
            }
            else if (data["tp"] == 2)
            {
                foreach (string key in data.Keys)
                {
                    _lvl_clanter_score[key] = data[key];
                }
            }
            else if (data["tp"] == 4)
            {
                if (data && data.ContainsKey("mapid"))
                {
                    _mapMonObj[data["mapid"]] = data["cntleft"];
                }

                //ui_npcDialog.MisInfoBack(_transdata, data);
                if (_transdata)
                {
                    _transdata = null;
                }
            }
        }

        public int GetMonCntLeft(int mapid)
        {
            return _mapMonObj[mapid];
        }

        //        //--------------------------------------------- 更新 击杀信息 -----------------------------------------------
        public void on_lvl_km(Variant data)
        {
            Variant lvl_conf = _curr_lvl_conf;
            uint mid = 0;
            if (!lvl_conf.ContainsKey("show_course") || lvl_conf["show_course"]._bool)
            {
                if (data.ContainsKey("mid"))
                {
                    mid = data["mid"];
                    if ((_cur_level.ContainsKey("km")) && data["sideid"] == _lvlsideid)
                    {
                        foreach (Variant k in _cur_level["km"]._arr)
                        {
                            if (k["mid"] == mid)
                            {
                                if (k["cntleft"] > 0)
                                {
                                    k["cntleft"]._int--;
                                    break;
                                }
                            }
                        }
                    }

                    if (_cur_level.ContainsKey("sidekm"))
                    {
                        foreach (Variant sidekm in _cur_level["sidekm"]._arr)
                        {
                            if (sidekm["sideid"] == data["sideid"])
                            {
                                foreach (Variant km in sidekm["km"]._arr)
                                {
                                    if (km["mid"] == mid)
                                    {
                                        if (km["cntleft"] > 0)
                                        {
                                            km["cntleft"]._int--;
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }

                    //if (uiLevel != null)
                    //{
                    //    uiLevel.OnKillCourseMon(data);
                    //}
                }


                if ((data.ContainsKey("pt")) && (_cur_level.ContainsKey("sidept")))
                {
                    //更新排行
                    get_lvl_pvpinfo_board();
                    Variant sidept = _cur_level["sidept"] as Variant;
                    foreach (Variant spt in _cur_level["sidept"]._arr)
                    {
                        if (spt["sideid"] == data["sideid"])
                        {
                            spt["pt"] = data["pt"];
                            //if (uiLevel != null)
                            //{
                            //    uiLevel.OnUpdateSidept(data);
                            //}
                            break;
                        }
                    }
                }
            }

            if (lvl_conf.ContainsKey("pvp"))
            {
                Variant pvp = lvl_conf["pvp"][0];
                if (lvl_conf.ContainsKey("cltwarid") && mid == pvp["cltwar"][0]["tar_mid"])
                {
                    _lvl_clanter_score["cntleft"]._int--;
                }
            }
        }

        public void LevelKillMonster(Variant data)
        {
            if (_cur_level != null)
            {
                if (_cur_level.ContainsKey("score_km"))
                {
                    bool exist = false;
                    foreach (Variant obj in _cur_level["score_km"]._arr)
                    {
                        if (data["mid"] == obj["mid"])
                        {
                            obj["cnt"]._int++;//上次退出前杀怪数	
                            exist = true;
                        }
                    }
                    if (!exist)
                    {
                        _cur_level["score_km"]._arr.Add(GameTools.createGroup("mid", data["mid"], "cnt", 1));
                    }
                    //uiLevel.LevelKill(null);
                }
            }

        }
        //        //		6.	消息名称：mod_lvl_selfpvpinfo
        //        //		消息功能：自己的击杀玩家、死亡次数、本次战场获得荣誉值发生变化，收到该消息后，需同步更新本地角色详细信息中的（cur_kp:当天击杀玩家数）字段，kp减原kp值得到新增kp值；
        //        //		cmd_id：238
        //        //		参数：{kp:本次副本击杀玩家数, lvl_hexp:本次副本获得荣誉值, dc:本次副本死亡次数}
        //        //		对应消息：无
        public void mod_lvl_selfpvpinfo(Variant data)
        {
            if (data.ContainsKey("kp"))
            {
                _update_kprec_kp(data["kp"]);
            }
            if (data.ContainsKey("dc"))
            {
                _update_kprec_dc(data["dc"]);
            }
            Variant marr;
            if (_curr_lvl_conf && _curr_lvl_conf.ContainsKey("map_lay"))
            {
                marr = _curr_lvl_conf["map_lay"][_lvlsideid]["m"];
            }
            else
            {
                marr = new Variant();
            }
            if (data.ContainsKey("mdc"))//本地图被击杀数
            {
                if (_cur_level)
                {
                    _cur_level["kprec"]["mdc"] = data["mdc"];
                    if (_curlay > 0 && _cur_level["kprec"]["mdc"] >= marr[_curlay]["mdc_exit"])
                    {
                        --_curlay;
                        changeLvl();
                    }
                }


            }
            if (data.ContainsKey("mkp"))
            {
                if (_cur_level)
                {
                    _cur_level["kprec"]["mkp"] = data["mkp"];
                    if (_curlay < marr.Length - 1 && _cur_level["kprec"]["mkp"] >= marr[_curlay]["mkp_enter"])
                    {
                        ++_curlay;
                        changeLvl();
                    }
                }
            }

            //if (uiLevel != null)
            //{
            //    uiLevel.UpdateSelfPvpinfo(data);
            //}
        }



        //        //----------------------------------- 更新当前副本信息 ------------------------------------		
        private Variant _kumiteply = new Variant(); // 车轮战模式 玩家信息
        private Variant _kumrs;//回合开始信息 
        private Variant _kumre;//回合结束信息 
        private Variant _curr_round_info = new Variant();

        public Variant GetCurrRoundInfo()
        {
            return _curr_round_info;
        }

        public Variant get_kumiteply()
        {
            return _kumiteply;
        }
        public Variant get_kumiteply_ply(uint cid)
        {
            if (_kumiteply.Length > 0)
            {
                foreach (Variant ply in _kumiteply._arr)
                {
                    if (ply["cid"] == cid)
                    {
                        return ply;
                    }
                }
            }
            return null;
        }

        public void on_self_die(Variant dispacher, Variant par)
        {
            if (_curr_round_info.ContainsKey("ghostcnt"))
            {
                _curr_round_info["ghostcnt"]._int--;
            }
        }

        //        //		6.	消息名称：lvl_side_info 
        //        //		消息功能：自己的副本信息变化情况；
        //        //		cmd_id：247
        //        //		参数：{lvlsideid:可选，阵营id, ghost:可选，自己的幽灵状态，=true为成为幽灵, round_change:可选，回合切换消息{ winplycids:回合胜利玩家cid数组，则根据该字段判断自己能否进入下一回合[], cur_round:新切换到的回合id,round_tm:回合结束时间（UNIX时间戳，单位秒）,ghostcnt:可选，离幽灵状态剩余复活次数,preptm: 可选，准备时间（UNIX时间戳，单位秒）}}
        //        //		对应消息：无
        public void on_lvl_side_info(Variant data)
        {
            _lvlsideid = data["lvlsideid"]._int;
            //uint selfcid = 0;//this.lgInGame.selfPlayer.cid;			


            //战场阵营
            if (data.ContainsKey ("lvlsideid")) {
                PlayerModel.getInstance().lvlsideid = data["lvlsideid"];
            }
            //帮派副本数据
            if (data.ContainsKey("sideclan"))
            {
                _set_sideclan(data["sideclan"]);
            }

            //if( data["ghost"] == true )
            //{

            //}

            if (data.ContainsKey("kumiteply"))
            {
                //更新玩家列表
                _set_kumite_plys(data["kumiteply"]);


            }

            if (data.ContainsKey("kumrs"))
            {
                _kumrs = data["kumrs"];
                _curr_round_info["preptm"] = _kumrs["preptm"];
                _curr_round_info["round_tm"] = _kumrs["rndtm"];

            }

            if (data.ContainsKey("kumre"))
            {
                _kumre = data["kumre"];
            }

            if (data.ContainsKey("round_change"))
            {
                Variant round_change = data["round_change"];

                _curr_round_info["cur_round"] = round_change["cur_round"];
                _curr_round_info["round_tm"] = round_change["round_tm"];
                _curr_round_info["preptm"] = round_change["preptm"];
                _curr_round_info["ghostcnt"] = round_change["ghostcnt"];


                Variant winplycids = round_change["winplycids"] as Variant;
                _curr_round_info["winplycids"] = winplycids;

                //if (uiLevel != null)
                //{
                //    uiLevel.OnLevelRoundChange(round_change);
                //}
            }
        }

        private void _set_kumite_plys(Variant plys)
        {
            if (_kumiteply.Length > 0)
            {
                int begin = _kumiteply.Length - 1;
                foreach (Variant new_ply in plys._arr)
                {
                    bool has = false;
                    for (uint i = (uint)begin; i >= 0; --i)
                    {
                        Variant ply = _kumiteply[(int)i];
                        if (new_ply["cid"] == ply["cid"])
                        {
                            ply["stat"] = new_ply["stat"];
                            has = true;
                            break;
                        }
                    }

                    if (!has)
                    {
                        _kumiteply._arr.Add(new_ply);
                    }
                }
            }
            else
            {
                _kumiteply = plys;
            }
        }

        //        //阵营信息
        private Variant _sideclan;
        public Variant get_sideclan(int sideid)
        {
            if (_sideclan)
            {
                return _sideclan[sideid];
            }
            return null;
        }

        private void _set_sideclan(Variant sideclan)
        {
            if (!_sideclan) _sideclan = new Variant();

            foreach (Variant o in sideclan._arr)
            {
                _sideclan[o["sideid"]] = o;
            }
        }

        //        //副本杀怪触发器相关
        private Variant _kmtrigs = null;
        public Variant get_kmtrigs()
        {
            return _kmtrigs;
        }
        public void SetKillMonTrigs(Variant kmtrigs)
        {
            if (_kmtrigs == null)
            {
                _kmtrigs = kmtrigs;
            }
            else
            {
                for (int i = _kmtrigs.Length - 1; i >= 0; i--)
                {
                    if (_kmtrigs[i]["cnt"] <= _kmtrigs[i]["kmcnt"])
                    {
                        _kmtrigs._arr.RemoveAt(i);//.splice(i,1);
                    }
                }
                foreach (Variant kmtrig in kmtrigs._arr)
                {
                    bool flag = false;
                    foreach (Variant okmtrig in _kmtrigs._arr)
                    {
                        bool equal = true;
                        foreach (string s in kmtrig.Keys)
                        {
                            if (kmtrig[s] != okmtrig[s])
                            {
                                equal = false;
                                break;
                            }
                        }
                        if (equal)
                        {
                            flag = true;
                            break;
                        }

                    }
                    if (!flag)
                    {
                        _kmtrigs._arr.Add(kmtrig);
                    }
                }
            }
            //if (uiLevel != null)
            //{
            //    uiLevel.UpdateKmtimgs();
            //}
        }
        public void OnKillTrigMon(uint km_mid)
        {
            if (_kmtrigs == null)
                return;

            foreach (Variant kmtrig in _kmtrigs._arr)
            {
                if (kmtrig["mid"] == km_mid)
                {
                    if (kmtrig["kmcnt"] >= kmtrig["cnt"])
                    {
                        kmtrig["kmcnt"] = kmtrig["cnt"];
                    }
                    else
                    {
                        ++kmtrig["kmcnt"]._int;
                    }
                    continue;
                }
            }
            //if (uiLevel != null)
            //{
            //    uiLevel.UpdateKmtimgs();
            //}

        }

     

        //        //-----------------------击杀信息
        //        // _curr_level  中属性kprec={kp，dc， ckp}  分别为/本次战场 击杀玩家数、死亡次数、当前连续击杀数
        private void _update_kprec_ckp(uint ckp)
        {
            _cur_level["kprec"]["ckp"] = ckp;
        }

        private void _update_kprec_dc(uint dc)
        {
            _cur_level["kprec"]["dc"] = dc;
        }

        private void _update_kprec_kp(uint kp)
        {
            if (_cur_level["kprec"])
            {
                int add_kp = (int)kp - _cur_level["kprec"]["kp"];
                _cur_level["kprec"]["kp"] = kp;
            }

        }


        //        //------------------------------------------- 匹配副本信息 
        private Variant _matchLvlInfo;


        //        //---------------------------------------------已创建的副本信息-----------------------------------------------		
        //已创建副本列表
        protected Variant _associate_lvls = new Variant();
        protected Variant _entercds = new Variant();

        public int get_level_cds(uint cdtp)
        {
            foreach (Variant cd in _entercds._arr)
            {
                if (cd["cdtp"] == cdtp)
                {
                    return cd["cdtm"];
                }
            }
            return 0;
        }

        //        //队伍
        public void on_ply_teamid_chang()
        {// 0 表示全部 1表示单人副本 2表示组队副本
            clear_associate_by_lmtp(2);
            get_associate_lvls(2);
        }


        public void get_associate_lvls(uint lmtp = 0, bool entercd = true)
        {
            igLevelMsg.get_associate_lvls(GameTools.createGroup("lmtp", 0, "entercd", entercd));
        }
        //        //		消息功能：获取当前已创建副本信息列表结果；
        //        //		cmd_id：243
        //        //		参数：{lvls:已创建副本信息数组[ {llid:副本实例id, end_tm:结束时间（服务器时间，unix时间戳，单位秒）, ltpid:副本配置id, diff_lvl:难度等级} ]}
        //        //		对应消息：get_associate_lvls
        public void get_associate_lvls_res(Variant data)
        {
            if (data.ContainsKey("lvls"))
            {
                _associate_lvls = data["lvls"] as Variant;
            }
            if (data.ContainsKey("entercds"))
            {
                _entercds = data["entercds"] as Variant;
            }
            //if (ui_enter != null)
            //{
            //    ui_enter.UpdateScriptInfo();
            //}
        }


        //        //获取对应的竞技场的副本实例，如果-1表示没有对应实例
        public int get_arena_llid(int arenaid)
        {
            foreach (Variant lvl in _associate_lvls._arr)
            {
                Variant lvl_data = muCCfg.svrLevelConf.get_level_data(lvl["ltpid"]);

                if (lvl_data.ContainsKey("arenaid") && lvl_data["arenaid"] == arenaid)
                    return lvl["llid"];
            }
            return 0;
        }

        private Variant _get_lvl_llid(uint ltpid)
        {
            if (_associate_lvls.Length > 0)
            {
                float tm_now = muNClt.CurServerTimeStampMS / 1000;
                for (int i = _associate_lvls.Length - 1; i >= 0; --i)
                {
                    Variant lvl = _associate_lvls[i];
                    if (lvl["end_tm"] + 120000 < tm_now)
                    {
                        _associate_lvls._arr.RemoveAt(i);//.splice(i, 1);
                        continue;
                    }

                    if (lvl["ltpid"] == ltpid)
                    {
                        return lvl;
                    }
                }
            }
            return null;
        }
        private void _add_lvl_llid(Variant data)
        {
            if (data.ContainsKey("llid"))
            {
                Variant ass = new Variant();
                //添加至缓存中
                //看缓存中是否有IID相同的副本，有的话表示这个副本已经结束。。按道理不应该存在
                for (uint i = 0; i < _associate_lvls.Count; ++i)
                {
                    ass = _associate_lvls[(int)i];
                    if (ass["llid"] == data["llid"])
                    {
                        //DebugTrace.add(DebugTrace.DTT_ERR,("check_in_lvl_res warning,liid has exist!liid:"+data["llid"]));
                        _associate_lvls._arr.RemoveAt((int)i);//.splice(i,1);
                        break;
                    }
                }
                Variant einfo = new Variant();
                einfo["llid"] = data["llid"];
                einfo["diff_lvl"] = 1;
                einfo["ltpid"] = data["ltpid"];
                Variant lvl_info = einfo;
                if (data.ContainsKey("diff_lvl"))
                {
                    lvl_info["diff_lvl"] = data["diff_lvl"];
                }
                if (null != data["cost_tp"])
                {
                    lvl_info["cost_tp"] = data["cost_tp"];
                }

                int arenaid = GetLvlArenaID(data["ltpid"]);
                int arenaexid = GetLvlArenaexID(data["ltpid"]);
                if (0 != arenaid || 0 != arenaexid)
                {   //竞技场副本创建成功,准备进入
                    for (uint j = 0; j < _associate_lvls.Count; j++)
                    {   //先清除原先创建过的副本实例
                        ass = _associate_lvls[(int)j];
                        if (ass["ltpid"] == data["ltpid"])
                        {
                            _associate_lvls._arr.RemoveAt((int)j);
                        }
                    }
                    Variant arena = muCCfg.svrLevelConf.get_arena_level((uint)arenaid);
                    Variant arenaex = muCCfg.svrLevelConf.get_arenaex_level((uint)arenaexid);

                    Variant showobj;
                    if (arena != null)
                        showobj = arena;
                    else
                        showobj = arenaex;

                    bool battle_lvl = true;
                    if (showobj != null && battle_lvl)
                    {
                        // lgPlayer player = _lgInGame.selfPlayer;	
                        //if(player.IsDie())
                        //{//在进入副本前，意外死亡，自动安全复活
                        //    _lgInGame.lgClient.session.igMapMsg.respawn(0);
                        //}

                        //                        if (arenaid != 0)
                        //                            ui_crossWar.ShowJoinMode(GameTools.createGroup("arenaid", arenaid, "llid", data["llid"]));
                        //                        else
                        //                            ui_crossWar.ShowJoinMode(GameTools.createGroup("arenaexid", arenaexid, "llid", data["llid"]));
                        //                        //_lgInGame.lgGD_Arena.SetBattleMatch(false);	//匹配成功，匹配链接，准备进入
                        ////						_main.UIMgr.interface_btn.ClearWarTime();
                        //                        if(!ui_crossWar.IsMyChange())
                        //                        {
                        //                            ui_crossWar.ClearWarTime();
                        //                        }
                        //                        ui_crossWar.ChangePrepareGoto(true);
                    }
                }

                this._associate_lvls._arr.Add(lvl_info);
            }
        }
        //从已创建副本缓存中清除
        private void _rmv_lvl_llid(uint llid)
        {
            for (int i = 0; i < _associate_lvls.Count; i++)
            {
                Variant lvl = this._associate_lvls[i];
                if (lvl["llid"] == llid)
                {
                    _associate_lvls._arr.RemoveAt(i);//.splice(i, 1);
                    break;
                }
            }
        }

        private void clear_associate_by_lmtp(uint lmtp)
        {
            for (int i = this._associate_lvls.Count - 1; i >= 0; --i)
            {
                Variant lvl = this._associate_lvls[i];
                Variant lvl_data = muCCfg.svrLevelConf.get_level_data(lvl["ltpid"]);
                if (lvl_data["lmtp"] == lmtp)
                {
                    _associate_lvls._arr.RemoveAt(i);//.splice(i, 1);
                }
            }
        }

        //        //------------------------------------------------  主角副本信息 ------------------------------------------------
        //        //剩余次数
        private Variant _lvl_infos = new Variant();
        //        //没有进入过的副本列表
        private Variant _no_entry_lvl = new Variant();

        private Variant _enters = new Variant();
        public Variant get_lvl_cnt(uint tpid)
        {
            return _lvl_infos[tpid];
        }

        public Variant get_lvlinfos()
        {
            return _lvl_infos;
        }

        public Variant get_entered_lvls()
        {
            return _enters;
        }


        /// <summary>
        /// 获得副本的剩余次数  返回 小于0 说明无限制 
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public int get_lvl_left_cnt(uint tpid)
        {
            int cnt = -1;
            Variant level_info = _lvl_infos[tpid];
            if (level_info)
            {
                if (level_info.ContainsKey("cntleft"))
                {
                    cnt = level_info["cntleft"];
                }
            }
            else
            {
                Variant level_conf = muCCfg.svrLevelConf.get_level_data(tpid);
                if (level_conf["dalyrep"] > 0)
                {	//如果配置了 dalyrep为0 表示次数无限制
                    cnt = level_conf["dalyrep"];
                }
            }
            return cnt;
        }


        /// <summary>
        /// 是否曾经进入过某个副本 
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public Boolean has_enter_lvl(int tpid)
        {
            if (_lvl_infos.ContainsKey(tpid))
            {
                return true;
            }

            //是否有标记过为没有进过的副本
            if (_no_entry_lvl.ContainsKey(tpid))
            {
                return false;
            }

            //发消息获取
            _no_entry_lvl[tpid] = tpid;
            igLevelMsg.get_lvl_cnt_info(GameTools.createGroup("ltpid", tpid));

            return false;
        }

        public void get_ply_lvlinfo()
        {
            _lvl_infos = new Variant();
            igLevelMsg.get_lvl_cnt_info(new Variant());
        }
        //        //		消息功能：获取副本信息列表，信息列表中未出现的副本则可进入次数为副本配置中指定的次数；
        //        //		参数：{lvls:副本信息数组[{ltpid: 副本配置id, cntleft:剩余可进入次数，可选，不出现为无限制, diff_lvl:当前难度等级, score:当前难度等级最高通过分数}]}
        //        //		对应消息：get_lvl_cnt_info
        public void get_lvl_info_res(Variant data)
        {
            //if (data.ContainsKey("entered") && data["entered"] != null)
            //{
            //    _enters = data["entered"];
            //}
            //if( data.ContainsKey("ltpid") )
            //{
            //    _no_entry_lvl[data["ltpid"]] = data["ltpid"];
            //    if (act != null)
            //    {
            //        act.GetLvlEnterTmRes(data["ltpid"]);
            //    }
            //    mission.RefreshMisTrack();
            //}
            //else
            //{
            //    foreach ( Variant lvlData in data["lvls"]._arr )
            //    {
            //        this._lvl_infos[ lvlData["ltpid"] ] = lvlData;
            //        if (act != null)
            //        {
            //            act.GetLvlEnterTmRes(lvlData["ltpid"]);
            //        }
            //        if( _no_entry_lvl.ContainsKey(lvlData["ltpid"]._int ) )
            //        {
            //            _no_entry_lvl.RemoveKey(lvlData["ltpid"]);
            //        }
            //    }

            //   // _lgInGame.lgClient.uiClient.AddDailyData("lgGdLevels",get_ply_lvlinfo);
            //}

        }
        public double GetLvlEnterTm(uint tpid)
        {
            if (_no_entry_lvl.ContainsKey((int)tpid))
            {
                return 0;
            }
            else if (_lvl_infos.ContainsKey((int)tpid))
            {
                if (_lvl_infos[tpid].ContainsKey("lastetm"))
                {
                    return _lvl_infos[tpid]["lastetm"]._double;
                }
                return 0;
            }
            _no_entry_lvl[tpid] = tpid;
            igLevelMsg.get_lvl_cnt_info(GameTools.createGroup("ltpid", tpid));// { tpid ltpid } );
            return -1;
        }

        private void _update_level_ctnleft(uint tpid, int cntleft)
        {
            Variant lvlData = _lvl_infos[tpid];
            if (lvlData != null)
            {
                lvlData["cntleft"] = cntleft;
            }
            else
            {
                float tm = muNClt.CurServerTimeStampMS;//_lgInGame.lgClient.session.connection.cur_server_tm;
                _lvl_infos[tpid] = GameTools.createGroup("ltpid", tpid, "cntleft", cntleft, "diff_lvl", 1, "score", 0, "lastetm", tm);
            }
        }

        private void _update_lvlinfo_on_lvlfin(uint tpid, Variant self_res)
        {
            Variant lvlInfo = _lvl_infos[tpid];
            if (lvlInfo)
            {
                if (lvlInfo["score"] < (self_res["score"]._int))
                {
                    lvlInfo["score"] = self_res["score"];
                }
                if (lvlInfo.ContainsKey("fcnt"))
                {
                    lvlInfo["fcnt"]._int++;
                }
                else
                {
                    lvlInfo["fcnt"]._int = 1;
                }
            }
            else
            {
                lvlInfo = GameTools.createGroup("ltpid", tpid, "diff_lvl", 1, "score", self_res["score"]._int, "fcnt", 1);//{tpid ltpid,1 diff_lvl,int score(self_res.score),1 fcnt};
                _lvl_infos[tpid] = lvlInfo;

                //dalyrep 为0, 则表示该副本 无次数无限制
                Variant lvl_conf = muCCfg.svrLevelConf.get_level_data(tpid);
                if (lvl_conf["dalyrep"] > 0)
                {
                    lvlInfo["cntleft"] = lvl_conf["dalyrep"] - 1;
                }
            }
            if (self_res.ContainsKey("diff_lvl"))
            {
                lvlInfo["diff_lvl"] = self_res["diff_lvl"];
            }
            if (self_res.ContainsKey("fin_diff"))
            {
                lvlInfo["fin_diff"] = self_res["fin_diff"];
            }
        }


        //        //------------------------------------ 副本奖励 
        //        //副本通关奖励相关	

        private int _curltpid = 0;
        //        //		消息功能：在副本胜利结束情况下，可调用该接口获取副本奖励；
        //        //		cmd_id：245
        //        //		参数：{}
        //        //		对应消息：无
        public void get_curr_lvl_prize()
        {
            if (_curltpid > 0)
            {
                igLevelMsg.get_lvl_prize(GameTools.createGroup("ltpid", _curltpid));// {_curltpid ltpid} );
            }
        }
        //        //		消息功能：角色领取副本奖励结果，将广播给副本中所有用户，包括自己
        //        //		cmd_id：246
        //        //		参数：{cid: 角色id, tpid:道具配置id, cnt:道具数量，可选}
        //        //		对应消息：get_lvl_prize
        //        //				
        public void lvl_get_prize_res(Variant data)
        {
            _remove_prize(data["ltpid"], _current_diff);

            _awdData = data;

            //  _lgInGame.delayDoMgr.AddDelayDo(_onGetLevelAwd, 2000);

        }

        private Variant _awdData;
      
        private Variant _lvlprizes = new Variant();
        public Variant GetPrize()
        {
            return _lvlprizes;
        }
        private void _add_lvlprize(Variant prize)
        {
            this._lvlprizes._arr.Add(prize);
        }
        private void _remove_prize(uint ltpid, uint diff_lvl)
        {
            for (int i = 0; i < _lvlprizes.Length; ++i)
            {
                Variant p = _lvlprizes[i];
                if (p["ltpid"] == ltpid)
                {
                    if (diff_lvl == 0 || p["diff_lvl"] == diff_lvl)
                    {
                        _lvlprizes._arr.RemoveAt(i);//.splice(i, 1);
                        break;
                    }
                }
            }
        }


        //        //------------------------------------------------  副本 相关 -------------------------------------------
        //        //		5.	消息名称：lvl_res
        //        //		cmd_id：233
        public void on_lvl_res(Variant data)
        {
            switch (data["tp"]._int)
            {
                case 1:
                    {
                        _update_lvlinfo_on_lvlfin(data["ltpid"], GameTools.createGroup("score", 0));

                        if (muCCfg.svrLevelConf.IsLevelHasItemPrize(data["ltpid"]))
                        {   //是通关奖励类型副本
                            this._add_lvlprize(GameTools.createGroup("ltpid", data["ltpid"], "diff_lvl", data["diff_lvl"]));//{data ltpid.ltpid,data diff_lvl.diff_lvl} );
                        }
                    }
                    break;
                case 2:
                    {
                        //爬塔记录
                        getRecordRes(data);
                    }
                    break;
                case 3:
                    {

                    }
                    break;
                case 8:
                  
                    break;
                case 9:
                    {//买buff的返回
                        if (data.ContainsKey("cost_gold"))//消耗金币
                        {
                            //this._lgInGame.lgGD_Gen.sub_gold( data.cost_gold );
                        }
                        if (data.ContainsKey("cost_yb"))//消耗元宝
                        {
                            // this._lgInGame.lgGD_Gen.sub_yb( data.cost_yb );
                        }

                        //if (uiLevel != null)
                        //{
                        //    uiLevel.OnBuyLevelBuffRes(data);
                        //}
                    }
                    break;
            }
        }
        //        //古战场消耗
        private Variant _hadCost;
       
        public Variant GetCurLvlCost()
        {
            return _hadCost;
        }
        //        //------------------------------------------------  攻城战 相关 -------------------------------------------
        private Dictionary<uint, Variant> _clanTerritorys = new Dictionary<uint, Variant>();//领地信息
        private Variant _loadTerState = new Variant();//领地加载状态	

        /// <summary>
        /// 本盟是否占有该领地
        /// </summary>
        /// <param name="clteid"></param>
        /// <returns></returns>
        public uint IsClanHasClte(uint clteid)
        {
            Variant chanTer = _clanTerritorys[clteid];
            return (chanTer != null) ? chanTer["clanid"]._uint : 0;
        }

        /// <summary>
        /// 该领地是否属于无人占领
        /// </summary>
        /// <param name="clteid"></param>
        /// <returns></returns>
        public Boolean IsCltNotOwn(uint clteid)
        {
            if (_clanTerritorys != null && _clanTerritorys[clteid] != null)
            {
                Variant chanTer = _clanTerritorys[clteid];
                return (0 == chanTer["clanid"]._int);
            }
            return false;
        }
        public void GetChanTerDalyAwd(uint clteid)
        {

        }
        public void OnClanError(int clanid)
        {
            if (_clanTerritorys.Count != 0)
            {
                foreach (Variant clan in _clanTerritorys.Values)
                {
                    if (clan["clanid"] == clanid)
                    {
                        clan["clanid"] = 0;
                        break;
                    }
                }
            }
        }
        private void initClanTerritory(Variant data)
        {
            data.RemoveKey("tp");//删除tp字段	
            _loadTerState.RemoveKey(data["clteid"]);//删除加载状态

            _clanTerritorys[data["clteid"]] = data;
            //战盟不存在情况
            // _lgInGame.lgGD_clans.get_claninfo_by_clanid(data.clanid,function (int clanid, Variant info):void{} );
            ////if (uiLevel != null)
            ////{
            ////    uiLevel.OnClanTerrInfoChange(data["0clteid"]);
            ////}

            //if(data["clanid"] && lgInGame.selfPlayer.netData.clanid == data["clanid"])
            //{	//上线时，自动去领取一次攻城战奖励
            //    foreach (Variant info in data["showinfo"]._arr)
            //    {
            //        if(info["cid"] == lgInGame.selfPlayer.cid && data["awdtm"] > 0)
            //        {
            //            igLevelMsg.get_clanter_info(GameTools.createGroup(
            //            "tp",2,"clteid",data["clteid"]));//{ 2 tp, data clteid.clteid  } );
            //            break;
            //        }
            //    }				
            //}
        }

        private Boolean adjustClanTerrInfo(Variant data)
        {
            Variant chanTer = _clanTerritorys[data["clteid"]];
            if (chanTer != null)
            {
                data.RemoveKey("tp");//删除tp字段
                foreach (string key in data.Keys)
                {
                    chanTer[key] = data[key];
                }
                return true;
            }
            return false;
        }

        //        //------------------------------ 攻城战  防御血量相关
        private Boolean setClanTerrBuildHPInfo(Variant data)
        {
            Variant terInfo = _clanTerritorys[data["clteid"]];
            if (terInfo != null)
            {
                terInfo["buildhp"] = data;

                return true;
            }
            return false;
        }
        private Variant getClanTerrBuildHPInfo(uint clteid)
        {
            Variant terInfo = _clanTerritorys[clteid];
            if (terInfo != null)
            {
                return terInfo["buildhp"];
            }
            return null;
        }
        private Boolean updateClanTerrBuildHP(Variant data)
        {
            Variant buildHpInfo = getClanTerrBuildHPInfo(data["clteid"]);
            if (buildHpInfo)
            {
                foreach (Variant mapInfo in buildHpInfo["mon_hp_pers"]._arr)
                {
                    if (mapInfo["mapid"] == data["mapid"])
                    {
                        foreach (Variant hpInfo in mapInfo["hp_pers"]._arr)
                        {
                            if (hpInfo["mid"] == data["mid"])
                            {
                                hpInfo["hp_per"] = 100;
                                return true;
                            }
                        }
                        break;
                    }
                }
            }
            return false;
        }
        //        //------------------------------ 攻城战  申请相关
        private Boolean setClanTerrReqs(Variant data)
        {
            Variant terInfo = _clanTerritorys[data["clteid"]];
            if (terInfo)
            {
                terInfo["war_reqs"] = data["war_reqs"];
                return true;
            }
            return false;
        }
        private Variant getClanTerrReqs(uint clteid)
        {
            Variant terInfo = _clanTerritorys[clteid];
            if (terInfo)
            {
                return terInfo["war_reqs"];
            }
            return null;
        }
        private Boolean addClanTerrReqs(Variant data)
        {
            Variant requests = getClanTerrReqs(data["clteid"]);
            if (requests)
            {
                requests._arr.Add(data["clanid"]);
                return true;
            }
            return false;
        }

        public void on_clanter_res(Variant data)
        {
            switch (data["tp"]._int)
            {
                case 1:
                    {
                        initClanTerritory(data);
                    }
                    break;
                case 2:
                    {
                        uint awdtm = data["awdtm"]._uint;
                        if (awdtm > 0)
                        {//可以领 则立即领
                            igLevelMsg.get_clanter_info(GameTools.createGroup(
                            "tp", 2, "clteid", data["clteid"]));//{ 2 tp, data clteid.clteid} );
                        }
                    }
                    break;
                case 3:
                    {
                        //if (adjustClanTerrInfo(data))
                        //{
                        //    if (uiLevel != null)
                        //    {
                        //        uiLevel.OnClanTerrInfoChange(data["clteid"]);
                        //    }
                        //}
                    }
                    break;
                case 5:
                    {//获取攻城战中建筑的血量的百分比信息返回
                        //if (setClanTerrBuildHPInfo(data))
                        //{
                        //    if (uiLevel != null)
                        //    {
                        //        uiLevel.UpdateAllClanTerrBuildHP(data["clteid"]);
                        //    }
                        //}
                    }
                    break;
                case 6:
                    {//修理攻城战中建筑返回
                        //if ((1 == data["res"]) || (-1461 == data["res"]))
                        //{
                        //    if (updateClanTerrBuildHP(data))
                        //    {
                        //        if (uiLevel != null)
                        //        {
                        //            uiLevel.UpdateClanTerrBuildHP(data["clteid"], data);
                        //        }
                        //    }
                        //}
                        break;
                    }
                case 7:
                    {//申请攻城的信息
                        //if (setClanTerrReqs(data))
                        //{
                        //    if (uiLevel != null)
                        //    {
                        //        uiLevel.UpdateClanTerrReqInfo(data["clteid"], data["war_reqs"]);
                        //    }
                        //}
                    }
                    break;
                case 8:
                    {//申请攻城的信息
                        //if (addClanTerrReqs(data))
                        //{
                        //    if (uiLevel != null)
                        //    {
                        //        uiLevel.AddClanTerrReq(data["clteid"], data["clanid"]);
                        //    }
                        //}
                    }
                    break;
            }
            if (_clanInfoCall != null)
            {
                _clanInfoCall(data);
                _clanInfoCall = null;
            }
        }

        private Action<Variant> _clanInfoCall;
        //        //-------------------C2S
        public Variant GetClanTerrInfo(uint clteid, Action<Variant> onfin = null)
        {
            Variant ret = _clanTerritorys[clteid];
            if (ret == null)
            {
                if (_loadTerState.ContainsKey((int)clteid))
                {
                    return null;
                }
                if (onfin != null)
                {
                    _clanInfoCall = onfin;
                }
                _loadTerState[clteid] = true;
                igLevelMsg.get_clanter_info(GameTools.createGroup(
                "tp", 1, "clteid", clteid));//{ 1 tp, clteid clteid } );
            }
            else
            {
                if (onfin != null)
                {
                    onfin(ret);
                }
                _clanInfoCall = null;
            }
            return ret;
        }
        /// <summary>
        /// 领取攻城战奖励
        /// </summary>
        /// <param name="clteid"></param>
        public void GetClanTerrAwd(int clteid)
        {
            igLevelMsg.get_clanter_info(GameTools.createGroup(
            "tp", 4, "clteid", clteid));//{ 4 tp, clteid clteid} );
        }
        /// <summary>
        /// 获取攻城战中建筑的血量的百分比信息
        /// </summary>
        /// <param name="clteid">帮派领地配置id</param>
        /// <returns></returns>
        public Variant GetClanTerrBuildHPInfo(uint clteid)
        {
            Variant buildInfo = this.getClanTerrBuildHPInfo(clteid);
            if (buildInfo)
            {
                return buildInfo;
            }

            igLevelMsg.get_clanter_info(GameTools.createGroup(
            "tp", 5, "clteid", clteid));//{ 5 tp, clteid clteid } );
            return null;
        }
        /// <summary>
        /// 修理攻城战中的建筑
        /// </summary>
        /// <param name="clteid"></param>
        /// <param name="mapid"></param>
        /// <param name="mid"></param>
        public void RepairClanTerrBuild(uint clteid, uint mapid, uint mid)
        {
            igLevelMsg.get_clanter_info(GameTools.createGroup(
            "tp", 6, "clteid", clteid, "mapid", mapid, "mid", mid));//{ 6 tp, clteid clteid, mapid mapid, mid mid} );
        }
        /// <summary>
        /// 获取申请攻城的帮派列表
        /// </summary>
        /// <param name="clteid"></param>
        /// <returns></returns>
        public Variant GetClanTerrRequests(int clteid)
        {
            Variant reqs = this.getClanTerrReqs((uint)clteid);
            if (reqs != null)
            {
                return reqs;
            }

            igLevelMsg.get_clanter_info(GameTools.createGroup(
            "tp", 7, "clteid", clteid));//{ 7 tp, clteid clteid} );
            return null;
        }
        /// <summary>
        /// 申请攻城
        /// </summary>
        /// <param name="clteid"></param>
        public void RequestClanTerr(int clteid)
        {
            igLevelMsg.get_clanter_info(GameTools.createGroup(
            "tp", 8, "clteid", clteid));//{ 8 tp, clteid clteid} );
        }

        //        //攻城战是否开启
        public Boolean is_city_war(uint clteid, uint lvltpid)
        {
            Variant terrConfig = muCCfg.svrLevelConf.get_clan_territory(clteid);
            if (terrConfig != null)
            {
                if (terrConfig["tp"] == 2)
                {
                    if (terrConfig["warlvl"]["tpid"] == lvltpid)
                    {   //进入领地争夺副本
                        float tmnow = muNClt.CurServerTimeStampMS;//_lgInGame.lgClient.session.connection.cur_server_tm;
                        if (ConfigUtil.check_tm(tmnow, terrConfig["warlvl"]["tmchk"]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //        //-----------------------首席（天下第一）      start-------------------------------------------------------------------------
        private Variant _carrchief_info = new Variant();
        private Variant _carrchief_info_cb = new Variant();
        public void clear_carrchief_npc_data(uint ltpid)
        {
            Variant lvl_data = muCCfg.svrLevelConf.get_level_data(ltpid);
            int carrchief = lvl_data["carrchief"];
            if (_carrchief_info.ContainsKey(carrchief))
            {
                _carrchief_info.ContainsKey(carrchief);
                get_carrchief_info((uint)carrchief);    //更新数据
            }
        }
        public void get_carrchief_npc_data(uint carr, Action<Variant> on_fin)
        {
            //Variant data = GetCarrchiefData(carr);
            //if (data == null)
            //{
            //    foreach (Variant obj in _carrchief_info._arr)
            //    {
            //        if (obj["carr"] == carr && on_fin == obj["on_fin"])
            //        {
            //            return;
            //        }
            //    }
            //    _carrchief_info_cb._arr.Add(GameTools.createGroup(
            //        "carr", carr, "on_fin", on_fin));//{carr carr, on_fin on_fin});
            //}
            //else
            //{
            //    on_fin(data);
            //}			
        }
        public Variant GetCarrchiefData(uint carr)
        {
            Variant data = _carrchief_info[carr];
            if (data == null)
            {
                _carrchief_info[carr] = new Variant();
                get_carrchief_info(carr);
                return null;
            }
            else
            {
                return data;
            }
        }
        public void OnMapChangedFin()
        {
            _carrchief_info_cb._arr.Clear();
        }
        private Variant _carrchief_npc_id = new Variant();
        public int get_carrchief_npc_id(uint carr)
        {
            if (_carrchief_npc_id.ContainsKey((int)carr))
                return _carrchief_npc_id[carr];

            int nid = muCCfg.svrNpcConf.get_carrchief_npc((int)carr);
            _carrchief_npc_id[carr] = nid;
            return nid;
        }


        //        //		服务器->客户端消息（S2C）
        //        //		2.	消息名称：carrchief
        //        //		消息功能：首席大弟子相关消息
        //        //		cmd_id：234
        //        //		参数：{
        //        //		  tp:首席大弟子消息类型，取值如下：
        //        //		  =1，获取首席大弟子显示信息，该值时消息结构如下：
        //        //		    cid:角色id,
        //        //		    level:角色等级,
        //        //		    carr:角色职业,
        //        //		    sex:角色性别,
        //        //		    wpnflvl:武器锻造等级,
        //        //		    eqpflvl:身上装备最大锻造等级,
        //        //		    stnlvl:身上装备最大宝石等级,
        //        //		    eqp:身上装备配置id数组[],
        //        //		    name:角色名称,
        //        //		  =2，获取冠军奖励结果消息：
        //        //		    carrchieftm:当前获得首席大弟子副本胜利时间，从该时间起经过配置中carrc_awd_expire_tm秒之后将不能领取奖励，=0则表示需清除奖励时间记录，同时需将carrchief置为false，即没有奖励了,
        //        //		  }
        //        //		对应消息：carrchief
        public void on_npc_shop(Variant data)
        {

            debug.Log("NPCSHOP============" + data.dump());
            //int res = data["res"];
            //if (res < 0)
            //    Globle.err_output(res);
            //switch (res)
            //{
            //    case 1: onRefresh(data); break;
            //    case 2: onBuy(data); break;
            //    case 3: onFloat(data); break;
            //    default:
            //        break;
            //}

        }
        //        //		客户端->服务器消息（C2S）
        //        //		1.	消息名称：carrchief
        //        //		消息功能：首席大弟子相关消息；
        //        //		cmd_id：234
        //        //		参数：{
        //        //		  tp:竞技场消息类型，取值如下：
        //        //		    =1，获取首席大弟子显示信息，消息结构如下：
        //        //		     carr:待获取信息首席大弟子职业id,
        //        //		    =2，获取首席大弟子冠军奖励,
        //        //		}
        //        //		对应消息：carrchief
        public void get_carrchief_info(uint carr)
        {
            igLevelMsg.get_carrchief_info(GameTools.createGroup(
            "tp", 1, "carr", carr));//{1 tp,carr carr});
        }
        public void get_carrchief_award()
        {
            igLevelMsg.get_carrchief_info(GameTools.createGroup(
            "tp", 2));//{2 tp});
        }
        //        //-----------------------首席（天下第一）      end-------------------------------------------------------------------------

        /// <summary>
        /// 获得副本的剩余次       * 返回 -100 说明无限制
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public int get_lvl_residue_cnt(uint tpid)
        {
            int cnt = 0;
            Variant level_info = get_lvl_cnt(tpid);
            Variant level_data = muCCfg.svrLevelConf.get_level_data(tpid);
            if (level_info != null)
            {
                if (level_info.ContainsKey("cntleft"))
                {
                    cnt = level_info["cntleft"]._int;
                    if ((0 == cnt) && (level_data != null) && (0 == level_data["dalyrep"]))
                    {   //如果次数为0 而且 dalyrep 也为0, 则表示该副本 无次数无限制
                        cnt = -100;
                    }
                }
                else
                {   //无次数限制
                    cnt = -100;
                }
            }
            else if (level_data != null)
            {
                if (level_data.ContainsKey("dalyrep") && level_data["dalyrep"] > 0)
                {   //如果配置了 dalyrep为0 表示次数无限制
                    cnt = level_data["dalyrep"];
                }
                else
                {
                    cnt = -100;
                }
            }
            return cnt;
        }

        /// <summary>
        /// 根据副本TPID获取副本实例结构，如果为null则表示无实例 
        /// </summary>
        /// <param name="ltpid"></param>
        /// <returns></returns>
        public Variant get_lvl_llid(uint ltpid)
        {
            if (_associate_lvls.Length == 0)
                return null;

            float tm_now = (this.g_mgr.g_netM as muNetCleint).CurServerTimeStampMS / 1000;// _lgingame.lgclient.session.connection.cur_server_tm / 1000;
            for (uint i = 0; i < _associate_lvls.Count; i++)
            {
                Variant lvl = _associate_lvls[(int)i];

                float end_tm = lvl.ContainsKey("end_tm") ? lvl["end_tm"]._float : 0;
                if (0 != end_tm && end_tm + 120000 < tm_now)
                {
                    _associate_lvls._arr.RemoveAt((int)i);//.splice(i,1);
                    i--;
                    continue;
                }

                if (lvl["ltpid"] == ltpid)
                {
                    return lvl;
                }
            }

            return null;
        }
        //        //是否在攻城战副本
        public Variant GetCitywarInfo()
        {
            if (_curr_lvl_conf && (_curr_lvl_conf.ContainsKey("pvp")))
            {
                Variant pvp = _curr_lvl_conf["pvp"];
                if (pvp && pvp[0] && (pvp[0].ContainsKey("cltwar")))
                {
                    return pvp[0]["cltwar"][0];
                }
            }
            return null;
        }

        public void GetLvlBuff(int id, int cost_tp)
        {
            igLevelMsg.get_lvl_info(GameTools.createGroup(
    "tp", 9, "id", id, "cost_tp", cost_tp));//{9 tp,id id,cost_tp cost_tp});
        }
        //        //----------------------爬塔start----------------------------------
        private Variant _lvlmisData;
        private Variant _record = new Variant();//首破 最快
        private Variant _loading = new Variant();
        public Variant GetRecord(uint fintp, int begin_idx, int end_idx)
        {
            string type = fintp + "_" + begin_idx + "_" + end_idx;
            float now_tm = muNClt.CurServerTimeStampMS;//.lgClient.session.connection.cur_server_tm;
            if (!(_loading.ContainsKey(type)))
            {
                _loading[type] = 0;
            }

            if (_loading[type] < now_tm)
            {
                _loading[type] = now_tm + 60000;
                GetLvlmisRecord(fintp, begin_idx, end_idx);
                return null;
            }
            else
            {
                Variant arr = _record[fintp];
                if (arr == null)
                {
                    _record[fintp] = new Variant();
                    return null;
                }

                Variant tmp = new Variant();
                for (int j = begin_idx; j < end_idx; ++j)
                {
                    tmp._arr.Add(arr[j]);
                }
                return tmp;
            }
        }
        private void getRecordRes(Variant data)
        {
            int fintp = data["fintp"];
            Variant arr;
            if (_record[fintp])
            {
                arr = _record[fintp];
            }
            else
            {
                arr = new Variant();
            }
            for (int i = 0; i < data["infos"].Length; i++)
            {
                arr[i + data["begin_idx"]] = data["infos"][i];
            }
            _record[fintp] = arr;
            //todo
            //ui_scriptAct.refreshTowerRecord(arr);
        }

        /// <summary>
        /// 获取侠客行首破
        /// </summary>
        /// <param name="fintp">信息类型，=0为首破、=1为最佳通关</param>
        /// <param name="begin_idx">起始索引，从0开始计算,</param>
        /// <param name="end_idx">结束索引（含）</param>
        private void GetLvlmisRecord(uint fintp, int begin_idx, int end_idx)
        {
            igLevelMsg.get_lvl_info(GameTools.createGroup(
            "tp", 2, "fintp", fintp, "begin_idx", begin_idx, "end_idx", end_idx));//{2 tp,fintp fintp,begin_idx begin_idx,end_idx end_idx});
        }
        //        //领取侠客行任务完成奖励
        //public void GetLvlmisPrize(int lmisid)
        //{
        //    //this._lgInGame.lgClient.session.igMissMsg.GetLvlmisPrize(lmisid);
        //    igMissionMsg.GetLvlmisPrize(lmisid);
        //}
        //获取侠客行任务相关信息
        private void GetLvlmisInfo()
        {
            igLevelMsg.GetLvlmisInfo();
        }
        //        //侠客行任务发生变化
        //public void lvlmis_changed(Variant data)
        //{
        //    int lmisid = data["lmisid"];
        //    switch (data["tp"]._int)
        //    {
        //        case 1:
        //            Variant misline = _lvlmisData["misline"];
        //            Variant levels = muCCfg.svrLevelConf.Getlvlmis();
        //            int line = 0;
        //            foreach (string curmisline in levels.Keys)
        //            {
        //                if (curmisline == data["lmisid"])
        //                {
        //                    line = levels[curmisline]["line"];
        //                    break;
        //                }
        //            }
        //            bool inflag = false;
        //            foreach (Variant obj in misline._arr)
        //            {
        //                if (line == obj["lineid"])
        //                {
        //                    obj["lmisid"] = lmisid;
        //                    inflag = true;
        //                    break;
        //                }
        //            }
        //            if (!inflag)
        //            {
        //                misline._arr.Add(GameTools.createGroup(
        //                "lienid", line, "lmisid", lmisid));//{line lineid,lmisid lmisid});
        //            }
        //            //_lgInGame.lgGD_miss.lvlMisChange();
        //            //todo更新任务
        //            ui_scriptAct.refreshTowerPage();
        //            break;
        //        case 2:
        //            break;
        //    }
        //}

        //        //获取侠客行任务信息结果
        //        /**
        //         * misline:任务线数组    lineid:任务线id,
        //         * lmisid:任务线当前最新完成任务配置id,  
        //         * prizes:已通关任务数组，包含有奖励的任务以及任务线=0（line=0）的任务
        //         * lmisid:可领取奖励侠客行任务id, 
        //         * prize: =0表示有奖励,=1表示无奖励
        //         */
        /// <summary>
        ///  //         * misline:任务线数组    lineid:任务线id,
        ///         * lmisid:任务线当前最新完成任务配置id,  
        ///         * prizes:已通关任务数组，包含有奖励的任务以及任务线=0（line=0）的任务
        ///         * lmisid:可领取奖励侠客行任务id, 
        ///         * prize: =0表示有奖励,=1表示无奖励
        ///         */
        /// </summary>
        /// <param name="data"></param>
        public void get_lvlmis_res(Variant data)
        {
            _lvlmisData = data;
            //todo
            //this.muLgClt.g_missionCT.lvlMisChange();//_lgInGame.lgGD_miss.lvlMisChange();
        }

        public Variant get_lvlmis_data()
        {
            if (_lvlmisData == null)
            {
                _lvlmisData = new Variant();
                GetLvlmisInfo();
            }
            return _lvlmisData;
        }

        //        //----------------------爬塔  end----------------------------------
        public void GetTerritory()
        {
            uint clanid = 0;//lgInGame.selfPlayer.netData.clanid;

            if (clanid != 0)
            {

            }

            Variant ter = muCCfg.svrLevelConf.get_clan_territory(2);
            if (ter != null)
            {
                igLevelMsg.get_clanter_info(GameTools.createGroup(
                "tp", 1, "clteid", ter["id"]));//{ 1 tp, ter clteid.id } );
            }
        }

        //        //----------------------------------------竞技场相关 start-------------------------------
        private int _curWaitTm;
        //        
        /// <summary>
        /// 是否在等待竞技场开始阶段
        /// </summary>
        /// <returns></returns>
        public Boolean IsWaitingStart()
        {
            return in_level && _curWaitTm != 0;
        }

        public void SetCurWaitTm(int tm)
        {
            _curWaitTm = tm;
        }

        public void EnterArenaLvl(Variant data)
        {//进入竞技场副本
            int llid;
            _enter_lvl_npc = 0;
            if (data.ContainsKey("arenaid") && data["arenaid"])
            {
                //报名竞技场副本成功
                Variant arena = muCCfg.svrLevelConf.get_arena_level(data["arenaid"]);
                if (arena && arena["battle_lvl"])
                {
                    llid = data.ContainsKey("llid") ? data["llid"]._int : get_arena_llid(data["arenaid"]);
                    uint ltpid = 0; //副本的ID，用来加载副本的地图和剧情的
                    enter_lvl((uint)llid, ltpid, 0, true);
                }
            }
            else if (data.ContainsKey("arenaexid") && data["arenaexid"])
            {
                //报名竞技场副本成功
            }
        }

        public int GetLvlArenaID(uint ltpid)
        {
            if (0 != ltpid)
            {
                Variant lvl_data = muCLientConfig.instance.svrLevelConf.get_level_data(ltpid);//_lgingame.lgclient.svrgameconfig.svrlevelconf.get_level_data(ltpid);
                if (lvl_data.ContainsKey("arenaid"))
                {
                    return lvl_data["arenaid"];
                }
            }
            return 0;
        }

        public int GetLvlArenaexID(uint ltpid)
        {
            if (0 != ltpid)
            {
                Variant lvl_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrLevelConf.get_level_data(ltpid); //_lgInGame.lgClient.svrGameConfig.svrLevelConf.get_level_data(ltpid);
                if (lvl_data.ContainsKey("arenaexid"))
                {
                    return lvl_data["arenaexid"];
                }
            }
            return 0;
        }
        //        //是否在跨服服务器的副本
        public Boolean IsInBattleSrvLvl()
        {
            if (in_level && _curltpid != 0)
            {
                int arenaid = GetLvlArenaID((uint)_curltpid);
                if (arenaid != 0)
                {	//报名竞技场副本成功
                    Variant arena = muCCfg.svrLevelConf.get_arena_level((uint)arenaid);
                    if (arena.ContainsKey("battle_lvl"))
                    {
                        return arena["battle_lvl"];
                    }
                }
            }
            return false;
        }
        //        //是否在多人跨服的副本
        public Boolean IsInMoreBattleSrvLvl()
        {
            if (0 == _curltpid)
                return false;

            Variant lvl_data = muCCfg.svrLevelConf.get_level_data((uint)_curltpid);
            if (!lvl_data)
                return false;
            if (lvl_data.ContainsKey("arenaexid"))
            {
                return true;
            }

            return false;
        }
        //        /////////////////////////////副本相关开////////////////////////////////////
        //        //		消息名称：battle_do_res
        //        //		消息功能：跨服战相关消息
        //        //		cmd_id：230
        //        //		参数：{
        //        //			tp:子消息类型，取值如下：
        //        //			=1,  连接跨服战返回
        //        //				res:  错误码
        //        //			=2，获取上次战场消耗返回  （服务器处理， 客户端无需理会）
        //        //			yb:	
        //        //			=4,   检测llid有效时返回  （服务器处理， 客户端无需理会）
        //        //			llid: 副本实例ID
        //        //			in_lvl:是否在副本  bool
        //        //			
        //        //			=5，角色属性刷新
        //        //			hp: 
        //        //			mp:
        //        //			dp:
        //        //			等角色属性		
        //        //		} 
        //        //		对应消息：battle_do
        public void on_battle_do_res(Variant data)
        {
            switch (data["tp"]._int)
            {
                case 5:
                    {
                        if (data.ContainsKey("pinfo"))
                        {
                            //						
                        }
                    }
                    break;
            }
        }

        //        /**  c2s
        //         *=1, 连接跨服战
        //              0 conn 为断开连接 1为建立连接
        //        =2，获取上次战场消耗 （服务器处理， 客户端无需理会）

        //        =3，发送角色基本信息 （服务器处理， 客户端无需理会）
        //            whcid:  角色cid
        //            level: 角色等级
        //            carrlvl:角色职业等级
        //            vip: 角色vip
        //            camp:角色阵营
        //            gm:
        //             guid_gm g_gm
        //            name:角色名字
        //        =4,  检测llid是否有效，及玩家是否在副本中 （服务器处理， 客户端无需理会）
        //           llid：副本实例ID

        //         * 
        //         */
        public void battle_do()
        {

        }

        //        //------------------------------副本共享次数 start--------------------------
        private Variant _lvlshare = new Variant();
        public void SetLvlShare(Variant lvlshare)
        {
            _lvlshare = lvlshare;
        }

        public Variant GetLvlShareByTp(uint tp)
        {
            foreach (Variant obj in _lvlshare._arr)
            {
                if (obj["tp"] == tp)
                {
                    return obj;
                }
            }
            return null;
        }

        public void RefreshLvlShare(Variant data)
        {
            bool hasPush = false;
            foreach (Variant obj in _lvlshare._arr)
            {
                if (obj["tp"] == data["tp"])
                {
                    hasPush = true;
                    GameTools.assignProp(obj, data);
                    break;
                }
            }
            if (!hasPush)
            {
                _lvlshare._arr.Add(data);
            }
        }
        //        //------------------------------副本共享次数 end--------------------------


        //port-------------------------------------------------------------
        private InGameLevelMsgs igLevelMsg
        {
            get
            {
                return (this.g_mgr.g_netM as muNetCleint).
                    getObject(OBJECT_NAME.MSG_LEVEL) as InGameLevelMsgs;
            }
        }
        //private InGameMissionMsgs igMissionMsg
        //{
        //    get
        //    {
        //        return muNClt.
        //            getObject(OBJECT_NAME.MSG_MISSION) as InGameMissionMsgs;
        //    }
        //}
        private muCLientConfig muCCfg
        {
            get
            {
                return this.g_mgr.g_gameConfM as muCLientConfig;
            }
        }

        private muNetCleint muNClt
        {
            get
            {
                return this.g_mgr.g_netM as muNetCleint;
            }
        }
        private muLGClient muLgClt
        {
            get
            {
                return this.g_mgr.g_gameM as muLGClient;
            }
        }
        //private LGIUILevel uiLevel
        //{
        //    get
        //    {
        //        //LGUILevel level = m_mgr.getLGUI(UIName.UI_LEVEL) as LGUILevel;
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_LEVEL) as LGIUILevel;
        //    }
        //}
        //private LGIUICityWarFace ui_crossWar
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_CITYWARFACE) as LGIUICityWarFace;
        //    }
        //}
        //private LGIUIMainUI lgmainui
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.LGUIMainUIImpl) as LGIUIMainUI;
        //    }
        //}
        ////private LGIUINpcDialog ui_npcDialog
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.LGUINpcDialogImpl) as LGIUINpcDialog;
        //    }
        //}
        //private LGIUIScriptEnter ui_enter
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_SCRIPTENTER) as LGIUIScriptEnter;
        //    }
        //}
        //private LGIUIActivity act
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_ACTIVITY) as LGIUIActivity;
        //    }
        //}
      
        //private LGIUIScriptActivity ui_scriptAct
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_SCRIPTACTIVITY) as LGIUIScriptActivity;
        //    }
        //}
    }
}