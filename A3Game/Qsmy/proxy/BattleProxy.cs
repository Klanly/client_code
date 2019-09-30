using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using MuGame.Qsmy.model;
using UnityEngine;
namespace MuGame
{
    class BattleProxy : BaseProxy<BattleProxy>
    {
        HudunModel hudunModel = HudunModel.getInstance();
        public static uint EVENT_SHIELD_FX = 1;
        public static uint EVENT_SHIELD_LOST = 2;
        public static readonly uint EVENT_SELF_KILL_MON = 3;
        public int skill_id;
        public int star_tm;
        public int end_tm;
        public BattleProxy()
            : base()
        {
            //addProxyListener(PKG_NAME.S2C_ON_STOP_ATK, on_stop_atk);
            addProxyListener(PKG_NAME.S2C_MONSTER_HATED, on_monsterHated);
            addProxyListener(PKG_NAME.S2C_ON_CAST_TARGET_SKILL, on_cast_target_skill); //怪物的技能和人物的技能
            addProxyListener(PKG_NAME.S2C_ON_CAST_GROUND_SKILL, on_cast_ground_skill); //对地面技能

            addProxyListener(PKG_NAME.S2C_ON_SINGLE_DAMAGE, on_single_damage);  //怪物的普通和技能伤害

            addProxyListener(PKG_NAME.S2C_ON_SINGLE_SKILL_RES, on_single_skill_res);

            addProxyListener(PKG_NAME.S2C_ON_CAST_SELF_SKILL, on_cast_self_skill);

            addProxyListener(PKG_NAME.S2C_ON_CAST_SKILL_ACT, on_cast_skill_act);

            addProxyListener(PKG_NAME.S2C_ON_BSTATE_CHANGE, on_bstate_change);

            addProxyListener(PKG_NAME.S2C_ON_ADD_STATE, on_add_state);
            addProxyListener(PKG_NAME.S2C_ON_DIE, on_die);
            addProxyListener(PKG_NAME.S2C_ON_CAST_SKILL_RES, on_cast_skill_res);
            addProxyListener(PKG_NAME.S2C_ON_CASTING_SKILL_RES, on_casting_skill_res);
            addProxyListener(PKG_NAME.S2C_ON_CANCEL_CASTING_SKILL_RES, on_cancel_casting_skill_res);
            addProxyListener(PKG_NAME.S2C_ON_RMV_STATE, on_rmv_state);
        }

        public void sendUseSelfSkill(uint skillid)
        {
            Variant v = GameTools.createGroup("sid", skillid, "start_tm", muNetCleint.instance.CurServerTimeStamp);
            sendRPC(PKG_NAME.S2C_USE_SELF_SKILL, v);
        }

        public void sendcast_target_skill(uint sid, List<uint> list_hitted, int lasthit, int lockid = -1)
        {
            if (list_hitted.Count <= 0) return;

            Variant msg = new Variant();
            msg["sid"] = sid;
            msg["crit"] = lasthit;

            Variant toiid_list = new Variant();
            for (int i = 0; i < list_hitted.Count; i++)
            {
                if (i > 15) continue;

                toiid_list.pushBack(list_hitted[i]);
            }

            msg["to_iid"] = toiid_list;
            if (lockid != -1)
                msg["lock_id"] = lockid;

            //debug.Log("请求攻击消息 " + msg.dump());

            sendRPC(PKG_NAME.C2S_CAST_TARGET_SKILL, msg);
        }

        //public void sendcast_fan_skill(uint sid, uint to_iid, int angle)
        //{
        //    sendRPC(PKG_NAME.C2S_CAST_GROUND_SKILL, GameTools.createGroup("to_iid", to_iid, "angle", angle, "sid", sid,
        //        "start_tm", muNetCleint.instance.CurServerTimeStampMS));
        //}

        public void sendcast_ground_skill(uint sid, float grdx, float grdy)
        {
            Variant v = GameTools.createGroup("x", grdx, "y", grdy, "sid", sid,
                "start_tm", muNetCleint.instance.CurServerTimeStampMS);

            sendRPC(PKG_NAME.C2S_CAST_TARGET_SKILL, v);
        }

