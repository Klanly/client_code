using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class LGAvatarGameInst : LGAvatar
    {
        public LGAvatarGameInst(gameManager m)
            : base(m)
        {
        }
        public const uint SKILL_TAR_SELF = 1;//目标技能 对自己施放
        public const uint SKILL_TAR_AILY = 2;//目标技能 对盟友施放
        public const uint SKILL_TAR_ENEMY = 4;//目标技能 对敌人施放
        public const uint SKILL_TAR_OTHER = 8;//目标技能 对别人施放


        protected const uint SKILL_INTERVAL = 1;//客户端 默认施放技能的间隔 0.1秒

        protected LGAvatarGameInst _selectLgAvatar;

        private bool _showSkill = true;
        private bool _only_self_skill_effect = true;

        protected Variant _states = new Variant();

        protected float _skillcd = 5;//公共CD
        protected bool _isCastingSkill = false;
        protected float _castingTm = 1000;
        protected float _curCastingTm = 0;


        override public void init()
        {
            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_PLAYER_RESPAWN,
            //    onRespawn
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_DIE,
            //    onDie
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_STOP_ATK,
            //    OnMsgStopAttack
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_RMV_STATE,
            //    RemoveStates
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_ADD_STATE,
            //    AddStates
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_SINGLE_SKILL_RES,
            //    single_skill_res
            //);
            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_BSTATE_CHANGE,
            //    BlessChange
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_CASTING_SKILL_RES,
            //    on_casting_skill
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_CAST_SKILL_RES,
            //    OnCastSkillRes
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_CANCEL_CASTING_SKILL_RES,
            //    on_cancel_casting_skill
            //);


            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_ON_CAST_GROUND_SKILL,
            //    cast_ground_skill
            //);

            //this.g_mgr.g_netM.addEventListener(
            //    GAME_EVENT.ON_LVL_UP,
            //    onLvlUpRes
            //);

            // 点击的处理，如黑龙卷轴的处理
            this.addEventListener(
                GAME_EVENT.SPRITE_ON_CLICK,
                onClick);

            base.init();
        }



        private void rmvEventListener()
        {
            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_DIE,
            //    onDie
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_STOP_ATK,
            //    OnMsgStopAttack
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_RMV_STATE,
            //    RemoveStates
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_ADD_STATE,
            //    AddStates
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_SINGLE_SKILL_RES,
            //    single_skill_res
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_BSTATE_CHANGE,
            //    BlessChange
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_CASTING_SKILL_RES,
            //    on_casting_skill
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_CAST_SKILL_RES,
            //    OnCastSkillRes
            //);

            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_CANCEL_CASTING_SKILL_RES,
            //    on_cancel_casting_skill
            //);


            //this.g_mgr.g_netM.removeEventListener(
            //    PKG_NAME.S2C_ON_CAST_GROUND_SKILL,
            //    cast_ground_skill
            //);


            //this.g_mgr.g_netM.removeEventListener(
            //    GAME_EVENT.ON_LVL_UP,
            //    onLvlUpRes
            //);

            this.removeEventListener(
                GAME_EVENT.SPRITE_ON_CLICK,
                onClick);

            onRmoveEventListener();
        }
        virtual protected void onRmoveEventListener()
        {

        }

        public int hp;
        public int max_hp;
        private Variant _viewInfo;
        override public Variant viewInfo
        {//子类重写
            get
            {
                return _viewInfo;
            }
            set
            {
                if (value.ContainsKey("hp"))
                {
                    hp = value["hp"];

                    if (value.ContainsKey("max_hp"))
                        max_hp = value["max_hp"];
                    else if (value.ContainsKey("battleAttrs"))
                        value["max_hp"] = max_hp = value["battleAttrs"]["max_hp"];
                }



                _viewInfo = value;
            }
        }
        override public Variant data
        {//子类重写
            get
            {
                return _viewInfo;
            }
            set
            {
                _viewInfo = value;
            }
        }

        public void refreshY()
        {
            grAvatar.updateY();
        }

        private GameObject _pGameobject;
        public GameObject pGameobject
        {
            get
            {


                if (_pGameobject == null)
                {
                    if (grAvatar == null || grAvatar.m_char == null || grAvatar.m_char.gameObject == null)
                        return null;
                    _pGameobject = grAvatar.m_char.gameObject.transform.parent.gameObject;
                }

                return _pGameobject;
            }
        }

        private GameObject _gameObj;
        public GameObject gameObj
        {
            get
            {
                if (_gameObj == null)
                    _gameObj = grAvatar.m_char.gameObject;
                return _gameObj;
            }
        }

        public Vector3 unityPos
        {
            get
            {
                if (pGameobject == null)
                    return UnityEngine.Vector3.zero;


                return pGameobject.transform.position;
            }

        }


        override public float x
        {
            //set
            //{
            //    viewInfo["x"]._float = value;
            //}
            get
            {
                if (pGameobject != null)
                    return pGameobject.transform.position.x;

                return viewInfo["x"]._float;
            }
        }
        override public float y
        {
            //set
            //{
            //    viewInfo["y"]._float = value;
            //}
            get
            {
                if (pGameobject != null)
                    return pGameobject.transform.position.z;

                return viewInfo["y"]._float ;
            }
        }

        public int max_dp
        {
            get
            {
                return data["max_dp"];
            }
        }
        public int mp
        {
            get
            {
                if (!data.ContainsKey("mp"))
                {
                    return 1000;
                }
                return data["mp"];
            }
        }
        public int max_mp
        {
            get
            {
                if (!data.ContainsKey("max_mp"))
                {//todo 
                    return 1000;
                }
                return data["max_mp"];
            }
        }
        public int dp
        {
            get
            {
                if (!data.ContainsKey("dp"))
                {
                    return 1000;
                }
                return data["dp"];
            }
        }


        public float skillcd
        {
            get
            {
                return _skillcd;
            }
        }

        public bool needRefreshY = true;
        public void tryRefrershY()
        {
            if (!needRefreshY)
                return;

            if(grAvatar==null)
                return;


            needRefreshY = !grAvatar.updateY();
        }


        public Variant getStates()
        {
            return _states;
        }
        //virtual protected void onLvlUpRes(GameEvent e)
        //{
        //    Variant info = e.data;
        //    if (info["iid"]._uint != this.getIid())
        //        return;
        //}
        //virtual protected void RemoveStates(GameEvent e)
        //{
        //    //if (_states.Count == 0) return;
        //    //Variant ids = e.data;
        //    //for( int i = 0; i < ids.Count; i++ )
        //    //{
        //    //    for(int j=0; i< this._states.length; ++i)
        //    //    {
        //    //        var state:Object = _states[i];		
        //    //        if(state.id != state_id) continue;

        //    //        var eff:LGIGraphEffect = state.eff as LGIGraphEffect;				
        //    //        if(eff != null) eff.release();

        //    //        _states.splice(i,1);
        //    //        var desc:Object = state.desc;
        //    //        if(desc && desc.hasOwnProperty("avatar"))
        //    //        {
        //    //            removeRepAvatar( REPT_BUFFA, desc );			
        //    //        }
        //    //        break;
        //    //    }
        //    //}
        //}



        //virtual protected void BlessChange(GameEvent e)
        //{

        //}

        virtual public int attackRange(Variant skilconf = null)
        {
            int rangsk = getSkillCastRang(skilconf);
            return GameConstant.DEF_ATTACK_RANGE + rangsk;
        }

        public bool isInAttackRange(LGAvatar lga)
        {
            return IsInRange(lga, attackRange(), false);
        }

        public bool isInSkillRange(LGAvatar lga, SkillData sdta)
        {
            return IsInRange(lga, sdta.range, false);
        }

        public bool isInSkillRange(LGAvatar lga, Variant skilconf)
        {
            int rangsk = getSkillCastRang(skilconf);
            int rang = GameConstant.DEF_ATTACK_RANGE + rangsk;

            return IsInRange(lga, rang, false);
        }
        public bool isInSkillRange(float x, float y, Variant skilconf)
        {
            int rangsk = getSkillCastRang(skilconf);
            int rang = GameConstant.DEF_ATTACK_RANGE + rangsk;

            return IsInRange(x, y, rang, false);
        }
        public int getSkillCastRang(Variant skilconf)
        {
            return skilconf != null && skilconf.ContainsKey("range") ? skilconf["range"]._int : 0;
        }

        ////  ==== self  ==== 
        //virtual protected void OnCastSkillRes(GameEvent e)
        //{
        //}
        //virtual protected void on_cancel_casting_skill(GameEvent e)
        //{

        //}



        ////  ==== self end ==== 
        //virtual protected void cast_ground_skill(GameEvent e)
        //{
        //    Variant data = e.data;
        //    uint iid = data["frm_iid"]._uint;
        //    if (iid != getIid()) return;
        //    uint sid = data["sid"]._uint;

        //    _cast_success(data);

        //    if (!can_play_skill_effect(iid, sid))
        //    {
        //        return;
        //    }
        //    LGAvatarGameInst to_lc = null;
        //    if (data.ContainsKey("to_iid"))
        //    {
        //        to_lc = get_Character_by_iid(data["to_iid"]);
        //    }
        //    Variant rotate = genConf.getRotateSkill(sid);
        //    LGAvatarGameInst frm_lc = get_Character_by_iid(iid);
        //    float angle = getEffAngle(rotate, frm_lc, to_lc);

        //    //在目标点施放一次性特效
        //    Variant sdata = svrSkillConf.get_skill_conf(sid);
        //    if (sdata != null && sdata.ContainsKey("fly"))
        //    {
        //        //Variant fly = sdata[ "fly" ][0];
        //        //if( data.ContainsKey( "to_iid" ) )
        //        //{
        //        //    if(to_lc == null)
        //        //        return;

        //        //    if(  fly.ContainsKey("end_eff") && fly[ "end_eff" ]._str != "" )
        //        //    {
        //        //        to_lc.addEff( fly[ "end_eff" ]._str  );
        //        //    }
        //        //}
        //        //else if( fly.ContainsKey("end_eff") && fly[ "end_eff" ]._str != "" )
        //        //{
        //        //    this.g_mgr.dispatchEvent(
        //        //        GameEvent.Create(GAME_EVENT.MAP_ADD_SHOW_EFF, this,
        //        //        GameTools.createGroup("effid", fly["end_eff"]._str, "x", data["x"]._float, "y", data["y"]._float, "angle", angle))
        //        //    );

        //        //    //(g_mgr.g_sceneM as muGRClient).createEffect( 
        //        //    //    fly[ "end_eff" ]._str,
        //        //    //    data["x"]._int * GameConstant.GEZI,
        //        //    //    data["y"]._int * GameConstant.GEZI
        //        //    //); 
        //        //}
        //    }
        //    else
        //    {
        //        string eff = "";
        //        if (sdata != null && sdata.ContainsKey("tres"))
        //        {
        //            Variant tres = sdata["tres"];
        //            if (tres != null && tres.ContainsKey("tar_eff"))
        //            {
        //                eff = tres["tar_eff"]._str;
        //            }
        //        }

        //        if (eff != "")
        //        {
        //            addEff(eff, angle);

        //            //Variant rotate = genConf.getRotateSkill(data.sid);
        //            //if(rotate)
        //            //{
        //            //	lgMovCharacter frm_lc = _lgclient.logicInGame.map.get_Character_by_iid(data.frm_iid);
        //            //	Point point;
        //            //	if(1==rotate.tp)//指定目标
        //            //	{
        //            //		point = lc.GetRotateEffPoint();
        //            //		effObj = _lgclient.grClient.createEffect(eff,point.x,point.y,1) as LGIGraphEffect;	
        //            //	}
        //            //	else
        //            //	{					
        //            //		point = frm_lc.GetRotateEffPoint();
        //            //		effObj = _lgclient.grClient.createEffect(eff,point.x,point.y,1) as LGIGraphEffect;
        //            //	}
        //            //	if( effObj )
        //            //	{
        //            //		rotate_eff( effObj, rotate, frm_lc, lc );
        //            //	}
        //            //}
        //            //else if(eff)
        //            //{
        //            //	_lgclient.grClient.createTargetEffect( eff, lc.grCha, 1 );
        //            //}
        //        }
        //    }
        //}

        public void _cast_success(Variant data)
        {
            Variant showinfo = this.data;
            uint sid = data["sid"]._uint;
            int sex = 0;
            if (showinfo.ContainsKey("sex"))
            {
                sex = showinfo["sex"]._int;
            }

            //debug.Log("使用技能 " + sid);

            if (data.ContainsKey("x"))
                playSkill(sid, sex, data.ContainsKey("to_iid") ? data["to_iid"]._uint : 0, data["x"], data["y"]);
            else
                playSkill(sid, sex, data.ContainsKey("to_iid") ? data["to_iid"]._uint : 0);
        }

        virtual protected void playSkill(uint sid, int sex, uint toIID, float tx = 0, float ty = 0)
        {
            //debug.Log("使用技能id= " + sid);
            if (destroy)
                return;

            SkillXmlData skillxml = SkillModel.getInstance().getSkillXml(sid);
            LGAvatarGameInst to_lc = null;

            if (skillxml.target_type == 1)
            {
                to_lc = get_Character_by_iid(toIID);
                if (skillxml.useJump)
                {
                    if (to_lc != null)
                        jump(skillxml, to_lc);
                }
                else
                {
                    if (toIID > 0)
                    {
                        if (to_lc != null && to_lc != this)
                        {
                            RotationToPt(to_lc.x, to_lc.y); //有目标才旋转方向
                        }
                    }
                    playSkillAction(sid, sex == 0 ? skillxml.eff : skillxml.eff_female, to_lc);
                }
            }
            if (skillxml.target_type == 2)
            {
                if (skillxml.useJump)
                {
                    jump(skillxml, tx, ty);
                    playSkillAction(sid, null);
                }
                else
                {
                    RotationToPt(tx, ty); //有目标才旋转方向
                    playSkillAction(sid, sex == 0 ? skillxml.eff : skillxml.eff_female);
                }
            }
        }

        public void playSkillAction(uint sid, string eff = null, LGAvatarGameInst to_lc = null)
        {
            if (destroy)
                return;

            _isCastingSkill = true;
            _curCastingTm = this.g_mgr.g_netM.CurServerTimeStampMS;

            //  Variant sdata = SkillConf.instance.getSkillid(sid+"");
            //if(!sdata.ContainsKey("stop") || sdata["stop"]._bool )
            {
                if (IsMoving())
                {
                    stop();
                }
            }

            if (to_lc != null)
            {
                if (to_lc != this)
                {
                    RotationToPt(to_lc.x, to_lc.y); //有目标才旋转方向
                }
            }
            //else if (data.ContainsKey("x") && data.ContainsKey("y"))
            //{
            //    RotationToPt(data["x"]._int * GameConstant.GEZI, data["y"]._int * GameConstant.GEZI);
            //}


            //播放施法动作

            Variant showinfo = this.data;
            //int carr = 0;
            //if (showinfo.ContainsKey("carr"))
            //{
            //    carr = showinfo["carr"]._int;
            //}
            //  sid = 1001;//临时 by lucisa



            play_animation(sid + "", GameConstant.ANI_LOOP_FLAG_NOT_LOOP);


            //  if (can_play_skill_effect(getIid(), sid))
            if (eff != null)
            {


                MapEffMgr.getInstance().play(eff, pGameobject.transform, 450f - lg_ori_angle, 0f);
                // addEff(eff, ori);
            }




            ////技能音效
            //   string soundId = genConf.getSkillSound(sid);
            //_lgclient.mediaClient.PlaySound(sid,false);
            //this.g_mgr.g_uiM.dispatchEvent(
            //    GameEvent.Create(GAME_EVENT.LG_MEDIA_PLAY, this,
            //    GameTools.createGroup("sid", soundId, "loop", false))
            //);
            //播放技能效果处理
            //   _skill_action(data);
        }

        //protected void OnMsgStopAttack(GameEvent e)
        //{
        //    OnMsgStopAttack();
        //}

        //virtual protected void OnMsgStopAttack()
        //{
        //    stop(null);
        //}

        virtual protected void onClick(GameEvent e)
        {
            operationLgAvatarSet(this);
        }

        protected void operationLgAvatarSet(LGAvatarGameInst lga)
        {
            if (lgMainPlayer.selectTarget != null)
            {
                //lgMainPlayer.selectTarget.removeEff(GameConstant.SELECT_EFF_ID);
            }
            if (!isMainPlayer())
            {
                lgMainPlayer.selectTarget = lga;
            }

            if (_selectLgAvatar != null)
            {
                _selectLgAvatar.removeEventListener(GAME_EVENT.SPRITE_DISPOSE, operationLgaDie);
                _selectLgAvatar.removeEventListener(GAME_EVENT.SPRITE_DIE, operationLgaDie);
            }

            _selectLgAvatar = lga;
            if (_selectLgAvatar == null) return;
            _selectLgAvatar.addEventListener(GAME_EVENT.SPRITE_DISPOSE, operationLgaDie);
            _selectLgAvatar.addEventListener(GAME_EVENT.SPRITE_DIE, operationLgaDie);
            if (!_selectLgAvatar.isMainPlayer())
            {
                // _selectLgAvatar.addEff(GameConstant.SELECT_EFF_ID, 0, true);
            }
        }
        virtual protected void onOperationLgaClear()
        {

        }
        private void operationLgaDie(GameEvent e)
        {
            operationLgAvatarClear();
        }
        protected void operationLgAvatarClear()
        {
            if (_selectLgAvatar == null) return;
            //if (!_selectLgAvatar.isMainPlayer())
            //{
            //    _selectLgAvatar.removeEff(GameConstant.SELECT_EFF_ID);
            //}

            onOperationLgaClear();

            _selectLgAvatar.removeEventListener(GAME_EVENT.SPRITE_DIE, operationLgaDie);
            _selectLgAvatar = null;
        }

        public void onDie(Variant data)
        {
            Die(data);
            grAvatar.curHp = 0;
        }


        private void onAttack(GameEvent e)
        {
            uint iid = e.data["frm_iid"]._uint;

            if (iid != getIid()) return;

            addAttack(e.data["to_iid"]._uint, e.data);
        }

        protected void addAttack(uint tar_iid, Variant data = null)
        {

        }

        public void onRespawn(Variant info)
        {
            Respawn(info);
            respawn(info);
        }

        virtual protected void respawn(Variant data)
        {//子类重写

        }



        protected bool isTrangFan(Variant skilconf)
        {
            return false;
        }






        public void jump(SkillXmlData jumpDta, LGAvatarGameInst to_lc)
        {
            this.stop();
            RotationToPt(to_lc.x, to_lc.y);
            Transform trans = to_lc.gameObj.transform;
            pGameobject.transform.position = trans.position;
            setPos(to_lc.x, to_lc.y);


            Vector3 begin = gameObj.transform.position;
            Vector3 vec = trans.position;
            float max = Vector3.Distance(begin, vec);
            Vector3 end = Vector3.Lerp(begin, vec, (float)(max - 1.5) / max);

            if (jumpDta.jump_canying != "null")
                MapEffMgr.getInstance().playMoveto(jumpDta.jump_canying, begin, end, 0.4f);
            if (jumpDta.eff != "null")
                MapEffMgr.getInstance().play(jumpDta.eff, trans.position, trans.rotation, 0f);

        }

        public void jump(SkillXmlData jumpDta, float gezi_distance)
        {
            this.stop();

            LGMap lgm = GRClient.instance.g_gameM.getObject(OBJECT_NAME.LG_MAP) as LGMap;
            Vec2 tp = lgm.getFarthestGPosByOri(x, y, lg_ori_angle * (float)Math.PI / 180, gezi_distance);
            setPos(tp.x * GameConstant.GEZI, tp.y * GameConstant.GEZI);
            //if (jumpDta.jump_canying != "null")
            //    MapEffMgr.getInstance().playMoveto(jumpDta.jump_canying, gameObj.transform.position, trans.position, 0.4f);
            //if (jumpDta.eff != "null")
            //    MapEffMgr.getInstance().play(jumpDta.eff, trans.position, trans.rotation);
        }
        public void jump(SkillXmlData jumpDta, float tx, float ty)
        {
            this.stop();
            Vector3 begin = gameObj.transform.position;
            Vector3 vec = new Vector3(tx / GameConstant.PIXEL_TRANS_UNITYPOS, GRClient.instance.getZ(tx, ty), ty / GameConstant.PIXEL_TRANS_UNITYPOS);
            float max = Vector3.Distance(begin, vec);
            Vector3 end = Vector3.Lerp(begin, vec, (float)(max - 1.5) / max);
            if (jumpDta.jump_canying != "null")
                MapEffMgr.getInstance().playMoveto(jumpDta.jump_canying, begin, end, 0.4f);
            if (jumpDta.eff != "null")
                MapEffMgr.getInstance().play(jumpDta.eff, vec, gameObj.transform.rotation, 0f);
            setPos(tx, ty);
        }



        //private Variant fly_array = new Variant();
        //public void fly(Variant data, LGAvatar lc = null, Variant skillData = null)
        //{
        //    if (!can_play_skill_effect(data["frm_iid"]._uint, data["sid"]._uint))
        //    {
        //        return;
        //    }
        //    Variant skill_data;
        //    uint sid = data["sid"]._uint;
        //    if (skillData == null)
        //    {
        //        skill_data = (this.g_mgr.g_gameConfM as muCLientConfig).svrSkillConf.get_skill_conf(sid);
        //    }
        //    else
        //    {
        //        skill_data = skillData;
        //    }

        //    if (skill_data == null || !skill_data.ContainsKey("fly"))
        //    {
        //        return;
        //    }
        //    LGAvatarGameInst to_lc = null;
        //    LGAvatarGameInst frm_lc = get_Character_by_iid(data["frm_iid"]._uint) as LGAvatarGameInst;
        //    if (data.ContainsKey("to_iid"))
        //    {
        //        to_lc = get_Character_by_iid(data["to_iid"]._uint) as LGAvatarGameInst;
        //    }
        //    if (to_lc != null)
        //    {
        //        lc = to_lc;
        //    }
        //    else
        //    {
        //        lc = frm_lc;
        //    }
        //    //飞行道具
        //    float flytm = 1000000;//1s
        //    if (data.ContainsKey("fly"))
        //    {
        //        flytm = data["fly"]["tm"]._float;
        //    }
        //    Variant fly = new Variant();
        //    if (skill_data["tar_tp"]._uint == 1)
        //    {
        //        //指定目标
        //        if (to_lc == null)
        //            return;
        //        fly["to_lc"] = new Variant();
        //        fly["to_lc"]._val = to_lc;
        //    }
        //    else if (skill_data["tar_tp"]._uint == 2)
        //    {
        //        //无目标
        //        fly["tar_pos"] = new Variant();
        //        fly["tar_pos"]._val = new Vec2(data["fly"]["to_x"]._float, data["fly"]["to_y"]._float);
        //    }
        //    else
        //    {
        //        fly["tar_pos"] = new Variant();
        //        fly["tar_pos"]._val = new Vec2(lc.x, lc.y);
        //    }
        //    //Vec2 effPoint = lc.GetRotateEffPoint();
        //    Vec2 effPoint = new Vec2(lc.x, lc.y);
        //    if (skill_data["fly"][0].ContainsKey("fly_eff"))
        //    {
        //        fly["fly_eff"] = skill_data["fly"][0]["fly_eff"]._str;
        //        fly["effPoint"] = new Variant();
        //        fly["effPoint"]._val = effPoint;
        //        //fly["fly_eff"] = new Variant();
        //        //fly["fly_eff"]._val = (g_mgr.g_sceneM as muGRClient).createEffect(
        //        //    skill_data["fly"]["fly_eff"]._str,
        //        //    effPoint.x * GameConstant.GEZI,
        //        //    effPoint.y * GameConstant.GEZI
        //        //);
        //    }
        //    Vec2 tar_pos = new Vec2();
        //    if (skill_data["tar_tp"]._uint == GameConstant.CAST_SKILL_TYPE_TARGET)
        //    {
        //        //指定目标
        //        Vec2 toPoint = new Vec2(to_lc.x, to_lc.y);
        //        tar_pos.x = to_lc.x;
        //        tar_pos.y = toPoint.y;
        //    }
        //    else if (skill_data["tar_tp"]._uint == GameConstant.CAST_SKILL_TYPE_POINT || skill_data["tar_tp"]._uint == GameConstant.CAST_SKILL_TYPE_NOTAR)
        //    {
        //        //无目标
        //        tar_pos.x = (fly["tar_pos"]._val as Vec2).x;
        //        tar_pos.y = (fly["tar_pos"]._val as Vec2).y;
        //    }

        //    if (fly.ContainsKey("fly_eff"))
        //    {
        //        //Vec2 v1 = new Vec2(tar_pos.x - effPoint.x, tar_pos.y - effPoint.y);
        //        //float angle = (float)v1.angleBetween(GameConstant.EFF_DEF_ORI);
        //        //if (tar_pos.x < effPoint.x)
        //        //{
        //        //    angle = -angle;
        //        //}
        //        Variant rotate = genConf.getRotateSkill(sid);
        //        float angle = getEffAngle(rotate, frm_lc, to_lc);
        //        fly["angle"] = angle;
        //        //(fly["fly_eff"]._val as IGREffectParticles).rotY = (float)(angle * 180 / Math.PI);
        //    }

        //    fly["end_eff"] = skill_data["fly"][0]["end_eff"];
        //    fly["lasttm"] = this.g_mgr.g_netM.CurServerTimeStampMS;
        //    fly["flytm"] = flytm;
        //    this.g_mgr.dispatchEventCL(
        //            OBJECT_NAME.LG_MAP,
        //            GameEvent.Create(GAME_EVENT.MAP_ADD_FLY_EFF, this,
        //            fly)
        //        );

        //    //fly_array.pushBack(fly);
        //}
        //private function fly_process():void{
        //	if(fly_array.length <= 0)
        //		return;

        //	float cur_tm = this._lgclient.session.connection.cur_server_tm;
        //	for(uint i = 0;i < fly_array.length;i++){
        //		Variant fly = fly_array[i];
        //		if(fly == null)
        //			continue;
        //		float passtm = cur_tm - fly.lasttm;
        //		LGIGraphEffect fly_eff = fly.fly_eff;
        //		float flytm = fly.flytm;
        //		if(passtm >= flytm){

        //			//到了
        //			//停止飞行特效，播放end特效
        //			if(fly_eff != null)
        //				fly_eff.release();

        //			if( can_play_skill_effect() )
        //			{
        //				if(fly.ContainsKey("to_lc") && fly.end_eff && fly.to_lc != null && fly.to_lc.grCha)
        //				{		
        //					_lgclient.grClient.createTargetEffect(fly.end_eff,fly.to_lc.grCha,1);
        //				}
        //				else if(fly.ContainsKey("tar_pos") && fly.end_eff)
        //				{
        //					_lgclient.grClient.createEffect(fly.end_eff,fly.tar_pos.x,fly.tar_pos.y,1);
        //				}	
        //			}											
        //			fly_array.splice(i,1);
        //			i--;
        //		}
        //		else
        //		{	//没到
        //			//更新飞行特效位置，（和方向）
        //			if(fly_eff == null)
        //				continue;
        //			int f_x = 0;
        //			int f_y = 0;

        //			if(fly.ContainsKey("to_lc")){
        //				lgMovCharacter to_lc = fly.to_lc;
        //				if(to_lc != null)
        //				{
        //					Point toPoint = to_lc.GetRotateEffPoint();
        //					f_x = to_lc.x;
        //					//								f_y = to_lc.y - _flypos;
        //					f_y = toPoint.y;
        //				}
        //			}
        //			else if(fly.ContainsKey("tar_pos")){
        //				Point tar_pos = fly.tar_pos;
        //				f_x = tar_pos.x;
        //				f_y = tar_pos.y;
        //			}

        //			float lx = (f_x - fly_eff.x) * (passtm/flytm);
        //			float ly = (f_y - fly_eff.y) * (passtm/flytm);
        //			fly_eff.x += lx;
        //			fly_eff.y += ly;
        //			fly.lasttm = cur_tm;
        //			fly.flytm -= passtm;
        //		}
        //	}
        //}

        //public void skill_process(float elapsed)
        //{
        //	fly_process();
        //	telep_process();
        //	fmv_process();
        //	jump_process();
        //}

        //private float _fmv_tm = 200;	
        //private void fmv_process()
        //{
        //	if(null==fmvArr || 0==fmvArr.length)
        //	{
        //		return;
        //	}

        //	for(int i=0;i<fmvArr.length;++i)
        //	{
        //		Variant fmv = fmvArr[i];
        //		if(!fmv.fmv.fmving)
        //		{
        //			continue;
        //		}
        //		float tm_now = getTimer();
        //		float pass_tm = tm_now - fmv.fmv.fmv_start_tm;
        //		if(pass_tm >= _fmv_tm)
        //		{
        //			fmv.fmv.fmving = false;

        //			fmv.lc.SetState("stand");
        //			fmv.lc.SetPos(fmv.fmv.fmv_end.x,fmv.fmv.fmv_end.y);
        //			//					fmv.lc.fmving = false;
        //			fmvArr.splice(i,1);
        //			--i;
        //			continue;
        //		}
        //		float persent = pass_tm / _fmv_tm;

        //		fmv.lc.SetPos(fmv.fmv.fmv_start.x+(fmv.fmv.fmv_end.x-fmv.fmv.fmv_start.x)*persent,fmv.fmv.fmv_start.y+(fmv.fmv.fmv_end.y-fmv.fmv.fmv_start.y)*persent);
        //	}
        //}
        //private Array fmvArr = new Array();
        //public void fmv(Variant data,lgMovCharacter lc)
        //{
        //	//被迫移动
        //	Variant fmv = data.fmv;
        //	Variant fmvData = new Object();
        //	if(t_ht_away==""){
        //		t_ht_away = LanguagePack.getLanguageText("SkillManager","t_ht_away");
        //	}
        //	if(t_ht_catch==""){
        //		t_ht_catch = LanguagePack.getLanguageText("SkillManager","t_ht_catch");
        //	}			
        //	lc.Stop();
        //	//			this._lc.setPos(fmv.to_x,fmv.to_y); 我们来个过程。。。比冒字好看多了
        //	fmvData.fmv_start = new Point(lc.x,lc.y);
        //	fmvData.fmv_end = new Point(fmv.to_x,fmv.to_y);
        //	fmvData.fmving = true;
        //	fmvData.fmv_start_tm = getTimer();

        //	//lc.setState("fmving");
        //	if(fmv.ContainsKey("dir"))
        //	{
        //		if(fmv.dir == 0)
        //		{
        //			//击退
        //			lc.OnHurt(t_ht_away);
        //		}
        //		else if(fmv.dir == 1)
        //		{
        //			//死亡之握
        //			lc.OnHurt(t_ht_catch);
        //		}
        //	}
        //	fmvArr.push({lc lc,fmvData fmv});
        //}
        //private string t_ht_away = "";
        //private string t_ht_catch = "";

        //override protected void onDispose()
        //{
        //	//删除飞行特效
        //	for(uint i = 0;i < fly_array.length;i++){
        //		Variant fly = fly_array[i];
        //		if(fly == null)
        //			continue; 
        //	}
        //	fly_array.length = 0;
        //}


        //public Vec2 GetRotateEffPoint(LGAvatar tar, float offper = 50)
        //{
        //    Vec2 ret = new Vec2(tar.x, tar.y);
        //    //if( tar && tar.grCha )
        //    //{
        //    //	Rectangle boundRect = tar.grCha.boundRect;	
        //    //	if( boundRect )
        //    //	{
        //    //		ret.y -= int( tar.grCha.offHeight + boundRect.height*offper/100 );
        //    //	}
        //    //}
        //    return ret;
        //}

        public float getEffAngle(Variant rotate, LGAvatarGameInst frm_lc, LGAvatarGameInst to_lc)
        {
            if (rotate == null)
                return 0;

            Vec2 v;
            float angle = 0;
            int tp = rotate["tp"]._int;
            if (1 == tp)//指定目标
            {
                if (frm_lc == null || to_lc == null)
                    return angle;

                v = new Vec2(to_lc.x - frm_lc.x, to_lc.y - frm_lc.y);
                angle = (float)(v.angleBetween(GameConstant.EFF_DEF_ORI));

                if (to_lc.x < frm_lc.x)
                {
                    angle = -angle;
                }
            }
            else
            {
                if (frm_lc == null)
                    return angle;

                angle = (float)(Math.PI / 2 - frm_lc.lg_ori_angle * Math.PI / 180);
            }

            return angle;
        }


        public bool can_play_skill_effect(uint from_iid, uint sid = 0)
        {
            if (!_showSkill)
            {
                return false;
            }

            if (_only_self_skill_effect)
            {
                return from_iid == lgMainPlayer.getIid();
            }
            return true;
        }

        //private int m_nhurtStep = 0;
        public void OnHurt(Variant data, LGAvatarGameInst frm_c)
        {
            //playOnhurtEff()
            bool isUser = isMainPlayer();
            bool isFromUser = false;
            bool isFromHero = false;
            if (frm_c != null)
            {
                isFromUser = frm_c.isMainPlayer();
                isFromHero = (frm_c is LGAvatarHero && ((LGAvatarHero)frm_c).isUserOwnHero);

                //if (!isUser || lgMainPlayer.stateTag != GameConstant.ST_MOVE)
                //    RotationToPt(frm_c.x, frm_c.y);
            }

            int skillid = data.ContainsKey("skill_id") ? data["skill_id"]._int : 0;
            //debug.Log("技能的伤害 技能ID = " + skillid);

            bool isdie = data.ContainsKey("isdie") ? data["isdie"]._bool : false;
            //bool isHpDmg = false;
            if (data.ContainsKey("dmg") && data["dmg"]._int != 0)
            {
                int dmg = -data["dmg"]._int;
                int hprest = 0;
                if (data.ContainsKey("hprest"))
                    hprest = data["hprest"];

                //isHpDmg = true;
                if (isUser)
                {
                    PlayerModel.getInstance().modHp(hprest);

                    SkillXmlData d = SkillModel.getInstance().getSkillXml((uint)skillid);

                    if (d.hitfall)
                    {
                        setMeshAni("hitfall", 1);
                    }
                }
                else if (this is LGAvatarHero)
                {
                    uint id = this.getMid();
                    //herohead.instance.refreshHeroHp(id, hprest);
                }

                modHp(hprest);
            }
            else if (data.ContainsKey("hpadd"))
            {
                int add = data["hpadd"]._int;
                int hprest = data["hprest"];
                modHp(data["hprest"]);
                //isHpDmg = true;
                if (isUser)
                    PlayerModel.getInstance().modHp(hprest);
            }

            //bool fightText = false;
            //if (isUser)
            //{
            //    data["ft"] = FightText.USER_TEXT;
            //    fightText = true;
            //}
            //else if (isFromUser)
            //{
            //    data["ft"] = FightText.ENEMY_TEXT;
            //}
            //else if (isFromHero)
            //{
            //    data["ft"] = FightText.HERO_TEXT;
            //}



            if (isdie)
            {
                if (!isFromUser && !isFromHero)
                    Die(data);
            }

            if (_isCastingSkill && _curCastingTm + _castingTm < this.g_mgr.g_netM.CurServerTimeStampMS)
            {
                _isCastingSkill = false;
            }


            //if (
            //    // !_isCastingSkill &&
            //    stateTag != GameConstant.ST_MOVE
            //    )
            {
                if (!isUser && (isFromUser || isFromHero))
                    data["hurteff"] = true;//SkillModel.getInstance().getSkillXml();

            }

            //if (false == isUser)
            //{
            //    if (m_nhurtStep == 0)
            //    {
            //        m_nhurtStep = 1;
            //        grAvatar.setAni("hurt", false);
            //    }
            //    else
            //    {
            //        m_nhurtStep = 0;
            //        grAvatar.setAni("dead", false);
            //    }
            //}


            //if (fightText)
            //    grAvatar.onHurt(data);
            //else if (isFromUser || isFromHero)
            //{
            //    data["isdie"] = isdie;
            //    //  debug.Log("发生伤害：" + isFromUser + " " + isFromHero + " " + data["frm_iid"] + " " + " " + skillid + " " + NetClient.instance.CurServerTimeStamp);
            // //   AttackPointMgr.instacne.setHurt(frm_c, this, data, isFromUser);
            //}

        }

        //private void OnSkillHurt(int hp_dmg, int mp_dmg, Variant data)
        //{
        //    modHp(-hp_dmg);

        //    //统一处理死亡字段  isdie、die
        //    bool isdie = false;
        //    if (data.ContainsKey("isdie"))
        //    {
        //        isdie = data["isdie"]._bool;
        //    }
        //    else
        //    {
        //        isdie = data["die"]._bool;
        //    }

        //    if (isdie)
        //    {
        //        //死亡清除目标
        //        Die(data);
        //    }
        //}
        public void modSpeed(int v)
        {
            this.data["speed"]._int = v;
        }

        public void modHp(int curhp)
        {
            int lasthp = this.hp;
            //  int v = this.data["hp"]._int + add;
            this.hp = curhp;
            if (this.hp > max_hp)
            {
                this.hp = max_hp;
            }

            if (grAvatar != null)
            {
                grAvatar.setUIHp(this.hp, this.max_hp);
            }

            //Variant hpData = GameTools.createGroup("cur", this.hp, "max", this.max_hp, "last", lasthp);
            //dispatchEvent(
            //    GameEvent.Create(GAME_EVENT.SPRITE_SET_HP, this, hpData)
            //);
        }


        public void modMp(int add)
        {

            int v = this.data["mp"]._int + add;
            if (v > this.data["max_mp"]._int)
            {
                v = this.data["max_mp"]._int;
            }

            this.data["mp"]._int = v;

            onModMp(add);
        }
        virtual protected void onModMp(int add)
        {
        }
        public void modDp(int add)
        {
            int v = this.data["dp"]._int + add;
            if (v > this.data["max_dp"]._int)
            {
                v = this.data["max_dp"]._int;
            }
            onModDp(add);
            this.data["dp"]._int = v;
        }

        virtual protected void onModDp(int add)
        {
        }

        public void modMaxDp(int max_v)
        {
            float persent = this.dp / this.max_dp;
            this.data["max_dp"] = max_v;

            this.data["dp"] = (int)(max_v * persent);
        }

        public void modMaxHp(int max_v, int cur_v = -1)
        {
            int hp = cur_v < 0 ? this.hp : cur_v;

            //    float persent = this.hp / this.max_hp;
            this.data["max_hp"] = max_v;

            //    this.data["hp"] = (int)(max_v * persent);
            this.max_hp = max_v;

            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modHp", "model/PlayerModel", hp,PlayerModel.getInstance ().max_hp);
            //if (SelfRole._inst != null) {
            //    PlayerNameUIMgr.getInstance().refreshHp(SelfRole._inst, hp, max_v);
            //}
            //if (grAvatar != null)
            //{
            //    grAvatar.setUIHp(hp, this.max_hp);
            //}
        }




        public bool in_pczone
        {
            get
            {
                return this.data["in_pczone"]._bool;
            }
            set
            {

                this.data["in_pczone"] = value;
            }
        }

        public bool follow
        {
            get
            {
                return this.data["follow"]._bool;
            }
            set
            {

                this.data["follow"] = value;
            }
        }


        public bool ghost
        {
            get
            {
                return this.data["ghost"]._bool;
            }
            set
            {

                this.data["ghost"] = value;
            }
        }

        public uint ride_mon
        {
            get
            {
                return this.data["ride_mon"]._uint;
            }
            set
            {

                this.data["ride_mon"] = value;
            }
        }


        virtual public void Die(Variant data)
        {

            removeAllEff();
            setDie();
            stop();
            operationLgAvatarClear();
            rmvEventListener();

            if (grAvatar.m_charShadow != null)
            {
                grAvatar.m_charShadow.SetActive(false);
            }

            dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SPRITE_DIE, this, null)
            );

        }

        public void addMoving(Variant data)
        {

            Variant moveinfo = new Variant();
            if (data == null || !data.ContainsKey("to_x"))
            {
                GameTools.PrintError("addMoving err!");
                return;
            }

            //debug.Log("收到移动消息 " + data.dump());
            float to_x = data["to_x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
            float to_y = data["to_y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
            debug.Log("收到移动消息 " + to_x + "  " + to_y);
            // movePosSingle(to_x, to_y, null);

            //if (Nav == null)
            //    return;
            //Nav.SetDestination(new Vector3(to_x, pGameobject.transform.position.y, to_y), 0.5f);

            setMoveInfo(moveinfo);
        }

        //private NavContorler _nav;
        //public NavContorler Nav
        //{
        //    get
        //    {
        //        if (_nav == null)
        //        {
        //            if (pGameobject == null)
        //                return null;
        //            _nav = pGameobject.AddComponent<NavContorler>();
        //            _nav.roleD = viewInfo;
        //            _nav.playani = navPlayAni;
        //            // _nav.autoPlayAni = !(this is lgSelfPlayer);
        //            _nav.speed = speed / GameConstant.PIXEL_TRANS_UNITYPOS;

        //            if (this is lgSelfPlayer)
        //                _nav.isMain = true;
        //        }
        //        return _nav;
        //    }
        //}
        private void navPlayAni(string ani, bool loop)
        {

            setMeshAni(ani, loop ? 0 : 1);
        }




        //===== move =====
        protected override void onStop()
        {
            //if (Nav != null)
            //    Nav.Stop();
        }


        int _countToSend = 0;
        public void move_byOri(float direction, float maxDis) //现在只有主角的移动在这里处理
        {

            //float x = (float)(pGameobject.transform.position.x + (float)Mathf.Cos(direction * Mathf.PI / 180) * 1.01);
            //float y = (float)(pGameobject.transform.position.z + (float)Mathf.Sin(direction * Mathf.PI / 180) * 1.01);

            //Vector3 vec3 = new Vector3(x, pGameobject.transform.position.y, y);


            ////if (Nav != null)
            ////    Nav.SetDestination(vec3);

            ////this.lg_ori_angle = direction;
            ////_currMoveOri = SPEMoveOri.create(this, lg_ori_angle, maxDis);
            ////this.setlgState(new SPSBase(_currMoveOri));

            //if (_countToSend > 5)
            //{
            //    _countToSend = 0;
            //    MoveProxy.getInstance().sendmoveRadian(pGameobject.transform.position.x * GameConstant.PIXEL_TRANS_UNITYPOS, pGameobject.transform.position.z * GameConstant.PIXEL_TRANS_UNITYPOS, direction, NetClient.instance.CurServerTimeStampMS);
            //}
            //else
            //{
            //    _countToSend++;
            //}
        }

        public void movePos(float tx, float ty)
        {
            Variant par = new Variant();
            //par["targetX"] = tx;
            //par["targetY"] = ty;
            //par["tp"] = GameConstant.CTP_MOVE_POS;
            //SPEMovePos ele = new SPEMovePos(par);
            //ele.lgChar = this;
            //this.setlgState(new SPSBase(ele));
        }


        private Action<Variant> _onMoveReach;
        public void movePosSingle(float tx, float ty, Action<Variant> onMoveReach, Variant userData = null, float range = 0, LGAvatarGameInst target = null)
        {
            //if (Nav == null || IsDie())
            //    return;

            //Nav.SetDestination(new Vector3(tx, pGameobject.transform.position.y, ty), range, onMoveReach, userData, target.pGameobject.transform);

            //if (this is LGAvatarMonster)
            //{
            //    //如果是很近距离的移动，就不要移动了
            //    if (Mathf.Abs(this.x - tx) + Mathf.Abs(this.y - ty) < 64f)
            //    {
            //        //debug.Log("如果是很近距离的移动，就不要移动了");
            //        if (moveSps != null)
            //        {
            //            moveSps.removeEventListener(GAME_EVENT.SPRITE_OP_REACHED, onMovePosSingleReach);
            //            moveSps.dispose();
            //        }

            //        return;
            //    }

            //    //让怪物移动的目标点有一定随机，确保不重叠起来
            //    tx += UnityEngine.Random.Range(-32f, 32f);
            //    ty += UnityEngine.Random.Range(-32f, 32f);
            //}

            //_onMoveReach = onMoveReach;

            //SPEMovePos ele = SPEMovePos.create(this, tx, ty, range, target);
            //if (moveSps != null)
            //{
            //    moveSps.removeEventListener(GAME_EVENT.SPRITE_OP_REACHED, onMovePosSingleReach);
            //    moveSps.dispose();
            //}

            //moveSps = new SPSBase(ele);
            //moveSps.setUserdata(userData);
            //moveSps.addEventListener(GAME_EVENT.SPRITE_OP_REACHED, onMovePosSingleReach);
            //moveSps.sps_start(ele);
        }

        //public void moveMapSingle(uint mapid)
        //{
        //    SPEMoveMap map = SPEMoveMap.create(this, mapid);
        //    SPSBase sps = new SPSBase(map);
        //    sps.addEventListener(GAME_EVENT .SPRITE_OP_REACHED ,onmove);
        //}

        private Dictionary<string, int> dEffing = new Dictionary<string, int>();
        public void addBuffer(int buff)
        {
            string str = "buff_" + buff;

            addEffect(str, str);
        }

        public void removeBuffer(int buff)
        {
            string str = "buff_" + buff;
            removeEffect(str);
        }

        public void removeAllEff()
        {
            foreach (string str in dEffing.Keys)
            {
                removeEffect(str);
            }
            dEffing.Clear();
        }

        public void addEffect(string id, string eff, bool needCache = false)
        {
            if (this.destroy)
                return;

            if (grAvatar != null && grAvatar.inited && data != null)
            {
                string effid = data["iid"]._str + id;
                EffectItem m_effect = MapEffMgr.getInstance().addEffItem(effid, eff, MapEffMgr.TYPE_AUTO, pGameobject.transform);
                m_effect.scale = grAvatar.m_char.scale.x;
                if (!m_effect.isAutoRemove)
                    dEffing[effid] = 1;

            }
            else if (needCache)
            {
                dCacheEffect[id] = eff;
            }
        }

        public void removeEffect(string id)
        {
            if (data == null)
                return;

            string effid = data["iid"]._str + id;
            dEffing.Remove(effid);
            MapEffMgr.getInstance().removeEffItem(effid);
        }


        public Dictionary<string, string> dCacheEffect = new Dictionary<string, string>();
        public Action<LGAvatarGameInst> grLoadedhandle;
        public void onGrLoaded(GRAvatar gr)
        {
            foreach (string id in dCacheEffect.Keys)
            {
                addEffect(id, dCacheEffect[id]);
            }
            dCacheEffect.Clear();

            if (grLoadedhandle != null && !destroy)
            {
                grLoadedhandle(this);
                grLoadedhandle = null;

            }


        }

        virtual protected void onMovePosSingleReach(GameEvent e)
        {
            this.stop();
            if (_onMoveReach != null) _onMoveReach(e.data);


        }
        public void Respawn(Variant data)
        {
            m_visible = true;

            //复活的时候换回以前的材质球
            if (m_bchangeDeadMtl)
            {
                (this as LGAvatarMonster).grAvatar.m_char.changeMtl(gameST.CHAR_MTL);

                m_bchangeDeadMtl = false;
            }

            //setAni("sidle");
            //setAni("run");
            setStand();
            setPos(data["x"]._float, data["y"]._float);
        }

        virtual public bool onAniEnd()
        {
            //if( m_loopFlag ) return false;
            //String lastAni = m_ani;

            if (IsDie())
            {


                //m_visible = false;
                if (this is LGAvatarMonster)
                {
                    grAvatar.playDrop_jinbi_fx();
                    grAvatar.m_char.changeMtl(gameST.DEAD_MTL);

                    m_bchangeDeadMtl = true;
                }

                //refreshAni();
                return true;
            }
            else
            {
                //if (Nav == null || Nav.isReach())
                //{
                //    setTag(GameConstant.ST_STAND);
                //}

                return false;
            }

            return false;
        }


        public uint iid
        {
            get
            {
                if (data.ContainsKey("iid") && data["iid"] != null)
                {
                    return this.data["iid"];
                }
                return 0;
            }
        }

        protected override void onDispose()
        {
            //if (_nav != null)
            //{
            //    _nav.dispose();
            //    _nav = null;
            //}

            grLoadedhandle = null;
            dCacheEffect.Clear();

            foreach (string effid in dEffing.Keys)
            {
                MapEffMgr.getInstance().removeEffItem(effid);
            }
            dEffing.Clear();

            base.onDispose();
        }
        private float m_dead_Burn = 0f;
        private bool m_bchangeDeadMtl = false;

        override public void updateProcess(float tmSlice)
        {
            tryRefrershY();


            if (m_bchangeDeadMtl)
            {
                if (this is LGAvatarMonster && m_dead_Burn < 1f)
                {

                    m_dead_Burn += tmSlice * 0.75f;
                    grAvatar.m_char.setMtlFloat(gameST.DEAD_MT_AMOUNT, m_dead_Burn);
                }

            }

        }

    }
}
