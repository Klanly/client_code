using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;

namespace MuGame
{
    class NbFbAniOver : NewbieTeachItem
    {
        public static NbFbAniOver create(string[] arr)
        {
            NbFbAniOver nbFBInit = new NbFbAniOver();

            return nbFBInit;
        }



        override public void addListener()
        {
            UiEventCenter.getInstance().addEventListener(UiEventCenter.EVENT_FB_ANI_OVER, handle);
        }

        public void handle(GameEvent e)
        {
            onHanlde(e);
        }

        override public void removeListener()
        {
            UiEventCenter.getInstance().removeEventListener(UiEventCenter.EVENT_FB_ANI_OVER, handle);
        }
    }
}
