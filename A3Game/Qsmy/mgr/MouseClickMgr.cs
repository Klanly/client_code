#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif


#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif



using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using Cross;
using GameFramework;
namespace MuGame
{


    class MouseClickMgr : GameEventDispatcher
    {
        public static uint EVENT_TOUCH_GAME_OBJECT = 1;
        public static uint EVENT_TOUCH_UI = 2;
        public Camera curScenceCamera;

        public static MouseClickMgr instance;
        public static void init()
        {
            if (instance == null)
                instance = new MouseClickMgr();
        }

        public bool m_UpdateNearCamNow = false;

        public void onSelectNpc(LGAvatarNpc npc)
        {
            Variant v = npc.viewInfo;

            //npc.doTalk();

            //return;

			//switch (npc.npcid)
			//{
			//    case 90001:
			//        if (FunctionOpenMgr.instance.checkLv(FunctionOpenMgr.FB_STORY, true))
			//        {
			//            ArrayList list = new ArrayList();
			//            //list.Add(TaskModel.getInstance().isNeedToOpenMapId);
			//            //list.Add(TaskModel.getInstance().isNeedToOpenTiLi);
			//            InterfaceMgr.getInstance().open(InterfaceMgr.FB_3D, list);
			//            //   InterfaceMgr.getInstance().open(InterfaceMgr.FB, list);
			//        }
			//        break;
			//    case 90004:


                   
			//        if (FunctionOpenMgr.instance.checkLv(FunctionOpenMgr.MYTH_SHOP, true))
			//            InterfaceMgr.getInstance().open(InterfaceMgr.STICAL);
			//        break;

			//    case 90005:
			//        if (FunctionOpenMgr.instance.checkLv(FunctionOpenMgr.EQUIP_JINJIE, true))
			//        {
			//            InterfaceMgr.getInstance().open(InterfaceMgr.EQUIP);
			//        }
			//        break;
			//    case 90006:
			//        if (FunctionOpenMgr.instance.checkLv(FunctionOpenMgr.FAMILY, true))
			//        {
			//            InterfaceMgr.getInstance().open(InterfaceMgr.FAMILY);
			//        }
			//        break;
			//}
        }



        // Camera ui_main_camera;
        public MouseClickMgr()
        {
            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(new processStruct(update, "MouseClickMgr"));
        }

        public void setCurScenceCamera(Camera ca)
        {
            curScenceCamera = ca;
        }

        public bool m_bTowTouchZoom = false;
        private float m_fTwoTouchDis = 0.0f;
        void update(float tmSlice)
        {
            m_bTowTouchZoom = false;
            float fmsw = 0.0f;
            bool inWin = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;
            //处理镜头的拉近拉远

            if (inWin)
                fmsw = Input.GetAxis("Mouse ScrollWheel");
            else
            {
                if (2 == Input.touchCount)
                {
                    //只要有一个手指不在UI上，就可以处理拉近拉远
                    if (false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                        {
                            Vector2 two_touch_pos_offset = Input.GetTouch(0).position - Input.GetTouch(1).position;
                            if (0.0f == m_fTwoTouchDis)
                            {
                                m_fTwoTouchDis = two_touch_pos_offset.magnitude;
                            }
                            else
                            {
                                float cur_dis = two_touch_pos_offset.magnitude;
                                fmsw = (cur_dis - m_fTwoTouchDis) * 0.0125f;
                                m_fTwoTouchDis = two_touch_pos_offset.magnitude;

                                m_bTowTouchZoom = true;
                            }
                        }
                    }
                }
                else
                {
                    m_fTwoTouchDis = 0.0f;
                }
            }

            if (false) //a3暂时屏蔽，滚轮拉近拉远的处理
            {
                if (fmsw != 0f || m_UpdateNearCamNow)
                {
                    m_UpdateNearCamNow = false;
                    //debug.Log("鼠标的滚轮在动 " + fmsw);
                    GRMap.M_FToCameraNearStep += fmsw * 0.2f;

                    if (GRMap.M_FToCameraNearStep < 0f) GRMap.M_FToCameraNearStep = 0f;
                    if (GRMap.M_FToCameraNearStep > 1f) GRMap.M_FToCameraNearStep = 1f;

                    //读取主角的坐骑的摄像机参数
                    Vector3 near_pos = GRMap.M_VGame_Cam_FARpos;
                    near_pos.x *= lgSelfPlayer.instance.m_fCameraNearXZ;
                    near_pos.y = lgSelfPlayer.instance.m_fCameraNearH;
                    near_pos.z *= lgSelfPlayer.instance.m_fCameraNearXZ;

                    Vector3 near_angle = GRMap.M_VGame_Cam_FARrot;
                    near_angle.x = 30f;

                    GRMap.GAME_CAM_CUR.transform.localPosition = Vector3.Lerp(GRMap.M_VGame_Cam_FARpos, near_pos, GRMap.M_FToCameraNearStep);
                    GRMap.GAME_CAM_CUR.transform.localEulerAngles = Vector3.Lerp(GRMap.M_VGame_Cam_FARrot, near_angle, GRMap.M_FToCameraNearStep);
                }
            }


            bool mouseTouch = Input.GetMouseButtonDown(0);
            if (mouseTouch || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                // if(ui_main_camera==null)
                //     ui_main_camera = GameObject.Find("ui_main_camera").GetComponent<Camera>();
                // Vector3 stwp = ui_main_camera.ScreenToWorldPoint(Input.mousePosition);
                //stwp.z = 0f;

                //if (mouseTouch)
                //{
                //    FightText.play(FightText.MOUSE_POINT, Input.mousePosition, 0);
                //}
                //else
                //{
                //    for (int i = 0; i < Input.touchCount; i++)
                //    {
                //        Touch t = Input.GetTouch(i);
                //        FightText.play(FightText.MOUSE_POINT, t.position, 0);
                //    }
                //}

                if ((inWin && EventSystem.current.IsPointerOverGameObject()) ||
                   (!inWin && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    )
                {

                    dispatchEvent(GameEvent.Create(EVENT_TOUCH_UI, this, EventSystem.current.currentSelectedGameObject));

                    //    debug.Log("当前触摸在UI上");
                }
                else if (GRMap.GAME_CAM_CAMERA != null && GRMap.GAME_CAM_CAMERA.gameObject.active)
                {

                    //   debug.Log("当前触摸不在UI上:" + GRMap.GAME_CAM_CAMERA.active);

                    Ray ray = GRMap.GAME_CAM_CAMERA.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Object3DBehaviour behav = hit.transform.gameObject.GetComponent<Object3DBehaviour>();
                        if (behav != null)
                        {
                            IGraphObject3D obj = behav.obj;
                            GREntity3D hitEnt = obj.helper["$graphObj"] as GREntity3D;
                            hitEnt.dispathEvent(Define.EventType.RAYCASTED, null);
                        }

                    }
                }
                else if (curScenceCamera != null && curScenceCamera.gameObject.active == true)
                {
                    Ray ray = curScenceCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        dispatchEvent(GameEvent.Create(EVENT_TOUCH_GAME_OBJECT, this, hit.transform.gameObject));
                    }
                }

            }
        }

    }

}
