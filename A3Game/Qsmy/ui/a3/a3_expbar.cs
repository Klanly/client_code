using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
using DG.Tweening;
using Lui;
namespace MuGame
{
    class a3_expbar : FloatUi
    {

        public GameObject BossRankingBtn;

        static public a3_expbar instance;
        GameObject against;
        Animator ani;
        Vector3 topPos;
        Vector3 lastPos;
        float lastPosY;
        float lastPosX;
        float itemChatMsgWidth;
        RectTransform itemChatMsgPrefab;
        Transform _chatContent;
        Vector3 _initPos;
        Transform lightTipTran;
        Vector3 lightTipPos1, lightTipPos2;
        float _initPosX;
        float _initPosY;
        Text txtConfig;
        //Text expt;
        //string strPk = ContMgr.getCont("a3_expbar_att");
        Text txtPKInfo;
        bool btnDwonClicked = true;
        Dictionary<string, GameObject> lightip = new Dictionary<string, GameObject>();
        //离线经验临时按钮 
        public BaseButton btnAutoOffLineExp;
        BaseButton btnWashredname;
        BaseButton btnNewFirendTips;//新好友申请提醒
        BaseButton btnTeamTips;//新队伍邀请
        Text txtNewFirendTips;//新好友数目显示
        BaseButton btn_1;
        BaseButton btn_out;

        GameObject iconHit;
        public static int feedTime;
        public Transform tfMHFloatBorder;
        bool isChatHiden { get; set; } = false;
        public Font ft;//显示数字的字体
        public override void init()
        {
            instance = this;
            ft = getComponentByPath<Text>("operator/btn_1_2/count/count").font;
            against = getGameObjectByPath("Against");
            BaseButton close_against = new BaseButton(transform.FindChild("Against/close"));
            close_against.onClick = onCloseAgainst;
            txtPKInfo = transform.FindChild("Against/Text").GetComponent<Text>();
            lightTipTran = getTransformByPath("operator/LightTips");
            lightTipPos1 = lightTipTran.GetComponent<RectTransform>().anchoredPosition;
            lightTipPos2 = getTransformByPath("operator/LightTipsPart2").GetComponent<RectTransform>().anchoredPosition;
            ani = transform.GetComponent<Animator>();
            alain();
            BaseButton btn_0 = new BaseButton(transform.FindChild("operator/btn_0"));
            btn_0.onClick = onBtn;

            btn_1 = new BaseButton(transform.FindChild("operator/btn_1"));
            btn_1.onClick = onBtn;

            BaseButton bnt_1_2 = new BaseButton(transform.FindChild("operator/btn_1_2"));
            bnt_1_2.onClick = onBtn;

            BaseButton btn_2 = new BaseButton(transform.FindChild("operator/btn_2"));
            btn_2.onClick = onBtn;
            btn_2.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_46")); };

            BaseButton btn_3 = new BaseButton(transform.FindChild("operator/btn_3"));
            btn_3.onClick = onBtn;

            BaseButton btn_up = new BaseButton(transform.FindChild("operator/btn_up"));
            btn_up.onClick = onBtn;
            btn_up.onClick += (GameObject go) => { isChatHiden = true; A3_BeStronger.Instance.Show = false; };

            BaseButton btn_down = new BaseButton(transform.FindChild("operator/btn_down"));
            btn_down.onClick = onBtn;
            btn_down.onClick += (GameObject go) => { isChatHiden = false; };

            BaseButton btn_4 = new BaseButton(transform.FindChild("operator/btn_4"));
            btn_4.onClick = onBtn;

            BaseButton btn_5 = new BaseButton(transform.FindChild("operator/btn_5"));
            btn_5.onClick = onBtn;
            btn_5.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_18")); };

            BaseButton btn_6 = new BaseButton(transform.FindChild("operator/btn_6"));
            btn_6.onClick = onBtn;
            btn_6.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_3")); };

            BaseButton btn_7 = new BaseButton(transform.FindChild("operator/btn_7"));
            btn_7.onClick = onBtn;
            btn_7.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_4")); };

            BaseButton btn_8 = new BaseButton(transform.FindChild("operator/btn_8"));
            btn_8.onClick = onBtn;
            btn_8.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_5")); };

            BaseButton btn_9 = new BaseButton(transform.FindChild("operator/btn_9"));
            btn_9.onClick = onBtn;
            btn_9.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_6")); };

            BaseButton btn_10 = new BaseButton(transform.FindChild("operator/btn_10"));
            btn_10.onClick = onBtn;
            btn_10.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_7")); };

            BaseButton btn_11 = new BaseButton(transform.FindChild("operator/btn_11"));
            btn_11.onClick = onBtn;

            BaseButton btn_12 = new BaseButton(transform.FindChild("operator/btn_12"));
            btn_12.onClick = onBtn;

            BaseButton btn_13 = new BaseButton(transform.FindChild("operator/btn_13"));
            btn_13.onClick = onBtn;
            btn_13.onClickFalse = noPet;

            BaseButton btn_14 = new BaseButton(transform.FindChild("operator/btn_14"));
            btn_14.onClick = onBtn;
            btn_14.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_9")); };

            BaseButton btn_16 = new BaseButton(transform.FindChild("operator/btn16"));
            btn_16.onClick = onBtn;

            BaseButton btn_17 = new BaseButton(transform.FindChild("operator/btn_17"));
            btn_17.onClick = onBtn;
            btn_17.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_42")); };

            BaseButton btn_18 = new BaseButton(getTransformByPath("operator/btn_18"));
            btn_18.onClick = onBtn;

            BaseButton btn_19 = new BaseButton(getTransformByPath("operator/btn_19"));
            btn_19.onClick = onBtn;
            btn_19.onClickFalse = (GameObject go) => { flytxt.instance.fly(ContMgr.getCont("func_limit_45")); };

            BaseButton mail = new BaseButton(transform.FindChild("mail"));
            mail.onClick = onBtn;

            BaseButton bossranking = new BaseButton(transform.FindChild("bossranking"));
            bossranking.onClick = onBtn;
            BossRankingBtn = getGameObjectByPath("bossranking");

            iconHit = this.transform.FindChild("operator/btn_9/IconHint").gameObject;

            btn_out = new BaseButton(transform.FindChild("btn_out"));
            btn_out.onClick = onBtnOut;
            btn_out.gameObject.SetActive(false);
            //魔物猎人
            tfMHFloatBorder = transform.FindChild("mh_tips/a3_mhfloatBorder");
            //离线经验
            btnAutoOffLineExp = new BaseButton(transform.FindChild("operator/LightTips/btnAuto_off_line_exp"));
            btnAutoOffLineExp.onClick = OnOfflineExp;

            if (welfareProxy.getInstance()._isShowEveryDataLogin)
                getTransformByPath("operator/LightTips/everyDayLogin").gameObject.SetActive(true);

            new BaseButton(getTransformByPath("operator/LightTips/everyDayLogin")).onClick = (GameObject go) =>
             {
                 //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EVERYDAYLOGIN);
                 welfareProxy.getInstance()._isShowEveryDataLogin = false;
                 welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.selfWelfareInfo);
             };

