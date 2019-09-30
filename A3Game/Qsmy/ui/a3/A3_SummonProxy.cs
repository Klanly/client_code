using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;
using System;
using System.Collections;

namespace MuGame
{
    internal class A3_SummonProxy : BaseProxy<A3_SummonProxy>
    {
        public static uint EVENT_LOADALL = 0;				//读所有
        public static uint EVENT_SHOWIDENTIFYANSWER = 1;	//鉴定操作结果
        public static uint EVENT_PUTSUMMONINBAG = 51;		//收入背包结果
        public static uint EVENT_USESUMMONFROMBAG = 52;		//从背包使用结果
		public static uint EVENT_UPDATE = 111;				//测试更新界面
        public static uint EVENT_STUDY = 8;                 //学习结果
		public static uint EVENT_FEEDEXP = 31;				//喂食经验
		public static uint EVENT_FEEDSM = 38;				//喂食寿命
        public static uint EVENT_CHUZHAN = 33;               //出战
        public static uint EVENT_XIUXI = 34;                   //休息
        public static uint EVENT_XUEXI = 35;                //学习
        public static uint EVENT_FORGET = 36;               //遗忘
        public static uint EVENT_SUMINFO = 37;                // 查看召唤兽信息
        public static uint EVENT_FENJIEINFO = 12;             //分解预览结果
        public static uint EVENT_FENJIERES = 13;              //分解结果
        public static uint EVENT_XILIAN = 2;                  //洗练
        public static uint EVENT_SAVE = 3;                     //保存洗练结果
        public static uint EVENT_SKILLUP = 16;               //技能升级
        public static uint EVENT_RONGHE = 15;                  //融合
        public static uint EVENT_INSUMMON = 7;                  //转移至召唤兽列表
        public static uint EVENT_SHOUHUN = 4;                   //兽魂
        public static uint EVENT_TUNSHI = 14;                  //吞噬

        public static uint EVENT_REFLIANXIE = 19;         //刷新连携
        public static uint EVENT_LINK = 20;             //连携数组

        public static uint EVENT_INTEGRATION = 32;			//融合
        

        public A3_SummonProxy()
        {
            //S2C Events REG
            addProxyListener(PKG_NAME.S2C_SUMMON_OPERATION_RES, SummonOP_new);
        }

        #region Public Function

        public void sendLoadSummons()
        {
            Variant msg = new Variant();
            msg["op"] = 0;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }
        #region
        public void getsummonInfo(uint id)
        {
            Variant msg = new Variant();
            msg["tp"] = 9;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }


