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
    class a3_timegifs : Window
    {
        public static bool showover = false;

        GameObject[] objs = new GameObject[7];

        Text des,
             des1,
             price;

        GameObject image,
                   contain,
                   tip;

        public static a3_timegifs instance;

        public override void init()
        {
            instance = this;
            for (int i = 0; i < 7; i++)
            {
                objs[i] = getTransformByPath("gifsname").GetChild(i).gameObject;
            }
            des = getComponentByPath<Text>("Text1");
            des1 = getComponentByPath<Text>("Text2");
            des1.text = ContMgr.getCont("timegifs_desc");
            price = getComponentByPath<Text>("price/num");

            image = getGameObjectByPath("Panel/Image");
            contain = getGameObjectByPath("Panel/contain");
            tip = getGameObjectByPath("tip");
            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_TIMEGIFS);
              };

            new BaseButton(getTransformByPath("Button")).onClick = (GameObject go) =>

            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_TIMEGIFS);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3);
            };
        }

        public override void onShowed()
        {
            int today = A3_SevendayModel.getInstance().thisday;
            creatrve(today);
        }

        int ts_count = XMLMgr.instance.GetSXML("welfare.daily_gift").GetNodeList("gift").Count;

        void creatrve(int thisday)
        {

            for (int j = 0; j < objs.Length; j++)
            {
                objs[j].SetActive(j == thisday - 1 ? true : false);
            }

            SXML x = XMLMgr.instance.GetSXML("welfare.daily_gift");
            des.text = x.GetNode("gift", "id==" + thisday).getString("note");
            price.text = x.GetNode("gift", "id==" + thisday).getString("yb");



            List<SXML> lst = XMLMgr.instance.GetSXML("welfare.daily_gift").GetNode("gift", "id==" + thisday).GetNodeList("item");
           


            foreach (SXML s in lst)
            {
                GameObject clone = GameObject.Instantiate(image) as GameObject;
                clone.SetActive(true);
                clone.transform.SetParent(contain.transform, false);
                uint item_id = s.getUint("id");
                GameObject go = IconImageMgr.getInstance().createA3ItemIcon(item_id, false, -1, 1, true);
                go.transform.SetParent(clone.transform.FindChild("icon").transform, false);
                clone.transform.FindChild("name/Text").GetComponent<Text>().text = s.getString("name");
                new BaseButton(go.transform).onClick = (GameObject gos) =>
                 {
                     showtip(item_id);
                 };
            }
            a3_runestone.commonScroview(contain, lst.Count);




        }
        public void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            transform.FindChild("tip/text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }
        public override void onClosed()
        {
            showover = true;
        }





    }
}