            btnWashredname = new BaseButton(transform.FindChild("operator/LightTips/wash_redname"));
            btnWashredname.onClick = onWashredname;

            btnNewFirendTips = new BaseButton(transform.FindChild("operator/LightTips/btnNewFirendTips"));
            btnNewFirendTips.gameObject.SetActive(false);
            btnNewFirendTips.onClick = onBtnNewFirendTipsClick;

            txtNewFirendTips = btnNewFirendTips.transform.FindChild("Text").GetComponent<Text>();

            btnTeamTips = new BaseButton(transform.FindChild("operator/LightTips/btnTeamTips"));
            //btnTeamTips.onClick = onBtnTeamTipsClick;
            btnTeamTips.gameObject.SetActive(false);

            topPos = transform.FindChild("operator/chat/top").transform.position;
            BaseButton btnChat = new BaseButton(transform.FindChild("operator/chat/btnChat"));
            btnChat.onClick = onBtnChatClick;
            lastPos = transform.FindChild("operator/chat/bottom").transform.position;
            lastPosY = lastPos.y;
            lastPosX = lastPos.x;
            _chatContent = transform.FindChild("operator/chat/mask/scroll/content");
            _initPos = transform.FindChild("operator/chat/initPos").transform.position;
            _initPosX = _initPos.x;
            _initPosY = _initPos.y;
            itemChatMsgPrefab = transform.FindChild("operator/chat/templet/itemChatMsg").GetComponent<RectTransform>();
            txtConfig = transform.FindChild("operator/chat/templet/txtConfig").GetComponent<Text>();
            RectTransform chatRectTransform = transform.FindChild("operator/chat").GetComponent<RectTransform>();
            itemChatMsgWidth = chatRectTransform.sizeDelta.x - btn_0.transform.GetComponent<RectTransform>().sizeDelta.x - 10.0f;
            //expt = transform.FindChild("exp_bar/Text").GetComponent<Text>();

            //EventTriggerListener.Get(transform.FindChild("exp_bar").gameObject).onDown = (GameObject g) =>
            //{
            //    expt.enabled = true;
            //};
            //EventTriggerListener.Get(transform.FindChild("exp_bar").gameObject).onUp = (GameObject g) =>
            //{
            //    expt.enabled = false;
            //};

