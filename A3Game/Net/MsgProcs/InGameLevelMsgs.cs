using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    public class InGameLevelMsgs : MsgProcduresBase
    {
        public static InGameLevelMsgs instance;
        public InGameLevelMsgs(IClientBase m)
            : base(m)
        {
            instance = this;
        }
        public static InGameLevelMsgs create(IClientBase m)
        {
            return new InGameLevelMsgs(m);
        }
        override public void init()
        {
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_GET_LVLMIS_RES, get_lvlmis_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CLANTER_RES, clanter_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_RES, lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CARRCHIEF, carrchief.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_BROADCAST, lvl_broadcast.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_ON_ARENA, on_arena.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, lvl_pvpinfo_board_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_MOD_LVL_SELFPVPINFO, mod_lvl_selfpvpinfo.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_ERR_MSG, lvl_err_msg.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CHECK_IN_LVL_RES, check_in_lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CREATE_LVL_RES, create_lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_ENTER_LVL_RES, enter_lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES, get_associate_lvls_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_GET_LVL_INFO_RES, get_lvl_info_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_FIN, lvl_fin.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_GET_PRIZE_RES, lvl_get_prize_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_SIDE_INFO, lvl_side_info.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CLOSE_LVL_RES, close_lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_KM, lvl_km.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LEAVE_LVL_RES, leave_lvl_res.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_ON_BATTLE_DO_RES, on_battle_do_res.create);
        }
        public void get_clanter_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CLANTER_RES, data);
        }

        public void get_lvl_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_RES, data);
        }

        public void get_carrchief_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CARRCHIEF, data);
        }

        public void get_arena_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_ON_ARENA, data);
        }

        public void get_lvl_pvpinfo_board(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_ERR_MSG, data);
        }
        public void check_in_lvl(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CHECK_IN_LVL_RES, data);
        }

        public void create_lvl(Variant data)
        {
            //这里的接口其实已经没有用了，要废弃掉了
            sendRPC(PKG_NAME.C2S_CREATE_LVL_RES, data);
        }

        public void enter_lvl(Variant data)
        {
            debug.Log("SCENE_LEVEL ---- 收到服务器创建完副本的消息，开始准备资源播放剧情，地图ID为" + data["mapid"]);
            //debug.Log(data.dump());

            //jason请求进入副本，要加载副本所有的相关的资源
            //通过level_id 来找到mapid ，预先加载好地图

            //sendData["mapid"] = curmapid;
            //sendData["ltpid"] = ltpid;


            Variant xml = SvrLevelConfig.instacne.get_level_data(data["ltpid"]);
            if (xml != null)
            {
                joinWorldInfo jinfo = this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD) as joinWorldInfo;

                //这里的mapid是对应gconf中的mapid
                jinfo.m_data["mpid"] = xml["map"][0]["id"];


                /************修改有时进地图副本时游戏卡住的bug
                （调试下来是进副本的协议和播放剧情的模块逻辑冲突问题，解决方案：屏蔽掉剧情播放）***********/

                //LGLoadResource._instance.m_nLoaded_MapID = -1;
                //LGLoadResource._instance._onMapChgLoad();

                //MapModel.getInstance().curLevelId = data["ltpid"]._uint;
                //GRMap.LEVEL_PLOT_ID = data["ltpid"]._int;

                ////sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
                //GRMap.SetPoltOver_EnterLevel(delegate ()
                //{
                //    debug.Log("SCENE_LEVEL ---- 剧情播放完毕，进入地图" + jinfo.m_data["mpid"]);


                //    debug.Log("!!sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data)1!!" + " " + debug.count);
                //    sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
                //    LGLoadResource._instance.m_nLoaded_MapID = jinfo.m_data["mpid"];
                //});

                MapModel.getInstance().curLevelId = data["ltpid"]._uint;
                InterfaceMgr.doCommandByLua("MapModel:getInstance().getcurLevelId", "model/MapModel", data["ltpid"]._uint);
                MapModel.getInstance().curDiff = data["diff_lvl"]._uint;
                GRMap.LEVEL_PLOT_ID = data["ltpid"]._int;

                debug.Log("!!sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data)2!!");
                sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
                LGLoadResource._instance.m_nLoaded_MapID = -1;
            }
            else
            {
                MapModel.getInstance().curLevelId = 0;
                InterfaceMgr.doCommandByLua("MapModel:getInstance().getcurLevelId", "model/MapModel", 0);
                debug.Log("!!sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data)2!!");
                sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
                LGLoadResource._instance.m_nLoaded_MapID = -1;
            }
        }

        public void get_associate_lvls(Variant data)
        {
            sendRPC(PKG_NAME.C2S_GET_ASSOCIATE_LVLS_RES, data);
        }

        //		消息功能：获取副本信息列表；
        //		cmd_id：244
        //		参数：{}
        //		对应消息：get_lvl_cnt_info_res
        public void get_lvl_cnt_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_GET_LVL_INFO_RES, data);
        }

        public void get_lvl_prize(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_FIN, data);
        }
        //		消息功能：离开副本，在副本中调用该函数可离开副本回到大世界，副本依然存在（组队副本不离开队伍）；
        //		cmd_id：246
        //		参数：{}
        public void leave_lvl()
        {
            sendRPC(PKG_NAME.C2S_LVL_GET_PRIZE_RES, new Variant());
        }

        public void close_lvl(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_SIDE_INFO, data);
        }

        //获取侠客行任务相关信息
        public void GetLvlmisInfo()
        {
            sendRPC(PKG_NAME.C2S_GET_LVLMIS_RES, new Variant());
        }
    }
    class get_lvlmis_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_GET_LVLMIS_RES;
            }
        }
        public static get_lvlmis_res create()
        {
            return new get_lvlmis_res();
        }
        override protected void _onProcess()
        {
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_lvlmis_res(msgData);	
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_GET_LVLMIS_RES, this, GameTools.CreateSwitchData("get_lvlmis_res", msgData))
            );
        }
    }

    class clanter_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CLANTER_RES;
            }
        }
        public static clanter_res create()
        {
            return new clanter_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CLANTER_RES, this, GameTools.CreateSwitchData("on_clanter_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_clanter_res(msgData);		
        }
    }

    class lvl_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_RES;
            }
        }
        public static lvl_res create()
        {
            return new lvl_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_RES, this, GameTools.CreateSwitchData("on_lvl_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_res(msgData);		
        }
    }

    class carrchief : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CARRCHIEF;
            }
        }
        public static carrchief create()
        {
            return new carrchief();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CARRCHIEF, this, GameTools.CreateSwitchData("on_carrchief_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_carrchief_res(msgData);		
        }
    }

    class on_arena : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ON_ARENA;
            }
        }
        public static on_arena create()
        {
            return new on_arena();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_ON_ARENA, this, GameTools.CreateSwitchData("on_arena_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_Arena.on_arena_res(msgData);		
        }
    }

    class lvl_broadcast : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_BROADCAST;
            }
        }
        public static lvl_broadcast create()
        {
            return new lvl_broadcast();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_BROADCAST, this, GameTools.CreateSwitchData("on_lvl_broadcast_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_broadcast_res(msgData);		
        }
    }

    class lvl_pvpinfo_board_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES;
            }
        }
        public static lvl_pvpinfo_board_res create()
        {
            return new lvl_pvpinfo_board_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, this, GameTools.CreateSwitchData("lvl_pvpinfo_board_msg", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.lvl_pvpinfo_board_msg(msgData);		
        }
    }

    class mod_lvl_selfpvpinfo : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_MOD_LVL_SELFPVPINFO;
            }
        }
        public static mod_lvl_selfpvpinfo create()
        {
            return new mod_lvl_selfpvpinfo();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_MOD_LVL_SELFPVPINFO, this, GameTools.CreateSwitchData("mod_lvl_selfpvpinfo", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.mod_lvl_selfpvpinfo(msgData);		
        }
    }

    class lvl_err_msg : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_ERR_MSG;
            }
        }
        public static lvl_err_msg create()
        {
            return new lvl_err_msg();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_ERR_MSG, this, GameTools.CreateSwitchData("on_lvl_err_msg", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_err_msg(msgData);		
            //通知ai
            //GameSession(this.session).logicClient.logicInGame.AIPly.onLvlErr(msgData.res);
        }
    }
    class check_in_lvl_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CHECK_IN_LVL_RES;
            }
        }
        public static check_in_lvl_res create()
        {
            return new check_in_lvl_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CHECK_IN_LVL_RES, this, GameTools.CreateSwitchData("on_check_in_lvl_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_check_in_lvl_res(msgData);
        }
    }
    class create_lvl_res : RPCMsgProcesser
    {
        // 消息id

        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CREATE_LVL_RES;
            }
        }
        public static create_lvl_res create()
        {
            return new create_lvl_res();
        }
        override protected void _onProcess()
        {
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_create_lvl_res(msgData);
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CREATE_LVL_RES, this, GameTools.CreateSwitchData("on_create_lvl_res", msgData))
        );
        }
    }
    class enter_lvl_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ENTER_LVL_RES;
            }
        }
        public static enter_lvl_res create()
        {
            return new enter_lvl_res();
        }
        override protected void _onProcess()
        {
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_enter_lvl_res(msgData);
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_ENTER_LVL_RES, this, GameTools.CreateSwitchData("on_enter_lvl_res", msgData))
            );
        }
    }
    class get_associate_lvls_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES;
            }
        }
        public static get_associate_lvls_res create()
        {
            return new get_associate_lvls_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES, this, GameTools.CreateSwitchData("get_associate_lvls_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_associate_lvls_res(msgData);
        }
    }
    class get_lvl_info_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_GET_LVL_INFO_RES;
            }
        }
        public static get_lvl_info_res create()
        {
            return new get_lvl_info_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_GET_LVL_INFO_RES, this, GameTools.CreateSwitchData("get_lvl_info_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_lvl_info_res(msgData);
        }
    }
    class lvl_fin : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_FIN;
            }
        }
        public static lvl_fin create()
        {
            return new lvl_fin();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_FIN, this, GameTools.CreateSwitchData("on_lvl_fin", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_fin(msgData);
        }
    }
    class lvl_get_prize_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_GET_PRIZE_RES;
            }
        }
        public static lvl_get_prize_res create()
        {
            return new lvl_get_prize_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_GET_PRIZE_RES, this, GameTools.CreateSwitchData("lvl_get_prize_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.lvl_get_prize_res(msgData);
        }
    }
    class lvl_side_info : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_SIDE_INFO;
            }
        }
        public static lvl_side_info create()
        {
            return new lvl_side_info();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_SIDE_INFO, this, GameTools.CreateSwitchData("on_lvl_side_info", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_side_info(msgData);
        }
    }
    class close_lvl_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CLOSE_LVL_RES;
            }
        }
        public static close_lvl_res create()
        {
            return new close_lvl_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CLOSE_LVL_RES, this, GameTools.CreateSwitchData("on_close_lvl_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_close_lvl_res(msgData);
        }
    }
    class lvl_km : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_KM;
            }
        }
        public static lvl_km create()
        {
            return new lvl_km();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LVL_KM, this, GameTools.CreateSwitchData("on_lvl_km", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_km(msgData);
        }
    }
    class leave_lvl_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LEAVE_LVL_RES;
            }
        }
        public static leave_lvl_res create()
        {
            return new leave_lvl_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LEAVE_LVL_RES, this, GameTools.CreateSwitchData("on_leave_lvl", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_leave_lvl();
        }
    }


    class on_battle_do_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ON_BATTLE_DO_RES;
            }
        }
        public static on_battle_do_res create()
        {
            return new on_battle_do_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_ON_BATTLE_DO_RES, this, GameTools.CreateSwitchData("on_battle_do_res", msgData))
            );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_battle_do_res(msgData);
        }
    }
}