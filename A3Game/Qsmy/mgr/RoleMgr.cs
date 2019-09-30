using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
using System;

namespace MuGame
{
    class RoleMgr
    {
        static public RoleMgr _instance = new RoleMgr();

        public RoleMgr()
        {

        }

        public BaseRole getRole(uint iid)
        {
            if ((SelfRole._inst != null && iid == SelfRole._inst.m_unIID) )
                return SelfRole._inst;
            BaseRole role = OtherPlayerMgr._inst.GetOtherPlayer(iid);
            if (role == null)
                role = MonsterMgr._inst.getServerMonster(iid);
            return role;
        }


        public void onMonsterHate(uint iid, uint hateiid = 0)
        {
            //LGAvatarGameInst mon = LGMonsters.instacne.getMonsterById(iid);
            //if (mon == null)
            //    return;
            //LGAvatarGameInst hate = null;
            //if (hateiid != 0)
            //    hate = getRoleByIID(hateiid);
            //mon.lockTarget = hate;

            //if (hate == lgSelfPlayer.instance)
            //    PlayerNameUIMgr.getInstance().showActive(mon.grAvatar);
        }


        //public void onStop(Variant data)
        //{
        //    LGAvatarGameInst to = getRoleByIID(data["iid"]); if (to == null) return;
        //    //   if (to._currMoveOri != null) to._currMoveOri = null;
        //    to.stop();
        //}

        //public void onPosCorrect(Variant data)
        //{
        //    LGAvatarGameInst to = getRoleByIID(data["iid"]);
        //    if (to == null) return;
        //    float x = data["x"]._float;
        //    float y = data["y"]._float;
        //    to.stop();
        //    to.setPos(x, y);
        //}

        //private void onCastSelfSkill(GameEvent e)
        //{
        //    //debug.Log("收到动作信息播放相应动作");
        //    //其他玩家使用技能
        //    uint unlock_iid = e.data["lock_iid"]._uint;
        //    int nskill_id = e.data["skillid"]._int;
        //    //debug.Log("收到动作信息播放相应动作 ------------ " + unlock_iid + "  ------- skill " + nskill_id);
        //    uint frm_iid = e.data["frm_iid"]._uint;
        //    ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(frm_iid);
        //    if (pr != null)
        //    {
        //        pr.PlaySkill(nskill_id);
        //    }
        //}

        public void clear()
        {
            LGMonsters.instacne.clear();
        }

        //public void onRespawn(Variant info)
        //{
        //    if (!info.ContainsKey("iid")) return;
        //    uint iid = info["iid"]._uint;
        //    LGAvatarGameInst from = getRoleByIID(iid);
        //    if (from == null) return;

        //    from.onRespawn(info);
        //}

        //public void onDie(Variant data)
        //{
        //    uint iid = data["iid"]._uint;
        //    LGAvatarGameInst from = getRoleByIID(iid);
        //    if (from == null) return;

        //    from.onDie(data);
        //}

        public void AddStates(Variant states)
        {



        }

        //public void onCastTargetSkill(Variant data)
        //{
        //    //debug.Log("使用技能了  ............");

        //    uint iid = data["frm_iid"]._uint;
        //    LGAvatarGameInst from = getRoleByIID(iid);
        //    if (from == null) return;
        //    if (from is lgSelfPlayer) return;

        //    from._cast_success(data);
        //}

        //public void onCastGroundSkill(Variant data)
        //{
        //    debug.Log("使用了地面范围技能  ............");

        //    uint iid = data["frm_iid"]._uint;
        //    LGAvatarGameInst from = getRoleByIID(iid);
        //    if (from == null) return;
        //    if (from is lgSelfPlayer) return;

        //    from._cast_success(data);
        //}

        //public void onSong(Variant vd)
        //{
        //    LGAvatarGameInst from = getRoleByIID(vd["iid"]);
        //    if (from != null)
        //        from.OnSong(vd);
        //}

