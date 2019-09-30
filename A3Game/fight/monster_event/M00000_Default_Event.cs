using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

public class M00000_Default_Event : Monster_Base_Event
{
    //自身特效
    private void onSFX(int id)
    {
        SceneFXMgr.Instantiate("FX_monsterSFX_com_FX_monster_" + id.ToString(), transform.position, transform.rotation, 6f);
    }


    private void Bullet_1()
    {
        //GameObject bult = GameObject.Instantiate(obj_prefab, pos, transform.rotation) as GameObject;
        //HitData hd = bult.gameObject.AddComponent<HitData>();
        //hd.m_vBornerPos = m_monRole.m_curModel.position;
        //hd.m_ePK_Type = PK_TYPE.PK_LEGION;
        //hd.m_unPK_Param = m_monRole.m_unLegionID;
        //hd.m_nDamage = 100;
        //hd.m_nHitType = 0;

        //bult.rigidbody.AddForce(transform.forward * 200f);
        //bult.layer = EnumLayer.LM_BT_FIGHT;
        //GameObject.Destroy(bult, 0.125f);
    }

    private void Bullet_2()
    {
        ////主角脚底锁定技能

        //Vector3 pos = SelfRole._inst.m_curModel.position;

        //GameObject bult = GameObject.Instantiate(obj_prefab, pos, transform.rotation) as GameObject;
        //HitData hd = bult.gameObject.AddComponent<HitData>();
        //hd.m_vBornerPos = m_monRole.m_curModel.position;
        //hd.m_ePK_Type = PK_TYPE.PK_LEGION;
        //hd.m_unPK_Param = m_monRole.m_unLegionID;
        //hd.m_nDamage = 100;
        //hd.m_nHitType = 0;


        //bult.layer = EnumLayer.LM_BT_FIGHT;
        //GameObject.Destroy(bult, 0.125f);
        //MapEffMgr.getInstance().addEffItem("m_10001_attack", "m_10001_attack", MapEffMgr.TYPE_AUTO, m_monRole.m_curModel);


    }

    public void onBullet(int type)
    {
        if (1 == type)
        {
            //普通攻击技能
            Vector3 pos = transform.position + transform.forward * 2f;
            pos.y += 1f;
            if (m_monRole.isfake)
            {
                HitData hd = m_monRole.BuildBullet(1,2f, SceneTFX.m_Bullet_Prefabs[1],pos,transform.rotation);
            }
        }
    }
}
