using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using GameFramework;
namespace MuGame
{
   public class NbCheckItems
    {
        public delegate bool boolDelegate(string[] arr);
        public static bool checkLV(string[] arr)
        {
            int lv = int.Parse(arr[1]);
            return PlayerModel.getInstance().lvl == lv;
        }

        public static bool checkZhuan(string[] arr)
        {
            int zhuan = int.Parse(arr[1]);
            return PlayerModel.getInstance().up_lvl == zhuan;
        }

        public static bool checkCurMap(string[] arr)
        {
            string mapid = arr[1];
            if (GRMap.instance == null || GRMap.instance.m_map == null)
                return false;
            return mapid == GRMap.instance.m_map.id;
        }

        public static bool checkCurNotMap(string[] arr)
        {
            string mapid = arr[1];
            if (GRMap.instance == null || GRMap.instance.m_map == null)
                return false;
            return mapid != GRMap.instance.m_map.id;
        }

        public static bool checkPlayingPlot(string[] arr)
        {
            bool b = int.Parse(arr[1]) == 1;

            return b == GRMap.playingPlot;
        }
        public static bool checkFbInited(string[] arr)
        {

            return false;//fb_3d.instance != null && fb_3d.instance.fbinited;
        }
        public static bool noCheck(string[] arr)
        {
            return true;
        }

        public static bool checkWinOpen(string[] arr)
        {
            string winid = arr[1];
            return InterfaceMgr.getInstance().checkWinOpened(winid);
        }

        public static bool checkCurTask(string[] arr)
        {
            int taskId = int.Parse(arr[1]);
            int taskState = int.Parse(arr[2]);
            TaskData taskData = A3_TaskModel.getInstance().GetTaskDataById(A3_TaskModel.getInstance().main_task_id);
            if (taskState == 0)
            {
                return taskId == A3_TaskModel.getInstance().main_task_id;// && ! taskData.isComplete;
            }
            else
            {
                return taskId == A3_TaskModel.getInstance().main_task_id;// && taskData.isComplete;
                
            }

        }

        public static bool checkHaveItem(string[] arr)
        {
            string iremid =arr[1];
            return false;//BagModel.getInstance().getItemNumById(iremid)>0;
        }

        public static bool checkWinClose(string[] arr)
        {
            string winid = arr[1];
            return !InterfaceMgr.getInstance().checkWinOpened(winid);
        }

      


        public static bool checkClick(string[] arr)
        {
           

            if (EventSystem.current.currentSelectedGameObject == null)
                return false;

            string clickname = arr[1];
            return clickname == EventSystem.current.currentSelectedGameObject.name;
        }


        public static bool checkSignCount(string[] arr)
        {
            int count = int.Parse(arr[1]);
            return false;//sign.curcount == count;
        }

        public static bool checkLastClose(string[] arr)
        {
            string winid = arr[1];
            return winid == UiEventCenter.lastClosedWinID;
        }

        public static Dictionary<string, boolDelegate> dCheck;
        public static void init()
        {
            dCheck = new Dictionary<string, boolDelegate>();
            dCheck["lv"] = checkLV;
            dCheck["zhuan"] = checkZhuan;
            dCheck["map"] = checkCurMap;
            dCheck["win"] = checkWinOpen;
            dCheck["task"] = checkCurTask;
            dCheck["click"] = checkClick;
            dCheck["unwin"] = checkWinClose;
            dCheck["haveitem"] = checkHaveItem;
            dCheck["lastclose"] = checkLastClose;
     
   

            dCheck["plot"] = checkPlayingPlot;

            dCheck["fbinited"] = checkFbInited;
            dCheck["noCheck"] = noCheck;
      }


        private List<NbCheckItem> lItems = new List<NbCheckItem>();
        public NbCheckItems(string str)
        {
            if (dCheck == null)
                init();

            string[] arrCheck = str.Split('|');
            for (int i = 0; i < arrCheck.Length; i++)
            {
                string[] arr = arrCheck[i].Split(',');
                if (dCheck.ContainsKey(arr[0]))
                {
                    lItems.Add(new NbCheckItem(dCheck[arr[0]], arr));
                }
                else
                {
                    Debug.LogError("新手初始化错误:::" + arr[0]);
                }
            }
        }

        public bool doCheck()
        {
            for(int i=0; i<lItems.Count;i++)
            {
                if(!lItems[i].doit())
                    return false;
            }
            return true;
        }


    }

   public class NbCheckItem
    {
        public NbCheckItems.boolDelegate _chekAction;
        public string[] _pram;

        public NbCheckItem(NbCheckItems.boolDelegate chekAction, string[] pram)
        {
            _chekAction = chekAction;
            _pram = pram;
        }

        public bool doit()
        {
            return _chekAction(_pram);
        }
    }
}
