using System;
using Cross;
using GameFramework;
using System.Collections.Generic;

namespace MuGame
{
    public class SvrItemConfig : configParser
    {
        static public SvrItemConfig instance;
        protected Variant _item_data;
        protected Variant _fb_awd;
        protected bool _hasSetFbAwd = false;

        public SvrItemConfig(ClientConfig m)
            : base(m)
        {

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrItemConfig(m as ClientConfig);
        }
        override protected Variant _formatConfig(Variant conf)
        {
            instance = this;
            //add by tangchunguang 2015/07/13 begin
            if (conf.ContainsKey("forge_att_lvl"))
            {
                Variant conf_forge_att_lvl = new Variant();

                foreach (Variant v in conf["forge_att_lvl"]._arr)
                {
                    conf_forge_att_lvl[v["name"]] = v;
                }

                conf["forge_att_lvl"] = conf_forge_att_lvl;
            }
            //add by tangchunguang 2015/07/13 end

            //add by tangchunguang 2015/07/21 begin
            if (conf.ContainsKey("itm_merge_grp"))
            {
                Variant conf_itm_merge_grp = new Variant();

                foreach (Variant v in conf["itm_merge_grp"]._arr)
                {
                    conf_itm_merge_grp[v["id"]._int] = v;
                }

                conf["itm_merge_grp"] = conf_itm_merge_grp;
            }
            if (conf.ContainsKey("uitem"))
            {
                conf["uitem"] = conf["uitem"].convertToDct("id");
            }
            if (conf.ContainsKey("item"))
            {
                conf["item"] = conf["item"].convertToDct("id");
            }
            if (conf.ContainsKey("equip"))
            {
                conf["equip"] = conf["equip"].convertToDct("id");
            }
            if (conf.ContainsKey("stone"))
            {
                conf["stone"] = conf["stone"].convertToDct("id");
            }
            if (conf.ContainsKey("ex_att_grp"))
            {
                conf["ex_att_grp"] = conf["ex_att_grp"].convertToDct("id");
            }

            //add by tangchunguang 2015/07/21 end

            return conf;
        }
        public Variant get_item_conf(uint tpid)
        {
            if (this.m_conf == null)
            {
                return null;
            }

            if (m_conf["uitem"].ContainsKey(tpid.ToString()))
            {
                return GameTools.createGroup("tp", 1, "conf", m_conf["uitem"][tpid.ToString()]);
            }
            else if (m_conf["item"].ContainsKey(tpid.ToString()))//tpid in this._svrConfig.item)
            {
                return GameTools.createGroup("tp", 2, "conf", m_conf["item"][tpid.ToString()]);//{tp:2, conf:this._svrConfig.item[tpid]};
            }
            else if (m_conf["equip"].ContainsKey(tpid.ToString()))//tpid in this._svrConfig.equip)
            {
                return GameTools.createGroup("tp", 3, "conf", m_conf["equip"][tpid.ToString()]);//{tp:3, conf:this._svrConfig.equip[tpid]};
            }
            else if (m_conf["stone"].ContainsKey(tpid.ToString()))//tpid in this._svrConfig.stone)
            {
                return GameTools.createGroup("tp", 4, "conf", m_conf["stone"][tpid.ToString()]);//{tp:4, conf:this._svrConfig.stone[tpid]};
            }
            return null;
        }

        public Variant get_firstvip_awd()
        {
            if (_hasSetFbAwd)
            {
                return _fb_awd;
            }
            else
            {
                foreach (Variant dtObj in m_conf["uitem"].Values)
                {
                    if (null != dtObj["buyvip"])
                    {
                        if (null != dtObj["buyvip"][0])
                        {
                            if (null != dtObj["buyvip"][0]["fb_awd"])
                            {
                                _hasSetFbAwd = true;
                                _fb_awd = dtObj["buyvip"][0]["fb_awd"][0]["itm"];
                                return _fb_awd;
                            }
                        }
                    }
                }
                _hasSetFbAwd = true;
                return null;
            }
        }

