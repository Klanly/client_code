using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    class StatePK : StateBase
    {
        static public StatePK Instance = new StatePK();

        private Vector3 startPos;
        private float timer = 0;
        public ProfessionRole Enemy;

        public override void Enter()
        {
            //debug.Log("========> Enter AI PK");
            startPos = SelfRole._inst.m_curModel.position;
            SelfRole._inst.m_LockRole = Enemy;
        }
        bool TargetLost { get; set; } = false;
        public override void Execute(float delta_time)
        {
            timer += delta_time;
            if (timer < 0.1f)
                return;
            timer -= 1.0f;

            if (TargetLost || Vector3.Distance(startPos, SelfRole._inst.m_curModel.position) > StateInit.Instance.PKDistance ||
                Enemy.isDead)
            {//!--超出追击距离，返回最近的挂机点
                Vector3 backpt = StateInit.Instance.GetNearestWayPoint();
                StateAutoMoveToPos.Instance.pos = backpt;
                StateAutoMoveToPos.Instance.stopdistance = 2.0f;
                SelfRole.fsm.ChangeState(StateAutoMoveToPos.Instance);
                return;
            }

            //!--获取可以使用的技能,并检查攻击距离
            int skid = StateInit.Instance.GetSkillCanUse();
            skill_a3Data skdata = null;
            Skill_a3Model.getInstance().skilldic.TryGetValue(skid, out skdata);
            float range = skdata.range / GameConstant.PIXEL_TRANS_UNITYPOS;
            TargetLost = Enemy.m_curModel == null;
            float enemyDis = Vector3.Distance(Enemy.m_curModel.position, SelfRole._inst.m_curModel.position);
            if (enemyDis > range)
            {
                SelfRole._inst.m_moveAgent.destination = Enemy.m_curModel.position;
                SelfRole._inst.m_moveAgent.stoppingDistance = range - 0.125f;
                SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
            }
            else
            {
                SelfRole._inst.m_moveAgent.Stop();
                SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                SelfRole._inst.TurnToRole(Enemy,true);
                bool ret = a1_gamejoy.inst_skillbar.playSkillById(skid);
                if (ret == true && StateInit.Instance.PreferedSkill == skid)
                {
                    StateInit.Instance.PreferedSkill = -1;
                }
            }
        }

        public override void Exit()
        {
            SelfRole._inst.m_moveAgent.Stop();
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
        }
    }
}
