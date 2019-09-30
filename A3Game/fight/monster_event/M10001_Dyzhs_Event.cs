using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//地狱召唤兽
public class M10001_Dyzhs_Event : Monster_Base_Event
{
    //自身特效
    private void onSFX(int id)
    {
        SceneFXMgr.Instantiate("FX_monsterSFX_10001_SFX_" + id.ToString(), transform.position, transform.rotation, 2f);
    }

    private void Bullet_1()
    {
        //普通攻击技能
        Vector3 pos = transform.position + transform.forward * 2f;
        pos.y += 1f;

        if (m_monRole.isfake)
        {
            HitData hd = m_monRole.BuildBullet(1, 0.125f, SceneTFX.m_Bullet_Prefabs[1], pos, transform.rotation);
            hd.m_nDamage = 1;
        }

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


    }

    public void onBullet(int type)
    {
        switch (type)
        {
            case 1: Bullet_1(); break;
            case 2: Bullet_2(); break;
            default: debug.Log("未知的子弹类型：" + type); break;
        }
    }
}