        public int get_vip_lvl(int tpid)
        {
            for (int i = 0; i < m_conf["uitem"].Count; i++)
            {
                if (tpid == m_conf["uitem"][i]["tpid"])
                {
                    Variant conf = m_conf["uitem"][tpid];
                    if (null != conf["buyvip"][0])
                    {
                        return conf["buyvip"][0]["lvl"];
                    }
                }
            }
            return 0;
        }

        public bool on_item_data(ByteArray data)
        {
            //try
            //{
            //m_conf = Package.inst.unserialize_obj(data);

            // _item_data = new Variant();

            //_item_data.pushBack(m_conf["uitem"]);
            //_item_data.pushBack(m_conf["item"]);
            //_item_data.pushBack(m_conf["equip"]);
            //_item_data.pushBack(m_conf["stone"]);
            //}
            //catch(par:*)
            //{
            //    DebugTrace.add(DebugTrace.DTT_ERR, "SvrItemConfig on_item_data err:" + par);			
            //}

            return true;
        }

        public Variant Get_ex_att_grp(uint id)
        {
            return m_conf["ex_att_grp"][id];
        }

        public Variant getAllItemData()
        {
            _item_data = new Variant();
            _item_data.pushBack(m_conf["uitem"]);
            _item_data.pushBack(m_conf["item"]);
            _item_data.pushBack(m_conf["equip"]);
            _item_data.pushBack(m_conf["stone"]);
            return _item_data;
        }

        public Variant getComposeItemData(uint tpid)
        {
            if (m_conf != null)
            {
                return m_conf["itm_merge_grp"][(int)tpid];
            }
            return null;
        }

        public Variant getComposeItemDataArray(Variant recipes)
        {
            Variant arr = new Variant();
            if (m_conf != null)
            {
                foreach (int tpid in recipes._arr)
                {
                    arr.pushBack(m_conf["itm_merge_grp"][tpid]);
                }
                return arr;
            }
            return null;
        }

        //获得需求属性调整配置
        public Variant get_attchk_adjust()
        {
            if (null != m_conf)
            {
                return m_conf["attchk_adjust"][0];
            }
            return null;
        }

        //获得高级强化属性配置
        public Variant get_sup_frg_att()
        {
            if (null != m_conf)
            {
                return m_conf["sup_frg_att"];
            }
            return null;
        }

        //获得高级强化属性等级配置
        public Variant get_sup_frg_att_lvl_by_id(string id)
        {
            if (null != m_conf)
            {
                if (m_conf["sup_frg_att_lvl"].ContainsKey(id))
                {
                    return m_conf["sup_frg_att_lvl"][id]["att_lvl"];
                }
            }
            return null;
        }

        /**
		 * 根据id获取装备鉴定卓越属性组
		 * */
        public Variant get_veri_exatt_grp_by_id(int id)
        {
            if (m_conf && m_conf.ContainsKey("veri_exatt_grp"))
            {
                return m_conf["veri_exatt_grp"][id];
            }
            return null;
        }

        /**
         * 获取高级强化组
         * */
        public Variant get_sup_frg_att_grp_by_id(int id)
        {
            if (m_conf && m_conf.ContainsKey("sup_frg_att_grp"))
            {
                return m_conf["sup_frg_att_grp"][id];
            }
            return null;
        }

        /**
         * 装备卓越属性
         * */
        public Variant get_ex_att()
        {
            if (null != m_conf)
            {
                if (m_conf.ContainsKey("ex_att"))
                {
                    return m_conf["ex_att"];
                }
            }
            return null;
        }

        /**
		 * 装备追加属性
		 * */
        public Variant get_add_att()
        {
            if (m_conf != null)
            {
                if (m_conf.ContainsKey("add_att"))
                {
                    return m_conf["add_att"];
                }
            }
            return null;
        }

        /**
         * 获取add_att_grp
         * */
        public Variant get_add_att_grp()
        {
            if (m_conf != null)
            {
                if (m_conf.ContainsKey("add_att_grp"))
                {
                    return m_conf["add_att_grp"];
                }
            }
            return null;
        }

