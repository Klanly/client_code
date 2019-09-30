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
    class a3_runeopen : FloatUi
    {
        static public a3_runeopen instance;
        Image ig_icon, ig_icon1;
        public int open_id = -1;
        public float x, y;
        public override void init()
        {
            instance = this;
            this.gameObject.SetActive(false);
            x = -76f;
            y = -255f;
        }

        public void refreshInfo()
        {
            if (open_id < 0)
                return;

            if (!GameRoomMgr.getInstance().checkCityRoom())
                return;

            this.gameObject.SetActive(true);

            ig_icon = transform.FindChild("mover/icon").GetComponent<Image>();
            ig_icon1 = transform.FindChild("icon").GetComponent<Image>();
            string file = "icon_rune_" + open_id;
            ig_icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            ig_icon1.sprite = GAMEAPI.ABUI_LoadSprite(file);
            ig_icon.gameObject.SetActive(true);

            CancelInvoke("timeGo");
            Invoke("timeGo", 3.3f);

            runAni(x, y);

            if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Up();

            open_id = -1;
        }
        void runAni(float x, float y)
        {
            ig_icon1.gameObject.SetActive(false);

            GameObject txtclone = ((GameObject)GameObject.Instantiate(ig_icon1.gameObject));
            txtclone.gameObject.SetActive(true);
            txtclone.transform.SetParent(transform, false);
            txtclone.transform.SetAsFirstSibling();

            Tweener tween1 = txtclone.transform.DOLocalMove(new Vector3(x, y, 0), 0.5f).SetDelay(2.8f);
            tween1.SetEase(Ease.InOutCirc);
            tween1.OnComplete(delegate ()
            {
                Destroy(txtclone);
            });
        }
        void timeGo()
        {
            this.gameObject.SetActive(false);

            //触发新手指引
            UiEventCenter.getInstance().onWinClosed(uiName);
            a3_task_auto.instance.stopAuto = false;
            a3_liteMinimap.instance.ZidongTask = false;
        }
    }
}
