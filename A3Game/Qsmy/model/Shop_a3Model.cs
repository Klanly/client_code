//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MuGame
//{
//    class Shop_a3Model:ModelBase<Shop_a3Model>
//    {
//        public Dictionary<int, iteminfo> itemsdic = new Dictionary<int, iteminfo>();
//        public Shop_a3Model()
//        {
//            Readxml();
//        }
//        void Readxml()
//        {

//            SXML xml = XMLMgr.instance.GetSXML("golden_shopa3.golden_shop", null);
//            do
//            {
//                iteminfo infos = new iteminfo();
//                infos.id = xml.getInt("id");
//                infos.type = xml.getInt("type");
//                infos.itemid = xml.getInt("itemid");
//                infos.itemname = xml.getString("itemname");
//                infos.des = xml.getString("descrip");
//                infos.money_type = xml.getInt("money_type");
//                infos.value = xml.getInt("value");
//                itemsdic[infos.id] = infos;
//            } while (xml.nextOne());

//        }
//    }
//    class iteminfo
//    {
//        public int id;
//        public int type;
//        public int itemid;
//        public string itemname;
//        public string des;
//        public int  money_type;
//        public int value;
//    }
    
//}
