using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	 
	 
	class LGOthers : lgGDBase, IObjectPlugin
	{
       static public LGOthers instance;
		public LGOthers( gameManager m):base(m)
		{
            instance = this;
		} 
		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGOthers( m as gameManager  );
        }
		 
		private Dictionary< uint, LGAvatarOther> _playersByIid;
		private Dictionary< uint, LGAvatarOther> _playersByCid;
		private Dictionary< uint, Variant> _playerShowInfosByIid;
		private bool _initFlag = false;
		private bool _mapChageFlag = false;

 

		override public void init()
		{
			//return;
			_playersByIid = new Dictionary< uint, LGAvatarOther>();
			_playersByCid = new Dictionary< uint, LGAvatarOther>();
			_playerShowInfosByIid = new Dictionary< uint, Variant>();
			
			this.g_mgr.g_netM.addEventListener( 
				PKG_NAME.S2C_PLAYER_ENTER_ZONE, 
				onPlayerEnterZone 
			); 

			

			this.g_mgr.g_netM.addEventListener( 
				PKG_NAME.S2C_SPRITE_LEAVE_ZONE, 
				onSpriteLeaveZone 
			); 

			//this.g_mgr.g_gameM.addEventListenerCL( 
			//	OBJECT_NAME.LG_MAP,
			//	GAME_EVENT.MAP_INIT, 
			//	onMapchg 
			//); 
 
            //this.g_mgr.g_netM.addEventListenerCL(
            //    OBJECT_NAME.DATA_JOIN_WORLD, 
            //    PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, 
            //    onMapchgBegin 
            //); 
			
			
			this.g_mgr.g_gameM.addEventListenerCL( 
				OBJECT_NAME.LG_JOIN_WORLD,
				GAME_EVENT.ON_MAP_CHANGE, 
				onMapchg 
			);  

			this.g_mgr.g_gameM.addEventListenerCL( 
				OBJECT_NAME.LG_JOIN_WORLD,
				GAME_EVENT.ON_ENTER_GAME, 
				onMapchg 
			); 
		}



		private void onSpriteLeaveZone( GameEvent e )
		{ 
			Variant data = e.data;
			foreach( uint iid in  data["iidary"]._arr )
			{ 
				if( !_playerShowInfosByIid.ContainsKey( iid ) ) continue;
				 
				//DebugTrace.print( " player onSpriteLeaveZone:" + iid  );
				Variant pinfo = _playerShowInfosByIid[ iid ];
				uint cid = pinfo["cid"]._uint;
				_playerShowInfosByIid.Remove( iid );
				 
				if( _playersByIid.ContainsKey( iid ) )
				{
					LGAvatarOther ct  = _playersByIid[ iid ];
					_playersByIid.Remove( iid );	
					_playersByCid.Remove( cid );
					ct.dispose();
				} 
			} 
		}
		
		//todo clear players
		private void onMapchgBegin( GameEvent e )
		{
			_mapChageFlag = false; 
			_initFlag = false;
			clear();
		}

		public void clear()
		{
			foreach ( LGAvatarOther ct in _playersByIid.Values )
			{
				this.g_mgr.g_processM.removeRender( ct );
				ct.dispose();
			}
			_playersByIid.Clear();
			_playersByCid.Clear();
			_playerShowInfosByIid.Clear();
		}

		public void onMapchg(GameEvent e )
		{
            clear();
			_mapChageFlag = true; 
			createZoneSprites();
           
		}
		private void onPlayerEnterZone( GameEvent e )
		{
            if (GRMap.playingPlot)
                return;

            debug.Log("!!onPlayerEnterZone!!"+  " " + debug.count);
			Variant info = e.data;
			foreach ( Variant p in info["pary"]._arr )
			{
				uint iid = p["iid"]._uint;
				uint cid = p["cid"]._uint;
				
				if( _playerShowInfosByIid.ContainsKey( iid ) )
				{//err?
					//DebugTrace.print( " ## ERR onPlayerEnterZone:" + iid  );
				}
				else
				{
					//DebugTrace.print( "onPlayerEnterZone:" + iid  );
					_playerShowInfosByIid[ iid ] = p; 					 
					if( _initFlag )
					{
						createPlayer( p );
					}
				}
			}
		}		

		private void createZoneSprites()
		{
			if( !_mapChageFlag ) return;
			_initFlag = true;
			foreach ( Variant p in _playerShowInfosByIid.Values )
			{
				createPlayer( p );
			}
		}

		public LGAvatarGameInst get_player_by_cid( uint cid )
		{
			if( !_playersByCid.ContainsKey( cid ) ) return null;
			return _playersByCid[ cid ];
		}
		public LGAvatarGameInst get_player_by_iid( uint iid )
		{
			if( !_playersByIid.ContainsKey( iid ) ) return null;
			return _playersByIid[ iid ];
		}
        public Dictionary<uint, LGAvatarOther> getPlayers
        {
            get
            {
                return _playersByIid;
            }
        }
        public LGAvatarOther getNearPlayer(int range = 1000)
        {
            if (_playersByIid == null || _playersByIid.Values == null) return null;
            LGAvatarOther charPlayer = null;
            float curdis = 1000f;

            int x = lgSelfPlayer.instance.viewInfo["x"];
            int y = lgSelfPlayer.instance.viewInfo["y"];
            foreach (LGAvatarOther ct in _playersByIid.Values)
            {
                if (ct.IsDie())
                    continue;
                if (ct.IsCollect())
                    continue;

                float dis = Math.Abs(ct.x - x) + Math.Abs(ct.y - y);

                if (dis > range)
                    continue;

                if (charPlayer == null && !ct.IsDie())
                {
                    charPlayer = ct;
                    curdis = dis;
                }
                //  else if (Math.Abs((charMon.x - x) * (charMon.x - x)) + Math.Abs((charMon.y - y) * (charMon.y - y)) > Math.Abs((ct.x - x) * (ct.x - x)) + Math.Abs((ct.y - y) * (ct.y - y)))
                else if (curdis > dis && !ct.IsDie())
                {
                    charPlayer = ct;
                    curdis = dis;
                }
            }
            return charPlayer;
        }
		private void createPlayer( Variant m )
		{	 			 
			uint iid = m["iid"]._uint; 
			uint cid = m["cid"]._uint; 

			LGAvatarOther ct = new  LGAvatarOther( this.g_mgr );
			_playersByIid[ iid ] = ct;
			_playersByCid[ cid ] = ct;


			//if( info.ContainsKey("moving") )
			//{
			//	//DebugTrace.print("other player todo moving!");
			//}

			//if( info.ContainsKey( "atking" ) )
			//{
			//	//DebugTrace.print("other player todo atking!");
			//}

			//if( info.ContainsKey( "states" ) )
			//{
			//	//DebugTrace.print("other player todo states!");
			//} 

			ct.initData( m );
			ct.init();
			this.g_mgr.g_processM.addRender( ct );

		}

		
		
		public void disposeChar( uint iid )
		{
			LGAvatarOther ct = _playersByIid[ iid ];
		 
			ct.dispose();	
		} 
		
		//public void on_attchange(Variant msgData)
		//{
		//	uint iid = msgData["iid"]._uint;
		//	if( !_playersByIid.ContainsKey( iid ) ) return;
		//	LGAvatarOther ct = _playersByIid[ iid ];
		//	ct.onAttchange( msgData );
		//}

	}
}

