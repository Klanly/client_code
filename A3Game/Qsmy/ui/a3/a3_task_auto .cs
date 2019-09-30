using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using MuGame;
namespace MuGame
{
    class a3_task_auto
    {
        public static a3_task_auto instance = new a3_task_auto();

        //public bool AutoTask = false;

        public bool stopAuto = false;
        public bool onTaskSearchMon = false;
        public bool bDoClanTask = false;
        public TaskData executeTask = null;
        private int tarNpcId = 0;
        public void RunTask(TaskData taskData = null, bool checkNextStep = false, bool checkItem = false, bool clickAuto = false)
        {
            if (stopAuto)
                return;
            //新手指引屏蔽自动寻路任务
            if (NewbieModel.getInstance().curItem != null && NewbieModel.getInstance().curItem.showing)
            {
                return;
            }
              

            if (taskData == null && A3_TaskModel.getInstance().main_task_id > 0)
                taskData = A3_TaskModel.getInstance().GetTaskDataById(A3_TaskModel.getInstance().main_task_id);
            if (taskData == null)
                return;
            A3_TaskModel.getInstance().curTask = taskData;
            executeTask = taskData;
            if (executeTask == null)
                return;


            //clickAuto为点击前往。必须执行
            Execute(executeTask, checkNextStep, checkItem, clickAuto);
        }


