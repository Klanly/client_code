using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class GameSdkMgr
    {
        private static GameSdkMgr _instance;

        public static GameSdk_base m_sdk;

        public static void init()
        {
            if (_instance == null)
            {
                _instance = new GameSdkMgr();
                switch (Globle.QSMY_SDK_Index)
                {
                    case ENUM_SDK_PLATFORM.QISJ_QUICK:
                        m_sdk = new GameSdk_quick();
                        break;
                    case ENUM_SDK_PLATFORM.QISJ_RYJTDREAM:
                        m_sdk = new GameSdk_ryjtDream();
                        break;
                    case ENUM_SDK_PLATFORM.DL_SDK:
                        m_sdk = new GameSdk_DL();
                        break;
                }
            }
        }

        public GameSdkMgr()
        {
            //  AnyPlotformSDK.Cmd_CallBack();
        }


        public static void Pay(rechargeData data)
        {
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            m_sdk.Pay(data);
        }

        public static void sharemsg(string sharemsg, string sharetype, string shareappid, string shareappkey)
        {
            m_sdk.sharemsg(sharemsg, sharetype, shareappid, shareappkey);
        }

        public static void record_selectionSever()
        {
            m_sdk.record_selectionSever();
        }


        public static void record_createRole(Variant data)
        {
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            m_sdk.record_createRole(data);
        }


        public static void record_login()
        {
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            m_sdk.record_login();
        }


        public static void record_LvlUp()
        {
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            m_sdk.record_LvlUp();
        }


        public static void record_quit()
        {
            if (Globle.QSMY_Platform_Index != ENUM_QSMY_PLATFORM.QSPF_LINKSDK) return;

            m_sdk.record_quit();
        }

      
        public static bool beginVoiceRecord()
        {
            return m_sdk.beginVoiceRecord();
        }
        public static void showbalance()
        {
            m_sdk.showbalance();
        }

        public static void cancelVoiceRecord()
        {
            m_sdk.cancelVoiceRecord();
        }

        public static void endVoiceRecord()
        {
            m_sdk.endVoiceRecord();
        }

        public static void playVoice(string path)
        {
            m_sdk.playVoice(path);
        }

        public static void stopVoice(string path)
        {
            m_sdk.stopVoice(path);
        }
        public static void clearVoices()
        {
            m_sdk.clearVoices();
        }

    }
}
