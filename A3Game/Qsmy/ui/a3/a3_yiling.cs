using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;

namespace MuGame
{
    class BaseYiling : Skin
    {
        public BaseYiling(Transform trans): base(trans){}
        virtual public void onShowed(){}
        virtual public void onClose(){}
    }

    class a3_yiling : Window
    {
        private TabControl tab;
        private Transform con;
        private BaseYiling current = null;
        private BaseYiling pet = null;
        private BaseYiling wing = null;
		public static a3_yiling instance; 

        public override void init()
        {
            con = getTransformByPath("con");

            tab = new TabControl();
            tab.onClickHanle = OnSwitch;
            tab.create(getGameObjectByPath("tab"), this.gameObject);

            BaseButton closeBtn = new BaseButton(getTransformByPath("close"));
            closeBtn.onClick = OnClose;
			CheckLock();
			instance = this;
        }

        public override void onShowed()
        {
            if (current != null)
            {
                current.onShowed();
            }
            else
            {
                tab.setSelectedIndex(0);
                OnSwitch(tab);
            }
            GRMap.GAME_CAMERA.SetActive(false);
        }

        public override void onClosed()
        {
            if (current != null)
            {
                current.onClose();
            }
            GRMap.GAME_CAMERA.SetActive(true);
        }

        private void OnSwitch(TabControl t)
        {
            int index = t.getSeletedIndex();
            if (current != null)
            {
                current.onClose();
                current.gameObject.SetActive(false);
            }

            if (index == 0)
            {
                if (A3_PetModel.getInstance().hasPet())
                {
                    if (pet == null)
                    {
                        GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_pet_skin");
                        GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                        //pet = new a3_pet_skin(panel.transform);
                        //pet.setPerent(con);
						//pet.gameObject.SetActive(false);
                    }
					if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET)) current = pet;
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("no_get_pet"));
                    current = null;
                }
            }
            else if (index == 1)
            {
                if (wing == null)
                {//TODO
                    GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_wing_skin");
                    GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                    //wing = new a3_wing_skin(panel.transform);
                   // wing.setPerent(con);
					//wing.gameObject.SetActive(false);
                }
				if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING)) current = wing;
            }

            if (current != null)
            {
                current.onShowed();
                current.visiable = true;
				if (!current.__mainTrans.gameObject.activeSelf) current.__mainTrans.gameObject.SetActive(true);
            }
        }

        void OnClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_YILING);
        }

		public void CheckLock() {
			transform.FindChild("tab/pet").gameObject.SetActive(false);
			transform.FindChild("tab/wing").gameObject.SetActive(false);
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET)) {
				//OpenPet();
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING)) {
				OpenWing();
			}
		}
		public void OpenPet() {
			var dt = transform.FindChild("tab/pet");
			if (dt) {
				dt.gameObject.SetActive(true);
			}
			A3_PetProxy.getInstance().GetPets();

		}
		public void OpenWing() {
			var dt = transform.FindChild("tab/wing");
			if (dt) {
				dt.gameObject.SetActive(true);
			}
			A3_WingProxy.getInstance().GetWings();
		}
    }
}
