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
    class TaelsProxy : BaseProxy<TaelsProxy>
    {

        public static uint SHOWED_TAELS = 1;
        public static uint TAELS = 2;


        public TaelsProxy()
        {
            addProxyListener(PKG_NAME.S2C_TAELS_EXCHANGE, onExchange);
        }
        public void sendExchange()
        {
            Variant msg = new Variant();
            msg["yinpiao_use"] = 0;
            sendRPC(PKG_NAME.S2C_TAELS_EXCHANGE, msg);
        }

        public void sendInfo()
        {
            Variant msg = new Variant();
            msg["yinpiao_info"] = 0;
            sendRPC(PKG_NAME.S2C_TAELS_EXCHANGE, msg);
        }



        public void onExchange(Variant data)
        {
            debug.Log("银票：" + data.dump());
            if (data["yinpiao_count"] != null)
            {
                dispatchEvent(GameEvent.Create(SHOWED_TAELS, this, data));
            }
            if (data["yinpiao_success"] != null)
            {
                if (data["res"] != null && data["res"] < 0)
                {
                    Globle.err_output(data["res"]);
                    return;
                }
                dispatchEvent(GameEvent.Create(TAELS, this, data));
            }
        }

    }
}
