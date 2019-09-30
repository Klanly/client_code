using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
namespace MuGame
{
    public class joinWorldInfo : LGDataBase
	{
		 
        protected Variant _blessArr = new Variant();//bstate数组
        public Variant rmis_fin
        {
            get
            {
                return m_data["rmis_fin"];
            }
        }
		 
        public Variant misacept
        {
            get
            {
                return m_data["misacept"];
            }
        }
 
       public uint carrlvl
        {
            get
            {
                return m_data["carrlvl"];
            }
        }
        public uint level
        {
            get
            {
                return m_data["level"];
            }
        }
         
        public uint mapid
        {
            get
            {
                return m_data["mpid"];
            }
        }
        public string name
        {
            get
            {
                return m_data["name"];
            }
        }
        public Variant mainPlayerInfo
        {
            get
            {
                return m_data;
            }
        }
        //hh
        public uint cid
        {
            get
            {
                return m_data["cid"];
            }
        }
        public int carr
        {
            get
            {
                return m_data["carr"];
            }
        }
        public Variant GetBless()
        {
            return _blessArr;
        }
        public static  joinWorldInfo instance;
        public joinWorldInfo(muNetCleint m): base(m)
		{
            instance = this;
		}
		public static IObjectPlugin create( IClientBase m )
        {           
            return new joinWorldInfo( m as muNetCleint );
        }
		override public void init()
		{// 
			m_data["state"] = GameConstant.ST_STAND;
			m_data["iid"] = 0;
			m_data["mapid"] = 0;
			m_data["x"] = 0;
			m_data["y"] = 0;
            m_data["hit"] = 0;
			m_data["name"] = 0;
			m_data["sex"] = 0;
			m_data["carr"] = 0;
			m_data["speed"] = 0;
			m_data["speedTrans"] = 0;
            m_data["level"] = 0;

            m_data["gm"] = 0;
			m_data["vip"] = 0;
			m_data["yb"] = 0;
			m_data["gold"] = 0;
			m_data["exp"] = 0;
			m_data["hp"] = 0;
			m_data["mp"] = 0;
            m_data["dp"] = 0;
            m_data["max_hp"] = 0;
            m_data["max_mp"] = 0;
            m_data["max_dp"] = 0; 

			m_data["eqp"] = null;
            m_data["misacept"] = null;
            m_data["rmis_fin"] = null;
            m_data["level"] = 0;

            m_data["roll_pt"] = 0;
            m_data["att_pt"] = 0;
            m_data["mount"] = 0;

            //hh
            m_data["cid"] = 0;
            m_data["carr"] = 0;
            m_data["carrlvl"] = 0;
            m_data["resetlvl"] = 0;
            m_data["clanid"] = 0;

            m_data["str"] = 0;

 
				
			m_data["ori"] = 0;		
 	

			g_mgr.addEventListener( PKG_NAME.S2C_JOIN_WORLD_RES, onJoinWorldRes );
			g_mgr.addEventListener( PKG_NAME.S2C_MAP_CHANGE, onMapChg );

			//g_mgr.addEventListener( PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, onChangeMapReadyRes );
 
		}
        //private void onChangeMapReadyRes( GameEvent e )
        //{//todo change failed
        //    (g_mgr.getObject( OBJECT_NAME.MSG_MAP ) as InGameMapMsgs).end_change_map();
        //     this.dispatchEvent( 
        //        GameEvent.Create( 
        //            PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES, 
        //            this,
        //            null
        //        ) 
        //    );
        //}

        Variant mapChangeD;
        private void onMapChg(GameEvent e)
        {
            //float x = e.data["x"];
            //float y = e.data["y"];

            //m_data["mapid"] = e.data["mpid"];
            //m_data["x"] = x;
            //m_data["y"] = y;
            //m_data["iid"] = e.data["iid"]; 
            if (a3_expbar.instance != null)
            {
                a3_expbar.instance.CloseAgainst();
            }
        


            mapChangeD = e.data;

            int curMapId = m_data.ContainsKey("mpid") ? m_data["mpid"]._int : 0;
            int toMapId = e.data["mpid"];

         

            if (curMapId != 0 && toMapId == 1 && loading_cloud.instance == null)
            {
           
                loading_cloud.showhandle = doChangeMap;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.LOADING_CLOUD);
            }
            else
            {
             //   debug.Log("!!joinWorldInfo!! false!! " + debug.count);
                doChangeMap();
            }
        }

