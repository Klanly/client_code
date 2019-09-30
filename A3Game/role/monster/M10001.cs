using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//地狱召唤兽
public class M10001 : MonsterRole
{
    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_fNavStoppingDis = 2f;
     //   m_fNavSpeed = 6f;
        base.Init(prefab_path, layer, pos, roatate);

        maxHp = curhp = 400;
    }

    override protected void Model_Loaded_Over()
    {
        M10001_Dyzhs_Event mhr = m_curModel.gameObject.AddComponent<M10001_Dyzhs_Event>();
        mhr.m_monRole = this;

        
    }
}
