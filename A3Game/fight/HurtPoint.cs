using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame;
using UnityEngine;


public class BaseHurt : MonoBehaviour
{
    protected bool CanHited(BaseRole br, HitData hd)
    {
        bool hited=false;

        bool ismysellf = false;//攻击的人是我
        if (hd.m_CastRole is ProfessionRole && hd.m_CastRole.m_isMain)
        {
            hd.m_ePK_Type = PlayerModel.getInstance().pk_state;
            ismysellf = true;
        }
        else
            ismysellf = false;
        switch (hd.m_ePK_Type)
        {
            
            case PK_TYPE.PK_PEACE:
                {
                    if (br is MonsterRole)
                        hited = true;
                    else if(br is ProfessionRole&&ismysellf&& SelfRole._inst.m_LockRole!=null&&!br.m_isMain)
                    {
                        hited = SelfRole._inst.m_LockRole.havefanjibuff == true ? true : false;
                    }
                }
                break;
            case PK_TYPE.PK_TEAM:
                {
                    //if (br.m_unTeamID != hd.m_unPK_Param) hited = true;
                    if (br is MonsterRole)
                        hited = true;
                    else if (br is ProfessionRole)
                    {
                        if (ismysellf)
                        {


                            //if (br.m_unCID != 0 && br.m_unCID != PlayerModel.getInstance().m_unPK_Param && br.m_unCID != PlayerModel.getInstance().m_unPK_Param2)
                            //    hited = true;
                        }
                        else
                        {
                            if (br.m_unCID != hd.m_unPK_Param) hited = true;
                        }
                    }


                }
                break;
            case PK_TYPE.PK_LEGION:
                {
                    //if (br.m_unLegionID != hd.m_unPK_Param) hited = true;
                    if (br is MonsterRole)
                        hited = true;
                    else if (br is ProfessionRole)
                    {
                        if (ismysellf)
                        {

                            if (br.m_unCID != 0 && br.m_unCID != PlayerModel.getInstance().m_unPK_Param && br.m_unCID != PlayerModel.getInstance().m_unPK_Param2)
                                hited = true;
                        }
                        else
                        {
                            if (br.m_unCID != hd.m_unPK_Param) hited = true;
                        }
                    }
                }
                break;
            case PK_TYPE.PK_HERO:
                {
                    if (br is MonsterRole) hited = true;
                    else if (br is ProfessionRole)
                    {
                        //debug.Log("红名类型是：：：：：：：：" + br.rednm);
                        if (br.rednm > 0)
                            hited = true;
                        else
                            hited = false;
                    }
                    //?????????????
                }
                break;
            case PK_TYPE.PK_PKALL:
                {
                    if (br is MonsterRole) hited = true;
                    else if (br is ProfessionRole)
                    {
                        if (ismysellf)
                        {
                           
                            if (br.m_unCID != 0)
                                hited = true;
                        }
                        else
                        {
                            if (br.m_unCID != hd.m_unPK_Param) hited = true;
                        }
                    }
                }
                break;
            case PK_TYPE.Pk_SPOET:
                if (br is MonsterRole)
                    hited = true;
                else if (br is ProfessionRole)
                {
                    if (ismysellf)
                    {
                        if (br.m_unCID != 0 && br.lvlsideid != PlayerModel.getInstance().lvlsideid)
                            hited = true;
                    }
                    else
                    {
                        if (br.m_unCID != hd.m_unPK_Param) hited = true;
                    }
                }
                break;
        }

        return hited;
    }

}


//主角的受击
public class SelfHurtPoint : BaseHurt
{
    public ProfessionRole m_selfRole;
    public void OnTriggerEnter(Collider other)
    {
        HitData hd = other.gameObject.GetComponent<HitData>();
        if (hd == null) return;
        if (hd.m_haveHittedList.Contains(m_selfRole.m_unIID))
            return;
        else
            hd.m_haveHittedList.Add(m_selfRole.m_unIID);

        if (PlayerModel.getInstance().up_lvl >= 1)
        {
            if (CanHited(m_selfRole, hd))
            {
                //临时处理只碰一次
                if (hd.m_bOnlyHit) other.enabled = false;

                if (hd.m_unSkillID == 3003)
                {//冰雨技能攻击到时播放dead2的特效
                    hd.HitAndStop(EnumAni.ANI_T_FXDEAD1);
                }
                else if (hd.m_unSkillID == 3006)
                {//陨石可穿过模型

                }
                else
                {
                    //停止动画
                    hd.HitAndStop();
                }

                if (hd.m_CastRole != null && hd.m_CastRole.isfake)
                    m_selfRole.onHurt(hd);

                //主角自己受到的伤害让服务器下发好了，自己不用去关心
                //m_selfRole.onHurt(hd);
                m_selfRole.ShowHurtFX(hd.m_nHurtFX);


                //if (hd.m_bNeedKill)
                //{
                //    GameObject.Destroy(other.gameObject);
                //}
                //播放死亡
                //GameObject.Destroy(other.gameObject);

                //显示迎战的ui
                if (!hd.m_CastRole.m_isMain&&hd.m_CastRole is ProfessionRole && PlayerModel.getInstance().now_pkState == 0)//PlayerModel.getInstance().now_pkState == 0 && hd.m_CastRole is ProfessionRole && PlayerModel.getInstance().now_nameState==0
                {
                    if (!hd.m_CastRole.isDead || hd.m_CastRole != null || !m_selfRole.isDead)
                    {
                        a3_expbar.instance.ShowAgainst(hd.m_CastRole);
                    }

                }

                //我非全体模式时，打我的人是我的团员或者对友，飘字提示
                if(PlayerModel.getInstance().now_pkState != 1&& !hd.m_CastRole.m_isMain && hd.m_CastRole is ProfessionRole)
                {

                    if ((TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam(hd.m_CastRole.roleName))||
                        (PlayerModel.getInstance().clanid != 0 && PlayerModel.getInstance().clanid == hd.m_CastRole.m_unLegionID))
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_pkmodel"));
                    }

                }

            }
        }







    }
}

