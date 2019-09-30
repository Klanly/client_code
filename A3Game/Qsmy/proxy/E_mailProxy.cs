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
    class E_mailProxy : BaseProxy<E_mailProxy>
    {
        const uint C2S_READ_MAIL = 153;
        const uint c2s_get_mail_item = 154;
        const uint c2s_get_mail_list = 152;
        const uint c2s_send_mail = 155;
        const uint c2s_send_family_mail = 227;
        const uint c2s_delete_mail = 158;
        const uint c2s_fetch_itm_card = 22;

        public static uint lis_sendMail_res = 155;
        public static uint lis_unreadType = 153;
        public static uint lis_deleteMail_res = 158;
        public static uint lis_get_new_mail = 154;
        public static uint lis_get_item = 157;
        public List<int> newType = new List<int>();

        private bool isApplied=false;
        public bool isNotice = false;
        public E_mailProxy()
        {
        //public const uint S2C_GET_MAIL_RES = 152; //邮件列表
        //public const uint S2C_GET_MAIL = 153;     //
        //public const uint S2C_GOT_NEW_MAIL = 154; //获得新邮件
        //public const uint S2C_SEND_MAIL_RES = 155;//发送邮件结果
        //public const uint S2C_LOCK_MAIL_RES = 156;//锁定邮件结果
        //public const uint S2C_GET_MAIL_ITEM_RES = 157;//获得邮件附件
        //public const uint S2C_DEL_MAIL_RES = 158; //删除邮件
            addProxyListener(PKG_NAME.S2C_GET_MAIL_RES, onloadE_mail);
            addProxyListener(PKG_NAME.S2C_DEL_MAIL_RES, replayOnDelete);
            addProxyListener(PKG_NAME.S2C_GOT_NEW_MAIL, getNewMail);
            addProxyListener(PKG_NAME.S2C_SEND_MAIL_RES, replayOnSend);
            addProxyListener(PKG_NAME.S2C_GET_MAIL, getUnreadType);
            addProxyListener(PKG_NAME.S2C_GET_MAIL_ITEM_RES, replayOnGetItem);
            sendLoadmail();
        }

        public void sendLoadmail()
        {
            debug.Log("发送152消息");
            
            sendRPC(c2s_get_mail_list, null);
        }

        public void readMail(uint type)
        {
            debug.Log("发送153消息");
            Variant msg = new Variant();
            msg["tp"] = type;
            sendRPC(C2S_READ_MAIL, msg);
        }

        public void getItem(uint id)
        {
            debug.Log("发送154消息");
            Variant msg = new Variant();
            msg["mailid"] = id;
            sendRPC(c2s_get_mail_item, msg);
        }

        public void fetch_itm_card(int cid)
        {
            Variant msg = new Variant();
            msg["cardid"] = cid.ToString();
            sendRPC(c2s_fetch_itm_card, msg);
        }

        public void sendNewMail(int type,string msg,uint cid=0)
        {
            debug.Log("发送邮件155");
            Variant _msg = new Variant();
            if (type == 4)
            {
                _msg["tp"] = type;
                _msg["tocid"] = cid;
                _msg["msg"] = msg;
                sendRPC(c2s_send_mail, _msg);
            }
            if (type==2||type==1)
            {
                _msg["send_tp"] = type;
                _msg["msg"] = msg;
                sendRPC(c2s_send_family_mail, _msg);
            }  
        }
       
        public void deleteMail(uint id)
        {
            debug.Log("发送158消息");
            Variant msg = new Variant();
            msg["mailid"] = id;
            sendRPC(c2s_delete_mail, msg);
        }

        void loadMailItems(Variant mail,mailData dta)
        {
            if (mail.ContainsKey("itm"))
            {
                if (mail["itm"].ContainsKey("money"))
                    dta.money = mail["itm"]["money"];
                if (mail["itm"].ContainsKey("yb"))
                    dta.yb = mail["itm"]["yb"];
                if (mail["itm"].ContainsKey("itm"))
                {
                    if (mail["itm"]["itm"]._arr.Count != 0)
                    {
                        foreach (Variant item in mail["itm"]["itm"]._arr)
                        {
                            mailItemData md = new mailItemData();
                            md.id = item["item_id"];
                            md.count = item["cnt"];
                            md.type = 1;
                            dta.items.Add(md);
                        }
                    }
                }

                if (mail["itm"].ContainsKey("dress"))
                {
                    if (mail["itm"]["dress"]._arr.Count != 0)
                    {
                        foreach (Variant item in mail["itm"]["dress"]._arr)
                        {
                            mailItemData md = new mailItemData();
                            md.id = item["id"];
                            md.count = item["cnt"];
                            md.type = 2;
                            dta.items.Add(md);
                        }
                    }
                }
            }
        }
        //S2C
        void onloadE_mail(Variant data)
        {
            isApplied = true;
            debug.Log("获取邮件列表");
            debug.Log(data.dump());
            
            foreach (Variant mail in data["mails"]._arr)
            {
                mailData dta = new mailData();
                dta.type = mail["tp"];
                dta.id = mail["id"];
                dta.time =getTime(mail["tm"]);
                dta.seconds = mail["tm"]._int32;
                dta.flag = mail["flag"];
                dta.msg = KeyWord.filter(mail["msg"]);
                loadMailItems(mail, dta);
                switch (dta.type)
                {
                    case 1: 
                        E_mailModel.getInstance().systemMailDic.Add(dta);
                        break;
                    case 2:
                        E_mailModel.getInstance().gameMailDic.Add(dta); 
                        break;
                    case 3:
                        if(mail["frmpinfo"].ContainsKey("cid"))
                            dta.frmcid = mail["frmpinfo"]["cid"];
                        if (mail["frmpinfo"].ContainsKey("name"))
                            dta.frmname = mail["frmpinfo"]["name"];
                        if (mail["frmpinfo"].ContainsKey("sex"))
                            dta.frmsex = mail["frmpinfo"]["sex"];
                        if (mail["frmpinfo"].ContainsKey("clanc"))
                            dta.clanc = mail["frmpinfo"]["clanc"];
                        E_mailModel.getInstance().familyMailDic.Add(dta);

                        bool isContain3 = false;
                        foreach (int type in newType)
                        {
                            if (type == 3)
                            {
                                isContain3 = true;
                                break;
                            }
                                
                        }
                        if (!isContain3)
                            newType.Add(3);

                        string FamMsg = dta.frmcid.ToString() + "#!#&" + dta.frmsex.ToString() + "#!#&" + dta.cid.ToString()
                            + "#!#&" + dta.time + "#!#&" + dta.frmname + "#!#&" + dta.msg + "#!#&" + dta.seconds + "#!#&" + dta.clanc + "#)#&";
                        dta.str = FamMsg;
                        E_mailModel.getInstance().famLocalStr.Add(dta);
                        //FileMgr.saveString(FileMgr.TYPE_MAIL, "fam", FileMgr.loadString(FileMgr.TYPE_MAIL, "fam") + FamMsg);
                        break;
                    case 4:
                        if (mail["frmpinfo"].ContainsKey("cid"))
                            dta.frmcid = mail["frmpinfo"]["cid"];
                        if (mail["frmpinfo"].ContainsKey("name"))
                            dta.frmname = mail["frmpinfo"]["name"];
                        if (mail["frmpinfo"].ContainsKey("sex"))
                            dta.frmsex = mail["frmpinfo"]["sex"];
                        if (mail["frmpinfo"].ContainsKey("clanc"))
                            dta.clanc = mail["frmpinfo"]["clanc"];
                        if (E_mailModel.getInstance().personalMailDic.ContainsKey(dta.frmcid))
                            E_mailModel.getInstance().personalMailDic[dta.frmcid].Add(dta);
                        else
                        {
                            List<mailData> d = new List<mailData>();
                            d.Add(dta);
                            E_mailModel.getInstance().personalMailDic.Add(dta.frmcid, d);
                            dispatchEvent(GameEvent.Create(lis_get_new_mail, this, null));
                        }

                        bool isContain4 = false;
                        foreach (int type in newType)
                        {
                            if (type == dta.frmcid)
                            {
                                isContain4 = true;
                                break;
                            }
                                
                        }
                        if (!isContain4)
                            newType.Add(dta.frmcid);

                        string PerMsg =dta.frmcid.ToString() + "#!#&" + dta.frmsex.ToString() + "#!#&" + dta.cid.ToString()
                            + "#!#&" + dta.time + "#!#&" + dta.frmname + "#!#&" + dta.msg + "#!#&" + dta.seconds + "#!#&" + dta.clanc + "#)#&";
                        dta.str = PerMsg;
                        E_mailModel.getInstance().perLocalStr.Add(dta);
                        //FileMgr.saveString(FileMgr.TYPE_MAIL, "per", FileMgr.loadString(FileMgr.TYPE_MAIL, "per") + PerMsg);
                        break;
                    //case 5: E_mailModel.getInstance().ahMailDic.Add(dta); break;
                    default:
                        break;
                }
            }
            E_mailModel.getInstance().saveLocalData(E_mailModel.getInstance().famLocalStr, 3);
            E_mailModel.getInstance().saveLocalData(E_mailModel.getInstance().perLocalStr, 4);
        }

        void getNewMail(Variant data)
        {
            if (!isApplied)
                return;
            //if (data["mail"]["tp"]==4)
            //{
            //    if (data["mail"]["frmpinfo"]["cid"] == PlayerModel.getInstance().cid)
            //        return;
            //}
            debug.Log("获取新邮件");
            debug.Log(data.dump());
            Variant typeData = new Variant();
            typeData["tp"] = data["mail"]["tp"];
            if (typeData["tp"]==4)
                typeData["secondType"] = data["mail"]["frmpinfo"]["cid"];
            dispatchEvent(GameEvent.Create(lis_unreadType, this, typeData));


            mailData dta = new mailData();
            dta.type = data["mail"]["tp"];
            dta.id = data["mail"]["id"];
            dta.time = getTime(data["mail"]["tm"]);
            dta.seconds = data["mail"]["tm"]._int32;
            dta.msg = KeyWord.filter(data["mail"]["msg"]);
            if (data["mail"]["frmpinfo"].ContainsKey("cid"))
                dta.frmcid = data["mail"]["frmpinfo"]["cid"];
            if (data["mail"]["frmpinfo"].ContainsKey("name"))
            {
                dta.frmname = data["mail"]["frmpinfo"]["name"];
                if (dta.type == 3 && dta.frmcid == PlayerModel.getInstance().cid)
                    dispatchEvent(GameEvent.Create(lis_sendMail_res, this, typeData));
            }   
            if (data["mail"]["frmpinfo"].ContainsKey("sex"))
                dta.frmsex = data["mail"]["frmpinfo"]["sex"];
            if (data["mail"]["frmpinfo"].ContainsKey("clanc"))
                dta.clanc = data["mail"]["frmpinfo"]["clanc"];
            loadMailItems(data["mail"], dta);
            switch (dta.type)
            {
                case 1: 

                    E_mailModel.getInstance().systemMailDic.Add(dta);
                    if (!isNotice)
                    {
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_Mail);
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
                        isNotice = true;
                    }
                    break;
                case 2: 
                    bool isContain2 = false;
                    foreach (int type in newType)
                    {
                        if (type == 2)
                        {
                            isContain2 = true;
                            break;
                        }       
                    }
                    if (!isContain2)
                        newType.Add(2);
                    E_mailModel.getInstance().gameMailDic.Add(dta); 
                    if (!isNotice)
                    {
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_Mail);
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
                        isNotice = true;
                    }
                    break;   
                case 3:
                    E_mailModel.getInstance().familyMailDic.Add(dta); 
                    string FamMsg =dta.frmcid.ToString() + "#!#&" + dta.frmsex.ToString() + "#!#&" + dta.cid.ToString()
                            + "#!#&" + dta.time + "#!#&" + dta.frmname + "#!#&" + dta.msg + "#!#&" + dta.seconds + "#!#&" + dta.clanc + "#)#&";
                    dta.str = FamMsg;
                    E_mailModel.getInstance().famLocalStr.Add(dta);
                    E_mailModel.getInstance().saveLocalData(E_mailModel.getInstance().famLocalStr, 3);
                    bool isContain3 = false;
                    foreach (int type in newType)
                    {
                        if (type == 3)
                        {
                            isContain3 = true;
                            break;
                        }
                                
                    }
                    if (!isContain3)
                        newType.Add(3);

                    if (!isNotice)
                    {
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_Mail);
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
                        isNotice = true;
                    }

                    //FileMgr.saveString(FileMgr.TYPE_MAIL, "fam", FileMgr.loadString(FileMgr.TYPE_MAIL, "fam") + FamMsg);
                    break;
                case 4:

                    if (E_mailModel.getInstance().personalMailDic.ContainsKey(dta.frmcid))
                        E_mailModel.getInstance().personalMailDic[dta.frmcid].Add(dta);
                    else
                    {
                        List<mailData> d = new List<mailData>();
                        d.Add(dta);
                        E_mailModel.getInstance().personalMailDic.Add(dta.frmcid, d);
                        dispatchEvent(GameEvent.Create(lis_get_new_mail, this, null));
                    }

                    bool isContain4 = false;
                    foreach (int type in newType)
                    {
                        if (type == dta.frmcid)
                        {
                            isContain4 = true;
                            break;
                        }          
                    }
                    if (!isContain4)
                        newType.Add(dta.frmcid);
                    if (!isNotice)
                    {
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_Mail);
                        //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_MAIN);
                        isNotice = true;
                    }

                    string PerMsg = dta.frmcid.ToString() + "#!#&" + dta.frmsex.ToString() + "#!#&" + dta.cid.ToString()
                            + "#!#&" + dta.time + "#!#&" + dta.frmname + "#!#&" + dta.msg + dta.seconds + "#!#&" + dta.clanc + "#)#&";
                    dta.str = PerMsg;
                    E_mailModel.getInstance().perLocalStr.Add(dta);
                    E_mailModel.getInstance().saveLocalData(E_mailModel.getInstance().perLocalStr, 4);
                    //FileMgr.saveString(FileMgr.TYPE_MAIL, "per", FileMgr.loadString(FileMgr.TYPE_MAIL, "per") + PerMsg);
                        
                    break;
                //case 5: E_mailModel.getInstance().ahMailDic.Add(dta); break;
                default:
                    break;
            }
        }

        void getUnreadType(Variant data)
        {
            debug.Log("收到未读邮件++++++++++++++++++" + data.dump());
            foreach (Variant newMail in data["new_mail"]._arr)
            {
                int _type = newMail["tp"];
                int _cnt=newMail["cnt"];
                if (_cnt!=0)
                {
                    isNotice = true;
                    if (_type!=4)
                    {
                        newType.Add(_type);
                    }
                }
            }
        }

        void replayOnSend(Variant data)
        {
            debug.Log("收到发送结果"+data.dump());
            dispatchEvent(GameEvent.Create(lis_sendMail_res, this, data));
        }

        void replayOnDelete(Variant data)
        {
            debug.Log("收到删除结果" + data.dump());
            dispatchEvent(GameEvent.Create(lis_deleteMail_res, this, data));
        }

        void replayOnGetItem(Variant data)
        {
            debug.Log("收取物品结果" + data.dump());
            dispatchEvent(GameEvent.Create(lis_get_item, this, data));
        }

        public  string getTime(string _time)
        {
            string timeStamp = _time;
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);

            string date = dtResult.ToShortDateString().ToString();
            string time = dtResult.ToLongTimeString().ToString();

            string[] date_arr = date.Split('/');
            string[] time_arr = time.Split(new char[]{':',' '});




            string result="";

            if (time_arr.Length == 3)
            {
                //result = date_arr[2] + "年" + date_arr[0] + "月" + date_arr[1] + "日" + " " + time_arr[0] + "时" + time_arr[1] + "分";
                result = ContMgr.getCont("comm_date1", new List<string> { date_arr[2].ToString(), date_arr[0].ToString(), date_arr[1].ToString(), time_arr[0].ToString(), time_arr[1].ToString() });
            }
            else if (time_arr.Length == 4)
            {
                if (time_arr[3] == "PM")
                    result = ContMgr.getCont("comm_date1", new List<string> { date_arr[2].ToString(), date_arr[0].ToString(), date_arr[1].ToString(), (int.Parse(time_arr[0]) + 12).ToString(), time_arr[1].ToString() });
                //result = date_arr[2] + "年" + date_arr[0] + "月" + date_arr[1] + "日" + " " + (int.Parse(time_arr[0]) + 12).ToString() + "时" + time_arr[1] + "分";
                else
                    result = ContMgr.getCont("comm_date1", new List<string> { date_arr[2].ToString(), date_arr[0].ToString(), date_arr[1].ToString(), time_arr[0].ToString(), time_arr[1].ToString() });
                //result = date_arr[2] + "年" + date_arr[0] + "月" + date_arr[1] + "日" + " " + time_arr[0] + "时" + time_arr[1] + "分";
            }
          
            return result;
        }

    }
}
