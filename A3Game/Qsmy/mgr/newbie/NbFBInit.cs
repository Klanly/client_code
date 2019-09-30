using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;

namespace MuGame
{
    class NbFBInit : NewbieTeachItem
    {
        public static NbFBInit create(string[] arr)
        {
            NbFBInit nbFBInit = new NbFBInit();

            return nbFBInit;
        }

    

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_FB_INITED, handle);
        }

        public void handle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_FB_INITED, handle);
        }
    }
}
