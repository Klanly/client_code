using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//食人鬼
public class M10002 : MonsterRole
{
    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_fNavStoppingDis = 1.5f;
     //   m_fNavSpeed = 8f;

     
        base.Init(prefab_path, layer, pos,roatate);

        maxHp = curhp = 200;
    }

    override protected void Model_Loaded_Over()
    {
        M10002_Srg_Event mhr = m_curModel.gameObject.AddComponent<M10002_Srg_Event>();
        mhr.m_monRole = this;
        
    }
}
