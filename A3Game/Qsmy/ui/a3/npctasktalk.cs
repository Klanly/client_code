using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;
using System.Collections;

namespace MuGame
{

    class npctasktalk : npctalk
    {
        private Transform conIcon;
        private Transform conOption;
        private GameObject optionTemp;
        private GameObject iconTemp;
        //public List<Transform> lUiPos = new List<Transform>();
        private BaseButton btnNext;
        private BaseButton btnExit;
        private TaskData curTask
        {
            get {
                if (a3_task_auto.instance.executeTask != null)
                    return a3_task_auto.instance.executeTask;
                else
                    return A3_TaskModel.getInstance().curTask;
            }
            set { A3_TaskModel.getInstance().curTask = value; }
        }
        Transform transTalk;
        private A3_TaskModel tkModel;
		Transform bg_task;
		Image headicon;

        public override void OnInit()
        {
			headicon = this.getTransformByPath("talk/npc_head/icon").GetComponent<Image>();
			bg_task = this.getTransformByPath("talk/bg_task");
            tkModel = A3_TaskModel.getInstance();
            //lUiPos.Add(getTransformByPath("con1"));
            //lUiPos.Add(getTransformByPath("con0"));
            conIcon = this.getTransformByPath("talk/con_icon");
			conOption = this.getTransformByPath("talk/bg_taskselect/con_options");
            optionTemp = this.getGameObjectByPath("talk/optionTemp");
            iconTemp = this.getGameObjectByPath("talk/iconTemp");
            transTalk = getTransformByPath("talk");
			btnNext = new BaseButton(this.getTransformByPath("talk/bg_task/next"));
            btnExit = new BaseButton(this.getTransformByPath("talk/close"));
            btnExit.onClick = OnCloseDialog;
			new BaseButton(this.getTransformByPath("talk/fg")).onClick = OnCloseDialog;

            base.OnInit();
            getComponentByPath<Text>("talk/bg_task/next/Text").text = ContMgr.getCont("ToSure_summon_2");
        }

        //刷新界面
        public override void refreshView()
        {
            EventTriggerListener.Get(fg).clearAllListener();

			base.txtDesc.text = dialog.curDesc;
			if (dialog.curType == "2") {
				headicon.sprite = GAMEAPI.ABUI_LoadSprite("icon_npctask_" + PlayerModel.getInstance().profession);
			}
			else {
				var iconid = XMLMgr.instance.GetSXML("npcs.npc", "id==" + dialog.m_npc.id).getUint("head_icon");
				var rl = GAMEAPI.ABUI_LoadSprite("icon_npctask_" + iconid);
				if (rl == null) rl = GAMEAPI.ABUI_LoadSprite("icon_npctask_" + 101);
				headicon.sprite = rl;
			}
            if (dialog.curType == "-1")
            {
                OnShowOptionUi();
                ShowNextBtn();
            }
            else
            {
                OnShowTaskUi();
            }
        }

        //刷新位置
        public override void refreshPos()
        {
            if (dialog.curType == "2")
            {
                base.txtName.text = PlayerModel.getInstance().name;
                //dialog.instance.showRole(true);
                //transTalk.position = lUiPos[0].position;
            }
            else
            {
                base.txtName.text = dialog.m_npc.name;
                //dialog.instance.showRole(false);
                //transTalk.position = lUiPos[1].position;
            }
        }

        //关闭处理
        public override void OnClosedProcess()
        {
            IsNpcTalk = false;
            if (conIcon.childCount > 0)
            {
                for (int i = 0; i < conIcon.childCount; i++)
                {
                    Destroy(conIcon.GetChild(i).gameObject);
                }
            }
            if (conOption.childCount > 0)
            {
                for (int i = 0; i < conOption.childCount; i++)
                {
                    Destroy(conOption.GetChild(i).gameObject);
                }
            }

            //if (hasWait)
            //{
            //    A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            //}
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
            base.OnClosedProcess();
        }
        
        //返回主界面
        private void OnCloseDialog(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
            A3_TaskOpt.Instance.HideSubmitItem();
        }

        #region 任务界面

