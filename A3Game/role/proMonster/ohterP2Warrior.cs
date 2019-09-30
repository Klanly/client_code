using UnityEngine;
using System.Collections;
using MuGame;

namespace MuGame
{
    class ohterP2Warrior : MonsterPlayer
    {
        static public GameObject WARRIOR_SFX1, WARRIOR_SFX2, WARRIOR_SFX3, WARRIOR_SFX4, WARRIOR_SFX5;
        GameObject m_SFX1, m_SFX2;
        public void Init(string name, int layer, Vector3 pos)
        {
            m_strMPAvatarPath = "profession_warrior_";
            m_eMPProfession = A3_PROFESSION.Warrior;
            //roleName = "我是战士";
            roleName = name;

            //    m_fNavSpeed = 2f;
            m_fNavStoppingDis = 0.125f;
            base.Init("profession_warrior_inst", layer, pos);
            //P2Warrior_Event warrior_event = m_curModel.gameObject.AddComponent<P2Warrior_Event>();
            //warrior_event.ohter_linkProfessionRole = this;
            M0x000_Role_Event mde = m_curModel.gameObject.AddComponent<M0x000_Role_Event>();
            mde.m_monRole = this;
            MonHurtPoint mhtt = m_curPhy.gameObject.AddComponent<MonHurtPoint>();
            mhtt.m_monRole = this;
            setNavLay(NavmeshUtils.listARE[1]);

            //P2Warrior_Event warrior_event = m_curModel.gameObject.AddComponent<P2Warrior_Event>();
            //warrior_event.m_linkProfessionRole = this;

            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

            //wing = 1;
            set_body(1, 1); //body = 0;
            set_weaponr(1, 1);
        }


        public override void PlaySkill(int id)
        {
            //if (m_curSkillId != 0)
            //    return;

            if (m_isMain)
                m_moveAgent.avoidancePriority = 50;


            m_curSkillId = id;
            if (2003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) //战士的旋风斩特殊处理
            {
                return;
            }

            if (2005 == id)
            {
                runSkill_2005();
            }
            if (2010 == id)
            {
                runSkill_2010();
            }
            if (2009 == id)
            {
                runSkill_2009();
            }
            if (id == 2006)
            {
                runSkill_2006();
            }

            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);


            if (2003 == id)
            {
                //旋风斩特殊处理

                m_fAttackCount = 3.5f;

                //其他玩家的处理，特殊处理
                if (m_moveAgent)
                {
                    m_moveAgent.updateRotation = true;
                    m_moveAgent.updatePosition = true;
                    m_fSkillShowTime = 3.5f;
                    m_moveAgent.speed = 3f;
                }

                GameObject fx_inst = GameObject.Instantiate(WARRIOR_SFX1) as GameObject;
                GameObject.Destroy(fx_inst, 3.5f);

                fx_inst.transform.SetParent(m_curModel, false);
            }
            else
            {
                m_fAttackCount = 1.0f;
            }
        }
        TickItem process_2005;
        float m_skill2005_time = 10;  //持续时间
        float m_cur2005_time = 0;
        public void runSkill_2005()
        {
            if (m_SFX1 == null)
            {
                m_SFX1 = GameObject.Instantiate(WARRIOR_SFX2) as GameObject;
                m_SFX1.transform.SetParent(m_curModel, false);
                m_SFX1.SetActive(false);
            }

            if (process_2005 == null)
            {
                process_2005 = new TickItem(onUpdate_2005);
                TickMgr.instance.addTick(process_2005);
            }
            m_cur2005_time = 0;
        }
        void onUpdate_2005(float s)
        {
            //跨地图时m_curModel==null
            if (m_curModel == null)
            {
                TickMgr.instance.removeTick(process_2005);
                return;
            }


            m_cur2005_time += s;
            if (m_cur2005_time > 1.0f)
            {
                m_SFX1.SetActive(true);
            }
            if (m_cur2005_time > m_skill2005_time)
            {
                GameObject.Destroy(m_SFX1);
                m_cur2005_time = 0;
                TickMgr.instance.removeTick(process_2005);
                process_2005 = null;
                m_SFX1 = null;
            }
        }

        //buff技能特殊处理
        TickItem process_2010;
        float m_skill2010_time = 10;  //持续时间
        float m_cur2010_time = 0;
        public void runSkill_2010()
        {
            if (m_SFX2 == null)
            {
                m_SFX2 = GameObject.Instantiate(WARRIOR_SFX2) as GameObject;
                m_SFX2.transform.SetParent(m_curModel, false);
                m_SFX2.SetActive(false);
            }

            if (process_2010 == null)
            {
                process_2010 = new TickItem(onUpdate_2010);
                TickMgr.instance.addTick(process_2010);
            }
            m_cur2010_time = 0;
        }
        void onUpdate_2010(float s)
        {
            //跨地图时m_curModel==null
            if (m_curModel == null)
            {
                TickMgr.instance.removeTick(process_2010);
                return;
            }


            m_cur2010_time += s;
            if (m_cur2010_time > 1.0f)
            {
                m_SFX2.SetActive(true);
            }
            if (m_cur2010_time > m_skill2010_time)
            {
                GameObject.Destroy(m_SFX2);
                m_cur2010_time = 0;
                TickMgr.instance.removeTick(process_2010);
                process_2010 = null;
                m_SFX2 = null;
            }
        }

        //跳斩技能特殊处理
        public void runSkill_2009()
        {
            GameObject fx_inst = GameObject.Instantiate(WARRIOR_SFX3) as GameObject;
            GameObject.Destroy(fx_inst, 3.5f);
            fx_inst.transform.SetParent(m_curModel, false);

            GameObject fx_inst_1 = GameObject.Instantiate(WARRIOR_SFX4) as GameObject;
            GameObject.Destroy(fx_inst_1, 3.5f);
            if (m_curModel.FindChild("Spine") != null)
                fx_inst_1.transform.SetParent(m_curModel.FindChild("Spine"), false);
        }
        public void runSkill_2006()
        {
            GameObject fx_inst = GameObject.Instantiate(WARRIOR_SFX5) as GameObject;
            GameObject.Destroy(fx_inst, 2f);
            fx_inst.transform.SetParent(m_curModel, false);
        }
    }
}
