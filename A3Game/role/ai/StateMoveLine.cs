using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class StateMoveLine : StateBase
    {
        static public StateMoveLine Instance = new StateMoveLine();

        public List<MapLinkInfo> line;
        public Vector3 curtargetPos;

        public Vector3 pos = Vector3.zero;
        public Action handle;
        public override void Enter()
        {
            //Debug.LogError("========> Enter AI MoveLine");
            if (line == null)
            {
                SelfRole.fsm.ChangeState(StateIdle.Instance);
                return;
            }
            if (SelfRole._inst?.m_moveAgent == null) return;
            SelfRole._inst.m_moveAgent.ResetPath();
            refredshPos();
        }


        private void refredshPos()
        {
            if (line.Count == 0)
            {
                if (pos != Vector3.zero)
                    curtargetPos = pos;
                else
                {
                    SelfRole._inst.m_moveAgent.ResetPath();
                    SelfRole._inst.m_moveAgent.Stop();
                    if (handle != null)
                        handle();
                    SelfRole.fsm.ChangeState(StateIdle.Instance);

                }
                return;
            }

            if (GRMap.instance.m_nCurMapID != line[0].mapid)
            {
                line.RemoveAt(0);
                curtargetPos = Vector3.zero;

                if (line.Count == 0)
                {
                    curtargetPos = pos;
                    if (curtargetPos == Vector3.zero)
                        return;

                    SelfRole._inst.SetDestPos(curtargetPos);
                    SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                    SelfRole.ChangeRideAniState( true );
                    waitTick = 3;
                }
            }


            if (curtargetPos == Vector3.zero)
            {
                curtargetPos = new Vector3(line[0].x, 0f, line[0].y);

                SelfRole._inst.SetDestPos(curtargetPos);
                SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                SelfRole.ChangeRideAniState( true );
                waitTick = 5;
            }
            else
            {
                SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                SelfRole.ChangeRideAniState( true );
            }
        }

        private int waitTick = 0;
        private float thinktime = 0;
        public override void Execute(float delta_time)
        {
            if (waitTick > 0)
            {
                waitTick--;
                return;
            }

            if (MapProxy.getInstance().changingMap)
                return;
            if (SelfRole._inst?.m_moveAgent == null) return;
            refredshPos();

            if (waitTick > 0)
            {
                waitTick--;
                return;
            }
            SelfRole._inst.m_curAni?.SetBool(EnumAni.ANI_RUN, true);
            SelfRole.ChangeRideAniState( true );
            if (line.Count == 0)
            {
                Vector3 v = SelfRole._inst.m_moveAgent.destination.ConvertToGamePosition();

                float dis = Vector3.Distance(SelfRole._inst.m_curModel.position.ConvertToGamePosition(), v);//SelfRole._inst.m_moveAgent.remainingDistance;
                if (dis <= SelfRole.fsm.StopDistance)
                {
                    SelfRole._inst?.m_moveAgent?.ResetPath();
                    SelfRole._inst?.m_moveAgent?.Stop();
                    if (handle != null)
                        handle();
                    SelfRole.fsm.ChangeState(StateIdle.Instance);


                    return;
                }



                thinktime += delta_time;
                if (thinktime > 1)
                {
                    thinktime = 0;
                    if (dis < 5f)
                    {
                        Quaternion rot = Quaternion.LookRotation(SelfRole._inst.m_curModel.forward - pos.normalized);
                        if (rot.eulerAngles.y > 10f)
                        {
                            SelfRole._inst?.TurnToPos(pos);
                        }

                    }

                }
            }


        }

        public override void Exit()
        {
            if (SelfRole.fsm.previousState != this)
            {
                handle = null;
                line = null;
                pos = Vector3.zero;
            }
            curtargetPos = Vector3.zero;
            if (SelfRole._inst?.m_moveAgent == null) return;
            SelfRole._inst.m_moveAgent.ResetPath();
            SelfRole._inst.m_moveAgent.Stop();
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
            SelfRole.ChangeRideAniState( false );
        }
    }
}
