using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    public class TickMgr
    {
        public static int tickNum = 0;

        private List<TickItem> ticks = new List<TickItem>();
        public void update(float delta)
        {

            List<TickItem> delItem = null;
            foreach (TickItem item in ticks)
            {
                if (item.isTicking == false)
                {
                    if (delItem == null)
                        delItem = new List<TickItem>();
                    delItem.Add(item);
                }
                else
                {
                    item.tick(delta);
                }
            }



            if (delItem != null)
            {
                foreach (TickItem item in delItem)
                {
                    ticks.Remove(item);
                }

            }

            if (ticksNeedAdd.Count > 0)
            {
                foreach (TickItem itm in ticksNeedAdd)
                {
                    ticks.Add(itm);
                }
                ticksNeedAdd.Clear();
            }
        }

        public void updateAfterRender()
        {
            tickNum++;
            if (tickNum > 60000)
                tickNum = 0;
        }

        private List<TickItem> ticksNeedAdd = new List<TickItem>();
        public void addTick(TickItem tick)
        {
            if (tick.isTicking)
                return;

            ticksNeedAdd.Add(tick);
            tick.isTicking = true;
        }

        public void removeTick(TickItem tick)
        {
            tick.isTicking = false;
        }


        public static TickMgr instance = new TickMgr();


    }

    public class TickItem
    {
        public bool isTicking = false;
        public Action<float> tick;

        public TickItem(Action<float> tickHandle)
        {
            tick = tickHandle;

        }
    }
}
