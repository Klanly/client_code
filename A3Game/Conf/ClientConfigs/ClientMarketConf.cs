using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientMarketConf : configParser
    {
        private bool isGetName = false;
        public ClientMarketConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientMarketConf( m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if (conf.ContainsKey("market"))
            {
                conf["market"] = GameTools.array2Map(conf["market"], "tp");
            }
            Variant retConf = new Variant();
            retConf["info"] = new Variant();
            retConf["items"] = new Variant();

            foreach (Variant dataObj in conf["market"].Values)
            {
                //dataObj["name"] = LanguagePack.getLanguageText("market", dataObj["name"]._str);
                //foreach (Variant item in conf["items"]._arr)
                //{
                //    item["name"] = LanguagePack.getLanguageText("market", item["name"]._str);
                //}
                Variant ar = new Variant();
                ar["id"] = dataObj["tp"];
                ar["name"] = dataObj["name"];
                ar["carr"] = dataObj["carr"];
                retConf["items"]._arr.Add(ar);
                retConf["info"]._arr.Add(dataObj["items"]);
            }
            return retConf;
        }
        public Variant getMarketConf()
        {
            if (!isGetName)
            {
                isGetName = true;
                foreach (Variant dataObj in m_conf["items"]._arr)
                {
                    dataObj["name"] = LanguagePack.getLanguageText("market", dataObj["name"]._str);
                }
                foreach (Variant items in m_conf["info"]._arr)
                {
                    foreach (Variant item in items._arr)
                    {
                        item["name"] = LanguagePack.getLanguageText("market", item["name"]._str);
                    }
                }
            }
            return m_conf;
        }
    }
}
