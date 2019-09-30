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

namespace MuGame
{
    class a3_role : Window
    {
        private GameObject m_SelfObj;//角色的avatar
        private GameObject m_Self_Camera;//拍摄avatar的摄像机
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        public IMesh m_functionbar_scene;
        private ProfessionAvatar m_proAvatar;
        private RawImage roleAvatar;
        private RenderTexture roleTexture;
        GameObject isAdornGo , titleIconGo , equipped, selectBgGo, selectButton;

        private AsyncOperation async = null;

        TabControl tabCtrl;
        private GameObject[] rolePanel;
        ScrollControler scrollControler;
        private Dictionary<int, Text> attr_text = new Dictionary<int, Text>();
        private Dictionary<int, Image> icon_ani = new Dictionary<int, Image>();

        bool isAdd = false;
        bool isReduce = false;
        float addTime = 0.5f;
        float rateTime = 0.0f;
        int addType;
        int left_pt_num = 0;

        private Transform container;

        List<Vector3> listPos = new List<Vector3>();
        public List<Transform> listPage = new List<Transform>();
        private List<GameObject> listIcon = new List<GameObject>();
        bool isCanAdd = false;
        Text isCanAddText;
        private int maxCount;
        private static int forceIndex;
        public static int ForceIndex
        {
            get
            {
                int forceTemp = forceIndex;
                forceIndex = -1;
                return forceTemp;
            }
            set
            {
                forceIndex = value;
                instan?.tabCtrl.setSelectedIndex(forceIndex);
            }
        }
        public static a3_role instan;
        Dictionary<int, Button> btn_pt_add = new Dictionary<int, Button>();
        Dictionary<int, Button> btn_pt_reduce = new Dictionary<int, Button>();
        //1.力量   2.魔力 3.敏捷 4.体力  5.智慧
        Dictionary<int, int> cur_att_pt = new Dictionary<int, int>();
        Dictionary<int, int> true_att_pt = new Dictionary<int, int>();
        Dictionary<int, int> base_att_pt = new Dictionary<int, int>();

        //染色
        BaseButton dye_use;
        //BaseButton dye_backinfo;
        //BaseButton btn_dye;
        Dictionary<uint, GameObject> dyes = new Dictionary<uint, GameObject>();
        SXML dedey = null;
        uint seleltColorId;
        private int index = 0;
        public int Index
        {
            get { return this.index; }
            set
            {
                if (this.index == value)
                    return;

                this.index = value % maxCount;

                SetSibling();
            }
        }
        private void SetSibling()
        {
            listPage[Index].SetAsLastSibling();
        }
        //private BaseButton btnWing;
        public Action OnRunStateChange = null;
        private bool canRun = true;
        public bool CanRun
        {
            get { return canRun; }
            set
            {
                canRun = value;
                if (OnRunStateChange != null)
                    OnRunStateChange();
            }
        }



        public override void init()
        {


            #region 初始化汉字
            getComponentByPath<Text>("ig_bg1/txt1").text = ContMgr.getCont("a3_role_0");
            getComponentByPath<Text>("ig_bg1/txt2").text = ContMgr.getCont("a3_role_1");
            getComponentByPath<Text>("ig_bg1/txt3").text = ContMgr.getCont("a3_role_2");
            getComponentByPath<Text>("ig_bg1/txt4").text = ContMgr.getCont("a3_role_3");
            getComponentByPath<Text>("ig_bg1/txt5").text = ContMgr.getCont("a3_role_4");
            getComponentByPath<Text>("ig_bg1/txt6").text = ContMgr.getCont("a3_role_5");
            getComponentByPath<Text>("ig_bg1/txt7").text = ContMgr.getCont("a3_role_6");
            getComponentByPath<Text>("ig_bg1/txt8").text = ContMgr.getCont("a3_role_7");
            getComponentByPath<Text>("ig_bg1/txt9").text = ContMgr.getCont("a3_role_8");
            getComponentByPath<Text>("ig_bg1/txt10").text = ContMgr.getCont("a3_role_9");
            getComponentByPath<Text>("playerInfo/panelTab/btn_attr/Text").text = ContMgr.getCont("a3_role_10");
            getComponentByPath<Text>("playerInfo/panelTab/btn_add/Text").text = ContMgr.getCont("a3_role_11");
            getComponentByPath<Text>("playerInfo/contents/panel_attr/team").text = ContMgr.getCont("a3_role_12");
            getComponentByPath<Text>("playerInfo/contents/panel_attr/no_team").text = ContMgr.getCont("a3_role_13");
            getComponentByPath<Text>("playerInfo/contents/panel_attr/attr_scroll/scroll/item/name").text = ContMgr.getCont("a3_role_16");
            getComponentByPath<Text>("playerInfo/contents/panel_add/bg/Text").text = ContMgr.getCont("a3_role_14");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_2/magic/Text").text = ContMgr.getCont("a3_role_15");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_3/minjie/Text").text = ContMgr.getCont("a3_role_17");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_1/strength/Text").text = ContMgr.getCont("a3_role_18_" + PlayerModel.getInstance().profession);
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_4/Image/Text").text = ContMgr.getCont("a3_role_19");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_5/Image/Text").text = ContMgr.getCont("a3_role_20");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_clear/Text").text = ContMgr.getCont("a3_role_21");
            getComponentByPath<Text>("playerInfo/contents/panel_add/btn_add_do/Text").text = ContMgr.getCont("a3_role_22");
            getComponentByPath<Text>("playerInfo/contents/panel_add/Text").text = ContMgr.getCont("a3_role_23");
            getComponentByPath<Text>("playerInfo/contents/panel_add/tishi/text").text = ContMgr.getCont("a3_role_24");
            getComponentByPath<Text>("playerInfo/contents/panel_add/tishi/can/yes/Text").text = ContMgr.getCont("a3_role_25");
            getComponentByPath<Text>("playerInfo/contents/panel_add/tishi/can/no/Text").text = ContMgr.getCont("a3_role_26");
            getComponentByPath<Text>("playerInfo/contents/panel_add/tishi/no/yes/Text").text = ContMgr.getCont("a3_role_25");
            getComponentByPath<Text>("equip_no/Text").text = ContMgr.getCont("a3_role_27");
            getComponentByPath<Text>("HeroTitleInfo/Title/Text").text = ContMgr.getCont("a3_role_28");
            getComponentByPath<Text>("HeroTitleInfo/TitleProperty/Property/PropertyInfo/Viewport/Content/ApparelTitle/Title").text = ContMgr.getCont("a3_role_29");
            getComponentByPath<Text>("HeroTitleInfo/TitleProperty/Property/PropertyInfo/Viewport/Content/ActivateTitle/Title").text = ContMgr.getCont("a3_role_30");
            getComponentByPath<Text>("HeroTitleInfo/TitleProperty/Property/RequireText").text = ContMgr.getCont("a3_role_31");
            getComponentByPath<Text>("HeroTitleInfo/TitleProperty/Button/Text").text = ContMgr.getCont("a3_role_32");
            getComponentByPath<Text>("HeroTitleInfo/TitleClassify/Classify/ClassifyInfo1/Text").text = ContMgr.getCont("a3_role_33");
            getComponentByPath<Text>("HeroTitleInfo/TitleClassify/Bottom/ConcealToggle/Label").text = ContMgr.getCont("a3_role_34");
            getComponentByPath<Text>("HeroTitleInfo/TitleClassify/Bottom/Toggle/Label").text = ContMgr.getCont("a3_role_35");
            #endregion

            


            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onclose;

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("playerInfo/contents/panel_attr/attr_scroll/scroll").GetComponent<ScrollRect>();
            scrollControler.create(scroll);

