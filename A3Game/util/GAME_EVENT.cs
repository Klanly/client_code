namespace MuGame
{
	//1 - 1000为消息处理事件
	//1001-4000 为游戏事件
	//4001-6000 为界面处理事件
	public class GAME_EVENT
	{

		public const uint LOAD_DEF_RES = 3001;  //加载最小运行资源  图片，配置等
		public const uint CONN_SET = 3011;		//设置连接参数
		public const uint CONN_ED = 3012;		//连接 成功
		public const uint CONN_ERR = 3013;		//连接 
		public const uint CONN_VER = 3014;		//连接 
		public const uint CONN_VERX = 3020;		//连接 
		public const uint CONN_CLOSE = 3015;		//连接 
		public const uint CONN_FAILE = 3016;		//连接 

		public const uint GAME_LOADING = 3050;		//加载

		public const uint GAME_INIT_START = 3060;		//准备游戏

		public const uint ON_LOGIN = 3021;				//
		public const uint ON_LOAD_MIN = 3022;			//首加载资源
		public const uint ON_LOAD_BEFORE_GAME = 3023;	//进入游戏预加载资源
		public const uint ON_LOAD_MAP = 3024;			//进入游戏预加载资源

		public const uint ON_SCENE_INIT_FIN = 3025;		//图形引擎初始化完成
		public const uint ON_SCENE_MAP_CREATE = 3026;	//地图创建完成 ,当前引擎需要创建完了地图后才能创建角色需保证顺序

		public const uint ON_ENTER_GAME = 3034;			//完成进入游戏
		public const uint ON_MAP_CHANGE = 3035;			//切地图

        public const uint DELETE_RETURE = 2004;        //删除角色服务器返回的参数
        public const uint S2C_CREAT_CHAR = 2005;       //创建角色服务器返回参数 

		//public const uint GET_SKILL = 2006;            //获取技能
		//public const uint LEARN_SKILL = 2007;          //学习技能
		//public const uint COST_SKILL = 2008;           //技能消耗
		//public const uint SETUP_SKILL = 2009;          //skillup

        public const uint GET_SKILL_LGUIMAIN = 2010;   //获取技能列表到主界面
        public const uint LEARN_SKILL_LGUIMAIN = 2011; //学习技能到主界面
        public const uint GET_SKILL_LGUICHAR = 2012;     //获取技能链表到主角面板
        public const uint LEARN_SKILL_LGUISYSTEM = 2013; //学习技能到UI系统设置界面
        public const uint SETUPSKILL_LGSKILL = 2014;     //setup到lgskill界面

		public const uint ON_CREATE_CHAR = 3031;		//
		public const uint ON_DELETE_CHAR = 3032;		//
		public const uint ON_SELECT_CHAR = 3033;		//

		public const uint ON_MAP_CLK = 3035;			//

		public const uint ITEM_ADD = 3036;      	//物品添加 
		public const uint ITEM_MOD = 3037;      	//物品修改
		public const uint ITEM_RMV = 3038;      	//物品删除
		public const uint ITEM_INFO_INIT = 3039;	//物品数数初始化
		public const uint ITEM_USE = 3040;			//物品使用
		public const uint ITEM_PICK = 3041;			//物品拾取
		public const uint ITEM_GIVE_UP = 3042;		//物品丢弃
		public const uint ITEM_BUY = 3043;			//物品购买
		public const uint ITEM_SELL = 3044;			//物品出售

        public const uint MIS_ACCEPT = 3045;		//接受任务
        public const uint MIS_COMMIT = 3046;		//提交任务
        public const uint MIS_FINISH = 3047;		//完成任务
        public const uint MIS_ABORD = 3048;		    //放弃任务
        public const uint MIS_TRANS = 3049;			//传送任务
        public const uint MIS_AUTO = 3050;			//自动任务
        public const uint MIS_DATA_CHANGE = 3051;	//任务数据变化

        public const uint UI_SHOP_CHOOSE = 3061;    //切换商城分页选取数据
        public const uint UI_SHOP_PREPARE = 3062;   //切换商城分页填充数据
        public const uint UI_SHOP_ACHIEVE = 3063;   //传递购买道具

        public const uint UI_CHAT_SEND = 3066;      //传递要发送的消息
        public const uint UI_CHAT_RES_SPEAKER = 3067;//判断是否广播消息
        public const uint UI_CHAT_RES_ME = 3068;    //发送消息返回值
        public const uint UI_CHAT_GET_OTHER = 3069; //获得其他人的聊天

        //逻辑类lgGD——》LGUI、UI
        public const uint LG_MEDIA_PLAY = 3301;
        public const uint LGGD_ACCEPT_MIS_RES = 3302;
        public const uint LGGD_COMMIT_MIS_RES = 3303;
        public const uint LGGD_SET_MISSION = 3304; 
        public const uint LGGD_CAN_COMMIT = 3305; 


	 
		public const uint TYPE_PKG_OFFSET = 2000;	//
		public const uint SVR_CONF_OK = 2100;		//
		public const uint CONF_MAPS = 2101;			//
		public const uint CONF_MAP = 2102;			//
		public const uint CONF_MISSION = 2103;		//
		public const uint CONF_MONSTER = 2104;		//
		//		public const uint CONF_ = 210;		//
		//		public const uint CONF_ = 210;		//
		//		public const uint CONF_ = 210;		//
		//		public const uint CONF_ = 210;		//
		//public const uint RPC_ = 1;		//
		//public const uint RPC_ = 1;		//
		//地图操作
		public const uint MAP_CHANGE = 2160; //
		public const uint MAP_INIT = 2161; //
		public const uint MAP_SIZE_CHANGE = 2162; //
		public const uint MAP_LINK_ADD = 2163; //
        public const uint MAP_ADD_SHOW_EFF = 2164; //
        //public const uint MAP_LOAD_READY = 2165; //
        public const uint MAP_ADD_FLY_EFF = 2166; //

		public const uint CAMERA_INIT = 2170; //
		public const uint CAMERA_BIND_CHAR = 2171; //

		public const uint SCENE_CREATE_CAMERA = 2180;			//创建主相机
		public const uint SCENE_CREATE_MAP = 2181;			//创建场景地图
		public const uint SCENE_CREATE_NPC = 2182;			//创建npc
		public const uint SCENE_CREATE_MAIN_CHAR = 2183;	//创建主角
		public const uint SCENE_CREATE_MONSTER = 2184;		//创建怪物
		public const uint SCENE_CREATE_OTHER_CHAR = 2185;		//创建其他玩家
        public const uint SCENE_CREATE_HERO = 2186;		//创建英雄
		

		//场景操作
		public const uint SPRITE_SET_DATA = 2100; //
		public const uint SPRITE_SET_VISIBLE = 2200; //
		public const uint SPRITE_ORI = 2201; //
		public const uint SPRITE_MOVE = 2202; //
		//public const uint SPRITE_STOP = 2203; //
		//public const uint SPRITE_ANI = 2204; //  动作变化
		public const uint SPRITE_DISPOSE = 2205; //
		public const uint SPRITE_SET_Z = 2204; //
		public const uint SPRITE_SET_XY = 2206; //
		public const uint SPRITE_SIZE_CHANGE = 2207; //
		public const uint SPRITE_MAP_CREATE = 2208; //
		public const uint SPRITE_CAMERA_LOOK_AT = 2209; //
		public const uint SPRITE_ADD_SHOW_EFF = 2210; // avatar特效
		//public const uint SPRITE_CHANGE_AVATAR = 2211; // 换装
		//public const uint SPRITE_REMOVE_AVATAR = 2212; // 换装
		//public const uint SPRITE_ANI_END = 2213; // 单个动作播完
		public const uint SPRITE_ONHURT_SHOW = 2220; 
        public const uint SPRITE_MAINPLAYER_MOVE = 2221; //  角色移动
        public const uint SPRITE_GR_CAMERA_MOVE = 2222; //  摄相机移动
        //public const uint SPRITE_ADD_TITLE = 2223; //  添加头顶的图片、文字
        //public const uint SPRITE_REMOVE_TITLE = 2224; //  移除头顶的图片、文字
        //public const uint SPRITE_SET_HP = 2225; //  头顶血条、防护值
        public const uint SPRITE_OBJ_MASK = 2226;//遮挡物体透视
        //public const uint SPRITE_REMOVE_EFF = 2227;//移除avatar特效
      
		//		public const uint SPRITE_ = 220; //
		//		public const uint SPRITE_ = 220; //
		//		public const uint SPRITE_ = 220; //
		//		public const uint SPRITE_ = 220; //
		public const uint SPRITE_ON_CLICK = 2280; // 选中
		//public const uint SPRITE_ON_CLICK_NPC = 2281; // 选中
		//public const uint SPRITE_ON_CLICK_SELF = 2282; // 选中
		//public const uint SPRITE_ON_CLICK_MON = 2283; // 选中
		//public const uint SPRITE_ON_CLICK_OTHER = 2284; // 选中
		public const uint SPRITE_DIE = 2285; //

		public const uint SPRITE_OP_REACHED = 2300; //
		public const uint SPRITE_OP_REACHED_INTER = 2301; //

       //public const uint GET_MAIL_LIST = 2311;
       //public const uint GET_MAIL = 2312;
       //public const uint GOT_NEW_MAIL = 2313;
       //public const uint SEND_ERROR_RES = 2314;
        //public const uint LOCK_MAIL_RES = 2315;
        //public const uint GET_MAIL_ITEM_RES = 2316;
        //public const uint SEND_MAIL_RES = 2317;
        //public const uint DEL_MAIL_RES = 2318;
        //public const uint OUTPUT_SERVER_ERR = 2319;

     


        public const uint S2C_GET_DBMKT_ITM = 5004;
        //自身属性改变相关
        public const uint MODE_HEXP = 5005;
        public const uint MODE_CLANG = 5006;
        public const uint SELF_INFO_CHANGE = 5007;
        public const uint MODE_BATPT = 5008;
        public const uint MODE_NOBPT = 5009;
        public const uint MODE_CARRLVLCHANGE= 5010;
        public const uint PRIZELVL = 5011;
        public const uint MODE_SOULPT = 5012;
        public const uint MODE_SHOPPT = 5013;
        public const uint MODE_LOTEXPT = 5014;
        public const uint MODE_TCYB_LOTT_COST = 5015;
        public const uint MODE_TCYB_LOTT = 5016;
        public const uint REFRESHLOTCNT = 5017;
        public const uint REFRESHLVLSHARE = 5018;
        public const uint ON_LVL_UP = 5019;
        public const uint REFRESH_GROW_PACK = 5020;
        public const uint PLAYER_INFO_CHANGED = 5021;
        public const uint R_PLAYER_INFO_CHANGED = 5022;
        public const uint MODIFY_TEAMMATE_DATA = 5023;
        public const uint MODE_ATTPT = 5024;
        public const uint ONADDPOINT = 5025;
        public const uint PASSWORD_SAFEDATARES = 5026;
        public const uint ONUPGRADE_NOBRES = 5027;
        public const uint SET_MERGE_INFO = 5028;
        public const uint ROLL_PT_BACK = 5029;
        public const uint ON_RESET_LVL = 5030;
        public const uint UPGRADE_RIDE_RES = 5031;
        public const uint RIDE_CHANGE = 5032;
        public const uint UPDATE_PLAYER_NAME = 5033;
        public const uint ON_INVEST_BACK = 5034;
        public const uint GET_OI_AWDS = 5035;
        public const uint PKGS_ITEM_BACK = 5036;
        public const uint RIDE_QUAL_ATT_ACTIVE_RES = 5037;
        public const uint SELECT_RIDE_SKILL_RES = 5038;
	}
}