using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class LevelProxy : BaseProxy<LevelProxy>
    {
        public List<Rewards> reward = new List<Rewards>();
        public List<Rewards> fbDrogward = new List<Rewards>();
        public bool is_open;
        public bool open_pic;
        public bool open_pic1;
        public string[] codes;
        public string[] codess;
        public string icon1;
       // public List<Rewards> prize = new List<Rewards>();
        public LevelProxy()
            : base()
        {
            //addProxyListener(PKG_NAME.S2C_GET_LVLMIS_RES, get_lvlmis_res);
            //addProxyListener(PKG_NAME.S2C_CLANTER_RES, clanter_res);
            addProxyListener(PKG_NAME.S2C_LVL_RES, lvl_res);
            //addProxyListener(PKG_NAME.S2C_CARRCHIEF, onNPCShop);
            addProxyListener(PKG_NAME.S2C_LVL_BROADCAST, lvl_broadcast);
            addProxyListener(PKG_NAME.S2C_ON_ARENA, on_arena);
            addProxyListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, lvl_pvpinfo_board_res);
            addProxyListener(PKG_NAME.S2C_MOD_LVL_SELFPVPINFO, mod_lvl_selfpvpinfo);
            addProxyListener(PKG_NAME.S2C_LVL_ERR_MSG, lvl_err_msg);
            addProxyListener(PKG_NAME.S2C_CHECK_IN_LVL_RES, check_in_lvl_res);
            addProxyListener(PKG_NAME.S2C_CREATE_LVL_RES, create_lvl_res);
            addProxyListener(PKG_NAME.S2C_ENTER_LVL_RES, enter_lvl_res);
            addProxyListener(PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES, get_associate_lvls_res);
            addProxyListener(PKG_NAME.S2C_LVL_FIN, lvl_fin);

            addProxyListener(PKG_NAME.S2C_LVL_SIDE_INFO, lvl_side_info);
            addProxyListener(PKG_NAME.S2C_CLOSE_LVL_RES, close_lvl_res);
            addProxyListener(PKG_NAME.S2C_LVL_KM, lvl_km);
            addProxyListener(PKG_NAME.S2C_LEAVE_LVL_RES, leave_lvl_res);
            addProxyListener(PKG_NAME.S2C_ON_BATTLE_DO_RES, on_battle_do_res);


            addProxyListener(PKG_NAME.S2C_GET_LVL_INFO_RES, get_lvl_info_res);
            addProxyListener(PKG_NAME.S2C_LVL_GET_PRIZE_RES, lvl_get_prize_res);


            addProxyListener(PKG_NAME.C2S_LVL_ZHSLY, lvl_zhsly_info);


        }

        public void getAwd_zhs(uint tp) {
            Variant data = new Variant();
            data["op"] = tp;
            sendRPC(PKG_NAME.C2S_LVL_ZHSLY, data);
        }

        public void sendBuyEnergy()
        {

            sendRPC(PKG_NAME.C2S_BUY_ENERGY);
        }


        public void sendGet_clanter_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CLANTER_RES, data);
        }

        public void sendGet_lvl_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_RES, data);
        }

        //public void sendGet_npcshop_info(Variant data)
        //{
        //    sendRPC(PKG_NAME.C2S_CARRCHIEF, data);
        //}

        public void sendGet_arena_info(Variant data)
        {
            sendRPC(PKG_NAME.C2S_ON_ARENA, data);
        }

        public void sendGet_lvl_pvpinfo_board(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_ERR_MSG, data);
        }
        public void sendCheck_in_lvl(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CHECK_IN_LVL_RES, data);
        }
        public void sendCreate_lvl(Variant data)
        {
            sendRPC(PKG_NAME.C2S_CREATE_LVL_RES, data);
        }

        public void sendEnter_lvl(Variant data)
        {
            debug.Log("!!sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data)3!! " + debug.count);
            sendRPC(PKG_NAME.C2S_ENTER_LVL_RES, data);
        }

        public void sendGet_associate_lvls(Variant data)
        {
            sendRPC(PKG_NAME.C2S_GET_ASSOCIATE_LVLS_RES, data);
        }

        //  <!--副本相关操作的操作码-->
        //<!--1为请求玩家的已通关副本信息-->
        //<!--2为扫荡副本，带参数，表示扫荡哪个副本(param1) 多少次(param2)-->
        //<!--3为重置副本，带参数，表示重置哪个副本(param1)-->
        public void sendGet_lvl_cnt_info(int type, int param1 = 0, int param2 = 0)
        {
            Variant data = new Variant();

            data["operation"] = type;

            if (param1 > 0)
                data["param1"] = param1;
            if (param2 > 0)
                data["param2"] = param2;
            sendRPC(PKG_NAME.C2S_GET_LVL_INFO_RES, data);
        }

        public void buyAnduseExp() {
            Variant data = new Variant();
            data["operation"] = 6;
            sendRPC(PKG_NAME.C2S_GET_LVL_INFO_RES, data);
        }

        public void sendGet_lvl_prize(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_FIN, data);
        }
        //		消息功能：离开副本，在副本中调用该函数可离开副本回到大世界，副本依然存在（组队副本不离开队伍）；
        //		cmd_id：246
        //		参数：{}
        public void sendLeave_lvl()
        {
            //sendRPC(PKG_NAME.C2S_LVL_GET_PRIZE_RES, new Variant());
            sendRPC(PKG_NAME.C2S_LEAVE_LVL_RES, new Variant());
        }

        public void sendClose_lvl(Variant data)
        {
            sendRPC(PKG_NAME.C2S_LVL_SIDE_INFO, data);
        }

        //获取侠客行任务相关信息
        public void sendGetLvlmisInfo()
        {
            sendRPC(PKG_NAME.C2S_GET_LVLMIS_RES, new Variant());
        }

        void lvl_zhsly_info(Variant msgData)
        {
            //Debug.LogError("FFFF"+ msgData.dump());
            int res = msgData["res"];
            if (res < 0)
            {
                //Globle.err_output(res);
                if (res == -466) {
                    a3_insideui_fb.instance?.showTs_zhs();
                }
                return;
            }
            switch (res)
            {
                case 1://获取信息初始化
                    a3_insideui_fb.instance?.ZHSLY_info(msgData["kill_count"], msgData["fin_count"]);
                    cishu = msgData["fin_count"];
                    count = msgData["kill_count"];
                    vip_buycount = msgData["vip_cnt"];
                    break;
                case 2://领取奖励
                    a3_insideui_fb.instance?.ZHSLY_info(msgData["kill_count"], msgData["fin_count"]);
                    cishu = msgData["fin_count"];
                    count = msgData["kill_count"];
                    vip_buycount = msgData["vip_cnt"];
                    break;
                case 3://同步刷怪数
                    a3_insideui_fb.instance?.ZHSLY_info(msgData["kill_count"],cishu);
                    count = msgData["kill_count"];
                    break;
                case 4://vip购买次数
                    vip_buycount = msgData["vip_cnt"];
                    a3_active_zhsly.instance.setCount();
                    break;
            }

        }

        //zhs
        public int cishu = 0;
        public int count = 0;
        public int vip_buycount = 0;


        private void get_lvlmis_res(Variant msgData)
        {
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_lvlmis_res(msgData);	
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_GET_LVLMIS_RES, this, GameTools.CreateSwitchData("get_lvlmis_res", msgData))
             );
        }

        private void clanter_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_CLANTER_RES, this, GameTools.CreateSwitchData("on_clanter_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_clanter_res(msgData);		
        }

        private void lvl_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LVL_RES, this, GameTools.CreateSwitchData("on_lvl_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_res(msgData);		
        }

        private void onNPCShop(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_CARRCHIEF, this, GameTools.CreateSwitchData("on_npcshop_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_carrchief_res(msgData);		
        }

        private void on_arena(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_ON_ARENA, this, GameTools.CreateSwitchData("on_arena_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_Arena.on_arena_res(msgData);		
        }

        private void lvl_broadcast(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LVL_BROADCAST, this, GameTools.CreateSwitchData("on_lvl_broadcast_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_broadcast_res(msgData);		
        }

        private void lvl_pvpinfo_board_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, this, GameTools.CreateSwitchData("lvl_pvpinfo_board_msg", msgData))
             );
            TeamProxy.getInstance().dispatchEvent(GameEvent.Create(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES,this,msgData));
        }

        private void mod_lvl_selfpvpinfo(Variant msgData)
        {

            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_MOD_LVL_SELFPVPINFO, this, GameTools.CreateSwitchData("mod_lvl_selfpvpinfo", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.mod_lvl_selfpvpinfo(msgData);		
        }

        private void lvl_err_msg(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LVL_ERR_MSG, this, GameTools.CreateSwitchData("on_lvl_err_msg", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_err_msg(msgData);		
            //通知ai
            //GameSession(this.session).logicClient.logicInGame.AIPly.onLvlErr(msgData.res);
        }
        private void check_in_lvl_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_CHECK_IN_LVL_RES, this, GameTools.CreateSwitchData("on_check_in_lvl_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_check_in_lvl_res(msgData);
        }
        private void create_lvl_res(Variant msgData)
        {
            debug.Log("Level Create ===============================");
            debug.Log(msgData.dump());
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_create_lvl_res(msgData);
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_CREATE_LVL_RES, this, GameTools.CreateSwitchData("on_create_lvl_res", msgData))
         );
        }
        private void enter_lvl_res(Variant msgData)
        {
            debug.Log("Level Enter ===============================");
            debug.Log(msgData.dump());
            uint ltpid = msgData["ltpid"];
            Variant d = SvrLevelConfig.instacne.get_level_data(ltpid);
            if (d.ContainsKey("public") && d["public"] == 1)
            {
                is_open = true;               
            }
            else
                is_open = false;
            if (d.ContainsKey("shengwu"))
            {
                if (d["shengwu"]==1)
                {
                    open_pic = true;
                    string icon = d["icon"];
                    codes = icon.Split(',');
                    icon1 = d["des"];
                    codess = icon1.Split(',');
                }
                else
                {
                    open_pic1 = true;
                    string icon = d["icon"];
                    codes = icon.Split(',');
                    icon1 = d["des"];
                    codess = icon1.Split(',');
                }
            }

            if (ltpid == 107 || ltpid == 9000)//竞技场 据点战场
            {
                starTime = msgData["start_tm"]._int64;
                // Debug.LogError("UUUU"+ (wait -( muNetCleint.instance.CurServerTimeStamp-msgData["start_tm"]._int64 )));
            }

            if (ltpid == 8000)//城战
            {
                A3_cityOfWarModel .getInstance ().starTime = msgData["start_tm"]._int64;
                A3_cityOfWarModel .getInstance ().endTime = msgData["end_tm"]._int64;
            }

            a3_counterpart.lvl = msgData["diff_lvl"];
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_enter_lvl_res(msgData);
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_ENTER_LVL_RES, this, GameTools.CreateSwitchData("on_enter_lvl_res", msgData))
             );
        }

        public long starTime = 0;

        public long starTime_jdzc = 0;

        private void get_associate_lvls_res(Variant msgData)
        {
            int res = msgData["res"];
            switch (res)
            {
                case 3:
                    int camid = msgData["trig_id"];
                    //检测是否有摄像机动画
                    SceneCamera.CheckTrrigerCam(camid);
                    //检测是否有剧情对白
                    if (a3_trrigerDialog.instance != null)
                        a3_trrigerDialog.instance.CheckDialog(camid);
                    break;
            }


            if (GameRoomMgr.getInstance().onLevelStatusChanges(msgData))
                return;
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_GET_ASSOCIATE_LVLS_RES, this, GameTools.CreateSwitchData("get_associate_lvls_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_associate_lvls_res(msgData);
        }
        //

        private void get_lvl_info_res(Variant msgData)
        {
            int levelId; MapData mapdata;
            int res = msgData["res"];

            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            debug.Log("KKKKKKKK"+ msgData.dump ());
           // debug.Log("<color=#00ff00>"+ msgData.dump() +"</color>");
            MapModel m = MapModel.getInstance();
            MapData d;
            switch (res)
            {
                case 1://表示请求副本信息的结果
                       //int time = -1;

                    //if (msgData.ContainsKey("refresh_time_left"))
                    //   time = msgData["refresh_time_left"];

                    int maxid0 = 0;
                    int maxid1 = 0;
                    //m.beginTimer = time + Time.time;
                    if (msgData.ContainsKey("lvls"))
                    {
                        List<Variant> lv = msgData["lvls"]._arr;

                        foreach (Variant v in lv)
                        {
                            d = m.getMapDta(v["lvlid"]);
                            if (d == null)
                            {
                                d = new MapData();
                            }

                            d.starNum = v["score"];
                            
                            if (v.ContainsKey("last_enter_time")) d.enterTime = v["last_enter_time"];
                            if (v.ContainsKey("enter_times")) d.cycleCount = v["enter_times"];
                            if (v.ContainsKey("limit_tm")) d.limit_tm = v["limit_tm"];
                            if (v.ContainsKey("vip_cnt")) d.vip_buycount = v["vip_cnt"];
                            m.AddMapDta(v["lvlid"], d);
                            d = m.getMapDta(v["lvlid"]);
                            if (d.type == 1)
                            {
                                if (d.starNum > 0 && d.id > maxid0)
                                {
                                    maxid0 = d.id;
                                }
                            }
                            else if (d.type == 2)
                            {
                                if (d.starNum > 0 && d.id > maxid1)
                                {
                                    maxid1 = d.id;
                                }
                            }
                            //if (v["lvlid"] == 104)
                            //{
                            //    if (v.ContainsKey ("diff_lvl"))
                            //    {
                            //        A3_ActiveModel.getInstance().nowlvl = v["diff_lvl"];
                            //    }
                            //}

                        }

                        m.setLastMapId(0, maxid0);
                        m.setLastMapId(1, maxid1);

                        m.inited = true;

                    }

                    if (msgData.ContainsKey ("mlzd_info"))
                    {
                        Variant mlzd = msgData["mlzd_info"];
                        if (mlzd.ContainsKey("mlzd_diff"))
                            A3_ActiveModel.getInstance().maxlvl = mlzd["mlzd_diff"];

                        if (mlzd.ContainsKey("diff_floor")) {
                            A3_ActiveModel.getInstance().nowlvl = mlzd["diff_floor"];
                        }
                        if (mlzd.ContainsKey("times"))
                            A3_ActiveModel.getInstance().count_mlzd = mlzd["times"];
                        if (mlzd.ContainsKey("sweep_type"))
                        {
                            A3_ActiveModel.getInstance().sweep_type = mlzd["sweep_type"];
                        }

                        if (mlzd.ContainsKey("tm"))
                            A3_ActiveModel.getInstance().Time = mlzd["tm"];
                        if (a3_active_mlzd.instans != null)
                        {
                            a3_active_mlzd.instans.RefreshLeftTimes();
                        }
                    }

                    MapModel mapM = MapModel.getInstance();
                    //凌晨界面打开时数据要刷新（单/组队人副本）
                    if (a3_counterpart._instance)
                    {
                        a3_counterpart._instance.refreshGoldTimes();
                        a3_counterpart._instance.refreshExpTimes();
                        a3_counterpart._instance.refreshMateTimes();
                    }
                    break;
                case 2:// <!--扫荡了多少次，客户端按此值对rewards数组进行分组-->

                    levelId = msgData["lvlid"];
                    mapdata = MapModel.getInstance().getMapDta(levelId);
                    mapdata.count = msgData["left_times"];
                    List<Variant> rewards = msgData["rewards"]._arr;

                    int len = rewards.Count / 3;
                    int idx = 0;
                    List<List<MapItemData>> ll = new List<List<MapItemData>>();
                    List<MapItemData> l;
                    MapItemData mapitemdata;
                    for (int i = 0; i < len; i++)
                    {
                        l = new List<MapItemData>();
                        for (int j = 0; j < 3; j++)
                        {
                            mapitemdata = new MapItemData();
                            mapitemdata.id = rewards[idx]["tpid"];
                            mapitemdata.count = rewards[idx]["cnt"];
                            l.Add(mapitemdata);
                            idx++;
                        }
                        ll.Add(l);
                    }

                    //fb_wipeout.showIt(ll, mapdata);
                    break;
                case 3:// <!--3表示重置副本-->
                    levelId = msgData["lvlid"];
                    mapdata = MapModel.getInstance().getMapDta(levelId);
                    mapdata.count = msgData["left_times"];
                    mapdata.resetCount = msgData["left_reset_times"];

                    //if (fb_info.instance != null)
                    //    fb_info.instance.onRefresh(levelId);
                    break;
                case 4:// <!--4表示有新的最高分产生，同步给客户端-->
                    int id = msgData["lvlid"];
                    if (id == 104)
                    {
                        if (msgData.ContainsKey("mlzd_diff"))
                            A3_ActiveModel.getInstance().maxlvl = msgData["mlzd_diff"];

                        if (msgData.ContainsKey("diff_floor")) {
                            A3_ActiveModel.getInstance().nowlvl = msgData["diff_floor"];
                        }
                    }
                    if (m.containerID(id))
                        break;

                    d = m.getMapDta(id);
                    if (d == null) return;
                    d.starNum = msgData["score"];
                    d.count = msgData["left_times"];
                    d.resetCount = msgData["left_reset_times"];



                    break;
                    //vip购买增加次数返回
                case 5:

                    MapData data = m.getMapDta(msgData["ltpid"]);
                    if (data == null)
                    {
                        data = new MapData();
                        data.vip_buycount = msgData["vip_cnt"];
                        MapModel.getInstance().AddMapDta(msgData["ltpid"],data);
                    }
                    else
                    {
                        MapModel.getInstance().dFbDta[msgData["ltpid"]].vip_buycount = msgData["vip_cnt"];
                    }

                    if (a3_counterpart._instance)
                    {
                        a3_counterpart._instance.refreshGoldTimes();
                        a3_counterpart._instance.refreshExpTimes();
                        a3_counterpart._instance.refreshMateTimes();
                    }
                    



                    //MJJDModel.getInstance().GetBuyRefresh(msgData);
                    break;
                case 6:
                    //MJJDModel.getInstance().GetMJJDTimes(msgData);
                    break;
                case 7://刷新墨家境地波数
                    //if (float_mjjd.instance != null && msgData.ContainsKey("current_wave"))
                    //    float_mjjd.instance.refreshBo(msgData["current_wave"]);
                    break;
                default:
                    break;

            }


            //NetClient.instance.dispatchEvent(
            //     GameEvent.Create(PKG_NAME.S2C_GET_LVL_INFO_RES, this, GameTools.CreateSwitchData("get_lvl_info_res", msgData))
            // );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.get_lvl_info_res(msgData);
        }


        private void lvl_fin(Variant msgData)
        {
            if (GameRoomMgr.getInstance().curRoom != null)
                GameRoomMgr.getInstance().curRoom.clear();

            //if (BaseRoomItem.instance != null ) {
            //    BaseRoomItem.instance.clearlist();
            //}
            reward.Clear();
            debug.Log("Fin Level ==============================="+ msgData.dump());

            if (msgData != null)
            {
                if (msgData["rewards"] != null)
                {
                    List<Variant> l = msgData["rewards"]._arr;
                    foreach (var v in l)
                    {
                        Rewards d = new Rewards();
                        d.tpid = v["tpid"];
                        d.cnt = v["cnt"];

                        reward.Add(d);
                    }
                }
                if (msgData.ContainsKey ("item_drop")) {
                    List<Variant> l = msgData["item_drop"]._arr;
                    foreach (var v in l)
                    {
                        Rewards d = new Rewards();
                        d.tpid = v["tpid"];
                        d.cnt = v["cnt"];
                        fbDrogward.Add(d);
                    }
                }

                //if (msgData.ContainsKey("kill_exp"))
                //{
                //    a3_fb_finish.allEXP = msgData["kill_exp"];//直接显示服务器发的数据
                //}
            }
            if (GameRoomMgr.getInstance().onLevelFinish(msgData))
                return;

            if (msgData.ContainsKey("ply_res"))
            {

                //int starNum = msgData["ply_res"]._arr[0]["score"];
                //if (starNum == 0)
                //    InterfaceMgr.getInstance().open(InterfaceMgr.FB_LOSE);
                //else
                //fb_win.star = starNum;

            }
            else
            {

                LevelProxy.getInstance().sendLeave_lvl();
            }
            //NetClient.instance.dispatchEvent(
            //     GameEvent.Create(PKG_NAME.S2C_LVL_FIN, this, GameTools.CreateSwitchData("on_lvl_fin", msgData))
            // );
            // GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_fin(msgData);
        }
        private void lvl_get_prize_res(Variant msgData)
        {
            //prize.Clear();
            bool b = GameRoomMgr.getInstance().onPrizeFinish(msgData);

            if (b)
                return;
           // Debug.LogError("KKKKK"+ msgData.dump());
            PVPRoom.instan?.refGet(msgData["ach_point"], msgData["exp"]);
            List<MapItemData> ld = new List<MapItemData>();
            if (msgData.ContainsKey("rewards"))
            {
                List<Variant> lv = msgData["rewards"]._arr;
                MapItemData d;

                foreach (Variant v in lv)
                {
                    d = new MapItemData();
                    d.count = v["cnt"];
                    d.id = v["tpid"];
                    ld.Add(d);

                    //Rewards l = new Rewards();
                    //l.tpid = v["tpid"];
                    //l.cnt = v["cnt"];
                    //prize.Add(l);
                    //Debug.LogError(l.cnt);
                }
            }

            //if (PVPRoom.instan != null)
            //{

            // }

            //fb_win.lItemData = ld;
            // InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FB_WIN);
            // GameCameraMgr.getInstance().useCamera("fb_win");
        }

        private void lvl_side_info(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LVL_SIDE_INFO, this, GameTools.CreateSwitchData("on_lvl_side_info", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_side_info(msgData);
        }

        private void close_lvl_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_CLOSE_LVL_RES, this, GameTools.CreateSwitchData("on_close_lvl_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_close_lvl_res(msgData);
        }
        private void lvl_km(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //     GameEvent.Create(PKG_NAME.S2C_LVL_KM, this, GameTools.CreateSwitchData("on_lvl_km", msgData))
            // );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_lvl_km(msgData);
        }
        private void leave_lvl_res(Variant msgData)
        {
            MapModel.getInstance().curLevelId = 0;
            InterfaceMgr.doCommandByLua("MapModel:getInstance().getcurLevelId", "model/MapModel", 0);
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_LEAVE_LVL_RES, this, GameTools.CreateSwitchData("on_leave_lvl", msgData))
             );
        }


        private void on_battle_do_res(Variant msgData)
        {
            NetClient.instance.dispatchEvent(
                 GameEvent.Create(PKG_NAME.S2C_ON_BATTLE_DO_RES, this, GameTools.CreateSwitchData("on_battle_do_res", msgData))
             );
            //GameSession(this.session).logicClient.logicInGame.lgGD_levels.on_battle_do_res(msgData);
        }
    }



}