        public void sendcast_self_skill(uint sid)
        {
            //debug.Log();
            sendRPC(PKG_NAME.C2S_CAST_SELF_SKILL, GameTools.createGroup("sid", sid,
                "start_tm", muNetCleint.instance.CurServerTimeStampMS));
        }
        public void send_cast_self_skill(uint iid, int skillid)
        {
            //释放技能，在单机副本模式时，不需要通知服务器
            //string str = "audio_skill_" + skillid;
            //MediaClient.instance.PlaySoundUrl(str, false, null);
            string str;
            switch (skillid)
            {
                case 3002:
                case 5003:
                case 2003:
                    str = "audio_skill_" + skillid;
                    MediaClient.instance.PlaySoundUrl(str, false, null);
                    break;
            }
            if (!SelfRole.s_bStandaloneScene)
            {
                debug.Log("id是：" + iid + "技能是：" + skillid);
                sendRPC(PKG_NAME.C2S_ON_CAST_SELF_SKILL, GameTools.createGroup("lock_iid", iid, "skillid", skillid));
            }
            //FightText.play(null, SelfRole._inst.getHeadPos(), -2 , skillId: (uint)skillid);
        }

        public void sendstop_atk()
        {
            sendRPC(8, new Variant());
        }
        public void sendcollectItm(uint iid)
        {
            sendRPC(PKG_NAME.S2C_ON_CAST_TARGET_SKILL, GameTools.createGroup("to_iid", iid));
        }
        public void sendcollectAreaItm(uint areaId)
        {
            sendRPC(PKG_NAME.S2C_ON_CAST_GROUND_SKILL, GameTools.createGroup("area_id", areaId));
        }
        public void sendgetNpcState(uint npcid, uint state)
        {
            sendRPC(107, GameTools.createGroup("npcid", npcid, "state", state));
        }


        void on_monsterHated(Variant msgData)
        {
            if (msgData.ContainsKey("hated_iid"))
                RoleMgr._instance.onMonsterHate(msgData["monster_iid"], msgData["hated_iid"]);
            else
                RoleMgr._instance.onMonsterHate(msgData["monster_iid"]);
        }

        void on_cast_target_skill(Variant msgData)
        {


            //debug.Log("!!!!!!!!!!!!!on_cast_target_skill:" + msgData.dump());

            //if (GRMap.playingPlot)
            //    return;

            ////debug.Log("有人释放一个技能了！！！！");
            //RoleMgr._instance.onCastTargetSkill(msgData);

            //NetClient.instance.dispatchEvent(
            //   GameEvent.Create(PKG_NAME.S2C_ON_CAST_TARGET_SKILL, this, msgData)
            //);
        }

        void on_cast_ground_skill(Variant msgData)
        {
            debug.Log("定点放技能：" + msgData.dump());

            uint tm = msgData["start_tm"];
            uint frm_iid = msgData["frm_iid"]._uint;

            BaseRole frmRole = RoleMgr._instance.getRole(frm_iid);
            if (frmRole == null || (frmRole is SelfRole))
                return;

            int sid = msgData["sid"];
            float x = msgData["x"]._float * GameConstant.GEZI / GameConstant.PIXEL_TRANS_UNITYPOS;
            float y = msgData["y"]._float * GameConstant.GEZI / GameConstant.PIXEL_TRANS_UNITYPOS;

            //NavMeshHit hit;
            Vector3 vec = new Vector3(x, frmRole.m_curModel.position.y, y);
            // NavMesh.SamplePosition(vec, out hit, 100f, frmRole.m_layer);

            //frmRole.TurnToPos(hit.position);
            if (frmRole is MS0000)
            {
                (frmRole as MS0000).ismapEffect = true;
                (frmRole as MS0000).effectVec = vec;
                frmRole.PlaySkill(sid);
            }
        }