        void doChangeMap()
        {
            updataDetial(mapChangeD);



            this.dispatchEvent(
                GameEvent.Create(
                    PKG_NAME.S2C_MAP_CHANGE,
                    this,
                    null
                )
            );
            mapChangeD = null;
        }


		private void onJoinWorldRes( GameEvent e )
		{
        //    debug.Log("玩家进入世界............");
            //return;


			updataDetial( e.data );
			foreach (string key in m_data.Keys)
			{			 
                if(key == "bstates")
			    {
                    _blessArr = m_data["bstates"];
			    }			
			}

			     
            this.g_mgr.g_processM.addProcess(  processStruct.create( this.updateProcess, "joinWorldInfo" ) );
			 
			this.dispatchEvent( 
				GameEvent.Create( 
					PKG_NAME.S2C_JOIN_WORLD_RES, 
					this,
					null
				) 
			);
		}

		public void updataDetial( Variant info )
		{
			foreach (string key in info.Keys)
            {

				if( key == "mpid" )
				{
                    m_data["mpid"] = info[key];
					continue;
				}

                if (key=="x")
                    m_data[key] = info[key]/GameConstant.PIXEL_TRANS_UNITYPOS;
                else if (key == "y")
                    m_data[key] = info[key] / GameConstant.PIXEL_TRANS_UNITYPOS;
                else
                m_data[key] = info[key];
            }
		}
		public void onSelfAttChange( Variant data )
		{						 
			//if(data.ContainsKey("mpadd"))
			//{
			//	this.modMp( data.mpadd );
			//}
			//if(data.ContainsKey("maxmp"))
			//{
			//	int changedMaxMp = data.maxmp - m_data.max_mp;
			//	if( changedMaxMp != 0 )
			//	{
			//		this.modMaxMp( data.maxmp );
			//		mainui.AddAttShow("maxmp",changedMaxMp);
			//	}
			//}
			//if(data.ContainsKey("camp")){
			//	m_data.camp = data.camp;
			//	//同步到其他需要显示阵营变化的地方
			//}
			//if(data.ContainsKey("skcds")){
			//	//技能CD
			//	lgInGame.plyController.touch_cd(data.skcds.sktp,data.skcds.cdtm);
			//}
			//if(data.ContainsKey("prepo")){
			//	//随身仓库开启
			//	this.lgInGame.lgGD_Gen.prepo = data.prepo;
			//}
			//if(data.ContainsKey("act_v")){
			//	//行动力变化，当前的行动力
			//	m_data.act_v = data.act_v;
			//	//TO DO
			//	//同步到需要显示行动力变化的地方
			//}
 
			//if(data.ContainsKey("carrlvl"))
			//{
			//	m_data.carrlvl = data.carrlvl;
			//	//TO DO
			//	//同步到需要显示职业等级变化的地方
			//	LGIUITransfer transfer = this.lgInGame.lgClient.uiClient.getLGUI("transfer") as LGIUITransfer;
			//	if(transfer)
			//	{
			//		transfer.trans_back();
			//	}
			//	//更新任务
			//	lgInGame.lgGD_miss.PlayerInfoChanged();
				
			//	LGIUISystemOpen sys = lgInGame.lgClient.uiClient.getLGUI("systemopen") as LGIUISystemOpen;
			//	sys.OnCarrlvl(data.carrlvl);
			//	LGIUICharacterInfo cha = lgInGame.lgClient.uiClient.getLGUI("mdlg_chainfo") as LGIUICharacterInfo;
			//	cha.SelfDetailInfoChange(m_data,null);
			//}
 
			//if(data.ContainsKey("ypvip")){
			//	m_data.ypvip = data.ypvip;
			//}


		}
		
