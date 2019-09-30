using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MuGame
{
    enum inheritType
    {
        _null = 0,
        eqp = 1,
        baoshi = 2
    }

    class inheritinfo {
       public uint id1;
       public uint id2;
       public inheritType type;
    }

    class a3_eqpInherit : Window
    {

        Transform eqpIcon1 = null;
        Transform eqpIcon2 = null;
        inheritType inheritType = inheritType._null;

        public override void init()
        {
            inText();
            eqpIcon1 = this.transform.FindChild("eqp1");
            eqpIcon2 = this.transform.FindChild("eqp2");
            new BaseButton(transform.FindChild("retun")).onClick =
            new BaseButton(transform.FindChild("no")).onClick = (GameObject go) =>
            {
                a3_EquipModel.getInstance().Attchange_wite = false;
                a3_attChange.instans.runTxt(null);
                onover();
            };

            new BaseButton(transform.FindChild("yes")).onClick = (GameObject go) =>
            {
                if (inheritType == inheritType.eqp)
                    EquipProxy.getInstance().sendInherit(eqp1_id, eqp2_id, 1, false);
                else if (inheritType == inheritType.baoshi)
                    EquipProxy.getInstance().sendInherit_baoshi(eqp1_id, eqp2_id);

              
            };

        }

        void inText()
        {
            this.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_1");//确定传承
            this.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_2");//不传承
            this.transform.FindChild("info").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_3");//是否把旧装备的强化等级、精炼等级以及追加等级传承到新装备上
            this.transform.FindChild("old").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_4");//旧装备
            this.transform.FindChild("new").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_5");//新装备
            this.transform.FindChild("money/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_6");//传承需花费：
            this.transform.FindChild("retunText").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_7");
            this.transform.FindChild("retun/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_eqpInherit_8");//确定
        }

        uint eqp1_id = 0;
        uint eqp2_id = 0;

        List< inheritinfo> datalist = new List<inheritinfo>();

        void onover() {
            inheritType = inheritType._null;
            eqp1_id = 0;
            eqp2_id = 0;
            if (datalist.Count > 0)
            {
                SetShow();
            }
            else {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQPINHERIT);
            }
        }

        public override void onShowed()
        {
            if (uiData != null && uiData.Count > 1)
            {
                //eqp1_id = (uint)uiData[0];
                //eqp2_id = (uint)uiData[1];
                //inheritType = (inheritType)uiData[2];
                //if (eqp2_id == eqp1_id)
                //{
                //    //InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQPINHERIT);
                //    return;
                //}
                inheritinfo info =  new inheritinfo();
                info.id1 = (uint)uiData[0];
                info.id2 = (uint)uiData[1];
                info.type = (inheritType)uiData[2];
                datalist.Add(info);
            }

            if (datalist.Count <= 0) {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQPINHERIT);
                return;
            }

            transform.SetAsLastSibling();
            EquipProxy.getInstance().addEventListener(EquipProxy.EVENT_EQUIP_INHERIT, onEquipInherit);
            SetShow();
        }

        void SetShow()
        {
            if (eqp1_id == 0 && eqp2_id == 0)
            {
                eqp1_id = datalist[0].id1;
                eqp2_id = datalist[0].id2;
                inheritType = datalist[0].type;
            }
            else return;
            clearicon();
            GameObject icon1 = IconImageMgr.getInstance().createA3ItemIcon(a3_EquipModel.getInstance().getEquipByAll(eqp1_id));
            icon1.transform.SetParent(eqpIcon1, false);
            icon1.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            GameObject icon2 = IconImageMgr.getInstance().createA3ItemIcon(a3_EquipModel.getInstance().getEquipByAll(eqp2_id));
            icon2.transform.SetParent(eqpIcon2, false);
            icon2.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            transform.FindChild("value1").gameObject.SetActive(false);
            transform.FindChild("value2").gameObject.SetActive(false);
            if (inheritType == inheritType.eqp)
            {
                transform.FindChild("value1").gameObject.SetActive(true);
                transform.FindChild("value2").gameObject.SetActive(true);
                SXML s_xml_inherit = XMLMgr.instance.GetSXML("item.inheritance", "equip_stage==" + a3_EquipModel.getInstance().getEquipByAll(eqp1_id).equipdata.stage);
                transform.FindChild("money").GetComponent<Text>().text = s_xml_inherit.getString("money");
                transform.FindChild("money").gameObject.SetActive(true);
                transform.FindChild("info").GetComponent<Text>().text = ContMgr.getCont("a3_eqpInherit_eqptop");
                setValue_tishi_cc();
            }
            else if (inheritType == inheritType.baoshi)
            {
                transform.FindChild("retun").gameObject.SetActive(false);
                transform.FindChild("retunText").gameObject.SetActive(false);
                transform.FindChild("money").gameObject.SetActive(false);
                transform.FindChild("info").GetComponent<Text>().text = ContMgr.getCont("a3_eqpInherit_baoshitop");
            }

            datalist.Remove(datalist[0]);
        }
        void setValue_tishi_cc()
        {
            a3_BagItemData data = a3_EquipModel.getInstance().getEquipByAll(eqp2_id);
            a3_BagItemData data1 = a3_EquipModel.getInstance().getEquipByAll(eqp1_id);
            string[] list_need1;
            string[] list_need2;
            int need1;
            int need2;
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + (data1.equipdata.stage)).GetNode("stage_info", "itemid==" + data.tpid);
            if (stage_xml == null)
            {
                return;
            }
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + data.equipdata.blessing_lv);
            list_need1 = stage_xml.getString("equip_limit1").Split(',');
            list_need2 = stage_xml.getString("equip_limit2").Split(',');
            need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            string text_need1, text_need2;
            bool cando1 = true;
            bool cando2 = true;
            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])])
            {
                cando1 = true;
                // text_need1 = " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")" + "</color>";
                text_need1 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            else
            {
                cando1 = false;
                // text_need1 = " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + ")" + "</color>";
                text_need1 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need1[0]) + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
            }
            if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])])
            {
                cando2 = true;
                //  text_need2 = " <color=#00FF00>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#00FF00>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }
            else
            {
                cando2 = false;
                //text_need2 = " <color=#f90e0e>" + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "(" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + ")" + "</color>";
                text_need2 = "<color=#f90e0e>" + Globle.getString(int.Parse(list_need2[0]) + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
            }

            if (cando1 && cando2)
            {
                transform.FindChild("yes").gameObject.SetActive(true);
                transform.FindChild("no").gameObject.SetActive(true);
                transform.FindChild("money").gameObject.SetActive(true);
                transform.FindChild("retun").gameObject.SetActive(false);
                transform.FindChild("retunText").gameObject.SetActive(false);
            }
            else
            {
                transform.FindChild("yes").gameObject.SetActive(false);
                transform.FindChild("no").gameObject.SetActive(false);
                transform.FindChild("money").gameObject.SetActive(false);
                transform.FindChild("retun").gameObject.SetActive(true);
                transform.FindChild("retunText").gameObject.SetActive(true);
            }
            transform.FindChild("value1").GetComponent<Text>().text = text_need1;
            transform.FindChild("value2").GetComponent<Text>().text = text_need2;
        }

        void onEquipInherit(GameEvent e)
        {
            Debug.LogError(e.data.dump ());
            if (eqp2_id != e.data["to_eqpinfo"]["id"]) { return; }
            if (a3_bag.isshow)
                a3_bag.isshow.refOneEquipIcon(eqp2_id);
            flytxt.instance.fly(ContMgr.getCont("a3_eqpInherit"));
            eqp1_id = 0;
            eqp2_id = 0;
            onover();
            a3_EquipModel.getInstance().Attchange_wite = false;
            a3_attChange.instans.runTxt(null);
        }
        void clearicon()
        {
            for (int i = 0; i < eqpIcon1.childCount; i++)
            {
                Destroy(eqpIcon1.GetChild(i).gameObject);
            }
            for (int i = 0; i < eqpIcon2.childCount; i++)
            {
                Destroy(eqpIcon2.GetChild(i).gameObject);
            }
        }

        public override void onClosed()
        {
            EquipProxy.getInstance().removeEventListener(EquipProxy.EVENT_EQUIP_INHERIT, onEquipInherit);
            
            inheritType = inheritType._null;
            eqp1_id = 0;
            eqp2_id = 0;
            a3_EquipModel.getInstance().Attchange_wite = false;
        }
    }
}
