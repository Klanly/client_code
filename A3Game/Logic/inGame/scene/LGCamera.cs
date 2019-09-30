using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
using DG.Tweening;
namespace MuGame
{
    public class LGCamera : lgGDBase, IObjectPlugin
    {
        public static LGCamera instance;
        public LGCamera(gameManager m)
            : base(m)
        {
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new LGCamera(m as gameManager);
        }        

        override public void init()
        {
            instance = this;
            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_ENTER_GAME,
                onJoinWorld
            );
        }

        private void onJoinWorld(GameEvent e)
        {           
            this.g_mgr.g_sceneM.dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_CAMERA, this, null)
            );
 
			this.dispatchEvent(GameEvent.Createimmedi(GAME_EVENT.CAMERA_INIT, this, null));
        }

        public void updateMainPlayerPos(bool force=false)
        {
            //if (GRMap.GAME_CAMERA == null)
            //    return;

            //Vector3 vec = lgSelfPlayer.instance.pGameobject.transform.position;
            //Transform tran = GRMap.GAME_CAMERA.transform;
            //if (force || Math.Abs(vec.x - tran.position.x) > 15 || Math.Abs(vec.z - tran.position.z) > 15)
            //    tran.position = vec;
            //else
            //    tran.DOMove(vec, 0.3f);
        }

        public void updateMainPlayerPos(float x, float y, float z)
        {
            if (GRMap.GAME_CAMERA == null)
                return;

            Vector3 vec = new Vector3(x, y, z);
            Transform tran = GRMap.GAME_CAMERA.transform;
            if (Math.Abs(x - tran.position.x) > 15 || Math.Abs(z - tran.position.z) > 15)
                tran.position = vec;
           else
                tran.DOMove(vec, 0.3f);
        }

        public void obj_mask(Vec3 chapos)
        {
            this.dispatchEvent(GameEvent.Createimmedi(GAME_EVENT.SPRITE_OBJ_MASK, this, chapos, true ));
        }
        
    }
}