using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MuGame
{
    class a3_selectcha_loadmodel
    {
        public a3_selectcha_loadmodel(uint id, int face)
        {
            chaCid = id;
            smshow_face = face;
        }

        public uint chaCid;
        public int smshow_face;
    }

    class a3_selectcha: FloatUi
    {
        private Button returnBtn;
        private Button enterGameBtn;
        private BaseButton[] chooseBtns;
        private Dictionary<uint, ProfessionAvatar> avatars = new Dictionary<uint, ProfessionAvatar>(); //<角色id,形象>
        private Dictionary<uint, GameObject> ava_pet_mon = new Dictionary<uint, GameObject>();//<角色id，召唤兽形象>
        private int chaSelectedIndex = -1; //当前选中index
        private uint chaCid = 0; //当前选中角色cid
        private ProfessionAvatar chaAvatar = null; //当前选中角色avatar
        private GameObject chaMonster_mon = null;//当前选中角色召唤兽avatar

        private AsyncOperation async = null;
        private GameObject standPoint = null;

        private ProfessionAvatar m_curProAvatar = null;

        private GameObject fg;

        private IEnumerator LoadScene()
        {
            if (fg != null)
                fg.SetActive(true);

            GAMEAPI.LoadAsset_Async("select_role_scene.assetbundle", "select_role_scene");
            while (GAMEAPI.Res_Async_Loaded() == false)
            {
                yield return new WaitForSeconds(0.1f);
            }

            async = SceneManager.LoadSceneAsync( "select_role_scene" );
            yield return async;

            if (MediaClient.instance._curMusicUrl != "audio_map_music_0")
            {
                MediaClient.instance.PlayMusicUrl("audio_map_music_0", null, true);

                Application.DontDestroyOnLoad(GameObject.Find("Audio"));
            }

            GAMEAPI.Unload_Asset("select_role_scene.assetbundle");

            if (fg != null)
                fg.SetActive(false);
        }

        public override void init()
        {


            getComponentByPath<Text>("deleteBtn/Text").text = ContMgr.getCont("a3_selectcha_0");
            getComponentByPath<Text>("enterGameBtn/Text").text = ContMgr.getCont("a3_selectcha_1");

            getComponentByPath<Text>("btnChoose/chaBtn0/chuangjian/zi").text = ContMgr.getCont("a3_selectcha_2");
            getComponentByPath<Text>("btnChoose/chaBtn1/chuangjian/zi").text = ContMgr.getCont("a3_selectcha_2");
            getComponentByPath<Text>("btnChoose/chaBtn2/chuangjian/zi").text = ContMgr.getCont("a3_selectcha_2");
            getComponentByPath<Text>("btnChoose/chaBtn3/chuangjian/zi").text = ContMgr.getCont("a3_selectcha_2");

            returnBtn = getComponentByPath<Button>("returnBtn");
            enterGameBtn = getComponentByPath<Button>("enterGameBtn");

            chooseBtns = new BaseButton[4]
            {
                new BaseButton(getTransformByPath("btnChoose/chaBtn0")),
                new BaseButton(getTransformByPath("btnChoose/chaBtn1")),
                new BaseButton(getTransformByPath("btnChoose/chaBtn2")),
                new BaseButton(getTransformByPath("btnChoose/chaBtn3")),
            };

            this.getEventTrigerByPath("returnBtn").onClick = onReturnClick;
            this.getEventTrigerByPath("enterGameBtn").onClick = onEnterGameClick;
            this.getEventTrigerByPath("TouchDrag").onDrag = OnDrag;
            this.getEventTrigerByPath("deleteBtn").onClick = onDeleteChaClick;

            
            if (transform.FindChild("fg") != null)
            {
                fg = getGameObjectByPath("fg");
                fg.SetActive(true);
                fg.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth, Baselayer.uiHeight);
            }

            muNetCleint.instance.charsInfoInst.addEventListener(UI_EVENT.UI_ACT_DELETE_CHAR, onDeleteChar);
        }

        void Update()
        {
            if (async != null && async.isDone)
            {
                async = null;

                standPoint = GameObject.Find("standpoint");
                //!--加载完成后,刷新一次,显示选中的角色
                Variant chas = muNetCleint.instance.charsInfoInst.getChas();
                if (chas.Count > 0)
                {
                    onChooseCha(chooseBtns[0].gameObject);
                }
            }

            if (m_curProAvatar != null) m_curProAvatar.FrameMove();
        }

        public override void onShowed()
        {
            GRMap.DontDestroyBaseGameObject();
            StartCoroutine(LoadScene());

            refreshChooseBtns();
           
        }

        private void refreshChooseBtns()
        {
            Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            
            for (int i = 0; i < chooseBtns.Length; i++)
            {
                Text btnTxt = chooseBtns[i].transform.FindChild("Text").GetComponent<Text>();
                Text infoText = chooseBtns[i].transform.FindChild("infoTxt").GetComponent<Text>();

                GameObject iconGo = chooseBtns[i].transform.FindChild("ig_bg").gameObject;

                if (i < chas.Count)
                {//显示角色

                    btnTxt.text = chas[i]["name"];
                    infoText.gameObject.SetActive(true);
                    iconGo.SetActive(true);

                    string file = "";
                    //string file1 = "";
                    int carr = chas[i]["carr"];
                    string carrstr = "";
                    switch (carr)
                    {
                        case 2:
                            carrstr = ContMgr.getCont("a3_firstRechargeAward_p1");
                            file = "icon_job_icon_selectcha_zs_1";
                            break;
                        case 3:
                            carrstr = ContMgr.getCont("a3_firstRechargeAward_p2");
                            file = "icon_job_icon_selectcha_fs_1";
                            break;
                        case 4:
                            carrstr = ContMgr.getCont("gongshou");
                            file = "icon_job_icon_h4";
                            break;
                        case 5:
                            carrstr = ContMgr.getCont("a3_firstRechargeAward_p3");
                            file = "icon_job_icon_selectcha_ck_1";
                            break;
                    }
                    iconGo.transform.FindChild("ig_icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    iconGo.transform.FindChild("ig_icon_null").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    string zhuan = string.Empty;
                    if (chas[i]["zhua"]!=null)
                        zhuan = chas[i]["zhua"]._int.ToString();
                   
                    infoText.text = zhuan + ContMgr.getCont("zhuan") + chas[i]["lvl"]._int + ContMgr.getCont("ji");
                    chooseBtns[i].onClick = onChooseCha;
                    chooseBtns[i].transform.FindChild("chuangjian").gameObject.SetActive(false);
                    chooseBtns[i].transform.FindChild("Text").gameObject.SetActive(true);
                    chooseBtns[i].transform.FindChild("infoTxt").gameObject.SetActive(true);
                    iconGo.transform.FindChild("ig_icon").gameObject.SetActive(false);
                }
                else
                {
                    btnTxt.gameObject.SetActive(false);
                    infoText.gameObject.SetActive(false);
                    iconGo.SetActive(false);
                    chooseBtns[i].transform.FindChild("chuangjian").gameObject.SetActive(true);
                    chooseBtns[i].transform.FindChild("Text").gameObject.SetActive(false);
                    chooseBtns[i].transform.FindChild("infoTxt").gameObject.SetActive(false);
                    chooseBtns[i].onClick = onCreateCha;
                }
            }
        }

        private void refreshAvatar()
        {
            if (standPoint == null)
                return;

            foreach (ProfessionAvatar ava in avatars.Values)
            {
                ava.m_curModel.gameObject.SetActive(false);
            }
            foreach (GameObject ava in ava_pet_mon.Values)
            {
                if(ava != null)
                ava.SetActive(false);
            }

            //获得选中的角色的形象,若未创建，进行创建
            ProfessionAvatar avatar = null;
            
            avatars.TryGetValue(chaCid, out avatar);
            if (avatar == null)
            {
                avatar = createAvatar(chaSelectedIndex);
                if (avatar == null) return;
                avatars[chaCid] = avatar;
            }

            chaAvatar = avatar;
            chaAvatar.m_curModel.gameObject.SetActive(true);

            GameObject avatar_mon = null;
            ava_pet_mon.TryGetValue(chaCid, out avatar_mon);
            if(avatar_mon == null )
            {
                createAvata_mon(chaSelectedIndex);
            }
            else
            {
                chaMonster_mon = avatar_mon;
                chaMonster_mon.SetActive(true);
            }
        }

        private void onChooseCha(GameObject go)
        {
            for (int i = 0; i < 4; i++)
            {
                if (go == chooseBtns[i].gameObject)
                {
                    chaSelectedIndex = i;
                    chooseBtns[i].transform.FindChild("Image").gameObject.SetActive(true);
                    chooseBtns[i].transform.FindChild("ig_bg/ig_icon").gameObject.SetActive(true);
                    chooseBtns[i].transform.FindChild("ig_bg/ig_icon_null").gameObject.SetActive(false);
                }
                else
                {
                    chooseBtns[i].transform.FindChild("Image").gameObject.SetActive(false);
                    chooseBtns[i].transform.FindChild("ig_bg/ig_icon").gameObject.SetActive(false);
                    chooseBtns[i].transform.FindChild("ig_bg/ig_icon_null").gameObject.SetActive(true);
                }
            }

            //获得选中的角色CID
            MediaClient.instance.StopSounds();
            Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            chaCid = chas[chaSelectedIndex]["cid"];
            int carr = chas[chaSelectedIndex]["carr"];
            switch (carr)
            {
                case 2:
                    MediaClient.instance.PlaySoundUrl("audio_common_zhanshi", false, null);
                    break;
                case 3:
                    MediaClient.instance.PlaySoundUrl("audio_common_fashi", false, null);
                    break;
                case 5:
                    MediaClient.instance.PlaySoundUrl("audio_common_cike", false, null);
                    break;
            }
            refreshAvatar();
        }

        private ProfessionAvatar createMagaAvatar(ChaOutInfo info)
        {
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
            GameObject magicGO = GameObject.Instantiate(obj_prefab, standPoint.transform.position, Quaternion.identity) as GameObject;

            Transform cur_model = magicGO.transform.FindChild("model");

            //创建avatar
            ProfessionAvatar m_proAvatar = new ProfessionAvatar();
            string str = "h_";
            m_proAvatar.Init_PA(A3_PROFESSION.Mage, "profession_mage_", str, EnumLayer.LM_SELFROLE, EnumMaterial.EMT_EQUIP_H, cur_model.transform, "Fx_armourFX_mage_");
            m_proAvatar.set_body(info.bodyID, info.bodyInte);
            m_proAvatar.set_weaponl(info.weaponLID, info.weaponLInte);
            //m_proAvatar.set_weaponr(info.weaponRID, info.weaponRInte);
            m_proAvatar.set_wing(info.wingID, info.wingInte);
			m_proAvatar.set_equip_color((uint)info.colorID);

            if (info.addeff)
            {
                m_proAvatar.set_equip_eff(info.bodyID, true);
            }
            m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(info.activecount));
            m_curProAvatar = m_proAvatar; 

            //火焰
            Transform cur_r_finger1 = magicGO.transform.FindChild("model/R_Finger1");
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
            GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
            light_fire.transform.SetParent(cur_r_finger1, false);

            return m_proAvatar;
        }

        private ProfessionAvatar createWarriorAvatar(ChaOutInfo info)
        {
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
            GameObject warriorGO = GameObject.Instantiate(obj_prefab, standPoint.transform.position, Quaternion.identity) as GameObject;

            Transform cur_model = warriorGO.transform.FindChild("model");
            //创建avatar
            ProfessionAvatar m_proAvatar = new ProfessionAvatar();
            string str = "h_";
            m_proAvatar.Init_PA(A3_PROFESSION.Warrior, "profession_warrior_", str, EnumLayer.LM_SELFROLE, EnumMaterial.EMT_EQUIP_H, cur_model, "Fx_armourFX_warrior_");
            m_proAvatar.set_body(info.bodyID, info.bodyInte);
            //m_proAvatar.set_weaponl(info.weaponLID, info.weaponLInte);
            m_proAvatar.set_weaponr(info.weaponRID, info.weaponRInte);
            m_proAvatar.set_wing(info.wingID, info.wingInte);
			m_proAvatar.set_equip_color((uint)info.colorID);

            if (info.addeff)
            {
                m_proAvatar.set_equip_eff(info.bodyID,true);
            }
            m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(info.activecount));
            m_curProAvatar = m_proAvatar;

            return m_proAvatar;
        }

        private GameObject createArcherAvatar(ChaOutInfo info)
        {//TODO 
            return null;
        }

        private ProfessionAvatar createAssassinAvatar(ChaOutInfo info)
        {
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
            GameObject rogueGO = GameObject.Instantiate(obj_prefab, standPoint.transform.position, Quaternion.identity) as GameObject;

            Transform cur_model = rogueGO.transform.FindChild("model");

            //创建avatar
            ProfessionAvatar m_proAvatar = new ProfessionAvatar();
            string str = "h_";
            m_proAvatar.Init_PA(A3_PROFESSION.Assassin, "profession_assa_", str, EnumLayer.LM_SELFROLE, EnumMaterial.EMT_EQUIP_H, cur_model, "Fx_armourFX_assa_");

            m_proAvatar.set_body(info.bodyID, info.bodyInte);
            m_proAvatar.set_weaponl(info.weaponLID, info.weaponLInte);
            m_proAvatar.set_weaponr(info.weaponRID, info.weaponRInte);
            m_proAvatar.set_wing(info.wingID, info.wingInte);
			m_proAvatar.set_equip_color((uint)info.colorID);
            if (info.addeff)
            {
                m_proAvatar.set_equip_eff(info.bodyID, true);
            }
            m_proAvatar.set_equip_eff(a3_EquipModel.getInstance ().GetEff_lvl (info.activecount ));
            m_curProAvatar = m_proAvatar;

            return m_proAvatar;
        }


        private void createAssassinAvatar_mon(int tpid)
        {
            int objid = 0;
            SXML itemsXMl = XMLMgr.instance.GetSXML("callbeast");
            var xml = itemsXMl.GetNode("callbeast", "id==" + tpid);
            var mid = xml.getInt("mid");
            var mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            objid = mxml.getInt("obj");

            GAMEAPI.ABModel_LoadGameObject("monster_" + objid, a3_selectchar_Model_Loaded, new a3_selectcha_loadmodel(chaCid, mxml.getInt("smshow_face")));
        }

        private void a3_selectchar_Model_Loaded(UnityEngine.Object model_obj, System.Object data)
        {
            if (standPoint == null)
                return;

            a3_selectcha_loadmodel info_data = data as a3_selectcha_loadmodel;
            if(ava_pet_mon.ContainsKey(info_data.chaCid) == false )
            {
                GameObject obj_prefab = model_obj as GameObject;
                GameObject rogueGO = GameObject.Instantiate(obj_prefab, new Vector3(standPoint.transform.position.x - 1.4f, standPoint.transform.position.y, standPoint.transform.position.z - 1f), Quaternion.identity) as GameObject;
                foreach (Transform tran in rogueGO.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_SELFROLE;// 更改物体的Layer层
                }
                Transform cur_model = rogueGO.transform.FindChild("model");
                var animm = cur_model.GetComponent<Animator>();
                animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;

                cur_model.gameObject.AddComponent<Summon_Base_Event>();
                cur_model.Rotate(Vector3.up, 90 - info_data.smshow_face);
                cur_model.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                rogueGO.SetActive(false);
                ava_pet_mon[chaCid] = rogueGO;
            }

            if(info_data.chaCid == chaCid )
            {
                chaMonster_mon = ava_pet_mon[chaCid];
                chaMonster_mon.SetActive(true);
            }
        }

        private ProfessionAvatar createAvatar(int chaIdx)
        {
            if (chaIdx < 0) return null;

            Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            debug.Log("III" + chas.dump());
            Variant curcha = chas[chaIdx];
            

            ChaOutInfo info = new ChaOutInfo();
            GetChaEqpInfo(curcha, 3, out info.bodyID, out info.bodyInte);
            GetChaWingInfo(curcha, out info.wingID, out info.wingInte);
			GetColorInfo(curcha, out info.colorID);
            GetEqpEff(curcha,out info.addeff,out info.activecount);
            int carr = curcha["carr"];
            ProfessionAvatar go = null;
            switch (carr)
            {
                case 2:
                    GetChaEqpInfo(curcha, 6, out info.weaponRID, out info.weaponRInte);
                    go = createWarriorAvatar(info);
                    break;
                case 3:
                    GetChaEqpInfo(curcha, 6, out info.weaponLID, out info.weaponLInte);

                    go = createMagaAvatar(info);
                    break;
                case 4:
                    //go = createArcherAvatar(info);
                    break;
                case 5:
                    GetChaEqpInfo(curcha, 6, out info.weaponRID, out info.weaponRInte);
                    GetChaEqpInfo(curcha, 6, out info.weaponLID, out info.weaponLInte);

                    go = createAssassinAvatar(info);
                    break;
            }
            foreach (Transform tran in go.m_curModel.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_SELFROLE;// 更改物体的Layer层
            }
            return go;
        }

        private void createAvata_mon(int chaIdx)
        {
            //GameObject go = null;
            if (chaIdx < 0) return;
            Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            Variant curcha = chas[chaIdx];
            if (!curcha.ContainsKey("smon"))
            {
                return;
            }
            int mon_id = curcha["smon"];
            if (mon_id <= 0 ) { return; }

            createAssassinAvatar_mon(mon_id);
        }

        private SXML itemXML = null;
        private void GetChaEqpInfo(Variant chaInfo, int tp, out int id, out int intens)
        {
            id = 0;
            intens = 0;

            if (chaInfo == null)
                return;

            Variant fashions = chaInfo["show"];

            Variant eqps = chaInfo["eqp"];
            //if (eqps == null)
             //   return;

            if (itemXML == null)
            {
                itemXML = XMLMgr.instance.GetSXML("item");
            }
            int fashion_E_Type = 11;
            int fashion_W_Type = 12;
            
            foreach (var eqp in eqps._arr)
            {
                int t_id = eqp["tpid"];
                int t_intens = eqp["stag"];
                int t_tp = itemXML.GetNode("item", "id==" + t_id).getInt("equip_type");

                if ( t_tp == tp )
                {
                    id = t_id;
                    intens = t_intens;
                }

                if ( tp ==  3)
                {
                    //if ( t_tp == fashion_E_Type )
                    //{
                    //    id = t_id;
                    //    intens = t_intens;
                    //    return;
                    //}                   
                }
                else if( tp ==  6 )
                {
                   // if ( t_tp == fashion_W_Type )
                   //  {
                        //id = t_id;
                        //intens = t_intens;
                        //return;
                    //}
                }            
            }
            if (fashions != null && fashions.Count > 0)
            {
                //数组0:武器  1:胸甲
                if (tp == 3)
                {
                    if (fashions[1] != 0)
                    {
                        id = fashions[1]._int;
                    }
                }
                else if (tp == 6)
                {
                    if (fashions[0] != 0)
                    {
                        id = fashions[0]._int;
                    }
                }

            }
        }

        private void GetChaWingInfo(Variant chaInfo, out int id, out int intens)
        {
            id = 0;
            intens = 0;

            if (chaInfo == null)
                return;

            if (!chaInfo.ContainsKey("wing"))
                return;

            Variant wing = chaInfo["wing"];
            id = wing["show"]; // show_stage
            //TODO 暂不考虑流光效果
        }

		private void GetColorInfo(Variant chaInfo, out int colorID) {
			a3_EquipModel.getInstance();
			colorID = 0;

			if (chaInfo == null)
				return;

			Variant eqps = chaInfo["eqp"];
			if (eqps == null)
				return;

			foreach (var v in eqps._arr) {
				if (v.ContainsKey("colo")) {
					colorID = v["colo"];
					return;
				}
			}
		}

        private void GetEqpEff(Variant chaInfo, out bool addEff,out int actcount)
        {
            addEff = false;
            actcount = 0;
            if (chaInfo == null) return;
            if (!chaInfo.ContainsKey("acti")) return;
            if (chaInfo["acti"] >= 10)
                addEff = true;
            actcount = chaInfo["acti"];
        }

        private void onCreateCha(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.FLYTXT);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SELECTCHA);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CREATECHA);
        }

        private void OnDrag(GameObject go, Vector2 delta)
        {
            if (chaAvatar != null)
            {
                chaAvatar.m_curModel.Rotate(Vector3.up, -delta.x);
            }
        }

        private void onReturnClick(GameObject go)
        {
            //TODO 
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_SELECTCHA);
        }

        private void onDeleteChar(GameEvent e)
        {
            refreshChooseBtns();

            chaSelectedIndex = -1;
            chaCid = 0;
            chaAvatar = null;
            chaMonster_mon = null;
            foreach (ProfessionAvatar ava in avatars.Values)
            {
                ava.m_curModel.gameObject.SetActive(false);
            }
            foreach (GameObject ava in ava_pet_mon.Values)
            {
                if (ava != null)
                    ava.SetActive(false);
            }
            Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            if (chas.Count > 0)
            {
                onChooseCha(chooseBtns[0].gameObject);
            }
        }

        private void onDeleteChaClick(GameObject go)
        {
            if (chaCid > 0)
            {
                Variant chas = muNetCleint.instance.charsInfoInst.getChas();
                confirmtext.showDeleChar(chas[chaSelectedIndex]);

            }

               
        }

        private void onEnterGameClick(GameObject go)
        {
            BattleProxy.getInstance();
            
            Variant chas = muNetCleint.instance.charsInfoInst.getChas();

            if (chaSelectedIndex < 0 || chaSelectedIndex >= chas.Count)
                return;

            uint cid = chas[chaSelectedIndex]["cid"]._uint;

            UIClient.instance.dispatchEvent(
                GameEvent.Create(UI_EVENT.UI_ACT_SELECT_CHAR, this, GameTools.createGroup("cid", cid)));

            UIClient.instance.dispatchEvent(
                GameEvent.Create(UI_EVENT.UI_ACT_ENTER_GAME, this, GameTools.createGroup("cid", cid)));

            //GameObject audio = GameObject.Find("Audio");
            //GameObject.Destroy(audio);

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SELECTCHA);
            InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.A3_SELECTCHA);
        }

        private void disposeAvatar()
        {
            foreach (ProfessionAvatar ava in avatars.Values)
            {
                GameObject.Destroy(ava.m_curModel.gameObject);
                ava.dispose();
            }

            foreach (GameObject ava in ava_pet_mon.Values)
            {
                if (ava != null)
                    ava.SetActive(false);
            }
            
            m_curProAvatar = null;

            avatars.Clear();
            ava_pet_mon.Clear();

            chaAvatar = null;
            chaMonster_mon = null;
        }

        public override void onClosed()
        {
            async = null;
            standPoint = null;
            itemXML = null;

            disposeAvatar();
        }

        private bool isFashion(int id)
        {
            SXML itemXMl= XMLMgr.instance.GetSXML("item"); ;
            if ( itemXMl == null )
            {
                return false;
            }
            SXML itemNode =  itemXMl.GetNode("item", "id==" + id);
            if ( itemNode == null )
            {
                return false;
            }
            int type = itemNode.getInt("equip_type");
            if ( type == 11 ||  type == 12 )
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    class ChaOutInfo
    {
        public int bodyID; 
        public int bodyInte;
        public int weaponLID;
        public int weaponLInte;
        public int weaponRID;
        public int weaponRInte;
        public int wingID;
        public int wingInte;
		public int colorID;
        public bool addeff;
        public int activecount;
    }
}
