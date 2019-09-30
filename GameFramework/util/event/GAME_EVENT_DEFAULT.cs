namespace GameFramework
{
	//1 - 1000为消息处理事件
	//1001-4000 为游戏事件
	//4001-6000 为界面处理事件
	public class GAME_EVENT_DEFAULT
	{
		public const uint TYPE_PKG_OFFSET = 2000;
		public const uint CONN_SET = 3011;		//设置连接参数
		public const uint CONN_ED = 3012;		//连接 成功
		public const uint CONN_ERR = 3013;		//连接 
		public const uint CONN_VER = 3014;		//连接 
		public const uint CONN_VERX = 3020;		//连接 
		public const uint CONN_CLOSE = 3015;		//连接 
		public const uint CONN_FAILE = 3016;		//连接 

		public const uint ON_LOGIN = 3021;				//
		public const uint ON_LOAD_MIN = 3022;			//首加载资源
		public const uint ON_LOAD_BEFORE_GAME = 3023;	//进入游戏预加载资源
		public const uint ON_LOAD_MAP = 3024;			//进入游戏预加载资源

		public const uint ON_SCENE_INIT_FIN = 3025;		//图形引擎初始化完成
		public const uint ON_SCENE_MAP_CREATE = 3026;	//地图创建完成 ,当前引擎需要创建完了地图后才能创建角色需保证顺序

		public const uint ON_ENTER_GAME = 3034;			//完成进入游戏

        public const uint LG_MEDIA_PLAY = 3301; //声音播放
	}
}