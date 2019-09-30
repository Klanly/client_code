using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
namespace MuGame
{
    class NbChangeWorld:NewbieTeachItem
    {
        public static NbChangeWorld create(string[] arr)
        {
            NbChangeWorld nbChangeWorld = new NbChangeWorld();

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
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_MAP_CHANGED, handle);
        }

        public void handle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_MAP_CHANGED, handle);
        }

        

    }
}
