using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;

namespace MuGame
{
    class RankingProxy : BaseProxy<RankingProxy>
    {
        private List<RankData> rankList1 = new List<RankData>();
        private List<RankData> rankList2 = new List<RankData>();
        private List<RankData> rankList3 = new List<RankData>();
        private List<RankData> rankList4 = new List<RankData>();
        private List<RankData> rankList = new List<RankData>();

        public Dictionary<uint, List<RankData>> dic1 = new Dictionary<uint, List<RankData>>();
        public Dictionary<uint, List<RankData>> dic2 = new Dictionary<uint, List<RankData>>();
        public Dictionary<uint, List<RankData>> dic3 = new Dictionary<uint, List<RankData>>();
        public Dictionary<uint, List<RankData>> dic4 = new Dictionary<uint, List<RankData>>();

        public int type;
        private uint currentPage;

        public static uint ON_GET_RANK_INFO = 10;

        public RankingProxy()
        {
           //addProxyListener(PKG_NAME.C2S_GET_RANK_INFO, getInfo);
        }

        public void sendMsg(uint type,uint page)
        {
            Variant msg = new Variant();
            msg["rank_type"] = type;
            msg["page"] = page;
            currentPage = page;
            sendRPC(254, msg);
        }

        void getInfo(Variant data)
        {
            if (data["res"] <= 0)
            {
                dispatchEvent(GameEvent.Create(ON_GET_RANK_INFO, this, data));
                return;
            }
            //数据读取——————
            type = data["res"];
            List<RankData> list = new List<RankData>();
            data["page"] = currentPage;
            if (data["ranks"]._arr.Count!=0)
            {
                int i=1;
                foreach (Variant rank in data["ranks"]._arr)
                {
                    RankData rankData = new RankData();
                    rankData.rank = rank["rank"];
                    rankData.cid = rank["cid"];
                    rankData.number = (int)(currentPage - 1) * 5 + i;
                    switch (type)
                    {
                        case 1:
                            rankData.name = rank["name"];
                            rankData.family_name = rank["family_name"];
                            if (rankData.family_name == "")
                                rankData.family_name = "无";
                            rankData.combpt = rank["combpt"];
                            rankData.sex = rank["sex"];
                            list.Add(rankData);
                            break;
                        case 2:
                            rankData.name = rank["name"];
                            rankData.family_name = rank["family_name"];
                            if (rankData.family_name == "")
                                rankData.family_name = "无";
                            rankData.combpt = rank["level"];
                            rankData.sex = rank["sex"];
                            list.Add(rankData);
                            break;
                        case 3:
                            rankData.name = rank["name"];
                            rankData.family_name = rank["family_name"];
                            if (rankData.family_name == "")
                                rankData.family_name = "无";
                            rankData.combpt = rank["hero_combpt"];
                            rankData.sex = rank["hero_id"];
                            if (rank.ContainsKey("strengthen"))
                                rankData.strengthen = rank["strengthen"];
                            list.Add(rankData);
                            break;
                        case 4:
                            rankData.name = rank["leader_name"];
                            rankData.family_name = rank["name"];
                            if (rankData.family_name == "")
                                rankData.family_name = "无";
                            rankData.combpt = rank["total_combpt"];
                            rankData.member_num = rank["member_num"];
                            rankData.logoid = rank["logo_id"];
                            rankData.family_level = rank["family_level"];
                            list.Add(rankData);
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                switch (type)
                {
                    case 1:
                        if (dic1.ContainsKey(currentPage))
                            dic1.Remove(currentPage);
                        dic1.Add(currentPage, list);
                        break;
                    case 2:
                        if (dic2.ContainsKey(currentPage))
                            dic2.Remove(currentPage);
                        dic2.Add(currentPage, list);
                        break;
                    case 3:
                        if (dic3.ContainsKey(currentPage))
                            dic3.Remove(currentPage);
                        dic3.Add(currentPage, list);
                        break;
                    case 4:
                        if (dic4.ContainsKey(currentPage))
                            dic4.Remove(currentPage);
                        dic4.Add(currentPage, list);
                        break;
                    default:
                        break;
                } 
            }
            dispatchEvent(GameEvent.Create(ON_GET_RANK_INFO, this, data));
        }
    }

    class RankData
    {
        public uint rank;
        public uint cid;
        public uint sex;
        public string name;
        public string family_name;
        public uint combpt;//战力
        public uint member_num;//家族人数
        public uint family_level;
        public uint strengthen;
        public uint logoid;
        public int number;//序号
    }

}

