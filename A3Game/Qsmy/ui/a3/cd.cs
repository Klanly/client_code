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
    public class cd : FloatUi
    {
        public static Action cdhandle;
        public static Action forceStophandle;
        public static Vector3 pos;
        public static long lastTimer;
        public static long secCD;
        public static long lastCD;
        public static cd instance;

        public static Action<cd> updateHandle;

        override public bool showBG
        {
            get { return false; }
        }

        public Text txt;

        public static void show(Action handle, float sec, bool isMS = false, Action stopHandle = null, Vector3 v = new Vector3())
        {
            SelfRole.fsm.Stop();

            cdhandle = handle;
            forceStophandle = stopHandle;
            secCD = isMS ? (long)sec : (long)(sec * 1000);
            lastTimer = secCD + NetClient.instance.CurServerTimeStampMS;
            pos = v;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.CD);
        }

        public static void hide()
        {
            if (instance == null)
                return;

            if (forceStophandle != null && lastTimer > NetClient.instance.CurServerTimeStampMS)
            {
                forceStophandle();
            }

            InterfaceMgr.getInstance().close(InterfaceMgr.CD);
        }

        private Transform transLine;
        public override void init()
        {
            // alain();
            transLine = getTransformByPath("line");
            txt = getComponentByPath<Text>("txt");




        }

        public override void onShowed()
        {
            instance = this;
            transform.SetAsFirstSibling();
            RectTransform rec = gameObject.GetComponent<RectTransform>();
            rec.position = SelfRole._inst.getHeadPos()+new Vector3 (0,30f,0);
            if (pos == Vector3.zero) {
               rec.position = SelfRole._inst.getHeadPos() + new Vector3(0, 30f, 0);
            }
            else
                rec.localPosition = pos;
            MediaClient.instance.PlaySoundUrl("audio_common_trance", false, null);
            StartCoroutine(runcd());
        }

        public override void onClosed()
        {
            instance = null;
            cdhandle = null;
            updateHandle = null;
            if (txt != null)
                txt.text = "";

            StopCoroutine(runcd());
        }

        private IEnumerator runcd()
        {

            while (true)
            {
                long curtime = NetClient.instance.CurServerTimeStampMS;

                if (updateHandle != null)
                    updateHandle(this);


                if (lastTimer < curtime)
                {
                    if (cdhandle != null)
                        cdhandle();
                    InterfaceMgr.getInstance().close(InterfaceMgr.CD);
                    yield break;
                }

                lastCD = secCD - lastTimer + curtime;
                Vector3 vec = Vector3.one;
                vec.x = (float)lastCD / (float)secCD;
                transLine.localScale = vec;

                yield return null;
            }
            yield break;
        }

    }
}
