using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    class SkillConf : configParser
    {
        static public SkillConf instance;
        public SkillConf(ClientConfig m)
            : base(m)
        {

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SkillConf(m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            instance = this;

            if (conf.ContainsKey("skill"))
            {
                conf["skill"] = GameTools.array2Map(conf["skill"], "id");

                Variant skills = conf["skill"];
                foreach (Variant data in skills.Values)
                {
                    if (data.ContainsKey("Level"))
                    {
                        data["Level"] = GameTools.array2Map(data["Level"], "level");
                    }
                }
            }




            //if (conf.ContainsKey("equip_upgrade"))
            //{
            //    conf["equip_upgrade"] = GameTools.array2Map(conf["equip_upgrade"], "item_id");
            //}
            return conf;
        }

        public Variant getSkillid(String id)
        {
            return conf["skill"][id];
        }

    }
}
