using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
namespace MuGame
{
    class a3_warehouse : Window
    {
        private GameObject itemListView;
        private GameObject houseListView;
        private GridLayoutGroup item_Parent;
        private GridLayoutGroup house_Parent;
        ScrollControler scrollControler;

        private Dictionary<uint, GameObject> itemicon = new Dictionary<uint, GameObject>();
        private Dictionary<uint, GameObject> houseicon = new Dictionary<uint, GameObject>();

        Scrollbar open_bar;
        public int cur_num = 1;
        bool isbag_open = false; //区分包裹和仓库
        int open_choose_tag = 1;

        bool is_auto = false;
        Text money;
        Text gold;
        Text coin;
        Text textPageIndex_right;
        Text textPageIndex_left;

        int pageIndex_right = 1;//当前页数       
        int maxPageNum_right = 6;//最大页数

        int pageIndex_left = 1;//当前页数       
        int maxPageNum_left = 6;//最大页数

        public override void init()
        {
            money = transform.FindChild("money_bg/money").GetComponent<Text>();
            gold = transform.FindChild("gem_bg/stone").GetComponent<Text>();
            coin = transform.FindChild("bdgem_bg/bindstone").GetComponent<Text>();
            textPageIndex_right = this.getComponentByPath<Text>("page_right/Text");
            textPageIndex_left= this.getComponentByPath<Text>("page_left/Text");

            itemListView = transform.FindChild("bag_scroll/scroll_view/contain").gameObject;
            item_Parent = itemListView.GetComponent<GridLayoutGroup>();
            houseListView = transform.FindChild("house_scroll/scroll_view/contain").gameObject;
            house_Parent = houseListView.GetComponent<GridLayoutGroup>();
            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onclose;
            BaseButton close_btn = new BaseButton(transform.FindChild("close_btn"));
            close_btn.onClick = onclose;
            new BaseButton(transform.FindChild("money_bg/money/add_money")).onClick = (GameObject go) => {
              
              
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                a3_exchange.Instance.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
            };
            new BaseButton(transform.FindChild("gem_bg/stone/add_stone")).onClick = (GameObject go) => {
                          
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                a3_Recharge.Instance?.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
            };           
            new BaseButton(transform.FindChild("bdgem_bg/bindstone/add_bangstone")).onClick = (GameObject go) =>
            {             
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            };
            BaseButton btn_open = new BaseButton(transform.FindChild("panel_open/open"));
            btn_open.onClick = onOpenLock;

            BaseButton btn_close_open = new BaseButton(transform.FindChild("panel_open/close"));
            btn_close_open.onClick = onCloseOpen;

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("bag_scroll/scroll_view").GetComponent<ScrollRect>();
            scrollControler.create(scroll);
            open_bar = transform.FindChild("panel_open/Scrollbar").GetComponent<Scrollbar>();
            open_bar.onValueChanged.AddListener(onNumChange);

            for (int i = 50; i < itemListView.transform.childCount; i++)
            {
                if (i >= a3_BagModel.getInstance().curi)
                {
                    GameObject lockig = itemListView.transform.GetChild(i).FindChild("lock").gameObject;
                    lockig.SetActive(true);
                    int tag = i + 1;
                    BaseButton btn = new BaseButton(lockig.transform);
                    btn.onClick = delegate(GameObject go) { this.onClickOpenBagLock(lockig, tag); };
                }
            }
            for (int i = 10; i < houseListView.transform.childCount; i++)
            {
                if (i >= a3_BagModel.getInstance().house_curi)
                {
                    GameObject lockig = houseListView.transform.GetChild(i).FindChild("lock").gameObject;
                    lockig.SetActive(true);
                    int tag = i + 1;
                    BaseButton btn = new BaseButton(lockig.transform);
                    btn.onClick = delegate(GameObject go) { this.onClickOpenHouseLock(lockig, tag); };
                }
            }
            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                int tag = i;
                tog.onValueChanged.AddListener(delegate(bool isOn)
                {
                    open_choose_tag = tag;
                    checkNumChange();
                });
            }
            Toggle auto = transform.FindChild("auto").GetComponent<Toggle>();
            auto.onValueChanged.AddListener(delegate(bool isOn)
            {
                is_auto = isOn;
            });


