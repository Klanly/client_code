using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	class sceneCtrlAvatarNpc : LGGRAvatar, IObjectPlugin
	{
		private uint _npcid = 0;		
		public sceneCtrlAvatarNpc( muGRClient m):base(m)
		{
		}

		public static IObjectPlugin create( IClientBase m )
        {           
            return new sceneCtrlAvatarNpc( m as muGRClient );
        }

		 
		override protected void initData( GameEvent e )
		{	
			Variant data = e.data;
			_npcid = data["nid"];

			this.avatarid = m_mgr.getNPCAvatar( _npcid );
			Variant avatarConf = m_mgr.getEntityConf( this.avatarid ); 
		 
			Variant info = new Variant();

            info["isCreateName"] = true;
            if (data.ContainsKey("titleConf"))
            {
                info["titleConf"] = data["titleConf"];
            }
            else
            {
                info["titleConf"] = getTitleConf("name", 0, GameTools.createGroup("text", data["name"]));
            }
            
			info[ "name" ] = data[ "name" ];
			//info[ "eqp" ] = data[ "eqp" ];
			info[ "avatarConf" ] = avatarConf;


			if (data.ContainsKey("ori"))
            {
                info["ori"] = data["ori"];
            }
			else
			{
				info["ori"] = 0;		
			}	
			info["ani"] = GameConstant.ANI_IDLE_NORMAL;		
			info["loop"] = true;		

			setPos( 
				(m_gameCtrl as LGAvatar).x,
				(m_gameCtrl as LGAvatar).y
			);
			 

			info[ "x" ] = m_x;
			info[ "y" ] = m_z;
			info[ "z" ] = m_y;
			
			this.dispatchEvent( 				 
				GameEvent.Create( GAME_EVENT.SPRITE_SET_DATA, this, info ) 
			);
		}


        override protected Variant getClickInfo()
		{
			Variant clkinfo = new Variant();
			clkinfo[ "npcid" ] = _npcid;
            return clkinfo;
		}
		
		override protected bool isRender()
		{
			return false;
		}

		private LGAvatarNpc gameCtrl
		{
			get{
				return m_gameCtrl as LGAvatarNpc;
			}
		}

	}
}
