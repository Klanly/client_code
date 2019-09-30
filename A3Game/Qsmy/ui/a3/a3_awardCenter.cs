using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

namespace MuGame
{
    class a3_awardCenter:Window
    {
        /* 每日充值奖励*/
        GameObject thisday_cost_image,
                   thisday_cost_contain;
        


        /*基金*/
        GameObject zuanshi_image,
                   zhuansheng_image,
                   zhanli_image;
        Transform  zuanshi_contain,
                   zhuansheng_contain,
                   zhanli_contain;
        GameObject zuanshi_btn,
                   zhuansheng_btn,
                   zhanli_btn;

        private TabControl _tabControl;
        private Transform _itemAward;
        private BaseAwardCenter _current;
        private BaseAwardCenter _lvlUpAwardPanel;
        private BaseAwardCenter _rechargePanel;
        private BaseAwardCenter _payPanel;
        private BaseAwardCenter _todayRechargePanel;
        private BaseAwardCenter _ExchangePanel;
        BaseAwardCenter _goldfund,
                        _livefund,
                        _cmtfund,
                        _thisdaycost;
        private Transform _content;
        public GameObject tip;
        public static a3_awardCenter instan;

        private List<GameObject> tabControlGoLst;
        private Dictionary<int,Variant> awardData;

        private Text timeText;

        public static Dictionary<int,Variant>   roleData = new Dictionary<int , Variant>();

        public static Sprite yellowSprite;
        public static Sprite graySprite;
        public static a3_awardCenter _instance;

        public static int finishNum = 0;


        public List<GameObject> lst = new List<GameObject>();
        #region ScrollControler
        ScrollControler scrollControer0;
        ScrollControler scrollControer1;
        ScrollControler scrollControer2;
        ScrollControler scrollControer3;
        ScrollControler scrollControer4;
        ScrollControler scrollControer5;
        ScrollControler scrollControer6;
        ScrollControler scrollControer7;
        #endregion

        override public void init()
        {
            #region ScrollControler
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("temp/lvlAwardPanel/awardItems"));
            scrollControer1 = new ScrollControler();
            scrollControer1.create(getComponentByPath<ScrollRect>("temp/rechargePanel/awardItems"));
            scrollControer2 = new ScrollControler();
            scrollControer2.create(getComponentByPath<ScrollRect>("temp/payPanel/awardItems"));
            scrollControer3 = new ScrollControler();
            scrollControer3.create(getComponentByPath<ScrollRect>("temp/toDayRechargePanel/awardItems"));
            scrollControer4 = new ScrollControler();
            scrollControer4.create(getComponentByPath<ScrollRect>("temp/exchangePanel/awardItems"));
            scrollControer5 = new ScrollControler();
            scrollControer5.create(getComponentByPath<ScrollRect>("temp/goldfund/down/Scrollview"));
            scrollControer6 = new ScrollControler();
            scrollControer6.create(getComponentByPath<ScrollRect>("temp/livefund/down/Scrollview"));
            scrollControer7 = new ScrollControler();
            scrollControer7.create(getComponentByPath<ScrollRect>("temp/cmtfund/down/Scrollview"));


            #endregion



            _content = transform.FindChild( "body/Center/right" );
            _itemAward = transform.FindChild("temp/itemAward");
            tip = transform.FindChild("tip").gameObject;

            BaseButton btnClose = new BaseButton(transform.FindChild("title/btnClose"));
            btnClose.onClick = onBtnCloseClick;
            _tabControl = new TabControl();
            _tabControl.onClickHanle = OnSwitch;
            _tabControl.create(getGameObjectByPath("body/left"), this.gameObject,-1);

            timeText = getGameObjectByPath( "body/Center/Time" ).GetComponent<Text>();

            tabControlGoLst = new List<GameObject>();
            Transform leftTab = getGameObjectByPath( "body/left" ).transform;
            for ( int i = 0 ; i < (leftTab.childCount-3) ; i++ )
            {
                tabControlGoLst.Add( leftTab.GetChild(i).gameObject);
              
            }
            lst.Add(leftTab.GetChild(leftTab.childCount - 1).gameObject);
            lst.Add(leftTab.GetChild(leftTab.childCount - 2).gameObject);
            lst.Add(leftTab.GetChild(leftTab.childCount - 3).gameObject);
            //A3_AwardCenterServer.getInstance().addEventListener( A3_AwardCenterServer.EVENT_AWARDLST , BuildAwardLstData );
            awardData = new Dictionary<int , Variant>();

            //roleData = new Dictionary<int , Variant>();

            //奖励中心   读表
            this.transform.FindChild("body/left/btn_goldfund/Text").GetComponent<Text>().text = ContMgr.getCont("btn_goldfund");
            this.transform.FindChild("body/left/btn_livefund/Text").GetComponent<Text>().text = ContMgr.getCont("btn_livefund");
            this.transform.FindChild("body/left/btn_cmtfund/Text").GetComponent<Text>().text = ContMgr.getCont("btn_cmtfund");
            this.transform.FindChild("body/left/btnRechargeAward/Text").GetComponent<Text>().text = ContMgr.getCont("btnRechargeAward");
            this.transform.FindChild("body/left/btnPay/Text").GetComponent<Text>().text = ContMgr.getCont("btnPay");
            this.transform.FindChild("body/left/btnAloneRecharge/Text").GetComponent<Text>().text = ContMgr.getCont("btnAloneRecharge");
            this.transform.FindChild("body/left/btnTuan/Text").GetComponent<Text>().text = ContMgr.getCont("btnTran");
            this.transform.FindChild("body/left/btnExchange/Text").GetComponent<Text>().text = ContMgr.getCont("btnExchange");

            this.transform.FindChild("temp/goldfund/top/Image/Text").GetComponent<Text>().text = ContMgr.getCont("btn_goldfund");
            this.transform.FindChild("temp/goldfund/top/Image/Text (1)").GetComponent<Text>().text = ContMgr.getCont("FanHuan");
            this.transform.FindChild("temp/goldfund/top/Image/Text (4)").GetComponent<Text>().text = ContMgr.getCont("YijiGouMai");

            this.transform.FindChild("temp/livefund/top/Image/Text").GetComponent<Text>().text = ContMgr.getCont("btn_livefund");
            this.transform.FindChild("temp/livefund/top/Image/Text (1)").GetComponent<Text>().text = ContMgr.getCont("FanHuan");
            this.transform.FindChild("temp/livefund/top/Image/Text (4)").GetComponent<Text>().text = ContMgr.getCont("YijiGouMai");

            this.transform.FindChild("temp/cmtfund/top/Image/Text").GetComponent<Text>().text = ContMgr.getCont("btn_cmtfund");
            this.transform.FindChild("temp/cmtfund/top/Image/Text (1)").GetComponent<Text>().text = ContMgr.getCont("FanHuan");
            this.transform.FindChild("temp/cmtfund/top/Image/Text (4)").GetComponent<Text>().text = ContMgr.getCont("YijiGouMaivip");
            this.transform.FindChild("temp/livefund/top/Button/Text").GetComponent<Text>().text = ContMgr.getCont("ButtonText1");
            this.transform.FindChild("temp/cmtfund/top/Button/Text").GetComponent<Text>().text = ContMgr.getCont("ButtonText2");
            this.transform.FindChild("temp/goldfund/top/Button/Text").GetComponent<Text>().text = ContMgr.getCont("ButtonText");
            getComponentByPath<Text>("temp/toDayRechargePanel/Details/Text (1)").text = ContMgr.getCont("buypeople");
            getComponentByPath<Text>("temp/toDayRechargePanel/Details/Text").text = ContMgr.getCont("buypicple");
            getComponentByPath<Text>("tip/text_bg/name/lite").text = ContMgr.getCont("shiyongdengji");
            getComponentByPath<Text>("tip/text_bg/name/has").text = ContMgr.getCont("haved");




            /*基金*/
            zuanshi_image = getGameObjectByPath("temp/goldfund/down/Scrollview/Image");
            zhuansheng_image = getGameObjectByPath("temp/livefund/down/Scrollview/Image");
            zhanli_image = getGameObjectByPath("temp/cmtfund/down/Scrollview/Image");
            zuanshi_contain = getTransformByPath("temp/goldfund/down/Scrollview/contain");
            zhuansheng_contain = getTransformByPath("temp/livefund/down/Scrollview/contain");
            zhanli_contain = getTransformByPath("temp/cmtfund/down/Scrollview/contain");
            zuanshi_btn = getGameObjectByPath("temp/goldfund/top/Button");
            zhuansheng_btn = getGameObjectByPath("temp/livefund/top/Button");
            zhanli_btn = getGameObjectByPath("temp/cmtfund/top/Button");
            new BaseButton(zuanshi_btn.transform).onClick = (GameObject go) => { btn_buyfund(1); };
            new BaseButton(zhuansheng_btn.transform).onClick = (GameObject go) => { btn_buyfund(2); };
            new BaseButton(zhanli_btn.transform).onClick = (GameObject go) => { btn_buyfund(3); };
            creatrve_fund(zuanshi_image, zuanshi_contain, 1);
            creatrve_fund(zhuansheng_image, zhuansheng_contain, 2);
            creatrve_fund(zhanli_image, zhanli_contain, 3);

            //welfareProxy.getInstance().showIconLight();

            /*当日充值奖励*/
            thisday_cost_image = getGameObjectByPath("temp/thisdaycost/down/Scrollview/Image");
            thisday_cost_contain= getGameObjectByPath("temp/thisdaycost/down/Scrollview/contain");

            BuildAwardLstData( A3_AwardCenterServer.getInstance().Alldata );

            creatrve_cost();
           
        }

        override public void onShowed()
        {
            _instance = this;
            A3_AwardCenterServer.getInstance().SendMsg(A3_AwardCenterServer.SERVER_SELFDATA);
            //A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.SERVER_SELFDATA);
            if (uiData == null)
            {
                //单笔充值过期，一直显示每日充值
                //if(A3_AwardCenterServer.enddbcz!=0&& A3_AwardCenterServer.enddbcz-muNetCleint.instance.CurServerTimeStamp<=0)
                //{
                //    _tabControl.setSelectedIndex(1);
                //}else
                //    {
                //    _tabControl.setSelectedIndex(0);
                //}
                if(A3_AwardCenterServer.getInstance().havedbcy&& !A3_AwardCenterServer.getInstance().isdbcz)
                    _tabControl.setSelectedIndex(0);
                else
                    _tabControl.setSelectedIndex(1);


            }
            else
            {
                if ((int)uiData[0] == 0)
                {
                    _tabControl.setSelectedIndex(2);
                }
                else if ((int)uiData[0] == 1)
                {
                    _tabControl.setSelectedIndex(0);
                }
            }
            OnSwitch(_tabControl);
            if (_current != null)
                _current.onShowed();
            RefreshInfo();
        }

