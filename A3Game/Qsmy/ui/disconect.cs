using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
namespace MuGame
{
    class disconect : Window
    {
        public static int ERROR_TYPE_DISCONNECT = 0;
        public static int ERROR_TYPE_SOCKET = 1;
        public static int ERROR_TYPE_SERVER = 2;
        public static int ERROR_TYPE_NETWORK = 3;

        public static bool showLine = false;

        public static bool needResetMusic = false;

        GameObject goInfo;
        GameObject goLoading;
        Text txt;

        public static disconect instance;

        public override void init()
        {
            goInfo = getGameObjectByPath("info");
            goLoading = getGameObjectByPath("loading");
            txt = getComponentByPath<Text>("info/txt");

            getEventTrigerByPath("info/bt").onClick = onCLick;

            getComponentByPath<Text>("loading/Text0").text = ContMgr.getCont("disconect_0");
            getComponentByPath<Text>("loading/Text1").text = ContMgr.getCont("disconect_1");
            getComponentByPath<Text>("loading/Text1").text = ContMgr.getCont("disconect_2");
            getComponentByPath<Text>("loading/Text3").text = ContMgr.getCont("disconect_3");
            getComponentByPath<Text>("info/Text").text = ContMgr.getCont("disconect_4");
            getComponentByPath<Text>("info/txt").text = ContMgr.getCont("disconect_5");
        }


        public override void onShowed()
        {
            transform.SetAsLastSibling();
            instance = this;
            NetClient.instance.addEventListener(GAME_EVENT.CONN_ERR, onError);

            //getGameObjectByPath("info/line").SetActive(showLine);
            //showLine = false;

            setErrorType(ERROR_TYPE_DISCONNECT);

            base.onShowed();

            if (a3_wing_skin.instance != null)
                a3_wing_skin.instance.wingAvatar.SetActive(false);
            //if (a3_summon.instan && a3_summon.instan.m_selectedSummon)
            //{
            //    a3_summon.instan.m_selectedSummon.SetActive(false);
            //}
            if (a3_summon_new.getInstance)
            {
                //for (int i = 0;i< a3_summon_new.getInstance.Avatorlist.Count;i++)
                //{
                //    if (a3_summon_new.getInstance.Avatorlist[i] != null&& a3_summon_new.getInstance.Avatorlist[i].activeSelf)
                //    {
                //        a3_summon_new.getInstance.Avatorlist[i].SetActive(false);
                //    }
                //}
                if (a3_summon_new.getInstance.avatorobj != null)
                {
                    a3_summon_new.getInstance.avatorobj.SetActive(false);
                }
                if (a3_summon_new.getInstance.avator_look != null) {
                    a3_summon_new.getInstance.avator_look.SetActive(false);
                }
            }

            NewbieModel.getInstance().hide();
            
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_DIS_CONECT);
        }

        public override void onClosed()
        {
            instance = null;

            NetClient.instance.removeEventListener(GAME_EVENT.CONN_ERR, onError);
            base.onClosed();
        }

        public void onError(GameEvent e)
        {
            setErrorType(ERROR_TYPE_SOCKET);
        }


        private int timers;
        public void setErrorType(int type)
        {
            if (muNetCleint.instance.CurServerTimeStamp - timers < 3)
            {
                DelayDoManager.singleton.AddDelayDo(() =>
                {
                    if (instance != null)
                        refreshText(type);
                }, 3);
            }
            else
            {
                refreshText(type);
            }


        }

        public void refreshText(int type)
        {
            goLoading.SetActive(false);
            goInfo.SetActive(true);

            txt.text = StringUtils.formatText(ContMgr.getCont("disconect_" + type));
        }

        override public bool showBG
        {
            get { return true; }
        }


        public override void dispose()
        {
            instance = null;
            //getComponentByPath<Button>("bt").onClick.RemoveAllListeners();
            NetClient.instance.removeEventListener(GAME_EVENT.CONN_ERR, onError);
            base.dispose();
        }

        void onCLick(GameObject go)
        {
            if (GlobleSetting.MUSIC_ON)
                disconect.needResetMusic = true;
            goLoading.SetActive(true);
            goInfo.SetActive(false);
            timers = muNetCleint.instance.CurServerTimeStamp;
            LGPlatInfo.inst.relogin();
            MapModel.getInstance().curLevelId = 0;
            //      LGPlatInfo.inst.logSDKAP("roleLogout");

            //  Application.Quit();
            //InterfaceMgr.getInstance().destory(InterfaceMgr.DISCONECT);
            //muLGClient.instance.g_outGameCT.dispatchEvent(GameEvent.Create(GAME_EVENT.GAME_INIT_START, this, null));
        }

    }
}