            rolePanel = new GameObject[2];
            rolePanel[0] = transform.FindChild("playerInfo/contents/panel_attr").gameObject;
            rolePanel[1] = transform.FindChild("playerInfo/contents/panel_add").gameObject;

            roleAvatar = transform.FindChild("avatar/RawImage").GetComponent<RawImage>();

            isCanAddText = rolePanel[1].transform.FindChild("btn_add_do/Text").GetComponent<Text>();

            BaseButton btn_addpt = new BaseButton(rolePanel[1].transform.FindChild("btn_add_do"));
            btn_addpt.onClick = onbtnAdd;

            BaseButton btn_clearpt = new BaseButton(rolePanel[1].transform.FindChild("btn_clear"));
            btn_clearpt.onClick = onbtnClear;

            BaseButton yes_clear = new BaseButton(rolePanel[1].transform.FindChild("tishi/can/yes"));
            yes_clear.onClick = on_yesClear;

            BaseButton no_clear = new BaseButton(rolePanel[1].transform.FindChild("tishi/can/no"));
            no_clear.onClick = on_noClear;

            BaseButton clealback = new BaseButton(rolePanel[1].transform.FindChild("tishi/no/yes"));
            clealback.onClick = onClealback;

            //称号
            BaseButton heroTitle_Button = new BaseButton(transform.FindChild("HeroTitleButton"));
            heroTitle_Button.onClick = (GameObject go) =>
           {
               transform.FindChild("HeroTitleInfo").gameObject.SetActive(true);
           };
            TitleData_Init();
            TitleUI_Init();
            //BaseButton btn_bag = new BaseButton(transform.FindChild("btn_bag"));
            //btn_bag.onClick = onbtnBag;

            //BaseButton up = new BaseButton(this.transform.FindChild("dye/up"));
            //up.onClick = doUp;

