using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
using UnityEngine;
namespace MuGame
{
	
	class LGMonsters : lgGDBase, IObjectPlugin
	{

        public static LGMonsters instacne;
		public LGMonsters( gameManager m):base(m)
		{
            instacne = this;
		} 
		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGMonsters( m as gameManager  );
        }
		 
		private Dictionary< uint, LGAvatarMonster> _mons;
		private Dictionary< uint, Variant> _monInfos;
		private Dictionary< uint, Variant> _monWaitCreateInfos = new Dictionary<uint,Variant>();
		private bool _initFlag = false;
		private bool _mapChageFlag = false;

        public Dictionary<uint, LGAvatarMonster> getMons()
        {
            return _mons;
        }

        public LGAvatarMonster getMonsterById(uint id)
        {
            if (!_mons.ContainsKey(id)) return null;
            return _mons[id];
        }


		override public void init()
		{
			//return;	 
			_mons = new Dictionary< uint, LGAvatarMonster>();
			_monInfos = new  Dictionary< uint, Variant>();
			this.g_mgr.g_netM.addEventListener( 
				PKG_NAME.S2C_MONSTER_ENTER_ZONE, 
				onMonsterEnterZone 
			); 

            //this.g_mgr.g_netM.addEventListenerCL(
            //    OBJECT_NAME.DATA_JOIN_WORLD, 
            //    PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, 
            //    onMapchgBegin 
            //); 

			this.g_mgr.g_netM.addEventListener( 
				PKG_NAME.S2C_SPRITE_LEAVE_ZONE, 
				onSpriteLeaveZone 
			); 
 
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

            this.g_mgr.g_netM.addEventListener(
             PKG_NAME.S2C_MONSTER_SPAWN,
             onRespawn
         );


          

			this.g_mgr.g_processM.addProcess( new processStruct( update, "LGMonsters" ) );
		}		

		private void update(float tmSlice)
		{ 
			if( _monWaitCreateInfos.Count <= 0 ) return;
			foreach ( Variant p in _monWaitCreateInfos.Values )
			{
				createMon( p );
				uint iid = p["iid"]._uint;
				_monWaitCreateInfos.Remove( iid );
				break;
			}
		}

		private void onMapchgBegin( GameEvent e )
		{
			_mapChageFlag = false; 
			_initFlag = false;
			clear();
		}

		public void clear()
		{ 
			foreach ( LGAvatarMonster ct in _mons.Values )
			{
				this.g_mgr.g_processM.removeRender( ct );
				ct.dispose();
			}
			_mons.Clear();
			_monInfos.Clear();

            LGHeros.instacne.clear();
		}

        public void onMapchg(GameEvent e)
		{
            clear();
			_mapChageFlag = true; 
			createZoneSprites();
          
		}
		 
		private void onSpriteLeaveZone( GameEvent e )
		{ 
			Variant data = e.data;
			foreach( uint iid in  data["iidary"]._arr )
			{ 
				
				if( !_monInfos.ContainsKey( iid ) ) continue; 
				_monInfos.Remove( iid );

				if( _monWaitCreateInfos.ContainsKey( iid ) )
				{
					_monWaitCreateInfos.Remove( iid );
				}


				if( !_mons.ContainsKey( iid ) ) continue;
				LGAvatarMonster ct  = _mons[ iid ];
				_mons.Remove( iid );
				ct.dispose();
			}
			  
			  
		}

        private void onRespawn(GameEvent e)
        {
            Variant info = e.data;
            if (!info.ContainsKey("iid")) return;
            uint iid = info["iid"]._uint;

            if (!_mons.ContainsKey(iid))
                return;
            LGAvatarMonster mon = _mons[iid];
            mon.Respawn(info);
        }

		private void onMonsterEnterZone( GameEvent e )
		{ 
			Variant info = e.data;
			foreach ( Variant m in info["monsters"]._arr )
			{
				uint iid = m["iid"]._uint;
				if( _mons.ContainsKey( iid ) )
				{//err?
					
				}
                else if (m.ContainsKey("owner_cid"))
                {
                    LGHeros.instacne.onHeroEnterZone(m);
                }
                else
                {
                    _monInfos[iid] = m;
                    if (_initFlag) createMon(m);
                }
			}
		} 
		private void createZoneSprites()
		{
			if( !_mapChageFlag ) return;
			_initFlag = true;
			foreach ( Variant p in _monInfos.Values )
			{
				createMon( p );
			}
			_mapChageFlag = false;
		} 
		private void addCreateMon( Variant m )
		{	
			uint iid = m["iid"]._uint;
			_monWaitCreateInfos[ iid ] = m;
		}

