using System.Collections.Generic;
using Cross;
using UnityEngine;
using System;

namespace MuGame
{
    internal class A3_SummonModel : ModelBase<A3_SummonModel>
    {
        private SXML itemsXMl;
        private Dictionary<uint, a3_BagItemData> _summons;
        public int appraiseCost { get; set; }
        public List<int> feedexplist = new List<int>();
        public List<int> feedsmlist = new List<int>();
        public uint lastatkID;//上次死亡的召唤兽id
        public uint nowShowAttackID;//当前出战的召唤兽uid
        public uint nowShowAttackModel;//当前召唤兽攻击模式
        public uint toAttackID;//由于主人死亡保存的出战召唤兽id
        public bool canFindsum = true;
        private Dictionary<int, float> sum_cds = new Dictionary<int, float>();
        public Dictionary<int, limitInfo> limitList = new Dictionary<int, limitInfo>();
        public List<uint> link_list = new List<uint>();
        public int allcount = 50;
        public int maxSm = 1000;

        public uint lastSummonID = 0;

        public A3_SummonModel()
            : base()
        {
            itemsXMl = XMLMgr.instance.GetSXML("callbeast");
            _summons = new Dictionary<uint, a3_BagItemData>();
            var appcost = itemsXMl.GetNode("appraise");
            appraiseCost = appcost.getInt("gold_cost");
            var feed = itemsXMl.GetNode("feed");
            string io = feed.getString("itm_order");
            var ss = io.Split(',');
            foreach (var v in ss) {
                feedexplist.Add(int.Parse(v));
            }
            var life = itemsXMl.GetNode("life");
            string sm = life.getString("itm_id");
            var ssm = sm.Split(',');
            foreach (var v in ssm) {
                feedsmlist.Add(int.Parse(v));
            }

            List<SXML> limitXml = itemsXMl.GetNodeList("limit");
            foreach (SXML s in limitXml)
            {
                limitInfo l = new limitInfo();
                l.topType = s.getInt("type");
                l.lvl = s.getInt("lvl");
                limitList[l.topType] = l;
            }

        }

        public Vector2 GetCombiningCost(int grand) {
            var combiningcost = itemsXMl.GetNode("combining", "quality==" + grand);
            int id = combiningcost.getInt("item_id");
            int num = combiningcost.getInt("num");
            return new Vector2(id, num);
        }

        public Vector4 GetTalentTypeMax(int talentid) {
            var idx = itemsXMl.GetNode("talent", "talent_id==" + talentid);
            int attmax = idx.GetNode("att").getInt("max");
            int defmax = idx.GetNode("def").getInt("max");
            int agimax = idx.GetNode("agi").getInt("max");
            int conmax = idx.GetNode("con").getInt("max");
            return new Vector4(attmax, defmax, agimax, conmax);
        }

        public Dictionary<uint, a3_BagItemData> GetSummons(bool Sort = false)
        {
            if (!Sort)
                return _summons;
            else {
                List<a3_SummonData> tS = new List<a3_SummonData>();
                Dictionary<uint, a3_BagItemData> sT = new Dictionary<uint, a3_BagItemData>();
                foreach (var v in _summons.Values)
                {
                    tS.Add(v.summondata);
                }
                tS.Sort();
                foreach (var v in tS)
                {
                    sT[(uint)v.id] = _summons[(uint)v.id];
                }
                return sT;
            }
        }

        public a3_SummonData GetSummonData(uint id)
        {
            return _summons.ContainsKey(id) ? _summons[id].summondata : default(a3_SummonData);
        }

        public SXML GetItemXml(uint tpid)
        {
            var vv = itemsXMl.GetNode("callbeast", "id==" + tpid);
            return vv;
        }

        public int getAttValue(a3_SummonData data, int type)
        {
            switch (type)
            {
                case 14:
                    return data.maxhp;
                case 5:
                    return data.max_attack;
                case 38:
                    return data.min_attack;
                case 6:
                    return data.physics_def;
                case 7:
                    return data.magic_def;
                case 19:
                    return data.physics_dmg_red;
                case 20:
                    return data.magic_dmg_red;
                case 22:
                    return data.double_damage_rate;
                case 23:
                    return data.reflect_crit_rate;
                case 33:
                    return data.fatal_damage;
                case 43:
                    return data.hit;
                case 44:
                    return data.dodge;
                default: return 1;
            }
        }

