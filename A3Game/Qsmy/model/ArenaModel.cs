using System;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    class ArenaModel : ModelBase<ArenaModel>
    {
        public int lastRank;
        public int rank;
        public int reputation;
        public int surplus_time;
        public int surplus_buy;
        public int award_rank;
        public int topRank=-1;

        public ArenaModel()
        {

        }

        public void changeRank(int r)
        {
            lastRank = rank;
            rank = r;
        }

        public void BaseInfoInit(Variant data)
        {
         lastRank= rank = data["myrank"]._int32;
            reputation = data["nobpt"]._int32;
            surplus_time = data["left_times"]._int32;
            surplus_buy = data["left_buytimes"]._int32;
            award_rank = data["avaliable_reward"]._int32;

            if (data.ContainsKey("top_rank"))
                topRank = data["top_rank"];
        }



        public void SetBuyTimeCount(int num)
        {
           surplus_buy = num;
        }
    }

    
}
