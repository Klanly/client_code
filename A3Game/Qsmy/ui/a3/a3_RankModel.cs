using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class a3_RankModel : ModelBase<a3_RankModel>
    {

        public static int now_id = 0;
        public static int nowexp = 0;
        public static bool nowisactive = true;
        public Dictionary<int, rankinfos> dicrankinfo;
        public a3_RankModel()
        {
            dicrankinfo = new Dictionary<int, rankinfos>();
            readxml();
        }


        void readxml()
        {
            List<SXML> xml = XMLMgr.instance.GetSXMLList("achievement.title");
            foreach (SXML x in xml)
            {
                rankinfos infos = new rankinfos();
                infos.name = x.getString("title_name"); // -- 原为rank_name
                infos.title_id = x.getInt("title_id");
                infos.rankexp = x.getInt("para");
                foreach (var v in x.GetNodeList("nature"))
                {
                    infos.nature[v.getUint("att_type")] = v.getInt("att_value");
                }
                dicrankinfo[infos.title_id] = infos;
            }
        }


        public void refreinfo(int title_nowid, int exp, bool isavtive = true)
        {
            now_id = title_nowid;
            nowexp = exp;
            nowisactive = isavtive;
        }

        public bool CheckTitleLevelupAvailable()
            => !FunctionOpenMgr.instance.Check(FunctionOpenMgr.ACHIEVEMENT)?
                false :
                dicrankinfo.ContainsKey(now_id + 1) ?
                    PlayerModel.getInstance().ach_point > dicrankinfo[now_id + 1].rankexp :
                false;

    }

    class rankinfos
    {
        public int title_id;
        public string name;
        public int rankexp;
		public Dictionary<uint, int> nature = new Dictionary<uint, int>();
    }


}
