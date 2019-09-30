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
    class piliang_fenjie : Window
    {
        private GridLayoutGroup item_Parent_fenjie;
        Dictionary<uint, a3_BagItemData> dic_BagItem = new Dictionary<uint, a3_BagItemData>();
        private Dictionary<uint, GameObject> itemcon_fenjie = new Dictionary<uint, GameObject>();

        private Toggle white, green, blue, purple, orange, red , save;
        Text mojing;
        Text shengguanghuiji;
        Text mifageli;

        public int mojing_num;
        public int shengguanghuiji_num;
        public int mifageli_num;
        public static piliang_fenjie instance;

        List<int> jilu1 = new List<int>();

        public override void init()
        {
            instance = this;
            item_Parent_fenjie = transform.FindChild("scroll_view/contain").GetComponent<GridLayoutGroup>();
            BaseButton btn_fenjieclose = new BaseButton(transform.FindChild("close"));
            btn_fenjieclose.onClick = onfenjieclose;

            BaseButton btn_fenjie = new BaseButton(transform.FindChild("info_bg/go"));
            btn_fenjie.onClick = Sendproxy;

            mojing = getComponentByPath<Text>("info_bg/mojing/num");
            shengguanghuiji = getComponentByPath<Text>("info_bg/shenguang/num");
            mifageli = getComponentByPath<Text>("info_bg/mifa/num");
            white = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_white");
            white.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison) { EquipsSureSell(1); OnLoadItem_fenjie(); }
                else { outItemCon_fenjie(1); EquipsNoSell(1); }
            });

            green = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_green");
            green.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    EquipsSureSell(2); OnLoadItem_fenjie();
                    if (white.isOn == false)
                    {
                        white.isOn = true;
                    }
                }
                else
                {
                    outItemCon_fenjie(2); EquipsNoSell(2);
                }
            });
            blue = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_blue");
            blue.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    EquipsSureSell(3); OnLoadItem_fenjie();
                    if (green.isOn == false)
                    {
                        green.isOn = true;
                    }
                }
                else
                {
                    outItemCon_fenjie(3); EquipsNoSell(3);
                }
            });
            purple = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_puple");
            purple.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    EquipsSureSell(4); OnLoadItem_fenjie();
                    if (blue.isOn == false)
                    {
                        blue.isOn = true;
                    }
                }
                else
                {
                    outItemCon_fenjie(4); EquipsNoSell(4);
                }
            });
            orange = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_orange");
            orange.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    EquipsSureSell(5); OnLoadItem_fenjie();
                    if (purple.isOn == false)
                    {
                        purple.isOn = true;
                    }
                }
                else
                {
                    outItemCon_fenjie(5); EquipsNoSell(5);
                }
            });

            red = getComponentByPath<Toggle>("info_bg/Toggle_all/Toggle_red");
            red.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    EquipsSureSell(6); OnLoadItem_fenjie();
                    if (orange.isOn == false)
                    {
                        orange.isOn = true;
                    }
                }
                else
                {
                    outItemCon_fenjie(6); EquipsNoSell(6);
                }
            });


            save = getComponentByPath<Toggle>("info_bg/Toggle_save");
            save.onValueChanged.AddListener(delegate (bool ison)
                {
                    if (ison)
                    {
                        if (dic_BagItem != null)
                        {
                            save_fenjie(); Save();
                            if (white.isOn) jilu1.Add(1);
                            if (green.isOn)jilu1.Add(2);
                            if (blue.isOn) jilu1.Add(3);
                            if (purple.isOn)jilu1.Add(4);
                            if (orange.isOn) jilu1.Add(5);
                            if (red.isOn) jilu1.Add(6);
                        }
                    }
                    else
                    {
                        mojing_num = 0;
                        shengguanghuiji_num = 0;
                        mifageli_num = 0;
                        foreach (int t in jilu1)
                        {
                            if (t == 1) { white.isOn = true; EquipsSureSell(1); OnLoadItem_fenjie(); }
                            if (t == 2) { green.isOn = true; EquipsSureSell(2); OnLoadItem_fenjie(); }
                            if (t == 3) { blue.isOn = true; EquipsSureSell(3); OnLoadItem_fenjie(); }
                            if (t == 4) { purple.isOn = true; EquipsSureSell(4); OnLoadItem_fenjie(); }
                            if (t == 5) { orange.isOn = true; EquipsSureSell(5); OnLoadItem_fenjie(); }
                            if (t == 6) { red.isOn = true; EquipsSureSell(6); OnLoadItem_fenjie(); }
                        }
                        jilu1.Clear();
                        //    int a = orange.isOn == true ? 5 : purple.isOn == true ? 4 : blue.isOn == true ? 3 : green.isOn == true ? 2 : white.isOn == true ? 1 : 0;

                        //    switch (a)
                        //    {
                        //        case 5:
                        //            EquipsSureSell(5);
                        //            OnLoadItem_fenjie();
                        //            EquipsSureSell(4); OnLoadItem_fenjie();
                        //            EquipsSureSell(3); OnLoadItem_fenjie();
                        //            EquipsSureSell(2); OnLoadItem_fenjie();
                        //            EquipsSureSell(1); OnLoadItem_fenjie();
                        //            purple.isOn = true;
                        //            blue.isOn = true;
                        //            green.isOn = true;
                        //            white.isOn = true;
                        //            break;
                        //        case 4:
                        //            EquipsSureSell(4); OnLoadItem_fenjie();
                        //            EquipsSureSell(3); OnLoadItem_fenjie();
                        //            EquipsSureSell(2); OnLoadItem_fenjie();
                        //            EquipsSureSell(1); OnLoadItem_fenjie();
                        //            blue.isOn = true;
                        //            green.isOn = true;
                        //            white.isOn = true;
                        //            break;
                        //        case 3:
                        //            EquipsSureSell(3); OnLoadItem_fenjie();
                        //            EquipsSureSell(2); OnLoadItem_fenjie();
                        //            EquipsSureSell(1); OnLoadItem_fenjie();
                        //            green.isOn = true;
                        //            white.isOn = true;
                        //            break;
                        //        case 2:
                        //            EquipsSureSell(2); OnLoadItem_fenjie();
                        //            EquipsSureSell(1); OnLoadItem_fenjie();
                        //            white.isOn = true;
                        //            break;
                        //        default: EquipsSureSell(1); OnLoadItem_fenjie(); break;
                        //    }

                    }
                    
                });



        white.isOn = false;
            green.isOn = false;
            blue.isOn = false;
            purple.isOn = false;
            orange.isOn = false;
            red.isOn = false;
            save.isOn = false;

            getComponentByPath<Text>("info_bg/go/Text").text = ContMgr.getCont("piliang_fenjie_1");
            getComponentByPath<Text>("info_bg/Toggle_all/Toggle_white/Label").text = ContMgr.getCont("piliang_fenjie_2");
            getComponentByPath<Text>("info_bg/Toggle_all/Toggle_green/Label").text = ContMgr.getCont("piliang_fenjie_3");
            getComponentByPath<Text>("info_bg/Toggle_all/Toggle_blue/Label").text = ContMgr.getCont("piliang_fenjie_4");
            getComponentByPath<Text>("info_bg/Toggle_all/Toggle_puple/Label").text = ContMgr.getCont("piliang_fenjie_5");
            getComponentByPath<Text>("info_bg/Toggle_all/Toggle_orange/Label").text = ContMgr.getCont("piliang_fenjie_6");
            getComponentByPath<Text>("info_bg/topText/Text").text = ContMgr.getCont("piliang_fenjie_1");
            getComponentByPath<Text>("info_bg/ExplainTwo").text = ContMgr.getCont("piliang_fenjie_7");
            getComponentByPath<Text>("info_bg/Toggle_save/Label").text = ContMgr.getCont("piliang_fenjie_8");

        }
        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
            mojing_num = 0;
            shengguanghuiji_num = 0;
            mifageli_num = 0;
            //OnLoadItem_fenjie();
            foreach (int t in a3_BagModel.getInstance().jilu)
            {
                if (t == 1) { white.isOn = true; EquipsSureSell(1); OnLoadItem_fenjie(); }
                if (t == 2) { green.isOn = true; EquipsSureSell(2); OnLoadItem_fenjie(); }
                if (t == 3) { blue.isOn = true; EquipsSureSell(3); OnLoadItem_fenjie(); }
                if (t == 4) { purple.isOn = true; EquipsSureSell(4); OnLoadItem_fenjie(); }
                if (t == 5) { orange.isOn = true; EquipsSureSell(5); OnLoadItem_fenjie(); }
                if (t == 6) { red.isOn = true; EquipsSureSell(6); OnLoadItem_fenjie(); }
                if (t == 7) { save.isOn = true; save_fenjie(); Save(); }
            }
            if (a3_BagModel.getInstance().jilu.Count <= 0)
            {
                white.isOn = false; green.isOn = false; blue.isOn = false; purple.isOn = false; orange.isOn = false;red.isOn = false;

            }
            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public override void onClosed()
        {
            dic_BagItem.Clear();
            clearCon_fenjie();
            conIndex = 0;
            mojing.text = 0 + "";
            shengguanghuiji.text = 0 + "";
            mifageli.text = 0 + "";
            a3_BagModel.getInstance().jilu.Clear();
            if (white.isOn) a3_BagModel.getInstance().jilu.Add(1);
            if (green.isOn) a3_BagModel.getInstance().jilu.Add(2);
            if (blue.isOn) a3_BagModel.getInstance().jilu.Add(3);
            if (purple.isOn) a3_BagModel.getInstance().jilu.Add(4);
            if (orange.isOn) a3_BagModel.getInstance().jilu.Add(5);
            if (red.isOn) a3_BagModel.getInstance().jilu.Add(6);
            if (save.isOn) a3_BagModel.getInstance().jilu.Add(7);
            clearCon_fenjie();
        }
        void CreateItemIcon_fenjie(a3_BagItemData data, int i)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true);
            icon.transform.SetParent(item_Parent_fenjie.transform.GetChild(i), false);
            itemcon_fenjie[data.id] = icon;
            BaseButton bs_bt = new BaseButton(icon.transform);
            uint id = data.id;
            bs_bt.onClick = delegate (GameObject go)
            {
                ArrayList data1 = new ArrayList();
                a3_BagItemData one = a3_BagModel.getInstance().getItems()[id];
                data1.Add(one);
                data1.Add(equip_tip_type.tip_forfenjie);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
            };
        }
        //显示分解物品
        int conIndex = 0;
        void OnLoadItem_fenjie()
        {
            if (dic_BagItem.Count > 0)
            {
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (itemcon_fenjie.ContainsKey(it)) { continue; }
                    CreateItemIcon_fenjie(dic_BagItem[it], conIndex);
                    conIndex++;
                }
            }
            setfenjieCon();
        }
        void setfenjieCon()
        {
            int h = 0;
            if (dic_BagItem.Count > 0)
            {
                h = itemcon_fenjie.Count / item_Parent_fenjie.constraintCount;
                if (itemcon_fenjie.Count % item_Parent_fenjie.constraintCount > 0)
                {
                    h += 1;
                }
            }
            RectTransform con = item_Parent_fenjie.gameObject.GetComponent<RectTransform>();
            float childSizeY = item_Parent_fenjie.cellSize.y;
            float spacing = item_Parent_fenjie.spacing.y;
            Vector2 newSize = new Vector2(con.sizeDelta.x, (childSizeY + spacing) * h);
            con.sizeDelta = newSize;
        }

        //显示分解获得物品数量
        void showItemNum(uint tpid, bool add)
        {
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            List<SXML> xmls = xml.GetNodeList("decompose");
            foreach (SXML x in xmls)
            {
                switch (x.getInt("item"))
                {
                    case 1540:
                        if (add)
                            mojing_num += x.getInt("num");
                        else
                        {
                            mojing_num -= x.getInt("num");
                        }
                        mojing.text = mojing_num.ToString();
                        break;
                    case 1541:
                        if (add)
                            shengguanghuiji_num += x.getInt("num");
                        else
                        {
                            shengguanghuiji_num -= x.getInt("num");
                        }
                        shengguanghuiji.text = shengguanghuiji_num.ToString();
                        break;
                    case 1542:
                        if (add)
                            mifageli_num += x.getInt("num");
                        else
                        {
                            mifageli_num -= x.getInt("num");
                        }
                        mifageli.text = mifageli_num.ToString();
                        break;
                }
            }
        }
        //消去要删除的本职业装备
        public void Save()
        {
            List<uint> removelist = new List<uint>();
            foreach (uint it in dic_BagItem.Keys)
            {
                if (a3_EquipModel.getInstance().checkisSelfEquip(a3_BagModel.getInstance().getItemDataById(a3_BagModel.getInstance().getUnEquips()[it].tpid)))
                {
                    removelist.Add(it);
                    showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, false);
                }
            }
            foreach (uint i in removelist)
            {
                dic_BagItem.Remove(i);
            }
        }
        //分解物品单个去除本职业装备
        public void save_fenjie()
        {
            GameObject con = item_Parent_fenjie.transform.parent.FindChild("icon").gameObject;
            if (type != -1)
            {
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (a3_EquipModel.getInstance().checkisSelfEquip(a3_BagModel.getInstance().getItemDataById(a3_BagModel.getInstance().getUnEquips()[it].tpid)))
                    {
                        conIndex--;
                        Destroy(itemcon_fenjie[it].transform.parent.gameObject);
                        itemcon_fenjie.Remove(it);
                        GameObject clon = Instantiate(con).gameObject;
                        clon.transform.SetParent(item_Parent_fenjie.transform, false);
                        clon.SetActive(true);
                        clon.transform.SetAsLastSibling();
                    }
                }
            }
            setfenjieCon();
        }
        //批量分解取出
        public void EquipsNoSell(int quality = 0)
        {
            List<uint> removelist = new List<uint>();
            foreach (uint it in dic_BagItem.Keys)
            {
                if (dic_BagItem[it].confdata.quality == quality)
                {
                    removelist.Add(it);
                    showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, false);
                }
            }
            foreach (uint i in removelist)
            {
                dic_BagItem.Remove(i);
            }
        }
        //分解物品单个去除
        public void outItemCon_fenjie(int type = -1, uint id = 0)
        {
            GameObject con = item_Parent_fenjie.transform.parent.FindChild("icon").gameObject;
            if (type != -1)
            {
                foreach (uint it in dic_BagItem.Keys)
                {
                    if (dic_BagItem[it].confdata.quality == type)
                    {
                        conIndex--;
                        Destroy(itemcon_fenjie[it].transform.parent.gameObject);
                        itemcon_fenjie.Remove(it);
                        GameObject clon = Instantiate(con).gameObject;
                        clon.transform.SetParent(item_Parent_fenjie.transform, false);
                        clon.SetActive(true);
                        clon.transform.SetAsLastSibling();
                    }
                }
            }
            else if (id > 0)
            {
                Destroy(itemcon_fenjie[id].transform.parent.gameObject);
                itemcon_fenjie.Remove(id);
                dic_BagItem.Remove(id);
                showItemNum(a3_BagModel.getInstance().getUnEquips()[id].tpid, false);
                GameObject clon = Instantiate(con).gameObject;
                clon.transform.SetParent(item_Parent_fenjie.transform, false);
                clon.SetActive(true);
                clon.transform.SetAsLastSibling();
                conIndex--;
            }
            setfenjieCon();
        }

        //批量分解加入
        public void EquipsSureSell(int quality = 0)
        {
            foreach (uint it in a3_BagModel.getInstance().getUnEquips().Keys)
            {
                uint tpid = a3_BagModel.getInstance().getUnEquips()[it].tpid;

                if (a3_BagModel.getInstance().getItemDataById(tpid).quality == quality)
                {
                    if (!a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getUnEquips()[it]))
                    {
                        continue;
                    }
                    if (a3_EquipModel.getInstance().getEquipByAll(it).ismark)
                    {
                        continue;
                    }
                    if (a3_EquipModel.getInstance().checkisSelfEquip(a3_BagModel.getInstance().getItemDataById(tpid)) && save.isOn == true)
                    {
                        continue;
                    }
                    else
                    {
                        if (dic_BagItem.ContainsKey(it))
                        {
                            dic_BagItem.Remove(it);
                        }
                        dic_BagItem[it] = a3_BagModel.getInstance().getUnEquips()[it];
                        showItemNum(a3_BagModel.getInstance().getUnEquips()[it].tpid, true);
                    }

                }
            }
        }

        //分解后刷新信息
        public void refresh()
        {
            if (mojing_num != 0 && shengguanghuiji_num == 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing"));
            if (mojing_num == 0 && shengguanghuiji_num != 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang"));
            if (mojing_num == 0 && shengguanghuiji_num == 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num != 0 && shengguanghuiji_num != 0 && mifageli_num == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + "," + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang"));
            if (mojing_num != 0 && shengguanghuiji_num == 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + "," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num == 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang") + "," + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            if (mojing_num != 0 && shengguanghuiji_num != 0 && mifageli_num != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + mojing_num + ContMgr.getCont("a3_bag_mojing") + shengguanghuiji_num + ContMgr.getCont("a3_bag_shenguang") + mifageli_num + ContMgr.getCont("a3_bag_mifa"));
            dic_BagItem.Clear();
            clearCon_fenjie();
            conIndex = 0;
            mojing_num = 0;
            shengguanghuiji_num = 0;
            mifageli_num = 0;
            mojing.text = 0 + "";
            shengguanghuiji.text = 0 + "";
            mifageli.text = 0 + "";
            InterfaceMgr.getInstance().close(InterfaceMgr.PILIANG_FENJIE);
        }

        //开始分解
        List<uint> dic_leftAllid = new List<uint>();
        void Sendproxy(GameObject go)
        {
            dic_leftAllid.Clear();
            foreach (uint i in dic_BagItem.Keys)
            {
                dic_leftAllid.Add(i);
            }
            EquipProxy.getInstance().sendsell(dic_leftAllid);
            onfenjieclose(null);
            //this.transform.FindChild("piliang_fenjie").gameObject.SetActive(false);
        }
        //清除icon
        void clearCon_fenjie()
        {
            if (itemcon_fenjie.Count > 0)
            {
                foreach (GameObject it in itemcon_fenjie.Values)
                {
                    Destroy(it);
                }
            }
            itemcon_fenjie.Clear();
        }
        //关闭界面
        void onfenjieclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.PILIANG_FENJIE);
            //this.transform.FindChild("piliang_fenjie").gameObject.SetActive(false);
            //refresh();
        }

    }
}
