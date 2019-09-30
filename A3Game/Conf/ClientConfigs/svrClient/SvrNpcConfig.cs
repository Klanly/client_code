using System;
using GameFramework;  
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrNPCConfig : configParser
    {
        public static SvrNPCConfig instance;
        public SvrNPCConfig( ClientConfig m ):base(m)
        {
            instance = this;
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrNPCConfig(m as ClientConfig);
        }

        protected override Variant _formatConfig(Variant conf)
        {
            if (conf != null)
            {
                Variant npcConf = conf["npc"];
                if (npcConf != null)
                {
                    npcConf = GameTools.array2Map(npcConf, "id");
                }
                foreach (string npcid in npcConf.Keys)
                {
                    if (npcConf[npcid] != null && npcConf[npcid].ContainsKey("lentry"))
                    {
                        Variant lentry = GameTools.split(npcConf[npcid]["lentry"]._str, ",", GameConstantDef.SPLIT_TYPE_INT);
                        npcConf[npcid]["lentry"] = lentry;
                    }
                }
                if (conf.ContainsKey("state_grp"))
                {
                    conf["state_grp"] = GameTools.array2Map(conf["state_grp"], "id");
                }
                if (conf.ContainsKey("dialog"))
                {
                    conf["dialog"] = GameTools.array2Map(conf["dialog"], "did");
                }
                conf["npc"] = npcConf;
            }


            return base._formatConfig(conf);
        }

        public Variant get_npc_data(int npcid)
        { 
            if (m_conf == null || !m_conf["npc"].ContainsKey(npcid.ToString()))
			{
				GameTools.PrintError( "get_npc_data ["+npcid+"] Config Err!"  );
                return null;
			}
            Variant npcData = m_conf["npc"][npcid.ToString()];
            
            if (npcData == null)
			{
				GameTools.PrintError( "get_npc_data npc["+npcid+"] not exist Err!"  );
				return null;
			}
            if (npcData.ContainsKey("name"))
                npcData["name"] = LanguagePack.getLanguageText("npcName", npcid.ToString());
            return npcData;
        }
        public int get_carrchief_npc(int carr)
        {
            foreach (string nid in m_conf["npc"].Keys)
            {
                Variant n = m_conf["npc"][nid];
                if (n.ContainsKey("carrchief") && (n["carrchief"]._int == carr))
                {
                    return n["id"]._int;
                }
            }
            return 0;
        }
        public Variant get_sell_item(int siid)
        {
            if (m_conf == null)
                return null;
            return m_conf["sell_item"][siid];
        }

        public String get_sell_type(int stid)
        {
            if (m_conf == null)
                return "";
            return m_conf["sell_type"][stid]["name"]._str;
        }
        public string get_dialog(int did)
        {
            if (m_conf == null)
                return "";

            bool b = m_conf.ContainsKey("dialog");
            bool b2 = m_conf["dialog"].ContainsKey(did.ToString());
            Variant d = m_conf["dialog"][did.ToString()];
            if (!m_conf.ContainsKey("dialog") || !m_conf["dialog"].ContainsKey(did.ToString()))
                return "";
            return m_conf["dialog"][did.ToString()]["content"];
        }
        public int getLevelEntryNpc(int lvlid)
        {
            foreach (String i in m_conf["npc"].Keys)
            {
                Variant npc = m_conf["npc"][i];
                if (!npc.ContainsKey("lentry"))
                    continue;
                Variant lentryArr = npc["lentry"];
                for (int j = 0; j < lentryArr.Count; j++)
                {
                    if (lentryArr[j]._int == lvlid)
                    {
                        return npc["id"]._int;
                    }
                }
            }
            return 0;
        }
        public Variant getNpcConf()
        {
            return m_conf;
        }
    }
}