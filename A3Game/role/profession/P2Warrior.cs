using System;
using UnityEngine;
using System.Collections;
using MuGame;
using Cross;

public class P2Warrior : ProfessionRole
{
    static public GameObject WARRIOR_SFX1, WARRIOR_SFX2, WARRIOR_SFX3, WARRIOR_SFX4,WARRIOR_SFX5;
    GameObject m_SFX1,m_SFX2;
    public void Init(string name, int layer, Vector3 pos, bool isUser = false , Variant serverData = null , uint cid = 0 )
    {
        m_ePRProfession = A3_PROFESSION.Warrior;
        m_strAvatarPath = "profession_warrior_";
        m_strEquipEffPath = "Fx_armourFX_warrior_";
        //roleName = "我是战士";
        roleName = name;

    //    m_fNavSpeed = 2f;
        m_fNavStoppingDis =0.125f;
        base.Init("profession_warrior_inst", layer, pos, isUser, serverData , cid);

        setNavLay(NavmeshUtils.listARE[1]);

        P2Warrior_Event warrior_event = m_curModel.gameObject.AddComponent<P2Warrior_Event>();
        warrior_event.m_linkProfessionRole = this;

        m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);

        //wing = 1;
        set_body(0, 0); //body = 0;
        set_weaponr(0, 0);

        m_skill2005_time = Skill_a3Model.getInstance().skilldic[2005].eff_last;
        m_skill2010_time = Skill_a3Model.getInstance().skilldic[2010].eff_last;
    }


    public override void PlaySkill(int id)
    {   
        if (m_curSkillId != 0)
            return;

        base.PlaySkill( id );

        if (m_isMain)
            m_moveAgent.avoidancePriority = 50;


        m_curSkillId = id;

        if (2003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) //战士的旋风斩特殊处理
        {
            return;
        }
        else
        {
            //m_curModel.gameObject.GetComponent<P2Warrior_Event>().SFX_2003_hide();
        }

        if (2005 == id && getShowSkillEff() != 2)
        {
            runSkill_2005();
        }
        if(2010 == id && getShowSkillEff() != 2)
        {
            runSkill_2010();
        }
        if (2009 == id && getShowSkillEff() != 2)
        {
            runSkill_2009();
        }
        if(id == 2006 && getShowSkillEff() != 2)
        {
            runSkill_2006();
        }

        //if (id == 5)
        //{
        //    if (m_testAvatar % 7 == 0) body = 0;
        //    if (m_testAvatar % 7 == 1) weapon_r = 0;
        //    if (m_testAvatar % 7 == 2) wing = 2;

        //    if (m_testAvatar % 7 == 3) body = 0;
        //    if (m_testAvatar % 7 == 4) weapon_r = 0;
        //    if (m_testAvatar % 7 == 5) wing = 0;
        //    if (m_testAvatar % 7 == 6) wing = 1;

        //    m_testAvatar++;
        //    return;
        //}

        //if (id == 4)
        //{
        //    if (m_testPro == 1 && SelfRole.s_bStandaloneScene)
        //    {
        //        debug.Log("成功过关");
        //        PlayerModel.getInstance().LeaveStandalone_CreateChar();
        //    }

        //    m_testPro++;
        //    return;
        //}



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

        }
        else
        {
            m_fAttackCount = 1.0f;
        }
    }

    public override void StartMove(float joy_x, float joy_y)
    {
        //debug.Log("战士的旋风斩特殊处理" + m_curAni.GetInteger(EnumAni.ANI_I_SKILL));
        if (!canMove)
            return;

        moving = true;
        if (2003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) //战士的旋风斩特殊处理
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
            //玩家控制的普通的移动，需要技能动作完之后再做处理
            if (m_fSkillShowTime > 0f) return;

            float next_x = (SceneCamera.m_right.x * joy_x + SceneCamera.m_forward.x * joy_y);
            float next_y = (SceneCamera.m_right.y * joy_x + SceneCamera.m_forward.y * joy_y);

            Vector3 vdir = new Vector3(next_x, 0, next_y);
            m_curModel.forward = vdir;

            if ( m_proAvatar.rideGo != null )
            {
                m_proAvatar.rideGo.GetComponent<Animator>().SetBool( EnumAni.ANI_T_RIDERUN , true );
            }

            m_curAni.SetBool(EnumAni.ANI_RUN, true);
        }
    }

    //毁灭伤害 技能特殊处理
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
