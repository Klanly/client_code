using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrCarrLvlConfig : configParser
    {

        public SvrCarrLvlConfig(ClientConfig m) :
            base(m)
        {

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrCarrLvlConfig(m as ClientConfig);
        }

        public Variant get_carr_lvl_desc(int carr, int lvl)
        {
            foreach (Variant carrObj in m_conf["carr"]._arr)
            {
                if (carrObj["carr"]._int == carr && carrObj["lvl"]._int == lvl)
                {
                    return carrObj;
                }
            }
            return null;
        }


    }
}
