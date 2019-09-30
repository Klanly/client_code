using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    public class GlobleSetting
    {
        public static bool SOUND_ON = true;
        public static bool MUSIC_ON = true;
        //gameSetting
        public static bool REFUSE_TEAM_INVITE = false;//拒绝组队邀请
        public static bool IGNORE_PRIVATE_INFO = false;//屏蔽私聊信息
        public static bool IGNORE_KNIGHTAGE_INVITE = false;//屏蔽骑士团邀请
        public static bool IGNORE_FRIEND_ADD_REMINDER = false;//屏蔽好友添加提示
        public static bool IGNORE_OTHER_EFFECT = false;//屏蔽他人特效
        public static bool IGNORE_OTHER_PLAYER = false;//屏蔽其他玩家 (只显示自己的模型,其它玩家的模型都不显示,包括宠物,只显示其它玩家的名字/称号等信息)
        public static bool IGNORE_OTHER_PET = false;//屏蔽其他玩家宠物

        
        static int minSoundValue = 0;
        static int minMusicValue = 0;
        public static void initSystemSetting()
        {
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SYS_SOUND))
                SOUND_ON = PlayeLocalInfo.loadInt(PlayeLocalInfo.SYS_SOUND) > minSoundValue;
            else
                SOUND_ON = true;
            MediaClient.instance.isPlaySound = SOUND_ON;

            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SYS_MUSIC))
                MUSIC_ON = PlayeLocalInfo.loadInt(PlayeLocalInfo.SYS_MUSIC) > minMusicValue;
            else
                MUSIC_ON = true;

            MediaClient.instance.isPlayMusic = MUSIC_ON;
            initSetting();
        }

        public static void setSound(bool on)
        {
            SOUND_ON = on;
            float soundValue = MediaClient.instance.getSoundVolume();
            int currentValue = (int)(float.Parse(soundValue.ToString("F2")) * 100);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.SYS_SOUND, on ? currentValue : minSoundValue);
            MediaClient.instance.isPlaySound = on;
        }
        public static void setMusic(bool on)
        {
            MUSIC_ON = on;
            float musicValue = MediaClient.instance.getMusicVolume();
            int currentValue = (int)(float.Parse(musicValue.ToString("F2")) * 100);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.SYS_MUSIC, on ? currentValue : minMusicValue);
            MediaClient.instance.isPlayMusic = on;
        }
        public static void initSetting()
        {
            initSystem();
            initGameSetting();
        }

        private static void initGameSetting()
        {
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.REFUSE_TEAM_INVITE))
            {
                REFUSE_TEAM_INVITE = PlayeLocalInfo.loadInt(PlayeLocalInfo.REFUSE_TEAM_INVITE) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_PRIVATE_INFO))
            {
                IGNORE_PRIVATE_INFO = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_PRIVATE_INFO) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_KNIGHTAGE_INVITE))
            {
                IGNORE_KNIGHTAGE_INVITE = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_KNIGHTAGE_INVITE) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_FRIEND_ADD_REMINDER))
            {
                IGNORE_FRIEND_ADD_REMINDER = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_FRIEND_ADD_REMINDER) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_OTHER_EFFECT))
            {
                IGNORE_OTHER_EFFECT = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_OTHER_EFFECT) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_OTHER_PLAYER))
            {
                IGNORE_OTHER_PLAYER = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_OTHER_PLAYER) == 1 ? true : false;
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.IGNORE_OTHER_PET))
            {
                IGNORE_OTHER_PET = PlayeLocalInfo.loadInt(PlayeLocalInfo.IGNORE_OTHER_PET) == 1 ? true : false;
            }
        }

        private static void initSystem()
        {
            //if (PlayeLocalInfo.checkKey(PlayeLocalInfo.VIDEO_QUALITY))
            //{
            //    SceneCamera.m_fScreenGQ_Level = float.Parse(PlayeLocalInfo.loadString(PlayeLocalInfo.VIDEO_QUALITY));
            //}

            //if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SCREEN_RESOLUTION))
            //{
            //    if (PlayeLocalInfo.loadInt(PlayeLocalInfo.SCREEN_RESOLUTION) != 0)
            //        SceneCamera.m_nScreenResolution_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.SCREEN_RESOLUTION);
            //}

            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.MODEL_DETAIL))
            {
                if (PlayeLocalInfo.loadInt(PlayeLocalInfo.MODEL_DETAIL) != 0)
                    SceneCamera.m_nModelDetail_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.MODEL_DETAIL);
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.FPS_LIMIT))
            {
                if (PlayeLocalInfo.loadInt(PlayeLocalInfo.FPS_LIMIT) != 0)
                    SceneCamera.m_nFPSLimit_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.FPS_LIMIT);
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.ROLE_SHADOW))
            {
                if (PlayeLocalInfo.loadInt(PlayeLocalInfo.ROLE_SHADOW) != 0)
                    SceneCamera.m_nShadowAndLightGQ_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.ROLE_SHADOW);
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SCENE_DETAIL))
            {
                if (PlayeLocalInfo.loadInt(PlayeLocalInfo.SCENE_DETAIL) != 0)
                    SceneCamera.m_nSceneGQ_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.SCENE_DETAIL);
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SKILL_EFFECT))
            {
                if(PlayeLocalInfo.loadInt(PlayeLocalInfo.SKILL_EFFECT) != 0)
                    SceneCamera.m_nSkillEff_Level = PlayeLocalInfo.loadInt(PlayeLocalInfo.SKILL_EFFECT);
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SYS_SOUND))
            {
                int soundValue = PlayeLocalInfo.loadInt(PlayeLocalInfo.SYS_SOUND);
                float valuef = (float)soundValue / 100.0f;
                MediaClient.instance.setSoundVolume(valuef);
            }
            else
            {
                MediaClient.instance.setSoundVolume(0.8f);//设置默认值
            }
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SYS_MUSIC))
            {
                int musicValue = PlayeLocalInfo.loadInt(PlayeLocalInfo.SYS_MUSIC);
                float valuef = (float)musicValue / 100.0f;
                MediaClient.instance.setMusicVolume(valuef);
            }
            else
            {
                MediaClient.instance.setMusicVolume(0.8f);//设置默认值
            }
            //if (PlayeLocalInfo.checkKey(PlayeLocalInfo.SCREEN_RESOLUTION))
            //{
            //    int skilleffValue = PlayeLocalInfo.loadInt(PlayeLocalInfo.SCREEN_RESOLUTION);
            //    SceneCamera.m_nSkillEff_Level = skilleffValue;
            //}
        }

    }
}
