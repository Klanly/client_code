using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientImgResConf : configParser
    {
    
        public ClientImgResConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new ClientImgResConf(m as ClientConfig);
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("img"))
            {
                _imgInfo = GameTools.array2Map(conf["img"], "name");
            }
            return null;
        }
        private Variant _imgInfo;
        public string GetImgRes(string name)
        {
            if (_imgInfo[name] != null)
            {
                return _imgInfo[name]["res"];
            }
            return "";

        }

    }
}
