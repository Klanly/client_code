using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

namespace MuGame
{
    class a3_task : Window
    {
        private Transform conTabBtn;

        private Transform conTask;
        public GameObject tabClan,tabEntrust;
        private GameObject transMain;
        private GameObject transDaily;
        private GameObject transBranch;
        private GameObject transEmpty;
        private GameObject transBranchEmpty;
        private GameObject transEntrust;
        private GameObject transClanEmpty;
        private GameObject transClanTask;
        private GameObject branchBtn;
        private GameObject dailyBtn;
        private Transform container;
        private BaseButton btnClose;
        private BranchMissionText bmis;
        private A3_TaskModel tkModel;
        private Dictionary<string, TaskData> bmisDic;
        private Dictionary<string, BranchMissionObj> bmisGoDic;
        public static a3_task instance;
        public ChapterInfos mainChapterInfo;
        private List<GameObject> listSubBranch;
        TabControl tc;
        public override void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("con_tab/btnTemp/Text").text = ContMgr.getCont("a3_task_0");
            getComponentByPath<Text>("con_tab/SubBranchBtn/Text").text = ContMgr.getCont("a3_task_0");
            getComponentByPath<Text>("con_task/mainTemp/0/state/Text").text = ContMgr.getCont("a3_task_1");
            getComponentByPath<Text>("con_task/mainTemp/0/title/Text").text = ContMgr.getCont("a3_task_2");
            getComponentByPath<Text>("con_task/mainTemp/0/slider/text2").text = ContMgr.getCont("a3_task_3");
            getComponentByPath<Text>("con_task/mainTemp/0/reward/state").text = ContMgr.getCont("a3_task_4");
            getComponentByPath<Text>("con_task/mainTemp/1/btn_move/Text").text = ContMgr.getCont("a3_task_5");
            getComponentByPath<Text>("con_task/mainTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_6");
            getComponentByPath<Text>("con_task/mainTemp/1/task/title").text = ContMgr.getCont("a3_task_7");
            getComponentByPath<Text>("con_task/mainTemp/1/reward/title").text = ContMgr.getCont("a3_task_8");
            getComponentByPath<Text>("con_task/mainTemp/1/title/Text").text = ContMgr.getCont("a3_task_9");
            getComponentByPath<Text>("con_task/dailyTemp/0/title/Text").text = ContMgr.getCont("a3_task_10");
            getComponentByPath<Text>("con_task/dailyTemp/0/btn_onekey/Text").text = ContMgr.getCont("a3_task_11");
            getComponentByPath<Text>("con_task/dailyTemp/0/reward/state").text = ContMgr.getCont("a3_task_12");
            getComponentByPath<Text>("con_task/dailyTemp/0/state/Text").text = ContMgr.getCont("a3_task_13");
            getComponentByPath<Text>("con_task/dailyTemp/1/btn_move/Text").text = ContMgr.getCont("a3_task_5");
            getComponentByPath<Text>("con_task/dailyTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_6");
            getComponentByPath<Text>("con_task/dailyTemp/1/task/title").text = ContMgr.getCont("a3_task_7");
            getComponentByPath<Text>("con_task/dailyTemp/1/reward/title").text = ContMgr.getCont("a3_task_8");
            getComponentByPath<Text>("con_task/dailyTemp/1/title/Text").text = ContMgr.getCont("a3_task_9");
            getComponentByPath<Text>("con_task/dailyTemp/1/star/title_1").text = ContMgr.getCont("a3_task_14");
            getComponentByPath<Text>("con_task/dailyTemp/1/star/btn_oneKey/Text").text = ContMgr.getCont("a3_task_15");
            getComponentByPath<Text>("con_task/dailyTemp/1/get_reward/1/Text").text = ContMgr.getCont("a3_task_16");
            getComponentByPath<Text>("con_task/dailyTemp/1/get_reward/2/Text").text = ContMgr.getCont("a3_task_17");
            getComponentByPath<Text>("con_task/branchTemp/0/title_bran/Text").text = ContMgr.getCont("a3_task_0");
            getComponentByPath<Text>("con_task/branchTemp/0/state/Text").text = ContMgr.getCont("a3_task_1");
            getComponentByPath<Text>("con_task/branchTemp/0/btnGo/Text").text = ContMgr.getCont("a3_task_5");
            getComponentByPath<Text>("con_task/branchTemp/0/desc/Text").text = ContMgr.getCont("a3_task_6");
            getComponentByPath<Text>("con_task/branchTemp/0/title/Text").text = ContMgr.getCont("a3_task_9");
            getComponentByPath<Text>("con_task/branchTemp/0/title_reward/Text").text = ContMgr.getCont("a3_task_18");
            getComponentByPath<Text>("con_task/entrustTemp/0/title/Text").text = ContMgr.getCont("a3_task_19");
            getComponentByPath<Text>("con_task/entrustTemp/0/reward/state").text = ContMgr.getCont("a3_task_20");
            getComponentByPath<Text>("con_task/entrustTemp/1/btn_move/Text").text = ContMgr.getCont("a3_task_5");
            getComponentByPath<Text>("con_task/entrustTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_6");
            getComponentByPath<Text>("con_task/entrustTemp/1/task/title").text = ContMgr.getCont("a3_task_7");
            getComponentByPath<Text>("con_task/entrustTemp/1/reward/title").text = ContMgr.getCont("a3_task_8");
            getComponentByPath<Text>("con_task/entrustTemp/1/title/Text").text = ContMgr.getCont("a3_task_9");
            getComponentByPath<Text>("con_task/emptyTemp/0/state/Text").text = ContMgr.getCont("a3_task_21");
            getComponentByPath<Text>("con_task/emptyTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyTemp/1/title/Text").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyBranchTemp/0/state/Text").text = ContMgr.getCont("a3_task_23");
            getComponentByPath<Text>("con_task/emptyBranchTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyBranchTemp/1/title/Text").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyentrustTemp/0/state/Text").text = ContMgr.getCont("a3_task_21");
            getComponentByPath<Text>("con_task/emptyentrustTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyentrustTemp/1/title/Text").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/clanTaskTemp/0/title/Text").text = ContMgr.getCont("a3_task_24");
            getComponentByPath<Text>("con_task/clanTaskTemp/0/reward/Text").text = ContMgr.getCont("a3_task_25");
            getComponentByPath<Text>("con_task/clanTaskTemp/1/btn_move/Text").text = ContMgr.getCont("a3_task_5");
            getComponentByPath<Text>("con_task/clanTaskTemp/1/task/title").text = ContMgr.getCont("a3_task_7");
            getComponentByPath<Text>("con_task/clanTaskTemp/1/reward/title").text = ContMgr.getCont("a3_task_8");
            getComponentByPath<Text>("con_task/emptyClanTemp/0/state/Text").text = ContMgr.getCont("a3_task_26");
            getComponentByPath<Text>("con_task/emptyClanTemp/1/task/Text_state").text = ContMgr.getCont("a3_task_22");
            getComponentByPath<Text>("con_task/emptyClanTemp/1/title/Text").text = ContMgr.getCont("a3_task_22");
            #endregion
            tkModel = A3_TaskModel.getInstance();

