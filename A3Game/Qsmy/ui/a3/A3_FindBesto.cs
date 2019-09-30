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
    class A3_FindBesto : Window
    {
        Transform itemViewCon;
        GameObject isthis;
        GameObject tip;
        GameObject tishi;
        public static A3_FindBesto instan;
        public override void init() {

            instan = this;
            tip = this.transform.FindChild("close_desc").gameObject;
            tishi = this.transform.FindChild("tishi").gameObject;
            new BaseButton(this.transform.FindChild("close")).onClick = (GameObject go) =>
           {
               InterfaceMgr.getInstance().close(InterfaceMgr.A3_FINDBESTO);
           };
            new BaseButton(this.transform.FindChild("do")).onClick = (GameObject go) => 
            {
                if (count >= 0)
                {
                    tishi.SetActive(true);
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("A3_FindBesto_cochange"));
                }
            };
            new BaseButton(this.transform.FindChild("bg/close_bg")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_FINDBESTO);
            };

            new BaseButton(tishi.transform.FindChild ("yes")).onClick = (GameObject go)=>
            {
                FindBestoProxy.getInstance().sendMap(count);
                tishi.SetActive(false);
            };
            new BaseButton(tishi.transform.FindChild("no")).onClick = (GameObject go) =>
            {
                tishi.SetActive(false);
            };
            itemViewCon = this.transform.FindChild("body/itemView/content");
            isthis = this.transform.FindChild("body/itemView/this").gameObject;
            intoUI();
            inText();
        }

        void inText()
        {
            this.transform.FindChild("close_desc/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_FindBesto_1");//使用限制：
            this.transform.FindChild("close_desc/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_FindBesto_2");//已拥有：
            this.transform.FindChild("tishi/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_FindBesto_3");//一天只可以兑换一次奖励，兑换后多余的宝图将被清空，是否兑换？
            this.transform.FindChild("tishi/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_FindBesto_4");//返回
            this.transform.FindChild("tishi/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_FindBesto_5");//确定
        }
        public override void onShowed()
        {
           // FindBestoProxy.getInstance().addEventListener(FindBestoProxy.EVENT_INFO, oninfo);
            //FindBestoProxy.getInstance().getinfo();
            count = -1;
            isthis.gameObject.SetActive(false);
            tishi.SetActive(false);
            refreCount();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
        }
        public override void onClosed()
        {
            //FindBestoProxy.getInstance().removeEventListener(FindBestoProxy.EVENT_INFO, oninfo);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        }

        public void refreCount()
        {
            this.transform.FindChild("count").GetComponent<Text>().text = PlayerModel.getInstance().treasure_num.ToString();
        }
        bool open = false;
        public int count = -1;
        //void oninfo(GameEvent e)
        //{
        //    Variant data = e.data;
        //    if (data.ContainsKey("mapid") && data["mapid"].Count > 0)
        //        open = true;
        //    else
        //    {
        //        open = false;
        //    }
        //}

        public void intoUI()
        {
            GameObject item = this.transform.FindChild("body/itemView/item").gameObject;
            //RectTransform con = this.transform.FindChild("body/awardItems/content").GetComponent<RectTransform>();
            SXML xml = XMLMgr.instance.GetSXML("treasure_reward");
            List<SXML> stagelist = xml.GetNodeList("reward");
            for (int i = 0; i < stagelist.Count;i++)
            {
                GameObject clon = (GameObject)Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(itemViewCon, false);
                clon.transform.FindChild("name").GetComponent<Text>().text = stagelist[i].getString("name");
                clon.transform.FindChild("count").GetComponent<Text>().text ="x"+ stagelist[i].getInt("cost").ToString();
                clon.transform.FindChild ("num").GetComponent<Text>().text  = stagelist[i].getInt("nums").ToString ();
                int id = stagelist[i].getInt("item_id");
                GameObject con_item = clon.transform.FindChild("icon").gameObject;
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById((uint)id),false, -1, 0.8f);
                icon.transform.SetParent(con_item.transform,false);

                new BaseButton(con_item.transform).onClick = (GameObject go) => 
                {
                    tip.SetActive(true);
                    SXML x = XMLMgr.instance.GetSXML("item.item", "id==" + id);
                    tip.transform.FindChild ("text_bg/name/namebg").GetComponent<Text>().text = x.getString("item_name");
                    tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid((uint)id) + ContMgr.getCont("ge");
                    tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(x.getInt ("quality"));
                    if (x.getInt("use_limit") == 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = "无限制"; }
                    else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = x.getString("use_limit") + "转"; }
                    tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(x.getString("desc"));
                    tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + x.getInt("icon_file"));
                    new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
                };

                clon.name = stagelist[i].getInt("id").ToString();

                new BaseButton(clon.transform).onClick = (GameObject go) => 
                {
                    count = int.Parse(go.name);
                    isthis.gameObject.SetActive(true);
                    isthis.transform.SetParent(go.transform);
                    isthis.transform.localPosition = Vector2.zero;
                };
            }
            //for (int j = 0;j < 2;j++)
            //{
            //    GameObject clon = (GameObject)Instantiate(item);
            //    clon.SetActive(true);
            //    clon.transform.SetParent(itemViewCon,false);
            //}
            //float childSizeY = con.GetComponent<GridLayoutGroup>().cellSize.y;
            //Vector2 newSize = new Vector2(con.sizeDelta.x,count * childSizeY);
            //con.sizeDelta = newSize;
        }
    }

}