        //------------------------------------属性变化  start-----------------------------------------------------------------------
		private long lasttm = 0;
		public void setPos( double x, double y)
		{ 
			long tm = g_mgr.g_netM.CurServerTimeStampMS;
			double lastx = m_data[ "x" ]._double;
			double lasty = m_data[ "y" ]._double;

			m_data[ "x" ]._double = x;
			m_data[ "y" ]._double = y; 
			float dis = (float)Math.Sqrt( (lastx-x)*(lastx-x) + (lasty-y)*(lasty-y) );
			//DebugTrace.printx( " === currtm["+ tm +"] lasttm["+ lasttm +"] tmsub["+ (tm - lasttm)+"] distance["+dis+"]  setPos lastx["+ lastx +"],lasty["+ lasty +"],currx["+ x +"],curry["+ y +"]  subx["+ (lastx-x)+"],suby["+ (lasty-y) +"]   === " );
			//DebugTrace.printx( " === currtm["+ tm +"] lasttm["+ lasttm +"] tmsub["+ (tm - lasttm)+"] distance["+dis+"]  setPos lastx["+ lastx +"],lasty["+ lasty +"],currx["+ x +"],curry["+ y +"]  subx["+ (lastx-x)+"],suby["+ (lasty-y) +"]   === " );
			//DebugTrace.contentAdd( " === currtm["+ tm +"] lasttm["+ lasttm +"] tmsub["+ (tm - lasttm)+"] distance["+dis+"]  setPos lastx["+ lastx +"],lasty["+ lasty +"],currx["+ x +"],curry["+ y +"]  subx["+ (lastx-x)+"],suby["+ (lasty-y) +"]   === " );
           
		   this.dispatchEvent(
               GameEvent.Createimmedi(UI_EVENT.UI_MAIN_TEXT, this, m_data)
           );
		   lasttm = tm;
		}
		//--------------------------------------Bless  start-------------------------------------------------------------------
		private void updateBless()
		{
			if(_blessArr.Count <= 0)
			{
				return;
			}
			long server_tm =   (this.g_mgr.g_netM as muNetCleint).CurServerTimeStampMS/1000;
			bool hasChange = false;
			for(int i =_blessArr.Count-1; i>0; --i)
			{
				Variant bless = _blessArr._arr[i];
				if(bless["end_tm"] < server_tm)
				{
					_blessArr._arr.RemoveAt(i);
					hasChange = true;
				}
			}
			if(hasChange)
			{
                //if(mainui)
                //{
                //    mainui.BlessChange(_blessArr);
                //}
			}
		}
		protected Variant getBlessById(int id)
		{
			Variant bless = new Variant();
			foreach(Variant temp in _blessArr.Values)
			{
				if(temp["id"]==id)
				{
					bless = temp;
					break;
				}
			}
			return bless;
		}
		protected void removeBless(int id)
		{
			for(int i=0;i<_blessArr.Count;++i)
			{
				if(_blessArr._arr[i][id]==id)
				{
					_blessArr._arr.RemoveAt(i);
					break;
				}
			}
		}
		public void BlessChange(Variant data)
		{
			Variant state = null;
			if(data.ContainsKey("mod"))
			{
				state = data["mod"];
			}
			else
			{
				state = data;
			}
			if(state.ContainsKey("rmvid"))
			{
				removeBless(state["rmvid"]);				
                //if(mainui)
                //{   //通知界面逻辑类
                //    mainui.BlessChange(_blessArr);
                //}
			}
			else
			{	 //看看原先是否有这个同类特效。如果有直接更新数据就可以了。
				Variant old_state = getBlessById(state["id"]);
				if(old_state != null)
                {   //逐一替换属性					
					foreach(String i in state.Keys)
					{
						old_state[i] = state[i];
					}
				}
				else
				{
					_blessArr._arr.Add(state);
				}				
                //if(mainui)
                //{   //通知界面逻辑类
                //    mainui.BlessChange(_blessArr);
                //}
			}
		}
		//-----------------------------------Bless  end----------------------------------------------------------------------

        //----------------------------------属性变化  end-----------------------------------------------------------------------


		//private void onAttack( GameEvent e )
		//{
		//	if( mainPlayerInfo["iid"]._uint != e.data["frm_iid"]._uint ) return;
		//	LGAvatar lga = lgMain.getLGAvatarByIid( e.data["to_iid"]._uint  );
		//	lgMain.OnAttack( e.data, lga ); 			
		//}

		//private void onHurt( GameEvent e )
		//{
		//	if( mainPlayerInfo["iid"]._uint != e.data["to_iid"]._uint ) return;
		//	LGAvatar lga = lgMain.getLGAvatarByIid( e.data["frm_iid"]._uint  );
		//	lgMain.OnAttack( e.data, lga ); 
		//}

		private lgSelfPlayer lgMain
		{
			get{
				return this.g_mgr.g_gameM.getObject( OBJECT_NAME.LG_MAIN_PLAY ) as lgSelfPlayer;
			}
		}
        //=== Iprocess ====     
        public void updateProcess(float tmSlice)
        {
            if (_blessArr.Count > 0)
            {
                updateBless();
            }
        }
        
	}
}