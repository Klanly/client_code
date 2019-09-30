using System;
using GameFramework;  using Cross;
namespace MuGame
{
 
	public class MuMain :gameMain
	{
		 
		public MuMain()
		{
		}
		override protected void onInit( Variant parma )
		{
			g_gameM.dispatchEventCL( 
				OBJECT_NAME.LG_OUT_GAME, 
				GameEvent.Create( GAME_EVENT.GAME_INIT_START, this, null ) 
			); 

			connInfo cinfo = g_netM.getObject( OBJECT_NAME.DATA_CONN ) as connInfo;
			cinfo.setInfo( parma );
		}
 

		override protected IClientBase createGameManager()
		{
			return new muLGClient( this );
		}
		

		override protected IClientBase createNetManager()
		{
			return new muNetCleint( this );
		}
		

		override protected IClientBase createSceneManager()
		{
			return new muGRClient( this );
		}
		

		override protected IClientBase createUiManager()
		{
			return new muUIClient( this );
		}
		

		override protected IClientBase createGameConfingManager()
		{
			return new muCLientConfig( this );
		}
		 
	}
}