using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
using System.Collections;
using System;

namespace MuGame
{
    class A3_LegionProxy : BaseProxy<A3_LegionProxy>
    {
        public const int EVENT_GETINFO = 1;             //请求军团信息
        public const int EVENT_CREATE = 2;              //请求创建军团
        public const int EVENT_APPLYFOR = 3;            //请求加入军团
        public const int EVENT_APPROVEORREJECT = 4;     //批准或拒绝申请人
        public const int EVENT_LVUP = 5;                //升级军团
        public const int EVENT_DONATE = 6;              //捐赠
        public const int EVENT_PRODE = 7;               //升职降职
        public const int EVENT_BELEADER = 8;            //转移团长
        public const int EVENT_QUIT = 9;                //请求离开军团
        public const int EVENT_REMOVE = 10;             //请求踢出军团
        public const int EVENT_APPLYMODE = 11;          //是否允许申请人不需要验证直接加入
        public const int EVENT_CHANGENOTICE = 12;       //修改骑士团通告
        public const int EVENT_GETMEMBER = 14;          //请求成员信息
        public const int EVENT_GETDIARY = 15;           //获取日志
        public const int EVENT_GETAPPLICANT = 16;       //获取申请人列表
        public const int EVENT_LOADLIST = 17;           //请求全部军团列表
        public const int EVENT_INVITE = 18;             //邀请加入骑士团
        public const int EVENT_ACCEPTAINVITE = 19;		//接受邀请加入骑士团
        public const int EVENT_DIRECT = 21;//军团邀请时的状态

        public const int EVENT_DELETECLAN = 22;         //解散骑士团
        public const int EVENT_CHECKNAME = 24;			//检查创建时的名称是否合理
        public const int EVENT_TASKREWARD = 32;         //完成军团任务获得奖励
        public const int EVENT_BEINVITE = 33;           //被邀请加入骑士团
        public const int EVENT_APPLYSUCCESSFUL = 34;    //申请加入成功
        public const int EVENT_GETQUIT = 36;            //被踢出骑士团
        public const int EVENT_GETDIN = 37;             //直接加入军团成功
        public const int EVENT_UPBUFF = 25;             //升级军团buff
        public const int EVENT_REPAIR = 26;             //军团维护消息
        public const int EVENT_FINDNAME = 27;           //模糊搜索军团名称
        public const int EVENT_CHANGE_NAME = 29;           // 修改工会名字
        public const int EVENT_BUILD = 28;              //军团建设

        public int gold;
        public int lvl;
        public bool join_legion;
        public Variant cacheProxyData;
        public A3_LegionProxy()
        {
            addProxyListener(PKG_NAME.C2S_CREATE_CLAN_RES, OnLegion);//军团信息
        }

        //请求成员信息
        public void SendGetMember()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_GETMEMBER;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        public void SendLegion_Repair()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_REPAIR;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //请求军团信息
        public void SendGetInfo()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_GETINFO;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求加入军团
        public void SendApplyFor(uint clid)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_APPLYFOR;
            msg["clid"] = clid;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //升级军团buff
        public void SendUp_Buff()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_UPBUFF;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }



