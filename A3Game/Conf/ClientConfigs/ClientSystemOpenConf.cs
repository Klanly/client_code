using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientSystemOpenConf : configParser
    {
        public ClientSystemOpenConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientSystemOpenConf( m as ClientConfig );
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("option"))
            {
                Variant temp = new Variant();
                foreach(Variant obj in conf["option"]._arr)
                {
                    if(obj["oid"] != "")
                    {
                        temp[obj["oid"]] = obj;
                    }
                    if(obj.ContainsKey("tmchk"))
                    {
                        foreach(Variant tmchk in obj["tmchk"]._arr)
                        {
                            if(tmchk.ContainsKey("tb"))
                            {
                                tmchk["tb"] = GameTools.GetTmchkAbs(tmchk["tb"]);
                            }
                            if(tmchk.ContainsKey("te"))
                            {
                                tmchk["te"] = GameTools.GetTmchkAbs(tmchk["te"]);
                            }
                        }
                        
                    }
                }
                conf = temp;
            }
            return conf;
        }
        public Variant GetSysOpenConf()
        {
            return m_conf;
        }
    }
}
