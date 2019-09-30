using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class lgSelfPlayer : LGAvatarGameInst
    {
        static public lgSelfPlayer instance;

        public int[] m_nDress_PartID = { 0, 0, 0, 0 };

        public float m_fCameraNearXZ = 0.517f;
        public float m_fCameraNearH = 3.6f;
        public bool m_bMounting = false;

        public lgSelfPlayer(gameManager m)
            : base(m)
        {
            //moveproxy = MoveProxy.getInstance();
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new lgSelfPlayer(m as gameManager);
        }

        override public int roleType
        {
            get { return ROLE_TYPE_USER; }
        }

        private uint _castSkilid = 0;
        private bool _smartSkil = false;
        private bool _singleAttackFlag = false;
        private bool _attackFlag = false;
        private int _nextSkillTurn = 0;

        override public void onInit()
        {
            instance = this;
            // this.g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_PLAY_MOV, onJoystickMove);
            // this.g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_PLAY_STOP, onJoystickEnd);

            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_ENTER_GAME,
                onJoinWorld
            );

            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_MAP,
                GAME_EVENT.MAP_CHANGE,
                onChangeMap
            );

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES,
            //    onChangeMapReadyRes
            //);
            this.g_mgr.g_netM.addEventListener(
                PKG_NAME.S2C_DETAIL_INFO_CHANGE,
                onDetailInfoChangeRes
             );

            this.g_mgr.g_netM.addEventListener(
               PKG_NAME.S2C_SELF_ATT_CHANGE,
               onSelfAttChange
            );


            this.g_mgr.g_netM.addEventListener(
                PKG_NAME.S2C_MODE_EXP,
                onModExpRes
             );
        }
        override public string processName
        {
            get
            {
                return "lgSelfPlayer";
            }
            set
            {
                _processName = value;
            }
        }

        public void RefreshPlayerAvatar()
        {
            m_nrefreshWpRibbon = 6;

            grAvatar.m_char.removeAvatar("RightHand");
            grAvatar.m_char.removeAvatar("body");
            grAvatar.m_char.removeAvatar("hair");
            grAvatar.m_char.removeAvatar("Bip01 Spine1");

            string sex_id = data["sex"];
            for (int i = 0; i < m_nDress_PartID.Length; i++)
            {
                int p_id = m_nDress_PartID[i];
                if (p_id > 0)
                {
                    if (GraphManager.singleton.getAvatarConf(sex_id, p_id.ToString() + sex_id) != null)
                        grAvatar.m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, p_id.ToString() + sex_id));
                    else if (i == 0)
                        grAvatar.m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, "1000" + sex_id));
                }
                else
                {
                    if (0 == i)
                    {
                        grAvatar.m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, "1000" + sex_id));
                    }
                }
            }
        }

        public int nextSkillTurn
        {
            get
            {
                return _nextSkillTurn;
            }
            set
            {
                _nextSkillTurn = value;
            }
        }


        public delegate bool boolDelegate(String id);
        public boolDelegate needAutoNextAttackHanle;
        override public bool onAniEnd()
        {
            //if (m_loopFlag) return false;

            String lastAni = m_ani;
            if (IsDie())
            {
                m_visible = false;
                refreshAni();
            }
            else
            {
                if (needAutoNextAttackHanle == null || !needAutoNextAttackHanle(lastAni))
                {
                    if (IsMoving())
                    {
                        setTag(GameConstant.ST_MOVE);
                    }
                    else
                    {
                        setTag(GameConstant.ST_STAND);
                    }

                    return false;
                }
                else
                    return true;
            }
            return false;
        }

        public void startMount(string id)
        {
            if (grAvatar != null)
            {
                m_fCameraNearH = 5.57f;
                m_fCameraNearXZ = 0.95f;
                MouseClickMgr.instance.m_UpdateNearCamNow = true;

                grAvatar.startMount(id);
                m_bMounting = true;
            }
        }

        public void disMount()
        {
            if (grAvatar != null)
            {
                m_fCameraNearH = 3.6f;
                m_fCameraNearXZ = 0.517f;
                MouseClickMgr.instance.m_UpdateNearCamNow = true;

                grAvatar.disMount();
                m_bMounting = false;
            }
        }

        public void on_self_attchange(Variant data)
        {
            //Boolean mpchange = false;
            //if (data.ContainsKey("mpadd"))
            //{
            //    this.modMp(data["mpadd"]);
            //}
            //if (data.ContainsKey("maxmp"))
            //{
            //    int changedMaxMp = data["maxmp"] - data["max_mp"];
            //    if (changedMaxMp != 0)
            //    {
            //        //modMaxMp( data["maxmp"] );
            //        //lguiMain.AddAttShow("maxmp", changedMaxMp);
            //    }
            //}
            //if (data.ContainsKey("camp"))
            //{
            //    data["camp"] = data["camp"];
            //    //同步到其他需要显示阵营变化的地方
            //}
            //if (data.ContainsKey("skcds"))
            //{
            //    //技能CD
            //    //lgInGame.plyController.touch_cd(data.skcds.sktp,data.skcds.cdtm);
            //}
            //if (data.ContainsKey("prepo"))
            //{
            //    //随身仓库开启
            //    (g_mgr.g_gameM as muLGClient).g_generalCT.prepo = data["prepo"];
            //}
            //if (data.ContainsKey("act_v"))
            //{
            //    //行动力变化，当前的行动力
            //    data["act_v"] = data["act_v"];
            //    //TO DO
            //    //同步到需要显示行动力变化的地方
            //}

            //if (data.ContainsKey("carrlvl"))
            //{
            //    data["carrlvl"] = data["carrlvl"];
            //    //TO DO
            //    //同步到需要显示职业等级变化的地方
            //    LGIUITransfer transfer = this.g_mgr.g_uiM.getLGUI(UIName.LGUITransferImpl) as LGIUITransfer;
            //    if (null != transfer)
            //    {
            //        transfer.trans_back();
            //    }
            //    //更新任务
            //    (g_mgr.g_gameM as muLGClient).g_missionCT.PlayerInfoChanged();

            //    LGIUISystemOpen sys = g_mgr.g_uiM.getLGUI(UIName.LGUISystemOpenImpl) as LGIUISystemOpen;
            //    sys.OnCarrlvl(data["carrlvl"]);
            //    LGIUICharacterInfo cha = g_mgr.g_uiM.getLGUI(UIName.UI_MDLG_CHAINFO) as LGIUICharacterInfo;
            //    cha.SelfDetailInfoChange(data, null);
            //}
            //if (data.ContainsKey("ypvip"))
            //{
            //    data["ypvip"] = data["ypvip"];
            //}
        }

        public void updateNetData(Variant data)
        {
            //TO DO..
        }


        public void moveToNpc(int npcid)
        {


            LGAvatarNpc npc = LGNpcs.instance.getNpc(npcid);
            if (npc != null)
                onSelectNpc(npc);
        }

        public void onSelectNpc(LGAvatarNpc npc)
        {
            if (IsDie())
                return;


            npc.faceToAvatar(this);
            //Vec2 v = new Vec2(npc.x-x, npc.y-y);
            //float angle = (float)( v.angleBetween( GameConstant.EFF_DEF_ORI ) );
            //if( npc.x < x )
            //{
            //	angle = -angle;
            //}
            //GameTools.PrintNotice( "angleBetween:" + angle + ", angle:"+ (angle*180/Math.PI ).ToString() +"x["+x+"] y["+y+"] npc.x["+npc.x+"] npc.y["+npc.y+"]"  );


            //addEff( "FxZ006", angle );	
            //return;

            if (Vector3.Distance(npc.pGameobject.transform.position, pGameobject.transform.position) <= GameConstant.OPEN_NPC_DISTANCE)
            {
                //if (Nav != null)
                //    Nav.Stop();
                RotationToPt(npc.x, npc.y);
                _reach_to_npc(npc);
            }
            else
            {
                movePosSingle(
                  npc.x,
                  npc.y,
                  (Variant info) =>
                  {
                      _reach_to_npc(npc);
                  },
                  null,
                 GameConstant.OPEN_NPC_DISTANCE, npc
              );
            }


        }

        public void onSelectOther(LGAvatarOther other)
        {

            //movePosSingle(
            //    other.x,
            //    other.y,
            //    (Variant info) =>
            //    {

            //    },
            //    null,
            //    GameConstant.DEF_ATTACK_RANGE,
            //    other
            //);

        }

        private void _reach_to_npc(LGAvatar npc)
        {
            this.stop();

            int runtype = 0;
            if (runtype == 2)
            {

            }
            else if (runtype == 0)
            {
                if (npc != null)
                {
                    //if (!IsNearbyPlayer(npc))
                    //{
                    //    return;
                    //}

                    MouseClickMgr.instance.onSelectNpc(npc as LGAvatarNpc);
                }
            }
        }



        private void onJoinWorld(GameEvent e)
        {
            initData();
        }

        //override protected void onLvlUpRes(GameEvent e)
        //{
        //    base.onLvlUpRes(e);
        //    Variant info = e.data;
        //    if (info["iid"]._uint != this.getIid())
        //        return;
        //    //on_lvl_up(info);
        //}

        private void onSelfAttChange(GameEvent e)
        {
            selfInfo.onSelfAttChange(e.data);
        }




        private void onDetailInfoChangeRes(GameEvent e)
        {
            Variant info = e.data;
            on_detail_info_change(info);
        }

        private void onModExpRes(GameEvent e)
        {
            Variant info = e.data;
            on_mod_exp(info);
        }

        public void on_mod_exp(Variant info)
        {
            this.data["exp"]._int += info["mod_exp"];
            //this._dispEvent(logicDataEvent.ON_MODE_EXP, {exp:info.mod_exp});
            //lguiMain.setCurExp(this.data["exp"], info["mod_exp"]);
            //lguiMain.AddAttShow("exp", info["mod_exp"]);
            int curexp = info["mod_exp"];
            string addExp = "";
            string expChangeStr = LanguagePack.getLanguageText("LGUIItemImpl", "getExp");
            string expChangeLoseStr = LanguagePack.getLanguageText("LGUIItemImpl", "loseExp");
            if (curexp > 0)
            {
                addExp = DebugTrace.Printf(expChangeStr, curexp.ToString());
            }
            else
            {
                addExp = DebugTrace.Printf(expChangeLoseStr, Math.Abs(curexp).ToString());
            }
            Variant sys_str = new Variant();
            sys_str.pushBack(addExp);
            //lguiMain.systemmsg(sys_str);
            //TO DO more
            //通知更多关心玩家经验值变化的地方
        }

        public void on_lvl_up(Variant info)
        {
            //if(lgInGame.Maxlvl < data.level)
            //{
            //    lgInGame.Maxlvl = data.level
            //}
            //this.data["level"] = info["level"];
            //Variant pinfo = info["pinfo"];
            //this.data["exp"] = pinfo["exp"];
            //if(pinfo.ContainsKey("max_hp"))
            //{
            //    modHp( pinfo["max_hp"] - hp );
            //}
            //if(pinfo.ContainsKey("max_mp"))
            //{
            //    modMp( pinfo["max_mp"] - mp );
            //}
            //if(pinfo.ContainsKey("max_dp"))
            //{
            //    modDp( pinfo["max_dp"] - dp );
            //}
            //modAttPt( pinfo );

            //chaui.SelfDetailInfoChange(info, "all");
            //lguiMain.setCurLevel(this.data);
            //lguiMain.setCurExp(this.data["exp"], info["mod_exp"]);
            //LGIUISystemOpen sysOpen = this.g_mgr.g_uiM.getLGUI(UIName.LGUISystemOpenImpl) as LGIUISystemOpen;
            //if(sysOpen != null)
            //{
            //    sysOpen.OnSelfLevelUp(this.data["level"]);
            //}

            //string expChangeStr = LanguagePack.getLanguageText("LGUIItemImpl", "getExp");
            //string msgStrLvl = LanguagePack.getLanguageText("addpoint","lvlUp");
            //string addExp = DebugTrace.Printf(expChangeStr, info["mod_exp"]._str);
            //Variant exp_v = new Variant();
            //exp_v.pushBack(addExp);
            //exp_v.pushBack(msgStrLvl);

            //lguiMain.systemmsg(exp_v);

            //lguiMain.RefreshRecMisAwardPan();
            //TO DO more
            //通知更多关心玩家等级与经验值变化的地方
            //LGIUINotify notify = this.g_mgr.g_uiM.getLGUI(UIName.LGUINotifyImpl) as LGIUINotify;
            //notify.notifyBackGift(this.data["level"]);
            //notify.notifyFlash(this.data["level"]);

            //LGIUIMultilevel multilevel = this.g_mgr.g_uiM.getLGUI(UIName.LGUIMultilevel) as LGIUIMultilevel;
            //if(multilevel != null)
            //{
            //    multilevel.RefreshDragonInfo();
            //}

            //LGIUIMessageBox msgbox =  this.g_mgr.g_uiM.getLGUI(UIName.LGUIMessageBoxImpl) as LGIUIMessageBox;
            //if(msgbox != null)
            //{
            //    msgbox.OnLevelUp(this.data["level"]);
            //}
            //LGIUIWelfare welfare = this.g_mgr.g_uiM.getLGUI(UIName.UI_MDLG_WELFARE) as LGIUIWelfare;
            //if(welfare != null)
            //{
            //    welfare.lvlUpShowCard();
            //}

            //(this.g_mgr.g_gameM as muLGClient).g_plyfunCT.show_trans_icon();
            //(this.g_mgr.g_gameM as muLGClient).g_plyfunCT.AutoTranfer();
            //LGIUITranlive tranlive = this.g_mgr.g_uiM.getLGUI(UIName.UI_TRANLIVE) as LGIUITranlive;
            //if(tranlive != null)
            //{
            //    tranlive.levelUP();
            //}
            //int reportlvl = genConf.GetCommonConf("reportlvl")._int;
            ////if(lgInGame.IsPlat_BM() && _netData.level == reportlvl)
            ////{
            ////    BridgeUtility.inst.ExternalInterfaceCall( "adv_iframe" );
            ////}
            //lguiMain.AddScreenShake("lvlUpShake");
            //(this.g_mgr.g_gameM as muLGClient).g_missionCT.PlayerLevelChange();


            ////this.lgInGame.trigMgr.DoTrigger( TriggerManager.TRGT_LEVEL, this.data["level"] );
            //(this.g_mgr.g_gameM as muLGClient).g_plyfunCT.GetUplvlInvestInfo();

            //LGIUIPvip pvip = this.g_mgr.g_uiM.getLGUI(UIName.UI_PVIP) as LGIUIPvip;
            //if (pvip != null)
            //{
            //    pvip.RefreshPvipLevelAward();
            //}

            //todo
            //if(lgInGame.IsPlatID_P51())
            //{
            //    planReportLvl();
            //}
        }

        public void SetResetName()
        {

        }
        //--------------------------血量、魔法-----------------------------------


        override protected void onModMp(int add)
        {
            //lguiMain.setmp(this.data["mp"], this.data["max_mp"]);
            //chaui.SelfDetailInfoChange(GameTools.createGroup("mp", this.data["mp"], "max_mp", this.data["max_mp"]), "mp");
        }

        override protected void onModDp(int add)
        {
            //lguiMain.setProtect(this.data["dp"], this.data["max_dp"]);
            //chaui.SelfDetailInfoChange(GameTools.createGroup("dp", this.data["dp"], "max_dp", this.data["max_dp"]), "dp");
        }



        //----------------------------------   角色 当前激活 称号  ---------------------------------------
        private Variant _actAchives = new Variant();
        public Variant GetActAchives()
        {
            return _actAchives;
        }
        public void modAttPt(Variant value)
        {
            if (data != null)
            {
                Variant ptdata = new Variant();
                if (value.ContainsKey("strpt"))
                {
                    ptdata["strpt"] = data["strpt"]._int + value["strpt"]._int;
                    data["str"] = data["str"]._int + value["strpt"]._int;
                }
                if (value.ContainsKey("conpt"))
                {
                    ptdata["conpt"] = data["conpt"]._int + value["conpt"]._int;
                    data["con"] = data["con"]._int + value["conpt"]._int;
                }

                if (value.ContainsKey("intept"))
                {
                    ptdata["intept"] = data["intept"]._int + value["intept"]._int;
                    data["inte"] = data["inte"]._int + value["intept"]._int;
                }

                if (value.ContainsKey("agipt"))
                {
                    ptdata["agipt"] = data["agipt"]._int + value["agipt"]._int;
                    data["agi"] = data["agi"]._int + value["agipt"]._int;
                }
                if (value.ContainsKey("wispt"))
                {
                    ptdata["wispt"] = data["wispt"]._int + value["wispt"]._int;
                }
                //data = GameTools.mergeSimpleObject(ptdata, data);
            }
            if (value.ContainsKey("att_pt"))
                on_detail_info_change(GameTools.createGroup("att_pt", value["att_pt"]));
        }
        public void on_detail_info_change(Variant data1)
        {
            //Variant changeArr = new Variant();
            //foreach (string key in data1.Keys)
            //{
            //    if (key == "inte")
            //    {
            //        int sub = skillatkChange(data[key], data1[key]);
            //        if (sub != 0)
            //        {
            //            changeArr._arr.Add(GameTools.createGroup("key", "skillatk", "val", sub));
            //        }
            //    }
            //    int ret;
            //    if (int.TryParse(data1[key], out ret))
            //    {
            //        int val = data1[key] - data[key];
            //        if (val != 0)
            //        {
            //            changeArr._arr.Add(GameTools.createGroup("key", key, "val", val));
            //        }
            //    }

            //}
            //if (data1.ContainsKey("combpt"))
            //{
            //    int combptNum;
            //    if (data1["combpt"] > data["combpt"])
            //    {
            //        combptNum = (data1["combpt"] - data["combpt"]);
            //        mainuiAttach.OpenCombptUp(GameTools.createGroup("mark", "+", "combpt", combptNum));
            //    }
            //}
            //chaui.SelfDetailInfoChange(data1, "all");
            ////lguiMain.setChaInfo(data1);
            ////lguiMain.AddAttShows(changeArr);
            //if (data1.ContainsKey("att_pt"))
            //{
            //    (this.g_mgr.g_uiM.getLGUI(UIName.UI_TRANLIVE) as LGUITranlive).leftPointChange();
            //}
        }
        //智力改变会导致技能攻击力改变
        private int skillatkChange(int inteBefore, int inteAfter)
        {
            int carr = data["carr"];
            int sdmgb = (this.g_mgr.g_gameConfM as muCLientConfig).svrGeneralConf.GetSkillDmg(carr, inteBefore);
            int sdmga = (this.g_mgr.g_gameConfM as muCLientConfig).svrGeneralConf.GetSkillDmg(carr, inteAfter);
            return (sdmga - sdmgb);
        }
        //protected LGUIMainuiAttach mainuiAttach
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.LGUIMainuiAttach) as LGUIMainuiAttach;
        //    }
        //}
        //protected LGUICharacterInfoImpl chaui
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.UI_MDLG_CHAINFO) as LGUICharacterInfoImpl;
        //    }
        //}
        //protected LGUITestChainfo newchaui
        //{
        //    get
        //    {
        //        return this.g_mgr.g_uiM.getLGUI(UIName.TEST_LGUI) as LGUITestChainfo;
        //    }
        //}
        public void SetnNobName()
        {

        }
        public void modAttPt(int data)
        {

        }

        //public void RefreshMountAvatar()
        //{

        //}

        public void SetCloseInfo(Variant data)
        {

        }

        private void initData()
        {
            //if (this.data.ContainsKey("states"))
            //{
            //    addStates(this.data["states"]["state_par"]);
            //}

            //playerInfos.get_player_detailinfo(getCid(), onGetDetialInfo);
        }
        private void onGetDetialInfo(Variant info)
        {
            selfInfo.updataDetial(info);
            if (this.destroy) return;

            //chaui.SelfDetailInfoChange(info, "all");
            this.g_mgr.g_sceneM.dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_MAIN_CHAR, this, null)
            );

            //AddShowEqps( this.data["equip"], true );

            hp = this.viewInfo["hp"] = PlayerModel.getInstance().hp;
            max_hp = this.viewInfo["max_hp"] = PlayerModel.getInstance().max_hp;



            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, this.viewInfo));
        }
        override public Variant viewInfo
        {
            get
            {
                return this.selfInfo.mainPlayerInfo;
            }
        }
        override public Variant data
        {// 
            get
            {
                return this.selfInfo.m_data;
            }
        }
        override public float x
        {
            get
            {
                return viewInfo["x"];
            }
        }
        override public float y
        {
            get
            {
                return viewInfo["y"];
            }
        }
        override public uint getIid()
        {
            return this.data["iid"]._uint;
        }

        override public uint getCid()
        {
            return this.data["cid"]._uint;
        }

        override public void setPos(float x, float y)
        {
            selfInfo.setPos(x, y);
        }

        public int cid
        {
            get
            {
                return this.data["cid"]._int;
            }
        }

        private long _lastSendtm = 0;




        public bool canMove()
        {
            if (IsDie())
                return false;

            if (loading_cloud.instance != null && loading_cloud.instance.showed)
                return false;

            return m_ani == "idle" || m_ani == "run";
        }

        public bool canAttack()
        {
            if (IsDie())
                return false;

            return m_ani == "idle" || m_ani == "run";
        }
        int idx = 0;
        public bool canAutoAttack()
        {
            if (IsDie())
                return false;


            return m_ani == "idle";
        }

        //private MoveProxy moveproxy;

        public void onJoystickMove(float orgdata)
        {
            //这里的orgdata 这里的值其实是摇杆的值


            //if (!canMove())
            //    return;



            //long currtm = this.g_mgr.g_netM.CurServerTimeStampMS;
            //lg_ori_angle = orgdata - GRMap.GAME_CAM_CAMERA.transform.eulerAngles.y;
    
            //float x = (float)(pGameobject.transform.position.x + (float)Mathf.Cos(lg_ori_angle * Mathf.PI / 180) * 1.01);
            //float y = (float)(pGameobject.transform.position.z + (float)Mathf.Sin(lg_ori_angle * Mathf.PI / 180) * 1.01);

            //Vector3 vec3 = new Vector3(x, pGameobject.transform.position.y, y);

            //if (Nav != null)
            //    Nav.SetDestination(vec3);
            //LGCamera.instance.updateMainPlayerPos();

            //if (currtm - _lastSendtm < GameConstant.MOVE_FREQUENCY_MIN_TM)
            //{
            //    MoveProxy.getInstance().sendmoveRadian(pGameobject.transform.position.x * GameConstant.PIXEL_TRANS_UNITYPOS, pGameobject.transform.position.z * GameConstant.PIXEL_TRANS_UNITYPOS, lg_ori_angle, NetClient.instance.CurServerTimeStampMS);
            //    _lastSendtm = currtm;
            //}

        }
        //public void autoMis(uint misid)
        //{
        //    //this.stop(null);
        //    SPEDoMission ele = SPEDoMission.create(this, (int)misid);
        //    SPSBase s = new SPSBase(ele);
        //    s.sps_start(ele);
        //}

        //public void autoMainMis()
        //{
        //    SPEDoMisLine ele = SPEDoMisLine.create(this, GameConstant.MISSION_LINE_MAIN);
        //    SPSBase s = new SPSBase(ele);
        //    s.sps_start(ele);
        //}
        //public void autoMisLine(uint line)
        //{
        //    SPEDoMisLine ele = SPEDoMisLine.create(this, line);
        //    SPSBase s = new SPSBase(ele);
        //    s.sps_start(ele);
        //}

        //public void autoTask()
        //{
        //    SPEDoTask ele = SPEDoTask.create(this);
        //    SPSBase s = new SPSBase(ele);
        //    s.sps_start(ele);
        //}

        //public void onMainMisAct(uint misid)
        //{
        //    Variant misData = (this.g_mgr.g_gameConfM as muCLientConfig).svrMisConf.get_mission_conf((int)misid);
        //    if (misData["misline"]._int == 1) selfPlayer.autoMainMis();
        //}
        //private void onMisAccept(GameEvent e)
        //{
        //    onMainMisAct(e.data["misid"]._uint);
        //}
        //private void onMisCommit(GameEvent e)
        //{
        //    onMainMisAct(e.data["misid"]._uint);
        //}

        //private void onMisCanCommit(GameEvent e)
        //{
        //    onMainMisAct(e.data["misid"]._uint);
        //}

        //override protected void OnMsgStopAttack()
        //{
        //    setStand();
        //}
        public void onJoystickEnd(bool force = false)
        {
            long tm = this.g_mgr.g_netM.CurServerTimeStampMS;
            //Variant msg = new Variant();
            //Variant info = this.selfInfo.mainPlayerInfo;


            try
            {
                float x = (float)(pGameobject.transform.position.x) * GameConstant.PIXEL_TRANS_UNITYPOS;
                float y = (float)(pGameobject.transform.position.z) * GameConstant.PIXEL_TRANS_UNITYPOS;

                Vector3 vec3 = new Vector3(x, pGameobject.transform.position.y, y);

                //if (Nav != null)
                //    Nav.Stop();

                LGCamera.instance.updateMainPlayerPos();


                MoveProxy.getInstance().sendstop(
                              (uint)x,
                             (uint)y,
                              1,
                              tm,
                              force );
            }
            catch (System.Exception ex)
            {

            }



            //sendRpc( PKG_NAME.C2S_STOP, msg );

            this.stop();
            onMoveLinkCheck();
        }

        public void onSelectMonster(LGAvatarMonster lga)
        {

            //RotationToPt(lga.x, lga.y);

            //Vec2 gpos = lgMap.getGPosByPPos(lga.x, lga.y);
            //SPEKillMon ele = SPEKillMon.create(
            //    this,
            //    lgMap.curMapId,
            //    gpos,
            //    lga.getMid(),
            //    lga
            //);
            //SPSBase s = new SPSBase(ele);
            //s.sps_start(ele);

        }
        //private void onSelectKilled( GameEvent e )
        //{
        //	stop( null );			
        //}


        private void onAttackReach(Variant info)
        {
            moveTOAttackTarget = null;
            //LGAvatarGameInst lga = null;
            //bool smartSkill = false;
            //uint skilid = 0;


            //if (info != null)
            //{
            //    if (!info.ContainsKey("tar"))
            //    {
            //        return;
            //    }
            //    lga = info["tar"]._val as LGAvatarGameInst;
            //    smartSkill = info["smartSkill"]._bool;
            //    skilid = info["skilid"]._uint;
            //}
            //  attack(lga, smartSkill, skilid);
        }


        override public bool attack(LGAvatarGameInst lga, bool smartSkill = false, uint skilid = 0)
        {
            //	this.play_animation( "run", GameConstant.ANI_LOOP_FLAG_LOOP );
            if (lga == null)
            {
                if (skilid > 0)
                {
                    castSkill(skilid);
                }
                else
                {
                    play_animation(GameConstant.ANI_SINGLE_H_ATK, GameConstant.ANI_LOOP_FLAG_NOT_LOOP);
                }
                return false;
            }

            if (!smartSkill && skilid <= 0)
            {
                skilid = getCurDefSkill();
            }
            SkillData sdta = SkillModel.getInstance().getSkillData(skilid);

            bool needjump = false;

            if (sdta.skill_data_xml != null && sdta.skill_data_xml.target_type == 1 && !isInSkillRange(lga, sdta))
            {
                int dist_x = (int)(lga.x) - (int)(this.x);
                int dist_y = (int)(lga.y) - (int)(this.y);

                //if (skillbar.instance.canUseAutoJump() && isInJumpRange(lga))
                //{
                //    needjump = true;
                //}
                //else
                //{
                //    Variant tar = new Variant();
                //    tar["tar"] = new Variant();
                //    tar["tar"]._val = lga;
                //    tar["smartSkill"] = smartSkill;
                //    tar["skilid"] = skilid;
                //    tar["range"] = sdta.range;
                //    moveTOAttackTarget = tar;

                //    movePosSingle(lga.x, lga.y, onAttackReach, tar, sdta.range, lga);
                //    return false;
                //}
            }

            //if ( isFriend( lga ) ) 
            //{
            //   return attackFriend( lga, skilid );    
            //}
            //else
            {
                if (CanAttack(lga))
                {

                    return attackEnemy(lga, smartSkill, skilid, needjump);
                }
            }
            return false;

        }

        private Variant moveTOAttackTarget = null;

        SkillData jumpSkilld;
        private bool isInJumpRange(LGAvatarGameInst avatar)
        {
            if (jumpSkilld == null)
                jumpSkilld = SkillModel.getInstance().getSkillData(1008);

            if (jumpSkilld == null)
                return false;

            return IsInRange(avatar, jumpSkilld.range, false);
        }

        private bool attackFriend(LGAvatarGameInst lga, uint skilid)
        {
            operationLgAvatarSet(lga);

            if (skilid > 0)
            {
                return castSkill(skilid);
            }
            else
            {
                _singleAttackFlag = true;
                uint iid = data["iid"]._uint;
                //msgBattle.attack(iid);
                play_animation(GameConstant.ANI_SINGLE_H_ATK, GameConstant.ANI_LOOP_FLAG_LOOP);
                return true;
            }
        }

        private bool attackEnemy(LGAvatarGameInst lga, bool smartSkill = false, uint skilid = 0, bool needjump = false)
        {
            Variant data = lga.data;

            if (!data.ContainsKey("iid")) return false;

            operationLgAvatarSet(lga);

            _castSkilid = skilid;
            _smartSkil = smartSkill;

            //_attackFlag = true;//不用自动 by lucisa

            if (smartSkill)
            {
                skilid = getSkilSmart();
            }

            if (skilid > 0)
            {
                return castSkill(skilid, needjump);
            }
            else
            {
                _singleAttackFlag = true;
                uint iid = data["iid"]._uint;
                //msgBattle.attack(iid);
                play_animation(GameConstant.ANI_SINGLE_H_ATK, GameConstant.ANI_LOOP_FLAG_LOOP);
                return true;
            }

        }
        private void stopAttact()
        {
            moveTOAttackTarget = null;
            setStand();
            _attackFlag = false;
            clearCastSkil();
            if (_singleAttackFlag)
            {
                _singleAttackFlag = false;
                //msgBattle.stop_atk();
            }
        }
        override protected void onStop()
        {
            base.onStop();
            moveTOAttackTarget = null;
            operationLgAvatarClear();
        }
        private void clearCastSkil()
        {
            _castSkilid = 0;
            _smartSkil = false;
        }
        override protected void onOperationLgaClear()
        {//是否自动任务？			
            //	stopAttact();	

            SKillCdBt.needAutoNextAttack = false;
        }

        private Dictionary<uint, int> dSkillPreload = new Dictionary<uint, int>();
        public void doSkillPreload()
        {
            //int sex = PlayerModel.getInstance().sex;
            Dictionary<uint, SkillData> dskill = SkillModel.getInstance().skillDatas;
            foreach (SkillData d in dskill.Values)
            {
                if (dSkillPreload.ContainsKey(d.id))
                    continue;
                dSkillPreload[d.id] = 1;

                play_animation(d.id.ToString(), GameConstant.ANI_LOOP_FLAG_NOT_LOOP);


                if (d.eff != "null")
                {
                    MapEffMgr.getInstance().play(d.eff, Vector3.zero, Vector3.zero, 0.1f);
                }
            }
        }

        override protected void playSkill(uint sid, int sex, uint toIID, float x = 0, float y = 0)
        {
            //skillbar.instance.onCD(sid);
            //base.playSkill(sid, sex, toIID, x, y);
        }

        //override protected void OnCastSkillRes(GameEvent e)
        //{
        //    Variant data = e.data;
        //    if (data["res"]._int < 0)
        //    {
        //        GameTools.PrintNotice("OnCastSkillRes failed!" + "res: " + data["res"]._int);
        //        return;
        //    }
        //    if (data["res"] == 1)
        //    {
        //        if (data.ContainsKey("mp_c"))
        //        {
        //            this.modMp(-data["mp_c"]);
        //        }
        //    }
        //    uint sid = data["sid"]._uint;


        //    _castTm = this.g_mgr.g_netM.CurServerTimeStampMS;
        //    List<uint> skidCdDie = new List<uint>();
        //    if (_skillCDs.Count > 0)
        //    {
        //        foreach (uint skid in _skillCDs.Keys)
        //        {
        //            float tm = _skillCDs[skid]["start_tm"]._float + _skillCDs[skid]["cd_tm"]._float * 100;
        //            if (tm <= _castTm)
        //            {
        //                skidCdDie.Add(skid);
        //                //delete _skillCDs[skid];
        //            }
        //        }
        //    }

        //    for (int i = 0; i < skidCdDie.Count; i++)
        //    {
        //        _skillCDs.Remove(skidCdDie[i]);
        //    }
        //    if (data.ContainsKey("cd_tm_spec"))//单独cd
        //    {
        //        _skillCDs[sid] = GameTools.createGroup("start_tm", _castTm, "cd_tm", data["cd_tm_spec"]._float);
        //    }
        //    if (data.ContainsKey("cdtm"))
        //    {
        //        _skillcd = data["cdtm"]._float;//公共cd
        //    }
        //    if (genConf.IsDefSkill(this.data["carr"], sid))
        //    {
        //        Variant defSkill = genConf.GetCarrDefSkill(this.data["carr"]);

        //        _nextSkillTurn++;
        //        if (_nextSkillTurn >= defSkill.Count)
        //        {
        //            _nextSkillTurn = 0;
        //        }
        //    }
        //    //lgskill.SetSkillCD(data, _skillCDs);
        //    //lguiMain.ColdDown(data, _skillCDs);

        //    // skillbar.instance.onCD(data["sid"], data["cdtm"]);


        //}

        //override protected void on_cancel_casting_skill(GameEvent e)
        //{

        //}


        private bool isFriend(LGAvatarGameInst lga)
        {
            if (lga.getIid() == this.getIid())
            {
                return true;
            }

            return false;
        }

        private bool CanAttack(LGAvatarGameInst lga)
        {
            if (lga == null) return false;
            if (this.IsDie() || lga.IsDie())
            {
                return false;
            }

            Variant selfData = this.data;
            Variant charData = lga.data;
            //uint owner_cid = 0;

            uint cid = selfData["cid"]._uint;

            if (lga is LGAvatarMonster)
            {
                if (lga.IsCollect())
                    return false;

                if (charData.ContainsKey("owner_cid"))//如果是战斗宠物
                {
                    uint owner_cid = charData["owner_cid"]._uint;
                    if (owner_cid == cid)
                    {	//开启pk
                        return false;
                    }


                    LGAvatarGameInst owner_ply = lgOthers.get_player_by_cid(owner_cid);
                    if (owner_ply != null)
                    {
                        charData = owner_ply.data;

                        if (null == charData || charData["level"]._uint < 50)
                        {
                            return false;
                        }
                    }
                    else//没有主人时，不可攻击
                    {
                        return false;
                    }
                }
            }



            //if( this.lgInGame.lgGD_levels.in_level )
            //{
            //	//战场逻辑		
            //	if( selfData.lvlsideid > 0 )//阵营模式
            //	{	
            //		if( charData.lvlsideid != selfData.lvlsideid )
            //			return true;					

            //		if( !this.lgInGame.lgGD_levels.is_currlvl_map_ignore_side(this.lgInGame.map.curMapId) )
            //		{
            //			return false;
            //		}
            //		//即使忽略阵营也不攻击己方的非战斗宠怪物
            //		if(targetChar is LGAvatarMonster)
            //		{
            //			return false;
            //		}
            //	}
            //}
            //else
            //{				
            //	//PVE 和 大世界逻辑
            //	if( targetChar is LGAvatarMonster )
            //	{
            //		//if( owner_cid == 0  ) 
            //		return true;
            //	}				

            //}

            return true;
        }
        private bool isSkilCanCast(uint sid)
        {//todo cd
            if (this.IsDie())
                return false;

            if (isSkilCD(sid))
                return false;

            return true;
        }
        private float _castTm = 0;//施放技能时间
        private Dictionary<uint, Variant> _skillCDs = new Dictionary<uint, Variant>();
        public bool isSkilCD(uint sid)
        {
            float currTm = NetClient.instance.CurServerTimeStampMS;
            if (_skillCDs.ContainsKey(sid))
            {
                return (_skillCDs[sid]["start_tm"]._float + _skillCDs[sid]["cd_tm"]._float * 100) > currTm || ((_castTm + _skillcd * 100) > currTm);
            }
            return (_castTm + _skillcd * 100) > currTm;
        }
        private uint getSkilSmart()
        {
            if (_castSkilid > 0)
            {
                if (genConf.IsDefSkill(this.data["carr"]._int, _castSkilid))
                {
                    return getCurDefSkill();
                }
                if (isSkilCanCast(_castSkilid))
                {
                    return _castSkilid;
                }
            }
            if (_smartSkil)
            {//todooo

            }
            return 0;
        }
        public uint getCurDefSkill()
        {
            Variant defskills = genConf.GetCarrDefSkill(this.data["carr"]._int);
            if (defskills != null)
            {
                return defskills[lgMainPlayer.nextSkillTurn]._uint;
            }
            return 0;
        }

        public bool castSkill(uint sid, bool needJump = false)
        {
            return false;
        }




        //private void onChangeMapReadyRes(GameEvent e)
        //{//do change map failed
        //    if (e.data["res"]._int < 0)
        //    {
        //        clearPauseFlag(GameConstant.PAUSE_ON_MAP_CHANGE);
        //        return;
        //    }

        //    //  ogLoading.show();
        //}


        private void onChangeMap(GameEvent e)
        {
            //   clearPauseFlag(GameConstant.PAUSE_ON_MAP_CHANGE);

            //if (Nav != null)
            //{
            //    Nav.running = false;
            //}

            this.x += 0.1f;
            this.y += 0.1f;
        }

        public void onMoveLinkCheck()
        {
            Variant ln = this.lgMap.get_map_link(
                this.lgMap.pixelToGridSize(this.x),
                this.lgMap.pixelToGridSize(this.y)
            );
            if (ln == null) return;

            //setPauseFlag(GameConstant.PAUSE_ON_MAP_CHANGE);

            lgMap.beginChangeMap(ln["gto"]._uint);
        }

        //private uint _pauseFlag = 0;
        //private void setPauseFlag(uint flag)
        //{
        //    _pauseFlag |= flag;
        //    this.pause = true;
        //    if (m_currSt != null)
        //    {
        //        m_currSt.setPause(this.pause);
        //    }
        //}

        //private void clearPauseFlag(uint flag)
        //{
        //    _pauseFlag &= ~flag;

        //    this.pause = (_pauseFlag != 0);

        //    if (m_currSt != null)
        //    {
        //        m_currSt.setPause(this.pause);
        //    }
        //}

        public LGAvatarMonster getSelectTarget()
        {
            return _selectLgAvatar as LGAvatarMonster;
        }
        public LGAvatarGameInst selectTarget
        {
            get
            {
                return _selectLgAvatar;
            }
            set
            {
                _selectLgAvatar = value;
            }
        }


        private void actSkill()
        {
            uint skilid = getSkilSmart();
            if (skilid <= 0) return;
            castSkill(skilid);
        }

        private int autoMovingTick = 0;
        private int m_nrefreshWpRibbon = 0;
        override public void updateProcess(float tmSlice)
        {
          
        }

      
        public Variant get_value(String attname)
        {
            Variant value;
            if (data != null && data.ContainsKey(attname))
            {
                value = data[attname];
            }
            else
            {
                return null;
            }
            return value;
        }
        //判断职业是否满足
        static public Boolean check_carr(int carr, Variant atts)
        {
            int self_lvl = atts["carrlvl"];
            int j = check_only_carr(carr, atts);
            if (j != -1)
            {
                int lvl = (carr >> (j * 4 + 1)) & 0x07;
                if (self_lvl >= lvl)
                {
                    return true;
                }
            }
            return false;
        }
        //判断职业是否满足（不判断转职）
        static public int check_only_carr(int carr, Variant atts)
        {
            int self_carr = atts["carr"];
            int loop = 8;

            //每4位为一个职业
            for (int j = 0; j < loop; ++j)
            {
                if (((carr >> (j * 4)) & 0x01) != 0)
                {
                    int car = j + 1;
                    if (self_carr == car)
                    {
                        return j;
                    }
                }
            }
            return -1;
        }

        public override void Die(Variant data)
        {
            base.Die(data);
            if (MapModel.getInstance().curLevelId == 0)
            {
                InterfaceMgr.getInstance().closeAllWin();
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.RELIVE);
            }

        }
        protected override void respawn(Variant data)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.RELIVE);
            base.respawn(data);

            setStand();
            setMeshAni("idle", 0);
        }

        //----------------------------------------checks相关 Start---------------------------------------------------
        private checks _checks = null;
        public bool check(Variant data)
        {
            if (data.isArr)
            {
                if (data.Count == 0)
                    return true;
            }

            if (_checks == null)
            {
                _checks = new checks();
            }

            foreach (string k in data.Keys)
            {
                if (!_checks.isPropertyMethod(k))
                    continue;
                if (data[k].isInteger)
                {
                    if (!_checks.getCheckMethod(k)(this, data[k], this.g_mgr))
                        return false;
                }
                else
                {
                    foreach (Variant i in data[k]._arr)
                    {

                        if (!_checks.getCheckMethod(k)(this, i, this.g_mgr))
                            return false;
                    }
                }
            }

            return true;
        }
    }


    class checks
    {
        private lgSelfPlayer _selfPlayer = null;
        private gameManager g_mgr;
        private Dictionary<string, Func<Variant, Boolean>> checkMethod;
        private Dictionary<string, Func<lgSelfPlayer, Variant, gameManager, Boolean>> propertyMethod;
        //private LGInGame lgInGame = null;
        public checks()
        {
            //check_complex_flvl
            //check_flvl
            //check_stns
            //check_bcnt
            //check_kcnt
            //check_have
            //check_ware
            checkMethod = new Dictionary<string, Func<Variant, bool>>();
            //checkMethod.Add("check_complex_flvl", check_complex_flvl);
            //checkMethod.Add("check_flvl", check_flvl);
            //checkMethod.Add("check_stns", check_stns);
            //checkMethod.Add("check_bcnt", check_bcnt);
            //checkMethod.Add("check_kcnt", check_kcnt);
            //checkMethod.Add("check_have", check_have);
            //checkMethod.Add("check_ware", check_ware);

            propertyMethod = new Dictionary<string, Func<lgSelfPlayer, Variant, gameManager, bool>>();
            propertyMethod.Add("attchk", attchk);
            propertyMethod.Add("eqpchk", eqpchk);

        }

        public bool isPropertyMethod(string name)
        {
            return propertyMethod.ContainsKey(name);
        }
        public Func<lgSelfPlayer, Variant, gameManager, bool> getCheckMethod(string name)
        {
            return isPropertyMethod(name) ? propertyMethod[name] : null;
        }

        public Boolean attchk(lgSelfPlayer self, Variant attchk, gameManager mgr = null)
        {
            Variant value = self.get_value(attchk["name"]);
            if (attchk["name"] == "carr")
            {	//职业判断
                return (lgSelfPlayer.check_carr(attchk["and"], self.data));
            }

            if (attchk["name"] == "autofinlvl")
            {	//是否使用过立即完成副本功能
                return value != null;//Boolean(value);
            }
            if (attchk.ContainsKey("have"))
            {
                if (value.isArr)
                {
                    if ((value._arr).IndexOf(attchk["have"]) != -1)
                        return true;
                }
                else
                {
                    if (value == attchk["have"])
                        return true;
                }
            }

            if (value.isArr)
                return false;
            if (attchk.ContainsKey("equal"))
            {
                if (value == attchk["equal"])
                    return true;
                else
                    return false;
            }
            else if (attchk.ContainsKey("min") || attchk.ContainsKey("max"))
            {
                if (attchk.ContainsKey("min"))
                {
                    if (value < attchk["min"])
                        return false;
                }
                if (attchk.ContainsKey("max"))
                {
                    if (value > attchk["max"])
                        return false;
                }
                return true;
            }
            return false;
        }

        //---------------------------------eqpCheck-------------------------------------------------
        public Boolean eqpchk(lgSelfPlayer self, Variant attchk, gameManager mgr)
        {
            if (_selfPlayer == null)
            {
                _selfPlayer = self;
            }
            if (this.g_mgr == null)
            {
                this.g_mgr = mgr;
            }
            string name = attchk["name"];
            string method = "check_" + name;

            if (!this.checkMethod.ContainsKey(method))
            {
                return true;
            }

            return this.checkMethod[method](attchk);
        }

        //身上装备是否满足位置 品质需求 chkTP{pos,qual };
        private Boolean check_pos_qual_body(Variant eqpchk, string chkTP)
        {
            //在获得身上的装备
            int cid = _selfPlayer.cid;
            Variant self_detail = _selfPlayer.data;
            Variant equip_arr = null;
            if (self_detail != null)
            {
                equip_arr = self_detail["equip"];
            }

            if (equip_arr == null)
                return false;

            Variant itemObj = null;
            foreach (Variant d in equip_arr._arr)
            {

                itemObj = (g_mgr.g_gameConfM as muCLientConfig).svrItemConf.get_item_conf(d["tpid"]);
                if (itemObj == null)
                    continue;
                if (itemObj["conf"][chkTP] == eqpchk[chkTP])
                {
                    return true;
                }

            }
            return false;
        }

        //背包装备是否满足位置需求 品质需求 
    


        private Variant equip_data
        {
            get
            {
                return _selfPlayer.data["eqp"];
            }
        }


    }


}