//其他玩家的受伤
public class OtherHurtPoint : BaseHurt
{
    public ProfessionRole m_otherRole;
    public void OnTriggerEnter(Collider other)
    {

        debug.Log("和其他玩家 发生 碰撞 " + other.name);
        //if (!m_otherRole.canbehurt)
        //    return;

        HitData hd = other.gameObject.GetComponent<HitData>();
        if (hd == null) return;
        if (hd.m_haveHittedList.Contains(m_otherRole.m_unIID))
            return;
        else
            hd.m_haveHittedList.Add(m_otherRole.m_unIID);

        if (OtherPlayerMgr._inst.m_mapOtherPlayer[m_otherRole.m_unIID].zhuan >= 1)
        {
            if (CanHited(m_otherRole, hd))
            {
                //临时处理只碰一次
                other.enabled = false;

                if (hd.m_unSkillID == 3003)
                {//冰雨技能攻击到时播放dead2的特效
                    hd.HitAndStop(EnumAni.ANI_T_FXDEAD1);
                }
                else if (hd.m_unSkillID == 3006)
                {//陨石可穿过模型

                }
                else
                {
                    //停止动画
                    hd.HitAndStop();
                }

                //if (hd.m_bNeedKill)
                //{
                //    GameObject.Destroy(other.gameObject);
                //}

                //播放死亡
                //GameObject.Destroy(other.gameObject);
                if (hd.m_CastRole == SelfRole._inst)
                {
                    hd.AddHittedRole(m_otherRole.m_unIID, true);

                }
                else
                {
                    m_otherRole.onHurt(hd);
                }

                m_otherRole.ShowHurtFX(hd.m_nHurtFX);

            }
        }

    }
}