        void on_cast_self_skill(Variant msgData)
        {
            //   debug.Log("收到动作信息播放相应动作");

            //uint tm = msgData["start_tm"]._uint;
            uint frm_iid = msgData["frm_iid"]._uint;
            int skill_id = msgData["sid"]._int;

            BaseRole frmRole = RoleMgr._instance.getRole(frm_iid);
            if (frmRole == null || (frmRole is SelfRole))
                return;


            if (msgData.ContainsKey("lock_iid"))
            {
                uint lock_iid = msgData["lock_iid"]._uint;
                BaseRole toRole = RoleMgr._instance.getRole(lock_iid);
                if (toRole != null)
                {
                    frmRole.TurnToRole(toRole, false);
                    frmRole.m_LockRole = toRole;
                }
                else
                {
                    debug.Log("攻击目标为空");
                }
            }

            //boss位移
            if (msgData.ContainsKey("telep"))
            {
                debug.Log("boss位移拉！！！！" + msgData.dump());
                float x = msgData["telep"]["to_x"];
                float y = msgData["telep"]["to_y"];
                frmRole.m_curModel.GetComponent<Monster_Base_Event>().onJump(x, y);
            }
            else
            {
                frmRole.OtherSkillShow();
                frmRole.PlaySkill(skill_id);
            }
        }

        void on_cast_skill_act(Variant msgData)
        {

            uint frm_iid = msgData["iid"];
            uint to_iid = msgData["to_iid"];
            uint skill_id = msgData["skill_id"];

            BaseRole toRole = RoleMgr._instance.getRole(to_iid);
            BaseRole frm = RoleMgr._instance.getRole(frm_iid);

            if (frm != null && toRole != null)
            {
                frm.m_LockRole = toRole;
                frm.TurnToRole(toRole, false);
                frm.PlaySkill((int)skill_id);
            }
            if (frm is MS0000)
            {
                debug.Log("放技能：" + msgData.dump());
            }

        }


