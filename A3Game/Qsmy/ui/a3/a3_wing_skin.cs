using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

namespace MuGame
{
    class a3_wing_skin : Window
    {
        public static a3_wing_skin instance;
        //名字
        private Text textName;
        private Text textLevel;
        private Text textStage;

        //icon table
        private Transform conIcon;
        private BaseButton btnTurnLeft;
        private BaseButton btnTurnRight;
        private Transform TouchPanel;

        private BaseButton btnHelp;
        private GameObject tempPgaeAtt;
        private Transform conAtt;

        //level container
        private Transform conLevelTable;
        //private Text textLevelCostMoneySum;
        private Text textLevelCostItemSum;
        //private Text textLevelLeftItemSum;
        private Text textSliderState;
        private BaseButton btnLevelUpgrade;
        private BaseButton btnLevelOneKey;
        private Transform conStar;
        private Slider sliderExpBar;

        //complete contaner
        private Transform conCompleteTable;
        private GameObject iconTemp;

        //stage contianer
        private Transform conStageTable;
        private Text textStageCostItemSum;
        private Text textStageRate;
        private Slider sliderStage;
        private BaseButton btnStageUp;

        //help panel
        private Transform conHelpPanel;
        private BaseButton btnCloseHelp;

        private A3_WingModel wingModel;
        private SXML wingXML;

        public GameObject wingAvatar;//目标翅膀模型
        private GameObject avatarCamera;//目标ui相机

        private float wingIconSizeX = 0;
        private float wingIconSizeY = 0;
        private float boundaryLeft = 0;
        private float boundaryRight = 0;

        private Transform aniStarTrans;
        private Transform conStarPoint;
        public int ShowStage_yuxuan = 0;
        Animator success;
        Animator fail;
        private processStruct process;

        private int needobj_id;
        private uint neednum = 0;

        private int needobjid_stage;
        private int neednum_stage;

        ScrollControler scrollControer0;

        public override void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("att"));

            wingModel = A3_WingModel.getInstance();
            wingXML = wingModel.WingXML;

            textName = this.getComponentByPath<Text>("Text_name");
            textLevel = this.getComponentByPath<Text>("Text_name/lvl");
            textStage = this.getComponentByPath<Text>("Text_name/stage");

            btnHelp = new BaseButton(this.getTransformByPath("title/help"));
            btnHelp.onClick = OnOpenHelp;

            tempPgaeAtt = this.getGameObjectByPath("att_temp");
            conAtt = this.getTransformByPath("att/grid");

            conLevelTable = this.getTransformByPath("con_level");
            textSliderState = this.getComponentByPath<Text>("con_level/expbar/text");
            btnLevelUpgrade = new BaseButton(this.getTransformByPath("con_level/upgrade"));
            btnLevelUpgrade.onClick = OnUpgradeClick;
            btnLevelOneKey = new BaseButton(this.getTransformByPath("con_level/onekey"));
            btnLevelOneKey.onClick = OnUpgradeOneKey;
            conStar = this.getTransformByPath("con_level/con_star");
            sliderExpBar = this.getComponentByPath<Slider>("con_level/expbar/slider");

            conCompleteTable = this.getTransformByPath("con_complete");


            conIcon = this.getTransformByPath("panel_icon/mask/scroll_rect/con_icon");
            iconTemp = this.getGameObjectByPath("panel_icon/icon_temp");
            btnTurnLeft = new BaseButton(this.getTransformByPath("panel_icon/btn_left"));
            btnTurnLeft.onClick = OnTurnLeftClick;
            btnTurnRight = new BaseButton(this.getTransformByPath("panel_icon/btn_right"));
            btnTurnRight.onClick = OnTurnRightClick;

            BaseButton close_btn = new BaseButton(this.getTransformByPath("btn_close"));
            close_btn.onClick = onClose;

            textLevelCostItemSum = this.getComponentByPath<Text>("con_level/upgrade/text");

            conStageTable = this.getTransformByPath("con_stage");
            textStageRate = this.getComponentByPath<Text>("con_stage/rate");
            textStageCostItemSum = this.getComponentByPath<Text>("con_stage/improve/text");
            btnStageUp = new BaseButton(this.getTransformByPath("con_stage/improve"));
            btnStageUp.onClick = OnStageUpClick;
            sliderStage = this.getComponentByPath<Slider>("con_stage/slider");
            sliderStage.onValueChanged.AddListener(OnSliderValueChange);
            sliderStage.value = 1;

            conHelpPanel = this.getTransformByPath("panel_help");
            btnCloseHelp = new BaseButton(this.getTransformByPath("panel_help/closeBtn"));
            btnCloseHelp.onClick = OnCloseHelp;

            aniExp = conLevelTable.GetComponent<Animator>();
            aniLevelUp = this.getGameObjectByPath("ani_lvlUP");
            process = new processStruct(Update_wing, "a3_wing_skin");

            this.getEventTrigerByPath("panel_icon").onDrag = onDragIcon;

