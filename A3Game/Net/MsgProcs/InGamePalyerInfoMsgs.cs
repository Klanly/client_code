using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    public class InGamePalyerInfoMsgs : MsgProcduresBase
    {
        public InGamePalyerInfoMsgs(IClientBase m)
            : base(m)
        {
        }
        public static InGamePalyerInfoMsgs create(IClientBase m)
        {
            return new InGamePalyerInfoMsgs(m);
        }
        override public void init()
        {
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_ATT_CHANGE, onAttChange.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_SELF_ATT_CHANGE, onSelfAttchange.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_DETAIL_INFO_CHANGE, onDetailInfoChange.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_SKEXP_CHANGE, onSkexpChange.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_PLAYER_SHOW_INFO, onPlayerShowInfo.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_PLAYER_DETAIL_INFO, onPlayerDetailInfo.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_LVL_UP, onLvlUp.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_MODE_EXP, onModeExp.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_Get_USER_CID_RES, onGetUserCidRes.create);
            //g_mgr.regRPCProcesser(PKG_NAME.S2C_QUERY_PLY_INFO_RES, onQueryPlyInfoRes.create);
        }

        public void AttChange()
        {
            Variant msg = new Variant();
            sendRPC(26, msg);
        }
        public void SelfAttchange()
        {
            Variant msg = new Variant();
            sendRPC(32, msg);
        }
        public void DetailInfoChange()
        {
            Variant msg = new Variant();
            sendRPC(40, msg);
        }
        public void SkexpChange()
        {
            Variant msg = new Variant();
            sendRPC(41, msg);
        }
        public void PlayerShowInfo(Variant cids)
        {
            Variant msg = new Variant();
            msg["cidary"] = cids;
            sendRPC(51, msg);
        }
        public void PlayerDetailInfo(uint cid)
        {
            Variant msg = new Variant();
            msg["cid"] = cid;
            sendRPC(52, msg);
        }
        public void lvl_up()
        {
            Variant msg = new Variant();
            sendRPC(60, msg);
        }
        public void mod_exp()
        {
            Variant msg = new Variant();
            sendRPC(61, msg);
        }
        public void on_get_user_cid_res(string name,bool ol,uint func)
        {
            Variant msg = new Variant();
            msg["ol"] = ol;
            msg["name"] = name;
            msg["func"] = func;
            sendRPC(251, msg);
        }
        public void on_query_ply_info_res(Variant data)
        {
            sendRPC(253, data);
        }
    }
    class onAttChange : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ATT_CHANGE;
            }
        }
        public static onAttChange create()
        {
            return new onAttChange();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_ATT_CHANGE, this, msgData)
            );
        }
    }

    class onSelfAttchange : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_SELF_ATT_CHANGE;
            }
        }
        public static onSelfAttchange create()
        {
            return new onSelfAttchange();
        }

        override protected void _onProcess()
        {
            //var _lgGD_Gen:lgGDGeneral = (session as GameSession).logicClient.logicInGame.lgGD_Gen;
            //if(msgData.ContainsKey("hexpadd"))
            //{
            //    _lgGD_Gen.mode_hexp(msgData["hexpadd"]);
            //}
            if (msgData.ContainsKey("hexpadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_HEXP, this, msgData["hexpadd"])
                );
            }
    //        if("clangadd" in msgData)
    //        {
    //            _lgGD_Gen.mode_clang(msgData.clangadd);
    //        }
            if (msgData.ContainsKey("clangadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_CLANG, this, msgData["clangadd"])
                );
            }
    //        if("clang" in msgData)
    //        {
    //            _lgGD_Gen.clang = msgData.clang;
    //            (session as GameSession).logicClient.logicInGame.lgGD_clans.self_info_change({clang:msgData.clang});
    ////			(session as GameSession).logicClient.logicInGame.lgGD_clans.set_clan_self_info({clang:msgData.clang});
    //        }
            if (msgData.ContainsKey("clang"))
            {
                Variant clang = new Variant();
                clang["clang"] = msgData["clang"];
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.SELF_INFO_CHANGE, this, clang)
                );
            }
    //        if("batptadd" in msgData)
    //        {
    //            _lgGD_Gen.mode_batpt(msgData.batptadd);
    //        }
            if (msgData.ContainsKey("batptadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_BATPT, this, msgData["batptadd"])
                );
            }
    //        if("nobptadd" in msgData)
    //        {
    //            _lgGD_Gen.mode_nobpt(msgData.nobptadd);
    //        }
            if (msgData.ContainsKey("nobptadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_NOBPT, this, msgData["nobptadd"])
                );
            }
    //        if("carrlvl" in msgData)
    //        {
    //            _lgGD_Gen.CarrlvlChange(msgData.carrlvl);
    //        }
            if (msgData.ContainsKey("carrlvl"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_CARRLVLCHANGE, this, msgData["carrlvl"])
                );
            }
    //        if ("prizelvl" in msgData){
    //            (session as GameSession).logicClient.logicInGame.selfPlayer.netData.prizelvl= msgData["prizelvl"];
    //            (session as GameSession).logicClient.logicInGame.lgGD_Award.refreshGrowPack();
    //        }
            if (msgData.ContainsKey("prizelvl"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.PRIZELVL, this, msgData["prizelvl"])
                );
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.REFRESH_GROW_PACK, this,null)
                );
            }
    //        if(msgData.hasOwnProperty("soulptadd"))
    //        {
    //            _lgGD_Gen.soulpt += msgData.soulptadd;
    //        }
            if (msgData.ContainsKey("soulptadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_SOULPT, this, msgData["soulptadd"])
                );
            }
    //        if("shopptadd" in msgData)
    //        {
    //            (session as GameSession).logicClient.logicInGame.lgGD_Gen.mode_shoppt(msgData.shopptadd);
    //        }
            if (msgData.ContainsKey("shopptadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_SHOPPT, this, msgData["shopptadd"])
                );
            }
    //        if("lotexptadd" in msgData)
    //        {
    //            (session as GameSession).logicClient.logicInGame.lgGD_Gen.mode_lotexpt(msgData.lotexptadd);
    //        }
            if (msgData.ContainsKey("lotexptadd"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_LOTEXPT, this, msgData["lotexptadd"])
                );
            }
    //        if("tcyb_lott_cost" in msgData)
    //        {
    //            (session as GameSession).logicClient.logicInGame.lgGD_Gen.mode_tcyb_lott_cost(msgData.tcyb_lott_cost);
    //            (session as GameSession).logicClient.logicInGame.lgGD_Lottery.RefreshLotCnt({usetp:3});
    //        }
            if (msgData.ContainsKey("tcyb_lott_cost"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_TCYB_LOTT_COST, this, msgData["tcyb_lott_cost"])
                );
                Variant usetp = new Variant();
                usetp["usetp"] = 3;
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.REFRESHLOTCNT, this, usetp)
                );
            }
    //        if("tcyb_lott" in msgData)
    //        {
    //            (session as GameSession).logicClient.logicInGame.lgGD_Gen.mode_tcyb_lott(msgData.tcyb_lott);
    //            (session as GameSession).logicClient.logicInGame.lgGD_Lottery.RefreshLotCnt({usetp:2});
    //        }
            if (msgData.ContainsKey("tcyb_lott"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODE_TCYB_LOTT, this, msgData["tcyb_lott"])
                );
                Variant usetp = new Variant();
                usetp["usetp"] = 2;
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.REFRESHLOTCNT, this, usetp)
                );
            }
          
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_SELF_ATT_CHANGE, this, msgData)
            );
            ((session as ClientSession).g_mgr.g_gameM as muLGClient).g_selfPlayer.on_self_attchange(msgData);
        }
    }

    class onDetailInfoChange : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_DETAIL_INFO_CHANGE;
            }
        }
        public static onDetailInfoChange create()
        {
            return new onDetailInfoChange();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_DETAIL_INFO_CHANGE, this, msgData)
            );
        }
    }

    class onSkexpChange : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_SKEXP_CHANGE;
            }
        }
        public static onSkexpChange create()
        {
            return new onSkexpChange();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_SKEXP_CHANGE, this, msgData)
            );
        }
    }

    class onPlayerShowInfo : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_PLAYER_SHOW_INFO;
            }
        }
        public static onPlayerShowInfo create()
        {
            return new onPlayerShowInfo();
        }

        override protected void _onProcess()
        {               
            
            (session as ClientSession).g_mgr.dispatchEvent(
				GameEvent.Create(PKG_NAME.S2C_PLAYER_SHOW_INFO, this, msgData )
			);
            
		    //(session as GameSession).logicClient.logicInGame.plyinfoMgr.on_player_show_info(showinfo);
	       
        }
    }

    class onPlayerDetailInfo : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_PLAYER_DETAIL_INFO;
            }
        }
        public static onPlayerDetailInfo create()
        {
            return new onPlayerDetailInfo();
        }

        override protected void _onProcess()
        {
            if (msgData["res"]._int == 1)
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_PLAYER_DETAIL_INFO, this, msgData["pinfo"])
                );
                //(session as GameSession).logicClient.logicInGame.plyinfoMgr.on_player_detail_info(msgData.pinfo);
            }
            else
            {
                //err
                (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_PLAYER_SHOW_INFO, this, msgData["res"])
                );
                //(session as GameSession).logicClient.logicInGame.mainUI.output_server_err(msgData.res);
            }
        }
    }

    class onLvlUp : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_LVL_UP;
            }
        }
        public static onLvlUp create()
        {
            return new onLvlUp();
        }

