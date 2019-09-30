using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;

namespace MuGame
{
    public class GameEventTrigger : TriggerHanldePoint
    {

        private static Dictionary<GameEventTrigger, int> dListeners = new Dictionary<GameEventTrigger, int>();
        public static void add(GameEventTrigger listener)
        {
            dListeners[listener] = 1;

        }
        public static void remove(GameEventTrigger listener)
        {
            listener.dispose();
            dListeners.Remove(listener);
        }
        public static void clear()
        {
            foreach (GameEventTrigger item in dListeners.Keys)
            {
                item.dispose();
            }
            dListeners.Clear();
        }


       

        public static Transform EVENT_CON;
        public int type;
        public int curNum;

        private int targetId;
        private int maxNum;

        override public void onTriggerHanlde()
        {
            if (EVENT_CON == null)
            {
                GameObject go = new GameObject();
                EVENT_CON = go.transform;
                go.name = "currentEventListener";
            }

            if (type == 1)
            {
                if (paramInts.Count < 2)
                    return;

                targetId = paramInts[0];
                maxNum = paramInts[1];
                MonsterMgr._inst.addEventListener(MonsterMgr.EVENT_MONSTER_REMOVED, onMonsterRemoved);
                transform.SetParent(EVENT_CON);
                add(this);
            }
            else if (type == 2)
            {
                MonsterMgr._inst.addEventListener(MonsterMgr.EVENT_ROLE_BORN, onRoleBorn);
                transform.SetParent(EVENT_CON);
                add(this);
            }
            else if (type == 3)
            {
                if (paramFloat.Count < 1)
                    return;

                Invoke("ontimeout", paramFloat[0]);
                transform.SetParent(EVENT_CON);
                add(this);
            }

        }

        void ontimeout()
        {
            CancelInvoke("ontimeout");
            doit();
            remove(this);
        }

        void onRoleBorn(GameEvent e)
        {
            doit();
            remove(this);
        }


        void onMonsterRemoved(GameEvent e)
        {
            MonsterRole m = (MonsterRole)e.orgdata;
            if (targetId == 0 || targetId == m.monsterid)
            {
                curNum++;
                if (curNum >= maxNum)
                {
                    doit();
                    remove(this);
                }
            }

        }


        void doit()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                TriggerHanldePoint hd = transform.GetChild(i).GetComponent<TriggerHanldePoint>();
                if (hd != null)
                    hd.onTriggerHanlde();
            }
        }

        public void dispose()
        {
            MonsterMgr._inst.removeEventListener(MonsterMgr.EVENT_ROLE_BORN, onRoleBorn);
            MonsterMgr._inst.removeEventListener(MonsterMgr.EVENT_MONSTER_REMOVED, onMonsterRemoved);
            Destroy(gameObject);
        }

    }
}
