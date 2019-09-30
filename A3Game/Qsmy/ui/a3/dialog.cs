using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;
namespace MuGame
{
    class dialog : Window
    {

        public static List<Vector3> lAvPos = new List<Vector3>() { new Vector3(-127.7f, -1.7f, -128f), new Vector3(-131.4f, -4.25f, -128f) };
        private ProfessionAvatar m_proAvatar;


        private static Action m_handle;
        private static List<string> m_desc;
        public static NpcRole m_npc;
        public static GameObject go_npc;
        private int curIdx = 0;

        public static NpcRole fake_npc;
        public static string fake_desc;


        public static dialog instance;

        public GameObject bg;
        override public bool showBG
        {
            get { return false; }
        }
        public static void showTalk(List<string> desc, Action handle, NpcRole npc, bool isfake = false)
        {
            if (desc == null)
                return;

            if (isfake)
            {
                fake_npc = npc;
                fake_desc = desc[0];
            }
            else
            {
                m_handle = handle;
                m_desc = desc;
                m_npc = npc;
            }

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.DIALOG);
        }




        public override void init()
        {
            alain();

            bg = getGameObjectByPath("bg");

            bg.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth * 1.5f, Baselayer.uiHeight * 1.5f);
          
            base.init();
        }

        private GameObject m_npc_obj;
        private Animator m_anim_npc;
        private Animator m_anim_usr;//角色的avatar
        //private GameObject m_skmesh_camera;//拍摄avatar的摄像机
        private GameObject m_npc_camera;
        private GameObject m_player_camera;
        private GameObject m_selfObj;

        public void InitPlayerCam(bool active = false)
        {
            if (m_player_camera == null)
            {
                m_player_camera = GameObject.Instantiate(GAMEAPI.ABUI_LoadPrefab("camera_player_camera")) as GameObject;
            }
            m_player_camera.SetActive(active);
        }
        public void InitNPCCam(bool active = true)
        {
            if (m_npc_camera == null)
            {
                m_npc_camera = GameObject.Instantiate(GAMEAPI.ABUI_LoadPrefab("camera_npc_camera")) as GameObject;
            }
            m_npc_camera.SetActive(active);
        }
        public void initAvatar()
        {
            //GameObject goSkmesh_camera;
            //m_skmesh_camera.transform.localPosition = new Vector3(-129.7f, 1.34f, -124.98f);

            if (m_anim_usr == null)
            {
                //加入主角的显示
                //Variant data = muNetCleint.instance.joinWorldInfoInst.mainPlayerInfo;
                //string model_id = data["sex"]._str;

                m_selfObj = null;
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    m_selfObj = GameObject.Instantiate(obj_prefab, new Vector3(500, 0f, 1f), new Quaternion(0, 0, 0, 0)) as GameObject;
                    
                    m_selfObj.transform.FindChild("model").localRotation = Quaternion.Euler(new Vector3(0, -20, 0));
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    m_selfObj = GameObject.Instantiate(obj_prefab, new Vector3(500, 0f, 1f), new Quaternion(0, 0, 0, 0)) as GameObject;
                    m_selfObj.transform.FindChild("model").localRotation = Quaternion.Euler(new Vector3(0, -20, 0));
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    m_selfObj = GameObject.Instantiate(obj_prefab, new Vector3(500, 0f, 1f), new Quaternion(0, 0, 0, 0)) as GameObject;
                    m_selfObj.transform.FindChild("model").localRotation = Quaternion.Euler(new Vector3(0, -20, 0));
                }

                Transform trans = m_selfObj.transform.FindChild("model");

                trans.localScale = new Vector3(1.3f, 1.3f, 1.3f);

                m_proAvatar = new ProfessionAvatar();

                m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath , "h_", EnumLayer.LM_FX, EnumMaterial.EMT_EQUIP_H, trans);

                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
                m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
                m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
                m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
                m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
                m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());
                m_anim_usr = trans.GetComponent<Animator>();

                //m_selfObj.transform.eulerAngles = Vector3.zero;


                Transform[] listT = m_selfObj.GetComponentsInChildren<Transform>();
                foreach (Transform tran in listT)
                {
                    tran.gameObject.layer = EnumLayer.LM_DEFAULT;
                }

                if (SelfRole._inst is P3Mage)
                {
                    Vector3 centervec = m_proAvatar.m_BodySkin.localBounds.center;
                    centervec.y = 1.3f;
                    Vector3 sizevec = m_proAvatar.m_BodySkin.localBounds.size;
                    m_proAvatar.m_ShoulderSkin .localBounds = m_proAvatar.m_BodySkin.localBounds = new Bounds(centervec, sizevec);

                    // usr.m_BodySkin.bounds
                }


                //GameObject skmesh_usr = GameObject.Instantiate(SelfRole._inst.m_curModel.gameObject) as GameObject;
                //m_anim_usr = skmesh_usr.GetComponent<Animator>();
                //Destroy(skmesh_usr.GetComponent<NavMeshAgent>());
                //skmesh_usr.transform.position = new Vector3(65535f, 1f, 1f);
                //skmesh_usr.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                //skmesh_usr.transform.eulerAngles = Vector3.zero;
            }


            NpcRole npc = m_npc;
            if (fake_npc != null)
                npc = fake_npc;

            if (m_npc_obj == null && npc != null)
            {
                GameObject skmesh_npc = GameObject.Instantiate(npc.gameObject) as GameObject;
                bool build_npc_success = true;
                if( skmesh_npc.transform.childCount > 0 )
                {
                    Transform tf_model = skmesh_npc.transform.GetChild(0).FindChild("model");
                    if(tf_model != null)
                    {
                        tf_model.localPosition = Vector3.zero;
                        m_anim_npc = tf_model.GetComponent<Animator>();
                        m_npc_obj = skmesh_npc;
                        Destroy(skmesh_npc.GetComponent<NavMeshAgent>());
                        skmesh_npc.transform.localPosition = npc.talkOffset;
                        skmesh_npc.transform.localScale = npc.talkScale;
                        skmesh_npc.transform.eulerAngles = Vector3.zero;


                        debug.Log(":::::::::::::::::::::::::::::::::::::::::::::" + skmesh_npc.transform.position + " " + skmesh_npc.gameObject.name);

                        Transform[] listTN = skmesh_npc.GetComponentsInChildren<Transform>();
                        foreach (Transform tran in listTN)
                        {
                            tran.gameObject.layer = EnumLayer.LM_DEFAULT;
                        }

                        NpcRole npcrole = skmesh_npc.GetComponent<NpcRole>();
                        if (npcrole != null)
                            Destroy(npcrole);
                    }
                    else
                    {
                        build_npc_success = false;
                    }
                }
                else
                {
                    build_npc_success = false;
                }

                if( build_npc_success == false )
                {
                    Debug.LogError("创建NPC失败： " + skmesh_npc.name);
                    GameObject.Destroy(skmesh_npc);
                }

            }
        }

        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        public static string curType;
        public static string curDesc;
        public void showDesc(string desc)
        {
            int idx = desc.IndexOf(":");

            if (idx < 0)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
                return;
            }


            curType = desc.Substring(0, idx);
            curDesc = desc.Substring(idx + 1, desc.Length - idx - 1);


            if (curType == "0" || curType == "1")
            {

                OnShowAvatar(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NPC_TALK);
            }
            else if (curType == "2" || curType == "3")
            {
                OnShowAvatar(false);
                //TODO 其他的对话设置
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NPC_TASK_TALK);
            }
            else if (curType == "-1")
            {
                OnShowAvatar(false);
                //TODO 打开功能窗口
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NPC_TASK_TALK);
            }
            else if (curType == "newbie")
            {

                continuedo = !isLastDesc;
                InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
                NewbieTeachMgr.getInstance().add(curDesc, -1);

            }

        }



        //显示隐藏avatar
        private void OnShowAvatar(bool show)
        {
            if (show)
            {
                //创建一个显示UI上角色的摄像机
                if(m_npc_camera == null)
                    m_npc_camera = GameObject.Instantiate(GAMEAPI.ABUI_LoadPrefab("camera_npc_camera")) as GameObject;
            }
            m_anim_usr.gameObject.SetActive(show);

            if (m_npc_obj != null)
                m_npc_obj.gameObject?.SetActive(show);
        }

        public void closeSubWins()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.NPC_TALK);
            InterfaceMgr.getInstance().close(InterfaceMgr.NPC_TASK_TALK);
        }



        public void showRole(bool usr)
        {
            if (usr)
            {
                m_anim_usr.transform.position = lAvPos[0];
                if (m_npc_obj != null)
                {
                    m_npc_obj.transform.position = new Vector3(65535f, 1f, 1f);
                    m_npc_obj.transform.localRotation = Quaternion.Euler(new Vector3(0, -20, 0));
                }
            }
            else
            {
                if (m_anim_usr != null)
                    m_anim_usr.transform.position = new Vector3(500f, 1f, 1f);
                if (m_npc_obj != null)
                    m_npc_obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 20, 0));

                if (m_npc_obj != null)
                {
                    if (fake_npc != null)
                        m_npc_obj.transform.position = fake_npc.talkOffset;
                    else
                        m_npc_obj.transform.position = m_npc.talkOffset;
                    m_anim_npc.SetTrigger("talk");
                }
            }
        }



        public static void next()
        {
            if(instance != null)
                instance.doNext();
        }

        public static bool isLastDesc = false;
        public void doNext()
        {
            if (fake_npc != null)
            {
                fake_npc = null;
                InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
                return;
            }

            closeSubWins();

            if ((curIdx + 1) >= m_desc.Count)
                isLastDesc = true;
            else
                isLastDesc = false;


            if (curIdx >= m_desc.Count)
            {
                if (m_handle != null)
                    m_handle();
                InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
            }
            else
            {
                showDesc(m_desc[curIdx]);
                curIdx++;
            }
        }

        public static bool continuedo = false;

        public override void onShowed()
        {
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.OnDragOut();
            instance = this;
            if (fake_npc != null)
            {
                InterfaceMgr.setUntouchable(bg);
                initAvatar();
                curDesc = fake_desc;
                OnShowAvatar(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NPC_TALK);

            }
            else
            {
                if (!continuedo)
                    curIdx = 0;
                continuedo = false;

                InterfaceMgr.setUntouchable(bg);
                initAvatar();
                // EventTriggerListener.Get(gameObject).onClick = onCLick;
                doNext();
            }


            base.onShowed();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_STORY);
            if (a3_chapter_hint.instance != null && npctalk.instance != null) { npctalk.instance.MinOrMax(false); }
        }

        public override void onClosed()
        {
            closeSubWins();

            if( m_proAvatar != null )
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            m_handle = null;
            instance = null;
            //if (m_skmesh_camera != null)
            //{
            //    GameObject.Destroy(m_skmesh_camera);
            //    m_skmesh_camera = null;
            //}
            if (m_player_camera != null)
            {
                GameObject.Destroy(m_player_camera);
                m_player_camera = null;
            }
            if (m_npc_camera != null)
            {
                GameObject.Destroy(m_npc_camera);
                m_npc_camera = null;
            }
            if (m_anim_usr != null)
            {
                Destroy(m_selfObj);
                m_selfObj = null;
                m_anim_usr = null;
            }

            if (m_npc_obj != null)
            {
                Destroy(m_npc_obj.gameObject);
                m_npc_obj = null;
            }
            EventTriggerListener.Get(gameObject).clearAllListener();
            base.onClosed();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        }

        //public void onCLick(GameObject go)
        //{
        //    doNext();
        //}
        public void GetNPCCamRdy()
        {
            if(m_player_camera != null)
                m_player_camera.SetActive(false);
            else
                dialog.instance.InitPlayerCam();
            if (m_npc_camera != null)
                m_npc_camera.SetActive(true);
            else
                dialog.instance.InitNPCCam();
        }


        public void GetPlayerCamRdy()
        {
            if (m_player_camera != null)
                m_player_camera.SetActive(true);
            else
                dialog.instance.InitPlayerCam();
            if (m_npc_camera != null)
                m_npc_camera.SetActive(false);
            else
                dialog.instance.InitNPCCam(false);
        }
    }
}
