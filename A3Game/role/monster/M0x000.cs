using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;


//暂时用单机表现，到时联调服务器
public class M0x000 : MonsterRole
{
	public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0) {

		m_fNavStoppingDis = 2f;

		base.Init(prefab_path, layer, pos, roatate);

		M0x000_Role_Event mde = m_curModel.gameObject.AddComponent<M0x000_Role_Event>();
		mde.m_monRole = this;
		maxHp = curhp = 1000;

		SetSkin();
	}

    override protected void Model_Loaded_Over()
    {

    }

    override protected void EnterAttackState() //进入攻击状态
	{
		m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 2001);
		//new P2Warrior().PlaySkill(1001);
		//if (SelfRole._inst is P2Warrior) {
		//    int range = UnityEngine.Random.Range(0, 10);
		//    if (range < 3) {
		//        m_fSkillShowTime = 5.0f;
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 1);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else if (range < 5) {
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 2);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else {
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, true);
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
		//    }
		//}
		//else if (SelfRole._inst is P3Mage) {
		//    int range = UnityEngine.Random.Range(0, 10);
		//    if (range < 3) {
		//        m_fSkillShowTime = 5.0f;
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 1);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else if (range < 5) {
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 2);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else {
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, true);
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
		//    }
		//}
		//else if (SelfRole._inst is P5Assassin) {
		//    int range = UnityEngine.Random.Range(0, 10);
		//    if (range < 3) {
		//        m_fSkillShowTime = 5.0f;
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 1);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else if (range < 5) {
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 2);
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
		//    }
		//    else {
		//        m_curAni.SetBool(EnumAni.ANI_ATTACK, true);
		//        m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
		//    }
		//}
		//else {
		//    return;
		//}

		
	}

	public override void PlaySkill(int id) {
		if (id != 0) return;
		m_fAttackCount = 0.5f;
		int range = UnityEngine.Random.Range(0, 10);
		if (range < 3){ 
			m_fAttackCount = 1.5f;
		}
		if (SelfRole._inst is P2Warrior) {
			id = 2001;
			if (range>=3 &&range <= 5) id = 2002;
		}
		else if (SelfRole._inst is P3Mage) {
			id = 3001;
			if (range >= 3 && range <= 5) id = 3003;
		}
		else if (SelfRole._inst is P5Assassin) {
			id = 5001;
			if (range >= 3 && range <= 5) id = 5002;
		}

		m_curSkillId = id;
		m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);
	}

	public void SetSkin(){
		SXML itemsXMl = XMLMgr.instance.GetSXML("mlzd");
		int lvln = SelfRole._inst.zhuan * SelfRole._inst.lvl;
		int waiguan = 2;
		int a_w_b_c = 1;
		var vv = itemsXMl.GetNodeList("stage");

		foreach (var v in vv)
		{
			string ss = v.getString("lvl");
			int a = int.Parse(ss.Split(',')[0]);
			int b = int.Parse(ss.Split(',')[1]);
			if (lvln <= a * b){
				waiguan = v.getInt("waiguan");
				break;
			}
		}

		GameObject m_Obj = this.m_curModel.parent.gameObject;
		ProfessionAvatar m_proAvatar;
		string m_strAvatarPath = "";
		if (SelfRole._inst is P2Warrior) {
			m_strAvatarPath = "profession/warrior/";
			a_w_b_c = 1;
		}
		else if (SelfRole._inst is P3Mage) {
			m_strAvatarPath = "profession/mage/";
			a_w_b_c = 2;
		}
		else if (SelfRole._inst is P5Assassin) {
			m_strAvatarPath = "profession/assa/";
			a_w_b_c = 3;
		}

		foreach (Transform tran in m_Obj.GetComponentsInChildren<Transform>()) {
			tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
		}

		Transform cur_model = m_Obj.transform.FindChild("model");

		//手上的小火球
		if (SelfRole._inst.m_LockRole is P3Mage) {
			Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
			var obj_prefab = Resources.Load<GameObject>("profession/avatar_ui/mage_r_finger_fire");
			GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
			light_fire.transform.SetParent(cur_r_finger1, false);
		}

		m_proAvatar = new ProfessionAvatar();
		m_proAvatar.Init(m_strAvatarPath, "h_", EnumLayer.LM_FX, EnumMaterial.EMT_EQUIP_H, cur_model);

		m_proAvatar.set_body(a_w_b_c * 100 + waiguan*10 + 3, 0);
		m_proAvatar.set_weaponl(a_w_b_c * 100 + waiguan * 10 + 6, 0);
		m_proAvatar.set_weaponr(a_w_b_c * 100 + waiguan * 10 + 6, 0);
		m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
		m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());

		m_curPhy = m_curModel.transform.FindChild("physics");
		try {
			m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
		} catch (System.Exception ex) {

		}
	}
}