        public SXML GetAttributeXml(int level)
        {
            var vv = itemsXMl.GetNode("attribute", "level==" + level);
            return vv;
        }

        public SXML GetItemFromBaby(uint babyid)
        {
            var vv = itemsXMl.GetNode("callbeast", "need_item==" + babyid);
            return vv;
        }

        a3_BagItemData SetDataFromXML(a3_BagItemData itemData)
        {
            var xml = GetItemXml((uint)itemData.summondata.tpid);
            if (xml == null) return itemData;
            itemData.summondata.name = xml.getString("name");
            var mid = xml.getInt("mid");
            var mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            itemData.summondata.objid = mxml.getInt("obj");
            //itemData.summondata.isSpecial = itemData.summondata.tpid>4300 ? false : true;
            return itemData;
        }

        a3_BagItemData SetDataFromVariant(a3_BagItemData itemData, Variant item)
        {
            itemData.summondata.id = item["id"];
            itemData.summondata.tpid = item["tpid"];
            itemData.summondata.level = item["level"];
            itemData.summondata.currentexp = item["exp"];
            if (item.ContainsKey("hp")) itemData.summondata.currenthp = item["hp"];
            itemData.summondata.lifespan = item["life"];
            itemData.summondata.power = item["combpt"];
            //itemData.summondata.grade = item["quality"];
            //itemData.summondata.isSpecial = item["type"]==2;
            //itemData.summondata.naturaltype = item["speciality"];
            //itemData.summondata.blood = item["bloodline"];
            itemData.summondata.luck = item["luckly"];
            //itemData.summondata.talent_type = item["talent_type"];
            //itemData.summondata.skillNum = item["skill_num"];
            itemData.summondata.attNatural = item["att"];
            itemData.summondata.defNatural = item["def"];
            itemData.summondata.agiNatural = item["agi"];
            itemData.summondata.conNatural = item["con"];
            itemData.summondata.star = item["talent"];
            if (item.ContainsKey("status")) itemData.summondata.status = item["status"];
            if (itemData.summondata.status > 0) nowShowAttackID = (uint)itemData.summondata.id;
            Variant atts = item["battleAttrs"];
            itemData.summondata.maxhp = atts["max_hp"];
            itemData.summondata.max_attack = atts["max_attack"];
            itemData.summondata.min_attack = atts["min_attack"];
            itemData.summondata.physics_def = atts["physics_def"];
            itemData.summondata.magic_def = atts["magic_def"];
            itemData.summondata.physics_dmg_red = atts["physics_dmg_red"];
            itemData.summondata.magic_dmg_red = atts["magic_dmg_red"];
            itemData.summondata.double_damage_rate = atts["fatal_att"];
            itemData.summondata.reflect_crit_rate = atts["fatal_dodge"];
            itemData.summondata.fatal_damage = atts["fatal_damage"];
            itemData.summondata.hit = atts["hit"];
            itemData.summondata.dodge = atts["dodge"];

            if (item.ContainsKey("skills"))
            {
                Variant sks = item["skills"];
                if (itemData.summondata.skills == null) itemData.summondata.skills = new Dictionary<int, summonskill>();
                for (int i = 0; i < sks.Count; i++)
                {
                    //itemData.summondata.skills[sks[i]["index"]] = sks[i]["skill_id"];
                    summonskill skill = new summonskill();
                    skill.skillid = sks[i]["skill_id"];
                    skill.skilllv = sks[i]["skill_lvl"];
                    itemData.summondata.skills[sks[i]["skill_id"]] = skill;
                }
            }


            if (item.ContainsKey("link_ply")) {
                Variant link = item["link_ply"];
                if (itemData.summondata.linkdata == null) itemData.summondata.linkdata = new Dictionary<int, link_data>();
                float Combpt = 0;
                for (int i = 0; i < link.Count; i++) {
                    link_data li = new link_data();
                    li.type = link[i]["att_type"];
                    li.per = link[i]["att_per"];
                    li.lock_state = link[i]["lock_state"];
                    itemData.summondata.linkdata[i] = li;

                    SXML x = XMLMgr.instance.GetSXML("calculate.combpt", "att_id==" + li.type);
                    if (x != null) {
                        float attvalue = (int)Math.Ceiling((A3_SummonModel.getInstance().getAttValue(itemData.summondata, li.type) * ((float)li.per / 100.00f)));
                        Combpt += (attvalue * x.getFloat("sm_per")) / 10000;
                    }
                }
                itemData.summondata.linkCombpt = (int)Combpt;
            }
            if (item.ContainsKey("att_soul"))
            {
                Variant soul = item["att_soul"];
                if (itemData.summondata.shouhun == null) itemData.summondata.shouhun = new Dictionary<int, summonshouhun>();


                for (int i = 0; i < soul.Count; i++)
                {
                    summonshouhun sh = new summonshouhun();
                    sh.soul_type = soul[i]["soul_type"];
                    sh.lvl = soul[i]["soul_lvl"];
                    sh.exp = soul[i]["soul_exp"];
                    itemData.summondata.shouhun[soul[i]["soul_type"]] = sh;
                }
            }

            if (item.ContainsKey("reset_talent") && item["reset_talent"].Count > 0)
            {
                Variant reset_talent = item["reset_talent"];
                itemData.summondata.haveReset = true;
                itemData.summondata.resetluck = reset_talent["luckly"];
                itemData.summondata.resetatt = reset_talent["att"];
                itemData.summondata.resetdef = reset_talent["def"];
                itemData.summondata.resetagi = reset_talent["agi"];
                itemData.summondata.resetcon = reset_talent["con"];
            }
            else { itemData.summondata.haveReset = false; }

            return itemData;
        }

