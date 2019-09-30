using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
namespace MuGame
{
    class StateAutoMoveToPos : StateBase
    {
        static public StateAutoMoveToPos Instance = new StateAutoMoveToPos();
        private int priority;
        public bool forceFaceToPos = true;
        public float stopdistance { get { return SelfRole.fsm.StopDistance; } set { SelfRole.fsm.StopDistance = value; } }
        public Vector3 pos = Vector3.zero;
        public Action handle;
        public override void Enter()
        {

            SelfRole._inst?.m_moveAgent?.ResetPath();
            priority = SelfRole._inst.m_moveAgent.avoidancePriority;
            if (pos == Vector3.zero || Vector3.Distance(SelfRole._inst.m_curModel.position.ConvertToGamePosition(), pos.ConvertToGamePosition()) < stopdistance)
            {
                SelfRole._inst.m_moveAgent?.Stop();
                handle?.Invoke();
                SelfRole.fsm.ChangeState(StateIdle.Instance);
                return;
            }

            NavMeshHit hit;
            NavMesh.SamplePosition(pos, out hit, StateInit.Instance.Distance, NavmeshUtils.allARE);
            if(hit.position.x != Mathf.Infinity && hit.position.y != Mathf.Infinity && hit.position.z != Mathf.Infinity)
                pos = hit.position;
            SelfRole._inst.SetDestPos(pos);
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
            SelfRole.ChangeRideAniState( true );

            waitTick = 3;
            //SelfRole._inst.moving = true;
        }

        private int waitTick = 0;
        private float thinktime = 0;
        private float stayTime = 0;
        private Vector3 lastStay;
        public override void Execute(float delta_time)
        {
            //if (StateInit.Instance.Origin != Vector3.zero && 
            //    Vector3.Distance(
            //        a: StateInit.Instance.Origin,
            //        b: new Vector3(SelfRole._inst.m_curModel.position.x, 0, SelfRole._inst.m_curModel.position.z
            //        )) < 2f/*任务导航到一定距离内*/)
            //{
            //    SelfRole.UnderTaskAutoMove = false; //导航结束
            //    if (!SelfRole.fsm.Autofighting)
            //        SelfRole.fsm.StartAutofight();
            //    SelfRole.fsm.ChangeState(StateAttack.Instance);
            //    return;
            //}           
            try
            {
                if (SelfRole._inst.m_curAni.GetBool(EnumAni.ANI_RUN) && Vector3.Distance(SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition(), lastStay) < 0.2f)
                {
                    stayTime += delta_time;
                    if (stayTime > 2f)
                    {
                        stayTime = 0f;
                        if (SelfRole.fsm.Autofighting)
                        {
                            SelfRole.fsm.RestartState(StateSearch.Instance);
                            return;
                        }
                    }
                }
            }
            catch (Exception e) { SelfRole.fsm.Stop(); }
            if (SelfRole._inst.m_moveAgent != null)
                SelfRole._inst.m_moveAgent.avoidancePriority = 1;
            if ( SelfRole._inst.m_curAni != null ) {
                SelfRole._inst.m_curAni.SetBool( EnumAni.ANI_RUN , true );
                SelfRole.ChangeRideAniState( true );
            }
            //Debug.LogError(SelfRole._inst.m_curAni+ "=>SelfRole._inst.m_curAni");
            if (waitTick > 0)
            {
                waitTick--;
                return;
            }
            if (SelfRole._inst.m_moveAgent == null) return;
            Vector3 cur = SelfRole._inst.m_curModel.position;
            
            SelfRole._inst.m_moveAgent.SetDestination(pos);
            //SelfRole._inst.m_moveAgent.SetDestination(pos = hit.position);
            //SelfRole._inst.SetDestPos(pos);
            float dis = Vector3.Distance(pos.ConvertToGamePosition(), cur.ConvertToGamePosition());
            if (dis <= stopdistance)
            {
                SelfRole._inst?.m_moveAgent?.Stop();
                handle?.Invoke();
                /*SelfRole.fsm.isExecuteTask = SelfRole.UnderTaskAutoMove;*/
                if (!SelfRole.fsm.Autofighting)
                    SelfRole.fsm.ChangeState(StateIdle.Instance);
                else
                    SelfRole.fsm.ChangeState(StateAttack.Instance);
                return;
            }

            //if (forceFaceToPos)
            //{
            //    NavMeshPath path = new NavMeshPath();
            //    SelfRole._inst.m_moveAgent.CalculatePath(SelfRole._inst.m_roleDta.pos, path);
            //    if (path.corners.Length > 1)
            //        SelfRole._inst.TurnToPos(path.corners[1]);
            //    //SelfRole._inst.TurnToPos(pos);
            //    return;
            //}

            thinktime += delta_time;
            if (thinktime > 1)
            {
                thinktime = 0;
                if (dis < 5f)
                {
                    Quaternion rot = Quaternion.LookRotation(SelfRole._inst.m_curModel.forward - pos.normalized);
                    if (rot.eulerAngles.y > 10f)
                    {
                        SelfRole._inst.TurnToPos(pos);
                    }
                }
            }
            lastStay = SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition();
        }

        public override void Exit()
        {
            if (SelfRole._inst.m_moveAgent != null)
                SelfRole._inst.m_moveAgent.avoidancePriority = priority;
            if (SelfRole.fsm.previousState != this)
            {
                //handle = null;
                pos = Vector3.zero;
            }
            handle = null;
            try
            {
                SelfRole._inst?.m_moveAgent?.Stop(); 
                SelfRole._inst?.m_curAni?.SetBool(EnumAni.ANI_RUN, false);
                SelfRole.ChangeRideAniState(false);
                lastStay = SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition();
            }
            catch (Exception) { lastStay = Vector3.zero; }
            stopdistance = 0.3f;
            //SelfRole._inst.moving = false;
        }
    }
}
