using System;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientFWGameConf : configParser
    {
        public ClientFWGameConf(ClientConfig m)
            : base(m)
        {

        }
        public static ClientFWGameConf create(IClientBase m)
        {

            return new ClientFWGameConf(m as ClientConfig);
        }
        override protected Variant _formatConfig( Variant conf )
		{
            if (conf.ContainsKey("chatitle"))
            {
                Variant chatitleObj = new Variant();
                for (int j = 0; j < conf["chatitle"].Length; j++)
                {
                    Variant chatitleConf = conf["chatitle"][j];
                    chatitleConf["show"] = GameTools.array2Map(chatitleConf["show"], "id");
                    if (chatitleConf.ContainsKey("sort"))
                    {
                        Variant sortArr = GameTools.split(chatitleConf["sort"]._str, ",");
                        for (int i = 0; i < sortArr.Length; ++i)
                        {
                            sortArr[i] = sortArr[i]._int;
                        }
                        chatitleConf["sort"] = sortArr;
                    }
                    if (chatitleConf.ContainsKey("tp") && chatitleConf["tp"] != null)
                    chatitleObj[chatitleConf["tp"]._str] = chatitleConf;
                }
                conf["chatitle"] = chatitleObj;
            }

            if (conf.ContainsKey("chagrd"))
            {
                conf["chagrd"] = GameTools.array2Map(conf["chagrd"], "tp");
            }

            if (conf.ContainsKey("chadyn"))
            {
                Variant chadynObj = new Variant();
                foreach (Variant chadynConf in conf["chadyn"]._arr)
                {
                    chadynConf["show"] = GameTools.array2Map(chadynConf["show"], "id");
                    chadynConf["ani"] = GameTools.array2Map(chadynConf["ani"], "tp");

                    chadynObj[chadynConf["tp"]._str] = chadynConf;
                }
                conf["chadyn"] = chadynObj;
            }
            if (conf.ContainsKey("chabar"))
            {
                Variant chabarObj = new Variant();
                foreach (Variant chabarConf in conf["chabar"]._arr)
                {
                    chabarConf["show"] = GameTools.array2Map(chabarConf["show"], "idx");
                    chabarConf["ani"] = chabarConf["ani"][0];
                    chabarObj[chabarConf["tp"]._str] = chabarConf;
                }
                conf["chabar"] = chabarObj;
            }
            if (conf.ContainsKey("uiprop"))
            {
                conf["uiprop"] = GameTools.array2Map(conf["uiprop"], "id");
            }

            if (conf.ContainsKey("sound"))
            {
                conf["sound"] = GameTools.array2Map(conf["sound"], "id");
            }

            if (conf.ContainsKey("uieff"))
            {
                conf["uieff"] = GameTools.array2Map(conf["uieff"], "id");
            }

            if (conf.ContainsKey("carrAni"))
            {
                conf["carrAni"] = GameTools.array2Map(conf["carrAni"], "id");
            }

            if (conf.ContainsKey("carrLayer"))
            {
                conf["carrLayer"] = GameTools.array2Map(conf["carrLayer"], "id");
            }
		
            return conf;
		}
		
        ////-------------------------------------------角色头顶显示配置---------------------------------------
        public Variant GetChaBarConf(string tp)
        {
            Variant barConf = conf["chabar"];
            if (barConf!=null)
            {
                return barConf[tp];
            }
            return null;
        }
        public Variant GetChaTitleConf(string tp)
        {
            Variant titleConf = conf["chatitle"];
            if (titleConf!=null)
            {
                return titleConf[tp];
            }
            return null;
        }

        public Variant GetChaGroundConf(string tp)
        {
            Variant grdConf = conf["chagrd"];
            if (grdConf!=null)
            {
                return grdConf[tp];
            }
            return null;
        }

        public Variant GetChaDynamicConf(string tp)
        {
            Variant dynConf = conf["chadyn"];
            if (dynConf!=null)
            {
                return dynConf[tp];
            }
            return null;
        }

        //------------------------------------------- 界面 相关属性------------------------------------------
        public Variant GetUIProp(string id)
        {
            Variant propConf = conf["uiprop"];
            if (propConf != null)
            {
                return propConf[id];
            }
            return null;
        }
        //声音文件	
        public String GetSoundUrl(string id)
        {
            Variant propConf = conf["sound"];
            if (propConf != null && propConf[id] != null)
            {
                return propConf[id]["url"];
            }
            return null;
        }
        //ui特效		
        public Variant GetUIEffConf(string id)
        {
            Variant propConf = conf["uieff"];
            if (propConf != null)
            {
                return propConf[id];
            }
            return null;
        }

        //职业动作	
        public String GetAniByName(int carr, string aniName)
        {
            if (conf["carrAni"][carr] != null)
            {
                Variant propConf = conf["carrAni"][carr]["ani"];
                if (propConf)
                {
                    foreach (Variant obj in propConf._arr)
                    {
                        if (obj["name"]._str == aniName)
                        {
                            return obj["ani"]._str;
                        }
                    }
                }
            }
            return "";
        }

        //获得层级关系
        public String GetLayerByAni(int carr, string aniName)
        {
            Variant propConf = conf["carrLayer"][carr]["layer"];
            if (propConf)
            {
                foreach (Variant obj in propConf._arr)
                {
                    if (obj["ani"]._str == aniName)
                    {
                        return obj["layer"]._str;
                    }
                }
            }
            return "";
        }
    }

}
    

