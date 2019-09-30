using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class A3_EntrustOpt : FloatUi
    {
        private static float waitThresholdDistance = 3f;
        private static float killThresholdDistance = 3f;
        private uint submitItemIId;
        private float timeCD => A3_TaskModel.getInstance().GetEntrustTask().need_tm;
        private Transform tfParentWait;
        public Transform tfSubmitItem;
        private Transform tfSubmitItemCon;
        private Transform tfFocus;
        private bool haveEnteredWaitPosition;
        public uint entrustItemId;
        private Image imgProcess;
        private long timeWaitTerminal;
        private long timeKillTerminal;
        private GameObject winKillMon;
        public static A3_EntrustOpt Instance;
        public bool IsOnEntrustWait { get; set; }
        public bool isWaiting;
        public bool isOnKillMon { get; set; }
        public bool IsOnEntrustGet { get; set; }        
        public Vector3 waitPosition;        
        public Vector3 killPosition;    
        public BaseButton BtnWait;        
        public bool isEntrustMonsterAlive;
        public bool isKeepingKillMon;
        private Text liteMinimapEntrustTaskTip;        
        public Text LiteMinimapEntrustTaskTip
        {
            get {
                if (liteMinimapEntrustTaskTip == null)
                    liteMinimapEntrustTaskTip = a3_liteMinimap.instance.GetEntrustTaskPage()?.FindChild("name").GetComponent<Text>();
                return liteMinimapEntrustTaskTip;
            } set {
                liteMinimapEntrustTaskTip = value;
            }
        }
        private List<a3_BagItemData> bagItem = new List<a3_BagItemData>();
        public override void init()
        {
            Instance = this;
            tfParentWait = transform.FindChild("wait");
            tfParentWait.gameObject.SetActive(false);            
            imgProcess = tfParentWait.FindChild("waitBG").GetComponent<Image>();
            (BtnWait = new BaseButton(tfParentWait.FindChild("waitBG/btnDoWait"))).onClick = OnWaitBtnClick;
            winKillMon = transform.FindChild("killmon").gameObject;
            winKillMon.SetActive(false);
            tfSubmitItem = transform.FindChild("submitItem");
            tfSubmitItemCon = tfSubmitItem.FindChild("mask/scrollview/con");
            tfFocus = tfSubmitItem.FindChild("focus");
            tfFocus.gameObject.SetActive(false);
            tfSubmitItem.gameObject.SetActive(false);
            Transform tfBtnStart = winKillMon.transform.FindChild("btnStart");
            Transform tfBtnCancel = winKillMon.transform.FindChild("btnDontStart");
            new BaseButton(tfBtnStart).onClick = OnStartBtnClick;
            new BaseButton(tfBtnCancel).onClick = OnCancelBtnClick;
            new BaseButton(transform.FindChild("submitItem/closeBtn")).onClick = (btnClose) => tfSubmitItem.gameObject.SetActive(false);
            new BaseButton(tfSubmitItem.FindChild("btnOK")).onClick = (go) => 
            {
                if (submitItemIId != 0)
                    A3_TaskProxy.getInstance().SendSubmit(A3_TaskModel.getInstance().GetEntrustTask()?.taskId ?? 0, submitItemIId);
                tfSubmitItem.gameObject.SetActive(false);
            };
            transform.SetParent(skillbar.instance.transform);
                if(a3_liteMinimap.instance)
            LiteMinimapEntrustTaskTip = a3_liteMinimap.instance.GetEntrustTaskPage()?.GetComponent<Text>();
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnCheck);
            //A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_SYNC_TASK, OnCheck); //在服务器运行较缓慢时使用
            if (A3_TaskModel.getInstance().GetEntrustTask()?.lose_tm > muNetCleint.instance.CurServerTimeStamp)
            { 
                isKeepingKillMon =
                isEntrustMonsterAlive = true;
                timeKillTerminal = A3_TaskModel.getInstance().GetEntrustTask().lose_tm;
            }
            Instance.name = "A3_EntrustOpt";
        }
        private void OnCheck(GameEvent e)
        {            
            if (e.data.ContainsKey("change_task"))
            {
                List<Variant> task = e.data["change_task"]._arr;
                if (task[0].ContainsKey("id") && A3_TaskModel.getInstance().GetTaskDataById(task[0]["id"]).taskT == TaskType.ENTRUST)
                {                    
                    if (task[0].ContainsKey("cnt") && task[0]["cnt"] > 0)
                    {
                        isKeepingKillMon = false;
                        isEntrustMonsterAlive = false;
                        LiteMinimapEntrustTaskTip.text = A3_TaskModel.getInstance().GetEntrustTask().taskName;
                        return;
                    }
                    if (task.Count > 0 && A3_TaskModel.getInstance().GetTaskDataById(task[0]["id"]).targetType == TaskTargetType.KILL_MONSTER_GIVEN)
                    {
                        isKeepingKillMon = true;
                        isEntrustMonsterAlive = true;
                        timeKillTerminal = task[0]["lose_tm"];
                    }
                }
            }
            else if (e.data.ContainsKey("emis"))
            {
                int taskId = e.data["emis"]["id"];
                if (A3_TaskModel.getInstance().GetTaskDataById(taskId).targetType == TaskTargetType.KILL_MONSTER_GIVEN)
                {
                    isKeepingKillMon = true;
                    isEntrustMonsterAlive = true;
                    timeKillTerminal = e.data["emis"]["lose_tm"];
                }
                else
                    if (A3_TaskModel.getInstance().GetTaskDataById(taskId).targetType == TaskTargetType.GET_ITEM_GIVEN)
                        entrustItemId = A3_TaskModel.getInstance().GetTaskXML().GetNode("task", "id==" + taskId).getUint("target_param2");
            }
        }
        void Update()
        {
            if (joystick.instance.moveing)
            {
                if (isWaiting)
                    StopCD();
                Reset(alsoHideGameObject: true);
                return;
            }
            if (isOnKillMon || IsOnEntrustWait)
            {
                if (IsOnEntrustWait && waitPosition != Vector3.zero)
                {
                    if (Vector3.Distance(SelfRole._inst.m_curModel.position, waitPosition) < waitThresholdDistance)
                    {
                        tfParentWait.gameObject.SetActive(true);                        
                        SelfRole._inst.m_moveAgent.Stop();
                        haveEnteredWaitPosition = true;
                    }
                    else if (haveEnteredWaitPosition)
                    {
                        Reset(alsoHideGameObject: true);
                        haveEnteredWaitPosition = false;
                    }
                }
                else if (isOnKillMon && killPosition != Vector3.zero)
                {
                    if (Vector3.Distance(SelfRole._inst.m_curModel.position, killPosition) < killThresholdDistance)
                    {
                        isOnKillMon = false;
                        if (!isEntrustMonsterAlive)
                            winKillMon.SetActive(true);
                        SelfRole._inst.m_moveAgent.Stop();
                    }
                }
            }
            if (isKeepingKillMon)
            {
                if(A3_TaskModel.getInstance().GetEntrustTask()!=null)
                    if (timeKillTerminal < muNetCleint.instance.CurServerTimeStamp)
                        isEntrustMonsterAlive = false;
                    else if (LiteMinimapEntrustTaskTip != null)
                        LiteMinimapEntrustTaskTip.text = A3_TaskModel.getInstance().GetEntrustTask().taskName + GetSecByTime(timeKillTerminal);
            }

        }

        private string GetSecByTime(long sec)
        {
            long currentTime = muNetCleint.instance.CurServerTimeStamp;
            long span = sec - currentTime;
            if (span > 0)
                return string.Format("{0:D2}:{1:D2}", span % 3600 / 60, span % 60);
            else
                return "";
        }


        void OnWaitBtnClick(GameObject go)
        {
            timeWaitTerminal = (int)timeCD * 1000 + NetClient.instance.CurServerTimeStampMS;
            BtnWait.interactable = !(isWaiting = true);
            A3_TaskProxy.getInstance().SendWaitStart(A3_TaskModel.getInstance().GetEntrustTask().taskId);
            if (timeCD > 0)
                StartCoroutine(RunCD());
        }

        void OnStartBtnClick(GameObject go)
        {
            if (isEntrustMonsterAlive)
                return;
            A3_TaskProxy.getInstance().SendCallMonster((uint)A3_TaskModel.getInstance().GetEntrustTask().taskId);
            //timeKillTerminal = muNetCleint.instance.CurServerTimeStamp + A3_TaskModel.getInstance().GetEntrustTask().release_tm;
            winKillMon.SetActive(false);
        }
        void OnCancelBtnClick(GameObject go) => Reset(alsoHideGameObject: true);        
        public void ShowSubmitItem()
        {
            if (tfSubmitItemCon == null) return;
            tfSubmitItem.gameObject.SetActive(true);
            tfFocus.SetParent(tfSubmitItem,false);
            tfFocus.gameObject.SetActive(false);
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            int i = 0;
            for (i = tfSubmitItemCon.childCount; i > 0; i--)
                DestroyImmediate(tfSubmitItemCon.GetChild(i - 1).gameObject);
            List<uint> idx, availableItemSubmit = new List<uint>() ;
            for (idx = new List<uint>(items.Keys); i < idx.Count; i++)
                if (items[idx[i]].tpid == entrustItemId)
                {
                    Transform tfIcon;
                    (tfIcon = IconImageMgr.getInstance().createA3ItemIcon(items[idx[i]].tpid, num: items[idx[i]].num, isicon: true).transform).SetParent(tfSubmitItemCon, false);
                    uint itemIId = items[idx[i]].id;
                    availableItemSubmit.Add(itemIId);
                    new BaseButton(tfIcon.GetComponentInChildren<Button>().transform).onClick = (go) => 
                    {
                        tfFocus.SetParent(go.transform,false);
                        tfFocus.gameObject.SetActive(true);
                        submitItemIId = itemIId;
                    };                   
                }

            if (tfSubmitItemCon.childCount > 0)
            {               
                tfFocus.SetParent(tfSubmitItemCon.GetChild(0), false);
                tfFocus.gameObject.SetActive(true);
                submitItemIId = items[availableItemSubmit[0]].id;
            }
        }

        public void Reset(bool alsoHideGameObject = false)
        {
            waitPosition = Vector3.zero;
            IsOnEntrustWait = haveEnteredWaitPosition = false;
            killPosition = Vector3.zero;
            isOnKillMon = false;
            if (alsoHideGameObject)
            {
                tfParentWait.gameObject.SetActive(false);
                winKillMon.SetActive(false);
            }
        }
        private IEnumerator RunCD()
        {
            while (!SelfRole._inst.m_curAni.GetBool(EnumAni.ANI_RUN) /*移动*/ && !SelfRole.s_bInTransmit /*传送*/)
            {
                long timeCurrent = NetClient.instance.CurServerTimeStampMS;
                if (timeWaitTerminal < timeCurrent)
                {
                    tfParentWait.gameObject.SetActive(false);
                    IsOnEntrustWait = false;
                    StopCD(true);
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
                A3_TaskProxy.getInstance().SendWaitEnd(A3_TaskModel.getInstance().GetEntrustTask().taskId);
        }

    }
}
