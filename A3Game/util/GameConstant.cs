using System;
using Cross;
using GameFramework;
namespace MuGame
{
    public class GameConstant
    {
        public static PlatformType PLAFORM_TYPE = PlatformType.NULL;
        public const uint GEZI = 32;                    //每一个格子的像素宽高
        public const uint GEZI_HDT = 5;                //灰度图格子的像素宽高
        public const float  GEZI_TRANS_UNITYPOS= 1.666f;
        public const float PIXEL_TRANS_UNITYPOS = 53.333f;
 
		//public const int DEF_ATTACK_RANGE = 90;// 
		public const int DEF_ATTACK_RANGE = 1;// 
		public const float OPEN_NPC_DISTANCE = 1f;//

        public const uint LINK_ACT_GRID_RANGE = 5;	//link点触发移动的格子距离,包含该值

        public const uint MOVE_FREQUENCY_MIN_TM = 300;	//最小发送移动消息间隔时间

        public const string LINK_EFFECT_ID = "linkeff";	//link特效id

        public const string SELECT_EFF_ID = "selecteff";	//选中特效id

        public const float CAMERA_Z_MOVE = 0.01f;

		//public const float CAMERA_ROTATION_X = 45f;
		//public const float CAMERA_ROTATION_Y = 45f;
		//public const float CAMERA_ROTATION_Z = 0f;

		//public const float CAMERA_FIELD_VIEW = 35f;

		////对应游戏逻辑坐标
		//public const float CAMERA_POSTION_OFFSET_FROM_CHAR_X = -9.5f;
		//public const float CAMERA_POSTION_OFFSET_FROM_CHAR_Y = -8.75f;
		//public const float CAMERA_POSTION_OFFSET_FROM_CHAR_Z = 13.5f;
 
		//
		static public float CHAR_OFFSET_Y = 0.1f;
		static public float SHADOW_OFFSET_Y = 0f;// 相对角色位置偏移
		//注: 游戏里会刷新该值
        //static public bool REFRESH_CAMERA_ROT_AND_FOV = false;
		static public float CAMERA_ROTATION_X = 45f;
		static public float CAMERA_ROTATION_Y = 45f;
        static public float CAMERA_ROTATION_Z = 0f;

        static public float CAMERA_FIELD_VIEW = 15f;

        //对应游戏逻辑坐标
        static public  float CAMERA_POSTION_OFFSET_FROM_CHAR_X = -19.3f;
        static public  float CAMERA_POSTION_OFFSET_FROM_CHAR_Y = 28.15f;
        static public  float CAMERA_POSTION_OFFSET_FROM_CHAR_Z = -20.35f;


        public const float ORI_UINT = 45.0f;

		public const int SCREEN_DEF_WIDTH = 1280;
		public const int SCREEN_DEF_HEIGHT = 720;

        public const uint SP_MAIN_CHAR = 1;
        public const uint SP_CHAR = 2;
        public const uint SP_Monster = 3;

        public const uint CONN_DEF = 10;
        public const uint CONN_CONNECTED = 20;
        public const uint CONN_LOGINED = 30;
        public const uint CONN_VER_GOT = 40;
        public const uint CONN_JOIN_WORLD = 50;
        //加载状态
        public const int INFO_LOADED = 0;
        public const int INFO_DEF = 1;
        public const int INFO_LOADING = 2;
        public const int INFO_ERR = -1;
        //角色方向
        public const int ORI_S = 1;
        public const int ORI_SE = 2;
        public const int ORI_SW = 3;
        public const int ORI_W = 4;
        public const int ORI_N = 5;
        public const int ORI_NW = 6;
        public const int ORI_NE = 7;
        public const int ORI_E = 8;

		//
		public static Vec2 EFF_DEF_ORI = new Vec2(0,1);//特效制作以Y轴正方向为基准方向
		//
        //public const string DEF_BONE_HAND_R = "Bip001 R Hand";
        //public const string DEF_BONE_HAND_L = "Bip001 R Hand";
		//默认模型
		public const string DEFAULT_AVATAR = "1070";//
		//public const string DEFAULT_SHADOW = "shadow";//

