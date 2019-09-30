using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using Cross;
using System.Collections;
using MuGame.role;

namespace MuGame
{
    public class MonsterPlayer : MonsterRole
    {
        public string m_strMPAvatarPath;
        public A3_PROFESSION m_eMPProfession = A3_PROFESSION.None;

        public bool m_bUserSelf = false;
        static public bool ismyself = false;
        public PetBird m_myPetBird = null;
        private int m_petID = -1;
        private int m_petStage = -1;
        public int zhuan = 0;//几转
        public int Pk_state;//攻击类型
        public int m_nKeepSkillCount = 0; //蓄力类技能
        public void Init(string prefab_path, int layer, Vector3 pos)
        {
            base.Init(prefab_path, layer, pos, 0);

            curhp = maxHp = 2000;
        }

        override protected void Model_Loaded_Over()
        {
            m_unLegionID = (uint)LOGION_DEF.LNDF_PLAYER;

            if (m_bUserSelf)
            {
                //SelfHurtPoint shtt = m_curPhy.gameObject.AddComponent<SelfHurtPoint>();
                //shtt.m_selfRole = this;
                ismyself = true;
            }
            else
            {
                //OtherHurtPoint ohtt = m_curPhy.gameObject.AddComponent<OtherHurtPoint>();
                //ohtt.m_otherRole = this;
                ismyself = false;
            }

            //roleName = "我是主角";
            PlayerNameUIMgr.getInstance().show(this);

            m_proAvatar.Init_PA(m_eMPProfession, m_strMPAvatarPath, "l_", m_curGameObj.layer, EnumMaterial.EMT_EQUIP_L, m_curModel);

            //if (m_isMain && viewType != VIEW_TYPE_ALL)
            //    refreshViewType(VIEW_TYPE_ALL);
        }

        public override void FrameMove(float delta_time)
        {
            base.FrameMove(delta_time);
            m_proAvatar.FrameMove();
        }

        protected ProfessionAvatar m_proAvatar = new ProfessionAvatar();
        private float m_fWingTime = 0f;

        public void creatPetAvatar( int carr)
        {
            if (m_myPetBird != null)
            {
                GameObject.Destroy(m_myPetBird.gameObject);
                m_myPetBird = null;
            }
            Transform stop = m_curModel.FindChild("birdstop");
            GameObject path = null;
            GameObject bird = null;
            string str = XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + carr).getString("mod");
            if (carr == 2) {
                GameObject birdPrefab;
                birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject(/*"profession_eagle"*/"profession_"+str);
                GameObject pathPrefab;
                pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
                if (birdPrefab == null || pathPrefab == null)
                    return;
                bird = GameObject.Instantiate(birdPrefab, stop.position, Quaternion.identity) as GameObject;
                path = GameObject.Instantiate(pathPrefab, stop.position, Quaternion.identity) as GameObject;
            }
            else if (carr == 3)
            {
                GameObject birdPrefab;
                birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject(/*"profession_yaque"*/"profession_" + str);
                GameObject pathPrefab;
                pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
                if (birdPrefab == null || pathPrefab == null)
                    return;
                bird = GameObject.Instantiate(birdPrefab, stop.position, Quaternion.identity) as GameObject;
                path = GameObject.Instantiate(pathPrefab, stop.position, Quaternion.identity) as GameObject;
            }
            else if (carr == 5)
            {
                GameObject birdPrefab;
                birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject(/*"profession_yingwu"*/"profession_" + str);
                GameObject pathPrefab;
                pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
                if (birdPrefab == null || pathPrefab == null)
                    return;
                bird = GameObject.Instantiate(birdPrefab, stop.position, Quaternion.identity) as GameObject;
                path = GameObject.Instantiate(pathPrefab, stop.position, Quaternion.identity) as GameObject;
            }

            if (bird == null || path == null)
                return;

            path.transform.parent = stop;
            bird.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            m_myPetBird = bird.AddComponent<PetBird>();            
            m_myPetBird.Path = path;

        }
        public void ChangePetAvatar(int petID, int petStage)
        {
            if (petID == m_petID && petStage == m_petStage)
                return;

            m_petID = petID;
            m_petStage = petStage;

            if (m_myPetBird != null)
            {
                GameObject.Destroy(m_myPetBird.gameObject);
                m_myPetBird = null;
            }

            Transform stop = m_curModel.FindChild("birdstop");
            string petava = A3_PetModel.getInstance().GetPetAvatar(m_petID, 0);
            if (petava == "")
                return;
            GameObject birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + petava);


