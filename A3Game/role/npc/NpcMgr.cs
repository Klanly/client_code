using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class NpcMgr
    {
        public static NpcMgr instance = new NpcMgr();
        public bool can_touch = true;
        private Dictionary<int, NpcRole> dNpc = new Dictionary<int, NpcRole>();

        public Dictionary<int, NpcXml> xNpc = new Dictionary<int, NpcXml>();
        public NpcMgr()
        {
            A3_TaskModel.getInstance().addEventListener(A3_TaskModel.ON_NPC_TASK_STATE_CHANGE, OnNpcTaskStateChange);
            initNpcXml();
        }
        public void onMapLoaded()
        {
            List<Variant> cacheProxy = A3_TaskModel.getInstance().cacheProxy;
            for (int i = 0; i < A3_TaskModel.getInstance().cacheProxy.Count; i++)
                OnNpcTaskStateChange(GameEvent.Create(0, null, cacheProxy[i]));
            A3_TaskModel.getInstance().cacheProxy.Clear();
        }
        private void initNpcXml()
        {
            xNpc.Clear();
            var xml = XMLMgr.instance.GetSXML("npcs");
            var list = xml.GetNodeList("npc");
            foreach (var v in list)
            {
                NpcXml item = new NpcXml();
                item.id = v.getInt("id");
                item.head_icon = v.getInt("head_icon");
                item.map_id = v.getInt("map_id");
                item.model = v.getString("model");
                xNpc[item.id] = item;
            }
        }


        public void addRole(NpcRole npc)
        {
            if (dNpc.ContainsKey(npc.id))
                return;

            dNpc[npc.id] = npc;

            CheckNpcTaskState(npc);
        }


        public NpcRole getRole(int id)
        {
            if (!dNpc.ContainsKey(id))
                return null;

            return dNpc[id];
        }


        public void clear()
        {
            foreach (NpcRole npc in dNpc.Values)
            {
                npc.dispose();
            }
            dNpc.Clear();
        }

        #region npc任务相关
      
        //设置npc的任务状态
        public void SetNpcTaskState(NpcRole npc, NpcTaskState taskState)
        {
            if (npc == null)
                return;

            int npcId = npc.id;

            npc.refreshTaskIcon(taskState);

            if (taskState != NpcTaskState.NONE)
            {
                npc.listTaskId = dicNpcTaskState[npc.id].Keys.ToList<int>();
            }
            else
            {
                npc.listTaskId = null;
            }

         //   npc.OnRefreshTitle();
        }

        //npc任务状态变换
        private Dictionary<int, Dictionary<int, int>> dicNpcTaskState = new Dictionary<int, Dictionary<int, int>>();
        private void OnNpcTaskStateChange(GameEvent e)
        {
            Variant data = e.data;

            int npcId = data["npcId"];
            int taskId = data["taskId"];
            int taskState = data["taskState"];

            if (!dicNpcTaskState.ContainsKey(npcId))
            {
                Dictionary<int, int> dicTemp = new Dictionary<int, int>();
                dicTemp[taskId] = taskState;
                dicNpcTaskState[npcId] = dicTemp;
            }
            else
            {
                if (taskState == 0)
                    dicNpcTaskState[npcId].Remove(taskId);
                else
                    dicNpcTaskState[npcId][taskId] = taskState;
            }

            NpcRole nRole = getRole(npcId);

            List<int> listState = dicNpcTaskState[npcId].Values.ToList<int>();
            int state = 0;
            if(listState.Count > 0)
                state = listState.Max<int>();

            SetNpcTaskState(nRole, (NpcTaskState)state);
            TaskData taskData = A3_TaskModel.getInstance().GetTaskDataById(taskId);
            if (taskData != null)
            {
                if (taskData.topShowOnLiteminimap)//#判空处理
                                   
                    a3_liteMinimap.instance?.SetTopShow(taskId);
                
                else
                a3_liteMinimap.instance?.RefreshTaskPage(taskId);
            }
        }

        //检查npc的任务状态
        private void CheckNpcTaskState(NpcRole npc)
        {
            int npcid = npc.id;

            if (dicNpcTaskState.ContainsKey(npcid))
            {
                Dictionary<int, int> taskState = dicNpcTaskState[npcid];

                List<int> listState = taskState.Values.ToList<int>();
                int state = 0;
                if(listState.Count >0)
                   state = listState.Max<int>();
                SetNpcTaskState(npc, (NpcTaskState)state);
            }
        }

        #endregion

      
    }
    public enum NpcTaskState
    {
        NONE = 0,
        UNREACHED = 1,
        REACHED = 2,
        UNFINISHED = 3,
        FINISHED = 4,
    }

    public class NpcXml
    {
        public int id;
        public int head_icon;
        public int map_id;
        public string model;
    }
}
