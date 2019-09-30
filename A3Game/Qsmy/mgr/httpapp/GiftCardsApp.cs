using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
using System.Collections;

namespace MuGame
{
    public class GiftCardsApp
    {
        public List<GiftCardData> lGiftCards = new List<GiftCardData>();
        public Dictionary<int, GiftCardData> dGiftCardData = new Dictionary<int, GiftCardData>();
        private List<GiftCardType> lType = new List<GiftCardType>();
        private Dictionary<int, GiftCardType> dTp = new Dictionary<int, GiftCardType>();
        private List<GiftCardType> cacheGetCode = new List<GiftCardType>();

        public GiftCardsApp()
        {
            //string path = "sid=" + Globle.curServerD.sid + "&uid=" + PlayerModel.getInstance().uid + "&cust=";
            //HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?paycard", path + 4, onHttpCallback);
            //debug.Log("::::::::::::::::::" + Globle.curServerD.do_url + "?paycard" + path + 1);
            //return;

            //if (PlayerModel.getInstance().lvl > 3)
            //{
            getRechangeCard();
                GiftCardProxy.getInstance().sendLoadItemCardInfo();
            //}

        }

        public GiftCardType getTp(int tp)
        {
            if (!dTp.ContainsKey(tp))
                return null;
            return dTp[tp];
        }

        public void getRechangeCard()
        {
            if (PlayerModel.getInstance().lvl < 10)
                return;

            string path = "sid=" + Globle.curServerD.sid + "&uid=" + PlayerModel.getInstance().uid + "&cid=" + PlayerModel.getInstance().cid + "&cust=2";
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?paycard", path, onRechageHandle);
        }
        public RechangeTaskData rechangeTaskData;
        public void onRechageHandle(string str)
        {
            debug.Log("收到充值礼包：" + str);
            if (str == "")
                return;



            //  Variant v = JsonManager.StringToVariant(str);
            JSONNode v = JSON.Parse(str);
            if (rechangeTaskData == null)
                rechangeTaskData = new RechangeTaskData();

            rechangeTaskData.init(v);

        }



        public void getFirstRechangeCard()
        {
            string path = "sid=" + Globle.curServerD.sid + "&uid=" + PlayerModel.getInstance().uid + "&cid=" + PlayerModel.getInstance().cid + "&tp=2";
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?card", path, onFirstRechange);
        }
        public void onFirstRechange(string str)
        {
            debug.Log("受宠礼包信息:" + str);
        }

        private float beginLoadingTimer = 0f;
        public void getAllCode()
        {

            if (cacheGetCode.Count > 0) return;
            if (lType.Count == 0) return;
            dGiftCardData.Clear();
            lGiftCards.Clear();
            foreach (GiftCardType type in lType)
            {
                cacheGetCode.Add(type);
            }

            beginLoadingTimer = Time.time;
            getCardsCode();

        }


        private GiftCardType curTransing;
        private void getCardsCode()
        {
            debug.Log("请求获得新卡:" + cacheGetCode.Count + "   " + curTransing);
            if (cacheGetCode.Count == 0)
            {
                foreach (GiftCardData d in lGiftCards)
                {
                    debug.Log("判断有新:" + beginLoadingTimer + d.creattimer);
                    if (beginLoadingTimer <= d.creattimer)
                    {
                        HttpAppMgr.instance.onGetnewCard();
                        if (d.cardType.functp == 4)
                        {
                            ArrayList arr = new ArrayList();
                            arr.Add(d);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_GIFTCARD, arr);
                        }
                        return;
                    }
                }
                return;
            }
            if (curTransing != null) return;

            curTransing = cacheGetCode[0];
            cacheGetCode.RemoveAt(0);


            string path = "sid=" + Globle.curServerD.sid + "&uid=" + PlayerModel.getInstance().uid + "&cid=" + PlayerModel.getInstance().cid + "&tp=";
            debug.Log("请求激活码::::::::::::::::::" + Globle.curServerD.do_url + "?card" + path + curTransing.id);
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?card", path + curTransing.id, onHttpCallback);

        }

        public void getShortCardsCode(string code)
        {
            string path = "sid=" + Globle.curServerD.sid + "&cid=" + PlayerModel.getInstance().cid + "&shortcode=";
            debug.Log("请求激活码::::::::::::::::::" + Globle.curServerD.do_url + "?card=2" + path + code);
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?card=2", path + code, onHttpShortCars);
        }

        public void getSevenCardsCode(string code)
        {
            string path = "sid=" + Globle.curServerD.sid + "&cid=" + PlayerModel.getInstance().cid + "&shortcode=";
            debug.Log("请求激活码::::::::::::::::::" + Globle.curServerD.do_url + "?receive_card=2" + path + code);
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?receive_card=2", path + code, onHttpSevenCards);
        }

