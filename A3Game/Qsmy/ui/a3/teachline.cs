using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;

namespace MuGame
{
    class teachline : Window
    {
        public static float remainCD = 1;
        public static string desc = "";
        public static void show(string str,float sec)
        {
            remainCD = sec;
            desc = str;

            if (instance != null)
            {
                instance.showDesc();
            }

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.NEWBIE_LINE);
        }


        public Text txt;
        public RectTransform bg;
        public static teachline instance;
        override public bool showBG
        {
            get { return false; }
        }

        public override void init()
        {
            bg = getComponentByPath<RectTransform>("Image");
            txt = getComponentByPath<Text>("Text");
        }

        public override void onShowed()
        {
            showDesc();
            instance = this;
          
            base.onShowed();
        }

        public void showDesc()
        {
            txt.text = desc;
        
            bg.sizeDelta = new Vector2(txt.preferredWidth+20, bg.sizeDelta.y);
            CancelInvoke("close");
            Invoke("close", remainCD);
        }

        public override void onClosed()
        {
            instance = null;
        }

        void close()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.NEWBIE_LINE);
        }


    }
}
