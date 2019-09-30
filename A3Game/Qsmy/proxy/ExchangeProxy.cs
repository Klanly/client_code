using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class ExchangeProxy : BaseProxy<ExchangeProxy>
    {
        public static uint EVENT_EXCHANGE_SUC = 0;  //兑换成功
        public static uint EVENT_EXCHANGE_SYNC_COUNT = 1; //同步兑换次数

        public ExchangeProxy()
        {
            addProxyListener(PKG_NAME.S2C_EXCHANGE_RES, OnExchange);
        }

        /// <summary>
        /// 获取可招财的次数
        /// </summary>
        public void GetExchangeInfo()
        {
            Variant msg = new Variant();
            msg["op"] = 0;
            sendRPC(PKG_NAME.C2S_EXCHANGE, msg);
        }

        /// <summary>
        /// 进行一次招财
        /// </summary>
        public void Exchange()
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.C2S_EXCHANGE, msg);
        }

        public void OnExchange(Variant data)
        {
            int tp = data["res"];
            switch (tp)
            {
                case 0: //服务器同步的招财次数
                    OnSyncCount(data);
                    //IconAddLightMgr.getInstance().showOrHideFire("Light_exchange", data);
                    break;
                case 1: //服务器返回的招财结果,同时会同步招财次数
                    OnOneceExchange(data);
                    //IconAddLightMgr.getInstance().showOrHideFire("Light_exchange", data);
                    break;
                default:
                    flytxt.instance.fly(ContMgr.getCont("ExchangeProxy_shibai"));
                    break;
            }
        }

        private void OnOneceExchange(Variant data)
        {
            ExchangeModel exModel = ExchangeModel.getInstance();
            exModel.Count = data["yinpiao_count"];
            dispatchEvent(GameEvent.Create(EVENT_EXCHANGE_SUC, this, null));
        }

        private void OnSyncCount(Variant data)
        {
            ExchangeModel exModel = ExchangeModel.getInstance();
            exModel.Count = data["yinpiao_count"];
            dispatchEvent(GameEvent.Create(EVENT_EXCHANGE_SYNC_COUNT, this, null));
        }


    }
}
