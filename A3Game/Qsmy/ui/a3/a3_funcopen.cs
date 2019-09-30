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
    class a3_funcopen : FloatUi
    {
        static public a3_funcopen instance;
        Image ig_icon,ig_icon1;
        public bool is_show = false;
        public override void init()
        {
            instance = this;
            this.gameObject.SetActive(false);
        }

        public void refreshInfo(int id, float x, float y)
        {
            this.gameObject.SetActive(true);
           
            ig_icon = transform.FindChild("mover/icon").GetComponent<Image>();
            ig_icon1 = transform.FindChild("icon").GetComponent<Image>();
            string file = "icon_func_open_" + id;
            ig_icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            ig_icon1.sprite = GAMEAPI.ABUI_LoadSprite(file);
            ig_icon.gameObject.SetActive(true);

            CancelInvoke("timeGo");
            Invoke("timeGo", 3.3f);

            runAni(x, y);

            if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
            InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
            if (SelfRole.fsm.Autofighting)
            { 
                if (StateInit.Instance.IsOutOfAutoPlayRange())
                {
                    SelfRole.fsm.Stop();
                }
                else
                {
                    SelfRole.fsm.Resume();
                }
            }
            else
                SelfRole.fsm.Stop();

            a3_task_auto.instance.stopAuto = true;
            is_show = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.DIALOG);
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
            is_show = false;
        }
    }
}