        private void BuildAwardLstData( GameEvent e )
        {
            Variant data = e.data;

            InterfaceMgr.getInstance().changeState( InterfaceMgr.STATE_FUNCTIONBAR );
            tip.SetActive( false );
            instan = this;
            //welfareProxy.getInstance().sendWelfare( welfareProxy.ActiveType.selfWelfareInfo );

            Variant awardLst = data["actonline_conf"];

            foreach ( GameObject item in tabControlGoLst )
            {
                item.SetActive( false );
            }
            foreach (GameObject i in lst)
            {
                i.SetActive(false);
            }

            //int fristIndex = -1;
            int fristIndex=0;
            int currServerTime =  muNetCleint.instance.CurServerTimeStamp;
            //按钮第一个过期了就显示第二个
            bool isfristisover = false;
            if (awardLst._arr.Count  <= 0)
            {
                tabControlGoLst[1].SetActive(true);
            }
            else
            {
                foreach (var item in awardLst._arr)
                {
                    //if ( currServerTime>=item["end"] )
                    //{
                    //    break;
                    //}
                    // awardData[ item[ "tp" ] ] = item;
                    // Debug.LogError("tp是：" + item["tp"]);
                    // int typeId = item["tp"];
                    // tabControlGoLst[ typeId -1 ].SetActive( true );
                    //if (typeId - 1==0)
                    // {
                    //     isfristisover = true;
                    // }


                    awardData[item["tp"]] = item;

                    int typeId = item["tp"];
                    if (typeId == 1 && currServerTime >= item["end"])
                    {
                        isfristisover = true;
                        tabControlGoLst[1].SetActive(true);
                    }
                    /*
                        tp =1   单笔充值  inedx=0
                        tp=2    消费返利  inedx=4
                        tp=3    充值有奖  inedx=2
                        tp=4    超值团购  inedx=5
                        tp=5    兑换奖励  inedx=3

                     * */
                    if (typeId != 1 && currServerTime >= item["end"])
                    {
                        int dex = -1;
                        switch (typeId)
                        {
                            case 1:
                                dex = 0;
                                break;
                            case 2:
                                dex = 4;
                                break;
                            case 3:
                                dex = 2;
                                break;
                            case 4:
                                dex = 5;
                                break;
                            case 5:
                                dex = 3;
                                break;

                        }
                        if (dex != -1)
                            tabControlGoLst[dex].SetActive(false);
                    }
                    if (currServerTime < item["end"])
                    {
                        int dex = -1;
                        switch (typeId)
                        {
                            case 1:
                                dex = 0;
                                break;
                            case 2:
                                dex = 4;
                                break;
                            case 3:
                                dex = 2;
                                break;
                            case 4:
                                dex = 5;
                                break;
                            case 5:
                                dex = 3;
                                break;

                        }
                        if (dex != -1)
                            tabControlGoLst[dex].SetActive(true);
                        //tabControlGoLst[5].SetActive(true);
                    }
                    tabControlGoLst[1].SetActive(true);


                    // tabControlGoLst[ typeId -1 ].transform.SetAsLastSibling();
                    //if ( fristIndex == -1)
                    //{
                    //    fristIndex = typeId -1;
                    //}
                }
            }

            foreach (GameObject i in lst)
            {
                i.SetActive(true);
            }

            if ( _current!=null )
            {
                _current.onShowed();
            }
            else 
            {
                    _tabControl.setSelectedIndex(isfristisover?fristIndex:1);
                    OnSwitch( _tabControl );
            }
            GRMap.GAME_CAMERA.SetActive( false );


        }

