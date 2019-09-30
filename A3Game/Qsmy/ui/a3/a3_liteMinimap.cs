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
using UnityEngine.EventSystems;

namespace MuGame
{
    //小地图及周边按钮UI改版,优化性能
    class a3_liteMinimap : FloatUi
    {
        //时时自动任务状态显示
        public  Dictionary<TaskType, GameObject> TaskType_objs = new Dictionary<TaskType, GameObject>();//所有任务image
        #region 是否在自动做任务
        //范围保护（停止自动）
                    //1：idle状态超过4秒(有些和npc站着聊天会超过)；  
                    //2：玩家死亡；
        //范围保护（开始自动）
                    //1：传送和移动属于任务传送时；
        /*false条件
        1.触发新手教程、过场动画(ok);
        2.玩家点其他界面触发传送,移动;(ok)
        3.战斗的时候取消自动战斗;(ok)
        4.移动过程中自动战斗状态;(ok)
        5.玩家手动点摇杆(ok);
        6.主线任务要求断开(idle状态超过4秒)(ok)
        7.主线任务的某些任务类型(type=4,)
        8.战斗过程点摇杆(ok)
        9.npc对话手动点击关闭(idle状态超过4秒)(ok)
        */
        #endregion
        private bool _ZidongTask = false;
        public bool ZidongTask
        {
            set
            {
                _ZidongTask = value;
                if (_ZidongTask)
                {
                    debug.Log("开始自动任务");
                }
                else
                {
                    debug.Log("停止自动任务");
                    ShowOrHideZidong(false);
                }
            }
            get
            {
                return _ZidongTask;
            }
        }
        public TaskType ZidongTaskType;//当前自动任务的type



        public static a3_liteMinimap instance;

        public GameObject noNet;
        public GameObject goUpwardArrow, goDownwardArrow;
        public ScrollRect scrlrectTaskPanel;

        public Transform batt;

        public GameObject net1;
        public GameObject net2;
        public GameObject net3;
        public GameObject chongDian;
        public GameObject dianLiang;

        public Transform transTask;
        private Animator taskAnim;
        private BaseButton btnTask;
        private BaseButton btnTask_close;

        public Text time1;
        public Text net;
        string strPos = "({0},{1})";
        public Text ping;

        float dianLiangNew;
        float notic_v;
        int notic_i;
        Transform taskPanel;
        Transform teamPanel;
        bool notice_w;
        // public BaseButton BtnEnterLottery => btnEnterLottery;
        //BaseButton btnEnterLottery;
        //BaseButton btnEnterElite;
        //BaseButton btnFirstRecharge;
        //BaseButton btnActive;
        //BaseButton btnMonthCard;
        //Animator aninToggleButtons;
        //Transform hideBtns;
        //Toggle togglePlus;
        //BaseButton btnCseth;
        //BaseButton ranking;
        //BaseButton activeDegree;
        bool isTaskBtnShow = false;

        TabControl tabCtrl1;
        int oldtab;

        GameObject equip_no;

        Text accentExp_text;
        Image accentExp_Image;
        Image Godicon;
        Text YGname;
        public GameObject taskinfo;
        public TaskData task_id;
        // private Dictionary<int, FunctionItem> dItem = new Dictionary<int, FunctionItem>();
        GameObject fun_open;
        Text fun_des;
        Image fun_icon;
        public int fun_i = 1;
        public int func_id;
        public int grade;
        public int level;
        public int currentTopShowSiblingIndex = 0;
        public GameObject godlight;
        public bool notice_ison;
        GameObject CangBaoTu;
        public int notice_i;
        public override void init()
        {
            inText();
            alain();
            time1 = getComponentByPath<Text>("mobile/time");
            time1.text = "";
            net = getComponentByPath<Text>("mobile/net/wifi or 4G");
            net.text = "";
            noNet = getGameObjectByPath("mobile/net/no_net");
            noNet.SetActive(false);
            batt = getComponentByPath<Transform>("mobile/battry/battry1");
            net1 = getGameObjectByPath("mobile/net/net_1");
            net2 = getGameObjectByPath("mobile/net/net_2");
            net3 = getGameObjectByPath("mobile/net/net_3");
            ping = getComponentByPath<Text>("mobile/Text");
            dianLiang = getGameObjectByPath("mobile/battry");
            chongDian = getGameObjectByPath("mobile/battry/chongdian");
            net1.SetActive(false);
            net2.SetActive(true);
            net3.SetActive(false);
            fun_open = transform.FindChild("fun_open").gameObject;
            fun_des = fun_open.transform.FindChild("fun_des").GetComponent<Text>();
            fun_icon = fun_open.transform.FindChild("fun_icon").GetComponent<Image>();
            fun_open.gameObject.SetActive(true);
            notice_model.getInstance().xml_time();
            CangBaoTu = transform.FindChild("notice").gameObject;
            CangBaoTu.SetActive(false);
            new BaseButton(CangBaoTu.transform.FindChild("bg")).onClick = join_tips;

            //godlight = transform.FindChild("normal/hidBtns/canclosebtn/cont/btnCseth/fire").gameObject;
            //godlight.SetActive(false);


            taskinfo = transform.FindChild("taskinfo").gameObject;

            chongDian.SetActive(false);
            dianLiang.SetActive(false);

            //hideBtns = transform.FindChild("normal/hidBtns");
            //btnActive = new BaseButton(transform.FindChild("normal/hidBtns/btnActive"));
            //btnActive.onClick = onActive;

            //new BaseButton(transform.FindChild("normal/hidBtns/huoyue")).onClick = (GameObject go) =>
            //{
            //    a3_activeDegreeProxy.getInstance()?.SendGetPoint(1);
            //    InterfaceMgr.getInstance().open(InterfaceMgr.A3_ACTIVEDEGREE);

            //};

            //btnFirstRecharge = new BaseButton(transform.FindChild("normal/hidBtns/canclosebtn/cont/btnFirstRecharge"));
            //btnFirstRecharge.onClick = onBtnFirstRechargeClick;

            //BaseButton btnShop = new BaseButton(transform.FindChild(("normal/hidBtns/btnShop")));
            //btnShop.onClick = onBtnShopClick;

            accentExp_text = transform.FindChild("taskinfo/bar/jindu").gameObject.GetComponent<Text>();
            accentExp_Image = transform.FindChild("taskinfo/bar/bar").gameObject.GetComponent<Image>();
            Godicon = transform.FindChild("taskinfo/bar/icon/icon").gameObject.GetComponent<Image>();
            YGname = transform.FindChild("taskinfo/bar/name").gameObject.GetComponent<Text>();

            new BaseButton(this.transform.FindChild("taskinfo/bar/open")).onClick = onYGfb;


            equip_no = this.transform.FindChild("equip_no").gameObject;
            // EventTriggerListener.Get(equip_no).onEnter = OnEquipEnter;
            // EventTriggerListener.Get(equip_no).onExit = OnEquipEixt;
            new BaseButton(equip_no.transform).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ROLE);
                a3_role.ForceIndex = 1;
            };
            new BaseButton(getTransformByPath("goonDart")).onClick = (GameObject go) =>
              {
                  flytxt.instance.fly(ContMgr.getCont("a3_liteMinimap_txt"));
                  a3_dartproxy.getInstance().gotoDart = true;
                  getGameObjectByPath("goonDart").SetActive(false);
              };
            equip_no.SetActive(false);

            instance = this;
            refreshMapname();

            //任务窗口
            transTask = this.getTransformByPath("taskinfo");
            btnTask = new BaseButton(transTask.FindChild("title/btnshow"));
            btnTask.onClick = OnTaskClickShow;
            btnTask_close = new BaseButton(transTask.FindChild("title/btnshow_close"));
            btnTask_close.onClick = OnTaskClickClose;
            taskAnim = transTask.GetComponent<Animator>();

            //Transform btnCon = transTask.FindChild("skin/con");

