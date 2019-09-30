using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;

namespace MuGame
{
    class a3_baotuUI : FloatUi
    {
        public override void init()
        {
            inText();
            new BaseButton(this.transform.FindChild("go")).onClick = (GameObject go) =>
            {
                SelfRole.fsm.Stop();
                SelfRole.moveToNPc(10, npcId: 1001);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAOTUUI);
            };
            new BaseButton(this.transform.FindChild("ok")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAOTUUI);
            };
        }

        void inText()
        {
            this.transform.FindChild("bg/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_baotuUI_1");
            this.transform.FindChild("ok/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_baotuUI_2");
            this.transform.FindChild("go/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_baotuUI_3");
        }

        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
        }
    }
}