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
    class a3_bag : Window
    {
        public static a3_bag Instance;
        private GameObject m_SelfObj;//角色的avatar
        private GameObject scene_Camera;
        private ProfessionAvatar m_proAvatar;
        private GameObject scene_Obj;
        private GameObject itemListView;
        private GridLayoutGroup item_Parent;
        private GridLayoutGroup item_Parent_chushou;
        private GridLayoutGroup item_Parent_fenjie;
        ScrollControler scrollControler;
        public static a3_bag indtans;
        public static a3_bag isshow;

        private Dictionary<uint, GameObject> itemicon = new Dictionary<uint,GameObject>();
        private Dictionary<int, GameObject> equipicon = new Dictionary<int, GameObject>();
        private Dictionary<string ,GameObject> F_equipicon = new Dictionary<string, GameObject>();
        //private Dictionary<int,GameObject> F_currIndexDic = new Dictionary<int, GameObject>() ;
        //private Dictionary<int,GameObject> F_OnShowcurrGoDic = new Dictionary<int, GameObject>() ;
        //private Dictionary<int,int> F_OnShowcurrIndexDic = new Dictionary<int, int>() ;
        private int F_index = 0;
        public static a3_BagItemData F_equipInfo = null;
        public static a3_BagItemData F_weaponInfo = null;
        public static a3_BagItemData _equipInfo = null;
        public static a3_BagItemData _weaponInfo = null;

        private bool is_F_equip = false;

        private Dictionary<uint, GameObject> itemcon_chushou = new Dictionary<uint, GameObject>();
        private Dictionary<uint, GameObject> itemcon_fenjie = new Dictionary<uint, GameObject>();
        private Dictionary<uint, int> cdtype = new Dictionary<uint, int>();
        private GameObject curitem;

        private Dictionary<int, Image> icon_ani = new Dictionary<int, Image>();
        private List<Sprite> ani = new List<Sprite>();

        public GameObject eqpView;
        private Toggle white, green, blue, purple, orange;
        Scrollbar open_bar;
        int cur_num = 1;
        int open_choose_tag = 1;

        public int mojing_num;
        public int shengguanghuiji_num;
        public int mifageli_num;

        //public int GetMoneyNum;

        public bool isbagToCK = false;

        Text mojing;
        Text shengguanghuiji;
        Text mifageli;

        Text Money;

        int shellCount = 0;

        Dictionary<uint, a3_BagItemData> dic_BagItem = new Dictionary<uint, a3_BagItemData>();
        Dictionary<uint, a3_BagItemData> dic_BagItem_shll = new Dictionary<uint, a3_BagItemData>();
        //List<uint> dic_BagItem = new List<uint>();

        public Dictionary<int, int> eqpatt = new Dictionary<int, int>();
        public Dictionary<int, int> Uneqpatt = new Dictionary<int, int>();



       // protected int[] m_num = { 0, 0,0};      //缓存的数量
       // protected float[] m_time = { 0.2f,0.2f,0.6f };  //间隔时间
       // protected Dictionary<int, List<string>> m_txtmap;  //缓存的txt
        private Text qhDashi_text;
        private Text LHlianjie_text;

        public override void init()
        {

            inText();                                                                       //初始化一系列背包属性
            Instance = this;
               indtans = this;
            itemListView = transform.FindChild("item_scroll/scroll_view/contain").gameObject;   //背包里的格子装的东西（排序用的Grid LayOut Group ）
            item_Parent = itemListView.GetComponent<GridLayoutGroup>();
            //item_Parent_chushou = transform.FindChild("piliang_chushou/info_bg/scroll_view/contain").GetComponent<GridLayoutGroup>();
            //item_Parent_fenjie = transform.FindChild("piliang_fenjie/scroll_view/contain").GetComponent<GridLayoutGroup>();
            transform.FindChild("item_scroll/v2_open_bag").gameObject.SetActive(true);
            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onclose;

            //var btn_ride = new BaseButton( transform.FindChild( "btn_ride" ) );

            //btn_ride.onClick = ( go ) =>
            //{
            //    InterfaceMgr.getInstance().close( InterfaceMgr.A3_BAG );
            //    InterfaceMgr.getInstance().ui_async_open( InterfaceMgr.RIDE_A3 );
            //    //A3_RideProxy.getInstance().SendC2S( 4 , "mount" , (uint)((int)A3_RideModel.getInstance().GetRideInfo().mount == 0 ? 1 : 0 ) );
            //};

            //BaseButton btn_fenjieclose = new BaseButton(transform.FindChild("piliang_fenjie/close"));
            //btn_fenjieclose.onClick = onfenjieclose;

            //BaseButton btn_chushouclose = new BaseButton(transform.FindChild("piliang_chushou/close"));
            //btn_chushouclose.onClick = onchoushouclose;

            BaseButton btn_close_open = new BaseButton(transform.FindChild("panel_open/close"));
            btn_close_open.onClick = onCloseOpen;

            BaseButton btn_equip = new BaseButton(transform.FindChild("item_scroll/equip"));
            btn_equip.onClick = onEquipSell;

            BaseButton btn_bag = new BaseButton(transform.FindChild("item_scroll/bag"));
            btn_bag.onClick = onZhengLi;

            BaseButton btn_chushou = new BaseButton(transform.FindChild("item_scroll/chushou"));
            btn_chushou.onClick = onChushou;


            BaseButton btn_open = new BaseButton(transform.FindChild("panel_open/open"));
            btn_open.onClick = onOpenLock;

            BaseButton pet_btn = new BaseButton(transform.FindChild("pet"));
            pet_btn.onClick = onOpenPet;

            BaseButton fashion_btn = new BaseButton(transform.FindChild("fashionButton"));
            BaseButton fashionInfo_close = new BaseButton(transform.FindChild("fashionInfo/close"));
            fashion_btn.onClick = onopenfashion;
            fashionInfo_close.onClick = onOpenFashion;

            F_equipicon["equip"] = transform.FindChild( "fashionInfo/equip" ).gameObject;
            F_equipicon[ "weapon" ] = transform.FindChild( "fashionInfo/weapon" ).gameObject;

            //BaseButton btn_fenjie = new BaseButton(transform.FindChild("piliang_fenjie/info_bg/go"));
            //btn_fenjie.onClick = Sendproxy;

            //BaseButton btn_chushou_do = new BaseButton(transform.FindChild("piliang_chushou/info_bg/go"));
            //btn_chushou_do.onClick = SellItem;

            new BaseButton(transform.FindChild("item_scroll/equip/tishi")).onClick = (GameObject go) =>
            {
                transform.FindChild("item_scroll/equip/tishi").gameObject.SetActive(false);
            };

            new BaseButton(transform.FindChild("QH_dashi/help")).onClick = (GameObject  go) => 
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_QHMASTER);
                transform.FindChild("QH_dashi").gameObject.SetActive(false);
            };
            new BaseButton(transform.FindChild("LH_lianjie/help")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LHLIANJIE);
                transform.FindChild("LH_lianjie").gameObject.SetActive(false);
            };

            new BaseButton (transform.FindChild ("QH_dashi/tach_close")).onClick = new BaseButton (transform.FindChild("QH_dashi/close")).onClick = (GameObject go) =>
            {
                transform.FindChild("QH_dashi").gameObject.SetActive(false);
            };
            new BaseButton(transform.FindChild("LH_lianjie/tach_close")).onClick = new BaseButton(transform.FindChild("LH_lianjie/close")).onClick = (GameObject go) =>
            {
                transform.FindChild("LH_lianjie").gameObject.SetActive(false);
            };

            new BaseButton (transform.FindChild ("oneKeyEqp")).onClick = (GameObject go)=>{
                Debug.LogError("jkkytgj ");

                for (int i= 1; i <= 10;i++)
                {
                    a3_BagItemData data = a3_BagModel.getInstance().getequipToUp(i);
                    if (data != null)
                    {
                        EquipProxy.getInstance().sendChangeEquip(data.id);
                    }
                }
            };

            //mojing = getComponentByPath<Text>("piliang_fenjie/info_bg/mojing/num");
            //shengguanghuiji = getComponentByPath<Text>("piliang_fenjie/info_bg/shenguang/num");
            //mifageli = getComponentByPath<Text>("piliang_fenjie/info_bg/mifa/num");

            //Money = getComponentByPath<Text>("piliang_chushou/money");

            qhDashi_text = transform.FindChild("qhdashi/Text").GetComponent<Text>();
            LHlianjie_text = transform.FindChild("LHlianjie/Text").GetComponent<Text>();
            BaseButton qhDashi = new BaseButton(transform.FindChild("qhdashi"));
            qhDashi.onClick = onOpenDashi;
            new BaseButton(transform.FindChild("LHlianjie")).onClick = onLHlianjie;
            eqpView = this.transform.FindChild("item_scroll/scroll_view").gameObject;

            new BaseButton(transform.FindChild("ig_bg1/equip8")).onClick =
                new BaseButton(transform.FindChild("ig_bg1/equip9")).onClick =
                new BaseButton(transform.FindChild("ig_bg1/equip10")).onClick = openGetJewelry;

            this.getEventTrigerByPath("avatar_touch").onDrag = OnDrag;
            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("item_scroll/scroll_view").GetComponent<ScrollRect>();
            scrollControler.create(scroll);

            string file = "";
            switch (PlayerModel.getInstance().profession)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_2";
                    break;
                case 3:
                    file = "icon_job_icon_3";
                    break;
                case 4:
                    file = "icon_job_icon_4";
                    break;
                case 5:
                    file = "icon_job_icon_5";
                    break;
            }

            for (int i = 0; i < itemListView.transform.childCount; i++)
            {
                if (i >= a3_BagModel.getInstance().curi)
                {
                    GameObject lockig = itemListView.transform.GetChild(i).FindChild("lock").gameObject;
                    lockig.SetActive(true);
                    int tag = i+1;
                    BaseButton btn = new BaseButton(lockig.transform);
                    btn.onClick = delegate(GameObject go) { this.onOpenLock(lockig, tag); };
                }
            }

            open_bar = transform.FindChild("panel_open/Scrollbar").GetComponent<Scrollbar>();
            open_bar.onValueChanged.AddListener(onNumChange);

            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                int tag = i;
                tog.onValueChanged.AddListener(delegate(bool isOn)
                {
                    open_choose_tag = tag;
                    checkNumChange();
                });
            }


            for (int i =1;i<=10;i++)
            {
                icon_ani[i] = this.transform.FindChild("ig_bg1/ain"+i).GetComponent<Image>();
            }

            for (int i = 1;i<=15;i+=2)
            {
                Sprite an = GAMEAPI.ABUI_LoadSprite("uifx_icon_001_icon_001_0" + i);
                ani.Add(an);
            }
        }

        void inText()
        {
            this.transform.FindChild("item_scroll/v2_open_bag/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_1");
            this.transform.FindChild("item_scroll/bag/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_2");
            this.transform.FindChild("item_scroll/equip/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_3");
            this.transform.FindChild("item_scroll/chushou/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_4");
            this.transform.FindChild("item_scroll/equip/tishi/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_5");
            this.transform.FindChild("ig_bg1/txt1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_6");
            this.transform.FindChild("ig_bg1/txt2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_7");
            this.transform.FindChild("ig_bg1/txt3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_8");
            this.transform.FindChild("ig_bg1/txt4").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_9");
            this.transform.FindChild("ig_bg1/txt5").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_10");
            this.transform.FindChild("ig_bg1/txt6").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_11");
            this.transform.FindChild("ig_bg1/txt7").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_12");
            this.transform.FindChild("ig_bg1/txt8").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_13");
            this.transform.FindChild("ig_bg1/txt9").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_14");
            this.transform.FindChild("ig_bg1/txt10").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_15");

            this.transform.FindChild("panel_open/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_16");
            this.transform.FindChild("panel_open/open/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_17");
            this.transform.FindChild("panel_open/open_choose/Toggle1/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_18");
            this.transform.FindChild("panel_open/open_choose/Toggle2/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_bag_19");
            getComponentByPath<Text>("panel_open/title/Text").text = ContMgr.getCont("jiesuo");
        }

        public override void onShowed()
        {


            isshow = this;
            isbagToCK = false;
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITME_SELL, onItemSell);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, onItemChange);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenLockRec);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_PUTON, onEquipOn);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_PUTDOWN, onEquipDown);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            PlayerModel.getInstance().addEventListener(PlayerModel.ON_ATTR_CHANGE, onAttrChange);
            onLoadItem();
            shellCount = this.transform.FindChild("item_scroll/scroll_view/contain").childCount;
            eqpView.SetActive(true);
            initEquipIcon();
            showfenjie_tishi();
            refreshMoney();
            refreshGold();
            refreshGift();
            onOpenLockRec(null);
            onAttrChange(null);
            refreshQHdashi();
            refreshLHlianjie();
            open_choose_tag = 1;
            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                if (i == open_choose_tag)
                    tog.isOn = true;
                else
                    tog.isOn = false;
            }
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            SetAni_Color();

            if(GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);

            createAvatar();
            createAvatar_body();
            setAni();
            BagProxy.getInstance().sendSellItems (4321, 1);

            Toclose = false;

            int num;
            if (A3_VipModel.getInstance().Level > 0)
                num = A3_VipModel.getInstance().vip_exchange_num(14);
            else
                num = 0;

            if (num == 1)
                transform.FindChild("item_scroll/v2_open_bag").gameObject.SetActive(false);


            if (a3_lottery.mInstance?.is_open ?? false)
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_LOTTERY);
            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public override void onClosed()
        {
            //BagProxy.getInstance().removeEventListener(BagProxy.EVENT_LOAD_BAG, onLoadItem);
            isshow = null;
            disposeAvatar();
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITME_SELL, onItemSell);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITEM_CHANGE, onItemChange);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenLockRec);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_PUTON, onEquipOn);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_PUTDOWN, onEquipDown);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            PlayerModel.getInstance().removeEventListener(PlayerModel.ON_ATTR_CHANGE, onAttrChange);
            transform.FindChild("QH_dashi").gameObject.SetActive(false);
            transform.FindChild("LH_lianjie").gameObject.SetActive(false);
            foreach (GameObject go in itemicon.Values)
            {   
                go.transform.parent.gameObject.SetActive( true );
                Destroy(go);
            }
            itemicon.Clear();
            cdtype.Clear();
            //onfenjieclose(null);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);

            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            //if (a3_itemLack.intans && a3_itemLack.intans.closewindow != null)
            //{
            //    if (Toclose)
            //    {
            //        InterfaceMgr.getInstance().open(a3_itemLack.intans.closewindow);
            //        a3_itemLack.intans.closewindow = null;
            //        Toclose = false;
            //    }
            //    else
            //    {
            //        if (!a3_itemLack.intans.noclear)
            //            a3_itemLack.intans.closewindow = null;
            //    }
            //}
            InterfaceMgr.getInstance().itemToWin(Toclose,this.uiName);
            if (a3_lottery.mInstance?.is_open ?? false)            
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY);
            //if (uiData?.Count > 0)
            //    (uiData[0] as Action)?.Invoke();

            onCloseFashion();
        }
        bool Toclose = false;
        int n = 0;
        float waitTime = 0.5f;
        bool isAtt = false;
        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
            if (a3_BagModel.getInstance().getItemCds() == null) return;

            if (a3_BagModel.getInstance().getItemCds().Count > 0)
            {
                foreach(int type in a3_BagModel.getInstance().getItemCds().Keys)
                {
                    foreach (uint id in cdtype.Keys)
                    {
                        if (type == cdtype[id])
                        {
                            if (a3_BagModel.getInstance().getItemCds()[type] <= 0)
                            {
                                itemicon[id].transform.FindChild("cd").gameObject.SetActive(false);
                                itemicon[id].transform.FindChild("cd_bar").gameObject.SetActive(false);
                            }
                            else
                            {
                                itemicon[id].transform.FindChild("cd").gameObject.SetActive(true);
                                itemicon[id].transform.FindChild("cd_bar").gameObject.SetActive(true);
                                itemicon[id].transform.FindChild("cd").GetComponent<Text>().text = ((int)a3_BagModel.getInstance().getItemCds()[type]).ToString();
                                if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                                {
                                    itemicon[id].transform.FindChild("cd_bar/cd_bar").GetComponent<Image>().fillAmount =
                                        a3_BagModel.getInstance().getItemCds()[type] / a3_BagModel.getInstance().getItems()[id].confdata.cd_time;
                                }
                            }
                        }
                    }
                }
            }

            //if (a3_EquipModel.getInstance().active_eqp.Count > 0)
            //{
            //    isAtt = true;
            //}
            //if (isAtt)
            //{
            //    foreach (int i in icon_ani.Keys)
            //    {
            //        if (a3_EquipModel.getInstance().active_eqp.ContainsKey(i))
            //        {
            //            icon_ani[i].gameObject.SetActive(true);
            //            icon_ani[i].sprite = ani[n];
            //        }
            //        else
            //        {
            //            icon_ani[i].gameObject.SetActive(false);
            //        }
            //    }
            //    waitTime -= Time.deltaTime;
            //    if (waitTime<=0)
            //    {
            //        n++;
            //        waitTime = 0.1f;
            //    }
            //    if (n >= ani.Count)
            //    {
            //        n = 0;
            //    }
            //    if (a3_EquipModel.getInstance().active_eqp.Count <= 0)
            //    {
            //        isAtt = false;
            //    }
            //}
        }



        void showfenjie_tishi()
        {
            if (a3_BagModel.getInstance().curi - a3_BagModel.getInstance().getItems().Count <= 5)
            {
                transform.FindChild("item_scroll/equip/tishi").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("item_scroll/equip/tishi").gameObject.SetActive(false);
            }
        }
        void setAni()
        {
            foreach (int i in icon_ani.Keys)
            {
                if (a3_EquipModel.getInstance().active_eqp.ContainsKey(i)) { icon_ani[i].gameObject.SetActive(true); }
                else { icon_ani[i].gameObject.SetActive(false); }
            }
        }


        public void  SetAni_Color()
        {
            foreach (int type in a3_EquipModel.getInstance().getEquipsByType().Keys)
            {
                Color col = new Color();
                switch (a3_EquipModel.getInstance().getEquipsByType()[type].equipdata.attribute)
                {
                    case 1:
                        col = new Color(0f,0.47f,0f);
                        break;
                    case 2:
                        col = new Color(0.68f,0.26f,0.03f);
                        break;
                    case 3:
                        col = new Color(0.76f,0.86f,0.33f);
                        break;
                    case 4:
                        col = new Color(0.97f,0.11f,0.87f);
                        break;
                    case 5:
                        col = new Color(0.17f,0.18f,0.57f);
                        break;
                }
                if(icon_ani.ContainsKey( type ) )
                icon_ani[type].GetComponent<Image>().color = col;
            }
        }

        public void refreshQHdashi()
        {
            Dictionary<uint, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquips();
            int minlvl = 0;
            bool frist = true;
            if (equips.Count < 10)
                minlvl = 0;
            else
            {
                foreach (uint i in equips.Keys)
                {
                    if (frist)
                    {
                        minlvl = equips[i].equipdata.intensify_lv;
                        frist = false;
                    }
                    if (equips[i].equipdata.intensify_lv < minlvl)
                        minlvl = equips[i].equipdata.intensify_lv;
                }
            }
            qhDashi_text.text = minlvl.ToString();
            frist = true;
        }

        public void refreshLHlianjie()
        {
            LHlianjie_text.text = a3_EquipModel.getInstance().active_eqp.Count.ToString();
        }

        void onclose(GameObject go)
        {
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
        }
        protected void refreshScrollRect()
        {
            int num = itemListView.transform.childCount;
            if (num <= 0)
                return;
            float height = itemListView.GetComponent<GridLayoutGroup>().cellSize.y; // item_Parent.cellSize.y;
            float space = itemListView.GetComponent<GridLayoutGroup>().spacing.y;
            int row = (int)Math.Ceiling((double)num / 5);

            RectTransform rect = itemListView.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0.0f, row * (height+space ));
        }

        public void initEquipIcon()
        {
            for (int i = 1; i <= 10; i++)
            {
                GameObject go = transform.FindChild("ig_bg1/txt"+ i).gameObject;
                if (go.transform.childCount > 0)
                {
                    Destroy(go.transform.GetChild(0).gameObject);
                }
            }

            Dictionary<int, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquipsByType();
            foreach (int i in equips.Keys)
            {
                a3_BagItemData data = equips[i];
                CreateEquipIcon(data);
                switch ( equips[ i ].confdata.equip_type )
                {
                    case 3:
                    _equipInfo = equips[ i ];
                    break;
                    case 11:
                    F_equipInfo = equips[ i ];
                    break;
                    case 6:
                    _weaponInfo = equips[ i ];
                    break;
                    case 12:
                    F_weaponInfo = equips[ i ];
                    break;
                }

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
        void onAttrChange(GameEvent e)
        {
            //Text value1 = transform.FindChild("info/value1").GetComponent<Text>();
            //value1.text = PlayerModel.getInstance().max_attack.ToString();

            //Text value2 = transform.FindChild("info/value2").GetComponent<Text>();
            //value2.text = PlayerModel.getInstance().physics_def.ToString();

            //Text value3 = transform.FindChild("info/value3").GetComponent<Text>();
            //value3.text = PlayerModel.getInstance().max_hp.ToString();
            transform.FindChild("fighting/value").GetComponent<Text>().text = PlayerModel.getInstance().combpt.ToString();
            refresh_equip();
        }
        public void refreshMoney()
        {
            Text money = transform.FindChild("item_scroll/money").GetComponent<Text>() ;
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("item_scroll/stone").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("item_scroll/bindstone").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
        }


        //public void flyAttChange(Dictionary<uint,int> att)
        //{
        //    foreach (uint id in att.Keys)
        //    {
        //        string text;
        //        if (att[id] > 0)
        //        {
        //            text =  Globle.getAttrAddById((int)id, att[id],true) ;
        //            fly(text,2);
        //        }
        //        //else if (att[id] < 0)
        //        //{
        //        //    text = "<color=#f90e0e>" + Globle.getAttrAddById((int)id, att[id] * (-1),false) + "</color>";
        //        //    fly(text, 2);
        //        //}
        //    }
        //}

        //void fly( string text,int tag =0 )
        //{

        //    GameObject item;
        //    GameObject txtclone;
        //    if (m_num[tag] > 0 && m_time[tag] > 0)
        //    {
        //        m_txtmap[tag].Add(text);
        //    }
        //    if (tag == 0)
        //    {
        //        item = transform.FindChild("flytext/txt_3_equ1").gameObject;
        //        txtclone = ((GameObject)GameObject.Instantiate(item));
        //        //txtclone.gameObject.SetActive(true);
        //        txtclone.transform.FindChild("txt").GetComponent<Text>().text = text;
        //        txtclone.transform.SetParent(item.transform.parent, false);
        //        Tweener tween1 = txtclone.transform.DOLocalMoveY(100, 1f).SetDelay(Mathf.Max(0, m_num[0] - 1) * m_time[0]).OnStart(() => {
        //            if (m_txtmap[0].Count > 0)
        //            {
        //                m_txtmap[0].RemoveAt(0);
        //                m_num[0]--;
        //            }
        //            txtclone.gameObject.SetActive(true);
        //        }); ;
        //        // tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //        tween1.OnComplete(delegate ()
        //        {
        //            Destroy(txtclone);
        //        });
        //    }
        //    else if (tag == 1)
        //    {
        //        item = transform.FindChild("flytext/txt_3_equ2").gameObject;
        //        txtclone = ((GameObject)GameObject.Instantiate(item));
        //        //txtclone.gameObject.SetActive(true);
        //        txtclone.transform.FindChild("txt").GetComponent<Text>().text = text;
        //        txtclone.transform.SetParent(item.transform.parent, false);
        //        Tweener tween2 = txtclone.transform.DOLocalMoveY(100, 1f).SetDelay(Mathf.Max(0, m_num[1] - 1) * m_time[1]).OnStart(() => {
        //            if (m_txtmap[1].Count > 0)
        //            {
        //                m_txtmap[1].RemoveAt(0);
        //                m_num[1]--;
        //            }
        //            txtclone.gameObject.SetActive(true);
        //        }); ;
        //        //tween2 = txtclone.transform.FindChild("txt").DOLocalMoveY(-100, 2.5f);
        //        tween2.OnComplete(delegate ()
        //        {
        //            Destroy(txtclone);
        //        });
        //    }
        //    else if (tag == 2)
        //    {
        //        item = transform.FindChild("flytext/txt_3_equ3").gameObject;
        //        txtclone = ((GameObject)GameObject.Instantiate(item));
        //        //txtclone.gameObject.SetActive(true);
        //        txtclone.transform.FindChild("txt").GetComponent<Text>().text = text;
        //        txtclone.transform.SetParent(item.transform.parent, false);
        //        Tweener tween1 = txtclone.transform.DOLocalMoveY(100, 1f).SetDelay(Mathf.Max(0, m_num[2]) * m_time[2]).OnStart(() => {
        //            //debug.Log("KKKK"+ m_txtmap[2].Count);
        //            if (m_txtmap[2].Count > 0)
        //            {
        //                m_txtmap[2].RemoveAt(0);
        //                m_num[2]--;
        //            }
        //            txtclone.gameObject.SetActive(true);
        //        }); ;
        //        tween1.OnComplete(delegate ()
        //        {
        //            Destroy(txtclone);
        //        });
        //    }
        //    m_num[tag]++;
        //}



        public void onLoadItem()
        {
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems(true);
            int i = 0;
            foreach (a3_BagItemData item in items.Values)
            {
                CreateItemIcon(item,i);
                i++;
            }

            refreshScrollRect();
        }

        //List<uint> addid = new List<uint>();
        void onItemChange(GameEvent e)
        {
            Variant data = e.data;
            debug.Log("更换装备背包：" + data.dump());
            if (data.ContainsKey("add"))
            {
                if (a3_BagModel.getInstance().getItems().Count > a3_BagModel.getInstance().curi)
                {
                    //addid.Clear();
                    //foreach (Variant item in data["add"]._arr)
                    //{
                    //    addid.Add(item["id"]);
                    //}
                }
                else
                {
                    foreach (Variant item in data["add"]._arr)
                    {
                        uint id = item["id"];
                        if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                        {
                            a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                            //if ( one.confdata.equip_type == 12 || one.confdata.equip_type == 11 )
                            //{
                            //    debug.Log( "====================: " +F_index );
                            //    if ( is_F_equip )
                            //        F_index++;
                            //    CreateItemIcon( one , F_index!=0? F_index-1 : 0);
                            //}
                            //else
                            //{
                                CreateItemIcon( one , itemicon.Count );
                            //}
                            
                        }
                    }
                }
                //refreshScrollRect();
            }
            if (data.ContainsKey("modcnts"))
            {
                foreach (Variant item in data["modcnts"]._arr)
                {
                    uint id = item["id"];
                    if (itemicon.ContainsKey(id))
                    {
                        itemicon[id].transform.FindChild("num").GetComponent<Text>().text = item["cnt"];
                        if ((int)item["cnt"] <= 1)
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(false);
                        else
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(true);


                        //a3_ItemData itemData =  a3_BagModel.getInstance().getItemDataById(item["tpid"]);

                        //if (itemData.use_type == 28)
                        //{
                        //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHANGE_NAME);
                        //}
                    }
                }
            }
            if (data.ContainsKey("rmvids"))
            {
                int i = 0;
                foreach (uint itemid in data["rmvids"]._arr)
                {
                    uint id = itemid;

                    if (itemicon.ContainsKey(id))
                    {
                        i++;
                        
                        //if ( is_F_equip && a3_EquipModel.getInstance().getEquips().ContainsKey( id ) && F_currIndexDic.ContainsKey( ( int ) id ) )
                        //{
                        //    a3_BagItemData one = a3_EquipModel.getInstance().getEquips()[id];
                        //    if ( one.confdata.equip_type == 12 || one.confdata.equip_type == 11 )
                        //    {
                        //        itemicon[ id ].SetActive( false );
                        //        itemicon[ id ].transform.SetParent(  F_currIndexDic[ ( int ) id ].transform  , false );

                        //        int currIndex = F_OnShowcurrIndexDic[(int)id];
                        //        debug.Log("--------:" + F_OnShowcurrGoDic.Count);
                        //        foreach ( int key in F_OnShowcurrGoDic.Keys )
                        //        {
                        //            debug.Log( "currIndex:" + currIndex +"--F_OnShowcurrIndexDic[ key ]:" + F_OnShowcurrIndexDic[ key ] );
                        //            if ( F_OnShowcurrIndexDic[ key ] > currIndex )
                        //            {
                        //                int bourn = F_OnShowcurrIndexDic[ key ] - 1;
                        //                F_OnShowcurrGoDic[ key ].transform.SetParent( item_Parent.transform.GetChild( bourn ).transform );
                        //                F_OnShowcurrGoDic[ key ].transform.localPosition = new Vector3( 0 , 0 , 0 );
                        //                F_OnShowcurrIndexDic[ key ] = bourn;

                        //            }
                        //        }

                        //        F_index--;

                        //    }

                        //}

                        GameObject go = itemicon[id].transform.parent.gameObject;
                        Destroy(go);
                        itemicon.Remove(id);
                        if (cdtype.ContainsKey(id))
                            cdtype.Remove(id);

                        //if ( F_currIndexDic.ContainsKey( ( int ) id ) )
                        //    F_currIndexDic.Remove( ( int )id );

                        GameObject item = transform.FindChild("item_scroll/scroll_view/icon").gameObject;
                        GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
                        itemclone.SetActive(true);
                        itemclone.transform.SetParent(item_Parent.transform, false);
                        itemclone.transform.SetSiblingIndex(itemicon.Count+i);
                    }
                }
                //refreshScrollRect();
            }
        }

        public void ghuaneqp(a3_BagItemData add, a3_BagItemData rem)
        {
            GameObject goo = null;
            a3_BagModel.getInstance().removeItem(rem.id);
            a3_BagModel.getInstance().addItem(add);
            if (itemicon.ContainsKey(rem.id))
            {
                goo = itemicon[rem.id].transform.parent.gameObject;
                Destroy(itemicon[rem.id].gameObject);
                itemicon.Remove(rem.id);
            }
            if (a3_BagModel.getInstance().getItems().ContainsKey(add.id))
            {
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(add, true, add.num);
                icon.transform.SetParent(goo.transform, false);
                itemicon[add.id] = icon;
                if (add.num <= 1)
                    icon.transform.FindChild("num").gameObject.SetActive(false);

                BaseButton bs_bt = new BaseButton(icon.transform);
                bs_bt.onClick = delegate (GameObject go) { this.onItemClick(icon, add.id); };
            }



        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        public void refresh_equip()
        {
            Dictionary<uint, a3_BagItemData> equip = a3_EquipModel.getInstance().getEquips();
            Dictionary<uint, a3_BagItemData> unequip = a3_BagModel.getInstance().getUnEquips();
            foreach (uint i in unequip.Keys)
            {
                if (!a3_EquipModel.getInstance().checkisSelfEquip(unequip[i].confdata))
                {
                }
                else 
                {
                    if (!a3_EquipModel.getInstance().checkCanEquip(unequip[i].confdata, unequip[i].equipdata.stage, unequip[i].equipdata.blessing_lv))
                    {
                        if (itemicon.ContainsKey(i) &&  itemicon[i]!=null)
                        itemicon[i].transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                    }
                    else 
                    {
                        if (itemicon.ContainsKey(i)&&itemicon[i] != null)
                        itemicon[i].transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
                    }
                }
            }
            foreach (uint j in equip.Keys)
            {
                if (!a3_EquipModel.getInstance().checkisSelfEquip(equip[j].confdata))
                {
                }
                else
                {
                    if (!a3_EquipModel.getInstance().checkCanEquip(equip[j].confdata, equip[j].equipdata.stage, equip[j].equipdata.blessing_lv))
                    {
                        if (equipicon.ContainsKey (equip[j].confdata.equip_type) && equipicon[equip[j].confdata.equip_type] != null)
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

        public void refreshMark(uint id) 
        {
            if (a3_EquipModel.getInstance().getEquipByAll(id).ismark)
                itemicon[id].transform.FindChild("iconborder/ismark").gameObject.SetActive(true);
            else
                itemicon[id].transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
        }
        public void refreshMarkRuneStones(uint id)
        {
            if(a3_BagModel.getInstance().getItems()[id].ismark)
                itemicon[id].transform.FindChild("iconborder/ismark").gameObject.SetActive(true);
            else
                itemicon[id].transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
        }
        void onOpenLockRec(GameEvent e)
        {
            for (int i = 50; i < itemListView.transform.childCount; i++)
            {
                GameObject lockig = itemListView.transform.GetChild(i).FindChild("lock").gameObject;
                if (i >= a3_BagModel.getInstance().curi)
                {
                    lockig.SetActive(true);
                }
                else
                {
                    lockig.SetActive(false);
                }
            }
            showfenjie_tishi();
        }

        void onItemSell(GameEvent e)
        {
            Variant data = e.data;
            uint id = 0;
            if (data.ContainsKey("id"))
                id = data["id"];
            string money = data["earn"];
           //flytxt.instance.fly("获得金钱：" + money);
        }

        void onEquipOn(GameEvent e)
        {
            Variant data = e.data;
            uint id = data["eqpinfo"]["id"];
           
            a3_BagItemData item = a3_EquipModel.getInstance().getEquips()[id];

            if ( item.confdata.equip_type == 11 )
                F_equipInfo = item;
            if ( item.confdata.equip_type == 12 )
                F_weaponInfo = item;
            if ( item.confdata.equip_type == 3 )
                _equipInfo = item;
            if ( item.confdata.equip_type == 6 )
                _weaponInfo = item;


            //if ( F_OnShowcurrGoDic.ContainsKey( ( int ) item.id ) )
            //{
            //    F_OnShowcurrGoDic.Remove( ( int ) item.id ); 
            //}

            //if ( F_OnShowcurrIndexDic.ContainsKey( ( int ) item.id ) )
            //{
            //    F_OnShowcurrIndexDic.Remove( ( int ) item.id );
            //}
            
            if (item.confdata.equip_type == 3 || item.confdata.equip_type == 6 ||item.confdata.equip_type == 11 ||item.confdata.equip_type == 12 )
            {
                if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
                createAvatar_body();
            }
            else {
                seteqp_eff();
                seteqp_eff_new();
            }
            int part_id = item.confdata.equip_type;
            if (equipicon.ContainsKey(part_id))
            {
                Destroy(equipicon[part_id]);
                equipicon.Remove(part_id);
            }
            
            CreateEquipIcon(item);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);

            //换装时更新战斗力对比提示
            Dictionary<uint, a3_BagItemData> unEquips = a3_BagModel.getInstance().getUnEquips();
            foreach (a3_BagItemData one in unEquips.Values)
            {
                if (one.confdata.equip_type == item.confdata.equip_type)
                {
                    if (one.equipdata.combpt > item.equipdata.combpt)
                    {
                        if (itemicon.ContainsKey(one.id))
                        {
                            itemicon[one.id].transform.FindChild("iconborder/is_upequip").gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (itemicon.ContainsKey(one.id))
                        {
                            itemicon[one.id].transform.FindChild("iconborder/is_upequip").gameObject.SetActive(false);
                        }
                    }
                }
            }
            setAni();
            SetAni_Color();
            //refresh_equip();
        }

        public void refOneEquipIcon(uint id)
        {
            a3_BagItemData item = a3_EquipModel.getInstance().getEquips()[id];
            if (item.confdata.equip_type == 3 || item.confdata.equip_type == 6)
            {
                if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
                createAvatar_body();
            }
            //else
            //{
            //    seteqp_eff();
            //}
            int type = item.confdata.equip_type;
            if (equipicon.ContainsKey(type))
            {
                Destroy(equipicon[type]);
                equipicon.Remove(type);
            }
            CreateEquipIcon(item);
        }

        void onEquipDown(GameEvent e)
        {
            Variant data = e.data;
            int part_id = data["part_id"];
            if ( part_id == 12 ) 
            {
                Destroy( F_equipicon[ "weapon" ].GetComponentInChildren<Button>().gameObject );
                F_weaponInfo = null;
            } else if (part_id == 11)
            {
                Destroy( F_equipicon[ "equip" ].GetComponentInChildren<Button>().gameObject );
                F_equipInfo = null;
            }
            else
            {
                equipicon[ part_id ].transform.parent.GetComponent<Text>().enabled = true;
                Destroy( equipicon[ part_id ] );
                equipicon.Remove( part_id );
            }

            if ( part_id == 3 )
                _equipInfo = null;
            if ( part_id == 6 )
                _weaponInfo = null;

            if (part_id == 3 || part_id == 6 || part_id == 11 || part_id == 12)
            {
                if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
                createAvatar_body();
            }
            else {
                seteqp_eff();
                seteqp_eff_new();
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);

            //换装时更新战斗力对比提示
            Dictionary<uint, a3_BagItemData> unEquips = a3_BagModel.getInstance().getUnEquips();
            foreach (a3_BagItemData one in unEquips.Values)
            {
                if (one.confdata.equip_type == part_id)
                {
                    if (itemicon.ContainsKey(one.id))
                    {
                        itemicon[one.id].transform.FindChild("iconborder/is_upequip").gameObject.SetActive(true);
                    }
                    //if (a3_EquipModel.getInstance().getEquipByAll(one.id).ismark)
                    //    itemicon[one.id].transform.FindChild("iconborder/ismark").gameObject.SetActive(true);
                }
                refreshMark(one.id);
            }
            refresh_equip();
            setAni(); 
        }   
        
        void CreateItemIcon(a3_BagItemData data,int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
            icon.transform.SetParent(item_Parent.transform.GetChild(i), false);
            itemicon[data.id] = icon;
            if (data.confdata.cd_type > 0)
            {
                cdtype[data.id] = data.confdata.cd_type;
            }

            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);

            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate(GameObject go) { this.onItemClick(icon, data.id); };

            if ( is_F_equip )
            {
                if ( data.confdata.equip_type == 12 || data.confdata.equip_type == 11 )
                {
                    itemicon[ data.id ].transform.parent.gameObject.SetActive( true );
                }
                else
                {
                    itemicon[ data.id ].transform.parent.gameObject.SetActive( false );
                }
            }
        }

        void CreateItemIcon_chushou(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data,false,data.num);
            icon.transform.SetParent(item_Parent_chushou.transform.GetChild(i),false);
            itemcon_chushou[data.id] = icon;
            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);
            //BaseButton bs_bt = new BaseButton(icon.transform);
            //bs_bt.onClick = delegate (GameObject go) { this.onItemClick(icon, data.id); };
        }

        void CreateItemIcon_fenjie(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            icon.transform.SetParent(item_Parent_fenjie.transform.GetChild(i), false);
            itemcon_fenjie[data.id] = icon;
            BaseButton bs_bt = new BaseButton(icon.transform);
            uint id = data.id;
            bs_bt.onClick = delegate (GameObject go) {
                ArrayList data1 = new ArrayList();
                a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                data1.Add(one);
                data1.Add(this.uiName);
                data1.Add(equip_tip_type.tip_forfenjie);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
            };
        }

        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
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
        void createAvatar_body()
        {
            GameObject obj_prefab;
            A3_PROFESSION eprofession = A3_PROFESSION.None;
            if (SelfRole._inst is P2Warrior)
            {
                eprofession = A3_PROFESSION.Warrior;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");//-213.44f, 0.1f, 0.8f
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P3Mage)
            {
                eprofession = A3_PROFESSION.Mage;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");//-213.48f, 0.19f, 0.5f
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 167, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P5Assassin)
            {
                eprofession = A3_PROFESSION.Assassin;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");//-213.46f, 0.12f, 1.2f
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
            if (SelfRole._inst is P3Mage)
            {
                Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                light_fire.transform.SetParent(cur_r_finger1, false);
            }
            m_proAvatar = new ProfessionAvatar();
            m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_" , EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
            seteqp_eff();
            seteqp_eff_new();
            m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
            m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
            m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
            m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
            m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());
        }

        void seteqp_eff() {
            if(m_proAvatar != null)
            {
                m_proAvatar.clear_oldeff();
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
            }
        }

        void seteqp_eff_new()
        {
            if (m_proAvatar != null)
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
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
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        void OnOpenPet(GameObject go)
        {
            A3_PetModel pm = A3_PetModel.getInstance();
            if (!pm.hasPet())
            {
                flytxt.instance.fly(ContMgr.getCont("a3_bag_nopet"));
                return;
            }

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_YILING);
        }

        void onInfo(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ROLE);
        }

        void CreateEquipIcon(a3_BagItemData data)
        {
            GameObject icon;
            if (data.confdata.equip_type != 8 && data.confdata.equip_type != 9&&data.confdata.equip_type!=10)
            {
                //icon = IconImageMgr.getInstance().createA3EquipIcon(data, 1.0f, true);
                icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            }
            else
            {
                icon = IconImageMgr.getInstance().createA3ItemIcon(data,true);
            }
            IconImageMgr.getInstance().refreshA3EquipIcon_byType(icon, data, EQUIP_SHOW_TYPE.SHOW_INTENSIFYANDSTAGE);

            GameObject parent = null;
            if ( data.confdata.equip_type == 11 ) //时装胸甲
            {
                parent = transform.FindChild( "fashionInfo/equip" ).gameObject;
                icon.transform.FindChild( "inlvl" ).gameObject.SetActive( false );
            } else if ( data.confdata.equip_type == 12 ) //时装武器
            {
                parent = transform.FindChild( "fashionInfo/weapon" ).gameObject;
                icon.transform.FindChild( "inlvl" ).gameObject.SetActive( false );
            }
            else
            {
                parent = transform.FindChild( "ig_bg1/txt" + data.confdata.equip_type ).gameObject;
                parent.GetComponent<Text>().enabled = false;
            }
           
          

            icon.transform.SetParent( parent.transform , false );
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            icon.name = data.id.ToString();
           // icon.transform.FindChild("lvl").gameObject.SetActive(true);
           // icon.transform.FindChild("lvl").GetComponent<Text>().text = data.equipdata.stage.ToString();
            equipicon[data.confdata.equip_type] = icon;

            icon.transform.GetComponent<Image>().color = new Vector4(0,0,0,0);

            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate(GameObject go) { this.onEquipClick(icon, data.id); };
        }

        void onItemClick(GameObject go, uint id)
        {
            a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];

            if (one.isNew)
            {
                one.isNew = false;
                a3_BagModel.getInstance().addItem(one);
                itemicon[id].transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
            }

            if (one.isEquip)
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                data.Add(equip_tip_type.BagPick_tip);
                data.Add(this.uiName);
                curitem = itemicon[id];
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
            }
            else if (one.isSummon)
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                curitem = itemicon[id];
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3TIPS_SUMMON, data);
            }
            else if(one.isrunestone)
            {
                ArrayList data = new ArrayList();          
                data.Add(one);
                data.Add(runestone_tipstype.bag_tip);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RUNESTONETIP, data);
            }
            else if(one.ishallows)
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                data.Add(equip_tip_type.hallowtips);
                data.Add(1);
                curitem = itemicon[id];
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                a3_itemtip.closeWin = InterfaceMgr.A3_BAG;
            }
            else
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                data.Add(equip_tip_type.Bag_tip);
                curitem = itemicon[id];
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                a3_itemtip.closeWin = InterfaceMgr.A3_BAG;
            }
        }
        void onEquipClick(GameObject go, uint id)
        {
            ArrayList data = new ArrayList();
            a3_BagItemData one = a3_EquipModel.getInstance().getEquips()[id];
            data.Add(one);
            data.Add(equip_tip_type.Bag_tip);
            data.Add(this.uiName);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
        }


        void onOpenLock(GameObject go, int tag)
        {
            transform.FindChild("panel_open").gameObject.SetActive(true);
            cur_num = tag - a3_BagModel.getInstance().curi;
            needEvent = false;
            open_bar.value = (float)cur_num / (150 - a3_BagModel.getInstance().curi);
            checkNumChange();
        }

        void onCloseOpen(GameObject go) 
        {
            transform.FindChild("panel_open").gameObject.SetActive(false);
        }
        bool needEvent = true;
        void onNumChange(float rate)
        {
            if (!needEvent)
            {
                needEvent = true;
                return;
            }

            cur_num = (int)Math.Floor(rate * (150- a3_BagModel.getInstance().curi));
            if (cur_num == 0)
                cur_num = 1;
            checkNumChange();
        }
        void checkNumChange()
        {
            transform.FindChild("panel_open/num").GetComponent<Text>().text = cur_num.ToString();
            string str = " ";
            switch (open_choose_tag)
            {
                case 1:
                    // str = string.Format("消耗{0}个钻石开启{1}个格子", 5 * cur_num, cur_num
                    str = ContMgr.getCont("a3_bag_gezi0", new List<string> { (5 * cur_num).ToString(), cur_num.ToString() });
                    break;
                case 2:
                    str = ContMgr.getCont("a3_bag_gezi1", new List<string> { (5 * cur_num).ToString(), cur_num.ToString() });
                    //str = string.Format("消耗{0}个绑定钻石开启{1}个格子", 5 * cur_num, cur_num);
                    break;
            }
            transform.FindChild("panel_open/desc").GetComponent<Text>().text = str;
        }
        void onOpenLock(GameObject go)
        {
            transform.FindChild("panel_open").gameObject.SetActive(false);
            if(open_choose_tag == 1)
                BagProxy.getInstance().sendOpenLock(2,cur_num, true);
            else
                BagProxy.getInstance().sendOpenLock(2,cur_num, false);
        }

        void onOpenPet(GameObject go)
        {
            if (PlayerModel.getInstance().havePet || FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
            {
                InterfaceMgr.getInstance().close(this.uiName);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEW_PET);
            }
            else 
            {
                flytxt.instance.fly(ContMgr.getCont("no_get_pet"));
            }

        }
        void onopenfashion(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_FASHIONSHOW);
        }


        void onOpenFashion(GameObject go)
        {
            GameObject fashionGo = transform.FindChild( "fashionInfo" ).gameObject;
            GameObject fightingGo = transform.FindChild( "fighting" ).gameObject;
            GameObject pitchOnBtn = transform.FindChild( "fashionButton/PitchOn" ).gameObject;

            F_index = 0;
            if ( !fashionGo.activeSelf )
            {
                is_F_equip = true;
                //第一次打开时装装备ui
                fashionGo.SetActive(true);
                fightingGo.SetActive( false );
                pitchOnBtn.SetActive(true);
                int i = 0;
                foreach ( var item in itemicon.Keys )
                {
                    a3_BagItemData one = a3_BagModel.getInstance().getItems()[item];
                    if ( one.confdata.equip_type == 12 || one.confdata.equip_type == 11 )
                    {
                        //F_currIndexDic[ (int)item ] = itemicon[ item ].transform.parent.gameObject;
                        //itemicon[ item ].transform.SetParent( item_Parent.transform.GetChild( F_index ) , false );
                        ////F_OnShowcurrGoDic[ ( int ) item ] = item_Parent.transform.GetChild( F_index ).gameObject;
                        //F_OnShowcurrGoDic[ ( int ) item ] = itemicon[ item ];
                        //F_OnShowcurrIndexDic[ ( int ) item ] = F_index;
                        //F_index++;

                        itemicon[ item ].transform.parent.gameObject.SetActive( true );

                    }
                    else
                    {
                        itemicon[ item ].transform.parent.gameObject.SetActive( false );
                    }

                    i++;

                }

                item_Parent.gameObject.SetActive( false );
                item_Parent.gameObject.SetActive( true );//修改显示bug unity 自身bug  unity自身没显示出来
            }
            else
            {
                //关闭时装装备ui
                //fashionGo.SetActive( false );
                if ( fashionGo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName( "fashionInfoOpen" ) )
                {
                    is_F_equip = false;
                    fashionGo.GetComponent<Animator>().Play( "fashionInfoClose" );
                    fightingGo.SetActive( true );
                    pitchOnBtn.SetActive( false );
                    foreach ( var item in itemicon.Keys )
                    {
                        a3_BagItemData one = a3_BagModel.getInstance().getItems()[item];

                        //if ( one.confdata.equip_type == 12 || one.confdata.equip_type == 11 )
                        //{
                        //    //if ( F_currIndexDic.ContainsKey( ( int ) item ) )
                        //    //{
                        //    //    itemicon[ item ].transform.SetParent(  F_currIndexDic[ ( int ) item ].transform  , false );
                        //    //}
                        //    //else
                        //    //{
                        //    //    itemicon[ item ].transform.SetParent( item_Parent.transform.GetChild( itemicon.Count-1 ) , false );
                        //    //}
                        //    itemicon[ item ].transform.parent.gameObject.SetActive( true );

                        //}
                        //else
                        //{
                        //    itemicon[ item ].transform.parent.gameObject.SetActive( true );
                        //}

                        itemicon[ item ].transform.parent.gameObject.SetActive( true );

                    }
                }
                else
                {
                    //打开时装装备ui
                    is_F_equip = true;
                    fashionGo.GetComponent<Animator>().Play( "fashionInfoOpen" );
                    fightingGo.SetActive( false );
                    pitchOnBtn.SetActive( true );
                    int i = 0;
                    foreach ( var item in itemicon.Keys )
                    {
                        a3_BagItemData one = a3_BagModel.getInstance().getItems()[item];
                        if ( one.confdata.equip_type == 12 || one.confdata.equip_type == 11 )
                        {
                            //F_currIndexDic[ ( int ) item ] = itemicon[ item ].transform.parent.gameObject;
                            //itemicon[ item ].transform.SetParent( item_Parent.transform.GetChild( F_index ) , false );
                            //F_OnShowcurrGoDic[ ( int ) item ] = itemicon[ item ];
                            //F_OnShowcurrIndexDic[ ( int ) item ] = F_index;
                            //F_index++;
                            itemicon[ item ].transform.parent.gameObject.SetActive(true);
                        }
                        else
                        {
                            itemicon[ item ].transform.parent.gameObject.SetActive( false );
                        }

                        i++;

                    }

                    item_Parent.gameObject.SetActive(false);
                    item_Parent.gameObject.SetActive( true );
                }
              
            }
        }
        void onCloseFashion()
        {
            transform.FindChild( "fashionInfo" ).gameObject.SetActive(false);
            transform.FindChild( "fighting" ).gameObject.SetActive(true);
            transform.FindChild( "fashionButton/PitchOn" ).gameObject.SetActive(false);
            is_F_equip = false;

            var F_weaponGo = F_equipicon[ "weapon" ].GetComponentInChildren<Button>();

            if ( F_weaponGo != null )
            {
                Destroy( F_weaponGo.gameObject );
            }

            var F_equipGo = F_equipicon[ "equip" ].GetComponentInChildren<Button>();

            if ( F_equipGo != null )
            {
                Destroy( F_equipGo.gameObject );
            }

        }
        //批量分解加入
        //public void EquipsSureSell(int quality = 0) 
        //{
        //    foreach(uint it in a3_BagModel.getInstance().getUnEquips().Keys)  
        //    {
        //        uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;

        //        if (a3_BagModel.getInstance().getItemDataById(tpid).quality == quality)
        //        {
        //            if (!a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getUnEquips()[it]))
        //            {
        //                continue;
        //            }
        //            if (a3_EquipModel.getInstance().getEquipByAll(it).ismark)
        //            {
        //                continue;
        //            }
        //            if (dic_BagItem.ContainsKey(it))
        //            {
        //                dic_BagItem.Remove(it);
        //            }
        //            dic_BagItem[it] = a3_BagModel.getInstance().getUnEquips()[it];
        //            showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, true);
        //        }
        //    }
        //}


        void erfershatt()
        {
            
        }

        //List<int> item_toShll = new List<int>() {1529,1530,1531,1532,1564,1565,1566,1567,1568,1569,1570,1573,1574,1575,1576,1577,1578,1579};

        ////一键出售加入
        //void SellPutin()
        //{
        //    foreach (uint it in a3_BagModel.getInstance().getUnEquips().Keys)
        //    {
        //        if (a3_BagModel.getInstance().HasBaoshi (a3_BagModel.getInstance().getUnEquips()[it]))
        //        {
        //            continue;
        //        }
        //        if (a3_BagModel.getInstance().getUnEquips()[it].ismark)
        //        {
        //            continue;
        //        }
        //        uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;
        //        if (a3_BagModel.getInstance().getItemDataById(tpid).quality <= 3)
        //        {
        //            if (dic_BagItem_shll.ContainsKey(it))
        //            {
        //                dic_BagItem_shll.Remove(it);
        //            }
        //            int num = a3_BagModel.getInstance().getUnEquips()[it].num;
        //            dic_BagItem_shll[it] = a3_BagModel.getInstance().getUnEquips()[it];
        //            ShowMoneyCount(a3_BagModel.getInstance().getUnEquips()[it].tpid, num,true);
        //        }
        //    }
        //    Dictionary<uint, a3_BagItemData> itemList = a3_BagModel.getInstance().getItems();
        //    foreach (uint it in itemList.Keys)
        //    {
        //        uint tpid = itemList[it].tpid;
        //            if (itemList[it].confdata.use_type ==2 || itemList[it].confdata.use_type == 3)
        //            {
        //                SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
        //                if (PlayerModel.getInstance().up_lvl > xml.getInt("use_limit"))
        //                {
        //                    if (PlayerModel.getInstance().up_lvl == 1) { continue; }
        //                    if (PlayerModel.getInstance().up_lvl == 3)
        //                    {
        //                        if (tpid == 1531)
        //                            continue;
        //                        if (tpid == 1532)
        //                            continue;
        //                    }
        //                    if (dic_BagItem_shll.ContainsKey(it))
        //                    {
        //                        dic_BagItem_shll.Remove(it);
        //                    }
        //                    int num = itemList[it].num;
        //                    dic_BagItem_shll[it] = itemList[it];
        //                    ShowMoneyCount(itemList[it].tpid, num, true);
        //                }
        //            }
        //    }
        //}

        //一键出售去除
        public void sellPutout(uint tpid , int num)
        {

        }

        //显示出售物品
        public void OnLoadTitm_chushou()
        {
            itemcon_chushou.Clear();
            int h = 0;
            if (dic_BagItem_shll.Count > 0)
            {
                int i = 0;
                foreach (a3_BagItemData item in dic_BagItem_shll.Values)
                {
                    CreateItemIcon_chushou(item, i);
                    i++;
                }
                h = dic_BagItem_shll.Count / 6;
                if (dic_BagItem_shll.Count % 6 > 0)
                {
                    h += 1;
                }
            }
            RectTransform con = item_Parent_chushou.gameObject.GetComponent<RectTransform>();
            float childSizeY = item_Parent_chushou.cellSize.y;
            Vector2 newSize = new Vector2(con.sizeDelta.x, childSizeY * h);
            con.sizeDelta = newSize;
        }



        void setfenjieCon()
        {
            int h = 0;
            if (dic_BagItem.Count > 0)
            {
                h = itemcon_fenjie.Count / item_Parent_fenjie.constraintCount;
                if (itemcon_fenjie.Count % item_Parent_fenjie.constraintCount > 0)
                {
                    h += 1;
                }
            }
            RectTransform con = item_Parent_fenjie.gameObject.GetComponent<RectTransform>();
            float childSizeY = item_Parent_fenjie.cellSize.y;
            float spacing = item_Parent_fenjie.spacing.y;
            Vector2 newSize = new Vector2(con.sizeDelta.x, (childSizeY+ spacing) * h);
            con.sizeDelta = newSize;
        }

        //显示分解物品
        int conIndex = 0;
        void OnLoadItem_fenjie()
        {
            if (dic_BagItem.Count > 0)
            {       
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (itemcon_fenjie.ContainsKey (it)) { continue; }
                    CreateItemIcon_fenjie(dic_BagItem[it], conIndex);
                    conIndex++;
                }
            }
            setfenjieCon();
        }

        //分解物品去除
        //public void outItemCon_fenjie(int type = -1,uint id = 0)
        //{
        //    GameObject con = item_Parent_fenjie.transform.parent.FindChild("icon").gameObject;
        //    if (type != -1)
        //    {
        //        foreach (uint it in dic_BagItem.Keys)
        //        {
        //            if (dic_BagItem[it].confdata.quality == type)
        //            {
        //                conIndex--;
        //                Destroy(itemcon_fenjie[it].transform.parent .gameObject);
        //                itemcon_fenjie.Remove(it);
        //                GameObject clon = Instantiate(con).gameObject;
        //                clon.transform.SetParent(item_Parent_fenjie.transform, false);
        //                clon.SetActive(true);
        //                clon.transform.SetAsLastSibling();
        //            }
        //        }
        //    }
        //    else if (id > 0)
        //    {
        //        Destroy(itemcon_fenjie[id].transform.parent.gameObject);
        //        itemcon_fenjie.Remove(id);
        //        dic_BagItem.Remove(id);
        //        showItemNum(a3_BagModel.getInstance().getUnEquips()[id].tpid, false);
        //        GameObject clon = Instantiate(con).gameObject;
        //        clon.transform.SetParent(item_Parent_fenjie.transform, false);
        //        clon.SetActive(true);
        //        clon.transform.SetAsLastSibling();
        //        conIndex--;
        //    }
        //    setfenjieCon();
        //}

        //显示出售获得金币数
        //void ShowMoneyCount(uint tpid , int num ,bool add)
        //{
        //    SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
        //    if (add)
        //    {
        //        GetMoneyNum += (xml.getInt("value")*num);
        //    }
        //    else
        //    {
        //        GetMoneyNum -= (xml.getInt("value")*num);
        //    }
        //    Money.text = GetMoneyNum.ToString();
        //}

        //出售后刷新
        //public void refresh_Sell()
        //{
        //    this.transform.FindChild("piliang_chushou").gameObject.SetActive(false);
        //    dic_BagItem_shll.Clear();
        //    Money.text = 0 + "";
        //    GetMoneyNum = 0;
        //}


        List<Variant> dic_Itemlist = new List<Variant>();
        //开始出售
        void SellItem(GameObject go)
        {
            dic_Itemlist.Clear();
            foreach (uint i in dic_BagItem_shll.Keys)
            {
                Variant item = new Variant();
                item["id"] = i;
                item["num"] = dic_BagItem_shll[i].num;
                dic_Itemlist.Add(item);
            }
            BagProxy.getInstance().sendSellItems(dic_Itemlist);
        }

        //批量分解取出
        //public void EquipsNoSell(int quality = 0)
        //{
        //    List<uint> removelist = new List<uint>();
        //    foreach (uint it in dic_BagItem.Keys)
        //    {
        //        if (dic_BagItem[it].confdata.quality == quality)
        //        {
        //            removelist.Add(it);
        //            showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, false);
        //        }
        //    }
        //    foreach (uint i in removelist) 
        //    {
        //        dic_BagItem.Remove(i);
        //    }
        //}
        //显示分解获得物品数量
        //void showItemNum(uint tpid, bool add)
        //{
        //    SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
        //    List<SXML> xmls = xml.GetNodeList("decompose");
        //    foreach (SXML x in xmls)
        //    {
        //        switch (x.getInt("item")) 
        //        {
        //            case 1540:
        //                if (add)
        //                    mojing_num += x.getInt("num");
        //                else
        //                {
        //                    mojing_num -= x.getInt("num");
        //                }
        //                mojing.text = mojing_num.ToString();
        //                break;
        //            case 1541:
        //                if (add)
        //                    shengguanghuiji_num += x.getInt("num");
        //                else
        //                {
        //                    shengguanghuiji_num -= x.getInt("num");
        //                }
        //                shengguanghuiji.text = shengguanghuiji_num.ToString();
        //                break;
        //            case 1542:
        //                 if (add)
        //                     mifageli_num += x.getInt("num");
        //                else
        //                {
        //                    mifageli_num -= x.getInt("num");
        //                }
        //                 mifageli.text = mifageli_num.ToString();
        //                break;
        //        }
        //    }
        //}

        ////开始分解
        //List<uint> dic_leftAllid = new List<uint>();
        //void Sendproxy(GameObject go) 
        //{  
        //    dic_leftAllid.Clear();
        //    foreach (uint i in dic_BagItem.Keys) 
        //    {
        //        dic_leftAllid.Add(i);
        //    }
        //    EquipProxy.getInstance().sendsell(dic_leftAllid);
        //    onfenjieclose(null);
        //    this.transform.FindChild("piliang_fenjie").gameObject.SetActive(false);
        //}

        //分解后刷新信息
        public void refresh()
        {
            if (mojing_num != 0 && shengguanghuiji_num == 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing"));
            if (mojing_num == 0 && shengguanghuiji_num != 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang"));
            if (mojing_num == 0 && shengguanghuiji_num == 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num != 0 && shengguanghuiji_num != 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + "," + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang"));
            if (mojing_num != 0 && shengguanghuiji_num == 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + "," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num == 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang") + "," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num != 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang") + mifageli_num + ContMgr.getCont("a3_bag_mifa"));

            mojing_num = 0;
            shengguanghuiji_num = 0;
            mifageli_num = 0;
        }

        void onEquipSell(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.PILIANG_FENJIE);
            transform.FindChild("item_scroll/equip/tishi").gameObject.SetActive(false);

        }
        //void onfenjieclose(GameObject go) 
        //{
        //    dic_BagItem.Clear();        
        //    clearCon_fenjie();
        //    conIndex = 0;
        //    mojing.text = 0 + "";
        //    shengguanghuiji.text = 0 + "";
        //    mifageli.text = 0 + "";
        //    this.transform.FindChild("piliang_fenjie").gameObject.SetActive(false);
        //    //refresh();
        //}

        //void onchoushouclose(GameObject go)
        //{
        //    refresh_Sell();
        //}
        void onZhengLi(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
            isbagToCK = true;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WAREHOUSE);
        }

        void clearCon()
        {
            if (itemcon_chushou.Count > 0 )
            {
                foreach (GameObject it in itemcon_chushou.Values)
                {
                    Destroy(it);
                }
            }
        }

        void clearCon_fenjie()
        {
            if (itemcon_fenjie.Count > 0)
            {
                foreach (GameObject it in itemcon_fenjie.Values)
                {
                    Destroy(it);
                }
            }
            itemcon_fenjie.Clear();
        }
        void onChushou(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.PILIANG_CHUSHOU);
           // this.transform.FindChild("piliang_chushou").gameObject.SetActive(true);
            //SellPutin();
