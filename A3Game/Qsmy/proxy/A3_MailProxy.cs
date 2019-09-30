using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_MailProxy : BaseProxy<A3_MailProxy>
    {
        public static uint MAIL_NEW_MAIL = 0;
        public static uint MAIL_NEW_MAIL_CONTENT = 1;
        public static uint MAIL_GET_ATTACHMENT = 2;
        public static uint MAIL_REMOVE_ONE = 3;
        public static uint MAIL_GET_ALL = 4;
        public static uint MAIL_DELETE_ALL = 5;

        public A3_MailProxy()
        {
            addProxyListener(PKG_NAME.S2C_GET_MAIL_RES, OnMail);
        }

        //!--获取邮件列表
        public void GetMails()
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 1;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        //!--获取一份邮件
        public void GetMailContent(uint mailid)
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 2;
            msg["mailid"] = mailid;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        //!--获取附件
        public void GetMailAttachment(uint mailid)
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 3;
            msg["mailid"] = mailid;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        //!--删除附件
        public void RemoveMail(uint mailid)
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 4;
            msg["mailid"] = mailid;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        //!--一键领取
        public void GetAllAttachment()
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 5;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        //!--一键删除
        public void DeleteAll()
        {
            Variant msg = new Variant();
            msg["mail_cmd"] = 6;
            sendRPC(PKG_NAME.C2S_GET_MAIL, msg);
        }

        public void OnMail(Variant data)
        {
            debug.Log("邮件信息："+data.dump());
            int res = data["res"];
			if (res < 0) Globle.err_output(res);
            switch (res)
            {
                case 1: //!--获取邮件列表
                    OnGetMailList(data);
                    break;

                case 2: //!--获得邮件详细信息
                    OnGetOneMail(data);
                    break;

                case 3: //!--获取附件结果
                    OnGetAttachment(data);
                    break;

                case 4: //!--删除附件
                    OnRemoveMail(data);
                    break;

                case 5: //!--一键领取附件
                    OnGetAll(data);
                    break;

                case 6: //!--一键删除
                    OnDeleteAll(data);
                    break;

                case 7: //!--获得一份新邮件
                    OnNewMail(data);
                    break;
            }
        }

        private void OnGetMailList(Variant data)
        {
            A3_MailModel mm = A3_MailModel.getInstance();
            mm.mail_simple.Clear();

            Variant mails = data["mails"];
            foreach (var mail in mails._arr)
            {
                A3_MailSimple mdata = new A3_MailSimple();
                mdata.id = mail["id"];
                mdata.tm = mail["tm"];
                mdata.tp = ContMgr.getCont("mail_type_" + mail["tp"]);
                mdata.got_itm = mail["got_itm"];
                mdata.flag = mail["flag"];
                mdata.title = ConvertString(mail["title"], "mail_title_");
                mdata.has_itm = mail["has_itm"];
                mm.mail_simple[mdata.id] = mdata;
            }
        }

        private void OnGetOneMail(Variant data)
        {
            A3_MailModel mm = A3_MailModel.getInstance();

            uint mailid = data["id"];
            if (mm.mail_details.ContainsKey(mailid))
            {
                mm.mail_details.Remove(mailid);
            }

            A3_MailDetail mdetail = new A3_MailDetail();
            mdetail.ms = mm.mail_simple[mailid];
            mdetail.ms.id = mailid;
            mdetail.ms.tp = ContMgr.getCont("mail_type_" + data["tp"]);
            mdetail.ms.tm = data["tm"];
            mdetail.ms.got_itm = data["got_itm"];
            mdetail.ms.flag = true;
            mdetail.msg = ConvertString(data["msg"], "mail_content_");
            mdetail.itms = new List<a3_BagItemData>();

            if (data.ContainsKey("itm"))
            {
                Variant itms = data["itm"];
                
                if (itms.ContainsKey("money")) 
                    mdetail.money = itms["money"];
                
                if (itms.ContainsKey("yb")) 
                    mdetail.yb = itms["yb"];
                
                if (itms.ContainsKey("bndyb")) 
                    mdetail.bndyb = itms["bndyb"];
                
                if (itms.ContainsKey("itm"))
                {
                    Variant itmary = itms["itm"];
                    foreach (var itm in itmary._arr)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        itemData.tpid = itm["tpid"];
                        itemData.num = itm["cnt"];
                        itemData.confdata = a3_BagModel.getInstance().getItemDataById(itemData.tpid);
                        mdetail.itms.Add(itemData);
                    }
                }
                if (itms.ContainsKey("eqp"))
                {
                    Variant eqpary = itms["eqp"];
                    foreach (var itm in eqpary._arr)
                    {
                        a3_BagItemData itemData = new a3_BagItemData();
                        if (itm.ContainsKey("tpid")) 
                            itemData.tpid = itm["tpid"];

                        if (itm.ContainsKey("bnd")) 
                            itemData.bnd = itm["bnd"];

                        a3_EquipModel.getInstance().equipData_read(itemData, itm);
                        itemData.confdata = a3_BagModel.getInstance().getItemDataById(itemData.tpid);
                        mdetail.itms.Add(itemData);
                    }
                }
            }
            mm.mail_details[mailid] = mdetail;
            dispatchEvent(GameEvent.Create(MAIL_NEW_MAIL_CONTENT, this, mailid));
        }

        private void OnGetAttachment(Variant data)
        {
            uint id = data["id"];
            A3_MailModel mm = A3_MailModel.getInstance();
            A3_MailSimple ms = mm.mail_simple[id];
            A3_MailDetail md = mm.mail_details[id];
            ms.got_itm = true;
            md.ms.got_itm = true;
            
            dispatchEvent(GameEvent.Create(MAIL_GET_ATTACHMENT, this, id));

			A3_MailProxy.getInstance().RemoveMail(id);
        }

        private void OnRemoveMail(Variant data)
        {
            uint id = data["id"];
            A3_MailModel mm = A3_MailModel.getInstance();
            mm.mail_simple.Remove(id);
            mm.mail_details.Remove(id);

            dispatchEvent(GameEvent.Create(MAIL_REMOVE_ONE, this, id));
        }

        private void OnGetAll(Variant data)
        {
            A3_MailModel mm = A3_MailModel.getInstance();
            Variant get_ids = data["ids"];
            foreach (uint id in get_ids._arr)
            {
                mm.mail_simple[id].got_itm = true;
                if (mm.mail_details.ContainsKey(id))
                {
                    mm.mail_details[id].ms.got_itm = true;
                }
            }
            if (get_ids._arr.Count>0||data["itm"]["itm"]._arr.Count>0)
            {
                flytxt.instance.fly(ContMgr.getCont("mail_hint_7"));
            }
            dispatchEvent(GameEvent.Create(MAIL_GET_ALL, this, null));
        }

        private void OnDeleteAll(Variant data)
        {
            A3_MailModel mm = A3_MailModel.getInstance();
            Variant get_ids = data["ids"];
            foreach (uint id in get_ids._arr)
            {
                if (mm.mail_simple.ContainsKey(id))
                    mm.mail_simple.Remove(id);
                if (mm.mail_details.ContainsKey(id))
                    mm.mail_details[id].ms.got_itm = true;
            }
			dispatchEvent(GameEvent.Create(MAIL_DELETE_ALL, this, data));
        }

        private void OnNewMail(Variant mail)
        {
            A3_MailModel mm = A3_MailModel.getInstance();
            A3_MailSimple mdata = new A3_MailSimple();
            mdata.id = mail["id"];
            mdata.tm = mail["tm"];
            mdata.tp = ContMgr.getCont("mail_type_" + mail["tp"]);
            mdata.got_itm = false;
            mdata.flag = mail["flag"];
            mdata.title = ConvertString(mail["title"], "mail_title_");
            mdata.has_itm = mail["has_itm"];
            if (mm.mail_simple.ContainsKey(mdata.id))
            {
                mm.mail_simple.Remove(mdata.id);
            }
            mm.mail_simple[mdata.id] = mdata;
            dispatchEvent(GameEvent.Create(MAIL_NEW_MAIL, this, mdata));
            a3_mail.expshowid = (int)mdata.id;
        }

        //!--将服务器发送过来的标题和正文内容{id#para0#para1...}转换为客户端文本
        private string ConvertString(string svrStr, string contPrefix)
        {
            string[] split = svrStr.Split('#');
            if (!split.Any())
            {
                return ContMgr.getCont(contPrefix + "0");
            }
            if (split.Length == 1)
            {//没有符号分割
                return svrStr;
            }
            string type = split[0];
            List<string> liststr = new List<string>();
            for (int i = 1; i < split.Count(); i++)
            {
                liststr.Add(split[i]);
            }
            return ContMgr.getCont(contPrefix + type, liststr);
        }
    }
}
