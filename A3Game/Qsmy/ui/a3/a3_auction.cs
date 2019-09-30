using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using UnityEngine.EventSystems;
using System.Collections;

namespace MuGame
{
    class a3_auction : Window
    {
        #region variable
        //a3BaseAuction _currentAuction = null;
        //Dictionary<string, a3BaseAuction> _activies = new Dictionary<string, a3BaseAuction>();
        TabControl _tabCtl;
        public Transform etra;
        public Transform djtip;
        List<Transform> txetClon = new List<Transform>();
        #endregion

        #region public
        public static a3_auction instance;
        public override void init()
        {
            instance = this;
            inText();
            //初始化
            SetCop();

            //== 配置(where [$name] -> tabs/$name == contents/$name)
            //_activies["buy"] = new a3_auction_buy(this, "contents/buy");
            //_activies["sell"] = new a3_auction_sell(this, "contents/sell");
            //_activies["rack"] = new a3_auction_rack(this, "contents/rack");
            //_activies["get"] = new a3_auction_get(this, "contents/get");


            //== 布局
            _tabCtl = InitLayout();
            //== 关闭窗口
            for (int i = 0; i <= 6; i++)
            {
                txetClon.Add(transform.FindChild("eqtip/info/attr_scroll/scroll/text" + i));
            }
        }

        void inText()
        {
            this.transform.FindChild("tabs/buy/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_1");
            this.transform.FindChild("tabs/sell/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_2");
            this.transform.FindChild("tabs/rack/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_3");
            this.transform.FindChild("tabs/get/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_4");
        }

        public override void onShowed()
        {
            //if (_tabCtl.getSeletedIndex() > 0) _tabCtl.setSelectedIndex(0);

            //if (_currentAuction != null)
            //{
            //    _currentAuction.onShowed();
            //}
            if (uiData != null)
            {
                int showtabId = (int)uiData[0];
                _tabCtl.setSelectedIndex(showtabId, true);

            }
            else
            {
                _tabCtl.setSelectedIndex(0,true);
            }


            //print("收到的拍卖行的下表是多少：" + _tabCtl.getSeletedIndex());
            A3_AuctionProxy.getInstance().SendSearchMsg();//SendMyRackMsg
            GRMap.GAME_CAMERA.SetActive(false);
        }

