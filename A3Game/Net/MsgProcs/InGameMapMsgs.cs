
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{

    public class InGameMapMsgs : MsgProcduresBase
    {
        public InGameMapMsgs(IClientBase m)
            : base(m)
        {
        }
        public static InGameMapMsgs create(IClientBase m)
        {
            return new InGameMapMsgs(m);
        }
        override public void init()
        {

        }
        public void goto_map(Variant data)
        {
            sendRPC(12, data);
        }

        public void npcTrans(uint npcid, uint trid)
        {
            Variant data = new Variant();
            data["npcid"] = npcid;
            data["trid"] = trid;
           sendRPC(19,data);
        }

        public void get_wrdboss_respawntm(bool islvl = false)
        {	//获取boss复活时间列表
            Variant data = new Variant();
            data["islvl"] = islvl;
           sendRPC( PKG_NAME.C2S_MONSTER_SPAWN, data );
        }
       

        public void respawn(bool useGolden=false)
        {
            Variant data = new Variant();
            data["useGolden"] = useGolden;
           sendRPC(PKG_NAME.C2S_PLAYER_RESPAWN,data);
        }

        public void get_sprite_hp_info(uint iid)
        {
            Variant data = new Variant();
            data["iid"] = iid;
           sendRPC( PKG_NAME.C2S_ON_SPRITE_HP_INFO_RES, data );
        }

        public void change_map(uint mapid)
        {
            Variant data = new Variant();
            data["gto"] = mapid;
            sendRPC(57,data);
        }
        public void end_change_map()
        {
           sendRPC(58,new Variant());
        }
    }
    //class add_npcs : RPCMsgProcesser
    //{
    //    // 消息id
    //    override public uint msgID
    //    {
    //        get
    //        {
    //            return PKG_NAME.S2C_ADD_NPCS;
    //        }
    //    }
    //    public static add_npcs create()
    //    {
    //        return new add_npcs();
    //    }
    //    override protected void _onProcess()
    //    {
    //        //(session as gamesession).logicclient.logicingame.addmapstempobj( msgdata );
    //        (session as ClientSession).g_mgr.dispatchEvent(
    //           GameEvent.Create(PKG_NAME.S2C_ADD_NPCS, this, msgData)
    //       );
    //    }
    //}
    class trig_eff : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_TRIG_EFF;
            }
        }
        public static trig_eff create()
        {
            return new trig_eff();
        }
        override protected void _onProcess()
        {
            //to do
            (session as ClientSession).g_mgr.dispatchEvent(
              GameEvent.Create(PKG_NAME.S2C_TRIG_EFF, this, msgData)
          );
        }
    }

    class monster_spawn : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_MONSTER_SPAWN;
            }
        }
        public static monster_spawn create()
        {
            return new monster_spawn();
        }
        override protected void _onProcess()
        {
            //if(!(session as gamesession).logicclient.logicingame.map)
            //{
            //    return;
            //}

            //if(msgdata)
            //{
            //    if(msgdata.containskey( "iid" ))
            //    {
            //        (session as gamesession).logicclient.logicingame.map.respawn(msgdata);
            //    }
            //    else
            //    {
            //        if((session as gamesession).logicclient.logicingame.lggd_worldboss)
            //        {
            //            (session as gamesession).logicclient.logicingame.lggd_worldboss.on_monster_spawn(msgdata);
            //        }				
            //    }	
            //}	
            (session as ClientSession).g_mgr.dispatchEvent(
              GameEvent.Create( PKG_NAME.S2C_MONSTER_SPAWN , this, msgData)
          );
        }
    }


    class sprite_invisible : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_SPRITE_INVISIBLE;
            }
        }
        public static sprite_invisible create()
        {
            return new sprite_invisible();
        }
        override protected void _onProcess()
        {
            //to do
            //if((session as gamesession).logicclient.logicingame.map)
            //{
            //    lgcharacter lgcha = (session as gamesession).logicclient.logicingame.map.get_character_by_iid(msgdata.iid);
            //    if( lgcha )
            //    {
            //        lgcha.showavatar(msgdata.invisible == 0 );
            //    }
            //}
            (session as ClientSession).g_mgr.dispatchEvent(
              GameEvent.Create(PKG_NAME.S2C_SPRITE_INVISIBLE, this, msgData)
          );

        }
    }

    class on_sprite_hp_info_res : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ON_SPRITE_HP_INFO_RES;
            }
        }
        public static on_sprite_hp_info_res create()
        {
            return new on_sprite_hp_info_res();
        }
        override protected void _onProcess()
        {
            //if(msgdata.res == 1)
            //{
            //    lgcharacter lgcha = (session as gamesession).logicclient.logicingame.map.get_character_by_iid(msgdata.info.iid);
            //    if( lgcha )
            //    {
            //        lgcha.on_sprite_hp_info_res(msgdata.info);

            //        lgcharacter selcha = (session as gamesession).logicclient.logicingame.plycontroller.select_target;
            //        if(  selcha == lgcha )
            //        {
            //            lgiuimainuiattach lguimain = (session as gamesession).logicclient.uiclient.getlgui("mainuiattach") as lgiuimainuiattach;
            //            if(lguimain)
            //            {
            //                lguimain.updatetargetinfo();
            //            }	
            //        }
            //    }
            //    else
            //    {
            //        //err
            //    }
            //}
            //else
            //{
            //    //err
            //    (session as gamesession).logicclient.logicingame.mainui.output_server_err(msgdata.res);
            //}
            (session as ClientSession).g_mgr.dispatchEvent(
              GameEvent.Create(PKG_NAME.S2C_ON_SPRITE_HP_INFO_RES, this, msgData)
          );
        }
    }

    //class on_player_enter_zone : RPCMsgProcesser
    //{
    //    // 消息id

    //    //override protected void _onProcess()
    //    //{
    //    //for (int i = 0; i < msgData.pary.length; i++) 
    //    //{
    //    //	Variant ply = msgData.pary[i];
    //    //	(session as GameSession).logicClient.logicInGame.plyinfoMgr.checkShowInfoRev(ply.cid,ply.rev,ply.iid);
    //    //	Variant pinfo = (session as GameSession).logicClient.logicInGame.plyinfoMgr.removePlayerInfo(ply.cid);
    //    //	lgPlayer lgply = (session as GameSession).logicClient.logicInGame.createPlayer();
    //    //	lgply.initNetData(ply);
    //    //	if(pinfo != null)
    //    //	{
    //    //		if(pinfo.appendDetailInfo)
    //    //		{
    //    //			lgply.appendDetailInfo(pinfo.data);
    //    //		}
    //    //		else if(pinfo.appendShowInfo)
    //    //		{
    //    //			lgply.appendShowInfo(pinfo.data);
    //    //		}
    //    //	}
    //    //	(session as GameSession).logicClient.logicInGame.map.addCharacter(lgply);
    //    //}

    //    //}

    //    override public uint msgID
    //    {
    //        get
    //        {
    //            return PKG_NAME.S2C_PLAYER_ENTER_ZONE;
    //        }
    //    }
    //    public static on_player_enter_zone create()
    //    {
    //        return new on_player_enter_zone();
    //    }

    //    override protected void _onProcess()
    //    {
    //        (session as ClientSession).g_mgr.dispatchEvent(
    //            GameEvent.Create(PKG_NAME.S2C_PLAYER_ENTER_ZONE, this, msgData)
    //        );
    //    }
    //}

    class on_monster_enter_zone : RPCMsgProcesser
    {
        // 消息id

        //override protected void _onProcess()
        //{
        //	for (int i = 0; i < msgData.monsters.length; i++) 
        //	{
        //		Variant monData = msgData.monsters[i];

        //		lgMonster lgMon = new lgMonster((session as GameSession).logicClient.logicInGame);
        //		lgMon.initNetData(monData);
        //		(session as GameSession).logicClient.logicInGame.map.addCharacter(lgMon);
        //	}
        //}

        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_MONSTER_ENTER_ZONE;
            }
        }
        public static on_monster_enter_zone create()
        {
            return new on_monster_enter_zone();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_MONSTER_ENTER_ZONE, this, msgData)
            );
        }

    }

    class on_sprite_leave_zone : RPCMsgProcesser
    {
        // 消息id

        //override protected void _onProcess()
        //{

        //	for (int i = 0; i < msgData.iidary.length; i++) 
        //	{
        //		uint iid = msgData.iidary[i];
        //		(session as GameSession).logicClient.logicInGame.map.removeCharacter(iid);

        //		lgCharacter lgCha = (session as GameSession).logicClient.logicInGame.map.get_Character_by_iid(iid);
        //		//缓存
        //		if(lgCha == null)
        //		{
        //			DebugTrace.add(DebugTrace.DTT_ERR,"on_sprite_leave_zone err,iid["+iid+"] is map.ContainsKey( not )!");
        //		}
        //		else
        //		{
        //			if(lgCha is lgPlayer)
        //			{
        //				(session as GameSession).logicClient.logicInGame.plyinfoMgr.addPlayerInfo(lgCha as lgPlayer);
        //			}

        //			lgCha.dispose();
        //			// TO  add DO  free list
        //		}
        //	}
        //}

        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_SPRITE_LEAVE_ZONE;
            }
        }
        public static on_sprite_leave_zone create()
        {
            return new on_sprite_leave_zone();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_SPRITE_LEAVE_ZONE, this, msgData)
            );
        }

    }

    //class on_begin_change_map_res : RPCMsgProcesser
    //{

    //    //	override protected void _onProcess()
    //    //	{
    //    //		if(msgData.res != SvrGameErrorCode.RES_OK)
    //    //		{
    //    //			// 重新尝试
    //    //			if(!(session as GameSession).logicClient.logicInGame.resend_chang_map())
    //    //			{
    //    //				// 重试次数超过上限，停止切换
    //    //				(session as GameSession).logicClient.logicInGame.cancel_change_map();
    //    //			}
    //    //			(session as GameSession).logicClient.logicInGame.mainUI.output_server_err(msgData.res);
    //    //			return;
    //    //		}

    //    //		(session as GameSession).logicClient.logicInGame.onBeginChangeMap();
    //    //	}
    //    override public uint msgID
    //    {
    //        get
    //        {
    //            return PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES;
    //        }
    //    }
    //    public static on_begin_change_map_res create()
    //    {
    //        return new on_begin_change_map_res();
    //    }

    //    override protected void _onProcess()
    //    {
    //        (session as ClientSession).g_mgr.dispatchEvent(
    //            GameEvent.Create(PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, this, msgData)
    //        );
    //    }

    //}

    class onMapChange : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_MAP_CHANGE;
            }
        }
        public static onMapChange create()
        {
            return new onMapChange();
        }

        override protected void _onProcess()
        {
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(PKG_NAME.S2C_MAP_CHANGE, this, msgData)
            );
        }
    }
    ///**
    // *其他人穿上、脱下装备结果 
    // * @param data
    // * 
    // */	
    class on_other_eqp_change : RPCMsgProcesser
    {
        // 消息id
        override public uint msgID
        {
            get
            {
                return PKG_NAME.S2C_ON_OTHER_EQP_CHANGE;
            }
        }
        public static on_other_eqp_change create()
        {
            return new on_other_eqp_change();
        }
        override protected void _onProcess()
        {
            //		lgPlayer lgPly = (session as GameSession).logicClient.logicInGame.map.get_player_by_iid( msgData.iid );		
            //		if( lgPly )
            //		{
            //			if(msgData.hasOwnProperty("rmv"))
            //			{
            //				lgPly.RmvShowEqps( msgData.rmv );			
            //			}

            //			if(msgData.hasOwnProperty("add"))
            //			{
            //				lgPly.AddShowEqps( [msgData.add] );			
            //			}
            //		}
            (session as ClientSession).g_mgr.dispatchEvent(
              GameEvent.Create(PKG_NAME.S2C_ON_OTHER_EQP_CHANGE, this, msgData)
          );
        }
    }
}
 



