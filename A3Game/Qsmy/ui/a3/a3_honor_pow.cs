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
    class a3_honor_pow : Window
    {
       // GameObject help;
        public override void init() {
            inText();
           // help = transform.FindChild("help_view").gameObject;
            new BaseButton(this.transform.FindChild("close")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_HONOR_POW);
            };
            //new BaseButton(this.transform.FindChild("help")).onClick = (GameObject go) => {
            //    help.SetActive(true);
            //};
            //new BaseButton(help.transform.FindChild ("close_tach")).onClick =(GameObject go)=> {
            //    help.SetActive(false);
            //};

        }

        void inText() {
            this.transform.FindChild("title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_pow_1");//荣耀之力
            this.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_pow_2");//紫色以上装备会加持荣耀之力，所有装备荣耀之力总和，会激活荣耀之力等级。
            this.transform.FindChild("need").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_pow_3");//荣耀之力

        }
        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
           // help.SetActive(false);
            setValue();

        }

        void setValue()
        {
            int honor_mun = 0;
            int lvl = 0;
            Dictionary<uint, a3_BagItemData> eqp = a3_EquipModel.getInstance().getEquips();
            foreach (a3_BagItemData it in eqp.Values)
            {
                if (a3_EquipModel .getInstance ().checkCanEquip (it))
                    honor_mun = it.equipdata.honor_num + honor_mun;
            }
            SXML xml = XMLMgr.instance.GetSXML("strength_of_honor");
            List<SXML> list = xml.GetNodeList("strength");
            if (honor_mun >= list[list.Count - 1].getInt("value"))
                lvl = list.Count;
            else
            {
                for (int i = 1; i <= list.Count; i++)
                {
                    if (honor_mun < list[i - 1].getInt("value"))
                    {
                        lvl = i - 1;
                        break;
                    }
                }
            }
            debug.Log("cunimni"+ honor_mun);
            this.transform.FindChild("lvl").GetComponent<Text>().text = lvl + ContMgr.getCont("ji");
            if (lvl <= 0) {
                //transform.FindChild("attVeiw").gameObject.SetActive(false);
                //transform.FindChild("tishi").gameObject.SetActive(true);
                List<SXML> attlist = list[1 - 1].GetNodeList("att");
                GameObject info_item_1 = transform.FindChild("attVeiw/info_item_noactive").gameObject;
                RectTransform con_info = transform.FindChild("attVeiw/con").GetComponent<RectTransform>();
                for (int i = 0; i < con_info.childCount; i++)
                {
                    Destroy(con_info.GetChild(i).gameObject);
                }
                foreach (SXML it_info in attlist)
                {
                    GameObject info_clon = Instantiate(info_item_1) as GameObject;
                    Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                    info_text.text = Globle.getAttrNameById(it_info.getInt("type")) + " +" + it_info.getInt("value");
                    info_clon.SetActive(true);
                    info_clon.transform.SetParent(con_info, false);
                }

            }
            else {
                //transform.FindChild("attVeiw").gameObject.SetActive(true);
                //transform.FindChild("tishi").gameObject.SetActive(false);
                List<SXML> attlist = list[lvl - 1].GetNodeList("att");
                GameObject info_item = transform.FindChild("attVeiw/info_item").gameObject;
                RectTransform con_info = transform.FindChild("attVeiw/con").GetComponent<RectTransform>();
                for (int i = 0; i < con_info.childCount; i++)
                {
                    Destroy(con_info.GetChild(i).gameObject);
                }
                foreach (SXML it_info in attlist)
                {
                    GameObject info_clon = Instantiate(info_item) as GameObject;
                    Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                    info_text.text = Globle.getAttrNameById(it_info.getInt("type")) + " +" + it_info.getInt("value") ;
                    info_clon.SetActive(true);
                    info_clon.transform.SetParent(con_info, false);
                }
            }

            SXML next = xml.GetNode("strength", "lv==" + (lvl + 1));
            if (next != null)
            {
                this.transform.FindChild("need").gameObject.SetActive(true);
                this.transform.FindChild("max").gameObject.SetActive(false);
                this.transform.FindChild("need/Text").GetComponent<Text>().text = (next.getInt("value") - honor_mun).ToString ();

            }
            else {
                this.transform.FindChild("need").gameObject.SetActive(false);
                this.transform.FindChild("max").gameObject.SetActive(true);
            }



        }

        public override void onClosed() { }

    }
}
