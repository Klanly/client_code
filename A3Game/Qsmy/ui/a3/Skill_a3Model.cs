using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame;
using System.Collections;
using UnityEngine;


namespace MuGame
{
    class Skill_a3Model : ModelBase<Skill_a3Model>
    {

        public int[] idsgroupone;
        public int[] idsgrouptwo;

        public List<int> skillid_have = new List<int>();
        public Variant skills;
        public List<int> all_skills = new List<int>();

        public Dictionary<int, skill_a3Data> skilldic = new Dictionary<int, skill_a3Data>();
        public List<skill_a3Data> skilllst = new List<skill_a3Data>();//只是为了找到第一个，每次打开界面的时候锁定第一个
        //符文~~~~~~~~~~~~~~~~~
        public Dictionary<int, runeData> runedic = new Dictionary<int, runeData>();
        public Dictionary<int, List<RuneRequire>> runeReqDic = new Dictionary<int, List<RuneRequire>>();
        SXML skillXML;
        public int openuplvl = 0;//符文开启等级
        public int openlvl = 0;
        public Skill_a3Model()
            : base()
        {
            idsgroupone = new int[4];
            idsgrouptwo = new int[4];
            skillXML = XMLMgr.instance.GetSXML("skill.skill");
            ReadXml();

            Readxml_rune();
        }
        public void initSkillList(List<Variant> arr)
        {
            skillid_have.Clear();
            foreach (Variant d in arr)
            {
                int id = d["skill_id"];
                skillid_have.Add(id);
            }
        }
        void ReadXml()
        {
            skill_a3Data.itemId = XMLMgr.instance.GetSXML("skill.skill_learn_item_id").getUint("item_id");
            List<SXML> xml = XMLMgr.instance.GetSXMLList("skill.skill");
            if (xml != null)
            {
                foreach (SXML x in xml)
                {
                    skill_a3Data skillinfo = new skill_a3Data();
                    skillinfo.skill_id = x.getInt("id");
                    skillinfo.carr = x.getInt("carr");
                    skillinfo.skill_name = x.getString("name");
                    skillinfo.action_tm = x.getFloat("action_tm");
                    skillinfo.des = x.getString("descr1");
                    skillinfo.open_zhuan = x.getInt("open_zhuan");
                    skillinfo.open_lvl = x.getInt("open_lvl");
                    skillinfo.xml = x;
                    skillinfo.item_num = x.getInt("item_num");
                    skillinfo.targetNum = x.getInt("target_num");
                    skillinfo.range = x.getInt("range");
                    skillinfo.skillType = x.getInt("skill_type");
                    skillinfo.skillType2 = x.getInt("skill_type2");
                    skillinfo.max_lvl = x.GetNodeList("skill_att").Count;
                    skillinfo.eff_last = x.getFloat("eff_last");

                    skilldic[skillinfo.skill_id] = skillinfo;
                    if (skillinfo.skill_id != 1)
                    {
                        skilllst.Add(skillinfo);
                    }
                }
            }
        }


