using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
    class a3_summoninfo : Window
    {
        private GameObject m_SelfObj;//召唤兽的avatar
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        Transform stars;

        public override void init()
        {






            getComponentByPath<Text>("playerInfo/panel_attr/info/gongji/Text").text = ContMgr.getCont("a3_summoninfo_0");
            getComponentByPath<Text>("playerInfo/panel_attr/info/fangyu/Text").text = ContMgr.getCont("a3_summoninfo_1");
            getComponentByPath<Text>("playerInfo/panel_attr/info/minjie/Text").text = ContMgr.getCont("a3_summoninfo_2");
            getComponentByPath<Text>("playerInfo/panel_attr/info/tili/Text").text = ContMgr.getCont("a3_summoninfo_3");
            getComponentByPath<Text>("playerInfo/panel_attr/info/xingyun/Text").text = ContMgr.getCont("a3_summoninfo_4");
            getComponentByPath<Text>("playerInfo/panel_attr/att/life").text = ContMgr.getCont("a3_summoninfo_5");
            getComponentByPath<Text>("playerInfo/panel_attr/att/phyatk").text = ContMgr.getCont("a3_summoninfo_6");
            getComponentByPath<Text>("playerInfo/panel_attr/att/phydef").text = ContMgr.getCont("a3_summoninfo_7");
            getComponentByPath<Text>("playerInfo/panel_attr/att/manadef").text = ContMgr.getCont("a3_summoninfo_8");
            getComponentByPath<Text>("playerInfo/panel_attr/att/hit").text = ContMgr.getCont("a3_summoninfo_9");
            getComponentByPath<Text>("playerInfo/panel_attr/att/manaatk").text = ContMgr.getCont("a3_summoninfo_10");
            getComponentByPath<Text>("playerInfo/panel_attr/att/crit").text = ContMgr.getCont("a3_summoninfo_11");
            getComponentByPath<Text>("playerInfo/panel_attr/att/dodge").text = ContMgr.getCont("a3_summoninfo_12");
            getComponentByPath<Text>("playerInfo/panel_attr/att/hitit").text = ContMgr.getCont("a3_summoninfo_13");
            getComponentByPath<Text>("playerInfo/panel_attr/att/fatal_damage").text = ContMgr.getCont("a3_summoninfo_14");
            getComponentByPath<Text>("playerInfo/panel_attr/att/reflect_crit_rate").text = ContMgr.getCont("a3_summoninfo_15");








            stars = transform.FindChild("stars");
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) =>
            {
                if (a3_ranking.isshow && a3_ranking.isshow.Toback)
                {
                    a3_ranking.isshow.Toback.SetActive(true); 
                    if (a3_ranking.isshow.showAvt != null && a3_ranking.isshow.scene_Camera != null)
                    {
                        a3_ranking.isshow.showAvt.SetActive(true);
                        a3_ranking.isshow.scene_Camera.SetActive(true);
                    }
                    a3_ranking.isshow.Toback = null;
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SUMMONINFO);
            };
            this.getEventTrigerByPath("avatar_touch").onDrag = OnDrag;
        }
        public override void onShowed()
        {

            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SUMINFO, GetInfo);
            uint tid = 0;
            if (uiData != null && uiData.Count > 0)
            {
                tid = (uint)uiData[0];
            }
            else {
                tid = SelfRole._inst.m_LockRole.m_unCID;
            }
            A3_SummonProxy.getInstance().sendgetinfo(tid);
            this.transform.SetAsLastSibling();
            GRMap.GAME_CAMERA.SetActive(false);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            
        }
        public override void onClosed()
        {
            disposeAvatar();
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_SUMINFO, GetInfo);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            if (a3_ranking.isshow && a3_ranking.isshow.Toback) {
                InterfaceMgr.getInstance().close(InterfaceMgr .A3_RANKING);
            }

        }
        int tpid = 0;
        void GetInfo(GameEvent e)
        {
            Variant data = e.data;
            tpid = data["summon"]["tpid"];
           // SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);         
            this.transform.FindChild("fighting/value").GetComponent<Text>().text = data["summon"]["combpt"];

            //this.transform.FindChild("playerInfo/panel_attr/name").GetComponent<Text>().text = xml.getString("item_name");
            this.transform.FindChild("playerInfo/panel_attr/lv").GetComponent<Text>().text = "LV "+ data["summon"]["level"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/longlife/Text").GetComponent<Text>().text = data["summon"]["life"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/luck/Text").GetComponent<Text>().text = data["summon"]["luckly"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/att/Text").GetComponent<Text>().text = data["summon"]["att"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/def/Text").GetComponent<Text>().text = data["summon"]["def"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/agi/Text").GetComponent<Text>().text = data["summon"]["agi"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/con/Text").GetComponent<Text>().text = data["summon"]["con"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/life/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["max_hp"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/phyatk/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["min_attack"]+"-" + data["summon"]["battleAttrs"]["max_attack"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/phydef/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["physics_def"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/manadef/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["magic_def"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/hit/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["physics_dmg_red"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/manaatk/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["magic_dmg_red"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/crit/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["fatal_att"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/dodge/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["dodge"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/fatal_damage/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["fatal_damage"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/hitit/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["hit"];
            //this.transform.FindChild("playerInfo/panel_attr/right/up/reflect_crit_rate/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["fatal_dodge"];

            SXML sumXml = XMLMgr.instance.GetSXML("callbeast");
            SXML Sumxml = sumXml.GetNode("callbeast", "id==" + tpid);
            int star = data["summon"]["talent"];
            SXML attxml = Sumxml.GetNode("star", "star_sum==" + star);
            int tpid_m = attxml.getInt("info_itm");
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid_m);
            this.transform.FindChild("playerInfo/panel_attr/name").GetComponent<Text>().text = xml.getString("item_name");
            this.transform.FindChild("playerInfo/panel_attr/info/gongji/value").GetComponent<Text>().text = data["summon"]["att"] + "/" + attxml.GetNode("att").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/fangyu/value").GetComponent<Text>().text = data["summon"]["def"] + "/" + attxml.GetNode("def").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/minjie/value").GetComponent<Text>().text = data["summon"]["agi"] + "/" + attxml.GetNode("agi").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/tili/value").GetComponent<Text>().text = data["summon"]["con"] + "/" + attxml.GetNode("con").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/xingyun/value").GetComponent<Text>().text = data["summon"]["luckly"] + "/" + 100;

            this.transform.FindChild("playerInfo/panel_attr/info/gongji/slider").GetComponent<Image>().fillAmount = (float)data["summon"]["att"] / (float)attxml.GetNode("att").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/fangyu/slider").GetComponent<Image>().fillAmount = (float)data["summon"]["def"] / (float)attxml.GetNode("def").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/minjie/slider").GetComponent<Image>().fillAmount = (float)data["summon"]["agi"] / (float)attxml.GetNode("agi").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/tili/slider").GetComponent<Image>().fillAmount = (float)data["summon"]["con"] / (float)attxml.GetNode("con").getInt("reset_max");
            this.transform.FindChild("playerInfo/panel_attr/info/xingyun/slider").GetComponent<Image>().fillAmount = (float)data["summon"]["luckly"] / (float)100;

            this.transform.FindChild("playerInfo/panel_attr/att/life/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["max_hp"];
            this.transform.FindChild("playerInfo/panel_attr/att/phyatk/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["min_attack"] + "-" + data["summon"]["battleAttrs"]["max_attack"];
            this.transform.FindChild("playerInfo/panel_attr/att/phydef/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["physics_def"];
            this.transform.FindChild("playerInfo/panel_attr/att/manadef/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["magic_def"];
            this.transform.FindChild("playerInfo/panel_attr/att/hit/Text").GetComponent<Text>().text = (float)data["summon"]["battleAttrs"]["physics_dmg_red"] / 10 + "%";
            this.transform.FindChild("playerInfo/panel_attr/att/manaatk/Text").GetComponent<Text>().text = (float)data["summon"]["battleAttrs"]["magic_dmg_red"] / 10 + "%";
            this.transform.FindChild("playerInfo/panel_attr/att/crit/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["fatal_att"];
            this.transform.FindChild("playerInfo/panel_attr/att/dodge/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["dodge"];
            this.transform.FindChild("playerInfo/panel_attr/att/fatal_damage/Text").GetComponent<Text>().text = (float)data["summon"]["battleAttrs"]["fatal_damage"] / 10 + "%";
            this.transform.FindChild("playerInfo/panel_attr/att/hitit/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["hit"];
            this.transform.FindChild("playerInfo/panel_attr/att/reflect_crit_rate/Text").GetComponent<Text>().text = data["summon"]["battleAttrs"]["fatal_dodge"];



            createAvatar();
            setStar(data["summon"]["talent"]);
            //Dictionary<int, int> skills = new Dictionary<int, int>();
            //Variant sks = data["summon"]["skills"];
            //for (int i = 0; i < sks.Count; i++)
            //{
            //    skills[sks[i]["index"]] = sks[i]["skill_id"];
            //}
            //setSkills(data["summon"]["skill_num"], skills);
            if (data["summon"].ContainsKey("skills")) {
                Variant sks = data["summon"]["skills"];
                Dictionary<int, summonskill> skillDic = new Dictionary<int, summonskill>();
                for (int i = 0; i < sks.Count; i++)
                {
                    summonskill skill = new summonskill();
                    skill.skillid = sks[i]["skill_id"];
                    skill.skilllv = sks[i]["skill_lvl"];
                    skillDic[sks[i]["skill_id"]] = skill;
                }
                var SkillCon = this.transform.FindChild("skills");
                for (int i = 0; i < SkillCon.childCount; i++)
                {
                    SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                    SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
                }
                int idner = 1;
                foreach (summonskill skill in skillDic.Values)
                {
                    Transform skillCell = SkillCon.FindChild(idner.ToString());
                    skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                    skillCell.FindChild("lock").gameObject.SetActive(false);
                    SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                    skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                    idner++;
                }
            }





        }

        void setStar( int starlvl )
        {
            for (int i = 0; i < 5; i++)
            {
                stars.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }

            for (int j = 0; j < starlvl;j++)
            {
                stars.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
        }


        void setSkills(int skillnum, Dictionary<int, int> skills)
        {
          
        }
        public void createAvatar()
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
            scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
            scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
            foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
            {
                if (tran.gameObject.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }

            int objid = 0;
            SXML itemsXMl = XMLMgr.instance.GetSXML("callbeast");
            var xml = itemsXMl.GetNode("callbeast", "id==" + tpid);
            var mid = xml.getInt("mid");
            var mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            objid = mxml.getInt("obj");

            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            m_SelfObj = GameObject.Instantiate(obj_prefab ,new Vector3(-77.38f, -0.46f, 14.92f), new Quaternion(0, 180, 0, 0)) as GameObject;
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
            }
            Transform cur_model = m_SelfObj.transform.FindChild("model");
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 90 - mxml.getInt("smshow_face"));
            cur_model.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;

        }

        public void disposeAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }

    }
}
