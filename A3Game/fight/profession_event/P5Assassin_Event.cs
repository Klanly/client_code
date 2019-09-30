using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using MuGame;

//刺客的技能
public class P5Assassin_Event : Profession_Base_Event
{
    static public GameObject ASSASSIN_S1, ASSASSIN_S2;

    public string fx_5003_name = "skill_5003";
    //自身特效
    private void onSFX(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        if (id == 5003)
        {//刺客5003技能特效特殊处理。
            if (transform.FindChild(fx_5003_name) != null)
                return;

            CancelInvoke("SFX_5003_hide");
            Invoke("SFX_5003_hide", 2.5f);

            GameObject fx_inst = GameObject.Instantiate(P5Assassin.ASSASSIN_SFX1) as GameObject;
            GameObject.Destroy(fx_inst, 4f);
            fx_5003_name = fx_inst.name;
            fx_inst.transform.SetParent(transform, false);

            return;
        }
        else
        {
            SFX_5003_hide();
        }

        SceneFXMgr.Instantiate("FX_assa_SFX_" + id.ToString(), transform.position, transform.rotation, 4f);
    }

    public void SFX_5003_hide()
    {
        if (transform.FindChild(fx_5003_name) != null)
            transform.FindChild(fx_5003_name).gameObject.SetActive(false);
    }

    //加入碰撞点
    public void onBullet(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        //刺客的三段攻击
        if (id == 1)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);
            hd.m_nHurtFX = 2;
        }

        if (id == 2)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);
            hd.m_nHurtSP_type = 1;
            hd.m_nHurtSP_pow = 2;

            hd.m_nHurtFX = 2;
        }

        if (id == 3)
        {
            Vector3 pos = transform.position + transform.forward * 2.5f;
            pos.y += 1f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);

            hd.m_nHurtFX = 2;

            hd.m_nLastHit = 1;
        }

        //刺客的疾风连刺
        if (id == 50021)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(5002, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

            hd.m_nDamage = 58;
            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5002);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5002);
            hd.m_nHurtFX = 4;
        }

        //刺客的疾风连刺
        if (id == 50022)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(5002, 0.125f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);

            hd.m_nDamage = 138;
            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5002);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5002);
            hd.m_nHurtFX = 5;

            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 3;

            hd.m_nLastHit = 1;
        }

        //刺客的疾风乱舞
        if (id == 5)
        {
            Vector3 pos = transform.position;
            pos.y += 1f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(5003, 0.125f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);

            hd.m_nDamage = 57;
            //hd.m_Color_Main = Color.blue;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5003);
           // hd.m_Color_Rim = Color.cyan;
            hd.m_Color_Rim= SkillModel.getskill_rcolor(5003);


            hd.m_nHurtFX = 5;
        }

        //(影袭)
        if (id == 50041)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5004, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5004);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5004);
            hd.m_nHurtFX = 4;
        }

        //(影袭)
        if (id == 50042)
        {
            Vector3 pos = transform.position + transform.forward * 1.5f;
            pos.y += 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5004, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5004);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5004);
            hd.m_nLastHit = 1;
            hd.m_nHurtFX = 5;
        }

        //特殊处理瞬移到敌人身后(影袭)
        if (131 == id)
        {
            if (m_linkProfessionRole.m_LockRole != null)
            {
                Vector3 dis_pos = m_linkProfessionRole.m_curModel.position - m_linkProfessionRole.m_LockRole.m_curModel.position;
                if (dis_pos.magnitude < 4f)
                {
                    m_linkProfessionRole.m_curModel.position = m_linkProfessionRole.m_LockRole.m_curModel.position - m_linkProfessionRole.m_LockRole.m_curModel.forward * 4f;
                    m_linkProfessionRole.m_curModel.forward = m_linkProfessionRole.m_LockRole.m_curModel.forward;
                }
            }

            //m_monRole.m_curModel.position = hd.m_CastRole.m_curModel.position + hd.m_CastRole.m_curModel.forward * 2f;
        }

        //幻影杀阵
        if(50071 == id)
        {
            Vector3 pos = transform.position ;
            //pos.y += 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5007, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5007);
           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5007);
            hd.m_nHurtFX = 5;
        }
        if (50072 == id)
        {
            Vector3 pos = transform.position ;
            //pos.y += 1f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5007, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);

            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5007);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5007);
            hd.m_nHurtFX = 1;
        }
        if (5006 == id)
        {
            Vector3 pos = transform.position;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5006, 1.0f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5006);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5006);
            hd.m_nHurtFX = 2;
        }
        if (5009 == id)
        {
            if (m_linkProfessionRole.m_LockRole != null)
            {//闪现过去后朝向目标
                m_linkProfessionRole.TurnToRole(m_linkProfessionRole.m_LockRole,true);
            }

            Vector3 pos = transform.position;
            HitData hd = m_linkProfessionRole.Build_PRBullet(5009, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);

           // hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(5009);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(5009);
            hd.m_nHurtFX = 2;
        }
    }


    public void onJump(int id)
    {
        if (5009 == id)
        {//死亡印记
            if (m_linkProfessionRole.m_LockRole != null)
            {
                NavMeshPath path = new NavMeshPath();
                NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
                Vector3 pos = transform.position;

                int i = 2;
                for (; i <= 2 && i >= 0; i--)
                {
                    Vector3 endPos = m_linkProfessionRole.m_LockRole.m_curModel.position + transform.forward * i;
                    if (nav.CalculatePath(endPos, path))
                    {
                        float dis = Vector3.Distance(pos, endPos);
                        transform.DOJump(endPos, 0.2f * dis + endPos.y, 1, 0.01f);
                        break;
                    }
                }
                GameObject inst = GameObject.Instantiate(ASSASSIN_S1) as GameObject;
                inst.transform.SetParent(m_linkProfessionRole.m_LockRole.m_curModel, false);
                
                GameObject.Destroy(inst, 3f);
            }
        }
        m_linkProfessionRole.ShowAll();
    }

    public void onHide(int id)
    {
        if (5009 == id)
        {
            if (m_linkProfessionRole.m_LockRole != null)
            {
                Vector3 pos = Vector3.zero;
                pos.y = pos.y + m_linkProfessionRole.m_LockRole.headOffset.y;

                GameObject inst = GameObject.Instantiate(ASSASSIN_S2, pos, m_linkProfessionRole.m_LockRole.m_curModel.rotation) as GameObject;
                inst.transform.SetParent(m_linkProfessionRole.m_LockRole.m_curModel, false);
                GameObject.Destroy(inst, 3f);
            }
        }

        base.onHide(id);
    }
}
