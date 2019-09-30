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
    class a3_summon_tunshi : BaseSummon
    {
        uint FuSum_Id = 0;
        Transform SkillCon;
        GameObject  tishi;
        GameObject helpcon;
        public a3_summon_tunshi(Transform trans, string name) : base(trans, name)
        {
            init();
        }

        void init()
        {

            tranObj.transform.FindChild("info_/Image/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_0");
            tranObj.transform.FindChild("info_/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_1");
            tranObj.transform.FindChild("info_/lvCon/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_2");
            tranObj.transform.FindChild("info_/shouhunCon/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_3");
            tranObj.transform.FindChild("info_/skillCon/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_4");
            tranObj.transform.FindChild("info_yulan/Image/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_5");
            tranObj.transform.FindChild("info_yulan/lvl").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_6");
            tranObj.transform.FindChild("info_yulan/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_3");
            tranObj.transform.FindChild("Button/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_7");
            tranObj.transform.FindChild("todo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_8");
            tranObj.transform.FindChild("info_yulan/Text1").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_9");
            tranObj.transform.FindChild("help/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_10");//help提示
            tranObj.transform.FindChild("help/close/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tunshi_11");//知道了
            tishi = tranObj.transform.FindChild("info_yulan/Text1").gameObject;
            helpcon = tranObj.transform.FindChild("help").gameObject;
            new BaseButton(tranObj.transform.FindChild("help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
                getSummonWin().avatorobj.SetActive(false);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
                getSummonWin().avatorobj.SetActive(true);
            };
            SkillCon = tranObj.transform.FindChild("info_yulan/skills");
            getEventTrigerByPath("tach").onDrag = OnDrag;
            new BaseButton(tranObj.transform.FindChild("Button")).onClick = (GameObject go) =>
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
                if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(FuSum_Id)) {

                    flytxt.instance.fly(ContMgr.getCont("summon_selete_sum"));
                    return;
                }
                GameObject toSrue = getSummonWin().GetSmallWin("uilayer_ToSure_summon");

                toSrue.SetActive(true);
                setSure(toSrue);
            };
        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (getSummonWin().avatorobj != null)
            {
                getSummonWin().avatorobj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        public override void onShowed() {
            tishi.SetActive(false);
            helpcon.SetActive(false);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_TUNSHI, onTunshi);
            showCurSummon();
            setAvator();
        }
        public override void onClose()
        {
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_TUNSHI, onTunshi);
            SetDispose();
            FuSum_Id = 0;
        }


        void setSure(GameObject pre)
        {
            if (getSummonWin().avatorobj != null)
            {
                getSummonWin().avatorobj.SetActive(false);
            }
            new BaseButton(pre.transform.FindChild("yes")).onClick = (GameObject go) =>
            {
                if (A3_SummonModel.getInstance().GetSummons().ContainsKey(FuSum_Id))
                {
                    A3_SummonProxy.getInstance().sendtunshi(CurSummonID, FuSum_Id);
                    pre.SetActive(false);
                    if (getSummonWin().avatorobj != null)
                    {
                        getSummonWin().avatorobj.SetActive(true);
                    }
                }
            };
            pre.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("summon_sure_top_2");
            pre.transform.FindChild("txt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("summon_sure_text_2"));
        }
        void showCurSummon()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            if (FuSum_Id <= 0 || !A3_SummonModel.getInstance().GetSummons().ContainsKey(FuSum_Id))
            {
                tranObj.transform.FindChild("info_xiaohao").gameObject.SetActive(false);
                tranObj.transform.FindChild("Button").gameObject.SetActive(true);
                tranObj.transform.FindChild("info_yulan/lvl/Text").GetComponent<Text>().text = data.summondata.level.ToString();
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider_add").gameObject.SetActive(false);
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider").gameObject.SetActive(true);
                tranObj.transform.FindChild("info_yulan/name").GetComponent<Text>().text = data.summondata.name;
                var xml = A3_SummonModel.getInstance().GetAttributeXml(data.summondata.level);
                int exp_max = xml.getInt("exp");
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider").GetComponent<Image>().fillAmount = (float)data.summondata.currentexp / (float)exp_max;
                tranObj.transform.FindChild("info_yulan/lvl_bar/value").GetComponent<Text>().text = data.summondata.currentexp + "/" + exp_max;
                foreach (int type in data.summondata.shouhun.Keys)
                {
                    Transform obj = tranObj.transform.FindChild("info_yulan/shouhun/" + type);
                    obj.transform.FindChild("up").gameObject.SetActive(false);
                    obj.transform.FindChild("Text").GetComponent<Text>().text = data.summondata.shouhun[type].lvl.ToString();
                }
                setskill(data);
            }
            else if (A3_SummonModel.getInstance().GetSummons().ContainsKey(FuSum_Id))
            {
                tranObj.transform.FindChild("info_xiaohao").gameObject.SetActive(true);
                tranObj.transform.FindChild("Button").gameObject.SetActive(false);
                a3_BagItemData Fudata = A3_SummonModel.getInstance().GetSummons()[FuSum_Id];

                tranObj.transform.FindChild("info_xiaohao/name").GetComponent<Text>().text = Fudata.summondata.name;
                tranObj.transform.FindChild("info_xiaohao/lvl").GetComponent<Text>().text = Fudata.summondata.level.ToString();
                Transform star = tranObj.transform.FindChild("info_xiaohao/stars");
                setStar(star, Fudata.summondata.star);
                for (int m = 0; m < tranObj.transform.FindChild("info_xiaohao/icon").childCount; m++)
                {
                    GameObject.Destroy(tranObj.transform.FindChild("info_xiaohao/icon").GetChild(m).gameObject);
                }
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(Fudata.confdata);
                icon.transform.SetParent(tranObj.transform.FindChild("info_xiaohao/icon"), false);
                new BaseButton(icon.transform).onClick = (GameObject go) => {
                    if (getSummonWin() == null) return;
                    GameObject plan = getSummonWin().GetSmallWin("uilayer_tofusum_summon");
      
                    plan.SetActive(true);
                    setFuSumlist(plan);
                };
                setExp(data, Fudata);
                foreach (int type in Fudata.summondata.shouhun.Keys)
                {
                    if (Fudata.summondata.shouhun[type].lvl > 0 || Fudata.summondata.shouhun[type].exp > 0)
                    {
                        tranObj.transform.FindChild("info_yulan/shouhun/" + type + "/up").gameObject.SetActive(true);
                    }
                    else {
                        tranObj.transform.FindChild("info_yulan/shouhun/" + type + "/up").gameObject.SetActive(false);
                    }
                }
                setskill(data,Fudata);

            }
            setmoney();
        }

        void setExp(a3_BagItemData data, a3_BagItemData Fudata)
        {
            tishi.SetActive(false);
            int allexp = 0;
            if (Fudata.summondata.level <= 1)
            {
                allexp += Fudata.summondata.currentexp;
            }
            else
            {
                for (int i = 1; i <= Fudata.summondata.level -1; i++)
                {
                    var xml = A3_SummonModel.getInstance().GetAttributeXml(i);
                    allexp += xml.getInt("exp");
                }
                allexp += Fudata.summondata.currentexp;
            }
            int newAllExp = allexp + data.summondata.currentexp;
            int curlvl = 0;
            curlvl += data.summondata.level;
            bool go_continue = false;
            while (true)
            {
                Variant v = canUplvl(curlvl, newAllExp);
                curlvl = v["lvl"];
                newAllExp = v["exp"];
                go_continue = v["go"];
                if (!go_continue) break;
            }
            SXML lvlXml = XMLMgr.instance.GetSXML("carrlvl.lvl_limit", "zhuanzheng==" + PlayerModel.getInstance().up_lvl);
            int PlayerMaxLvl = lvlXml.getInt("level_limit") + (int)PlayerModel.getInstance().lvl;
            if (curlvl >= PlayerMaxLvl) {
                curlvl = PlayerMaxLvl;
                newAllExp = 0;
                tishi.SetActive(true);
            }

            if (curlvl > data.summondata.level)
            {
                tranObj.transform.FindChild("info_yulan/lvl/Text").GetComponent<Text>().text = data.summondata.level + "(" + curlvl + ")";
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider_add").gameObject.SetActive(true);
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider").gameObject.SetActive(false);
                var xml = A3_SummonModel.getInstance().GetAttributeXml(curlvl);
                int exp_max = xml.getInt("exp");
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider_add").GetComponent<Image>().fillAmount = (float)newAllExp / (float)exp_max;
            }
            else {
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider_add").gameObject.SetActive(true);
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider").gameObject.SetActive(true);
                tranObj.transform.FindChild("info_yulan/lvl/Text").GetComponent<Text>().text = data.summondata.level + "(" + curlvl + ")";
                var xml = A3_SummonModel.getInstance().GetAttributeXml(data.summondata.level);
                int exp_max = xml.getInt("exp");
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider").GetComponent<Image>().fillAmount = (float)data.summondata.currentexp / (float)exp_max;
                var xml_next = A3_SummonModel.getInstance().GetAttributeXml(data.summondata.level);
                int exp_max_next = xml.getInt("exp");
                tranObj.transform.FindChild("info_yulan/lvl_bar/slider_add").GetComponent<Image>().fillAmount = (float)newAllExp / (float)exp_max_next;
                tranObj.transform.FindChild("info_yulan/lvl_bar/value").GetComponent<Text>().text = newAllExp + "/" + exp_max_next;
            }


        }

        Variant canUplvl(int lvl, int exp)
        {
            Variant data = new Variant();
            int maxlvl = XMLMgr.instance.GetSXML("callbeast").GetNodeList("attribute").Count;
            if (lvl >= maxlvl)
            {
                if (exp >= A3_SummonModel.getInstance().GetAttributeXml(maxlvl).getInt("exp"))
                {
                    exp = A3_SummonModel.getInstance().GetAttributeXml(maxlvl).getInt("exp");
                }
                data["lvl"] = maxlvl;
                data["go"] = false;
            }
            else {
                if (exp >= A3_SummonModel.getInstance().GetAttributeXml(lvl).getInt("exp"))
                {
                    exp -= A3_SummonModel.getInstance().GetAttributeXml(lvl).getInt("exp");
                    lvl += 1;
                    data["go"] = true;
                }
                else { data["go"] = false; }
                data["lvl"] = lvl;
            }
            
            data["exp"] = exp;
            return data;
        }


        void setskill(a3_BagItemData sum , a3_BagItemData fu = null)
        {
            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("icon/nul").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
            }
            int idner = 1;
            if (fu == null)
            {
                foreach (summonskill skill in sum.summondata.skills.Values)
                {
                    Transform skillCell = SkillCon.FindChild(idner.ToString());
                    skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                    skillCell.FindChild("icon/nul").gameObject.SetActive(false);
                    skillCell.FindChild("lock").gameObject.SetActive(false);
                    SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                    skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                    new BaseButton(skillCell).onClick = (GameObject go) =>
                    {
                        if (getSummonWin() == null) return;
                        if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                        GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");
                        tip.SetActive(true);
                        setskilltip(tip, skill);
                    };
                    idner++;
                }
            }
            else {
                foreach (summonskill skill in sum.summondata.skills.Values)
                {
                    Transform skillCell = SkillCon.FindChild(idner.ToString());
                    skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                    skillCell.FindChild("icon/nul").gameObject.SetActive(false);
                    skillCell.FindChild("lock").gameObject.SetActive(false);
                    SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                    skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                    new BaseButton(skillCell).onClick = (GameObject go) =>
                    {
                        if (getSummonWin() == null) return;
                        if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                        GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");

                        tip.SetActive(true);
                        setskilltip(tip, skill);
                    };
                    idner++;
                }
                int skillcount = 0;
                skillcount = sum.summondata.skills.Count;
                foreach (summonskill skill in fu.summondata.skills.Values) {
                    if (!sum.summondata.skills.Keys.Contains(skill.skillid))
                        skillcount++;
                }

                if (skillcount > 12)
                {
                    foreach (summonskill skill in fu.summondata.skills.Values)
                    {
                        if (sum.summondata.skills.Keys.Contains(skill.skillid))
                        {
                            continue;
                        }
                        if (idner > 12) { break; }
                        Transform skillCell = SkillCon.FindChild(idner.ToString());
                        skillCell.FindChild("icon/icon").gameObject.SetActive(false);
                        skillCell.FindChild("lock").gameObject.SetActive(false);
                        skillCell.FindChild("icon/nul").gameObject.SetActive(true);
                        new BaseButton(skillCell).onClick = (GameObject go) =>
                        {
                            if (getSummonWin() == null) return;
                            GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");
                            tip.SetActive(true);
                            setskilltip(tip);
                        };
                        idner++;
                    }

                }
                else {
                    foreach (summonskill skill in fu.summondata.skills.Values) {
                        if (sum.summondata.skills.Keys.Contains(skill.skillid))
                        {
                            continue;
                        }
                        Transform skillCell = SkillCon.FindChild(idner.ToString());
                        skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                        skillCell.FindChild("icon/nul").gameObject.SetActive(false);
                        skillCell.FindChild("lock").gameObject.SetActive(false);
                        SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                        skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                        new BaseButton(skillCell).onClick = (GameObject go) =>
                        {
                            if (getSummonWin() == null) return;
                            if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                            GameObject tip = getSummonWin().GetSmallWin("uilayer_skilltip_summon");

                            tip.SetActive(true);
                            setskilltip(tip, skill);
                        };
                        idner++;
                    }
                }
            }
        }

        void setskilltip(GameObject tip, summonskill skill = null)
        {   if (skill != null)
            {
                SXML skillXml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                tip.transform.FindChild("name").GetComponent<Text>().text = skillXml.getString("name") + skill.skilllv + ContMgr.getCont("ji");
                SXML curlvl = skillXml.GetNode("skill_att", "skill_lv==" + skill.skilllv);
                tip.transform.FindChild("des").GetComponent<Text>().text = curlvl.getString("descr2");
            }
            else {
                tip.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("nullskillTop"); 
                tip.transform.FindChild("des").GetComponent<Text>().text = ContMgr.getCont("nullskillText"); 
            }
        }
        void setFuSumlist(GameObject obj)
        {
            Transform Con = obj.transform.FindChild("summonlist/summons/scroll/content");
            GameObject item = obj.transform.FindChild("summonlist/summons/scroll/0").gameObject;
            obj.transform.FindChild("top").GetComponent<Text>().text = ContMgr.getCont("selsctTunshisummon");
            Dictionary<uint, a3_BagItemData> sT = A3_SummonModel.getInstance().GetSummons(true);
            for (int i = 0; i < Con.transform.childCount; i++)
            {
                GameObject.Destroy(Con.transform.GetChild(i).gameObject);
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
                clon.transform.FindChild("lv").GetComponent<Text>().text = it.summondata.level.ToString();
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(it.confdata);
                icon.transform.SetParent(clon.transform.FindChild("icon"), false);
                Transform starRoot = clon.transform.FindChild("stars");
                setStar(starRoot, it.summondata .star );

                new BaseButton(clon.transform).onClick = (GameObject go) => {
                    setFuID(it.id);
                    closeWin("uilayer_tofusum_summon");
                };
            }
            Con.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        void setFuID(uint Value)
        {
            if (Value != FuSum_Id)
            {
                FuSum_Id = Value;
                showCurSummon();
            }
        }

        void SetDispose()
        {
            if (getSummonWin().avatorobj != null)
            {
                GameObject.Destroy(getSummonWin().avatorobj);
                GameObject.Destroy(getSummonWin().camobj);
                getSummonWin().avatorobj = null;
                getSummonWin().camobj = null;
            }
        }
        void setAvator()
        {
            if (CurSummonID <= 0) return;
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            SetDispose();
            SXML xml = sumXml.GetNode("callbeast", "id==" + sum.tpid);
            int mid = xml.getInt("mid");
            SXML mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            int objid = mxml.getInt("obj");
            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            float y = float.Parse(mxml.getString("smshow_pos"));
            getSummonWin().avatorobj = GameObject.Instantiate(obj_prefab, new Vector3(-153.632f, 0.778f + y, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in getSummonWin().avatorobj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = getSummonWin().avatorobj.transform.FindChild("model");
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 270 - mxml.getInt("smshow_face"));
            float scale = mxml.getFloat("smshow_scale");
            if (scale < 0) { scale = 0.5f; }
            cur_model.transform.localScale = new Vector3(scale, scale, scale);
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            getSummonWin().camobj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = getSummonWin().camobj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
        }
        public void setStar(Transform starRoot, int num)
        {
            for (int i = 0; i < 5; i++)
            {
                starRoot.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }

            for (int j = 0; j < num; j++)
            {
                starRoot.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
        }

        public override void onCurSummonIdChange()
        {
            base.onCurSummonIdChange();
            FuSum_Id = 0;
            setAvator();
            showCurSummon();
        }


        public void setmoney()
        {
            Text money = tranObj.transform.FindChild("todo/money").GetComponent<Text>();
            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID) && A3_SummonModel.getInstance().GetSummons().ContainsKey(FuSum_Id))
            {
                a3_BagItemData curdata = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                int needmoney = 0;
                needmoney = sumXml.GetNode("combin_cost", "star==" + curdata.summondata.star).getInt("money");
                money.text = needmoney.ToString();
            }
            else {
                money.text = 0.ToString();
            }
        }

        public override void onAddNewSmallWin(string name)
        {
            base.onAddNewSmallWin(name);

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


        public void onTunshi(GameEvent v)
        {
            Variant data = v.data;
            if (data.ContainsKey("summon"))
            {
                FuSum_Id = 0;
                showCurSummon();
                refreSumlist((int)CurSummonID);
            }

        }
    }
}
