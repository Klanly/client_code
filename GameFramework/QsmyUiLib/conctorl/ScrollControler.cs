using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{

    public class ScrollControler
    {
        private ScrollRect m_scrollR;
        private float m_fSRDragedY;
        private int m_nSRDragCancel = 0;
        private int m_srDragNum;
        public ScrollControler()
        {

        }

        public void create(ScrollRect scroll, int srDragNum = 4)
        {
            m_scrollR = scroll;
            m_srDragNum = srDragNum;
            EventTriggerListener.Get(m_scrollR.gameObject).onDrag = onDragSR;
            EventTriggerListener.Get(m_scrollR.gameObject).onDragEnd = onDragSRend;
            EventTriggerListener.Get(m_scrollR.gameObject).onInPoDrag = onInPoDragSR;
        }

        void onDragSR(GameObject go, Vector2 delta)
        {
            m_nSRDragCancel--;
        }

        void onDragSRend(GameObject go, Vector2 pos)
        {
            if (m_nSRDragCancel > 0)
            {
                float y_offset = (pos.y - m_fSRDragedY) * 5f;

                if (Math.Abs(y_offset) > 2f)
                {
                    Vector2 np = m_scrollR.normalizedPosition;
                    if (np.y > 0.98f) np.y = 0.98f;
                    if (np.y < 0.02f) np.y = 0.02f;

                    m_scrollR.normalizedPosition = np;

                    Vector2 cur = m_scrollR.velocity;
                    cur.y += (pos.y - m_fSRDragedY) * 5f;
                    m_scrollR.velocity = cur;
                }
            }
        }

        void onInPoDragSR(GameObject go, Vector2 pos)
        {
            m_fSRDragedY = pos.y;
            m_nSRDragCancel = 4;
        }
    }
}
