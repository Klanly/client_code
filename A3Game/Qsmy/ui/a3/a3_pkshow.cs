using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
using DG.Tweening;


namespace MuGame
{
    class a3_pkshow:Window
    {

        GameObject  sence_camera,
                    sence_avater1,
                    sence_avater2,
                    secve_obj;
        ProfessionAvatar m_proAvatar1, m_proAvatar2;


        GameObject image;
        public static a3_pkshow _instance;


        int llid=-1;
        string matchname ="";
        public override void init()
        {
            image = getGameObjectByPath("Image");
        }


        public override void onShowed()
        {

            if (uiData!=null)
            {
                FriendProxy.getInstance().sendgetplayerinfo((uint)uiData[0]);
                getComponentByPath<Text>("selfname").text = PlayerModel.getInstance().name;
                getComponentByPath<Text>("selfname/selflv").text = PlayerModel.getInstance().up_lvl + ContMgr.getCont("zhuan") + PlayerModel.getInstance().lvl + ContMgr.getCont("ji");
                getComponentByPath<Text>("selfzdl").text = PlayerModel.getInstance().combpt.ToString();
                if ((string)uiData[1] != "")
                    matchname = (string)uiData[1];
               // getComponentByPath<Text>("matchname/matchname").text = (string)uiData[1];
                llid = (int)uiData[2];
            }
            _instance = this;
            getGameObjectByPath("ig_bg_bg").SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            showPKSence();
            createAvatar_self();
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
            //image.transform.DOScale(new Vector3(1, 1, 1), 2);
        }
        void close()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKSHOW);
            CancelInvoke("close");
        }
        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            desPKSence();
            desAvatar();
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
           // image.transform.DOScale(new Vector3(0, 0, 0), 1);
        }

        void Update()
        {
            if (m_proAvatar1 != null) m_proAvatar1.FrameMove();
            if (m_proAvatar2 != null) m_proAvatar2.FrameMove();
            if(myselfload&&matchload)
            {
                playskill_start();
            }

        }

        int carr = 0;
        int show_wing = 0;
        uint cuid = 0;

        Dictionary<int, a3_BagItemData> Equips = new Dictionary<int, a3_BagItemData>();
        public Dictionary<int, a3_BagItemData> active_eqp = new Dictionary<int, a3_BagItemData>();

        
       public     void GetInfo(GameEvent e)
        {
            System.Random ro = new System.Random();
            int num = PlayerModel.getInstance().combpt;
            int minnum = num - 200>0?num-200:num;
            int maxnum = num - 100>0?num-100:num;
            Variant data = e.data;
            int a = ro.Next(minnum, maxnum);
            print("随机数生成：" + a);
            getComponentByPath<Text>("matchname").text =matchname==""?data["name"]._str : matchname;
            getComponentByPath<Text>("matchname/matchlv").text = matchname == "" ? data["zhuan"]._str + ContMgr.getCont("zhuan") + data["lvl"]._str + ContMgr.getCont("ji") : PlayerModel.getInstance().up_lvl + ContMgr.getCont("zhuan") + PlayerModel.getInstance().lvl + ContMgr.getCont("ji");
            getComponentByPath<Text>("matchzdl").text = matchname == "" ? data["combpt"]._str : a.ToString();
            cuid = data["cid"];
            carr = data["carr"];
            if (data.ContainsKey("show_wing"))
                show_wing = data["show_wing"];

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

            createAvatar_match();
        }
        public bool isactive_eqp(a3_BagItemData data)
        {
            if (data.equipdata.attribute == 0)
                return false;
            int needTpye_act = a3_EquipModel.getInstance().eqp_type_act[data.confdata.equip_type];
            if (!Equips.ContainsKey(needTpye_act))
                return false;
            int needatt = a3_EquipModel.getInstance().eqp_att_act[data.equipdata.attribute];
            if (Equips[needTpye_act].equipdata.attribute == needatt)
                return true;
            else
                return false;
        }
        ///////////场景
        void showPKSence()
        {

            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);
            if (secve_obj == null)
            {
                GameObject obj_prefab;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_pk_camera");
                sence_camera = GameObject.Instantiate(obj_prefab) as GameObject;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_pk_sence");
                secve_obj = GameObject.Instantiate(obj_prefab) as GameObject;

                foreach (Transform tran in secve_obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta")
                        tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                    else
                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
            }
        }
        void desPKSence()
        {

            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            if (secve_obj != null) GameObject.Destroy(secve_obj);
            if (sence_camera != null) GameObject.Destroy(sence_camera);
        }
        ///////////人物
        bool myselfload = false;
        bool matchload = false;
        /*自己*/
        void createAvatar_self()
        {
            GameObject obj_prefab;
            A3_PROFESSION eprofession = A3_PROFESSION.None;
            if (SelfRole._inst is P2Warrior)
            {
                eprofession = A3_PROFESSION.Warrior;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");//-213.44f, 0.1f, 0.8f
                sence_avater1 = GameObject.Instantiate(obj_prefab, new Vector3(-141.2f, 7.86f, 24.88f), new Quaternion(0, 0, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P3Mage)
            {
                eprofession = A3_PROFESSION.Mage;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");//-213.48f, 0.19f, 0.5f
                sence_avater1 = GameObject.Instantiate(obj_prefab, new Vector3(-140.88f, 7.86f, 24.88f), new Quaternion(0, 0, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P5Assassin)
            {
                eprofession = A3_PROFESSION.Assassin;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");//-213.46f, 0.12f, 1.2f
                sence_avater1 = GameObject.Instantiate(obj_prefab, new Vector3(-141.2f, 7.86f, 24.88f), new Quaternion(0, 0, 0, 0)) as GameObject;
            }
            else
            {
                return;
            }
            foreach (Transform tran in sence_avater1.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
            }
            Transform cur_model = sence_avater1.transform.FindChild("model");
            if (SelfRole._inst is P3Mage)
            {
                Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                light_fire.transform.SetParent(cur_r_finger1, false);
            }
            m_proAvatar1 = new ProfessionAvatar();
            string h_or_l;
            int type = SelfRole._inst.get_bodyid() != 0 ? a3_BagModel.getInstance().getEquipTypeBytpId(SelfRole._inst.get_bodyid()) : 0;
            if (type == 11 || type == 12)
            {
                h_or_l = "l_";
            }
            else
            {
                h_or_l = "h_";
            }

            m_proAvatar1.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, h_or_l, EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
            if (m_proAvatar1 != null)
            {
                m_proAvatar1.clear_oldeff();
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar1.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
            }
            if (m_proAvatar1 != null)
                m_proAvatar1.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));

            m_proAvatar1.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
            m_proAvatar1.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
            m_proAvatar1.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
            m_proAvatar1.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
            m_proAvatar1.set_equip_color(SelfRole._inst.get_equip_colorid());
            myselfload = true;

        }
        /*对手*/
        void createAvatar_match()
        {
            
            if (sence_avater2 == null)
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                string m_strAvatarPath = "";
                string equipEff_path = "";
                if (carr == 2)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    sence_avater2 = GameObject.Instantiate(obj_prefab, new Vector3(-143.82f, 7.86f, 24.88f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_warrior_";
                    equipEff_path = "Fx_armourFX_warrior_";
                    //sence_avater2.transform.eulerAngles = new Vector3(3.13f, 34.793f, 5.908f);

                }
                else if (carr == 3)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    sence_avater2 = GameObject.Instantiate(obj_prefab, new Vector3(-143.82f, 7.86f, 24.88f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_mage_";
                    equipEff_path = "Fx_armourFX_mage_";
                    //sence_avater2.transform.eulerAngles = new Vector3(3.197f, 29.584f, 6.8f);
                }
                else if (carr == 5)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    sence_avater2 = GameObject.Instantiate(obj_prefab, new Vector3(-143.82f, 7.86f, 24.88f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_assa_";
                    equipEff_path = "Fx_armourFX_assa_";
                    //sence_avater2.transform.eulerAngles = new Vector3(1.497f, 32.282f, 5.623f);
                }
                else
                {
                    return;
                }


                Transform cur_model = sence_avater2.transform.FindChild("model");
               // cur_model.Rotate(Vector3.up, 200f);
                //cur_model.rotation = new Quaternion(-3.135f,-130f,-8.074f,1);
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
                m_proAvatar2 = new ProfessionAvatar();
                m_proAvatar2.Init_PA(eprofession, m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, equipEff_path);
                if (active_eqp.Count >= 10)
                {
                    m_proAvatar2.set_equip_eff(bodyid, true);
                }
                m_proAvatar2.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(active_eqp.Count));
                m_proAvatar2.set_body(bodyid, bodyFxid);
                m_proAvatar2.set_weaponl(m_Weapon_LID, m_Weapon_LFXID);
                m_proAvatar2.set_weaponr(m_Weapon_RID, m_Weapon_RFXID);
                m_proAvatar2.set_wing(show_wing, show_wing);
                m_proAvatar2.set_equip_color(colorid);
                foreach (Transform tran in sence_avater2.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                matchload = true;
            }
        }
        void desAvatar()
        {
            if (m_proAvatar1 != null)
            {
                m_proAvatar1.dispose();
                m_proAvatar1 = null;
            }
            if (m_proAvatar2 != null)
            {
                m_proAvatar2.dispose();
                m_proAvatar2 = null;
            }

            if (sence_avater1 != null) GameObject.Destroy(sence_avater1);
            if (sence_avater2 != null) GameObject.Destroy(sence_avater2);
        }




        void playskill_start()
        {

            CancelInvoke("playskill");
            Invoke("playskill", 1);
            matchload = false;
            myselfload = false;
        }
        void playskill( )
        {
            
            if (sence_avater2 != null)
                sence_avater2.transform.FindChild("model").GetComponent<Animator>().SetBool("play", true);

            if (sence_avater1 != null)
            {
                sence_avater1.transform.FindChild("model").GetComponent<Animator>().SetBool("play", true);
               // stateinfo1 = sence_avater1.transform.FindChild("model").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            }

            CancelInvoke("playskill");
             CancelInvoke("gotomap");
             Invoke("gotomap", 4f);
        }



        void gotomap()
        {
            sence_avater1.transform.FindChild("model").GetComponent<Animator>().SetBool("play", false);
            sence_avater2.transform.FindChild("model").GetComponent<Animator>().SetBool("play", false);
            CancelInvoke("gotomap");

            if(llid!=-1)
            {
                a3_sportsProxy.getInstance().GotoJJCmap(llid);
            }


        }

    }
}
