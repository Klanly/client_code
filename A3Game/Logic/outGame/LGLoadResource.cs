using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  
using Cross;
using UnityEngine;
using System.IO;

namespace MuGame
{
	public class LGLoadResource : lgGDBase,IObjectPlugin
	{
		//private string _mainConfFile = "main";

        public int m_nLoaded_MapID = -1;

        static public LGLoadResource _instance;

		public LGLoadResource( gameManager m):base(m)
		{
            _instance = this;
		}

		public static IObjectPlugin create( IClientBase m )
        {           
            return new LGLoadResource( m as gameManager  );
        }

		override public void init()
		{
            //UILoading.loading.showLoading = true;
            //UILoading.loading.showring(true);
            //Variant data = new Variant();
            //data["name"] = "正在连接服务器";
            //UILoading.loading.setText(data);

            ArrayList data = new ArrayList();
            data.Add("正在连接服务器");
           

            //loading开始 lucisa
            if (login.instance)
                login.instance.showUILoading();
            else
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.BEGIN_LOADING);

			g_mgr.g_netM.addEventListenerCL( OBJECT_NAME.DATA_CONN, GAME_EVENT.CONN_VER, onServerVer );
			
			g_mgr.g_netM.addEventListenerCL( OBJECT_NAME.DATA_JOIN_WORLD, PKG_NAME.S2C_MAP_CHANGE, onMapChgLoad );

			//g_mgr.g_gameConfM.addEventListener( GAME_EVENT.CONF_MAP, onSingleMapConfLoad );


			
		}
		

		private void onServerVer( GameEvent e )
		{
            //Variant data = new Variant();
            //data["name"] = "load main.xml";
            //UILoading.loading.setText(data);

            if( beginloading.instance != null ) beginloading.instance.setText("load main.xml");

            //if (debug.instance != null) debug.instance.showMsg(ContMgr.getOutGameCont("debug2"));

			loadMainConf();
		}
 
 
		private void loadMainConf()
		{
            Debug.Log("加载staticdata.dat 资源------------------------------------------------");
            byte[] data = File.ReadAllBytes(Cross.LoaderBehavior.DATA_PATH + "OutAssets/staticdata.dat");
            ByteArray sd_data = new ByteArray(data);
            sd_data.uncompress();
            XMLMgr.instance.init(sd_data);
            sd_data.clear();
            sd_data = null;


            ////a3_BagModel.getInstance();
            ////SkillModel.getInstance();
            //PlayerModel.getInstance();

            //A3_AchievementModel.getInstance();
            //A3_activeDegreeModel.getInstance();
            //A3_BuffModel.getInstance();
            ////a3_counterpartModel.getInstance(); //需要地图信息
            //A3_dartModel.getInstance();
            //A3_EliteMonsterModel.getInstance();
            //A3_HallowsModel.getInstance();
            //A3_LegionModel.getInstance();
            //A3_MailModel.getInstance();
            //A3_NPCShopModel.getInstance();
            //A3_PetModel.getInstance();
            //a3_rankingModel.getInstance();
            //A3_RuneStoneModel.getInstance();
            //A3_SevendayModel.getInstance();
            //A3_SlayDragonModel.getInstance();
            //A3_SmithyModel.getInstance();
            //a3_speedTeamModel.getInstance();
            //A3_TeamModel.getInstance();
            ////A3_VipModel.getInstance(); //这时候服务器解析其数据
            ////A3_WingModel.getInstance(); //这时候服务器解析其数据
            ////a3_ygyiwuModel.getInstance(); //还没有收到服务器的职业，所以不能初始化
            //ArenaModel.getInstance();
            ////AutoPlayModel
            //MuGame.Qsmy.model.AutoPlayModel.getInstance();
            //a3_BagModel.getInstance();
            //E_mailModel.getInstance();
            //EquipModel.getInstance();
            //ExchangeModel.getInstance();
            //FindBestoModel.getInstance();
            //GameConfig.getInstance();
            //LotteryModel.getInstance();
            //notice_model.getInstance();
            //OffLineModel.getInstance();
            //RechargeModel.getInstance();
            //ResetLvLAwardModel.getInstance();
            //ResetLvLModel.getInstance();
            //SignModel.getInstance();
            //SkillModel.getInstance();
            //TaskModel.getInstance();
            //WeaponModel.getInstance();
            //WelfareModel.getInstance();
            




            //DebugTrace.print("load main.xml!");

            //将从OutAssets/staticdata/staticdata.dat中解包出需要的xml
            //URLReqImpl urlreq_staticdata = new URLReqImpl();
            //urlreq_staticdata.dataFormat = "binary";
            //urlreq_staticdata.url = "staticdata/staticdata.dat";
            //urlreq_staticdata.load((IURLReq url_req, object ret) =>
            //{
            //    byte[] data = ret as byte[];
            //    //debug.Log("加载staticdata 成功。。。。 len " + data.Length);

            //    ByteArray sd_data = new ByteArray(data);
            //    sd_data.uncompress();
            //    XMLMgr.instance.init(sd_data);
            //    //while (sd_data.bytesAvailable > 4)
            //    //{


            //        //int name_len = sd_data.readInt();
            //        //string name_str = sd_data.readUTF8Bytes(name_len);
            //        //int data_len = sd_data.readInt();
            //        //string data_str = sd_data.readUTF8Bytes(data_len);

            //        //debug.Log("处理表 " + name_str + " 大小=" + data_len);

            //        //XMLMgr.instance.AddXmlData(name_str, ref data_str);

            //        //name_str = null;
            //        //data_str = null;
            //  //  }
            //},
            //null,
            //(IURLReq url_req, string err) =>
            //{
            //    debug.Log("加载staticdata 失败。。。。。。。。。。。。" + url_req.url);
            //});
            LoaderBehavior.ms_AllLoadedFin = onloadUIConf;
            string mainConfFile = (
                g_mgr.g_netM.getObject(OBJECT_NAME.DATA_CONN) as connInfo
            ).mainConfFile;

			g_mgr.g_gameConfM.loadConfigs(
				mainConfFile,
				onloadMainConf
			);
		}

