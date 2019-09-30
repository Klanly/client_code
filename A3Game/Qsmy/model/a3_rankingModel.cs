using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using System.Collections;
using UnityEngine;
namespace MuGame
{
    class a3_rankingModel : ModelBase<a3_rankingModel>
    {
        //public Dictionary<int, List<RankingData>> rank0 = new Dictionary<int, List<RankingData>>();
        //public Dictionary<int, List<RankingData>> rank1 = new Dictionary<int, List<RankingData>>();
        //public Dictionary<int, List<RankingData>> rank2 = new Dictionary<int, List<RankingData>>();
        //public Dictionary<int, List<RankingData>> rank3 = new Dictionary<int, List<RankingData>>();
        //public Dictionary<int, List<RankingData>> rank4 = new Dictionary<int, List<RankingData>>();

        //public int stage0 = 1;
        //public int stage1 = 1;
        //public int stage2 = 1;
        //public int stage3 = 1;
        //public int stage4 = 1;

        public Dictionary<uint, RankingData> zhanli = new Dictionary<uint, RankingData>();
        public Dictionary<uint, RankingData> lvl = new Dictionary<uint, RankingData>();
        public Dictionary<uint, RankingData> wing = new Dictionary<uint, RankingData>();
        public Dictionary<uint, RankingData> summon = new Dictionary<uint, RankingData>();
        public Dictionary<uint, RankingData> juntuan = new Dictionary<uint, RankingData>();
        public Dictionary<uint, RankingData> spost = new Dictionary<uint, RankingData>();

        public  bool zhanli_frist = true;
        public  bool lvl_frist = true;
        public  bool chibang_frist = true;
        public  bool juntuan_frist = true;
        public  bool summon_frist = true;
        public bool spost_frist = true;

        public float time = 0;
        //bool run = false;
        TickItem process_3008;
        //public void SetDic(int type, List<RankingData> l)
        //{
        //    switch (type)
        //    {
        //        case 1:
        //            rank0[stage0] = l;
        //            stage0++;
        //            break;
        //        case 2:
        //            rank1[stage1] = l;
        //            stage1++;
        //            break;
        //        case 3:
        //            rank2[stage2] = l;
        //            stage2++;
        //            break;
        //        case 4:
        //            rank3[stage3] = l;
        //            stage3++;
        //            break;
        //        case 5:
        //            rank4[stage4] = l;
        //            stage4++;
        //            break;
        //    }
        //}

        public void runTime( float t)
        {

            time = t;
            if (process_3008 != null)
            {
                TickMgr.instance.removeTick(process_3008);
                process_3008 = null;
            }
            process_3008 = new TickItem(onUpdate_3008);
            TickMgr.instance.addTick(process_3008);
        }

        float t = 0;
        void onUpdate_3008(float s)
        {
            t += s;
            if ( t >= 1)
            {
                time--;
                if (a3_ranking.isshow)
                {
                    a3_ranking.isshow.setTime(time);
                }
                t = 0;
            }
            if (time <= 0)
            {
                zhanli_frist = true;
                lvl_frist = true;
                chibang_frist = true;
                juntuan_frist = true;
                summon_frist = true;
                spost_frist = true;
                a3_rankingProxy.getInstance().getTime();
                TickMgr.instance.removeTick(process_3008);
                process_3008 = null;
            }
        }

    }
}
