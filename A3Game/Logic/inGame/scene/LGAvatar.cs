using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;

namespace MuGame
{
    class LGAvatar : LGAvatarBase, IProcess
    {
        public LGAvatarGameInst lockTarget;

        //	protected SPSBase m_currSt;
        //protected List<SPSBase> m_stQue = new List<SPSBase>();

        protected float m_ori = 0.0001f;//朝向,角度(设置为0.0001是为了第一次的重置)

        protected string m_ani = GameConstant.ANI_IDLE_NORMAL;
        protected bool m_loopFlag = false;
        protected bool m_visible = true;
        protected int m_stateTag = GameConstant.ST_STAND; //状态标识
        protected Variant m_conf;

        public GRAvatar grAvatar;
        public LGGRAvatar lggrAvatar;

        public LGAvatar(gameManager m)
            : base(m)
        {

        }
        override public void init()
        {
            onInit();
        }

        virtual public void onInit()
        {//子类重写
        }

        virtual public Variant viewInfo
        {//子类重写
            get;
            set;
        }

        virtual public Variant data
        {//子类重写
            get;
            set;
        }
        virtual public Variant conf
        {//子类重写
            get
            {
                return null;
            }
        }

        protected void setMoveInfo(Variant moveinfo)
        {
            this.data["moving"] = moveinfo;
        }
        protected void clearMoveInfo()
        {
            if (!this.data.ContainsKey("moving")) return;
            this.data.RemoveKey("moving");
        }

        public override void initGr(GRBaseImpls grBase, LGGRBaseImpls sctrl)
        {
            if (grBase is GRAvatar)
                grAvatar = grBase as GRAvatar;

            if (sctrl is LGGRAvatar)
                lggrAvatar = sctrl as LGGRAvatar;
        }

        public bool isMainPlayer()
        {
            return roleType == ROLE_TYPE_USER;
            // return getCid() == selfPlayer.getCid();
        }
        public bool IsCollect()
        {
            if (m_conf != null && m_conf.ContainsKey("collect_tar"))
            {
                return m_conf["collect_tar"]._int > 0;
            }
            return false;
        }

        public void setConfig(Variant conf)
        {
            m_conf = conf;
        }

        public string getAni()
        {
            return m_ani;
        }

        public bool IsRunningAni()
        {
            if (grAvatar != null)
            {
                GRCharacter3D curchar = (this as LGAvatarGameInst).grAvatar.m_char;
                if (curchar != null)
                {
                    return curchar.IsRunningAnim();
                }
            }

            return false;
        }

        public void refreshIdle()
        {
            setMeshAni("run", 0);
            setMeshAni("idle", 0);
        }

        protected void setMeshAni(string ani, int loop)
        {
            if (this is LGAvatarGameInst && (this as LGAvatarGameInst).grAvatar != null)
            {
                m_ani = ani;
                refreshAniLoopflag();
                if (m_loopFlag) loop = 0;

                (this as LGAvatarGameInst).grAvatar.setAni(ani, loop);
            }

            //dispatchEvent(
            //    GameEvent.Createimmedi(GAME_EVENT.SPRITE_ANI, this, createAniD(ani, flag))
            //);
        }

        public string getMoveAni()
        {// 
            return GameConstant.ANI_MOVE_NORMAL;
        }

        public bool visibleFlag()
        {// 
            return m_visible;
        }

        override public float lg_ori_angle
        {
            get
            {
                return m_ori;
            }

            set
            {
                if (m_ori == value) return;
                m_ori = value;

                if (grAvatar != null)
                {
                    (this as LGAvatarGameInst).grAvatar.setOri(m_ori);
                }
            }
        }

        virtual public void setPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float speed
        {
            get
            {
                return this.viewInfo["speed"]._float;
            }
        }

        public bool IsDie()
        {
            return m_stateTag == GameConstant.ST_DIE;
        }

        public bool IsMoving()
        {
            return m_stateTag == GameConstant.ST_MOVE;
        }

        public void setAttack()
        {
            setTag(GameConstant.ST_ATK);
        }

        public void setStand()
        {
            setTag(GameConstant.ST_STAND);
        }

        public void setDie()
        {
            setTag(GameConstant.ST_DIE);
            playDeadSound();
        }

        virtual protected void playDeadSound()
        {

        }

        protected void setTag(int tag)
        {
            //if( isMainPlayer() && tag == GameConstant.ST_STAND )
            //{
            //    if( false )//todo pk state
            //    {
            //        tag = GameConstant.ST_STAND_FOR_ATK;
            //    }
            //}

            //if (tag == GameConstant.ST_MOVE)
            //{
            //    debug.Log("播放 跑动动作");
            //}
            if (m_stateTag == GameConstant.ST_DIE && !(this is lgSelfPlayer)) return;
            m_stateTag = tag;
            refreshAni();
        }

