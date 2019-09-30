using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;

namespace MuGame
{

    /// <summary>
    /// 空闲状态
    /// 
    /// 1. 直接进入search状态
	/// 2. 进入collect状态
    /// </summary>
    class StateIdle : StateBase
    {
        static public StateIdle Instance = new StateIdle();

        public override void Enter()
        {
           SelfRole._inst?.m_moveAgent?.Stop();
            //if (SelfRole.fsm.previousState != this)
            //    MoveProxy.getInstance().SendMoveMsgToServer(SelfRole._inst.m_curModel.position, SelfRole._inst.m_curModel.position);
        }

        public override void Execute(float delta_time)
        {
            if (SelfRole.fsm.Autofighting && SelfRole.fsm.IsPause == false)
            {
                if ((SelfRole._inst.m_LockRole != null && PlayerModel.getInstance().pk_state != PK_TYPE.PK_PEACE) ||
                    (SelfRole._inst.m_LockRole is MonsterRole && PlayerModel.getInstance().pk_state == PK_TYPE.PK_PEACE))
                    SelfRole.fsm.ChangeState(StateAttack.Instance);
                else
                    SelfRole.fsm.ChangeState(StateSearch.Instance);
            }
            if (SelfRole.fsm.AutoCollect && SelfRole.fsm.IsPause == false)
            {
                SelfRole.fsm.ChangeState(StateCollect.Instance);
            }

        }

        public override void Exit()
        {
        }
    }
}
