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
//    class MJJDModel : ModelBase<MJJDModel>
//    {
//        public int use_time;//s使用次数
//        public int left_time;
//        public int max_time = 3;//最大挑战次数 算上vip购买
//        public int cur_mxa_time;
//        public List<MJJDData> mjjdList = new List<MJJDData>();
//        public List<CompaignData> compaignList = new List<CompaignData>();
//        public MJJDModel()
//            : base()
//        {

//        }
//        public void GetMJJDTimes(Variant data)
//        {//data["mjjd_times"]
//            left_time = GetMaxBuyTime() - data["mjjd_buy_times"];
//            use_time = (1 - (int)data["mjjd_times"]) + data["mjjd_buy_times"];
//            cur_mxa_time = 1 + GetMaxBuyTime();
//            if (mjjd.instance != null)
//            {
//                mjjd.instance.Refresh();
//            }
//        }
//        public void GetBuyRefresh(Variant data)
//        {
//            left_time = GetMaxBuyTime() - data["mjjd_buy_times"];
//            use_time =(1- (int)data["mjjd_times"])+ data["mjjd_buy_times"];
//            cur_mxa_time = 1 + GetMaxBuyTime();
//            if (mjjd.instance != null)
//            {
//                mjjd.instance.onGetBuyRes();
//            }
//        }
//        public int GetMaxBuyTime()
//        {//获得vip最大购买次数
//            int count = 0;
//            SXML vipSXML = XMLMgr.instance.GetSXML("vip.viplevel", "vip_level==" + PlayerModel.getInstance().vip);
//            if (vipSXML != null)
//            {
//                SXML temp = vipSXML.GetNode("vt", "type==1");
//                if (temp != null)
//                {
//                    count = temp.getInt("value");
//                }
//            }
//            return count;
//        }
//        public void initMjjdData()
//        {
//            SXML bwcSXML = XMLMgr.instance.GetSXML("mjjd.info2");
//            if (bwcSXML != null)
//            {
//                CompaignData data = new CompaignData();
//                data.lv_limit = bwcSXML.getInt("open_lv");
//                List<int> drop = new List<int>();
//                drop.Add(bwcSXML.getInt("drop_info"));
//                drop.Add(bwcSXML.getInt("drop_info2"));
//                data.drop_list = drop;
//                data.name = bwcSXML.getString("name");
//                compaignList.Add(data);
//            }
//            SXML mjjdSXML = XMLMgr.instance.GetSXML("mjjd.info");
//            if (mjjdSXML != null)
//            {
//                do
//                {
//                    CompaignData data = new CompaignData();
//                    data.lv_limit = mjjdSXML.getInt("open_lv");
//                    List<int> drop = new List<int>();
//                    drop.Add(mjjdSXML.getInt("drop_info"));
//                    data.drop_list = drop;
//                    data.name = mjjdSXML.getString("name");
//                    compaignList.Add(data);
//                    SXML temp = mjjdSXML.GetNode("diff", null);
//                    if (temp != null)
//                    {
//                        do
//                        {
//                            MJJDData mjjd = new MJJDData();
//                            mjjd.diff_lv = temp.getString("diff_lv");
//                            mjjd.lv_limit = temp.getInt("lv_limit");
//                            mjjd.map_id = temp.getInt("map_id");
//                            mjjdList.Add(mjjd);
//                        } while (temp.nextOne());
//                    }
//                } while (mjjdSXML.nextOne());
//            }
//        }
//    }
//    public class MJJDData
//    {
//        public string diff_lv;
//        public int map_id;
//        public int lv_limit;
//    }
//    public class CompaignData
//    {
//        public string name;
//        public int lv_limit;
//        public List<int> drop_list;
//    }
//}
