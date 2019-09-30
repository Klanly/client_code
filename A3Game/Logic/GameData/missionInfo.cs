using System;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    public class missionInfo : LGDataBase
    {
        private Variant _line_data = new Variant();//任务线进度
        private Variant _no_line_data = new Variant();//任务剩余完成次数cntleft

        private Variant _playerMis = new Variant();//已接任务
        private Variant _acceptable = new Variant();//可接受任务

        private Variant _mis_qa_arr = new Variant();//问题目标数组
        private Variant _mis_uitm_arr = new Variant();
        private Variant _mis_operate_arr = new Variant();
        private Variant _mis_cgoal_arr = new Variant(); //客户端目标数组

        private Variant _misAction = new Variant();//操作目标goal:

        private uint _current_map_id = 0;

        public missionInfo(muNetCleint m)
            : base(m)
        {
           
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new missionInfo(m as muNetCleint);
        }

        public override void init()
        {
            //(g_mgr as muNetCleint).joinWorldInfoInst.addEventListener(PKG_NAME.S2C_JOIN_WORLD_RES, onJoinWorldRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_GMIS_INFO_RES, onGmisInfoRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_GMIS_AWD_RES, onGmisAwdRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_RMIS_INFO_RES, onRmisInfoRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_ACCEPT_MIS_RES, onAcceptMisRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_COMMIT_MIS_RES, onCommitMisRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_FINED_MIS_STATE_RES, onFinedMisStateRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_DATA_MIS_MODIFY_RES, onDataMisModityRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_MIS_LINE_STATE_RES, onMisLineStateRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_ABORD_MIS_RES, onAbordMisRes);
            //this.g_mgr.g_netM.addEventListener(PKG_NAME.S2C_LVLMIS_PRIZE_RES, onLvlMisPrizeRes);
        }


        //--------------------------------消息处理 start-------------------------------------
        private void onJoinWorldRes(GameEvent e)
        {
            Variant acept = (g_mgr as muNetCleint).joinWorldInfoInst.misacept;
            setAcceptMis(acept);
            read_current_map_mis_line(0);

            initRmisData();
            InitPlayerRmisData();
            
        }
        //获取已完成任务状态结果
        private void onFinedMisStateRes(GameEvent e)
        {
            Variant data = e.data;
            int i = 0;
			Variant mis;
			//已完成任务信息
			if(data.ContainsKey("misfined"))
			{
				Variant misfined = data["misfined"];
				if(misfined != null)
				{
					for(i = 0;i < misfined.Count;i++)
					{
						mis = misfined[i];
						if(mis.ContainsKey("cntleft"))
							this._no_line_data[mis["misid"]._str] = mis["cntleft"];
						else
							this._no_line_data[mis["misid"]._str] = 0;
					}
				}
			}
			if(data.ContainsKey("unfined"))
			{	//未完成任务信息
				Variant unfined = data["unfined"];
				if(unfined != null){
					for(i = 0;i < unfined.Count;i++){
						int mis_id = unfined[i];
						mis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mis_id);
						if(mis == null)
							continue;
						if(mis.ContainsKey("rep") && mis["rep"] > 0)
							this._no_line_data[mis_id.ToString()] = mis["rep"];
                        else if (mis.ContainsKey("dalyrep"))
							this._no_line_data[mis_id.ToString()] = mis["dalyrep"];
						else
							this._no_line_data[mis_id.ToString()] = 1;
					}
				}
			}
			//更新可接任务
			acceptable_refault();
			//更新任务状态
			setNpcMis();
        }
        //委托完成任务
        private void onDataMisModityRes(GameEvent e)
        {

        }
        //获取任务线进展情况信息
        private void onMisLineStateRes(GameEvent e)
        {
            Variant misline = e.data["misline"];
            //记录任务线完成情况
			for(int i = 0;i < misline.Count;i++)
			{
				Variant line = misline[i];
				Variant misconf = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(line["curmis"]._int);
				if(misconf == null)//如果当前任务被删
				{
					int lineid = getLastComMis(line);
					_line_data[line["lineid"]._str] = lineid;
				}
				else
				{
					_line_data[line["lineid"]._str] = line["curmis"];
				}
			}
			
			//_can_show_cache_warning();	
			//reflush_option_limit();
			//更新可接任务
			acceptable_refault();
			setNpcMis();
			
			//_lgInGame.mainUI.OnMissionRes();
        }
        private int getLastComMis(Variant line)
		{
            Variant linemis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_by_line(line["lineid"]._int);
			int last=0;
            if (linemis == null)
                return last;

			foreach(Variant mis in linemis.Values)
			{
				if(mis["id"] < line["curmis"])
				{
					if(last < mis["id"] )
					{
						last = mis["id"] ;
					}
				}
			}
			return last;
		}


        //领取侠客行任务完成奖励
        private void onLvlMisPrizeRes(GameEvent e)
        {

        }


        private void onAbordMisRes(GameEvent e)
        {//放弃任务，服务器返回
            Variant data = e.data;
            int res = 0;
            if (data.ContainsKey("res"))
            {
                res = data["res"];
            }

            if (res == 1)
            {
                int misid = data["misid"];
                foreach (Variant mis in _playerMis._arr)
                {
                    if (mis != null && mis["misid"] == misid)
                    {
                        _playerMis._arr.Remove(mis);
                        break;
                    }
                }
                //this.dispatchEvent(GameEvent.Create(GAME_EVENT.S2C_ABORD_MIS_RES, this, data));
            }
            else
            {

            }
        }

        /*
         * 	 res:=1为成功, 
	     *   mis: 
		 *      misid:任务id, 
		 *      cntleft:可选，剩余可做次数, 
		 *      km:可选，杀怪信息
		 *	        monid:怪物id,
		 *	        cnt:已杀数量, 
		 *      kp:可选，大世界杀玩家数量, 
		 *      pzkp:可选，擂台杀玩家数量, 
		 *      pzckp:可选，擂台连杀是否完成，=0为未完成，=1为完成, 
		 *      fintm:可选，任务限时完成世界，UNIX时间戳，单位秒
         *
         */
        private void onAcceptMisRes(GameEvent e)
        {//接受任务，服务器返回
            Variant mis = e.data;
            int misid = mis["misid"];
            Variant misData = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);//...
			//设置本地处理任务（如果是本地处理任务）
			init_single_localmis( misid, true );

			//接受任务，加入到已接任务从可接任务中删除
			if( addPlayerMiss( mis ) )
			{	//在可接任务里面清掉任务可接数据
				delete_accept_mis(misid);
			}
			//添加 mission_for_bubble_arr中对应数据
