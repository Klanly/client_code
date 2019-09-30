using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;
using GameFramework;

namespace MuGame
{
    /// <summary>
    /// 进入攻击状态:寻找最近的怪
    /// 
    /// </summary>
    class StateAttack : StateBase
    {
        public static StateAttack Instance = new StateAttack();
        private int mon = 0;
        private long exe_action_tm = 0;
        private float xml_action_tm;
        //public static float minRange;         
        public static float MinRange = 2f;
        //{
        //    set { minRange = value;/*Mathf.Max(value,1.5f); */}
        //    get { return minRange; }
        //} 
        public override void Enter()
        {
            if (muNetCleint.instance.CurServerTimeStamp - exe_action_tm < xml_action_tm || SelfRole._inst.isPlayingSkill)
                return;
            BattleProxy.getInstance().addEventListener(BattleProxy.EVENT_SELF_KILL_MON, OnKillMon);
            //xml_action_tm = 0.0f;
            CHECK_AGAIN: CheckPK();
            if (!SelfRole.UnderPlayerAttack)
            {
                if (PlayerModel.getInstance().pk_state != PK_TYPE.PK_PEACE)
                {
                    BaseRole plr = OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.transform.position, selector: EnemySelector, pkState: PlayerModel.getInstance().pk_state);
                    if (plr != null)
                    {
                        SelfRole._inst.m_LockRole = plr;
                        return;
                    }
                }
                if ((SelfRole._inst.m_LockRole is ProfessionRole && PlayerModel.getInstance().pk_state == PK_TYPE.PK_PEACE) ||
                        SelfRole._inst.m_LockRole == null)
                {
                    MonsterRole mon = MonsterMgr._inst.FindNearestMonster(/*SelfRole.fsm.Autofighting ? StateInit.Instance.Origin :*/ SelfRole._inst.m_curModel.position);
                    if (mon == null)
                    {
                        SelfRole.fsm.ChangeState(StateIdle.Instance);
                        return;
                    }
                    SelfRole._inst.m_LockRole = mon;
                }
                else
                {
                    if (SelfRole.fsm.Autofighting
                        && Vector3.Distance(SelfRole._inst.m_LockRole.m_curModel.position.ConvertToGamePosition(), SelfRole._inst.m_curModel.position.ConvertToGamePosition()) > StateInit.Instance.Distance)
                    {
                        SelfRole._inst.m_LockRole = null;
                        SelfRole.fsm.ChangeState(StateIdle.Instance);
                    }
                }
                return;
            }
            else
            {
                if (SelfRole.LastAttackPlayer != null)
                    SelfRole._inst.m_LockRole = SelfRole.LastAttackPlayer;
                else
                {
                    SelfRole.UnderPlayerAttack = false;
                    goto CHECK_AGAIN;
                }
            }
        }

        public float StateTimeover { get; set; }
        public override void Execute(float delta_time)
        {
            //exe_action_tm += delta_time;
            //Debug.LogError(string.Format("当前时间:{0},技能开始时间：{1},时间间隔:{2},技能的执行时间:{3}", muNetCleint.instance.CurServerTimeStamp, exe_action_tm, muNetCleint.instance.CurServerTimeStamp - exe_action_tm, xml_action_tm));
            if (muNetCleint.instance.CurServerTimeStamp - exe_action_tm < xml_action_tm || SelfRole._inst.isPlayingSkill)
            {
                return;
            }

            if (mon > 0) //每击杀[一次](不是一只)怪物,额外触发一次拾取物品
            {
                mon--;
                if (!PlayerModel.getInstance().havePet|| (PlayerModel.getInstance().havePet && PlayerModel.getInstance().last_time == 0))
                    SelfRole.fsm.ChangeState(StatePick.Instance);
                return;
            }

            if (SelfRole._inst.m_LockRole != null)
                if (SelfRole._inst.m_LockRole is CollectRole)
                    SelfRole._inst.m_LockRole = null;
                else if (PlayerModel.getInstance().pk_state == PK_TYPE.PK_PEACE)
                {
                    if (SelfRole._inst.m_LockRole is ProfessionRole)
                        SelfRole._inst.m_LockRole = null;
                }
                else if (PlayerModel.getInstance().pk_state == PK_TYPE.PK_TEAM && TeamProxy.getInstance().MyTeamData != null)
                    if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Exists((m) => m.cid == SelfRole._inst.m_LockRole.m_unCID))
                        SelfRole._inst.m_LockRole = null;

