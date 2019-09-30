using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using Cross;

class MS0000 : MonsterRole
{
    public int owner_cid = 0;
    //private bool _invisible= false; //处理技能隐身

    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_fNavStoppingDis = 2f;

        base.Init(prefab_path, layer, pos, roatate);

        maxHp = curhp = 1000;
    }

    override protected void Model_Loaded_Over()
    {
        MS0000_Default_Event mde = m_curModel.gameObject.AddComponent<MS0000_Default_Event>();
        mde.m_monRole = this;

        Variant dd = new Variant();
        dd["cur"] = 100;
        dd["max"] = 100;

        PlayerNameUIMgr.getInstance().refreshHp(this, dd);

        //if ( base.masterid == PlayerModel.getInstance().cid )
        //{
        //    this.invisibleState = SelfRole._inst.invisible;

        //}
        //else {

        //    foreach ( var otherrole in OtherPlayerMgr._inst.m_mapOtherPlayer )
        //    {
        //        if ( base.masterid ==  otherrole.Value.m_unCID)
        //        {
        //            this.invisibleState = otherrole.Value.invisible;
        //        }
        //    }

        //}
       

    }

    public bool ismapEffect = false;
    public Vector3 effectVec;
    public override void PlaySkill(int id)
    {
        debug.Log("PlaySkill:   " + id);
        //flytxt.instance.fly(id.ToString ());

        MS0000_Default_Event mde = m_curModel.gameObject.GetComponent<MS0000_Default_Event>();
        mde.effect = null;
        if (id != 1)
        {
            if (ismapEffect)
            {
                mde.vec = effectVec;
                ismapEffect = false;
            }
            // mde.onSFX_EFF(id.ToString());         
            mde.effect = id.ToString();
        }
        else
        {
            selfFx(id);
        }


        int play_skill;
        if (id == 1)
            play_skill = 1;
        else
        {
            SXML sxml = XMLMgr.instance.GetSXML("skill.skill", "id==" + id);
            if (sxml != null)
            {
                play_skill = sxml.getInt("action_tp");
                flyskill(sxml.getString("name"));
            }
            else
                play_skill = 1;
        }
        m_fAttackCount = 0.5f;
        if (play_skill == 1 )
        {
            OtherSkillShow();
            EnterAttackState();
        }
        else 
        {
            m_fSkillShowTime = 5.0f;
            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, play_skill);
            m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
        }

        //selfFx(play_skill);
    }

    
    //召唤兽普通攻击特效
    void selfFx(int id)
    {
        MS0000_Default_Event mde = m_curModel.gameObject.GetComponent<MS0000_Default_Event>();
        SXML sxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + summonid);
        if (sxml != null && sxml.getString("skillid_1") != "")
            // mde.onSFX_EFF(sxml.getString("skillid_"+id));
            mde.effect = sxml.getString("skillid_" + id);
    }

    //public bool invisibleState
    //{

    //    set
    //    {

    //        _invisible = value;

    //        if ( base.masterid == PlayerModel.getInstance().cid || owner_cid  == PlayerModel.getInstance().cid )
    //        {
    //            if ( m_MonBody && _invisible == true )
    //            {
    //                m_MonBody.material = EnumMaterial.EMT_SKILL_HIDE;

    //            }

    //            else if ( m_MonBody!= null && body_m_mat != null &&  _invisible == false )
    //            {

    //                m_MonBody.material = body_m_mat;
    //            }

    //        }

    //        else
    //        {

    //            if ( m_curModel == null )
    //            {
    //                return;
    //            }

    //            var layer =   _invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_MONSTER;

    //            layer =  SceneCamera.m_nModelDetail_Level != 1 ? EnumLayer.LM_DEFAULT : layer;

    //            if ( _invisible == true )
    //            {
    //                foreach ( var item in m_curModel.gameObject.GetComponentsInChildren<Transform>() )
    //                {
    //                    item.gameObject.layer = layer;
    //                }

    //                PlayerNameUIMgr.getInstance().hide( this );
    //            }
    //            else
    //            {

    //                foreach ( var item in m_curModel.gameObject.GetComponentsInChildren<Transform>() )
    //                {
    //                    item.gameObject.layer = layer;
    //                }

    //                PlayerNameUIMgr.getInstance().show( this );
    //                PlayerNameUIMgr.getInstance().setName( this , this.roleName , this.ownerName + ContMgr.getCont( "MonsterMgr" ) );

    //            }


    //        }



    //    }

    //    get
    //    {

    //        return _invisible;

    //    }

    //}

}