        void on_casting_skill_res(Variant msgData)
        {
            debug.Log("释放怪物的大招技能???????????");
            debug.Log(msgData.dump()); 

            uint frm_iid = msgData["iid"];
            uint to_iid = msgData["to_iid"];
            uint skill_id = msgData["skid"];

            BaseRole toRole = RoleMgr._instance.getRole(to_iid);
            BaseRole frm = RoleMgr._instance.getRole(frm_iid);
            if (a3_trrigerDialog.instance != null)
                a3_trrigerDialog.instance.ShowNotice();
            if (frm != null && toRole != null)
            {
                frm.m_LockRole = toRole;

                if (msgData.ContainsKey("pos"))
                {//定点位置
                    float x = msgData["pos"]["x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                    float y = msgData["pos"]["y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;

                    Vector3 vec = new Vector3(x, frm.m_curModel.position.y, y);
                    if (frm is MonsterRole)
                    {
                        (frm as MonsterRole).ismapfx = true;
                        (frm as MonsterRole).fxvec = new Vector3(x, frm.m_curModel.position.y, y);
                    }
                    frm.TurnToPos(vec);
                }
                else
                {
                    frm.TurnToRole(toRole, false);
                }

                frm.PlaySkill((int)skill_id);
            }

            // RoleMgr._instance.onSong(msgData);

            //NetClient.instance.dispatchEvent(
            // GameEvent.Create(PKG_NAME.S2C_ON_CASTING_SKILL_RES, this, msgData));
        }

        //enum damage_hited_type
        //{
        //    MISS = 0, //未命中
        //    HIT = 1,//命中（普通命中）
        //    CRIT = 2,//暴击
        //    DOUBLE = 3,//双倍
        //    REFLECT = 4,//反射
        //    IGNORE = 5,//抵消
        //}


        public int hurt_old_time = -1;

        //这里处理由于切地图导致协议延后处理
        public List<Variant> battleProxy_damage = new List<Variant>();
        public List<Variant> battleProxy_die = new List<Variant>();
        public void onMapLoaded()
        {
            if (battleProxy_damage.Count > 0)
            {
                foreach (Variant msg in battleProxy_damage)
                {
                    bool need_do = false;
                    if (msg.ContainsKey("damages"))
                    {
                        List<Variant> l = msg["damages"]._arr;
                        foreach (Variant v in l)
                        {
                            if (v["to_iid"] == PlayerModel.getInstance().iid || v["isdie"]._bool == true)
                            {
                                need_do = true;
                                break;
                            }
                        }
                    }
                    if (msg.ContainsKey("link_damage"))
                    {
                        List<Variant> l = msg["link_damage"]._arr;
                        foreach (Variant v in l)
                        {
                            if (v["to_iid"] == PlayerModel.getInstance().iid || v["isdie"]._bool == true)
                            {
                                need_do = true;
                                break;
                            }
                        }
                    }
                    if (msg.ContainsKey("random_damage"))
                    {
                        List<Variant> l = msg["random_damage"]._arr;
                        foreach (Variant v in l)
                        {
                            if (v["to_iid"] == PlayerModel.getInstance().iid || v["isdie"]._bool == true)
                            {
                                need_do = true;
                                break;
                            }
                        }
                    }

                    if (need_do)
                    {
                        on_single_damage(msg);
                        debug.Log("延迟的damage" + msg.dump());
                    }
                }
            }
            if (battleProxy_die.Count > 0)
            {
                foreach (Variant v in battleProxy_die)
                {
                    on_die(v);
                    debug.Log("延迟的die" + v.dump());
                }
            }

            battleProxy_damage.Clear();
            battleProxy_die.Clear();
        }
        void on_single_damage(Variant msg)
        {
            if (GRMap.grmap_loading)
            {
                battleProxy_damage.Add(msg);
                return;
            }


            //RoleMgr._instance.onHurt(msgData);            
            if (msg.ContainsKey("damages"))
            {
                List<Variant> l = msg["damages"]._arr;
                foreach (Variant v in l)
                {
                    //被攻击时记录一个时间，表里面有个时间间隔，两次攻击的时间差>=这个间隔，算脱离战斗状态（分线功能（可分线））
                    if (v["frm_iid"] == PlayerModel.getInstance().iid || v["to_iid"] == PlayerModel.getInstance().iid)
                    {
                        hurt_old_time = NetClient.instance.CurServerTimeStamp;
                    }
                    doHurt(v);
                }
            }
            if (msg.ContainsKey("link_damage"))
            {
                bool hasKilledMon = false;
                int tuneid = msg["rune_id"];

                List<Variant> l = msg["link_damage"]._arr;
                foreach (Variant v in l)
                {
                    uint to_iid = v["to_iid"]._uint;
                    int damage = v["dmg"]._int;
                    bool isdie = v["isdie"]._bool;//<v name="isdie" type="bool" />
                    int hprest = v["hprest"]._int;
                    uint frm_iid = v["frm_iid"]._uint;
                    bool stagger = false;
                    hasKilledMon |= frm_iid == PlayerModel.getInstance().iid;
                    if (v.ContainsKey("stagger"))
                    {
                        stagger = v["stagger"];
                    }
                    BaseRole toRole = RoleMgr._instance.getRole(to_iid);
                    if (toRole == null )
                        return;
                    BaseRole frm = RoleMgr._instance.getRole(frm_iid);
                    debug.Log("名字"+ toRole.roleName);
                    doHurt(toRole, frm, damage, isdie, hprest, tuneid, false, stagger);

                    GameObject eff = EffMgr.getRuneEff(tuneid);
                    if (eff != null)
                    {
                        //GameObject fx_inst = GameObject.Instantiate(eff) as GameObject;
                        //GameObject.Destroy(fx_inst, 1.2f);
                        //fx_inst.transform.SetParent(toRole.m_curModel, false);

                        EffMgr.instance.addEff(frm, toRole, GameObject.Instantiate(eff) as GameObject, 0.4f);

                        GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[1], toRole.m_curModel.position, toRole.m_curModel.rotation) as GameObject;
                        fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                        GameObject.Destroy(fx_inst, 2f);
                    }
                }
                if (hasKilledMon)
                    dispatchEvent(GameEvent.Create(EVENT_SELF_KILL_MON, this, null));
            }
            //if (msg.ContainsKey("holy_shield"))
            //{
            //    //剩余pvp盾值
            //    hudunModel.NowCount = msg["holy_shield"];
            //}

            if (msg.ContainsKey("random_damage"))
            {
                int tuneid = msg["rune_id"];


                List<Variant> l = msg["random_damage"]._arr;
                foreach (Variant v in l)
                {
                    uint to_iid = v["to_iid"]._uint;
                    int damage = v["dmg"]._int;
                    bool isdie = v["isdie"]._bool;//<v name="isdie" type="bool" />
                    int hprest = v["hprest"]._int;
                    uint frm_iid = v["frm_iid"]._uint;
                    bool stagger = false;
                    if (v.ContainsKey("stagger"))
                    {
                        stagger = v["stagger"];
                    }
                    BaseRole toRole = RoleMgr._instance.getRole(to_iid);
                    if (toRole == null)
                        return;
                    BaseRole frm = RoleMgr._instance.getRole(frm_iid);

                    doHurt(toRole, frm, damage, isdie, hprest, -1, false, stagger);
                    GameObject eff = EffMgr.getRuneEff(tuneid);
                    if (eff != null)
                    {
                        GameObject fx_inst = GameObject.Instantiate(eff) as GameObject;
                        GameObject.Destroy(fx_inst, 2f);
                        fx_inst.transform.SetParent(toRole.m_curModel, false);
                    }

                }
            }
        }


