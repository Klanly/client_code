using System;
using System.Collections.Generic;
using GameFramework;  
using Cross;
namespace MuGame
{
 
	public class connInfo : LGDataBase
	{
        public uint uid = 100000068;
		//public string server_ip = "10.1.59.133";
		//public string token = "123";
		//public int server_port = 46999;			
		//public uint server_id = 20;		
		//public uint clnt = 0;		
		//public string server_config_url = "http://10.1.59.133/do.php";		
		public Variant keyt = new Variant();// = { key:, type:2 };
		

		 //public string server_ip = "10.1.59.138";
		 //public string token = "123";
		 //public int server_port = 62999;			
		 //public uint server_id = 3;		
		 //public uint clnt = 0;		
		 //public string server_config_url = "http://10.1.59.138/do.php";		


		public string server_ip = "10.1.8.45";
		public string token = "123";
		public int server_port = 60999;			
		public uint server_id = 5;		
		public uint clnt = 0;		
		public string server_config_url = "http://10.1.8.76/do.php";	
		public string mainConfFile = "main";
		private Variant _verinfo;
		public connInfo( muNetCleint m):base(m)
		{

			
		}

		public static IObjectPlugin create( IClientBase m )
        {           
            return new connInfo( m as muNetCleint );
        }

		override public void init()
		{//
			//keyt["key"] = "@#x 65cb!k";
			//keyt["type"] = 1;

			g_mgr.addEventListener( GAME_EVENT.CONN_ED,		onConnect	 );
			g_mgr.addEventListener( GAME_EVENT.CONN_ERR,	onError		 );
			g_mgr.addEventListener( GAME_EVENT.CONN_VER,	onVerfin	 );
			g_mgr.addEventListener( GAME_EVENT.CONN_CLOSE,	onClose		 );
			g_mgr.addEventListener( GAME_EVENT.CONN_FAILE,	onConnfailed );
		}
 		public Variant getVerInfo()
		{
			return _verinfo;
		}

        TickItem tick;
        int tick_num = 0;
        private void onError(GameEvent e) 
		{
            debug.Log("连接错误");
            //debug.Log(e);
            if (login.instance != null && LGOutGame.instance != null)
            {
               
                Action handel = () =>
                {
                    tick_num = 0;
                    tick = new TickItem(onUpdate);
                    TickMgr.instance.addTick(tick);
                };
                login.instance.msg.show(true, "服务器连接错误，请重新连接", handel);
            }
		}

        void onUpdate(float s)
        {
            tick_num++;
            if (tick_num > 100)
            {
                LGOutGame.instance.reStart();
                TickMgr.instance.removeTick(tick);
            }
        }


        private void onConnect(GameEvent e) 
		{
			this.dispatchEvent( GameEvent.Create( GAME_EVENT.CONN_ED, this, null ) );
		}
		
		private void onVerfin(GameEvent e) 
		{
			_verinfo = g_mgr.g_netM.getServerVersionInfo();
			this.dispatchEvent( GameEvent.Create( GAME_EVENT.CONN_VER, this, null ) );
		} 
		private void onClose( GameEvent e ) 
		{
			
		}
		private void onConnfailed(GameEvent e) 
		{
			
		}

        public void reconect()
        {
            this.dispatchEvent(GameEvent.Createimmedi(GAME_EVENT.CONN_SET, this, null));
        }

		
		public void setInfo( Variant info )
		{
			if( info == null ) return;	 
	 
			Variant outgamevar = info["outgamevar"]; 
			this.server_ip = outgamevar["server_ip"]._str;
			this.uid = outgamevar["uid"]._uint;
			this.token = outgamevar["token"]._str;
			this.server_port = outgamevar["server_port"]._int32;
			this.clnt = outgamevar["clnt"]._uint;

			this.server_id = info["server_id"]._uint;
			this.server_config_url = info["server_config_url"]._str;
			this.mainConfFile = info["mainConfig"]._str;

            debug.Log("网络连接设置：" + outgamevar.dump());

			this.dispatchEvent( GameEvent.Createimmedi( GAME_EVENT.CONN_SET, this, null ) );
		}
	}
}