        public void sendIdentifySummons(List<uint> idlist)
        {
            Variant msg = new Variant();
            Variant ids = new Variant();
            foreach (uint v in idlist)
            {
                Variant id = new Variant();
                id["id"] = v;
                int i = (int)v;
                ids._arr.Add(i);
            }
            msg["ids"] = ids;
            msg["tp"] = 1;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendZHSummon(uint tpid)
        {
            Variant msg = new Variant();
            msg["tp"] = 1;
            msg["tpid"] = tpid;
            if (A3_SummonModel.getInstance().GetSummons().Count >= 50)
            {
                flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_bagmax"));
            }
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
            debug.Log(msg.dump());
        }

        public void sendOPSkill(int op, int summonid, int skillid,int index)
        {
            Variant msg = new Variant();
            msg["tp"] = 4;
            msg["op"] = op;//1遗忘、2学习
            msg["id"] = summonid;
            msg["skill_id"] = skillid;
            msg["index"] = index;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
            debug.Log(msg.dump());
        }

        public void sendPutSummonInBag(int id)
        {
            Variant msg = new Variant();
            msg["tp"] = 5;
            msg["op"] = 1;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendUseSummonFromBag(int id)
        {
            Variant msg = new Variant();
            msg["tp"] = 5;
            msg["op"] = 2;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

		public void sendFeedExp(int summonid, int feeditemid,int num) {
			Variant msg = new Variant();
			msg["tp"] = 3;
			msg["op"] = 1;
			msg["id"] = summonid;
			msg["feed_id"] = feeditemid;
            msg["num"] = num;
            debug.Log("召唤兽经验" + msg.dump());
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
		}

		public void sendFeedSM(int summonid, int feeditemid,int num) {
			Variant msg = new Variant();
			msg["tp"] = 3;
			msg["op"] = 2;
			msg["id"] = summonid;
			msg["feed_id"] = feeditemid;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
		}

		public void sendIntegration(int mainid, int otherid) {
			Variant msg = new Variant();
			msg["tp"] = 2;
			msg["id"] = mainid;
			msg["blend_id"] = otherid;
			sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
		}

		public void sendGoAttack(int summonid) {
			Variant msg = new Variant();
			msg["tp"] = 6;
			msg["op"] = 1;
			msg["id"] = summonid;
			sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
		}

		public void sendGoBack(int summonid) {
			Variant msg = new Variant();
			msg["tp"] = 6;
			msg["op"] = 2;
			msg["id"] = summonid;
			sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
		}



        #endregion 


        public void sendGetsummon(uint tpid,uint talent,uint gettype,List<uint> sums = null) {
            Variant msg = new Variant();
            msg["op"] = 1;
            msg["tpid"] = tpid;
            msg["talent"] = talent;
            msg["get_type"] = gettype;
            if (gettype == 1) {
                msg["summon_cost"] = new Variant();
                for (int i = 0; i < sums.Count; i++)
                {
                    
                    msg["summon_cost"].pushBack(sums[i]);
                }
            }
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
            //Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendUseSummon(uint id) {
            Variant msg = new Variant();
            msg["op"] = 7;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
          //  Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendgetinfo(uint id)
        { 
            Variant msg = new Variant();
            msg["op"] = 8;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendChuzhan(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 9;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendZhaohui()
        {
            Variant msg = new Variant();
            msg["op"] = 10;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendExp(uint id,uint tpid,uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 5;
            msg["id"] = id;
            msg["feed_id"] = tpid;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendSM(uint id, uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 6;
            msg["id"] = id;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
          //  Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendFenjie_look(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 12;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendFenjie(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 13;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
          //  Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendXilian(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
         //   Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendXilian_save(uint id)
        {
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendUpskill(uint id,uint skillid)
        {
            Variant msg = new Variant();
            msg["op"] = 16;
            msg["id"] = id;
            msg["skill_id"] = skillid;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
          //  Debug.LogError("召唤兽" + msg.dump());
        }

        public void sendRonghe(uint Zid,uint Fid)
        {
            Variant msg = new Variant();
            msg["op"] = 15;
            msg["id"] = Zid;
            msg["blend_id"] = Fid;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
           // Debug.LogError("召唤兽" + msg.dump());
        }
        public void sendshouhun(uint id,uint type ,uint num)
        {
            Variant msg = new Variant();
            msg["op"] = 4;
            msg["id"] = id;
            msg["soul_type"] = type;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendtunshi(uint Zid,uint Fid) {
            Variant msg = new Variant();
            msg["op"] = 14;
            msg["id"] = Zid;
            msg["blend_id"] = Fid;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendlianxie( uint Zid) {
            Variant msg = new Variant();
            msg["op"] = 19;
            msg["id"] = Zid;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendaddlx(uint Zid)
        {
            Variant msg = new Variant();
            msg["op"] = 17;
            msg["id"] = Zid;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendlocklx(uint Zid , uint id , bool stage) {
            Variant msg = new Variant();
            msg["op"] = 18;
            msg["id"] = Zid;
            msg["index"] = id;
            msg["lock_state"] = stage;
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
        }

        public void sendlink(List<uint> id) {
            Variant msg = new Variant();
            msg["op"] = 20;
            if (id.Count > 0)
            {
                msg["list_ids"] = new Variant();
                for (int i = 0; i < id.Count; i++)
                {
                    msg["list_ids"].pushBack(id[i]);
                }
            }
            sendRPC(PKG_NAME.S2C_SUMMON_OPERATION_RES, msg);
            Debug.LogError("召唤兽信息" + msg.dump());
        }

        public bool getNewSum = false;
        #endregion Public Function

        #region Private Function

        private void SummonOP_new(Variant data) {
            Debug.LogError("召唤兽信息" + data.dump());
            int tp = -1;
            if (data.ContainsKey("res"))
            {
                tp = data["res"];
                if (tp < 0)
                {
                    Globle.err_output(tp);
                    return;
                }
            }
            uint summon_id = 0;
            Variant info = new Variant();
            switch (tp)
            {
                case 0: //读取召唤兽列表
                    info = data["summons"];
                    if (info != null)
                        foreach (Variant item in info._arr)
                        {
                            A3_SummonModel.getInstance().AddSummon(item);
                        }
                    if (data.ContainsKey("summon_on"))
                    {
                        Variant so = data["summon_on"];
                        A3_SummonModel.getInstance().nowShowAttackID = so["id"];
                        A3_SummonModel.getInstance().nowShowAttackModel = so["att_type"];
                        A3_SummonModel.getInstance().lastSummonID = so[ "id" ];
                }
                    if (data.ContainsKey("link_list")) {
                        A3_SummonModel.getInstance().link_list.Clear();
                        foreach (Variant item in data["link_list"]._arr)
                        {
                            A3_SummonModel.getInstance().link_list.Add(item);
                        }
                    }

                    break;
                case 1: // 获得新的召唤兽
                    getNewSum = true;
                    dispatchEvent(GameEvent.Create(EVENT_SHOWIDENTIFYANSWER, this, data));
                    break;
                case 2: // 资质洗练
                    if (data.ContainsKey("reset_info"))
                    {
                        Variant reset_info = data["reset_info"];
                        if (data.ContainsKey("summon_id"))
                        {
                            summon_id = data["summon_id"];
                            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
                            {
                                var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
                                sm.summondata.haveReset = true;
                                sm.summondata.resetluck = reset_info["luckly"];
                                sm.summondata.resetatt = reset_info["att"];
                                sm.summondata.resetdef = reset_info["def"];
                                sm.summondata.resetagi = reset_info["agi"];
                                sm.summondata.resetcon = reset_info["con"];
                                A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
                                A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
                            }
                        }
                        dispatchEvent(GameEvent.Create(EVENT_XILIAN, this, data));
                    }

                    break;
                case 3: // 资质替换
                    if (data.ContainsKey("summon"))
                    {
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                        dispatchEvent(GameEvent.Create(EVENT_SAVE, this, data));
                    }
                    break;
                case 4: // 兽魂强化
                    if (data.ContainsKey("summon_id"))
                    {
                        summon_id = data["summon_id"];
                        if (data.ContainsKey("soul_info"))
                        {
                            int type = data["soul_info"]["soul_type"];
                            int lvl = data["soul_info"]["soul_lvl"];
                            int exp = data["soul_info"]["soul_exp"];
                            if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
                            {
                                var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
                                if (sm.summondata.shouhun.ContainsKey(type))
                                {
                                    summonshouhun sh = new summonshouhun();
                                    sh.soul_type = type;
                                    sh.lvl = lvl;
                                    sh.exp = exp;
                                    sm.summondata.shouhun[type] = sh;
                                }
                            }
                        }
                    }
                    else if (data.ContainsKey("summon"))
                    {
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_SHOUHUN, this, data));
                    break;
                case 5: // 喂食经验
                    if (data.ContainsKey("summon_id"))
                    {
                        summon_id = data["summon_id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
                        {
                            if (data.ContainsKey("add_exp"))
                            {
                                int addexp = data["add_exp"];
                                var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
                                sm.summondata.currentexp = addexp;
                                A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
                                A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
                            }
                        }
                    }
                    else if (data.ContainsKey("summon"))
                    {
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_FEEDEXP, this, data));
                    break;
                case 6: // 喂食寿命
                    if (data.ContainsKey("summon_id"))
                    {
                        summon_id = data["summon_id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
                        {
                            if (data.ContainsKey("summon_life"))
                            {
                                int lifespan = data["summon_life"];
                                var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
                                sm.summondata.lifespan = lifespan;
                                A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
                                A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
                            }
                        }
                        dispatchEvent(GameEvent.Create(EVENT_FEEDSM, this, data));
                        if (data["summon_id"] == A3_SummonModel.getInstance().nowShowAttackID || data["summon_id"] == A3_SummonModel.getInstance().lastatkID)
                        {
                            if (a3_herohead.instance)
                            {
                                a3_herohead.instance.refresh_sumbar();
                            }
                        }
                    }
                    break;
                case 7: // 转移到召唤兽列表
                    info = data["summon"];
                    A3_SummonModel.getInstance().AddSummon(info);
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_chenggongzh"));
                    dispatchEvent(GameEvent.Create(EVENT_INSUMMON, this, data));
                    break;
                case 8: // 查询召唤兽信息
                    dispatchEvent(GameEvent.Create(EVENT_SUMINFO, this, data));
                    break;
                case 9: // 出战召唤兽
                    summon_id = data["summon_id"];
                    A3_SummonModel.getInstance().lastSummonID = summon_id;
                    A3_SummonModel.getInstance().nowShowAttackID = summon_id;
                    if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
                    {
                        var sd = A3_SummonModel.getInstance().GetSummons()[summon_id];
                        sd.summondata.status = 1;
                    }
                    flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_togo"));
                    if (a3_herohead.instance)
                    {
                        a3_herohead.instance.refresh_sumbar();
                    }
                    dispatchEvent(GameEvent.Create(EVENT_CHUZHAN, this, data));
                    break;
                case 10: // 召回召唤兽

                    //休息
                    uint summon_id_10 = data["summon_id"];
                    A3_SummonModel.getInstance().nowShowAttackID = 0;
                    if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id_10)) {
                        var sd = A3_SummonModel.getInstance().GetSummons()[summon_id_10];
                        sd.summondata.status = 0;
                    }
                    if (data.ContainsKey("summon_life"))
                    {
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id_10))
                        {
                            var sm = A3_SummonModel.getInstance().GetSummons()[summon_id_10];
                            int oldsm = sm.summondata.lifespan;
                            sm.summondata.lifespan = data["summon_life"];
                            A3_SummonModel.getInstance().GetSummons().Remove(summon_id_10);
                            A3_SummonModel.getInstance().GetSummons()[summon_id_10] = sm;
                            //A3_SummonModel.getInstance().SortSummon();
                            if (oldsm > data["summon_life"] && data.ContainsKey("die_timelist"))
                            {
                                //召唤兽死亡 
                                A3_SummonModel.getInstance().addSumCD((int)summon_id_10, data["die_timelist"][0]._int64 - muNetCleint.instance.CurServerTimeStamp);
                                debug.Log("time" + (data["die_timelist"][0]._int - muNetCleint.instance.CurServerTimeStamp));
                                if (a3_herohead.instance)
                                {
                                    A3_SummonModel.getInstance().lastatkID = summon_id_10;
                                    a3_herohead.instance.refresh_sumbar();
                                    a3_herohead.instance.refresh_sumHp(0, 1);//召唤兽出视野同步不到血量，这里强行降血条设为0
                                }
                            }
                        }
                    }
                    else {
                        //召回
                        if (a3_herohead.instance)
                        {
                            A3_SummonModel.getInstance().lastatkID = 0;//清除出战召唤兽id缓存
                            a3_herohead.instance.refresh_sumbar();
                            a3_herohead.instance.refresh_sumHp(0, 1);//召唤兽出视野同步不到血量，这里强行降血条设为0
                        }
                    }
                    if (!data.ContainsKey("summon_life") || data["summon_life"] > 0)
                    {
                        flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_torest"));
                    }
                    dispatchEvent(GameEvent.Create(EVENT_XIUXI, this, data));
                    break;
                case 11: //设置召唤兽三种攻击模式
                    break;
                case 12: //查询分解信息

                    dispatchEvent(GameEvent.Create(EVENT_FENJIEINFO, this, data));
                    break;
                case 13://分解结果
                    if (data.ContainsKey("rmv_id"))
                    {
                        uint rmvid = data["rmv_id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(rmvid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(rmvid);
                        }
                    }

                    dispatchEvent(GameEvent.Create(EVENT_FENJIERES, this, data));
                    break;
                case 14:
                    if (data.ContainsKey("rmv_id"))
                    {
                        uint rmvid = data["rmv_id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(rmvid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(rmvid);
                        }
                    }
                    if (data.ContainsKey("summon"))
                    {
                        flytxt.instance.fly(ContMgr.getCont("TunshiChengGong"));
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                    }
                    else {
                        flytxt.instance.fly(ContMgr.getCont("TunshiShiBai"));
                    }
                    dispatchEvent(GameEvent.Create(EVENT_TUNSHI, this, data));
                    break;
                case 15://融合
                    if (data.ContainsKey ("rmv_id"))
                    {
                        uint rmvid = data["rmv_id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(rmvid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(rmvid);
                        }
                    }
                    if (data.ContainsKey("summon"))
                    {
                        flytxt.instance.fly(ContMgr.getCont("RongheChengGong"));
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                    }
                    else {
                        flytxt.instance.fly(ContMgr.getCont("RongheShiBai"));
                    }
                    dispatchEvent(GameEvent.Create(EVENT_RONGHE, this, data));
                    break;
                case 16://技能升级
                    if (data.ContainsKey("summon"))
                    { 
                        info = data["summon"];
                        uint uid = info["id"];
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
                        {
                            A3_SummonModel.getInstance().GetSummons().Remove(uid);
                        }
                        A3_SummonModel.getInstance().AddSummon(info);
                        dispatchEvent(GameEvent.Create(EVENT_SKILLUP, this, data));
                    }
                    break;
                case 18:
                    if (data.ContainsKey("summon_id"))
                    {
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(data["summon_id"]))
                        {
                            var sd = A3_SummonModel.getInstance().GetSummons()[data["summon_id"]];
                            if (data.ContainsKey("link_ply"))
                            {
                                if (sd.summondata.linkdata == null) { sd.summondata.linkdata = new Dictionary<int, link_data>(); }
                                sd.summondata.linkdata.Clear();
                                Variant link = data["link_ply"];
                                for (int i = 0; i < link.Count; i++)
                                {
                                    link_data li = new link_data();
                                    li.type = link[i]["att_type"];
                                    li.per = link[i]["att_per"];
                                    li.lock_state = link[i]["lock_state"];
                                    sd.summondata.linkdata[i] = li;
                                }
                            }
                            dispatchEvent(GameEvent.Create(EVENT_REFLIANXIE, this, data));
                        }
                    }
                    break;
                case 17:
                    if (data.ContainsKey("summon_id"))
                    {
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(data["summon_id"]))
                        {
                            var sd = A3_SummonModel.getInstance().GetSummons()[data["summon_id"]];
                            if (data.ContainsKey("link_ply"))
                            {
                                if (sd.summondata.linkdata == null) { sd.summondata.linkdata = new Dictionary<int, link_data>(); }
                                sd.summondata.linkdata.Clear();
                                Variant link = data["link_ply"];
                                for (int i = 0; i < link.Count; i++)
                                {
                                    link_data li = new link_data();
                                    li.type = link[i]["att_type"];
                                    li.per = link[i]["att_per"];
                                    li.lock_state = link[i]["lock_state"];
                                    sd.summondata.linkdata[i] = li;
                                }
                            }
                            dispatchEvent(GameEvent.Create(EVENT_REFLIANXIE, this, data));
                        }
                    }
                    break;
                case 19:
                    if (data.ContainsKey("summon_id"))
                    {
                        if (A3_SummonModel.getInstance().GetSummons().ContainsKey(data["summon_id"]))
                        {
                            var sd = A3_SummonModel.getInstance().GetSummons()[data["summon_id"]];
                            if (data.ContainsKey("link_ply")) {
                                float Combpt = 0;
                                if (sd.summondata .linkdata == null) { sd.summondata.linkdata = new Dictionary<int, link_data>(); }
                                sd.summondata.linkdata.Clear();
                                Variant link = data["link_ply"];
                                for (int i = 0; i < link.Count; i++)
                                {
                                    link_data li = new link_data();
                                    li.type = link[i]["att_type"];
                                    li.per = link[i]["att_per"];
                                    li.lock_state = link[i]["lock_state"];
                                    sd.summondata.linkdata[i] = li;
                                    SXML x = XMLMgr.instance.GetSXML("calculate.combpt", "att_id==" + li.type);
                                    if (x != null)
                                    {
                                        float attvalue = (int)Math.Ceiling((A3_SummonModel.getInstance().getAttValue(sd.summondata, li.type) * ((float)li.per / 100.00f)));
                                        Combpt += (attvalue * x.getFloat("sm_per")) / 10000;
                                    }
                                }
                                sd.summondata.linkCombpt = (int)Combpt;
                            }
                            dispatchEvent(GameEvent.Create(EVENT_REFLIANXIE, this, data));
                        }
                    }
                    break;
                case 20:
                    if (data.ContainsKey("link_list"))
                    {
                        A3_SummonModel.getInstance().link_list.Clear();
                        foreach (Variant item in data["link_list"]._arr)
                        {
                            A3_SummonModel.getInstance().link_list.Add(item);
                        }
                        dispatchEvent(GameEvent.Create(EVENT_LINK, this, data));
                    }
                    break;
            }

            List<uint> removeid = new List<uint>();
            foreach (uint id in A3_SummonModel .getInstance ().link_list) {
                if ( !A3_SummonModel .getInstance ().GetSummons ().ContainsKey (id)) {
                    removeid.Add(id);
                }
            }

            foreach (uint remove in removeid) {
                if (A3_SummonModel.getInstance().link_list.Contains (remove)) {
                    A3_SummonModel.getInstance().link_list.Remove(remove);
                }
            }

        }

   //     private void SummonOP(Variant data)
   //     {

   //         debug.Log("召唤兽信息" + data.dump());
            
   //         int tp = -1;
   //         Variant info = new Variant();
			//uint summon_id = 0;
   //         if (data.ContainsKey("tp"))
   //         {
   //             tp = data["tp"];
   //             if (tp < 0)
   //             {
   //                 Globle.err_output(tp);
   //                 return;
   //             }
   //         }
   //         //<更新代码>
   //         if (data.ContainsKey("summon"))
   //         {
                
   //            // dispatchEvent(GameEvent.Create(EVENT_UPDATE, this, data));
   //         }
   //         //</更新代码>
   //         switch (tp)
   //         {
   //             case 0:
   //                 //读取召唤兽列表
   //                 info = data["summons"];
			//		if(info!=null)
			//			foreach (Variant item in info._arr)
			//			{
			//				A3_SummonModel.getInstance().AddSummon(item);
			//			}
   //                 if (data.ContainsKey("summon_on"))
   //                 {
   //                     Variant so = data["summon_on"];
   //                     A3_SummonModel.getInstance().nowShowAttackID = so["id"];
   //                     A3_SummonModel.getInstance().nowShowAttackModel = so["att_type"];
   //                 }
   //                 //A3_SummonModel.getInstance().SortSummon();
   //                 //dispatchEvent(GameEvent.Create(EVENT_LOADALL, this, data));
   //                 //dispatchEvent(GameEvent.Create(EVENT_UPDATE, this, data));
   //                 break;
   //             case 1:
   //                 //鉴定结果
   //                 if (data.ContainsKey ("summons")) {
   //                     info = data["summons"];
   //                     foreach (Variant item in info._arr)
   //                     {
   //                         A3_SummonModel.getInstance().AddSummon(item);
   //                     }
   //                     //A3_SummonModel.getInstance().SortSummon();
   //                 }
   //                 dispatchEvent(GameEvent.Create(EVENT_SHOWIDENTIFYANSWER, this, data));
   //                 break;
			//	case 2:
			//		//放回背包
			//		int id = data["summon_id"];
			//		A3_SummonModel.getInstance().RemoveSummon(id);
			//		dispatchEvent(GameEvent.Create(EVENT_PUTSUMMONINBAG, this, data));
			//		break;
   //             case 3:
   //                 //从背包中使用
   //                 info = data["summon"];
   //                 A3_SummonModel.getInstance().AddSummon(info);
   //                 //A3_SummonModel.getInstance().SortSummon();
   //                 dispatchEvent(GameEvent.Create(EVENT_USESUMMONFROMBAG, this, data));
   //                 break;
			//	case 4:
			//		//融合
			//		summon_id = data["summon_id"];
			//		if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id)) {
			//			A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
			//		}
   //                 if (data.ContainsKey("summon"))
   //                 {
   //                     info = data["summon"];
   //                     uint uid = info["id"];
   //                     if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
   //                     {
   //                         A3_SummonModel.getInstance().GetSummons().Remove(uid);
   //                     }
   //                     A3_SummonModel.getInstance().AddSummon(info);
   //                    // A3_SummonModel.getInstance().SortSummon();
   //                 }
			//		dispatchEvent(GameEvent.Create(EVENT_INTEGRATION, this, data));
			//		break;
			//	case 5:
   //                 //喂养
   //                 if (data.ContainsKey("summon_id"))
   //                 {
   //                     summon_id = data["summon_id"];
   //                     if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
   //                     {
   //                         if (data.ContainsKey("add_exp"))
   //                         {
   //                             int addexp = data["add_exp"];
   //                             var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
   //                             sm.summondata.currentexp = addexp;
   //                             A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
   //                             A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
   //                             //A3_SummonModel.getInstance().SortSummon();
   //                         }
   //                     }
   //                 }
   //                 else if (data.ContainsKey("summon"))
   //                 {
   //                     info = data["summon"];
   //                     uint uid = info["id"];
   //                     if (A3_SummonModel.getInstance().GetSummons().ContainsKey(uid))
   //                     {
   //                         A3_SummonModel.getInstance().GetSummons().Remove(uid);
   //                     }
   //                     A3_SummonModel.getInstance().AddSummon(info);
   //                 }
   //                 dispatchEvent(GameEvent.Create(EVENT_FEEDEXP, this, data));
			//		break;
   //             case 6:
   //                 if (data.ContainsKey("summon_id"))
   //                 {
   //                     summon_id = data["summon_id"];
   //                     if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
   //                     {
   //                         if (data.ContainsKey("summon_life"))
   //                         {
   //                             int lifespan = data["summon_life"];
   //                             var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
   //                             sm.summondata.lifespan = lifespan;
   //                             A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
   //                             A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
   //                         }
   //                     }
   //                     dispatchEvent(GameEvent.Create(EVENT_FEEDSM, this, data));
   //                     if (a3_summon.instan)
   //                     {
   //                         a3_summon.instan.Event_UIFeedSMClicked(null);
   //                     }
   //                     if (data["summon_id"] == A3_SummonModel.getInstance().nowShowAttackID || data["summon_id"] == A3_SummonModel.getInstance().lastatkID)
   //                     {
   //                         if (a3_herohead.instance)
   //                         {
   //                             a3_herohead.instance.refresh_sumbar();
   //                         }
   //                     }
   //                 }
   //                 break;
   //             case 7:
   //                 //遗忘
   //                 if (data.ContainsKey("summon_id")) summon_id = data["summon_id"];
   //                 if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
   //                 {
   //                     int skid = data["skill_id"];
   //                     int sksKey = data["index"];
   //                     var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
   //                     if (sm.summondata.skills.ContainsKey(sksKey))
   //                     {
   //                         sm.summondata.skills.Remove(sksKey);
   //                     }
   //                     //++++++else sm.summondata.skills[sksKey] = skid;
   //                     A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
   //                     A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
   //                    // A3_SummonModel.getInstance().SortSummon();
   //                 }
   //                 dispatchEvent(GameEvent.Create(EVENT_FORGET, this, data));
   //                 break;
   //             case 8:
   //                 //学习
   //                 if (data["skill_id"] == 0)
   //                 {
   //                     flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_studybad"));
   //                     if (a3_summon.instan)
   //                         a3_summon.instan.ShowSkillBooks();
   //                     return;
   //                 }
   //                 if (data.ContainsKey("summon_id")) summon_id = data["summon_id"];
   //                 if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
   //                 {
   //                     int skid = data["skill_id"];
   //                     int sksKey = data["index"];
   //                     var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
   //                     if (sm.summondata.skills.ContainsKey(sksKey))
   //                     {
   //                         sm.summondata.skills.Remove(sksKey);
   //                     }
   //                     //++++++++else sm.summondata.skills[sksKey] = skid;
   //                     A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
   //                     A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
   //                    // A3_SummonModel.getInstance().SortSummon();
   //                 }
   //                 dispatchEvent(GameEvent.Create(EVENT_XUEXI, this, data));
   //                 break;
   //             case 9:
			//		//出战
			//		summon_id = data["summon_id"];
			//		A3_SummonModel.getInstance().nowShowAttackID = summon_id;
			//		if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id)) {
			//			var sd = A3_SummonModel.getInstance().GetSummons()[summon_id];
			//			sd.summondata.status = 1;
			//		}
			//		flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_togo"));
   //                 if (a3_herohead.instance)
   //                 {
   //                     a3_herohead.instance.refresh_sumbar();
   //                 }
   //                 dispatchEvent(GameEvent.Create(EVENT_CHUZHAN, this, data));
			//		break;
			//	case 10:
			//		//休息
			//		summon_id = data["summon_id"];
			//		A3_SummonModel.getInstance().nowShowAttackID = 0;
			//		if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id)) {
			//			var sd = A3_SummonModel.getInstance().GetSummons()[summon_id];
			//			sd.summondata.status = 0;
			//		}
   //                 if (data.ContainsKey("summon_life"))
   //                 {
   //                     if (A3_SummonModel.getInstance().GetSummons().ContainsKey(summon_id))
   //                     {
   //                         var sm = A3_SummonModel.getInstance().GetSummons()[summon_id];
   //                         int oldsm = sm.summondata.lifespan;
   //                         sm.summondata.lifespan = data["summon_life"];
   //                         A3_SummonModel.getInstance().GetSummons().Remove(summon_id);
   //                         A3_SummonModel.getInstance().GetSummons()[summon_id] = sm;
   //                         //A3_SummonModel.getInstance().SortSummon();
   //                         if (oldsm > data["summon_life"] && data.ContainsKey("die_timelist"))
   //                         {
   //                             //召唤兽死亡 
   //                             //A3_SummonModel.getInstance().sum_doCd(10);
   //                             A3_SummonModel.getInstance().addSumCD((int)summon_id, data["die_timelist"][0]._int - muNetCleint.instance.CurServerTimeStamp);
   //                             debug.Log("time" + (data["die_timelist"][0]._int - muNetCleint.instance.CurServerTimeStamp));
   //                             if (a3_herohead.instance)
   //                             {
   //                                 A3_SummonModel.getInstance().lastatkID = summon_id;
   //                                 a3_herohead.instance.refresh_sumbar();
   //                                 a3_herohead.instance.refresh_sumHp(0, 1);//召唤兽出视野同步不到血量，这里强行降血条设为0
   //                             }
   //                         }
   //                     }
   //                 }
   //                 else {
   //                     //召回
   //                     if (a3_herohead.instance)
   //                     {
   //                         A3_SummonModel.getInstance().lastatkID = 0;//清除出战召唤兽id缓存
   //                         a3_herohead.instance.refresh_sumbar();
   //                         a3_herohead.instance.refresh_sumHp(0, 1);//召唤兽出视野同步不到血量，这里强行降血条设为0
   //                     }
   //                 }
   //                 if (!data.ContainsKey("summon_life") || data["summon_life"] > 0)
   //                 {
   //                     flytxt.instance.fly(ContMgr.getCont("A3_SummonProxy_torest"));
   //                 }
			//		dispatchEvent(GameEvent.Create(EVENT_XIUXI, this, data));
			//		break;
   //             case 11:
   //                 dispatchEvent(GameEvent.Create(EVENT_SUMINFO, this, data));
   //                 break;
			//	default:
			//		break;
   //         }

   //     }

        #endregion Private Function
    }
}