            //BaseButton down = new BaseButton(this.transform.FindChild("dye/down"));
            //down.onClick = doDown;

            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_2/magic").gameObject).onEnter = (GameObject go) =>
              {
                  rolePanel[1].transform.FindChild("btn_2/des").FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("src_attr_hint_2_" + PlayerModel.getInstance().profession);
                  rolePanel[1].transform.FindChild("btn_2/des").gameObject.SetActive(true);
              };
            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_2/magic").gameObject).onExit = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_2/des").gameObject.SetActive(false);
            };

            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_3/minjie").gameObject).onEnter = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_3/des").FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("src_attr_hint_3_" + PlayerModel.getInstance().profession);
                rolePanel[1].transform.FindChild("btn_3/des").gameObject.SetActive(true);
            };
            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_3/minjie").gameObject).onExit = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_3/des").gameObject.SetActive(false);
            };

            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_1/strength").gameObject).onEnter = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_1/des").FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("src_attr_hint_1_" + PlayerModel.getInstance().profession);
                rolePanel[1].transform.FindChild("btn_1/des").gameObject.SetActive(true);
            };
            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_1/strength").gameObject).onExit = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_1/des").gameObject.SetActive(false);
            };

            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_4/Image").gameObject).onEnter = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_4/des").FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("src_attr_hint_4_" + PlayerModel.getInstance().profession);
                rolePanel[1].transform.FindChild("btn_4/des").gameObject.SetActive(true);
            };
            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_4/Image").gameObject).onExit = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_4/des").gameObject.SetActive(false);
            };

            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_5/Image").gameObject).onEnter = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_5/des").FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("src_attr_hint_5_" + PlayerModel.getInstance().profession);
                rolePanel[1].transform.FindChild("btn_5/des").gameObject.SetActive(true);
            };
            EventTriggerListener.Get(rolePanel[1].transform.FindChild("btn_5/Image").gameObject).onExit = (GameObject go) =>
            {
                rolePanel[1].transform.FindChild("btn_5/des").gameObject.SetActive(false);
            };

            //this.getEventTrigerByPath("dye").onDrag = onDragIcon;
            tabCtrl = new TabControl();
            tabCtrl.onClickHanle = onTab;
            tabCtrl.create(this.getGameObjectByPath("playerInfo/panelTab"), this.gameObject);
            if (forceIndex != -1)
                tabCtrl.setSelectedIndex(ForceIndex);

            //摄像机视距图片大小比例
            if (cemaraRectTran == null)
                cemaraRectTran = GameObject.Find("canvas").GetComponent<RectTransform>();
            RectTransform rt = cemaraRectTran;
            RectTransform bg1 = this.transform.FindChild("ig_bg1").GetComponent<RectTransform>();
            RectTransform thisRect = this.GetComponent<RectTransform>();
            bg1.sizeDelta = new Vector2(rt.rect.width, rt.rect.height);
            thisRect.sizeDelta = new Vector2(rt.rect.width, rt.rect.height);


            this.getEventTrigerByPath("avatar/avatar_touch").onDrag = OnDrag;



            string file = "";
            switch (PlayerModel.getInstance().profession)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_h2";
                    break;
                case 3:
                    file = "icon_job_icon_h3";
                    break;
                case 4:
                    file = "icon_job_icon_h4";
                    break;
                case 5:
                    file = "icon_job_icon_h5";
                    break;
            }
            Image head = transform.FindChild("playerInfo/contents/panel_attr/hero_ig/ig").GetComponent<Image>();
            head.sprite = GAMEAPI.ABUI_LoadSprite(file);

            #region 染色暂时用不上
            // btn_dye = new BaseButton(transform.FindChild("btn_dye"));
            //btn_dye.onClick = (GameObject go) => 
            //{
            //    go.transform.FindChild("isthis").gameObject.SetActive(true);
            //    dye_backinfo.transform.FindChild("isthis").gameObject.SetActive(false);
            //    showDye();
            //};

            //dye_backinfo = new BaseButton(transform.FindChild("backinfo"));
            //dye_backinfo.onClick = (GameObject go) => 
            //{
            //    go.transform.FindChild("isthis").gameObject.SetActive(true);
            //    btn_dye.transform.FindChild("isthis").gameObject.SetActive(false);
            //    hideDye();
            //};
            #endregion
            for (int i = 1; i <= 10; i++)
            {
                icon_ani[i] = this.transform.FindChild("ig_bg1/ain" + i).GetComponent<Image>();
            }

            initAttr();
            initPointAttr();
            initDye();

            //btnWing = new BaseButton(this.getTransformByPath("btn_wing"));
            //btnWing.onClick = OnEquWing;


        }

        public override void onShowed()
        {
            refreshAttr();
            refreshAttPoint();
            PlayerModel.getInstance().addEventListener(PlayerModel.ON_ATTR_CHANGE, onAttrChange);
            PlayerInfoProxy.getInstance().addEventListener(PlayerInfoProxy.EVENT_ADD_POINT, onAddPt);
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_SHOW_CHANGE, OnShowStageChange);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_USE_DYE, refreshDye);
            a3_HeroTitleServer.getInstance().addEventListener(a3_HeroTitleServer.SET_SHOW_TITLE, ChangeIsShowTilte);
            a3_HeroTitleServer.getInstance().addEventListener(a3_HeroTitleServer.LOCKE_TITLE, SetLockTitle);

            onAttrChange(null);
            //Invoke("closeCam", 0.02f);

            if (uiData != null)
            {
                int i = (int)uiData[0];
                tabCtrl.setSelectedIndex(i, true);
            }
            initEquipIcon();
            //dye_backinfo.transform.FindChild("isthis").gameObject.SetActive(true);
            // btn_dye.transform.FindChild("isthis").gameObject.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);

            GRMap.GAME_CAMERA.SetActive(false);
            createAvatar();
            refreshEquip();
            //延时1秒隐藏摄像机，解决一些手机上显示的bug
            //CancelInvoke("showCam");
            //Invoke("showCam", 0.2f);
            setAni();
            SetAni_Color();
            UiEventCenter.getInstance().onWinOpen(uiName);

            SetTitleUI();
        }
        //void showCam()
        //{
        //    if (isshow != null)
        //    {
        //        createAvatar();
        //    }
        //}
        public override void onClosed()
        {
            instan = null;
            disposeAvatar();

            PlayerModel.getInstance().removeEventListener(PlayerModel.ON_ATTR_CHANGE, onAttrChange);
            PlayerInfoProxy.getInstance().removeEventListener(PlayerInfoProxy.EVENT_ADD_POINT, onAddPt);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_SHOW_CHANGE, OnShowStageChange);
            A3_WingProxy.getInstance().removeEventListener(BagProxy.EVENT_USE_DYE, refreshDye);
            a3_HeroTitleServer.getInstance().removeEventListener(a3_HeroTitleServer.SET_SHOW_TITLE, ChangeIsShowTilte);
            a3_HeroTitleServer.getInstance().removeEventListener(a3_HeroTitleServer.LOCKE_TITLE, SetLockTitle);

            hideDye();
            rolePanel[1].transform.FindChild("tishi").gameObject.SetActive(false);

            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);

            isAdd = false;
            isReduce = false;

            GRMap.GAME_CAMERA.SetActive(true);
        }

        //void closeCam()
        //{

        //    CancelInvoke("closeCam");
        //}
        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
            //if (scene_Obj != null)
            //{
            //    scene_Obj.transform.FindChild("scene_ta").Rotate(0, -delta.x,0,0);
            //}
        }

        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 167, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else
                {
                    return;
                }

                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
                }

                Transform cur_model = m_SelfObj.transform.FindChild("model");

                //手上的小火球
                if (SelfRole._inst is P3Mage)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                m_proAvatar = new ProfessionAvatar();
                m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));

                m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
                m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
                m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
                m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
                m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());


                //cam.targetTexture = roleTexture;
                //roleAvatar.texture = roleTexture;

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
            }
        }
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (m_Self_Camera != null) GameObject.Destroy(m_Self_Camera);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }

        void Update()
        {
            if (isAdd || isReduce)
            {
                addTime -= Time.deltaTime;
                if (addTime < 0)
                {
                    rateTime += 0.05f;
                    addTime = 0.5f - rateTime;
                    if (cur_att_pt.ContainsKey(addType))
                    {
                        if (isAdd)
                        {
                            cur_att_pt[addType]++;
                            left_pt_num--;
                        }
                        if (isReduce)
                        {
                            cur_att_pt[addType]--;
                            left_pt_num++;
                        }
                        rolePanel[1].transform.FindChild("num").GetComponent<Text>().text = left_pt_num.ToString();
                        rolePanel[1].transform.FindChild("btn_" + addType + "/value").GetComponent<Text>().text = cur_att_pt[addType].ToString();
                        checkLeftPtNum();
                    }
                }
            }

            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        void checkLeftPtNum()
        {
            if (left_pt_num <= 0)
            {
                left_pt_num = 0;
                foreach (Button btn in btn_pt_add.Values)
                {
                    btn.interactable = false;
                    isAdd = false;
                }
            }
            else
            {
                foreach (Button btn in btn_pt_add.Values)
                {
                    btn.interactable = true;
                }
            }
            if (left_pt_num < PlayerModel.getInstance().pt_att)
            {
                foreach (int key in cur_att_pt.Keys)
                {
                    if (cur_att_pt[key] > 0)
                        btn_pt_reduce[key].interactable = true;
                    else
                        btn_pt_reduce[key].interactable = false;
                }
                isCanAdd = true;
                isCanAddText.text = ContMgr.getCont("a3_role_sure");
            }
            else
            {
                isReduce = false;
                foreach (int key in cur_att_pt.Keys)
                {
                    btn_pt_reduce[key].interactable = false;
                }
                isCanAdd = false;
                isCanAddText.text = ContMgr.getCont("a3_role_addpoint");
            }

            if (cur_att_pt.ContainsKey(addType))
            {
                if (cur_att_pt[addType] <= 0)
                {
                    isReduce = false;
                }
            }
        }
        void refreshAttr()
        {
            transform.FindChild("playerInfo/contents/panel_attr/name").GetComponent<Text>().text = PlayerModel.getInstance().name;
            transform.FindChild("playerInfo/contents/panel_attr/lv").GetComponent<Text>().text = "Lv" + PlayerModel.getInstance().lvl + "（" + PlayerModel.getInstance().up_lvl + ContMgr.getCont("zhuan") + "）";
            if (A3_LegionModel.getInstance().myLegion == null || A3_LegionModel.getInstance().myLegion.clname == "")
            {
                transform.FindChild("playerInfo/contents/panel_attr/team").gameObject.SetActive(false);
                transform.FindChild("playerInfo/contents/panel_attr/no_team").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("playerInfo/contents/panel_attr/team").gameObject.SetActive(true);
                transform.FindChild("playerInfo/contents/panel_attr/team/team_name").GetComponent<Text>().text = A3_LegionModel.getInstance().myLegion.clname;
                transform.FindChild("playerInfo/contents/panel_attr/no_team").gameObject.SetActive(false);
            }
        }
        void refreshAttPoint()
        {
            if (PlayerModel.getInstance().profession == 3)
            {//法师没有敏捷
                cur_att_pt[2] = 0;
                true_att_pt[2] = PlayerModel.getInstance().attr_list[4];
            }
            else
            {
                cur_att_pt[3] = 0;
                true_att_pt[3] = PlayerModel.getInstance().attr_list[2];
            }
            cur_att_pt[1] = 0;
            cur_att_pt[4] = 0;
            cur_att_pt[5] = 0;

            true_att_pt[1] = PlayerModel.getInstance().attr_list[1];
            true_att_pt[4] = PlayerModel.getInstance().attr_list[3];
            true_att_pt[5] = PlayerModel.getInstance().attr_list[34];

            foreach (int type in cur_att_pt.Keys)
            {
                rolePanel[1].transform.FindChild("btn_" + type + "/value").GetComponent<Text>().text = cur_att_pt[type].ToString();
                rolePanel[1].transform.FindChild("btn_" + type + "/value_all").GetComponent<Text>().text = (true_att_pt[type]).ToString();
            }

            left_pt_num = PlayerModel.getInstance().pt_att;

            rolePanel[1].transform.FindChild("num").GetComponent<Text>().text = left_pt_num.ToString();

            checkLeftPtNum();
        }
        //1.力量   2.魔力 3.敏捷 4.体力  5.智慧
        void refreshAttPointAuto()
        {
            SXML Xml = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + PlayerModel.getInstance().profession);
            string[] type = Xml.GetNode("point").getString("type").Split(',');
            string[] volue = Xml.GetNode("point").getString("volue").Split(',');
            int[] addtype = { int.Parse(type[0]), int.Parse(type[1]), int.Parse(type[2]), int.Parse(type[3]) };
            float[] tem = { float.Parse(volue[0]), float.Parse(volue[1]), float.Parse(volue[2]), float.Parse(volue[3]) };
            addPointAuto(left_pt_num, addtype, tem);
            //if ( PlayerModel.getInstance().profession == 2 )
            //{//战士
            //    int[] addtype = {int.Parse (type[0]), int.Parse(type[1]), int.Parse(type[2]), int.Parse(type[3]) };
            //    float[] tem = { float .Parse (volue[0]), float.Parse(volue[1]), float.Parse(volue[2]), float.Parse(volue[3]) };
            //    //float[] tem = {0.27f,0.33f,0.21f,0.19f};
            //    addPointAuto( left_pt_num , addtype , tem );
            //}
            //if ( PlayerModel.getInstance().profession == 3 )
            //{//法师
            //    int[] addtype = { 2, 4, 1, 5 };
            //    float[] tem = {0.43f,  0.23f, 0.17f, 0.17f };
            //    addPointAuto( left_pt_num , addtype , tem );
            //}
            //if ( PlayerModel.getInstance().profession == 5 )
            //{//刺客
            //    int[] addtype = { 3,  1, 4, 5 };
            //    float[] tem = { 0.43f, 0.17f, 0.23f, 0.17f };
            //    addPointAuto( left_pt_num , addtype , tem );
            //}
        }
        void addPointAuto(int left_num, int[] addtype, float[] tem)
        {
            int[] add = { 0, 0, 0, 0 };

            float sum = 0;
            for (int i = 0; i < tem.Length; i++)
            {
                sum += tem[i];
            }
            float a = left_num / sum;
            //int b = left_num % (int)sum;

            for (int i = 0; i < 4; i++)
            {
                add[i] = (int)(tem[i] * a);
            }

            int b = left_num - (add[0] + add[1] + add[2] + add[3]);

            if (b > 0)
            {
                int over_num = b;
                while (over_num > 0)
                {
                    if (over_num >= 4)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        add[2] += 1;
                        add[3] += 1;
                        over_num -= 4;
                    }
                    if (over_num >= 3)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        add[2] += 1;
                        over_num -= 3;
                    }
                    if (over_num >= 2)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        over_num -= 2;
                    }
                    if (over_num >= 1)
                    {
                        add[0] += 1;
                        over_num -= 1;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                cur_att_pt[addtype[i]] = add[i];
            }
            foreach (int type in cur_att_pt.Keys)
            {
                rolePanel[1].transform.FindChild("btn_" + type + "/value").GetComponent<Text>().text = cur_att_pt[type].ToString();
            }

            left_pt_num = 0;
            rolePanel[1].transform.FindChild("num").GetComponent<Text>().text = left_pt_num.ToString();

            checkLeftPtNum();
        }

        //private IEnumerator LoadScene()
        //{
        //    async = Application.LoadLevelAsync("show_scene");
        //    yield return async;

        //    //if (MediaClient.getInstance()._curMusicUrl != "audio_map_music_0")
        //    //{
        //    //    MediaClient.getInstance().PlayMusicUrl("audio_map_music_0", null, true);

        //    //    Application.DontDestroyOnLoad(GameObject.Find("Audio"));
        //    //}
        //}

        void onTab(TabControl t)
        {
            rolePanel[0].SetActive(false);
            rolePanel[1].SetActive(false);
            if (t.getSeletedIndex() == 0)
            {
                rolePanel[0].SetActive(true);
                refreshAttr();
            }
            else
            {
                rolePanel[1].SetActive(true);
            }
        }
        public void initEquipIcon()
        {
            for (int i = 1; i <= 10; i++)
            {
                GameObject go = transform.FindChild("ig_bg1/txt" + i).gameObject;
                go.GetComponent<Text>().enabled = true;
                if (go.transform.childCount > 0)
                {
                    Destroy(go.transform.GetChild(0).gameObject);
                }
            }
            equipicon.Clear();

            Dictionary<int, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquipsByType();
            foreach (int i in equips.Keys)
            {
                a3_BagItemData data = equips[i];
                if (data.confdata.equip_type == 11 || data.confdata.equip_type == 12)
                    break;
                CreateEquipIcon(data);
            }
        }
        void CreateEquipIcon(a3_BagItemData data)
        {
            GameObject icon;
            icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            IconImageMgr.getInstance().refreshA3EquipIcon_byType(icon, data, EQUIP_SHOW_TYPE.SHOW_INTENSIFYANDSTAGE);

            GameObject parent = transform.FindChild("ig_bg1/txt" + data.confdata.equip_type).gameObject;
            icon.transform.SetParent(parent.transform, false);
            parent.GetComponent<Text>().enabled = false;
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            equipicon[data.confdata.equip_type] = icon;
            icon.transform.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) { this.onEquipClick(icon, data.id); };
        }
        void onEquipClick(GameObject go, uint id)
        {
            ArrayList data = new ArrayList();
            a3_BagItemData one = a3_EquipModel.getInstance().getEquips()[id];
            data.Add(one);
            data.Add(equip_tip_type.tip_ForLook);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
        }
        void setAni()
        {
            foreach (int i in icon_ani.Keys)
            {
                if (a3_EquipModel.getInstance().active_eqp.ContainsKey(i)) { icon_ani[i].gameObject.SetActive(true); }
                else { icon_ani[i].gameObject.SetActive(false); }
            }
        }
        public void SetAni_Color()
        {
            foreach (int type in a3_EquipModel.getInstance().getEquipsByType().Keys)
            {
                if (type == 11 || type == 12)
                    break;

                Color col = new Color();
                switch (a3_EquipModel.getInstance().getEquipsByType()[type].equipdata.attribute)
                {
                    case 1:
                        col = new Color(0f, 0.47f, 0f);
                        break;
                    case 2:
                        col = new Color(0.68f, 0.26f, 0.03f);
                        break;
                    case 3:
                        col = new Color(0.76f, 0.86f, 0.33f);
                        break;
                    case 4:
                        col = new Color(0.97f, 0.11f, 0.87f);
                        break;
                    case 5:
                        col = new Color(0.17f, 0.18f, 0.57f);
                        break;
                }
                icon_ani[type].GetComponent<Image>().color = col;
            }
        }
        void initAttr()
        {
            GameObject item = transform.FindChild("playerInfo/contents/panel_attr/attr_scroll/scroll/item").gameObject;
            GameObject parent = transform.FindChild("playerInfo/contents/panel_attr/attr_scroll/scroll/contain").gameObject;
            SXML xml = XMLMgr.instance.GetSXML("carrlvl");
            string str = xml.GetNode("att_show").getString("att_type");
            string[] att = str.Split(',');
            int index = 0;
            for (int m = 0; m < att.Length; m++)
            {
                //if (i == 1 || i == 2 || i == 3 || i == 4)
                //    continue;
                //if (i == 29 || i == 30 || i == 31 || i == 32 || i == 34 || i == 35 || i == 36 || i == 37 || i == 38 || i == 39 || i == 40 || i == 41 || i == 42)
                //    continue;
                //if (i == 28 || i == 17 || i== 21||i==24||i==25||i==18||i==39||i == 40||i==41||i==42||i==27||i==26)
                //    continue;

                index++;
                GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
                itemclone.SetActive(true);
                Text name = itemclone.transform.FindChild("name").GetComponent<Text>();
                Text value = itemclone.transform.FindChild("value").GetComponent<Text>();

                //if (i == 5)
                //    name.text = "攻击力:" + PlayerModel.getInstance().attr_list[38] + "-" + PlayerModel.getInstance().attr_list[5];
                //else
                //name.text = Globle.getAttrAddById(i, PlayerModel.getInstance().attr_list[(uint)i]);
                int i = int.Parse(att[m]);
                name.text = Globle.getAttrNameById(i) + "：";
                if (i == 5)
                {
                    name.text = ContMgr.getCont("a3_role_attack");
                    value.text = PlayerModel.getInstance().attr_list[38] + "-" + PlayerModel.getInstance().attr_list[5];
                }
                else if (i == 17 || i == 19 || i == 20 || i == 24 || i == 25 || i == 29 || i == 30 || i == 31 || i == 32
                        || i == 33 || i == 35 || i == 36 || i == 37 || i == 39 || i == 40 || i == 17 || i == 41)
                    value.text = "+" + (float)PlayerModel.getInstance().attr_list[(uint)i] / 10 + "%";
                else
                    value.text = "+" + PlayerModel.getInstance().attr_list[(uint)i].ToString();
                if (index % 2 != 0)
                    itemclone.transform.FindChild("ig_bg").gameObject.SetActive(false);
                itemclone.transform.SetParent(parent.transform, false);

                attr_text[i] = value;
            }

            float height = parent.transform.GetComponent<GridLayoutGroup>().cellSize.y;
            RectTransform rect = parent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0.0f, attr_text.Count * height);
        }
        void initPointAttr()
        {
            for (int i = 1; i <= 5; i++)
            {
                GameObject btn = rolePanel[1].transform.FindChild("btn_" + i).gameObject;

                if (PlayerModel.getInstance().profession == 3 && i == 3)
                {
                    btn.SetActive(false);
                }
                else if (PlayerModel.getInstance().profession != 3 && i == 2)
                {
                    btn.SetActive(false);
                }
                else
                {
                    Button btn_add = btn.transform.FindChild("btn_add").GetComponent<Button>();
                    Button btn_reduce = btn.transform.FindChild("btn_reduce").GetComponent<Button>();

                    int type = i;

                    EventTriggerListener.Get(btn_add.gameObject).onDown = delegate (GameObject go) { this.onClickAdd(go, type); };
                    EventTriggerListener.Get(btn_add.gameObject).onExit = delegate (GameObject go) { this.onClickAddExit(go, type); };

                    EventTriggerListener.Get(btn_reduce.gameObject).onDown = delegate (GameObject go) { this.onClickReduce(go, type); };
                    EventTriggerListener.Get(btn_reduce.gameObject).onExit = delegate (GameObject go) { this.onClickReduceExit(go, type); };

                    btn_pt_add[type] = btn_add;
                    btn_pt_reduce[type] = btn_reduce;
                }
            }

            SXML s_xml = XMLMgr.instance.GetSXML("creat_character.character", "job_type==" + PlayerModel.getInstance().profession);
            List<SXML> xml = s_xml.GetNodeList("character");
            foreach (SXML x in xml)
            {
                switch (x.getInt("att_type"))
                {
                    case 1:
                        base_att_pt[1] = x.getInt("att_value");
                        break;
                    case 2:
                        base_att_pt[3] = x.getInt("att_value");
                        break;
                    case 3:
                        base_att_pt[4] = x.getInt("att_value");
                        break;
                    case 4:
                        base_att_pt[2] = x.getInt("att_value");
                        break;
                    case 34:
                        base_att_pt[5] = x.getInt("att_value");
                        break;
                }

            }
        }

        void onClickAdd(GameObject go, int type)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;

            isAdd = true;
            addType = type;
            rateTime = 0.0f;
            addTime = 0.5f;

            if (cur_att_pt.ContainsKey(addType))
            {
                cur_att_pt[addType]++;
                rolePanel[1].transform.FindChild("btn_" + type + "/value").GetComponent<Text>().text = cur_att_pt[addType].ToString();
                left_pt_num--;
                rolePanel[1].transform.FindChild("num").GetComponent<Text>().text = left_pt_num.ToString();
                checkLeftPtNum();
            }
        }
        void onClickAddExit(GameObject go, int type)
        {
            isAdd = false;
        }

        void onClickReduce(GameObject go, int type)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;

            isReduce = true;
            addType = type;
            rateTime = 0.0f;
            addTime = 0.5f;

            if (cur_att_pt.ContainsKey(addType))
            {
                cur_att_pt[addType]--;
                rolePanel[1].transform.FindChild("btn_" + type + "/value").GetComponent<Text>().text = cur_att_pt[addType].ToString();
                left_pt_num++;
                rolePanel[1].transform.FindChild("num").GetComponent<Text>().text = left_pt_num.ToString();
                checkLeftPtNum();
            }
        }//
        void onClickReduceExit(GameObject go, int type)
        {
            isReduce = false;
        }

        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ROLE);
        }


        void onbtnAdd(GameObject go)
        {
            if (isCanAdd)
            {
                Dictionary<int, int> add_pt = new Dictionary<int, int>();
                foreach (int key in cur_att_pt.Keys)
                {
                    if (cur_att_pt[key] > 0)
                        add_pt[key] = cur_att_pt[key];
                }
                if (add_pt.Count > 0)
                    PlayerInfoProxy.getInstance().sendAddPoint(0, add_pt);
            }
            else
            {
                refreshAttPointAuto();
            }
        }

        void onbtnClear(GameObject go)
        {
            //PlayerInfoProxy.getInstance().sendAddPoint(1, null);
            rolePanel[1].transform.FindChild("tishi").gameObject.SetActive(true);
            SXML xml = XMLMgr.instance.GetSXML("carrlvl");
            string str = xml.GetNode("points_reset").getString("cost");
            if (PlayerModel.getInstance().up_lvl == 0 && PlayerModel.getInstance().lvl <= 80)
            {
                rolePanel[1].transform.FindChild("tishi/text").GetComponent<Text>().text = ContMgr.getCont("a3_role_openlock");
                rolePanel[1].transform.FindChild("tishi/no").gameObject.SetActive(true);
                rolePanel[1].transform.FindChild("tishi/can").gameObject.SetActive(false);
            }
            else
            {
                rolePanel[1].transform.FindChild("tishi/text").GetComponent<Text>().text = ContMgr.getCont("a3_role_needcost") + str + ContMgr.getCont("a3_role_zsnum");
                rolePanel[1].transform.FindChild("tishi/no").gameObject.SetActive(false);
                rolePanel[1].transform.FindChild("tishi/can").gameObject.SetActive(true);
            }
        }


        void on_yesClear(GameObject go)
        {
            PlayerInfoProxy.getInstance().sendAddPoint(1, null);
            rolePanel[1].transform.FindChild("tishi").gameObject.SetActive(false);
        }

        void onClealback(GameObject go)
        {
            rolePanel[1].transform.FindChild("tishi").gameObject.SetActive(false);
        }
        void on_noClear(GameObject go)
        {
            rolePanel[1].transform.FindChild("tishi").gameObject.SetActive(false);
        }
        //void onbtnBag(GameObject go)
        //{
        //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ROLE);
        //    InterfaceMgr.getInstance().open(InterfaceMgr.A3_BAG);
        //}

        void onAttrChange(GameEvent e)
        {
            foreach (int key in attr_text.Keys)
            {
                if (key == 5)
                    attr_text[key].text = PlayerModel.getInstance().attr_list[38] + "-" + PlayerModel.getInstance().attr_list[5];
                else if (key == 17 || key == 19 || key == 20 || key == 24 || key == 25 || key == 29 || key == 30 || key == 31 || key == 32
                    || key == 33 || key == 35 || key == 36 || key == 37 || key == 39 || key == 40 || key == 17 || key == 41)
                    attr_text[key].text = (float)PlayerModel.getInstance().attr_list[(uint)key] / 10 + "%";
                else
                    attr_text[key].text = PlayerModel.getInstance().attr_list[(uint)key].ToString();
            }
            transform.FindChild("playerInfo/contents/panel_attr/fighting/value").GetComponent<Text>().text = PlayerModel.getInstance().combpt.ToString();

            refresh_equip();
            refreshEquip();
        }
        void onAddPt(GameEvent e)
        {
            refreshAttPoint();
        }

        void initDye()
        {

        }

        private Dictionary<int, GameObject> equipicon = new Dictionary<int, GameObject>();

        public void refresh_equip()
        {

            Dictionary<uint, a3_BagItemData> equip = a3_EquipModel.getInstance().getEquips();
            foreach (uint j in equip.Keys)
            {
                if (!a3_EquipModel.getInstance().checkisSelfEquip(equip[j].confdata))
                {
                }
                else
                {
                    if (!a3_EquipModel.getInstance().checkCanEquip(equip[j].confdata, equip[j].equipdata.stage, equip[j].equipdata.blessing_lv))
                    {
                        if (equipicon.ContainsKey(equip[j].confdata.equip_type) && equipicon[equip[j].confdata.equip_type] != null)
                            equipicon[equip[j].confdata.equip_type].transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                    }
                    else
                    {

                        if (equipicon.ContainsKey(equip[j].confdata.equip_type) && equipicon[equip[j].confdata.equip_type] != null)
                        {
                            equipicon[equip[j].confdata.equip_type].transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
                        }
                    }
                }
            }

        }



        public void refreshEquip()
        {
            Dictionary<uint, a3_BagItemData> equip = a3_EquipModel.getInstance().getEquips();
            bool show_equip_no = false;
            foreach (uint i in equip.Keys)
            {
                if (!a3_EquipModel.getInstance().checkCanEquip(equip[i].confdata, equip[i].equipdata.stage, equip[i].equipdata.blessing_lv))
                {
                    show_equip_no = true;
                    break;
                }
            }
            if (show_equip_no)
            {
                this.transform.FindChild("equip_no").gameObject.SetActive(true);
            }
            else
            {
                this.transform.FindChild("equip_no").gameObject.SetActive(false);
            }
        }

        void showDye()
        {

            //         //transform.FindChild("dye/Scrollbar").GetComponent<Scrollbar>().value = 1;
            //         dye_use.interactable = false;
            //         transform.FindChild("playerInfo").gameObject.SetActive(false);
            //transform.FindChild("dye").gameObject.SetActive(true);
            //         //if (!a3_EquipModel.getInstance().getEquipsByType().ContainsKey(3)) {
            //         //	transform.FindChild("dye/name/0").gameObject.SetActive(false);
            //         //	transform.FindChild("dye/name/1").gameObject.SetActive(true);
            //         //	flytxt.instance.fly("未穿戴可染色的装备，不能进行染色操作哦！");
            //         //}
            //         //else {
            //         //	if (m_proAvatar!=null)
            //         //		m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());
            //         //	transform.FindChild("dye/name/0").gameObject.SetActive(true);
            //         //	transform.FindChild("dye/name/1").gameObject.SetActive(false);
            //         //	string namestr =  a3_BagModel.getInstance().getEquipNameInfo(a3_EquipModel.getInstance().getEquipsByType()[3]);
            //         //	transform.FindChild("dye/name/0/name").GetComponent<Text>().text = namestr;
            //         //}

            //         foreach (var d in dyes.Values)
            //         {
            //             var sl = d.transform.FindChild("select");
            //             sl.gameObject.SetActive(false);
            //         }
            //         refreshDye(null);
        }
        void hideDye()
        {
            ////BagProxy.getInstance().removeEventListener(BagProxy.EVENT_USE_DYE, refreshDye);
            //transform.FindChild("dye").gameObject.SetActive(false);
            //         transform.FindChild("playerInfo").gameObject.SetActive(true);
            ////transform.FindChild("dye/dye_help/panel_help").gameObject.SetActive(false);
            ////if (m_proAvatar!=null)
            ////	m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());
        }


        void doUp(GameObject go)
        {
            goUp();
        }
        void doDown(GameObject go)
        {
            goDown();
        }
        Vector3 upVec = new Vector3();
        void goUp()
        {
            if (!CanRun)
                return;
            CanRun = false;
            Tween tween = null;
            //for (int i = 0; i< listPage.Count; i++)
            //{
            //    int nextIndex = i + 1;
            //    if (nextIndex >= listPage.Count)
            //    {
            //        nextIndex = 0;
            //    }

            //}           
            upVec = listIcon[listIcon.Count - 1].transform.localPosition;
            for (int i = 0; i < listIcon.Count; i++)
            {
                Vector3 nowVec = listIcon[i].transform.localPosition;
                tween = listIcon[i].transform.DOLocalMove(upVec, 1f);
                if (listIcon[i].transform.localPosition.y >= 280 || listIcon[i].transform.localPosition.x >= 100)
                {
                    listIcon[i].gameObject.SetActive(false);
                }
                else
                    listIcon[i].gameObject.SetActive(true);
                //listIcon[i].transform.localPosition = upVec;

                upVec = nowVec;
            }
            tween.OnComplete(() => OnComplete());
        }
        void goDown()
        {
            if (!CanRun)
                return;
            CanRun = false;
            Tween tween = null;
            upVec = listIcon[0].transform.localPosition;
            for (int i = listIcon.Count - 1; i >= 0; i--)
            {
                Vector3 nowVec = listIcon[i].transform.localPosition;
                tween = listIcon[i].transform.DOLocalMove(upVec, 1f);
                if (listIcon[i].transform.localPosition.y <= -280 || listIcon[i].transform.localPosition.x >= 100)
                {
                    listIcon[i].gameObject.SetActive(false);
                }
                else
                    listIcon[i].gameObject.SetActive(true);

                upVec = nowVec;
            }
            tween.OnComplete(() => OnComplete());
        }

        void OnComplete()
        {
            CanRun = true;
        }
        void onDragIcon(GameObject go, Vector2 delta)
        {
            float y = delta.y;

            if (y > 0)
            {
                goUp();
            }
            else if (y < 0)
            {
                goDown();
            }
        }
        public void refreshDye(GameEvent e)
        {

        }


        private void OnShowStageChange(GameEvent e)
        {
            m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
        }

        private void SetTitleUI()
        {
            GameObject content = transform.FindChild("HeroTitleInfo/TitleClassify/InfoList/Scroll View/Viewport/Content").gameObject;

            List<TitleData> itemListData = a3_HeroTitleServer.getInstance().gainTitleIdList;

            for (int i = itemListData.Count - 1; i >= 0; i--)
            {
                if ( itemListData[i].isforever == false && NetClient.instance.CurServerTimeStamp > itemListData[i].title_limit)
                {
                    itemListData.RemoveAt(i);
                }
            }

            if (a3_HeroTitleServer.getInstance().eqpTitleId != 0)
            {
                isAdornGo.transform.SetParent(content.transform.FindChild(a3_HeroTitleServer.getInstance().eqpTitleId + "").transform);
                isAdornGo.SetActive(true);
                isAdornGo.transform.localPosition = new Vector3(0, 0, 0);

            }
            else {

                isAdornGo.SetActive(false);
                titleIconGo.GetComponent<Image>().sprite = null;
                titleIconGo.SetActive(titleIconGo.GetComponent<Image>().sprite != null && a3_HeroTitleServer.getInstance().isShowTitle);

            }     

            for (int i = 0; i < content.transform.childCount ; i++)
            {
                content.transform.GetChild(i).FindChild("LockBg").gameObject.SetActive(true);
                content.transform.GetChild(i).FindChild("Icon").GetComponent<Image>().color = new Color(255f/255f, 255f/255f, 255f/255f, 100f/255f);
            }

            for (int i = 0; i < itemListData.Count; i++)
            {
                content.transform.FindChild(itemListData[i].title_id + "").FindChild("LockBg").gameObject.SetActive(false);
                content.transform.FindChild(itemListData[i].title_id + "").FindChild("Icon").GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }

           }

        private void TitleUI_Init()
        {
            //List
            GameObject content = transform.FindChild("HeroTitleInfo/TitleClassify/InfoList/Scroll View/Viewport/Content").gameObject;
            GameObject itemGo = content.transform.FindChild("Item").gameObject;
            isAdornGo = transform.FindChild("HeroTitleInfo/TitleClassify/InfoList/Scroll View/Viewport/isAdorn").gameObject;
             selectBgGo = transform.FindChild("HeroTitleInfo/TitleClassify/InfoList/Scroll View/Viewport/SelectBG").gameObject;

            //Info
            GameObject actText = transform.FindChild("HeroTitleInfo/TitleProperty/Property/PropertyInfo/Viewport/Content/ApparelTitle/Text").gameObject;
            GameObject eqpText = transform.FindChild("HeroTitleInfo/TitleProperty/Property/PropertyInfo/Viewport/Content/ActivateTitle/Text").gameObject;
            GameObject requireText = transform.FindChild("HeroTitleInfo/TitleProperty/Property/RequireText").gameObject;
            GameObject tiTleIcon = transform.FindChild("HeroTitleInfo/TitleProperty/TiTleIcon").gameObject;

            selectButton = transform.FindChild("HeroTitleInfo/TitleProperty/Button").gameObject;
            selectButton.GetComponent<Button>().interactable = false;
            selectButton.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_herotitle_no");
            equipped = transform.FindChild("HeroTitleInfo/TitleProperty/Equipped").gameObject;

            titleIconGo = transform.FindChild("HeroTitleIcon").gameObject;

            GameObject showGetToggle = transform.FindChild("HeroTitleInfo/TitleClassify/Bottom/Toggle").gameObject;
            GameObject concealToggle = transform.FindChild("HeroTitleInfo/TitleClassify/Bottom/ConcealToggle").gameObject;
            concealToggle.GetComponent<Toggle>().isOn = !a3_HeroTitleServer.getInstance().isShowTitle;


            int selectItemId = 1;

            List<RoleTitleXmlData> roleTitleXmlData = a3_HeroTitleServer.getInstance().roleTitleXmlData;
            Dictionary<int, RoleTitleXmlData> roleTitleData_Dic = a3_HeroTitleServer.getInstance().roleTitleData_Dic;

            //if ( content.transform.childCount != 0)
            //{
            //    for ( int i = 0 ; i < content.transform.childCount ; i++ )
            //    {
            //       Destroy( content.transform.GetChild( i ));
            //    }
            //}

            for (int i = 0; i < roleTitleXmlData.Count; i++)
            {
                GameObject _itemGo;
                if (i != 0)
                {
                    _itemGo = GameObject.Instantiate(itemGo);
                    _itemGo.transform.SetParent(content.transform);
                    _itemGo.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    _itemGo = itemGo;
                }

                _itemGo.name = roleTitleXmlData[i].title_id + "";
                _itemGo.transform.FindChild("Icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(roleTitleData_Dic[roleTitleXmlData[i].title_id].title_img);
                _itemGo.transform.FindChild("Icon").GetComponent<Image>().SetNativeSize();
                new BaseButton(_itemGo.transform).onClick = (GameObject oo) =>
                {
                    int index = int.Parse(_itemGo.name) - 1;
                    selectItemId = index + 1;
                    selectBgGo.transform.SetParent(_itemGo.transform);
                    selectBgGo.SetActive(true);
                    selectBgGo.transform.localPosition = new Vector3(0, 0, 0);
                    Dictionary<int, int> _act = roleTitleXmlData[index].nature_act;
                    string _actStr = "";
                    foreach (int key in _act.Keys)
                    {
                        _actStr = _actStr + ContMgr.getCont("globle_attr" + key) + "+" + _act[key] + "\n";
                    }
                    actText.GetComponent<Text>().text = _actStr;

                    Dictionary<int, int> _eqp = roleTitleXmlData[index].nature_eqp;
                    string _eqpStr = "";
                    foreach (int key in _eqp.Keys)
                    {
                        if (key == 30) //经验值
                        {
                            _eqpStr = _eqpStr + ContMgr.getCont("globle_attr" + key) + "+" + _eqp[key] + "%\n";
                        }
                        else
                        {
                            _eqpStr = _eqpStr + ContMgr.getCont("globle_attr" + key) + "+" + _eqp[key] + "\n";
                        }

                    }
                    eqpText.GetComponent<Text>().text = _eqpStr;
                    requireText.GetComponent<Text>().text = roleTitleXmlData[index].desc;
                    tiTleIcon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(roleTitleData_Dic[selectItemId].title_img);
                    tiTleIcon.GetComponent<Image>().SetNativeSize();

                    if (_itemGo.transform.FindChild("LockBg").gameObject.activeSelf)
                    {
                        selectButton.GetComponent<Button>().interactable = false;
                        selectButton.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_herotitle_no");
                        selectButton.SetActive(true);
                        equipped.SetActive(false);
                    }
                    else
                    {
                        if (selectItemId == a3_HeroTitleServer.getInstance().eqpTitleId)
                        {
                            selectButton.SetActive(false);
                            equipped.SetActive(true);
                        }
                        else
                        {
                            selectButton.SetActive(true);
                            equipped.SetActive(false);
                        }

                        selectButton.GetComponent<Button>().interactable = true;
                        selectButton.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_herotitle_get");
                    }

                };
            }

            selectBgGo.transform.SetParent(itemGo.transform, true);
            selectBgGo.SetActive(true);
            selectBgGo.transform.localPosition = new Vector3(0, 0, 0);
            selectBgGo.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            selectBgGo.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Dictionary<int, int> act = roleTitleXmlData[0].nature_act;
            string actStr = "";
            foreach (int key in act.Keys)
            {
                actStr = actStr + ContMgr.getCont("globle_attr" + key) + "+" + act[key] + "\n";
            }
            actText.GetComponent<Text>().text = actStr;

            Dictionary<int, int> eqp = roleTitleXmlData[0].nature_eqp;
            string eqpStr = "";
            foreach (int key in eqp.Keys)
            {
                if (key == 30) //经验值
                {
                    eqpStr = eqpStr + ContMgr.getCont("globle_attr" + key) + "+" + eqp[key] + "%\n";
                }
                else
                {
                    eqpStr = eqpStr + ContMgr.getCont("globle_attr" + key) + "+" + eqp[key] + "\n";
                }
            }
            eqpText.GetComponent<Text>().text = eqpStr;
            requireText.GetComponent<Text>().text = roleTitleXmlData[0].desc;
            tiTleIcon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(roleTitleData_Dic[1].title_img);
            tiTleIcon.GetComponent<Image>().SetNativeSize();

            if (a3_HeroTitleServer.getInstance().eqpTitleId != 0)
            {
                isAdornGo.transform.SetParent(content.transform.FindChild(a3_HeroTitleServer.getInstance().eqpTitleId + "").transform);
                isAdornGo.SetActive(true);
                isAdornGo.transform.localPosition = new Vector3(0, 0, 0);
                if (a3_HeroTitleServer.getInstance().isShowTitle)
                {
                    titleIconGo.SetActive(true);
                }
                titleIconGo.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(roleTitleData_Dic[a3_HeroTitleServer.getInstance().eqpTitleId].title_img);
                titleIconGo.GetComponent<Image>().SetNativeSize();
                if (a3_HeroTitleServer.getInstance().eqpTitleId == 1)
                {
                    selectButton.SetActive(false);
                    equipped.SetActive(true);
                }
            }
            else
            {
                titleIconGo.SetActive(false);
            }

            new BaseButton(selectButton.transform).onClick = (GameObject oo) =>
            {
                debug.Log(selectItemId + "----------------------");

                if ( selectItemId != 0)
                {
                    for (int i = a3_HeroTitleServer.getInstance().gainTitleIdList.Count - 1 ; i >= 0; i--)
                    {
                        if  ( a3_HeroTitleServer.getInstance().gainTitleIdList[i].isforever == false && selectItemId == a3_HeroTitleServer.getInstance().gainTitleIdList[i].title_id )
                        {
                            if (NetClient.instance.CurServerTimeStamp > a3_HeroTitleServer.getInstance().gainTitleIdList[i].title_limit)
                            {
                                Globle.err_output(-7811);

                                return;
                            }
                        }
                    }
                }

                Variant v = new Variant();
                v["title_sign"] = selectItemId;
                a3_HeroTitleServer.getInstance().SendMsg(a3_HeroTitleServer.SET_TITLE, v);
                selectButton.gameObject.SetActive(false);
                equipped.SetActive(true);

                isAdornGo.transform.SetParent(content.transform.FindChild(selectItemId + "").transform);
                isAdornGo.SetActive(true);
                isAdornGo.transform.localPosition = new Vector3(0, 0, 0);

                titleIconGo.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(roleTitleData_Dic[selectItemId].title_img);
                titleIconGo.gameObject.SetActive(titleIconGo.GetComponent<Image>().sprite!=null && a3_HeroTitleServer.getInstance().isShowTitle);
                titleIconGo.GetComponent<Image>().SetNativeSize();

            };

            showGetToggle.GetComponent<Toggle>().onValueChanged.AddListener((bool b) =>
           {
               if (b)
               {
                   for (int i = 0; i < roleTitleXmlData.Count; i++)
                   {
                       GameObject go = content.transform.FindChild(roleTitleXmlData[i].title_id + "").gameObject;
                       if (go.transform.FindChild("LockBg").gameObject.activeSelf)
                       {
                           go.SetActive(false);
                       }
                   }
               }
               else
               {
                   for (int i = 0; i < roleTitleXmlData.Count; i++)
                   {
                       GameObject go = content.transform.FindChild(roleTitleXmlData[i].title_id + "").gameObject;
                       go.SetActive(true);
                   }

               }
           });

            //隐藏头衔
            concealToggle.GetComponent<Toggle>().onValueChanged.AddListener((bool b) =>
           {
               Variant v = new Variant();
               v["display"] = !b;
               a3_HeroTitleServer.getInstance().SendMsg(a3_HeroTitleServer.SET_SHOW_TITLE, v);
               
           });

        }

        private void ChangeIsShowTilte(GameEvent e)
        {
            bool b = a3_HeroTitleServer.getInstance().isShowTitle;

            if (b)
            {
                if (titleIconGo.GetComponent<Image>().sprite != null)
                    titleIconGo.SetActive(true);

                if (!PlayerNameUIMgr.getInstance().selfTitleGo.transform.parent.parent.FindChild("mapcount").gameObject.activeSelf)
                {
                    if (PlayerNameUIMgr.getInstance().selfTitleGo.transform.GetComponent<Image>().sprite != null)
                        PlayerNameUIMgr.getInstance().selfTitleGo.transform.parent.gameObject.SetActive(true);
                }
            }
            else
            {
                titleIconGo.SetActive(false);
                PlayerNameUIMgr.getInstance().selfTitleGo.transform.parent.gameObject.SetActive(false);
            }

        }

        private void SetLockTitle(GameEvent e)
        {
            GameObject content = transform.FindChild("HeroTitleInfo/TitleClassify/InfoList/Scroll View/Viewport/Content").gameObject;

            int id = e.data["lock_title"]._int;

            if (int.Parse(isAdornGo.transform.parent.name) == id)
            {
                content.transform.FindChild(id + "").FindChild("LockBg").gameObject.SetActive(true);
                content.transform.FindChild(id + "").FindChild("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 100f/255f);

            } //头衔过期

            if (a3_HeroTitleServer.getInstance().eqpTitleId != 0)
            {
                isAdornGo.transform.SetParent(content.transform.FindChild(a3_HeroTitleServer.getInstance().eqpTitleId + "").transform);
                isAdornGo.SetActive(true);
                isAdornGo.transform.localPosition = new Vector3(0, 0, 0);

            }
            else {

                isAdornGo.SetActive(false);

                titleIconGo.GetComponent<Image>().sprite = null;

                PlayerNameUIMgr.getInstance().selfTitleGo.transform.GetComponent<Image>().sprite = null;


            }

            titleIconGo.SetActive( titleIconGo.GetComponent<Image>().sprite!= null && a3_HeroTitleServer.getInstance().isShowTitle);

            PlayerNameUIMgr.getInstance().selfTitleGo.transform.parent.gameObject.SetActive(

                PlayerNameUIMgr.getInstance().selfTitleGo.transform.GetComponent<Image>().sprite!=null && a3_HeroTitleServer.getInstance().isShowTitle

            );

            if (int.Parse(selectBgGo.transform.parent.name) == id)
            {
                selectButton.GetComponent<Button>().interactable = false;

                selectButton.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_herotitle_no");

                selectButton.SetActive(true);

                equipped.SetActive(false);

            }

        }


        private void TitleData_Init()
        {
            a3_HeroTitleServer.getInstance().roleTitleXmlData.Clear();
            List<SXML> s_xml = XMLMgr.instance.GetSXMLList("title.titlelist", null);

            for (int i = 0; i < s_xml.Count; i++)
            {
                RoleTitleXmlData xmlData = new RoleTitleXmlData();
                xmlData.title_id = int.Parse(s_xml[i].getString("title_id"));
                xmlData.category = int.Parse(s_xml[i].getString("category"));
                xmlData.title_name = s_xml[i].getString("title_name");
                xmlData.desc = s_xml[i].getString("desc");
                xmlData.title_img = s_xml[i].getString("title_img");

                Dictionary<int, int> act = new Dictionary<int, int>();
                List<SXML> act_xml = s_xml[i].GetNodeList("nature_act", null);
                for (int j = 0; j < act_xml.Count; j++)
                {
                    act[act_xml[j].getInt("att_type")] = act_xml[j].getInt("att_value");
                }
                xmlData.nature_act = act;

                Dictionary<int, int> eqp = new Dictionary<int, int>();
                List<SXML> eqp_xml = s_xml[i].GetNodeList("nature_eqp", null);
                for (int j = 0; j < eqp_xml.Count; j++)
                {
                    eqp[eqp_xml[j].getInt("att_type")] = eqp_xml[j].getInt("att_value");
                }
                xmlData.nature_eqp = eqp;


                a3_HeroTitleServer.getInstance().roleTitleXmlData.Add(xmlData);
                a3_HeroTitleServer.getInstance().roleTitleData_Dic[xmlData.title_id] = xmlData;
                //roleTitleXmlData
            }
        }
    }

    public class RoleTitleXmlData
    {
        public int title_id;
        public int category;
        public string title_name;
        public string desc;
        public string title_img;

        public Dictionary<int, int> nature_act = new Dictionary<int, int>();
        public Dictionary<int, int> nature_eqp = new Dictionary<int, int>();

    }

    public class TitleData
    {
        public uint title_id;  //称号id
        public uint title_limit;//称号到期时间
        public bool isforever;//称号是否永久

        public TitleData(uint title_id , uint title_limit , bool isforever) {

            this.title_id = title_id;
            this.title_limit = title_limit;
            this.isforever = isforever;

        }
    }


}
