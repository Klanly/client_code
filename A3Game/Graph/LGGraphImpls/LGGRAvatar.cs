
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{


    public class LGGRAvatar : LGGRBaseImpls
    {
        public LGGRAvatar(muGRClient m)
            : base(m)
        {
        }

        //public static IObjectPlugin create( IClientBase m )
        //{           
        //	return new sceneCtrlAvatar( m as muGRClient );
        //}

        private string _ani = "";
        private float _ori;
        private string _avatarid = "";
        private bool _visible = true;

        private float _moveScale = 0.8f;
        private float _moveScaleZ = 0.6f;

        override public void init()
        {


            base.init();
        }

        override protected void onSetGameCtrl()
        {
            m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_SET_DATA, initData);

            m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_DISPOSE, ondispose);
            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_ANI, onAni);
            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_SET_HP, onSetHp);

            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_ADD_TITLE, onAddTitle);
            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_TITLE, onRemoveTitle);
            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_EFF, onRemoveEff);

            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_CHANGE_AVATAR, onChangeAvatar);
            //m_gameCtrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_AVATAR, onRemoveAvatar);
            //selfinfo.addEventListener(GAME_EVENT.SPRITE_MAINPLAYER_MOVE, onMainPlayerMove);
        }

        virtual protected void initData(GameEvent e)
        {
        }
        override protected void onSetDrawBase()
        {
            m_drawBase.addEventListener(GAME_EVENT.SPRITE_ON_CLICK, onClick);
            //m_drawBase.addEventListener(GAME_EVENT.SPRITE_ANI_END, onAniEnd);
        }
        private void RMVListener()
        {
            m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_SET_DATA, initData);

            m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_DISPOSE, ondispose);
            //m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_ANI, onAni);
            //m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_REMOVE_EFF, onRemoveEff);

            //m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_CHANGE_AVATAR, onChangeAvatar);
            //m_gameCtrl.removeEventListener(GAME_EVENT.SPRITE_REMOVE_AVATAR, onRemoveAvatar);

            //selfinfo.removeEventListener(GAME_EVENT.SPRITE_MAINPLAYER_MOVE, onMainPlayerMove);

            m_drawBase.removeEventListener(GAME_EVENT.SPRITE_ON_CLICK, onClick);
            //m_drawBase.removeEventListener(GAME_EVENT.SPRITE_ANI_END, onAniEnd);

        }


        //todo title sprite
        protected Vec2 getRealPos()
        {
            return new Vec2(
                (m_gameCtrl as LGAvatar).x,
                (m_gameCtrl as LGAvatar).y
            );
        }

        protected string getAni()
        {
            if ((int)(m_x * 100) != (int)(m_xMoveto * 100) ||
                (int)(m_y * 100) != (int)(m_yMoveto * 100)
             )
            {
                return (m_gameCtrl as LGAvatar).getMoveAni();
            }

            return (m_gameCtrl as LGAvatar).getAni();
        }
        //protected bool getLoopFlag()
        //{// 
        //    return (m_gameCtrl as LGAvatar).getLoopFlag(); ;
        //}

        //protected float getOri()
        //{
        //    return (m_gameCtrl as LGAvatar).lg_ori_angle;
        //}

        protected bool visibleFlag()
        {
            return (m_gameCtrl as LGAvatar).visibleFlag();
        }

        protected string avatarid
        {
            get
            {
                return _avatarid;
            }
            set
            {
                _avatarid = value;
            }
        }

        private void onClick(GameEvent e)
        {
            Variant einfo = getClickInfo();
            m_gameCtrl.dispatchEvent(
                    GameEvent.Createimmedi(GAME_EVENT.SPRITE_ON_CLICK, this, einfo)
                );
            //onClick();
        }

        //private void onAniEnd(GameEvent e)
        //{
        //    debug.Log("动作播放完了！！！~~~");
        //    m_gameCtrl.dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_ANI_END, this, null)
        //    );
        //}

        virtual protected Variant getClickInfo()
        {
            return null;
        }




        //private void onRemoveEff(GameEvent e)
        //{
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_REMOVE_EFF, this, e.data)
        //    );
        //}


        //private void onAni(GameEvent e)
        //{
        //    Variant aniInfo = e.data;
        //    aniAct(aniInfo["ani"]._str, aniInfo["loop"]._bool);
        //}

        private void ondispose(GameEvent e)
        {
            dispose();
        }
        override protected void onDispose()
        {
            RMVListener();
        }
        //override public void updateProcess(float tmSlice)
        //{
        //    if (!isRender()) return;
        //    updatePos();
        //    updateInfo();
        //}

        private float getScaleVal(float fval, float tval)
        {
            return (1 - _moveScale) * fval + _moveScale * tval;
        }
        private float getScaleValZ(float fval, float tval)
        {
            return (1 - _moveScaleZ) * fval + _moveScaleZ * tval;
        }

        public void updateToTerrainZ()
        {
            Vec2 p = getRealPos();
            setPos(p.x, p.y);

            //UnityEngine.Debug.Log("m_zMoveto::::" + m_zMoveto);

            if (m_x == m_xMoveto &&
                m_y == m_yMoveto &&
                m_z == m_zMoveto)
            {
                return;
            }

            setPosX(m_xMoveto);
            setPosY(m_yMoveto);
            setPosZ(m_zMoveto);
        }


        private void updatePos()
        {
            Vec2 p = getRealPos();
            //debug.Log("更新移动位置 x " + p.x + "   y " + p.y);

            setPos(p.x, p.y);

            if (m_x == m_xMoveto &&
                m_y == m_yMoveto &&
                m_z == m_zMoveto)
            {
                return;
            }

            //DebugTrace.contentAdd( " >>>>>>>>>  m_x["+m_x+"] m_xMoveto["+m_xMoveto+"] m_y["+m_y+"] m_yMoveto["+m_yMoveto+"] m_z["+m_z+"] m_zMoveto["+m_zMoveto+"] xReal["+xReal+"]  yReal["+yReal+"] "  );
            //setPosX( getScaleVal( m_x, m_xMoveto ) );
            //setPosY( getScaleVal( m_y, m_yMoveto ) );
            //setPosZ( getScaleValZ( m_z, m_zMoveto ) );

            setPosX(m_xMoveto);
            setPosY(m_yMoveto);

            float tempz = m_zMoveto - m_z;
            float smooth_z = m_zMoveto;
            if (Math.Abs(tempz) < 0.6f)
            {
                smooth_z = m_z + tempz * 0.125f;
                if (Math.Abs(m_zMoveto - smooth_z) < 0.03125f)
                {
                    smooth_z = m_zMoveto;
                }
            }





            //debug.Log("m_z = " + m_z + "  m_zMoveto = " + m_zMoveto);
            setPosZ(smooth_z);


            ////jason 注释， 这个每帧在这里抛这种事件，可能是有问题的
            ////注释了就不能移动了，因为这里需要将参数传给GRAvatar，来改变位置信息
            this.dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SPRITE_SET_XY, this, null)
            );
        }

        //protected void aniAct(string ani, bool loop)
        //{
        //    debug.Log("动作转发者 hahaahahaha");
            
        //    Variant aniInfo = new Variant();
        //    aniInfo["ani"] = ani;
        //    aniInfo["loop"] = loop;
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_ANI, this, aniInfo)
        //    );
        //}

        private void updateInfo()
        {
            //float currOri = getOri();
            //if (_ori != currOri)
            //{
            //    _ori = currOri;
            //    Variant oriInfo = new Variant();
            //    oriInfo["ori"] = _ori;
            //    this.dispatchEvent(
            //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_ORI, this, oriInfo)
            //    );
            //}

            if (_visible != visibleFlag())
            {
                _visible = visibleFlag();
                Variant vInfo = new Variant();
                vInfo["visible"] = visibleFlag();
                this.dispatchEvent(
                    GameEvent.Createimmedi(GAME_EVENT.SPRITE_SET_VISIBLE, this, vInfo)
                );
            }
        }

        virtual protected bool isRender()
        {
            return false;
        }

        //private void onChangeAvatar(GameEvent e)
        //{//partid			 
        //    changeAvatar(e.data);
        //}

        //private void onRemoveAvatar(GameEvent e)
        //{// { partid: }
        //    removeAvatar(e.data);
        //}

        //private void onMainPlayerMove(GameEvent e)
        //{//  
        //	this.dispatchEvent( e ); 		
        //} 


        //protected void changeAvatar(Variant arr)
        //{//partid
        //    for (int i = 0; i < arr.Count; i++)
        //    {
        //        this.dispatchEvent(
        //            GameEvent.Createimmedi(GAME_EVENT.SPRITE_CHANGE_AVATAR, this, arr[i])
        //        );
        //    }
        //}

        //protected void removeAvatar(Variant arr)
        //{// { partid: }			 
        //    for (int i = 0; i < arr.Count; i++)
        //    {
        //        this.dispatchEvent(
        //            GameEvent.Createimmedi(GAME_EVENT.SPRITE_REMOVE_AVATAR, this, arr[i])
        //        );
        //    }
        //}

        //protected void onSetHp(GameEvent e)
        //{
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_SET_HP, this, e.data)
        //    );
        //}

        //protected void onAddTitle(GameEvent e)
        //{
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_ADD_TITLE, this, e.data)
        //    );
        //}
        //protected void onRemoveTitle(GameEvent e)
        //{
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_REMOVE_TITLE, this, e.data)
        //    );
        //}

        virtual protected Variant getTitleConf(string tp, int showtp = 0, Variant showInfo = null)
        {
            return GameTools.createGroup("tp", tp, "showtp", showtp, "showInfo", showInfo);
        }


        // ==== objs ====
        protected joinWorldInfo selfinfo
        {
            get
            {
                return this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD) as joinWorldInfo;
            }
        }

    }
}