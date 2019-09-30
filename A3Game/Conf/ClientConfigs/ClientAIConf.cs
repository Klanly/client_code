using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class ClientAIConf : configParser
    {
        public ClientAIConf( ClientConfig m ) :base( m )
        { 

        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new ClientAIConf(m as ClientConfig);
        }
        override protected Variant _formatConfig(Variant conf)
		{
			// TO DO : add more format segment
			if(conf.ContainsKey("ai"))
			{
				conf["ai"] = conf["ai"][0];
				if(conf["ai"].ContainsKey("autobattle"))
				{
					conf["ai"]["autobattle"] = conf["ai"]["autobattle"][0];
					if(conf["ai"]["autobattle"].ContainsKey("mprecitms"))
					{
						conf["ai"]["autobattle"]["mprecitms"] = conf["ai"]["autobattle"]["mprecitms"][0];
					}
                    if (conf["ai"]["autobattle"].ContainsKey("hprecitms"))
					{
						conf["ai"]["autobattle"]["hprecitms"] = conf["ai"]["autobattle"]["hprecitms"][0];
					}
				}
			}
			return conf;
		}

        //public function get mpRecItms():Array
        //{
        //    return (_conf.ai.autobattle.mprecitms.tpids as String).split(",");	
        //}
        public Variant mpRecItms
        {
            get{
                string temp = m_conf["ai"]["autobattle"]["mprecitms"]["tpids"]._str;
                Variant arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
                return arr;
                //return (m_conf["ai"]["autobattle"]["mprecitms"]["tpids"]._str).split(",");
            }
        }
        public /*List<string>*/Variant hpRecItms
        {
            get{
                string temp = m_conf["ai"]["autobattle"]["hprecitms"]["tpids"]._str;
                Variant arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
                return arr;
                //return (m_conf["ai"]["autobattle"]["hprecitms"]["tpids"]._str).split(",");	
            }
        }
        //public function get hpRecItms():Array
        //{       
        //    return (_conf.ai.autobattle.hprecitms.tpids as String).split(",");	
        //}
    }
}