        public override void onClosed()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_GET);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_SELL);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_RACK);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_BUY);

            //if (_currentAuction != null) _currentAuction.onClose();
            GRMap.GAME_CAMERA.SetActive(true);
        }

        public void ShowDJTip(a3_BagItemData data, bool puton = true, bool buy = false)//djtip
        {
            var info = djtip.FindChild("info");
            if (puton)
            {
                info.FindChild("put").gameObject.SetActive(true);
            }
            else
            {
                info.FindChild("put").gameObject.SetActive(false);
            }
            if (buy)
            {
                info.FindChild("buy").gameObject.SetActive(true);
            }
            else
            {
                info.FindChild("buy").gameObject.SetActive(false);
            }
            for (int i = 1; i <= 5; i++)
            {
                if (i == data.confdata.quality)
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(true);
                }
                else
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(false);
                }
            }
            info.FindChild("money").GetComponent<Text>().text = ContMgr.getCont("a3_auction_jiazhi") + data.confdata.value;
            info.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
            info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_auction_kejiaoyi");
            info.FindChild("profession").GetComponent<Text>().text = ContMgr.getCont("a3_auction_cailiao");
            var s_xml = a3_BagModel.getInstance().getItemXml((int)data.tpid);

            info.FindChild("desc").GetComponent<Text>().text = StringUtils.formatText(s_xml.getString("desc"));
            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                GameObject.Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = null;
            icon = IconImageMgr.getInstance().createA3ItemIcon(data.tpid);
            icon.transform.SetParent(Image, false);

        }
        public void ShowEPTip(a3_BagItemData data, bool puton = true, bool buy = false)
        {
            var info = etra.FindChild("info");
            if (puton)
            {
                info.FindChild("put").gameObject.SetActive(true);
            }
            else
            {
                info.FindChild("put").gameObject.SetActive(false);
            }
            if (buy)
            {
                info.FindChild("buy").gameObject.SetActive(true);
            }
            else
            {
                info.FindChild("buy").gameObject.SetActive(false);
            }
            for (int i = 1; i <= 5; i++)
            {
                if (i == data.confdata.quality)
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(true);
                }
                else
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(false);
                }
            }
            info.FindChild("money").GetComponent<Text>().text = ContMgr.getCont("a3_auction_jiazhi") + data.confdata.value;
            info.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
            info.FindChild("txt_value").GetComponent<Text>().text = data.equipdata.combpt.ToString();
            info.FindChild("lv").GetComponent<Text>().text = data.equipdata.stage.ToString() + ContMgr.getCont("a3_auction_jie") + Globle.getEquipTextByType(data.confdata.equip_type);
            if (data.confdata.equip_type < 1)
            {
                info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_auction_kejiaoyi");
            }
            else
            {
                if (a3_BagModel.getInstance().isWorked(data))
                {
                    info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_auction_kejiaoyi");
                }
                else
                {
                    info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_auction_nokejiaoyi");
                }
            }

            string str = "";
            switch (data.confdata.job_limit)
            {
                case 1:
                   // str = "职业：全职业";
                    str = ContMgr.getCont("a3_auction0");
                    break;
                case 2:
                   // str = "职业：狂战士";
                    str = ContMgr.getCont("a3_auction1");
                    break;
                case 3:
                   // str = "职业：法师";
                    str = ContMgr.getCont("a3_auction2");
                    break;
                case 5:
                    //str = "职业：暗影";
                    str = ContMgr.getCont("a3_auction3");
                    break;
            }
            info.FindChild("profession").GetComponent<Text>().text = str;
            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                GameObject.Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = null;
            if (data.confdata.equip_type > 0)
            {
                icon = IconImageMgr.getInstance().createA3EquipIcon(data);
            }
            else
                icon = IconImageMgr.getInstance().createA3ItemIcon(data.tpid);
            icon.transform.SetParent(Image, false);
            icon.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);

            SetStar(etra.FindChild("info/stars"), data.equipdata.intensify_lv);

            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + data.equipdata.blessing_lv);
            info.FindChild("zhufu/title").GetComponent<Text>().text = ContMgr.getCont("a3_auction_zhufu") + data.equipdata.blessing_lv
                + ContMgr.getCont("a3_auction_need") + blessing.getInt ("blessing_att") + "%）";
            if (data.confdata.equip_type <= 0)//装备还是材料的判定条件
            {
                info.FindChild("zhufu/title").GetComponent<Text>().text = ContMgr.getCont("a3_auction_zhufu0");
                info.FindChild("zhufu/text1").GetComponent<Text>().text = ContMgr.getCont("a3_auction_liliang0");
                info.FindChild("zhufu/text2").GetComponent<Text>().text = ContMgr.getCont("a3_auction_mingjie0");
                SetStar(etra.FindChild("info/stars"), 0);
                info.FindChild("name").GetComponent<Text>().text = "";
                info.FindChild("txt_value").GetComponent<Text>().text = "";
                info.FindChild("lv").GetComponent<Text>().text = ContMgr.getCont("a3_auction_cailiao1");
                info.FindChild("profession").GetComponent<Text>().text = ContMgr.getCont("a3_auction_cailiao");
                info.FindChild("attr_scroll").gameObject.SetActive(false);
            }
            else
            {
                info.FindChild("attr_scroll").gameObject.SetActive(true);
                initAtt(info, data);
            }

        }
        #endregion

        #region private
        void SetCop()
        {
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION);
            };
            etra = transform.FindChild("eqtip");
            djtip = transform.FindChild("djtip");
            new BaseButton(etra.transform.FindChild("touch")).onClick = (GameObject) =>
            {
                etra.gameObject.SetActive(false);
            };
            new BaseButton(etra.transform.FindChild("info/cancel")).onClick = (GameObject) =>
            {
                etra.gameObject.SetActive(false);
            };
            new BaseButton(djtip.transform.FindChild("touch")).onClick = (GameObject) =>
            {
                djtip.gameObject.SetActive(false);
            };
            new BaseButton(djtip.transform.FindChild("info/cancel")).onClick = (GameObject) =>
            {
                djtip.gameObject.SetActive(false);
            };
        }
        TabControl InitLayout()
        {
            TabControl tab = new TabControl();
            tab.create(getGameObjectByPath("tabs"), this.gameObject);
            //List<Transform> contents = new List<Transform>();
            //Transform contentsRoot = getGameObjectByPath("contents").transform;
            //foreach (var v in contentsRoot.GetComponentsInChildren<Transform>(true))
            //{
            //    if (v.parent == contentsRoot)
            //    {
            //        v.gameObject.SetActive(false);
            //        contents.Add(v);
            //    }
            //}
            tab.onClickHanle += (TabControl tb) =>
            {
                //if (!_activies.ContainsKey(contents[tb.getSeletedIndex()].name))
                //{
                //    debug.Log("没有界面配置");
                //    if (_activies.Count > 0)
                //        tb.setSelectedIndex(tb.getIndexByName(new List<a3BaseAuction>(_activies.Values)[0].pathStrName));
                //    return;
                //}
                //for (int i = 0; i < contents.Count; i++)
                //{
                //    if (i != tb.getSeletedIndex()) contents[i].gameObject.SetActive(false);
                //    else contents[i].gameObject.SetActive(true);
                //}
                //if (_currentAuction != null)
                //{
                //    _currentAuction.onClose();
                //}
                //if (_currentAuction != null && _activies.ContainsKey(contents[tb.getSeletedIndex()].name))
                //{
                //    _currentAuction = _activies[contents[tb.getSeletedIndex()].name];
                //    _currentAuction.onShowed();
                //}
                //else _currentAuction = _activies[contents[tb.getSeletedIndex()].name];
                switch (tb.getSeletedIndex())
                {
                    case 0:
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_GET);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_SELL);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_RACK);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION_BUY);
                        break;
                    case 1:
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_BUY);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_GET);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_RACK);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION_SELL);
                        break;
                    case 2:
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_GET);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_SELL);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_BUY);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION_RACK);
                        break;
                    case 3:
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_SELL);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_RACK);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUCTION_BUY);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION_GET);
                        break;
                    default:
                        break;
                }
            };
           
            return tab;
        }
        void initAtt(Transform info, a3_BagItemData equip_data)
        {
            int conMon = 0;
            Transform AttCon = info.transform.FindChild("attr_scroll/scroll/contain");
            for (int i = 0; i < AttCon.transform.childCount; i++)
            {
                Destroy(AttCon.GetChild(i).gameObject);
            }
            SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).GetNode("stage_info", "itemid==" + equip_data.tpid);


           

            #region 祝福
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + equip_data.equipdata.blessing_lv);
            string[] list_need1 = stage_xml.getString("equip_limit1").Split(',');
            string[] list_need2 = stage_xml.getString("equip_limit2").Split(',');
            int need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            int need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            string text_need1, text_need2;
            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])])
            {
                if (need1 <= 0)
                {
                    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + "-";
                }
                else
                {
                    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
                }
            }
            else
            {
                if (need1 <= 0)
                {
                    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + "-";
                }
                else
                {
                    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
                }
            }
            if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])])
            {
                if (need2 <= 0)
                {
                    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + "-";
                }
                else
                {
                    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
                }
            }
            else
            {
                if (need2 <= 0)
                {
                    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + "-";
                }
                else
                {
                    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
                }
            }

            info.FindChild("zhufu/text1").GetComponent<Text>().text = text_need1;
            info.FindChild("zhufu/text2").GetComponent<Text>().text = text_need2;
            #endregion

            #region 基础属性
            string[] list_att0 = XMLMgr.instance.GetSXML("item.stage", "stage_level==0").GetNode("stage_info", "itemid==" + equip_data.tpid).getString("basic_att").Split(',');
            string[] list_att2 = stage_xml.getString("basic_att").Split(',');
            Transform AttName1 = Instantiate(txetClon[0]).transform;
            AttName1.SetParent(AttCon, false);
            AttName1.gameObject.SetActive(true);
            AttName1.GetComponent<Text>().text = ContMgr.getCont("a3_auction_jichu");
            conMon++;

            Transform att_basic = Instantiate(txetClon[1]).transform;
            att_basic.SetParent(AttCon, false);
            att_basic.gameObject.SetActive(true);
            conMon++;

            if (equip_data.equipdata.intensify_lv > 0)
            {
                SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + equip_data.equipdata.intensify_lv).GetNode("intensify_info", "itemid==" + equip_data.tpid);
                string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');

                if (list_att1.Length > 1)
                {
                    int add1 = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));
                    int add2 = int.Parse(list_att1[1]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[1]) - int.Parse(list_att0[1]));

                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
                        + "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
                }
                else
                {
                    int add = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));

                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
                }
            }
            else if (equip_data.equipdata.stage > 0)
            {
                if (list_att0.Length > 1)
                {
                    int add1 = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                    int add2 = int.Parse(list_att2[1]) - int.Parse(list_att0[1]);

                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
                        + "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
                }
                else
                {
                    int add = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
                }
            }
            else
            {
                if (list_att0.Length > 1)
                {
                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0]
                        + "-" + list_att0[1];
                }
                else
                {
                    att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0];
                }
            }
            #endregion

            #region 附加属性
            Transform AttName2 = Instantiate(txetClon[0]).transform;
            AttName2.SetParent(AttCon, false);
            AttName2.gameObject.SetActive(true);
            AttName2.GetComponent<Text>().text = ContMgr.getCont("a3_auction_fujia");
            conMon++;

            int subjoin_num = 0;
            SXML subjoin_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + equip_data.confdata.equip_level);
            foreach (int type in equip_data.equipdata.subjoin_att.Keys)
            {
                subjoin_num++;
                Transform att_Add = Instantiate(txetClon[2]).transform;
                att_Add.SetParent(AttCon, false);
                att_Add.gameObject.SetActive(true);
                conMon++;
                SXML subjoin_xml_node = subjoin_xml.GetNode("subjoin_att_info", "att_type==" + type);
                Text subjoin_text = att_Add.FindChild("Text").GetComponent<Text>();
                subjoin_text.text = Globle.getAttrAddById(type, equip_data.equipdata.subjoin_att[type]);
                if (equip_data.equipdata.subjoin_att[type] >= subjoin_xml_node.getInt("max"))
                {
                    att_Add.FindChild("max").gameObject.SetActive(true);
                }
                else
                {
                    att_Add.FindChild("max").gameObject.SetActive(false);
                }
            }
            #endregion

            #region 词缀属性
            Transform AttName3 = Instantiate(txetClon[0]).transform;
            AttName3.SetParent(AttCon, false);
            AttName3.gameObject.SetActive(true);
            AttName3.GetComponent<Text>().text = ContMgr.getCont("a3_auction_linghun");
            conMon++;

            Transform att_cizhui = Instantiate(txetClon[6]).transform;
            att_cizhui.SetParent(AttCon, false);
            att_cizhui.gameObject.SetActive(true);
            conMon++;

            string str = "";
            if (equip_data.equipdata.att_type > 0)
            {
                str = Globle.getAttrAddById(equip_data.equipdata.att_type, equip_data.equipdata.att_value);
                if (a3_EquipModel.getInstance().active_eqp.ContainsKey(equip_data.confdata.equip_type))
                {
                    if (a3_EquipModel.getInstance().active_eqp[equip_data.confdata.equip_type].id == equip_data.id)
                    {
                        str = "<color=#0cf373>" + str + "</color>";
                    }
                    else
                    {

                        str = "<color=#4d3d3d>" + str + ContMgr.getCont("a3_auction_Z") + Getcizhui(a3_EquipModel.getInstance().eqp_att_act[equip_data.equipdata.attribute]) + ContMgr.getCont("a3_auction_S") + Globle.getEquipTextByType(a3_EquipModel.getInstance().eqp_type_act[equip_data.confdata.equip_type]) + ContMgr.getCont("a3_auction_J") + "</color>";
                    }
                }
                else
                {
                    str = "<color=#4d3d3d>" + str + ContMgr.getCont("a3_auction_Z") + Getcizhui(a3_EquipModel.getInstance().eqp_att_act[equip_data.equipdata.attribute]) + ContMgr.getCont("a3_auction_S") + Globle.getEquipTextByType(a3_EquipModel.getInstance().eqp_type_act[equip_data.confdata.equip_type]) + ContMgr.getCont("a3_auction_J") + "</color>";
                }
            }
            else
            {
                str = "<color=#4d3d3d>" + ContMgr.getCont("FriendProxy_wu") + "</color>";
            }
            att_cizhui.FindChild("Text").GetComponent<Text>().text = str;

            if (equip_data.equipdata.baoshi != null)
            {

                Transform AttName6 = Instantiate(txetClon[0]).transform;
                AttName6.SetParent(AttCon, false);
                AttName6.gameObject.SetActive(true);
                AttName6.GetComponent<Text>().text = ContMgr.getCont("a3_auction_baoshi");
                conMon++;

                foreach (int i in equip_data.equipdata.baoshi.Keys)
                {
                    Transform att_baoshi = Instantiate(txetClon[3]).transform;
                    att_baoshi.SetParent(AttCon, false);
                    att_baoshi.gameObject.SetActive(true);
                    Image icon = att_baoshi.transform.FindChild("icon").GetComponent<Image>();
                    Text gem_text = att_baoshi.FindChild("Text").GetComponent<Text>();
                    if (equip_data.equipdata.baoshi[i] <= 0)
                    {
                        icon.gameObject.SetActive(false);
                        gem_text.text = ContMgr.getCont("a3_auction_can");
                    }
                    else
                    {
                        SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                        SXML s_xml = itemsXMl.GetNode("item", "id==" + equip_data.equipdata.baoshi[i]);
                        string File = "icon_item_" + s_xml.getString("icon_file");
                        icon.sprite = GAMEAPI.ABUI_LoadSprite(File);
                        SXML s_xml1 = itemsXMl.GetNode("gem_info", "item_id==" + equip_data.equipdata.baoshi[i]);
                        List<SXML> gem_add = s_xml1.GetNodeList("gem_add");
                        int add_type = 0;
                        int add_vaule = 0;
                        //bool ok = false;
                        foreach (SXML x in gem_add)
                        {
                            if (x.getInt("equip_type") == equip_data.confdata.equip_type)
                            {
                                add_type = x.getInt("att_type");
                                add_vaule = x.getInt("att_value");
                                break;
                            }
                        }
                        gem_text.text = Globle.getAttrAddById(add_type, add_vaule);
                    }
                    conMon++;
                }
            }
            #endregion

            #region 追加属性
            Transform AttName5 = Instantiate(txetClon[0]).transform;
            AttName5.SetParent(AttCon, false);
            AttName5.gameObject.SetActive(true);
            AttName5.GetComponent<Text>().text = ContMgr.getCont("a3_auction_zhuijia");
            conMon++;

            Transform att_outadd = Instantiate(txetClon[1]).transform;
            att_outadd.SetParent(AttCon, false);
            att_outadd.gameObject.SetActive(true);
            conMon++;
            SXML add_xml = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (equip_data.equipdata.add_level + 1));

            SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
            int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
            int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * equip_data.equipdata.add_level;
            att_outadd.FindChild("Text").GetComponent<Text>().text = Globle.getAttrAddById(attType, attValue);
            float childSizey = AttCon.GetComponent<GridLayoutGroup>().cellSize.y;
            float childSpacing = AttCon.GetComponent<GridLayoutGroup>().spacing.y;
            Vector2 newSize = new Vector2(AttCon.GetComponent<RectTransform>().sizeDelta.x, conMon * (childSizey + childSpacing) + childSpacing);
            AttCon.GetComponent<RectTransform>().sizeDelta = newSize;
            #endregion
        }
        void SetStar(Transform starRoot, int num)
        {
            int count = 0;
            foreach (var v in starRoot.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent != null && v.parent.parent == starRoot.transform)
                {
                    if (count < num) { v.gameObject.SetActive(true); count++; }
                    else { v.gameObject.SetActive(false); }
                }
            }
        }
        string Getcizhui(int type)
        {
            string str = "";
            switch (type)
            {
                case 1: str = ContMgr.getCont("a3_equiptip_f"); break;
                case 2: str = ContMgr.getCont("a3_equiptip_h"); break;
                case 3: str = ContMgr.getCont("a3_equiptip_g"); break;
                case 4: str = ContMgr.getCont("a3_equiptip_l"); break;
                case 5: str = ContMgr.getCont("a3_equiptip_b"); break;
            }
            return str;
        }
        #endregion
    }

    //class a3BaseAuction : Skin
    //{
    //    public a3_auction main { get; set; }
    //    public string pathStrName { get; set; }
    //    public a3BaseAuction(Window win, string pathStr)
    //        : base(win.getTransformByPath(pathStr))
    //    {
    //        var ss = pathStr.Split('/');
    //        this.pathStrName = ss[Mathf.Max(0, ss.Length - 1)];
    //        main = win as a3_auction;
    //        init();
    //    }
    //    public virtual void init() { }
    //    public virtual void onShowed() { }
    //    public virtual void onClose() { }
    //}



    /// <summary>
    /// buy
    /// </summary>
    class a3_auction_buy : Window
    {
        GameObject _prefab;
        Transform _content;
        GridLayoutGroup _glg;
        GameObject _select;
        Dictionary<uint, GameObject> gos = new Dictionary<uint, GameObject>();
        uint _selectId;
        int _page_index;
        int _page_max;
        int _goods_count;
        BaseButton btn_refresh;
        BaseButton btn_buy;
        BaseButton btn_help;
        BaseButton btn_left;
        BaseButton btn_right;
        Dictionary<string, uint> selectSearch = new Dictionary<string, uint>();
        Transform help;
        Transform help_par;
        string searchStr;
        InputField inputf;
        int num;
        int maxNum;
        List<SXML> listxml = new List<SXML>();
        int selectMax;
        int selectPrice;
        Transform etra;
        Transform djtip;
        ScrollControler scrollControer0;
        public a3_auction_buy()
        {
        }
        public override void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("cells/scroll"));
            inText();
            etra = a3_auction.instance.transform.FindChild("eqtip");
            djtip = a3_auction.instance.transform.FindChild("djtip");
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            transform.SetParent(a3_auction.instance.transform.FindChild("contents"));
            transform.localScale = Vector3.one;
            var info = etra.FindChild("info");
            var djInfo = djtip.FindChild("info");
            searchStr = default(string);
            _prefab = transform.FindChild("cells/scroll/0").gameObject;
            _content = transform.FindChild("cells/scroll/content");
            _glg = _content.GetComponent<GridLayoutGroup>();
            _select = transform.FindChild("cells/scroll/select").gameObject;
            help = transform.FindChild("help/panel_help");
            help_par = transform.FindChild("help");
            btn_refresh = new BaseButton(transform.FindChild("refresh"));
            btn_buy = new BaseButton(transform.FindChild("buy"));
            btn_help = new BaseButton(transform.FindChild("description"));
            btn_left = new BaseButton(transform.FindChild("count/left"));
            btn_right = new BaseButton(transform.FindChild("count/right"));

            btn_buy.onClick = (GameObject go) =>
            {
                if (_selectId != 0 && A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.equip_type > 0)
                {
                    uint cid = A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cid;
                    A3_AuctionProxy.getInstance().SendBuyMsg(_selectId, cid, (uint)maxNum);//直接买最小量
                }
                else if (_selectId != 0 && A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.equip_type < 1)
                {
                    uint cid = A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cid;

                    if (cid == PlayerModel.getInstance().cid)
                    {
                        flytxt.instance.fly(ContMgr.getCont("auction_tag0"));
                        return;
                    }
                    listxml.Clear();
                    listxml = XMLMgr.instance.GetSXMLList("item.item", "id==" + A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.tpid);
                    getTransformByPath("buynum/bg/contain/des_bg/Text").GetComponent<Text>().text = StringUtils.formatText(listxml[0].getString("desc"));
                    getTransformByPath("buynum/bg/contain/name").GetComponent<Text>().text = listxml[0].getString("item_name");
                    getTransformByPath("buynum/bg/contain/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listxml[0].getString("icon_file"));
                    getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum).ToString();
                    selectPrice = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum);
                    selectMax = maxNum;
                    getGameObjectByPath("buynum").SetActive(true);
                }

            };
            btn_left.onClick = (GameObject go) =>
            {
                _page_index = Mathf.Max(0, _page_index - 1);
                SendSearch((uint)_page_index);
                _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            };
            btn_right.onClick = (GameObject go) =>
            {
                _page_index = Mathf.Min(_page_max, _page_index + 1);
                SendSearch((uint)_page_index);
                _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            };
            btn_refresh.onClick = (GameObject go) =>
            {
                _page_index = 0;
                SendSearch((uint)_page_index);
                _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            };
            new BaseButton(transform.FindChild("fg/gold/add")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
              };
            new BaseButton(transform.FindChild("help/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            new BaseButton(transform.FindChild("help/panel_help/bg_0")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
            };
            btn_help.onClick = (GameObject go) =>
            {
                help.gameObject.SetActive(true);
                help.SetParent(help_par);
            };

            new BaseButton(getTransformByPath("buynum/bg/contain/btn_reduce")).onClick = (GameObject go) =>
             {
                 int nownum;
                 if (int.TryParse(inputf.text, out nownum))
                 {
                 }
                 if (nownum - 1 < 1) return;
                 inputf.text = (nownum - 1).ToString();
                 if (int.TryParse(inputf.text, out num))
                 {
                 }
                 getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();

             };
            new BaseButton(getTransformByPath("buynum/bg/contain/btn_add")).onClick = (GameObject go) =>
            {
                int nownum;
                if (int.TryParse(inputf.text, out nownum))
                {
                }
                if (nownum + 1 > selectMax) return;
                inputf.text = (nownum + 1).ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };
            new BaseButton(getTransformByPath("buynum/bg/contain/min")).onClick = (GameObject go) =>
            {
                inputf.text = 1.ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };
            new BaseButton(getTransformByPath("buynum/bg/contain/max")).onClick = (GameObject go) =>
            {
                inputf.text = selectMax.ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };

            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_LOADALL, Event_OnLoadAll);
            Transform filt = transform.FindChild("filt");
            foreach (var v in filt.GetComponentsInChildren<Button>(true))
            {
                if (v.transform.parent == filt)
                {
                    new BaseButton(v.transform).onClick = UI_Filt;
                    Transform f0 = transform.FindChild("filt/box/" + v.name + "/0");
                    if (f0 != null)
                    {
                        foreach (var bt in f0.GetComponentsInChildren<Button>(true))
                        {
                            var temp = new List<string>();
                            temp.Add(v.name);
                            new BaseButton(bt.transform).onClick = (GameObject go) =>
                            {
                                UI_SelectFilt(temp[0], int.Parse(go.name));
                                if (v.name == "part" && bt.name == "1")
                                {
                                    getTransformByPath("filt/level").GetComponent<Button>().interactable = false;
                                    getTransformByPath("filt/crr").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    getTransformByPath("filt/level").GetComponent<Button>().interactable = true;
                                    getTransformByPath("filt/crr").GetComponent<Button>().interactable = true;
                                }
                            };
                        }
                    }
                }
            }
            #region
            new BaseButton(transform.FindChild("buynum/btn")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("buynum").SetActive(false);
                inputf.text = 1.ToString();
            };

            inputf = getTransformByPath("buynum/bg/contain/bug/InputField").GetComponent<InputField>();
            inputf.text = 1.ToString();
            if (int.TryParse(inputf.text, out num))
            {
            }
            new BaseButton(getTransformByPath("buynum/bg/ok")).onClick = (GameObject go) =>
            {
                uint cid = A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cid;
                A3_AuctionProxy.getInstance().SendBuyMsg(_selectId, cid, (uint)num);
                getGameObjectByPath("buynum").SetActive(false);
                inputf.text = 1.ToString();
            };
            inputf.onValueChanged.AddListener((string ss) =>
            {
                if (int.Parse(inputf.text) > selectMax)
                {
                    inputf.text = selectMax.ToString();
                    num = selectMax;
                }
                else
                    num = int.Parse(inputf.text);
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            });

            #endregion
            new BaseButton(transform.FindChild("fg/search/btn_search")).onClick = (GameObject) =>
            {
                searchStr = transform.FindChild("fg/search/InputField").GetComponent<InputField>().text;
                SendSearch((uint)_page_index);
            };
            transform.FindChild("fg/search/InputField").GetComponent<InputField>().onValueChanged.AddListener((string ss) =>
            {
                if (ss == "")
                {
                    searchStr = "";
                    SendSearch((uint)_page_index);
                }
            });
            new BaseButton(etra.transform.FindChild("info/buy")).onClick = (GameObject) =>
            {
                //debug.Log(maxNum + "num::::::::::");
                if (_selectId != 0 && A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.equip_type > 0)
                {
                    uint cid = A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cid;
                    A3_AuctionProxy.getInstance().SendBuyMsg(_selectId, cid, (uint)maxNum);
                }
                etra.gameObject.SetActive(false);
            };
            new BaseButton(djtip.transform.FindChild("info/buy")).onClick = (GameObject) =>
            {
                inputf.text = 1.ToString();
                listxml.Clear();
                listxml = XMLMgr.instance.GetSXMLList("item.item", "id==" + A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.tpid);
                getTransformByPath("buynum/bg/contain/des_bg/Text").GetComponent<Text>().text = StringUtils.formatText(listxml[0].getString("desc"));
                getTransformByPath("buynum/bg/contain/name").GetComponent<Text>().text = listxml[0].getString("item_name");
                getTransformByPath("buynum/bg/contain/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listxml[0].getString("icon_file"));
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum).ToString();
                selectPrice = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum);
                selectMax = maxNum;
                getGameObjectByPath("buynum").SetActive(true);
                djtip.gameObject.SetActive(false);
            };
        }

        void inText()
        {
            this.transform.FindChild("buy/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_5");
            this.transform.FindChild("refresh/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_6");
            this.transform.FindChild("filt/part/p").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_7");
            this.transform.FindChild("filt/grade/p").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_8");
            this.transform.FindChild("filt/attribute/p").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_9");
            this.transform.FindChild("filt/price/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_10");
            this.transform.FindChild("filt/crr/p").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_11");
            this.transform.FindChild("help/panel_help/descTxt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_auction_12"));
            this.transform.FindChild("filt/box/part/0/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_13");
            this.transform.FindChild("filt/box/part/0/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_14");
            this.transform.FindChild("filt/box/part/0/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_15");
            this.transform.FindChild("filt/box/part/0/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_16");
            this.transform.FindChild("filt/box/part/0/4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_17");
            this.transform.FindChild("filt/box/part/0/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_18");
            this.transform.FindChild("filt/box/part/0/6/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_19");
            this.transform.FindChild("filt/box/part/0/7/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_20");
            this.transform.FindChild("filt/box/part/0/8/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_21");
            this.transform.FindChild("filt/box/part/0/9/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_22");
            this.transform.FindChild("filt/box/part/0/10/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_23");
            this.transform.FindChild("filt/box/part/0/11/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_24");
            this.transform.FindChild("filt/box/grade/0/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_25");
            this.transform.FindChild("filt/box/grade/0/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_26");
            this.transform.FindChild("filt/box/grade/0/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_27");
            this.transform.FindChild("filt/box/grade/0/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_28");
            this.transform.FindChild("filt/box/grade/0/4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_29");
            this.transform.FindChild("filt/box/grade/0/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_30");
            this.transform.FindChild("filt/box/grade/0/6/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_30_1");

            this.transform.FindChild("filt/box/attribute/0/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_31");
            this.transform.FindChild("filt/box/attribute/0/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_32");
            this.transform.FindChild("filt/box/attribute/0/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_33");
            this.transform.FindChild("filt/box/attribute/0/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_34");
            this.transform.FindChild("filt/box/attribute/0/4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_35");
            this.transform.FindChild("filt/box/attribute/0/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_36");

            this.transform.FindChild("filt/box/crr/0/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_37");
            this.transform.FindChild("filt/box/crr/0/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_38");
            this.transform.FindChild("filt/box/crr/0/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_39");
            this.transform.FindChild("filt/box/crr/0/5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_40");
        }
        public override void onShowed()
        {
            searchStr = default(string);
            _page_index = 0;
            _page_max = 0;
            _goods_count = 0;
            btn_left.interactable = false;
            btn_right.interactable = false;
            selectSearch.Clear();
            selectSearch["updown"] = 0;
            UI_SelectFiltName("part", 0);
            UI_SelectFiltName("grade", 0);
            UI_SelectFiltName("attribute", 0);
            UI_SelectFiltName("level", 0);
            UI_SelectFiltName("crr", 0);
            Transform up = transform.FindChild("filt/price/sellsortup");
            Transform down = transform.FindChild("filt/price/sellsortdown");
            up.gameObject.SetActive(true);
            down.gameObject.SetActive(false);
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            transform.FindChild("fg/search/InputField").GetComponent<InputField>().text = "";
            btn_buy.interactable = false;
            _selectId = 0;
            _select.SetActive(false);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_LOADALL, Event_OnLoadAll);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_BUYSUCCESS, Event_BuySuccess);
            A3_AuctionProxy.getInstance().SendSearchMsg();
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + 0 + " / " + 0 + ")";
            _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

        }
        public void SetUIData(uint id, a3_BagItemData data)//买装备的界面
        {
            if (!gos.ContainsKey(id)) return;
            Image icon = gos[id].transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(data.confdata.file);
            //List<a3_BagItemData> temp = new List<a3_BagItemData>();
            ArrayList al = new ArrayList();
            al.Add(data);
            al.Add(gos[id]);
            new BaseButton(icon.transform).onClick = (GameObject gg) =>//icon的展示按钮
            {
                _select.transform.SetParent(((GameObject)al[1]).transform);
                _select.transform.localPosition = Vector3.zero;
                //_select.transform.SetAsLastSibling();
                _select.SetActive(true);
                _select.transform.SetSiblingIndex(1);
                _selectId = ((a3_BagItemData)al[0]).id;
                btn_buy.interactable = true;
                maxNum = A3_AuctionModel.getInstance().GetItems()[_selectId].num;
                if (data.confdata.equip_type < 1)
                {
                    justShowBuynum();
                    //main.djtip.gameObject.SetActive(true);
                    //main.ShowDJTip((a3_BagItemData)al[0], false, true);
                }
                else
                {
                    //main.etra.gameObject.SetActive(true);
                    //main.ShowEPTip((a3_BagItemData)al[0], false, true);
                    ArrayList list = new ArrayList();
                    list.Add((a3_BagItemData)al[0]);
                    list.Add(equip_tip_type.tip_forAuc_buy);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, list);
                }

            };

            if (data.confdata.job_limit == PlayerModel.getInstance().profession)
            {
                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type))
                {
                    var ep = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
                    if (ep.equipdata.combpt > data.equipdata.combpt)
                    {
                        gos[id].transform.FindChild("arrow").gameObject.SetActive(false);
                    }
                    else gos[id].transform.FindChild("arrow").gameObject.SetActive(true);
                }
                else gos[id].transform.FindChild("arrow").gameObject.SetActive(true);
            }
            gos[id].transform.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
            gos[id].transform.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
            gos[id].transform.FindChild("lv").GetComponent<Text>().text = "+" + data.equipdata.intensify_lv + ContMgr.getCont("a3_auction_zhui") + data.equipdata.add_level;




            //gos[id].transform.FindChild("seller").GetComponent<Text>().text = data.auctiondata.seller;
            string cizhui= ContMgr.getCont("FriendProxy_wu");
            switch (data.equipdata.attribute)
            {
                case 0: break;
                case 1:
                    //cizhui = "[风]";
                    cizhui = ContMgr.getCont("a3_equip_11");
                    break;
                case 2:
                    //cizhui = "[火]";
                    cizhui = ContMgr.getCont("a3_equip_12");
                    break;
                case 3:
                    cizhui = ContMgr.getCont("a3_equip_13");
                    //cizhui = "[光]";
                    break;
                case 4:
                    cizhui = ContMgr.getCont("a3_equip_14");
                    //cizhui = "[雷]";
                    break;
                case 5:
                    cizhui = ContMgr.getCont("a3_equip_15");
                    // cizhui = "[冰]";
                    break;
            }
            gos[id].transform.FindChild("seller").GetComponent<Text>().text = cizhui;
            gos[id].transform.FindChild("grade").GetComponent<Text>().text = data.equipdata.stage + ContMgr.getCont("a3_auction_jie");

            gos[id].transform.FindChild("addlv").GetComponent<Text>().text = ContMgr.getCont("a3_auction_zhui") + data.equipdata.add_level;
            if (data.confdata.equip_type < 1)
            {
                gos[id].transform.FindChild("num").gameObject.SetActive(true);
                gos[id].transform.FindChild("num").GetComponent<Text>().text = data.num.ToString();
            }

            gos[id].transform.FindChild("pro/war").gameObject.SetActive(false);
            gos[id].transform.FindChild("pro/mage").gameObject.SetActive(false);
            gos[id].transform.FindChild("pro/ass").gameObject.SetActive(false);
            switch (data.confdata.job_limit)
            {
                case 1:
                    break;
                case 2:
                    gos[id].transform.FindChild("pro/war").gameObject.SetActive(true);
                    break;
                case 3:
                    gos[id].transform.FindChild("pro/mage").gameObject.SetActive(true);
                    break;
                case 4:
                    gos[id].transform.FindChild("pro/war").gameObject.SetActive(true);
                    break;
                case 5:
                    gos[id].transform.FindChild("pro/ass").gameObject.SetActive(true);
                    break;
            }
            if (data.confdata.equip_type > 0)//按装备类型来区分材料
            {
                gos[id].transform.FindChild("sell").GetComponent<Text>().text = +data.auctiondata.cost + ContMgr.getCont("a3_auction_zuan");
            }
            else//A3_AuctionModel.getInstance().GetItems()[id].num个数
                gos[id].transform.FindChild("sell").GetComponent<Text>().text = data.auctiondata.cost / A3_AuctionModel.getInstance().GetItems()[id].num + ContMgr.getCont("a3_auction_zuannum");
        }
        void justShowBuynum()
        {
            inputf.text = 1.ToString();
            listxml.Clear();
            listxml = XMLMgr.instance.GetSXMLList("item.item", "id==" + A3_AuctionModel.getInstance().GetItems()[_selectId].confdata.tpid);
            getTransformByPath("buynum/bg/contain/des_bg/Text").GetComponent<Text>().text = StringUtils.formatText(listxml[0].getString("desc"));
            getTransformByPath("buynum/bg/contain/name").GetComponent<Text>().text = listxml[0].getString("item_name");
            getTransformByPath("buynum/bg/contain/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listxml[0].getString("icon_file"));
            getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum).ToString();
            selectPrice = (A3_AuctionModel.getInstance().GetItems()[_selectId].auctiondata.cost / maxNum);
            selectMax = maxNum;
            getGameObjectByPath("buynum").SetActive(true);
        }
        void Event_OnLoadAll(GameEvent e)
        {
            //debug.Log("加载所有" + e.data.dump());
            _page_max = 0;
            if (gos.Count > 0)
            {
                _select.transform.SetParent(transform);
                foreach (var v in _content.GetComponentsInChildren<Transform>(true))
                {
                    if (v.parent == _content)
                    {
                        GameObject.Destroy(v.gameObject);
                    }
                }
                gos.Clear();
            }
            Variant data = e.data;
            if (data.ContainsKey("count"))
            {
                _goods_count = data["count"];
            }
            _page_max = Mathf.CeilToInt((float)_goods_count / 8f) - 1;
            _page_max = Mathf.Max(0, _page_max);
            _page_index = Mathf.Min(_page_index, _page_max);
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + (_page_index + 1) + " / " + (_page_max + 1) + ")";
            if (_page_max == _page_index) { btn_right.interactable = false; }
            else { btn_right.interactable = true; }
            if (_page_index > 0) { btn_left.interactable = true; }
            else { btn_left.interactable = false; }

            _selectId = 0;
            _select.transform.SetParent(transform);
            _select.SetActive(false);
            List<a3_BagItemData> temp = new List<a3_BagItemData>(A3_AuctionModel.getInstance().GetItems().Values);
            foreach (var v in temp)
            {
                var go = GameObject.Instantiate(_prefab) as GameObject;
                go.transform.SetParent(_content);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                List<a3_BagItemData> tp = new List<a3_BagItemData>();
                tp.Add(v);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    _select.transform.SetParent(g.transform);
                    _select.transform.localPosition = Vector3.zero;
                    //_select.transform.SetAsLastSibling();
                    _select.SetActive(true);
                    _select.transform.SetSiblingIndex(1);
                    _selectId = tp[0].id;
                    maxNum = A3_AuctionModel.getInstance().GetItems()[_selectId].num;

                    btn_buy.interactable = true;
                };
                gos[v.id] = go;
                SetUIData(v.id, v);
            }
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (_glg.cellSize.y + 0.1f) * temp.Count);
            btn_buy.interactable = false;
        }

        void Event_BuySuccess(GameEvent e)
        {
            _selectId = 0;
            _select.transform.SetParent(transform);
            _select.SetActive(false);
            btn_buy.interactable = false;
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            _page_index = 0;
            SendSearch((uint)_page_index);
            _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            Variant data = e.data;
            Variant list = data["get_list"];
            foreach (var v in list._arr)
            {
                int cid = v["cid"];
                uint id = v["id"];
                if (gos.ContainsKey(id)) { GameObject.Destroy(gos[id]); gos.Remove(id); }
                if (cid != PlayerModel.getInstance().cid)
                {
                    if (v["equip_type"] > 0)
                    {
                        if (v.ContainsKey("name"))
                            flytxt.instance.fly(ContMgr.getCont("a3_auction_buyaonr") + v["name"] + "!");
                    }
                    else
                    {
                        if (v.ContainsKey("name"))
                            flytxt.instance.fly(ContMgr.getCont("a3_auction_buya") + v["num"] + ContMgr.getCont("ge") + v["name"] + "!");
                    }

                }
                else
                {
                    if (v.ContainsKey("name"))
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_paimaia") + v["name"] + "!");
                }

            }
            if (etra.gameObject.activeSelf) etra.gameObject.SetActive(false);
        }

        public override void onClosed()
        {
            searchStr = default(string);
            _selectId = 0;
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_LOADALL, Event_OnLoadAll);
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_BUYSUCCESS, Event_BuySuccess);
            _select.transform.SetParent(transform);
            foreach (var v in _content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == _content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            gos.Clear();
        }

        void UI_Filt(GameObject go)
        {
            Transform cont = transform.FindChild("filt/box/" + go.name);
            if (cont != null)
            {
                if (!cont.gameObject.activeSelf)
                {
                    Transform root = transform.FindChild("filt/box/");
                    foreach (var v in transform.FindChild("filt/box/").GetComponentsInChildren<Transform>(true))
                    {
                        if (v.parent == root)
                        {
                            if (v.gameObject.activeSelf) v.gameObject.SetActive(false);
                        }
                    }
                }
                cont.gameObject.SetActive(!cont.gameObject.activeSelf);
            }
            if (go.name == "price")
            {
                Transform up = go.transform.FindChild("sellsortup");
                Transform down = go.transform.FindChild("sellsortdown");
                up.gameObject.SetActive(!up.gameObject.activeSelf);
                down.gameObject.SetActive(!down.gameObject.activeSelf);
                selectSearch["updown"] = (uint)(up.gameObject.activeSelf ? 0 : 1);
                _page_index = 0;
                SendSearch((uint)_page_index);
            }
        }

        void UI_SelectFilt(string type, int id)
        {
            selectSearch[type] = (uint)id;
            Transform cont = transform.FindChild("filt/box/" + type);
            if (cont != null)
            {
                cont.gameObject.SetActive(false);
            }
            UI_SelectFiltName(type, id);
            _page_index = 0;
            SendSearch((uint)_page_index);
        }
        void UI_SelectFiltName(string type, int id)
        {
            selectSearch[type] = (uint)id;
            string str = transform.FindChild("filt/box/" + type + "/0/" + id + "/Text").GetComponent<Text>().text;
            transform.FindChild("filt/" + type + "/Text").GetComponent<Text>().text = str;
        }

        void SendSearch(uint page)
        {
            if (!selectSearch.ContainsKey("updown")) selectSearch["updown"] = 0;
            if (!selectSearch.ContainsKey("crr")) selectSearch["crr"] = 0;
            if (!selectSearch.ContainsKey("part")) selectSearch["part"] = 0;
            if (!selectSearch.ContainsKey("attribute")) selectSearch["attribute"] = 0;
            if (!selectSearch.ContainsKey("level")) selectSearch["level"] = 0;
            if (!selectSearch.ContainsKey("grade")) selectSearch["grade"] = 0;
            A3_AuctionProxy.getInstance().SendSearchMsg(page, selectSearch["updown"], selectSearch["crr"], selectSearch["part"], selectSearch["level"], selectSearch["grade"], searchStr, selectSearch["attribute"]);

        }

    }

    /// <summary>
    /// get
    /// </summary>
    class a3_auction_get : Window
    {
        GameObject _prefab;
        Transform _content;
        GridLayoutGroup _glg;
        GameObject _select;
        Dictionary<uint, GameObject> gos = new Dictionary<uint, GameObject>();
        uint _selectId;
        BaseButton btn_get;
        BaseButton btn_getall;
        BaseButton btn_help;
        Transform help;
        Transform help_par;



        int itpouu;
        int eqpouu;
        int jewraa;
        int exratoo;
        int expardayy;
        int eqplell;


        public a3_auction_get()
        {
        }
        public override void init()
        {
            inText();
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            transform.SetParent(a3_auction.instance.transform.FindChild("contents"));
            transform.localScale = Vector3.one;
            _prefab = transform.FindChild("cells/scroll/0").gameObject;
            _content = transform.FindChild("cells/scroll/content");
            help = transform.FindChild("help/panel_help");
            help_par = transform.FindChild("help");
            _glg = _content.GetComponent<GridLayoutGroup>();
            _select = transform.FindChild("cells/scroll/select").gameObject;
            btn_getall = new BaseButton(transform.FindChild("getall"));
            btn_get = new BaseButton(transform.FindChild("get"));
            btn_help = new BaseButton(transform.FindChild("description"));

            btn_get.onClick = (GameObject g) =>
            {
                if (_selectId > 0)
                    A3_AuctionProxy.getInstance().SendGetMsg(_selectId);
            };
            btn_getall.onClick = (GameObject g) =>
            {
                A3_AuctionProxy.getInstance().SendGetAllMsg();
            };
            new BaseButton(transform.FindChild("help/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            new BaseButton(transform.FindChild("help/panel_help/bg_0")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            new BaseButton(transform.FindChild("fg/gold/add")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            };
            btn_help.onClick = (GameObject go) =>
            {
                help.gameObject.SetActive(true);
            };
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_NEWGET, (GameEvent e) =>
            {
                Variant da = e.data;
                bool isnew = da["new"];
                if (isnew) a3_auction.instance.transform.FindChild("tabs/get/notice").gameObject.SetActive(true);
                else a3_auction.instance.transform.FindChild("tabs/get/notice").gameObject.SetActive(false);
            });
           
        }

        void inText() {
            this.transform.FindChild("count/countttt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_60");
            this.transform.FindChild("tip").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_61");
            this.transform.FindChild("get/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_62");
            this.transform.FindChild("getall/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_63");
            this.transform.FindChild("help/panel_help/descTxt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_auction_64"));
            this.transform.FindChild ("cells/scroll/0/name").GetComponent <Text>().text = ContMgr.getCont("uilayer_a3_auction_65");
            this.transform.FindChild("cells/scroll/0/tip").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_66");


        }
        public override void onShowed()
        {
            
            btn_get.interactable = false;
            _selectId = 0;
            _select.SetActive(false);
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + 0 + ")";
            _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_MYGET, Event_OnMyGet);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_GETMYGET, Event_GetOneSuccess);
            A3_AuctionProxy.getInstance().SendMyRackMsg();
        }
        public void SetUIData(uint id, a3_BagItemData data)
        {

            
            if (!gos.ContainsKey(id)) return;
            if (data.auctiondata.get_type != 3)
            {
                Image icon = gos[id].transform.FindChild("icon").GetComponent<Image>();
                icon.sprite = GAMEAPI.ABUI_LoadSprite(data.confdata.file);
                gos[id].transform.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
                gos[id].transform.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
                gos[id].transform.FindChild("sell").GetComponent<Text>().text = data.auctiondata.cost.ToString();
            }
            else
            {
                if(data.auctiondata.cost > 1)
                    gos[id].transform.FindChild("sell").GetComponent<Text>().text = (int)(data.auctiondata.cost * 0.9f) + "";
                else
                    gos[id].transform.FindChild("sell").GetComponent<Text>().text = "1";
            }
            if (data.confdata.equip_type < 1)
            {
                gos[id].transform.FindChild("num").gameObject.SetActive(true);
                gos[id].transform.FindChild("num").GetComponent<Text>().text = data.num.ToString();
            }


            SXML lis = XMLMgr.instance.GetSXML("acution.parm");
            itpouu = lis.getInt("item_pou");
            eqpouu = lis.getInt("eqp_pou");
            jewraa = lis.getInt("jew_ratio");
            exratoo = lis.getInt("ext_ratio");
            expardayy = lis.getInt("expired_days");



           SXML  xml = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
            if (xml != null)
            {
               
                eqplell = xml.getInt("equip_level");
               
            }

            TimeSpan ts = new TimeSpan(expardayy * 24, 0, data.auctiondata.get_tm);
            if (ts.TotalSeconds < muNetCleint.instance.CurServerTimeStamp)
            {


                int x =1;
                int minPrice = data.equipdata.baseCombpt / 100 + data.equipdata.eqp_level * 10;
                //transform.FindChild("price/2/Image/Text").GetComponent<Text>().text = minPrice + ContMgr.getCont("a3_auction_zuannum");
                if (data.confdata.equip_type>0)
                {
                   x= eqpouu * eqplell;
                    if (data.confdata.equip_type == 8 || data.confdata.equip_type == 9 || data.confdata.equip_type == 10)
                    {
                        x = eqpouu * jewraa *eqplell;
                    }
                }
                else
                {
                     x = itpouu * data.confdata.quality * a3_auction_sell.num;
                }
               

                x *= exratoo;
                gos[id].transform.FindChild("time").GetComponent<Text>().text = ContMgr.getCont("a3_auction_timeover") + "\n" + ContMgr.getCont("a3_auction_baocunfei") + x;
            }
            else
            {
                ts = new TimeSpan(0, 0, (int)(ts.TotalSeconds - muNetCleint.instance.CurServerTimeStamp));
                if (ts.TotalSeconds >= 86400)
                {
                    gos[id].transform.FindChild("time").GetComponent<Text>().text = ContMgr.getCont("a3_auction_havedays", ((int)ts.TotalDays).ToString());
                }
                else
                {
                    gos[id].transform.FindChild("time").GetComponent<Text>().text = ContMgr.getCont("a3_auction_havetime", ((int)ts.TotalHours % 24).ToString(), ts.Minutes.ToString());
                }
            }
            gos[id].transform.FindChild("tip").GetComponent<Text>().text = A3_AuctionModel.getInstance().FromGetTypeToString(data.auctiondata.get_type);
        }

        public override void onClosed()
        {
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_MYGET, Event_OnMyGet);
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_GETMYGET, Event_GetOneSuccess);
            _selectId = 0;
            _select.transform.SetParent(transform);
            foreach (var v in _content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == _content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            gos.Clear();
        }

        void Event_OnMyGet(GameEvent e)
        {
            //test
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            List<a3_BagItemData> temp = new List<a3_BagItemData>(A3_AuctionModel.getInstance().GetMyItems_down().Values);
            foreach (var v in temp)
            {
                var go = GameObject.Instantiate(_prefab) as GameObject;
                go.transform.SetParent(_content);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                List<a3_BagItemData> tp = new List<a3_BagItemData>();
                tp.Add(v);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    _select.transform.SetParent(g.transform);
                    _select.transform.localPosition = Vector3.zero;
                    //_select.transform.SetAsLastSibling();
                    _select.SetActive(true);
                    _select.transform.SetSiblingIndex(1);
                    _selectId = tp[0].id;
                    btn_get.interactable = true;
                };
                gos[v.id] = go;
                
                SetUIData(v.id, v);
            }
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (_glg.cellSize.y) * temp.Count);//+ 0.1f
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + temp.Count + ")";
        }

        void Event_GetOneSuccess(GameEvent e)
        {
            _selectId = 0;
            _select.transform.SetParent(transform);
            _select.SetActive(false);
            btn_get.interactable = false;
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            Variant data = e.data;
            uint id = data["auc_id"];
            if (gos.ContainsKey(id))
            {
                GameObject.Destroy(gos[id]);
            }
            gos.Remove(id);
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + gos.Count + ")";
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (_glg.cellSize.y + 0.1f) * gos.Count);

        }

    }

    /// <summary>
    /// rack
    /// </summary>
    class a3_auction_rack : Window
    {
        GameObject _prefab;
        Transform _content;
        GridLayoutGroup _glg;
        GameObject _select;
        Dictionary<uint, GameObject> gos = new Dictionary<uint, GameObject>();
        uint _selectId;
        BaseButton btn_refresh;
        BaseButton btn_putoff;
        BaseButton btn_help;
        Transform help;
        Transform help_par;
        Transform etra;
        Transform djtip;
        ScrollControler scrollControer0;
        public a3_auction_rack()
        {
        }
        public override void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("cells/scroll"));
            inText();
            etra = a3_auction.instance.transform.FindChild("eqtip");
            djtip = a3_auction.instance.transform.FindChild("djtip");
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            transform.SetParent(a3_auction.instance.transform.FindChild("contents"));
            transform.localScale = Vector3.one;
            _prefab = transform.FindChild("cells/scroll/0").gameObject;
            _content = transform.FindChild("cells/scroll/content");
            _glg = _content.GetComponent<GridLayoutGroup>();
            _select = transform.FindChild("cells/scroll/select").gameObject;
            help = transform.FindChild("help/panel_help");
            help_par = transform.FindChild("help");
            btn_refresh = new BaseButton(transform.FindChild("refresh"));
            btn_putoff = new BaseButton(transform.FindChild("putoff"));
            btn_help = new BaseButton(transform.FindChild("description"));

            btn_putoff.onClick = (GameObject g) =>
            {
                A3_AuctionProxy.getInstance().SendPutOffMsg(_selectId);
            };
            new BaseButton(transform.FindChild("fg/gold/add")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            };
            new BaseButton(transform.FindChild("help/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            new BaseButton(transform.FindChild("help/panel_help/bg_0")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            btn_help.onClick = (GameObject go) =>
            {
                help.gameObject.SetActive(true);
                //help.localPosition = Vector3.zero;
            };
        }

        void inText()
        {
            this.transform.FindChild("putoff/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_54");
            this.transform.FindChild("count/countttt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_55");
            this.transform.FindChild("fg/GameObject/sell").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_56");
            this.transform.FindChild("fg/GameObject/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_57");
            this.transform.FindChild("fg/GameObject/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_58");
            this.transform.FindChild("help/panel_help/descTxt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_auction_59"));
        }
        public override void onShowed()
        {
            btn_putoff.interactable = false;
            _selectId = 0;
            _select.SetActive(false);
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + 0 + ")";
            _content.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_LOADMYSHELF, Event_LoadMyRack);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_PUTOFFSUCCESS, Event_PutOffSuccess);
            A3_AuctionProxy.getInstance().SendMyRackMsg();
        }
        public void SetUIData(uint id, a3_BagItemData data)
        {
            if (!gos.ContainsKey(id)) return;
            Image icon = gos[id].transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(data.confdata.file);
            gos[id].transform.FindChild("name").GetComponent<Text>().text = data.confdata.item_name;
            gos[id].transform.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);
            TimeSpan ts = new TimeSpan(data.auctiondata.pro_tm, 0, 0);
            int seconds = (int)(ts.TotalSeconds + data.auctiondata.tm - muNetCleint.instance.CurServerTimeStamp);
            ts = new TimeSpan(0, 0, Mathf.Max(0, seconds));
            gos[id].transform.FindChild("time").GetComponent<Text>().text = ContMgr.getCont("a3_auction_times", ((int)ts.TotalHours).ToString(), ts.Minutes.ToString(), ts.Seconds.ToString());
            if (data.confdata.equip_type < 1)
            {
                gos[id].transform.FindChild("num").gameObject.SetActive(true);
                gos[id].transform.FindChild("num").GetComponent<Text>().text = data.num.ToString();
            }
            if (data.confdata.equip_type > 0)
            {
                gos[id].transform.FindChild("sell").GetComponent<Text>().text = data.auctiondata.cost + ContMgr.getCont("a3_auction_zuan");
            }
            else
                gos[id].transform.FindChild("sell").GetComponent<Text>().text = data.auctiondata.cost / data.num + ContMgr.getCont("a3_auction_zuannum");
        }

        public override void onClosed()
        {
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_LOADMYSHELF, Event_LoadMyRack);
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_PUTOFFSUCCESS, Event_PutOffSuccess);
            _selectId = 0;
            _select.transform.SetParent(transform);
            foreach (var v in _content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == _content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            gos.Clear();
        }
        void Event_LoadMyRack(GameEvent e)
        {
            List<a3_BagItemData> temp = new List<a3_BagItemData>(A3_AuctionModel.getInstance().GetMyItems_up().Values);
            Transform tra = getTransformByPath("cells/scroll/content");
            int counts = tra.childCount;
            for (int i = counts; i > 0; i--)
            {
                GameObject.DestroyImmediate(tra.GetChild(i - 1).gameObject);
            }
            foreach (var v in temp)
            {
                var go = GameObject.Instantiate(_prefab) as GameObject;
                go.transform.SetParent(_content);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                List<a3_BagItemData> tp = new List<a3_BagItemData>();
                tp.Add(v);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    _select.transform.SetParent(g.transform);
                    _select.transform.localPosition = Vector3.zero;
                    _select.SetActive(true);
                    _select.transform.SetSiblingIndex(1);
                    _selectId = tp[0].id;
                    btn_putoff.interactable = true;
                };
                if (gos.ContainsKey(v.id))
                {
                    gos[v.id] = go;
                }
                else
                    gos.Add(v.id, go);

                SetUIData(v.id, v);
            }
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (_glg.cellSize.y + 0.1f) * temp.Count);
            transform.FindChild("count/count").GetComponent<Text>().text = "(" + temp.Count + ")";
        }

        void Event_PutOffSuccess(GameEvent e)
        {
            _selectId = 0;
            _select.transform.SetParent(transform);
            _select.SetActive(false);
            transform.FindChild("fg/gold/Text").GetComponent<Text>().text = PlayerModel.getInstance().gold.ToString();
            btn_putoff.interactable = false;
            Variant data = e.data;
            if (data.ContainsKey("auc_data"))
            {
                Variant vd = data["auc_data"];
                foreach (Variant v in vd._arr)
                {
                    uint id = v["id"];
                    if (v["equip_type"] > 0)
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_lostone") + v["name"]);
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_lostnum") + v["num"] + ContMgr.getCont("ge") + v["name"]);

                    }
                    if (gos.ContainsKey(id))
                    {
                        foreach (var item in gos)
                        {
                            if (item.Key != id)
                                GameObject.Destroy(item.Value);
                        }
                    }
                    gos.Remove(id);
                }
                if (vd.Count < 1)
                {
                    foreach (var item in gos)
                    {
                        GameObject.Destroy(item.Value);
                    }

                    gos.Clear();
                }
            }
            if (data.ContainsKey("get_list"))
            {
                Variant vd = data["get_list"];
                foreach (Variant v in vd._arr)
                {
                    uint id = v["id"];
                    if (v["equip_type"] > 0)
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_lostone") + v["name"]);
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_lostnum") + v["num"] + ContMgr.getCont("ge") + v["name"]);
                    }
                    if (gos.ContainsKey(id))
                    {
                        GameObject.Destroy(gos[id]);
                    }
                    gos.Remove(id);
                }
            }
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (_glg.cellSize.y + 0.1f) * gos.Count);
            A3_AuctionProxy.getInstance().SendMyRackMsg();
        }

    }

    /// <summary>
    /// sell
    /// </summary>
    class a3_auction_sell : Window
    {
        Transform _content;
        GameObject _select;
        uint _selectId;
        GameObject _selectIdIcon;
        List<GameObject> ir = new List<GameObject>();
        Dictionary<uint, GameObject> _gos = new Dictionary<uint, GameObject>();
        int minPrice;
        int comper;
        int itpou;
        int eqpou;
        int eqplel;
        int jewra;
        int exrato;
        int exparday;
        BaseButton sell;
        public InputField input;
        Transform help;
        Transform help_par;
        BaseButton btn_help;
        Text reducePrice;
         public static   int needPriceTime = 1;
        public static int num;
        int maxNum;
        int inputNum;
         InputField inputf;
        int selectMax;
        int selectPrice;
        Transform etra;
        Transform edj;
        public static a3_auction_sell instans;
        List<SXML> listxml = new List<SXML>();
        public a3_auction_sell()
        {
        }
        public override void init()
        {
            instans = this;
            inText();
            etra = a3_auction.instance.transform.FindChild("eqtip");
            edj = a3_auction.instance.transform.FindChild("djtip");
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            transform.SetParent(a3_auction.instance.transform.FindChild("contents"));
            transform.localScale = Vector3.one;
            btn_help = new BaseButton(transform.FindChild("description"));
            _content = transform.FindChild("cells/scroll/content");
            _select = transform.FindChild("cells/scroll/bagselect").gameObject;
            input = transform.FindChild("price/1/InputField").GetComponent<InputField>();
            help = transform.FindChild("help/panel_help");
            help_par = transform.FindChild("help");
            reducePrice = transform.FindChild("price/reduce/Image/reducePrice").GetComponent<Text>();
            foreach (var v in _content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == _content)
                {
                    ir.Add(v.gameObject);
                }
            }
            sell = new BaseButton(transform.FindChild("sell"));
            sell.onClick = (GameObject g) =>
            {
                int timetwo=0;
                List<SXML> lit = new List<SXML>();
                lit = sml.GetNodeList("time_ratio");      
                    timetwo = lit[0].getInt("put_tm");
                int time = timetwo;
                if (_selectId <= 0 || _selectIdIcon == null) return;
                int price = 0;
                price = int.Parse(input.text) * num;

                if (input.text != null && input.text != "") price = int.Parse(input.text) * num;
                else
                    input.text = (minPrice / num).ToString();//显示默认价格
             
                bool g24 = transform.FindChild("time/1/tgroup/1").GetComponent<Toggle>().isOn;
                bool g48 = transform.FindChild("time/1/tgroup/2").GetComponent<Toggle>().isOn;

                if (g24 == true)
                {
                     time = lit[1].getInt("put_tm");
                }
                else if (g48 == true) {
                        time = lit[2].getInt("put_tm");
                };
                price = int.Parse(input.text) * num;
                // if (price < minPrice * num)
                //{
                //    flytxt.instance.fly(ContMgr.getCont("a3_auction_txtxt"));
                //    return;
                //}

                //debug.Log(price + ":::::::::::" + time);
                A3_AuctionProxy.getInstance().SendPutOnMsg((uint)_selectId, (uint)time, (uint)price, (uint)num);
                //reducePrice.text = "";
                //transform.FindChild("price/3/Image/Text").GetComponent<Text>().text = "";
                inputf.text = 1.ToString();//数量
                num = 1;

            };//上架
            new BaseButton(transform.FindChild("diamond/add")).onClick = (GameObject go) =>
             {
                 InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                 if (a3_Recharge.Instance != null)
                     a3_Recharge.Instance.transform.SetAsLastSibling();
             };
            new BaseButton(transform.FindChild("gold/add")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                if (a3_exchange.Instance != null)
                    a3_exchange.Instance.transform.SetAsLastSibling();
            };

            new BaseButton(transform.FindChild("help/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            new BaseButton(transform.FindChild("help/panel_help/bg_0")).onClick = (GameObject) =>
            {
                help.gameObject.SetActive(false);
                help.SetParent(help_par);
                //help.localPosition = Vector3.zero;
            };
            btn_help.onClick = (GameObject go) =>
            {
                help.gameObject.SetActive(true);
                //help.localPosition = Vector3.zero;
            };

            new BaseButton(getTransformByPath("buynum/bg/contain/btn_reduce")).onClick = (GameObject go) =>
            {
                int nownum;
                if (int.TryParse(inputf.text, out nownum))
                {
                }
                if (nownum - 1 < 1) return;
                inputf.text = (nownum - 1).ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();

            };
            new BaseButton(getTransformByPath("buynum/bg/contain/btn_add")).onClick = (GameObject go) =>
            {
                int nownum;
                if (int.TryParse(inputf.text, out nownum))
                {
                }
                if (nownum + 1 > selectMax) return;
                inputf.text = (nownum + 1).ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };
            new BaseButton(getTransformByPath("buynum/bg/contain/min")).onClick = (GameObject go) =>
            {
                inputf.text = 1.ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };
            new BaseButton(getTransformByPath("buynum/bg/contain/max")).onClick = (GameObject go) =>
            {
                inputf.text = selectMax.ToString();
                if (int.TryParse(inputf.text, out num))
                {
                }
                getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (num * selectPrice).ToString();
            };

            new BaseButton(transform.FindChild("buynum/btn")).onClick = (GameObject go) =>
              {
                  getGameObjectByPath("buynum").SetActive(false);
              };
            new BaseButton(etra.transform.FindChild("info/put")).onClick = (GameObject go) =>
            {
                if (a3_BagModel.getInstance().getItems()[_selectId].confdata.equip_type > 0)
                {
                    if (_selectId != 0 && a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getItems()[_selectId]))
                    {
                        var bd = a3_BagModel.getInstance().getItems()[_selectId];
                        SetInfo(bd);
                    }
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_nojiaoyi"));
                        etra.gameObject.SetActive(false);
                    }
                }

            };
            new BaseButton(edj.transform.FindChild("info/put")).onClick = (GameObject go) =>
              {
                  inputf.text = 1.ToString();
                  listxml.Clear();
                  listxml = XMLMgr.instance.GetSXMLList("item.item", "id==" + a3_BagModel.getInstance().getItems()[_selectId].confdata.tpid);
                  getTransformByPath("buynum/bg/contain/des_bg/Text").GetComponent<Text>().text = StringUtils.formatText(listxml[0].getString("desc"));
                  getTransformByPath("buynum/bg/contain/name").GetComponent<Text>().text = listxml[0].getString("item_name");
                  getTransformByPath("buynum/bg/contain/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listxml[0].getString("icon_file"));
                  getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (a3_BagModel.getInstance().getItems()[_selectId].auctiondata.cost / maxNum).ToString();
                  selectPrice = (a3_BagModel.getInstance().getItems()[_selectId].auctiondata.cost / maxNum);
                  selectMax = maxNum;
                  getGameObjectByPath("buynum").SetActive(true);
                  edj.gameObject.SetActive(false);
              };
            /*input.onValueChanged.AddListener((string str) =>
            {
                //if (int.TryParse(str, out inputNum) && inputNum < minPrice)
                //{
                //    str = minPrice.ToString();
                //    //input.text = str;
                //}

                if (now_data.confdata.equip_type < 1)
                {
                    SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + now_data.tpid);
                    int minnb = xml.getInt("trade");
                    int maxnb = xml.getInt("trade_max");
                    if (int.TryParse(str, out inputNum) && inputNum < minnb)
                    {
                        str = xml.getString("trade");
                        //input.text = str;
                    }
                    if(int.TryParse(str, out inputNum) && inputNum > maxnb)
                    {
                        str = xml.getString("trade_max");
                    }
                    
                }
                else
                {            
                    SXML xdl = sml.GetNode("min_shelf_yb", "quality==" + now_data.confdata.quality);
                    if (int.TryParse(str, out inputNum) && inputNum < xdl.getInt("min_yb"))
                    {
                        str = xdl.getString("min_yb");
                        //input.text = str;
                    }
                    if (int.TryParse(str, out inputNum) && inputNum > xdl.getInt("max_yb"))
                    {
                        str = xdl.getString("max_yb");
                    }
                }

                if (int.TryParse(str, out inputNum))
                {
                    if (inputNum == 1)
                    {
                        reducePrice.text = 0.ToString();
                    }
                    else {
                        reducePrice.text = Mathf.Ceil((inputNum * num * ((float)comper / 100))) + "";
                    }
                   
                    transform.FindChild("price/3/Image/Text").GetComponent<Text>().text = ((int)inputNum * num).ToString();
                }
            });*/
            inputf = getTransformByPath("buynum/bg/contain/bug/InputField").GetComponent<InputField>();
            inputf.text = 1.ToString();
            if (int.TryParse(inputf.text, out num))
            {
            }
            new BaseButton(getTransformByPath("buynum/bg/ok")).onClick = (GameObject go) =>
            {
                if (int.TryParse(inputf.text, out num))
                {
                }
                if (a3_BagModel.getInstance().getItems()[_selectId].confdata.equip_type < 1)
                {
                    var bd = a3_BagModel.getInstance().getItems()[_selectId];
                    SetInfo(bd);
                }
                else
                {
                    if (_selectId != 0 && a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getItems()[_selectId]))
                    {
                        var bd = a3_BagModel.getInstance().getItems()[_selectId];
                        SetInfo(bd);
                    }
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_auction_nojiaoyi"));
                        edj.gameObject.SetActive(false);
                    }
                }
                getGameObjectByPath("buynum").SetActive(false);
            };
            inputf.onValueChanged.AddListener((string ss) =>
            {
                if (int.Parse(inputf.text) > selectMax)
                {
                    inputf.text = selectMax.ToString();
                    num = selectMax;
                }
                else
                    num = int.Parse(inputf.text);
            });
            transform.FindChild("time/1/tgroup/0").GetComponent<Toggle>().onValueChanged.AddListener((bool check) =>
            {
                if (check)
                {
                    List<SXML> lit = new List<SXML>();
                    lit = sml.GetNodeList("time_ratio");
                        needPriceTime = lit[0].getInt("ratio");
                    if (_selectId != 0)
                    {
                        var bd = a3_BagModel.getInstance().getItems()[_selectId];
                        SetNeedPrice(bd);
                    }
                }
            });
            transform.FindChild("time/1/tgroup/1").GetComponent<Toggle>().onValueChanged.AddListener((bool check) =>
            {
                if (check)
                {
                    List<SXML> lit = new List<SXML>();
                    lit = sml.GetNodeList("time_ratio");
                        needPriceTime = lit[1].getInt("ratio");
                    if (_selectId != 0)
                    {
                        var bd = a3_BagModel.getInstance().getItems()[_selectId];
                        SetNeedPrice(bd);
                    }
                }
            });
            transform.FindChild("time/1/tgroup/2").GetComponent<Toggle>().onValueChanged.AddListener((bool check) =>
            {
                if (check)
                {
                    List<SXML> lit = new List<SXML>();
                    lit = sml.GetNodeList("time_ratio");
                        needPriceTime = lit[2].getInt("ratio");
                    if (_selectId != 0)
                    {
                        var bd = a3_BagModel.getInstance().getItems()[_selectId];
                        SetNeedPrice(bd);
                    }
                }
            });
        }

        void inText()
        {
            this.transform.FindChild("price/1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_41");
            this.transform.FindChild("price/2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_42");
            this.transform.FindChild("price/3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_43");
            this.transform.FindChild("price/reduce").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_44");
            this.transform.FindChild("price/reduce/Image/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_45");
            this.transform.FindChild("time/1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_46");
            this.transform.FindChild("time/1/tgroup/0/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_47");
            this.transform.FindChild("time/1/tgroup/1/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_48");
            this.transform.FindChild("time/1/tgroup/2/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_49");
            this.transform.FindChild("time/price/1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_50");
            this.transform.FindChild("sell/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_51");
            this.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_auction_52");
            this.transform.FindChild("help/panel_help/descTxt").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_auction_53"));
        }
        public override void onShowed()
        {
            instans = this;
            transform.FindChild("time/1/tgroup/0").GetComponent<Toggle>().isOn = true;
            needPriceTime = 1;
            etra.gameObject.SetActive(false);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            A3_AuctionProxy.getInstance().addEventListener(A3_AuctionProxy.EVENT_SELLSCUCCESS, Event_OnSellDone);
            transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = "0";
            BeginShow();
            refreshMoney();
            EndClear();
            refreshGold();
        }
        void justShowBuynum()
        {
            inputf.text = 1.ToString();
            listxml.Clear();
            listxml = XMLMgr.instance.GetSXMLList("item.item", "id==" + a3_BagModel.getInstance().getItems()[_selectId].confdata.tpid);
            getTransformByPath("buynum/bg/contain/des_bg/Text").GetComponent<Text>().text = StringUtils.formatText(listxml[0].getString("desc"));
            getTransformByPath("buynum/bg/contain/name").GetComponent<Text>().text = listxml[0].getString("item_name");
            getTransformByPath("buynum/bg/contain/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + listxml[0].getString("icon_file"));
            getTransformByPath("buynum/bg/contain/paymoney/needMoney").GetComponent<Text>().text = (a3_BagModel.getInstance().getItems()[_selectId].auctiondata.cost / maxNum).ToString();
            selectPrice = (a3_BagModel.getInstance().getItems()[_selectId].auctiondata.cost / maxNum);
            selectMax = maxNum;
            getGameObjectByPath("buynum").SetActive(true);
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
        }
        public void refreshMoney()
        {
            Text money = transform.FindChild("gold/Text").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("diamond/Text").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public override void onClosed()
        {
            instans = null;
            etra.gameObject.SetActive(false);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            A3_AuctionProxy.getInstance().removeEventListener(A3_AuctionProxy.EVENT_SELLSCUCCESS, Event_OnSellDone);

        }
        void BeginShow()
        {
            if (_selectIdIcon != null) GameObject.Destroy(_selectIdIcon);
            _selectId = 0;
            _select.transform.SetParent(transform);
            List<GameObject> temp = new List<GameObject>(_gos.Values);
            for (int i = 0; i < temp.Count; i++)
            {
                GameObject.Destroy(temp[i]);
            }
            _gos.Clear();
            input.text = "";
            reducePrice.text = "";
            transform.FindChild("price/3/Image/Text").GetComponent<Text>().text = "";
            sell.interactable = false;
            minPrice = 0;
            transform.FindChild("sellobj/name").GetComponent<Text>().text = "";
            transform.FindChild("sellobj/lv").GetComponent<Text>().text = "";
            transform.FindChild("price/2/Image/Text").GetComponent<Text>().text = "";
            _selectId = 0;
            _select.SetActive(false);
            for (int i = 50; i < ir.Count; i++)
            {
                GameObject lockig = ir[i].transform.FindChild("lock").gameObject;
                if (i >= a3_BagModel.getInstance().curi)
                {
                    lockig.SetActive(true);
                }
                else
                {
                    lockig.SetActive(false);
                }
            }
            List<SXML> list = new List<SXML>();
            List<a3_BagItemData> bag = new List<a3_BagItemData>();
            SXML xml = XMLMgr.instance.GetSXML("item");
            list = xml.GetNodeList("item");

            foreach (var v in a3_BagModel.getInstance().getItems().Values)
            {
                bag.Add(v);
            }
            List<a3_BagItemData> bag2 = new List<a3_BagItemData>();

            for (int j = 0; j < bag.Count; j++)//和表中数据对照显示相应装备或可上架道具
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (bag[j].tpid == list[i].getInt("id") && (list[i].getInt("trade") > 0 || (bag[j].isEquip && a3_BagModel.getInstance().isWorked(bag[j]))))
                    {
                        if ( bag[ j ].confdata.equip_type == 11 || bag[ j ].confdata.equip_type == 12 )//过滤时装
                            break;
                        bag2.Add(bag[j]);
                        //debug.Log("+++===========" + list[i].getString("item_name"));
                        break;
                    }
                }
            }

            for (int i = 0; i < bag2.Count; i++)//出售时的背包
            {
                var go = SetIcon(bag2[i]);
                if (ir.Count >= i) go.transform.SetParent(ir[i].transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                _gos[bag2[i].id] = go;
                List<a3_BagItemData> tp = new List<a3_BagItemData>();
                tp.Add(bag2[i]);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    //SetInfo(tp[0]);
                    if (tp[0].confdata.equip_type < 1)
                    {
                        ShowDJTip(tp[0]);
                    }
                    else
                    {
                        //ShowEPTip(tp[0]);
                        ArrayList l = new ArrayList();
                        l.Add(tp[0]);
                        l.Add(equip_tip_type.tip_forAuc_put);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, l);
                    }
                };

            }
        }
        void EndClear()
        {

        }
        void ShowEPTip(a3_BagItemData data)
        {
            //debug.Log("数量" + data.num);
            maxNum = data.num;
            _selectId = data.id;
            etra.gameObject.SetActive(true);
            ShowEPTip(data);

        }
        void ShowDJTip(a3_BagItemData data)
        {
            //debug.Log("数量" + data.num);
            maxNum = data.num;
            _selectId = data.id;
            justShowBuynum();
            //main.djtip.gameObject.SetActive(true);
            //main.ShowDJTip(data);
        }
        SXML sml = XMLMgr.instance.GetSXML("acution");

        public void SetInfo(a3_BagItemData data)//选择数量后的出售物品icon
        {
            now_data = data;
            SXML xml;
            SXML lis = XMLMgr.instance.GetSXML("acution.parm");
            comper = lis.getInt("procedure");
            itpou = lis.getInt("item_pou");
            eqpou = lis.getInt("eqp_pou");
            jewra = lis.getInt("jew_ratio");
            exrato = lis.getInt("ext_ratio");
            exparday = lis.getInt("expired_days");




            etra.gameObject.SetActive(false);
            if (_selectIdIcon != null) GameObject.DestroyImmediate(_selectIdIcon);
            if (!_gos.ContainsKey(data.id)) return;
            GameObject g = _gos[data.id];
            _select.transform.SetParent(g.transform);
            _select.transform.localPosition = Vector3.zero;
            //_select.transform.SetAsLastSibling();
            _select.SetActive(true);
            _select.transform.SetSiblingIndex(1);
            _selectId = data.id;
            _selectIdIcon = GameObject.Instantiate(g) as GameObject;
            _selectIdIcon.transform.FindChild("bagselect").gameObject.SetActive(false);
            _selectIdIcon.transform.SetParent(transform.FindChild("sellobj/Image"));
            _selectIdIcon.transform.localPosition = Vector3.zero;
            _selectIdIcon.transform.localScale = Vector3.one;
            if (data.confdata.equip_type < 1)
            {
                _selectIdIcon.transform.FindChild("num").gameObject.SetActive(true);
                _selectIdIcon.transform.FindChild("num").GetComponent<Text>().text = num.ToString();
            }
            transform.FindChild("sellobj/name").GetComponent<Text>().text = data.confdata.item_name;
            transform.FindChild("sellobj/name").GetComponent<Text>().color = Globle.getColorByQuality(data.confdata.quality);

   

            if (data.confdata.equip_type < 1)
            {
                transform.FindChild("sellobj/lv").GetComponent<Text>().text = ContMgr.getCont("a3_auction_cailiao");
                xml = XMLMgr.instance.GetSXML("item.item", "id=="+ data.tpid);
                if (xml != null)
                {
                    minPrice = xml.getInt("trade_default");
                    eqplel = xml.getInt("equip_level");
                    input.text = (minPrice).ToString();
                }
            }
            else
            {
                num = 1;
                transform.FindChild("sellobj/lv").GetComponent<Text>().text = "+" + data.equipdata.intensify_lv + ContMgr.getCont("a3_auction_zhui") + data.equipdata.add_level;
                int eqpLevel = a3_EquipModel.getInstance().getEquipByItemId(data.tpid).eqp_level;
                //minPrice = data.equipdata.baseCombpt / 100 + eqpLevel * 10; //Mathf.Max(1, data.equipdata.combpt / 100) + data.equipdata.intensify_lv * data.equipdata.intensify_lv * 50 + data.equipdata.stage * data.equipdata.stage * 100;
                SXML xdl = sml.GetNode("min_shelf_yb", "quality==" + data.confdata.quality); ;
                minPrice = xdl.getInt("default_yb");
                //foreach (var v in data.equipdata.gem_att.Values)
                //{
                //    minPrice += v;
                //}
                input.text = minPrice.ToString();
                reducePrice.text = (int)(minPrice * ((float)comper / 100)) + "";
                //minPrice += data.equipdata.add_level * data.equipdata.add_level;
            }
            if (data.confdata.equip_type < 1)
            {
                transform.FindChild("price/2/Image/Text").GetComponent<Text>().text = minPrice + ContMgr.getCont("a3_auction_gegong");//,共" + num * minPrice + "钻
                reducePrice.text = (int)((num * minPrice) * ((float)comper/100)) + "";
                
            }
            else
            {
                SXML lio = sml.GetNode("min_shelf_yb", "quality==" + data.confdata.quality);
                transform.FindChild("price/2/Image/Text").GetComponent<Text>().text = lio.getInt("min_yb") + "";
            }

            if (data.confdata.equip_type > 0)
            {
                xml = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
                if (xml != null)
                {
                    eqplel = xml.getInt("equip_level");  
                }
                int x = eqpou * eqplel;
              
                if (data.confdata.equip_type == 8 || data.confdata.equip_type == 9 || data.confdata.equip_type == 10)
                {
                    x = eqpou*10* eqplel;
                }

                x *= needPriceTime;
                transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = x.ToString();
            }
            else
            {
                int x1 = itpou* data.confdata.quality* num;
                x1 *= needPriceTime;
                // x1 = data.confdata.quality * x1 * num * needPriceTime;//基础值*品质值*数量  基础值100
                transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = x1.ToString();
            }


            sell.interactable = true;
        }
        GameObject SetIcon(a3_BagItemData data)
        {
            GameObject go = IconImageMgr.getInstance().createA3ItemIcon(data.confdata, true, -1, 1f, false, -1, 0, false, false);
            if (data.confdata.equip_type < 1)//显示数量
            {
                go.transform.FindChild("num").gameObject.SetActive(true);
                go.transform.FindChild("num").GetComponent<Text>().text = data.num.ToString();
            }

            go.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
            go.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
            return go;
        }

        void SetNeedPrice(a3_BagItemData data)
        {
            //int x = ((int)data.tpid / 10) % 10;
            //x = (x + 1) * 10000;
            //if (data.confdata.equip_type == 8 || data.confdata.equip_type == 9 || data.confdata.equip_type == 10)
            //{
            //    x = 100000;
            //}
            //x *= needPriceTime;
            //transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = x.ToString();
            if (data.confdata.equip_type > 0)
            {
                int x =eqpou*eqplel;
               
                if (data.confdata.equip_type == 8 || data.confdata.equip_type == 9 || data.confdata.equip_type == 10)
                {
                    x = eqpou * eqplel* jewra;
                }

                x *= needPriceTime;
                transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = x.ToString();
            }
            else
            {
               
                int x1 = itpou*data.confdata.quality* num ;//基础值*品质值*数量  基础值100
                x1 *= needPriceTime;
                transform.FindChild("time/price/1/Image/Text").GetComponent<Text>().text = x1.ToString();
            }
        }

        void Event_OnSellDone(GameEvent e)
        {
            EndClear();
            BeginShow();
            flytxt.instance.fly(ContMgr.getCont("a3_auction_oksell") + e.data["auc_data"]._arr[0]["name"] + "*" + e.data["auc_data"]._arr[0]["num"]);
        }

        a3_BagItemData now_data = null;
         void Update()
         {
            if (now_data == null)
                return;
            if (now_data.confdata.equip_type < 1)
            {
                SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + now_data.tpid);
                int minnb = xml.getInt("trade");
                int maxnb = xml.getInt("trade_max");
                if(!int.TryParse(input.text, out inputNum))
                {
                    input.text = xml.getString("trade");
                }
                if (int.TryParse(input.text, out inputNum) && inputNum < minnb)
                {
                  input.text = xml.getString("trade");
                    //input.text = str;
                }
                if(int.TryParse(input.text, out inputNum) && inputNum > maxnb)
                {
                   input.text = xml.getString("trade_max");
                }

            }
            else
            {            
                SXML xdl = sml.GetNode("min_shelf_yb", "quality==" + now_data.confdata.quality);
                if(!int.TryParse(input.text, out inputNum))
                {
                    input.text = xdl.getString("min_yb");
                }
                if (int.TryParse(input.text, out inputNum) && inputNum < xdl.getInt("min_yb"))
                {
                  input.text = xdl.getString("min_yb");
                    //input.text = str;
                }
                if (int.TryParse(input.text, out inputNum) && inputNum > xdl.getInt("max_yb"))
                {
                   input.text = xdl.getString("max_yb");
                }
            }

            if (int.TryParse(input.text, out inputNum))
            {
                if (inputNum == 1)
                {
                    reducePrice.text = 0.ToString();
                }
                else {
                    reducePrice.text = Mathf.Ceil((inputNum * num * ((float)comper / 100))) + "";
                }

                transform.FindChild("price/3/Image/Text").GetComponent<Text>().text = ((int)inputNum * num).ToString();
            }
        }
    }
}
