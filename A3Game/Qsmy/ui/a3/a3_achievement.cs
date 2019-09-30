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
    class a3_achievement : Window
    {
        AchiveSkin honur;
        AchiveSkin rank;
        AchiveSkin currentAchieve = null;
        TabControl tc;
        public static a3_achievement instance;
        public override void init()
        {
           // honur = new a3_honor(this, transform.FindChild("contents/a3_honor"));
           // rank = new a3_rank(this, transform.FindChild("contents/a3_rank"));

            tc = new TabControl();
            instance = this;
            tc.onClickHanle = (TabControl t) =>
            {
                int i = t.getSeletedIndex();
                int n = 0;
                Transform content = transform.FindChild("contents");
                foreach (var v in content.GetComponentsInChildren<Transform>(true))
                {
                    if (v.parent == content)
                    {
                        if (n == i)
                        {
                            v.gameObject.SetActive(true);
                        }
                        else v.gameObject.SetActive(false);
                        n++;
                    }
                }
                if (currentAchieve != null) currentAchieve.onClosed();
                switch (i)
                {
                    case 0:
                        currentAchieve = honur;
                        break;
                    case 1:
                        currentAchieve = rank;
                        break;
                }
                if (currentAchieve != null) currentAchieve.onShowed();
            };
            tc.create(transform.FindChild("tabs").gameObject, this.gameObject);
        }

        public override void onShowed()
        {
            if (uiData == null)
                tc.setSelectedIndex(0, true);
            else
            {
                int index = (int)uiData[0];
                tc.setSelectedIndex(index, true);
            }
            GRMap.GAME_CAMERA.SetActive(false);
            UiEventCenter.getInstance().onWinOpen(uiName);
            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE);//获取玩家成就信息
        }

        public override void onClosed()
        {
            if (currentAchieve != null) currentAchieve.onClosed();
            GRMap.GAME_CAMERA.SetActive(true);
        }
    }

    class AchiveSkin : Skin
    {
        public a3_achievement main { get; set; }
        public AchiveSkin(Window win, Transform tran)
            : base(tran)
        {
            main = win as a3_achievement;
            init();
        }
        public virtual void init() { }
        public virtual void onShowed() { }
        public virtual void onClosed() { }
    }
}