            aniStarTrans = this.getTransformByPath("con_level/con_star/ani_star");
            conStarPoint = this.getTransformByPath("con_level/point");

            this.getEventTrigerByPath("con_avatar/avatar_touch").onDrag = OnDrag;

            BaseButton btnWing = new BaseButton(this.getTransformByPath("btnWing"));
            btnWing.onClick = OnEquWing;

            success = this.transform.FindChild("ani_success").GetComponent<Animator>();
            fail = this.transform.FindChild("ani_fail").GetComponent<Animator>();


            needobjid_stage = XMLMgr.instance.GetSXML("wings.stage_item").getInt("item_id");
            needobj_id = XMLMgr.instance.GetSXML("wings.level_item").getInt("item_id");

            #region 初始化汉字
            getComponentByPath<Text>("title/Text").text = ContMgr.getCont("a3_wing_skin_0");
            getComponentByPath<Text>("att_temp/text_name").text = ContMgr.getCont("a3_wing_skin_1");
            getComponentByPath<Text>("con_level/upgrade/Text").text = ContMgr.getCont("a3_wing_skin_2");
            getComponentByPath<Text>("con_level/onekey/Text").text = ContMgr.getCont("a3_wing_skin_3");
            getComponentByPath<Text>("con_level/shengyu").text = ContMgr.getCont("a3_wing_skin_4");
            getComponentByPath<Text>("con_stage/Text").text = ContMgr.getCont("a3_wing_skin_5");
            getComponentByPath<Text>("con_stage/shengyu").text = ContMgr.getCont("a3_wing_skin_4");
            getComponentByPath<Text>("con_complete/title/text").text = ContMgr.getCont("a3_wing_skin_6");
            getComponentByPath<Text>("btn_equip/Text").text = ContMgr.getCont("a3_wing_skin_7");
            getComponentByPath<Text>("panel_icon/icon_temp/lvl").text = ContMgr.getCont("a3_wing_skin_8");
            getComponentByPath<Text>("btnWing/Text").text = ContMgr.getCont("a3_wing_skin_9");
            getComponentByPath<Text>("panel_help/descTxt").text = ContMgr.getCont("a3_wing_skin_10");
            getComponentByPath<Text>("panel_help/closeBtn/Text").text = ContMgr.getCont("a3_wing_skin_11");
            #endregion


        }

        //显示索引
        private const int LEVEL_INDEX = 0;
        private const int STAGE_INDEX = 1;
        private const int COMPLETE_INDEX = 2;
        //显示页面
        public int pageIndex;

        public void RefreshPanel(int stage, int level)
        {
            if (level >= wingModel.GetStageMaxLevel(stage))
            {
                pageIndex = STAGE_INDEX;
                if (stage >= wingModel.GetXmlMaxStage())
                {
                    pageIndex = COMPLETE_INDEX;
                }
                return;
            }
            pageIndex = LEVEL_INDEX;

           
        }


        //翅膀数据
        private WingsData curWing = null;
        public override void onShowed()
        {
            instance = this;

            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
          
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_STAGE_CHANGE, OnStageChange);
          
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_SHOW_CHANGE, OnShowStageChange);
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_STAGE_DIFT, OnStageNO);
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_LEVEL_EXP_CHANGE, OnLevelExpChange);
            A3_WingProxy.getInstance().addEventListener(A3_WingProxy.ON_LEVEL_AUTO_UPGRADE, OnLevelExpChange);
            int curStage = wingModel.Stage;
            int curLevel = wingModel.Level;
            int showStage = wingModel.ShowStage;
            ShowStage_yuxuan = 0;
            curWing = wingModel.dicWingsData[curStage];
            WingsData sData = wingModel.dicWingsData[curStage];

            InitExpSlider(wingModel.Stage, wingModel.Level,wingModel.Exp);

            CreatAllWingsIcon(curStage);
           
            ShowTitle(sData);
            RefreshAtt(curStage, curLevel);
            RefreshStar(curLevel);
            RefreshCostInfo(wingModel.Stage, wingModel.Level);
            OnSetIconBGImage(showStage);

            OnStageSliderSetting(curWing);
            RefreshPanel(curStage, curLevel);
            ShowPage(pageIndex);

            InitExpState();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            base.onShowed();

            GRMap.GAME_CAMERA.SetActive(false);

            CreatWingAvatar();

            UiEventCenter.getInstance().onWinOpen(uiName);
            setNeedNum_lvl();
            setNeedNum_stage();
            //CancelInvoke("showCam");
            //Invoke("showCam", 0.2f);
        }
        //void showCam()
        //{
        //    if (instance != null)
        //    {
        //        CreatWingAvatar();
        //    }
        //}
        public override void onClosed()
        {
            instance = null;
            (CrossApp.singleton.getPlugin("processManager") as processManager).removeProcess(process);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_LEVEL_EXP_CHANGE, OnLevelExpChange);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_STAGE_CHANGE, OnStageChange);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_LEVEL_AUTO_UPGRADE, OnLevelExpChange);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_SHOW_CHANGE, OnShowStageChange);
            A3_WingProxy.getInstance().removeEventListener(A3_WingProxy.ON_STAGE_DIFT, OnStageNO);

            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            DisposeAvatar();

            DisposeIcon();
            fail.gameObject.SetActive(false);
            success.gameObject.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);

            transform.FindChild("con_level/expbar/expUp").gameObject.SetActive(false);
            GRMap.GAME_CAMERA.SetActive(true);
        }

        #region 动画Updata


        void Update_wing(float deltaTime)
        {
            if (ExpBarHandle != null)
                ExpBarHandle();
        }

        #endregion

        #region 翅膀升级相关

        //翅膀等级变化, 不同等级的翅膀有不同的星级显示
        private int MaxLevel = 10;
        private void RefreshStar(int goalLevle)
        {
            //TODO 改变星级, 改变属性, 进入进阶面板 , 播放升级特效
            if (goalLevle > MaxLevel)
                goalLevle = MaxLevel;

            int i, j;
            for (i = 0; i < goalLevle; i++)
            {
                conStar.GetChild(i).GetChild(0).gameObject.SetActive(true);
                conStar.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            for (j = i; j < MaxLevel; j++)
            {
                conStar.GetChild(j).GetChild(0).gameObject.SetActive(false);
                conStar.GetChild(j).GetChild(1).gameObject.SetActive(true);
            }
        }

        //经验变化的动画 
        private Animator aniExp;
        private GameObject aniLevelUp;
        //翅膀升级经验变化
        private float getExp = 0;
        private void OnLevelExpChange(GameEvent e)
        {

            int lastLvl = wingModel.lastLevel;
            int newLvl = wingModel.Level;
            if (newLvl > lastLvl)
            {
                getExp += sliderExpBar.maxValue - sliderExpBar.value;
                getExp += wingModel.Exp;
            }
            else
            {
                getExp += wingModel.Exp - wingModel.lastExp;
            }
            _speed =(int) (getExp/20);
            if (_speed < 20) { _speed = 20; }
            if (getExp > 0)
            {
                ChangeState(ExpBarState.addExp);
            }
            setNeedNum_lvl();

        }

        ////自动升级
        //private void OnAutoUpgrade(GameEvent e)
        //{
        //    //TODO 自动升级
        //}


        //*添加动画状态
        private void EnterAddExp()
        {
            aniExp.SetBool("Add", true);

            //!-添加操作
            ExpBarHandle += OnExpValueChane;
        }

        private void ExitAddExp()
        {
            aniExp.SetBool("Add", false);

            //!-退出操作
            ExpBarHandle = null;
        }

        //添加经验
        private float expSpeed = 1;
        private void OnExpValueChane()
        {
            expSpeed = _speed;
            if (getExp < expSpeed)
                expSpeed = getExp;
            if ((sliderExpBar.maxValue - sliderExpBar.value) < expSpeed)
            {
                expSpeed = (sliderExpBar.maxValue - sliderExpBar.value);
            }

            getExp -= expSpeed;
            sliderExpBar.value += expSpeed;

            float exceed = 0;
            if (sliderExpBar.value > sliderExpBar.maxValue)
                exceed = sliderExpBar.value - sliderExpBar.maxValue;

            getExp += exceed;

            textSliderState.text = sliderExpBar.value + "/" + sliderExpBar.maxValue;
            if (sliderExpBar.normalizedValue >= 1)
            {
                ChangeState(ExpBarState.expBarup);
            }
            else if (getExp <= 0)
            {
                ChangeState(ExpBarState.init);
            }
        }


        int _speed = 0;
        //进入初始化
        private void EnterInit()
        {
            if (getExp > 0)
            {
                ChangeState(ExpBarState.addExp);
            }
            else
            {
                btnLevelUpgrade.interactable = true;
                btnLevelOneKey.interactable = true;
                ExpBarHandle = null;

                aniLevelUp.SetActive(false);
            }
        }

        private void ExitInit()
        {
            btnLevelUpgrade.interactable = false;
            btnLevelOneKey.interactable = false;
        }

        //*添加动画状态
      

        //*经验值满
        private void EnterExpBarUp()
        {
            aniExp.SetBool("Up", true);

            ExpBarHandle += CheckExpBarUp;
        }

        private void ExitExpBarUp()
        {
            aniExp.SetBool("Up", false);

            ExpBarHandle = null;
            if (wingModel.Level!= 0)
            aniStarTrans.localPosition = conStar.GetChild(wingModel.Level - 1).localPosition;
        }

        private void CheckExpBarUp()
        {
            AnimatorStateInfo state = aniExp.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime >= 1 && state.IsName("ExpBarUp"))
                ChangeState(ExpBarState.point);
        }

        //*飞光点
        private void EnterPoint()
        {
            InitExpSlider(wingModel.Stage, wingModel.Level, 0);
            aniLevelUp.SetActive(true);
            ShowPoint();

            ExpBarHandle = null;
        }

        private void ExitPoint()
        {
            ExpBarHandle = null;
            aniLevelUp.SetActive(false);
        }

        private Vector3 oriPos;
        private Tween curT;
        private void ShowPoint()
        {
            conStarPoint.gameObject.SetActive(true);
            oriPos = conStarPoint.position;

            curT = conStarPoint.DOMove(aniStarTrans.position, 0.9F)
                .SetEase(Ease.Linear)
                .OnComplete(this.OnComplete);
        }

        private void OnComplete()
        {
            debug.Log(" on complete");
            conStarPoint.position = oriPos;
            conStarPoint.gameObject.SetActive(false);
            curT.Kill();

            ChangeState(ExpBarState.star);
        }

        //*添加星星
        private void EnterStar()
        {
            aniExp.SetBool("Star", true);

            ExpBarHandle += CheckStar;
        }

        private void ExitStar()
        {
            aniExp.SetBool("Star", false);
            int curStage = wingModel.Stage;
            WingsData sData = wingModel.dicWingsData[curStage];
            ShowTitle(sData);
            RefreshStar(wingModel.Level);
            RefreshAtt(wingModel.Stage, wingModel.Level);
            RefreshCostInfo(wingModel.Stage, wingModel.Level);
            RefreshPanel(wingModel.Stage , wingModel.Level);
            ShowPage(pageIndex);
            ExpBarHandle = null;
        }

        private void CheckStar()
        {
            AnimatorStateInfo state = aniExp.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime >= 1 && state.IsName("Star"))
                ChangeState(ExpBarState.init);
        }

        //初始化经验条
        private void InitExpSlider(int stage, int level,int exp)
        {
            sliderExpBar.maxValue = wingModel.GetLevelUpMaxExp(stage, level);
            sliderExpBar.value = exp;
            textSliderState.text = sliderExpBar.value + "/" + sliderExpBar.maxValue;
        }

        //升级
        private void OnUpgradeClick(GameObject go)
        {
            if (a3_BagModel.getInstance().getItemNumByTpid((uint)needobj_id) < neednum)
            {
                addtoget(a3_BagModel.getInstance().getItemDataById((uint)needobj_id));
            }

            A3_WingProxy.getInstance().SendUpgradeLevel();
        }

        //自动升级
        private void OnUpgradeOneKey(GameObject go)
        {
            if (a3_BagModel.getInstance().getItemNumByTpid((uint)needobj_id) < neednum)
            {
                addtoget(a3_BagModel.getInstance().getItemDataById((uint)needobj_id));
            }
            A3_WingProxy.getInstance().SendAutoUpgradeLevel();
        }

        void setNeedNum_lvl()
        {
            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)needobj_id);
            this.transform.FindChild("con_level/Num").GetComponent<Text>().text = num.ToString();
        }

        void setNeedNum_stage()
        {
            int num = a3_BagModel.getInstance().getItemNumByTpid((uint)needobjid_stage);
            this.transform.FindChild("con_stage/Num").GetComponent<Text>().text = num.ToString();
        }
        void addtoget(a3_ItemData item)
        {
            ArrayList data1 = new ArrayList();
            data1.Add(item);
            data1.Add(InterfaceMgr.A3_WIBG_SKIN);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
        }

        public enum ExpBarState
        {
            init = 0,
            addExp = 1,
            expBarup = 2,
            point = 3,
            star = 4,
        }

        private void InitExpState()
        {
            ChangeState(ExpBarState.init);
        }
        private ExpBarState currentState = ExpBarState.init;
        private Action ExpBarHandle = null;
        //变更状态
        private void ChangeState(ExpBarState newState)
        {
            switch (currentState)
            {
                case ExpBarState.init:
                    ExitInit();
                    break;
                case ExpBarState.addExp:
                    ExitAddExp();
                    break;
                case ExpBarState.expBarup:
                    ExitExpBarUp();
                    break;
                case ExpBarState.point:
                    ExitPoint();
                    break;
                case ExpBarState.star:
                    ExitStar();
                    break;
                default:
                    break;
            }
      
            currentState = newState;

            switch (newState)
            {
                case ExpBarState.init:
                    EnterInit();
                    break;
                case ExpBarState.addExp:
                    EnterAddExp();
                    //ExpBarHandle += OnAddExp;
                    break;
                case ExpBarState.expBarup:
                    EnterExpBarUp();
                    //ExpBarHandle += CheckExpBarUp;
                    break;
                case ExpBarState.point:
                    EnterPoint();
                    break;
                case ExpBarState.star:
                    EnterStar();
                    //ExpBarHandle += CheckStar;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region stage相关
        //进阶成功
        private void OnStageChange(GameEvent e)
        {
            //success.enabled = false;
            int curStage = wingModel.Stage;
            WingsData sData = wingModel.dicWingsData[curStage];
            ShowTitle(sData);
            RefreshCostInfo(wingModel.Stage,wingModel.Level);
            RefreshPanel(wingModel.ShowStage, wingModel.Level);
            ShowPage(pageIndex);
            RefreshAtt(wingModel.Stage, wingModel.Level);
            RefreshStar(wingModel.Level);
            OnUnLockNweStage();
            InitExpSlider(wingModel.Stage, wingModel.Level, 0);
            costItem =(int) sData.stageCrystalMin;
            textStageCostItemSum.text = costItem.ToString ();
            neednum_stage = costItem;
            success.gameObject.SetActive(true);
            sliderStage.value = 1;
            //success.enabled = true;
            success.Play("wing_jjsuccess", -1,0);
            curWing = wingModel.dicWingsData[curStage];
            OnStageSliderSetting(curWing);
            DisposeAvatar();
            CreatWingAvatar();
            setNeedNum_stage();
            //WingsData sData = wModel.dicWingsData[showStage];
            //ShowTitle(sData);
        }
        //进阶失败
        private void OnStageNO(GameEvent e)
        {
            setNeedNum_stage();
            success.gameObject.SetActive(false);
            //fail.gameObject.SetActive(false);
            fail.gameObject.SetActive(true);
            fail.Play("wing_jjfail", -1,0);
        }

        //升级 材料/进阶金币花费变化
        private void RefreshCostInfo(int stage, int level)
        {
            if (pageIndex == LEVEL_INDEX)
            {            
                textLevelCostItemSum.text = Globle.getBigText(wingModel.GetLevelUpCostItemSum(stage, level));

               
                neednum = wingModel.GetLevelUpCostItemSum(stage, level);

            }
            else if (pageIndex == STAGE_INDEX)
            {
                //        
            }
            else
            {
                //
            }
        }

        //进阶 升级界面变化
        private void ShowPage(int pageIndex)
        {
            switch (pageIndex)
            {
                case LEVEL_INDEX:

                    conLevelTable.gameObject.SetActive(true);
                    conStageTable.gameObject.SetActive(false);
                    conCompleteTable.gameObject.SetActive(false);
                    break;
                case STAGE_INDEX:

                    conLevelTable.gameObject.SetActive(false);
                    conStageTable.gameObject.SetActive(true);
                    conCompleteTable.gameObject.SetActive(false);
                    break;
                case COMPLETE_INDEX:

                    conLevelTable.gameObject.SetActive(false);
                    conStageTable.gameObject.SetActive(false);
                    conCompleteTable.gameObject.SetActive(true);
                    break;
            }
        }

        //属性窗口数值
        private void RefreshAtt(int curStage, int curLevel)
        {
            SXML stageXml = wingXML.GetNode("wing_stage", "stage_id==" + curStage);
            SXML levelXml = stageXml.GetNode("wing_level", "level_id==" + curLevel);
            List<SXML> atts = levelXml.GetNodeList("att", null);

            Dictionary<int, string> dicAtt = new Dictionary<int, string>();

            int att_type;
            string  att_value;
            string[] attackStr = new string[2];

            for (int j = 0; j < atts.Count; j++)
            {
                att_type = atts[j].getInt("att_type");
                att_value = atts[j].getString("att_value");

                if (att_type == 5)
                {
                    attackStr[1] = att_value;
                    dicAtt.Add(att_type, "");
                }
                else if (att_type == 38)
                    attackStr[0] = att_value;
                else
                    dicAtt.Add(att_type, att_value);
            }
            if (dicAtt.ContainsKey(5))
            {
                dicAtt[5] = attackStr[0] + "-" + attackStr[1];
            }

            int k = 0;
            int childCount = conAtt.childCount;
            List<int> listKeys = dicAtt.Keys.ToList<int>();
            for (k = 0; k < listKeys.Count; k++)
            {
                GameObject attentity;

                if (k >= childCount)
                {
                    attentity = GameObject.Instantiate(tempPgaeAtt) as GameObject;
                    attentity.transform.SetParent(conAtt, false);
                }
                else
                    attentity = conAtt.GetChild(k).gameObject;

                attentity.gameObject.SetActive(true);

                Text textName = attentity.transform.FindChild("text_name").GetComponent<Text>();
                Text textValue = attentity.transform.FindChild("text_value").GetComponent<Text>();

                if (listKeys[k] == 5)
                    textName.text = ContMgr.getCont("a3_wing_skin_");
                else 
                textName.text = Globle.getAttrNameById(listKeys[k]);
                textValue.text = dicAtt[listKeys[k]];
            }

            for (int j = k; k < conAtt.childCount; j++)
            {
                conAtt.GetChild(j).gameObject.SetActive(false);
            }

        }

        //名字/等级/阶级变化
        private void ShowTitle(WingsData data)
        {
            textName.text = data.wingName;
            textLevel.text = "LV " + wingModel.Level;
            textStage.text = "(" + wingModel.Stage + ContMgr.getCont("a3_auction_jie") +")";
        }

        //滑动条设置
        private void OnStageSliderSetting(WingsData data)
        {

            OnStageUpRateChange(sliderStage.value);
        }

        //获得滑动条的值
        private void OnSliderValueChange(float scale)
        {
            if (pageIndex != STAGE_INDEX)
                return;

            OnStageUpRateChange(scale);
        }

        //进阶消耗
        private int costItem;
        //进阶成功几率显示
        private void OnStageUpRateChange(float scale)
        {
            float cntMin = curWing.stageCrystalMin;
            float cntMax = curWing.stageCrystalMax;
            float ratMin = curWing.stageRateMin;
            float ratMax = curWing.stageRateMax;
            float step = curWing.stageCrystalStep;
            //scale = scale / sliderStage.maxValue;
            debug.Log(scale.ToString());
            //if (scale < 0.1f)
            //{
            //    scale = 0;
            //    sliderStage.value = 0;
            //}
            //else if (scale >= 0.1f && scale < 0.2f)
            //{
            //    scale = 0.1f;
            //    sliderStage.value = 0.1f;
            //}
            //else if (scale >= 0.2f && scale < 0.4f)
            //{
            //    scale = 0.2f;
            //    sliderStage.value = 0.2f;
            //}
            //else if (scale >= 0.4f && scale < 0.6f)
            //{
            //    scale = 0.4f;
            //    sliderStage.value = 0.4f;
            //}
            //else if (scale >= 0.6f && scale < 0.8f)
            //{
            //    scale = 0.6f;
            //    sliderStage.value = 0.6f;
            //}
            //else if (scale >= 0.8f && scale < 1f)
            //{
            //    scale = 0.8f;
            //    sliderStage.value = 0.8f;
            //}
            //else {
            //    scale = 1;
            //    sliderStage.value = 1;
            //}

            int rate = (int)Math.Round((scale * (ratMax - ratMin) + ratMin) / 100);
            if (rate < 10)
            {
                scale = 0;
                rate = 0;
                sliderStage.value = 0;
            }
            else if (rate >= 10 && rate <20)
            {
                scale = 0.1f;
                rate = 10;
                sliderStage.value = 0.1f;
            }
            else if (rate >= 20 && rate < 40)
            {
                scale = 0.2f;
                rate = 20;
                sliderStage.value = 0.2f;
            }
            else if (rate >= 40 && rate < 60)
            {
                scale = 0.4f;
                rate = 40;
                sliderStage.value = 0.4f;
            }
            else if (rate >= 60 && rate < 80)
            {
                scale = 0.6f;
                rate = 60;
                sliderStage.value = 0.6f;
            }
            else if (rate >= 80&& rate < 100)
            {
                scale = 0.8f;
                rate = 80;
                sliderStage.value = 0.8f;
            }
            else
            {
                scale = 1;
                rate = 100;
                sliderStage.value = 1;
            }
            costItem = (int)(scale * cntMin * 10);
            textStageCostItemSum.text = costItem.ToString();
            neednum_stage = costItem;
            textStageRate.text = rate + "%";
        }

        //点击进阶
        private void OnStageUpClick(GameObject go)
        {
            debug.Log("OnStageUpClick");
            /*TODO
             * 道具足够->金币足够->扣除消耗->
             * */
            if (a3_BagModel.getInstance().getItemNumByTpid((uint)needobjid_stage) < neednum_stage)
            {
                addtoget(a3_BagModel.getInstance().getItemDataById((uint)needobjid_stage));
            }

            A3_WingProxy.getInstance().SendUpgradeStage(costItem);
        }

        #endregion

        #region 翅膀avatar相关

        private void OnShowStageChange(GameEvent e)
        {
            //CreatWingAvatar();

           
        }

        //创建翅膀//------------由于缺少模型, 暂时设置为1
        ProfessionAvatar m_proAvatar;
        private GameObject scene_Obj;
        private void CreatWingAvatar()
        {
            DisposeAvatar();

            if (wingAvatar == null)
            {
                //string path = "";
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else
                {
                    return;
                }

                Transform cur_model = wingAvatar.transform.FindChild("model");

                foreach (Transform tran in wingAvatar.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
                }
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

                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
                scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
                foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta")
                        tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                    else
                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
                avatarCamera = GameObject.Instantiate(obj_prefab) as GameObject;


                //Camera cam = avatarCamera.GetComponentInChildren<Camera>();
                //if (cam != null)
                //{
                //    float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                //    cam.orthographicSize = r_size;
                //}
            }
        }

        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        //处理avatar
        private void DisposeAvatar()
        {
            if (wingAvatar != null) GameObject.Destroy(wingAvatar);
            if (avatarCamera != null) GameObject.Destroy(avatarCamera);
            if(scene_Obj != null) GameObject.Destroy(scene_Obj);

            wingAvatar = null;
            avatarCamera = null;
            scene_Obj = null;
        }

        //切换翅膀Avatar
        private void OnWingAvatarChange()
        {
            DisposeAvatar();
           // CreatWingAvatar();
        }

        //解锁新翅膀
        private void OnUnLockNweStage()
        {
            int curIndex = wingModel.Stage;

            conIcon.GetChild(curIndex - 1).transform.FindChild("image_lock").gameObject.SetActive(false);
        }

        ////翅膀装备/卸下 按钮变化
        //private void OnIconChangeByEquip()
        //{
        //    bool hasEquip = wingModel.hasEquipWing;

        //    string curIndex = wingModel.showStage.ToString();

        //    if (hasEquip)
        //    {
        //        btnOnEquip.transform.GetChild(0).GetComponent<Text>().text = "卸下";
        //        conIcon.FindChild(curIndex).transform.FindChild("icon_bg/image_out").gameObject.SetActive(false);
        //        conIcon.FindChild(curIndex).transform.FindChild("icon_bg/image_on").gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        btnOnEquip.transform.GetChild(0).GetComponent<Text>().text = "装备";
        //        conIcon.FindChild(curIndex).transform.FindChild("icon_bg/image_out").gameObject.SetActive(true);
        //        conIcon.FindChild(curIndex).transform.FindChild("icon_bg/image_on").gameObject.SetActive(false);
        //    }
        //}

        #endregion

        #region 翅膀icon相关


        private Dictionary<int, GameObject> dicIcon = new Dictionary<int, GameObject>();
        //创建翅膀Icon
        private void CreatAllWingsIcon(int curStage)
        {
            Dictionary<int, WingsData> dWingsData = wingModel.dicWingsData;

            RectTransform iconRect = iconTemp.GetComponent<RectTransform>();
            wingIconSizeX = iconRect.sizeDelta.x;
            wingIconSizeY = iconRect.sizeDelta.y;

            foreach (WingsData item in dWingsData.Values)
            {
                GameObject iconClon = GameObject.Instantiate(iconTemp) as GameObject;
                Image iconSprite = iconClon.transform.FindChild("icon_bg/icon").GetComponent<Image>();
                iconSprite.sprite = GAMEAPI.ABUI_LoadSprite(item.spriteFile);

                GameObject imageLock = iconClon.transform.FindChild("image_lock").gameObject;
                if (!item.isUnlock(curStage))
                    imageLock.SetActive(true);
                else
                    imageLock.SetActive(false);

                iconClon.transform.SetParent(conIcon, false);
                iconClon.name = item.stage.ToString();

                Text lvl = iconClon.transform.FindChild("lvl").GetComponent<Text>();
                lvl.text = item.stage +ContMgr.getCont("a3_auction_jie");

                iconClon.SetActive(true);

                BaseButton btnSelect = new BaseButton(iconClon.transform);
                btnSelect.onClick = OnSelectWingByIcon;

                dicIcon[item.stage] = iconClon;
            }

            //TODO 更新container大小 设置边际
            RectTransform iconConRect = conIcon.GetComponent<RectTransform>();
            int count = dWingsData.Count;

            Vector2 newSize = new Vector2(wingIconSizeX * count, wingIconSizeY);
            iconConRect.sizeDelta = newSize;

            boundaryLeft = 0;
            boundaryRight = -(wingIconSizeX * (count-1));
        }

        private void DisposeIcon()
        {
            foreach (GameObject go in dicIcon.Values)
            {
                GameObject.Destroy(go);
            }
            dicIcon.Clear();
        }

        //点击icon切换翅膀
        private void OnSelectWingByIcon(GameObject go)
        {
            int stage = wingModel.Stage;
            int curStage = Int32.Parse(go.name);
            changeWing(curStage);
            if (curStage > stage)
            {
                refor_icon();
                //flytxt.instance.fly("未激活该翅膀");
                go.transform.FindChild("icon_bg/yuxuan_on").gameObject.SetActive(true);
                return;
            }
            wingModel.ShowStage = curStage;

            OnSetIconBGImage(curStage);
            //点击装备翅膀
            A3_WingProxy.getInstance().SendShowStage(curStage);
            transform.FindChild("btnWing/Text").GetComponent<Text>().text = ContMgr.getCont("a3_wing_skin_close");
        }

        void changeWing( int curStage)
        {
            if (wingAvatar != null)
            {
                if (wingAvatar.transform.FindChild("model/Plus_B").transform.childCount > 0)
                    GameObject.Destroy(wingAvatar.transform.FindChild("model/Plus_B").transform.GetChild(0).gameObject);
            }

            A3_PROFESSION eprofession = A3_PROFESSION.None;
            if (SelfRole._inst is P2Warrior)
            {
                eprofession = A3_PROFESSION.Warrior;
            }
            else if (SelfRole._inst is P3Mage)
            {
                eprofession = A3_PROFESSION.Mage;
            }
            else if (SelfRole._inst is P5Assassin)
            {
                eprofession = A3_PROFESSION.Assassin;
            }

            Transform cur_model = wingAvatar.transform.FindChild("model");
            m_proAvatar = new ProfessionAvatar();
            m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
            m_proAvatar.set_wing(curStage, curStage);
        }


        void refor_icon()
        {
            foreach (int key in dicIcon.Keys)
            {
                Transform iconBg = dicIcon[key].transform.FindChild("icon_bg");
                iconBg.GetChild(2).gameObject.SetActive(false);
            }
        }
        //切换翅膀icon背景, 表示当前已选着
        public void OnSetIconBGImage(int showStage)
        {
            foreach (int key in dicIcon.Keys)
            {
                Transform iconBg = dicIcon[key].transform.FindChild("icon_bg");
                if (key == showStage)
                {
                    if (A3_WingModel.getInstance().Stage >= showStage)
                    {
                        iconBg.GetChild(0).gameObject.SetActive(false);
                        iconBg.GetChild(1).gameObject.SetActive(true);
                        iconBg.GetChild(2).gameObject.SetActive(false);
                    }
                }
                else
                {
                    iconBg.GetChild(0).gameObject.SetActive(true);
                    iconBg.GetChild(1).gameObject.SetActive(false);
                    iconBg.GetChild(2).gameObject.SetActive(false);
                }
            }
        }

        private void OnEquWing(GameObject go)
        {
            A3_WingModel wModel = A3_WingModel.getInstance();
            //TODO 装备/卸下翅膀
            if (wModel.ShowStage > 0)
            {
                if (wingAvatar != null)
                {
                    if (wingAvatar.transform.FindChild("model/Plus_B").transform.childCount > 0)
                        GameObject.Destroy(wingAvatar.transform.FindChild("model/Plus_B").transform.GetChild(0).gameObject);
                }
                A3_WingProxy.getInstance().SendShowStage(0);
                wModel.LastShowState = wModel.ShowStage;
                go.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_wing_skin_show");
            }
            else
            {
                changeWing(wModel.LastShowState);
                A3_WingProxy.getInstance().SendShowStage(wModel.LastShowState);
                go.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_wing_skin_close");
            }
            
        }

        //icon panel操作
        private void OnTurnLeftClick(GameObject go)
        {
            TurnRight();
        }

        private void OnTurnRightClick(GameObject go)
        {
            TurnLeft();
            
        }
        
        //icon索引
        private int iconIndex = 0;
        private bool canRun = true;
        private float speed = 0.5F;

        //向左移动一个
        private void TurnLeft()
        {
            RectTransform rectTran = conIcon.GetComponent<RectTransform>();

            Vector2 lastRectPos = rectTran.anchoredPosition;
            float newRectPosX = lastRectPos.x - wingIconSizeX * 3;

            if ((int)newRectPosX < boundaryRight || !canRun )
                return;

            Vector3 lastPos = conIcon.localPosition;
            float newPosX = lastPos.x - wingIconSizeX;

            Tween tween = conIcon.DOLocalMoveX(newPosX, speed);
            tween.SetEase(Ease.Linear);
            canRun = false;

            tween.OnComplete(delegate()
            {
                OnTweeComplete();
                iconIndex++;
            });
        }

        //向右移动一个
        private void TurnRight()
        {
            RectTransform rectTran = conIcon.GetComponent<RectTransform>();

            Vector2 lastRectPos = rectTran.anchoredPosition;
            float newRectPosX = lastRectPos.x + wingIconSizeX ;

            if ((int)newRectPosX > boundaryLeft || !canRun )
                return;

            Vector3 lastPos = conIcon.localPosition;
            float newPosX = lastPos.x + wingIconSizeX;

            Tween tween = conIcon.DOLocalMoveX(newPosX, speed);
            tween.SetEase(Ease.Linear);
            canRun = false;

            tween.OnComplete(delegate()
            {
                OnTweeComplete();
                iconIndex--;
            });
        }

        private void OnTweeComplete()
        {
            canRun = true;
        }

        //拖动icon
        private void onDragIcon(GameObject go, Vector2 delta)
        {
            float y = delta.y;

            if (y > 0)
            {
                TurnLeft();
            }
            else if (y < 0)
            {
                TurnRight();
               
            }
        }

        #endregion

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (wingAvatar != null)
            {
                wingAvatar.transform.Rotate(Vector3.up, -delta.x);
            }
        }



        //打开说明界面
        private void OnOpenHelp(GameObject go)
        {
            debug.Log("OnRulerWindowClick");
            //DisposeAvatar();

            conHelpPanel.gameObject.SetActive(true);
        }

        //关闭说明界面
        private void OnCloseHelp(GameObject go)
        {
            conHelpPanel.gameObject.SetActive(false);

            CreatWingAvatar();
        }

        void onClose(GameObject go )
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_WIBG_SKIN);
        }

    }
}