        /**
         * 装备追加属性flag_ex_att
         * */
        public Variant get_flag_ex_att()
        {
            if (m_conf != null)
            {
                if (m_conf.ContainsKey("flag_ex_att"))
                {
                    return m_conf["flag_ex_att"];
                }
            }
            return null;
        }

        /**
         * 获取flag_ex_grp
         * */
        public Variant get_flag_ex_grp()
        {
            if (m_conf != null)
            {
                if (m_conf.ContainsKey("flag_ex_grp"))
                {
                    return m_conf["flag_ex_grp"];
                }
            }
            return null;
        }

        /**
         * 获取某个forge_att
         * */
        public Variant GetForgeAttLvlById(string id)
        {
            if (null != m_conf)
            {
                return m_conf["forge_att_lvl"][id];
            }
            return null;
        }

        /**
         * 获取所有的forge_att
         * */
        public Variant GetForgeAttLvl()
        {
            if (null != m_conf)
            {
                return m_conf["forge_att_lvl"];
            }
            return null;
        }

        /**
		 * 获取某个高级强化组
		 * */
        public Variant GetSupfrgattgrpById(int id)
        {
            if (null != m_conf && m_conf.ContainsKey("sup_frg_att_grp"))
            {
                return m_conf["sup_frg_att_grp"][id];
            }
            return null;
        }

        /**
		 * 装备幸运属性
		 * */
        public Variant GetLuckAtt()
        {
            if (null != m_conf)
            {
                if (m_conf.ContainsKey("luck_att"))
                {
                    return m_conf["luck_att"];
                }
            }
            return null;
        }

        /**
		 * 获取套装属性
		 * */
        public Variant GetSuitConf(uint suitid)
        {
            for (int i = 0; i < m_conf["suit"].Count; i++)
            {
                if (suitid == m_conf["suit"][i]["suitid"]._uint)
                {
                    return m_conf["suit"][i];
                }
            }
            return null;
        }