        private bool Execute(TaskData taskData, bool checkNextStep, bool checkItems ,bool clickAuto)
        {
            if (taskData.taskT == TaskType.CLAN && A3_LegionModel.getInstance().myLegion.id == 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_task_auto_nocy"));
                return false;
            }
            //这里将任务的自动改成只读配置表（待测试）
            bool forceMove = taskData.next_step;

            if (forceMove || clickAuto)
            {
                if (SelfRole.fsm.Autofighting)
                {
                    SelfRole.fsm.Stop();
                    StateInit.Instance.Origin = Vector3.zero;
                }
            }
            else
                return false;

            int npcId = 0;
            int mapId = 0;
            int posX = 0;
            int posY = 0;


            SXML taskXml = XMLMgr.instance.GetSXML("task.Task", "id==" + taskData.taskId);
            if (taskXml == null)
            {
                debug.Log("任务Id错误::" + taskData.taskId);
                return false;
            }
            if (taskData.isComplete && taskData.taskT != TaskType.DAILY)
            {
                if (taskXml.getInt("complete_way") == 3)
                {
                    A3_TaskProxy.getInstance().SendGetAward();
                    return true;
                }
                npcId = taskXml.getInt("complete_npc_id");
                SXML npcsXml = XMLMgr.instance.GetSXML("npcs.npc", "id==" + npcId);
                if (npcsXml != null)
                {
                    mapId = npcsXml.getInt("map_id");
                }
                List<string> listDialog = new List<string>();

                string strDialog = taskXml.getString("complete_dialog");
                strDialog = GameFramework.StringUtils.formatText(strDialog);
                string[] listTempDia = strDialog.Split(';');
                listDialog = listTempDia.ToList<string>();

                tarNpcId = npcId;
                //if(forceMove)
                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = mapId,
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc)
                //});
                if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                    SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true),taskTrans:true);
                else
                    SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true);
            }
            else
            {
                mapId = taskXml.getInt("tasking_map_id");
                posX = taskXml.getInt("target_coordinate_x");
                posY = taskXml.getInt("target_coordinate_y");
                float mapY = taskXml.getFloat("map_y");
                mapY = (mapY == -1 ? 0 : mapY);
                Vector3 pos = new Vector3(posX, mapY, posY);
                switch (taskData.targetType)
                {
                    case TaskTargetType.DODAILY: /*页面切换到每日任务*/
                        ArrayList arr = new ArrayList();
                        var tasks = A3_TaskModel.getInstance().GetDicTaskData();
                        bool hasDailyTask = false;
                        int i = 0;
                        for (List<int> idx = new List<int>(tasks.Keys); i < tasks.Count; i++)
                            if (hasDailyTask = tasks[idx[i]].taskT == TaskType.DAILY)
                                break;
                        if (hasDailyTask/*tasks.Count(v => v.Value.taskT == TaskType.DAILY) > 0*/)
                        {
                            List<int> k = tasks.Keys.ToList();
                            for (i = 0; i < k.Count; i++)
                                if (tasks[k[i]].taskT == TaskType.DAILY)
                                    arr.Add(tasks[k[i]].taskId);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK, arr);
                            //MonsterMgr._inst.taskMonId = XMLMgr.instance.GetSXML("task.Task", "id==" + taskData.taskId)?.getInt("target_param2") ?? 0;
                        }
                        break;
                    case TaskTargetType.FRIEND: /*关闭当前页面,弹出好友页面*/
                        if (a3_liteMinimap.instance)
                            a3_liteMinimap.instance.ZidongTask = false;
                        arr = new ArrayList();
                        arr.Add(1 /* index of friend panel*/ );
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arr);
                        break;
                    case TaskTargetType.WING: /*关闭当前页面,弹出飞翼页面*/
                        if (a3_liteMinimap.instance)
                            a3_liteMinimap.instance.ZidongTask = false;
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WIBG_SKIN);
                        break;

                    case TaskTargetType.GETEXP:/*卡级任务、转生任务*/

                        debug.Log("当前是转生等级任务");
                        StateAutoMoveToPos.Instance.stopdistance = 0.3f;
                        int zs = int.Parse(taskXml.getString("target_param2").Split(',')[0]);
                        int ls = int.Parse(taskXml.getString("target_param2").Split(',')[1]);
                        int trriger_type = int.Parse(taskXml.getString("trigger"));
                        if (trriger_type == 1)
                        {
                            if (a3_liteMinimap.instance)
                                a3_liteMinimap.instance.ZidongTask = false;
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WANTLVUP);
                        }
                        else
                        {
                            bool b = true;
                            int pp = PlayerModel.getInstance().profession;
                            uint pl = PlayerModel.getInstance().lvl;
                            uint pz = PlayerModel.getInstance().up_lvl;
                            uint exp = PlayerModel.getInstance().exp;
                            uint needExp = ResetLvLModel.getInstance().getNeedExpByCurrentZhuan(pp, pz);
                            uint needLvL = ResetLvLModel.getInstance().getNeedLvLByCurrentZhuan(pp, pz);
                            if (pz >= 10) break;//10转之后无法再次转生
                            if (needLvL > pl) b = false;
                            if (b)
                            {
                                npcId = XMLMgr.instance.GetSXML("task.zhuan_npc").getInt("id");
                                SXML npcsXml = XMLMgr.instance.GetSXML("npcs.npc", "id==" + npcId);
                                if (npcsXml != null)
                                {
                                    mapId = npcsXml.getInt("map_id");
                                }

                                List<string> listDialog = new List<string>();

                                string strDialog = taskXml.getString("target_dialog");
                                strDialog = GameFramework.StringUtils.formatText(strDialog);
                                string[] listTempDia = strDialog.Split(';');
                                listDialog = listTempDia.ToList<string>();

                                //if (GRMap.instance != null)
                                //{
                                //    InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                                //    {
                                //        mapId = mapId,
                                //        check_beforeShow = true,
                                //        handle_customized_afterTransmit = () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc)
                                //    });
                                //}
                                if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                                    SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true),taskTrans:true);
                                else
                                    SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true);
                                //InterfaceMgr.getInstance().open(InterfaceMgr.A3_RESETLVL);
                            }
                            else
                            {
                                if (a3_liteMinimap.instance)
                                    a3_liteMinimap.instance.ZidongTask = false;
                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WANTLVUP);
                            }
                        }
                        break;
                    case TaskTargetType.VISIT:/*访问（无条件）*/
                        {
                            StateAutoMoveToPos.Instance.stopdistance = 2f;
                            npcId = taskXml.getInt("target_param2");
                            SXML npcsXml = XMLMgr.instance.GetSXML("npcs.npc", "id==" + npcId);
                            if (npcsXml != null)
                            {
                                mapId = npcsXml.getInt("map_id");
                            }

                            List<string> listDialog = new List<string>();
                            string strDialog = taskXml.getString("target_dialog");
                            strDialog = GameFramework.StringUtils.formatText(strDialog);
                            string[] listTempDia = strDialog.Split(';');
                            listDialog = listTempDia.ToList<string>();

                            tarNpcId = npcId;
                            if (GRMap.instance != null)
                            {
                                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                                //{
                                //    mapId = mapId,
                                //    check_beforeShow = true,
                                //    handle_customized_afterTransmit = () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc)
                                //});
                                if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                                    SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true),taskTrans:true);
                                else
                                    SelfRole.moveToNPc(mapId, npcId, listDialog, OnTalkWithNpc,taskmove:true);
                            }
                        }
                        break;

                    case TaskTargetType.KILL:/*杀怪类*/
                        //StateAutoMoveToPos.Instance.stopdistance = 2.0f;
                        SelfRole.UnderTaskAutoMove = true;
                        onTaskSearchMon = taskData.taskT == TaskType.MAIN;
                        int _taskId;
                        if (PlayerModel.getInstance().task_monsterId.ContainsKey(taskData.taskId))
                        {
                            if (!PlayerModel.getInstance().task_monsterIdOnAttack.ContainsKey(taskData.taskId))
                            {
                                PlayerModel.getInstance().task_monsterIdOnAttack.Add(
                                    key: taskData.taskId,
                                    value: PlayerModel.getInstance().task_monsterId[taskData.taskId]
                                );
                            }
                            PlayerModel.getInstance().task_monsterId.Remove(taskData.taskId);
                            _taskId = taskData.taskId;
                        }
                        else
                        {
                            _taskId = A3_TaskModel.getInstance().GetTaskXML().GetNode("Task", "id==" + taskData.taskId).getInt("target_param2");
                            PlayerModel.getInstance().task_monsterIdOnAttack.Add(
                                key: taskData.taskId,
                                value: _taskId
                            );
                        }
                        SXML _taskXml = XMLMgr.instance.GetSXML("task.Task", "id==" + taskData.taskId);
                        //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                        //{
                        //    mapId = mapId,
                        //    check_beforeShow = true,
                        //    handle_customized_afterTransmit = () =>
                        //    {
                        //        Vector3 fightOrigin = pos;
                        //        StateInit.Instance.Origin = fightOrigin;
                        //        SelfRole.moveto(mapId, pos, () =>
                        //        {
                        //            SelfRole.fsm.StartAutofight();
                        //            MonsterMgr._inst.taskMonId = _taskXml.getInt("target_param2");
                        //        }, stopDis: 2.0f);
                        //    }
                        //});
                        Action afterTransmit = () => {
                            Vector3 fightOrigin = pos;
                            StateInit.Instance.Origin = fightOrigin;
                            SelfRole.moveto(mapId, pos, () =>
                            {
                                SelfRole.fsm.StartAutofight();
                                MonsterMgr._inst.taskMonId = _taskXml.getInt("target_param2");
                            }, stopDis: 2.0f,taskmove:true);
                        };
                        if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                            SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: afterTransmit,taskTrans:true);
                        else
                            SelfRole.WalkToMap(mapId, pos, afterTransmit,taskmove:true);
                        break;
                    case TaskTargetType.COLLECT:

                        //Action afterTransmit_collect = () =>
                        //{
                        //    StateAutoMoveToPos.Instance.stopdistance = 0.3f;
                        //    SelfRole.moveto(mapId, pos, () => SelfRole.fsm.StartAutoCollect());
                        //};
                        Action afterTransmit_collect = () => SelfRole.WalkToMap(mapId, pos, SelfRole.fsm.StartAutoCollect,taskmove:true);
                        if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                            SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: afterTransmit_collect,taskTrans:true);
                        else
                            afterTransmit_collect();
                        break;
                    case TaskTargetType.FB:
                        if (a3_liteMinimap.instance)
                            a3_liteMinimap.instance.ZidongTask = false;
                        int fbid = taskXml.getInt("target_param2");
                        //StateAutoMoveToPos.Instance.stopdistance = 0.3f;
                        if (GRMap.instance.m_nCurMapID == mapId || GameRoomMgr.getInstance().curRoom is PlotRoom)
                            SelfRole.moveto(mapId, pos, () => SelfRole.fsm.StartAutofight(), stopDis: 2.0f,taskmove:true /* 副本中开始战斗的停止距离 */ );
                        else
                        {
                            Variant sendData = new Variant();
                            sendData["npcid"] = 0;
                            sendData["ltpid"] = fbid;
                            sendData["diff_lvl"] = 1;
                            int levelinfoid = taskXml.getInt("level_info");
                            var tainf = XMLMgr.instance.GetSXML("task.level_info", "id==" + levelinfoid);
                            bool guide = (taskXml.getInt("guide") == 1);
                            int type = taskXml.getInt("level_yw");
                            if (type == 1)
                            {
                                MsgBoxMgr.getInstance().showTask_fb_confirm(tainf.getString("title"), tainf.getString("desc"),
                                    guide, a3_ygyiwuModel.getInstance().GetYiWu_God(a3_ygyiwuModel.getInstance().nowGod_id).need_zdl, () => LevelProxy.getInstance().sendCreate_lvl(sendData));
                            }
                            else
                            {
                                MsgBoxMgr.getInstance().showTask_fb_confirm(tainf.getString("title"), tainf.getString("desc"),
                                    guide, () => LevelProxy.getInstance().sendCreate_lvl(sendData));
                            }
                        }
                        break;
                    case TaskTargetType.WAIT_POINT_GIVEN: /*在指定区域读条*/
                    case TaskTargetType.GET_ITEM_GIVEN: /*收集指定道具*/
                    case TaskTargetType.KILL_MONSTER_GIVEN: /*打指定怪物*/
                        DealByType(taskData, checkItems);
                        break;
                }
                SXML _taskXML = XMLMgr.instance.GetSXML("task.Task", "id==" + taskData.taskId);
                if (SelfRole.UnderTaskAutoMove = taskData.targetType == TaskTargetType.KILL)
                {
                    StateAutoMoveToPos.Instance.stopdistance = 2.0f;
                    Vector3 fightOrigin = new Vector3(
                        x: _taskXML.getInt("target_coordinate_x"),
                        y: 0,
                        z: _taskXML.getInt("target_coordinate_y")
                    );
                    StateInit.Instance.Origin = fightOrigin;
                }
                if (taskData.targetType == TaskTargetType.KILL || taskData.targetType == TaskTargetType.DODAILY)
                {
                    int monId = _taskXML.getInt("target_param2");
                    if (monId != -1)
                        MonsterMgr._inst.taskMonId = monId;
                }
            }
            return true;
        }
        private void OnTalkWithNpc()
        {
            //if (/*!executeTask.isComplete && */executeTask.targetType == TaskTargetType.VISIT)
            //{
            //    A3_TaskProxy.getInstance().SendTalkWithNpc(tarNpcId);
            //}

            //AutoTask = true;
        }

        private void DealByType(TaskData taskData, bool checkItems)
        {
            A3_TaskOpt.Instance.ResetStat();
            A3_TaskOpt.Instance.Reset(alsoHideGameObject: true, resetCase: 0);
            TaskTargetType taskTargetType = taskData.targetType;
            TaskType taskType = taskData.taskT;
            if (A3_TaskOpt.Instance == null)
            {
                //Debug.LogError("请将A3_TaskOpt预制件默认设为Active");
                return;// not init
            }
            if (!A3_TaskOpt.Instance.taskOptElement.ContainsKey(taskData.taskId))
                A3_TaskOpt.Instance.taskOptElement[taskData.taskId] = new TaskOptElement(taskData.taskId);
            A3_TaskOpt.Instance.curTaskId = taskData.taskId;
            if (taskTargetType == TaskTargetType.WAIT_POINT_GIVEN/* || TaskTargetType.MESSAGE_GIVEN_POS == taskTargetType*/)
            {

                Vector3 waitPosition = Vector3.zero;
                var pointInfo = A3_TaskModel.getInstance().GetTaskXML().GetNode("Task", "id==" + taskData.taskId);
                //A3_TaskOpt.Instance.IsOnTaskWait = true;
                A3_TaskOpt.Instance.LockStat = false;
                if (A3_TaskOpt.Instance.isWaiting)
                    A3_TaskOpt.Instance.StopCD();
                A3_TaskOpt.Instance.BtnWait.interactable = true;
                A3_TaskOpt.Instance.waitPosition = new Vector3(pointInfo.getFloat("target_coordinate_x"), 0, pointInfo.getFloat("target_coordinate_y"));
                waitPosition = A3_TaskOpt.Instance.waitPosition;
                A3_TaskOpt.Instance.actionImage.sprite = GAMEAPI.ABUI_LoadSprite("icon_task_action_" + pointInfo.getInt("act_icon"));
                A3_TaskOpt.Instance.transform.FindChild("wait/action_text").GetComponent<Text>().text = pointInfo.getString("act_name");
               
                

                #region
                PlayerModel.getInstance().task_monsterIdOnAttack[taskData.taskId] = pointInfo.getInt("target_param2");
                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = pointInfo.getInt("tasking_map_id"),
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.WalkToMap(pointInfo.getInt("tasking_map_id"),
                //    waitPosition),
                //    targetPosition = waitPosition
                //});
                int mapId = pointInfo.getInt("tasking_map_id");
                if (MapModel.getInstance().dicMappoint.ContainsKey(mapId))
                {
                    int mapPoint = MapModel.getInstance().dicMappoint[mapId];
                    if (GRMap.instance.m_nCurMapID != mapId && GRMap.instance.m_nCurMapID != mapPoint)
                        SelfRole.Transmit(mapPoint, () => SelfRole.WalkToMap(mapId, waitPosition,taskmove:true),taskTrans:true);
                    else
                        SelfRole.WalkToMap(mapId, waitPosition,taskmove:true);
                }

            } // endif wait given point
            else if (taskTargetType == TaskTargetType.KILL_MONSTER_GIVEN)
            {
                A3_TaskOpt.Instance.IsOnKillMon = true;
                Vector3 waitPosition = Vector3.zero;
                SXML monInfo = A3_TaskModel.getInstance().GetTaskXML().GetNode("Task", "id==" + taskData.taskId);

                if (monInfo != null)
                {
                    //A3_TaskOpt.Instance.IsOnKillMon = true;
                    A3_TaskOpt.Instance.LockStat = false;
                    PlayerModel.getInstance().task_monsterIdOnAttack.Add(taskData.taskId, monInfo.getInt("target_param2"));
                    A3_TaskOpt.Instance.killPosition = new Vector3(monInfo.getFloat("target_coordinate_x"), 0, monInfo.getFloat("target_coordinate_y"));
                    waitPosition = A3_TaskOpt.Instance.killPosition;
                }

                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = monInfo.getInt("tasking_map_id"),
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.WalkToMap(monInfo.getInt("tasking_map_id"),
                //    waitPosition),
                //    targetPosition = waitPosition
                //});
                int mapId = monInfo.getInt("tasking_map_id");
                if (MapModel.getInstance().dicMappoint.ContainsKey(mapId))
                {
                    int mapPoint = MapModel.getInstance().dicMappoint[mapId];
                    if (GRMap.instance.m_nCurMapID != mapId && GRMap.instance.m_nCurMapID != mapPoint)
                        SelfRole.Transmit(mapPoint, () => SelfRole.WalkToMap(mapId, waitPosition,taskmove:true),taskTrans:true);
                    else
                        SelfRole.WalkToMap(mapId, waitPosition,taskmove:true);
                }
               
            } // endif kill given monster
            else if (taskTargetType == TaskTargetType.GET_ITEM_GIVEN)
            {
                int holdNum = 0, needNum = 0;
                //A3_TaskOpt.Instance.IsOnTaskWait = true;
                Action uiHandle = null;
                NpcShopData npcShopData = null;
                SXML taskinfoGetItem = A3_TaskModel.getInstance().GetTaskXML().GetNode("Task", "id==" + taskData.taskId);

                A3_TaskOpt.Instance.taskItemId = taskinfoGetItem.getUint("target_param2");
                needNum = taskData.completion - taskData.taskRate;//taskinfoGetItem.getInt("target_param1");
                holdNum = a3_BagModel.getInstance().getItemNumByTpid(A3_TaskOpt.Instance.taskItemId);
                uiHandle = () => A3_TaskOpt.Instance?.ShowSubmitItem();
                npcShopData = A3_NPCShopModel.getInstance().GetDataByItemId(A3_TaskOpt.Instance.taskItemId);
                if (holdNum >= needNum)
                {
                    int npcId = taskinfoGetItem.getInt("complete_npc_id");
                    int mapId = taskinfoGetItem.getInt("tasking_map_id");
                    SXML npcInfo = XMLMgr.instance.GetSXML("npcs").GetNode("npc", "id==" + npcId);
                    float mapY = npcInfo.getFloat("location_height");
                    if (mapY < 0) mapY = 0;
                    Vector3 npcPos = new Vector3(npcInfo.getFloat("location_x") / GameConstant.GEZI_TRANS_UNITYPOS, mapY / GameConstant.GEZI_TRANS_UNITYPOS, npcInfo.getFloat("location_y") / GameConstant.GEZI_TRANS_UNITYPOS);
                    //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                    //{
                    //    mapId = mapId,
                    //    check_beforeShow = true,
                    //    handle_customized_afterTransmit = () =>
                    //    {
                    //        SelfRole.WalkToMap(mapId, npcPos, handle: () => { A3_TaskOpt.Instance.ShowSubmitItem(); }, stopDis: 2f);
                    //    }
                    //});                    
                    if (MapModel.getInstance().dicMappoint.ContainsKey(mapId))
                    {
                        int mapPoint = MapModel.getInstance().dicMappoint[mapId];
                        if (GRMap.instance.m_nCurMapID != mapId && GRMap.instance.m_nCurMapID != mapPoint)
                            SelfRole.Transmit(mapPoint, () => SelfRole.WalkToMap(mapId, npcPos, () => { A3_TaskOpt.Instance.ShowSubmitItem(); }, 2f,taskmove:true),taskTrans:true);
                        else
                            SelfRole.WalkToMap(mapId, npcPos, () => { A3_TaskOpt.Instance.ShowSubmitItem(); }, 2f,taskmove:true);
                    }
                }
                else if (npcShopData != null)
                {
                    //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                    //{
                    //    mapId = npcShopData.mapId,
                    //    check_beforeShow = true,
                    //    handle_customized_afterTransmit = () => SelfRole.moveToNPc(npcShopData.mapId, npcShopData.npc_id)
                    //});
                    int mapId = npcShopData.mapId;
                    if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                        SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.moveToNPc(mapId, npcShopData.npc_id,taskmove:true),taskTrans:true);
                    else
                        SelfRole.moveToNPc(mapId, npcShopData.npc_id,taskmove:true);
                }
                else
                {
                    if (checkItems)
                    {
                        ArrayList data = new ArrayList();
                        data.Add(a3_BagModel.getInstance().getItemDataById(A3_TaskOpt.Instance.taskItemId));
                        //data.Add(null);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data);
                    }
                }
            } // endif get given item
            #endregion
        }




        //private void AutoCollect()
        //{
        //    CollectRole role = null;
        //    foreach (MonsterRole m in MonsterMgr._inst.m_mapMonster.Values)
        //    {
        //        if (m is CollectRole && A3_TaskModel.getInstance().IfCurrentCollectItem(m.monsterid))
        //        {
        //            role = m as CollectRole;
        //            break;
        //        }
        //    }
        //    if (role != null)
        //    {

        //    }
        //}

        public void PauseAutoKill(int taskId = -1)
        {
            if (taskId <= 0)
                taskId = A3_TaskModel.getInstance().main_task_id;
            Dictionary<int, int> bufferTaskDic = PlayerModel.getInstance().task_monsterId;
            Dictionary<int, int> runningTaskDic = PlayerModel.getInstance().task_monsterIdOnAttack;
            if (!bufferTaskDic.ContainsKey(taskId) && runningTaskDic.ContainsKey(taskId))
            {
                bufferTaskDic.Add(taskId, runningTaskDic[taskId]);
                runningTaskDic.Remove(taskId);
            }
        }


    }


}
