
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{

	class LGGRMap : LGGRBaseImpls, IObjectPlugin
	{
		 
		private bool _mapSpriteCreatFlag = false;
        static public LGGRMap instance;
        static public bool haveListener = false;

        public LGGRMap( muGRClient m):base(m)
		{
		}
		public static IObjectPlugin create( IClientBase m )
        {
            if(instance == null)
            {
                instance = new LGGRMap(m as muGRClient);
            }
                
            return instance;
        }

		override protected void onSetGameCtrl()
		{
            if (haveListener)
                return;

			this.gameCtrl.addEventListener( GAME_EVENT.MAP_INIT, onDataInit );	
			this.gameCtrl.addEventListener( GAME_EVENT.MAP_SIZE_CHANGE, onViewSizeChange );
			this.gameCtrl.addEventListener( GAME_EVENT.MAP_CHANGE, onChangeMap );
			this.gameCtrl.addEventListener( GAME_EVENT.MAP_LINK_ADD, onLinkAdd );
            this.gameCtrl.addEventListener( GAME_EVENT.MAP_ADD_FLY_EFF, onAddFlyEff );

            haveListener = true;

        }  
		override protected void onSetDrawBase()
		{
			//addEventListener(GAME_EVENT.MAP_LOAD_READY, onLoadReady );
		}

		private LGMap gameCtrl
		{
			get{
				return m_gameCtrl as LGMap;
			}
		}
		
		//private void onLoadReady( GameEvent e )
		//{
		//	this.g_mgr.dispatchEvent( e );	
		//}

		//第一次初始化游戏的时候，进来初始化地图
		private void onDataInit( GameEvent e )
		{
			this.dispatchEvent( 
				GameEvent.Create( GAME_EVENT.SPRITE_SET_DATA, this, e.data ) 
			);	
		}
		private void onChangeMap( GameEvent e )
		{ 
			this.dispatchEvent( 
				GameEvent.Create( GAME_EVENT.MAP_CHANGE, this, e.data ) 
			);
		}
		private void onLinkAdd( GameEvent e )
		{ 
			this.dispatchEvent( 
				GameEvent.Create( GAME_EVENT.MAP_LINK_ADD, this, e.data ) 
			);
		}

        private Variant flyEff = new Variant();
        private void onAddFlyEff(GameEvent e)
        {
            flyEff.pushBack(e.data);

            this.dispatchEvent(
                GameEvent.Create(GAME_EVENT.MAP_ADD_FLY_EFF, this, e.data)
            );
        }

		//private Variant _infoLast;
		override public void updateProcess( float tmSlice )
		{

			//if( !isRender() ) return;
			
			//Variant info = this.gameCtrl.getViewInfo();
			//Variant infoLast = getPos();
			 
			
			//if( !infoLast || (infoLast["x"] != info["x"]) || (infoLast["y"]!= info["y"]) ) 
			//{				 
			//	this.dispatchEvent( 
			//		GameEvent.Createimmedi( GAME_EVENT.SPRITE_SET_XY, this, info ) 
			//	);
			//	setPos( info );
			//}
			
		}
		protected bool isRender()
		{
			return _mapSpriteCreatFlag && 
				( this.gameCtrl != null) && 
				this.gameCtrl.showFlag();
		}
		//e.data { w:, h:}
		public void onViewSizeChange(GameEvent e)
		{
			//drawBaseActFun( "viewSizeChange", e.data );
			this.dispatchEvent( 
				GameEvent.Createimmedi( GAME_EVENT.SPRITE_SIZE_CHANGE, this, e.data ) 
			);
		}
		private void ondispose( GameEvent e )
		{
			dispose();
		}
	}
}

