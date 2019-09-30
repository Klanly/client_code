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
    class a3_liteMiniBaseMap2 : Window
    {
        public static a3_liteMiniBaseMap2 instance;

        public GameObject goMapcon;
        public GameObject goP;

        public int mapid;
        MiniMapItem curMiniMap;

        public Quaternion mapRotation; //地图旋转角度（由于场景摄像机调试角度引起的转向）

        Transform TeammateCon;
        Transform enemyCon;
        GameObject TeamObj;
        GameObject EnemyObj;
        Transform btns;

        public override void init()
        {
            alain();
            inText();
            this.transform.SetAsFirstSibling();

            goMapcon = getGameObjectByPath("panel_map");
            goP = getGameObjectByPath("panel_map/p");


            TeammateCon = this.transform.FindChild("panel_map/icon_Teammate");
            enemyCon = this.transform.FindChild("panel_map/icon_enemy");
            TeamObj = this.transform.FindChild("panel_map/signal1").gameObject;
            EnemyObj = this.transform.FindChild("panel_map/signal0").gameObject;
            BaseButton btn_close = new BaseButton(transform.FindChild("close"));
            btn_close.onClick = onCloseMap;
            btns = this.transform.FindChild("panel_map/btns");

            for (int i =0; i < btns.childCount; i++ ) {
                new BaseButton(btns.GetChild(i)).onClick = (GameObject go) => {
                    Vector3 vec = SceneCamera.getPosOnMiniMap(curMiniMap.mapScale);
                    vec = mapRotation * vec;
                    int x = (int)vec.x;
                    int y = (int)vec.y;

                    A3_cityOfWarProxy.getInstance().send_signal(uint.Parse (go.name), x, y);
                };
            }
            mapRotation = Quaternion.Euler(0f, 0f, 0f);

        }

        void inText()
        {
            for (int i = 0; i < this.transform .FindChild ("panel_map/btns").childCount;i++) {
                this.transform.FindChild("panel_map/btns").GetChild (i).FindChild ("Text").GetComponent <Text>().text = ContMgr.getCont("uilayer_a3_liteMiniBaseMap2_"+ i);
            }
        }


        public override void onShowed()
        {
            instance = this;
            A3_cityOfWarProxy.getInstance().sendPos_info();
  
            if (curMiniMap == null || curMiniMap.id != GRMap.instance.m_nCurMapID)
            {
                init_open_map();
            }
            clear_pos();
            InvokeRepeating("refreshPos_otherP", 0, 3f);
            SetSignal();
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
        }


        public void SetSignal() {
            foreach (signalInfo info in A3_cityOfWarModel.getInstance().signalList)
            {
                if (info!= null && info.cd >=1) {
                    if (info.signalObj == null) {
                        GameObject obj = Instantiate(this.transform.FindChild("panel_map/signal/" + info.signalType).gameObject) as GameObject;
                        obj.SetActive(true);
                        int x2 = (int)info.x;
                        int y2 = (int)info.y;
                        Vector3 vec = new Vector3(x2, y2,0);  // SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                      //  vec = mapRotation * vec;
                        debug.Log("oooooo" + vec);
                        obj.transform.localPosition = vec;
                        info.signalObj = obj;
                       // obj.transform.SetParent(TeammateCon, false);
                        obj.transform.SetParent(this.transform .FindChild ("panel_map/signalCon"),false);
                    }
                }
            }
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

        void clear_pos()
        {
            for (int i = 0; i < TeammateCon.childCount; i++)
            {
                Destroy(TeammateCon.GetChild(i).gameObject);
            }
            for (int i = 0; i < enemyCon.childCount; i++)
            {
                Destroy(enemyCon.GetChild(i).gameObject);
            }
            posObj_team.Clear();
            posObj_enemy.Clear();
        }

        Dictionary<int, GameObject> posObj_team = new Dictionary<int, GameObject>();
        Dictionary<int, GameObject> posObj_enemy = new Dictionary<int, GameObject>();

        //void refreshPos_otherP()
        //{
        //    a3_sportsProxy.getInstance().getTeam_pos();
        //    if (a3_sportsProxy.getInstance().list_position != null)
        //    {
        //        for (int i = 0; i < a3_sportsProxy.getInstance().list_position.Count; i++)
        //        {
        //            if (a3_sportsProxy.getInstance().list_position[i].lvlsideid == 2)
        //            {
        //                if (posObj.ContainsKey(a3_sportsProxy.getInstance().list_position[i].iid))
        //                {
        //                    posObj[a3_sportsProxy.getInstance().list_position[i].iid].SetActive(true);

        //                    int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
        //                    int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
        //                    Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
        //                    vec = mapRotation * vec;
        //                    posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
        //                }
        //                else
        //                {
        //                    for (int m = 0; m < TeammateCon.childCount; m++)
        //                    {
        //                        if (TeammateCon.GetChild(m).gameObject.activeSelf == false)
        //                        {
        //                            TeammateCon.GetChild(m).gameObject.SetActive(true);
        //                            posObj[a3_sportsProxy.getInstance().list_position[i].iid] = TeammateCon.GetChild(m).gameObject;
        //                            int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
        //                            int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
        //                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
        //                            vec = mapRotation * vec;
        //                            posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (posObj.ContainsKey(a3_sportsProxy.getInstance().list_position[i].iid))
        //                {
        //                    posObj[a3_sportsProxy.getInstance().list_position[i].iid].SetActive(true);

        //                    int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
        //                    int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
        //                    Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
        //                    vec = mapRotation * vec;
        //                    posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
        //                }
        //                else
        //                {
        //                    for (int m = 0; m < enemyCon.childCount; m++)
        //                    {
        //                        if (enemyCon.GetChild(m).gameObject.activeSelf == false)
        //                        {
        //                            enemyCon.GetChild(m).gameObject.SetActive(true);
        //                            posObj[a3_sportsProxy.getInstance().list_position[i].iid] = enemyCon.GetChild(m).gameObject;
        //                            int x2 = (int)a3_sportsProxy.getInstance().list_position[i].x;
        //                            int y2 = (int)a3_sportsProxy.getInstance().list_position[i].y;
        //                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
        //                            vec = mapRotation * vec;
        //                            posObj[a3_sportsProxy.getInstance().list_position[i].iid].transform.localPosition = vec;
        //                            break;
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //}


        //Dictionary<int, GameObject> posObj = new Dictionary<int, GameObject>();

        void refreshPos_otherP()
        {

           // Debug.LogError(PlayerModel.getInstance().lvlsideid);
            A3_cityOfWarProxy.getInstance().sendPos_info();
            if (A3_cityOfWarProxy .getInstance ().list_position  != null && A3_cityOfWarProxy.getInstance().list_position.Count  > 0) {
                foreach (PlayerPos_cityWar info in A3_cityOfWarProxy.getInstance().list_position.Values)
                {
                    if (info.lvlsideid == PlayerModel.getInstance().lvlsideid)
                    {
                        if (posObj_team.ContainsKey(info.iid))
                        {
                            int x2 = (int)info.x;
                            int y2 = (int)info.y;
                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                            vec = mapRotation * vec;
                            posObj_team[info.iid].transform.localPosition = vec;
                        }
                        else
                        {
                            GameObject clon = Instantiate(TeamObj) as GameObject;
                            clon.SetActive(true);
                            int x2 = (int)info.x;
                            int y2 = (int)info.y;
                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                            vec = mapRotation * vec;
                            clon.transform.localPosition = vec;
                            posObj_team[info.iid] = clon;
                            clon.transform.SetParent(TeammateCon,false);
                        }
                    }
                    else {
                        if (posObj_enemy.ContainsKey(info.iid))
                        {
                            int x2 = (int)info.x;
                            int y2 = (int)info.y;
                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                            vec = mapRotation * vec;
                            posObj_enemy[info.iid].transform.localPosition = vec;
                        }
                        else
                        {
                            GameObject clon = Instantiate(EnemyObj) as GameObject;
                            clon.SetActive(true);
                            int x2 = (int)info.x;
                            int y2 = (int)info.y;
                            Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);
                            vec = mapRotation * vec;
                            clon.transform.localPosition = vec;
                            clon.transform.SetParent(enemyCon, false);
                            posObj_enemy[info.iid] = clon;
                        }
                    }
                }
            }

            List<int> clearid = new List<int>();
            foreach (int id in posObj_team.Keys ) {
                if ( !A3_cityOfWarProxy.getInstance().list_position.ContainsKey (id)) {
                    clearid.Add(id);
                }
            }
            foreach (int id in posObj_enemy.Keys)
            {
                if (!A3_cityOfWarProxy.getInstance().list_position.ContainsKey(id))
                {
                    clearid.Add(id);
                }
            }

            foreach (int i in clearid) {
                if (posObj_team.ContainsKey (i)) {
                    Destroy(posObj_team[i]);
                    posObj_team.Remove(i);
                }

                if (posObj_enemy.ContainsKey(i))
                {
                    Destroy(posObj_enemy[i]);
                    posObj_enemy.Remove(i);
                }
            }


        }
        void onCloseMap(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIBASEMAP2);
        }

    }
}
