using System;
using System.Collections.Generic;
using System.Linq;
//using System.Management.Instrumentation;
using System.Text;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_autoplay_pick : Window
    {
        private Toggle[] eqpToggles;
        private Toggle[] matToggles;
		Toggle equip_Toggles;
		Toggle pet_Toggles;
		Toggle wing_Toggles;
		Toggle summon_Toggles;
		Toggle drugs_Toggles;
		Toggle golds_Toggles;
		Toggle other_Toggles;


        private AutoPlayModel apModel;

        public override void init()
        {
            apModel = AutoPlayModel.getInstance();
            inText();
            this.getEventTrigerByPath("ig_bg_bg").onClick = OnClose;
            BaseButton okBtn = new BaseButton(getTransformByPath("ok"));
            okBtn.onClick = OnOK;

            eqpToggles = new Toggle[6];
            for (int i = 0; i < 6; i++)
            {
                eqpToggles[i] = getComponentByPath<Toggle>("eqp" + i);
            }

            matToggles = new Toggle[5];
            for (int i = 0; i < 5; i++)
            {
                matToggles[i] = getComponentByPath<Toggle>("mat" + i);
            }

			equip_Toggles = getComponentByPath<Toggle>("eqp_cailiao");
			pet_Toggles = getComponentByPath<Toggle>("pet_cailiao");
			wing_Toggles = getComponentByPath<Toggle>("wing_cailiao");
			summon_Toggles = getComponentByPath<Toggle>("summon_cailiao");
			drugs_Toggles = getComponentByPath<Toggle>("drugs");
			golds_Toggles = getComponentByPath<Toggle>("golds");
			other_Toggles = getComponentByPath<Toggle>("other");
        }


        void inText() {
            this.transform.FindChild("title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_1");
            this.transform.FindChild("t2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_2");
            this.transform.FindChild("eqp0/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_3");
            this.transform.FindChild("eqp1/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_4");
            this.transform.FindChild("eqp2/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_5");
            this.transform.FindChild("eqp3/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_6");
            this.transform.FindChild("eqp4/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_7");
            this.transform.FindChild("eqp5/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_14");
            this.transform.FindChild("eqp_cailiao/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_8");
            this.transform.FindChild("wing_cailiao/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_9");
            this.transform.FindChild("summon_cailiao/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_10");
            this.transform.FindChild("golds/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_11");
            this.transform.FindChild("other/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_12");
            this.transform.FindChild("ok/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_autoplay_pick_13");

        }
        public override void onShowed()
        {
            this.transform.SetAsLastSibling();
            int pickeqp = apModel.PickEqp;
            int pickmat = apModel.PickMat;
            for (int i = 0; i < 6; i++)
            {
                if ((pickeqp & (1 << i)) == 0)
                {
                    eqpToggles[i].isOn = false;
                }
                else
                {
                    eqpToggles[i].isOn = true;
                }
 
            }


            for (int i = 0; i < 5; i++)
            {
                if ((pickmat & (1 << i)) == 0)
                {
                    matToggles[i].isOn = false;
                }
                else
                {
                    matToggles[i].isOn = true;
                }
            }

			equip_Toggles.isOn = apModel.PickEqp_cailiao == 1;
			pet_Toggles.isOn = apModel.PickPet_cailiao == 1;
			wing_Toggles.isOn = apModel.PickWing_cailiao == 1;
			summon_Toggles.isOn = apModel.PickSummon_cailiao == 1;
			drugs_Toggles.isOn = apModel.PickDrugs == 1;
			golds_Toggles.isOn = apModel.PickGold == 1;
			other_Toggles.isOn = apModel.PickOther == 1;
        }

        void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY_PICK);
        }

        private void OnOK(GameObject go)
        {
            apModel.PickEqp = 0;
            apModel.PickMat = 0;
            for (int i = 0; i < 6; i++)
            {
                if (eqpToggles[i].isOn)
                {
                    apModel.PickEqp += (1 << i);
                }



			}

            for (int i = 0; i < 5; i++) {
                if (matToggles[i].isOn)
                {
                    apModel.PickMat += (1 << i);
                }

            }


			if (equip_Toggles.isOn) apModel.PickEqp_cailiao = 1;
			else apModel.PickEqp_cailiao = 0;

			if (pet_Toggles.isOn) apModel.PickPet_cailiao = 1;
			else apModel.PickPet_cailiao = 0;

			if (wing_Toggles.isOn) apModel.PickWing_cailiao = 1;
			else apModel.PickWing_cailiao = 0;

			if (summon_Toggles.isOn) apModel.PickSummon_cailiao = 1;
			else apModel.PickSummon_cailiao = 0;

			if (drugs_Toggles.isOn) apModel.PickDrugs = 1;
			else apModel.PickDrugs = 0;

			if (golds_Toggles.isOn) apModel.PickGold = 1;
			else apModel.PickGold = 0;

			if (other_Toggles.isOn) apModel.PickOther = 1;
			else apModel.PickOther = 0;

            OnClose(null);
        }
    }
}
