using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_pet_desc : Window
    {
        public override void init()
        {

            getComponentByPath<Text>("title").text = ContMgr.getCont("a3_pet_desc_0");
            getComponentByPath<Text>("descTxt").text = ContMgr.getCont("a3_pet_desc_1");

            getEventTrigerByPath("closeBtn").onClick = OnClose;
            this.getEventTrigerByPath("ig_bg_bg").onClick = OnClose;
        }

        private void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_PET_DESC);
        }
    }
}