            transTask.FindChild("skin/view").GetComponent<ScrollRect>().onValueChanged.AddListener((any) => CheckArrow());
            CheckLock();
            CheckLock4Screamingbox();
            CheckFirstRecharge();

            taskPanel = transform.FindChild("taskinfo/skin/view/con");
            teamPanel = transform.FindChild("taskinfo/skin/team");
            scrlrectTaskPanel = transform.FindChild("taskinfo/skin/view").GetComponent<ScrollRect>();
            goUpwardArrow = transform.FindChild("taskinfo/skin/view/Head").gameObject;
            goDownwardArrow = transform.FindChild("taskinfo/skin/view/Tail").gameObject;
            isTaskBtnShow = false;
            OnTaskClickShow(null);
            //任务和组队信息切换
            oldtab = 1;
            tabCtrl1 = new TabControl();
            tabCtrl1.onClickHanle = onTab1;
            tabCtrl1.create(this.getGameObjectByPath("taskinfo/title/panelTab1"), this.gameObject, 0, 0, true);
            showActiveIcon(GeneralProxy.getInstance().active_open, GeneralProxy.getInstance().active_left_tm);

            InvokeRepeating("setTextPos", 0, 3f);
            if (Application.platform == RuntimePlatform.Android)
            {
                InvokeRepeating("BatteryValue", 0, 8f);
            }
            InvokeRepeating("notice_show", 0, 1f);


