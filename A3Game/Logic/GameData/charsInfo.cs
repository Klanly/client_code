using System;
using System.Collections.Generic;
using GameFramework;  
using Cross;
namespace MuGame
{
 
	public class charsInfo : LGDataBase
	{
		public charsInfo( muNetCleint m ):base(m)
		{

		}

		public static IObjectPlugin create( IClientBase m )
        {           
            return new charsInfo( m as muNetCleint );
        }

		public int g_gm = 0;		
		public bool g_safe = false;
		private Variant _chas;
		override public void init()
		{//
			g_mgr.addEventListener(GAME_EVENT.ON_LOGIN, onLogin);
            g_mgr.addEventListener( GAME_EVENT.DELETE_RETURE, onDeleteChar);
            g_mgr.addEventListener( GAME_EVENT.S2C_CREAT_CHAR, onCreatChar);
		}
		//创建角色
        private void onCreatChar(GameEvent e)
        {
            Variant data = e.data;
            if (data["res"]._int > 0)
            {
                _chas._arr.Add(data["cha"]);
            }
            this.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_CREATE_CHAR, this, data));
        }
		//删除角色
        private void onDeleteChar(GameEvent e)
        {
            if (e.data["res"] < 0)
                Globle.err_output(e.data["res"]);
            else
                flytxt.instance.fly(ContMgr.getCont("role_delete"));
            Variant data = e.data;
            if (data["res"]._int > 0)
            {
                uint cid = data["cid"]._uint;
                for (int i = 0; i < _chas.Count; i++)
                {
                    if (_chas[i].ContainsKey("cid") && _chas[i]["cid"]._uint == cid)
                    {
                        _chas._arr.RemoveAt(i);
                    }
                }
                this.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_DELETE_CHAR, this, data));

                //if (selchar.instance)
                //    selchar.instance.onDelChar(data);
            }
        }
		//选择角色
		private void onSelectChar( Variant data  )
		{
		}
		private void onClose( Variant data  )
		{
		}
		
		private void onConnectFailed( Variant data  )
		{
		}

		public Variant getChas()
		{
			return _chas;
		}
		
		private void onLogin( GameEvent e ) 
		{			 
			Variant data = e.data;
			if( data["res"]._int < 0 )
			{
				return;
			}
			_chas = data["chas"]; 
			//_gm = data["gm"]._int;
			//_safe = data["safe"]._bool;  
			this.dispatchEvent( GameEvent.Create( GAME_EVENT.ON_LOGIN, this, null ) );
		}
	}
}