        private bool hasWait = false;
        public static bool IsNpcTalk = false;
        //显示任务界面
        private void OnShowTaskUi()
        {
            IsNpcTalk = true;
            if (curTask.isComplete)
            {
                ShowTask();
            }
            else
            {
                //未完成时的显示奖励
                conIcon.gameObject.SetActive(true);
                OnShowAwardInfo();
                if (/*tkModel.*/curTask.targetType == TaskTargetType.VISIT)
                {
                    A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_TASK_REFRESH, OnRefreshTask);
                    //hasWait = true;
                    //InterfaceMgr.getInstance().open(InterfaceMgr.WAIT_LOADING);
                    //dialog
                    A3_TaskProxy.getInstance().SendTalkWithNpc(dialog.m_npc.id);
                }
                if (/*tkModel.*/curTask.targetType == TaskTargetType.GETEXP )
                {
                    SXML taskXml = XMLMgr.instance.GetSXML("task.Task", "id==" + /*tkModel.*/curTask.taskId);
                    int type = int.Parse(taskXml.getString("trigger"));
                    if(type == 2)
                    {
                        btnNext.interactable = true;
                        btnNext.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk0");
                        btnNext.onClick = OnOpenZhuan;
                    }
                }
                if (curTask.targetType == TaskTargetType.GET_ITEM_GIVEN)
                {
                    A3_TaskOpt.Instance.taskItemId = (uint)/*tkModel.*/curTask.completionAim;
                    btnNext.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk1");                   
                    btnNext.onClick += (go) =>
                    {
                        int needNum = curTask.completion - curTask.taskRate;
                        int holdNum = a3_BagModel.getInstance().getItemNumByTpid(A3_TaskOpt.Instance.taskItemId);
                        if (holdNum < needNum)
                        {
                            InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
                            ArrayList data = new ArrayList();
                            data.Add(a3_BagModel.getInstance().getItemDataById(A3_TaskOpt.Instance.taskItemId));                            
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data);
                            A3_TaskOpt.Instance.HideSubmitItem();
                        }                         
                        else
                            A3_TaskOpt.Instance?.ShowSubmitItem();
                    };
                }
               
            }
        }

        private void OnRefreshTask(GameEvent e)
        {            
            if (curTask.isComplete)
            {
                ShowTask();
            }
        }

        //显示任务界面
        private void ShowTask()
        {
            //if (hasWait)
            //{
            //    InterfaceMgr.getInstance().close(InterfaceMgr.WAIT_LOADING);
            //}
            conIcon.gameObject.SetActive(true);
            conOption.parent.gameObject.SetActive(false);
			bg_task.gameObject.SetActive(false);

            if (dialog.isLastDesc)
            {
                OnShowAwardInfo();
                ShowCompleteBtn();
            }
            else
            {
                OnShowAwardInfo();
                ShowNextBtn();
            }
            if (curTask.taskT == TaskType.ENTRUST|| curTask.taskT == TaskType.CLAN || curTask.taskT == TaskType.MAIN)
            {
                CancelInvoke("task_auto_click");
                Invoke("task_auto_click", 2);
            }
        }

        void task_auto_click()
        {
            if (dialog.instance != null && btnNext.transform != null && btnNext.interactable == true)
                btnNext.doClick();
        }

        //显示任务按钮
        private void ShowCompleteBtn()
        {
            int taskId = /*tkModel.*/curTask.taskId;

            Text text = btnNext.transform.FindChild("Text").GetComponent<Text>();

            NpcTaskState nState = tkModel.GetTaskState(taskId);
            switch (nState)
            {
                case NpcTaskState.NONE:
                    break;
                case NpcTaskState.UNREACHED:
                    text.text = ContMgr.getCont("npctasktalk2");
                    btnNext.interactable = false;
                    break;
                case NpcTaskState.REACHED:
                    text.text = ContMgr.getCont("npctasktalk2");
                    btnNext.interactable = true;
                    btnNext.onClick = OnAcceptTask;
                    break;
                case NpcTaskState.UNFINISHED:
                    text.text = ContMgr.getCont("npctasktalk3");
                    btnNext.interactable = false;
                    break;
                case NpcTaskState.FINISHED:
                    text.text = ContMgr.getCont("npctasktalk3");
                    btnNext.interactable = true;
                    btnNext.onClick = OnSubmitTask;
                    break;
                default:
                    break;
            }
        }

        private void ShowNextBtn()
        {
            btnNext.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk4");
                btnNext.onClick = base.onClick;
                btnNext.interactable = true;
           

        }

        //显示奖励信息
        private void OnShowAwardInfo()
        {
            //TODO 假如有物品, 则创icon
            bg_task.gameObject.SetActive(true);
            conOption.parent.gameObject.SetActive(false);
            //int selectId = tkModel.SelectTaskId;
            TaskData taskData = /*tkModel.*/curTask;
            Dictionary<uint, int> dicValueReward;
            switch (taskData.taskT)
            {
                case TaskType.CLAN:
                    dicValueReward = tkModel.GetClanRewardDic(taskData.taskCount);
                    break;
                default:
                    dicValueReward = tkModel.GetValueReward(taskData.taskId);
                    break;
            }
            Dictionary<uint, int> dicItemReward = tkModel.GetItemReward(taskData.taskId);
            List<a3_BagItemData> listEquipReward = tkModel.GetEquipReward(taskData.taskId);

            if (taskData.guide)
                btnNext.transform.FindChild("guide_task_info").gameObject.SetActive(true);
            else
                btnNext.transform.FindChild("guide_task_info").gameObject.SetActive(false);

            if (dicValueReward != null) {
				if (conIcon.childCount > 0) {
					for (int i = 0; i < conIcon.childCount; i++) {
						Destroy(conIcon.GetChild(i).gameObject);
					}
				}
                foreach (var v in dicValueReward.Keys) {

                    a3_ItemData item = a3_BagModel.getInstance().getItemDataById(v);
                    switch (taskData.taskT)
                    {
                        default:
                            item.file = "icon_comm_0x" + v;
                            break;
                        case TaskType.CLAN:
                            item.file = "icon_comm_1x" + v;
                            break;
                    }   
					GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item, false, dicValueReward[v], 0.8f);
					var bd = icon.transform.FindChild("iconbor");
					if (bd != null) bd.gameObject.SetActive(false);

					GameObject rootClon = GameObject.Instantiate(iconTemp) as GameObject;

					icon.transform.SetParent(rootClon.transform.FindChild("icon"), false);
					rootClon.transform.SetParent(conIcon, false);

					rootClon.name = item.tpid.ToString();
					rootClon.SetActive(true);

					var iimg = icon.GetComponent<Image>();
					if (iimg != null) iimg.enabled = false;

				}
			}

            if (listEquipReward != null)
            {
                foreach (a3_BagItemData v in listEquipReward)
                {
                    if (!v.isEquip)
                        continue;

					GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(v.id, false, -1, 0.8f);
					var bd = icon.transform.FindChild("iconborder");
					if (bd != null) bd.gameObject.SetActive(false);
                    GameObject rootClon = GameObject.Instantiate(iconTemp) as GameObject;

                    icon.transform.SetParent(rootClon.transform.FindChild("icon"), false);
                    rootClon.transform.SetParent(conIcon, false);

                    rootClon.name = v.tpid.ToString();
                    rootClon.SetActive(true);

					var iimg = icon.GetComponent<Image>();
					if (iimg != null) iimg.enabled = false;
                }
            }

            if (dicItemReward != null)
            {
                foreach (uint key in dicItemReward.Keys)
                {
                    int num = dicItemReward[key];
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(
                        key, true, num, 0.8F, false);
					var bd = icon.transform.FindChild("iconborder");
					if (bd != null) bd.gameObject.SetActive(false);

                    GameObject rootClon = GameObject.Instantiate(iconTemp) as GameObject;

                    icon.transform.SetParent(rootClon.transform.FindChild("icon"), false);
                    rootClon.transform.SetParent(conIcon, false);

                    rootClon.name = key.ToString();
                    rootClon.SetActive(true);

					var iimg = icon.GetComponent<Image>();
					if (iimg != null) iimg.enabled = false;
                }
            }
        }

        //提交任务
        private void OnSubmitTask(GameObject go)
        {
            btnNext.interactable = false;
            A3_TaskProxy.getInstance().SendGetAward();
            if (!/*tkModel.*/curTask.story_hint.Equals("null"))
                flytxt.instance.fly(/*tkModel.*/curTask.story_hint, 8);
            dialog.next();
        }

        //接收任务
        private void OnAcceptTask(GameObject go)
        {
            btnNext.interactable = false;
            //TODO 判断任务是否可以领取
            debug.Log("领取任务");
            dialog.next();
        }

        //打开转生界面
        private void OnOpenZhuan(GameObject go)
        {
            btnNext.interactable = false;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RESETLVL);
            InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
            dialog.next();
        }


        #endregion

        #region 功能界面

        //显示功能界面
        private void OnShowOptionUi()
        {
            conIcon.gameObject.SetActive(false);
            conOption.parent.gameObject.SetActive(true);
			bg_task.gameObject.SetActive(false);

            NpcRole npc = dialog.m_npc;
            List<int> listtid = npc.listTaskId;
            if (listtid != null)
            {
                for (int i = 0; i < listtid.Count; i++)
                {
                    int id = listtid[i];
                    string name = tkModel.GetTaskDataById(id).taskName;
                    bool isMain = tkModel.GetTaskDataById(id).taskT == TaskType.MAIN;
                    GameObject btnClon = Instantiate(optionTemp) as GameObject;
                    //if (isMain)
                    //else
                    switch (tkModel.GetTaskDataById(id).taskT)
                    {
                        case TaskType.MAIN: btnClon.transform.FindChild("sign/main").gameObject.SetActive(true); break;
                        case TaskType.BRANCH: btnClon.transform.FindChild("sign/branch").gameObject.SetActive(true); break;
                        case TaskType.CLAN: btnClon.transform.FindChild("sign/clan").gameObject.SetActive(true); break;
                        case TaskType.ENTRUST: btnClon.transform.FindChild("sign/entrust").gameObject.SetActive(true); break;
                    }
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = name;
                    
                    btnClon.transform.SetParent(conOption, false);
                    btnClon.SetActive(true);

                    BaseButton btn = new BaseButton(btnClon.transform);
                    btn.onClick = OnOptionBtnClick;
                    btn.gameObject.name = id.ToString();
                 
                }
            }

            if (npc.openid != "")
            {
                GameObject btnClon = Instantiate(optionTemp) as GameObject;
                btnClon.transform.FindChild("sign/func").gameObject.SetActive(true);

                if (npc.openid == "a3_warehouse") {
					btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk5");
				}
                
                if (npc.openid == "A3_FindBesto") {
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk6");
                }

                if (npc.openid == "a3_resetlvl")
                {
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk0");
                }

                if (npc.openid == "A3_Smithy")
                {
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk7");
                }
                if (npc.openid == "a3_npc_shop")
                {
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk8");
                    npc_id = npc.id;
                }
                if (npc.openid=="a3_legion_dart")
                {
                    btnClon.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("npctasktalk9");
                }

                btnClon.transform.SetParent(conOption, false);
                btnClon.SetActive(true);
                BaseButton btn = new BaseButton(btnClon.transform);
                btn.onClick = OnOptionBtnClick;
                btn.gameObject.name = npc.openid;
            }
        }
        int npc_id;
        //选择功能按钮
        private void OnOptionBtnClick(GameObject go)
        {
            dialog.next();
            
            int taskId = 0;
            if (int.TryParse(go.name, out taskId))
            {
                List<string> ldesc = tkModel.GetDialogkDesc(taskId);
                tkModel.curTask = tkModel.GetTaskDataById(taskId);

                dialog.showTalk(ldesc, null, dialog.m_npc);

                //OnCompleteTalk();
            }
            else
            {
                string opName = go.name;
                ArrayList lst = new ArrayList();
                if (opName== "a3_npc_shop")
               {
                    
                    lst.Add(npc_id);
                   // List<SXML> listNPCShop= XMLMgr.instance.GetSXMLList("npc_shop.npc_shop", "npc_id==" + npc_id);
                    //A3_NPCShopModel.getInstance().listNPCShop.Clear();
                    //A3_NPCShopModel.getInstance().listNPCShop = listNPCShop;
                    //A3_NPCShopProxy.getInstance().sendShowFloat((uint)listNPCShop[0].getInt("shop_id"));
               }
                if (opName== "a3_legi  on_dart")
                {
                    if (A3_LegionModel.getInstance().myLegion.id == 0)
                    {
                        flytxt.instance.fly(ContMgr.getCont("npctasktalk10"));
                        return;
                    }
                }
                if(opName == "a3_npc_shop")
				    InterfaceMgr.getInstance().ui_async_open(opName,lst);
                else
                    InterfaceMgr.getInstance().ui_async_open(opName);
                //List<string> ldesc = new List<string>() { "1:让我准备准备" };
                //dialog.showTalk(ldesc, 
                //    () => InterfaceMgr.getInstance().open(opName), 
                //    dialog.m_npc);
            }
        }

        #endregion
    }

}
