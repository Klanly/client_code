using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    class StateDie : StateBase
    {
        public static StateDie Instance = new StateDie();

        private bool hasSend = false;

        public override void Enter()
        {
            //debug.Log("========> Enter AI Die");
            hasSend = false;
        }

        public override void Execute(float delta_time)
        {
            if (!SelfRole._inst.isDead)
            {//活了
                SelfRole.fsm.ChangeState(StateIdle.Instance);
                return;
            }

            //!--死亡自动复活主逻辑,移至a3_relive类
        }

        public override void Exit()
        {
            hasSend = true;
        }
    }
}
