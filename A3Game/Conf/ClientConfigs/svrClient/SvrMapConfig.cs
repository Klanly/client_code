using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrMapConfig : configParser
    {
        public Dictionary<uint, Variant> _mapConfs = new Dictionary<uint, Variant>();
        public static SvrMapConfig instance;
        public SvrMapConfig(ClientConfig m)
            : base(m)
        {
            instance = this;
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrMapConfig(m as ClientConfig);
        }
        override protected void onData()
        {
            //DebugTrace.print("local SvrMapConfig load!");
            Variant map = this.m_conf["m"];
            for (int i = 0; i < map.Count; i++)
            {
                Variant m = map[i];
                uint mapid = m["id"]._uint;
                _mapConfs[mapid] = m;
            }
        }

		public Dictionary<uint, Variant> mapConfs
		{
			get{
				return _mapConfs;
			}
		}
        protected void format_mission()
        {

        }
        public Variant getSingleMapConf(uint mapid)
        {
            if (!_mapConfs.ContainsKey(mapid))
            {
                UnityEngine.Debug.LogWarning("找不到地图配置getSingleMapConf::mapid:"+mapid);
                return null;
            }

            return _mapConfs[mapid];
        }

        //        package Net.ServerGameConfs
        //{
        //    import Net.ServerGameConfig;

        //    import crossAPI.net.NetConst;
        //    import crossAPI.net.URLReq;
        //    import crossAPI.os;

        //    import crossCore.DebugTrace;
        //    import crossCore.StringUtil;

        //    import flash.utils.ByteArray;
        //    import flash.utils.Dictionary;
        //    import flash.utils.Endian;

        //    public class SvrMapConfig extends SvrConfigBase
        //    {
        public Variant simpleMapConfig;
        public Variant mapConfig;
        protected Variant _npc_in_map_id;

        //        public function SvrMapConfig(s:ServerGameConfig)
        //        {
        //            super(s);
        //            simpleMapConfig = new Dictionary();
        //            mapConfig = new Dictionary();
        //            _npc_in_map_id = new Dictionary();
        //        }
        //public function prepareMapConfig(mapID:uint, onFin:Function, onProg:Function):void
        public void prepareMapConfig(uint mapID, Action onFin, Action<uint, int, int> onProg)
        {
            //if(mapID in mapConfig)
            if (mapConfig[mapID])
            {
                onFin();
                return;
            }
            //var url:String = _sc.serverURL+"?sid="+_sc.serverID+"&get_smap&id=" + mapID +"&ver=" + StringUtil.make_version(_sc.configVers.mpver);
            Variant url = m_conf["serverURL"] + "?sid=" + m_conf["serverID"] + "&get_smap&id=" + mapID + "&ver=" + StringUtil.make_version(m_conf["configVers"]["mpver"]);
            //var urlReq:URLReq = os.net.createURLReq(url);
            IURLReq urlReq = os.net.CreateURLReq(url);
            urlReq.dataFormat = NetConst.URL_DATA_FORMAT_BINARY;
            //()=>{}
            urlReq.load(
                //function (r:URLReq, data:ByteArray):void
                (IURLReq r, object ret) =>
                {
					byte[] data = ret as byte[];
                    //if(!on_map_data(data))
                    ByteArray confbyte = new ByteArray(data);
                    if (!on_map_data(confbyte))
                    {
                        //DebugTrace.add(DebugTrace.DTT_ERR, "parser map id["+mapID+"] config error");
                        DebugTrace.add(Define.DebugTrace.DTT_ERR, "parser map id[" + mapID + "] config error");
                    }
                    onFin();
                },
                //function (r:URLReq, bytesLoaded:uint, bytesTotal:uint):void
               (IURLReq r, float progress) =>
               {
                   onProg(mapID, (int)(progress * 100), 100);
               },
                (IURLReq r, String msg) =>
                {
                    //DebugTrace.add(DebugTrace.DTT_ERR, "load map id["+mapID+"] config error msg: " + msg);					
                    DebugTrace.add(Define.DebugTrace.DTT_ERR, "load map id[" + mapID + "] config error msg: " + msg);
                    onFin();
                }
            );
        }

        //public function getMapConfig(mapID:uint):Object
        public Variant getMapConfig(uint mapID)
        {
            return mapConfig[mapID];

        }

        /**
         * 成功获取指定版本的单个地图信息
         */
        //public function on_map_data(data:ByteArray):Boolean
        public Boolean on_map_data(ByteArray data)
        {
            data.uncompress();

            //var map_id:uint = data.readUnsignedShort();
            uint map_id = data.readUnsignedShort();

            //var mapInfo:Object = new Object();
            Variant mapInfo = new Variant();
            mapConfig[map_id] = mapInfo;


            //mapInfo.param = new Object();
            //mapInfo.param.id = map_id;
            mapInfo["param"] = new Variant();
            mapInfo["param"]["id"] = map_id;

            //mapInfo.param.tile_size = data.readUnsignedShort();
            mapInfo["param"]["tile_size"] = data.readUnsignedShort();
            //mapInfo.param.width = data.readUnsignedShort();
            mapInfo["param"]["width"] = data.readUnsignedShort();
            //mapInfo.param.height = data.readUnsignedShort();
            mapInfo["param"]["height"] = data.readUnsignedShort();
            //mapInfo.param.tile_set = data.readUnsignedShort();
            mapInfo["param"]["tile_set"] = data.readUnsignedShort();
            //mapInfo.param.pk = data.readUnsignedByte();
            mapInfo["param"]["pk"] = data.readUnsignedByte();
            //mapInfo.param.name = StringUtil.read_NTSTR(data);
            //map["name"] = StringUtil.read_NTSTR(data,"");或
            mapInfo["param"]["name"] = StringUtil.read_NTSTR(data, "utf-8");

            //var links_count:uint = data.readUnsignedShort();
            uint links_count = data.readUnsignedShort();
            //mapInfo.param.links_count = links_count;
            mapInfo["param"]["links_count"] = links_count;
            //mapInfo.Link = new Array();
            mapInfo["Link"] = new Variant();

            //var i:uint = 0;
            uint i = 0;
            for (; i < links_count; ++i)
            {
                //var link_t:Object = new Object();
                Variant link_t = new Variant();

                //link_t.goto = data.readUnsignedShort();
                link_t["goto"] = data.readUnsignedShort();
                //link_t.x = data.readUnsignedShort();
                link_t["x"] = data.readUnsignedShort();
                //link_t.y = data.readUnsignedShort();
                link_t["y"] = data.readUnsignedShort();
                //link_t.to_x = data.readUnsignedShort();
                link_t["to_x"] = data.readUnsignedShort();
                //link_t.to_y = data.readUnsignedShort();
                link_t["to_y"] = data.readUnsignedShort();
                //mapInfo.Link.push(link_t);
                mapInfo["Link"]._arr.Add(link_t);
                //mapInfo ._arr .Add (link_t);
            }

            //var npcs_count:uint = data.readUnsignedShort();
            uint npcs_count = data.readUnsignedShort();
            //mapInfo.Npc = new Array();
            mapInfo["Npc"] = new Variant();

            for (i = 0; i < npcs_count; ++i)
            {
                //var npc_t:Object = new Object();
                Variant npc_t = new Variant();

                //npc_t.nid = data.readUnsignedShort();
                //npc_t.x = data.readShort();
                //npc_t.y = data.readShort();
                //npc_t.nr = data.readUnsignedShort();
                npc_t["nid"] = data.readUnsignedShort();
                npc_t["x"] = data.readShort();
                npc_t["y"] = data.readShort();
                npc_t["nr"] = data.readUnsignedShort();

                //mapInfo.Npc.push(npc_t);
                mapInfo["Npc"]._arr.Add(npc_t);
            }

            //var map_item_count:uint = data.readUnsignedShort();
            uint map_item_count = data.readUnsignedShort();
            //mapInfo.MapItems = new Array();
            mapInfo["MapItems"] = new Variant();

            for (i = 0; i < map_item_count; ++i)
            {
                //var map_item_t:Object = new Object();
                Variant map_item_t = new Variant();

                //map_item_t.iid = data.readUnsignedShort();
                //map_item_t.x = data.readShort();
                //map_item_t.y = data.readShort();
                //map_item_t.r = data.readUnsignedByte();
                //map_item_t.order = data.readUnsignedByte();
                //map_item_t.zorder = data.readShort();
                //map_item_t.blendMode = data.readUnsignedByte();
                map_item_t["iid"] = data.readUnsignedShort();
                map_item_t["x"] = data.readShort();
                map_item_t["y"] = data.readShort();
                map_item_t["r"] = data.readUnsignedByte();
                map_item_t["order"] = data.readUnsignedByte();
                map_item_t["zorder"] = data.readShort();
                map_item_t["blendMode"] = data.readUnsignedByte();

                //mapInfo.MapItems.push(map_item_t);
                mapInfo["MapItems"]._arr.Add(map_item_t);
            }

            //var map_effect_count:uint = data.readUnsignedShort();
            uint map_effect_count = data.readUnsignedShort();
            //mapInfo.MapEffects = new Array();
            mapInfo["MapEffects"] = new Variant();

            for (i = 0; i < map_effect_count; ++i)
            {
                //var map_effect_t:Object = new Object();
                Variant map_effect_t = new Variant();

                //map_effect_t.x = data.readShort();
                //map_effect_t.y = data.readShort();
                //map_effect_t.r = data.readUnsignedByte();
                //map_effect_t.order = data.readUnsignedByte();
                //map_effect_t.zorder = data.readShort();
                //map_effect_t.blendMode = data.readUnsignedByte();
                //map_effect_t.obj = StringUtil.read_NTSTR(data);
                map_effect_t["x"] = data.readShort();
                map_effect_t["y"] = data.readShort();
                map_effect_t["r"] = data.readUnsignedByte();
                map_effect_t["order"] = data.readUnsignedByte();
                map_effect_t["zorder"] = data.readShort();
                map_effect_t["blendMode"] = data.readUnsignedByte();
                //map["name"] = StringUtil.read_NTSTR(data,"");或
                map_effect_t["obj"] = StringUtil.read_NTSTR(data, "utf-8");

                //mapInfo.MapEffects.push(map_effect_t);
                mapInfo["MapEffects"]._arr.Add(map_effect_t);
            }

            //var monster_count:uint = data.readUnsignedShort();
            uint monster_count = data.readUnsignedShort();
            //mapInfo.MapMons = new Array();
            mapInfo["MapMons"] = new Variant();

            for (i = 0; i < monster_count; ++i)
            {
                //var monster_t:Object = new Object();
                Variant monster_t = new Variant();

                //monster_t.mid = data.readUnsignedShort(); 
                //monster_t.x = data.readShort();
                //monster_t.y = data.readShort();
                monster_t["mid"] = data.readUnsignedShort();
                monster_t["x"] = data.readShort();
                monster_t["y"] = data.readShort();

                //mapInfo.MapMons.push(monster_t);
                mapInfo["MapMons"]._arr.Add(monster_t);
            }

            //var grid_data:ByteArray = new ByteArray();
            ByteArray grid_data = new ByteArray();
            //grid_data.endian= Endian.LITTLE_ENDIAN;
            //grid_data.m_endian =Define.Endian.LITTLE_ENDIAN;//!!!!!!

            //data.readBytes(grid_data);
            data.readBytes(grid_data, 0, data.length);


            //mapInfo.gridData = new Array();
            mapInfo["gridData"] = new Variant();
            for (i = 0; i < grid_data.length; i += 2)
            {
                //mapInfo.gridData.push(grid_data.readUnsignedShort());
                mapInfo["gridData"]._arr.Add(grid_data.readUnsignedShort());
            }

            //if(mapInfo.gridData.length < mapInfo.param.width*mapInfo.param.height)
            if (mapInfo["gridData"]["length"] < mapInfo["param"]["width"] * mapInfo["param"]["height"])
            {
                // err : grid data length not match
                //DebugTrace.add(DebugTrace.DTT_ERR, "map id["+map_id+"] width["+mapInfo.param.width+"] height["+mapInfo.param.height+"] grid data length["+mapInfo.gridData.length+"] err");
                DebugTrace.add(Define.DebugTrace.DTT_ERR, "map id[" + map_id + "] width[" + mapInfo["param"]["width"] + "] height[" + mapInfo["param"]["height"] + "] grid data length[" + mapInfo["gridData"]["length"] + "] err");
            }

            return true;
        }

        //public function on_maps_data(data:ByteArray):Boolean
        public Boolean on_maps_data(ByteArray data)
		{
			// 解析地图数据
			data.uncompress();
			
            //var map_count:uint = data.readUnsignedShort();
            uint map_count = data.readUnsignedShort();
			
            //var i:uint = 0;
            uint i = 0;
			for(; i<map_count; ++i)
			{
                //var map:Object = new Object();
                Variant map =new Variant();
				
                //var map_id:uint = data.readUnsignedShort();
                uint map_id = data.readUnsignedShort();
				simpleMapConfig[map_id] = map;
				
                //map.name = StringUtil.read_NTSTR(data);
                //map["name"] = StringUtil.read_NTSTR(data,"");或
                 map["name"] = StringUtil.read_NTSTR(data,"utf-8");
				
                //var links_count:uint = data.readUnsignedShort();
                uint links_count = data.readUnsignedShort();
				
                //var links:Array = new Array();
                Variant  links =new Variant ();
                //var j:uint = 0;
                uint j = 0;
				for(; j< links_count; ++j)
				{
                    //var to_map_id:uint = data.readUnsignedShort();
                    uint to_map_id = data.readUnsignedShort();
                    //links.push(to_map_id);
                    links._arr .Add (to_map_id);
				}
                //map.links = links;
                map["links"] = links;
				
                //var npc_count:uint = data.readUnsignedShort();
				uint npc_count = data.readUnsignedShort();

                //var npcs:Array = new Array();
                Variant npcs =new Variant();
                //var npcinfo:Object = new Object();
                Variant npcinfo = new Variant();
				for(j=0; j< npc_count; ++j)
				{
                    //var npc_id:uint = data.readUnsignedShort();
                    //var npc_x:int = data.readShort();
                    //var npc_y:int = data.readShort();
                    uint  npc_id = data.readUnsignedShort();
					int  npc_x = data.readShort();
			        int  npc_y = data.readShort();
					
                    //npcs.push(npc_id);
                    npcs ._arr .Add (npc_id);
                    //npcinfo[npc_id] = {id:npc_id, x:npc_x, y:npc_y};
                    Variant ff = new Variant();
                    ff["id"]=npc_id;
                    ff["x"]=npc_x;
                    ff["y"]=npc_y;
                    npcinfo[npc_id] = ff;
				}
                //map.npcs = npcs;
                //map.npcinfo = npcinfo;
                map["npcs"] = npcs;
                map["npcinfo"] = npcinfo;
			}
			
			return true;
		}

		 
 
 
        //public function get_npc_map_id(npc_id:uint):uint
        public uint get_npc_map_id(uint npc_id)
        {
            //获取NPC所在地图ID
            //if(npc_id in _npc_in_map_id)
            if (_npc_in_map_id == null)
            {
                _npc_in_map_id = new Variant();
				 //for(var i:String in this.simpleMapConfig)
				if (_mapConfs != null && _mapConfs.Count > 0)
				{
					foreach (Variant map in this._mapConfs.Values)
					{
						uint mapid = map["id"]._uint;
						if (!map.ContainsKey("n"))
							continue;
					 
						Variant npcinfos = map["n"];
						 
						foreach( Variant npc in npcinfos._arr )
						{
							uint npc_id_t = npc["nid"]._uint;
							_npc_in_map_id[ npc_id_t.ToString() ] = mapid;
						}
					}
				}
            }

            if ( _npc_in_map_id.ContainsKey( npc_id.ToString() ) )
            {
                return _npc_in_map_id[npc_id.ToString()];
            }

			GameTools.PrintError( "get_npc_map_id npc["+npc_id+"] not Exist!" );

            return 0;
        }


       
        //public function getMapConfigByMapid(mapid : int) : Object
        public Variant getMapConfigByMapid(int mapid)
        {
            return simpleMapConfig[mapid];
        }

		public Variant get_npc_info( uint npc_id)
		{
			//获取NPC详细信息
			foreach( Variant map in _mapConfs.Values )
			{				 
				if( !map.ContainsKey("n") )
					continue;
				Variant npcinfos = map["n"];
				foreach( Variant npc in npcinfos._arr )
				{
					if( npc["nid"]._uint == npc_id )
					{
						return npc;
					}
				}
			}
			return null;
		}
		
    }
}
