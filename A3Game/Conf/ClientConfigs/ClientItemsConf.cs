using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ClientItemsConf : configParser
    {
        static public ClientItemsConf instace;
        public ClientItemsConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create( IClientBase m )
        {           
            return new ClientItemsConf(m as ClientConfig);
        }

        protected override Variant _formatConfig(Variant conf)
        {
            instace = this;
            if(conf.ContainsKey("item"))
            {
                
                conf["item"] = GameTools.array2Map(conf["item"], "tpid");
            }
            if(conf.ContainsKey("itemeqp"))
            {
                conf["itemeqp"] = GameTools.array2Map(conf["itemeqp"], "tpid");
            }
            if(conf.ContainsKey("itemqual"))
            {
                conf["itemqual"] = GameTools.array2Map(conf["itemqual"], "qual");
            }
            
            if(conf.ContainsKey("flvlmat"))
            {
                Variant flvlmatObj = new Variant();
                foreach(Variant flvlmat in conf["flvlmat"]._arr)
                {
                    flvlmat["flvl"] = GameTools.array2Map(flvlmat["flvl"], "lvl");
                    flvlmatObj[ flvlmat["id"] ] = flvlmat;
                }
                conf["flvlmat"] = flvlmatObj;
            }
            if(conf.ContainsKey("flvlemt"))
            {
                Variant flvlemtObj = new Variant();
                foreach(Variant flvlemt in conf["flvlemt"]._arr)
                {
                    Variant attachEffect = new Variant();
                    foreach(Variant flvl_emt in flvlemt["attachEffect"]._arr)
                    {
                        if(attachEffect[flvl_emt["lvl"]]==null)
                        {
                            attachEffect[flvl_emt["lvl"]] = new Variant();
                        }
                        attachEffect[flvl_emt["lvl"]]._arr.Add(flvl_emt);
                    }
                    flvlemt["attachEffect"] = attachEffect;
                    flvlemtObj[ flvlemt["id"] ] = flvlemt;
                }
                conf["flvlemt"] = flvlemtObj;
            }
            if(conf.ContainsKey("itemIcon"))
            {
                conf["itemIcon"] = GameTools.array2Map(conf["itemIcon"], "tp");				
            }
            if(conf.ContainsKey("clientitem"))
            {
                conf["clientitem"] = GameTools.array2Map(conf["clientitem"], "tp");				
            }
            if(conf.ContainsKey("itemeff"))
            {
                conf["itemeff"] = GameTools.array2Map(conf["itemeff"], "tp");				
            }
            if(conf.ContainsKey("eqpeff"))
            {
                conf["eqpeff"] = GameTools.array2Map(conf["eqpeff"], "tp");
            }
            // TO DO : add more format segment
            
            return conf;
        }
            /**
         * 获取需要在tips上显示3D模型的物品信息
         * */
        public Variant GetShow3dItem(int tpid)
        {
            if(m_conf["item"].ContainsKey(tpid))
            {
                Variant arr = m_conf["item"][tpid]["show3D"];
                if(null != arr)
                {
                    return arr[0];
                }
            }
            return null;
        }
        private Variant _clientItem;
        /**
         * 用于客户端显示的物品
         * */
        public Variant GetClientItem(int tpid)
        {
            if(null==_clientItem)
            {
                _clientItem = new Variant();
                Variant itemArr = m_conf["clientitem"][0]["item"];
                foreach(Variant item in itemArr._arr)
                {
                    _clientItem[item["tpid"]] = item;
                }
            }
            return _clientItem[tpid];
        }
        /**
         * 装备积分
         * */
        public int GetEqppt(uint tpid)
        {
            int eqppt = -1;
            Variant item_conf = m_conf["item"][tpid];
            if( null != item_conf  && (item_conf.ContainsKey("eqppt")) )
            {
                eqppt = item_conf["eqppt"];
            }
            return eqppt;
        }
        public string get_item_icon_url(uint tpid)
        {
            if(m_conf["item"].ContainsKey(tpid.ToString()))
            {
                return m_conf["item"][tpid.ToString()]["icon"];
            }
            return "";
        }
        
        public Variant get_item_conf(uint tpid)
        {
            return m_conf["item"][tpid.ToString()];
        }

        public string get_item_replace_icon_url(uint tpid,string stype)
        {
            if(m_conf["item"].ContainsKey(tpid.ToString()))
            {
                string sUrl = m_conf["item"][tpid]["icon"];
                switch(stype)
                {
                    case "atf":
                        return sUrl.Replace(".png",m_conf["png2atf"]);
                    case "jtx":
                        return sUrl.Replace(".png",m_conf["png2jtx"]);
                    case "ptx":
                        return sUrl.Replace(".png",m_conf["png2ptx"]);
                    default:
                        return sUrl;
                }
            }
            return "";
        }
        
        /**
         *通过道具品质获得名字颜色值 
         * 
         */		
        public string GetItemNameColorByQual(uint qual)
        {
            if (m_conf["itemqual"].ContainsKey(qual.ToString()))
            {
                return m_conf["itemqual"][qual.ToString()]["color"];
            }
            return "";
        }
        
        /**
         *通过道具品质获得道具底色
         * 
         */
        public string GetItemIconBgByQual(uint qual)
        {
            if (m_conf["itemqual"].ContainsKey(qual.ToString()))//2015/07/25
            {
                return m_conf["itemqual"][qual.ToString()]["icon"];
            }
            return "";
        }
        
        /**
         *通过道具类型获得道具背景
         * 
         */		
        
        public string GetItemIconBgByTp(uint tp)
        {
            //string bg ="";
            //if(m_conf["itemIcon"].ContainsKey((int)tp))
            //{
            //    if(pos == 0)
            //    {
            //        bg = m_conf["itemIcon"][tp]["icon"];
            //    }
            //    if(pos == 3 || pos == 6 || pos == 16)
            //    {
            //        bg = m_conf["itemIcon"][tp]["weaponicon"];
            //    }
            //    if(pos == 11)
            //    {
            //        bg = m_conf["itemIcon"][tp]["wingicon"];
            //    }
            //}
            //if(null == bg || !isEqp)
            //{
            //    return m_conf["itemIcon"][tp]["icon"];
            //}
            //return bg;
            if (m_conf.ContainsKey("itemeff"))
            {
                if (m_conf["itemeff"].ContainsKey(tp.ToString()))
                {
                    return m_conf["itemeff"][tp.ToString()]._str;
                }
            }
            return "";
        }	
        /**
         * 根据道具类型/品质获得道具特效 
         */
        public Variant GetItemIconEffByTp(uint tp)
        {
            if (conf.ContainsKey("itemeff") && conf["itemeff"].ContainsKey(tp.ToString()))
            {
                return m_conf["itemeff"][tp.ToString()];
            }
            return "";
        }
        /**
         * 获得指定装备的道具效果
         * */
        public Variant GetAppointIconEff(uint tp)
        {
            if (m_conf.ContainsKey("eqpeff") && m_conf["eqpeff"].ContainsKey(tp.ToString()))
            {
                return m_conf["eqpeff"][tp.ToString()];
            }
            return "";
        }
        /**
         *获取装备强化效果配置
         * 
         */		
        public string GetItemFlvlmat( uint tpid, int flvl )
        {
            Variant itmConf = m_conf["item"][tpid];			
            if( null != itmConf && (itmConf.ContainsKey("flvlmat")) )
            {
                Variant flvlConf = m_conf["flvlmat"][ itmConf["flvlmat"] ];
                if( null != flvlConf )
                {
                    return flvlConf["flvl"][flvl]._str; 
                }
            }			
            return "";
        }
        /**
         *获取装备强化效果配置emt
         * 
         */		
        public Variant GetItemFlvlemt(uint tpid,uint flvl )
        {
            Variant itmConf = m_conf["item"][tpid];			
            if( null != itmConf && (itmConf.ContainsKey("flvlemt")) )
            {
                Variant flvlConf = m_conf["flvlemt"][ itmConf["flvlemt"] ];
                if( null != flvlConf )
                {
                    return flvlConf["attachEffect"][flvl]; 
                }
            }			
            return null;
        }
        /**
         *获得装备等级
         */	
        public uint GetItemGrade(uint tpid)
        {
            if(m_conf["item"].ContainsKey((int)tpid))
            {
                return m_conf["item"][tpid]["grade"];
            }
            return 0;
        }
        
        /**
         * 获得装备大图片用于在人物界面显示
         */		
        public string GetItemEquipUrl(uint tpid)
        {
            if(m_conf.ContainsKey("itemeqp") && m_conf["itemeqp"].ContainsKey((int)tpid))
            {
                return m_conf["itemeqp"][tpid]["icon"];
            }
            return "";
        }
       
    }
}
