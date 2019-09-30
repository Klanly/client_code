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
using MuGame.Qsmy.model;

namespace MuGame
{

    class a3_itemLack : Window
    {
        a3_ItemData item_data = new a3_ItemData();
        GameObject toget;
        Transform Btncon;
        public string closewindow = null;
        public static a3_itemLack intans;
        private GameObject avatarobj = null;
        public ArrayList back_uidata;

        public override void init()
        {
            inText();
            intans = this;
            toget = transform.FindChild("toGet").gameObject;
            Btncon = transform.FindChild("toGet/scrollview/con");
            new BaseButton(this.transform.FindChild("close")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
            };
            new BaseButton(this.transform.FindChild("Get")).onClick = (GameObject go) =>
            {
                toget.SetActive(true);
                onShowGet();
            };
        }
        void inText()
        {
            this.transform.FindChild("info/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_1");//道具不足
            this.transform.FindChild("close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_2");//关闭
            this.transform.FindChild("Get/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_3");//获取途径
            this.transform.FindChild("toGet/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_4");//获取途径
            this.transform.FindChild("toGet/scrollview/con/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_5");//商城
            this.transform.FindChild("toGet/scrollview/con/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_6");//VIP礼包
            this.transform.FindChild("toGet/scrollview/con/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_7");//活动副本
            this.transform.FindChild("toGet/scrollview/con/4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_8");//装备分解
            this.transform.FindChild("toGet/scrollview/con/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_9");//首充
            this.transform.FindChild("toGet/scrollview/con/6/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_10");//地宫首领
            this.transform.FindChild("toGet/scrollview/con/7/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_11");//金币商店
            this.transform.FindChild("toGet/scrollview/con/8/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_12");//军团任务
            this.transform.FindChild("toGet/scrollview/con/9/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_13");//军团活动
            this.transform.FindChild("toGet/scrollview/con/10/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemLack_14");//兽灵秘境
        }

        public override void onShowed()
        {
            ToWin = false;
            transform.SetAsLastSibling();
            toget.SetActive(false);
            if (uiData == null)
                return;
            if (uiData.Count != 0)
            {
                item_data = (a3_ItemData)uiData[0];
                if (uiData.Count > 1)
                {
                    closewindow = (string)uiData[1];
                }
                if (uiData.Count > 2) {
                    avatarobj = (GameObject)uiData[2];
                }
                if (uiData.Count > 3 ) {
                    back_uidata = (ArrayList)uiData[3];
                }
            }
            Transform info = transform.FindChild("info");
            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_data);
            icon.transform.SetParent(Image, false);

            info.FindChild("name").GetComponent<Text>().text = item_data.item_name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(item_data.quality);
            info.FindChild ("desc").GetComponent<Text>().text = StringUtils.formatText(item_data.desc);

            if (avatarobj != null) {
                avatarobj.SetActive(false);
            }
        }

        bool ToWin = false;
        public override void onClosed()
        {
            if (!ToWin)//自己关闭不清空closewindow
            {
                if (closewindow != null && closewindow != "")
                    closewindow = null;
            }
            if (avatarobj != null) {
                avatarobj.SetActive(true);
                avatarobj = null;
            }
        }

        void closeObj()
        {
        }
        void onShowGet()
        {
            for (int m = 0; m < Btncon.childCount; m++)
            {
                Btncon.GetChild(m).gameObject.SetActive(false);
            }

            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + item_data.tpid);
            List<SXML> x = xml.GetNodeList("drop_info");
            if (x== null || x.Count <= 0) return;
            foreach (SXML it in x)
            {
                int drop_type = it.getInt("drop_type");
                switch (drop_type)
                {
                    case 1:
                        if (closewindow == InterfaceMgr.SHOP_A3)
                            continue;
                        Btncon.FindChild("1").gameObject.SetActive(true);
                        int shopid = it.getInt("id");
                        new BaseButton(Btncon.FindChild("1")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList data1 = new ArrayList();
                            data1.Add(shopid);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3,data1);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 2:
                        if (closewindow == InterfaceMgr.A3_VIP)
                            continue;
                        Btncon.FindChild("2").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("2")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList data1 = new ArrayList();
                            data1.Add(1);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP, data1);                              
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 3:
                        if (closewindow == InterfaceMgr.A3_COUNTERPART)
                            continue;
                        Btncon.FindChild("3").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("3")).onClick = (GameObject go) =>
                        {
                            
                            InterfaceMgr.getInstance().close(closewindow);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 4:
                        if (closewindow == InterfaceMgr.A3_BAG)
                            continue;
                        Btncon.FindChild("4").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("4")).onClick = (GameObject go) => { 
                            InterfaceMgr.getInstance().close(closewindow);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 5:
                        if (closewindow == InterfaceMgr.A3_FIRESTRECHARGEAWARD)
                            continue;
                        Btncon.FindChild("5").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("5")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_FIRESTRECHARGEAWARD);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 6:
                        if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
                        {
                            continue;
                        }
                        if (closewindow == InterfaceMgr.A3_ELITEMON)
                            continue;
                        Btncon.FindChild("6").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("6")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList arr1 = new ArrayList();
                            arr1.Add(ELITE_MONSTER_PAGE_IDX.BOSSPAGE);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON, arr1);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 7:
                        if (closewindow == InterfaceMgr.SHOP_A3)
                            continue;
                        Btncon.FindChild("7").gameObject.SetActive(true);
                        int shopid1 = it.getInt("id");
                        new BaseButton(Btncon.FindChild("7")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList data1 = new ArrayList();
                            data1.Add(shopid1);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3, data1);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;

                    case 8:
                        if (closewindow == InterfaceMgr.A3_TASK)
                            continue;
                        Btncon.FindChild("8").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("8")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            a3_task.openwin = 5;
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 9:
                        if (closewindow == InterfaceMgr.A3_SHEJIAO)
                            continue;
                        Btncon.FindChild("9").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("9")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList arr = new ArrayList();
                            arr.Add(0);
                            arr.Add(2);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arr);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                    case 10:
                        if (closewindow == InterfaceMgr.A3_SHEJIAO)
                            continue;
                        Btncon.FindChild("10").gameObject.SetActive(true);
                        new BaseButton(Btncon.FindChild("10")).onClick = (GameObject go) =>
                        {
                            InterfaceMgr.getInstance().close(closewindow);
                            ArrayList arr = new ArrayList();
                            arr.Add("summonpark");
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, arr);
                            ToWin = true;
                            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMLACK);
                        };
                        break;
                }
            }
        }
    }
}
