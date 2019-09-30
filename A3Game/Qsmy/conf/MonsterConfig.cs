using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    class MonsterConfig: configParser
    {
        static public MonsterConfig instance;
         public MonsterConfig(ClientConfig m)
            : base(m)
        {

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new MonsterConfig(m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            instance = this;

            if (conf.ContainsKey("monsters"))
            {
                conf["monsters"] = GameTools.array2Map(conf["monsters"], "id");
            }

            //if (conf.ContainsKey("equip_upgrade"))
            //{
            //    conf["equip_upgrade"] = GameTools.array2Map(conf["equip_upgrade"], "item_id");
            //}
            return conf;
        }

        public Variant getMonster(String id)
        {
            return conf["monsters"][id];
        }
    }
}
