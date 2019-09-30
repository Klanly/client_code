using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using MuGame;

public class P2Warrior_Event : Profession_Base_Event
{
    //战士的几类子弹技能
    static public GameObject WARRIOR_B1;

    public string fx_2003_name = "skill_2003";
    //自身特效
    private void onSFX(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        if (id == 2003)
        {//战士的2003技能特效特殊处理。
            if (transform.FindChild(fx_2003_name) != null)
                return;
            CancelInvoke("SFX_2003_hide");
            Invoke("SFX_2003_hide", 3.5f);
            GameObject fx_inst = GameObject.Instantiate(P2Warrior.WARRIOR_SFX1) as GameObject;
            GameObject.Destroy(fx_inst, 5f);
            fx_2003_name = fx_inst.name;
            fx_inst.transform.SetParent(transform, false);

            return;
        }
        else
        {
            SFX_2003_hide();
        }

        SceneFXMgr.Instantiate("FX_warrior_SFX_" + id.ToString(), transform.position, transform.rotation, 4f);
    }

    public void SFX_2003_hide()
    {
        if (transform.FindChild(fx_2003_name) != null)
            transform.FindChild(fx_2003_name).gameObject.SetActive(false);
    }

    public void onBullet(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        //战士普通三段攻击
        if (id == 1)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

            hd.m_nDamage = 108;
            // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2001);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2001);
            hd.m_nHurtFX = 2;
        }

