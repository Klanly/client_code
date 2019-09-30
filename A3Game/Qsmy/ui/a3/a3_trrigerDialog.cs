using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
namespace MuGame
{
    class a3_trrigerDialog : FloatUi
    {
        static public a3_trrigerDialog instance;

        Text txt_dialog;
        Image ig_icon;
        GameObject go_dialog;
        GameObject Notice;
        public override void init()
        {
            instance = this;

            go_dialog = transform.FindChild("dialog").gameObject;
            Notice = this.transform.FindChild("Notice").gameObject;
            Notice.SetActive(false);
            go_dialog.SetActive(false);
            txt_dialog = transform.FindChild("dialog/txt").GetComponent<Text>();
            ig_icon = transform.FindChild("dialog/icon").GetComponent<Image>();

            transform.FindChild("Notice/txt").GetComponent<Text>().text = ContMgr.getCont("a3_trrigerDialog_Notice");
        }

        public void CheckDialog(int trrigerid)
        {
            uint mapid = MapModel.getInstance().curLevelId;
            int difflvl = (int)MapModel.getInstance().curDiff;
            Variant d = SvrLevelConfig.instacne.get_level_data(mapid);
            Variant t = d["diff_lvl"][difflvl]["map"][0];
            foreach (Variant a in t["trigger"]._arr)
            {
                int id = a["id"];
                if (id == trrigerid)
                {
                    if (a.ContainsKey("dialog"))
                    {
                        doDialog(a["dialog"][0]["icon"], a["dialog"][0]["des"], a["dialog"][0]["last"]);
                    }
                    break;
                } 
            }
        }

        public void ShowNotice()
        {
            uint mapid = MapModel.getInstance().curLevelId;
            Variant d = SvrLevelConfig.instacne.get_level_data(mapid);
            if (!d.ContainsKey("notice")) return;
            if (d["notice"] > 0) {
                doNotice(d["notice"]);
            }
        }


        void doNotice(float time)
        {
            if (Notice.activeSelf) { return; }
            Notice.SetActive(true);
            CancelInvoke("onshowend_Notice");
            Invoke("onshowend_Notice", time);

        }

        void onshowend_Notice() {
            Notice.SetActive(false);
        }


        public void doDialog(int icon, string txt, float time)
        {
            
            go_dialog.SetActive(true);

            string file = "icon_bosshead_" + icon;
            ig_icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            txt_dialog.text = txt;

            CancelInvoke("onshowend");
            Invoke("onshowend", time);
        }

        void onshowend()
        {
            go_dialog.SetActive(false);
        }
    }
}
