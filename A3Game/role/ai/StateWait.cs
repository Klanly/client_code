using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class StateWait : StateBase
    {
        static public StateWait Instance = new StateWait();

        public float wait_time = 0.0f;
        private float timer = 0;

        public override void Enter()
        {
            timer = 0;
        }

        public override void Execute(float delta_time)
        {
            timer += delta_time;
            if (timer < wait_time)
                return;

            SelfRole.fsm.RevertToPreviousState();
        }

        public override void Exit()
        {
            timer = 0;
        }
    }
}