        a3_BagItemData SetBagItemData(a3_BagItemData itemData, Variant item)
        {
            SXML sumXml = XMLMgr.instance.GetSXML("callbeast.callbeast", "id==" + itemData.summondata.tpid);
            SXML attxml = sumXml.GetNode("star", "star_sum==" + itemData.summondata.star);
            int tpid = attxml.getInt("info_itm");
            var xmlSummon = GetItemXml((uint)itemData.summondata.tpid);
            //itemData.confdata.item_name = xmlSummon.getString("name");
            itemData.id = item["id"];
            itemData.tpid = item["tpid"];
            var s_xml = a3_BagModel.getInstance().getItemXml(tpid);
            if (s_xml != null)
            {
                itemData.confdata.item_name = s_xml.getString("item_name");
                itemData.confdata.file = "icon_item_" + s_xml.getString("icon_file");
                itemData.confdata.borderfile = "icon_itemborder_b039_0" + s_xml.getString("quality");
                itemData.confdata.item_name = s_xml.getString("item_name");
                itemData.confdata.quality = s_xml.getInt("quality");
                itemData.confdata.desc = s_xml.getString("desc");
                itemData.confdata.desc2 = s_xml.getString("desc2");
                itemData.confdata.value = s_xml.getInt("value");
                itemData.confdata.use_lv = s_xml.getInt("use_lv");
                itemData.confdata.use_limit = s_xml.getInt("use_limit");
                itemData.confdata.use_type = s_xml.getInt("use_type");
                int score = s_xml.getInt("intensify_score");
                itemData.confdata.intensify_score = score;
                itemData.confdata.item_type = s_xml.getInt("item_type");
                itemData.confdata.equip_type = s_xml.getInt("equip_type");
                itemData.confdata.equip_level = s_xml.getInt("equip_level");
                itemData.confdata.job_limit = s_xml.getInt("job_limit");
                itemData.confdata.modelId = s_xml.getInt("model_id");
                //itemData.confdata.on_sale = s_xml.getInt("on_sale");
            }
            return itemData;
        }

        public a3_BagItemData GetSummonData(a3_BagItemData itemData, Variant item)
        {
            itemData.isSummon = true;
            SXML xmldata = null;
            //        if (IsBaby(itemData))
            //        {
            //            itemData.isSummon = false;
            //            xmldata = GetItemFromBaby(itemData.tpid);
            //            var vv = xmldata.getInt("talent_id");
            //if (vv >= 0) {
            //	var strv = GetSummonTypeById(vv);
            //	if (strv.Length >= 2) {
            //		itemData.summondata.isSpecial = true;
            //		itemData.summondata.grade = strv[1];
            //		itemData.summondata.naturaltype = strv[2];
            //	}
            //	else if (strv.Length >= 1) {
            //		itemData.summondata.isSpecial = false;
            //		itemData.summondata.grade = strv[1];
            //		itemData.summondata.naturaltype = strv[2];
            //	}
            //}
            //else {
            //	itemData.summondata.isSpecial = false;
            //	itemData.summondata.grade = 0;
            //	itemData.summondata.naturaltype = 0;
            //}
            //            return itemData;
            //        }
            // else
            {
                itemData = SetDataFromVariant(itemData, item);
                itemData = SetDataFromXML(itemData);
            }
            return itemData;
        }

