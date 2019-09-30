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
    class a3_pkMapUI : FloatUi
    {
        public override void init()
        {

            getComponentByPath<Text>("bg/Text").text = ContMgr.getCont("a3_pkMapUI_0");
            getComponentByPath<Text>("bg/Text1").text = ContMgr.getCont("a3_pkMapUI_1");
            getComponentByPath<Text>("bg/Text2").text = ContMgr.getCont("a3_pkMapUI_2");
            getComponentByPath<Text>("bg/Text3").text = ContMgr.getCont("a3_pkMapUI_3");
            getComponentByPath<Text>("bg/Text4").text = ContMgr.getCont("a3_pkMapUI_4");
            getComponentByPath<Text>("bg/Text5").text = ContMgr.getCont("a3_pkMapUI_5");
            getComponentByPath<Text>("ok/Text").text = ContMgr.getCont("a3_pkMapUI_6");       
            getComponentByPath<Text>("cancel/Text").text = ContMgr.getCont("a3_pkMapUI_6");
            new BaseButton(this.transform.FindChild("cancel")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMAPUI);
            };

            new BaseButton(this.transform.FindChild("ok")).onClick = (GameObject go) =>
            {
                a3_PkmodelProxy.getInstance().sendProxy(2);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMAPUI);
            };
        }

        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
        }
    }
}
