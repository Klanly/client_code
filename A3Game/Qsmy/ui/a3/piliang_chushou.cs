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
using DG.Tweening;
namespace MuGame
{

    class piliang_chushou : Window
    {
        private GridLayoutGroup item_Parent_chushou;
        private Dictionary<uint, GameObject> itemcon_chushou = new Dictionary<uint, GameObject>();
        Dictionary<uint, a3_BagItemData> dic_BagItem_shll = new Dictionary<uint, a3_BagItemData>();
        public int GetMoneyNum;
        public static piliang_chushou instance;
        Text Money;
        public override void init() {
            instance = this;
            item_Parent_chushou = transform.FindChild("info_bg/scroll_view/contain").GetComponent<GridLayoutGroup>();
            Money = getComponentByPath<Text>("money");
            BaseButton btn_chushouclose = new BaseButton(transform.FindChild("close"));
            btn_chushouclose.onClick = onchoushouclose;
            BaseButton btn_chushou_do = new BaseButton(transform.FindChild("info_bg/go"));
            btn_chushou_do.onClick = SellItem;

            getComponentByPath<Text>("info_bg/Text").text = ContMgr.getCont("piliang_chushou_0");
            getComponentByPath<Text>("info_bg/topText/Text").text = ContMgr.getCont("piliang_chushou_1");
            getComponentByPath<Text>("info_bg/go/Text").text = ContMgr.getCont("piliang_chushou_2");

        }
        public override void onShowed() {

            this.transform.SetAsLastSibling(); 
            SellPutin();
            clearCon();
            OnLoadTitm_chushou();
        }
        public override void onClosed() {



        }
        void CreateItemIcon_chushou(a3_BagItemData data, int i)
        {
            uint id = data.id;
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, false, data.num);
            icon.transform.SetParent(item_Parent_chushou.transform.GetChild(i), false);
            itemcon_chushou[data.id] = icon;
            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) {
                if (data.isEquip)
                {
                    ArrayList data1 = new ArrayList();
                    a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                    data1.Add(one);
                    data1.Add(equip_tip_type.tip_forchushou );
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
                }
                else {
                    if (dic_BagItem_shll.ContainsKey(id))
                    {
                        ArrayList data1 = new ArrayList();
                        a3_BagItemData one = dic_BagItem_shll[id];
                        data1.Add(one);
                        data1.Add(equip_tip_type.tip_forchushou);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data1);
                    }

                }
            };
        }
        //一键出售加入
        void SellPutin()
        {
            foreach (uint it in a3_BagModel.getInstance().getUnEquips().Keys)
            {
                if (a3_BagModel.getInstance().HasBaoshi(a3_BagModel.getInstance().getUnEquips()[it]))
                {
                    continue;
                }
                if (a3_BagModel.getInstance().getUnEquips()[it].ismark)
                {
                    continue;
                }
                uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;
                if (a3_BagModel.getInstance().getItemDataById(tpid).quality <= 3)
                {
                    if (dic_BagItem_shll.ContainsKey(it))
                    {
                        dic_BagItem_shll.Remove(it);
                    }
                    int num = a3_BagModel.getInstance().getUnEquips()[it].num;
                    dic_BagItem_shll[it] = a3_BagModel.getInstance().getUnEquips()[it];
                    ShowMoneyCount(a3_BagModel.getInstance().getUnEquips()[it].tpid, num, true);
                }
            }
            Dictionary<uint, a3_BagItemData> itemList = a3_BagModel.getInstance().getItems();
            foreach (uint it in itemList.Keys)
            {
                uint tpid = itemList[it].tpid;
                if (itemList[it].confdata.use_type == 2 || itemList[it].confdata.use_type == 3)
                {
                    SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
                    if (PlayerModel.getInstance().up_lvl > xml.getInt("use_limit"))
                    {
                        if (dic_BagItem_shll.ContainsKey(it))
                        {
                            dic_BagItem_shll.Remove(it);
                        }
                        int num = itemList[it].num;
                        dic_BagItem_shll[it] = itemList[it];
                        ShowMoneyCount(itemList[it].tpid, num, true);
                    }
                }
            }
        }

        //取出物品
        public void outItem_chushou(uint id, int num = 1,bool isequip = false)
        {
            GameObject con = transform.FindChild("info_bg/scroll_view/icon").gameObject;
            if (isequip)
            {
                itemcon_chushou[id].transform.parent.SetAsLastSibling();
                Destroy(itemcon_chushou[id].gameObject);
                itemcon_chushou.Remove(id);
                dic_BagItem_shll.Remove(id);
                ShowMoneyCount(a3_BagModel.getInstance().getUnEquips()[id].tpid, 1, false);
            }
            else {
                if (num >= dic_BagItem_shll[id].num)
                {
                    itemcon_chushou[id].transform.parent.SetAsLastSibling();
                    Destroy(itemcon_chushou[id].gameObject);
                    itemcon_chushou.Remove(id);
                    dic_BagItem_shll.Remove(id);
                    ShowMoneyCount(a3_BagModel.getInstance().getItems()[id].tpid, num, false);
                }
                else
                {
                    a3_BagItemData one = dic_BagItem_shll[id];
                    one.num = dic_BagItem_shll[id].num - num;
                    dic_BagItem_shll[id] = one;
                    itemcon_chushou[id].transform.FindChild("num").GetComponent<Text>().text = one.num.ToString(); 
                    ShowMoneyCount(dic_BagItem_shll[id].tpid, num, false);
                }
            }
        }

        //显示出售物品
        public void OnLoadTitm_chushou()
        {
            itemcon_chushou.Clear();
            int h = 0;
            if (dic_BagItem_shll.Count > 0)
            {
                int i = 0;
                foreach (a3_BagItemData item in dic_BagItem_shll.Values)
                {
                    CreateItemIcon_chushou(item, i);
                    i++;
                }
                h = dic_BagItem_shll.Count / 6;
                if (dic_BagItem_shll.Count % 6 > 0)
                {
                    h += 1;
                }
            }
            RectTransform con = item_Parent_chushou.gameObject.GetComponent<RectTransform>();
            float childSizeY = item_Parent_chushou.cellSize.y;
            Vector2 newSize = new Vector2(con.sizeDelta.x, childSizeY * h);
            con.sizeDelta = newSize;
        }
        void clearCon()
        {
            if (itemcon_chushou.Count > 0)
            {
                foreach (GameObject it in itemcon_chushou.Values)
                {
                    Destroy(it);
                }
            }
        }

        //出售后刷新
        public void refresh_Sell()
        {
            dic_BagItem_shll.Clear();
            Money.text = 0 + "";
            GetMoneyNum = 0;
            InterfaceMgr.getInstance().close(this.uiName);
        }
        void onchoushouclose(GameObject go)
        {
            refresh_Sell();
        }
        List<Variant> dic_Itemlist = new List<Variant>();
        //开始出售
        void SellItem(GameObject go)
        {
            dic_Itemlist.Clear();
            foreach (uint i in dic_BagItem_shll.Keys)
            {
                Variant item = new Variant();
                item["id"] = i;
                item["num"] = dic_BagItem_shll[i].num;
                dic_Itemlist.Add(item);
            }
            BagProxy.getInstance().sendSellItems(dic_Itemlist);
        }
        void ShowMoneyCount(uint tpid, int num, bool add)
        {
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            if (add)
            {
                GetMoneyNum += (xml.getInt("value") * num);
            }
            else
            {
                GetMoneyNum -= (xml.getInt("value") * num);
            }
            Money.text = GetMoneyNum.ToString();
        }
    }
}
