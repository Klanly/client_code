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
    class a3_expbar : FloatUi
    {
        public override void init()
        {
            alain();
            BaseButton btn_1 = new BaseButton(transform.FindChild("operator/btn_1"));
            btn_1.onClick = onBtn;

            BaseButton btn_2 = new BaseButton(transform.FindChild("operator/btn_2"));
            btn_2.onClick = onBtn;

            BaseButton btn_3 = new BaseButton(transform.FindChild("operator/btn_3"));
            btn_3.onClick = onBtn;

            BaseButton btn_chat = new BaseButton(transform.FindChild("operator/chat/Button"));
            btn_chat.onClick = onBtn;

            BaseButton btn_up = new BaseButton(transform.FindChild("operator/btn_up"));
            btn_up.onClick = onBtn;

            BaseButton btn_down = new BaseButton(transform.FindChild("operator/btn_down"));
            btn_down.onClick = onBtn;
        }

        void onBtn(GameObject go)
        {

        }
        
    }
}