        public int stateTag
        {
            get
            {
                return m_stateTag;
            }
        }

        ////久的接口，已经弃用
        //virtual public void AddTitleSprite(string tp, int showtp = 0, Variant showInfo = null)
        //{
        //    //Variant data = GameTools.createGroup("tp", tp, "showtp", showtp);
        //    //data["showInfo"] = showInfo;
        //    //this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_ADD_TITLE, this, data));
        //}

        ////久的接口，已经弃用
        //virtual public void RemoveTitleSprite(string tp, int showtp = 0)
        //{
        //    //Variant data = GameTools.createGroup("tp", tp, "showtp", showtp);
        //    //this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_REMOVE_TITLE, this, data));
        //}

        public void UpRide(bool b)
        {

        }

        virtual public bool attack(LGAvatarGameInst lg, bool smartSkill = false, uint skillid = 0)
        {
            return false;
        }

        public Variant GetStates()
        {
            return null;
        }





        // ============  state set  ============
        //private void changeAniState(SPSBase st)
        //{
        //    m_currSt = st;

        //    setTag(m_currSt.tag_of_act);

        //    m_currSt.refreshParam();
        //}


        //public void onChangeState( SPSBase st )
        //{		 
        //    //setTag( st.tag );
        //}	

        public void stop()
        {
            clearMoveInfo();

            if (!IsDie()) setStand();


            //this.dispatchEvent( 
            //    GameEvent.Create( GAME_EVENT.SPRITE_STOP, this, null ) 
            //);

            onStop();
        }

        virtual protected void onStop()
        {

        }

        public void addShowEff(string effid)
        {//即时显示效果

            Variant sconf = this.g_mgr.g_sceneM.getEffectConf(effid);
            this.dispatchEvent(
                GameEvent.Create(GAME_EVENT.SPRITE_ADD_SHOW_EFF, this, sconf)
            );
        }

        ////有时间顺序的  设置奖态不能为空，没状态可设默认
        //public void setlgState(SPSBase st)
        //{
        //    //_setState( st );
        //    if (m_currSt != null)
        //    {
        //        m_currSt.dispose();
        //    }

        //    changeAniState(st);
        //    refreshAni();
        //}

        //protected void RmvShowEqps(Variant equips)
        //{
        //    if (data == null)
        //        return;
        //    Variant eqps = data["eqp"];

        //    foreach (Variant rmvEqp in equips._arr)
        //    {
        //        if (eqps != null)
        //        {
        //            for (int i = 0; i < eqps.Length; i++)
        //            {
        //                Variant eqp = eqps[i];
        //                if (eqp["tpid"]._int == rmvEqp["tpid"]._int)
        //                {
        //                    eqps._arr.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //        }

        //        Variant itmConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrItemConf.get_item_conf(rmvEqp["tpid"]._uint);
        //        if (itmConf != null)
        //        {
        //            rmvCharEquip(rmvEqp, itmConf["conf"]);
        //            Variant rmvEqps = new Variant();
        //            rmvEqps._arr.Add( rmvEqp );
        //            removeAvatar( rmvEqps );

        //            //setTakePosFlag( GameConstant.TAKEPOS_NONE );
        //        }

        //    }
        //    refreshAni();
        //}

        //private Variant _curRide;
        protected void rmvCharEquip(Variant eqp, Variant eqpConf)
        {
            //switch (eqpConf["pos"]._int)
            //{
            //    case GameConstant.EQP_POS_GUARD:
            //        {
            //            this.ChangeFollowGuard(0);
            //            break;
            //        }
            //    case GameConstant.EQP_POS_PET:
            //        {
            //            this.ChangeFollowPet(0);
            //            break;
            //        }
            //    case GameConstant.EQP_POS_RIDE:
            //        {
            //            //_curRide = null;
            //            break;
            //            //if (_hideRide) return;
            //        }
            //}
            ////rmvGREquip(eqp, eqpConf);
        }

        //protected void AddShowEqps(Variant equips, bool initflag=false )
        //{
        //    if (data == null)
        //        return;

        //    if( !data.ContainsKey("eqp") ) data["eqp"] = new Variant();

        //    Variant eqps = data["eqp"];
        //    Variant addEqps = new Variant();			 
        //    foreach (Variant addEqp in equips._arr)
        //    {
        //        if ( !initflag )
        //        {
        //            eqps._arr.Add(addEqp);
        //        }

