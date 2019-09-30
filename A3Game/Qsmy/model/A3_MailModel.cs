using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;

namespace MuGame
{
    public class A3_MailModel : ModelBase<A3_MailModel>
    {
        public Dictionary<uint, A3_MailSimple> mail_simple = new Dictionary<uint, A3_MailSimple>();
        public Dictionary<uint, A3_MailDetail> mail_details = new Dictionary<uint, A3_MailDetail>();

        //!--所有邮件中是否有附件物品未领
        public bool HasItemInMails()
        {
            var etor = mail_simple.GetEnumerator();
            while (etor.MoveNext())
            {
                if (etor.Current.Value.has_itm == true &&
                    etor.Current.Value.got_itm == false)
                    return true;
            }

            return false;
        }

        //!--指定邮件中是否有附件未领取
        public bool HasItemInMail(uint mailid)
        {
            if (!mail_simple.ContainsKey(mailid))
                return false;

            if (mail_simple[mailid].has_itm == true &&
                mail_simple[mailid].got_itm == false)
                return true;

            return false;
        }

    }


    public class A3_MailSimple 
    {
        public uint id;
        public uint tm;
        public string tp;
        public bool got_itm;
        public bool flag;
        public string title;
    
        public bool has_itm;
    }

    public class A3_MailDetail
    {
        public A3_MailSimple ms;
        public string msg;
        public uint money;
        public uint yb;
        public uint bndyb;
        public List<a3_BagItemData> itms;

        public A3_MailDetail()
        {
            itms = new List<a3_BagItemData>();
        }
    }

}
