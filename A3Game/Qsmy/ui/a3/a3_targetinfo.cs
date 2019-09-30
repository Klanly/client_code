using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
	class a3_targetinfo : Window
	{
		GameObject m_Obj;
        private GameObject m_SelfObj;//角色的avatar
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        private ProfessionAvatar m_proAvatar;
        private Dictionary<int, Image> icon_ani = new Dictionary<int, Image>();
        public static a3_targetinfo instan;

        public static a3_targetinfo isshow;
        public override void init() {

            getComponentByPath<Text>("playerInfo/panel_attr/team").text = ContMgr.getCont("a3_targetinfo_0");
            getComponentByPath<Text>("playerInfo/panel_attr/no_team").text = ContMgr.getCont("a3_targetinfo_1");
            getComponentByPath<Text>("playerInfo/panel_attr/attr_scroll/scroll/contain/1/name").text = ContMgr.getCont("a3_targetinfo_2");
            getComponentByPath<Text>("playerInfo/panel_attr/attr_scroll/scroll/contain/2/name").text = ContMgr.getCont("a3_targetinfo_3");
            getComponentByPath<Text>("playerInfo/panel_attr/attr_scroll/scroll/contain/3/name").text = ContMgr.getCont("a3_targetinfo_4");
            getComponentByPath<Text>("playerInfo/panel_attr/att/atk/value").text = ContMgr.getCont("a3_targetinfo_5");
            getComponentByPath<Text>("playerInfo/panel_attr/att/hp/value").text = ContMgr.getCont("a3_targetinfo_6");
            getComponentByPath<Text>("playerInfo/panel_attr/att/phydef/value").text = ContMgr.getCont("a3_targetinfo_7");
            getComponentByPath<Text>("playerInfo/panel_attr/att/manadef/value").text = ContMgr.getCont("a3_targetinfo_8");
            getComponentByPath<Text>("eqtip/info/attr_scroll/scroll/contain/attr1/title").text = ContMgr.getCont("a3_targetinfo_9");
            getComponentByPath<Text>("eqtip/info/attr_scroll/scroll/contain/attr2/title").text = ContMgr.getCont("a3_targetinfo_10");
            getComponentByPath<Text>("eqtip/info/attr_scroll/scroll/contain/attr3/title").text = ContMgr.getCont("a3_targetinfo_11");
            getComponentByPath<Text>("eqtip/info/attr_scroll/scroll/contain/attr4/title").text = ContMgr.getCont("a3_targetinfo_12");
            getComponentByPath<Text>("eqtip/info/cancel/Text").text = ContMgr.getCont("a3_targetinfo_13");
            getComponentByPath<Text>("playerInfo/panel_attr/att/atk").text = ContMgr.getCont("a3_targetinfo_14");
            getComponentByPath<Text>("ig_bg1/txt1").text = ContMgr.getCont("uilayer_a3_auction_15");
            getComponentByPath<Text>("ig_bg1/txt2").text = ContMgr.getCont("uilayer_a3_auction_16");
            getComponentByPath<Text>("ig_bg1/txt3").text = ContMgr.getCont("uilayer_a3_auction_17");
            getComponentByPath<Text>("ig_bg1/txt4").text = ContMgr.getCont("uilayer_a3_auction_18");
            getComponentByPath<Text>("ig_bg1/txt5").text = ContMgr.getCont("uilayer_a3_auction_19");
            getComponentByPath<Text>("ig_bg1/txt6").text = ContMgr.getCont("uilayer_a3_auction_20");
            getComponentByPath<Text>("ig_bg1/txt7").text = ContMgr.getCont("uilayer_a3_auction_21");
            getComponentByPath<Text>("ig_bg1/txt8").text = ContMgr.getCont("uilayer_a3_auction_22");
            getComponentByPath<Text>("ig_bg1/txt9").text = ContMgr.getCont("uilayer_a3_auction_23");
            getComponentByPath<Text>("ig_bg1/txt10").text = ContMgr.getCont("uilayer_a3_auction_24");



            //== 关闭窗口
            instan = this;

            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) => 
            {
                if (a3_ranking.isshow && a3_ranking.isshow.Toback)
                {
                    a3_ranking.isshow.Toback.SetActive(true);
                    if (a3_ranking.isshow.showAvt != null && a3_ranking.isshow.scene_Camera!= null)
                    {
                        a3_ranking.isshow.showAvt.SetActive(true);
                        a3_ranking.isshow.scene_Camera.SetActive(true);
                    }
                    a3_ranking.isshow.Toback = null;
                }
                if (PlayerModel.getInstance().showFriend)
                {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO);
                    PlayerModel.getInstance().showFriend = false;
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_TARGETINFO);
            };
            for (int i = 1; i <= 10; i++)
            {
                icon_ani[i] = this.transform.FindChild("ig_bg1/ain" + i).GetComponent<Image>();
            }
            this.getEventTrigerByPath("avatar_touch").onDrag = OnDrag;
        }
        string RetWin = null;
		public override void onShowed() {
            isshow = this;
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
			
			uint tid = 0;
			if (uiData != null && uiData.Count > 0) {               
				tid = (uint)uiData[0];
                if (uiData.Count > 1)
                    RetWin = (string)uiData[1];
            }
			else {
				tid = SelfRole._inst.m_LockRole.m_unCID;
			}
			FriendProxy.getInstance().sendgetplayerinfo(tid);
			this.transform.SetAsLastSibling();
            create_scene();
            GRMap.GAME_CAMERA.SetActive(false);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
        }
		public override void onClosed() {
            isshow = null;
			FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
            disposeAvatar();
            for (int i = 1; i <= 10; i++)
            {
                GameObject go = transform.FindChild("ig_bg1/txt" + i).gameObject;
                go.GetComponent<Text>().enabled = true;
                if (go.transform.childCount > 0)
                {
                    Destroy(go.transform.GetChild(0).gameObject);
                }
            }
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);


            if (itemFriendPrefab.instance?.watch_avt == true)
            {
                ArrayList arr = new ArrayList();
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arr);
                itemFriendPrefab.instance.watch_avt = false;
               
            }

            if (itemNearbyListPrefab.instance?.watch_avt == true)
            {
                ArrayList arr = new ArrayList();
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arr);
                itemNearbyListPrefab.instance.watch_avt = false;

            }
            if (RetWin != null)
            {
                if (RetWin.Equals(InterfaceMgr.A3_FB_TEAM))
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(null);
                    arr.Add(false);
                    InterfaceMgr.getInstance().ui_async_open(RetWin,arr);
                }
                else
                    InterfaceMgr.getInstance().ui_async_open(RetWin);
                RetWin = null;
            }

            if (a3_ranking.isshow && a3_ranking.isshow.Toback)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RANKING);
            }

        }

        string name = "";
        int carr = 0;
        int lvl = 0;
        int zhuan = 0;
        int combpt = 0;
        int wing_lvl = 0;
        int wing_stage = 0;
        int show_wing = 0;
        int pet_type = 0;
        string clan_name = "";
        int jjclvl = 0;
        int jjcfec = 0;
        int summon_combpt = 0;
        Dictionary<int, a3_BagItemData> Equips = new Dictionary<int, a3_BagItemData> ();
        int[] fashionsshows = new int[2];//时装
        public Dictionary<int, a3_BagItemData> active_eqp = new Dictionary<int, a3_BagItemData>();
        void GetInfo(GameEvent e) {
			Variant data = e.data;
            name = data["name"];
            carr = data["carr"];
            lvl = data["lvl"];
            zhuan = data["zhuan"];
            combpt = data["combpt"];
            wing_lvl = data["wing_lvl"];
            if (data.ContainsKey ("wing_stage")) 
                wing_stage = data["wing_stage"];
            if(data.ContainsKey ("show_wing"))
                show_wing = data["show_wing"];
            if(data.ContainsKey ("pet_type"))
                pet_type = data["pet_type"];           
            if (data.ContainsKey("clan_name"))           
                clan_name = data["clan_name"];
            if (data.ContainsKey("summon_combpt"))
                summon_combpt = data["summon_combpt"];
            if (data.ContainsKey("grade"))
                jjclvl = data["grade"];
            if (data.ContainsKey("score"))
                jjcfec = data["score"];
            if(data.ContainsKey("dress_list"))
            {
                if (data["dress_list"] != null && data["dress_list"].Count > 0)
                {
                    fashionsshows[0] = data["dress_list"][0]._int;
                    fashionsshows[1] = data["dress_list"][1]._int;
                }
                else
                {
                    fashionsshows[0] = fashionsshows[1] = 0;
                }
            }
            else clan_name = "";
            string file = "";
            switch (carr)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_h2";
                    break;
                case 3:
                    file = "icon_job_icon_h3";
                    break;
                case 4:
                    file = "icon_job_icon_h4";
                    break;
                case 5:
                    file = "icon_job_icon_h5";
                    break;
            }
            Image head = transform.FindChild("playerInfo/panel_attr/hero_ig/ig").GetComponent<Image>();
            head.sprite = GAMEAPI.ABUI_LoadSprite(file);
            transform.FindChild("playerInfo/panel_attr/name").GetComponent<Text>().text = name;
            transform.FindChild("playerInfo/panel_attr/lv").GetComponent<Text>().text = "Lv" + lvl + "（" + zhuan + ContMgr.getCont("zhuan")+"）";
            getTransformByPath("fighting/value").GetComponent<Text>().text = combpt.ToString();
            if (jjclvl > 0)
            {
                SXML itXml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + jjclvl);
                string jjc = itXml.getString("name");
                transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/1/value").GetComponent<Text>().text = jjc;
            }
            else {
                transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/1/value").GetComponent<Text>().text = ContMgr.getCont("a3_targetinfo_nodw");
            }
            if (jjclvl < 10)
            {
                transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/1/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + jjclvl);
            }
            else {
                transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/1/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + jjclvl);
            }
            transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/2/value").GetComponent<Text>().text = summon_combpt.ToString();
            transform.FindChild("playerInfo/panel_attr/attr_scroll/scroll/contain/3/value").GetComponent<Text>().text = wing_stage+ ContMgr.getCont("a3_auction_jie") + wing_lvl+ ContMgr.getCont("ji");

            if (data.ContainsKey("min_attack"))
            {
                transform.FindChild("playerInfo/panel_attr/att/atk/value").GetComponent<Text>().text = data["min_attack"] + "-" + data["max_attack"];
                transform.FindChild("playerInfo/panel_attr/att/hp/value").GetComponent<Text>().text = data["max_hp"];
                transform.FindChild("playerInfo/panel_attr/att/phydef/value").GetComponent<Text>().text = data["physics_def"];
                transform.FindChild("playerInfo/panel_attr/att/manadef/value").GetComponent<Text>().text = data["magic_def"];
            }

            if (clan_name == "")
            {
                transform.FindChild("playerInfo/panel_attr/team").gameObject.SetActive(false);
                transform.FindChild("playerInfo/panel_attr/no_team").gameObject.SetActive(true);
            }
            else {
                transform.FindChild("playerInfo/panel_attr/team").gameObject.SetActive(true);
                transform.FindChild("playerInfo/panel_attr/team/team_name").GetComponent<Text>().text = clan_name;
                transform.FindChild("playerInfo/panel_attr/no_team").gameObject.SetActive(false);
            }

            Variant equips = data["equipments"];
            Equips.Clear();
            active_eqp.Clear();
            foreach (var v in equips._arr)
			{
				a3_BagItemData item = new a3_BagItemData();
				item.confdata.equip_type = v["part_id"];
				Variant info = v["eqpinfo"];
				item.id = info["id"];
				item.tpid = info["tpid"];
				item.confdata = a3_BagModel.getInstance().getItemDataById(item.tpid);
				a3_EquipModel.getInstance().equipData_read(item, info);
                Equips[item.confdata.equip_type] = item;
            }
            foreach (a3_BagItemData item in Equips.Values)
            {
                if (isactive_eqp(item))
                {
                    active_eqp[item.confdata.equip_type] = item;
                }
            }
            //Dictionary < int, a3_BagItemData > newEquip = new Dictionary<int, a3_BagItemData>();
            //foreach (int itemid in Equips.Keys)
            //{
            //    a3_BagItemData one = Equips[itemid];
            //    one.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att(Equips[itemid]));
            //    newEquip[itemid] = one;
            //}

            //Equips.Clear();
            //Equips = newEquip;

            initEquipIcon();
            createAvatar();
            setAni();
            SetAni_Color();
        }

        public bool isactive_eqp(a3_BagItemData data)
        {
            if (data.equipdata.attribute == 0)
                return false;
            int needTpye_act = a3_EquipModel .getInstance().eqp_type_act[data.confdata.equip_type];
            if (!Equips.ContainsKey(needTpye_act))
                return false;
            int needatt = a3_EquipModel.getInstance().eqp_att_act[data.equipdata.attribute];
            if (Equips[needTpye_act].equipdata.attribute == needatt)
                return true;
            else
                return false;
        }

        private Dictionary<int, GameObject> equipicon = new Dictionary<int, GameObject>();
        public void initEquipIcon()
        { 

            equipicon.Clear();
            Dictionary<int, a3_BagItemData> equips = Equips;
            foreach (int i in equips.Keys)
            {
                a3_BagItemData data = equips[i];
                if ( data.confdata.equip_type == 11 || data.confdata.equip_type == 12 )
                    break;
                CreateEquipIcon(data);
            }
        }
        void CreateEquipIcon(a3_BagItemData data)
        {
            GameObject icon;
            icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            IconImageMgr.getInstance().refreshA3EquipIcon_byType(icon, data, EQUIP_SHOW_TYPE.SHOW_INTENSIFYANDSTAGE);

            GameObject parent = transform.FindChild("ig_bg1/txt" + data.confdata.equip_type).gameObject;
            icon.transform.SetParent(parent.transform, false);
            parent.GetComponent<Text>().enabled = false;
            icon.transform.FindChild("iconborder").gameObject.SetActive(false);
            equipicon[data.confdata.equip_type] = icon;
            icon.transform.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) { this.onEquipClick(icon, data); };
        }

        void setAni()
        {
            foreach (int i in icon_ani.Keys)
            {
                if (active_eqp.ContainsKey(i)) { icon_ani[i].gameObject.SetActive(true); }
                else { icon_ani[i].gameObject.SetActive(false); }
            }
        }
        public void SetAni_Color()
        {
            foreach (int type in Equips.Keys)
            {
                if ( type == 11 ||  type ==12)
                    break;
                Color col = new Color();
                switch (Equips[type].equipdata.attribute)
                {
                    case 1:
                        col = new Color(0f, 0.47f, 0f);
                        break;
                    case 2:
                        col = new Color(0.68f, 0.26f, 0.03f);
                        break;
                    case 3:
                        col = new Color(0.76f, 0.86f, 0.33f);
                        break;
                    case 4:
                        col = new Color(0.97f, 0.11f, 0.87f);
                        break;
                    case 5:
                        col = new Color(0.17f, 0.18f, 0.57f);
                        break;
                }
                icon_ani[type].GetComponent<Image>().color = col;
            }
        }
        void onEquipClick(GameObject go, a3_BagItemData one)
        {
            ArrayList data = new ArrayList();
            data.Add(one);
            data.Add(equip_tip_type.tip_ForLook);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
        }
        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                string m_strAvatarPath = "";
                string equipEff_path = "";
                if (carr == 2)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                    m_strAvatarPath = "profession_warrior_";
                    equipEff_path = "Fx_armourFX_warrior_";
                }
                else if (carr == 3)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 167, 0, 0)) as GameObject;
                    m_strAvatarPath = "profession_mage_";
                    equipEff_path = "Fx_armourFX_mage_";
                }
                else if (carr == 5)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                    m_strAvatarPath = "profession_assa_";
                    equipEff_path = "Fx_armourFX_assa_";
                }
                else
                {
                    return;
                }

                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
                }

                Transform cur_model = m_SelfObj.transform.FindChild("model");

                //手上的小火球
                if (carr == 3)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                int bodyid = 0;
                int bodyFxid = 0;
                uint colorid = 0;
                if (Equips.ContainsKey(3))
                {
                    bodyid = (int)Equips[3].tpid;
                    bodyFxid = Equips[3].equipdata.stage;
                    colorid = Equips[3].equipdata.color;
                }
                if ( Equips.ContainsKey( 11 ) )
                {
                    bodyid = ( int ) Equips[ 11 ].tpid;
                    bodyFxid = Equips[ 11 ].equipdata.stage;
                    colorid = Equips[ 11 ].equipdata.color;
                }
                int m_Weapon_LID = 0;
                int m_Weapon_LFXID = 0;
                int m_Weapon_RID = 0;
                int m_Weapon_RFXID = 0;

                if (Equips.ContainsKey(6))
                {
                    switch (carr)
                    {
                        case 2:
                            m_Weapon_RID = (int)Equips[6].tpid;
                            m_Weapon_RFXID = Equips[6].equipdata.stage;
                            break;
                        case 3:
                            m_Weapon_LID = (int)Equips[6].tpid;
                            m_Weapon_LFXID = Equips[6].equipdata.stage;
                            break;
                        case 5:
                            m_Weapon_LID = (int)Equips[6].tpid;
                            m_Weapon_LFXID = Equips[6].equipdata.stage;
                            m_Weapon_RID = (int)Equips[6].tpid;
                            m_Weapon_RFXID = Equips[6].equipdata.stage;
                            break;
                    }
                }
                if ( Equips.ContainsKey( 12 ) )
                {
                    switch ( carr )
                    {
                        case 2:
                        m_Weapon_RID = ( int ) Equips[ 12 ].tpid;
                        m_Weapon_RFXID = Equips[ 12 ].equipdata.stage;
                        break;
                        case 3:
                        m_Weapon_LID = ( int ) Equips[ 12 ].tpid;
                        m_Weapon_LFXID = Equips[ 12 ].equipdata.stage;
                        break;
                        case 5:
                        m_Weapon_LID = ( int ) Equips[ 12 ].tpid;
                        m_Weapon_LFXID = Equips[ 12 ].equipdata.stage;
                        m_Weapon_RID = ( int ) Equips[ 12 ].tpid;
                        m_Weapon_RFXID = Equips[ 12].equipdata.stage;
                        break;
                    }
                }
                m_proAvatar = new ProfessionAvatar();
                m_proAvatar.Init_PA(eprofession, m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, equipEff_path);
                if (active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(bodyid, true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel .getInstance ().GetEff_lvl (active_eqp.Count));
                if (fashionsshows[1] != 0)
                    m_proAvatar.set_body(fashionsshows[1], 0);
                else
                    m_proAvatar.set_body(bodyid, bodyFxid);
                if (fashionsshows[0] != 0)
                {
                    m_proAvatar.set_weaponl(fashionsshows[0], 0);
                    m_proAvatar.set_weaponr(fashionsshows[0], 0);
                }
                else
                {
                    m_proAvatar.set_weaponl(m_Weapon_LID, m_Weapon_LFXID);
                    m_proAvatar.set_weaponr(m_Weapon_RID, m_Weapon_RFXID);
                }
                m_proAvatar.set_wing(show_wing, show_wing);
                m_proAvatar.set_equip_color(colorid);
                if (m_proAvatar != null) m_proAvatar.FrameMove();
            }
        }

        public void create_scene()
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
            scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
            scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
            foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
            {
                if (tran.gameObject.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
        }
  //      void CreateAvatar(Variant d) {
		//	int m_BodyID = 0;
		//	int m_BodyFXID = 0;
		//	uint m_EquipColorID = 0;
		//	int m_Weapon_LID = 0;
		//	int m_Weapon_LFXID = 0;
		//	int m_Weapon_RID = 0;
		//	int m_Weapon_RFXID = 0;
		//	int carr = d["carr"];
		//	int winglv = d["show_wing"];
		//	int wingstage = d["show_wing"];

		//	foreach (Variant vp in d["equipments"]._arr) {
		//		a3_ItemData data = a3_BagModel.getInstance().getItemDataById(vp["eqpinfo"]["tpid"]);
		//		Variant p = vp["eqpinfo"];
		//		if (data.equip_type == 3) {
		//			int bodyid = (int)data.tpid;
		//			int bodyFxid = p["intensify_lv"];
		//			m_BodyID = bodyid;
		//			m_BodyFXID = bodyFxid;

		//			uint colorid = 0;
		//			if (p.ContainsKey("colour"))
		//				colorid = p["colour"];
		//			m_EquipColorID = colorid;
		//		}
		//		if (data.equip_type == 6) {
		//			int weaponid = (int)data.tpid;
		//			int weaponFxid = p["intensify_lv"];
		//			switch (carr) {
		//				case 2:
		//					m_Weapon_RID = weaponid;
		//					m_Weapon_RFXID = weaponFxid;
		//					break;
		//				case 3:
		//					m_Weapon_LID = weaponid;
		//					m_Weapon_LFXID = weaponFxid;
		//					break;
		//				case 5:
		//					m_Weapon_LID = weaponid;
		//					m_Weapon_LFXID = weaponFxid;
		//					m_Weapon_RID = weaponid;
		//					m_Weapon_RFXID = weaponFxid;
		//					break;
		//			}
		//		}
		//	}
		//	//CreateAvatar(carr, m_BodyID, m_BodyFXID, m_EquipColorID, m_Weapon_LID, m_Weapon_LFXID, m_Weapon_RID, m_Weapon_RFXID, winglv, wingstage);
		//}

        //void DisposeAvatar() {
        //	m_proAvatar = null;

        //	if (m_Obj != null) GameObject.Destroy(m_Obj);
        //	if (m_Camera != null) GameObject.Destroy(m_Camera);
        //}
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        void OnDrag(GameObject go, Vector2 delta) {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }

        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        //      void SetOccupationIcon() {
        //	if (SelfRole._inst.m_LockRole is P2Warrior) {
        //		getTransformByPath("name/pro/mage").gameObject.SetActive(false);
        //		getTransformByPath("name/pro/ass").gameObject.SetActive(false);
        //		getTransformByPath("name/pro/war").gameObject.SetActive(true);
        //	}
        //	else if (SelfRole._inst.m_LockRole is P3Mage) {
        //		getTransformByPath("name/pro/mage").gameObject.SetActive(true);
        //		getTransformByPath("name/pro/ass").gameObject.SetActive(false);
        //		getTransformByPath("name/pro/war").gameObject.SetActive(false);
        //	}
        //	else if (SelfRole._inst.m_LockRole is P5Assassin) {
        //		getTransformByPath("name/pro/mage").gameObject.SetActive(false);
        //		getTransformByPath("name/pro/ass").gameObject.SetActive(true);
        //		getTransformByPath("name/pro/war").gameObject.SetActive(false);
        //	}
        //}

        //void ETest() {
        //	var v = a3_EquipModel.getInstance().getEquipsByType()[3];
        //	CreateEquipIcon(v);
        //}

        //void CreateEquipIcon(a3_BagItemData data) {
        //	GameObject icon;
        //	if (data.confdata.equip_type != 8 && data.confdata.equip_type != 9 && data.confdata.equip_type != 10) {
        //		icon = IconImageMgr.getInstance().createA3EquipIcon(data, 1.0f, true);
        //	}
        //	else {
        //		icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
        //	}
        //	IconImageMgr.getInstance().refreshA3EquipIcon_byType(icon, data, EQUIP_SHOW_TYPE.SHOW_INTENSIFY);

        //	GameObject parent = transform.FindChild("ig_bg2/txt" + data.confdata.equip_type).gameObject;
        //	icon.transform.SetParent(parent.transform, false);
        //	icon.name = data.id.ToString();
        //	icon.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(false);
        //	icon.transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
        //	icon.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
        //	icon.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
        //	icon.transform.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);

        //	List<a3_BagItemData> temp = new List<a3_BagItemData>();
        //	temp.Add(data);
        //	new BaseButton(icon.transform).onClick = (GameObject go) => {
        //		//transform.FindChild("eqtip").gameObject.SetActive(true);
        //		if (m_Obj != null && m_Obj.activeSelf) { m_Obj.SetActive(false); }
        //              //ShowEPTip(temp[0]);
        //              ArrayList data1 = new ArrayList();
        //              data1.Add(temp[0]);
        //              data1.Add(equip_tip_type.tip_ForLook);
        //              InterfaceMgr.getInstance().open(InterfaceMgr.A3_EQUIPTIP, data1);
        //          };
        //	eqs.Add(icon);
        //}

        public void setavt()
        {
            if (m_Obj != null && !m_Obj.activeSelf) { m_Obj.SetActive(true); }
        }

		//public void ShowEPTip(a3_BagItemData data, bool puton = true, bool buy = false) {
		//	var info = etra.FindChild("info");
		//	for (int i = 1; i <= 5; i++) {
		//		if (i == data.confdata.quality) {
		//			info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(true);
		//		}
		//		else {
		//			info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(false);
		//		}
		//	}
		//	info.FindChild("money").GetComponent<Text>().text = "价值：" + data.confdata.value;
		//	info.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
		//	info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
		//	info.FindChild("txt_value").GetComponent<Text>().text = data.equipdata.combpt.ToString();
		//	info.FindChild("lv").GetComponent<Text>().text = data.equipdata.stage.ToString() + "阶" + Globle.getEquipTextByType(data.confdata.equip_type);
		//	string str = "";
		//	switch (data.confdata.job_limit) {
		//		case 1:
		//			str = "全职业";
		//			break;
		//		case 2:
		//			str = "战士";
		//			break;
		//		case 3:
		//			str = "法师";
		//			break;
		//		case 5:
		//			str = "刺客";
		//			break;
		//	}
		//	info.FindChild("profession").GetComponent<Text>().text = str;
		//	Transform Image = info.FindChild("icon");
		//	if (Image.childCount > 0) {
		//		GameObject.Destroy(Image.GetChild(0).gameObject);
		//	}
		//	GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(data);
		//	icon.transform.SetParent(Image, false);
		//	icon.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(false);
		//	icon.transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
		//	icon.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
		//	icon.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);

		//	SetStar(etra.FindChild("info/stars"), data.equipdata.intensify_lv);

		//	info.FindChild("zhufu/title").GetComponent<Text>().text = "祝福" + data.equipdata.blessing_lv
		//		+ "（需求-" + 3 * data.equipdata.blessing_lv + "%）";
		//	initAtt(info, data);
		//}

		//void initAtt(Transform info, a3_BagItemData equip_data) {
		//	SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
		//	Text text_basic_att = info.FindChild("attr_scroll/scroll/contain/attr1/text1").GetComponent<Text>();
		//	SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).GetNode("stage_info", "itemid==" + equip_data.tpid);

		//	//祝福
		//	string[] list_need1 = stage_xml.getString("equip_limit1").Split(',');
		//	string[] list_need2 = stage_xml.getString("equip_limit2").Split(',');
		//	int need1 = int.Parse(list_need1[1]) * (100 - 3 * equip_data.equipdata.blessing_lv) / 100;
		//	int need2 = int.Parse(list_need2[1]) * (100 - 3 * equip_data.equipdata.blessing_lv) / 100;
		//	string text_need1, text_need2;
		//	if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])]) {
		//		text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
		//	}
		//	else {
		//		text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
		//	}
		//	if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])]) {
		//		text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
		//	}
		//	else {
		//		text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
		//	}

		//	info.FindChild("zhufu/text1").GetComponent<Text>().text = text_need1;
		//	info.FindChild("zhufu/text2").GetComponent<Text>().text = text_need2;

		//	//基础属性
		//	string[] list_att0 = XMLMgr.instance.GetSXML("item.stage", "stage_level==0").GetNode("stage_info", "itemid==" + equip_data.tpid).getString("basic_att").Split(',');
		//	string[] list_att2 = stage_xml.getString("basic_att").Split(',');
		//	if (equip_data.equipdata.intensify_lv > 0) {
		//		SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + equip_data.equipdata.intensify_lv).GetNode("intensify_info", "itemid==" + equip_data.tpid);
		//		string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');

		//		if (list_att1.Length > 1) {
		//			int add1 = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));
		//			int add2 = int.Parse(list_att1[1]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[1]) - int.Parse(list_att0[1]));

		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
		//				+ "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
		//		}
		//		else {
		//			int add = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));

		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
		//		}
		//	}
		//	else if (equip_data.equipdata.stage > 0) {
		//		if (list_att0.Length > 1) {
		//			int add1 = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
		//			int add2 = int.Parse(list_att2[1]) - int.Parse(list_att0[1]);

		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
		//				+ "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
		//		}
		//		else {
		//			int add = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
		//		}
		//	}
		//	else {
		//		if (list_att0.Length > 1) {
		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0]
		//				+ "-" + list_att0[1];
		//		}
		//		else {
		//			text_basic_att.text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0];
		//		}
		//	}

		//	//附加属性
		//	int subjoin_num = 0;
		//	SXML subjoin_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + equip_data.confdata.equip_level);
		//	foreach (int type in equip_data.equipdata.subjoin_att.Keys) {
		//		subjoin_num++;

		//		SXML subjoin_xml_node = subjoin_xml.GetNode("subjoin_att_info", "att_type==" + type);
		//		Text subjoin_text = info.FindChild("attr_scroll/scroll/contain/attr2/texts/scroll/contain/text" + subjoin_num).GetComponent<Text>();
		//		subjoin_text.gameObject.SetActive(true);
		//		subjoin_text.text = Globle.getAttrAddById(type, equip_data.equipdata.subjoin_att[type]);
		//		if (subjoin_xml_node!=null && equip_data.equipdata.subjoin_att[type] >= subjoin_xml_node.getInt("max")) {
		//			info.FindChild("attr_scroll/scroll/contain/attr2/texts/scroll/contain/text" + subjoin_num + "/max").gameObject.SetActive(true);
		//		}
		//		else {
		//			info.FindChild("attr_scroll/scroll/contain/attr2/texts/scroll/contain/text" + subjoin_num + "/max").gameObject.SetActive(false);
		//		}
		//	}
		//	for (int i = subjoin_num + 1; i <= 5; i++) {
		//		GameObject subjoin_go = info.FindChild("attr_scroll/scroll/contain/attr2/texts/scroll/contain/text" + i).gameObject;
		//		subjoin_go.transform.FindChild("max").gameObject.SetActive(false);
		//		subjoin_go.GetComponent<Text>().text = "无";
		//		//subjoin_go.SetActive(false);
		//	}

		//	//宝石属性
		//	int gem_num = 0;
		//	foreach (int type in equip_data.equipdata.gem_att.Keys) {
		//		gem_num++;
		//		Text gem_text = info.FindChild("attr_scroll/scroll/contain/attr3/text" + gem_num).GetComponent<Text>();
		//		gem_text.text = Globle.getAttrAddById(type, equip_data.equipdata.gem_att[type]);
		//	}
		//	//info.FindChild("attr_scroll/scroll/contain/attr3").localPosition = new Vector3(0, -200 + (5 - subjoin_num) * 20, 0);

		//	//追加属性
		//	SXML add_xml = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (equip_data.equipdata.add_level + 1));

		//	SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
		//	int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
		//	int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * equip_data.equipdata.add_level;
		//	info.FindChild("attr_scroll/scroll/contain/attr4/text1").GetComponent<Text>().text = Globle.getAttrAddById(attType, attValue);
		//}
		void SetStar(Transform starRoot, int num) {
			int count = 0;
			foreach (var v in starRoot.GetComponentsInChildren<Transform>(true)) {
				if (v.parent != null && v.parent.parent == starRoot.transform) {
					if (count < num) { v.gameObject.SetActive(true); count++; }
					else { v.gameObject.SetActive(false); }
				}
			}
		}
	}
}
