using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class sdkloading:LoadingUI
    {
        public override void init()
        {
            base.init();
            getComponentByPath<Text>("Text").text = ContMgr.getCont("sdkloading_0");
        }

        public override void onShowed()
        {
            InterfaceMgr.getInstance().showLoadingBg(true);
            base.onShowed();
        }

        public override void onClosed()
        {
            InterfaceMgr.getInstance().showLoadingBg(false);
            base.onClosed();
        }
    }
}
