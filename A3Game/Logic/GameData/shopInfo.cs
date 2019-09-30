using System;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class shopInfo : LGDataBase
    {
        private Variant _common = new Variant();

        public Variant _godsDct = new Variant();
        public Variant _shopGods = new Variant();
        public Variant _tpid = new Variant();
        public Variant _cost = new Variant();
        public int _buyGodsId;

        public shopInfo(muNetCleint m)
			: base(m)
		{
			
		}
        public static IObjectPlugin create(IClientBase m)
        {
            return new shopInfo(m as muNetCleint);
        }
        override public void init()
        {//
            this.g_mgr.addEventListener(GAME_EVENT.S2C_GET_DBMKT_ITM, onGetShopGodsRes);
           // this.g_mgr.addEventListener(PKG_NAME.S2C_BUY_ITEM_RES, onBuyItemRes);
            this.addEventListener(GAME_EVENT.UI_SHOP_CHOOSE, chooseShop);
            //this.addEventListener(UI_EVENT.UI_SHOP_BUY, onBuyItem);
        }
        
        public Variant shopGods
        {
            get { return _shopGods; }
        }

        private void onGetShopGods(GameEvent e)
        {
            //this.dispatchEvent(GameEvent.Create(PKG_NAME.S2C_GET_DBMKT_ITM, this, null));
        }

        private void onBuyItem(GameEvent e)
        {
            Variant item = e.data;
            //this.dispatchEvent(GameEvent.Create(PKG_NAME.S2C_BUY_ITEM_RES, this, item));
        }

        private void onGetShopGodsRes(GameEvent e)
        {
            _godsDct = e.data;
            dispatchEvent(
                GameEvent.Create(GAME_EVENT.UI_SHOP_CHOOSE, this, null)
            );
        }

        private void onBuyItemRes(GameEvent e)
        {
            Variant data = e.data;
            dispatchEvent(
                GameEvent.Create(GAME_EVENT.UI_SHOP_ACHIEVE, this, data)
            );
        }

        //初始化Hot界面,选取消息数据
        private void chooseShop(GameEvent e)
        {
            if (e.data == null)
            {
                return;
            }
            int shopIdx = e.data._int;
            if (6 >= _tpid.Count)
            {
                if (0 == _godsDct["itms"].Count)
                {
                    _tpid = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_hot_items_tpid();
                    getCommonGods();
                    int[] costint = new int[24] { 10, 10, 10, 10, 10, 10, 
                                                10, 10, 10, 10, 10, 10, 
                                                10, 10, 10, 10, 10, 10,
                                                10, 10, 10, 10, 10, 10 };
                    for (int i = 0; i < costint.Length; i++)
                    {
                        _cost[i] = costint[i];
                        _godsDct["itms"][i] = _tpid[i];
                    }
                    for (int i = 0, j = 24; i < costint.Length; i++, j++)
                    {
                        _godsDct["itms"][j] = _cost[i];
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        _tpid[i] = _godsDct["itms"][i]["tpid"];
                        _cost[i] = _godsDct["itms"][i]["yb"];
                    }
                    getCommonGods();
                    for (int j = 6; j < 24; j++)
                    {
                        _cost[j] = 10;
                    }
                }
            }

            if (!_godsDct["ybmkt"]["discnt"])
            {
                int discntint = 100;
                _godsDct["ybmkt"]["discnt"] = discntint;
            }

            int start = 6 * shopIdx, finish = 6 * (1 + shopIdx);
            _shopGods["tpid"] = new Variant();
            _shopGods["img"] = new Variant();
            _shopGods["name"] = new Variant();
            _shopGods["cost"] = new Variant();
            _shopGods["discnt"] = new Variant();

            _shopGods["discnt"] = _godsDct["ybmkt"]["discnt"];
            for (int k = start; k < finish; k++)
            {
                _shopGods["tpid"].pushBack(_tpid[k]);
                string file = (this.g_mgr.g_gameConfM as muCLientConfig).localItems.get_item_icon_url(_tpid[k]);
                _shopGods["img"].pushBack(file);
                string name = LanguagePack.getLanguageText("items_xml", Convert.ToString(_tpid[k]._int));
                _shopGods["name"].pushBack(name);
                _shopGods["cost"].pushBack(_cost[k]);
            }
            dispatchEvent(
                GameEvent.Create(GAME_EVENT.UI_SHOP_PREPARE, this, _shopGods)
            );
        }

        private void getCommonGods()
        {
            _common = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_common_items_tpid();
            for (int j = 0, k = 0; j < 18; j++)
            {
                Random ro = new Random();
                int iUp = _common.Count;
                int iDown = 0;
                int check = 0;
                k = ro.Next(iDown, iUp);
                foreach (int tpid in _tpid._arr)
                {
                    if (_common[k] != tpid)
                    {
                        check++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (check == _tpid.Length)
                {
                    _tpid.pushBack(_common[k]);
                }
                else
                {
                    j -= 1;
                }
            }
        }

        public Variant getItemQG()
        {
            Variant hot_items = new Variant();
            hot_items = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_hot_items();
            //Variant itemQG = new Variant();
            //foreach (Variant itm in hot_items._arr)
            //{
            //    if (itm.ContainsKey("")) 
            //    {
            //        itemQG._arr.Add(itm);
            //    }
            //}
            return hot_items;
        }

        public Variant getItemCY()
        {
            Variant hsell_items = new Variant();
            hsell_items = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_sell_items();
            //Variant itemCY = new Variant();
            //foreach (Variant itm in hsell_items._arr)
            //{
            //    if (itm.ContainsKey(""))
            //    {
            //        itemCY._arr.Add(itm);
            //    }
            //}
            return hsell_items;

        }
        public Variant getItemXH()
        {
            Variant bndsell_items = new Variant();
            bndsell_items = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_bndsell_items();
            //Variant itemXH = new Variant();
            //foreach (Variant itm in bndsell_items._arr)
            //{
            //    if (itm.ContainsKey(""))
            //    {
            //        itemXH._arr.Add(itm);
            //    }
            //}
            return bndsell_items;
        }

        public Variant getItemBZ()
        {
            Variant shopppt_items = new Variant();
            shopppt_items = (this.g_mgr.g_gameConfM as muCLientConfig).svrMarketConf.get_shopppt_items();
            //Variant itemBZ = new Variant();
            //foreach (Variant itm in shopppt_items._arr)
            //{
            //    if (itm.ContainsKey(""))
            //    {
            //        itemBZ._arr.Add(itm);
            //    }
            //}
            return shopppt_items;
        }
    }
}
