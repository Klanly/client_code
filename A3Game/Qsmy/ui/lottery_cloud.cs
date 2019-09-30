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
    class lottery_cloud:Window
    {
        public override void init()
        {
            getEventTrigerByPath("lottery_way/close").onClick = onClose;
        }
        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.LOTTERY_CLOUD);
            InterfaceMgr.getInstance().open(InterfaceMgr.LOTTERY);
        }
    }
}
