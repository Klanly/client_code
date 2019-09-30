using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
namespace MuGame
{
    class A3_SlayDragonProxy : BaseProxy<A3_SlayDragonProxy>
    {
        public static readonly uint REFRESH = 1;
        public static readonly uint OPEN_LVL = 2;
        public static readonly uint END_TIME = 3;
        private const int SEND_UNLOCK_DRAGON = 3;
        private const int SEND_ENTER_DRAGONROOM = 2;
        private const int SEND_CREATE_DRAGONROOM = 1;
        private const int SEND_GET_DRAGONINFO = 4;
        private const int SEND_TRYUNLOCK_DRAGON = 5;
        public A3_SlayDragonProxy()
        {
            addProxyListener(PKG_NAME.S2C_CLAN_SLAYDRAGON,OnSlayDragon);
        }

        private void OnSlayDragon(Variant data)
        {
            int res = data["res"];
            debug.Log("军团屠龙消息::::" + data.dump());            
            switch (res)
            {
                case 1:
                    dispatchEvent(GameEvent.Create(REFRESH, this, data));
                    break;
                case 2:
                    dispatchEvent(GameEvent.Create(OPEN_LVL, this, data));
                    break;
                case 3:
                    dispatchEvent(GameEvent.Create(END_TIME, this, data));
                    break;
                default:
                    break;
            }
        }

        //请求创建巨龙副本
        public void SendCreate(uint dragonId,int diffLv)
        {
            Variant data = new Variant();
            data["op"] = SEND_CREATE_DRAGONROOM;
            data["ltpid"] = dragonId;
            data["diff_lvl"] = diffLv;
            sendRPC(PKG_NAME.C2S_CLAN_SLAYDRAGON, data);
        }
        //前往巨龙副本
        public void SendGo()
        {
            Variant data = new Variant();
            data["op"] = SEND_ENTER_DRAGONROOM;
            sendRPC(PKG_NAME.C2S_CLAN_SLAYDRAGON, data);
        }
        //召唤巨龙
        public void SendUnlock(uint dragonId)
        {
            Variant data = new Variant();
            data["op"] = SEND_UNLOCK_DRAGON;
            data["lvl_id"] = dragonId;            
            sendRPC(PKG_NAME.C2S_CLAN_SLAYDRAGON, data);
        }
        //请求信息
        public void SendGetData()
        {
            Variant data = new Variant();
            data["op"] = SEND_GET_DRAGONINFO;
            sendRPC(PKG_NAME.C2S_CLAN_SLAYDRAGON, data);
        }
        //上交育龙果实
        public void SendGive(int num = 1)
        {
            Variant data = new Variant();
            data["op"] = SEND_TRYUNLOCK_DRAGON;
            data["num"] = num;
            sendRPC(PKG_NAME.C2S_CLAN_SLAYDRAGON, data);
        }        
    }
}
