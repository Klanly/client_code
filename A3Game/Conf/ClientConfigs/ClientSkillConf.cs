using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientSkillConf : configParser
    {
        public ClientSkillConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientSkillConf( m as ClientConfig );
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("skill"))
            {
                conf["skill"] = GameTools.array2Map(conf["skill"], "skid");
            }
            
            if(conf.ContainsKey("state"))
            {
                conf["state"] = GameTools.array2Map(conf["state"], "stateid");
            }
            if(conf.ContainsKey("bstate"))
            {
                conf["bstate"] = GameTools.array2Map(conf["bstate"], "bstateid");
            }
            // TO DO : add more format segment
            return conf;
        }
            
        //public string get_skill_icon_url(uint skid)
        //{
        //    if(m_conf["skill"].ContainsKey(skid.ToString()))
        //    {
        //        return m_conf["skill"][skid.ToString()]["icon"];
        //    }
        //    return "";
        //}
        public string get_skill_icon_url(uint skid,int carr = 0)
        {
            if (m_conf["skill"].ContainsKey(skid.ToString()))
            {
                Variant skillCarrConf = m_conf["skill"][skid.ToString()];
                if (!skillCarrConf.ContainsKey("carr"))
                {
                    return skillCarrConf["icon"];
                }
                Variant skillConf = skillCarrConf["carr"];
                for (int i = 0; i < skillConf.Count; i++)
                {
                    if (skillConf[i]["id"]._int == carr)
                    {
                        return skillConf[i]["icon"];
                    }
                }
            }
            return "";
        }
        
        public string get_state_icon_url(uint stateid)
        {
            if(m_conf["state"].ContainsKey(stateid.ToString()))
            {
                return m_conf["state"][stateid.ToString()]["icon"];
            }
            return "";
        }
        public string GetBlessIconUrl(uint bstateid)
        {
            if(m_conf["bstate"].ContainsKey((int)bstateid))
            {
                return m_conf["bstate"][bstateid]["icon"];
            }
            return "";
        }
        
        public Variant GetContentSkill(uint skid,uint lvl)
        {
            if(m_conf["skill"][skid] && m_conf["skill"][skid]["sklvl"])
            {
                foreach(Variant sklvlObj in m_conf["skill"][skid]["sklvl"].Values)
                {
                    if(sklvlObj["lvl"] == lvl)
                    {
                        return sklvlObj;
                    }
                }
            }
            
            return null;
        }
    }
}