        //角色动作

		//public const string ANI_IDLE_NORMAL = "a1";//待机
		//public const string ANI_FLY_IDLE = "a4";//飞行待机
		//public const string ANI_MOVE_NORMAL = "a5";//移动
		//public const string ANI_SINGLE_H_ATK = "a18";//单手-
		//public const string ANI_FLY_MOVE = "a7";//飞行移动
		//public const string ANI_DOUBLE_H_ATK = "a6";//双手-
		//public const string ANI_SWIMMING = "a9";//游泳
		//public const string ANI_DIE = "a28";//死亡
		//public const string ANI_GETHIT = "a26";//直立受击动作
		//public const string ANI_NORMAL_RUN = "a6";//跑步
		//public const string ANI_ATK_IDLE = "a2";//战斗待机
		//public const string ANI_DISMOUNT = "a17";//下坐骑-
		//public const string ANI_MOUNT_IDLE = "a3";//坐骑待机
		//public const string ANI_MOUNT_MOVE = "a8";//坐骑移动
		//public const string ANI_MOUNT_SH_ATK = "a22";//坐骑单手
		//public const string ANI_MOUNT_DH_ATK = "a23";//坐骑双手

        public const string ANI_MOVE_NORMAL ="a5";	// 单手武器跑步
        //public const string ANI_MOVE_A = "a3";	//单手武器飞行移动
        //public const string ANI_MOVE_B = "a6";	//单手武器走路
        //public const string ANI_MOVE_C = "a8";	//双手武器跑步
        //public const string ANI_MOVE_D = "a9";	//双手武器走路

        //public const string ANI_ON_HIT = "a7";	//单手武器站立受击

		//public const string ANI_IDLE_NORMAL ="a1";	//单手武器站立待机
        public const string ANI_IDLE_NORMAL = "idle";	//单手武器站立待机
        //public const string ANI_IDLE_A ="a10";	//双手大剑战斗待机
        //public const string ANI_IDLE_B ="a11";	//双手大剑站立待机
        //public const string ANI_IDLE_C ="a12";	//双手镰刀站立待机
        //public const string ANI_IDLE_D ="a13";	//双手镰刀战斗待机
        //public const string ANI_IDLE_E ="a14";	//双手战锤战斗待机
        //public const string ANI_IDLE_F ="a15";	//双手战锤站立待机
        //public const string ANI_IDLE_G ="a2";	//单手武器战斗待机
        //public const string ANI_IDLE_H ="a4";	//单手武器飞行待机

        public const string ANI_SINGLE_H_ATK ="a16";	//普通攻击1
        //public const string ANI_DOUBLE_H_ATK ="a17";	//普通攻击2
        //public const string ANI_NORMAL_ATK ="a18";	//普通攻击2
 

        //public const string ANI_SKILL_A="a19";	//技能1
        //public const string ANI_SKILL_B="a20";	//技能2
        //public const string ANI_SKILL_C="a21";	//技能3
        //public const string ANI_SKILL_D="a22";	//技能4
        //public const string ANI_SKILL_E="a23";	//技能5

        //public const string ANI_DIE ="dead";	//死亡
        //public const string ANI_SWIMMING ="a25";	//游泳
        //public const string ANI_GO_UP ="a26";	//出场

		//
		public const int ANI_LOOP_FLAG_AUTO = 0;
		public const int ANI_LOOP_FLAG_NOT_LOOP = 1;
		public const int ANI_LOOP_FLAG_LOOP = 2;

        //角色状态 不超过256
        public const int ST_MOVE = 10;
        public const int ST_STAND = 20;
        //public const int ST_STAND_FOR_ATK = 22;
        //public const int ST_EFF = 30;
        //public const int ST_SONG = 30;
        public const int ST_ATK = 40;
        //public const int ST_MIS = 40;
        public const int ST_DIE = 50;



