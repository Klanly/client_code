using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cross;
using GameFramework;
using UnityEngine;
namespace MuGame
{

    public enum TaskType //除NULL外,暂定为数值越小优先度越高 MAIN -> ENTRUST -> CLAN
    {
        NULL = 0,
        MAIN = 1,
        DAILY = 3,
        BRANCH = 2,
        ENTRUST = 4,    //委托
        CLAN = 5        //军团
    }
    public enum TaskTargetType
    {
        DODAILY = 1,
        FRIEND = 13,
        WING = 16,
        VISIT = 22,// 访问（无条件）
        KILL = 23, // 杀怪类
        COLLECT = 24,//采集
        GETEXP = 25,  //卡级任务、转生任务
        FB = 4,
        GET_ITEM_GIVEN = 41, //获得指定道具
        KILL_MONSTER_GIVEN = 42, //击杀特定怪物
        WAIT_POINT_GIVEN = 43, //指定位置进行读条
        //MESSAGE_GIVEN_POS = 44,
    }

    public class A3_TaskModel : ModelBase<A3_TaskModel>
    {
        public static uint ON_NPC_TASK_STATE_CHANGE = 1;
        public static uint DAILY_TASK_LIMIT = 10;
        public static uint REMOVE_TOP_SHOW_TASK = 11;
        public static uint CLAN_LOOP_LIMIT;
        public static uint CLAN_CNT_EACHLOOP;
        public static uint CLAN_MAX_COUNT;
        public List<Variant> cacheProxy = new List<Variant>();
        public A3_TaskModel() : base()
        {
            if (XMLMgr.instance.GetSXML("task") == null) return;

            DAILY_TASK_LIMIT = XMLMgr.instance.GetSXML("task").GetNode("limit_num").getUint("dailytask");
            CLAN_CNT_EACHLOOP = XMLMgr.instance.GetSXML("task").GetNode("cmis_limit").getUint("cmis_limit");
            CLAN_LOOP_LIMIT = XMLMgr.instance.GetSXML("task").GetNode("cmis_limit").getUint("loop_limit");
            CLAN_MAX_COUNT = CLAN_CNT_EACHLOOP * CLAN_LOOP_LIMIT;
            dicEntrustExtraReward = new Dictionary<int, List<EntrustExtraRewardData>>();
            List<SXML> xmlEntrustExtraReward = XMLMgr.instance.GetSXMLList("task.entrust_extra_award");
            if (xmlEntrustExtraReward != null)
                for (int i = 0; i < xmlEntrustExtraReward.Count; i++)
                {
                    int zhuan = xmlEntrustExtraReward[i].getInt("zhuan");
                    if (zhuan >= 0)
                    {
                        dicEntrustExtraReward[zhuan] = new List<EntrustExtraRewardData>();
                        var rewardList = xmlEntrustExtraReward[i].GetNodeList("RewardItem");
                        for (int j = 0; j < rewardList.Count; j++)
                        {
                            EntrustExtraRewardData reward = new EntrustExtraRewardData();
                            reward.tpid = rewardList[j].getUint("item_id");
                            reward.num = rewardList[j].getInt("value");
                            if (reward.tpid != 0 && reward.num != 0)
                                dicEntrustExtraReward[zhuan].Add(reward);
                        }
                        if (dicEntrustExtraReward[zhuan].Count == 0)
                            dicEntrustExtraReward.Remove(zhuan);
                    }
                }
        }

        public bool isSubTask = false;
        public int main_task_id;
        public int main_chapter_id = -1;
        private SXML taskXML = null;
        private SXML TaskXML
        {
            get { return taskXML ?? (taskXML = XMLMgr.instance.GetSXML("task")); }
        }
        public Func<SXML> GetTaskXML => () => TaskXML;
        public Dictionary<TaskType, Variant> dicTaskConfig = new Dictionary<TaskType, Variant>();
        public Dictionary<int /*zhuan*/ , List<EntrustExtraRewardData>> dicEntrustExtraReward;
        public TaskData curTask = null;

        //已领取的任务列表
        private Dictionary<int, TaskData> dicPlayerTask = new Dictionary<int, TaskData>();
        //可领取任务
        private Dictionary<int, TaskData> dicAcceptableTask = new Dictionary<int, TaskData>();

        //npc身上的任务列表
        //private Dictionary<int, List<int>> dicNpcTaskId = new Dictionary<int, List<int>>();

        ////获取任务信息
        //private void TaskNpcConf()
        //{
        //    List<SXML> npcXml = TaskXML.GetNodeList("Task");
        //    foreach (SXML xml in npcXml)
        //    {
        //        if (xml.m_dAtttr.ContainsKey("complete_npc_id"))
        //        {
        //            int taskid = xml.getInt("id");
        //            int npcId = xml.getInt("complete_npc_id");
        //            if (dicNpcTaskId[npcId] == null)
        //            {
        //                dicNpcTaskId[npcId] = new List<int>();
        //            }
        //            dicNpcTaskId[npcId].Add(taskid);
        //        }
        //    }
        //}

        //根据主线任务ID获取章节信息
        public ChapterInfos GetChapterInfosById(int chapterId)
        {
            ChapterInfos infos = new ChapterInfos();
            SXML chapterXml = TaskXML.GetNode("Chapter", "id==" + chapterId);
            if (chapterXml != null)
            {
                infos.id = chapterXml.getInt("id");
                infos.name = chapterXml.getString("name");
                infos.description = chapterXml.getString("description");
            }

            return infos;
        }

        //获取已领取的任务列表
        public Dictionary<int, TaskData> GetDicTaskData()
        {
            return this.dicPlayerTask;
        }

        #region 添加任务

