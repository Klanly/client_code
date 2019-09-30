using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_autoplay_eqp : Window
    {
        private Toggle[] eqpToggles;
        private Toggle sellToggle;
        private Toggle recyleToggle;
        private AutoPlayModel apModel;

        public override void init()
        {
            apModel = AutoPlayModel.getInstance();
            inText();
            this.getEventTrigerByPath("ig_bg_bg").onClick = OnClose;
            
            eqpToggles = new Toggle[6];
            for (int i = 0; i < 6; i++)
            {
                eqpToggles[i] = getComponentByPath<Toggle>("eqp" + i);
            }

            sellToggle = getComponentByPath<Toggle>("sell");
            sellToggle.onValueChanged.AddListener(OnSellChange);

            recyleToggle = getComponentByPath<Toggle>("recyle");
            recyleToggle.onValueChanged.AddListener(OnRecyleChange);

            BaseButton okBtn = new BaseButton(getTransformByPath("ok"));
            okBtn.onClick = OnOK;
        }


        void inText()
        {
            this.transform.FindChild("title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_1");
            this.transform.FindChild("t1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_2");
            this.transform.FindChild("t2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_3");
            this.transform.FindChild("eqp0/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_4");
            this.transform.FindChild("eqp1/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_5");
            this.transform.FindChild("eqp2/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_6");
            this.transform.FindChild("eqp3/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_7");
            this.transform.FindChild("eqp4/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_8");
            this.transform.FindChild("eqp5/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_12");
            this.transform.FindChild("sell/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_9");
            this.transform.FindChild("recyle/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_10");
            this.transform.FindChild("ok/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_eqp_11");
        }
        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
            int eqpproc = apModel.EqpProc;
            for (int i = 0; i < 6; i++)
            {
                if ((eqpproc & (1 << i)) == 0)
                {
                    eqpToggles[i].isOn = false;
                }
                else
                {
                    eqpToggles[i].isOn = true;
                }
            }

            sellToggle.isOn = false;
            recyleToggle.isOn = false;
            switch (apModel.EqpType)
            {
                case 0:
                    break;
                case 1:
                    sellToggle.isOn = true;
                    break;
                case 2:
                    recyleToggle.isOn = true;
                    break;
            }

        }

        void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY_EQP);
        }

        private void OnSellChange(bool v)
        {
            if (v)
            {
                recyleToggle.isOn = false;
            }
        }

        private void OnRecyleChange(bool v)
        {
            if (v)
            {
                sellToggle.isOn = false;
            }
        }

        private void OnOK(GameObject go)
        {
            apModel.EqpProc = 0;
            for (int i = 0; i < 6; i++)
            {
                if (eqpToggles[i].isOn)
                {
                    apModel.EqpProc += (1 << i);
                }
            }
            if (sellToggle.isOn)
            {
                apModel.EqpType = 1;
            }
            else if (recyleToggle.isOn)
            {
                apModel.EqpType = 2;
            }
            else
            {
                apModel.EqpType = 0;
            }

            OnClose(null);
        }
    }
}