        if (id == 2)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
            hd.m_nDamage = 128;
            // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2001);
           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2001);
            hd.m_nHurtFX = 2;
        }

        if (id == 3)
        {
            Vector3 pos = transform.position + transform.forward * 2.5f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
            hd.m_nDamage = 188;
            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2001);
           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2001);
            //hd.m_nHurtSP_type = 1;
            //hd.m_nHurtSP_pow = 3;

            hd.m_nHurtFX = 2;

            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 3;

            hd.m_nLastHit = 1;
        }

        //战士的旋风斩(飓风)
        if (id == 4)
        {
            Vector3 pos = transform.position;// +transform.forward * 1.5f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2003, 0.125f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);
            hd.m_nDamage = 88;
            //hd.m_Color_Main = Color.red;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2003);
           // hd.m_Color_Rim = Color.red;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2003);
            hd.m_nHurtFX = 3;
        }

        //战士的纵砍(力劈)
        if (id == 21)
        {
            Vector3 pos = transform.position + transform.forward * 4f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2004, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
            hd.m_nDamage = 388;
            // hd.m_Color_Main = Color.red;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2004);
          //  hd.m_Color_Rim = Color.red;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2004);
            hd.m_nHurtSP_type = 1;
            hd.m_nHurtSP_pow = 4;
        }


        //加入战士的丢斧头的技能(这是跟踪弹类的技能)
        if (13 == id)
        {
            if (m_linkProfessionRole.m_LockRole != null)
            {
                Vector3 pos = transform.position + transform.forward * 0.5f;
                pos.y += 1.25f;

                HitData hd = m_linkProfessionRole.Build_PRBullet(2004, 0f, WARRIOR_B1, pos, transform.rotation);

                FollowBullet_Mgr.AddBullet(m_linkProfessionRole.m_LockRole, hd, 0.8f);
            }
        }

        //战士的回锋技能，会将怪拉来
        if (141 == id)
        {
            Vector3 pos = transform.position + transform.forward * 3.0f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2002, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);

            hd.m_nDamage = 108;
            // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2002);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2002);
            hd.m_nHurtFX = 2;
        }

        if (142 == id)
        {
            Vector3 pos = transform.position + transform.forward * 6.0f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2002, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);

            hd.m_nDamage = 88;
            //  hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2002);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2002);
            hd.m_nHurtFX = 2;
        }

        if (143 == id)
        {
            Vector3 pos = transform.position + transform.forward * 4.0f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2002, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);

            hd.m_nDamage = 188;
            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2002);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2002);
            hd.m_nHurtFX = 2;

            //有拉到Cast面前的效果
            hd.m_nHurtSP_type = 21;
            hd.m_nHurtSP_pow = 4;
        }
        if (2009 == id)
        {
            Vector3 pos = transform.position + transform.forward * 3f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2009, 0.2f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);
            hd.m_nDamage = 88;
            //hd.m_Color_Main = Color.red;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2009);
           // hd.m_Color_Rim = Color.red;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2009);
            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 4;

            hd.m_nHurtFX = 6;
        }
        if(2006 == id)
        {
            Vector3 pos = transform.position/* + transform.forward * 3f*/;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2006, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
            hd.m_nDamage = 88;
           // hd.m_Color_Main = Color.red;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2006);
            //hd.m_Color_Rim = Color.red;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2006);
            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 4;

            hd.m_nHurtFX = 6;
        }
        if (2007 == id)
        {
            Vector3 pos = transform.position + transform.forward * 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(2007, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
            hd.m_nDamage = 88;
            // hd.m_Color_Main = Color.red;
            hd.m_Color_Main = SkillModel.getskill_mcolor(2007);
            //hd.m_Color_Rim = Color.red;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(2007);
            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 10;

            hd.m_nHurtFX = 3;
        }
    }
    public void onJump(int id)
    {
        if (2009 == id)
        {
            //判断是否跳到阻挡外
            NavMeshPath path = new NavMeshPath();
            NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
            Vector3 pos = transform.position;
            if (m_linkProfessionRole.m_LockRole != null)
            {
                float true_dis = Vector3.Distance(pos, m_linkProfessionRole.m_LockRole.m_curModel.position);

                int j = 1;
                for (; j < true_dis; j++)
                {
                    Vector3 endPos = m_linkProfessionRole.m_LockRole.m_curModel.position - transform.forward * j;
                    endPos = transform.position + transform.forward * j;
                    if (nav.CalculatePath(endPos, path))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (j < true_dis)
                {//半途有阻碍
                    Vector3 endPos = pos + transform.forward * j;
                    float dis = Vector3.Distance(pos, endPos);
                    transform.DOJump(endPos, 0.2f * dis, 1, 0.07f * dis);
                }
                else
                {//半途无阻碍
                    int i = 3;
                    for (; i <= 3 && i >= 0; i--)
                    {
                        Vector3 endPos = m_linkProfessionRole.m_LockRole.m_curModel.position - transform.forward * i;
                        if (nav.CalculatePath(endPos, path))
                        {
                            float dis = Vector3.Distance(pos, endPos);
                            transform.DOJump(endPos, 0.2f * dis + endPos.y, 1, 0.07f * dis);
                            break;
                        }
                    }
                }
            }
            else
            {
                //原地方向起跳
                int i = 1;
                Vector3 endPos = transform.position + transform.forward * i;
                for (; i < 10; i++)
                {
                    endPos = transform.position + transform.forward * i;
                    if (nav.CalculatePath(endPos, path))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if(i > 3)
                    transform.DOJump(endPos, 0.2f*i + endPos.y, 1, 0.07f * i);    
            }

            path = null;
        }
        if(id == 2006)
        {
            //判断是否跳到阻挡外
            NavMeshPath path = new NavMeshPath();
            NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
            Vector3 pos = transform.position;
           
            if (m_linkProfessionRole.m_LockRole != null)
            {
                float true_dis = Vector3.Distance(pos, m_linkProfessionRole.m_LockRole.m_curModel.position);

                int j = 1;
                for (; j < true_dis; j++)
                {
                    Vector3 endPos = m_linkProfessionRole.m_LockRole.m_curModel.position - transform.forward * j;
                    endPos = transform.position + transform.forward * j;
                    if (nav.CalculatePath(endPos, path))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (j < true_dis)
                {//半途有阻碍
                    Vector3 endPos = pos + transform.forward * j;
                    float dis = Vector3.Distance(pos, endPos);
                   // transform.DOJump(endPos, pos.y, 1, 0.07f * dis);
                }
                else
                {//半途无阻碍
                    int i = 3;
                    for (; i <= 3 && i >= 0; i--)
                    {
                        Vector3 endPos = m_linkProfessionRole.m_LockRole.m_curModel.position - transform.forward * i;
                        if (nav.CalculatePath(endPos, path))
                        {
                            float dis = Vector3.Distance(pos, endPos);
                           // transform.DOJump(endPos, endPos.y, 1, 0.07f * dis);
                            break;
                        }
                    }
                }
            }
            else
            {
                //原地方向起跳
                int i = 1;
                Vector3 endPos = transform.position + transform.forward * i;
                for (; i < 10; i++)
                {
                    endPos = transform.position + transform.forward * i;
                    if (nav.CalculatePath(endPos, path))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
               // if (i > 3)
                  //  transform.DOJump(endPos, endPos.y, 1, 0.07f * i);
            }

            path = null;
        }
    }
}
