using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    public class StateMachine
    {
        //!--当前状态
        public StateBase currentState;
        //!--上一个状态
        public StateBase previousState;
        //!--全局状态
        public StateBase globalState;
        //!--初始状态
        public StateBase initState;
        //!--代理状态
        public StateBase proxyState;

        public Action<bool> OnFSMStartStop = null;

        // --计算挂机时与起点的距离
        public float DistanceNowAndOri => StateInit.Instance.Origin != Vector3.zero ?
            Vector3.Distance(
                a: new Vector3(StateInit.Instance.Origin.x, 0, StateInit.Instance.Origin.z),
                b: new Vector3(SelfRole._inst.m_curModel.position.x, 0, SelfRole._inst.m_curModel.position.z)
            ) : 0f;

        // --玩家手动操作移动时的起始位置
        public Vector3 manBeginPos;
        public bool CheckJoystickMoveDis(Vector3 ori,float maxDis) =>
            Vector3.Distance(
                a: new Vector3(manBeginPos.x, 0, manBeginPos.z),
                b: new Vector3(ori.x, 0, ori.z)
            ) > maxDis;
        //!--是否开启自动战斗
        private bool autofighting = false;
        public bool Autofighting
        {
            get { return autofighting; }
            private set
            {
                if (autofighting ^ value)
                {
                    autofighting = value;
                    OnFSMStartStop?.Invoke(value);
                    StateInit.Instance.Origin = SelfRole._inst.m_curModel.position;
                    if (!value)
                    {
                        MonsterMgr._inst.taskMonId = null;
                    }
                }

            }
        }

		//!--自动采集
		private bool autoCollect = false;
		public bool AutoCollect {
			get { return autoCollect; }
			set {
				autoCollect = value;
			}
		}
        public float StopDistance = 0.3f;
        //!--是否暂停自动战斗
        public bool IsPause { get; set; }
        // --<自动战斗> 玩家是否正在返回途中
        //private bool isReturning;
        public StateMachine()
        {
            IsPause = false;

            currentState = null;
            previousState = null;
            globalState = null;
            proxyState = null;

            skill_a3Data skdata = null;
            int skid = -1;
            switch (PlayerModel.getInstance().profession)
            {
                default:
                case 2: skid = 2001; break;
                case 3: skid = 3001; break;
                case 5: skid = 5001; break;
            }
            Skill_a3Model.getInstance().skilldic.TryGetValue(skid, out skdata);
            StateAttack.MinRange = skdata.range / GameConstant.PIXEL_TRANS_UNITYPOS;
        }

        public void Configure(StateBase initState, StateBase globalState, StateBase proxyState)
        {
            this.initState = initState;
            this.globalState = globalState;
            this.proxyState = proxyState;
        }

        public void Update(float delta_time)
        {
            if (proxyState != null)
                proxyState.Execute(delta_time);

            if (globalState != null)
                globalState.Execute(delta_time);

            if (currentState != null)
                currentState.Execute(delta_time);

            if (autofighting && (SelfRole._inst.m_LockRole == null && DistanceNowAndOri > StateInit.Instance.Distance))
                MoveToOri();
       }
        public void MoveToOri()
        {
            //SelfRole.moveto(pos: StateInit.Instance.Origin, dis: 2f, forceStop:false);
            StateAutoMoveToPos.Instance.pos = StateInit.Instance.Origin;
            StateAutoMoveToPos.Instance.stopdistance = 0.3f;
            SelfRole.fsm.ChangeState(StateAutoMoveToPos.Instance);
            SelfRole._inst.m_LockRole = null;
        }
        public void RestartState(StateBase currentState)
        {
            StateInit.Instance.LockOriPos = true;
            currentState?.Exit();
            currentState?.Enter();
            StateInit.Instance.LockOriPos = false;
        }
        public void ChangeState(StateBase newState)
        {
            previousState = currentState;
            if(currentState != null)
                currentState.Exit();

            currentState = newState;

            if(currentState != null)
                currentState.Enter();
        }

        public void RevertToPreviousState()
        {
            if(previousState != null)
                ChangeState(previousState);
        }

        public void StartAutofight()
        {
            StateProxy.Instance.remindNotEnoughMoney = true;
            Autofighting = true;
            IsPause = false;
            ChangeState(initState);
        }

		public void StartAutoCollect() {
			AutoCollect = true;
			IsPause = false;
		}

        public void Stop()
        {            
            //A3_TaskOpt.Instance?.ResetStat();
            //SelfRole._inst.m_LockRole = null;
            Autofighting = false;
			AutoCollect = false;
            //if (a3_task_auto.instance != null)
            //    a3_task_auto.instance.executeTask = null;
            cd.hide();
            ChangeState(StateIdle.Instance);            
            ClearAutoConfig(); // 如果任务需要跨地图打怪,再在这里加个判定是否使用ClearAutoConfig
        }
        public void ClearAutoConfig()
        {
            SelfRole.UnderTaskAutoMove = false;
            foreach (var item in PlayerModel.getInstance().task_monsterIdOnAttack)
                if (!PlayerModel.getInstance().task_monsterId.ContainsKey(item.Key))
                    PlayerModel.getInstance().task_monsterId.Add(item.Key, item.Value);
            PlayerModel.getInstance().task_monsterIdOnAttack.Clear();
            StateSearch.Instance.onTaskMonsterSearch = false;
            if (a3_task_auto.instance.onTaskSearchMon)
            {
                StateSearch.Instance.onTaskMonsterSearch = false;
                a3_task_auto.instance.PauseAutoKill();
            }
        }
        public void Pause()
        {
            IsPause = true;
            ChangeState(StateIdle.Instance);
        }

        public void Resume()
        {
            IsPause = false;
            ChangeState(StateIdle.Instance);
        }
    }
}