        //public bool canHardShowObj = true;
        //public void onHurt(Variant d)
        //{
        //    //普通攻击
        //    //Variant d = e.data;
        //    LGAvatarGameInst from = getRole(d["frm_iid"]);
        //    LGAvatarGameInst to = getRoleByIID(d["to_iid"]);
        //    int hited = d["hited"];//是否击中，=0为免疫，=1为未命中，=2为闪避，=3为命中
        //    bool criatk = d["criatk"];//暴击

        //    if (hited == 3)
        //    {
        //        if (from != null)
        //            from.OnAttack(d, to);

        //        if (to != null)
        //            to.OnHurt(d, from);


        //        //if (from == null && canHardShowObj && to is lgSelfPlayer)
        //        //{
        //        //    canHardShowObj = false;
        //        //    MapProxy.getInstance().sendShowMapObj();
        //        //}
        //    }
        //}
        private GameObject healEff;
        public void onAttchange(Variant msgData)
        {
            debug.Log("onAttchange::" + msgData.dump());
            BaseRole role = getRole(msgData["iid"]);
            if (role == null) return;

            //if (msgData.ContainsKey("hpadd") && msgData["hpadd"].ContainsKey("die") && msgData["hpadd"]["die"]._bool)
            //    role.Die(msgData);

            bool isUser = role.m_isMain;

            if ( msgData.ContainsKey( "speed" ) && role is ProfessionRole  )
            {
                var  pro = (role as ProfessionRole);

                pro.speed = msgData[ "speed" ]._int;

                if ( isUser )
                {
                    PlayerModel.getInstance().speed = pro.speed;
                }

            } 

            uint frm_iid = 0;
            LGAvatarGameInst frm_lc = null;
            if (msgData.ContainsKey("hpchange"))
            {
                Variant hpchanged = msgData["hpchange"];
                int hpchange = hpchanged["hpchange"];
                int curhp = hpchanged["hp_left"];
                Variant d = new Variant();
                if (isUser)
                {
                    PlayerModel.getInstance().modHp(curhp);
                }
                if (hpchange > 0)
                {
                    role.modHp(curhp);
                    if (isUser)
                    {
                        FightText.play(FightText.HEAL_TEXT, role.getHeadPos(), hpchange);
                    }
                    if (healEff == null)
                        healEff = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fuwenFX_FX_fuwen_chuyong");
                    if (healEff != null && role is ProfessionRole)
                    {
                        GameObject fx_inst = GameObject.Instantiate(healEff) as GameObject;
                        GameObject.Destroy(fx_inst, 1f);
                        fx_inst.transform.SetParent(role.m_curModel, false);
                    }
                    if (msgData.ContainsKey("rune_ids"))
                    {
                        List<Variant> l = msgData["rune_ids"]._arr;
                        foreach (Variant rune_id in l)
                        {
                            FightText.play(FightText.ADD_IMG_TEXT, role.getHeadPos(), hpchange, false, rune_id);
                        }
                    }
                }
                else if (hpchange < 0)
                {
                    frm_iid = hpchanged["frm_iid"];
                    BaseRole frm = RoleMgr._instance.getRole(frm_iid);

                    if (msgData.ContainsKey("rune_ids"))
                    {
                        Variant rune = msgData["rune_ids"];
                        if (rune.Count > 0) {
                            int id_rune = rune[0];
                            List<Variant> l = msgData["rune_ids"]._arr;
                            foreach (Variant rune_id in l)
                            {
                                role.onServerHurt(-hpchange, curhp, hpchanged["die"], frm, id_rune);
                            }
                        }
                    }

                }
            }

            if (msgData.ContainsKey("mpchange"))
            {
                Variant mpchanged = msgData["mpchange"];
                int mpchange = mpchanged["mpchange"];
                int curmp = mpchanged["mp_left"];
                if (isUser)
                {
                    PlayerModel.getInstance().modMp(curmp);
                }
            }
            if (msgData.ContainsKey("pk_state"))
            {
                int pkstate = msgData["pk_state"];
                switch (pkstate)
                {
                    case 0:
                        role.m_ePK_Type = PK_TYPE.PK_PEACE;
                        break;
                    case 1:
                        role.m_ePK_Type = PK_TYPE.PK_PKALL;
                        break;
                    case 2:
                        role.m_ePK_Type = PK_TYPE.PK_TEAM;
                        break;
                    case 3:
                        role.m_ePK_Type = PK_TYPE.PK_LEGION;
                        break;
                    case 4:
                        role.m_ePK_Type = PK_TYPE.PK_HERO;
                        break;
                    case 5:
                        role.m_ePK_Type = PK_TYPE.Pk_SPOET;
                        break;

                };
            }
            if (msgData.ContainsKey("clanid"))
            {
                role.m_unLegionID = msgData["clanid"];
            }
            if (msgData.ContainsKey("teamid"))
            {
                role.m_unTeamID = msgData["teamid"];

                if ( isUser )
                {
                    PlayerModel.getInstance().teamid = role.m_unTeamID;

                    PlayerNameUIMgr.getInstance().refeshHpColor();

                }
                else {

                    PlayerNameUIMgr.getInstance().refeshHpColor( role );

                }

            }

            if (msgData.ContainsKey("rune_ids"))
            {
                List<Variant> l = msgData["rune_ids"]._arr;
                foreach (Variant rune_id in l)
                {
                    GameObject eff = EffMgr.getRuneEff(rune_id._int);
                    if (eff != null)
                    {
                        GameObject fx_inst = GameObject.Instantiate(eff) as GameObject;
                        GameObject.Destroy(fx_inst, 2f);
                        fx_inst.transform.SetParent(role.m_curModel, false);
                    }
                }
                //if (!msgData.ContainsKey("hpchange"))
                //{
                //    foreach (Variant rune_id in l)
                //    {
                //        FightText.play(FightText.IMG_TEXT, role.getHeadPos(), 0, false, rune_id);
                //    }
                //}
            }

            if (msgData.ContainsKey("sprite_flag"))
            {
                uint call = msgData["sprite_flag"];
                uint iid = msgData["iid"];
                var vv = MonsterMgr._inst.getMonster(iid);
                if (vv != null)
                {
                    SkinnedMeshRenderer render = vv.m_curModel.FindChild("body").GetComponent<SkinnedMeshRenderer>();
                    switch (call)
                    {
                        case 0:
                            foreach (var v in render.sharedMaterials)
                            {
                                v.shader = Shader.Find("A3/A3_Char_Streamer_H");
                                v.SetColor("_RimColor", new Color(0, 0, 0, 0));
                                v.SetFloat("_RimWidth", 0f);
                            }
                            break;
                        case 1:
                            render.sharedMaterial = U3DAPI.U3DResLoad<Material>("default/monster_1021_heite_gold");
                            break;
                    }
                }
            }

            if ( isUser == false )
            {
                if ( msgData.ContainsKey( "dress" ) )
                {       
                    var _role  = role as ProfessionRole;

                    _role.rideId = msgData.getValue ( "dress" )._int;

                    if ( msgData.getValue( "mount" )._uint == (uint)RIDESTATE.UP && _role !=null )
                    {
                        _role.ridestate = RIDESTATE.UP;

                        if ( _role.invisible == false &&  _role.dianjiTime == -1 && _role.isUp == false )
                        {
                            _role.ChangeRideState( true );
                        }

                        //_role.set_Ride( msgData.getValue( "dress" )._int);
                    }

                    else if( msgData.getValue( "mount" )._uint == ( uint ) RIDESTATE.Down && _role !=null )
                    {
                        _role.ridestate = RIDESTATE.Down;

                        _role.Remove_Ride();

                    }
                    
                }

            } //其他玩家坐骑切换

            //if (msgData.ContainsKey("speed"))
            //    role.modSpeed(msgData["speed"]._int);

            //if (msgData.ContainsKey("maxhp"))
            //{
            //    int maxhp = msgData["maxhp"];
            //    role.modMaxHp(maxhp);
            //    if (isUser)
            //    {
            //        PlayerModel.getInstance().max_hp = maxhp;
            //    }
            //}

            //if (msgData.ContainsKey("maxdp"))
            //    role.modMaxDp(msgData["maxdp"]._int);

            //if (msgData.ContainsKey("in_pczone"))
            //{
            //    if (role.isMainPlayer())
            //    {
            //        if (msgData["in_pczone"]._bool)
            //        {
            //           // lgGeneral.PKStateChange(0);需要切换pk模式 lucisa
            //        }
            //        lguiMain.PczoneChange(msgData["in_pczone"]._bool);
            //    }
            //    this.in_pczone = msgData["in_pczone"]._bool;

            //}
            //if (msgData.ContainsKey("follow"))
            //{
            //    this.follow = msgData["follow"]._bool;
            //}
            //if (msgData.ContainsKey("ghost"))
            //{
            //    this.ghost = msgData["ghost"]._bool;
            //}
            //if (msgData.ContainsKey("ride_mon_id"))
            //{
            //    this.ride_mon = msgData["ride_mon_id"];//坐骑 lucisa
            //}
        }

