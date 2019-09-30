//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Cross;
//using GameFramework;

////已经不用了
//namespace MuGame
//{
//    class NavContorler : MonoBehaviour
//    {
//        public Variant roleD;

//        public bool running = true;

//        NavMeshAgent agent;
//        public Action<string, bool> playani;
//        private string locoState = "Locomotion_Stand";

//        public bool isMain = false;

//        private Vector3 linkStart;
//        private Vector3 linkEnd;
//        private Quaternion linkRotate;

//        Transform _target;

//        private string curAni = "idle";

//        public Action<Variant> _onReachHandle;
//        Variant _onReachDta;
//        void Start()
//        {
//            agent = GetComponent<NavMeshAgent>();
//            if (agent == null)
//                agent = gameObject.AddComponent<NavMeshAgent>();
//            agent.acceleration = 9999f;
//            agent.angularSpeed = 360f;
//            agent.autoTraverseOffMeshLink = false;
//            agent.speed = _speed;
//            agent.radius = 0.1f;
//            StartCoroutine(AnimationStateMachine());

//            if (targetVec != null)
//                agent.SetDestination(targetVec);

//        }

//        Vector3 targetVec;
//        float _distance = 0;
//        public void SetDestination(Vector3 vec, float distance = 0, Action<Variant> onReach = null, Variant onReachDta = null, Transform target = null)
//        {
//            _onReachHandle = onReach;
//            _onReachDta = onReachDta;
//            _distance = distance;
//            targetVec = vec;
//            _target = target;

//            if (!running)
//                return;

//            if (agent)
//            {
//                agent.stoppingDistance = distance;
//                if (Vector3.Distance(vec, transform.position) <= distance)
//                {
//                    onArive();
//                }
//                else
//                {
//                    agent.SetDestination(vec);
//                }
//            }
//        }

//        public bool isReach()
//        {
//            if (agent == null)
//                return true;

//            return agent.remainingDistance > _distance;
//        }


//        public void dispose()
//        {
//            _target = null;
//            Destroy(agent);
//            agent = null;
//            StopAllCoroutines();
//            Destroy(this);
//        }

//        public void Stop()
//        {
//            _onReachHandle = null;
//            _distance = 0;

//            if (!running)
//                return;
//            agent.SetDestination(transform.position);
//            agent.Stop(true);
//        }

//        private void onArive()
//        {
//           // if (_onReachHandle != null)
//           // {
//           //     _onReachHandle(_onReachDta);
//           //     _onReachHandle = null;
//           // }

//           // if (_target != null)
//           // {
//           //     Quaternion rot = Quaternion.LookRotation(_target.position - transform.position);
//           //     rot.z = 0;
//           //     transform.rotation = rot;
//           //     _target = null;
//           // }

//           // if (isMain)
//           // {
//           //     long tm = NetClient.instance.CurServerTimeStampMS;
//           //     MoveProxy.getInstance().sendstop(
//           //(uint)(transform.position.x * GameConstant.PIXEL_TRANS_UNITYPOS),
//           //    (uint)(transform.position.y * GameConstant.PIXEL_TRANS_UNITYPOS),
//           //    1,
//           //    tm);
//           // }
//        }


//        float _speed = 0;
//        public float speed
//        {
//            set
//            {
//                _speed = value;
//                if (agent)
//                    agent.speed = value;
//            }
//            get { return _speed; }
//        }

//        IEnumerator AnimationStateMachine()
//        {
//            while (running)
//            {
//                yield return StartCoroutine(locoState);
//            }
//        }

//        IEnumerator Locomotion_Stand()
//        {
//            do
//            {
//                if (!running)
//                    yield break;

//                if (curAni != "idle")
//                {

//                    curAni = "idle";
//                    playani("idle", true);
//                }
//                //  UpdateAnimationBlend();
//                yield return null;
//            } while (agent.remainingDistance <= _distance);

//            locoState = "Locomotion_Move";
//            yield return null;
//        }

