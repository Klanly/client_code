using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrGeneralConfig : configParser
    {

        public SvrGeneralConfig(ClientConfig m)
            : base(m)
        {

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrGeneralConfig(m as ClientConfig);
        }

        protected override Variant _formatConfig(Variant conf)
        {
            if (conf.ContainsKey("vip"))
            {
                foreach (Variant vip in conf["vip"]._arr)
                {
                    if (vip.ContainsKey("item_grp"))
                    {
                        foreach (Variant item_grp in vip["item_grp"]._arr)
                        {
                            if (item_grp.ContainsKey("ids"))
                            {
                                item_grp["ids"] = GameTools.split(item_grp["ids"]._str, ",", GameConstantDef.SPLIT_TYPE_INT);
                            }
                        }
                    }
                }
            }
            if(conf.ContainsKey("awdact"))
            {
                foreach (Variant obj in conf["awdact"]._arr)
                {
                    foreach (Variant awd in obj["target"]._arr)
                    {
                        awd["awd"] = awd["awd"].convertToDct("awdid");
                    }
                }
            }
            if (conf.ContainsKey("carr"))
            {
                conf["carr"] = GameTools.array2Map(conf["carr"], "id");
            }

            return base._formatConfig(conf);
        }


        public Variant getmap_need_lvl
        {
            get
            {
                Variant map_need = new Variant();
                for (int i = 0; i < conf["map_need_lvl"].Count; i++)
                {
                    map_need[conf["map_need_lvl"][i]["mapid"]._int] = conf["map_need_lvl"][i]["lvl"]._int;
                }
                return map_need;
            }
        }
        /**
     * 获取vip奖励信息
     * */
        public Variant get_vip_fb_awd(int viplv)
        {
            if (conf.ContainsKey("vip_fb_awd"))
            return conf["vip_fb_awd"]["viplv"];
            return null;
        }
        /**
		 * 获取指定等级的VIP配置信息
		 * @param VIP等级
		 * @return VIP配置信息
		 */
        public Variant get_vip_info(int level)
        {
            Variant vip = conf["vip"];
            if (level < vip.Count)
            {
                return vip[level];
            }
            else
                return null;
        }
        public Variant get_pvip_dayawd_byPlvl(uint Plvl)
        {
            Variant pvip_dayawd = conf["pvip_dayawd"];
            if (pvip_dayawd != null)
            {
                foreach (Variant v_conf in pvip_dayawd._arr)
                {
                    if (v_conf["pvip"]._uint == Plvl)
                    {
                        return v_conf;
                    }
                }
            }
            return null;
        }
        public Variant get_pvip_dayawd()
        {
            return conf["pvip_dayawd"];
        }
        //获取黄钻成长礼包配置
        public Variant get_pvip_growawd()
        {
            return conf["pvip_lmawd"];
        }
        //获取黄钻能量豪礼信息
        public Variant get_pvip_power()
        {
            if (conf != null)
            {
                if (conf.ContainsKey("pvip_power"))
                {
                    return conf["pvip_power"];
                }
            }
            return null;
        }
        public Variant get_pvip()
        {
            return conf["pvip"];
        }
        //获取黄钻能量豪礼信息
        public Variant GetPowerConf(uint id)
        {
            if (conf != null)
            {
                Variant pvip_power = conf["pvip_power"];
                if (pvip_power != null)
                {
                    foreach (Variant obj in pvip_power._arr)
                    {
                        if (obj["id"]._uint == id)
                            return obj;
                    }
 
                }
            }
            return null;
        }
        /**
		 *获得抽奖配置 
		 */
        public Variant get_lottery(int lv)
        {
            Variant lottery = conf["lottery"];
            if (lottery == null)
                return null;
            for (int i = 0; i < lottery.Length; i++)
            {
                if (lottery[i]["lvl"]._int == lv)
                    return lottery[i];
            }
                return null;
        }
        //游戏全局配置
        public float get_game_general_data(string name)
        {
            if (conf["game"][0].ContainsKey(name)) 
            return conf["game"][0][name]._float;
            return float.NaN;
        }
        public Variant get_game_general_object(string name)
        {
            if (conf["game"].ContainsKey(name)) 
            return conf["game"][name];
            return null;
        }
        /**
		 * 获取市场的信息tp:市场类型
		 * */
        public Variant GetAucInfo(int tp)
        {
            if (conf.ContainsKey("auc"))
            {
                return conf["auc"][tp.ToString()];
            }
            return null;
        }
        /**
		 * 获取市场数量
		 * */
        public int GetAucNum()
        {
            int num = 0;
            if (conf.ContainsKey("auc"))
            {
                foreach (Variant temp in conf["auc"]._arr)
                {
                    if (temp["stall_cnt"])
                        ++num;
                }
            }
            
            return num;
        }
        public Variant  GetNobilityData()
		{
            return conf["nobility"]; 
		}	
        public Variant GetNobilityAwd()
        {
            if (conf == null || !conf.ContainsKey("nobawd"))
                return null;
            return conf["nobawd"];
        }
        public Variant GetNobByid(int lvl)
        {
            Variant data = conf["nobility"];
            if (data != null)
                return data[lvl-1];
            return null;
        }
        public string GetNobName(int lvl)
        {
            Variant data = conf["nobility"];
            if (data != null && data.ContainsKey(lvl.ToString()))
            {
                return data[lvl.ToString()]["name"];
            }
            return "";
        }
        public Variant GetAchiveData()
        {
            return conf["achive"];
        }
        /**
		 * 获取某个成就信息
		 * */
        public Variant GetAchieve(int id)
        {
            return conf["achive"][id.ToString()];
        }
        //获得战盟数据
        public uint get_clan_general_data(int lvl, string name)
        {
            if (!conf["clan"].ContainsKey(lvl.ToString()))
                return 0;
            Variant g = conf["clan"][lvl.ToString()];
            return g[name]._uint;
        }
        /**
		 * 获取战盟等级数据
		 */
        public Variant get_clanlvl_data(uint lvl)
        {
            if (conf.ContainsKey("clan"))
                return conf["clan"][lvl.ToString()];
            return null;
        }
        /**
		 * 职业相关信息
		 * */
        public Variant GetCarrlvl(int carr)
        {
            return conf["carr"][carr.ToString()];
        }
        public Variant GetResetlvl()
        {
            return conf["resetlvl"];
        }
        //获取重生洗练配置
        public Variant get_attpt_roll()
        {
            return conf["attpt_roll"];
        }
        public Variant GetVipData()
        {
            return conf["vip"];
        }
        public Variant getMapex(uint mpid)
        {
            if (!conf.ContainsKey("mapex"))
                return null;
            Variant mapex = conf["mapex"];
            if (!mapex.ContainsKey(mpid.ToString()))
                return null;
            Variant mapdate = mapex[mpid.ToString()];
            return mapdate;
        }
        public Variant ybract
        {
            get
            {
                return conf["ybract"];
            }

        }

        public Variant awdact
        {
            get
            {
                return conf["awdact"];
            }

        }
        public Variant rankact
        {
            get
            {
                return conf["rankact"];
            }

        }
        //add by jf
        public Variant ol_award
        {
            get
            {
                return conf["ol_award"];
            }

        }
        //end add by jf
        //获取签到奖励
        public Variant GetSigninawd()
        {
            return conf["signin_awd"];
        }
        //获取连续登陆奖励
        public Variant GetClogawd()
        {
            return conf["clogawd"];
        }
        public Variant GetConfByTypeId(string type, int id)
        {
            if (conf != null && conf.ContainsKey(type))
            {
                Variant arr = conf[type];
                foreach (Variant obj in arr._arr)
                {
                    if (obj["id"]._int == id)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //---------------------获取膜拜配置数据-----------------------------------------------------------------
        public Variant GetWorshipData()
        {
            return conf["worship"];
        }
        //---------------------获取膜拜配置数据end-----------------------------------------------------------------
        /**
         * 获取成就目标
         * */
        public Variant GetAchieveGoal(uint achieveID, int carr)
        {
            if (conf["achieve"] != null)
            {
                Variant achieve = conf["achieve"][achieveID.ToString()];
                if (achieve != null)
                {
                    if (carr > 0 && achieve["carr_goal"] != null)
                    {
                        Variant carr_goal = achieve["carr_goal"];
                        if (carr_goal.ContainsKey(carr.ToString()))
                            return carr_goal[carr.ToString()];
                    }
                    return achieve["goal"];
                }
            }
            return null;
        }
        /**
		 * 通过职业和智力获取技能伤害
		 * */
        public int GetSkillDmg(int carr, int inte)
        {
            int dmg = 100;
            int inte2atk = int.MaxValue;
            Variant carrData = GetCarrlvl(carr);
            if (carrData != null)
            {
                Variant v_base = carrData["baseatt"][0];
                if (v_base.ContainsKey("inte2atk"))
                {
                    inte2atk = v_base["inte2atk"]._int;
                }
            }
            if (carr == 1 || carr == 4)//战士或魔剑士
            {
                dmg = 200;
            }
            dmg = dmg + inte / (inte + inte2atk) * 100;
            return dmg;
        }
        public Variant GetDefSkills(int carr)
        {
            Variant carrData = GetCarrlvl(carr);
            if (carrData != null && carrData.ContainsKey("defskil"))
            {
                GameTools.Sort(carrData["defskil"],"id");
                return carrData["defskil"];
            }
            return null;
        }
        public Variant GetDefSkillData(int carr, uint sid)
        {
            Variant defskils = GetDefSkills(carr);
            if (defskils != null)
            {
                foreach (Variant def in defskils._arr)
                {
                    if (def["id"]._uint == sid)
                    {
                        if (!def.ContainsKey("sklvl"))
                        {
                            def["sklvl"] = def["lvl"];
                        }
                        return def;
                    }
                }
            }
            return null;
        }
        public bool IsDefSkill(int carr, uint sid)
        {//是否是默认职业技能
            Variant defData = GetDefSkillData(carr, sid);

            return defData != null;
        }

        //-----------------------节日活动----------------------------------
        public Variant GetFestivalData()
        {
            return conf["festact"];
        }
        public Variant GetFestivalDataById(int id)
        {
            if (conf.ContainsKey("festact"))
            {
                Variant data = conf["festact"];
                if (data.ContainsKey(id.ToString()))
                {
                    return data[id.ToString()];
                }
            }
            return null;
        }
        //-----------------------在线奖励-------------------------
        public Variant GetOnlinePrize(int aid)
        {
            if (conf["ol_award"] != null)
            {
                foreach (Variant obj in conf["ol_award"]._arr)
                {
                    if (obj["aid"]._int == aid)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //------------------------------------地图进入等级配置-------------------------------------------
        public Variant get_entermaplvl(uint mapid)
        {
            if (conf.ContainsKey("map_need_lvl"))
            {
                Variant data = conf["map_need_lvl"];
                foreach (Variant obj in data._arr)
                {
                    if (obj["mapid"]._uint == mapid)
                        return obj;
                }
            }
            Variant val = new Variant();
            val["lvl"] = 1;
            return val;
        }
        /**
		 *成长礼包的奖励 
		 * @return 
		 * 
		 */
        public Variant lvlprizes
        {
            get
            {
                return conf["lvlprize"];
            }
        }
        //------------------------登录奖励-------------------------------
        //连续登录物品奖励
        protected Variant _dayAwdArr;
        public Variant get_day_awd()
        {
            if (_dayAwdArr == null)
            {
                _dayAwdArr = new Variant();
                Variant arr = conf["day_awd"];
                foreach (Variant obj in arr._arr)
                {
                    _dayAwdArr._arr.Add(obj);
                }
            }
            return _dayAwdArr;
        }
        //-----obj sort -------------
        //public int SortObjFun(string key)
        //{


        //}
        public Variant getExchangeInfo()
        {
            return conf["ltryptexchg"];
        }
        //---------------------------一次性奖励      start----------------------------------------------------
        //once_awd
        public Variant GetMicroloadAwd()
        {
            if (conf.ContainsKey("once_awd"))
            {
                foreach (Variant obj in conf["once_awd"]._arr)
                {
                    if (obj["id"]._int == 1)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //---------------------------一次性奖励      end----------------------------------------------------

        //---------------------------线路限制地图配置     start----------------------------------------------------
        //need_line_map
        private Variant _need_line_map;
        public int GetMapNeedLine(uint mapid)
        {
            if (_need_line_map == null)
            {
                _need_line_map = new Variant();
                if (conf.ContainsKey("need_line_map"))
                {
                    foreach (Variant obj in conf["need_line_map"]._arr)
                    {
                        _need_line_map[obj["mpid"]] = obj["line"];
                    }
                }
            }
            if (_need_line_map.ContainsKey(mapid.ToString()))
            {
                return _need_line_map[mapid.ToString()];
            }
            else
            {
                return -1;
            }
        }
        //---------------------------线路限制地图配置      end----------------------------------------------------
        //---------------------------藏宝阁  start-----------------------------------------
        public Variant GetRanShopData()
        {
            return conf["randstore"][0];
        }
        public Variant GetRanShopByLevel(uint level)
        {
            Variant randstoreData = conf["randstore"][0];
            if (randstoreData != null)
            {
                Variant randshops = randstoreData["rs"];
                foreach (Variant rs in randstoreData["rs"]._arr)
                {
                    if (rs["minlvl"]._int <= level && (rs["maxlvl"]._int) >= level)
                    {
                        return rs;
                    }
                }
            }
            return null;
        }
        public Variant GetRanShopItemCost(uint id)
        {
            Variant randstoreData = conf["randstore"][0];
           
            if (randstoreData != null)
            {
                Variant itms = randstoreData["itm"];
                if (itms != null)
                {
                    foreach (Variant itm in itms._arr)
                    {
                        if (itm["id"]._uint == id)
                            return itm;
                    }
                }
               
            }
            return null;
        }
        //---------------------------藏宝阁  end-----------------------------------------
        //---------------------------卓越、强化连锁  start-----------------------------------------
        private Variant _wareExattData;
        private Variant _wareFlvlData;
        private Variant _sortExattData;
        private Variant _sortFlvlData;
        /**
		 * 获取卓越连锁数据
		 */
        public Variant GetWareExattData()
        {
            if (_wareExattData == null)
            {
                _wareExattData = new Variant();
                Variant wareEqpData = conf["ware_eqp_act"];
                foreach (Variant obj in wareEqpData._arr)
                {
                    if (obj["tp"]._int == 2)
                    {
                        _wareExattData._arr.Add(obj);
                    }
                }
            }
            if (_sortExattData == null)
            {
                _sortExattData = new Variant();
                foreach (Variant exatObj in _wareExattData._arr)
                {
                    Variant a = new Variant();
                    a["cnt"] = exatObj["eqpchk"][0]["cnt"];
                    a["data"] = exatObj;
                    _sortExattData._arr.Add(a);
                }
                _sortExattData._arr.Sort();
            }
            return _sortExattData;
        }
        /**
        * 获取强化连锁数据
        */
        public Variant GetWareFlvlData()
        {
            if (_wareFlvlData == null)
            {
                _wareFlvlData = new Variant();
                Variant wareEqpData = conf["ware_eqp_act"];
                foreach (Variant obj in wareEqpData._arr)
                {
                    if (obj["tp"] == 1)
                    {
                        _wareFlvlData._arr.Add(obj);
                    }
                }
            }
            if (_sortFlvlData == null)
            {
                _sortFlvlData = new Variant();
                foreach (Variant flvlObj in _wareFlvlData._arr)
                {
                    Variant data = new Variant();
                    data["flvl"] = flvlObj["eqpchk"][0]["flvl"];
                    data["data"] = flvlObj;
                    _sortFlvlData._arr.Add(data);
                }
                _sortFlvlData._arr.Sort();
            }
            return _sortFlvlData;
        }
        /**
        * 获取卓越连锁等级
        */
        public int GetWareExattDataLvl(int cnt)
        {
            if (_sortExattData != null)
            {
                for (int i = 0; i < _sortExattData.Count; i++)
                {
                    if (_sortExattData[i]["cnt"] == cnt)
                    {
                        int level = i + 1;
                        if (level > _sortExattData.Count - 1)
                        {
                            level = _sortExattData.Count - 1;
                        }
                        return level;
                    }
                }
            }
           
            return 0;
        }
        /**
		 * 获取强化连锁等级
		 */
        public int GetWareFlvlDataLvl(int flvl)
        {
            if (_sortFlvlData != null)
            {
                for (int i = 0; i < _sortFlvlData.Count; i++)
                {
                    if (_sortFlvlData[i]["flvl"] == flvl)
                    {
                        int level = i + 1;
                        if (level > _sortFlvlData.Count - 1)
                        {
                            level = _sortFlvlData.Count - 1;
                        }
                        return level;
                    }
                }
            }
            return 0;
        }
        //---------------------------卓越、强化连锁  end-----------------------------------------
        //---------------------------副本时间 start-----------------------------------------
        public float get_lvl_pvpinfo_get_tm()
        {
            return conf["game"]["lvl_pvpinfo_get_tm"]._float;
        }
        //---------------------------副本时间 end-----------------------------------------
        //---------------------------------获取Q点兑换yb 配置--------------------------------------------------------------------
        private Variant _pmkt_yb;
        public Variant get_pmkt_yb()
        {
            if (_pmkt_yb == null)
            {
                _pmkt_yb = new Variant();
                if (conf.ContainsKey("pmkt_yb"))
                {
                    foreach (Variant obj in conf["pmkt_yb"]._arr)
                    {
                        _pmkt_yb._arr.Add(obj);
                    }
                }
                _pmkt_yb._arr.Sort();
            }
            return _pmkt_yb;
        }
        //------------------------------分享配置----------------------
        //每日
        public Variant GetDailyShare()
        {
            return conf["divt_awd"];
        }
        //累计
        public Variant GetAcuShare()
        {
            return conf["ivtlvl_awd"];
        }
        // 服务器坐骑养成配置
        public Variant getMountCulSConf()
        {
            return conf["mount"];
        }
        public int GetMountAvatar(int qual)
        {
            if (conf.ContainsKey("mount"))
            {
                Variant quals = conf["mount"]["qual"];
                if (quals != null)
                {
                    foreach (Variant obj in quals._arr)
                    {
                        if (obj["val"] == qual)
                            return obj["mid"]._int;
                    }
                }
            }
            return 0;
        }
        /**
        * 获取坐骑对应等级配置属性
        */
        public Variant GetMountLvlAtt(uint lvl, Variant mount)
        {
            Variant att = new Variant();
            Variant mountData = mount;
            if (mountData == null)
                mountData = conf["mount"];
            if (mountData != null && mountData.ContainsKey("lvl"))
            {
                Variant mountLvls = mountData["lvl"];
                foreach (Variant lvlObj in mountLvls._arr)
                {
                    if (lvlObj.ContainsKey("val") && lvlObj["val"]._uint == lvl)
                    {
                        //等级属性设置
                        att = lvlObj["att"][0];
                        break;
                    }
                }
            }
            return att;
        }
        /**
        * 获取坐骑对应等级配置
        */
        public Variant GetMountLvl(uint lvl)
        {
            if (conf.ContainsKey("mount") && conf["mount"].ContainsKey("lvl"))
            {
                Variant mountLvls = conf["mount"]["lvl"];
                foreach (Variant lvlObj in mountLvls._arr)
                {
                    if (lvlObj.ContainsKey("val") && lvlObj["val"]._uint == lvl)
                    {
                        return lvlObj;
                    }
                }
            }
            return null;
        }
        /**
		 * 获取坐骑对应等级配置
		 */
        public Variant GetMountQualLvl(int lvl)
        {
            if (conf.ContainsKey("mount"))
            {
                Variant mountObj = conf["mount"];
                int qual_section = mountObj["qual_section"]._int;
                if (mountObj.ContainsKey("lvl"))
                {
                    int qual = lvl / qual_section;
                    int mountlvl = lvl % qual_section;
                    if (mountlvl == 0)
                    {
                        if (qual != 0)
                            qual = qual - 1;
                        mountlvl = qual_section;
                    }
                    Variant mountLvls = conf["mount"]["lvl"];
                    foreach (Variant lvlObj in mountLvls._arr)
                    {
                        if (lvlObj.ContainsKey("val") && lvlObj["val"]._int == lvl)
                        {
                            Variant data = new Variant();
                            data["qual"] = qual;
                            data["lvl"] = mountlvl;
                            data["data"] = lvlObj;
                            return data;
                        }
                    }
                }
            }
            return null;
        }
        /**
		 * 获取坐骑对应等阶属性配置
		 */

        private Variant _qualAttObj;
        public Variant GetMountQualAtt(uint qual)
        {
            if (_qualAttObj == null)
                _qualAttObj = new Variant();
            if (conf.ContainsKey("mount") && !_qualAttObj.ContainsKey("qual"))
            {
                Variant mountObj = conf["mount"];
                int qual_section = mountObj["qual_section"]._int;
                if (mountObj.ContainsKey("qual"))
                {
                    Variant quals = mountObj["qual"];
                    foreach (Variant qualObj in quals._arr)
                    {
                        if (qualObj["val"] == qual)
                            _qualAttObj["qual"] = qualObj;
                    }
                }
            }
            return _qualAttObj["qual"];
        }
        /**
		 * 计算战斗力配置
		 */
        private Variant combptConf;
        public Variant GetCombptConf()
        {
            if (conf.ContainsKey("combpt"))
            {
                if (combptConf == null)
                {
                    combptConf = new Variant();
                    foreach (Variant obj in conf["combpt"]._arr)
                    {
                        combptConf[obj["attname"]] = obj["per"];
                    }
                }
            }
            return combptConf;
        }
        public Variant GetRoomInfo()
        {
            return conf["room"];
        }
        //目标任务配置
        public Variant GetGmisConf()
        {
            return conf["gmis"];
        }
        private Variant _gmis_killmon;
        public Variant GetGmisKillmon()
        {
            if (_gmis_killmon == null)
            {
                _gmis_killmon = new Variant();
                foreach (Variant gmisObj in conf["gmis"]._arr)
                {
                    Variant killmons = new Variant();
                    foreach (Variant goalObj in gmisObj["goal"]._arr)
                    {
                        if (!goalObj.ContainsKey("kilmon"))
                            continue;
                        foreach (Variant killObj in goalObj["kilmon"]._arr)
                        {
                            if (killmons._str.IndexOf(killObj["monid"]._str) == -1)
                            {
                                killmons._arr.Add(killObj["monid"]);
                            }
                        }
                    }
                    Variant data = new Variant();
                    data["id"] = gmisObj["id"];
                    data["km"] = killmons;
                    _gmis_killmon._arr.Add(data);
                }
            }
            return _gmis_killmon;
        }
        public Variant GetGmisConfById(uint id)
        {
            if (conf.ContainsKey("gmis"))
                if (conf["gmis"].ContainsKey(id.ToString()))
                return conf["gmis"][id.ToString()]._uint;
            return null;
        }
        public Variant GetGmisAwdById(uint id)
        {
            if (conf.ContainsKey("gmis") && conf["gmis"][id.ToString()] != null)
            {
                Variant gmis = conf["gmis"][id.ToString()];
                Variant awd = new Variant();
                Variant vipawd = new Variant();
                if (gmis["awd"] != null)
                {
                    awd = gmis["awd"][0];
                }
                if (gmis["vipawd"] != null)
                {
                    vipawd = gmis["vipawd"][0];
                }
                Variant data = new Variant();
                data["id"] = id;
                data["awd"] = awd;
                data["vipawd"] = vipawd;
                return data;
            }
            return null;
        }
        public Variant GetGmisGoalById(uint id)
        {
            if (conf.ContainsKey("gmis") && conf["gmis"].ContainsKey(id.ToString()) && conf["gmis"][id.ToString()] != null)
            {
                Variant gmis = conf["gmis"][id.ToString()];
                return gmis["goal"][0];
            }
            return null;
        }
        //获得月投资配置
        public Variant GetMonthInvest()
        {
            if (conf.ContainsKey("monthinvest"))
                return conf["monthinvest"];
            return null;
        }
        public Variant GetUplvlInvest()
        {
            if (conf.ContainsKey("uplvlinvest"))
                return conf["uplvlinvest"];
            return null;
        }
        public Variant GetArenaCwinAwd()
        {
            if (conf.ContainsKey("arena_cwin_awd"))
                return conf["arena_cwin_awd"];
            return null;
        }
        //---------------------------------进入跨服战清除其他职业 buff  start-------------------------------------------------------------------
        public bool CarrBStateLimit(uint carr, uint bstid)
        {
            if (conf.ContainsKey("state_limit"))
            {
                Variant state_limit = conf["state_limit"];
                if (state_limit != null)
                {
                    foreach (Variant limit in state_limit._arr)
                    {
                        if (limit["carr"]._uint == carr)
                        {
                            Variant bstate = limit["bstate"];
                            foreach (Variant id in bstate._arr)
                            {
                                if (id._uint == bstid)
                                    return true;
                            }
                            break;
                        }
                    }
                }
            }
            return false;
        }
        public bool CarrStateLimit(uint carr, uint stid)
        {
            if (conf.ContainsKey("state_limit"))
            {
                Variant state_limit = conf["state_limit"];
                if (state_limit != null)
                {
                    foreach (Variant limit in state_limit._arr)
                    {
                        if (limit["carr"]._uint == carr)
                        {
                            Variant state = limit["state"];
                            foreach (Variant id in state._arr)
                            {
                                if (id._uint == stid)
                                    return true;
                            }
                            break;
                        }
                    }
                }
            }
            return false;
        }
        //---------------------------------进入跨服战清除其他职业 buff  end-------------------------------------------------------------------
        public uint GetLevelShareRep(uint tp)
        {
            if (conf.ContainsKey("level_share"))
            {
                foreach (Variant shareObj in conf["level_share"]._arr)
                {
                    if (shareObj["tp"]._uint == tp)
                    {
                        if (shareObj.ContainsKey("dailyrep"))
                            return shareObj["dailyrep"]._uint;
                        else if (shareObj.ContainsKey("rep"))
                            return shareObj["rep"]._uint;
                    }
                }
            }
            return 0;
        }
        //---------------------------------离线奖励系数   start-------------------------------------------------------------------
        public Variant GetOflExp()
        {
            return conf["ofl_exp"];
        }
        //---------------------------------离线奖励系数   end-------------------------------------------------------------------
        public Variant GetRedPaperSvrConf()
        {
            if (conf.ContainsKey("red_paper") && conf["red_paper"] != null)
                return conf["red_paper"][0];
            return null;
        }
        /**
       * 基础属性
       */
        public Variant GetBaseAtt(int carr)
        {
            Variant carrConf = conf["carr"][carr.ToString()];
            if (carrConf != null && carrConf["baseatt"] != null)
            {
                return carrConf["baseatt"][0];
            }
            return null;
        }
    }
}