        public static void ClearMark(bool clearAnyway = false, PK_TYPE pkState = PK_TYPE.PK_PEACE, Func<BaseRole, bool> filterHandle = null)
        {
            int i = 0;
            var monDic = MonsterMgr._inst.m_mapMonster;
            var playerDic = OtherPlayerMgr._inst.m_mapOtherPlayer;
            switch (pkState)
            {
                default: goto case PK_TYPE.PK_PEACE;
                case PK_TYPE.PK_PKALL:                    
                    for (List<uint> idx = new List<uint>(playerDic.Keys); i < idx.Count; i++)
                    {
                        if (i == idx.Count - 1 || clearAnyway)
                        {
                            for (int j = 0; j < idx.Count; j++) 
                                playerDic[idx[j]].m_isMarked = false;
                            break;
                        }
                        if (!playerDic[idx[i]].m_isMarked && (filterHandle?.Invoke(playerDic[idx[i]]) ?? true))
                            break;
                    }
                    goto case PK_TYPE.PK_PEACE;
                case PK_TYPE.PK_TEAM:
                    for (List<uint> idx = new List<uint>(playerDic.Keys); i < idx.Count; i++)
                    {
                        if (i == idx.Count - 1 || clearAnyway)
                        {
                            for (int j = 0; j < idx.Count; j++)
                                playerDic[idx[j]].m_isMarked = false;
                            break;
                        }
                        if (!playerDic[idx[i]].m_isMarked && (filterHandle?.Invoke(playerDic[idx[i]]) ?? true))
                            break;
                    }
                    goto case PK_TYPE.PK_PEACE;
                case PK_TYPE.PK_PEACE:
                    i = 0;
                    for (List<uint> idx = new List<uint>(monDic.Keys); i < idx.Count; i++)
                    {
                        if (i == idx.Count - 1 || clearAnyway)
                        {
                            for (int j = 0; j < idx.Count; j++)
                                monDic[idx[j]].m_isMarked = false;
                            break;
                        }
                        if (!monDic[idx[i]].m_isMarked && (filterHandle?.Invoke(monDic[idx[i]]) ?? true))
                            break;
                    }
                    break;
                case PK_TYPE.Pk_SPOET:
                    for (List<uint> idx = new List<uint>(playerDic.Keys); i < idx.Count; i++) {
                        for (int j = 0; j < idx.Count; j++)
                            playerDic[idx[j]].m_isMarked = false;
                        break;
                    }
                    break;
            }
        }

    }




}
