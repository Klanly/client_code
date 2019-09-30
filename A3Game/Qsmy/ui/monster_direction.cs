//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameFramework;
//using UnityEngine;
//using UnityEngine.UI;
//using Cross;
//namespace MuGame
//{
//    class monster_direction : FloatUi
//    {
//        RectTransform rect;
//        processStruct process;

//        bool hided = false;

//        public override void init()
//        {
//            process = new processStruct(onUpdate, "fb_main");
//            rect = getComponentByPath<RectTransform>("arrow");
//        }

//        public override void onShowed()
//        {
//            InterfaceMgr.setUntouchable(rect.gameObject);
//            (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
//            base.onShowed();
//        }

//        public override void onClosed()
//        {
//            role = null;
//            (CrossApp.singleton.getPlugin("processManager") as processManager).removeProcess(process);
//            base.onClosed();
//        }

//        void showIt()
//        {
//            if (!hided)
//                return;
//            hided = false;
//            rect.anchoredPosition = Vector2.zero;
//        }

//        void hideIt()
//        {
//            if (hided)
//                return;
//            hided = true;
//            rect.anchoredPosition = new Vector2(65535f, 65535f);
//        }

//        LGAvatarGameInst role;
//        void onUpdate(float s)
//        {
//            if (InterfaceMgr.ui_Camera_cam == null || GRMap.GAME_CAM_CAMERA == null)
//                return;

//            if (role == null || role.IsDie())
//                role = LGMonsters.instacne.getNearMon();




//            if (role == null)
//            {
//                // debug.Log("role == null");
//                hideIt();
//                return;
//            }


//            try
//            {
//                if (role.pGameobject == null)
//                {
//                    role = null;
//                    hideIt();
//                    return;
//                }

//            }
//            catch (System.Exception ex)
//            {
//                role = null;
//                hideIt();
//                return;
//            }

//            LGAvatarGameInst avatar = role;
//            float len = getDistance(avatar);
//            //debug.Log("len == " + len + " " + hided + " " + rect.anchoredPosition);
//            if (avatar != null && !avatar.IsDie() && len > 300)
//            {
//                showIt();
//                rotationToPt(avatar);
//            }
//            else
//            {
//                hideIt();
//            }
//        }

//        public float getDistance(LGAvatarGameInst avatar)
//        {
//            return Math.Abs(lgSelfPlayer.instance.x - avatar.x) + Math.Abs(lgSelfPlayer.instance.y - avatar.y);
//        }

//        public void rotationToPt(LGAvatarGameInst av)
//        {
//            if (av == null)
//                return;

//            //debug.Log(":1:" + InterfaceMgr.cameraText);
//            //debug.Log(":2:" + GRMap.GAME_CAM_CAMERA);
//            //debug.Log(":3:" + av.gameObj);

//            Vector3 vec0 =GRMap.GAME_CAM_CAMERA.WorldToScreenPoint(av.gameObj.transform.position);
//            Vector3 vec1 = GRMap.GAME_CAM_CAMERA.WorldToScreenPoint(lgSelfPlayer.instance.pGameobject.transform.position);
//            float a = (float)((Math.Atan2(vec0.y - vec1.y, vec0.x - vec1.x)) * (180 / Math.PI)) - 90;

//            rect.eulerAngles = new Vector3(0, 0, a); ;
//        }

//    }
//}
