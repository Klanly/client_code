using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
namespace MuGame
{
    class NbWipeOutOver : NewbieTeachItem
    {
        public static NbWipeOutOver create(string[] arr)
        {
            NbWipeOutOver nb = new NbWipeOutOver();

            return nb;
        }

     

        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_FB_WIPEOUT_OVER, handle);
        }

        public void handle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_FB_WIPEOUT_OVER, handle);
        }
    }
}
