using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using DG.Tweening;
using System.Collections;

namespace MuGame
{
    class a3_npc_shop : Window
    {
        public static a3_npc_shop instance;
        //List<SXML> listNPCShop = new List<SXML>();
        //List<SXML> listNormal = new List<SXML>();
        //List<SXML> listChange = new List<SXML>();
        //List<SXML> listItem = new List<SXML>();
        //int sizey;
        //float cellsizey;
        //int times;
        //int npc_shopid;
        //int goodsLength;
        //List<int> goodsID = new List<int>();
       
        //GameObject itemclone;
        public int selectItemID=0;
        //public int itemType;
        //GameObject selectItem;
        //GameObject selectIcon;

        Dictionary<int, shopDatas> dic_info = new Dictionary<int, shopDatas>();
        GameObject item;
        Transform contents;
        public bool isnpcshop = false;
        public override void init()
        {

            getComponentByPath<Text>("buy/Text").text = ContMgr.getCont("a3_npc_shop_0");

            getComponentByPath<Text>("title").text = ContMgr.getCont("a3_npcshop2");            
            #region  button.onclick
            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_NPC_SHOP);
              };
            new BaseButton(getTransformByPath("buy")).onClick = (GameObject go) =>
            {
                if (selectItemID == 0)
                    flytxt.instance.fly(ContMgr.getCont("a3_npc_shop_changeitem"));
                else
                {
                    isnpcshop = true;
                    Shop_a3Proxy.getInstance().sendinfo(2, selectItemID, 1);
                }
                //A3_NPCShopProxy.getInstance().sendBuy((uint)A3_NPCShopModel.getInstance().listNPCShop[0].getInt("shop_id"), (uint)selectItemID, (uint)itemType, 1);
            };

