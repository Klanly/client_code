
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    class RechargeProxy : BaseProxy<RechargeProxy>
    {
        const uint C2S_GET_RECHARGE = 113;
        const uint S2C_GET_RECHARGE_RES = 113;
        public static uint LIS_RECHARGE_TYPE_RES = 1;
        public Dictionary<int, int> Rechargetm = new Dictionary<int, int>();

        public RechargeProxy()
        {
            addProxyListener(S2C_GET_RECHARGE_RES, getRechargeDate);
        }
        public void sendGetRechargeInfo()
        {

            sendRPC(C2S_GET_RECHARGE, null);
        }

        public void getRechargeDate(Variant data)
        {
            Rechargetm[1] = 0;
            Rechargetm[2] = 0;
            Rechargetm[3] = 0;
            if (data.ContainsKey("monthly"))
            {
                Rechargetm[1] = data["monthly"]._int32;
            }
            if (data.ContainsKey("quarterly"))
            {
                Rechargetm[2] = data["quarterly"]._int32;
            }
            if (data.ContainsKey("yearly"))
            {
                Rechargetm[3] = data["yearly"]._int32;
            }
            dispatchEvent(GameEvent.Create(LIS_RECHARGE_TYPE_RES, this, null));
        }
    }
}
