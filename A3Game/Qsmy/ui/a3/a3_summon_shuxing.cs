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
    class a3_summon_shuxing : BaseSummon
    {

        int btninder = 0;
        Transform Att;
        Transform Info;
        Transform SkillCon;
       // Dictionary<int, GameObject> skillCon_obj = new Dictionary<int, GameObject>();
        public a3_summon_shuxing(Transform trans,string name) : base(trans,name)
        {
            init();
        }
        void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("btns/fenjie/Text").text = ContMgr.getCont("a3_summon_shuxing_0");
            getComponentByPath<Text>("btns/chuzhan/Text").text = ContMgr.getCont("a3_summon_shuxing_1");
            getComponentByPath<Text>("zizhi_btn/zizhi").text = ContMgr.getCont("a3_summon_shuxing_2");
            getComponentByPath<Text>("allAtt_btn/shuxing").text = ContMgr.getCont("a3_summon_shuxing_3");
            getComponentByPath<Text>("exp/Text").text = ContMgr.getCont("a3_summon_shuxing_4");
            getComponentByPath<Text>("lifespan/Text").text = ContMgr.getCont("a3_summon_shuxing_5");
            getComponentByPath<Text>("info/gongji/Text").text = ContMgr.getCont("a3_summon_xilian_4");
            getComponentByPath<Text>("info/fangyu/Text").text = ContMgr.getCont("a3_summon_xilian_5");
            getComponentByPath<Text>("info/minjie/Text").text = ContMgr.getCont("a3_summon_xilian_2");
            getComponentByPath<Text>("info/tili/Text").text = ContMgr.getCont("a3_summon_xilian_3");
            getComponentByPath<Text>("info/xingyun/Text").text = ContMgr.getCont("a3_summoninfo_4");
            getComponentByPath<Text>("att/life").text = ContMgr.getCont("a3_summoninfo_5");
            getComponentByPath<Text>("att/phyatk").text = ContMgr.getCont("a3_summoninfo_6");
            getComponentByPath<Text>("att/phydef").text = ContMgr.getCont("a3_summoninfo_7");
            getComponentByPath<Text>("att/manadef").text = ContMgr.getCont("a3_summoninfo_8");
            getComponentByPath<Text>("att/hit").text = ContMgr.getCont("a3_summoninfo_9");
            getComponentByPath<Text>("att/manaatk").text = ContMgr.getCont("a3_summoninfo_10");
            getComponentByPath<Text>("att/crit").text = ContMgr.getCont("a3_summoninfo_11");
            getComponentByPath<Text>("att/dodge").text = ContMgr.getCont("a3_summoninfo_12");
            getComponentByPath<Text>("att/hitit").text = ContMgr.getCont("a3_summoninfo_13");
            getComponentByPath<Text>("att/fatal_damage").text = ContMgr.getCont("a3_summoninfo_14");
            getComponentByPath<Text>("att/reflect_crit_rate").text = ContMgr.getCont("a3_summoninfo_15");
           #endregion

            new BaseButton(tranObj.transform.FindChild("allAtt_btn")).onClick = AttBtn;
            new BaseButton(tranObj.transform.FindChild("zizhi_btn")).onClick = zizhiBtn;
            new BaseButton(tranObj.transform.FindChild("btns/chuzhan")).onClick = onchuzhan;
            new BaseButton(tranObj.transform.FindChild("btns/fenjie")).onClick = onfenjie;
            new BaseButton(tranObj.transform.FindChild("exp/add")).onClick = feed_exp;
            new BaseButton(tranObj.transform.FindChild("lifespan/add")).onClick = feed_life;

            getEventTrigerByPath("tach").onDrag = OnDrag;
            Att = tranObj.transform.FindChild("att");
            Info = tranObj.transform.FindChild("info");
            SkillCon = tranObj.transform.FindChild("skills");


        }
        public override void onShowed()
        {
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_CHUZHAN, onChuzhan);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_XIUXI, onXiuxi);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_FEEDEXP, Event_S2CFeedEXP);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_FEEDSM, Event_S2CFeedSM);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_FENJIEINFO, FenjieInfo);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_FENJIERES, Fenjieres);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SKILLUP, onskillup);
            refreshInfo();
            btninder = 0;
            Att.gameObject.SetActive(false);
            Info.gameObject.SetActive(true);
            tranObj.transform.FindChild("allAtt_btn").GetComponent<Button>().interactable = true;
            tranObj.transform.FindChild("zizhi_btn").GetComponent<Button>().interactable = false;
            //tranObj.transform.FindChild("allAtt_btn/zizhi").gameObject.SetActive(true);
            //tranObj.transform.FindChild("allAtt_btn/shuxing").gameObject.SetActive(false);

            closeWin("uilayer_feedpan");
            closeWin("uilayer_feedpan2");
            closeWin("uilayer_usetip_summon");
            closeWin("uilayer_getlook_summon");

        }

        public override void onClose()
        {
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_CHUZHAN, onChuzhan);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_XIUXI, onXiuxi);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_FEEDEXP, Event_S2CFeedEXP);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_FEEDSM, Event_S2CFeedSM);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_FENJIEINFO, FenjieInfo);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_FENJIERES, Fenjieres);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_SKILLUP, onskillup);
            SetDispose();
        }
        public override void onCurSummonIdChange()
        {
            base.onCurSummonIdChange();
            refreshInfo();
            SetBtn_Text();
        }

        public override void onAddNewSmallWin(string name)
        {//初始化小窗口，一般是加一些事件，相当于小窗口的init
            base.onAddNewSmallWin(name);
            switch (name)
            {
                case "uilayer_usetip_summon":
                    #region 初始化物品使用窗口
                    GameObject useTip = getSummonWin()?.GetSmallWin(name);
                    new BaseButton (useTip.transform.FindChild("info/bodyNum/btn_add")).onClick = onadd;
                    new BaseButton(useTip.transform.FindChild("info/bodyNum/btn_reduce")).onClick = onreduce;
                    new BaseButton(useTip.transform.FindChild("info/bodyNum/max")).onClick = onmax;
                    new BaseButton(useTip.transform.FindChild("info/bodyNum/min")).onClick = onmin;
                    #region/*提取预制件中的汉字*/
                    useTip.transform.FindChild("info/use/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_0");
                    useTip.transform.FindChild("info/sell/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_1");
                    useTip.transform.FindChild("info/put/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_2");
                    useTip.transform.FindChild("info/text_lv").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_3");
                    useTip.transform.FindChild("info/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_4");
                    useTip.transform.FindChild("info/bodyNum/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_5");
                    useTip.transform.FindChild("info/bodyNum/res").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_6");
                    useTip.transform.FindChild("info/bodyNum/bug/txt").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_7");
                    useTip.transform.FindChild("info/bodyNum/min/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_8");
                    useTip.transform.FindChild("info/bodyNum/max/Text").GetComponent<Text>().text = ContMgr.getCont("usetip_summon_9");
                    #endregion
                    new BaseButton(useTip.transform.FindChild("touch")).onClick = (GameObject go) =>
                    {
                        useTip.SetActive(false);
                        if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                        {
                            getSummonWin().avatorobj.SetActive(true);
                        }
                        curFood_tpid = 0;
                    };
                    EventTriggerListener.Get(useTip.transform.FindChild("info/bodyNum/btn_add").gameObject).onDown = delegate (GameObject go) {
                        isAdd = true;
                        rateTime = 0.0f;
                        addTime = 0.5f;
                    };
                    EventTriggerListener.Get(useTip.transform.FindChild("info/bodyNum/btn_add").gameObject).onExit = delegate (GameObject go) {
                        isAdd = false;
                    };
                    EventTriggerListener.Get(useTip.transform.FindChild("info/bodyNum/btn_reduce").gameObject).onDown = delegate (GameObject go) {
                        isReduce = true;
                        rateTime = 0.0f;
                        addTime = 0.5f;
                    };
                    EventTriggerListener.Get(useTip.transform.FindChild("info/bodyNum/btn_reduce").gameObject).onExit = delegate (GameObject go) {
                        isReduce = false;
                    };
                    #endregion
                    break;
                case "uilayer_feedpan":
                    #region 初始exp喂养窗口
                    GameObject expplan = getSummonWin()?.GetSmallWin(name);
                    expplan.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("feedpan_0");
                    if (food_obj.Count <= 0)
                    {
                        GameObject prf = expplan.transform.FindChild("feeditems/scroll/0").gameObject;
                        Transform root = expplan.transform.FindChild("feeditems/scroll/content");
                        new BaseButton(expplan.transform.FindChild("tach")).onClick = (GameObject gg) =>
                        {
                            expplan.SetActive(false);
                        };
                        foreach (int id in A3_SummonModel.getInstance().feedexplist)
                        {
                            GameObject oo = (GameObject)GameObject.Instantiate(prf);
                            oo.transform.SetParent(root, false);
                            oo.transform.localScale = Vector3.one;
                            oo.SetActive(true);
                            a3_ItemData data = a3_BagModel.getInstance().getItemDataById((uint)id);
                            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
                            icon.transform.SetParent(oo.transform.FindChild("icon"), false);
                            icon.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                            uint typeid = (uint)id;
                            oo.name = typeid.ToString();
                            new BaseButton(oo.transform).onClick = (GameObject goo) =>
                            {
                                if (a3_BagModel.getInstance().getItemNumByTpid(typeid) > 0)
                                {
                                    curFood_tpid = typeid;
                                    setUseTip();
                                }
                                else
                                {
                                    flytxt.instance.fly(ContMgr.getCont("a3_summon4"));
                                    if (XMLMgr.instance.GetSXML("item.item", "id==" + typeid).GetNode("drop_info") == null)
                                        return;
                                    ArrayList data1 = new ArrayList();
                                    data1.Add(a3_BagModel.getInstance().getItemDataById(typeid));
                                    data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                                    if (getSummonWin().avatorobj != null)
                                        data1.Add(getSummonWin().avatorobj);
                                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                                }
                            };
                            var tg = oo.transform.FindChild("Text");
                            if (tg != null)
                            {
                                Text tt = tg.GetComponent<Text>();
                                tt.text = data.item_name + "\n" + ContMgr.getCont("a3_summon5") + data.main_effect + ContMgr.getCont("a3_summon6");
                            }
                            food_obj[(uint)id] = icon;
                        }
                    }
                    #endregion
                    break;
                case "uilayer_feedpan2":
                    #region 初始life喂养窗口
                    GameObject lifeplan = getSummonWin()?.GetSmallWin(name);
                    lifeplan.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("feedpan_1");
                    if (sm_obj.Count <= 0)
                    {
                        GameObject prf = lifeplan.transform.FindChild("feeditems/scroll/0").gameObject;
                        Transform root = lifeplan.transform.FindChild("feeditems/scroll/content");
                        new BaseButton(lifeplan.transform.FindChild("tach")).onClick = (GameObject gg) =>
                        {
                            lifeplan.SetActive(false);
                        };
                        foreach (int id in A3_SummonModel.getInstance().feedsmlist)
                        {
                            GameObject go = (GameObject)GameObject.Instantiate(prf);
                            go.transform.SetParent(root);
                            go.transform.localScale = Vector3.one;
                            go.SetActive(true);
                            a3_ItemData data = a3_BagModel.getInstance().getItemDataById((uint)id);
                            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
                            icon.transform.SetParent(go.transform.FindChild("icon"), false);
                            icon.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                            uint typeid = (uint)id;
                            new BaseButton(go.transform).onClick = (GameObject goo) =>
                            {
                                if (a3_BagModel.getInstance().getItemNumByTpid(typeid) > 0)
                                {
                                    curFood_tpid = typeid;
                                    setUseTip();
                                }
                                else
                                {
                                    flytxt.instance.fly(ContMgr.getCont("a3_summon4"));
                                    if (XMLMgr.instance.GetSXML("item.item", "id==" + typeid).GetNode("drop_info") == null)
                                        return;
                                    ArrayList data1 = new ArrayList();
                                    data1.Add(a3_BagModel.getInstance().getItemDataById(typeid));
                                    data1.Add(InterfaceMgr .A3_SUMMON_NEW);
                                    if (getSummonWin().avatorobj != null)
                                        data1.Add(getSummonWin().avatorobj);
                                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                                }
                            };
                            var tg = go.transform.FindChild("Text");
                            if (tg != null)
                            {
                                Text tt = tg.GetComponent<Text>();
                                tt.text = data.item_name + "\n" + ContMgr.getCont("a3_summon5") + data.main_effect + ContMgr.getCont("a3_summon7");
                            }
                            sm_obj[(uint)id] = icon;
                        }
                    }
                    #endregion
                    break;
                case "uilayer_getlook_summon":
                    #region 初始化分解预览
                    GameObject Fenjie = getSummonWin()?.GetSmallWin(name);
                    Fenjie.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("getlook_summon_0");
                    Fenjie.transform.FindChild("Todo/Text").GetComponent<Text>().text = ContMgr.getCont("getlook_summon_1");
                    new BaseButton(Fenjie.transform.FindChild("tach")).onClick = (GameObject go) => {
                        Fenjie.SetActive(false);
                        if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                        {
                            getSummonWin().avatorobj.SetActive(true);
                        }
                    };
                    new BaseButton(Fenjie.transform.FindChild("Todo")).onClick = (GameObject go) => {
                        A3_SummonProxy.getInstance().sendFenjie(CurSummonID);
                    };
                    #endregion
                    break;
                case "uilayer_skillup_summon":
                    #region  初始化技能升级
                    GameObject skillCon = getSummonWin()?.GetSmallWin(name);
                    skillCon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("skillup_summon_0");
                    skillCon.transform.FindChild("canup/Todo/Text").GetComponent<Text>().text = ContMgr.getCont("skillup_summon_1");
                    skillCon.transform.FindChild("max/black/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
                    new BaseButton (skillCon.transform.FindChild("max/black")).onClick =
                    new BaseButton(skillCon.transform.FindChild("tach")).onClick = (GameObject go) => {
                        skillCon.SetActive(false);
                        if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                        {
                            getSummonWin().avatorobj.SetActive(true);
                        }
                    };
                    #endregion
                    break;
            }

        }
        void AttBtn(GameObject go) {

            Att.gameObject.SetActive(true);
            Info.gameObject.SetActive(false);
            tranObj.transform.FindChild("allAtt_btn").GetComponent<Button>().interactable = false;
            tranObj.transform.FindChild("zizhi_btn").GetComponent<Button>().interactable = true;

            
            //if (btninder == 0)
            //{
            //    Att.gameObject.SetActive(true);
            //    Info.gameObject.SetActive(false);
            //    go.transform.FindChild("zizhi").gameObject.SetActive(false);
            //    go.transform.FindChild("shuxing").gameObject.SetActive(true);
            //    btninder = 1;
            //}
            //else if (btninder == 1) {
            //    Att.gameObject.SetActive(false);
            //    Info.gameObject.SetActive(true);
            //    go.transform.FindChild("zizhi").gameObject.SetActive(true);
            //    go.transform.FindChild("shuxing").gameObject.SetActive(false);
            //    btninder = 0;
            //}
        }
        void zizhiBtn(GameObject go)
        {
            Att.gameObject.SetActive(false);
            Info.gameObject.SetActive(true);
            tranObj.transform.FindChild("allAtt_btn").GetComponent<Button>().interactable = true;
            tranObj.transform.FindChild("zizhi_btn").GetComponent<Button>().interactable = false;
        }

        void onchuzhan(GameObject go) {

            if ( SelfRole._inst.invisible  )
            {
                flytxt.instance.fly( ContMgr.getCont( "a3_summoninvisible" )   );

                return;
            }

            if (CurSummonID == A3_SummonModel.getInstance().nowShowAttackID)
            {
                A3_SummonProxy.getInstance().sendZhaohui();
            }
            else
            {
                if (A3_SummonModel.getInstance().getSumCds().ContainsKey((int)CurSummonID))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon10"));
                }
                else
                    A3_SummonProxy.getInstance().sendChuzhan(CurSummonID);
            }
        }
        void onfenjie(GameObject go) {
            if (CurSummonID == A3_SummonModel.getInstance().nowShowAttackID)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_summon3"));
                return;
            }
            if (A3_SummonModel.getInstance ().GetSummons ()[CurSummonID].summondata.level < 50)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_summon28"));
                return;
            }
            A3_SummonProxy.getInstance().sendFenjie_look(CurSummonID);
        }
        void feed_exp(GameObject go) {
            if (getSummonWin() == null) return;
            SXML lvlXml = XMLMgr.instance.GetSXML("carrlvl.lvl_limit", "zhuanzheng==" + PlayerModel.getInstance().up_lvl);
            int PlayerMaxLvl = lvlXml.getInt("level_limit") + (int)PlayerModel.getInstance().lvl;
            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
            {
                a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                if (PlayerMaxLvl <= sum.summondata .level ) {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_maxExp"));
                    return;
                }
            }
            GameObject plan = getSummonWin().GetSmallWin("uilayer_feedpan");
           
            plan.SetActive(true);
            refFeed_expWin();
        }
        void feed_life(GameObject go) {
            if (getSummonWin() == null) return;
            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
            {
                a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                if (sum.summondata.lifespan >= A3_SummonModel.getInstance().maxSm)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_maxLife"));
                    return;
                }
            }
            GameObject plan = getSummonWin().GetSmallWin("uilayer_feedpan2");
            
            plan.SetActive(true);
            refFeed_lifeWin();
        }

        #region 属性界面信息
        void refreshInfo()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            setSumBaseinfo(data);
            setAtt(data);
            setZizhi(data);
            setExp(data);
            setLife(data);
            setAvator(data);
            setskill(data);
        }

        void setSumBaseinfo(a3_BagItemData sum)
        {
            tranObj.transform.FindChild("tet_top/lv/Text").GetComponent<Text>().text = sum.summondata.level.ToString();
            tranObj.transform.FindChild("tet_top/name/Text").GetComponent<Text>().text = sum.summondata.name;
            tranObj.transform.FindChild("power/num").GetComponent<Text>().text = sum.summondata.power.ToString();
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
        void setAvator(a3_BagItemData sum)
        {
            if (CurSummonID <= 0) return;
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

        void setAtt(a3_BagItemData sum)
        {
            tranObj.transform.FindChild("att/life/Text").GetComponent<Text>().text = sum.summondata.maxhp.ToString();
            tranObj.transform.FindChild("att/phyatk/Text").GetComponent<Text>().text = sum.summondata.min_attack + "-" + sum.summondata.max_attack;
            tranObj.transform.FindChild("att/phydef/Text").GetComponent<Text>().text = sum.summondata.physics_def.ToString();
            tranObj.transform.FindChild("att/manadef/Text").GetComponent<Text>().text = sum.summondata.magic_def .ToString();
            tranObj.transform.FindChild("att/hit/Text").GetComponent<Text>().text = (float)sum.summondata.physics_dmg_red / 10 + "%";
            tranObj.transform.FindChild("att/manaatk/Text").GetComponent<Text>().text = (float)sum.summondata.magic_dmg_red/ 10 + "%";
            tranObj.transform.FindChild("att/crit/Text").GetComponent<Text>().text = sum.summondata.double_damage_rate.ToString();
            tranObj.transform.FindChild("att/dodge/Text").GetComponent<Text>().text = sum.summondata.dodge.ToString();
            tranObj.transform.FindChild("att/fatal_damage/Text").GetComponent<Text>().text = (float)sum.summondata.fatal_damage / 10 + "%";
            tranObj.transform.FindChild("att/hitit/Text").GetComponent<Text>().text = sum.summondata.hit.ToString();
            tranObj.transform.FindChild("att/reflect_crit_rate/Text").GetComponent<Text>().text = sum.summondata.reflect_crit_rate.ToString();
        }

        void setZizhi(a3_BagItemData sum) {
            SXML xml = sumXml.GetNode("callbeast", "id==" + sum.summondata.tpid);
            SXML attxml = xml.GetNode("star", "star_sum==" + sum.summondata.star);
            tranObj.transform.FindChild("info/gongji/value").GetComponent<Text>().text = sum.summondata.attNatural+"/"+ attxml.GetNode("att").getInt("reset_max");
            tranObj.transform.FindChild("info/fangyu/value").GetComponent<Text>().text = sum.summondata.defNatural + "/" + attxml.GetNode("def").getInt("reset_max");
            tranObj.transform.FindChild("info/minjie/value").GetComponent<Text>().text = sum.summondata.agiNatural + "/"+ attxml.GetNode("agi").getInt("reset_max");
            tranObj.transform.FindChild("info/tili/value").GetComponent<Text>().text = sum.summondata.conNatural + "/" + attxml.GetNode("con").getInt("reset_max");
            tranObj.transform.FindChild("info/xingyun/value").GetComponent<Text>().text = sum.summondata.luck + "/" + 100;

            tranObj.transform.FindChild("info/gongji/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.attNatural /(float)attxml.GetNode("att").getInt("reset_max");
            tranObj.transform.FindChild("info/fangyu/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.defNatural / (float)attxml.GetNode("def").getInt("reset_max");
            tranObj.transform.FindChild("info/minjie/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.agiNatural / (float)attxml.GetNode("agi").getInt("reset_max");
            tranObj.transform.FindChild("info/tili/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.conNatural / (float)attxml.GetNode("con").getInt("reset_max");
            tranObj.transform.FindChild("info/xingyun/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.luck / (float)100;
        }

        void setExp(a3_BagItemData sum) {
            var xml = A3_SummonModel.getInstance().GetAttributeXml(sum.summondata.level);
            int exp_max = xml.getInt("exp");
            tranObj.transform.FindChild("exp/value").GetComponent<Text>().text = sum.summondata.currentexp + "/" + exp_max;
            var exp_slider = transform.FindChild("exp/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.currentexp / (float)exp_max;
        }

        void setLife(a3_BagItemData sum) {
            transform.FindChild("lifespan/slider").GetComponent<Image>().fillAmount = (float)sum.summondata.lifespan / (float)A3_SummonModel.getInstance().maxSm;
            transform.FindChild("lifespan/value").GetComponent<Text>().text = sum.summondata.lifespan + "/" + A3_SummonModel.getInstance().maxSm; ;
        }

        void setskill(a3_BagItemData sum)
        {
            //skillCon_obj.Clear();
            for (int i =0;i<SkillCon.childCount;i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
            }
            int idner = 1;
            foreach (summonskill skill in sum.summondata.skills.Values)
            {
                Transform skillCell = SkillCon.FindChild(idner.ToString());
                skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                skillCell.FindChild("lock").gameObject.SetActive(false);
                SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                new BaseButton(skillCell.transform).onClick = (GameObject go) =>
                {
                    if (getSummonWin() == null) return;
                    if (skillCell.FindChild("icon/icon").gameObject.activeSelf == false) return;
                    GameObject plan = getSummonWin().GetSmallWin("uilayer_skillup_summon");
                    plan.SetActive(true);
                    setUpSkill(plan, skill);

                };
                idner++;
            }
        }
        #endregion

        #region 刷新喂养经验丹窗口
        Dictionary<uint, GameObject> food_obj = new Dictionary<uint, GameObject>();
        uint curFood_tpid = 0;
        public void refFeed_expWin()
        {
            foreach (uint tpid in food_obj.Keys) //刷新数量
            {
                food_obj[tpid].transform.FindChild("num").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(tpid) + "";
                food_obj[tpid].transform.FindChild("num").gameObject.SetActive(true);
            }
        }
        #endregion

        #region 刷新喂养寿命丹窗口
        Dictionary<uint, GameObject> sm_obj = new Dictionary<uint, GameObject>();
        void refFeed_lifeWin()
        {
            foreach (uint tpid in sm_obj.Keys)
            {
                sm_obj[tpid].transform.FindChild("num").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(tpid) + "";
                sm_obj[tpid].transform.FindChild("num").gameObject.SetActive(true);
            }
        }
        #endregion

        int cur_num;
        int maxCanEat = 0;
        void setUseTip()
        {
            if (getSummonWin() == null) return;
            GameObject useTip = getSummonWin().GetSmallWin("uilayer_usetip_summon");


            useTip.SetActive(true);
            useTip.transform.SetAsLastSibling();

            maxCanEat = 0;
            if (A3_SummonModel.getInstance().feedexplist.Contains((int)curFood_tpid))
            {
                maxCanEat = CanEat_count(curFood_tpid);
            } else if (A3_SummonModel.getInstance().feedsmlist.Contains((int)curFood_tpid))
            {

            }
            if (getSummonWin().avatorobj && getSummonWin().avatorobj.activeSelf )
            {
                getSummonWin().avatorobj.SetActive(false);
            }
            a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById(curFood_tpid);
            Transform info = useTip.transform.FindChild("info");
            info.FindChild("name").GetComponent<Text>().text = itemdata.item_name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(itemdata.quality);
            info.FindChild("desc").GetComponent<Text>().text = StringUtils.formatText(itemdata.desc);
            if (itemdata.use_limit > 0)
                info.FindChild("lv").GetComponent<Text>().text = itemdata.use_limit + ContMgr.getCont("zhuan") + itemdata.use_lv + ContMgr.getCont("ji");
            else
                info.FindChild("lv").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi");
            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
               GameObject.Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
            icon.transform.SetParent(Image, false);
            cur_num = 0;
            refreshNum();
            new BaseButton(useTip.transform.FindChild("info/use")).onClick = (GameObject go) =>
            {
                if (cur_num <= 0) { return; }
                if (A3_SummonModel.getInstance().feedexplist.Contains((int)curFood_tpid))
                {
                    Send_FeedExp((int)curFood_tpid, cur_num);
                }
                else if (A3_SummonModel.getInstance().feedsmlist.Contains((int)curFood_tpid))
                {
                    Send_FeedSM((int)curFood_tpid, cur_num);
                }
                getSummonWin().GetSmallWin("uilayer_feedpan").SetActive(false);
                getSummonWin().GetSmallWin("uilayer_feedpan2").SetActive(false);
                useTip.SetActive(false);
                if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                {
                    getSummonWin().avatorobj.SetActive(true);
                }
            };  
        }
        private void Send_FeedExp(int tpid, int num)
        {
            A3_SummonProxy.getInstance().sendExp(CurSummonID,(uint) tpid, (uint)num);
        }
        private void Send_FeedSM(int tpid, int num)
        {
            A3_SummonProxy.getInstance().sendSM(CurSummonID, (uint)num);
        }
        void refreshNum()
        {
            //if (cur_num == 0)
            //    cur_num = 1;
            getSummonWin().GetSmallWin("uilayer_usetip_summon").transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();   
            getSummonWin().GetSmallWin("uilayer_usetip_summon").transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (a3_BagModel.getInstance().getItemDataById((uint)curFood_tpid).main_effect * cur_num).ToString();
            string str = "";
            if (A3_SummonModel.getInstance().feedexplist.Contains((int)curFood_tpid))
            {
                str = ContMgr.getCont("a3_summon8");
            }
            else if (A3_SummonModel.getInstance().feedsmlist.Contains((int)curFood_tpid))
            {
                str = ContMgr.getCont("a3_summon9");
            }
            getSummonWin().GetSmallWin("uilayer_usetip_summon").transform.FindChild("info/bodyNum/res").GetComponent<Text>().text = str;
            //计算经验和寿命值
        }
        bool isAdd = false;
        bool isReduce = false;
        float addTime = 0.5f;
        float rateTime = 0.0f;
        public override void _updata()
        {
            base._updata();
            {
                if (isAdd || isReduce)
                {
                    addTime -= Time.deltaTime;
                    if (addTime < 0)
                    {
                        rateTime += 0.05f;
                        addTime = 0.5f - rateTime;
                        if (isAdd)
                        {
                            onadd(null);
                        }
                        if (isReduce)
                        {
                            onreduce(null);
                        }
                    }
                }
            }
        }
        void setUpSkill(GameObject pre , summonskill skill)
        {
            SXML skillXml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
            List<SXML> lvls = skillXml.GetNodeList("skill_att");
            Text needText = pre.transform.FindChild("newSkill").GetComponent<Text>();
            pre.transform.FindChild("oldSkill").GetComponent<Text>().text = skillXml.getString("name") + skill.skilllv + ContMgr.getCont("ji");
            SXML curlvl = skillXml.GetNode("skill_att", "skill_lv==" + skill.skilllv);
            pre.transform.FindChild("oldres").GetComponent<Text>().text = curlvl.getString ("descr2");
            int itemnum = 0; 
            if (skill.skilllv >= lvls.Count)
            {
                pre.transform.FindChild("canup").gameObject.SetActive(false);
                pre.transform.FindChild("max").gameObject.SetActive(true);
                pre.transform.FindChild("needtext").GetComponent<Text>().text = ContMgr.getCont("jinengmanji");
                needText.gameObject.SetActive(false);
            }
            else {
                pre.transform.FindChild("canup").gameObject.SetActive(true);
                pre.transform.FindChild("max").gameObject.SetActive(false);
                needText.gameObject.SetActive(true);
                needText.text = skillXml.getString("name") + (skill.skilllv + 1) + ContMgr.getCont("ji");
                int itemid = curlvl.getInt("item_id");
                SXML lastlvl = skillXml.GetNode("skill_att", "skill_lv==" + (skill.skilllv+1));
                itemnum = lastlvl.getInt("item_num");
                int havelvl = lastlvl.getInt("open_lvl");

                SXML curlvl_need = skillXml.GetNode("skill_att", "skill_lv==" + (skill.skilllv+1));
                pre.transform.FindChild("needtext").GetComponent<Text>().text = curlvl_need.getString("descr2");
                a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
                icon.transform.SetParent(pre.transform.FindChild("canup/iconcon/icon"), false);
                new BaseButton(icon.transform).onClick = (GameObject go) =>
                {
                    ArrayList arr = new ArrayList();
                    arr.Add((uint)itemid);
                    arr.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                };
                pre.transform.FindChild("canup/Todo/Text").GetComponent<Text>().text =ContMgr.getCont("skillup_summon_1");
                if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                {
                    a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                    if (sum.summondata.level < havelvl)
                    {
                        pre.transform.FindChild("canup/Todo/Text").GetComponent<Text>().text = havelvl + ContMgr.getCont("ji");
                    }
                }
                pre.transform.FindChild("canup/iconcon/count").GetComponent<Text>().text = itemnum.ToString();
                new BaseButton(pre.transform.FindChild("canup/Todo")).onClick = (GameObject go) =>
                {
                    a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                    if (sum.summondata.level < havelvl)
                    {
                        flytxt.instance.fly(ContMgr.getCont("sumlvllose",new List <string> { havelvl.ToString()}));
                    }else 
                    A3_SummonProxy.getInstance().sendUpskill(CurSummonID, (uint)skill.skillid);
                };
            }
        }
        void onadd(GameObject go)
        {
            cur_num++;
            if (cur_num >= a3_BagModel.getInstance().getItemNumByTpid(curFood_tpid))
                cur_num = a3_BagModel.getInstance().getItemNumByTpid(curFood_tpid);
            if (cur_num >= 999) { cur_num = 999; }
            int CanUse = CanUseNum();
            if (cur_num > CanUse) cur_num = CanUse;
            refreshNum();
        }
        void onreduce(GameObject go)
        {
            cur_num--;
            if (cur_num < 0)
                cur_num = 0;
            refreshNum();
        }
        private void onmin(GameObject obj)
        {
            cur_num = 0;
            refreshNum();
        }
        private void onmax(GameObject obj)
        {
            int CanUse = CanUseNum();
            if (a3_BagModel.getInstance().getItemNumByTpid(curFood_tpid) <= 999)
            {
                cur_num = a3_BagModel.getInstance().getItemNumByTpid(curFood_tpid);
            }
            else
            {
                cur_num = 999;
            }

            if (cur_num > CanUse) cur_num = CanUse;
            refreshNum();

        }

        int CanEat_count( uint curFood_tpid) {
            int count = 0;
            SXML lvlXml = XMLMgr.instance.GetSXML("carrlvl.lvl_limit", "zhuanzheng==" + PlayerModel .getInstance ().up_lvl);
            int PlayerMaxLvl = lvlXml.getInt("level_limit") + (int)PlayerModel.getInstance().lvl;
            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
            {
                a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                int canUplvl = PlayerMaxLvl - sum.summondata.level;
                long needExp = 0;
                for (int i = sum.summondata.level;i <= PlayerMaxLvl -1;i++ ) {
                    var xml = A3_SummonModel.getInstance().GetAttributeXml(i);
                    needExp += xml.getInt("exp");
                }
                needExp -= sum.summondata.currentexp;
                SXML item = XMLMgr.instance.GetSXML("item.item", "id==" + curFood_tpid);
                int one_getExp = item.getInt("main_effect");
                if (needExp > 0)
                {
                    count = (int)(needExp / one_getExp);
                    if (needExp % one_getExp > 0) { count += 1; }
                }
                else {
                    count = 0;
                }
            }
            return count;
        }



        int CanUseNum() {
            int num = 0;
            if (A3_SummonModel.getInstance().feedsmlist.Contains((int)curFood_tpid))
            {
                a3_ItemData data = a3_BagModel.getInstance().getItemDataById(curFood_tpid);
                a3_BagItemData data_sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                int life = A3_SummonModel.getInstance ().maxSm - data_sum.summondata.lifespan;
                if (life % data.main_effect > 0)
                {
                    num = life / data.main_effect + 1;
                }
                else num = life / data.main_effect;
            }
            if (A3_SummonModel.getInstance().feedexplist.Contains((int)curFood_tpid))
            {
                num = maxCanEat;
            }
            return num;
        }
        

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (getSummonWin().avatorobj != null)
            {
                getSummonWin().avatorobj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        public void SetBtn_Text()
        {
            if (CurSummonID <= 0) return;
            if (CurSummonID == A3_SummonModel.getInstance().nowShowAttackID)
            {
                tranObj.transform.FindChild("btns/chuzhan/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon21");
            }
            else {
                tranObj.transform.FindChild("btns/chuzhan/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon22");
            }
        }

        void onChuzhan(GameEvent evt)
        {
            Variant data = evt.data;
            if (data.ContainsKey("summon_id"))
            {
                uint id = data["summon_id"];
                //refsumInfo((uint)data["summon_id"]);
                if (CurSummonID == id)
                {
                    if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                    {
                        a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                        setLife(sum);
                        setAtt(sum);
                        SetBtn_Text();
                    }
                }
                getSummonWin()?.refreshZHSlist_chuzhan();
            }
        }

        void onXiuxi(GameEvent evt)
        {
            Variant data = evt.data;
            if (data.ContainsKey("summon_id"))
            {
                uint id = data["summon_id"];
                if (CurSummonID == id)
                {
                    if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                    {
                        a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                        setLife(sum);
                        setAtt(sum);
                        SetBtn_Text();
                    }
                }
                getSummonWin()?.refreshZHSlist_chuzhan();
            }
        }

        void Event_S2CFeedEXP(GameEvent evt)
        {
            Variant data = evt.data;
            uint id = 0;
            if (data.ContainsKey("summon"))
            {
                Variant summon = new Variant();
                summon = data["summon"];
                id = summon["id"];
            }
            else if(data.ContainsKey("summon_id"))
            {
                id = data["summon_id"];
            }
            if (CurSummonID == (uint)id)
            {
                if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                {
                    a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                    setAtt(sum);
                    setExp(sum);
                    setSumBaseinfo(sum);
                }
            }
            getSummonWin()?.setSum_one((uint)id);
        }

        void Event_S2CFeedSM(GameEvent evt)
        {
            Variant data = evt.data;
            if (data.ContainsKey("summon_id"))
            {
                uint id = data["summon_id"];
                if (CurSummonID == id)
                {
                    if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID)) {
                        a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                        setLife(sum);
                    }
                }
            }
        }

        void FenjieInfo(GameEvent evt)
        {
            Variant data = evt.data;
            GameObject plan = getSummonWin().GetSmallWin("uilayer_getlook_summon");


            plan.SetActive(true);
            GameObject pre = plan.transform.FindChild("iconcon").gameObject;
            Transform con = plan.transform.FindChild("items");
            for (int i = 0;i< con.childCount;i++) {
                GameObject.Destroy(con.GetChild (i).gameObject);
            }
            if (getSummonWin().avatorobj && getSummonWin().avatorobj.activeSelf)
            {
                getSummonWin().avatorobj.SetActive(false);
            }
            for (int i =0;i< data["item_cost"].Count;i++)
            {
                GameObject clon = GameObject.Instantiate(pre) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(con,false);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel .getInstance ().getItemDataById (data["item_cost"][i]["item_id"]));
                icon.transform.SetParent(clon.transform.FindChild ("icon"),false);
                clon.transform.FindChild("count").GetComponent<Text>().text = data["item_cost"][i]["item_num"];

            }
        }
        void Fenjieres(GameEvent evt)
        {
            closeWin("uilayer_getlook_summon");
            Variant data = evt.data;
            if (data.ContainsKey ("rmv_id"))
            {
                refreSumlist();
            }
        }

        void onskillup(GameEvent evt)
        {
            closeWin("uilayer_skillup_summon");
            flytxt.instance.fly(ContMgr.getCont("a3_summon_sjcg"));
            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
            {
                a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                setSumBaseinfo(sum);
                setAtt(sum);
                setskill(sum);
            }
        }
    }
}
