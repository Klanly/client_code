using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using Cross;

class MDC000 : MonsterRole
{
    public string escort_name = string.Empty;
    Animator ani = new Animator();
    bool show = false;
    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        base.Init(prefab_path, layer, pos, roatate);
    }

    override protected void Model_Loaded_Over()
    {
        M00000_Default_Event mde = m_curModel.gameObject.AddComponent<M00000_Default_Event>();
        mde.m_monRole = this;
        ani = m_curModel.gameObject.GetComponent<Animator>();
        AnimatorStateInfo anima = ani.GetCurrentAnimatorStateInfo(0);
        a3_dartproxy.getInstance().addEventListener(a3_dartproxy.DARTHPNOW, PlayerNameUIMgr.getInstance().carinfo);
    }

    public override void FrameMove(float delta_time)
    {
        base.FrameMove(delta_time);
        if (!show)
        {
            SXML xml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + dartid);
            ani.speed = xml.GetNodeList("att")[0].getFloat("speed_run");
            show = true;
        }

    }
}