public class MonHurtPoint : BaseHurt
{
    public MonsterRole m_monRole;
    private static uint lastUsedSkillId = 1; // 最后释放的技能
    private static int lastUsedSkillTargetNum = 0; // 最后释放技能的最大命中数目
    private static float lastUseSkillTime; // 计时器:用于检测是否可以重置命中数目
    private static float SkillCheckTimeSpan = 0.05f;
    //玩家当前正在攻击的段数
    //玩家当前正在使用技能的一些信息
    //     每段的攻击间隔
    //     每段
    private static skill_a3Data skillData;
    public void OnTriggerEnter(Collider other)
    {
        if (m_monRole.m_layer == EnumLayer.LM_COLLECT)
            return;



        HitData hd = other.gameObject.GetComponent<HitData>();
        if (hd == null) return;

        if (hd.m_CastRole is MonsterRole)
            return;

        if (hd.m_CastRole == SelfRole._inst && m_monRole is MS0000 && ((MS0000)m_monRole).owner_cid == PlayerModel.getInstance().cid)
            return;

        //   debug.Log("怪物 有碰撞 " + other.name);
        if (!m_monRole.canbehurt)
            return;
        // -- Modified Here --
        if (hd.m_haveHittedList.Contains(m_monRole.m_unIID))
            return;
        else
            hd.m_haveHittedList.Add(m_monRole.m_unIID);

        //bool hited = false;

        //只要不是怪物自己，都可以打到怪
        //if (FIGHT_A3_SIDE.FA3S_MONSTER != hd.m_eFight_Side)
        //{
        //    hited = true;
        //}

        if (CanHited(m_monRole, hd))
                  MonsterHurtEffectWork(other, hd);
    }
    private void MonsterHurtEffectWork(Collider other,HitData hd)
    {
        if (!m_monRole.isfake)
            hd.m_nDamage = 0;
        if (hd.m_CastRole == SelfRole._inst && !m_monRole.isfake)
        {
            if (hd.AddHittedRole(m_monRole.m_unIID, false))
            {
                m_monRole.setHitFlash(hd);
                //播放受击特效
                if (hd.m_nHurtFX > 0 && hd.m_nHurtFX < 10)
                {
                    if (hd.m_nHurtFX == 6)
                    {//受击为击退和眩晕时特效特殊处理
                        Vector3 pos = Vector3.zero;
                        pos.y = pos.y + m_monRole.headOffset.y;
                        GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[hd.m_nHurtFX], pos, m_monRole.m_curModel.rotation) as GameObject;
                        fx_inst.transform.SetParent(m_monRole.m_curModel, false);
                        GameObject.Destroy(fx_inst, 2f);
                    }
                    else
                    {
                        GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[hd.m_nHurtFX], m_monRole.m_curModel.position, m_monRole.m_curModel.rotation) as GameObject;
                        fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                        GameObject.Destroy(fx_inst, 2f);
                    }
                }
                m_monRole.PlayHurtFront();
            }
        }
        else
            m_monRole.onHurt(hd);
        #region 其它处理
        if (hd.m_bOnlyHit) //临时处理只碰一次
            other.enabled = false;
        if (hd.m_unSkillID == 3003) //冰雨技能攻击到怪物时播放dead2的特效
            hd.HitAndStop(EnumAni.ANI_T_FXDEAD1);
        #region 陨石处理
        else if (hd.m_unSkillID == 3006)
        {//陨石可穿过模型

        }
        #endregion
        else //停止动画
            hd.HitAndStop();
        #region 
        //if (hd.m_nHurtSP_type == 11)
        //{
        //    m_monRole.m_curModel.position = hd.m_CastRole.m_curModel.position + hd.m_CastRole.m_curModel.forward * 2f;
        //}
        //else
        //{
        //    //m_monRole.m_fSkillSPTime = 0.25f;
        //}
        #endregion  
        if (hd.m_nHurtSP_type > 0)
        {
            if (hd.m_nHurtSP_pow > m_monRole.m_nSPWeight * 3)
            {
                m_monRole.m_nSPLevel = 3;
            }
            else if (hd.m_nHurtSP_pow > m_monRole.m_nSPWeight * 2)
            {
                m_monRole.m_nSPLevel = 2;
            }
            else if (hd.m_nHurtSP_pow > m_monRole.m_nSPWeight)
            {
                m_monRole.m_nSPLevel = 1;
            }
            else
            {
                m_monRole.m_nSPLevel = 0;
            }

            if (m_monRole.m_nSPLevel > 0)
            {
                if (!m_monRole.isBoss_c)
                {
                    if (1 == hd.m_nHurtSP_type ||
                    13 == hd.m_nHurtSP_type)
                    {
                        m_monRole.m_fSkillSPup_Value = 0.25f; //时间
                                                              //m_monRole.m_fSkillSPup_Keep = 0.125f;
                        m_monRole.m_nSkillSP_up = 1;
                    }
                    if (11 == hd.m_nHurtSP_type || 13 == hd.m_nHurtSP_type) //击退
                    {
                        m_monRole.m_fSkillSPfb_Value = 0.1f; //时间
                        m_monRole.m_nSkillSP_fb = 1;
                        m_monRole.m_vSkillSP_dir = hd.m_CastRole.m_curModel.forward;
                    }
                    if (12 == hd.m_nHurtSP_type || 14 == hd.m_nHurtSP_type) //拉近
                    {
                        m_monRole.m_fSkillSPfb_Value = 2.5f; //时间
                        m_monRole.m_nSkillSP_fb = -41;
                        m_monRole.m_vSkillSP_dir = hd.m_nHurtSP_pos;
                        //m_monRole.m_vSkillSP_dir = hd.m_CastRole.m_curModel.position;
                    }
                    if (21 == hd.m_nHurtSP_type) //拉动
                    {
                        m_monRole.m_fSkillSPfb_Value = 0.5f; //时间
                        m_monRole.m_nSkillSP_fb = -21;
                        m_monRole.m_vSkillSP_dir = hd.m_CastRole.m_curModel.position;
                    }
                    if (31 == hd.m_nHurtSP_type) //冰冻
                    {
                        m_monRole.setHitFlash(hd);
                        m_monRole.m_fSkillSPfb_Value = 2.5f; //时间
                        m_monRole.m_nSkillSP_fb = -31;
                        m_monRole.m_vSkillSP_dir = hd.m_CastRole.m_curModel.position;
                    }
                }
            }
        }
        #region 击倒处理
        //if (hurtFakeRole)
        //{
        //    Vector3 from = transform.forward;
        //    Vector3 to = hd.m_vBornerPos - transform.position;
        //    float fangle = Vector3.Angle(from, to);

        //    if (fangle > 90f)
        //    {
        //        if (hd.m_nHitType > 1000) //1000以上表示击倒
        //        {
        //            m_monRole.PlayFallBack();
        //        }
        //        else
        //        {
        //            m_monRole.PlayHurtBack();
        //        }
        //    }
        //    else
        //    {
        //        if (hd.m_nHitType > 1000) //1000以上表示击倒
        //        {
        //            m_monRole.PlayFallFront();
        //        }
        //        else
        //        {
        //            m_monRole.PlayHurtFront();
        //        }
        //    }

        //    m_monRole.onHurt(hd);



        //}
        #endregion
        #endregion
    }
}
