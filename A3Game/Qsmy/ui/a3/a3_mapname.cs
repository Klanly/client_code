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
    class a3_mapname : FloatUi
    {
        static public a3_mapname instance;
        public override void init()
        {
            instance = this;
            this.gameObject.SetActive(false);
        }

        public void refreshInfo()
        {
            this.gameObject.SetActive(true);
            Image ig = transform.FindChild("ig").GetComponent<Image>();
            ig.enabled = false;
            Variant conf = SvrMapConfig.instance.getSingleMapConf((uint)GRMap.instance.m_nCurMapID);
            if (conf.ContainsKey("map_title"))
            {
                if(conf["map_title"]==1) ig.enabled = true;
                if(conf["map_title"] == 0) ig.enabled = false;
            }          
            string file = "icon_map_pic_" + GRMap.instance.m_nCurMapID;
            ig.sprite = GAMEAPI.ABUI_LoadSprite(file);

            CancelInvoke("timeGo");
            Invoke("timeGo", 3);

        }
      
        void timeGo()
        {
            this.gameObject.SetActive(false);
        }
    }
}
