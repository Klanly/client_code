//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Cross;
//using GameFramework;
//using MuGame;
//using System.Collections;
//using UnityEngine;
//namespace MuGame
//{
//    class CampaignModel : ModelBase<CampaignModel>
//    {

//        public Dictionary<int, Data> dic = new Dictionary<int, Data>();
//        public Dictionary<int, DataTitle> dictitle = new Dictionary<int, DataTitle>();

//        //public CampaignModel()
//        //    : base()
//        //{
//        //    ReadXML();
//        //    ReadXMLTITLE();
//        //}
//        //void ReadXML()
//        //{
//        //    Data data;
//        //    SXML xml = XMLMgr.instance.GetSXML("achievement.achievement", null);
//        //    do
//        //    {
//        //        data = new Data();
//        //        data.id = xml.getInt("achievement_id");
//        //        data.category = xml.getInt("category");
//        //        data.desc = xml.getString("desc");
//        //        data.type = xml.getInt("type");
//        //        data.paraml = xml.getString("paraml");
//        //        data.gift = xml.getInt("gift");
//        //        data.point = xml.getInt("point");
//        //        dic.Add(data.id, data);
//        //    } while (xml.nextOne());
//        //}
//        //void ReadXMLTITLE()
//        //{
//        //    DataTitle datatitle;
//        //    SXML xml = XMLMgr.instance.GetSXML("achievement.title", null);
//        //    do
//        //    {
//        //        datatitle = new DataTitle();
//        //        datatitle.id = xml.getInt("title_id");
//        //        datatitle.para = xml.getInt("para");
//        //        dictitle.Add(datatitle.id, datatitle);
//        //    } while (xml.nextOne());
//        //}

//        //public AchieveTitleData ReadXmls(int id)
//        //{
//        //    AchieveTitleData achieveTitle = new AchieveTitleData();
//        //    AchieveTitleNatureData achieveTitleNatur = new AchieveTitleNatureData();
//        //    SXML achieveTitleSXML = XMLMgr.instance.GetSXML("achievement.title", "title_id==" + id);
//        //    SXML achieveTitleNatueSXML = achieveTitleSXML.GetNode("nature", "type" + id);
//        //    if (achieveTitleSXML != null)
//        //    {
//        //        //string file = "icon/achieve/" + id.ToString();
//        //        id = achieveTitleSXML.getInt("title_id") /* +图片路径*/;
//        //        achieveTitle.id = id;
//        //        achieveTitle.name = achieveTitleSXML.getString("title_name");
//        //        achieveTitle.para = achieveTitleSXML.getInt("para");
//        //        if (achieveTitleNatueSXML != null)
//        //        {
//        //            achieveTitleNatur.type = achieveTitleNatueSXML.getString("type");
//        //            achieveTitleNatur.value = achieveTitleNatueSXML.getInt("value");
//        //            achieveTitle.titleData = achieveTitleNatur;
//        //        }
//        //    }

//        //    return achieveTitle;
//        //}
//        public int GetMaxBuyTime()
//        {//获得vip最大购买次数
//            int count = 0;
//            SXML vipSXML = XMLMgr.instance.GetSXML("vip.viplevel", "vip_level==" + PlayerModel.getInstance().vip);
//            if (vipSXML != null)
//            {
//                count = vipSXML.getInt("1");
//            }
//            return count;
//        }
//        public struct CampaignData
//        {
//            public string name;
//            public uint limt;
//            public bool isOpen;
//        }
//    }
//}
