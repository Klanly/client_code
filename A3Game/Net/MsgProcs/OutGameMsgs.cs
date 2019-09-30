
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{
	
	public class outGameMsgs : MsgProcduresBase
	{
        public static outGameMsgs instance;
		public outGameMsgs( IClientBase m ):base( m )
		{
            instance = this;
		}
		public static outGameMsgs create( IClientBase m )
		{
			return new outGameMsgs( m );
		}
		override public void init()
		{
			//g_mgr.regTpkgProcesser( PKG_NAME.S2C_TPKG_ON_DELET_CHAR, onDeleteChaRes.create );
          //  g_mgr.regTpkgProcesser(PKG_NAME.S2C_TPKG_ON_CREATE_CHAR, onCreateChaRes.create);

            SessionFuncMgr.instance.addFunc(PKG_NAME.S2C_TPKG_ON_CREATE_CHAR, onCreat, false);

            SessionFuncMgr.instance.addFunc(PKG_NAME.S2C_TPKG_ON_DELET_CHAR, oDelChar, false);

            SessionFuncMgr.instance.addFunc(PKG_NAME.S2C_TPKG_OUT_SVR, onShowOutSvr, false);

            SessionFuncMgr.instance.addFunc(PKG_NAME.S2C_TPKG_LOGIN_LINE, onLoginLine, false);

            SessionFuncMgr.instance.addFunc(PKG_NAME.S2C_TPKG_ERR, onLoginErr, false);
        }
		
		public void selectCha( uint cid)
		{
			Variant msg = new Variant();
			msg["cid"] = cid;
			sendTpkg( PKG_NAME.TYPG_SELECT_CID, msg );
		}
		
		public void deleteCha( uint cid )
		{
			Variant msg = new Variant();
			msg["cid"] = cid;
			sendTpkg(4, msg );
		}

        public void onCreat(Variant data)
        {
            SceneCamera.m_isFirstLogin = true;
            NetClient.instance.dispatchEvent(
               GameEvent.Create(GAME_EVENT.S2C_CREAT_CHAR, this, data)
           );
        }

        public void oDelChar(Variant data)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(GAME_EVENT.DELETE_RETURE, this, data)
             );
        }

        public void onLoginErr(Variant data)
        {
            debug.Log("角色登入失败信息：" + data.dump());
            if (debug.instance != null)
                debug.instance.showMsg(data["reas"], 100);
        }

        public void onLoginLine(Variant data)
        {
            debug.Log("排队登入信息==" + data.dump());
            if (login.instance != null)
            {
                int num = data._int;
                string txt = ContMgr.getCont("sever_list1", new List<string> { num.ToString() });
                string txt1 = "";
                if (num > 1000)
                    txt1 = ContMgr.getCont("sever_list2");
                else
                {
                    double time = Math.Ceiling((double)num / 50);
                    txt1 = ContMgr.getCont("sever_list3", new List<string> { time.ToString() });
                }
                login.instance.setWaitingTxt(txt + txt1);
            }
        }

        public void onShowOutSvr(Variant data)
        {
            if (broadcasting.instance != null)
                broadcasting.instance.addGonggaoMsg(data["msg"]);
                        
            data["tp"] = (int)ChatToType.SystemMsg;
            if(a3_chatroom._instance != null)
                a3_chatroom._instance.otherSays(data);
        }

        public void createCha( string name, uint carr, uint sex)
		{
			Variant msg = new Variant();
			msg["name"] = name;
			msg["carr"] = carr;
			msg["sex"] = sex;

			sendTpkg(5, msg );
		}
		
		public void getWorldCount()
		{
			sendTpkg(6, null);
			 
		}
	}
 
	class onDeleteChaRes : TPKGMsgProcesser
	{
		public static TPKGMsgProcesser create()
		{
			return new onDeleteChaRes();
		}

		// 消息id 
		override public uint msgID
		{
			get {
				return PKG_NAME.S2C_TPKG_ON_DELET_CHAR;
			}
		}

		override protected void _onProcess()
		{
            (session as ClientSession).g_mgr.dispatchEvent(
                GameEvent.Create(GAME_EVENT.DELETE_RETURE, this, msgData)
            );
			
		}
	}

	class onCreateChaRes : TPKGMsgProcesser
	{
        public static TPKGMsgProcesser create()
        {
            return new onCreateChaRes();
        }
		// 消息id
		override public uint msgID
		{
			get {
				return PKG_NAME.S2C_TPKG_ON_CREATE_CHAR;
			}
		}
	
		override protected void _onProcess()
		{
            (session as ClientSession).g_mgr.dispatchEvent(
               GameEvent.Create(GAME_EVENT.S2C_CREAT_CHAR, this, msgData)
           );
		}
	}

	class onGetWorldCountRes : TPKGMsgProcesser
	{
		// 消息id
		override public uint msgID
		{
			get {
				return PKG_NAME.S2C_TPKG_GET_WORLD_COUNT;
			}
		}
	
		override protected void _onProcess()
		{
			//(session as GameSession).logicClient.logicOutgame.onGetWorldCountRes(msgData.cnt);
		}
	}

	class onErrRes : TPKGMsgProcesser
	{
		// 消息id
		override public uint msgID
		{
			get {
				return PKG_NAME.S2C_TPKG_ERR;
			}
		}
	
		override protected void _onProcess()
		{
			//(session as GameSession).logicClient.logicOutgame.onErrRes(msgData.res);
		}
	}
} 
