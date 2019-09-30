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

namespace MuGame
{
    class shop_a3 : Window
    {

        bool AddorMin = true;
        int nowstate = 5;
        CanvasGroup bg;
        CanvasGroup bg_image;

        Dictionary<int, shopDatas> dic = new Dictionary<int, shopDatas>();
        List<shopDatas> lins_data = new List<shopDatas>();
        List<shopDatas> shop_data = new List<shopDatas>();
        public static int now_id = 0;

        Dictionary<int, GameObject> havePurchase = new Dictionary<int, GameObject>();//商品
        Dictionary<int, GameObject> limitedobj = new Dictionary<int, GameObject>();//限时抢购
        Dictionary<int, limitedinmfos> limitedinmfo = new Dictionary<int, limitedinmfos>();//显示抢购信息
        Dictionary<int /*activityId*/ , Queue<int> /* shop id*/> limitedActivity = new Dictionary<int, Queue<int>>();
        GameObject contain;
        GameObject nullitems;
        GameObject image_goodsorbundinggem;
        GameObject image_packs;
        GameObject image_limitedactive;
        GameObject buy_success;
        GameObject max;
        GameObject min;


        Text itiemd_time;//限时抢购的时间

        Text money;
        Text gold;
        Text coin;
        Text renown;
        Text textPageIndex;


        public GameObject objsurebuy;
        public GameObject desc;
        public GameObject close_btn;
        Text surebuy_name;
        Text surebuy_des;
        InputField Inputbuy_num;
        Text buy_num;
        Text needbuymoney;
        Scrollbar bar;
        GameObject bar_Handle;

        GameObject icon_small;
        Text text1;
        Text text_name;
        Text text_dengji;
        Text text_hasnum;
        public TabControl tab;
        public static shop_a3 _instance;
        ScrollControler scrollControer;
        GameObject[] btn;
        BaseButton chongzhi;

        //float f_Sens;
        //Scrollbar f_bar;
        Dictionary<Transform, BaseButton> tab_btns = new Dictionary<Transform, BaseButton>();
        //Transform f_id;

        //每页最大显示数量
        int maxNum = 8;
        //当前显示的页面索引
        int pageIndex = 1;
        //当前的页面类型
        int selectType = 1;
        //最大页数
        int maxPageNum = 1;

        int btni = 0;
        public int selectnum = 0;

