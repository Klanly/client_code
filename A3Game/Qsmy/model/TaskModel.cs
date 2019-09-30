using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using System.Collections;
namespace MuGame
{
    class TaskModel : ModelBase<TaskModel>
    {
        ////public Dictionary<string, TaskData> taskDatas;
        //public TaskData mainTask;
        //public Dictionary<int, TaskData> jobTask;
        //public Dictionary<int, TaskData> dayTask;
        //public int isNeedToOpenMapId = -1;
        //public bool isSubTask = false;
        //public int isNeedToOpenTiLi = -1;
        //public TaskModel()
        //    : base()
        //{
        //    mainTask = new TaskData();
        //    jobTask = new Dictionary<int, TaskData>();
        //    dayTask = new Dictionary<int, TaskData>();
        //}
   
        //public void addJobTask(TaskData data)
        //{
        //    if (data.id < 2000 && data.id >= 5000)
        //        return;
        //    jobTask[data.id] = data;

        //    jobTask.QSTDOrderBy();
        //}
        //public void addDayTask(TaskData data)
        //{
        //    if (data.id < 5000)
        //    {
        //        return;
        //    }
        //    dayTask[data.id] = data;
        //    //if (dayTask.ContainsKey(data.id))
        //    //{
        //    //    dayTask[data.id] = data;
        //    //}
        //    //else
        //    //{
        //    //    dayTask.Add(data.id, data);
        //    //}
            
        //}
        //public void changeTask(TaskData data)
        //{
        //    if (data.id < 2000)
        //    {
        //        mainTask = data;
        //    }
        //    else if (data.id < 5000)
        //    {
        //        if (jobTask.ContainsKey(data.id))
        //        {
        //            jobTask[data.id] = data;
        //        }
        //        else
        //        {
        //            jobTask.Add(data.id,data);
        //        }
        //    }
        //    else 
        //    {
        //        if (dayTask.ContainsKey(data.id))
        //        {
        //            dayTask[data.id] = data;
        //        }
        //        else
        //        {
        //            dayTask.Add(data.id, data);
        //        }
        //    }
        //}
        //public TaskData getTaskById(int id)
        //{
        //    if (mainTask.id == id)
        //    {
        //        return mainTask;
        //    }
        //    else if (jobTask.ContainsKey(id))
        //    {
        //        return jobTask[id];
        //    }
        //    else if (dayTask.ContainsKey(id))
        //    {
        //        return dayTask[id];
        //    }
        //    else
        //    {
        //        return new TaskData();
        //    }
        //}
        //public void removeTaskById(int id)
        //{
        //    if (id < 2000)
        //    {
        //        mainTask = new TaskData();
        //    }
        //    else if (id < 5000)
        //    {
        //        jobTask.Remove(id);
        //    }
        //    else
        //    {
        //        dayTask.Remove(id);
        //    }
        //}
        //public TaskData getTaskDataById(int id)
        //{
        //    SXML s_xml = XMLMgr.instance.GetSXML("task.Task", "id==" + id);

        //    TaskData data = new TaskData();
        //    data.id = id;
        //    if (s_xml != null)
        //    {
        //        data.name = s_xml.getString("name");
        //        data.rewardList = new List<BaseItemData>();

        //        SXML s_reward = s_xml.GetNode("RewardValue", null);
        //        if (s_reward != null)
        //        {
        //            do
        //            {
        //                BaseItemData rewardData = new BaseItemData();
        //                rewardData.id = s_reward.getString("type");
        //                rewardData.num = s_reward.getInt("value");
        //                data.rewardList.Add(rewardData);
        //            } while (s_reward.nextOne());
        //        }

        //        s_reward = s_xml.GetNode("RewardItem", null);
        //        if (s_reward != null)
        //        {
        //            do
        //            {
        //                BaseItemData rewardData = new BaseItemData();
        //                rewardData.id = s_reward.getString("item_id");
        //                rewardData.num = s_reward.getInt("value");
        //                data.rewardList.Add(rewardData);
        //            } while (s_reward.nextOne());
        //        }
        //    }
        //    return data;
        //}
        //public void showGet(int id)
        //{
        //    //List<TaskRewardData> datas = new List<TaskRewardData>();
        //    //SXML s_xml = XMLMgr.instance.GetSXML("task.Task", "id==" + id);
        //    //if (s_xml != null)
        //    //{
        //    //    SXML s_reward = s_xml.GetNode("RewardItem", null);
        //    //    if (s_reward != null)
        //    //    {
        //    //        do
        //    //        {
        //    //            TaskRewardData rewardData = new TaskRewardData();
        //    //            rewardData.id = s_reward.getString("item_id");
        //    //            rewardData.num = s_reward.getInt("value");
        //    //            datas.Add(rewardData);
        //    //        } while (s_reward.nextOne());
        //    //    }
        //    //}
        //    ArrayList list = new ArrayList();
        //    list.Add(getTaskDataById(id).rewardList);
        //    InterfaceMgr.getInstance().open(InterfaceMgr.GETTING, list);
        //}
    }
    //public class TaskData
    //{
    //    public int id;
    //    public int cnt;
    //    public bool isEnd;
    //    public string name;
    //    public List<BaseItemData> rewardList;
    //}
   
}