        void doHurt(BaseRole toRole, BaseRole frm, int damage, bool isdie, int hprest, int iscrit, bool miss, bool stagger, uint skill_id = 0)
        {

            toRole.onServerHurt(damage, hprest, isdie, frm, iscrit, miss, stagger);

            if (toRole.m_isMain && !miss)
            {
                PlayerModel.getInstance().modHp(hprest);
            }

            if (frm != null)
            {
                //if (frm.m_isMain)
                //{//主角不改变朝向,有些技能伤害是延迟的

                //}
                //else
                //{
                //    frm.TurnToRole(toRole, false);
                //}
                //if (frm is M000P2 || frm is M000P3 || frm is M000P5
                //    frm is ohterP2Warrior || frm is ohterP3Mage || frm is ohterP5Assassin)
                //{
                //    frm.PlaySkill((int)skill_id);
                //    frm.m_LockRole = toRole;
                //}
                //else frm.PlaySkill(0);
                //frm.PlaySkill((int)skill_id);
            }


            if (frm is ProfessionRole &&
                toRole.m_isMain &&
                SelfRole.fsm.Autofighting &&
                AutoPlayModel.getInstance().AutoPK > 0)
            {
                if (StatePK.Instance.Enemy != frm &&
                    SelfRole.fsm.currentState != StatePK.Instance)
                {
                    StatePK.Instance.Enemy = (ProfessionRole)frm;
                    SelfRole.fsm.ChangeState(StatePK.Instance);
                }
            }
        }

