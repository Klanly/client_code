using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
namespace MuGame
{
    class TaskProxy : BaseProxy<TaskProxy>
    {
        //public static uint ON_LOAD_TASKS = 0;
        //public static uint ON_LOAD_DAT_TASKS = 1;
        //public static uint ON_FINISH_TASK = 2;
        //public static uint ON_CHANGE_TASK = 3;

        //public TaskProxy()
        //{
        //    addProxyListener(PKG_NAME.C2S_ACCEPT_MIS, onLoadTask);
        //    addProxyListener(PKG_NAME.S2C_ON_DMIS_RES, onLoadDayTask);
        //    addProxyListener(PKG_NAME.C2S_COMMIT_MIS, onFinishTask);
        //    addProxyListener(PKG_NAME.C2S_CHANGE_MIS, onChange);
        //}
        //public void sendLoadTask()
        //{
        //    sendRPC(PKG_NAME.C2S_ACCEPT_MIS, null);
        //}
        //public void sendFinishTask(int id)
        //{
        //    Variant msg = new Variant();
        //    msg["id"] = id;
        //    sendRPC(PKG_NAME.C2S_COMMIT_MIS, msg);
        //}
        //void onLoadTask(Variant data)
        //{
        //    if (data.ContainsKey("res"))
        //    {
        //        int res = data["res"];
        //        if (res < 0)
        //        {
        //            Globle.err_output(res);
        //            return;
        //        }
        //        Variant info = data["info"]["mis"];
        //        bool hasMainTask = false;
        //        foreach (Variant item in info._arr)
        //        {
        //            TaskData one = TaskModel.getInstance().getTaskDataById(item["misid"]);
        //            one.cnt = item["cnt"];

        //            SXML xml = XMLMgr.instance.GetSXML("task.Task", "id==" + item["misid"]);
        //            int cnt = xml.getInt("target_param1");
        //            if (one.cnt < cnt)
        //            {
        //                one.isEnd = false;
        //            }
        //            else
        //            {
        //                one.isEnd = true;
        //                IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_TASK);
        //                IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
        //            }

        //            if (one.id < 2000)
        //            {
        //                hasMainTask = true;
        //                TaskModel.getInstance().mainTask = one;
        //            }
        //            else
        //            {
        //                TaskModel.getInstance().addJobTask(one);
        //            }
        //        }

        //        if (!hasMainTask)
        //        {//表示没有主线任务
        //            TaskModel.getInstance().mainTask = new TaskData();
        //            TaskModel.getInstance().mainTask.id = -1;
        //        }

        //        dispatchEvent(GameEvent.Create(ON_LOAD_TASKS, this, null));
        //    }
            

        //    if (data.ContainsKey("misid"))
        //    {
        //        int id = data["misid"];
        //        int cnt = data["cnt"];

        //        TaskData one = TaskModel.getInstance().getTaskDataById(id);
        //        one.cnt = cnt;

        //        SXML xml = XMLMgr.instance.GetSXML("task.Task", "id==" + id);
        //        int need_cnt = xml.getInt("target_param1");
        //        if (one.cnt < need_cnt)
        //        {
        //            one.isEnd = false;
        //        }
        //        else
        //        {
        //            one.isEnd = true;
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_TASK);
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
        //        }

        //        if (one.id < 2000)
        //        {
        //            TaskModel.getInstance().mainTask = one;
        //        }
        //        else
        //        {
        //            TaskModel.getInstance().addJobTask(one);
        //        }
        //    }

        //}
        //void onLoadDayTask(Variant data)
        //{
        //    Variant info = data["info"];
        //    foreach (Variant item in info._arr)
        //    {
        //        TaskData one = TaskModel.getInstance().getTaskDataById(item["id"]);
        //        one.cnt = item["cnt"];

        //        if (one.id < 5000)
        //        {
        //            return;
        //        }

        //        SXML xml = XMLMgr.instance.GetSXML("task.Task", "id==" + item["id"]);
        //        int cnt = xml.getInt("target_param1");
        //        if (one.cnt < cnt)
        //        {
        //            one.isEnd = false;
        //        }
        //        else
        //        {
        //            one.isEnd = true;
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_TASK);
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
        //        }

        //        if (one.isEnd == false && xml.getInt("screen") == 1)
        //        {//屏蔽掉未完成的领取月卡奖励

        //        }
        //        else
        //        {
        //            TaskModel.getInstance().addDayTask(one);
        //        }
        //    }

        //    dispatchEvent(GameEvent.Create(ON_LOAD_DAT_TASKS, this, null));
        //}
        //void onFinishTask(Variant data)
        //{
        //    Variant info = data;
        //    int res = data["res"];
        //    if (res < 0)
        //    {
        //        Globle.err_output(res);
        //        return;
        //    }
        //    int id = data["id"];
        //    TaskModel.getInstance().removeTaskById(id);

        //    dispatchEvent(GameEvent.Create(ON_FINISH_TASK, this, data));

        //    TaskModel.getInstance().showGet(id);
        //}
        //void onChange(Variant data)
        //{
        //    TaskData one = TaskModel.getInstance().getTaskDataById(data["id"]);
        //    one.cnt = data["cnt"];
        //    SXML xml = XMLMgr.instance.GetSXML("task.Task", "id==" + data["id"]);
        //    int cnt = xml.getInt("target_param1");
        //    if (one.cnt < cnt)
        //    {
        //        one.isEnd = false;
        //    }
        //    else
        //    {
        //        one.isEnd = true;
        //        if ((int)data["id"] > 5000 && PlayerModel.getInstance().lvl < 10)
        //        {

        //        }
        //        else
        //        {
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_TASK);
        //            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
        //        }
        //    }
        //    TaskModel.getInstance().changeTask(one);
        //    dispatchEvent(GameEvent.Create(ON_CHANGE_TASK, this, null));
        //}
    }
}