        public static shop_a3 instance;
        public override void init()
        {
            _instance = this;
          
            textPageIndex = this.getComponentByPath<Text>("panel_right/Image1/Text");
            bg_image = getGameObjectByPath("panel_down/image").GetComponent<CanvasGroup>();
            contain = transform.FindChild("panel_right/scroll_rect/contain").gameObject;
            image_goodsorbundinggem = transform.FindChild("panel_right/scroll_rect/Image_goodsorbundinggem").gameObject;
            image_packs = transform.FindChild("panel_right/scroll_rect/Image_packs").gameObject;
            nullitems = transform.FindChild("panel_right/null_image").gameObject;
            image_limitedactive = transform.FindChild("panel_right/scroll_rect/Image_limitedactive").gameObject;
            itiemd_time = transform.FindChild("panel_right/Image/Text").GetComponent<Text>();
            money = transform.FindChild("panel_down/gems/image/num").GetComponent<Text>();
            gold = transform.FindChild("panel_down/gemss/image/num").GetComponent<Text>();
            coin = transform.FindChild("panel_down/gemsss/image/num").GetComponent<Text>();
            renown = transform.FindChild( "panel_down/renown/image/num" ).GetComponent<Text>();

            objsurebuy = transform.FindChild("objdes").gameObject;
            surebuy_name = objsurebuy.transform.FindChild("bg/contain/name").GetComponent<Text>();
            surebuy_des = objsurebuy.transform.FindChild("bg/contain/des_bg/Text").GetComponent<Text>();
            Inputbuy_num = objsurebuy.transform.FindChild("bg/contain/bug/InputField").GetComponent<InputField>();
            buy_num = Inputbuy_num.transform.FindChild("Text").GetComponent<Text>();
            bar = objsurebuy.transform.FindChild("bg/contain/Scrollbar").GetComponent<Scrollbar>();
            needbuymoney = transform.FindChild("objdes/bg/contain/paymoney/money").GetComponent<Text>();
            buy_success = transform.FindChild("buy_success").gameObject;
            bar_Handle = objsurebuy.transform.FindChild("bg/contain/Scrollbar/Sliding Area/Handle").gameObject;

            close_btn = transform.FindChild("close_desc").gameObject;
            desc = transform.FindChild("close_desc/text_bg").gameObject;
            icon_small = desc.transform.FindChild("iconbg/icon").gameObject;
            text1 = desc.transform.FindChild("text").GetComponent<Text>();
            text_name = desc.transform.FindChild("name/namebg").GetComponent<Text>();
            text_dengji = desc.transform.FindChild("name/dengji").GetComponent<Text>();
            text_hasnum = desc.transform.FindChild("name/hasnum").GetComponent<Text>();
            //====================tabcontrol===============================
            tab = new TabControl();
            tab.onClickHanle = tabhandel;
            selectnum = Shop_a3Model.getInstance().selectnum;
            tab.create(getGameObjectByPath("panel_left/content"), this.gameObject, selectnum, 0, false);
            //==================button放在这===============================
            #region 所有的button
            new BaseButton(transform.FindChild("panel_down/gemsss/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                a3_exchange.Instance?.transform.SetAsLastSibling();
            };
            new BaseButton(transform.FindChild("panel_down/gems/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                a3_Recharge.Instance?.transform.SetAsLastSibling();
            };
            new BaseButton(transform.FindChild("panel_down/addmoney/text")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                if (a3_Recharge.Instance != null)
                    a3_Recharge.Instance.transform.SetAsLastSibling();
            };
            new BaseButton(transform.FindChild("panel_right/Image1/right")).onClick = (GameObject go) =>
            {
                pageIndex += 1;
                int cc = btni + 1; if (cc > 4 && pageIndex >= maxPageNum + 1)
                { pageIndex = maxPageNum; }
                if (cc >= 5) cc = 4;

                if (pageIndex > maxPageNum)
                {
                    if (shop_data.Count % maxNum != 0)
                    {
                        if (pageIndex > (shop_data.Count / maxNum) + 1) return;
                    }
                    if (shop_data.Count % maxNum == 0)
                    {
                        if (pageIndex > (shop_data.Count / maxNum)) return;
                    }
                }
                OnShowSelect(pageIndex);

            };
            new BaseButton(transform.FindChild("panel_right/Image1/left")).onClick = (GameObject go) =>
            {
                pageIndex--;
                int oo = btni - 1; if (oo < 0 && pageIndex <= 0)
                { oo = 0; return; }
                if (oo < 0) oo = 0;
                if (pageIndex <= 0)
                    pageIndex = maxPageNum;
                OnShowSelect(pageIndex);
            };
            new BaseButton(getTransformByPath("contribute/goto")).onClick = (GameObject go) =>
             {
                 ArrayList arr = new ArrayList();
                 arr.Add(0);
                 InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO,arr);
                 a3_shejiao.instance?.transform.SetAsLastSibling();
             };

            BaseButton bs_bt1 = new BaseButton(transform.FindChild("objdes/bg/contain/btn_reduce"));
            bs_bt1.onClick = onLefts;
            BaseButton bs_bt2 = new BaseButton(transform.FindChild("objdes/bg/contain/btn_add"));
            bs_bt2.onClick = onRights;
            BaseButton max = new BaseButton(transform.FindChild("objdes/bg/contain/max"));
            max.onClick = onmax;
            BaseButton min = new BaseButton(transform.FindChild("objdes/bg/contain/min"));
            min.onClick = onmin;
            BaseButton bt_close = new BaseButton(transform.FindChild("close"));
            bt_close.onClick = onClose;
            BaseButton bt_close1 = new BaseButton(transform.FindChild("objdes/btn"));
            bt_close1.onClick = onClose1;
            BaseButton bt_close_desc = new BaseButton(transform.FindChild("close_desc/close_btn"));
            bt_close_desc.onClick = close_desc;
            BaseButton bt_close3 = new BaseButton(buy_success.transform.FindChild("bg/Button"));
            bt_close3.onClick = onClose3;
            chongzhi = new BaseButton(this.transform.FindChild("chongzhibtn"));
            chongzhi.onClick = OnChongzhi;
            #endregion
            //=============================================================
            InvokeRepeating("OnShowAchievementPage", 0, 0.3f);
            foreach (var item in Shop_a3Model.getInstance().itemsdic)
            {
                dic.Add(item.Key, item.Value);
            }
            if (A3_LegionModel.getInstance().myLegion.clname == null)//初始化判断当前有没有军团
            {
                getGameObjectByPath("panel_left/content/5").SetActive(false);
            }
            else
            {
                if (FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
                {
                    getGameObjectByPath("panel_left/content/5").SetActive(true);
                }
                else
                    getGameObjectByPath("panel_left/content/5").SetActive(false);
            }

            getGameObjectByPath( "panel_left/content/7" ).transform.SetSiblingIndex(3); 

            switch (selectnum)//用于跳转时候才初始化情况
            {
                case 0: tab1(); break;
                case 1: tab2(); break;
                case 2: tab3(); break;
                case 3: tab4(); break;
                case 4: tab5(); break;
                case 5: tab6(); break;
                case 6: tab7(); break;
                default:
                    break;
            }
            getComponentByPath<Text>("contribute/num").text = A3_LegionModel.getInstance().donate.ToString();

            getComponentByPath<Text>("panel_right/scroll_rect/Image_limitedactive/buy_over/Text").text = ContMgr.getCont("shop_a3_0");
            getComponentByPath<Text>("panel_right/null_image/Text").text = ContMgr.getCont("shop_a3_1"); 
            getComponentByPath<Text>("panel_right/libao_text_bg/Text").text = ContMgr.getCont("shop_a3_2");
            getComponentByPath<Text>("panel_left/content/1/Text").text = ContMgr.getCont("shop_a3_3");
            getComponentByPath<Text>("panel_left/content/2/Text").text = ContMgr.getCont("shop_a3_4");
            getComponentByPath<Text>("panel_left/content/3/Text").text = ContMgr.getCont("shop_a3_5");
            getComponentByPath<Text>("panel_left/content/4/Text").text = ContMgr.getCont("shop_a3_6");
            getComponentByPath<Text>("panel_left/content/5/Text").text = ContMgr.getCont("shop_a3_7");
            getComponentByPath<Text>("panel_left/content/6/Text").text = ContMgr.getCont("shop_a3_8");
            getComponentByPath<Text>("panel_left/content/7/Text").text = ContMgr.getCont("shop_a3_9");
            getComponentByPath<Text>("chongzhibtn/Text").text = ContMgr.getCont("shop_a3_10");
            getComponentByPath<Text>("objdes/bg/contain/paymoney/Text").text = ContMgr.getCont("shop_a3_11");
            getComponentByPath<Text>("objdes/bg/contain/min/Text").text = ContMgr.getCont("usetip_summon_8");
            getComponentByPath<Text>("objdes/bg/contain/max/Text").text = ContMgr.getCont("usetip_summon_9");
            getComponentByPath<Text>("objdes/bg/Button/Text").text = ContMgr.getCont("shop_a3_12");
            getComponentByPath<Text>("close_desc/text_bg/name/lite").text = ContMgr.getCont("shop_a3_13");
            getComponentByPath<Text>("close_desc/text_bg/name/has").text = ContMgr.getCont("shop_a3_14");

        }
        #region   about tabcontrol
        public bool isbangding = false;
        void tabhandel(TabControl t)
        {
          
            selectnum = t.getSeletedIndex();
            isbangding = selectnum == 2 ? true : false;
            switch (selectnum)
            {
                case 0: tab1(); break;
                case 1: tab2(); break;
                case 2: tab3(); break;
                case 3: tab4(); break;
                case 4: tab5(); break;
                case 5: tab6(); break;
                case 6: tab7(); break;
                default:
                    break;
            }
        }

        // Add

        void tab7()
        {
            OnShowRenownShop();
            OnShowSelect( 1 );
        }

        void OnShowRenownShop() {

            btni = 7;

            shop_data.Clear();

            getGameObjectByPath( "panel_down" ).SetActive( true );
            getGameObjectByPath( "contribute" ).SetActive( false );

            if ( nowstate == 7 )

                return;

            deletecontain();

            foreach ( int i in dic.Keys )
            {
                if ( dic[ i ].type == 7 )
                {
                    shop_data.Add( dic[ i ] );

                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;

                    OnSetGameObjectData( objClone , i );
                }
            }

            nowstate = 7;

        }

        void OnSetGameObjectData(GameObject objClone , int index ) {

            shopDatas  data = Shop_a3Model.getInstance().itemsdic[ index ];

            int itemId = data.itemid;
            int shop_id = data.id;
            int money_type = data.money_type;
            int item_money = data.value;

            if ( objClone.active == false ) objClone.SetActive( true );
            objClone.transform.SetParent( contain.transform , false );
            objClone.transform.FindChild( "bg/name" ).GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById( ( uint ) itemId ).item_name;
            int color = a3_BagModel.getInstance().getItemDataById((uint)itemId).quality;
            objClone.transform.FindChild( "bg/name" ).GetComponent<Text>().color = Globle.getColorByQuality( color );
            objClone.transform.FindChild( "bg/Image/price" ).GetComponent<Text>().text = data.value.ToString();
            objClone.transform.FindChild( "bg/surplus_num" ).GetComponent<Text>().text = ContMgr.getCont( "shop_a3_todaynum" ) + data.limiteD + "/" + data.limiteNum + ")";
            objClone.transform.FindChild( "bg/Image/gold" ).gameObject.SetActive( money_type == 3 );
            objClone.transform.FindChild( "bg/Image/bangdingbaoshi" ).gameObject.SetActive( money_type == 4 );
            objClone.transform.FindChild( "bg/Image/contribute" ).gameObject.SetActive( money_type == 10 );
            objClone.transform.FindChild( "bg/Image/renown" ).gameObject.SetActive( money_type == 5 );

            GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
            string file = a3_BagModel.getInstance().getItemDataById((uint)data.itemid).file;
            icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( file );
            objClone.transform.FindChild( "bg/surplus_num" ).gameObject.SetActive( true );
            objClone.transform.FindChild( "bg/Image/money" ).gameObject.SetActive( false );

            BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/btn").transform);

            btn.onClick = delegate ( GameObject goo )
            {
                if ( data.limiteD == 0 )
                { flytxt.instance.fly( ContMgr.getCont( "shop_a3_txt" ) , 1 ); return; }
                objsurebuy.SetActive( true );

                surebuy( shop_id , itemId , Shop_a3Model.getInstance().itemsdic[ shop_id ].limiteD , money_type , item_money , 2 );
            };

            new BaseButton( icon.transform ).onClick = ( GameObject gos ) =>
            {
                icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( file );
                close_btn.SetActive( true );
                List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + itemId);
                foreach ( var x in xml )
                {
                    string ss = x.getString("desc");
                    text1.text = StringUtils.formatText( ss );
                    text_name.text = x.getString( "item_name" );
                    text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid( ( uint ) itemId ) + ContMgr.getCont( "ge" );
                    text_dengji.text = x.getString( "use_limit" );
                    int zhuan = x.getInt("use_limit");
                    if ( zhuan == 0 ) text_dengji.text = ContMgr.getCont( "a3_active_wuxianzhi" );
                    else
                        text_dengji.text = x.getString( "use_limit" ) + ContMgr.getCont( "zhuan" );
                    text_name.color = Globle.getColorByQuality( color );
                }
            };

            havePurchase[ index ] = objClone;

        }


        void tab6()
        {
            onShowlimitedactive();
            OnShowSelect(1);
        }
        void tab1()
        {
            onShowpacks();
            OnShowSelect(1);
        }
        void tab2()
        {
            onShowgoods();
            OnShowSelect(1); 
        }
        public     void tab3()
        {
            onShowbundinggem();
            OnShowSelect(1);
        }
        void tab4()
        {
            onSummonTab();
            OnShowSelect(1);
        }
        void tab5()
        {
            onLegion();
            OnShowSelect(1);
        }

        #endregion

        private void close_desc(GameObject obj)
        {
            close_btn.SetActive(false);
        }

        void haveClan(GameEvent e)
        {
            if (FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
            {
                getGameObjectByPath("panel_left/content/5").SetActive(true);
            }
            else
                getGameObjectByPath("panel_left/content/5").SetActive(false);
        }
        void deleteClan(GameEvent e)
        {
            getGameObjectByPath("panel_left/content/5").SetActive(false);
            tab.setSelectedIndex(0);
        }
        void RefreshInfo(GameEvent e)
        {
            if (FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
            {
                getGameObjectByPath("panel_left/content/5").SetActive(true);
            }
            else
                getGameObjectByPath("panel_left/content/5").SetActive(false);
        }
        void changeDonate(GameEvent e)
        {
            onLegion();
            if (e.data.ContainsKey("donate"))
                A3_LegionModel.getInstance().donate = e.data["donate"];
            getComponentByPath<Text>("contribute/num").text = A3_LegionModel.getInstance().donate.ToString();
        }
        private void OnShowAchievementPage()
        {
            float yy = contain.GetComponent<RectTransform>().anchoredPosition.y;
            float y1 = image_goodsorbundinggem.GetComponent<RectTransform>().sizeDelta.y;
            if (yy < y1)
            {
                pageIndex = 1;
                textPageIndex.text = 1 + "/" + maxPageNum;
                return;
            }
            for (int i = 2; i <= maxPageNum; i++)
            {
                if (yy >= y1 && yy >= 4 * y1 * i - (7 * y1) - 20 && yy < 4 * y1 * (i + 1) - (7 * y1) - 20)
                {

                    pageIndex = i;
                    textPageIndex.text = i + "/" + maxPageNum;
                }
            }

        }
        private void OnShowSelect(int ss)//页面的计数
        {
            float y1 = image_goodsorbundinggem.GetComponent<RectTransform>().sizeDelta.y;
            pageIndex = ss;
            maxNum = 8;

            if (shop_data.Count % maxNum != 0 || shop_data.Count == 0)
                maxPageNum = shop_data.Count / maxNum + 1;
            else
                maxPageNum = shop_data.Count / maxNum;
            textPageIndex.text = pageIndex + "/" + maxPageNum;

            contain.GetComponent<RectTransform>().sizeDelta = new Vector2(0, ((contain.GetComponent<GridLayoutGroup>().cellSize.y) + 2) * (Mathf.CeilToInt(shop_data.Count / 2.0f)));
            int ls_yu = shop_data.Count % maxNum;
            int ls_cs = shop_data.Count / maxNum;
            if (maxPageNum > 1)
            {
                if (ls_yu % 2 != 0) ls_yu++;
                if (ls_yu == 0 || pageIndex < maxPageNum)
                    contain.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (pageIndex - 1) * (4 * y1 + 8), 0);
                else if (pageIndex >= 2)
                {
                    contain.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (pageIndex - 2) * (4 * y1 + 8) + (ls_yu / 2) * (y1 + 2), 0);

                }
            }

            if (maxPageNum <= 1) contain.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        public void setopen(int id)
        {
            int type = 0;
            SXML shopXML = XMLMgr.instance.GetSXML("golden_shop.golden_shop", "id==" + id);
            if (shopXML == null) return;
            type = shopXML.getInt("type");
            int group = 0;
            group = shopXML.getInt("group");
            switch (type)
            {
                case 1://商品
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(1);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
                case 2://礼包
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(0);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
                case 3://绑定宝石
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(2);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
                case 4://限时抢购
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(4);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
                case 5://金币商店
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(3);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
                case 6://军团商店
                    objsurebuy.SetActive(true);
                    tab.setSelectedIndex(5);
                    surebuy(id, dic[id].itemid, 0, dic[id].money_type, dic[id].value, 2);
                    break;
            }
            float f = contain.GetComponent<GridLayoutGroup>().cellSize.y + contain.GetComponent<GridLayoutGroup>().spacing.y;
            contain.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, f * 4 * (group - 1), 0);
        }
        public override void onShowed()
        {
            instance = this;
            Toclose = false;
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            if (!FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
            {
                tab.setSelectedIndex(0);
            }
            refreshGold();
            refreshGift();
            refreshCoin();
            refreshRenown();
            OnShowSelect(1);
            bg_image.interactable = false;
            bg_image.blocksRaycasts = false;
            selectnum = Shop_a3Model.getInstance().selectnum;
            if (uiData != null)
            {
                int id = (int)uiData[0];
                setopen(id);
            }
            else if(Shop_a3Model.getInstance ().toSelect ) {
                 tab.setSelectedIndex(Shop_a3Model .getInstance ().selectType);
         
            }

            GRMap.GAME_CAMERA.SetActive(false);

            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CREATE, haveClan);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_DELETECLAN, deleteClan);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_QUIT, deleteClan);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETINFO, changeDonate);
            Shop_a3Proxy.getInstance().addEventListener(Shop_a3Proxy.DONATECHANGE, changeDonate);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshInfo);

            Shop_a3Proxy.getInstance().addEventListener(Shop_a3Proxy.LIMITED, onLimited);
            Shop_a3Proxy.getInstance().addEventListener(Shop_a3Proxy.CHANGELIMITED, onChangeLimited);
            Shop_a3Proxy.getInstance().addEventListener(Shop_a3Proxy.DELETELIMITED, onDeleteLimited);
            Shop_a3Model.getInstance().toSelect = false;
        }
        bool Toclose = false;
        public override void onClosed()
        {
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            Shop_a3Proxy.getInstance().removeEventListener(Shop_a3Proxy.DELETELIMITED, onDeleteLimited);
            Shop_a3Proxy.getInstance().removeEventListener(Shop_a3Proxy.CHANGELIMITED, onChangeLimited);
            Shop_a3Proxy.getInstance().removeEventListener(Shop_a3Proxy.LIMITED, onLimited);

            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CREATE, haveClan);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_DELETECLAN, deleteClan);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_QUIT, deleteClan);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETINFO, changeDonate);
            Shop_a3Proxy.getInstance().removeEventListener(Shop_a3Proxy.DONATECHANGE, changeDonate);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshInfo);

            GRMap.GAME_CAMERA.SetActive(true);

            transform.FindChild("close_desc").gameObject.SetActive(false);
            transform.FindChild("objdes").gameObject.SetActive(false);
            transform.FindChild("buy_success").gameObject.SetActive(false);
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
        }
        #region tabcontrlo的相关方法
        void onShowgoods()//商品
        {
            btni = 2;
            getGameObjectByPath("panel_down").SetActive(true);
            getGameObjectByPath("contribute").SetActive(false);
            shop_data.Clear();
            if (nowstate == 0)
                return;
            deletecontain();
            foreach (int i in dic.Keys)
            {
                if (dic[i].type == 1)
                {
                    shop_data.Add(dic[i]);
                    int key = dic[i].itemid;
                    int shop_id = dic[i].id;
                    int money_type = dic[i].money_type;
                    int item_money = dic[i].value;
                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;

                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).item_name;
                    int color = a3_BagModel.getInstance().getItemDataById((uint)key).quality;
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                    objClone.transform.FindChild("bg/Image/price").GetComponent<Text>().text = dic[i].value.ToString();
                    GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).file;
                    icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    if (money_type == 3)
                    {
                        objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(true);
                        objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);
                    }
                    else
                    {
                        objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(true);
                        objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);
                    }

                    objClone.transform.FindChild("bg/surplus_num").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/Image/money").gameObject.SetActive(false);


                    BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/btn").transform);
                    btn.onClick = delegate (GameObject goo)
                    {
                        objsurebuy.SetActive(true);
                        surebuy(shop_id, key, 0, money_type, item_money, 2);
                    };
                    new BaseButton(icon.transform).onClick = (GameObject gos) =>
                    {
                        icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                        close_btn.SetActive(true);

                        List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + key);

                        foreach (var x in xml)
                        {
                            string ss = x.getString("desc");
                            text1.text = StringUtils.formatText(ss);
                            text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)key) + ContMgr.getCont("ge");
                            text_name.text = x.getString("item_name");
                            int zhuan = x.getInt("use_limit");
                            if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                            else
                                text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");

                            text_name.color = Globle.getColorByQuality(color);
                        }
                    };
                }
            }
            nowstate = 0;
            refreshcontain(contain, image_goodsorbundinggem);
        }
        void onSummonTab()//召唤兽
        {
            btni = 4;
            shop_data.Clear();
            getGameObjectByPath("panel_down").SetActive(true);
            getGameObjectByPath("contribute").SetActive(false);
            if (nowstate == 1)
                return;
            deletecontain();
            foreach (int i in dic.Keys)
            {
                if (dic[i].type == 5)
                {
                    shop_data.Add(dic[i]);
                    int key = dic[i].itemid;
                    int shop_id = dic[i].id;
                    int money_type = dic[i].money_type;
                    int item_money = dic[i].value;
                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;

                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).item_name;
                    int color = a3_BagModel.getInstance().getItemDataById((uint)key).quality;
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                    objClone.transform.FindChild("bg/Image/price").GetComponent<Text>().text = dic[i].value.ToString();
                    GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).file;
                    icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/Image/money").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);

                    objClone.transform.FindChild("bg/surplus_num").gameObject.SetActive(false);
                    BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/btn").transform);
                    btn.onClick = delegate (GameObject goo)
                    {
                        objsurebuy.SetActive(true);
                        surebuy(shop_id, key, 0, money_type, item_money, 5);
                    };
                    new BaseButton(icon.transform).onClick = (GameObject gos) =>
                    {
                        icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                        close_btn.SetActive(true);

                        List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + key);

                        foreach (var x in xml)
                        {
                            text1.text = x.getString("desc");
                            text_name.text = x.getString("item_name");
                            text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)key) + ContMgr.getCont("ge");
                            text_dengji.text = x.getString("use_limit");
                            int zhuan = x.getInt("use_limit");
                            if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                            else
                                text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");


                            text_name.color = Globle.getColorByQuality(color);
                        }
                    };
                }
            }
            nowstate = 1;
            refreshcontain(contain, image_goodsorbundinggem);

        }
        void onShowpacks()//礼包

        {
            btni = 1;
            getGameObjectByPath("panel_down").SetActive(true);
            getGameObjectByPath("contribute").SetActive( false );
            shop_data.Clear();
            deletecontain();
            foreach (int i in dic.Keys)
            {
                if (dic[i].type == 2)
                {
                    if (dic[i].day != -1)
                    {
                        if (A3_SevenDayProxy.tinmesover==true)
                        {
                            continue;
                        }
                        else
                        {
                            if (dic[i].day != A3_SevendayModel.getInstance().thisday)
                            {
                                continue;
                            }

                        }
                    }
                    shop_data.Add(dic[i]);
                    int key = dic[i].itemid;
                    int shop_id = dic[i].id;
                    int money_type = dic[i].money_type;
                    int item_money = dic[i].value;
                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;
                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).item_name;
                    int color = a3_BagModel.getInstance().getItemDataById((uint)key).quality;
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                    objClone.transform.FindChild("bg/Image/price").GetComponent<Text>().text = dic[i].value.ToString();
                    GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).file;
                    icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);

                    objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/Image/money").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);

                    objClone.transform.FindChild("bg/surplus_num").gameObject.SetActive(true);

                    objClone.transform.FindChild( "bg/surplus_num" ).GetComponent<Text>().text = ContMgr.getCont( "shop_a3_todaynum" ) + dic[ i ].limiteD + "/" + dic[ i ].limiteNum + ")";

                    if ( dic[ i ].isover == true )
                    {
                        objClone.transform.FindChild( "bg/surplus_num" ).GetComponent<Text>().text = ContMgr.getCont( "shop_a3_buycsxg" ) + dic[ i ].limiteD + "/" + dic[ i ].limiteNum + ")";
                    }

                    BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/btn").transform);
                    btn.onClick = delegate (GameObject goo)
                    {
                        if ( dic[ i ].limiteD == 0  && dic[ i ].isover == false )
                        { flytxt.instance.fly( ContMgr.getCont( "shop_a3_txt" ) , 1 ); return; }

                        if ( dic[ i ].limiteD == 0  && dic[ i ].isover == true )
                        { flytxt.instance.fly( ContMgr.getCont( "shop_a3_notbuy" ) , 1 ); return; }


                        objsurebuy.SetActive(true);
                        surebuy(shop_id, key, Shop_a3Model.getInstance().itemsdic[shop_id].limiteD, money_type, item_money, 2);
                    };

                    new BaseButton(icon.transform).onClick = (GameObject gos) =>
                    {
                        icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                        close_btn.SetActive(true);

                        List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + key);

                        foreach (var x in xml)
                        {
                            text1.text = x.getString("desc");
                            text_name.text = x.getString("item_name");
                            text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)key) + ContMgr.getCont("ge");
                            text_dengji.text = x.getString("use_limit");
                            int zhuan = x.getInt("use_limit");
                            if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                            else
                                text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");
                            text_name.color = Globle.getColorByQuality(color);
                        }
                    };

                    havePurchase[ i ] =objClone;
                }
            }
            nowstate = -1;
            refreshcontain(contain, image_goodsorbundinggem);
          
        }
        void onLegion()//军团商店
        {
            shop_data.Clear();
            getGameObjectByPath("panel_down").SetActive(false);
            getGameObjectByPath("contribute").SetActive(true);
            deletecontain();
            //=================
            foreach (int i in dic.Keys)
            {
                if (dic[i].type == 6)
                {
                    shop_data.Add(dic[i]);
                    int key = dic[i].itemid;
                    int shop_id = dic[i].id;
                    int money_type = dic[i].money_type;
                    int item_money = dic[i].value;//价格
                    string name = dic[i].itemName;
                    int limitnum = dic[i].limiteNum;
                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;
                    objClone.SetActive(true);
                    objClone.name = shop_id.ToString();
                    objClone.transform.SetParent(contain.transform, false);

                    objClone.transform.FindChild("bg/name").GetComponent<Text>().text = name;
                    int color = a3_BagModel.getInstance().getItemDataById((uint)key).quality;
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                    objClone.transform.FindChild("bg/Image/price").GetComponent<Text>().text = dic[i].value.ToString();

                    string strss = ContMgr.getCont("shop_a3_dhnum") + Shop_a3Model.getInstance().itemsdic[i].limiteD + "/" + limitnum + ")";
                    objClone.transform.FindChild("bg/surplus_num").GetComponent<Text>().text = strss;

                    if (money_type == 10)//贡献值
                    {
                        objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(true);
                    }

                    GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).file;
                    icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);

                    objClone.transform.FindChild("bg/surplus_num").gameObject.SetActive(true);
                    havePurchase[i] = objClone;

                    new BaseButton(objClone.transform.FindChild("bg/btn").transform).onClick = (GameObject goo) =>
                    {
                        if (dic[i].limiteD == 0)
                        { flytxt.instance.fly(ContMgr.getCont("shop_a3_txt"), 1); return; }
                        objsurebuy.SetActive(true);
                        print("shop_id:" + shop_id);
                        surebuy(shop_id, key, Shop_a3Model.getInstance().itemsdic[shop_id].limiteD, money_type, item_money, 6);
                    };
                    new BaseButton(icon.transform).onClick = (GameObject gos) =>
                    {
                        icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                        close_btn.SetActive(true);

                        List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + key);
                        foreach (var x in xml)
                        {
                            string ss = x.getString("desc");
                            text1.text = StringUtils.formatText(ss);
                            text_name.text = x.getString("item_name");
                            text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)key) + ContMgr.getCont("ge");
                            text_dengji.text = x.getString("use_limit");
                            int zhuan = x.getInt("use_limit");
                            if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                            else
                                text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");
                            text_name.color = Globle.getColorByQuality(color);
                        }
                    };
                }
            }
            int nameid;
            int s = 0;
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                nameid = int.Parse(contain.transform.GetChild(s).name);
                if (Shop_a3Model.getInstance().itemsdic[nameid].limiteD == 0)
                    contain.transform.GetChild(s).transform.SetAsLastSibling();
                else
                    s++;
            }
            //=================
            nowstate = 5;
        }
        void onShowbundinggem()//绑定宝石
        {
            btni = 3;
            shop_data.Clear();
            getGameObjectByPath("panel_down").SetActive(true);
            getGameObjectByPath("contribute").SetActive(false);
            if (nowstate == 3)
                return;
            deletecontain();
            foreach (int i in dic.Keys)
            {
                if (dic[i].type == 3)
                {
                    shop_data.Add(dic[i]);
                    int key = dic[i].itemid;
                    int shop_id = dic[i].id;
                    int money_type = dic[i].money_type;
                    int item_money = dic[i].value;

                    GameObject objClone = GameObject.Instantiate(image_goodsorbundinggem) as GameObject;
                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).item_name;
                    int color = a3_BagModel.getInstance().getItemDataById((uint)key).quality;
                    objClone.transform.FindChild("bg/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                    objClone.transform.FindChild("bg/Image/price").GetComponent<Text>().text = dic[i].value.ToString();
                    objClone.transform.FindChild("bg/surplus_num").GetComponent<Text>().text = ContMgr.getCont("shop_a3_todaynum") + Shop_a3Model.getInstance().itemsdic[i].limiteD + "/" + Shop_a3Model.getInstance().itemsdic[i].limiteNum + ")";
                    if (dic[i].limiteD == 0)
                    {
                        objClone.transform.FindChild("buy_over").gameObject.SetActive(false);

                    }
                    if (money_type == 3)
                    {
                        objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(true);
                        objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);
                    }
                    else
                    {
                        objClone.transform.FindChild("bg/Image/gold").gameObject.SetActive(false);
                        objClone.transform.FindChild("bg/Image/bangdingbaoshi").gameObject.SetActive(true);
                        objClone.transform.FindChild("bg/Image/contribute").gameObject.SetActive(false);
                    }
                    GameObject icon = objClone.transform.FindChild("bg/icon").gameObject;
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic[i].itemid).file;
                    icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    objClone.transform.FindChild("bg/surplus_num").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/Image/money").gameObject.SetActive(false);
                    BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/btn").transform);
                    btn.onClick = delegate (GameObject goo)
                    {
                        if (dic[i].limiteD == 0)
                        { flytxt.instance.fly(ContMgr.getCont("shop_a3_txt"), 1); return; }
                        objsurebuy.SetActive(true);
                        print("shop_id:" + shop_id);
                        surebuy(shop_id, key, Shop_a3Model.getInstance().itemsdic[shop_id].limiteD, money_type, item_money, 2);
                    };
                    havePurchase[i] = objClone;
                    new BaseButton(icon.transform).onClick = (GameObject gos) =>
                    {
                        icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                        close_btn.SetActive(true);

                        List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + key);
                        foreach (var x in xml)
                        {
                            string ss = x.getString("desc");
                            text1.text = StringUtils.formatText(ss);
                            text_name.text = x.getString("item_name");
                            text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)key) + ContMgr.getCont("ge");
                            text_dengji.text = x.getString("use_limit");
                            int zhuan = x.getInt("use_limit");
                            if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                            else
                                text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");
                            text_name.color = Globle.getColorByQuality(color);
                        }
                    };
                }
            }
            nowstate = 3;
            refreshcontain(contain, image_goodsorbundinggem);
        }
        void onShowlimitedactive()//限时活动
        {
            Shop_a3Proxy.getInstance().sendinfo(1, 0, 0);
            btni = 0;
            shop_data.Clear();
            getGameObjectByPath("panel_down").SetActive(true);
            getGameObjectByPath("contribute").SetActive(false);
            if (nowstate == 2)
                return;

            nowstate = 2;


        }
        #endregion
        void onChangeLimited(GameEvent e) => CreateLimitedShopItem(e.data);

        void onDeleteLimited(GameEvent e)
        {
            int activityId = e.data["id"];
            RemoveLimitedItemsByActivityId(activityId);
        }
        void RemoveLimitedItemsByActivityId(int activityId)
        {
            if (limitedActivity.ContainsKey(activityId))
            {
                for (int i = limitedActivity[activityId].Count; i > 0; i--)
                {
                    int idx = limitedActivity[activityId].Dequeue();
                    Destroy(limitedobj[GetLimitedObjIndex(activityId, idx)]);
                    limitedobj.Remove(GetLimitedObjIndex(activityId, idx));
                }
            }
        }
        void onLimited(GameEvent e)
        {
            deletecontain();
            if (e.data["discounts"].Length > 0)
            {
                nullitems.SetActive(false);
                int i = 0;
                for (List<Variant> shopItem = new List<Variant>(e.data["discounts"]._arr); i < shopItem.Count; i++)
                {
                    CreateLimitedShopItem(shopItem[i]);
                }
            }
            else
                nullitems.SetActive(true);
        }
        void CreateLimitedShopItem(Variant v)
        {
            btni = 0;

            shop_data.Clear();
           // if (v["start_time"] != null)
           if (v.ContainsKey ("end_tm"))
            {
                itiemd_time.text = Globle.getStrTime(v["end_tm"], false, false) + ContMgr.getCont("shop_a3_jiezhi");
            }
            //↓商城活动的信息
            int activityId = v["id"];
            string shop_name = v["name"]._str;
            string shop_msg = v["msg"]._str;

            List<Variant> storeItem = new List<Variant>(v["store"]._arr);
            if (!limitedActivity.ContainsKey(activityId))
            {
                limitedActivity.Add(activityId, new Queue<int>());
            }
            else
            {
                RemoveLimitedItemsByActivityId(activityId);
                limitedActivity[activityId].Clear();
            }

            shopDatas data = new shopDatas();
            for (int k = 0; k < storeItem.Count; k++)
            {
                int item_iid = storeItem[k]["id"]._int;
                int item_id = storeItem[k]["tpid"]._int;
                int total_num = storeItem[k]["cnt"];

                data.id = item_iid;
                shop_data.Add(data);    

                int purchase_num = total_num;
                if (storeItem[k].ContainsKey("left_num"))
                    purchase_num = storeItem[k]["left_num"];
                int money_type = storeItem[k]["tp"];
                int price = storeItem[k]["cost"];
                uint discount = storeItem[k]["discount"]._uint;
                int price_src = (int)(Math.Round(value: price / ((float)discount / 100), mode: MidpointRounding.AwayFromZero));//中国的四舍五入
                GameObject objClone = GameObject.Instantiate(image_limitedactive) as GameObject;
                objClone.SetActive(true);
                objClone.transform.SetParent(contain.transform, false);

                objClone.transform.FindChild("bg/bg1/name").GetComponent<Text>().text = a3_BagModel.getInstance().getItemDataById((uint)item_id).item_name;
                int color = a3_BagModel.getInstance().getItemDataById((uint)item_id).quality;
                objClone.transform.FindChild("bg/bg1/name").GetComponent<Text>().color = Globle.getColorByQuality(color);
                objClone.transform.FindChild("bg/bg1/price_old").GetComponent<Text>().text = ContMgr.getCont("shop_a3_old") + price_src;
                objClone.transform.FindChild("bg/bg1/price_now").GetComponent<Text>().text = ContMgr.getCont("shop_a3_now") + price;
                objClone.transform.FindChild("bg/bg1/Image_btn/price").GetComponent<Text>().text = price.ToString();
                objClone.transform.FindChild("bg/bg1/remain_num/num").GetComponent<Text>().text = ContMgr.getCont("shop_a3_shengyu") + purchase_num;
                objClone.transform.FindChild("bg/bg1/Image/Text").GetComponent<Text>().text = ContMgr.getCont("shop_a3_zhekou") + discount + "%";
                GameObject icon = objClone.transform.FindChild("bg/bg1/icon").gameObject;
                string file = a3_BagModel.getInstance().getItemDataById((uint)item_id).file;
                icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                if (money_type == 3)
                {
                    //钻石
                    objClone.transform.FindChild("bg/bg1/Image_btn/gold").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/bg1/Image_btn/bangdingbaoshi").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/money").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/contribute").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/renown").gameObject.SetActive(false);
                }
                else if (money_type == 4)
                {
                    objClone.transform.FindChild("bg/bg1/Image_btn/gold").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/bangdingbaoshi").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/bg1/Image_btn/money").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/contribute").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/renown").gameObject.SetActive(false);
                    //绑定钻石
                }
                else if (money_type == 2)
                {
                    objClone.transform.FindChild("bg/bg1/Image_btn/gold").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/bangdingbaoshi").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/money").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/bg1/Image_btn/contribute").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/renown").gameObject.SetActive(false);
                }
                else if (money_type == 10)
                {
                    objClone.transform.FindChild("bg/bg1/Image_btn/gold").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/bangdingbaoshi").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/money").gameObject.SetActive(false);
                    objClone.transform.FindChild("bg/bg1/Image_btn/contribute").gameObject.SetActive(true);
                    objClone.transform.FindChild("bg/bg1/Image_btn/renown").gameObject.SetActive(false);
                }
                BaseButton btn = new BaseButton(objClone.transform.FindChild("bg/bg1/btn"));
                btn.onClick = delegate (GameObject go)
                {
                    if (purchase_num == 0)
                    { flytxt.instance.fly(ContMgr.getCont("shop_a3_txt1"), 1); return; }
                    objsurebuy.SetActive(true);
                    surebuy(activityId, item_id, purchase_num, money_type, price, 3, item_iid);
                };
                limitedobj[GetLimitedObjIndex(activityId, item_iid)] = objClone;
                limitedActivity[activityId].Enqueue(item_iid);


                new BaseButton(icon.transform).onClick = (GameObject gos) =>
                {
                    icon_small.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    close_btn.SetActive(true);

                    List<SXML> xml = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);

                    foreach (var x in xml)
                    {
                        string ss = x.getString("desc");
                        text1.text = StringUtils.formatText(ss);
                        text_hasnum.text = a3_BagModel.getInstance().getItemNumByTpid((uint)item_id)+ ContMgr.getCont("ge");
                        text_name.text = x.getString("item_name");
                        int zhuan = x.getInt("use_limit");
                        if (zhuan == 0) text_dengji.text = ContMgr.getCont("a3_active_wuxianzhi");
                        else
                            text_dengji.text = x.getString("use_limit") + ContMgr.getCont("zhuan");
                        text_name.color = Globle.getColorByQuality(color);
                    }
                };
            }

            nowstate = 2;
        }
        int itenmoneys = 0;//单价
        int maxnum = 999;//购买的最大个数
        bool canbuy = false;
        void surebuy(int shop_id, int item_id, int max_num, int montytype, int itemmoney, int type, int item_iid = -1)
        {
            bar.value = 0;
            canbuy = true;
            GameObject icon = objsurebuy.transform.FindChild("bg/contain/icon").gameObject;
            if (icon.transform.childCount > 0)
            {
                for (int i = 0; i < icon.transform.childCount; i++)
                {
                    Destroy(icon.transform.GetChild(i).gameObject);
                }
            }
            GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)item_id, false, -1, 1.0f, true);
            item.transform.SetParent(icon.transform, false);
            surebuy_name.text = a3_BagModel.getInstance().getItemDataById((uint)item_id).item_name;
            int color = a3_BagModel.getInstance().getItemDataById((uint)item_id).quality;
            surebuy_name.color = Globle.getColorByQuality(color);
            string ss = a3_BagModel.getInstance().getItemDataById((uint)item_id).desc;

            surebuy_des.text = StringUtils.formatText(ss);
            if (max_num == 0) max_num = a3_BagModel.getInstance().getItemDataById((uint)item_id).maxnum;

            objsurebuy.transform.FindChild("bg/contain/paymoney/moneyIco").gameObject.SetActive(false);
            objsurebuy.transform.FindChild( "bg/contain/paymoney/renown" ).gameObject.SetActive( false );

            if ( montytype == 3 )
            {
                objsurebuy.transform.FindChild( "bg/contain/paymoney/gold" ).gameObject.SetActive( true );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/bangdingbaoshi" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/contribute" ).gameObject.SetActive( false );
                if ( max_num > 0 )
                {
                    if ( PlayerModel.getInstance().gold >= max_num * itemmoney )
                        maxnum = max_num;
                    else
                        maxnum = ( int ) PlayerModel.getInstance().gold / itemmoney;
                }
                else
                {
                    maxnum = ( int ) PlayerModel.getInstance().gold / itemmoney;
                    if ( maxnum > 999 )
                        maxnum = 999;
                }
            }
            else if ( montytype == 4 )
            {
                objsurebuy.transform.FindChild( "bg/contain/paymoney/gold" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/contribute" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/bangdingbaoshi" ).gameObject.SetActive( true );
                if ( max_num > 0 )
                {
                    if ( PlayerModel.getInstance().gift >= max_num * itemmoney )
                        maxnum = max_num;
                    else
                        maxnum = ( int ) PlayerModel.getInstance().gift / itemmoney;
                }
                else
                {
                    maxnum = ( int ) PlayerModel.getInstance().gift / itemmoney;
                    if ( maxnum > 999 )
                        maxnum = 999;
                }
            }
            else if ( montytype == 2 )
            {
                objsurebuy.transform.FindChild( "bg/contain/paymoney/gold" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/bangdingbaoshi" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/contribute" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/moneyIco" ).gameObject.SetActive( true );
                if ( max_num > 0 )
                {
                    if ( PlayerModel.getInstance().money >= max_num * itemmoney )
                        maxnum = max_num;
                    else
                        maxnum = ( int ) PlayerModel.getInstance().money / itemmoney;
                }
                else
                {
                    maxnum = ( int ) PlayerModel.getInstance().money / itemmoney;
                    if ( maxnum > 999 )
                        maxnum = 999;
                }
            }

            else if ( montytype == 5 ) {

                objsurebuy.transform.FindChild( "bg/contain/paymoney/gold" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/bangdingbaoshi" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/contribute" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/moneyIco" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/renown" ).gameObject.SetActive( true );

                if ( max_num > 0 )
                {
                    if ( PlayerModel.getInstance().nobpt >= max_num * itemmoney )
                        maxnum = max_num;
                    else
                        maxnum = ( int ) PlayerModel.getInstance().nobpt / itemmoney;
                }
                else
                {
                    maxnum = ( int ) PlayerModel.getInstance().nobpt / itemmoney;
                    if ( maxnum > 999 )
                        maxnum = 999;
                }
            }

            else if ( montytype == 10 ) //////  军团
            {
                objsurebuy.transform.FindChild( "bg/contain/paymoney/gold" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/bangdingbaoshi" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/moneyIco" ).gameObject.SetActive( false );
                objsurebuy.transform.FindChild( "bg/contain/paymoney/contribute" ).gameObject.SetActive( true );
                if ( max_num > 0 )
                {
                    if ( A3_LegionModel.getInstance().donate >= max_num * itemmoney )
                        maxnum = max_num;
                    else
                        maxnum = ( int ) A3_LegionModel.getInstance().donate / itemmoney;
                }
                else
                {
                    maxnum = ( int ) A3_LegionModel.getInstance().donate / itemmoney;
                    if ( maxnum > 999 )
                        maxnum = 999;
                }
            }

            itenmoneys = itemmoney;
            BaseButton btn = new BaseButton(transform.FindChild("objdes/bg/Button").transform);
            btn.onClick = delegate (GameObject goo)
            {
                if (int.Parse(Inputbuy_num.text) <= 0)
                {
                    if (montytype == 3)
                        flytxt.instance.fly(ContMgr.getCont("shop_a3_enough0"), 1);
                    else if (montytype == 4)
                        flytxt.instance.fly(ContMgr.getCont("shop_a3_enough1"), 1);
                    else if (montytype == 2)
                        flytxt.instance.fly(ContMgr.getCont("shop_a3_enough2"), 1);
                    else if (montytype == 5)
                        flytxt.instance.fly( ContMgr.getCont( "shop_a3_enough4" ) , 1);
                    else if ( montytype == 10 )
                        flytxt.instance.fly( ContMgr.getCont( "shop_a3_enough3" ) , 1 );
                }
                else
                {
                    if (type == 3)
                    {
                        Shop_a3Proxy.getInstance().sendinfo(type, shop_id, int.Parse(Inputbuy_num.text), item_iid);
                        limitedinmfos info = new limitedinmfos();
                        info.item_id = item_id;
                        info.buy_num = int.Parse(Inputbuy_num.text);
                        print("我购买的数量是：" + info.buy_num);
                        limitedinmfo[item_iid] = info;
                    }
                    else
                        Shop_a3Proxy.getInstance().sendinfo(type, shop_id, int.Parse(Inputbuy_num.text));
                }

                objsurebuy.SetActive(false);
            };
        }
        int GetLimitedObjIndex(int activityId, int itemIId) => activityId << 9 + itemIId;


        public void Refresh(int id, int num)//购买后刷新界面(前3个)
        {

            if (havePurchase.ContainsKey(id))
            {
                string value="";

                if ( dic[ id ].isover == true )
                {
                    value = ContMgr.getCont( "shop_a3_buycsxg" );

                }else {

                    value = ContMgr.getCont( "shop_a3_todaynum" );

                    }

                string strs =  value + dic[id].limiteD + "/" + Shop_a3Model.getInstance().itemsdic[id].limiteNum + ")";
                havePurchase[id].transform.FindChild("bg/surplus_num").GetComponent<Text>().text = strs;

            }
            if (dic[id].limiteD == 0)
            {
                havePurchase[id].transform.FindChild("buy_over").gameObject.SetActive(false);
                new BaseButton(havePurchase[id].transform.FindChild("bg/btn")).onClick = (GameObject go) =>
                {
                    if ( dic[ id ].isover == true )
                    { flytxt.instance.fly( ContMgr.getCont( "shop_a3_notbuy" ) , 1 ); return; }

                    flytxt.instance.fly(ContMgr.getCont("shop_a3_txt"), 1);
                };
            }
            objsurebuy.SetActive(false);
            GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)dic[id].itemid, false, -1, 1.0f, true);
            string str = ContMgr.getCont("shop_a3_buy") + a3_BagModel.getInstance().getItemDataById((uint)dic[id].itemid).item_name + "*" + num;
            flytxt.instance.fly(str);
            refreshGold();
            refreshGift();
            refreshCoin();
            refreshRenown();
        }

        public void Refresh_limited(int activityId, int id, int num)//购买后刷新界面(限时抢购)
        {
            limitedobj[GetLimitedObjIndex(activityId, id)].transform.FindChild("bg/bg1/remain_num/num").GetComponent<Text>().text = ContMgr.getCont("shop_a3_shengyu") + num;
            if (num == 0)
                limitedobj[GetLimitedObjIndex(activityId, id)].transform.FindChild("buy_over").gameObject.SetActive(true);
            objsurebuy.SetActive(false);

            string str = ContMgr.getCont("shop_a3_buy") + a3_BagModel.getInstance().getItemDataById((uint)limitedinmfo[id].item_id).item_name + "*" + limitedinmfo[id].buy_num;
            flytxt.instance.fly(str);
            GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)limitedinmfo[id].item_id, false, -1, 1.0f, true);
            objsurebuy.SetActive(false);
            refreshGold();
            refreshGift();
            refreshCoin();
            refreshRenown();
        }


        //时间倒计时：
        int times = -1;
        public void timeCountdown(int time)
        {
            times = time;
            InvokeRepeating("Countdown", 0, 1);
        }
        void Countdown()
        {
            times -= 1;
            if (times <= 0)
            {
                Shop_a3Proxy.getInstance().sendinfo(1, 0, 0);
                times = 86400;
            }
        }


        float reduceoradd;
        void Update()
        {




            //int cc = btni + 1; if (cc > 4) cc = 1;

            //int ls_yu = shop_data.Count % maxNum;
            //int ls_cs = shop_data.Count / maxNum;
            //if (ls_yu % 2 != 0) ls_yu++;
            //float yy = contain.GetComponent<RectTransform>().anchoredPosition.y;
            //float y1 = image_goodsorbundinggem.GetComponent<RectTransform>().sizeDelta.y;
            ////向后划
            //if (yy > 10)
            //{               
            //    if (maxPageNum == 1)
            //    {
            //        if (yy > y1/2 ) tab_btns[btn[cc].transform].doClick();
            //    }
            //    else if (shop_data.Count % maxNum != 0)
            //    {
            //        if (yy > (maxPageNum - 2) * 4 * y1 + (ls_yu / 2) * y1 + 50) tab_btns[btn[cc].transform].doClick();
            //    }
            //    else if (shop_data.Count % maxNum == 0)
            //    {
            //        if (yy > (maxPageNum - 1) * 4 * y1 + y1 + 50) tab_btns[btn[cc].transform].doClick();
            //    }
            //}
            ////向前划
            //if (yy < -50)
            //{
            //    int oo = btni - 1; if (oo <= 0) oo = 4;
            //    tab_btns[btn[oo].transform].doClick();
            //    //yy = 0;
            //    if (maxPageNum != 1)
            //    {
            //        if (shop_data.Count % maxNum == 0) yy = (maxPageNum - 1) * 4 * y1;
            //        else yy = (maxPageNum - 2) * 4 * y1 + ls_yu/2 * y1;
            //    }
            //}
            if (canbuy)
            {

                //只能输入数字规范化处理
                int num = 1;
                if (Inputbuy_num.text != null)
                {
                    int number;
                    if (int.TryParse(Inputbuy_num.text, out number))
                    {
                        num = number;
                        Inputbuy_num.text = num.ToString();
                    }
                    else
                    {
                        Inputbuy_num.text = "1";
                        return;
                    }
                }
                if (Convert.ToInt32(Inputbuy_num.text) > maxnum)
                {
                    Inputbuy_num.text = maxnum.ToString();
                }
                if (Convert.ToInt32(Inputbuy_num.text) > 999)
                {
                    Inputbuy_num.text = Convert.ToString(999);
                }
                reduceoradd = 1 / ((float)maxnum + 1);
                needbuymoney.text = (int.Parse(Inputbuy_num.text) * itenmoneys).ToString();
                if (Inputbuy_num.isFocused)
                {
                    bar.value = float.Parse(Inputbuy_num.text) / maxnum;
                }
                else
                {

                    if (maxnum > 0)
                    {
                        if (bar.value == 0)
                            Inputbuy_num.text = Convert.ToString(1);
                        else if (bar.value == 1)
                            Inputbuy_num.text = Convert.ToString(maxnum);
                        else
                        {
                            Inputbuy_num.text = ((int)(Mathf.Ceil((float)(bar.value * maxnum)))).ToString();
                        }
                    }
                    else
                        bar.value = 1;
                }
                if (maxnum == 1) bar.value = 1;

            }

        }
        public void refreinfo(int title_nowid)
        {
            now_id = title_nowid;

        }
        void onLefts(GameObject go)
        {
            print("reduceoradd是：" + reduceoradd);
            bar.value -= reduceoradd;
        }
        void onRights(GameObject go)
        {
            if (bar.value == 0)
            {
                bar.value += reduceoradd;
            }
            bar.value += reduceoradd;
        }
        void onmin(GameObject go)
        {
            bar.value = 0;
        }
        void onmax(GameObject go)
        {
            bar.value = 1;
        }

        int nownum;
        void deletecontain()
        {
            nullitems.SetActive(false);
            nownum = contain.transform.childCount;
            if (nownum > 0)
            {
                for (int i = nownum; i > 0; i--)
                {
                    DestroyImmediate(contain.transform.GetChild(i - 1).gameObject);
                }
            }
            itiemd_time.text = "";

        }
        void refreshcontain(GameObject contain, GameObject go)
        {
            //GridLayoutGroup glt = contain.GetComponent<GridLayoutGroup>();

            //RectTransform rts = contain.GetComponent<RectTransform>();

            //RectTransform rts_go = go.GetComponent<RectTransform>();
            //int num = contain.transform.childCount - nownum;
            ////glt.cellSize = new Vector2(rts_go.sizeDelta.x, rts_go.sizeDelta.y);
            ////// glt.cellSize = new Vector2(405, 136);
            //if (rts_go.sizeDelta.x > rts.sizeDelta.x / 2)
            //{
            //    rts.sizeDelta = new Vector2(rts_go.sizeDelta.x, num * rts_go.sizeDelta.y);
            //}
            //else
            //{
            //    rts.sizeDelta = new Vector2(rts_go.sizeDelta.x * 2, (int)Math.Ceiling((double)num / 2) * rts_go.sizeDelta.y);
            //    //print("y是多少？" + (int)Math.Ceiling((double)num / 2) * rts_go.sizeDelta.y);
            //}
            //Invoke("setbar", 0.4f);

        }

        void onClose(GameObject go)
        {
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.SHOP_A3);
            if (a3_sports._instantiate  != null && a3_sports._instantiate.goback )
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS);
            }
        }
        void onClose1(GameObject go)
        {
            canbuy = false;
            objsurebuy.SetActive(false);
        }


        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshCoin();
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
        void onClose3(GameObject go)
        {
            buy_success.SetActive(false);
        }
        public void refreshGold()
        {
            money.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            gold.text = PlayerModel.getInstance().gift.ToString();
        }
        public void refreshCoin()
        {
            //Text money = transform.FindChild("item_scroll/money").GetComponent<Text>();
            //money.text = Globle.getBigText(PlayerModel.getInstance().money);
            // coin.text = PlayerModel.getInstance().money.ToString();
            coin.text = Globle.getBigText(PlayerModel.getInstance().money);
        }

        public void refreshRenown()
        {
            //Text money = transform.FindChild("item_scroll/money").GetComponent<Text>();
            //money.text = Globle.getBigText(PlayerModel.getInstance().money);
            // coin.text = PlayerModel.getInstance().money.ToString();
            renown.text = Globle.getBigText((uint)PlayerModel.getInstance().nobpt);
        }

        private void OnChongzhi(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            InterfaceMgr.getInstance().close(InterfaceMgr.SHOP_A3);
        }

    }
    class limitedinmfos
    {
        public int item_id;
        public int buy_num;
    }

}
