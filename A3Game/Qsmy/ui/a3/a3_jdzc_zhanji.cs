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
    class a3_jdzc_zhanji : Window
    {
        ScrollControler scrollControler;
        public override void init()
        {

            this.transform.FindChild("Image/title1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_5");//玩家
            this.transform.FindChild("Image/title2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_6");//等级
            this.transform.FindChild("Image/title3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_7");//战绩
            this.transform.FindChild("Image/title4").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_8");//战力
            this.transform.FindChild("Image/title5").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_9");//荣誉

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("scroll_rect").GetComponent<ScrollRect>();
            scrollControler.create(scroll);
            new BaseButton(this.transform.FindChild("close")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().close(InterfaceMgr .A3_JDZC_ZHANJI );
            };

            new BaseButton(this.transform.FindChild("close_fb")).onClick = (GameObject go) =>
            {
                a3_insideui_fb.instance.outfb(go);
            };
        }

        Dictionary<int, info_teamPlayer> data;
        public override void onShowed()
        {
            if (uiData != null && uiData.Count > 0)
            {
                data = (Dictionary<int, info_teamPlayer>)uiData[0];
                openzhanji(data);
                this.transform.SetAsLastSibling();
            }
            else {

            }
        }

        public void openzhanji(Dictionary<int, info_teamPlayer> data)
        {
           Transform Con = this.transform .FindChild("scroll_rect/contain");
            if (Con.childCount > 0)
            {
                for (int i = 0; i < Con.childCount; i++)
                {
                    Destroy(Con.GetChild(i).gameObject);
                }
            }
            GameObject one = this.transform .FindChild("scroll_rect/item_zhanli").gameObject;
            foreach (int id in data.Keys)
            {
                GameObject clon = Instantiate(one) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(Con, false);
                clon.transform.FindChild("carricon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_" + data[id].carr);
                clon.transform.FindChild("name").GetComponent<Text>().text = data[id].name;
                clon.transform.FindChild("lvl").GetComponent<Text>().text = ContMgr.getCont("worldmap_lv", new List<string> { data[id].zhuan.ToString(), data[id].lvl.ToString() });
                clon.transform.FindChild("zhanji").GetComponent<Text>().text = data[id].kill_cnt + "/" + data[id].die_cnt + "/" + data[id].assists_cnt;
                clon.transform.FindChild("dmg").GetComponent<Text>().text = data[id].dmg.ToString();
                clon.transform.FindChild("getpiont").GetComponent<Text>().text = data[id].ach_point.ToString();
            }

        }

        public override void onClosed() {
            Transform Con = this.transform.FindChild("scroll_rect/contain");
            if (Con.childCount > 0)
            {
                for (int i = 0; i < Con.childCount; i++)
                {
                    Destroy(Con.GetChild(i).gameObject);
                }
            }
        }

    }
}
