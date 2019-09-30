using System;
using GameFramework;  
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrMonsterConfig : configParser
    {
        public Variant m_monsterConfig = new Variant();
        public SvrMonsterConfig( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create( IClientBase m )
        {           
            return new SvrMonsterConfig( m as ClientConfig );
        }

        public Variant resolve()
        {
            if (this.m_conf == null)
                return null;
            if (this.conf.ContainsKey("mon"))
            {
                for (int i = 0; i < this.conf["mon"].Count; i++)
                {
                    m_monsterConfig[this.conf["mon"][i]["id"]._str] = this.conf["mon"][i];
                }
                return m_monsterConfig;
            }
            return null;

        }
        public Variant get_monster_data(int mid)
        {
            if (m_monsterConfig != null)
            {
                if (!m_monsterConfig.ContainsKey(mid.ToString()))
                    return null;
                return m_monsterConfig[mid.ToString()];
            }
            return null;
        }
        override protected Variant _formatConfig(Variant conf)
        {
            return conf;
        }
        override protected void onData()
        {
            resolve();            
        }
	 
    }
}