            new BaseButton(getTransformByPath("notice/close")).onClick = (GameObject go) => { getGameObjectByPath("notice").SetActive(false); };
        }

        void inText()
        {
            this.transform.FindChild("notice/fun_des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_liteMinimap_1");//点击此处参加活动
            this.transform.FindChild("taskinfo/skin/team/createam/go/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_liteMinimap_2");//创建队伍
            this.transform.FindChild("taskinfo/skin/team/createam/speedteam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_liteMinimap_3");//快速组队

        }
        private void join_tips(GameObject obj)
        {
            if (MapModel.getInstance().curLevelId > 0)
            {
                flytxt.instance.fly(ContMgr.getCont("infb_null"));
                return;
            }
            switch (notice_i)
            {
                case 1:
                    {

                        ArrayList dl = new ArrayList();
                        dl.Add("findbtu");
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                    }
                    break;
                case 2:
                    {
                        ArrayList d2 = new ArrayList();
                        d2.Add("sports_jjc");
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS, d2);

                    }
                    break;
                case 3:
                    {
                        ArrayList d3 = new ArrayList();
                        d3.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON, d3);
                    }
                    break;
                case 4:
                    {
                        ArrayList d4 = new ArrayList();
                        d4.Add("forchest");
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, d4);

                    }
                    break;
                case 5:
                    {

                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE_GODLIGHT);
                    }

                    break;

            }
            if (CangBaoTu.activeSelf)
                CangBaoTu.SetActive(false);
        }

        private void CheckArrow()
        {
            if (taskPanel.childCount > 3)
            {
                if (taskPanel.GetChild(0).transform.position.y <= goUpwardArrow.transform.position.y)
                {
                    goUpwardArrow.SetActive(false);
                    goDownwardArrow.SetActive(true);
                }
                else if (taskPanel.GetChild(taskPanel.childCount - 1).transform.position.y >= goDownwardArrow.transform.position.y)
                {
                    goUpwardArrow.SetActive(true);
                    goDownwardArrow.SetActive(false);
                }
                else
                {
                    goUpwardArrow.SetActive(true);
                    goDownwardArrow.SetActive(true);
                }
            }
            else
            {
                goUpwardArrow.SetActive(false);
                goDownwardArrow.SetActive(false);
            }
        }

        private void OnItemChanged(GameEvent e)
        {
            int i = 0;
            Dictionary<int, TaskData> taskDic = A3_TaskModel.getInstance().GetDicTaskData();
            for (List<int> idx = new List<int>(taskDic.Keys); i < idx.Count; i++)
                if (taskDic[idx[i]].targetType == TaskTargetType.GET_ITEM_GIVEN)
                {
                    GameObject taskPage = dicTaskPage[idx[i]];
                    Text textDesc = taskPage.transform.FindChild("desc").GetComponent<Text>();
                    string desc = A3_TaskModel.getInstance().GetTaskDesc(idx[i], taskDic[idx[i]].isComplete) + GetCountStr(idx[i]);
                    textDesc.text = desc;
                }
        }

        public void function_open(int i)
        {
            int lv = (int)PlayerModel.getInstance().lvl;
            int level2 = (int)PlayerModel.getInstance().up_lvl;
            var xml = XMLMgr.instance.GetSXML("func_open.func_pre.order", "id==" + i);
            if (xml == null)
            {
                fun_open.gameObject.SetActive(false);
                return;
            }
            else
            {
                fun_des.text = xml.getString("des");
                fun_icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_func_open_" + xml.getInt("icon"));
                func_id = xml.getInt("func_id");
                if (func_id == 0)
                {
                    grade = xml.getInt("grade");
                    level = xml.getInt("level");
                    if ((lv * 100 + level2) >= (grade * 100 + level))
                    {
                        function_open(i + 1);
                    }
                }
            }

            if (FunctionItem.Instance != null)
            {
                if (FunctionOpenMgr.instance.dItem.Keys.Contains(func_id) && func_id != 0)
                {
                    if (FunctionOpenMgr.instance.dItem[func_id].opened == true)
                    {
                        function_open(i + 1);
                    }
                }
            }

        }

        public void showbtnIcon(bool show)
        {
            if (show)
            {
                fun_open.transform.localScale = Vector3.one;
            } else
                fun_open.transform.localScale = Vector3.zero;
        }


        bool showDrawAvaiable = false;


        float i = 0;

        public void Update()
        {

            // 自动任务范围保护（停止自动）
            if (ZidongTask)
            {
                if (SelfRole._inst.isDead)
                {
                    ZidongTask = false;
                }
            }
            if(ZidongTask&&(SelfRole.fsm.currentState == StateIdle.Instance|| SelfRole.fsm.currentState == StateSearch.Instance) && !npctasktalk.IsNpcTalk)
            {
                if (NewbieModel.getInstance().curItem == null || (NewbieModel.getInstance().curItem != null && !NewbieModel.getInstance().curItem.showing))
                {
                    if (!SelfRole.s_bInTransmit)
                    {
                        i += 1 * Time.deltaTime;

                        //可以精简成2秒（排除采集，指定读条,章节播放情况）
                        if (i >= 4f)
                        {
                            ZidongTask = false;
                            i = 0;
                        }
                    }
                    else
                        i = 0;

                }
                else
                    i = 0;
            }
            else
            {
                i = 0;

            }

            //print("iiiiiiiiiiiiiiiiiiiiiiiiii:" + i);
            //print("我当前的状态：" + SelfRole.fsm.currentState);
            //print("我的自动任务状态：" + ZidongTask);
            //if (NewbieModel.getInstance().curItem != null)
            //    print("新手指引：" + NewbieModel.getInstance().curItem.showing);
            //else
            //    print("新手指引为空");
            //if (muNetCleint.instance == null) return;
            //print("当前任务id：" + A3_TaskModel.getInstance().main_task_id);
            //清除小地图导航路径
            if (SelfRole._inst != null)
            {
                if (SelfRole._inst.m_curModel != null)
                {
                    if (SelfRole.fsm.currentState == StateIdle.Instance)
                    {
                        worldmap.Desmapimg();
                    }
                }
            }

            if (System.DateTime.Now.Minute > 9)
            {
                time1.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
            }
            else
            {
                time1.text = System.DateTime.Now.Hour + ":0" + System.DateTime.Now.Minute;
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                net.text = "";
                noNet.SetActive(true);
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                net.text = "4G";
                noNet.SetActive(false);
            }
            else
            {
                noNet.SetActive(false);
                net.text = "WIFI";
            }
            if (muNetCleint.instance.curServerPing < 60)
            {
                net3.SetActive(true);
                net2.SetActive(false);
                net1.SetActive(false);
            }
            else if (muNetCleint.instance.curServerPing < 200)
            {
                net3.SetActive(false);
                net2.SetActive(true);
                net1.SetActive(false);
            }
            else
            {
                net3.SetActive(false);
                net2.SetActive(false);
                net1.SetActive(true);
            }

            //if (!showDrawAvaiable && timesDraw > 0 && (timer -= Time.deltaTime) <= 0)
            //{
            //    BtnEnterLottery.transform.FindChild("fire")?.gameObject.SetActive(true);
            //    showDrawAvaiable = true;
            //}
            timers += Time.deltaTime;
            if (timers > 1.0f)
            {
                timers -= 1.0f;             
                ping.text = muNetCleint.instance.curServerPing.ToString();

            }

            if (TeamProxy.getInstance() != null)
            {
                if (TeamProxy.getInstance().MyTeamData != null)
                {
                    if (this.getGameObjectByPath("taskinfo/title/Team_Num").gameObject.activeInHierarchy == false)
                        this.getGameObjectByPath("taskinfo/title/Team_Num").SetActive(true);

                    int num = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
                    this.getGameObjectByPath("taskinfo/title/Team_Num/text").GetComponent<Text>().text = num.ToString();
                }
                else
                {
                    if (this.getGameObjectByPath("taskinfo/title/Team_Num").gameObject.activeInHierarchy == true)
                        this.getGameObjectByPath("taskinfo/title/Team_Num").SetActive(false);
                }
            }
            //if (transform.FindChild("taskinfo/bar").gameObject.activeInHierarchy == false)
            //{
            //    float x = transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition.x;
            //    transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -50);
            //}
            //else
            //{
            //    float x = transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition.x;
            //    transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -5.3f);
            //}
            if (tabCtrl1.getSeletedIndex()==1&& getGameObjectByPath("taskinfo/bar").activeSelf==true)
            {
                getGameObjectByPath("taskinfo/bar").SetActive(false);
            }
        }

        void notice_show()
        {

            if (notice_w == false)
            {

                for (int i = 0; i < notice_model.getInstance().notice.Count; i++)
                {
                    foreach (var v in notice_model.getInstance().notice[i].time.Keys)
                    {
                        notice_ison = (notice_model.getInstance().notice[i].zhuan * 100 + notice_model.getInstance().notice[i].level > PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl);
                      
                        int star_t = (int)(v * 60);
                        int end_t = (int)(notice_model.getInstance().notice[i].time[v] * 60);

                        if (System.DateTime.Now.Minute + System.DateTime.Now.Hour * 60 >= star_t && System.DateTime.Now.Minute + System.DateTime.Now.Hour * 60 <= end_t)
                        {

                            if (!CangBaoTu.activeSelf && !notice_ison)
                            {
                                CangBaoTu.SetActive(true);
                                notice_i = notice_model.getInstance().notice[i].id;
                                CangBaoTu.transform.FindChild("des").GetComponent<Text>().text = notice_model.getInstance().notice[i].des;
                                Image icon = CangBaoTu.transform.FindChild("icon").GetComponent<Image>();
                                icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_notice_" + notice_model.getInstance().notice[i].icon);
                                notice_tt = System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Hour * 3600 + notice_model.getInstance().notice[i].last;
                                notice_w = true;

                                notic_v = v;
                                notic_i = i;

                            }
                        }

                    }

                }
            }
            else
            {
                if (notice_model.getInstance().notice[notic_i].time[notic_v] != 0)
                    notice_model.getInstance().notice[notic_i].time[notic_v] = 0;
                if (notice_tt == System.DateTime.Now.Second + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Hour * 3600)
                {
                    if (CangBaoTu.activeSelf)
                        CangBaoTu.SetActive(false);
                    notice_w = false;
                }
            }
        }

        #region 电量获取相关
        int GetBattery()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                try
                {
                    string batte = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
                    return int.Parse(batte);
                }
                catch (Exception)
                {

                    Debug.LogError("获取不到电量");
                }
            }
            return 100;
        }


        public static int BatteryState()//电池的状态,1是正在充电
        {
            return CallStatic("GetBattery", "BatteryState");
        }

        public static int CallStatic(string className, string methodName, params object[] args)
        {
//#if UNITY_ANDROID  && !UNITY_EDITOR
            try
            {
                string CLASS_NAME = "com.example.asgardgame.androidnative.GetBattery";
                AndroidJavaObject bridge = new AndroidJavaObject(CLASS_NAME);
                int value = bridge.CallStatic<int>(methodName, args);
                return value;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
//#endif
            return -1;
        }
        public static int BatteryLevel()
        {
            return CallStatic("GetBattery", "BatteryLevel");
        }
        void BatteryValue()
        {


            dianLiangNew = batt.localScale.x;
            //batt.localScale = new Vector3((float)GetBattery() / 100, 1, 0);
            
            int battery_level = BatteryLevel();
            if (battery_level != -1)
            {
                batt.localScale = new Vector3((float)battery_level / 100, 1, 0);
                dianLiang.SetActive(true);
            }
            else
            {
                dianLiang.SetActive(false);
            }
            if (BatteryState() == 1)
            {
                if (!chongDian.activeSelf)
                    chongDian.SetActive(true);
            }
            else
            {
                if (chongDian.activeSelf)
                    chongDian.SetActive(false);
            }
        }
        #endregion
     

        public override void onShowed()
        {
            function_open(fun_i);//功能开启预告

            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnSubmitTask);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_NEW_TASK, OnAddNewTask);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_DISSOLVETEAM, onLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, onCreatTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, onBeInvite);
            
            //ResetLvLProxy.getInstance().addEventListener(ResetLvLProxy.EVENT_SHOWRESETLVL, onShowResetLvL);
            welfareProxy.getInstance().addEventListener(welfareProxy.SHOWFIRSTRECHARGE, onShowFirstRecharge);
            PlayerModel.getInstance().addEventListener(PlayerModel.ON_ATTR_CHANGE, refreshEquip);
            ResetLvLProxy.getInstance().resetLvL();
            EquipProxy.getInstance().addEventListener(EquipProxy.ONEQUIP, refreshEquip);
            LotteryProxy.getInstance().addEventListener(LotteryProxy.LOADLOTTERY, ShowFreeDrawAvaible);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.ON_REACH_ACHIEVEMENT, showAchi);
            A3_TaskModel.getInstance().addEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnTopShowSiblingIndexSub);
            TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, CheckLock);
            PlayerInfoProxy.getInstance().addEventListener(PlayerInfoProxy.EVENT_SELF_ON_LV_CHANGE, CheckLock);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.SIGNorREPAIR, singorrepair);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.SIGNINFO, refreshSign);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, OnItemChanged);
            a3_dartproxy.getInstance().addEventListener(a3_dartproxy.DARTHPNOW, dartHP);
            OnTaskInfoChange();
            OnActiveInfoChange();
            refreshEquip(null);
            refreshYGexp();
            petRenew();
        }

        #region 显示获得的成就信息
        long nowtime = 0;
        List<int> achi = new List<int>();
        void showAchi(GameEvent e)
        {
            long gaptime = NetClient.instance.CurServerTimeStampMS - nowtime;//收到事件的间隔时间
            if (e.data.ContainsKey("changed"))
            {

                List<SXML> xml = new List<SXML>();
                nowtime = NetClient.instance.CurServerTimeStampMS;
                if (gaptime < 100 || gaptime == NetClient.instance.CurServerTimeStampMS)
                {
                    achi.Add(e.data["changed"][0]["id"]);
                    achi.Sort();
                    StartCoroutine(wait(xml, nowtime, achi));
                }
                else
                {
                    GameObject go = Instantiate(getGameObjectByPath("achievement"));
                    go.transform.SetParent(getTransformByPath("achi"));
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    achi.Clear();
                    xml = XMLMgr.instance.GetSXMLList("achievement.achievement", "achievement_id==" + e.data["changed"][0]["id"]);
                    go.transform.FindChild("Text").GetComponent<Text>().text = xml[0].getString("name");
                    go.transform.FindChild("achipnt/Text").GetComponent<Text>().text = xml[0].getInt("point").ToString();
                    go.SetActive(true);
                    go.name = xml[0].getString("name");
                    Destroy(go, 1f);
                }
            }
        }
        void petRenew()
        {
            if (A3_PetModel.showrenew)
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_PET_RENEW);
                a3_pet_renew.instance?.transform.SetAsLastSibling();
            }
        }
        IEnumerator wait(List<SXML> xml, long nowtime, List<int> achis)
        {
            for (int i = 0; i < achis.Count; i++)
            {
                GameObject go = Instantiate(getGameObjectByPath("achievement"));
                go.transform.SetParent(getTransformByPath("achi"));
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(0, 0, 0);
                xml = XMLMgr.instance.GetSXMLList("achievement.achievement", "achievement_id==" + achis[i]);
                if (xml.Count > 0)
                {
                    go.transform.FindChild("Text").GetComponent<Text>().text = xml[0].getString("name");
                    go.transform.FindChild("achipnt/Text").GetComponent<Text>().text = xml[0].getInt("point").ToString();
                    go.name = xml[0].getString("name");
                    go.SetActive(true);
                }
                yield return new WaitForSeconds(1);
                Destroy(go.gameObject, 0.5f);
            }
            achis.Clear();

        }

        #endregion
        void dartHP(GameEvent e)
        {
            Variant list = SvrMapConfig.instance.getSingleMapConf(e.data["map_id"]);
            getTransformByPath("goonDart/map_name").GetComponent<Text>().text = list["map_name"];
            getTransformByPath("goonDart/dartHP").GetComponent<Text>().text = ContMgr.getCont("a3_liteMinimap_xl") + e.data["hp_per"] + "%";
        }
        void singorrepair(GameEvent e)
        {
            //if (e.data["daysign"] == DateTime.Now.Day)
            //{
            //    getGameObjectByPath("normal/hidBtns/btnMonthCard/fire").SetActive(false);
            //}
        }
        void refreshSign(GameEvent e)
        {
            //int len = e.data["qd_days"].Length;
            //if (len > 0 && e.data["qd_days"][len - 1] == DateTime.Now.Day)
            //    getGameObjectByPath("normal/hidBtns/btnMonthCard/fire").SetActive(false);
            //else
            //    getGameObjectByPath("normal/hidBtns/btnMonthCard/fire").SetActive(true);
        }
        private void CheckLock(GameEvent e) => CheckLock();
        int timesDraw = 0;
        float timer = 0;
        private void ShowFreeDrawAvaible(GameEvent e)
        {
            //if (e.data.ContainsKey("left_times"))
            //{
            //    timesDraw = e.data["left_times"]._int;
            //}
            //if (e.data.ContainsKey("left_tm"))//剩余时间
            //{
            //    timer = e.data["left_tm"]._float;
            //}
            //showDrawAvaiable = timesDraw > 0 && timer <= 0;
            //BtnEnterLottery.transform.FindChild("fire")?.gameObject.SetActive(showDrawAvaiable);
        }
       
        public void refreshMapname()
        {
            //if (GRMap.curSvrConf == null)
            //    return;
            //if (GRMap.curSvrConf.ContainsKey("pk"))
            //{
            //    Color c = new Color(1.0f, 1.0f, 1.0f);
            //    switch (GRMap.curSvrConf["pk"]._int)
            //    {
            //        case 0:
            //            c = new Color(0.4f, 1.0f, 1.0f);
            //            break;
            //        case 1:
            //            c = new Color(1.0f, 1.0f, 0.01f);
            //            break;
            //        case 2:
            //            c = new Color(0.97f, 0.05f, 0.05f);
            //            break;
            //    }

            //    txtMapName.color = c;
            //}
            //txtMapName.text = GRMap.curSvrConf.ContainsKey("map_name") ? GRMap.curSvrConf["map_name"]._str : "--";

            //string skinid = GRMap.curSvrConf.ContainsKey("tp") ? GRMap.curSvrConf["tp"]._str : "0";

        }
        //切地图有问题
        public void refreshYGexp()
        {
            if (a3_ygyiwuModel.getInstance().nowGod_id < 0)
            {
                return;
            }
            debug.Log("这里" + a3_ygyiwuModel.getInstance().nowGod_id);
            if (a3_ygyiwuModel.getInstance().nowGod_id > 9)
            {
                transform.FindChild("taskinfo/bar").gameObject.SetActive(false);
                transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition = transform.FindChild("taskinfo/pos0").GetComponent<RectTransform>().anchoredPosition;
                return;
            }
            else
            {
                transform.FindChild("taskinfo/bar").gameObject.SetActive(true);
                transform.FindChild("taskinfo/skin/view").GetComponent<RectTransform>().anchoredPosition = transform.FindChild("taskinfo/pos1").GetComponent<RectTransform>().anchoredPosition;
            }
            YGname.text = a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).name;
            int lastexp = a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).needexp;
            accentExp_Image.fillAmount = (float)PlayerModel.getInstance().accent_exp / (float)lastexp;
            Godicon.sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_smallicon_" + a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).iconid);

            if (PlayerModel.getInstance().accent_exp >= lastexp)
            {
                accentExp_text.text = ContMgr.getCont("a3_liteMinimap_tzsw");
            }
            else
            {
                float val = ((float)PlayerModel.getInstance().accent_exp / (float)lastexp);
                accentExp_text.text = val * 100 + "%";
            }
        }

        void onYGfb(GameObject go)
        {
            if (PlayerModel.getInstance().accent_exp >= a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).needexp)
            {
                MsgBoxMgr.getInstance().showTask_fb_confirm(a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).fbBox_title, a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).fbBox_dec, true, a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).need_zdl, () => toYGfb());
            }
            else
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_YGYIWU);
            }
        }

        void onCreatTeam(GameEvent e)
        {
            getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
            ArrayList array = new ArrayList();
            array.Add(teamPanel);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            if (select == 0)
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_CURRENTTEAMINFO);
            tabCtrl1.setSelectedIndex(1);
        }
        void onBeInvite(GameEvent e)
        {
            getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
            ArrayList array = new ArrayList();
            array.Add(teamPanel);
            //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            //if (select == 0)
            //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_CURRENTTEAMINFO);
            tabCtrl1.setSelectedIndex(1);
        }

        void toYGfb()
        {
            debug.Log("Enter");
            Variant sendData = new Variant();
            sendData["mapid"] = 3334;
            sendData["npcid"] = 0;
            sendData["ltpid"] = a3_ygyiwuModel.getInstance().nowGodFB_id;
            sendData["diff_lvl"] = 1;
            LevelProxy.getInstance().sendCreate_lvl(sendData);
        }

        private void setTextPos()
        {
            if (SelfRole._inst == null || SelfRole._inst.m_curModel == null) return;

            //if (txtPos == null || SelfRole._inst.m_curModel == null) return;       
            string txt = string.Format(strPos, (int)SelfRole._inst.m_curModel.position.x, (int)SelfRole._inst.m_curModel.position.z);
            //txtPos.text = txt;
            InterfaceMgr.doCommandByLua("a1_high_fightgame.setTextMapPos", "ui/interfaces/high/a1_high_fightgame", txt);
        }

        public float anchoredPositionY = 0f;
        public void refreshByUIState()
        {
            if (MapModel.getInstance().curLevelId > 0)
            {
                // transform.FindChild("normal/hidBtns/canclosebtn/cont/btnFirstRecharge").gameObject.SetActive(false);
                if ((GameRoomMgr.getInstance().curRoom is PlotRoom))
                    transform.FindChild("taskinfo/bar").gameObject.SetActive(false);
                if (!(GameRoomMgr.getInstance().curRoom is PlotRoom))
                    transform.FindChild("taskinfo").gameObject.SetActive(false);

                // transform.FindChild("normal/hidBtns/btnShop").gameObject.SetActive(false);
                transform.FindChild("taskinfo/bar").gameObject.SetActive(false);
                float x = transform.FindChild("taskinfo/skin").GetComponent<RectTransform>().anchoredPosition.x;
                //transform.FindChild("taskinfo/skin").GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 48);
                anchoredPositionY = transform.FindChild( "taskinfo/skin/view" ).GetComponent<RectTransform>().anchoredPosition.y;
                transform.FindChild( "taskinfo/skin/view" ).GetComponent<RectTransform>().anchoredPosition = new Vector2( x , anchoredPositionY+48 );



                //hideBtns.gameObject.SetActive(false);
                // togglePlus.gameObject.SetActive(false);
            }
            else
            {
                // transform.FindChild("normal/hidBtns/canclosebtn/cont/btnFirstRecharge").gameObject.SetActive(true);
                transform.FindChild("taskinfo").gameObject.SetActive(true);
                //transform.FindChild("normal/hidBtns/btnShop").gameObject.SetActive(true);
                if (transform.FindChild("taskinfo/title/panelTab1/btn_equiped").GetComponent<Button>().interactable == true && a3_ygyiwuModel.getInstance().nowGod_id <= 9)
                {
                    transform.FindChild("taskinfo/bar").gameObject.SetActive(true);
                }
                float x = transform.FindChild("taskinfo/skin").GetComponent<RectTransform>().anchoredPosition.x;
                //transform.FindChild("taskinfo/skin").GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
                transform.FindChild( "taskinfo/skin/view" ).GetComponent<RectTransform>().anchoredPosition = new Vector2( x , anchoredPositionY );
                // hideBtns.gameObject.SetActive(true);
                // togglePlus.gameObject.SetActive(true);

                // onTogglePlusClick(false);
                isTaskBtnShow = false;
                OnTaskClickShow(null);
                refreshYGexp();
            }
        }

        public void CheckLock()
        {

            //btnActive.gameObject.GetComponent<Image>().enabled = false;
            //goFB.gameObject.GetComponent<Image>().enabled = false;
            //btnEnterElite.transform.GetComponent<Image>().enabled = FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS);
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.ACTIVITES))
            //{
            //    OpenActive();
            //}
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.COUNTERPART))
            //{
            //    OpenFB();
            //}
        }
        public void OpenActive()
        {
            //btnActive.gameObject.GetComponent<Image>().enabled = true;
        }

        public void OpenFB()
        {
            //goFB.gameObject.GetComponent<Image>().enabled = true;
        }

        public void onWorldMap(GameObject go)
        {

            if (GRMap.curSvrConf.ContainsKey("maptype") && GRMap.curSvrConf["maptype"] > 0)
            {
                flytxt.instance.fly(ContMgr.getCont("worldmap_cantopen"));
                return;
            }

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.WORLD_MAP);
            //worldmap.instance.resfreshTeamPos();
        }


        public override void onClosed()
        {
            LotteryProxy.getInstance().removeEventListener(LotteryProxy.LOTTERYTIP_FREEDRAW, ShowFreeDrawAvaible);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnSubmitTask);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_NEW_TASK, OnAddNewTask);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_DISSOLVETEAM, onLeaveTeam);
            //ResetLvLProxy.getInstance().removeEventListener(ResetLvLProxy.EVENT_RESETLVL, onShowResetLvL);
            welfareProxy.getInstance().removeEventListener(welfareProxy.SHOWFIRSTRECHARGE, onShowFirstRecharge);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, CheckLock);
            PlayerInfoProxy.getInstance().removeEventListener(PlayerInfoProxy.EVENT_SELF_ON_LV_CHANGE, CheckLock);
            A3_TaskModel.getInstance().removeEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnTopShowSiblingIndexSub);
        }

       
        void onActive(GameObject go)//活动入口进入的默认界面
        {
            ArrayList arr = new ArrayList();
            arr.Add("pvp");
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, arr);
        }
        void onBtnFirstRechargeClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_FIRESTRECHARGEAWARD);
        }
        void onBtnShopClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3);
        }
      
        //public void onTogglePlusClick(bool b)
        //{
        //    togglePlus.isOn = b;
        //    aninToggleButtons.SetBool("onoff", b);
        //    if (b && (A3_BeStronger.Instance.ContentShown?.gameObject.activeSelf ?? false))
        //    {
        //        A3_BeStronger.Instance.ContentShown.gameObject.SetActive(false);
        //        A3_BeStronger.Instance.ClickPanel.gameObject.SetActive(false);
        //    }
        //}
        void onMoneyDraw(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
        }

        void onAutoPlay(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUTOPLAY2);
        }
        void onBtnMonthCardClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SIGN);
        }
        void onBtnCsethClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE_GODLIGHT);
        }
        void onranking(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RANKING);
        }

        bool isNB = false;
        public void changeCtr_NB() {
            isNB = true;
            tabCtrl1.setSelectedIndex(0);
        }
        #region 任务窗口

        private Dictionary<int, GameObject> dicTaskPage = new Dictionary<int, GameObject>();
        //刷新显示任务提示窗口
        public void InitTaskInfo() => OnTaskInfoChange();
        private void OnTaskInfoChange()
        {
            Transform conTaskInfo = transTask.FindChild("skin/view/con");
            GameObject pageTemp = transTask.FindChild("skin/pageTemp").gameObject;

            A3_TaskModel tkModel = A3_TaskModel.getInstance();
            Dictionary<int, TaskData> dicTemp = tkModel.GetDicTaskData();

            foreach (GameObject go in dicTaskPage.Values)
            {
                Destroy(go);
            }
            dicTaskPage.Clear();

            foreach (TaskData data in dicTemp.Values)
            {
                OnCreateTaskPage(data, pageTemp, conTaskInfo);
            }
        }

        public void OnActiveInfoChange() {
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.DEVIL_HUNTER))
            {
                return;
            }

            //当日可用次数
            int thisday_count = XMLMgr.instance.GetSXML("monsterhunter.daily_cnt").getInt("num"); ;
            //vip额外可购买次数
            int vip_canbuycount = A3_VipModel.getInstance().vip_exchange_num(24);
            //vip已经够买次数
            int vip_buycount = A3_ActiveModel.getInstance().vip_buy_count;
            int allcount = thisday_count + vip_buycount;
            if (A3_ActiveModel.getInstance().mwlr_charges < allcount)
            {
                if (activeDic.ContainsKey("mwlr"))
                {
                    activeDic["mwlr"].transform.FindChild("desc").GetComponent<Text>().text = ContMgr.getCont("fb_info_11", A3_ActiveModel.getInstance().mwlr_charges.ToString(), allcount.ToString());
                }
                else {
                    ArrayList d = new ArrayList();
                    d.Add("mwlr");
                    OnCreateActivePage("mwlr",InterfaceMgr .A3_ACTIVE , d);
                }
            }
            else {
                if (activeDic.ContainsKey("mwlr")) {
                    Destroy(activeDic["mwlr"]);
                    activeDic.Remove("mwlr");
                }
            }


        }

        public void RefreshTaskPage(int taskId)
        {
            if (dicTaskPage.ContainsKey(taskId))
                dicTaskPage[taskId].transform.SetSiblingIndex(currentTopShowSiblingIndex);
            RectTransform rectConTaskInfo = transTask.FindChild("skin/view/con").GetComponent<RectTransform>();
            rectConTaskInfo.anchoredPosition = new Vector2(rectConTaskInfo.anchoredPosition.x, 0);
            if (dicTaskPage.Count > 3)
                goDownwardArrow.SetActive(true);
            else
                goDownwardArrow.SetActive(false);
        }

        GameObject has_guide_show = null;
        //创建任务page
        private void OnCreateTaskPage(TaskData data, GameObject pageTemp, Transform container)
        {



            A3_TaskModel tkModel = A3_TaskModel.getInstance();

            GameObject clonePage = Instantiate(pageTemp) as GameObject;

            int curType = (int)data.taskT;
            Text textName = clonePage.transform.FindChild("name").GetComponent<Text>();
            Text countTxt = clonePage.transform.FindChild("name/count").GetComponent<Text>();
            Text textTitle = clonePage.transform.FindChild("name/title").GetComponent<Text>();
            Text textDesc = clonePage.transform.FindChild("desc").GetComponent<Text>();
            GameObject ig_guide = clonePage.transform.FindChild("guide_task_info").gameObject;

            TaskType_objs[data.taskT]=clonePage;
            textName.text = data.taskName;

            string desc = tkModel.GetTaskDesc(data.taskId, data.isComplete) + GetCountStr(data.taskId);
            textDesc.text = desc;
            if (data.guide)
            {
                has_guide_show = clonePage.transform.FindChild("guide_task_info").gameObject;
                if (!MsgBoxMgr.isShow_guide)
                    ig_guide.SetActive(true);
            }
            else
            {
                ig_guide.SetActive(false);
                has_guide_show = null;
            }

            textTitle.text = tkModel.GetTaskTypeStr(data.taskT);

            if (data.taskT == TaskType.MAIN)
            {
                if (container.FindChild("page_main") != null)
                {
                    Destroy(container.FindChild("page_main").gameObject);
                }
            }

            clonePage.transform.SetParent(container, false);

            clonePage.SetActive(true);
            clonePage.name = data.taskT.ToString();
            dicTaskPage[data.taskId] = clonePage;
            if (data.taskT == TaskType.MAIN)
            {
                clonePage.name = "page_main";
                clonePage.transform.SetAsFirstSibling();
            }
            else clonePage.transform.SetSiblingIndex(curType);
           

            if (data.taskT == TaskType.CLAN)
            {
                string str = textName.text;
               
                //countTxt.gameObject.SetActive(true);
               // countTxt.text = string.Format("({0}/10)", 1 + data.taskCount);
                textName.text = str + string.Format("({0}/10)", 1 + data.taskCount);
            }
            else if(data.taskT == TaskType.ENTRUST)
            {
                string str = textName.text;
                textName.text = str + string.Format("({0}/20)", 1 + data.taskCount);

               // countTxt.gameObject.SetActive(true);
                //countTxt.text = string.Format("({0}/20)", 1 + data.taskCount);
            }

           // 任务page他不是刷新数据，是删掉重新创建（任务自动显示要刷新）
            if (ZidongTask)
            {
                if (data.taskT == ZidongTaskType)
                {
                    clonePage.transform.FindChild("zidong").gameObject.SetActive(true);
                }
            }
            BaseButton btn = new BaseButton(clonePage.transform);
            btn_main = btn;
            int taskid = data.taskId;
            taskid_main = taskid;
            btn.onClick = delegate
            {
                taskname = clonePage.gameObject.name;
                a3_task_auto.instance.stopAuto = false;
                OnTaskInfoClick(taskid, btn.gameObject,true);             
                if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);


                // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
            };
            //if (data.topShowOnLiteminimap)
            //    SetTopShow(data.taskId);
            //if (A3_EntrustOpt.Instance != null)
            //    A3_EntrustOpt.Instance.LiteMinimapEntrustTaskTip = GetEntrustTaskPage()?.GetComponent<Text>();
            //if (A3_ClanTaskOpt.Instance != null)
            //    A3_ClanTaskOpt.Instance.LiteMinimapClanTaskTip = GetEntrustTaskPage()?.GetComponent<Text>();           
           // clonePage.transform.FindChild("zidong").gameObject.SetActive(false);
        }


        Dictionary<string, GameObject> activeDic = new Dictionary<string, GameObject>();
        private void OnCreateActivePage(string name , string uiName ,ArrayList arr) {

            if (activeDic.ContainsKey (name)) { return; }
            Transform conTaskInfo = transTask.FindChild("skin/view/con");
            GameObject pageTemp = transTask.FindChild("skin/pageTemp").gameObject;
            GameObject clonePage = Instantiate(pageTemp) as GameObject;
            clonePage.transform.FindChild("guide_task_info").gameObject.SetActive(false);
            Text textName = clonePage.transform.FindChild("name").GetComponent<Text>();
            Text textTitle = clonePage.transform.FindChild("name/title").GetComponent<Text>();
            Text textDesc = clonePage.transform.FindChild("desc").GetComponent<Text>();
            //当日可用次数
            int thisday_count = XMLMgr.instance.GetSXML("monsterhunter.daily_cnt").getInt("num"); ;
            //vip额外可购买次数
            int vip_canbuycount = A3_VipModel.getInstance().vip_exchange_num(24);
            //vip已经够买次数
            int vip_buycount = A3_ActiveModel.getInstance().vip_buy_count;
            int allcount = thisday_count + vip_buycount;
            textDesc.text = ContMgr.getCont("fb_info_11", A3_ActiveModel.getInstance().mwlr_charges.ToString (), allcount .ToString ()) ;
            clonePage.SetActive(true);
            clonePage.name = name;
            textTitle.text = ContMgr.getCont("a3_liteMinimap_active_1");
            switch (name)
            {
                case "mwlr":
                    textName.text = ContMgr .getCont ("uilayer_a3_active_3");
                    break;
            }
            clonePage.transform.SetParent(conTaskInfo, false);
            clonePage.transform.SetAsLastSibling();
            BaseButton btn = new BaseButton(clonePage.transform);
            btn.onClick = delegate
            {
                InterfaceMgr.getInstance().ui_async_open(uiName, arr);
                if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            };
            activeDic[name] = clonePage;
        }


        BaseButton btn_main;
        public int taskid_main;
        string taskname = string.Empty;
       // 章节连接(自动任务范围保护)
        public void TaskBtn(GameObject go ,bool isshowzodongimage)
        {
            taskid_main=A3_TaskModel.getInstance().main_task_id;
            a3_task_auto.instance.stopAuto = false;
            OnTaskInfoClick(taskid_main, btn_main.gameObject, false, isshowzodongimage);
            if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
        }
        public void SetTopShow(int taskId)
        {
            if (!dicTaskPage.ContainsKey(taskId))
                return;
            dicTaskPage[taskId].transform.SetSiblingIndex(currentTopShowSiblingIndex);
            currentTopShowSiblingIndex++;
        }

        public void setGuide()
        {
            if (MsgBoxMgr.isShow_guide)
            {
                if (has_guide_show != null)
                    has_guide_show.SetActive(false);
            }
            else
            {
                if (has_guide_show != null)
                    has_guide_show.SetActive(true);
            }
        }

        public Transform GetEntrustTaskPage()
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.ENTRUST)
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
            }
            return null;
        }

        public Transform GetClanTaskPage()
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.CLAN)
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
            }
            return null;
        }
        public Transform GetTaskPage(int taskId)
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
                if (taskIdOnLiteminimap[i] == taskId)
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
            return null;
        }
        public Transform GetEntrustTaskPage(out int taskId)
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            taskId = -1;
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.ENTRUST)
                {
                    taskId = taskIdOnLiteminimap[i];
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
                }
            }
            return null;
        }
        public Transform GetClanTaskPage(out int taskId)
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            taskId = -1;
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.CLAN)
                {
                    taskId = taskIdOnLiteminimap[i];
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
                }
            }
            return null;
        }
        public Transform GetDailyTaskPage(out int taskId)
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            taskId = -1;
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.DAILY)
                {
                    taskId = taskIdOnLiteminimap[i];
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
                }
            }
            return null;
        }
        public Transform GetDailyTaskPage()
        {
            List<int> taskIdOnLiteminimap = new List<int>(dicTaskPage.Keys);
            for (int i = 0; i < taskIdOnLiteminimap.Count; i++)
            {
                TaskType taskTypeOnLiteminimap = A3_TaskModel.getInstance().GetTaskDataById(taskIdOnLiteminimap[i])?.taskT ?? TaskType.NULL;
                if (taskTypeOnLiteminimap == TaskType.DAILY)
                    return dicTaskPage[taskIdOnLiteminimap[i]].transform;
            }
            return null;
        }
        //点击任务面板
        private void OnTaskInfoClick(int id,GameObject btn,bool zhuxian,bool isshowimage=false)
        {
          //  debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ:" + id+ A3_TaskModel.getInstance().main_task_id);


            A3_TaskModel tkModel = A3_TaskModel.getInstance();
            tkModel.curTask = tkModel.GetTaskDataById(id);
            task_id = tkModel.curTask;
            if (tkModel.curTask.taskT == TaskType.CLAN )
            {
                if(A3_LegionModel.getInstance().myLegion==null|| (A3_LegionModel.getInstance().myLegion != null&& A3_LegionModel.getInstance().myLegion.id == 0))
                {
                    ShowOrHideZidong(false);
                }
               if(A3_LegionModel.getInstance().myLegion != null && A3_LegionModel.getInstance().myLegion.id != 0)
                {
                    ZidongTask = true;
                    ZidongTaskType = TaskType.CLAN;
                    ShowOrHideZidong(true);
                }
            }
            else
            {
                ZidongTask = true;
                ZidongTaskType = tkModel.curTask.taskT;
                ShowOrHideZidong(true);               
            }
            if (tkModel.curTask.taskT == TaskType.ENTRUST)
            {

                //flytxt.instance.fly(ContMgr.getCont("zhixianrenwu"));
            }
            if (tkModel.curTask.taskT == TaskType.CLAN && A3_LegionModel.getInstance().myLegion != null && A3_LegionModel.getInstance().myLegion.id != 0)
            {
                a3_task_auto.instance.bDoClanTask = true;
                // flytxt.instance.fly(ContMgr.getCont("juntuanrenwu"));
            }
            if (tkModel.curTask.taskT == TaskType.MAIN)
            {
                // if (zhuxian)
                // flytxt.instance.fly(ContMgr.getCont("zhuxianrenwu"));
            }
            if (isshowimage)
                return;

            if (tkModel.curTask.taskT != TaskType.MAIN && tkModel.curTask.isComplete)
                switch (tkModel.curTask.completeWay)
                {
                    case 1:
                        OnAutoMove(tkModel.curTask);
                        break;
                    case 2:
                        OnOpenTaskWindow(tkModel.curTask.taskId);
                        break;
                    default:
                        break;
                }
            else
            {
                OnAutoMove(tkModel.curTask);
            }




        }
        //自动任务图片显示隐藏
        public void ShowOrHideZidong(bool show=false)
        {

            foreach (TaskType tp in TaskType_objs.Keys)
            {
                if (TaskType_objs[tp].gameObject != null && TaskType_objs[tp].transform.FindChild("zidong").gameObject != null)
                {                
                    if (TaskType_objs[tp].name == taskname)
                    {
                            TaskType_objs[tp].transform.FindChild("zidong").gameObject.SetActive(show?true:false);
                    }
                    else
                        TaskType_objs[tp].transform.FindChild("zidong").gameObject.SetActive(false);
                }
                 
            }

        }
       

        //打开任务面板
        private void OnOpenTaskWindow(int taskId)
        {
            ArrayList arr = new ArrayList();
            arr.Add(taskId);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK, arr);
        }

        //刷新任务信息
        private void OnRefreshTask(GameEvent e)
        {
            // function_open(fun_i);
            A3_TaskModel tkModel = A3_TaskModel.getInstance();

            Variant data = e.data;
            List<Variant> listData = data["change_task"]._arr;
            foreach (Variant v in listData)
            {
                int taskId = v["id"];
                TaskData taskData = tkModel.GetTaskDataById(taskId);

                GameObject taskPage = dicTaskPage[taskId];

                Text textDesc = taskPage.transform.FindChild("desc").GetComponent<Text>();

                string desc = tkModel.GetTaskDesc(taskId, taskData.isComplete) + GetCountStr(taskId);
                textDesc.text = desc;

                //if (taskData.isComplete && !SelfRole.s_bInTransmit && taskData.targetType != TaskTargetType.GETEXP /*TaskTargetType.KILL || taskData.taskT != TaskType.MAIN*/)
                //{
                //    SelfRole.fsm.Stop();
                //    a3_task_auto.instance.RunTask(taskData, checkNextStep: true);
                //}
                //if (taskData.isComplete && taskData.taskT == TaskType.MAIN)
                //{
                //    debug.Log("自动进行任务::" + taskData.taskId);
                //    OnAutoMove(taskData);
                //}
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.COUNTERPART))
            {
                OpenFB();
            }
        }

        //自动寻路
        private void OnAutoMove(TaskData taskData)
        {
            worldmap.Desmapimg();
            SelfRole.fsm.Stop();
            a3_task_auto.instance.RunTask(taskData, checkItem: true, clickAuto: true);
        }
        public void SetTaskInfoVisible(bool visible)
        {
            transform.FindChild("taskinfo").gameObject.SetActive(visible);
            if (visible)
                OnTaskClickShow(null);
            else
                OnTaskClickClose(null);
        }
        //播放动画
        private void OnTaskClickShow(GameObject go)
        {
            if (isTaskBtnShow) return;

            isTaskBtnShow = true;
            taskAnim.SetBool("onoff", true);
            btnTask_close.gameObject.SetActive(true);
            btnTask.gameObject.SetActive(false);
        }
        private void OnTaskClickClose(GameObject go)
        {
            if (!isTaskBtnShow) return;

            isTaskBtnShow = false;
            taskAnim.SetBool("onoff", false);
            btnTask_close.gameObject.SetActive(false);
            btnTask.gameObject.SetActive(true);
        }

        private string GetCountStr(int id)
        {
            A3_TaskModel tkModel = A3_TaskModel.getInstance();
            TaskData data = tkModel.GetTaskDataById(id);
            string str = string.Empty;
            if (!data.isComplete)
            {
                int count = data.taskRate;
                int maxCount = data.completion;
                if (data.targetType == TaskTargetType.GET_ITEM_GIVEN)
                {
                    int holdNum = a3_BagModel.getInstance().getItemNumByTpid((uint)data.completionAim);
                    int leftNum = maxCount - count;
                    if (leftNum > 0)
                        str = "(" + holdNum + "/" + leftNum + ")";
                    else
                        str = "(" + count + "/" + maxCount + ")";
                }
                else
                {
                    str = "(" + count + "/" + maxCount + ")";
                }

            }

            return str;
        }

        //添加新任务
        private void OnAddNewTask(GameEvent e)
        {
            Transform conTaskInfo = transTask.FindChild("skin/view/con");
            GameObject pageTemp = transTask.FindChild("skin/pageTemp").gameObject;
            List<TaskData> listTemp = A3_TaskModel.getInstance().listAddTask;
            foreach (TaskData data in listTemp)
            {
                int taskId;
                if (data.taskT == TaskType.DAILY)
                {
                    Destroy(GetDailyTaskPage(out taskId)?.gameObject);
                    if (taskId != -1)
                    {
                        dicTaskPage.Remove(taskId);
                        A3_TaskModel.getInstance().GetDicTaskData().Remove(taskId);
                    }
                }
                else if (data.taskT == TaskType.ENTRUST)
                {
                    Destroy(GetEntrustTaskPage(out taskId)?.gameObject);
                    int newTaskId = -1;
                    if (e.data.ContainsKey("emis") && e.data["emis"].ContainsKey("id"))
                        newTaskId = e.data["emis"]["id"];
                    if (taskId != -1)
                        if (taskId != newTaskId)
                        {
                            dicTaskPage.Remove(taskId);
                            A3_TaskModel.getInstance().GetDicTaskData().Remove(taskId);
                        }
                }
                else if (data.taskT == TaskType.CLAN)
                {
                    Destroy(GetClanTaskPage(out taskId)?.gameObject);
                    if (taskId != -1)
                    {
                        dicTaskPage.Remove(taskId);
                        A3_TaskModel.getInstance().GetDicTaskData().Remove(taskId);
                    }
                }
                else if (dicTaskPage.ContainsKey(data.taskId))
                    Destroy(dicTaskPage[data.taskId]);
                if (data.taskId > 0)
                    OnCreateTaskPage(data, pageTemp, conTaskInfo);

                //if (data.taskT == TaskType.MAIN)
                //{
                //    A3_TaskModel tkModel = A3_TaskModel.getInstance();
                //    tkModel.curTask = data;
                //    OnAutoMove(data);
                //}
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.COUNTERPART))
            {
                OpenFB();
            }
        }

        //移除任务
        private void OnSubmitTask(GameEvent e)
        {
            function_open(fun_i);
            int taskId = e.data["id"];
            if (dicTaskPage.ContainsKey(taskId))
            {
                Destroy(dicTaskPage[taskId]);
                dicTaskPage.Remove(taskId);
            }
        }
        private void OnTopShowSiblingIndexSub(GameEvent e)
        {
            if (currentTopShowSiblingIndex > 0)
                currentTopShowSiblingIndex--;
        }
        public void jf() { }
        public void SubmitTask(int taskId)
        {
            if (dicTaskPage.ContainsKey(taskId))
            {
                Destroy(dicTaskPage[taskId]);
                dicTaskPage.Remove(taskId);
            }
        }

        #endregion
        void onLeaveTeam(GameEvent e)
        {
           tabCtrl1.setSelectedIndex(1);//离开队伍跳到1
        }
        int select;
        int oldtab1 = 0;
        public void OnTeamSelected()
        {
            select = 1;
            oldtab1 = 0;
            ArrayList array = new ArrayList();
            array.Add(teamPanel);
            if (!a3_currentTeamPanel.leave)
            {
                if (TeamProxy.getInstance().MyTeamData != null && oldtab != 3)
                {
                    getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
                    ArrayList arrs = new ArrayList();
                    arrs.Add(2);
                    if (oldtab == 1)
                        oldtab = 2;
                    else
                    {
                        if (!InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_FB_TEAM))
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arrs);
                    }
                }
                else
                {
                    if (TeamProxy.getInstance().MyTeamData != null)
                    {
                        getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
                    }
                    else
                    {
                        getGameObjectByPath("taskinfo/skin/team/createam").SetActive(true);
                        new BaseButton(getTransformByPath("taskinfo/skin/team/createam/go")).onClick = (GameObject go) =>
                        {
                            TeamProxy.getInstance().SendCreateTeam(0);
                            getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
                        };
                        new BaseButton(getTransformByPath("taskinfo/skin/team/createam/speedteam")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPEEDTEAM);
                            // getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
                        };

                    }
                    ArrayList arrs = new ArrayList();
                    arrs.Add(2);
                    if (oldtab == 1)
                        oldtab = 2;
                    else
                    {
                        if (!InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_FB_TEAM))
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arrs);
                    }
                }
            }
            else
                getGameObjectByPath("taskinfo/skin/team/createam").SetActive(true);

            getGameObjectByPath("taskinfo/bar").SetActive(false);


            scrlrectTaskPanel.StopMovement();
            goUpwardArrow.SetActive(false);
            goDownwardArrow.SetActive(false);

        }
        void onTab1(TabControl t)//任务和队伍的小界面显示
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_CURRENTTEAMINFO);
            taskPanel.gameObject.SetActive(false);
            if (t.getSeletedIndex() == 0)
            {
                taskPanel.gameObject.SetActive(true);
                getGameObjectByPath("taskinfo/skin/team/createam").SetActive(false);
                if (oldtab1 == 1 && !isNB)
                {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK, null);
                    oldtab1 = 0;
                }
                else
                    oldtab1 = 1;
                select = 0;
                if (a3_ygyiwuModel.getInstance().nowGod_id <= 9)
                    getGameObjectByPath("taskinfo/bar").SetActive(true);
                else
                    getGameObjectByPath("taskinfo/bar").SetActive(false);
                oldtab = 1;
                if ((GameRoomMgr.getInstance().curRoom is PlotRoom))
                    transform.FindChild("taskinfo/bar").gameObject.SetActive(false);
                oldtab1 = 1;
                CheckArrow();
                if (isNB) isNB = false;
            }
            else
            {
                getGameObjectByPath("taskinfo/bar").SetActive(false);
                OnTeamSelected();
            }
        }

        void onShowFirstRecharge(GameEvent e)
        {
            bool show = e.data["show"];
            //btnFirstRecharge.gameObject.SetActive(show);
            //InterfaceMgr.doCommandByLua("a1_low_fightgame.openorclosefr", "ui/interfaces/low/a1_low_fightgame", show);
            A3_TaskProxy.getInstance().showFirst = show;
        }
        public  void onSuccessGetFirstRechargeAward(GameEvent e)
        {
            //btnFirstRecharge.gameObject.SetActive(false);
            bool b = false;
            if (a3_Recharge .isshow ) {
                a3_Recharge.isshow.refre_recharge();
            }
            InterfaceMgr.doCommandByLua("a1_low_fightgame.openorclosefr", "ui/interfaces/low/a1_low_fightgame", b);
            A3_TaskProxy.getInstance().showFirst = false;
        }
        public void CheckLock4Screamingbox()
        {
            //btnEnterLottery.gameObject.GetComponent<Image>().enabled = false;
            //if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SCREAMINGBOX))
            //{
            //    OpenMH();
            //}
        }
        public void OpenMH()
        {
            //btnEnterLottery.gameObject.GetComponent<Image>().enabled = true;
        }
        public void CheckFirstRecharge()
        {
            welfareProxy.getInstance().addEventListener(welfareProxy.SUCCESSGETFIRSTRECHARGEAWARD, onSuccessGetFirstRechargeAward);
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.selfWelfareInfo);
      
        }
        public void updateUICseth()
        {
            //if (MapModel.getInstance().curLevelId > 0)
            //{
            //    btnMonthCard.gameObject.SetActive(false);
            //}
            //else
            //{
            //    btnMonthCard.gameObject.SetActive(true);
            //}
        }

        public uint active_leftTm = 0;
        private int notice_tt;
        private float timers;

        public void showActiveIcon(bool open, uint time)
        {
            if (open)
            {
                active_leftTm = time;
               // btnCseth.gameObject.SetActive(true);
                CancelInvoke("activeTimeGo");
                InvokeRepeating("activeTimeGo", 0, 1);
                //godlight.SetActive(true);
            }
            //else
                //godlight.SetActive(false);

        }
        void activeTimeGo()
        {
            active_leftTm--;
            if (active_leftTm <= 0)
            {
                active_leftTm = 0;
                //btnCseth.gameObject.SetActive(false);
                CancelInvoke("activeTimeGo");
            }
        }

        public void refreshEquip(GameEvent e)
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
                equip_no.SetActive(true);
            }
            else
            {
                equip_no.SetActive(false);
            }
        }

        //void OnEquipEnter(GameObject go)
        //{
        //    go.transform.FindChild("text_bg").gameObject.SetActive(true);
        //}
        //void OnEquipEixt(GameObject go)
        //{
        //    go.transform.FindChild("text_bg").gameObject.SetActive(false);
        //}
    }
}