//clearCon();
            //OnLoadTitm_chushou();
        }
        void onOpenDashi(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HONOR_POW );
            //transform.FindChild("QH_dashi").gameObject.SetActive(true);
            //set_QH_att();
        }
        void onLHlianjie(GameObject go)
        {
            //InterfaceMgr.getInstance().open(InterfaceMgr.A3_LHLIANJIE);
            transform.FindChild("LH_lianjie").gameObject.SetActive(true);
            set_LH_att();
        }

        void openGetJewelry(GameObject go)
        {
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
            {
                ArrayList l = new ArrayList();
                l.Add(InterfaceMgr.A3_BAG);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_GETJEWELRYWAY, l);
            }
            else {
                flytxt.instance.fly(ContMgr.getCont("a3_bag_lvlock"));
            }
        }
        void set_QH_att()
        {
            Dictionary<uint, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquips();
            int minlvl = 0;
            bool frist = true;
            if (equips.Count < 10)
                minlvl = 0;
            else
            {
                foreach (uint i in equips.Keys)
                {
                    if (frist)
                    {
                        minlvl = equips[i].equipdata.intensify_lv;
                        frist = false;
                    }
                    if (equips[i].equipdata.intensify_lv < minlvl)
                        minlvl = equips[i].equipdata.intensify_lv;
                }
            }
            transform.FindChild ("QH_dashi/lvl").GetComponent<Text>().text = minlvl.ToString();
            int QHlvl = 0;
            SXML dSXML = XMLMgr.instance.GetSXML("intensifymaster.level", "lvl==" + PlayerModel.getInstance().up_lvl);
            List<SXML> qhlist = dSXML.GetNodeList("intensify");
            for (int i =0;i < qhlist.Count;i++)
            {
                if (minlvl >= qhlist[i].getInt("qh") && qhlist.Count <= i + 1)
                {
                    QHlvl = qhlist[i].getInt("qh");
                    break;
                }
                else if (minlvl < qhlist[i].getInt("qh") && i == 0)
                {
                    QHlvl = 0;
                    break;
                }
                else if (minlvl >= qhlist[i].getInt("qh") && qhlist.Count > i + 1 &&  minlvl < qhlist[i + 1].getInt("qh"))
                {
                    QHlvl = qhlist[i].getInt("qh");
                    break;
                }
            }
            SXML attXml = dSXML.GetNode("intensify", "qh==" + QHlvl);
            if (attXml == null)
            {
                transform.FindChild("QH_dashi/attVeiw").gameObject.SetActive(false);
                transform.FindChild("QH_dashi/tishi").gameObject.SetActive(true);
                return;
            }
            transform.FindChild("QH_dashi/attVeiw").gameObject.SetActive(true);
            transform.FindChild("QH_dashi/tishi").gameObject.SetActive(false);
            List<SXML> info = attXml.GetNodeList("att");
            GameObject info_item =transform.FindChild("QH_dashi/attVeiw/info_item").gameObject;
            RectTransform con_info = transform.FindChild("QH_dashi/attVeiw/con").GetComponent<RectTransform>();
            for (int i= 0;i< con_info.childCount;i++ )
            {
                Destroy(con_info.GetChild (i).gameObject);
            }
            foreach (SXML it_info in info)
            {
                GameObject info_clon = Instantiate(info_item) as GameObject;
                Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                info_text.text = "+" + it_info.getInt("value") + Globle.getAttrNameById(it_info.getInt("type"));
                info_clon.SetActive(true);
                info_clon.transform.SetParent(con_info, false);
            }
        }
        void set_LH_att()
        {
            int count = a3_EquipModel.getInstance().active_eqp.Count;
            transform.FindChild("LH_lianjie/lvl").GetComponent<Text>().text = count.ToString();
            SXML dSXML = XMLMgr.instance.GetSXML("activate_fun.activate_num");
            SXML attxml = dSXML.GetNode("num", "cout==" + count);
            if (attxml == null)
            {
                transform.FindChild("LH_lianjie/attVeiw").gameObject.SetActive(false);
                transform.FindChild("LH_lianjie/tishi").gameObject.SetActive(true);
                return;
            }
            transform.FindChild("LH_lianjie/attVeiw").gameObject.SetActive(true);
            transform.FindChild("LH_lianjie/tishi").gameObject.SetActive(false);
            List<SXML> info = attxml.GetNodeList("type");
            GameObject info_item = transform.FindChild("LH_lianjie/attVeiw/info_item").gameObject;
            RectTransform con_info = transform.FindChild("LH_lianjie/attVeiw/con").GetComponent<RectTransform>();
            for (int i = 0; i < con_info.childCount; i++)
            {
                Destroy(con_info.GetChild(i).gameObject);
            }
            foreach (SXML it_info in info)
            {
                GameObject info_clon = Instantiate(info_item) as GameObject;
                Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                info_text.text = Globle.getAttrNameById(it_info.getInt("att_type")) + "+" + it_info.getInt("att_value");
                info_clon.SetActive(true);
                info_clon.transform.SetParent(con_info, false);
            }
        }


    }
}