        //角色操作分类
        public const int CTP_BASE = 1;
        public const int CTP_MOVE_POS = 2;
        public const int CTP_STAND = 3;
        public const int CTP_STOP = 4;
        public const int CTP_MOVE_ORI = 5;
        public const int CTP_MOVE_MAP = 6;
        public const int CTP_KILL_MON = 7;
        public const int CTP_MISSION = 8;
        public const int CTP_MISSION_LINE = 9;
        public const int CTP_ENTER_LEVEL = 10;
        public const int CTP_ATTACK = 11;
        public const int CTP_TASK = 12;

		//主角操作暂停标志
		public const uint PAUSE_ON_MAP_CHANGE = 1;
		//public const uint PAUSE_ON_ = 2;
		//public const uint PAUSE_ON_ = 4;
        //角色操作数据错误
        public const int E_PATH_NOT_FIND = 1;
        public const int E_NEED_TP = 2;
        public const int E_NEED_PAR = 3;

        ////改变状态图标
        //public const uint STATE_PK = 1;    //开启PK
        //public const uint STATE_LABA = 2;   //打开声音
        //public const uint STATE_PLAYER = 3;  // 屏蔽玩家
        //public const uint STATE_TEAM = 4;    //开启组队pk
        //public const uint STATE_ALL = 5;    //开启全体pk

        ////option_tips显示位置 
        //public const int OPTION_TIPS_LEFT = 0;
        //public const int OPTION_TIPS_UP = 1;
        //public const int OPTION_TIPS_RIGHT = 2;
        //public const int OPTION_TIPS_DOWN = 3;

        //public const int BUBBLE_PET_LABEL = 1;

        ////右下角飞出文本类型 text_show acp
        //public const int TXT_NORMAL = 0;
        //public const int TXT_FOR_MIS_ACP = 1;
        //public const int TXT_FOR_MIS_FIN = 2;
        //public const int TXT_FOR_SYSTEM = 3;

        //聊天界面位置状态  
        public const int POS_NORMAL = 0;
        public const int POS_HIGH = 1;
        public const int POS_HIGHER = 2;

        ////身上装备的格子数
        //public const int EQP_NUM = 10;

        ////阵营
        //public const int CARR_SOLDIER = 1;//战士
        //public const int CARR_MASTER = 2;//法师
        //public const int CARR_ARCHER = 3;//弓箭手

        ////tile状态
        //public const int TILE_DEFAULT = 0;
        //public const int TILE_ABLED = 1;
        //public const int TILE_DISABLED = -1;

        ////vip卡
        //public const uint VIP_WEEK = 35;
        //public const uint VIP_HALFYEAR = 37;
        //public const uint VIP_MONTH = 36;

        //public const uint SHOP_HOTSELL = 0;//热卖
        //public const uint SHOP_QSELL_NORMAL = 1;//Q点常用道具
        //public const uint SHOP_QSELL_PET = 2;//Q点宠物坐骑
        //public const uint SHOP_SELL_BDYB = 3;//绑定元宝
        //public const uint SHOP_HSELL = 4;//荣誉	
        //public const uint SHOP_SELL_POINT = 5;//积分商城	
        //public const uint SHOP_LIMITHOT = 200;//限时抢购区

        //public const uint SHOP_PTSELL = 6;//积分		
        //public const uint SHOP_QSELL_MATERIA = 7;//Q点药品
        //public const uint SHOP_CLAN_PT = 8;//帮派令
        //public const uint SHOP_QSELL_STONE = 10;//Q点石头
        //public const uint SHOP_PVIP_DISCNT = 80;//黄钻Q点购买打折百分数

        public const int MAX_BOARD_CHARS = 65 * 2;
        public const int CARDID_LENGTH_15 = 15;
        public const int CARDID_LENGTH_18 = 18;
        public const int CARDNAME_LENGTH_5 = 5;
        //public const int SERVER_ID = 1;

        //public const int AUTO_FEED_PET_CONDINATION = 30;
        //public const int AUTO_REPAIR_EQUIP_CONDINATION = 10;
        //public const uint AUTO_GAME_UPDATE_INTERVAL = 1000 / 4;

        //public const int MISSION_TRACE_RANGE = 500;
        //public const int MISSION_TRACE_RANGE_SQUARE = MISSION_TRACE_RANGE * MISSION_TRACE_RANGE;