        private void onloadMainConf()
        {
            //Variant data = new Variant();
            //data["name"] = "Language";
            //UILoading.loading.setText(data);

            ////暂时不考虑语言包
            //if (beginloading.instance != null) beginloading.instance.setText("Language");
            //g_mgr.g_localizeM.loadPreloadLanguagePack(onloadLan);

            onloadLan();
        }

        protected void onloadLan()
        {
            ////Variant data = new Variant();
            ////data["name"] = "LoadUIConf";
            ////UILoading.loading.setText(data);

            //if( beginloading.instance != null ) beginloading.instance.setText("LoadUIConf");
            //g_mgr.g_uiM.loadPreloadUIConfig(onloadUIConf);


            if (beginloading.instance != null) beginloading.instance.setText("Wait Xml Parsing");
            //if (debug.instance != null) debug.instance.hideMsg();
            //要确保，现在没有资源在加载了
            
        }

        protected void onloadUIConf()
        {
            LGLoadResource thisptr = this;
            dispatchEvent(GameEvent.Create(GAME_EVENT.ON_LOAD_MIN, thisptr, null));
            //_loadMapResource();
        }

        //private void onSingleMapConfLoad(GameEvent e)
        //{
        //    _loadMapResource();
        //}

        private void onMapChgLoad(GameEvent e)
        {
            _onMapChgLoad();
        }

        public void _onMapChgLoad()
        {
            if (a3_expbar.instance != null)
            {
                a3_expbar.instance.CloseAgainst();
            }
            uint mapid = (
                g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD) as joinWorldInfo
            ).mapid;
            
            if (mapid <= 0)
            {
                return;
            }

            if (m_nLoaded_MapID == mapid )
            {
                if (GRMap.CUR_POLTOVER_CB != null)
                {
                        GRMap.CUR_POLTOVER_CB();
                        GRMap.CUR_POLTOVER_CB = null;
                }
                return;
            }

			//Variant data = new Variant();
			//data["name"] = "LoadMap mapid" + mapid;
			//if ((this.g_mgr.g_uiM.getLGUI(UIName.LGUIOGLoadingImpl) as LGUIOGLoadingImpl).uiPanel != null)
			//	(this.g_mgr.g_uiM.getLGUI(UIName.LGUIOGLoadingImpl) as LGUIOGLoadingImpl).uiPanel.statusLoading.text = "LoadMap mapid" + mapid;
			//UILoading.loading.setText(data);

            loadMapRes(mapid.ToString());
        }

        private void loadMapRes(string mapid)
        {
            _loadMapResource();
            //(this.g_mgr.g_gameConfM as muCLientConfig).localGrd.get_value(
            //    mapid,
            //    () =>
            //    {
            //        _loadMapResource();
            //    }
            // );
        }

        private void _loadMapResource()
        {//todo 			 
            //this.g_mgr.g_uiM.getLGUI() as LGIUI
          
            LGLoadResource thisptr = this;
            dispatchEvent(GameEvent.Create(GAME_EVENT.ON_LOAD_MAP, thisptr, null));
        }
 
		 
	}

}
        