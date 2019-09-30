using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  
using Cross;
using UnityEngine;

namespace MuGame
{
    public enum PLOT_CHARRES_TYPE
    {
        PCRT_HERO = 0,
        PCRT_MONSTER = 1,
        PCRT_NPC = 2,
        PCRT_MOUNT = 3,
        PCRT_AVATAR = 4,
    }

    public class gameST
    {
        static public bool _bUntestPlot = true;
      
        //Plot 剧情的输出接口
        //static public Action<int> REQ_PLOT_RES = null;
        //static public Action REQ_PLAY_PLOT = null;
        //static public Action REQ_STOP_PLOT = null;


        //外部公用接口
        static public Action<GameObject, float> REQ_SET_FAST_BLOOM = null;
  

        //Plot 剧情的出入接口
        static public Action<GameObject[], PLOT_CHARRES_TYPE, int, string[]> REV_CHARRES_LINKER = GRMap.REV_CHARRES_LINKER;
        static public Action<GameObject[], string> REV_FXRES_LINKER = GRMap.REV_FXRES_LINKER;
        static public Action<GameObject, int> REV_SOUNDRES_LINKER = GRMap.REV_SOUNDRES_LINKER;
        static public Action<String> REV_ZIMU_TEXT = GRMap.REV_ZIMU_TEXT;
        static public Action<String> REV_PLOT_UI = GRMap.REV_PLOT_UI;
        static public Action REV_RES_LIST_OK;
        static public Action REV_PLOT_PLAY_OVER;

        //游戏中的影子的处理
        static public GameObject SHADOW_PREFAB;
        static public int HIT_Main_Color_nameID = -1;
        static public int HIT_Rim_Color_nameID = -1;
        static public int HIT_Rim_Width_nameID = -1;

        static public Material BORN_MTL;
        static public Material DEAD_MTL;
        static public Material CHAR_MTL;
        static public int DEAD_MT_AMOUNT = -1;
        static public int BORN_MT_AMOUNT = -1;
        static public int MTL_Main_Tex = -1;
        static public int MTL_Dead_Tex = -1;
        static public int MTL_Born_Tex = -1;

        protected GRWorld3D m_world;
        protected GRCharacter3D m_char;
        protected gameMain main;

        protected void _debugAvatar()
        {
            new CrossApp(true);

            ConfigManager confMgr = CrossApp.singleton.getPlugin("conf") as ConfigManager;
            confMgr.loadExtendConfig("gconf/avatar", (Variant v) =>
            {
                GraphManager.singleton._formatAvatarConf(v);

                confMgr.loadExtendConfig("gconf/effect", (Variant vv) =>
                {
                    GraphManager.singleton._formatEffectConf(vv);

                    confMgr.loadExtendConfig("gconf/material", (Variant vvv) =>
                    {
                        GraphManager.singleton._formatMaterialConf(vvv);

                        m_world = GraphManager.singleton.createWorld3D("main");

                        m_world.cam.pos = new Vec3(0, 0, 4);
                        m_world.cam.lookAt(new Vec3(0, 0, 0));

                        m_char = m_world.createEntity(Define.GREntityType.CHARACTER) as GRCharacter3D;
                        m_char.load(GraphManager.singleton.getCharacterConf("0"));
                        m_char.applyAvatar(GraphManager.singleton.getAvatarConf("0", "2016"));
                        m_char.applyAvatar(GraphManager.singleton.getAvatarConf("0", "9999"));

                        os.sys.addGlobalEventListener(Define.EventType.UI_MOUSE_UP, (Cross.Event e) =>
                        {
                            m_char.removeAvatar("wing");
                        });

                    });
                });
            });
        }

        public void init( Variant parma )
        {
            loadBaseData();

            if(main == null)
               main = new MuMain();

			main.init( parma );

            //this._debugAvatar();
		}

        public static void loadBaseData()
        {
            SHADOW_PREFAB = U3DAPI.U3DResLoad<GameObject>("default/shadow");
            if (SHADOW_PREFAB == null)
            {
                SHADOW_PREFAB = new GameObject();
            }

            HIT_Main_Color_nameID = Shader.PropertyToID("_Color");
            HIT_Rim_Color_nameID = Shader.PropertyToID("_RimColor");
            HIT_Rim_Width_nameID = Shader.PropertyToID("_RimWidth");

            MTL_Main_Tex = Shader.PropertyToID("_MainTex");
            MTL_Dead_Tex = Shader.PropertyToID("_MainTex");
            MTL_Born_Tex = Shader.PropertyToID("_MainTex");

            BORN_MTL = U3DAPI.U3DResLoad<Material>("mtl/born_mtl");
            DEAD_MTL = U3DAPI.U3DResLoad<Material>("mtl/dead_mtl");
            DEAD_MT_AMOUNT = Shader.PropertyToID("_Amount");
            BORN_MT_AMOUNT = Shader.PropertyToID("_Amount");

            CHAR_MTL = U3DAPI.U3DResLoad<Material>("mtl/char_mtl");

            EnumMaterial.EMT_EQUIP_L = U3DAPI.U3DResLoad<Material>("mtl/equip_low");
            EnumMaterial.EMT_EQUIP_H = U3DAPI.U3DResLoad<Material>("mtl/equip_high");
            EnumMaterial.EMT_SKILL_HIDE = U3DAPI.U3DResLoad<Material>("mtl/skill_hide");
        }

        public gameMain m
        {
            get {
                return main;
            }
        }


		public void init( 
			string server_config_url, 
			string server_ip, 
			uint server_id, 
			uint port, 
			uint uid, 
			uint clnt, 
			string token, 
			string mainConfig 
		)
		{
            //这是第一个埋点
            LGPlatInfo.inst.firstAnalysisPoint(server_id, uid);
          //  LGPlatInfo.inst.logSDKCustomAP("init");
            loadBaseData();

			//test();
			//return;

            //this._debugAvatar();
            //return;

            DebugTrace.Printf("" + os.sys.windowWidth + "," + os.sys.windowHeight);
            //(os.graph as GraphInterfaceImpl).onResize(os.sys.windowWidth, os.sys.windowHeight);
            //(os.graph as GraphInterfaceImpl).defCam2d.depth=99;
            Variant conf = new Variant();
            conf["server_config_url"] = server_config_url;
            conf["server_id"] = server_id;
            conf["mainConfig"] = mainConfig;
            conf["outgamevar"] = new Variant();
            conf["outgamevar"]["server_ip"] = server_ip;
            conf["outgamevar"]["server_port"] = port;
            conf["outgamevar"]["uid"] = uid;
            conf["outgamevar"]["token"] = token;
            conf["outgamevar"]["clnt"] = clnt;

            debug.Log("初始化进来.............0000000000000000");

            Stopwatch a1 = new Stopwatch(); a1.Start();//开始计时
            if (main == null)
                main = new MuMain();

            main.init(conf);
            a1.Stop(); debug.Log("cost init time =" + a1.ElapsedMilliseconds);//终止计时


            debug.Log("初始化进来.............1111111111111111");
		} 


		private void test()
		{
			//Variant a = new Variant();
			//a["num"] = "+5";
			//return;

			Variant x = new Variant( 1 );
			Variant b = new Variant( 1 );

			Variant xx = new Variant();
			Variant bb = new Variant();

			xx["x"] = x;
			bb["b"] = b;

			if( x == b )
			{
                DebugTrace.Printf("x");
			}
			if(xx["x"] == bb["b"] )
			{
                DebugTrace.Printf("x");
			} 
		}
    }
}
