using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
namespace MuGame
{
    class VipProxy : BaseProxy<VipProxy>
    {
        public static uint EVENT_ON_VIP_CHANGE = 0;
        public VipProxy()
        {
            addProxyListener(228, onVip);
            addProxyListener(229, onBuyTime);
        }
        public void sendBuyTime(int id)
        {
            Variant msg = new Variant();
            msg["int_act"] = id;
            msg["cnt"] = 1;
            sendRPC(229, msg);
        }
        public void sendLoadVip()
        {
            sendRPC(228, null);
        }
        void onBuyTime(Variant data)
        {
            int res = data["res"];
            if (res != 1)
            {
                Globle.err_output(res);
                return;
            }
        }
        void onVip(Variant data)
        {
            PlayerModel.getInstance().vipChange(data);
            dispatchEvent(GameEvent.Create(EVENT_ON_VIP_CHANGE, this, data));
        }
    }
}
