using System;
using UnityEngine;
using System.Collections;
using MuGame;
using GameFramework;

namespace MuGame
{
    class ohterP3Mage : MonsterPlayer
    {
        static public GameObject P3MAGE_SFX1, P3MAGE_SFX2, P3MAGE_SFX3;
        GameObject m_SFX1, m_SFX2, m_SFX3;
        public void Init(string name, int layer, Vector3 pos)
        {
            m_strMPAvatarPath = "profession_mage_";
            m_eMPProfession = A3_PROFESSION.Mage;

            //roleName = "我是法师";
            roleName = name;

            //  m_fNavSpeed = 5f;
            setNavLay(NavmeshUtils.listARE[1]);

            base.Init("profession_mage_inst", layer, pos);
            //P3Mage_Event mage_event = m_curModel.gameObject.AddComponent<P3Mage_Event>();
            //mage_event.ohter_linkProfessionRole = this;
            M0x000_Role_Event mde = m_curModel.gameObject.AddComponent<M0x000_Role_Event>();
            mde.m_monRole = this;
            MonHurtPoint mhtt = m_curPhy.gameObject.AddComponent<MonHurtPoint>();
            mhtt.m_monRole = this;
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

            //wing = 1;
            set_weaponl(0, 0); //weapon_l = 0;
            set_body(0, 0);// body = 0;
        }
        public override void PlaySkill(int id)
        {
            //if (m_curSkillId != 0)
            //    return;

            //if (3007 == id)
            //{
            //    m_nKeepSkillCount = 0;
            //}

            //if (3002 == id)
            //{
            //    runSkill_3002();
            //}
            //if (3003 == id)
            //{
            //    runSkill_3003();
            //}
            if (3008 == id)
            {
                runSkill_3008();
            }
            if (30081 == id)
            {
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
        TickItem process_3003;
        float m_skill3003_time = 4;  //持续时间
        float m_cur3003_time = 0;
        int m_skill3003_num = 20;   //冰锥数量
        int m_cur3003_num = 0;
        Vector3 m_3003_pos;
        Quaternion m_3003_rotation;
        //void runSkill_3003()
        //{
        //    if (process_3003 == null)
        //    {
        //        process_3003 = new TickItem(onUpdate_3003);
        //        TickMgr.instance.addTick(process_3003);
        //    }
        //    m_cur3003_time = 0;
        //    m_cur3003_num = 0;
        //    m_3003_pos = m_curModel.transform.position;
        //    m_3003_rotation = m_curModel.transform.rotation;

        //    if (m_SFX2 == null)
        //    {
        //        m_SFX2 = GameObject.Instantiate(P3MAGE_SFX2, m_3003_pos, m_3003_rotation) as GameObject;
        //        m_SFX2.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        //    }
        //}
        //void onUpdate_3003(float s)
        //{
        //    //跨地图时m_curModel==null
        //    if (m_curModel == null)
        //    {
        //        TickMgr.instance.removeTick(process_3003);

        //        process_3003 = null;
        //        m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
        //        GameObject.Destroy(m_SFX2, 2);
        //        m_SFX2 = null;

        //        return;
        //    }

        //    m_cur3003_time += s;
        //    float nexttime = 0.5f + m_cur3003_num * (m_skill3003_time - 1) / m_skill3003_num;
        //    if (m_cur3003_time > nexttime)
        //    {
        //        onBullet_3003(m_3003_pos, m_3003_rotation);
        //        m_cur3003_num++;
        //    }

        //    if (m_cur3003_time > m_skill3003_time)
        //    {
        //        m_cur3003_time = 0;
        //        m_cur3003_num = 0;
        //        TickMgr.instance.removeTick(process_3003);
        //        process_3003 = null;

        //        m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
        //        GameObject.Destroy(m_SFX2, 2);
        //        m_SFX2 = null;
        //    }
        //}
        //public void onBullet_3003(Vector3 pos, Quaternion rotation)
        //{
        //    //冰雹术
        //    pos.z += UnityEngine.Random.Range(-3f, 3f);
        //    pos.x += UnityEngine.Random.Range(-3f, 3f);
        //    pos.y = 16f + pos.y;

        //    GameObject bult = GameObject.Instantiate(P3Mage_Event.MAGE_B3, pos, rotation) as GameObject;
        //    bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

        //    Transform real_track = bult.transform.FindChild("t");
        //    if (real_track != null)
        //    {
        //        HitData hd = Link_PRBullet(3003, 2f, bult, real_track);

        //        hd.m_nHurtSP_type = 11;
        //        hd.m_nHurtSP_pow = 1;
        //        hd.m_nDamage = 177;

        //        hd.m_Color_Main = Color.blue;
        //        hd.m_Color_Rim = Color.white;

        //        hd.m_aniTrack = real_track.GetComponent<Animator>();
        //        real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

        //        Transform real_fx = real_track.FindChild("f");
        //        if (real_fx != null)
        //        {
        //            hd.m_aniFx = real_fx.GetComponent<Animator>();
        //        }
        //    }
        //}


        //护盾技能特殊处理
        TickItem process_3008;
        float m_cur3008_time = 0;
        public void runSkill_3008()
        {
            if (m_SFX1 == null)
            {
                m_SFX1 = GameObject.Instantiate(P3MAGE_SFX1) as GameObject;
                m_SFX1.transform.SetParent(m_curModel, false);
                m_SFX1.SetActive(false);
            }
            if (process_3008 == null)
            {
                process_3008 = new TickItem(onUpdate_3008);
                TickMgr.instance.addTick(process_3008);
            }
            m_cur3008_time = 0;
        }
        public void removeSkill_30081()
        {
            if (m_SFX1 != null)
            {
                GameObject.Destroy(m_SFX1);
                m_cur3008_time = 0;
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
}
