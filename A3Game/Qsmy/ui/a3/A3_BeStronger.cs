using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using EquipStrengthOption = MuGame.a3_EquipModel.EquipStrengthOption;
using System.Collections;

namespace MuGame
{
    /// <summary>
    /// 我要变强
    /// </summary>
    class A3_BeStronger : FloatUi
    {
        ScrollRect scrollRect_contentShown;
        public Transform ClickPanel;
        public static A3_BeStronger Instance;
        public static Transform ContentHiden;

        public Transform Owner { get { return transform.parent; } set { transform.SetParent(value,false); } }
        public Dictionary<Title_BtnStronger, BeStrongerBtn> btnInContent = new Dictionary<Title_BtnStronger, BeStrongerBtn>();

        public Transform template;
        public RectTransform ContentShown2D;
        public Transform upBtn;
        public GameObject up_sub_btnPrefab;
        public Transform ContentShown;
        public int ShownItemNum => ContentShown?.childCount ?? 0;
        public bool Show { set { ContentShown.gameObject.SetActive(value); } }
        static Action<GameObject> handler;
        static A3_BeStronger()
        {
            handler = (GameObject go) =>
            {
                if (go?.transform.Equals(Instance?.upBtn) ?? false)
                    return;
                if (Instance?.ContentShown.gameObject?.activeSelf ?? false && !(Instance?.btnInContent.ContainsValue(go) ?? !false))
                {
                    Instance.ContentShown.gameObject.SetActive(false);
                    Instance.ClickPanel.gameObject.SetActive(false);
                }
            };
            BaseButton.Handler = handler;
        }
        A3_BeStronger() { }

        void Btn_RefreshOrCreate(bool b_check, Title_BtnStronger buttonTitle)
        {
            if (b_check)
            {
                if (!btnInContent.ContainsKey(buttonTitle))
                    CreateButton(buttonTitle, Instantiate(up_sub_btnPrefab).transform);
                btnInContent[buttonTitle].State = HideOrShown.Shown;
            }
            else if (btnInContent.ContainsKey(buttonTitle))
                btnInContent[buttonTitle].State = HideOrShown.Hide;
        }

        void OnUpBtnClick(GameObject go)
        {
            if (ContentShown.gameObject.activeSelf)
            {
                ContentShown.gameObject.SetActive(false);                
                return;
            }
            CheckUpItem();
            ContentShown.gameObject.SetActive(true);
            ClickPanel.gameObject.SetActive(true);
            a1_gamejoy.inst_skillbar.transform.SetAsFirstSibling();
        }