        //批准或拒绝申请人
        public void SendYN(uint cid, bool yes)
        {
            Variant msg = new Variant();
            msg["cid"] = cid;
            msg["clan_cmd"] = EVENT_APPROVEORREJECT;
            msg["approved"] = yes;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //升级军团
        public void SendLVUP()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_LVUP;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //捐款
        public void SendDonate(uint money)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_DONATE;
            msg["money"] = money;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //建设
        public void SendBuild(uint id)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_DONATE;
            msg["type"] = id;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //建设奖励
        public void SendBuildAwd(uint id)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_BUILD;
            msg["type"] = id;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //转移团长
        public void SendBeLeader(uint cid)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_BELEADER;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //升职或降职(type:1升职，0降职)
        public void PromotionOrDemotion(uint cid, uint type)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_PRODE;
            msg["cid"] = cid;
            msg["tp"] = type;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //修改允许直接加入 <!-- =true 直接加入 =false 需要验证 -->
        public void SendChangeApplyMode(bool b)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_APPLYMODE;
            msg["direct_join"] = b ? 1 : 0;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        public void SendChangeToggleMode(bool b)//有军团邀请时自动加入与否
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_DIRECT;
            msg["direct_join"] = b ? 1 : 0;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //请求离开军团
        public void SendQuit()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_QUIT;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //邀请加入骑士团
        public void SendInvite(uint cid)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_INVITE;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求离开军团
        public void SendRemove(uint cid)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_REMOVE;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //获取申请人列表
        public void SendGetApplicant()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_GETAPPLICANT;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求日志信息
        public void SendGetDiary()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_GETDIARY;
            if (A3_LegionModel.getInstance().logdata != null &&
                A3_LegionModel.getInstance().logdata.ContainsKey("clanlog_list") &&
                A3_LegionModel.getInstance().logdata["clanlog_list"]._arr.Count > 0)
            {
                Variant list = A3_LegionModel.getInstance().logdata["clanlog_list"];
                int id = 0;
                foreach (var v in list._arr)
                {
                    int x = v["id"];
                    if (x > id) id = x;
                }
                msg["id"] = id;
            }
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求全部军团列表
        public void SendGetList()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_LOADLIST;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }
        //修改公告
        public void SendChangeNotice(string notice)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_CHANGENOTICE;
            msg["notice"] = notice;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求创建军团
        public void SendCreateLegion(string name)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_CREATE;
            msg["clname"] = name;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //请求解散军团
        public void SendDeleteLegion()
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_DELETECLAN;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //检查创建时的名称是否合理
        public void SendCheckName(string name)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_CHECKNAME;
            msg["clname"] = name;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        //接受邀请加入骑士团
        public void SendAcceptInvite(uint clanid, bool approved)
        {

            if (approved)
                join_legion = true;

            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_ACCEPTAINVITE;
            msg["clanid"] = clanid;
            msg["approved"] = approved;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);


        }

        public void ChangeLegionName(string legionName)
        {
            if (legionName == null || legionName.Equals(string.Empty)) return;

            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_CHANGE_NAME;
            msg["clname"] = legionName;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);

        }

        //模糊搜索
        public void sendfindname(string name)
        {
            Variant msg = new Variant();
            msg["clan_cmd"] = EVENT_FINDNAME;
            msg["clname"] = name;
            sendRPC(PKG_NAME.C2S_CREATE_CLAN_RES, msg);
        }

        void OnLegion(Variant data)
        {
            int res = data["res"];
            debug.Log("军团消息" + data.dump());
            //if (res < 0)
            //{
            //    Globle.err_output(res);
            //    return;
            //}
            Variant vd = new Variant();
            switch (res)
            {
                case EVENT_GETINFO:
                    //if (data.ContainsKey("direct_join_clan"))
                    //{

                    //}
                    if (data.ContainsKey("id"))
                    {
                        A3_LegionData d = new A3_LegionData();
                        d.id = data["id"];
                        d.lvl = data["lvl"];
                        d.clname = data["clname"];
                        d.name = data["name"];
                        d.notice = data["notice"];
                        d.gold = data["money"];
                        d.plycnt = data["plycnt"];
                        d.exp = data["clan_pt"];
                        if (data.ContainsKey("ol_cnt")) d.ol_cnt = data["ol_cnt"];
                        if (data.ContainsKey("combpt")) d.combpt = data["combpt"];
                        if (data.ContainsKey("rankidx")) d.rankidx = data["rankidx"];
                        if (data.ContainsKey("clanc")) d.clanc = data["clanc"];
                        d.anabasis_tm = data["anabasis_tm"];
                        A3_LegionModel.getInstance().myLegion = d;
                        int b = data["direct_join"];
                        A3_LegionModel.getInstance().CanAutoApply = b == 1 ? true : false;
                        A3_LegionModel.getInstance().SetMyLegion(d.lvl);
                        A3_LegionModel.getInstance().donate = data["donate"];
                        // a3_legion_info.mInstance.jx_up(d.gold,d.lvl);
                        gold = d.gold;
                        lvl = d.lvl;

                    }
                    else
                    {
                        A3_LegionModel.getInstance().myLegion = new A3_LegionData();
                    }
                    dispatchEvent(GameEvent.Create(EVENT_GETINFO, this, data));
                    break;
                case EVENT_CREATE:
                    dispatchEvent(GameEvent.Create(EVENT_CREATE, this, data));
                    removeEventListener(A3_LegionProxy.EVENT_CHECKNAME, a3_legion.mInstance.SetCheckName);
                    //a3_herohead.instance.legion_bf = true;
                    //a3_herohead.instance.isclear = true;
                    //if (a3_herohead.instance != null)
                    //    a3_herohead.instance.refresBuff();
                    break;
                case EVENT_APPLYFOR:
                    int clid = data["clid"];
                    dispatchEvent(GameEvent.Create(EVENT_APPLYFOR, this, data));
                    flytxt.instance.fly(ContMgr.getCont("Legion_ok"));
                    break;
                case EVENT_LVUP:
                    A3_LegionModel.getInstance().myLegion.gold = data["money"];
                    A3_LegionModel.getInstance().myLegion.lvl = data["lvl"];
                    A3_LegionModel.getInstance().myLegion.exp = 0;
                    A3_LegionModel.getInstance().SetMyLegion(A3_LegionModel.getInstance().myLegion.lvl);
                    dispatchEvent(GameEvent.Create(EVENT_GETINFO, this, data));
                    flytxt.instance.fly(ContMgr.getCont("Legionup_ok"));
                    break;
                case EVENT_GETMEMBER://14
                    vd = data["pls"];
                    A3_LegionModel.getInstance().members.Clear();
                    foreach (var v in vd._arr)
                    {
                        A3_LegionModel.getInstance().AddMember(v);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_GETMEMBER, this, data));
                    //if (a3_legion.mInstance == null)
                    //    cacheProxyData = data;
                    break;
                case EVENT_APPROVEORREJECT:
                    dispatchEvent(GameEvent.Create(EVENT_APPROVEORREJECT, this, data));
                    break;
                case EVENT_QUIT:
                    A3_LegionModel.getInstance().myLegion = new A3_LegionData();
                    dispatchEvent(GameEvent.Create(EVENT_QUIT, this, data));
                    if (a3_task_auto.instance.executeTask?.taskT == TaskType.CLAN)
                    {
                        SelfRole.fsm.Stop();
                        flytxt.instance.fly(ContMgr.getCont("Legion_out"));
                    }
                    else
                        flytxt.instance.fly(ContMgr.getCont("Legionout_ok"));
                    //a3_herohead.instance.legion_bf = false;
                    //a3_herohead.instance.isclear =true;
                    //if (a3_herohead.instance != null)
                    //    a3_herohead.instance.refresBuff();


                    break;
                case EVENT_UPBUFF:

                    flytxt.instance.fly(ContMgr.getCont("Legionbuff_up"));
                    break;



                case EVENT_DONATE:
                    int money = data["money"];
                    //flytxt.instance.fly("获得了" + money / 1000 + "点贡献度");
                    flytxt.instance.fly(ContMgr.getCont("Legiondonate_add",new List<string> { (money / 1000).ToString() } ));
                    SendGetInfo();
                    break;
                case EVENT_GETAPPLICANT:
                    A3_LegionModel.getInstance().RefreshApplicant(data);
                    dispatchEvent(GameEvent.Create(EVENT_GETAPPLICANT, this, data));
                    break;
                case EVENT_CHANGENOTICE:
                    dispatchEvent(GameEvent.Create(EVENT_CHANGENOTICE, this, data));
                    break;
                case EVENT_INVITE:

                    dispatchEvent(GameEvent.Create(EVENT_INVITE, this, data));
                    break;
                case EVENT_BELEADER:
                    SendGetMember();
                    break;
                case EVENT_GETDIARY:
                    if (data != null && data.ContainsKey("clanlog_list") && data["clanlog_list"]._arr.Count > 0)
                    {
                        A3_LegionModel.getInstance().AddLog(data);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_GETDIARY, this, data));
                    break;
                case EVENT_APPLYMODE:
                    int c = data["direct_join"];
                    A3_LegionModel.getInstance().CanAutoApply = c == 1 ? true : false;
                    dispatchEvent(GameEvent.Create(EVENT_APPLYMODE, this, data));
                    break;
                case EVENT_CHECKNAME:
                    dispatchEvent(GameEvent.Create(EVENT_CHECKNAME, this, data));
                    break;
                case EVENT_DELETECLAN:
                    SendGetInfo();
                    dispatchEvent(GameEvent.Create(EVENT_DELETECLAN, this, data));
                    flytxt.instance.fly(ContMgr.getCont("Legion_bye"));
                    //a3_herohead.instance.legion_bf = false;
                    //a3_herohead.instance.isclear = true;
                    //if (a3_herohead.instance != null)
                    //    a3_herohead.instance.refresBuff();
                    break;
                case EVENT_LOADLIST:
                    A3_LegionModel.getInstance().list.Clear();
                    A3_LegionModel.getInstance().list2.Clear();
                    vd = data["info"];
                    foreach (var v in vd._arr)
                    {
                        A3_LegionData d = new A3_LegionData();
                        d.id = v["id"];
                        d.clname = v["clname"];
                        d.combpt = v["combpt"];
                        d.lvl = v["lvl"];
                        d.name = v["name"];
                        d.plycnt = v["plycnt"];
                        d.direct_join = v["direct_join"];
                        d.huoyue = v["last_active"];
                        A3_LegionModel.getInstance().list.Add(d);
                        A3_LegionModel.getInstance().list2.Add(d);

                    }

                    //for (int i = 0; i < A3_LegionModel.getInstance().list.Count; i++)
                    //{
                    //    for (int j = 0; j < A3_LegionModel.getInstance().list.Count; j++)
                    //    {
                    //        if (A3_LegionModel.getInstance().list[i].id > A3_LegionModel.getInstance().list[j].id)
                    //        {
                    //            A3_LegionData temp = A3_LegionModel.getInstance().list[i];
                    //            A3_LegionModel.getInstance().list[i] = A3_LegionModel.getInstance().list[j];
                    //            A3_LegionModel.getInstance().list[j] = temp;

                    //        }
                    //    }                       
                    //}




                    dispatchEvent(GameEvent.Create(EVENT_LOADLIST, this, data));
                    break;
                case EVENT_REMOVE:
                    int ci = data["cid"];
                    if (A3_LegionModel.getInstance().members.ContainsKey(ci))
                    {
                        A3_LegionModel.getInstance().members.Remove(ci);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_GETMEMBER, this, data));
                    break;
                case EVENT_REPAIR:
                  
                    dispatchEvent(GameEvent.Create(EVENT_REPAIR, this, data));
                 
                    break;
                case EVENT_TASKREWARD:
                    if (A3_LegionModel.getInstance().myLegion.id != 0)
                    {
                        int taskCount;
                        if (A3_TaskModel.getInstance() == null || A3_TaskModel.getInstance().GetClanTask() == null)
                            taskCount = 9;
                        else
                            taskCount = A3_TaskModel.getInstance().GetClanTask().taskCount;
                        Dictionary<uint, int> rewardDic = A3_TaskModel.getInstance().GetClanRewardDic(taskCount);
                        flytxt.instance.StopDelayFly();
                        if (data.ContainsKey("money"))
                        {
                            if (rewardDic.ContainsKey((uint)A3_TaskModel.REWARD_CLAN_MONEY))
                                flytxt.instance.AddDelayFlytxt(ContMgr.getCont("Legion_money",new List<string> { (rewardDic[(uint)A3_TaskModel.REWARD_CLAN_MONEY]).ToString() }));
                            //flytxt.instance.AddDelayFlytxt("军团资金+" + rewardDic[(uint)A3_TaskModel.REWARD_CLAN_MONEY]);
                            A3_LegionModel.getInstance().myLegion.gold = data["money"]._int;
                        }
                        if (data.ContainsKey("clan_pt"))
                        {
                            if (rewardDic.ContainsKey((uint)A3_TaskModel.REWARD_CLAN_EXP))
                                flytxt.instance.AddDelayFlytxt(ContMgr.getCont("Legion_exp", new List<string> { (rewardDic[(uint)A3_TaskModel.REWARD_CLAN_EXP]).ToString() }));
                            //flytxt.instance.AddDelayFlytxt("军团经验+" + rewardDic[(uint)A3_TaskModel.REWARD_CLAN_EXP]);
                            A3_LegionModel.getInstance().myLegion.clan_pt = data["clan_pt"]._int;
                        }
                        if (data.ContainsKey("donate"))
                        {
                            if (rewardDic.ContainsKey((uint)A3_TaskModel.REWARD_CLAN_DONATE))
                                flytxt.instance.AddDelayFlytxt(ContMgr.getCont("Legion_gongxian", new List<string> { (rewardDic[(uint)A3_TaskModel.REWARD_CLAN_DONATE]).ToString() }));
                            //flytxt.instance.AddDelayFlytxt("军团贡献+" + rewardDic[(uint)A3_TaskModel.REWARD_CLAN_DONATE]);
                            A3_LegionModel.getInstance().donate = data["donate"]._int;
                        }
                        if (data.ContainsKey("active"))
                            A3_LegionModel.getInstance().myLegion.huoyue = data["active"]._int;
                        flytxt.instance.StartDelayFly();
                    }
                    break;
                case EVENT_BEINVITE:
                    uint clanid = data["clanid"];
                    string name = data["name"];
                    string clan_name = data["clan_name"];
                    int clan_lvl = data["clan_lvl"];
                    if (a3_legion.mInstance.dic0.isOn)
                    {
                        SendAcceptInvite(clanid, true);
                        flytxt.instance.fly(ContMgr.getCont("Legion_request"));
                        a3_dartproxy.getInstance().sendDartGo();//查看军团镖车信息
                        dispatchEvent(GameEvent.Create(EVENT_ACCEPTAINVITE, this, data)); 
                    }
                    else
                    {
                        MsgBoxMgr.getInstance().showConfirm(name + ContMgr.getCont("add_request") + clan_lvl + ContMgr.getCont("lvLegion") + clan_name, () =>
                        {
                            SendAcceptInvite(clanid, true);
                        }, () =>
                        {
                            SendAcceptInvite(clanid, false);
                        });
                    }
                    dispatchEvent(GameEvent.Create(EVENT_BEINVITE, this, data));
                    break;
                case EVENT_APPLYSUCCESSFUL:
                    bool approved = data["approved"];
                    if (approved)
                    {
                        A3_LegionProxy.getInstance().SendGetInfo();
                    }
                    a3_dartproxy.getInstance().sendDartGo();//查看军团镖车信息
                    dispatchEvent(GameEvent.Create(EVENT_APPLYSUCCESSFUL, this, data));
                    break;
                case 35:
                    A3_LegionProxy.getInstance().SendGetMember();
                    int cid = data["cid"];
                    int clanc = data["clanc"];
                    int oldclanc = data["oldclanc"];
                    string name1 = data["name"];
                    string dic = string.Empty;
                    if ((uint)cid == PlayerModel.getInstance().cid)
                    {
                        name1 = ContMgr.getCont("u");
                    }
                    if (oldclanc > clanc)
                        dic = ContMgr.getCont("up");
                    else
                        dic = ContMgr.getCont("down");

                    flytxt.instance.fly(name1 + dic + A3_LegionModel.getInstance().GetClancToName(clanc));
                    break;
                case 36:
                    A3_LegionProxy.getInstance().SendGetInfo();
                    if (a3_buff.instance != null)
                    {
                        a3_buff.instance.Quited();
                    }
                    break;
                case EVENT_GETDIN://37
                    flytxt.instance.fly(ContMgr.getCont("Legionadd_ok"));
                    a3_dartproxy.getInstance().sendDartGo();//查看军团镖车信息
                    dispatchEvent(GameEvent.Create(A3_LegionProxy.EVENT_CREATE, this, data));
                    //a3_herohead.instance.isclear = true;
                    //a3_herohead.instance.legion_bf = true;
                    //if (a3_herohead.instance != null)
                    //    a3_herohead.instance.refresBuff();
                    break;
                case EVENT_FINDNAME://27模糊搜索
                    if (data["info"].Count<=0)
                    {
                        flytxt.instance.fly(ContMgr.getCont("nofinname"));
                    }
                    else
                    {
                        A3_LegionModel.getInstance().finfname.Clear();
                        A3_LegionModel.getInstance().finfname = new List<A3_LegionData>();
                        foreach (var v in data["info"]._arr)
                        {
                            A3_LegionData d = new A3_LegionData();
                            d.id = v["id"];
                            d.clname = v["clname"];
                            d.combpt = v["combpt"];
                            d.lvl = v["lvl"];
                            d.name = v["name"];
                            d.plycnt = v["plycnt"];
                            d.direct_join = v["direct_join"];
                            d.huoyue = v["last_active"];
                            A3_LegionModel.getInstance().finfname.Add(d);
                        }
                        if (a3_legion.Instance != null)
                        {
                            a3_legion.Instance.addpanels.SetActive(false);
                           a3_legion.Instance.Findnames();
                        }

                    }
                    break;

                case EVENT_CHANGE_NAME:

                    A3_LegionModel.getInstance().myLegion.clname = data["clname"];

                    dispatchEvent(GameEvent.Create(EVENT_CHANGE_NAME, this, data));

                    break;
                case EVENT_BUILD:
                    A3_LegionModel.getInstance().build_awd[data["type"]] = data["type"];
                    if (a3_legion_build.instance)
                        a3_legion_build.instance.RefresAward();
                    break;

                default:

                    if(res < 0)
                         Globle.err_output(res);
                    break;
            }
        }
    }
}
