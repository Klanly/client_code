using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientGuideConf : configParser
    {
 
        public ClientGuideConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new ClientGuideConf(m as ClientConfig);
        }
        protected override Variant  _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("guide"))
            {
                _guides = GameTools.array2Map( conf["guide"], "id");
            }
            if( conf.ContainsKey("ui") )
            {
                _uiConf = conf["ui"][0];
            }
            return null;
        }
        
        private Variant _guides;
        private Variant _uiConf;
        public Variant GetGuideConf(string id )
        {
            return _guides[id];
        }
        
        public Variant GetGuideUIConf()
        {
            return _uiConf;
        }
    }
}
