using System;
using UnityEngine;
using System.Collections;
using MuGame;
using GameFramework;
using Cross;

public class P3Mage : ProfessionRole
{
    static public GameObject P3MAGE_SFX1, P3MAGE_SFX2, P3MAGE_SFX3;
    GameObject m_SFX1, m_SFX2, m_SFX3;
    public void Init(string name, int layer,Vector3 pos,bool isUser=false, Variant serverData = null , uint cid = 0 )
    {
        m_ePRProfession = A3_PROFESSION.Mage;
        m_strAvatarPath = "profession_mage_";
        m_strEquipEffPath = "Fx_armourFX_mage_";
        //roleName = "我是法师";
        roleName = name;

      //  m_fNavSpeed = 5f;
        setNavLay(NavmeshUtils.listARE[1]);
        
        base.Init("profession_mage_inst", layer, pos, isUser, serverData , cid);

        P3Mage_Event mage_event = m_curModel.gameObject.AddComponent<P3Mage_Event>();
        mage_event.m_linkProfessionRole = this;

        m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

        //wing = 1;
        set_weaponl(0, 0); //weapon_l = 0;
        set_body(0, 0);// body = 0;
    }


    public override void PlaySkill(int id)
    {
        if (m_curSkillId != 0)
            return;

        base.PlaySkill( id );

        //if (3007 == id)
        //{
        //    m_nKeepSkillCount = 0;
        //}

        //if (3002 == id)
        //{
        //    runSkill_3002();
        //}
        if (3003 == id && getShowSkillEff() != 2)
        {
            runSkill_3003();
        }
        if (3008 == id && getShowSkillEff() != 2)
        {
            runSkill_3008();
        }
        if (30081 == id)
        {//移除buff。只限前端使用
            removeSkill_30081();
        }
        //if (id == 4)
        //{
        //    if (UnityEngine.Random.Range(0, 3) < 1)
        //    {
        //        //debug.Log("加入特殊效果");
        //        m_proAvatar.push_fx(1);
        //    }
        //    else
        //    {
        //        //debug.Log("移除----特殊效果");
        //        m_proAvatar.pop_fx();
        //    }
        //    return;
        //}
        if (m_isMain)
            m_moveAgent.avoidancePriority = 50;

        m_curSkillId = id;
        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);
       
        m_fAttackCount = 1.0f;

