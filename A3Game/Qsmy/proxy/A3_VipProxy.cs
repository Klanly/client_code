using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
namespace MuGame
{
    class A3_VipProxy : BaseProxy<A3_VipProxy>
    {
        public static uint EVENT_ON_VIP_CHANGE = 0;

        public A3_VipProxy()
        {
            addProxyListener(PKG_NAME.S2C_VIP_RES, OnVip);
        }

#region client to server

        public void GetVip()
        {
            sendRPC(PKG_NAME.S2C_VIP_RES, null);
        }

        public void GetVipGift( int val)
        {
            Variant msg = new Variant();
            msg["gift_id"] = val;
            sendRPC(PKG_NAME.S2C_VIP_RES, msg);
        }

#endregion



#region server to client callback

        void OnVip(Variant data)
        {
            A3_VipModel vipModel = A3_VipModel.getInstance();
            int res = data["res"];
            if(res == 1)
            {
                debug.Log(data.dump());
                vipModel.Level = data["viplvl"];
                vipModel.Exp = data["vipexp"];
                vipModel.isGetVipGift.Clear();
                foreach (uint itemid in data["vip_gifts"]._arr)
                {
                    vipModel.isGetVipGift.Add(itemid);
                }
                A3_VipModel.getInstance().viplvl_refresh();
            }
            if (a3_vip.instan)
                a3_vip.instan.OnGiftBtnRefresh();

            if (res == 2)
            {
                
            }

            if (data.ContainsKey("vip_level"))
            {
                //TODO 获取VIP等级奖励
            }

            //dispatchEvent(GameEvent.Create(EVENT_ON_VIP_CHANGE, this, data));
        }


#endregion
    }
}
