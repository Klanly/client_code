using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{


    class LGMap : lgGDBase, IObjectPlugin
    {
        private uint _tile_size = 0;
        private uint _globaMapW = 0;
        private uint _globaMapH = 0;
        private uint _currMapid = 0;
        private mapCalc _mapCalc;
		protected Variant _tmpLinks = new Variant();

        public uint m_unMapWidth = 0;
        public uint m_unMapHeight = 0;

        public Variant _viewInfo = new Variant();
        public LGMap(gameManager m)
            : base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new LGMap(m as gameManager);
        }

		

        override public void init()
        {
            _mapCalc = new mapCalc(this);
            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_ENTER_GAME,
                onJoinWorld
            );

            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_MAP_CHANGE,
                onChangeMap
            );
            this.g_mgr.addEventListener(
                GAME_EVENT.MAP_ADD_FLY_EFF,
                onAddFlyEff
            );

        }
		public Variant getViewInfo()
        {
            return _viewInfo;
        }

        public uint curMapId
        {
            get
            {
                return _currMapid;
            }
        }

		public uint curMapWidth
		{
			get
            {
                Variant conf = currMapSvrConf();
				if (conf != null)
				{
					uint tile_size = conf["tile_size"]._uint;
					uint globaMapW = conf["width"]._uint * _tile_size;
					 
					return globaMapW;
				}
				return 0;
            }
		}

		public uint curMapHeight
		{
			get
            {
                Variant conf = currMapSvrConf();
				if (conf != null)
				{
					uint tile_size = conf["tile_size"]._uint;
					uint globaMapH = conf["height"]._uint * _tile_size;			
					return globaMapH;		 
				}
				return 0;
            }
		}
        public bool showFlag()
        {
            return _currMapid > 0;
        }
		public Variant tmpLinks
		{
			get{			
				return _tmpLinks;
			}
		}
        private void onChangeMap(GameEvent e)
        {
            Variant einfo = refreshMapInfo();
            this.dispatchEvent(GameEvent.Create(GAME_EVENT.MAP_CHANGE, this, einfo));
        }

        private void onJoinWorld(GameEvent e)
        {
           // DelayDoManager.singleton.AddDelayDo(playMapMusic, 10, DelayDoManager.CF_CHANGE_MAP);
           
            trySetDrawBase();
        }

        private void onAddFlyEff(GameEvent e)
        {
            this.dispatchEvent(GameEvent.Createimmedi(GAME_EVENT.MAP_ADD_FLY_EFF, this, e.data));
        }

        public void EnterStandalone()
        {
            this.g_mgr.g_sceneM.dispatchEvent( GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_MAP, this, null));

            SvrMapConfig svrMapConfig = GRClient.instance.g_gameConfM.getObject(OBJECT_NAME.CONF_SERVER_MAP) as SvrMapConfig;

            uint mapid = 3333;
            Variant conf = svrMapConfig.getSingleMapConf(mapid);

            Variant einfo = new Variant();
            //einfo["localConf"] = localConf;
            einfo["conf"] = conf;
            einfo["mapid"] = mapid;
            einfo["tmpLinks"] = new Variant();

            //Variant einfo = refreshMapInfo();
            //this.dispatchEvent(GameEvent.Create(GAME_EVENT.MAP_INIT, this, einfo));

            this.dispatchEvent(GameEvent.Create(GAME_EVENT.MAP_INIT, this, einfo));
        }

        //public bool grdMove(double x, double y)
        //{
        //    short[] m_grdValue = (this.g_mgr.g_gameConfM as muCLientConfig).localGrd.grd;
        //    if (m_grdValue == null)
        //        return false;

        //    double ratio = GameConstant.GEZI_HDT * 0.4;

        //    int height = (int)((this.g_mgr.g_gameConfM as muCLientConfig).localGrd.grayWidth / ratio);
        //    int width = (int)((this.g_mgr.g_gameConfM as muCLientConfig).localGrd.grayHeight / ratio);

        //    int ix = (int)(x / ratio);
        //    int iy = (int)(y / ratio);

        //    if (ix < 0 || iy < 0 || ix >= width || iy >= height) //超出地图范围，一律认为不可行
        //        return false;

        //    return (m_grdValue[iy * width + ix] == 0);
        //}

        public LGAvatar get_player_by_cid(uint cid)
        {             
            if (cid == selfPlayer.getCid())
            {
                return selfPlayer;
            }
            return otherPlayers.get_player_by_cid(cid);
        }

        public LGAvatar get_Character_by_iid(uint iid)
        {
            return selfPlayer.get_Character_by_iid( iid );            
        }

		public LGAvatar get_mon_by_mid(uint mid)
        {
            return monsters.get_mon_by_mid( mid );            
        }
		public LGAvatar get_NPC(uint nid)
        {
            return lgNpcs.getNpc( (int)nid );            
        }

        private void trySetDrawBase()
        {
            if(GRMap.instance != null)
                this.dispatchEvent(GameEvent.Create(GAME_EVENT.SCENE_CREATE_MAP, this, null));
            else
                this.g_mgr.g_sceneM.dispatchEvent(GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_MAP, this, null));

            Variant einfo = refreshMapInfo();		


            this.dispatchEvent(GameEvent.Create(GAME_EVENT.MAP_INIT, this, einfo));
        }


		private void refreshCameraParma()
		{
            ////_currMapid			 
            //Variant info = this.g_mgr.g_sceneM.getGMapCameraInfo( _currMapid.ToString() );
            //if( info == null )
            //{

            //    GameConstant.CAMERA_ROTATION_X = 45f;
            //    GameConstant.CAMERA_ROTATION_Y = 45f;
            //    GameConstant.CAMERA_ROTATION_Z = 0f;
            //    GameConstant.CAMERA_FIELD_VIEW = 15f;
            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_X = -7.25f;
            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Y = 10.98f;
            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Z = -7.3f;
            //}
            //else
            //{
            //    //debug.Log("当前地图的ID="+_currMapid);
            //    //debug.Log(info.dump());

            //    GameConstant.CAMERA_ROTATION_X = info["rx"]._float;
            //    GameConstant.CAMERA_ROTATION_Y = info["ry"]._float;
            //    GameConstant.CAMERA_ROTATION_Z = info["rz"]._float;
            //    GameConstant.CAMERA_FIELD_VIEW = info["fview"]._float; 

            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_X = info["x"]._float;
            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Y = info["y"]._float;
            //    GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Z = info["z"]._float;
            //}

            //GameConstant.REFRESH_CAMERA_ROT_AND_FOV = true;
            //joystick.refreshAngleOffset(GameConstant.CAMERA_ROTATION_Y);
		}
		private Variant currMapLocalConf()
		{
			if( _currMapid <= 0 )
			{
				//DebugTrace.print( "get currMapLocalConf err!" );
				return null;
			}
			return this.g_mgr.g_sceneM.getMapConf( _currMapid.ToString() );
		}
		private Variant currMapSvrConf()
		{
			if( _currMapid <= 0 )
			{
				//DebugTrace.print( "get currMapSvrConf err!" );
				return null;
			}
			 Variant conf = svrMapConfig.getSingleMapConf( _currMapid );
			return conf;
		}

        private Variant refreshMapInfo()
        {
			joinWorldInfo jinfo =   this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD) as joinWorldInfo;
            
            Variant pinfo = jinfo.mainPlayerInfo;

            _currMapid = jinfo.mapid;

            //todo map conf
            Variant localConf = currMapLocalConf();


            Variant conf = currMapSvrConf();
            if (conf != null)
            {
                _tile_size = conf["tile_size"];
                m_unMapWidth = conf["width"];
                m_unMapHeight = conf["height"];
                _globaMapW = conf["width"] * _tile_size;
                _globaMapH = conf["height"] * _tile_size;
            }

            //_pause = false;

            Variant mcale = new Variant();
            Variant param = new Variant();
            mcale["param"] = param;
            param["width"] = _globaMapW;
            param["height"] = _globaMapH;
            //gridData

            //short[] grd = (this.g_mgr.g_gameConfM as muCLientConfig).localGrd.grd;

            //_mapCalc.changeMap(mcale, grd);

            //refreshCameraParma();

      //      formatXY(pinfo["x"], pinfo["y"]);

            Variant einfo = new Variant();
            einfo["localConf"] = localConf;
            einfo["conf"] = conf;
            einfo["mapid"] = _currMapid;
			einfo["tmpLinks"] = _tmpLinks;
            return einfo;
        }
        private void formatXY(int x, int y)
        {

        }


        public bool IsWalkAble(int gx, int gy)
        {
            return _mapCalc.IsWalkAble(gx, gy);
        }

        //public bool IsCovered(int x, int y)
        //{
        //    return _mapCalc.IsCovered(x, y);
        //}
        public List<GridST> findPath(Vec2 start, Vec2 end)
        {
            return _mapCalc.findPath(start, end);
        }

		public List<uint> getMapPath(uint curMapId, uint dest_map_id) 
		{
            return _mapCalc.get_map_path( curMapId, dest_map_id );
        }

        // get pixel position by grid position
        public Vec2 getPPosByGPos(float gridX, float gridY)
        {
            return new Vec2((int)(gridX * GameConstant.GEZI + GameConstant.GEZI / 2f), (int)(gridY * GameConstant.GEZI + GameConstant.GEZI / 2f) );
        }
        public Vec2 getGPosByPPos(float pixelX, float pixelY)
        {
            return new Vec2((int)(pixelX / GameConstant.GEZI), (int)(pixelY / GameConstant.GEZI));
        }

		public float getFarthestDistance( float stPixelX, float stPixelY, float ori )
        {// 
			Vec2 p = getFarthestGPosByOri( stPixelX, stPixelY, ori );
			int subx = ( int )( p.x*GameConstant.GEZI - stPixelX );
			int suby = ( int )( p.y*GameConstant.GEZI - stPixelY );
			return (float)Math.Sqrt( subx*subx + suby*suby );           
        }

		//获取 射线上最远可行点
        public Vec2 getFarthestGPosByOri(float stPixelX, float stPixelY, float radian,float gezi_distance=707)
        {// 
			Vec2 p = getGPosByPPos( stPixelX, stPixelY );	
			float grid_x = p.x;
			float grid_y = p.y; 
 
			float ori_cos = (float)Math.Cos( radian );
			float ori_sin = (float)Math.Sin( radian );
			float ori_tan = ori_sin/ori_cos;

			float incr_x = ori_cos >= 0 ? 1 : -1;
			float incr_y = ori_sin >= 0 ? 1 : -1;

			float tmp = Math.Abs(ori_tan);
			bool not_cross = Math.Abs( tmp - 1 ) >= 0.01; //是否对角线  0.01为容错值
			bool main_y = tmp > 1;

			//sys.trace( sys.SLT_ERR, "find_furthest_canwalk_grid radian=[" + radian + "]    ori_tan=[" + ori_tan + "]   ori_cos=[" + ori_cos + "]  ori_sin=[" + ori_sin + "] tmp=[" + tmp + "]\n" );
			 //以变化大的为基准  依次遍历经过的所有格子（与直线相交的）
			float grid_fir = grid_x;
			float grid_sec = grid_y;
			float incr_fir = incr_x;
			float incr_sec = incr_y;
			float radio = ori_tan;
			if ( main_y )
			{
				grid_fir = grid_y;
				grid_sec = grid_x;
				incr_fir = incr_y;
				incr_sec = incr_x;
				radio = 1/ori_tan;
			}

			float add_grid_fir = 0;
			float add_grid_sec = 0;  
			while ( true )
			{
                if (gezi_distance<707 && gezi_distance <= Math.Abs(add_grid_fir))
                    break;

				add_grid_fir += incr_fir;  
				tmp = (int)( add_grid_fir * radio );
				if ( not_cross )
				{
					if ( tmp != add_grid_sec )
					{
						if ( tmp-add_grid_sec != incr_sec )
						{
						}

						add_grid_sec = tmp;
						add_grid_fir -= incr_fir; //退格
					}
				}
				else
				{
					add_grid_sec += incr_sec;
				}
            
				//sys.trace( sys.SLT_ERR, "get_ray_end_grid main_y=[" + main_y + "] add_grid_fir=[" + add_grid_fir + "]  add_grid_sec=[" + add_grid_sec + "]  incr_fir=[" + incr_fir + "] incr_sec=[" + incr_sec + "]\n" );
				if ( main_y )
				{
					if( !IsWalkAble( (int)( grid_sec+add_grid_sec), (int)(grid_fir+add_grid_fir) ) )
					{
						break;
					}
					grid_x = grid_sec+add_grid_sec;
					grid_y = grid_fir+add_grid_fir;
				}
				else
				{
					if( !IsWalkAble( (int)(grid_fir+add_grid_fir), (int)(grid_sec+add_grid_sec) ) )
					{
						break;
					}
					grid_y = grid_sec+add_grid_sec;
					grid_x = grid_fir+add_grid_fir;
				}
			}
 
			return new Vec2( grid_x, grid_y );           
        }



		public void AddTempLinks( List<Variant> linkDatas )
		{
			long currTm = this.g_mgr.g_netM.CurServerTimeStampMS;
			Variant addlinks = new Variant();
			foreach( Variant linkData in linkDatas )
			{
				linkData["goto"] = linkData["gto"];
				linkData.RemoveKey( "gto" );
				Variant linfo = new Variant();
				linfo["data"] = linkData;
				linfo["ctm"] = currTm;
				_tmpLinks.pushBack( linfo );
				addlinks.pushBack( linfo );			 
			}
						
			dispatchEvent(
				GameEvent.Create( GAME_EVENT.MAP_LINK_ADD, this, addlinks )
			);
		}

		public Variant get_map_link( uint mapid )
		{
			Variant curMapConf = currMapSvrConf();
			if( !curMapConf.ContainsKey("l") ) return null;
			List<Variant> linkarray = curMapConf["l"]._arr;
			int i;
			Variant ln;
			for(i = 0;i < linkarray.Count;i++)
			{
				 ln = linkarray[i];
				 if( ln["gto"]._uint == mapid )
				 {
					return ln;
				 }
			}
			if( _tmpLinks.Values != null )
			{	
				foreach( Variant l in _tmpLinks.Values )
				{	
					if( l["data"]["goto"]._uint == mapid )
					{
						return l["data"];
					}
				}
			}
			return null;
		}

		public Variant get_map_link( int grid_x, int grid_y )
		{
			Variant curMapConf = currMapSvrConf();
			if( !curMapConf.ContainsKey("l") ) return null;
			List<Variant> linkarray = curMapConf["l"]._arr;
			for(int i = 0;i < linkarray.Count;i++)
			{
				int lx = linkarray[i]["x"];
				int ly = linkarray[i]["y"];
				if( Math.Abs( lx - grid_x ) <= GameConstant.LINK_ACT_GRID_RANGE && Math.Abs( ly - grid_y ) <=  GameConstant.LINK_ACT_GRID_RANGE )
				{
					return linkarray[i];
				}
			}
			if( _tmpLinks.Values != null )
			{	
				foreach( Variant linkData in _tmpLinks.Values )
				{	
					int lx = linkData["data"]["x"]._int;
					int ly = linkData["data"]["y"]._int;
				 
					if(Math.Abs( lx - grid_x) <  GameConstant.LINK_ACT_GRID_RANGE && Math.Abs( ly - grid_y) <  GameConstant.LINK_ACT_GRID_RANGE )
					{
						return linkData["data"];
					}
				}
			}
			
			return null;
		}
		public int pixelToGridSize( float psize )
        {
            return (int)(psize / GameConstant.GEZI );
        }
		public float gridSizeToPixel( int gridSize )
        {
            return gridSize * GameConstant.GEZI;
        }


		public void beginChangeMap( uint mapid )
		{
			msgMap.change_map( mapid );
		}

        public void playMapMusic(bool force=false)
		{

            //Variant data = (CrossApp.singleton.getPlugin("graph") as GraphManager).getGMapInfo(this.curMapId);
            Variant data = GRClient.instance.g_sceneM.getGMapInfo(this.curMapId.ToString());
            if (data != null)
            {
              
                //string path = data["music_file"];
                //if(path!=null)
                //    MediaClient.getInstance().PlayMusicUrl(path, null, force);
           }

            //conf = (this.g_mgr.g_gameConfM as muCLientConfig).localGeneral.getMapSound(this.curMapId);
            //if( conf != null )
            //{
            //    this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(GAME_EVENT.LG_MEDIA_PLAY, this, GameTools.createGroup("sid", conf["sid"]._str, "loop", true)));
            //    //MediaClient.singleton.PlaySound(conf["sid"]._str, true);	
            //}
		}
        public int GetPKState()
        {
            Variant conf = currMapSvrConf();
            return conf != null && conf.ContainsKey("pk") ? conf["pk"]._int : 0;
        }



		// ===== objs ====	
		private SvrMapConfig svrMapConfig
		{
			get{
				return this.g_mgr.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_MAP )  as SvrMapConfig;			
			}
		}	 
		private InGameMapMsgs msgMap
		{
			get{
				return this.g_mgr.g_netM.getObject( OBJECT_NAME.MSG_MAP ) as InGameMapMsgs;
			}
		}
		private lgSelfPlayer selfPlayer 
		{
			get{
				return this.g_mgr.getObject( OBJECT_NAME.LG_MAIN_PLAY ) as lgSelfPlayer;
			}
		}
		private LGOthers otherPlayers 
		{
			get{
				return this.g_mgr.getObject( OBJECT_NAME.LG_OTHER_PLAYERS ) as LGOthers;
			}
		}
		private LGMonsters monsters 
		{
			get{
				return this.g_mgr.getObject( OBJECT_NAME.LG_MONSTERS ) as LGMonsters;
			}
		}
		private LGNpcs lgNpcs 
		{
			get{
				return this.g_mgr.getObject( OBJECT_NAME.LG_NPCS ) as LGNpcs;
			}
		}
        //private MediaClient m_media
        //{
        //    get
        //    {
        //        return MediaClient.getInstance();
        //    }
        //} 

    }


    class GridST
    {
        public float x = 0;
        public float y = 0;

        public int G = 0;
        public int H = 0; //当前格到终点格的距离
        public int F = 0; // F = G + H
        public GridST parent = null;//当前方格的父方格
        public bool isCorner = false;
        public bool isEnd = false;
        public uint tile_idx = 0;
        public GridST()
        {
        }
    }



    class mapCalc
    {

        private LGMap _lgmap;
        private Variant _curMapConf;
        private short[] _grd;
        //碰撞信息,记录方法 1为不可通过,0为可通过
        private Dictionary<int, uint> _CollideInfoEx = new Dictionary<int, uint>(); // for new pathfinding
        //private Dictionary<int, uint> _CoverInfoEx = new Dictionary<int, uint>(); // for new pathfinding

        private Variant _BlockZones = new Variant();
        private Dictionary<int, uint> _BlockZoneInfo = new Dictionary<int, uint>();

        public mapCalc(LGMap m)
        {
            _lgmap = m;
        }

        // --------------------------------------------------------------------
        // map change & initialize

        //{param:, gridData:}
        public void changeMap(Variant mpconf, short[] grd)
        {
            _curMapConf = mpconf;
            _grd = grd;
            _clearCurMap();
            _initCurMap();
        }

        protected void _initCurMap()
        {
            _initCollide();
        }
        protected void _clearCurMap()
        {
            _CollideInfoEx.Clear();
            //_CoverInfoEx.Clear();

            _BlockZones = new Variant();
            _BlockZoneInfo.Clear();
        }
        // --------------------------------------------------------------------
        // block zone

        private void _calcBlockZoneInfo()
        {
            _BlockZoneInfo.Clear();

            foreach (Variant bzinfo in _BlockZones.Values)
            {
                for (int x = bzinfo["left"]; x < bzinfo["right"]; ++x)
                {
                    for (int y = bzinfo["top"]; y < bzinfo["bottom"]; ++y)
                    {
                        _BlockZoneInfo[(y << 9 | x & 0x000001ff)] = 1;
                    }
                }
            }
        }
        //private void _calcBlockZoneInfo()
        //{
        //	_BlockZoneInfo.Clear();

        //	foreach (Variant bzinfo in _BlockZones.Values )
        //	{
        //		for(int x = bzinfo.left; x < bzinfo.right; ++x)
        //		{
        //			for(int y = bzinfo.top; y < bzinfo.bottom; ++y)
        //			{
        //				_BlockZoneInfo[(y<<9 | x&0x000001ff )] = 1;
        //			}
        //		}
        //	}
        //}


        public void addBlockZone(Array data)
        {
            foreach (Variant bzinfo in data)
            {
                _BlockZones[bzinfo["id"]] = bzinfo;
            }

            _calcBlockZoneInfo();
        }
        //public void rmvBlockZone(Array data)
        //{
        //	foreach (Variant bzinfo in data)
        //	{
        //		if(!(bzinfo._BlockZones.ContainsKey( id )))
        //		{
        //			continue;
        //		}

        //		delete _BlockZones[bzinfo.id];
        //	}

        //	_calcBlockZoneInfo();
        //}
        public void clearBlockZone()
        {
            _BlockZones = new Variant();

            _calcBlockZoneInfo();
        }

        // --------------------------------------------------------------------
        // grid collide
        private int _get_tile_id(int idx)
        {
            if (idx >= _grd.Length || idx < 0)
                return -1;
            return _grd[idx];
        }

        /**
         *初始化地图的碰撞信息 
         */
        private void _initCollide()
        {
            //根据Data初始化Collide
            if (_curMapConf == null)
            {
                return;
            }
            //根据地表信息初始化碰撞信息
            int width = (int)(_curMapConf["param"]["width"]._int / GameConstant.GEZI);
            int height = (int)(_curMapConf["param"]["height"]._int / GameConstant.GEZI);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //获取地表碰撞
                    //_CollideInfo.push(0);int idx = (_buffer_gy+j)*_map_data.param.width + (_buffer_gx+i);
                    int idx = j * width + i;
                    int tid = _get_tile_id(idx);
                    if (tid < 0)
                        continue;
                    //					uint flag = _mapMgr._tile_sets[_map_data.param.tile_set].tiles[tid].flag;
                    //碰撞信息已经转存，tid = (tid&0x0fff)|gflag<<12,gflag = collide(0x01) | cover(0x02)
                    uint gflag = (uint)(tid >> 12);                    

                    _CollideInfoEx[((j) << 9 | (i) & 0x000001ff)] = gflag & 0x01;
                    //_CoverInfoEx[((j) << 9 | (i) & 0x000001ff)] = gflag & 0x02;
                }
            }
        }

        public bool IsWalkAble(float gx, float gy)
        {
            if (_curMapConf == null)
                return false;

            int width = (int)(_curMapConf["param"]["width"]._int / GameConstant.GEZI);
            int height = (int)(_curMapConf["param"]["height"]._int / GameConstant.GEZI);
         
            //是否合法地图TILE坐标(是否小于或大于最大值)
            if (gx < 0 || gx >= width || gy < 0 || gy >= height)
                return false;
            int idx = (((int)(gy)) << 9 | ((int)(gx)) & 0x000001ff);
            //int idx = (int)( gx + gy * width );
            //是否有碰撞信息
			int flag = 0;
			if( _CollideInfoEx.ContainsKey(idx) )
			{
				flag = (int)(_CollideInfoEx[idx]);
			}
            if (flag == 1)
                return false;

            if (this._BlockZoneInfo.ContainsKey(idx) && this._BlockZoneInfo[idx] > 0)
            {
                return false;
            }

            return true;
        }

        //public bool IsCovered(int x, int y)
        //{
        //    if (_curMapConf == null)
        //        return false;

        //    int width = _curMapConf["param"]["width"]._int32;
        //    int height = _curMapConf["param"]["height"]._int32;

        //    if (x < 0 || x >= width || y < 0 || y >= height)
        //        return false;
        //    //是否被遮挡
        //    int flag = (int)(_CoverInfoEx[((y) << 9 | (x) & 0x000001ff)]);
        //    if (flag == 2)
        //        return true;
        //    return false;
        //}

        protected Vec2 _getWalkabelEnd(Vec2 start, Vec2 end)
        {
            int xc = (int)start.x - (int)end.x;
            int yc = (int)start.y - (int)end.y;

            int maxc = Math.Max(Math.Abs((int)xc), Math.Abs((int)yc));

            Vec2 p = null;

            for (uint i = 0; i < maxc; ++i)
            {

                int x = (int)(end.x + xc * (i / maxc));

                int y = (int)(end.y + yc * (i / maxc));

                Vec2 p_t = new Vec2(x, y);

                if (IsWalkAble((int)p_t.x, (int)p_t.y))
                {
                    p = p_t;
                    break;
                }
            }

            return p;
        }

        private int _OpenGrids_compfunc(GridST g1, GridST g2)
        {
            if (g1.F < g2.F)
            {
                return -1;
            }
            else if (g1.F == g2.F)
            {
                if (g1.H < g2.H)
                {
                    return -1;
                }
            }

            return 1;
        }
        public List<GridST> findPath(Vec2 start, Vec2 end)
        {
            if (!this.IsWalkAble((int)end.x, (int)end.y))
            {
                //如果目标点不能行走，以此取目标点和出发点连线上的点，改目标点为最接近目标点的可行走点，开始寻路

                end = this._getWalkabelEnd(start, end);

                if (end == null)
				{
					GameTools.PrintNotice( "end point not walkable!" );
					return null;
				}
            }

            if (start.x == end.x && start.y == end.y)
            {
                return new List<GridST>();
            }



            int map_width = (int)(_curMapConf["param"]["width"]._int / GameConstant.GEZI);
            int map_height = (int)(_curMapConf["param"]["height"]._int / GameConstant.GEZI);

            List<GridST> Open_Grids = new List<GridST>();
            Dictionary<uint, GridST> Open_Grids_map = new Dictionary<uint, GridST>();
            Dictionary<int, int> Grids_hex_mark = new Dictionary<int, int>();

            GridST st = new GridST();
            st.x = (int)start.x;
            st.y = (int)start.y;
            st.parent = null;
            st.G = 0;
            st.H = 0;
            st.F = 0;
            st.isCorner = false;
            st.tile_idx = 0;

            Open_Grids.Add(st);

            GridST end_tile = null;
            int i = 0;
            int j = 0;
            while (Open_Grids.Count > 0)
            {

                GridST cur_tile = ArrayUtil.pop_priority_queue(Open_Grids, _OpenGrids_compfunc);

                uint tile_idx = (uint)(((int)(cur_tile.y)) << 9 | ((int)(cur_tile.x)) & 0x000001ff);

                Grids_hex_mark[(int)tile_idx] = 1; // 标记为已走

                //debug.dbginfo("cur node x["+cur_tile.x+"] y["+cur_tile.y+"]");

                for (i = 0; i < 3; ++i)
                {
                    int edge_tile_y = (int)(cur_tile.y + i - 1);

                    if (edge_tile_y < 0 || edge_tile_y >= map_height) // 坐标不合法
                    {
                        continue;
                    }

                    uint edge_tile_idx_y = (uint)(edge_tile_y << 9);
                    for (j = 0; j < 3; ++j)
                    {
                        int edge_tile_x = (int)(cur_tile.x + j - 1);

                        if (edge_tile_x < 0 || edge_tile_x >= map_width) // 坐标不合法
                        {
                            continue;
                        }

                        uint edge_tile_idx = (uint)( edge_tile_idx_y | (uint)( edge_tile_x & 0x000001ff ) );


                        if (Grids_hex_mark.ContainsKey((int)edge_tile_idx)) // 不能走，或已判断过
                        {

                            continue;
                        }
						int index = (int)edge_tile_idx;
						//if( index > _CollideInfoEx.Count -1 )
						//{
						//	//GameTools.PrintError( "CollideInfoEx info err!"  );
						//	continue;
						//}

						if ((_CollideInfoEx[index] == 1) ||
								(this._BlockZoneInfo.ContainsKey((int)edge_tile_idx) &&
								  this._BlockZoneInfo[(int)edge_tile_idx] > 0))
						// 格子不可行走
						{

							Grids_hex_mark[(int)edge_tile_idx] = 1; // 标记为 已判断过，不能走
							continue;
						}

                        // 计算 F, G, H
                        int G = cur_tile.G + 1; // 上下左右 +1
                        bool isCorner = false;
                        if (cur_tile.x != edge_tile_x && cur_tile.y != edge_tile_y)
                        {
                            G = cur_tile.G + 2; // 边角 +2
                            isCorner = true;
                        }


                        int H = (int)(Math.Abs(edge_tile_y - end.y) + Math.Abs(edge_tile_x - end.x));
                        int F = G + H;

                        if (Open_Grids_map.ContainsKey(edge_tile_idx))
                        {
                            // 已在开放格子列表中
                            //dbg_str += "founded!";
                            GridST edge_tile = Open_Grids_map[edge_tile_idx];
                            if (F < edge_tile.F || (F == edge_tile.F && H < edge_tile.H))
                            {
                                // 新路径更佳
                                edge_tile.F = F;
                                edge_tile.H = H;
                                edge_tile.G = G;
                                edge_tile.parent = cur_tile;
                                edge_tile.isCorner = isCorner;
                                edge_tile.tile_idx = edge_tile_idx;
                            }
                        }
                        else
                        {
                            // 新格子

                            GridST stt = new GridST();
                            stt.x = edge_tile_x;
                            stt.y = edge_tile_y;
                            stt.parent = cur_tile;
                            stt.G = G;
                            stt.H = H;
                            stt.F = F;
                            stt.isCorner = isCorner;
                            stt.tile_idx = edge_tile_idx;
                            Open_Grids_map[edge_tile_idx] = stt;


                            ArrayUtil.push_priority_queue(Open_Grids, Open_Grids_map[edge_tile_idx], _OpenGrids_compfunc);


                            if (edge_tile_x == end.x && edge_tile_y == end.y)
                            {
                                end_tile = Open_Grids_map[edge_tile_idx];
                                break;
                            }
                        }


                    }

                    if (end_tile != null)
                    {
                        // 找到了
                        break;
                    }
                }

                if (end_tile != null)
                {
                    // 找到了
                    break;
                }
            }



            if (end_tile == null)
            {
                return null;
            }

            List<GridST> path_ary = new List<GridST>();
            for (; end_tile != null; )
            {

                GridST ast = new GridST();
                ast.x = end_tile.x;
                ast.y = end_tile.y;
                ast.parent = end_tile.parent;
                ast.G = end_tile.G;
                ast.H = end_tile.H;
                ast.F = end_tile.F;
                ast.isCorner = end_tile.isCorner;
                ast.tile_idx = end_tile.tile_idx;

                path_ary.Add(ast);

                end_tile = end_tile.parent;
            }

            return path_ary;
        }

        private int getDirection(int fx, int fy, int tx, int ty)
        {// [1,2，4，8，16,32,64,128,右下左上,右下，左下，左上，右上,]

            int xsub = tx - fx;
            int ysub = ty - fy;
            int d = 0;
            if (ysub == 0)
            {
                d = (xsub > 0) ? 1 : 4;
            }
            else if (xsub == 0)
            {
                d = (ysub > 0) ? 2 : 8;
            }
            else if (xsub > 0 && ysub > 0)
            {
                d = 16;
            }
            else if (xsub > 0 && ysub < 0)
            {
                d = 128;
            }
            else if (xsub < 0 && ysub > 0)
            {
                d = 32;
            }
            else if (xsub < 0 && ysub < 0)
            {
                d = 64;
            }
            return d;
        }

        //public Array GetSmoothPath(Array path_ary)
        //{
        //	if( !path_ary || path_ary.length<= 2 ) 
        //	{
        //		return path_ary;
        //	}

        //	int pathLen = path_ary.length - 2;


        //	pathLen = path_ary.length;
        //	Variant pt;
        //	int cnt = 0;
        //	for (int j = 0; j <= pathLen - 2;  )
        //	{
        //		pt = path_ary[ j ];
        //		for (int i = pathLen - 1; i >= 0; i--)
        //		{
        //			if (floydCrossAble(path_ary[i], pt))
        //			{
        //				for (int k = i-1; k > j ; k--)
        //				{
        //					path_ary.splice(k, 1);
        //				}
        //				pathLen = path_ary.length;

        //				break;
        //			}
        //		}			
        //		j++;
        //	}
        //	return path_ary;

        //}
        //private bool floydCrossAble(Variant n1, Variant n2) 
        //{
        //	Array ps = bresenhamNodes(new Point(n1.x, n1.y), new Point(n2.x, n2.y));
        //	for (int i = ps.length - 2; i > 0; i--)
        //	{
        //		if (!IsWalkAble(ps[i].x,ps[i].y)) 
        //		{
        //			return false;
        //		}
        //	}
        //	return true;
        //}

        //private function bresenhamNodes(Point p1, Point p2):Array 
        //{
        //	bool steep = Math.abs(p2.y - p1.y) > Math.abs(p2.x - p1.x);
        //	if (steep) 
        //	{
        //		int temp = p1.x;
        //		p1.x = p1.y;
        //		p1.y = temp;
        //		temp = p2.x;
        //		p2.x = p2.y;
        //		p2.y = temp;
        //	}
        //	int stepX = p2.x > p1.x?1:(p2.x < p1.x? -0 1);
        //	float deltay = (p2.y - p1.y)/Math.abs(p2.x-p1.x);
        //	Array ret = [];
        //	float nowX = p1.x + stepX;
        //	float nowY = p1.y + deltay;
        //	if (steep) 
        //	{
        //		ret.push(new Point(p1.y,p1.x));
        //	}
        //	else 
        //	{
        //		ret.push(new Point(p1.x,p1.y));
        //	}
        //	if (Math.abs(p1.x - p2.x) == Math.abs(p1.y - p2.y))
        //	{
        //		if(p1.x<p2.x&&p1.y<p2.y)
        //		{
        //			ret.push(new Point(p1.x, p1.y + 1), new Point(p2.x, p2.y - 1));
        //		}
        //		else if(p1.x>p2.x&&p1.y>p2.y)
        //		{
        //			ret.push(new Point(p1.x, p1.y - 1), new Point(p2.x, p2.y + 1));
        //		}
        //		else if(p1.x<p2.x&&p1.y>p2.y)
        //		{
        //			ret.push(new Point(p1.x, p1.y - 1), new Point(p2.x, p2.y + 1));
        //		}
        //		else if(p1.x>p2.x&&p1.y<p2.y)
        //		{
        //			ret.push(new Point(p1.x, p1.y + 1), new Point(p2.x, p2.y - 1));
        //		}
        //	}
        //	while (nowX != p2.x) 
        //	{
        //		int fy = Math.floor(nowY);
        //		int cy = Math.ceil(nowY);
        //		if (steep) 
        //		{
        //			ret.push(new Point(fy, nowX));
        //		}
        //		else
        //		{
        //			ret.push(new Point(nowX, fy));
        //		}
        //		if (fy != cy) 
        //		{
        //			if (steep) 
        //			{
        //				ret.push(new Point(cy,nowX));
        //			}
        //			else
        //			{
        //				ret.push(new Point(nowX, cy));
        //			}
        //		}
        //		else if(deltay!=0)
        //		{
        //			if (steep)
        //			{
        //				ret.push(new Point(cy+1,nowX));
        //				ret.push(new Point(cy-1,nowX));
        //			}
        //			else
        //			{
        //				ret.push(new Point(nowX, cy+1));
        //				ret.push(new Point(nowX, cy-1));
        //			}
        //		}
        //		nowX += stepX;
        //		nowY += deltay;
        //	}
        //	if (steep) 
        //	{
        //		ret.push(new Point(p2.y,p2.x));
        //	}
        //	else 
        //	{
        //		ret.push(new Point(p2.x,p2.y));
        //	}
        //	return ret;
        //}
        //获取指定点附近 随机一个可行走的位置
        //grid 指定搜索的Grid位置
        //minRad 开始弧度   rangeRad搜索弧度范围  2.094 为120度
        //minR 最小半径  range搜索区域
        //maxCount 最大查找次数
        public Vec2 FindCanMoveGridPosNearBy(Vec2 grid, float minRad,
                                                  float rangeRad, uint minR = 2, uint range = 3, int maxCount = 5)
        {
            Vec2 ret = new Vec2(grid.x, grid.y);
            if (_curMapConf != null)
            {
                int width = _curMapConf["param"]["width"]._int32;
                int height = _curMapConf["param"]["height"]._int32;

                int count = maxCount;
                Random ran = new Random();
                ;
                while (count > 0)
                {
                    double radian = minRad + ran.NextDouble() * rangeRad;
                    double radius = minR + ran.NextDouble() * range;

                    double tmp = radius * Math.Cos(radian);
                    int nearx = (int)(grid.x + tmp);
                    if (tmp < 0) nearx++;//向圆心靠 避免舍弃小数 造成过大偏离圆心

                    tmp = radius * Math.Sin(radian);
                    int neary = (int)(grid.y - tmp);
                    if (tmp > 0) neary++;//向圆心靠 避免舍弃小数 造成过大偏离圆心

                    if (nearx >= 0 && neary >= 0 && nearx < width && neary < height)
                    {
                        if ((_CollideInfoEx[((neary) << 9 | (nearx) & 0x000001ff)] == 0) &&
                            !_BlockZoneInfo.ContainsKey((neary) << 9 | (nearx) & 0x000001ff))
                        {
                            ret.x = nearx;
                            ret.y = neary;
                            break;
                        }
                    }
                    count--;
                }
            }
            return ret;
        }


		//寻地图
		public List<uint> get_map_path(uint curMapId, uint dest_map_id) 
		{
			List<uint> path = new List<uint>();
			//世界寻路
			if( curMapId == dest_map_id )
			{//在当前地图
				path.Add( dest_map_id );
				return path;
			}
			
			//搜索路径
			DFSTraverse( curMapId, dest_map_id );
			
			//追溯路径
			
			uint map_id_temp = dest_map_id;
			
			for(;;)
			{
				path.Add( map_id_temp );
				Variant map_temp =  this.svrMapConfig.mapConfs[ map_id_temp ];
				 
				if( map_temp == null || !map_temp.ContainsKey("parent") || map_temp["parent"]._uint == 0)
					break;
				map_id_temp = map_temp["parent"]._uint;
			}
			
			return path;
		}
		private Dictionary<uint, int> visited = new Dictionary<uint, int>();
		private void DFSTraverse(uint start_map_id,uint dest_map_id)
		{
			//查询路径
			 
			foreach(uint key in  this.svrMapConfig.mapConfs.Keys )
			{
				visited[ key ] = 0;//初始化，全部标志成未访问
			}
			
			DFS( start_map_id, start_map_id, dest_map_id, 1 );//以map_id为源点开始搜索
		}
		private void DFS(uint origin_map, uint start_map_id, uint dest_map_id, int length, uint parent_id=0 )
		{
			//TODO 逻辑在这里处理
			visited[ start_map_id ] = length;//标志距离
			
			Variant v = this.svrMapConfig.mapConfs[ start_map_id ];
			
			v["parent"] = parent_id;
			
			//是否为终点
			if( start_map_id == dest_map_id )
				return;//是终点，结束搜索
			
			//依次搜索邻接点
			Variant links = v["l"];
			List<uint> edges = new List<uint>();
			foreach( Variant l in links._arr )
			{
				edges.Add( l["gto"]._uint );
			}

			//如果地图是当前所在地图，查看触发出来的link点
			if( start_map_id == origin_map )
			{
				if( _lgmap.tmpLinks != null &&  _lgmap.tmpLinks.Count > 0 )
				{
					foreach( Variant l in _lgmap.tmpLinks.Values )
					{
						edges.Add( l["data"]["goto"]._uint );
					}			 
				}
			}
			

			for(int i = 0 ; i < edges.Count ; i++ ){
				uint v_map_id = edges[i];
				if( !visited.ContainsKey( v_map_id) )
				{
					//DebugTrace.print( "err: map["+ start_map_id+"] to not exsit ["+v_map_id+"]!" );
					continue;
				}
				if(visited[v_map_id] == 0 || visited[v_map_id]>length+1){//邻接点未访问 || 目前距离较短
					DFS( origin_map, v_map_id, dest_map_id, length+1, start_map_id );//以改邻接点为出发点开始搜索
				}
			}
		}



		private SvrMapConfig svrMapConfig
		{
			get{
				return _lgmap.g_mgr.g_gameConfM.getObject( OBJECT_NAME.CONF_SERVER_MAP )  as SvrMapConfig;			
			}
		}

    }
	 

}

