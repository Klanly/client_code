using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace MuGame
{
    class beginloading : LoadingUI
    {

        private Text text;
        static public beginloading instance;
        //void Start()
        //{
        //    instance = this;
        //    text = this.getTransformByPath("Text").GetComponent<Text>();


        //}

        public override void init()
        {
            instance = this;
            text = this.getTransformByPath("Text").GetComponent<Text>();

           // setText(uiData[0] as string);

            if (cemaraRectTran == null)
                cemaraRectTran = GameObject.Find("canvas_main").GetComponent<RectTransform>();

            RectTransform cv = cemaraRectTran;

            RectTransform bg = this.getTransformByPath("bg").GetComponent<RectTransform>();
            bg.sizeDelta = new Vector2(cv.rect.width, cv.rect.height);

            getComponentByPath<Text>("Text").text = ContMgr.getCont("beginloading_0");
        }
       

        public void setText(string str)
        {
          //  text.text = str;
        }

        public override void dispose()
        {
            instance = null;
            base.dispose();
        }
    }
}