        void doHurt(Variant v)
        {
            //debug.Log("doHurt:::::" + v.dump());
            uint to_iid = v["to_iid"]._uint;
            int damage = v["dmg"]._int;
            bool isdie = v["isdie"]._bool;//<v name="isdie" type="bool" />
            int hprest = v["hprest"]._int;
            uint frm_iid = v["frm_iid"]._uint;
            uint skill_id = v["skill_id"]._uint;
            bool stagger = false;
            if (v.ContainsKey("stagger"))
            {
                stagger = v["stagger"];
            }
  
            BaseRole toRole = RoleMgr._instance.getRole(to_iid);
            if (isdie)
            {
                if (toRole is P5Assassin)
                {
                    MediaClient.instance.PlaySoundUrl("audio_common_assassin_dead", false, null);
                }
                if (toRole is P2Warrior)
                {
                    MediaClient.instance.PlaySoundUrl("audio_common_warrior_dead", false, null);
                }
                if (toRole is P3Mage)
                {
                    MediaClient.instance.PlaySoundUrl("audio_common_mage_dead", false, null);
                }
            }

            if (toRole == null)
                return;
            BaseRole frm = RoleMgr._instance.getRole(frm_iid);
            //隐身的第一击额外伤害
            if (v.ContainsKey("invisible_first_atk") && frm != null && frm.m_isMain)
            {
                if (v["rune_id"])
                {
                    int runeid = v["rune_id"];
                    int damage_atk = v["invisible_first_atk"];
                    FightText.play(FightText.IMG_TEXT, toRole.getHeadPos(), damage_atk, false,runeid,null,toRole);
                }
                //FightText.play(FightText.userText, toRole.getHeadPos(), v["invisible_first_atk"]);
                //flytxt.instance.fly("隐身的第一击额外伤害");
                //FightText.play(FightText.IMG_TEXT, role.getHeadPos(), hpchange, false, rune_id);
            }
            //0:未命中 , 1:命中（普通命中） , 2:暴击,3:双倍,4:反射,5:抵消,6:无视防御
            if (frm is MS0000)
            {
                debug.Log("伤害" + skill_id);
            }
            if (v.ContainsKey("hited"))
            {
                if (v.ContainsKey("ignore_defense_damage") && frm != null && frm.m_isMain)
                {
                    doHurt(toRole, frm, damage, isdie, hprest, 6, false, stagger, skill_id);
                    //FightText.play(FightText.IMG_TEXT_2, toRole.getHeadPos(), damage, false, 6,null,toRole);
                }
                else
                {
                    int hited = v["hited"];
                    switch (hited)
                    {
                        case 0://未命中
                            if (frm != null && (toRole.m_isMain || frm.m_isMain))
                                FightText.play(FightText.MISS_TEXT, frm.getHeadPos(), 0, false, -1, null, frm); break;
                        case 1://命中（普通命中）
                            doHurt(toRole, frm, damage, isdie, hprest, 1, false, stagger, skill_id); break;
                        case 2://暴击
                            doHurt(toRole, frm, damage, isdie, hprest, 2, false, stagger, skill_id); break;
                        case 3://双倍
                            doHurt(toRole, frm, damage, isdie, hprest, 3, false, stagger, skill_id); break;
                        case 4://反射
                            if (v["rune_id"] && frm.m_isMain)
                            {
                                int runeid = v["rune_id"];
                                FightText.play(FightText.IMG_TEXT, toRole.getHeadPos(), damage, false, runeid,null,toRole);
                            }
                            else if (v["rune_id"] && toRole.m_isMain)
                                doHurt(toRole, frm, damage, isdie, hprest, 4, false, stagger, skill_id); break;
                        case 5://抵消
                            doHurt(toRole, frm, damage, isdie, hprest, 5, false, stagger, skill_id); break;
                    }
                }
            }
            else
                doHurt(toRole, frm, damage, isdie, hprest, -1, true, stagger);

            if (toRole.m_isMain)
            {
                //重置护盾充能时间
                hudunModel.isNoAttack = false;
                if (a3_herohead.instance != null)
                    a3_herohead.instance.wait_attack(hudunModel.noAttackTime);
                if (frm is ProfessionRole)
                {//屏幕受攻击预警
                    if (a3_lowblood.instance != null)
                        a3_lowblood.instance.begin();
                }
                if (a3_insideui_fb .instance != null && PlayerModel .getInstance ().inFb  ) {
                    a3_insideui_fb.instance.Cancel();
                }

            }
            if (PlayerModel.getInstance().treasure_num > 0)
            {
                if (frm is ProfessionRole && toRole.m_isMain)
                {
                    FindBestoModel.getInstance().Canfly = false;
                    if (a3_herohead.instance != null)
                        a3_herohead.instance.wait_attack_baotu(FindBestoModel.getInstance().waitTime);
                }
            }
            if (v.ContainsKey("dmg_shield"))
            {
                //pvp盾受到的伤害
                FightText.play(FightText.SHEILD_TEXT, toRole.getHeadPos(), v["dmg_shield"],false,-1,null,toRole);
                onShield(toRole);
            }

            if (v.ContainsKey("holy_shield"))
            {
                //剩余pvp盾值
                if (toRole.m_isMain)
                {
                    hudunModel.NowCount = v["holy_shield"];
                }
                if (v["holy_shield"] <= 0)
                {
                    //dispatchEvent(GameEvent.Create(EVENT_SHIELD_LOST, this, null));
                    onShieldLost(toRole);
                }
            }
        }
        float waitTime = 0;
        float shield_time = 1f;
        TickItem process;
        bool ShieldFirst = true;
        GameObject shield_fx_clon;
        void onShield(BaseRole Role)
        {
            if (ShieldFirst)
            {
                GameObject shield_fx = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_player_common_FX_com_dun_ilde");
                if (shield_fx != null && shield_fx_clon == null)
                {
                    shield_fx_clon = GameObject.Instantiate(shield_fx) as GameObject;
                    shield_fx_clon.transform.SetParent(Role.m_curModel, false);
                    if (process == null)
                    {
                        process = new TickItem(updata_Wite);
                        TickMgr.instance.addTick(process);
                        waitTime = 0;
                    }
                    ShieldFirst = false;
                }

            }
        }
        void updata_Wite(float s)
        {
            waitTime += s;
            if (waitTime > shield_time)
            {
                GameObject.Destroy(shield_fx_clon);
                waitTime = 0;
                TickMgr.instance.removeTick(process);
                process = null;
                shield_fx_clon = null;
                ShieldFirst = true;
            }
        }
        void onShieldLost(BaseRole Role)
        {
            GameObject shield_fx_lost = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_player_common_FX_com_dun_broken");
            if (shield_fx_lost != null)
            {
                GameObject shield_fx_lost_clon = GameObject.Instantiate(shield_fx_lost) as GameObject;
                shield_fx_lost_clon.transform.SetParent(Role.m_curModel, false);
                GameObject.Destroy(shield_fx_lost_clon, 2);
                if (shield_fx_clon != null)
                {
                    GameObject.Destroy(shield_fx_clon);
                }
            }
        }
        void on_bstate_change(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //   GameEvent.Create(PKG_NAME.S2C_ON_BSTATE_CHANGE, this, msgData)
            //);
        }