            //PlayerInfoProxy.getInstance().addEventListener(PlayerInfoProxy.EVENT_ON_EXP_CHANGE, refreshExp);
            //PlayerInfoProxy.getInstance().addEventListener(PlayerInfoProxy.EVENT_ON_LV_CHANGE, refreshExp);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_NEW_MAIL, OnNewMail);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_PET, OpenPet);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_HAVE_PET, closePet);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, feedOpenPet);
            //refreshExp(null);
            // InterfaceMgr.getInstance().open(InterfaceMgr.A3_CHATROOM);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_NEWGET, (GameEvent e) =>
            {
                Variant da = e.data;
                bool isnew = da["new"];
                var tar = transform.FindChild("operator/btn_6/notice");
                if (isnew) tar.gameObject.SetActive(true);
                else tar.gameObject.SetActive(false);
            });
            A3_AuctionProxy.getInstance().SendMyRackMsg();
            FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TEAMAPPLYPANEL);
            CheckLock();
            CheckLightTip();
            bag_Count();
            //if (feedTime == 0 && PlayerModel.getInstance().havePet)
            //    transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(true);
            if (!PlayerModel.getInstance().havePet)
                transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(false);
            else
            {
                if(feedTime == 0)
                {
                    // transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(true);
                }
                else
                    transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(false);
            }
            if (a3_active_mwlr_kill.initLoginData != null && a3_active_mwlr_kill.initLoginData.Count > 0)
            {
                a3_active_mwlr_kill.Instance.ReloadData(a3_active_mwlr_kill.initLoginData);
                a3_active_mwlr_kill.initLoginData = null;
            }
            else
            {
                a3_active_mwlr_kill.Instance.Bar_Mon?.gameObject.SetActive(false);
                a3_active_mwlr_kill.Instance.Clear();
            }
            #region 魔物猎人寻路(已弃用)
            //new BaseButton(transform.FindChild("mh_tips/a3_mhfloatBorder")).onClick = (GameObject go) =>
            //{
            //    var v_mapInfo = A3_ActiveModel.getInstance().mwlr_map_info;
            //    var v_mapId = A3_ActiveModel.getInstance().mwlr_map_id;
            //    for (int i = 0; i < v_mapInfo.Count; i++)
            //        if (!v_mapInfo[i]["kill"]._bool)
            //        {
            //            if (SelfRole.fsm.Autofighting)
            //                SelfRole.fsm.Stop();
            //            if (SelfRole.UnderTaskAutoMove)
            //                SelfRole.fsm.ClearAutoConfig();

            //            A3_ActiveModel.getInstance().mwlr_target_monId = v_mapInfo[0]["target_mid"];
            //            A3_ActiveModel.getInstance().mwlr_on = true;
            //            SelfRole.WalkToMap(
            //                id: v_mapInfo[i]["map_id"]._int,
            //                vec: new Vector3(
            //                    x: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[i]].x,
            //                    y: SelfRole._inst.m_curModel.position.y,
            //                    z: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[i]].z
            //                ),
            //                handle: () =>
            //                {
            //                    if (A3_ActiveModel.getInstance().mwlr_on)
            //                        if (SelfRole._inst.m_moveAgent.remainingDistance < 1f)
            //                            if (!SelfRole.fsm.Autofighting)
            //                            {
            //                                SelfRole.fsm.StartAutofight();
            //                                SelfRole.fsm.ChangeState(StateAttack.Instance);
            //                            }
            //                }
            //            );
            //            break;
            //        }
            //};
            #endregion
            int siblingOffset = tfMHFloatBorder.FindChild("hunt_icon_0").GetSiblingIndex();
            for (int i = 0; i < 5; i++)
            {
                new BaseButton(tfMHFloatBorder.GetChild(siblingOffset + i)).onClick = (GameObject go) =>
                {
                    Variant v_mapInfo = A3_ActiveModel.getInstance().mwlr_map_info;
                    List<int> v_mapId = A3_ActiveModel.getInstance().mwlr_map_id;
                    int index = go.transform.GetSiblingIndex() - siblingOffset;
                    if (SelfRole.fsm.Autofighting)
                        SelfRole.fsm.Stop();
                    if (!v_mapInfo[index]["kill"]._bool)
                    {
                        A3_ActiveModel.getInstance().mwlr_target_monId = v_mapInfo[index]["target_mid"];
                        A3_ActiveModel.getInstance().mwlr_on = true;
                        //SelfRole.WalkToMap(
                        //    id: v_mapInfo[index]["map_id"]._int,
                        //    vec: new Vector3(
                        //        x: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[index]].x,
                        //        y: SelfRole._inst.m_curModel.position.y,
                        //        z: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[index]].z),
                        //    handle: () =>
                        //    {
                        //        if (A3_ActiveModel.getInstance().mwlr_on)
                        //            if (SelfRole._inst.m_moveAgent.remainingDistance < 1f)
                        //                if (!SelfRole.fsm.Autofighting)
                        //                {
                        //                    SelfRole.fsm.StartAutofight();
                        //                    SelfRole.fsm.ChangeState(StateAttack.Instance);
                        //                }
                        //    });
                        int mapId = v_mapInfo[index]["map_id"]._int;
                        Vector3 vec = new Vector3(
                                x: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[index]].x,
                                y: SelfRole._inst.m_curModel.position.y,
                                z: A3_ActiveModel.getInstance().mwlr_mons_pos[v_mapId[index]].z);
                        Action handle = () =>
                        {
                            if (A3_ActiveModel.getInstance().mwlr_on)
                                if (SelfRole._inst.m_moveAgent.isOnNavMesh && SelfRole._inst.m_moveAgent.remainingDistance < 1f)
                                    if (!SelfRole.fsm.Autofighting)
                                    {
                                        SelfRole.fsm.StartAutofight();
                                        SelfRole.fsm.ChangeState(StateAttack.Instance);
                                    }
                        };
                        if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                            SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.WalkToMap(mapId, vec, handle));
                        else
                            SelfRole.WalkToMap(mapId, vec, handle);
                    }
                };
            }
            addIconHintImage();
            showiconHit();
        }

        public void HoldTip()
        {
            lightTipTran.GetComponent<RectTransform>().anchoredPosition = lightTipPos2;
        }

        public void DownTip()
        {
            lightTipTran.GetComponent<RectTransform>().anchoredPosition = lightTipPos1;
        }
        public void refreshExp(GameEvent e)
        {

            //SXML s_xml = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + PlayerModel.getInstance().profession);
            //SXML s_exp = s_xml.GetNode("zhuanshen", "zhuan==" + PlayerModel.getInstance().up_lvl);
            //s_exp = s_exp.GetNode("carr", "lvl==" + PlayerModel.getInstance().lvl);

            //int cost_exp = s_exp.getInt("exp");
            //transform.FindChild("exp_bar").gameObject.GetComponent<Image>().fillAmount = (float)PlayerModel.getInstance().exp / (float)cost_exp;
            //expt.text = PlayerModel.getInstance().exp + "/" + cost_exp;
        }

        public void OnNewMail(GameEvent e)
        {
            //transform.FindChild("mail").gameObject.SetActive(true);
            ShowLightTip("mail", (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add(0);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MAIL, arr);
                RemoveLightTip("mail");
            });
        }

        public void HideMailHint()
        {
            transform.FindChild("mail").gameObject.SetActive(false);
        }
        void onBtnChatClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHATROOM);//CHATROOM
        }
        //bool btnDwonClicked = true;
        void onBtn(GameObject go)
        {
            switch (go.name)
            {
                //聊天
                case "btn_0":

                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHATROOM);//CHATROOM
                    break;
                //背包
                case "btn_1":
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_FINDBESTO);
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_WANTLVUP);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
                  
                    //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEWACTIVE);
                    //InterfaceMgr.openByLua("a3_bag");                   
                    break;
                case "btn_1_2":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
                    //InterfaceMgr.openByLua("a3_bag");
                    break;
                //任务
                case "btn_2":
                //暂用
                //InterfaceMgr.getInstance().open(InterfaceMgr.A3_RANK);
                //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK);
                InterfaceMgr.getInstance().ui_async_open( InterfaceMgr.RIDE_A3 );
                break;
                //人物属性
                case "btn_3":
                    //InterfaceMgr.openByLua("a3_task");
                    a3_role.ForceIndex = 0;
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ROLE);
                    //InterfaceMgr.openByLua("a3_role");
                    break;
                //社交
                case "btn_4":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO);
                    break;
                //工坊
                case "btn_5":
                    if (can_Btn5())
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
                    }
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_expbar_needone"));
                    }
                    break;
                //拍卖
                case "btn_6":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION);
                    break;
                //成就
                case "btn_7":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HONOR );
                    //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RANK );
                    break;
                //召唤兽
                case "btn_8":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW);
                    break;
                //技能
                case "btn_9":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SKILL_A3);
                    break;
                //飞翼
                case "btn_10":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WIBG_SKIN);

                    break;
                //邮件
                case "btn_11":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MAIL);
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_YGYIWU);
                    break;
                //设置
                case "btn_12":
                   // InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVEONLINE);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SYSTEM_SETTING);
                    //if (debug.instance != null)
                    //    debug.instance.changetxt();
                    break;
                //宠物
                case "btn_13":
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_PET_SKIN);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEW_PET);
                    //PetFlyText();
                    break;
                case "btn_14":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HUDUN);
                    //InterfaceMgr.openByLua("a3_task");
                    break;
                case "btn16":
                    SelfRole.fsm.ChangeState(StateIdle.Instance);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION);
                    break;
                case "btn_17":
                    InterfaceMgr.openByLua("a3_star_pic");
                    break;
                case "btn_18":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RUNESTONE);
                    break;
                case "btn_up":
                    On_Btn_Up();
                    break;
                case "btn_down":
                    On_Btn_Down();
                    break;
                case "mail":
                    ArrayList arr = new ArrayList();
                    arr.Add(0);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MAIL, arr);
                    transform.FindChild("mail").gameObject.SetActive(false);
                    break;
                case "btn_19":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HALLOWS);
                    break;
                case "bossranking":
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BOSSRANKING);
                    break;
            }
        }
        public void onBtnOut(GameObject go)
        {
            btn_out.gameObject.SetActive(false);
            On_Btn_Down();
        }

        public void showiconHit()
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
        public void On_Btn_Down()
        {
            if (btnDwonClicked) return;
            btnDwonClicked = true;
            ani.SetBool("onoff", false);
            getComponentByPath<Transform>("operator/btn_down").gameObject.SetActive(false);
            getComponentByPath<Transform>("operator/btn_up").gameObject.SetActive(true);
            //getComponentByPath<Transform>( "IsDown" ).gameObject.SetActive( true );
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.onoffAni(false);
            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.onoff_skillbarAni(false);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.onTogglePlusClick(false);
            btn_out.gameObject.SetActive(false);
        }
        private void OnOfflineExp(GameObject go)
        {
            //getGameObjectByPath("operator/LightTips/btnAuto_off_line_exp").SetActive(false);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.OFFLINEEXP);
        }
        public void On_Btn_Up()
        {
            if (!btnDwonClicked) return;
            btnDwonClicked = false;
            ani.SetBool("onoff", true);
            getComponentByPath<Transform>("operator/btn_down").gameObject.SetActive(true);
            getComponentByPath<Transform>("operator/btn_up").gameObject.SetActive(false);
            //getComponentByPath<Transform>( "IsDown" ).gameObject.SetActive( false );
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.onoffAni(true);
            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.onoff_skillbarAni(true);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            //if (a3_liteMinimap.instance != null)
            //    a3_liteMinimap.instance.onTogglePlusClick(true);

            btn_out.gameObject.SetActive(true);
        }
        public void ShowAgainst(BaseRole m_CastRole)
        {
            if (against.activeSelf == false)
            {
                against.SetActive(true);
                //txtPKInfo.text = string.Format(strPk, m_CastRole.roleName);
                txtPKInfo.text = ContMgr.getCont("a3_expbar_att",new List<string>() { m_CastRole.roleName.ToString() });
                BaseButton againstbtn = new BaseButton(transform.FindChild("Against/Button"));
                againstbtn.onClick = delegate
                {
                    onAgainst(m_CastRole);
                };
                Invoke("set_FalsePanel", 30);
            }

        }

        public void showBtnIcon( bool show)
        {
            if (show) {
                transform.FindChild("operator/LightTips").localScale = Vector3.one;
                transform.FindChild("operator/btn_up").localScale = Vector3.one;
                transform.FindChild("operator/btn_1").localScale = Vector3.one;
            } else {

                transform.FindChild("operator/LightTips").localScale = Vector3.zero ;
                transform.FindChild("operator/btn_up").localScale = Vector3.zero;
                transform.FindChild("operator/btn_1").localScale = Vector3.zero;
            }

        }


        void set_FalsePanel()
        {
            if (against.activeSelf)
                against.gameObject.SetActive(false);

        }

        public void CloseAgainst()
        {
            if (against.activeSelf == true)
            {
                against.SetActive(false);
            }
        }
        void onCloseAgainst(GameObject go)
        {
            against.SetActive(false);
        }
        public void Update()
        {
            a3_active_mwlr_kill.Instance.Update();
        }
        void PetFlyText()
        {
            if (A3_PetModel.getInstance().hasPet())
            {
                uint feedid = A3_PetModel.getInstance().GetFeedItemTpid();
                int num = a3_BagModel.getInstance().getItemNumByTpid(feedid);
                bool Auto_buy = A3_PetModel.getInstance().Auto_buy;
                if (num <= 0 && Auto_buy == false)
                {
                    flytxt.instance.fly(ContMgr.getCont("pet_no_feed"));
                }
            }
        }
        void onAgainst(BaseRole m_CastRole)
        {
            //吧目标转为打我的人，如果他死了/退出/传送，飘字提示
            if (m_CastRole.isDead || m_CastRole == null || !OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(m_CastRole.m_unIID))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_expbar_nopeople"), 1);
                against.SetActive(false);
            }
            else
            {
                //a3_PkmodelProxy.getInstance().sendProxy(1);

                // print("打我的人的坐标：" + m_CastRole.m_curPhy.position);
                SelfRole._inst.m_LockRole = m_CastRole;
                against.SetActive(false);

            }
        }
        Queue<GameObject> itemChatMsgQuene = new Queue<GameObject>();
        public void setRollWord(List<msg4roll> msgrollList)
        {
            if (itemChatMsgQuene.Count >= 3)
            {
                GameObject go = itemChatMsgQuene.Dequeue();
                Destroy(go);
            }
            //create itemChatMsg
            GameObject itemChatMsg = GameObject.Instantiate(itemChatMsgPrefab.gameObject) as GameObject;
            LRichText lRichText = itemChatMsg.GetComponent<LRichText>();
            int fontSize = txtConfig.fontSize;
            lRichText.font = txtConfig.font;
            foreach (var msgRoll in msgrollList)
                lRichText.insertElement(msgRoll.msgStr, msgRoll.color, fontSize, false, false, msgRoll.color, msgRoll.data);
            a3_chatroom._instance.setLRichTextAction(lRichText, true);
            lRichText.reloadData(-0.5f);
            itemChatMsg.transform.SetParent(_chatContent, false);
            itemChatMsg.transform.localScale = Vector3.one;
            itemChatMsg.SetActive(true);
            RectTransform[] rects = itemChatMsg.GetComponentsInChildren<RectTransform>(false);
            float min = 0, max = 0;
            if (rects.Length > 0)
                min = max = rects[0].anchoredPosition.y;
            for (int i = 0; i < rects.Length; i++)
            {
                if (rects[i].anchoredPosition.y > max)
                    max = rects[i].anchoredPosition.y;
                if (rects[i].anchoredPosition.y < min)
                    min = rects[i].anchoredPosition.y;
            }
            itemChatMsg.GetComponent<LayoutElement>().minHeight = max - min;
            itemChatMsgQuene.Enqueue(itemChatMsg);
            //hasNewMsg = false; //moveMsgRollWord();
        }

        bool can_Btn5()
        {
            Dictionary<uint, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquips();
            Dictionary<uint, a3_BagItemData> unequips = a3_BagModel.getInstance().getUnEquips();
            if (equips.Count <= 0 && unequips.Count <= 0) { return false; }
            else { return true; }
        }
        //void moveMsgRollWord()
        //{
        //    Vector3 moveV3 = new Vector3(_chatContent.position.x, _chatContent.position.y + 100, _chatContent.position.z);
        //    _chatContent.DOMove(moveV3, 2.0f).OnUpdate(() =>
        //    {
        //        if (itemChatMsgEndPostion.position.y >= lastPos.y + 10)
        //        {
        //            _chatContent.DOKill();
        //            lastItemChatMsgPostion = itemChatMsgEndPostion;
        //        }
        //    });
        //}

        void ClickFalse(GameObject go)
        {
            flytxt.instance.fly(ContMgr.getCont("a3_expbar_nogn"));
        }
        void noPet(GameObject go)
        {
            flytxt.instance.fly(ContMgr.getCont("func_limit_8"));
        }
        //public void CheckNewSkill()
        //{
        //    bool newskill = false;
        //    foreach (var v in Skill_a3Model.getInstance().skilldic.Values)
        //    {
        //        if (v.carr != PlayerModel.getInstance().profession) continue;
        //        uint a = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl;
        //        var gv = v.xml.GetNode("skill_att", "skill_lv==" + (v.now_lv + 1));
        //        if (gv != null)
        //        {
        //            int b = gv.getInt("open_zhuan") * 100 + gv.getInt("open_lvl");
        //            if (gv != null && a >= b && v.skill_id % 1000 != 1 && v.now_lv == 0 && v.skill_id != 1 && a3_BagModel.getInstance().getItemNumByTpid(1540) >= gv.getInt("item_num"))
        //            {
        //                newskill = true;
        //            }
        //        }
        //    }
        //    if (newskill)
        //    {
        //        ShowLightTip("newskill", (GameObject g) =>
        //        {
        //            InterfaceMgr.getInstance().open(InterfaceMgr.SKILL_A3);
        //            RemoveLightTip("newskill");
        //        });
        //    }
        //    else RemoveLightTip("newskill");
        //}

        //public void CheckNewAtt()
        //{
        //    SXML s_xml = XMLMgr.instance.GetSXML("carrlvl.points_hint");
        //    int mmint = s_xml.getInt("points_num");
        //    if (PlayerModel.getInstance().pt_att >= mmint)
        //    {
        //        ShowLightTip("newatt", (GameObject g) =>
        //        {
        //            ArrayList arr = new ArrayList();
        //            arr.Add(1);
        //            InterfaceMgr.getInstance().open(InterfaceMgr.A3_ROLE, arr);
        //            RemoveLightTip("newatt");
        //        });
        //    }
        //    else RemoveLightTip("newatt");
        //}

        void CheckLightTip()
        {
            //CheckNewAtt();
            //CheckNewSkill();
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_RECEIVEAPPLYFRIEND, onReceiveApplyFriend);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEINVITE, onNoticeInvite);
        }

        public void bag_Count()
        {
            int Count = a3_BagModel.getInstance().curi - a3_BagModel.getInstance().getItems().Count;
            switch (Count)
            {
                case 4:
                case 3:
                case 2:
                case 1:
                case 5:
                    btn_1.gameObject.transform.FindChild("count").gameObject.SetActive(true);
                    btn_1.gameObject.transform.FindChild("man").gameObject.SetActive(false);
                    btn_1.gameObject.transform.FindChild("count/count").GetComponent<Text>().text = Count.ToString();
                    IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_BAG, -1, Count);
                    break;

                case 0:
                    btn_1.gameObject.transform.FindChild("count").gameObject.SetActive(false);
                    btn_1.gameObject.transform.FindChild("man").gameObject.SetActive(true);
                    IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_BAG, -1, Count, true);
                    break;
                default:
                    btn_1.gameObject.transform.FindChild("count").gameObject.SetActive(false);
                    btn_1.gameObject.transform.FindChild("man").gameObject.SetActive(false);
                    IconHintMgr.getInsatnce().closeHint(IconHintMgr.TYPE_BAG, true, true);
                    break;
            }
        }


        public void ShowLightTip(string iconname, Action<GameObject> OnClicked, int num = -1, bool force = false)
        {
            GameObject g = null;
            if (!lightip.ContainsKey(iconname) && !force)
            {
                Transform content = getTransformByPath("operator/LightTips");
                GameObject temp = getGameObjectByPath("lt_temp/" + iconname);
                if (temp == null) return;
                g = GameObject.Instantiate(temp) as GameObject;
                g.transform.SetParent(content);
                g.transform.localScale = Vector3.one;
                g.SetActive(true);
                lightip[iconname] = g;
            }
            else
            {
                g = lightip[iconname];
            }
            if (g != null)
            {
                BaseButton mail = new BaseButton(g.transform);
                mail.onClick += OnClicked;
            }
        }

        public void RemoveLightTip(string iconname)
        {
            if (lightip.ContainsKey(iconname))
            {
                GameObject.Destroy(lightip[iconname]);
                lightip.Remove(iconname);
            }
        }

        public void CheckLock()
        {
            //transform.FindChild("operator/btn_13").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_10").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_10/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_5").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_5/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_6").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_6/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_8").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_8/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_7").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_7/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_9").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_9/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_13").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_13/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_14").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_14/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_17").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_17/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_19").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_19/local").gameObject.SetActive(true);
            transform.FindChild("operator/btn_2").gameObject.GetComponent<Button>().interactable = false;
            transform.FindChild("operator/btn_2/local").gameObject.SetActive(true);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING))
            {
                OpenSWING_PET();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP))
            {
                OpenEQP();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.STAR_PIC))
            {
                OpenSTAR_PIC();
            }

            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.HUDUN))
            {
                OpenHUDUN();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.AUCTION_GUILD))
            {
                OpenAuction();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
            {
                OpenSummon();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.ACHIEVEMENT))
            {
                OpenAchievement();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SKILL))
            {
                OpenSkill();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.CHASTEN_JAIL))
           {
                OpenHallow();
            }
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
            //{
            //    OpenPET();
            //}
                if (PlayerModel.getInstance().havePet || FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
            {
                OpenPET();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.MOUNT))
            {
                OpenMOUNT();
            }

        }

        private void OpenSTAR_PIC()
        {
            transform.FindChild("operator/btn_17").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_17/local").gameObject.SetActive(false);
        }

        public void OpenSWING_PET()
        {
            transform.FindChild("operator/btn_10").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_10/local").gameObject.SetActive(false);
        }

        public void OpenHUDUN()
        {
            transform.FindChild("operator/btn_14").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_14/local").gameObject.SetActive(false);
        }
        public void OpenEQP()
        {
            transform.FindChild("operator/btn_5").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_5/local").gameObject.SetActive(false);
        }

        public void OpenAuction()
        {
            transform.FindChild("operator/btn_6").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_6/local").gameObject.SetActive(false);
        }
        public void OpenSummon()
        {
            transform.FindChild("operator/btn_8").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_8/local").gameObject.SetActive(false);
        }
        public void OpenAchievement()
        {
            transform.FindChild("operator/btn_7").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_7/local").gameObject.SetActive(false);
        }
        public void OpenSkill()
        {
            transform.FindChild("operator/btn_9").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_9/local").gameObject.SetActive(false);
        }
        public void OpenHallow()
        {
            transform.FindChild("operator/btn_19").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_19/local").gameObject.SetActive(false);
        }

        public void OpenPET()
        {
            transform.FindChild("operator/btn_13").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_13/local").gameObject.SetActive(false);
        }
        public void OpenMOUNT() {
            transform.FindChild("operator/btn_2").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_2/local").gameObject.SetActive(false);
        }

        public void OpenPet(GameEvent e)
        {
            transform.FindChild("operator/btn_13").gameObject.GetComponent<Button>().interactable = true;
            transform.FindChild("operator/btn_13/local").gameObject.SetActive(false);
        }
        void closePet(GameEvent e)
        {
           // transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(true);
            //if (a3_herohead.instance != null)
            //{
            //    a3_herohead.instance.isclear = true;
            //    a3_herohead.instance.pet_bf = true;
            //        a3_herohead.instance.refresBuff();
            //}

        }
        void feedOpenPet(GameEvent e)
        {
            transform.FindChild("operator/btn_13/nofeed").gameObject.SetActive(false);
            //if (a3_herohead.instance != null)
            //{
            //    a3_herohead.instance.isclear = true;
            //    a3_herohead.instance.pet_bf = true;            
            //        a3_herohead.instance.refresBuff();
            //}

        }
        override public void onShowed()
        {
            SetOffLineExpButton();
            ShowWashRed();
            if (A3_cityOfWarModel.getInstance ().CanInFB()) {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CITYWARTIP );
            }

        }
        //设置离线经验按钮
        private void SetOffLineExpButton()
        {
            OffLineModel lineModel = OffLineModel.getInstance();

            bool b = PlayerModel.getInstance().up_lvl > 0;
            // btnAutoOffLineExp.gameObject.SetActive(b);

            btnAutoOffLineExp.gameObject.SetActive(lineModel.CanGetExp && b);

            Text text = btnAutoOffLineExp.transform.GetChild(0).GetComponent<Text>();

            if (lineModel.OffLineTime == OffLineModel.maxTime)
            {
                btnAutoOffLineExp.transform.FindChild("Image_full").gameObject.SetActive(true);
            }
            else
            {
                btnAutoOffLineExp.transform.FindChild("Image_full").gameObject.SetActive(false);
            }
        }
        public void ShowWashRed()
        {
            if (PlayerModel.getInstance().sinsNub > 15)
                btnWashredname.gameObject.SetActive(true);
            else
                btnWashredname.gameObject.SetActive(false);
        }
        //洗红名
        void onWashredname(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WASHREDNAME);
        }
        void onReceiveApplyFriend(GameEvent e)
        {
            btnNewFirendTips.gameObject.SetActive(true);
            int num = FriendProxy.getInstance().requestFirendList.Count;
            txtNewFirendTips.text = num.ToString();
        }
        //加好友提醒
        void onBtnNewFirendTipsClick(GameObject go)
        {
            btnNewFirendTips.gameObject.SetActive(false);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BEREQUESTFRIEND);
        }
        void onBtnTeamTipsClick(GameObject go)
        {
            //btnTeamTips.gameObject.SetActive(false);
            //ArrayList arr=teamInvitedListData[0];
            //teamInvitedListData.RemoveAt(0);
            //InterfaceMgr.getInstance().open(InterfaceMgr.A3_TEAMINVITEDPANEL, arr);
        }
        List<ArrayList> teamInvitedListData = new List<ArrayList>();
        void onNoticeInvite(GameEvent e)
        {
            Variant data = e.data;
            uint tid = data["tid"];
            uint cid = data["cid"];
            string name = data["name"];
            uint lvl = data["lvl"];
            uint zhuan = data["zhuan"];
            uint carr = data["carr"];
            uint combpt = data["combpt"];
            ItemTeamData itd = new ItemTeamData();
            itd.teamId = tid;
            itd.cid = cid;
            itd.name = name;
            itd.lvl = lvl;
            itd.zhuan = zhuan;
            itd.carr = carr;
            itd.combpt = (int)combpt;
            ArrayList arr1 = new ArrayList();
            arr1.Add(itd);
            teamInvitedListData.Add(arr1);
            ArrayList arr2 = teamInvitedListData[0];
            teamInvitedListData.RemoveAt(0);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TEAMINVITEDPANEL, arr2);
        }
        public void showBtnTeamTips(bool b)
        {
            //btnTeamTips.gameObject.SetActive(b);
        }
        public Dictionary<string, GameObject> hint_obj = new Dictionary<string, GameObject>();
        void addIconHintImage()
        {

            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_3"), IconHintMgr.TYPE_ROLE);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_4"), IconHintMgr.TYPE_SHEJIAO);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_7"), IconHintMgr.TYPE_ACHIEVEMENT);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_9"), IconHintMgr.TYPE_SKILL);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn16"), IconHintMgr.TYPE_YGYUWU);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_5"), IconHintMgr.TYPE_EQUIP);
            IconHintMgr.getInsatnce().addHint(getTransformByPath("operator/btn_10"), IconHintMgr.TYPE_WING_SKIN);
            IconHintMgr.getInsatnce().addHint_havenum(getTransformByPath("operator/btn_1_2"), IconHintMgr.TYPE_BAG);
            IconHintMgr.getInsatnce().addHint_havenum(getTransformByPath("operator/btn_11"), IconHintMgr.TYPE_MAIL);
        }
    }
}
