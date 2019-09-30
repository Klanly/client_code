using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using Cross;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class a3_summon_ronghe : BaseSummon
    {
        public a3_summon_ronghe(Transform trans, string name) : base(trans, name)
        {
            init();
        }

        uint fuSumID = 0;
        void init()
        {

            tranObj.transform.FindChild("zhu/bg/title").GetComponent<Text>().text = ContMgr.getCont("a3_summon_ronghe_0");
            tranObj.transform.FindChild("fu/bg/title").GetComponent<Text>().text = ContMgr.getCont("a3_summon_ronghe_1");
            tranObj.transform.FindChild("todo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_ronghe_2");


            new BaseButton(tranObj.transform.FindChild("fu/Button")).onClick = (GameObject go) => 
            {
                if (getSummonWin() == null) return;
                GameObject plan = getSummonWin().GetSmallWin("uilayer_tofusum_summon");
                plan.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("tofusum_summon_0");
                plan.SetActive(true);
                setFuSumlist(plan);
            };

            new BaseButton(tranObj.transform.FindChild("todo")).onClick = (GameObject go) =>
            {
                if (getSummonWin() == null) return;
                if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(fuSumID))
                {

                    flytxt.instance.fly(ContMgr.getCont("summon_selete_ronghe"));
                    return;
                }
                GameObject toSrue = getSummonWin().GetSmallWin("uilayer_ToSure_summon");
                toSrue.SetActive(true);
                setSure(toSrue);
            };
        }
        public override void onShowed() {
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_RONGHE, onRonghe);

            SetCurSumInfo();
            fuSumID = 0;

            closeWin("uilayer_tofusum_summon");
            tranObj.transform.FindChild("fu/info").gameObject.SetActive(false);
            tranObj.transform.FindChild("fu/Button").gameObject.SetActive(true);
            setNullFuSkill();
            setGailvAndNeedMoney(true);
        }
        public override void onClose(){
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_RONGHE, onRonghe);
        }
        public override void onAddNewSmallWin(string name)
        {
            switch (name)
            {
                case "uilayer_tofusum_summon":
                    GameObject fusum = getSummonWin()?.GetSmallWin(name);
                    fusum.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("tofusum_summon_0");
                    new BaseButton(fusum.transform.FindChild("tach")).onClick = (GameObject go) => {
                        fusum.SetActive(false);
                    };
                    break;
                case "uilayer_skilltip_summon":
                    GameObject skilltip = getSummonWin()?.GetSmallWin(name);
                    skilltip.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("skilltip_summon_0");
                    skilltip.transform.FindChild("black/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
                    new BaseButton(skilltip.transform.FindChild("tach")).onClick = 
                        new BaseButton(skilltip.transform.FindChild("black")).onClick = 
                        (GameObject go) => {
                        skilltip.SetActive(false);
                    };
                    break;
                case "uilayer_ToSure_summon":
                    GameObject tosure = getSummonWin()?.GetSmallWin(name);
                    tosure.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_0");
                    tosure.transform.FindChild("txt").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_0");
                    tosure.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_2");
                    tosure.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
                    new BaseButton(tosure.transform.FindChild("tach")).onClick =
                    new BaseButton(tosure.transform.FindChild("no")).onClick =
                    (GameObject go) => {
                        tosure.SetActive(false);
                        if (getSummonWin().avatorobj)
                        {
                            getSummonWin().avatorobj.SetActive(true);
                        }
                    };
                    break;
            }
        }

        void setSure(GameObject pre)
        {
            new BaseButton(pre.transform.FindChild("yes")).onClick = (GameObject go) => 
            {
                if (A3_SummonModel.getInstance().GetSummons().ContainsKey(fuSumID))
                {
                    A3_SummonProxy.getInstance().sendRonghe(CurSummonID, fuSumID);
                    pre.SetActive(false);
                }
            };
            pre.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("summon_sure_top_1");
            pre.transform.FindChild("txt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("summon_sure_text_1")); 
        }

        public override void onCurSummonIdChange() {
            base.onCurSummonIdChange();
            SetCurSumInfo();
            if (CurSummonID == fuSumID) {
                tranObj.transform.FindChild("fu/info").gameObject.SetActive(false);
                tranObj.transform.FindChild("fu/Button").gameObject.SetActive(true);
                setNullFuSkill();
                fuSumID = 0;
            }
        }

        void SetCurSumInfo()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            tranObj.transform.FindChild("zhu/lv").GetComponent<Text>().text = data.summondata .level.ToString ();
            tranObj.transform.FindChild("zhu/name").GetComponent<Text>().text = data.summondata.name ;
            tranObj.transform.FindChild("zhu/zhanlimun").GetComponent<Text>().text = data.summondata.power.ToString ();
            for (int m = 0; m < tranObj.transform.FindChild("zhu/icon").childCount; m++)
            {
                GameObject.Destroy(tranObj.transform.FindChild("zhu/icon").GetChild(m).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data.confdata);
            icon.transform.SetParent(tranObj.transform.FindChild("zhu/icon"), false);
            Transform SkillCon = tranObj.transform.FindChild("zhu/skills");
            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
            }
            int idner = 1;
            foreach (summonskill skill in data.summondata.skills.Values)
            {
                Transform skillCell = SkillCon.FindChild(idner.ToString());
                skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                skillCell.FindChild("lock").gameObject.SetActive(false);
                SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                new BaseButton(skillCell).onClick = (GameObject go) => {
                    if (getSummonWin() == null) return;
                    if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                    GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");
                    tip.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("skilltip_summon_0");
                    tip.transform.FindChild("black/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
                    tip.SetActive(true);
                    setskilltip(tip, skill);
                };
                idner++;
            }
        }

        void setFuSumlist(GameObject obj)
        {
            Transform Con = obj.transform.FindChild("summonlist/summons/scroll/content");
            GameObject item = obj.transform.FindChild("summonlist/summons/scroll/0").gameObject;
            obj.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("selsctfusummon");
            Dictionary<uint, a3_BagItemData> sT = A3_SummonModel.getInstance ().GetSummons(true);
            for (int i = 0;i< Con.transform.childCount;i++) {
                GameObject.Destroy(Con.transform.GetChild (i).gameObject);
            }

            foreach (a3_BagItemData it in sT.Values)
            {
                if ((uint)it.summondata.id == A3_SummonModel.getInstance().nowShowAttackID)
                    continue;
                if ((uint)it.summondata.id == CurSummonID) continue;
                GameObject clon = GameObject.Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(Con, false);

                clon.transform.FindChild("name").GetComponent<Text>().text = it.summondata.name;
                clon.transform.FindChild("lv").GetComponent<Text>().text = it.summondata.level .ToString ();
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(it.confdata);
                icon.transform.SetParent(clon.transform.FindChild("icon"),false);
                Transform starRoot = clon.transform.FindChild("stars");
                for (int i = 0; i < 5; i++)
                {
                    starRoot.GetChild(i).FindChild("b").gameObject.SetActive(false);
                }

                for (int j = 0; j < it.summondata.star; j++)
                {
                    starRoot.GetChild(j).FindChild("b").gameObject.SetActive(true);
                }
                new BaseButton(clon.transform).onClick = (GameObject go) => {
                    setFuID( it.id);
                    closeWin("uilayer_tofusum_summon");
                };
            }
            Con.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        void setFuID(uint Value)
        {
            if (Value != fuSumID)
            {
                fuSumID = Value;
                SetFuSumInfo();
            }
        }

        void SetFuSumInfo()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey (fuSumID))
            {
                tranObj.transform.FindChild("fu/info").gameObject.SetActive(false);
                tranObj.transform.FindChild("fu/Button").gameObject.SetActive(true);
                setGailvAndNeedMoney(true);
                return;
            }
            tranObj.transform.FindChild("fu/info").gameObject.SetActive(true);
            tranObj.transform.FindChild("fu/Button").gameObject.SetActive(false);
            a3_BagItemData fusum = A3_SummonModel.getInstance().GetSummons()[fuSumID];
            tranObj.transform.FindChild("fu/info/lv").GetComponent<Text>().text = fusum.summondata.level.ToString();
            tranObj.transform.FindChild("fu/info/name").GetComponent<Text>().text = fusum.summondata.name;
            tranObj.transform.FindChild("fu/info/zhanlimun").GetComponent<Text>().text = fusum.summondata.power.ToString ();
            Transform SkillCon = tranObj.transform.FindChild("fu/skills");
            for (int m = 0;m<tranObj.transform.FindChild ("fu/info/icon").childCount;m++)
            {
                GameObject.Destroy(tranObj.transform.FindChild("fu/info/icon").GetChild (m).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(fusum.confdata);
            icon.transform.SetParent(tranObj.transform.FindChild("fu/info/icon"), false);
            new BaseButton(icon.transform).onClick = (GameObject go) => {
                if (getSummonWin() == null) return;
                GameObject plan = getSummonWin().GetSmallWin("uilayer_tofusum_summon");
           
                plan.SetActive(true);
                setFuSumlist(plan);
            };

            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("mark").gameObject.SetActive(false);
            }
            int idner = 1;
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            foreach (summonskill skill in fusum.summondata.skills.Values)
            {
                Transform skillCell = SkillCon.FindChild(idner.ToString());
                skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                if (data.summondata.skills.Keys.Contains (skill.skillid)) {
                    skillCell.FindChild("mark").gameObject.SetActive(true);
                }
                new BaseButton(skillCell).onClick = (GameObject go) => {
                    if (getSummonWin() == null) return;
                    if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                    GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");

                    tip.SetActive(true);
                    setskilltip(tip, skill);
                };
                idner++;
            }
            setGailvAndNeedMoney();
        }

        void setskilltip(GameObject tip, summonskill skill)
        {
            SXML skillXml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
            tip.transform.FindChild("name").GetComponent<Text>().text = skillXml.getString("name") + skill.skilllv + ContMgr.getCont("ji");
            SXML curlvl = skillXml.GetNode("skill_att", "skill_lv==" + skill.skilllv);
            tip.transform.FindChild("des").GetComponent<Text>().text = curlvl.getString("descr2");
        }

        void setNullFuSkill()
        {
            Transform SkillCon = tranObj.transform.FindChild("fu/skills");

            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("mark").gameObject.SetActive(false);
            }
        }

        void onRonghe(GameEvent evt)
        {
            Variant data = evt.data;
            if (data.ContainsKey("summon"))
            {
                SetCurSumInfo();
            }
            tranObj.transform.FindChild("fu/info").gameObject.SetActive(false);
            tranObj.transform.FindChild("fu/Button").gameObject.SetActive(true);
            setNullFuSkill();
            setGailvAndNeedMoney(true);
            refreSumlist((int)CurSummonID);
        }

        void setGailvAndNeedMoney(bool nullsum =false)
        {
            Text gclv = tranObj.transform.FindChild("cglv").GetComponent<Text>();

            Text money = tranObj.transform.FindChild("todo/money").GetComponent<Text>();
            if (nullsum)
            {
                gclv.text = "(" + ContMgr.getCont("chenggonglv")+"0%"+ ")";
                money.text = 0.ToString();
            }
            else
            {
                if (A3_SummonModel.getInstance().GetSummons().ContainsKey(fuSumID) && A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                {
                    a3_BagItemData curdata = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                    a3_BagItemData fudata = A3_SummonModel.getInstance().GetSummons()[fuSumID];
                    int needmoney = 0;
                    needmoney = sumXml.GetNode("combin_cost", "star==" + curdata.summondata.star).getInt("money");
                    money.text = needmoney.ToString();

                    int rate = sumXml.GetNode("success_rate").getInt("luck_add_mull");
                    int cg1 = (int)((rate * curdata.summondata.luck) / 100);
                    int cg2 = 0;
                    List<int> skill = new List<int>();
                    foreach (int id in curdata.summondata.skills .Keys)
                    {
                        if (!skill.Contains(id))
                        {
                            skill.Add(id);
                        }
                    }
                    foreach (int id in fudata.summondata.skills.Keys)
                    {
                        if (!skill.Contains(id))
                        {
                            skill.Add(id);
                        }
                    }
                    SXML ra = sumXml.GetNode("success_rate");
                    List<SXML> skilllixt = ra.GetNodeList("skill_count");

                    int _count = skill.Count;
                    if (skill.Count >= skilllixt.Count) {
                        _count = skilllixt.Count;
                    }

                    cg2 = ra.GetNode("skill_count", "count==" + _count).getInt("rate");
                    gclv.text = "(" + ContMgr.getCont("chenggonglv") + (cg1 + cg2) + "%" + ")"; 
                }
            }
        }


    }
}
