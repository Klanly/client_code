using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using MuGame;

public class P3Mage_Event : Profession_Base_Event
{
    static public GameObject MAGE_B1;
    static public GameObject MAGE_B2;
    static public GameObject MAGE_B3;
    static public GameObject MAGE_B4_1;
    static public GameObject MAGE_B4_2;
    static public GameObject MAGE_B6;
    static public GameObject MAGE_S3002;
    static public GameObject MAGE_S3002_2;

    static public GameObject MAGE_S3011;
    static public GameObject MAGE_S3011_1;

    //自身特效
    private void onSFX(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        SceneFXMgr.Instantiate("FX_mage_SFX_" + id.ToString(), transform.position, transform.rotation, 6f);
    }

    //加入碰撞点
    public void onBullet(int id)
    {
        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

       
        //法师的陨石术
        if (3006 == id)
        {
            if (m_linkProfessionRole.m_LockRole == null)
                return;

            Vector3 pos = m_linkProfessionRole.m_LockRole.m_curModel.position;
            pos.z += UnityEngine.Random.Range(-2f, 2f);
            pos.x += UnityEngine.Random.Range(-2f, 2f);
            pos.y = pos.y + 8f;

            GameObject bult = GameObject.Instantiate(P3Mage_Event.MAGE_B6, pos, transform.rotation) as GameObject;
            bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

            Transform real_track = bult.transform.FindChild("t");
            if (real_track != null)
            {
                HitData hd = m_linkProfessionRole.Link_PRBullet(3006, 3.0f, bult, real_track);

                hd.m_nHurtSP_type = 11;
                hd.m_nHurtSP_pow = 1;

                //陨石要穿过角色直到地面爆开
                hd.m_bOnlyHit = false;

                // hd.m_Color_Main = Color.red;
                hd.m_Color_Main = SkillModel.getskill_mcolor(3006);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(3006);
                hd.m_aniTrack = real_track.GetComponent<Animator>();
                real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

                Transform real_fx = real_track.FindChild("f");
                if (real_fx != null)
                {
                    hd.m_aniFx = real_fx.GetComponent<Animator>();
                }
            }
        }

        //三火球术
        if (id == 2)
        {
            Vector3 pos = transform.position;
            pos.y += 1f + UnityEngine.Random.Range(0f, 1f);
            if (UnityEngine.Random.Range(0, 16) > 7)
            {
                pos += transform.right * (0.5f + UnityEngine.Random.Range(0f, 1f));
            }
            else
            {
                pos -= transform.right * (0.5f + UnityEngine.Random.Range(0f, 1f));
            }

            GameObject bult = GameObject.Instantiate(MAGE_B2, pos, transform.rotation) as GameObject;
            bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

            Transform real_track = bult.transform.FindChild("t");
            if (real_track != null)
            {
                HitData hd = m_linkProfessionRole.Link_PRBullet(3005, 2f, bult, real_track);
                hd.m_Color_Main = SkillModel.getskill_mcolor(3005);
                hd.m_Color_Rim = SkillModel.getskill_rcolor(3005);
                hd.m_nHurtSP_type = 11;
                hd.m_nHurtSP_pow = 1;

                hd.m_aniTrack = real_track.GetComponent<Animator>();
                real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

                Transform real_fx = real_track.FindChild("f");
                if (real_fx != null)
                {
                    hd.m_aniFx = real_fx.GetComponent<Animator>();
                }
            }
        }

        //法师雷电术
        if (3002 == id)
        {
            cur_3002_num = 0;
            cur_3002_forward = transform.forward;
            cur_3002_pos = transform.position;

            CancelInvoke("skill_3002");
            InvokeRepeating("skill_3002", 0, 0.2f);

            //if (m_linkProfessionRole.m_LockRole != null)
            //{
            //    if (m_linkProfessionRole.m_LockRole.isDead)
            //    {
            //        return;
            //    }

            //    float dis = Vector3.Distance(transform.position, m_linkProfessionRole.m_LockRole.m_curModel.position);
            //    float angle = Vector3.Angle(transform.position, m_linkProfessionRole.m_LockRole.m_curModel.position);

            //    Vector3 forward = m_linkProfessionRole.m_LockRole.m_curModel.position - transform.position;
            //    Quaternion qua = Quaternion.LookRotation(forward);

            //    float rate = dis / 3.2f;

            //    GameObject s3002;
            //    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            //        s3002 = MAGE_S3002;
            //    else
            //        s3002 = MAGE_S3002_2;
            //    HitData hd = m_linkProfessionRole.Build_PRBullet(3002, 2.0f, s3002, pos, qua);
            //    hd.m_hdRootObj.transform.localScale = new Vector3(1, 1, rate);
            //    hd.m_nDamage = 1088;
            //    hd.m_Color_Main = Color.gray;
            //    hd.m_Color_Rim = Color.white;

            //    hd.m_nHurtFX = 1;

            //    //雷电指定攻击目标必中
            //    if (m_linkProfessionRole.m_LockRole is MonsterRole)
            //        m_linkProfessionRole.m_LockRole.m_curPhy.GetComponent<MonHurtPoint>().OnTriggerEnter(hd.m_hdRootObj.GetComponent<BoxCollider>());
            //    else if (m_linkProfessionRole.m_LockRole.m_isMain)
            //        m_linkProfessionRole.m_LockRole.m_curPhy.GetComponent<SelfHurtPoint>().OnTriggerEnter(hd.m_hdRootObj.GetComponent<BoxCollider>());
            //    else
            //        m_linkProfessionRole.m_LockRole.m_curPhy.GetComponent<OtherHurtPoint>().OnTriggerEnter(hd.m_hdRootObj.GetComponent<BoxCollider>());
            //}

        }

        if (3011 == id)
        {
            if (m_linkProfessionRole == null)
                return;

            if (m_linkProfessionRole.m_LockRole == null || m_linkProfessionRole.m_LockRole.m_curModel == null)
                return;

            Vector3 ball_30111_pos_start = transform.position;
            ball_30111_pos_start.y = ball_30111_pos_start.y + m_linkProfessionRole.headOffset.y / 2;
            Vector3 ball_30111_pos = m_linkProfessionRole.m_LockRole.m_curModel.position;
            float ball_30111_hight = ball_30111_pos.y + m_linkProfessionRole.m_LockRole.headOffset.y / 2;
            float ball_3011_dis = Vector3.Distance(transform.position, m_linkProfessionRole.m_LockRole.m_curModel.position);

            m_linkProfessionRole.TurnToRole(m_linkProfessionRole.m_LockRole, false);

            for (int i = 0; i < 6; i++)
            {
                float pos_x = UnityEngine.Random.Range(-1, 1);
                float pos_y = UnityEngine.Random.Range(-0.5f, 1);
                float pos_z = UnityEngine.Random.Range(-1, 1);

                GameObject fx_inst = GameObject.Instantiate(MAGE_S3011,
                    new Vector3(ball_30111_pos_start.x + pos_x, ball_30111_pos_start.y + pos_y, ball_30111_pos_start.z + pos_z), transform.rotation) as GameObject;
                fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                Destroy(fx_inst, 4);

                Transform ball = fx_inst.transform;
                float time = 0.35f + ball_3011_dis * 0.01f;
                if (time > 3) time = 3;
                Tweener tween1 = ball.DOLocalMove(new Vector3(ball_30111_pos.x, ball_30111_hight, ball_30111_pos.z), time).SetDelay(0.1f * i);
                tween1.SetUpdate(true);
                int rang = UnityEngine.Random.Range(0, 5);
                switch (rang)
                {
                    case 1:
                        tween1.SetEase(Ease.OutQuad);
                        break;
                    case 2:
                        tween1.SetEase(Ease.OutCirc);
                        break;
                    case 3:
                        tween1.SetEase(Ease.OutCubic);
                        break;
                    case 4:
                        tween1.SetEase(Ease.OutExpo);
                        break;
                    case 5:
                        tween1.SetEase(Ease.OutElastic);
                        break;
                }
                tween1.OnComplete(delegate ()
                {
                    ball.gameObject.SetActive(false);
                    GameObject fx_inst_end = GameObject.Instantiate(MAGE_S3011_1) as GameObject;
                    fx_inst_end.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                    fx_inst_end.transform.localPosition = ball.transform.localPosition;
                    Destroy(fx_inst_end,2f);
                    Destroy(fx_inst, 1);

                    HitData hd = m_linkProfessionRole.Build_PRBullet(3004, 0.125f, SceneTFX.m_Bullet_Prefabs[1],
                        new Vector3(ball_30111_pos.x, ball_30111_hight, ball_30111_pos.z), transform.rotation);
                    hd.m_nDamage = 288;
                    hd.m_Color_Main = SkillModel.getskill_mcolor(3004);
                    hd.m_Color_Rim = SkillModel.getskill_rcolor(3004);
                    //hd.m_Color_Main = Color.blue;
                    //hd.m_Color_Rim = Color.cyan;

                    //hd.m_nHurtFX = 1;

                    //hd.m_nHurtSP_type = 11;
                    //hd.m_nHurtSP_pow = 2;
                });
            }
        }

        if (m_linkProfessionRole.getShowSkillEff() == 2)
            return;

        //debug.Log("法师的攻击记录点 id = " + id);
        if (id == 300111 || id == 300112 || id == 300121 || id == 300122 || id == 30013)
        {
            float dis = 1;
            int type = 1;

            switch (id)
            {
                case 300111:
                    dis = 2;
                    type = 4;
                    break;
                case 300112:
                    dis = 8;
                    type = 4;
                    break;
                case 300121:
                    dis = 2;
                    type = 5;
                    break;
                case 300122:
                    dis = 8;
                    type = 5;
                    break;
                case 30013:
                    dis = 3;
                    type = 7;
                    break;
            }

            Vector3 pos = transform.position + transform.forward * dis;

            HitData hd = m_linkProfessionRole.Build_PRBullet(3001, 0.125f, SceneTFX.m_Bullet_Prefabs[type], pos, transform.rotation);
            hd.m_nDamage = 288;
            hd.m_nHurtFX = 8;
            // hd.m_Color_Main = Color.blue;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3001);

           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3001);
        }

