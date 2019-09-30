
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{

	abstract public class NetClient : clientBase, IProcess
	{
        public static NetClient instance;
		private ClientSession _session;
		private IConnection _conn;
		public NetClient(gameMain m):base(m)
		{
            instance = this;
		}
		override public void init()
		{
			_conn = new ConnectionImpl();
			_session = new ClientSession( _conn, _netM ); 

			_session.setNetClient( this );

			this.g_processM.addProcess( this );

			onInit();
		}
		public void regRPCProcesser(uint msgID, NetManager.RPCProcCreator procCrtFunc)
		{
			_netM.regRPCProcesser( msgID, procCrtFunc );
		}

		public void regTpkgProcesser(uint msgID, NetManager.TPKGProcCreator procCrtFunc)
		{
			_netM.regTpkgProcesser( msgID, procCrtFunc );
		}

		
		private NetManager _netM
		{
			get{
				return CrossApp.singleton.getPlugin("net") as NetManager ;
			}
		}

		abstract protected void onInit();
 
		
		public void onHBSend()
		{
		}
		public void onHBRecv()
		{
		}

        public bool isConnect
        {
            get { if (_conn == null)return false; return _conn.isConnect; }
        }

		public void onError(string msg) 
		{
            

			this.dispatchEvent( GameEvent.Create( GAME_EVENT_DEFAULT .CONN_ERR, this, null ) ); 
		}
	 

		public void onConnect() 
		{
			this.dispatchEvent( GameEvent.Create( GAME_EVENT_DEFAULT.CONN_ED, this, null ) ); 			
		}

		public void onServerVersionRecv() 
		{			
			this.dispatchEvent( GameEvent.Create( GAME_EVENT_DEFAULT.CONN_VER, this, null ) );			
		}
		public void onConnectionClose() 
		{
			this.dispatchEvent( GameEvent.Create( GAME_EVENT_DEFAULT.CONN_CLOSE, this, null ) ); 
			
		}
		public void onConnectFailed() 
		{
			this.dispatchEvent( GameEvent.Create( GAME_EVENT_DEFAULT.CONN_FAILE, this, null ) ); 
		}



		//public void onRPC(uint cmdID, string cmdName, Variant par)
		//{
		//	//trace( "onRpc" + cmdID );
		//	this.dispatchEvent( GameEvent.Createimmedi( cmdID, this, par ) ); 
		//}
		///// 58 -> 50    
		//public void onTPKG(uint cmdID, Variant par)
		//{
		//	//trace( "ontpkg" + cmdID );

		//	this.dispatchEvent( 
		//		GameEvent.Create( 
		//			(GAME_EVENT_DEFAULT.TYPE_PKG_OFFSET + cmdID ), 
		//			this, 
		//			par
		//		 )
		//	); 
		//}
		public void onLogin(Variant data) 
		{			 
			this.dispatchEvent( 
				GameEvent.Create( GAME_EVENT_DEFAULT.ON_LOGIN, this, data )
			 ); 
		}

		//public void onFullTPKG(uint cmdID, Variant par)
		//{
		//	//trace( "onFullTPKG" + cmdID );

			 
		//} 
 
		public void sendRpc( uint cmd, Variant msg )
		{
			if( cmd != 9 ) GameTools.PrintDetial( "sendRpc cmd["+cmd+"] msg:" + msg.dump()  );
			_conn.PSendRPC( cmd, msg );
		}
		public void sendTpkg(  uint cmd, Variant msg )
		{
			//DebugTrace.print( "not use this sendTpkg function ["+ cmd +"] ! " );		
			_conn.PSendTPKG( cmd, msg );
		}
		
		//连接

        private Variant connectData;
		public bool connect(string addr, int port, uint uid, string token, uint client, Variant sinfo, bool ipv6 = false)
		{
			//if( sinfo != null  ) 
			//{
			//	_session.setSec(..);
			//}

            connectData = new Variant();
            connectData["addr"] = addr;
            connectData["port"] = port;
            connectData["uid"] = uid;
            connectData["token"] = token;
            connectData["client"] = client;
            connectData["sinfo"] = sinfo;
            connectData["ipv6"] = ipv6;

            return _session.connect( addr, port, uid, token, client, ipv6);
		}

        public void reConnect(Variant v=null)
        {
            if (v != null)
                connectData = v;

            if (connectData == null)
                return;

            connect(connectData["addr"], connectData["port"], connectData["uid"], connectData["token"], connectData["client"], connectData["sinfo"], connectData["ipv6"]);
        }
 

		public void reqServerVersion()
		{
			_conn.RequestServerVersion();
		}
		public Variant getServerVersionInfo()
		{
			return _conn.ConfigVersions;
		}

		public string rpcProfileDump()
		{
			return _session.rpcProfileDump();
		}

		
		public IURLReq getUrlImpl()
		{
			return new URLReqImpl();
		}
		/// <summary>
		/// 当前服务器时间戳，单位秒
		/// </summary>
		public int CurServerTimeStamp 
		{
			get
			{
				return _conn.CurServerTimeStamp;
			}
		}

        public int curServerPing
        {
            
            get
			{

               

				return _conn.Latency;
			}
        }

        public void disconnect()
        {
            _conn.Disconnect();
        }

        //public bool isConnected()
        //{
        //    return _conn.isConnected;
        //}

 
		/// <summary>
		/// 当前服务器时间戳，单位毫秒
		/// </summary>
		public long CurServerTimeStampMS 
		{
			get
			{
				return _conn.CurServerTimeStampMS;
			}
		}


		//=== Iprocess ====
		private bool _pause = false;
		private bool _destory = false;
		private string _processName = "";
		public void updateProcess(float tmSlice)
		{
			_conn.onProcess();
		}

		public bool destroy
		{
			get
			{
				return _destory;
			}
			set
			{

				_destory = value;
			}
		}
		public bool pause
		{
			get
			{
				return _pause;
			}
			set
			{

				_pause = value;
			}
		}

		public string processName
		{
			get
			{
				return "NetClient";
			}
			set
			{
				_processName =value;
			}
			 
		}
		 
	} 
}