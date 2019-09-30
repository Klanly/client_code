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
//    class Skill_a3Model:ModelBase<Skill_a3Model>
//    {
//        public Dictionary<int, skill_a3Data> skilldic = new Dictionary<int, skill_a3Data>();
//        public Skill_a3Model()
//            :base()
//        {
//            ReadXml();
//        }


//        void ReadXml()
//        {
//            skill_a3Data data;
//            SXML xml = XMLMgr.instance.GetSXML("skill_a3.skill", null);
//            if (xml != null)
//            {
//                do
//                {
//                    data = new skill_a3Data();
//                    data.skill_id = xml.getInt("skill_id");
//                    data.skill_name = xml.getString("skill_name");
//                    data.skill_openlv = xml.getInt("skill_openlv");
//                    data.skill_lv = new List<skill_a3lv>();
//                    skilldic[data.skill_id] = data;  
//                    SXML sxml = xml.GetNode("Level", null);
//                    if (sxml != null)
//                    {
//                        do
//                        {
//                            skill_a3lv datas = new skill_a3lv();
//                            datas.skill_lv = sxml.getInt("skill_level");
//                            datas.skill_des = sxml.getString("skill_descrip");
//                            datas.needmoney = sxml.getInt("skill_costmoney");
//                            datas.needpoint = sxml.getInt("skill_costpoint");
//                            data.skill_lv.Add(datas);
//                        } while (sxml.nextOne());
//                    }
                
//                }while (xml.nextOne());

//            }
//        }
//    }

//    class skill_a3Data
//    {
//        public int skill_id;
//        public string skill_name;
//        public int skill_openlv;
//        public List<skill_a3lv> skill_lv;
//    }
//    class skill_a3lv
//    {
//        public int skill_lv;
//        public string  skill_des;
//        public int needmoney;
//        public int needpoint;
//    }
//}
