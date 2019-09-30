using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Diagnostics;

namespace MuGame
{
    class A3_Smithy : Window
    {
        public readonly uint RANDOM_ITEM_ID = 99;
        public readonly int USE_SCROLL = 15;
        #region Variable
        private List<KeyValuePair<int, string>> listMainItem;
        private List<int> listCarr;
        private int currentSelectedCarr;
        public int CurrentSelectedCarr
        {
            protected set
            {
                currentSelectedCarr = value < 0 ? 0 : value;
                listMainItem.Clear();
                switch (currentSelectedCarr)
                {
                    default:
                        listMainItem.Add(new KeyValuePair<int, string>(2, ContMgr.getCont("A3_Smithy0")));
                        listMainItem.Add(new KeyValuePair<int, string>(3, ContMgr.getCont("A3_Smithy1")));
                        listMainItem.Add(new KeyValuePair<int, string>(5, ContMgr.getCont("A3_Smithy2")));
                        listMainItem.Add(new KeyValuePair<int, string>(15, ContMgr.getCont("A3_Smithy3")));
                        break;

                    case 1:
                        listMainItem.Add(new KeyValuePair<int, string>(2, ContMgr.getCont("A3_Smithy0")));
                        listMainItem.Add(new KeyValuePair<int, string>(15, ContMgr.getCont("A3_Smithy3")));
                        break;
                    case 2:
                        listMainItem.Add(new KeyValuePair<int, string>(3, ContMgr.getCont("A3_Smithy1")));
                        listMainItem.Add(new KeyValuePair<int, string>(15, ContMgr.getCont("A3_Smithy3")));
                        break;
                    case 3:
                        listMainItem.Add(new KeyValuePair<int, string>(5, ContMgr.getCont("A3_Smithy2")));
                        listMainItem.Add(new KeyValuePair<int, string>(15, ContMgr.getCont("A3_Smithy3")));
                        break;
                    case 4:
                        listMainItem.Add(new KeyValuePair<int, string>(15, ContMgr.getCont("A3_Smithy3")));
                        break;
                }
            }
            get { return currentSelectedCarr; }
        }
        public List<int> listPartIdx;
        private int currentSelectedPart;
        private int CurrentSelectedPart
        {
            get
            {
                if (currentSelectedPart != 0)
                    return listPartIdx[currentSelectedPart];
                return 0;
            }
            set { currentSelectedPart = value; }
        }
        private int currentExpandCarr;
        private int targetNum = 1;
        private uint targetId;
        private bool isAccessible;
        private static A3_Smithy instance;
        public static A3_Smithy Instance
        {
            set
            {
                if (value == null)
                {
                    if (instance != null)
                        instance.isAccessible = false;
                }
                else
                {
                    instance = value;
                    instance.isAccessible = true;
                }
            }
            get
            {
                if (instance?.isAccessible ?? false)
                    return instance;
                else
                    return null;

            }
        }
        private Dictionary<int, ExpandState> dicExpandState;
        private int cur_num = 1,open_choose_tag = 1;
        private Scrollbar open_bar;
        #endregion

        #region Object
        private Text moneyNeed;
        private InputField textNum;
        private Dictionary<uint, GameObject> itemicon = new Dictionary<uint, GameObject>();
        private Dictionary<int, GameObject> dicMainObj;
        private Dictionary<int, SubHeadNode> dicSubHead;

        private List<MatInfo> targetMatList;
        private List<GameObject> matObjList = new List<GameObject>();
        private Slider sliderExp;
        #region enter panel
        private Transform choosePart,
                          checkPart,
                          checkRandomPart,
                          mainPanel;
        #endregion

        #region main panel   
        #region left panel
        private GameObject targetIcon,
                           randomItemIcon,
                           relearnTip;
        private Transform targetEqp,
                          targetMatListObj,
                          matIcon,
                          mainItem,
                          subItem,
                          subHead;
        public Transform transEqpList;

        private Transform rlrnArmor,
                          rlrnWeapon,
                          rlrnJewelry;
        private Toggle tglRlrnArmor,
                       tglRlrnWeapon,
                       tglRlrnJewelry;
        private GridLayoutGroup itemParent;

        private Dropdown dropdownCarr,
                         dropdownPart;
        private Text txtLv;
        private GameObject fxSmithyLvUp;
        
        #endregion

        #region right bag panel
        private Dictionary<uint, a3_BagItemData> dic_BagItem_shll = new Dictionary<uint, a3_BagItemData>();
        private Dictionary<uint, GameObject> itemcon_chushou = new Dictionary<uint, GameObject>();
        private GridLayoutGroup item_Parent_chushou;
        private int GetMoneyNum = 0;
        private Text Money;
        private List<Variant> dic_Itemlist = new List<Variant>();
        #endregion
        #endregion
        #endregion


