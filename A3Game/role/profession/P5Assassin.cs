using System;
using UnityEngine;
using System.Collections;
using MuGame;
using Cross;

public class P5Assassin : ProfessionRole
{
    static public GameObject ASSASSIN_SFX1, ASSASSIN_SFX2, ASSASSIN_SFX3;
    public void Init(string name, int layer, Vector3 pos, bool isUser = false, Variant serverData = null , uint cid = 0 )
    {
        m_ePRProfession = A3_PROFESSION.Assassin;
        m_strAvatarPath = "profession_assa_";
        m_strEquipEffPath = "Fx_armourFX_assa_";
        roleName = name;

        //   m_fNavSpeed = 6f;

        base.Init("profession_assa_inst", layer, pos, isUser, serverData , cid);

        setNavLay(NavmeshUtils.listARE[1]);

        P5Assassin_Event assassin_event = m_curModel.gameObject.AddComponent<P5Assassin_Event>();
        assassin_event.m_linkProfessionRole = this;

        m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

        //wing = 1;
        set_weaponr(0, 0);
        set_weaponl(0, 0);
        set_body(0, 0);

        m_skill5010_time = Skill_a3Model.getInstance().skilldic[5010].eff_last;
        m_skill5005_time = Skill_a3Model.getInstance().skilldic[5005].eff_last;
    }

    public override void PlaySkill(int id)
    {
        if (m_curSkillId != 0)
            return;

            base.PlaySkill( id );

        if (5005 == id && getShowSkillEff() != 2)
        {
            runSkill_5005();
        }

        if (5010 == id && getShowSkillEff() != 2)
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
        else
        {
            //m_curModel.gameObject.GetComponent<P5Assassin_Event>().SFX_5003_hide();
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

        }

        if (5006 == id || 5005 == id || 5010 == id)
        {
            m_fAttackCount = 0.5f;
        }
    }


    public override void StartMove(float joy_x, float joy_y)
    {
        //debug.Log("战士的旋风斩特殊处理" + m_curAni.GetInteger(EnumAni.ANI_I_SKILL));
        if (!canMove)
            return;
        moving = true;
        if (5003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) //战士的旋风斩特殊处理
        {
            float distance = 0.06f;
            float next_x = (SceneCamera.m_right.x * joy_x + SceneCamera.m_forward.x * joy_y) * distance;
            float next_y = (SceneCamera.m_right.y * joy_x + SceneCamera.m_forward.y * joy_y) * distance;

            Vector3 vdir = new Vector3(next_x, 0, next_y);

            Vector3 pos = m_curModel.position + vdir;
            m_curModel.position = pos;
        }
        else
        {
            if (m_fSkillShowTime > 0f) return;

            float next_x = (SceneCamera.m_right.x * joy_x + SceneCamera.m_forward.x * joy_y);
            float next_y = (SceneCamera.m_right.y * joy_x + SceneCamera.m_forward.y * joy_y);

            Vector3 vdir = new Vector3(next_x, 0, next_y);

            if ( m_proAvatar.rideGo != null ) { 

                //m_curModel.transform.forward = vdir;

                //m_curModel.transform.position = m_curModel.transform.position + m_curModel.transform.forward*0.3f;

                m_proAvatar.rideGo.GetComponent<Animator>().SetBool( EnumAni.ANI_T_RIDERUN , true );

            }

            m_curModel.forward = vdir;

            m_curAni.SetBool( EnumAni.ANI_RUN , true );


        }
    }

    //杀意技能特殊处理
    float m_skill5005_time = 60;
    public void runSkill_5005()
    {
        GameObject fx_inst = GameObject.Instantiate(ASSASSIN_SFX2) as GameObject;
        fx_inst.transform.SetParent(m_curModel, false);
        GameObject.Destroy(fx_inst, m_skill5005_time);
    }

    float m_skill5010_time = 60;
    //淬毒技能特殊处理
    public void runSkill_5010()
    {
        GameObject fx_inst1 = GameObject.Instantiate(ASSASSIN_SFX3) as GameObject;
        GameObject fx_inst2 = GameObject.Instantiate(ASSASSIN_SFX3) as GameObject;
        fx_inst1.transform.SetParent(m_curModel.transform.FindChild("Weapon_L"), false);
        fx_inst2.transform.SetParent(m_curModel.transform.FindChild("Weapon_R"), false);
        GameObject.Destroy(fx_inst1, m_skill5010_time);
        GameObject.Destroy(fx_inst2, m_skill5010_time);
    }
}

