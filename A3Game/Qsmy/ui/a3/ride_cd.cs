using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    public class ride_cd : FloatUi
    {
        Image transLine;
        public float currTime = 0;
        public static float READTIME = 2f;
        public static Action callBack = null;
        public static bool isShow = false;

        override public bool showBG
        {
            get { return false; }
        }

        public override void init()
        {
            transLine = getTransformByPath( "line" ).GetComponent<Image>();
        }

        public override void onShowed()
        {
            currTime = 0f;
        }

        public override void onClosed()
        {
            isShow = false;
        }

        public static void show( Action handle , bool isMain ) {

            if ( isMain )
            {
                isShow = true;
                InterfaceMgr.getInstance().ui_async_open( InterfaceMgr.RIDE_CD );

                callBack = handle;

            }
            else {

                handle();
            }

        }


        void Update()
        {
            if ( isShow == false || transLine == null )
            {
                return;
            }

            currTime = currTime + Time.deltaTime;

            if ( currTime >= READTIME )
            {
                isShow = false;

                currTime = 0f;

                transLine.fillAmount = 1;

                if ( callBack != null )
                {
                    callBack();
                }
                
                hide();
            }

            transLine.fillAmount = currTime / READTIME;

        }


        public static void hide(  )
        {
                callBack = null;
                isShow = false;
                InterfaceMgr.getInstance().close( InterfaceMgr.RIDE_CD );
        }

    }

}
