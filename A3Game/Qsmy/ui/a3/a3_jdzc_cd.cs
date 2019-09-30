using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_jdzc_cd : FloatUi
    {
        public static Action cdhandle;
        public static Action forceStophandle;
        public static long secCD;
        public static Vector3 pos;
        public static a3_jdzc_cd instance;

        public override void init() {


        }

        public static void show(Action handle, float sec, bool isMS = false, Action stopHandle = null, Vector3 v = new Vector3()) {
            SelfRole.fsm.Stop();

            cdhandle = handle;
            forceStophandle = stopHandle;
            if (isMS) { secCD = (long)sec; } else { secCD = (long)(sec*1000); }
            pos = v;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_JDZC_CD);
        }


        public override void onShowed()
        {
            instance = this;
        }

        public override void onClosed()
        {
            instance = null;

        }


        public static void hide() {
            if (instance == null)
                return;
        }
        private IEnumerator runcd() {





            yield break;
        }

    }
}