        void on_single_skill_res(Variant msgData)
        {

            debug.Log("UUUUUUU" + msgData.dump());
            uint to_iid = msgData["to_iid"]._uint;
            uint frm_iid = msgData["frm_iid"]._uint;
            BaseRole toRole = RoleMgr._instance.getRole(to_iid);
            BaseRole frmRole = RoleMgr._instance.getRole(frm_iid);

            //toRole.m_unTeamID = 0;

            if (!msgData.ContainsKey("states"))
                return;

            if (toRole != null && toRole.m_isMain)
            {
                Variant data = msgData["states"];
                if (msgData["sid"])
                {
                    int runeid = msgData["sid"];
                    //skill_id = runeid;
                    FightText.play(FightText.BUFF_TEXT, toRole.getHeadPos(), 0, false, runeid,null,toRole);
                }
                //if (data["id"] != 10000)
                A3_BuffModel.getInstance().addBuffList(data);

                ////副本祝福
                //if (data["id"] == 10000)
                //{
                //    A3_ActiveProxy.getInstance().dispatchEvent(GameEvent.Create(A3_ActiveProxy.EVENT_ONBLESS, this, data));
                //}
            }
            else
            {
                A3_BuffModel.getInstance().addOtherBuff(toRole, msgData["states"]["id"]);
            }

            if (msgData.ContainsKey("states"))
            {//技能的配置表特效播放
                SXML xml = XMLMgr.instance.GetSXML("skill.state", "id==" + msgData["states"]["id"]);
                string eff_file = xml.getString("effect");
                if (frmRole is MonsterRole && (frmRole as MonsterRole).issummon && to_iid == frm_iid)
                {
                    frmRole.PlaySkill(msgData["sid"]);
                }
                if (eff_file != "null")
                {
                    if (SceneCamera.m_nSkillEff_Level == 1 || toRole.m_isMain ||
                        (frmRole is MonsterRole && (frmRole as MonsterRole).masterid == PlayerModel.getInstance().cid))
                    {//屏蔽隐藏其他玩家时的buff特效
                        float time = xml.getFloat("last");
                        GameObject fx_prefab = GAMEAPI.ABFight_LoadPrefab(eff_file);
                        GameObject fx_inst = GameObject.Instantiate(fx_prefab) as GameObject;

                        fx_inst.transform.SetParent(toRole.m_curModel, false);
                        GameObject.Destroy(fx_inst, time);

                        if (xml.getFloat("head") > 0)
                        {//头顶显示
                            fx_inst.transform.localPosition = new Vector3(0, toRole.headOffset_half.y / 2 + xml.getFloat("head"), 0);
                        }
                    }
                }
            }
        }
        void on_add_state(Variant msgData)
        {
            debug.Log("添加buff" + msgData.dump());
            if (msgData != null && msgData["iid"] != null)
            {
                uint to_iid = msgData["iid"]._uint;

                BaseRole toRole = RoleMgr._instance.getRole(to_iid);
                if (toRole != null || to_iid == PlayerModel.getInstance().iid )
                {
                    if ( (toRole !=null && toRole.m_isMain) || to_iid == PlayerModel.getInstance().iid )
                    {
                        if ( !msgData.ContainsKey( "states" ) )
                            return;
                        foreach ( Variant one in msgData[ "states" ]._arr )
                        {
                            Variant data = one;
                            if ( data[ "id" ] == 10001 && a3_herohead.instance != null )
                            {

                                end_tm = data[ "end_tm" ];
                                star_tm = data[ "start_tm" ];
                                //a3_herohead.instance.doubleexp_bf = true;
                                a3_herohead.instance.exp_time = end_tm - muNetCleint.instance.CurServerTimeStamp;

                            }

                            A3_BuffModel.getInstance().addBuffList( data );
                        }
                    }

                }
                else
                {
                    foreach (Variant one in msgData["states"]._arr)
                    {
                        A3_BuffModel.getInstance().addOtherBuff(toRole, one["id"]);
                    }
                }
            }
        }

