using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientVipConf : configParser
    {
        public ClientVipConf( ClientConfig m ) :base(m)
        { 

        }
        public static ClientVipConf create(IClientBase m)
        {
            return new ClientVipConf( m as ClientConfig );
        }
        protected override Variant _formatConfig(Variant conf)
        {
           Variant temparr;
            Variant temparrdes;
            Variant temparrfun;
            Variant temparrstate;
            if(conf.ContainsKey("vipdata"))
            {
                Variant vipdataobj = conf["vipdata"][0];
                if(null != vipdataobj)
                {
                    temparr = vipdataobj["vip"] as  Variant;
                    Variant vipvipData = new Variant();
                    foreach (Variant vipvip in temparr._arr)
                    {
                        if(vipvip.ContainsKey("des"))
                        {
                            Variant desobj = vipvip["des"][0];
                            temparrdes = desobj["item"] as Variant;
                            //foreach(Variant des in temparrdes._arr)
                            //{
                            //    des["item"] = GameTools.array2Map( des["item"], "des" );
                            //}
                            desobj[0] =temparrdes; 
                            conf["vipdata"]["des"] = desobj;
                        }

                        if(vipvip.ContainsKey("fun"))
                        {
                            Variant desfun = vipvip["fun"][0];
                            temparrfun = desfun["item"] as Variant;
                            foreach(Variant fun in temparrfun._arr)
                            {
                                fun["item"] = GameTools.array2Map( fun["item"], "des" );
                            }
                            desfun[0] = temparrfun;
                            conf["vipdata"]["fun"] = desfun;
                        }

                        if(vipvip.ContainsKey("state"))
                        {
                            Variant desstate = vipvip["state"][0];
                            temparrstate = desstate["item"] as Variant;
                            foreach (Variant state in temparrstate._arr)
                            {
                                state["item"] = GameTools.array2Map( state["item"], "des");
                            }
                            desstate[0] = temparrstate;
                            
                            conf["vipdata"]["state"] = desstate;
                        }
                        
                        //vipvip.des = transTmchks(vipvip.des);
                        vipvipData[vipvip["id"]] = vipvip;	
                    }
                    conf["vipdata"] = vipvipData;
                }
            }
            return conf;
        }
        public Variant get_vip_description(int id = 1)
		{
            return conf["vipdata"][id.ToString()];
		}
    }
}
