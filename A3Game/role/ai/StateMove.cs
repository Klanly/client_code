using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    class StateMove : StateBase
    {
        public static StateMove Instance = new StateMove();

        public float stop_distance = 0.125f; 

        public override void Enter()
        {
            //debug.Log("========> Enter AI Move");
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
        }

        public override void Execute(float delta_time)
        {
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);

            BaseRole target = SelfRole._inst.m_LockRole;
            if (target == null)
            {//目标丢失, 切换至空闲状态
                SelfRole.fsm.ChangeState(StateIdle.Instance);
                return;
            }
            Vector3 hisPos = target.m_curModel.position;
            Vector3 myPos = SelfRole._inst.m_curModel.position;

            SelfRole._inst.TurnToPos(hisPos);

            if (Vector3.Distance(hisPos, myPos) < stop_distance)
            {//距离内，进入攻击状态
                SelfRole.fsm.ChangeState(StateAttack.Instance);
                return;
            }
            else
                SelfRole._inst.m_rshelper.CheckMoveAgent(delta_time,resetImmediately: true);
        }

        public override void Exit()
        {
            SelfRole._inst.StopMove();
        }
    }
}
