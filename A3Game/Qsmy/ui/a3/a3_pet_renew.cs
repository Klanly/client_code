using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

namespace MuGame
{
    class a3_pet_renew : Window
    {
        public static a3_pet_renew instance;
        private GameObject m_SelfObj;
        private GameObject scene_Camera;
        GameObject bird = null;
        public override void init()
        {

            getComponentByPath<Text>("bg/desc").text = ContMgr.getCont("a3_pet_renew_0");
            getComponentByPath<Text>("gorenew/Text").text = ContMgr.getCont("a3_pet_renew_1");

            instance = this;
            new BaseButton(getTransformByPath("gorenew")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_PET_RENEW);
                  InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEW_PET);
                  A3_PetModel.showbuy = true;
              };
            new BaseButton(getTransformByPath("bg/dark")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_PET_RENEW);
            };
            transform.SetAsLastSibling();
        }
        public override void onShowed()
        {
            creatAvatar();
        }
        public override void onClosed()
        {
            disposeAvatar();
        }
        void creatAvatar()
        {
            GameObject birdPrefab;
            string str="";
            str = XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + A3_PetModel.curPetid).getString("mod");

            //switch (A3_PetModel.curPetid)
            //{
            //    case 2: str = "eagle"; break;
            //    case 5: str = "yingwu"; break;
            //    case 3: str = "yaque"; break;
            //    default:
            //        break;
            //}
            birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_"+str);

            m_SelfObj = GameObject.Instantiate(birdPrefab, new Vector3(-153.92f, 0.89f, 0f), new Quaternion(0, 180, 0, 0)) as GameObject;
            bird = m_SelfObj;
            bird.GetComponent<Animator>().applyRootMotion = false;
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            if (birdPrefab == null)
                return;
            bird.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            if (m_SelfObj != null)
            {
                GameObject obj_prefab;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            }
            DontDestroyOnLoad(m_SelfObj);
            DontDestroyOnLoad(scene_Camera);
        }
        public void disposeAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
    }
}