        //获取任务详细信息
        private bool SyncXmlConf(TaskData data)
        {
            SXML taskXml = TaskXML.GetNode("Task", "id==" + data.taskId);
            bool hasXml = taskXml != null;
            if (hasXml)
            {
                data.taskName = taskXml.getString("name");
                data.targetType = (TaskTargetType)taskXml.getInt("target_type");
                data.completion = taskXml.getInt("target_param1");
                data.completionAim = taskXml.getInt("target_param2");
                data.completionStr = taskXml.getString("target_param2");
                data.extraAward = taskXml.getInt("extra_award");
                data.chapterId = taskXml.getInt("Chapter_id");
                data.taskMapid = taskXml.getInt("tasking_map_id");
                data.transto = taskXml.getInt("transto");
                data.guide = (taskXml.getInt("guide") == 1);
                data.npcId = taskXml.getInt("complete_npc_id");
                data.completeWay = taskXml.getInt("complete_way");
                data.story_hint = taskXml.getString("story_hint");
                data.next_step = taskXml.hasValue("next_step");
                data.explain = taskXml.getString("explain");
                data.topShowOnLiteminimap = taskXml.hasValue("top_show");
                data.isComplete = data.taskRate >= data.completion;
                data.need_tm = (taskXml.hasValue("need_tm") ? taskXml.getFloat("need_tm") : default(float));
                data.extraRateDesc = (taskXml.hasValue("target_bar") ? taskXml.getString("target_bar") : null);
                data.showMessage = taskXml.hasValue("show_message");
                data.autoStart = !taskXml.hasValue("auto_stop");
                //if (data.taskId == main_task_id && data.isComplete)
                //{
                //FunctionOpenMgr.instance.onFinshedMainTask(A3_TaskModel.getInstance().main_task_id, true);
                //}
            }
            return hasXml;
        }

        //判断是否新章节
        bool isNewChapter(int taskid)
        {
            bool b = false;
            SXML xmla = TaskXML.GetNode("Task", "id==" + (taskid - 1));
            if (xmla == null) return true;
            SXML xmlb = TaskXML.GetNode("Task", "id==" + (taskid));
            if (xmla.getInt("Chapter_id") < xmlb.getInt("Chapter_id")) return true;
            return b;
        }

