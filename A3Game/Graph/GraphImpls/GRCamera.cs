
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
using DG.Tweening;
namespace MuGame
{
    class GRCamera : GRBaseImpls, IObjectPlugin
    {
        public GRCamera(muGRClient m)
            : base(m)
        {

        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new GRCamera(m as muGRClient);
        }
        override public void init()
        {
        }

        override protected void onSetSceneCtrl()
        {

            m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_DATA, setData);

          //  m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_XY, upDateView);
            //m_ctrl.addEventListener( GAME_EVENT.SPRITE_SET_Z, upDateZ);
            m_ctrl.addEventListener(GAME_EVENT.SPRITE_OBJ_MASK, objmask);
        }

        //void onUpdate(float s)
        //{
        //    //Transform tran = GRMap.GAME_CAMERA.transform;

        //    //float tempx = needCameraX - tran.position.x;
        //    //float tempz = needCameraZ - tran.position.z;

        //    //float smooth_x = needCameraX;
        //    //float smooth_z = needCameraZ;
        //    //float absX = Math.Abs(tempx);
        //    //if (absX < 30f)
        //    //{
        //    //    smooth_x = tran.position.x + ((absX == tempx) ? 0.5f : -0.5f);
        //    //    if (Math.Abs(needCameraX - smooth_x) < 0.21f)
        //    //    {
        //    //        smooth_x = needCameraX;
        //    //    }
        //    //}

        //    //float absZ = Math.Abs(tempz);
        //    //if (absZ < 30f)
        //    //{
        //    //    smooth_z = tran.position.y + ((absZ == tempz) ? 0.05f : -0.05f);
        //    //    if (Math.Abs(needCameraZ - smooth_z) <0.21f)
        //    //    {
        //    //        smooth_z = needCameraZ;
        //    //    }
        //    //}



        //    //  GRMap.GAME_CAMERA.transform.position = new UnityEngine.Vector3(smooth_x, needCameraY, smooth_z);
        //}

        override protected void onSetGraphImpl()
        {
            //    os.sys.addGlobalEventListener(Define.EventType.MOUSE_DOWN, onGlobalMouseDown);
        }

        private void onGlobalMouseDown(Cross.Event e)
        {
            //MouseEvent me = e as MouseEvent;
            //if (os.sys.execute)
            //{
            //    GREntity3D entity = m_cam.rayCast(new Vec3(me.globalX, me.globalY, 0));
            //}
        }
        //private bool _initflag = false;
        private GRCamera3D m_cam
        {
            get
            {

                return m_gr as GRCamera3D;
            }
        }

        private void setData(GameEvent e)
        {
            Vec3 einfo = e.orgdata as Vec3;
            updateXYZ(einfo.x, einfo.y, einfo.z);
        }
        private void upDateView(GameEvent e)
        {
            Vec3 einfo = e.orgdata as Vec3;
            updateXYZ(einfo.x, einfo.y, einfo.z);
        }

        private Vec3 _lookat = new Vec3();
        private void upDateZ(GameEvent e)
        {
            Variant einfo = e.data;
            float posZ = einfo["z"] + GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Z;

            m_cam.y = posZ;
        }

        private void objmask(GameEvent e)
        {
            m_cam.obj_mask(e.orgdata as Vec3, m_cam.pos);
        }

        //data { x:, y: }
        private void updateXYZ(float unity_x, float unity_y, float unity_z)
        {
            ////更新摄像机的位置
            //float posX = unity_x + GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_X;
            //float posY = unity_y + GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Y;
            //float posZ = unity_z + GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_Z;

            //debug.Log("场景中摄像机的位置 x=" + GameConstant.CAMERA_POSTION_OFFSET_FROM_CHAR_X);

            //if (m_cam == null || GameConstant.REFRESH_CAMERA_ROT_AND_FOV)
            //{
            //    if (m_cam == null)
            //    {
            //        setGraphImpl( g_mgr.getGraphCamera() );
            //    }

            //    GameConstant.REFRESH_CAMERA_ROT_AND_FOV = false;

            //    m_cam.fov = GameConstant.CAMERA_FIELD_VIEW;

            //    float rotX = GameConstant.CAMERA_ROTATION_X;
            //    float rotY = GameConstant.CAMERA_ROTATION_Y;
            //    float rotZ = GameConstant.CAMERA_ROTATION_Z;

            //    updataRotation( rotX, rotY, rotZ  );
            //}

            //m_cam.pos = new Vec3(posX, posY, posZ);

            //Vector3 vec = new Vector3(unity_x, unity_y, unity_z);
            //Transform tran = GRMap.GAME_CAMERA.transform;
            //if (Math.Abs(unity_x - tran.position.x) > 10 || Math.Abs(unity_z - tran.position.z) > 10)
            //    tran.position = vec;
            //else
            //    tran.DOMove(vec, 0.3f);
            //else
            //    GRMap.GAME_CAMERA.transform.position = new Vector3(posX, posY, posZ);




            //Variant p = new Variant();
            //p._val = m_cam.pos;
            //this.m_ctrl.g_mgr.dispatchEvent(
            //    GameEvent.Createimmedi(  
            //        GAME_EVENT.SPRITE_GR_CAMERA_MOVE, 
            //        this, 
            //        p 
            //    )
            //);
        }

        float needCameraX = 0;
        float needCameraY = 0;
        float needCameraZ = 0;

        private void updataRotation(float x, float y, float z)
        {
            m_cam.rot = new Vec3(x, y, z);
        }
        private void onLookAt(GameEvent e)
        {
            //var data:Object = e.data;
            //_map.upDateView( data );
        }
        //public GRCamera3D cam
        //{
        //	get{
        //		return m_cam;
        //	}
        //}
        override public void dispose()
        {
            this.g_mgr.deleteEntity(this.m_cam);
        }

        override public void updateProcess(float tmSlice)
        {

        }
    }
}
