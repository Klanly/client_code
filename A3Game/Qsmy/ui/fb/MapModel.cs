using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using MuGame;
namespace MuGame
{
    public class MapModel
    {
        public Dictionary<int, MapData> dFbDta = new Dictionary<int, MapData>();
        public Dictionary<int, Dictionary<int, List<MapData>>> lData = new Dictionary<int, Dictionary<int, List<MapData>>>();
        public Dictionary<string, MapFogXml> dFogXmls = new Dictionary<string, MapFogXml>();
        public int energy;
        public float beginTimer;

        public bool inited = false;

        public uint curLevelId;
        public uint curDiff; //地图难度

        public Dictionary<string, int> lItemUsed;
		SXML instancelvlxml;
        public Dictionary<int, int> dicMappoint;
        //public int curCountGoldenBuy = 100;
        //public int curCountUseItem10062 = 100;
        //public int curCountUseItem10061 = 100;
        //public int curCountUseItem10060 = 100;
        public MapModel()
        {
			instancelvlxml = XMLMgr.instance.GetSXML("instancelvl");
			show_instanceLvl(103);//预热
                                  //MapFogXml tempfogxml;
                                  //SXML xml = XMLMgr.instance.GetSXML("worldcfg.camsetting", null);
                                  //do
                                  //{
                                  //    tempfogxml = new MapFogXml();
                                  //    tempfogxml.displayfog = xml.getInt("displayfog") == 1;
                                  //    if (tempfogxml.displayfog)
                                  //    {
                                  //        tempfogxml.fogden = xml.getFloat("fogden");
                                  //        tempfogxml.fogcolor = new Vec4(xml.getFloat("fogcolor_r"), xml.getFloat("fogcolor_g"), xml.getFloat("fogcolor_b"), xml.getFloat("fogcolor_a"));
                                  //        tempfogxml.strdistance = xml.getFloat("strdistance");
                                  //        tempfogxml.enddistance = xml.getFloat("enddistance");
                                  //        tempfogxml.fogmode = xml.getString("fogmode");
                                  //    }
                                  //    dFogXmls[xml.getString("map_type") + "_" + xml.getString("stage_group")] = tempfogxml;
                                  //} while (xml.nextOne());

            //lItemUsed = new Dictionary<string, int>();
            //lItemUsed["-1"] = 100;
            //lItemUsed["10060"] = 100;
            //lItemUsed["10061"] = 100;
            //lItemUsed["10062"] = 100;

            //SXML xml = XMLMgr.instance.GetSXML("worldcfg.world", null);
            //MapData tb;
            //MapItemData trb;
            //do
            //{
            //    tb = new MapData();
            //    tb.id = xml.getInt("level_id");
            //    tb.name = xml.getString("map_name");
            //    tb.mapid = xml.getInt("map_id");
            //    tb.stage_group = xml.getInt("stage_group");
            //    tb.plot = xml.getString("plot");
            //    tb.type = xml.getInt("map_type");
            //    tb.lv_limit = xml.getInt("open_lv");
            //    tb.starNum = 0;
            //    tb.fighting = xml.getInt("fighting");
            //    tb.bossId = xml.getInt("bossid");
            //    tb.count = 3;
            //    tb.resetCount = 0;
            //    SXML itemxml = xml.GetNode("reward", null);
            //    if (itemxml != null)
            //    {
            //        do
            //        {
            //            if (itemxml.getInt("reward_type") == 1)
            //                tb.exp = itemxml.getInt("value");
            //            else if (itemxml.getInt("reward_type") == 2)
            //                tb.money = itemxml.getInt("value");
            //        } while (itemxml.nextOne());
            //    }
            //    itemxml = xml.GetNode("item", null);
            //    if (itemxml != null)
            //    {
            //        tb.lItem = new List<MapItemData>();
            //        do
            //        {
            //            trb = new MapItemData();
            //            trb.id = itemxml.getInt("id");
            //            trb.count = itemxml.getInt("count");
            //            trb.firstRechange = itemxml.getInt("first_victory") == 1;
            //            tb.lItem.Add(trb);
            //        } while (itemxml.nextOne());
            //    }
            //    dFbDta[tb.id] = tb;
            //    if (tb.type > 0 && tb.stage_group > 0)
            //    {
            //        if (!lData.ContainsKey(tb.type))
            //            lData[tb.type] = new Dictionary<int, List<MapData>>();

            //        if (!lData[tb.type].ContainsKey(tb.stage_group))
            //            lData[tb.type][tb.stage_group] = new List<MapData>();


            //        lData[tb.type][tb.stage_group].Add(tb);
            //    }
            //} while (xml.nextOne());
            dicMappoint = new Dictionary<int, int>();
            List<SXML> listMappoint = XMLMgr.instance.GetSXML("mappoint").GetNodeList("trans_remind");
            for (int i = 0; i < listMappoint.Count; i++)
            {
                int mapId, transId;
                if ((mapId = listMappoint[i].getInt("map_id")) != -1 && (transId = listMappoint[i].getInt("trance_id")) != -1)
                    dicMappoint.Add(mapId, 1 + transId * 100);
            }
        }

