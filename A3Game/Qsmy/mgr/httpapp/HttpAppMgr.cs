using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    public class HttpAppMgr : GameEventDispatcher
    {
        public static uint EVENT_GET_GIFT_CARD = 0;
        public static uint EVENT_GETREWARD_ITEMS = 1;

        public GiftCardsApp giftCard;


        public HttpAppMgr()
        {


        }

        public void initGift()
        {
            if (giftCard == null)
                giftCard = new GiftCardsApp();
        }

        public int getTodyPayedRmb()
        {
            if(giftCard==null || giftCard.rechangeTaskData==null)
                return 0;

            return giftCard.rechangeTaskData.moneyPayed;
        }

        


        public void sendInputGiftCode(string code)
        {
            if (code.Length == 5)
            {
                giftCard.getShortCardsCode(code);
            }
            else if (code.Length == 7)
            {
                giftCard.getSevenCardsCode(code);
            }
            else
                GiftCardProxy.getInstance().sendFetchCard(code);
        }
   

        public void onGetnewCard()
        {
            debug.Log("获得新礼品发event");
            dispatchEvent(GameEvent.Create(EVENT_GET_GIFT_CARD, this, null));


           useGiftCards(giftCard.lGiftCards[0]);
        }

        public void onGetRewardItems(int tp)
        {


            GiftCardType giftTp = giftCard.getTp(tp);
   
            debug.Log("兑换玩成！：");
            dispatchEvent(GameEvent.Create(EVENT_GETREWARD_ITEMS, this, giftTp));
        }

        public List<GiftCardData> getGiftCards()
        {
            if (giftCard == null)
                return new List<GiftCardData>();

            return giftCard.lGiftCards;
        }

        public void useGiftCards(GiftCardData card)
        {
            if (card == null)
                return;
            debug.Log("使用礼品卡:"+card.id);
            card.getItems();
            giftCard.lGiftCards.Remove(card);
        }



        public static void POSTSvr(string query, string param, Action<Variant> cb, bool rcvJSONHandler = true, string method = "POST")
        {

            ////这里要改成异步的才能继续
            //return;
            if (query == null || query == "" || cb == null) return;
            IURLReq urlReq = os.net.CreateURLReq(null);
            urlReq.url = query;
            urlReq.contentType = NetConst.URL_CONTENT_TYPE_URLENCODE;
            urlReq.dataFormat = NetConst.URL_DATA_FORMAT_TEXT;

            string data = "";
            data += param;
            debug.Log(" POSTSvr query:" + query + "\n param:" + param);
            urlReq.data = data;

            //DebugTrace.dumpObj("POSTSvr data:" + data);

            urlReq.method = method;


            urlReq.load(
                //delegate(IURLReq r, byte[] vari)
                delegate(IURLReq r, object vari)
                {
                    if (vari == null)
                    {
                        DebugTrace.print(" POSTSvr urlReq.load vari Null!");
                    }
                    string str = vari as string;

                    DebugTrace.print(" POSTSvr urlReq.loaded str[" + str + "]!");

                    Variant t = JsonManager.StringToVariant(str);

                    if (cb != null)
                        cb(JsonManager.StringToVariant(str));

                },
                null,
                null
            );

        }

        public static void POSTSvrstr(string query, string param, Action<string> cb, bool rcvJSONHandler = true, string method = "POST")
        {

            ////这里要改成异步的才能继续
            //return;
            if (query == null || query == "" || cb == null) return;
            IURLReq urlReq = os.net.CreateURLReq(null);
            urlReq.url = query;
            urlReq.contentType = NetConst.URL_CONTENT_TYPE_URLENCODE;
            urlReq.dataFormat = NetConst.URL_DATA_FORMAT_TEXT;

            string data = "";
            data += param;
            debug.Log(" POSTSvr query:" + query + "\n param:" + param);
            urlReq.data = data;

            //DebugTrace.dumpObj("POSTSvr data:" + data);

            urlReq.method = method;


            urlReq.load(
                //delegate(IURLReq r, byte[] vari)
                delegate(IURLReq r, object vari)
                {
                    if (vari == null)
                    {
                        DebugTrace.print(" POSTSvr urlReq.load vari Null!");
                    }
                    string str = vari as string;

                    DebugTrace.print(" POSTSvr urlReq.loaded str[" + str + "]!");

                    Variant t = JsonManager.StringToVariant(str);

                    if (cb != null)
                        cb(str);
                },
                null,
                null
            );

        }

        public static HttpAppMgr instance;
        public static void init()
        {
            if (instance == null)
            {
                instance = new HttpAppMgr();

            }
        }
    }



}
