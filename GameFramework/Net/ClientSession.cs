using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace GameFramework
{
	public class ClientSession : Session
	{
		private NetClient _netClient;		 
		public ClientSession (IConnection c, NetManager mgr):base( c, mgr )
		{
            type = 1;
		}

		public NetClient g_mgr
		{
			get{
				return _netClient;
			}
		}
		public void setNetClient( NetClient cl )
		{
			_netClient = cl;
		}

		// IConnectionEventHandler implements -----------------------------------
		override public void onConnect()
		{
			_netClient.onConnect();
		}
		override public void onConnectFailed()
		{
			_netClient.onConnectFailed();
		}
		override public void onConnectionClose()
		{
			_netClient.onConnectionClose();
		}
		
		override public void onError(string msg)
		{
			_netClient.onError( msg );
		}
		
		override public void onLogin(Variant msg)
		{
			_netClient.onLogin( msg );
		}
		
		override public void onServerVersionRecv()
		{
			_netClient.onServerVersionRecv();
		}
		
		override public void onHBSend()
		{
			_netClient.onHBSend();
		}
		override public void onHBRecv()
		{
			_netClient.onHBRecv();
		}

	}
}
