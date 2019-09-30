using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
	class StateCollect : StateBase
	{
		public static StateCollect Instance = new StateCollect();
		private CollectRole collTarget = null;
		public bool collecting;

		public override void Enter() {
			collecting = false;
		}

		public override void Execute(float delta_time) {
			if (collTarget == null) {
				foreach (MonsterRole m in MonsterMgr._inst.m_mapMonster.Values) {
					if (m is CollectRole && A3_TaskModel.getInstance().IfCurrentCollectItem(m.monsterid)) {
						var vv = m as CollectRole;
						if (!vv.becollected) {
							collTarget = vv;
							break;
						}
					}
				}
				if (collTarget != null) {
					SelfRole._inst.TurnToPos(collTarget.m_curModel.transform.position);
					SelfRole._inst.SetDestPos(collTarget.m_curModel.transform.position);
					if (!SelfRole._inst.m_moveAgent.hasPath) {
                        SelfRole._inst.m_moveAgent.ResetPath();
                        SelfRole._inst.m_moveAgent.Stop();
						return;
					}
					else {
						SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                        SelfRole.ChangeRideAniState(true);
                    }
				}
			}
            if (SelfRole._inst.m_curModel == null) return;//加载模型时
            if (collTarget == null)
            {
                SelfRole._inst.m_moveAgent.ResetPath();
                SelfRole._inst.m_moveAgent.Stop();
				SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                SelfRole.ChangeRideAniState( false );
            }
			else if (!collecting) {
                if (Vector3.Distance(collTarget.m_curModel.transform.position, SelfRole._inst.m_curModel.transform.position) <= 2)
                {
                    SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                    SelfRole.ChangeRideAniState( false );
                    collTarget.onClick();
                    collecting = true;
                }
                else
                {
                    SelfRole._inst.TurnToPos(collTarget.m_curModel.transform.position);
                    SelfRole._inst.SetDestPos(collTarget.m_curModel.transform.position);
                    SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                    SelfRole.ChangeRideAniState( true );
                }
			}

		}

		public override void Exit() {
            SelfRole._inst.m_moveAgent.ResetPath();
            SelfRole._inst.m_moveAgent.Stop();
			SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
            SelfRole.ChangeRideAniState( false );
            collecting = false;
			SelfRole.fsm.AutoCollect = false;
			collTarget = null;
		}
	}
}
