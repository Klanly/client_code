using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrLevelConfig : configParser
    {
        public static SvrLevelConfig instacne;
        public SvrLevelConfig(ClientConfig m)
            : base(m)
        {
            instacne = this;
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrLevelConfig(m as ClientConfig);
        }

        /**
         * @author cobra
         * Created on 2013-12-26下午3:47:34
         */

        //package Net.ServerGameConfs
        //{
        //    import Net.ServerGameConfig;

        //    import crossAPI.net.Package;

        //    import crossCore.DebugTrace;

        //    import flash.utils.ByteArray;

        //    public class SvrLevelConfig extends SvrConfigBase
        //    {
        //        public function SvrLevelConfig(s:ServerGameConfig)
        //        {
        //            super(s);
        //        }

        //        public function on_level_data(data:ByteArray):Boolean
        //        {
        //            try
        //            {
        //                this._svrConfig = Package.inst.unserialize_obj(data);
        //            }
        //            catch(par:*)
        //            {
        //                DebugTrace.add(DebugTrace.DTT_ERR, "SvrLevelConfig on_level_data err:" + par);			
        //            }

        //            return true;
        //        }
        override protected Variant _formatConfig(Variant conf)
        {
            if (conf.ContainsKey("lvl"))
            {
              
                conf["lvl"] = conf["lvl"].convertToDct("tpid");
                foreach (Variant lvl in conf["lvl"].Values)
                {
                    if (lvl.ContainsKey("diff_lvl"))
                    {// 将diff_lvl的本地数据结构转换成和服务器的数据结构一致
                        List<Variant> list = new List<Variant>();// 新的diff_lvl数据结构列表
                        for (int i = 0; i < lvl["diff_lvl"].Length; i++)
                        {
                            int lv = lvl["diff_lvl"][i]["lv"]._int;

                            while (lv >= list.Count)
                            {
                                list.Add(null);
                            }

                            list[lv] = lvl["diff_lvl"][i];// 将diff_lvl的lv数据置为数组下标索引值

                            //lvl["diff_lvl"] = lvl["diff_lvl"].convertToDct("lv");
                        }
                        lvl.RemoveKey("diff_lvl");// 删除掉原始数据
                        lvl["diff_lvl"] = new Variant();
                        lvl["diff_lvl"].setToArray();
                        for (int i = 0; i < list.Count; ++i)
                        {// 载入新的数据
                            lvl["diff_lvl"].pushBack(list[i]);
                        }
                    }
                    if (lvl.ContainsKey("pvp"))
                    {
                        if (lvl["pvp"][0].ContainsKey("side"))
                        for (int i = 0; i < lvl["pvp"][0]["side"].Length; i++)
                        {
                            lvl["pvp"][0]["side"] = lvl["pvp"][0]["side"].convertToDct("id");
                        }
                    }
                    if (lvl.ContainsKey("tmchk"))
                    {
                        for (int i = 0; i < lvl["tmchk"].Count; i++)
                        {
                            if (lvl["tmchk"][i].ContainsKey("dtb"))
                            {
                                Variant dtb = GameTools.split(lvl["tmchk"][i]["dtb"],":",GameConstantDef.SPLIT_TYPE_INT);
                                lvl["tmchk"][i]["dtb"] = GameTools.createGroup("h", dtb[0], "min", dtb[1], "s", dtb[2]);
                            }
                            if (lvl["tmchk"][i].ContainsKey("dte"))
                            {
                                Variant dte = GameTools.split(lvl["tmchk"][i]["dte"], ":", GameConstantDef.SPLIT_TYPE_INT);
                                lvl["tmchk"][i]["dte"] = GameTools.createGroup("h", dte[0], "min", dte[1], "s", dte[2]);
                            }
                            if (lvl["tmchk"][i].ContainsKey("tb"))
                            {
                                Variant tb = GameTools.split(lvl["tmchk"][i]["tb"], " ");
                                Variant tb0 = GameTools.split(tb[0], "-", GameConstantDef.SPLIT_TYPE_INT);
                                Variant tb1 = GameTools.split(tb[1], ":", GameConstantDef.SPLIT_TYPE_INT);
                                lvl["tmchk"][i]["tb"] = GameTools.createGroup("y", tb0[0], "mon", tb0[1], "d", tb0[2], "h", tb1[0], "min", tb1[1], "s", tb1[2]);
                            }
                            if (lvl["tmchk"][i].ContainsKey("te"))
                            {
                                Variant te = GameTools.split(lvl["tmchk"][i]["te"], " ");
                                Variant te0 = GameTools.split(te[0], "-", GameConstantDef.SPLIT_TYPE_INT);
                                Variant te1 = GameTools.split(te[1], ":", GameConstantDef.SPLIT_TYPE_INT);
                                lvl["tmchk"][i]["te"] = GameTools.createGroup("y", te0[0], "mon", te0[1], "d", te0[2], "h", te1[0], "min", te1[1], "s", te1[2]);
                            }
                        }
                    }
                }
            }
            return conf;
        }
        public Variant get_level_data(uint ltpid)
        {
            //return this._svrConfig.lvl[ltpid];
            return m_conf["lvl"][ltpid.ToString()];
        }

        public bool isLevel(int ltpid)
        {
            Variant v= m_conf["lvl"];
            return v.ContainsKey(ltpid.ToString());
        }

        public Variant get_clan_territory(uint id)
        {
            //return this._svrConfig.clan_territory[id];
            return this.m_conf["clan_territory"][id];
        }
        public Variant is_level_map(uint mid)
        {
            //if(this._svrConfig == null)
            if (m_conf == null)
                return false;


            //var lvls:Object = this._svrConfig.lvl;
            Variant lvls = m_conf["lvl"];

            //for(var i:String in lvls)
            foreach (Variant lvl  in lvls.Values)
            {
 
                Variant map = lvl["map"];
 
                uint mapid = map[0]["id"]._uint;

                if (mapid == mid)
                    return true;
            }

            return false;
        }

        //副本战场奖励
        //private var _levelAwdInfo:Object = {};
        private Variant _levelAwdInfo = new Variant();
        //public function getLevelAwd(ltpid:uint):Object
        public Variant getLevelAwd(uint ltpid)
        {
            //if( _levelAwdInfo.hasOwnProperty(ltpid) ) 
            if (_levelAwdInfo.ContainsKey("ltpid"))///???
            {
                return _levelAwdInfo[ltpid];
            }

            //for each( var conf:Object in this._svrConfig.lvl )
            foreach (Variant conf in m_conf["lvl"]._arr)
            {
                //if( conf.tpid == ltpid && conf.hasOwnProperty("pvp") && conf.pvp[0].hasOwnProperty("rnk_awd") )
                if (conf["tpid"] == ltpid && conf.ContainsKey("pvp") && conf["pvp"][0].ContainsKey("rnk_awd"))
                {
                    _levelAwdInfo[ltpid] = conf;
                    return conf;
                }
            }
            return null;
        }

        //判断是否 副本有 物品奖励
        //public function IsLevelHasItemPrize( tpid:uint ):Boolean
        public Boolean IsLevelHasItemPrize(uint tpid)
        {
            //var ret:Boolean = false;
            Boolean ret = false;
            //var conf:Object = this._svrConfig.lvl[tpid];
            Variant conf = m_conf["lvl"][tpid];
            if (conf != null)
            {
                //var diffInfo:Array = conf.diff_lvl as Array;
                Variant diffInfo = conf["diff_lvl"];

                //for each( var diff:Object in diffInfo )
                foreach (Variant diff in diffInfo._arr)
                {
                    //ret = diff.hasOwnProperty("awd_itm");
                    ret = diff.ContainsKey("awd_itm");
                    break;
                }
            }
            return ret;
        }
        //爬塔任务配置长度
        //public function get_levelmis_datalen():int
        public int get_levelmis_datalen()
        {
            //var len:int = 0;
            int len = 0;
            //if( "lvlmis" in this._svrConfig )
            if (m_conf.ContainsKey("lvlmis"))
            {
                //for each(var lvlmis:Object in _svrConfig.lvlmis)
                foreach (Variant lvlmis in m_conf["lvlmis"]._arr)
                {
                    ++len;
                }
            }
            return len;
        }

        //public function get_levelmis_data( ltpid:uint ):Object
        public Variant get_levelmis_data(uint ltpid)
        {
            //if( "lvlmis" in  this._svrConfig )
            if (m_conf.ContainsKey("lvlmis"))
            {
                //for each( var lvl:Object in _svrConfig.lvlmis )
                foreach (Variant lvl in m_conf["lvlmis"]._arr)
                {
                    //if( lvl.tpid == ltpid )
                    if (lvl["tpid"] == ltpid)
                    {
                        return lvl;
                    }
                }
            }
            return null;
        }
        //public function Getlvlmis():Array
        public Variant Getlvlmis()
        {
            //return _svrConfig.lvlmis;
            return m_conf["lvlmis"];
        }
        //public function GetlvlmisLine(misid:uint):uint
        public uint GetlvlmisLine(uint misid)
        {
            //if(misid in _svrConfig.lvlmis)
            if (m_conf["lvlmis"].ContainsKey(misid.ToString()))
            {
                //return _svrConfig.lvlmis[misid].line;
                return m_conf["lvlmis"][misid]["line"];
            }
            return 0;
        }
        //public function get_levelmis_byPrelvlmis( ltpid:uint ):Object
        public Variant get_levelmis_byPrelvlmis(uint ltpid)
        {
            //if( "lvlmis" in  this._svrConfig )
            if (m_conf.ContainsKey("lvlmis"))
            {
                //for each( var lvl:Object in _svrConfig.lvlmis )
                foreach (Variant lvl in m_conf["lvlmis"]._arr)
                {
                    //if( lvl.prelvlmis == ltpid )
                    if (lvl["prelvlmis"] == ltpid)
                    {
                        return lvl;
                    }
                }
            }
            return null;
        }
        //获取已通过章节的爬塔层数
        //public function get_passlvlmis_len(chapter:uint):uint
        public uint get_passlvlmis_len(uint chapter)
        {
            //var len:uint = 0;
            uint len = 0;
            //if( "lvlmis" in this._svrConfig )
            if (m_conf.ContainsKey("lvlmis"))
            {
                //for each( var lvl:Object in _svrConfig.lvlmis )
                foreach (Variant lvl in m_conf["lvlmis"]._arr)
                {
                    //if( lvl.chapter >= chapter )
                    if (lvl["chapter"] >= chapter)
                    {
                        continue;
                    }
                    ++len;
                }
            }
            return len;
        }
        //通过章节获取爬塔任务
        //public function get_levelmis_byChapter(chapter:uint):Array
        public Variant get_levelmis_byChapter(uint chapter)
        {
            //if( "lvlmis" in this._svrConfig )
            if (m_conf.ContainsKey("lvlmis"))
            {
                //var chapterArr:Array = new Array();
                Variant chapterArr = new Variant();

                //for each( var lvl:Object in _svrConfig.lvlmis )
                foreach (Variant lvl in m_conf["lvlmis"]._arr)
                {
                    //if( lvl.chapter == chapter )
                    if (lvl["chapter"] == chapter)
                    {
                        //chapterArr.push(lvl);
                        chapterArr._arr.Add(lvl);
                    }
                }
                //chapterArr.sortOn("tpid", Array.NUMERIC);
                chapterArr._arr.Sort((left, right) =>
                {
                    if (left["tpid"] > right["tpid"])
                        return 1;
                    else if (left["tpid"] == right["tpid"])
                        return 0;
                    else
                        return -1;
                });
                return chapterArr;
            }
            return null;
        }

        //获取爬塔的层级
        //public function GetLevelMisFloor(lvlmis:Object):uint
        public uint GetLevelMisFloor(Variant lvlmis)
        {
            //var floor:uint = 0;
            uint floor = 0;
            //var prelen:uint = get_passlvlmis_len(lvlmis.chapter);
            uint prelen = get_passlvlmis_len(lvlmis["chapter"]);
            //var chapterArr:Array = get_levelmis_byChapter(lvlmis.chapter);
            Variant chapterArr = get_levelmis_byChapter(lvlmis["chapter"]);
            //for(var i:int = 0; i < chapterArr.length; i++)
            for (int i = 0; i < chapterArr.Length; i++)
            {
                //var chapterData:Object = chapterArr[i];
                Variant chapterData = chapterArr[i];
                //if(chapterData.tpid == lvlmis.tpid)
                if (chapterData["tpid"] == lvlmis["tpid"])
                {
                    //floor = prelen + i + 1;
                    floor = (uint)(prelen + i + 1);


                }
            }
            return floor;
        }

        //public function get_carrchief(carr:uint):Object
        public Variant get_carrchief(uint carr)
        {
            //if(_svrConfig == null || !_svrConfig.hasOwnProperty("carr_chief"))
            if (m_conf == null || !m_conf.ContainsKey("carr_chief"))
                return null;
            //var carr_chief:Array = _svrConfig.carr_chief;
            Variant carr_chief = m_conf["carr_chief"];

            return carr_chief[carr];
        }

        //public function get_arena_level(arenaid:uint):Object
        public Variant get_arena_level(uint arenaid)
        {
            //if(_svrConfig == null || !_svrConfig.hasOwnProperty("arena"))
            if (m_conf == null || !m_conf.ContainsKey("arena"))
                return null;

            //var arena:Array = _svrConfig.arena;
            Variant arena = m_conf["arena"];

            return arena[arenaid];
        }
        //根据arenaexid获取跨服战多人配置
        //public function get_arenaex_level(arenaexid:uint):Object
        public Variant get_arenaex_level(uint arenaexid)
        {
            //if(_svrConfig == null || !_svrConfig.hasOwnProperty("arenaex"))
            if (m_conf == null || !m_conf.ContainsKey("arenaex"))
                return null;

            //var arenaex:Array = _svrConfig.arenaex;
            Variant arenaex = m_conf["arenaex"];
            //for each(var obj:Object in arenaex)
            foreach (Variant obj in arenaex._arr)
            {
                if (obj!=null)
                {
                    return obj;
                }
            }
            return null;
        }

        //private var _multiLevel:Array;
        private Variant _multiLevel;
        //副本组队的副本
        //public function GetMultiLevel():Array
        public Variant GetMultiLevel()
        {
            if (_multiLevel == null)
            {
                _multiLevel = new Variant();

                //var levels:Array = _svrConfig.lvl;
                Variant levels = m_conf["lvl"];

                //for each(var level:Object in levels)
                foreach (Variant level in levels.Values)
                {
                    //if("room" in level)	
                    if (level.ContainsKey("room"))
                    {
                        //_multiLevel.push(level);
                        _multiLevel._arr.Add(level);

                    }
                }
                // _multiLevel.sortOn("room", Array.NUMERIC);
                _multiLevel._arr.Sort((left, right) =>
                {
                    if (left["tpid"] > right["tpid"])
                        return 1;
                    else if (left["tpid"] == right["tpid"])
                        return 0;
                    else
                        return -1;
                });
            }
            return _multiLevel;
        }

        //public function GetLevelMap(ltpid:uint, diff:uint):Object
        public Variant GetLevelMap(uint ltpid, uint diff)
        {
            //var lvl:Object = this._svrConfig.lvl[ltpid];
            Variant lvl = m_conf["lvl"][ltpid];
            //if(lvl && lvl.diff_lvl)
            if (lvl !=null&& lvl["diff_lvl"]!=null)
            {
                //for each(var diffObj:Object in lvl.diff_lvl)
                foreach (Variant diffObj in lvl["diff_lvl"]._arr)
                {
                    //if(diffObj.lv == diff)
                    if (diffObj["lv"] == diff)
                    {
                        //return diffObj.map;
                        return diffObj["map"];
                    }
                }
            }
            return null;
        }
        //public function GetLevelMapById(ltpid:uint, diff:uint, mapid:uint):Object
        public Variant GetLevelMapById(uint ltpid, uint diff, uint mapid)
        {
            //var lvl:Object = this._svrConfig.lvl[ltpid];
            Variant lvl = m_conf["lvl"][ltpid];
            //if(lvl && lvl.diff_lvl)
            if (lvl!=null && lvl["diff_lvl"]!=null)
            {
                //for each(var diffObj:Object in lvl.diff_lvl)
                foreach (Variant diffObj in lvl["diff_lvl"]._arr)
                {
                    //if(diffObj.lv == diff)
                    if (diffObj["lv"] == diff)
                    {
                        //for each(var mapObj:Object in diffObj.map)
                        foreach (Variant mapObj in diffObj["map"]._arr)
                        {
                            //if(mapObj.id == mapid)
                            if (mapObj["id"] == mapid)
                            {
                                return mapObj;
                            }
                        }
                        break;
                    }
                }
            }
            return null;
        }

        //public function GetLevelNextMapById(ltpid:uint, diff:uint, mapid:uint):Object
        public Variant GetLevelNextMapById(uint ltpid, uint diff, uint mapid)
        {
            //var lvl:Object = this._svrConfig.lvl[ltpid];
            Variant lvl = m_conf["lvl"][ltpid];
            //if(lvl && lvl.diff_lvl)
            if (lvl!=null && lvl["diff_lvl"]!=null)
            {
                //for each(var diffObj:Object in lvl.diff_lvl)
                foreach (Variant diffObj in lvl["diff_lvl"]._arr)
                {
                    //if(diffObj.lv == diff)
                    if (diffObj["lv"] == diff)
                    {
                        //for each(var mapObj:Object in diffObj.map)
                        foreach (Variant mapObj in diffObj["map"]._arr)
                        {
                            //if(mapObj.dir_enter && mapObj.dir_enter[0] && mapObj.dir_enter[0].km)
                            if (mapObj["dir_enter"]!=null && mapObj["dir_enter"][0]!=null && mapObj["dir_enter"][0]["km"]!=null)
                            {
                                //for each(var kmObj:Object in mapObj.dir_enter[0].km)
                                foreach (Variant kmObj in mapObj["dir_enter"][0]["km"]._arr)
                                {
                                    //if(kmObj.mapid == mapid)
                                    if (kmObj["mapid"] == mapid)
                                    {
                                        return mapObj;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return null;
        }

        //public function GetDirEnterByMapid(ltpid:uint, diff:uint, mapid:uint):Object
        public Variant GetDirEnterByMapid(uint ltpid, uint diff, uint mapid)
        {
            //var nextMapObj:Object = GetLevelNextMapById(ltpid, diff, mapid);
            Variant nextMapObj = GetLevelNextMapById(ltpid, diff, mapid);
            //if(nextMapObj && nextMapObj.dir_enter)
            if (nextMapObj!=null && nextMapObj["dir_enter"]!=null)
            {
                //return nextMapObj.dir_enter[0];
                return nextMapObj["dir_enter"][0];
            }
            return null;
        }

        //public function GetLevelDiffCostCh(ltpid:uint, diff:uint=1):Object
        public Variant GetLevelDiffCostCh(uint ltpid, uint diff = 1)
        {
            //var lvl:Object = this._svrConfig.lvl[ltpid];
            Variant lvl = m_conf["lvl"][ltpid.ToString()];
            //if(lvl && lvl.diff_lvl)
            if (lvl!=null && lvl["diff_lvl"]!=null)
            {
                //var costTp:Object = {};
                Variant costTp = new Variant();
                //for each(var diffObj:Object in lvl.diff_lvl)
                foreach (Variant diffObj in lvl["diff_lvl"]._arr)
                {
                    if (null == diffObj)
                        continue;

                    //if(diffObj.lv == diff && diffObj.cost_ch)
                    if (diffObj["lv"] == diff && diffObj.ContainsKey("cost_ch")?diffObj["cost_ch"]!=null:false)
                    {
                        //for each(var costCh:Object in diffObj.cost_ch)
                        foreach (Variant costCh in diffObj["cost_ch"]._arr)
                        {
                            //costTp[costCh.tp] = costCh.cost;
                            costTp[costCh["tp"]] = costCh["cost"];
                        }
                        break;
                    }
                }

                return costTp;

            }
            return null;
        }

        //public function GetLevelShareTpConf(ltpid:uint, diff:uint=1):Object
        public Variant GetLevelShareTpConf(uint ltpid, uint diff = 1)
        {
            //var costTpObj:Object = GetLevelDiffCostCh(ltpid, diff);
            Variant costTpObj = GetLevelDiffCostCh(ltpid, diff);
            //for(var tp:String in costTpObj)
            if(costTpObj.Count != 0)
            foreach (string tp in costTpObj.Keys)
            {
                //for each(var cost:Object in costTpObj[tp])
                foreach (Variant cost in costTpObj[tp]._arr)
                {
                    //if(cost.share)
                    if (cost["share"]!= null)
                    {
                        //return cost.share[0];
                        return cost["share"][0];
                    }
                }
            }
            return null;
        }

    }
}
