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
    class a3_pkmodel:Window
    {
        GameObject[] objs = new GameObject[5];


        public static a3_pkmodel _instance;
        ScrollControler scrollControer;
        public override void init()
        {

            getComponentByPath<Text>("scrollRect/contain/grid0/bg/Text1").text = ContMgr.getCont("a3_pkmodel_0");
            getComponentByPath<Text>("scrollRect/contain/grid1/bg/Text1").text = ContMgr.getCont("a3_pkmodel_1");
            getComponentByPath<Text>("scrollRect/contain/grid2/bg/Text1").text = ContMgr.getCont("a3_pkmodel_2");


            scrollControer = new ScrollControler();
            ScrollRect scroll = transform.FindChild("scrollRect").GetComponent<ScrollRect>();
            scrollControer.create(scroll);

            for (int i = 0; i < 3; i++)
            {
                objs[i] = transform.FindChild("scrollRect/contain/grid" + i).gameObject;
                BaseButton btns = new BaseButton(objs[i].transform.FindChild("bg/"+i));
                btns.onClick = onBtnClicks;
            }

            BaseButton btn = new BaseButton(transform.FindChild("closeBtn"));
            btn.onClick = close;
        }
        public override void onShowed()
        {
            _instance = this;
            ShowThisImage(PlayerModel.getInstance().now_pkState);
        }

        public override void onClosed()
        {
            base.onClosed();
        }
        void onBtnClicks(GameObject go)
        {
            //for (int i = 0; i < objs.Length; i++)
            //{
            //    objs[i].transform.FindChild("bg/"+i).gameObject.SetActive(true);
              
            //}
            //go.SetActive(false);
            //go.transform.parent.transform.FindChild("now_txt").gameObject.SetActive(true);
            a3_PkmodelProxy.getInstance().sendProxy(int.Parse(go.name));
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMODEL);
            NewbieModel.getInstance().hide();

        }

        public  void ShowThisImage(int state)
        {
            for (int i = 0; i < 3; i++)
            {
                if (state == i)
                    objs[i].transform.FindChild("bg/this").gameObject.SetActive(true);
                else
                    objs[i].transform.FindChild("bg/this").gameObject.SetActive(false);
            }
        }

        void close(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMODEL);
        }

    }
}