        //public const int BEGINNER_ITEM_LEVLE = 30;//获得物品引导等级

        //public const int GW = 0;
        //public const int SL = 1;
        //public const int WD = 2;
        //public const int XY = 3;
        //public const int BH = 4;

        //public int[] ADD_HP_ITEMS = { 5, 40 };
        //public int[] ADD_MP_ITEMS = { 6, 41 };

        //public const uint MAX_CLAN_NAME_LENGTH = 32;

        //public const int OFL_TYPE_YB_2 = 3;
        //public const int OFL_TYPE_YB_15 = 2;
        //public const int OFL_TYPE_GOLD_1 = 1;
        //public const int OFL_TYPE_NONE = 4;

        //public Variant ARROW_ANIMATION_KEYFRAMES()
        //{
        //    Variant data = new Variant();
        //    Variant v1 = new Variant();
        //    Variant v2 = new Variant();
        //    Variant v3 = new Variant();
        //    v1["time"] = 0;
        //    v1["tx"] = 0;
        //    v1["ty"] = 0;
        //    v2["time"] = 800;
        //    v2["tx"] = 0;
        //    v2["ty"] = 10;
        //    v3["time"] = 1800;
        //    v3["tx"] = 0;
        //    v3["ty"] = 0;

        //    data[0] = v1;
        //    data[1] = v2;
        //    data[2] = v3;
        //    return data;
        //}

        //public const int ARROW_ANIMATION_DURATION = 1800 + 1;

        //public const int MIN_OPEN_BAG_HELP_LEVEL = 10;

        //public const uint GUIDE_NOTICE_DURATION = 6000;

        //public const uint FIRST_ADD_BSTATE_LEVEL = 10;

        //public const uint BUTTON_FLASH_INTERVAL = 250;

        //public const int FACE_ICON_ANIM_INTERVAL = 200;
        //public const int FACE_ICON_WIDTH = 24;
        //public const int FACE_ICON_HEIGHT = 24;

        //public const int VIP_FUNC_TRANS = 1; 	// 缩地成寸		
        //public const int VIP_FUNC_OFLAWD = 2; 	// 离线经验
        ////public  const int VIP_FUNC_3 		= 3; 	// 素云岭之巅
        //public const int VIP_FUNC_AWD = 4; 	// 许愿树

        //public const int VIP_FUNC_BSTATE = 5; 	// 祝福加身
        //public const int VIP_FUNC_LOTLVL = 6; 	// 闭关苦修
        //public const int VIP_FUNC_PETRA = 7; 	// 宠物之心			
        //public const int VIP_FUNC_LVLMISCNT = 8; 	// 豪侠之志

        //public const int VIP_FUNC_LVLCNT = 9; 	// 疯狂副本
        ////public  const int VIP_FUNC_10 		= 10; 	// 江湖商铺
        //public const int VIP_FUNC_EXPMUL = 11; 	// 经验加成		
        //public const int VIP_FUNC_HEXPMUL = 12;	// 杀戮之心

        //public const int VIP_FUNC_MERIACC = 13;	// 经脉之心		
        //public const int VIP_FUNC_FRGRATE = 14; 	// 锻造之心
        //public const int VIP_FUNC_STNLVL = 15;	// 宝石工匠
        //public const int VIP_FUNC_FRGLVL = 16;	// 强化工匠

        //public const int ITEM_QUAL_WHITE = 1;	// 白色
        //public const int ITEM_QUAL_GREEN = 2;	// 绿色
        //public const int ITEM_QUAL_BLUE = 3;	// 蓝色
        //public const int ITEM_QUAL_PURPLE = 4;	// 紫色
        //public const int ITME_QUAL_ORANGE = 5;	// 橙色

        //public const int GUIDE_SISTER_DEFAULT_DURATION = 10000; // 默认新手引导显示时间
        //public const int GUIDE_SISTER_ACUPUP_DURATION = 15000;

        //public const float OPEN_WAR_TIME_COUNT_DOWN = 60000 * 15; //(24*2+12)*1000*60*60;