        public void skillGroups(List<Variant> skillgroup)
        {
            for (int i = 0; i < 4; i++)
            {
                idsgroupone[i] = skillgroup[i];
                idsgrouptwo[i] = skillgroup[i + 4];
            }
        }
        public void skillinfos(int skill_id, int skill_lv)//技能信息
        {
            skilldic[skill_id].now_lv = skill_lv;
            if (!all_skills.Contains(skill_id))
            {
                all_skills.Add(skill_id);
                skill_a3.skills.Add(skill_id);
            }


        }
        //符文~~~~~~~~~~~
        void Readxml_rune()
        {
            List<SXML> lstxml = XMLMgr.instance.GetSXMLList("rune.rune");
            SXML sxml = XMLMgr.instance.GetSXML("rune.open");
            openuplvl = sxml.getInt("zhuan");
            openlvl = sxml.getInt("level");

            foreach (SXML x in lstxml)
            {
                runeData runedata = new runeData();
                runedata.id = x.getInt("id");
                runedata.type = x.getInt("type");
                runedata.name = x.getString("name");
                runedata.desc = x.getString("desc");
                runedata.carr = x.getInt("carr");
                runedata.open_zhuan = x.getInt("open_zhuan");
                runedata.open_lv = x.getInt("open_lv");
                runedic[runedata.id] = runedata;
            }
            ReadRuneRequire(lstxml);
        }
        private void ReadRuneRequire(List<SXML> listSxml = null)
        {
            if (listSxml == null)
                listSxml = XMLMgr.instance.GetSXMLList("rune.rune");
            for (int i = 0; i < listSxml.Count; i++)
            {
                List<SXML> levelInfo = listSxml[i].GetNodeList("level");
                int runeId = listSxml[i].getInt("id");
                if (PlayerModel.getInstance().profession != listSxml[i].getInt("carr")
                    && listSxml[i].getInt("carr") != 1)
                    continue;
                if (!runeReqDic.ContainsKey(runeId))
                    runeReqDic.Add(runeId, new List<RuneRequire>());
                else
                {
                    Debug.LogError(string.Format("符文Id配置重复,重复id为:{0}", runeId));
                    continue;
                }
                for (int j = 0; j < levelInfo.Count; j++)
                {
                    //List<SXML> requireLst = levelInfo[j].GetNodeList("require");
                    runeReqDic[runeId].Add
                    (
                        item: new RuneRequire
                        {
                            req_role_zhuan = levelInfo[j]?.getInt("role_zhuan") ?? 0,
                            req_role_lvl = levelInfo[j]?.getInt("role_lvl") ?? 0,
                            req_cost = levelInfo[i]?.getInt("money_cost") ?? 0
                            //req_pre_totalLv = requireLst[1]?.getInt("other_rune_total_level") ?? 0,
                            //req_pre_tarType = requireLst[1]?.getInt("pre_run_level") ?? 0,
                            //req_pre_rid = requireLst[2]?.getInt("pre_rune_id") ?? 0,
                            //req_pre_rlv = requireLst[2]?.getInt("pre_run_level") ?? 0
                        }
                    );
                }
            }
        }
        //void ReadOrRefreshXmlRuneReq()
        //{
        //    List<SXML> lstxml = XMLMgr.instance.GetSXMLList("rune.rune");
        //    foreach (var rune in runedic.Values)
        //    {
        //        if (rune.carr == PlayerModel.getInstance().profession)
        //            foreach (SXML x in lstxml)
        //            {
        //                SXML runeRequire = x.GetNode("level.require", "lv==" + rune.now_lv);
        //                RuneRequire rReq = new RuneRequire();
        //                rReq.req_open_lvl = runeRequire.getInt("role_level");
        //                rReq.req_open_zhuan = runeRequire.getInt("role_zhuan");
        //                rReq.req_pre_rid = runeRequire.getInt("pre_rune_id");
        //                rReq.req_pre_rlv = runeRequire.getInt("pre_run_level");
        //                rReq.req_pre_totalLv = runeRequire.getInt("other_rune_total_level");
        //            }
        //    }

        //}

