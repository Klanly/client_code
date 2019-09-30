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
    class a3_LHlianjie : Window
    {
        public static a3_LHlianjie instance;
        Dictionary<int, GameObject> tipObj = new Dictionary<int, GameObject>();
        RectTransform con;
        //Transform isthis;
        public override void init()
        {
            intext();
            instance = this;
            BaseButton Closelianjie = new BaseButton(transform.FindChild("close"));
            BaseButton Closelianjie1 = new BaseButton(transform.FindChild("touch"));
            Closelianjie.onClick = onClose;
            Closelianjie1.onClick = onClose;
            con = transform.FindChild("view/con").GetComponent<RectTransform>();
            //isthis = transform.FindChild("isthis");
            //initLianjie();
        }

        void intext()
        {
            this.transform.FindChild("help/descTxt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_LHlianjie_1");//1,金装具有灵魂属性，这个属性需要使用对应部位的对应灵魂属性激活才会生效。{n} 2,部位对应：头 > 肩 > 胸 > 腿 > 鞋 > 武器 > 副手 > 项链 > 戒指 > 指环 > 头。{n} 3,属性对应：冰 > 风 > 火 > 光 > 雷 > 冰。

        }
        public override void onShowed()
        {
            //refresh();
            transform.SetAsLastSibling();
           // setCon();
        }

        public override void onClosed() { }

        public void initLianjie()
        {
            GameObject item = transform.FindChild("view/item").gameObject;
            
            if (con.childCount > 0)
            {
                for (int i = 0; i < con.childCount; i++)
                {
                    Destroy(con.GetChild(i).gameObject);
                }
            }
            SXML dSXML = XMLMgr.instance.GetSXML("activate_fun.activate_num");
            List<SXML> LHcount = dSXML.GetNodeList("num");
            foreach (SXML it in LHcount)
            {
                GameObject clon = Instantiate(item) as GameObject;
                Text qh_lvl = clon.transform.FindChild("count").GetComponent<Text>();
                qh_lvl.text = it.getInt("cout").ToString ();
                List<SXML> info = it.GetNodeList("type");
                GameObject info_item = clon.transform.FindChild("scrollview/info_item").gameObject;
                RectTransform con_info = clon.transform.FindChild("scrollview/con").GetComponent<RectTransform>();
                foreach (SXML it_info in info)
                {
                    GameObject info_clon = Instantiate(info_item) as GameObject;
                    Text info_text = info_clon.transform.FindChild("Text").GetComponent<Text>();
                    info_text.text = Globle.getAttrNameById(it_info.getInt("att_type")) +"+" + it_info.getInt("att_value");
                    info_clon.SetActive(true);
                    info_clon.transform.SetParent(con_info, false);
                }
                clon.SetActive(true);
                clon.transform.SetParent(con, false);
                tipObj[it.getInt("cout")] = clon;
            }
            float childSizeX = item.transform.GetComponent<RectTransform>().sizeDelta.x;
            float spacing = con.GetComponent<GridLayoutGroup>().spacing.x;
            Vector2 newSize = new Vector2(LHcount.Count * (childSizeX + spacing), con.sizeDelta.x);
            con.sizeDelta = newSize;
        }
        void setCon()
        {
            int count = a3_EquipModel.getInstance().active_eqp.Count;
            float con_x = con.GetComponent<GridLayoutGroup>().cellSize.x;

            if (count <= 1)
            {
                con.anchoredPosition = new Vector2(0, con.anchoredPosition.y);
            }
            else if (count >= 7)
            {
                con.anchoredPosition = new Vector2(-(6 * con_x), con.anchoredPosition.y);
            }
            else
            {
                con.anchoredPosition = new Vector2(-((count - 1)* con_x), con.anchoredPosition.y);
            }
           
        }

        //public void refresh()
        //{
        //    int count = a3_EquipModel.getInstance().active_eqp.Count;
        //    if (count <= 0)
        //    {
        //        isthis.gameObject.SetActive(false);
        //        return;
        //    }
        //    isthis.gameObject.SetActive(true);
        //    isthis.transform.SetParent(tipObj[count].transform.FindChild("it"),false);
        //    isthis.localPosition = Vector3.zero;
        //}

        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LHLIANJIE);
        }
    }
}