        //public const int BEGINNER_GUIDE_LOT_LEVEL = 20; // 第一次修行弹出新手指引的等级

        //public const string SERVER_TIMEZONE_TEXT = "GMT8";
        //// 服务器时区，GMT8为北京时间
        //public const int SERVER_TIMEZONE = 8;
        //// -480，Date的北京时间的时区偏移量，分钟
        //public const int SERVER_TIMEZONEOFFSET = -(8 * 60);
        //// 北京时间的时区偏移量，毫秒
        //public const float SERVER_TIMEZONEOFFSET_MS = SERVER_TIMEZONEOFFSET * 60 * 1000;

        //public const int MAX_PACKAGE_SPACE = 16;

        //public const int SHOW_OFFLINE_REWARD_WINDOW = 30; // 打开福利窗口，优先显示离线奖励的等级

        //public const int MAX_ACUPUP_VALID_TIP_LEVEL = 15; // 经脉修炼冷却后，如果小于该等级，就显示相关提示。

        ////		public  const int SECOND_ACUPUP_LEVEL = 35; // 第二条经脉开启等级

        //public const int AUTO_GAME_ATTACK_INTERVAL = 1000;



        //public const int MIN_SHOW_WH_LEVEL_IN_CHAR_PROPERTY = 30;

        //public const string ID_FIRST_ACUP_ACTIVATE = "first_acup_activate";

        public const int EQP_POS_HAT = 1;//帽子
        public const int EQP_POS_NECK = 2;//项链
        public const int EQP_POS_COAT = 3;//上衣
        public const int EQP_POS_PANT = 4;//裤子
        public const int EQP_POS_SHOE = 5;//鞋子
        public const int EQP_POS_WEAPON = 6;//武器
        public const int EQP_POS_HAND = 7;//手套
        public const int EQP_POS_FASHION = 8;//时装
        public const int EQP_POS_RING = 9;//戒指
        public const int EQP_POS_RIDE = 10;//坐骑
        public const int EQP_POS_WING = 11;//翅膀
        public const int EQP_POS_GUARD = 12;//守护
        public const int EQP_POS_PET = 13;//宠物
        public const int EQP_POS_MEDAL = 14;//勋章

        public const int EQP_WEAPON_BOW = 1;//弓
        public const int EQP_WEAPON_CROSSBOW = 2;//弩
        public const int EQP_WEAPON_SHIELD = 3;//盾
        public const int EQP_WEAPON_ARROWS = 4;//箭矢

        public const int EQP_POSUNIQ_BOW = 3;//弓或弩
        public const int EQP_POSUNIQ_CROSIER = 2;//法师单手杖
        public const int EQP_POSUNIQ_SHIELD = 1;//盾
        public const int EQP_POSUNIQ_ARROWS = 4;//箭矢

        //public const int RNT_NORMAL = 0;    // 普通
        //public const int RNT_RASCAL = 1;    // 无赖
        //public const int RNT_EVIL = 2;  //恶人
        //public const int RNT_DEVIL = 3; //魔头
        //public const int RNT_RIGHT = 4; //义士
        //public const int RNT_HERO = 5; //英雄
        //public const int RNT_SHERO = 6; //大侠

        //public const int PKT_PEACE = 0;    // 和平
        //public const int PKT_ALL = 1; //全部
        //public const int PKT_TEAM = 2;    // 组队
        //public const int PKT_CLAN = 3;  //帮派

        //Client UI_Class EVENT type
        public const uint EVT_MouseClick = 0;
        public const uint EVT_DoubleClick = 1;
        public const uint EVT_MouseOver = 2;
        public const uint EVT_MouseOut = 3;
        public const uint EVT_DragDrop = 4;
        public const uint EVT_TextLink = 5;
        public const uint EVT_SelectChange = 6;
        public const uint EVT_PosChange = 7;
        public const uint ACT_Close = 100;
        public const uint ACT_Open = 101;

 

