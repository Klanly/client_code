using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	 
	
	class LGNpcs : lgGDBase, IObjectPlugin
	{
        public static LGNpcs instance;
		public LGNpcs( gameManager m):base(m)
		{
            instance = this;
		} 
		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGNpcs( m as gameManager  );
        }
		 
		private Dictionary< string, LGAvatarNpc> _npcs;

        public LGAvatarNpc getNpc(int npcid)
        {
            if (_npcs == null || !_npcs.ContainsKey(npcid.ToString()))
                return null;

            return _npcs[npcid.ToString()] as LGAvatarNpc;
        }

        public Dictionary<string, LGAvatarNpc> getNpcs()
        {
            return _npcs;
        }

		override public void init()
		{
			//return;
			_npcs = new Dictionary< string, LGAvatarNpc>(); 

			//this.g_mgr.g_gameM.addEventListenerCL( 
			//	OBJECT_NAME.LG_MAP,
			//	GAME_EVENT.MAP_INIT, 
			//	onMapchg 
			//); 

            //this.g_mgr.g_gameM.addEventListenerCL( 
            //    OBJECT_NAME.LG_JOIN_WORLD,
            //    GAME_EVENT.ON_MAP_CHANGE, 
            //    onMapchg 
            //);  

            //this.g_mgr.g_gameM.addEventListenerCL( 
            //    OBJECT_NAME.LG_JOIN_WORLD,
            //    GAME_EVENT.ON_ENTER_GAME, 
            //    onMapchg 
            //); 

		}

		private void clear()
		{ 
			foreach ( LGAvatarNpc ct in _npcs.Values )
			{
				ct.dispose();
			}
			_npcs.Clear(); 
		}

		public void onMapchg()
		{ 
			clear();
			createMapNpcs();
		}



		private void createMapNpcs()
		{
            if (SelfRole.s_bStandaloneScene) return;

            joinWorldInfo jinfo = this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD)
                as joinWorldInfo;

            Variant pinfo = jinfo.mainPlayerInfo;

            uint mapid = jinfo.mapid;
			
			
			Variant conf =  ( 
				this.g_mgr.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_MAP ) 
				as SvrMapConfig
			) .getSingleMapConf( mapid );

			if(conf == null || !conf.ContainsKey("n") ) 
                return;
			 
			Variant npcs = conf["n"];
			foreach( Variant n in npcs._arr )
			{
				int nid = int.Parse( n["nid"] );
				Variant npc =  ( 
					this.g_mgr.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_NPC ) 
					as SvrNPCConfig
				).get_npc_data( nid );

				if( npc == null ) 
				{
					//DebugTrace.print( "map[ "+mapid+" ] npc[ "+nid +" ] no data ERR!" );
					continue;
				}
				LGAvatarNpc ct = new LGAvatarNpc( this.g_mgr );
				_npcs[ nid.ToString() ] = ct;

                if (npc.ContainsKey("defdid"))
                    ct.dialogId = npc["defdid"];

				Variant info = new Variant();
                info["x"] = n["x"]._int/ GameConstant.GEZI_TRANS_UNITYPOS;
                info["y"] = n["y"]._int / GameConstant.GEZI_TRANS_UNITYPOS;
				info[ "nid" ] = n["nid"];

				info[ "name" ] = npc["name"];
				info[ "octOri" ] = n["r"];
				 
				ct.initData( info );
				ct.init();
			}
			 
		}


		 
	}
}