            conTabBtn = this.getTransformByPath("con_tab");

            conTask = this.getTransformByPath("con_task/container");
            transMain = this.getGameObjectByPath("con_task/mainTemp");
            transDaily = this.getGameObjectByPath("con_task/dailyTemp");
            transBranch = this.getGameObjectByPath("con_task/branchTemp");
            transEmpty = this.getGameObjectByPath("con_task/emptyTemp");
            transClanEmpty = this.getGameObjectByPath("con_task/emptyClanTemp");
            transBranchEmpty = this.getGameObjectByPath("con_task/emptyBranchTemp");
            transEntrust = this.getGameObjectByPath("con_task/entrustTemp");
            transClanTask = this.getGameObjectByPath("con_task/clanTaskTemp");
            btnClose = new BaseButton(this.getTransformByPath("btn_close"));
            btnClose.onClick = OnCloseClick;

            InitConTabBtn();

            CheckLock();

            listSubBranch = new List<GameObject>();
            bmisDic = new Dictionary<string, TaskData>();
            bmisGoDic = new Dictionary<string, BranchMissionObj>();
            instance = this;

            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_APPLYSUCCESSFUL, (e) => tabClan.SetActive(true));
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CREATE, (e) => tabClan.SetActive(true));
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_ACCEPTAINVITE, (e) => tabClan.SetActive(true));

           

        }

        //初始化按钮
        private void InitConTabBtn()
        {
            GameObject btnTemp = this.getGameObjectByPath("con_tab/btnTemp");
            container = this.getTransformByPath("con_tab/view/con");

            List<TaskType> listTaskType = new List<TaskType>() { TaskType.MAIN, TaskType.BRANCH, TaskType.DAILY, TaskType.ENTRUST, TaskType.CLAN };

            for (int i = 0; i < listTaskType.Count; i++)
            {
                GameObject clon = Instantiate(btnTemp) as GameObject;
                clon.gameObject.SetActive(true);
                Text txt = clon.transform.FindChild("Text").GetComponent<Text>();
                string str = string.Empty;
                switch (listTaskType[i])
                {
                    case TaskType.MAIN:
                        str = ContMgr.getCont("task_btn_name_1");
                        break;
                    case TaskType.DAILY:
                        str = ContMgr.getCont("task_btn_name_2");
                        dailyBtn = clon;
                        break;
                    case TaskType.BRANCH:
                        str = ContMgr.getCont("task_btn_name_3");
                        //clon.GetComponent<Button>().onClick.AddListener(ShowSubBranchBtn);
                        break;
                    case TaskType.ENTRUST:
                        str = ContMgr.getCont("task_btn_name_4");
                        tabEntrust = clon;
                        tabEntrust.SetActive(A3_TaskModel.getInstance().GetEntrustTask() != null);
                        break;
                    case TaskType.CLAN:                        
                        str = ContMgr.getCont("task_btn_name_5");
                        tabClan = clon;
                        if (A3_LegionModel.getInstance().myLegion.id == 0)
                            tabClan.SetActive(A3_TaskModel.getInstance().GetClanTask() != null);
                        break;
                    default:
                        break;
                }
                txt.text = str;
                clon.name = ((int)listTaskType[i]).ToString();

                //BaseButton btn = new BaseButton(clon.transform);
                // btn.onClick = OnTaskSwichClick;

                clon.transform.SetParent(container, false);
            }
            tc = new TabControl();
            tc.create(container.gameObject, this.gameObject);
            tc.onClickHanle = (TabControl tb) =>
            {
                var vi = tb.getSeletedIndex();
                OnShowCurTaskTable((TaskType)(vi + 1));
            };
        }
        private GameObject currentObj;
        private GameObject branchPage;
        private void ShowSubBranchBtn()
        {
            UnityEngine.Events.UnityAction<bool> action;
            Dictionary<int, TaskData> dicTask = A3_TaskModel.getInstance().GetDicTaskData();
            Dictionary<int, TaskData> dicBranchTask = new Dictionary<int, TaskData>();

            foreach (var task in dicTask)
                if (task.Value.taskT == TaskType.BRANCH)
                    dicBranchTask.Add(task.Key, task.Value);
            Transform tfLastBranchBtn = transform.FindChild("con_tab/view/con/2");
            if (dicBranchTask.Count == 0)
            {
                tfLastBranchBtn.gameObject.SetActive(false);
                return;
            }
            GameObject btnTemp = this.getGameObjectByPath("con_tab/SubBranchBtn");
            Transform container = this.getTransformByPath("con_tab/view/con");
            int i = 0;
            for (i = 0; i < listSubBranch.Count; i++) // 待优化 - 应在每完成一个支线后立即销毁
                DestroyImmediate(listSubBranch[i]);
            listSubBranch.Clear();
            List<int> branchTaskIdx = dicBranchTask.Keys.ToList();
            bmisDic.Clear();

            for (i = 0; i < branchTaskIdx.Count; i++)
            {
                GameObject subBranchBtn = Instantiate(btnTemp);
                subBranchBtn.transform.SetParent(container, false);
                subBranchBtn.transform.SetSiblingIndex(tfLastBranchBtn.GetSiblingIndex() + 1);
                tfLastBranchBtn = subBranchBtn.transform;
                TaskData task = dicBranchTask[branchTaskIdx[i]];
                subBranchBtn.transform.GetComponent<Toggle>().onValueChanged.AddListener(
                    action = delegate (bool isOn)
                    {
                        if (isOn)
                        {
                            BranchMissionObj bmisObj = new BranchMissionObj();
                            for (int j = 0; j < conTask.childCount; j++)
                                conTask.GetChild(j).gameObject.SetActive(false);
                            subBranchBtn.transform.GetChild(0).gameObject.SetActive(true);
                            subBranchBtn.transform.GetChild(1).gameObject.SetActive(false);
                            if (!a3_task.instance.bmisGoDic.ContainsKey(subBranchBtn.transform.name))
                            {
                                Destroy(conTask.FindChild("branchPage")?.gameObject);
                                GameObject go = OnCreateBanchPage(transBranch, bmisDic[subBranchBtn.transform.name]);
                                go.name = "branchPage";
                                bmisGoDic.Add(subBranchBtn.transform.name, bmisObj = new BranchMissionObj { panel = go, btnGo = go.transform.FindChild("0/btnGo").gameObject });
                                (currentObj = go).transform.SetParent(conTask);
                                bmisObj.btnGo?.GetComponent<Button>()?.onClick.AddListener(
                                    delegate ()
                                    {
                                        worldmap.Desmapimg();
                                        SelfRole.fsm.Stop();
                                        a3_task_auto.instance.RunTask(task, clickAuto: true);
                                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_TASK);
                                    }
                                );
                            }
                            else
                                (currentObj = (bmisObj = bmisGoDic[subBranchBtn.transform.name]).panel).SetActive(true);

                        }
                        else
                        {
                            subBranchBtn.transform.GetChild(0).gameObject.SetActive(false);
                            subBranchBtn.transform.GetChild(1).gameObject.SetActive(true);
                            if (a3_task.instance.bmisGoDic.ContainsKey(subBranchBtn.transform.name))
                            {
                                Destroy(bmisGoDic[subBranchBtn.transform.name].panel);
                                bmisGoDic.Remove(subBranchBtn.transform.name);
                            }
                        }
                    }
                );
                //subBranchBtn.transform.name = i.ToString();
                subBranchBtn.transform.name = branchTaskIdx[i].ToString();
                bmisDic.Add(subBranchBtn.transform.name, dicBranchTask[branchTaskIdx[i]]);
                subBranchBtn.SetActive(true);
                listSubBranch.Add(subBranchBtn);
                Text txt = subBranchBtn.transform.GetComponentInChildren<Text>();
                txt.text = dicBranchTask[branchTaskIdx[i]].taskName;
            }

            if (dailyBtn != null && dailyBtn.activeSelf)
                dailyBtn.transform.SetAsLastSibling();
            //第一个任务默认为选中状态
            if (!listSubBranch[0].transform.GetComponent<Toggle>().isOn)
            {
                listSubBranch[0].transform.GetComponent<Toggle>().isOn = true;
                bmisGoDic[listSubBranch[0].name].panel.SetActive(true);
                bmisDic[listSubBranch[0].name] = dicBranchTask[int.Parse(listSubBranch[0].name)];
            }
            //if (!bmisGoDic.ContainsKey("0"))
            //    bmisGoDic.Add("0", OnCreateBanchPage(transBranch, bmisDic["0"]));
            //else
            //    bmisGoDic["0"].SetActive(true);

        }
        public void CheckLock()
        {
            var dt = transform.FindChild("con_tab/view/con/" + ((int)TaskType.DAILY).ToString());
            if (dt)
            {
                dt.gameObject.SetActive(false);
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DAILY_TASK))
            {
                OpenDailyTask();
            }
        }
        public void OpenDailyTask()
        {
            var dt = transform.FindChild("con_tab/view/con/" + ((int)TaskType.DAILY).ToString());
            if (dt)
            {
                dt.gameObject.SetActive(true);
            }
        }

        public override void onShowed()
        {
            Toclose = false;
            base.onShowed();

            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnSubmitTask);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_NEW_TASK, OnAddNewTask);


            OnShowTaskPanel();
            setWin();
            GRMap.GAME_CAMERA.SetActive(false);
        }

        public static int openwin = 0;
        void setWin()
        {
            if (openwin == 0) return;
            tc.setSelectedIndex(tc.getIndexByName(((int)openwin).ToString()), true);
            openwin = 0;
        }

        bool Toclose = false;
        public override void onClosed()
        {
            base.onClosed();
            instance = null;

            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnSubmitTask);
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_NEW_TASK, OnAddNewTask);

            ResetTaskType();

            GRMap.GAME_CAMERA.SetActive(true);

            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
        }

        //添加新任务
        private void OnAddNewTask(GameEvent e)
        {
            List<TaskData> listTemp = tkModel.listAddTask;
            foreach (TaskData data in listTemp)
            {
                OnShowTaskPage(data);
            }

        }

        //刷新任务列表
        private void OnTaskRateRefresh(TaskData data)
        {
            if (dicTaskPage.ContainsKey(data.taskId))
            {
                GameObject page = dicTaskPage[data.taskId];

                switch (data.taskT)
                {
                    case TaskType.NULL:
                        break;
                    case TaskType.MAIN:
                        OnMainTaskDataChange(page, data);
                        break;
                    case TaskType.DAILY:
                        OnDailyTaskDataChange(page, data);
                        break;
                    case TaskType.BRANCH:
                        break;
                    case TaskType.ENTRUST:
                        onENTRUSTChange(page, data);break;
                    default:
                        break;
                }
            }
        }

        //移除任务
        private void OnSubmitTask(GameEvent e)
        {
            int taskId = e.data["id"];

            if (dicTaskPage.ContainsKey(taskId))
            {
                Destroy(dicTaskPage[taskId]);
                dicTaskPage.Remove(taskId);
            }
        }

        //初始化任务界面(关闭界面处理)
        private void ResetTaskType()
        {
            curTaskType = TaskType.NULL;

            DisposeAvatar();
            DisposeCamera();
        }

        private void OnRefreshTask(GameEvent e)
        {
            Variant data = e.data;
            List<Variant> listData = data["change_task"]._arr;
            foreach (Variant v in listData)
            {
                int taskId = v["id"];
                TaskData taskData = tkModel.GetTaskDataById(taskId);
                Transform conState = dicTaskPage[taskId].transform.GetChild(1);
                OnTaskRateRefresh(taskData);
            }
        }

        #region 选择显示界面
        //打开任务界面显示
        private void OnShowTaskPanel()
        {
            int showTaskId = 0;
            if (uiData != null)
            {
                showTaskId = (int)uiData[0];
            }

            TaskType selectType = TaskType.MAIN;
            if (showTaskId != 0)
            {
                selectType = tkModel.GetTaskDataById(showTaskId).taskT;//GetTaskTypeById(showTaskId);
            }

            tc.setSelectedIndex(tc.getIndexByName(((int)selectType).ToString()), true);
            //OnShowCurTaskTable(selectType);
        }

        //切换选项
        private void OnTaskSwichClick(GameObject go)
        {
            int index = int.Parse(go.name);

            this.OnShowCurTaskTable((TaskType)index);
        }

        //当前选着的任务类型
        private TaskType curTaskType;
        //选着显示任务类型
        void OnShowCurTaskTable(TaskType type)
        {
            if (type == curTaskType || type == TaskType.NULL)
                return;

            curTaskType = type;
            OnCurTaskTypeChange();

            Dictionary<int, TaskData> dicTaskData = new Dictionary<int, TaskData>();
            dicTaskData = tkModel.GetTaskDataByTaskType(type);
            ShowSubBranchBtn();
            if (type == TaskType.BRANCH)
            {
                ShowSubBranchBtn();
                if (bmisGoDic.ContainsKey("0"))
                    bmisGoDic["0"].panel.SetActive(true);
            }
            else
            {

                if (currentObj != null)
                    currentObj.SetActive(false);
                for (int i = 0; i < listSubBranch.Count; i++) // 待优化 - 应在每完成一个支线后立即销毁
                {
                    DestroyImmediate(listSubBranch[i]);
                }
                bmisGoDic.Clear();
                if (dicTaskData.Count > 0)
                {
                    foreach (TaskData data in dicTaskData.Values)
                    {
                        OnShowTaskPage(data);
                    }
                }
                else
                {
                    TaskData tempData = new TaskData();
                    tempData.taskId = 0;
                    tempData.taskT = TaskType.NULL;

                    OnShowTaskPage(tempData);
                }
            }
        }

        //任务的page
        private Dictionary<int, GameObject> dicTaskPage = new Dictionary<int, GameObject>();
        GameObject pageTemp;
        //显示任务内容
        private void OnShowTaskPage(TaskData data)
        {
            tkModel.curTask = data;
            int taskId = data.taskId;

            if (dicTaskPage.ContainsKey(taskId))
            {
                foreach (int key in dicTaskPage.Keys)
                {
                    bool b = key == taskId;
                    dicTaskPage[key].SetActive(b);
                    if (b)
                    {
                        OnTaskRateRefresh(data);
                    }
                }
            }
            else
            {
                foreach (int key in dicTaskPage.Keys)
                {
                    dicTaskPage[key].SetActive(false);
                }

                //创建任务的page

                TaskType taskT = data.taskT;

                switch (taskT)
                {
                    case TaskType.NULL:
                        if (curTaskType == TaskType.BRANCH)
                            pageTemp = OnCreateEmptyPage(transBranchEmpty, data);
                        else if(curTaskType == TaskType.CLAN)
                            pageTemp = OnCreateEmptyPage(transClanEmpty, data); 
                        else
                            pageTemp = OnCreateEmptyPage(transEmpty, data);
                        break;
                    case TaskType.MAIN:
                        pageTemp = OnCreateMainPage(transMain, data);
                        break;
                    case TaskType.DAILY:
                        pageTemp = OnCreateDailyPage(transDaily, data);
                        break;
                    case TaskType.ENTRUST:
                        pageTemp = OnCreateEntrustPage(transEntrust, data);
                        break;
                    case TaskType.CLAN:
                        pageTemp = OnCreateClanTaskPage(transClanTask, data);
                        break;
                    //case TaskType.BRANCH:
                    //pageTemp = OnCreateBanchPage(transBranch, data);
                    //  ShowSubBranchBtn();
                    // break;
                    default:
                        break;
                }

                pageTemp.SetActive(true);
                dicTaskPage[taskId] = pageTemp;
            }
        }

        //目标任务变化
        private void OnCurTaskTypeChange()
        {
            //DisposeAvatar();
            //DisposeCamera();
        }
        #endregion

        #region 空任务界面
        private GameObject OnCreateEmptyPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;

            pageClon.transform.SetParent(conTask, false);
            return pageClon;
        }
        #endregion

        #region 主线任务相关
        //显示主线任务
        private GameObject OnCreateMainPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;

            //0任务描述, 1任务进度
            Transform transDesc = pageClon.transform.GetChild(0);
            Transform transState = pageClon.transform.GetChild(1);

            OnTaskDescChange(transDesc, data);
            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
            ShowPanelTaskReward(transState, data);
            OnTaskNameChange(transState, data);
            ShowChapterPrize(transDesc, data);

            pageClon.transform.SetParent(conTask, false);

            Transform btnMove = transState.FindChild("btn_move");
            InitMoveBtn(btnMove);

            return pageClon;
        }

        private void OnMainTaskDataChange(GameObject page, TaskData data)
        {
            //0任务描述, 1任务进度
            Transform transDesc = page.transform.GetChild(0);
            Transform transState = page.transform.GetChild(1);

            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
        }

        #endregion

        #region 每日任务相关
        //每日任务
        private Text btntextOneKeyFinish;
        private GameObject OnCreateDailyPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;

            //0任务描述, 1任务进度
            Transform transDesc = pageClon.transform.GetChild(0);
            Transform transState = pageClon.transform.GetChild(1);

            int id = data.taskId;

            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
            OnTaskNameChange(transState, data);
            OnStarInfoChange(transState, data);
            OnOneKeyFinishCostChange(transDesc, data);
            OnPrizeAndMoveBtnChange(transState, data);
            ShowDailyExtraPrize(transDesc, data);
            ShowPanelTaskReward(transState, data);

            Transform btnMove = transState.FindChild("btn_move");
            InitMoveBtn(btnMove);
            Transform btnUpgradeStar = transState.FindChild("star/btn_oneKey");
            InitOnkeUpgradeStar(btnUpgradeStar);
            Transform btnGetReward1 = transState.FindChild("get_reward/1");
            Transform btnGetReward2 = transState.FindChild("get_reward/2");
            InitGetPrizeBtn(btnGetReward1);
            InitGetPrizeBtn(btnGetReward2);
            Transform btnOneKeyFinish = transDesc.FindChild("btn_onekey");
            InitOneKeyFinishTask(btnOneKeyFinish);

            pageClon.transform.SetParent(conTask, false);

            transDesc.FindChild("reward/state").GetComponent<Text>().text = ContMgr.getCont("daily_limit_tip_1", new string[] { A3_TaskModel.DAILY_TASK_LIMIT.ToString() });

            return pageClon;
        }
        //刷新一键完成按钮上的钻石数量
        private void OnRefreshOneKeyFinishBtnCost() => btntextOneKeyFinish.text = (
                Mathf.Max(tkModel.GetOneKeyFinishEveryOneCost() * (tkModel.GetDailyMaxCount() - tkModel.GetTaskDataByTaskType(TaskType.DAILY).Count), 0)).ToString();
        //每日任务星星变化
        int starLevel = 0;
        private void OnStarInfoChange(Transform conState, TaskData data)
        {
            Transform conStar = conState.FindChild("star/con_star");
            starLevel = data.taskStar;

            for (int i = 0; i < conStar.childCount; i++)
            {
                if (i < starLevel)
                {
                    conStar.GetChild(i).GetChild(0).gameObject.SetActive(true);
                    conStar.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    conStar.GetChild(i).GetChild(0).gameObject.SetActive(false);
                    conStar.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
            }

            //TODO 金币消耗变化
            Text textCost = conState.FindChild("star/Text_cost").GetComponent<Text>();
            textCost.text = Globle.getBigText(tkModel.GetRefreshStarCost());

            ShowPanelTaskReward(conState, data);
        }

        //每日任务一键完成任务的钻石消耗变化
        private void OnOneKeyFinishCostChange(Transform conDesc, TaskData data)
        {
            Text textCost = conDesc.FindChild("btn_onekey/cost").GetComponent<Text>();

            int maxCount = tkModel.GetDailyMaxCount();
            int leftCount = maxCount - data.taskCount;
            int oneCost = tkModel.GetOneKeyFinishEveryOneCost();
            int cost = oneCost * leftCount;

            textCost.text = Globle.getBigText((uint)cost);

            uint myYb = PlayerModel.getInstance().gold;

            if (myYb < cost)
                textCost.color = Globle.getColorByQuality(7);
            else
                textCost.color = Globle.getColorByQuality(2);


        }

        //任务奖励
        private void ShowPanelTaskReward(Transform conDesc, TaskData data)
        {
            var listRw = data.taskT == TaskType.CLAN ? tkModel.GetClanReward(data.taskCount) : tkModel.GetReward(data.taskId);
            GameObject pb = conDesc.FindChild("reward/view/icon_bg").gameObject;
            Transform container = conDesc.FindChild("reward/view/con");
            for (int i = 0; i < container.childCount; i++)
            {
                GameObject go = container.GetChild(i).gameObject;
                Destroy(go);
            }

            foreach (var v in listRw)
            {
                uint iconid = 0;
                int num = v.num;
                switch (v.type)
                {
                    case 1:
                        iconid = 3002;
                        break;
                    case 2:
                        if (!v.item.isEquip)
                            continue;
                        iconid = (uint)v.id;
                        num = -1;
                        break;
                    case 3:
                        iconid = (uint)v.id;
                        break;
                }
                a3_ItemData item = a3_BagModel.getInstance().getItemDataById(iconid);
                if (v.type == 1)
                {
                    if (data.taskT == TaskType.CLAN)
                    {
                        item.file = "icon_comm_1x" + v.id;
                    }
                    else
                    {
                        item.file = "icon_comm_0x" + v.id;
                    }
                }
                if (num != -1 && data.taskT == TaskType.DAILY && starLevel != 0) num = num * starLevel;
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item, false, num, 0.8f);
                var pbg = GameObject.Instantiate(pb) as GameObject;
                pbg.transform.SetParent(icon.transform);
                pbg.transform.SetAsFirstSibling();
                pbg.transform.localScale = Vector3.one;
                pbg.transform.localPosition = Vector3.zero;
                pbg.GetComponent<RectTransform>().sizeDelta /= 0.8f;
                pbg.SetActive(true);
                var bd = icon.transform.FindChild("iconborder/equip_canequip");
                if (bd != null) bd.gameObject.SetActive(false);
                icon.transform.SetParent(container.transform, false);
                icon.SetActive(true);
                icon.transform.FindChild("iconborder").GetComponent<RectTransform>().sizeDelta = new Vector2(78f, 78f);
            }
        }

        //每日任务额外奖励
        private void ShowDailyExtraPrize(Transform conDesc, TaskData data)
        {
            int extarId = data.extraAward;
            Dictionary<uint, int> dicExtar = tkModel.GetExtarPrizeData(extarId);

            Transform container = conDesc.FindChild("reward/view/con");
            OnCreatePrizeIcon(container, dicExtar, 0.8F);
        }

        //章节奖励
        private void ShowChapterPrize(Transform conDesc, TaskData data)
        {
            int chapterId = data.chapterId;
            Dictionary<uint, int> dicExtar = tkModel.GetChapterPrizeData(chapterId);

            Transform container = conDesc.FindChild("reward/view/con");
            OnCreatePrizeIcon(container, dicExtar, 0.8F);
        }

        //每日任务的任务奖励
        private void ShowDailyTaskPrize(Transform conState, TaskData data)
        {
            Dictionary<uint, int> dicReward = tkModel.GetValueReward(data.taskId);
            if (dicReward == null)
                return;

            Transform container = conState.FindChild("reward/view/con");
            OnCreatePrizeIcon(container, dicReward, 0.8F);
        }

        private void OnDailyTaskDataChange(GameObject page, TaskData data)
        {
            //0任务描述, 1任务进度
            Transform transDesc = page.transform.GetChild(0);
            Transform transState = page.transform.GetChild(1);

            OnOneKeyFinishCostChange(transDesc, data);
            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
            //OnTaskNameChange(transState, data);
            OnStarInfoChange(transState, data);
            OnPrizeAndMoveBtnChange(transState, data);
        }

        #endregion

        #region 支线任务相关
        private GameObject OnCreateBanchPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;
            bmis.name = pageClon.transform.FindChild("0/title/Text").GetComponent<Text>();
            bmis.desc = pageClon.transform.FindChild("0/desc/Text").GetComponent<Text>();
            bmis.btnGo = pageClon.transform.FindChild("0/btnGo").GetComponent<Button>();
            if (data.explain.Length > 0)
                pageClon.transform.FindChild("0/state/Text").GetComponent<Text>().text = data.explain;
            bmis.rewardCon = pageClon.transform.FindChild("0/con_icon");
            bmis.name.text = data.taskName;
            bmis.desc.text = A3_TaskModel.getInstance().GetTaskDesc(data.taskId) + "(" + data.taskRate + "/" + data.completion + ")";
            var listReward = A3_TaskModel.getInstance().GetReward(data.taskId);
            for (int i = 0; i < bmis.rewardCon.childCount; i++)
            {
                Destroy(bmis.rewardCon.GetChild(i).gameObject);
            }
            for (int i = 0; i < listReward.Count; i++)
            {
                GameObject go = null;
                if (listReward[i].type == 1) // 经验、金币
                    go = IconImageMgr.getInstance().createMoneyIcon(type: listReward[i].id.ToString(), num: listReward[i].num);
                else if (listReward[i].type == 2) // 装备
                    go = IconImageMgr.getInstance().createA3ItemIcon(listReward[i].item, istouch: true);
                else // 道具
                    go = IconImageMgr.getInstance().createA3ItemIcon(listReward[i].item, istouch: true, num: listReward[i].num);
                if (go != null)
                    go.transform.SetParent(bmis.rewardCon, false);
            }
            pageClon.transform.SetParent(conTask, false);
            pageClon.SetActive(true);
            return pageClon;
        }

        //讨伐任务额外奖励
        private void ShowKillExtraPrize(Transform conDesc, TaskData data)
        {
            int extarId = data.extraAward;
            Dictionary<uint, int> dicExtar = tkModel.GetExtarPrizeData(extarId);
            Transform container = conDesc.FindChild("con_icon");


            OnCreatePrizeIcon(container, dicExtar, 0.8F);
        }

        ////讨伐任务的任务奖励
        //private void ShowKillTaskPrize(Transform conState, TaskData data)
        //{
        //    List<RewardValue> listValue = data.listRewardValue;

        //    Dictionary<a3_ItemData, int> dicItem = new Dictionary<a3_ItemData, int>();
        //    Transform container = conState.FindChild("reward/view/con");
        //    a3_BagModel bgModel = a3_BagModel.getInstance();

        //    foreach (RewardValue item in listValue)
        //    {
        //        a3_ItemData itemTemp = new a3_ItemData();

        //        itemTemp = bgModel.getItemDataById((uint)item.id);

        //        int num = item.num;
        //        dicItem[itemTemp] = num;
        //    }

        //    OnCreatePrizeIcon(container, dicItem, 0.8F);
        //}

        //上一个怪物的avatar id
        private int lastAvatarId = -1;
        private GameObject monsterAvatar = null;//角色的avatar
        private GameObject monsterCamera = null;//拍摄avatar的摄像机
        //创建相关怪物的avatar
        private void CreateMonsterAvatar(int id)
        {
            if (lastAvatarId == id)
                return;
            if (monsterAvatar != null)
                Destroy(monsterAvatar);

            GameObject obj = null;
            SXML xml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + id);
            string path = string.Empty;

            if (xml != null)
            {
                string str = xml.getString("obj");
                path = "monster_" + str;
            }

            obj = GAMEAPI.ABModel_LoadNow_GameObject(path);
            if (obj != null)
            {
                monsterAvatar = GameObject.Instantiate(obj, new Vector3(-128.8f, 0f, 0f), new Quaternion(0, 90, 0, 0)) as GameObject;
            }

            //MonsterRole mRole = new MonsterRole();
            //mRole.Init(path, 0, new Vector3(-128.8f, 0f, 0f));

        }

        //创建相关的相机
        private void CreateMonsterCamera()
        {
            GameObject obj = null;
            if (monsterCamera == null)
            {
                obj = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_wing_ui_camera");
                if (obj != null)
                {
                    monsterCamera = GameObject.Instantiate(obj) as GameObject;
                    Camera cam = monsterCamera.GetComponentInChildren<Camera>();
                    if (cam != null)
                    {
                        float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                        cam.orthographicSize = r_size;
                    }
                }
            }
        }

        //处理avatar
        private void DisposeAvatar()
        {
            if (monsterAvatar != null)
            {
                Destroy(monsterAvatar);
                lastAvatarId = -1;
            }
        }

        //处理相机
        private void DisposeCamera()
        {
            if (monsterCamera != null)
            {
                Destroy(monsterCamera);
            }
        }


        #endregion
        #region 委托任务相关

        void onENTRUSTChange(GameObject page, TaskData data)
        {
            Transform transDesc = page.transform.GetChild(0);
            Transform transState = page.transform.GetChild(1);

            OnTaskCountChange(transDesc, data);
        }
        private GameObject OnCreateEntrustPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;

            //0任务描述, 1任务进度
            Transform transDesc = pageClon.transform.GetChild(0);
            Transform transState = pageClon.transform.GetChild(1);
            Transform transExtraReward = transDesc.transform.FindChild("reward/view/con");
            int id = data.taskId;

            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
            OnTaskNameChange(transState, data);
            ShowPanelTaskReward(transState, data);
            ShowPanelExtraEntTaskReward(transDesc, A3_TaskModel.getInstance().GetEntrustRewardList());
            Transform btnMove = transState.FindChild("btn_move");
            InitMoveBtn(btnMove);

            pageClon.transform.SetParent(conTask, false);

            //List<SXML> listExtraReward = XMLMgr.instance.GetSXML("task").GetNodeList("entrust_extra_award");
            //if (listExtraReward.Count != 0)
            //{
            //    int i, zhuanNeed, lvNeed;
            //    for (i = 0; i < listExtraReward.Count; i++)
            //    {
            //        zhuanNeed = listExtraReward[i].getInt("zhuan");
            //        lvNeed = listExtraReward[i].getInt("lv");
            //        if (zhuanNeed > PlayerModel.getInstance().up_lvl || zhuanNeed == PlayerModel.getInstance().up_lvl && lvNeed > PlayerModel.getInstance().lvl)
            //            break;
            //    }
            //    if (i > 0)
            //        i = i - 1;
            //    List<SXML> listRewardOnLv = listExtraReward[i].GetNodeList("RewardItem");
            //    uint itemId;
            //    int itemCount;
            //    for (int j = 0; j < listRewardOnLv.Count; j++)
            //    {
            //        itemId = listRewardOnLv[j].getUint("item_id");
            //        itemCount = listRewardOnLv[j].getInt("value");
            //        GameObject rewardItem = IconImageMgr.getInstance().createA3ItemIcon(itemid: itemId, istouch: true, num: itemCount, ignoreLimit: true, isicon: true);
            //        rewardItem.transform.SetParent(transExtraReward, false);
            //    }
            //}
            return pageClon;
        }
        void ShowPanelExtraEntTaskReward(Transform tfRewardRoot,List<EntrustExtraRewardData> rewardList)
        {
            Transform con = tfRewardRoot.FindChild("reward/view/con");
            for (int i = con.childCount; i > 0; i++)
                DestroyImmediate(con.GetChild(i).gameObject);
            for (int i = 0; i < rewardList.Count; i++)
                IconImageMgr.getInstance().createA3ItemIcon(rewardList[i].tpid, num: rewardList[i].num).transform.SetParent(con, false);

            float childSizeX = con.transform.GetComponent<GridLayoutGroup>().cellSize.x;
            float childSizeX_sp = con.transform.GetComponent<GridLayoutGroup>().spacing.x;
            Vector2 newSize = new Vector2(rewardList.Count * childSizeX + (rewardList.Count -1) * childSizeX_sp, con.GetComponent <RectTransform>().sizeDelta.y);
            con.GetComponent<RectTransform>().sizeDelta = newSize;
        }
        #endregion
        private GameObject OnCreateClanTaskPage(GameObject pageTemp, TaskData data)
        {
            GameObject pageClon = Instantiate(pageTemp) as GameObject;

            //0任务描述, 1任务进度
            Transform transDesc = pageClon.transform.GetChild(0);
            Transform transState = pageClon.transform.GetChild(1);
            Transform transExtraReward = transDesc.transform.FindChild("reward/view/con");
            int id = data.taskId;
            if (data.explain.Length > 0)
                pageClon.transform.FindChild("0/state").GetComponent<Text>().text = data.explain;
            OnTaskCountChange(transDesc, data);
            OnTaskRateChange(transState, data);
            OnTaskStateChange(transState, data);
            OnTaskNameChange(transState, data);
            ShowPanelTaskReward(transState, data);
            #region 每轮奖励
            List<SXML> rewardEachLoop = XMLMgr.instance.GetSXML("task.clan_extra").GetNodeList("RewardItem");
            Transform con = pageClon.transform.FindChild("0/reward/view/con");
            for (int i = con.childCount; i > 0; i++)
                DestroyImmediate(con.GetChild(i).gameObject);
            for (int i = 0; i < rewardEachLoop.Count; i++)
                IconImageMgr.getInstance().createA3ItemIcon(rewardEachLoop[i].getUint("item_id"), num: rewardEachLoop[i].getInt("value")).transform.SetParent(con, false);
            #endregion
            Transform btnMove = transState.FindChild("btn_move");
            InitMoveBtn(btnMove);

            pageClon.transform.SetParent(conTask, false);
            return pageClon;
        }
        #region 任务通用
        //任务描述变化
        private void OnTaskDescChange(Transform conDesc, TaskData data)
        {
            Text textDesc = conDesc.FindChild("state/Text").GetComponent<Text>();
            Text textTetle = conDesc.FindChild("title/Text").GetComponent<Text>();
            ChapterInfos mainChapterInfo = tkModel.GetChapterInfosById(data.chapterId);

            textTetle.text = mainChapterInfo.name;
            textDesc.text = mainChapterInfo.description;
        }

        //任务总进度发生变化
        private void OnTaskCountChange(Transform conDesc, TaskData data)
        {
            Slider sliderRate = conDesc.FindChild("slider/slider").GetComponent<Slider>();
            Text textRate = conDesc.FindChild("slider/text").GetComponent<Text>();

            int count = data.taskCount;
            int loop = data.taskLoop;
            int maxCount = tkModel.GetTaskMaxCount(data.taskId);

            if (data.taskT == TaskType.ENTRUST)
            {
                sliderRate.maxValue = maxCount;           
                sliderRate.value = count + 1;
                textRate.text = "(" + ((count % maxCount) + 1) + "/" + maxCount + ")";
            }
            else if (data.taskT == TaskType.CLAN)
            {
                maxCount = (int)A3_TaskModel.CLAN_MAX_COUNT;
                sliderRate.maxValue = maxCount;
                sliderRate.value = loop * A3_TaskModel.CLAN_CNT_EACHLOOP + count + 1;
                textRate.text = "(" + (sliderRate.value) + "/" + maxCount + ")";
            }
            else
            {
                sliderRate.maxValue = maxCount;
                sliderRate.value = count;
                textRate.text = "(" + count + "/" + maxCount + ")";
            }
        }

        //当前任务进度发生变化
        private void OnTaskRateChange(Transform conState, TaskData data)
        {
            Text textRate = conState.FindChild("task/Text_state/Text_count").GetComponent<Text>();
            Transform taskCount = conState.FindChild("task/Text_state/Text_count");

            TaskTargetType targetType = data.targetType;


            if (targetType == TaskTargetType.VISIT)
                taskCount.gameObject.SetActive(false);
            else
                taskCount.gameObject.SetActive(true);

            int maxCount = data.completion;
            int count = data.taskRate;

            //textRate.text = "(" + count + "/" + maxCount + ")";
            textRate.text = ""; 
        }

        //任务内容变化
        private void OnTaskStateChange(Transform conState, TaskData data)
        {
            Text textState = conState.FindChild("task/Text_state").GetComponent<Text>();
            Transform taskCount = conState.FindChild("task/Text_state/Text_count");

            TaskTargetType targetType = data.targetType;
            if (targetType == TaskTargetType.VISIT)
                taskCount.gameObject.SetActive(false);
            else
                taskCount.gameObject.SetActive(true);

            int maxCount = data.completion;
            int count = data.taskRate;

            textState.text = tkModel.GetTaskDesc(data.taskId, false);

            if (data.isComplete)
            {
                textState.text = tkModel.GetTaskDesc(data.taskId, false) + ContMgr.getCont("canover");
                taskCount.gameObject.SetActive(true);
            }
            else
            {
                textState.text = tkModel.GetTaskDesc(data.taskId, false) + " (" + count + "/" + maxCount + ")";
                taskCount.gameObject.SetActive(true);
            }
        }

        //任务名字变化
        private void OnTaskNameChange(Transform conState, TaskData data)
        {
            Text textTitle = conState.FindChild("title/Text").GetComponent<Text>();

            textTitle.text = data.taskName;
        }

        //创建奖励icon
        private void OnCreatePrizeIcon(Transform container, Dictionary<uint, int> dicItem, float iconScale)
        {
            GameObject pb = container.parent.FindChild("icon_bg").gameObject;
            for (int i = 0; i < container.childCount; i++)
            {
                GameObject go = container.GetChild(i).gameObject;
                Destroy(go);
            }

            foreach (uint id in dicItem.Keys)
            {
                int num = dicItem[id];
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(id, false, num, iconScale, true, ignoreLimit: true);

                var pbg = GameObject.Instantiate(pb) as GameObject;
                pbg.transform.SetParent(icon.transform);
                pbg.transform.SetAsFirstSibling();
                pbg.transform.localScale = Vector3.one;
                pbg.transform.localPosition = Vector3.zero;
                pbg.GetComponent<RectTransform>().sizeDelta /= 0.8f;
                pbg.SetActive(true);

                icon.transform.SetParent(container.transform, false);
                icon.SetActive(true);
                icon.transform.FindChild("iconborder").GetComponent<RectTransform>().sizeDelta = new Vector2(78f, 78f);
            }
        }

        //初始化移动按钮
        private void InitMoveBtn(Transform trans)
        {
            BaseButton btn = new BaseButton(trans);
            btn.onClick = OnMoveClick;
        }

        //初始化领奖按钮
        private void InitGetPrizeBtn(Transform trans)
        {
            BaseButton btn = new BaseButton(trans);
            btn.onClick = OnGetPrizeClick;
        }

        //初始化一件升级按钮
        private void InitOnkeUpgradeStar(Transform trans)
        {
            BaseButton btn = new BaseButton(trans);
            btn.onClick = InitOnkeyUpgradeStarClick;
        }

        //初始化一键完成任务按钮
        private void InitOneKeyFinishTask(Transform trans)
        {
            BaseButton btn = new BaseButton(trans);
            btn.onClick = OnTaskOneKeyFiniskClick;
        }

        //领取奖励按钮和移动按钮切换
        private void OnPrizeAndMoveBtnChange(Transform panel, TaskData data)
        {
            int taskId = data.taskId;
            Transform moveBtn = panel.FindChild("btn_move");
            Transform prizeBtn = panel.FindChild("get_reward");

            bool isComplete = data.isComplete;

            moveBtn.gameObject.SetActive(!isComplete);
            prizeBtn.gameObject.SetActive(isComplete);

        }

        #endregion

        #region 点击事件
        //移动到目标
        public void OnMoveClick(GameObject go)
        {
            A3_TaskModel tkModel = A3_TaskModel.getInstance();

            a3_task_auto.instance.RunTask(tkModel.curTask, clickAuto: true);

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TASK);
        }

        //一键完成日常任务
        private void OnTaskOneKeyFiniskClick(GameObject go)
        {

            Dictionary<int, TaskData> dicTaskData = tkModel.GetTaskDataByTaskType(TaskType.DAILY);

            foreach (TaskData data in dicTaskData.Values)
            {
                OnOneKeyFinishTask(data);
            }
        }

        //快速完成任务
        private void OnOneKeyFinishTask(TaskData data)
        {
            int maxCount = tkModel.GetDailyMaxCount();
            int leftCount = maxCount - data.taskCount;

            if (leftCount < 1)
            {
                debug.Log("每日任务已完成");
                return;
            }

            int oneCost = tkModel.GetOneKeyFinishEveryOneCost();
            int cost = oneCost * leftCount;
            uint myYb = PlayerModel.getInstance().gold;

            if (myYb < cost)
            {
                flytxt.instance.fly(ContMgr.getError("-1001"));
                return;
            }

            A3_TaskProxy.getInstance().OneKeyFinishTask();
        }

        //点击关闭
        private void OnCloseClick(GameObject go)
        {
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TASK);
        }

        //改变奖励星级
        private void InitOnkeyUpgradeStarClick(GameObject go)
        {
            uint myMoney = PlayerModel.getInstance().money;
            uint cost = tkModel.GetRefreshStarCost();
            int taskId = tkModel.curTask.taskId;

            TaskData data = tkModel.GetTaskDataById(taskId);

            if (myMoney < cost)
            {
                ContMgr.getError("-1002");
                return;
            }
            int maxLevel = tkModel.GetMaxStarLevel();
            if (data.taskStar > maxLevel)
            {
                debug.Log("已达到最高星级");
                return;
            }

            A3_TaskProxy.getInstance().SendUpgradeStar();

        }

        #endregion

        #region 领取奖励按钮

        //领取奖励
        private void OnGetPrizeClick(GameObject go)
        {
            int rate = int.Parse(go.name);
            int curTaskId = tkModel.curTask.taskId;
            TaskData curTask = tkModel.curTask;

            //TODO 发送领取奖励
            if (!curTask.isComplete)
            {
                debug.Log("任务未完成");
                return;
            }

            switch (rate)
            {
                case 1:
                    OnePrize(rate);
                    break;
                case 2:
                    DoublePrize(rate);
                    break;
                default:
                    break;
            }


        }

        //双倍奖励
        private void DoublePrize(int rate)
        {
            uint cost = tkModel.GetDoublePrizeCost();
            uint myYb = PlayerModel.getInstance().gold;

            if (myYb < cost)
            {
                flytxt.instance.fly(ContMgr.getCont("off_line_exp_gold"));
                //InterfaceMgr.getInstance().open(InterfaceMgr.A3_RECHARGE).winComponent.transform.SetAsLastSibling();
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                return;
            }

            A3_TaskProxy.getInstance().SendGetAward(rate);
        }

        //普通奖励
        private void OnePrize(int rate)
        {
            A3_TaskProxy.getInstance().SendGetAward();
        }

        #endregion

        //执行任务
        public static void ExcutTask(int taskId)
        {
            A3_TaskModel tkModel = A3_TaskModel.getInstance();
            TaskData data = tkModel.GetTaskDataById(taskId);
        }

    }
    struct BranchMissionText
    {
        public Text desc;
        public Text name;
        public Transform rewardCon;
        public Button btnGo;
    }
    class BranchMissionObj
    {
        public GameObject panel;
        public GameObject btnGo;
    }
}
