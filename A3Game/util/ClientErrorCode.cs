using System;
using Cross;
using GameFramework;
namespace MuGame
{
    public class ClientErrorCode
    {
        public ClientErrorCode()
		{
		}
		
		public static String error_string(int errcode)
		{
			return LanguagePack.getLanguageText("clientError",errcode.ToString());
		}
        //static public var TEST_ERR:uint = 1;
		
		//聊天错误信息
		public const int CHATMSG_1                  = -1001;    //聊天框发送消息太频繁
		public const int CHATMSG_2                  = -1002;    //【队伍】未加入队伍，不能发言
		public const int CHATMSG_3                  = -1003;    //【战盟】未加入战盟，不能发言
		public const int CHATMSG_4                  = -1004;    //请输入正确的私聊对象名称格式"/" + 名称  + 空格  + 内容
		public const int CHATMSG_5                  = -1005;    //世界聊天等级不够(等级在general中配置)
		public const int NULL_MSG                   = -1006;    //发送内容不能为空，请重新输入!
		public const int NO_SPEAKER                 = -1007;    //缺少小喇叭，无法发布全服喊话！
		
		
		//道具相关错误提示 2001 - ；
		public const int NO_USE_ITEM                = -2001;    //"该道具不可直接使用"
		public const int NO_SELECT_ITEM             = -2002;    //"没有选择道具！"
		public const int ITEM_IN_CD                 = -2003;    //"尚未冷却"
		public const int ITEM_NO_SPLIT              = -2004;    //"道具不可拆分"
		public const int WAREHOUSE_IS_FULL          = -2005;    //"仓库已满"
		public const int ITEM_NO_DELET              = -2006;    //"此装备不可销毁!"
		public const int NO_WAREHOUSE               = -2007;    //"仓库未开启"
		public const int NO_TRANS_IN_LEVEL          = -2008;    //副本中不可使用传送道具
		public const int PACKAGE_IS_FULL            = -2009;    //背包已满，无法进行购买
		public const int ITEM_NO_SELL               = -2010;    //道具无法出售
		public const int NO_ADD_ITEM_IN_BATCHSELL   = -2011;    //无法添加出售道具
		public const int OPEN_PREPO_SUCCESS        	= -2012;    // 随身仓库开通成功！
		
		//战盟相关错误提示 2020-
		public const int JOIN_CLAN_FAIL        	    = -2020;    // 申请加入战盟失败
		public const int CREAT_CLAN_FAIL        	= -2021;    //创建战盟失败，您数入的名称内含有敏感字。
		public const int CLANNAME_PUT_ERROR        	= -2022;    //请输入正确的名称。
		public const int MESG_TOO_LOOG        	    = -2023;    //你输入的信息太长。
		public const int CHANGGE_NOTICE_FAIL        = -2024;    //修改公告失败，您输入的信息内含有敏感字。
		public const int NO_CLAN_LEADER        	    = -2025;    //只有副盟主才能进行弹劾
		public const int NO_FIRE_CLAN_LEADER        = -2026;    //盟主威震四方，不可弹劾
		
		//组队相关错误提示 2040-
		public const int JOIN_TEAM_FAIL             = -2040;	//加入组队失败
			
		//任务相关错误提示 2060-
		public const int HAS_MAST_QUAL              = -2060;	//已达最大品质
		public const int NO_ENOUGH_GOLD             = -2061;	//没有足够的金币
        

    }
}