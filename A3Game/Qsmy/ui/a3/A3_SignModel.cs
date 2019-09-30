//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Cross;

//namespace MuGame
//{
//    class A3_SignModel:ModelBase<A3_SignModel>
//    {
//        public Dictionary<int, signinfo> dic = new Dictionary<int, signinfo>();
//        public Dictionary<int, signtotal> dics = new Dictionary<int, signtotal>();
//        public A3_SignModel()
//        {
//            ReadXml();
//        }
        //void ReadXml()
        //{            
        //    List<SXML> xml = XMLMgr.instance.GetSXMLList("signup_a3.signup");
        //    if (xml != null)
        //    {
        //        foreach (SXML x in xml)
        //        {
        //            signinfo data= new signinfo();
        //            data.signup_times = x.getInt("signup_times");
        //            data.gemtype = x.getInt("gemtype");
        //            data.gemnum = x.getInt("gemnum");
        //            dic[data.signup_times] = data;
        //        }
        //    }
        //    List<SXML> xmls = XMLMgr.instance.GetSXMLList("signup_a3.total");
        //    if (xmls != null)
        //    {
        //        foreach (SXML x in xmls)
        //        {
        //            signtotal datas = new signtotal();
        //            datas.total = x.getInt("total");
        //            dics[datas.total] = datas;
        //        }
        //    }
        //}


        //public void  refresh_sign(int thisday,int qd_day)
        //{
        //    foreach (int i in dic.Keys)
        //    {
        //        if (i == qd_day)
        //        {
        //            dic[i].issign = true;
        //        }
        //        if (i < thisday && i != qd_day)
        //        {
        //            dic[i].isrepairsign = true;
        //        }
        //    }
        //}
        //public void  refresh_total(int count_type)
        //{
        //    foreach (int i in dics.Keys)
        //    {
        //        if (i==count_type)
        //        {
        //            dics[i].isopen = true;
        //        }
        //    }
        //}

        




//    }

//    class signinfo
//    {
//        public int signup_times;
//        //public bool issign = false;//是不是签过了
//        //public bool isrepairsign = false;//是不是补签 
//        public int gemtype;
//        public int gemnum;
//    }
//    class signtotal
//    {
//        public int total;
//        public bool isopen = false;//有没有领取
//    }

//}
