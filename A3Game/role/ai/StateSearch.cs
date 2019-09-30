using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;
using Random = System.Random;

namespace MuGame
{
    /// <summary>
    /// 处理挂机点
    /// </summary>
    class StateSearch : StateBase
    {
        public bool onTaskMonsterSearch { get { return a3_task_auto.instance.onTaskSearchMon; } set { a3_task_auto.instance.onTaskSearchMon = value; } }
        public static StateSearch Instance = new StateSearch();
        private float thinktm = 0.0f;

        public override void Enter()
        {
            thinktm = 1000.0f;
            
        }

        bool isOutOfAutoPlayRange => StateInit.Instance.IsOutOfAutoPlayRange();
        public override void Execute(float delta_time)
        {
            thinktm += delta_time;
            if (thinktm < 0.25f)
                return;
            thinktm = 0;
            if (isOutOfAutoPlayRange)
            {
                StateAutoMoveToPos.Instance.pos = StateInit.Instance.Origin;
                StateAutoMoveToPos.Instance.stopdistance = 0.3f;
                SelfRole.fsm.ChangeState(StateAutoMoveToPos.Instance);            
                //SelfRole._inst.m_LockRole = null;
                return;
            }
            Vector3 curpos = SelfRole._inst.m_curModel.position;
            MonsterRole mon = MonsterMgr._inst.FindNearestMonster(/*SelfRole.fsm.Autofighting ? StateInit.Instance.Origin : */curpos, onTask: onTaskMonsterSearch);
            if (mon == null)
            {
                //!--如果玩家周围没有monster,寻找新的随机的挂点
                Vector3 autopoint;
                bool result = FindRandomPropAutoPoint(curpos, out autopoint);
                if (result &&
                    Vector2.Distance(
                        new Vector2(StateInit.Instance.Origin.x, StateInit.Instance.Origin.z), 
                        new Vector2(autopoint.x, autopoint.z)
                    ) < StateInit.Instance.Distance)
                {
                    //!--圈内有挂点
                    if (Vector3.Distance(autopoint, curpos) <= 2.0f)
                        return;
                    StateAutoMoveToPos.Instance.pos = autopoint;
                    StateAutoMoveToPos.Instance.stopdistance = 2.0f;
                    SelfRole.fsm.ChangeState(StateAutoMoveToPos.Instance);
                    return;
                }
                if (!PlayerModel.getInstance().havePet|| (PlayerModel.getInstance().havePet && PlayerModel.getInstance().last_time == 0))
                    SelfRole.fsm.ChangeState(StatePick.Instance);
                return;
            }

            //!--有怪物在视野,直接开始攻击        
            SelfRole._inst.m_LockRole = mon;
            SelfRole.fsm.ChangeState(StateAttack.Instance);            
        }

        public override void Exit()
        {
        }

        /// <summary>
        /// 寻找随机的挂机点
        /// </summary>
        private bool FindRandomPropAutoPoint(Vector3 curpos, out Vector3 autopoint)
        {
            List<Vector3> aps = StateInit.Instance.AutoPoints;            
            if (aps == null || aps.Count == 0)
            {
                autopoint = Vector3.zero;
                return false;
            }

            Random rd = new Random();
            int idx = rd.Next(aps.Count);
            autopoint = new Vector3( aps[idx].x, aps[idx].y, aps[idx].z);
            return true;
        }
    }
}
