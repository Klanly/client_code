using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;

namespace MuGame
{
    class StateGlobal : StateBase
    {
        public static StateGlobal Instance = new StateGlobal();

        public override void Enter()
        {
        }

        public override void Execute(float delta_time)
        {
            if (SelfRole._inst.isDead)
            {
                SelfRole.fsm.ChangeState(StateDie.Instance);
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
