using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{

    public class InGameGeneralMsgs : MsgProcduresBase
    {

        public InGameGeneralMsgs(IClientBase m)
            : base(m)
        {
        }
        public static InGameGeneralMsgs create(IClientBase m)
        {
            return new InGameGeneralMsgs(m);
        }
        override public void init()
        {
            MapProxy.getInstance();
            g_mgr.regRPCProcesser(PKG_NAME.S2C_FCM_NOTIFY, fcm_notify.create);
            // g_mgr.regRPCProcesser(PKG_NAME.S2C_PK_STATE_CHANGE, pk_state_change.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_PK_V_CHANGE, pk_v_change.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_LINE_CHANGE, line_change.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_GAIN_ACHIVE, gain_achive.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_CUR_ARCHIVE_CHANGE, cur_archive_change.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_GET_ACHIVES_RES, get_achives_res.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_LINE_INFO_RES, line_info_res.create);
            g_mgr.regRPCProcesser(PKG_NAME.S2C_JOIN_WORLD_RES, onJoinWorldRes.create);
            //  g_mgr.regRPCProcesser(PKG_NAME.S2C_ON_CLIENT_CONFIG, on_client_config.create);

            //g_mgr.regRPCProcesser(PKG_NAME.S2C_ON_ERR_MSG, on_err_msg.create);
        }

        // TO DO : add send msg function


        /**
         *保存快捷键设置到服务器
         * 
         */
        public void SendSaveQuickbar(Variant msg)
        {
            sendRPC(150, GameTools.createGroup("quickbar", msg));
        }

        /**
         *获取角色称号信息
         * 
         */
        public void GetAchives()
        {
            sendRPC(PKG_NAME.C2S_GET_ACHIVES_RES, new Variant());
        }
        /**
         *获取角色称号信息
         * 
         */
        public void ActiveAchive(uint achid)
        {
            Variant msg = new Variant();
            msg["archiveid"] = achid;
            sendRPC(PKG_NAME.S2C_CUR_ARCHIVE_CHANGE, msg);
        }

        /**
         *获取角色排行称号信息
         * 
         */
        public void GetRnkAchives()
        {
            Variant msg = new Variant();
            msg["rnkach"] = PKG_NAME.S2C_FCM_NOTIFY;
            sendRPC(PKG_NAME.C2S_GET_ACHIVES_RES, msg);
        }
        /**
         * 改变PK状态
         * */
        public void SendPKState(Variant data)
        {
            //  sendRPC(PKG_NAME.S2C_PK_STATE_CHANGE, data);
        }
    }


    class fcm_notify : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_FCM_NOTIFY;
            }
        }
        public static fcm_notify create()
        {
            return new fcm_notify();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_FCM_NOTIFY, this, msgData)
            );
            //to do
            //(session as gamesession).logicclient.logicingame.setfcmnotify(msgdata);
        }
    }

    class pk_state_change : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                // return PKG_NAME.S2C_PK_STATE_CHANGE;
                return 0;
            }
        }
        static public pk_state_change create()
        {
            return new pk_state_change();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(0 /*PKG_NAME.S2C_PK_STATE_CHANGE */, this, msgData)
            );
            //if("pk_state" in msgdata)
            //{
            //    (session as gamesession).logicclient.logicingame.lggd_gen.pkstatechange(msgdata.pk_state);
            //}
            //else
            //{
            //    (session as gamesession).logicclient.logicingame.mainui.show_defense(msgdata);
            //}
        }
    }

    class pk_v_change : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_PK_V_CHANGE;
            }
        }
        public static pk_v_change create()
        {
            return new pk_v_change();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_PK_V_CHANGE, this, msgData)
            );
            //to do
            //        var lgcha:lgmovcharacter = (session as gamesession).logicclient.logicingame.map.get_character_by_iid(msgdata.iid);
            //        if(lgcha)
            //        {
            //            var isself:boolean = lgcha.isself();
            //            if(isself)
            //            {
            //                if("pk_v" in msgdata)
            //                {		
            //                    var chaui:lgiuicharacterinfo = (session as gamesession).logicclient.uiclient.getlgui("mdlg_chainfo") as lgiuicharacterinfo;
            //                    chaui.selfdetailinfochange({pk:msgdata.pk_v}, "pk");					
            ////					_uiclient.logicclient.logicingame.selfplayer.netdata
            //                }
            //            }
            //            lgcha.setpkvalue(msgdata.pk_v,msgdata.rednm);
            //            lgcha.shownamebypk();
            //            lgcha.showname();
            //        }
        }
    }
    class line_change : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LINE_CHANGE;
            }
        }
        static public line_change create()
        {
            return new line_change();
        }
        override protected void _onProcess()
        {	//消息功能：线路切换结果
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LINE_CHANGE, this, msgData)
            );
            //to do
            //(session as gamesession).logicclient.logicingame.lggd_worldline.line_change(msgdata);
        }
    }
    class gain_achive : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_GAIN_ACHIVE;
            }
        }
        static public gain_achive create()
        {
            return new gain_achive();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_GAIN_ACHIVE, this, msgData)
            );
            //(session as gamesession).logicclient.logicingame.lggd_achives.onaddachive( msgdata );
        }
    }
    class cur_archive_change : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_CUR_ARCHIVE_CHANGE;
            }
        }
        public static cur_archive_change create()
        {
            return new cur_archive_change();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_CUR_ARCHIVE_CHANGE, this, msgData)
            );
            //(session as gamesession).logicclient.logicingame.lggd_achives.onactachivechange( msgdata );
        }
    }
    class get_achives_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_GET_ACHIVES_RES;
            }
        }
        static public get_achives_res create()
        {
            return new get_achives_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_GET_ACHIVES_RES, this, msgData)
            );
            //(session as gamesession).logicclient.logicingame.lggd_achives.ongetachiveres( msgdata );
        }
    }
    class line_info_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LINE_INFO_RES;
            }
        }
        static public line_info_res create()
        {
            return new line_info_res();
        }
        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_LINE_INFO_RES, this, msgData)
            );
            //to do
        }
    }

    //角色初始化进入游戏
    class onJoinWorldRes : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_JOIN_WORLD_RES;
            }
        }
        public static onJoinWorldRes create()
        {
            return new onJoinWorldRes();
        }

        override protected void _onProcess()
        {
            debug.Log(">>>>>>>>>>>>>>>onJoinWorldRes::" + msgData.dump());

            //Session.S_CACHING_MSG = true;

            //先将场景文件放进协程进行加载，再进行初始化
            SvrMapConfig svrMapConfig = GRClient.instance.g_gameConfM.getObject(OBJECT_NAME.CONF_SERVER_MAP) as SvrMapConfig;
            Variant map_info = SvrMapConfig.instance.getSingleMapConf(msgData["mpid"]._uint32);
            if (map_info == null)
            {
                //debug.Log("找不到地图id=" + msgData["mpid"]._uint32.ToString() + "的数据");
            }
            else
            {
                GAMEAPI.LoadAsset_Async(map_info["name"]._str + ".assetbundle", map_info["name"]._str);
                //debug.Log("map_info::" + map_info.dump());
            }

            //不处理，就有被release了的可能，所以要马上处理
            //Stopwatch a1 = new Stopwatch(); a1.Start();//开始计时
            PlayerModel.getInstance().init(msgData);
            //a1.Stop(); debug.Log("cost PlayerModel.getInstance().init time =" + a1.ElapsedMilliseconds);//终止计时

            //a1 = new Stopwatch(); a1.Start();//开始计时
            (session as ClientSession).g_mgr.dispatchEvent(GameEvent.Create(PKG_NAME.S2C_JOIN_WORLD_RES, this, msgData));
            //a1.Stop(); debug.Log("cost S2C_JOIN_WORLD_RES time =" + a1.ElapsedMilliseconds);//终止计时
            //a1 = null;
        }
    }

    class on_err_msg : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ON_ERR_MSG;
            }
        }
        static public on_err_msg create()
        {
            return new on_err_msg();
        }
        override protected void _onProcess()
        {
            //(session as ClientSession).g_mgr.dispatchEvent(
            //    GameEvent.Create(PKG_NAME.S2C_ON_ERR_MSG, this, msgData)
            //);
            if (msgData["res"] == -1204)//此处处理有问题  后面待调整
            {
                //warning , use for change map err,
                //GameSession(this.session).logicClient.logicInGame.AIPly.onMapChangeFin();
            }
         
            else if (-253 == msgData["res"])//采集目标不在范围内
            {
                //GameSession(this.session).logicClient.logicInGame.selfPlayer.ReCollectChar();
            }
            else if (-1808 == msgData["res"])
            {
                //LGIUIAccount account = (session as ClientSession).g_mgr.g_uiM.getLGUI("accountsafe") as LGIUIAccount;
                //account.DoUnlock();
            }
            else if (-218 == msgData["res"])
            {//最大转生
            }
            else if (-1217 == msgData["res"])
            {//任务未完成
            }
            else if (-1204 == msgData["res"])
            {//等级未完成
            }
          
            else if (-616 == msgData["res"])//护送任务不能传送
            {
                //GameSession(this.session).logicClient.logicInGame.clientAI.fail_fast_map();
            }
            //LGIUIMainUI mainui = ((session as ClientSession).g_mgr.g_gameM as muLGClient).g_uiM.getLGUI(UIName.LGUIMainUIImpl) as LGIUIMainUI;
            //mainui.output_server_err(msgData["res"]);

        }
    }
}




