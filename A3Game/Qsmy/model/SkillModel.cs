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
    class SkillModel : ModelBase<SkillModel>
    {
        public Dictionary<int, Skillnextinfo> dic_n_info = new Dictionary<int, Skillnextinfo>();

        public static uint EVENT_INIT_INFO = 0;
        public static uint EVENT_CHANGE_INFO = 1;
        

        protected Variant _skillArr = null;//技能数组
        protected Variant _clientConf = null;

        public Dictionary<uint, SkillXmlData> skillXMl;
        public Dictionary<uint, SkillHitedXml> skillHitXmls;
        public Dictionary<uint, SkillData> skillDatas;
        public List<SkillData> lSkill;
        public SkillModel()
            : base()
        {
            initXml();
            getnext_info();
            //   GeneralProxy.getInstance().sendGetClientConfig();
        }

        public void initXml()
        {
            skillHitXmls = new Dictionary<uint, SkillHitedXml>();
            SkillHitedXml skillhitxml;
            SXML s_xml = XMLMgr.instance.GetSXML("skill.hited", null);
            if (s_xml == null)
            {
                debug.Log("config [skill.hited] is missing");
                return;
            }

            do
            {
                skillhitxml = new SkillHitedXml();
                string[] arr = s_xml.getString("rim_Color").Split(new string[] { "," }, StringSplitOptions.None);
                Color c1;  Color c2;
                if(arr.Length==3)
                   c1  = new Color(float.Parse(arr[0]),float.Parse(arr[1]),float.Parse(arr[2] ));
                else continue;
                arr = s_xml.getString("main_Color").Split(new string[] { "," }, StringSplitOptions.None);
                 if(arr.Length==3)
                   c2= new Color(float.Parse(arr[0]),float.Parse(arr[1]),float.Parse(arr[2] ));
                else continue;
                skillhitxml.rimColor = c1;
                skillhitxml.mainColor = c2;
                skillHitXmls[s_xml.getUint("id")] = skillhitxml;
            } while (s_xml.nextOne());

            skillXMl = new Dictionary<uint, SkillXmlData>();
             s_xml = XMLMgr.instance.GetSXML("skill.skill", null);

            SkillXmlData tempXml = null;
            SkillLvXmlData tempLvXml;
            SXML tempxml;
            do
            {
                uint id = s_xml.getUint("id");
                tempXml = new SkillXmlData();
                tempXml.id = id;
                tempXml.skill_name = s_xml.getString("skill_name");
                tempXml.eff = s_xml.getString("eff");
                tempXml.eff_female = s_xml.getString("eff_female");
                tempXml.target_type = s_xml.getInt("skill_targettype");
              

                tempxml = s_xml.GetNode("jump", null);
                tempXml.useJump = false;
             
                tempXml.hitfall = s_xml.getInt("hitfall")==1;
                if (tempxml != null)
                {
                    tempXml.useJump = true;
                    tempXml.jump_canying = tempxml.getString("canying");
                }

                uint hidedid = s_xml.getUint("hide_id");
                if (skillHitXmls.ContainsKey(hidedid))
                {
                    tempXml.hitxml = skillHitXmls[hidedid];
                }


                tempXml.lv = new Dictionary<uint, SkillLvXmlData>();
                tempxml = s_xml.GetNode("Level", null);

                if (tempxml != null)
                {
                    do
                    {
                        tempLvXml = new SkillLvXmlData();
                        uint lv = tempxml.getUint("level");
                        tempLvXml.range = (tempxml.getInt("range") + (int)GameConstant.GEZI)/GameConstant.PIXEL_TRANS_UNITYPOS;
                        tempLvXml.range_gezi = (int)tempLvXml.range / (int)GameConstant.GEZI;
                        tempLvXml.cd = tempxml.getUint("cd_time");
                        tempLvXml.desc = tempxml.getString("description");
                        tempLvXml.pvp_param = tempxml.getInt("pvp_param");
                        tempLvXml.needMoney = tempxml.getInt("need_money");
                        tempLvXml.attr = tempxml.getInt("skill_attribute");
                        tempLvXml.needExp = tempxml.getInt("need_exp");

                        tempXml.lv[lv] = tempLvXml;
                    } while (tempxml.nextOne());
                }

                skillXMl[id] = tempXml;
            } while (s_xml.nextOne());
        }
        public void changeSkillList(Variant data)
        {
            uint skillid = data["skill_id"];
            uint skilllv = data["skill_level"];
            if (skillid > 1009)
            {
                return;
            }
            SkillData skillOne = getSkillData(skillid);
            if (skillOne == null)
                skillOne = new SkillData();

            //int sex = PlayerModel.getInstance().sex;
            skillOne.id = skillid;
            skillOne.lv = skilllv;
            skillOne.skill_data_xml = skillXMl[skillid];
            //skillOne.eff = sex == 0 ? skillOne.xml.eff : skillOne.xml.eff_female;
            skillOne.range_gezi = skillOne.skill_data_xml.lv[skillOne.lv].range_gezi;
            skillOne.range = skillOne.skill_data_xml.lv[skillOne.lv].range ;
            skillOne.maxCd = skillOne.skill_data_xml.lv[skillOne.lv].cd * 100;

            if (!skillDatas.ContainsKey(skillid))
            {
                skillDatas[skillid]= skillOne;
                lSkill.Add(skillOne);
            }

            dispatchEvent(GameEvent.Create(EVENT_CHANGE_INFO, this, null));
        }

        public void initSkillList(List<Variant> arr)
        {
            if (skillDatas == null)
                skillDatas = new Dictionary<uint, SkillData>();
            if (lSkill == null)
                lSkill = new List<SkillData>();
            //int sex = PlayerModel.getInstance().sex;
            SkillData data;


            //lucisa
            //for(uint i=1001;i<1008;i++)
            //{
            //    data = new SkillData();
            //    uint id =i;

            //    data.id = id;
            //    data.lv = 1;
            //    data.xml = skillXMl[id];
            //    data.eff = sex == 0 ? data.xml.eff : data.xml.eff_female;

            //    data.range = data.xml.lv[data.lv].range + GameConstant.DEF_ATTACK_RANGE;
            //    data.maxCd = data.xml.lv[data.lv].cd * 100;
            //    skillDatas[data.id] = data;
            //    lSkill.Add(data);
            //}



            //lucisa



            foreach (Variant d in arr)
            {
                if (d["skill_id"] < 1010)
                {
                    data = new SkillData();
                    uint id = d["skill_id"];

                    data.id = id;
                    data.lv = d["skill_level"];

                    if (skillXMl == null) continue;

                    if (skillXMl.ContainsKey(id))
                    {
                        data.skill_data_xml = skillXMl[id];

                        //data.eff = sex == 0 ? data.xml.eff : data.xml.eff_female;
                        data.range_gezi = (data.skill_data_xml.lv[data.lv].range_gezi);
                        data.range = (data.skill_data_xml.lv[data.lv].range) ;
                        data.maxCd = data.skill_data_xml.lv[data.lv].cd * 100;
                        skillDatas[data.id] = data;
                        lSkill.Add(data);
                    }
                }

                //if (id == 1001)
                //{
                //    for (uint i = 1002; i < 1004; i++)
                //    {
                //        data = new SkillData();
                //        data.id = i;
                //        data.lv = d["skill_level"];
                //        data.xml = skillXMl[i];
                //        data.eff = sex == 0 ? data.xml.eff : data.xml.eff_female;

                //        data.range = data.xml.lv[data.lv].range + GameConstant.DEF_ATTACK_RANGE;
                //        data.maxCd = data.xml.lv[data.lv].cd * 100;
                //        skillDatas[data.id] = data;
                //        lSkill.Add(data);
                //    }
                //}
            }

            //if (!skillDatas.ContainsKey(1008))
            //{
            //    data = new SkillData();

            //    data.id = 1008;
            //    data.lv = 1;
            //    data.xml = skillXMl[1008];
            //    data.eff = sex == 0 ? data.xml.eff : data.xml.eff_female;

            //    data.range = data.xml.lv[data.lv].range;
            //    data.range_gezi = data.xml.lv[data.lv].range / (int)GameConstant.GEZI;
            //    data.maxCd = data.xml.lv[data.lv].cd * 100;
            //    skillDatas[data.id] = data;
            //    lSkill.Add(data);
            //}
            //if (!skillDatas.ContainsKey(1009))
            //{
            //    data = new SkillData();

            //    data.id = 1009;
            //    data.lv = 1;
            //    data.xml = skillXMl[1009];
            //    data.eff = sex == 0 ? data.xml.eff : data.xml.eff_female;

            //    data.range = data.xml.lv[data.lv].range;
            //    data.range_gezi = data.xml.lv[data.lv].range / (int)GameConstant.GEZI;
            //    data.maxCd = data.xml.lv[data.lv].cd * 100;
            //    skillDatas[data.id] = data;
            //    lSkill.Add(data);
            //}


            dispatchEvent(GameEvent.Create(EVENT_INIT_INFO, this, null));
        }

        public SkillData getSkillData(uint id)
        {
            if (skillDatas.ContainsKey(id))
                return skillDatas[id];
            return null;
        }

        public SkillXmlData getSkillXml(uint id)
        {
            if (skillXMl.ContainsKey(id))
                return skillXMl[id];
            return skillXMl[1001];
        }
        static public Color getskill_mcolor(int id)
        {
            
            string sr = XMLMgr.instance.GetSXML("skill.skill", "id=="+id).getString("m_Color_Main");
            if (sr == null)
                return Color.gray;
            else
            {
                string[] i = sr.Split(',');          
                return new Color(float.Parse(i[0]), float.Parse(i[1]), float.Parse(i[2]));
            }

        }
        static public Color getskill_rcolor(int id)
        {
            string sr = XMLMgr.instance.GetSXML("skill.skill", "id==" + id).getString("m_Color_Rim");
            if (sr == null)
                return Color.red;
            else
            {
                string[] i = sr.Split(',');
                return new Color(float.Parse(i[0]), float.Parse(i[1]), float.Parse(i[2]));
            }
        }


        public Dictionary<int, Skillnextinfo> getnext_info()
        {
            dic_n_info.Clear();
            List<SXML> xml = XMLMgr.instance.GetSXMLList("skill.skill");

            for (int i = 0; i < xml.Count; i++)
            {
                Skillnextinfo n_info = new Skillnextinfo();

                n_info.id = xml[i].getInt("id");
                n_info.lst = new Dictionary<int, MuGame.Skillattinfo>();
                List<SXML> xmls = xml[i].GetNodeList("skill_att");
                for (int j = 0; j < xmls.Count; j++)
                {
                    Skillattinfo si=new Skillattinfo();
                    si.skill_lv= xmls[j].getInt("skill_lv");
                    si.open_lvl = xmls[j].getInt("open_lvl");
                    si.open_zhuan= xmls[j].getInt("open_zhuan");
                    n_info.lst[si.skill_lv] = si;
                }
                dic_n_info[n_info.id] = n_info;

            }
            if (dic_n_info == null)
                return null;
            else
                return dic_n_info;
        }
    }






    public class SkillData
    {
       
        public uint lv;
        public uint id;
        public uint maxCd;
        public int spt;
        public float range;
        public int range_gezi;
        public String eff;
        public SkillXmlData skill_data_xml = null;
    }

    public class SkillXmlData
    {
        public bool hitfall;
        public String eff_female;
        public uint id;
        public String skill_name;
        public String eff;
        public Dictionary<uint, SkillLvXmlData> lv;
        public int target_type;
        public bool useJump;
        public string jump_canying;
        public int targetNum;

        public SkillHitedXml hitxml;
    }



    public class SkillLvXmlData
    {
        public float range;
        public int range_gezi;
        public uint cd;
        public string desc;
        public int needMoney;
        public int needExp;
        public int attr;
        public int pvp_param;

        public int jump_range;
    }

     public class SkillHitedXml
    {
        public Color rimColor;
       public Color mainColor;
    }


    public class Skillnextinfo
    {
        public int id;
        public Dictionary<int, Skillattinfo> lst;

    }
    public class Skillattinfo
    {
        public int skill_lv;
        public int open_zhuan;
        public int open_lvl;
    }
}
