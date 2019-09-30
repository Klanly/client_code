using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using System.Collections;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    public class PlayerModel : ModelBase<PlayerModel>
    {
        public static uint ON_ATTR_CHANGE = 0;
        public static uint ON_LEVEL_CHANGED = 1;
        public int profession;
        public uint cid;//角色id
        public uint uid;//用户id
        public uint iid;//用户服务器分配的当前内存实例id
        public uint crttm; //创角时间蹉
        public String name = "nil";
        public uint vip = 0;
        public bool _isvipActive = false;
        private bool _inFb;
        public bool showFriend = false;
        public int clan_buff_lvl;
        public bool inFb
        {
            set
            {
                if (_inFb = value)
                    A3_BeStronger.Instance?.gameObject.SetActive(false);
                else
                    A3_BeStronger.Instance?.CheckUpItem();
            }
            get { return _inFb; }
        }
        public bool isvipActive
        {
            set
            {
                _isvipActive = value;
                if (_isvipActive == false)
                {
                    //   lgSelfPlayer.instance.grAvatar.refreshVipLv(0);
                    //HeroModel.getInstance().checkVipHeroGetOut();
                }
            }
            get { return _isvipActive; }
        }
        public bool _istitleActive = false;
        public bool istitleActive
        {
            set
            {
                _istitleActive = value;
                if (_istitleActive == false)
                {
                    if (SelfRole._inst != null)
                        SelfRole._inst.refreshtitle(0);
                }
            }
            get { return _istitleActive; }
        }
        public Vector3 enter_map_pos = new Vector3();
        public int now_pkState = 0;//当前的pk类型;
        public PK_TYPE pk_state = PK_TYPE.PK_PEACE;
        public uint m_unPK_Param = 0;
        public uint m_unPK_Param2 = 33333333;

        public bool firsrecharge = false;//是否首充过

        public int now_nameState = 0;//当前的红名类型
        public REDNAME_TYPE name_state = REDNAME_TYPE.RNT_NORMAL;
        public uint sinsNub;//罪恶值
        public uint hitBack = 0;//自身反击buff时间
        public uint clanid;//军团id
        public uint teamid;//队伍id
        public uint exp_time;//双倍金币卡时间

        public uint lvlsideid ;//阵营id
        public bool inSpost = false;
        public bool inCityWar = false;

        Dictionary<int, int> cur_att_pt = new Dictionary<int, int>();
        private uint _money;
        public uint money
        {
            get { return _money; }
            set
            {
                _money = value;
                a3_BagModel.getInstance()?.OnMoneyChange();
            }
        }
        public uint gift;
        public uint ach_point;//成就点
        public int nobpt; //声望值
        public uint gold;//钻石数量
        public uint _mapid;
        public uint mapid
        {
            get
            {
                return _mapid;
            }
            set
            {
                _mapid = value;
            }
        }//当前地图id
        public uint _lvl;//等级
        public uint treasure_num;//宝图数量
        public int serial;
        public uint lvl
        {
            get { return _lvl; }
            set
            {
                _lvl = value;
                //if (a3_expbar.instance != null) a3_expbar.instance.CheckNewSkill();
            }
        }
        public uint up_lvl = 0;//几转
        public uint exp;//当前经验值;
        public int accent_exp;//遗物经验
        public int hp;//当前hp
        public int mp;//当前mp
        public int combpt; //战斗力
        public bool _pkState = false;
        private int _max_hp;//hp上限
        public int max_hp
        {
            get
            {
                return _max_hp;
            }
            set
            {
                if (_max_hp == value)
                    return;

                _max_hp = value;
                if (lgSelfPlayer.instance != null)
                    lgSelfPlayer.instance.modMaxHp(value, this.hp);
            }
        }

        public float mapBeginX = 0f;
        public float mapBeginY = 0f;
        public float mapBeginroatate = 0f;


        public uint strength; //力量
        public uint agility; //敏捷
        public uint constitution; //体力
        public uint intelligence; //魔力
        public uint max_attack; //最大攻击力
        public uint physics_def; //物防
        public uint magic_def; //法防
        public uint fire_att; //火攻
        public uint ice_att; //冰攻
        public uint light_att; //光攻
        public uint fire_def; //火防
        public uint ice_def; //冰防
        public uint light_def; //光防
        public uint max_mp; //MP
        public uint crime; //罪恶值
        public uint mp_abate; //MP降低‰
        public uint hp_suck; //HP吸回
        public uint physics_dmg_red; //物理伤害减免‰
        public uint magic_dmg_red; //魔法伤害减免‰
        public uint skill_damage; //技能破坏力
        public uint fatal_att; //致命攻击
        public uint fatal_dodge; //致命闪避
        public uint max_hp_add; //增加HP‰
        public uint max_mp_add; //增加MP‰
        public uint hp_recovery; //HP恢复
        public uint mp_recovery; //MP恢复
        public uint mp_suck; //MP吸回
        public uint magic_shield; //魔法护盾
        public uint exp_add; //exp增加‰
        public uint blessing; //祝福
        public uint knowledge_add; //知识值增加
        public uint fatal_damage; //致命伤害提升‰
        public uint fire_def_add; //火防增加‰
        public uint ice_def_add; //冰防增加‰
        public uint light_def_add; //光防增加‰
        public uint wisdom; //智慧，增加MP上限
        public uint min_attack; //最小攻击力
        public uint double_damage_rate; // 39,双倍伤害概率
        public uint reflect_crit_rate; // 40,反射受到的致命伤害（暴击伤害）的概率
        public uint ignore_crit_rate; // 41,抵消（忽视）一次受到的致命伤害的概率
        public uint crit_add_hp; // 42,每次暴击恢复多少点生命值
        public uint hit; // 43,命中率
        public uint dodge; // 44,闪避率
        public uint ignore_defense_damage;//45无视防御伤害
        public uint stagger;//46硬直

        int _pt_att;    //剩余点数
        public int line ;//当前线路
        public int pt_att
        {
            get { return _pt_att; }
            set
            {
                //int x = _pt_att;
                _pt_att = value;

                if (PlayerModel.getInstance().up_lvl == 0 && PlayerModel.getInstance().lvl <= 80)
                {
                    autoAddPrint();
                }

                A3_BeStronger.Instance?.CheckUpItem();
            }
        }
        // 
        public int pt_strpt;  //力量
        public int pt_conpt;  //体力
        public int pt_intept; //魔力
        public int pt_wispt;  //智慧
        public int pt_agipt;  //敏捷

        public Dictionary<int, int> task_monsterId = new Dictionary<int, int>(); // 玩家当前所有任务要求击杀的怪物id
        public Dictionary<int, int> task_monsterIdOnAttack = new Dictionary<int, int>(); // 玩家当前选择去击杀的怪物id
        public Dictionary<uint, int> attr_list = new Dictionary<uint, int>();
        public Dictionary<uint, int> attChange_eqp = new Dictionary<uint, int>();       

        public bool inDefendArea = true;
        public bool isFirstRechange = false;
        public bool hasKaifuActive = true;

        public int last_time;
        public bool first;
        public bool havePet=false;
        public int selfPetTime;
        public void modHp(int hprest)
        {
            hp = hprest;
            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modHp", "model/PlayerModel", hp,max_hp);
             SelfRole .setMaxhp(max_hp);
        }

        public void modMp(int mprest)
        {
            mp = mprest;
            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modMp", "model/PlayerModel", mp,max_mp);
        }


        public bool showBaotu_ui = false;

        public int speed=0;//移动速度

        public void init(Variant data)
        {
            //耗时最厉害的步骤
            //Stopwatch a1;
            //a1 = new Stopwatch(); a1.Start();//开始计时
            //a3_BagModel.getInstance();
            //a1.Stop(); debug.Log("cost a3_BagModel =" + a1.ElapsedMilliseconds);//终止计时
            //a1 = new Stopwatch(); a1.Start();//开始计时

            //a1 = new Stopwatch(); a1.Start();//开始计时
            //SkillModel.getInstance();
            //a1.Stop(); debug.Log("cost SkillModel =" + a1.ElapsedMilliseconds);//终止计时
            //a1 = new Stopwatch(); a1.Start();//开始计时

            //a1 = new Stopwatch(); a1.Start();//开始计时
            //initProxy();
            //a1.Stop(); debug.Log("cost p88 =" + a1.ElapsedMilliseconds);//终止计时
            //a1 = new Stopwatch(); a1.Start();//开始计时
            //InterfaceMgr.doCommandByLua("PlayerModel:getInstance().initInfo", "model/PlayerModel", data);
            //a1.Stop(); debug.Log("cost p99 =" + a1.ElapsedMilliseconds);//终止计时


            Globle.inGame = true;
            line = data["line"];
            if (!data.ContainsKey("battleAttrs"))
                return;

            profession = data["carr"];
            cid = data["cid"];
            uid = data["uid"];
            iid = data["iid"];
            name = data["name"];

            speed = data[ "speed" ];

            clan_buff_lvl = data["clan_buff_lvl"];
            enter_map_pos.x = data["x"] / GameConstant.PIXEL_TRANS_UNITYPOS;
            enter_map_pos.y = 0f;
            enter_map_pos.z = data["y"] / GameConstant.PIXEL_TRANS_UNITYPOS;

            debug.Log("玩家出生的坐标为" + enter_map_pos);

            if (data.ContainsKey("crttm"))
            {
                crttm = data["crttm"];
            }

            if (data.ContainsKey("yb"))
            {
                gold = data["yb"];
            }
            if (data.ContainsKey("ach_point"))
            {
                ach_point = data["ach_point"];
            }
            if (data.ContainsKey("money"))
            {
                money = data["money"];
            }
            if (data.ContainsKey("bndyb"))
            {
                gift = data["bndyb"];
            }
            if (data.ContainsKey("zhuan"))
            {
                up_lvl = data["zhuan"];
            }
            if (data.ContainsKey("pet_food_last_time"))
            {
                last_time = data["pet_food_last_time"];
                a3_expbar.feedTime = last_time;
            }
            if (data.ContainsKey("first_pet_food"))
            {
                first = data["first_pet_food"];
            }
            if (data.ContainsKey("pet"))
            {
                if (data["pet"]["id"] > 0)
                {
                    havePet = true;
                }
                else
                    havePet = false;
                Variant spet = data["pet"];
                A3_PetModel cpet = A3_PetModel.getInstance();
                cpet.Tpid = spet["id"];
                A3_PetModel.curPetid = spet["id"];
                A3_PetModel.getInstance().petId = data["pet"]["id_arr"]._arr;

            }
            if (data.ContainsKey("treasure_num"))
            {
                treasure_num = data["treasure_num"];
                //Debug.LogError("treasure_num"  + treasure_num);
                if (treasure_num >= 50)
                {
                    showBaotu_ui = true;
                    //InterfaceMgr.getInstance().open(InterfaceMgr.A3_BAOTUUI);
                }
                //SelfRole._inst.refreshmapCount(data["treasure_num"]);
            }

            if (data.ContainsKey("first_double"))
            {
                foreach (int info in data["first_double"]._arr)
                {
                    if (RechargeModel.getInstance().rechargeMenu.ContainsKey(info))
                    {
                        if (RechargeModel.getInstance().rechargeMenu[info].first_double >= 1)
                        {
                            if (!RechargeModel.getInstance().firsted.Contains(info)) { RechargeModel.getInstance().firsted.Add(info); }
                        }
                    }
                }
            }

                if (data.ContainsKey("serial_kp"))
            {
                serial = data["serial_kp"];
            }
            mapid = data["mpid"];
            lvl = data["lvl"];
            oldLv = lvl;
            exp = data["exp"];
            if (data.ContainsKey("remains_exp"))
            {
                accent_exp = data["remains_exp"];
            }
            hp = data["hp"];
            combpt = data["combpt"];
            oldCombpt = combpt;

            inDefendArea = !data["in_pczone"]._bool;
            clanid = data["clanid"];
            if (data.ContainsKey("teamid"))
            {
                teamid = data["teamid"];
            }
            pkState = data["pk_state"] == 1;
            // if (data["zhuan"] >= 1)
            {
                if (data.ContainsKey("pk_state"))
                {
                    now_pkState = data["pk_state"];
                }

                //now_pkState = 0;
                switch (now_pkState)
                {
                    case 0:
                        pk_state = PK_TYPE.PK_PEACE;
                        break;
                    case 1:
                        pk_state = PK_TYPE.PK_PKALL;
                        m_unPK_Param = cid;
                        m_unPK_Param2 = cid;
                        break;
                    case 2:
                        pk_state = PK_TYPE.PK_TEAM;
                        m_unPK_Param = teamid;
                        m_unPK_Param2 = clanid;
                        break;
                    case 3:
                        pk_state = PK_TYPE.PK_LEGION;
                        m_unPK_Param = clanid;
                        break;
                    case 4:
                        pk_state = PK_TYPE.PK_HERO;
                        //？？？
                        break;
                    case 5:
                        pk_state = PK_TYPE.Pk_SPOET;

                        break;
                }
                if (data.ContainsKey("rednm"))
                {
                    now_nameState = data["rednm"];
                    //debug.Log("红名类型：" + now_nameState);

                }
                if (data.ContainsKey("pk_v"))
                {

                    sinsNub = data["pk_v"];


                    crime = data["pk_v"];
                    attr_list[16] = data["pk_v"];
                    debug.Log("罪恶值：" + sinsNub);
                }
                if (data.ContainsKey("strike_back_tm"))
                {
                    hitBack = data["strike_back_tm"] - (uint)NetClient.instance.CurServerTimeStamp;
                    // debug.Log("反击时间：" + data["strike_back_tm"] + "当前服务器时间：" + NetClient.instance.CurServerTimeStamp);
                }
                switch (now_nameState)
                {
                    case 0:
                        name_state = REDNAME_TYPE.RNT_NORMAL;
                        break;
                    case 1:
                        name_state = REDNAME_TYPE.RNT_RASCAL;
                        break;
                    case 2:
                        name_state = REDNAME_TYPE.RNT_EVIL;
                        break;
                    case 3:
                        name_state = REDNAME_TYPE.RNT_DEVIL;
                        break;
                };
            }


            pt_att = data["att_pt"];
            pt_strpt = data["strpt"];
            pt_conpt = data["conpt"];
            pt_intept = data["intept"];
            pt_wispt = data["wispt"];
            pt_agipt = data["agipt"];


            if (data.ContainsKey("battleAttrs"))
            {
                attrChange(data);
            }
            if (data.ContainsKey("nobpt"))
            {//声望点
                PlayerModel.getInstance().nobpt = data["nobpt"];
            }
            if (data.ContainsKey("curpets"))
            {
                //Variant petMon = data["curpets"];
                //if (petMon.ContainsKey("pets1"))
                //    HeroModel.getInstance().setZx(1, parsePet(petMon["pets1"]._arr), false);
                //if (petMon.ContainsKey("pets2"))
                //    HeroModel.getInstance().setZx(2, parsePet(petMon["pets2"]._arr), false);
                //if (petMon.ContainsKey("pets3"))
                //    HeroModel.getInstance().setZx(3, parsePet(petMon["pets3"]._arr), false);
                //HeroModel.getInstance().changeZx(petMon["curZxIdx"]._int);
            }

            SkillModel.getInstance().initSkillList(data["skills"]._arr);
            //a3的skill
            Skill_a3Model.getInstance().initSkillList(data["skills"]._arr);
            Skill_a3Model.getInstance().skillGroups(data["skill_groups"]._arr);
            debug.Log("技能信息：" + data["skills"].dump());
            Skill_a3Model.getInstance().skills = data["skills"];
            if (data["skills"].Length > 0)
            {
                foreach (Variant v in data["skills"]._arr)
                {
                    Skill_a3Model.getInstance().skillinfos(v["skill_id"], v["skill_level"]);
                }
            }
            if (data.ContainsKey("items"))
            {
                a3_BagModel.getInstance().initItemList(data["items"]._arr);
            }

            if (data.ContainsKey("equipments2"))
            {
                a3_EquipModel.getInstance().initEquipList(data["equipments2"]._arr);
            }
            if(data.ContainsKey("dress_list"))
            {
                if(data["dress_list"]!=null&& data["dress_list"].Count>0)
                {
                        A3_FashionShowModel.getInstance().first_nowfs[0]=data["dress_list"][0];
                        A3_FashionShowModel.getInstance().first_nowfs[1] = data["dress_list"][1];
                }
                else
                {
                    A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().first_nowfs[1] = 0;
                }
            }
            //符石
            if (data.ContainsKey("eqp_stones2"))
            {
                A3_RuneStoneModel.getInstance().initDressupInfos(data["eqp_stones2"]._arr);
            }

            //debug.Log("时装数据------------------------------------------------------------------");
            if (data.ContainsKey("dressments"))
            {
                //debug.Log("有要更新的时装数据");
                List<Variant> dressments = data["dressments"]._arr;
                foreach (Variant dressone in dressments)
                {
                    int ndressid = dressone["dressid"]._int;
                    SXML dress_info = XMLMgr.instance.GetSXML("dress.dress_info", "id==" + ndressid);
                    int npartid = dress_info.getInt("dress_type");
                    if (npartid >= 0 && npartid < lgSelfPlayer.instance.m_nDress_PartID.Length)
                    {
                        lgSelfPlayer.instance.m_nDress_PartID[npartid] = ndressid;
                    }

                    debug.Log("时装的ID " + dressone["dressid"]);
                }

                //EquipModel.getInstance().initEquipList(data["equipments"]._arr);
            }


            if (data.ContainsKey("has_kaifuactivity"))
                hasKaifuActive = data["has_kaifuactivity"];

            //if (minimap.instance != null)
            //{
            //    minimap.instance.normalUi.activebt.gameObject.SetActive(hasKaifuActive);
            //}

            AutoPlayModel.getInstance().Init();

            //debug.Log("翅膀数据------------------------------------------------------------------");
            if (data.ContainsKey("wing"))
            {
                int wingID = data["wing"]["show_stage"];
                A3_WingModel wingModel = A3_WingModel.getInstance();

                if (wingID > 0)
                {
                    wingModel.ShowStage = wingID;
                }
                else
                {
                    wingModel.ShowStage = 0;
                }
            }
            if (data.ContainsKey("ach_point"))
            {
                istitleActive = data["ach_title"] > 0 ? true : false;
                istitleActive = data["title_display"]._bool;
                a3_RankModel.getInstance().refreinfo(data["ach_title"], data["ach_point"], data["title_display"]._bool);

            }
            FunctionOpenMgr.instance.onLvUp((int)up_lvl, (int)lvl);
            initProxy();
            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().initInfo", "model/PlayerModel", data);

            GameSdkMgr.record_login();

        }

        public void attrChange(Variant data)
        {
            //debug.Log("XXXX"+data.dump());
            Variant att = data["battleAttrs"];
            strength = att["strength"];
            agility = att["agility"];
            constitution = att["constitution"];
            intelligence = att["intelligence"];
            max_attack = att["max_attack"];
            physics_def = att["physics_def"];
            magic_def = att["magic_def"];
            fire_att = att["fire_att"];
            ice_att = att["ice_att"];
            light_att = att["light_att"];
            fire_def = att["fire_def"];
            ice_def = att["ice_def"];
            light_def = att["light_def"];
            max_hp = att["max_hp"];
            max_mp = att["max_mp"];
            //crime = att["crime"];
            mp_abate = att["mp_abate"];
            hp_suck = att["hp_suck"];
            physics_dmg_red = att["physics_dmg_red"];
            magic_dmg_red = att["magic_dmg_red"];
            skill_damage = att["skill_damage"];
            fatal_att = att["fatal_att"];
            fatal_dodge = att["fatal_dodge"];
            max_hp_add = att["max_hp_add"];
            max_mp_add = att["max_mp_add"];
            hp_recovery = att["hp_recovery"];
            mp_recovery = att["mp_recovery"];
            mp_suck = att["mp_suck"];
            magic_shield = att["magic_shield"];
            exp_add = att["exp_add"];
            blessing = att["blessing"];
            knowledge_add = att["knowledge_add"];
            fatal_damage = att["fatal_damage"];
            fire_def_add = att["fire_def_add"];
            ice_def_add = att["ice_def_add"];
            light_def_add = att["light_def_add"];
            wisdom = att["wisdom"];
            min_attack = att["min_attack"];
            if (att.ContainsKey("hit"))
            {
                double_damage_rate = att["double_damage_rate"];
                reflect_crit_rate = att["reflect_crit_rate"];
                ignore_crit_rate = att["ignore_crit_rate"];
                crit_add_hp = att["crit_add_hp"];
                hit = att["hit"];
                dodge = att["dodge"];
                ignore_defense_damage = att["ignore_defense_damage"];
                stagger = att["stagger"];
            }


            attr_list[1] = att["strength"];
            attr_list[2] = att["agility"];
            attr_list[3] = att["constitution"];
            attr_list[4] = att["intelligence"];
            attr_list[5] = att["max_attack"];
            attr_list[6] = att["physics_def"];
            attr_list[7] = att["magic_def"];
            attr_list[8] = att["fire_att"];
            attr_list[9] = att["ice_att"];
            attr_list[10] = att["light_att"];
            attr_list[11] = att["fire_def"];
            attr_list[12] = att["ice_def"];
            attr_list[13] = att["light_def"];
            attr_list[14] = att["max_hp"];
            attr_list[15] = att["max_mp"];
            //attr_list[16] = att["crime"];
            attr_list[17] = att["mp_abate"];
            attr_list[18] = att["hp_suck"];
            attr_list[19] = att["physics_dmg_red"];
            attr_list[20] = att["magic_dmg_red"];
            attr_list[21] = att["skill_damage"];
            attr_list[22] = att["fatal_att"];
            attr_list[23] = att["fatal_dodge"];
            attr_list[24] = att["max_hp_add"];
            attr_list[25] = att["max_mp_add"];
            attr_list[26] = att["hp_recovery"];
            attr_list[27] = att["mp_recovery"];
            attr_list[28] = att["mp_suck"];
            attr_list[29] = att["magic_shield"];
            attr_list[30] = att["exp_add"];
            attr_list[31] = att["blessing"];
            attr_list[32] = att["knowledge_add"];
            attr_list[33] = att["fatal_damage"];
            attr_list[34] = att["wisdom"];
            attr_list[35] = att["fire_def_add"];
            attr_list[36] = att["ice_def_add"];
            attr_list[37] = att["light_def_add"];
            attr_list[38] = att["min_attack"];
            if (att.ContainsKey("hit"))
            {
                attr_list[39] = att["double_damage_rate"];
                attr_list[40] = att["reflect_crit_rate"];
                attr_list[41] = att["ignore_crit_rate"];
                attr_list[42] = att["crit_add_hp"];
                attr_list[43] = att["hit"];
                attr_list[44] = att["dodge"];
                attr_list[45] = att["ignore_defense_damage"];
                attr_list[46] = att["stagger"];
            }
        }

        public void attrChangeCheck(Variant att)
        {
            bool change = false;
            attChange_eqp.Clear();
            if (att.ContainsKey("strength")) { attChange_eqp[1] = (int)(att["strength"] - strength); strength = att["strength"]; attr_list[1] = att["strength"]; change = true; }
            if (att.ContainsKey("agility")) { attChange_eqp[2] = (int)(att["agility"] - agility); agility = att["agility"]; attr_list[2] = att["agility"]; change = true; }
            if (att.ContainsKey("constitution")) { attChange_eqp[3] = (int)(att["constitution"] - constitution); constitution = att["constitution"]; attr_list[3] = att["constitution"]; change = true; }
            if (att.ContainsKey("intelligence")) { attChange_eqp[4] = (int)(att["intelligence"] - intelligence); intelligence = att["intelligence"]; attr_list[4] = att["intelligence"]; change = true; }
            if (att.ContainsKey("max_attack")) { attChange_eqp[5] = (int)(att["max_attack"] - max_attack); max_attack = att["max_attack"]; attr_list[5] = att["max_attack"]; change = true; }
            if (att.ContainsKey("physics_def")) { attChange_eqp[6] = (int)(att["physics_def"] - physics_def); physics_def = att["physics_def"]; attr_list[6] = att["physics_def"]; change = true; }
            if (att.ContainsKey("magic_def")) { attChange_eqp[7] = (int)(att["magic_def"] - magic_def); magic_def = att["magic_def"]; attr_list[7] = att["magic_def"]; change = true; }
            if (att.ContainsKey("fire_att")) { attChange_eqp[8] = (int)(att["fire_att"] - fire_att); fire_att = att["fire_att"]; attr_list[8] = att["fire_att"]; change = true; }
            if (att.ContainsKey("ice_att")) { attChange_eqp[9] = (int)(att["ice_att"] - ice_att); ice_att = att["ice_att"]; attr_list[9] = att["ice_att"]; change = true; }
            if (att.ContainsKey("light_att")) { attChange_eqp[10] = (int)(att["light_att"] - light_att); light_att = att["light_att"]; attr_list[10] = att["light_att"]; change = true; }
            if (att.ContainsKey("fire_def")) { attChange_eqp[11] = (int)(att["fire_def"] - fire_def); fire_def = att["fire_def"]; attr_list[11] = att["fire_def"]; change = true; }
            if (att.ContainsKey("ice_def")) { attChange_eqp[12] = (int)(att["ice_def"] - ice_def); ice_def = att["ice_def"]; attr_list[12] = att["ice_def"]; change = true; }
            if (att.ContainsKey("light_def")) { attChange_eqp[13] = (int)(att["light_def"] - light_def); light_def = att["light_def"]; attr_list[13] = att["light_def"]; change = true; }
            if (att.ContainsKey("max_hp")) { attChange_eqp[14] = (int)(att["max_hp"] - max_hp); max_hp = att["max_hp"]; attr_list[14] = att["max_hp"]; change = true; }
            if (att.ContainsKey("max_mp")) { attChange_eqp[15] = (int)(att["max_mp"] - max_mp); max_mp = att["max_mp"]; attr_list[15] = att["max_mp"]; change = true; }
            //if (att.ContainsKey("crime")) { attChange_eqp[16] = (int)(att["crime"] - crime); crime = att["crime"]; attr_list[16] = att["crime"]; change = true; }
            if (att.ContainsKey("mp_abate")) { attChange_eqp[17] = (int)(att["mp_abate"] - mp_abate); mp_abate = att["mp_abate"]; attr_list[17] = att["mp_abate"]; change = true; }
            if (att.ContainsKey("hp_suck")) { attChange_eqp[18] = (int)(att["hp_suck"] - hp_suck); hp_suck = att["hp_suck"]; attr_list[18] = att["hp_suck"]; change = true; }
            if (att.ContainsKey("physics_dmg_red")) { attChange_eqp[19] = (int)(att["physics_dmg_red"] - physics_dmg_red); physics_dmg_red = att["physics_dmg_red"]; attr_list[19] = att["physics_dmg_red"]; change = true; }
            if (att.ContainsKey("magic_dmg_red")) { attChange_eqp[20] = (int)(att["magic_dmg_red"] - magic_dmg_red); magic_dmg_red = att["magic_dmg_red"]; attr_list[20] = att["magic_dmg_red"]; change = true; }
            if (att.ContainsKey("skill_damage")) { attChange_eqp[21] = (int)(att["skill_damage"] - skill_damage); skill_damage = att["skill_damage"]; attr_list[21] = att["skill_damage"]; change = true; }
            if (att.ContainsKey("fatal_att")) { attChange_eqp[22] = (int)(att["fatal_att"] - fatal_att); fatal_att = att["fatal_att"]; attr_list[22] = att["fatal_att"]; change = true; }
            if (att.ContainsKey("fatal_dodge")) { attChange_eqp[23] = (int)(att["fatal_dodge"] - fatal_dodge); fatal_dodge = att["fatal_dodge"]; attr_list[23] = att["fatal_dodge"]; change = true; }
            if (att.ContainsKey("max_hp_add")) { attChange_eqp[24] = (int)(att["max_hp_add"] - max_hp_add); max_hp_add = att["max_hp_add"]; attr_list[24] = att["max_hp_add"]; change = true; }
            if (att.ContainsKey("max_mp_add")) { attChange_eqp[25] = (int)(att["max_mp_add"] - max_mp_add); max_mp_add = att["max_mp_add"]; attr_list[25] = att["max_mp_add"]; change = true; }
            if (att.ContainsKey("hp_recovery")) { attChange_eqp[26] = (int)(att["hp_recovery"] - hp_recovery); hp_recovery = att["hp_recovery"]; attr_list[26] = att["hp_recovery"]; change = true; }
            if (att.ContainsKey("mp_recovery")) { attChange_eqp[27] = (int)(att["mp_recovery"] - mp_recovery); mp_recovery = att["mp_recovery"]; attr_list[27] = att["mp_recovery"]; change = true; }
            if (att.ContainsKey("mp_suck")) { attChange_eqp[28] = (int)(att["mp_suck"] - mp_suck); mp_suck = att["mp_suck"]; attr_list[28] = att["mp_suck"]; change = true; }
            if (att.ContainsKey("stremagic_shieldngth")) { attChange_eqp[29] = (int)(att["magic_shield"] - magic_shield); magic_shield = att["magic_shield"]; attr_list[29] = att["magic_shield"]; change = true; }
            if (att.ContainsKey("exp_add")) { attChange_eqp[30] = (int)(att["exp_add"] - exp_add); exp_add = att["exp_add"]; attr_list[30] = att["exp_add"]; change = true; }
            if (att.ContainsKey("blessing")) { attChange_eqp[31] = (int)(att["blessing"] - blessing); blessing = att["blessing"]; attr_list[31] = att["blessing"]; change = true; }
            if (att.ContainsKey("knowledge_add")) { attChange_eqp[32] = (int)(att["knowledge_add"] - knowledge_add); knowledge_add = att["knowledge_add"]; attr_list[32] = att["knowledge_add"]; change = true; }
            if (att.ContainsKey("fatal_damage")) { attChange_eqp[33] = (int)(att["fatal_damage"] - fatal_damage); fatal_damage = att["fatal_damage"]; attr_list[33] = att["fatal_damage"]; change = true; }
            if (att.ContainsKey("wisdom")) { attChange_eqp[34] = (int)(att["wisdom"] - wisdom); wisdom = att["wisdom"]; attr_list[34] = att["wisdom"]; change = true; }
            if (att.ContainsKey("fire_def_add")) { attChange_eqp[35] = (int)(att["fire_def_add"] - fire_def_add); fire_def_add = att["fire_def_add"]; attr_list[35] = att["fire_def_add"]; change = true; }
            if (att.ContainsKey("ice_def_add")) { attChange_eqp[36] = (int)(att["ice_def_add"] - ice_def_add); ice_def_add = att["ice_def_add"]; attr_list[36] = att["ice_def_add"]; change = true; }
            if (att.ContainsKey("light_def_add")) { attChange_eqp[37] = (int)(att["light_def_add"] - light_def_add); light_def_add = att["light_def_add"]; attr_list[37] = att["light_def_add"]; change = true; }
            if (att.ContainsKey("min_attack")) { attChange_eqp[38] = (int)(att["min_attack"] - min_attack); min_attack = att["min_attack"]; attr_list[38] = att["min_attack"]; change = true; }
            if (att.ContainsKey("double_damage_rate")) { attChange_eqp[39] = (int)(att["double_damage_rate"] - double_damage_rate); double_damage_rate = att["double_damage_rate"]; attr_list[39] = att["double_damage_rate"]; change = true; }
            if (att.ContainsKey("reflect_crit_rate")) { attChange_eqp[40] = (int)(att["reflect_crit_rate"] - reflect_crit_rate); reflect_crit_rate = att["reflect_crit_rate"]; attr_list[40] = att["reflect_crit_rate"]; change = true; }
            if (att.ContainsKey("ignore_crit_rate")) { attChange_eqp[41] = (int)(att["ignore_crit_rate"] - ignore_crit_rate); ignore_crit_rate = att["ignore_crit_rate"]; attr_list[41] = att["ignore_crit_rate"]; change = true; }
            if (att.ContainsKey("crit_add_hp")) { attChange_eqp[42] = (int)(att["crit_add_hp"] - crit_add_hp); crit_add_hp = att["crit_add_hp"]; attr_list[42] = att["crit_add_hp"]; change = true; }
            if (att.ContainsKey("hit")) { attChange_eqp[43] = (int)(att["hit"] - hit); hit = att["hit"]; attr_list[43] = att["hit"]; change = true; }
            if (att.ContainsKey("dodge")) { attChange_eqp[44] = (int)(att["dodge"] - dodge); dodge = att["dodge"]; attr_list[44] = att["dodge"]; change = true; }
            if (att.ContainsKey("ignore_defense_damage")) { attChange_eqp[45] = (int)(att["ignore_defense_damage"] - ignore_defense_damage); ignore_defense_damage = att["ignore_defense_damage"]; attr_list[45] = att["ignore_defense_damage"]; change = true; }
            if (att.ContainsKey("stagger")) { attChange_eqp[46] = (int)(att["stagger"] - stagger); stagger = att["stagger"]; attr_list[46] = att["stagger"]; change = true; }

            //罪恶值用“pa_v”，摒弃crime
            if (att.ContainsKey("pk_v")) { sinsNub = att["pk_v"]; crime = sinsNub; attr_list[16] = (int)sinsNub; change = true; }
            if (att.ContainsKey("pk_v"))
            {
                debug.Log("更新罪恶值：" + sinsNub);
                if (a3_expbar.instance)
                {
                    a3_expbar.instance.ShowWashRed();
                    if (a3_washredname._instance)
                        a3_washredname._instance.point();
                }

            }
            if (att.ContainsKey("equip"))
            {
                if (att["equip"])
                {
                    Dictionary<uint, int> list = new Dictionary<uint, int>();
                    foreach (uint id in attChange_eqp.Keys)
                    {
                        if (attChange_eqp[id] > 0)
                        {
                           
                            list[id] = attChange_eqp[id];
                           
                        }
                    }
                    if (list.Count > 0) {
                        //ArrayList l = new ArrayList();
                        //l.Add(list);
                        //InterfaceMgr.getInstance().open(InterfaceMgr.A3_ATTCHANGE, l);
                        a3_attChange.instans.runTxt(list);
                    }
                }
            }

            if (change)
                dispatchEvent(GameEvent.Create(ON_ATTR_CHANGE, this, null));
        }

        public void attPointCheck(Variant att)
        {
            debug.Log(att.dump());
            if (att.ContainsKey("att_pt")) { pt_att = att["att_pt"]; }
            if (att.ContainsKey("strpt")) { pt_strpt = att["strpt"]; }
            if (att.ContainsKey("conpt")) { pt_conpt = att["conpt"]; }
            if (att.ContainsKey("intept")) { pt_intept = att["intept"]; }
            if (att.ContainsKey("wispt")) { pt_wispt = att["wispt"]; }
            if (att.ContainsKey("agipt")) { pt_agipt = att["agipt"]; }


            if (att.ContainsKey("rednm")) { now_nameState = att["rednm"]; }
            switch (now_nameState)
            {
                case 0:
                    name_state = REDNAME_TYPE.RNT_NORMAL;
                    break;
                case 1:
                    name_state = REDNAME_TYPE.RNT_RASCAL;
                    break;
                case 2:
                    name_state = REDNAME_TYPE.RNT_EVIL;
                    break;
                case 3:
                    name_state = REDNAME_TYPE.RNT_DEVIL;
                    break;
            };
            if (SelfRole._inst != null)
                SelfRole._inst.refreshnamecolor(now_nameState);
        }

        public void LeaveStandalone_CreateChar()
        {
            SelfRole.s_bStandaloneScene = false;
            a1_gamejoy.inst_skillbar?.getGameObjectByPath("skillbar/combat/apbtn")?.SetActive(true);
            a1_gamejoy.inst_skillbar?.getGameObjectByPath("skillbar/combat/bt_changeLock")?.SetActive(true);
            UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_SELECT_CHAR, this, GameTools.createGroup("cid", cid)));
            UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_ENTER_GAME, this, GameTools.createGroup("cid", cid)));
        }

        public uint oldLv = 1;
        public uint olduplvl = 0;
        public int oldCombpt = 0;
        public void lvUp(Variant data)
        {
            debug.Log(":::::" + data.dump());
            if (data.ContainsKey("zhuan"))
            {
                olduplvl = up_lvl;
                up_lvl = data["zhuan"];
                //角色升级了，看有没有等级礼包可以领（福利的iconlight）
                welfareProxy.b_zhuan = WelfareModel.getInstance().for_dengjilibao(welfareProxy.getInstance().dengjijiangli);
                //welfareProxy.getInstance().showIconLight();
                //角色升级了，看看有没有解锁新的首领地图；(首领的iconlight)
                EliteMonsterProxy.getInstance().SendProxy();
            }
            if (data.ContainsKey("lvl"))
            {
                //角色升级了，看看有没有解锁新的首领地图；(首领的iconlight)
                EliteMonsterProxy.getInstance().SendProxy();
                if (data["lvl"] > lvl )
                {
                    lvl = data["lvl"];
                    //if (muLGClient.instance.g_mapCT.curMapId == 1)
                    //{
                    oldLv = lvl;
                    if (SelfRole._inst != null)
                        SelfRole._inst.ShowLvUpFx();

                    //InterfaceMgr.getInstance().open(InterfaceMgr.UPLEVEL);
                    //}

                    //  LGPlatInfo.inst.logSDKAP("setLevel");
                    if(data["zhuan"] <= olduplvl)//转生时在转生动画结束后检测
                        FunctionOpenMgr.instance.onLvUp((int)up_lvl, (int)lvl, true);

                    if (a3_funcopen.instance != null)
                    {//功能开启界面是优先显示，不显示升阶提示
                        if (!a3_funcopen.instance.is_show)
                        {
                            if (a3_lvup.instance != null)
                                a3_lvup.instance.refreshInfo(lvl);
                        }
                    }


                    dispatchEvent(GameEvent.Create(ON_LEVEL_CHANGED, this, lvl));
                }
                else
                {
                    lvl = data["lvl"];//用于转生之后更新UI
                }
                MediaClient.instance.PlaySoundUrl("audio_common_levelup", false, null);

                GameSdkMgr.record_LvlUp();
                if (a3_expbar.instance)
                {
                    a3_expbar.instance.showiconHit();
                }
            }
            olduplvl = up_lvl;
            if (data.ContainsKey("combpt"))
            {
                //if (combpt < data["combpt"])
                //{
                //    if (muLGClient.instance.g_mapCT.curMapId == 1)
                //    {
                //        fightingup.instance.runTxt(combpt, data["combpt"]);
                //        oldCombpt = data["combpt"];
                //    }
                //}
                combpt = data["combpt"];
                dispatchEvent(GameEvent.Create(ON_ATTR_CHANGE, this, null));
            }

            if (data.ContainsKey("pinfo"))
            {
                Variant info = data["pinfo"];
                exp = info["exp"];
                hp = info["hp"];

                //if (combpt < info["combpt"])
                //{
                //    if (muLGClient.instance.g_mapCT.curMapId == 1)
                //    {
                //        fightingup.instance.runTxt(combpt, info["combpt"]);
                //        oldCombpt = info["combpt"];
                //    }
                //}
                combpt = info["combpt"];
                if (info.ContainsKey("battleAttrs"))
                {
                    PlayerModel.getInstance().attrChange(info);
                    dispatchEvent(GameEvent.Create(ON_ATTR_CHANGE, this, null));
                }
            }

            InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modInfo", "model/PlayerModel", data);
        }

        void autoAddPrint()
        {        //1.力量   2.魔力 3.敏捷 4.体力  5.智慧
            Dictionary<int, int> add_pt = new Dictionary<int, int>();
            //if (PlayerModel.getInstance().profession == 2)
            //{//战士
            //    int[] addtype = { 4, 3, 1, 5 };
            //    int[] tem = { 2, 2, 1, 1 };
            //    addPointAuto(_pt_att, addtype, tem);
            //}
            //if (PlayerModel.getInstance().profession == 3)
            //{//法师
            //    int[] addtype = { 2, 4, 1, 5 };
            //    int[] tem = { 2, 2, 1, 1 };
            //    addPointAuto(_pt_att, addtype, tem);
            //}
            //if (PlayerModel.getInstance().profession == 5)
            //{//刺客
            //    int[] addtype = { 3, 1, 4, 5 };
            //    int[] tem = { 2, 1, 2, 1 };
            //    addPointAuto(_pt_att, addtype, tem);
            //}
            SXML Xml = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + PlayerModel.getInstance().profession);
            string[] type = Xml.GetNode("point").getString("type").Split(',');
            string[] volue = Xml.GetNode("point").getString("autovolue").Split(',');
            int[] addtype = { int.Parse(type[0]), int.Parse(type[1]), int.Parse(type[2]), int.Parse(type[3]) };
            int[] tem = { int.Parse(volue[0]), int.Parse(volue[1]), int.Parse(volue[2]), int.Parse(volue[3]) };
            addPointAuto(_pt_att, addtype, tem);


            foreach (int key in cur_att_pt.Keys)
            {
                if (cur_att_pt[key] > 0)
                    add_pt[key] = cur_att_pt[key];
            }
            if (add_pt.Count > 0)
                PlayerInfoProxy.getInstance().sendAddPoint(0, add_pt);
        }
        void addPointAuto(int left_num, int[] addtype, int[] tem)
        {
            int[] add = { 0, 0, 0, 0 };

            int sum = 0;
            for (int i = 0; i < tem.Length; i++)
            {
                sum += tem[i];
            }
            int a = (int)Math.Floor((double)left_num / sum);
            int b = left_num % sum;

            for (int i = 0; i < 4; i++)
            {
                add[i] = tem[i] * a;
            }

            if (b > 0)
            {
                int over_num = b;
                while (over_num > 0)
                {
                    if (over_num >= 4)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        add[2] += 1;
                        add[3] += 1;
                        over_num -= 4;
                    }
                    if (over_num >= 3)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        add[2] += 1;
                        over_num -= 3;
                    }
                    if (over_num >= 2)
                    {
                        add[0] += 1;
                        add[1] += 1;
                        over_num -= 2;
                    }
                    if (over_num >= 1)
                    {
                        add[0] += 1;
                        over_num -= 1;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                cur_att_pt[addtype[i]] = add[i];
            }

        }

        public void isShowVipUpLayer()
        {
            if (lvl > oldLv)
            {
                if (muLGClient.instance.g_mapCT.curMapId == 1)
                {
                    oldLv = lvl;
                    //InterfaceMgr.getInstance().open(InterfaceMgr.UPLEVEL);
                }
            }
        }

        bool isFirstVipLoad = true;
        public void vipChange(Variant data)
        {
            //uint oldvip = vip;
            //vip = data["vip_lv"];
            //if (vip != 0 && vip > oldvip && !isFirstVipLoad)
            //{
            //    InterfaceMgr.getInstance().open(InterfaceMgr.VIP_UP);
            //}

            //if (lgSelfPlayer.instance.grAvatar != null)
            //    lgSelfPlayer.instance.grAvatar.refreshVipLv(vip);

            //isFirstVipLoad = false;
        }
        public void titileChange(Variant data)
        {
            if (data["title"] > 0)
                istitleActive = true;
            else
                istitleActive = false;
            if (SelfRole._inst != null)
            {
                SelfRole._inst.refreshtitle(data["title"]);
            }
        }
        public void titleShoworHide(Variant data)
        {
            istitleActive = data["title_display"]._bool;
            if (data["title_display"]._bool)
            {
                if (a3_RankModel.now_id > 0)
                {
                    if (SelfRole._inst != null)
                    {
                        SelfRole._inst.refreshtitle(a3_RankModel.now_id);
                    }
                }
                else
                    istitleActive = false;
            }
        }
        void initProxy()
        {
            //a3_activeOnlineProxy.getInstance().SendProxy(1);
           
            A3_FashionShowModel.getInstance();
            a3_fashionshowProxy.getInstance().SendProxys(1, null);
            a3_ActiveOnlineModel.getInstance();
            A3_SevendayModel.getInstance();
            A3_HallowsModel.getInstance();
            A3_HallowsProxy.getInstance().SendHallowsProxy(1);
            A3_HallowsProxy.getInstance().SendHallowsProxy(10);//游戏兑换码
            a3_RuneStoneProxy.getInstance().sendporxy(5);
            LevelProxy.getInstance().getAwd_zhs(1);
            A3_BeStrongerProxy.getInstance();
            A3_LegionProxy.getInstance().SendGetInfo();//获取玩家军团信息
            A3_cityOfWarProxy.getInstance().sendProxy(1);
            A3_TaskProxy.getInstance().SendGetTask();//获取玩家任务信息
            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE);//获取玩家成就信息
            A3_WingProxy.getInstance().GetWings();
            A3_MailProxy.getInstance().GetMails();
           // A3_NPCShopProxy.getInstance().sendShowAll();
            a3_dartproxy.getInstance().sendDartGo();//查看军团镖车信息
            //A3_PetProxy.getInstance().GetPets();
            A3_ygyiwuProxy.getInstance().SendYGinfo(1);
            EliteMonsterProxy.getInstance().SendProxy();
            // A3_VipProxy.getInstance().GetVip();
            //ExchangeProxy.getInstance().GetExchangeInfo();
            A3_SmithyProxy.getInstance();
            a3_newActiveProxy.getInstance();
            a3_newActiveProxy.getInstance().SendProxy(1);
            a3_newActiveProxy.getInstance().SendProxy(2);
            BagProxy.getInstance().sendLoadItems(0);
            BagProxy.getInstance().sendLoadItems(1);
            SkillProxy.getInstance();
            GeneralProxy.getInstance();
            FindBestoProxy.getInstance();
            Skill_a3Proxy.getInstance();
            A3_ygyiwuProxy.getInstance();
            a3_PkmodelProxy.getInstance();
            A3_VipProxy.getInstance().GetVip();
            A3_HudunProxy.getInstance().sendinfo(0);
            a3_activeDegreeProxy.getInstance().SendGetPoint(1);//活跃度的协议
            TeamProxy.getInstance();
            A3_RideProxy.getInstance().SendC2S( 1 ); //坐骑
            Shop_a3Proxy.getInstance().sendinfo( 0 , 0 , 0 );

            ContMgr.init();
           
            OffLineExpProxy.getInstance();

            LevelProxy.getInstance().sendGet_lvl_cnt_info(1);

            A3_SummonProxy.getInstance().sendLoadSummons();
            A3_ActiveProxy.getInstance().SendGetHuntInfo();


            a3_HeroTitleServer.getInstance().SendMsg( a3_HeroTitleServer.GET_TITLE ); //头衔数据

          

            if (HttpAppMgr.instance == null && Globle.DebugMode == 2)
                HttpAppMgr.init();

            E_mailModel.getInstance().init();

            if (HttpAppMgr.instance != null)
                HttpAppMgr.instance.initGift();

            //  GameSdkMgr.recordLogin();
        }

     
        public bool pkState
        {
            get { return _pkState; }
            set { if (_pkState == value) return; _pkState = value; /*if (herohead.instance != null)herohead.instance.refreshPkState(value);*/ }
        }

        public bool checkPK()
        {
            return _pkState && !inDefendArea;
        }

        internal void refreshByChangeMap(Variant msgData)
        {
            modHp(msgData["hp"]);
            if (msgData.ContainsKey("mp"))
                modMp(msgData["mp"]);
            iid = msgData["iid"];

            if (msgData.ContainsKey("face"))
            {
                mapBeginroatate = msgData["face"];
            }

            mapBeginX = msgData["x"] / GameConstant.PIXEL_TRANS_UNITYPOS;
            mapBeginY = msgData["y"] / GameConstant.PIXEL_TRANS_UNITYPOS;

            if (msgData.ContainsKey("mpid"))
                mapid = msgData["mpid"];
            //   debug.Log("refreshByChangeMap refreshByChangeMaprefreshByChangeMaprefreshByChangeMaprefreshByChangeMap" + mapBeginX);
        }


        //求特定转生、等级、经验a，距离所需b全部经验
        public uint GetNeedExp(uint a_zhuan, uint a_lvl, uint a_exp, uint b_zhuan, uint b_lvl, uint b_exp)
        {
            var clxml = XMLMgr.instance.GetSXML("carrlvl");
            uint xexp = 0;
            for (uint i = a_zhuan; i < b_zhuan; i++)
            {
                var e = clxml.GetNode("carr", "carr==" + profession).GetNode("zhuanshen", "zhuan==" + i).GetNodeList("carr");
                foreach (var v in e)
                {
                    int vl = v.getInt("lvl");
                    if (vl >= a_lvl)
                        xexp += v.getUint("exp");
                }
            }
            if (b_zhuan > a_zhuan)
            {
                var n = clxml.GetNode("carr", "carr==" + profession).GetNode("zhuanshen", "zhuan==" + b_zhuan).GetNodeList("carr");
                foreach (var v in n)
                {
                    int vl = v.getInt("lvl");
                    if (vl < a_lvl)
                        xexp += v.getUint("exp");
                }
            }
            xexp -= a_exp;
            xexp += b_exp;
            return xexp > 0 ? xexp : 0;
        }

        public bool IsCaptain => TeamProxy.getInstance().MyTeamData?.meIsCaptain ?? false;
        public bool IsCaptainOrAlone => TeamProxy.getInstance().MyTeamData?.meIsCaptain ?? true;
        public bool IsInATeam => TeamProxy.getInstance().MyTeamData != null;
        public bool CheckLevel(int zhuan, int level)
        {
            if (up_lvl < zhuan) return false;
            if (up_lvl > zhuan) return true;
            if (up_lvl == zhuan && lvl >= level) return true;
            else return false;
        }

        private uint maxExp = 0u;

        public uint GetCurrMaxExp() {

            var sxml = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + this.profession);

            var s_exp = sxml.GetNode("zhuanshen", "zhuan==" + this.up_lvl);

            var nextLvlXml = s_exp.GetNode("carr", "lvl==" + this.lvl + 1); //下一个等级配置

            if ( nextLvlXml != null)
            {
                maxExp = 0u;

                return maxExp;
            }

            var expPoolLvl = s_exp.getInt("exp_pool_level");

            var xml = s_exp.GetNode("carr", "lvl==" + this.lvl);

            var cost_exp = xml.getUint("exp");

            if ( nextLvlXml == null && maxExp == 0u )
            {
                var nexts_exp = sxml.GetNode("zhuanshen", "zhuan==" + (this.up_lvl + 1u)); // 取下一转职等级配置

                if (nexts_exp != null)
                {
                    for (int i = 0; i < expPoolLvl - 1 ; i++)
                    {
                        var currXml = nexts_exp.GetNode("carr", "lvl==" + (i + 1));

                        maxExp += currXml.getUint("exp");

                    }

                }
            }

            return maxExp ;

        }
    }
}
