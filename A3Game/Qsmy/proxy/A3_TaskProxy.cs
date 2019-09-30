using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
namespace MuGame
{
    class A3_TaskProxy : BaseProxy<A3_TaskProxy>
    {
        private uint GET_TASK_INFO = 1;
        private uint GET_TASK_AWARD = 2;
        private uint TASK_DIALOG = 3;
        private uint ACCEPT_TASK = 4;
        public uint COLLECT_TARGET = 5;
        private uint ONEKEY_FINISH = 6;
        private uint UPGRADE_STAR = 7;
        private uint SUBMIT_ITEM = 8;
        private uint WAIT_START = 9;
        private uint WAIT_END = 10;
        private uint CALL_MONSTER = 11;

        public const uint ON_SYNC_TASK = 0;
        public const uint ON_GET_TASK_AWARD = 1;
        public const uint ON_GET_NEW_TASK = 2;
        public const uint ON_TASK_REFRESH = 3;
        public const uint ON_TASK_STAR_CHANGE = 4;

        public A3_TaskProxy()
        {
            addProxyListener(PKG_NAME.S2C_TASK_RES, OnTask);
        }

        #region client to server

        public void SendGetTask()
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = GET_TASK_INFO;

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        public void SendGetAward(int rate = 0)
        {
            int id = A3_TaskModel.getInstance().curTask.taskId;

            debug.Log("完成任务ID::" + id);

            Variant msg = new Variant();
            msg["mis_cmd"] = GET_TASK_AWARD;
            msg["id"] = id;
            if (rate > 1)
            {
                msg["double_exp"] = true;
            }

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        public void SendTalkWithNpc(int npcId)
        {
            debug.Log("发送对话ID::" + npcId);

            Variant msg = new Variant();
            msg["mis_cmd"] = TASK_DIALOG;
            msg["npcid"] = npcId;

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        public void SendAcceptTask()
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = ACCEPT_TASK;

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        //一键完成任务
        public void OneKeyFinishTask()
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = ONEKEY_FINISH;

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        //一键升星
        public void SendUpgradeStar()
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = UPGRADE_STAR;

            sendRPC(PKG_NAME.C2S_TASK, msg);
        }

        //召唤怪物
        public void SendCallMonster(uint id)
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = CALL_MONSTER;
            msg["id"] = id;
            sendRPC(PKG_NAME.C2S_TASK, msg);
            if (!SelfRole.fsm.Autofighting)
            {
                SelfRole.fsm.StartAutofight();
                SelfRole.fsm.ClearAutoConfig();
                flytxt.flyUseContId("autoplay_start");
            }
        }

        public void SendWaitStart(int id)
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = WAIT_START;
            msg["id"] = (uint)id;
            sendRPC(PKG_NAME.C2S_TASK, msg);
        }
        public void SendWaitEnd(int id)
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = WAIT_END; 
            msg["id"] = (uint)id; 
            sendRPC(PKG_NAME.C2S_TASK, msg);
        }
        public void SendSubmit(int taskId, uint itemIId)
        {
            Variant msg = new Variant();
            msg["mis_cmd"] = SUBMIT_ITEM;
            msg["id"] = taskId;
            msg["item_id"] = itemIId;
            sendRPC(PKG_NAME.C2S_TASK, msg);
        }
        #endregion


        public bool showFirst = false;

        #region server to client callback

        private void OnTask(Variant data)
        {
            //data.RemoveKey("cmis");
            debug.Log("任务::" + data.dump());
            if (SelfRole._inst != null)
                SelfRole._inst.m_LockRole = null;
            if (data.ContainsKey("res"))
            {
                int res = data["res"];
                switch (res)
                {
                    case 1:
                        OnSyncTaskInfo(data);
                        FunctionOpenMgr.instance.onFinshedMainTask(A3_TaskModel.getInstance().main_task_id);
                        dispatchEvent(GameEvent.alloc(ON_SYNC_TASK, this, data));
                        break;
                    case 2:
                        OnGetTaskAward(data);
                        dispatchEvent(GameEvent.alloc(ON_GET_TASK_AWARD, this, data));
                        break;
                    case 5:

                        break;
                    case 7:
                        OnAddTaskInfo(data);

                        dispatchEvent(GameEvent.alloc(ON_GET_NEW_TASK, this, data));

                        if (data.ContainsKey ("mlmis")) {
                            if (data["mlmis"].ContainsKey ("id")) {
                                int id = XMLMgr.instance.GetSXML("welfare").GetNode("newbie").getInt("id");

                                if (data["mlmis"].ContainsKey("id")) {
                                    if (data["mlmis"]["id"] == id  && showFirst) {
                                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr .A3_FIRESTRECHARGEAWARD);
                                    }
                                }
                            }
                        }
                        break;
                    case 8:                        
                        OnRefreshTaskCount(data);
                        dispatchEvent(GameEvent.alloc(ON_TASK_REFRESH, this, data));
                        break;
                    default:
                        Globle.err_output(res);
                        break;
                }
            }
            if (data.ContainsKey("mlmis"))
            {
                A3_TaskModel.getInstance().main_task_id = data["mlmis"]["id"];
            }
            //FunctionOpenMgr.instance.onFinshedMainTask(A3_TaskModel.getInstance().main_task_id);
        }

        //同步任务信息
        private void OnSyncTaskInfo(Variant data)
        {
            A3_TaskModel.getInstance().OnSyncTask(data);
            if (a3_liteMinimap.instance?.transTask.FindChild("skin/view/con").childCount == 0 || a3_liteMinimap.instance?.TaskType_objs.Count == 0) //应对于服务器运作缓慢时
                a3_liteMinimap.instance.InitTaskInfo();

        }

        //领取任务奖励
        private void OnGetTaskAward(Variant data)
        {
            A3_TaskModel.getInstance().OnSubmitTask(data);
        }

        //获得新任务
        private void OnAddTaskInfo(Variant data)
        {
            A3_TaskModel.getInstance().OnAddTask(data);
        }

        //变更任务
        private void OnRefreshTaskCount(Variant data)
        {
            A3_TaskModel.getInstance().RefreshTask(data);
        }
        #endregion
    }
}
