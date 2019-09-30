using System;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
	public interface IClientBase:IGameEventDispatcherCollections
	{
		void init();


	 	void regCreator( string name, Func<IClientBase, IObjectPlugin> creator );

		 
		IObjectPlugin createInst( string name, bool single );
		IObjectPlugin getObject( string objectName );

		bool hasCreator( string name );

		
		void createAllSingleInst();

		NetClient g_netM
		{
			get;
		}
		

		gameManager g_gameM
		{
			get;
		}
 


		GRClient g_sceneM
		{
			get;
		}
 
 		UIClient g_uiM
		{
			get;
		}

		ClientConfig g_gameConfM
		{
			get;
		}
 



		processManager g_processM
		{
			get;
		}

	 
		LocalizeManager g_localizeM
		{
			get ;
		}
	}
}