        public a3_BagItemData SetBabyData(a3_BagItemData itemData, Variant item)
        {
            if (!IsBaby(itemData)) return itemData;
            itemData.isSummon = true;
            var xmldata = GetItemFromBaby(itemData.tpid);
            var vv = xmldata.getInt("talent_id");
            var strv = GetSummonTypeById(vv);
            if (strv.Length >= 2)
            {
                itemData.summondata.isSpecial = true;
                itemData.summondata.grade = strv[1];
                itemData.summondata.naturaltype = strv[2];
            }
            else if (strv.Length >= 1)
            {
                itemData.summondata.isSpecial = false;
                itemData.summondata.grade = strv[1];
                itemData.summondata.naturaltype = strv[2];
            }
            return itemData;
        }

        public bool IsSummon(a3_BagItemData itemData, Variant item)
        {
            if (item.ContainsKey("talent_type") || (itemData.tpid >= 4000 && itemData.tpid <= 4400)) return true;
            else return false;
        }

        public bool IsBaby(a3_BagItemData itemData)
        {
            if (itemData.tpid >= 4000 && itemData.tpid <= 4200) return true;
            else return false;
        }




        public int[] GetSummonTypeById(int id)
        {
            int[] str = null;
            switch (id)
            {
                case 1:
                    str = new int[3] { 0, 1, 1 };
                    break;

                case 2:
                    str = new int[3] { 0, 1, 2 };
                    break;

                case 3:
                    str = new int[3] { 0, 1, 3 };
                    break;

                case 4:
                    str = new int[3] { 0, 1, 4 };
                    break;

                case 5:
                    str = new int[3] { 0, 2, 1 };
                    break;

                case 6:
                    str = new int[3] { 0, 2, 2 };
                    break;

                case 7:
                    str = new int[3] { 0, 2, 3 };
                    break;

                case 8:
                    str = new int[3] { 0, 2, 4 };
                    break;

                case 9:
                    str = new int[3] { 0, 3, 1 };
                    break;

                case 10:
                    str = new int[3] { 0, 3, 2 };
                    break;

                case 11:
                    str = new int[3] { 0, 3, 3 };
                    break;

                case 12:
                    str = new int[3] { 0, 3, 4 };
                    break;

                case 13:
                    str = new int[3] { 1, 1, 1 };
                    break;

                case 14:
                    str = new int[3] { 1, 1, 2 };
                    break;

                case 15:
                    str = new int[3] { 1, 1, 3 };
                    break;

                case 16:
                    str = new int[3] { 1, 1, 4 };
                    break;

                case 17:
                    str = new int[3] { 1, 2, 1 };
                    break;

                case 18:
                    str = new int[3] { 1, 2, 2 };
                    break;

                case 19:
                    str = new int[3] { 1, 2, 3 };
                    break;

                case 20:
                    str = new int[3] { 1, 2, 4 };
                    break;

                case 21:
                    str = new int[3] { 1, 3, 1 };
                    break;

                case 22:
                    str = new int[3] { 1, 3, 2 };
                    break;

                case 23:
                    str = new int[3] { 1, 3, 3 };
                    break;

                case 24:
                    str = new int[3] { 1, 3, 4 };
                    break;
            }
            return str;
        }

        public string IntGradeToStr(int i)
        {
            if (i == 0) {
                return ContMgr.getCont("a3_summonModel_type0");
            }
            else if (i == 1)
            {
                return ContMgr.getCont("a3_summonModel_type1");
            }
            else if (i == 2)
            {
                return ContMgr.getCont("a3_summonModel_type2");
            }
            else
            {
                return ContMgr.getCont("a3_summonModel_type3");
            }
        }

        public string IntLvlToStr(int i)
        {
            if (i == 0) { return ContMgr.getCont("a3_summonModel_type0"); }
            else if (i == 1)
            {
                return ContMgr.getCont("a3_summonModel_type4");
            }
            else if (i == 2)
            {
                return ContMgr.getCont("a3_summonModel_type5");
            }
            else
            {
                return ContMgr.getCont("a3_summonModel_type6");
            }
        }