        //声音相关
        //世界类型和副本类型声音常量
        public const string WORLD_BG_1 = "world_bg_1";
        public const string WORLD_BG_2 = "world_bg_2";
        public const string SCRIPT_BG_1 = "script_bg_1";
        public const string SCRIPT_BG_2 = "script_bg_2";

        //鼠标单击声音
        public const string MOUSE_CLICK_1 = "mouse_click_1";
        //UI界面打开声音
        public const string UI_OPEAN_1 = "ui_opean_1";
        //UI界面关闭声音
        public const string UI_CLOSE_1 = "ui_close_1";
        //任务接收
        public const string TASK_RECEIVE_1 = "task_receive_1";
        //任务完成
        public const string TASK_FINISH_1 = "task_finish_1";
        //物品进入背包声音
        public const string TO_BAG_1 = "to_bag_1";
        //背包整理声音
        public const string MAKE_BAG_1 = "make_bag_1";
        //装备修理声音
        public const string REPAIR_EQ_1 = "repair_eq_1";
        //铜币的交接声音
        public const string PAY_GOLD_1 = "pay_gold_1";
        //人物升级声音
        public const string CHARACTER_UP_1 = "character_up_1";
        //竞技场等待音效
        public const string ARENA_WAITING = "arena_waiting";
        //锻造成功
        public const string FORGING_SUCCESS_1 = "forging_success_1";
        //锻造失败
        public const string FORGING_FAIL_1 = "forging_fail_1";
        //锻造中材料放入
        public const string FORGING_ADD_1 = "forging_add_1";
        //好友上线成功
        public const string FEINED_LOGIN_1 = "frined_login_1";
        //密语
        public const string PRIVATE_TALK_1 = "private_talk_1";
        //信件
        public const string MAIL_1 = "mail_1";
        //挂售物品出售
        public const string SALE_1 = "sale_1";
        //交互类声音效果
        public const string CONNECT_MUSIC = "connect_music";
        //技能声音
        public const string SKILL_1 = "skill_1";
        public const string SKILL_2 = "skill_2";
        //穿脱装备音效
        public const string TAKE_OFF = "take_off";
        public const string PUT_ON = "put_on";

        public const int CAST_SKILL_TYPE_NOTAR = 0;//无目标释放
        public const int CAST_SKILL_TYPE_TARGET = 1;//对指定目标释放
        public const int CAST_SKILL_TYPE_POINT = 2;//对指定目标点释放

        public const string SUPERTXT_PROFIX = "<txt ";//supertext 的头
        public const string SUPERTXT_END = "/>";//supertext 的头
        public const string SUPERTXT_BR = "<br/>";//supertext 的头

        public const uint ICMF_NO_NOTIFY = 1;    // 无道具变化内容道具变化消息，需客户端重新获取相应背包、仓库等道具列表，已弃用
        public const uint ICMF_COL_ITEM = 2;    // 采集获得道具
        public const uint ICMF_DECOMP_EQP = 3;    // 分解装备消耗道具
        public const uint ICMF_DELAY_EXPIRE = 4;    // 延长装备时效消耗道具
        public const uint ICMF_LVL_PRIZE = 5;    // 领取副本通关奖励获得道具
        public const uint ICMF_OPEN_PKG = 6;    // 开启礼包获得道具
        public const uint ICMF_FGUESS_RETURN = 7;    // 划拳失败后返回获得的游戏币
        public const uint ICMF_LVL_TM_COST = 8;    // 计时副本消耗
        public const uint ICMF_PICK_ITEM = 9;    // 拾取物品
        public const uint ICMF_RMV_EQP = 100; 	// 脱下装备
        public const uint ICMF_NEW_ITEM = 101; 	// 新物品

        //plattype
        public const int PLAT_LY = 1;//联运
        public const int PLAT_QQ = 2;//QQ
        public const int PLAT_YN = 3;//越南
        public const int PLAT_YY = 4;//多玩YY
        public const int PLAT_TG = 5;//泰国
        public const int PLAT_360 = 6;//360
        public const int PLAT_BM = 7;//北美
        public const int PLAT_P4399 = 10;//4399
        public const int PLAT_NM = 11;//南美
        public const int Plat_TW = 9;//台湾

