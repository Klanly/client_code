
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;

namespace GameFramework
{
	 
	public abstract class MsgProcduresBase : GameEventDispatcher, IMessageSend, IObjectPlugin
	{	 
       
		public NetClient g_mgr;
		//private Variant _data;
		private string _controlId;
		public MsgProcduresBase( IClientBase m ):base()
		{
			g_mgr = m as NetClient;
		}
		abstract public void init();

       

		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}

		public void sendRPC(uint cmd, Variant msg)
		{
            try
            {
                g_mgr.sendRpc(cmd, msg);
            }
            catch (Exception ex)
            {
                g_debug.Log("sendRPC 消息错误 ID为" + cmd.ToString() + "  Exception " + ex.Message); 
            }
		}

		public void sendTpkg(uint cmd, Variant msg)
		{
            try
            {
                g_mgr.sendTpkg(cmd, msg);
            }
            catch (Exception ex)
            {
                g_debug.Log("sendTpkg 消息错误 ID为" + cmd.ToString() + "  Exception " + ex.Message);
            }
		}

        public void regMsgs(NetManager mgr)
		{
		}
        public void skexp_up(uint skillID,int tp = 0)
		{
            //if(tp == 0)
            //{
            //    this.session.sendRPC(90,{skid:skillID});
            //}
            //else
            //{
            //    this.session.sendRPC(90,{skid:skillID,tp:tp});
            //}
		}
	}
}
