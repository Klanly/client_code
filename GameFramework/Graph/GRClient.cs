
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
	 
	abstract public class GRClient : clientBase
	{
		private GRWorld3D _world;		
		private Variant _size = new Variant(); 
		public static GRClient instance; 


		public GRClient( gameMain m):base(m)
		{
            instance = this;
		}

		private GraphManager GraphMgr 
		{
			get{
				return CrossApp.singleton.getPlugin("graph") as GraphManager;
			}
		}
		public Variant getMapConf( string mapid )
		{
			Variant conf = GraphMgr.getMapConf( mapid );
			if ( conf == null )
			{
				DebugTrace.print( "  >>> ERR!  getMapConf mapid["+ mapid +"] null!" );
			}

			return conf;
		}

		private Dictionary< string, Variant > _mapInfos = null;
		public Variant getGMapInfo(string id)
        {
			if( _mapInfos == null )
			{
				_mapInfos = new Dictionary<string,Variant>();
				foreach ( Variant m in  GraphMgr.MapInfo._arr )
				{		
					if( m.ContainsKey("camera") )
					{
						m["camera"] = m["camera"]._arr[0];
					}			 
					_mapInfos[ m["id"]._str ] = m;
				}

			}
			if ( !_mapInfos.ContainsKey( id ) ) return null;
            return _mapInfos[ id ];
        }

		public Variant getGMapCameraInfo(string id)
        {
			Variant n = getGMapInfo( id );
			if( n == null ) return null;
			if( !n.ContainsKey("camera") )  return null;
            return n[ "camera" ];
        }

        public Variant getMapInfo
        {
            get 
            {
                return GraphMgr.MapInfo;
            }
        }
		public Variant getEntityConf( string avatarid )
		{
			Variant conf = this.GraphMgr.getCharacterConf( avatarid );
			if ( conf == null )
			{
				DebugTrace.print( "  >>> ERR!  getCharacterConf avatarid["+ avatarid +"] null!" );
			}
			return conf;
		} 
		public Variant getAvatarConf( string chaid, string avaID, bool isleft=false )
		{
			if( isleft )
			{//todoooo
				avaID = avaID+"L";
			}
			Variant conf = this.GraphMgr.getAvatarConf( chaid, avaID );
			if ( conf == null )
			{
				DebugTrace.print( "  >>> ERR!  getCharacterConf  chaid["+ chaid +"] avaID["+ avaID +"] null!" );
			}
			return conf;
		} 
		public Variant getEffectConf( string effid )
		{
			Variant conf = GraphMgr.getEffectConf( effid );
			 
			if ( conf == null )
			{
				DebugTrace.print( "  >>> ERR!  getEffectConf  effid["+ effid +"] null!" );
			}
			return conf;
		} 

		override public void init()
		{
			_size.setToDct();
			_size["w"] = 1400;
			_size["h"] = 800;
			_world = new GRWorld3D("mainWorld", this.GraphMgr);

			onInit();
		}
		abstract protected void onInit();
		//{
		//		regCreator( OBJECT_NAME_DEF.MAP, mapctrl );
		//}

		public Variant getSize() 
		{
			return _size;
		}
		public void setScreenSize( int w, int h )
		{
			_size["w"] = w;
			_size["h"] = h;
			
			//todo 
		} 
 

		//========				 
		virtual public string getNPCAvatar( uint npcid )
		{
			return "105";
		}

		virtual public string getAvatarId( uint sex, uint carr )
		{
			return "107";
		}

		virtual public string getMonAvatarId( uint mid )
		{
			return "106";
		}
        virtual public string getHeroAvararId(uint mid)
		{
			return "10001";
		}
        
		virtual public avatarInfo getEqpAvatarInfo( uint tpid, Variant eqp )
		{
			return null;
		}

        abstract public float getZ(float x, float y);
		

		public Vec3 trans3DPos(float x, float y )
		{
			Vec3 ret = new Vec3();
            ret.x = x;//(float)GameTools.inst.pixelToUnit( (double)x );
			ret.y = y;//(float)GameTools.inst.pixelToUnit( (double)y );
			ret.z = (float)getZ( x, y);
			return ret;
		}

	 


		//========


		public void createMapObject( lgGDBase mapCtrl )
		{ 
			createSceneObjectPair( 
				mapCtrl, 
				OBJECT_NAME_DEF.SCENE_MAP_CTRL, 
				OBJECT_NAME_DEF.SCENE_MAP_DRAW 
			);
		}

        //public void createAvatarObject(lgGDBase avatarCtrl)
        //{		
        //    //createSceneObjectPair( mapCtrl, OBJECT_NAME_DEF.SCENE_MAP_CTRL, OBJECT_NAME_DEF.SCENE_MAP_DRAW );			
        //}
        public void createMainAvatarObject(lgGDBase avatarCtrl)
		{		
			createSceneObjectPair( 
				avatarCtrl,
				OBJECT_NAME_DEF.SCENE_MAIN_PLAY_CTRL, 
				OBJECT_NAME_DEF.SCENE_MAIN_PLAY_DRAW 
			);			
		}

        public void createCamera(lgGDBase cameraCtrl)
		{
			createSceneObjectPair( 
				cameraCtrl,
				OBJECT_NAME_DEF.SCENE_CAMERA_CTRL, 
				OBJECT_NAME_DEF.SCENE_CAMERA_DRAW 
			);		
		}

        public void createOtherPlayer(lgGDBase otherPlayerCtrl)
		{
			createSceneObjectPair( 
				otherPlayerCtrl,
				OBJECT_NAME_DEF.SCENE_OTHER_PLAY_CTRL, 
				OBJECT_NAME_DEF.SCENE_OTHER_PLAY_DRAW 
			);		
		}

		public void createMonster( lgGDBase monsterCtrl )
		{
			createSceneObjectPair( 
				monsterCtrl,
				OBJECT_NAME_DEF.SCENE_MONSTER_CTRL, 
				OBJECT_NAME_DEF.SCENE_MONSTER_DRAW 
			);		
		}

        public void createHero(lgGDBase heroCtrl)
		{
			createSceneObjectPair(
                heroCtrl,
				OBJECT_NAME_DEF.SCENE_HERO_CTRL, 
				OBJECT_NAME_DEF.SCENE_HERO_DRAW 
			);		
		}

        public void createNPC(lgGDBase npcCtrl)
		{
			createSceneObjectPair( 
				npcCtrl,
				OBJECT_NAME_DEF.SCENE_NPC_CTRL, 
				OBJECT_NAME_DEF.SCENE_NPC_DRAW 
			);		
		}

 
		private void createSceneObjectPair( lgGDBase mapCtrl, string ctrlName, string drawName  )
		{
			LGGRBaseImpls sctrl = createInst( ctrlName, false ) as LGGRBaseImpls;
			GRBaseImpls dbase = createInst( drawName, false  ) as GRBaseImpls;

			if( sctrl == null )
			{
				DebugTrace.print( "GRClient create ctrlName["+ctrlName+"] null!");
				return;
			}
			if( dbase == null )
			{
				DebugTrace.print( "GRClient create drawName["+drawName+"] null!");
				return;
			}


            mapCtrl.initGr(dbase, sctrl);
            dbase.initLg(mapCtrl);
			sctrl.init();			 
			dbase.init();

			sctrl.setGameCtrl( mapCtrl );

			sctrl.setDrawBase( dbase );
			dbase.setSceneCtrl( sctrl );

			if( OBJECT_NAME_DEF.SCENE_MAIN_PLAY_CTRL == ctrlName )
			{
				this.g_processM.addRender( sctrl, true );
				this.g_processM.addRender( dbase, true );
			}
			else
			{
				this.g_processM.addRender( sctrl );
				this.g_processM.addRender( dbase );
			}
		}
		 
 
		//=======================

		public IGREffectParticles createEffect( string id ) 
		{ 
			IGREffectParticles eff = _world.createEntity(Define.GREntityType.EFFECT_PARTICLE, id ) as IGREffectParticles;
             string path;
            if (id.Length < 20)
            {
                Variant conf = GraphMgr.getEffectConf(id);
                if (conf == null)
                {
                    GameTools.PrintError("createEffect[" + id + "] no conf ERR!");
                    return null;
                }
                path = conf["file"]._str;
            }
            else
            {
                path = id;
            }

            eff.asset = os.asset.getAsset<IAssetParticles>(path);           
			if( eff == null )
			{
				 GameTools.PrintError( "createEffect["+id+"] ERR!" );
				 return null;
			} 
			
            return eff;
        } 

		public IGRMap createGraphMap( string mapid )
		{
			IGRMap grMap = _world.createMap( mapid );
			return grMap;
		}

        //public GRCamera3D createGraphCamera()
        //{
        //    GRCamera3D m_camera = 
        //        _world.createEntity( Define.GREntityType.CAMERA ) as GRCamera3D;
        //    return m_camera;
        //}
		public GRCamera3D getGraphCamera()
		{
			GRCamera3D m_camera = _world.cam;
			return m_camera;
		}
		public GRCharacter3D createGraphChar( Variant conf )
		{
		
			GRCharacter3D m_cha = 
				_world.createEntity( Define.GREntityType.CHARACTER ) as GRCharacter3D;
			
			m_cha.load( conf );
			return m_cha;
 
		}

		public void deleteEntity( IGREntity ent )
		{
			_world.deleteEntity( ent );
		}
        //virtual public GRCharacter3D createGraphCharShadow( string avatarid )
        //{
        //    return null;
        //}
		 
        public GRWorld3D world
        {
            get 
            {
                if (_world != null)
                    return _world;
                return null;
            }
        }
	}
	public class avatarInfo
    {
		
		public uint tpid = 0;
		public string avatarid = "";
		public int pos = 0;
		public int subtp = 0;
		public string eqppos = "r";//"l"

		//¶¯Ì¬Êý¾Ý
		public bool isLeft = false;
		public int flvl = 0;
		public uint iid = 0;
	}
}