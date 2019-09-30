using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//战士外观和技能的怪物
public class M000P2 : MonsterRole
{
    ProfessionAvatar m_proAvatar = new ProfessionAvatar();

    GameObject m_SFX1, m_SFX2;
	private string[] lFirstName;
	private string[] lLastName0;
	private string[] lLastName1;
	public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0) {
		roleName = RandomName();

		m_fNavStoppingDis = 2f;

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
			id = 2001;
		}

		if (m_isMain)
			m_moveAgent.avoidancePriority = 50;


		m_curSkillId = id;
		if (2003 == m_curAni.GetInteger(EnumAni.ANI_I_SKILL)) //战士的旋风斩特殊处理
        {
			return;
		}

		if (2005 == id) {
			runSkill_2005();
		}
		if (2010 == id) {
			runSkill_2010();
		}
		if (2009 == id) {
			runSkill_2009();
		}

		m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);


		if (2003 == id) {
			//旋风斩特殊处理

			m_fAttackCount = 3.5f;

			//其他玩家的处理，特殊处理
			if (m_moveAgent) {
				m_moveAgent.updateRotation = true;
				m_moveAgent.updatePosition = true;
				m_fSkillShowTime = 3.5f;
				m_moveAgent.speed = 3f;
			}

			GameObject fx_inst = GameObject.Instantiate(P2Warrior.WARRIOR_SFX1) as GameObject;
			GameObject.Destroy(fx_inst, 3.5f);

			fx_inst.transform.SetParent(m_curModel, false);
		}
		else {
			m_fAttackCount = 1.5f;
		}
	}

	//毁灭伤害 技能特殊处理
	TickItem process_2005;
	float m_skill2005_time = 10;  //持续时间
	float m_cur2005_time = 0;
	public void runSkill_2005() {
		if (m_SFX1 == null) {
			m_SFX1 = GameObject.Instantiate(P2Warrior.WARRIOR_SFX2) as GameObject;
			m_SFX1.transform.SetParent(m_curModel, false);
			m_SFX1.SetActive(false);
		}

		if (process_2005 == null) {
			process_2005 = new TickItem(onUpdate_2005);
			TickMgr.instance.addTick(process_2005);
		}
		m_cur2005_time = 0;
	}
	void onUpdate_2005(float s) {
		//跨地图时m_curModel==null
		if (m_curModel == null) {
			TickMgr.instance.removeTick(process_2005);
			return;
		}


		m_cur2005_time += s;
		if (m_cur2005_time > 1.0f) {
			m_SFX1.SetActive(true);
		}
		if (m_cur2005_time > m_skill2005_time) {
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
	public void runSkill_2010() {
		if (m_SFX2 == null) {
			m_SFX2 = GameObject.Instantiate(P2Warrior.WARRIOR_SFX2) as GameObject;
			m_SFX2.transform.SetParent(m_curModel, false);
			m_SFX2.SetActive(false);
		}

		if (process_2010 == null) {
			process_2010 = new TickItem(onUpdate_2010);
			TickMgr.instance.addTick(process_2010);
		}
		m_cur2010_time = 0;
	}
	void onUpdate_2010(float s) {
		//跨地图时m_curModel==null
		if (m_curModel == null) {
			TickMgr.instance.removeTick(process_2010);
			return;
		}


		m_cur2010_time += s;
		if (m_cur2010_time > 1.0f) {
			m_SFX2.SetActive(true);
		}
		if (m_cur2010_time > m_skill2010_time) {
			GameObject.Destroy(m_SFX2);
			m_cur2010_time = 0;
			TickMgr.instance.removeTick(process_2010);
			process_2010 = null;
			m_SFX2 = null;
		}
	}

	//跳斩技能特殊处理
	public void runSkill_2009() {
		GameObject fx_inst = GameObject.Instantiate(P2Warrior.WARRIOR_SFX3) as GameObject;
		GameObject.Destroy(fx_inst, 3.5f);
		fx_inst.transform.SetParent(m_curModel, false);

		GameObject fx_inst_1 = GameObject.Instantiate(P2Warrior.WARRIOR_SFX4) as GameObject;
		GameObject.Destroy(fx_inst_1, 3.5f);
		if (m_curModel.FindChild("Spine") != null)
			fx_inst_1.transform.SetParent(m_curModel.FindChild("Spine"), false);
	}

	public void SetSkin() {
		SXML itemsXMl = XMLMgr.instance.GetSXML("mlzd");
		int lvln = SelfRole._inst.zhuan * 10 + SelfRole._inst.lvl;
		int waiguan = 2;
		int a_w_b_c = 1;
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
		m_strAvatarPath = "profession_warrior_";

		foreach (Transform tran in m_Obj.GetComponentsInChildren<Transform>()) {
			tran.gameObject.layer = EnumLayer.LM_SELFROLE;// 更改物体的Layer层
		}

		Transform cur_model = m_Obj.transform.FindChild("model");

		m_proAvatar.Init_PA(A3_PROFESSION.Warrior, m_strAvatarPath, "l_", m_curGameObj.layer, EnumMaterial.EMT_EQUIP_L, m_curModel);

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
