using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    public class LogicConstants
    {
        //改变状态图标
        public  const uint STATE_PK = 1;    //开启PK
        public  const uint STATE_LABA = 2;   //打开声音
        public  const uint STATE_PLAYER = 3;  // 屏蔽玩家
        public  const uint STATE_TEAM = 4;    //开启组队pk
        public  const uint STATE_ALL = 5;    //开启全体pk
        //option_tips显示位置 
        public  const int OPTION_TIPS_LEFT = 0;
        public  const int OPTION_TIPS_UP = 1;
        public  const int OPTION_TIPS_RIGHT = 2;
        public  const int OPTION_TIPS_DOWN = 3;

        public  const int BUBBLE_PET_LABEL = 1;
        //右下角飞出文本类型 text_show acp
        public  const int TXT_NORMAL = 0;
        public  const int TXT_FOR_MIS_ACP = 1;
        public  const int TXT_FOR_MIS_FIN = 2;
        public  const int TXT_FOR_SYSTEM = 3;
        //聊天界面位置状态 
        public  const int POS_NORMAL = 0;
        public  const int POS_HIGH = 1;
        public  const int POS_HIGHER = 2;
        //身上装备的格子数
        public  const int EQP_NUM = 10;

        //阵营
        public  const int CARR_SOLDIER = 1;//战士
        public  const int CARR_MASTER = 2;//法师
        public  const int CARR_ARCHER = 3;//弓箭手

        //tile状态
        public  const int TILE_DEFAULT = 0;
        public  const int TILE_ABLED = 1;
        public  const int TILE_DISABLED = -1;

        //vip卡
        public  const uint VIP_WEEK = 35;
        public  const uint VIP_HALFYEAR = 37;
        public  const uint VIP_MONTH = 36;

        public  const uint SHOP_HOTSELL = 0;//热卖
        public  const uint SHOP_QSELL_NORMAL = 1;//Q点常用道具
        public  const uint SHOP_QSELL_PET = 2;//Q点宠物坐骑
        public  const uint SHOP_SELL_BDYB = 3;//绑定元宝
        public  const uint SHOP_HSELL = 4;//荣誉	
        public  const uint SHOP_SELL_POINT = 5;//积分商城	
        public  const uint SHOP_LIMITHOT = 200;//限时抢购区

        public  const uint SHOP_PTSELL = 6;//积分		
        public  const uint SHOP_QSELL_MATERIA = 7;//Q点药品
        public  const uint SHOP_CLAN_PT = 8;//帮派令
        public  const uint SHOP_QSELL_STONE = 10;//Q点石头
        public  const uint SHOP_PVIP_DISCNT = 80;//黄钻Q点购买打折百分数

        public  const int MAX_BOARD_CHARS = 65 * 2;
        public  const int CARDID_LENGTH_15 = 15;
        public  const int CARDID_LENGTH_18 = 18;
        public  const int CARDNAME_LENGTH_5 = 5;
        public  const int SERVER_ID = 1;

        public  const int AUTO_FEED_PET_CONDINATION = 30;
        public  const int AUTO_REPAIR_EQUIP_CONDINATION = 10;
        public  const uint AUTO_GAME_UPDATE_INTERVAL = 1000 / 4;

        public  const int MISSION_TRACE_RANGE = 500;
        public  const int MISSION_TRACE_RANGE_SQUARE = MISSION_TRACE_RANGE * MISSION_TRACE_RANGE;

        public  const int BEGINNER_ITEM_LEVLE = 30;//获得物品引导等级

        public  const int GW = 0;
        public  const int SL = 1;
        public  const int WD = 2;
        public  const int XY = 3;
        public  const int BH = 4;

        public   int[]  ADD_HP_ITEMS = {5,40};
        public   int[]  ADD_MP_ITEMS = {6,41};
        
        public  const uint MAX_CLAN_NAME_LENGTH = 32;

        public  const int OFL_TYPE_YB_2 = 3;
        public  const int OFL_TYPE_YB_15 = 2;
        public  const int OFL_TYPE_GOLD_1 = 1;
        public  const int OFL_TYPE_NONE = 4;
        //public  const Variant ARROW_ANIMATION_KEYFRAMES = [
        //    { 0 time, 0 tx, 0 ty },
        //    { 800 time, 0 tx, 10 ty },
        //    { 1800 time, 0 tx, 0 ty }
        //];
        public void ARROW_ANIMATION_KEYFRAMES()
        {
            Variant v = new Variant();
            Variant a = new Variant();
            Variant b = new Variant();
            Variant c = new Variant();
            a["time"] = 0;
            a["tx"] = 0;
            a["ty"]=0;
            b["time"] = 800;
            b["tx"] = 0;
            b["ty"] = 10;
            c["time"] = 1800;
            c["tx"] = 0;
            c["ty"] = 0;
            v._arr.Add(a);
            v._arr.Add(b);
            v._arr.Add(c);
        }
        public  const int ARROW_ANIMATION_DURATION = 1800 + 1;

        public  const int MIN_OPEN_BAG_HELP_LEVEL = 10;

        public  const uint GUIDE_NOTICE_DURATION = 6000;

        public  const uint FIRST_ADD_BSTATE_LEVEL = 10;

        public  const uint BUTTON_FLASH_INTERVAL = 250;

        public  const int FACE_ICON_ANIM_INTERVAL = 200;
        public  const int FACE_ICON_WIDTH = 24;
        public  const int FACE_ICON_HEIGHT = 24;

        public  const int VIP_FUNC_TRANS = 1; 	// 缩地成寸		
        public  const int VIP_FUNC_OFLAWD = 2; 	// 离线经验
        //public  const int VIP_FUNC_3 		= 3; 	// 素云岭之巅
        public  const int VIP_FUNC_AWD = 4; 	// 许愿树

        public  const int VIP_FUNC_BSTATE = 5; 	// 祝福加身
        public  const int VIP_FUNC_LOTLVL = 6; 	// 闭关苦修
        public  const int VIP_FUNC_PETRA = 7; 	// 宠物之心			
        public  const int VIP_FUNC_LVLMISCNT = 8; 	// 豪侠之志

        public  const int VIP_FUNC_LVLCNT = 9; 	// 疯狂副本
        //public  const int VIP_FUNC_10 		= 10; 	// 江湖商铺
        public  const int VIP_FUNC_EXPMUL = 11; 	// 经验加成		
        public  const int VIP_FUNC_HEXPMUL = 12;	// 杀戮之心

        public  const int VIP_FUNC_MERIACC = 13;	// 经脉之心		
        public  const int VIP_FUNC_FRGRATE = 14; 	// 锻造之心
        public  const int VIP_FUNC_STNLVL = 15;	// 宝石工匠
        public  const int VIP_FUNC_FRGLVL = 16;	// 强化工匠

        public  const int ITEM_QUAL_WHITE = 1;	// 白色
        public  const int ITEM_QUAL_GREEN = 2;	// 绿色
        public  const int ITEM_QUAL_BLUE = 3;	// 蓝色
        public  const int ITEM_QUAL_PURPLE = 4;	// 紫色
        public  const int ITME_QUAL_ORANGE = 5;	// 橙色

        public  const int GUIDE_SISTER_DEFAULT_DURATION = 10000; // 默认新手引导显示时间
        public  const int GUIDE_SISTER_ACUPUP_DURATION = 15000;

        public  const float OPEN_WAR_TIME_COUNT_DOWN = 60000 * 15; //(24*2+12)*1000*60*60;

        public  const int BEGINNER_GUIDE_LOT_LEVEL = 20; // 第一次修行弹出新手指引的等级

        public  const string SERVER_TIMEZONE_TEXT = "GMT8";
        // 服务器时区，GMT8为北京时间
        public  const int SERVER_TIMEZONE = 8;
        // -480，Date的北京时间的时区偏移量，分钟
        public  const int SERVER_TIMEZONEOFFSET = -(8 * 60);
        // 北京时间的时区偏移量，毫秒
        public  const float SERVER_TIMEZONEOFFSET_MS = SERVER_TIMEZONEOFFSET * 60 * 1000;

        public  const int MAX_PACKAGE_SPACE = 16;

        public  const int SHOW_OFFLINE_REWARD_WINDOW = 30; // 打开福利窗口，优先显示离线奖励的等级

        public  const int MAX_ACUPUP_VALID_TIP_LEVEL = 15; // 经脉修炼冷却后，如果小于该等级，就显示相关提示。

        //		public  const int SECOND_ACUPUP_LEVEL = 35; // 第二条经脉开启等级

        public  const int AUTO_GAME_ATTACK_INTERVAL = 1000;

        public  const int OPEN_NPC_DISTANCE = 200;//打开npc面板的距离 像素

        public  const int MIN_SHOW_WH_LEVEL_IN_CHAR_PROPERTY = 30;

        public  const string ID_FIRST_ACUP_ACTIVATE = "first_acup_activate";

        public  const int EQP_POS_HAT = 1;//帽子
        public  const int EQP_POS_NECK = 2;//项链
        public  const int EQP_POS_COAT = 3;//上衣
        public  const int EQP_POS_PANT = 4;//裤子
        public  const int EQP_POS_SHOE = 5;//鞋子
        public  const int EQP_POS_WEAPON = 6;//武器
        public  const int EQP_POS_HAND = 7;//手套
        public  const int EQP_POS_FASHION = 8;//时装
        public  const int EQP_POS_RING = 9;//戒指
        public  const int EQP_POS_RIDE = 10;//坐骑
        public  const int EQP_POS_WING = 11;//翅膀
        public  const int EQP_POS_GUARD = 12;//守护
        public  const int EQP_POS_PET = 13;//宠物
        public  const int EQP_POS_MEDAL = 14;//勋章

        public  const int EQP_WEAPON_BOW = 1;//弓
        public  const int EQP_WEAPON_CROSSBOW = 2;//弩
        public  const int EQP_WEAPON_SHIELD = 3;//盾
        public  const int EQP_WEAPON_ARROWS = 4;//箭矢

        public  const int EQP_POSUNIQ_BOW = 3;//弓或弩
        public  const int EQP_POSUNIQ_CROSIER = 2;//法师单手杖
        public  const int EQP_POSUNIQ_SHIELD = 1;//盾
        public  const int EQP_POSUNIQ_ARROWS = 4;//箭矢

        public  const int RNT_NORMAL = 0;    // 普通
        public  const int RNT_RASCAL = 1;    // 无赖
        public  const int RNT_EVIL = 2;  //恶人
        public  const int RNT_DEVIL = 3; //魔头
        public  const int RNT_RIGHT = 4; //义士
        public  const int RNT_HERO = 5; //英雄
        public  const int RNT_SHERO = 6; //大侠

        public  const int PKT_PEACE = 0;    // 和平
        public  const int PKT_ALL = 1; //全部
        public  const int PKT_TEAM = 2;    // 组队
        public  const int PKT_CLAN = 3;  //帮派

        //Client UI_Class EVENT type
        public  const uint EVT_MouseClick = 0;
        public  const uint EVT_DoubleClick = 1;
        public  const uint EVT_MouseOver = 2;
        public  const uint EVT_MouseOut = 3;
        public  const uint EVT_DragDrop = 4;
        public  const uint EVT_TextLink = 5;
        public  const uint EVT_SelectChange = 6;
        public  const uint EVT_PosChange = 7;
        public  const uint ACT_Close = 100;
        public  const uint ACT_Open = 101;

         public const string ANI_NONE = "";//无
         public const string ANI_NORMAL_IDLE = "a1";//待机
         public const string ANI_FLY_IDLE = "a2";//飞行待机
         public const string ANI_ATK_IDLE = "a16";//战斗待机
         public const string ANI_MOUNT_IDLE = "a20";//坐骑待机
         public const string ANI_NORMAL_MOVE = "a3";//移动
         public const string ANI_NORMAL_RUN = "a15";//跑步
         public const string ANI_FLY_MOVE = "a5";//飞行移动
         public const string ANI_MOUNT_MOVE = "a21";//坐骑移动
         public const string ANI_SWIMMING = "a7";//游泳
         public const string ANI_SINGLE_H_ATK = "a4";//单手
         public const string ANI_DOUBLE_H_ATK = "a6";//双手
         public const string ANI_MOUNT_SH_ATK = "a22";//坐骑单手
         public const string ANI_MOUNT_DH_ATK = "a23";//坐骑双手
         public const string ANI_DIE = "a8";//死亡
         public const string ANI_DISMOUNT = "a17";//下坐骑
         public const string ANI_GETHIT = "a9";//受击动作

        //声音相关
        //世界类型和副本类型声音常量
        public  const string WORLD_BG_1 = "world_bg_1";
        public  const string WORLD_BG_2 = "world_bg_2";
        public  const string SCRIPT_BG_1 = "script_bg_1";
        public  const string SCRIPT_BG_2 = "script_bg_2";

        //鼠标单击声音
        public  const string MOUSE_CLICK_1 = "mouse_click_1";
        //UI界面打开声音
        public  const string UI_OPEAN_1 = "ui_opean_1";
        //UI界面关闭声音
        public  const string UI_CLOSE_1 = "ui_close_1";
        //任务接收
        public  const string TASK_RECEIVE_1 = "task_receive_1";
        //任务完成
        public  const string TASK_FINISH_1 = "task_finish_1";
        //物品进入背包声音
        public  const string TO_BAG_1 = "to_bag_1";
        //背包整理声音
        public  const string MAKE_BAG_1 = "make_bag_1";
        //装备修理声音
        public  const string REPAIR_EQ_1 = "repair_eq_1";
        //铜币的交接声音
        public  const string PAY_GOLD_1 = "pay_gold_1";
        //人物升级声音
        public  const string CHARACTER_UP_1 = "character_up_1";
        //竞技场等待音效
        public  const string ARENA_WAITING = "arena_waiting";
        //锻造成功
        public  const string FORGING_SUCCESS_1 = "forging_success_1";
        //锻造失败
        public  const string FORGING_FAIL_1 = "forging_fail_1";
        //锻造中材料放入
        public  const string FORGING_ADD_1 = "forging_add_1";
        //好友上线成功
        public  const string FEINED_LOGIN_1 = "frined_login_1";
        //密语
        public  const string PRIVATE_TALK_1 = "private_talk_1";
        //信件
        public  const string MAIL_1 = "mail_1";
        //挂售物品出售
        public  const string SALE_1 = "sale_1";
        //交互类声音效果
        public  const string CONNECT_MUSIC = "connect_music";
        //技能声音
        public  const string SKILL_1 = "skill_1";
        public  const string SKILL_2 = "skill_2";
        //穿脱装备音效
        public  const string TAKE_OFF = "take_off";
        public  const string PUT_ON = "put_on";

         public const uint CAST_SKILL_TYPE_NOTAR = 0;//无目标释放
         public const uint CAST_SKILL_TYPE_TARGET = 1;//对指定目标释放
         public const uint CAST_SKILL_TYPE_POINT = 2;//对指定目标点释放

         public const string SUPERTXT_PROFIX = "<txt ";//supertext 的头
         public const string SUPERTXT_END = "/>";//supertext 的头
         public const string SUPERTXT_BR = "<br/>";//supertext 的头

        public  const uint ICMF_NO_NOTIFY = 1;    // 无道具变化内容道具变化消息，需客户端重新获取相应背包、仓库等道具列表，已弃用
        public  const uint ICMF_COL_ITEM = 2;    // 采集获得道具
        public  const uint ICMF_DECOMP_EQP = 3;    // 分解装备消耗道具
        public  const uint ICMF_DELAY_EXPIRE = 4;    // 延长装备时效消耗道具
        public  const uint ICMF_LVL_PRIZE = 5;    // 领取副本通关奖励获得道具
        public  const uint ICMF_OPEN_PKG = 6;    // 开启礼包获得道具
        public  const uint ICMF_FGUESS_RETURN = 7;    // 划拳失败后返回获得的游戏币
        public  const uint ICMF_LVL_TM_COST = 8;    // 计时副本消耗
        public  const uint ICMF_PICK_ITEM = 9;    // 拾取物品
        public  const uint ICMF_RMV_EQP = 100; 	// 脱下装备
        public  const uint ICMF_NEW_ITEM = 101; 	// 新物品

        //plattype
        public  const int PLAT_LY = 1;//联运
        public  const int PLAT_QQ = 2;//QQ
        public  const int PLAT_YN = 3;//越南
        public  const int PLAT_YY = 4;//多玩YY
        public  const int PLAT_TG = 5;//泰国
        public  const int PLAT_360 = 6;//360
        public  const int PLAT_BM = 7;//北美
        public  const int PLAT_P4399 = 10;//4399
        public  const int PLAT_NM = 11;//南美
        public  const int Plat_TW = 9;//台湾

        //platid
        public  const int PLATID_P51 = 12;//51
        public  const int PLATID_PPTV = 14;//pptv

        //界面功能开启入口
        public  const string MULTILVL_CHALLENGE = "multilvl_challenge";//副本大厅挑战副本
        public  const string LUCKYDRAW_RADLUCKY = "luckydraw_radlucky";//幸运转盘
        public  const string LUCKYDRAW_RADCHARGE = "luckydraw_radcharge";//充值转盘
        public  const string LUCKYDRAW_RADCONSUME = "luckydraw_radconsume";//消费转盘




    }
}
