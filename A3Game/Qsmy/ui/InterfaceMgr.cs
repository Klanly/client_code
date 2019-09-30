using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    public class InterfaceMgr //: GameEventDispatcher
    {
        static public readonly float UI_KEEP_TIME = 180f; //3分钟内不开关显示，在后台，清理掉

        private class Asnyc_UIData
        {
            public string name = null;
            public ArrayList data = null;
            public bool isFunctionBar = false;

            public Asnyc_UIData(string name, ArrayList data, bool isFunctionBar)
            {
                this.name = name;
                this.data = data;
                this.isFunctionBar = isFunctionBar;
            }
        }

        public enum UI_LL
        {
            UILL_NONE = 0,
            UILL_LOADING = 1,
            UILL_LOADED = 100,
            UILL_FAILED = -1,
        }

        private class winData
        {
            public string winName = null;
            public float winOverTime = 0f;
            public UI_LL winUILL = UI_LL.UILL_NONE;
            public GameObject winItem = null;
            public Baselayer winComponent = null;

            public bool winReserve = false;
            public bool winShowWaitLoad = false;
        }

        public static bool LINK_RUN_CS = true;
        public bool worldmap;
        public static Action<string, object> handleOpenByLua;

        public delegate object[] objDelegate(string commandid, params object[] args);
        public delegate object[] objDelegate1(string commandid, string path, params object[] args);
        public static objDelegate doCommandByLua_discard;
        public static objDelegate1 doCommandByLua;

        public delegate GameObject objDeleget5(string abname, string assetname);
        public delegate Sprite objDelegate6(string abname, string assetname);
        public static objDeleget5 doGetAssert;
        public static objDelegate6 doGetAssert_sp;
        private Action _afterClose;
        public Action afterClose {
            set { _afterClose = value; }
            get {
                Action t = null;
                if (_afterClose != null)
                {
                     t = (Action)_afterClose.Clone();
                    _afterClose = null;
                }
                return t;
            }
        }

        public InterfaceMgr()
        {
            init();
        }

        public static void openByLua(string name, object pram = null)
        {
            if (handleOpenByLua == null) return;
            handleOpenByLua(name, pram);
        }

        public void doAction(string name, params object[] args)
        {
            debug.Log("aaaaaaaaa==" + name);
        }

        //a3
        static public string A3_FASHIONSHOW = "a3_fashionshow";
        static public string A3_ONERECHARGE = "a3_onerecharge";
        static public string A3_BOSSRANKING = "a3_bossranking";
        static public string A3_LEGION_BUILD = "a3_legion_build";
        static public string A3_TIMEGIFS = "a3_timegifs";
        static public string A3_PKSHOW = "a3_pkshow";
        static public string A3_ACTIVEONLINE="a3_activeOnline";
        static public string A3_MWLRCHANGE = "a3_mwlrchange";
        static public string A3_SEVENDAY = "a3_sevenday";
        static public string A3_HALLOWS = "a3_hallows";
        static public string A3_MAPCHANGELINE = "a3_mapChangeLine";
        static public string A3_RUNESTONE = "a3_runestone";
        static public string A3_RUNESTONETIP = "a3_runestonetip";
        static public string RETURN_BT = "returnbt";
        static public string A3_BLESSING = "a3_blessing";
        static public string A3_CHAPTERHINT = "a3_chapter_hint";
        static public string A3_WANTLVUP = "a3_wantlvup";
        static public string A3_DOING = "a3_doing";
        //static public string A3_SUMMON = "a3_summon";
        static public string A3_AUCTION = "a3_auction";
        static public string A3_AUCTION_GET = "a3_auction_get";
        static public string A3_AUCTION_SELL = "a3_auction_sell";
        static public string A3_AUCTION_BUY = "a3_auction_buy";
        static public string A3_AUCTION_RACK = "a3_auction_rack";
        static public string A3_TARGETINFO = "a3_targetinfo";
        static public string A3_INSIDEUI_FB = "a3_insideui_fb";
        static public string A3_FB_FINISH = "a3_fb_finish";
        static public string A3_SPEEDTEAM = "a3_SpeedTeam";

        static public string A3_SPORTS = "a3_sports";

        static public string A3_BUFF = "a3_buff";
        static public string A3_CITYWARTIP = "a3_cityWarTip";

        static public string A3_ACTIVE = "a3_active";
        static public string A3_MINIMAP = "a3_minimap";
        static public string A3_LITEMINIMAP = "a3_liteMinimap";//简版小地图
        static public string A3_LITEMINIBASEMAP = "a3_liteMiniBaseMap";
        static public string A3_LITEMINIBASEMAP1 = "a3_liteMiniBaseMap1";
        static public string A3_LITEMINIBASEMAP2 = "a3_liteMiniBaseMap2";
        static public string A3_HEROHEAD = "a3_herohead";
        static public string A3_EXPBAR = "a3_expbar";
        //static public string A3_FUNCTIONBAR = "a3_functionbar";
        static public string A3_BAG = "a3_bag";
        static public string A3_ACTIVE_GODLIGHT = "a3_active_godlight";
        static public string A3_WAREHOUSE = "a3_warehouse";
        //static public string A3_EQUIPSELL = "a3_equipsell";
        static public string A3_EQUIPTIP = "a3_equiptip";
        static public string A3_EQUIPUP = "a3_equipup";
        static public string A3_ITEMTIP = "a3_itemtip";
        static public string A3_DYETIP = "a3_dyetip";
        static public string A3TIPS_SUMMON = "a3tips_summon";
        static public string A3_LVUP = "a3_lvup";
        static public string A3_LOWBLOOD = "a3_lowblood";
        static public string A3_TRRIGERDIALOG = "a3_trrigerDialog";
        static public string A3_MAPNAME = "a3_mapname";
        static public string A3_FUNCOPEN = "a3_funcopen";
        static public string A3_SILLOPEN = "a3_skillopen";
        static public string A3_RUNEOPEN = "a3_runeopen";
        static public string A3_ROLE = "a3_role";
        static public string A3_EQUIP = "a3_equip";
       // static public string A3_EQUIP_NEW = "a3_equip_new";
        static public string A3_PET_DESC = "a3_pet_desc";
        static public string A3_EXCHANGE = "a3_exchange";
        static public string SHOP_A3 = "shop_a3";
        static public string SKILL_A3 = "skill_a3";
        //static public string A3_RANK = "a3_rank";
        static public string A3_CREATECHA = "a3_createcha";
        static public string A3_SELECTCHA = "a3_selectcha";
        static public string A3_SIGN = "a3_sign";
        static public string A3_GIFTCARD = "a3_giftCard";
        static public string A3_AUTOPLAY = "a3_autoplay";
        static public string A3_AUTOPLAY2 = "a3_autoplay2";
        public static string A3_AUTOPLAY_EQP = "a3_autoplay_eqp";
        static public string A3_AUTOPLAY_SKILL = "a3_autoplay_skill";
        public static string A3_AUTOPLAY_PICK = "a3_autoplay_pick";
        static public string A3_RESETLVL = "a3_resetlvl";// 转生面板
        static public string A3_GETGOLDWAY = "a3_getGoldWay";// 金币获取方式面板
        static public string A3_RESETLVLSUCCESS = "a3_resetLvLSuccess";//转生成功界面
        static public string A3_STORE = "a3_store";
        public static string A3_RELIVE = "a3_relive";
        public static string A3_MAIL = "a3_mail";
        static public string A3_LOTTERY = "a3_lottery";//抽奖面板
        //static public string A3_ENTERLOTTERY = "a3_enterlottery";//进入抽奖面板按钮floatUI TODO:需要整合到其它一起
        static public string TARGET_HEAD = "tragethead";
        static public string A3_ITEMLACK = "a3_itemLack";
        static public string A3_VIP = "a3_vip";
        static public string A3_RECHARGE = "a3_Recharge";
        static public string A3_NEWACTIVE = "a3_newActive";
        public static string A3_YILING = "a3_yiling";
        static public string A3_WASHREDNAME = "a3_washredname";
        static public string WORLD_MAP = "worldmap";
        static public string WORLD_MAP_SUB = "worldmapsubwin";
        static public string A3_TASK = "a3_task";
        static public string A3_SYSTEM_SETTING = "a3_systemSetting";
        static public string A3_CURRENTTEAMINFO = "a3_currentTeamInfo";
        static public string A3_HUDUN = "a3_hudun";
        static public string NEWBIE_LINE = "teachline";
        static public string A3_YGYIWU = "a3_ygyiwu";
        static public string CD = "cd";
        static public string A3_JDZC_CD = "a3_jdzc_cd";
        static public string A3_JDZC_ZHANJI = "a3_jdzc_zhanji";
        static public string LOGIN = "login";
        static public string SERVE_CHOOSE = "choose_server";
        static public string A3_CHATROOM = "a3_chatroom";//聊天
        static public string A3_FINDBESTO = "A3_FindBesto";

        static public string A3_PKMODEL = "a3_pkmodel";
        static public string A3_ACHIEVEMENT = "a3_achievement";//成就
        static public string A3_RANK = "a3_rank";
        static public string A3_HONOR = "a3_honor";
        static public string A3_SHEJIAO = "a3_shejiao";//社交
        static public string A3_BEREQUESTFRIEND = "a3_beRequestFriend";//
        static public string NPC_TALK = "npctalk";
        static public string A3_AWARDCENTER = "a3_awardCenter";//奖励中心
        static public string A3_COUNTERPART = "a3_counterpart";//副本
        static public string A3_NPC_SHOP = "a3_npc_shop";
        static public string A3_FIRESTRECHARGEAWARD = "a3_firstRechargeAward";//首冲奖励
        static public string A3_TEAMAPPLYPANEL = "a3_teamApplyPanel";//队伍申请
        static public string A3_TEAMINVITEDPANEL = "a3_teamInvitedPanel";//队伍邀请
        //static public string A3_TEAMPANEL = "a3_teamPanel";//组队
        static public string A3_TEAMMEMBERLIST = "a3_teamMemberList";//队员列表
        public static string A3_LEGION_DART = "a3_legion_dart";//押镖
        public static string A3_SLAY_DRAGON = "A3_SlayDragon";//屠龙

        static public string NPC_TASK_TALK = "npctasktalk";
        static public string NEWBIE = "newbie";
        static public string A3_INTERACTOTHERUI = "a3_interactOtherUI";//与其他玩家交互UI，如加好友，查看玩家信息等
        static public string A3_EVERYDAYLOGIN = "a3_everydayLogin";//每日登陆奖励
        static public string A3_DOUBLEEXP = "a3_doubleExp";

        static public string A3_ACTIVEDEGREE = "a3_activeDegree";//活跃度

        //a3_eqpInherit
        static public string A3_EQPINHERIT = "a3_eqpInherit";//换装传承

        static public string A3_SUMMON_NEW = "a3_summon_new";//新召唤兽界面
        static public string A3_WIBG_SKIN = "a3_wing_skin";
        static public string A3_PET_SKIN = "a3_pet_skin";
        static public string A3_NEW_PET = "a3_new_pet";
        static public string A3_PET_RENEW = "a3_pet_renew";
        static public string CONFIRM_TEXT = "confirmtext";
        static public string A3_SMITHY = "A3_Smithy";
        static public string A3_MINITIP = "a3_miniTip"; // 装备道具tip
        static public string TRANSMIT_PANEL = "TransmitPanel";
        //loading
        static public string BEGIN_LOADING = "beginloading";
        static public string MAP_LOADING = "maploading";
        static public string DISCONECT = "disconect";
        static public string LOADING_CLOUD = "loading_cloud";
        static public string FLYTXT = "flytxt";
        static public string FIGHTINGTXT = "fightingup";
        //static public string PLAYER_INFO = "playerinfo";
        static public string WAIT_LOADING = "wait_loading";
        static public string SDK_LOADING = "sdkloading";

        //static public string COMBO_TEXT = "combo_txt";
        //static public string MONSTER_DICT = "monster_direction";
        static public string PK_NOTIFY = "pk_notify";
        //static public string SKILL_TEST = "skillbartest";
        static public string CD_TIME = "cdtime";
        static public string A3_QHMASTER = "a3_QHmaster";
        static public string A3_RANKING = "a3_ranking";
        static public string A3_SUMMONINFO = "a3_summoninfo";
        static public string A3_WINGINFO = "a3_Winginfo";
        static public string A3_CITYOFWAR = "A3_cityOfWar";

        static public string A3_HOMORPOW_UP = "a3_honorPow_up";

        static public string A3_HONOR_POW = "a3_honor_pow";

        static public string A3_CHANGE_NAME = "a3_changeName";

        //story
        //  static public string STORY_RETURN = "story_returnbt";

        //3d ui
        //static public string FB_3D = "fb_3d";
        //static public string HERO_3D = "hero3d";

        //主界面
        static public string A1_GAMEJOY = "a1_gamejoy";
        //static public string JOYSTICK = "joystick";
        //static public string SKILL_BAR = "skillbar";

        static public string A3_BESTRONGER = "A3_BeStronger";
        //static public string CREATE_CHAR = "creatchar";
        //static public string SELECT_CHAR = "selchar";
        static public string DEBUG = "debug";
        //static public string ACTIVE = "active";
        //static public string FB_WIN = "fb_win";
        //static public string FB_LOSE = "fb_lose";
        //static public string UPLEVEL = "uplv";
        //static public string GETTING = "getthing";
        //static public string BAG = "bag";
        //static public string SHOP = "shop";
        //static public string EQUIP = "equip";
        //static public string HERO_lua_HEAD = "herohead";
        //static public string MINIMAP = "minimap";
        //static public string DRESS = "dress";
        //static public string DRESSUPGRADE = "dressupgrade";
        //static public string WEAPONUPGRADE = "weapon_upgrade";
        //static public string WEAPON = "weapon";
        //static public string WEAPONLUCKYDRAW = "weapon_luckydraw";
        //static public string TASK = "task";
        //static public string HERO = "hero";
        //static public string STRONG = "strong";
        //static public string STORY_DIALOG = "storydialog";
        //static public string PLOT_LINKUI = "plot_linkui";
        //static public string FB = "fb_map_main";
        //static public string FB_MAIN = "fb_main";
        //static public string FB_INFO = "fb_info";
        //static public string FB_ENERGY = "fb_energy_new";
        //static public string ACHIEVE = "achieve";
        //static public string SKILL = "skill";
        //static public string MOUNT = "mount";
        //static public string PAIHANG = "paihang";
        //static public string FAMILY = "family";
        //static public string FAMILY_CHECKS = "family_checks";
        //static public string FAMILY_CREATE = "family_create";
        //static public string FAMILY_DONATION = "family_contribution";
        //static public string FAMILY_ICONTAGS = "family_icontags";
        //static public string FAMILY_JOBAPPOINT = "family_jobappoint";
        //static public string FAMILY_SKILL = "family_skill";
        //static public string FAMILY_SETTING = "family_setting";
        //static public string FAMILY_NOTICE = "family_notice";
        //static public string FAMILY_TREASURE = "family_treasure";
        //static public string ATTR = "heroattr";
        //static public string VIP = "vip";
        //static public string VIP_UP = "vip_ani";
        //static public string SIGN = "sign";
        //static public string RECHARGE = "recharge";
        //static public string FRIEND = "friend";
        //static public string FRIEND_SEARCH = "friend_search";
        //static public string WIPE_OUT = "fb_wipeout";
        //static public string E_MAIL = "my_mail";
        //static public string STICAL = "stical";
        //static public string BUY_SIGN = "buy_sign";
        //static public string ARENA = "arena";
        //static public string ARENA_BUY = "arena_buy";
        //static public string ARENA_REPORT = "arena_report";
        //static public string ARENA_REWARDS = "arena_rewards";
        //static public string ARENA_SHOP = "arena_shop";
        //static public string ARENA_ACCOUNT = "arena_account";
        //static public string FIRST_RECHANGE = "first_rechange";
        //static public string TAELS = "taels";
        //static public string SETTINGS = "settings";
        static public string RELIVE = "relive";
        //static public string FACTION = "faction";
        //static public string MAILPAPER = "mail_paper";
        static public string BROADCASTING = "broadcasting";
        //static public string RANKING = "ranking";
        //static public string LOTTERY = "lottery";
        //static public string LOTTERY_DRAW = "lottery_draw";
        //static public string HERO_NOTICE = "hero_notice";
        static public string OFFLINEEXP = "off_line_exp";
        //static public string GETGIFT = "getgift";
        //static public string NEEDGET = "needget";
        static public string A3_LHLIANJIE = "a3_LHlianjie";
        static public string A3_ELITEMON = "A3_EliteMonster";
        static public string A3_QUICKOP = "A3_QuickOp";
        static public string A3_PKMAPUI = "a3_pkMapUI";
        static public string A3_ATTCHANGE = "a3_attChange";
        static public string A3_GETJEWELRYWAY = "a3_getJewelryWay";
        static public string A3_BAOTUUI = "a3_baotuUI";

        static public string PILIANG_FENJIE = "piliang_fenjie";

        static public string PILIANG_CHUSHOU = "piliang_chushou";

        static public string RIDE_A3 = "ride_a3";
        static public string RIDE_CD = "ride_cd";
        static public string A3_ROLL_ITEM = "a3_RollItem";

        //static public string EQUIP_QH = "equip_QH";



        //static public string A3_ENTRUSTOPT = "A3_EntrustOpt";
        //static public string A3_CLANTASKOPT = "A3_ClanTaskOpt";
        static public string A3_TASKOPT = "A3_TaskOpt";
        //场景界面
        //static public string FLOAT_ARENA = "floatui_arena_hp";
        //活动界面
        //static public string CAMPAIGN = "campaign";
        //static public string MJJD = "mjjd";//墨家禁地
        //static public string MJJD_WIN = "mhjd_win";
        //static public string MJJD_FLOAT = "float_mjjd";
        //static public string GETNEW = "getnew";

        static public string DIALOG = "dialog";
        static public string A3_FB_TEAM = "a3_fb_team";
        //static public string ab_url = "ab_layer.assetbundle";
        public RectTransform cemaraRectTransform;


        //public List<String> assertBandle_list = new List<string>();

        //public void addAssertBandleUi(string name)
        //{
        //    if (!assertBandle_list.Contains(name))
        //    {
        //        assertBandle_list.Add(name);
        //    }
        //}

        void init()
        {
            //a3
            creatWinData(A3_FASHIONSHOW);
            creatWinData(A3_DOUBLEEXP);
            creatWinData(A3_ONERECHARGE);
            creatWinData(A3_BOSSRANKING);
            creatWinData(A3_LEGION_BUILD);
            creatWinData(A3_TIMEGIFS);
            creatWinData(A3_CITYOFWAR);
            creatWinData(A3_PKSHOW);
            creatWinData(A3_JDZC_ZHANJI);
            creatWinData(A3_HONOR);
            creatWinData(A3_RANK);
            creatWinData(A3_SPORTS);
            creatWinData(A3_ACTIVEONLINE);
            creatWinData(A3_NEWACTIVE);
            creatWinData(A3_MWLRCHANGE);
            creatWinData(A3_SEVENDAY);
            creatWinData(A3_SUMMON_NEW,false,true);
            creatWinData(A3_HALLOWS);
            creatWinData(A3_MAPCHANGELINE);
            creatWinData(A3_HOMORPOW_UP);
            creatWinData(A3_HONOR_POW);
            creatWinData(NEWBIE);
            //creatWinData(EQUIP_QH);
            creatWinData(PILIANG_CHUSHOU);
            creatWinData(PILIANG_FENJIE,true);
            creatWinData(A3_RUNESTONETIP);
            creatWinData(A3_RUNESTONE);
            creatWinData(A3_BAOTUUI);
            creatWinData(A3_EQPINHERIT);
            creatWinData(A3_GETJEWELRYWAY);
            creatWinData(A3_WINGINFO);
            creatWinData(A3_SUMMONINFO);
            creatWinData(A3_PKMAPUI);
            creatWinData(A3_ITEMLACK);
            creatWinData(A3_RANKING,false,true);
            creatWinData(A3_LHLIANJIE);
            creatWinData(A3_QHMASTER);
            creatWinData(A3_FINDBESTO);
            creatWinData(RETURN_BT);
            creatWinData(A3_PET_SKIN);
            creatWinData(A3_NEW_PET,false,true);
            creatWinData(A3_PET_RENEW);
            creatWinData(A3_WIBG_SKIN,false,true);
            creatWinData(A3_YGYIWU,false,true);
            creatWinData(A3_RECHARGE);
            creatWinData(A3_HUDUN);
            creatWinData(NEWBIE_LINE);
            creatWinData(NPC_TASK_TALK);
            creatWinData(A3_DOING);
            creatWinData(A3_AUCTION,false,true);
            creatWinData(A3_AUCTION_GET);
            creatWinData(A3_AUCTION_SELL);
            creatWinData(A3_AUCTION_RACK);
            creatWinData(A3_AUCTION_BUY);
            creatWinData(A3_CHAPTERHINT);
            creatWinData(A3_BLESSING);
            creatWinData(A3_WANTLVUP);
            creatWinData(NPC_TALK);
            creatWinData(A3_TARGETINFO);
            creatWinData(A3TIPS_SUMMON);
            creatWinData(A3_INSIDEUI_FB);
            creatWinData(A3_FB_FINISH);
            creatWinData(A3_SPEEDTEAM);
            creatWinData(A3_ACTIVEDEGREE,false,true);
            creatWinData(A3_DYETIP);
            creatWinData(A3_ACTIVE,false, true);
            //creatWinData(A3_SUMMON);
            creatWinData(A3_MINIMAP);
            creatWinData(A3_HEROHEAD);
            creatWinData(A3_BUFF);//buff
            creatWinData(A3_EXPBAR);
            //creatWinData(A3_FUNCTIONBAR);
            creatWinData(A3_ACTIVE_GODLIGHT);
            creatWinData(A3_WAREHOUSE);         
            //creatWinData(A3_EQUIPSELL);
            creatWinData(A3_EQUIPTIP);
            creatWinData(A3_TRRIGERDIALOG);
            creatWinData(A3_ITEMTIP);
            creatWinData(A3_ROLE,false,true);
            creatWinData(A3_PET_DESC);          
            creatWinData(SHOP_A3,false,true);
            creatWinData(SKILL_A3,false,true);
            //creatWinData(A3_RANK);
            creatWinData(A3_CREATECHA);
            creatWinData(A3_SELECTCHA);
            creatWinData(A3_SIGN,true ,true);
            creatWinData(A3_EXCHANGE);
            creatWinData(A3_AUTOPLAY);
            creatWinData(A3_AUTOPLAY_SKILL);
            creatWinData(A3_STORE);
            creatWinData(A3_RELIVE);
            creatWinData(WORLD_MAP);
            creatWinData(WORLD_MAP_SUB);
            creatWinData(A3_AUTOPLAY2);
            creatWinData(A3_AUTOPLAY_EQP);
            creatWinData(A3_AUTOPLAY_PICK);
            creatWinData(A3_VIP);
            creatWinData(A3_YILING);
            creatWinData(A3_WASHREDNAME);
            creatWinData(A3_TASK,false,true);
            //creatWinData(SKILL_TEST);
            creatWinData(A3_MAIL,false,true);
            creatWinData(LOGIN);
            creatWinData(SERVE_CHOOSE);
            //creatWinData(MAILPAPER);
            //creatWinData(MINIMAP);
            //creatWinData(HERO_lua_HEAD);
            //creatWinData(SELECT_CHAR);
            //creatWinData(CREATE_CHAR);
            //creatWinData(HERO_3D);
            //creatWinData(HERO_NOTICE);
            creatWinData(DISCONECT);
            creatWinData(BEGIN_LOADING);
            //creatWinData(FUNCTION_BAR);          
            creatWinData(FIGHTINGTXT);
            //creatWinData(EXP_BAR);
            creatWinData(A3_GIFTCARD);
            //creatWinData(PLAYER_INFO);
            creatWinData(A1_GAMEJOY, true);
            //creatWinData(JOYSTICK);
            //creatWinData(SKILL_BAR);
            //creatWinData(EQUIP);
            //creatWinData(WEAPON);
            //creatWinData(DRESS);
            //creatWinData(DRESSUPGRADE);
            //creatWinData(WEAPONUPGRADE);
            //creatWinData(WEAPONLUCKYDRAW);
            //creatWinData(TASK);
            //creatWinData(HERO);
            //creatWinData(STRONG);
            creatWinData(A3_SYSTEM_SETTING,false,true);
            //   creatWinData(STORY_RETURN);
            //creatWinData(STORY_DIALOG);
            //creatWinData(PLOT_LINKUI);
            //creatWinData(FB_MAIN);
            //creatWinData(FB_INFO);
            //creatWinData(FB_WIN);
            //creatWinData(FB_LOSE);
            //creatWinData(ACHIEVE);
            //creatWinData(SKILL);
            //creatWinData(MOUNT);
            //creatWinData(PAIHANG);
            //creatWinData(FAMILY);
            //creatWinData(FAMILY_CHECKS);
            //creatWinData(FAMILY_CREATE);
            //creatWinData(FAMILY_DONATION);
            //creatWinData(FAMILY_ICONTAGS);
            //creatWinData(FAMILY_JOBAPPOINT);
            //creatWinData(FAMILY_SKILL);
            //creatWinData(FAMILY_SETTING);
            //creatWinData(FAMILY_NOTICE);
            //creatWinData(FAMILY_TREASURE);
            //creatWinData(ATTR);
            //creatWinData(VIP);
            //creatWinData(VIP_UP);
            //creatWinData(GETTING);
            //creatWinData(SIGN);
            //creatWinData(RECHARGE);
            //creatWinData(WIPE_OUT);
            //creatWinData(FRIEND);
            //creatWinData(FRIEND_SEARCH);
            //creatWinData(UPLEVEL);
            //creatWinData(E_MAIL);
            creatWinData(LOADING_CLOUD);
            //creatWinData(ARENA);
            //creatWinData(ARENA_BUY);
            //creatWinData(ARENA_REPORT);
            //creatWinData(ARENA_REWARDS);
            //creatWinData(ARENA_SHOP);
            //creatWinData(ARENA_ACCOUNT);
            //creatWinData(COMBO_TEXT);
            //creatWinData(BUY_SIGN);
            //creatWinData(FIRST_RECHANGE);
            //creatWinData(TAELS);
            //creatWinData(SETTINGS);
            creatWinData(RELIVE);
            //creatWinData(STICAL);
            //creatWinData(FACTION);
            creatWinData(BROADCASTING);
            //creatWinData(FB_ENERGY);
            //creatWinData(LOTTERY);
            //creatWinData(LOTTERY_DRAW);
            //creatWinData(RANKING);
            //creatWinData(FB);
            creatWinData(PK_NOTIFY);
            //creatWinData(FLOAT_ARENA);
            //creatWinData(FB_3D);
            creatWinData(OFFLINEEXP);
            //creatWinData(MONSTER_DICT);
            creatWinData(CD_TIME);
            creatWinData(A3_CITYWARTIP);
            creatWinData(A3_JDZC_CD);
            //creatWinData(ACTIVE);
            //creatWinData(GETGIFT);
            //creatWinData(NEEDGET);
            creatWinData(SDK_LOADING);
            //creatWinData(CAMPAIGN);
            //creatWinData(MJJD);
            //creatWinData(MJJD_WIN);
            //creatWinData(MJJD_FLOAT);
            //creatWinData(GETNEW);
            creatWinData(DIALOG);
            creatWinData(A3_RESETLVL);          //转生面板
            creatWinData(A3_GETGOLDWAY);        //金币获取方式面板
            creatWinData(A3_RESETLVLSUCCESS);   //转生成功面板
            creatWinData(A3_LOTTERY,false,true);//抽奖UI
            //creatWinData(A3_ENTERLOTTERY);      //进入抽奖按钮
            creatWinData(A3_PKMODEL);
            creatWinData(CONFIRM_TEXT);
            creatWinData(A3_SHEJIAO,false,true);
            creatWinData(A3_BEREQUESTFRIEND);
            creatWinData(A3_ACHIEVEMENT,false,true);
            creatWinData(A3_INTERACTOTHERUI);
            creatWinData(A3_AWARDCENTER,false,true);//奖励中心
            creatWinData(A3_COUNTERPART,false ,true);//副本
            creatWinData(A3_NPC_SHOP);//npcShop
            creatWinData(A3_FIRESTRECHARGEAWARD);//首冲奖励
            creatWinData(A3_TEAMAPPLYPANEL);
            creatWinData(A3_TEAMINVITEDPANEL);
            //creatWinData(A3_TEAMPANEL);
            creatWinData(A3_TEAMMEMBERLIST);
            creatWinData(A3_LEGION_DART);
            creatWinData(A3_SLAY_DRAGON);
            creatWinData(A3_CURRENTTEAMINFO);
            creatWinData(A3_EVERYDAYLOGIN);
            creatWinData(A3_LITEMINIMAP,false,true);
            creatWinData(A3_LITEMINIBASEMAP);
            creatWinData(A3_LITEMINIBASEMAP1);
            creatWinData(A3_LITEMINIBASEMAP2);
            creatWinData(A3_SMITHY); //铁匠铺
            creatWinData(A3_ELITEMON,false,true); //精英怪物
            //creatWinData(A3_ENTRUSTOPT); //委托任务的相关操作
            creatWinData(A3_MINITIP); //装备道具tip
            creatWinData(TRANSMIT_PANEL);//传送面板
            //creatWinData(A3_CLANTASKOPT);//军团任务的相关操作
            creatWinData(A3_TASKOPT);
            creatWinData(A3_FB_TEAM);

            creatWinData(A3_BAG, false, true);
            creatWinData(A3_EQUIP, false, true);
           // creatWinData(A3_EQUIP_NEW, false, true);
            creatWinData(CD, true);
            creatWinData(TARGET_HEAD, true);
            creatWinData(A3_ATTCHANGE, true);
            creatWinData(A3_BESTRONGER, true);
            creatWinData(WAIT_LOADING, true);
            creatWinData(A3_CHATROOM, true);
            creatWinData(A3_LOWBLOOD, true);
            creatWinData(A3_MAPNAME, true);
            creatWinData(A3_FUNCOPEN, true);
            creatWinData(A3_EQUIPUP, true);
            creatWinData(A3_LVUP, true);
            creatWinData(A3_SILLOPEN, true);
            creatWinData(A3_RUNEOPEN, true);
            creatWinData(MAP_LOADING, true);
            creatWinData(DEBUG, true);
            creatWinData(FLYTXT, true);
            creatWinData(A3_QUICKOP, true); //快捷操作
            creatWinData( RIDE_A3 , false , true ); //快捷操作
            creatWinData( RIDE_CD , true );
            creatWinData(A3_CHANGE_NAME);
            creatWinData(A3_ROLL_ITEM); //roll  item

            m_strUI_winNames = new string[winPool.Count];
            winPool.Keys.CopyTo(m_strUI_winNames, 0);



            m_obj_GameScreen = GameObject.Find("game_screen");
            if (m_obj_GameScreen)
            {
                m_ri_GameScreen = m_obj_GameScreen.GetComponent<RawImage>();
                m_obj_GameScreen.SetActive(false);
            }




            Transform bgLaytram = GameObject.Find("bgLayer").transform.FindChild("bgLayer1");
            bgLayer = GameObject.Find("bgLayer").transform;
            bgLayerObj = bgLaytram.gameObject;
            GameObject winLayerObj = GameObject.Find("winLayer");
            winLayer = winLayerObj.transform;
            GameObject floatUiObj = GameObject.Find("floatUI");
            floatUI = floatUiObj.transform;

            gamejobCanvas = GameObject.Find("canvas_gamejoy").transform;
            stroyLayer = GameObject.Find("storyLayer").transform;
            fightTextLayer = GameObject.Find("fightText").transform;
            loadingLayer = GameObject.Find("loadingLayer").transform;
            goPlayerNameLayer = GameObject.Find("playername");
            newbieLayer = GameObject.Find("newbieLayer").transform;
            dropItemLayer = GameObject.Find("dropItemLayer").transform;

            canvas_low = GameObject.Find("canvas_low").transform;
            canvas_high = GameObject.Find("canvas_high").transform;

            setUntouchable(goPlayerNameLayer);
            bgLaytram.localScale = new Vector3(3, 3, 1);


            goMain = GameObject.Find("canvas_main");
            mainLoadingBg = goMain.transform.FindChild("loadingbg").gameObject;
            mainLoadingBg.SetActive(false);

            //  if (Globle.DebugMode == 2)
            ui_async_open(DEBUG);

            //if (CrossApp.singleton != null)
            //{
            //    process = new processStruct(onUpdate, "InterfaceMgr");
            //    (CrossApp.singleton.getPlugin("processManager") as processManager).addProcess(process);
            //}

            //    debug.Log("::::::" + GameObject.Find("ui_main_camera"));
            ui_Camera_tf = GameObject.Find("ui_main_camera").transform;
            ui_Camera_cam = GameObject.Find("ui_main_camera").GetComponent<Camera>();

            ResetUI_Main_Camera();
        }

        public void ResetUI_Main_Camera()
        {
            Transform trancOverLay = GameObject.Find("Canvas_overlay").transform;
            Vector3 vec = trancOverLay.position;
            vec.z = -100;
            ui_Camera_tf.position = vec;
            ui_Camera_cam.orthographicSize = trancOverLay.localScale.x * 320f;
        }

        public void linkGameScreen(RenderTexture rt)
        {
            if (m_ri_GameScreen != null)
                m_ri_GameScreen.texture = rt;
        }

        public void showGameScreen()
        {
            if (m_obj_GameScreen != null)
                m_obj_GameScreen.SetActive(true);
        }

        public void hideGameScreen()
        {
            if (m_obj_GameScreen != null)
                m_obj_GameScreen.SetActive(false);
        }

        public void showLoadingBg(bool b)
        {

            mainLoadingBg.SetActive(b);
        }


        private GameObject mainLoadingBg;

        private Vector3 vecHide = new Vector3(999999, 0, 0);
        private void hideLayer(Transform lay)
        {
            lay.localScale = Vector3.zero;
        }

        private void showLayer(Transform lay)
        {
            lay.localScale = Vector3.one;
        }

        private void hideLayerCanvas(Transform lay)
        {
            if (lay.GetComponent<Canvas>() != null)
                lay.GetComponent<Canvas>().enabled = false;
        }
        private void showLayerCanvas(Transform lay)
        {
            if (lay.GetComponent<Canvas>() != null)
                lay.GetComponent<Canvas>().enabled = true;
        }

        private float m_fUI_DisposeTime = 0f;
        private int m_nUI_DisposeIndex = 0;
        public void FrameMove(float fdt)
        {            
            m_fUI_DisposeTime += fdt;
            if (m_fUI_DisposeTime > 1f)
            {
                m_fUI_DisposeTime = 0f;

                if (m_nUI_DisposeIndex >= winPool.Count) m_nUI_DisposeIndex = 0;
                string cur_name = m_strUI_winNames[m_nUI_DisposeIndex];
                ++m_nUI_DisposeIndex;
                winData windata = winPool[cur_name];
                if (windata.winUILL == UI_LL.UILL_LOADED && !windata.winReserve)
                {
                    if (windata.winItem == null)
                    {//预制件已被移除,重置下
                        windata.winItem = null;
                        windata.winComponent = null;
                        windata.winUILL = UI_LL.UILL_NONE;
                    }
                    else if (windata.winItem.activeInHierarchy)
                    {
                        windata.winOverTime = Time.realtimeSinceStartup + UI_KEEP_TIME;
                    }
                    else if (windata.winOverTime < Time.realtimeSinceStartup)
                    {
                        DisposeUI(cur_name);
                        //Debug.LogError("Time.realtimeSinceStartup = " + Time.realtimeSinceStartup);
                        //Debug.Log("DisposeUI::" + cur_name + "   windata.winOverTime = " + windata.winOverTime);
                    }
                }                
                //Debug.Log(m_nUI_DisposeIndex + "   m_nUI_DisposeIndex   : " + cur_name);
            }
            //FlyFightTextMgr.FrameMove(fdt);
        }

        ////processStruct process;
        //int tick = 0;
        //void onUpdate(float s)
        //{
        //    tick++;
        //    if (tick < 30)
        //        return;

        //    tick = 0;
        //    if (ui_Camera_cam != null && !ui_Camera_cam.gameObject.active)
        //        ui_Camera_cam.gameObject.SetActive(true);
        //}

        private GameObject m_obj_GameScreen;
        private RawImage m_ri_GameScreen;

        public static Transform ui_Camera_tf;
        public static Camera ui_Camera_cam;
        public static GameObject goMain;

        private GameObject goPlayerNameLayer;
        private Transform bgLayer;
        public Transform floatUI;
        public Transform winLayer;
        private GameObject bgLayerObj;
        private Transform gamejobCanvas;
        private Transform loadingLayer;
        private Transform stroyLayer;
        private Transform fightTextLayer;
        private Transform newbieLayer;
        private Transform dropItemLayer;

        private Transform canvas_low;
        private Transform canvas_high;

        public string[] m_strUI_winNames = null;
        private Dictionary<string, winData> winPool = new Dictionary<string, winData>();

        private List<string> dOpeningWin = new List<string>();

        //private int showBGWinsNum = 0;

        winData creatWinData(string name, bool keep = false, bool waitload = false)
        {
            winData windata = new winData();
            windata.winName = name;
            windata.winReserve = keep;
            windata.winShowWaitLoad = waitload;
            winPool.Add(name, windata);
            return windata;
        }


        public static void setUntouchable(GameObject go)
        {
            CanvasGroup cg = go.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = go.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
        }

        private void UIprefab_Loaded(UnityEngine.Object uobj, System.Object name_sobj)
        {
            Asnyc_UIData ac_ui_data = name_sobj as Asnyc_UIData;
            winData windata = winPool[ac_ui_data.name];
            Baselayer uiComponent = null;

            GameObject root = null;
            GameObject commonUIPrefab = uobj as GameObject;
            Type tui_class = System.Type.GetType("MuGame." + ac_ui_data.name);
            if (commonUIPrefab == null || tui_class == null)
            {
                windata.winUILL = UI_LL.UILL_FAILED;
                ac_ui_data = null;
                return;
            }

            windata.winUILL = UI_LL.UILL_LOADED;
            windata.winOverTime = Time.realtimeSinceStartup + UI_KEEP_TIME;
            GAMEAPI.KeepOneAsset("uilayer_" + ac_ui_data.name + ".assetbundle");
            root = GameObject.Instantiate<GameObject>(commonUIPrefab);

            if (LINK_RUN_CS)
            {
                root.AddComponent(tui_class);

                uiComponent = (Baselayer)root.GetComponent(windata.winName);
                uiComponent.uiName = ac_ui_data.name;
                uiComponent.uiData = ac_ui_data.data;
                windata.winComponent = uiComponent;

                if (uiComponent.type == Baselayer.LAYER_TYPE_FLOATUI)
                    root.transform.SetParent(floatUI, false);
                else if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
                {
                    root.transform.SetParent(winLayer, false);

                    //if (ac_ui_data.name != WAIT_LOADING)
                    //{
                    //    //MediaClient.getInstance().PlaySoundUrl("media/ui/open", false, null);
                    //}
                    uiComponent.addBg();

                    if (ac_ui_data.isFunctionBar)
                    {
                        uiComponent.isFunctionBar = true;
                    }
                    else
                    {
                        uiComponent.isFunctionBar = false;
                    }
                }
                else if (uiComponent.type == Baselayer.LAYER_TYPE_LOADING)
                    root.transform.SetParent(loadingLayer, false);
                else if (uiComponent.type == Baselayer.LAYER_TYPE_STORY)
                    root.transform.SetParent(stroyLayer, false);
                else if (uiComponent.type == Baselayer.LAYER_TYPE_FIGHT_TEXT)
                    root.transform.SetParent(fightTextLayer, false);
                else if (uiComponent.type == Baselayer.LAYER_TYPE_GAMEJOY)
                    root.transform.SetParent(gamejobCanvas, false);

                windata.winItem = root;
                root.transform.localPosition = new Vector3(0, 0, 0);
                root.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                //for test ui loading
                windata.winItem = root;
                root.transform.SetParent(winLayer, false);
            }

            if (uiComponent != null)
            {
                if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
                {
                    if (ac_ui_data.name != WAIT_LOADING && ac_ui_data.name != A3_EVERYDAYLOGIN)
                    {
                        MediaClient.instance.PlaySoundUrl("audio_common_open_interface", false, null);
                    }

                    dOpeningWin.Add(ac_ui_data.name);
                    if (ac_ui_data.isFunctionBar)
                    {
                        uiComponent.isFunctionBar = true;
                    }
                    else
                    {
                        uiComponent.isFunctionBar = false;
                    }
                }

                //if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW_3D)
                //{
                //    MediaClient.instance.PlaySoundUrl("media/ui/open", false, null);
                //}

                if (uiComponent is IBgLayerUI)
                {
                    IBgLayerUI winComponent = (IBgLayerUI)uiComponent;
                }
            }

            if (windata.winShowWaitLoad)
            {
                close(InterfaceMgr.WAIT_LOADING);
            }

            ac_ui_data = null;
        }

        public void ui_async_open(string name, ArrayList data = null, bool isFunctionBar = false)
        {
            //if (name == A3_RECHARGE)
            //{//猎手平台暂时屏蔽充值
            //    flytxt.instance.fly(ContMgr.getCont("comm_un_recharge"));
            //    return;
            //}


            ////暂时关闭异步处理
            //ui_now_open(name, data, isFunctionBar);
            //return;
            

            winData windata = winPool[name];
            if (windata == null) return;

            if (lgSelfPlayer.instance != null && lgSelfPlayer.instance.getAni() == "run")
            {//自动寻路时打开其他ui停止寻路
                if (a1_gamejoy.inst_joystick != null)
                    a1_gamejoy.inst_joystick.OnDragOut();
                else
                    lgSelfPlayer.instance.onJoystickEnd();
            }

            if (windata.winUILL == UI_LL.UILL_NONE)
            {
                if (windata.winShowWaitLoad)
                {
                    ui_now_open(InterfaceMgr.WAIT_LOADING);
                }

                //debug.Log("async :: prefab ==== ............ " + name);
                windata.winUILL = UI_LL.UILL_LOADING;
                GAMEAPI.ABLayer_LoadGameObject("uilayer_" + name, UIprefab_Loaded, new Asnyc_UIData(name, data, isFunctionBar));
            }
            else if (windata.winUILL == UI_LL.UILL_LOADED)
            {
                Baselayer uiComponent = null;
                uiComponent = (Baselayer)windata.winItem.GetComponent(name);
                uiComponent.uiData = data;
                uiComponent.__open();

                if (uiComponent != null)
                {
                    if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
                    {
                        if (name != WAIT_LOADING && name != A3_EVERYDAYLOGIN)
                        {
                            MediaClient.instance.PlaySoundUrl("audio_common_open_interface", false, null);
                        }

                        dOpeningWin.Add(name);
                        if (isFunctionBar)
                        {
                            uiComponent.isFunctionBar = true;
                        }
                        else
                        {
                            uiComponent.isFunctionBar = false;
                        }


                    }

                    //if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW_3D)
                    //{
                    //    MediaClient.instance.PlaySoundUrl("media/ui/open", false, null);
                    //}
                }
            }
            if (dOpeningWin.Contains(A3_BOSSRANKING) && name != A3_BOSSRANKING)
            {
               // a3_bossranking.instance?.showui(false);

                InterfaceMgr.getInstance().close(InterfaceMgr.A3_BOSSRANKING);
            }
        }

        public void ui_now_open(string name, ArrayList data = null, bool isFunctionBar = false)
        {
            //if (name == A3_RECHARGE)
            //{//猎手平台暂时屏蔽充值
            //    flytxt.instance.fly(ContMgr.getCont("comm_un_recharge"));
            //    return;
            //}

            winData windata = winPool[name];
            if (windata == null) return;

            if (windata.winUILL == UI_LL.UILL_LOADING || windata.winUILL == UI_LL.UILL_FAILED) return;

            if (lgSelfPlayer.instance != null && lgSelfPlayer.instance.getAni() == "run")
            {//自动寻路时打开其他ui停止寻路
                if (a1_gamejoy.inst_joystick != null)
                    a1_gamejoy.inst_joystick.OnDragOut();
                else
                    lgSelfPlayer.instance.onJoystickEnd();
            }

            Baselayer uiComponent = null;
            if (windata.winUILL == UI_LL.UILL_NONE)
            {
                debug.Log("prefab ==== ............ " + name);

                GameObject commonUIPrefab;
                GameObject root;

                commonUIPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_" + name);
                if (commonUIPrefab == null) return;

                windata.winUILL = UI_LL.UILL_LOADED;
                windata.winOverTime = Time.realtimeSinceStartup + UI_KEEP_TIME;
                GAMEAPI.KeepOneAsset("uilayer_" + name + ".assetbundle");
                root = GameObject.Instantiate(commonUIPrefab) as GameObject;


                //string tempName = windata.winTempName;
                //if (tempName == null)
                string tempName = name;

                if (LINK_RUN_CS)
                {
                    Type tui_class = System.Type.GetType("MuGame." + tempName);
                    if (tui_class == null) return;
                    root.AddComponent(tui_class);

                    uiComponent = (Baselayer)root.GetComponent(windata.winName);
                    uiComponent.uiName = name;
                    uiComponent.uiData = data;
                    windata.winComponent = uiComponent;

                    if (uiComponent.type == Baselayer.LAYER_TYPE_FLOATUI)
                        root.transform.SetParent(floatUI, false);
                    else if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
                    {
                        root.transform.SetParent(winLayer, false);

                        //if (name != WAIT_LOADING)
                        //{
                        //    //MediaClient.getInstance().PlaySoundUrl("media/ui/open", false, null);
                        //}
                        uiComponent.addBg();



                        if (isFunctionBar)
                        {
                            uiComponent.isFunctionBar = true;
                        }
                        else
                        {
                            uiComponent.isFunctionBar = false;
                        }
                    }
                    else if (uiComponent.type == Baselayer.LAYER_TYPE_LOADING)
                        root.transform.SetParent(loadingLayer, false);
                    else if (uiComponent.type == Baselayer.LAYER_TYPE_STORY)
                        root.transform.SetParent(stroyLayer, false);
                    else if (uiComponent.type == Baselayer.LAYER_TYPE_FIGHT_TEXT)
                        root.transform.SetParent(fightTextLayer, false);
                    else if (uiComponent.type == Baselayer.LAYER_TYPE_GAMEJOY)
                        root.transform.SetParent(gamejobCanvas, false);

                    windata.winItem = root;
                    root.transform.localPosition = new Vector3(0, 0, 0);
                    root.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    windata.winItem = root;
                    root.transform.SetParent(winLayer, false);
                }
            }
            else
            {
                uiComponent = (Baselayer)windata.winItem.GetComponent(name);
                uiComponent.uiData = data;
                uiComponent.__open();
            }

            if (uiComponent != null)
            {
                if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
                {
                    if (name != WAIT_LOADING && name != A3_EVERYDAYLOGIN)
                    {
                        MediaClient.instance.PlaySoundUrl("audio_common_open_interface", false, null);
                    }

                    dOpeningWin.Add(name);
                    if (isFunctionBar)
                    {
                        uiComponent.isFunctionBar = true;
                    }
                    else
                    {
                        uiComponent.isFunctionBar = false;
                    }
                }

                //if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW_3D)
                //{
                //    MediaClient.instance.PlaySoundUrl("media/ui/open", false, null);
                //}

                if (uiComponent is IBgLayerUI)
                {
                    IBgLayerUI winComponent = (IBgLayerUI)uiComponent;
                    //if (winComponent.showBG == true)
                    //{
                    //    showBGWinsNum++;
                    //    bgLayerObj.SetActive(true); 
                    //}
                }
            }
        }

        public bool checkWinOpened(string winid)
        {
            return winPool.ContainsKey(winid) && winPool[winid].winItem != null && winPool[winid].winItem.activeInHierarchy;
        }

        public void close(string name)
        {
            if (name == null || name.Equals("")) return;
            winData windata = winPool[name];
            if (windata.winItem == null)
            {
                if (dOpeningWin.Count == 0)
                {
                    afterClose?.Invoke();
                }
                return;
            }

            windata.winOverTime = Time.realtimeSinceStartup + UI_KEEP_TIME;

            //windata.winItem.SetActive(false);
            //windata.winComponent.onClosed();

            Baselayer uiComponent = (Baselayer)windata.winItem.GetComponent(windata.winName);

            uiComponent.__close();

            if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
            {
                //if (name != WAIT_LOADING)
                //{
                //    //MediaClient.instance.PlaySoundUrl("media/ui/close", false, null);
                //}

                //if (uiComponent.isFunctionBar)
                //{
                //    functionbar.instance.showCam();
                //}
            }

            //if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW_3D)
            //{
            //    MediaClient.instance.PlaySoundUrl("media/ui/close", false, null);
            //}

            if (uiComponent.type == Baselayer.LAYER_TYPE_WINDOW)
            {
                dOpeningWin.Remove(name);
                if (dOpeningWin.Count == 0)
                {
                    afterClose?.Invoke();
                }
            }
            
        }

        public void closeAllWin(String except = "")
        {
            String str;
            List<string> closeWin = new List<string>();
            for (int i = 0; i < dOpeningWin.Count; i++)
            {
                str = dOpeningWin[i];
                if (except != str && str != LOGIN)
                {
                    closeWin.Add(str);
                }
            }
            for (int i = 0; i < closeWin.Count; i++)
            {
                str = closeWin[i];
                close(str);
            }
        }

        public void closeAllWin(List<string> except)
        {
            string str;
            List<string> closeWin = new List<string>();
            for (int i = 0; i < dOpeningWin.Count; i++)
            {
                str = dOpeningWin[i];
                if (except == null || !except.Contains(str) && str != LOGIN)
                {
                    closeWin.Add(str);
                }

            }
            for (int i = 0; i < closeWin.Count; i++)
            {
                str = closeWin[i];
                close(str);
            }
        }

        public void closeFui_NB() // 关闭一些在指引时可能打开的FloatUi
        {
            close(A3_CHATROOM);
            close(A3_PKMAPUI);
        }

        //解决第一次打开ui卡的问题。loading的时候提前打开一遍
        //public void openUiFirstTime()
        //{
        //    List<string> m_first_ui = new List<string>();
        //    m_first_ui.Add(A3_BAG);
        //    m_first_ui.Add(A3_SHEJIAO);
        //    m_first_ui.Add(A3_EQUIP);

        //    List<string> m_first_ui_lua = new List<string>();

        //    if (maploading.instance != null)
        //        maploading.instance.loadingUi(m_first_ui, m_first_ui_lua);
        //}
        public void closeUiFirstTime()
        {
            close(A3_BAG);
            close(A3_SHEJIAO);
            close(A3_EQUIP);
        }

        public void initFirstUi()
        {
            List<string> m_first_ui = new List<string>();

            m_first_ui.Add(A3_LITEMINIMAP);
            m_first_ui.Add(A1_GAMEJOY);
            //m_first_ui.Add(JOYSTICK);
            //m_first_ui.Add(SKILL_BAR);
            m_first_ui.Add(A3_BESTRONGER);
            m_first_ui.Add(FLYTXT);
            m_first_ui.Add(A3_ATTCHANGE);
            m_first_ui.Add(PK_NOTIFY);
            m_first_ui.Add(A3_HEROHEAD);
            m_first_ui.Add(BROADCASTING);
            m_first_ui.Add(A3_EXPBAR);
            m_first_ui.Add(A3_EQUIPUP);
            m_first_ui.Add(A3_LOWBLOOD);
            m_first_ui.Add(A3_TRRIGERDIALOG);
            m_first_ui.Add(A3_LVUP);
            m_first_ui.Add(A3_MAPNAME);
            m_first_ui.Add(A3_FUNCOPEN);
            m_first_ui.Add(A3_SILLOPEN);
            m_first_ui.Add(A3_RUNEOPEN);
            m_first_ui.Add(TARGET_HEAD);
            m_first_ui.Add(A3_QUICKOP);
            m_first_ui.Add(A3_TASKOPT);
            m_first_ui.Add(A3_CHATROOM);
            m_first_ui.Add(A3_BUFF);
            //m_first_ui.Add(A3_ENTERLOTTERY);
            if (PlayerModel.getInstance().showBaotu_ui)
            {
                m_first_ui.Add(A3_BAOTUUI);
                PlayerModel.getInstance().showBaotu_ui = false;
            }

            List<string> m_first_ui_lua = new List<string>();
            //m_first_ui_lua.Add("a3_litemap");
            //m_first_ui_lua.Add("a3_litemap_btns");
            m_first_ui_lua.Add("flytxt");
            m_first_ui_lua.Add("fightingup");
            //m_first_ui_lua.Add("herohead2");
            //m_first_ui_lua.Add("herohead");
            //m_first_ui_lua.Add("expbar");
            m_first_ui_lua.Add("a1_low_fightgame");
            //m_first_ui_lua.Add("a1_mid_fightgame");
            m_first_ui_lua.Add("a1_high_fightgame");

            if (maploading.instance != null)
                maploading.instance.loadingUi(m_first_ui, m_first_ui_lua, true);
        }

        public void closeAllWins(String[] except = null)
        {
            String str;
            List<string> closeWin = new List<string>();
            List<String> ls = null;
            if (except != null) ls = new List<String>(except);
            for (int i = 0; i < dOpeningWin.Count; i++)
            {
                str = dOpeningWin[i];
                if (ls != null && !ls.Contains(str) && str != LOGIN)
                {
                    closeWin.Add(str);
                }

            }
            for (int i = 0; i < closeWin.Count; i++)
            {
                str = closeWin[i];
                close(str);
            }
        }
        public bool AnyWinOpen()
        {            
            for (int i = 0; i < dOpeningWin.Count; i++)
                if (checkWinOpened(dOpeningWin[i]))
                    return true;
            return false;
        }

        static public int STATE_NORMAL = 0;
        static public int STATE_STORY = 1;
        static public int STATE_FUNCTIONBAR = 2;
        static public int STATE_FB_WIN = 3;
        static public int STATE_FB_BATTLE = 4;
        static public int STATE_3DUI = 5;
        static public int STATE_HIDE_ALL = 6;
        static public int STATE_SHOW_ONLYWIN = 7;
        static public int STATE_DIS_CONECT = 8;
        static public int STATE_ZHUANSHENG_ANI = 9;
        private int curState = STATE_NORMAL;
        public void changeState(int state)
        {
            if (curState == state)
                return;
            curState = state;
            if (state == STATE_NORMAL)
            {
                showLayer(stroyLayer);
                showLayer(floatUI);
                showLayer(winLayer);
                showLayer(bgLayer);
                showLayer(goPlayerNameLayer.transform);
                showLayer(fightTextLayer);
                showLayer(newbieLayer);
                showLayer(loadingLayer);
                showLayer(dropItemLayer);

                showLayerCanvas(canvas_high);
                showLayerCanvas(canvas_low);
                showLayerCanvas(gamejobCanvas);

                ui_Camera_cam.gameObject.SetActive(true);

                //if (herohead.instance) herohead.instance.refreshByUIState();
                if (broadcasting.instance)
                    broadcasting.instance.on_off(true);
                if (npctalk.instance != null)
                    npctalk.instance.MinOrMax();

                //   close(STORY_RETURN);
            }
            else if (state == STATE_STORY)
            {
                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);
                hideLayer(floatUI);
                closeAllWins(new string[] { InterfaceMgr.DIALOG, InterfaceMgr.CD, InterfaceMgr.A3_CHAPTERHINT });
                hideLayer(bgLayer);
                hideLayer(goPlayerNameLayer.transform);
              
                //   open(STORY_RETURN);
                ui_Camera_cam.gameObject.SetActive(true);

                showLayer(stroyLayer);
            }
            else if (state == STATE_FUNCTIONBAR)
            {
                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);
                hideLayer(floatUI);
                hideLayer(bgLayer);
                hideLayer(dropItemLayer);
                hideLayer(goPlayerNameLayer.transform);
                hideLayer(fightTextLayer);
                ui_Camera_cam.gameObject.SetActive(true);
            }
            else if (state == STATE_FB_WIN)
            {
                hideLayer(goPlayerNameLayer.transform);
                hideLayer(floatUI);
                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);
                ui_Camera_cam.gameObject.SetActive(true);
            }
            else if (state == STATE_DIS_CONECT)
            {
                hideLayer(stroyLayer);
                showLayer(winLayer);
                ui_Camera_cam.gameObject.SetActive(true);
            }
            else if (state == STATE_FB_BATTLE)
            { 
                showLayerCanvas(canvas_high);
                showLayerCanvas(canvas_low);
                showLayerCanvas(gamejobCanvas);
                showLayer(floatUI);
                showLayer(winLayer);
                showLayer(bgLayer);
                showLayer(goPlayerNameLayer.transform);

                //if (herohead.instance)
                //    herohead.instance.refreshByUIState();
                //if (minimap.instance)
                //    minimap.instance.refreshByUIState();
                if (broadcasting.instance)
                    broadcasting.instance.on_off(false);
                ui_Camera_cam.gameObject.SetActive(true);
                // close(STORY_RETURN);
            }
            else if (state == STATE_HIDE_ALL)
            {

                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);
                hideLayer(floatUI);
                hideLayer(winLayer);
                hideLayer(bgLayer);
                hideLayer(goPlayerNameLayer.transform);
                hideLayer(newbieLayer);
            }
            else if (state == STATE_SHOW_ONLYWIN)
            {
                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);

                hideLayer(loadingLayer);
                hideLayer(floatUI);
                hideLayer(bgLayer);
                hideLayer(goPlayerNameLayer.transform);
            }
            else if (state == STATE_ZHUANSHENG_ANI)
            {
                hideLayerCanvas(canvas_high);
                hideLayerCanvas(canvas_low);
                hideLayerCanvas(gamejobCanvas);

                hideLayer(floatUI);
                hideLayer(winLayer);
                hideLayer(bgLayer);
                hideLayer(goPlayerNameLayer.transform);
                hideLayer(newbieLayer);
                hideLayer(stroyLayer);
            }
        }


        public void itemToWin(bool Toclose, string win_name)//道具索引
        {
            // Toclose 是确认是否是点击关闭按钮的，所以点击关闭按钮内将Toclose设为true，在onshowed中Toclose设为false
            if (a3_itemLack.intans && a3_itemLack.intans.closewindow != null)
            {
                if (Toclose)
                {
                    if (a3_itemLack.intans.closewindow != win_name)
                        InterfaceMgr.getInstance().ui_async_open(a3_itemLack.intans.closewindow,a3_itemLack .intans .back_uidata);
                    a3_itemLack.intans.closewindow = null;
                    a3_itemLack.intans.back_uidata = null;
                }
                else
                {
                    if (a3_itemLack.intans.closewindow != win_name)
                    {
                        a3_itemLack.intans.closewindow = null;
                        a3_itemLack.intans.back_uidata = null;
                    }

                }
            }
        }

        public void DisposeUI(string name)
        {
            winData windata = winPool[name];
            if (windata.winUILL == UI_LL.UILL_LOADED)
            {
                if (windata.winComponent != null)
                {
                    windata.winComponent.dispose();
                }
                else
                {
                    GameObject.Destroy(windata.winItem);
                }

                windata.winItem = null;
                windata.winComponent = null;

                GAMEAPI.KillOneAsset("uilayer_" + name + ".assetbundle");
                windata.winUILL = UI_LL.UILL_NONE;
            }
        }

        static private InterfaceMgr _instance;
        static public InterfaceMgr getInstance()
        {
            if (_instance == null)
            {
                _instance = new InterfaceMgr();
            }
            return _instance;
        }

        public void setGameJoy(bool b)
        {
            if (a1_gamejoy.inst_joystick != null)
                a1_gamejoy.inst_joystick.onoffAni(b);

        }


        public void setGameSkill(bool b) {
            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.onoff_skillbarAni(b);
        }

        //void Awake()
        //{
        //    _instance = this;
        //}

        //static public EventTriggerListener Get(GameObject go)
        //{

        //    EventTriggerListener listener = go.GetComponent<EventTriggerListener>();

        //    if (listener == null) listener = go.AddComponent<EventTriggerListener>();

        //    return listener;

        //}
    }
}
