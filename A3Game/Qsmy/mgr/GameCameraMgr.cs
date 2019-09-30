using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;
using GameFramework;
namespace MuGame
{
    class GameCameraMgr
    {
        processStruct process;
        Transform transUser;

        Transform curTransCamera;
        GameObject curGoCamera;
        CameraAniTempCS curScCamera;
        Animator animator;
        //public void useCamera(String id)
        //{
        //    if (transUser == null)
        //        transUser = FightAniUserTempSC.goUser.transform;
        //    if (process == null)
        //        process = new processStruct(onUpdate, "GameCameraMgr");
        //    clearCurCamera();
        //    GameObject camera = U3DAPI.U3DResLoad<GameObject>(id);
        //    if (camera == null)
        //        return;

        //    curGoCamera = GameObject.Instantiate(camera) as GameObject;
        //    curGoCamera.transform.SetParent(transUser,false);
        //    curScCamera = curGoCamera.transform.FindChild("Camera").GetComponent<CameraAniTempCS>();

        //    if (curScCamera == null)
        //        curScCamera = curGoCamera.transform.FindChild("Camera").gameObject.AddComponent<CameraAniTempCS>();

        //    curTransCamera = curGoCamera.transform.FindChild("Camera");
        //    animator = curGoCamera.transform.FindChild("Camera").GetComponent<Animator>();
        //    animator.speed = 1f;
        //    GRMap.GAME_CAMERA.SetActive(false);
        //    (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
        //}

        public void stop()
        {
            if (curScCamera == null)
                return;

            animator.speed = 0;
        }


        void onUpdate(float s)
        {
            if (curScCamera == null)
                return;

            if (curScCamera.stop)
                return;

            if (curScCamera.lookatUser)
                curTransCamera.LookAt(transUser);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                    clearCurCamera();
            }
        }


        public void clearCurCamera()
        {
            if (curGoCamera == null)
                return;
            curScCamera.clearAllPrelab();
            GRMap.GAME_CAMERA.SetActive(true);
            GameObject.Destroy(curGoCamera);
            curGoCamera = null;
            curScCamera = null;
            animator = null;

            Globle.setTimeScale(1f);
        }

        static private GameCameraMgr instance;
        static public GameCameraMgr getInstance()
        {
            if (instance == null)
                instance = new GameCameraMgr();
            return instance;
        }
    }
}
