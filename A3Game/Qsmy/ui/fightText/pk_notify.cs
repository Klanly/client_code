using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
namespace MuGame
{
    class pk_notify:FloatUi
    {
        private Text txt;

        public static pk_notify instance;


        //public static void show()
        //{
        //    if (instance != null)
        //        instance.refesh();
        //    else
        //        InterfaceMgr.getInstance().open(InterfaceMgr.PK_NOTIFY);
        //}

        public override void init()
        {
            txt =getComponentByPath<Text>("txt");
            this.gameObject.SetActive(false);
        }

        public override void onShowed()
        {
            instance = this;
            refesh();
        }

        public override void onClosed()
        {
            instance = null;
            if(sq!=null)
            sq.Kill();
            sq = null;
        }
        Sequence sq;
        public void refesh()
        {
            //jason   不能使用这种txt.material 这里一改，所有的字的material都看不到了！
            return; 

            txt.text = PlayerModel.getInstance().inDefendArea ?  ContMgr.getCont("pk_notify_out") : ContMgr.getCont("pk_notify_in");

            if (sq != null)
                sq.Kill();

                 Color color = txt.material.GetColor("_Color");
            color.a = 1f;
            txt.material.SetColor("_Color", color);
            sq = DOTween.Sequence()
                .Append(txt.material.DOFade(0, 1f).From())
         .AppendInterval(5f).Append(txt.material.DOFade(0, 1f))
        .AppendCallback(() => { InterfaceMgr.getInstance().close(InterfaceMgr.PK_NOTIFY); });
        }
    }
}