        public MapData get3Starmap()
        {
     
            foreach (MapData data in dFbDta.Values)
            {
                if (data.starNum == 3)
                    return data;
            }
            return null;
        }

        public Variant getCurLevelVar()
        {
            if (curLevelId == 0)
                return null;
            return SvrLevelConfig.instacne.get_level_data(curLevelId);
        }

        //public MapFogXml getFogXml(int mapType, int stagetype)
        //{
        //    if (dFogXmls.ContainsKey(mapType + "_" + stagetype))
        //        return dFogXmls[mapType + "_" + stagetype];
        //    return null;
        //}
        public bool CheckAutoPlay()
        {
            if (curLevelId == 0)
                return false;
            return SvrLevelConfig.instacne.get_level_data(curLevelId)?.ContainsKey("auto_play") ?? false;
        }
        public bool containerID(int mapid)
        {
            return dFbDta.ContainsKey(mapid);
        }

        public float getMaxCD()
        {
            int maxCD = 480 - VipMgr.getValue(VipMgr.TILI_RESTORE_TIME);
            return maxCD;
        }

        public MapData getMapDta(int id)
        {
            if (dFbDta.ContainsKey(id))
                return dFbDta[id];
            return null;
        }

		public void AddMapDta(int id, MapData data) {
			dFbDta[id] = data;
		}

        public int maxId0 = 0;
        public int maxId1 = 0;
        public int maxStageId0 = 1;
        public int maxStageId1 = 1;
        public void setLastMapId(int mapType, int mapid)
        {
          
            if (mapType == 0)
            {
                if (mapid <= maxId0) return;
                maxId0 = mapid;
            }
            else if (mapType == 1)
            {
                if (mapid <= maxId1) return;
                maxId1 = mapid;
            }

          

            int curStageId = dFbDta[mapid].stage_group;
            Dictionary<int, List<MapData>> ds = lData[mapType+1];
            int len = ds.Count;
            foreach (List<MapData> l in ds.Values)
            {
                if (l.Count > 0)
                {
                    MapData d = l[l.Count - 1];
                    if (d.id == mapid && d.stage_group < len)
                    {
                        curStageId = d.stage_group + 1;
                    }
                }
            }

            if (mapType == 0)
                maxStageId0 = curStageId;
            else if (mapType == 1)
                maxStageId1 = curStageId;
        }

        public List<MapData> getFbListByGroup(int stagegroupId, int uiGroupId)
        {
            return lData[stagegroupId][uiGroupId];
        }

        static private MapModel _instance;
        static public MapModel getInstance()
        {
            if (_instance == null)
                _instance = new MapModel();
            return _instance;
        }

		//根据副本id和主角转生等级，得到怪物外观等级
		public int show_instanceLvl(uint levelid) {
			int show_lv = 0;
			Variant sdc = SvrLevelConfig.instacne.get_level_data(levelid);
			if (sdc != null) {
			int i = sdc["lmtp"];
			var z = instancelvlxml.GetNode("zhuan", "zhuan==" + PlayerModel.getInstance().up_lvl);
			var l = z.GetNodeList("att", "lv==" + PlayerModel.getInstance().lvl);
			foreach (var v in l) {
					if (v.getInt("lmtp") == i) {
						show_lv = v.getInt("show_lv");
						break;
					}
				}

			}
			return show_lv;
		}
    }

    public class MapData
    {
        public int id;
        public int mapid;
        public String name;
        public int groupId;
        public int type;
        public int relive_type;
        public String plot;
        public int pvp_type;
        public int lv_limit;
        public int fighting;//建议战斗力
        public int energy;//消耗体力值
        public int lv_uplimit;//等级上限
        public int stage_group;//剧情分组
        public int ui_group;//ui分组
        public int return_map_id;//
        public int return_map_x;//
        public int return_map_y;//
        public int time;//通关时间
        public List<MapItemData> lItem;
		public long enterTime;//上次进入时间戳
		public int cycleCount;//本周期总共进入次数
		int killNum;//杀敌数
		public int limit_tm;//剩余时间
        public int vip_buycount = 0;//vip已购买次数
		public int kmNum {
			get {
				return killNum;
			}
			set {
				killNum = value;
				if (OnKillNumChange != null) OnKillNumChange(killNum);
			}
		}
		public int total_enemyNum;//敌人总数

        public int bossId;

        public int money;
        public int exp;

        public int starNum;
        public int count;
        public int resetCount;

		public Action<int> OnKillNumChange;

        public int getCanResetCount()
        {
            int count = VipMgr.getValue(VipMgr.PLOT_STAGE_RESET);
            if (resetCount > count)
                return 0;

            return count - resetCount;
        }
    }

    public class MapFogXml
    {
        public bool displayfog;
        public float fogden;
        public Vec4 fogcolor;
        public string fogmode;
        public float strdistance;
        public float enddistance;
    }

    public class MapItemData
    {
        public int id;
        public int count;
        public bool firstRechange;//首次必掉
    }
    public class Rewards
    {
        public int tpid;
        public int cnt;
    }
}
