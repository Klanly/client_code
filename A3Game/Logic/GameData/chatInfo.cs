using System;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class chatInfo : LGDataBase
    {
        public chatInfo(muNetCleint m)
			: base(m)
		{
			
		}
        public static IObjectPlugin create(IClientBase m)
        {
            return new chatInfo(m as muNetCleint);
        }
        override public void init()
        {//
            this.addEventListener(GAME_EVENT.UI_CHAT_SEND, sendTalk);
            this.g_mgr.addEventListener(PKG_NAME.S2C_CHAT_MSG_RES, sendTalkRes);
            this.g_mgr.addEventListener(PKG_NAME.S2C_CHAT_MSG, getTalk);
        }


        public void sendTalk(GameEvent e)
        {
            Variant talk = new Variant();
            talk["msg"] = e.data["msg"];
            talk["tp"] = e.data["tp"];
            talk["cid"] = e.data["cid"];
            //talk["nm"] = ingame.world.player.name;
            talk["vip"] = null;
            talk["tid"] = null;  //队伍
            talk["clanid"] = null;  //帮派
            talk["withtid"] = false;
            //this.dispatchEvent(GameEvent.Create(PKG_NAME.S2C_CHAT_MSG, this, talk));
        }

        public void sendTalkRes(GameEvent e)
        {
            Variant res = e.data;
            dispatchEvent(
                GameEvent.Create(GAME_EVENT.UI_CHAT_RES_ME, this, res)
            );
        }

        public void getTalk(GameEvent e)
        {
            Variant msg = e.data;
            dispatchEvent(
                GameEvent.Create(GAME_EVENT.UI_CHAT_GET_OTHER, this, msg)
            );
        }
    }
}
