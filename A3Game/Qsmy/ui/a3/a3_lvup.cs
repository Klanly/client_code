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
    class a3_lvup : FloatUi
    {
        static public a3_lvup instance;
        public override void init()
        {
            instance = this;
            this.gameObject.SetActive(false);
        }

      
        public void refreshInfo(uint lv)
        {
            this.gameObject.SetActive(true);
            transform.FindChild("lv").GetComponent<Text>().text = lv.ToString();
            CancelInvoke("timeGo");
            Invoke("timeGo", 3);
        }

        void timeGo()
        {
            this.gameObject.SetActive(false);
        }
    }
}
