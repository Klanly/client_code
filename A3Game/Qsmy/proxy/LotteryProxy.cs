using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;

namespace MuGame
{
    class LotteryProxy : BaseProxy<LotteryProxy>
    {
        public static uint LOADLOTTERY = 1;
        public static uint LOTTERYOK_FREEDRAW = 2;//免费抽
        public static uint LOTTERYOK_ICEDRAWONCE = 3;//钻石抽一次
        public static uint LOTTERYOK_ICEDRAWTENTH = 4;//钻石连抽10次
        public static uint LOTTERYTIP_FREEDRAW = 5;//免费抽取的提醒
        public static uint LOTTERYOK_ICED_NEWBIE = 6;//指引抽一次
        public static uint LOTTERYNEW_ITEM = 7;//在开启抽奖页面时接收到新的抽奖信息
        public static uint LOTTERYOK_FREE_TENTH = 8;

        public static uint NEWDRAW = 9;//高级抽奖
        

        public LotteryProxy()
        {
            addProxyListener(PKG_NAME.C2S_LOTTERY, onloadlottery);
        }
        public void sendlottery(int id)
        {
            Variant msg = new Variant();
            msg["lottery_cmd"] = id;
            debug.Log("发送"+ msg.dump ());
            sendRPC(PKG_NAME.C2S_LOTTERY, msg);
        }
        public void onloadlottery(Variant data)
        {
            int res = data["res"];
            debug.Log("C#：：" + data.dump());
            if (res == (int)LotteryType.CurrentInfo)//获取当前抽奖信息
            {
                IconAddLightMgr.getInstance().showOrHideFire("ShowFreeDrawAvaible", data);
                //InterfaceMgr.doCommandByLua("a1_low_fightgame.ShowFreeDrawAvaible", "ui/interfaces/low/a1_low_fightgame", data);
                dispatchEvent(GameEvent.Create(LOADLOTTERY, this, data));

            }
            else if (res == (int)LotteryType.FreeDraw)//免费抽奖
            {
                dispatchEvent(GameEvent.Create(LOTTERYOK_FREEDRAW, this, data));
            }
            else if (res == (int)LotteryType.IceDrawOnce)//钻石抽奖
            {
                dispatchEvent(GameEvent.Create(LOTTERYOK_ICEDRAWONCE, this, data));
            }
            else if (res == (int)LotteryType.IceDrawTenth)//钻石十连抽
            {
                dispatchEvent(GameEvent.Create(LOTTERYOK_ICEDRAWTENTH, this, data));
            }
            else if (res == (int)LotteryType.FreeTenth)
            {
                dispatchEvent(GameEvent.Create(LOTTERYOK_FREE_TENTH, this, data));
            }
            else if (res == (int)LotteryType.NewBieDraw)
            {
                dispatchEvent(GameEvent.Create(LOTTERYOK_ICED_NEWBIE, this, data));
            }
            else if (res == (int)LotteryType.NewDrawInfo)
            {
                dispatchEvent(GameEvent.Create(LOTTERYNEW_ITEM, this, data));
            }
            else if (res == (int)LotteryType.newDraw)
            {
                dispatchEvent(GameEvent.Create(NEWDRAW, this, data));
            }
            else
            {
                if (res < 0)
                    Globle.err_output(res);
                return;
            }
        }
    }
}
