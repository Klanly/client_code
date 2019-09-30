using System;
using GameFramework;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Cross;
using MuGame.role;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


namespace MuGame
{

    
    public class city_War : MonoBehaviour
    {

        GameObject door;
        public Material WIN_MTL_1, WIN_MTL_2;
        void Start() {
            Debug.LogError("进城战");
            WIN_MTL_1 = U3DAPI.U3DResLoad<Material>("mtl/zc2_chengmen1");
            WIN_MTL_2 = U3DAPI.U3DResLoad<Material>("mtl/zc2_chengmen2");
         
            A3_cityOfWarProxy.getInstance().addEventListener(A3_cityOfWarProxy.REFRESHFBINFO, onFbInfo);
            door = GameObject.Find("Object368");
            door.SetActive(true);
            PlayerModel.getInstance().inCityWar = true;
            InvokeRepeating("refersh_Nav", 0, 3f);
            door.GetComponent<MeshRenderer>().material = WIN_MTL_2;

            InvokeRepeating("Refsignal", 0, 1);

        }

        List<signalInfo> removeList = new List<signalInfo>();
        void Refsignal() {
            if (A3_cityOfWarModel.getInstance().signalList.Count != 0)
            {
                foreach (signalInfo info in A3_cityOfWarModel.getInstance().signalList)
                {
                    info.cd--;
                    if (info.cd <= 0)
                    {
                        removeList.Add(info);
                        if (info.signalObj != null) {
                            Destroy(info.signalObj);
                        }
                    }
                }

                foreach (signalInfo removeObj in removeList)
                {
                    if (A3_cityOfWarModel.getInstance().signalList.Contains(removeObj))
                    {
                        A3_cityOfWarModel.getInstance().signalList.Remove(removeObj);
                    }
                }
                removeList.Clear();
                if (A3_cityOfWarModel.getInstance().signalList.Count  <= 0)
                {
                    a3_insideui_fb.instance.changesignal(0);
                }
            }
        }




        void refersh_Nav() {
            if (PlayerModel .getInstance ().lvlsideid != 0) {
                if (PlayerModel.getInstance().lvlsideid == 1)
                {
                    door.GetComponent<MeshRenderer>().material = WIN_MTL_2;
                }
                else if (PlayerModel.getInstance().lvlsideid == 2)
                {
                    door.GetComponent<MeshRenderer>().material = WIN_MTL_1;

                    //if(SelfRole._inst.m_moveAgent != null)
                    //    Debug.LogError("JJ"+ NavmeshUtils.IsNavMeshLayerOpen(SelfRole._inst.m_moveAgent, "ARE2"));
                    //Debug.LogError(NavmeshUtils.listARE[1] + NavmeshUtils.listARE[2]);

                    if (SelfRole._inst != null && SelfRole._inst.m_moveAgent != null && !NavmeshUtils.IsNavMeshLayerOpen(SelfRole._inst.m_moveAgent, "ARE2")) {
                        SelfRole._inst.setNavLay(NavmeshUtils.listARE[1] + NavmeshUtils.listARE[2]);
                        SelfRole._inst.m_moveAgent.enabled = false;
                        SelfRole._inst.m_moveAgent.enabled = true;
                    }
                    else if(SelfRole._inst != null && SelfRole._inst.m_moveAgent != null && NavmeshUtils.IsNavMeshLayerOpen(SelfRole._inst.m_moveAgent, "ARE2"))
                        CancelInvoke("refersh_Nav");
                }
                
            }
        }


        void onFbInfo(GameEvent e) {
            Variant data = e.data;
            a3_insideui_fb.instance?.SetFbInfo();
            if (PlayerModel.getInstance().lvlsideid == 1)
            {
                if (A3_cityOfWarModel.getInstance().door_open)
                {
                    SelfRole._inst.setNavLay(NavmeshUtils.listARE[1] + NavmeshUtils.listARE[2]);
                    door.SetActive(false);
                }
                else
                {
                    SelfRole._inst.setNavLay(NavmeshUtils.listARE[1]);
                    door.SetActive(true);
                }
            }
            else if (PlayerModel.getInstance().lvlsideid == 2)
            {
                if (A3_cityOfWarModel.getInstance().door_open)
                {
                    door.SetActive(false);
                }
                else {
                    door.SetActive(true);
                }
            }
        }


        void OnDestroy()
        {
            A3_cityOfWarProxy.getInstance().removeEventListener(A3_cityOfWarProxy.REFRESHFBINFO, onFbInfo);
            PlayerModel.getInstance().lvlsideid = 0;
            PlayerModel.getInstance().inCityWar = false;
            A3_cityOfWarProxy.getInstance().sendProxy(1);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIBASEMAP1);
            CancelInvoke("refersh_Nav");
            CancelInvoke("Refsignal");
            A3_cityOfWarModel.getInstance().signalList.Clear();
        }

    }
}
