using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientTriggerConf : configParser
    {
        public ClientTriggerConf( ClientConfig m ):base(m)
        {
            _triggers = new Variant();
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientTriggerConf( m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("trigger"))
            {
                foreach( Variant trConf in conf["trigger"]._arr )
                {
                    if( !(trConf.ContainsKey("action")) ) continue;
                    
                    foreach( Variant actConf in trConf["action"]._arr )
                    {
                        actConf["actdata"] = actConf["actdata"][0];
                    }
                    _triggers[trConf["id"]] = trConf;		
                }
            }

            return null;
        }
        private Variant _triggers;
                
        public Variant GetTriggers(uint lvl )
        {
            Variant ret = new Variant();
            Variant lvlConf;
            foreach( Variant trg in _triggers.Values )
            {
                lvlConf = trg["lvl"];
                if( lvlConf )
                {
                    if( lvlConf[0]["min"] > lvl || lvlConf[0]["max"] < lvl )
                    {
                        continue;
                    }
                }
                if(trg["condition"])
                {
                    foreach(Variant cod in trg["condition"].Values)
                    {
                        if(cod["hasmis"])
                        {
                            string misStr = cod["hasmis"];
                            Variant misArr = GameTools.split(misStr, ",");
                            for(int i=0;i<misArr.Length;++i)
                            {
                                misArr[i] = (int)misArr[i];
                            }
                            cod["hasmis"] = misArr;
                        }
                    }
                }
                ret._arr.Add( trg );
            }
            return ret;
        }
    }
}
