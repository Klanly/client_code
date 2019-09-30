using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
namespace MuGame
{
    class GiftCardProxy : BaseProxy<GiftCardProxy>
    {
        public GiftCardProxy()
        {
            addProxyListener(PKG_NAME.C2S_ITEM_CARD_RES, onItemCardInfo);
            addProxyListener(PKG_NAME.S2C_ON_ERR_MSG, onErrorRes);
        }

        public void sendFetchCard(string id)
        {
            Variant d = new Variant();
            d["cardid"] = id;
            debug.Log("发送协议激活码:" + id);
            sendRPC(PKG_NAME.C2S_FETCH_ITM_CARD, d);
        }

        public void sendLoadItemCardInfo(List<int> list = null)
        {
            Variant d = new Variant();

            if (list != null)
            {
                List<Variant> l = new List<Variant>();
                foreach (int num in list)
                {
                    Variant v = new Variant();
                    v._uint = (uint)num;
                    l.Add(v);
                }   
                //  d["awdtps"]._arr= l;
            }

            sendRPC(PKG_NAME.C2S_ITEM_CARD_RES, d);
        }

        private void onErrorRes(Variant data)
        {
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
            }
        }

        private void onItemCardInfo(Variant d)
        {
            if (d.ContainsKey("tp"))//兑换完奖励
            {
                HttpAppMgr.instance.onGetRewardItems(d["tp"]);
                return;
            }

            if (d.ContainsKey("tpchange"))//兑换码变化
            {
             
                int sec = ConfigUtil.getRandom(0, 20);

                DelayDoManager.singleton.AddDelayDo(() =>
                {
                    sendLoadItemCardInfo();
                }, sec);
                debug.Log("兑换码后台发生变！！！间隔发送请求礼品卡：" + sec);
                return;
            }

            if (d.ContainsKey("card_fetch"))
            {
                Variant card_fetch = d["card_fetch"];
                int tp = card_fetch["tp"];
                string cardid = card_fetch["cardid"];

                Variant reward = card_fetch["reward"];
                if (reward.ContainsKey("money"))
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_moneyadd") + reward["money"]);
                }
                if (reward.ContainsKey("yb"))
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_zuanshiadd") + reward["yb"]);
                }
                if (reward.ContainsKey("bndyb"))
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_bangzuanadd") + reward["bndyb"]);
                }
                if (reward.ContainsKey("itm"))
                {
                    List<Variant> tpawds = reward["itm"]._arr;
                    for (int i = 0; i < tpawds.Count; i++)
                    {
                        a3_ItemData one = a3_BagModel.getInstance().getItemDataById(tpawds[i]["tpid"]);
                        string name = Globle.getColorStrByQuality(one.item_name,one.quality);
                        flytxt.instance.fly(ContMgr.getCont("comm_get") + name + "+" + tpawds[i]["cnt"]);
                    }
                    
                }
            }

            //if (d.ContainsKey("fetched_cards"))
            //{
            //    Variant fetched_cards = d["fetched_cards"];
            //    int tp = fetched_cards["tp"];
            //    int tm = fetched_cards["tm"];//时间戳
            //}

            if (d.ContainsKey("cards"))
            {
                List<Variant> cardsa = d["cards"]._arr;
                for (int i = 0; i < cardsa.Count; i++)
                {
                    int tpa = cardsa[i]["tp"];
                    string cardida = cardsa[i]["cardid"];

                    if (tpa == 4)
                    {//补偿卡礼包通过邮件领取

                    }
                }
            }

            if (d.ContainsKey("tpawds"))
            {
                List<Variant> tpawds = d["tpawds"]._arr;
                for (int i = 0; i < tpawds.Count; i++)
                {
                    GiftCardType giftType = new GiftCardType();
                    giftType.init(tpawds[i]);
                    HttpAppMgr.instance.giftCard.addGiftType(giftType);
                }
            }

            HttpAppMgr.instance.giftCard.getAllCode();
           
            //if (d.ContainsKey("tpawds"))
            //{
            //    List<Variant> l = d["tpawds"];
            //    for (int i = 0; i < l.Count; i++)
            //    {

            //    }

            //}

        }
    }


}