            GameObject pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
            if (birdPrefab == null || pathPrefab == null)
                return;

            GameObject bird = GameObject.Instantiate(birdPrefab, stop.position, Quaternion.identity) as GameObject;
            GameObject path = GameObject.Instantiate(pathPrefab, stop.position, Quaternion.identity) as GameObject;
            if (bird == null || path == null)
                return;

            path.transform.parent = stop;
            bird.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            m_myPetBird = bird.AddComponent<PetBird>();
            m_myPetBird.Path = path;
        }
        //protected override void FrameMove(float delta_time)
        //{
        //    base.FrameMove(delta_time);
        //}
        protected override void onRefresh_ViewType()
        {
            //if (!m_isMain && m_moveAgent != null && m_moveAgent.enabled)
            //    //m_rshelper.SetNavMeshInfo(avoidancePriority: 20);
            //    //modify
            //    if (viewType == BaseRole.VIEW_TYPE_ALL)
            //    {
            //        set_weaponl(m_roleDta.m_Weapon_LID, m_roleDta.m_Weapon_LFXID); //weapon_l = m_roleDta.m_Weapon_LID;
            //        set_weaponr(m_roleDta.m_Weapon_RID, m_roleDta.m_Weapon_RFXID); //weapon_r = m_roleDta.m_Weapon_RID;
            //        set_wing(m_roleDta.m_WindID, m_roleDta.m_WingFXID);// wing = m_roleDta.m_WindID;
            //        set_body(m_roleDta.m_BodyID, m_roleDta.m_BodyFXID); //body = m_roleDta.m_BodyID;
            //                                                            //添加动画重绑
            //        rebind_ani();
            //        set_equip_color(m_roleDta.m_EquipColorID);
            //        //if (isDead)
            //        //    onDead(true);
            //        //else
            //        //{
            //        //    GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG2) as GameObject;
            //        //    goeff.transform.SetParent(m_curModel, false);
            //        //    GameObject.Destroy(goeff, 2f);
            //        //}

            //    }
            //    else if (viewType == BaseRole.VIEW_TYPE_NAV)
            //    {


            //        m_proAvatar.set_weaponl(-1, 0);
            //        m_proAvatar.set_weaponr(-1, 0);
            //        m_proAvatar.set_wing(-1, 0);
            //        m_proAvatar.set_body(-1, 0);
            //        m_proAvatar.set_equip_color(m_roleDta.m_EquipColorID);
            //    }
        }
        public void set_weaponl(int id, int fxlevel)
        {
            if (viewType != VIEW_TYPE_ALL)
            {
                m_roleDta.m_Weapon_LID = id;
                m_roleDta.m_Weapon_LFXID = fxlevel;
                return;
            }

            m_proAvatar.set_weaponl(id, fxlevel);
        }
        public void set_weaponr(int id, int fxlevel)
        {
            if (viewType != VIEW_TYPE_ALL)
            {
                m_roleDta.m_Weapon_RID = id;
                m_roleDta.m_Weapon_RFXID = fxlevel;
                return;
            }

            m_proAvatar.set_weaponr(id, fxlevel);
        }
        public void set_wing(int id, int fxlevel)
        {
            if (viewType != VIEW_TYPE_ALL)
            {
                m_roleDta.m_WindID = id;
                m_roleDta.m_WingFXID = fxlevel;
                return;
            }

            m_proAvatar.set_wing(id, fxlevel);

            if (m_proAvatar.m_WindID > 0 && m_proAvatar.m_WingObj != null)
            {
                m_curAni.SetFloat(EnumAni.ANI_F_FLY, 1f);
            }
            else
            {
                m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);
            }

        }
        public void set_body(int id, int fxlevel)
        {
            if (viewType != VIEW_TYPE_ALL)
            {
                m_roleDta.m_BodyID = id;
                m_roleDta.m_BodyFXID = fxlevel;
                return;
            }

            m_proAvatar.set_body(id, fxlevel);
        }

        public void rebind_ani()
        {
            m_curAni.Rebind();
            if (m_roleDta.m_WindID > 0)
            {
                m_curAni.SetFloat(EnumAni.ANI_F_FLY, 1f);
            }
            else
            {
                m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);
            }
        }
        public void set_equip_color(uint id)
        {
            if (viewType != VIEW_TYPE_ALL)
            {
                m_roleDta.m_EquipColorID = id;
                return;
            }

            m_proAvatar.set_equip_color(id);
        }
        public void FlyWing(float time)
        {
            if (m_proAvatar.m_Wing_Animstate != null)
            {
                m_proAvatar.m_Wing_Animstate.speed = 1.5f;
                m_fWingTime = time;
            }
        }

        public override void dispose()
        {
            base.dispose();
            m_proAvatar.dispose();
            m_proAvatar = null;

            if (m_myPetBird != null)
            {
                GameObject.Destroy(m_myPetBird.gameObject);
            }
        }

        public int lvl = 0;
        public int combpt = 0;
        public string clanName;
        public void refreshViewData1(Variant v)
        {
            int carr = v["carr"];
            if (v.ContainsKey("eqp"))
            {
                m_roleDta.m_BodyID = 0;
                m_roleDta.m_BodyFXID = 0;
                m_roleDta.m_EquipColorID = 0;
                m_roleDta.m_Weapon_LID = 0;
                m_roleDta.m_Weapon_LFXID = 0;
                m_roleDta.m_Weapon_RID = 0;
                m_roleDta.m_Weapon_RFXID = 0;

                foreach (Variant p in v["eqp"]._arr)
                {
                    a3_ItemData data = a3_BagModel.getInstance().getItemDataById(p["tpid"]);
                    if (data.equip_type == 3)
                    {
                        int bodyid = (int)data.tpid;
                        int bodyFxid = p["intensify"];
                        m_roleDta.m_BodyID = bodyid;
                        m_roleDta.m_BodyFXID = bodyFxid;

                        uint colorid = 0;
                        if (p.ContainsKey("colour"))
                            colorid = p["colour"];
                        m_roleDta.m_EquipColorID = colorid;
                    }
                    if (data.equip_type == 6)
                    {
                        int weaponid = (int)data.tpid;
                        int weaponFxid = p["intensify"];
                        switch (carr)
                        {
                            case 2:
                                m_roleDta.m_Weapon_RID = weaponid;
                                m_roleDta.m_Weapon_RFXID = weaponFxid;
                                break;
                            case 3:
                                m_roleDta.m_Weapon_LID = weaponid;
                                m_roleDta.m_Weapon_LFXID = weaponFxid;
                                break;
                            case 5:
                                m_roleDta.m_Weapon_LID = weaponid;
                                m_roleDta.m_Weapon_LFXID = weaponFxid;
                                m_roleDta.m_Weapon_RID = weaponid;
                                m_roleDta.m_Weapon_RFXID = weaponFxid;
                                break;
                        }
                    }
                }
            }
            if (v.ContainsKey("wing"))
            {
                m_roleDta.m_WindID = v["wing"];
                m_roleDta.m_WingFXID = v["wing"];
            }
            //军衔SS
            if (v.ContainsKey("ach_title"))
            {
                title_id = v["ach_title"];
                isactive = v["title_display"]._bool;
                PlayerNameUIMgr.getInstance().refreshTitlelv(this, title_id);
            }
            if (v.ContainsKey("lvl"))
                lvl = v["lvl"];

            if (v.ContainsKey("combpt"))
            {
                combpt = v["combpt"];
            }
            if (v.ContainsKey("clname"))
            {
                clanName = v["clname"];
            }
            ArrayList arry = new ArrayList();
            arry.Add(m_unCID);
            arry.Add(combpt);
            if (FriendProxy.getInstance() != null)
                FriendProxy.getInstance().reFreshProfessionInfo(arry);

            // if (OtherPlayerMgr._inst.VIEW_PLAYER_TYPE == 1 || m_isMain)
            //refreshViewType(VIEW_TYPE_ALL);
            //onRefresh_ViewType();


            set_weaponl(m_roleDta.m_Weapon_LID, m_roleDta.m_Weapon_LFXID);
            set_weaponr(m_roleDta.m_Weapon_RID, m_roleDta.m_Weapon_RFXID);
            set_wing(m_roleDta.m_WindID, m_roleDta.m_WingFXID);
            set_body(m_roleDta.m_BodyID, m_roleDta.m_BodyFXID);
        }

    }
}
