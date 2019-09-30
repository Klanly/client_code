using System;
using Cross;
using GameFramework;
namespace MuGame
{
    public class logicDataEvent
    {
         public const int LDET_MAP_ADD_PlY = 1;
         public const int LDET_MAP_RMV_PlY = 2;

         public const int GET_SELF_CLAN_INFO = 200;

         public const int LDET_SELF_DIE = 100;			//自己死亡
         public const int ON_LVL_UP = 101;				//自己升级
         public const int ON_GOLD_CHANGE = 102;			//自己铜币改变
         public const int ON_MODE_EXP = 103;				//自己经验改变
         public const int ON_MODE_SKEXP = 104;			//技能值改变
         public const int ON_MODE_NOBPT = 105;			//声望改变
         public const int ON_MODE_MERIPT = 106;			//星魂改变
         public const int ON_MODE_BNDYB = 107;			//礼券改变
         public const int ON_RESET_LVL = 108;				//转生改变
         public const int MISSION_CHANGE = 203;			//自己任务改变

         public const int BOSS_DATA_CHANGE = 300;				//boss数据改变
         public const int BOSS_DATA_UPDATE = 301;				//单个boss数据更新
    }
}
