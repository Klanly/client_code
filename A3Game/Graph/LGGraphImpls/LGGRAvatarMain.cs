
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	class LGGRAvatarMain :LGGRAvatar,IObjectPlugin
	{
		private uint _iid = 0;		
			
		public LGGRAvatarMain( muGRClient m):base(m)
		{
		}
		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGGRAvatarMain( m as muGRClient );
        }

		override protected void onSetGameCtrl()
		{
			base.onSetGameCtrl();
			
		}
		 
		override protected void initData( GameEvent e )
		{	
			Variant data = e.data;
			_iid = data["iid"];

			this.avatarid = m_mgr.getAvatarId( data["sex"], data["carr"] );
			Variant avatarConf = m_mgr.getEntityConf( this.avatarid );

			Variant info = new Variant();
			info[ "iid" ] = _iid;
            if (data.ContainsKey("titleConf"))
            {
                info["titleConf"] = data["titleConf"];
            }
            else
            {
                info["titleConf"] = getTitleConf("name", 0, GameTools.createGroup("text", data["name"]));
            }
			//info[ "x" ] = data[ "transX" ];
			//info[ "y" ] = data[ "transY" ];
			//info[ "z" ] = data[ "transZ" ];

			info[ "name" ] = data[ "name" ];
			info[ "equip" ] = data[ "equip" ];
			info[ "avatarConf" ] = avatarConf;

            info["max_hp"] = PlayerModel.getInstance().max_hp;
          //  info["max_mp"] = this.gamectrl.max_mp;
            info["hp"] = PlayerModel.getInstance().hp;
        //    info["mp"] = this.gamectrl.mp;

			info["ori"] = 0;		
			info["ani"] = GameConstant.ANI_IDLE_NORMAL;		
			info["loop"] = true;		

			setPos( 
				(m_gameCtrl as LGAvatar).x,
				(m_gameCtrl as LGAvatar).y
			);
			Vec3 p = getPoss(); 

			info[ "x" ] = p.x;
			info[ "y" ] = p.y;
			info[ "z" ] = p.z;
			info[ "isMain" ] = true;
			this.dispatchEvent( 
				//GameEvent.Create( GAME_EVENT.SPRITE_SET_DATA, this, info ) 
				GameEvent.Createimmedi( GAME_EVENT.SPRITE_SET_DATA, this, info ) 
			);
		}
		
		override public void setPos( float x, float y )  
		{
			base.setPos( x, y );			
		}
		
		override protected bool isRender()
		{
			return _iid > 0;
		}

		private lgSelfPlayer gameCtrl
		{
			get{
				return m_gameCtrl as lgSelfPlayer;
			}
		}

	}
}