            new BaseButton(transform.FindChild("page_right/right")).onClick = (GameObject go) =>
            {
                if(pageIndex_right< maxPageNum_right)
                pageIndex_right++;
               
                show_page_right();

            };
            new BaseButton(transform.FindChild("page_right/left")).onClick = (GameObject go) =>
            {
                if(pageIndex_right>1)
                pageIndex_right--;
                show_page_right();
            };

            new BaseButton(transform.FindChild("page_left/right")).onClick = (GameObject go) =>
            {
                if (pageIndex_left < maxPageNum_left)
                    pageIndex_left++;
                show_page_left();
            };
            new BaseButton(transform.FindChild("page_left/left")).onClick = (GameObject go) =>
            {
                if (pageIndex_left >1)
                    pageIndex_left--;
                show_page_left();
            };
            InvokeRepeating("OnShowAchievementPage_right", 0, 0.3f);
            InvokeRepeating("OnShowAchievementPage_left", 0, 0.3f);


            getComponentByPath<Text>("auto/Text").text = ContMgr.getCont("a3_warehouse_0");
            getComponentByPath<Text>("close_btn/Text").text = ContMgr.getCont("a3_warehouse_1");
            getComponentByPath<Text>("panel_open/Text").text = ContMgr.getCont("a3_warehouse_2");
            getComponentByPath<Text>("panel_open/open/Text").text = ContMgr.getCont("a3_warehouse_3");
            getComponentByPath<Text>("panel_open/open_choose/Toggle1/Label").text = ContMgr.getCont("a3_warehouse_4");
            getComponentByPath<Text>("panel_open/open_choose/Toggle2/Label").text = ContMgr.getCont("a3_warehouse_5");
            getComponentByPath<Text>("panel_open/title/Text").text = ContMgr.getCont("a3_warehouse_6");
        }
        private void OnShowAchievementPage_right()
        {
            float yy = itemListView.GetComponent<RectTransform>().anchoredPosition.y;
            float y1 = transform.FindChild("bag_scroll/scroll_view/icon").GetComponent<RectTransform>().sizeDelta.y;
            if (yy < y1)
            {
                pageIndex_right = 1;
                textPageIndex_right.text = 1 + "/" + maxPageNum_right;
                return;
            }
            for (int i = 2; i <= maxPageNum_right; i++)
            {
                if (yy >= y1 && yy >= 5*y1*i-(8*y1) && yy < 5*y1*(i+1)-(8*y1))
                {
                    pageIndex_right = i;
                    textPageIndex_right.text = i + "/" + maxPageNum_right;
                }
            }

        }
        private void OnShowAchievementPage_left()
        {
            float yy = houseListView.GetComponent<RectTransform>().anchoredPosition.y;
            float y1 = transform.FindChild("house_scroll/scroll_view/icon").GetComponent<RectTransform>().sizeDelta.y;
            if (yy < y1)
            {
                pageIndex_left = 1;
                textPageIndex_left.text = 1 + "/" + maxPageNum_left;
                return;
            }
            for (int i = 2; i <= maxPageNum_left; i++)
            {
                if (yy >= y1 && yy >= 5 * y1 * i - (8 * y1) && yy < 5 * y1 * (i + 1) - (8 * y1))
                {
                    pageIndex_left = i;
                    textPageIndex_left.text = i + "/" + maxPageNum_left;
                }
            }

        }
        private void show_page_left()
        {
            float y1 = transform.FindChild("house_scroll/scroll_view/icon").GetComponent<RectTransform>().sizeDelta.y;
            textPageIndex_left.text = pageIndex_left + "/" + maxPageNum_left;
            houseListView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (pageIndex_left-1)*5 * y1, 0);

        }

        private void show_page_right()
        {
            float y1 = transform.FindChild("bag_scroll/scroll_view/icon").GetComponent<RectTransform>().sizeDelta.y;
            textPageIndex_right.text = pageIndex_right + "/" + maxPageNum_right;
            itemListView.GetComponent<RectTransform>().anchoredPosition = new Vector3(2.5f, (pageIndex_right-1) * 5 * y1, 0);
        }

        public override void onShowed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, onItemChange);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenBagLockRec);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_OPEN_HOUSELOCK, onOpenHouseLockRec);
            BagProxy.getInstance().addEventListener(BagProxy.EVENT_ROOM_TURN, onRoomTurn);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            onOpenBagLockRec(null);
            onOpenHouseLockRec(null);
            refreshGold();
            refreshGift();
            refreshMoney();
            onLoadItem();
            open_choose_tag = 1;
            for (int i = 1; i <= 2; i++)
            {
                Toggle tog = transform.FindChild("panel_open/open_choose/Toggle" + i).GetComponent<Toggle>();
                if (i == open_choose_tag)
                    tog.isOn = true;
                else
                    tog.isOn = false;
            }

            transform.FindChild("auto").GetComponent<Toggle>().isOn = false;
            is_auto = false;
        }
        //public void refreshGold()
        //{
        //    money.text = Globle.getBigText(PlayerModel.getInstance().gold);
        //}
        //public void refreshGift()
        //{
        //    gold.text = PlayerModel.getInstance().gift.ToString();
        //}
        //public void refreshCoin()
        //{
        //    coin.text = Globle.getBigText(PlayerModel.getInstance().money);
        //}

        public void refreshMoney()
        {
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            coin.text = PlayerModel.getInstance().gift.ToString();
        }

        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshMoney();
            }
            if (info.ContainsKey("yb"))
            {
                refreshGold();
            }
            if (info.ContainsKey("bndyb"))
            {
                refreshGift();
            }
        }
        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ITEM_CHANGE, onItemChange);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_OPEN_BAGLOCK, onOpenBagLockRec);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_OPEN_HOUSELOCK, onOpenHouseLockRec);
            BagProxy.getInstance().removeEventListener(BagProxy.EVENT_ROOM_TURN, onRoomTurn);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            foreach (GameObject go in itemicon.Values)
            {
                Destroy(go);
            }
            itemicon.Clear();
            foreach (GameObject go in houseicon.Values)
            {
                Destroy(go);
            }
            houseicon.Clear();
            if (a3_bag.indtans)
            {
                if (a3_bag.indtans.isbagToCK)
                {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
                    a3_bag.indtans.isbagToCK = false;
                }
            }
        }
        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_WAREHOUSE);

            if (A3_Smithy.Instance?.gameObject.activeSelf ?? false)
                return;


        }
        public void onLoadItem()
        {
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            int i = 0;
            foreach (a3_BagItemData item in items.Values)
            {
                CreateItemIcon(item, item_Parent.transform.GetChild(i), false);
                i++;
            }

            Dictionary<uint, a3_BagItemData> houseitems = a3_BagModel.getInstance().getHouseItems();
            int j = 0;
            foreach (a3_BagItemData item in houseitems.Values)
            {
                CreateItemIcon(item, house_Parent.transform.GetChild(j), true);
                j++;
            }
        }

        void onItemChange(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("add"))
            {
                foreach (Variant item in data["add"]._arr)
                {
                    uint id = item["id"];
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                    {
                        if (item_Parent.transform.GetChild(itemicon.Count).childCount > 1)
                        {
                            for (int i = 0;i< item_Parent.transform.GetChild(itemicon.Count).childCount;i++)
                            {
                                if (item_Parent.transform.GetChild(itemicon.Count).GetChild(i).name == "lock")
                                {
                                    continue;
                                }
                                else
                                {
                                    Destroy(item_Parent.transform.GetChild(itemicon.Count).GetChild(i).gameObject);
                                }
                            }
                        }
                        a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                        CreateItemIcon(one, item_Parent.transform.GetChild(itemicon.Count), false);
                    }
                }
            }
            if (data.ContainsKey("modcnts"))
            {
                foreach (Variant item in data["modcnts"]._arr)
                {
                    uint id = item["id"];
                    if (itemicon.ContainsKey(id))
                    {
                        itemicon[id].transform.FindChild("num").GetComponent<Text>().text = item["cnt"];
                        if ((int)item["cnt"] <= 1)
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(false);
                        else
                            itemicon[id].transform.FindChild("num").gameObject.SetActive(true);
                    }
                }
            }
            if (data.ContainsKey("rmvids"))
            {
                foreach (uint itemid in data["rmvids"]._arr)
                {
                    uint id = itemid;

                    if (itemicon.ContainsKey(id))
                    {
                        GameObject go = itemicon[id].transform.parent.gameObject;
                        Destroy(go);
                        itemicon.Remove(id);

                        GameObject item = transform.FindChild("bag_scroll/scroll_view/icon").gameObject;
                        GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
                        itemclone.SetActive(true);
                        itemclone.transform.SetParent(item_Parent.transform, false);
                        itemclone.transform.SetSiblingIndex(itemicon.Count + 1);
                    }
                }
            }
        }

        void onRoomTurn(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("add"))
            {
                foreach (Variant item in data["add"]._arr)
                {
                    uint id = item["id"];
                    if (a3_BagModel.getInstance().getHouseItems().ContainsKey(id))
                    {
                        a3_BagItemData one = a3_BagModel.getInstance().getHouseItems()[id];
                        CreateItemIcon(one, house_Parent.transform.GetChild(houseicon.Count), true);
                    }
                }
            }
            if (data.ContainsKey("modcnts"))
            {
                foreach (Variant item in data["modcnts"]._arr)
                {
                    uint id = item["id"];
                    if (houseicon.ContainsKey(id))
                    {
                        houseicon[id].transform.FindChild("num").GetComponent<Text>().text = item["cnt"];
                        if ((int)item["cnt"] <= 1)
                            houseicon[id].transform.FindChild("num").gameObject.SetActive(false);
                        else
                            houseicon[id].transform.FindChild("num").gameObject.SetActive(true);
                    }
                }
            }
            if (data.ContainsKey("rmvids"))
            {
                foreach (uint itemid in data["rmvids"]._arr)
                {
                    uint id = itemid;

                    if (houseicon.ContainsKey(id))
                    {
                        GameObject go = houseicon[id].transform.parent.gameObject;
                        Destroy(go);
                        houseicon.Remove(id);

                        GameObject item = transform.FindChild("house_scroll/scroll_view/icon").gameObject;
                        GameObject itemclone = ((GameObject)GameObject.Instantiate(item));
                        itemclone.SetActive(true);
                        itemclone.transform.SetParent(house_Parent.transform, false);
                        itemclone.transform.SetSiblingIndex(houseicon.Count + 1);
                    }
                }
            }
        }

        void onOpenBagLockRec(GameEvent e)
        {
            for (int i = 50; i < itemListView.transform.childCount; i++)
            {
                GameObject lockig = itemListView.transform.GetChild(i).FindChild("lock").gameObject;
                if (i >= a3_BagModel.getInstance().curi)
                {
                    lockig.SetActive(true);
                }
                else
                {
                    lockig.SetActive(false);
                }
            }
        }

        void onOpenHouseLockRec(GameEvent e)
        {
            for (int i = 10; i < houseListView.transform.childCount; i++)
            {
                GameObject lockig = houseListView.transform.GetChild(i).FindChild("lock").gameObject;
                if (i >= a3_BagModel.getInstance().house_curi)
                {
                    lockig.SetActive(true);
                }
                else
                {
                    lockig.SetActive(false);
                }
            }
        }
        uint nextid = 0;
        uint nextid1 = 0;
        void CreateItemIcon(a3_BagItemData data, Transform parent, bool ishouse = false)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
            icon.transform.SetParent(parent, false);
            if (ishouse)
                houseicon[data.id] = icon;
            else
                itemicon[data.id] = icon;

            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);

            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate(GameObject go) { this.onItemClick(icon, data.id, ishouse); };
        }

        void onItemClick(GameObject go, uint id, bool ishouse)
        {
            a3_BagItemData one;
            if (ishouse)
            {//点击仓库
                one = a3_BagModel.getInstance().getHouseItems()[id];
                if (is_auto)
                {
                    if (nextid != id)
                    {
                        BagProxy.getInstance().sendRoomItems(false, one.id, one.num);
                        nextid = id;
                    }
                }
                else
                {
                    if (one.isEquip)
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        data.Add(equip_tip_type.HouseOut_tip);
                        data.Add(this.uiName);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
                    }
                    else if (one.isSummon)
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3TIPS_SUMMON, data);
                    }
                    else
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        data.Add(equip_tip_type.HouseOut_tip);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                    }
                }
            }
            else
            {//点击包裹
                one = a3_BagModel.getInstance().getItems()[id];
                if (is_auto&& (a3_BagModel.getInstance().house_curi - a3_BagModel.getInstance().getHouseItems().Count) >= 1)
                {
                    if (nextid1 != id)
                    {
                        BagProxy.getInstance().sendRoomItems(true, one.id, one.num);
                        nextid1 = id;
                    }
                }
                else if (!is_auto)
                {

                    if (one.isNew)
                    {
                        one.isNew = false;
                        a3_BagModel.getInstance().addItem(one);
                        itemicon[id].transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
                    }

                    if (one.isEquip)
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        data.Add(equip_tip_type.HouseIn_tip);
                        data.Add(this.uiName);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
                    }
                    else if (one.isSummon)
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3TIPS_SUMMON, data);
                    }
                    else
                    {
                        ArrayList data = new ArrayList();
                        data.Add(one);
                        data.Add(equip_tip_type.HouseIn_tip);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                    }
                }
                else if ((a3_BagModel.getInstance().house_curi - a3_BagModel.getInstance().getHouseItems().Count) < 1)
                {
                    //flytxt.instance.fly(ContMgr.getCont("cangkuyiman"));

                        BagProxy.getInstance().sendRoomItems(true, one.id, one.num);
                   
                }
            }
        }
        void onClickOpenBagLock(GameObject go, int tag)
        {
            isbag_open = true;
            transform.FindChild("panel_open").gameObject.SetActive(true);
            cur_num = tag - a3_BagModel.getInstance().curi;
            Debug.Log("我是tag                              " + tag);
            Debug.Log("我是下面那个                              " + a3_BagModel.getInstance().curi);
            needEvent = false;
            open_bar.value = (float)cur_num / (150 - a3_BagModel.getInstance().curi);
            checkNumChange();
        }
        void onClickOpenHouseLock(GameObject go, int tag)
        {
            isbag_open = false;
            transform.FindChild("panel_open").gameObject.SetActive(true);
            cur_num = tag - a3_BagModel.getInstance().house_curi;
            needEvent = false;
            open_bar.value = (float)cur_num / (150 - a3_BagModel.getInstance().house_curi);
            checkNumChange();
        }
        bool needEvent = true;
        void onNumChange(float rate)
        {
            if (!needEvent)
            {
                needEvent = true;
                return;
            }
            if (isbag_open)
            {
                cur_num = (int)Math.Floor(rate * (150 - a3_BagModel.getInstance().curi));
            }
            else
            {
                cur_num = (int)Math.Floor(rate * (150 - a3_BagModel.getInstance().house_curi));
            }
            if (cur_num == 0)
                cur_num = 1;
            checkNumChange();
        }
        void checkNumChange()
        {
            string str = " ";
            transform.FindChild("panel_open/num").GetComponent<Text>().text = cur_num.ToString();
            switch (open_choose_tag)
            {
                case 1:
                    str = ContMgr.getCont("a3_warehouse0", new List<string>() { (5 * cur_num).ToString(), cur_num.ToString() });
                    break;
                case 2:
                    str = ContMgr.getCont("a3_warehouse1", new List<string>() { (5 * cur_num).ToString(), cur_num.ToString() });
                    break;
            }
            transform.FindChild("panel_open/desc").GetComponent<Text>().text = str;
        }
        void onOpenLock(GameObject go)
        {
            transform.FindChild("panel_open").gameObject.SetActive(false);
            if (isbag_open)
            {
                if (open_choose_tag == 1)
                    BagProxy.getInstance().sendOpenLock(2, cur_num, true);
                else
                    BagProxy.getInstance().sendOpenLock(2, cur_num, false);
            }
            else
            {
                if (open_choose_tag == 1)
                    BagProxy.getInstance().sendOpenLock(3, cur_num, true);
                else
                    BagProxy.getInstance().sendOpenLock(3, cur_num, false);
            }
            
            
        }
        void onCloseOpen(GameObject go)
        {
            transform.FindChild("panel_open").gameObject.SetActive(false);
        }
    }
}