        void onHttpSevenCards(string str)
        {
            if (str == "")
            {
                return;
            }
            Variant v = JsonManager.StringToVariant(str);
            if (v["r"] == 1)
            {
                debug.Log("领取成功");
            }
            else
            {
                Globle.err_output(v["r"]);
                debug.Log("激活码领取的错误码：" + v["r"]);
            }
        }

        void onHttpShortCars(string str)
        {
            if (str == "")
            {
                return;
            }
            Variant v = JsonManager.StringToVariant(str);
            if (v["r"] == 1)
            {
                debug.Log("获得激活码：" + v["res"] + " " + str);

                string id = v["res"];
                HttpAppMgr.instance.sendInputGiftCode(id);
            }
            else
            {
                Globle.err_output(v["r"]);
                debug.Log("激活码领取的错误码：" + v["r"]);
            }
        }

        public void onFirstRechangebbb(string str)
        {
            debug.Log("受宠礼包信息:" + str);
        }

        public void addGiftType(GiftCardType type)
        {
            if (dTp.ContainsKey(type.id))
                lType.Remove(dTp[type.id]);

            lType.Add(type);
            dTp[type.id] = type;
        }

        void onHttpCallback(string str)
        {
            if (curTransing == null)
            {
                curTransing = null;
                getCardsCode();
                return;
            }


            if (str == "")
            {
                curTransing = null;
                getCardsCode();
                return;
            }



            Variant v = JsonManager.StringToVariant(str);
            if (v["r"] == 1)
            {
                debug.Log("获得激活码：" + curTransing.id + " " + str);
                GiftCardData cardta = new GiftCardData();
                cardta.id = curTransing.id;
                cardta.code = v["res"];
                cardta.cardType = curTransing;
                cardta.initTimer();
                if (dGiftCardData.ContainsKey(cardta.id))
                {
                    GiftCardData temp = dGiftCardData[cardta.id];
                    lGiftCards.Remove(temp);
                }

                dGiftCardData[cardta.id] = cardta;

                lGiftCards.Add(cardta);

                debug.Log("lGiftCards.clount::" + lGiftCards.Count);
            }
            else
            {
                Globle.err_output(v["r"]);
                debug.Log("激活码领取的错误码：" + v["r"]);
            }
            curTransing = null;
            getCardsCode();
        }
    }




    public class GiftCardType
    {
        public int id;
        public int functp;
        public int acttm;
        public int endTm;
        public string name;
        public int golden = 0;
        public int money = 0;
        public int yinpiao = 0;
        public string desc = "";
        public List<BaseItemData> lItem;



        public void init(Variant d)
        {
            debug.Log("初始化激活码类型::" + d["tp"] + "  " + d["functp"]);

            if (d.ContainsKey("crttm"))
            {
                acttm = d["crttm"];
                debug.Log("crttm::" + d["crttm"]);
            }

            id = d["tp"];
            functp = d["functp"];
            endTm = d["fintm"];
            name = d["name"];
            if (d.ContainsKey("golden"))
                money = d["golden"];
            if (d.ContainsKey("yb"))
                golden = d["yb"];
            if (d.ContainsKey("bndyb"))
                yinpiao = d["bndyb"];

            if (d.ContainsKey("desc"))
                desc = d["desc"];

            if (d.ContainsKey("itm"))
            {
                lItem = new List<BaseItemData>();
                List<Variant> lItemVa = d["itm"]._arr;
                for (int i = 0; i < lItemVa.Count; i++)
                {
                    BaseItemData itemdata = new BaseItemData();
                    itemdata.id = lItemVa[i]["id"];
                    itemdata.num = lItemVa[i]["cnt"];
                    lItem.Add(itemdata);
                }
            }

        }

    }

    public class GiftCardData
    {
        public int id;
        public string code;
        public GiftCardType cardType;
        public float creattimer = 0f;

        public void initTimer()
        {
            if (creattimer == 0f)
                creattimer = Time.time;
        }

        public void getItems()
        {
            GiftCardProxy.getInstance().sendFetchCard(code);
        }

        //public bool checkOverTime()
        //{
        //    return
        //}
    }


    public class RechangeTaskData
    {
        public int moneyPayed = 0;
        private bool sended = false;
        public void init(JSONNode d)
        {
            if (d["tcyb_day"] != null)
            {
                moneyPayed = d["tcyb_day"].AsInt / 10;
                debug.Log("当日充值：" + moneyPayed);
            }

            if (sended)
                return;

            if (d["1000"] == null)
                return;

            d = d["1000"];
            if (d["cards"] != null)
            {
                debug.Log(" cards ：" + d["cards"]);
                JSONArray arr = d["cards"].AsArray;
                foreach (JSONNode cardStr in arr)
                {
                    GiftCardProxy.getInstance().sendFetchCard(cardStr.ToString());
                    sended = true;
                }
            }
        }
    }
}
