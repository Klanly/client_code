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
using DG.Tweening;
namespace MuGame
{
    class a3_giftCard : Window
    {
        static public a3_giftCard instance;

        GiftCardData giftdata;

        public override void init()
        {
            inText();
            instance = this;
            BaseButton btnClose = new BaseButton(transform.FindChild("btn_do"));
            btnClose.onClick = onBtnClose;
        }

        void inText() {
            this.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_giftCard_1");//2017年1月补偿卡
            this.transform.FindChild("btn_do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_giftCard_2");//确认
        }
        public override void onShowed()
        {
            base.onShowed();
            if (uiData != null && uiData.Count > 0)
            {
                giftdata = (GiftCardData)uiData[0];
            }

            if (giftdata == null)
                return;

            transform.FindChild("name").GetComponent<Text>().text = giftdata.cardType.name;
            transform.FindChild("desc").GetComponent<Text>().text = giftdata.cardType.desc;

            Transform con = transform.FindChild("itemMask/items");
            for (int i = 0; i < con.childCount; i++)
            {
                GameObject.Destroy(con.GetChild(i).gameObject);
            }
            foreach (BaseItemData one in giftdata.cardType.lItem)
            {
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(uint.Parse(one.id), true, one.num);
                icon.transform.FindChild("bicon").gameObject.SetActive(true);
                icon.transform.SetParent(con, false);
            }
           
        }
        void onBtnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GIFTCARD);
        }
    }
}