        //3号打一片的技能
        if (id == 101)
        {
            Vector3 pos = transform.position;

            HitData hd = m_linkProfessionRole.Build_PRBullet(3004, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);
            hd.m_nDamage = 288;
            //hd.m_Color_Main = Color.blue;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3004);
            //hd.m_Color_Rim = Color.cyan;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3004);
            hd.m_nHurtFX = 1;

            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 2;
        }

        //法师冰封
        if (3009 == id)
        {
            Vector3 pos = transform.position + transform.forward * 3.5f;

            HitData hd = m_linkProfessionRole.Build_PRBullet(3009, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);
            hd.m_nDamage = 288;
            // hd.m_Color_Main = Color.white;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3009);
            //hd.m_Color_Rim = new Color(0.02f, 0.73f, 0.92f, 0.51f);
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3009);
            hd.m_nHurtSP_type = 31;
            hd.m_nHurtSP_pow = 2;
        }

        if (30071 == id)
        {
            Vector3 pos = transform.position + transform.forward * 3.5f;
            HitData hd = m_linkProfessionRole.Build_PRBullet(3007, 3.5f, SceneTFX.m_Bullet_Prefabs[8], pos, transform.rotation);
            hd.m_nDamage = 0;
            //hd.m_Color_Main = Color.gray;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3007);
            //hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3007);
            pos.y += 1f;
            hd.m_nHurtSP_pos = pos;

            hd.m_nHurtSP_type = 14;
            hd.m_nHurtSP_pow = 4;
            hd.m_hurtNum = 0;
        }
        if (30072 == id)
        {
            cur_3007_1_time = 0;
            CancelInvoke("skill_3007_1");
            InvokeRepeating("skill_3007_1", 0, 0.4f);
        }
        if (30073 == id)
        {
            cur_3007_2_time = 0;
            CancelInvoke("skill_3007_2");
            InvokeRepeating("skill_3007_2", 0, 0.4f);
        }
    }

    //技能3002
    int cur_3002_num = 0;
    public Vector3 cur_3002_pos;
    public Vector3 cur_3002_forward;
    void skill_3002()
    {
        cur_3002_num++;

        Vector3 pos = cur_3002_pos + cur_3002_forward * 2f * cur_3002_num;

        float x = UnityEngine.Random.Range(0f, 1f);
        float z = UnityEngine.Random.Range(0f, 1f);

        pos.x = pos.x + x;
        pos.z = pos.z + z;

        GameObject fx_inst = GameObject.Instantiate(MAGE_S3002, pos, transform.rotation) as GameObject;
        if (SceneCamera.m_nSkillEff_Level > 1)
        {//隐藏部分特效
            Transform hide = fx_inst.transform.FindChild("hide");
            if (hide != null)
                hide.gameObject.SetActive(false);
        }
        fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        Destroy(fx_inst, 1.5f);

        HitData hd = m_linkProfessionRole.Build_PRBullet(3002, 1.0f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
        hd.m_nDamage = 1088;
        // hd.m_Color_Main = Color.gray;
        hd.m_Color_Main = SkillModel.getskill_mcolor(3002);
        //hd.m_Color_Rim = Color.white;
        hd.m_Color_Rim = SkillModel.getskill_rcolor(3002);
        hd.m_nHurtFX = 1;

        if (cur_3002_num >= 6)
        {
            CancelInvoke("skill_3002");
        }
    }


    int cur_3007_2_time = 0;
    int cur_3007_1_time = 0;
    void skill_3007_1()
    {
        cur_3007_1_time++;
        Vector3 pos = transform.position + transform.forward * 2f;
        HitData hd = m_linkProfessionRole.Build_PRBullet(3007, 0.3f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
        hd.m_nDamage = 88;
        //hd.m_Color_Main = Color.gray;
        hd.m_Color_Main = SkillModel.getskill_mcolor(3007);
        //hd.m_Color_Rim = Color.white;
        hd.m_Color_Rim = SkillModel.getskill_rcolor(3007);
        //hd.m_nHurtSP_type = 13;
        //hd.m_nHurtSP_pow = 4;

        if (cur_3007_1_time >= 5)
        {
            CancelInvoke("skill_3007_1");
        }
    }

    void skill_3007_2()
    {
        cur_3007_2_time++;
        Vector3 pos = transform.position + transform.forward * 2f;
        HitData hd = m_linkProfessionRole.Build_PRBullet(3007, 0.3f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
        hd.m_nDamage = 88;
        //hd.m_Color_Main = Color.gray;
        hd.m_Color_Main = SkillModel.getskill_mcolor(3007);
       // hd.m_Color_Rim = Color.white;
        hd.m_Color_Rim = SkillModel.getskill_rcolor(3007);
        //hd.m_nHurtSP_type = 13;
        //hd.m_nHurtSP_pow = 4;
        hd.m_nLastHit = 1;

        if (cur_3007_2_time >= 5)
        {
            CancelInvoke("skill_3007_2");
        }
    }
               

    public void onJump(int id)
    {
        if (3010 == id)
        {//闪现
            NavMeshPath path = new NavMeshPath();
            NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
            Vector3 pos = transform.position;

            int max_dis = 10;
            for (int i = max_dis; i >= 0; i--)
            {
                Vector3 endPos = pos + transform.forward * i;
                if (nav.CalculatePath(endPos, path))
                {
                    float dis = Vector3.Distance(pos, endPos);
                    transform.DOJump(endPos, 0.2f * dis + endPos.y, 1, 0.03f);
                    break;
                }
            }
        }
    }
}