        public bool CheckUpItem()
        {
            if (PlayerModel.getInstance().inFb)
                return false;
            // 检查加点
            if (PlayerModel.getInstance().up_lvl > 0)
                Btn_RefreshOrCreate(PlayerModel.getInstance()?.pt_att > 0   , Title_BtnStronger.Player_Attribute);
            // 检查装备强化
            EquipStrengthOption availableOptions = a3_EquipModel.getInstance().CheckEquipStrengthAvailable()[true];
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_ENCHANT))
                Btn_RefreshOrCreate((availableOptions & EquipStrengthOption.Add) != 0x00, Title_BtnStronger.Equipment_Add);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_MOUNTING))
                Btn_RefreshOrCreate((availableOptions & EquipStrengthOption.Gem) != 0x00, Title_BtnStronger.Equipment_Gem);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_ENHANCEMENT))
                Btn_RefreshOrCreate((availableOptions & EquipStrengthOption.Intensify) != 0x00, Title_BtnStronger.Equipment_Intensify);
            //Btn_RefreshOrCreate((availableOptions & EquipStrengthOption.Stage) != 0x00, Title_BtnStronger.Equipment_Stageup);
            // 检查飞翼
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING))
                Btn_RefreshOrCreate(A3_WingModel.getInstance()?.CheckLevelupAvailable() ?? false, Title_BtnStronger.Wings);
            // 检查宠物
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
            //    Btn_RefreshOrCreate(A3_PetModel.getInstance()?.CheckLevelupAvaiable() ?? false, Title_BtnStronger.Pet);
            // 检查护盾
            //Btn_RefreshOrCreate(HudunModel.getInstance().CheckLevelupAvailable(), Title_BtnStronger.Shield);
            // 检查技能
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SKILL))
                Btn_RefreshOrCreate(Skill_a3Model.getInstance()?.CheckSkillLevelupAvailable() ?? false, Title_BtnStronger.Skill_LevelUp);
            // 检查符文

            // 检查军衔
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.ACHIEVEMENT))
                Btn_RefreshOrCreate(a3_RankModel.getInstance()?.CheckTitleLevelupAvailable() ?? false, Title_BtnStronger.Title);
            RefreshView();
            if (ShownItemNum > 0) {                
                upBtn?.gameObject.SetActive(true);
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);
                return true;
            } else {
                upBtn?.gameObject.SetActive(false);
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
                return false;
           }
        }

        public void CreateButton(Title_BtnStronger title, Transform btn)
        {
            BeStrongerBtn _btn = new BeStrongerBtn
            {
                button = btn,
                State = HideOrShown.Shown
            };
            if (btnInContent.ContainsKey(title))
                btnInContent[title] = _btn;
            else
                btnInContent.Add(title, _btn);
            _btn.initBtnFunc(title);
        }

        public override void init()
        {
            Instance = this;
            upBtn = transform.FindChild("upbtn");
            ContentHiden = upBtn.transform.FindChild("HideView");
            ContentShown = upBtn.transform.FindChild("ShowMask/ShowView");
            ClickPanel = upBtn.transform.FindChild("ClickPanel");
            ContentShown2D = ContentShown.GetComponent<RectTransform>();
            scrollRect_contentShown = ContentShown.GetComponent<ScrollRect>();
            new BaseButton(upBtn).onClick = OnUpBtnClick;
            template = upBtn.FindChild("template");
            up_sub_btnPrefab = template.FindChild("sub_btn").gameObject;
            Owner = a1_gamejoy.inst_skillbar.transform.FindChild("skillbar/combat");
            transform.SetAsLastSibling();
            //transform.SetAsFirstSibling();
            new BaseButton(ClickPanel).onClick = 
                (GameObject go) =>
                {
                    ContentShown?.gameObject.SetActive(false);
                    ClickPanel.gameObject.SetActive(false);
                };
            // ContentShown.gameObject.AddComponent<DragHelper>();
            EventTriggerListener.Get(a1_gamejoy.inst_joystick.Stick).onDown += (GameObject go) => handler(go);
            // 对于一些加载顺序一定在变强之前的对象,需要通过这种方式挂句柄
            Button[] btns = a1_gamejoy.inst_skillbar.GetComponentsInChildren<Button>();
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i].onClick.AddListener(delegate () { handler(null); });
            }
        }


        public override void onShowed() => CheckUpItem();

        public void RefreshView()
        {
            List<Title_BtnStronger> btnDicIdx = new List<Title_BtnStronger>(btnInContent.Keys);
            for (int i = 0; i < btnDicIdx.Count; i++)
            {
                switch (btnInContent[btnDicIdx[i]].State)
                {
                    default:
                    case HideOrShown.Hide:
                        btnInContent[btnDicIdx[i]].button.SetParent(ContentHiden, false);
                        break;
                    case HideOrShown.Shown:
                        btnInContent[btnDicIdx[i]].button.SetParent(ContentShown, false);
                        break;
                }
            }
        }

        public void HideShownPanel()
        {
            ContentShown?.gameObject.SetActive(false);
            ClickPanel?.gameObject.SetActive(false);
        }
        public static implicit operator bool(A3_BeStronger any) => Instance != null;        
    }

    public enum HideOrShown
    {
        Shown = 0,
        Hide = 1,
    }

    public enum Title_BtnStronger
    {
        Player_Attribute = 0,       // 角色属性点
        Equipment_Add = 1,          // 装备追加
        Equipment_Gem = 2,          // 装备宝石镶嵌
        Equipment_Intensify = 3,    // 装备强化
        Equipment_Stageup = 4,      // 装备进阶
        Wings = 5,                  // 飞翼强化
        Pet = 6,                    // 宠物升级
        Shield = 7,                 // 护盾
        Skill_LevelUp = 8,          // 技能升级
        Title = 9,                  // 军衔
    }

    public class BeStrongerBtn
    {
        HideOrShown currentState;

        public Transform button;
        public HideOrShown State
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public void initBtnFunc(Title_BtnStronger title)
        {
            Action<GameObject> onBtnClick = null;
            switch (title)
            {
                case Title_BtnStronger.Player_Attribute: // 角色加点
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_att");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        button.name = "stronger_att_btn";
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ROLE);
                        a3_role.ForceIndex = 1;
                    };
                    break;
                case Title_BtnStronger.Equipment_Add: // 装备追加
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_add");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
                        a3_equip.instance.tabIndex = 5;
                    };
                    break;
                case Title_BtnStronger.Equipment_Gem: // 宝石镶嵌
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_gem");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
                        a3_equip.instance.tabIndex = 4;
                    };
                    break;
                case Title_BtnStronger.Equipment_Intensify: // 装备强化
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_intensify");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
                        if (a3_equip.instance!=null)
                            a3_equip.instance.tabIndex = 0;
                    };
                    break;
                //case Title_BtnStronger.Equipment_Stageup:
                //TextShadow.text =
                //Text.text = "装备进阶";
                //A3_BeStronger.Instance.template.FindChild("stronger_stage").SetParent(button);
                //onBtnClick = (GameObject go) =>
                //{
                //    A3_BeStronger.Instance.ContentShown.gameObject.SetActive(false);
                //    InterfaceMgr.getInstance().open(InterfaceMgr.A3_EQUIP);
                //    a3_equip.instance.tabIndex = 3;
                //};
                //break;
                case Title_BtnStronger.Wings: // 飞翼
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_wing");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WIBG_SKIN);
                    };
                    break;
                case Title_BtnStronger.Pet: // 宠物
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_pet");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_PET_SKIN);
                    };
                    break;
                case Title_BtnStronger.Shield: // 护盾
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_shield");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HUDUN);
                    };
                    break;
                case Title_BtnStronger.Skill_LevelUp: // 升级技能
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_skill");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SKILL_A3);
                    };
                    break;
                case Title_BtnStronger.Title: // 提升军衔
                    if (button.childCount == 0)
                    {
                        Transform img = A3_BeStronger.Instance.template.FindChild("stronger_title");
                        img.SetParent(button, false);
                        img.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                    onBtnClick = (GameObject go) =>
                    {
                        A3_BeStronger.Instance.HideShownPanel();
                        //ArrayList arrs = new ArrayList();
                        //arrs.Add(1);
                        //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACHIEVEMENT, arrs);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RANK);
                    };
                    break;
                default:
                    break;
            }
            new BaseButton(button).onClick = onBtnClick;
        }
        public static implicit operator BeStrongerBtn(GameObject go)
        {
            BeStrongerBtn btnObj = new BeStrongerBtn();
            btnObj.button = go?.transform;
            return btnObj;
        }
        public static implicit operator GameObject(BeStrongerBtn btnObj)=> btnObj.button?.gameObject;
        public override bool Equals(object obj) => button.Equals((obj as BeStrongerBtn).button);
        public override int GetHashCode() => button.GetHashCode();
    }
}