        public override void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("choosePart/border/info").text = ContMgr.getCont("A3_Smithy_0");
            getComponentByPath<Text>("Main/left_panel/wk/exp/title_lv/txt").text = ContMgr.getCont("A3_Smithy_1");
            getComponentByPath<Text>("Main/left_panel/wk/btn_do/Text").text = ContMgr.getCont("A3_Smithy_2");
            getComponentByPath<Text>("Main/left_panel/wk/filter/title_carr/Text").text = ContMgr.getCont("A3_Smithy_3");
            getComponentByPath<Text>("Main/left_panel/wk/filter/title_part/Text").text = ContMgr.getCont("A3_Smithy_4");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/topText/Text").text = ContMgr.getCont("A3_Smithy_5");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/go/Text").text = ContMgr.getCont("A3_Smithy_6");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_white/Label").text = ContMgr.getCont("A3_Smithy_7");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_green/Label").text = ContMgr.getCont("A3_Smithy_8");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_blue/Label").text = ContMgr.getCont("A3_Smithy_9");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_puple/Label").text = ContMgr.getCont("A3_Smithy_10");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_orange/Label").text = ContMgr.getCont("A3_Smithy_11");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Explain_text").text = ContMgr.getCont("A3_Smithy_12");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Explain_text1").text = ContMgr.getCont("A3_Smithy_13");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Explain_text2").text = ContMgr.getCont("A3_Smithy_14");
            getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/Explain_text3").text = ContMgr.getCont("A3_Smithy_15");
            getComponentByPath<Text>("Main/right_bag/piliang_chushou/info_bg/Text").text = ContMgr.getCont("A3_Smithy_16");
            getComponentByPath<Text>("Main/right_bag/piliang_chushou/info_bg/topText/Text").text = ContMgr.getCont("A3_Smithy_17");
            getComponentByPath<Text>("Main/right_bag/piliang_chushou/info_bg/go/Text").text = ContMgr.getCont("A3_Smithy_18");
            getComponentByPath<Text>("Main/right_bag/piliang_chushou/Image/Text").text = ContMgr.getCont("A3_Smithy_19");
            getComponentByPath<Text>("check/border/Text").text = ContMgr.getCont("A3_Smithy_20");
            getComponentByPath<Text>("check/border/Text1").text = ContMgr.getCont("A3_Smithy_21");
            getComponentByPath<Text>("check/border/cancel/Text").text = ContMgr.getCont("A3_Smithy_22");
            getComponentByPath<Text>("check/border/OK/Text").text = ContMgr.getCont("A3_Smithy_23");
            getComponentByPath<Text>("checkSuggest/border/Text").text = ContMgr.getCont("A3_Smithy_24");
            getComponentByPath<Text>("checkSuggest/border/cancel/Text").text = ContMgr.getCont("A3_Smithy_22");
            getComponentByPath<Text>("checkSuggest/border/OK/Text").text = ContMgr.getCont("A3_Smithy_23");
            getComponentByPath<Text>("HelpTip/Bg/info").text = ContMgr.getCont("A3_Smithy_25");
            getComponentByPath<Text>("HelpTip/nk/lv1").text = ContMgr.getCont("A3_Smithy_26");
            getComponentByPath<Text>("HelpTip/nk/lv2").text = ContMgr.getCont("A3_Smithy_27");
            getComponentByPath<Text>("HelpTip/nk/lv3").text = ContMgr.getCont("A3_Smithy_28");
            getComponentByPath<Text>("HelpTip/nk/lv4").text = ContMgr.getCont("A3_Smithy_29");
            getComponentByPath<Text>("HelpTip/nk/lv5").text = ContMgr.getCont("A3_Smithy_30");
            getComponentByPath<Text>("HelpTip/nk/lv6").text = ContMgr.getCont("A3_Smithy_31");
            getComponentByPath<Text>("HelpTip/nk/lv7").text = ContMgr.getCont("A3_Smithy_32");
            getComponentByPath<Text>("HelpTip/nk/lv8").text = ContMgr.getCont("A3_Smithy_33");
            getComponentByPath<Text>("HelpTip/nk/lv9").text = ContMgr.getCont("A3_Smithy_34");
            getComponentByPath<Text>("HelpTip/nk/lv10").text = ContMgr.getCont("A3_Smithy_35");
            getComponentByPath<Text>("RelearnTip/Bg/Text").text = ContMgr.getCont("A3_Smithy_36");
            getComponentByPath<Text>("RelearnTip/relearn_select/learn_weapon/Text").text = ContMgr.getCont("A3_Smithy_37");
            getComponentByPath<Text>("RelearnTip/relearn_select/learn_armor/Text").text = ContMgr.getCont("A3_Smithy_38");
            getComponentByPath<Text>("RelearnTip/relearn_select/learn_jew/Text").text = ContMgr.getCont("A3_Smithy_39");

            getComponentByPath<Text>("panel_open/Text").text = ContMgr.getCont("A3_Smithy_40");
            getComponentByPath<Text>("panel_open/open/Text").text = ContMgr.getCont("A3_Smithy_41");
            getComponentByPath<Text>("panel_open/open_choose/Toggle1/Label").text = ContMgr.getCont("A3_Smithy_42");
            getComponentByPath<Text>("panel_open/open_choose/Toggle2/Label").text = ContMgr.getCont("A3_Smithy_43");
            getComponentByPath<Text>("panel_open/title/Text").text = ContMgr.getCont("A3_Smithy_44");

            #endregion




            base.init();
            instance = this;
            #region 左侧打造面板
            listMainItem = new List<KeyValuePair<int, string>>();

            this.listCarr = new List<int> { 2, 3, 5 };

            choosePart = transform.FindChild("choosePart");
            checkPart = transform.FindChild("check");
            checkRandomPart = transform.FindChild("checkSuggest");
            mainPanel = transform.FindChild("Main");
            relearnTip = transform.FindChild("RelearnTip").gameObject;
            sliderExp = transform.FindChild("Main/left_panel/wk/exp/Slider").GetComponent<Slider>();
            txtLv = transform.FindChild("Main/left_panel/wk/exp/title_lv/lv").GetComponent<Text>();
            new BaseButton(transform.FindChild("Main/left_panel/wk/btn_do")).onClick = (GameObject go) => Make();
            new BaseButton(transform.FindChild("Main/left_panel/wk/close")).onClick = (go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_SMITHY);
            targetMatListObj = transform.FindChild("Main/left_panel/wk/mat/mat_list");
            targetEqp = transform.FindChild("Main/left_panel/wk/mat/target_eqp/icon");
            itemParent = transform.FindChild("Main/right_bag/item_scroll/scroll_view/contain").GetComponent<GridLayoutGroup>();
            textNum = transform.FindChild("Main/left_panel/wk/num/InputField").GetComponent<InputField>();
            textNum.onEndEdit.AddListener((text) =>
            {
                if (int.TryParse(text, out targetNum))
                {
                    if (targetNum <= 0)
                    {
                        targetNum = 1;
                        textNum.text = "1";
                    }
                    else if (targetNum > 100)
                    {
                        targetNum = 100;
                        textNum.text = "100";
                    }
                }
                else
                {
                    targetNum = 1;
                    textNum.text = "1";
                }
                RefreshNumText();
            });
            moneyNeed = transform.FindChild("Main/left_panel/wk/btn_do/cost").GetComponent<Text>();
            new BaseButton(transform.FindChild("Main/left_panel/wk/num/num_sub")).onClick = (GameObject go) =>
            {
                if (targetNum <= 1)
                {
                    textNum.text = "1";
                    return;
                }
                textNum.text = (--targetNum).ToString();
                RefreshNumText();
                //for (int i = 0; i < matObjList.Count; i++)
                //{
                //    matObjList[i].transform.FindChild("Text").GetComponent<Text>().text =
                //    a3_BagModel.getInstance().getItemNumByTpid(targetMatList[i].tpid) + "/" + targetMatList[i].num * targetNum;                    
                //}
                //if(targetId!=RANDOM_ITEM_ID)
                //    moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostById(targetId, (int)targetNum).ToString();
                //else
                //    moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostByScroll((int)targetNum).ToString();
            };
            new BaseButton(transform.FindChild("Main/left_panel/wk/num/num_add")).onClick = (GameObject go) =>
            {
                textNum.text = ((++targetNum) > 10 ? (targetNum = 10) : targetNum).ToString();
                RefreshNumText();
            };            
            new BaseButton(transform.FindChild("Main/left_panel/wk/help")).onClick = (go) => { transform.FindChild("HelpTip").gameObject.SetActive(true); };
            new BaseButton(transform.FindChild("Main/left_panel/wk/relearn")).onClick = ShowRelearnWin;
            new BaseButton(transform.FindChild("RelearnTip/btn_useMoney")).onClick = OnRelearnByMoney;
            new BaseButton(transform.FindChild("RelearnTip/btn_useDiamond")).onClick = OnRelearnByDiamond;
            new BaseButton(transform.FindChild("HelpTip/closeArea")).onClick = (go) => { transform.FindChild("HelpTip").gameObject.SetActive(false); };
            new BaseButton(transform.FindChild("RelearnTip/closeArea")).onClick = (go) => { relearnTip.SetActive(false); };
            matIcon = transform.FindChild("template/mat_icon");
            mainItem = transform.FindChild("template/main");
            subItem = transform.FindChild("template/sub");
            subHead = transform.FindChild("template/subHead");
            randomItemIcon = transform.FindChild("template/randomItem").gameObject;
            (fxSmithyLvUp = transform.FindChild("Main/left_panel/wk/exp/lv_up_fx").gameObject).SetActive(false);
            transEqpList = transform.FindChild("Main/left_panel/wk/panelbody/scroll");
            dicMainObj = new Dictionary<int, GameObject>();
            dicExpandState = new Dictionary<int, ExpandState>();
            dicSubHead = new Dictionary<int, SubHeadNode>();

            dropdownCarr = transform.FindChild("Main/left_panel/wk/filter/dropdown_carr").GetComponent<Dropdown>();
            dropdownCarr.ClearOptions();
            List<Dropdown.OptionData> listCarr = new List<Dropdown.OptionData>();
            listCarr.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy4")));
            listCarr.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy0")));
            listCarr.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy1")));
            listCarr.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy2")));
            //listCarr.Add(new Dropdown.OptionData("卷轴"));
            dropdownCarr.AddOptions(listCarr);
            dropdownCarr.onValueChanged.AddListener(OnSelectCarr);
            dropdownPart = transform.FindChild("Main/left_panel/wk/filter/dropdown_part").GetComponent<Dropdown>();
            listPartIdx = new List<int>();
            dropdownPart.ClearOptions();
            dropdownPart.onValueChanged.AddListener(OnSelectPart);
            getEventTrigerByPath("ig_bg_bg").onClick = (GameObject go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_SMITHY);
            rlrnArmor = transform.FindChild("RelearnTip/relearn_select/learn_armor");
            rlrnJewelry = transform.FindChild("RelearnTip/relearn_select/learn_jew");
            rlrnWeapon = transform.FindChild("RelearnTip/relearn_select/learn_weapon");
            tglRlrnArmor = rlrnArmor.GetComponent<Toggle>();
            tglRlrnWeapon = rlrnWeapon.GetComponent<Toggle>();
            tglRlrnJewelry = rlrnJewelry.GetComponent<Toggle>();
            #endregion

            #region 右侧背包
            new BaseButton(transform.FindChild("Main/right_bag/item_scroll/bag")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WAREHOUSE);
            };
            BaseButton btn_fenjie = new BaseButton(transform.FindChild("Main/right_bag/piliang_fenjie/info_bg/go"));
            btn_fenjie.onClick = Sendproxy;
            item_Parent_fenjie = transform.FindChild("Main/right_bag/piliang_fenjie/scroll_view/contain").GetComponent<GridLayoutGroup>();
            new BaseButton(transform.FindChild("Main/right_bag/item_scroll/chushou")).onClick = onChushou;
            Money = getComponentByPath<Text>("Main/right_bag/piliang_chushou/money");
            item_Parent_chushou = transform.FindChild("Main/right_bag/piliang_chushou/info_bg/scroll_view/contain").GetComponent<GridLayoutGroup>();
            new BaseButton(transform.FindChild("Main/right_bag/piliang_chushou/close")).onClick = (GameObject go) => refresh_Sell();
            new BaseButton(transform.FindChild("Main/right_bag/piliang_chushou/info_bg/go")).onClick = SellItem;
            mojing = getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/mojing/num");
            shengguanghuiji = getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/shenguang/num");
            mifageli = getComponentByPath<Text>("Main/right_bag/piliang_fenjie/info_bg/mifa/num");
            white = getComponentByPath<Toggle>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_white");
            white.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison) { EquipsSureSell(1); OnLoadItem_fenjie(); }
                else { outItemCon_fenjie(1); EquipsNoSell(1); }
            });

            green = getComponentByPath<Toggle>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_green");
            green.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    if (white.isOn == false)
                    {
                        white.isOn = true;
                    }
                    EquipsSureSell(2); OnLoadItem_fenjie();
                }
                else
                {
                    outItemCon_fenjie(2); EquipsNoSell(2);
                }
            });
            blue = getComponentByPath<Toggle>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_blue");
            blue.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    if (green.isOn == false)
                    {
                        green.isOn = true;
                    }
                    EquipsSureSell(3); OnLoadItem_fenjie();
                }
                else
                {
                    outItemCon_fenjie(3); EquipsNoSell(3);
                }
            });
            purple = getComponentByPath<Toggle>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_puple");
            purple.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    if (blue.isOn == false)
                    {
                        blue.isOn = true;
                    }
                    EquipsSureSell(4); OnLoadItem_fenjie();
                }
                else
                {
                    outItemCon_fenjie(4); EquipsNoSell(4);
                }
            });
            orange = getComponentByPath<Toggle>("Main/right_bag/piliang_fenjie/info_bg/Toggle_all/Toggle_orange");
            orange.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    if (purple.isOn == false)
                    {
                        purple.isOn = true;
                    }
                    EquipsSureSell(5); OnLoadItem_fenjie();
                }
                else
                {
                    outItemCon_fenjie(5); EquipsNoSell(5);
                }
            });
            BaseButton btn_equip = new BaseButton(transform.FindChild("Main/right_bag/item_scroll/equip"));
            btn_equip.onClick = onEquipSell;
            BaseButton btn_fenjieclose = new BaseButton(transform.FindChild("Main/right_bag/piliang_fenjie/close"));
            btn_fenjieclose.onClick = onfenjieclose;
            BaseButton btn_close_open = new BaseButton(transform.FindChild("panel_open/close"));
            btn_close_open.onClick = onCloseOpen;
            BaseButton btn_open = new BaseButton(transform.FindChild("panel_open/open"));
            btn_open.onClick = onOpenLock;
            open_bar = transform.FindChild("panel_open/Scrollbar").GetComponent<Scrollbar>();
            open_bar.onValueChanged.AddListener(onNumChange);
            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                int tag = i;
                tog.onValueChanged.AddListener(delegate (bool isOn)
                {
                    open_choose_tag = tag;
                    checkNumChange();
                });
            }
            #endregion            
        }
        private void ShowRelearnWin(GameObject go)
        {
            relearnTip.SetActive(true);
            transform.FindChild("RelearnTip/btn_useMoney/Text").GetComponent<Text>().text = A3_SmithyModel.getInstance().rlrnMoneyCost.ToString();
            transform.FindChild("RelearnTip/btn_useDiamond/Text").GetComponent<Text>().text = A3_SmithyModel.getInstance().rlrnDiamondCost.ToString();
            rlrnArmor.gameObject.SetActive(true);
            rlrnJewelry.gameObject.SetActive(true);
            rlrnWeapon.gameObject.SetActive(true);
            switch (A3_SmithyModel.getInstance().CurSmithyType)
            {
                case SMITHY_PART.WEAPON: rlrnWeapon.gameObject.SetActive(false); break;
                case SMITHY_PART.JEWELRY: rlrnJewelry.gameObject.SetActive(false); break;
                case SMITHY_PART.ARMOR: rlrnArmor.gameObject.SetActive(false); break;
            }
        }
        private void RefreshNumText()
        {
            for (int i = 0; i < matObjList.Count; i++)
            {
                matObjList[i].transform.FindChild("Text").GetComponent<Text>().text =
                a3_BagModel.getInstance().getItemNumByTpid(targetMatList[i].tpid) + "/" + targetMatList[i].num * targetNum;
            }
            if (targetId != RANDOM_ITEM_ID)
                moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostById(targetId, (int)targetNum).ToString();
            else
                moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostByScroll((int)targetNum).ToString();
        }
        public override void onShowed()
        {
            base.onShowed();
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, OnItemChange);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITME_SELL, onItemSell);
            A3_SmithyProxy.getInstance().addEventListener(A3_SmithyProxy.ON_SMITHYDATACHANGED, OnSmithyDataChanged);
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_PUTON, onEquipOn);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenLockRec);
            A3_SmithyProxy.getInstance().SendRefresh();
            int.TryParse(textNum?.text ?? "1", out targetNum);
            //targetNum = int.Parse(textNum?.text ?? "1");
            CurrentSelectedCarr = 0;
            CurrentSelectedPart = 0;
            LoadItem();
            RefreshPanel();
            onOpenLockRec(null);
            open_choose_tag = 1;
            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                if (i == open_choose_tag)
                    tog.isOn = true;
                else
                    tog.isOn = false;
            }
            Instance = this;
        }

        public override void onClosed()
        {
            A3_SmithyProxy.getInstance().removeEventListener(A3_SmithyProxy.ON_SMITHYDATACHANGED, OnSmithyDataChanged);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITME_SELL, onItemSell);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITEM_CHANGE, OnItemChange);
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_PUTON, onEquipOn);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenLockRec);
            OnSelectedTargetEqp(0);
            CollapseItem(currentSelectedCarr);
            foreach (GameObject go in itemicon.Values)
                Destroy(go);
            itemicon.Clear();
            Instance = null;
            curBodyEqpInfo = new a3_BagItemData();
            targetId = 0;
            base.onClosed();
        }

        void onEquipOn(GameEvent e)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
            DestroyImmediate(curEqpInfo.Value);
            int count = itemParent.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform icon = itemParent.transform.GetChild(i);
                if (icon && icon.childCount > 1)
                    continue;
                else
                {
                    if (curBodyEqpInfo.id != 0)
                        CreateItemIcon(curBodyEqpInfo, i);
                    break;
                }
            }
        }


        void HideLvUp() => fxSmithyLvUp.SetActive(false);
        void ShowLvUp() => fxSmithyLvUp.SetActive(true);
        void DoLvUp()
        {
            Invoke("ShowLvUp", 0f);
            Invoke("HideLvUp", 3f);
        }
        private void OnSmithyDataChanged(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("exp"))
                A3_SmithyModel.getInstance().CurSmithyExp = data["exp"];
            if (data.ContainsKey("lvl"))
            {
                int prmLv = A3_SmithyModel.getInstance().CurSmithyLevel;
                A3_SmithyModel.getInstance().CurSmithyLevel = data["lvl"];
                if (prmLv != 0 && A3_SmithyModel.getInstance().CurSmithyLevel != prmLv)
                {
                    for (int i = 0; i < listCarr.Count; i++)
                        if (dicExpandState.ContainsKey(listCarr[i]) && dicExpandState[listCarr[i]] != ExpandState.NotInitialized && dicSubHead.ContainsKey(listCarr[i]))
                        {
                            dicSubHead[listCarr[i]].ShowItemByPart(CurrentSelectedPart);
                            dicSubHead[listCarr[i]].RectScroll.sizeDelta = new Vector2(
                                x: dicSubHead[listCarr[i]].RectScroll.sizeDelta.x,
                                y: dicSubHead[listCarr[i]].GetFixedHeight(CurrentSelectedPart)
                            );
                            if (listCarr[i] > 0)
                                dicSubHead[listCarr[i]].FixHeight();
                        }
                    DoLvUp();
                }
                if (A3_SmithyModel.getInstance().CurSmithyLevel == 0)
                {
                    InitSmithy(0);
                    return;
                }
                float curExp = A3_SmithyModel.getInstance().CurSmithyExp;
                float maxExp = A3_SmithyModel.getInstance().CalcMaxExp(A3_SmithyModel.getInstance().CurSmithyLevel);
                sliderExp.value = curExp / maxExp;
                txtLv.text = A3_SmithyModel.getInstance().CurSmithyLevel.ToString();
            }

            if (data.ContainsKey("type"))
            {
                A3_SmithyModel.getInstance().CurSmithyType = (SMITHY_PART)data["type"]._int;
                InitSmithy(data["type"]._uint);
            }
        }

        private void InitSmithy(uint typeId)
        {
            List<Dropdown.OptionData> listPart = new List<Dropdown.OptionData>();
            if (typeId != 0 && listPartIdx.Count > 1) return;
            if (dicMainObj.ContainsKey(USE_SCROLL))
            {
                dicMainObj[USE_SCROLL].transform.FindChild("img_expand").gameObject.SetActive(false);
                dicMainObj[USE_SCROLL].transform.FindChild("img_collapse").gameObject.SetActive(false);
            }
            switch (typeId)
            {
                default:
                case 0: /* 尚未学习 */
                    mainPanel.gameObject.SetActive(false);
                    InitLearnPanel();
                    break;
                case 2: /* 武器 */
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy4")));
                    listPartIdx.Add((int)EquipPart.All);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip6")));
                    listPartIdx.Add((int)EquipPart.MainWeapon);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip7")));
                    listPartIdx.Add((int)EquipPart.CoWeapon);
                    SubHeadNode.ListPartIdx = listPartIdx;
                    dropdownPart.AddOptions(listPart);
                    choosePart.gameObject.SetActive(false);
                    mainPanel.gameObject.SetActive(true);
                    break;
                case 1: /* 防具 */
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy4")));
                    listPartIdx.Add((int)EquipPart.All);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip1")));
                    listPartIdx.Add((int)EquipPart.Head);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip2")));
                    listPartIdx.Add((int)EquipPart.Shoulder);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip3")));
                    listPartIdx.Add((int)EquipPart.MainArmor);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip4")));
                    listPartIdx.Add((int)EquipPart.Leg);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip5")));
                    listPartIdx.Add((int)EquipPart.Foot);
                    SubHeadNode.ListPartIdx = listPartIdx;
                    dropdownPart.AddOptions(listPart);
                    choosePart.gameObject.SetActive(false);
                    mainPanel.gameObject.SetActive(true);
                    break;
                case 3: /* 首饰 */
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("A3_Smithy4")));
                    listPartIdx.Add((int)EquipPart.All);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip8")));
                    listPartIdx.Add((int)EquipPart.Necklace);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip10")));
                    listPartIdx.Add((int)EquipPart.LeftRing);
                    listPart.Add(new Dropdown.OptionData(ContMgr.getCont("globle_equip9")));
                    listPartIdx.Add((int)EquipPart.RightRing);
                    SubHeadNode.ListPartIdx = listPartIdx;
                    dropdownPart.AddOptions(listPart);
                    choosePart.gameObject.SetActive(false);
                    mainPanel.gameObject.SetActive(true);
                    break;
            }
            RefreshPanel();
        }

        private void InitLearnPanel()
        {
            choosePart.gameObject.SetActive(true);
            new BaseButton(choosePart.FindChild("border/learn_weapon")).onClick = (GameObject go) => InitCheckPanel(partId: 2);
            new BaseButton(choosePart.FindChild("border/learn_armor")).onClick = (GameObject go) => InitCheckPanel(partId: 1);
            new BaseButton(choosePart.FindChild("border/learn_accessories")).onClick = (GameObject go) => InitCheckPanel(partId: 3);
            new BaseButton(choosePart.FindChild("border/learn_suggest")).onClick = (GameObject go) => InitCheckRandomPanel();
        }

        private void InitCheckPanel(uint partId)
        {
            choosePart.gameObject.SetActive(false);
            checkPart.gameObject.SetActive(true);
            switch (partId)
            {
                case 1: checkPart.FindChild("border/Part").GetComponent<Text>().text = ContMgr.getCont("A3_Smithy5"); break;
                case 2: checkPart.FindChild("border/Part").GetComponent<Text>().text = ContMgr.getCont("globle_equip6"); break;
                case 3: checkPart.FindChild("border/Part").GetComponent<Text>().text = ContMgr.getCont("A3_Smithy6"); break;
            }
            new BaseButton(checkPart.FindChild("border/OK")).onClick = (GameObject _go) =>
            {
                checkPart.gameObject.SetActive(false);
                A3_SmithyProxy.getInstance().SendChooseLearn(partId);
                InitSmithy(partId);
            };
            new BaseButton(checkPart.FindChild("border/cancel")).onClick = (GameObject _go) =>
            {
                choosePart.gameObject.SetActive(true);
                checkPart.gameObject.SetActive(false);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SMITHY);
            };
        }

        private void InitCheckRandomPanel()
        {
            choosePart.gameObject.SetActive(false);
            checkRandomPart.gameObject.SetActive(true);
            new BaseButton(checkRandomPart.FindChild("border/OK")).onClick = (GameObject _go) =>
            {
                uint partId = (uint)UnityEngine.Random.Range(1, 3);
                A3_SmithyProxy.getInstance().SendChooseLearn(partId);
                InitSmithy(partId);
                checkRandomPart.gameObject.SetActive(false);
            };
            new BaseButton(checkRandomPart.FindChild("border/cancel")).onClick = (GameObject _go) =>
            {
                choosePart.gameObject.SetActive(true);
                checkRandomPart.gameObject.SetActive(false);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SMITHY);
            };
        }

        /// <summary>
        /// 制作装备
        /// </summary>
        private void @Make()
        {
            targetNum = 1;
            float cdTime = 3f;
            if (!int.TryParse(textNum.text, out targetNum))
                textNum.text = "1";
            if (targetId <= 0)
                flytxt.instance.fly(ContMgr.getCont("A3_Smithy_txt"));
            else if (IsMatEnough)
                if (IsMoneyEnough)
                    if (IsRoomEnough)
                    {
                        cd.updateHandle = (cd item) =>
                        {
                            int temp = (int)(cd.secCD - cd.lastCD) / 100;
                            item.txt.text = ((float)temp / 10f).ToString();
                        };
                        cd.show(() =>
                        {
                            if (targetId == RANDOM_ITEM_ID)
                                A3_SmithyProxy.getInstance().SendMakeByScroll(num: targetNum);
                            else
                                A3_SmithyProxy.getInstance().SendMake(targetId, num: targetNum);
                        }, cdTime);
                    }
                    else flytxt.instance.fly(ContMgr.getCont("A3_Smithy_txt1"));
                else flytxt.instance.fly(ContMgr.getCont("A3_Smithy_txt2"));
            else flytxt.instance.fly(ContMgr.getCont("A3_Smithy_txt3"));
        }

        private bool IsMatEnough => CheckMat();
        /// <summary>
        /// 检查玩家身上的材料是否足够
        /// </summary>
        /// <returns>玩家身上是否有足够的材料</returns>
        private bool CheckMat()
        {
            if (targetId != RANDOM_ITEM_ID)
            {
                for (int i = 0; i < targetMatList.Count; i++)
                    if (a3_BagModel.getInstance().getItemNumByTpid(targetMatList[i].tpid) < targetMatList[i].num * targetNum)
                        return false;
            }
            else
            {
                List<MatInfo> matList = A3_SmithyModel.getInstance().smithyInfoDicUseScroll[(int)A3_SmithyModel.getInstance().CurSmithyType];
                for (int i = 0; i < matList.Count; i++)
                    if (matList[i].num * targetNum > a3_BagModel.getInstance().getItemNumByTpid(matList[i].tpid))
                        return false;
            }
            return true;
        }

        private bool IsRoomEnough => a3_BagModel.getInstance().getItems().Count + targetNum <= a3_BagModel.getInstance().curi;
        private bool IsMoneyEnough => PlayerModel.getInstance().money > (
            targetId == RANDOM_ITEM_ID ?
                A3_SmithyModel.getInstance().GetMoneyCostByScroll(targetNum) :
                A3_SmithyModel.getInstance().GetMoneyCostById(targetId, targetNum)
            );
        /// <summary>
        /// 加载物品
        /// </summary>
        public void LoadItem()
        {
            int i = 0;
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            for (List<uint> idx = new List<uint>(items.Keys); i < items.Count; ++i)
                CreateItemIcon(items[idx[i]], i);
        }

        /// <summary>
        /// 玩家选择要打造的装备
        /// </summary>
        void OnSelectedTargetEqp(uint id)
        {
            for (int i = (targetMatListObj?.childCount ?? 0) - 1; i > -1; i--)
                DestroyImmediate(targetMatListObj.GetChild(i).gameObject);
            matObjList.Clear();
            if (targetIcon != null)
                DestroyImmediate(targetIcon);
            targetId = id;
            if (targetId == 0)
                return;
            targetIcon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById(targetId), istouch: true, ignoreAllEquipTip: true, scale: 0.7f);
            new BaseButton(targetIcon.GetComponentInChildren<Button>().transform).onClick = (go) =>
            {
                ArrayList uiData = new ArrayList();
                uiData.Add(targetId);
                uiData.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, uiData);
            };
            targetIcon.transform.SetParent(targetEqp, false);
            targetMatList = GetMatList(targetId);
            if (textNum.text == "")
            {
                textNum.text = "1";
                targetNum = 1;
            }
            for (int i = 0; i < targetMatList.Count; ++i)
            {
                a3_ItemData data = a3_BagModel.getInstance().getItemDataById(targetMatList[i].tpid);
                uint tpid = targetMatList[i].tpid;
                GameObject _matIcon = IconImageMgr.getInstance().createA3ItemIcon(data, istouch: true, scale: 0.6f);
                _matIcon.transform.FindChild("bicon")?.GetComponent<Image>()?.gameObject.SetActive(true);
                new BaseButton(_matIcon.transform.GetComponent<Button>().transform).onClick = (go) =>
                {
                    ArrayList uiData = new ArrayList();
                    uiData.Add(tpid);
                    uiData.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, uiData);
                };
                matIcon.name = "Item";
                Transform _matIconTran = Instantiate(matIcon).transform;
                _matIcon.transform.SetParent(_matIconTran, false);
                _matIconTran.transform.FindChild("Text").GetComponent<Text>().text =
                    a3_BagModel.getInstance().getItemNumByTpid(targetMatList[i].tpid) + "/" + targetMatList[i].num * targetNum;
                _matIconTran.SetParent(targetMatListObj, false);
                matObjList.Add(_matIconTran.gameObject);
            }
            moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostById(targetId, (int)targetNum).ToString();
        }

        /// <summary>
        /// 卷轴打造
        /// </summary>
        void OnSelectedScroll()
        {
            if (targetNum == 0)
            {
                targetNum = 1;
                textNum.text = "1";
            }
            for (int i = targetMatListObj.childCount - 1; i > -1; i--)
                DestroyImmediate(targetMatListObj.GetChild(i).gameObject);
            matObjList.Clear();
            if (targetIcon != null)
                DestroyImmediate(targetIcon);
            targetId = RANDOM_ITEM_ID;
            targetIcon = GameObject.Instantiate(randomItemIcon);
            new BaseButton(targetIcon.GetComponentInChildren<Button>().transform).onClick = (go) =>
            {
                ArrayList uiData = new ArrayList();
                uiData.Add(1700u/*随机物品Id*/);
                uiData.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, uiData);
            };
            targetIcon.transform.SetParent(targetEqp, false);
            targetMatList = A3_SmithyModel.getInstance().GetMatListUseScroll();
            for (int i = 0; i < targetMatList.Count; ++i)
            {
                a3_ItemData data = a3_BagModel.getInstance().getItemDataById(targetMatList[i].tpid);
                uint tpid = targetMatList[i].tpid;
                GameObject _matIcon = IconImageMgr.getInstance().createA3ItemIcon(data, istouch: true, scale: 0.6f);
                _matIcon.transform.FindChild("bicon")?.GetComponent<Image>()?.gameObject.SetActive(true);
                new BaseButton(_matIcon.transform.GetComponent<Button>().transform).onClick = (go) =>
                {
                    ArrayList uiData = new ArrayList();
                    uiData.Add(tpid);
                    uiData.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, uiData);
                };
                matIcon.name = "Item";
                Transform _matIconTran = Instantiate(matIcon).transform;
                _matIcon.transform.SetParent(_matIconTran, false);
                _matIconTran.transform.FindChild("Text").GetComponent<Text>().text =
                    a3_BagModel.getInstance().getItemNumByTpid(targetMatList[i].tpid) + "/" + targetMatList[i].num * targetNum;
                _matIconTran.SetParent(targetMatListObj, false);
                matObjList.Add(_matIconTran.gameObject);
            }
            moneyNeed.text = A3_SmithyModel.getInstance().GetMoneyCostByScroll((int)targetNum).ToString();
        }

        List<MatInfo> GetMatList(uint tpid) => A3_SmithyModel.getInstance().GetMatListById(tpid);

        private void OnSelectCarr(int select)
        {
            CurrentSelectedCarr = select;
            RefreshPanel();
        }

        private void OnSelectPart(int select)
        {
            CurrentSelectedPart = select;
            if (!dicSubHead.ContainsKey(currentExpandCarr) || dicExpandState[currentExpandCarr] == ExpandState.NotInitialized)
                return;
            RefreshExpand();
            for (int i = 0; i < listMainItem.Count; i++)
            {
                if (dicSubHead.ContainsKey(listMainItem[i].Key))
                    dicSubHead[listMainItem[i].Key].ShowItemByPart(CurrentSelectedPart);
            }
            dicSubHead[currentExpandCarr].RectScroll.sizeDelta = new Vector2(
                x: dicSubHead[currentExpandCarr].RectScroll.sizeDelta.x,
                y: dicSubHead[currentExpandCarr].GetFixedHeight(CurrentSelectedPart)
            );
            if (currentExpandCarr > 0)
                dicSubHead[currentExpandCarr].FixHeight();
        }

        private void RefreshExpand()
        {
            int i = 0,
                smithyLv = A3_SmithyModel.getInstance().CurSmithyLevel;
            for (List<int> idx = new List<int>(dicExpandState.Keys); i < dicExpandState.Count; i++)
            {
                if (!dicSubHead.ContainsKey(idx[i]))
                    continue;
                List<a3_EquipData> equips = new List<a3_EquipData>();
                List<a3_EquipData> _equipList = a3_EquipModel.getInstance().GetEquipListByEquipType(CurrentSelectedPart);
                for (int j = 0; j < _equipList.Count; j++)
                    if(GetMatList(_equipList[j].tpid.GetValueOrDefault(0)).Count>0)
                        equips.Add(_equipList[j]);

                for (int j = 0; j < equips.Count; j++)
                {
                    a3_ItemData bagData = a3_BagModel.getInstance().getItemDataById(equips[j].tpid.Value);
                    if (bagData.job_limit != 1 /* 1表示通用职业 */ && bagData.job_limit != idx[i])
                        continue;
                    GameObject goSub = Instantiate(subItem.gameObject);
                    goSub.SetActive(bagData.equip_level <= A3_SmithyModel.getInstance().smithyLevelInfoList[smithyLv - 1].MaxAllowedSetLv);
                    goSub.transform.FindChild("Text").GetComponent<Text>().text = a3_BagModel.getInstance().getEquipName(bagData.tpid);
                    dicSubHead[idx[i]].Add(bagData.tpid, new KeyValuePair<int, GameObject>(bagData.equip_type, goSub));
                    uint id = bagData.tpid;
                    new BaseButton(goSub.transform).onClick = delegate (GameObject go) { OnSelectedTargetEqp(id); };
                }
            }
        }
        /// <summary>
        /// 在职业筛选条件选取完毕后刷新面板
        /// </summary>
        private void RefreshPanel()
        {
            int i = 0;
            for (List<int> idx = new List<int>(dicMainObj.Keys); i < dicMainObj.Count; i++)
            {
                dicMainObj[idx[i]].SetActive(false);
                if (CurrentSelectedCarr != 0)
                    if (CurrentSelectedCarr != idx[i] && dicSubHead.ContainsKey(idx[i]))
                    {
                        dicSubHead[idx[i]].HeadObj.SetActive(false);
                        CollapseItem(idx[i]);
                        //dicExpandState[idx[i]] = ExpandState.Collaspsed;
                    }
            }
            for (i = 0; i < listMainItem.Count; ++i)
            {
                if (!dicMainObj.ContainsKey(listMainItem[i].Key))//Create
                {
                    GameObject goMainItem = Instantiate(mainItem.gameObject);
                    goMainItem.transform.FindChild("Text").GetComponent<Text>().text = listMainItem[i].Value;
                    goMainItem.transform.SetParent(transEqpList, false);
                    dicMainObj.Add(listMainItem[i].Key, goMainItem);
                    dicExpandState.Add(listMainItem[i].Key, ExpandState.NotInitialized);
                    int selectedCarr = listMainItem[i].Key;
                    if (listMainItem[i].Key == USE_SCROLL)
                        new BaseButton(goMainItem.transform).onClick = delegate (GameObject go) { OnSelectedScroll(); };
                    else
                        new BaseButton(goMainItem.transform).onClick = delegate (GameObject go) { OnExpandItemClick(selectedCarr); };
                }
                dicMainObj[listMainItem[i].Key].SetActive(true);
            }
        }
        /// <summary>
        /// 展开或收拢选中项
        /// </summary>
        /// <param name="selectedCarr">当前所选的职业</param>
        private void OnExpandItemClick(int selectedCarr)
        {
            switch (dicExpandState[selectedCarr])
            {
                case ExpandState.Expanded:
                    CollapseItem(selectedCarr);
                    break;
                default:
                    ExpandItem(selectedCarr);
                    int i = 0;
                    for (List<int> idx = new List<int>(dicExpandState.Keys); i < idx.Count; i++)
                        if (idx[i] != selectedCarr && dicExpandState[idx[i]] == ExpandState.Expanded)
                            CollapseItem(idx[i]);
                    Invoke("FixContentHeight", 0.2f);
                    break;
            }
        }

        private void FixContentHeight() => dicSubHead[currentExpandCarr].FixContentHeight();
        /// <summary>
        /// 收拢选中项
        /// </summary>
        /// <param name="index">当前所选的职业</param>
        private void CollapseItem(int selectedCarr)
        {
            if (selectedCarr == 0) return;
            if(dicExpandState.ContainsKey(selectedCarr))
                dicExpandState[selectedCarr] = ExpandState.Collaspsed;
            if (dicMainObj.ContainsKey(selectedCarr))
                dicMainObj[selectedCarr].transform.FindChild("img_expand").gameObject.SetActive(false);
            if (dicSubHead.ContainsKey(selectedCarr))
                dicSubHead[selectedCarr].HeadObj.SetActive(false);
        }

        /// <summary>
        /// 展开选中项
        /// </summary>
        /// <param name="selectedCarr">当前所选的职业</param>
        private void ExpandItem(int selectedCarr)
        {
            currentExpandCarr = selectedCarr;
            dicMainObj[selectedCarr].transform.FindChild("img_expand").gameObject.SetActive(true);
            if (!dicExpandState.ContainsKey(selectedCarr) || dicExpandState[selectedCarr] == ExpandState.NotInitialized)
                ExpandInit(selectedCarr);
            else
                dicSubHead[selectedCarr].HeadObj.SetActive(true);

            dicSubHead[selectedCarr].RectScroll.sizeDelta = new Vector2(
                x: dicSubHead[selectedCarr].RectScroll.sizeDelta.x,
                y: dicSubHead[selectedCarr].GetFixedHeight(CurrentSelectedPart)
            );
            dicSubHead[selectedCarr].FixHeight();
            dicExpandState[currentExpandCarr] = ExpandState.Expanded;
        }

        /// <summary>
        /// 初始化展开项
        /// </summary>
        /// <param name="selectedCarr">选中对象的下标: 2-战士 3-法师 5-刺客 </param>
        private void ExpandInit(int selectedCarr)
        {
            int smithyLv = A3_SmithyModel.getInstance().CurSmithyLevel;
            List<a3_EquipData> equips = new List<a3_EquipData>();
            List<a3_EquipData> _equipList = a3_EquipModel.getInstance().GetEquipListByEquipType(CurrentSelectedPart);
            for (int i = 0; i < _equipList.Count; i++)
                if (CurrentSelectedPart == _equipList[i].eqp_type || (CurrentSelectedPart == 0 && listPartIdx.Contains(_equipList[i].eqp_type)))
                    if (GetMatList(_equipList[i].tpid.GetValueOrDefault(0)).Count > 0)
                        equips.Add(_equipList[i]);

            SubHeadNode subNode = new SubHeadNode(Instantiate(subHead).gameObject);
            for (int i = 0; i < equips.Count; i++)
            {
                a3_ItemData bagData = a3_BagModel.getInstance().getItemDataById(equips[i].tpid.Value);
                if (bagData.job_limit != 1 /* 1表示通用职业 */ && bagData.job_limit != selectedCarr)
                    continue;
                GameObject goSub = Instantiate(subItem.gameObject);
                goSub.SetActive(bagData.equip_level <= A3_SmithyModel.getInstance().smithyLevelInfoList[smithyLv - 1].MaxAllowedSetLv);
                a3_ItemData item = a3_BagModel.getInstance().getItemDataById(bagData.tpid);
                goSub.transform.FindChild("Text").GetComponent<Text>().text = DyeEquipNameByQuality(item.item_name,item.quality);
                subNode.Add(bagData.tpid, new KeyValuePair<int, GameObject>(bagData.equip_type, goSub));
                uint id = bagData.tpid;
                new BaseButton(goSub.transform).onClick = delegate (GameObject go) { OnSelectedTargetEqp(id); };
            }
            subNode.HeadObj.transform.SetParent(transEqpList, false);
            subNode.HeadObj.transform.SetSiblingIndex(dicMainObj[selectedCarr].transform.GetSiblingIndex() + 1);
            if (!dicSubHead.ContainsKey(selectedCarr))
                dicSubHead.Add(selectedCarr, subNode);
        }
        private string DyeEquipNameByQuality(string eqpName, int quality)
        {
            if (quality == 1)
                return "<color=#FFFFFF>" + eqpName + "</color>";
            if (quality == 2)
                return "<color=#6CE868>" + eqpName + "</color>";
            if (quality == 3)
                return "<color=#4E8BEE>" + eqpName + "</color>";
            if (quality == 4)
                return "<color=#7C48D5>" + eqpName + "</color>";
            if (quality == 5)
                return "<color=#FFD800>" + eqpName + "</color>";
            if (quality == 6)
                return "<color=#FFD800>" + eqpName + "</color>";
            if (quality == 7)
                return "<color=#FFD800>" + eqpName + "</color>";
            return "<color=#FFFFFF>" + eqpName + "</color>";

        }
        /// <summary>
        /// 清空所有信息
        /// </summary>
        private void Clear()
        {
            //清空dropDown里的内容
            dropdownPart.ClearOptions();
            SubHeadNode.ListPartIdx?.Clear();
            listPartIdx.Clear();
            //清空target区域包括matlist
            for (int i = targetMatListObj.childCount; i > 0; i--)
                DestroyImmediate(targetMatListObj.GetChild(i - 1).gameObject);
            DestroyImmediate(targetIcon?.gameObject);
            for (int i = transEqpList.childCount; i > 0; i--)
                DestroyImmediate(transEqpList.GetChild(i - 1).gameObject);
            dicSubHead.Clear();
            dicMainObj.Clear();
            dicExpandState.Clear();
        }
        private void OnRelearnByMoney(GameObject go)
        {
            if (PlayerModel.getInstance().money < A3_SmithyModel.getInstance().rlrnMoneyCost)
            {
                flytxt.instance.fly(ContMgr.getCont("off_line_exp_money"));
                return;
            }
            OnRelearn(1 /*使用金币*/ );
        }
        private void OnRelearnByDiamond(GameObject go)
        {
            if (PlayerModel.getInstance().gold < A3_SmithyModel.getInstance().rlrnDiamondCost)
            {
                flytxt.instance.fly(ContMgr.getCont("off_line_exp_gold"));
                return;
            }
            OnRelearn(2 /*使用钻石*/ );
        }
        private void OnRelearn(int costWay)
        {                        
            //检查选择的部位
            int rlrnPart = 0;
            if (tglRlrnArmor.gameObject.activeSelf && tglRlrnArmor.isOn)
                rlrnPart=(int)SMITHY_PART.ARMOR;
            else if (tglRlrnWeapon.gameObject.activeSelf && tglRlrnWeapon.isOn)
                rlrnPart = (int)SMITHY_PART.WEAPON;
            else if (tglRlrnJewelry.gameObject.activeSelf && tglRlrnJewelry.isOn)
                rlrnPart = (int)SMITHY_PART.JEWELRY;
            if (rlrnPart == 0)
                return;
            //隐藏当前界面
            Clear();
            mainPanel.gameObject.SetActive(false);
            //发送协议
            A3_SmithyProxy.getInstance().SendRelearn(rlrnPart, costWay);

            relearnTip.SetActive(false);
        }
        #region 右侧物品栏
        void CreateItemIcon(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
            icon.transform.SetParent(itemParent.transform.GetChild(i), false);
            itemicon[data.id] = icon;

            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);

            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = (GameObject go) => OnItemClick(icon, data.id);
        }

        KeyValuePair<int, GameObject> curEqpInfo;
        a3_BagItemData curBodyEqpInfo;
        void OnItemClick(GameObject go, uint id)
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
                curEqpInfo = new KeyValuePair<int, GameObject>(go.transform.GetSiblingIndex(), go);
                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(one.confdata.equip_type))
                    curBodyEqpInfo = a3_EquipModel.getInstance().getEquipsByType()[one.confdata.equip_type];
                ArrayList data = new ArrayList();
                data.Add(one);
                data.Add(equip_tip_type.BagPick_tip);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
            }
            else if (one.isSummon)
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3TIPS_SUMMON, data);
            }
            else
            {
                ArrayList data = new ArrayList();
                data.Add(one);
                data.Add(equip_tip_type.Bag_tip);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
            }
        }

        void OnItemChange(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("add"))
            {
                if (!(a3_BagModel.getInstance().getItems().Count > a3_BagModel.getInstance().curi))
                {
                    foreach (Variant item in data["add"]._arr)
                    {
                        uint id = item["id"];
                        if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                        {
                            a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                            CreateItemIcon(one, itemicon.Count);
                        }
                    }
                }
            }
            if (data.ContainsKey("modcnts"))
            {
                foreach (Variant item in data["modcnts"]._arr)
                {
                    uint id = item["id"];
                    if (itemicon.ContainsKey(id))
                    {
                        itemicon[id].transform.FindChild("num").GetComponent<Text>().text = item["cnt"]._str;
                        if (item["cnt"]._int32 <= 1)
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(false);
                        else
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(true);
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
                        GameObject go = itemicon[id].transform.parent.gameObject;
                        Destroy(go);
                        itemicon.Remove(id);

                        GameObject item = transform.FindChild("Main/right_bag/item_scroll/scroll_view/icon").gameObject;
                        GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
                        itemclone.SetActive(true);
                        itemclone.transform.SetParent(itemParent.transform, false);
                        itemclone.transform.SetSiblingIndex(itemicon.Count + i);
                    }
                }
            }
            if (targetId != 0)
            {
                if (targetId != RANDOM_ITEM_ID)
                    OnSelectedTargetEqp(targetId);
                if (targetId == RANDOM_ITEM_ID)
                    OnSelectedScroll();
            }
        }

        void onChushou(GameObject go)
        {
            this.transform.FindChild("Main/right_bag/piliang_chushou").gameObject.SetActive(true);
            Money.text = "0";
            SellPutin();
            clearCon();
            OnLoadTitm_chushou();
        }

        //一键出售加入
        void SellPutin()
        {
            foreach (uint it in a3_BagModel.getInstance().getUnEquips().Keys)
            {
                uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;
                if (a3_BagModel.getInstance().getItemDataById(tpid).quality <= 3)
                {
                    if (dic_BagItem_shll.ContainsKey(it))
                    {
                        dic_BagItem_shll.Remove(it);
                    }
                    int num = a3_BagModel.getInstance().getUnEquips()[it].num;
                    dic_BagItem_shll[it] = a3_BagModel.getInstance().getUnEquips()[it];
                    ShowMoneyCount(a3_BagModel.getInstance().getUnEquips()[it].tpid, num, true);
                }
            }
            Dictionary<uint, a3_BagItemData> itemList = a3_BagModel.getInstance().getItems();
            foreach (uint it in itemList.Keys)
            {
                uint tpid = itemList[it].tpid;
                if (itemList[it].confdata.use_type == 2 || itemList[it].confdata.use_type == 3)
                {
                    SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
                    if (PlayerModel.getInstance().up_lvl > xml.getInt("use_limit"))
                    {
                        if (PlayerModel.getInstance().up_lvl == 1) { continue; }
                        if (PlayerModel.getInstance().up_lvl == 3)
                        {
                            if (tpid == 1531)
                                continue;
                            if (tpid == 1532)
                                continue;
                        }
                        if (dic_BagItem_shll.ContainsKey(it))
                        {
                            dic_BagItem_shll.Remove(it);
                        }
                        int num = itemList[it].num;
                        dic_BagItem_shll[it] = itemList[it];
                        ShowMoneyCount(itemList[it].tpid, num, true);
                    }
                }
            }
        }

        void clearCon()
        {
            if (itemcon_chushou.Count > 0)
            {
                foreach (GameObject it in itemcon_chushou.Values)
                {
                    Destroy(it);
                }
            }
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

        //显示出售获得金币数
        void ShowMoneyCount(uint tpid, int num, bool add)
        {
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            if (add)
            {
                GetMoneyNum += (xml.getInt("value") * num);
            }
            else
            {
                GetMoneyNum -= (xml.getInt("value") * num);
            }
            Money.text = GetMoneyNum.ToString();
        }

        void CreateItemIcon_chushou(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, false, data.num);
            icon.transform.SetParent(item_Parent_chushou.transform.GetChild(i), false);
            itemcon_chushou[data.id] = icon;
            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);
        }

        //出售后刷新
        public void refresh_Sell()
        {
            this.transform.FindChild("Main/right_bag/piliang_chushou").gameObject.SetActive(false);
            dic_BagItem_shll.Clear();
            Money.text = 0 + "";
            GetMoneyNum = 0;
        }

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

        void onItemSell(GameEvent e)
        {
            Variant data = e.data;
            uint id = 0;
            if (data.ContainsKey("id"))
                id = data["id"];
            string money = data["earn"];
            //flytxt.instance.fly("获得金钱：" + money);
        }

        private Toggle white, green, blue, purple, orange;
        Dictionary<uint, a3_BagItemData> dic_BagItem = new Dictionary<uint, a3_BagItemData>();
        public int mojing_num;
        public int shengguanghuiji_num;
        public int mifageli_num;
        int conIndex = 0;

        Text mojing;
        Text shengguanghuiji;
        Text mifageli;
        private Dictionary<uint, GameObject> itemcon_fenjie = new Dictionary<uint, GameObject>();
        //显示分解物品        
        void OnLoadItem_fenjie()
        {
            if (dic_BagItem.Count > 0)
            {
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (itemcon_fenjie.ContainsKey(it)) { continue; }
                    CreateItemIcon_fenjie(dic_BagItem[it], conIndex);
                    conIndex++;
                }
            }
            setfenjieCon();
        }
        private GridLayoutGroup item_Parent_fenjie;
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
            Vector2 newSize = new Vector2(con.sizeDelta.x, (childSizeY + spacing) * h);
            con.sizeDelta = newSize;
        }
        void CreateItemIcon_fenjie(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            icon.transform.SetParent(item_Parent_fenjie.transform.GetChild(i), false);
            itemcon_fenjie[data.id] = icon;
            BaseButton bs_bt = new BaseButton(icon.transform);
            uint id = data.id;
            bs_bt.onClick = delegate (GameObject go)
            {
                ArrayList data1 = new ArrayList();
                a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                data1.Add(one);
                data1.Add(equip_tip_type.tip_forfenjie);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
            };
        }
        //批量分解取出
        public void EquipsNoSell(int quality = 0)
        {
            List<uint> removelist = new List<uint>();
            foreach (uint it in dic_BagItem.Keys)
            {
                if (dic_BagItem[it].confdata.quality == quality)
                {
                    removelist.Add(it);
                    showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, false);
                }
            }
            foreach (uint i in removelist)
            {
                dic_BagItem.Remove(i);
            }
        }
        //显示分解获得物品数量
        void showItemNum(uint tpid, bool add)
        {
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            List<SXML> xmls = xml.GetNodeList("decompose");
            foreach (SXML x in xmls)
            {
                switch (x.getInt("item"))
                {
                    case 1540:
                        if (add)
                            mojing_num += x.getInt("num");
                        else
                        {
                            mojing_num -= x.getInt("num");
                        }
                        mojing.text = mojing_num.ToString();
                        break;
                    case 1541:
                        if (add)
                            shengguanghuiji_num += x.getInt("num");
                        else
                        {
                            shengguanghuiji_num -= x.getInt("num");
                        }
                        shengguanghuiji.text = shengguanghuiji_num.ToString();
                        break;
                    case 1542:
                        if (add)
                            mifageli_num += x.getInt("num");
                        else
                        {
                            mifageli_num -= x.getInt("num");
                        }
                        mifageli.text = mifageli_num.ToString();
                        break;
                }
            }
        }

        //开始分解
        List<uint> dic_leftAllid = new List<uint>();
        void Sendproxy(GameObject go)
        {
            dic_leftAllid.Clear();
            foreach (uint i in dic_BagItem.Keys)
            {
                dic_leftAllid.Add(i);
            }
            EquipProxy.getInstance().sendsell(dic_leftAllid);
            onfenjieclose(null);
            this.transform.FindChild("Main/right_bag/piliang_fenjie").gameObject.SetActive(false);
        }

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
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing")+"," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num == 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang")+"," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num != 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang") + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            dic_BagItem.Clear();
            clearCon_fenjie();
            conIndex = 0;
            mojing_num = 0;
            shengguanghuiji_num = 0;
            mifageli_num = 0;
            mojing.text = 0 + "";
            shengguanghuiji.text = 0 + "";
            mifageli.text = 0 + "";
            purple.isOn = false;
            blue.isOn = false;
            green.isOn = false;
            white.isOn = false;
            orange.isOn = false;
        }
        void onfenjieclose(GameObject go)
        {
            dic_BagItem.Clear();
            clearCon_fenjie();
            conIndex = 0;
            mojing.text = 0 + "";
            shengguanghuiji.text = 0 + "";
            mifageli.text = 0 + "";
            transform.FindChild("Main/right_bag/piliang_fenjie").gameObject.SetActive(false);
        }
        void onOpenLockRec(GameEvent e)
        {
            for (int i = 50; i < itemParent.transform.childCount; i++)
            {
                GameObject lockig = itemParent.transform.GetChild(i).FindChild("lock").gameObject;
                if (i >= a3_BagModel.getInstance().curi)
                {
                    lockig.SetActive(true);
                    int tag = i + 1;
                    BaseButton btn = new BaseButton(lockig.transform);
                    btn.onClick = delegate (GameObject go) { onOpenLock(lockig, tag); };
                }
                else
                    lockig.SetActive(false);                
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
        public void EquipsSureSell(int quality = 0)
        {
            foreach (uint it in a3_BagModel.getInstance().getUnEquips().Keys)
            {
                uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;

                if (a3_BagModel.getInstance().getItemDataById(tpid).quality == quality)
                {
                    if (!a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getUnEquips()[it]))
                    {
                        continue;
                    }
                    if (a3_EquipModel.getInstance().getEquipByAll(it).ismark)
                    {
                        continue;
                    }
                    if (dic_BagItem.ContainsKey(it))
                    {
                        dic_BagItem.Remove(it);
                    }
                    dic_BagItem[it] = a3_BagModel.getInstance().getUnEquips()[it];
                    showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, true);
                }
            }
        }
        //分解物品去除
        public void outItemCon_fenjie(int type = -1, uint id = 0)
        {
            GameObject con = item_Parent_fenjie.transform.parent.FindChild("icon").gameObject;
            if (type != -1)
            {
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (dic_BagItem[it].confdata.quality == type)
                    {
                        conIndex--;
                        Destroy(itemcon_fenjie[it].transform.parent.gameObject);
                        itemcon_fenjie.Remove(it);
                        GameObject clon = Instantiate(con).gameObject;
                        clon.transform.SetParent(item_Parent_fenjie.transform, false);
                        clon.SetActive(true);
                        clon.transform.SetAsLastSibling();
                    }
                }
            }
            else if (id > 0)
            {
                Destroy(itemcon_fenjie[id].transform.parent.gameObject);
                itemcon_fenjie.Remove(id);
                dic_BagItem.Remove(id);
                showItemNum(a3_BagModel.getInstance().getUnEquips()[id].tpid, false);
                GameObject clon = Instantiate(con).gameObject;
                clon.transform.SetParent(item_Parent_fenjie.transform, false);
                clon.SetActive(true);
                clon.transform.SetAsLastSibling();
                conIndex--;
            }
            setfenjieCon();
        }
        void onEquipSell(GameObject go)
        {

            //EquipsNoSell(1);
            white.isOn = false;
            //EquipsNoSell(2); 
            green.isOn = false;
            //EquipsNoSell(3); 
            blue.isOn = false;
            //EquipsNoSell(4); 
            purple.isOn = false;
            //EquipsNoSell(5);
            orange.isOn = false;
            this.transform.FindChild("Main/right_bag/piliang_fenjie").gameObject.SetActive(true);
            mojing_num = 0;
            shengguanghuiji_num = 0;
            mifageli_num = 0;
            clearCon_fenjie();
            OnLoadItem_fenjie();
            //transform.FindChild("Main/right_bag/item_scroll/equip/tishi").gameObject.SetActive(false);

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

            cur_num = (int)Math.Floor(rate * (150 - a3_BagModel.getInstance().curi));
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
                    str = ContMgr.getCont("A3_Smithy_txt4", new List<string>() { (5 * cur_num).ToString(), cur_num.ToString() });
                    break;
                case 2:
                    str = ContMgr.getCont("A3_Smithy_txt5", new List<string>() { (5 * cur_num).ToString(), cur_num.ToString() });
                    break;
            }
            transform.FindChild("panel_open/desc").GetComponent<Text>().text = str;
        }
        void onOpenLock(GameObject go)
        {
            transform.FindChild("panel_open").gameObject.SetActive(false);
            if (open_choose_tag == 1)
                BagProxy.getInstance().sendOpenLock(2, cur_num, true);
            else
                BagProxy.getInstance().sendOpenLock(2, cur_num, false);
        }
        #endregion
    }
    enum EquipPart
    {
        All = 0,
        Head = 1,
        Shoulder = 2,
        MainArmor = 3,
        Leg = 4,
        Foot = 5,
        MainWeapon = 6,
        CoWeapon = 7,
        Necklace = 8,
        LeftRing = 9,
        RightRing = 10
    }
    enum ExpandState
    {
        NotInitialized = 0,
        Expanded = 1,
        Collaspsed = 2
    }

    class SubHeadNode
    {
        private Dictionary<uint /* tpid */ , KeyValuePair<int /* part */ , GameObject>> items; // 所有子显示项

        public static List<int> ListPartIdx;
        public RectTransform RectScroll;
        public Transform content;
        public GameObject HeadObj; // 父显示项
        public LayoutElement layoutElement;
        public SubHeadNode(GameObject go)
        {
            items = new Dictionary<uint, KeyValuePair<int, GameObject>>();
            HeadObj = go;
            content = go.transform.FindChild("scroll/content");
            layoutElement = go.GetComponent<LayoutElement>();
            RectScroll = content?.GetComponent<RectTransform>();
        }
        public void FixHeight()
        {
            layoutElement.minHeight = Mathf.Min(content.GetComponent<RectTransform>().sizeDelta.y, A3_Smithy.Instance.transEqpList.GetComponent<RectTransform>().sizeDelta.y);
        }
        /// <summary>
        /// 添加一个子显示项到父显示项
        /// </summary>
        /// <param name="item">单个子显示项</param>
        public void Add(uint tpid, KeyValuePair<int, GameObject> item)
        {
            if (!items.ContainsKey(tpid))
            {
                items.Add(tpid, item);
                item.Value.transform.SetParent(content, false);
            }
        }

        /// <summary>
        /// 根据所选部位显示装备列表
        /// </summary>
        /// <param name="part">装备部位</param>
        public void ShowItemByPart(int part)
        {
            List<uint> idx = new List<uint>(items.Keys);
            for (int i = 0; i < idx.Count; i++)
            {
                int eqpLv = a3_BagModel.getInstance().getItemDataById(idx[i]).equip_level;
                bool isAbleToShow = (part == items[idx[i]].Key || (part == 0 && ListPartIdx.Contains(items[idx[i]].Key)));
                isAbleToShow = isAbleToShow && eqpLv <= A3_SmithyModel.getInstance().GetMaxAllowedSetLevel(A3_SmithyModel.getInstance().CurSmithyLevel);
                items[idx[i]].Value.SetActive(isAbleToShow);
                items[idx[i]].Value.transform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// 计算scroll的高度
        /// </summary>
        /// <param name="part">所选的部位</param>
        /// <returns>scroll的高度</returns>
        public float GetFixedHeight(int part)
        {            
            int i = 0;
            uint count = 0;
            for (List<uint> idx = new List<uint>(items.Keys); i < idx.Count; i++)
                if ((part == 0 && ListPartIdx.Contains(items[idx[i]].Key) || part == items[idx[i]].Key) && items[idx[i]].Value.activeSelf)
                    count++;
            return count * content.GetComponent<VerticalLayoutGroup>().spacing;
        }
        public void FixContentHeight()
        {
            int visibleChildCount = 0;
            float? firstY = null, nextY = null;
            for (int i = 0; i < content.childCount; i++)
                if (content.transform.GetChild(i).gameObject.activeSelf)
                    visibleChildCount++;
            for (int i = 0; i < content.childCount; i++)
            {
                if (content.transform.GetChild(i).gameObject.activeSelf)
                {
                    if (!firstY.HasValue)
                        firstY = content.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
                    else if (!nextY.HasValue)
                        nextY = content.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
                    else
                        break;
                }
            }
            if (!nextY.HasValue) return;
            content.GetComponent<LayoutElement>().minHeight = (firstY.Value - nextY.Value) * visibleChildCount;
        }
    }
}