        //同步任务信息
        public void OnSyncTask(Variant data)
        {
            dicPlayerTask.Clear();

            List<TaskData> listTemp = new List<TaskData>();
            if (data.ContainsKey("mlmis"))
            {
                Variant mainTask = data["mlmis"];
                TaskData tempData = new TaskData();

                tempData.taskId = mainTask["id"];
                main_task_id = mainTask["id"];
                tempData.taskRate = mainTask["cnt"];
                tempData.taskT = TaskType.MAIN;
                if (mainTask.ContainsKey("lose_tm"))
                    tempData.lose_tm = mainTask["lose_tm"];
                tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                main_chapter_id = TaskXML.GetNode("Task", "id==" + tempData.taskId).getInt("Chapter_id");
                listTemp.Add(tempData);
            }

            if (data.ContainsKey("dmis"))
            {
                Variant dailyTask = data["dmis"];
                TaskData tempData = new TaskData();

                tempData.taskT = TaskType.DAILY;
                if (dailyTask.ContainsKey("dmis_count"))
                    tempData.taskCount = dailyTask["dmis_count"];
                if (dailyTask.ContainsKey("id"))
                {

                    tempData.taskId = dailyTask["id"];
                    tempData.taskRate = dailyTask["cnt"];
                    tempData.taskStar = dailyTask["star"];
                    listTemp.Add(tempData);
                }
            }

            if (data.ContainsKey("bmis"))
            {
                List<Variant> listVar = new List<Variant>();

                listVar = data["bmis"]._arr;

                foreach (Variant item in listVar)
                {
                    TaskData tempData = new TaskData();
                    tempData.taskT = TaskType.BRANCH;
                    tempData.taskId = item["id"];
                    tempData.taskRate = item["cnt"];
                    if (item.ContainsKey("lose_tm"))
                        tempData.lose_tm = item["lose_tm"];
                    SXML t = TaskXML.GetNode("Task", "id==" + tempData.taskId);
                    if (t != null)
                    {
                        tempData.release_tm = t.GetNode("m")?.getInt("release_tm") ?? 0;
                        SyncXmlConf(tempData);
                        listTemp.Add(tempData);
                    }
                }
            }

            if (data.ContainsKey("emis"))
            {
                Variant entrustTask = data["emis"];
                TaskData tempData = new TaskData();
                if (entrustTask.ContainsKey("id"))
                {
                    tempData.taskT = TaskType.ENTRUST;
                    tempData.taskCount = entrustTask["emis_count"];// + entrustTask["loop_count"] * 10;
                    tempData.taskLoop = entrustTask["loop_count"];
                    tempData.taskId = entrustTask["id"];
                    tempData.taskRate = entrustTask["cnt"];
                    if (entrustTask.ContainsKey("lose_tm"))
                        tempData.lose_tm = entrustTask["lose_tm"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                    listTemp.Add(tempData);                    
                }
            }
            if (data.ContainsKey("cmis"))
            {
                Variant clanTask = data["cmis"];
                TaskData tempData = new TaskData();
                if (clanTask.ContainsKey("id"))
                {
                    tempData.taskT = TaskType.CLAN;
                    tempData.taskCount = clanTask["cmis_count"];// + clanTask["loop_count"] * 10;
                    tempData.taskLoop = clanTask["loop_count"];
                    tempData.taskId = clanTask["id"];
                    tempData.taskRate = clanTask["cnt"];
                    if (clanTask.ContainsKey("lose_tm"))
                        tempData.lose_tm = clanTask["lose_tm"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                    listTemp.Add(tempData);
                }
            }
            foreach (TaskData taskData in listTemp)
            {
                if (!SyncXmlConf(taskData))
                    debug.Log("未找到任务配置");

                dicPlayerTask[taskData.taskId] = taskData;
                if (taskData.taskT == TaskType.MAIN)
                    dicPlayerTask[taskData.taskId].taskCount = GetMainTaskIndex(taskData.taskId);

                DispatchNpcEvent(taskData.taskId);
            }
        }
        //获取委托每轮奖励
        public List<EntrustExtraRewardData> GetEntrustRewardList()
        {
            List<int> level = new List<int>(dicEntrustExtraReward.Keys);
            int availableLv = -1;
            for (int i = 0; i < level.Count; i++)
                if (level[i] > PlayerModel.getInstance().up_lvl)
                    break;
                else
                    availableLv = level[i];
            if (availableLv == -1)
                return null;
            else
                return dicEntrustExtraReward[availableLv];
        }
        //添加任务
        public List<TaskData> listAddTask;        
        public void OnAddTask(Variant data)
        {
            List<TaskData> listTemp = new List<TaskData>();
            if (data.ContainsKey("mlmis"))
            {
                Variant mainTask = data["mlmis"];
                TaskData tempData = new TaskData();
                main_task_id = mainTask["id"];
                tempData.taskId = mainTask["id"];
                tempData.taskRate = mainTask["cnt"];
                tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                tempData.taskT = TaskType.MAIN;
                listTemp.Add(tempData);
            }
            else if (data.ContainsKey("dmis"))
            {
                Variant dailyTask = data["dmis"];
                TaskData tempData = new TaskData();
                tempData.taskT = TaskType.DAILY;
                tempData.taskCount = dailyTask["dmis_count"];
                if (dailyTask.ContainsKey("id"))
                {
                    tempData.taskId = dailyTask["id"];
                    tempData.taskRate = dailyTask["cnt"];
                    tempData.taskStar = dailyTask["star"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                }
                else if (GetDailyTask() != null)
                    a3_liteMinimap.instance.SubmitTask(GetDailyTask().taskId);
                if (tempData.taskCount >= DAILY_TASK_LIMIT)
                    flytxt.instance.fly(ContMgr.getCont("A3_TaskModel_thidayok"), 5);
                listTemp.Add(tempData);
            }
            else if (data.ContainsKey("bmis"))
            {
                List<Variant> listVar = new List<Variant>();
                listVar = data["bmis"]._arr;
                foreach (Variant item in listVar)
                {
                    TaskData tempData = new TaskData();
                    tempData.taskT = TaskType.BRANCH;
                    tempData.taskId = item["id"];
                    tempData.taskRate = item["cnt"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                    listTemp.Add(tempData);
                }
            }
            else if (data.ContainsKey("emis"))
            {
                Variant entrustTask = data["emis"];
                TaskData tempData = new TaskData();
                tempData.taskT = TaskType.ENTRUST;
                tempData.taskCount = entrustTask["emis_count"];
                if (entrustTask.ContainsKey("id"))
                {
                    tempData.taskId = entrustTask["id"];
                    tempData.taskRate = entrustTask["cnt"];
                    tempData.taskLoop = entrustTask["loop_count"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;                    
                }
                a3_task.instance?.tabEntrust.SetActive(true);
                listTemp.Add(tempData);
            }
            else if (data.ContainsKey("cmis"))
            {
                Variant clanTask = data["cmis"];
                TaskData tempData = new TaskData();
                tempData.taskT = TaskType.CLAN;
                tempData.taskCount = clanTask["cmis_count"];
                if (clanTask.ContainsKey("id"))
                {
                    tempData.taskId = clanTask["id"];
                    tempData.taskRate = clanTask["cnt"];
                    tempData.taskLoop = clanTask["loop_count"];
                    tempData.release_tm = TaskXML.GetNode("Task", "id==" + tempData.taskId).GetNode("m")?.getInt("release_tm") ?? 0;
                }
                listTemp.Add(tempData);
            }
            listAddTask = new List<TaskData>();
            foreach (TaskData taskData in listTemp)
            {
                if (!SyncXmlConf(taskData))
                    debug.Log("未找到任务配置");
                OnProcessTaskList(taskData);
                dicPlayerTask[taskData.taskId] = taskData;
                listAddTask.Add(taskData);
                DispatchNpcEvent(taskData.taskId);
                if (taskData.taskT == TaskType.MAIN && taskData.targetType != TaskTargetType.GETEXP || taskData.taskT == TaskType.ENTRUST || taskData.taskT == TaskType.CLAN)
                    if (curTask == null || curTask.taskT == taskData.taskT)
                        //if (taskData.transto > 0)
                        //    SelfRole.Transmit(taskData.transto, () => a3_task_auto.instance.RunTask(taskData, true), false, true);
                        //else 
                        if (!SelfRole.s_bInTransmit)
                        {
                            if (!a3_task_auto.instance.bDoClanTask && taskData.taskT == TaskType.CLAN)
                                a3_task_auto.instance.bDoClanTask = true;
                            else
                            {
                                if (taskData.autoStart)
                                    a3_task_auto.instance.RunTask(taskData, true);
                            }
                        }
                if (taskData.taskT == TaskType.DAILY)
                    a3_task_auto.instance.RunTask(taskData);
                if (taskData.targetType == TaskTargetType.KILL)
                {
                    SXML task_nodeInfo = TaskXML.GetNode("Task", "id==" + taskData.taskId);
                    if (!PlayerModel.getInstance().task_monsterIdOnAttack.ContainsKey(taskData.taskId))
                        PlayerModel.getInstance().task_monsterIdOnAttack.Add(taskData.taskId, task_nodeInfo.getInt("target_param2"));
                }
                a3_liteMinimap.instance?.RefreshTaskPage(taskData.taskId);
            }
        }

        public static void onCD(cd item)
        {
            int temp = (int)(cd.secCD - cd.lastCD) / 100;
            item.txt.text = ContMgr.getCont("A3_TaskModel_csz") + ((float)temp / 10f).ToString();
        }

        //处理任务列表
        private void OnProcessTaskList(TaskData taskData)
        {//待优化
            int id = taskData.taskId;
            if (id == 0)
            {
                List<int> listTemp = new List<int>();
                foreach (TaskData data in dicPlayerTask.Values)
                {
                    if (data.taskT == taskData.taskT)
                    {
                        listTemp.Add(data.taskId);
                    }
                }
                foreach (int tempId in listTemp)
                {
                    removeTask(tempId);
                }
                taskData.taskT = TaskType.NULL;
            }
            else
            {
                dicPlayerTask[id] = taskData;
                switch (taskData.taskT)
                {
                    case TaskType.MAIN:
                        dicPlayerTask[id].taskCount = GetMainTaskIndex(id);
                        break;
                    case TaskType.DAILY:
                    case TaskType.BRANCH:
                    case TaskType.ENTRUST:
                    default:
                        break;
                }
            }

        }
        #endregion

        #region 移除任务

        //获得任务奖励, 移除任务 

        public void OnSubmitTask(Variant data)
        {
            int taskId = data["id"];
            var rdl = GetReward(taskId);
            int ic = 0;
            List<KeyValuePair<string, string>> rewardList = new List<KeyValuePair<string, string>>();
            foreach (var v in rdl)
            {
                //KeyValuePair<string, string> reward = new KeyValuePair<string, string>();
                //if (v.type == 1 && v.id == 1)
                //    flytxt.instance.fly("EXP+" + v.num, 3);
                //if (v.type == 1 && v.id == 1)
                //    reward = new KeyValuePair<string, string>(key: "txt_8", value: "经验 + " + v.num);
                //if (v.type == 1 && v.id == 2)
                //    reward = new KeyValuePair<string, string>(key: "txt_9", value: "金币 + " + v.num);
                //if (reward.Key != null)
                //    rewardList.Add(reward);
                if (v.type == 2 || v.type == 3)
                {
                    //判断下一个任务是否是同一个NPC
                    if (!IsTalkWithSameNpc())
                    {
                        a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)v.id);
                        if (item.job_limit == PlayerModel.getInstance().profession)
                        {
                            flytxt.instance.fly(item.item_name + "  +" + Mathf.Max(1, v.num), 5, Globle.getColorByQuality(item.quality));
                            GameObject go = IconImageMgr.getInstance().createA3ItemIconTip(itemid: (uint)v.id, num: v.num);
                            GameObject.Destroy(go.transform.FindChild("iconborder/equip_canequip").gameObject);
                            GameObject.Destroy(go.transform.FindChild("num").gameObject);
                            flytxt.instance.fly(null, 6, showIcon: go);
                        }
                        if (v.type == 3)
                            flytxt.instance.fly(null, 6, showIcon: IconImageMgr.getInstance().createA3ItemIconTip(itemid: (uint)v.id, num: v.num));
                    }
                }
            }
            if (rewardList.Count > 0)
            {
                flytxt.instance.FlyQueue(rewardList);
                //flytxt.instance.FlyNext();
            }
            if (dicPlayerTask.ContainsKey(taskId))
            {
                var md = dicPlayerTask[taskId];
                if (md.taskT == TaskType.MAIN && isNewChapter(md.taskId) && main_chapter_id >= 0)
                {
                    a3_chapter_hint.ShowChapterHint(md.chapterId);
                }
                if (md.taskT == TaskType.CLAN)
                {
                    if (md.taskCount + 1 == CLAN_CNT_EACHLOOP)
                    {
                        List<SXML> rewardEachLoop = XMLMgr.instance.GetSXML("task.clan_extra").GetNodeList("RewardItem");
                        List<string> rwdMsgs = new List<string>();
                        for (int i = 0; i < rewardEachLoop.Count; i++)
                        {
                            uint itemId = rewardEachLoop[i].getUint("item_id");
                            int itemCnt = rewardEachLoop[i].getInt("value");
                            a3_ItemData rwdDta = a3_BagModel.getInstance().getItemDataById(itemId);
                            if (rwdDta.item_name != null)
                            {
                                string name = rwdDta.item_name;
                                rwdMsgs.Add(ContMgr.getCont("A3_TaskModel_get", new List<string>() { name, itemCnt.ToString()}));
                            }
                        }
                        if (rwdMsgs.Count != 0)
                        {
                            flytxt.instance.AddDelayFlytxtList(rwdMsgs);
                            flytxt.instance.StartDelayFly(0.2f * (1 + rdl.Count));
                        }
                    }
                }
            }


            removeTask(taskId);
        }

        public bool IsTalkWithSameNpc()
        {
            SXML nowMainTask = TaskXML.GetNode("Task", "id==" + main_task_id);
            if (nowMainTask != null)
            {
                int nextMainTaskId = nowMainTask.getInt("follow_task_id");
                if (nextMainTaskId > 0)
                {
                    SXML nextMainTask = TaskXML.GetNode("Task", "id==" + nextMainTaskId);
                    if (nextMainTask != null)
                    {
                        int targetType = nextMainTask.getInt("target_type");
                        if (targetType == 22 && nextMainTask.getInt("complete_npc_id") == nowMainTask.getInt("complete_npc_id"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //移除(提交)提交任务
        private void removeTask(int taskId)
        {
            if (!dicPlayerTask.ContainsKey(taskId))
                return;
            if (dicPlayerTask[taskId].topShowOnLiteminimap)
                dispatchEvent(GameEvent.Create(REMOVE_TOP_SHOW_TASK, this, null));

            if (GetTaskDataById(taskId).taskT == TaskType.ENTRUST)
                a3_task.instance?.tabEntrust.SetActive(false);
            dicPlayerTask.Remove(taskId);
            curTask = null;
            DispatchNpcEvent(taskId);
        }

        #endregion

        #region 刷新任务

        //刷新任务进度
        public void RefreshTask(Variant data)
        {
            List<Variant> listData = data["change_task"]._arr;            
            TaskData commitTask = null;
            foreach (Variant d in listData)
            {
                int id = d["id"];
                int count = d["cnt"];

                if (!dicPlayerTask.ContainsKey(id))
                    continue;

                TaskData tempData = dicPlayerTask[id];
                tempData.taskRate = count;

                if (d.ContainsKey("star"))
                {
                    tempData.taskStar = d["star"];

                }
                tempData.isComplete = count >= tempData.completion;
                if (tempData.taskId == main_task_id && tempData.isComplete)
                {
                    //改成提交任务时算为完成任务
                    //FunctionOpenMgr.instance.onFinshedMainTask(A3_TaskModel.getInstance().main_task_id, true, true);
                }

                dicPlayerTask[id] = tempData;

                if (tempData.targetType == TaskTargetType.COLLECT || tempData.targetType == TaskTargetType.KILL)
                {
                    SXML taskXml = XMLMgr.instance.GetSXML("task.Task", "id==" + tempData.taskId);
                    int Id = taskXml.getInt("target_param2");
                    SXML mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + Id);
                    string sn = mxml.getString("name");
                    flytxt.instance.fly((tempData.extraRateDesc != null ? tempData.extraRateDesc : sn) + "(" + tempData.taskRate + "/" + tempData.completion + ")", 5);
                }
                if (tempData.targetType == TaskTargetType.KILL_MONSTER_GIVEN)
                {
                    if (d.ContainsKey("lose_tm"))
                        tempData.lose_tm = d["lose_tm"];
                }
                if (tempData.taskRate == tempData.completion)
                {
                    PlayerModel.getInstance().task_monsterIdOnAttack.Remove(tempData.taskId);
                    PlayerModel.getInstance().task_monsterIdOnAttack.Remove(tempData.taskId);
                    SelfRole.UnderTaskAutoMove = false;
                }
                if (tempData.isComplete)
                {
                    DispatchNpcEvent(id);
                    flytxt.instance.fly(tempData.taskName + ContMgr.getCont("canover"), 5);
                }
                if (commitTask == null || (tempData.taskT == TaskType.MAIN && tempData.isComplete && tempData.next_step
               || tempData.taskT == TaskType.ENTRUST && TaskType.ENTRUST < commitTask.taskT
                || tempData.taskT == TaskType.CLAN && TaskType.CLAN < commitTask.taskT))
                    commitTask = tempData;
                if (tempData.taskT == TaskType.MAIN && tempData.chapterId > main_chapter_id && tempData.isComplete)
                    main_chapter_id = tempData.chapterId;
               }
            if (commitTask != null && commitTask.targetType != TaskTargetType.GETEXP && commitTask.isComplete)
                a3_task_auto.instance.RunTask(commitTask);
                

        }

        #endregion

        //创建Npc状态事件
        private void DispatchNpcEvent(int taskId)
        {
            Variant data = new Variant();
            SXML xml = TaskXML.GetNode("Task", "id==" + taskId);
            if (xml != null)
            {
                data["npcId"] = xml.getInt("complete_npc_id");
                data["taskId"] = taskId;
                data["taskState"] = (int)GetTaskState(taskId);
                if (GRMap.grmap_loading)
                    cacheProxy.Add(data);
                dispatchEvent(GameEvent.Create(ON_NPC_TASK_STATE_CHANGE, this, data));
            }
        }

        //获取任务信息
        public TaskData GetTaskDataById(int taskId)
        {
            TaskData data = null;

            if (dicPlayerTask.ContainsKey(taskId))
                data = dicPlayerTask[taskId];

            return data;
        }

        //获得任务最大次数
        public int GetTaskMaxCount(int taskId)
        {
            TaskData data = dicPlayerTask[taskId];

            int maxCount = (int)DAILY_TASK_LIMIT;
            switch (data.taskT)
            {
                case TaskType.MAIN:
                    int chapterId = data.chapterId;
                    List<SXML> listXml = TaskXML.GetNodeList("Task", "Chapter_id==" + chapterId);
                    maxCount = listXml.Count;
                    break;
                case TaskType.DAILY:
                    maxCount = (int)DAILY_TASK_LIMIT;
                    break;
                case TaskType.BRANCH:
                    maxCount = 5;
                    break;
                case TaskType.ENTRUST:
                    maxCount = XMLMgr.instance.GetSXML("task").GetNode("emis_limit").getInt("emis_limit");
                    //maxCount = ((maxCount = XMLMgr.instance.GetSXML("task").GetNode("emis_limit").getInt("emis_limit")) == -1) ? maxCount : 10;
                    break;
                case TaskType.CLAN:
                    maxCount = ((maxCount = XMLMgr.instance.GetSXML("task").GetNode("cmis_limit").getInt("cmis_limit")) == -1) ? maxCount : 10;
                    break;
                default:
                    break;
            }

            return maxCount;
        }


        //是否已经领取该任务
        public bool IsAcceptTask(int taskId)
        {
            bool b = dicPlayerTask.ContainsKey(taskId);
            return b;
        }

        //是否完成总进度
        public bool IsCompleteCount(int taskId)
        {
            //TODO 检查任务是否已经完成
            TaskData data = dicPlayerTask[taskId];

            bool b = false;

            b = data.taskCount >= GetTaskMaxCount(taskId);

            return b;
        }

        //如果是对话, 获得npc对话内容
        public List<string> GetDialogkDesc(int taskId)
        {
            List<string> listTemp = new List<string>();
            SXML xml = TaskXML.GetNode("Task", "id==" + taskId);
            if (xml != null)
            {
                string strDialog;
                if (GetTaskDataById(taskId).isComplete)
                    strDialog = xml.getString("complete_dialog");
                else
                    strDialog = xml.getString("target_dialog");
                strDialog = GameFramework.StringUtils.formatText(strDialog);
                string[] listTempDia = strDialog.Split(';');

                listTemp = listTempDia.ToList<string>();
            }

            return listTemp;
        }

        //获得目标描述
        public string GetTaskDesc(int taskId, bool isComplete = false)
        {
            string str = string.Empty;
            SXML xml = TaskXML.GetNode("Task", "id==" + taskId);
            if (xml != null)
            {
                if (isComplete)
                    str = xml.getString("complete_desc");
                else
                    str = xml.getString("target_desc");
                str = GameFramework.StringUtils.formatText(str);
            }
            return str;
        }

        //获得任务状态
        public NpcTaskState GetTaskState(int taskId)
        {
            NpcTaskState state = NpcTaskState.NONE;
            if (!dicPlayerTask.ContainsKey(taskId))
                return state;
            TaskData taskData = dicPlayerTask[taskId];

            if (dicPlayerTask.ContainsKey(taskId))
            {
                if (taskData.isComplete)
                    state = NpcTaskState.FINISHED;
                else
                    state = NpcTaskState.UNFINISHED;
            }
            else if (dicAcceptableTask.ContainsKey(taskId))
            {
                //是否达到领取任务条件
                state = NpcTaskState.NONE;
                //可领取
                //为达到条件

            }
            else
            {
                state = NpcTaskState.NONE;
            }

            return state;
        }

        //获得任务标题
        public string GetTaskTypeStr(TaskType type)
        {
            string str = string.Empty;

            switch (type)
            {
                case TaskType.NULL:
                    break;
                case TaskType.MAIN:
                    str = ContMgr.getCont("task_minimap_title_1");
                    break;
                case TaskType.DAILY:
                    str = ContMgr.getCont("task_minimap_title_2");
                    break;
                case TaskType.BRANCH:
                    str = ContMgr.getCont("task_minimap_title_3");
                    break;
                case TaskType.ENTRUST:
                    str = ContMgr.getCont("task_minimap_title_4");
                    break;
                case TaskType.CLAN:
                    str = ContMgr.getCont("task_minimap_title_5");
                    break;
                default:
                    break;
            }

            return str;
        }

        //获得日常任务刷星星的金币
        public uint GetRefreshStarCost()
        {
            uint cost = 0;

            SXML xml = TaskXML.GetNode("refresh");
            if (xml != null)
            {
                cost = xml.getUint("gold_cost");
            }

            return cost;
        }

        //获取双倍日常奖励钻石消耗
        public uint GetDoublePrizeCost()
        {
            uint cost = 100;

            return cost;
        }

        //任务最大星星数
        public int GetMaxStarLevel()
        {
            int level = 5;

            return level;
        }

        //获得每个任务的元宝消耗
        public int GetOneKeyFinishEveryOneCost()
        {
            int cost = 0;

            SXML xml = TaskXML.GetNode("quickfinish");
            if (xml != null)
            {
                cost = xml.getInt("yb_cost");
            }

            return cost;
        }

        //获得每日任务最大完成数
        public int GetDailyMaxCount()
        {
            int maxCount = (int)DAILY_TASK_LIMIT;

            return maxCount;
        }

        //每日讨伐最大完成数
        public int GetKillMaxCount()
        {
            int maxCount = 5;

            return maxCount;
        }

        //获取额外奖励内容
        public Dictionary<uint, int> GetExtarPrizeData(int extarId)
        {
            Dictionary<uint, int> dicTemp = new Dictionary<uint, int>();

            SXML xml = TaskXML.GetNode("extra", "id==" + extarId);
            if (xml != null)
            {
                List<SXML> listXml = xml.GetNodeList("RewardItem");
                if (listXml != null)
                {
                    for (int i = 0; i < listXml.Count; i++)
                    {
                        uint itemId = listXml[i].getUint("item_id");
                        int num = listXml[i].getInt("value");
                        dicTemp[itemId] = num;
                    }
                }

            }

            return dicTemp;
        }

        //获得章节奖励内容
        public Dictionary<uint, int> GetChapterPrizeData(int chapterId)
        {
            Dictionary<uint, int> dicTemp = new Dictionary<uint, int>();

            SXML xml = TaskXML.GetNode("Cha_gift", "id==" + chapterId);
            if (xml != null)
            {
                List<SXML> listXml_eqp = xml.GetNodeList("RewardEqp");
                if (listXml_eqp != null)
                {
                    for (int i = 0; i < listXml_eqp.Count; i++)
                    {
                        if (listXml_eqp[i].getInt("carr") == PlayerModel.getInstance().profession)
                        {
                            uint itemId = listXml_eqp[i].getUint("id");
                            dicTemp[itemId] = -1;
                        }
                    }
                }
                List<SXML> listXml_item = xml.GetNodeList("RewardItem");
                if (listXml_item != null)
                {
                    for (int i = 0; i < listXml_item.Count; i++)
                    {
                        uint itemId = listXml_item[i].getUint("item_id");
                        int num = listXml_item[i].getInt("value");
                        dicTemp[itemId] = num;
                    }
                }

            }

            return dicTemp;
        }

        public Vector3 GetTaskTargetPos(int taskId)
        {
            Vector3 pos = Vector3.zero;
            SXML taskInfo = TaskXML.GetNode("Task", "id==" + taskId);
            if (taskInfo == null)
                return pos;
            float x = taskInfo.getFloat("target_coordinate_x"), y = taskInfo.getFloat("target_coordinate_y");
            pos = new Vector3(x, 0, y);
            return pos;
        }

        //获得任务奖励
        public List<TaskRewardData> GetReward(int taskId)
        {
            var rs = new List<TaskRewardData>();
            SXML curXml = this.TaskXML.GetNode("Task", "id==" + taskId);

            //改成获得任务奖励时算成任务完成
            if(main_task_id == taskId)
                FunctionOpenMgr.instance.onFinshedMainTask(A3_TaskModel.getInstance().main_task_id, true, true);

            if (curXml != null)
            {
                List<SXML> listValue = curXml.GetNodeList("RewardValue");
                if (listValue != null)
                {
                    foreach (SXML xml in listValue)
                    {
                        TaskRewardData da = new TaskRewardData();
                        da.type = 1;
                        da.id = xml.getInt("type");
                        da.num = xml.getInt("value");
                        rs.Add(da);
                    }
                }

                listValue = curXml.GetNodeList("RewardEqp");
                if (listValue != null)
                {
                    foreach (SXML v in listValue)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        a3_EquipData equipdata = new a3_EquipData();

                        uint iid = v.getUint("id");
                        int carr = v.getInt("carr");
                        int stage = v.getInt("stage");
                        int intensify = v.getInt("intensify");
                        bool isSuit = PlayerModel.getInstance().profession == carr;

                        itemData.id = iid;
                        itemData.isEquip = isSuit;
                        equipdata.stage = stage;
                        equipdata.intensify_lv = intensify;
                        itemData.equipdata = equipdata;

                        TaskRewardData da = new TaskRewardData();
                        da.type = 2;
                        da.id = (int)itemData.id;
                        da.item = itemData;
                        rs.Add(da);
                    }
                }

                listValue = curXml.GetNodeList("RewardItem");
                if (listValue != null)
                {
                    foreach (SXML xml in listValue)
                    {
                        TaskRewardData da = new TaskRewardData();
                        da.type = 3;
                        da.id = xml.getInt("item_id");
                        da.num = xml.getInt("value");
                        rs.Add(da);
                    }
                }
            }
            return rs;
        }
        public static readonly int REWARD_CLAN_MONEY = 1;
        public static readonly int REWARD_CLAN_EXP = 2;
        public static readonly int REWARD_CLAN_DONATE = 3;
        public static readonly int REWARD_CLAN_ACTIVE = 4;
        public List<TaskRewardData> GetClanReward(int taskCount)
        {
            var rs = new List<TaskRewardData>();
            SXML curXml = this.TaskXML.GetNode("clan_award", "count==" + (taskCount + 1));
            if (curXml != null)
            {
                SXML lvReward = curXml.GetNode("lvl", "lv==" + A3_LegionModel.getInstance().myLegion.lvl.ToString());
                if (lvReward != null)
                {
                    if (lvReward.hasValue("money"))
                    {
                        TaskRewardData da = new TaskRewardData();
                        da.type = 1;
                        da.id = REWARD_CLAN_MONEY;
                        da.num = lvReward.getInt("money");
                        rs.Add(da);
                    }
                    if (lvReward.hasValue("exp"))
                    {
                        TaskRewardData da = new TaskRewardData();
                        da.type = 1;
                        da.id = REWARD_CLAN_EXP;
                        da.num = lvReward.getInt("exp");
                        rs.Add(da);
                    }
                }

                List<SXML> listValue = curXml.GetNodeList("RewardClan");
                for (int i = 0; i < listValue.Count; i++)
                {
                    TaskRewardData da = new TaskRewardData();
                    da.type = 1;
                    da.id = listValue[i].getInt("type");
                    da.num = listValue[i].getInt("value");
                    if (da.id != REWARD_CLAN_ACTIVE /*不显示军团活跃度*/)
                        rs.Add(da);
                }
            }
            return rs;
        }

        public Dictionary<uint, int> GetClanRewardDic(int taskCount)
        {
            Dictionary<uint, int> dic = new Dictionary<uint, int>();
            SXML curXml = this.TaskXML.GetNode("clan_award", "count==" + (taskCount + 1));
            if (curXml != null)
            {
                SXML lvReward = curXml.GetNode("lvl", "lv==" + A3_LegionModel.getInstance().myLegion.lvl.ToString());
                if (lvReward != null)
                {
                    if (lvReward.hasValue("money"))
                        dic.Add((uint)REWARD_CLAN_MONEY, lvReward.getInt("money"));
                    if (lvReward.hasValue("exp"))
                        dic.Add((uint)REWARD_CLAN_EXP, lvReward.getInt("exp"));
                }

                List<SXML> listValue = curXml.GetNodeList("RewardClan");
                for (int i = 0; i < listValue.Count; i++)
                    if (listValue[i].getUint("type") != REWARD_CLAN_ACTIVE/*不显示军团活跃度*/)
                        dic.Add(listValue[i].getUint("type"), listValue[i].getInt("value"));
            }
            return dic;
        }
        //获得任务数值奖励
        public Dictionary<uint, int> GetValueReward(int taskId)
        {
            Dictionary<uint, int> dicValue = null;
            SXML curXml = this.TaskXML.GetNode("Task", "id==" + taskId);
            if (curXml != null)
            {
                dicValue = new Dictionary<uint, int>();
                List<SXML> listValue = curXml.GetNodeList("RewardValue");
                if (listValue != null)
                {
                    foreach (SXML xml in listValue)
                    {
                        uint type = xml.getUint("type");
                        int value = xml.getInt("value");

                        dicValue.Add(type, value);
                    }
                }
            }
            return dicValue;
        }

        //获得任务装备奖励
        public List<a3_BagItemData> GetEquipReward(int taskId)
        {
            List<a3_BagItemData> listEquip = null;
            SXML curXml = this.TaskXML.GetNode("Task", "id==" + taskId);
            if (curXml != null)
            {
                listEquip = new List<a3_BagItemData>();
                List<SXML> listValue = curXml.GetNodeList("RewardEqp");
                if (listValue != null)
                {
                    foreach (SXML xml in listValue)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        a3_EquipData equipdata = new a3_EquipData();

                        uint iid = xml.getUint("id");
                        int carr = xml.getInt("carr");
                        int stage = xml.getInt("stage");
                        int intensify = xml.getInt("intensify");
                        bool isSuit = PlayerModel.getInstance().profession == carr;

                        itemData.id = iid;
                        itemData.isEquip = isSuit;
                        equipdata.stage = stage;
                        equipdata.intensify_lv = intensify;
                        itemData.equipdata = equipdata;

                        listEquip.Add(itemData);
                    }
                }
            }
            return listEquip;
        }

        //获得物品奖励
        public Dictionary<uint, int> GetItemReward(int taskId)
        {
            Dictionary<uint, int> dicItem = null;
            SXML curXml = this.TaskXML.GetNode("Task", "id==" + taskId);
            if (curXml != null)
            {
                dicItem = new Dictionary<uint, int>();
                List<SXML> listValue = curXml.GetNodeList("RewardItem");
                if (listValue != null)
                {
                    foreach (SXML xml in listValue)
                    {
                        uint type = xml.getUint("item_id");
                        int value = xml.getInt("value");

                        dicItem.Add(type, value);
                    }
                }
            }

            return dicItem;
        }

        //判断采集物是否当前所有任务的采集物之一
        public bool IfCurrentCollectItem(int collectorID)
        {
            foreach (var v in dicPlayerTask.Values)
            {
                SXML curXml = this.TaskXML.GetNode("Task", "id==" + v.taskId);
                if (curXml != null)
                {
                    var id = curXml.getInt("target_param2");
                    if (id == collectorID)
                        return !v.isComplete;
                }
            }
            return false;
        }

        //获得任务的类型
        public TaskType GetTaskTypeById(int taskId)
        {
            TaskType type = TaskType.NULL;

            if (taskId >= 1 && taskId <= 20000)
                type = TaskType.MAIN;
            if (taskId >= 20001 && taskId <= 30000)
                type = TaskType.DAILY;
            if (taskId >= 30001 && taskId <= 40000)
                type = TaskType.BRANCH;
            if (taskId >= 50001 && taskId <= 60000)
                type = TaskType.ENTRUST;

            return type;
        }

        //获得玩家对应类型的任务
        public Dictionary<int, TaskData> GetTaskDataByTaskType(TaskType type)
        {
            Dictionary<int, TaskData> dicTemp = new Dictionary<int, TaskData>();

            foreach (int taskId in dicPlayerTask.Keys)
            {
                TaskType taskT = dicPlayerTask[taskId].taskT;
                if (taskT == type)
                    dicTemp[taskId] = dicPlayerTask[taskId];
            }

            return dicTemp;
        }

        public TaskData GetDailyTask()
        {
            foreach (int taskId in dicPlayerTask.Keys)
            {
                TaskType taskT = dicPlayerTask[taskId].taskT;
                if (taskT == TaskType.DAILY)
                    return dicPlayerTask[taskId];
            }
            return null;
        }
        public TaskData GetEntrustTask()
        {
            foreach (int taskId in dicPlayerTask.Keys)
            {
                TaskType taskT = dicPlayerTask[taskId].taskT;
                if (taskT == TaskType.ENTRUST)
                    return dicPlayerTask[taskId];
            }
            return null;
        }
        public TaskData GetClanTask()
        {
            foreach (int taskId in dicPlayerTask.Keys)
            {
                TaskType taskT = dicPlayerTask[taskId].taskT;
                if (taskT == TaskType.CLAN)
                    return dicPlayerTask[taskId];
            }
            return null;
        }
        //获得此主线任务在章节中第几个
        private int GetMainTaskIndex(int taskId)
        {
            int index = 0;
            TaskData infos = dicPlayerTask[taskId];
            int chapterId = infos.chapterId;

            List<SXML> listXml = TaskXML.GetNodeList("Task", "Chapter_id==" + chapterId);
            for (int i = 0; i < listXml.Count; i++)
            {
                int id = listXml[i].getInt("id");
                if (id == taskId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }


    public class TaskData
    {
        public TaskType taskT;//任务类型
        public string taskName;//任务名字
        public int taskId;//任务ID
        public int taskRate;//任务进度
        public int taskCount;//任务总进度
        public int taskLoop;//任务轮数
        public int taskStar;//任务星级
        public bool isComplete;//是否完成任务
        public TaskTargetType targetType;//任务类型
        public int completion;//目标完成条件(param1)
        public int completionAim;
        public string completionStr;
        public int extraAward;//额外奖励
        public int chapterId;//章节Id
        public int taskMapid;//目标地图id
        public int transto;//任务传送	
        public bool guide;//提示点击
        public int npcId;
        public int completeWay;
        public string story_hint;
        public bool next_step;//是否强制玩家在满足条件时继续任务
        public string explain;//任务面板上的任务描述
        public bool topShowOnLiteminimap;//是否在floatUi上的面板置顶
        public long lose_tm;
        public long release_tm;
        public float need_tm;//读条任务的时间        
        public string extraRateDesc;
        public bool showMessage;
        public bool autoStart = true;
    }

    public class ChapterInfos
    {
        public int id;//章节ID
        public string name;//章节名字
        public string description;//章节描述
    }

    public class TaskRewardData
    {
        public int type;            //类型1非物品、2装备、3道具
        public int id;              //id 1为经验 2为金币,对于军团任务而言 1为军团经验,2为军团资金,3为军团贡献,4为军团活跃
        public a3_BagItemData item; //为装备或道具时
        public int num;             //数量
    }
    public class EntrustExtraRewardData
    {
        public uint tpid;
        public int num;
    }
}

