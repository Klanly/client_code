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
using UnityEngine.EventSystems;

namespace MuGame
{
    class a3_liteMiniBaseMap1 : Window
    {
        public static a3_liteMiniBaseMap1 instance;

        public GameObject goMapcon;
        public GameObject goP;

        public int mapid;
        MiniMapItem curMiniMap;
    
        public Quaternion mapRotation; //地图旋转角度（由于场景摄像机调试角度引起的转向）

        Transform TeammateCon;
        Transform enemyCon;

        public override void init()
        {
            alain();

            this.transform.SetAsFirstSibling();

            goMapcon = getGameObjectByPath("panel_map");
            goP = getGameObjectByPath("panel_map/p");


            TeammateCon = this.transform.FindChild("panel_map/icon_Teammate");
            enemyCon = this.transform.FindChild("panel_map/icon_enemy");
            BaseButton btn_close = new BaseButton(transform.FindChild("close"));
            btn_close.onClick = onCloseMap;

            mapRotation = Quaternion.Euler(0f, 0f, 0f);
      
        }

        public override void onShowed()
        {
            instance = this;

            InvokeRepeating("refreshPos_otherP", 0, 1f);
            if (curMiniMap == null || curMiniMap.id != GRMap.instance.m_nCurMapID)
            {
                init_open_map();
            }
            clear_pos();
        }
        public override void onClosed()
        {
            instance = null;
            CancelInvoke("refreshPos_otherP");
            clear_pos();
        }
        public void Update()
        {
            if (GRMap.grmap_loading)
                return;

            refreshPos();
            refreshFlag();
           // refreshPos_otherP();
        }


        GameObject map_obj;

        void init_open_map()
        {
            mapid = GRMap.instance.m_nCurMapID;
            GameObject temp = GAMEAPI.ABUI_LoadPrefab("map_prefab_map" + mapid);
            if (temp == null)
                return;

            SXML xml = XMLMgr.instance.GetSXML("mappoint.trans_remind", "map_id==" + mapid);
            if (xml.getInt("rotation") != -1)
                mapRotation = mapRotation = Quaternion.Euler(0f, 0f, xml.getInt("rotation"));
            else
                mapRotation = Quaternion.Euler(0f, 0f, 0);

            GameObject go = GameObject.Instantiate(temp) as GameObject;
            go.transform.FindChild("map").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("map_map" + mapid);
            go.transform.SetParent(goMapcon.transform, false);
            curMiniMap = new MiniMapItem(go.transform, mapRotation);
            curMiniMap.id = mapid;
            Transform goNone = curMiniMap.__mainTrans.Find("none");
            if (goNone != null)
            {
                goNone.gameObject.SetActive(false);
            }
            map_obj = go;
            go.transform.SetSiblingIndex(1);
        }
 


        Dictionary<string, GameObject> dMonInMinimap = new Dictionary<string, GameObject>();
        void refreshFlag() {

            if (map_obj == null)  return;

            List<flag_info> flag_list = jdzc_zhandian.flag_list.Values.ToList<flag_info>();
            foreach (flag_info flag in flag_list)
            {
                GameObject go;
                if (dMonInMinimap.ContainsKey("falg" + flag.flag_idx))
                {
                    go = dMonInMinimap["falg" + flag.flag_idx];
                }
                else
                {
                    go = map_obj.transform.FindChild("strongpoint/" + flag.flag_idx).gameObject;
                    dMonInMinimap["falg" + flag.flag_idx] = go;
                    //go.transform.SetParent(transItemCon);
                }
                if (flag.owner_side == 0)
                {
                    go.transform.FindChild("gray").gameObject.SetActive(true);
                    go.transform.FindChild("blue").gameObject.SetActive(false);
                    go.transform.FindChild("red").gameObject.SetActive(false);
                }
                else if (flag.owner_side == 2)
                {
                    go.transform.FindChild("gray").gameObject.SetActive(false);
                    go.transform.FindChild("blue").gameObject.SetActive(true);
                    go.transform.FindChild("red").gameObject.SetActive(false);
                }
                else if (flag.owner_side == 1)
                {
                    go.transform.FindChild("gray").gameObject.SetActive(false);
                    go.transform.FindChild("blue").gameObject.SetActive(false);
                    go.transform.FindChild("red").gameObject.SetActive(true);
                }
            }
        }

        public void refreshPos()
        {
            goP.SetActive(mapid == GRMap.instance.m_nCurMapID || mapid == 0);
            if (curMiniMap == null || goP.active == false)
                return;
            Vector3 vec = SceneCamera.getPosOnMiniMap(curMiniMap.mapScale);
            vec = mapRotation * vec;

            goP.transform.localPosition = vec;

            vec = SelfRole._inst.m_curModel.eulerAngles;
            vec.y = -vec.y;
            goP.transform.localEulerAngles = new Vector3(0f, 0f, 180f - SelfRole._inst.m_curModel.eulerAngles.y + mapRotation.eulerAngles.z);

        }

        void clear_pos() {
            for (int i = 0;i< TeammateCon.childCount;i++) {
                TeammateCon.GetChild(i).gameObject.SetActive(false) ;
            }
            for (int i = 0; i < enemyCon.childCount; i++)
            {
                enemyCon.GetChild(i).gameObject.SetActive(false);
            }
            posObj.Clear();
        }

        Dictionary<int, GameObject> posObj = new Dictionary<int, GameObject>();

        void refreshPos_otherP() {
            a3_sportsProxy.getInstance().getTeam_pos();
            if (a3_sportsProxy .getInstance ().list_position != null)
            {
                    for (int i = 0; i < a3_sportsProxy.getInstance().list_position.Count; i++)
                    {
                        if (a3_sportsProxy.getInstance().list_position[i].lvlsideid == 2)
                        {
                            if (posObj.ContainsKey(a3_sportsProxy.getInstance().list_position[i].iid))
                            {
                                posObj[a3_sportsProxy.getInstance().list_position[i].iid].SetActive(true);

                                int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
                                int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
                                Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                                vec = mapRotation * vec;        
                                posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
                            }
                            else
                            {
                                for (int m = 0; m < TeammateCon.childCount; m++)
                                {
                                    if (TeammateCon.GetChild(m).gameObject.activeSelf == false)
                                    {
                                        TeammateCon.GetChild(m).gameObject.SetActive(true);
                                        posObj[a3_sportsProxy.getInstance().list_position[i].iid] = TeammateCon.GetChild(m).gameObject;
                                        int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
                                        int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
                                        Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                                        vec = mapRotation * vec;
                                        posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (posObj.ContainsKey(a3_sportsProxy.getInstance().list_position[i].iid))
                            {
                                posObj[a3_sportsProxy.getInstance().list_position[i].iid].SetActive(true);

                                int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
                                int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
                                Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                                vec = mapRotation * vec;
                                posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
                            }
                            else
                            {
                                for (int m = 0; m < enemyCon.childCount; m++)
                                {
                                    if (enemyCon.GetChild(m).gameObject.activeSelf == false)
                                    {
                                        enemyCon.GetChild(m).gameObject.SetActive(true);
                                        posObj[a3_sportsProxy.getInstance().list_position[i].iid] = enemyCon.GetChild(m).gameObject;
                                        int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
                                        int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
                                        Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                                        vec = mapRotation * vec;
                                        posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
                                        break;
                                    }
                                }
                            }

                        }
                    }
            }
        }


        void onCloseMap(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIBASEMAP1);
        }

    }
}