        //        //Variant itmConf = (this.g_mgr.g_gameConfM as muCLientConfig).svrItemConf.get_item_conf(addEqp["tpid"]._uint);
        //        //if (itmConf != null)
        //        //{
        //        //    //changeCharEquip(addEqp, itmConf["conf"]);
        //        //     addEqps._arr.Add( addEqp );
        //        //    if(itmConf["conf"].ContainsKey("takepos"))
        //        //    {
        //        //        if (itmConf["conf"]["takepos"]._int == 1)
        //        //        {
        //        //            setTakePosFlag(GameConstant.TAKEPOS_SINGLE);
        //        //        }
        //        //        else if (itmConf["conf"]["takepos"]._int == 2)
        //        //        {
        //        //            setTakePosFlag(GameConstant.TAKEPOS_DOUBLE);
        //        //        }
        //        //    }
        //        //    if (itmConf["conf"].ContainsKey("subtp"))
        //        //    {
        //        //        int f = itmConf["conf"]["subtp"]._int;
        //        //        setTakePosSubFlag(f);
        //        //    }
        //        //}
        //    }

        //    changeAvatar( addEqps );

        //    refreshAni();
        //}

        protected void refreshAni()
        {
            //string ani = takeposToAni( m_stateTag , takeposFlag , takeposFlagSub );
            //debug.Log("刷新动作 ... " + ani);

            switch (m_stateTag)
            {
                case GameConstant.ST_STAND: setMeshAni("idle", 0); break;
                case GameConstant.ST_MOVE: setMeshAni("run", 0); break;
                case GameConstant.ST_ATK: setMeshAni("attack", 1); break;
                case GameConstant.ST_DIE: setMeshAni("dead", 1); break;

                default: setMeshAni("idle", 0); break;
            }
        }

        private void refreshAniLoopflag()
        {
            m_loopFlag = false;
            if (m_ani == "idle" || m_ani == "run")
            {
                m_loopFlag = true;
            }
        }

        public void play_animation(string ani, int flag = GameConstant.ANI_LOOP_FLAG_AUTO)
        {
            setMeshAni(ani, 0);
        }

        public void playAniOnMoveReach()
        {
            setTag(GameConstant.ST_STAND);
            //setAni( GameConstant.ANI_IDLE_NORMAL, GameConstant.ANI_LOOP_FLAG_AUTO );			 
        }

