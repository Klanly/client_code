using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace MuGame
{
    class a3_active : Window
    {

        public static bool MwlrIsDoing = false;
        #region variable
        a3BaseActive _currentActive = null;
        Dictionary<string, a3BaseActive> _activies = new Dictionary<string, a3BaseActive>();
        public static a3_active instance;
        public static a3_active onshow;
        //int tabid = 1;
        #endregion
        bool Toclose = false;
        #region public
        public bool map_light;
        public override void init()
        {
            //== 活动配置(where [$name] -> tabs/$name == contents/$name)
            _activies["mlzd"] = new a3_active_mlzd(this, "contents/mlzd");                  // 魔炼之地
            _activies["summonpark"] = new a3_active_zhsly(this, "contents/summonpark");     // 召唤兽乐园
            _activies["mwlr"] = new a3_active_mwlr(this, "contents/mwlr");                  // 魔物猎人
           // _activies["pvp"] = new a3_active_pvp(this, "contents/pvp");                     // 竞技场
            _activies["forchest"] = new a3_active_forchest(this, "contents/forchest");           // 抢宝箱
            _activies["findbtu"] = new a3_active_findbtu(this, "contents/findbtu");         //藏宝图
            //== 布局
            InitLayout();
            //InitLayout2();
            //== 关闭窗口
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) =>
            {
                Toclose = true;
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
            };

            CheckLock();
            this.transform.FindChild("tach").gameObject.SetActive(false) ;
            instance = this;
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, (GameEvent e) =>
            {
                //放弃魔物猎人
                if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DEVIL_HUNTER) && a3_active_mwlr_kill.Instance.Count != 0)
                {
                    A3_ActiveProxy.getInstance().SendGiveUpHunt();
                }
            });

            inText();
        }

        void inText()
        {
            this.transform.FindChild("scroll_view/contain/mlzd/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_1");
            this.transform.FindChild("scroll_view/contain/summonpark/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_2");
            this.transform.FindChild("scroll_view/contain/mwlr/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_3");
            this.transform.FindChild("scroll_view/contain/forchest/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_4");
            this.transform.FindChild("scroll_view/contain/findbtu/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_5");

            this.transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_34");
            this.transform.FindChild("tip/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_35");
        }
        public string openView = null;
        public string pastopen = null;
        public override void onShowed()
        {
            Toclose = false;
            A3_ActiveProxy.getInstance().SendGetHuntInfo();
            //ref_into();
            if (uiData != null)
            {
                string showtabId = (string)uiData[0];
                ShowTabContent(showtabId);
            }
            //else if (tabid > 0) ShowTabContent(0);
            else if (_currentActive != null) _currentActive.onShowed();
            if(GRMap.GAME_CAMERA!=null)
                GRMap.GAME_CAMERA.SetActive(false);
            var rt = getComponentByPath<RectTransform>("scroll_view/contain");
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 0);
            GridLayoutGroup sizey = rt.transform.GetComponent<GridLayoutGroup>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.transform.childCount * sizey.cellSize.y);
            onshow = this;
            for (int i = 0; i < rt.transform.childCount; i++)
            {
                if (rt.transform.GetChild(i).gameObject.activeSelf)
                {
                    openView = rt.transform.GetChild(i).gameObject.name;
                    break;
                }
            }
            if (uiData == null && openView != null && pastopen == null)
            {
                ShowTabContent(openView);
                openView = null;
            }
            else if (uiData == null && pastopen != null)
            {
                ShowTabContent(pastopen);
            }
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        public override void onClosed()
        {
            if (a3_active_mwlr.instance != null)
                a3_active_mwlr.instance.getTransformByPath("timer/timerRec").GetComponent<Text>().text = "00:00:00";
            if (_currentActive != null)
                _currentActive.onClose();
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            if (a3_active_mwlr_kill.Instance != null&& a3_active_mwlr_kill.Instance.MHInfo && a3_active_mwlr_kill.Instance.MHInfo.gameObject && a3_active_mwlr_kill.Instance.NewAction)
            {
                if (a3_active_mwlr_kill.Instance.NewAction)
                    a3_active_mwlr_kill.Instance.MHAnimator?.SetTrigger("start");
                a3_active_mwlr_kill.Instance.Reset();
                a3_active_mwlr_kill.Instance.NewAction = false;
            }
            else if (A3_ActiveModel.getInstance().mwlr_giveup)
            {
                a3_active_mwlr_kill.Instance.Clear();
            }
            onshow = null;
            //if (a3_itemLack.intans && a3_itemLack.intans.closewindow != null)
            //{
            //    if (Toclose)
            //    {
            //        InterfaceMgr.getInstance().ui_async_open(a3_itemLack.intans.closewindow);
            //        a3_itemLack.intans.closewindow = null;
            //        Toclose = false;
            //    }
            //    else
            //    {
            //        a3_itemLack.intans.closewindow = null;
            //    }
            //}
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
            if (a3_getJewelryWay.instance && a3_getJewelryWay.instance.closeWin != null && Toclose)
            {
                InterfaceMgr.getInstance().ui_async_open(a3_getJewelryWay.instance.closeWin);
                a3_getJewelryWay.instance.closeWin = null;
            }
        }

        public void CheckLock()
        {
            getGameObjectByPath("scroll_view/contain/mlzd").gameObject.SetActive(false);
            getGameObjectByPath("scroll_view/contain/summonpark").gameObject.SetActive(false);
            getGameObjectByPath("scroll_view/contain/mwlr").gameObject.SetActive(false);
            //getGameObjectByPath("scroll_view/contain/pvp").gameObject.SetActive(false);
            getGameObjectByPath("scroll_view/contain/forchest").gameObject.SetActive(false);



            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.CHASTEN_JAIL))
            {
                OpenMLZD();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_PARK))
            {
                OpenSummon();
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DEVIL_HUNTER))
            {
                OpenMWLR();
            }
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PVP_DUNGEON))
            //{
            //    OpenPVP();
            //}
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.FOR_CHEST))
            {
                OpenForChest();
            }
        }

        //============
        public void OpenMLZD()
        {
            getGameObjectByPath("scroll_view/contain/mlzd").gameObject.SetActive(true);
        }
        public void OpenSummon()
        {
            getGameObjectByPath("scroll_view/contain/summonpark").gameObject.SetActive(true);
        }
        public void OpenMWLR()
        {
            getGameObjectByPath("scroll_view/contain/mwlr").gameObject.SetActive(true);
        }
        //public void OpenPVP()
        //{
        //    getGameObjectByPath("scroll_view/contain/pvp").gameObject.SetActive(true);
        //}
        public void OpenForChest()
        {
            getGameObjectByPath("scroll_view/contain/forchest").gameObject.SetActive(true);
        }
        //============
        void Update()
        {
            if (a3_active_mwlr.instance != null)
                a3_active_mwlr.instance.Re_Time();
            //if (a3_active_worldboss.instance != null) a3_active_worldboss.instance.Update();

            //if (a3_active_mlzd .instans != null) {
            //    a3_active_mlzd.instans.refreTime();
            //}

            if (a3_active_mlzd.instans != null) {
                a3_active_mlzd.instans._update();
            }
        }

        int Time_bt = 0;
        public void Runtimer_bt(int time)
        {
            Time_bt = time;
            CancelInvoke("runTime_bt");
            InvokeRepeating("runTime_bt", 0, 1);
        }
        void runTime_bt()
        {
            Time_bt--;
            if (a3_active_findbtu.instans != null)
            {
                a3_active_findbtu.instans.showtime(Time_bt);
            }
            else
            {
                CancelInvoke("runTime_bt");
                return;
            }
            if (Time_bt <= 0)
            {
                Time_bt = 0;
                CancelInvoke("runTime_bt");
                FindBestoProxy.getInstance().getinfo();
            }
        }

        #endregion

        #region private
        void InitLayout()
        {
            Transform contentsRoot = getGameObjectByPath("contents").transform;
            foreach (var v in contentsRoot.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == contentsRoot)
                {
                    v.gameObject.SetActive(false);
                }
            }


            var tabctt = getGameObjectByPath("scroll_view/contain");
            foreach (var v in tabctt.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == tabctt.transform)
                {
                    v.GetComponent<Button>().interactable = true;
                    new BaseButton(v.transform).onClick = Tab_click;
                }
            }
        }

        //void ShowTabContent(int tabid)
        //{
        //    Transform contentsRoot = getGameObjectByPath("contents").transform;
        //    var cttnt = contentsRoot.GetChild(tabid);
        //    if (cttnt != null)
        //    {
        //        Tab_click(cttnt.gameObject);
        //    }
        //}

        void ShowTabContent(string tabname)
        {
            Transform contentsRoot = getGameObjectByPath("contents").transform;
            var cttnt = contentsRoot.FindChild(tabname);
            pastopen = tabname;
            if (cttnt != null)
            {
                Tab_click(cttnt.gameObject);
            }
        }

        void Tab_click(GameObject go)
        {
            if (!_activies.ContainsKey(go.name))
                return;

            pastopen = go.name;
            Transform contentsRoot = getGameObjectByPath("contents").transform;
            foreach (var v in contentsRoot.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == contentsRoot)
                {
                    v.gameObject.SetActive(false);
                }
            }
            var cttnt = contentsRoot.FindChild(go.name);
            if (cttnt != null)
            {
                cttnt.gameObject.SetActive(true);
            }
            var tabctt = getGameObjectByPath("scroll_view/contain");
            foreach (var v in tabctt.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == tabctt.transform)
                {
                    v.GetComponent<Button>().interactable = true;
                }
            }
            var vgt = go.GetComponent<Button>();
            if (vgt == null) vgt = tabctt.transform.FindChild(go.name).GetComponent<Button>();
            if (vgt != null) vgt.interactable = false;

            if (_currentActive != null)
            {
                _currentActive.onClose();
            }
            if (_activies.ContainsKey(go.name))
            {
                _currentActive = _activies[go.name];
                _currentActive.onShowed();
            }
        }
        #endregion
    }

    class a3BaseActive : Skin
    {


        public a3_active main { get; set; }
        public string pathStrName { get; set; }
        public a3BaseActive(Window win, string pathStr)
            : base(win.getTransformByPath(pathStr))
        {
            var ss = pathStr.Split('/');
            this.pathStrName = ss[Mathf.Max(0, ss.Length - 1)];//返回当中的最大值
            main = win as a3_active;
            init();
        }
        public virtual void init()
        {

        }
        public virtual void onShowed()
        {

        }
        public virtual void onClose() { }
    }
    #region  移植到a3_counterpart
    //金币副本
    //class a3_active_gold : a3BaseActive
    //{
    //    public a3_active_gold(Window win, string pathStr)
    //        : base(win, pathStr)
    //    {
    //    }
    //    BaseButton enterbtn;
    //    public override void init()
    //    {
    //        //进入副本
    //        enterbtn = new BaseButton(getTransformByPath("enter"));
    //        enterbtn.onClick = (GameObject go) =>
    //        {
    //            if (true)
    //            {
    //                Variant sendData = new Variant();
    //                sendData["mapid"] = 3335;
    //                sendData["npcid"] = 0;
    //                sendData["ltpid"] = 102;
    //                sendData["diff_lvl"] = 1;
    //                LevelProxy.getInstance().sendCreate_lvl(sendData);
    //            }
    //        };
    //    }
    //    public override void onShowed()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(101);
    //        int tm = data["tm"];
    //        TimeSpan ts = new TimeSpan(0, tm, 0);
    //        getTransformByPath("cue/time").GetComponent<Text>().text = "副本时间： " + ts.Hours + "时" + ts.Minutes + "分" + ts.Seconds + "秒";
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>0/1</color> （每天0点重置）";
    //        getTransformByPath("cue/reword").GetComponent<Text>().text = "副本奖励： <color=#ffff00>大量金币</color>";
    //        RefreshLeftTimes();
    //    }
    //    public override void onClose()
    //    {
    //    }

    //    void RefreshLeftTimes()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(102);
    //        int max_times = data["daily_cnt"];
    //        int use_times = 0;
    //        if (MapModel.getInstance().dFbDta.ContainsKey(102))
    //        {
    //            use_times = Mathf.Min(MapModel.getInstance().dFbDta[102].cycleCount, max_times);
    //        }
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>" + use_times + "/" + max_times + "</color> （每天0点重置）";
    //        if ((max_times - use_times) <= 0)
    //        {
    //            enterbtn.interactable = false;
    //        }
    //        else enterbtn.interactable = true;
    //    }
    //}

    //经验副本
    //class a3_active_exp : a3BaseActive
    //{
    //    public static int diff = 0;
    //    public a3_active_exp(Window win, string pathStr)
    //        : base(win, pathStr)
    //    {
    //    }
    //    BaseButton enterbtn;
    //    public override void init()
    //    {
    //        //进入副本
    //        enterbtn = new BaseButton(getTransformByPath("enter"));
    //        enterbtn.onClick = (GameObject go) =>
    //        {
    //            if (true)
    //            {
    //                debug.Log("Enter");
    //                Variant sendData = new Variant();
    //                sendData["mapid"] = 3334;
    //                sendData["npcid"] = 0;
    //                sendData["ltpid"] = 101;
    //                sendData["diff_lvl"] = 1;
    //                diff = sendData["diff_lvl"];
    //                LevelProxy.getInstance().sendCreate_lvl(sendData);
    //            }
    //        };
    //    }
    //    public override void onShowed()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(101);
    //        int tm = data["tm"];
    //        TimeSpan ts = new TimeSpan(0, tm, 0);
    //        getTransformByPath("cue/time").GetComponent<Text>().text = "副本时间： " + ts.Hours + "时" + ts.Minutes + "分" + ts.Seconds + "秒";
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>0/1</color> （每天0点重置）";
    //        getTransformByPath("cue/reword").GetComponent<Text>().text = "副本奖励： <color=#ff9900>大量经验</color>";
    //        RefreshLeftTimes();
    //    }

    //    void RefreshLeftTimes()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(101);
    //        int max_times = data["daily_cnt"];
    //        int use_times = 0;
    //        if (MapModel.getInstance().dFbDta.ContainsKey(101))
    //        {
    //            use_times = Mathf.Min(MapModel.getInstance().dFbDta[101].cycleCount, max_times);
    //        }
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>" + use_times + "/" + max_times + "</color> （每天0点重置）";
    //        if ((max_times - use_times) <= 0)
    //        {
    //            enterbtn.interactable = false;
    //        }
    //        else enterbtn.interactable = true;
    //    }
    //    public override void onClose()
    //    {
    //    }
    //}

    //风神王座
    //class a3_active_fswz : a3BaseActive
    //{
    //    public a3_active_fswz(Window win, string pathStr)
    //        : base(win, pathStr)
    //    {
    //    }
    //    BaseButton enterbtn;
    //    public override void init()
    //    {
    //        //进入副本
    //        enterbtn = new BaseButton(getTransformByPath("enter"));
    //        enterbtn.onClick = (GameObject go) =>
    //        {
    //            if (true)
    //            {
    //                debug.Log("Enter");
    //                Variant sendData = new Variant();
    //                sendData["mapid"] = 3339;
    //                sendData["npcid"] = 0;
    //                sendData["ltpid"] = 103;
    //                sendData["diff_lvl"] = 1;
    //                LevelProxy.getInstance().sendCreate_lvl(sendData);
    //            }
    //        };
    //    }
    //    public override void onShowed()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(103);
    //        int tm = data["tm"];
    //        TimeSpan ts = new TimeSpan(0, tm, 0);
    //        getTransformByPath("cue/time").GetComponent<Text>().text = "副本时间： " + ts.Hours + "时" + ts.Minutes + "分" + ts.Seconds + "秒";
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>0/1</color> （每天0点重置）";
    //        getTransformByPath("cue/reword").GetComponent<Text>().text = "副本奖励： <color=#ff9900>大量材料</color>";
    //        RefreshLeftTimes();
    //    }

    //    void RefreshLeftTimes()
    //    {
    //        Variant data = SvrLevelConfig.instacne.get_level_data(103);
    //        int max_times = data["daily_cnt"];
    //        int use_times = 0;
    //        if (MapModel.getInstance().dFbDta.ContainsKey(103))
    //        {
    //            use_times = Mathf.Min(MapModel.getInstance().dFbDta[103].cycleCount, max_times);
    //        }
    //        getTransformByPath("cue/limit").GetComponent<Text>().text = "副本限制： <color=#00ff00>" + use_times + "/" + max_times + "</color> （每天0点重置）";
    //        if ((max_times - use_times) <= 0)
    //        {
    //            enterbtn.interactable = false;
    //        }
    //        else enterbtn.interactable = true;
    //    }
    //    public override void onClose()
    //    {
    //    }
    //}
    #endregion
    //磨炼之地
    class a3_active_mlzd : a3BaseActive
    {
        RectTransform parView;
        Transform lvlView;
        RectTransform top;
        RectTransform down;
        GameObject isthis;
        int diff_lvl = 0;
        int alllvl = 0;
        private GameObject tip;
        public static a3_active_mlzd instans;

        private float parView_pos_x;
        public a3_active_mlzd(Window win, string pathStr)
            : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        private ScrollRect rect;
        private RectTransform rect_Content;
        private float contentCellSize;
        private GameObject isthisClon = null;

        private int minlvl = 1;
        private int maxlvl = 1;
        public int CountForOne = 10;//每阶层有10层


        Text Time_text;
        GameObject sweepCon;
        GameObject tosure;


        GameObject eff;
        GameObject tach;
        public override void init()
        {
            parView = transform.FindChild("scrollview/con").GetComponent<RectTransform>();
            parView.pivot = new Vector2(0, 1);
            parView_pos_x = parView.anchoredPosition.x;
            // transform.FindChild("scrollview").gameObject.AddComponent<Ex_List>();
            rect = transform.FindChild("scrollview").GetComponent<ScrollRect>();
            lvlView = parView.FindChild("lvlCon");
            top = parView.FindChild("top").GetComponent<RectTransform>();
            down = parView.FindChild("down").GetComponent<RectTransform>();
            isthis = transform.FindChild("scrollview/this").gameObject;//指向将要打哪一层
            tip = transform.parent.parent.FindChild("tip").gameObject;//他是提示文本
            rect.onValueChanged.AddListener(delegate { onChange(); });
            EventTriggerListener.Get(rect.gameObject).onDragEnd = Dragend;
            EventTriggerListener.Get(rect.gameObject).onDragIn = BeginDrag;

            sweepCon = this.transform.FindChild("onekeycon").gameObject;
            Time_text = this.transform.FindChild("time/time_text").GetComponent<Text>();
            tosure = this.transform.FindChild("tosure").gameObject;
            rect_Content = parView.GetComponent<RectTransform>();

            eff = this.transform.FindChild("effect/con").gameObject;
            tach = this.transform.parent.parent.FindChild("tach").gameObject;
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("enter"));
            enterbtn.onClick = (GameObject go) =>
            {
                if (a3_active.MwlrIsDoing)
                {
                   
                    ArrayList art = new ArrayList();
                    art.Add(fb_type.mlzd);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE,art);
                }
                else
                {
                    EnterOnClick();
                }
            };

            contentCellSize = lvlView.GetComponent<GridLayoutGroup>().cellSize.y + lvlView.GetComponent<GridLayoutGroup>().spacing.y;
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            maxlvl = data["all"][0]["level"];

            new BaseButton(this.transform.FindChild("onekey")).onClick = (GameObject go) => {
                if (A3_ActiveModel.getInstance().maxlvl <= 0) {
                    flytxt.instance.fly(ContMgr.getCont("a3_active_5"));
                }
                else
                {
                    if (A3_ActiveModel.getInstance().maxlvl <= A3_ActiveModel.getInstance().nowlvl)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_active_6"));
                    }
                    else
                    {
                        sweepCon.SetActive(true);
                        setSweep();
                    }
                }
            };
            new BaseButton(tosure.transform.FindChild("yes")).onClick = (GameObject go) => { A3_ActiveProxy.getInstance().sendget(); };
            new BaseButton(tosure.transform.FindChild("no")).onClick = (GameObject go) => { tosure.SetActive(false); };

            new BaseButton(sweepCon.transform.FindChild("close")).onClick = (GameObject go) => { sweepCon.SetActive(false); };
            new BaseButton(sweepCon.transform.FindChild("moneykey")).onClick = (GameObject go) => {
                A3_ActiveProxy.getInstance().sendsweep(2);
            };
            new BaseButton(sweepCon.transform.FindChild("vipkey")).onClick = (GameObject go) => { A3_ActiveProxy.getInstance().sendsweep(3); };

            new BaseButton(this.transform.FindChild("get")).onClick = (GameObject go) => {
                if (A3_ActiveModel .getInstance ().nowlvl <= 0) {
                    flytxt.instance.fly(ContMgr .getCont ("a3_active_1"));
                }
                else 
                    tosure.SetActive(true);
            };

            inText();
        }


        void setSweep() {
            sweepCon.transform.FindChild("count").GetComponent<Text>().text = A3_ActiveModel.getInstance().maxlvl.ToString();
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            int lvl_loes = data["sweep"][0]["differ_floor"];

           
            sweepCon.transform.FindChild("vipkey/num").GetComponent<Text>().text = ((A3_ActiveModel.getInstance().maxlvl - A3_ActiveModel.getInstance().nowlvl ) * data["sweep"][0]["floor_yb"]).ToString();
            if (A3_ActiveModel.getInstance().maxlvl - lvl_loes <= 0) {
                sweepCon.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_2");
                sweepCon.transform.FindChild("moneykey").GetComponent<Button>().interactable = false;
                sweepCon.transform.FindChild("moneykey/num").GetComponent<Text>().text = "0";
            }
            else {
                sweepCon.transform.FindChild("moneykey/num").GetComponent<Text>().text = ((A3_ActiveModel.getInstance().maxlvl - A3_ActiveModel.getInstance().nowlvl - lvl_loes) * data["sweep"][0]["floor_money"]).ToString();
                sweepCon.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_3", new List<string> { (A3_ActiveModel.getInstance().maxlvl - lvl_loes).ToString() });
                sweepCon.transform.FindChild("moneykey").GetComponent<Button>().interactable = true;
            }
            sweepCon.transform.FindChild("text2").GetComponent<Text>().text = ContMgr.getCont("a3_active_4", new List<string> { A3_ActiveModel.getInstance().maxlvl.ToString() });
        }


        void inText()
        {
            this.transform.FindChild("info/context").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_6");
            this.transform.FindChild("enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_7");
            this.transform.FindChild("title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_8");
            this.transform .FindChild ("Text_lvl").GetComponent <Text>().text = ContMgr.getCont("uilayer_a3_active_36");
            this.transform.FindChild("get/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_37");//领奖
            this.transform.FindChild("onekey/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_38");//扫荡
            this.transform.FindChild("onekeycon/txe1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_39");//最高通关层数：
            this.transform.FindChild("onekeycon/moneykey/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_40");//扫荡
            this.transform.FindChild("onekeycon/vipkey/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_41");//扫荡
            this.transform.FindChild("onekeycon/tet2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_42");//(VIP5解锁)
            this.transform.FindChild("tosure/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_43");//确定
            this.transform.FindChild("tosure/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_44");//取消
            this.transform.FindChild("tosure/tet").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_45");
            this.transform.FindChild("cue/null").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_46");
        }
        public void EnterOnClick()
        {
            if (diff_lvl <= A3_ActiveModel.getInstance().nowlvl + 1)
            {

                debug.Log("Enter");
                Variant sendData = new Variant();
                sendData["mapid"] = 3338;
                sendData["npcid"] = 0;
                sendData["ltpid"] = 104;
                sendData["diff_lvl"] = diff_lvl;
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_active_mlzd"));
            }
            //setWin();
            //setView();

            contentCellSize = lvlView.GetComponent<GridLayoutGroup>().cellSize.y + lvlView.GetComponent<GridLayoutGroup>().spacing.y;
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            maxlvl = data["all"][0]["level"];
        }




        float time = 2f;

        public void _update() {
            if (eff.activeSelf ) {
                time -= Time.deltaTime;
                if (time <= 0) {
                    oneffOver();
                    time = 2f;
                }
            }

        }




        float startV = -1;
        float endV = -1;
        bool Load = false;

        void onChange()
        {
            if (rect.verticalNormalizedPosition > 0.8)
            {
                toup = true;
                todown = false;
            }
            else if (rect.verticalNormalizedPosition < 0.2)
            {
                todown = true;
                toup = false;
            }
            else
            {
                todown = false;
                toup = false;
            }
        }

        bool toup = false;
        bool todown = false;
        void Dragend(GameObject go, Vector2 delta)
        { 
            Load = true;
            endV = rect.verticalNormalizedPosition;
            if (endV - startV > 0 && toup)
            {
                addAndRec_Obj(true);
            }
            else if (endV - startV < 0 && todown)
            {
                addAndRec_Obj(false);
            }
        }

        void BeginDrag(GameObject go)
        {
            startV = rect.verticalNormalizedPosition;
        }
        public override void onShowed()
        {
            instans = this;
            tasklvl = A3_ActiveModel.getInstance().nowlvl + 1;
            oneffOver();
            time = 2f;
            sweepCon.SetActive(false);
            tosure.SetActive(false);
            if (tasklvl > maxlvl)
            {
                tasklvl = maxlvl;
            }
            tip.SetActive(false);
            getTransformByPath("cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_active_fblock");
            RefreshLeftTimes();
            //onshowlvl(A3_ActiveModel.getInstance().nowlvl+1);
            //setlock();
            SetView();
        }

        public void refreview()
        {
            tasklvl = A3_ActiveModel.getInstance().nowlvl + 1;
            sweepCon.SetActive(false);
            tosure.SetActive(false);
            if (tasklvl > maxlvl)
            {
                tasklvl = maxlvl;
            }
            RefreshLeftTimes();
            SetView();
        }

        public void onGetFit(int lvl = 0) {
            refreview();
            if (lvl == 0) return;
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            for (int i = 0; i < data["info"].Count; i++)
            {
                string[] args = ((string)data["info"][i]["lvl"]).Split(',');
                int minlv = int.Parse(args[0]);
                int maxlv = int.Parse(args[1]);
                if (lvl >= minlv && lvl <= maxlv)
                {
                    a3_ItemData item1 = a3_BagModel.getInstance().getItemDataById(data["info"][i]["item"]);
                    flytxt.instance.fly(ContMgr .getCont ("BagProxy_geteitem") + item1.item_name);
                    return;
                }
            }
        }

        public void onSweep()
        {
            parView.gameObject.SetActive(false);
            eff.SetActive(true);
            refreview();
        }
        void oneffOver() {
            parView.gameObject.SetActive(true);
            tach.SetActive(false);
            eff.SetActive(false);
        }

        public virtual void onClose()
        {
            instans = null;
        }       

       public  void refreTime()
        {
            if (A3_ActiveModel.getInstance().count_mlzd >= max_times)
            {
                Time_text.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Time_text.transform.parent.gameObject.SetActive(true);
                Time_text.text = GetTm((int)(3600 - (muNetCleint.instance.CurServerTimeStamp - A3_ActiveModel.getInstance().Time)));            
            }
        }
        string GetTm(int tm)
        {
            string m = (tm / 60 % 60).ToString().Length > 1 ? (tm / 60 % 60).ToString() : "0" + (tm / 60 % 60);
            string s = (tm % 60).ToString().Length > 1 ? (tm % 60).ToString() : "0" + (tm % 60);
            string t = m + ":" + s;
            return t;
        }

        int max_times;
        public void RefreshLeftTimes()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            max_times = data["daily_cnt"];



            this.transform.FindChild("lvl").GetComponent<Text>().text = A3_ActiveModel.getInstance().nowlvl.ToString();
            setInfo_One(A3_ActiveModel.getInstance().nowlvl);
            string str = (max_times - A3_ActiveModel.getInstance().count_mlzd) + "/" + max_times;
            if (max_times - A3_ActiveModel.getInstance().count_mlzd <= 0)
            {
                str = "<color=#f90e0e>" + str + "</color>";
                enterbtn.interactable = false;
                this.transform.FindChild("onekey").GetComponent<Button>().interactable = false;
                this.transform.FindChild("get").GetComponent<Button>().interactable = false;
            }
            else
            {
                str = "<color=#00FF00>" + str + "</color>";
                enterbtn.interactable = true;
                if (A3_ActiveModel.getInstance().sweep_type == 0)
                {
                    this.transform.FindChild("onekey").GetComponent<Button>().interactable = true;
                }
                else
                {
                    this.transform.FindChild("onekey").GetComponent<Button>().interactable = false;
                }
                this.transform.FindChild("get").GetComponent<Button>().interactable = true;
            }

            this.transform.FindChild("get/count").GetComponent<Text>().text = str; 

            //Variant data = SvrLevelConfig.instacne.get_level_data(104);
            //max_times = data["daily_cnt"];
            //int use_times = A3_ActiveModel.getInstance().count_mlzd;
            ////if (MapModel.getInstance().dFbDta.ContainsKey(104))
            ////{
            ////    use_times = Mathf.Min(MapModel.getInstance().dFbDta[104].cycleCount, max_times);
            ////}
            //getTransformByPath("cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_active_fblock1") + use_times + "/" + max_times + ContMgr.getCont("a3_active_fblock2");
            //if (use_times <= 0)
            //{
            //    enterbtn.interactable = false;
            //}
            //else enterbtn.interactable = true;
        }

        Dictionary<int, GameObject> lvlobj = new Dictionary<int, GameObject>();
        int tasklvl = 0;//初始化层级
        int lastlvl = 0;//用于记录向上加载的区间id最小值
        int Pastlvl = 0;//用于记录向下加载的区间id最大值

        #region 初始塔层
        void SetView()
        {

            lvlobj.Clear();
            for (int i = 0; i < lvlView.childCount; i++)
            {
                GameObject.Destroy(lvlView.GetChild(i).gameObject);
            }

            GameObject item = transform.FindChild("scrollview/lvlitem").gameObject;
            int pos = GetPos(tasklvl);
            int nowPosMax = pos * CountForOne;
            int nowPosMin = nowPosMax - CountForOne + 1;
            lastlvl = nowPosMax + 1;
            Pastlvl = nowPosMin - 1;
            int count = 0;
            for (int lvl = nowPosMin; lvl <= nowPosMax; lvl++)
            {
                GameObject clon = GameObject.Instantiate(item);
                count++;
                clon.SetActive(true);
                clon.transform.SetParent(lvlView.transform, false);
                clon.name = (lvl).ToString();
                clon.transform.SetAsFirstSibling();
                clon.transform.FindChild("namebg/name").GetComponent<Text>().text = ContMgr.getCont("di") + lvl + ContMgr.getCont("ceng");
                setFriend(clon, lvl);
                if (lvl == tasklvl)
                {
                    locklvl(clon, lvl);
                    thisonelvPos = count;
                }
                lvlobj[lvl] = clon;
                setlockAndClick(clon, lvl);
            }
            setSize();
            setNowPos();
        }
        void setSize()
        {
            int pos = GetPos(tasklvl);
            float item_y = lvlView.GetComponent<GridLayoutGroup>().cellSize.y;
            float item_spacing_y = lvlView.GetComponent<GridLayoutGroup>().spacing.y;
            float lvlCon_y = lvlobj.Count * item_y + (lvlView.childCount - 1) * item_spacing_y;
            float con_y = 0;
            if (pos == GetPos(minlvl))
            {
                top.gameObject.SetActive(false);
                down.gameObject.SetActive(true);
                con_y = (down.offsetMin.y - (down.offsetMax.y * -1)) + lvlCon_y;
                lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, 0);
            }
            else if (pos == GetPos(maxlvl))
            {
                top.gameObject.SetActive(true);
                down.gameObject.SetActive(false);
                con_y = ((top.offsetMin.y * -1) - top.offsetMax.y) + lvlCon_y;
                lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, -((top.offsetMin.y * -1) - top.offsetMax.y));
            }
            else
            {
                top.gameObject.SetActive(false);
                down.gameObject.SetActive(false);
                con_y = lvlCon_y;
                lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, 0);
            }
            Vector2 newSize = new Vector2(parView.sizeDelta.x, con_y);
            parView.sizeDelta = newSize;
        }

        #endregion


        int thisonelvPos = 0;
        void setNowPos()
        {
            float lvlView_y = 0;
            int pos = GetPos(tasklvl);
            if (pos == GetPos(minlvl))
            {
                if (thisonelvPos >= 1 && thisonelvPos <= 4)
                {
                    //lvlView_y = 653.25f; 
                    parView.pivot = Vector2.zero;
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 0);

                }
                else
                {
                    //lvlView_y = 262.75f; 
                    parView.pivot = new Vector2(0, 1);
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 1);
                }
            }
            else if (pos == GetPos(maxlvl))
            {
                if (thisonelvPos >= 1 && thisonelvPos <= 5)
                {
                    // lvlView_y = 730.75f;
                    parView.pivot = Vector2.zero;
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 0);
                }
                else
                {
                    //lvlView_y = 262.75f; 
                    parView.pivot = new Vector2(0, 1);
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 1);
                }
            }
            else
            {
                if (thisonelvPos >= 1 && thisonelvPos <= 4)
                {
                    //lvlView_y = 565.25f;
                    parView.pivot = Vector2.zero;
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 0);
                }
                else
                {
                    // lvlView_y = 262.75f;
                    parView.pivot = new Vector2(0, 1);
                    parView.anchorMax = parView.anchorMin = new Vector2(0, 1);
                }
            }
            parView.anchoredPosition = new Vector2(parView_pos_x, 0);
        }


        void addAndRec_Obj(bool up)
        {
            if (up && Load)
            {
                //加
                int pos = GetPos(lastlvl);
                if (pos > GetPos(maxlvl))
                { Load = false; return; }
                GameObject item = transform.FindChild("scrollview/lvlitem").gameObject;//磨练之地的层数
                int nowPosMax = pos * CountForOne;
                int nowPosMin = nowPosMax - CountForOne + 1;
                lastlvl = nowPosMax + 1;
                for (int lvl = nowPosMin; lvl <= nowPosMax; lvl++)
                {
                    GameObject clon = GameObject.Instantiate(item);
                    clon.SetActive(true);
                    clon.transform.SetParent(lvlView.transform, false);
                    clon.name = (lvl).ToString();
                    clon.transform.SetAsFirstSibling();
                    clon.transform.FindChild("namebg/name").GetComponent<Text>().text =ContMgr.getCont("di") + lvl + ContMgr.getCont("ceng");//第多少层
                    setFriend(clon, lvl);
                    if (lvl == diff_lvl)
                    {
                        if (isthisClon != null)
                        {
                            GameObject.Destroy(isthisClon);
                        }
                        isthisClon = GameObject.Instantiate(isthis);
                        isthisClon.transform.localPosition = Vector3.zero;
                        isthisClon.SetActive(true);
                        if (clon.transform.FindChild("fplayer").gameObject.activeSelf)
                        {
                            clon.transform.FindChild("fplayer").gameObject.SetActive(false);
                            friendinfo = clon.transform.FindChild("fplayer").gameObject;
                        }
                        isthisClon.transform.SetParent(clon.transform, false);
                    }
                    lvlobj[lvl] = clon;
                    setlockAndClick(clon, lvl);
                }
                Vector2 xy;
                xy.x = rect_Content.offsetMax.x;
                if (pos == GetPos(maxlvl))
                {
                    top.gameObject.SetActive(true);
                    xy.y = rect_Content.offsetMax.y + (contentCellSize * CountForOne) + ((top.offsetMin.y * -1) - top.offsetMax.y);
                    lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, -((top.offsetMin.y * -1) - top.offsetMax.y));
                }
                else
                {
                    top.gameObject.SetActive(false);
                    xy.y = rect_Content.offsetMax.y + (contentCellSize * CountForOne);
                    lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, 0);
                }
                rect_Content.offsetMax = xy;

                //减
                int recpos = pos - 2;
                if (recpos < GetPos(minlvl))
                { Load = false; return; }
                int recid_max = recpos * CountForOne;
                int recid_min = recid_max - CountForOne + 1;
                int recCount = 0;
                Pastlvl = recid_max;
                for (int lvl = recid_min; lvl <= recid_max; lvl++)
                {
                    if (lvlobj.ContainsKey(lvl))
                    {
                        GameObject.Destroy(lvlobj[lvl]);
                        lvlobj.Remove(lvl);
                        recCount++;
                    }
                }
                if (recCount > 0)
                {
                    Vector2 New_xy;
                    New_xy.x = rect_Content.offsetMin.x;
                    if (recpos == GetPos(minlvl))
                    {
                        down.gameObject.SetActive(false);
                        New_xy.y = rect_Content.offsetMin.y + (contentCellSize * recCount) + (down.offsetMin.y - (down.offsetMax.y * -1));
                    }
                    else
                    {
                        New_xy.y = rect_Content.offsetMin.y + (contentCellSize * recCount);
                    }
                    rect_Content.offsetMin = New_xy;
                }
            }
            else
            {
                //加
                int pos = GetPos(Pastlvl);
                if (pos < GetPos(minlvl)) { Load = false; return; }
                GameObject item = transform.FindChild("scrollview/lvlitem").gameObject;
                int nowPosMax = pos * CountForOne;
                int nowPosMin = nowPosMax - CountForOne + 1;
                Pastlvl = nowPosMin - 1;
                for (int lvl = nowPosMax; lvl >= nowPosMin; lvl--)
                {
                    GameObject clon = GameObject.Instantiate(item);
                    clon.SetActive(true);
                    clon.transform.SetParent(lvlView.transform, false);
                    clon.name = (lvl).ToString();
                    clon.transform.SetAsLastSibling();
                    clon.transform.FindChild("namebg/name").GetComponent<Text>().text = ContMgr.getCont("di") + lvl + ContMgr.getCont("ceng");
                    lvlobj[lvl] = clon;
                    setFriend(clon, lvl);
                    if (lvl == diff_lvl)
                    {
                        if (isthisClon != null)
                        {
                            GameObject.Destroy(isthisClon);
                        }
                        isthisClon = GameObject.Instantiate(isthis);
                        isthisClon.transform.localPosition = Vector3.zero;
                        isthisClon.SetActive(true);
                        if (clon.transform.FindChild("fplayer").gameObject.activeSelf)
                        {
                            clon.transform.FindChild("fplayer").gameObject.SetActive(false);
                            friendinfo = clon.transform.FindChild("fplayer").gameObject;
                        }
                        isthisClon.transform.SetParent(clon.transform, false);
                    }
                    setlockAndClick(clon, lvl);
                }
                Vector2 xy;
                xy.x = rect_Content.offsetMin.x;
                if (pos == GetPos(minlvl))
                {
                    down.gameObject.SetActive(true);
                    xy.y = rect_Content.offsetMin.y - (contentCellSize * CountForOne) - (down.offsetMin.y - (down.offsetMax.y * -1));
                }
                else
                {
                    down.gameObject.SetActive(false);
                    xy.y = rect_Content.offsetMin.y - (contentCellSize * CountForOne);
                }
                rect_Content.offsetMin = xy;

                //减
                int recpos = pos + 2;
                if (recpos > GetPos(maxlvl))
                { Load = false; return; }
                int recid_max = recpos * CountForOne;
                int recid_min = recid_max - CountForOne + 1;
                int recCount = 0;
                lastlvl = recid_max;
                for (int lvl = recid_min; lvl <= recid_max; lvl++)
                {
                    if (lvlobj.ContainsKey(lvl))
                    {
                        GameObject.Destroy(lvlobj[lvl]);
                        lvlobj.Remove(lvl);
                        recCount++;
                    }
                }
                if (recCount > 0)
                {
                    Vector2 New_xy;
                    New_xy.x = rect_Content.offsetMax.x;
                    if (recpos == GetPos(maxlvl))
                    {
                        top.gameObject.SetActive(false);
                        New_xy.y = rect_Content.offsetMax.y - (contentCellSize * recCount) - ((top.offsetMin.y * -1) - top.offsetMax.y);
                        lvlView.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlView.GetComponent<RectTransform>().anchoredPosition.x, 0);
                    }
                    else
                    {
                        New_xy.y = rect_Content.offsetMax.y - (contentCellSize * recCount);
                    }
                    rect_Content.offsetMax = New_xy;
                }
            }
            Load = false;
        }

        void addClick(GameObject go, int lvl)
        {
            new BaseButton(go.transform).onClick = (GameObject oo) =>
            {
                locklvl(oo, lvl);
            };
        }


        GameObject friendinfo = null;
        void locklvl(GameObject go, int lvl)
        {
            diff_lvl = lvl;
            if (isthisClon != null)
            {
                GameObject.Destroy(isthisClon);
            }
            isthisClon = GameObject.Instantiate(isthis);
            isthisClon.transform.localPosition = Vector3.zero;
            isthisClon.SetActive(true);
            isthisClon.transform.SetParent(go.transform, false);
            //setInfo_One(lvl);
            if (friendinfo != null && friendinfo.activeSelf == false)
            {
                friendinfo.SetActive(true);
            }
            if (go.transform.FindChild("fplayer").gameObject.activeSelf)
            {
                go.transform.FindChild("fplayer").gameObject.SetActive(false);
                friendinfo = go.transform.FindChild("fplayer").gameObject;
            }


        }

        void setlockAndClick(GameObject go, int lvl)
        {
            if (lvl <= tasklvl)
            {
                addClick(go, lvl);
                go.transform.FindChild("namebg").gameObject.SetActive(true);
                go.transform.FindChild("lock").gameObject.SetActive(false);
            }
            else
            {
                go.transform.FindChild("namebg").gameObject.SetActive(false);
                go.transform.FindChild("lock").gameObject.SetActive(true);
            }
        }

        void setlock()
        {
            foreach (int i in lvlobj.Keys)
            {
                if (i <= A3_ActiveModel.getInstance().nowlvl + 1)
                {
                    lvlobj[i].transform.FindChild("namebg").gameObject.SetActive(true);
                    lvlobj[i].transform.FindChild("lock").gameObject.SetActive(false);
                }
                else
                {
                    lvlobj[i].transform.FindChild("namebg").gameObject.SetActive(false);
                    lvlobj[i].transform.FindChild("lock").gameObject.SetActive(true);
                }
            }
        }
        void setInfo_One(int lvl)
        {

            if (lvl <= 0) {
                transform.FindChild("cue/icon").gameObject.SetActive(false);
                transform.FindChild("cue/null").gameObject.SetActive(true);
                return;
            }
            transform.FindChild("cue/null").gameObject.SetActive(false);
            transform.FindChild("cue/icon").gameObject.SetActive(true);
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            for (int i = 0; i < data["info"].Count; i++)
            {
                string[] args = ((string)data["info"][i]["lvl"]).Split(',');
                int minlv = int.Parse(args[0]);
                int maxlv = int.Parse(args[1]);
                if (lvl >= minlv && lvl <= maxlv)
                {
                    string dec = data["info"][i]["des"];
                    transform.FindChild("info/context").GetComponent<Text>().text = dec.Replace("{0}", lvl.ToString());
                    transform.FindChild("cue/info").GetComponent<Text>().text = data["info"][i]["des2"];
                    a3_ItemData item1 = a3_BagModel.getInstance().getItemDataById(data["info"][i]["item"]);
                    for (int m = 0; m < transform.FindChild("cue/icon").childCount; m++)
                    {
                        GameObject.Destroy(transform.FindChild("cue/icon").GetChild(m).gameObject);
                    }
                    IconImageMgr.getInstance().createA3ItemIcon(item1).transform.SetParent(transform.FindChild("cue/icon"), false);
                    uint id = uint.Parse(data["info"][i]["item"]);
                    new BaseButton(transform.FindChild("cue/icon")).onClick = (GameObject go) =>
                    {
                        tip.SetActive(true);
                        a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
                        tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
                        tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);

                        tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
                        if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
                        else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
                        tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
                        tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

                        new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
                    };

                    return;
                }
            }
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        void setFriend(GameObject go, int lvl)
        {
            uint playerid = 0;
            foreach (itemFriendData item in FriendProxy.getInstance().FriendDataList.Values)
            {
                if (item.mlzd_lv == lvl)
                {
                    if (playerid > 0)
                    {
                        if (item.zhuan > FriendProxy.getInstance().FriendDataList[playerid].zhuan)
                        {
                            playerid = item.cid;
                        }
                        else if (item.zhuan == FriendProxy.getInstance().FriendDataList[playerid].zhuan)
                        {
                            if (item.lvl > FriendProxy.getInstance().FriendDataList[playerid].lvl)
                            {        
                                playerid = item.cid;
                            }
                        }
                    }
                    else
                        playerid = item.cid;
                }
            }

            if (playerid > 0)
            {
                go.transform.FindChild("fplayer").gameObject.SetActive(true);
                go.transform.FindChild("fplayer").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_npctask_" + FriendProxy.getInstance().FriendDataList[playerid].carr);
                go.transform.FindChild("fplayer/name").GetComponent<Text>().text = FriendProxy.getInstance().FriendDataList[playerid].name;
            }
            else
            {
                go.transform.FindChild("fplayer").gameObject.SetActive(false);
            }


        }


        void setInfo(int lvl)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(104);
            transform.FindChild("info/context").GetComponent<Text>().text = data["info"][lvl - 1]["des"];

            transform.FindChild("cue/info").GetComponent<Text>().text = data["info"][lvl - 1]["des2"];

            a3_ItemData item1 = a3_BagModel.getInstance().getItemDataById(data["info"][lvl - 1]["item"]);
            for (int m = 0; m < transform.FindChild("cue/icon").childCount; m++)
            {
                GameObject.Destroy(transform.FindChild("cue/icon").GetChild(m).gameObject);
            }
            IconImageMgr.getInstance().createA3ItemIcon(item1).transform.SetParent(transform.FindChild("cue/icon"), false);

            uint id = uint.Parse(data["info"][lvl - 1]["item"]);
            new BaseButton(transform.FindChild("cue/icon")).onClick = (GameObject go) =>
              {
                  tip.SetActive(true);
                  a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
                  tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
                  tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
                  tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
                  if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
                  else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
                  tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
                  tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

                  new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
              };

        }

        void onshowlvl(int lvl)
        {

            if (lvl >= lvlobj.Count)
            {
                lvl = lvlobj.Count;
            }
            diff_lvl = lvl;
            isthis.gameObject.SetActive(true);
            isthis.transform.SetParent(lvlobj[lvl].transform, false);
            float lvlView_y = 0;
            float item_y = lvlView.GetComponent<GridLayoutGroup>().cellSize.y;
            switch (lvl)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    lvlView_y = 464;// ((top.offsetMin.y * -1) - top.offsetMax.y) + item_y * (alllvl - 5);
                    break;
                case 6:
                case 7:
                case 8:
                    lvlView_y = 18;
                    break;
            }
            parView.GetComponent<RectTransform>().anchoredPosition = new Vector2(parView.GetComponent<RectTransform>().anchoredPosition.x, lvlView_y);
            setInfo(lvl);
        }
        //获得所属阶层
        int GetPos(int lvl)
        {
            int Float = 0;
            if (lvl % CountForOne > 0)
                Float = (lvl / CountForOne) + 1;
            else
                Float = lvl / CountForOne;
            return Float;
        }




    }


    //召唤兽乐园
    class a3_active_zhsly : a3BaseActive
    {
        public a3_active_zhsly(Window win, string pathStr)
            : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        public static a3_active_zhsly instance;
        Dictionary<uint, uint> diff_lvl_zhaun = new Dictionary<uint, uint>();
        Variant data = SvrLevelConfig.instacne.get_level_data(105);
        uint diff_lvl;



        public override void init()
        {
            //for (int i = 1; i < 5; i++)
            //{
            //    if (!diff_lvl_zhaun.ContainsKey((uint)i))
            //        diff_lvl_zhaun.Add((uint)i, data["diff_lvl"][i]["zhuan"]);
            //}
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("enter"));
            enterbtn.onClick = (GameObject go) =>
            {
                if(a3_active.MwlrIsDoing)
                {
                   
                    ArrayList lst = new ArrayList();
                    lst.Add(fb_type.slmj);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE,lst);
                }
                else
                {
                    EnterOnClick();
                }
            };
            inText();

            new BaseButton(getTransformByPath("addnum")).onClick = (GameObject go) =>
              {

                  
              };
        }

        void inText()
        {
            this.transform.FindChild("title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_9");
            this.transform.FindChild("info/context").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_10");
            this.transform.FindChild("cue/reword/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_11");
            this.transform.FindChild("cue/reword/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_12");
            this.transform.FindChild("enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_13");

        }

        public void EnterOnClick()
        {

            if (true)
            {
                Variant sendData = new Variant();
                sendData["mapid"] = 3340;
                sendData["npcid"] = 0;
                sendData["ltpid"] = 105;
                sendData["diff_lvl"] = PlayerModel.getInstance().up_lvl;
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }

        }



        int mapid = 3340;
        public override void onShowed()
        {
            instance = this;
            uint playerUP_lvl = PlayerModel.getInstance().up_lvl;
            //foreach (var item in diff_lvl_zhaun)
            //{
            //    if (item.Value == playerUP_lvl)
            //    {
            //        diff_lvl = item.Key;
            //    }
            //}
            //if (playerUP_lvl > diff_lvl_zhaun[4])
            //{
            //    diff_lvl = 4;
            //}
            //Variant data = SvrLevelConfig.instacne.get_level_data(105);
            //for (int i = 1; i <= data["diff_lvl"].Count-1; i++)
            //{
            //    int zhuan = data["diff_lvl"][i]["zhuan"];
            //    if (PlayerModel.getInstance().up_lvl == zhuan)
            //    {
            //        mapid = data["diff_lvl"][i]["map"][0]["id"];
            //        Debug.LogError("PPPP" + mapid);
            //        break;
            //    }
            //}

            // int tm = data["tm"];
            // TimeSpan ts = new TimeSpan(0, tm, 0);
            getTransformByPath("cue/time").gameObject.SetActive(false);
            //getTransformByPath("cue/time").GetComponent<Text>().text = "副本时间： " + ts.Hours + "时" + ts.Minutes + "分" + ts.Seconds + "秒";
            if (MapModel.getInstance().dFbDta.ContainsKey(105))
            {
               // var d = MapModel.getInstance().dFbDta[105];
               // TimeSpan tss = new TimeSpan(0, 0, d.limit_tm);
                //getTransformByPath("cue/limit").GetComponent<Text>().text = "剩余时间： " + tss.Hours + "时" + tss.Minutes + "分" + tss.Seconds + "秒";
               // getTransformByPath("cue/limit").GetComponent<Text>().text =ContMgr.getCont("a3_active_time",new List<string>{ tss.Hours.ToString(), tss.Minutes.ToString(), tss.Seconds.ToString() });
                //if (d.limit_tm <= 0) enterbtn.interactable = false;
                //if (d.limit_tm <= 0) enterbtn.interactable = false;
               // else enterbtn.interactable = true;
            }
            else
            {
                //getTransformByPath("cue/limit").GetComponent<Text>().text = "剩余时间： " + 1 + "时" + 0 + "分" + 0 + "秒";
               // getTransformByPath("cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_active_time", new List<string> { 1.ToString(), 0.ToString(), 0.ToString() });
               // enterbtn.interactable = true;
            }

            getTransformByPath("cue/reword").GetComponent<Text>().text = ContMgr.getCont("a3_active_fbjiangli");
            RefreshLeftTimes();

            setCount();
        }

      public    void setCount() {
            //vip可购买次数
            int vip_count = A3_VipModel.getInstance().vip_exchange_num(23);
            //vip已购买次数
            int vip_buycount = LevelProxy.getInstance().vip_buycount;
            //vip可购买剩余次数
            int vip_havecount = vip_count- vip_buycount;

            int curnum = LevelProxy.getInstance().cishu;
            int curkillnum = LevelProxy.getInstance().count;
            int Zkillcount = SvrLevelConfig.instacne.get_level_data(105)["lvl_target"][0]["single_kill_amounts"];
            int Zcishu = SvrLevelConfig.instacne.get_level_data(105)["lvl_target"][0]["daily_times"]+ vip_buycount;


            this.transform.FindChild("cishu").GetComponent<Text>().text = ContMgr.getCont("fb_info_11", new List<string> { curnum.ToString(), Zcishu.ToString() });
            this.transform.FindChild("jindu").GetComponent<Text>().text = ContMgr.getCont("fb_info_2", new List<string> { curkillnum.ToString(), Zkillcount.ToString() });
            this.transform.FindChild("vip_count").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_havecount.ToString() });
            new BaseButton(this.transform.FindChild("addnum")).onClick = (GameObject go) =>
              {

                  if(vip_havecount<=0)
                  {
                      InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP);
                      InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                  }
                  else
                  {
                      int price= SvrLevelConfig.instacne.get_level_data(105)["lvl_target"][0]["vip_cost"];
                      MsgBoxMgr.getInstance().showConfirm(price + ContMgr.getCont("zuanshi_ci"), () =>
                      {
                          LevelProxy.getInstance().getAwd_zhs(4);
                      });

                     

                      //购买
                  }
              };
        }

        void RefreshLeftTimes()
        {
            //Variant data = SvrLevelConfig.instacne.get_level_data(105);
            //int max_times = data["daily_cnt"];
            //int use_times = 0;
            //if (MapModel.getInstance().dFbDta.ContainsKey(105))
            //{
            //    use_times = Mathf.Min(MapModel.getInstance().dFbDta[105].cycleCount, max_times);
            //}
            //if (MapModel.getInstance().dFbDta.ContainsKey(105))
            //{
            //    var d = MapModel.getInstance().dFbDta[105];
            //    TimeSpan tss = new TimeSpan(0, 0, d.limit_tm);
            //    //getTransformByPath("cue/limit").GetComponent<Text>().text = "剩余时间： " + tss.Hours + "时" + tss.Minutes + "分" + tss.Seconds + "秒";
            //    getTransformByPath("cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_active_time", new List<string> { tss.Hours.ToString(), tss.Minutes.ToString(), tss.Seconds.ToString() });
            //    if (d.limit_tm <= 0) enterbtn.interactable = false;
            //    else enterbtn.interactable = true;
            //}
            //else
            //{
            //    //getTransformByPath("cue/limit").GetComponent<Text>().text = "剩余时间： " + 1 + "时" + 0 + "分" + 0 + "秒";
            //    getTransformByPath("cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_active_time", new List<string> { 1.ToString(), 0.ToString(), 0.ToString() });
            //    enterbtn.interactable = true;
            //}
        }
        public override void onClose()
        {
        }
    }
    class a3_active_mwlr_kill
    {
        public Animator MHAnimator { get; set; }
        public Transform MHInfo { get; set; }
        public int Count { get; set; } //Should be initialed when players login in game or click the button 'search'        
        public Transform Bar_Mon;
        public List<Transform> Mon_Icons = new List<Transform>();
        public Text Text_DoubleTime;
        public static Variant initLoginData { get; set; }
        private static a3_active_mwlr_kill instance;
        public static a3_active_mwlr_kill Instance => instance = instance ?? new a3_active_mwlr_kill();
        public bool NewAction { get; set; }
        private a3_active_mwlr_kill() { Init(); }
        private bool? b_clear;
        private bool? b_reset;
        public bool isReset => b_reset.HasValue;
        private bool b_endPassed;
        public List<int> listKilled;
        public void Init() // On a3_expbar Init
        {
            instance = this;
            listKilled = new List<int>();
            NewAction = false;
            if (a3_expbar.instance) {
                MHInfo = a3_expbar.instance.transform.FindChild("mh_tips");
                MHAnimator = MHInfo.GetComponent<Animator>();
                Bar_Mon = a3_expbar.instance.transform.FindChild("mh_tips/a3_mhfloatBorder");
                Text_DoubleTime = Bar_Mon.FindChild("Text").GetComponent<Text>();
                for (int i = 0; i < Bar_Mon.childCount; i++)
                {
                    Transform huntIcon = Bar_Mon.FindChild("hunt_icon_" + i.ToString());
                    if (huntIcon != null)
                        Mon_Icons.Add(huntIcon);
                }
            }
            b_clear = !(b_reset = true);
        }



        public void Update() // Each Frame
        {
            if (Bar_Mon == null) {
                MHInfo = a3_expbar.instance.transform.FindChild("mh_tips");
                MHAnimator = MHInfo.GetComponent<Animator>();
                Bar_Mon = a3_expbar.instance.transform.FindChild("mh_tips/a3_mhfloatBorder");
                Text_DoubleTime = Bar_Mon.FindChild("Text").GetComponent<Text>();
            }
            if (A3_ActiveModel.getInstance().mwlr_map_id.Count == 0 && !Bar_Mon.gameObject.activeSelf)
                return;
            if (Text_DoubleTime != null && Text_DoubleTime.gameObject.activeSelf)
            {
                TimeSpan ts = new TimeSpan(0, 0, Mathf.Max(0, A3_ActiveModel.getInstance().mwlr_doubletime - muNetCleint.instance.CurServerTimeStamp));
                Text_DoubleTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
                if (ts.Hours == 0 && ts.Minutes == 0 && ts.Seconds == 0)
                    Text_DoubleTime.gameObject.SetActive(false);
            }

            if (b_clear.HasValue && b_reset.HasValue)
            {
                if (MHAnimator?.GetCurrentAnimatorStateInfo(0).IsName("mh_init") ?? false)
                {
                    if (b_clear.GetValueOrDefault())
                    {
                        Bar_Mon.gameObject.SetActive(false);
                        if (b_endPassed /* '狩猎结束'已经播放完毕 */ )
                        {
                            MHInfo.gameObject.SetActive(false);
                            b_clear = null;
                            b_endPassed = false;
                            return;
                        }
                    }
                    if (b_reset.GetValueOrDefault())
                    {
                        if (Count > 0)
                            Bar_Mon.gameObject.SetActive(true);
                        MHInfo.gameObject.SetActive(true);
                        b_reset = null;
                    }
                }
                if (MHAnimator?.GetCurrentAnimatorStateInfo(0).IsName("mh_end") ?? false)
                    b_endPassed = true;
            }
        }
        public void Reset() // When: Players Click the Button 'Search' or Players Login in
        {
            if (Bar_Mon == null)
            {
                MHInfo = a3_expbar.instance.transform.FindChild("mh_tips");
                MHAnimator = MHInfo.GetComponent<Animator>();
                Bar_Mon = a3_expbar.instance.transform.FindChild("mh_tips/a3_mhfloatBorder");
                Text_DoubleTime = Bar_Mon.FindChild("Text").GetComponent<Text>();
            }
            b_clear = !(b_reset = true);
            a3_expbar.instance.HoldTip();
            Text_DoubleTime.gameObject.SetActive(true);
            for (int i = 0; i < Count; i++)
            {
                Mon_Icons[i].GetChild(0 /* 击杀图标 */).gameObject.SetActive(false);
                if (A3_ActiveModel.getInstance().mwlr_map_id.Count > i)
                {
                    Variant mapInfo = SvrMapConfig.instance.mapConfs[(uint)A3_ActiveModel.getInstance().mwlr_map_id[i]];
                    if (mapInfo.ContainsKey("map_name"))
                        Mon_Icons[i].GetChild(1 /* 怪物位置 */).GetComponent<Text>().text = mapInfo["map_name"];
                    else
                        Mon_Icons[i].GetChild(1).GetComponent<Text>().text = "";
                    Mon_Icons[i].gameObject.SetActive(true);
                }
            }
            if (Count == 0) A3_ActiveModel.getInstance().mwlr_mon_killed = 0;
            if (A3_ActiveModel.getInstance().mwlr_mon_killed > 0)
                Refresh();
        }
        public void Refresh() // When: Monster Die
        {
            for (int i = 0; i < A3_ActiveModel.getInstance().mwlr_mon_killed; i++)
            {
                //Mon_Icons[i].transform.GetChild(0).gameObject.SetActive(true);
                Mon_Icons[A3_ActiveModel.getInstance().listKilled[i]].transform.GetChild(0).gameObject.SetActive(true);                
            }
        }
        public void Clear() // When: Last Monster Die
        {
            if (Bar_Mon == null)
            {
                MHInfo = a3_expbar.instance.transform.FindChild("mh_tips");
                MHAnimator = MHInfo.GetComponent<Animator>();
                Bar_Mon = a3_expbar.instance.transform.FindChild("mh_tips/a3_mhfloatBorder");
                Text_DoubleTime = Bar_Mon.FindChild("Text").GetComponent<Text>();
            }
            if (Bar_Mon.gameObject.activeSelf)
                MHAnimator?.SetTrigger("end");
            Count = 0;
            for (int i = 0; i < Mon_Icons.Count; i++)
                Mon_Icons[i]?.gameObject.SetActive(false);
            b_clear = !(b_reset = false);
            A3_ActiveModel.getInstance().listKilled.Clear();
        }
        public void ReloadData(Variant mapInfo)
        {
            Clear();
            if (mapInfo.Count == 0)
                return;
            A3_ActiveModel.getInstance().mwlr_mon_killed = 0;
            Count = mapInfo.Count;
            for (int i = 0; i < Count; i++)
                if (mapInfo[i]["kill"]._bool)
                {
                    A3_ActiveModel.getInstance().mwlr_mon_killed++;
                    if (!A3_ActiveModel.getInstance().listKilled.Contains(i))
                        A3_ActiveModel.getInstance().listKilled.Add(i);
                }
            a3_active.MwlrIsDoing = true;
            Reset();
            if(MHInfo != null)
                MHInfo.gameObject.SetActive(true);
        }
    }

    //魔物猎人
    class a3_active_mwlr : a3BaseActive
    {
        public static a3_active_mwlr instance;
        private Text textTargetMonsterName;
        public a3_active_mwlr(Window win, string pathStr)
            : base(win, pathStr) { }
        public BaseButton searchbtn;
        public BaseButton createTeamBtn, searchTeamBtn;
        Transform search_ani;
        GameObject I302;
        GameObject goTotalLeftTime;
        Image timebar;
        float needtime;
        GameObject createobj;
        GameObject camobj;
        Transform tfReward;
        Transform tfRewardIcon;
        Transform rewardItemTip;
        Text rewardItemTip_itemName;
        Text rewardItemTip_itemDesc;
        Transform rewardItemTip_IconRoot;
        public override void init()
        {
            instance = this;
            textTargetMonsterName = transform.FindChild("B/roleimg/text").GetComponent<Text>();//猎杀目标的文本
            timebar = getComponentByPath<Image>("B/info/1/time/bar");//双倍时间进度条图片
            search_ani = getTransformByPath("search_ani");//开始搜索容器的名字
            I302 = getTransformByPath("B/info/3/S/1").gameObject;//宝箱右边的图片
            var img = getTransformByPath("B/roleimg/img");//猎杀目标图片
            if (img != null) img.gameObject.SetActive(false);
            tfReward = transform.FindChild("A/reward/rewardScroll/rewardLayout");//猎杀奖励图标所在的容器
            rewardItemTip = transform.FindChild("A/reward/rewardItemTip");//暂时没看懂     床前明月光
            rewardItemTip_itemName = rewardItemTip.FindChild("text_bg/nameBg/itemName").GetComponent<Text>();//猎杀奖励不同的描述锁代表装备的文本（装备，首饰等）
            rewardItemTip_itemDesc = rewardItemTip.FindChild("text_bg/text").GetComponent<Text>();//对上面所出现东西进行的描述
            rewardItemTip_IconRoot = rewardItemTip.FindChild("text_bg/iconbg/icon");//猎杀奖励的图标
            new BaseButton(rewardItemTip.FindChild("close_btn")).onClick = (GameObject go) => rewardItemTip.gameObject.SetActive(false);
            tfRewardIcon = transform.FindChild("A/reward/template/icon");
            var rewardItemList = XMLMgr.instance.GetSXML("monsterhunter").GetNodeList("item");
            if (rewardItemList != null)
            {
                for (int i = 0; i < rewardItemList.Count; i++)//6
                {
                    uint item_id = rewardItemList[i].getUint("item");
                    GameObject item = IconImageMgr.getInstance().createA3ItemIcon(item_id,false,scale:0.8f,ignoreLimit: true, isicon: true);
                    GameObject rewardIcon = GameObject.Instantiate(tfRewardIcon.gameObject);
                    item.transform.SetParent(rewardIcon.transform, false);
                    item.transform.SetAsFirstSibling();
                    CreateButtonShowReward(rewardIcon, item_id);
                    rewardIcon.transform.SetParent(rewardIcon.transform, false);
                    rewardIcon.transform.SetParent(tfReward, false);
                }
            }
            searchbtn = new BaseButton(getTransformByPath("A/searchbtn"));
            searchbtn.onClick = (GameObject go) =>
            {
                if (PlayerModel.getInstance().line != 0)
                {//当支线时要切换到主线
                    string str = ContMgr.getCont("changeline0");
                    MsgBoxMgr.getInstance().showConfirm(str, () =>
                    {
                        SelfRole.Transmit(MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID], null, false, false, 0);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                    });
                }
                else
                {
                    if (!PlayerModel.getInstance().IsCaptainOrAlone)
                    {
                        Globle.err_output(-6600);
                        return;
                    }
                    a3_active_mwlr_kill.Instance.Clear();
                    a3_active_mwlr_kill.Instance.NewAction = true;
                    a3_active_mwlr_kill.Instance.Reset();
                    search_ani.gameObject.SetActive(true);
                    PlaySearchAni();
                }
            };
            searchTeamBtn = new BaseButton(getTransformByPath("A/searchteambtn"));
            searchTeamBtn.onClick = (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add(2 /*monster hunter*/);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPEEDTEAM, arr);


            };
            createTeamBtn = new BaseButton(getTransformByPath("A/createteambtn"));
            createTeamBtn.onClick = (GameObject go) =>
            {
                TeamProxy.getInstance().SendCreateTeam(2 /* monster hunter */);
            };
            new BaseButton(getTransformByPath("search_ani/panel1/bg_0")).onClick = (GameObject go) =>//开始搜索的黑色幕布当点击他的时候
            {
                cd.hide();//开始搜索界面消失
                search_ani.gameObject.SetActive(false);//开始搜索界面的容隐藏
            };
            new BaseButton(getTransformByPath("search_ani/panel2/bg_0")).onClick = (GameObject go) =>//panel的黑色的幕布
            {
                search_ani.gameObject.SetActive(false);
            };
            new BaseButton(getTransformByPath("B/info/1/giveup")).onClick = (GameObject go) =>//终止按钮
            {
                a3_active_mwlr_kill.Instance.NewAction = false;
                A3_ActiveProxy.getInstance().SendGiveUpHunt();
            };
            A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_MWLR_NEW, (e) => { a3_active_mwlr_kill.Instance.ReloadData(A3_ActiveModel.getInstance().mwlr_map_info); });

            inText();
        }
        // StringUtils.formatText(x.getString("desc"));
        void inText() {
            this.transform.FindChild("timer/titleLeftTime").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_14");//双倍奖励时制
            this.transform.FindChild("timer/titleTotalLeftTime").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_15");//猎杀时限文本
            this.transform.FindChild("charges").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_16");//今日猎杀次数
            this.transform.FindChild("A/title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_17");//活动说明
            this.transform.FindChild("A/createteambtn/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_18");//创建队伍
            this.transform.FindChild("A/searchteambtn/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_19");//寻找队伍
            this.transform.FindChild("A/notice").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_20"));//活动说明介绍的文本
            this.transform.FindChild("A/reward/items_title/title").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_21"));//猎杀奖励
            //sthis.transform.FindChild("B/roleimg/text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_22"));
            this.transform.FindChild("B/info/4/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_23"));//目标位置

        }

        private void CreateButtonShowReward(GameObject rewardIcon, uint itemId)//第一次打开活动界面的时候会运行一次，猜测应该是和魔物猎人副本有关
        {
            SXML itemInfo = a3_BagModel.getInstance().getItemXml((int)itemId);
            string desc = itemInfo.getString("desc"),
                   item_name = itemInfo.getString("item_name");
            int showType = itemInfo.getInt("show_type");
            showType = showType == -1 ? 1 : showType;
            new BaseButton(rewardIcon.transform.FindChild("btn")).onClick = (GameObject go) =>
            {
                //rewardItemTip.gameObject.SetActive(true);
                //rewardItemTip_itemName.text = item_name;
                //rewardItemTip_itemDesc.text = desc;
                //if (rewardItemTip_IconRoot.childCount > 0)
                //    GameObject.Destroy(rewardItemTip_IconRoot.GetChild(0).gameObject);
                //IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(rewardItemTip_IconRoot, false);
                ArrayList paraList = new ArrayList();
                paraList.Add(itemId);
                paraList.Add(showType);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
            };
        }
        public override void onShowed()
        {
            DestroyModel();
            instance = this;
            A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_MLZDOPCUCCESS, Refresh);            
            search_ani.gameObject.SetActive(false);
            Refresh();
            if (getTransformByPath("A").gameObject.activeSelf)
            {
                if (TeamProxy.getInstance().MyTeamData != null)
                {
                    searchbtn.gameObject.SetActive(true);
                    searchTeamBtn.gameObject.SetActive(false);
                    createTeamBtn.gameObject.SetActive(false);
                }
                else
                {
                    searchbtn.gameObject.SetActive(false);
                    searchTeamBtn.gameObject.SetActive(true);
                    createTeamBtn.gameObject.SetActive(true);
                }
            }
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, OnCreateTeamSuccess);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, OnLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, OnJoinTeamSuccess);
        }
        void RefreshLeftTimes()
        {

        }
        public override void onClose()
        {
            DestroyModel();
            instance = null;
            needtime = 0;
            A3_ActiveProxy.getInstance().removeEventListener(A3_ActiveProxy.EVENT_MLZDOPCUCCESS, Refresh);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CREATETEAM, OnCreateTeamSuccess);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_LEAVETEAM, OnLeaveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_AFFIRMINVITE, OnJoinTeamSuccess);
        }
        public void OnLeaveTeam(GameEvent e)
        {
            flytxt.instance.fly(ContMgr.getCont("a3_active_gooutranks"));
            searchbtn.gameObject.SetActive(false);
            searchTeamBtn.gameObject.SetActive(true);
            createTeamBtn.gameObject.SetActive(true);
        }

        private void OnCreateTeamSuccess(GameEvent e)
        {
            if (e.data.ContainsKey("ltpid") && e.data["ltpid"] == 2/*monster hunter*/)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_active_creatrveranks"));
                searchbtn.gameObject.SetActive(true);
                searchTeamBtn.gameObject.SetActive(false);
                createTeamBtn.gameObject.SetActive(false);
            }
        }
        private void OnJoinTeamSuccess(GameEvent e)
        {
            flytxt.instance.fly(ContMgr.getCont("a3_active_addranks"));
            searchbtn.gameObject.SetActive(true);
            searchTeamBtn.gameObject.SetActive(false);
            createTeamBtn.gameObject.SetActive(false);
        }
        void PlaySearchAni()
        {
            search_ani.FindChild("panel1").gameObject.SetActive(true);
            search_ani.FindChild("panel2").gameObject.SetActive(false);
            cd.updateHandle = onCD;
            cd.show(() =>
            {
                A3_ActiveProxy.getInstance().SendStartHunt();
            }, 3f, false, null, new Vector3(67, 32, 0));
        }
        public static void onCD(cd item)
        {
            int temp = (int)(cd.secCD - cd.lastCD) / 100;
            //item.txt.text = "搜索中 " + ((float)temp / 10f).ToString();
            item.txt.text = "";

        }

        void Refresh(GameEvent e = null)
        {
            //当日可用次数
            int thisday_count = XMLMgr.instance.GetSXML("monsterhunter.daily_cnt").getInt("num"); ;
            //vip额外可购买次数
            int vip_canbuycount = A3_VipModel.getInstance().vip_exchange_num(24);
            //vip已经够买次数
            int vip_buycount = A3_ActiveModel.getInstance().vip_buy_count;
            //vip可购买剩余次数
            int vip_havecount = vip_canbuycount - vip_buycount;
            //现有总次数
            int allcount = thisday_count + vip_buycount;


            getTransformByPath("charges/num").GetComponent<Text>().text = A3_ActiveModel.getInstance().mwlr_charges + "/" + allcount;
            getTransformByPath("vipcount/num").GetComponent<Text>().text = vip_havecount.ToString();
            new BaseButton(getTransformByPath("vipcount/addnum")).onClick = (GameObject go) =>
            {
                int price = XMLMgr.instance.GetSXML("monsterhunter.vip_cost").getInt("value"); ;
                if (vip_havecount <= 0)
                {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                }
                else
                {
                    MsgBoxMgr.getInstance().showConfirm(price + ContMgr.getCont("zuanshi_ci"), () =>
                    {
                        A3_ActiveProxy.getInstance().SendVipCount();
                    });
                }

            };

            var vv1 = search_ani.FindChild("panel1").gameObject;
            if (vv1.activeSelf) vv1.SetActive(false);
            //var vv2 = search_ani.FindChild("panel2").gameObject;
            //if (!vv2.activeSelf) vv2.SetActive(true);
            if (A3_ActiveModel.getInstance().mwlr_map_info.Count > 0 &&
                A3_ActiveModel.getInstance().mwlr_map_info[0]["target_mid"]._int > 0)
            {
                a3_active_mwlr_kill.Instance.ReloadData(A3_ActiveModel.getInstance().mwlr_map_info);
                getTransformByPath("A").gameObject.SetActive(false);
                getTransformByPath("B").gameObject.SetActive(true);
                var xx = XMLMgr.instance.GetSXML("monsters").GetNode("monsters", "id==" + A3_ActiveModel.getInstance().mwlr_map_info[0]["target_mid"]);
                search_ani.FindChild("panel2/name").GetComponent<Text>().text = xx.getString("name");
                CreatModel(xx.getString("obj"));
            }
            else
            {
                getTransformByPath("A").gameObject.SetActive(true);
                getTransformByPath("B").gameObject.SetActive(false);
                DestroyModel();
                searchbtn.interactable = (allcount- A3_ActiveModel.getInstance().mwlr_charges) > 0;
            }




            Re_Time();
            //Transform pf = getTransformByPath("B/info/4/0");
            Transform pf = getTransformByPath("B/info/4/0_map");
            Transform root = getTransformByPath("B/info/4/S");
            foreach (var v in root.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == root) GameObject.Destroy(v.gameObject);
            }
            //foreach (var v in A3_ActiveModel.getInstance().mwlr_map_id)
            for (int i = 0; i < A3_ActiveModel.getInstance().mwlr_map_id.Count; i++)
            {
                GameObject g = GameObject.Instantiate(pf.gameObject) as GameObject;
                g.transform.SetParent(root);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = Vector3.one;
                g.transform.GetChild(0).gameObject.SetActive(A3_ActiveModel.getInstance().mwlr_map_info[i]["kill"]._bool);
                if (A3_ActiveModel.getInstance().mwlr_map_id[i] > 0)
                {
                    var vv = SvrMapConfig.instance.mapConfs[(uint)A3_ActiveModel.getInstance().mwlr_map_id[i]];
                    g.GetComponent<Text>().text = vv["map_name"];
                }
                g.gameObject.SetActive(true);
                List<int> t = new List<int>();
                t.Add(A3_ActiveModel.getInstance().mwlr_map_id[i]);
                new BaseButton(g.transform).onClick = (GameObject go) =>
                {
                    SXML mapNodeInfo = XMLMgr.instance.GetSXML("monsterhunter").GetNode("map", "mapid==" + t[0]);
                    int point_id = mapNodeInfo.getInt("point_id");

                    A3_ActiveModel.getInstance().mwlr_target_monId = A3_ActiveModel.getInstance().mwlr_map_info[0]["target_mid"];// 现在用t[0],因为一共就一个怪,以后可能会增加更多的怪物,因此先准备成一个List
                    A3_ActiveModel.getInstance().mwlr_target_pos = new Vector3(
                                x: A3_ActiveModel.getInstance().mwlr_mons_pos[t[0]].x,
                                y: 0,
                                z: A3_ActiveModel.getInstance().mwlr_mons_pos[t[0]].z);
                    A3_ActiveModel.getInstance().mwlr_on = true;

                    // MapProxy.getInstance().sendBeginChangeMap(point_id, true); // -- 传送到地图入口
                    int mapId = mapNodeInfo.getInt("mapid");
                    Vector3 vec = A3_ActiveModel.getInstance().mwlr_target_pos;
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
                    //SelfRole.WalkToMap(
                    //    id: mapNodeInfo.m_dAtttr["mapid"].intvalue,
                    //    vec: A3_ActiveModel.getInstance().mwlr_target_pos,
                    //    handle: () =>
                    //    {
                    //        if (A3_ActiveModel.getInstance().mwlr_on)
                    //            if (SelfRole._inst.m_moveAgent.remainingDistance < 1f)
                    //                if (!SelfRole.fsm.Autofighting)
                    //                {
                    //                    SelfRole.fsm.StartAutofight();
                    //                    SelfRole.fsm.ChangeState(StateAttack.Instance);
                    //                }
                    //    }
                    //);// -- 走着去


                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                };
            }
            if (A3_ActiveModel.getInstance().mwlr_doubletime <= 0)
                needtime = 0;
            else
                needtime = A3_ActiveModel.getInstance().mwlr_doubletime - muNetCleint.instance.CurServerTimeStamp;

            if (A3_ActiveModel.getInstance().mwlr_map_info.Count > 0)
            {
                textTargetMonsterName.text = XMLMgr.instance.GetSXML(
                    id: "monsters.monsters",
                    filter: "id==" + A3_ActiveModel.getInstance().mwlr_map_info[0]["target_mid"]._str
                ).getString(key: "name"); // 目前怪物都是同一种
            }
        }

        void CreatModel(string obj)
        {
            if (createobj != null)
            {
                GameObject.DestroyImmediate(createobj);
                GameObject.DestroyImmediate(camobj);
            }
            var obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + obj);
            createobj = GameObject.Instantiate(obj_prefab, new Vector3(-153.43f, 1f, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in createobj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = createobj.transform.FindChild("model");
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            camobj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = camobj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 180f);
            cur_model.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
            foreach (Transform tran in obj_prefab.GetComponentsInChildren<Transform>(true))
            {
                tran.gameObject.layer = EnumLayer.LM_FX;
            }

        }

        void DestroyModel()
        {
            if (createobj != null)
                GameObject.DestroyImmediate(createobj);
            if (camobj != null)
                GameObject.DestroyImmediate(camobj);

        }

        public void Re_Time()
        {
            if (A3_ActiveModel.getInstance().mwlr_totaltime <= 0)
            {
                getTransformByPath("timer/timerTotalLeftTime").GetComponent<Text>().text = "00:00:00";
                getTransformByPath("timer/timerRec").GetComponent<Text>().text = "00:00:00";
                return;
            }
            if (A3_ActiveModel.getInstance().mwlr_doubletime <= 0)
                getTransformByPath("timer/timerRec").GetComponent<Text>().text = "00:00:00";
                //    if (A3_ActiveModel.getInstance().mwlr_doubletime <= 0)
                //{
                //    return;
                //}
            TimeSpan ts = new TimeSpan(0, 0, Mathf.Max(0, A3_ActiveModel.getInstance().mwlr_doubletime - muNetCleint.instance.CurServerTimeStamp));
            TimeSpan tTotal = new TimeSpan(0, 0, Mathf.Max(0, A3_ActiveModel.getInstance().mwlr_totaltime - muNetCleint.instance.CurServerTimeStamp));
            if (getTransformByPath("B").gameObject.activeSelf)
            {
                getTransformByPath("timer/timerRec").GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
                getTransformByPath("timer/timerTotalLeftTime").GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", tTotal.Hours, tTotal.Minutes, tTotal.Seconds);
            }
            else
            {
                getTransformByPath("timer/timerRec").GetComponent<Text>().text = "00:00:00";
                getTransformByPath("timer/timerTotalLeftTime").GetComponent<Text>().text = "00:00:00";
            }
            timebar.fillAmount = (float)ts.TotalSeconds / needtime;

            if (A3_ActiveModel.getInstance().mwlr_doubletime - muNetCleint.instance.CurServerTimeStamp > 0)
            {
                if (!I302.activeSelf) I302.SetActive(true);
            }
            else
            {
                if (I302.activeSelf) I302.SetActive(false);
            }
        }
    }


    //抢宝箱
    class a3_active_forchest : a3BaseActive
    {
        public static a3_active_forchest instance;
        public bool box_ing;
        BaseButton goToMap;
        GameObject help;
        string[]  time1;
        string[]  time2;
        GameObject pre;
        Transform contain;
        public a3_active_forchest(Window win, string pathStr)
            : base(win, pathStr)
        {

        }
        public override void init()
        {
            pre = this.transform.FindChild("bg2/items/Image").gameObject;
            contain = this.transform.FindChild("bg2/items/scroll/contain");
            help = this.transform.FindChild("help").gameObject;
            new BaseButton(this.transform.FindChild("bg2/hp")).onClick = (GameObject go) =>
            {
                help.SetActive(true);
            };
            new BaseButton(transform.FindChild("help/panel_help/bg/closeBtn")).onClick = (GameObject go) =>
            {
                help.SetActive(false);
            };

            SXML xml = XMLMgr.instance?.GetSXML("box.box", "id==" + 1);
             time1 = xml.getString("time1").Split(',');
             time2 = xml.getString("time2").Split(',');

            new BaseButton(transform.FindChild("bg2/btn_transmit")).onClick = (GameObject go) =>
            {


                if (!box_ing)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_active_nohavetime"));
                }
                else
                {
                    //a3_active.instance.map_light = true;
                    //InterfaceMgr.getInstance().worldmap = true;
                    ////InterfaceMgr.getInstance().open(InterfaceMgr.WORLD_MAP).winItem.transform.SetAsLastSibling();
                    //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.WORLD_MAP);

                    if (PlayerModel.getInstance().line != 0)
                    {//当支线时要切换到主线
                        string str = ContMgr.getCont("changeline0");
                        MsgBoxMgr.getInstance().showConfirm(str, () =>
                        {
                            SelfRole.Transmit(MapModel.getInstance().dicMappoint[10],null,false,false,0);
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                        });
                     }
                    else
                    {
                        if (GRMap.instance.m_nCurMapID != 10)
                            SelfRole.Transmit(MapModel.getInstance().dicMappoint[10]);
                        else
                            flytxt.instance.fly(ContMgr.getCont("a3_active_samemap"));
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                    }
                }
                // InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);

                //InterfaceMgr.getInstance().worldmap = false;
            };
            inText();

        }
        void inText()
        {
            this.transform.FindChild("bg2/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_24");
            this.transform.FindChild("bg2/btn_transmit/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_25");
            this.transform.FindChild("bg2/items").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_26");
            this.transform.FindChild("bg2/info/context").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_27");
            this.transform.FindChild("bg2/info/context1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_28");
        }
        public override void onShowed()
        {
            destorycontain();
            image_show();
            int hour = System.DateTime.Now.Hour;
           
            int min = System.DateTime.Now.Minute;
            if ( hour + (min / 60f) > (int.Parse(time1[0]) + ((int.Parse(time1[1]) - 15) / 60f)) && hour + (min / 60f) <= (int.Parse(time1[0]) + ((int.Parse(time1[1]) + 25) / 60f)))
            {
                box_ing = true;
            }
           else if (hour + (min / 60f) > (int.Parse(time2[0]) + ((int.Parse(time2[1]) - 15) / 60f)) && hour + (min / 60f) <= (int.Parse(time2[0]) + ((int.Parse(time2[1]) + 25) / 60f)))
            {
                box_ing = true;
            }
            else
                box_ing = false;
        
        }
        public void image_show()
        {
            List<SXML> x = XMLMgr.instance.GetSXMLList("box.award");
           // List<SXML> x = XMLMgr.instance.GetSXMLList("huoyue.reward");
            for (int i = 0; i < x.Count; i++)
            {
                GameObject objClone = GameObject.Instantiate(pre) as GameObject;
                objClone.SetActive(true);
                objClone.transform.SetParent(contain,false);
                int cc = x[i].getInt("item_id");
                int color = a3_BagModel.getInstance().getItemDataById((uint)cc).quality;
                objClone.transform.FindChild("Image").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_itemborder_b039_0" + color);
                objClone.transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + cc);
                BaseButton btn = new BaseButton(objClone.transform);
                btn.onClick = delegate (GameObject goo)
                {
                    //ArrayList data1 = new ArrayList();
                    //a3_BagItemData one = a3_BagModel.getInstance().getItems()[(uint)cc];
                                  
                    //data1.Add(one);
                    //data1.Add(equip_tip_type.Comon_tip);
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_EQUIPTIP, data1);


                    ArrayList arr = new ArrayList();
                    arr.Add((uint)cc);//第一项为物品id
                    arr.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                };
            }


        }
        public void destorycontain()
        {
            if (contain.transform.childCount > 0)
            {
                for (int i = contain.transform.childCount-1; i>=0; i--)
                {
                    GameObject.Destroy(contain.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    //藏宝图
    class a3_active_findbtu : a3BaseActive
    {//./steal_monster
        public a3_active_findbtu(Window win, string pathStr)
            : base(win, pathStr)
        {
        }
        bool open = false;
        public static a3_active_findbtu instans;
        Text Time;
        Transform itemViewCon;
        GameObject tip;
        public override void init()
        {
            new BaseButton(transform.FindChild("b/help")).onClick = (GameObject go) =>
            {
                transform.FindChild("b/tishi").gameObject.SetActive(true);
            };

            new BaseButton(transform.FindChild("b/tishi/tach")).onClick =
                new BaseButton(transform.FindChild("b/tishi/close")).onClick = (GameObject go) =>
                {
                    transform.FindChild("b/tishi").gameObject.SetActive(false);
                };
            tip = this.transform.FindChild("close_desc").gameObject;
            itemViewCon = this.transform.FindChild("a/body/itemView/content");
            Time = transform.FindChild("a/time").GetComponent<Text>();
            setrewards();
            this.getEventTrigerByPath("b/avatar_touch").onDrag = OnDrag;

            inText();
        }

        void inText()
        {
            this.transform.FindChild("a/tishi/title/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_29");
            this.transform.FindChild("a/tishi/title/notice").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_30"));
            this.transform.FindChild("b/text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_active_31"));
            this.transform.FindChild("close_desc/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_32");
            this.transform.FindChild("close_desc/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_active_33");
        }
        public override void onShowed()
        {
            instans = this;
            transform.FindChild("b/tishi").gameObject.SetActive(false);
            FindBestoProxy.getInstance().addEventListener(FindBestoProxy.EVENT_INFO, oninfo);
            FindBestoProxy.getInstance().getinfo();
            tip.SetActive(false);
        }
        public override void onClose()
        {
            instans = null;
            runTime = 0;
            FindBestoProxy.getInstance().removeEventListener(FindBestoProxy.EVENT_INFO, oninfo);
            SetDisposeAvatar();
        }
       public GameObject monobj;
        GameObject Camobj;
        void OnDrag(GameObject go, Vector2 delta)
        {
            if (monobj != null)
            {
                monobj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        void SetCreateAvatar()
        {
            SetDisposeAvatar();
            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + 10066);
            monobj = GameObject.Instantiate(obj_prefab, new Vector3(-153.317f, 0.445f, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in monobj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = monobj.transform.FindChild("model");
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            var t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            Camobj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = Camobj.GetComponentInChildren<Camera>();
            cur_model.Rotate(Vector3.up, 180f);
            cur_model.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }

        }
        void SetDisposeAvatar()
        {
            if (monobj != null)
            {
                GameObject.Destroy(monobj);
                GameObject.Destroy(Camobj);
            }
        }
        List<int> mapid = new List<int>();
        int runTime = 0;
        void oninfo(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("count_down"))
            {
                runTime = data["count_down"];
            }
            if (data.ContainsKey("count_down") && data["count_down"] > 0)
                open = false;
            else if(data.ContainsKey("mapid")&& data["mapid"].Count > 0)
            {
                open = true;
            }
            if (open)
            {
                mapid.Clear();
                for (int i = 0; i < data["mapid"].Count; i++)
                {
                    mapid.Add(data["mapid"][i]);
                }
                transform.FindChild("b").gameObject.SetActive(true);
                transform.FindChild("a").gameObject.SetActive(false);
                SetCreateAvatar();
                setBtns();
            }
            else
            {
                transform.FindChild("b").gameObject.SetActive(false);
                transform.FindChild("a").gameObject.SetActive(true);
                SetDisposeAvatar();
                if (runTime > 0)
                {
                    Time.gameObject.SetActive(true);
                    this.transform.FindChild("a/no_open").gameObject.SetActive(false);
                    a3_active.onshow?.Runtimer_bt(runTime);
                }
                else
                {
                    Time.gameObject.SetActive(false);
                    this.transform.FindChild("a/no_open").gameObject.SetActive(true);
                }
                return;
            }
        }


        public void showtime(int time)
        {
            TimeSpan ts = new TimeSpan(0, 0, time);
            if (ts.Minutes >= 10 && ts.Seconds >= 10)
                Time.text = ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
            else if (ts.Minutes < 10 && ts.Seconds >= 10)
                Time.text = ts.Hours + ":0" + ts.Minutes + ":" + ts.Seconds;
            else if (ts.Minutes >= 10 && ts.Seconds < 10)
                Time.text = ts.Hours + ":" + ts.Minutes + ":0" + ts.Seconds;
            else if (ts.Minutes < 10 && ts.Seconds < 10)
                Time.text = ts.Hours + ":0" + ts.Minutes + ":0" + ts.Seconds;
        }

        void setBtns()
        {
            // transform.FindChild ("b/btns")
            for (int i = 0;
                i < transform.FindChild("b/btns").childCount; i++)
            {
                Variant vmap = SvrMapConfig.instance.getSingleMapConf((uint)mapid[i]);
                string name = vmap.ContainsKey("map_name") ? vmap["map_name"]._str : "--";
                transform.FindChild("b/btns").GetChild(i).FindChild("name").GetComponent<Text>().text = name;
                transform.FindChild("b/btns").GetChild(i).FindChild("lvl").GetComponent<Text>().text = ContMgr.getCont("a3_resetlvl_lv", new List<string>() { vmap["lv_up"], vmap["lv"] });



                int mapicon = vmap.ContainsKey("icon") ? vmap["icon"]._int : 1;
                transform.FindChild("b/btns").GetChild(i).GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_treasure_" + mapicon);
                bool canfly = false;
                int point = vmap.ContainsKey("point_id") ? vmap["point_id"]._int : 101;
                if (PlayerModel.getInstance().up_lvl > vmap["lv_up"])
                {
                    canfly = true;
                }
                else if (PlayerModel.getInstance().up_lvl == vmap["lv_up"])
                {
                    if (PlayerModel.getInstance().lvl >= vmap["lv"])
                    {
                        canfly = true;
                    }
                    else
                    {
                        canfly = false;
                    }

                }
                else
                {
                    canfly = false;
                }
                new BaseButton(transform.FindChild("b/btns").GetChild(i).FindChild("go")).onClick = (GameObject go) =>
               {
                   if (canfly)
                   {
                       if (PlayerModel.getInstance().line != 0)
                       {//当支线时要切换到主线
                           string str = ContMgr.getCont("changeline0");
                           MsgBoxMgr.getInstance().showConfirm(str, () =>
                           {
                               SelfRole.Transmit(point, null, false, false, 0);
                               InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                           });
                       }
                       else
                       {
                           SelfRole.Transmit(toid: point);
                           InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVE);
                           // worldmapsubwin.id = point;
                           //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.WORLD_MAP_SUB);
                       }
                   }
                   else
                   {
                       flytxt.instance.fly(ContMgr.getCont("a3_active_nolv"));
                   }
               };
            }
        }

        void setrewards()
        {
            GameObject item = this.transform.FindChild("a/body/itemView/item").gameObject;
            SXML xml = XMLMgr.instance.GetSXML("treasure_reward");
            List<SXML> stagelist = xml.GetNodeList("reward");
            for (int i = 0; i < stagelist.Count; i++)
            {
                GameObject clon = (GameObject)GameObject.Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(itemViewCon, false);
                clon.transform.FindChild("count").GetComponent<Text>().text = stagelist[i].getInt("cost").ToString();
                int id = stagelist[i].getInt("item_id");
                GameObject con_item = clon.transform.FindChild("icon").gameObject;
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById((uint)id), false, -1, 0.8f);
                icon.transform.SetParent(con_item.transform, false);

                new BaseButton(con_item.transform).onClick = (GameObject go) =>
                {
                    tip.SetActive(true);
                    SXML x = XMLMgr.instance.GetSXML("item.item", "id==" + id);
                    tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = x.getString("item_name");
                    tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(x.getInt("quality"));
                    tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid((uint)id) + ContMgr.getCont("ge");
                    if (x.getInt("use_limit") == 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
                    else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = x.getString("use_limit") + ContMgr.getCont("zhuan"); }
                    tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(x.getString("desc"));
                    tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + x.getInt("icon_file"));
                    new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
                };

            }
        }
    }

    //竞技场
    class a3_active_pvp : a3BaseActive
    {
        public static a3_active_pvp instance;
        Text findCount;
        Text buyCount;
        Text duanwei;
        Text top_saiji;
        int findid = 0;
        Image rank;
        //Text giftzuanshi;
        //Text giftmingwang;
        //Text box;
        Text buy_Count_zuan;
        GameObject tip;
        public GameObject no_open;
        public GameObject yes_open;
        public a3_active_pvp(Window win, string pathStr) : base(win, pathStr)
        { }

        public override void init()
        {
            tip = this.transform.FindChild("tip").gameObject;
            findCount = this.transform.FindChild("find_info/day_count/text").GetComponent<Text>();
            buyCount = this.transform.FindChild("find_info/buy_count/text").GetComponent<Text>();
           // giftzuanshi = this.transform.FindChild("GetReward/tem1/zuan/bg_text/tex").GetComponent<Text>();
           // giftmingwang = this.transform.FindChild("GetReward/tem1/mingwang/bg_text/tex").GetComponent<Text>();
           // box = this.transform.FindChild("GetReward/tem1/box_name/tex").GetComponent<Text>();
            buy_Count_zuan = this.transform.FindChild("find_info/find/text2/text").GetComponent<Text>();
            rank = this.transform.FindChild("info/icon").GetComponent<Image>();
            duanwei = this.transform.FindChild("info/duanwei").GetComponent<Text>();
            top_saiji = this.transform.FindChild("top_text").GetComponent<Text>();
            no_open = this.transform.FindChild("find_info/no_open").gameObject;
            yes_open = this.transform.FindChild("find_info/find").gameObject;
            new BaseButton(getTransformByPath("reward")).onClick = (GameObject go) =>
            {
                 A3_ActiveProxy.getInstance().SendPVP(5);

            };
            new BaseButton(getTransformByPath("rank")).onClick = (GameObject go) =>
            {
                //排行
            };
            new BaseButton(getTransformByPath("find_info/find")).onClick = (GameObject go) =>
            {
                if(a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.jjc);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE,ast);
                }
                else
                {
                    EnterOnClick();
                }

            };
            //new BaseButton(getTransformByPath("GetReward/tem1/back")).onClick = (GameObject go) =>
            //{
            //    this.transform.FindChild("GetReward").gameObject.SetActive(false);
            //};
            //new BaseButton(getTransformByPath("GetReward/tem1/Get")).onClick = (GameObject go) =>
            //{
            //    //领取奖励
            //    A3_ActiveProxy.getInstance().SendPVP(5);
            //};
            new BaseButton(getTransformByPath("Get_tab_bt")).onClick = (GameObject go) =>
            {
                //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(false);
                this.transform.FindChild("GetInfo_tab").gameObject.SetActive(true);
                intoTab();
            };
            new BaseButton(getTransformByPath("GetInfo_tab/back")).onClick = (GameObject go) =>
            {
                //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(true);
                this.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            };
            new BaseButton(getTransformByPath("Finding/back")).onClick = (GameObject go) =>
            {
                //取消匹配
                A3_ActiveProxy.getInstance().SendPVP(3);
            };
            ref_zuan_count();
        }

        public void EnterOnClick()
        {

            //搜索对手
            if (findid == 0)
            {
                A3_ActiveProxy.getInstance().SendPVP(2);
            }
            else
            {
                if (A3_ActiveModel.getInstance().buy_cnt <= A3_ActiveModel.getInstance().buyCount)
                    flytxt.instance.fly("可购买次数不足");
                else
                {
                    A3_ActiveProxy.getInstance().SendPVP(4);
                }
            }
        }


        bool b = true;
        public override void onShowed()
        {
            tip.SetActive(false);
            A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_PVPSITE_INFO, Refresh);
            A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_PVPGETREW, ReGet);
            if (b)
            {
                A3_ActiveProxy.getInstance().SendPVP(1);
                b = false;
            }
            instance = this;
            refro_score();
            A3_ActiveProxy.getInstance().SendPVP(6);
            refCount();
        }
        public override void onClose()
        {
            this.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            refInto();
            instance = null;
            A3_ActiveProxy.getInstance().removeEventListener(A3_ActiveProxy.EVENT_PVPSITE_INFO, Refresh);
            A3_ActiveProxy.getInstance().removeEventListener(A3_ActiveProxy.EVENT_PVPGETREW, ReGet);
        }
        private void refresh_opentime()
        {
            List<SXML> opentimeList = XMLMgr.instance.GetSXML("jjc").GetNodeList("t");
            int i = 0;
            int timezoneSpan = (DateTime.Now - DateTime.UtcNow).Hours;
            if (timezoneSpan < 0) timezoneSpan = timezoneSpan + 12;
            int cur_h = (muNetCleint.instance.CurServerTimeStamp / 3600 + timezoneSpan) % 24;
            for (; i < opentimeList.Count; i++)
            {
                int open_h = int.Parse(opentimeList[i].getString("opnetime").Split(',')[0]);
                if (cur_h < open_h)
                {
                    //no_open.transform.FindChild("text1").GetComponent<Text>().text = string.Format("{0}点开放", open_h);
                    no_open.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_open_time", new List<string> { open_h.ToString() });
                    break;
                }
            }
            if (opentimeList.Count > 0 && i == opentimeList.Count)
                no_open.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_opentmmo_time", new List<string> { opentimeList[0].getString("opnetime").Split(',')[0] });
            //no_open.transform.FindChild("text1").GetComponent<Text>().text = string.Format("明日{0}点开放", int.Parse(opentimeList[0].getString("opnetime").Split(',')[0]));

        }
        public void setbtn(bool open)
        {
            no_open.SetActive(!open);
            yes_open.SetActive(open);
            refresh_opentime();
        }

        bool frist = true;
        //载入竞技奖励说明表
        void intoTab()
        {
            if (frist)
            {
                Transform con = this.transform.FindChild("GetInfo_tab/scrollview/con");
                if (con.childCount > 0)
                {
                    for (int i = 0; i < con.childCount; i++)
                    {
                        GameObject.Destroy(con.GetChild(i).gameObject);
                    }
                }
                GameObject item = this.transform.FindChild("GetInfo_tab/scrollview/item").gameObject;
                List<SXML> Xml = XMLMgr.instance.GetSXMLList("jjc.reward");
                int length = Xml.Count;
                for (int i = length; i > 0; i--)
                {
                    GameObject clon = (GameObject)GameObject.Instantiate(item);
                    SXML itXml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + i);
                    string name = itXml.getString("name");
                    Image icon = clon.transform.FindChild("icon").GetComponent<Image>();
                    if (i < 10)
                    {
                        icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + i);
                    }
                    else
                    {
                        icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + i);
                    }
                    string color = "";
                    if (i >= 10)
                    {
                        color = "<color=#FFA500>";
                    }
                    else if (i < 10 && i >= 7)
                    {
                        color = "<color=#FF00FF>";
                    }
                    else if (i < 7)
                    {
                        color = "<color=#00BFFF>";
                    }
                    if (i == 1)
                    {
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_bangzuan") + "</color>" + 0;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_mingwang") + "</color>" + 0;
                        clon.transform.FindChild("4/Text").GetComponent<Text>().text = color + ContMgr.getCont("FriendProxy_wu") + "</color>";
                        clon.transform.FindChild("4/icon").gameObject.SetActive(false);
                    }
                    else
                    {
                        int zhuanshi = itXml.GetNode("gem").getInt("num");
                        int mingwang = itXml.GetNode("rep").getInt("num");
                        int box = itXml.GetNode("box").getInt("id");
                        var xml = XMLMgr.instance.GetSXML("item.item", "id==" + box);
                        string bx = xml.getString("item_name");
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_bangzuan")+ "</color>" + zhuanshi;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_mingwang") + "</color>" + mingwang;
                        clon.transform.FindChild("4/Text").GetComponent<Text>().text = color + bx + "</color>";
                        clon.transform.FindChild("4/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + xml.getString("icon_file"));
                        new BaseButton(clon.transform.FindChild("4")).onClick = (GameObject go) =>
                        {
                            showtip((uint)box);
                        };
                    }
                    clon.transform.FindChild("1/Text").GetComponent<Text>().text = color + name + "</color>";
                    clon.transform.SetParent(con, false);
                    clon.SetActive(true);
                }
            }
            frist = false;
        }

        void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }

        public void refGiff()
        {
            //if (A3_ActiveModel.getInstance().lastgrage <= 1)
            //{
            //    giftzuanshi.text = "0";
            //    giftmingwang.text = "0";
            //    box.text =ContMgr.getCont("FriendProxy_wu");
            //}
            //else
            //{
            //    SXML itXml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + A3_ActiveModel.getInstance().lastgrage);
            //    giftzuanshi.text = itXml.GetNode("gem").getInt("num").ToString();
            //    giftmingwang.text = itXml.GetNode("rep").getInt("num").ToString();
            //    box.text = itXml.GetNode("box").getInt("id").ToString();
            //}
        }
        public void openFind()
        {
            this.transform.FindChild("find_info").gameObject.SetActive(false);
            this.transform.FindChild("Finding").gameObject.SetActive(true);
        }
        public void CloseFind()
        {
            this.transform.FindChild("find_info").gameObject.SetActive(true);
            this.transform.FindChild("Finding").gameObject.SetActive(false);
        }
        public void refCount_buy(int Buy_Count)
        {
            refCount();
            reffind();
        }

        void ref_zuan_count()
        {
            buy_Count_zuan.text = A3_ActiveModel.getInstance().buy_zuan_count.ToString();
        }
        public void refStar(int Count)
        {
            if (A3_ActiveModel.getInstance().grade <= 0)
                return;
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + A3_ActiveModel.getInstance().grade);
            int pointCount = Xml.getInt("star");
            if (pointCount <= 0)
                return;
            Transform star = this.transform.FindChild("info/star");
            for (int i = 0; i < star.childCount; i++)
            {
                star.GetChild(i).FindChild("this").gameObject.SetActive(false);
                star.GetChild(i).gameObject.SetActive(false);
            }
            for (int m = pointCount; m > 0; m--)
            {
                star.GetChild(m - 1).gameObject.SetActive(true);
            }
            for (int j = 0; j < Count; j++)
            {
                star.GetChild(j).FindChild("this").gameObject.SetActive(true);
            }
        }
        public void refCount(int Count)
        {
            findCount.text = (A3_ActiveModel.getInstance().callenge_cnt - Count + A3_ActiveModel.getInstance().buyCount) + "/" + A3_ActiveModel.getInstance().callenge_cnt;
            reffind();
        }
        //初始化界面
        public void refInto()
        {
            //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(true);
            //this.transform.FindChild("GetReward/tem2").gameObject.SetActive(false);
            //this.transform.FindChild("GetReward").gameObject.SetActive(false);
            this.transform.FindChild("find_info").gameObject.SetActive(true);
            this.transform.FindChild("Finding").gameObject.SetActive(false);
        }
        void Refresh(GameEvent e = null)
        {
            Variant v = e.data;
            if (v["grade"] <= 0)
                return;
            top_saiji.text = ContMgr.getCont("di") + v["tour_time"] + ContMgr.getCont("saiji");
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + v["grade"]);
            duanwei.text = ContMgr.getCont("duanwei") + Xml.getString("name");
            if (v["grade"] < 10)
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + v["grade"]);
            else
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + v["grade"]);

            setGift();
            refCount();
            refStar(v["score"]);
            reffind();
        }

        void ReGet(GameEvent e)
        {
            setGift();
        }

        bool toGet = false;
        void setGift()
        {
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + A3_ActiveModel.getInstance ().lastgrage);
            this.transform.FindChild ("Gift/last_rank").GetComponent<Text>().text = Xml.getString("name");
            transform.FindChild("Gift/rew").gameObject.SetActive(false);
            transform.FindChild("Gift/geted").gameObject.SetActive(false);
            transform.FindChild("Gift/nullrew").gameObject.SetActive(false);
            this.transform.FindChild("reward").gameObject.SetActive(true);
            if (A3_ActiveModel.getInstance().lastgrage > 1) {
                if (A3_ActiveModel.getInstance().Canget <= 0)
                {
                    //有奖励
                    this.transform.FindChild("reward/tet").GetComponent<Text>().text = ContMgr.getCont("lingqu");
                    this.transform.FindChild("reward").GetComponent<Button>().interactable = true;
                    transform.FindChild("Gift/rew").gameObject.SetActive(true);
                    uint boxid = (uint)Xml.GetNode("box").getInt("id");
                    var xml = XMLMgr.instance.GetSXML("item.item", "id==" + boxid);
                    int zhuanshi = Xml.GetNode("gem").getInt("num");
                    int mingwang = Xml.GetNode("rep").getInt("num");
                    transform.FindChild("Gift/rew/gem/count").GetComponent<Text>().text = zhuanshi.ToString();
                    transform.FindChild("Gift/rew/rep/count").GetComponent<Text>().text = mingwang.ToString();
                    Image icon = transform.FindChild("Gift/rew/box").GetComponent<Image>();
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + xml.getString("icon_file"));
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        showtip(boxid);
                    };
                }
                else
                {
                    this.transform.FindChild("reward").GetComponent<Button>().interactable = false;

                    this.transform.FindChild("reward/tet").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                    transform.FindChild("Gift/geted").gameObject.SetActive(true);
                }
            }
            else {
                //无奖励
                this.transform.FindChild("reward").gameObject.SetActive(false);
                transform.FindChild("Gift/nullrew").gameObject.SetActive(true);
            }
            //Image icon = clon.transform.FindChild("icon").GetComponent<Image>();

        }
        //void setGet() {
        //    if (A3_ActiveModel .getInstance().Canget == 0)
        //    {
        //        this.transform.FindChild("reward/has").gameObject.SetActive(false);
        //        //this.transform.FindChild("GetReward/tem1/Get").GetComponent<Button>().interactable = false;
        //    }
        //    else
        //    {
        //        this.transform.FindChild("reward/has").gameObject.SetActive(true);
        //        //this.transform.FindChild("GetReward/tem1/Get").GetComponent<Button>().interactable = true;
        //    }

        //}

        public void refCount()
        {
            findCount.text = (A3_ActiveModel.getInstance().callenge_cnt - A3_ActiveModel.getInstance().pvpCount + A3_ActiveModel.getInstance().buyCount) + "/" + A3_ActiveModel.getInstance().callenge_cnt;
            buyCount.text = A3_ActiveModel.getInstance().buyCount + "/ " + A3_ActiveModel.getInstance().buy_cnt;
        }
        void reffind()
        {
            if (A3_ActiveModel.getInstance().callenge_cnt - A3_ActiveModel.getInstance().pvpCount + A3_ActiveModel.getInstance().buyCount <= 0)
            {
                findid = 1;
                this.transform.FindChild("find_info/find/text1").gameObject.SetActive(false);
                this.transform.FindChild("find_info/find/text2").gameObject.SetActive(true);
            }
            else
            {
                this.transform.FindChild("find_info/find/text1").gameObject.SetActive(true);
                this.transform.FindChild("find_info/find/text2").gameObject.SetActive(false);
                findid = 0;
            }
        }
        public void refro_score()
        {
            if (A3_ActiveModel.getInstance().grade <= 0)
                return;
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + A3_ActiveModel.getInstance().grade);
            duanwei.text = "段位：" + Xml.getString("name");
            if (A3_ActiveModel.getInstance().grade < 10)
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + A3_ActiveModel.getInstance().grade);
            else
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + A3_ActiveModel.getInstance().grade);
            refStar(A3_ActiveModel.getInstance().score);
        }
    }
}
