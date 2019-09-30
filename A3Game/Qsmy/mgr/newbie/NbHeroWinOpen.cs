using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
namespace MuGame
{
    class NbHeroWinOpen : NewbieTeachItem
    {
        //  private string winName;
        public static NbHeroWinOpen create(string[] arr)
        {
            //if (param.Length != paramNum)
            //{

            //    return null;
            //}

            NbHeroWinOpen nbwinopen = new NbHeroWinOpen();
            //  nbwinopen.winName = param[1].ToString();

            return nbwinopen;
        }

        //override public bool check()
        //{
        //    return InterfaceMgr.getInstance().checkWinOpened(winName);
        //}

        override public void addListener()
        {


            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_HERO_WIN_OPEN, doHandle);
        }


        public void doHandle(GameEvent e)
        {
          
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_HERO_WIN_OPEN, doHandle);
        }
    }
}
