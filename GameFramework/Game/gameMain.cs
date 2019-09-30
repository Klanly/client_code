using System;
using Cross;
using UnityEngine;
namespace GameFramework
{
 	abstract public class gameMain
	{
		public IClientBase g_gameM;
		public IClientBase g_netM;
		public IClientBase g_sceneM;
		public IClientBase g_uiM;
		public IClientBase g_gameConfM;
		public gameMain()
		{
			
		} 

		public void init( Variant parma )
		{
            if (CrossApp.singleton == null)
            {
                g_debug.Log("gameMain.............1");

                new CrossApp(true);
                g_debug.Log("gameMain.............1-1");
                CrossApp.singleton.regPlugin(new gameEventDelegate());
                g_debug.Log("gameMain.............1-2");
                CrossApp.singleton.regPlugin(new processManager());
                g_debug.Log("gameMain.............1-3");
                CrossApp.singleton.init();
                g_debug.Log("gameMain.............1-4");


                g_gameM = createGameManager();
                g_debug.Log("gameMain.............1-5");
                g_netM = createNetManager();
                g_debug.Log("gameMain.............1-6");
                g_sceneM = createSceneManager();
                g_debug.Log("gameMain.............1-7");
                g_uiM = createUiManager();
                g_debug.Log("gameMain.............1-8");
                g_gameConfM = createGameConfingManager();
                g_debug.Log("gameMain.............1-9");

                g_netM.init();
                g_debug.Log("gameMain.............1-10");
                g_gameM.init();
                g_debug.Log("gameMain.............1-11");
                g_gameConfM.init();
                g_debug.Log("gameMain.............1-12");
                g_sceneM.init();
                g_debug.Log("gameMain.............1-13");
                g_uiM.init();
                g_debug.Log("gameMain.............1-14");
            }

            g_debug.Log("gameMain.............2");
			onInit( parma );
            g_debug.Log("gameMain.............2-ee");
		}
		 
		abstract protected void onInit( Variant parma );
		abstract protected IClientBase createGameManager();
		 

		abstract protected IClientBase createNetManager();
		 

		abstract protected IClientBase createSceneManager();
		 

		abstract protected IClientBase createUiManager();
		 

		abstract protected IClientBase createGameConfingManager();
		 
	}
  
    public class g_debug {
        public static bool show_debug = true;

        public static g_debug instance;
        public g_debug()
        {
            if (PlayerPrefs.HasKey("debugShow"))
                show_debug = PlayerPrefs.GetInt("debugShow") == 1 ? true : false;
            else
                show_debug = true;

            instance = this;
        }

        public static void Log(string msg)
        {
            if (g_debug.instance == null)
                g_debug.instance = new g_debug();

            if (show_debug)
                Debug.Log(msg);
        }
    }
}