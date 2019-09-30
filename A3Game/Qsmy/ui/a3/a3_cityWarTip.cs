using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class a3_cityWarTip : Window
    {
        public override void init() {

            this.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_cityWarTip_1");
            this.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_cityWarTip_2");
            this.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_cityWarTip_3");


            new BaseButton(this.transform.FindChild("yes")).onClick = (GameObject go) => {
                A3_cityOfWarProxy.getInstance().sendInfb();
                InterfaceMgr.getInstance().close(this.uiName);
            };
            new BaseButton(this.transform.FindChild("no")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().close(this.uiName);
            };
        }

    }
}
