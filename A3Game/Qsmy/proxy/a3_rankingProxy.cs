using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;

namespace MuGame
{
    class a3_rankingProxy : BaseProxy<a3_rankingProxy>
    {
        public static uint EVENT_INFO = 0;

        public a3_rankingProxy()
        {
            addProxyListener(PKG_NAME.C2S_GET_RANK_INFO, getInfo);
        }

        public void send_Getinfo(uint type, uint page)
        {
            Variant msg = new Variant();
            msg["rank_type"] = type;
            msg["page"] = page;
            sendRPC(PKG_NAME.C2S_GET_RANK_INFO, msg);
        }
        public void send_Getinfo(uint type, uint page,uint self)
        {
            Variant msg = new Variant();
            msg["rank_type"] = type;
            msg["page"] = page;
            msg["self_rank"] = self;
            sendRPC(PKG_NAME.C2S_GET_RANK_INFO, msg);
        }
        public void getTime()
        {
            Variant msg = new Variant();
            msg["rank_type"] = 0;
            sendRPC(PKG_NAME.C2S_GET_RANK_INFO, msg);
        }

        void getInfo(Variant data)
        {
            debug.Log("排行榜"+data.dump());
            int res = data["res"];
            if (res < 0) { return; }
            switch (res)
            {
                case 0:
                    float m = data["limit"];
                    a3_rankingModel.getInstance().runTime(m);
                    if (a3_ranking.isshow)
                    {
                        a3_ranking.isshow.setTime(m);
                    }
                    break;
                case 1:
                   // List<RankingData> list = new List<RankingData>();
                    Variant info = data["ranks"];
                    foreach (Variant one in info._arr)
                    {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.cid = one["cid"];
                        RankingData.carr = one["carr"];
                        RankingData.combpt = one["combpt"];
                        RankingData.zhuan = one["zhuan"];
                        RankingData.lvl = one["lvl"];
                        RankingData.viplvl = one["vip"];
                        RankingData.name = one["name"];

                        a3_rankingModel.getInstance().zhanli[RankingData.rank] = RankingData;
                      //  list.Add(RankingData);
                    }
                    //a3_rankingModel.getInstance().SetDic(res, list);
                    //if (a3_ranking.instan)
                    //{
                    //    a3_ranking.instan.Getinfo_panel(list,res);
                    //}
                    if (a3_rankingModel.getInstance().zhanli.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info.Count < 10)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info.Count == 0 && a3_rankingModel.getInstance().zhanli.Count % 10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    break;
                case 2:
                   // List<RankingData> list1 = new List<RankingData>();
                    Variant info1 = data["ranks"];
                    foreach (Variant one in info1._arr)
                    {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.cid = one["cid"];
                        RankingData.carr = one["carr"];
                        RankingData.combpt = one["combpt"];
                        RankingData.zhuan = one["zhuan"];
                        RankingData.lvl = one["lvl"];
                        RankingData.viplvl = one["vip"];
                        RankingData.name = one["name"];
                        a3_rankingModel.getInstance().lvl[RankingData.rank] = RankingData;
                       // list1.Add(RankingData);
                    }
                    if (a3_rankingModel.getInstance().lvl.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info1.Count < 10)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info1.Count == 0 && a3_rankingModel.getInstance().lvl.Count % 10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    //if (a3_ranking.instan)
                    //{
                    //    a3_ranking.instan.Getinfo_panel(list1, res);
                    //}
                    break;
                case 3:
                   // List<RankingData> list2 = new List<RankingData>();
                    Variant info2 = data["ranks"];
                    foreach (Variant one in info2._arr)
                    {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.cid = one["cid"];
                        RankingData.carr = one["carr"];
                        RankingData.flylvl = one["level"];
                        RankingData.stage = one["stage"];
                        RankingData.viplvl = one["vip"];
                        RankingData.name = one["name"];
                        a3_rankingModel.getInstance().wing[RankingData.rank] = RankingData;
                       // list2.Add(RankingData);
                    }
                    if (a3_rankingModel.getInstance().wing.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info2.Count < 10)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info2.Count == 0 && a3_rankingModel.getInstance().wing.Count % 10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    //if (a3_ranking.instan)
                    //{
                    //    a3_ranking.instan.Getinfo_panel(list2, res);
                    //}
                    break;
                case 4:
                   // List<RankingData> list3 = new List<RankingData>();
                    Variant info3 = data["ranks"];
                    foreach (Variant one in info3._arr)
                    {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.jt_combpt = one["combpt"];
                        RankingData.jt_id = one["clanid"];
                        RankingData.jt_lvl = one["lvl"];
                        RankingData.jt_name = one["clname"];
                        a3_rankingModel.getInstance().juntuan[RankingData.rank] = RankingData;
                      //  list3.Add(RankingData);
                    }
                    if (a3_rankingModel.getInstance().juntuan.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info3.Count < 10)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info3.Count == 0 && a3_rankingModel.getInstance().juntuan.Count % 10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    //if (a3_ranking.instan)
                    //{
                    //    a3_ranking.instan.Getinfo_panel(list3, res);
                    //}
                    break;
                case 5:
                   // List<RankingData> list4 = new List<RankingData>();
                    Variant info4 = data["ranks"];
                    foreach (Variant one in info4._arr)
                    {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.cid = one["cid"];
                        RankingData.carr = one["carr"];
                        RankingData.name = one["name"];
                        RankingData.viplvl = one["vip"];
                        RankingData.zhs_combpt = one["combpt"];
                        RankingData.talent = one["talent"];
                        RankingData.zhs_id = one["id"];
                        RankingData.zhs_lvl = one["level"];
                        RankingData.zhs_tpid = one["tpid"];
                        a3_rankingModel.getInstance().summon[RankingData.rank] = RankingData;
                     //   list4.Add(RankingData);
                    }
                    if (a3_rankingModel.getInstance().summon.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info4.Count < 10 && info4.Count > 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info4.Count ==0 && a3_rankingModel.getInstance().summon.Count %10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    //if (a3_ranking.instan)
                    //{
                    //    a3_ranking.instan.Getinfo_panel(list4, res);
                    //}
                    break;

                case 7:
                    Variant info7 = data["ranks"];
                    foreach (Variant one in info7._arr) {
                        RankingData RankingData = new RankingData();
                        RankingData.rank = one["rank"];
                        RankingData.cid = one["cid"];
                        RankingData.carr = one["carr"];
                        RankingData.name = one["name"];
                        RankingData.viplvl = one["vip"];
                        RankingData.piont = one["point"];
                        if (one.ContainsKey ("combpt")) {
                            RankingData.combpt = one["combpt"];
                        }
                        a3_rankingModel.getInstance().spost[RankingData.rank] = RankingData;
                    }
                    if (a3_rankingModel.getInstance().spost.Count >= 50)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info7.Count < 10 && info7.Count > 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }
                    else if (info7.Count == 0 && a3_rankingModel.getInstance().spost.Count % 10 == 0)
                    {
                        dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    }

                    break;
                case -7100:
                case -7101:
                    dispatchEvent(GameEvent.Create(EVENT_INFO, this, data));
                    break;

            }
            if (data.ContainsKey("self_rank"))
            {
                if (a3_ranking.instan)
                {
                    a3_ranking.instan.refresh_myRank(res, data["self_rank"]);
                }
            }
            
            

        }

    }
    class RankingData
    {
        public uint rank;
        public uint cid;
        public string name;
        public uint combpt;//战力
        public int zhuan;
        public int lvl;
        public int viplvl;
        public uint carr;
        public int stage;//翅膀阶级
        public int flylvl;//翅膀等级
        public int jt_id;//军团id
        public int jt_combpt;//军团战力
        public int jt_lvl;//军团等级
        public string jt_name;//军团名
        public int zhs_tpid;//召唤兽tpid
        public int talent;//召唤兽星级
        public int zhs_combpt;//召唤兽战力
        public int zhs_lvl;//召唤兽等级
        public int zhs_id;//召唤兽id
        public int piont;//积分点
    }
}
