using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class NbTask : NewbieTeachItem
    {
       // private int taskId;
        public static NbTask create(string[] arr)
        {
            NbTask nbTask = new NbTask();
            return nbTask;
        }

     

        override public void addListener()
        {
            A3_TaskProxy.getInstance().addEventListener(A3_TaskProxy.ON_GET_NEW_TASK, onHanlde);
        }

        override public void removeListener()
        {
            A3_TaskProxy.getInstance().removeEventListener(A3_TaskProxy.ON_GET_NEW_TASK, onHanlde);
        }
    }
}
