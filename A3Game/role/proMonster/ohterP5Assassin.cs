using System;
using UnityEngine;
using System.Collections;
using MuGame;


namespace MuGame
{
    class ohterP5Assassin : MonsterPlayer
    {
        static public GameObject ASSASSIN_SFX1, ASSASSIN_SFX2, ASSASSIN_SFX3;
        public void Init(string name, int layer, Vector3 pos)
        {
            m_strMPAvatarPath = "profession_assa_";
            m_eMPProfession = A3_PROFESSION.Assassin;

            roleName = name;

            //   m_fNavSpeed = 6f;

            base.Init("profession_assa_inst", layer, pos);
            //P5Assassin_Event assassin_event = m_curModel.gameObject.AddComponent<P5Assassin_Event>();
            //assassin_event.ohter_linkProfessionRole = this;
            M0x000_Role_Event mde = m_curModel.gameObject.AddComponent<M0x000_Role_Event>();
            mde.m_monRole = this;
            MonHurtPoint mhtt = m_curPhy.gameObject.AddComponent<MonHurtPoint>();
            mhtt.m_monRole = this;
            setNavLay(NavmeshUtils.listARE[1]);

            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

            //wing = 1;
            set_weaponr(0, 0);
            set_weaponl(0, 0);
            set_body(0, 0);
        }

        public override void PlaySkill(int id)
        {
            //if (m_curSkillId != 0)
            //    return;

            if (5005 == id)
            {
                runSkill_5005();
            }

            if (5010 == id)
            {
                runSkill_5010();
            }

            //if (m_isMain && 5008 == id) //隐身技能
            //{
            //    m_proAvatar.push_fx(1);
            //    m_bHide_state = true;
            //    if (SelfRole.s_bStandaloneScene)
            //        m_fHideTime = 6f;
            //    return;
            //}

            if (5003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL))
            {
                return;
            }



            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);
            m_curSkillId = id;
            if (m_isMain)
                m_moveAgent.avoidancePriority = 50;


            m_fAttackCount = 1.0f;


            if (5003 == id)
            {
                //旋风斩特殊处理
                m_fAttackCount = 2.5f;

                //其他玩家的处理，特殊处理
                if (!m_isMain)
                {
                    m_moveAgent.updateRotation = true;
                    m_moveAgent.updatePosition = true;
                    m_fSkillShowTime = 2.5f;
                    m_moveAgent.speed = 5f;
                }

                GameObject fx_inst = GameObject.Instantiate(ASSASSIN_SFX1) as GameObject;
                GameObject.Destroy(fx_inst, 2.5f);

                fx_inst.transform.SetParent(m_curModel, false);
            }

            if (5006 == id || 5005 == id || 5010 == id)
            {
                m_fAttackCount = 0.5f;
            }
        }

        //杀意技能特殊处理
        public void runSkill_5005()
        {
            GameObject fx_inst = GameObject.Instantiate(ASSASSIN_SFX2) as GameObject;
            fx_inst.transform.SetParent(m_curModel, false);
            GameObject.Destroy(fx_inst, 10);
        }

        //淬毒技能特殊处理
        public void runSkill_5010()
        {
            GameObject fx_inst1 = GameObject.Instantiate(ASSASSIN_SFX3) as GameObject;
            GameObject fx_inst2 = GameObject.Instantiate(ASSASSIN_SFX3) as GameObject;
            fx_inst1.transform.SetParent(m_curModel.transform.FindChild("Weapon_L"), false);
            fx_inst2.transform.SetParent(m_curModel.transform.FindChild("Weapon_R"), false);
            GameObject.Destroy(fx_inst1, 60);
            GameObject.Destroy(fx_inst2, 60);
        }
    }
}
