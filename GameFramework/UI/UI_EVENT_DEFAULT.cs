namespace GameFramework
{
	//1 - 1000为消息处理事件
	//1001-4000 为游戏事件
	//4001-6000 为界面处理事件
	public class UI_EVENT_DEFAULT
	{
		//界面广播事件
		public const uint UI_OPEN = 4001;               //打开指定界面
		public const uint UI_CLOSE = 4002;              //关闭指定界面
		public const uint UI_OPEN_SWITCH = 4003;        //反向开关界面
	}
}