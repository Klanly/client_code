using System;
using System.Collections.Generic;


namespace GameFramework
{
 
		public class taskEvent
		{
            static protected LinkedList<taskEvent> m_pool = new LinkedList<taskEvent>();

			public GameEvent evt;
			public Action<GameEvent> listenerFun = null;

            static public taskEvent alloc()
            {
                if (m_pool.Count > 0)
                {
                    taskEvent evt = m_pool.Last.Value;
                    m_pool.RemoveLast();
                    return evt;
                }

                return new taskEvent();
            }

            static public void free(taskEvent te)
            {
                if (te == null)
                    return;

                te.evt = null;
                te.listenerFun = null;

                m_pool.AddLast(te);
            }
		}

 
}
