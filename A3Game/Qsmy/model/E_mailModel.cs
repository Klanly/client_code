using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using System.Collections;
using UnityEngine;
namespace MuGame
{
    class E_mailModel : ModelBase<E_mailModel>
    {
        public static int toCid = 0;

        public List<mailData> familyMailDic = new List<mailData>();
        public List<mailData> ahMailDic = new List<mailData>();
        public List<mailData> systemMailDic = new List<mailData>();
        public List<mailData> gameMailDic = new List<mailData>();
        public Dictionary<int, List<mailData>> personalMailDic = new Dictionary<int, List<mailData>>();

        string[] sep1 = { "#!#&" };
        string[] sep2 = { "#)#&" };
        const int _15days=1296000;
        System.DateTime nowTime = System.DateTime.Now;
        int timeSmp;
        public List<mailData> perLocalStr=new List<mailData>();
        
        public List<mailData> famLocalStr = new List<mailData>();
        public Dictionary<mailData, GiftCardData> giftCardDataDic = new Dictionary<mailData, GiftCardData>();

        bool isInited = false;
        public void init()
        {
            if (isInited)
            {
                return;
            }
            isInited = true;
            if (HttpAppMgr.instance!=null)
            {
                HttpAppMgr.instance.addEventListener(HttpAppMgr.EVENT_GET_GIFT_CARD, getGiftCard);
                E_mailProxy.getInstance();
            }
            timeSmp = muNetCleint.instance.CurServerTimeStamp;
            //timeSmp=ConvertDateTimeInt(nowTime);
            debug.Log(timeSmp.ToString());
            perLocalStr.Clear();
            famLocalStr.Clear();

            //bool b = PlayeLocalInfo.checkKey("aa");
            //if (b)
            //{
            //    string st = PlayeLocalInfo.loadString("aa");

            //}
            //else
            //{
            //    PlayeLocalInfo.saveString("aa", "aaaaaaaaaaaaaaa");
            //}
           
            string perStr="";
            perStr=FileMgr.loadString(FileMgr.TYPE_MAIL, "per");
            string famStr = "";
            famStr = FileMgr.loadString(FileMgr.TYPE_MAIL, "fam");

            if (perStr != "" && perStr != " ")
            {
                string[] msgPer = perStr.Split(sep2, StringSplitOptions.None);

                try
                {
                    foreach (string str in msgPer)
                    {
                        string[] arr = str.Split(sep1, StringSplitOptions.None);
                        debug.Log(arr[0] + 22);
                        //frmcid+frmsex+cid+time+frmname+msg
                        if (arr[0] != ""&&arr[0]!=" ")
                        {
                            if (int.Parse(arr[6]) + _15days > timeSmp)
                            {
                                mailData data = new mailData();
                                data.frmcid = int.Parse(arr[0]);
                                data.frmsex = int.Parse(arr[1]);
                                data.cid = int.Parse(arr[2]);
                                data.time = arr[3];
                                data.frmname = arr[4];
                                data.msg = arr[5];
                                data.str = str + "#)#&";
                                if (E_mailModel.getInstance().personalMailDic.ContainsKey(data.frmcid))
                                    E_mailModel.getInstance().personalMailDic[data.frmcid].Add(data);
                                else
                                {
                                    List<mailData> list = new List<mailData>();
                                    list.Add(data);
                                    E_mailModel.getInstance().personalMailDic.Add(data.frmcid, list);
                                }
                                perLocalStr.Add(data);
                            }
                        }
                    }
                    saveLocalData(perLocalStr, 4);
                }
                catch (System.Exception ex)
                {
                    FileMgr.saveString(FileMgr.TYPE_MAIL, "per", " ");
                    FileMgr.saveString(FileMgr.TYPE_MAIL, "fam", " ");
                }
            }
            if (famStr != ""&& famStr!=" ")
            {
                string[] msgFam = famStr.Split(sep2, StringSplitOptions.None);
                foreach (string str in msgFam)
                {
                    string[] arr = str.Split(sep1, StringSplitOptions.None);
                    if (arr[0] != "" && arr[0] != " ")
                    {
                        if (int.Parse(arr[6]) + _15days > timeSmp)
                        {
                            mailData data = new mailData();
                            //frmcid+frmsex+cid+time+frmname+msg
                            data.frmcid = int.Parse(arr[0]);
                            data.frmsex = int.Parse(arr[1]);
                            data.cid = int.Parse(arr[2]);
                            data.time = arr[3];
                            data.frmname = arr[4];
                            data.msg = arr[5];
                            data.clanc = int.Parse(arr[7]);
                            data.str = str + "#)#&";
                            familyMailDic.Add(data);
                            famLocalStr.Add(data);
                        } 
                    }
                }
                saveLocalData(famLocalStr, 3);
            }
            UIClient.instance.addEventListener(UI_EVENT.ON_LOAD_ITEMS, onEndLoadItem);
        }