        void on_die(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //   GameEvent.Create(PKG_NAME.S2C_ON_DIE, this, msgData)
            //);

            if (GRMap.grmap_loading)
            {
                battleProxy_die.Add(msgData);
                return;
            }

            uint to_iid = msgData["iid"]._uint;

            BaseRole toRole = RoleMgr._instance.getRole(to_iid);
            if (toRole == null)
                return;
            doHurt(toRole, null, 0, true, -1, -1, false, false);
        }

        void on_cast_skill_res(Variant msgData)
        {
            //debug.Log("战斗返回信息： " + msgData.dump());

            //if (msgData["res"] < 0)
            //{
            //    if (!SelfRole.fsm.Autofighting)
            //        flytxt.instance.fly(err_string.get_Err_String(msgData["res"]));
            //}
            //else            
            if (msgData["res"] == -768)
            {
                SelfRole._inst.m_curModel.position =
                SelfRole._inst.m_roleDta.pos =
                MoveProxy.getInstance().GetLastSendXY() / 53.3f + new Vector3(0, SelfRole._inst.m_roleDta.pos.y, 0);
            }


            //NetClient.instance.dispatchEvent(
            //   GameEvent.Create(PKG_NAME.S2C_ON_CAST_SKILL_RES, this, msgData)
            //);



        }
        public void grtCoumy() { }

        void on_cancel_casting_skill_res(Variant msgData)
        {
            //NetClient.instance.dispatchEvent(
            //   GameEvent.Create(PKG_NAME.S2C_ON_CANCEL_CASTING_SKILL_RES, this, msgData)
            //);
        }
        //--------------------------------------------buff问题暂时注掉
        void on_rmv_state(Variant msgData)
        {
            debug.Log("移除buff" + msgData.dump());
            //uint iid = msgData["iid"];
            uint iid = msgData["iid"]._uint;
            BaseRole toRole = RoleMgr._instance.getRole(iid);
            if ((toRole != null && toRole.m_isMain) || iid == PlayerModel.getInstance().iid )
            {
                foreach (uint id in msgData["ids"]._arr)
                {
                    A3_BuffModel.getInstance().RemoveBuff(id);
                }
            }
            else
            {
                foreach (uint id in msgData["ids"]._arr)
                {
                    A3_BuffModel.getInstance().removeOtherBuff(toRole, id);
                }
            }
            //-------------------------------------------------------------
            //LGAvatarGameInst av = RoleMgr._instance.getRoleByIID(iid);
            //if (av == null)
            //    return;
            //List<Variant> ids = msgData["ids"]._arr;
            //foreach (Variant id in ids)
            //{
            //    av.removeBuffer(id);
            //}
        }
    }
}
