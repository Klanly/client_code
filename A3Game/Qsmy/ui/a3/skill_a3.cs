using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Cross;
using DG.Tweening;
using MuGame.Qsmy.model;

namespace MuGame
{
    class skill_a3 : Window
    {

        GameObject[] skillgroupsone = new GameObject[4];
        GameObject[] skillgrouptwo = new GameObject[4];

        Dictionary<int, skill_a3Data> datas = Skill_a3Model.getInstance().skilldic;
        Dictionary<int, GameObject> dicobj = new Dictionary<int, GameObject>();

        int times = 1;//计时器时间
        GameObject skill_image;//拖拽的图片
        GameObject commonimage = null;//创建拖拽的图片
        GameObject image;
        GameObject contain;
        GameObject teackAni_go;
        Text skill_name;
        Text skill_lv;
        Text skill_des;
        Text skill_nowdes;
        Text skill_nextdes;
        Text needlvandzhuan;
        Text needmoneys;
        Button addlvbtn;
        GameObject iconHit;
        //Text needstudymoney;
        //Text needstudypoint;
        Text havepoint;
        GameObject group1;
        GameObject group2;
        //符文~~~~~~~~~~~~~~
        int nowid = 0;

        //int refresh_time = int.MaxValue;//倒计时时间
        Dictionary<int, runeData> runedatas = Skill_a3Model.getInstance().runedic;
        Dictionary<int, GameObject> runeobj = new Dictionary<int, GameObject>();
        Dictionary<int, int> info_time = new Dictionary<int, int>();
        bool istimein = false;
        Dictionary<int, int> info_times = new Dictionary<int, int>();
        GameObject imagerune0;
        GameObject panel0;
        GameObject imagerune1;
        GameObject panel1;
        GameObject imagerune2;
        GameObject panel2;


        GameObject runeinfosObj;
        Text runename;
        Text runelv;
        Text runedes;
        Text term;
        Text term_time;
        GameObject trem_timeImage;
        GameObject studybtn;
        GameObject costgoldbtn;
        GameObject costgoldbtn_free;
        GameObject costgoldbtn_nofree;

        GameObject go;
        GameObject gobefor;
        GameObject gotext;
      //  GameObject fx_sjcg;
        Transform father;
        Transform father2;

        Text costgold_num;
        EventTriggerListener tab1;
        EventTriggerListener tab2;
        GameObject rune_open;
        public static List<int> skills = new List<int>();
        public static Variant showdic = new Variant();
        public static Variant showdic2 = new Variant();

        int sid;
        int nowlv;
        //int nomojing;

        public static skill_a3 _instance;
        public static bool show_teack_ani = false;
        public static int upgold;
        public static int upmojing;
        int needmojingnum;
        bool isfull = false;
        private uint needobj_skillId;
        private int neednum_skill;
        public bool toshilian = false;

        public override void init()
        {

            getGameObjectByPath("skill/panel_right/bg/Image02").SetActive(false);
            _instance = this;
            contain = transform.FindChild("skill/panel_left/skill_list/contain").gameObject;
            image = transform.FindChild("skill/panel_left/skill_list/skill").gameObject;
            skill_lv = getComponentByPath<Text>("skill/panel_right/bg/lv");
            skill_name = transform.FindChild("skill/panel_right/bg/name").GetComponent<Text>();
            skill_des = transform.FindChild("skill/panel_right/bg/Image00/des").GetComponent<Text>();
            skill_nowdes = transform.FindChild("skill/panel_right/bg/Image00/now/des").GetComponent<Text>();
            skill_nextdes = transform.FindChild("skill/panel_right/bg/Image00/next/des").GetComponent<Text>();
            needlvandzhuan = getComponentByPath<Text>("skill/panel_right/bg/Button/need0/lvandzhuan");
            needmoneys = getComponentByPath<Text>("skill/panel_right/bg/Button/need1/money");
            addlvbtn = getComponentByPath<Button>("skill/panel_right/bg/Button");
            //needstudymoney = transform.FindChild("skill/panel_right/bg/Image01/money").GetComponent<Text>();
            //needstudypoint = transform.FindChild("skill/panel_right/bg/Image01/point").GetComponent<Text>();
            havepoint = transform.FindChild("skill/panel_right/bg/Image02/point").GetComponent<Text>();
            group1 = transform.FindChild("skill/panel_right/skillgroupsone").gameObject;
            group2 = transform.FindChild("skill/panel_right/skillgroupstwo").gameObject;
            rune_open = transform.FindChild("rune/tishi_bg").gameObject;
            teackAni_go = transform.FindChild("teach_ani").gameObject;
            go = getGameObjectByPath("onekeyInfo/body");
            gobefor = getGameObjectByPath("showOneKey/body");
            father = getTransformByPath("onekeyInfo/bg/scroll/cont");
            father2 = getTransformByPath("showOneKey/bg/scroll/cont");
          //  fx_sjcg = GAMEAPI.ABFight_LoadPrefab("FX_uiFX_FX_ui_sjcg");//一键升级的特效

            iconHit = this.transform.FindChild("shilian/iconHit").gameObject;
            for (int i = 0; i < 4; i++)
            {
                skillgroupsone[i] = transform.FindChild("skill/panel_right/skillgroupsone/" + i).gameObject;
                EventTriggerListener.Get(skillgroupsone[i]).onDown = OnBeginDragimage;
                EventTriggerListener.Get(skillgroupsone[i]).onUp = (GameObject oe) => { onDragEndimage(oe, Vector2.zero); };
                EventTriggerListener.Get(skillgroupsone[i]).onEnter = OnEnterimage;
                EventTriggerListener.Get(skillgroupsone[i]).onExit = OnExitimage;
                EventTriggerListener.Get(skillgroupsone[i]).onDrag = onDragimage;
                EventTriggerListener.Get(skillgroupsone[i]).onDragEnd = onDragEndimage;
            }
            for (int i = 0; i < 4; i++)
            {
                skillgrouptwo[i] = transform.FindChild("skill/panel_right/skillgroupstwo/" + i).gameObject;
                EventTriggerListener.Get(skillgrouptwo[i]).onDown = OnBeginDragimage;
                EventTriggerListener.Get(skillgrouptwo[i]).onUp = (GameObject oe) => { onDragEndimage(oe, Vector2.zero); };
                EventTriggerListener.Get(skillgrouptwo[i]).onEnter = OnEnterimage;
                EventTriggerListener.Get(skillgrouptwo[i]).onExit = OnExitimage;
                EventTriggerListener.Get(skillgrouptwo[i]).onDrag = onDragimage;
                EventTriggerListener.Get(skillgrouptwo[i]).onDragEnd = onDragEndimage;
            }
            BaseButton bt_close = new BaseButton(transform.FindChild("close"));
            bt_close.onClick = onClose;
            BaseButton btnpanel1 = new BaseButton(transform.FindChild("panel/skill"));
            btnpanel1.onClick = onOpskill;
            BaseButton btnpanel2 = new BaseButton(transform.FindChild("panel/rune"));
            btnpanel2.onClick = onOprune;
            tab1 = this.getEventTrigerByPath("skill/panel_right/tab1");
            tab1.onClick = onTabone;
            tab2 = this.getEventTrigerByPath("skill/panel_right/tab2");
            tab2.onClick = onTabtwo;
            creatrve();
            //符文~~~~~~~~~
            imagerune0 = transform.FindChild("rune/bg00/rune_attack").gameObject;
            panel0 = transform.FindChild("rune/bg00/panel").gameObject;
            imagerune1 = transform.FindChild("rune/bg01/rune_defense").gameObject;
            panel1 = transform.FindChild("rune/bg01/panel").gameObject;
            imagerune2 = transform.FindChild("rune/bg02/rune_life").gameObject;
            panel2 = transform.FindChild("rune/bg02/panel").gameObject;
            runeinfosObj = transform.FindChild("rune/runeinfo").gameObject;
            runename = runeinfosObj.transform.FindChild("name").GetComponent<Text>();
            runelv = runeinfosObj.transform.FindChild("lv").GetComponent<Text>();
            runedes = runeinfosObj.transform.FindChild("Image/Image/effect/des").GetComponent<Text>();
            term = runeinfosObj.transform.FindChild("Image/Image/term/term_txt").GetComponent<Text>();
            term_time = runeinfosObj.transform.FindChild("Image/Image/term/termtime_txt").GetComponent<Text>();
            trem_timeImage = runeinfosObj.transform.FindChild("Image/Image/term/Image").gameObject;
            studybtn = runeinfosObj.transform.FindChild("studybtn").gameObject;
            costgoldbtn = runeinfosObj.transform.FindChild("costgoldbtn").gameObject;
            costgoldbtn_free = costgoldbtn.transform.FindChild("free").gameObject;
            costgoldbtn_nofree = costgoldbtn.transform.FindChild("nofree").gameObject;
            costgold_num = costgoldbtn_nofree.transform.FindChild("num").GetComponent<Text>();
            creatrverune();
            BaseButton btns = new BaseButton(runeinfosObj.transform.FindChild("costgoldbtn").transform);
            btns.onClick = onAddspeedClick;
            BaseButton btnss = new BaseButton(runeinfosObj.transform.FindChild("studybtn").transform);
            btnss.onClick = onStudyRuneClick;
            int job = PlayerModel.getInstance().profession * 1000;
            nils(skills);
            new BaseButton(getTransformByPath("showOneKey/yes")).onClick = (GameObject go) =>//确定一键升级
              {
                  oldmojing = a3_BagModel.getInstance().getItemNumByTpid(1540);
                  Variant ski = new Variant() ;
                  for (int i = 0; i < skills.Count; i++)
                  {
                      if (skills[i] != job + 1)
                          getGameObjectByPath("skill/panel_left/skill_list/contain/" + skills[i] + "/addlv").SetActive(false);
                  }
                  int k = 0;
                  foreach (var item in skill)
                  {
                      Variant skilow = new Variant();
                      skilow["skill_id"] = item.Key;
                      skilow["add_lvl"] = item.Value;
                      ski.pushBack(skilow);
                      k++;
                  }
                  Skill_a3Proxy.getInstance().sendSkillsneed(ski);
                  getGameObjectByPath("showOneKey").SetActive(false);
                  getTransformByPath("skill/panel_left/onekey").gameObject.SetActive(true);
              };

            new BaseButton(getTransformByPath("shilian")).onClick = (GameObject go) => {
                toshilian = true;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_YGYIWU);
            };

