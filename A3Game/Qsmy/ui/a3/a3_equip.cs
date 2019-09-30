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
    class BaseEquip : Skin
    {
        public BaseEquip(Transform trans) : base(trans)
        {

        }
        virtual public void onShowed() { }
        virtual public void onClose() { }
    }


    class a3_equip : Window
    {
        public  Dictionary<uint, GameObject> equipicon = new Dictionary<uint, GameObject>();
        TabControl tabCtrl1;
        public int tabIndex
        {
            get
            {
                return tabIdx;
            }
            set
            {
                onTab(value);
            }
        }

        public int tabIndex0
        {
            get
            {
                return tabCtrl1.getSeletedIndex();
            }
            set
            {
                tabCtrl1.setSelectedIndex(value);
                onTab1(tabCtrl1);
            }
        }
        private GameObject[] equipPanel,infoPanel;
       // private GameObject unequipedList;
        private GameObject shellcon;
        private GameObject shellcon_baoshi;
        private uint[] intensify_need_id = new uint[3]; 
        ScrollControler scrollControler1, scrollControler2;
        private GameObject[] shell;
        private GameObject itemListView;
        private GridLayoutGroup item_Parent;
        private Image add_Change;
        private Text qhDashi_text;
        //private GameObject QHDashi;
        private GameObject Inbaoshi;
        private GameObject canUp;
        private GameObject ismaxlvl;
        private Dictionary<a3_ItemData,int> Needobj  = new  Dictionary<a3_ItemData, int>();
        private Transform isthiseqp;
        private Text NextTet;
        bool isAdd = false;
        float addTime = 0.5f;
        float rateTime = 0.0f;
        int addType;
        public a3_BagItemData curChooseEquip;
        int curChooseAttType; //重铸时当前选中的属性id
        int curChooseAttTag = 6; //当前选中的项tag

        int curChooseInheritTag = 1;  //传承类型
        int curChooseInheritUseTag = 1; //传承消耗的物品类型
        uint curInheritId1 = 0; //传承id1；
        uint curInheritId2 = 0; //传承id2;
        public uint curInheritId3 = 0; //镶嵌装备id

        uint curBaoshiId = 0;//镶嵌宝石id
        int outKey;//摘下宝石下标
        uint hcbaoshiId = 0;//合成宝石tpid
        uint hcid = 0; // 合成宝石id
        GameObject isthis;
        private Text hcMoney;
        private Text zxMoney;
         //GameObject[] ani_choose;
		public static a3_equip instance;
        int addTyp = 0;
        GameObject tishi_jl;
        GameObject tishi_cc;
        int tabIdx;
        List<GameObject> tab = new List<GameObject>();
        Dictionary<int, GameObject> baoshi_con = new Dictionary<int, GameObject>();
        Dictionary<int, GameObject> baoshi_con2 = new Dictionary<int, GameObject>();
        GameObject geticon;
        //Animator ani_strength_1;
        Animator ani_strength_2;
        Animator ani_strength_3;
        Animator ani_inherit;
        Animator ani_add_att;
        Animator ani_add_att1;
        Animator ani_Advance_1;
        Animator ani_Advance_2;
        Animator ani_Advance_3;

        Animator add_double_min;
        Animator add_double_big;
        ScrollControler scrollControler;

        public override void init()
        {
            intext();
            addIconHintImage();
            equipPanel = new GameObject[2];
            equipPanel[0] = transform.FindChild("panel_equiped").gameObject;
            equipPanel[1] = transform.FindChild("panel_unequiped").gameObject;
            itemListView = transform.FindChild("panel_unequiped/equip_info/bag_scroll/scroll_view/contain").gameObject;
            item_Parent = itemListView.GetComponent<GridLayoutGroup>();
            isthiseqp = equipPanel[0].transform.FindChild("equip_info/scrollview/this");
            infoPanel = new GameObject[8];
            infoPanel[0] = transform.FindChild("panel_tab1").gameObject;
            infoPanel[1] = transform.FindChild("panel_tab2").gameObject;
            infoPanel[2] = transform.FindChild("panel_tab3").gameObject;
            infoPanel[3] = transform.FindChild("panel_tab4").gameObject;
            infoPanel[4] = transform.FindChild("panel_tab5").gameObject;
            infoPanel[5] = transform.FindChild("panel_tab6").gameObject;
            infoPanel[6] = transform.FindChild("panel_tab7").gameObject;
            infoPanel[7] = transform.FindChild("panel_tab8").gameObject;

            tabCtrl1 = new TabControl();
            tabCtrl1.onClickHanle = onTab1;
            tabCtrl1.create(this.getGameObjectByPath("panelTab1"), this.gameObject);

            for (int i = 0; i < transform.FindChild("panelTab2/con").childCount;i++)
            {
                tab.Add(transform.FindChild("panelTab2/con").GetChild(i).gameObject);
            }

            for (int i =0;i<tab.Count;i++)
            {
                int tag = i;
                new BaseButton(tab[i].transform).onClick = delegate (GameObject go)
                {
                    onTab(tag);   
                };
            }
            shellcon = equipPanel[1].transform.FindChild("equip_info/bag_scroll/scroll_view/contain").gameObject;
            shellcon_baoshi = equipPanel[1].transform.FindChild("equip_info/bag_scroll_baoshi/scroll_view/contain").gameObject;
            scrollControler2 = new ScrollControler();
            ScrollRect scroll2 = equipPanel[1].transform.FindChild("equip_info/bag_scroll/scroll_view").GetComponent<ScrollRect>();
            scrollControler2.create(scroll2);

            isthis = infoPanel[7].transform.FindChild("bg/isthis").gameObject;
            canUp = infoPanel[0].transform.FindChild("canUp").gameObject;
            ismaxlvl = infoPanel[0].transform.FindChild("maxlvl").gameObject;

            ani_strength_2 = infoPanel[0].transform.FindChild("ani_success").GetComponent<Animator>();
            ani_strength_3 = infoPanel[0].transform.FindChild("ani_fail").GetComponent<Animator>();
            ani_inherit = infoPanel[2].transform.FindChild("ani_cc").GetComponent<Animator>();
            ani_add_att = infoPanel[5].transform.FindChild("zj_levelUP").GetComponent<Animator>();
            ani_add_att1 = infoPanel[5].transform.FindChild("ani_zj").GetComponent<Animator>();
            ani_Advance_1 = infoPanel[3].transform.FindChild("ani_lightning").GetComponent<Animator>();
            ani_Advance_2 = infoPanel[3].transform.FindChild("ani_success").GetComponent<Animator>();
            ani_Advance_3 = infoPanel[3].transform.FindChild("ani_fail").GetComponent<Animator>();
            add_double_min = infoPanel[5].transform.FindChild("add_double_min").GetComponent<Animator>();
            add_double_big = infoPanel[5].transform.FindChild("add_double_big").GetComponent<Animator>();

            //qhDashi_text = infoPanel[0].transform.FindChild("qhdashi/Text").GetComponent<Text>();
            Inbaoshi = transform.FindChild("Inbaoshi").gameObject;
            BaseButton Baoshi_close = new BaseButton(Inbaoshi.transform.FindChild("close"));
            Baoshi_close.onClick = (GameObject go) =>
            {
                Inbaoshi.SetActive(false);
                curBaoshiId = 0;
            };
            NextTet = infoPanel[0].transform.FindChild("NextTet/Text").GetComponent<Text>();
            add_Change = infoPanel[5].transform.FindChild("add_lvl").GetComponent<Image>();

            hcMoney = infoPanel[6].transform.FindChild("do/money").GetComponent<Text>();
            zxMoney = infoPanel[7].transform.FindChild("do/money").GetComponent<Text>();

            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onclose;
            
            BaseButton btn_strength = new BaseButton(infoPanel[0].transform.FindChild("btn_do"));
            btn_strength.onClick = onStrength;

            BaseButton btn_change = new BaseButton(infoPanel[1].transform.FindChild("btn_do"));
            btn_change.onClick = onChange;

            BaseButton btn_change_attr = new BaseButton(transform.FindChild("change_panel/btn_yes"));
            btn_change_attr.onClick = onYesChange;

            BaseButton btn_unchange_attr = new BaseButton(transform.FindChild("change_panel/btn_no"));
            btn_unchange_attr.onClick = onNoChange;

            BaseButton btn_advance = new BaseButton(infoPanel[3].transform.FindChild("btn_do"));
            btn_advance.onClick = onAdvance;

            BaseButton btn_addattr = new BaseButton(infoPanel[5].transform.FindChild("btn_do"));
            btn_addattr.onClick = onAddAttr;

            BaseButton btn_do_ten = new BaseButton(infoPanel[5].transform.FindChild("btn_do_ten"));
            btn_do_ten.onClick = onAddAttr_num;

            BaseButton btn_inherit = new BaseButton(infoPanel[2].transform.FindChild("btn_do"));
            btn_inherit.onClick = onInherit;

            //BaseButton qhDashi = new BaseButton(infoPanel[0].transform.FindChild("qhdashi"));
            //qhDashi.onClick = onOpenDashi;

            BaseButton hecheng = new BaseButton(infoPanel[6].transform.FindChild("do"));
            hecheng.onClick = onHeCheng;

            new BaseButton(infoPanel[7].transform.FindChild("do")).onClick = onSendOut;

            new BaseButton (infoPanel[0].transform.FindChild ("help_btn")).onClick = (GameObject go)=>{ infoPanel[0].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[0].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[0].transform.FindChild("help").gameObject.SetActive(false); };

            new BaseButton(infoPanel[7].transform.FindChild("help_btn")).onClick = (GameObject go) => { infoPanel[7].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[7].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[7].transform.FindChild("help").gameObject.SetActive(false); };

            new BaseButton(infoPanel[5].transform.FindChild("help_btn")).onClick = (GameObject go) => { infoPanel[5].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[5].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[5].transform.FindChild("help").gameObject.SetActive(false); };

            new BaseButton(infoPanel[6].transform.FindChild("help_btn")).onClick = (GameObject go) => { infoPanel[6].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[6].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[6].transform.FindChild("help").gameObject.SetActive(false); };

            new BaseButton(infoPanel[2].transform.FindChild("help_btn")).onClick = (GameObject go) => { infoPanel[2].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[2].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[2].transform.FindChild("help").gameObject.SetActive(false); };

            new BaseButton(infoPanel[1].transform.FindChild("help_btn")).onClick = (GameObject go) => { infoPanel[1].transform.FindChild("help").gameObject.SetActive(true); };
            new BaseButton(infoPanel[1].transform.FindChild("help/close")).onClick = (GameObject go) => { infoPanel[1].transform.FindChild("help").gameObject.SetActive(false); };


            tishi_jl = infoPanel[3].transform.FindChild("tishi").gameObject;
            new BaseButton (tishi_jl.transform.FindChild ("yes")).onClick = (GameObject go) =>{ EquipProxy.getInstance().sendAdvance(curChooseEquip.id); tishi_jl.SetActive(false); };
            new BaseButton(tishi_jl.transform.FindChild("no")).onClick = (GameObject go) => { tishi_jl.SetActive(false);};

            tishi_cc = infoPanel[2].transform.FindChild("tishi").gameObject;
            new BaseButton(tishi_cc.transform.FindChild("yes")).onClick = (GameObject go) => {
                if (curChooseInheritUseTag == 1)
                    EquipProxy.getInstance().sendInherit(curInheritId1, curInheritId2, curChooseInheritTag, false);
                else
                    EquipProxy.getInstance().sendInherit(curInheritId1, curInheritId2, curChooseInheritTag, true);
                tishi_cc.SetActive(false); };
            new BaseButton(tishi_cc.transform.FindChild("no")).onClick = (GameObject go) => { tishi_cc.SetActive(false); };

            for (int i= 0;i<3;i++)
            {
                baoshi_con[i] = infoPanel[7].transform.FindChild("bg/icon"+i).gameObject;
            }
            for (int j = 0 ; j < 5; j++ )
            {
                baoshi_con2[j] = infoPanel[6].transform.FindChild("bg/icon" + j).gameObject;
            }
            for (int m = 1;m<= 10; m++)
            {
                eqp_obj[m]= equipPanel[0].transform.FindChild("equip_info/scrollview/con/eqp" + m).gameObject;
            }
            geticon = infoPanel[6].transform.FindChild("bg/Geticon").gameObject;

            initUi();
			instance = this;
			CheckLock();

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("panel_equiped/equip_info/scrollview").GetComponent<ScrollRect>();
            scrollControler.create(scroll);
            scrollControler1 = new ScrollControler();
            ScrollRect scroll1 = equipPanel[1].transform.FindChild("equip_info/bag_scroll_baoshi/scroll_view").GetComponent<ScrollRect>();
            scrollControler1.create(scroll1);

        }
        void intext()
        {
            this.transform.FindChild("panelTab1/btn_equiped/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_1");//身上
            this.transform.FindChild("panelTab1/btn_unequiped/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_2");//背包
            this.transform.FindChild("panelTab2/con/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_3");//强化
            this.transform.FindChild("panelTab2/con/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_4");//重铸
            this.transform.FindChild("panelTab2/con/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_5");// 传承
            this.transform.FindChild("panelTab2/con/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_6");//精炼
            this.transform.FindChild("panelTab2/con/4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_7");//培养
            this.transform.FindChild("panelTab2/con/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_8");//追加
            this.transform.FindChild("panelTab2/con/6/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_9");//宝石
            this.transform.FindChild("panelTab2/con/7/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_10");//镶嵌
            this.transform.FindChild("panel_tab1/btn_do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_11");//强化
            this.transform.FindChild("panel_tab1/btn_goUP/gotext").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_12");//前往精炼
            this.transform.FindChild("panel_tab1/btn_goUP/maxtext").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_13");//已经满级
            this.transform.FindChild("panel_tab1/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_14"));//1,装备强化到+15后，可以进行精炼操作。2,精炼可以大幅度提升装备的属性，并且使装备发光。3,精炼后的装备可以重新强化，每个精炼等级强化的成功概率不同。
            this.transform.FindChild("panel_tab1/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_15");//知道了
            this.transform.FindChild("panel_tab2/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_15");//知道了
            this.transform.FindChild("panel_tab2/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_53"));
            this.transform.FindChild("panel_tab3/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_15");//知道了
            this.transform.FindChild("panel_tab3/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_54"));
            this.transform.FindChild("panel_tab6/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_15");//知道了
            this.transform.FindChild("panel_tab6/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_55"));
            this.transform.FindChild("panel_tab7/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_15");//知道了
            this.transform.FindChild("panel_tab7/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_56"));
            this.transform.FindChild("panel_tab1/image/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_16");//成功率：
            this.transform.FindChild("panel_tab2/btn_do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_17");//重铸
            this.transform.FindChild("panel_tab2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_18");//◆重铸可获取新的属性。
            this.transform.FindChild("panel_tab3/duibi/left/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_19");//传承装备
            this.transform.FindChild("panel_tab3/duibi/right/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_20");//继承装备
            this.transform.FindChild("panel_tab3/cctext").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_21");//传承{n}装备
            this.transform.FindChild("panel_tab3/jctext").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_22"); //继承{n}装备
            this.transform.FindChild("panel_tab3/desc1").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_23"));//◆只有同职业同部位的装备才能相互传承。◆传承后，传承装备的精炼数、强化、追加属性会归0。◆继承装备会获得传承装备的精炼数、强化、追加属性。
            this.transform.FindChild("panel_tab3/btn_do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_24");//传承
            this.transform.FindChild("panel_tab3/tishi/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_25");//传承后,穿戴装备所需人物属性将提升至
            this.transform.FindChild("panel_tab3/tishi/Text (2)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_26");//是否继续传承
            this.transform.FindChild("panel_tab3/tishi/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_27");//是
            this.transform.FindChild("panel_tab3/tishi/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_28");//否
            this.transform.FindChild("panel_tab4/btn_do/maxlvl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_29");//已经满级
            this.transform.FindChild("panel_tab4/btn_do/money/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_30");//精炼
            this.transform.FindChild("panel_tab4/needStrength/maxlvl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_31");//已经满级
            this.transform.FindChild("panel_tab4/needStrength/tostrength").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_32");//前往强化
            this.transform.FindChild("panel_tab4/tishi/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_33");//精炼后,穿戴装备所需人物属性将提升至
            this.transform.FindChild("panel_tab4/tishi/Text (2)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_34");//是否继续精炼
            this.transform.FindChild("panel_tab4/tishi/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_35");//是
            this.transform.FindChild("panel_tab4/tishi/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_36");//否
            this.transform.FindChild("panel_tab6/btn_do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_37");//追加
            this.transform.FindChild("panel_tab6/lvl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_38");//追加等级
            this.transform.FindChild("panel_tab7/do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_39");//合成
            this.transform.FindChild("panel_tab8/do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_40");//摘取
            this.transform.FindChild("panel_tab8/help/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_equip_41"));//1, 宝石镶嵌在不同的装备上，获得的属性加成各有不同。2,镶嵌宝石不需要任何花费，但取下宝石需要消耗一定数量的金币。
            this.transform.FindChild("panel_tab8/help/p1/r/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_42");//提升生命值
            this.transform.FindChild("panel_tab8/help/p1/b/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_43");//提升物理防御
            this.transform.FindChild("panel_tab8/help/p1/y/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_44");//提升魔法防御
            this.transform.FindChild("panel_tab8/help/p2/r/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_45");//提升攻击下限
            this.transform.FindChild("panel_tab8/help/p2/b/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_46");//提升攻击上限
            this.transform.FindChild("panel_tab8/help/p2/y/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_47");//提升无视防御伤害
            this.transform.FindChild("panel_tab8/help/p3/r/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_48");//提升致命攻击
            this.transform.FindChild("panel_tab8/help/p3/b/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_49");//提升闪避
            this.transform.FindChild("panel_tab8/help/p3/y/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_50");//提升命中
            this.transform.FindChild("panel_tab8/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_51");//知道了

            this.transform.FindChild("panel_tab4/limit0").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_52");//知道了

            this.transform.FindChild("panel_tab6/btn_do_ten/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equip_59");
        }


        public override void onShowed()
        {
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_STRENGTH, onEquipStrength);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_CHANGE_ATT, onChangeAtt);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_DO_CHANGE_ATT, onDoChangeAtt);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_ADVANCE, onEquipAdvance);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_GEM_UP, onEquipGem);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_ADDATTR, onEquipAtt);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_INHERIT, onEquipInherit);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_BAOSHI, onEquipBaoshi);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_BAOSHI_HC, onBaoshi_hc);
            transform.SetAsLastSibling();
            shot_eqp();

            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);

            initEquipPanel();
            initBaoshiPanel();
            refreshScrollRect();
            outKey = -1;
            hcbaoshiId = 0;
            hcid = 0;
            refMask();

            if (uiData != null)
            {
                uint id = (uint)uiData[0];
                onClickEquip(equipicon[id], id);
            }
            else
            {
                if (equipicon.Count > 0)
                {
                    onClickEquip(equipicon[equipicon.Keys.First()], equipicon.Keys.First());
                }
            }
            tishi_jl.SetActive(false);
            tishi_cc.SetActive(false);
            refreshMoney();
            refreshGold();
            refreshGift();
            onTab(0);
            //refreshQHdashi();
            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public override void onClosed()
        {
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_STRENGTH, onEquipStrength);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_CHANGE_ATT, onChangeAtt);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_DO_CHANGE_ATT, onDoChangeAtt);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_ADVANCE, onEquipAdvance);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_GEM_UP, onEquipGem);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_ADDATTR, onEquipAtt);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_INHERIT, onEquipInherit);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_BAOSHI, onEquipBaoshi);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_BAOSHI_HC, onBaoshi_hc);
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            Inbaoshi.SetActive(false);
            reset();
            isAdd = false;
            curInheritId1 = 0;
            curInheritId2 = 0;
        }
        Dictionary<int, GameObject> eqp_obj = new Dictionary<int, GameObject>();
        void initUi()
        {
            //强化
            SXML s_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==1");
            if (s_xml != null)
           {//添加强化需要的材料icon
                for (int i = 0; i < 3; i++)
                {
                    intensify_need_id[i] = uint.Parse(s_xml.getString("intensify_material" + (i + 1)).Split(',')[0]);
                }
            }
            BaseButton btn_inherit_remove1 = new BaseButton(infoPanel[2].transform.FindChild("btn_close1"));
            btn_inherit_remove1.onClick = onInheritRemove1;
            BaseButton btn_inherit_remove2 = new BaseButton(infoPanel[2].transform.FindChild("btn_close2"));
            btn_inherit_remove2.onClick = onInheritRemove2;


            //宝石属性
            //for (int i = 1; i <= 3; i++)
            //{
            //    int tag = i;
            //    BaseButton btn_advance = new BaseButton(infoPanel[4].transform.FindChild("stone"+ i +"/btn_do"));
            //    //btn_advance.onClick = delegate(GameObject go) { this.onGemClick(tag); };
            //    EventTriggerListener.Get(btn_advance.gameObject).onDown = delegate(GameObject go) { this.onGemClick(tag); };
            //    EventTriggerListener.Get(btn_advance.gameObject).onExit = delegate(GameObject go) { this.onGemClickExit(tag); };
            //}
        }
        void onTab1(TabControl t)
        {
            equipPanel[0].SetActive(false);
            equipPanel[1].SetActive(false);
            if (t.getSeletedIndex() == 0)
            {
                equipPanel[0].SetActive(true);
            }
            else
            { 
                equipPanel[1].SetActive(true);
            }
        }
        void onTab(int tag)
        {
            if (curChooseEquip.id <= 0)
                return;

            if (curInheritId1 > 0 || curInheritId2 > 0)
            {
                curInheritId1 = 0;
                curInheritId2 = 0;
                refreshUnEquipItem();
            }
            curInheritId3 = curChooseEquip.id;
            tabIdx = tag;
            infoPanel[0].SetActive(false);
            infoPanel[1].SetActive(false);
            infoPanel[2].SetActive(false);
            infoPanel[3].SetActive(false);
            infoPanel[4].SetActive(false);
            infoPanel[5].SetActive(false);
            infoPanel[6].SetActive(false);
            infoPanel[7].SetActive(false);
            for (int i = 0;i<tab.Count;i++)
            {
                tab[i].GetComponent<Button>().interactable = true;
            }
            tab[tabIdx].GetComponent<Button>().interactable = false;
            infoPanel[tabIdx].SetActive(true);

            switch (tabIdx)
            {
                case 0:
                    infoPanel[0].transform.FindChild("help").gameObject.SetActive(false);
                    refreshStrength(curChooseEquip);
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 1:
                    refrenshChange(curChooseEquip);
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 2:
                    refreshInherit();
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 3:
                    refreshAdvance(curChooseEquip);
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 4:
                    //refreshGemAtt(curChooseEquip);
                    //refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    //refeqptext();
                    break;
                case 5:
                    infoPanel[5].transform.FindChild("help").gameObject.SetActive(false);
                    refreshAddAtt(curChooseEquip);
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 6:
                    infoPanel[6].transform.FindChild("help").gameObject.SetActive(false);
                    ref_hc_baoshiMoney();
                    tabCtrl1.setSelectedIndex(1);
                    onTab1(tabCtrl1);
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
                case 7:
                    infoPanel[7].transform.FindChild("help").gameObject.SetActive(false);
                    refrenshChangeBaoshi();
                    refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_COMMON);
                    refeqptext();
                    break;
            }
            clear_baoshi();
            hcbaoshiId = 0;
            hcid = 0;
            Inbaoshi.SetActive(false);
            outKey = -1;
            refMask();
            refeqp();
        }
        //void resetEquipList()
        //{
        //    for (int i = 0; i < unequipedList.transform.childCount; i++)
        //    {
        //        GameObject go = unequipedList.transform.GetChild(i).gameObject;
        //        Destroy(go);
        //    }
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        GameObject go = equipPanel[0].transform.FindChild("equip_info/equip" + i).gameObject;
        //        if (go.transform.childCount > 0)
        //        {
        //            Destroy(go.transform.GetChild(0).gameObject);
        //        }
        //    }
        //}
        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIP);
        }

        void Update()
        {
            //if (isAdd) 
            //{ 
            //    addTime -= Time.deltaTime;
            //    if (addTime < 0)
            //    {
            //        time -= Time.deltaTime;
            //        if (time < 0)
            //        {
            //            rateTime += 0.05f;
            //            addTime = 0.5f - rateTime;
            //            sendGem(addType) ;
            //            time = 0.2f;
            //        }
            //    }
            //}
        }

        bool isCan(a3_BagItemData data1, a3_BagItemData data2)
        {
            foreach (int type in data2.equipdata.gem_att.Keys)
            {
                if (data2.equipdata.gem_att[type] > data1.equipdata.gem_att[type])
                {
                    return true ;
                }
            }
            return false;
       }
        //void refreshEquipIcon(uint id, EQUIP_SHOW_TYPE type = EQUIP_SHOW_TYPE.SHOW_COMMON)
        //{
        //    if (equipicon.ContainsKey(id))
        //    {
        //        a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(id);
        //        if (a3_EquipModel.getInstance().getEquips().ContainsKey(id))
        //        {
        //            IconImageMgr.getInstance().refreshA3EquipIcon_byType(equipicon[id], data, type);
        //        }
        //        else
        //        {
        //            for (int i = 1; i <= 15; i++)
        //            {
        //                equipicon[id].transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);
        //                if (i <= data.equipdata.intensify_lv)
        //                    equipicon[id].transform.FindChild("stars/contain/star" + i).gameObject.SetActive(true);
        //                else
        //                    equipicon[id].transform.FindChild("stars/contain/star" + i).gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //}  
        void rightInfo(a3_BagItemData data1, a3_BagItemData data2)
        {
            infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(true);
            if (data1.equipdata.intensify_lv <= data2.equipdata.intensify_lv)
            {
                infoPanel[2].transform.FindChild("duibi/right/star/Text").GetComponent<Text>().text = " <color=#f90e0e>" + "+" + data2.equipdata.intensify_lv + "</color>";
                //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(false);
            }
            else { infoPanel[2].transform.FindChild("duibi/right/star/Text").GetComponent<Text>().text = "+" + data2.equipdata.intensify_lv; }
            if (data1.equipdata.add_level < data2.equipdata.add_level)
            {
                infoPanel[2].transform.FindChild("duibi/right/4").GetComponent<Text>().text = " <color=#f90e0e>" + ContMgr.getCont("a3_equip_add") + data2.equipdata.add_level + "</color>";
                //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(false);
            }
            else { infoPanel[2].transform.FindChild("duibi/right/4").GetComponent<Text>().text = ContMgr.getCont("a3_equip_add") + data2.equipdata.add_level; }
            int gem_num = 0;
            foreach (int type in data1.equipdata.gem_att.Keys)
            {
                gem_num++;
                if (data1.equipdata.gem_att[type] < data2.equipdata.gem_att[type])
                { 
                    infoPanel[2].transform.FindChild("duibi/right/" + gem_num).GetComponent<Text>().text = " <color=#f90e0e>" + Globle.getAttrAddById(type, data2.equipdata.gem_att[type]) + "</color>";
                    //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(false);
                }
                else { infoPanel[2].transform.FindChild("duibi/right/" + gem_num).GetComponent<Text>().text = Globle.getAttrAddById(type, data2.equipdata.gem_att[type]) ; }
            }
        }
        void leftInfo_do(a3_BagItemData data)
        {
            infoPanel[2].transform.FindChild("duibi/left").gameObject.SetActive(true);
            infoPanel[2].transform.FindChild("duibi/left/star/Text").GetComponent<Text>().text = "+ 0";
            SXML gem_xml = XMLMgr.instance.GetSXML("item.gem", "item_id==" + data.tpid);
            SXML gem = gem_xml.GetNode("gem_info", "stage_level==" + data.equipdata.stage);
            int gem_num = 0;
            foreach (int type in data.equipdata.gem_att.Keys)
            {
                gem_num++;
                Text gem_text = infoPanel[2].transform.FindChild("duibi/left/" + gem_num).GetComponent<Text>();
                gem_text.text = Globle.getAttrAddById(type, 0);
            }
            infoPanel[2].transform.FindChild("duibi/left/4").GetComponent<Text>().text = ContMgr.getCont("a3_equip_add") + 0;
        }
        //void do_yulan(GameObject go)
        //{
        //    a3_BagItemData data1 = a3_EquipModel.getInstance().getEquipByAll(curInheritId1);
        //    a3_BagItemData data2 = a3_EquipModel.getInstance().getEquipByAll(curInheritId2);
        //    if (go.transform.FindChild("Text").GetComponent<Text>().text == "预览")
        //    {
        //        leftInfo_do(data1);
        //        rightInfo_do(data1, data2);
        //        go.transform.FindChild("Text").GetComponent<Text>().text = "返回";
        //    }
        //    else if(go.transform.FindChild("Text").GetComponent<Text>().text == "返回")
        //    {
        //        leftInfo(data1);
        //        rightInfo(data1, data2);
        //        go.transform.FindChild("Text").GetComponent<Text>().text = "预览";
        //    }
        //}
        //public void refreshDashi()
        //{
        //    GameObject item = QHDashi.transform.FindChild("view/item").gameObject;
        //    RectTransform con = QHDashi.transform.FindChild("view/con").GetComponent<RectTransform>();
        //    if (con.childCount > 0)
        //    {
        //        for (int i = 0; i < con.childCount; i++ ) 
        //        {
        //            Destroy(con.GetChild(i).gameObject);
        //        }
        //    }
        //    SXML dSXML = XMLMgr.instance.GetSXML("intensifymaster.level", "lvl==" + PlayerModel.getInstance().up_lvl);
        //    List<SXML> DSlvl = dSXML.GetNodeList("intensify");
        //    foreach (SXML it in DSlvl)
        //    {
        //        GameObject clon = Instantiate(item) as GameObject;
        //        Text qh_lvl = clon.transform.FindChild("qh_lvl/Text").GetComponent<Text>();
        //        qh_lvl.text = "+"+it.getInt("qh");
        //        List<SXML> info = it.GetNodeList("att");
        //        GameObject info_item = clon.transform.FindChild("scrollview/info_item").gameObject;
        //        RectTransform con_info = clon.transform.FindChild("scrollview/con").GetComponent<RectTransform>();
        //        foreach (SXML it_info in info) 
        //        {
        //            GameObject info_clon = Instantiate(info_item) as GameObject;
        //            Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
        //            info_text.text = "+" + it_info.getInt("value") + Globle.getAttrNameById(it_info.getInt("type"));
        //            info_clon.SetActive(true);
        //            info_clon.transform.SetParent(con_info,false);
        //        }
        //        clon.SetActive(true);
        //        clon.transform.SetParent(con,false);
        //    }
        //    float childSizeX = item.transform.GetComponent<RectTransform>().sizeDelta.x;
        //    float spacing = con.GetComponent<GridLayoutGroup>().spacing.x;
        //    Vector2 newSize = new Vector2(DSlvl.Count * (childSizeX+ spacing), con.sizeDelta.x);
        //    con.sizeDelta = newSize;
        //}


        #region 装备选取
        public void onOutEqp()//取出
        {
            for (int i = 0; i < 3; i++)
            {
                infoPanel[7].transform.FindChild("bg/att" + i).gameObject.SetActive(false);
            }
            foreach (int i in baoshi_con.Keys)
            {
                for (int g = 0; g < baoshi_con[i].transform.childCount; g++)
                {
                    if (baoshi_con[i].transform.GetChild(g).name == "local")
                    {
                        baoshi_con[i].transform.GetChild(g).gameObject.SetActive(true);
                    }
                    else if (baoshi_con[i].transform.GetChild(g).name == "isthis")
                    {
                        continue;
                    }
                    else
                    {
                        Destroy(baoshi_con[i].transform.GetChild(g).gameObject);
                    }
                }
            }
            Transform eqp_con = infoPanel[7].transform.FindChild("bg/equipicon");
            if (eqp_con.childCount > 0)
            {
                for (int i = 0; i < eqp_con.childCount; i++)
                {
                    Destroy(eqp_con.GetChild(i).gameObject);
                }
            }
            isthis.gameObject.SetActive(false);
            equipPanel[1].transform.FindChild("equip_info/bag_scroll_baoshi").gameObject.SetActive(false);
            equipPanel[1].transform.FindChild("equip_info/bag_scroll").gameObject.SetActive(true);
            curInheritId3 = 0;
            outKey = -1;
            refMask();
            ref_zx_baoshiMoney();
        }
        public void onClickEquip(GameObject go, uint id)//添加
        {
            curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
            refeqp_this();
            switch (tabIdx)
            {
                case 0:
                    refreshStrength(curChooseEquip);
                    break;
                case 1:
                    refrenshChange(curChooseEquip);
                    break;
                case 2:
                    if (curInheritId1 > 0 && curInheritId2 > 0)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_equip_yx"));
                    }
                    else if (curInheritId1 > 0)
                    {
                        if (curInheritId1 != id)
                        {
                            if (a3_EquipModel.getInstance().getEquipByAll(id).confdata.equip_type != a3_EquipModel.getInstance().getEquipByAll(curInheritId1).confdata.equip_type)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type0"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.stage > a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.stage)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type1"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.stage == a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.stage && a3_EquipModel.getInstance().getEquipByAll(id).equipdata.intensify_lv >= a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.intensify_lv)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type2"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.add_level > a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.add_level)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type3"));
                            }
                            //else if (isCan(a3_EquipModel.getInstance().getEquipByAll(curInheritId1), a3_EquipModel.getInstance().getEquipByAll(id)))
                            //{
                            //    flytxt.instance.fly("宝石等级不满足传承条件");
                            //}
                            else
                            {
                                curInheritId2 = id;
                                refreshInherit();
                            }
                        }
                    }
                    else if (curInheritId2 > 0)
                    {
                        if (curInheritId2 != id)
                        {
                            if (a3_EquipModel.getInstance().getEquipByAll(id).confdata.equip_type != a3_EquipModel.getInstance().getEquipByAll(curInheritId2).confdata.equip_type)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type0"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.stage > a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.stage)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type1"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.stage == a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.stage && a3_EquipModel.getInstance().getEquipByAll(id).equipdata.intensify_lv >= a3_EquipModel.getInstance().getEquipByAll(curInheritId1).equipdata.intensify_lv)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type2"));
                            }
                            else if (a3_EquipModel.getInstance().getEquipByAll(id).equipdata.add_level < a3_EquipModel.getInstance().getEquipByAll(curInheritId2).equipdata.add_level)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_equip_type3"));
                            }
                            //else if ( isCan(a3_EquipModel.getInstance().getEquipByAll(id), a3_EquipModel.getInstance().getEquipByAll(id)))
                            //{
                            //    flytxt.instance.fly("宝石等级不满足传承条件");
                            //}
                            else
                            {
                                curInheritId1 = id;
                                refreshInherit();
                            }
                        }
                    }
                    else
                    {
                        curInheritId1 = id;
                        refreshInherit();
                    }
                    refreshUnEquipItem();
                    break;
                case 3:
                    refreshAdvance(curChooseEquip);
                    break;
                case 4:
                    //refreshGemAtt(curChooseEquip);
                    break;
                case 5:
                    refreshAddAtt(curChooseEquip);
                    break;
                case 6:

                    break;
                case 7:
                    curInheritId3 = id;
                    refrenshChangeBaoshi();
                    refeqp();
                    break;
            }

            showIconHintImage();
        }
        #endregion
        #region 强化大师
        void refreshQHdashi()
        {
            //Dictionary<uint, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquips();
            //int minlvl = 0;
            //bool frist = true;
            //if (equips.Count < 10)
            //    minlvl = 0;
            //else
            //{
            //    foreach (uint i in equips.Keys)
            //    {
            //        if (frist)
            //        {
            //            minlvl = equips[i].equipdata.intensify_lv;
            //            frist = false;
            //        }
            //        if (equips[i].equipdata.intensify_lv < minlvl)
            //            minlvl = equips[i].equipdata.intensify_lv;
            //    }
            //}
            //qhDashi_text.text = minlvl.ToString();
            //frist = true;
        }
        //void onOpenDashi(GameObject go)
        //{
        //    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_QHMASTER);
        //}
        #endregion
        #region 道具索引
        void addtoget(a3_ItemData item)
        {
            if (XMLMgr.instance.GetSXML("item.item", "id==" + item.tpid).GetNode("drop_info") == null)
                return;
            ArrayList data1 = new ArrayList();
            data1.Add(item);
            data1.Add(InterfaceMgr.A3_EQUIP);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
        }
        #endregion
        #region 旧宝石系统
        //public void fly(string txt, int tag)
        //{
        //    GameObject item;
        //    GameObject txtclone;
        //    Tweener tween1;
        //    switch (tag)
        //    {
        //        case 1:
        //            item = transform.FindChild("panel_tab5/flytext/txt_3_equ1").gameObject;
        //            txtclone = ((GameObject)GameObject.Instantiate(item));
        //            txtclone.gameObject.SetActive(true);
        //            txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //            txtclone.transform.SetParent(item.transform.parent, false);
        //            tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //            tween1.OnComplete(delegate ()
        //            {
        //                Destroy(txtclone);
        //            });
        //            break;
        //        case 2:
        //            item = transform.FindChild("panel_tab5/flytext/txt_3_equ2").gameObject;
        //            txtclone = ((GameObject)GameObject.Instantiate(item));
        //            txtclone.gameObject.SetActive(true);
        //            txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //            txtclone.transform.SetParent(item.transform.parent, false);
        //            //GameObject.Destroy(txtclone,2.5f);
        //            tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //            tween1.OnComplete(delegate ()
        //            {
        //                Destroy(txtclone);
        //            });
        //            break;
        //        case 3:
        //            item = transform.FindChild("panel_tab5/flytext/txt_3_equ3").gameObject;
        //            txtclone = ((GameObject)GameObject.Instantiate(item));
        //            txtclone.gameObject.SetActive(true);
        //            txtclone.transform.FindChild("txt").GetComponent<Text>().text = txt;
        //            txtclone.transform.SetParent(item.transform.parent, false);
        //            //GameObject.Destroy(txtclone,2.5f);
        //            tween1 = txtclone.transform.FindChild("txt").DOLocalMoveY(100, 2.5f);
        //            tween1.OnComplete(delegate ()
        //            {
        //                Destroy(txtclone);
        //            });
        //            break;
        //    }
        //}
        //void onGemClick(int tag)
        //{
        //    isAdd = true;
        //    rateTime = 0.0f;
        //    addTime = 0.5f;
        //    addTyp = tag;
        //    addType = tag;
        //    sendGem(tag);
        //}
        //void sendGem(int tag)
        //{
        //    SXML s_xml = XMLMgr.instance.GetSXML("item.gem", "item_id==" + curChooseEquip.tpid);
        //    SXML s_gem_xml = s_xml.GetNode("gem_info", "stage_level==" + curChooseEquip.equipdata.stage);

        //    List<SXML> s_gematt_xml = s_gem_xml.GetNodeList("gem_att");
        //    int type = s_gematt_xml[tag - 1].getInt("att_type");

        //    EquipProxy.getInstance().sendGemUp(curChooseEquip.id, type);
        //}
        //void onGemClickExit(int tag)
        //{
        //    isAdd = false;
        //}
        //void refreshGemAtt(a3_BagItemData data)
        //{
        //    infoPanel[4].transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);
        //    Transform Image = infoPanel[4].transform.FindChild("icon");
        //    if (Image.childCount > 0)
        //    {
        //        for (int j = 0; j < Image.childCount; j++)
        //        {
        //            Destroy(Image.GetChild(j).gameObject);
        //        }
        //    }
        //    GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
        //    icon.transform.SetParent(Image, false);
        //    addEquipclick(icon, data);

        //    SXML s_xml = XMLMgr.instance.GetSXML("item.gem", "item_id==" + data.tpid);
        //    SXML s_gem_xml = s_xml.GetNode("gem_info", "stage_level==" + data.equipdata.stage);
        //    int i = 0;

        //    foreach (int type in data.equipdata.gem_att.Keys)
        //    {
        //        i++;
        //        infoPanel[4].transform.FindChild("attr" + i).GetComponent<Text>().text = " +" + data.equipdata.gem_att[type];
        //    }
        //    List<SXML> s_gematt_xml = s_gem_xml.GetNodeList("gem_att");
        //    infoPanel[4].transform.FindChild("max1").GetComponent<Text>().text = "MAX +" + s_gematt_xml[0].getString("att_max");
        //    infoPanel[4].transform.FindChild("max2").GetComponent<Text>().text = "MAX +" + s_gematt_xml[1].getString("att_max");
        //    infoPanel[4].transform.FindChild("max3").GetComponent<Text>().text = "MAX +" + s_gematt_xml[2].getString("att_max");
        //    i = 0;

        //    foreach (SXML x in s_gematt_xml)
        //    {
        //        infoPanel[4].transform.FindChild("stone" + (i + 1) + "/desc").GetComponent<Text>().text = x.getString("desc");
        //        Image icon_bg = infoPanel[4].transform.FindChild("icon" + (i + 1)).GetComponent<Image>();
        //        if (icon_bg.transform.childCount > 0)
        //        {
        //            Destroy(icon_bg.transform.GetChild(0).gameObject);
        //        }
        //        a3_ItemData icon_data = a3_BagModel.getInstance().getItemDataById(x.getUint("need_itemid"));
        //        icon_bg.sprite = GAMEAPI.ABUI_LoadSprite(icon_data.file);


        //        int itemNum = a3_BagModel.getInstance().getItemNumByTpid(x.getUint("need_itemid"));
        //        string need_str;
        //        if (itemNum < x.getInt("need_value"))
        //        {
        //            need_str = "<color=#f90e0e>" + itemNum + "</color>/" + x.getInt("need_value");
        //            //infoPanel[4].transform.FindChild("stone" + (i + 1) + "/btn_do").GetComponent<Button>().enabled = false;
        //        }
        //        else
        //        {
        //            need_str = "<color=#ffffff>" + itemNum + "</color>/" + x.getInt("need_value");
        //            //infoPanel[4].transform.FindChild("stone" + (i + 1) + "/btn_do").GetComponent<Button>().enabled = true;
        //        }
        //        infoPanel[4].transform.FindChild("stone" + (i + 1) + "/num").GetComponent<Text>().text = need_str;

        //        i++;

        //        float max = x.getFloat("att_max");
        //        infoPanel[4].transform.FindChild("bar" + i).GetComponent<Image>().fillAmount = data.equipdata.gem_att[x.getInt("att_type")] / max;
        //        infoPanel[4].transform.FindChild("top" + i).transform.localPosition = new Vector3(-370 + i * 170, 185 * data.equipdata.gem_att[x.getInt("att_type")] / max - 230, 0);
        //    }
        //}
        //float time = 0.2f;
        #endregion
        #region 操作结果相关
        void onEquipStrength(GameEvent e)
        {
            Variant data = e.data;
            bool success = data["success"];
            uint id = data["id"];
            curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
            refreshStrength(curChooseEquip);

            //refreshEquipIcon(id, EQUIP_SHOW_TYPE.SHOW_INTENSIFY);

            //ani_strength_1.enabled = true;
            //ani_strength_1.Play("qh_hamar", -1, 0f);

            if (success)
            {
                ani_strength_2.gameObject.SetActive(true);
                ani_strength_2.Play("ch_success", -1, 0f);
                //flytxt.instance.fly("强化成功！");

            }
            else
            {
                ani_strength_3.gameObject.SetActive(true);
                ani_strength_3.Play("ani_fail", -1, 0f);
                //flytxt.instance.fly("强化失败！");
            }
            //refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_INTENSIFY);
            refeqptext();
           // refreshQHdashi();
            showIconHintImage();
        }
        void onEquipAdvance(GameEvent e)
        {
            Variant data = e.data;
            bool success = data["success"];

            if (success)
            {
                uint id = data["id"];
                curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
                refreshAdvance(curChooseEquip);

                ani_Advance_1.gameObject.SetActive(true);
                ani_Advance_1.Play("jj_lightning", -1, 0f);


                //flytxt.instance.fly("进阶成功！");
                //refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_STAGE);
                refeqptext();
                ani_Advance_2.gameObject.SetActive(true);
                ani_Advance_2.Play("jj_success", -1, 0f);
            }
            else
            {
                ani_Advance_1.gameObject.SetActive(true);
                ani_Advance_1.Play("jj_lightning", -1, 0f);

                //flytxt.instance.fly("进阶失败！");

                ani_Advance_3.gameObject.SetActive(true);
                ani_Advance_3.Play("jj_fail", -1, 0f);
                refreshAdvance(curChooseEquip);
            }
            showIconHintImage();
        }
        void onEquipGem(GameEvent e)
        {
            //Variant data = e.data;
            //uint id = data["id"];
            //curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
            //refreshGemAtt(curChooseEquip);
            //debug.Log(data.dump());


            //fly("+" + data["att_value"], addTyp);
            //FightText.play(FightText.HEAL_TEXT, toRole.getHeadPos(), 1);
            //flytxt.instance.fly("培养成功！");
        }
        void onEquipAtt(GameEvent e)
        {
            Variant data = e.data;
            uint id = data["id"];
            curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
            refreshAddAtt(curChooseEquip);
            bool leapfrog = false;
            if (data.ContainsKey("leapfrog"))
            {
                leapfrog = data["leapfrog"];
            }
            else leapfrog = false;

            //refreshEquipIcon(id, EQUIP_SHOW_TYPE.SHOW_ADDLV);

            if (leapfrog)
            {
                //flytxt.instance.fly("跳级！");
                ani_add_att.gameObject.SetActive(true);
                ani_add_att.Play("zj_levelup", -1, 0f);

                SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + curChooseEquip.tpid);
                int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
                int attValue_cur = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * curChooseEquip.equipdata.add_level;
                int attValue_last = 0;
                if (curChooseEquip.equipdata.add_level > 1) {
                    attValue_last = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * (curChooseEquip.equipdata.add_level - 1);
                }
                flytxt.instance.fly(Globle.getAttrAddById(attType, (attValue_cur - attValue_last)));
            }
            else
            {
                // flytxt.instance.fly("追加成功！");
                if (data.ContainsKey("double_rate"))
                {
                    if (data["double_rate"] == 2)
                    {
                        add_double_min.gameObject.SetActive(true);
                        add_double_min.Play("zj_levelup", -1, 0f);
                    }
                    else if (data["double_rate"] > 2)
                    {
                        add_double_big.gameObject.SetActive(true);
                        add_double_big.Play("zj_levelup", -1, 0f);
                    }

                }
            }

            if (data["do_add_level_up"])
            {
                ani_add_att.gameObject.SetActive(true);
                ani_add_att.Play("zj_levelup", -1, 0f);
            }
            //refreshEquipItem(EQUIP_SHOW_TYPE.SHOW_ADDLV);
            refeqptext();
            ani_add_att1.enabled = true;
            ani_add_att1.Play("equip_zj_head", -1, 0f);
            showIconHintImage();
        }

        void onEquipInherit(GameEvent e)
        {
            Variant data = e.data;
            //flytxt.instance.fly("传承成功！");
            if (a3_BagModel.getInstance().getUnEquips().ContainsKey(curChooseEquip.id))
            {
                curChooseEquip = a3_BagModel.getInstance().getUnEquips()[curChooseEquip.id];
            }
            else if (a3_EquipModel.getInstance().getEquips().ContainsKey(curChooseEquip.id))
            {
                curChooseEquip = a3_EquipModel.getInstance().getEquips()[curChooseEquip.id];
            }
            //curChooseEquip = a3_BagModel.getInstance()

            uint id1 = data["frm_eqpinfo"]["id"];
            uint id2 = data["to_eqpinfo"]["id"];

            // refreshEquipIcon(id1);
            //refreshEquipIcon(id2);


            curInheritId1 = 0;
            curInheritId2 = 0;
            refreshUnEquipItem();
            refreshInherit();
            refeqptext();
            ani_inherit.gameObject.SetActive(true);
            ani_inherit.Play("cc_arrow", -1, 0f);
            showIconHintImage();
        }
        void onEquipBaoshi(GameEvent e)
        {
            if (a3_BagModel.getInstance().getUnEquips().ContainsKey(curChooseEquip.id))
            {
                curChooseEquip = a3_BagModel.getInstance().getUnEquips()[curChooseEquip.id];
            }
            else if (a3_EquipModel.getInstance().getEquips().ContainsKey(curChooseEquip.id))
            {
                curChooseEquip = a3_EquipModel.getInstance().getEquips()[curChooseEquip.id];
            }
            refrenshChangeBaoshi();
            initBaoshiPanel();
            refeqptext();
            showIconHintImage();
        }
        void onChangeAtt(GameEvent e)
        {
            Variant data = e.data;
            int type = data["type"];
            int value = data["value"];

            transform.FindChild("change_panel").gameObject.SetActive(true);

            string str1 = Globle.getAttrAddById(curChooseAttType, curChooseEquip.equipdata.subjoin_att[curChooseAttType]);
            string str2 = Globle.getAttrAddById(type, value);

            transform.FindChild("change_panel/old_type").GetComponent<Text>().text = str1;
            transform.FindChild("change_panel/new_type").GetComponent<Text>().text = str2;

            SXML s_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + curChooseEquip.confdata.equip_level);

            if (s_xml.GetNode("subjoin_att_info", "att_type==" + curChooseAttType).getInt("max") <= curChooseEquip.equipdata.subjoin_att[curChooseAttType])
            {
                transform.FindChild("change_panel/old_max").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("change_panel/old_max").gameObject.SetActive(false);
            }

            if (s_xml.GetNode("subjoin_att_info", "att_type==" + type).getInt("max") <= value)
            {
                transform.FindChild("change_panel/new_max").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("change_panel/new_max").gameObject.SetActive(false);
            }
            showIconHintImage();
        }
        void onDoChangeAtt(GameEvent e)
        {
            Variant data = e.data;
            uint id = data["id"];
            curChooseEquip = a3_EquipModel.getInstance().getEquipByAll(id);
            refrenshChange(curChooseEquip);
            showIconHintImage();
        }
        #endregion
        #region 金币相关
        public void refreshMoney()
        {
            Text money = transform.FindChild("top/money").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("top/stone").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("top/bindstone").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
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
        #endregion
        #region 背包装备相关
        public void refeqp()
        {
            if (curInheritId3 > 0 && tabIdx == 7 || tabIdx == 6)
            {
                equipPanel[1].transform.FindChild("equip_info/bag_scroll_baoshi").gameObject.SetActive(true);
                equipPanel[1].transform.FindChild("equip_info/bag_scroll").gameObject.SetActive(false);
            }
            else
            {
                equipPanel[1].transform.FindChild("equip_info/bag_scroll_baoshi").gameObject.SetActive(false);
                equipPanel[1].transform.FindChild("equip_info/bag_scroll").gameObject.SetActive(true);
            }
        }
        void shot_eqp()
        {
            for (int i = 1; i <= 10; i++)
            {
                equipPanel[0].transform.FindChild("equip_info/scrollview/con/eqp" + i).SetAsLastSibling();
            }
        }
        GameObject createUnEquipItem(a3_BagItemData data)
        {
            GameObject item = equipPanel[1].transform.FindChild("equip_info/scroll/item").gameObject;
            GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
            itemclone.name = data.id.ToString();
            itemclone.SetActive(true);

            itemclone.transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);
            Transform Image = itemclone.transform.FindChild("icon");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            icon.transform.SetParent(Image, false);

            for (int i = 1; i <= 15; i++)
            {
                if (i <= data.equipdata.intensify_lv)
                    itemclone.transform.FindChild("stars/contain/star" + i).gameObject.SetActive(true);
                else
                    itemclone.transform.FindChild("stars/contain/star" + i).gameObject.SetActive(false);
            }

            addEquipTipClick_bag(icon, data);

            return itemclone;
        }
        void addEquipTipClick_bag(GameObject go, a3_BagItemData data)
        {
            go.transform.GetComponent<Button>().enabled = true;
            go.transform.GetComponent<Button>().onClick.AddListener((UnityEngine.Events.UnityAction)delegate ()
            {
                ArrayList list = new ArrayList();
                a3_BagItemData one = a3_EquipModel.getInstance().getEquipByAll(data.id);
                list.Add(one);
                list.Add(equip_tip_type.Equip_tip);
                list.Add(this.uiName);
                InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_EQUIPTIP, (ArrayList)list);
            });
        }
        void addEquipclick(GameObject go, a3_BagItemData data)
        {
            go.transform.GetComponent<Button>().enabled = true;
            go.transform.GetComponent<Button>().onClick.AddListener((UnityEngine.Events.UnityAction)delegate ()
            {
                ArrayList list = new ArrayList();
                a3_BagItemData one = a3_EquipModel.getInstance().getEquipByAll(data.id);
                list.Add(one);
                list.Add(equip_tip_type.tip_ForLook);
                list.Add(this.uiName);
                InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_EQUIPTIP, (ArrayList)list);
            });
        }
        void initEquipPanel()
        {
            equipicon.Clear();
            Dictionary<int, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquipsByType();
            for (int j = 1; j <= 10; j++)
            {
                GameObject parent = equipPanel[0].transform.FindChild("equip_info/scrollview/con/eqp" + j).gameObject;
                if (parent.transform.FindChild("equip").childCount > 0)
                {
                    Destroy(parent.transform.FindChild("equip").GetChild(0).gameObject);
                }
            }
            for (int m = 1; m <= 10; m++)
            {
                eqp_obj[m].transform.FindChild("has").gameObject.SetActive(false);
                eqp_obj[m].transform.FindChild("null").gameObject.SetActive(false);
                if (equips.ContainsKey(m))
                {

                    eqp_obj[m].transform.FindChild("has").gameObject.SetActive(true);
                    GameObject go = createEquipItem(equips[m]);
                    uint id = equips[m].id;
                    GameObject parent = equipPanel[0].transform.FindChild("equip_info/scrollview/con/eqp" + equips[m].confdata.equip_type).gameObject;
                    go.transform.SetParent(parent.transform.FindChild("equip"), false);
                    //go.transform.FindChild("lvl").GetComponent<Text>().text = one.equipdata.stage.ToString();
                    BaseButton btn_equip = new BaseButton(eqp_obj[m].transform, 0);
                    btn_equip.onClick = delegate (GameObject goo) { this.onClickEquip(go, id); };
                    equipicon[id] = go;
                    addEquipTipClick_bag(go, equips[m]);
                }
                else
                {
                    eqp_obj[m].transform.FindChild("null").gameObject.SetActive(true);
                    BaseButton btn_equip = new BaseButton(eqp_obj[m].transform, 0);
                    btn_equip.onClick = null;
                    eqp_obj[m].transform.SetAsLastSibling();
                }
            }
            //foreach (a3_BagItemData one in equips.Values)
            //{
            //    GameObject go = createEquipItem(one);
            //    uint id = one.id;
            //    GameObject parent = equipPanel[0].transform.FindChild("equip_info/scrollview/con/eqp" + one.confdata.equip_type).gameObject;
            //    go.transform.SetParent(parent.transform.FindChild("equip"), false);
            //    //go.transform.FindChild("lvl").GetComponent<Text>().text = one.equipdata.stage.ToString();
            //    BaseButton btn_equip = new BaseButton(go.transform.parent.parent, 0);
            //    btn_equip.onClick = delegate(GameObject goo) { this.onClickEquip(go, id); };

            //    equipicon[id] = go;
            //}

            Dictionary<uint, a3_BagItemData> unequips = a3_BagModel.getInstance().getUnEquips();
            int i = 0;
            foreach (a3_BagItemData one in unequips.Values)
            {
                // GameObject go = createUnEquipItem(one);
                GameObject gg = createUnEquipItem_bag(one);
                //gg.GetComponent<RectTransform>().localScale = new Vector3(0.9f,0.9f,1);
                gg.transform.SetParent(shellcon.transform.GetChild(i), false);
                //gg.transform.localPosition = Vector2.zero;

                i++;
                uint id = one.id;
                //go.transform.SetParent(unequipedList.transform, false);

                //BaseButton btn_equip = new BaseButton(go.transform, 0);
                //btn_equip.onClick = delegate(GameObject goo) { this.onClickEquip(go, id); };

                equipicon[id] = gg;
            }
            int num2 = unequips.Count;
            // float height2 = unequipedList.GetComponent<GridLayoutGroup>().cellSize.y;
            //RectTransform rect2 = unequipedList.GetComponent<RectTransform>();
            // rect2.sizeDelta = new Vector2(0.0f, num2 * height2);
        }
        string getEquipNameInfo(a3_BagItemData data)
        {
            string name = "";
            int quality = 1;
            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
            if (s_xml != null)
            {
                name = s_xml.getString("item_name");
                quality = s_xml.getInt("quality");
            }
            //string pro = "";
            //pro = ContMgr.getCont("a3_equip_" + data.equipdata.stage);
            //switch (data.equipdata.stage)
            //{
            //    case 0:
            //        pro = "普通的";
            //        break;
            //    case 1:
            //        pro = "强化的";
            //        break;
            //    case 2:
            //        pro = "打磨的";
            //        break;
            //    case 3:
            //        pro = "优良的";
            //        break;
            //    case 4:
            //        pro = "珍惜的";
            //        break;
            //    case 5:
            //        pro = "祝福的";
            //        break;
            //    case 6:
            //        pro = "完美的";
            //        break;
            //    case 7:
            //        pro = "卓越的";
            //        break;
            //    case 8:
            //        pro = "传说的";
            //        break;
            //    case 9:
            //        pro = "神话的";
            //        break;
            //    case 10:
            //        pro = "创世的";
            //        break;
            //}

            //name = pro + name;
            string cizhui = "";
            switch (data.equipdata.attribute)
            {
                case 0: break;
                case 1:
                    //cizhui = "[风]";
                    ContMgr.getCont("a3_equip_11");
                    break;
                case 2:
                    //cizhui = "[火]";
                    ContMgr.getCont("a3_equip_12");
                    break;
                case 3:
                   // cizhui = "[光]";
                    ContMgr.getCont("a3_equip_13");
                    break;
                case 4:
                    //cizhui = "[雷]";
                    ContMgr.getCont("a3_equip_14");
                    break;
                case 5:
                   // cizhui = "[冰]";
                    ContMgr.getCont("a3_equip_15");
                    break;
            }
            name = name + cizhui;
            name = Globle.getColorStrByQuality(name, quality);
            return name;
        }
        void refeqptext()
        {
            Dictionary<int, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquipsByType();
            foreach (int type in equips.Keys)
            {
                eqp_obj[type].transform.FindChild("has/lvl").GetComponent<Text>().text = ContMgr.getCont("a3_equip_jing") + equips[type].equipdata.stage + ContMgr.getCont("a3_equip_qiang") + equips[type].equipdata.intensify_lv;
                eqp_obj[type].transform.FindChild("has/zhui").GetComponent<Text>().text = ContMgr.getCont("a3_equip_zhui") + equips[type].equipdata.add_level;
                eqp_obj[type].transform.FindChild("has/name").GetComponent<Text>().text = getEquipNameInfo(equips[type]);
                for (int n = 0; n < 3; n++)
                {
                    eqp_obj[type].transform.FindChild("has/bs" + n).gameObject.SetActive(false);
                }
                foreach (int k in equips[type].equipdata.baoshi.Keys)
                {
                    eqp_obj[type].transform.FindChild("has/bs" + k).gameObject.SetActive(true);
                    if (equips[type].equipdata.baoshi[k] > 0)
                    {
                        SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                        SXML s_xml = itemsXMl.GetNode("item", "id==" + equips[type].equipdata.baoshi[k]);
                        string File = "icon_item_" + s_xml.getString("icon_file");
                        eqp_obj[type].transform.FindChild("has/bs" + k).FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(File);
                        eqp_obj[type].transform.FindChild("has/bs" + k).FindChild("icon").gameObject.SetActive(true);
                    }
                    else
                    {
                        eqp_obj[type].transform.FindChild("has/bs" + k).FindChild("icon").gameObject.SetActive(false);
                    }
                }
            }
        }
        GameObject createEquipItem(a3_BagItemData data)
        {
            GameObject icon;
            if (data.confdata.equip_type != 8 && data.confdata.equip_type != 9 && data.confdata.equip_type != 10)
            {
                icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            }
            else
            {
                icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            }
            //addEquipTipClick(icon, data);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            return icon;
        }
        void refreshEquipItem(EQUIP_SHOW_TYPE type)
        {
            Dictionary<uint, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquips();
            foreach (a3_BagItemData one in equips.Values)
            {
                if (equipicon.ContainsKey(one.id))
                {
                    IconImageMgr.getInstance().refreshA3EquipIcon_byType(equipicon[one.id], one, type);
                }
            }
        }
        void refreshUnEquipItem()
        {
            int num = 0;
            int j = 0;
            Dictionary<uint, a3_BagItemData> equips = a3_BagModel.getInstance().getUnEquips();
            if (curInheritId1 > 0 && curInheritId2 > 0)
            {

            }
            else if (curInheritId1 > 0)
            {
                a3_BagItemData one = a3_EquipModel.getInstance().getEquipByAll(curInheritId1);
                foreach (a3_BagItemData data in equips.Values)
                {
                    if (data.confdata.job_limit == one.confdata.job_limit
                        && data.confdata.equip_type == one.confdata.equip_type
                        && data.id != one.id)
                    {
                        equipicon[data.id].SetActive(true);
                        Transform go = shellcon.transform.GetChild(j).GetChild(0);
                        go.SetParent(equipicon[data.id].transform.parent, false);
                        go.transform.localPosition = Vector3.zero;
                        equipicon[data.id].transform.SetParent(shellcon.transform.GetChild(j), false);
                        //      equipicon[data.id].transform.localPosition = Vector3.zero;
                        //equipicon[data.id].transform.localPosition = shellcon.transform.GetChild(j).transform.localPosition;
                        j++;
                        num++;
                    }
                    else
                    {
                        equipicon[data.id].SetActive(false);
                    }
                }
            }
            else if (curInheritId2 > 0)
            {
                a3_BagItemData one = a3_EquipModel.getInstance().getEquipByAll(curInheritId2);
                foreach (a3_BagItemData data in equips.Values)
                {
                    if (data.confdata.job_limit == one.confdata.job_limit
                        && data.confdata.equip_type == one.confdata.equip_type
                        && data.id != one.id)
                    {
                        equipicon[data.id].SetActive(true);
                        Transform go = shellcon.transform.GetChild(j).GetChild(0);
                        go.SetParent(equipicon[data.id].transform.parent, false);
                        go.transform.localPosition = Vector3.zero;
                        equipicon[data.id].transform.SetParent(shellcon.transform.GetChild(j), false);
                        //equipicon[data.id].transform.localPosition = Vector3.zero;
                        j++;
                        num++;
                    }
                    else
                    {
                        equipicon[data.id].SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < shellcon.transform.childCount; i++)
                {
                    if (shellcon.transform.GetChild(i).transform.childCount > 0)
                    {
                        shellcon.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                    }
                    else { break; }
                    num++;
                }
            }
            //unequipedList.transform.localPosition = new Vector3(-210, 240, 0);
            // float height2 = unequipedList.GetComponent<GridLayoutGroup>().cellSize.y;
            // RectTransform rect2 = unequipedList.GetComponent<RectTransform>();
            //rect2.sizeDelta = new Vector2(0.0f, num * height2);
        }
        GameObject createUnEquipItem_bag(a3_BagItemData data)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            addEquipTipClick_bag(icon, data);
            return icon;
        }
        protected void refreshScrollRect()
        {
            int num = itemListView.transform.childCount;
            if (num <= 0)
                return;
            float height = item_Parent.cellSize.y;
            int row = (int)Math.Ceiling((double)num / 5);
            RectTransform rect = itemListView.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0.0f, row * height);
        }
        void reset()
        {
            for (int i = 0; i < shellcon.transform.childCount; i++)
            {
                if (shellcon.transform.GetChild(i).transform.childCount > 0)
                {
                    Destroy(shellcon.transform.GetChild(i).transform.GetChild(0).gameObject);
                }
                else
                {
                    break;
                }
            }
        }
        void refeqp_this()
        {
            if (a3_EquipModel.getInstance().getEquips().ContainsKey(curChooseEquip.id))
            {
                isthiseqp.gameObject.SetActive(true);
                isthiseqp.SetParent(eqp_obj[curChooseEquip.confdata.equip_type].transform, false);
                isthiseqp.localPosition = Vector3.zero;
            }
            else
            {
                isthiseqp.gameObject.SetActive(false);
            }
        }
        #endregion
        #region 强化相关
        void setStrength_NextTet(a3_BagItemData equip_data)
        {
            NextTet.transform.parent.gameObject.SetActive(true);
            SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + (equip_data.equipdata.intensify_lv + 1)).GetNode("intensify_info", "itemid==" + equip_data.tpid);
            string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');
            SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
            int add = 0;
            int add2 = 0;
            int lv = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).getInt("extra");
            if (list_att1.Length > 1)
            {
                add = int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                add2 = int.Parse(intensify_xml.getString("intensify_att").Split(',')[1]) * lv / 100;

                NextTet.text = ContMgr.getCont("a3_wing_skin1_") + " " + add + "-" + add2;
            }
            else
            {
                add = int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                NextTet.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " + " + add;
            }


        }
        void onStrength(GameObject go)
        {

            if (curChooseEquip.equipdata.intensify_lv < 15)
            {
                if (Needobj.Count > 0)
                {
                    foreach (a3_ItemData item in Needobj.Keys)
                    {
                        addtoget(item);
                    }
                }
                EquipProxy.getInstance().sendStrengthEquip(curChooseEquip.id);
            }
            else
            {
                flytxt.instance.fly("强化已至最高级！");
            }
        }
        void refreshStrength(a3_BagItemData data)
        {
            Needobj.Clear();
            //ani_strength_1.enabled = false;
            ani_strength_2.gameObject.SetActive(false);
            ani_strength_3.gameObject.SetActive(false);
            canUp.SetActive(false);
            ismaxlvl.SetActive(false);

            //infoPanel[0].transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);
            Transform Image = infoPanel[0].transform.FindChild("icon");
            if (Image.childCount > 0)
            {
                for (int i = 0; i < Image.childCount; i++)
                {
                    Destroy(Image.GetChild(i).gameObject);
                }
            }
            GameObject icon = createEquipItem(data);// IconImageMgr.getInstance().createA3ItemIcon
            icon.transform.FindChild("iconborder").gameObject.SetActive(false);
            icon.transform.FindChild("stars").gameObject.SetActive(false);
            icon.transform.FindChild("shuxing").gameObject.SetActive(false);
            icon.transform.FindChild("inlvl").gameObject.SetActive(false);
            icon.transform.SetParent(Image, false);
            addEquipclick(icon, data);


            for (int i = 1; i <= 15; i++)
            {
                if (i <= data.equipdata.intensify_lv)
                    infoPanel[0].transform.FindChild("stars/star" + i).gameObject.SetActive(true);
                else
                    infoPanel[0].transform.FindChild("stars/star" + i).gameObject.SetActive(false);
            }

            SXML xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + (data.equipdata.stage));
            infoPanel[0].transform.FindChild("value2").GetComponent<Text>().text = xml.getString("intensify_rate") + "%";

            SXML s_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + (data.equipdata.intensify_lv + 1));
            SXML sxml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + data.equipdata.stage);
            if (s_xml != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    int num = int.Parse(s_xml.getString("intensify_material" + (i + 1)).Split(',')[1]) + sxml.getInt("i_extra");
                    int havenum = a3_BagModel.getInstance().getItemNumByTpid(intensify_need_id[i]);
                    string text_need;
                    if (havenum >= num)
                        text_need = "<color=#ffffff>" + havenum + "</color>/" + num;
                    else
                    {
                        text_need = "<color=#f90e0e>" + havenum + "</color>/" + num;
                        Needobj[a3_BagModel.getInstance().getItemDataById(intensify_need_id[i])] = num;
                    }
                    infoPanel[0].transform.FindChild("need_item/item" + (i + 1) + "/num").GetComponent<Text>().text = text_need;

                }

                infoPanel[0].transform.FindChild("btn_do/value1").GetComponent<Text>().text =(s_xml.getInt("intensify_money") * sxml.getFloat("ratio") + sxml.getInt("m_extra")).ToString ();
                infoPanel[0].transform.FindChild("btn_do").gameObject.SetActive(true);
                infoPanel[0].transform.FindChild("btn_goUP").gameObject.SetActive(false);

                setStrength_NextTet(data);

            }
            else
            {//强化到满级
                infoPanel[0].transform.FindChild("need_item/item1" + "/num").GetComponent<Text>().text = "<color=#ffffff>" + a3_BagModel.getInstance().getItemNumByTpid(intensify_need_id[0]) + "</color>" + "/0";
                if (data.equipdata.stage < 10)
                {
                    canUp.SetActive(true);
                    infoPanel[0].transform.FindChild("btn_goUP/gotext").gameObject.SetActive(true);
                    infoPanel[0].transform.FindChild("btn_goUP/maxtext").gameObject.SetActive(false);
                    infoPanel[0].transform.FindChild("btn_goUP").GetComponent<Button>().interactable = true;
                }
                else if (data.equipdata.stage >= 10)
                {
                    ismaxlvl.SetActive(true);
                    infoPanel[0].transform.FindChild("btn_goUP/gotext").gameObject.SetActive(false);
                    infoPanel[0].transform.FindChild("btn_goUP/maxtext").gameObject.SetActive(true);
                    infoPanel[0].transform.FindChild("btn_goUP").GetComponent<Button>().interactable = false;
                }
                NextTet.transform.parent.gameObject.SetActive(false);
                infoPanel[0].transform.FindChild("btn_do").gameObject.SetActive(false);
                infoPanel[0].transform.FindChild("btn_goUP").gameObject.SetActive(true);
                new BaseButton(infoPanel[0].transform.FindChild("btn_goUP")).onClick = (GameObject go) =>
                {
                    onTab(3);
                };
            }

        }
        #endregion
        #region 精炼相关
        string str = "";
        void refreshAdvance(a3_BagItemData data)
        {
            ani_Advance_1.gameObject.SetActive(false);
            ani_Advance_2.gameObject.SetActive(false);
            ani_Advance_3.gameObject.SetActive(false);

            Needobj.Clear();
            str = "";

            Transform Image1 = infoPanel[3].transform.FindChild("icon1");
            if (Image1.childCount > 0)
            {
                for (int i = 0; i < Image1.childCount; i++)
                {
                    Destroy(Image1.GetChild(i).gameObject);
                }
            }
            GameObject icon1 = IconImageMgr.getInstance().createA3EquipIcon(data);
            icon1.transform.SetParent(Image1, false);
            addEquipclick(icon1, data);

            infoPanel[3].transform.FindChild("txt_lv").GetComponent<Text>().text = data.equipdata.stage + ContMgr.getCont("a3_equip_jie");

            bool can_advance = true;

            if (data.equipdata.intensify_lv >= 15)
            {
                infoPanel[3].transform.FindChild("limit0/yes").gameObject.SetActive(true);
                infoPanel[3].transform.FindChild("limit0/no").gameObject.SetActive(false);
            }
            else
            {
                infoPanel[3].transform.FindChild("limit0/yes").gameObject.SetActive(false);
                infoPanel[3].transform.FindChild("limit0/no").gameObject.SetActive(true);
                //can_advance = false;
            }
            SXML s_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + data.equipdata.stage);
            SXML s_xml_node = s_xml.GetNode("stage_info", "itemid==" + data.tpid);
            List<SXML> s_xml_material = s_xml_node.GetNodeList("stage_material");
            if (s_xml_material != null)
            {
                string limit1 = ContMgr.getCont("a3_equip_lv") + s_xml_node.getInt("zhuan") + ContMgr.getCont("zhuan") + s_xml_node.getInt("level") + ContMgr.getCont("ji");
                int limit2 = s_xml_node.getInt("stage_money");
                infoPanel[3].transform.FindChild("limit1").GetComponent<Text>().text = limit1;
                if (limit2 > 0)
                {
                    infoPanel[3].transform.FindChild("btn_do/money").gameObject.SetActive(true);
                    infoPanel[3].transform.FindChild("btn_do/maxlvl").gameObject.SetActive(false);
                    infoPanel[3].transform.FindChild("btn_do/money").GetComponent<Text>().text = limit2.ToString();
                    infoPanel[3].transform.FindChild("btn_do").GetComponent<Button>().interactable = true;
                }
                else
                {
                    infoPanel[3].transform.FindChild("btn_do/money").gameObject.SetActive(false);
                    infoPanel[3].transform.FindChild("btn_do/maxlvl").gameObject.SetActive(true);
                    infoPanel[3].transform.FindChild("btn_do").GetComponent<Button>().interactable = false;
                }

                //这里判断阶级，到时修改
                if (PlayerModel.getInstance().up_lvl > s_xml_node.getInt("zhuan"))
                {
                    infoPanel[3].transform.FindChild("limit1/yes").gameObject.SetActive(true);
                    infoPanel[3].transform.FindChild("limit1/no").gameObject.SetActive(false);
                }
                else if (PlayerModel.getInstance().up_lvl == s_xml_node.getInt("zhuan"))
                {
                    if (PlayerModel.getInstance().lvl >= s_xml_node.getInt("level"))
                    {
                        infoPanel[3].transform.FindChild("limit1/yes").gameObject.SetActive(true);
                        infoPanel[3].transform.FindChild("limit1/no").gameObject.SetActive(false);
                    }
                    else
                    {
                        infoPanel[3].transform.FindChild("limit1/yes").gameObject.SetActive(false);
                        infoPanel[3].transform.FindChild("limit1/no").gameObject.SetActive(true);
                    }
                }
                else
                {
                    infoPanel[3].transform.FindChild("limit1/yes").gameObject.SetActive(false);
                    infoPanel[3].transform.FindChild("limit1/no").gameObject.SetActive(true);
                    //can_advance = false;
                }
                if (PlayerModel.getInstance().money < s_xml_node.getInt("stage_money")) { }
                //can_advance = false;


                int index = 0;
                infoPanel[3].transform.FindChild("limit0").gameObject.SetActive(true);
                infoPanel[3].transform.FindChild("limit1").gameObject.SetActive(true);
                infoPanel[3].transform.FindChild("limit2").gameObject.SetActive(false);
                infoPanel[3].transform.FindChild("limit3").gameObject.SetActive(false);
                infoPanel[3].transform.FindChild("limit4").gameObject.SetActive(false);

                foreach (SXML x in s_xml_material)
                {
                    string name = a3_BagModel.getInstance().getItemDataById((uint)x.getInt("item")).item_name;
                    Needobj[a3_BagModel.getInstance().getItemDataById((uint)x.getInt("item"))] = x.getInt("num");

                    if (index == 0)
                    {
                        infoPanel[3].transform.FindChild("limit2").gameObject.SetActive(true);
                        string limit3 = name + "：" + x.getInt("num");
                        infoPanel[3].transform.FindChild("limit2").GetComponent<Text>().text = limit3;
                        if (a3_BagModel.getInstance().getItemNumByTpid((uint)x.getInt("item")) >= x.getInt("num"))
                        {
                            infoPanel[3].transform.FindChild("limit2/yes").gameObject.SetActive(true);
                            infoPanel[3].transform.FindChild("limit2/no").gameObject.SetActive(false);
                        }
                        else
                        {
                            infoPanel[3].transform.FindChild("limit2/yes").gameObject.SetActive(false);
                            infoPanel[3].transform.FindChild("limit2/no").gameObject.SetActive(true);
                            //can_advance = false;
                        }
                    }
                    if (index == 1)
                    {
                        string limit4 = name + "：" + x.getInt("num");
                        if ((uint)x.getInt("item") == 1541)
                        {
                            infoPanel[3].transform.FindChild("limit3").gameObject.SetActive(true);
                            infoPanel[3].transform.FindChild("limit3").GetComponent<Text>().text = limit4;
                            if (a3_BagModel.getInstance().getItemNumByTpid((uint)x.getInt("item")) >= x.getInt("num"))
                            {
                                infoPanel[3].transform.FindChild("limit3/yes").gameObject.SetActive(true);
                                infoPanel[3].transform.FindChild("limit3/no").gameObject.SetActive(false);
                            }
                            else
                            {
                                infoPanel[3].transform.FindChild("limit3/yes").gameObject.SetActive(false);
                                infoPanel[3].transform.FindChild("limit3/no").gameObject.SetActive(true);
                                //can_advance = false;
                            }
                        }
                        else
                        {
                            infoPanel[3].transform.FindChild("limit4").gameObject.SetActive(true);
                            infoPanel[3].transform.FindChild("limit4").GetComponent<Text>().text = limit4;
                            if (a3_BagModel.getInstance().getItemNumByTpid((uint)x.getInt("item")) >= x.getInt("num"))
                            {
                                infoPanel[3].transform.FindChild("limit4/yes").gameObject.SetActive(true);
                                infoPanel[3].transform.FindChild("limit4/no").gameObject.SetActive(false);
                            }
                            else
                            {
                                infoPanel[3].transform.FindChild("limit4/yes").gameObject.SetActive(false);
                                infoPanel[3].transform.FindChild("limit4/no").gameObject.SetActive(true);
                                //can_advance = false;
                            }
                        }
                    }
                    index++;
                }
            }
            else
            {
            }

            if (infoPanel[3].transform.FindChild("limit0/no").gameObject.activeSelf == true)
            {
                Needobj.Clear();
                infoPanel[3].transform.FindChild("needStrength").gameObject.SetActive(true);
                infoPanel[3].transform.FindChild("btn_do").gameObject.SetActive(false);
                new BaseButton(infoPanel[3].transform.FindChild("needStrength")).onClick = (GameObject go) =>
                {
                    onTab(0);
                };  
            }
            else
            {
                infoPanel[3].transform.FindChild("needStrength").gameObject.SetActive(false);
                infoPanel[3].transform.FindChild("btn_do").gameObject.SetActive(true);
                if (infoPanel[3].transform.FindChild("limit1/no").gameObject.activeSelf == true)
                {
                    str = "角色等级不足";
                    Needobj.Clear();
                }

                else if (infoPanel[3].transform.FindChild("limit2").gameObject.activeSelf == true && infoPanel[3].transform.FindChild("limit2/no").gameObject.activeSelf == true)
                {
                    str = "魔晶数量不足";
                }
                else if (infoPanel[3].transform.FindChild("limit3").gameObject.activeSelf == true && infoPanel[3].transform.FindChild("limit3/no").gameObject.activeSelf == true)
                {
                    str = "神光徽记数量不足";
                }
                else if (infoPanel[3].transform.FindChild("limit4").gameObject.activeSelf == true && infoPanel[3].transform.FindChild("limit4/no").gameObject.activeSelf == true)
                {
                    str = "秘法颗粒数量不足";
                }
                else if (PlayerModel.getInstance().money < s_xml_node.getInt("stage_money"))
                {
                    str = "金币不足";
                }
            }

            //if (can_advance)
            //    infoPanel[3].transform.FindChild("btn_do").GetComponent<Button>().interactable = true;
            //else
            //    infoPanel[3].transform.FindChild("btn_do").GetComponent<Button>().interactable = false;
        }
        void onAdvance(GameObject go)
        {
            if (Needobj.Count > 0)
            {
                foreach (a3_ItemData item in Needobj.Keys)
                {
                    if (Needobj[item] > a3_BagModel.getInstance().getItemNumByTpid(item.tpid))
                    {
                        addtoget(item);
                        break;
                    }
                }
            }

            if (str != "")
            {
                flytxt.instance.fly(str);
            }
            else
            {
                //EquipProxy.getInstance().sendAdvance(curChooseEquip.id);
                //tishi_jl.SetActive(true);
                setValue_tishi();
            }
        }

        void setValue_tishi() {

            string[] list_need1;
            string[] list_need2;
            int need1;
            int need2;
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + (curChooseEquip.equipdata.stage+1)).GetNode("stage_info", "itemid==" + curChooseEquip.tpid);
            if (stage_xml == null)
            {
                tishi_jl.SetActive(false);
                return;
            }
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + curChooseEquip.equipdata.blessing_lv);
            list_need1 = stage_xml.getString("equip_limit1").Split(',');
            list_need2 = stage_xml.getString("equip_limit2").Split(',');
            need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            string text_need1, text_need2;

            bool cando1 = true;
            bool cando2 = true;
            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])])
            {
                cando1 = true;
                // text_need1 = " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")"+ "</color>";
                text_need1 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            else {
                cando1 = false;
                //text_need1 =  " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")" + "</color>";
                text_need1 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])])
            {
                cando2 = true;
                // text_need2 =  " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }
            else {
                cando2 = false;
                // text_need2 = " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }
            tishi_jl.transform.FindChild("value1").GetComponent<Text>().text = text_need1;
            tishi_jl.transform.FindChild("value2").GetComponent<Text>().text = text_need2 ;

            if (cando1 && cando2)
            {
                EquipProxy.getInstance().sendAdvance(curChooseEquip.id);
                tishi_jl.SetActive(false);
            }
            else
            {
                tishi_jl.SetActive(true);
            }
        }

        #endregion
        #region 重铸相关
        void onChange(GameObject go)
        {
            if (curChooseAttType != -1)
            {
                EquipProxy.getInstance().sendChangeAtt(curChooseEquip.id, curChooseAttType);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_equip_notype"));
            }
        }
        void onYesChange(GameObject go)
        {
            EquipProxy.getInstance().sendDoChangeAtt(curChooseEquip.id, true);
            transform.FindChild("change_panel").gameObject.SetActive(false);
        }
        void onNoChange(GameObject go)
        {
            EquipProxy.getInstance().sendDoChangeAtt(curChooseEquip.id, false);
            transform.FindChild("change_panel").gameObject.SetActive(false);
        }
        void refrenshChange(a3_BagItemData data)
        {
            infoPanel[1].transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);

            Transform Image = infoPanel[1].transform.FindChild("icon");
            if (Image.childCount > 0)
            {
                for (int j = 0; j < Image.childCount; j++)
                {
                    Destroy(Image.GetChild(j).gameObject);
                }
            }
            GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
            icon.transform.SetParent(Image, false);
            addEquipclick(icon, data);

            SXML s_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + data.confdata.equip_level);
            infoPanel[1].transform.FindChild("btn_do/need_money").GetComponent<Text>().text = s_xml.getString("recasting_money");

            int i = 0;
            curChooseAttType = -1;
            int curOldAttTag = curChooseAttTag;
            foreach (int type in data.equipdata.subjoin_att.Keys)
            {
                i++;
                Toggle tog = infoPanel[1].transform.FindChild("attr_info/Toggle" + i).GetComponent<Toggle>();
                tog.gameObject.SetActive(true);
                tog.onValueChanged.RemoveAllListeners();
                int id = type;
                int tag = i;
                tog.onValueChanged.AddListener(delegate (bool isOn)
                {
                    curChooseAttType = id;
                    curChooseAttTag = tag;
                });

                tog.transform.FindChild("Label").GetComponent<Text>().text = Globle.getAttrAddById(type, data.equipdata.subjoin_att[type]);
                SXML s_xml_node = s_xml.GetNode("subjoin_att_info", "att_type==" + type);
                if (s_xml_node.getInt("max") <= data.equipdata.subjoin_att[type])
                {
                    tog.transform.FindChild("Max").gameObject.SetActive(true);
                }
                else
                {
                    tog.transform.FindChild("Max").gameObject.SetActive(false);
                }
                if (i == 1 && data.equipdata.subjoin_att.Count < curOldAttTag)
                {//默认选中第一个
                    tog.isOn = true;
                    curChooseAttType = id;
                    curChooseAttTag = 1;
                }
                else if (i == curOldAttTag)
                {//保存之前选中的项
                    tog.isOn = true;
                    curChooseAttType = id;
                    curChooseAttTag = i;
                }
                else
                {
                    tog.isOn = false;
                }
            }
            for (i = i + 1; i <= 5; i++)
            {
                Toggle tog = infoPanel[1].transform.FindChild("attr_info/Toggle" + i).GetComponent<Toggle>();
                tog.gameObject.SetActive(false);
            }
        }
        #endregion
        #region 传承相关
        string getstr(int type)
        {
            string str = "";
            switch (type)
            {
                case 0: break;
                case 1:
                    //str = "【风】";
                    ContMgr.getCont("a3_equip_11");
                    break;
                case 2:
                    //str = "【火】";
                    ContMgr.getCont("a3_equip_12");
                    break;
                case 3:
                    //str = "【光】";
                    ContMgr.getCont("a3_equip_13");
                    break;
                case 4:
                    //str = "【雷】";
                    ContMgr.getCont("a3_equip_14");
                    break;
                case 5:
                    //str = "【冰】";
                    ContMgr.getCont("a3_equip_15");
                    break;
            }
            return str;
        }
        void rightInfo_do(a3_BagItemData data1, a3_BagItemData data2)
        {
            infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(true);
            infoPanel[2].transform.FindChild("duibi").gameObject.SetActive(true);

            infoPanel[2].transform.FindChild("duibi/right/1").GetComponent<Text>().text = Globle.getColorStrByQuality(data2.confdata.item_name + getstr(data2.equipdata.attribute) + data2.equipdata.stage + "阶", data2.confdata.quality) + "<color=#00FF00>" + "（+" + (data1.equipdata.stage - data2.equipdata.stage) + "阶" + "）" + "</color>";

            if (data1.equipdata.intensify_lv < data2.equipdata.intensify_lv)
            {
                infoPanel[2].transform.FindChild("duibi/right/2").GetComponent<Text>().text = ContMgr.getCont("a3_equip_addlv") + data2.equipdata.intensify_lv + "<color=#f90e0e>" + "（-" + (data2.equipdata.intensify_lv - data1.equipdata.intensify_lv) + "）" + "</color>";
            }
            else
            {
                infoPanel[2].transform.FindChild("duibi/right/2").GetComponent<Text>().text = ContMgr.getCont("a3_equip_addlv") + data2.equipdata.intensify_lv + "<color=#00FF00>" + "（+" + (data1.equipdata.intensify_lv - data2.equipdata.intensify_lv) + "）" + "</color>";
            }
            SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + data2.tpid);
            int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
            int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * data2.equipdata.add_level;

            SXML add_xml1 = XMLMgr.instance.GetSXML("item.item", "id==" + data1.tpid);
            int attValue1 = int.Parse(add_xml1.getString("add_atttype").Split(',')[1]) * data1.equipdata.add_level;

            int maxlvl = 20 * data2.equipdata.stage;
            int maxValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * maxlvl;
            if (data1.equipdata.add_level > maxlvl)
            {
                infoPanel[2].transform.FindChild("duibi/right/3").GetComponent<Text>().text = ContMgr.getCont("a3_equip_add1") + Globle.getAttrAddById(attType, attValue) + "<color=#00FF00>" + "（+" + (maxValue - attValue) + "）" + "</color>";
            }
            else
            {
                infoPanel[2].transform.FindChild("duibi/right/3").GetComponent<Text>().text = ContMgr.getCont("a3_equip_add1") + Globle.getAttrAddById(attType, attValue) + "<color=#00FF00>" + "（+" + (attValue1 - attValue) + "）" + "</color>";
            }
        }
        void leftInfo(a3_BagItemData data)
        {
            infoPanel[2].transform.FindChild("duibi/left").gameObject.SetActive(true);
            //infoPanel[2].transform.FindChild("duibi/left/star/Text").GetComponent<Text>().text = "+" + data.equipdata.intensify_lv;
            // SXML gem_xml = XMLMgr.instance.GetSXML("item.gem", "item_id==" + data.tpid);
            // SXML gem = gem_xml.GetNode("gem_info", "stage_level==" + data.equipdata.stage);
            //int gem_num = 0;
            //foreach (int type in data.equipdata.gem_att.Keys)
            //{
            //    gem_num++;
            //    Text gem_text = infoPanel[2].transform.FindChild("duibi/left/" + gem_num).GetComponent<Text>();
            //    gem_text.text = Globle.getAttrAddById(type, data.equipdata.gem_att[type]);            
            //}
            infoPanel[2].transform.FindChild("duibi/left/1").GetComponent<Text>().text = data.confdata.item_name + getstr(data.equipdata.attribute) + data.equipdata.stage + "阶";
            infoPanel[2].transform.FindChild("duibi/left/1").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
            infoPanel[2].transform.FindChild("duibi/left/2").GetComponent<Text>().text = ContMgr.getCont("a3_equip_addlv") + data.equipdata.intensify_lv;
            //SXML add_xml = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (data.equipdata.add_level + 1));
            SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
            int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
            int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * data.equipdata.add_level;
            infoPanel[2].transform.FindChild("duibi/left/3").GetComponent<Text>().text = ContMgr.getCont("a3_equip_add1") + Globle.getAttrAddById(attType, attValue);
        }
        void refreshInherit()
        {
            ani_inherit.gameObject.SetActive(false);

            infoPanel[2].transform.FindChild("cctext").gameObject.SetActive(true);
            infoPanel[2].transform.FindChild("jctext").gameObject.SetActive(true);
            Transform Image1 = infoPanel[2].transform.FindChild("icon1");
            if (Image1.childCount > 0)
            {
                Destroy(Image1.GetChild(0).gameObject);
            }

            Transform Image2 = infoPanel[2].transform.FindChild("icon2");
            if (Image2.childCount > 0)
            {
                Destroy(Image2.GetChild(0).gameObject);
            }

            if (curInheritId1 > 0 && curInheritId2 > 0)
            {
                infoPanel[2].transform.FindChild("cctext").gameObject.SetActive(false);
                infoPanel[2].transform.FindChild("jctext").gameObject.SetActive(false);
                //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(true);
                //infoPanel[2].transform.FindChild("duibi/yulan/Text").GetComponent<Text>().text = "预览";
                a3_BagItemData data1 = a3_EquipModel.getInstance().getEquipByAll(curInheritId1);
                GameObject icon1 = IconImageMgr.getInstance().createA3EquipIcon(data1);
                icon1.transform.SetParent(Image1, false);

                addEquipclick(icon1, data1);

                a3_BagItemData data2 = a3_EquipModel.getInstance().getEquipByAll(curInheritId2);
                GameObject icon2 = IconImageMgr.getInstance().createA3EquipIcon(data2);
                icon2.transform.SetParent(Image2, false);

                addEquipclick(icon2, data2);

                infoPanel[2].transform.FindChild("btn_close1").gameObject.SetActive(true);
                infoPanel[2].transform.FindChild("btn_close2").gameObject.SetActive(true);
                infoPanel[2].transform.FindChild("btn_close1/name").GetComponent<Text>().text = data1.confdata.item_name;
                infoPanel[2].transform.FindChild("btn_close2/name").GetComponent<Text>().text = data2.confdata.item_name;
                infoPanel[2].transform.FindChild("btn_do").GetComponent<Button>().interactable = true;

                SXML s_xml_inherit = XMLMgr.instance.GetSXML("item.inheritance", "equip_stage==" + data1.equipdata.stage);
                infoPanel[2].transform.FindChild("btn_do/need_money").GetComponent<Text>().text = s_xml_inherit.getString("money");
                //duiBi(data1, data2);
                leftInfo(data1);
                rightInfo_do(data1, data2);
                //infoPanel[2].transform.FindChild("choose2/Toggle2/need_strone").GetComponent<Text>().text = s_xml_inherit.getString("diamond");
            }
            else
            {
                //infoPanel[2].transform.FindChild("duibi").gameObject.SetActive(false);
                //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(false);
                infoPanel[2].transform.FindChild("btn_do").GetComponent<Button>().interactable = false;
                infoPanel[2].transform.FindChild("btn_do/need_money").GetComponent<Text>().text = "0";
                //infoPanel[2].transform.FindChild("choose2/Toggle2/need_strone").GetComponent<Text>().text = "0";

                if (curInheritId1 > 0)
                {
                    infoPanel[2].transform.FindChild("cctext").gameObject.SetActive(false);
                    a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(curInheritId1);
                    GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
                    icon.transform.SetParent(Image1, false);
                    //infoPanel[2].transform.FindChild("duibi/yulan").gameObject.SetActive(false);
                    leftInfo(data);
                    addEquipclick(icon, data);

                    infoPanel[2].transform.FindChild("btn_close1").gameObject.SetActive(true);
                    infoPanel[2].transform.FindChild("btn_close1/name").GetComponent<Text>().text = data.confdata.item_name;
                }
                else if (curInheritId2 > 0)
                {
                    infoPanel[2].transform.FindChild("jctext").gameObject.SetActive(false);
                    infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(false);
                    a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(curInheritId2);
                    GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
                    icon.transform.SetParent(Image2, false);
                    addEquipclick(icon, data);

                    infoPanel[2].transform.FindChild("btn_close2").gameObject.SetActive(true);
                    infoPanel[2].transform.FindChild("btn_close2/name").GetComponent<Text>().text = data.confdata.item_name;
                }
                else
                {
                    infoPanel[2].transform.FindChild("btn_close1").gameObject.SetActive(false);
                    infoPanel[2].transform.FindChild("btn_close2").gameObject.SetActive(false);
                    infoPanel[2].transform.FindChild("duibi/left").gameObject.SetActive(false);
                    infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(false);
                }
            }
        }
        void onInheritRemove1(GameObject go)
        { 
            curInheritId1 = 0;
            Transform Image = infoPanel[2].transform.FindChild("icon1");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            go.SetActive(false);
            infoPanel[2].transform.FindChild("btn_do").GetComponent<Button>().interactable = false;
            //infoPanel[2].transform.FindChild("duibi").gameObject.SetActive(false);
            infoPanel[2].transform.FindChild("duibi/left").gameObject.SetActive(false);
            infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(false);
            infoPanel[2].transform.FindChild("btn_do/need_money").GetComponent<Text>().text = "0";
            //infoPanel[2].transform.FindChild("choose2/Toggle2/need_strone").GetComponent<Text>().text = "0";

            refreshUnEquipItem();
        }
        void onInheritRemove2(GameObject go)
        {
            curInheritId2 = 0;
            Transform Image = infoPanel[2].transform.FindChild("icon2");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            go.SetActive(false);
            infoPanel[2].transform.FindChild("btn_do").GetComponent<Button>().interactable = false;
            //infoPanel[2].transform.FindChild("duibi").gameObject.SetActive(false);
            infoPanel[2].transform.FindChild("duibi/right").gameObject.SetActive(false);
            infoPanel[2].transform.FindChild("btn_do/need_money").GetComponent<Text>().text = "0";
            //infoPanel[2].transform.FindChild("choose2/Toggle2/need_strone").GetComponent<Text>().text = "0";

            refreshUnEquipItem();
        }
        void onInherit(GameObject go)
        {
            setValue_tishi_cc();
        }

        void setValue_tishi_cc()
        {
            a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(curInheritId2);
            a3_BagItemData data1 = a3_EquipModel.getInstance().getEquipByAll(curInheritId1);

            string[] list_need1;
            string[] list_need2;
            int need1;
            int need2;
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + (data1.equipdata.stage )).GetNode("stage_info", "itemid==" + data.tpid);
            if (stage_xml == null)
            {
                tishi_cc.SetActive(false);
                return;
            }
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + data.equipdata.blessing_lv);
            list_need1 = stage_xml.getString("equip_limit1").Split(',');
            list_need2 = stage_xml.getString("equip_limit2").Split(',');
            need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            string text_need1, text_need2;

            bool cando1 = true;
            bool cando2 = true;
            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])])
            {
                cando1 = true;
                // text_need1 = " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")" + "</color>";
                text_need1 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            else
            {
                cando1 = false;
                // text_need1 = " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")" + "</color>";
                text_need1 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])])
            {
                cando2 = true;
                // text_need2 = " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }
            else
            {
                cando2 = false;
                // text_need2 = " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }
            tishi_cc.transform.FindChild("value1").GetComponent<Text>().text = text_need1;
            tishi_cc.transform.FindChild("value2").GetComponent<Text>().text = text_need2;

            if (cando1 && cando2)
            {
                if (curChooseInheritUseTag == 1)
                    EquipProxy.getInstance().sendInherit(curInheritId1, curInheritId2, curChooseInheritTag, false);
                else
                    EquipProxy.getInstance().sendInherit(curInheritId1, curInheritId2, curChooseInheritTag, true);
                tishi_cc.SetActive(false);
            }
            else {
                tishi_cc.SetActive(true);
            }

        }
        #endregion
        #region 追加相关
        void onAddAttr(GameObject go)
        {

            if (Needobj.Count > 0)
            {
                foreach (a3_ItemData item in Needobj.Keys)
                {
                    addtoget(item);
                }
            }

            if (curChooseEquip.equipdata.add_level < 20 * curChooseEquip.equipdata.stage)
            {
                EquipProxy.getInstance().sendAddAttr(curChooseEquip.id);
            }
            else
            {
                if (20 * curChooseEquip.equipdata.stage <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_wjl"));
                }
                else if (curChooseEquip.equipdata.add_level >= 20 * 10)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_zjs"));
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_jlh"));
                }
            }
        }

        void onAddAttr_num(GameObject go) {
            if (Needobj.Count > 0)
            {
                foreach (a3_ItemData item in Needobj.Keys)
                {
                    addtoget(item);
                }
            }
            if (curChooseEquip.equipdata.add_level < 20 * curChooseEquip.equipdata.stage)
            {
                EquipProxy.getInstance().sendAddAttr(curChooseEquip.id , 10);
            }
            else
            {
                if (20 * curChooseEquip.equipdata.stage <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_wjl"));
                }
                else if (curChooseEquip.equipdata.add_level >= 20 * 10)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_zjs"));
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equip_jlh"));
                }
            }


        }

        void refreshAddAtt(a3_BagItemData data)
        {

            Needobj.Clear();
            ani_add_att1.enabled = false;
            ani_add_att.gameObject.SetActive(false);
            add_double_min.gameObject.SetActive(false);
            add_double_big.gameObject.SetActive(false);

           // infoPanel[5].transform.FindChild("name").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipNameInfo(data);
            Transform Image = infoPanel[5].transform.FindChild("icon");
            if (Image.childCount > 0)
            {
                for (int i = 0; i < Image.childCount; i++)
                {
                    Destroy(Image.GetChild(i).gameObject);
                }
            }
            GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
            icon.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.86f, 1);
            icon.transform.SetParent(Image, false);
            addEquipclick(icon, data);

            infoPanel[5].transform.FindChild("add_lv").GetComponent<Text>().text = data.equipdata.add_level + "<color=#66FFFF>/"
                +data.equipdata.stage * 20 + "</color>";

            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
            int attType = int.Parse(s_xml.getString("add_atttype").Split(',')[0]);
            int attValue = int.Parse(s_xml.getString("add_atttype").Split(',')[1]) * data.equipdata.add_level;

            infoPanel[5].transform.FindChild("add_value").GetComponent<Text>().text = Globle.getAttrAddById(attType, attValue);

            SXML s_xml_att = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (data.equipdata.add_level + 1));
            uint id = data.tpid;
            int maxlvl = 20 * data.equipdata.stage;
            add_Change.fillAmount = (float)data.equipdata.add_level / maxlvl;


            if (s_xml_att != null)
            {
                Image expbar = infoPanel[5].transform.FindChild("bar").GetComponent<Image>();
                expbar.fillAmount = data.equipdata.add_exp / s_xml_att.getFloat("add_exp");

                uint material_id = s_xml_att.getUint("material_id");
                Transform Image1 = infoPanel[5].transform.FindChild("icon_need");
                if (Image1.childCount > 0)
                {
                    Destroy(Image1.GetChild(0).gameObject);
                }
                GameObject icon1 = IconImageMgr.getInstance().createA3ItemIcon(material_id);
                icon1.transform.SetParent(Image1, false);

                int material_num = s_xml_att.getInt("material_num");
                Text need_str = infoPanel[5].transform.FindChild("need_num").GetComponent<Text>();

                if (material_num <= a3_BagModel.getInstance().getItemNumByTpid(material_id))
                    need_str.color = Color.white;
                else
                {
                    need_str.color = Color.red;
                    Needobj[a3_BagModel.getInstance().getItemDataById(material_id)] = material_num;
                }
                need_str.text = a3_BagModel.getInstance().getItemNumByTpid(material_id)+"/"+ material_num;
                infoPanel[5].transform.FindChild("btn_do/money").GetComponent<Text>().text = s_xml_att.getString("money");
            }
            else
            {//已满级
                //flytxt.instance.fly("已满级！！！");
            }
        }
        #endregion
        #region 宝石相关
        void initBaoshiPanel()
        {
            for (int m = 0; m < shellcon_baoshi.transform.childCount; m++)
            {
                for (int n = 0; n < shellcon_baoshi.transform.GetChild(m).childCount; n++)
                {
                    Destroy(shellcon_baoshi.transform.GetChild(m).GetChild(n).gameObject);
                }
                if (shellcon_baoshi.transform.GetChild(m).childCount <= 0)
                    break;
            }
            Dictionary<uint, a3_BagItemData> item = a3_BagModel.getInstance().getItems();
            int i = 0;
            foreach (a3_BagItemData one in item.Values)
            {
                if (one.confdata.use_type == 19)
                {
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(one, true, one.num);
                    icon.transform.SetParent(shellcon_baoshi.transform.GetChild(i), false);
                    icon.transform.GetComponent<Button>().enabled = true;
                    icon.transform.GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        curBaoshiId = one.tpid;
                        Inbaoshi.SetActive(true);
                        Transform con = Inbaoshi.transform.FindChild("icon");

                        for (int m = 0; m < con.childCount; m++)
                        {
                            Destroy(con.GetChild(m).gameObject);
                        }
                        Inbaoshi.transform.FindChild("name").GetComponent<Text>().text = one.confdata.item_name;
                        Inbaoshi.transform.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(one.confdata.quality);

                        Inbaoshi.transform.FindChild("dec").GetComponent<Text>().text = StringUtils.formatText(one.confdata.desc);
                        GameObject icon1 = IconImageMgr.getInstance().createA3ItemIcon(one);
                        icon1.transform.SetParent(con, false);
                        if (tabIdx == 7)
                        {
                            Inbaoshi.transform.FindChild("do/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equip_xiangqian");
                            new BaseButton(Inbaoshi.transform.FindChild("do")).onClick = (GameObject go) =>
                            {
                                a3_BagItemData eqp = a3_EquipModel.getInstance().getEquipByAll(curInheritId3);
                                bool isCan = false;
                                foreach (int type in eqp.equipdata.baoshi.Values)
                                {
                                    if (type != 0)
                                    {
                                        isCan = false;
                                    }
                                    else
                                    {
                                        isCan = true;
                                        break;
                                    }
                                }
                                if (isCan)
                                {
                                    EquipProxy.getInstance().send_Changebaoshi((uint)curInheritId3, curBaoshiId);
                                }
                                else
                                {
                                    flytxt.instance.fly(ContMgr.getCont("a3_equip_gzb"));
                                }
                                Inbaoshi.SetActive(false);
                            };
                        }
                        else if (tabIdx == 6)
                        {
                            Inbaoshi.transform.FindChild("do/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equip_fangru");
                            new BaseButton(Inbaoshi.transform.FindChild("do")).onClick = (GameObject go) =>
                            {
                                input_baoshi(one);
                                Inbaoshi.SetActive(false);
                                ref_hc_baoshiMoney();
                            };
                        }
                    });
                    i++;
                }
            }
        }

        void input_baoshi(a3_BagItemData data)
        {
            clear_baoshi();
            SXML itemsXMl = XMLMgr.instance.GetSXML("item");
            SXML s_xml1 = itemsXMl.GetNode("gem_intensify.gem", "item_id==" + data.tpid);
            uint getid = (uint)s_xml1.getInt("get_num");
            if (getid <= 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_equip_maxlv"));
            }
            else if (data.num < 5)
            {
                addtoget(data.confdata);
                flytxt.instance.fly(ContMgr.getCont("a3_equip_nobaoshi"));
            }
            else
            {
                hcbaoshiId = data.tpid;
                hcid = data.id;
                for (int i = 0; i < baoshi_con2.Count; i++)
                {
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
                    icon.transform.SetParent(baoshi_con2[i].transform, false);
                }
                if (getid > 0)
                {
                    GameObject icon1 = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById(getid));
                    icon1.transform.FindChild("iconborder").gameObject.SetActive(false);
                    icon1.transform.SetParent(geticon.transform, false);
                }
                else
                {
                    hcbaoshiId = 0;
                    hcid = 0;
                }
            }
        }
        void clear_baoshi()
        {
            for (int i = 0; i < baoshi_con2.Count; i++)
            {
                for (int j = 0; j < baoshi_con2[i].transform.childCount; j++)
                {
                    Destroy(baoshi_con2[i].transform.GetChild(j).gameObject);
                }
            }
            for (int i = 0; i < geticon.transform.childCount; i++)
            {
                Destroy(geticon.transform.GetChild(i).gameObject);
            }
        }

        void onHeCheng(GameObject go)
        {
            if (hcbaoshiId > 0)
                EquipProxy.getInstance().send_hcBaoshi(hcbaoshiId, 1);
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_equip_qjr"));
            }
        }

        void onBaoshi_hc(GameEvent e)
        {
            Variant data = e.data;
            //uint id = data["id"];

            //flytxt.instance.fly("获得"+ a3_BagModel.getInstance().getItems()[id].confdata.item_name);

            SXML itemsXMl = XMLMgr.instance.GetSXML("item");
            SXML s_xml1 = itemsXMl.GetNode("gem_intensify.gem", "item_id==" + hcbaoshiId);

            uint getid = (uint)s_xml1.getInt("get_num");
            if (getid > 0)
            {
                SXML x = itemsXMl.GetNode("item", "id==" + getid);
                flytxt.instance.fly(ContMgr.getCont("a3_equip_get") + x.getString("item_name"));
            }

            if (a3_BagModel.getInstance().getItems().ContainsKey(hcid))
            {
                if (a3_BagModel.getInstance().getItems()[hcid].num >= 5)
                {

                }
                else
                {
                    clear_baoshi();
                    hcbaoshiId = 0;
                    hcid = 0;
                }
            }
            else
            {
                clear_baoshi();
                hcbaoshiId = 0;
                hcid = 0;
            }
            //clear_baoshi();
            //GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItems()[id],true);
            //icon.transform.FindChild("iconborder").gameObject.SetActive(false);
            //icon.transform.SetParent(geticon.transform, false);
            initBaoshiPanel();
            ref_hc_baoshiMoney();
        }
        //刷新宝石合成费用
        public void ref_hc_baoshiMoney()
        {
            if (hcbaoshiId <= 0)
            {
                hcMoney.text = "0";
            }
            else
            {
                SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                SXML s_xml1 = itemsXMl.GetNode("gem_intensify.gem", "item_id==" + hcbaoshiId);
                hcMoney.text = s_xml1.getInt("money").ToString();
            }
        }

        //刷新摘取宝石费用
        public void ref_zx_baoshiMoney()
        {
            if (outKey < 0 || outKey > 2)
            {
                zxMoney.text = "0";
            }
            else
            {
                SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                a3_BagItemData eqp = a3_EquipModel.getInstance().getEquipByAll(curInheritId3);
                SXML s_xml1 = itemsXMl.GetNode("gem_info", "item_id==" + eqp.equipdata.baoshi[outKey]);
                zxMoney.text = s_xml1.getInt("take_off_money").ToString();
            }
        }
        void refrenshChangeBaoshi()
        {
            outKey = -1;
            refMask();
            ref_zx_baoshiMoney();
            foreach (int i in baoshi_con.Keys)
            {
                for (int g = 0; g < baoshi_con[i].transform.childCount; g++)
                {
                    if (baoshi_con[i].transform.GetChild(g).name == "local")
                    {
                        baoshi_con[i].transform.GetChild(g).gameObject.SetActive(true);
                    }
                    else if (baoshi_con[i].transform.GetChild(g).name == "isthis")
                    {
                        continue;
                    }
                    else
                    {
                        Destroy(baoshi_con[i].transform.GetChild(g).gameObject);
                    }
                }
            }
            Transform eqp_con = infoPanel[7].transform.FindChild("bg/equipicon");
            if (eqp_con.childCount > 0)
            {
                for (int i = 0; i < eqp_con.childCount; i++)
                {
                    Destroy(eqp_con.GetChild(i).gameObject);
                }
            }
            if (curInheritId3 <= 0)
            {
                return;
            }
            a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(curInheritId3);
            GameObject icon1 = IconImageMgr.getInstance().createA3EquipIcon(data);
            icon1.transform.SetParent(eqp_con, false);
            addEquipTipClick_bag(icon1, data);
            if (data.equipdata.baoshi == null) { return; }

            for (int i = 0; i < 3; i++)
            {
                infoPanel[7].transform.FindChild("bg/att" + i).gameObject.SetActive(false);
            }
            foreach (int i in data.equipdata.baoshi.Keys)
            {
                if (data.equipdata.baoshi[i] == 0)//有孔没宝石
                {
                    baoshi_con[i].transform.FindChild("local").gameObject.SetActive(false);
                    infoPanel[7].transform.FindChild("bg/att" + i).gameObject.SetActive(true);
                    infoPanel[7].transform.FindChild("bg/att" + i + "/icon").gameObject.SetActive(false);
                    infoPanel[7].transform.FindChild("bg/att" + i + "/att").GetComponent<Text>().text = ContMgr.getCont("a3_equip_kxq");
                }

                if (data.equipdata.baoshi[i] > 0)//有孔有宝石
                {
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById((uint)data.equipdata.baoshi[i]), false, -1, 1f, false, -1, 0, false, false);
                    baoshi_con[i].transform.FindChild("local").gameObject.SetActive(false);
                    icon.transform.SetParent(baoshi_con[i].transform, false);
                    SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                    SXML s_xml = itemsXMl.GetNode("item", "id==" + data.equipdata.baoshi[i]);
                    string File = "icon_item_" + s_xml.getString("icon_file");
                    infoPanel[7].transform.FindChild("bg/att" + i + "/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(File);
                    infoPanel[7].transform.FindChild("bg/att" + i + "/icon").gameObject.SetActive(true);
                    SXML s_xml1 = itemsXMl.GetNode("gem_info", "item_id==" + data.equipdata.baoshi[i]);
                    List<SXML> gem_add = s_xml1.GetNodeList("gem_add");
                    int add_type = 0;
                    int add_vaule = 0;
                    foreach (SXML x in gem_add)
                    {
                        if (x.getInt("equip_type") == data.confdata.equip_type)
                        {
                            add_type = x.getInt("att_type");
                            add_vaule = x.getInt("att_value");
                            break;
                        }
                    }
                    infoPanel[7].transform.FindChild("bg/att" + i).gameObject.SetActive(true);
                    infoPanel[7].transform.FindChild("bg/att" + i + "/att").GetComponent<Text>().text = Globle.getAttrAddById(add_type, add_vaule);
                    int tag = i;
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        outKey = tag;
                        refMask();
                    };
                }
            }


        }
        void refMask()
        {

            if (outKey >= 0 && outKey <= 2)
            {
                isthis.transform.SetParent(baoshi_con[outKey].transform, false);
                isthis.SetActive(true);
                isthis.transform.localPosition = Vector3.zero;
                ref_zx_baoshiMoney();
            }
            else
            {
                isthis.SetActive(false);
            }
        }
        void onSendOut(GameObject go)
        {
            if (outKey >= 0 && outKey <= 2)
            {
                EquipProxy.getInstance().send_outBaoshi(curInheritId3, (uint)(outKey + 1));
                outKey = -1;
                refMask();
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_equip_pleasezhaiqu"));
            }
        }

        #endregion
        #region 开启检测
        public void CheckLock() {
			transform.FindChild("panelTab2/con/0").gameObject.SetActive(false);
			transform.FindChild("panelTab2/con/1").gameObject.SetActive(false);
			transform.FindChild("panelTab2/con/2").gameObject.SetActive(false);
			transform.FindChild("panelTab2/con/3").gameObject.SetActive(false);
			transform.FindChild("panelTab2/con/4").gameObject.SetActive(false);
			transform.FindChild("panelTab2/con/5").gameObject.SetActive(false);
            transform.FindChild("panelTab2/con/6").gameObject.SetActive(false);
            transform.FindChild("panelTab2/con/7").gameObject.SetActive(false);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_ENHANCEMENT)) {
				OpenQH();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_REMOLD)) {
				OpenCZ();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_INHERITANCE)) {
				OpenCC();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_LVUP)) {
				OpenJJ();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_MOUNTING)) {
				OpenBS();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_ENCHANT)) {
				OpenZJ();
			}
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_BSHC)){
                OpenBSHC();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_BSXQ)){
                OpenBSXQ();
            }
        }
		public void OpenQH() {
			transform.FindChild("panelTab2/con/0").gameObject.SetActive(true);
		}
		public void OpenCZ() {
			transform.FindChild("panelTab2/con/1").gameObject.SetActive(true);
		}
		public void OpenCC() {
			transform.FindChild("panelTab2/con/2").gameObject.SetActive(true);
		}
		public void OpenJJ() {
			transform.FindChild("panelTab2/con/3").gameObject.SetActive(true);
		}
		public void OpenBS() {
			transform.FindChild("panelTab2/con/4").gameObject.SetActive(true);
		}
		public void OpenZJ() {
			transform.FindChild("panelTab2/con/5").gameObject.SetActive(true);
		}
        public void OpenBSHC(){
            transform.FindChild("panelTab2/con/6").gameObject.SetActive(true);
        }
        public void OpenBSXQ(){
            transform.FindChild("panelTab2/con/7").gameObject.SetActive(true);
        }
        #endregion

        #region 
        void addIconHintImage()
        {
            IconHintMgr.getInsatnce().addHint_equip(getTransformByPath("panelTab2/con/0"), IconHintMgr.TYPE_QIANGHUA);
            IconHintMgr.getInsatnce().addHint_equip(getTransformByPath("panelTab2/con/3"), IconHintMgr.TYPE_JINGLIAN);
            IconHintMgr.getInsatnce().addHint_equip(getTransformByPath("panelTab2/con/5"), IconHintMgr.TYPE_ZHUIJIA);
            IconHintMgr.getInsatnce().addHint_equip(getTransformByPath("panelTab2/con/7"), IconHintMgr.TYPE_XIANGQIAN);

        }

        void showIconHintImage()
        {
            if (check_QH())
                IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_QIANGHUA);
            else
                IconHintMgr.getInsatnce().closeHint(IconHintMgr.TYPE_QIANGHUA);

            if (check_JL()) IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_JINGLIAN);
            else
                IconHintMgr.getInsatnce().closeHint(IconHintMgr.TYPE_JINGLIAN);

            if(check_ZJ())
                IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_ZHUIJIA);
            else
                IconHintMgr.getInsatnce().closeHint(IconHintMgr.TYPE_ZHUIJIA);

            if (check_XQ())
                IconHintMgr.getInsatnce().showHint(IconHintMgr.TYPE_XIANGQIAN);
            else
                IconHintMgr.getInsatnce().closeHint(IconHintMgr.TYPE_XIANGQIAN);
        }
        bool check_QH() {
            SXML s_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + (curChooseEquip.equipdata.intensify_lv + 1));
            SXML sxml_s = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + curChooseEquip.equipdata.stage);
            if (s_xml != null)
            {
                if ((s_xml.getInt("intensify_money") * sxml_s.getFloat("ratio") + sxml_s.getInt("m_extra")) > PlayerModel.getInstance().money)
                    return false;
                string rate = s_xml.getString("intensify_material1");
                string[] str_matInfo = rate.Split(',');
                SXML sxml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + curChooseEquip.equipdata.stage);
                int num = a3_BagModel.getInstance().getItemNumByTpid(uint.Parse(str_matInfo[0]));
                if (num >= int.Parse(str_matInfo[1])+ sxml.getInt("i_extra"))
                    return true;
                else return false;
            }
            else
                return false;
        }
        bool check_JL() {
            bool can = true;
            SXML s_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + curChooseEquip.equipdata.stage);
            SXML s_xml_node = s_xml.GetNode("stage_info", "itemid==" + curChooseEquip.tpid);
            if (s_xml_node.getInt("stage_money") > PlayerModel.getInstance().money)
                return false;
            if (curChooseEquip.equipdata.intensify_lv < 15) return false;
            if ((s_xml_node.getInt("zhuan") > PlayerModel.getInstance().up_lvl) || (s_xml_node.getInt("zhuan") == PlayerModel.getInstance().up_lvl && s_xml_node.getInt ("level") > PlayerModel.getInstance ().lvl)) return false;
            List<SXML> s_xml_material = s_xml_node.GetNodeList("stage_material");
            foreach (SXML x in s_xml_material)
            {
                if (x.getInt("num") <= 0)
                    return false;
                int num = a3_BagModel.getInstance().getItemNumByTpid(x.getUint ("item"));
                if (num < x.getInt("num")) return false;
            }
            return can;
        }
        bool check_ZJ() {
            bool can = true;
            if (curChooseEquip.equipdata.stage <= 0) return false;
            if (curChooseEquip.equipdata.add_level >= 20 * curChooseEquip.equipdata.stage) return false;
            SXML s_xml_att = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (curChooseEquip.equipdata.add_level + 1));
            if (s_xml_att == null) return false;
            if (s_xml_att.getInt("money") > PlayerModel.getInstance().money) return false;
            uint material_id = s_xml_att.getUint("material_id");
            int material_num = s_xml_att.getInt("material_num");
            if (material_num > a3_BagModel.getInstance().getItemNumByTpid(material_id)) return false;
            return can;
        }
        bool check_XQ() {
            if (curChooseEquip.equipdata.baoshi.Count <= 0) return false;
            bool a = false;
            foreach (int i in curChooseEquip.equipdata.baoshi.Keys)
            {
                if (curChooseEquip.equipdata.baoshi[i] <= 0)
                {
                    a = true;
                    break;
                }
            }
            bool b = false;
            Dictionary<uint, a3_BagItemData> item = a3_BagModel.getInstance().getItems();
            foreach (a3_BagItemData one in item.Values)
            {
                if (one.confdata.use_type == 19)
                {
                    b = true;
                    break;
                }
            }
            return a && b;
        }

        #endregion
    }
}