//        IEnumerator Locomotion_Move()
//        {
//            do
//            {
//                if (!running)
//                    yield break;


//                if (isMain)
//                    LGCamera.instance.updateMainPlayerPos();

//                if ( curAni != "run")
//                {

//                    curAni = "run";
//                    playani("run", true);
//                }

//            //    UpdateAnimationBlend();
//                yield return null;

//                if (agent.active == false)
//                {
//                    yield return null;
//                }

//                if (agent.isOnOffMeshLink)
//                {
//                    locoState = SelectLinkAnimation();
//                    yield break;
//                    //  yield return null;  
//                }
//            } while (agent.remainingDistance > _distance);

//            onArive();
//            locoState = "Locomotion_Stand";
//            yield return null;
//        }

//        IEnumerator Locomotion_Jump()
//        {

//            // string linkAnim = "jump";
//            // Vector3 posStart = transform.position;

//            // agent.Stop(true);
//            //// anim.CrossFade(linkAnim, 0.1f, PlayMode.StopAll);
//            // //if (playani != null)
//            // //    playani("jump", false);
//            // transform.rotation = linkRotate;

//            // do
//            // {
//            //     //计算新的位置
//            //     float tlerp = anim[linkAnim].normalizedTime;
//            //     Vector3 newPos = Vector3.Lerp(posStart, linkEnd, tlerp);
//            //     newPos.y += 10f * Mathf.Sin(3.14159f * tlerp);
//            //     transform.position = newPos;

//            //     yield return new WaitForSeconds(0);
//            // } while (anim[linkAnim].normalizedTime < 1);
//            // //动画恢复到Idle
//            // anim.Play("idle");
//            // agent.CompleteOffMeshLink();
//            // agent.Resume();
//            // //下一个状态为Stand
//            // transform.position = linkEnd;
//            // locoState = "Locomotion_Stand";
//            yield return null;
//        }



//        private string SelectLinkAnimation()
//        {
//            OffMeshLinkData link = agent.currentOffMeshLinkData;
//            float distS = (transform.position - link.startPos).magnitude;
//            float distE = (transform.position - link.endPos).magnitude;

//            if (distS < distE)
//            {
//                linkStart = link.startPos;
//                linkEnd = link.endPos;
//            }
//            else
//            {
//                linkStart = link.endPos;
//                linkEnd = link.startPos;
//            }

//            Vector3 alignDir = linkEnd - linkStart;
//            alignDir.y = 0;
//            linkRotate = Quaternion.LookRotation(alignDir);

//            if (link.linkType == OffMeshLinkType.LinkTypeManual)
//            {
//                // return ("Locomotion_Ladder");
//                return ("Locomotion_Jump");
//            }
//            else
//            {
//                return ("Locomotion_Jump");
//            }
//        }



//        private void UpdateAnimationBlend()
//        {
//            float walkAnimationSpeed = 1.5f;
//            float runAnimationSpeed = 4.0f;
//            //   float speedThreshold = 0.1f;

//            Vector3 velocityXZ = new Vector3(agent.velocity.x, 0.0f, agent.velocity.z);
//            float speed = velocityXZ.magnitude;

//            //  anim["run"].speed = speed / runAnimationSpeed;


//            //   if (speed > (walkAnimationSpeed + runAnimationSpeed) / 2)
//            if (speed > 0)
//            {
//                //  anim.CrossFade("run");

//                if (playani != null && curAni != "run")
//                {
//                    debug.Log(":::run  " + speed);
//                    curAni = "run";
//                    playani("run", true);
//                }

//            }
//            //else if (speed > speedThreshold)
//            //{
//            //    anim.CrossFade("Walk");
//            //}
//            else
//            {
//                if (playani != null && curAni != "idle")
//                {
//                    debug.Log(":::idle  " + speed);
//                    curAni = "idle";
//                    playani("idle", true);
//                }

//                //anim.CrossFade("idle", 0.1f, PlayMode.StopAll);
//            }
//        }

//    }
//}
