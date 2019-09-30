using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using MuGame;

namespace MuGame
{
    class a3_exchange : Window
    {
        int diamand ; 
        public static a3_exchange Instance;
        public override void init()
        {
            inText();
            Instance = this;
            SXML xml = XMLMgr.instance.GetSXML("acution.exchange");
            diamand = xml.getInt("initial");
            getComponentByPath<Text>("diamand/Text").text = diamand.ToString();
            this.getEventTrigerByPath("btclose").onClick = onClose;
            this.getEventTrigerByPath("exchangeBtn").onClick = onExchange;
        }


        void inText() {
            SXML xml = XMLMgr.instance.GetSXML("acution.exchange");
            int goldcins = xml.getInt("exc_gold");
            this.transform.FindChild("gold/Text").GetComponent<Text>().text = goldcins.ToString();
            this.transform.FindChild("exchangeBtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_exchange_2");
        }
        public override void onShowed()
        {
            ExchangeProxy.getInstance().addEventListener(ExchangeProxy.EVENT_EXCHANGE_SUC, onExchangeSuccess);
            ExchangeProxy.getInstance().addEventListener(ExchangeProxy.EVENT_EXCHANGE_SYNC_COUNT, onSyncCount);            
            refreshCount();
        }

        public override void onClosed()
        {
            ExchangeProxy.getInstance().removeEventListener(ExchangeProxy.EVENT_EXCHANGE_SUC, onExchangeSuccess);
            ExchangeProxy.getInstance().removeEventListener(ExchangeProxy.EVENT_EXCHANGE_SYNC_COUNT, onSyncCount);
            CreateModelOnClosed();
        }
        private void CreateModelOnClosed()
        {
            if (InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_LOTTERY))
            {
                a3_lottery.mInstance.CreateModel();
                return;
            }
        }
        void onExchangeSuccess(GameEvent e)
        {
            refreshCount();
        }

        void onSyncCount(GameEvent e)
        {
            refreshCount();
        }

        void refreshCount()
        {
            int num;
            if (A3_VipModel.getInstance().Level > 0)
                num = A3_VipModel.getInstance().vip_exchange_num(3);
            else
                num = 10;
            ExchangeModel exModel = ExchangeModel.getInstance();
            SXML xml = XMLMgr.instance.GetSXML("acution.exchange");
            diamand = xml.getInt("initial");
            getComponentByPath<Text>("diamand/Text").text = (diamand * (exModel.Count + 1)).ToString();
            if (num - exModel.Count >= 0)
                getComponentByPath<Text>("exchangeBtn/Text/leftCnt").text = "(" + (num - exModel.Count) + "/" + num + ")";
        }

        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EXCHANGE);
        }

        void onExchange(GameObject go)
        {
            ExchangeModel exModel = ExchangeModel.getInstance();
            SXML xml = XMLMgr.instance.GetSXML("acution.exchange");
            diamand = xml.getInt("initial");
            if (PlayerModel.getInstance().gold < diamand * (exModel.Count + 1))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_exchange_nozs"));
                return;
            }



            int num;
            if (A3_VipModel.getInstance().Level > 0)
                num = A3_VipModel.getInstance().vip_exchange_num(3);
            else
                num = 10;

            if ((num - exModel.Count) <= 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_exchange_yj"));
                return;
            }

            ExchangeProxy exProxy = ExchangeProxy.getInstance();
            exProxy.Exchange();
        }
    }
}
