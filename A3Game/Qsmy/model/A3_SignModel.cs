//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MuGame
//{
//    class A3_SignModel:ModelBase<A3_SignModel>
//    {
//        public Dictionary<int, signinfo> dic = new Dictionary<int, signinfo>();
//        public Dictionary<int, signtotal> dics = new Dictionary<int, signtotal>();
//        public A3_SignModel()
//        {
//            ReadXml();
//            ReadXmls();
//        }
//        void ReadXml()
//        {
//            signinfo data;
//            SXML xml = XMLMgr.instance.GetSXML("sign_a3.signup", null);
//            if (xml != null)
//            {
//                do
//                {
//                    data = new signinfo();
//                    data.signup_times = xml.getInt("signup_times");
//                    data.gemtype = xml.getInt("gemtype");
//                    data.gemnum = xml.getInt("gemnum");
//                    data.items = new List<item_info>();
//                    dic[data.signup_times] = data;
//                    SXML sxml = xml.GetNode("item", null);
//                    if (sxml != null)
//                    {
//                        do
//                        {
//                            item_info datas = new item_info();
//                            datas.item_id = sxml.getInt("item_id");
//                            datas.num = sxml.getInt("num");
//                            data.items.Add(datas);
//                        } while (sxml.nextOne());
//                    }

//                } while (xml.nextOne());

//            }
//        }
//        void ReadXmls()
//        {

//            signtotal datas;
//            SXML xmls = XMLMgr.instance.GetSXML("sign_a3.total", null);
//            if (xmls != null)
//            {
//                do
//                {
//                    datas = new signtotal();
//                    datas.total = xmls.getInt("total");
//                    datas.itemss = new List<item_info>();
//                    dics[datas.total] = datas;
//                    SXML sxml = xmls.GetNode("item", null);
//                    if (sxml != null)
//                    {
//                        do
//                        {
//                            item_info datass= new item_info();
//                            datass.item_id = sxml.getInt("item_id");
//                            datass.num = sxml.getInt("num");
//                            datas.itemss.Add(datass);
//                        } while (sxml.nextOne());
//                    }

//                } while (xmls.nextOne());

//            }
//        }
//    }

//    class signinfo
//    {
//        public int signup_times;
//        public int gemtype;
//        public int gemnum;
//        public List<item_info> items;
//    }
//    class item_info
//    {
//        public int item_id;
//        public int num;

//    }
//    class signtotal
//    {
//        public int total;
//        public List<item_info> itemss;
//    }

//}
