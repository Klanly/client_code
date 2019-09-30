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
    class returnbt:StoryUI
    {
        public BaseButton bt;
        public override void init()
        {
            alain();
            bt = new BaseButton(getTransformByPath("Button"));
            bt.onClick = onClick;

            this.gameObject.SetActive(false);

            CancelInvoke("showui_phone");
            Invoke("showui_phone", 0.1f);

            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.refreshAllSkills();
        }

        public void showui_phone()
        {
            this.gameObject.SetActive(true);
        }


        void onClick(GameObject go)
        {
            SceneCamera.ResetAfterLoginCam();
        }


    }
}