        public string IntNaturalToStr(int i)
        {
            if (i == 0) {
                return ContMgr.getCont("a3_summonModel_type0");
            }
            else if (i == 1) {
                return ContMgr.getCont("a3_summonModel_type7");
            }
            else if (i == 2) {
                return ContMgr.getCont("a3_summonModel_type8");
            }
            else if (i == 3) {
                return ContMgr.getCont("a3_summonModel_type9");
            }
            else {
                return ContMgr.getCont("a3_summonModel_type10");
            }
        }

        public void AddSummon(Variant item)
        {
            a3_BagItemData itemData = new a3_BagItemData();
            itemData = A3_SummonModel.getInstance().SetDataFromVariant(itemData, item);
            itemData = A3_SummonModel.getInstance().SetDataFromXML(itemData);
            itemData = A3_SummonModel.getInstance().SetBagItemData(itemData, item);
            _summons[itemData.id] = itemData;
        }

        public void RemoveSummon(int id)
        {
            if (_summons.ContainsKey((uint)id))
            {
                _summons.Remove((uint)id);
            }
        }


        public bool Checklvl(int t, uint summonId) {

            if (!GetSummons().ContainsKey(summonId))
                return true;
            a3_BagItemData data = GetSummons()[summonId];

            if (limitList.ContainsKey(t)) {
                int needlvl = limitList[t].lvl;
                if (data.summondata.level >= needlvl)
                    return true;
            }

            return false;
        }

        public void addSumCD(int id, float time)
        {
            if (process_cd == null)
            {
                process_cd = new TickItem(onUpdateCd);
                TickMgr.instance.addTick(process_cd);
            }
            sum_cds[id] = time;
        }

        TickItem process_cd;
        public void sum_doCd(int time)
        {
            sumcd = time;
            A3_SummonModel.getInstance().canFindsum = false;
            if (process_cd != null)
            {
                TickMgr.instance.removeTick(process_cd);
                process_cd = null;
            }
            process_cd = new TickItem(onUpdateCd);
            TickMgr.instance.addTick(process_cd);
        }


        public Dictionary<int, float> getSumCds() => sum_cds;

        private List<int> sum_remove_cds = new List<int>();
        private List<int> sum_reduce_cds = new List<int>();
        float sumcd = 0;
        void onUpdateCd(float s)
        {
            //if (sumcd <= 0)
            //{
            //    A3_SummonModel.getInstance().canFindsum = true;
            //    TickMgr.instance.removeTick(process_cd);
            //    process_cd = null;
            //}
            //sumcd = sumcd - s;

            //if (a3_herohead.instance)
            //{
            //    a3_herohead.instance.refresh_sumCd(sumcd);
            //}
            foreach (int type in sum_cds.Keys)
            {
                sum_reduce_cds.Add(type);
                if (sum_cds[type] <= 0)
                {
                    sum_remove_cds.Add(type);
                }
            }
            foreach (int type in sum_reduce_cds)
            {
                sum_cds[type] = sum_cds[type] - s;
            }
            foreach (int type in sum_remove_cds)
            {
                sum_cds.Remove(type);
            }
            sum_remove_cds.Clear();
            sum_reduce_cds.Clear();
            if (sum_cds.Count == 0)
            {
                TickMgr.instance.removeTick(process_cd);
                process_cd = null;
            }
        }

    }



    public class summonskill {
        public int skillid;
        public int skilllv;

    }

    public class link_data {
        public int type;
        public int per;
        public bool lock_state;
    }

    public class summonshouhun
    {
        public int soul_type;
        public int lvl;
        public int exp;
    }

    public class limitInfo {
        public int topType;
        public int lvl;
    }

    public class link_info : IComparable<link_info>
    {
        public int combpt;
        public uint id;
        public int CompareTo(link_info other)
        {
            if (combpt > other.combpt)
                return -1;
            else if (combpt < other.combpt)
                return 1;
            else return 0;
        }
    }
    

    public enum topTpye {
        shuxing = 1,
        xilian = 2,
        shouhun = 3,
        ronghe = 4,
        tunshi = 5,
        lianxie = 6,
    }
}