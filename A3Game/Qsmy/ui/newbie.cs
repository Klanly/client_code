using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class newbie : FloatUi
    {

        GameObject obj;
        public override void onShowed()
        {
            if (uiData != null && uiData.Count > 0)
            {
                obj = (GameObject)uiData[0];
            }

            CancelInvoke("timeGo");
            Invoke("timeGo", 0.5f);
        }

        void timeGo()
        {
            InterfaceMgr.getInstance().close(this.uiName);
        }


        public override void onClosed() {

            if(obj != null)
                obj.SetActive(true);
            //触发新手指引
            UiEventCenter.getInstance().onWinClosed(uiName);
        }
    }
}