        if (3010 == id)
            m_fAttackCount = 0.5f;
    }

    //法师电击技能特殊处理
    //TickItem process_3002;
    //float m_cur3002_time = 0;

    //public int m_skill3002_zigs = 100;
    //public float m_skill3002_scale = 2f;
    //public float m_skill3002_speed = 5f;
    //ParticleEmitter m_skill3002_particleEmitter;

    //Perlin m_skill3002_noise;
    //float m_skill3002_oneOverZigs;
    //private Particle[] m_skill3002_particles;
    //void runSkill_3002()
    //{
    //    if (m_SFX3 == null)
    //    {
    //        m_SFX3 = GameObject.Instantiate(P3MAGE_SFX3) as GameObject;
    //        m_SFX3.transform.SetParent(U3DAPI.FX_POOL_TF, false);`
    //        m_skill3002_particleEmitter = m_SFX3.GetComponent<ParticleEmitter>();
    //        GameObject.Destroy(m_SFX3, 2);
    //        SceneFXMgr.Instantiate("FX_mage_SFX_dianji_01", m_curModel.transform.position, m_curModel.transform.rotation, 4f);
    //    }

    //    GameObject fx_inst = GameObject.Instantiate(P3MAGE_SFX3, m_curModel.transform.position, m_curModel.transform.rotation) as GameObject;
    //    fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
    //    GameObject.Destroy(fx_inst, 2f);

    //    float dis = Vector3.Distance(m_LockRole.m_curModel.position, m_curModel.position);
    //    fx_inst.transform.localScale = new Vector3(1,1,4);
    //    if (process_3002 == null)
    //    {
    //        process_3002 = new TickItem(onUpdate_3002);
    //        TickMgr.instance.addTick(process_3002);

    //        m_skill3002_oneOverZigs = 1f / (float)m_skill3002_zigs;
    //        m_skill3002_particleEmitter.emit = false;

    //        m_skill3002_particleEmitter.Emit(m_skill3002_zigs);
    //        m_skill3002_particles = m_skill3002_particleEmitter.particles;
    //    }
    //}
    //void onUpdate_3002(float s)
    //{
    //    if (m_LockRole == null || m_cur3002_time > 0.6f)
    //    {
    //        m_cur3002_time = 0;
    //        TickMgr.instance.removeTick(process_3002);
    //        process_3002 = null;

    //        GameObject.Destroy(m_SFX3);
    //        m_SFX3 = null;
    //        return;
    //    }

    //    m_cur3002_time += s;

    //    if (m_skill3002_noise == null)
    //        m_skill3002_noise = new Perlin();

    //    float timex = Time.time * m_skill3002_speed * 0.1365143f;
    //    float timey = Time.time * m_skill3002_speed * 1.21688f;
    //    float timez = Time.time * m_skill3002_speed * 2.5564f;


    //    Vector3 pos1 = m_curModel.transform.position;
    //    pos1 = new Vector3(pos1.x, pos1.y + 1, pos1.z);
    //    Vector3 pos2 = m_LockRole.m_curModel.position;
    //    float rate = 0;
    //    if (m_cur3002_time < 0.3f)
    //        rate = m_cur3002_time / 0.3f;
    //    else
    //        rate = (m_cur3002_time - 0.1f) / 0.5f;

    //    Vector3 pos3 = new Vector3((pos2.x - pos1.x) * rate + pos1.x, pos1.y + 1, (pos2.z - pos1.z)* rate + pos1.z);

    //    for (int i = 0; i < m_skill3002_particles.Length; i++)
    //    {


    //        Vector3 position = Vector3.Lerp(pos1, pos3, m_skill3002_oneOverZigs * (float)i);
    //        Vector3 offset = new Vector3(m_skill3002_noise.Noise(timex + position.x, timex + position.y, timex + position.z),
    //                                    m_skill3002_noise.Noise(timey + position.x, timey + position.y, timey + position.z),
    //                                    m_skill3002_noise.Noise(timez + position.x, timez + position.y, timez + position.z));
    //        position += (offset * m_skill3002_scale * ((float)i * m_skill3002_oneOverZigs));

    //        m_skill3002_particles[i].position = position;
    //        m_skill3002_particles[i].color = Color.white;
    //        m_skill3002_particles[i].energy = 1f;
    //    }

    //    m_skill3002_particleEmitter.particles = m_skill3002_particles;
    //}

    //法师技能3003冰雨特殊处理
    TickItem process_3003;
    float m_skill3003_time = 4;  //持续时间
    float m_cur3003_time = 0;
    int m_skill3003_num = 20;   //冰锥数量
    int m_cur3003_num = 0;
    Vector3 m_3003_pos;
    Quaternion m_3003_rotation;
    void runSkill_3003()
    {
        if (process_3003 == null)
        {
            process_3003 = new TickItem(onUpdate_3003);
            TickMgr.instance.addTick(process_3003);
        }
        m_cur3003_time = 0;
        m_cur3003_num = 0;
        m_3003_pos = m_curModel.transform.position;
        m_3003_rotation = m_curModel.transform.rotation;

        if (m_SFX2 == null)
        {
            m_SFX2 = GameObject.Instantiate(P3MAGE_SFX2, m_3003_pos, m_3003_rotation) as GameObject;
            m_SFX2.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        }
    }
    void onUpdate_3003(float s)
    {
        //跨地图时m_curModel==null
        if (m_curModel == null)
        {
            TickMgr.instance.removeTick(process_3003);

            process_3003 = null;
            m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
            GameObject.Destroy(m_SFX2, 2);
            m_SFX2 = null;

            return;
        }

        m_cur3003_time += s;
        float nexttime = 0.5f + m_cur3003_num * (m_skill3003_time - 1) / m_skill3003_num;
        if (m_cur3003_time > nexttime)
        {
            onBullet_3003(m_3003_pos,m_3003_rotation);
            m_cur3003_num++;
        }

        if (m_cur3003_time > m_skill3003_time)
        {
            m_cur3003_time = 0;
            m_cur3003_num = 0;
            TickMgr.instance.removeTick(process_3003);
            process_3003 = null;

            m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
            GameObject.Destroy(m_SFX2, 2);
            m_SFX2 = null;
        }
    }
    public void onBullet_3003(Vector3 pos, Quaternion rotation)
    {
        //冰雹术
        pos.z += UnityEngine.Random.Range(-3f, 3f);
        pos.x += UnityEngine.Random.Range(-3f, 3f);
        pos.y = 16f + pos.y;

        GameObject bult = GameObject.Instantiate(P3Mage_Event.MAGE_B3, pos, rotation) as GameObject;
        bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

        Transform real_track = bult.transform.FindChild("t");
        if (real_track != null)
        {
            HitData hd = Link_PRBullet(3003, 2f, bult, real_track);

            hd.m_nHurtSP_type = 11;
            hd.m_nHurtSP_pow = 1;
            hd.m_nDamage = 177;

            //hd.m_Color_Main = Color.blue;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3003);
           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3003);
            hd.m_aniTrack = real_track.GetComponent<Animator>();
            real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

            Transform real_fx = real_track.FindChild("f");
            if (real_fx != null)
            {
                hd.m_aniFx = real_fx.GetComponent<Animator>();
            }
        }
    }

    //护盾技能特殊处理
    TickItem process_3008;
    float m_cur3008_time = 0;
    public void runSkill_3008()
    {
        if (m_SFX1 == null)
        {
            m_SFX1 = GameObject.Instantiate(P3MAGE_SFX1) as GameObject;
            m_SFX1.transform.SetParent(m_curModel, false);
            m_SFX1.SetActive(true);
        }

        if (process_3008 == null)
        {
            process_3008 = new TickItem(onUpdate_3008);
            TickMgr.instance.addTick(process_3008);
        }
        m_cur3008_time = 0;
    }
    void removeSkill_30081()
    {
        if (m_SFX1!=null)
        {
            GameObject.Destroy(m_SFX1);
            m_cur3008_time = 0;
            if(process_3008 != null)
            TickMgr.instance.removeTick(process_3008);
            process_3008 = null;
            m_SFX1 = null;

            GameObject _insta = GameObject.Instantiate(P3MAGE_SFX3) as GameObject;
            _insta.transform.SetParent(m_curModel, false);
            GameObject.Destroy(_insta, 1f);
        }
    }

    void onUpdate_3008(float s)
    {
        //跨地图时m_curModel==null
        if (m_curModel == null)
        {
            TickMgr.instance.removeTick(process_3008);
            return;
        }


        m_cur3008_time += s;
        if (m_cur3008_time > 0.6f)
        {
            m_SFX1.SetActive(true);
            m_cur3008_time = 0;
            TickMgr.instance.removeTick(process_3008);
            process_3008 = null;
        }
    }
}
