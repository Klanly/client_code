using System;
using Cross;
using UnityEngine;

namespace MuGame
{
    public class AndroidPlotformSDK : IPlotformSDK
    {
        public static Action<String> ANDROID_PLOTFORM_SDK_CALL;
        public static Action<String, String> ANDROID_PLOTFORM_SDK_INFO_CALL;
        public static Action ANDROID_HIDE_STATUSBAR;
        private static bool m_bInCalling = false;
        private static int m_nResetAppCount = 0;
        public static bool m_bLogined = false;

        public static Action<Variant> LOGIN_ACTION;

        public void FrameMove()
        {
            if (m_nResetAppCount > 0)
            {
                m_nResetAppCount--;
                if (0 == m_nResetAppCount)
                {
                    debug.Log("重启游戏APP");
                    AnyPlotformSDK.Call_Cmd("resetapp");
                }
            }
        }

        public void Add_moreCmdInfo(string info, string jstr)
        {
            if (Application.platform != RuntimePlatform.Android) return;
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            ANDROID_PLOTFORM_SDK_INFO_CALL(info, jstr);
        }


        public int isinited
        {
            get { return 1; }
        }


        public void Call_Cmd(string cmd, string info = null, string jstr = null, bool waiting = true)
        {
            debug.Log("Call_Cmd::" + Application.platform + " " + Globle.QSMY_Platform_Index);
            if (Application.platform != RuntimePlatform.Android) return;
          //  if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            //if (m_bInCalling == false)
            {
                //if (waiting)
                //    InterfaceMgr.getInstance().open(InterfaceMgr.SDK_LOADING);

                m_bInCalling = true;

                if (info != null && jstr != null)
                {
                    ANDROID_PLOTFORM_SDK_INFO_CALL(info, jstr);
                }

                ANDROID_PLOTFORM_SDK_CALL(cmd);

                debug.Log("ANDROID_PLOTFORM_SDK_CALL::");
            }
        }


        public void Cmd_CallBack(Variant v)
        {
            //发送的请求返回了 这里现只处理充值

            debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!Cmd_CallBack!!!!!" + v.dump());
            int result = v["result"];
            //if (result != 85)
            //    InterfaceMgr.getInstance().close(InterfaceMgr.SDK_LOADING);
            m_bInCalling = false;


            if (result == 11 || result == 12 || result == 13)//登录游戏
            {
                if (m_bLogined && result == 11)
                {
                    //切换账号
                    m_nResetAppCount = 1;
                }
                else
                {
                    LOGIN_ACTION(v);
                }
            }
            else if (result == 43)//退出游戏
            {
                GameSdkMgr.record_quit();
                //AnyPlotformSDK.Call_Cmd("close");
                // Application.Quit();


            }
            else if (result == 21)//登出
            {
                debug.Log("登出回调！！！！！");

                AnyPlotformSDK.Call_Cmd("resetapp");
                //GameSdkMgr.record_quit();
                //AnyPlotformSDK.Call_Cmd("loginout");
                m_nResetAppCount = 1;
                //Application.Quit();
            }
            else if (result == 61) //切换
            {
                //AnyPlotformSDK.Call_Cmd("switchAccount");
            }
            else if (result == 81)//开始录音
            {
                Variant data = v["data"];

                GameSdkMgr.m_sdk.voiceRecordHanlde("begin", "", 0);
            }
            else if (result == 82)//开始录音失败
            {
                Variant data = v["data"];
                if (data.ContainsKey("error"))
                {
                    GameSdkMgr.m_sdk.voiceRecordHanlde("error", data["error"], 0);
                    return;
                }

            }
            else if (result == 83)//结束录音
            {
                Variant data = v["data"];

                GameSdkMgr.m_sdk.voiceRecordHanlde("end", data["url"], data["seconds"]);
            }
            else if (result == 84)//结束录音失败
            {
                Variant data = v["data"];
                if (data.ContainsKey("error"))
                {
                    GameSdkMgr.m_sdk.voiceRecordHanlde("error", data["error"], 0);
                    return;
                }
            }
            else if (result == 91)//播放语音失败
            {
                GameSdkMgr.m_sdk.voicePlayedHanlde("error");
            }
            else if (result == 92)//播放结束
            {
                // if (GameSdkMgr.playedHanlde != null)
                GameSdkMgr.m_sdk.voicePlayedHanlde("played");
            }
          
        }


    }
}