            //if (SelfRole._inst.m_LockRole is ProfessionRole &&
            //    SelfRole._inst.m_ePK_Type == PK_TYPE.PK_PEACE) // 玩家在和平状态下挂机,手动锁定一个玩家时
            //    SelfRole._inst.m_LockRole = null;
            if (SelfRole._inst.m_LockRole == null)
            {
                if (PlayerModel.getInstance().pk_state != PK_TYPE.PK_PEACE)
                {
                    BaseRole plr = OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.transform.position, selector: EnemySelector, pkState: PlayerModel.getInstance().pk_state);
                    if (plr != null)
                        SelfRole._inst.m_LockRole = plr;
                }
                if (SelfRole._inst.m_LockRole == null)
                {
                    //if (SelfRole._inst.m_LockRole != null /* || SelfRole._inst.m_LockRole is MonsterRole*/)
                    SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestMonster(SelfRole._inst.m_curModel.transform.position);
                    MonsterRole mon = SelfRole._inst.m_LockRole as MonsterRole;
                    try
                    {
                        if (PlayerModel.getInstance().task_monsterIdOnAttack.Count != 0 &&
                            !PlayerModel.getInstance().task_monsterIdOnAttack.ContainsValue(mon.monsterid))
                        {
                            SelfRole._inst.m_LockRole = null;
                            SelfRole.fsm.ChangeState(StateIdle.Instance);
                            return;
                        }
                    }
                    catch (Exception)   // 玩家在"角色接受任务后开始自动战斗"时,猛点空白区域,会引发此异常
                                        // 原因是SelfRole._inst.m_LockRole被强制置为null,导致mon未引用到实例对象
                    {
                        SelfRole._inst.m_LockRole = null;
                        SelfRole.fsm.ChangeState(StateIdle.Instance);
                        return;
                    }

                }
            }
            BaseRole target = SelfRole._inst.m_LockRole;
            try
            {
                Vector3 targetPosition = SelfRole._inst.m_LockRole.m_curModel.position;
            }
            catch (Exception)
            {
                SelfRole._inst.m_LockRole = null;
                SelfRole.fsm.RestartState(this);
                return;
            }
            if (target == null || target.isDead)
            {
                StateTimeover += delta_time;
                if (StateTimeover > 1.5f)
                {
                    StateTimeover = 0f;
                    SelfRole.fsm.MoveToOri();
                    return;
                }
                SelfRole._inst.m_LockRole = null;
                //if (BaseRoomItem.instance.dDropItem.Count != 0)
                //    SelfRole.fsm.ChangeState(StatePick.Instance);
                //else
                //    SelfRole.fsm.RestartState(this);
                if (BaseRoomItem.instance.dDropItem_own.Count != 0)
                {
                    if(!PlayerModel.getInstance().havePet|| (PlayerModel.getInstance().havePet && PlayerModel.getInstance().last_time == 0))
                      SelfRole.fsm.ChangeState(StatePick.Instance);                      
                }
                else
                    SelfRole.fsm.RestartState(this);
                return;
            }

            StateTimeover = 0;
            int skid = StateInit.Instance.GetSkillCanUse();
            skill_a3Data skdata = null;
            Skill_a3Model.getInstance().skilldic.TryGetValue(skid, out skdata);
            float range = skdata.range / GameConstant.PIXEL_TRANS_UNITYPOS;
            if (skdata.skillType != 0 && Vector3.Distance(target.m_curModel.position, SelfRole._inst.m_curModel.position) > MinRange)
            {
                if (range < Vector3.Distance(SelfRole._inst.m_curModel.position, target.m_curModel.position))
                {
                    StateAutoMoveToPos.Instance.stopdistance = 2.0f;
                    StateAutoMoveToPos.Instance.pos = SelfRole._inst.m_LockRole.m_curModel.position;                    
                    SelfRole.fsm.ChangeState(StateAutoMoveToPos.Instance);
                    return;
                }
            }            
            SelfRole._inst.TurnToPos(SelfRole._inst.m_LockRole.m_curModel.position);
            exe_action_tm = muNetCleint.instance.CurServerTimeStamp;
            switch (skid)
            {
                case 2001:
                case 3001:
                case 4001:
                case 5001:
                    xml_action_tm = 0;
                    break;
                default:
                    xml_action_tm = skdata.action_tm * 0.1f;
                    break;
            }
            bool ret = a1_gamejoy.inst_skillbar.playSkillById(skid);
            if (ret)
            {
                if (StateInit.Instance.PreferedSkill == skid)
                    StateInit.Instance.PreferedSkill = -1;
            }
            else
                xml_action_tm = 0f;
        }

        public override void Exit()
        {
            BattleProxy.getInstance().removeEventListener(BattleProxy.EVENT_SELF_KILL_MON, OnKillMon);
        }

        private void CheckPK()
        {
            ProfessionRole castRole = SelfRole.LastAttackPlayer;
            if (castRole == null) return;
            switch (SelfRole._inst.m_ePK_Type)
            {
                case PK_TYPE.PK_PKALL:
                    SelfRole._inst.m_LockRole = castRole;
                    SelfRole.LastAttackPlayer = castRole;
                    break;
                case PK_TYPE.PK_TEAM:
                    if (castRole.m_unTeamID != SelfRole._inst.m_unTeamID || !PlayerModel.getInstance().IsInATeam)
                    {
                        SelfRole._inst.m_LockRole = castRole;
                        SelfRole.LastAttackPlayer = castRole;
                    }
                    else
                    {
                        SelfRole.UnderPlayerAttack = false;
                        SelfRole._inst.m_LockRole = null;
                        SelfRole.LastAttackPlayer = null;
                    }
                    break;
                case PK_TYPE.PK_PEACE:
                case PK_TYPE.Pk_SPOET:
                    if (castRole.lvlsideid != PlayerModel.getInstance().lvlsideid)
                    {
                        SelfRole._inst.m_LockRole = castRole;
                        SelfRole.LastAttackPlayer = castRole;
                    }
                    else {
                        SelfRole.UnderPlayerAttack = false;
                        SelfRole._inst.m_LockRole = null;
                        SelfRole.LastAttackPlayer = null;
                    }
                    break;
                default:
                    SelfRole.UnderPlayerAttack = false;
                    SelfRole._inst.m_LockRole = null;
                    SelfRole.LastAttackPlayer = null;
                    break;
            }

        }
        private void OnKillMon(GameEvent e) => mon++;
        private bool EnemySelector(ProfessionRole p)
        {
            if (p == null)
                return true;
            if (PlayerModel.getInstance().pk_state == PK_TYPE.PK_PKALL)
                return false; // 在全体攻击模式下,没有需要过滤的目标
            else if (PlayerModel.getInstance().pk_state == PK_TYPE.PK_TEAM)
            {
                if (TeamProxy.getInstance().MyTeamData == null || TeamProxy.getInstance().MyTeamData.itemTeamDataList == null)
                    return false; //在团队模式下,如果没有队伍,则没有需要过滤的目标
                else if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Exists((m) => m.cid == p.m_unCID))
                {
                    if (SelfRole._inst.m_LockRole == p) // 正在攻击我的角色成为了我的队员,清除锁定
                        SelfRole._inst.m_LockRole = null;
                    return true; //过滤队员
                }
                else
                    return false;
            }
            return false;
        }
    }
}