            #endregion
            #region 事件监听
           // A3_NPCShopProxy.getInstance().addEventListener(A3_NPCShopProxy.EVENT_NPCSHOP_REFRESH, onRefresh);
           // A3_NPCShopProxy.getInstance().addEventListener(A3_NPCShopProxy.EVENT_NPCSHOP_BUY, onBuy);
           // A3_NPCShopProxy.getInstance().addEventListener(A3_NPCShopProxy.EVENT_NPCSHOP_TIME, onShowFloat);
            #endregion
            //-----------------------------------------
            item = getGameObjectByPath("panel_right/scroll_rect/changeItem");
            contents = getTransformByPath("panel_right/scroll_rect/contains");
           // haveTimes = A3_NPCShopModel.getInstance().alltimes - NetClient.instance.CurServerTimeStamp - 1;//计时
            //-----------------------------------------
            //InvokeRepeating("time", 0, 1);
            //change();
            // cloneItem(goodsLength);
        }
        
        public override void onShowed()
        {
            instance = this;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            if (uiData != null)
                dic_info = Shop_a3Model.getInstance().GetinfoByNPC_id((int)uiData[0]);
            CreatrveObj();
           // change();
           //Invoke("ShowFirstItem", 0.2f);


        }
      
        public override void onClosed()
        {
            isnpcshop = false;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            DesObj();
        }

        void DesObj()
        {
            if (contents.childCount > 0)
            {
                for(int i=0;i<contents.childCount;i++)
                {
                    Destroy(contents.GetChild(i).gameObject);
                }
            }

        }

        void CreatrveObj()
        {

            if (dic_info.Count <= 0)
                return;
            foreach(int i in dic_info.Keys)
            {
                GameObject objclone = GameObject.Instantiate(item) as GameObject;
                objclone.SetActive(true);
                objclone.transform.SetParent(contents, false);
                objclone.name = dic_info[i].id.ToString();
                RefreshInfo(dic_info[i], objclone);
                new BaseButton(objclone.transform).onClick = (GameObject go) =>
                  {
                      for (int j = 0; j < contents.childCount; j++)
                      {
                          int g = j;
                          contents.GetChild(g).FindChild("bg/bg1/select").gameObject.SetActive(false);
                      }
                      objclone.transform.FindChild("bg/bg1/select").gameObject.SetActive(true);
                      selectItemID = dic_info[i].id;
                  };
            }
        }

        void RefreshInfo(shopDatas datas,GameObject go)
        {

            Transform icon= go.transform.FindChild("bg/bg1/icon").transform;
            GameObject gos= IconImageMgr.getInstance().createA3ItemIcon((uint)datas.itemid, false, -1, 1f);
            gos.transform.SetParent(icon, false);
            Text name = go.transform.FindChild("bg/bg1/name").GetComponent<Text>();
            name.text = datas.itemName;
            Text price= go.transform.FindChild("bg/bg1/price_now").GetComponent<Text>();
            price.text = datas.value.ToString();


        }

        /*void cloneItem(int lenth)
        {
            for (int i = 0; i < lenth; i++)
            {
                itemclone = Instantiate(item);
                itemclone.transform.SetParent(contents);
                itemclone.transform.localScale = Vector3.one;
                itemclone.SetActive(true);
                itemclone.name = goodsID[i].ToString();
                Image icon = itemclone.transform.FindChild("bg/bg1/icon").GetComponent<Image>();
                Text txt = itemclone.transform.FindChild("bg/bg1/name").GetComponent<Text>();
                int nameid;
                int name;
                int money_type;
                int shop_type;
                if(goodsID[i] == selectItemID)
                    itemclone.transform.FindChild("bg/bg1/select").gameObject.SetActive(true);
                if (goodsID[i] < 5001)
                {
                    itemclone.transform.FindChild("bg/bg1/four_icon/up").gameObject.SetActive(false);
                    itemclone.transform.FindChild("bg/bg1/four_icon/down").gameObject.SetActive(false);
                    itemclone.transform.FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(false);
                    itemclone.transform.FindChild("bg/bg1/need").gameObject.SetActive(false);
                    listNormal = XMLMgr.instance.GetSXMLList("npc_shop.goods_list", "id==" + goodsID[i]);
                    itemclone.transform.FindChild("bg/bg1/limittext/limitnum").GetComponent<Text>().text = ContMgr.getCont("a3_npc_shop_no");
                    itemclone.transform.FindChild("bg/bg1/price_now").GetComponent<Text>().text = listNormal[0].getInt("value").ToString();
                    if (listNormal[0].getInt("money_type") == 3)
                    {
                        itemclone.transform.FindChild("bg/bg1/four_icon/gold").gameObject.SetActive(false);
                        itemclone.transform.FindChild("bg/bg1/four_icon/diamond").gameObject.SetActive(true);
                    }
                    else
                    {
                        itemclone.transform.FindChild("bg/bg1/four_icon/gold").gameObject.SetActive(true);
                        itemclone.transform.FindChild("bg/bg1/four_icon/diamond").gameObject.SetActive(false);
                    }
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listNormal[0].getInt("item_id"));
                    nameid = listNormal[0].getInt("item_id");
                    name = goodsID[i];
                    money_type = listNormal[0].getInt("money_type");
                    shop_type = 0;
                }
                else
                {
                    listChange = XMLMgr.instance.GetSXMLList("npc_shop.float_list", "id==" + goodsID[i]);
                    itemclone.transform.FindChild("bg/bg1/limittext/limitnum").GetComponent<Text>().text = A3_NPCShopModel.getInstance().float_list_num[(uint)listChange[0].getInt("item_id")].ToString();
                    itemclone.transform.FindChild("bg/bg1/price_now").GetComponent<Text>().text = A3_NPCShopModel.getInstance().float_list[(uint)listChange[0].getInt("item_id")].ToString();
                    itemclone.transform.FindChild("bg/bg1/need").GetComponent<Text>().text= A3_NPCShopModel.getInstance().limit_num[(uint)listChange[0].getInt("item_id")].ToString();
                    if (listChange[0].getInt("money_type") == 3)
                    {
                        itemclone.transform.FindChild("bg/bg1/four_icon/gold").gameObject.SetActive(false);
                        itemclone.transform.FindChild("bg/bg1/four_icon/diamond").gameObject.SetActive(true);
                    }
                    else
                    {
                        itemclone.transform.FindChild("bg/bg1/four_icon/gold").gameObject.SetActive(true);
                        itemclone.transform.FindChild("bg/bg1/four_icon/diamond").gameObject.SetActive(false);
                    }
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listChange[0].getInt("item_id"));
                    nameid = listChange[0].getInt("item_id");
                    name = goodsID[i];
                    money_type = listChange[0].getInt("money_type");
                    shop_type = 1;
                    for (int k = 0; k < A3_NPCShopModel.getInstance().price.Count; k++)
                    {
                        for (int j = 0; j < contents.childCount; j++)
                        {
                            if (A3_NPCShopModel.getInstance().price.ContainsKey(int.Parse(contents.GetChild(j).name)))
                            {
                                int lastprice = A3_NPCShopModel.getInstance().price[int.Parse(contents.GetChild(j).name)].lastprice;
                                int nowprice = A3_NPCShopModel.getInstance().price[int.Parse(contents.GetChild(j).name)].nowprice;

                                if (lastprice < nowprice)
                                {
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(true);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(false);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(false);
                                }
                                else if (lastprice > nowprice)
                                {
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(false);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(false);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(true);
                                }
                                else
                                {
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(false);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(false);
                                    contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(true);
                                }
                                continue;
                            }
                        }
                    }
                }
                listItem = XMLMgr.instance.GetSXMLList("item.item", "id==" + nameid);
                txt.text = listItem[0].getString("item_name");

                new BaseButton(itemclone.transform).onClick = (GameObject go) =>
                 {//nameid为当前选中的物品id
                     for (int j = 0; j < contents.childCount; j++)
                     {
                         selectItem = getGameObjectByPath("panel_right/scroll_rect/contains/" + contents.GetChild(j).name + "/bg/bg1/select");
                         //selectIcon = getGameObjectByPath("panel_right/scroll_rect/contains/" + contents.GetChild(j).name + "/bg/bg1/seicon");
                         if (int.Parse(contents.GetChild(j).name) == name)
                         {
                             selectItem.SetActive(true);
                         }
                         else
                         {
                             selectItem.SetActive(false);
                             //selectIcon.SetActive(false);
                         }

                     }
                     selectItemID = name;
                     itemType = shop_type;
                 };

                new BaseButton(itemclone.transform.FindChild("bg/bg1/icon")).onClick = (GameObject go) =>
                  {
                      for (int j = 0; j < contents.childCount; j++)
                      {
                          selectItem = getGameObjectByPath("panel_right/scroll_rect/contains/" + contents.GetChild(j).name + "/bg/bg1/select");
                          selectIcon = getGameObjectByPath("panel_right/scroll_rect/contains/" + contents.GetChild(j).name + "/bg/bg1/seicon");
                          if (int.Parse(contents.GetChild(j).name) == name)
                          {
                              selectItem.SetActive(true);
                              //selectIcon.SetActive(true);
                          }
                          else
                          {
                              selectItem.SetActive(false);
                              //selectIcon.SetActive(false);
                          }

                      }
                      selectItemID = name;

                  };
            }
        }







        /* private void ShowFirstItem()
       {
           if (contents.childCount > 0)
           {
               contents.GetChild(0).FindChild("bg/bg1/select").gameObject.SetActive(true);
               selectItemID = goodsID[0];
               if (selectItemID < 5001)
                   itemType = 0;
               else
                   itemType = 1;
           }
       }*/
        /*void change()
        {
            getComponentByPath<Text>("title").text = "NPC商店";/* A3_NPCShopModel.getInstance().listNPCShop[0].getString("shop_name");//npcshop的名字
            string str = A3_NPCShopModel.getInstance().listNPCShop[0].getString("goods_list");
            string[] goods_list = str.Split(new char[] { ',' });
            goodsLength = goods_list.Length;
            int res;
            for (int i = 0; i < goodsLength; i++)
            {
                if (int.TryParse(goods_list[i], out res))
                    goodsID.Add(res);
            }
            foreach (var item in A3_NPCShopModel.getInstance().float_list)
            {
                List<SXML> list = XMLMgr.instance.GetSXMLList("npc_shop.float_list", "item_id==" + item.Key);
                goodsID.Add(list[0].getInt("id"));
            }
            goodsLength = goodsID.Count;

            cellsizey = contents.GetComponent<GridLayoutGroup>().cellSize.y;
            float cellapcing = contents.GetComponent<GridLayoutGroup>().spacing.y;
            sizey = goodsLength / 2 + goodsLength % 2;
            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(contents.GetComponent<RectTransform>().sizeDelta.x, sizey * (cellsizey + cellapcing));
        }
    */
        /*int haveTimes;
        int min;
        int sec;
        void time()//计时器
        {

            min = haveTimes / 60;
            sec = haveTimes % 60;

            if (sec < 0 && min > 0)
            {
                min--;
                sec = 59;
            }
            else if (min <= 0 && sec < 0)
            {
                min = 0;
                sec = 0;
            }
            if (min <= 0)
            {
                min = 0;
            }
            if (haveTimes > 0)
            {
                if (sec < 10 && min > 9)
                    getComponentByPath<Text>("timego").text = ContMgr.getCont("npc_shop_1") + min + ":0" + sec;
                else if (sec > 9 && min > 9)
                    getComponentByPath<Text>("timego").text = ContMgr.getCont("npc_shop_1") + min + ":" + sec;
                else if (sec > 9 && min < 10)
                    getComponentByPath<Text>("timego").text = ContMgr.getCont("npc_shop_1") + "0" + min + ":" + sec;
                else
                    getComponentByPath<Text>("timego").text = ContMgr.getCont("npc_shop_1") + "0" + min + ":0" + sec;
                haveTimes--;
            }
            else
            {
                haveTimes = 0;
                getComponentByPath<Text>("timego").text = ContMgr.getCont("npc_shop_2");
            }

        }*/
        /*void onRefresh(GameEvent e)
        {
            A3_NPCShopProxy.getInstance().sendShowFloat((uint)A3_NPCShopModel.getInstance().listNPCShop[0].getInt("shop_id"));
        }*/
        /*void onBuy(GameEvent e)
        {
            string name = "";
            int limit_num = 0;
            int idx = e.data["shop_idx"];
            int buy_num = e.data["shop_num"];
            if (e.data["limit_num"] != null)
            {
                limit_num = e.data["limit_num"];
                int item_id = listChange[0].getInt("item_id");
                List<SXML> lis = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
                name = lis[0].getString("item_name");
                flytxt.instance.fly(ContMgr.getCont("a3_npc_shop_buy") + name + "*" + buy_num);
                A3_NPCShopProxy.getInstance().sendShowAll();
                for (int i = 0; i < contents.childCount; i++)
                {
                    if (idx == int.Parse(contents.GetChild(i).name))
                    {
                        contents.GetChild(i).FindChild("bg/bg1/limittext/limitnum").GetComponent<Text>().text = limit_num.ToString();
                        return;
                    }
                }
            }
            else
            {
                int item_id = listNormal[0].getInt("item_id");
                List<SXML> lis = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
                name = lis[0].getString("item_name");
            }
            flytxt.instance.fly(ContMgr.getCont("a3_npc_shop_buy") + name + "*" + buy_num);
        }*/
        /*void onShowFloat(GameEvent e)
        {
            haveTimes = A3_NPCShopModel.getInstance().alltimes - NetClient.instance.CurServerTimeStamp - 1;//计时
            string str = A3_NPCShopModel.getInstance().listNPCShop[0].getString("goods_list");
            string[] goods_lists = str.Split(new char[] { ',' });
            int goodsLengths = goods_lists.Length;
            int res;
            goodsID.Clear();
            for (int i = contents.childCount; i > 0; i--)
            {
                DestroyImmediate(contents.GetChild(i - 1).gameObject);
            }
            for (int i = 0; i < goodsLengths; i++)
            {
                if (int.TryParse(goods_lists[i], out res))
                    goodsID.Add(res);
            }
            foreach (var item in A3_NPCShopModel.getInstance().float_list)
            {
                List<SXML> list = XMLMgr.instance.GetSXMLList("npc_shop.float_list", "item_id==" + item.Key);
                goodsID.Add(list[0].getInt("id"));
            }
            goodsLengths = goodsID.Count;
            cloneItem(goodsLengths);
            for (int i = 0; i < A3_NPCShopModel.getInstance().price.Count; i++)
            {
                for (int j = 0; j < contents.childCount; j++)
                {
                    if (A3_NPCShopModel.getInstance().price.ContainsKey(int.Parse(contents.GetChild(j).name)))
                    {
                        int lastprice = A3_NPCShopModel.getInstance().price[int.Parse(contents.GetChild(j).name)].lastprice;
                        int nowprice = A3_NPCShopModel.getInstance().price[int.Parse(contents.GetChild(j).name)].nowprice;

                        if (lastprice < nowprice)
                        {
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(true);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(false);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(false);
                        }
                        else if (lastprice > nowprice)
                        {
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(false);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(false);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(true);
                        }
                        else
                        {
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/up").gameObject.SetActive(false);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/down").gameObject.SetActive(false);
                            contents.GetChild(j).FindChild("bg/bg1/four_icon/changeless").gameObject.SetActive(true);
                        }
                        continue;
                    }
                }
            }
        }*/
    }
}
