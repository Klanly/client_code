using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace MuGame
{

   public class DoAfterMgr
    {
        private List<Action> listAfterRender = new List<Action>();
        public void addAfterRender(Action fun)
        {
            listAfterRender.Add(fun);
        }

        public void onAfterRender()
        {
            if (listAfterRender.Count == 0)
                return;

            foreach (Action fun in listAfterRender)
            {
                fun();
            }
            listAfterRender.Clear();
        }

   

    


        public static DoAfterMgr instacne;
        public static DoAfterMgr init()
        {
            if (instacne == null)
                instacne = new DoAfterMgr();
            return instacne;
        }
    }


}
