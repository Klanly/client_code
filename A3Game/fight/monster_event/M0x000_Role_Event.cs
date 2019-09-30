using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using DG.Tweening;

public class M0x000_Role_Event : Monster_Base_Event
{
    //加入不能转身的时间no turn
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

    static public GameObject WARRIOR_B1;
    static public GameObject ASSASSIN_S1, ASSASSIN_S2;

    public string fx_2003_name = "skill_2003";
    public string fx_5003_name = "skill_5003";
    private void onNT(float time) {
		m_monRole.m_fSkillShowTime = time;
	}
	private void onSFX(int id) {
		if (m_monRole is M000P2 || m_monRole is ohterP2Warrior ) {
            if (id == 2003)
            {
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
			SceneFXMgr.Instantiate("FX_warrior_SFX_" + id.ToString(), transform.position, transform.rotation, 4f);
		}
		else if (m_monRole is M000P3 || m_monRole is ohterP3Mage) {
			SceneFXMgr.Instantiate("FX_mage_SFX_" + id.ToString(), transform.position, transform.rotation, 6f);
		}
		else if (m_monRole is M000P5 || m_monRole is ohterP5Assassin) {
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
            SceneFXMgr.Instantiate("FX_assa_SFX_" + id.ToString(), transform.position, transform.rotation, 4f);
		}
	}
    void SFX_2003_hide()
    {
        if (transform.FindChild(fx_2003_name) != null)
            transform.FindChild(fx_2003_name).gameObject.SetActive(false);
    }
    void SFX_5003_hide()
    {
        if (transform.FindChild(fx_5003_name) != null)
            transform.FindChild(fx_5003_name).gameObject.SetActive(false);
    }

    public void onWing(float time) {
		
	}
	private void Bullet_1() {

	}
    private void onND() { }
	private void onSound() { }
	public void onShake(string param) {

	}

	private void Bullet_2() {

	}
    public void onJump(int id)
    {
        if (2009 == id)
        {
            //判断是否跳到阻挡外
            NavMeshPath path = new NavMeshPath();
            NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
            Vector3 pos = transform.position;
            if (m_monRole.m_LockRole != null)
            {
                float true_dis = Vector3.Distance(pos, m_monRole.m_LockRole.m_curModel.position);

                int j = 1;
                for (; j < true_dis; j++)
                {
                    Vector3 endPos = m_monRole.m_LockRole.m_curModel.position - transform.forward * j;
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
                        Vector3 endPos = m_monRole.m_LockRole.m_curModel.position - transform.forward * i;
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
                if (i > 3)
                    transform.DOJump(endPos, 0.2f * i + endPos.y, 1, 0.07f * i);
            }

            path = null;
        }
        if (id == 2006)
        {
            //判断是否跳到阻挡外
            NavMeshPath path = new NavMeshPath();
            NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
            Vector3 pos = transform.position;

            if (m_monRole.m_LockRole != null)
            {
                float true_dis = Vector3.Distance(pos, m_monRole.m_LockRole.m_curModel.position);

                int j = 1;
                for (; j < true_dis; j++)
                {
                    Vector3 endPos = m_monRole.m_LockRole.m_curModel.position - transform.forward * j;
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
                   // transform.DOJump(/*endPos*/pos, pos.y, 1, 0/*0.07f * dis*/);
                }
                else
                {//半途无阻碍
                    int i = 3;
                    for (; i <= 3 && i >= 0; i--)
                    {
                        Vector3 endPos = m_monRole.m_LockRole.m_curModel.position - transform.forward * i;
                        if (nav.CalculatePath(endPos, path))
                        {
                            float dis = Vector3.Distance(pos, endPos);
                           // transform.DOJump(/*endPos*/pos, endPos.y, 1, 0.07f * dis);
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
              //  if (i > 3)
                  //  transform.DOJump(/*endPos*/pos, endPos.y, 1, 0.07f * i);
            }

            path = null;
        }
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
        if (5009 == id)
        {//死亡印记
            if (m_monRole.m_LockRole != null)
            {
                NavMeshPath path = new NavMeshPath();
                NavMeshAgent nav = transform.GetComponent<NavMeshAgent>();
                Vector3 pos = transform.position;

                int i = 2;
                for (; i <= 2 && i >= 0; i--)
                {
                    Vector3 endPos = m_monRole.m_LockRole.m_curModel.position + transform.forward * i;
                    if (nav.CalculatePath(endPos, path))
                    {
                        float dis = Vector3.Distance(pos, endPos);
                        transform.DOJump(endPos, 0.2f * dis + endPos.y, 1, 0.01f);
                        break;
                    }
                }
                GameObject inst = GameObject.Instantiate(ASSASSIN_S1) as GameObject;
                inst.transform.SetParent(m_monRole.m_LockRole.m_curModel, false);

                GameObject.Destroy(inst, 3f);
            }
        }
       // m_monRole.ShowAll();
    }
    public void onBullet(int id) {
        if (m_monRole is M000P3 || m_monRole is ohterP3Mage)
        {
            if (id == 1)
            {
                Vector3 pos = transform.position + transform.forward * 0.5f;
                pos.y += 1.25f;

                GameObject bult = GameObject.Instantiate(MAGE_B1, pos, transform.rotation) as GameObject;
                bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

                Transform real_track = bult.transform.FindChild("t");
                if (real_track != null)
                {
                    HitData hd = m_monRole.Link_PRBullet(3001, 2f, bult, real_track);

                    //hd.m_Color_Main = Color.gray;
                    hd.m_Color_Main= SkillModel.getskill_mcolor(3001);
                   // hd.m_Color_Rim = Color.white;
                    hd.m_Color_Rim = SkillModel.getskill_rcolor(3001);
                    hd.m_nDamage = 277;

                    hd.m_aniTrack = real_track.GetComponent<Animator>();
                    real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

                    Transform real_fx = real_track.FindChild("f");
                    if (real_fx != null)
                    {
                        hd.m_aniFx = real_fx.GetComponent<Animator>();
                    }
                }
            }
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
                    HitData hd = m_monRole.Link_PRBullet(3005, 2f, bult, real_track);
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
            if (id == 101)
            {
                Vector3 pos = transform.position;

                HitData hd = m_monRole.Build_PRBullet(3004, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);
                hd.m_nDamage = 288;
                //hd.m_Color_Main = Color.blue;
                hd.m_Color_Main = SkillModel.getskill_mcolor(3004);
               // hd.m_Color_Rim = Color.cyan;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(3004);
                hd.m_nHurtFX = 1;

                hd.m_nHurtSP_type = 11;
                hd.m_nHurtSP_pow = 2;
            }
            if (3006 == id)
            {
                if (m_monRole.m_LockRole == null)
                    return;

                Vector3 pos = m_monRole.m_LockRole.m_curModel.position;
                pos.z += UnityEngine.Random.Range(-2f, 2f);
                pos.x += UnityEngine.Random.Range(-2f, 2f);
                pos.y = pos.y + 8f;

                GameObject bult = GameObject.Instantiate(P3Mage_Event.MAGE_B6, pos, transform.rotation) as GameObject;
                bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

                Transform real_track = bult.transform.FindChild("t");
                if (real_track != null)
                {
                    HitData hd = m_monRole.Link_PRBullet(3006, 3.0f, bult, real_track);

                    hd.m_nHurtSP_type = 11;
                    hd.m_nHurtSP_pow = 1;

                    //陨石要穿过角色直到地面爆开
                    hd.m_bOnlyHit = false;

                    // hd.m_Color_Main = Color.blue;
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
            if (3002 == id)
            {
                cur_3002_num = 0;
                cur_3002_forward = transform.forward;
                cur_3002_pos = transform.position;

                CancelInvoke("skill_3002");
                InvokeRepeating("skill_3002", 0, 0.2f);
            }
            if (3009 == id)
            {
                Vector3 pos = transform.position + transform.forward * 3.5f;

                HitData hd = m_monRole.Build_PRBullet(3009, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);
                hd.m_nDamage = 288;
                //hd.m_Color_Main = Color.white;
                hd.m_Color_Main = SkillModel.getskill_mcolor(3009);
                // hd.m_Color_Rim = new Color(0.02f, 0.73f, 0.92f, 0.51f);
                hd.m_Color_Rim = SkillModel.getskill_rcolor(3009);
                hd.m_nHurtSP_type = 31;
                hd.m_nHurtSP_pow = 2;
            }

            if (30071 == id)
            {
                Vector3 pos = transform.position + transform.forward * 7f;
                HitData hd = m_monRole.Build_PRBullet(3007, 3.5f, SceneTFX.m_Bullet_Prefabs[8], pos, transform.rotation);
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
            if (30111 == id)
            {
                m_cur_ball_30111.Clear();

                if (m_monRole == null)
                    return;

                ball_30111_pos = m_monRole.m_LockRole.m_curModel.position;
                ball_30111_hight = m_monRole.m_LockRole.headOffset.y / 2;
                ball_3011_dis = Vector3.Distance(transform.position, m_monRole.m_LockRole.m_curModel.position);

                GameObject fx_inst = GameObject.Instantiate(MAGE_S3011, transform.position, transform.rotation) as GameObject;
                fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);

                for (int i = 1; i <= fx_inst.transform.FindChild("heiqiu").childCount; i++)
                {
                    m_cur_ball_30111.Add(fx_inst.transform.FindChild("heiqiu/0" + i));
                }

                GameObject.Destroy(fx_inst, 6);
            }
            if (30112 == id)
            {
                if (m_monRole.m_LockRole != null)
                {
                    m_monRole.TurnToRole(m_monRole.m_LockRole, false);
                    //m_cur_ball_30111.Clear();
                    //return;
                }

                if (m_cur_ball_30111.Count == 0)
                    return;

                //float dis = Vector3.Distance(transform.position, m_linkProfessionRole.m_LockRole.m_curModel.position);
                //float high = m_linkProfessionRole.m_LockRole.headOffset.y / 2;
                for (int i = 0; i < m_cur_ball_30111.Count; i++)
                {
                    Transform one = m_cur_ball_30111[i];
                    one.GetComponent<Animator>().enabled = false;
                    Transform ball = one.FindChild("top/heiqiu (1)");
                    Vector3 pos_ball = ball.localPosition;
                    ball.transform.DOLocalMove(pos_ball / 3, 0.35f).SetDelay(0.1f * (int)(i + 1) / 2);
                    Tweener tween1 = one.transform.DOLocalMove(new Vector3(0, ball_30111_hight, ball_3011_dis), 0.35f + ball_3011_dis * 0.01f).SetDelay(0.1f * i);
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
                        GameObject fx_inst = GameObject.Instantiate(MAGE_S3011_1) as GameObject;
                        fx_inst.transform.SetParent(one, false);

                        HitData hd = m_monRole.Build_PRBullet(3004, 0.125f, SceneTFX.m_Bullet_Prefabs[1],
                            ball_30111_pos, transform.rotation);
                        hd.m_nDamage = 288;
                    });
                    if (i == m_cur_ball_30111.Count - 1)
                    {
                        m_cur_ball_30111.Clear();
                    }
                }
            }
        }
        else if (m_monRole is M000P2 || m_monRole is ohterP2Warrior)
        {
            if (id == 1)
            {
                Vector3 pos = transform.position + transform.forward * 1.5f;
                HitData hd = m_monRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

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
                HitData hd = m_monRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
                hd.m_nDamage = 128;
                // hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2001);
                //hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2001);
                hd.m_nHurtFX = 2;
            }

            if (id == 3)
            {
                Vector3 pos = transform.position + transform.forward * 2.5f;
                HitData hd = m_monRole.Build_PRBullet(2001, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
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
            if (id == 4)
            {
                Vector3 pos = transform.position;// +transform.forward * 1.5f;
                HitData hd = m_monRole.Build_PRBullet(2003, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
                hd.m_nDamage = 88;
                //hd.m_Color_Main = Color.red;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2003);
                //hd.m_Color_Rim = Color.red;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2003);
                hd.m_nHurtFX = 3;
            }
            if (id == 21)
            {
                Vector3 pos = transform.position + transform.forward * 4f;
                HitData hd = m_monRole.Build_PRBullet(2004, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
                hd.m_nDamage = 388;
                //hd.m_Color_Main = Color.red;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2004);
               // hd.m_Color_Rim = Color.red;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2004);
                hd.m_nHurtSP_type = 1;
                hd.m_nHurtSP_pow = 4;
            }
            if (13 == id)
            {
                if (m_monRole.m_LockRole != null)
                {
                    Vector3 pos = transform.position + transform.forward * 0.5f;
                    pos.y += 1.25f;

                    HitData hd = m_monRole.Build_PRBullet(2004, 0f, WARRIOR_B1, pos, transform.rotation);

                    FollowBullet_Mgr.AddBullet(m_monRole.m_LockRole, hd, 0.8f);
                }
            }
            if (141 == id)
            {
                Vector3 pos = transform.position + transform.forward * 3.0f;
                HitData hd = m_monRole.Build_PRBullet(2002, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);

                hd.m_nDamage = 108;
                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2002);
                //hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2002);
                hd.m_nHurtFX = 2;
            }
            if (142 == id)
            {
                Vector3 pos = transform.position + transform.forward * 6.0f;
                HitData hd = m_monRole.Build_PRBullet(2002, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);

                hd.m_nDamage = 88;
                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2002);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2002);
                hd.m_nHurtFX = 2;
            }

            if (143 == id)
            {
                Vector3 pos = transform.position + transform.forward * 4.0f;
                HitData hd = m_monRole.Build_PRBullet(2002, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);

                hd.m_nDamage = 188;
                // hd.m_Color_Main = Color.gray;
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
                HitData hd = m_monRole.Build_PRBullet(2009, 0.2f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);
                hd.m_nDamage = 88;
                // hd.m_Color_Main = Color.red;
                hd.m_Color_Main = SkillModel.getskill_mcolor(2009);
               // hd.m_Color_Rim = Color.red;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(2009);
                hd.m_nHurtSP_type = 11;
                hd.m_nHurtSP_pow = 4;

                hd.m_nHurtFX = 6;
            }
            if (2006 == id)
            {
                Vector3 pos = transform.position/* + transform.forward * 3f*/;
                HitData hd = m_monRole.Build_PRBullet(2006, 0.2f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
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
                HitData hd = m_monRole.Build_PRBullet(2007, 0.3f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
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
        else if (m_monRole is M000P5 || m_monRole is ohterP5Assassin)
        {
            if (id == 1)
            {
                Vector3 pos = transform.position + transform.forward * 1.5f;
                pos.y += 1f;
                HitData hd = m_monRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                // hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);
                hd.m_nHurtFX = 2;
            }

            if (id == 2)
            {
                Vector3 pos = transform.position + transform.forward * 1.5f;
                pos.y += 1f;

                HitData hd = m_monRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
                // hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);
                hd.m_nHurtSP_type = 1;
                hd.m_nHurtSP_pow = 2;

                hd.m_nHurtFX = 2;
            }

            if (id == 3)
            {
                Vector3 pos = transform.position + transform.forward * 2.5f;
                pos.y += 1f;

                HitData hd = m_monRole.Build_PRBullet(5001, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);
                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5001);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5001);
                hd.m_nHurtFX = 2;

                hd.m_nLastHit = 1;
            }

            //刺客的疾风连刺
            if (id == 50021)
            {
                Vector3 pos = transform.position + transform.forward * 1.5f;
                pos.y += 1f;

                HitData hd = m_monRole.Build_PRBullet(5002, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                hd.m_nDamage = 58;
                // hd.m_Color_Main = Color.gray;
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

                HitData hd = m_monRole.Build_PRBullet(5002, 0.125f, SceneTFX.m_Bullet_Prefabs[5], pos, transform.rotation);

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

                HitData hd = m_monRole.Build_PRBullet(5003, 0.125f, SceneTFX.m_Bullet_Prefabs[4], pos, transform.rotation);

                hd.m_nDamage = 57;
                // hd.m_Color_Main = Color.blue;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5003);
               // hd.m_Color_Rim = Color.cyan;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5003);
                hd.m_nHurtFX = 5;
            }

            //(影袭)
            if (id == 50041)
            {
                Vector3 pos = transform.position + transform.forward * 1.5f;
                pos.y += 1f;
                HitData hd = m_monRole.Build_PRBullet(5004, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

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
                HitData hd = m_monRole.Build_PRBullet(5004, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5004);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5004);
                hd.m_nLastHit = 1;
                hd.m_nHurtFX = 5;
            }

            //特殊处理瞬移到敌人身后(影袭)
            if (131 == id)
            {
                if (m_monRole.m_LockRole != null)
                {
                    Vector3 dis_pos = m_monRole.m_curModel.position - m_monRole.m_LockRole.m_curModel.position;
                    if (dis_pos.magnitude < 4f)
                    {
                        m_monRole.m_curModel.position = m_monRole.m_LockRole.m_curModel.position - m_monRole.m_LockRole.m_curModel.forward * 4f;
                        m_monRole.m_curModel.forward = m_monRole.m_LockRole.m_curModel.forward;
                    }
                }

                //m_monRole.m_curModel.position = hd.m_CastRole.m_curModel.position + hd.m_CastRole.m_curModel.forward * 2f;
            }

            //幻影杀阵
            if (50071 == id)
            {
                Vector3 pos = transform.position;
                //pos.y += 1f;
                HitData hd = m_monRole.Build_PRBullet(5007, 0.125f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5007);
              //  hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5007);
                hd.m_nHurtFX = 5;
            }
            if (50072 == id)
            {
                Vector3 pos = transform.position;
                //pos.y += 1f;
                HitData hd = m_monRole.Build_PRBullet(5007, 0.125f, SceneTFX.m_Bullet_Prefabs[7], pos, transform.rotation);

                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5007);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5007);
                hd.m_nHurtFX = 1;
            }
            if (5006 == id)
            {
                Vector3 pos = transform.position;
                HitData hd = m_monRole.Build_PRBullet(5006, 1.0f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                //hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5006);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5006);
                hd.m_nHurtFX = 2;
            }
            if (5009 == id)
            {
                if (m_monRole.m_LockRole != null)
                {//闪现过去后朝向目标
                    m_monRole.TurnToRole(m_monRole.m_LockRole, false);
                }

                Vector3 pos = transform.position;
                HitData hd = m_monRole.Build_PRBullet(5009, 1.0f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);

                // hd.m_Color_Main = Color.gray;
                hd.m_Color_Main = SkillModel.getskill_mcolor(5009);
               // hd.m_Color_Rim = Color.white;
                hd.m_Color_Rim = SkillModel.getskill_rcolor(5009);
                hd.m_nHurtFX = 2;
            }
        }
        else
        {
            if (1 == id)
            {
                //普通攻击技能
                Vector3 pos = transform.position + transform.forward * 2f;
                pos.y += 1f;

                HitData hd = m_monRole.BuildBullet(1, 0.125f, SceneTFX.m_Bullet_Prefabs[1], pos, transform.rotation);
                if (!m_monRole.isfake)
                    hd.m_nDamage = 0;
            }
        }
	}
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
        fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        Destroy(fx_inst, 0.5f);

        HitData hd = m_monRole.Build_PRBullet(3002, 1.0f, SceneTFX.m_Bullet_Prefabs[3], pos, transform.rotation);
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

    public Vector3 ball_30111_pos;
    public float ball_30111_hight;
    public float ball_3011_dis;
    public List<Transform> m_cur_ball_30111 = new List<Transform>();

    int cur_3007_2_time = 0;
    int cur_3007_1_time = 0;
    void skill_3007_1()
    {
        cur_3007_1_time++;
        Vector3 pos = transform.position + transform.forward * 2f;
        HitData hd = m_monRole.Build_PRBullet(3007, 0.3f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
        hd.m_nDamage = 88;
        // hd.m_Color_Main = Color.gray;
        hd.m_Color_Main = SkillModel.getskill_mcolor(3007);
       // hd.m_Color_Rim = Color.white;
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
        HitData hd = m_monRole.Build_PRBullet(3007, 0.3f, SceneTFX.m_Bullet_Prefabs[6], pos, transform.rotation);
        hd.m_nDamage = 88;
        //  hd.m_Color_Main = Color.gray;
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

    public void onHide(int id)
    {
        if (5009 == id)
        {
            if (m_monRole.m_LockRole != null)
            {
                Vector3 pos = Vector3.zero;
                pos.y = pos.y + m_monRole.m_LockRole.headOffset.y;

                GameObject inst = GameObject.Instantiate(ASSASSIN_S2, pos, m_monRole.m_LockRole.m_curModel.rotation) as GameObject;
                inst.transform.SetParent(m_monRole.m_LockRole.m_curModel, false);
                GameObject.Destroy(inst, 3f);
            }
        }
    }

}