        public Dictionary<int, runeData> Reshreinfos(int id, int lv = -1, int time = -1)
        {
            foreach (int i in runedic.Keys)
            {
                if (runedic[i].id == id)
                {
                    if (lv != -1)
                        runedic[i].now_lv = lv;
                    if (time != -1)
                        runedic[i].time = time;
                    break;
                }

            }
            return runedic;
        }
        public int GetCheckCount(int defaultValue = 10)
        {
            List<SXML> list = XMLMgr.instance.GetSXML("skill.skill_check").GetNodeList("skill_check");
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].getInt("check_zhuan") > PlayerModel.getInstance().up_lvl || list[i].getInt("check_zhuan") == PlayerModel.getInstance().up_lvl && list[i].getInt("check_lv") >= PlayerModel.getInstance().lvl)
                    return list[i].getInt("check_count");
            }
            return defaultValue;
        }
        public bool CheckSkillLevelupAvailable()
        {
            int maxCheckableCount = 0;
            List<int> skillDicKeys = new List<int>(skilldic.Keys);
            bool canUpSkill = false;
            for (int i = 0; i < skillDicKeys.Count; i++)
            {
                if (skilldic[skillDicKeys[i]].now_lv <= 0)
                    continue;
                else if (skilldic[skillDicKeys[i]].now_lv < skilldic[skillDicKeys[i]].max_lvl)
                    maxCheckableCount = Mathf.Max(maxCheckableCount, skilldic[skillDicKeys[i]].max_lvl - skilldic[skillDicKeys[i]].now_lv);
            }
            int checkCount = Mathf.Min(GetCheckCount(), maxCheckableCount);
            for (int i = 0; i < skillDicKeys.Count; i++)
            {
                int totalCostMoney = 0, totalCostItem = 0, j;
                canUpSkill = false;
                for (j = 0; j < checkCount; j++)
                {
                    if (skilldic[skillDicKeys[i]]?.xml == null
                        || skilldic[skillDicKeys[i]].xml.m_dAtttr.ContainsKey("normal_skill"))
                        break;
                    if (PlayerModel.getInstance().profession == skilldic[skillDicKeys[i]].carr)
                    {
                        if (skilldic[skillDicKeys[i]].now_lv <= 0) break;
                        if (skilldic[skillDicKeys[i]].now_lv + j < skilldic[skillDicKeys[i]].max_lvl)
                        {
                            int open_zhuan = skilldic[skillDicKeys[i]].xml.GetNode("skill_att", "skill_lv==" + (skilldic[skillDicKeys[i]].now_lv + 1 + j)).getInt("open_zhuan");
                            if (PlayerModel.getInstance().up_lvl >= open_zhuan)
                            {
                                int open_lvl = skilldic[skillDicKeys[i]].xml.GetNode("skill_att", "skill_lv==" + (skilldic[skillDicKeys[i]].now_lv + 1 + j)).getInt("open_lvl");
                                if (PlayerModel.getInstance().lvl >= open_lvl)
                                {
                                    totalCostMoney += skilldic[skillDicKeys[i]].xml.GetNode("skill_att", "skill_lv==" + (skilldic[skillDicKeys[i]].now_lv + 1 + j)).getInt("money");
                                    totalCostItem += skilldic[skillDicKeys[i]].xml.GetNode("skill_att", "skill_lv==" + (skilldic[skillDicKeys[i]].now_lv + 1 + j)).getInt("item_num");
                                    canUpSkill = a3_BagModel.getInstance().getItemNumByTpid(skill_a3Data.itemId) >= totalCostItem && PlayerModel.getInstance().money >= totalCostMoney;
                                }
                                else break;
                            }
                            else break;
                        }
                        else break;
                    }
                }
                if (j == checkCount && canUpSkill)
                    return true;
            }
            return false;
        }

        public bool CheckRuneLevelupAvailable()
        {
            if (runedic == null)
                ReadRuneRequire();
            List<int> rReqKeys = new List<int>(runeReqDic.Keys);
            for (int i = 0; i < rReqKeys.Count; i++)
            {
                int runeLv = runedic[rReqKeys[i]].now_lv;
                //符文等级是否已经是最高?
                if (runeLv == runeReqDic[rReqKeys[i]].Count)
                    continue;
                //其它符文是否满足条件(包括总符文等级和其它符文等级)?
                RuneRequire rReqInfo = runeReqDic[rReqKeys[i]][runeLv + 1];
                //if (runedic[rReqInfo.req_pre_rid].now_lv < rReqInfo.req_pre_rlv)
                //    continue;
                //int req_total_lvl = 0;
                //for (int j = 0; j < rReqKeys.Count; j++)
                //    if (runedic[rReqKeys[j]].type == rReqInfo.req_pre_tarType)
                //        req_total_lvl = req_total_lvl + runedic[rReqKeys[j]].now_lv;
                //if (req_total_lvl < rReqInfo.req_pre_totalLv)
                //    continue;
                //玩家转生和等级是否满足研究条件?
                if (PlayerModel.getInstance().lvl >= rReqInfo.req_role_lvl && PlayerModel.getInstance().up_lvl >= rReqInfo.req_role_zhuan)
                    return true;
            }
            return false;
        }
    }
    class skill_a3Data
    {
        public static uint itemId;
        public int max_lvl;
        public int skill_id;
        public int carr;                             //职业
        public string skill_name;
        public int item_num;
        public float action_tm;
        public string des;
        public int now_lv;
        public int open_zhuan;
        public int open_lvl;
        public SXML xml;
        public int targetNum;
        public int range;
        public int skillType;//0self,1target,2ground
        public int skillType2;//0攻击技能 1buff技能 2位移技能 3隐身
        public float eff_last;//buff特效持续时间
        public uint cd
        {
            get
            {
                if (now_lv == 0)
                    return xml.GetNode("skill_att", "skill_lv==1").getUint("cd") * 100;

                return xml.GetNode("skill_att", "skill_lv==" + now_lv).getUint("cd") * 100;
            }
        }

        public int mp
        {
            get { if (SelfRole.s_bStandaloneScene) return 0; return xml.GetNode("skill_att", "skill_lv==" + now_lv).getInt("mp"); }
        }

        public long endCD = 0;
        public int cdTime
        {
            get
            {
                long t = muNetCleint.instance.CurServerTimeStampMS;
                if (endCD < t)
                {
                    endCD = 0;
                    return 0;
                }

                return (int)(endCD - t);
            }
        }

        public void doCD()
        {
            long tempCd = muNetCleint.instance.CurServerTimeStampMS + cd;

            if (endCD < tempCd)
                endCD = tempCd;
        }
    }

    class runeData
    {
        public int id;
        public int type;
        public string name;
        public string desc;
        public int carr;
        public int open_zhuan;
        public int open_lv;
        public int now_lv = 0;
        public int time = -1;
    }

    class RuneRequire
    {
        public int req_role_zhuan;
        public int req_role_lvl;
        //public int req_pre_totalLv;
        //public int req_pre_tarType;
        //public int req_pre_rid;
        //public int req_pre_rlv;
        public int req_cost;
    }


}