		public void createMon( Variant m )
		{	 
			int monid = m["mid"]._int;
			uint iid = m["iid"]._uint;

           // debug.Log("!!createMon!! monid:" + monid + " " + iid + " " + debug.count);

            Variant b = MonsterConfig.instance.conf;
            Variant mconf = b["monsters"][monid+""];

			if( mconf == null ) 
			{
				GameTools.PrintError( " mon[ "+   monid +" ] no conf ERR!" );
				return;
			}

            m["x"] = m["x"] / GameConstant.PIXEL_TRANS_UNITYPOS;
            m["y"] = m["y"] / GameConstant.PIXEL_TRANS_UNITYPOS;

			LGAvatarMonster ct = new LGAvatarMonster( this.g_mgr );
			_mons[ iid ] = ct;

           

			//if( info.ContainsKey( "owner_cid" ) )
			//{
			//	GameTools.PrintNotice("mon todo owner_cid!");
			//}

			//if( info.ContainsKey("moving") )
			//{
			//	GameTools.PrintNotice("mon todo moving!");
			//}

			//if( info.ContainsKey( "atking" ) )
			//{
			//	GameTools.PrintNotice("mon todo atking!");
			//}

			//if( info.ContainsKey( "states" ) )
			//{
			//	GameTools.PrintNotice("mon todo states!");
			//} 
		 
			ct.initData( m );
			ct.init();

			this.g_mgr.g_processM.addRender( ct );
           // GameRoomMgr.getInstance().onMonsterEnterView(ct.grAvatar);
        }
 
		public LGAvatarMonster get_mon_by_iid( uint iid )
		{
			if( !_mons.ContainsKey( iid ) ) return null;
			return _mons[ iid ];
		}

		public LGAvatarMonster get_mon_by_mid( uint mid )
		{
			if( _mons == null || _mons.Values == null ) return null;	
			foreach( LGAvatarMonster m in _mons.Values )
			{
				if( m.getMid() != mid ) continue;
				return m;
			}
			return null;
		}

        //public void on_attchange(Variant msgData)
        //{
        //    uint iid = msgData["iid"]._uint;
        //    if( !_mons.ContainsKey( iid ) ) return;
        //    LGAvatarMonster ct = _mons[ iid ];
        //    ct.onAttchange( msgData );
        //}

        public LGAvatarMonster getNearMon(int range=1000)
        {
			if( _mons == null || _mons.Values ==null ) return null;
            LGAvatarMonster charMon = null;
            float curdis=1000f;

            Vector3 vec =  lgMainPlayer.unityPos;
            float x = vec.x;
            float y = vec.z;
            foreach ( LGAvatarMonster ct in _mons.Values )
			{
                if (ct.IsDie())
                    continue;
                if(ct.IsCollect())
                    continue;
                Vector3 vecmon = ct.unityPos;
                float dis = Math.Abs(vecmon.x - x) + Math.Abs(vecmon.y - y);

                if (dis > range)
                    continue;

                if (charMon == null && !ct.IsDie())
                {
                    charMon = ct;
                    curdis = dis;
                }
              //  else if (Math.Abs((charMon.x - x) * (charMon.x - x)) + Math.Abs((charMon.y - y) * (charMon.y - y)) > Math.Abs((ct.x - x) * (ct.x - x)) + Math.Abs((ct.y - y) * (ct.y - y)))
                else if (curdis > dis && !ct.IsDie())
                {
                    charMon = ct;
                    curdis = dis;
                }
			}
            return charMon;
        }


        private lgSelfPlayer lgMainPlayer
        {
            get
            {
                return this.g_mgr.getObject(OBJECT_NAME.LG_MAIN_PLAY) as lgSelfPlayer;
            }
        }
		 
	}
}

