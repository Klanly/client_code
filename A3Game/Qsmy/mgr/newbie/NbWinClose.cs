using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
namespace MuGame
{
    class NbWinClose : NewbieTeachItem
    {
        public string winId = "";
        public static NbWinClose create(string[] arr)
        {
            NbWinClose nb = new NbWinClose();

            if (arr.Length > 1)
            {
                nb.winId = arr[1];
            }
            return nb;
        }

        //override public bool check()
        //{
        //    return InterfaceMgr.getInstance().checkWinOpened(winName);
        //}

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_WIN_CLOSE, dohandle);
        }

        private void dohandle(GameEvent e)
        {
            if (winId=="" || winId == e.orgdata.ToString())
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_WIN_CLOSE, dohandle);
        }
    }
}
