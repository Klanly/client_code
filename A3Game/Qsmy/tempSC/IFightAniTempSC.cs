using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    public interface IFightAniTempSC
    {
        void onAttackPoint(int skillid);
        void onAttackBegin(int num);
        void onAttackShake_time_num_str(string pram);
        void onAttack_sound(int id);
    }
}
