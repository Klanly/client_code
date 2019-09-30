
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	
	class LGGRCamera : LGGRBaseImpls, IObjectPlugin
	{
		public LGGRCamera(muGRClient m):base(m)
		{
			 
		}
		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGGRCamera( m as muGRClient );
        }
	
		private void updateMainPlayerPos( GameEvent e )
		{
			this.dispatchEvent(	
                GameEvent.Createimmedi( GAME_EVENT.SPRITE_SET_XY, this, e.orgdata ) 
			);
		}
        private void obj_mask(GameEvent e)
        {
            this.dispatchEvent(
                GameEvent.Create(GAME_EVENT.SPRITE_OBJ_MASK, this, e.orgdata)
            );
        }
		

		private lgSelfPlayer selfPlayer
		{
			get
			{
				return g_mgr.g_gameM.getObject( OBJECT_NAME.LG_MAIN_PLAY ) as lgSelfPlayer;
			}
		}

		private LGCamera gameCtrl
		{
			get
			{
				return m_gameCtrl as LGCamera;
			}
		}

		override protected void onSetGameCtrl()
		{			
			this.gameCtrl.addEventListener( GAME_EVENT.SPRITE_DISPOSE, ondispose );

			this.gameCtrl.addEventListener(GAME_EVENT.CAMERA_INIT, onDataInit);	
			this.gameCtrl.addEventListener(GAME_EVENT.SPRITE_SET_XY, updateMainPlayerPos);
            this.gameCtrl.addEventListener(GAME_EVENT.SPRITE_OBJ_MASK, obj_mask);
		}
		override protected void onSetDrawBase()
		{
		}

		private void onDataInit(GameEvent e)
		{			 
			Vec3 v = g_mgr.trans3DPos( 
				selfPlayer.x,
				selfPlayer.y 
			); 

		 
			this.dispatchEvent(
				GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, v )
			);
		}
		private void ondispose( GameEvent e )
		{
			dispose();
		}
	}
}