            new BaseButton(getTransformByPath("skill/panel_left/onekey")).onClick = (GameObject go) =>//一键升级所需
            {
                Variant ski = new Variant();
                skills.Sort();
                for (int i = 0; i < skills.Count; i++)
                {
                    ski[i] = skills[i];
                }
                oneKey(skills);
                if (isfull)
                {
                    flytxt.instance.fly(ContMgr.getCont("skill_a3_maxlv"));
                    return;
                }
                if (!canlevel)
                {
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_nolv"));
                }
              //  else if (!canmojing)
              //  {
                   // flytxt.instance.fly(ContMgr.getCont("skill_a3_enoughmj"));
                   // addtoget(a3_BagModel.getInstance().getItemDataById(1540));
              //  }
                else if (!canmoney)
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_nomoney"));
                }
                else
                {
                    onekeyUp(skills);
                    showUP(skill);
                    getTransformByPath("skill/panel_left/onekey").gameObject.SetActive(false);
                }
            };
            Skill_a3Proxy.getInstance().sendProxys(1);
            
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.RUNE))
            {
                setRune_open();
                getGameObjectByPath("panel/lockp").SetActive(false);
            }
            else
            {
                getGameObjectByPath("panel/lockp").SetActive(true);
            }
            new BaseButton(getTransformByPath("panel/lockp")).onClick = (GameObject go) =>
              {
                  flytxt.instance.fly(ContMgr.getCont("func_limit_43"));
              };
            //新手指引选中技能用
            BaseButton btchooseskill = new BaseButton(transform.FindChild("newbie_btn_skill"));
            btchooseskill.onClick = onNewbie;
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.SKILLUPINFO, upSkillInfo);
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.SKILLINFO, skillInfo);
            new BaseButton(getTransformByPath("onekeyInfo")).onClick = (GameObject go) =>
              {
                  getGameObjectByPath("onekeyInfo").SetActive(false);
              };
            new BaseButton(getTransformByPath("showOneKey/back")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("showOneKey").SetActive(false);
                getTransformByPath("skill/panel_left/onekey").gameObject.SetActive(true);
            };
            new BaseButton(getTransformByPath("onekeyInfo/yes")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("onekeyInfo").SetActive(false);
            };

            needobj_skillId = (uint)XMLMgr.instance.GetSXML("skill.skill_learn_item_id").getInt("item_id");

            #region 初始化汉字提取
            getComponentByPath<Text>("skill/panel_left/onekey/Text").text = ContMgr.getCont("skill_a3_0");
            getComponentByPath<Text>("skill/panel_right/bg/Image00/next").text = ContMgr.getCont("skill_a3_1");
            getComponentByPath<Text>("skill/panel_right/bg/Image00/now").text = ContMgr.getCont("skill_a3_2");
            getComponentByPath<Text>("skill/panel_right/bg/Button/need1/Text").text = ContMgr.getCont("skillup_summon_1");
            getComponentByPath<Text>("skill/panel_right/bg/Button/need0/lvandzhuan").text = ContMgr.getCont("skill_a3_3");
            getComponentByPath<Text>("skill/panel_right/tab1/Text").text = ContMgr.getCont("skill_a3_4");
            getComponentByPath<Text>("skill/panel_right/tab2/Text").text = ContMgr.getCont("skill_a3_5");
            getComponentByPath<Text>("rune/runeinfo/Image/Image/effect").text = ContMgr.getCont("skill_a3_6");
            getComponentByPath<Text>("rune/runeinfo/Image/Image/term").text = ContMgr.getCont("skill_a3_7");
            getComponentByPath<Text>("rune/runeinfo/studybtn/Text").text = ContMgr.getCont("skill_a3_8");
            getComponentByPath<Text>("rune/runeinfo/costgoldbtn/nofree/Text").text = ContMgr.getCont("skill_a3_9");
            getComponentByPath<Text>("rune/runeinfo/costgoldbtn/free").text = ContMgr.getCont("skill_a3_10");
            getComponentByPath<Text>("rune/runeinfo/title/info").text = ContMgr.getCont("skill_a3_11");
            getComponentByPath<Text>("rune/tishi_bg/goget/Text (1)").text = ContMgr.getCont("skill_a3_12");
            getComponentByPath<Text>("panel/skill/Image0/Text").text = ContMgr.getCont("skill_a3_13");
            getComponentByPath<Text>("panel/skill/Image1/Text").text = ContMgr.getCont("skill_a3_13");
            getComponentByPath<Text>("panel/rune/Image0/Text").text = ContMgr.getCont("skill_a3_14");
            getComponentByPath<Text>("panel/rune/Image1/Text").text = ContMgr.getCont("skill_a3_14");
            getComponentByPath<Text>("teach_ani/Text").text = ContMgr.getCont("skill_a3_15");
            getComponentByPath<Text>("onekeyInfo/bg/up").text = ContMgr.getCont("skill_a3_16");
            getComponentByPath<Text>("onekeyInfo/yes/yes").text = ContMgr.getCont("skill_a3_17");
            getComponentByPath<Text>("showOneKey/bg/up").text = ContMgr.getCont("skill_a3_18");
            getComponentByPath<Text>("showOneKey/yes/yes").text = ContMgr.getCont("skill_a3_17");
            getComponentByPath<Text>("panel/rune/Image1/Text").text = ContMgr.getCont("skill_a3_14");
            #endregion
        }

        void showiconHit()
        {
            if (a3_ygyiwuModel.getInstance().canToNowPre())
            {
                iconHit.SetActive(true);
            }
            else
            {
                iconHit.SetActive(false);
            }

        }
        void upSkillInfo(GameEvent e)
        {
            debug.Log("Screen.size:(" + Screen.width + "," + Screen.height + ")");
           // GameObject sjcg=Instantiate(fx_sjcg);
           // float width = (float)Screen.width / 831;
           // float height = (float)Screen.height / 459;
            SXML xml;
            int nowlvl;
            int id;
            Transform isthis = transform.FindChild("skill/panel_left/skill_list/contain");
            //sjcg.transform.SetParent(this.transform);
           // sjcg.transform.localPosition = Vector3.zero;
           // sjcg.transform.localScale = new Vector3(width,height,1);
           // Destroy(sjcg,2f);
            nils(skills);
            flytxt.instance.fly(ContMgr.getCont("a3_summon_sjcg"));
            foreach (var item in skill)
            {
                id = item.Key;
                xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + (item.Key).ToString());
                nowlvl = datas[item.Key].now_lv + item.Value;
                datas[item.Key].now_lv = nowlvl;

                if (xml.GetNode("skill_att", "skill_lv==" + nowlvl.ToString()) != null && isthis.FindChild(id + "/isthis").gameObject.activeSelf == true)
                {
                    skill_lv.text = "" + nowlvl;
                    if (xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()) != null)
                    {
                        havepoint.text = xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("item_num") + "";
                        needmoneys.text = xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("money").ToString();
                        skill_nextdes.text = xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getString("descr2");
                    }
                    else
                    {
                        needmoneys.transform.parent.gameObject.SetActive(false);
                        needlvandzhuan.transform.parent.gameObject.SetActive(true);
                        skill_nextdes.text = ContMgr.getCont("skill_a3_maxlv");
                        needlvandzhuan.text = ContMgr.getCont("skill_a3_maxlv");
                        havepoint.text = ContMgr.getCont("skill_a3_maxlv");
                        continue;
                    }


                    if (PlayerModel.getInstance().up_lvl > xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_zhuan"))
                    {
                        needmoneys.transform.parent.gameObject.SetActive(true);
                        needlvandzhuan.transform.parent.gameObject.SetActive(false);
                        lvorzhuan = false;
                        moneyormojingEnough(xml, item.Key);
                    }
                    else if (PlayerModel.getInstance().up_lvl == xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_zhuan"))
                    {
                        if (PlayerModel.getInstance().lvl >= xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_lvl"))
                        {
                            needmoneys.transform.parent.gameObject.SetActive(true);
                            needlvandzhuan.transform.parent.gameObject.SetActive(false);
                            lvorzhuan = false;
                            moneyormojingEnough(xml, item.Key);

                        }
                        else
                        {
                            needmoneys.transform.parent.gameObject.SetActive(false);
                            needlvandzhuan.transform.parent.gameObject.SetActive(true);
                            lvorzhuan = true;
                            needlvandzhuan.text = "<color=#ff0000>" + xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_zhuan") + ContMgr.getCont("zhuan") + xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_lvl") + ContMgr.getCont("ji") + "</color>";

                        }
                    }
                    else
                    {
                        needmoneys.transform.parent.gameObject.SetActive(false);
                        needlvandzhuan.transform.parent.gameObject.SetActive(true);
                        lvorzhuan = true;
                        needlvandzhuan.text = "<color=#ff0000>" + xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_zhuan") + ContMgr.getCont("zhuan") + xml.GetNode("skill_att", "skill_lv==" + (nowlvl + 1).ToString()).getInt("open_lvl") + ContMgr.getCont("ji") + "</color>";

                    }
                }
                else if (xml.GetNode("skill_att", "skill_lv==" + nowlvl.ToString()) == null && isthis.FindChild(id + "/isthis").gameObject.activeSelf == true)
                {
                    needmoneys.transform.parent.gameObject.SetActive(false);
                    needlvandzhuan.transform.parent.gameObject.SetActive(true);
                    skill_nextdes.text = ContMgr.getCont("skill_a3_maxlv");
                    needlvandzhuan.text = ContMgr.getCont("skill_a3_maxlv");
                    havepoint.text = ContMgr.getCont("skill_a3_maxlv");
                    neednum_skill = -1;
                }
            }
        }
        void showUP(Dictionary<int, int> canskills)
        {

            getGameObjectByPath("showOneKey").SetActive(true);
            if (father2.childCount > 1)
            {
                for (int i = 0; i < father2.childCount; i++)
                {
                    Destroy(father2.GetChild(i).gameObject);
                }
            }
            SXML xml;
            Transform isthis = transform.FindChild("skill/panel_left/skill_list/contain");
            father2.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, (canskills.Count + 2) * father2.transform.GetComponent<GridLayoutGroup>().cellSize.y);
            foreach (var item in canskills)
            {
                xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + (item.Key).ToString());
                gotext = Instantiate(gobefor);
                gotext.transform.SetParent(father2);
                gotext.transform.localScale = Vector3.one;
                gotext.SetActive(true);
                gotext.transform.GetComponent<Text>().text = datas[item.Key].skill_name + ContMgr.getCont("skill_a3_tisheng") + item.Value + ContMgr.getCont("ji");
            }
            gotext = Instantiate(gobefor);
            gotext.transform.SetParent(father2);
            gotext.transform.localScale = Vector3.one;
            gotext.SetActive(true);
            gotext.transform.GetComponent<Text>().text = ContMgr.getCont("skill_a3_jbsj") + sumMonry;
            //gotext = Instantiate(gobefor);
            //gotext.transform.SetParent(father2);
            //gotext.transform.localScale = Vector3.one;
            //gotext.SetActive(true);
            //gotext.transform.GetComponent<Text>().text = ContMgr.getCont("skill_a3_mjsj") + sumMojing;

            needmojingnum = sumMojing;
        }
        void skillInfo(GameEvent e)
        {
            int id = e.data["skid"];
            datas[id].now_lv = e.data["sklvl"];
        }

        void onNewbie(GameObject go)
        {
            if (contain.transform.childCount > 1)
            {
                if (PlayerModel.getInstance().lvl > 5)
                    OnBeginDrag(contain.transform.GetChild(1).gameObject);
                else
                    OnBeginDrag(contain.transform.GetChild(0).gameObject);
            }
        }
        
        public override void onShowed()
        {
            onOpskill(null);
            openrefreshskillinfo();
            showCanStudy();
            onTabone(transform.FindChild("skill/panel_right/tab1").gameObject);
            //if (a3_herohead.instance.isshowskill)
            //{
            showLevelupImage();
            //a3_herohead.instance.isshowskill = false;
            //}
            //符文~~~~~~~~
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.RUNEINFOS, RuneInfos);
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.RUNERESEARCH, onResearch);
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.RUNEADDSPEED, onAddspeed);
            Skill_a3Proxy.getInstance().addEventListener(Skill_a3Proxy.RUNERESEARCHOVER, onResearchOver);
            RefreshLockInfo();
            onshoeInfos(100);
            showskilldatas(fristid);
            isthisImage();
            dicobj[fristid].transform.FindChild("isthis").gameObject.SetActive(true);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.RUNE))
            {
                getGameObjectByPath("panel/lockp").SetActive(false);
            }
            toshilian = false;
            showTeachAni(show_teack_ani);
            getGameObjectByPath("skill/panel_right/tab2/lock").SetActive(A3_TaskModel.getInstance().main_task_id >=626?false:true);
            UiEventCenter.getInstance().onWinOpen(uiName);
            showiconHit();
        }
        public override void onClosed()
        {
            isthisImage();
            sendgroups();
            canStudySkill.Clear();
            //符文~~~~~~~~~
            Skill_a3Proxy.getInstance().removeEventListener(Skill_a3Proxy.RUNEINFOS, RuneInfos);
            Skill_a3Proxy.getInstance().removeEventListener(Skill_a3Proxy.RUNERESEARCH, onResearch);
            Skill_a3Proxy.getInstance().removeEventListener(Skill_a3Proxy.RUNEADDSPEED, onAddspeed);
            Skill_a3Proxy.getInstance().removeEventListener(Skill_a3Proxy.RUNERESEARCHOVER, onResearchOver);
            if (commonimage != null)
            {
                Destroy(commonimage.gameObject);
            }
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);

            if (a3_expbar.instance != null)
                a3_expbar.instance.RemoveLightTip("newskill");

            show_teack_ani = false;
            A3_BeStronger.Instance.CheckUpItem();
            a1_gamejoy.inst_joystick.playselfSkills(a1_gamejoy.inst_joystick.NowSkillIndex);
           // playselfSkills();
        }
        #region

        //自动战斗技能
        //AutoPlayModel apModel;
        //bool isone = true;
        //void playselfSkills()
        //{
        //    apModel = AutoPlayModel.getInstance();
        //    if(isone)
        //    {
        //        for(int i=0;i<Skill_a3Model.getInstance().idsgroupone.Length;i++)
        //        {
        //            if(Skill_a3Model.getInstance().idsgroupone[i]!=0)
        //            {
        //                if (Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgroupone[i]].skillType2 != 1 ||
        //                    Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgroupone[i]].skillType2 != 2)
        //                    apModel.Skills[i] = Skill_a3Model.getInstance().idsgroupone[i];
        //                else
        //                    apModel.Skills[i] = 0;
        //            }else
        //            {
        //                apModel.Skills[i] = 0;
        //            }

        //        }
        //    }else
        //    {
        //        for (int i = 0; i < Skill_a3Model.getInstance().idsgrouptwo.Length; i++)
        //        {
        //            if(Skill_a3Model.getInstance().idsgrouptwo[i]!=0)
        //            {
        //                if (Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgrouptwo[i]].skillType2 != 1 ||
        //                    Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgrouptwo[i]].skillType2 != 2)
        //                    apModel.Skills[i] = Skill_a3Model.getInstance().idsgrouptwo[i];
        //                else
        //                    apModel.Skills[i] = 0;
        //            }else
        //                apModel.Skills[i] = 0;

        //        }
        //    }
            
        //}
        //升级刷新
        public void uprefreshskillinfo(int id, int lv)
        {
            dicobj[id].transform.FindChild("lv").GetComponent<Text>().text = "" + datas[id].now_lv;
            showskilldatas(id);
            EventTriggerListener.Get(dicobj[id]).onDown = OnBeginDrag;
            EventTriggerListener.Get(dicobj[id]).onDrag = OnDrag;
            EventTriggerListener.Get(dicobj[id]).onDragEnd = OnDragEnd;
            EventTriggerListener.Get(dicobj[id]).onUp = OnUpiamge;
            if (transform.FindChild("skill/panel_right/skillgroupsone").gameObject.activeSelf)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (skillgroupsone[i].transform.childCount > 0 && skillgroupsone[i].transform.GetChild(0).name == id.ToString())
                    {
                        getComponentByPath<Text>("skill/panel_right/lv" + i).text = "" + lv;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (skillgrouptwo[i].transform.childCount > 0 && skillgrouptwo[i].transform.GetChild(0).name == id.ToString())
                    {
                        getComponentByPath<Text>("skill/panel_right/lv" + i).text = "" + lv;
                    }
                }
            }
        }
        public void showTeachAni(bool show)
        {
            if (show)
                teackAni_go.SetActive(true);
            else
                teackAni_go.SetActive(false);
        }

        void OnBeginDrag(GameObject go)
        {
            if (commonimage != null)
            {
                Destroy(commonimage);
                commonimage = null;
            }
            isthisImage();
            go.transform.FindChild("isthis").gameObject.SetActive(true);
            //if (go.transform.FindChild("addlv").gameObject.activeSelf)
            //{
            //go.transform.FindChild("addlv").gameObject.SetActive(false);
            //}
            commonimage = GameObject.Instantiate(go.transform.FindChild("skill_image").gameObject) as GameObject;
            commonimage.name = go.name;
            commonimage.transform.SetParent(transform, false);
            commonimage.SetActive(false);

            var si = go.transform.FindChild("skill_image").GetComponent<UIMoveToPoint>();
            if (si != null)
            {
                si.Kill();
            }

            if (datas[int.Parse(commonimage.name)].now_lv <= 0)
            {
                if (commonimage != null)
                {
                    showskilldatas(int.Parse(commonimage.name));
                }

                Destroy(commonimage);
                commonimage = null;
            }
            else
            {
                commonimage.SetActive(true);
                commonimage.GetComponent<CanvasGroup>().interactable = false;
                commonimage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                commonimage.transform.localPosition = new Vector3((Input.mousePosition.x - 0.5f * Screen.width) * cemaraRectTran.rect.width / Screen.width, (Input.mousePosition.y - 0.5f * Screen.height) * cemaraRectTran.rect.height / Screen.height, 0.0f);
                //print("鼠标的坐标x：" + Input.mousePosition.x +"y:" +Input.mousePosition.y);
                //print("屏幕坐标x：" + Screen.width + "y:"+Screen.height);
                //print("摄像机矩阵x:" + cemaraRectTran.rect.width + "y:" + cemaraRectTran.rect.height);
            }
        }
        void OnDrag(GameObject go, Vector2 delta)
        {
            if (commonimage == null)
                return;
            commonimage.transform.localPosition = new Vector3((Input.mousePosition.x - 0.5f * Screen.width) * cemaraRectTran.rect.width / Screen.width, (Input.mousePosition.y - 0.5f * Screen.height) * cemaraRectTran.rect.height / Screen.height, 0.0f);
            //commonimage.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        }
        void OnDragEnd(GameObject go, Vector2 delta)
        {
            if (commonimage != null)
            {
                if (num != -1)
                {
                    GameObject objClone = GameObject.Instantiate(commonimage) as GameObject;
                    objClone.name = commonimage.name;
                    if (num <= 3)
                        deletesame(num, skillgroupsone, objClone);
                    else
                        deletesame(num - 4, skillgrouptwo, objClone);

                    //新手指引拖拽技能结束
                    UiEventCenter.getInstance().onSkillDrawEnd();
                    show_teack_ani = false;
                    showTeachAni(false);
                }
                else
                    showskilldatas(int.Parse(commonimage.name));
            }
            Destroy(commonimage);
            commonimage = null;
        }

        GameObject gos = null;
        void OnBeginDragimage(GameObject go)
        {
            if (commonimage != null)
            {
                Destroy(commonimage);
                commonimage = null;
            }
            if (go.transform.childCount > 0)
            {
                gos = go.transform.GetChild(0).gameObject;
                commonimage = GameObject.Instantiate(go.transform.GetChild(0).gameObject) as GameObject;
                commonimage.name = go.transform.GetChild(0).name;
                commonimage.transform.SetParent(transform, false);
                showskilldatas(int.Parse(commonimage.name));
                commonimage.SetActive(true);
                isdragimage = true;
                commonimage.GetComponent<CanvasGroup>().interactable = false;
                commonimage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                commonimage.transform.localPosition = new Vector3((Input.mousePosition.x - 0.5f * Screen.width) * cemaraRectTran.rect.width / Screen.width, (Input.mousePosition.y - 0.5f * Screen.height) * cemaraRectTran.rect.height / Screen.height, 0.0f);
            }
            else
                return;
        }
        bool isdragimage = false;// 在不在拖拽
        void onDragimage(GameObject go, Vector2 delta)
        {
            if (go.transform.childCount == 0)
                return;
            commonimage.transform.localPosition = new Vector3((Input.mousePosition.x - 0.5f * Screen.width) * cemaraRectTran.rect.width / Screen.width, (Input.mousePosition.y - 0.5f * Screen.height) * cemaraRectTran.rect.height / Screen.height, 0.0f);
        }
        GameObject goss;

        void OnUpiamge(GameObject go)
        {
            OnDragEnd(go, Vector2.zero);
        }
        void onDragEndimage(GameObject go, Vector2 delta)
        {
            if (go.transform.childCount <= 0)
            {
                return;
            }
            if (gos != null)
            {
                goss = gos.transform.parent.gameObject;
            }

            if (isdragimage)
            {
                isdragimage = false;
                //print("name shi :" + num);
                if (num != -1)
                {
                    if (num < 4)
                    {
                        if (skillgroupsone[num].transform.childCount > 0)
                            swapposition(skillgroupsone, num);
                        else
                        {
                            groupids(skillgroupsone, int.Parse(goss.name), false, null);//把我移的变成空
                            getComponentByPath<Text>("skill/panel_right/lv" + goss.name).text = "";
                            gos.transform.SetParent(skillgroupsone[num].transform, false);
                            getComponentByPath<Text>("skill/panel_right/lv" + num).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(gos.name)].now_lv;
                            gos.transform.localPosition = new Vector3(0, 0, 0);
                            groupids(skillgroupsone, num, true, gos);
                        }
                    }
                    else
                    {
                        num = num - 4;
                        if (skillgrouptwo[num].transform.childCount > 0)
                            swapposition(skillgrouptwo, num);
                        else
                        {
                            groupids(skillgrouptwo, int.Parse(goss.name), false, null);
                            getComponentByPath<Text>("skill/panel_right/lv" + goss.name).text = "";
                            gos.transform.SetParent(skillgrouptwo[num].transform, false);
                            getComponentByPath<Text>("skill/panel_right/lv" + num).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(gos.name)].now_lv;
                            gos.transform.localPosition = new Vector3(0, 0, 0);
                            groupids(skillgrouptwo, num, true, gos);
                        }
                    }
                }
                else
                {
                    if (group1.gameObject.activeSelf)
                    {
                        groupids(skillgroupsone, int.Parse(goss.name), false, null);
                        getComponentByPath<Text>("skill/panel_right/lv" + goss.name).text = "";
                    }
                    else
                    {
                        groupids(skillgrouptwo, int.Parse(goss.name), false, null);
                        getComponentByPath<Text>("skill/panel_right/lv" + goss.name).text = "";
                    }
                    Destroy(gos);
                    gos = null;

                }

            }
            else
            {
                if(commonimage!=null)
                    showskilldatas(int.Parse(commonimage.name));
                if (commonimage != null)
                    Destroy(commonimage.gameObject);
            }
            Destroy(commonimage);
            commonimage = null;
        }
        int num = -1;
        void OnEnterimage(GameObject go)
        {
            if (go == skillgroupsone[0])
                num = 0;
            else if (go == skillgroupsone[1])
                num = 1;
            else if (go == skillgroupsone[2])
                num = 2;
            else if (go == skillgroupsone[3])
                num = 3;
            else if (go == skillgrouptwo[0])
                num = 4;
            else if (go == skillgrouptwo[1])
                num = 5;
            else if (go == skillgrouptwo[2])
                num = 6;
            else if (go == skillgrouptwo[3])
                num = 7;
            else
                num = -1;
        }
        void OnExitimage(GameObject go)
        {
            num = -1;
        }


        //点击显示该技能的信息
        bool lvorzhuan = false;
        bool needmoney = false;
        bool needmojing = false;

        bool canlevel = false;
        bool canmoney = false;
        bool canmojing = false;
        void moneyormojingEnough(SXML xml, int id)
        {

            if (PlayerModel.getInstance().money >= xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("money"))
                needmoney = true;
            else
                needmoney = false;
            needmoneys.text = xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("money").ToString();
            // addlvbtn.interactable = needmoney ? true : false;
            //if (needmoney)
            //{
            //    addlvbtn.interactable = true;
            //    //needstudypoint.text = "<color=#00FF00>" + "升级所需魔镜：" + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("item_num") + "</color>";
            //}
            //else
            //{
            //    //needstudymoney.text = "<color=#ff0000>" + "升级所需金币：" + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("money") + "</color>";
            //    //needstudypoint.text = "<color=#ff0000>" + "升级所需魔镜：" + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("item_num") + "</color>";
            //}
        }
        void showskilldatas(int id, int j = 0)
        {
            // print("玩家的等级：" + PlayerModel.getInstance().lvl);
            int ids = id;
            skill_name.text = datas[id].skill_name.ToString();
            skill_lv.text = "" + datas[id].now_lv;
            skill_des.text = datas[id].des;
            SXML xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + id.ToString());
            if (datas[id].now_lv != 0)
                skill_nowdes.text = xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv).ToString()).getString("descr2");
            else
                skill_nowdes.text = "-";
            int numbymojing = a3_BagModel.getInstance().getItemNumByTpid(1540);


            if (xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()) != null)
            {
                if (numbymojing >= xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("item_num"))
                    needmojing = true;
                else
                    needmojing = false;
                havepoint.text = xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("item_num") + "";
                neednum_skill = xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("item_num");
                skill_nextdes.text = xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getString("descr2");
                if (PlayerModel.getInstance().up_lvl > xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_zhuan"))
                {
                    needmoneys.transform.parent.gameObject.SetActive(true);
                    needlvandzhuan.transform.parent.gameObject.SetActive(false);
                    lvorzhuan = false;
                    moneyormojingEnough(xml, id);


                }
                else if (PlayerModel.getInstance().up_lvl == xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_zhuan"))
                {
                    if (PlayerModel.getInstance().lvl >= xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_lvl"))
                    {
                        needmoneys.transform.parent.gameObject.SetActive(true);
                        needlvandzhuan.transform.parent.gameObject.SetActive(false);
                        lvorzhuan = false;
                        moneyormojingEnough(xml, id);

                    }
                    else
                    {
                        needmoneys.transform.parent.gameObject.SetActive(false);
                        needlvandzhuan.transform.parent.gameObject.SetActive(true);
                        lvorzhuan = true;
                        needlvandzhuan.text = "<color=#ff0000>" + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_zhuan") + ContMgr.getCont("zhuan") + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_lvl") + ContMgr.getCont("ji") + "</color>";
                        //addlvbtn.interactable = false;
                        //needstudypoint.text = " ";
                    }
                }
                else
                {
                    needmoneys.transform.parent.gameObject.SetActive(false);
                    needlvandzhuan.transform.parent.gameObject.SetActive(true);
                    lvorzhuan = true;
                    needlvandzhuan.text = "<color=#ff0000>" + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_zhuan") + ContMgr.getCont("zhuan") + xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1).ToString()).getInt("open_lvl") + ContMgr.getCont("ji") + "</color>";
                    //addlvbtn.interactable = false;
                    //needstudypoint.text = " ";
                }
            }
            else
            {
                needmoneys.transform.parent.gameObject.SetActive(false);
                needlvandzhuan.transform.parent.gameObject.SetActive(true);
                //addlvbtn.interactable = false;
                skill_nextdes.text = ContMgr.getCont("skill_a3_maxlv");
                needlvandzhuan.text = ContMgr.getCont("skill_a3_maxlv");
                havepoint.text = ContMgr.getCont("skill_a3_maxlv");
                neednum_skill = -1;
            }

            BaseButton study_skill = new BaseButton(transform.FindChild("skill/panel_right/bg/Button"));
            study_skill.onClick = delegate (GameObject go)
            {
                //debug.Log(id + ":::::::::::::::::::");

                if (datas[id].now_lv <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("skill_a3_noskill"), 1);
                    //return;
                }
                if (lvorzhuan)
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_nolv"), 1);
                else
                {
                    if (needmoney == false)
                        flytxt.instance.fly(ContMgr.getCont("A3_Smithy_txt2"), 1);
                    else
                    {
                       // if (needmojing == false)
                       // {
                           // addtoget(a3_BagModel.getInstance().getItemDataById(needobj_skillId));
                           // flytxt.instance.fly(ContMgr.getCont("skill_a3_enoughmj"), 1);
                       // }
                       // else
                            Skill_a3Proxy.getInstance().sendProxy(ids, null);
                    }

                }

            };

        }

        void addtoget(a3_ItemData item)
        {
            ArrayList data1 = new ArrayList();
            data1.Add(item);
            data1.Add(InterfaceMgr.SKILL_A3);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
        }
        int oldmojing;
        void nils(List<int> skillss)
        {
            SXML xml;
            if (skillss.Count == 10)
            {
                int nils = 0;
                for (int i = 0; i < skillss.Count; i++)
                {
                    xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skillss[i].ToString());
                    SXML xml_node = xml.GetNode("skill_att", "skill_lv==" + (datas[skillss[i]].now_lv + 1).ToString());
                    if (xml_node == null)
                    {
                        nils++;
                    }
                }
                if (nils == 10)
                {
                    getButtonByPath("skill/panel_left/onekey").interactable = false;
                    getTransformByPath("skill/panel_left/onekey/Text").GetComponent<Text>().text = ContMgr.getCont("skill_a3_maxlv");
                    isfull = true;
                }
            }
        }
        void oneKey(List<int> skillss)
        {
            canlevel = false;
            canmoney = false;
            canmojing = false;
            SXML xml;
            int numbymojings;
            //=======needmojing===============
            numbymojings = a3_BagModel.getInstance().getItemNumByTpid(1540);

            int myMoney = (int)PlayerModel.getInstance().money;
            int minmoj = numbymojings + 1;
            int minmoney = myMoney + 1;
            int minuplv = (int)PlayerModel.getInstance().up_lvl + 1;
            int minlv = (int)PlayerModel.getInstance().lvl + 1;
            for (int j = 0; j < skillss.Count; j++)
            {
                xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skillss[j].ToString());
                SXML xml_node = xml.GetNode("skill_att", "skill_lv==" + (datas[skillss[j]].now_lv + 1).ToString());
                if (xml_node != null)
                {
                    //============= 等级 =============
                    if (PlayerModel.getInstance().up_lvl > xml_node.getInt("open_zhuan"))
                    {
                        minuplv = xml_node.getInt("open_zhuan");
                        //============= 魔晶 =============
                        if (numbymojings >= (datas[skillss[j]].now_lv + 1) * 2)
                        {
                            minmoj = (datas[skillss[j]].now_lv + 1) * 2;
                        }
                        //============= 金币 =============
                        if (myMoney >= (datas[skillss[j]].now_lv + 1) * 1000)
                        {
                            minmoney = xml_node.getInt("money");
                        }
                        minlv = 0;
                    }
                    else if (PlayerModel.getInstance().up_lvl == xml_node.getInt("open_zhuan"))
                    {
                        minuplv = xml_node.getInt("open_zhuan");
                        if (PlayerModel.getInstance().lvl >= xml_node.getInt("open_lvl"))
                        {
                            minlv = xml_node.getInt("open_lvl");
                            //============= 魔晶 =============
                            if (numbymojings >= (datas[skillss[j]].now_lv + 1) * 2)
                            {
                                minmoj = (datas[skillss[j]].now_lv + 1) * 2;
                            }
                            //============= 金币 =============
                            if (myMoney >= (datas[skillss[j]].now_lv + 1) * 1000)
                            {
                                minmoney = xml_node.getInt("money");
                            }
                        }
                    }
                }
            }
            if (minuplv <= (int)PlayerModel.getInstance().up_lvl && minlv <= (int)PlayerModel.getInstance().lvl) canlevel = true;
            if (minmoj <= numbymojings && canlevel) canmojing = true;
            if (minmoney <= myMoney && canlevel /*&& canmojing*/) canmoney = true;
            //Debug.LogError(canmojing + ":::canmojing:::" + canmoney + ":::canmoney:::::" + canlevel + ":::::level:::" + minmoney + ":::" + myMoney);
            //-----------------------------------
            nils(skillss);

        }
        Dictionary<int, int> skill = new Dictionary<int, int>();
        int sumMonry = 0;
        int sumMojing = 0;
        void onekeyUp(List<int> skillss)
        {
            sumMonry = 0;
            sumMojing = 0;
            int uplvls;
            int lvls;
            skill.Clear();
            SXML xml;

            int numbymojings, nummoney;
            numbymojings = a3_BagModel.getInstance().getItemNumByTpid(1540);
            nummoney = (int)PlayerModel.getInstance().money;
            Dictionary<int, Skillnextinfo> dic = SkillModel.getInstance().dic_n_info;
            for (int i = 1; i < dic.Count; i++)
            {
                for (int j = 0; j < skillss.Count; j++)
                {

                   // xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + skillss[j].ToString());
                    //SXML xml_node = xml.GetNode("skill_att", "skill_lv==" + (datas[skillss[j]].now_lv + i).ToString());
                    if (dic.ContainsKey(skillss[j]))
                    {
                        if (dic[skillss[j]].lst.ContainsKey(datas[skillss[j]].now_lv + i))
                        {
                            uplvls = dic[skillss[j]].lst[datas[skillss[j]].now_lv + i].open_zhuan;
                            lvls = dic[skillss[j]].lst[datas[skillss[j]].now_lv + i].open_lvl;
                            //uplvls = xml_node.getInt("open_zhuan");
                           // lvls = xml_node.getInt("open_lvl");
                            if (PlayerModel.getInstance().up_lvl > uplvls)
                            {
                                if (/*numbymojings >= (datas[skillss[j]].now_lv + i) * 2 && */nummoney >= (datas[skillss[j]].now_lv + i) * 1000)
                                {
                                    numbymojings -= ((datas[skillss[j]].now_lv + i) * 2);
                                    sumMojing += ((datas[skillss[j]].now_lv + i) * 2);

                                    nummoney -= (datas[skillss[j]].now_lv + i) * 1000;
                                    sumMonry += (datas[skillss[j]].now_lv + i) * 1000;

                                    if (skill.ContainsKey(skillss[j]))
                                    {
                                        skill[skillss[j]] = i;
                                    }
                                    else
                                        skill.Add(skillss[j], i);
                                }
                            }
                            else if (PlayerModel.getInstance().up_lvl == uplvls)
                            {
                                if (PlayerModel.getInstance().lvl >= lvls)
                                {
                                    if (/*numbymojings >= (datas[skillss[j]].now_lv + i) * 2 && */nummoney >= (datas[skillss[j]].now_lv + i) * 1000)
                                    {
                                        numbymojings -= ((datas[skillss[j]].now_lv + i) * 2);
                                        sumMojing += ((datas[skillss[j]].now_lv + i) * 2);

                                        nummoney -= (datas[skillss[j]].now_lv + i) * 1000;
                                        sumMonry += (datas[skillss[j]].now_lv + i) * 1000;

                                        if (skill.ContainsKey(skillss[j]))
                                        {
                                            skill[skillss[j]] = i;
                                        }
                                        else
                                        {
                                            skill.Add(skillss[j], i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                   
            }
            debug.Log(sumMonry + "::money" + sumMojing + "::mojing");
        }

        //将相同的或者本来里面有的删除
        void deletesame(int num, GameObject[] go, GameObject objClone)
        {
            for (int i = 0; i < 4; i++)
            {
                if (go[i].transform.childCount > 0 && go[i].transform.GetChild(0).name == objClone.name)
                {
                    Destroy(go[i].transform.GetChild(0).gameObject);
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "";
                    groupids(go, i, false, go[i].transform.GetChild(0).gameObject);
                }
            }
            if (go[num].transform.childCount > 0)
            {
                Destroy(go[num].transform.GetChild(0).gameObject);
                getComponentByPath<Text>("skill/panel_right/lv" + num).text = "";
                groupids(go, num, false, go[num].transform.GetChild(0).gameObject);
            }
            objClone.transform.SetParent(go[num].transform, false);
            objClone.transform.localPosition = new Vector3(0, 0, 0);
            objClone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //print("num:" + num + "id:" +objClone.name);
            getComponentByPath<Text>("skill/panel_right/lv" + num).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(objClone.name)].now_lv;
            groupids(go, num, true, objClone);
        }
        //互换位置
        void swapposition(GameObject[] go, int num)
        {
            GameObject commomgos = go[num].transform.GetChild(0).gameObject;

            //go[num].transform.GetChild(0).gameObject.transform.SetParent(gos.transform.parent.transform, false);
            commomgos.transform.SetParent(gos.transform.parent.transform, false);
            commomgos.transform.localPosition = new Vector3(0, 0, 0);
            groupids(go, int.Parse(gos.transform.parent.name), true, commomgos);
            getComponentByPath<Text>("skill/panel_right/lv" + gos.transform.parent.name).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(commomgos.name)].now_lv;

            gos.transform.SetParent(go[num].transform, false);
            gos.transform.localPosition = new Vector3(0, 0, 0);
            groupids(go, int.Parse(gos.transform.parent.name), true, gos);
            getComponentByPath<Text>("skill/panel_right/lv" + gos.transform.parent.name).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(gos.name)].now_lv;
        }
        //存两组的id数据
        void groupids(GameObject[] go, int num, bool deletoradd, GameObject gos)
        {
            if (deletoradd)
            {
                if (go == skillgroupsone)
                {
                    Skill_a3Model.getInstance().idsgroupone[num] = int.Parse(gos.name);
                    if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 1)
                        a1_gamejoy.inst_skillbar.refreSkill(num + 1, int.Parse(gos.name));
                }
                else
                {
                    Skill_a3Model.getInstance().idsgrouptwo[num] = int.Parse(gos.name);
                    if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 2)
                        a1_gamejoy.inst_skillbar.refreSkill(num + 1, int.Parse(gos.name));
                }
            }
            else
            {
                if (go == skillgroupsone)
                {
                    Skill_a3Model.getInstance().idsgroupone[num] = 0;

                    if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 1)
                        a1_gamejoy.inst_skillbar.refreSkill(num + 1, null);
                }
                else
                {
                    Skill_a3Model.getInstance().idsgrouptwo[num] = 0;
                    if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 2)
                        a1_gamejoy.inst_skillbar.refreSkill(num + 1, null);
                }
            }
        }

        public void moveAni(int skillid, int arr, int index)
        {
            var fromobj = dicobj[skillid].transform.FindChild("skill_image");
            Transform toobj = null;
            bool a = false;
            if ((arr == 1 && group1.activeSelf) || (arr == 2 && group2.activeSelf))
            {
                a = true;
                if (!group1.activeSelf) toobj = group2.transform.FindChild(index.ToString());
                else toobj = group1.transform.FindChild(index.ToString());
            }
            else
            {
                if (group1.activeSelf) toobj = tab2.transform;
                else toobj = tab1.transform;
            }
            UIMoveToPoint utp = UIMoveToPoint.Get(fromobj.gameObject);

            if (utp.dutime == 0) utp.dutime = 1f;
            if (a)
            {
                utp.endscale = Vector3.one;
                toobj.gameObject.SetActive(false);
                utp.Move(toobj.transform, () => { toobj.gameObject.SetActive(true); });
            }
            else
            {
                utp.endscale = Vector3.zero;
                utp.Move(toobj.transform);
            }

        }


        int fristid = 0;

        //打开界面刷新信息
        void creatrve()
        {
            foreach (int i in datas.Keys)
            {
                if (PlayerModel.getInstance().profession == datas[i].carr)
                {
                    if (datas[i].open_lvl == 1 && datas[i].open_zhuan == 0)
                    {
                        continue;
                    }
                    GameObject skill_imageClone = GameObject.Instantiate(image) as GameObject;
                    skill_imageClone.SetActive(true);
                    skill_imageClone.transform.SetParent(contain.transform, false);
                    skill_imageClone.name = i.ToString();
                    skill_imageClone.transform.FindChild("close_skill").gameObject.SetActive(true);
                    skill_imageClone.transform.FindChild("close_skill/lv").GetComponent<Text>().text = datas[i].open_zhuan + ContMgr.getCont("zhuan") + datas[i].open_lvl + ContMgr.getCont("a3_ygyiwu");
                    skill_image = skill_imageClone.transform.FindChild("skill_image/skill").gameObject;
                    string file = "icon_skill_" + i;
                    skill_image.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    skill_imageClone.transform.FindChild("lv").GetComponent<Text>().text = "" + "" + "0";
                    skill_imageClone.transform.FindChild("name").GetComponent<Text>().text = datas[i].skill_name;
                    if (fristid == 0)
                    {
                        fristid = i;
                    }
                    dicobj[i] = skill_imageClone;
                }
            }
        }
        public void showCanStudy()
        {
            foreach (int id in datas.Keys)
            {
                if (PlayerModel.getInstance().profession == datas[id].carr)
                {
                    //if (!(datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)){}
                    if (PlayerModel.getInstance().up_lvl > datas[id].open_zhuan)
                    {
                        if (datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)
                        {
                            continue;
                        }
                        if (datas[id].now_lv == 0)
                        {
                            dicobj[id].transform.FindChild("canStudy").gameObject.SetActive(true);
                        }
                        else
                        {
                            dicobj[id].transform.FindChild("canStudy").gameObject.SetActive(false);
                        }
                    }
                    else if (PlayerModel.getInstance().up_lvl == datas[id].open_zhuan)
                    {
                        if (PlayerModel.getInstance().lvl >= datas[id].open_lvl)
                        {
                            if (datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)
                            {
                                continue;
                            }
                            if (datas[id].now_lv == 0)
                            {
                                dicobj[id].transform.FindChild("canStudy").gameObject.SetActive(true);
                            }
                            else
                            {
                                dicobj[id].transform.FindChild("canStudy").gameObject.SetActive(false);
                            }

                        }
                    }
                }
            }
        }

        public void openrefreshskillinfo()
        {
            //等级锁
            foreach (int id in datas.Keys)
            {
                if (PlayerModel.getInstance().profession == datas[id].carr)
                {
                    //if (!(datas[id].open_lvl == 1 && datas[id].open_zhuan == 0))
                    //{
                    //    dicobj[id].transform.FindChild("lv").gameObject.SetActive(false);
                    //}

                    //if (PlayerModel.getInstance().up_lvl > datas[id].open_zhuan)
                    //{
                    if (datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)
                    {
                        continue;
                    }
                    //    lockinfos(id);

                    //}
                    //else if (PlayerModel.getInstance().up_lvl == datas[id].open_zhuan)
                    //{
                    //    if (PlayerModel.getInstance().lvl >= datas[id].open_lvl)
                    //    {
                    //        if (datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)
                    //        {
                    //            continue;
                    //        }
                    //        lockinfos(id);

                    //    }
                    //}
                    if (datas[id].now_lv != 0)
                    {
                        lockinfos(id);
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                getComponentByPath<Text>("skill/panel_right/lv" + i).text = "";
            }
            //技能的两个数组
            for (int i = 0; i < 4; i++)
            {

                if (Skill_a3Model.getInstance().idsgroupone[i] != 0)
                {
                    GameObject skill_images = GameObject.Instantiate(image.transform.FindChild("skill_image").gameObject) as GameObject;
                    string file = "icon_skill_" + Skill_a3Model.getInstance().idsgroupone[i];
                    skill_images.transform.FindChild("skill").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    skill_images.SetActive(true);
                    if (skillgroupsone[i].transform.childCount > 0)
                    {
                        Destroy(skillgroupsone[i].transform.GetChild(0).gameObject);
                    }
                    skill_images.name = Skill_a3Model.getInstance().idsgroupone[i].ToString();
                    skill_images.transform.SetParent(skillgroupsone[i].transform, false);
                    skill_images.transform.localPosition = new Vector3(0, 0, 0);
                    skill_images.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "" + Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgroupone[i]].now_lv;
                }

                if (Skill_a3Model.getInstance().idsgrouptwo[i] != 0)
                {
                    GameObject skill_images = GameObject.Instantiate(image.transform.FindChild("skill_image").gameObject) as GameObject;
                    string file = "icon_skill_" + Skill_a3Model.getInstance().idsgrouptwo[i];
                    skill_images.transform.FindChild("skill").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    if (skillgrouptwo[i].transform.childCount > 0)
                    {
                        Destroy(skillgrouptwo[i].transform.GetChild(0).gameObject);
                    }
                    skill_images.name = Skill_a3Model.getInstance().idsgrouptwo[i].ToString();
                    skill_images.transform.SetParent(skillgrouptwo[i].transform, false);
                    skill_images.transform.localPosition = new Vector3(0, 0, 0);
                    skill_images.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
        }
        void lockinfos(int id)
        {

            if (datas[id].skill_name == Skill_a3Model.getInstance().skilllst[1].skill_name || datas[id].skill_name == Skill_a3Model.getInstance().skilllst[11].skill_name || datas[id].skill_name == Skill_a3Model.getInstance().skilllst[21].skill_name)
            {
                dicobj[id].transform.FindChild("isthis").gameObject.SetActive(true);
                showskilldatas(id);
            }
            //print("id是多少：" + id);
            dicobj[id].transform.FindChild("close_skill").gameObject.SetActive(false);
            dicobj[id].transform.FindChild("lv").gameObject.SetActive(true);
            dicobj[id].transform.FindChild("lv").GetComponent<Text>().text = datas[id].now_lv + "";
            dicobj[id].transform.FindChild("name").GetComponent<Text>().text = datas[id].skill_name;
            canStudySkill[id] = datas[id].now_lv;
            EventTriggerListener.Get(dicobj[id]).onDown = OnBeginDrag;
            EventTriggerListener.Get(dicobj[id]).onDrag = OnDrag;
            EventTriggerListener.Get(dicobj[id]).onDragEnd = OnDragEnd;
            EventTriggerListener.Get(dicobj[id]).onUp = OnUpiamge;

        }
        Dictionary<int, int> canStudySkill = new Dictionary<int, int>();
        public void showLevelupImage()
        {
            //int nums = 0;
            //foreach (int id in canStudySkill.Keys)
            //{
            //    nums += canStudySkill[id];
            //}
            //foreach (int num in canStudySkill.Keys)
            //{
            //    if (canStudySkill[num] < (nums / canStudySkill.Keys.Count))
            //    {
            //        dicobj[num].transform.FindChild("addlv").gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        dicobj[num].transform.FindChild("addlv").gameObject.SetActive(false);
            //    }
            //}
            uint play_zhuan, play_lvl;
            play_zhuan = PlayerModel.getInstance().up_lvl;
            play_lvl = PlayerModel.getInstance().lvl;
            foreach (int id in datas.Keys)
            {
                if (PlayerModel.getInstance().profession == datas[id].carr)
                {
                    if (datas[id].open_lvl == 1 && datas[id].open_zhuan == 0)
                    {
                        continue;
                    }
                    List<SXML> m = datas[id].xml.GetNodeList("skill_att");
                    if (datas[id].now_lv >= m.Count)
                    {
                        dicobj[id].transform.FindChild("addlv").gameObject.SetActive(false);
                        continue;
                    }
                    var gv = datas[id].xml.GetNode("skill_att", "skill_lv==" + (datas[id].now_lv + 1));

                    if (play_zhuan > gv.getInt("open_zhuan"))
                    {
                        if (dicobj[id].transform.FindChild("canStudy").gameObject.activeSelf == false)
                        {
                            dicobj[id].transform.FindChild("addlv").gameObject.SetActive(true);
                        }
                        else { dicobj[id].transform.FindChild("addlv").gameObject.SetActive(false); }
                    }
                    else if (play_zhuan == gv.getInt("open_zhuan"))
                    {
                        if (play_lvl >= gv.getInt("open_lvl"))
                        {
                            if (dicobj[id].transform.FindChild("canStudy").gameObject.activeSelf == false)
                            {
                                dicobj[id].transform.FindChild("addlv").gameObject.SetActive(true);
                            }
                            else { dicobj[id].transform.FindChild("addlv").gameObject.SetActive(false); }
                        }
                        else
                        {
                            dicobj[id].transform.FindChild("addlv").gameObject.SetActive(false);
                        }
                    }
                    else { dicobj[id].transform.FindChild("addlv").gameObject.SetActive(false); }

                }
            }
        }


        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.SKILL_A3);
        }
        void onTabone(GameObject go)
        {
           // isone = true;
            getGameObjectByPath("skill/panel_right/tab1/Image").SetActive(true);
            getGameObjectByPath("skill/panel_right/tab2/Image").SetActive(false);
            shoeCanvasGroup();
            group1.SetActive(true);
            group2.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                if (skillgroupsone[i].transform.childCount != 0)
                {
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(skillgroupsone[i].transform.GetChild(0).name)].now_lv;
                }
                else
                {
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "";
                }
            }
        }
        void setRune_open()
        {
            transform.FindChild("rune/tishi_bg/openlvl/Text").GetComponent<Text>().text = ContMgr.getCont("func_limit_43");
            new BaseButton(transform.FindChild("rune/tishi_bg/goget")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.SKILL_A3);
                ArrayList arr = new ArrayList();
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_YGYIWU, arr);
            };
        }
        void onTabtwo(GameObject go)
        {
            //主线任务626之前，不开放2
            if (A3_TaskModel.getInstance().main_task_id <626)
            {
                flytxt.instance.fly(ContMgr.getCont("skillbar1"));
                return;
            }
            //isone = false;
            getGameObjectByPath("skill/panel_right/tab1/Image").SetActive(false);
            getGameObjectByPath("skill/panel_right/tab2/Image").SetActive(true);
            shoeCanvasGroup();
            group1.SetActive(false);
            group2.SetActive(true);
            for (int i = 0; i < 4; i++)
            {
                if (skillgrouptwo[i].transform.childCount != 0)
                {
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "" + Skill_a3Model.getInstance().skilldic[int.Parse(skillgrouptwo[i].transform.GetChild(0).name)].now_lv;
                }
                else
                {
                    getComponentByPath<Text>("skill/panel_right/lv" + i).text = "";
                }
            }
        }
        void onOpskill(GameObject go)
        {
            shoeCanvasGroup();
            getGameObjectByPath("panel/skill/Image0").SetActive(false);
            getGameObjectByPath("panel/skill/Image1").SetActive(true);
            getGameObjectByPath("panel/rune/Image0").SetActive(true);
            getGameObjectByPath("panel/rune/Image1").SetActive(false);
            transform.FindChild("skill").gameObject.SetActive(true);
            transform.FindChild("rune").gameObject.SetActive(false);
        }
        void onOprune(GameObject go)
        {

            getGameObjectByPath("panel/skill/Image0").SetActive(true);
            getGameObjectByPath("panel/skill/Image1").SetActive(false);
            getGameObjectByPath("panel/rune/Image0").SetActive(false);
            getGameObjectByPath("panel/rune/Image1").SetActive(true);
            transform.FindChild("skill").gameObject.SetActive(false);
            transform.FindChild("rune").gameObject.SetActive(true);

            if (PlayerModel.getInstance().up_lvl < Skill_a3Model.getInstance().openuplvl)
            {
                rune_open.SetActive(true);
                setTishi(1);
            }
            else if (PlayerModel.getInstance().up_lvl == Skill_a3Model.getInstance().openuplvl)
            {
                if (PlayerModel.getInstance().lvl < Skill_a3Model.getInstance().openlvl)
                {
                    rune_open.SetActive(true);
                    setTishi(1);
                }
                else
                {
                    if (ishasRune()) { rune_open.SetActive(false); }
                    else
                    {
                        rune_open.SetActive(true);
                        setTishi(2);
                    }
                }
            }
            else
            {
                if (ishasRune()) { rune_open.SetActive(false); }
                else
                {
                    rune_open.SetActive(true);
                    setTishi(2);
                }
            }

        }

        void setTishi(int type)
        {
            if (type == 1)
            {
                transform.FindChild("rune/tishi_bg/openlvl").gameObject.SetActive(true);
                transform.FindChild("rune/tishi_bg/goget").gameObject.SetActive(false);
            }
            else if (type == 2)
            {
                transform.FindChild("rune/tishi_bg/openlvl").gameObject.SetActive(false);
                transform.FindChild("rune/tishi_bg/goget").gameObject.SetActive(true);
            }
        }
        bool ishasRune()
        {
            bool has = false;
            foreach (runeData rune in Skill_a3Model.getInstance().runedic.Values)
            {
                if (rune.now_lv > 0) { has = true; break; }
            }
            return has;
        }

        void sendgroups()
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                lst.Add(Skill_a3Model.getInstance().idsgroupone[i]);

            }
            lst.AddRange(Skill_a3Model.getInstance().idsgrouptwo);
            Skill_a3Proxy.getInstance().sendProxy(0, lst);
        }
        void shoeCanvasGroup()
        {
            for (int i = 0; i < 4; i++)
            {
                getComponentByPath<CanvasGroup>("skill/panel_right/skillgroupstwo/image" + i).interactable = false;
                getComponentByPath<CanvasGroup>("skill/panel_right/skillgroupstwo/image" + i).blocksRaycasts = false;
            }
            for (int i = 0; i < 4; i++)
            {
                getComponentByPath<CanvasGroup>("skill/panel_right/skillgroupsone/image" + i).interactable = false;
                getComponentByPath<CanvasGroup>("skill/panel_right/skillgroupsone/image" + i).blocksRaycasts = false;
            }

        }
        void isthisImage()
        {
            foreach (int i in dicobj.Keys)
            {
                dicobj[i].transform.FindChild("isthis").gameObject.SetActive(false);
            }
        }
        #endregion
        #region
        //符文~~~~~~~~~
        //创建
        void creatrverune()
        {
            foreach (int i in runedatas.Keys)
            {
                if (PlayerModel.getInstance().profession == runedatas[i].carr || runedatas[i].carr == 1)
                {
                    if (runedatas[i].type == 1)
                        creatrvedatas(imagerune0, panel0, runedatas[i].id);
                    else if (runedatas[i].type == 2)
                        creatrvedatas(imagerune1, panel1, runedatas[i].id);
                    else if (runedatas[i].type == 3)
                        creatrvedatas(imagerune2, panel2, runedatas[i].id);
                }
            }

        }
        void creatrvedatas(GameObject obj, GameObject panel, int id)
        {
            GameObject objClone = GameObject.Instantiate(obj) as GameObject;
            objClone.name = id.ToString();
            objClone.SetActive(true);
            objClone.transform.SetParent(panel.transform, false);
            string file = "icon_rune_" + id;
            objClone.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
            BaseButton btn = new BaseButton(objClone.transform.FindChild("Image").transform);
            btn.onClick = delegate (GameObject go)
            {
                onshoeInfos(int.Parse(objClone.name));

            };
            runeobj[id] = objClone;
        }
        void onshoeInfos(int rune_id)
        {
            runeinfosObj.SetActive(true);
            RefreshInfos(rune_id);
            showthisrune(rune_id);
        }
        //等级锁
        void RefreshLockInfo()
        {
            foreach (int id in runeobj.Keys)
            {
                //    if (PlayerModel.getInstance().up_lvl > runedatas[id].open_zhuan)
                //    {
                //        runeobj[id].transform.FindChild("lock").gameObject.SetActive(false);
                //    }
                //    else if (PlayerModel.getInstance().up_lvl == runedatas[id].open_zhuan)
                //    {
                //        if (PlayerModel.getInstance().lvl >= runedatas[id].open_lv)
                //        {
                //            runeobj[id].transform.FindChild("lock").gameObject.SetActive(false);
                //        }
                //        else
                //        {
                //            runeobj[id].transform.FindChild("lock").gameObject.SetActive(true);
                //            runeobj[id].transform.FindChild("lock/Text").GetComponent<Text>().text = runedatas[id].open_zhuan + "转" + runedatas[id].open_lv + "级开启";
                //        }
                //    }
                //    else
                //    {
                //        runeobj[id].transform.FindChild("lock").gameObject.SetActive(true);
                //        runeobj[id].transform.FindChild("lock/Text").GetComponent<Text>().text = runedatas[id].open_zhuan + "转" + runedatas[id].open_lv + "级开启";
                //    }
                if (runedatas[id].now_lv <= 0)
                {
                    runeobj[id].transform.FindChild("lock").gameObject.SetActive(true);
                }
                else
                {
                    runeobj[id].transform.FindChild("lock").gameObject.SetActive(false);
                }
            }
        }

        //打开界面请求所有信息
        void RuneInfos(GameEvent e)
        {
            if (e.data["runes"].Length > 0)
            {
                foreach (Variant v in e.data["runes"]._arr)
                {
                    Skill_a3Model.getInstance().Reshreinfos(v["id"], v["lvl"], v["upgrade_count_down"]);
                    if (v["upgrade_count_down"] > 0)
                    {
                        info_time[v["id"]] = v["upgrade_count_down"] + 1;
                        SXML xml = XMLMgr.instance.GetSXML("rune.rune", "id==" + v["id"]);
                        int xmls = xml.GetNode("level", "lv==" + (v["lvl"] + 1)).getInt("time_cost");
                        info_times[v["id"]] = xmls;

                        print("给我的id和等级:" + v["id"] + "和" + v["lvl"]);
                        if (runeobj.ContainsKey(v["id"]))
                        {
                            //runeobj[v["id"]].transform.FindChild("time").gameObject.SetActive(false);
                            runeobj[v["id"]].transform.FindChild("image").gameObject.SetActive(true);
                            InvokeRepeating("timess", 0, 1);
                            istimein = true;
                        }
                        else
                        {
                            runeobj[v["id"]].transform.FindChild("image").gameObject.SetActive(false);
                            //runeobj[v["id"]].transform.FindChild("time").gameObject.SetActive(false);
                        }
                    }
                }
            }
            onshoeInfos(100);
            RefreshLockInfo();
        }
        //开始研究
        void onResearch(GameEvent e)
        {

            //Skill_a3Model.getInstance().Reshreinfos(e.data["id"], -1, e.data["upgrade_count_down"]);
            info_time[e.data["id"]] = e.data["upgrade_count_down"] + 1;
            info_times[e.data["id"]] = e.data["upgrade_count_down"] + 1;
            RefreshInfos(e.data["id"]);
            if (istimein == false)
            {
                InvokeRepeating("timess", 0, 1);
                istimein = true;
            }
            runeobj[e.data["id"]].transform.FindChild("image").gameObject.SetActive(true);
            //if (runeobj.ContainsKey(e.data["id"]))
            //    runeobj[e.data["id"]].transform.FindChild("time").gameObject.SetActive(true);
            //else
            //    runeobj[e.data["id"]].transform.FindChild("time").gameObject.SetActive(false);

        }
        //加速结果
        void onAddspeed(GameEvent e)
        {
            info_time[e.data["id"]] = 0;
            info_time.Remove(e.data["id"]);
            runeobj[e.data["id"]].transform.FindChild("progress").GetComponent<Image>().fillAmount = 0;
            RefreshInfos(e.data["id"]);
            runeobj[e.data["id"]].transform.FindChild("image").gameObject.SetActive(false);
            //if (runeobj[e.data["id"]].transform.FindChild("time").gameObject.activeSelf)
            //{
            //    runeobj[e.data["id"]].transform.FindChild("time").gameObject.SetActive(false);
            //}
            studybtn.SetActive(true);
            costgoldbtn.SetActive(false);
        }
        //研究完成
        void onResearchOver(GameEvent e)
        {
            info_time[e.data["id"]] = 0;
            info_time.Remove(e.data["id"]);
            runeobj[e.data["id"]].transform.FindChild("progress").GetComponent<Image>().fillAmount = 0;
            runeobj[e.data["id"]].transform.FindChild("image").gameObject.SetActive(false);
            //if (runeobj[e.data["id"]].transform.FindChild("time").gameObject.activeSelf)
            //{
            //    runeobj[e.data["id"]].transform.FindChild("time").gameObject.SetActive(false);
            //}
            if (term_time.gameObject.activeSelf)
            {
                term_time.gameObject.SetActive(false);
                trem_timeImage.SetActive(false);
                //term.gameObject.SetActive(true);
                RefreshInfos(e.data["id"]);
            }
            studybtn.SetActive(true);
            costgoldbtn.SetActive(false);
        }
        //时间倒计时
        void timess()
        {
            int[] info_list = new int[info_time.Keys.Count];   //time
            int[] key_list = new int[info_time.Keys.Count];    //id
            int index = 0;
            if (info_time.Keys.Count == 0)
            {
                CancelInvoke("timess");
                istimein = false;
            }
            else
            {
                foreach (int key in info_time.Keys)
                {
                    info_time.TryGetValue(key, out info_list[index]);
                    key_list[index] = key;
                    index++;
                }
            }
            for (int i = 0; i < info_list.Length; i++)
            {
                info_list[i]--;
                print("id是：" + key_list[i]);
                float add_time = 1 / (float)info_times[key_list[i]];
                float nowtime = (float)info_times[key_list[i]] - info_list[i];

                if (info_list[i] > 0)
                {
                    double tm = add_time * nowtime;
                    runeobj[key_list[i]].transform.FindChild("progress").GetComponent<Image>().fillAmount = (float)Math.Round(tm, 3);
                    //print("加的时间：" + add_time + "时间差：" + nowtime + "表里的时间：" + info_times[key_list[i]] + "服务其给我的时间：" + info_list[i] + "比例：" +(float)Math.Round(tm, 3));
                    //runeobj[key_list[i]].transform.FindChild("image").gameObject.SetActive(true);
                }
                if (info_list[i] <= 0)
                {
                    info_list[i] = 0;
                    runeobj[key_list[i]].transform.FindChild("image").gameObject.SetActive(false);
                    runeobj[key_list[i]].transform.FindChild("progress").GetComponent<Image>().fillAmount = 0;
                }
                //runeobj[key_list[i]].transform.FindChild("time").GetComponent<Text>().text = Globle.formatTime(info_list[i]);
                info_time[key_list[i]] = info_list[i];
                Skill_a3Model.getInstance().runedic[key_list[i]].time = info_list[i];
            }
            if (term_time.gameObject.activeSelf && info_time.ContainsKey(int.Parse(term_time.gameObject.name)))
            {
                trem_timeImage.SetActive(true);
                term_time.text = /*"距离研究完成还需" + "\n" +*/ Globle.formatTime(info_time[int.Parse(term_time.gameObject.name)]);
                if (info_time[int.Parse(term_time.gameObject.name)] > 10)
                {
                    costgoldbtn_free.SetActive(false);
                    costgoldbtn_nofree.SetActive(true);
                    int num = info_time[int.Parse(term_time.gameObject.name)] / 600;
                    if (num < 10)
                        num = 10;
                    costgold_num.text = num.ToString();
                }
                else if (info_time[int.Parse(term_time.gameObject.name)] <= 10)
                {
                    costgoldbtn_free.SetActive(true);
                    costgoldbtn_nofree.SetActive(false);
                }

            }

        }
        //刷新信息界面
        bool islv = true;
        bool ispre_lv = true;
        bool isotherRuneAll_lv = true;
        void RefreshInfos(int id)
        {
            nowid = id;
            Dictionary<int, runeData> runeinfos = Skill_a3Model.getInstance().runedic;
            runelv.text = "Lv:" + runeinfos[id].now_lv.ToString();
            runename.text = Skill_a3Model.getInstance().runedic[id].name;
            runedes.text = Skill_a3Model.getInstance().runedic[id].desc;
            SXML xml = XMLMgr.instance.GetSXML("rune.rune", "id==" + id);
            SXML xmls = xml.GetNode("level", "lv==" + (runeinfos[id].now_lv + 1).ToString());
            if (xml.GetNode("level", "lv==" + (runeinfos[id].now_lv + 1).ToString()) != null)
            {
                term.text = "";
                List<SXML> xmlss = xmls.GetNodeList("require");
                foreach (SXML x in xmlss)
                {
                    if (x.getInt("role_zhuan") != -1)
                    {
                        if (PlayerModel.getInstance().up_lvl > x.getInt("role_zhuan"))
                        {
                            term.text += "<color=#00FF00>" + x.getInt("role_zhuan") + ContMgr.getCont("zhuan") + x.getInt("role_level") + ContMgr.getCont("ji") + "</color>" + "\n";
                            islv = true;
                        }
                        else if (PlayerModel.getInstance().up_lvl == x.getInt("role_zhuan"))
                        {
                            if (PlayerModel.getInstance().lvl >= x.getInt("role_level"))
                            {
                                islv = true;
                                term.text += "<color=#00FF00>" + x.getInt("role_zhuan") + ContMgr.getCont("zhuan") + x.getInt("role_level") + ContMgr.getCont("ji") + "</color>" + "\n";
                            }
                            else
                            {
                                islv = false;
                                term.text += "<color=#ff0000>" + x.getInt("role_zhuan") + ContMgr.getCont("zhuan") + x.getInt("role_level") + ContMgr.getCont("ji") + "</color>" + "\n";
                            }
                        }
                        else
                        {
                            islv = false;
                            term.text += "<color=#ff0000>" + x.getInt("role_zhuan") + ContMgr.getCont("zhuan") + x.getInt("role_level") + ContMgr.getCont("ji") + "</color>" + "\n";
                        }
                    }
                    if (x.getInt("pre_run_id") != -1)
                    {
                        if (runeinfos[x.getInt("pre_run_id")].now_lv < x.getInt("pre_run_level"))
                        {
                            ispre_lv = false;
                            term.text += "<color=#ff0000>" + runeinfos[x.getInt("pre_run_id")].name + "LV:" + x.getInt("pre_run_level") + "</color>" + "\n";
                        }
                        else
                        {
                            term.text += "<color=#00FF00>" + runeinfos[x.getInt("pre_run_id")].name + "LV:" + x.getInt("pre_run_level") + "</color>" + "\n";
                            ispre_lv = true;
                        }
                    }
                    else
                    {
                        ispre_lv = true;
                    }
                    int alllv = 0;
                    if (x.getInt("other_rune_t_level") != -1)
                    {
                        foreach (int i in runeinfos.Keys)
                        {
                            if (runeinfos[i].type == x.getInt("type"))
                                alllv += runeinfos[i].now_lv;
                        }
                        if (alllv >= x.getInt("other_rune_t_level"))
                        {
                            isotherRuneAll_lv = true;
                            if (x.getInt("type") == 1)
                            {
                                term.text += "<color=#00FF00>" + ContMgr.getCont("skill_a3_txt1") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                            else if (x.getInt("type") == 2)
                            {
                                term.text += "<color=#00FF00>" + ContMgr.getCont("skill_a3_txt2") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                            else if (x.getInt("type") == 3)
                            {
                                term.text += "<color=#00FF00>" + ContMgr.getCont("skill_a3_txt3") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                        }
                        else
                        {
                            isotherRuneAll_lv = false;
                            if (x.getInt("type") == 1)
                            {
                                term.text += "<color=#ff0000>" + ContMgr.getCont("skill_a3_txt1") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                            else if (x.getInt("type") == 2)
                            {
                                term.text += "<color=#ff0000>" + ContMgr.getCont("skill_a3_txt2") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                            else if (x.getInt("type") == 3)
                            {
                                term.text += "<color=#ff0000>" + ContMgr.getCont("skill_a3_txt3") + x.getInt("other_rune_t_level") + "</color>" + "\n";
                            }
                        }
                    }
                    else
                    {
                        isotherRuneAll_lv = true;
                    }


                }
            }
            else
            {
                runelv.text = "Lv:" + runeinfos[id].now_lv.ToString();
                term.text = ContMgr.getCont("skill_a3_maxlv");
            }
            if (runeinfos[id].time == -1 || runeinfos[id].time == 0)
            {
                term_time.gameObject.SetActive(false);
                trem_timeImage.SetActive(false);
                //term.gameObject.SetActive(true);
                studybtn.SetActive(true);
                costgoldbtn.SetActive(false);
            }
            else
            {
                print("现在的时间是：" + runeinfos[id].time);
                showTime(runeinfos[id].time, id);
            }
        }
        void showTime(int time, int id)
        {
            term_time.gameObject.SetActive(true);
            trem_timeImage.SetActive(true);
            term_time.gameObject.name = id.ToString();
            //term.gameObject.SetActive(false);
            studybtn.SetActive(false);
            costgoldbtn.SetActive(true);
        }
        //加速按钮
        void onAddspeedClick(GameObject go)
        {
            //print("倒计时间是：" + refresh_time);
            if (costgoldbtn_free.activeSelf == false)
            {
                if (PlayerModel.getInstance().gold < int.Parse(costgold_num.text))
                    flytxt.instance.fly(ContMgr.getCont("skill_a3_enoughbs"), 1);
                else
                    Skill_a3Proxy.getInstance().sendProxys(3, nowid, false);
            }
            else if (costgoldbtn_free.activeSelf)
            {

                Skill_a3Proxy.getInstance().sendProxys(3, nowid, true);
            }
        }
        //研究符文按钮
        void onStudyRuneClick(GameObject go)
        {


            if (ispre_lv == false)
                flytxt.instance.fly(ContMgr.getCont("skill_a3_txt4"), 1);
            else
            {
                if (isotherRuneAll_lv == false)
                    flytxt.instance.fly(ContMgr.getCont("skill_a3_txt5"), 1);
                else
                {
                    if (islv == false)
                        flytxt.instance.fly(ContMgr.getCont("skill_a3_txt6"), 1);
                }
            }
            if (islv && isotherRuneAll_lv && ispre_lv)
            {
                Skill_a3Proxy.getInstance().sendProxys(2, nowid);
            }
        }
        //
        void showthisrune(int id)
        {
            foreach (int i in runeobj.Keys)
            {
                if (i == id)
                    runeobj[i].transform.FindChild("this").gameObject.SetActive(true);
                else
                    runeobj[i].transform.FindChild("this").gameObject.SetActive(false);
            }
        }



        #endregion
    }

}