        //platid
        public const int PLATID_P51 = 12;//51
        public const int PLATID_PPTV = 14;//pptv

        //界面功能开启入口
        public const string MULTILVL_CHALLENGE = "multilvl_challenge";//副本大厅挑战副本
        public const string LUCKYDRAW_RADLUCKY = "luckydraw_radlucky";//幸运转盘
        public const string LUCKYDRAW_RADCHARGE = "luckydraw_radcharge";//充值转盘
        public const string LUCKYDRAW_RADCONSUME = "luckydraw_radconsume";//消费转盘

        public const int TRANSCRIPTTILESUM = 5;
        public const int LAST_TM = 10000;	//持续时间
        public const int LAST_TM_V = 1000;
        public const int INTERVAL = 1000*60*5;
        //---------------UIMBCheck-------------------
        public const int B1 = 1;//默认提示
        public const int B2 = 2;//领取双倍奖励
        public const int B3 = 3;//个人市场中购买物品的提示
        public const int B4 = 4;
        public const int B5 = 5; //离开副本提示
        public const int B6 = 6;	//传送提示
        public const int B7 = 7; //装备升级提示
        public const int B8 = 8;//任务传送提示

        public const int PAGE_UP_X = 1;
		public const int PAGE_DOWN_X = 2;
        public const uint HUMDR_ACTIVITY = 0;
        public const uint OTHER_ACTIVITY = 1;

        public const int TYPE_FRIEND = 1;               //好友
        public const int TYPE_ENEMY = 2;                //仇敌
        public const int TYPE_BLACK = 3;                //黑名单


		public const int MISSION_LINE_MAIN = 1;          // 任务主线
		//---------------DO MISION STATE -------------------
		public const uint DOMISSION_STATE_NONE		= 0;
		public const uint DOMISSION_STATE_DOACCEPT	= 1;//接任务
		public const uint DOMISSION_STATE_ACCEPTING	= 2;
		public const uint DOMISSION_STATE_ACCEPTED	= 3;
		public const uint DOMISSION_STATE_DOING		= 4;
		public const uint DOMISSION_STATE_WAITFIN	= 5;//等待玩家操作
		public const uint DOMISSION_STATE_COMMITING	= 6;//可提交任务
		public const uint DOMISSION_STATE_COMMITED	= 7;
		public const uint DOMISSION_STATE_COMPLETE	= 8;//任务完成

		// ------------- errno ------------------
		public const int ERR_CONF				= -1;
		public const int ERR_MIS_CONF			= -2;
		public const int ERR_NPC_CONF			= -3;
		public const int ERR_MON_CONF			= -4;
		public const int ERR_MAP_CONF			= -5;
		public const int ERR_INIT				= -100; //初始化错误
		public const int ERR_MIS_DATA			= -1000;
		public const int ERR_ENTER_LEVEL		= -1001;
		public const int ERR_IN_LEVEL		= -1002;

		public const int ERR_MAP_PATH		= -1101;
		public const int ERR_PATH		= -1102;

        public const int FOUR = 4;
        public const int FIVE = 5;

 
		//load step
		public const int LOADING_MAP_GRD		= 10;//加载地图碰撞
		public const int LOADING_MAP			= 20;//加载地图

        public const int TAKEPOS_NONE = 0x0;  //无  武器
        public const int TAKEPOS_SINGLE = 0x10000;  //单手  武器
        public const int TAKEPOS_DOUBLE = 0x20000; //双手  武器
        public const int TAKEPOS_WING = 0x40000;  //翅膀
        public const int TAKEPOS_MOUNT = 0x80000;  //坐骑

		//eqp subtp
		public const int TAKEPOS_SUBTP_11 = 11<<16;  //
		public const int TAKEPOS_SUBTP_12 = 12<<16;  //
		public const int TAKEPOS_SUBTP_13 = 13<<16;  //

        public enum PlatformType 
        {
            NULL = 0, //无平台 （电脑下运行）
            i360 = 1, //360 平台 
            baidu = 2  //百度平台
        }
    }
}