        public bool is_uitem(uint item_id)
        {
            return m_conf["uitem"].ContainsKey(item_id.ToString());
            //for (int i = 0; i < m_conf["uitem"].Count; i++)
            //{
            //    if (item_id == m_conf["uitem"][i]["item_id"]._uint)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public bool is_equip(uint item_id)
        {
            return m_conf["equip"].ContainsKey(item_id.ToString());
            //for (int i = 0; i < m_conf["equip"].Count; i++)
            //{
            //    if (item_id == m_conf["equip"][i]["item_id"]._uint)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        //强化相关
        public Variant get_attchk_adjust_by_id(string id)
        {
            if (null != m_conf)
            {
                Variant tempData = m_conf["attchk_adjust"][0];

                return tempData[id];
            }
            return null;
        }

        public Variant get_forge_lvl_conf(uint lvl)
        {
            for (int i = 0; i < m_conf["forge_lvl"].Count; i++)
            {
                if (lvl == m_conf["forge_lvl"][i]["lvl"]._int)
                {
                    return m_conf["forge_lvl"][i];
                }
            }
            return null;
        }

        public Variant get_forge_att_lvl()
        {
            if (m_conf != null)
            {
                return m_conf["forge_att_lvl"];
            }
            return null;
        }

        public Variant get_forge_att_lvl_by_id(String id)
        {
            if (null != m_conf)
            {
                return m_conf["forge_att_lvl"][id];
            }
            return null;
        }

        /**
		 * 根据强化组id获取强化信息
		 * */
        public Variant GetForgeLvlByGrp(int grpid)
        {
            return m_conf["eqp_forge_grp"][grpid];
        }

        public Variant GetForgeGrp()
        {
            return m_conf["eqp_forge_grp"];
        }

        public int MaxgetForgetFpLvl()
        {
            if (m_conf.ContainsKey("add_att_lvl"))
            {
                Variant att_lvl = m_conf["add_att_lvl"];
                return att_lvl[att_lvl.Count - 1]["lvl"];
            }
            return 0;
        }

        protected Variant _forgeRateItm = null;
        public Variant GetForgeRate()
        {//获得强化符
            if (_forgeRateItm == null)
            {
                _forgeRateItm = new Variant();
                Variant item = m_conf["item"];
                foreach (Variant itm in item.Values)
                {
                    if (itm.ContainsKey("frgrate"))
                    {
                        _forgeRateItm.pushBack(itm);
                    }
                }
            }
            return _forgeRateItm;
        }

        public Variant get_add_att_lvl()
        {
            if (m_conf != null)
            {
                return m_conf["add_att_lvl"];
            }
            return null;
        }

        public Variant get_sup_frg_lvl()
        {
            if (null != m_conf)
            {
                return m_conf["sup_frg_lvl"];
            }
            return null;
        }

        /**
		 *获得锻造的最大等级 
		 */
        public int get_forge_max_lvl()
        {
            int length = m_conf["forge_lvl"].Count;
            return m_conf["forge_lvl"][length - 1]["lvl"];
        }

        //可使用传送的道具数量 
        public Variant get_fly_item_tpid()
        {
            Variant fly_items = new Variant();

            foreach (string key in m_conf.Keys)
            {
                Variant datas = m_conf[key];

                if (datas.Keys == null)
                {
                    for (int id = 0; id < datas.Length; id++)
                    //foreach (string id in datas._arr)
                    {
                        Variant obj = datas[id];
                        if (obj != null && obj.ContainsKey("trans") && obj["trans"] == 1)
                        {
                            fly_items.pushBack(obj["id"]);
                        }
                    }
                }
                else
                {
                    //for (int id = 0; id < datas.Length; id++)
                    foreach (string id in datas.Keys)
                    {
                        Variant obj = datas[id];
                        if (obj != null && obj.ContainsKey("trans") && obj["trans"]._str == "1")
                        {
                            fly_items.pushBack(obj["id"]);
                        }
                    }
                }

            }
            return fly_items;
        }

        //获得特殊道具物品
        public uint get_specialitm(string special)
        {
            if (m_conf == null)
            {
                return 0;
            }

            foreach (string key in m_conf["key"].Keys)
            {
                Variant obj = m_conf[key];
                foreach (string s in obj.Keys)
                {
                    Variant o = obj[s];
                    if (o.ContainsKey(special))
                    {
                        return o[special];
                    }
                }
            }
            return 0;
        }

        public Variant get_itm_decomp_grp()
        {
            return m_conf["itm_decomp_grp"];
        }

        public Variant get_itm_merge_grp_by_id(int id)
        {
            if (m_conf.ContainsKey("itm_merge_grp"))
            {
                return m_conf["itm_merge_grp"][id];
            }
            return null;
        }

        //获取npc出售物品的各种属性
        public Variant get_sell_eqp_att_by_id(int id)
        {
            if (m_conf.ContainsKey("sell_eqp_att"))
            {
                return m_conf["sell_eqp_att"][id];
            }
            return null;
        }

        //获取商城出售物品的各种属性
        public Variant get_mkt_eqp_att_by_id(int id)
        {
            if (m_conf.ContainsKey("mkt_eqp_att"))
            {
                return m_conf["mkt_eqp_att"][id];
            }
            return null;
        }

        /**
         *获得寻宝物品中的各种属性
         */
        public Variant get_make_att(int id)
        {
            if (m_conf.ContainsKey("make_att"))
            {
                return m_conf["make_att"][id];
            }
            return null;
        }

        /**
         *获得藏宝阁物品中的各种属性
         */
        public Variant get_rstore_eqp_att_by_id(int id)
        {
            if (m_conf.ContainsKey("rstore_eqp_att"))
            {
                return m_conf["rstore_eqp_att"][id];
            }
            return null;
        }
        //--------------------------------------------------------------------------------------------------------
        /**
         * 检测物品的各种限制
         * */
        public bool CheckItmAtt(Variant eqp, Variant chk_confs)
        {
            foreach (Variant chk in chk_confs._arr)
            {
                int eqp_val = 0;

                if (!eqp.ContainsKey(chk["name"]._str))
                {
                    if (chk["name"] == "exatt_cnt")
                    {
                        Variant exArr = DecodeExatt(eqp["exatt"], eqp);
                        eqp_val = exArr.Count;
                    }
                    else if (chk["name"] == "grade")
                    {
                        Variant itemConf = get_item_conf(eqp["tpid"]);
                        if (null != itemConf && itemConf["conf"])
                        {
                            eqp_val = itemConf["conf"]["grade"];
                        }
                    }
                    else if (chk["fun"] != "notmatch")
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    eqp_val = eqp[chk["name"]];
                }

                if ("equal" == chk["fun"])
                {
                    if (chk["val"] != eqp_val)
                    {
                        return false;
                    }
                }
                else if ("min" == chk["fun"])
                {
                    if (chk["val"] > eqp_val)
                    {
                        return false;
                    }
                }
                else if ("max" == chk["fun"])
                {
                    if (chk["val"] < eqp_val)
                    {
                        return false;
                    }
                }
                else if ("match" == chk["fun"])
                {
                    bool matched = false;
                    foreach (int val in chk["val"].Values)
                    {
                        if (val == eqp_val)
                        {
                            matched = true;
                            break;
                        }
                    }
                    if (!matched)
                    {
                        return false;
                    }
                }
                else if ("notmatch" == chk["fun"])
                {
                    bool notmatch = true;
                    foreach (int val_no in chk["val"].Values)
                    {
                        if (val_no == eqp_val)
                        {
                            notmatch = false;
                            break;
                        }
                    }
                    if (!notmatch)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        //-------------------------------------------------- 解析  -----------------------------------------------
        /**
         * 解析高级强化
         * */
        public Variant DecodeSupfrgatt(int values)
        {
            Variant arr = new Variant();
            for (int i = 0; i < 3; ++i)
            {
                int id = (values >> (i * 10 + 4)) & 0x3f;
                int lvl = (values >> i * 10) & 0x0f;

                Variant info = new Variant();
                info["id"] = id;
                info["lvl"] = lvl;
                arr._arr.Add(info);
            }
            return arr;
        }

        /**
         * 解析额外追加属性
         * ...
         * */
        public Variant DecodeExAddatt(int values)
        {
            if (values > 0)
            {
                Variant arr = new Variant();
                int id = 0;
                int lvl = 0;
                for (int i = 0; i < 2; ++i)
                {
                    lvl = (values >> (i * 8)) & 0x0f;
                    id = (values >> (i * 8 + 4)) & 0x0f;
                    if (id > 0)
                    {
                        Variant info = new Variant();
                        info["id"] = id;
                        info["lvl"] = lvl;
                        arr._arr.Add(info);
                    }
                }
                return arr;
            }
            return null;
        }

        /**
         * 解析追加属性
         * */
        public Variant DecodeAddatt(int values)
        {
            if (values > 0)
            {
                Variant arr = new Variant();
                int id = 0;
                int lvl = 0;
                for (int i = 0; i < 2; ++i)
                {
                    lvl = (values >> (i * 8)) & 0x0f;
                    id = (values >> (i * 8 + 4)) & 0x0f;
                    if (id > 0)
                    {
                        Variant info = new Variant();
                        info["id"] = id;
                        info["lvl"] = lvl;
                        arr._arr.Add(info);
                    }
                }
                return arr;
            }
            return null;
        }

        /**
         * 解析卓越属性
         * */
        public Variant DecodeExatt(int values, Variant data)
        {
            Variant arr = new Variant();
            Variant tempArr = m_conf["ex_att"];
            int exlevel1 = 0;
            int exlevel2 = 0;
            if (null != data)
            {
                if (data.ContainsKey("exlevel1"))
                {
                    exlevel1 = data["exlevel1"];
                }

                if (data.ContainsKey("exlevel2"))
                {
                    exlevel2 = data["exlevel2"];
                }
            }
            if (null != tempArr)
            {
                for (int i = 0; i < tempArr.Count; ++i)
                {
                    int lvl = -1;
                    int yb = 0;
                    Variant obj = tempArr[i];
                    //Variant attObj = {name:obj["att"][0]["name"], val:obj["att"][0]["val"], lvl:lvl};
                    Variant attObj = new Variant();
                    attObj["name"] = obj["att"][0]["name"];
                    attObj["val"] = obj["att"][0]["val"];
                    attObj["lvl"] = lvl;

                    Variant lvls = obj["att"][0]["lvs"];
                    if (i < 8)
                    {
                        lvl = (exlevel1 >> 4 * i) & 0xf;
                    }
                    else
                    {
                        lvl = (exlevel2 >> 4 * (i - 8)) & 0xf;
                    }
                    if (null != lvls)
                    {
                        foreach (Variant lvlObj in lvls.Values)
                        {
                            if (lvlObj["lv"] == lvl)
                            {
                                attObj["val"] = lvlObj["val"];
                                break;
                            }
                        }
                    }
                    if (obj["id"] <= values)
                    {
                        if (((int)(values & obj["id"])) == obj["id"]._int)
                        {
                            yb += obj.ContainsKey("yb") ? obj["yb"]._int : 0;

                            Variant info = new Variant();
                            info["att"] = attObj;
                            info["id"] = obj["id"];
                            info["yb"] = yb;
                            arr._arr.Add(info);
                        }
                    }
                }
            }
            return arr;
        }

        /**
         *坐骑装备位置 
         */
        public Variant GetRideEquipPos()
        {
            if (m_conf.ContainsKey("ride_eqp_pos"))
            {
                return m_conf["ride_eqp_pos"][0];
            }
            return null;
        }

        /**
         *翅膀装备位置 
         */
        public Variant GetWingEquipPos()
        {
            if (m_conf.ContainsKey("wing_eqp_pos"))
            {
                return m_conf["wing_eqp_pos"][0];
            }
            return null;
        }

        /**
         * 装备熔炼
         */
        public Variant GetEqpSmelt()
        {
            return m_conf["eqp_smelt"];
        }

        /**
         *获得装备等级
         */
        public int GetItemGrade(uint tpid)
        {
            if (m_conf["uitem"].ContainsKey("tpid"))
            {
                return -m_conf["uitem"][tpid]["grade"]._int;
            }
            else if (m_conf["item"].ContainsKey("tpid"))
            {
                return m_conf["item"][tpid]["grade"]._int;
            }
            else if (m_conf["equip"].ContainsKey("tpid"))
            {
                return m_conf["equip"][tpid]["grade"]._int;
            }
            else if (m_conf["stone"].ContainsKey("tpid"))
            {
                return m_conf["stone"][tpid]["grade"]._int;
            }
            return 0;
        }

        /**
         * 通过技能id获取物品
         */
        public Variant GetSkilItemBySkid(uint skid)
        {
            foreach (Variant uitem in m_conf["uitem"].Values)
            {
                Variant skillbk = uitem["skillbk"];
                foreach (Variant skillObj in skillbk.Values)
                {
                    Variant skill = skillObj["skill"];
                    if (null != skill && skill[0]["skid"] && skill[0]["skid"] == skid)
                    {
                        return uitem;
                    }
                }
            }
            return null;
        }

        /**
         * 装备稀有属性
         * */
        public Variant get_rare_att()
        {
            if (null != m_conf)
            {
                if (m_conf.ContainsKey("rare_att"))
                {
                    return m_conf["rare_att"];
                }
            }
            return null;
        }

        public Variant Get_rare_att_grp(uint id)
        {
            return m_conf["rare_att_grp"][id];
        }

        /**
         * 解析稀有属性
         * */
        public Variant DecodeRareExatt(int values, Variant data)
        {
            Variant arr = new Variant();
            Variant tempArr = m_conf["rare_att"];
            if (null != tempArr)
            {
                for (int i = 0; i < tempArr.Count; ++i)
                {
                    int yb = 0;
                    Variant obj = tempArr[i];
                    if (obj["id"] <= values)
                    {

                        if (((int)(values & obj["id"])) == obj["id"]._int)
                        {
                            yb += obj["yb"];

                            Variant info = new Variant();
                            info["att"] = obj["att"][0];
                            info["id"] = obj["id"];
                            info["yb"] = yb;
                            arr._arr.Add(info);
                        }
                    }
                }
            }
            return arr;
        }
    }
}