        override public void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            instan = null;
        }

       public void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            transform.FindChild("tip/text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }

        public void OnSwitch(TabControl tabc)
        {
            int index = tabc.getSeletedIndex();
            if (_current!=null)
            {
                _current.onClose();
                _current.gameObject.SetActive(false);
            }
            /*单笔充值*/
            if (index==0)
            {
                if (_payPanel == null)
                {
                    Transform tr = transform.FindChild("temp/payPanel");
                    _payPanel = new PayPanel(tr, awardData[index + 1], timeText);
                    _payPanel.setPerent(_content);
                }
                _current = _payPanel;
                //if (_lvlUpAwardPanel==null)
                //{
                //    Transform tr = transform.FindChild("temp/lvlAwardPanel");
                //   _lvlUpAwardPanel = new LvlUpAwardPanel(tr , awardData[ index+1 ] , timeText );
                //   _lvlUpAwardPanel.setPerent(_content);
                //}
                //_current = _lvlUpAwardPanel;
            }
            /*每日充值*/
            if (index==1)
            {
                if (_thisdaycost == null)
                {
                    Transform tr = getTransformByPath("temp/thisdaycost");
                    _thisdaycost = new ThisdayCost(tr);

                }
                _current = _thisdaycost;
                getComponentByPath<ScrollRect>("temp/cmtfund/down/Scrollview").verticalNormalizedPosition = 1;
                //if (_rechargePanel == null)
                //{
                //    Transform tr = transform.FindChild("temp/rechargePanel");
                //    _rechargePanel = new RechargePanel( tr , awardData[ index +1 ] , timeText );
                //    _rechargePanel.setPerent(_content);
                //}
                //_current = _rechargePanel;
            }
            /*充值奖励*/
            if (index==2)
            {
                _current = _payPanel;
                if (_lvlUpAwardPanel == null)
                {
                    Transform tr = transform.FindChild("temp/lvlAwardPanel");
                    _lvlUpAwardPanel = new LvlUpAwardPanel(tr, awardData[index + 1], timeText);
                    _lvlUpAwardPanel.setPerent(_content);
                }
                _current = _lvlUpAwardPanel;
                //if (_payPanel == null)
                //{
                //    Transform tr = transform.FindChild("temp/payPanel");
                //    _payPanel = new PayPanel( tr , awardData[ index+1 ] , timeText );
                //    _payPanel.setPerent(_content);
                //}
                //_current = _payPanel;
            }
            /*兑换奖励*/
            if (index==3)
            {
                if (_ExchangePanel == null)
                {
                    Transform tr = transform.FindChild("temp/exchangePanel");
                    _ExchangePanel = new ExchangePanel(tr, awardData[index + 2], timeText);
                    _ExchangePanel.setPerent(_content);
                }
                _current = _ExchangePanel;
                //if (_todayRechargePanel == null)
                //{
                //    Transform tr = transform.FindChild("temp/toDayRechargePanel");
                //    _todayRechargePanel = new TodayRechargePanel( tr , awardData[ index+1 ] , timeText );
                //    _todayRechargePanel.setPerent(_content);
                //}
                //_current = _todayRechargePanel;
            }
            /*消费返利*/
            if ( index==4 )
            {
                if (_rechargePanel == null)
                {
                    Transform tr = transform.FindChild("temp/rechargePanel");
                    _rechargePanel = new RechargePanel(tr, awardData[index -2], timeText);
                    _rechargePanel.setPerent(_content);
                }
                _current = _rechargePanel;
                //if ( _ExchangePanel == null )
                //{
                //    Transform tr = transform.FindChild("temp/exchangePanel");
                //    _ExchangePanel = new ExchangePanel( tr , awardData[ index+1 ] , timeText );
                //    _ExchangePanel.setPerent( _content );
                //}
                //_current = _ExchangePanel;
            }
            /*超值团购*/
            if (index==5)
            {
                if (_todayRechargePanel == null)
                {
                    Transform tr = transform.FindChild("temp/toDayRechargePanel");
                    _todayRechargePanel = new TodayRechargePanel(tr, awardData[index -1], timeText);
                    _todayRechargePanel.setPerent(_content);
                }
                _current = _todayRechargePanel;
                //if(_goldfund==null)
                //{
                //    Transform tr = getTransformByPath("temp/goldfund");
                //    _goldfund = new GoldFund(getTransformByPath("temp/goldfund"));
                //}
                //_current = _goldfund;
                //RefreshFund_all();
                //getComponentByPath<ScrollRect>("temp/goldfund/down/Scrollview").verticalNormalizedPosition = 1;
            }
            /*钻石基金*/
            if (index==6)
            {
                if (_goldfund == null)
                {
                    Transform tr = getTransformByPath("temp/goldfund");
                    _goldfund = new GoldFund(getTransformByPath("temp/goldfund"));
                }
                _current = _goldfund;
                RefreshFund_all();
                getComponentByPath<ScrollRect>("temp/goldfund/down/Scrollview").verticalNormalizedPosition = 1;
                //if (_livefund == null)
                //{
                //    Transform tr = getTransformByPath("temp/livefund");
                //    _livefund = new LiveFund(getTransformByPath("temp/livefund"));
                //}
                //_current = _livefund;
                //RefreshFund_all();
                //getComponentByPath<ScrollRect>("temp/livefund/down/Scrollview").verticalNormalizedPosition = 1;
            }
            /*转生基金*/
            if (index==7)
            {
                if (_livefund == null)
                {
                    Transform tr = getTransformByPath("temp/livefund");
                    _livefund = new LiveFund(getTransformByPath("temp/livefund"));
                }
                _current = _livefund;
                RefreshFund_all();
                getComponentByPath<ScrollRect>("temp/livefund/down/Scrollview").verticalNormalizedPosition = 1;
                //if (_cmtfund == null)
                //{
                //    Transform tr = getTransformByPath("temp/cmtfund");
                //    _cmtfund = new CmtFund(getTransformByPath("temp/cmtfund"));
                //}
                //_current = _cmtfund;
                //RefreshFund_all();
                //getComponentByPath<ScrollRect>("temp/cmtfund/down/Scrollview").verticalNormalizedPosition = 1;
            }
            /*=战力基金*/
            if (index == 8)
            {
                if (_cmtfund == null)
                {
                    Transform tr = getTransformByPath("temp/cmtfund");
                    _cmtfund = new CmtFund(getTransformByPath("temp/cmtfund"));
                }
                _current = _cmtfund;
                RefreshFund_all();
                getComponentByPath<ScrollRect>("temp/cmtfund/down/Scrollview").verticalNormalizedPosition = 1;
                //if (_thisdaycost == null)
                //{
                //    Transform tr = getTransformByPath("temp/thisdaycost");
                //    _thisdaycost = new ThisdayCost(tr);

                //}
                //_current = _thisdaycost;
                //getComponentByPath<ScrollRect>("temp/cmtfund/down/Scrollview").verticalNormalizedPosition = 1;

            }

            _current.onShowed();
            _current.gameObject.SetActive(true);



        }
        void onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AWARDCENTER);
        }
        public void SetshowIconLight()
        {
            welfareProxy.getInstance().showIconLight(finishNum != 0);
        }


        #region//基金
        Dictionary<int, GameObject> fund_obj = new Dictionary<int, GameObject>();
        void creatrve_fund(GameObject image, Transform contain, int type)
        {
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            int num = 0;
            foreach (int i in dic.Keys)
            {
                if (dic[i].type != type)
                    continue;
                GameObject objclone = GameObject.Instantiate(image) as GameObject;
                objclone.SetActive(true);
                objclone.transform.SetParent(contain.transform, false);
                objclone.name = dic[i].id.ToString();
                fund_obj[i] = objclone;
                GameObject icon = objclone.transform.FindChild("icon").gameObject;
                icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(dic[i].file);

                Text needmoney = objclone.transform.FindChild("zuanshi/Text").GetComponent<Text>();
                needmoney.text = dic[i].zuanshi_num.ToString();

                Text needmoney1 = objclone.transform.FindChild("bangzuan/Text").GetComponent<Text>();
                needmoney1.text = dic[i].bangzuan_num.ToString();

                Text des = objclone.transform.FindChild("des").GetComponent<Text>();
                des.text = dic[i].des;

                GameObject exp = objclone.transform.FindChild("exp/exp").gameObject;

                int id = dic[i].id;
                new BaseButton(objclone.transform.FindChild("Button")).onClick = (GameObject go) => { buybtnOnclick(id); };

                Text exp_txt = objclone.transform.FindChild("exp/Text").GetComponent<Text>();
                exp_txt.text = "0/" + dic[i].need_paraml;

                objclone.transform.FindChild("Button").gameObject.SetActive(false);

                objclone.transform.FindChild("over").gameObject.SetActive(false);
                num++;
            }
            a3_runestone.commonScroview(contain.gameObject, num);
        }
        /*购买基金*/
        void btn_buyfund(int type)
        {
            a3_activeOnlineProxy.getInstance().SendProxy(3, fund_type: type);
        }
        /*购买后刷新*/
        public void Refreshbuy_fund(int type)
        {
            switch (type)
            {
                case 1:
                    zuanshi_btn.SetActive(false);
                    break;
                case 2:
                    zhuansheng_btn.SetActive(false);
                    break;
                case 3:
                    zhanli_btn.SetActive(false);
                    break;
            }
            RefreshFund_all();
        }
        /*整体刷新*/
        public void RefreshFund_all()
        {
            zuanshi_btn.SetActive(a3_ActiveOnlineModel.getInstance().zuanshi_fund ? false : true);
            zhuansheng_btn.SetActive(a3_ActiveOnlineModel.getInstance().zhuansheng_fund ? false : true);
            zhanli_btn.SetActive(a3_ActiveOnlineModel.getInstance().zhanli_fund ? false : true);
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            foreach (int i in dic.Keys)
            {
                Refreshfun(i);
            }
        }
        /*单个领取或购买刷新*/
        public void Refreshfun(int id)
        {

            //进度,exp，按钮状态
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            int nub = 0;
            switch (dic[id].type)
            {
                case 1:
                    nub = a3_ActiveOnlineModel.getInstance().zuanshi_fund ? a3_ActiveOnlineModel.getInstance().zuanshi_fundnow : 0;
                    break;
                case 2:
                    nub = a3_ActiveOnlineModel.getInstance().zhuansheng_fund ? a3_ActiveOnlineModel.getInstance().zhuansheng_fundnow : 0;
                    break;
                case 3:
                    nub = a3_ActiveOnlineModel.getInstance().zhanli_fund ? a3_ActiveOnlineModel.getInstance().zhanli_fundnow : 0;
                    break;
            }
            switch (dic[id].receive_type)
            {
                case 0:
                    break;
                case 1:
                case 2:
                    nub=dic[id].need_paraml;
                    break;

            }




            fund_obj[id].transform.FindChild("exp/Text").GetComponent<Text>().text = nub + "/" + dic[id].need_paraml;
            float x = nub / (float)dic[id].need_paraml;
            if (x >= 1)
                x = 1;
            fund_obj[id].transform.FindChild("exp/exp").GetComponent<Transform>().localScale = new Vector3(x, 1, 1);
            GameObject btn_obj = fund_obj[id].transform.FindChild("Button").gameObject;
            GameObject over_obj = fund_obj[id].transform.FindChild("over").gameObject;
            switch (dic[id].receive_type)
            {
                case 0:
                    btn_obj.SetActive(false);
                    over_obj.SetActive(false);
                    break;
                case 1:
                    btn_obj.SetActive(true);
                    over_obj.SetActive(false);
                    break;
                case 2:
                    btn_obj.SetActive(false);
                    over_obj.SetActive(true);
                    fund_obj[id].transform.SetAsLastSibling();
                    break;
            }
        }
        /*领取奖励*/
        void buybtnOnclick(int id)
        {
            //print("我发送的ID是：" + id);
            a3_activeOnlineProxy.getInstance().SendProxy(4, awd_id: id);

        }
        #endregion


        #region 当日充值奖励
        Dictionary<int, GameObject> dic_obj = new Dictionary<int, GameObject>();

        int ts_count = XMLMgr.instance.GetSXML("welfare.daily_charge").GetNodeList("charge").Count;

        //创建
        void creatrve_cost()
        {

           for(int i=0; i<ts_count;i++)
            {
                int xxxx = i;
                int  x = XMLMgr.instance.GetSXML("welfare.daily_charge").GetNode("charge", "id==" + (i + 1)).getInt("cumulate");

                List<SXML> lst= XMLMgr.instance.GetSXML("welfare.daily_charge").GetNode("charge", "id==" + (i + 1)).GetNodeList("RewardItem");
                GameObject clone = GameObject.Instantiate(thisday_cost_image) as GameObject;
                clone.SetActive(true);
                clone.transform.SetParent(thisday_cost_contain.transform,false);

                clone.transform.FindChild("des/num").GetComponent<Text>().text = x.ToString();

                foreach(  SXML s in lst)
                {
                    uint item_id = s.getUint("item_id");
                    int item_num = s.getInt("value");
                    GameObject go = IconImageMgr.getInstance().createA3ItemIcon(item_id,false, item_num, 1,true);
                    go.transform.SetParent(clone.transform.FindChild("Panel").transform, false);

                    new BaseButton(go.transform).onClick = (GameObject gos) =>
                      {
                          showtip(item_id);
                      };
                   



                }

                clone.transform.FindChild("Button").gameObject.SetActive(false);
                clone.transform.FindChild("over").gameObject.SetActive(false);



                dic_obj[i + 1] = clone;

            }
            //a3_runestone.commonScroview(thisday_cost_contain, 8);


        }



        public void RefreshInfo()
        {
            uint num = welfareProxy.todayTotal_recharge;
             foreach (int x in dic_obj.Keys)
             {
                int count = XMLMgr.instance.GetSXML("welfare.daily_charge").GetNode("charge", "id==" + x).getInt("cumulate");
                //按钮刷新
                if (welfareProxy.getInstance().dailyGift.Contains(x))
                {
                    dic_obj[x].transform.FindChild("Button").gameObject.SetActive(false);
                    dic_obj[x].transform.FindChild("over").gameObject.SetActive(true);

                    dic_obj[x].transform.SetAsLastSibling();
                }
                else
                {
                   
                    if (num>= count)
                    {
                        dic_obj[x].transform.FindChild("Button").gameObject.SetActive(true);
                        dic_obj[x].transform.FindChild("Button").GetComponent<Button>().interactable = true;
                        dic_obj[x].transform.FindChild("over").gameObject.SetActive(false);
                        dic_obj[x].transform.FindChild("Button/Text").GetComponent<Text>().text = ContMgr.getCont("off_line_exp_1");
                        new BaseButton(dic_obj[x].transform.FindChild("Button")).onClick = (GameObject go) =>
                        {
                            uint id = (uint)x;
                            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.dayRechargeAward, id);
                        };
                    }
                    else
                    {
                        dic_obj[x].transform.FindChild("Button").gameObject.SetActive(true);
                        new BaseButton(dic_obj[x].transform.FindChild("Button")).onClick = (GameObject go) =>
                          {
                              InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                              InterfaceMgr.getInstance().close(InterfaceMgr.A3_AWARDCENTER);
                          };
                        dic_obj[x].transform.FindChild("Button").GetComponent<Button>().interactable = true;
                        dic_obj[x].transform.FindChild("Button/Text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_1");
                        dic_obj[x].transform.FindChild("over").gameObject.SetActive(false);
                    }
                }

                //进度条刷新
                if(num>= count)
                {
                    
                    dic_obj[x].transform.FindChild("exp/exp").GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                    dic_obj[x].transform.FindChild("exp/Text").GetComponent<Text>().text = count + "/" + count;
                }
                else
                {
                    float xx = num / (float)count;
                    dic_obj[x].transform.FindChild("exp/exp").GetComponent<Transform>().localScale = new Vector3(xx, 1, 1);
                    dic_obj[x].transform.FindChild("exp/Text").GetComponent<Text>().text = num + "/" + count;
                }


             }
        }

        #endregion


    }

    public class ThisdayCost : BaseAwardCenter
    {


        public ThisdayCost(Transform tsm) : base(tsm)
        {
        }
    }
    public class GoldFund : BaseAwardCenter
    {
        public GoldFund(Transform tsm):base(tsm)
        {
                
        }


    }
    public class LiveFund : BaseAwardCenter
    {
        public LiveFund(Transform tsm):base(tsm)
        {

        }
    }
    public class CmtFund : BaseAwardCenter
    {
        public CmtFund(Transform tsm):base(tsm)
        {

        }
    }

    public class LvlUpAwardPanel : BaseAwardCenter//充值有奖面板
    {
        Dictionary<uint, awardCenterItem4zhuan> lvlUpAwardsDic;

        //string strDesc = "角色等级到达{0}转";
       // string strDesc = ContMgr.getCont("a3_awardCenter0");
        Transform root;
        Transform itemsParent;
        Text _timeText;
        List<Variant> action;
        private Variant data;
        Dictionary<string,GameObject> itemGoLst = new Dictionary<string, GameObject>();
        public LvlUpAwardPanel(Transform trans, Variant v ,Text timeText) :base(trans)
        {
            root = trans;
            lvlUpAwardsDic = new Dictionary<uint, awardCenterItem4zhuan>();
            itemsParent= root.FindChild("awardItems/content");
            //List<WelfareModel.itemWelfareData> iwfdList = WelfareModel.getInstance().getLevelReward();
            //foreach (WelfareModel.itemWelfareData iwd in iwfdList)
            //{

            //}

            data = v;
            _timeText = timeText;
            action = data["action"]["list"]._arr;


            itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140 * ( action.Count));//lvlUpAwardsDic            
            //welfareProxy.getInstance().addEventListener(welfareProxy.UPLEVELAWARD, onUpLevelAward);
        }


        override public void onShowed()
        {
            //if (welfareProxy.getInstance().dengjijiangli == null) return;
            //List<uint> checkedList =new List<uint>();
            //checkedList.Clear();
            //foreach (Variant variantList in  welfareProxy.getInstance().dengjijiangli)
            //{
            //    uint id = (uint)variantList;
            //    checkedList.Add(id);
            //    if (lvlUpAwardsDic.ContainsKey(id))
            //    {
            //        lvlUpAwardsDic[id].transform.FindChild("state/imgChecked").gameObject.SetActive(true);
            //        lvlUpAwardsDic[id].transform.FindChild("state/btnGet").gameObject.SetActive(false);
            //        lvlUpAwardsDic[id].transform.SetAsLastSibling();
            //    }
            //}
            //foreach ( KeyValuePair<uint , awardCenterItem4zhuan> item in lvlUpAwardsDic )
            //{
            //    if ( checkedList.Contains( item.Key ) ) continue;
            //    if ( item.Value.m_iwd.zhuan > PlayerModel.getInstance().up_lvl )//无法领取的
            //    {
            //        lvlUpAwardsDic[ item.Key ].CanNotCheck();
            //    }
            //    else
            //    {
            //        lvlUpAwardsDic[ item.Key ].CanCheck();
            //    }
            //}

            string name = data["name"];
            int begin = data["begin"];
            int end = data["end"];
          
            _timeText.text =  ContMgr.getCont("ButtonText3")+"： <color=#1BD4EF>" +Globle.getStrTime( begin , true , true ) + "</color> 至 <color=#1BD4EF>" +Globle.getStrTime( end , true , true )+"</color>";

            Variant Selfdata = null;
            if ( a3_awardCenter.roleData.ContainsKey( data[ "tp" ] ) )
            {
                Selfdata = a3_awardCenter.roleData[ data[ "tp" ] ];
            }
            else
            {
                Selfdata = new Variant();
                Selfdata[ "lists" ] = new Variant();
                Selfdata[ "num" ] = 0;
            }
            Dictionary<int,Variant> Selfdata_Item = new Dictionary<int, Variant>();
            foreach ( Variant item in Selfdata[ "lists" ]._arr )
            {
                Selfdata_Item[ item[ "id" ]._int ] = item;
            }
            foreach ( Variant item in action )
            {

                //WelfareModel.itemWelfareData iwfdTemp = iwd;
                ////iwfdTemp.desc = strDesc;
                //a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                //GameObject go =IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);

                if ( !itemGoLst.ContainsKey( item[ "id" ]._str ) )
                {
                    itemGoLst[ item[ "id" ]._str ] = IconImageMgr.getInstance().creatItemAwardCenterIcon( null );
                }

                GameObject _go = itemGoLst[ item[ "id" ]._str ];
                _go.name = "itemWelfare";
                _go.transform.SetParent( itemsParent );
                _go.transform.localScale = Vector3.one;
                GameObject go = _go.transform.FindChild("Common").gameObject;

                go.transform.FindChild( "Text_One" ).gameObject.SetActive( true );
                go.transform.FindChild( "Text_One/txtInfor" ).GetComponent<Text>().text = ContMgr.getCont("txtInfor");

                Text conditionText = go.transform.FindChild("Text_One/txtInforNum").GetComponent<Text>();
                conditionText.text = item[ "param" ];

                GameObject progressGo = go.transform.FindChild("Progress/Image").gameObject;
                int _num = int.Parse( conditionText.text);
                int num = Selfdata["num"]._int;
                progressGo.GetComponent<Image>().fillAmount = ( float ) num/( float ) _num;
                go.transform.FindChild("Progress/Text").gameObject.SetActive(true);
                go.transform.FindChild("Progress/Text").GetComponent<Text>().text = num + "/" + _num;

                List<Variant> rewardValue = item["RewardValue"]._arr; //货币奖励
                for ( int i = 0 ; i < rewardValue.Count ; i++ )
                {
                    string type = rewardValue[i]["type"];
                    GameObject iconGo =  IconImageMgr.getInstance().createMoneyIcon(type);
                    GameObject icon_gold = go.transform.FindChild("icon_Item" + (i+1)).gameObject;
                    go.transform.FindChild( "icon_Item" + ( i+1 )+ "/icon" ).gameObject.SetActive( false );
                    go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Border" ).gameObject.SetActive( false ); ;
                    iconGo.transform.SetParent( icon_gold.transform );
                    iconGo.transform.localPosition = new Vector3( 0 , 0 , 0 );
                    iconGo.transform.localScale = new Vector3( 1 , 1 , 1 );

                    //new BaseButton( iconGo.transform ).onClick=( GameObject oo ) =>
                    //{

                    //    if ( a3_awardCenter.instan ) 
                    //    {
                    //        a3_awardCenter.instan.showtip(uint.Parse( type ) );
                    //    }
                     //};

                }

                List<Variant> rewardItem = item["RewardItem"]._arr;//道具奖励
                List<string> itemNameNumLst = new List<string>();
                for ( int i = rewardValue.Count ; i < rewardItem.Count+rewardValue.Count ; i++ )
                {
                    uint id = rewardItem[i-rewardValue.Count]["item_id"];
                    uint valueNum =rewardItem[i-rewardValue.Count].ContainsKey("value")?rewardItem[i-rewardValue.Count]["value"]._uint:0;
                    if ( valueNum != 0 )
                    {
                        go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Num" ).GetComponent<Text>().text = valueNum + "";
                    }
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(id);
                    GameObject icon_Go_Borderfile =  go.transform.FindChild("icon_Item" + (i+1) + "/Border" ).gameObject;
                    icon_Go_Borderfile.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.borderfile );
                    GameObject icon_Go = go.transform.FindChild("icon_Item" + (i+1) + "/icon").gameObject;
                    icon_Go.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.file );
                    itemNameNumLst.Add( itemData.item_name +"*" + valueNum );
                    new BaseButton( icon_Go.transform ).onClick=( GameObject oo ) =>
                    {

                        if ( a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip( id );
                        }
                     
                    };

                }

                if ( rewardItem.Count+rewardValue.Count < 4 )
                {
                    for ( int i = rewardItem.Count+rewardValue.Count+1 ; i <= 4 ; i++ )
                    {
                        go.transform.FindChild( "icon_Item" + i ).gameObject.SetActive( false );
                    }
                }


                Transform  btnGet = go.transform.FindChild( "state");
                if ( a3_awardCenter.yellowSprite == null )
                {
                    a3_awardCenter.yellowSprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.pressedSprite;
                    a3_awardCenter.graySprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.pressedSprite;
                }
                if ( num>=_num )
                {
                    if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 1 )
                    {
                        btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                        SpriteState ss = new SpriteState();
                        ss.pressedSprite = a3_awardCenter.yellowSprite;
                        btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;
                        btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter5" );
                        
                    }
                    else if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 2 )
                    {
                        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                    }
                }
                else
                {
                    //SpriteState ss = new SpriteState();
                    //ss.pressedSprite = a3_awardCenter.graySprite;
                    //btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;
                    //btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.graySprite;
                    btnGet.FindChild("btnGet/Text").GetComponent<Text>().text = "立即充值";/* ContMgr.getCont( "a3_awardCenter4" )*/;
                }

                new BaseButton( btnGet.FindChild( "btnGet" ) ).onClick=( GameObject oo ) =>
                {
                    if ( num>=_num )
                    {
                        Variant Itemdata = new Variant();
                        Itemdata[ "act_type" ] = data[ "tp" ]._uint;
                        Itemdata[ "award_id" ] = item[ "id" ]._uint;

                        A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.EVENT_GETAWARD , Itemdata );

                        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                        a3_awardCenter.finishNum--;
                        a3_awardCenter.instan.SetshowIconLight();
                        if ( itemNameNumLst.Count!=0 )
                        {
                            for ( int i = 0 ; i < itemNameNumLst.Count ; i++ )
                            {
                                flytxt.instance.fly( itemNameNumLst[i]);
                            }
                        }
                    }
                    else
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AWARDCENTER);
                        //flytxt.instance.fly( ContMgr.getCont( "a3_award_unable" ) );
                    }

                };


                //awardCenterItem4zhuan aci = new awardCenterItem4zhuan(go.transform, iwfdTemp);
                //uint id = iwd.itemId;

                //lvlUpAwardsDic.Add( iwd.id , aci );
            }


        }
        void onUpLevelAward(GameEvent e)
        {
            uint giftId = e.data["gift_id"];
            if (lvlUpAwardsDic.ContainsKey(giftId))
            {
                lvlUpAwardsDic[giftId].Checked();
            }
        }
        
    }
    
    public class RechargePanel:BaseAwardCenter//消费返利面板
    {
        Transform root;
        Transform itemsParent;
        Dictionary<uint, awardCenterItem4Recharge> rechargeAwardsDic;
        //string strDesc = "累积充值{0}钻石可获得价值{1}的大礼包一个";
        // string strDesc = ContMgr.getCont("a3_awardCenter1");

        Text _timeText;
        List<Variant> action;
        Variant data ;

        Dictionary<string,GameObject> itemGoLst = new Dictionary<string, GameObject>();
        public RechargePanel(Transform trans , Variant v , Text timeText ) :base(trans)
        {
            //钻石 PlayerModel.getInstance().money;
            root = trans;
            rechargeAwardsDic = new Dictionary<uint, awardCenterItem4Recharge>();
            itemsParent = root.FindChild("awardItems/content");
            //List<WelfareModel.itemWelfareData> cumulateRechargeAwardList = WelfareModel.getInstance().getCumulateRechargeAward();
            //foreach (WelfareModel.itemWelfareData iwd in cumulateRechargeAwardList)
            //{
            //    WelfareModel.itemWelfareData iwfdTemp = iwd;
            //    //iwfdTemp.desc = strDesc;
            //    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
            //    GameObject go = IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
            //    go.name = "itemWelfare";
            //    go.transform.SetParent(itemsParent);
            //    go.transform.localScale = Vector3.one;
            //    uint id = iwd.itemId;
            //    new BaseButton(go.transform.FindChild("icon")).onClick = (GameObject oo) =>
            //    {
            //        if (a3_awardCenter.instan)
            //        {
            //            a3_awardCenter.instan.showtip(id);
            //        }
            //    };
            //    awardCenterItem4Recharge aci = new awardCenterItem4Recharge(go.transform, iwfdTemp);
            //    rechargeAwardsDic.Add(iwd.id, aci);
            //}
            //itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100 * (rechargeAwardsDic.Count+4));
            //welfareProxy.getInstance().addEventListener(welfareProxy.ACCUMULATERECHARGE, onAccumulateRecharge);
            data = v;
            _timeText = timeText;
            action = data[ "action" ][ "list" ]._arr;


            itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , 140 * ( action.Count) );
            //welfareProxy.getInstance().addEventListener( welfareProxy.ACCUMULATERECHARGE , onAccumulateRecharge );


        }

        override public void onShowed()
        {
            //if (welfareProxy.getInstance().leijichongzhi == null) return;
            //List<uint> checkedList = new List<uint>();
            //checkedList.Clear();
            //foreach (Variant variantList in welfareProxy.getInstance().leijichongzhi)
            //{
            //    uint id = (uint)variantList;
            //    checkedList.Add(id);
            //    if (rechargeAwardsDic.ContainsKey(id))
            //    {
            //        rechargeAwardsDic[id].transform.FindChild("state/imgChecked").gameObject.SetActive(true);
            //        rechargeAwardsDic[id].transform.FindChild("state/btnGet").gameObject.SetActive(false);
            //        rechargeAwardsDic[id].transform.SetAsLastSibling();
            //    }
            //}
            //foreach (KeyValuePair<uint, awardCenterItem4Recharge> item in rechargeAwardsDic)
            //{
            //    if (checkedList.Contains(item.Key)) continue;
            //    if (item.Value.m_iwd.cumulateNum > welfareProxy.totalRecharge)//无法领取的
            //    {
            //        rechargeAwardsDic[item.Key].CanNotCheck();
            //    }
            //    else
            //    {
            //        rechargeAwardsDic[item.Key].CanCheck();
            //    }
            //}


            string name = data["name"];
            int begin = data["begin"];
            int end = data["end"];
            Variant Selfdata = null;
            _timeText.text =  ContMgr.getCont("ButtonText3")+"： <color=#1BD4EF>" + Globle.getStrTime( begin , true , true ) + "</color> 至 <color=#1BD4EF>" +Globle.getStrTime( end , true , true )+"</color>";

            if ( a3_awardCenter.roleData.ContainsKey( data[ "tp" ] ) )
            {
                Selfdata = a3_awardCenter.roleData[ data[ "tp" ] ];
            }
            else
            {
                Selfdata = new Variant();
                Selfdata[ "lists" ] = new Variant();
                Selfdata[ "num" ] = 0;
            }

            Dictionary<int,Variant> Selfdata_Item = new Dictionary<int, Variant>();
            foreach ( Variant item in Selfdata[ "lists" ]._arr )
            {
                Selfdata_Item[ item[ "id" ]._int ] = item;
            }

            foreach ( Variant item in action )
            {

                //WelfareModel.itemWelfareData iwfdTemp = iwd;
                ////iwfdTemp.desc = strDesc;
                //a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                //GameObject go =IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
                if ( !itemGoLst.ContainsKey( item[ "id" ]._str ) )
                {
                    itemGoLst[ item[ "id" ]._str ] = IconImageMgr.getInstance().creatItemAwardCenterIcon( null );
                }
                GameObject _go = itemGoLst[ item[ "id" ]._str ];
                _go.name = "itemWelfare";
                _go.transform.SetParent( itemsParent );
                _go.transform.localScale = Vector3.one;
                GameObject go = _go.transform.FindChild("Common").gameObject;

                go.transform.FindChild( "Text_One" ).gameObject.SetActive( true );
                go.transform.FindChild( "Text_One/txtInfor" ).GetComponent<Text>().text = ContMgr.getCont("txtInfor1");

                Text conditionText = go.transform.FindChild("Text_One/txtInforNum").GetComponent<Text>();
                conditionText.text = item[ "param" ];

                GameObject progressGo = go.transform.FindChild("Progress/Image").gameObject;
                int _num = int.Parse( conditionText.text);
                int num = Selfdata["num"]._int;
                progressGo.GetComponent<Image>().fillAmount = ( float ) num/( float ) _num;
                go.transform.FindChild("Progress/Text").gameObject.SetActive(true);
                go.transform.FindChild("Progress/Text").GetComponent<Text>().text = num + "/" + _num;

                List<Variant> rewardValue = item["RewardValue"]._arr; //货币奖励
                for ( int i = 0 ; i < rewardValue.Count ; i++ )
                {
                    string type = rewardValue[i]["type"];
                    GameObject iconGo =  IconImageMgr.getInstance().createMoneyIcon(type);
                    GameObject icon_gold = go.transform.FindChild("icon_Item" + (i+1)).gameObject;
                    go.transform.FindChild( "icon_Item" + ( i+1 )+ "/icon" ).gameObject.SetActive( false );
                    go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Border" ).gameObject.SetActive( false ); ;
                    iconGo.transform.SetParent( icon_gold.transform );
                    iconGo.transform.localPosition = new Vector3( 0 , 0 , 0 );
                    iconGo.transform.localScale = new Vector3( 1 , 1 , 1 );

                    //new BaseButton( iconGo.transform ).onClick=( GameObject oo ) =>
                    //{

                    //    if ( a3_awardCenter.instan )
                    //    {
                    //        a3_awardCenter.instan.showtip(uint.Parse( type ) );
                    //    }
                    //};

                }

                List<Variant> rewardItem = item["RewardItem"]._arr;//道具奖励
                List<string> itemNameNumLst = new List<string>();
                for ( int i = rewardValue.Count ; i < rewardItem.Count+rewardValue.Count ; i++ )
                {
                    uint id = rewardItem[i-rewardValue.Count]["item_id"];
                    uint valueNum =rewardItem[i-rewardValue.Count].ContainsKey("value")?rewardItem[i-rewardValue.Count]["value"]._uint:0;
                    if ( valueNum != 0 )
                    {
                        go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Num" ).GetComponent<Text>().text = valueNum + "";
                    }
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(id);
                    GameObject icon_Go_Borderfile =  go.transform.FindChild("icon_Item" + (i+1) + "/Border" ).gameObject;
                    icon_Go_Borderfile.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.borderfile );
                    GameObject icon_Go = go.transform.FindChild("icon_Item" + (i+1) + "/icon").gameObject;
                    icon_Go.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.file );
                    itemNameNumLst.Add( itemData.item_name +"*" + valueNum );
                    new BaseButton( icon_Go.transform ).onClick=( GameObject oo ) =>
                    {

                        if ( a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip( id );
                        }
                    };

                }

                if ( rewardItem.Count+rewardValue.Count < 4 )
                {
                    for ( int i = rewardItem.Count+rewardValue.Count+1 ; i <= 4 ; i++ )
                    {
                        go.transform.FindChild( "icon_Item" + i ).gameObject.SetActive( false );
                    }
                }


                Transform  btnGet = go.transform.FindChild( "state");
                if ( a3_awardCenter.yellowSprite == null )
                {
                    a3_awardCenter.yellowSprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.pressedSprite;
                    a3_awardCenter.graySprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.disabledSprite;
                }
                if ( num>=_num )
                {
                    if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 1 )
                    {
                        btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                        SpriteState ss = new SpriteState();
                        ss.pressedSprite = a3_awardCenter.yellowSprite;
                        btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;

                        btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter5" );
                    }
                    else if( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 2 )
                    {
                        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                    }
                    
                }
                else
                {
                  //  SpriteState ss = new SpriteState();
                  //  ss.pressedSprite = a3_awardCenter.graySprite;
                   // btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;
                    //btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.graySprite;

                    btnGet.FindChild("btnGet/Text").GetComponent<Text>().text = "立即充值";/* ContMgr.getCont( "a3_awardCenter4" )*/;;

                }

                new BaseButton( btnGet.FindChild( "btnGet" ) ).onClick=( GameObject oo ) =>
                {
                    if ( num>=_num )
                    {
                        Variant Itemdata = new Variant();
                        Itemdata[ "act_type" ] = data[ "tp" ]._uint;
                        Itemdata[ "award_id" ] = item[ "id" ]._uint;

                        A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.EVENT_GETAWARD , Itemdata );

                        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                        a3_awardCenter.finishNum--;
                        a3_awardCenter.instan?.SetshowIconLight();
                        if ( itemNameNumLst.Count!=0 )
                        {
                            for ( int i = 0 ; i < itemNameNumLst.Count ; i++ )
                            {
                                flytxt.instance.fly( itemNameNumLst[ i ] );
                            }
                        }
                    }
                    else
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AWARDCENTER);
                        //flytxt.instance.fly( ContMgr.getCont( "a3_award_unable" ) );
                    }

                };



            }

        }

        override public void onClose()
        {
       
        }
        void onAccumulateRecharge(GameEvent e)
        {
            uint giftId = e.data["gift_id"];
            if (rechargeAwardsDic.ContainsKey(giftId))
            {
                rechargeAwardsDic[giftId].Checked();
            }
        }
    }
    public class PayPanel:BaseAwardCenter//单笔充值面板
    {
        Transform root;
        Transform itemsParent;
        Dictionary<uint, awardCenterItem4Consumption> consumptionAwardsDic;
        //string strDesc = "累积消费{0}钻石可获得{1}一个";
        //string strDesc =ContMgr.getCont("a3_awardCenter2");

        Text _timeText;
        List<Variant> action;
        Variant data ;
        Dictionary<string,GameObject> itemGoLst = new Dictionary<string, GameObject>();
        public PayPanel( Transform trans , Variant v , Text timeText )
            : base(trans)
        {
            root = trans;
            consumptionAwardsDic = new Dictionary<uint, awardCenterItem4Consumption>();
            itemsParent = root.FindChild("awardItems/content");
            //List<WelfareModel.itemWelfareData> cumulateConsumptionAwardList = WelfareModel.getInstance().getCumulateConsumption();
            //foreach (WelfareModel.itemWelfareData iwd in cumulateConsumptionAwardList)
            //{
            //    WelfareModel.itemWelfareData iwfdTemp = iwd;
            //    //iwfdTemp.desc = strDesc;
            //    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
            //    iwfdTemp.awardName = itemData.item_name;
            //    GameObject go = IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
            //    go.name = "itemWelfare";
            //    go.transform.SetParent(itemsParent);
            //    go.transform.localScale = Vector3.one;
            //    uint id = iwd.itemId;
            //    new BaseButton(go.transform.FindChild("icon")).onClick = (GameObject oo) =>
            //    {
            //        if (a3_awardCenter.instan)
            //        {
            //            a3_awardCenter.instan.showtip(id);
            //        }
            //    };
            //    awardCenterItem4Consumption aci = new awardCenterItem4Consumption(go.transform, iwfdTemp);
            //    consumptionAwardsDic.Add(iwd.id, aci);
            //}

            data = v;
            _timeText = timeText;
            action = data[ "action" ][ "list" ]._arr;



            itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140 * ( action.Count));
            //welfareProxy.getInstance().addEventListener(welfareProxy.ACCUMULATECONSUMPTION, onAccumulateConsumption);

        }

        override public void onShowed()
        {
            //if (welfareProxy.getInstance().leijixiaofei == null) return;
            //List<uint> checkedList = new List<uint>();
            //checkedList.Clear();
            //foreach (Variant variantList in welfareProxy.getInstance().leijixiaofei)
            //{
            //    uint id = (uint)variantList;
            //    checkedList.Add(id);
            //    if (consumptionAwardsDic.ContainsKey(id))
            //    {
            //        consumptionAwardsDic[id].transform.FindChild("state/imgChecked").gameObject.SetActive(true);
            //        consumptionAwardsDic[id].transform.FindChild("state/btnGet").gameObject.SetActive(false);
            //        consumptionAwardsDic[id].transform.SetAsLastSibling();
            //    }
            //}
            //foreach (KeyValuePair<uint, awardCenterItem4Consumption> item in consumptionAwardsDic)
            //{
            //    if (checkedList.Contains(item.Key)) continue;
            //    if (item.Value.m_iwd.cumulateNum > welfareProxy.totalXiaofei)//无法领取的
            //    {
            //        consumptionAwardsDic[item.Key].CanNotCheck();
            //    }
            //    else
            //    {
            //        consumptionAwardsDic[item.Key].CanCheck();
            //    }
            //}

            string name = data["name"];
            int begin = data["begin"];
            int end = data["end"];

            _timeText.text =  ContMgr.getCont("ButtonText3")+"： <color=#1BD4EF>" + Globle.getStrTime( begin , true , true ) + "</color> 至 <color=#1BD4EF>" +Globle.getStrTime( end , true , true )+"</color>";

            Variant Selfdata = null;
            if ( a3_awardCenter.roleData.ContainsKey( data[ "tp" ] ) )
            {
                Selfdata = a3_awardCenter.roleData[ data[ "tp" ] ];
            }
            else
            {
                Selfdata = new Variant();
                Selfdata[ "lists" ] = new Variant();
                Selfdata[ "num" ] = 0;
            }
            Dictionary<int,Variant> Selfdata_Item = new Dictionary<int, Variant>();
            foreach ( Variant item in Selfdata[ "lists" ]._arr )
            {
                Selfdata_Item[ item[ "id" ]._int ] = item;
            }

            foreach ( Variant item in action )
            {

                //WelfareModel.itemWelfareData iwfdTemp = iwd;
                ////iwfdTemp.desc = strDesc;
                //a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                //GameObject go =IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
                if ( !itemGoLst.ContainsKey( item[ "id" ]._str ) )
                {
                    itemGoLst[ item[ "id" ]._str ] = IconImageMgr.getInstance().creatItemAwardCenterIcon( null );
                }
                GameObject _go = itemGoLst[ item[ "id" ]._str ];
                _go.name = "itemWelfare";
                _go.transform.SetParent( itemsParent );
                _go.transform.localScale = Vector3.one;
                GameObject go = _go.transform.FindChild("Common").gameObject;

                go.transform.FindChild( "Text_Two" ).gameObject.SetActive( true );
                go.transform.FindChild( "Text_Two/txtInfor" ).GetComponent<Text>().text = ContMgr.getCont("txtInfor2");
                go.transform.FindChild( "Text_One" ).gameObject.SetActive( false );
                
                Text conditionText = go.transform.FindChild("Text_Two/txtInforNum").GetComponent<Text>();
                conditionText.text = item[ "param" ];

                //GameObject progressGo = go.transform.FindChild("Progress/Image").gameObject;
                GameObject progressGo = go.transform.FindChild("Progress").gameObject;
                progressGo.SetActive(false);
                int _num = int.Parse( conditionText.text);
                int num = Selfdata["num"]._int;
                //progressGo.GetComponent<Image>().fillAmount = ( float ) num/( float ) _num;

                List<Variant> rewardValue = item["RewardValue"]._arr; //货币奖励
                for ( int i = 0 ; i < rewardValue.Count ; i++ )
                {
                    string type = rewardValue[i]["type"];
                    GameObject iconGo =  IconImageMgr.getInstance().createMoneyIcon(type);
                    GameObject icon_gold = go.transform.FindChild("icon_Item" + (i+1)).gameObject;
                    go.transform.FindChild( "icon_Item" + ( i+1 )+ "/icon" ).gameObject.SetActive( false );
                    go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Border" ).gameObject.SetActive( false ); ;
                    iconGo.transform.SetParent( icon_gold.transform );
                    iconGo.transform.localPosition = new Vector3( 0 , 0 , 0 );
                    iconGo.transform.localScale = new Vector3( 1 , 1 , 1 );

                    //new BaseButton( iconGo.transform ).onClick=( GameObject oo ) =>
                    //{

                    //    if ( a3_awardCenter.instan )
                    //    {
                    //        a3_awardCenter.instan.showtip(uint.Parse( type ) );
                    //    }
                    //};

                }

                List<Variant> rewardItem = item["RewardItem"]._arr;//道具奖励
                List<string> itemNameNumLst = new List<string>();
                for ( int i = rewardValue.Count ; i < rewardItem.Count+rewardValue.Count ; i++ )
                {
                    uint id = rewardItem[i-rewardValue.Count]["item_id"];
                    uint valueNum =rewardItem[i-rewardValue.Count].ContainsKey("value")?rewardItem[i-rewardValue.Count]["value"]._uint:0;
                    if ( valueNum != 0 )
                    {
                        go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Num" ).GetComponent<Text>().text = valueNum + "";
                    }
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(id);
                    GameObject icon_Go_Borderfile =  go.transform.FindChild("icon_Item" + (i+1) + "/Border" ).gameObject;
                    icon_Go_Borderfile.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.borderfile );
                    GameObject icon_Go = go.transform.FindChild("icon_Item" + (i+1) + "/icon").gameObject;
                    icon_Go.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.file );
                    itemNameNumLst.Add( itemData.item_name +"*" + valueNum );

                    new BaseButton( icon_Go.transform ).onClick=( GameObject oo ) =>
                    {

                        if ( a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip( id );
                        }
                    };

                }

                if ( rewardItem.Count+rewardValue.Count < 4 )
                {
                    for ( int i = rewardItem.Count+rewardValue.Count+1 ; i <= 4 ; i++ )
                    {
                        go.transform.FindChild( "icon_Item" + i ).gameObject.SetActive( false );
                    }
                }


                Transform  btnGet = go.transform.FindChild( "state");
                if ( a3_awardCenter.yellowSprite == null )
                {
                    a3_awardCenter.yellowSprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.pressedSprite;
                    a3_awardCenter.graySprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.disabledSprite;
                }
                if ( num>=_num )
                {
                    if (Selfdata_Item.ContainsKey(item["id"]._int))
                    {


                        if (Selfdata_Item[item["id"]._int]["state"]._int == 1)
                        {
                            btnGet.FindChild("btnGet").GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                            SpriteState ss = new SpriteState();
                            ss.pressedSprite = a3_awardCenter.yellowSprite;
                            btnGet.FindChild("btnGet").GetComponent<Button>().spriteState = ss;

                            btnGet.FindChild("btnGet/Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
                        }
                        else if (Selfdata_Item[item["id"]._int]["state"]._int == 2)
                        {
                            btnGet.FindChild("btnGet").gameObject.SetActive(false);//领取按钮隐藏
                            btnGet.FindChild("imgChecked").gameObject.SetActive(true);
                        }
                    }

                }
                else
                {
                   // SpriteState ss = new SpriteState();
                   // ss.pressedSprite = a3_awardCenter.graySprite;
                   // btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;
                    //btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.graySprite;

                    btnGet.FindChild("btnGet/Text").GetComponent<Text>().text = "立即充值";/* ContMgr.getCont( "a3_awardCenter4" );*/

                }

                new BaseButton( btnGet.FindChild( "btnGet" ) ).onClick=( GameObject oo ) =>
                {
                    if ( num>=_num )
                    {
                        Variant Itemdata = new Variant();
                        Itemdata[ "act_type" ] = data[ "tp" ]._uint;
                        Itemdata[ "award_id" ] = item[ "id" ]._uint;

                        A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.EVENT_GETAWARD , Itemdata );

                        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                        a3_awardCenter.finishNum--;
                        a3_awardCenter.instan.SetshowIconLight();
                        if ( itemNameNumLst.Count!=0 )
                        {
                            for ( int i = 0 ; i < itemNameNumLst.Count ; i++ )
                            {
                                flytxt.instance.fly( itemNameNumLst[ i ] );
                            }
                        }
                    }
                    else
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AWARDCENTER);
                        //flytxt.instance.fly( ContMgr.getCont( "a3_award_unable" ) );
                    }

                };
            }

        }

        override public void onClose()
        {
         
        }
        void onAccumulateConsumption(GameEvent e)
        {
            uint giftId = e.data["gift_id"];
            if (consumptionAwardsDic.ContainsKey(giftId))
            {
                consumptionAwardsDic[giftId].Checked();
            }
        }
    }

    public class ExchangePanel : BaseAwardCenter//兑换奖励
    {
        Transform root;
        Transform itemsParent;
        Dictionary<uint, awardCenterItem4Consumption> consumptionAwardsDic;
        //string strDesc = "累积消费{0}钻石可获得{1}一个";
        //string strDesc =ContMgr.getCont("a3_awardCenter2");

        Text _timeText;
        List<Variant> action;
        Variant data ;
        Dictionary<string,GameObject> itemGoLst = new Dictionary<string, GameObject>();
        public ExchangePanel( Transform trans , Variant v , Text timeText )
            : base( trans )
        {
            root = trans;
            consumptionAwardsDic = new Dictionary<uint , awardCenterItem4Consumption>();
            itemsParent = root.FindChild( "awardItems/content" );
            //List<WelfareModel.itemWelfareData> cumulateConsumptionAwardList = WelfareModel.getInstance().getCumulateConsumption();
            //foreach (WelfareModel.itemWelfareData iwd in cumulateConsumptionAwardList)
            //{
            //    WelfareModel.itemWelfareData iwfdTemp = iwd;
            //    //iwfdTemp.desc = strDesc;
            //    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
            //    iwfdTemp.awardName = itemData.item_name;
            //    GameObject go = IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
            //    go.name = "itemWelfare";
            //    go.transform.SetParent(itemsParent);
            //    go.transform.localScale = Vector3.one;
            //    uint id = iwd.itemId;
            //    new BaseButton(go.transform.FindChild("icon")).onClick = (GameObject oo) =>
            //    {
            //        if (a3_awardCenter.instan)
            //        {
            //            a3_awardCenter.instan.showtip(id);
            //        }
            //    };
            //    awardCenterItem4Consumption aci = new awardCenterItem4Consumption(go.transform, iwfdTemp);
            //    consumptionAwardsDic.Add(iwd.id, aci);
            //}

            data = v;
            _timeText = timeText;
            action = data[ "action" ][ "list" ]._arr;



            itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , 140 * ( action.Count ) );
            //welfareProxy.getInstance().addEventListener( welfareProxy.ACCUMULATECONSUMPTION , onAccumulateConsumption );

        }

        override public void onShowed()
        {
            //if (welfareProxy.getInstance().leijixiaofei == null) return;
            //List<uint> checkedList = new List<uint>();
            //checkedList.Clear();
            //foreach (Variant variantList in welfareProxy.getInstance().leijixiaofei)
            //{
            //    uint id = (uint)variantList;
            //    checkedList.Add(id);
            //    if (consumptionAwardsDic.ContainsKey(id))
            //    {
            //        consumptionAwardsDic[id].transform.FindChild("state/imgChecked").gameObject.SetActive(true);
            //        consumptionAwardsDic[id].transform.FindChild("state/btnGet").gameObject.SetActive(false);
            //        consumptionAwardsDic[id].transform.SetAsLastSibling();
            //    }
            //}
            //foreach (KeyValuePair<uint, awardCenterItem4Consumption> item in consumptionAwardsDic)
            //{
            //    if (checkedList.Contains(item.Key)) continue;
            //    if (item.Value.m_iwd.cumulateNum > welfareProxy.totalXiaofei)//无法领取的
            //    {
            //        consumptionAwardsDic[item.Key].CanNotCheck();
            //    }
            //    else
            //    {
            //        consumptionAwardsDic[item.Key].CanCheck();
            //    }
            //}

            string name = data["name"];
            int begin = data["begin"];
            int end = data["end"];

            _timeText.text = ContMgr.getCont("ButtonText3") + "： <color=#1BD4EF>" + Globle.getStrTime(begin, true, true) + "</color> 至 <color=#1BD4EF>" + Globle.getStrTime(end, true, true) + "</color>";

            Variant Selfdata = null;
            if (a3_awardCenter.roleData.ContainsKey(data["tp"]))
            {
                Selfdata = a3_awardCenter.roleData[data["tp"]];
            }
            else
            {
                Selfdata = new Variant();
                Selfdata["tp"] = 5;
                Selfdata["lists"].setToArray();
            }

            Dictionary<int, Variant> Selfdata_Item = new Dictionary<int, Variant>();
            foreach (Variant item in Selfdata["lists"]._arr)
            {
                Selfdata_Item[item["id"]._int] = item;
            }

            foreach (Variant item in action)
            {

                //WelfareModel.itemWelfareData iwfdTemp = iwd;
                ////iwfdTemp.desc = strDesc;
                //a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                //GameObject go =IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
                if (!itemGoLst.ContainsKey(item["id"]._str))
                {
                    itemGoLst[item["id"]._str] = IconImageMgr.getInstance().creatItemAwardCenterIcon(null);
                }
                GameObject _go = itemGoLst[item["id"]._str];
                _go.name = "itemWelfare";
                _go.transform.SetParent(itemsParent);
                _go.transform.localScale = Vector3.one;
                _go.transform.FindChild("Common").gameObject.SetActive(false);

                GameObject go = _go.transform.FindChild("Specific").gameObject;
                go.SetActive(true);

                int selfLimit = 0;
                if (Selfdata_Item.Count != 0 && Selfdata_Item.ContainsKey(item["id"]))
                {
                    selfLimit = Selfdata_Item[item["id"]]["reach_num"]._int;
                }

                int currLimit = item["limit"] == null ? 0 : item["limit"]._int - selfLimit;
                go.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("keduihuan") + currLimit;


                List<Variant> item_cost = item["item_cost"]._arr; //所需物品
                bool isNo = false;
                int nub = 0;
                for (int i = 0; i < item_cost.Count; i++)
                {
                    nub++;
                    uint valueId = item_cost[i]["item_id"]._uint;
                    int valueNumOfBag = a3_BagModel.getInstance().getItemNumByTpid(valueId);
                    int needNum = item_cost[i]["value"]._int;

                    Text numGoText = go.transform.FindChild("Item/icon_Item" + (i + 1) + "/Num").gameObject.GetComponent<Text>();
                    numGoText.text = valueNumOfBag + "/" + needNum;
                    if (valueNumOfBag >= needNum)
                        numGoText.color = Color.green;
                    else
                    {
                        isNo = true;
                        numGoText.color = Color.red;
                    }

                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(valueId);
                    GameObject itemGo = go.transform.FindChild("Item/icon_Item" + (i + 1)).gameObject;
                    itemGo.transform.FindChild("Border").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(itemData.borderfile);
                    itemGo.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(itemData.file);
                    itemGo.SetActive(true);
                    new BaseButton(itemGo.transform).onClick = (GameObject oo) =>
                    {

                        if (a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip(valueId);
                        }
                    };

                    if (i < item_cost.Count - 1)
                    {
                        go.transform.FindChild("Item/Add" + (i + 1)).gameObject.SetActive(true);
                    }

                }

                bool isNos = false;
                if (item["money_cost"] != null && item["money_cost"]._arr.Count > 0)
                {
                    List<Variant> money_cost = item["money_cost"]._arr; //所需货币

                    for (int i = 0; i < money_cost.Count; i++)
                    {
                        int moneytype = money_cost[i]["money_type"]._int;
                        GameObject money_icon = IconImageMgr.getInstance().createMoneyIcon(moneytype.ToString());
                        uint moneytype_have = 0;
                        switch (moneytype)
                        {
                            case 2:
                                moneytype_have = PlayerModel.getInstance().money;
                                break;
                            case 3:
                                moneytype_have = PlayerModel.getInstance().gold;
                                break;
                            case 4:
                                moneytype_have = PlayerModel.getInstance().gift;
                                break;

                        }
                        int needNum = money_cost[i]["value"]._int;
                        Text numGoText = go.transform.FindChild("Item/icon_Item" + (nub + 1) + "/Num").gameObject.GetComponent<Text>();
                        numGoText.text = moneytype_have + "/" + needNum;
                        if (moneytype_have >= needNum)
                            numGoText.color = Color.green;
                        else
                        {
                            isNos = true;
                            numGoText.color = Color.red;
                        }
                        GameObject itemData = IconImageMgr.getInstance().createMoneyIcon(moneytype.ToString());
                        GameObject icon_gold = go.transform.FindChild("Item/icon_Item" + (nub + 1)).gameObject;
                        icon_gold.SetActive(true);
                        icon_gold.transform.FindChild("icon").gameObject.SetActive(false);
                        icon_gold.transform.FindChild("Border").gameObject.SetActive(false);
                        itemData.transform.SetParent(icon_gold.transform, false);
                        itemData.transform.SetAsFirstSibling();
                        if (i < nub)
                        {
                            go.transform.FindChild("Item/Add" + (i + 1)).gameObject.SetActive(true);
                        }
                    }
                }







                List<Variant> rewardValue = item["RewardValue"]._arr; //货币奖励
                List<string> itemNameNumLst = new List<string>();
                if (rewardValue != null && rewardValue.Count > 0)
                {
                    string type = rewardValue[0]["type"];
                    GameObject iconGo = IconImageMgr.getInstance().createMoneyIcon(type);
                    GameObject icon_gold = go.transform.FindChild("Item/icon_Item5").gameObject;
                    icon_gold.transform.FindChild("icon").gameObject.SetActive(false);
                    icon_gold.transform.FindChild("Border").gameObject.SetActive(false); ;
                    iconGo.transform.SetParent(icon_gold.transform);
                    iconGo.transform.localPosition = new Vector3(0, 0, 0);
                    iconGo.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    List<Variant> rewardItem = item["RewardItem"]._arr;
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(rewardItem[0]["item_id"]._uint);
                    GameObject itemGo = go.transform.FindChild("Item/icon_Item5").gameObject;
                    itemGo.transform.FindChild("Border").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(itemData.borderfile);
                    itemGo.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(itemData.file);
                    itemGo.transform.FindChild("Num").GetComponent<Text>().text = rewardItem[0]["value"]._uint + "";
                    itemNameNumLst.Add(itemData.item_name + "*" + rewardItem[0]["value"]._uint);
                    new BaseButton(itemGo.transform).onClick = (GameObject oo) =>
                    {

                        if (a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip(rewardItem[0]["item_id"]._uint);
                        }
                    };

                }




                Transform btnGet = go.transform.FindChild("state");
                if (a3_awardCenter.yellowSprite == null)
                {
                    a3_awardCenter.yellowSprite = btnGet.FindChild("btnGet").GetComponent<Button>().spriteState.pressedSprite;
                    a3_awardCenter.graySprite = btnGet.FindChild("btnGet").GetComponent<Button>().spriteState.disabledSprite;
                }
                //if ( num>=_num )
                //{
                //    if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 1 )
                //    {
                //        btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter5" );
                //    }
                //    else if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 2 )
                //    {
                //        btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                //        btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                //    }

                //}
                //else
                //{
                //    btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter4" );
                //}

                if (!isNo && !isNos && currLimit > 0)
                {
                    btnGet.FindChild("btnGet").GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                    SpriteState ss = new SpriteState();
                    ss.pressedSprite = a3_awardCenter.yellowSprite;
                    btnGet.FindChild("btnGet").GetComponent<Button>().spriteState = ss;
                }
                else
                {
                    SpriteState ss = new SpriteState();
                    ss.pressedSprite = a3_awardCenter.graySprite;
                    btnGet.FindChild("btnGet").GetComponent<Button>().spriteState = ss;
                    btnGet.FindChild("btnGet").GetComponent<Image>().sprite = a3_awardCenter.graySprite;
                }

                new BaseButton(btnGet.FindChild("btnGet")).onClick = (GameObject oo) =>
              {
                  if (!isNo && currLimit > 0)
                  {
                      Variant Itemdata = new Variant();
                      Itemdata["award_id"] = item["id"]._uint;

                      A3_AwardCenterServer.getInstance().SendMsg(A3_AwardCenterServer.EVENT_EXCHANGE, Itemdata);
                      currLimit -= 1;
                      go.transform.FindChild("Text").GetComponent<Text>().text = "可兑换:" + currLimit;

                      if (itemNameNumLst.Count != 0)
                      {
                          for (int i = 0; i < itemNameNumLst.Count; i++)
                          {
                              flytxt.instance.fly(itemNameNumLst[i]);
                          }
                      }
                      //btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                      //btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                      for (int i = 0; i < item_cost.Count; i++)
                      {
                          uint valueId = item_cost[i]["item_id"]._uint;
                          int needNum = item_cost[i]["value"]._int;
                          int valueNumOfBag = a3_BagModel.getInstance().getItemNumByTpid(valueId) - needNum;


                          Text numGoText = go.transform.FindChild("Item/icon_Item" + (i + 1) + "/Num").gameObject.GetComponent<Text>();
                          numGoText.text = valueNumOfBag + "/" + needNum;
                          if (valueNumOfBag >= needNum)
                          {
                              numGoText.color = Color.green;
                          }
                          else
                          {
                              isNo = true;
                              numGoText.color = Color.red;
                          }
                      }
                      if (item["money_cost"] != null && item["money_cost"]._arr.Count > 0)
                      {
                          List<Variant> money_cost = item["money_cost"]._arr; //所需货币

                          for (int i = 0; i < money_cost.Count; i++)
                          {
                              int moneytype = money_cost[i]["money_type"]._int;
                              uint moneytype_have = 0;
                              switch (moneytype)
                              {
                                  case 2:
                                      moneytype_have = PlayerModel.getInstance().money- moneytype_have;
                                      break;
                                  case 3:
                                      moneytype_have = PlayerModel.getInstance().gold- moneytype_have;
                                      break;
                                  case 4:
                                      moneytype_have = PlayerModel.getInstance().gift- moneytype_have;
                                      break;

                              }
                              int needNum = money_cost[i]["value"]._int;
                              Text numGoText = go.transform.FindChild("Item/icon_Item" + (nub + 1) + "/Num").gameObject.GetComponent<Text>();
                              numGoText.text = moneytype_have+ "/" + needNum;
                              if (moneytype_have >= needNum)
                                  numGoText.color = Color.green;
                              else
                              {
                                  isNos = true;
                                  numGoText.color = Color.red;
                              }
                          }
                      }

                          if (isNo || isNos || currLimit <= 0)
                          {
                              SpriteState ss = new SpriteState();
                              ss.pressedSprite = a3_awardCenter.graySprite;
                              btnGet.FindChild("btnGet").GetComponent<Button>().spriteState = ss;
                              btnGet.FindChild("btnGet").GetComponent<Image>().sprite = a3_awardCenter.graySprite;
                          }
                      }
                      else
                      {
                          flytxt.instance.fly(ContMgr.getCont("a3_award_unable"));
                      }

                  };

                 }
                  
        }
            

        

        override public void onClose()
        {

        }
        void onAccumulateConsumption( GameEvent e )
        {
            uint giftId = e.data["gift_id"];
            if ( consumptionAwardsDic.ContainsKey( giftId ) )
            {
                consumptionAwardsDic[ giftId ].Checked();
            }
        }
    }

    public class TodayRechargePanel:BaseAwardCenter//团购
    {
        Transform root;
        Transform itemsParent;
        Dictionary<uint, awardCenterItem4todayRecharge> todayRechargeAwardsDic;
        //string strDesc = "今日累积充值{0}钻石可获得{1}一个";
        //string strDesc = ContMgr.getCont("a3_awardCenter3");
        Text _timeText;
        List<Variant> action;
        Variant data ;
        Dictionary<string,GameObject> itemGoLst = new Dictionary<string, GameObject>();
        int index = 0;
        public TodayRechargePanel( Transform trans , Variant v , Text timeText )
            : base(trans)
        {
            root = trans;
            todayRechargeAwardsDic = new Dictionary<uint, awardCenterItem4todayRecharge>();
            itemsParent = root.FindChild("awardItems/content");
            //List<WelfareModel.itemWelfareData> dailyRechargeAwardList = WelfareModel.getInstance().getDailyRecharge();
            //foreach (WelfareModel.itemWelfareData iwd in dailyRechargeAwardList)
            //{
            //    WelfareModel.itemWelfareData iwfdTemp = iwd;
            //    //iwfdTemp.desc = strDesc;
            //    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
            //    iwfdTemp.awardName = itemData.item_name;
            //    GameObject go = IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
            //    go.name = "itemWelfare";
            //    go.transform.SetParent(itemsParent);
            //    go.transform.localScale = Vector3.one;
            //    uint id = iwd.itemId;
            //    new BaseButton(go.transform.FindChild("icon")).onClick = (GameObject oo) =>
            //    {
            //        if (a3_awardCenter.instan)
            //        {
            //            a3_awardCenter.instan.showtip(id);
            //        }
            //    };
            //    awardCenterItem4todayRecharge aci = new awardCenterItem4todayRecharge(go.transform, iwfdTemp);
            //    todayRechargeAwardsDic.Add(iwd.id, aci);
            //}

            data = v;
            _timeText = timeText;
            action = data[ "action" ][ "list" ]._arr;
            debug.Log("团购:"+data.dump());
            A3_AwardCenterServer.getInstance().addEventListener( A3_AwardCenterServer.EVENT_TUAN_INFORM , (e)=> {
                data[ "total_num" ] = e.data[ "total_num" ];
                index++;
                if ( index == 2 )
                {
                    index = 0;
                    onShowed();
                }
             
            } );

            A3_AwardCenterServer.getInstance().addEventListener( A3_AwardCenterServer.EVENT_TUAN , ( e ) =>
            {
                data[ "num" ] = e.data[ "num" ];
                index++;
                if ( index == 2 )
                {
                    index = 0;
                    onShowed();
                }

            } );

            itemsParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140 * ( action.Count));
            //welfareProxy.getInstance().addEventListener(welfareProxy.ACCUMULATETODAYRECHARGE, onAccumulateTodayRecharge);
        }
     
        override public void onShowed()
        {
            //if (welfareProxy.getInstance().dailyGift == null) return;
            //List<uint> checkedList = new List<uint>();
            //checkedList.Clear();
            //foreach (Variant variantList in welfareProxy.getInstance().dailyGift)
            //{
            //    uint id = (uint)variantList;
            //    checkedList.Add(id);
            //    if (todayRechargeAwardsDic.ContainsKey(id))
            //    {
            //        todayRechargeAwardsDic[id].transform.FindChild("state/imgChecked").gameObject.SetActive(true);
            //        todayRechargeAwardsDic[id].transform.FindChild("state/btnGet").gameObject.SetActive(false);
            //        todayRechargeAwardsDic[id].transform.SetAsLastSibling();
            //    }
            //}
            //foreach (KeyValuePair<uint, awardCenterItem4todayRecharge> item in todayRechargeAwardsDic)
            //{
            //    if (checkedList.Contains(item.Key)) continue;
            //    if (item.Value.m_iwd.cumulateNum > welfareProxy.todayTotal_recharge)//无法领取的
            //    {
            //        todayRechargeAwardsDic[item.Key].CanNotCheck();
            //    }
            //    else
            //    {
            //        todayRechargeAwardsDic[item.Key].CanCheck();
            //    }
            //}

            string name = data["name"];
            int begin = data["begin"];
            int end = data["end"];

            _timeText.text =  ContMgr.getCont("ButtonText3")+"： <color=#1BD4EF>" + Globle.getStrTime( begin , true , true ) + "</color> 至 <color=#1BD4EF>" +Globle.getStrTime( end , true , true )+"</color>";

            Variant Selfdata = null;
            if ( a3_awardCenter.roleData.ContainsKey( data[ "tp" ] ) )
            {
                Selfdata = a3_awardCenter.roleData[data["tp"]];
            }
            else
            {
                Selfdata =  new Variant();
                Selfdata[ "tp" ] = 4;
                Selfdata[ "num" ] = 0;
                Selfdata[ "lists" ].setToArray();
            }
            if ( data[ "num" ]!= null && data["num"] != 0 )
            {
                Selfdata[ "num" ] = data[ "num" ];
            }

            Dictionary<int,Variant> Selfdata_Item = new Dictionary<int, Variant>();
            foreach ( Variant item in Selfdata[ "lists" ]._arr )
            { 
                Selfdata_Item[ item[ "id" ]._int ] = item;
            }


            uint itemId = data["action"]["item_id"]._uint;
            int costType = data["action"]["cost_type"]._int;
            int costNum =  data["action"]["item_cost"]._int;
            int itemNum = data["total_num"]._int;
            a3_ItemData propData = a3_BagModel.getInstance().getItemDataById(itemId);
            root.FindChild("Details/IconBG/IconB").transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( propData.borderfile );
            root.FindChild( "Details/IconBG/Icon" ).transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( propData.file );

            new BaseButton( root.FindChild( "Details/IconBG").transform ).onClick=( GameObject oo ) =>
            {

                if ( a3_awardCenter._instance)
                {
                    a3_awardCenter._instance.showtip( itemId );
                }
            };
            string costType_file = "";

            switch (costType)
            {
                case 2: costType_file = "icon_coin_coin1"; break;
                case 3: costType_file = "icon_coin_coin2"; break;
                case 4: costType_file = "icon_coin_coin3"; break;
            }
            root.FindChild ("Details/Image").GetComponent <Image>().sprite = GAMEAPI.ABUI_LoadSprite(costType_file);

            root.FindChild( "Details/Name" ).transform.GetComponent<Text>().text = propData.item_name;
            root.FindChild( "Details/Num" ).transform.GetComponent<Text>().text = itemNum + "";
            root.FindChild( "Details/PayNum" ).transform.GetComponent<Text>().text = costNum + "";

            GameObject button = root.FindChild("Details/Button").gameObject;
            if ( Selfdata[ "num" ]._int != 0 )
            {
                button.SetActive( false );
            }
            else
            {
                new BaseButton( button.transform ).onClick=( GameObject oo ) =>
                {
                    A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.EVENT_TUAN );
                };
            }
          


            foreach ( Variant item in action )
            {

                //WelfareModel.itemWelfareData iwfdTemp = iwd;
                ////iwfdTemp.desc = strDesc;
                //a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                //GameObject go =IconImageMgr.getInstance().creatItemAwardCenterIcon(itemData);
                if ( !itemGoLst.ContainsKey( item[ "id" ]._str ) )
                {
                    itemGoLst[ item[ "id" ]._str ] = IconImageMgr.getInstance().creatItemAwardCenterIcon( null );
                }
                GameObject _go = itemGoLst[ item[ "id" ]._str ];
                _go.name = "itemWelfare";
                _go.transform.SetParent( itemsParent );
                _go.transform.localScale = Vector3.one;
                GameObject go = _go.transform.FindChild("Common").gameObject;

                go.transform.FindChild( "Text_Three" ).gameObject.SetActive( true );
                go.transform.FindChild( "Text_One" ).gameObject.SetActive(false);

                Text conditionText = go.transform.FindChild("Text_Three/txtInforNum").GetComponent<Text>();
                conditionText.text = item[ "param" ]._str + "次";

                GameObject progressGo = go.transform.FindChild("Progress/Image").gameObject;
                //GameObject progressGo = go.transform.FindChild("Progress").gameObject;
                //progressGo.SetActive( false );
                int _num = item[ "param" ]._int;
                int num = data["total_num"]._int;
                progressGo.GetComponent<Image>().fillAmount = ( float ) num/( float ) _num;
                go.transform.FindChild("Progress/Text").gameObject.SetActive(true);
                go.transform.FindChild("Progress/Text").GetComponent<Text>().text = num + "/" + _num;
                List<Variant> rewardValue = item["RewardValue"]._arr; //货币奖励
                for ( int i = 0 ; i < rewardValue.Count ; i++ )
                {
                    string type = rewardValue[i]["type"];
                    GameObject iconGo =  IconImageMgr.getInstance().createMoneyIcon(type);
                    GameObject icon_gold = go.transform.FindChild("icon_Item" + (i+1)).gameObject;
                    go.transform.FindChild( "icon_Item" + ( i+1 )+ "/icon" ).gameObject.SetActive( false );
                    go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Border" ).gameObject.SetActive( false ); ;
                    iconGo.transform.SetParent( icon_gold.transform );
                    iconGo.transform.localPosition = new Vector3( 0 , 0 , 0 );
                    iconGo.transform.localScale = new Vector3( 1 , 1 , 1 );

                    //new BaseButton( iconGo.transform ).onClick=( GameObject oo ) =>
                    //{

                    //    if ( a3_awardCenter.instan )
                    //    {
                    //        a3_awardCenter.instan.showtip(uint.Parse( type ) );
                    //    }
                    //};

                }

                List<Variant> rewardItem = item["RewardItem"]._arr;//道具奖励
                List<string> itemNameNumLst = new List<string>();
                for ( int i = rewardValue.Count ; i < rewardItem.Count+rewardValue.Count ; i++ )
                {
                    uint id = rewardItem[i-rewardValue.Count]["item_id"];
                    uint valueNum =rewardItem[i-rewardValue.Count].ContainsKey("value")?rewardItem[i-rewardValue.Count]["value"]._uint:0;
                    if ( valueNum != 0 )
                    {
                        go.transform.FindChild( "icon_Item" + ( i+1 ) + "/Num" ).GetComponent<Text>().text = valueNum + "";
                    }
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(id);
                    GameObject icon_Go_Borderfile =  go.transform.FindChild("icon_Item" + (i+1) + "/Border" ).gameObject;
                    icon_Go_Borderfile.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.borderfile );
                    GameObject icon_Go = go.transform.FindChild("icon_Item" + (i+1) + "/icon").gameObject;
                    icon_Go.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itemData.file );
                    itemNameNumLst.Add( itemData.item_name +"*" + valueNum );

                    new BaseButton( icon_Go.transform ).onClick=( GameObject oo ) =>
                    {

                        if ( a3_awardCenter._instance)
                        {
                            a3_awardCenter._instance.showtip( id );
                        }
                    };

                }

                if ( rewardItem.Count+rewardValue.Count < 4 )
                {
                    for ( int i = rewardItem.Count+rewardValue.Count+1 ; i <= 4 ; i++ )
                    {
                        go.transform.FindChild( "icon_Item" + i ).gameObject.SetActive( false );
                    }
                }


                Transform  btnGet = go.transform.FindChild( "state");
                if ( a3_awardCenter.yellowSprite == null )
                {
                    a3_awardCenter.yellowSprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.pressedSprite;
                    a3_awardCenter.graySprite =  btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState.disabledSprite;
                }
                if ( Selfdata[ "num" ]._int == 0 )
                {
                    btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                }
                else
                {
                    if ( num>=_num )
                    {
                        if ( Selfdata_Item.ContainsKey( item[ "id" ]._int ) )
                        {
                            if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 1 )
                            {
                                btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                                SpriteState ss = new SpriteState();
                                ss.pressedSprite = a3_awardCenter.yellowSprite;
                                btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;

                                btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter5" );
                                a3_awardCenter.finishNum++;
                            }
                            else if ( Selfdata_Item[ item[ "id" ]._int ][ "state" ]._int == 2 )
                            {
                                btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                                btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                            }
                        }
                        else
                        {
                           
                            if ( btnGet.FindChild( "imgChecked" ).gameObject.activeSelf == false)
                            {
                                btnGet.FindChild( "btnGet" ).gameObject.SetActive( true );
                            }
                            btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.yellowSprite;
                            SpriteState ss = new SpriteState();
                            ss.pressedSprite = a3_awardCenter.yellowSprite;
                            btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;

                            btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter5" );
                            a3_awardCenter.finishNum++;
                        }

                    }
                    else
                    {
                        if ( btnGet.FindChild( "imgChecked" ).gameObject.activeSelf == false )
                        {
                            btnGet.FindChild( "btnGet" ).gameObject.SetActive( true );
                        }

                        SpriteState ss = new SpriteState();
                        ss.pressedSprite = a3_awardCenter.graySprite;
                        btnGet.FindChild( "btnGet" ).GetComponent<Button>().spriteState = ss;
                        btnGet.FindChild( "btnGet" ).GetComponent<Image>().sprite = a3_awardCenter.graySprite;

                        btnGet.FindChild( "btnGet/Text" ).GetComponent<Text>().text = ContMgr.getCont( "a3_awardCenter4" );
                    }

                    new BaseButton( btnGet.FindChild( "btnGet" ) ).onClick=( GameObject oo ) =>
                    {
                        if ( num>=_num )
                        {
                            Variant Itemdata = new Variant();
                            Itemdata[ "award_id" ] = item[ "id" ]._uint;

                            A3_AwardCenterServer.getInstance().SendMsg( A3_AwardCenterServer.EVENT_TUAN_GETAWARD , Itemdata );

                            btnGet.FindChild( "btnGet" ).gameObject.SetActive( false );//领取按钮隐藏
                            btnGet.FindChild( "imgChecked" ).gameObject.SetActive( true );
                            a3_awardCenter.finishNum--;
                            a3_awardCenter.instan.SetshowIconLight();
                            if ( itemNameNumLst.Count!=0 )
                            {
                                for ( int i = 0 ; i < itemNameNumLst.Count ; i++ )
                                {
                                    flytxt.instance.fly( itemNameNumLst[ i ] );
                                }
                            }
                        }
                        else
                        {
                            flytxt.instance.fly( ContMgr.getCont( "a3_award_unable" ) );
                        }

                    };
                }
             
            }


        }
        void onAccumulateTodayRecharge(GameEvent e)
        {
            uint giftId = e.data["gift_id"];
            if (todayRechargeAwardsDic.ContainsKey(giftId))
            {
                todayRechargeAwardsDic[giftId].Checked();
            }
        }
        override public void onClose()
        {
          
        }
    }
    class awardCenterItem4zhuan : BaseAwardCenter
    {
        public Transform root;
        Text txtNum;
        Text txtInfo;
        Transform imgChecked;
        BaseButton btnGet;
        public WelfareModel.itemWelfareData m_iwd;
       public awardCenterItem4zhuan(Transform trans,WelfareModel.itemWelfareData iwd):base(trans)
        {
            root = trans;
            m_iwd = iwd;
            txtInfo = getComponentByPath<Text>("txtInfor");
            imgChecked=getGameObjectByPath("state/imgChecked").transform;
            btnGet = new BaseButton(root.transform.FindChild("state/btnGet"));
            btnGet.onClick = onBtnGetClick;
            //txtInfo.text = string.Format(iwd.desc, iwd.zhuan);
            txtInfo.text = ContMgr.getCont("a3_awardCenter0", new List<string>() { iwd.zhuan.ToString(), iwd.award_num.ToString() });
            if (iwd.zhuan > PlayerModel.getInstance().up_lvl)
            {
                btnGet.interactable = false;
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");
            }
            else
            {
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
                btnGet.interactable = true;
            }

            //txtNum.text = iwd.num.ToString();
           // txtNum.gameObject.SetActive(true);
           
        }
       public void SetInfo(a3_ItemData id)
       {

           txtInfo.text = id.desc;

           root.gameObject.SetActive(true);
       }
       public void Checked()//已领取
       {
           btnGet.gameObject.SetActive(false);//领取按钮隐藏
           imgChecked.gameObject.SetActive(true);
           root.SetAsLastSibling();
       }
       public void CanNotCheck()//无法领取
       {
           btnGet.interactable = false;
           btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");

       }
       public void CanCheck()//可以领取
       {
           btnGet.interactable = true;
           btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
       }
      
        void onBtnGetClick(GameObject go)
        {
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.lvlAward, m_iwd.id); 
        }
        
    }
    class awardCenterItem4Recharge : BaseAwardCenter
    {
        public Transform root;
        Text txtNum;
        Text txtInfo;
        Transform imgChecked;
        BaseButton btnGet;
        public WelfareModel.itemWelfareData m_iwd;
        public awardCenterItem4Recharge(Transform trans, WelfareModel.itemWelfareData iwd)
            : base(trans)
        {
            root = trans;
            m_iwd = iwd;
            txtInfo = getComponentByPath<Text>("txtInfor");
            imgChecked = getGameObjectByPath("state/imgChecked").transform;
            btnGet = new BaseButton(root.transform.FindChild("state/btnGet"));
            btnGet.onClick = onBtnGetClick;
            //txtInfo.text = string.Format(iwd.desc, iwd.cumulateNum,iwd.worth);
            txtInfo.text = ContMgr.getCont("a3_awardCenter1", new List<string>() { iwd.cumulateNum.ToString(), iwd.worth.ToString() });
            if (iwd.zhuan > PlayerModel.getInstance().up_lvl)
            {
                btnGet.interactable = false;
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");
            }
            else
            {
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
                btnGet.interactable = true;
            }

          //  txtNum.text = iwd.num.ToString();
           // txtNum.gameObject.SetActive(true);

        }
        public void SetInfo(a3_ItemData id)
        {

            txtInfo.text = id.desc;

            root.gameObject.SetActive(true);
        }
        public void Checked()//已领取
        {
            btnGet.gameObject.SetActive(false);//领取按钮隐藏
            imgChecked.gameObject.SetActive(true);
            root.SetAsLastSibling();
        }
        public void CanNotCheck()//无法领取
        {
            btnGet.interactable = false;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");

        }
        public void CanCheck()//可以领取
        {
            btnGet.interactable = true;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
        }

        void onBtnGetClick(GameObject go)
        {
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.accumulateRecharge, m_iwd.id);
        }

    }
    class awardCenterItem4Consumption : BaseAwardCenter
    {
        public Transform root;
        Text txtNum;
        Text txtInfo;
        Transform imgChecked;
        BaseButton btnGet;
        public WelfareModel.itemWelfareData m_iwd;
        public awardCenterItem4Consumption(Transform trans, WelfareModel.itemWelfareData iwd)
            : base(trans)
        {
            root = trans;
            m_iwd = iwd;
            txtInfo = getComponentByPath<Text>("txtInfor");
            imgChecked = getGameObjectByPath("state/imgChecked").transform;
            btnGet = new BaseButton(root.transform.FindChild("state/btnGet"));
            btnGet.onClick = onBtnGetClick;
           // txtInfo.text = string.Format(iwd.desc, iwd.cumulateNum, iwd.awardName);
            txtInfo.text = ContMgr.getCont("a3_awardCenter2",new List<string>() { iwd.cumulateNum.ToString(), iwd.awardName });
            if (iwd.zhuan > PlayerModel.getInstance().up_lvl)
            {
                btnGet.interactable = false;
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");
            }
            else
            {
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
                btnGet.interactable = true;
            }

           // txtNum.text = iwd.num.ToString();
           // txtNum.gameObject.SetActive(true);

        }
        public void SetInfo(a3_ItemData id)
        {

            txtInfo.text = id.desc;

            root.gameObject.SetActive(true);
        }
        public void Checked()//已领取
        {
            btnGet.gameObject.SetActive(false);//领取按钮隐藏
            imgChecked.gameObject.SetActive(true);
            root.SetAsLastSibling();
        }
        public void CanNotCheck()//无法领取
        {
            btnGet.interactable = false;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");

        }
        public void CanCheck()//可以领取
        {
            btnGet.interactable = true;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
        }

        void onBtnGetClick(GameObject go)
        {
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.accumulateConsumption, m_iwd.id);
        }

    }
    class awardCenterItem4todayRecharge : BaseAwardCenter
    {
        public Transform root;
        Text txtNum;
        Text txtInfo;
        Transform imgChecked;
        BaseButton btnGet;
        public WelfareModel.itemWelfareData m_iwd;
        public awardCenterItem4todayRecharge(Transform trans, WelfareModel.itemWelfareData iwd)
            : base(trans)
        {
            root = trans;
            m_iwd = iwd;
            txtInfo = getComponentByPath<Text>("txtInfor");
            imgChecked = getGameObjectByPath("state/imgChecked").transform;
            btnGet = new BaseButton(root.transform.FindChild("state/btnGet"));
            btnGet.onClick = onBtnGetClick;
            //txtInfo.text = string.Format(iwd.desc, iwd.cumulateNum, iwd.awardName);
            txtInfo.text = ContMgr.getCont("a3_awardCenter3", new List<string>() { iwd.cumulateNum.ToString(), iwd.awardName});
            if (iwd.zhuan > PlayerModel.getInstance().up_lvl)
            {
                btnGet.interactable = false;
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");
            }
            else
            {
                btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
                btnGet.interactable = true;
            }

            //txtNum.text = iwd.num.ToString();
            //txtNum.gameObject.SetActive(true);

        }
        public void SetInfo(a3_ItemData id)
        {
            txtInfo.text = id.desc;

            root.gameObject.SetActive(true);
        }
        public void Checked()//已领取
        {
            btnGet.gameObject.SetActive(false);//领取按钮隐藏
            imgChecked.gameObject.SetActive(true);
            root.SetAsLastSibling();
        }
        public void CanNotCheck()//无法领取
        {
            btnGet.interactable = false;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter4");

        }
        public void CanCheck()//可以领取
        {
            btnGet.interactable = true;
            btnGet.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_awardCenter5");
        }

        void onBtnGetClick(GameObject go)
        {
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.dayRechargeAward, m_iwd.id);
        }

    }
    public class BaseAwardCenter:Skin
    {
        public BaseAwardCenter(Transform trans) : base(trans) { }
        virtual public void onShowed(){}
        virtual public void onClose(){}
    }
}
