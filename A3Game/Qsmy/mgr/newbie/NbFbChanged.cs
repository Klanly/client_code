using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;

namespace MuGame
{
    class NbFbChanged : NewbieTeachItem
    {
        public static NbFbChanged create(string[] arr)
        {
            NbFbChanged nbFBInit = new NbFbChanged();

            return nbFBInit;
        }



        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_FB_CHANGED, handle);
        }

        public void handle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_FB_CHANGED, handle);
        }
    }
}
