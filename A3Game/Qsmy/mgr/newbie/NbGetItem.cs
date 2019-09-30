using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
namespace MuGame
{
    class NbGetItem : NewbieTeachItem
    {
        public string itemid;
        public int itemNum;

        public static NbGetItem create(string[] arr)
        {
            NbGetItem nbChangeWorld = new NbGetItem();

            if (arr.Length == 3)
            {
                nbChangeWorld.itemid = arr[1];
                nbChangeWorld.itemNum = int.Parse(arr[2]);
            }
            else
            {
                Debug.LogError("NbGetItem参数数量错误：" + arr.Length);
            }


            return nbChangeWorld;
        }

        //override public bool check()
        //{
        //    if (GRMap.instance == null || GRMap.instance.m_map == null)
        //        return false;

        //    return GRMap.instance.m_map.id == mapid;
        //}

        override public void addListener()
        {
            UIClient.instance.addEventListener(UI_EVENT.ON_CHANGE_ITEM, handle);
            //  UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_MAP_CHANGED, handle);
        }

        public void handle(GameEvent e)
        {
            Variant data = e.data;
            foreach (Variant item in data._arr)
            {
               string id = item["id"];

               if (id != itemid)
                   continue;

               if (itemNum <= BagModel.getInstance().getItemNumById(id))
                   onHanlde(e);

            }
        }

        override public void removeListener()
        {
            UIClient.instance.removeEventListener(UI_EVENT.ON_CHANGE_ITEM, handle);
            //   UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_MAP_CHANGED, handle);
        }

    }
}