        public void dispose()
        {
            onDispose();
            this.destroy = true;
            //this.m_currSt = null;
            //this.m_stQue.Clear();
            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_DISPOSE, this, null));
        }

        virtual protected void onDispose()
        {

        }

        public List<GridST> findPath(float fx, float fy, float tx, float ty)
        {
            // List<Variant> path = this.mgr.mapCT.findPath(
            //	this.mgr.mapCT.getGPosByPPos(fx, fy),
            //	this.mgr.mapCT.getGPosByPPos(tx, ty) 
            //);

            List<GridST> path = lgMap.findPath(
                lgMap.getGPosByPPos(fx, fy),
                lgMap.getGPosByPPos(tx, ty)
            );
            if (path == null) return null;



            ////DebugTrace.print( "move from x:"+fx+", y:"+fy+" -to-> x:"+tx+",y:"+ty+"|" + "gx:"+fx/32+", gy:"+fy/32+" -to-> gx:"+tx/32+",gy:"+ty/32+","  );
            //for(int i=path.Count-1; i>=0; i-- )
            //{
            //	GridST t = path[i];
            //	//DebugTrace.print( "[path idx["+i+"]  pos x:"+ t.x +", y:"+t.y+"]" );
            //}

            return path;
        }

        //public void changeAvatar( Variant arr )
        //{//tpids[tpid]

        //    this.dispatchEvent(
        //            GameEvent.Create( GAME_EVENT.SPRITE_CHANGE_AVATAR, this, arr ) 
        //    ); 
        //}

        //public void removeAvatar( Variant arr )
        //{//tpids[tpid]

        //    this.dispatchEvent(
        //        GameEvent.Create( GAME_EVENT.SPRITE_REMOVE_AVATAR, this, arr ) 
        //    ); 
        //}



        virtual public uint getIid()
        {
            return 0;
        }
        virtual public uint getCid()
        {
            return 0;
        }
        virtual public uint getMid()
        {
            return 0;
        }
        virtual public uint getNid()
        {
            return 0;
        }

        public float RotationToPt(float tx, float ty)
        {
            float a = (float)((Math.Atan2(ty - this.grAvatar.pTrans.position.z, tx - this.grAvatar.pTrans.position.x)) * (180 / Math.PI));
            this.lg_ori_angle = a;
            return a;
        }

        public void faceToAvatar(LGAvatar avatar)
        {
            RotationToPt(avatar.grAvatar.pTrans.position.x, avatar.grAvatar.pTrans.position.z);
        }

        public bool m_bSinging = false;
        public void OnSong(Variant vd)
        {
            //播放吟唱动作和警戒范围
            m_bSinging = true;
            setMeshAni("song", 255);

            //debug.Log("吟唱技能 ID=" + vd.dump());
            if (grAvatar != null && vd.ContainsKey("skid"))
            {
                grAvatar.playSong(vd["skid"]._uint);
            }
        }

        //public void OnSOver()
        //{
        //    setMeshAni("sover", 1);
        //    m_bSinging = false;
        //}

        private bool m_bsong_c = false;
        public void OnAttack(Variant data, LGAvatar toChar)
        {
            //if (m_bsong_c)
            //{
            //    m_bsong_c = false;
            //    //OnSOver();
            //}
            //else
            //{
            //    m_bsong_c = true;
            //    OnSong();
            //}
            //return;


            if (IsDie()) return;
            if (isMainPlayer())
                return;

            if (toChar == null) return;

            //设置正面朝目标	
            RotationToPt(toChar.x, toChar.y);

            //if (m_ani == "run" || m_ani == "idle")
            //{

            playAttackAni();
            //}
        }

        virtual protected void playAttackAni(bool criatk = false)
        {
            setAttack();
        }

        public LGAvatarGameInst get_Character_by_iid(uint iid)
        {
            if (iid == selfInfo.mainPlayerInfo["iid"]._uint)
            {
                return lgMainPlayer;
            }
            LGAvatarGameInst lga = lgMonsters.get_mon_by_iid(iid);
            if (lga == null)
            {
                lga = lgOthers.get_player_by_iid(iid);
            }
            return lga;
        }

        public bool IsInRange(LGAvatar lga, float rangePixel, bool iscell = true)
        {
            if (iscell)
                return IsInRange(lga.x, lga.y, rangePixel, iscell);
            float dis =  Vector3.Distance(lga.grAvatar.pTrans.position, grAvatar.pTrans.position);

            return dis <= rangePixel;
        }


        public bool IsInRange(float _x, float _y, float range, bool iscell = true)
        {
            float dist_x;
            float dist_y;
            if (iscell)
            {
                dist_x = (int)(_x - this.x) / GameConstant.GEZI_TRANS_UNITYPOS;
                dist_y = (int)(_y - this.y) / GameConstant.GEZI_TRANS_UNITYPOS;
            }
            else
            {
                dist_x = (int)(_x) - (int)(this.x);
                dist_y = (int)(_y) - (int)(this.y);
            }
            return (dist_x * dist_x + dist_y * dist_y < range * range);
        }

        protected Variant getTitleConf(string tp, int showtp = 0, Variant showInfo = null)
        {
            return GameTools.createGroup("tp", tp, "showtp", showtp, "showInfo", showInfo);
        }

        // ==== objs ====
        protected ClientSkillConf skillConf
        {
            get
            {
                return (this.g_mgr.g_gameConfM as muCLientConfig).getObject(OBJECT_NAME.CONF_LOCAL_SKILL) as ClientSkillConf;
            }
        }

        protected ClientGeneralConf genConf
        {
            get
            {
                return (this.g_mgr.g_gameConfM as muCLientConfig).getObject(OBJECT_NAME.CONF_LOCAL_GENERAL) as ClientGeneralConf;
            }
        }
        protected SvrSkillConfig svrSkillConf
        {
            get
            {
                return (this.g_mgr.g_gameConfM as muCLientConfig).getObject(OBJECT_NAME.CONF_SERVER_SKILL) as SvrSkillConfig;
            }
        }
        protected LGMap lgMap
        {
            get
            {
                return this.g_mgr.getObject(OBJECT_NAME.LG_MAP) as LGMap;
            }
        }
        protected lgSelfPlayer selfPlayer
        {
            get
            {
                return (this.g_mgr.g_gameM as muLGClient).g_selfPlayer;
            }
        }

        protected joinWorldInfo selfInfo
        {
            get
            {
                return this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_JOIN_WORLD) as joinWorldInfo;
            }
        }
        protected LGOthers lgOthers
        {
            get
            {
                return this.g_mgr.g_gameM.getObject(OBJECT_NAME.LG_OTHER_PLAYERS) as LGOthers;
            }
        }
        protected LGMonsters lgMonsters
        {
            get
            {
                return this.g_mgr.g_gameM.getObject(OBJECT_NAME.LG_MONSTERS) as LGMonsters;
            }
        }
        protected lgSelfPlayer lgMainPlayer
        {
            get
            {
                return this.g_mgr.g_gameM.getObject(OBJECT_NAME.LG_MAIN_PLAY) as lgSelfPlayer;
            }
        }

        private bool _pause = false;
        private bool _destory = false;
        protected string _processName = "";



        public bool destroy
        {
            get
            {
                return _destory;
            }
            set
            {

                _destory = value;
            }
        }

        virtual public string processName
        {
            get
            {
                return "LGAvatar";
            }
            set
            {
                _processName = value;
            }

        }

        public bool pause
        {
            get
            {
                return _pause;
            }
            set
            {

                _pause = value;
            }
        }

        virtual public void updateProcess(float tmSlice)
        {
        }
    }
}