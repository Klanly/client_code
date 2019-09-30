using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using UnityEngine.EventSystems;
using System.Collections;

namespace MuGame
{
    class a3_doubleExp : Window
    {

        int num = 0;
        GameObject haveCon,nullCon;

        public override void init() {
            haveCon = this.transform.FindChild("have").gameObject;
            nullCon = this.transform.FindChild("null").gameObject;

            new BaseButton(this.transform.FindChild("have/do")).onClick = (GameObject go) => {
                a3_BagModel.getInstance().useItemByTpid(1528,1);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_DOUBLEEXP);
            };

            new BaseButton(this.transform.FindChild("null/do")).onClick = (GameObject go) => {
                LevelProxy.getInstance().buyAnduseExp();
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_DOUBLEEXP);
            };

            new BaseButton (transform.FindChild("null/close")).onClick =
            new BaseButton(transform.FindChild("have/close")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_DOUBLEEXP);
            };
            inText();
        }

        void inText() {
            transform .FindChild ("have/do/Text").GetComponent <Text>().text = ContMgr.getCont("a3_doubleExp_2");//立即使用
            transform.FindChild("have/close/Text").GetComponent<Text>().text = ContMgr.getCont("a3_doubleExp_3");//返回

            transform.FindChild("null/do/Text").GetComponent<Text>().text = ContMgr.getCont("a3_doubleExp_4");//购买并使用
            transform.FindChild("null/close/Text").GetComponent<Text>().text = ContMgr.getCont("a3_doubleExp_5");//返回
            transform.FindChild("null/Text").GetComponent<Text>().text = ContMgr.getCont("a3_doubleExp_6");//双倍经验卷轴不足
        }

        public override void onShowed() {

            haveCon.SetActive(false);
            nullCon.SetActive(false);

            num = a3_BagModel.getInstance().getItemNumByTpid(1528);
            int money = XMLMgr.instance.GetSXML("golden_shop.golden_shop", "id==" + 1037).getInt("value");


            this.transform.FindChild("null/do/num").GetComponent<Text>().text = "X" + money;

            if (num > 0) 
                haveCon.SetActive(true);
            else
                nullCon.SetActive(true);

            this.transform.FindChild("have/Text").GetComponent<Text>().text = ContMgr.getCont("a3_doubleExp_1", num.ToString());

            transform.SetAsLastSibling();

        }
        public override void onClosed() { }
    }
}
