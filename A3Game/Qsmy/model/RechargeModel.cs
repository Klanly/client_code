using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class RechargeModel : ModelBase<RechargeModel>
    {
        public RechargeModel()
            : base()
        {
            init();
        }
        public Dictionary<int, rechargeData> rechargeMenu;
        public List<int> firsted = new List<int>();

        public rechargeData getRechargeDataById(int id)
        {
            if (rechargeMenu == null)
                init();
            if (rechargeMenu.ContainsKey(id))
                return rechargeMenu[id];
            return null;
        }

        public void init()
        {
            rechargeMenu = new Dictionary<int, rechargeData>();

            SXML xml;

            if (Globle.QSMY_SDK_CHILD == ENUM_SDK_CHILD.none)
            {
                xml = XMLMgr.instance.GetSXML("recharge");
            }
            else
            {
                xml = XMLMgr.instance.GetSXML("recharge_" + Globle.QSMY_SDK_CHILD.ToString());
            }

            List<SXML> rechargeSXML = xml.GetNodeList("recharge");
            if (rechargeSXML == null)
            {
                return;
            }
            foreach (SXML x in rechargeSXML) 
            {
                rechargeData data = new rechargeData();
                data.id = x.getInt("id");
                data.name = x.getString("name");
                data.golden = x.getString("golden");
                data.golden_value = x.getString("golden_value");
                data.days = x.getString("days");
                data.daynum = x.getString("daynum");
                data.first_double = x.getInt("first_double");
                data.desc = x.getString("desc");
                data.desc1 = x.getString("desc1");
                data.desc2 = x.getString("desc2");
                data.payid = x.getString("payid");
                data.pay_android_id = x.getString("android_id");
                data.pay_ios_id = x.getString("ios_id");
                data.ka = x.getInt("ka");
                rechargeMenu[data.id] = data;
            }
        }

    }

    public class rechargeData
    {
        public int id;
        public string name;
        public string golden;
        public string golden_value;
        public string days;
        public string daynum;
        public int first_double;
        public string desc;
        public string desc1;
        public string desc2;
        public string pay_android_id;
        public string pay_ios_id;
        public bool isFirst;
        public string payid;
        public int ka;
    }
}
