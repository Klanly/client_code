using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//默认怪，找不到怪物类时候的默认逻辑怪
public class M00000 : MonsterRole
{
    public bool isCollect = false;
    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_fNavStoppingDis = 2f;
        base.Init(prefab_path, layer, pos, roatate);
        maxHp = curhp = 1000;
    }

    override protected void Model_Loaded_Over()
    {
        //Debug.LogError("M00000  模型数据加载完毕了");

        M00000_Default_Event mde = m_curModel.gameObject.AddComponent<M00000_Default_Event>();
        PlayerNameUIMgr.getInstance().show(this);
        mde.m_monRole = this;

        if (isCollect)
        {
            m_curPhy.gameObject.layer = EnumLayer.LM_COLLECT;
        }
    }
}
