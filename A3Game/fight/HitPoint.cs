using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
//攻击点的数据，如伤害，击飞等等
public class HitData : MonoBehaviour
{
    public GameObject m_hdRootObj; //HitData不一定就绑在root上，可能在它的child里，子弹类技能明显
    public BaseRole m_CastRole; //释放这次伤害的人

    public Vector3 m_vBornerPos; //释放者当时的位置
    public uint m_unSkillID = 0; //这是唯一需要发给服务器知道的（服务器判定是否有这个技能），然后各种演算，服务器下发伤害
    public int m_nDamage = 1;

    
    public int m_nLastHit = 0; //发给服务器的，表示最后一下的攻击效果,一般攻击力比较大
    public int m_nHittedCount = 3;
    public List<uint> m_unListRoleHitted; //命中了多少目标
    public List<uint> HittedisProfession;//命中的目标是玩家..

    public List<uint> m_haveHittedList = new List<uint>(); //已经击中过的做下记录

    public int m_nHitType = 0; //如普通，击飞，击退等

    //战斗的阵营，决定哪些
    public PK_TYPE m_ePK_Type = PK_TYPE.PK_PKALL;
    public uint m_unPK_Param = 0;

    public bool m_bSelfHit = false; //玩家主角的攻击

    public Animator m_aniTrack;
    public Animator m_aniFx;

    public bool m_bOnlyHit = false;

    public Color m_Color_Main = Color.gray;
    public Color m_Color_Rim = Color.red;

    public int m_nHurtFX = 0;
    public int m_nHurtSP_type = 0;
    public int m_nHurtSP_pow = 0;
    public Vector3 m_nHurtSP_pos;

    public int m_hurtNum = -1;

    public bool AddHittedRole(uint id, bool isProfession)
    {
        if (null == m_unListRoleHitted)
        {
            m_unListRoleHitted = new List<uint>();
            HittedisProfession = new List<uint>();
        }

        if (m_hurtNum == -1)
        {
            m_hurtNum = Skill_a3Model.getInstance().skilldic[(int)m_unSkillID].targetNum;            
        }

        if (m_hurtNum > 0)
        {
            if (isProfession)
            {
                HittedisProfession.Add(id);
            }
            m_unListRoleHitted.Add(id);
            m_hurtNum--;

            if (m_hurtNum == 0)
            {//优先打中选中的目标
                if (SelfRole._inst.m_LockRole != null)
                {
                    if (!m_unListRoleHitted.Contains(SelfRole._inst.m_LockRole.m_unIID))
                    {
                        float dis = Vector3.Distance(SelfRole._inst.m_curModel.position, SelfRole._inst.m_LockRole.m_curModel.position);
                        if (dis * GameConstant.PIXEL_TRANS_UNITYPOS <= Skill_a3Model.getInstance().skilldic[(int)m_unSkillID].range)
                        {
                            uint removeid = m_unListRoleHitted[0];
                            m_unListRoleHitted.Remove(removeid);
                            m_unListRoleHitted.Add(SelfRole._inst.m_LockRole.m_unIID);

                            if (isProfession)
                                HittedisProfession.Add(SelfRole._inst.m_LockRole.m_unIID);
                            if (HittedisProfession.Contains(removeid))
                                HittedisProfession.Remove(removeid);
                        }
                    }
                    else
                    {
                        //将锁定的排到最后一个。这样服务器优先处理锁定目标
                        m_unListRoleHitted.Remove(SelfRole._inst.m_LockRole.m_unIID);
                        m_unListRoleHitted.Add(SelfRole._inst.m_LockRole.m_unIID);
                    }
                }
            }
        }
        return m_hurtNum >= m_unListRoleHitted.Count + HittedisProfession.Count;
    }

    //命中后特效轨迹停顿
    public void HitAndStop(int ani = -1, bool shake = false)
    {
        if (m_aniTrack != null)
        {
            m_aniTrack.speed = 0;
        }

        if (m_aniFx != null)
        {
            if(ani == -1)
                m_aniFx.SetTrigger(EnumAni.ANI_T_FXDEAD);
            else
                m_aniFx.SetTrigger(ani);
        }
        if (shake)
        {
            SceneCamera.cameraShake(0.4f, 20, 0.5f);
        }
    }

    void Update()
    {

        if (m_unListRoleHitted != null)
        {
            print("攻击到的人的个数:" + m_unListRoleHitted.Count);
            PkmodelAdmin.RefreshList(m_unListRoleHitted, HittedisProfession);/*, HittedisProfessionByRedname*/
            if (m_unListRoleHitted != null)
            {
                int lockId = -1;
                if (SelfRole._inst.m_LockRole != null && m_unListRoleHitted.Contains(SelfRole._inst.m_LockRole.m_unIID))
                {
                    lockId = (int)SelfRole._inst.m_LockRole.m_unIID;
                }
                //if(m_unListRoleHitted.Count<=0)
                //{
                //    print("空的");
                //}
                //else
                //{
                //    for (int i = 0; i < m_unListRoleHitted.Count; i++)
                //    {
                //        print("发送的iid是多少：" + m_unListRoleHitted[i]);
                //    }
                //}

                BattleProxy.getInstance().sendcast_target_skill(m_unSkillID, m_unListRoleHitted, m_nLastHit, lockId);
                m_unListRoleHitted = null;
                HittedisProfession = null;
                //HittedisProfessionByRedname = null;
            }  
        }
    }

}
