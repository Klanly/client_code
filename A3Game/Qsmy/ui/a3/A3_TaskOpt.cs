using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class TaskOptElement
    {
        public int taskId;
        public bool isTaskMonsterAlive;
        public bool isKeepingKillMon;
        //public long timeKillTerminal;
        public TaskOptElement() { }
        public TaskOptElement(int taskId, bool? isTaskMonsterAlive = null, bool? isKeepingKillMon = null/*, long? timeKillTerminal = null*/)
        {
            this.taskId = taskId;
            this.isTaskMonsterAlive = isTaskMonsterAlive.GetValueOrDefault(this.isTaskMonsterAlive);
            this.isKeepingKillMon = isKeepingKillMon.GetValueOrDefault(this.isKeepingKillMon);
            //this.timeKillTerminal = timeKillTerminal.GetValueOrDefault(this.timeKillTerminal);
        }
        public void InitUi(Text liteMinimapTaskTimer = null)
        {
            this.liteMinimapTaskTimer = liteMinimapTaskTimer;
        }
        private Text _liteMinimapTaskTimer;
        public Text liteMinimapTaskTimer
        {
            set
            {
                _liteMinimapTaskTimer = value;
            }
            get
            {
                if (_liteMinimapTaskTimer == null)
                    _liteMinimapTaskTimer = a3_liteMinimap.instance.GetTaskPage(taskId).transform.FindChild("name/timer").GetComponent<Text>();
                return _liteMinimapTaskTimer;
            }
        }
        public void Set(bool? isTaskMonsterAlive = null, bool? isKeepingKillMon = null, long? timeKillTerminal = null)
        {
            this.isTaskMonsterAlive = isTaskMonsterAlive.GetValueOrDefault(this.isTaskMonsterAlive);
            this.isKeepingKillMon = isKeepingKillMon.GetValueOrDefault(this.isKeepingKillMon);
            //this.timeKillTerminal = timeKillTerminal.GetValueOrDefault(this.timeKillTerminal);
        }
    }
    class A3_TaskOpt : FloatUi
    {
        public int curTaskId;
        private static float waitThresholdDistance = 3f;
        private static float killThresholdDistance = 1.5f;
        private uint submitItemIId;
        private float timeCD => A3_TaskModel.getInstance().GetTaskDataById(curTaskId).need_tm;
        private Transform tfParentWait;
        public Transform tfSubmitItem;
        private Transform tfSubmitItemCon;
        private Transform tfSubmitItemMainCon;
        private Transform tfFocus;
        private bool haveEnteredWaitPosition;
        public uint taskItemId;
        private Image imgProcess;
        private long timeWaitTerminal;
        //private Dictionary<int/*task id*/, long/*timer value*/> timeKillTerminal;
        private GameObject winKillMon, winKillDragon;
        public static A3_TaskOpt Instance;
        public bool IsOnTaskWait { get; set; }
        public bool isWaiting;
        public bool IsOnKillMon { get; set; }
        public bool IsOnTaskGet { get; set; }
        public Vector3 waitPosition;
        public Vector3 killPosition;
        public BaseButton BtnWait;
        //public bool isTaskMonsterAlive;
        //public Dictionary<int/*task id*/,bool /*is killing*/> isKeepingKillMon;
        public Dictionary<int, TaskOptElement> taskOptElement;
        private Vector3 scaleIcon;
        //public Dictionary<uint,
        public Image actionImage;
        Transform tfBtnStart;
        private List<a3_BagItemData> bagItem = new List<a3_BagItemData>();
        public override void init()
        {
            Instance = this;
            tfParentWait = transform.FindChild("wait");
            tfParentWait.gameObject.SetActive(false);
            imgProcess = tfParentWait.FindChild("waitBG").GetComponent<Image>();
            (BtnWait = new BaseButton(tfParentWait.FindChild("waitBG/btnDoWait"))).onClick = OnWaitBtnClick;
            actionImage = tfParentWait.FindChild("waitBG/btnDoWait").GetComponent<Image>();
            winKillMon = transform.FindChild("killmon").gameObject;
            winKillMon.SetActive(false);
            winKillDragon = transform.FindChild("killDragon").gameObject;
            winKillDragon.SetActive(false);
            tfSubmitItem = transform.FindChild("submitItem");
            tfSubmitItemMainCon = tfSubmitItem.FindChild("mask/scrollview/rect");
            tfSubmitItemCon = tfSubmitItem.FindChild("mask/scrollview/rect/con");
            tfFocus = tfSubmitItem.FindChild("focus");
            tfFocus.gameObject.SetActive(false);
            tfSubmitItem.gameObject.SetActive(false);
            tfBtnStart = winKillMon.transform.FindChild("btnStart");
            Transform tfBtnCancel = winKillMon.transform.FindChild("btnDontStart");
            new BaseButton(tfBtnStart).onClick = OnStartBtnClick;
            new BaseButton(tfBtnCancel).onClick = OnCancelBtnClick;
            new BaseButton(transform.FindChild("submitItem/closeBtn")).onClick = (btnClose) => tfSubmitItem.gameObject.SetActive(false);
            new BaseButton(transform.FindChild("killmon/closeArea")).onClick = (go) => go.transform.parent.gameObject.SetActive(false);
            new BaseButton(tfSubmitItem.FindChild("btnOK")).onClick = (go) =>
            {
                if (submitItemIId != 0 && curTaskId != 0)
                    A3_TaskProxy.getInstance().SendSubmit(curTaskId, submitItemIId);
                tfSubmitItem.gameObject.SetActive(false);
            };
            //transform.SetParent(skillbar.instance.transform);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnCheck);
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_NEW_TASK, OnCheck);

            taskOptElement = new Dictionary<int, TaskOptElement>();
            Dictionary<int, TaskData> listTask = A3_TaskModel.getInstance().GetDicTaskData();
            List<int> idx = new List<int>(listTask.Keys);
            for (int i = 0; i < idx.Count; i++)
            {
                int taskId = idx[i];
                if (listTask[taskId].release_tm > 0)
                    if (listTask[taskId].lose_tm > muNetCleint.instance.CurServerTimeStamp)
                    {
                        taskOptElement[taskId] =
                            new TaskOptElement(taskId,
                            isKeepingKillMon: true,
                            isTaskMonsterAlive: true//,
                                                    //timeKillTerminal: A3_TaskModel.getInstance().GetTaskDataById(taskId).lose_tm
                        );
                        taskOptElement[taskId].InitUi(
                            liteMinimapTaskTimer: a3_liteMinimap.instance.GetTaskPage(taskId)?.transform.FindChild("name/timer").GetComponent<Text>()
                            );
                    }
            }
            Instance.name = "A3_TaskOpt";
            waitThresholdDistance = XMLMgr.instance.GetSXML("task.range").getFloat("action_range") / GameConstant.PIXEL_TRANS_UNITYPOS;
            scaleIcon = transform.FindChild("submitItem/iconConfig")?.localScale ?? Vector3.zero;
            new BaseButton(winKillDragon.transform.FindChild("btnStart")).onClick = (go) =>
            {
                uint dragonId = A3_SlayDragonModel.getInstance().GetUnlockedDragonId();
                int diffLv = A3_SlayDragonModel.getInstance().GetUnlockedDiffLv();
                A3_SlayDragonProxy.getInstance().SendGo();
            };
            new BaseButton(winKillDragon.transform.FindChild("btnNope")).onClick = (go) => winKillDragon.SetActive(false);
            if (!IsInvoking("RunTimer"))
                InvokeRepeating("RunTimer", 0f, 1.0f);



            getComponentByPath<Text>("killmon/bg/desc").text = ContMgr.getCont("A3_TaskOpt_0");
            getComponentByPath<Text>("killmon/btncreatteam/Text").text = ContMgr.getCont("A3_TaskOpt_1");
            getComponentByPath<Text>("killmon/btnDontStart/Text").text = ContMgr.getCont("A3_TaskOpt_2");
            getComponentByPath<Text>("killmon/btnStart/Text").text = ContMgr.getCont("A3_TaskOpt_3");
            getComponentByPath<Text>("wait/action_text").text = ContMgr.getCont("A3_TaskOpt_4");
            getComponentByPath<Text>("submitItem/title/Text").text = ContMgr.getCont("A3_TaskOpt_5");
            getComponentByPath<Text>("submitItem/btnOK/text").text = ContMgr.getCont("A3_TaskOpt_6");
            getComponentByPath<Text>("killDragon/bg/desc").text = ContMgr.getCont("A3_TaskOpt_7");
            getComponentByPath<Text>("killDragon/btnStart/Text").text = ContMgr.getCont("A3_TaskOpt_8");
            getComponentByPath<Text>("killDragon/btnNope/Text").text = ContMgr.getCont("A3_TaskOpt_2");






        }
        public void ShowDragonWin()
        {
            winKillDragon?.SetActive(true);
        }
        private void OnCheck(GameEvent e)
        {
            Variant data = e.data;
            if (data == null) return;
            if (data.ContainsKey("change_task") && data["change_task"].Length > 0)
            {
                Variant task = data["change_task"][0];
                if (task.ContainsKey("id"))
                {
                    int taskId = task["id"]._int;
                    TaskData taskData = A3_TaskModel.getInstance().GetTaskDataById(taskId);
                    if (taskOptElement.ContainsKey(taskId) && task.ContainsKey("cnt") && task["cnt"]._int >= taskData.completion)
                    {
                        taskOptElement[taskId].Set(
                            isKeepingKillMon: false,
                            isTaskMonsterAlive: false,
                            timeKillTerminal: -1
                        );
                        if (taskOptElement.ContainsKey(taskId))
                        {
                            taskOptElement[taskId].Set(isTaskMonsterAlive: false);
                            taskOptElement[taskId].liteMinimapTaskTimer.gameObject?.SetActive(false);
                            taskOptElement.Remove(taskId);
                        }
                        //taskOptElement[taskId].liteMinimapTaskTimer.gameObject.SetActive(false);
                    }
                    else if (taskData.targetType == TaskTargetType.KILL_MONSTER_GIVEN)
                    {
                        uint lose_tm = task.ContainsKey("lose_tm") ? task["lose_tm"]._uint : 0;
                        taskOptElement[taskId].Set(
                            isKeepingKillMon: true,
                            isTaskMonsterAlive: taskData.release_tm > lose_tm - muNetCleint.instance.CurServerTimeStamp,
                            timeKillTerminal: lose_tm
                        );
                    }
                }
            }
            else
            {
                int taskId = 0;
                Variant misData = new Variant();
                TaskTargetType taskTargetType = default(TaskTargetType);
                if (data.ContainsKey("mlmis"))
                    taskTargetType = A3_TaskModel.getInstance().GetTaskDataById((taskId = (misData = data["mlmis"])["id"]._int)).targetType;
                if (data.ContainsKey("bmis"))
                {
                    misData = data["bmis"];
                    if (misData != null)
                    {
                        taskId = misData["id"]._int;
                        if (taskId > 0)
                        {
                            TaskData tData = A3_TaskModel.getInstance().GetTaskDataById(taskId);
                            if (tData != null)
                                taskTargetType = tData.targetType;
                        }
                    }
                }
                if (data.ContainsKey("dmis"))
                    taskTargetType = A3_TaskModel.getInstance().GetTaskDataById((taskId = (misData = data["dmis"])["id"]._int)).targetType;
                if (data.ContainsKey("emis"))
                    taskTargetType = A3_TaskModel.getInstance().GetTaskDataById((taskId = (misData = data["emis"])["id"]._int)).targetType;
                if (data.ContainsKey("cmis"))
                    taskTargetType = A3_TaskModel.getInstance().GetTaskDataById((taskId = (misData = data["cmis"])["id"]._int)).targetType;
                if (taskTargetType == TaskTargetType.KILL_MONSTER_GIVEN)
                {
                    if (misData != null)
                    {
                        long timeKillTerminal;
                        if (misData.ContainsKey("cnt") && misData["cnt"] < A3_TaskModel.getInstance().GetTaskDataById(taskId).completion && misData.ContainsKey("lose_tm"))
                            timeKillTerminal = misData["lose_tm"]._int64;
                        else
                            timeKillTerminal = 0;
                        if (taskOptElement.ContainsKey(taskId))
                            taskOptElement[taskId].Set(isKeepingKillMon: true, isTaskMonsterAlive: false, timeKillTerminal: timeKillTerminal);
                    }
                }
                else if (taskTargetType == TaskTargetType.GET_ITEM_GIVEN)
                    taskItemId = (uint)A3_TaskModel.getInstance().GetTaskDataById(taskId).completionAim;//GetTaskXML().GetNode("task", "id==" + taskId).getUint("target_param2");

            }
            if (!IsInvoking("RunTimer"))
                InvokeRepeating("RunTimer", 0f, 1.0f);
        }

        float waitTime = 5f;
        private void RunTimer()
        {
            Dictionary<int, TaskData> dicTask = A3_TaskModel.getInstance().GetDicTaskData();
            List<int> idx = new List<int>(dicTask.Keys);
            bool isTiming = false;
            for (int i = 0; i < idx.Count; i++)
            {
                if (!taskOptElement.ContainsKey(idx[i])) continue;
                TaskOptElement tempElem = taskOptElement[idx[i]];
                float endTime = dicTask[idx[i]].lose_tm,
                    curTime = muNetCleint.instance.CurServerTimeStamp,
                    duration = dicTask[idx[i]].release_tm;
                TaskData task = dicTask[idx[i]];
                if (taskOptElement.ContainsKey(idx[i]))
                {
                    long timeSpn = task.lose_tm - muNetCleint.instance.CurServerTimeStamp;
                    bool isCurTiming = TaskTargetType.KILL_MONSTER_GIVEN == task.targetType && !task.isComplete && timeSpn > 0 && task.release_tm > timeSpn;
                    if (isCurTiming)
                    {
                        tempElem.liteMinimapTaskTimer.gameObject.SetActive(true);
                        tempElem.liteMinimapTaskTimer.text = GetSecByTime(task.lose_tm);
                        taskOptElement[tempElem.taskId].Set(isTaskMonsterAlive: true);
                    }
                    else
                    {
                        taskOptElement[tempElem.taskId].Set(isTaskMonsterAlive: false);
                        tempElem.liteMinimapTaskTimer.gameObject.SetActive(false);
                    }
                    isTiming |= isCurTiming;
                }
            }
            if (waitTime < 0)
            {
                CancelInvoke("RunTimer");
                waitTime = 5f;
                return;
            }
            if (!isTiming)
                waitTime--;
        }

        public bool LockStat = false;
        public void ResetStat()
        {
            //LockStat = false;
            IsOnKillMon = false;
            IsOnTaskWait = false;
        }
        void Update()
        {
            
            if (GRMap.grmap_loading || SelfRole.s_bInTransmit) { if (isWaiting) StopCD(); return; }
            if (SelfRole._inst == null  || a1_gamejoy.inst_joystick != null && a1_gamejoy.inst_joystick.moveing)
            {
                if (isWaiting) StopCD();
                Reset(alsoHideGameObject: true);
                return;
            }
            TaskData curTask = A3_TaskModel.getInstance().curTask;
            if (curTask == null) return;
            if (!LockStat)
            {
                IsOnKillMon = !curTask.isComplete && curTask.targetType == TaskTargetType.KILL_MONSTER_GIVEN;
                IsOnTaskWait = !curTask.isComplete && curTask.targetType == TaskTargetType.WAIT_POINT_GIVEN;
            }
            //if (IsOnKillMon || IsOnTaskWait)
            //{
            Vector3 curPos = new Vector3(SelfRole._inst.m_curModel.position.x, 0, SelfRole._inst.m_curModel.position.z);
            if (waitPosition != Vector3.zero)
                if (Vector3.Distance(curPos, waitPosition) > waitThresholdDistance)
                {
                    if (isWaiting)
                        StopCD();
                    Reset(alsoHideGameObject: true, resetCase: 1);
                }                
            if (killPosition != Vector3.zero)
                if (Vector3.Distance(curPos, killPosition) > killThresholdDistance)
                {
                    if (isWaiting)
                        StopCD();
                    Reset(alsoHideGameObject: true, resetCase: 2);
                }
            if (IsOnTaskWait)
            {
                if (waitPosition == Vector3.zero)
                    waitPosition = A3_TaskModel.getInstance().GetTaskTargetPos(curTask.taskId);
                if (waitPosition != Vector3.zero)
                {
                    if (Vector3.Distance(curPos, waitPosition) < waitThresholdDistance)
                    {
                        LockStat = true;
                        IsOnTaskWait = false;
                        //tfParentWait.gameObject.SetActive(true);
                        ArrayList arr = new ArrayList();
                        arr.Add(tfParentWait.gameObject);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NEWBIE, arr);//引导触发
                        OnWaitBtnClick(A3_TaskModel.getInstance().GetTaskDataById(curTaskId));
                        SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                        SelfRole.ChangeRideAniState( false);
                        if (SelfRole._inst.m_moveAgent.isActiveAndEnabled)
                            SelfRole._inst.m_moveAgent.Stop();
                        haveEnteredWaitPosition = true;
                    }
                    else if (haveEnteredWaitPosition)
                    {
                        Reset(alsoHideGameObject: true, resetCase: 2);
                        haveEnteredWaitPosition = false;
                    }
                }
            }
            if (IsOnKillMon && killPosition != Vector3.zero)
            {
                if (Vector3.Distance(curPos, killPosition) < killThresholdDistance)
                {
                    //IsOnKillMon = false;
                    int curTaskId = a3_task_auto.instance.executeTask?.taskId ?? A3_TaskModel.getInstance().curTask.taskId;
                    SelfRole.fsm.Stop();
                    if (taskOptElement.ContainsKey(curTaskId))
                    {
                        LockStat = true;
                        IsOnKillMon = false;
                        if (!taskOptElement[curTaskId].isTaskMonsterAlive)
                        {
                            winKillMon.SetActive(true);
                            OnWaitBtnClick1(A3_TaskModel.getInstance().GetTaskDataById(curTaskId));
                        }


                        //else
                        //    if (!SelfRole.fsm.Autofighting)
                        //        SelfRole.fsm.StartAutofight();
                    }
                    //if (SelfRole._inst.m_moveAgent.isActiveAndEnabled)
                }
                else
                    winKillMon.SetActive(false);
            }
            //}
        }

        private string GetSecByTime(long sec)
        {
            long currentTime = muNetCleint.instance.CurServerTimeStamp;
            long span = sec - currentTime;
            if (span > 0)
                //return string.Format("{0:D2}:{1:D2}", span % 3600 / 60, span % 60);
                return "[" + span.ToString() + "]";// string.Format("{0}", span);
            else
                return "";
        }
        public override void onShowed()
        {
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_TASK_AWARD, OnStopTimer);
            base.onShowed();
        }

        void OnStopTimer(GameEvent e)
        {
            if (e.data.ContainsKey("id"))
            {
                int taskId = e.data["id"];
                if (taskOptElement.ContainsKey(taskId))
                {
                    taskOptElement[taskId].Set(isTaskMonsterAlive: false);
                    taskOptElement[taskId].liteMinimapTaskTimer.gameObject?.SetActive(false);
                    taskOptElement.Remove(taskId);
                }
            }
        }
        //添加自动点击倒计时(搜寻)
        public void OnWaitBtnClick(TaskData taskData)
        {
            task_data = taskData;
            if (taskData.taskT == TaskType.ENTRUST|| taskData.taskT == TaskType.CLAN || taskData.taskT == TaskType.MAIN)
            {
                CancelInvoke("task_auto_click");
                Invoke("task_auto_click", 1.5f);
            }
        }
        TaskData task_data = new TaskData();
        void task_auto_click()
        {
            if(task_data!=null)
            {
                if(task_data.taskT == TaskType.MAIN && NewbieModel.getInstance().curItem != null && NewbieModel.getInstance().curItem.showing)
                {
                    return;
                }
            }

            if (BtnWait.interactable==true&& tfParentWait.gameObject.activeSelf&& imgProcess.fillAmount==0)
            {
               OnWaitBtnClick(BtnWait.gameObject);
            }
        }

        public   void OnWaitBtnClick(GameObject go)
        {
            SelfRole.fsm.Stop();
            timeWaitTerminal = (int)timeCD * 1000 + NetClient.instance.CurServerTimeStampMS;
            BtnWait.interactable = !(isWaiting = true);
            A3_TaskProxy.getInstance().SendWaitStart(curTaskId);
            if (timeCD > 0)
            {
                TaskData task = A3_TaskModel.getInstance().GetTaskDataById(curTaskId);
                StartCoroutine(RunCD());
            }
        }

        //添加自动点击倒计时（召唤boss）
        public void OnWaitBtnClick1(TaskData taskData)

        {
            //task_data = taskData;
            if (taskData.taskT == TaskType.ENTRUST)
            {
                CancelInvoke("task_auto_click1");
                Invoke("task_auto_click1", 2);
            }
        }
        void task_auto_click1()
        {
            //if (task_data != null)
            //{
            //    if (task_data.taskT == TaskType.MAIN && NewbieModel.getInstance().curItem != null && NewbieModel.getInstance().curItem.showing)
            //    {
            //        return;
            //    }
            //}
            if (winKillMon.activeSelf)
            {
                OnStartBtnClick(tfBtnStart.gameObject);
            }
        }
        void OnStartBtnClick(GameObject go)
        {
            if (curTaskId != 0 && taskOptElement[curTaskId].isTaskMonsterAlive)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_taskopt_monsteralive"));
                winKillMon.SetActive(false);
                return;
            }
            A3_TaskProxy.getInstance().SendCallMonster((uint)curTaskId);
            winKillMon.SetActive(false);
        }
        void OnCancelBtnClick(GameObject go) => Reset(alsoHideGameObject: true);
        public void ShowSubmitItem()
        {
            if (tfSubmitItemCon == null) return;
            tfSubmitItem.gameObject.SetActive(true);
            tfFocus.SetParent(tfSubmitItem, false);
            tfFocus.gameObject.SetActive(false);
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            int i = 0;
            for (i = tfSubmitItemCon.childCount; i > 0; i--)
                DestroyImmediate(tfSubmitItemCon.GetChild(i - 1).gameObject);
            List<uint> idx, availableItemSubmit = new List<uint>();
            for (idx = new List<uint>(items.Keys); i < idx.Count; i++)
                if (items[idx[i]].tpid == taskItemId)
                {
                    Transform tfIcon;
                    (tfIcon = IconImageMgr.getInstance().createA3ItemIcon(items[idx[i]].tpid, num: items[idx[i]].num, isicon: true).transform).SetParent(tfSubmitItemCon, false);
                    tfIcon.localScale = scaleIcon;
                    uint itemIId = items[idx[i]].id;
                    availableItemSubmit.Add(itemIId);
                    new BaseButton(tfIcon.GetComponentInChildren<Button>().transform).onClick = (go) =>
                    {
                        tfFocus.SetParent(go.transform, false);
                        tfFocus.gameObject.SetActive(true);
                        submitItemIId = itemIId;
                    };
                }

            if (tfSubmitItemCon.childCount > 0)
            {
                tfFocus.SetParent(tfSubmitItemCon.GetChild(0), false);
                tfFocus.gameObject.SetActive(true);
                submitItemIId = items[availableItemSubmit[0]].id;
                tfSubmitItemMainCon.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    tfSubmitItemCon.GetChild(0).GetComponent<RectTransform>().sizeDelta.x * (tfSubmitItemCon.childCount + 1),
                    tfSubmitItemCon.GetChild(0).GetComponent<RectTransform>().sizeDelta.y
                );
            }
        }

        public void Reset(bool alsoHideGameObject = false,int resetCase = 0)
        {
            switch (resetCase)
            {
                default: case 0:
                    waitPosition = Vector3.zero;                    
                    haveEnteredWaitPosition = false;
                    killPosition = Vector3.zero;
                    if (alsoHideGameObject)
                    {
                        tfParentWait.gameObject.SetActive(false);
                        winKillMon.SetActive(false);
                    }
                    break;
                case 1:
                    //waitPosition = Vector3.zero;
                    if (alsoHideGameObject)
                        tfParentWait.gameObject.SetActive(false);
                    break;
                case 2:
                    //haveEnteredWaitPosition = false;
                    //killPosition = Vector3.zero;
                    if (alsoHideGameObject)
                        winKillMon.SetActive(false);
                    break;
            }            
            //if (alsoHideGameObject)
            //{
            //    tfParentWait.gameObject.SetActive(false);
            //    winKillMon.SetActive(false);
            //}
        }
        public void HideSubmitItem()
        {
            if (tfSubmitItem.gameObject.activeSelf)
                tfSubmitItem.gameObject.SetActive(false);
        }
        private IEnumerator RunCD()
        {
            while (!SelfRole._inst.m_curAni.GetBool(EnumAni.ANI_RUN) /*移动*/ && !SelfRole.s_bInTransmit /*传送*/)
            {
                long timeCurrent = NetClient.instance.CurServerTimeStampMS;
                if (timeWaitTerminal < timeCurrent)
                {
                    tfParentWait.gameObject.SetActive(false);
                    //IsOnTaskWait = false;
                    StopCD(true);
                    if (A3_TaskModel.getInstance().curTask.showMessage)
                    {
                        string msg = string.Format(A3_TaskModel.getInstance().curTask.completionStr, A3_LegionModel.getInstance().myLegion.clname);
                        a3_chatroom._instance.SendMsg(msg);
                    }
                    yield break;
                }
                imgProcess.fillAmount = (timeCD * 1000 - (timeWaitTerminal - timeCurrent)) / (timeCD * 1000);
                yield return null;
            }
            imgProcess.fillAmount = 0;
        }
        public void StopCD(bool isFinish = false)
        {
            BtnWait.interactable = !(isWaiting = false);
            imgProcess.fillAmount = 0;
            StopCoroutine(RunCD());
            if (isFinish)
                A3_TaskProxy.getInstance().SendWaitEnd(curTaskId);
        }

    }
}
