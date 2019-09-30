using System;
using UnityEngine;
/*
[Flags]
public enum PLAYER_MOTION_STATE
{
    IDLE = 0x01,
    RUNNING = 0x02,
    CLEAR = 0x04,
    CROWDED = 0x08,
}*/

public enum PlayerAgentAvoidancePriority
{
    Disconnected = 0,
    Idle = 20,
    Run = 50,
}

public class RoleStateHelper
{
    private ProfessionRole m_insOperate;
    private float timer = 0f;
    //private Vector3 currentFramePosition;
    //private Vector3 lastFramePosition;
    private RoleStateHelper() {/* lastFramePosition = Vector3.zero; */}

    public RoleStateHelper(ProfessionRole insUnderOperate) : this() { m_insOperate = insUnderOperate; }

    /*#region UnusedCode
    public delegate bool DispCalcHandler(Vector3 pos1, Vector3 pos2);
    public static readonly float InstantaneousDispThreshold = 0.1f; // 瞬时位移临界值

    /// <summary>
    /// 判断玩家周围的怪物数量是否满足围堵现象发生的条件
    /// </summary>
    /// <param name="checkRadius">检测周围怪物的半径</param>
    /// <param name="maxAllowedMonsterNumber">周围最大允许存在的怪物数量</param>
    /// <returns>true: 玩家被怪物包围, false: 玩家没有被包围</returns>
    public virtual bool IsPlayerSurrendedByMonsters(float checkRadius, int maxAllowedMonsterNumber)
    {
        int num_monsterNearPlayer = 0;
        Collider[] nearPlayer = Physics.OverlapSphere(position: m_insOperate.m_curModel.transform.position, radius: checkRadius, layerMask: 1 << LayerMask.NameToLayer("bt_fight"));
        for (int i = 0; i < nearPlayer.Length; i++)
            if (nearPlayer[i].transform.parent.gameObject.layer == LayerMask.NameToLayer("monster"))
                num_monsterNearPlayer++;
        return num_monsterNearPlayer > maxAllowedMonsterNumber;
    }

    /// <summary>
    /// 刷新玩家角色运动状态
    /// </summary>
    /// <param name="state">用于保存玩家运动状态的枚举型变量</param>
    /// <param name="handler">额外使用辅助计算位移的函数,默认不使用</param>
    /// <param name="args">额外计算位移函数的位移参数</param>
    /// <returns>返回设置后的状态</returns>
    public virtual PLAYER_MOTION_STATE RefreshPMState(ref PLAYER_MOTION_STATE state, DispCalcHandler handler = null)
    {
        if (!m_insOperate.m_bUserSelf || m_insOperate.moving)
        {
            if (m_insOperate.m_bUserSelf)
            {
                currentFramePosition = m_insOperate.m_curModel.transform.position;
                state = PLAYER_MOTION_STATE.RUNNING;
            }
            else
                state = PLAYER_MOTION_STATE.IDLE;

            if (IsPlayerSurrendedByMonsters(2f, maxAllowedMonsterNumber: 5) &&
                (handler == null || handler(currentFramePosition, lastFramePosition)))
                state = state | PLAYER_MOTION_STATE.CROWDED;
            if (!IsPlayerSurrendedByMonsters(2f, maxAllowedMonsterNumber: 2))
                state = state | PLAYER_MOTION_STATE.CLEAR;
            lastFramePosition = currentFramePosition;
        }
        else
            state = PLAYER_MOTION_STATE.IDLE;
        return state;
    }

    /// <summary>
    /// 刷新玩家角色运动状态,并根据运动状态调整导航代理
    /// </summary>
    /// <param name="state">用于保存玩家运动状态的枚举型变量</param>
    /// <param name="moveAgent">用于保存玩家导航代理的变量</param>
    /// <param name="handler">额外使用辅助计算位移的函数,默认不使用</param>
    /// <param name="lastFramePosition">额外计算位移函数的位移参数</param>    
    public virtual void RefreshNavMesh(ref PLAYER_MOTION_STATE state, NavMeshAgent moveAgent, DispCalcHandler handler = null)
    {
        switch (RefreshPMState(ref state, handler))
        {
            default:
            case PLAYER_MOTION_STATE.IDLE:
                moveAgent.avoidancePriority = (int)PlayerAgentAvoidancePriority.Idle;
                break;

            case PLAYER_MOTION_STATE.RUNNING:
                moveAgent.avoidancePriority = (int)PlayerAgentAvoidancePriority.Run;
                break;

            case PLAYER_MOTION_STATE.CLEAR | PLAYER_MOTION_STATE.IDLE:
            case PLAYER_MOTION_STATE.CLEAR | PLAYER_MOTION_STATE.RUNNING:
                moveAgent.radius = 0.5f;
                moveAgent.height = 2f;
                break;

            case PLAYER_MOTION_STATE.CROWDED | PLAYER_MOTION_STATE.IDLE:
            case PLAYER_MOTION_STATE.CROWDED | PLAYER_MOTION_STATE.RUNNING:
                moveAgent.radius = 1e-5f;
                moveAgent.height = 1e-5f;
                break;
        }
    }
    #endregion*/

    public void SetNavMeshInfo(int avoidancePriority)
    {
        if (m_insOperate.m_moveAgent == null)
            return;
        m_insOperate.m_moveAgent.avoidancePriority = avoidancePriority;
    }
    public void SetNavMeshInfo(float radius, float height)
    {
        if (m_insOperate.m_moveAgent == null)
            return;
        m_insOperate.m_moveAgent.radius = radius;
        m_insOperate.m_moveAgent.height = height;
    }
    public void SetNavMeshInfo(int avoidancePriority, float radius, float height)
    {
        SetNavMeshInfo(avoidancePriority);
        SetNavMeshInfo(radius, height);
    }
    public bool CheckMoveAgent(float deltaTime,float maxAllowedStayTime=0.5f,bool resetImmediately=false)
    {
        if (m_insOperate.m_moveAgent.velocity.z != 0)
            timer += deltaTime;
        else
            timer = Mathf.Clamp(timer-deltaTime,min:0,max:timer);

        bool IsOverStay = timer > maxAllowedStayTime;
        if (resetImmediately && IsOverStay)
            try { ResetAutoFight(); }
            catch (Exception e) { Debug.Log(e.Message); }
        return !IsOverStay;
    }
    public void ResetAutoFight(MuGame.StateMachine stateMachine=null)
    {
        timer = 0f;
        if (stateMachine == null)
        {
            if (SelfRole.fsm != null)
            {
                SelfRole.fsm.Stop();
                SelfRole.fsm.StartAutofight();
            }
            else
                throw new Exception("StateMachineNotFoundException");
        }
        else
        {
            stateMachine.Stop();
            stateMachine.StartAutofight();
        }
    }
}

