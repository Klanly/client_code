using System;
using GameFramework;  using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrMarketConfig : configParser
    {
        protected Variant m_item = new Variant();
        public SvrMarketConfig( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrMarketConfig(m as ClientConfig);
        }

         /**
          * 去数组化
          */
        override protected void onData()
        {
            m_item = m_conf.clone();

            if (null != m_conf["hot"])
            {
                Variant hot = m_conf["hot"];
                m_conf.RemoveKey("hot");
                m_conf["hot"] = new Variant();
                for (int i = 0; i < hot.Count; i++)
                {
                    m_conf["hot"][hot[i]["item_id"]._str] = hot[i];
                }
            }

            if (null != m_conf["sell"])
            {
                Variant sell = m_conf["sell"];
                m_conf.RemoveKey("sell");
                m_conf["sell"] = new Variant();
                for (int i = 0; i < sell.Count; i++)
                {
                    m_conf["sell"][sell[i]["item_id"]._str] = sell[i];
                }
            }

            if (null != m_conf["hsell"])
            {
                Variant hsell = m_conf["hsell"];
                m_conf.RemoveKey("hsell");
                m_conf["hsell"] = new Variant();
                for (int i = 0; i < hsell.Count; i++)
                {
                    m_conf["hsell"][hsell[i]["item_id"]._str] = hsell[i];
                }
            }

            if (null != m_conf["bndsell"])
            {
                Variant bndsell = m_conf["bndsell"];
                m_conf.RemoveKey("bndsell");
                m_conf["bndsell"] = new Variant();
                for (int i = 0; i < bndsell.Count; i++)
                {
                    m_conf["bndsell"][bndsell[i]["item_id"]._str] = bndsell[i];
                    //m_conf["hot"][bndsell[i]["item_id"]._str] = bndsell[i];
                }
            }

            if (null != m_conf["shoppt"])
            {
                Variant shoppt = m_conf["shoppt"];
                m_conf.RemoveKey("shoppt");
                m_conf["shoppt"] = new Variant();
                for (int i = 0; i < shoppt.Count; i++)
                {
                    m_conf["hot"][shoppt[i]["item_id"]._str] = shoppt[i];
                }
            }

            if (null != m_conf["lottery"])
            {
                Variant lottery = m_conf["lottery"];
                m_conf.RemoveKey("lottery");
                m_conf["lottery"] = new Variant();
                for (int i = 0; i < lottery.Count; i++)
                {
                    m_conf["hot"][lottery[i]["item_id"]._str] = lottery[i];
                }
            }

        }

        /*商城 hot sell*/
        public Variant get_hot_items_tpid()
        {
            Variant tpidHot = new Variant();
            for (int i = 0; i < 6; i++)
            {
                tpidHot.pushBack(m_item["hot"][i]["item_id"]);
            }
            return tpidHot;
        }
        public Variant get_common_items_tpid()
        {
            Variant tpidCommon = new Variant();
            for (int i = 0; i < m_item["sell"].Count; i++)
            {
                tpidCommon.pushBack(m_item["sell"][i]["item_id"]);
            }
            return tpidCommon;
        }

        public Variant get_hot_items()
		{
            return this.m_conf["hot"];
		}
		public Variant get_sell_items()
		{
            return this.m_conf["sell"];
		}
		public Variant get_hsell_items()
		{
            return this.m_conf["hsell"];
		}
		public Variant get_bndsell_items() 
		{
            return this.m_conf["bndsell"];
		}
		public Variant get_shopppt_items()
		{
            return this.m_conf["shoppt"];
		}
		public Variant get_lottery_items()
		{
            return this.m_conf["lottery"];
		}

        /**
         * 是否有多种价格 
         * @return  int
         * 			sell 0x0001
         * 			bndsell 0x0010
         * 			shoppt 0x0100
         */
        private Variant _sellItem = new Variant();
        public int IsMultiType(int tpid)
        {
            if (0 == _sellItem.Count)
            {
                _sellItem = new Variant();
                foreach (Variant obj in m_conf["sell"].Values)
                {
                    _sellItem[obj["item_id"]._int.ToString()] = 0x0001;
                }
                if (m_conf.ContainsKey("bndsell"))
                foreach (Variant obj in m_conf["bndsell"].Values)
                {
                    if(_sellItem.ContainsKey(obj["item_id"]._int.ToString()))
                    _sellItem[obj["item_id"]._int.ToString()] |= 0x0010;
                }
                if (m_conf.ContainsKey("shoppt"))
                foreach (Variant obj in m_conf["shoppt"].Values)
                {
                    _sellItem[obj["item_id"]._int.ToString()] |= 0x0100;
                }
            }
            if(_sellItem.ContainsKey(tpid.ToString()))
			{
				return _sellItem[tpid.ToString()];
			}
            return 0;
        }

        /**
         *根据物品配置id获取商城元宝道具 
         * @param tpid
         * @return 
         * 
         */
        public Variant get_market_sellitem_by_tpid(int tpid)
        {
            if (m_conf == null || m_conf["sell"] == null)
                return null;
            Variant o = new Variant();
            for (int i = 0; i < m_item["sell"].Count; i++)
            {
                o = m_item["sell"][i];

                if (o == null)
                    continue;

                if (o["item_id"] == tpid)
                    return o;
            }

            return null;
        }

        /**
         *根据物品配置编号获得 物品在商城的买卖数据 
         */
        public Variant get_game_market_sell_data_by_tpid(int tpid)
        {
            if (m_conf == null)
                return null;

            string[] sell_array = { "sell", "bndsell", "hsell", "ptsell", "petptsell", "shoppt", "lottery" };

            foreach (string s in sell_array)
            {
                if (m_conf[s] != null && m_conf[s].ContainsKey(tpid.ToString()))
                {
                    return m_conf[s][tpid.ToString()];
                }
            }

            return null;
        }

        //获得maket配置
        public Variant GetMaketConfig()
        {
            return m_conf;
        }

        public Variant Getptsell()
        {
            return m_conf["ptsell"];
        }

        /**
         *根据物品配置编号。获得商城的Q点道具 
         */
        public Variant get_market_Psellitem_by_tpid(int tpid)
        {
            if (m_item && m_item["psell"])
            {
                foreach (int i in m_item["psell"]._arr)
                {
                    if (m_item["psell"][i]["item_id"] == tpid)
                    {
                        return m_item["psell"][i];
                    }
                }
            }
            return null;
        }
        public Variant get_market_bnd_by_tpid(int tpid)
        {
            if (m_item && m_item["bndsell"])
            {
                foreach (int i in m_item["bndsell"]._arr)
                {
                    if (m_item["bndsell"][i]["item_id"] == tpid)
                    {
                        return m_item["bndsell"][i];
                    }
                }
            }
            return null;
        }

        /**
         * 获取战盟商城物品
         */
        public Variant get_clan_items()
        {
            return m_conf["clan"];
        }
    }
} 
