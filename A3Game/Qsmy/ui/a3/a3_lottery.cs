using Cross;
using DG.Tweening;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    internal class a3_lottery : Window
    {
        public static a3_lottery mInstance;
        private int[] LootterCoolingTime = new int[5] { 0, 10 * 60, 30 * 60, 60 * 60, 120 * 60 };//冷却次数对应的时间
        //private int[] LootterCoolingTime = new int[5] { 0, 10, 10, 10, 10 };
        private enum DrawType
        {
            Non,
            FreeOnce,
            IceOnce,
            IceTenth,
            FreeTenth,
        }
        public bool is_open;
        private LotteryType currentDrawType = 0;//当前的抽取方式
        private BaseButton btn_close;
        private Text txtNotic;
        private Text txtNoticVip;
        private Text txtGold, txtDiamond, txtPersonalDiamond;
        //bottom
        private BaseButton btn_freeOnce;
        private BaseButton btn_freeTenth;
        private BaseButton btn_iceOnce;
        private Text txtIceOnce, txtFreeTenth;
        private Text txtLeftTimes;
        private BaseButton btn_IceTenth;
        private BaseButton btn_openBag;        
        private BaseButton btn_newbie_ice;
        private float btnCD, curBtnTime;
        //body left
        private Transform awardsAchieved;//用于显示抽奖得到的真实物品

        //Hint
        private BaseButton bg_hintFree;

        private Text hintFreeText;
        private BaseButton bg_hintIce;
        private Text hintIceText;

        private BaseButton btn_buyIce;
        private BaseButton btn_cancel;

        private float timer = 0, timerTenth = 0;
        private float totalTime = 1, totalTimeTenth = 1;
        private int timesDraw = int.MinValue;//抽的次数

        ///
        public Vector3[] _arrayV3;

        public List<Transform> mV3List;
        private int _awardCount;
        private Transform _awardContents;
        //public lotteryAwardController _lotterAwardController;
        private Transform _lotteryInfoPanelTf;
        private lotteryInfoPanel _lotteryInfoPanel;
        public TickItem tickItemTime;

        //抽奖结果确认按钮和界面
        private BaseButton btn_OkOne;
        private BaseButton btn_OkTen;
        private Transform TipOne;
        private Transform TipTen;
        private Transform TipOneP;
        private Transform TipTenP;

        private Transform Tipfive;
        private Transform TipfiveP;
        private List<GameObject> DrawLstOne = new List<GameObject>();
        private List<GameObject> DrawLstTen = new List<GameObject>();
        private List<Vector3> DrawLstOneV3 = new List<Vector3>();
        private List<Vector3> DrawLstTenV3 = new List<Vector3>();
        private BaseButton Btn_TenSkip;

        private Vector3 PosOne;
        private Text NameOne;

        private Vector3[] PosTen;
        private Vector3[] PosFive;

        private Vector3 PosQuality;

        private float updateTime = 0, updateTimeTenth = 0;
        private float textTime = 5.6f;

        private int outTime = 0, outTimeTenth = 0;

        private Animator ani_Add;
        private Animator ani_Text;
        private Animator ani_Text_VIP ;
        private Image textBg;
        Transform _imgTip;
        Image imgOnceTimer, imgTenthTimer;
        int click = 0;
        List<Animator> animAwardItemList;
        int animAwardItemListIndex;
        int iconAwardItemListIndex;
        float timeSpan = 0.5F;
        float rollTimeSpan;
        GameObject prefabBoxFX;

        int CurrLotlogTM = 0;
        int CurrLotlogTM_vip = 0;
        public override void init()
        {
            inText();

            mInstance = this;
            rollTimeSpan = XMLMgr.instance.GetSXML("lottery.gift_cd").getFloat("time");
            btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onBtnCloseClick;
            btn_openBag = new BaseButton(transform.FindChild("bottom/btn_openbag"));
            btn_openBag.onClick = onBtnOpenBagClick;
            //movePathData mpd = initMovingPath();
            //_lotterAwardController.init(mpd);
            txtNotic = transform.FindChild("notice/Text").GetComponent<Text>();
            txtNoticVip = transform.FindChild("noticeVIP/Text").GetComponent<Text>();
            //left
            awardsAchieved = transform.FindChild("body/left/bg/hlg_award");
            //bottom
            btn_freeOnce = new BaseButton(transform.FindChild("bottom/btn_freeOnce"));
            btn_iceOnce = new BaseButton(transform.FindChild("bottom/btn_iceOnce"));
            btn_freeTenth = new BaseButton(transform.FindChild("bottom/btn_freeTenth"));
            btn_freeTenth.onClick = onBtnFreeTenthClick;
            txtIceOnce = btn_iceOnce.transform.FindChild("free/txtInfo").GetComponent<Text>();
            btn_IceTenth = new BaseButton(transform.FindChild("bottom/btn_iceTenth"));
            txtFreeTenth = btn_IceTenth.transform.FindChild("txtInfo").GetComponent<Text>();
            btn_freeOnce.onClick = onBtnFreeOnceClick;
            btn_iceOnce.onClick = onBtnIceOnceClick;
            btn_IceTenth.onClick = onBtnIceTenthClick;
            new BaseButton(this.transform.FindChild("bottom/btn_iceTenth_new")).onClick = onBtnIceTenthClick_new;
            txtLeftTimes = btn_freeOnce.transform.FindChild("LeftTime/Times").GetComponent<Text>();
            //bg_hintFree
            _lotteryInfoPanelTf = transform.FindChild("Hint/lotteryInfoPanel");
            bg_hintFree = new BaseButton(transform.FindChild("Hint/forFree"));
            bg_hintFree.onClick = onHintClick;
            hintFreeText = bg_hintFree.transform.FindChild("Text").GetComponent<Text>();

            bg_hintIce = new BaseButton(transform.FindChild("Hint/forIce"));
            bg_hintIce.onClick = onHiIceClick;
            hintIceText = bg_hintIce.transform.FindChild("Text").GetComponent<Text>();
            btn_buyIce = new BaseButton(bg_hintIce.transform.FindChild("btn_ok"));
            btn_buyIce.onClick = onHitIceBuyIce;
            btn_cancel = new BaseButton(bg_hintIce.transform.FindChild("btn_cancel"));
            btn_cancel.onClick = onHintIceCancle;
            BaseButton btn_notice = new BaseButton(transform.FindChild("bottom/btn_notice"));
            btn_notice.onClick = onBtnNoticeClick;
            _lotteryInfoPanel = new lotteryInfoPanel(_lotteryInfoPanelTf);
            //_lotteryInfoPanel.root.gameObject.SetActive(false);

            //_lotteryInfoPanelTf = transform.FindChild("Hint/lotteryInfoPanel");
            _imgTip = transform.FindChild("imgTip");
            _imgTip.gameObject.SetActive(false);
            TipOne = transform.FindChild("TipOne");
            TipOneP = transform.FindChild("TipOne/Node");
            btn_OkOne = new BaseButton(transform.FindChild("TipOne/BtnOne"));
            btn_OkOne.onClick = onBtnOkOneClick;

            btn_newbie_ice = new BaseButton(transform.FindChild("bottom/btn_NewBieOnce"));
            btn_newbie_ice.onClick = onNewBieIceOnce;
            btn_newbie_ice.gameObject.SetActive(false);
            TipTen = transform.FindChild("TipTen");
            TipTenP = transform.FindChild("TipTen/Node");
            btn_OkTen = new BaseButton(transform.FindChild("TipTen/BtnTen"));
            btn_OkTen.onClick = onBtnOkTenClick;

            Tipfive = transform.FindChild("Tipfive");
            TipfiveP = transform.FindChild("Tipfive/Node");
            btn_OkTen = new BaseButton(transform.FindChild("Tipfive/BtnTen"));
            btn_OkTen.onClick = onBtnOkfiveClick;

            Btn_TenSkip = new BaseButton(transform.FindChild("TipTen/BtnSkip"));
            Btn_TenSkip.onClick = onBtnTenSkipClick;

            PosOne = transform.FindChild("TipOne/icon").position;
            NameOne = transform.FindChild("TipOne/name").GetComponent<Text>();
            PosQuality = transform.FindChild("TipTen/ImageTop").position;
            PosTen = new Vector3[10];
            for (int i = 0; i < 10; i++)
            {
                PosTen[i] = transform.FindChild("TipTen/GardIcon/Image" + i).position;
            }
            PosFive = new Vector3[5];
            for (int i = 0; i < 5; i++)
            {
                PosFive[i] = transform.FindChild("Tipfive/GardIcon/Image" + i).position;
            }

            ani_Add = transform.FindChild("TipTen/Add").GetComponent<Animator>();
            ani_Text = transform.FindChild("notice").GetComponent<Animator>();
            ani_Text_VIP = transform.FindChild("noticeVIP").GetComponent<Animator>();
            textBg = transform.FindChild("notice/bg").GetComponent<Image>();

            transform.FindChild( "notice" ).gameObject.SetActive( false );

            //animAwardItemList = new List<Animator>();
            //Transform tfAwardContent = transform.FindChild("body/awardContents");
            //for (int i = 0; i < (tfAwardContent?.childCount ?? 0); i++)
            //    animAwardItemList.Add(tfAwardContent.GetChild(i).GetComponent<Animator>());
            //timeSpan = LotteryModel.getInstance().getAwardItemAnimTimeSpan();
            txtGold = transform.FindChild("money/gold/image/num").GetComponent<Text>();
            txtDiamond = transform.FindChild("money/diamond/image/num").GetComponent<Text>();
            txtPersonalDiamond = transform.FindChild("money/personalDiamond/image/num").GetComponent<Text>();
            //充值button
            new BaseButton(transform.FindChild("money/gold/btnAdd")).onClick = OnGetGold;
            new BaseButton(transform.FindChild("money/diamond/btnAdd")).onClick = OnGetDiamond;
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            Transform tfLotteryCost = transform.FindChild("bottom/btn_iceOnce/free/icon/Text");
            if (tfLotteryCost)
                tfLotteryCost.GetComponent<Text>().text = LotteryModel.getInstance().lotteryCostOnce.ToString();
            tfLotteryCost = transform.FindChild("bottom/btn_iceTenth/icon/Text"); 
            if (tfLotteryCost)
                tfLotteryCost.GetComponent<Text>().text = LotteryModel.getInstance().lotteryCostTenth.ToString();

            transform.FindChild("bottom/btn_iceTenth_new/Text").GetComponent<Text>().text = LotteryModel.getInstance().lotteryCostVip.ToString();
            var tfTest = transform.FindChild("bottom/btn_iceOnce/free/timer");
            imgOnceTimer = transform.FindChild("bottom/btn_iceOnce/free/timer").GetComponent<Image>();
            imgTenthTimer = transform.FindChild("bottom/btn_iceTenth/timer").GetComponent<Image>(); 

            btnCD = XMLMgr.instance.GetSXML("lottery").GetNode("anniu_cd")?.getFloat("val") ?? 3f;
            if (btnCD < 0) btnCD = 3f;

            prefabBoxFX = GAMEAPI.ABFight_LoadPrefab("FX_npcFX_FX_npc_137_open");

            
        }

        void inText()
        {
            this.transform.FindChild("txt_1").GetComponent<Text>().text = ContMgr.getCont("a3_lottery_1");
            this.transform.FindChild("txt_2").GetComponent<Text>().text = ContMgr.getCont("a3_lottery_1_1");
            getComponentByPath<Text>("bottom/btn_freeOnce/Text").text = ContMgr.getCont("worldmapsubwin_1");
            getComponentByPath<Text>("bottom/btn_freeOnce/LeftTime/Text").text = ContMgr.getCont("shengyu");
            getComponentByPath<Text>("bottom/btn_freeOnce/LeftTime/Text1").text = ContMgr.getCont("ci");
            getComponentByPath<Text>("bottom/btn_freeTenth/Text").text = ContMgr.getCont("xunbaoshici");
            getComponentByPath<Text>("bottom/btn_freeTenth/Text1").text = ContMgr.getCont("worldmapsubwin_1");
        }

        private movePathData initMovingPath()
        {
            Transform leftRoad = transform.FindChild("bg/leftRoad");
            Transform rightRoad = transform.FindChild("bg/rightRoad");
            _awardCount = leftRoad.childCount + rightRoad.childCount;
            _arrayV3 = new Vector3[_awardCount];
            mV3List = new List<Transform>();
            for (int i = 0; i < leftRoad.childCount; i++)
            {
                _arrayV3[i] = leftRoad.GetChild(i).position;
                mV3List.Add(leftRoad.GetChild(i));
            }
            for (int i = 0; i < rightRoad.childCount; i++)
            {
                _arrayV3[leftRoad.childCount + i] = rightRoad.GetChild(i).position;
                mV3List.Add(rightRoad.GetChild(i));
            }
            _awardContents = transform.FindChild("body/awardContents");
            //_lotterAwardController = transform.gameObject.AddComponent<lotteryAwardController>();
            Transform tfBag = transform.FindChild("bottom/btn_openbag");
            movePathData mpd = new movePathData();
            mpd.arrayV3 = _arrayV3;
            mpd.parent = _awardContents;
            mpd.endPos = tfBag.position;
            mpd.startPos = leftRoad.GetChild(0).position;

            return mpd;
        }

        public override void onShowed()
        {
            setNewBie_btn();
            //_lotterAwardController.playNormal();
            btn_close.addEvent();
            btn_openBag.addEvent();
            btn_freeOnce.addEvent();
            btn_iceOnce.addEvent();
            btn_IceTenth.addEvent();
            bg_hintFree.addEvent();
            bg_hintIce.addEvent();
            btn_buyIce.addEvent(); 
            btn_cancel.addEvent();

            btn_OkTen.addEvent();
            btn_OkOne.addEvent();
            Btn_TenSkip.addEvent();
            // TipOne.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // TipTen.localScale = new Vector3(0.1f, 0.1f, 0.1f);
           
            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOADLOTTERY, onLoadLotterys);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_FREEDRAW, onLotteryFreeDraw);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_ICEDRAWONCE, onLotteryIceOnce);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_ICEDRAWTENTH, onLotteryIceTenth);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_FREE_TENTH, onLotteryFreeTenth);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_ICED_NEWBIE, onLotteryIceTenth_newbie);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYNEW_ITEM, ShowNew);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.NEWDRAW, onLotteryIceTenth_new);

            BagProxy.getInstance().addEventListener(UI_EVENT.ON_MONEY_CHANGE, OnShowMoney);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);

            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            CreateModel();

            refreshMoney();
            refreshGold();
            refreshGift();
            Tipfive.gameObject.SetActive(false);
            updateTime = 0;
            textTime = 5.6f;

            /*
            reset();

            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOADLOTTERY, onLoadLotterys);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_FREEDRAW, onLotteryFreeDraw);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_ICEDRAWONCE, onLotteryIceOnce);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOTTERYOK_ICEDRAWTENTH, onLotteryIceTenth);
            UIClient.instance.addEventListener(UI_EVENT.ON_GETLOTTERYDRAWINFO, onGetAcquisitionAwardInfo);
            GRMap.GAME_CAMERA.SetActive(false);
             */
            GRMap.GAME_CAMERA.SetActive(false);
            //InvokeRepeating("DoAwardItemMove", 1f, timeSpan);
            txtGold.text = Globle.getBigText(PlayerModel.getInstance().money);
            txtDiamond.text = PlayerModel.getInstance().gold.ToString();
            txtPersonalDiamond.text = PlayerModel.getInstance().gift.ToString();
            InvokeRepeating("Roll", 0f, rollTimeSpan);
            curBtnTime = btnCD;

            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        //private void DoAwardItemMove()
        //{
        //    animAwardItemList[animAwardItemListIndex].SetTrigger("move");
        //    if(animAwardItemList[animAwardItemListIndex].transform.childCount > 0)
        //        Destroy(animAwardItemList[animAwardItemListIndex].transform.GetChild(0)?.gameObject);
        //    IconImageMgr.getInstance().createA3ItemIcon(LotteryModel.getInstance().lotteryAwardItems[iconAwardItemListIndex].itemId, ignoreLimit: true)
        //        .transform.SetParent(animAwardItemList[animAwardItemListIndex].transform, false);

        //    animAwardItemListIndex++;
        //    if (animAwardItemListIndex > animAwardItemList.Count - 1)
        //        animAwardItemListIndex = 0;

        //    iconAwardItemListIndex++;
        //    if (iconAwardItemListIndex > LotteryModel.getInstance().lotteryAwardItems.Count - 1)
        //        iconAwardItemListIndex = 0;
        //}
        public void ShowNew(GameEvent e) => _lotteryInfoPanel.onShow(e.data);
        public override void onClosed()
        {
            CancelInvoke("Roll");
            if (TipOne.gameObject.activeSelf)
                onBtnOkOneClick(btn_OkOne.gameObject);
            if (TipTen.gameObject.activeSelf)
            {
                onBtnTenSkipClick(TipTen.gameObject);
                onBtnOkTenClick(TipTen.gameObject);
            }
            DestroyModel();
            //CancelInvoke("DoAwardItemMove");
            //iconAwardItemListIndex = 0;
            //for (int i = 0; i < animAwardItemList.Count; i++)
            //    if ((animAwardItemList[i]?.transform?.childCount ?? 0) > 0)
            //        for (int j = animAwardItemList[i].transform.childCount - 1; j >= 0; j--)
            //            Destroy(animAwardItemList[i].transform.GetChild(j).gameObject);

            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOADLOTTERY, onLoadLotterys);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYOK_FREEDRAW, onLotteryFreeDraw);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYOK_ICED_NEWBIE, onLotteryIceTenth_newbie);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYOK_ICEDRAWONCE, onLotteryIceOnce);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYOK_ICEDRAWTENTH, onLotteryIceTenth);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYOK_FREE_TENTH, onLotteryFreeTenth);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYNEW_ITEM, ShowNew);
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.NEWDRAW, onLotteryIceTenth_new);
            BagProxy.getInstance().removeEventListener(UI_EVENT.ON_MONEY_CHANGE, OnShowMoney);
            UIClient.instance.removeEventListener(UI_EVENT.ON_GETLOTTERYDRAWINFO, onGetAcquisitionAwardInfo);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            btn_close.removeAllListener();
            btn_openBag.removeAllListener();
            btn_freeOnce.removeAllListener();
            btn_iceOnce.removeAllListener();
            btn_IceTenth.removeAllListener();
            bg_hintFree.removeAllListener();
            bg_hintIce.removeAllListener();
            btn_buyIce.removeAllListener();
            btn_cancel.removeAllListener();
            GRMap.GAME_CAMERA.SetActive(true);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            btn_OkTen.removeAllListener();
            btn_OkOne.removeAllListener();
            Btn_TenSkip.removeAllListener();
            ani_Text.Play("drawtext", -1, 1);
            ani_Text.transform.FindChild( "Text" ).gameObject.SetActive( false );
            ani_Text.enabled = false;
            ani_Text_VIP.Play("drawtext", -1, 1);
            ani_Text_VIP.transform.FindChild("Text").gameObject.SetActive(false);
            ani_Text_VIP.enabled = false;
            if (LotteryModel.getInstance().isNewBie)
            {
                LotteryModel.getInstance().isNewBie = false;
                txtIceOnce.gameObject.SetActive(true);
                btn_freeOnce.transform.FindChild("LeftTime").gameObject.SetActive(true);
                btn_newbie_ice.gameObject.SetActive(false);
            }

            //_lotterAwardController.close();
            _lotteryInfoPanel.onClosed();
            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
            curBtnTime = btnCD;
            StopAllCoroutines();

            btn_IceTenth.interactable = true;
            btn_iceOnce.interactable = true;
        }
        private void Roll()
        {
            if (_lotteryInfoPanel.Content.childCount >= 10)
            {
                int lastIndex = LotteryModel.getInstance().lotteryAwardInfoItems.Count - 1;
                itemLotteryAwardInfoData temp;
                temp = LotteryModel.getInstance().lotteryAwardInfoItems[lastIndex];
                LotteryModel.getInstance().lotteryAwardInfoItems[lastIndex] = LotteryModel.getInstance().lotteryAwardInfoItems[0];
                LotteryModel.getInstance().lotteryAwardInfoItems[0] = temp;
                _lotteryInfoPanel.Content.GetChild(_lotteryInfoPanel.Content.childCount - 1).SetAsFirstSibling();
            }
        }
        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshMoney();
            }
            if (info.ContainsKey("yb"))
            {
                refreshGold();
            }
            if (info.ContainsKey("bndyb"))
            {
                refreshGift();
            }
        }
        public void refreshMoney()
        {
            Text money = transform.FindChild("money/gold/image/num").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("money/diamond/image/num").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("money/personalDiamond/image/num").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
        }
        private void OnShowMoney(GameEvent e)
        {
            txtGold.text = Globle.getBigText(PlayerModel.getInstance().money);  
            txtDiamond.text = PlayerModel.getInstance().gold.ToString();
            txtPersonalDiamond.text = PlayerModel.getInstance().gift.ToString();
        }

        private void onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LOTTERY);
            is_open = false;
        }

        void setNewBie_btn()
        {
            if (LotteryModel.getInstance().isNewBie)
            {
                btn_newbie_ice.gameObject.SetActive(true);
                txtIceOnce.gameObject.SetActive(false);
                btn_freeOnce.transform.FindChild("LeftTime").gameObject.SetActive(false);
            }
        }

        private void onBtnOpenBagClick(GameObject go)
        {
            is_open = true;

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LOTTERY);
            //ArrayList data = new ArrayList();
            //Action actionReturnToLottery = delegate () { InterfaceMgr.getInstance().open(InterfaceMgr.A3_LOTTERY); };
            //data.Add(actionReturnToLottery);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
            if (a3_bag.Instance != null)
                a3_bag.Instance.transform.SetAsLastSibling();


        }
        private void onBtnFreeOnceClick(GameObject go)
        {
            if (curBtnTime < btnCD) return;
            curBtnTime = 0;
            if (timer > 0.3f && timesDraw > 0)
            {
                setHintFreeInfo(ContMgr.getCont("a3_lottery_txt"));
            }
            else if (timesDraw == 0)
            {
                //txt_freeOnce.text = "明天再来";
            }
            else if (timer <= 0.0f && timesDraw > 0)
            {
                //setDrawButtonUnEnable(DrawType.FreeOnce);
                currentDrawType = LotteryType.FreeDraw;
                //a3_liteMinimap.instance.BtnEnterLottery.transform.FindChild("fire")?.gameObject.SetActive(false);                                
                if (curMonAvatar != null)
                    OpenBox(curMonAvatar.GetComponent<Animator>());
                //LotteryProxy.getInstance().sendlottery((int)LotteryType.FreeDraw);
                Invoke("SendLottery", LotteryModel.getInstance().delayTime);
            }
        }

        private void onBtnIceOnceClick(GameObject go)
        {
            if (curBtnTime < btnCD) return;
            curBtnTime = 0;
            if (PlayerModel.getInstance().gold < LotteryModel.getInstance().lotteryCostOnce)
                setHitIceInfo();
            else
            {
                //setDrawButtonUnEnable(DrawType.IceOnce);
                currentDrawType = LotteryType.IceDrawOnce;                
                if (curMonAvatar != null)
                    OpenBox(curMonAvatar.GetComponent<Animator>());
                Invoke("SendLottery", LotteryModel.getInstance().delayTime);

            }
        }

        private void onNewBieIceOnce(GameObject go)
        {            
            LotteryProxy.getInstance().sendlottery((int)LotteryType.NewBieDraw);
        }

        private void onBtnIceTenthClick(GameObject go)
        {
            if (curBtnTime < btnCD) return;
            curBtnTime = 0;
            if (PlayerModel.getInstance().gold < LotteryModel.getInstance().lotteryCostTenth)
                setHitIceInfo();
            else
            {
                btn_IceTenth.interactable = false;
                //setDrawButtonUnEnable(DrawType.IceTenth);
                currentDrawType = LotteryType.IceDrawTenth;
                if (curMonAvatar != null)
                    OpenBox(curMonAvatar.GetComponent<Animator>()); 
                Invoke("SendLottery", LotteryModel.getInstance().delayTime);
            }
            StartCoroutine(wait());
        }

        private void onBtnIceTenthClick_new(GameObject go) {
            if (A3_VipModel.getInstance().Level < LotteryModel.getInstance().needviplvl )
            {
                flytxt.instance.fly(ContMgr.getCont("a3_a3_lottery_vip_lose"));
                return;
            }

            if (curBtnTime < btnCD) return;
            curBtnTime = 0;
            if (PlayerModel.getInstance().gold < LotteryModel.getInstance().lotteryCostVip)
                setHitIceInfo();
            else
            {
                btn_IceTenth.interactable = false;
                //setDrawButtonUnEnable(DrawType.IceTenth);
                currentDrawType = LotteryType.newDraw;
                if (curMonAvatar1 != null)
                    OpenBox(curMonAvatar1.GetComponent<Animator>());
                Invoke("SendLottery", LotteryModel.getInstance().delayTime);
            }
            StartCoroutine(wait());

        }

        private void onBtnFreeTenthClick(GameObject go)
        {
            if (curBtnTime < btnCD) return;
            curBtnTime = 0;
            btn_IceTenth.interactable = false;
            //setDrawButtonUnEnable(DrawType.FreeTenth);
            currentDrawType = LotteryType.FreeTenth;            
            if (curMonAvatar != null)                
                OpenBox(curMonAvatar.GetComponent<Animator>());
            Invoke("SendLottery", LotteryModel.getInstance().delayTime);
            StartCoroutine(wait());
        }
        private void OpenBox(Animator avatarAnim)
        {
            if (avatarAnim != null)
            {
                avatarAnim.SetTrigger("open");
                
                //if (prefabBoxFX != null)
                //{
                //    GameObject boxFX = GameObject.Instantiate(prefabBoxFX);
                //    boxFX.transform.SetParent(curMonAvatar.transform, false);
                //    boxFX.layer = EnumLayer.LM_FX;
                //    for (int i = 0; i < boxFX.transform.childCount; i++)
                //        boxFX.transform.GetChild(i).gameObject.layer = EnumLayer.LM_FX;
                //    boxFX.transform.SetParent(curMonAvatar.transform, false);
                //}
            }
        }
        private void SendLottery()
        {
            LotteryProxy.getInstance().sendlottery((int)currentDrawType);
        }
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.2f);
        }
        private void onHintClick(GameObject go)
        {
            if (bg_hintFree.gameObject.activeSelf) bg_hintFree.gameObject.SetActive(false);
        }

        private void onHiIceClick(GameObject go)
        {
            if (bg_hintIce.gameObject.activeSelf) bg_hintIce.gameObject.SetActive(false);
        }

        private void onHitIceBuyIce(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3);
        }

        private void onHintIceCancle(GameObject go)
        {
            if (bg_hintIce.gameObject.activeSelf)
            {
                DestroyModel();
                bg_hintIce.gameObject.SetActive(false);
                CreateModel();
            }
        }

        private void onBtnNoticeClick(GameObject go)
        {
            _lotteryInfoPanelTf.gameObject.SetActive(true);
        }

        private void onBtnOkOneClick(GameObject go)
        {
            // Transform Node = transform.FindChild("TipOne");
            for (int i = 0; i < TipOneP.childCount; i++)
            {
                if (TipOneP.GetChild(i).name == "itemLotteryAward")
                {
                    Destroy(TipOneP.GetChild(i).transform.gameObject);
                }
            }
            TipOne.gameObject.SetActive(false);
            _imgTip.gameObject.SetActive(false);
            btn_iceOnce.interactable = true;
            btn_IceTenth.interactable = true;
            DestroyModel();
            CreateModel();
        }

        private void onBtnOkTenClick(GameObject go)
        {
            // Transform Node = transform.FindChild("TipTen");
            for (int i = 0; i < TipTenP.childCount; i++)
            {
                if (TipTenP.GetChild(i).name == "itemLotteryAward")
                {
                    Destroy(TipTenP.GetChild(i).transform.gameObject);
                }
            }

            _imgTip.gameObject.SetActive(false);
            TipTen.gameObject.SetActive(false);
            Tipfive.gameObject.SetActive(false);
            btn_iceOnce.interactable = true;
            btn_IceTenth.interactable = true;
            DestroyModel();
            CreateModel();
            Btn_TenSkip.gameObject.SetActive(true);
        }

        private void onBtnOkfiveClick(GameObject go)
        {
            for (int i = 0; i < TipfiveP.childCount; i++)
            {
                if (TipfiveP.GetChild(i).name == "itemLotteryAward")
                {
                    Destroy(TipfiveP.GetChild(i).transform.gameObject);
                }
            }

            _imgTip.gameObject.SetActive(false);
            TipTen.gameObject.SetActive(false);
            Tipfive.gameObject.SetActive(false);
            btn_iceOnce.interactable = true;
            btn_IceTenth.interactable = true;
            DestroyModel();
            CreateModel();
            Btn_TenSkip.gameObject.SetActive(true);
        }

        private void onBtnTenSkipClick(GameObject go)
        {
            // 第二个参数为层, 第三个参数为停留帧 0为起始 1为最后
            ani_Add.Play("drawadd", -1, 1);

            //DrawLstOne[0].transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
            //DrawLstOne[0].transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
            //DrawLstOne[0].transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
            //transform.FindChild("TipTen/Text").GetComponent<Text>().CrossFadeAlpha(1, 0, true);

            for (int i = 0; i < 10; i++)
            {
                DrawLstTen[i].transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
                DrawLstTen[i].transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
                DrawLstTen[i].transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(1, 0, true);
                transform.FindChild("TipTen/GardText").GetChild(i).GetComponent<Text>().CrossFadeAlpha(1, 0, true);
            }

            go.SetActive(false);
        }

        private void onLoadLotterys(GameEvent e)//获奖信息
        {
            bool isText = true;
            Variant dt = e.data;
            if (isText)
            {
                isText = false;
                if (dt.ContainsKey("left_times"))
                {
                    timesDraw = dt["left_times"]._int;
                    int nTimes = LotteryModel.getInstance().lotteryTimeOnce.Count - timesDraw - 1;
                    if (nTimes >= 0 && nTimes < LotteryModel.getInstance().lotteryTimeOnce.Count)
                        totalTime = LotteryModel.getInstance().lotteryTimeOnce[nTimes];
                }
                if (dt.ContainsKey("left_tm"))//剩余时间
                {
                    timer = dt["left_tm"]._float;

                    if (timesDraw == 0)
                        timer = 0;
                    if (timer > 0)
                    {
                        int t = (int)timer;
                        string h = (t / 60 / 60).ToString().Length > 1 ? (t / 60 / 60).ToString() : "0" + (t / 60 / 60);
                        string m = (t / 60 % 60).ToString().Length > 1 ? (t / 60 % 60).ToString() : "0" + (t / 60 % 60);
                        string s = (t % 60).ToString().Length > 1 ? (t % 60).ToString() : "0" + (t % 60);
                        string te = h + ":" + m + ":" + s + ContMgr.getCont("a3_lottery_free");
                        txtIceOnce.text = te;
                    }
                    // i = (int)timer;
                    // tickItemTime = new TickItem(onUpdateLottery);
                    // TickMgr.instance.addTick(tickItemTime);
                    // if (dt["left_tm"]._int > 0)
                    // {
                    //     iceOnceBtnShow(dt);
                    // }
                    // else
                    // {
                    //     //txt_freeOnce.text = "免费一次";
                    //     if (!btn_freeOnce.gameObject.activeSelf)
                    //     {
                    //         btn_freeOnce.gameObject.SetActive(true);
                    //         btn_iceOnce.gameObject.SetActive(false);
                    //     }
                    // }
                }
                if (dt.ContainsKey("ten_free_time"))
                    timerTenth = dt["ten_free_time"];
                _lotteryInfoPanel.onShow(e.data);
                Variant data = new Variant();
            }
            // if(timesDraw <= 0){
            //     btn_freeOnce.gameObject.SetActive(false);
            //     btn_iceOnce.gameObject.SetActive(true);
            // }
            debug.Log("抽奖跑马灯:" + dt.dump() );
            setNoticeInfo(dt);
            setNoticeInfo_vip(dt);
        }

        private void setNoticeInfo(Variant dt)
        {
            if (dt.ContainsKey("lotlog"))
            {
                List<Variant> lotlogs = dt["lotlog"]._arr;

                if ( lotlogs.Count <= 0  )
                {
                    transform.FindChild( "notice" ).gameObject.SetActive(false);

                    return;
                }

                transform.FindChild( "notice" ).gameObject.SetActive( true );

                int tm = lotlogs[ 0 ][ "tm" ]._int;
                itemLotteryAwardInfoData lai = new itemLotteryAwardInfoData();
                lai.name = lotlogs[0]["name"]._str;
                lai.tpid = lotlogs[0]["tpid"]._uint;
                lai.cnt = lotlogs[0]["cnt"]._uint;
                lai.tm = lotlogs[0].ContainsKey("intensify") ? lotlogs[0]["intensify"]._uint : 0;
                lai.stage = lotlogs[0].ContainsKey("stage") ? lotlogs[0]["stage"]._uint : 0;
                //恭喜xxx（玩家名）抽中了xxx（装备名）
                // txtNotic.text = string.Format("恭喜{0}抽中了{1}", lai.name, a3_BagModel.getInstance().getItemDataById(lai.tpid).item_name);
                txtNotic.text = ContMgr.getCont("a3_lottery_cg") + "<color=#FF0000FF>" + lai.name + "</color>" + ContMgr.getCont("a3_lottery_get") + setQualityColor(a3_BagModel.getInstance().getItemDataById(lai.tpid).item_name, a3_BagModel.getInstance().getItemDataById(lai.tpid).quality);

                // 判断动画是否已经播完
                AnimatorStateInfo animatorInfo;
                if ( tm != CurrLotlogTM )
                {
                    ani_Text.transform.FindChild( "Text" ).gameObject.SetActive( true );
                    ani_Text.enabled = true;
                    animatorInfo = ani_Text.GetCurrentAnimatorStateInfo( 0 );
                    if ( animatorInfo.normalizedTime >= 1 && animatorInfo.IsName( "drawtext" ) )
                    {
                        ani_Text.Play( "drawtext" , -1 , 0 );
                    }
                    CurrLotlogTM = tm;
                }
            }
        }

        private void setNoticeInfo_vip(Variant dt)
        {
            if (dt.ContainsKey("vip_log"))
            {
                List<Variant> lotlogs = dt["vip_log"]._arr;

                if (lotlogs.Count <= 0)
                {
                    transform.FindChild("noticeVIP").gameObject.SetActive(false);

                    return;
                }

                transform.FindChild("noticeVIP").gameObject.SetActive(true);

                int tm = lotlogs[0]["tm"]._int;
                itemLotteryAwardInfoData lai = new itemLotteryAwardInfoData();
                lai.name = lotlogs[0]["name"]._str;
                lai.tpid = lotlogs[0]["tpid"]._uint;
                lai.cnt = lotlogs[0]["cnt"]._uint;
                lai.tm = lotlogs[0].ContainsKey("intensify") ? lotlogs[0]["intensify"]._uint : 0;
                lai.stage = lotlogs[0].ContainsKey("stage") ? lotlogs[0]["stage"]._uint : 0;
                //恭喜xxx（玩家名）抽中了xxx（装备名）
                // txtNotic.text = string.Format("恭喜{0}抽中了{1}", lai.name, a3_BagModel.getInstance().getItemDataById(lai.tpid).item_name);
                txtNoticVip.text = ContMgr.getCont("a3_lottery_cg") + "<color=#FF0000FF>" + lai.name + "</color>" + ContMgr.getCont("a3_lottery_get_vip") + setQualityColor(a3_BagModel.getInstance().getItemDataById(lai.tpid).item_name, a3_BagModel.getInstance().getItemDataById(lai.tpid).quality);

                // 判断动画是否已经播完
                AnimatorStateInfo animatorInfo;
                if (tm != CurrLotlogTM_vip)
                {
                    ani_Text_VIP.transform.FindChild("Text").gameObject.SetActive(true);
                    ani_Text_VIP.enabled = true;
                    animatorInfo = ani_Text_VIP.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.normalizedTime >= 1 && animatorInfo.IsName("drawtext"))
                    {
                        ani_Text_VIP.Play("drawtext", -1, 0);
                    }
                    CurrLotlogTM_vip = tm;
                }
            }

        }


        //private void iceOnceBtnShow(Variant dt)
        //{
        //    string leftTimes = dt["left_times"]._int.ToString();
        //    int leftTm = dt["left_tm"]._int;
        //    string freeOnceStr = string.Empty;
        //    freeOnceStr = "00:" + (leftTm / 60).ToString() + ":" + (leftTm % 60).ToString() + "后免费";
        //    leftTimes = "(" + leftTimes + "/5)";
        //    txtIceOnce.text = freeOnceStr;// +leftTimes;
        //    if (!btn_iceOnce.gameObject.activeSelf)
        //    {
        //        btn_iceOnce.gameObject.SetActive(true);
        //        btn_freeOnce.gameObject.SetActive(false);
        //    }
        //}

        private void setFreeOnceDraw(int leftTm = -1, bool freeBtnShow = true)
        {
            txtLeftTimes.gameObject.SetActive(false);
            if (leftTm <= 0)
            {
                txtIceOnce.gameObject.SetActive(false);
            }
            else
            {
                //txtIceOnce.gameObject.SetActive(true);
                string freeOnceStr = string.Empty;
                //string h = (leftTm / 60 / 60).ToString().Length > 1 ? (leftTm / 60 / 60).ToString() : "0" + (leftTm / 60 / 60);
                //string m = (leftTm / 60 % 60).ToString().Length > 1 ? (leftTm / 60 % 60).ToString() : "0" + (leftTm / 60 / 60);
                //string s = (leftTm % 60).ToString().Length > 1 ? (leftTm % 60).ToString() : "0" + (leftTm % 60);
                //freeOnceStr = h + ":" + m + ":" + s + ContMgr.getCont("a3_lottery_free");
                //txtIceOnce.text = freeOnceStr;
                //TODO: 处理成填充条,总时间根据当前剩余次数去获取
                int ntimesDraw = LotteryModel.getInstance().lotteryTimeOnce.Count - timesDraw - 1;
                if (ntimesDraw >= 0)
                    imgOnceTimer.fillAmount = timer / LotteryModel.getInstance().lotteryTimeOnce[ntimesDraw];
            }
            btn_iceOnce.gameObject.SetActive(!freeBtnShow);
            btn_freeOnce.gameObject.SetActive(freeBtnShow);
        }        
        private void onLotteryFreeDraw(GameEvent e)//免费抽
        {
            _imgTip.gameObject.SetActive(true);
            DestroyModel();            
            btn_OkOne.gameObject.SetActive(false);
            btn_iceOnce.interactable = false;
            btn_IceTenth.interactable = false;
            Variant data = e.data;
            List<Variant> awardsLottery = data["ids"]._arr;
            uint id = awardsLottery[0]._uint;

            uint itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
            a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(itemId);
            int num = LotteryModel.getInstance().getAwardNumById((uint)id);
            GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num, true);
            go.name = "itemLotteryAward";
            go.transform.SetParent(TipOneP, false);
            go.transform.position = PosOne;

            NameOne.text = setQualityColor(itemData.item_name.ToString(), itemData.quality);
         
            TipOne.gameObject.SetActive(true);
            TipOne.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Image qicon = go.transform.FindChild("qicon").GetComponent<Image>();
            Image bicon = go.transform.FindChild("bicon").GetComponent<Image>();
            Image sicon = go.transform.FindChild("icon").GetComponent<Image>();
            qicon.CrossFadeAlpha(0, 0, true);
            bicon.CrossFadeAlpha(0, 0, true);
            sicon.CrossFadeAlpha(0, 0, true);
            NameOne.CrossFadeAlpha(0, 0, true);

            Sequence seq = DOTween.Sequence();
            Tweener t1 = TipOne.DOScale(1, 0.5f);
            seq.Append(t1);

            t1.OnComplete((TweenCallback)delegate ()
            {
                qicon.CrossFadeAlpha(1, 0.2f, true);
                bicon.CrossFadeAlpha(1, 0.2f, true);
                sicon.CrossFadeAlpha(1, 0.2f, true);
                NameOne.CrossFadeAlpha(1, 0.2f, true);
                this.btn_OkOne.gameObject.SetActive(true);
            });


            // _lotterAwardController.playOneAward((int)id);
            if (timesDraw > 1)
            {
                int nTimesDraw = LotteryModel.getInstance().lotteryTimeOnce.Count - timesDraw - 1;
                if (nTimesDraw >= 0)
                {
                    totalTime = LotteryModel.getInstance().lotteryTimeOnce[nTimesDraw];
                    timer = LotteryModel.getInstance().lotteryTimeOnce[nTimesDraw];
                }
                //LootterCoolingTime[5 - timesDraw + 1];
                // i = (int)timer;
                // if (tickItemTime == null) tickItemTime = new TickItem(onUpdateLottery);

                // TickMgr.instance.addTick(tickItemTime);
            }
            else
            {
                timer = 0;
                // i = (int)timer;
            }
            if (timesDraw != 0)
                timesDraw--;
            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
        }

        private void onLotteryIceOnce(GameEvent e)//钻石抽一次
        {
            _imgTip.gameObject.SetActive(true);
            DestroyModel();
            btn_OkOne.gameObject.SetActive(false);
            btn_iceOnce.interactable = false;
            btn_IceTenth.interactable = false;
            Variant data = e.data;
            if (data["res"] < 0) return;
            List<Variant> awardsLottery = data["ids"]._arr;
            uint id = awardsLottery[0]._uint;

            uint itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
            a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(itemId);
            int num = LotteryModel.getInstance().getAwardNumById((uint)id);
            GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num, true);
            go.name = "itemLotteryAward";
            go.transform.SetParent(TipOneP, false);
            go.transform.position = PosOne;

            BaseButton btn_equip = new BaseButton(go.transform);
            // btn_equip.onClick = delegate(GameObject gb) { this.onEquipClick(go, id); };

            NameOne.text = setQualityColor(itemData.item_name.ToString(), itemData.quality);
 
            TipOne.gameObject.SetActive(true);
            TipOne.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            //SetColor和CrossFadeAlpha 设置的Alpha值不为同一个 无法一起使用
            // qicon.transform.GetComponent<Image>().color = new Color(1,1,1,0);
            // sicon.transform.GetComponent<Image>().color = new Color(1,1,1,0);

            Image qicon = go.transform.FindChild("qicon").GetComponent<Image>();
            Image bicon = go.transform.FindChild("bicon").GetComponent<Image>();
            Image sicon = go.transform.FindChild("icon").GetComponent<Image>();
            qicon.CrossFadeAlpha(0, 0, true);
            bicon.CrossFadeAlpha(0, 0, true);
            sicon.CrossFadeAlpha(0, 0, true);
            NameOne.CrossFadeAlpha(0, 0, true);

            Sequence seq = DOTween.Sequence();
            Tweener t1 = TipOne.DOScale(1, 0.5f);
            seq.Append(t1);

            t1.OnComplete((TweenCallback)delegate ()
            {
                qicon.CrossFadeAlpha(1, 0.2f, true);
                bicon.CrossFadeAlpha(1, 0.2f, true);
                sicon.CrossFadeAlpha(1, 0.2f, true);
                NameOne.CrossFadeAlpha(1, 0.2f, true);
                this.btn_OkOne.gameObject.SetActive(true);
            });


            // _lotterAwardController.playOneAward((int)id);
        }

        private void onLotteryIceTenth_newbie(GameEvent e)
        {
            _imgTip.gameObject.SetActive(true);
            DestroyModel();
            btn_OkTen.gameObject.SetActive(false);
            btn_iceOnce.interactable = false;
            btn_IceTenth.interactable = false;
            Variant data = e.data;
            uint itemId = data["item_id"];
            a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(itemId);
            GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, 1, true);
            go.name = "itemLotteryAward";
            go.transform.SetParent(TipOneP, false);
            go.transform.position = PosOne;
            NameOne.text = setQualityColor(itemData.item_name.ToString(), itemData.quality);
       
            TipOne.gameObject.SetActive(true);
            TipOne.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Image qicon = go.transform.FindChild("qicon").GetComponent<Image>();
            Image bicon = go.transform.FindChild("bicon").GetComponent<Image>();
            Image sicon = go.transform.FindChild("icon").GetComponent<Image>();
            qicon.CrossFadeAlpha(0, 0, true);
            bicon.CrossFadeAlpha(0, 0, true);
            sicon.CrossFadeAlpha(0, 0, true);
            NameOne.CrossFadeAlpha(0, 0, true);
            Sequence seq = DOTween.Sequence();
            Tweener t1 = TipOne.DOScale(1, 0.5f);
            seq.Append(t1);

            t1.OnComplete((TweenCallback)delegate ()
            {
                qicon.CrossFadeAlpha(1, 0.2f, true);
                bicon.CrossFadeAlpha(1, 0.2f, true);
                sicon.CrossFadeAlpha(1, 0.2f, true);
                NameOne.CrossFadeAlpha(1, 0.2f, true);
                this.btn_OkTen.gameObject.SetActive(true);
            });
        }


        void onLotteryIceTenth_new(GameEvent e) {
            _imgTip.gameObject.SetActive(true);
            DestroyModel();
            debug.Log("抽奖信息:::" + e.data["ids"].dump());
            btn_iceOnce.interactable = false;
            btn_IceTenth.interactable = false;
            Variant data = e.data;
            List<Variant> awardsLottery = data["ids"]._arr;

            if (awardsLottery.Count > 5)
            {
                awardsLottery.RemoveRange(5, awardsLottery.Count - 5);
            }
            List<int> ids = new List<int>();
            ids.Clear();
            DrawLstOne.Clear();
            DrawLstTen.Clear();
            DrawLstOneV3.Clear();
            DrawLstTenV3.Clear();
            uint NeedId = 0;
            uint NeedNum = 0;
            uint Count = 0;
            bool isNeed = true;

            uint id = 0;
            uint itemId = 0;
            a3_ItemData itemData = new a3_ItemData();
            int num = 0;
            GameObject go;

            bool temp = true;

            List<GameObject> lst = new List<GameObject>();
            List<GameObject> lstTop = new List<GameObject>();
            List<Text> lsx = new List<Text>();

            lst.Clear();
            lstTop.Clear();
            lsx.Clear();

            //得到ID和第几位
            for (int i = 0; i < awardsLottery.Count; i++)
            {
                id = awardsLottery[i]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                Count++;

                if (itemData.quality == 5)
                {
                    NeedId = id;
                    NeedNum = Count - 1;
                    break;
                }
                if (itemData.quality == 4 && temp)
                {
                    temp = false;
                    NeedId = id;
                    NeedNum = Count - 1;
                }
            }

            if (temp && NeedId == 0)
            {
                id = awardsLottery[0]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                NeedId = id;
                NeedNum = 0;
            }


            for (int i = 0; i < awardsLottery.Count; i++)
            {
                id = awardsLottery[i]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                num = LotteryModel.getInstance().getAwardNumById((uint)id);
                go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num, true);
                go.name = "itemLotteryAward";
                //第二个参数为true或无时 子物体会和父物体一起发生变化
                go.transform.SetParent(TipfiveP, false);
                go.transform.position = PosFive[i];
                transform.FindChild("Tipfive/GardText").GetChild(i).GetComponent<Text>().text = setQualityColor(itemData.item_name.ToString(), itemData.quality);

                lst.Add(go);
                DrawLstTen.Add(go);
                DrawLstTenV3.Add(PosFive[i]);
                lsx.Add(transform.FindChild("Tipfive/GardText").GetChild(i).GetComponent<Text>());
                go.transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                go.transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                go.transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                transform.FindChild("Tipfive/GardText").GetChild(i).GetComponent<Text>().CrossFadeAlpha(0, 0, true);

            }

            Tipfive.gameObject.SetActive(true);
            Tipfive.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Sequence seq = DOTween.Sequence();
            Tweener t1 = Tipfive.DOScale(1, 0.5f);
            seq.Append(t1);
            t1.OnComplete((TweenCallback)delegate ()
            {
                ani_Add.Play("drawadd");
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[0], lsx[0]); }, 0));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[1], lsx[1]); }, 0.2f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[2], lsx[2]); }, 0.4f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[3], lsx[3]); }, 0.6f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[4], lsx[4]); }, 0.8f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo((Action)(() => { this.btn_OkTen.gameObject.SetActive(true); }), 2.0f));
            });


        }

        private void OnGetGold(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
            DestroyModel();
            a3_exchange.Instance?.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
        }
        public bool toRecharge = false ;
        private void OnGetDiamond(GameObject go)
        {
            InterfaceMgr.getInstance().close(this.uiName);
            toRecharge = true;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            //DestroyModel();
            //a3_Recharge.Instance?.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
        }
        private void onLotteryIceTenth(GameEvent e)//钻石连续抽10次
        {
            _imgTip.gameObject.SetActive(true);
            DestroyModel();
            btn_OkTen.gameObject.SetActive(false);
            debug.Log("抽奖信息:::" + e.data["ids"].dump());
            btn_iceOnce.interactable = false;
            btn_IceTenth.interactable = false;
            Variant data = e.data;
            List<Variant> awardsLottery = data["ids"]._arr;
            if ( awardsLottery.Count > 10 )
            {
                awardsLottery.RemoveRange( 10 , awardsLottery.Count - 10 );
            }
            List<int> ids = new List<int>();
            ids.Clear();
            DrawLstOne.Clear();
            DrawLstTen.Clear();
            DrawLstOneV3.Clear();
            DrawLstTenV3.Clear();

            //单独显示的物品ID和第几位
            uint NeedId = 0;
            uint NeedNum = 0;
            uint Count = 0;
            bool isNeed = true;

            uint id = 0;
            uint itemId = 0;
            a3_ItemData itemData = new a3_ItemData();
            int num = 0;
            GameObject go;

            bool temp = true;

            List<GameObject> lst = new List<GameObject>();
            List<GameObject> lstTop = new List<GameObject>();
            List<Text> lsx = new List<Text>();

            lst.Clear();
            lstTop.Clear();
            lsx.Clear();

            //得到ID和第几位
            for (int i = 0; i < awardsLottery.Count; i++)
            {
                id = awardsLottery[i]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                Count++;

                if (itemData.quality == 5)
                {
                    NeedId = id;
                    NeedNum = Count - 1;
                    break;
                }
                if (itemData.quality == 4 && temp)
                {
                    temp = false;
                    NeedId = id;
                    NeedNum = Count - 1;
                }
            }

            if (temp && NeedId == 0)
            {
                id = awardsLottery[0]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                NeedId = id;
                NeedNum = 0;
            }

            for (int i = 0; i < awardsLottery.Count; i++)
            {
                id = awardsLottery[i]._uint;
                itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                num = LotteryModel.getInstance().getAwardNumById((uint)id);
                go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num, true);
                go.name = "itemLotteryAward";
                //第二个参数为true或无时 子物体会和父物体一起发生变化
                go.transform.SetParent(TipTenP, false);

                //if (id == NeedId && isNeed)
                //{
                //    //isNeed = false;
                //    //go.transform.position = PosQuality;
                //    //transform.FindChild("TipTen/Text").GetComponent<Text>().text = setQualityColor(itemData.item_name.ToString(), itemData.quality);
                //    //lstTop.Add(go);
                //    //DrawLstOne.Add(go);
                //    //DrawLstOneV3.Add(PosQuality);
                //    //go.transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                //    //go.transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                //    //go.transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                //    //transform.FindChild("TipTen/Text").GetComponent<Text>().CrossFadeAlpha(0, 0, true);
                //}
                //else
                //{
                //if (i <= NeedNum)
                //    {
                go.transform.position = PosTen[i];
                transform.FindChild("TipTen/GardText").GetChild(i).GetComponent<Text>().text = setQualityColor(itemData.item_name.ToString(), itemData.quality);

                lst.Add(go);
                DrawLstTen.Add(go);
                DrawLstTenV3.Add(PosTen[i]);
                lsx.Add(transform.FindChild("TipTen/GardText").GetChild(i).GetComponent<Text>());
                go.transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                go.transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                go.transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
                transform.FindChild("TipTen/GardText").GetChild(i).GetComponent<Text>().CrossFadeAlpha(0, 0, true);

            }
            //    }
            //    else
            //    {
            //        go.transform.position = PosTen[i - 1];
            //        transform.FindChild("TipTen/GardText").GetChild(i - 1).GetComponent<Text>().text = setQualityColor(itemData.item_name.ToString(), itemData.quality);

            //        lst.Add(go);
            //        DrawLstTen.Add(go);
            //        DrawLstTenV3.Add(PosTen[i - 1]);
            //        lsx.Add(transform.FindChild("TipTen/GardText").GetChild(i - 1).GetComponent<Text>());
            //        go.transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
            //        go.transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
            //        go.transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(0, 0, true);
            //        transform.FindChild("TipTen/GardText").GetChild(i - 1).GetComponent<Text>().CrossFadeAlpha(0, 0, true);
            //    }
            //}
            //}
  
            TipTen.gameObject.SetActive(true);
            TipTen.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Sequence seq = DOTween.Sequence();
            Tweener t1 = TipTen.DOScale(1, 0.5f);
            seq.Append(t1);            
            t1.OnComplete((TweenCallback)delegate ()
            {
                ani_Add.Play("drawadd");
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[0], lsx[0]); }, 0));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[1], lsx[1]); }, 0.2f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[2], lsx[2]); }, 0.4f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[3], lsx[3]); }, 0.6f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[4], lsx[4]); }, 0.8f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[5], lsx[5]); }, 1f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[6], lsx[6]); }, 1.2f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[7], lsx[7]); }, 1.4f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[8], lsx[8]); }, 1.6f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { setTween(lst[9], lsx[9]); }, 1.8f));
                StartCoroutine(DelayToInvoke.DelayToInvokeDo((Action)(() => { this.btn_OkTen.gameObject.SetActive(true); }), 2.0f));
            });

            // _lotterAwardController.playTenAward(ids);
        }
        private void onLotteryFreeTenth(GameEvent e)
        {
            DestroyModel();
            onLotteryIceTenth(e);
            LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
        }
        GameObject curMonAvatar, curMonAvatar1, camObj, curLotteryScene;
        //public void CreateModel()
        //{            
        //    DestroyModel();
        //    GAMEAPI.ABModel_LoadGameObject("npc_137", Model_LoadedOK, null);
        //}

        private void Model_LoadedOK(UnityEngine.Object model_obj, System.Object data)
        {
            GameObject obj_prefab = model_obj as GameObject;
            if ((int)data == 1)
            {
                curMonAvatar = GameObject.Instantiate(obj_prefab, LotteryModel.getInstance().boxPosition, Quaternion.Euler(LotteryModel.getInstance().boxRotation)) as GameObject;
                curMonAvatar.transform.localScale = LotteryModel.getInstance().boxScale;
                foreach (Transform tran in curMonAvatar.GetComponentsInChildren<Transform>())
                    tran.gameObject.layer = EnumLayer.LM_FX;
            }
            else if ((int)data == 2) {
                curMonAvatar1 = GameObject.Instantiate(obj_prefab, LotteryModel.getInstance().boxPosition1, Quaternion.Euler(LotteryModel.getInstance().boxRotation1)) as GameObject;
                curMonAvatar1.transform.localScale = LotteryModel.getInstance().boxScale1;
                foreach (Transform tran in curMonAvatar1.GetComponentsInChildren<Transform>())
                    tran.gameObject.layer = EnumLayer.LM_FX;
            }

        }
        public void CreateModel() {
            DestroyModel();
            GameObject obj_scene = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_lotteryShow_scene");
            if (obj_scene != null)
            {
                curLotteryScene = GameObject.Instantiate(obj_scene);
                Transform[] arr_tran = curLotteryScene.GetComponentsInChildren<Transform>();
                for (int i = 0; i < arr_tran.Length; i++)
                    arr_tran[i].gameObject.layer = EnumLayer.LM_FX;
            }
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_lottery_ui_camera");
            camObj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = camObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
            GAMEAPI.ABModel_LoadGameObject("npc_137", Model_LoadedOK, 2);
            GAMEAPI.ABModel_LoadGameObject("npc_137", Model_LoadedOK, 1);
        }
        

        public void DestroyModel()
        {            
            if (curMonAvatar != null)
                GameObject.DestroyImmediate(curMonAvatar);
            if (curMonAvatar1 != null)
                GameObject.DestroyImmediate(curMonAvatar1);
            if (camObj != null )
                GameObject.DestroyImmediate(camObj);
            if (curLotteryScene != null )
                GameObject.DestroyImmediate(curLotteryScene);
        }

        public string GetLotteryItemNameColor(string name, int quality) => setQualityColor(name, quality);
        private string setQualityColor(string name, int quality)
        {
            if (quality == 1)
                return "<color=#ffffff>" + name + "</color>";
            if (quality == 2)
                return "<color=#00FF00>" + name + "</color>";
            if (quality == 3)
                return "<color=#66FFFF>" + name + "</color>";
            if (quality == 4)
                return "<color=#FF00FF>" + name + "</color>";
            if (quality == 5)
                return "<color=#f7790a>" + name + "</color>";
            if (quality == 6)
                return "<color=#f90e0e>" + name + "</color>";
            if (quality == 7)
                return "<color=#f90e0e>" + name + "</color>";

            return "<color=#ffffff>" + name + "</color>";
        }

        private void setTween(GameObject go1, Text go2)
        {
            if (go1)
            {
                go1.transform.FindChild("qicon").GetComponent<Image>().CrossFadeAlpha(1, 0.2f, true);
                go1.transform.FindChild("bicon").GetComponent<Image>().CrossFadeAlpha(1, 0.2f, true);
                go1.transform.FindChild("icon").GetComponent<Image>().CrossFadeAlpha(1, 0.2f, true);
                if (go2)
                {
                    go2.CrossFadeAlpha(1, 0.2f, true);
                }
                else
                {
                    transform.FindChild("TipTen/Text").GetComponent<Text>().CrossFadeAlpha(1, 0.2f, true);
                }
            }
        }

        private void onEquipClick(GameObject gb, uint id)
        {
            debug.Log("点击icon打开装备预览界面");
            ArrayList data = new ArrayList();
            // a3_BagItemData one = a3_EquipModel.getInstance().getEquips()[id];
            a3_BagItemData itemData = new a3_BagItemData();
            itemData.id = id;
            itemData.num = 1;
            itemData.ismark = false;
            // itemData.tpid = itm["tpid"];
            // itemData.num = itm["cnt"];
            itemData.confdata = a3_BagModel.getInstance().getItemDataById(id);
            data.Add(itemData);
            data.Add(equip_tip_type.BagPick_tip);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
        }

        private void setDrawButtonUnEnable(DrawType dt)
        {
            //currentDrawType = dt;
            //btn_iceOnce.interactable = false;
            //btn_IceTenth.interactable = false;
        }

        //Transform tfLeftTimeParent;
        private void Update()
        {
            if (btn_freeOnce == null) return;

            curBtnTime += Time.deltaTime;
            updateTime -= Time.deltaTime;
            updateTimeTenth -= Time.deltaTime;
            //tfLeftTimeParent = txtLeftTimes.transform.parent;
            if (timesDraw <= 0)
            {
                btn_freeOnce.gameObject.SetActive(false);
                txtLeftTimes.gameObject.SetActive(true);
                //for (int i = 0; i < tfLeftTimeParent.childCount; i++)
                //    if (tfLeftTimeParent.GetChild(i).name != "Times")
                //        tfLeftTimeParent.GetChild(i).gameObject.SetActive(false);
                txtIceOnce.text = ContMgr.getCont("a3_lottery_bye");
                btn_iceOnce.gameObject.SetActive(true);
                imgOnceTimer.fillAmount = 0;
                txtIceOnce.gameObject.SetActive(true);
            }
            else if (timer <= 0)
            {
                btn_freeOnce.gameObject.SetActive(true);
                txtLeftTimes.gameObject.SetActive(true);
                //for (int i = 0; i < tfLeftTimeParent.childCount; i++)
                //    tfLeftTimeParent.GetChild(i).gameObject.SetActive(true);
                txtLeftTimes.text = timesDraw.ToString();
                btn_iceOnce.gameObject.SetActive(false);
                txtIceOnce.gameObject.SetActive(false);
                imgOnceTimer.fillAmount = 1;

            }
            else
            {
                btn_freeOnce.gameObject.SetActive(false);
                txtLeftTimes.gameObject.SetActive(false);
                btn_iceOnce.gameObject.SetActive(true);
                txtIceOnce.gameObject.SetActive(true);

                if (updateTime <= 0)
                {
                    updateTime = 1;
                    timer--;
                    outTime = (int)timer;
                    imgOnceTimer.fillAmount = 1 - timer / totalTime;
                    string h = (outTime / 60 / 60).ToString().Length > 1 ? (outTime / 60 / 60).ToString() : "0" + (outTime / 60 / 60);
                    string m = (outTime / 60 % 60).ToString().Length > 1 ? (outTime / 60 % 60).ToString() : "0" + (outTime / 60 % 60);
                    string s = (outTime % 60).ToString().Length > 1 ? (outTime % 60).ToString() : "0" + (outTime % 60);
                    string t = h + ":" + m + ":" + s + ContMgr.getCont("a3_lottery_free");
                    txtIceOnce.text = t;
                }
            }

            if (timerTenth <= 0)
            {
                btn_freeTenth.gameObject.SetActive(true);
                btn_IceTenth.gameObject.SetActive(false);
                txtFreeTenth.gameObject.SetActive(false);
                imgTenthTimer.fillAmount = 1;

            }
            else
            {
                btn_freeTenth.gameObject.SetActive(false);
                btn_IceTenth.gameObject.SetActive(true);
                txtFreeTenth.gameObject.SetActive(true);

                if (updateTimeTenth <= 0)
                {
                    updateTimeTenth = 1;
                    timerTenth--;
                    outTimeTenth = (int)timerTenth;
                    imgTenthTimer.fillAmount = 1 - timerTenth / LotteryModel.getInstance().lotteryTimeTenth;
                    string h = (outTimeTenth / 60 / 60).ToString().Length > 1 ? (outTimeTenth / 60 / 60).ToString() : "0" + (outTimeTenth / 60 / 60);
                    string m = (outTimeTenth / 60 % 60).ToString().Length > 1 ? (outTimeTenth / 60 % 60).ToString() : "0" + (outTimeTenth / 60 % 60);
                    string s = (outTimeTenth % 60).ToString().Length > 1 ? (outTimeTenth % 60).ToString() : "0" + (outTimeTenth % 60);
                    string t = h + ":" + m + ":" + s + ContMgr.getCont("a3_lottery_free");
                    txtFreeTenth.text = t;
                }

            }
            //textTime -= Time.deltaTime;
            //if (textTime <= 0)
            //{
            //    textTime = 5.6f;
            //    LotteryProxy.getInstance().sendlottery((int)LotteryType.CurrentInfo);
            //} 
            // AnimatorStateInfo animatorInfo;
            // animatorInfo = ani_Text.GetCurrentAnimatorStateInfo(0);
            // if(animatorInfo.normalizedTime > 1 && animatorInfo.IsName("drawtext")){
            //     textBg.gameObject.SetActive(false);
            // }else{
            //     textBg.gameObject.SetActive(true);
            // }

            // if (timer < .3f)
            // {
            //     if (timesDraw == 0)
            //     {
            //         btn_freeOnce.gameObject.SetActive(false);
            //         btn_iceOnce.gameObject.SetActive(true);
            //         //txt_freeOnce.text = "明天再来";
            //     }
            //     else
            //     {
            //         //txt_freeOnce.text = "免费一次";
            //     }
            // }
            // timer -= Time.deltaTime;
        }

        public float times = 0;
        public int i;

        private void onUpdateLottery(float s)
        {
            times += s;
            if (times >= 1)
            {
                i--;
                if (i <= 0)
                {
                    i = 0;

                    setFreeOnceDraw(0, true);
                    TickMgr.instance.removeTick(tickItemTime);
                    tickItemTime = null;
                }
                else
                {
                    setFreeOnceDraw(i, false);
                }
                times = 0;
            }
        }

        private void setHintFreeInfo(string info)
        {
            if (!bg_hintFree.gameObject.activeSelf) bg_hintFree.gameObject.SetActive(true);
            hintFreeText.text = info;
        }

        private void setHitIceInfo()
        {
            // if (!bg_hintIce.gameObject.activeSelf) bg_hintIce.gameObject.SetActive(true);
            DestroyModel();
            MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("a3_lottery_gocz"), backDepositHandle);
        }

        private void backDepositHandle()
        {
            InterfaceMgr.getInstance().close(this.uiName);
            toRecharge = true;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
        }

        private class award
        {
            public Transform transform;
            public uint rootType;//
            public uint type;//奖品类型
            public uint id;
            public uint itemType;
            public uint itemId;
            public uint num;
            public string itemName;
            public uint cost;
            public uint stage;
            public uint intensify;
        }

        public void onGetAcquisitionAwardInfo(GameEvent e)
        {
            itemLotteryAwardInfoData lai = new itemLotteryAwardInfoData();
            lai.name = e.data["name"]._str;
            lai.tpid = uint.Parse(e.data["itemId"]);
            lai.cnt = uint.Parse(e.data["cnt"]);
            //lai.tm =lotlogs[i].ContainsKey("intensify")?lotlogs[i]["intensify"]._uint:0;
            lai.stage = uint.Parse(e.data["stage"]);

            GameObject gob = IconImageMgr.getInstance().createLotteryInfo(lai);

            gob.name = "lotteryItemAwardInfo";
            gob.transform.localScale = Vector3.one;
            gob.transform.SetSiblingIndex(0);
        }

        //////////////////////////////////////////////////////////////////////////new code
        public class lotteryAwardController : MonoBehaviour
        {
            private float t = 8.0f;
            private float _awardCount = 11.0f;
            public lotteryAwardController mInstance;
            private List<itemLotteryAward> allLotteryAwardsDic;
            public List<itemLotteryAward> movingLotteryAwards;
            private movePathData _mpd;
            private List<itemLotteryAward> quickMovingLotteryAwards;
            public List<itemLotteryAward> mTempVisualLotteryAwards;
            private itemLotteryAward middleItemLotteryAward;
            private float waitMovesecond;

            public void init(movePathData mpd)
            {
                mInstance = this;
                _mpd = mpd;
                movingLotteryAwards = new List<itemLotteryAward>();
                allLotteryAwardsDic = new List<itemLotteryAward>();
                quickMovingLotteryAwards = new List<itemLotteryAward>();
                mTempVisualLotteryAwards = new List<itemLotteryAward>();
                List<itemLotteryAwardData> lotteryAwardItems = LotteryModel.getInstance().lotteryAwardItems;

                for (int i = 0; i < lotteryAwardItems.Count; i++)
                {
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(lotteryAwardItems[i].itemId);
                    int num = LotteryModel.getInstance().getAwardNumById(lotteryAwardItems[i].id);
                    GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num);
                    go.name = lotteryAwardItems[i].id.ToString();
                    go.transform.SetParent(_mpd.parent);
                    go.transform.localScale = Vector3.zero;
                    go.transform.position = _mpd.arrayV3[0];
                    itemLotteryAward ila = new itemLotteryAward(go.transform);
                    allLotteryAwardsDic.Add(ila);
                }
            }

            private List<itemLotteryAward> allLotteryAwardsDicTemp;

            public void playNormal()
            {
                if (mInstance.IsInvoking("invokeQuickMove")) { mInstance.CancelInvoke("invokeQuickMove"); }
                if (mInstance.IsInvoking("invokeQuickMove4Ten")) { mInstance.CancelInvoke("invokeQuickMove4Ten"); }

                for (int i = 0; i < a3_lottery.mInstance.mV3List.Count; i++)
                {
                    a3_lottery.mInstance.mV3List[i].gameObject.SetActive(false);
                }
                mTempVisualLotteryAwards.Clear();
                movingLotteryAwards.Clear();
                allLotteryAwardsDicTemp = new List<itemLotteryAward>();
                allLotteryAwardsDicTemp.Clear();
                allLotteryAwardsDicTemp.AddRange(allLotteryAwardsDic);
                _mpd.duration = t;
                _mpd.pt = PathType.Linear;
                _mpd.pm = PathMode.Full3D;
                _mpd.isTempObj = false;

                float s = getArcLength(_mpd.arrayV3);
                float v = s / t;
                waitMovesecond = s / _awardCount / v;
                mInstance.InvokeRepeating("invokeWait4oneSecond", waitMovesecond, waitMovesecond);
            }

            private List<itemLotteryAward> quickMovingLotteryAwardsTemp;
            private float _dureationQuickMoveTime = 0.1f;
            private float _repeatRateTime = 0.1f;
            private int movingCount = 0;
            public int mMoveCompleteCount = 0;

            public void playOneAward(int id)//只有一个奖励时候
            {
                movingCount = 0;
                mInstance.CancelInvoke("invokeWait4oneSecond");
                quickMovingLotteryAwards.Clear();
                mTempVisualLotteryAwards.Clear();
                //+11个
                quickMovingLotteryAwards.AddRange(movingLotteryAwards);
                for (int i = 0; i < quickMovingLotteryAwards.Count; i++)
                {
                    quickMovingLotteryAwards[i].doKill();
                }
                int visualCount = quickMovingLotteryAwards.Count;
                for (int index = visualCount; index < 5; index++)//如果可视的移动物品不足5个
                {
                    itemLotteryAward ilaTemp = allLotteryAwardsDicTemp[index];
                    quickMovingLotteryAwards.Add(ilaTemp);
                }
                //+1个real award
                uint itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                int num = LotteryModel.getInstance().getAwardNumById((uint)id);
                GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num);
                go.name = "itemLotteryAward";
                go.transform.SetParent(_mpd.parent);
                go.transform.localScale = Vector3.zero;
                go.transform.position = _mpd.arrayV3[0];
                itemLotteryAward ila = new itemLotteryAward(go.transform);

                quickMovingLotteryAwards.Add(ila);
                middleItemLotteryAward = ila;
                //+5 个
                if (movingLotteryAwards.Count < 6)
                {
                    for (int i = 6; i < 11; i++)
                    {
                        itemLotteryAward ilaTemp = allLotteryAwardsDicTemp[i];
                        quickMovingLotteryAwards.Add(ilaTemp);
                    }
                }
                else
                {
                    for (int i = movingLotteryAwards.Count; i < movingLotteryAwards.Count + 5; i++)
                    {
                        itemLotteryAward ilaTemp = allLotteryAwardsDicTemp[i];
                        quickMovingLotteryAwards.Add(ilaTemp);
                    }
                }

                if (quickMovingLotteryAwardsTemp == null)
                {
                    quickMovingLotteryAwardsTemp = new List<itemLotteryAward>();
                }
                else
                {
                    quickMovingLotteryAwardsTemp.Clear();
                }
                quickMovingLotteryAwardsTemp.AddRange(quickMovingLotteryAwards);
                movingCount = movingLotteryAwards.Count;
                mInstance.InvokeRepeating("invokeQuickMove", _dureationQuickMoveTime, _repeatRateTime);
            }

            public void playTenAward(List<int> ids)//11个奖励时候
            {
                mMoveCompleteCount = 0;
                mInstance.CancelInvoke("invokeWait4oneSecond");
                mInstance.CancelInvoke("invokeQuickMove4Ten");
                quickMovingLotteryAwards.Clear();
                quickMovingLotteryAwards.AddRange(movingLotteryAwards);
                //+11个real award
                for (int i = 0; i < ids.Count; i++)
                {
                    int id = ids[i];
                    uint itemId = LotteryModel.getInstance().getAwardItemIdById((uint)id);
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(itemId);
                    int num = LotteryModel.getInstance().getAwardNumById((uint)id);
                    GameObject go = IconImageMgr.getInstance().createItemIcon4Lottery(itemData, false, num);
                    go.name = "itemLotteryAward";
                    go.transform.SetParent(_mpd.parent);
                    go.transform.localScale = Vector3.zero;
                    go.transform.position = _mpd.arrayV3[0];
                    itemLotteryAward ila = new itemLotteryAward(go.transform);
                    quickMovingLotteryAwards.Add(ila);
                }
                movingCount = movingLotteryAwards.Count;
                if (quickMovingLotteryAwardsTemp == null)
                {
                    quickMovingLotteryAwardsTemp = new List<itemLotteryAward>();
                }
                else
                {
                    quickMovingLotteryAwardsTemp.Clear();
                }
                quickMovingLotteryAwardsTemp.AddRange(quickMovingLotteryAwards);
                Debug.LogWarning(_dureationQuickMoveTime + ":_dureationQuickMoveTime" + "|_repeatRateTime:" + _repeatRateTime);
                mInstance.InvokeRepeating("invokeQuickMove4Ten", _dureationQuickMoveTime, _repeatRateTime);
            }

            private void invokeWait4oneSecond()
            {
                if (allLotteryAwardsDicTemp.Count > 0)
                {
                    allLotteryAwardsDicTemp[0].root.localScale = Vector3.one;
                    allLotteryAwardsDicTemp[0].doPath(_mpd);
                    itemLotteryAward _lai = allLotteryAwardsDicTemp[0];
                    allLotteryAwardsDicTemp.RemoveAt(0);
                    allLotteryAwardsDicTemp.Add(_lai);
                }
                else
                {
                    mInstance.CancelInvoke("invokeWait4oneSecond");
                }
            }

            private void invokeQuickMove()//快速移动
            {
                if (quickMovingLotteryAwardsTemp.Count > 0)
                {
                    int index = quickMovingLotteryAwards.IndexOf(quickMovingLotteryAwardsTemp[0]);
                    movePathData mpdTemp = (movePathData)_mpd.Clone();
                    mpdTemp.arrayV3 = getPathByIndex(index);
                    mpdTemp.duration = 2.0f;
                    if (quickMovingLotteryAwards.Count >= 17)
                    {
                        mpdTemp.hide = index < 6 ? true : false;
                    }
                    else
                    {
                        mpdTemp.hide = false;
                    }

                    quickMovingLotteryAwardsTemp[0].root.localScale = Vector3.one;

                    if (middleItemLotteryAward == quickMovingLotteryAwardsTemp[0])
                    {
                        quickMovingLotteryAwardsTemp[0].doQuickPath(mpdTemp, true);
                    }
                    else
                    {
                        quickMovingLotteryAwardsTemp[0].doQuickPath(mpdTemp);
                        mTempVisualLotteryAwards.Add(quickMovingLotteryAwardsTemp[0]);
                    }
                    itemLotteryAward _lai = quickMovingLotteryAwardsTemp[0];
                    quickMovingLotteryAwardsTemp.RemoveAt(0);
                }
                else
                {
                    mInstance.CancelInvoke("invokeQuickMove");
                }
            }

            private void invokeQuickMove4Ten()//快速移动
            {

                if (mInstance.IsInvoking("invokeWait4oneSecond")) { mInstance.CancelInvoke("invokeWait4oneSecond"); }
                if (mInstance.IsInvoking("invokeQuickMove")) { mInstance.CancelInvoke("invokeQuickMove"); }

                if (quickMovingLotteryAwardsTemp.Count > 0)
                {
                    int index = quickMovingLotteryAwards.IndexOf(quickMovingLotteryAwardsTemp[0]);
                    movePathData mpdTemp = (movePathData)_mpd.Clone();
                    mpdTemp.arrayV3 = getPathByIndex4Ten(index);
                    mpdTemp.isTempObj = index < movingCount ? false : true;
                    mpdTemp.duration = 2.0f;

                    mpdTemp.hide = index < movingCount ? true : false;

                    quickMovingLotteryAwardsTemp[0].root.localScale = Vector3.one;

                    quickMovingLotteryAwardsTemp[0].doQuickPathTen(mpdTemp);
                    quickMovingLotteryAwardsTemp.RemoveAt(0);
                }
                else
                {
                    mInstance.CancelInvoke("invokeQuickMove4Ten");
                }
            }

            private Vector3[] getPathByIndex(int index)//根据索引获得路径
            {
                if (quickMovingLotteryAwards.Count < 17)//初始时候,可能正在运动的物品数量不足11个
                {
                    if (quickMovingLotteryAwards.Count > 11)
                    {
                        Vector3[] pathV3 = new Vector3[_mpd.arrayV3.Length];
                        _mpd.arrayV3.CopyTo(pathV3, 0);
                        List<Vector3> pathV3List = pathV3.ToList();

                        if (index < movingCount)
                        {
                            for (int i = 0; i < movingCount - index; i++)
                            {
                                try
                                {
                                    pathV3List.RemoveAt(0);
                                }
                                catch (System.Exception ex)
                                {
                                    Debug.LogError("i:" + i + "index:" + index + "movingCount:" + movingCount);
                                }
                            }
                            int overCount = quickMovingLotteryAwards.Count - 11;
                            if (overCount < index)//显示的道具
                            {
                                for (int i = 0; i < index - overCount; i++)
                                {
                                    int count = pathV3List.Count;
                                    pathV3List.RemoveAt(count - 1);
                                }
                            }
                            return pathV3List.ToArray();
                        }
                        else
                        {
                            int moveCount = pathV3List.Count - (quickMovingLotteryAwards.Count - index);
                            for (int i = 0; i < moveCount; i++)
                            {
                                int count = pathV3List.Count;
                                pathV3List.RemoveAt(count - 1);
                            }
                            return pathV3List.ToArray();
                        }
                    }
                    else
                    {
                        Vector3[] pathV3 = new Vector3[_mpd.arrayV3.Length];
                        _mpd.arrayV3.CopyTo(pathV3, 0);
                        List<Vector3> pathV3List = pathV3.ToList();

                        if (index < movingCount)
                        {
                            for (int i = 0; i < movingCount; i++)
                            {
                                pathV3List.RemoveAt(i);
                            }
                        }
                        for (int i = 0; i < index; i++)
                        {
                            int count = pathV3List.Count;
                            pathV3List.RemoveAt(count - 1);
                        }
                        return pathV3List.ToArray();
                    }
                }
                else
                {
                    Vector3[] pathV3 = new Vector3[_mpd.arrayV3.Length];
                    _mpd.arrayV3.CopyTo(pathV3, 0);
                    if (index <= _mpd.arrayV3.Length)
                    {
                        List<Vector3> pathV3List = pathV3.ToList();
                        for (int i = _mpd.arrayV3.Length; i > index; i--)
                        {
                            pathV3List.RemoveAt(0);
                        }
                        if (index >= 6)
                        {
                            int removeCount = index - 6;
                            for (int i = 0; i < removeCount; i++)
                            {
                                pathV3List.RemoveAt(pathV3List.Count - 1);
                            }
                        }
                        _dureationQuickMoveTime = 0.0f;
                        _repeatRateTime = 0.1f;
                        return pathV3List.ToArray();
                    }
                    else
                    {
                        List<Vector3> pathV3List = pathV3.ToList();
                        int removeCount = pathV3List.Count - (quickMovingLotteryAwards.Count - index);
                        for (int i = 0; i < removeCount; i++)
                        {
                            pathV3List.RemoveAt(pathV3List.Count - 1);
                        }
                        _dureationQuickMoveTime = 0.0f;
                        _repeatRateTime = waitMovesecond;
                        return pathV3List.ToArray();
                    }
                }
            }

            private Vector3[] getPathByIndex4Ten(int index)//根据索引获得路径
            {
                List<Vector3> tempPathV3 = new List<Vector3>();
                Vector3[] pathV3 = new Vector3[_mpd.arrayV3.Length];
                _mpd.arrayV3.CopyTo(pathV3, 0);
                if (index < movingCount)
                {
                    List<Vector3> pathV3List = pathV3.ToList();
                    for (int i = 0; i < movingCount - index; i++)
                    {
                        pathV3List.RemoveAt(0);
                    }
                    _dureationQuickMoveTime = 0.0f;
                    _repeatRateTime = 0.1f;
                    _mpd.isTempObj = false;
                    tempPathV3 = pathV3List;
                }
                else
                {
                    List<Vector3> pathV3List = pathV3.ToList();
                    int removeCount = index - (movingCount);//DOTO:这里算的不对;
                    for (int i = 0; i < removeCount; i++)
                    {
                        int removeIndex = pathV3List.Count - 1;
                        pathV3List.RemoveAt(removeIndex);
                    }
                    _dureationQuickMoveTime = 0.0f;
                    _repeatRateTime = 0.1f;
                    _mpd.isTempObj = true;
                    tempPathV3 = pathV3List;
                }

                return tempPathV3.ToArray();
            }

            private float getArcLength(Vector3[] v3Array)
            {
                float f = 0;
                for (int i = 0; i < v3Array.Length - 1; i++)
                {
                    f += Vector3.Distance(v3Array[i + 1], v3Array[i]);
                }
                return f;
            }

            public void close()
            {
                for (int i = 0; i < movingLotteryAwards.Count; i++)
                {
                    movingLotteryAwards[i].resetPos();
                }
                mInstance.CancelInvoke("invokeWait4oneSecond");
                mInstance.CancelInvoke("invokeQuickMove");
                mInstance.CancelInvoke("invokeQuickMove4Ten");
            }
        }

        public class itemLotteryAward : Skin
        {
            public Transform root;
            private movePathData _mpd = new movePathData();
            private bool isMoving = false;
            private Vector3 startPostion;

            public itemLotteryAward(Transform trans) : base(trans)
            {
                root = trans;
                UIClient.instance.addEventListener(UI_EVENT.ON_STOPLOTTERYAWARD, onStopLotteryAward);
            }

            public void doPath(movePathData mpd)
            {
                //if (!mInstance._lotterAwardController.movingLotteryAwards.Contains(this))
                //{
                //    mInstance._lotterAwardController.movingLotteryAwards.Add(this);
                //}
                _mpd = mpd;
                startPostion = _mpd.arrayV3[0];
                //this.transform.DOPath(mpd.arrayV3, mpd.duration, mpd.pt, mpd.pm).SetEase(Ease.Linear)
                //    .OnComplete(() => { this.transform.localScale = Vector3.zero; this.transform.position = mpd.arrayV3[0]; mInstance._lotterAwardController.movingLotteryAwards.Remove(this); });
            }

            public void doQuickPath(movePathData mpd, bool middleTarget = false)
            {
                //if (!mInstance._lotterAwardController.movingLotteryAwards.Contains(this))
                //{
                //    mInstance._lotterAwardController.movingLotteryAwards.Add(this);
                //}
                _mpd = mpd;
                //if (_mpd.arrayV3.Length <= 0) { mInstance._lotterAwardController.movingLotteryAwards.Remove(this); isMoving = false; this.transform.localScale = Vector3.zero; return; }

                if (middleTarget)
                {
                    this.transform.DOPath(mpd.arrayV3, mpd.duration, mpd.pt, mpd.pm).SetEase(Ease.Linear).OnUpdate(() =>//当最中间一个物品移动到中间位置时候通知其他物品停止移动
                    {
                        Vector3 v3 = mInstance._arrayV3[mInstance._arrayV3.Length / 2 + 1];
                        if (this.transform.position - v3 == Vector3.zero)
                        {
                            UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_STOPLOTTERYAWARD, this, null));
                            this.transform.DOPause();
                        }
                        isMoving = true;
                    }).OnComplete(() => { moveToBag(_mpd); mInstance.mV3List[mInstance._arrayV3.Length / 2].gameObject.SetActive(true); });
                }
                //else
                //{
                //    this.transform.DOPath(mpd.arrayV3, mpd.duration, mpd.pt, mpd.pm).SetEase(Ease.Linear).OnUpdate(() => { isMoving = true; })
                //        .OnComplete(() => { this.transform.DOKill(); mInstance._lotterAwardController.movingLotteryAwards.Remove(this); isMoving = false; this.transform.localScale = mpd.hide ? Vector3.zero : Vector3.one; });
                //}
            }

            public void doQuickPathTen(movePathData mpd)
            {
                //if (!mInstance._lotterAwardController.movingLotteryAwards.Contains(this))
                //{
                //    mInstance._lotterAwardController.movingLotteryAwards.Add(this);
                //}
                _mpd = mpd;
                //if (_mpd.arrayV3.Length <= 0) { mInstance._lotterAwardController.movingLotteryAwards.Remove(this); isMoving = false; this.transform.localScale = Vector3.zero; return; }

                //this.transform.DOPath(mpd.arrayV3, mpd.duration, mpd.pt, mpd.pm).SetEase(Ease.Linear).OnUpdate(() => { isMoving = true; })
                //    .OnComplete(() => { this.transform.DOKill(); mInstance._lotterAwardController.movingLotteryAwards.Remove(this); onAllMoveComplete(mpd); isMoving = false; this.transform.localScale = mpd.hide ? Vector3.zero : Vector3.one; });
            }

            private void onStopLotteryAward(GameEvent e)
            {
                if (isMoving)
                {
                    this.transform.DOPause();
                }
            }

            private void onAllMoveComplete(movePathData mpd)
            {
                //int movingCount = mInstance._lotterAwardController.movingLotteryAwards.Count;
                //if (movingCount <= 0)
                //{
                //    for (int i = 0; i < mInstance.mV3List.Count; i++)
                //    {
                //        mInstance.mV3List[i].gameObject.SetActive(true);
                //    }
                //}
                moveToBag4Ten(_mpd);
            }

            public void play()
            {
                this.transform.DOPlay();
            }

            public void resetPos()
            {
                this.transform.position = _mpd.arrayV3[0];
                this.transform.localScale = Vector3.zero;
                this.transform.DOKill();
            }

            public void doKill()
            {
                this.transform.DOKill();
            }

            public void moveToBag(movePathData mpd)
            {
                this.transform.DOMove(mpd.endPos, mpd.duration).SetEase(Ease.InQuint).OnComplete(() =>
                {
                    this.transform.localScale = Vector3.zero;
                    //List<itemLotteryAward> ilaList = mInstance._lotterAwardController.mInstance.mTempVisualLotteryAwards;
                    //for (int i = 0; i < ilaList.Count; i++)
                    //{
                    //    ilaList[i].root.localScale = Vector3.zero;
                    //    ilaList[i].root.position = mpd.startPos;
                    //}
                    //mInstance._lotterAwardController.mInstance.playNormal();
                });
            }

            public void moveToBag4Ten(movePathData mpd)
            {
                this.transform.DOMove(mpd.endPos, mpd.duration).SetEase(Ease.InQuint).OnComplete(() =>
                {
                    this.transform.localScale = Vector3.zero;
                    if (_mpd.isTempObj)
                    {
                        Destroy(this.transform.gameObject);
                        //mInstance._lotterAwardController.mMoveCompleteCount++;
                        //if (mInstance._lotterAwardController.mMoveCompleteCount == 11)
                        //{
                        //    mInstance._lotterAwardController.mInstance.playNormal();
                        //    for (int i = 0; i < mInstance.mV3List.Count; i++)
                        //    {
                        //        mInstance.mV3List[i].gameObject.SetActive(false);
                        //    }
                        //}
                    }
                    else
                    {
                        transform.localScale = Vector3.zero;
                        transform.position = mpd.startPos;
                    }
                });
            }
        }

        public class lotteryInfoPanel : Skin
        {
            public Transform root;
            private Transform _content;
            public Transform Content => _content;
            private Queue<itemLotteryInfo> iliQueue;
            public lotteryInfoPanel(Transform trans) : base(trans)
            {
                root = trans;
                iliQueue = new Queue<itemLotteryInfo>();
                BaseButton btnClose = new BaseButton(getTransformByPath("title/btnClose"));
                btnClose.onClick = onBtnCloseClick;
                _content = transform.FindChild("scroll/contents");
            }

            private void onBtnCloseClick(GameObject go)
            {
                root.gameObject.SetActive(false);
            }

            public void onShow(Variant dt)
            {
                float itemHight = float.MinValue;
                if (dt.ContainsKey("lotlog"))
                {
                    List<Variant> lotlogs = dt["lotlog"]._arr;
                    debug.Log(dt["lotlog"].dump());
                    for (int i = 0; i < lotlogs.Count; i++)
                    {
                        itemLotteryAwardInfoData lai = new itemLotteryAwardInfoData();
                        lai.name = lotlogs[i]["name"]._str;
                        lai.tpid = lotlogs[i]["tpid"]._uint;
                        lai.cnt = lotlogs[i]["cnt"]._uint;
                        lai.tm = lotlogs[i].ContainsKey("intensify") ? lotlogs[i]["intensify"]._uint : 0;
                        lai.stage = lotlogs[i].ContainsKey("stage") ? lotlogs[i]["stage"]._uint : 0;
                        if (IsNewLotteryItem(lai))
                        {
                            GameObject gob = IconImageMgr.getInstance().createLotteryInfo(lai);
                            itemLotteryInfo ili = new itemLotteryInfo(gob.transform, _content);
                            if (!iliQueue.Contains(ili))
                            {
                                iliQueue.Enqueue(ili);
                                LotteryModel.getInstance().lotteryAwardInfoItems.Add(lai);
                            }
                            if (itemHight == float.MinValue) itemHight = gob.GetComponent<LayoutElement>().preferredHeight;

                            if (iliQueue.Count > 10)
                            {
                                for (int j = iliQueue.Count - 1; j > 0; j--)
                                    if (iliQueue.Count > 10)
                                    {
                                        LotteryModel.getInstance().lotteryAwardInfoItems.RemoveAt(0);
                                        DestroyImmediate(iliQueue.Dequeue().gameObject);
                                    }
                                    else
                                        break;
                            }
                        }
                    }
                    RectTransform contentnRt = _content.GetComponent<RectTransform>();
                    //float hightContentRt = lotlogs.Count * itemHight;
                    //contentnRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hightContentRt);
                    //flytxt.instance.fly(_content.GetComponent<RectTransform>().sizeDelta.y.ToString());
                }
            }
            private bool IsNewLotteryItem(itemLotteryAwardInfoData inf)
            {
                List<itemLotteryAwardInfoData> infList = LotteryModel.getInstance().lotteryAwardInfoItems;
                if (infList.Count < 10) return true;
                for (int i = 0; i < infList.Count; i++)
                    if (inf.name.Equals(infList[i].name) && inf.tm == infList[i].tm && inf.tpid == infList[i].tpid)
                        return false;
                return true;
            }
            public void onClosed()
            {
                for (int i = iliQueue.Count; i > 0; i--)
                {
                    Destroy(iliQueue.Dequeue().gameObject);
                }
                LotteryModel.getInstance().lotteryAwardInfoItems.Clear();
            }
        }

        private class itemLotteryInfo : Skin
        {
            public Transform root;

            public itemLotteryInfo(Transform trans, Transform parent) : base(trans)
            {
                GameObject go = trans.gameObject;
                root = go.transform;
                go.name = "lotteryItemAwardInfo";
                go.transform.localScale = Vector3.one;
                root.SetParent(parent, false);
            }
        }

        public class movePathData : ICloneable
        {
            public Vector3[] arrayV3;
            public float duration;
            public PathType pt;
            public PathMode pm;
            public Vector3 startPos;
            public Vector3 endPos;
            public Transform parent;
            public bool hide;
            public bool isTempObj;

            public object Clone()
            {
                movePathData mpd = new movePathData();
                mpd.arrayV3 = new Vector3[this.arrayV3.Length];
                this.arrayV3.CopyTo(mpd.arrayV3, 0);
                mpd.duration = this.duration;
                mpd.hide = this.hide;
                mpd.endPos = this.endPos;
                mpd.startPos = this.startPos;
                mpd.isTempObj = this.isTempObj;
                return mpd;
            }
        }

        //////////////////////////////////
        // 延迟执行
        public class DelayToInvoke : MonoBehaviour
        {
            public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
            {                
                yield return new WaitForSeconds(delaySeconds);
                action();
            }
        }


    }
}