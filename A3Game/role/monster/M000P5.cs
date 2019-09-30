using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//刺客外观和技能的怪物
public class M000P5 : MonsterRole
{
    ProfessionAvatar m_proAvatar = new ProfessionAvatar();

    private string[] lFirstName;
	private string[] lLastName0;
	private string[] lLastName1;
	public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0) {

		m_fNavStoppingDis = 2f;
		roleName = RandomName();
		base.Init(prefab_path, layer, pos, roatate);

        maxHp = curhp = 1000;
    }

    override protected void Model_Loaded_Over()
    {
        M0x000_Role_Event mde = m_curModel.gameObject.AddComponent<M0x000_Role_Event>();
        mde.m_monRole = this;
        

        SetSkin();
    }

    public override void FrameMove(float delta_time)
    {
        base.FrameMove(delta_time);
        m_proAvatar.FrameMove();
    }

    public override void dispose()
    {
        base.dispose();
        m_proAvatar.dispose();
        m_proAvatar = null;
    }

    public override void PlaySkill(int id) {
		if (m_curSkillId != 0)
			return;

		if (id == 1) {
			id = 5001;
		}

		if (5005 == id) {
			runSkill_5005();
		}

		if (5010 == id) {
			runSkill_5010();
		}

		if (5003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) {
			return;
		}



		m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);
		m_curSkillId = id;
		if (m_isMain)
			m_moveAgent.avoidancePriority = 50;

		if (5003 == id) {
			//旋风斩特殊处理
			m_fAttackCount = 2.5f;

			//其他玩家的处理，特殊处理
			if (!m_isMain) {
				m_moveAgent.updateRotation = true;
				m_moveAgent.updatePosition = true;
				m_fSkillShowTime = 2.5f;
				m_moveAgent.speed = 5f;
			}

			GameObject fx_inst = GameObject.Instantiate(P5Assassin.ASSASSIN_SFX1) as GameObject;
			GameObject.Destroy(fx_inst, 2.5f);

			fx_inst.transform.SetParent(m_curModel, false);
		}
		else {
			m_fAttackCount = 1.5f;
		}
	}

	//杀意技能特殊处理
	public void runSkill_5005() {
		GameObject fx_inst = GameObject.Instantiate(P5Assassin.ASSASSIN_SFX2) as GameObject;
		fx_inst.transform.SetParent(m_curModel, false);
		GameObject.Destroy(fx_inst, 10);
	}

	//淬毒技能特殊处理
	public void runSkill_5010() {
		GameObject fx_inst1 = GameObject.Instantiate(P5Assassin.ASSASSIN_SFX3) as GameObject;
		GameObject fx_inst2 = GameObject.Instantiate(P5Assassin.ASSASSIN_SFX3) as GameObject;
		fx_inst1.transform.SetParent(m_curModel.transform.FindChild("Weapon_L"), false);
		fx_inst2.transform.SetParent(m_curModel.transform.FindChild("Weapon_R"), false);
		GameObject.Destroy(fx_inst1, 60);
		GameObject.Destroy(fx_inst2, 60);
	}

	public void SetSkin() {
		SXML itemsXMl = XMLMgr.instance.GetSXML("mlzd");
		int lvln = SelfRole._inst.zhuan * 10 + SelfRole._inst.lvl;
		int waiguan = 2;
		int a_w_b_c = 3;
		var vv = itemsXMl.GetNodeList("stage");

		foreach (var v in vv) {
			string ss = v.getString("lvl");
			int a = int.Parse(ss.Split(',')[0]);
			int b = int.Parse(ss.Split(',')[1]);
			if (lvln <= a * 10 + b) {
				waiguan = v.getInt("waiguan");
				break;
			}
		}

		GameObject m_Obj = this.m_curGameObj;
		string m_strAvatarPath = "";
		m_strAvatarPath = "profession_assa_";

		foreach (Transform tran in m_Obj.GetComponentsInChildren<Transform>()) {
			tran.gameObject.layer = EnumLayer.LM_SELFROLE;// 更改物体的Layer层
		}

		Transform cur_model = m_Obj.transform.FindChild("model");

		m_proAvatar.Init_PA(A3_PROFESSION.Assassin, m_strAvatarPath, "l_", m_curGameObj.layer, EnumMaterial.EMT_EQUIP_L, m_curModel);

		m_proAvatar.set_body(a_w_b_c * 100 + waiguan * 10 + 3, 0);
		m_proAvatar.set_weaponl(a_w_b_c * 100 + waiguan * 10 + 6, 0);
		m_proAvatar.set_weaponr(a_w_b_c * 100 + waiguan * 10 + 6, 0);
		m_proAvatar.set_wing(0, 0);
		//m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
		//m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());

		m_curPhy = m_curModel.transform.FindChild("physics");
		try {
			m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
		} catch (System.Exception ex) {

		}
	}

	private string RandomName() {
		if (lFirstName == null) {
			lFirstName = XMLMgr.instance.GetSXML("comm.ranName.firstName").getString("value").Split(',');
			lLastName0 = XMLMgr.instance.GetSXML("comm.ranName.lastName", "sex==0").getString("value").Split(',');
			lLastName1 = XMLMgr.instance.GetSXML("comm.ranName.lastName", "sex==1").getString("value").Split(',');
		}

		System.Random r = new System.Random();
		int first = r.Next(0, lFirstName.Length);
		int last = r.Next(0, lLastName0.Length);
		return lFirstName[first] + lFirstName[last];
	}
}
