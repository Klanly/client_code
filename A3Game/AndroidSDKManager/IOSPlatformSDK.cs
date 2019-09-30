using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Cross;
using UnityEngine;



namespace MuGame
{
    public class IOSPlatformSDK : IPlotformSDK
    {
        //!--IOS插件声明，所有unity调用ios SDK插件走这里
        //[DllImport("__Internal")]
        //public static extern void CallSDKFunc(string type, string jsonpara);

        public static Action<String, String> IOS_PLOTFORM_SDK_CALL;

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
            if (Application.platform != RuntimePlatform.IPhonePlayer) return;
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            //CallSDKFunc(info, jstr);
        }

        public void Call_Cmd(string cmd, string info = null, string jstr = null, bool waiting = true)
        {
            debug.Log("Call Cmd " + cmd + "  " + info + "  " + jstr + "\n");
            if (Application.platform != RuntimePlatform.IPhonePlayer) return;
            ////if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            ////if (m_bInCalling == false)
            //{
            //    //if (waiting)
            //    //    InterfaceMgr.getInstance().open(InterfaceMgr.SDK_LOADING);

            //    //m_bInCalling = true;

            //    if (info != null && jstr != null)
            //    {
            //        CallSDKFunc(info, jstr);
            //    }

            //    CallSDKFunc(cmd, "");
            //}

            IOS_PLOTFORM_SDK_CALL(cmd, jstr);
            debug.Log("IOS_PLOTFORM_SDK_CALL::");
        }

        int _inited = -1;//-1未初始化,1为成功，2为失败
        public int isinited
        {
            get { return _inited; }
        }

        public void Cmd_CallBack(Variant v)
        {
            debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!Cmd_CallBack!!!!!");

            //InterfaceMgr.getInstance().close(InterfaceMgr.SDK_LOADING);
            m_bInCalling = false;


            int result = v["result"];

            debug.Log(result.ToString());

            if (result < 3)
            {
                _inited = result;
            }
            else if (result == 11 || result == 12 || result == 13)//登录游戏
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
            else if (result == 42)//退出游戏
            {
                GameSdkMgr.record_quit();
                AnyPlotformSDK.Call_Cmd("close");
                // Application.Quit();


            }
            else if (result == 21)//登出
            {
                GameSdkMgr.record_quit();

                m_nResetAppCount = 1;
                //Application.Quit();
            }
        }
    }
}
