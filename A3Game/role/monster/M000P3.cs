using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;

//法师外观和技能的怪物
public class M000P3 : MonsterRole
{
    ProfessionAvatar m_proAvatar = new ProfessionAvatar();

    GameObject m_SFX1, m_SFX2, m_SFX3;
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
			id = 3001;
		}

		if (3003 == id) {
			runSkill_3003();
		}
		if (3008 == id) {
			runSkill_3008();
		}

		if (m_isMain)
			m_moveAgent.avoidancePriority = 50;

		m_curSkillId = id;
		m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);

		m_fAttackCount = 1.0f;
	}

	TickItem process_3003;
	float m_skill3003_time = 4;  //持续时间
	float m_cur3003_time = 0;
	int m_skill3003_num = 20;   //冰锥数量
	int m_cur3003_num = 0;
	Vector3 m_3003_pos;
	Quaternion m_3003_rotation;
	void runSkill_3003() {
		if (process_3003 == null) {
			process_3003 = new TickItem(onUpdate_3003);
			TickMgr.instance.addTick(process_3003);
		}
		m_cur3003_time = 0;
		m_cur3003_num = 0;
		m_3003_pos = m_curModel.transform.position;
		m_3003_rotation = m_curModel.transform.rotation;

		if (m_SFX2 == null) {
			m_SFX2 = GameObject.Instantiate(P3Mage.P3MAGE_SFX2, m_3003_pos, m_3003_rotation) as GameObject;
			m_SFX2.transform.SetParent(U3DAPI.FX_POOL_TF, false);
		}
	}
	void onUpdate_3003(float s) {
		//跨地图时m_curModel==null
		if (m_curModel == null) {
			TickMgr.instance.removeTick(process_3003);

			process_3003 = null;
			m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
			GameObject.Destroy(m_SFX2, 2);
			m_SFX2 = null;

			return;
		}

		m_cur3003_time += s;
		float nexttime = 0.5f + m_cur3003_num * (m_skill3003_time - 1) / m_skill3003_num;
		if (m_cur3003_time > nexttime) {
			onBullet_3003(m_3003_pos, m_3003_rotation);
			m_cur3003_num++;
		}

		if (m_cur3003_time > m_skill3003_time) {
			m_cur3003_time = 0;
			m_cur3003_num = 0;
			TickMgr.instance.removeTick(process_3003);
			process_3003 = null;

			m_SFX2.transform.FindChild("f").GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
			GameObject.Destroy(m_SFX2, 2);
			m_SFX2 = null;
		}
	}
	public void onBullet_3003(Vector3 pos, Quaternion rotation) {
		//冰雹术
		pos.z += UnityEngine.Random.Range(-3f, 3f);
		pos.x += UnityEngine.Random.Range(-3f, 3f);
		pos.y = 16f;

		GameObject bult = GameObject.Instantiate(P3Mage_Event.MAGE_B3, pos, rotation) as GameObject;
		bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

		Transform real_track = bult.transform.FindChild("t");
		if (real_track != null) {
			HitData hd = Link_PRBullet(3003, 2f, bult, real_track);

			hd.m_nHurtSP_type = 11;
			hd.m_nHurtSP_pow = 1;
			hd.m_nDamage = 177;

			//hd.m_Color_Main = Color.blue;
            hd.m_Color_Main = SkillModel.getskill_mcolor(3003);

           // hd.m_Color_Rim = Color.white;
            hd.m_Color_Rim = SkillModel.getskill_rcolor(3003);
            hd.m_aniTrack = real_track.GetComponent<Animator>();
			real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

			Transform real_fx = real_track.FindChild("f");
			if (real_fx != null) {
				hd.m_aniFx = real_fx.GetComponent<Animator>();
			}
		}
	}

	public HitData Link_PRBullet(uint skillid, float t, GameObject root, Transform linker) {
		HitData hd = linker.gameObject.AddComponent<HitData>();

		hd.m_hdRootObj = root;
		hd.m_CastRole = this;
		hd.m_vBornerPos = m_curModel.position;

		hd.m_ePK_Type = m_ePK_Type;
		switch (m_ePK_Type) {
			case PK_TYPE.PK_TEAM: hd.m_unPK_Param = m_unTeamID; break;
			case PK_TYPE.PK_LEGION: hd.m_unPK_Param = m_unLegionID; break;
			case PK_TYPE.PK_PKALL: hd.m_unPK_Param = m_unCID; break;
		}

		hd.m_unSkillID = skillid;
		hd.m_nDamage = 100;
		hd.m_nHitType = 0;

		linker.gameObject.layer = EnumLayer.LM_BT_FIGHT;
		GameObject.Destroy(root, t);

		//可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
		if (m_fDisposeTime < t) m_fDisposeTime = t;

		return hd;
	}

	//护盾技能特殊处理
	TickItem process_3008;
	float m_skill3008_time = 30;  //持续时间
	float m_cur3008_time = 0;
	public void runSkill_3008() {
		if (m_SFX1 == null) {
			m_SFX1 = GameObject.Instantiate(P3Mage.P3MAGE_SFX1) as GameObject;
			m_SFX1.transform.SetParent(m_curModel, false);
			m_SFX1.SetActive(false);
		}

		if (process_3008 == null) {
			process_3008 = new TickItem(onUpdate_3008);
			TickMgr.instance.addTick(process_3008);
		}
		m_cur3008_time = 0;
	}
	void onUpdate_3008(float s) {
		//跨地图时m_curModel==null
		if (m_curModel == null) {
			TickMgr.instance.removeTick(process_3008);
			return;
		}


		m_cur3008_time += s;
		if (m_cur3008_time > 0.6f) {
			m_SFX1.SetActive(true);
		}
		if (m_cur3008_time > m_skill3008_time) {
			GameObject.Destroy(m_SFX1);
			m_cur3008_time = 0;
			TickMgr.instance.removeTick(process_3008);
			process_3008 = null;
			m_SFX1 = null;

			GameObject _insta = GameObject.Instantiate(P3Mage.P3MAGE_SFX3) as GameObject;
			_insta.transform.SetParent(m_curModel, false);
			GameObject.Destroy(_insta, 1f);
		}
	}

	public void SetSkin() {
		SXML itemsXMl = XMLMgr.instance.GetSXML("mlzd");
		int lvln = SelfRole._inst.zhuan * 10 + SelfRole._inst.lvl;
		int waiguan = 2;
		int a_w_b_c = 2;
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
		m_strAvatarPath = "profession_mage_";

		foreach (Transform tran in m_Obj.GetComponentsInChildren<Transform>()) {
			tran.gameObject.layer = EnumLayer.LM_SELFROLE;// 更改物体的Layer层
		}

		Transform cur_model = m_Obj.transform.FindChild("model");

		m_proAvatar.Init_PA(A3_PROFESSION.Mage, m_strAvatarPath, "l_", m_curGameObj.layer, EnumMaterial.EMT_EQUIP_L, m_curModel);

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
