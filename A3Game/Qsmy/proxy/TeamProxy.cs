using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class TeamProxy : BaseProxy<TeamProxy>
    {
        public static uint EVENT_CREATETEAM = 1201;//创建队伍
        public static uint EVENT_DISSOLVETEAM = 1202;//解散队伍
        public static uint EVENT_TEAMLISTINFO = 1203;//获得队伍列表
        public static uint EVENT_AFFIRMINVITE = 1205;//
        public static uint EVENT_NEWMEMBERJOIN = 1206;//新队员加入
        public static uint EVENT_KICKOUT = 1207;//踢出成员
        public static uint EVENT_CHANGETEAMINFO = 1208;//队伍信息变更
        public static uint EVENT_NOTICEHAVEMEMBERLEAVE = 1209;//通知有成员离开队伍
        public static uint EVENT_LEAVETEAM = 1210;//离开队伍,不管主动还是被T
        public static uint EVENT_CHANGECAPTAIN = 1211;//更换队长
        public static uint EVENT_SYNCTEAMBLOOD = 1212;//同步队伍血量
        public static uint EVENT_NOTICEONLINESTATECHANGE = 1213;//在线状态变更
        public static uint EVENT_NOTICEINVITE = 1214;//队伍邀请提示
        public static uint EVENT_TEAMOBJECT_CHANGE = 1215;//队长改变队伍目的；
        public static uint EVENT_TEAM_READY = 1216;//队员准备
        public static uint EVENT_CURPAGE_TEAM = 1217;//页签队伍信息

        public bool haveSentLeaveMsg = false;
        public bool joinedTeam = false;
        public static uint wantedWatchTeamId;
        public static uint WatchTeamId_limited;
        public ItemTeamMemberData mapItemTeamData;

        public ItemTeamMemberData pageItemTeamData;
        public ItemTeamMemberData MyTeamData;

        public readonly static uint BEGININDEX = 0, ENDINDEX = 20;
        Dictionary<uint, float> InvitedDic = new Dictionary<uint, float>();//邀请加入的队员列表
        Dictionary<uint, float> ApplyDic = new Dictionary<uint, float>();//申请加入的队伍列表
        public bool now_Team = false;

        public List<TeamPosition> teamlist_position = new List<TeamPosition>();
        public List<ItemTeamData> teamMemberposData = new List<ItemTeamData>();
        //public  List<ItemTeamData> teamPosMapid = new List<ItemTeamData>();
        // public List<ItemTeamData> teamPosMapid1 = new List<ItemTeamData>();
        public uint trage_cid;

        public TeamProxy()
        {
            InvitedDic.Clear();
            addProxyListener(PKG_NAME.S2C_CREATE_TEAM_RES, OnTeamInfo);
        }
        public void OnTeamInfo(Variant data)
        {           
            int res = data["res"];
            Debug.Log("队伍信息" + data.dump());
            if (res < 0)
            {
                if (res == -1309)
                {
                    SendCreateTeam(0);
                    now_Team = true;
                    //TeamProxy.getInstance().SendInvite(trage_cid);
                    //now_Team = false;
                }
                else
                    Globle.err_output(res);
                return;
            }
            switch ((TeamCmd)res)
            {

                case TeamCmd.CreateTeam:
                    SetCreateTeam(data);
                    break;
                case TeamCmd.GetMapTeam:
                    SetMapTeam(data);
                    break;
                case TeamCmd.WatchTeamInfo:
                    SetWatchTeamInfo(data);
                    break;
                case TeamCmd.SyncTeamBlood://同步队伍血量
                    SetSyncTeamBlood(data);
                    break;
                case TeamCmd.CurrentMapTeamPos://队员坐标；
                    GetTeamPos(data);
                    break;
                case TeamCmd.ApplyJoinTeam:
                    SetApplyJoinTeam(data);
                    break;
                case TeamCmd.AffirmApply:
                    break;
                case TeamCmd.LeaveTeam://离开队伍 都是case 8 不管主动离开还是被T
                    SetLeaveTeam(data);
                    break;
                case TeamCmd.Invite:
                    break;
                case TeamCmd.AffirmInvite://10确认邀请
                    SetAffirmInvite(data);
                    break;
                case TeamCmd.KickOut://踢出
                    SetKickOut(data);
                    break;
                case TeamCmd.ChangeCaptain://更换队长
                    SetChangeCaptain(data);
                    break;
                case TeamCmd.Dissolve:
                    SetDissolve(data);
                    break;
                case TeamCmd.EditorInfo://修改队伍信息
                    SetChangeTeamInfo(data);
                    break;
                case TeamCmd.ChangeObject://15队长队伍目的信息变更时，服务器发来数据
                    SetTeamobject_Change(data);
                    break;
                case TeamCmd.Ready://16队员准备
                    SetTeamReady(data);
                    break;

                case TeamCmd.Get_curPageTeam:

                    Get_curPageTeam_info(data);
                    break;
                case TeamCmd.NoticeCaptainNewInfo://20 通知队长有新的申请
                    SetNoticeCaptainNewInfo(data);
                    break;
                case TeamCmd.NoticeApplyBeRefuse://21 申请加入队伍被拒绝
                    break;
                case TeamCmd.NoticeHaveMemberLeave://22通知有成员离开队伍
                    SetNoticeHaveMemberLeave(data);
                    break;
                case TeamCmd.NoticeInvite://23通知邀请
                    SetNoticeInvite(data);
                    break;
                case TeamCmd.NewMemberJoin://24新队员入队
                    SetNewMemberJoin(data);
                    MonsterMgr._inst.RefreshVaildMonster();
                    break;
                case TeamCmd.NoticeInviteBeRefuse://25通知邀请被拒绝
                    SetNoticeInviteBeRefuse(data);
                    break;
                case TeamCmd.NoticeOnlineStateChanged://26在线状态变更
                    SetNoticeOnlineStateChange(data);
                    break;
                //case TeamCmd.NoticeCombptOrLvChange:
                //    SetNoticeLvOrCombptChange(data);
                //    break;
                case TeamCmd.DropItemRoll:

                    A3_RollModel.getInstance().SetRollDropItem(data["roll_dpitms"]._arr);
                    if(a3_RollItem.single == null) InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ROLL_ITEM);

                    break;

                case TeamCmd.Roll:

                    A3_RollModel.getInstance().SetRollResult(data);
                    dispatchEvent(GameEvent.Createimmedi((uint)TeamCmd.Roll, this, data));

                    break;

                default:

                    break;

            }
        }

        private void Get_curPageTeam_info(Variant data)
        {
            pageItemTeamData = new ItemTeamMemberData();
            List<ItemTeamData> pageteamListInfoDataList = new List<ItemTeamData>();
            pageteamListInfoDataList.Clear();
            uint totalCount = data["total_cnt"];
            uint idx_begin = data["idx_begin"];
            if (a3_SpeedTeam.instance != null)
            {
                a3_SpeedTeam.instance.allnum = (int)totalCount;
                a3_SpeedTeam.instance.begion_idx = (int)idx_begin;
            }
            pageItemTeamData.totalCount = totalCount;
            pageItemTeamData.idxBegin = idx_begin;
            List<Variant> info = data["info"]._arr;            
            foreach (Variant item in info)
            {
                ItemTeamData tlid = new ItemTeamData();
                tlid.teamId = item["tid"];
                tlid.curcnt = item["curcnt"];
                tlid.maxcnt = item["maxcnt"];
                tlid.name = item["lname"];
                tlid.carr = item["lcarr"];
                tlid.lvl = item["llevel"];
                tlid.zhuan = item["lzhuan"];
                tlid.mapId = item["lmapid"];
                tlid.ltpid = item["ltpid"];
                tlid.ldiff = item["ldiff"];
                tlid.members = item["members"]._arr;
                if (string.IsNullOrEmpty(item["lclname"]))
                {
                    tlid.knightage =ContMgr.getCont("FriendProxy_wu");
                }
                else
                {
                    tlid.knightage = item["lclname"];
                }
                pageteamListInfoDataList.Add(tlid);
                //itdList.Add(pageteamListInfoDataList.itemTeamDataList[i]);
                //teamPosMapid.Add(tlid);
            }
            pageItemTeamData.itemTeamDataList = pageteamListInfoDataList;                        
            a3_SpeedTeam.instance?.GetTeam_info(pageItemTeamData);
            dispatchEvent(GameEvent.Create(EVENT_CURPAGE_TEAM, this, data));
        }

        #region c2s
        public void SendCreateTeam(int v)//创建队伍(队伍的目标参数0-自定义，1-挂机。其余的根据组队的相关副本id)
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.CreateTeam;
            msg["ltpid"] = v;
            msg["ldiff"] = 0;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void sendobject_change(int v)//改变队伍目的。
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.ChangeObject;
            //       msg["iid"] = 120;
            msg["ltpid"] = v;
            msg["ldiff"] = 0;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendGetMapTeam(uint begin, uint end)//获取地图队伍
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.GetMapTeam;

            msg["idx_begin"] = begin;
            msg["idx_end"] = end;

            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }

        public void SendGetPageTeam(uint ltpid, uint begin, uint end,uint diff = 0)//获取地图队伍
        {         
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.Get_curPageTeam;
            msg["ltpid"] = ltpid;
            msg["idx_begin"] = begin;
            msg["idx_end"] = end;
            msg["ldiff"] = diff;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendWatchTeamInfo(uint tid)//查看队伍信息
        {

            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.WatchTeamInfo;
            msg["tid"] = tid;
            wantedWatchTeamId = tid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendSyncTeamBlood()//同步队伍血量
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.SyncTeamBlood;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendCurrentMapTeamPos()//当前地图,队友坐标
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.CurrentMapTeamPos;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendApplyJoinTeam(uint tid)//申请加入队伍
        {
            if (ApplyDic.ContainsKey(tid))
            {
                bool isCanApply = (Time.time - ApplyDic[tid]) > 10 ? true : false;//距离上次申请加入这个队伍超过10秒才可再次申请
                if (isCanApply)
                {
                    ApplyDic[tid] = Time.time;
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("TeamProxy_txt"));
                    return;
                }
            }
            else
            {
                ApplyDic[tid] = Time.time;
            }


            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.ApplyJoinTeam;
            msg["team_id"] = tid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendAffirmApply(uint cid, bool approved)//确认申请
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.AffirmApply;
            msg["cid"] = cid;
            msg["approved"] = approved;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendLeaveTeam(uint tid)//离开队伍
        {
            Variant msg = new Variant();  
            msg["team_cmd"] = (uint)TeamCmd.LeaveTeam;
            msg["team_id"] = tid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendTEAM(uint cid)
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = 14;
            msg["cid"] = cid;
            sendRPC(170, msg);
        }
        public void SendInvite(uint cid)//邀请
        {
            if (MyTeamData != null)
            {
                if (MyTeamData.itemTeamDataList.Count == 5) { flytxt.instance.fly(ContMgr.getCont("TeamProxy_maxpeople")); return; }
                else
                    flytxt.instance.fly(ContMgr.getCont("TeamProxy_sendok"));
                //在这里加邀请者list的name
                // debug.Log(MyTeamData.itemTeamDataList.Count.ToString());
            }
            else
            {
               // if(!now_Team)
                    flytxt.instance.fly(ContMgr.getCont("TeamProxy_creatrve"));
                //else
                   // flytxt.instance.fly(ContMgr.getCont("TeamProxy_join_craTeam"));
                return;
            }
            if (InvitedDic.ContainsKey(cid))
            {
                bool isCanInvite = (Time.time - InvitedDic[cid]) > 10 ? true : false;//距离上次邀请这个人超过10秒才可再次邀请
                if (isCanInvite)
                {
                    InvitedDic[cid] = Time.time;
                    // 
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("TeamProxy_text"));
                    return;
                }
            }
            else
            {
                InvitedDic[cid] = Time.time;
            }
            debug.Log("cid" + cid);
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.Invite;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendAffirmInvite(uint cid, uint tid, bool cofirmed)//case 10 确认邀请
        {

            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.AffirmInvite;
            msg["cid"] = cid;
            msg["tid"] = tid;
            msg["cofirmed"] = cofirmed;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendKickOut(uint cid)////踢出
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.KickOut;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendChangeCaptain(uint cid)////更换队长
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.ChangeCaptain;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendDissolve(uint teamId = 0)//解散
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.Dissolve;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
            //if (teamId != 0)
            //{
            //    Variant data = new Variant();
            //    data["teamid"] = teamId;
            //    dispatchEvent(GameEvent.Create(EVENT_DISSOLVETEAM, this, data));
            //}
        }
        public void SendEditorInfoDirJoin(bool dirJoin)//修改信息
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.EditorInfo;
            msg["dir_join"] = dirJoin;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendEditorInfoMembInv(bool membInv)//修改信息
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.EditorInfo;
            msg["memb_inv"] = membInv;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }
        public void SendReady(bool ready, uint ltpid, uint ldiff)//队员准备
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.Ready;
            msg["ready"] = ready;
            msg["ltpid"] = ltpid;
            msg["ldiff"] = ldiff;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }

        public void SendRoll(uint type , uint dpid)//roll
        {
            Variant msg = new Variant();
            msg["team_cmd"] = (uint)TeamCmd.Roll;
            msg["type"] = type;
            msg["dpid"] = dpid;
            sendRPC(PKG_NAME.C2S_CREATE_TEAM_RES, msg);
        }

        #endregion
        #region s2c
        void SetCreateTeam(Variant data)
        {
            //teamPosMapid1.Clear();
            if (MyTeamData != null)
            {
                MyTeamData = null; MyTeamData = new ItemTeamMemberData();
            }
            else
            {
                MyTeamData = new ItemTeamMemberData();
            }
            joinedTeam = true;

            uint teamId = data["teamid"];

            ItemTeamData itd = new ItemTeamData();
            itd.name = PlayerModel.getInstance().name;
            itd.lvl = PlayerModel.getInstance().lvl;
            itd.knightage = PlayerModel.getInstance().clanid.ToString();
            itd.mapId = PlayerModel.getInstance().mapid;
            itd.MembCount = 1;
            itd.cid = PlayerModel.getInstance().cid;
            itd.zhuan = PlayerModel.getInstance().up_lvl;
            itd.combpt = PlayerModel.getInstance().combpt;
            itd.teamId = teamId;
            itd.isCaptain = true;
            itd.showRemoveMemberBtn = false;
            itd.online = true;
            MyTeamData.teamId = teamId;
            MyTeamData.dirJoin = true;
            MyTeamData.membInv = false;
            MyTeamData.leaderCid = itd.cid;
            MyTeamData.meIsCaptain = true;
            MyTeamData.ltpid = data["ltpid"];
            MyTeamData.itemTeamDataList.Add(itd);
            dispatchEvent(GameEvent.Create(EVENT_CREATETEAM, this, data));
            SendEditorInfoDirJoin(true);//是否允许玩家加入
            if (!A3_TeamModel.getInstance().cidNameElse.ContainsKey(itd.cid))
                A3_TeamModel.getInstance().cidNameElse.Add(itd.cid, itd.name);
            //if (trage == true)
            //{

            //}
           //点击他人信息组队（若没有队伍先创建在组队）
           if(now_Team)
            {
                TeamProxy.getInstance().SendInvite(trage_cid);
                now_Team = false;
            }

        }
        void SetMapTeam(Variant data)
        {
            mapItemTeamData = new ItemTeamMemberData();
            List<ItemTeamData> teamListInfoDataList = new List<ItemTeamData>();
            uint totalCount = data["total_cnt"];
            uint idx_begin = data["idx_begin"];
            mapItemTeamData.totalCount = totalCount;
            mapItemTeamData.idxBegin = idx_begin;
            List<Variant> info = data["info"]._arr;
            foreach (Variant item in info)
            {
                ItemTeamData tlid = new ItemTeamData();
                tlid.teamId = item["tid"];
                tlid.curcnt = item["curcnt"];
                tlid.maxcnt = item["maxcnt"];
                tlid.name = item["lname"];
                tlid.carr = item["lcarr"];
                tlid.lvl = item["llevel"];
                tlid.zhuan = item["lzhuan"];
                tlid.mapId = item["lmapid"];
                tlid.ltpid = item["ltpid"];
                tlid.ldiff = item["ldiff"];
                tlid.members = item["members"]._arr;
                if (string.IsNullOrEmpty(item["lclname"]))
                {
                    tlid.knightage = ContMgr.getCont("a3_active_wuxianzhi");
                }
                else
                {
                    tlid.knightage = item["lclname"];
                }
                teamListInfoDataList.Add(tlid);
                //teamPosMapid.Add(tlid);
            }
            mapItemTeamData.itemTeamDataList = teamListInfoDataList;

            dispatchEvent(GameEvent.Create(EVENT_TEAMLISTINFO, this, mapItemTeamData));
        }
        void SetWatchTeamInfo(Variant dt)
        {

            List<ItemTeamData> teamMemberData = new List<ItemTeamData>();
            teamMemberData.Clear();
            teamMemberposData.Clear();
            foreach (Variant data in dt["members"]._arr)
            {
                uint cid = data["cid"];
                string name = data["name"];
                uint lvl = data["lvl"];
                uint zhuan = data["zhuan"];
                uint combpt = data["combpt"];
                uint carr = data["carr"];
                uint mapid=0;
               if(data.ContainsKey("mapid"))    
                mapid = data["mapid"];
               
                bool online = data["online"];
               
                string clname = data["clname"];
                if (string.IsNullOrEmpty(clname))
                {
                    clname =ContMgr.getCont("FriendProxy_wu");
                }
                ItemTeamData itd = new ItemTeamData();
                itd.cid = cid;
                itd.name = name;
                itd.lvl = lvl;
                itd.zhuan = zhuan;
                itd.combpt = (int)combpt;
                itd.carr = carr;
                
                itd.mapId = mapid;
                itd.online = online;
                itd.knightage = clname;
                teamMemberData.Add(itd);
                teamMemberposData.Add(itd);

            }
            uint captainCid = dt["lcid"];
            for (int i = 0; i < teamMemberData.Count; i++)
            {
                if (teamMemberData[i].cid == captainCid)
                {
                    teamMemberData[i].isCaptain = true;
                }
                else
                {
                    teamMemberData[i].isCaptain = false;
                }
            }
            ArrayList arry = new ArrayList();
            arry.Add(teamMemberData);
            if (worldmap.getmapid == false)
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TEAMMEMBERLIST, arry);

        }
        void SetSyncTeamBlood(Variant data)//case 4 同步队伍血量
        {
            dispatchEvent(GameEvent.Create(EVENT_SYNCTEAMBLOOD, this, data));
        }
        void GetTeamPos(Variant data)//队员坐标   打断点可以知道data    是mempos = [{cid,x,y}]
        {

            //debug.Log("GetTeamPos:::"+data.dump());
            teamlist_position.Clear();
            if (data != null)
            {
                List<Variant> l = data["mempos"]._arr;
                foreach (var v in l)
                {
                    TeamPosition temp = new TeamPosition();
                    temp.cid = v["cid"];
                    temp.x = (uint)((v["x"]) / 53.3f);
                    temp.y = (uint)((v["y"]) / 53.3f);
                    teamlist_position.Add(temp);
                }
            }
        }
        void SetApplyJoinTeam(Variant data)
        {
            flytxt.instance.fly(ContMgr.getCont("TeamProxy_havesend"));
        }
        void SetLeaveTeam(Variant data)
        {
            MyTeamData = null;
            dispatchEvent(GameEvent.Create(EVENT_LEAVETEAM, this, null));
            A3_TeamModel.getInstance().cidNameElse.Clear();
            MonsterMgr._inst.HideInvaildMonster();
            flytxt.instance.fly(ContMgr.getCont("TeamProxy_hanveout"));
        }
        void SetAffirmInvite(Variant data)//10 确认邀请
        {
            ItemTeamMemberData AffirmInviteData = new ItemTeamMemberData();

            if (data.ContainsKey("cofirmed")) { bool cofirmed = data["cofirmed"]; AffirmInviteData.cofirmed = cofirmed; }
            if (data.ContainsKey("tid"))
            {
                uint tid = data["tid"];
                AffirmInviteData.teamId = tid;
                List<ItemTeamData> itdList = new List<ItemTeamData>();
                List<Variant> plysList = data["plys"]._arr;
                uint leaderCid = data["leader_cid"];
                bool dirJoin = data["dir_join"];
                bool membInv = data["memb_inv"];
                uint ltpid = data["ltpid"];
                AffirmInviteData.ltpid = ltpid;
                AffirmInviteData.leaderCid = leaderCid;
                AffirmInviteData.dirJoin = dirJoin;
                AffirmInviteData.membInv = membInv;
                if (MyTeamData == null) MyTeamData = new ItemTeamMemberData();

                MyTeamData.teamId = tid;
                MyTeamData.leaderCid = leaderCid;
                MyTeamData.dirJoin = dirJoin;
                MyTeamData.membInv = membInv;
                MyTeamData.ltpid = ltpid;
                foreach (Variant pl in plysList)
                {
                    uint cid = pl["cid"];
                    string name = pl["name"];
                    uint lvl = pl["lvl"];
                    uint zhuan = pl["zhuan"];
                    uint combpt = pl["combpt"];
                    uint carr = pl["carr"];
                    bool online = pl["online"];
                    ItemTeamData itd = new ItemTeamData();
                    itd.cid = cid;
                    itd.name = name;
                    itd.lvl = lvl;
                    itd.zhuan = zhuan;
                    itd.combpt = (int)combpt;
                    itd.carr = carr;
                    itd.online = online;
                    itd.isCaptain = leaderCid == cid ? true : false;
                    itd.showRemoveMemberBtn = false;
                    itdList.Add(itd);
                    MyTeamData.itemTeamDataList.Add(itd);
                    if ((ltpid == 108 || ltpid == 109 || ltpid == 110 || ltpid == 111) && !A3_TeamModel.getInstance().cidName.ContainsKey(cid))
                    {
                        A3_TeamModel.getInstance().cidName.Add(cid, name);
                    }
                    if (!A3_TeamModel.getInstance().cidNameElse.ContainsKey(cid))
                    {
                        A3_TeamModel.getInstance().cidNameElse.Add(cid, name);
                    }
                }
                if (ltpid == 108 || ltpid == 109 || ltpid == 110 || ltpid == 111)//组队副本直接跳到组队页面
                {
                    //ArrayList arr = new ArrayList();
                    //arr.Add(1);
                    //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, arr);
                    //a3_counterpart.instance?.transform.SetAsLastSibling();
                    //a3_counterpart.instance?.getGameObjectByPath("currentTeam").SetActive(true);
                    A3_TeamModel.getInstance().bein = true;
                    A3_TeamModel.getInstance().ltpids = ltpid;

                }
                //服务器返回的是不包含自己在内的人,需要把自己加进去
                ItemTeamData itdSelf = new ItemTeamData();
                itdSelf.cid = PlayerModel.getInstance().cid;
                itdSelf.name = PlayerModel.getInstance().name;
                itdSelf.lvl = PlayerModel.getInstance().lvl;
                itdSelf.zhuan = PlayerModel.getInstance().up_lvl;
                itdSelf.combpt = (int)PlayerModel.getInstance().combpt;
                itdSelf.carr = (uint)PlayerModel.getInstance().profession;
                itdSelf.online = true;
                itdSelf.isCaptain = false;
                itdSelf.showRemoveMemberBtn = false;
                itdSelf.ltpid = data["ltpid"];
                itdList.Add(itdSelf);

                AffirmInviteData.itemTeamDataList = itdList;
                MyTeamData.itemTeamDataList.Add(itdSelf);
                A3_TeamModel.getInstance().AffirmInviteData = AffirmInviteData;
                dispatchEvent(GameEvent.Create(EVENT_AFFIRMINVITE, this, data));
                joinedTeam = true;
                MonsterMgr._inst.RefreshVaildMonster();
            }
        }

        void SetKickOut(Variant data)
        {
            uint cid = data["cid"];
            int memberCount = MyTeamData.itemTeamDataList.Count;
            for (int i = 0; i < memberCount; i++)
            {
                if (MyTeamData.itemTeamDataList[i].cid == cid)
                {
                    MyTeamData.itemTeamDataList.Remove(MyTeamData.itemTeamDataList[i]);
                    MyTeamData.removedIndex = (uint)i;//踢出的位置
                    dispatchEvent(GameEvent.Create(EVENT_KICKOUT, this, data));
                    break;
                }
            }
            if (A3_TeamModel.getInstance().cidNameElse.ContainsKey(cid))
                A3_TeamModel.getInstance().cidNameElse.Remove(cid);
        }
        void SetChangeCaptain(Variant data)//12更换队长
        {
            //先将原队长的信息设置了
            for (int i = 0; i < MyTeamData.itemTeamDataList.Count; i++)
            {
                if (MyTeamData.leaderCid == MyTeamData.itemTeamDataList[i].cid)
                {
                    MyTeamData.itemTeamDataList[i].isCaptain = false;
                    MyTeamData.itemTeamDataList[i].showRemoveMemberBtn = true;
                    break;
                }
            }

            //再更换现任队长的信息
            uint cid = data["cid"];
            MyTeamData.leaderCid = cid;
            if (cid == PlayerModel.getInstance().cid) MyTeamData.meIsCaptain = true;
            for (int i = 0; i < MyTeamData.itemTeamDataList.Count; i++)
            {
                if (MyTeamData.itemTeamDataList[i].cid == cid)
                {

                    MyTeamData.itemTeamDataList[i].isCaptain = true;
                    if (MyTeamData.meIsCaptain && MyTeamData.itemTeamDataList[i].cid != PlayerModel.getInstance().cid)
                    {
                        MyTeamData.itemTeamDataList[i].showRemoveMemberBtn = true;
                    }
                    else
                    {
                        MyTeamData.itemTeamDataList[i].showRemoveMemberBtn = false;
                    }
                    if (cid == PlayerModel.getInstance().cid)
                    {
                        MyTeamData.meIsCaptain = true;
                    }
                    else
                    {
                        MyTeamData.meIsCaptain = false;
                    }
                }
                else
                {
                    MyTeamData.itemTeamDataList[i].isCaptain = false;
                }
            }
            dispatchEvent(GameEvent.Create(EVENT_CHANGECAPTAIN, this, null));
        }
        void SetDissolve(Variant data)//13解散队伍
        {
            A3_TeamModel.getInstance().cidNameElse.Clear();
            joinedTeam = false;
            MyTeamData = null;
            dispatchEvent(GameEvent.Create(EVENT_DISSOLVETEAM, this, data));
        }
        void SetChangeTeamInfo(Variant data)//14修改队伍信息
        {
            if (data.ContainsKey("dir_join"))
            {
                bool dirJoin = data["dir_join"];
                MyTeamData.dirJoin = dirJoin;
            }
            if (data.ContainsKey("memb_inv"))
            {
                bool membInv = data["memb_inv"];
                MyTeamData.membInv = membInv;
            }
            dispatchEvent(GameEvent.Create(EVENT_CHANGETEAMINFO, this, data));
        }
        void SetNoticeCaptainNewInfo(Variant data)
        {
            uint cid = data["cid"];
            string name = data["name"];
            uint lvl = data["lvl"];
            uint zhuan = data["zhuan"];
            uint carr = data["carr"];
            uint combpt = data["combpt"];
            ItemTeamData itd = new ItemTeamData();
            itd.cid = cid;
            itd.name = name;
            itd.lvl = lvl;
            itd.zhuan = zhuan;
            itd.carr = carr;
            itd.combpt = (int)combpt;
            itd.showRemoveMemberBtn = true;
            if (!A3_TeamModel.getInstance().cidNameElse.ContainsKey(itd.cid))
                A3_TeamModel.getInstance().cidNameElse.Add(itd.cid, itd.name);
            a3_teamApplyPanel.mInstance.Show(itd);

        }
        void SetTeamReady(Variant data)
        {
            ArrayList arr = new ArrayList();
            arr.Add(EVENT_TEAM_READY);
            arr.Add(data);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART,arr);
            //dispatchEvent(GameEvent.Create(EVENT_TEAM_READY, this, data));
        }
        void SetNoticeHaveMemberLeave(Variant data)//22通知有成员离开队伍
        {
            uint cid = data["cid"];
            int memberCount = MyTeamData.itemTeamDataList.Count;
            for (int i = 0; i < memberCount; i++)
            {
                if (MyTeamData.itemTeamDataList[i].cid == cid)
                {
                    MyTeamData.itemTeamDataList.Remove(MyTeamData.itemTeamDataList[i]);
                    MyTeamData.removedIndex = (uint)i;
                    dispatchEvent(GameEvent.Create(EVENT_NOTICEHAVEMEMBERLEAVE, this, data));
                    break;
                }
            }
            if (A3_TeamModel.getInstance().cidNameElse.ContainsKey(cid))
                A3_TeamModel.getInstance().cidNameElse.Remove(cid);
        }

        void SetNoticeInvite(Variant data)
        {
            if (GlobleSetting.REFUSE_TEAM_INVITE) return;//客户端开启拒绝组队邀请

            uint tid = data["tid"];
            uint cid = data["cid"];
            string name = data["name"];
            uint lvl = data["lvl"];
            uint zhuan = data["zhuan"];
            uint carr = data["carr"];
            uint combpt = data["combpt"];
            ItemTeamData itd = new ItemTeamData();
            itd.teamId = tid;
            itd.cid = cid;
            itd.name = name;
            itd.lvl = lvl;
            itd.zhuan = zhuan;
            itd.carr = carr;
            itd.combpt = (int)combpt;
            if (MyTeamData == null)
            {
                dispatchEvent(GameEvent.Create(EVENT_NOTICEINVITE, this, data));
            }
        }
        void SetNewMemberJoin(Variant data)//新成员入队
        {
            uint cid = data["cid"];
            string name = data["name"];
            uint lvl = data["lvl"];
            uint zhuan = data["zhuan"];
            uint carr = data["carr"];
            uint combpt = data["combpt"];
            ItemTeamData itd = new ItemTeamData();
            itd.cid = cid;
            itd.name = name;
            itd.lvl = lvl;
            itd.zhuan = zhuan;
            itd.carr = carr;
            itd.combpt = (int)combpt;
            itd.isCaptain = false;
            itd.online = true;
            if (MyTeamData.meIsCaptain)
            {
                itd.showRemoveMemberBtn = true;
            }
            else
            {
                itd.showRemoveMemberBtn = false;
            }

            ArrayList array = new ArrayList();
            array.Add(itd);
            bool isNewMember = true;
            for (int i = 0; i < MyTeamData.itemTeamDataList.Count; i++)//断线重连也是新玩家,需要判断下
            {
                if (MyTeamData.itemTeamDataList[i].cid == cid)
                {
                    isNewMember = false;
                    MyTeamData.itemTeamDataList[i].online = true;
                    dispatchEvent(GameEvent.Create(EVENT_NOTICEONLINESTATECHANGE, this, data));//TODO:如果存在则直接派发一个更改在线状态的消息
                    break;
                }
            }
            if (isNewMember)
            {
                MyTeamData.itemTeamDataList.Add(itd);
                flytxt.instance.fly(itd.name + ContMgr.getCont("TeamProxy_add"));
                dispatchEvent(GameEvent.Create(EVENT_NEWMEMBERJOIN, this, data));
            }
            if (!A3_TeamModel.getInstance().cidNameElse.ContainsKey(itd.cid))
                A3_TeamModel.getInstance().cidNameElse.Add(itd.cid, itd.name);
        }
        void SetNoticeInviteBeRefuse(Variant data)//通知邀请被拒绝
        {
            uint cid = data["cid"];
            string name = data["name"];
            //flytxt.instance.fly(name + "拒绝了您的队伍邀请.");
        }
        private void SetTeamobject_Change(Variant data)//更改队伍目的
        {
            uint ltpid = data["ltpid"];
            if (ltpid == 108 || ltpid == 109 || ltpid == 110 || ltpid == 111)
            {
                A3_TeamModel.getInstance().cidName = A3_TeamModel.getInstance().cidNameElse;
                ArrayList arr = new ArrayList();
                arr.Add(1);
                TeamProxy.getInstance().MyTeamData.ltpid = ltpid;
                A3_TeamModel.getInstance().bein = true;
                A3_TeamModel.getInstance().ltpids = ltpid;
                //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, arr);
                //a3_counterpart.instance?.transform.SetAsLastSibling();
                //a3_counterpart.instance?.changePos();
                //a3_counterpart.instance?.getGameObjectByPath("currentTeam").SetActive(true);
            }
            //else
            //{                
            //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
            //    a3_counterpart.instance?.getGameObjectByPath("currentTeam").SetActive(false);
            //    a3_counterpart.instance?.getGameObjectByPath("haoyou").SetActive(false);
            //    a3_counterpart.instance?.getGameObjectByPath("yaoqing").SetActive(false);
            //}
            dispatchEvent(GameEvent.Create(EVENT_TEAMOBJECT_CHANGE, this, data));
        }

        void SetNoticeOnlineStateChange(Variant data)//在线状态变更
        {
            uint cid = data["cid"];
            bool online = data["online"];
            int count = MyTeamData.itemTeamDataList.Count;
            int offLineIndex = -1;
            for (int i = 0; i < count; i++)
            {
                if (MyTeamData.itemTeamDataList[i].online == false && offLineIndex == -1)//取得第一个位置的离线队友位置
                {
                    offLineIndex = i;
                }
                if (MyTeamData.itemTeamDataList[i].cid == cid)
                {
                    if (online)
                    {
                        ItemTeamData itd = MyTeamData.itemTeamDataList[i];
                        MyTeamData.itemTeamDataList.RemoveAt(i);
                        itd.online = online;
                        MyTeamData.itemTeamDataList.Insert(offLineIndex, itd);
                    }
                    else
                    {
                        ItemTeamData itd = MyTeamData.itemTeamDataList[i];
                        MyTeamData.itemTeamDataList.RemoveAt(i);
                        itd.online = online;
                        MyTeamData.itemTeamDataList.Add(itd);
                    }

                    break;
                }
            }
            dispatchEvent(GameEvent.Create(EVENT_NOTICEONLINESTATECHANGE, this, data));
        }

        void SetNoticeLvOrCombptChange(Variant data)//战斗力等级变更
        {
            //if (MyTeamData == null || (MyTeamData != null && MyTeamData.itemTeamDataList.Count <= 0))
            //    return;
            //else
            //{
            //    for (int i = 0; i < MyTeamData.itemTeamDataList.Count; i++)
            //    {
            //        MyTeamData.itemTeamDataList[i].lvl = data["lvl"];
            //        MyTeamData.itemTeamDataList[i].zhuan = data["zhuan"];
            //        MyTeamData.itemTeamDataList[i].combpt = data["combpt"];
            //    }
            //}
            ////if (teamMemberposData == null)
            ////    return;
            ////else
            ////{
            ////    for(int i=0;i<teamMemberposData.Count;i++)
            ////    {
            ////        if(teamMemberposData[i].cid==data["cid"])
            ////        {
            ////            teamMemberposData[i].lvl = data["lvl"];
            ////            teamMemberposData[i].zhuan = data["zhuan"];
            ////            teamMemberposData[i].combpt = data["combpt"];
            ////        }
            ////    }
            ////}
        }

        public void SetTeamPanelInfo()
        {
            if (MyTeamData != null)
            {
                a3_currentTeamPanel._instance.gameObject.SetActive(true);
                a3_teamPanel._instance.gameObject.SetActive(false);
                a3_currentTeamPanel._instance.Show(MyTeamData);
            }
            else
            {
                a3_teamPanel._instance.gameObject.SetActive(true);
                a3_currentTeamPanel._instance.gameObject.SetActive(false);
            }
        }
        #endregion
        enum TeamCmd
        {
            //c2s
            CreateTeam = 1,//创建队伍
            GetMapTeam,//获取地图队伍
            WatchTeamInfo,//查看队伍信息
            SyncTeamBlood,//同步队伍血量
            CurrentMapTeamPos,//当前地图,队友坐标
            ApplyJoinTeam = 6,//申请加入队伍
            AffirmApply,//确认申请
            LeaveTeam,//离开队伍
            Invite = 9,//邀请
            AffirmInvite,//确认邀请
            KickOut,//踢出
            ChangeCaptain = 12,//更换队长
            Dissolve,//解散
            EditorInfo,//修改信息
            ChangeObject,//改变队伍目的
            Ready = 16,//队员准备
            Get_curPageTeam = 17,
            Roll = 18,// roll 点

            //s2c
            NoticeCaptainNewInfo = 20,//通知队长有新的申请
            NoticeApplyBeRefuse = 21,//通知申请被拒绝
            NoticeHaveMemberLeave = 22,//通知有成员离开队伍
            NoticeInvite = 23,//通知邀请 
            NewMemberJoin = 24,//新队员入队
            NoticeInviteBeRefuse = 25,//通知邀请被拒绝
            NoticeOnlineStateChanged = 26,//在线状态变更
            NoticeCombptOrLvChange=27,    //队伍战斗力等级变更
            DropItemRoll = 28     //roll点的物品 组队副本

        }

    }
}
