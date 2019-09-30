//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MuGame
//{
//    class Rank_a3Model:ModelBase<Rank_a3Model>
//    {
//        public Dictionary<int, rankinfos> dicrankinfo = new Dictionary<int, rankinfos>();
//        public Rank_a3Model()
//        {
//            readxml();
//        }


//        void readxml()
//        {
//            rankinfos rankinfo;
//            SXML xml=XMLMgr.instance.GetSXML("rankname_a3.rank_level",null);
//            if(xml!=null)
//            {
//                do 
//                {
//                    rankinfo=new rankinfos();
//                    rankinfo.level=xml.getInt("level");
//                    rankinfo.name=xml.getString("rank_name");
//                    rankinfo.rankexp=xml.getInt("rank_exp");
//                    rankinfo.rank_attribute=new List<rank_attributes>();
//                    dicrankinfo[rankinfo.level] = rankinfo;
//                    SXML xmls=xml.GetNode("att",null);
//                    if(xmls!=null)
//                    {
//                        do 
//                        {
//                            rank_attributes ta=new rank_attributes();
//                            ta.type=xmls.getInt("att_type");
//                            ta.value=xmls.getInt("att_value");
//                            rankinfo.rank_attribute.Add(ta);
//                        } while (xmls.nextOne());                
//                    }

//                } 
//                while (xml.nextOne());
//            }


            


//        }
//    }

//    class rankinfos
//    {
//        public int level;
//        public string name;
//        public int rankexp;
//        public List<rank_attributes> rank_attribute;
//    }
//    class rank_attributes
//    {
//        public int type;
//        public int value;
//    }


//}