//			add_mis_in_bubble_arr(misid);
			
			missionChange(misid);	
			//是否显示玩家对白
			
			//给玩家温馨提示
            string misGoalDesc = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.getMisGoalDesc(misid);
			if(misid.ToString() != misGoalDesc)
			{
				//_lgInGame.mainUI.systemmsg([misGoalDesc],16);
			}
			int mistype = getMisType(misid);
			if(4 == mistype || 5 == mistype || 7 == mistype)
			{	//完成日常任务要提示
				string acceptStr = LanguagePack.getLanguageText("missionMsg", "accept");
				string misName = LanguagePack.getLanguageText("misName", misid.ToString());
				//acceptStr = Printf(acceptStr, misName);
				//_lgInGame.mainUI.systemmsg([acceptStr], 4);
			}
			if( misData.ContainsKey("rmis") )
			{	//酒馆类任务
				Variant rmis_info = GetRmisDesc(misData["rmis"]._int);
				if(rmis_info != null)
				{	// 1号类型的酒馆任务 (封印/运镖)直接自动去目的地，
					if(1 == rmis_info["type"]._int)
					{	//启动 自动寻路
						//var doAct:AIActDoMission = new AIActDoMission(lgInGame.AIPly, {misid:misid});
						//lgInGame.AIPly.doAct(doAct);
						//_lgInGame.lgGD_RMis.onAcceptRmisMis(rmis_info.id);
					}
					else if(2 == rmis_info["type"])
					{//日常任务
						onAcceptRmisMis(rmis_info["id"]);
					}
				}
			}
            //关闭npc界面
            Variant einfo = new Variant();
            einfo["name"] = UIName.UI_MDLG_NPCDIALOG;
            //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_CLOSE, this, einfo));
			//var npcdlg:LGIUINpcDialog = this._lgInGame.lgClient.uiClient.getLGUI("npcdialog") as LGIUINpcDialog;
			//npcdlg.CloseNPCDialog();
			//if(uiMis)
			//{
			//	uiMis.MissionChange(0,misid);
			//}
            //var misLinks:Array = this._lgInGame.lgClient.clientConfig.genConf.GetMisLinks();
            //for each(var mid:Object in misLinks)
            //{
            //    if(mid.id == misid)
            //    {
            //        this._lgInGame.AIPly.stop();
            //        break;
            //    }
            //}
            //var lgmai:LGIUIMainuiAttach= _lgInGame.lgClient.uiClient.getLGUI("mainuiAttach") as LGIUIMainuiAttach;
            //lgmai.OnMisComplete(misid);
            //this.lgInGame.trigMgr.DoTrigger( TriggerManager.TRGT_ACPMIS, misid );


            //this.dispatchEvent(GameEvent.Create(GAME_EVENT.S2C_ACCEPT_MIS_RES, this, data));
        }

        /*
         * 	res:=1为成功,  
	     *  misid:任务id, 
	     *  achives:可选，获得的成就id数组[] 
         *
         */
        private void onCommitMisRes(GameEvent e)
        {//提交任务，服务器返回
            Variant data = e.data;
            int misid = data["misid"];

            //如果是新手任务，则弹出使用物品界面
			if(isNewPlayermis(misid))
            {
				/*var mis_award:Object = _main._uiUtility.get_mis_award(mis_id)*/;
				Variant mis_award = getMisAward(misid);
				if(mis_award != null){
					
					//var uiImpl:LGIUIItems = this._lgInGame.lgClient.uiClient.getLGUI("items") as LGIUIItems;
					
					if(mis_award.ContainsKey("eqp") && mis_award["eqp"][0] != null)
                    {
						//TODO : 弹出使用物品提示
						//uiImpl.openUseitemPrompt(mis_award.eqp[0]);
					}
					else if(mis_award.ContainsKey("itm") && mis_award["itm"][0] != null){
						Variant item = mis_award["itm"][0];
						//var item_id:uint = item.id;
						//if(_lgInGame.lgClient.svrGameConfig.svrItemConf.is_equip(item_id) || _lgInGame.lgClient.svrGameConfig.svrItemConf.is_uitem(item_id))
                        //{
							//TODO : 弹出使用物品提示
							//uiImpl.openUseitemPrompt(mis_award.itm[0]);
						//}
					}
				}
			}
			
			Variant accpetData = _playerMis[ misid.ToString() ];
			if(  accpetData != null )
			{
				Variant misConf = accpetData["configdata"];
                Variant misGoal = misConf["goal"];
				if(misGoal.ContainsKey("lvl_score_awd"))
				{
					//_lgInGame.lgGD_levels.ClearLvlScoreAwd(misData.goal.lvl_score_awd[0]);
				}
				if( misConf.ContainsKey("dalyrep") && misConf["dalyrep"]._int > 0 )
				{
					if( !_no_line_data.ContainsKey( misid.ToString() ) )
					{
						_no_line_data[misid.ToString()] = misConf["dalyrep"];
					}
					_no_line_data[ misid.ToString() ]--;
				}
				Variant misline = misConf["misline"];
				if( misline != null )
				{
					_line_data[misline.ToString()] = misid;
                    _line_refresh_acceptable(misline._int);
				}
                preMisComplete(misid);
				deletePlayerMis( misid );		//从已接任务中删除
			}
            onCompleteRmis(misid);
			missionChange(misid);
//			this._main.UIMgr.text_show.Close();
//			if(mis_id == 220)
//			{
//				this._main._first_app.on_newuser_change(0);
//				this._main._first_app.on_user_act(9);
//			}
//			//如果当前和他对话的任务中还有可接。或者完成的任务。先接光。交光操作。
//			if( _main.UIMgr.mis_get_equip.is_closed() )
//			{
//				real_is_have_npc_mis(mis_id);
//			}
            //关闭npc界面
            Variant einfo = new Variant();
            einfo["name"] = UIName.UI_MDLG_NPCDIALOG;
            //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_CLOSE, this, einfo));
            //var npcdlg:LGIUINpcDialog = this._lgInGame.lgClient.uiClient.getLGUI("npcdialog") as LGIUINpcDialog;
            //npcdlg.CloseNPCDialog();
            //var sysOpen:LGIUISystemOpen= this._lgInGame.lgClient.uiClient.getLGUI("systemopen") as LGIUISystemOpen;
            //if(sysOpen)
            //{
            //    sysOpen.OnSelfCpMission(mis_id);
            //}
            //if(uiMis)
            //{
            //    uiMis.MissionChange(1,mis_id);
            //    uiMis.onComplete(mis_id);//...	
            //    if(!IsMIsCanAutocommit(mis_id))
            //    {//手动完成任务，自动下个任务
            //        _lgInGame.delayDoMgr.AddDelayDo(function():void{uiMis.AutoMis(mis_id);}, 500);
            //    }
				
            //}
			//lgInGame.lgGD_PlyFun.AutoTranfer();
			
			if(is_main_mis(misid))
			{
				//如果是主线任务奖励的最后一个任务，则关闭界面
                //if(_lgInGame.lgClient.clientConfig.genConf.IsLastMis(mis_id))
                //{
                //    var lgmai:LGIUIMainUI= _lgInGame.lgClient.uiClient.getLGUI(UIName.LGUIMainUIImpl) as LGIUIMainUI;
                //    lgmai.MisAwdOver();
                //}
			}
			
            //if(_lgInGame.lgClient.clientConfig.genConf.GetCommonConf("mountSoulTrigger") == mis_id)
            //{
            //    GuideManager.singleton.Start("buyMountSoul");
            //}
            //this.lgInGame.trigMgr.DoTrigger( TriggerManager.TRGT_COMMIS, mis_id );

            //this.dispatchEvent(GameEvent.Create(GAME_EVENT.S2C_COMMIT_MIS_RES, this, data));
        }



        //由于需要选择全地图的任务，所以这里改成不受本地图限制
		public void read_current_map_mis_line(uint mapid)
		{
			if(_current_map_id != 0)
				return;
			
			_current_map_id = mapid;
			
			//			//获取该地图上NPC所涉及的任务线完成信息，缓存
			Variant line_arr = new Variant();
			Variant no_line_arr = new Variant();
			Variant dalyrep_arr = new Variant();
            Variant miss = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_missions();
			
			foreach(Variant mis in miss.Values)
            {
				Variant accept = mis["accept"];
				if(accept.ContainsKey("attchk")){
					//var attchkt:Array = accept.attchk;
					//if(!ConfigUtil.attchk(attchkt,this._lgInGame.lgGD_Gen.netData))
					//	continue;
				}
				add_mis_info(no_line_arr,line_arr,mis);
				add_daymis_info(dalyrep_arr,mis);
			}

            //if (line_arr.Count > 0)
                //获取任务线信息
                //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetMisLineState(line_arr);
                //this.m_mgr.g_uiM.dispatchEvent(GameEvent.Create(PKG_NAME.C2S_MIS_LINE_STATE, this, line_arr));

            //if (no_line_arr.Count > 0)
                //获取非任务线任务信息
                //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetFinedMisState(no_line_arr);
                //this.m_mgr.g_uiM.dispatchEvent(GameEvent.Create(PKG_NAME.C2S_FINED_MIS_STATE, this, no_line_arr));

            //if (dalyrep_arr.Count > 0)
				//获得日常任务
                //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetFinedMisState(dalyrep_arr);
                //this.m_mgr.g_uiM.dispatchEvent(GameEvent.Create(PKG_NAME.C2S_FINED_MIS_STATE, this, dalyrep_arr));
		}

        //--------------------------------消息处理 end-------------------------------------
        //------------------------------字符处理  start-------------------------------------------------------------------
		public string get_mis_name(int mis_id)
		{
			//可重复做的任务后面加上（当前完成次数/总共可以完成次数）
			Variant mis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mis_id);
			string misName = "";
			if(mis == null)
				return misName;
			
			if( mis.ContainsKey( "rmis" ) &&  !is_acceptable_mis( mis) )
			{//对于没有接受的酒馆任务 使用 酒馆任务的名字
				misName = LanguagePack.getLanguageText("rmisName",mis["rmis"]._str);
				return  misName;
			}
			
			misName = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.getMisName(mis_id);
			string str = misName;
			if(mis["misline"] > 0)
				return str;
			
			if((!mis.ContainsKey("rep") || mis["rep"] <= 0) &&
				(!mis.ContainsKey("dalyrep") || mis["dalyrep"] <= 0))
				return str;
			
			int leftcnt;
			if(_no_line_data.ContainsKey(mis_id))
				leftcnt = this._no_line_data[mis_id];
			else
				leftcnt = mis["dalyrep"];
			
			int overcnt = mis["dalyrep"]._int - leftcnt;
            str += "(" + overcnt.ToString() + "/" + mis["dalyrep"]._str + ")";
			return str;
		}
        /**
		 * 通过任务类型id 获取具体的 任务类型
		 * 
		 */	
		private string get_type_by_id(int typeid)
		{
			return LanguagePack.getLanguageText("mission_manager", "mis_type_" + typeid.ToString());
		}
		/**
		 *获得需要物品的链接字段。
		 * @uitem 需要物品数据
		 * @mid 任务编号
		 */
		public string get_need_item_str(Variant uitem, int mid)
		{
			string colitm_event = "";
			if(uitem.ContainsKey("open_ui")){
				//open ui to use system fun to get item!
				//open target ui;
				string open_ui = uitem["open_ui"];
				colitm_event = "openui_" + open_ui;
			}
			else if(uitem.ContainsKey("collect")){
				//区域采集任务
				Variant collect = uitem["collect"][0];
				int mpid = collect["mpid"];
				int areaid = collect["areaid"];
//				var mapex:Object = ConfigDataManager.mgr.get_mapex(mpid);
//				if(mapex != null){
//					colitm_event = "collect_" + mpid + "_" + areaid + "_" + mid;
//				}
			}
			else if(uitem.ContainsKey("pos")){
				//去指定位置。杀怪并获得某件物品
				Variant m_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mid);
				
				Variant goal = getMissionGoal(m_data);
				
				int ci_mon_id = get_mon_id_by_killmonitm(goal["kilmonitm"],uitem["tpid"]._int);
				
				int lvl_id = 0;
				if(uitem.ContainsKey("level_id"))
                {
					lvl_id = uitem["level_id"];
				}
				colitm_event = "mon_" + ci_mon_id.ToString()+"_" + uitem["pos"][0]["mpid"] +"_"+uitem["pos"][0]["x"]+"_"+uitem["pos"][0]["y"]+"_"+lvl_id.ToString()+"_"+mid.ToString();				
			}
			else if(uitem.ContainsKey("npcid")){
				//去npc哪里购买某件物品
				colitm_event = "buyitem_" + uitem["npcid"]._str+"_"+ mid.ToString() +"_"+uitem["tpid"]._str;
				
			}
			else if(uitem.ContainsKey("tpid")){
				int rptid;
				if(uitem.ContainsKey("ttpid")){
					//需要获得的道具和显示的道具不一样，显示真正获得的道具
					rptid = uitem["ttpid"];
				}
				else{
					rptid = uitem["tpid"];
				}
				Variant s_shopItem = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_game_market_sell_data_by_tpid(rptid);
				//如果上面两个都没的话。去商场里面看看。
				if(s_shopItem != null){
					colitm_event = "shop_" + rptid.ToString();
				}
			}		
			if(uitem.ContainsKey("level_id")){
				colitm_event += "_" + uitem["level_id"];
			}	
			return colitm_event;
		}

		//------------------------------字符处理  end------------------------------------------------------------

        /**
		 *获得npc的已接任务。 
		 */
		public Variant get_npc_misacept(int npcid)
		{
			Variant npcmisArr = new Variant();
			Variant mobj = null;
			foreach( Variant acceptMiss in _playerMis.Values )
			{
				mobj = acceptMiss["configdata"];	
				if(mobj == null)
				{
					continue;
				}
				if(mobj["awards"][0]["npc"]._int == npcid){
					npcmisArr.pushBack(mobj["id"]);
				}
			}
			return npcmisArr;
		}
		/**
		 *获得npc的可接任务。 
		 */
		public Variant get_npc_acceptable_mis(int npcid)
        {	
			Variant acceptedRmis = null;
			Variant npcmisArr = new Variant();
			Variant tpobj = null;
			for(int i = 0; i < acceptableMis.Count; i++)
            {
                tpobj = acceptableMis[i];	
				if(tpobj == null) continue;
				
				if(tpobj["accept"][0]["npc"]._int == npcid)
                {
					//过滤掉 不可接受的酒馆任务
					if( tpobj.ContainsKey("rmis") )
					{
						if( acceptedRmis == null )
							acceptedRmis = GetPlayerAcceptedRmis();
						
						if( IsArrayHasValue( acceptedRmis, tpobj["rmis"] ) || 
							!IsRmisCanAccept(tpobj["rmis"])  ) 
							continue;
					}
					
					npcmisArr.pushBack(tpobj["id"]);
				}
			}
			return npcmisArr;
		}

        //获取任务目标
        public Variant getMissionGoal(Variant conf, int goalid = 0)
        {
            if (conf == null)
                return null;

            if (conf.ContainsKey("carr_gaol"))
            {
                Variant carr_goal = conf["carr_goal"];
                Variant netData = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.mainPlayerInfo;
                if (carr_goal.ContainsKey(netData["carr"]._int))
                {
                    return carr_goal;
                }
            }

            return conf["goal"][0];
        }

        public int getMisType(int misid)
        {
            //不在列表中的任务通过其他途径确定任务类型
            Variant mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (mis_data == null)
                return 1;//容错

            int misline = mis_data["misline"];
            if (misline == 1)
            {
                return 1;//主线任务
            }
            else if (mis_data.ContainsKey("autocomit_yb") && mis_data["autocomit_yb"] > 0)
            {
                return 5;	//委托任务
            }
            else if (mis_data.ContainsKey("goaladdition_daly"))
            {
                return 7;	//循环目标
            }
            else
            {
                //现在读配置文件只能区分 循环任务
                //				if( this._main._gamescene._cgeneral.IsLoopMis( misid ) )
                //				{
                //					return 3;
                //				}

                if ((mis_data.ContainsKey("rmis") && mis_data["rmis"] > 0) || (mis_data.ContainsKey("dalyrep") && mis_data["dalyrep"] > 0))
                {	//酒馆任务添加到日常任务
                    return 4;//"日常任务"
                }
                return 2;// 默认的 是"支线任务"
            }
        }

        /**
		 *获得第一个新手期的主线任务  
		 */
		public int get_first_beginner_misaccet()
		{
			int min_id = 0;
			foreach(string i in _playerMis.Keys)
			{
				if(min_id > _playerMis[i]["misid"]._int)
				{
					min_id = _playerMis[i]["misid"]._int;
				}
			}
			Variant misdata;
			int misid = 0;
			Variant acceptMiss = _playerMis[min_id.ToString()];
			if(null != acceptMiss && null != acceptMiss["configdata"])
			{
				misdata = acceptMiss["configdata"];
				if( 1 == misdata["misline"])
				{//如果有主线任务
					misid = misdata["id"];
				}
			}
			else
			{	//如果没有已接任务或者已接任务中 没有可接任务 就从可接任务中取			
				misdata = _acceptable[0];
				if(null != misdata && 1 == misdata["misline"])
				{
					misid = misdata["id"];
				}
			}
			return misid;
		}
		/**
		 *获得任务状态。 
		 */
		public int get_mis_state(int mis_id)
        {
			Variant mis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mis_id);
			if(mis == null){
				return 0;
			}
			if(is_acceptable_mis(mis)){
				return 1;   //接受。
			}
			return is_mis_complete(mis_id) ? 3 : 2;// 可完成    没有完成。
		}
		//获取任务状态//misline>0的任务有效
		//1 未接受
		//2 已接受不可提交
		//4 已接受可提交
		//8 已经完成过
		public int get_mis_flag(int mis_id)
        {
			Variant mis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mis_id);
			if(mis == null){
				return 1;
			}
			
			if(this.is_accepted_mis(mis_id)){
				//已接受
				bool complete = is_mis_complete(mis_id);
				if(complete)
					return 4;
				else
					return 2;
			}
			else{
				if(mis["misline"]._int <= 0){
					if(_no_line_data.ContainsKey(mis_id.ToString())){
						return 8;
					}
					else{
						return 1;
					}
				}
				else{
					//未接受
                    if (!_line_data.ContainsKey(mis["misline"]._str) || _line_data[mis["misline"]._str]._int < mis_id)
                    {
						//未完成
						return 1;
					}
					else{
						//已完成
						return 8;
					}
				}
			}
		}

        //获取目标动态杀怪数量
		public int getKillmonCnt(int misid)
		{
			int cnt = 0;
			Variant configdata = null;
			Variant data = null;
			if(_playerMis[misid.ToString()] != null)
			{
				Variant goal = _playerMis[misid.ToString()]["goal"];
				configdata = _playerMis[misid.ToString()]["configdata"];
				data = _playerMis[misid.ToString()]["data"];
				if(goal.ContainsKey("kilmon_map"))
				{
					cnt = goal["kilmon_map"][0]["cnt"];
				}
				else if(goal.ContainsKey("kilmon"))
				{
					cnt = goal["kilmon"][0]["cnt"];
				}
			}
			if(configdata != null && configdata.ContainsKey("goaladdition_daly"))
			{
				int dalyrep = configdata["dalyrep"];
                Variant goaladdition_daly = configdata["goaladdition_daly"][0];
				float a = goaladdition_daly["coefficient_a"];
				float b = goaladdition_daly["coefficient_b"];
				float c = goaladdition_daly["coefficient_c"];
				float fix = goaladdition_daly["fix"];
				int daycnt = dalyrep - data["cntleft"];
				cnt = (int)((a * daycnt) * cnt * cnt + b * cnt + c) ;
				if(cnt > fix)
				{
					cnt = cnt - (int)(cnt % fix);
				}
			}
			
			return cnt;
		}

        //任务变化
        public void missionChange(int misid)
        {
            if (misid > 0)
            {
                refreshMisNpc(misid);
            }
            else
            {
                setNpcMis();
            }

            Variant data = new Variant();
            data["misid"] = misid;
            this.dispatchEvent(GameEvent.Create(GAME_EVENT.MIS_DATA_CHANGE, this, data));
        }
        //---------------------------本地任务  start-----------------------------------
        // 初始化本地模拟任务，问答，使用道具，点击界面等  
        private void init_local_mis()
        {
            if (_playerMis.Count <= 0)
                return;

            foreach (Variant obj in _playerMis.Values)
            {
                if (obj == null)
                    continue;

                DebugTrace.dumpObj(obj);
                int misid = obj["misid"];
                init_single_localmis(misid);
            }
        }

        private void init_single_localmis(int misid, bool newAccept = false)
        {
            Variant misData = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (misData == null) return;

            Variant goal = this.getMissionGoal(misData);
            if (goal.ContainsKey("qa"))
            {
                Variant v_qa = new Variant();
                v_qa["id"] = misid;
                v_qa["complete"] = false;
                this._mis_qa_arr[goal["qa"]["qamis"].ToString()] = v_qa;
            }
            if (goal.ContainsKey("uitm"))
            {
                Variant v_uitm = new Variant();
                v_uitm["id"] = misid;
                v_uitm["complete"] = false;
            }
            if (goal.ContainsKey("clientgoal"))
            {

            }

            if (newAccept && goal.ContainsKey("operate"))
            {
                Variant v_opt = new Variant();
                v_opt["id"] = misid;
                v_opt["complete"] = false;
                this._mis_operate_arr[goal["operate"]] = v_opt;
            }
        }

        private void clearLocalMis(int misid)
        {//清除当地任务

            if (_playerMis == null || _playerMis.Count <= 0)
                return;

            foreach (Variant acceptMis in _playerMis._arr)
            {
                if (acceptMis["misid"] != misid)
                    continue;

                Variant goal = acceptMis["goal"];
                if (goal.ContainsKey("qa"))
                {
                    Variant g_qa = goal["qa"];
                    this._mis_qa_arr.RemoveKey(g_qa["qamis"].ToString());
                }
                if (goal.ContainsKey("uitm"))
                {
                    Variant g_uitm = goal["uitm"];
                    this._mis_uitm_arr.RemoveKey(g_uitm.ToString());
                }
                if (goal.ContainsKey("clientgoal"))
                {

                }
                if (goal.ContainsKey("operate"))
                {
                    this._mis_operate_arr.RemoveKey(goal["operate"].ToString());
                }
                break;
            }
        }

        public Variant get_mis_cgoal_arr()
        {
            return _mis_cgoal_arr;
        }
        public Variant get_mis_qa(int id)
        {
            return _mis_qa_arr[id.ToString()];
        }

        public void MisDataChange(String type, Variant data)
        {
            int fincnt = 0;
            if (data.ContainsKey("fincnt"))
            {
                fincnt = data["fincnt"];
            }
            else
            {
                fincnt = 1;
            }
            switch (type)
            {
                case "rmis":
                    {
                        //var par:int = lgInGame.lgClient.clientConfig.genConf.GetDalyrepRmisType(data.id);
                        //reflushCgoalsData({type:"rmis", par:par, fincnt:fincnt, rmisid:data.id});
                    } break;
            }
        }

        //type :完成目标类型;  par：完成目标参数； fincnt:完成目标次数
        protected void reflushCgoalsData(Variant data)
        {	//任务数据改变时， 刷新客户端目标数据
            bool hadfresh = false;
            foreach (Variant cgoal in _mis_cgoal_arr.Values)
            {
                foreach (Variant cgoaldata in cgoal["cgoals"].Values)
                {
                    if (data["type"] == cgoaldata["type"])
                    {
                        if ("rmis" == data["type"]._str && data["par"] == cgoaldata["par"])
                        {
                            cgoaldata["fincnt"]._int += data["fincnt"]._int;
                            hadfresh = true;
                            break;
                        }
                    }
                }
                if (hadfresh)
                {
                    bool allfin = true;
                    foreach (Variant obj in cgoal["cgoals"].Values)
                    {
                        if (obj["fincnt"] < obj["cnt"])
                        {
                            allfin = false;
                            break;
                        }
                    }
                    if (allfin)
                    {
                        cgoal["complete"] = true;
                    }
                    break;
                }
            }
            if (hadfresh)
            {
                //lgInGame.lgClient.uiClient.uiUtility.SaveFlag("clientgoal", _mis_cgoal_arr);
                //lgInGame.lgClient.uiClient.uiUtility.FlushFlag();	//存储到缓存
                //_missionLGUI.setMissions(0);						//刷新任务面板
                if ("rmis" == data["type"])
                {
                    //player_rmis_change(data.rmisid);
                }
            }
        }

        //---------------------------本地任务  end-----------------------------------

        //---------------------------委托任务 start--------------------------------------------



        //---------------------------委托任务 end--------------------------------------------

        //---------------------------已接受任务 start----------------------------------
        public Variant misacept
        {
            get
            {
                return _playerMis;
            }
        }

        public Variant getAcceptMisInfo(int misid)
        {
            return _playerMis[misid.ToString()];
        }

        //设置已接受任务
        public void setAcceptMis(Variant data)
        {
            foreach (Variant obj in data._arr)
            {
                addPlayerMiss(obj);
            }
            init_local_mis();
        }

        public Variant no_line_data
        {
            get
            {
                return _no_line_data;
            }
        }

        //获取线路进度
        public int get_line_proc(int misline)
        {
            return _line_data[misline.ToString()];
        }

        public Variant get_allLine_proc()
        {
            return _line_data;
        }

        //操作目标数据
        public Variant getMisGoalAction(int misid)
        {
            return _misAction[misid.ToString()];
        }
        //删除对应已接任务
        private void deletePlayerMis(int misid)
        {
            Variant mis = _playerMis[misid.ToString()];
            if (mis != null)
            {
                _playerMis.RemoveKey(misid.ToString());

                clearLocalMis(misid);
            }
        }

        private bool addPlayerMiss(Variant data)
        {
            int misid = data["misid"];
            Variant misData = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (misData != null)
            {
                if (data.ContainsKey("action"))
                {
                    _misAction[misid.ToString()] = data["action"];
                }
                Variant acceptMis = new Variant();
                acceptMis["misid"] = misid;
                acceptMis["data"] = data;					//任务当前数据
                acceptMis["configdata"] = misData;			//任务配置数据
                acceptMis["isComplete"] = false;			//任务是否完成
                int goalid = 0;
                if (data.ContainsKey("goalid"))
                {
                    goalid = data["goalid"];
                }
                acceptMis["goal"] = getMissionGoal(misData, goalid);	//任务目标
                _playerMis[misid.ToString()] = acceptMis;
                _updataAcceptMisState(acceptMis);
                return true;
            }
            return false;
        }

        private void _updataAcceptMisState(Variant acceptObj, bool flag = false)
        {
            Variant goal = acceptObj["goal"];
            int misid = acceptObj["misid"];
            bool complete = true;
            int i = 0;
            int j = 0;
            Variant acceptData = acceptObj["data"];
            Variant misConf = acceptObj["configdata"];

            Variant appgoal;
            Variant rAppgoalData;

            if (goal.ContainsKey("microclient") && goal["microclient"] == 1)
            {//需要微客户端登陆

            }
            if (goal.ContainsKey("uitm"))
            {//需使用道具id，客户端逻辑处理字段，服务器不验证，=0为无需求；[可选填，默认为0]

            }
            if (goal.ContainsKey("clientgoal"))
            {//客户端目标
                Variant goaldata = _mis_cgoal_arr[goal["clientgoal"]];
                complete = (goaldata != null && goaldata["complete"]._bool);
            }
            if (goal.ContainsKey("operate"))
            {//客户端操作目标，客户端逻辑处理字段，服务器不验证；	
                Variant operateState = _mis_operate_arr[goal["operate"]];
                if (operateState == null || operateState["complete"]._bool)
                {	//没有记录 则认为是已完成		
                    complete = true;
                }
                else
                {
                    complete = false;
                }
            }
            if (goal.ContainsKey("qa"))
            {//问答任务，客户端处理逻辑，服务器不验证；[可选填，默认为空]
                Variant qaObj = goal["qa"];
                int qa_id = qaObj["qamis"];
                if (_mis_qa_arr.ContainsKey(qa_id.ToString()))
                {
                    Variant mis_qa = _mis_qa_arr[qa_id.ToString()];
                    if (mis_qa["complete"] != true)
                    {
                        complete = false;
                    }
                }
            }
            if (goal.ContainsKey("colmon"))
            {

            }
            if (goal.ContainsKey("colitm"))
            {//colitm：需要获得道具数量，可有多个，提交任务后消耗；[可选填，默认为空]
                //tpid：道具类型id
                //cnt：道具数量

            }
            if (goal.ContainsKey("ownitm"))
            {//ownitm：需要获得道具数量，可有多个，提交任务后不消耗；[可选填，默认为空]
                //tpid：道具类型id
                //cnt：道具数量

            }
            if (goal.ContainsKey("kilmon"))
            {//kilmon：需要杀死指定怪物数量，可有多个；[可选填，默认为空]
                //monid：怪物id
                //cnt：杀怪数量
                Variant kilmon = goal["kilmon"];
				for(i = 0; i < kilmon.Count; i++)
				{
					Variant km = kilmon[i];
					Variant mon = (this.g_mgr.g_gameConfM as muCLientConfig).svrMonsterConf.get_monster_data(km["monid"]._int);		
					if(mon == null)
					{
						complete = false;
						continue;
					}	
					int km_count = 0;
					
					float qual_kil = 100;
					if(5 == getMisType(misid))
					{//悬赏任务  杀怪数量根据品质减少

                        Variant appgoal_tmp = null;
						Variant rmisInfo = GetAppawdRmis( misid );//未处理
						Variant playerRmisInfo = GetPlayerRmisInfo(misConf["rmis"]._int);
						if(rmisInfo != null)
						{	//附加奖励更新后 数据更新
                            rAppgoalData = rmisInfo["goal"];
							appgoal = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_appgoal(rAppgoalData["id"]._int);
                            appgoal_tmp = getObjectBykeyValue(appgoal["qual_grp"], "qual", rAppgoalData["qual"]);
                            qual_kil = 100 + appgoal_tmp["per"];
						}
						else if(playerRmisInfo != null && playerRmisInfo["misid"] == misid)//当前未接悬赏信息
						{
                            rAppgoalData = playerRmisInfo["appgoal"];
							appgoal = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_appgoal(rAppgoalData["appgoal"]._int);
							int qual = 1;
							if(rAppgoalData["qual"] != 0)
							{
								qual = rAppgoalData["qual"];
							}
                            appgoal_tmp = getObjectBykeyValue(appgoal["qual_grp"], "qual", qual);
                            qual_kil = 100 + appgoal_tmp["per"];
						}
					}

                    if(acceptObj != null && acceptData != null)
					{
						Variant kms  = acceptData["km"];
						if(kms == null)
							continue;
						for(j = 0; j < kms.Count; j++){
							Variant km_t = kms[j];
                            if (km_t["monid"]._int == km["monid"]._int)
                            {
								km_count = km_t["cnt"];
								break;
							}
						}
					}
					if(km_count < (int)(km["cnt"] * qual_kil / 100))
						complete = false;
				}
            }
            if (goal.ContainsKey("kilmon_map"))
            {//kilmon_map：需要杀死指定地图怪物数量，可有多个；[可选填，默认为空]
                //mapid：地图id
                //cnt：杀怪数量
                Variant kilmon_map = goal["kilmon_map"];
				for(i = 0; i < kilmon_map.Count; i++)
				{
					Variant kmap = kilmon_map[i];
					int kmap_count = 0;
					
					float map_qual_kil = 100;
					if(5 == getMisType(misid))
					{//悬赏任务  杀怪数量根据品质减少

                        Variant appgoal_tmp = null;
						Variant map_rmisInfo = GetAppawdRmis( misid );//未处理
						Variant m_playerRmisInfo = GetPlayerRmisInfo(misConf["rmis"]._int);
						if(map_rmisInfo)
						{	//附加奖励更新后 数据更新
                            rAppgoalData = map_rmisInfo["goal"];
							appgoal = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_appgoal(rAppgoalData["id"]._int);
                            appgoal_tmp = getObjectBykeyValue(appgoal["qual_grp"], "qual", rAppgoalData["qual"]);
                            map_qual_kil = 100 + appgoal_tmp["per"];
						}
						else if(m_playerRmisInfo != null && m_playerRmisInfo["misid"] == misid)//当前未接悬赏信息
						{
                            rAppgoalData = m_playerRmisInfo["appgoal"];
                            appgoal = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_appgoal(rAppgoalData["appgoal"]._int);
							int qual = 1;
							if(rAppgoalData["qual"] != 0)
							{
								qual = rAppgoalData["qual"];
							}
                            appgoal_tmp = getObjectBykeyValue(appgoal["qual_grp"], "qual", qual);
                            map_qual_kil = 100 + appgoal_tmp["per"];
						}
					}
					
					if(acceptObj != null && acceptData != null)
					{
						Variant kmaps = acceptData["km_map"];
						if(kmaps == null)
							continue;
						for(i = 0;i< kmaps.Count;i++)
						{
							Variant kmap_t = kmaps[i];
                            if (kmap_t["mapid"]._int == kmap["mapid"]._int)
							{
								kmap_count = kmap_t["cnt"];
								break;
							}
						}
					}
					int real_kmcnt = getKillmonCnt(misid);
					if(kmap_count < real_kmcnt * map_qual_kil / 100)
						complete = false;
				}

            }
            if (goal.ContainsKey("pzckp"))
            {//在大世界地图PK区域（擂台）中连杀人数，到达该数值后完成任务
                Variant pzckp = goal["pzckp"];
                if (acceptData != null && acceptData.ContainsKey("pzckp"))
                {
                    if (acceptData["pzckp"] == 1)
                    {
                        complete = true;
                    }
                    else
                    {
                        complete = false;
                    }
                }
            }
            if (goal.ContainsKey("pzkp"))
            {//在大世界地图PK区域（擂台）中累计杀人数，到达该数值后完成任务
                Variant pzkp = goal["pzkp"];
                int pzkp_cnt = 0;
                if (acceptData != null && acceptData.ContainsKey("pzkp"))
                {
                    pzkp_cnt = acceptData["pzkp"];
                }
                if (pzkp_cnt < pzkp[0]["cnt"])
                    complete = false;
            }
            if (goal.ContainsKey("kp"))
            {//在大世界地图中（除擂台外）中累计杀人数，到达该数值后完成任务
                Variant kp = goal["kp"];
                int kp_cnt = 0;
                if (acceptData && acceptData.ContainsKey("kp"))
                {
                    kp_cnt = acceptData["kp"];
                }
                if (kp_cnt < kp[0]["cnt"])
                    complete = false;
            }
            if (goal.ContainsKey("joinclan"))
            {//要求加入帮派
                Variant player_data = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.mainPlayerInfo;
                if (player_data.ContainsKey("clanid") && player_data["clanid"] > 0)
                {
                    complete = true;
                }
                else
                {
                    complete = false;
                }
            }
            if (goal.ContainsKey("attchk"))
            {//人物属性

            }
            if (goal.ContainsKey("enterlvl"))
            {//要求进入过某个副本

            }
            if (goal.ContainsKey("eqpchk"))
            {//要求达到装备某个要求

            }
            if (goal.ContainsKey("finlvlmis"))
            {//要求完成某个挑战副本

            }
            if (goal.ContainsKey("finlvldiff"))
            {//需要完成的副本难度

            }
            if (goal.ContainsKey("meri"))
            {//需要完成的星魂目标

            }
            if (goal.ContainsKey("action"))
            {//需要完成的操作目标
                complete = false;
                Variant actionArr = goal["action"];
                Variant actionData = _misAction[misid.ToString()];
                foreach (Variant actionObj in actionArr._arr)
                {
                    if (actionData == null || actionData[actionObj["id"]] != null)
                    {
                        complete = false;
                        break;
                    }
                    else
                    {
                        complete = true;
                    }
                }
            }
            if (goal.ContainsKey("ownskil"))
            {//学习到指定技能

            }
            if (goal.ContainsKey("lvl_score_awd"))
            {//指定副本领取的最高积分奖励

            }

            acceptObj["isComplete"] = complete;
            if (flag && complete)
            {
                //_missionLGUI.OpenMissionGuide(acceptObj.misid);
            }
        }

        //---------------------------已接受任务 end----------------------------------
        //---------------------------可接任务 start -------------------------------------------
        public Variant acceptableMis
        {
            get
            {
                return _acceptable;
            }
        }
        //任务线 状态改变 刷新可接任务
        private void _line_refresh_acceptable(int lineid)
        {
            Variant misArr = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_by_line(lineid);
            if (misArr == null || misArr.Count == 0)
            {
                return;
            }

            foreach (Variant misData in misArr.Values)
            {
                acceptable_reflesh_one_mission(misData, true);
            }

        }

        private void acceptable_reflesh_one_mission(Variant misData, bool adjustState = false )
		{//确保 该任务没有加过
			if( is_acceptable_mis( misData )) 
			{
				if(!_is_in_acceptable(misData["id"]))
				{
					_acceptable.pushBack( misData );
				}				
			}
		}

        //此任务是否是在可接受任务列表
        private bool _is_in_acceptable(int misid)
        {
            if (_acceptable.Count == 0)
            {
                return false;
            }

            foreach (Variant mis in _acceptable._arr)
            {
                if (mis == null) continue;

                if (mis["id"] == misid)
                {
                    return true;
                }
            }

            return false;
        }
        // update 可接任务
        public void acceptable_refault()
		{
			_acceptable = new Variant();
			_line_refresh_acceptable(1);
            Variant miss = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_missions();
			foreach( Variant misData in miss.Values )
			{
				if( is_acceptable_mis( misData )) 
				{
					if(!_is_in_acceptable(misData["id"]))
					{
						_acceptable.pushBack( misData );
					}	
				}
			}
            
			_acceptable._arr.Sort((left, right) =>
            {
                if (left["misline"] > right["misline"])
                    return 1;
                else if (left["misline"] == right["misline"])
                    return 0;
                else
                    return -1;
            });
			
			//_missionLGUI.setMissions();
		}

        // ------------------------------------delete 可接任务 ------------------------------------------------------	
		private void delete_accept_mis(int misid)
		{
			Variant mdata = null;
            for (int i = 0; i < _acceptable.Count; i++)
            {
                mdata = _acceptable[i];
                if (misid == mdata["id"])
                {
                    _acceptable._arr.RemoveAt(i);
                    break;
                }
            }	
//			refresh_mis_npc(misid);//mission_change中修改
		}

        //---------------------------可接任务 end -------------------------------------------

        //---------------------------NPC状态设置 start -------------------------------------------
        //加载时候设置所有npc的状态 
        public void setNpcMis()
        {

        }

        //刷新任务 npc 的头像
        public void refreshMisNpc(int misid)
        {

        }

        //npc刷新任务
        public void updateNpcsMisState(Variant npcs)
        {

        }

        //npc刷新任务
        public void updateNpcMisState(Variant npc)
        {

        }

        private void _updateNpcMisState(LGAvatarNpc npc)
        {

        }

        /*
         * 添加NPC头顶任务状态
         * 0：清除
         * 1：可接
         * 2：已接
         * 3：完成
         */
        private void _addMisTop(int type, LGAvatarNpc npc)
        {

        }

        //获得NPC头顶状态
        public int getNpcMissionTopState(int npcid)
        {

            return 0;
        }

        //---------------------------NPC状态设置 end -------------------------------------------

        //------------------------------条件判断 start-------------------------------
        //此任务是否是已接受任务
        public bool is_accepted_mis(int misid)
        {
            Variant acceptMis = _playerMis[misid.ToString()];
            return (acceptMis != null) ? true : false;
        }

        //是否为可接任务，1。前置任务是否完成 2。是否已经接受
        public bool is_acceptable_mis(Variant misData)
        {
            if (misData == null)
                return false;

            //是否为可接任务，1。前置任务是否完成 2。是否已经接受
            //line.fin 当前完成至任务id, line.accept 是否接受任务线当前任务
            int i = 0;
            //Variant itm = null;
            int misid = misData["id"];
            //是否已经接过
            if (is_accepted_mis(misid))
                return false;

            if (misData.ContainsKey("misline") && !(misData.ContainsKey("dalyrep") && misData["dalyrep"] > 0 && misData["misline"] != 1))
            {
                //任务线任务小于或等于任务线完成进度表示此任务已经完成过
                if (!misData.ContainsKey("rmis"))
                {
                    if (_line_data.ContainsKey(misData["misline"]._str))
                    {
                        if (_line_data[misData["misline"].ToString()] >= misid)
                            return false;
                    }
                }
            }
            Variant accept = misData["accept"];
            if (accept.ContainsKey("unaccept_able") && accept["unaccept_able"])
                return false;

            int premis = 0;
            if (accept.ContainsKey("premis") && accept["premis"]._str !="")
            {
                premis = accept["premis"]._int;
            }
            Variant accept_mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(premis);

            //如果地图没有这条线 就不用接受了。
            if (accept_mis_data != null && !_line_data.ContainsKey(accept_mis_data["misline"].ToString()))
            {
                return false;
            }

            if (accept_mis_data != null && _line_data[accept_mis_data["misline"].ToString()] < premis)
            {
                //前置任务未完成
                //小于premis表示此任务前置条件未完成
                return false;
            }

            //主线任务只要前置完成就一直显示一直显示
            if (misData["misline"] == 1)
            {
                if (premis == 0)
                {
                    //主线任务前置任务为0，即无限制的任务，需要判断是否有show_premis字段，有才显示,没有或show_premis任务未完成则不显示
                    if (accept.ContainsKey("show_premis"))
                    {
                        int show_premis = accept["show_premis"];
                        Variant show_premis_mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(show_premis);
                        if (show_premis_mis_data != null && _line_data[show_premis_mis_data["misline"].ToString()] >= show_premis)
                            return true;
                    }

                    return false;
                }
                else
                {
                    //主线任务的前置任务已经完成，除了职业限制外，都显示
                    if (accept.ContainsKey("attchk"))
                    {
                        Variant attchks = accept["attchk"];
                        for (i = 0; i < attchks.Count; i++)
                        {
                            Variant ck = attchks[i];
                            string ckname = ck["name"];
                            if (ckname == "carr")
                            {
                                //								return attchk.check_carr(ck.and,_lgInGame.lgGD_Gen.netData);
                            }
                        }
                    }
                    return true;
                }
            }
            //if(!ConfigUtil.attchk(accept.attchk, this._lgInGame.selfPlayer.netData))
            //	return false;

            Variant chan = accept["clan"];//帮派需求
            if (chan != null)
            {
                //				if( !_main._gamescene.faction_mgr.CheckFaction( chan[0] ) )
                //					return false;
            }
            //非任务线任务，是否已到达最大完成次数
            if (_no_line_data.ContainsKey(misid.ToString()))
            {
                if (!misData.ContainsKey("rmis"))
                {
                    //酒馆任务
                    int leftcnt = _no_line_data[misid.ToString()];
                    if (leftcnt <= 0)
                        return false;
                }
            }
            //判断时间是否过期
            if (misData.ContainsKey("tmchk"))
            {
                if (!is_in_open_tm(misid))
                    return false;
            }
            //微端任务 当没有微端时 不显示任务
            Variant goal = this.getMissionGoal(misData);
            if (goal.ContainsKey("microclient"))
            {
                /*if( _main.mini_url == "" ) return false;*/
            }
            //判断日常任务完成的数量
            if (is_dalyrep_mis(misData))
            {
                if (!_no_line_data.ContainsKey(misid.ToString()))
                    return true;//如果没有此数据，说明此任务未完成过

                int havec = _no_line_data[misid.ToString()];
                if (havec <= 0)
                    return false;
            }

            return true;
        }

        //判断玩家是否接受酒馆任务
        public bool is_accepted_rmis(int tp)
        {
            foreach (Variant acceptMis in _playerMis._arr)
            {
                if (acceptMis == null) continue;

                Variant confData = acceptMis["configdata"];
                if (confData != null && confData.ContainsKey("rmis"))
                {
                    //Variant info = this.lgInGame.lgGD_RMis.GetRmisDesc(confData.rmis);
                    //if(info != null && uint(info.type) == tp)
                    //{	//1号类型的酒馆任务 不能换线， 2号类型的随机任务 可以换线
                    //return true;
                    //}				
                }
            }
            return false;
        }

        //是否是日常任务
        public bool is_dalyrep_mis(Variant misData)
        {
            if (misData == null)
                return false;

            if (misData.ContainsKey("dalyrep") && misData["dalyrep"] > 0)
                return true;

            return false;
        }

        /**
		 *是否在开放时间内开放
		 * @param misid 任务编号 
		 */
        public bool is_in_open_tm(int misid)
        {
            Variant mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (!mis_data.ContainsKey("tmchk"))
                return true;

            if (!mis_data["tmchk"].ContainsKey("tb"))
            {
                return true;
            }

            //当前时间
            long tmNumber = (this.g_mgr.g_netM as muNetCleint).CurServerTimeStampMS;

            //return ConfigUtil.check_tm(tmNumber,mis_data.tmchk);
            return false;
        }

        //已达成目标 不一定已提交
        public bool is_mis_complete(int misid)
        {
            Variant acceptMis = _playerMis[misid.ToString()];
            return (acceptMis != null) ? acceptMis["isComplete"]._bool : false;
        }
        //已完成该任务
        public bool is_mis_has_complete(int misid)
        {
            Variant mis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (mis == null) return false;

            int misline = mis["misline"];
            if (misline <= 0)
            {	//非主线
                return _no_line_data.ContainsKey(misid.ToString());
            }
            else
            {	//主线
                if (mis.ContainsKey("rmis"))
                {	//酒馆任务 不走这里，不需要知道是否提交成功，只需要知道 是否达成目标
                    return false;
                }
                else
                {
                    Variant misaccept = _playerMis[misid.ToString()];
                    if (mis.ContainsKey("dalyrep") && mis["dalyrep"] > 0)
                    {//循环任务
                        Variant acpData = misaccept["data"];
                        if (misaccept != null && acpData.ContainsKey("cntleft") && acpData["cntleft"] >= 0)
                        {
                            return false;
                        }
                        foreach (Variant acpObj in _acceptable._arr)
                        {
                            if (acpObj == null) continue;

                            if (acpObj["id"] == misid)
                            {
                                return false;
                            }
                        }
                    }
                    return _line_data.ContainsKey(misline.ToString()) && (_line_data[misline.ToString()] >= misid);
                }
            }
            return false;
        }

        public bool IsMisCanAutocommit(int misid)
        {
            Variant misConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (!misConf) return false;
            Variant noAutoCommit = null;
            //noAutoCommit = _lgInGame.lgClient.clientConfig.genConf.GetNoAutoCommit(misConf.misline);
            if (noAutoCommit != null)
            {
                if (!noAutoCommit.ContainsKey("misid") || noAutoCommit["misid"] > 0 && noAutoCommit["misid"] < misid)
                {
                    return false;
                }
            }

            if (misConf.ContainsKey("goaladdition_daly"))
            {//循环任务手动提交
                return false;
            }

            Variant awards = misConf["awards"];
            if (!awards.ContainsKey("npc") || awards["npc"] <= 0)
            {	//没有领奖NPC的 任务 都可自动提交
                return !misConf.ContainsKey("rmis");//可双倍完成的酒馆任务除外 
            }
            return false;
        }

        /**
		 *判断当前任务是否在非任务线里面
		 */
        public bool is_in_no_line_data(Variant mis)
        {
            if (_no_line_data.ContainsKey(mis["id"].ToString()))
                return true;

            return false;
        }

        /**
		 * 判断是否属于主线任务
		 */
        public bool is_main_mis(int misid)
        {
            Variant mainMis = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_by_line(1);
            if (mainMis != null && mainMis.Count > 0)
            {
                return (mainMis[misid] != null);
            }
            return false;
        }

        //任务是否已经达成目标
        public bool is_mis_goal(int misid)
        {
            Variant acept_mis_info = _playerMis[misid.ToString()];
            if (acept_mis_info == null)
                return false;

            Variant mis_data = acept_mis_info["configdata"];
            Variant goal = acept_mis_info["goal"];
            Variant kilmon = goal["kilmon"];
            // 更新 km
            Variant acceptData = acept_mis_info["data"];
            if (acceptData != null && acceptData.ContainsKey("km"))
            {
                foreach (Variant km in acceptData["km"]._arr)
                {
                    for (int i = 0; i < kilmon.Count; i++)
                    {
                        Variant kkm = kilmon[i];
                        if (km["monid"] == kkm["monid"])
                        {
                            if (km["cnt"] < kkm["cnt"])
                                return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
		 *是否是新手任务
		 * @mid 任务编号 
		 */
        public bool isNewPlayermis(int misid)
        {
            //获得任务。
            Variant mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);

            if (mis_data != null && mis_data.ContainsKey("spcmis") && mis_data["spcmis"] == 1)
            {
                return true;
            }
            return false;
        }

        /**
         *是否是可以放弃任务。
         * @mid 任务编号 
         */
        public bool is_cant_abord_mis(int misid)
        {
            Variant mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
            if (mis_data != null && mis_data.ContainsKey("cant_abord") && mis_data["cant_abord"] == 1)
            {
                return true;
            }
            return false;
        }

        //------------------------------条件判断 end-------------------------------
        //------------------------------其他功能处理 start--------------------------------------
        /**
		 *获得 接受任务后杀怪额外掉落掉道具的怪物编号 
		 */
		public int get_mon_id_by_killmonitm(Variant kilmonitm,int tpid)
        {
			for(int i = 0; i < kilmonitm.Count; i++)
            {
				Variant ki = kilmonitm[i];
				if(ki["tpid"] == tpid)
					return ki["monids"][0];
			}
			return 0;
		}

        /**
		 *添加请求的日常任务的数据 
		 */
		private void add_daymis_info(Variant dayarr, Variant mis)
		{
			if(mis == null)
			{
				return;
			}
			if(is_dalyrep_mis(mis))
			{
				if(_no_line_data.ContainsKey(mis["id"]._str))
					return;
				dayarr.pushBack(mis["id"]);
			}
		}
		private void add_mis_info(Variant no_line_arr,Variant line_arr, Variant mis)
		{
			if(mis == null)
			{
				return;
			}
			int misline = mis["misline"];
			if(misline <= 0)
			{
				if(_no_line_data.ContainsKey(mis["id"]._str))
					return;
				
				no_line_arr.pushBack(mis["id"]);
			}
            if(_line_data.ContainsKey(misline.ToString()))
					return;
			bool has = false;
			for(int k = 0; k < line_arr.Count; k++)
			{
				if(line_arr[k] == misline)
				{
					has = true;
					break;
				}
			}
			if(!has)
                line_arr.pushBack(misline);
		}
		//-------------------------------其他功能处理 end--------------------------------------
        //-----------------------------------获取任务奖励数据  start-------------------------------------------------------------
		public Variant getMisAward(int misid, int carr = -1)
		{
			int exp = 0;
			int skexp = 0;
			int meript = 0;
			int gld = 0;
			int clana = 0;
			int clang = 0;
			int clangld = 0;//战盟资金奖励
			int clanyb = 0;
			Variant achives = new Variant();
			Variant eqp = new Variant();
			Variant itm = new Variant();
            Variant data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
			if(data == null)
			{
				return null;
			}
			Variant misAwards = new Variant();
			if(data.ContainsKey("gawards") && data["gawards"] != null)
			{//匹配目标奖励
				Variant goal;
				if(_playerMis[misid])
				{
					goal = _playerMis[misid.ToString()]["goal"];
				}
				else
				{
					goal = getMissionGoal(data);
				}
				foreach(Variant gawd in data["gawards"]._arr)
				{
					if(gawd["gid"] == goal["id"])
					{
						misAwards = gawd;
						break;
					}
				}
			}
			else
			{
				misAwards = data["awards"];
			}
			
			Variant awards = misAwards["award"];
			if(awards == null)
				return null;
			//int i;
			foreach(Variant award in awards._arr)
			{
				bool is_licit_award = true;	//是否合理
				if(award["carrid"]._int > 0)
				{
					int bcarr;
					if(carr < 0)
					{
                        Variant selfData = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.mainPlayerInfo;
						bcarr = selfData["carr"];
					}
					else
					{
						bcarr = carr;
					}
					if(bcarr != award["carrid"]._int)
					{
						is_licit_award = false;
					}
				}
				if(!is_licit_award) continue;
				if(award.ContainsKey("exp"))
				{
					exp += award["exp"]._int;
				}
				if(award.ContainsKey("skexp")){
					skexp += award["skexp"]._int;
				}
				if(award.ContainsKey("gld")){
					gld += award["gld"]._int;
				}
				if(award.ContainsKey("clan")){
					Variant clanAwds = award["clan"];
					for( int n = 0; n < clanAwds.Count; n++){
						clana += clanAwds[n]["clana"]._int;
						clang += clanAwds[n]["clang"]._int;
						clangld += clanAwds[n]["gold"]._int;
						clanyb += clanAwds[n]["yb"]._int;
					}
				}
				if(award.ContainsKey("achive")){
                    achives.pushBack(award["achive"]);
				}
				if(award.ContainsKey("eqp")){
					for(int k = 0; k < award["eqp"].Count; k++){
						eqp.pushBack(award["eqp"][k]);
					}
				}
				if(award.ContainsKey("itm")){
					for(int j = 0; j < award["itm"].Count;j++){
						itm.pushBack(award["itm"][j]);
					}
				}
				if(award.ContainsKey("meript"))
				{
					meript = award["meript"];
				}
			}
            Variant ret = new Variant();
            ret["exp"] = exp;
            ret["skexp"] = skexp;
            ret["gld"] = gld;
            ret["clana"] = clana;
            ret["clang"] = clang;
            ret["achives"] = achives;
            ret["eqp"] = eqp;
            ret["itm"] = itm;
            ret["meript"] = meript;
            ret["clangld"] = clangld;
            ret["clanyb"] = clanyb;

            return ret;
		}
		//-----------------------------------获取任务奖励数据 end-------------------------------------------------------------
        //-------------------------------主线任务奖励Start-------------------------------------------------
		private int _mlineawd = 0;
		public void SetMlineawd(int misid)
		{
			_mlineawd = misid;
		}
		
		public void AddMlineawd(int misid)
		{
			_mlineawd = misid;
			//_lgInGame.mainUI.AddMlineawd();
		}
		
		public int GetMlineawd()
		{
			return _mlineawd;
		}
		//-------------------------------主线任务奖励End-------------------------------------------------
		//不在相同任务线的前置任务。。
		private void preMisComplete(int misid)
		{
            //var relArr:Array = (this.m_mgr.g_gameConfM as muCLientConfig).localGeneral.GetMisRelate(misid);
            //if(relArr)
            //{
            //    for each( var mid:uint in relArr )
            //    {
            //        var misData:Object =  _lgInGame.lgClient.svrGameConfig.svrMissionConf.get_mission_conf(mid);
            //        acceptable_reflesh_one_mission( misData, true );
            //    }
            //}
		}
		//更新酒馆任务
		public void UpdateRmis(int misid)
		{
			foreach(Variant obj in _playerMis.Values)	
			{
				if(obj["misid"]._int == misid)
				{
					if(!obj["isComplete"]._bool)
					{
						_updataAcceptMisState( obj );
					}
					break;
				}
			}
		}
		
		/**
		 * 判断是否满足条件提交任务
		 */
		public bool is_attchk_commit_mis(int misid)
		{
			bool isCommit = true;
			Variant selfData = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.mainPlayerInfo;
			Variant misData = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
			if(misData != null && misData.ContainsKey("goal"))
			{
				Variant attchk = misData["goal"]["attchk"];
				if(attchk != null)
				{
					foreach(Variant obj in attchk._arr)
					{
						if(obj["name"] == "level" && selfData["level"]._int < obj["min"]._int)
						{
							isCommit = false;
							//var ui_mainui:LGIUIMainUI = this._lgInGame.lgClient.uiClient.getLGUI(UIName.LGUIMainUIImpl) as LGIUIMainUI;
							//ui_mainui.output_server_err(-1204);
							break;
						}
					}
				}
			}
			
			return isCommit;
		}

        //---------------------------------玩家操作返回 使用道具、操作、答题等 start------------------------------------------------------------------		
		/**
		 * 使用道具后回调，查看是否有 使用该道具的任务，有的话标为完成 
		 * @param item_id
		 * 
		 */		
		public void on_item_use(int item_id)
		{
			if(!this._mis_uitm_arr.ContainsKey(item_id.ToString()) || this._mis_uitm_arr[item_id.ToString()]["complete"] == true)
				return;		
			this._mis_uitm_arr[item_id.ToString()]["complete"] = true;
			
			int misid = _mis_uitm_arr[item_id.ToString()]["id"];
			Variant accpetData = _playerMis[misid.ToString()];
			if(accpetData != null)
			{
				accpetData["isComplete"] = true;
			}
			this.missionChange(misid);
		}
		
		//某某操作完成 更新目标是客户端操作的任务状态
		public void OnOperateComplete( string type )
		{
			Variant operate = _mis_operate_arr[ type ];
			if( operate != null )
			{
				operate["complete"] = true;
				
				int misid = operate["id"];
				Variant accpetData = _playerMis[ misid.ToString() ];
				if(accpetData != null)
				{
					accpetData["isComplete"] = true;
				}
				this.missionChange(misid);
			}
		}
		/**
		 * 问题回答完后回调，查看是否有回答该问题的任务，有的话标为完成
		 * @param qa_id
		 * 
		 */		
		public void on_qa_answer(int qa_id)
        {
			if(!this._mis_qa_arr.ContainsKey(qa_id.ToString()) || this._mis_qa_arr[qa_id.ToString()]["complete"] == true)
				return;
			
			this._mis_qa_arr[qa_id.ToString()]["complete"] = true;
			
			int misid = _mis_qa_arr[qa_id.ToString()]["id"];
			Variant accpetData = _playerMis[ misid.ToString() ];
			if(accpetData != null)
			{
				accpetData["isComplete"] = true;
			}
			this.missionChange(misid);
		}

		/**
		 *接受任务 移动。
		 * @mid 任务编号。 
		 */
		public void accpet_move(int mid,bool ignore_level = false)
        {
			//前提显示。如果在挂机状态下。或者在副本中。可以此不用操作。
//			if(_main._gamescene.auto_game_mgr && _main._gamescene.auto_game_mgr.running)
//			{
//				return;
//			}
			Variant mdata = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(mid);			
			if(mdata == null)
			{
				return;
			}				
			//int mpid = 0;  //地图编号
			//int x = 0;	   //去的x轴
			//int y = 0;	   //去的y轴
			//int tpid = 0;  //物品编号。
			Variant arr = null; 
			Variant pos = null; //位置数据源。
			//Variant runobj = null;  //跑动到终点 需要做什么的标记参数
			//自动寻路到接受任务的npc  
			// 为什么要 判断成立一个就return 一个。的原因是 。有些东西有目标。没有坐标。对于这些没有坐标的。就不与处理了。
			// 并且的放到下一个里面去。
			if(mdata != null && mdata.ContainsKey("goal"))
			{
				//string str = "";			
				Variant goal = getMissionGoal(mdata);				
				if(goal.ContainsKey("kilmon"))
				{
					//去找怪
					arr = goal["kilmon"][0];
					if(arr != null)
					{
						string k_event = "mon_"+arr["monid"]+"_";
						if(arr.ContainsKey("pos"))
						{
							pos = arr["pos"][0];
							k_event += pos["mpid"]+"_"+pos["x"]+"_"+pos["y"];
						}
						else
						{
							k_event += "0_0_0";
						}						
						int levelid = arr["level_id"]._int;
						if( levelid != 0 )
						{
							//如果正在新手引导 侠客行 则不自动进行
//							if( _main._gamescene._guide_mgr.GetCurrRookieType() == "travel" )
//							{
//								var userdata:Object = _main._gamescene._guide_mgr.GetRookieUserdata(); 
//								if( levelid == int(userdata.travelid) )
//								{
//									return;
//								}
//							}					
						}
						
						k_event += "_"+levelid + "_"+mid;
						//_missionLGUI.accept_move_execute_link_event(k_event);
					}	
				}				
				if(goal.ContainsKey("ownitm"))
				{
					//需要获得道具 不消耗。
					arr = goal["ownitm"][0];
					if(arr != null){
						string ownitmstr = get_need_item_str(arr, mdata["id"]._int);
						//_missionLGUI.accept_move_execute_link_event(ownitmstr);
					}
				}				
				if(goal.ContainsKey("colitm"))
				{
					//需要获得道具 消耗。
					arr = goal["colitm"][0];
					if(arr != null){
						string evntestr = get_need_item_str(arr, mdata["id"]._int);
						//_missionLGUI.accept_move_execute_link_event(evntestr);
						//							
					}
				}				
				if(goal.ContainsKey("enterlvl"))
				{
					//进入副本
					string enterlvlstr = "enterlvl_" + goal["enterlvl"]._str;
					//_missionLGUI.accept_move_execute_link_event(enterlvlstr);
				}
                if (goal.ContainsKey("jcamp"))
				{
					//加入阵营
					//_missionLGUI.accept_move_execute_link_event( "openui_jcamp" );
				}
                if (goal.ContainsKey("qa"))
				{
					//问答npc
					to_npc_open_mis(mid,goal["qa"]["npc"]._int);
					return;
				}
                if (goal.ContainsKey("talknpc"))
				{
					//去找npc 对话。
					//接受任务后。如果没有完成这个任务的话就没有必要在打开了。
					if(is_mis_complete(mid))
					{
						//_missionLGUI.to_find_npc(mid,goal.talknpc);
					}
					return;
				}					 				
			}
		}		
		/**
		 *开始自动任务寻路状态
		 * @fobj 寻路带的数据 
		 */
		private void open_findway(Variant fobj)
		{
		}		
		/**
		 *关闭自动任务寻路状态 
		 */
		public void close_findway()
        {
            //_findwayobj.isfind = false;
            //_findwayobj.sttm = getTimer();
            //_findwayobj.mid = 0;
            //_findwayobj.npcid = 0;
//			_main._gamescene._guide_mgr.DelRookieHideFlag( "misfindway" );	
		}		
		/**
		 *自动任务寻路状态 
		 */
		private void findway_process()
		{
            //if(!_findwayobj["isfind"].)
            //    return;
			
            //var ntm:int = getTimer();			
            ////如果停留时间到了。让他们去寻路
            //if(ntm - _findwayobj.sttm >= _findwayobj.stoptm)
            //{
            //    close_findway();
            //}
		}
		
		/**
		 *自动跑到npc 处并且打开任务面板
		 * @mid 任务编号。
		 * @npc_id npc编号。 
		 */
		public void auto_to_npc_open_mis(int mid,int npc_id)
		{
			//open_findway({mid:mid,npcid:npc_id});
		}
		
		
		/**
		 *跑到npc 处并且打开任务面板。
		 * @mid 任务编号。
		 * @npc_id npc编号。 
		 */
		public void to_npc_open_mis(int mid,int npc_id,bool ignore_level=false)
		{	
			//前提显示。如果在挂机状态下。或者在副本中.可以此不用操作。
//			if(_main._gamescene.auto_game_mgr && _main._gamescene.auto_game_mgr.running)
//			{
//				return;
//			}			
			//lgInGame.clientAI.move_to_npc(npc_id,mid);
		}		
		
        //public void process(float tm)
        //{
        //    findway_process();
        //}
        //---------------------------------玩家操作返回 使用道具、操作、答题等 end---------










        //------------------------------酒馆任务 start--------------------------------------------
        private Variant _rmisConfData = new Variant();//缓存酒馆任务配置信息
        private Variant _playerRmis = new Variant();//玩家身上的酒馆信息
        private Variant _rmis_share = new Variant();//共享任务信息
        private int _freeInsure = 0;//免费购买保险的次数
        private Variant _appawdRmis;	//有附加奖励的酒馆任务信息

        private bool _reSetPlaymis = false;
        private bool _reSetShare = false;

        private void requestRmisInfo(int id)
        {
            if (id != 0)
            {
                //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetRmisInfo(id);
            }
        }

        //初始化数据
        public void initRmisData()
        {
            Variant rmisConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_rmiss();
            if (rmisConf != null)
            {
                for (int i = 0; i < rmisConf.Count; i++)
                {
                    Variant data = rmisConf[i];
                    Variant rmisObj = _rmisConfData[data["id"]._str];
                    if (rmisObj == null)
                    {
                        _rmisConfData[data["id"]._str] = data;
                    }
                    else
                    {
                        foreach (string key in data.Keys)
                        {
                            rmisObj[key] = data[key];
                        }
                    }
                }
            }
        }

        //初始化玩家自己的酒馆信息
        public void InitPlayerRmisData()
        {
            Variant obj;
            Variant detail = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.mainPlayerInfo;
            _reSetPlaymis = true;
            _reSetShare = true;
            foreach (string i in _rmisConfData.Keys)
            {
                obj = _rmisConfData[i];
                //不做帮派条件检测   帮派信息可能未被初始化
                //if( lgPlayer.is_attchk( obj.attchk, detail ) ) 
                //{
                	requestRmisInfo( obj["id"]._int );
                //}
            }
            //防止玩家已经接到酒馆任务  自身却超过接受任务的条件 的情况
            Variant acceptRmiss = GetPlayerAcceptedRmis();
            for (int i = 0; i < acceptRmiss.Count; i++)
            {
                requestRmisInfo(acceptRmiss[i]._int);
            }
            //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetAppawd();
            //(this.g_mgr.g_netM as muNetCleint).igMissionMsgs.GetRmisShareInfo();
            //			GetInsureInfo();
            //			initCDMaskBitmapData();	
        }
        static public bool IsArrayHasValue(Variant data, int value)
        {
            if (data.Count <= 0) 
                return false;

            foreach (Variant item in data._arr)
            {
                if (value == item._int)
                    return true;
            }
            return false;
        }
        //获取已经接受任务中是酒馆任务的酒馆id
        public Variant GetPlayerAcceptedRmis()
        {
            Variant retArray = new Variant();
            if (misacept.Count > 0)
            {
                foreach (Variant obj in misacept.Values)
                {
                    Variant misObj = obj["configdata"];
                    if (misObj == null)
                        continue;
                    if (misObj.ContainsKey("rmis"))
                    {
                        retArray.pushBack(misObj["rmis"]);
                    }
                }
            }
            return retArray;
        }

        //获取酒馆任务中的活动任务
        public Variant GetRmisActivities()
        {
            Variant retArray = new Variant();
            foreach (Variant obj in _rmisConfData._arr)
            {
                if (obj.ContainsKey("part_type") && (obj["part_type"] & 8) > 0)
                {
                    retArray.pushBack(obj);
                }
            }
            return retArray;
        }
        //酒馆任务是否可以接受
        public bool IsRmisCanAccept(int rmisId)
        {
            bool ret = false;
            Variant playRmis = _playerRmis[rmisId.ToString()];
            if (playRmis != null)
            {
                Variant rmisObj = _rmisConfData[rmisId.ToString()];
                int cnt = 0;
                if (rmisObj.ContainsKey("rmis_share"))
                {
                    Variant tdata = _rmis_share[rmisObj["rmis_share"]];
                    if (tdata != null)
                    {
                        cnt = tdata["cnt"];
                    }
                    else
                    {
                        Variant svr = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.GetRmisShare(rmisObj["rmis_share"]);
                        if (svr != null)
                        {
                            cnt = svr["dalycnt"];
                        }
                    }
                }
                else
                {
                    cnt = playRmis["cnt"];
                }
                if (cnt > 0)
                {
                    Variant rmisConf = _rmisConfData[rmisId.ToString()];
                    Variant clan = rmisConf["clan"];//帮派条件
                    ret = clan == null /*||  _main._gamescene.faction_mgr.CheckFaction(clan[0])*/;
                }
            }
            return ret;
        }

        public int GetLeftFreeInsure()
        {
            return this._freeInsure;
        }

        //获得玩家酒馆任务 当前品质任务
        public int GetPlayerRmisCurrQualMis(int rmisId)
        {
            int misid = 0;
            Variant playRmis = _playerRmis[rmisId.ToString()];
            if (playRmis != null)
            {
                misid = playRmis["misids"][0];//运镖任务  只有一个子任务	
            }
            return misid;
        }

        //获得品质icon
        public Variant GetRmisQualIcon(int rmisid)
        {
            Variant ret = null;
            Variant rmisData = _rmisConfData[rmisid.ToString()];
            if (rmisData != null)
            {
                ret = new Variant();
                foreach (Variant qualObj in rmisData["rqual"].Values)
                {
                    foreach (Variant qual_grp in qualObj["qual_grp"].Values)
                    {
                        //if( rmisData.type == 1 ) 
                        //						misId = misObj.id;
                        ret.pushBack("rmis_icon_" + qual_grp["qual"]);
                        //目前运镖任务 只有一个子任务
                        //						break;
                    }
                }
            }
            return ret;
        }

        //获取 玩家当前酒馆任务的状态
        public Variant GetPlayerRmisInfo(int rmisid)
        {
            Variant ret = null;
            Variant qualData = null;
            Variant tdata;
            Variant rsConf;
            Variant rqual;
            Variant rmisData = _rmisConfData[rmisid.ToString()];
            Variant playerRmis = _playerRmis[rmisid.ToString()];
            if (rmisData != null && playerRmis != null)
            {
                switch (rmisData["type"]._int)
                {
                    case 1://运镖任务
                        {
                            ret = new Variant();
                            if (rmisData.ContainsKey("rmis_share"))
                            {
                                tdata = _rmis_share[rmisData["rmis_share"]];
                                if (tdata == null)
                                {
                                    tdata = new Variant();
                                    rsConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.GetRmisShare(rmisData["rmis_share"]);
                                    if (rsConf != null)
                                    {
                                        tdata["cnt"] = rsConf["dalycnt"];
                                        tdata["fincnt"] = 0;
                                    }
                                    _rmis_share[rmisData["rmis_share"]] = tdata;
                                }
                                ret["cnt"] = tdata["cnt"];
                                ret["fincnt"] = tdata["fincnt"];
                            }
                            else
                            {
                                ret["cnt"] = playerRmis["cnt"];
                                ret["fincnt"] = playerRmis["fincnt"];
                            }
                            ret["misid"] = playerRmis["misids"][0];//运镖任务  只有一个子任务		
                            ret["tm"] = playerRmis["rtm"];
                            Variant rqualtemp = playerRmis["rqual"][0];
                            ret["rqualid"] = rqualtemp["id"];
                            ret["failcnt"] = rqualtemp["failcnt"];
                            ret["freecnt"] = rqualtemp["freecnt"];
                            ret["qual"] = rqualtemp["qual"];
                            rqual = rmisData["rqual"][rqualtemp["id"]];
                            qualData = rqual["qual_grp"][rqualtemp["qual"]];

                            ret["yb"] = qualData["ryb"];
                            ret["percent"] = qualData["uprate"];
                            if (rqualtemp["failcnt"] > qualData["failcnt"])
                            {
                                ret["addper"] = (rqualtemp["failcnt"] - qualData["failcnt"]) * qualData["failper"];
                            }
                            else
                            {
                                ret["addper"] = 0;
                            }
                            ret["type"] = rmisData["type"];//类型
                        } break;
                    case 2://悬赏
                        {
                            ret = new Variant();
                            ret["misid"] = playerRmis["misids"][0];//运镖任务  只有一个子任务		
                            if (rmisData.ContainsKey("rmis_share"))
                            {
                                tdata = _rmis_share[rmisData["rmis_share"]];
                                if (tdata == null)
                                {
                                    tdata = new Variant();
                                    rsConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.GetRmisShare(rmisData["rmis_share"]);
                                    if (rsConf != null)
                                    {
                                        tdata["cnt"] = rsConf["dalycnt"];
                                        tdata["fincnt"] = 0;
                                        tdata["dalyawd"] = false;
                                    }
                                    _rmis_share[rmisData["rmis_share"]] = tdata;
                                }
                                ret["cnt"] = tdata["cnt"];
                                ret["fincnt"] = tdata["fincnt"];
                                ret["dalyawd"] = tdata["dalyawd"];
                            }
                            else
                            {
                                ret["cnt"] = playerRmis["cnt"];
                                ret["fincnt"] = playerRmis["fincnt"];
                                ret["dalyawd"] = playerRmis["dalyawd"];
                            }
                            Variant appawd = new Variant();
                            Variant appgoal = new Variant();
                            foreach (Variant obj in playerRmis["rqual"]._arr)
                            {
                                Variant temp = getObjectBykeyValue(rmisData["rqual"], "id", obj["id"]._int);
                                //Variant temp = rmisData["rqual"][obj["id"]._int];
                                foreach (Variant qualObj in temp["qual_grp"]._arr)
                                {
                                    if (qualObj != null && qualObj["qual"]._int == obj["qual"]._int)
                                    {
                                        qualData = qualObj;
                                        break;
                                    }
                                }

                                if (temp.ContainsKey("reflushmis"))
                                {
                                    appgoal = temp;
                                    foreach (string s in qualData.Keys)
                                    {
                                        appgoal[s] = qualData[s];
                                    }
                                }
                                else
                                {
                                    appawd = temp;
                                    foreach (string key in qualData.Keys)
                                    {
                                        appawd[key] = qualData[key];
                                    }
                                }
                            }
                            ret["appawd"] = appawd;
                            ret["appgoal"] = appgoal;
                            ret["type"] = rmisData["type"];//类型
                        } break;
                }
            }
            return ret;
        }

        //从数组中去值对
        public Variant getObjectBykeyValue(Variant data, string key, Variant value)
        {
            if (data == null || data.Count <= 0)
                return null;

            foreach (Variant obj in data._arr)
            {
                if (obj != null && obj[key]._int == value._int)
                {
                    return obj;
                }
            }

            return null;
        }

        //获得酒馆任务描述
		public Variant GetRmisDesc( int rmisid )
		{
			return _rmisConfData[ rmisid.ToString() ];
		}

        public void onCompleteRmis(int misid)
		{
			if(_appawdRmis != null)
			{
				if(_appawdRmis[misid.ToString()] != null)
				{
                    _appawdRmis[misid.ToString()] = null;
				}
                Variant mis_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf(misid);
				Variant plyRmis = _playerRmis[mis_data["rmis"].ToString()];
				if(mis_data != null && plyRmis != null)
				{
					Variant rmisObj = _rmisConfData[ mis_data["rmis"]._str ];
					if(rmisObj.ContainsKey("rmis_share"))
					{
						Variant tdata = _rmis_share[rmisObj["rmis_share"]];
						if(tdata)
						{
							tdata["fincnt"]++;	
						}
					}
					else
					{
						_playerRmis[mis_data["rmis"].ToString()]["fincnt"]++;
					}
					//lgInGame.lgGD_miss.MisDataChange( "rmis", {id:mis_data.rmis});
				}
			}
		}

        public void onAcceptRmisMis(int misid)
        {
            Variant rmisObj = _rmisConfData[misid.ToString()];
            if (rmisObj.ContainsKey("rmis_share"))
            {
                Variant tdata = _rmis_share[rmisObj["rmis_share"]];
                if (tdata)
                {
                    tdata["cnt"]--;
                }
            }
        }

        public int GetRmisAppwdExp(int rmisid, int misid)
		{
			Variant rmisData = GetRmisDesc( rmisid );								
			if(rmisData != null && rmisData.ContainsKey("appawd"))
			{	//有附加奖励的 酒馆任务 ，如悬赏随机任务
				Variant rmisInfo = GetAppawdRmis( misid );
				if(rmisInfo != null)
				{					
					Variant appaward = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mis_appawd(rmisData["appawd"]);						
					if(appaward != null)
					{
						Variant qual_grp = appaward["qual_grp"];	
						int qual = rmisInfo["qual"]._int;
						if(qual > 0 && qual_grp != null && qual_grp.Count >= qual - 1 )
						{
							return (qual_grp[qual - 1]["awdper"]);	
						}	
					}	
				}
			}
			return 0;
		}

        public Variant GetAppawdRmis(int misid)
        {
            return _appawdRmis != null ? _appawdRmis[misid.ToString()] : null;
        }

        public void onRmisInfoRes(GameEvent e)
        {
            Variant data = e.data;
            if(data == null) return;

            int id = 0;
            if (data.ContainsKey("id"))
            {
                id = data["id"];
            }
            Variant playerRmis;
            //string text1;
            //string text2;
            bool isRfin = false;
            Variant rmisObj = _rmisConfData[id.ToString()];
            Variant rmisFin = (this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.rmis_fin;
            Variant rmisFinObj;
            switch (data["tp"]._int)
            {
                case 1:
                    {
                        if (_reSetPlaymis)
                        {
                            _reSetPlaymis = false;
                            _playerRmis = new Variant();
                        }
                        playerRmis = _playerRmis[id.ToString()];
                        _playerRmis[id.ToString()] = data;
                        if (playerRmis == null)
                        {//刷新任务面板
                            missionChange(0);
                        }
                        else
                        {
                            switch (rmisObj["type"]._int)
                            {
                                case 1:
                                    //lgiuiMission.PlayerRmisChangedCarry( id );
                                    break;
                                case 2:
                                    //lgiuiMission.PlayerRmisChanged(id);
                                    break;
                                default:
                                    return;
                            }
                        }
                        //this.lgInGame.lgGD_miss.player_rmis_change(id);
                        //_lgInGame.lgClient.uiClient.AddDailyData("lgGDRMissions",InitPlayerRmisData);

                    } break;
                case 2:
                    {
                        playerRmis = _playerRmis[id.ToString()];
                        if (playerRmis != null)
                        {
                            foreach (string s in data.Keys)
                            {
                                if (s != "rqual")
                                {	//品质状态，需要单独更新
                                    playerRmis[s] = data[s];
                                }
                            }
                            //单独更新品质信息
                            foreach (Variant obj in playerRmis["rqual"].Values)
                            {
                                Variant rqual = data["rqual"];
                                if (obj["id"] == rqual["id"])
                                {
                                    foreach (string rs in rqual.Keys)
                                    {
                                        obj[rs] = rqual[rs];
                                    }
                                }
                            }
                            switch (rmisObj["type"]._int)
                            {
                                case 1:
                                    //lgiuiMission.PlayerRmisChangedCarry( id );
                                    break;
                                case 2:
                                    //lgiuiMission.PlayerRmisChanged(id);
                                    break;
                                default:
                                    return;
                            }
                            //						this._main.UIMgr.MessageBox.Show(this._main.attributes.translate_text(text2), 2);
                        }
                    } break;
                case 3:
                    {
                        //					resetMisSkillCD(data);
                    } break;
                case 4:
                    {
                        //					callHelp( data );			
                    } break;
                case 5:
                    {
                        //					text1 = this._main._gamescene.lanuage_pack.get_string( "UI_Class_npc_carry", "buy_insure_success" );
                        //					this._main.UIMgr.MessageBox.Show( text1 );					
                    } break;
                case 6:
                    {
                        //					if( data.hasOwnProperty( "misskil_cds" ) )
                        //					{
                        //						for each( var obj:Object in data.misskil_cds )
                        //						resetMisSkillCD( obj );
                        //					}
                        //					if(data.hasOwnProperty( "insure_misids" ))
                        //					{
                        //						this._main.UIMgr.skills_carry.set_safty_state(data.insure_misids);
                        //					}
                        //					if( data.hasOwnProperty( "free_misinsure" ) )
                        //					{
                        //						this._freeInsure = data.free_misinsure as int;
                        //					}
                    } break;
                case 7:
                    {
                        //					resetMisSkillCD( data );
                    } break;
                case 8:
                    {
                        if (data.ContainsKey("mis_append") && data["mis_append"].Count > 0)
                        {
                            foreach (Variant rmisData in data["mis_append"]._arr)
                            {
                                int misid = rmisData["misid"];
                                if (null == _appawdRmis)
                                {
                                    _appawdRmis = new Variant();
                                }
                                _appawdRmis[misid.ToString()] = rmisData;
                                //_lgInGame.lgGD_miss.UpdateRmis(misid);
                            }

                        }
                    } break;
                case 9:
                    {	//一键完成剩余次数的酒馆任务  成功返回

                        if (null == _playerRmis[id.ToString()])
                        {
                            _playerRmis[id.ToString()] = new Variant();
                        }
                        playerRmis = _playerRmis[id.ToString()];
                        foreach (string s in data.Keys)
                        {
                            playerRmis[s] = data[s];
                        }
                        //lgInGame.lgGD_miss.MisDataChange( "rmis", {id:id, fincnt:data.fincnt});
                        switch (rmisObj["type"]._int)
                        {
                            case 1:
                                //lgiuiMission.PlayerRmisChangedCarry( id );
                                break;
                            case 2:
                                //lgiuiMission.setMissions();
                                break;
                            default:
                                return;
                        }
                    } break;
                case 10:
                    {	//
                        if (null == _playerRmis[id.ToString()])
                        {
                            _playerRmis[id.ToString()] = new Variant();
                        }
                        playerRmis = _playerRmis[id.ToString()];
                        foreach (Variant rfinObj in rmisFin.Values)
                        {
                            if (rfinObj["rid"] == id)
                            {
                                rfinObj["fcnt"]++;
                                isRfin = true;
                                break;
                            }
                        }
                        if (!isRfin && rmisFin)
                        {
                            rmisFinObj = new Variant();
                            rmisFinObj["rid"] = id;
                            rmisFinObj["fcnt"] = 1;
                            rmisFin.pushBack(rmisFinObj);
                        }
                        playerRmis["dalyawd"] = data["dalyawd"];
                        switch (rmisObj["type"]._int)
                        {
                            case 1:
                                //lgiuiMission.PlayerRmisChangedCarry( id );
                                break;
                            case 2:
                                //lgiuiMission.setMissions();
                                break;
                            default:
                                return;
                        }
                    } break;
                case 11:
                    {
                        if (_reSetShare)
                        {
                            _reSetShare = false;
                            _rmis_share = new Variant();
                        }
                        foreach (Variant rsfinObj in rmisFin._arr)
                        {
                            if (rsfinObj != null && rsfinObj["rid"] == id)
                            {
                                rsfinObj["fcnt"]++;
                                isRfin = true;
                                break;
                            }
                        }
                        if (!isRfin && rmisFin != null)
                        {
                            rmisFinObj = new Variant();
                            rmisFinObj["rid"] = id;
                            rmisFinObj["fcnt"] = 1;
                            rmisFin.pushBack(rmisFinObj);
                        }
                        Variant arr = data["rmis_share"];
                        if (arr.Count > 0)
                        {
                            foreach (Variant obj1 in arr._arr)
                            {
                                _rmis_share[obj1["id"]] = obj1;
                                foreach (Variant rmisConf in _rmisConfData.Values)
                                {
                                    if (rmisConf.ContainsKey("rmis_share") && rmisConf["rmis_share"] == obj1["id"])
                                    {
                                        switch (rmisConf["type"]._int)
                                        {
                                            case 1:
                                                //lgiuiMission.PlayerRmisChangedCarry( obj1.id );
                                                break;
                                            case 2:
                                                //lgiuiMission.setMissions();
                                                break;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    } break;
            }
        }
        //------------------------------酒馆任务 end--------------------------------------------

        //------------------------------任务目标gmis start--------------------------------------------
        private Variant _fingmis;
		private Variant _finvips;
        private Variant _killmons = new Variant();

        public void onGmisInfoRes(GameEvent e)
		{
            Variant data = e.data;
			switch(data["tp"]._int)
			{
				case 1:
				{//获取基本信息
					_fingmis = data["fin_gmis"];
					_finvips = data["fin_vip"];
					if(data.ContainsKey("killmons") && data["killmons"] != null)
					{
						_killmons = data["killmons"];
					}
					break;
				}
				case 2:
				{//击杀怪物
					bool hasGmis = false;
					foreach(Variant kmObj in _killmons._arr)
					{
						if(kmObj["gid"] == data["gid"])
						{
							hasGmis = true;
							foreach(Variant monObj in data["km"]._arr)
							{
								bool hadkill = false;
								foreach(Variant obj in kmObj["km"]._arr)
								{
									if(obj["monid"] == monObj["monid"])
									{
										hadkill = true;
										obj["cnt"] += monObj["cnt"]._int;
										break;
									}
								}
								if(!hadkill)
								{
									kmObj["km"].pushBack(monObj);
								}
							}
							break;
						}
					}
					
					if(!hasGmis)
					{
                        Variant gdata = new Variant();
                        gdata["gid"] = data["gid"];
                        gdata["km"] = data["km"];
                        _killmons.pushBack(gdata);
					}
					break;
				}
			}
			
			//ui_weekGoal.RefreshGmisData(data);
		}

        public void onGmisAwdRes(GameEvent e)
		{
            Variant data = e.data;
			if(data.ContainsKey("gmisid"))
			{
				_fingmis.pushBack(data["gmisid"]);
			}
			if(data.ContainsKey("vip"))
			{
                _finvips.pushBack(data["vip"]);
			}
			//ui_weekGoal.RefreshGmisAwd(data);
		}




        //------------------------------任务目标gmis end--------------------------------------------
        
    }
}