//未完成
        override protected void _onProcess()
        {
            


            //var lgply:lgPlayer = (session as GameSession).logicClient.logicInGame.map.get_Character_by_iid(msgData.iid) as lgPlayer;
            //if(lgply == null)
            //{
            //    DebugTrace.add(Define.DebugTrace.DTT_DTL , "lvl_up msg err,iid[" + msgData["iid"] + "] player is not in map!");
            //    return;
            //}
            //lgply.on_lvl_up(msgData);
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(GAME_EVENT.ON_LVL_UP, this, msgData)
            );
            if(msgData.ContainsKey("pinfo"))
            {
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.R_PLAYER_INFO_CHANGED, this, null)
                );
                (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.PLAYER_INFO_CHANGED, this, null)
                );
                //(session as GameSession).logicClient.logicInGame.lgGD_RMis.PlayerInfoChanged();
                //(session as GameSession).logicClient.logicInGame.lgGD_miss.PlayerInfoChanged();		
            }
            //(session as GameSession).logicClient.logicInGame.lgGD_Team.modify_teammate_data(lgply.cid,lgply.netData);

            Variant plyInfo = new Variant();
            //plyInfo["cid"] = lgply["cid"];
            //plyInfo["netData"] = lgply["netData"];
            (session as ClientSession).g_mgr.dispatchEvent(
                    GameEvent.Create(GAME_EVENT.MODIFY_TEAMMATE_DATA, this, msgData["lvlshare"])
                );
        }
    }

    class onModeExp : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_MODE_EXP;
            }
        }
        public static onModeExp create()
        {
            return new onModeExp();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_MODE_EXP, this, msgData)
            );
        }
    }

    class onGetUserCidRes : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_Get_USER_CID_RES;
            }
        }
        public static onGetUserCidRes create()
        {
            return new onGetUserCidRes();
        }

        override protected void _onProcess()
        {
            //((session as ClientSession).g_mgr.g_gameM as muLGClient).g_MgrPlayerInfoCT.on_get_user_cid_res(msgData);
            //(session as ClientSession).g_mgr.dispatchEvent(
            //    GameEvent.Create(PKG_NAME.S2C_Get_USER_CID_RES, this, msgData)
            //);
        }
    }

    //class onQueryPlyInfoRes : RPCMsgProcesser
    //{
    //    // 消息id
    //    override public uint msgID
    //    {
    //        get
    //        {
    //            return PKG_NAME.S2C_QUERY_PLY_INFO_RES;
    //        }
    //    }
    //    public static onQueryPlyInfoRes create()
    //    {
    //        return new onQueryPlyInfoRes();
    //    }

    //    override protected void _onProcess()
    //    {
    //        (session as ClientSession).g_mgr.dispatchEvent(
    //            GameEvent.Create(PKG_NAME.S2C_QUERY_PLY_INFO_RES, this, msgData)
    //        );
    //        ((session as ClientSession).g_mgr.g_gameM as muLGClient).g_MgrPlayerInfoCT.on_query_ply_info_res(msgData);
    //    }
    //}
}
