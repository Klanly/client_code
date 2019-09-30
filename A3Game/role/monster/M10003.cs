using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//双头龙
public class M10003 : MonsterRole
{
    public Animator m_LH_Anim_Skill1; //左龙头喷火焰
    public Animator m_RH_Anim_Skill1; //右龙头喷火焰

    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_bFlyMonster = true;
        m_fNavSpeed = 3f; //飞行怪物需要这样的来处理寻路
        m_fNavStoppingDis = 4f;
        m_fAttackAngle = 35f; //120度内都可以攻击

        m_nSPWeight = 64;

        m_nNavPriority = 49;
        if (m_moveAgent != null)
        {
            m_moveAgent.avoidancePriority = 49; //要挤开玩家
            m_moveAgent.speed = m_fNavSpeed;
            m_moveAgent.radius = 4f;
        }

        base.Init(prefab_path, layer, pos, roatate);
        maxHp = curhp = 2000;//4000;
    }

    override protected void Model_Loaded_Over()
    {
        M10003_Stl_Event mhr = m_curModel.gameObject.AddComponent<M10003_Stl_Event>();
        mhr.m_monRole = this;
        mhr.m_StlRole = this;

        //双头的火焰攻击
        Transform lhead = m_curModel.FindChild("Dummy001");
        Transform rhead = m_curModel.FindChild("Bip001 HeadNub");

        if (lhead != null)
        {
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("bullet_10003_sbt1_s2");
            GameObject bult = GameObject.Instantiate(obj_prefab) as GameObject;
            bult.transform.SetParent(lhead, false);
            m_LH_Anim_Skill1 = bult.GetComponent<Animator>();

            Transform bt_l = bult.transform.FindChild("bt");
            if (bt_l != null)
            {
                HitData hd = bt_l.gameObject.AddComponent<HitData>();
                hd.m_CastRole = this;
                hd.m_ePK_Type = PK_TYPE.PK_LEGION;
                hd.m_unPK_Param = m_unLegionID;
                hd.m_vBornerPos = m_curModel.position;
                hd.m_nDamage = 888;
                hd.m_nHitType = 0;
                hd.m_bOnlyHit = false;

                bt_l.gameObject.layer = EnumLayer.LM_BT_FIGHT;
            }
        }

        if (rhead != null && lhead != null)
        {
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("bullet_10003_sbt1_s1");
            GameObject bult = GameObject.Instantiate(obj_prefab) as GameObject;
            bult.transform.SetParent(rhead, false);
            m_RH_Anim_Skill1 = bult.GetComponent<Animator>();

            Transform bt_r = bult.transform.FindChild("bt");
            if (bt_r != null)
            {
                HitData hd = bt_r.gameObject.AddComponent<HitData>();
                hd.m_CastRole = this;
                hd.m_ePK_Type = PK_TYPE.PK_LEGION;
                hd.m_unPK_Param = m_unLegionID;
                hd.m_vBornerPos = m_curModel.position;
                hd.m_nDamage = 888;
                hd.m_nHitType = 0;
                hd.m_bOnlyHit = false;

                bt_r.gameObject.layer = EnumLayer.LM_BT_FIGHT;
            }
        }

        if (m_LH_Anim_Skill1 == null) m_LH_Anim_Skill1 = U3DAPI.DEF_ANIMATOR;
        if (m_RH_Anim_Skill1 == null) m_RH_Anim_Skill1 = U3DAPI.DEF_ANIMATOR;

        //出场来一下，喷火
        PlayFire();
    }

    //龙有2种攻击模式，所以这里要做切换
    override protected void EnterAttackState() //进入攻击状态
    {
        if (UnityEngine.Random.Range(0, 10) < 3)
        {
            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 1);
            m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
        }
        else
        {
            m_curAni.SetBool(EnumAni.ANI_ATTACK, true);
            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
        }
    }

    override protected void LeaveAttackState() //离开攻击状态
    {
        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
    }

    public void PlayFire()
    {
        m_LH_Anim_Skill1.SetTrigger(EnumAni.ANI_T_SBT_ATK);
        m_RH_Anim_Skill1.SetTrigger(EnumAni.ANI_T_SBT_ATK);
    }

    public override void onDeadEnd()
    {
        base.onDeadEnd();

        debug.Log("成功过关");
      //  PlayerModel.getInstance().LeaveStandalone_CreateChar();
    }

}
