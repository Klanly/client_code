using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientSystemConf : configParser
    {
        public ClientSystemConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientSystemConf( m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("defaultSet"))
            {
                conf["defaultSet"] = conf["defaultSet"][0];
            }
            if(conf.ContainsKey("autoSet"))
            {
                conf["autoSet"] = conf["autoSet"][0];
            }
            if(conf.ContainsKey("system"))
            {
                conf["system"] = conf["system"][0];
            }
            return conf;
        }
                
        private Variant _defaultSet;
        public Variant defaultSet()
        {
            if(null == _defaultSet)
            {
                _defaultSet = new Variant();
                foreach(string s in m_conf["defaultSet"].Keys)
                {
                    _defaultSet[s] = m_conf["defaultSet"][s][0]["val"];
                }
            }
            return _defaultSet;
        }

        private Variant _autoSet;
        public Variant autoSet()
        {
            if(null == _autoSet)
            {
                _autoSet = new Variant();
                foreach(string s in m_conf["autoSet"].Keys)
                {
                    _autoSet[s] = m_conf["autoSet"][s][0]["val"];
                }
            }
            return _autoSet;
        }
        
        private Variant _systemdata;
        public Variant system()
        {
            if(null == _systemdata)
            {
                _systemdata = new Variant();
                foreach(string s in m_conf["system"].Keys)
                {
                    _systemdata[s] = m_conf["system"][s][0]["val"];
                }
            }
            return _systemdata;
        }
    }
}