        public void saveLocalData(List<mailData> list, int type)
        {
            int num = list.Count-100;
            if (num>0)
            {
                for (int i = 0; i < num;i++ )
                {
                    list.Remove(list[0]);
                }
            }
            string str = "";
            foreach (mailData data in list)
            {
                str = str + data.str;
            }
            switch (type)
            {
                case 3: FileMgr.saveString(FileMgr.TYPE_MAIL, "fam", str); break;
                case 4: FileMgr.saveString(FileMgr.TYPE_MAIL, "per", str); break;
                default:
                    break;
            }
        }

        public void onSendMail(int type,int cid=0,string name=null)
        {
            //家族邮件type=3,私人邮件type=4
            ArrayList data = new ArrayList();
            data.Add(type);
            if (type==4)
            {
                data.Add(cid);
                data.Add(name);
            }
            //InterfaceMgr.getInstance().open(InterfaceMgr.MAILPAPER, data);
        }

        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public void getGiftCard(GameEvent e)
        {
            if (getGiftCardData())
            {
                if (!E_mailProxy.getInstance().isNotice)
                {
                    //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_Mail);
                    //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
                    E_mailProxy.getInstance().isNotice = true;
                }
            }
            //if (my_mail.instance != null)
            //    my_mail.instance.refreshGift();
        }

        public bool getGiftCardData()
        {
            List<GiftCardData> gifdta = HttpAppMgr.instance.getGiftCards();
            giftCardDataDic.Clear();
            bool ishaveCard = false;
            if (gifdta.Count==0)
                return ishaveCard;
            foreach (GiftCardData dta in gifdta)
            {
                if (dta.cardType.functp==4)
                {
                    mailData maildta = new mailData();
                    maildta.type = 1;
                    maildta.code = dta.code;
                    maildta.acttm = dta.cardType.acttm;
                    maildta.money = dta.cardType.money;
                    maildta.yb = dta.cardType.golden;
                    maildta.yinpiao = dta.cardType.yinpiao;
                    maildta.msg = dta.cardType.desc;
                    debug.Log(maildta.msg);
                    if (maildta.msg == "")
                    {
                        maildta.msg = dta.cardType.name;
                        //debug.Log(maildta.msg + "名字");
                    }
                    maildta.seconds = dta.cardType.acttm;
                    maildta.time = E_mailProxy.getInstance().getTime(maildta.seconds.ToString());
                    if (dta.cardType.lItem != null)
                    {
                        foreach (BaseItemData item in dta.cardType.lItem)
                        {
                            mailItemData d = new mailItemData();
                            d.id = int.Parse(item.id);
                            d.count = item.num;
                            d.type = 1;
                            maildta.items.Add(d);
                        }
                    }
                    systemMailDic.Add(maildta);
                    giftCardDataDic.Add(maildta, dta);
                    ishaveCard = true;
                }
            }
            return ishaveCard;
        }

        void onEndLoadItem(GameEvent e)
        {
            //getnew.isOn = true;
        }
    }

    class mailData
    {
        public int seconds;
        public int type;
        public int cid;
        public int id;
        public string time;
        public int frmcid;
        public string frmname;
        public int frmsex;
        public int flag;
        public string msg;
        public int clanc;
        public int money;
        public int yb;
        public int yinpiao;
        public string str;
        public string code = "--";
        public float acttm = 0;
        public List<mailItemData> items=new List<mailItemData>();
        public bool isGet=false;
    }

    class mailItemData
    {
        public int id;
        public int count;
        public int type;
    }
}