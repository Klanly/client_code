using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
namespace MuGame
{
    class NbWinOpen : NewbieTeachItem
    {
      //  private string winName;
        public static NbWinOpen create(string[] arr)
        {
            //if (param.Length != paramNum)
            //{

            //    return null;
            //}

            NbWinOpen nbwinopen = new NbWinOpen();
          //  nbwinopen.winName = param[1].ToString();

            return nbwinopen;
        }

        //override public bool check()
        //{
        //    return InterfaceMgr.getInstance().checkWinOpened(winName);
        //}

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_WIN_OPEN, doHandle);
        }

        public void doHandle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_WIN_OPEN, doHandle);
        }
    }
}
