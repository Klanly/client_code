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
    class a3_liteMiniBaseMap : FloatUi
    {
        public static a3_liteMiniBaseMap instance;

        public Transform transItemCon;
        public GameObject goUser;
        public GameObject goTempEnemy;
        public GameObject goTempBoss;
        public GameObject citydoor;
        public GameObject cityNpc;
        public GameObject goTempNpc;
        public GameObject goTempJy;
        public GameObject goTempOther;
        public GameObject goTempFriend;
        public GameObject goTempJudian;

        MiniMapItem curMiniMap;
        public Camera m_minimap_camara;
        public static GameObject camGo;
        public static float usrAnglesOffset = 0f;

        Text MapName;

        Dictionary<string, GameObject> dMonInMinimap = new Dictionary<string, GameObject>();

        public override void init()
        {
            alain();

            this.transform.SetAsFirstSibling();

            transItemCon = getTransformByPath("normal/minimap/itemcon");
            goUser = getGameObjectByPath("normal/minimap/itemcon/tempU");
            goTempEnemy = getGameObjectByPath("normal/minimap/tempE");
            goTempBoss = getGameObjectByPath("normal/minimap/tempboss");
            cityNpc = getGameObjectByPath("normal/minimap/cityNpc");
            citydoor = getGameObjectByPath("normal/minimap/citydoor");
            goTempNpc = getGameObjectByPath("normal/minimap/tempNpc");
            goTempJy = getGameObjectByPath("normal/minimap/tempjy");
            goTempOther = getGameObjectByPath("normal/minimap/tempOther");
            goTempFriend = getGameObjectByPath("normal/minimap/tempFriend");
            goTempJudian = getGameObjectByPath("normal/minimap/tempJudian");

            MapName = transform.FindChild("normal/minimap/name").GetComponent<Text>();

            BaseButton btn_open = new BaseButton(transform.FindChild("btn_do"));
            btn_open.onClick = onOpenMap;

            goTempNpc.SetActive(false);
            goTempJy.SetActive(false);
            goTempOther.SetActive(false);
            goTempBoss.SetActive(false);
            goTempEnemy.SetActive(false);
            goTempFriend.SetActive(false);
            goTempJudian.SetActive(false);
        }

        public override void onShowed()
        {
            instance = this;
            initm_minimap_camara();
            setMapName();
            //transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
        }

        void setMapName()
        {
            uint mapid = (uint)GRMap.instance.m_nCurMapID;
            Variant mapConf = SvrMapConfig.instance.getSingleMapConf(mapid);
            if (mapConf.ContainsKey ("map_name")) {
                MapName.text = mapConf["map_name"];
            }
        }
        public override void onClosed()
        {
            instance = null;
            clear();
            Destroy(camGo);
        }
        public void Update()
        {
            if (GRMap.grmap_loading)
                return;

            if (camGo == null || SelfRole._inst == null || SelfRole._inst.m_curModel == null)
                return;
            camGo.transform.position = SelfRole._inst.m_curModel.position;
            Dictionary<uint, ProfessionRole> m_role = OtherPlayerMgr._inst.m_mapOtherPlayer;
            Vector3 vec;


            goUser.transform.localEulerAngles = new Vector3(0f, 0f, usrAnglesOffset - SelfRole._inst.m_curModel.eulerAngles.y);

            foreach (ProfessionRole role in m_role.Values)
            {
                if (role.m_curPhy != null)
                {
                    GameObject go;

                    if (dMonInMinimap.ContainsKey(role.strIID))
                    {
                        go = dMonInMinimap[role.strIID];
                    }
                    else
                    {
                        if (PlayerModel.getInstance().lvlsideid != 0)
                        {//阵营模式
                            if (GRMap.instance.m_nCurMapID == 3358) {
                                if (PlayerModel.getInstance().lvlsideid == role.lvlsideid) {
                                    go = Instantiate(goTempEnemy) as GameObject;
                                }
                                else
                                    go = Instantiate(goTempFriend) as GameObject;
                            }
                            else
                            {
                                if (role.lvlsideid == 1)
                                    go = Instantiate(goTempFriend) as GameObject;
                                else
                                    go = Instantiate(goTempEnemy) as GameObject;
                            }
                        }
                        else
                        {
                            go = Instantiate(goTempOther) as GameObject;

                        }
                        go.name = role.strIID;
                        dMonInMinimap[role.strIID] = go;
                        go.transform.SetParent(transItemCon);
                    }

                    vec = InterfaceMgr.ui_Camera_cam.ScreenToWorldPoint(m_minimap_camara.WorldToScreenPoint(role.m_curPhy.position));
                    vec.z = 0;
                    go.SetActive(true);
                    go.transform.position = vec;
                }
            }

            List<MonsterRole> list = MonsterMgr._inst.m_listMonster;
            foreach (MonsterRole role in list)
            {
                if (role.m_curPhy != null)
                {
                    GameObject go;
                    if (dMonInMinimap.ContainsKey(role.strIID))
                    {
                        go = dMonInMinimap[role.strIID];

                    }
                    else
                    {
                        int type = role.tempXMl.getInt("boss");
                        if (type == 1)
                            go = GameObject.Instantiate(goTempBoss) as GameObject;
                        else if (type == 2)
                            go = GameObject.Instantiate(goTempJy) as GameObject;
                        else if ( type == 3)
                            go = GameObject.Instantiate(citydoor) as GameObject;
                        else if (type == 4)
                            go = GameObject.Instantiate(cityNpc) as GameObject;
                        else
                            go = GameObject.Instantiate(goTempEnemy) as GameObject;
                        go.name = role.strIID;

                        go.SetActive(true);
                        dMonInMinimap[role.strIID] = go;
                        go.transform.SetParent(transItemCon);
                    }




                    vec = InterfaceMgr.ui_Camera_cam.ScreenToWorldPoint(m_minimap_camara.WorldToScreenPoint(role.m_curPhy.position));
                    vec.z = 0;
                    go.transform.position = vec;
                }

                //vec = m_minimap_camara.WorldToScreenPoint(SelfRole._inst .m_curModel.position);
                //vec.z = 0;
                //goUser.transform.position = vec;
            }

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
                    go = Instantiate(goTempJudian) as GameObject;
                    go.SetActive(true);
                    dMonInMinimap["falg" + flag.flag_idx] = go;
                    go.transform.SetParent(transItemCon);
                }
                if (flag.owner_side == 0)
                {
                    go.transform.FindChild("tempNone").gameObject.SetActive(true);
                    go.transform.FindChild("tempOn").gameObject.SetActive(false);
                    go.transform.FindChild("tempOut").gameObject.SetActive(false);
                }
                else if (flag.owner_side == 2)
                {
                    go.transform.FindChild("tempNone").gameObject.SetActive(false);
                    go.transform.FindChild("tempOn").gameObject.SetActive(true);
                    go.transform.FindChild("tempOut").gameObject.SetActive(false);
                }
                else if(flag.owner_side == 1)
                {
                    go.transform.FindChild("tempNone").gameObject.SetActive(false);
                    go.transform.FindChild("tempOn").gameObject.SetActive(false);
                    go.transform.FindChild("tempOut").gameObject.SetActive(true);
                }
                Vector3 pos = new Vector3(flag.flag_pos.x / GameConstant.PIXEL_TRANS_UNITYPOS,
                    flag.flag_pos.y / GameConstant.PIXEL_TRANS_UNITYPOS,
                    flag.flag_pos.z / GameConstant.PIXEL_TRANS_UNITYPOS); 

                vec = InterfaceMgr.ui_Camera_cam.ScreenToWorldPoint(m_minimap_camara.WorldToScreenPoint(pos));
                vec.z = 0;
                go.transform.position = vec;
            }
        }
        public void initm_minimap_camara()
        {
            camGo = GameObject.Find("camera_camera_minimap(Clone)");
            if (m_minimap_camara == null)
            {
                GameObject temogo = GAMEAPI.ABUI_LoadPrefab("camera_camera_minimap");
                camGo = GameObject.Instantiate(temogo) as GameObject;
                Application.DontDestroyOnLoad(camGo);
                SceneCamera.refreshMiniMapCanvas();  //临时

            }
            m_minimap_camara = camGo.transform.FindChild("camera").GetComponent<Camera>();

            RectTransform trans = GameObject.Find("camcon").GetComponent<RectTransform>();

            Vector3 pos0 = trans.position;
            camGo.transform.SetParent(this.transform);
            camGo.transform.SetAsFirstSibling();
            float w = (Baselayer.uiWidth + trans.anchoredPosition.x - trans.rect.width) / Baselayer.uiWidth;
            float h = (Baselayer.uiHeight + trans.anchoredPosition.y - trans.rect.height) / Baselayer.uiHeight;
            Vector3 pos1 = InterfaceMgr.ui_Camera_cam.WorldToScreenPoint(pos0);

            m_minimap_camara.rect = new UnityEngine.Rect(w, h, trans.sizeDelta.x / Baselayer.uiWidth, trans.sizeDelta.y / Baselayer.uiHeight);

            refreshMiniCam();
        }
        public void refreshMiniCam()
        {
            if (m_minimap_camara == null || SceneCamera.m_curCamGo == null)
                return;

            Vector3 vec;




            float temp = (SceneCamera.m_curCamGo.transform.transform.eulerAngles.y) % 360f;
            vec = m_minimap_camara.transform.eulerAngles;
            //  debug.Log(":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::" + temp);

            if (temp < 90f)
            {
                // vec = m_minimap_camara.transform.eulerAngles;
                usrAnglesOffset = vec.y = 90f;
            }
            else if (temp < 180f)
            {
                usrAnglesOffset = vec.y = 180f;
            }
            else if (temp < 270f)
            {
                // vec = m_minimap_camara.transform.eulerAngles;
                usrAnglesOffset = vec.y = 270f;
            }
            else
            {
                // vec = m_minimap_camara.transform.eulerAngles;
                usrAnglesOffset = vec.y = 0f;
            }
            m_minimap_camara.transform.eulerAngles = vec;

        }

        public void clear()
        {
            foreach (GameObject go in dMonInMinimap.Values)
            {
                GameObject.Destroy(go);
            }

            dMonInMinimap.Clear();
        }

        public void removeRoleInMiniMap(string iid)
        {

            if (dMonInMinimap.ContainsKey(iid))
            {
                GameObject go = dMonInMinimap[iid];
                dMonInMinimap.Remove(iid);
                Destroy(go);
            }

        }

        void onOpenMap(GameObject go)
        {
            if(GRMap.instance.m_nCurMapID == 3358)
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LITEMINIBASEMAP2);
            else 
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LITEMINIBASEMAP1);
        }

    }
}
