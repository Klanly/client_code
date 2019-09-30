using UnityEngine;
namespace MuGame
{
   public  class PlayeLocalInfo
    {
        static public readonly string DEBUG_UID = "debugUid";
        static public readonly string DEBUG_TKN = "debugTkn";
        static public readonly string DEBUG_SELECTED = "bebugSelect";

        static public readonly string AUTOFIGHT = "autofight";

        static public readonly string LOGIN_SERVER_SID = "sid";

        static public readonly string SYS_SOUND = "sysSound";
        static public readonly string SYS_MUSIC = "sysMusic";

        //setting
        //static public readonly string VIDEO_QUALITY = "video_Quality";//画面品质
        //static public readonly string VIDEO_QUALITY = "video_Quality";//画面品质
        static public readonly string SKILL_EFFECT = "skillEffect";//技能效果
        static public readonly string SCREEN_RESOLUTION  = "screen_solution";//分辨率
        static public readonly string FPS_LIMIT   = "dynamicLighting";//动态光影
        static public readonly string ROLE_SHADOW   = "roleShadow";//人物影子
        static public readonly string SCENE_DETAIL  = "sceneDetail";//场景细节
        static public readonly string MODEL_DETAIL = "modelDetail"; //其他玩家模型
        //gameSetting
        static public readonly string REFUSE_TEAM_INVITE         = "refuse_team_invite";  //拒绝组队邀请
        static public readonly string IGNORE_PRIVATE_INFO        = "ignore_private_info";  //屏蔽私聊信息
        static public readonly string IGNORE_KNIGHTAGE_INVITE    = "ignore_knightage_invite";  //屏蔽骑士团邀请
        static public readonly string IGNORE_FRIEND_ADD_REMINDER = "ignore_friend_add_reminder";  //屏蔽好友添加提示
        static public readonly string IGNORE_OTHER_EFFECT        = "ignore_other_effect";  //屏蔽他人特效
        static public readonly string IGNORE_OTHER_PLAYER        = "ignore_other_player";  //屏蔽其它玩家
        static public readonly string IGNORE_OTHER_PET           = "ignore_other_pet";            //屏蔽其它玩家宠物


        static public readonly string DEBUG_SHOW = "debugShow";



        static public void saveInt(string id,int value)
        {
            PlayerPrefs.SetInt(id, value);
            PlayerPrefs.Save();
        }

        static public void saveString(string id, string value)
        {
            PlayerPrefs.SetString(id, value);
            PlayerPrefs.Save();
        }

        static public int loadInt(string id)
        {
            if (!checkKey(id))
                return 0;
            return PlayerPrefs.GetInt(id);
        }

        static public string loadString(string id)
        {
            return PlayerPrefs.GetString(id);
        }

        static public bool checkKey(string id)
        {
            return PlayerPrefs.HasKey(id);
        
        }

    }
}
