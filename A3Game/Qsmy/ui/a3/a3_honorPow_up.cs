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
    class a3_honorPow_up : Window
    {
        GameObject info_item;
        RectTransform con_info;
        public override void init() {
            info_item = transform.FindChild("attVeiw/info_item").gameObject;
            con_info = transform.FindChild("attVeiw/con").GetComponent<RectTransform>();
            new BaseButton (this.transform.FindChild("btn_close")).onClick =
            new BaseButton(this.transform.FindChild("close")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().close(this.uiName);
            };
        }
        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
           
            setview();
        }


        void setview()
        {

            for (int i = 0; i < con_info.childCount; i++)
            {
                Destroy(con_info.GetChild(i).gameObject);
            }
            int lvl = a3_EquipModel.getInstance().getHonorPowlvl();
            SXML xml = XMLMgr.instance.GetSXML("strength_of_honor");
            List<SXML> list = xml.GetNodeList("strength");
            List<SXML> attlist = xml.GetNode("strength", "lv==" + lvl).GetNodeList("att");
            Dictionary<int, int> att = new Dictionary<int, int>();
            foreach (SXML it_info in attlist)
            {
                if (att.ContainsKey(it_info.getInt("type")))
                    att[it_info.getInt("type")] += it_info.getInt("value");
                else
                    att[it_info.getInt("type")] = it_info.getInt("value");
            }
            Dictionary<int, int> att_old = new Dictionary<int, int>();
            if (lvl > 1)
            {
                List<SXML> attlist1 = xml.GetNode("strength", "lv==" + (lvl - 1)).GetNodeList("att");
                foreach (SXML it_info in attlist1)
                {
                    if (att_old.ContainsKey(it_info.getInt("type")))
                        att_old[it_info.getInt("type")] += it_info.getInt("value");
                    else
                        att_old[it_info.getInt("type")] = it_info.getInt("value");
                }
            }
            foreach (int type in att.Keys)
            {
                GameObject info_clon = Instantiate(info_item) as GameObject;
                info_clon.SetActive(true);
                info_clon.transform.SetParent(con_info, false);
                info_clon.transform.FindChild("old").GetComponent<Text>().text = Globle.getAttrNameById(type) + " +" + att[type];
                if(att_old.ContainsKey (type))
                    info_clon.transform.FindChild("new").GetComponent<Text>().text = "+" + (att[type] - att_old[type]);
                else
                    info_clon.transform.FindChild("new").GetComponent<Text>().text = "+" + (att[type] - 0);
            }
        }
        public override void onClosed()
        {
            a3_EquipModel.getInstance().Attchange_wite = false;
            a3_attChange.instans?.runTxt(null);
        }
    }
}
