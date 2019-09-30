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
    class a3_QHmaster: Window
    {
        public static a3_QHmaster instance;
        public override void init()
        {
            instance = this;
            refreshDashi();
            BaseButton CloseDAshi = new BaseButton(transform.FindChild("close"));
            BaseButton CloseDashi1 = new BaseButton(transform.FindChild("touch"));
            CloseDAshi.onClick = onCloseDashi;
            CloseDashi1.onClick = onCloseDashi;
        }
        public override void onShowed() {
            transform.SetAsLastSibling();
        }
        public override void onClosed() { }
        public void refreshDashi()
        {
            GameObject item = transform.FindChild("view/item").gameObject;
            RectTransform con = transform.FindChild("view/con").GetComponent<RectTransform>();
            if (con.childCount > 0)
            {
                for (int i = 0; i < con.childCount; i++)
                {
                    Destroy(con.GetChild(i).gameObject);
                }
            }
            SXML dSXML = XMLMgr.instance.GetSXML("intensifymaster.level", "lvl==" + PlayerModel.getInstance().up_lvl);
            List<SXML> DSlvl = dSXML.GetNodeList("intensify");
            foreach (SXML it in DSlvl)
            {
                GameObject clon = Instantiate(item) as GameObject;
                Text qh_lvl = clon.transform.FindChild("qh_lvl/Text").GetComponent<Text>();
                qh_lvl.text = "+" + it.getInt("qh");
                List<SXML> info = it.GetNodeList("att");
                GameObject info_item = clon.transform.FindChild("scrollview/info_item").gameObject;
                RectTransform con_info = clon.transform.FindChild("scrollview/con").GetComponent<RectTransform>();
                foreach (SXML it_info in info)
                {
                    GameObject info_clon = Instantiate(info_item) as GameObject;
                    Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                    info_text.text = "+" + it_info.getInt("value") + Globle.getAttrNameById(it_info.getInt("type"));
                    info_clon.SetActive(true);
                    info_clon.transform.SetParent(con_info, false);
                }
                clon.SetActive(true);
                clon.transform.SetParent(con, false);
            }
            float childSizeX = item.transform.GetComponent<RectTransform>().sizeDelta.x;
            float spacing = con.GetComponent<GridLayoutGroup>().spacing.x;
            Vector2 newSize = new Vector2(DSlvl.Count * (childSizeX + spacing), con.sizeDelta.x);
            con.sizeDelta = newSize;
        }
        void onCloseDashi(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_QHMASTER);
        }
    }
}
