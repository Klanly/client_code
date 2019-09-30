using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Cross;
using DG.Tweening;
namespace MuGame
{
    class worldmap : Window
    {
        public static GameObject EFFECT_CHUANSONG1;
        public static GameObject EFFECT_CHUANSONG2;

        BaseButton btclose;
        public TabControl tab;


        //public bool worldmaps;
        GameObject m_goWorldmap;
        public GameObject m_goMapcon;

        MiniMapItem curMiniMap;

        GameObject goP;
        GameObject goP1;
        GameObject goP22;
        GameObject sg;

        ItemList npcList;
        ItemList monsterList;
        ItemList wayList;
        public static int mapid = 0;
        public static float ggnum = 1;
        //   public List<BaseButton> lButton;
        public static worldmap instance;

        BaseButton btGoCity;

        TickItem tick;

        GameObject goTabrole;
        GameObject temp1;

        Vector3 vecBegin;
        Vector3 vecEnd;

        a3_mapChangeLine mapChangeLine;

        public Quaternion mapRotation; //地图旋转角度（由于场景摄像机调试角度引起的转向）

        public static float tempH;
        public List<GameObject> ssg = new List<GameObject>();
        public static bool getmapid = false;


        public static worldmap _instance;
        public override void init()
        {
            _instance = this;
            ditu = getComponentByPath<Text>("shijieditu");
            // PlayerModel.SendPosition();
            m_goWorldmap = getGameObjectByPath("worldmap");
            //vecBegin = m_goWorldmap.transform.position;
            //vecEnd = vecBegin;
            //vecEnd.y = 600f;
            //m_goWorldmap.transform.position = vecEnd;
            m_goMapcon = getGameObjectByPath("mapcon");

            btclose = new BaseButton(getTransformByPath("btclose"));
            btclose.onClick = onClose;

            btGoCity = new BaseButton(getTransformByPath("tabrole/gotomain"));
            btGoCity.onClick = onGotoCity;

            GameObject temp = getGameObjectByPath("tabrole/temp");
            temp.SetActive(false);
            tempH = temp.GetComponent<RectTransform>().sizeDelta.y;
            npcList = new ItemList(getTransformByPath("tabrole/npcmain"), 0, temp, onNpcListClick, onMonsterListClick, onWayListClick);
            monsterList = new ItemList(getTransformByPath("tabrole/monstermain"), 1, temp, onNpcListClick, onMonsterListClick, onWayListClick);
            wayList = new ItemList(getTransformByPath("tabrole/waymain"), 2, temp, onNpcListClick, onMonsterListClick, onWayListClick);
            goTabrole = getGameObjectByPath("tabrole");

            tick = new TickItem(onUpdate);

            tab = new TabControl();
            tab.onClickHanle = ontab;

            tab.create(getGameObjectByPath("tab"), gameObject, 0, 0);
            


            goP = getGameObjectByPath("mapcon/p");
            goP1 = getGameObjectByPath("mapcon/icon");
            goP22 = transform.FindChild("worldmap/icon").gameObject;
            //BaseButton btnChangeLine = new BaseButton(getTransformByPath("tabrole/btnChangeLine"));
            //btnChangeLine.onClick = onBtnChangeLineClick;
           

            initWorldMap();



            if (m_goMapcon.activeInHierarchy && TeamProxy.getInstance().MyTeamData != null)
                teampos();
            //resfreshTeamPos();
            mapRotation = Quaternion.Euler(0f, 0f, 0f);


            getComponentByPath<Text>("tab/world/Text").text = ContMgr.getCont("worldmap_0");
            getComponentByPath<Text>("tab/map/Text").text = ContMgr.getCont("worldmap_1");
            getComponentByPath<Text>("tabrole/monstermain/way/Text").text = ContMgr.getCont("worldmap_2");
            getComponentByPath<Text>("tabrole/monstermain/monster/Text").text = ContMgr.getCont("worldmap_3");
            getComponentByPath<Text>("tabrole/npcmain/way/Text").text = ContMgr.getCont("worldmap_2");
            getComponentByPath<Text>("tabrole/npcmain/monster/Text").text = ContMgr.getCont("worldmap_3");
            getComponentByPath<Text>("tabrole/waymain/way/Text").text = ContMgr.getCont("worldmap_2");
            getComponentByPath<Text>("tabrole/waymain/monster/Text").text = ContMgr.getCont("worldmap_3");
            getComponentByPath<Text>("tabrole/gotomain/Text").text = ContMgr.getCont("worldmap_4");
            getComponentByPath<Text>("tabrole/btnChangeLine/Text").text = ContMgr.getCont("worldmap_5");
            getComponentByPath<Text>("worldmap/title/Text").text = ContMgr.getCont("worldmap_6");
            getComponentByPath<Text>("worldmap/name").text = ContMgr.getCont("worldmap_7");
            getComponentByPath<Text>("shijieditu").text = ContMgr.getCont("worldmap_7");
            transform.SetAsLastSibling();


        }


        //===================================================================
        public void teamWorldPic()
        {
            ////ItemTeamMemberData ss = new ItemTeamMemberData();
            //ssg.Clear();

            if (TeamProxy.getInstance().MyTeamData != null)
            {
                getmapid = true;
                TeamProxy.getInstance().SendWatchTeamInfo(TeamProxy.getInstance().MyTeamData.teamId);
            }



            foreach (var v in goP22.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == goP22.transform)
                    v.gameObject.SetActive(false);
            }

            if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().teamMemberposData != null)
            {
                for (int i = 0; i < TeamProxy.getInstance().teamMemberposData.Count; i++)
                {
                    uint cc = TeamProxy.getInstance().teamMemberposData[i].mapId;


                    if (TeamProxy.getInstance().teamMemberposData[i].isCaptain)//
                    {
                        temp1 = goP22.transform.FindChild("signalteam").gameObject;
                        temp1.SetActive(true);
                        //sg = GameObject.Find("signalteam");
                        //temp1 = GameObject.Instantiate(sg) as GameObject;
                    }
                    else
                    {
                        temp1 = goP22.transform.FindChild("signal" + i).gameObject;
                        temp1.SetActive(true);
                        //sg = GameObject.Find("signal");
                        //temp1 = GameObject.Instantiate(sg) as GameObject;

                    }

                    //if (TeamProxy.getInstance().teamMemberposData[i].online == false)
                    //{
                    //    temp1 = goP22.transform.FindChild("signal_lx").gameObject;
                    //    temp1.SetActive(true);
                    //}

                    if(cc==0)
                        continue;

                    temp1.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    float x = transform.FindChild("worldmap/map/" + cc).localPosition.x;
                    float y = transform.FindChild("worldmap/map/" + cc).localPosition.y;

                    temp1.transform.localPosition = new Vector3(x, y, 0);

                    //ssg.Add(temp1);

                }
            }


        }
        
        public void onGotoCity(GameObject go)
        {
            if (!FindBestoModel.getInstance().Canfly)
            {
                flytxt.instance.fly(FindBestoModel.getInstance().nofly_txt);
                InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
                return;
            }
            cd.updateHandle = worldmapsubwin.onCD;

            GameObject goeff = GameObject.Instantiate(EFFECT_CHUANSONG1) as GameObject;
            goeff.transform.SetParent(SelfRole._inst.m_curModel, false);

            cd.show(() =>
            {
                MapProxy.getInstance().sendBeginChangeMap(0, true);
            },
            2.8f, false,
            () =>
            {
                Destroy(goeff);
            }

            );
            InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
        }

        public void teampos()
        {
            if (m_goMapcon.activeInHierarchy&& TeamProxy.getInstance().MyTeamData != null)
            {
                resfreshTeamPos();
            }
        }
        public void worldteampos()
        {
            if (m_goWorldmap.activeInHierarchy && TeamProxy.getInstance().MyTeamData != null)
            {
                teamWorldPic();
            }
        }
        public void onUpdate(float s)
        {

            //if (InterfaceMgr.getInstance().worldmap == true) changetab(0);
            refreshPos();

        }
        public void planePic()
        {

            if (TeamProxy.getInstance().MyTeamData != null)
            {


                if (TeamProxy.getInstance().teamlist_position.Count != 0 && TeamProxy.getInstance().MyTeamData.meIsCaptain == false)
                {

                    goP.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_pic_team_member");
                    goP.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.65f, 0.75f);
                }
                if (TeamProxy.getInstance().teamlist_position.Count != 0 && TeamProxy.getInstance().MyTeamData.meIsCaptain == true)
                {

                    goP.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_pic_team_captain");
                    goP.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.65f, 0.75f);
                }


            }
            else
            {
                goP.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_pic_player");

            }
        }
        public void resfreshTeamPos()
        {

            //ssg.Clear();

            TeamProxy.getInstance().SendCurrentMapTeamPos();


            //foreach (var v in goP1.GetComponentsInChildren<Transform>(true))
            //{
            //    if (v.parent == goP1.transform)
            //        v.gameObject.SetActive(false);

            //}
            //===============================================
            //getmapid = true;
            //TeamProxy.getInstance().SendWatchTeamInfo(PlayerModel.getInstance().teamid);


            if (TeamProxy.getInstance().teamlist_position != null && TeamProxy.getInstance().MyTeamData != null)
            {
                for (int i = 0; i < TeamProxy.getInstance().teamlist_position.Count; i++)
                {

                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain == false && TeamProxy.getInstance().MyTeamData.leaderCid == TeamProxy.getInstance().teamlist_position[i].cid)//
                    {
                        temp1 = goP1.transform.FindChild("signalteam").gameObject;
                        temp1.SetActive(true);
                        temp1.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    }
                    else
                    {
                        temp1 = goP1.transform.FindChild("signal" + i).gameObject;
                        temp1.SetActive(true);
                        temp1.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    }
                 
                    //if (TeamProxy.getInstance().teamMemberposData[i].online == false)
                    //{
                    //    temp1 = goP1.transform.FindChild("signal_lx").gameObject;
                    //    temp1.SetActive(true);
                    //    temp1.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    //}



                    // temp1.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);


                    //ssg.Add(temp1);
                    //===========================================================
                    int x2 = (int)TeamProxy.getInstance().teamlist_position[i].x;

                    int y2 = (int)TeamProxy.getInstance().teamlist_position[i].y;


                    Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);

                    vec = mapRotation * vec;

                    temp1.transform.localPosition = vec;
                }
            }

            //if (TeamProxy.getInstance().teamlist_position!=null&&TeamProxy.getInstance().teamlist_position.Count != 0)
            //{
            //    for (int i = 0; i < TeamProxy.getInstance().teamlist_position.Count; i++)
            //    {
            //        int x2 = (int)TeamProxy.getInstance().teamlist_position[i].x;

            //        int y2 = (int)TeamProxy.getInstance().teamlist_position[i].y;


            //        Vector3 vec = SceneCamera.getTeamPosOnMinMap(x2, y2, curMiniMap.mapScale);

            //        ssg[i].transform.localPosition = vec;

            //    }
            //}


            //planePic();
        }



        public void onNpcListClick()
        {
            onRoleTab(0);
        }

        public void onMonsterListClick()
        {
            onRoleTab(1);
        }

        public void onWayListClick()
        {
            onRoleTab(2);
        }



        private bool firstRun = true;
        public override void onShowed()
        {
            instance = this;
           // getGameObjectByPath("linePanel").SetActive(false);
            firstRun = true;
            tab.setSelectedIndex(1, true);
            TickMgr.instance.addTick(tick);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);
            m_goWorldmap.transform.FindChild("map/box_dxc").gameObject.SetActive(false);
            m_goWorldmap.transform.FindChild("map/box_ahsd").gameObject.SetActive(false);
          
            if (InterfaceMgr.getInstance().worldmap == true)
            {
                // m_goWorldmap.SetActive(true);
               
                //Debug.LogError("tttt");
                //changetab(0);
                tab.setSelectedIndex(0);
                //goTabrole.SetActive(false);
                //transform.FindChild("tab/world").gameObject
                //transform.FindChild("tab/world").GetComponent<Button>().interactable = true;
                //transform.FindChild("tab/map").GetComponent<Button>().interactable = false;
                //tab.create(getGameObjectByPath("tab"), gameObject, 0, 0, true);
                InterfaceMgr.getInstance().worldmap = false;
            }
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        public override void onClosed()
        {
            if (a3_active.instance != null)
            {
                if (a3_active.instance.map_light)
                {
                    a3_active.instance.map_light = false;
                }
            }
             InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            m_goWorldmap.transform.DOKill();
            TickMgr.instance.removeTick(tick);
            instance = null;
            mapid = 0;
            base.onClosed();
        }

        public void onRoleTab(int type)
        {
            if (tab == null || tab.getSeletedIndex() == 0 || curMiniMap == null)
                return;
            Transform goWay = curMiniMap.__mainTrans.Find("way");
            Transform goNpc = curMiniMap.__mainTrans.Find("npc");
            Transform goMonster = curMiniMap.__mainTrans.Find("monster");
            switch (type)
            {
                //case 0:

                //    if (goNpc)
                //        goNpc.gameObject.SetActive(true);
                //    if (goMonster)
                //        goMonster.gameObject.SetActive(true);
                //    break;
                case 0:

                    if (goNpc)
                        goNpc.gameObject.SetActive(true);
                    if (goMonster)
                        goMonster.gameObject.SetActive(false);
                    if (goWay)
                        goWay.gameObject.SetActive(false);
                    monsterList.visiable = false;
                    npcList.visiable = true;
                    wayList.visiable = false;
                    break;
                case 1:
                    if (goNpc)
                        goNpc.gameObject.SetActive(false);
                    if (goMonster)
                        goMonster.gameObject.SetActive(true);
                    if (goWay)
                        goWay.gameObject.SetActive(false);
                    monsterList.visiable = true;
                    npcList.visiable = false;
                    wayList.visiable = false;
                    break;
                case 2:
                   
                    if (goNpc)
                        goNpc.gameObject.SetActive(false);
                    if (goMonster)
                        goMonster.gameObject.SetActive(false);
                    if (goWay)
                        goWay.gameObject.SetActive(true);                  
                    monsterList.visiable = false;
                    npcList.visiable = false;
                    wayList.visiable = true;
                    break;
            }
        }

        List<MapBt> lMap = new List<MapBt>();
        public void initWorldMap()
        {

            Transform t = getTransformByPath("worldmap/bts");
            Transform tmap = getTransformByPath("worldmap/map");
            for (int i = 0; i < t.childCount; i++)
            {
                Transform trans = t.GetChild(i);
                Transform transmap = tmap.FindChild(trans.gameObject.name);
                lMap.Add(new MapBt(trans, transmap));
            }
        }
        Text curMap;
        Text ditu;
        public void changetab(int idx)
        {

            if (idx == 0)
            {
                m_goWorldmap.SetActive(true);
                if(TeamProxy.getInstance().MyTeamData != null)
                InvokeRepeating("teampos", 0, 0.5f);

                //m_goWorldmap.transform.DOKill();
                //m_goWorldmap.transform.position = vecEnd;
                //m_goWorldmap.transform.DOMove(vecBegin, 0.5f);
                m_goMapcon.SetActive(true);
                goTabrole.SetActive(false);
                monsterList.visiable = false;
                npcList.visiable = false;
                if (a3_active.instance != null)
                {
                    if (a3_active.instance.map_light)
                    {
                     
                        m_goWorldmap.transform.FindChild("map/box_dxc").gameObject.SetActive(true);
                        m_goWorldmap.transform.FindChild("map/box_ahsd").gameObject.SetActive(true);
                    }
                }
               
                for (int i = 0; i < lMap.Count; i++)
                {
             
                    lMap[i].refresh();
                }

                if (curMiniMap != null)
                {
                    curMap = curMiniMap.transform.FindChild("a/mainbt/Text").GetComponent<Text>();
                    curMap.gameObject.SetActive(false);
                    ditu.gameObject.SetActive(true);
                }
            }
            else
            {
                if (curMiniMap != null)
                {
                    ditu.gameObject.SetActive(false);
                    curMap = curMiniMap.transform.FindChild("a/mainbt/Text").GetComponent<Text>();
                    curMap.gameObject.SetActive(true);
                }

                m_goWorldmap.SetActive(false);
                InvokeRepeating("worldteampos", 0, 5f);
                //curMap.text=
                //if (firstRun)
                //{
                //    m_goWorldmap.SetActive(false);
                //}
                //else
                //{
                //    m_goWorldmap.transform.DOKill();
                //    m_goWorldmap.transform.position = vecBegin;
                //    m_goWorldmap.transform.DOMove(vecEnd, 0.5f).OnComplete(() =>
                //    {
                //        m_goWorldmap.SetActive(false);
                //    });
                //}
                m_goMapcon.SetActive(true);
                goTabrole.SetActive(true);
                onRoleTab(2);
                if (mapid == 0)
                    mapid = GRMap.instance.m_nCurMapID;



                if (curMiniMap == null || curMiniMap.id != mapid)
                {
                    refreshMiniMap();
                }

                refreshPos();

                SXML xml = XMLMgr.instance.GetSXML("mappoint");
                List<SXML> l = xml.GetNodeList("p", "mapid==" + mapid);
                npcList.refresh(l);
                wayList.refresh(l);
                monsterList.refresh(l);
            }
            firstRun = false;
        }

        public void refreshMiniMap()
        {
            if (curMiniMap != null)
            {
                curMiniMap.dispose();
                curMiniMap = null;
            }




            GameObject temp = GAMEAPI.ABUI_LoadPrefab("map_prefab_map" + mapid);
            if (temp == null)
                return;

            SXML xml = XMLMgr.instance.GetSXML("mappoint.trans_remind","map_id==" + mapid);
            if(xml.getInt("rotation") != -1)
                mapRotation = mapRotation = Quaternion.Euler(0f, 0f, xml.getInt("rotation"));
            else
                mapRotation = Quaternion.Euler(0f, 0f, 0);

            GameObject go = GameObject.Instantiate(temp) as GameObject;
            go.transform.FindChild("map").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("map_map" + mapid);
            go.transform.SetParent(m_goMapcon.transform, false);
            curMiniMap = new MiniMapItem(go.transform, mapRotation);
            curMiniMap.id = mapid;
            Transform goNone = curMiniMap.__mainTrans.Find("none");
            if (goNone != null)
            {
                goNone.gameObject.SetActive(false);
            }
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

        public void ontab(TabControl t)
        {
            int idx = t.getSeletedIndex();
            changetab(idx);
            if (idx == 1)
                onRoleTab(2);
        }

        public void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
            if (a3_active.instance != null)
            {
                if (a3_active.instance.map_light)
                {
                    a3_active.instance.map_light = false;
                }
            }
        }

        private static Dictionary<int, MapLinkItem> dMapLink;
        private static void initLink()
        {
            dMapLink = new Dictionary<int, MapLinkItem>();
            Dictionary<uint, Variant> l = SvrMapConfig.instance._mapConfs;
            foreach (Variant v in l.Values)
            {
                MapLinkItem item = new MapLinkItem();
                item.mapid = v["id"];

                if (!v.ContainsKey("l"))
                    continue;

                List<Variant> ll = v["l"]._arr;
                for (int k = 0; k < ll.Count; k++)
                {


                    Variant vinfo = ll[k];

                    if (vinfo.ContainsKey("trans") && vinfo["trans"] == 1)
                    {
                        MapLinkInfo info = new MapLinkInfo();
                        info.id = vinfo["id"];
                        info.mapid = item.mapid;
                        info.toMap = vinfo["id"];
                        info.id = vinfo["gto"];
                        info.x = vinfo["ux"];
                        info.y = vinfo["uz"];
                        item.lLink.Add(info);
                    }


                }
                dMapLink[item.mapid] = item;
            }
        }
        public static bool getMapLine(int begin, int end, List<MapLinkInfo> line, Dictionary<int, int> dfinded = null)
        {

            if (dMapLink == null) initLink();

            if (dfinded == null)
                dfinded = new Dictionary<int, int>();

            if (begin == end)
            {
                return true;
            }

            if (!dMapLink.ContainsKey(begin))
                return false;


            MapLinkItem item = dMapLink[begin];
            dfinded[begin] = 1;
            for (int i = 0; i < item.lLink.Count; i++)
            {

                int id = item.lLink[i].id;



                if (dfinded.ContainsKey(id))
                    continue;


                if (id == end)
                {
                    line.Insert(0, item.lLink[i]);
                    return true;
                }


                if (getMapLine(id, end, line, dfinded))
                {
                    line.Insert(0, item.lLink[i]);
                    return true;
                }


            }
            return false;
        }
        //void onBtnChangeLineClick(GameObject go)
        //{
        //    getGameObjectByPath("linePanel").SetActive(true);
        //    ChangeLineProxy.getInstance().sendLineProxy();
        //}


        private List<Vector2> mapImgPath = new List<Vector2>();
        private static List<GameObject> mapImgpb = new List<GameObject>();
        private static GameObject plane;
        private static GameObject mapimg;

        //画小地图自动寻路路径 zjw
        public void DrawMapImage(NavMeshPath path)
        {


            if (plane != null)
            {

                Desmapimg();


            }
            else
            {
                mapimg = GAMEAPI.ABUI_LoadPrefab("map_icon_scene_mappath");
                plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.name = "NmaImageList";
                plane.transform.SetParent(transform.Find("mapcon").transform);
                plane.transform.localPosition = Vector3.zero;
                plane.transform.localScale = new Vector3(1, 1, 1);
                plane.transform.SetSiblingIndex(transform.Find("mapcon/p").transform.GetSiblingIndex());
            }

            mapImgPath.Clear();

            // mapImgpb.Clear();

            //计算生成坐标
            for (int p = 0; p < path.corners.Length; p++)
            {
                if (p != path.corners.Length - 1)
                {

                    var AllWayp = Vector3.Distance(path.corners[p + 1], path.corners[p]);

                    if (AllWayp > 5f)
                    {
                        var AllNumde = (int)(AllWayp / 5);

                        Vector3 AllWaypV3 = path.corners[p + 1] - path.corners[p];
                        Vector3 WaypV3 = AllWaypV3 / (AllNumde + 1);
                        Vector3 pos = path.corners[p];

                        for (int num = 0; num < AllNumde; num++)
                        {
                            pos += WaypV3;
                            mapImgPath.Add(SceneCamera.getPosOnMiniMapNMA(pos));

                        }

                        mapImgPath.Add(SceneCamera.getPosOnMiniMapNMA(path.corners[p + 1]));

                    }
                    else if (AllWayp > 2f)
                    {

                        mapImgPath.Add(SceneCamera.getPosOnMiniMapNMA(path.corners[p + 1]));

                    }

                }

            }




            for (int pos = 0; pos < mapImgPath.Count; pos++)
            {
                if (mapImgPath.Count > mapImgpb.Count)
                {
                    for (int y = 0; y < (mapImgPath.Count - mapImgpb.Count); y++)
                    {
                        mapImgpb.Add(GameObject.Instantiate(mapimg) as GameObject);
                    }
                }

                mapImgpb[pos].transform.SetParent(plane.transform);

                mapImgpb[pos].transform.localPosition = mapRotation * mapImgPath[pos];
            }


        }


        //清除小地图自动引导图标 zjw
        public static void Desmapimg()
        {
            if (mapImgpb.Count != 0)
            {
                GameObject GC = GameObject.CreatePrimitive(PrimitiveType.Plane);
                GC.name = "GC";

                for (int i = 0; i < mapImgpb.Count; i++)
                {
                    mapImgpb[i].transform.SetParent(GC.transform);
                    mapImgpb[i].SetActive(false);
                }
                mapImgpb.Clear();

                GameObject.Destroy(GC);
            }
        }

        //分线信息
        //public  void line_info(int  e)
        //{
        //    mapChangeLine = new a3_mapChangeLine(getTransformByPath("linePanel"),e);
        //}
    }

    public class MapLinkItem
    {
        public int mapid;
        public List<MapLinkInfo> lLink = new List<MapLinkInfo>();
    }

    public class MapLinkInfo
    {
        public int id;
        public int mapid;
        public float x;
        public float y;
        public int toMap;
    }

    class ItemList : Skin
    {
        Button btWay;
        Button btNpc;
        Button btMonster;
        Transform transCon;
        ScrollControler scrollControler;
        GameObject itemListView;
        GridLayoutGroup item_Parent;
        int mapid = 0;

        int type;
        GameObject tempItem;
        Action npcHandle;
        Action monHandle;
        Action wayHandle;
        public ItemList(Transform trans, int t, GameObject temp, Action npcClick, Action monClick, Action wayClick)
            : base(trans)
        {
            wayHandle = wayClick;
            npcHandle = npcClick;
            monHandle = monClick;
            tempItem = temp;
            type = t;
            initUI();
        }
        int childCount;
        void initUI()
        {
            btWay = getComponentByPath<Button>("way");
            btNpc = getComponentByPath<Button>("npc");
            btMonster = getComponentByPath<Button>("monster");
            transCon = getTransformByPath("con/con");

            if (type == 0)
                btNpc.interactable = false;
            else if (type == 1)
                btMonster.interactable = false;
            else if (type == 2)
                btWay.interactable = false;
            btMonster.onClick.AddListener(onMonsterClick);
            btNpc.onClick.AddListener(onNpcClick);
            btWay.onClick.AddListener(onWayClick);
            itemListView = transform.FindChild("con/con").gameObject;
            item_Parent = itemListView.GetComponent<GridLayoutGroup>();

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("con").GetComponent<ScrollRect>();
            scrollControler.create(scroll);


        }

        protected void refreshScrollRect()
        {
            int num = itemListView.transform.childCount;


            if (num <= 0)
                return;
            //   float height = item_Parent.cellSize.y;
            //  int row = (int)Math.Ceiling((double)num / 5);
            RectTransform rect = itemListView.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0.0f, itemnum * (worldmap.tempH+2));
           //rect.anchoredPosition = new Vector2(90, -(itemnum * worldmap.tempH)/2);
            //Vector2 vec2 = itemListView.transform.parent.GetComponent<RectTransform>().sizeDelta;
            //if (rect.sizeDelta.y < vec2.y)
            //    itemListView.transform.localPosition = new Vector3(0, 0, 0);//rect.sizeDelta.y/num
            //else
            //{
               // itemListView.transform.localPosition = Vector3.zero;//(vec2.y - rect.sizeDelta.y) / 2
            //                                                        //}
            //Transform con = transform.FindChild("con/con");
            //if (itemnum * worldmap.tempH <= 200)
            //{
            //   // itemListView.transform.localPosition = Vector3.zero;
            //    con.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, itemnum * worldmap.tempH);
            //}
           
        }
        public void clear()
        {
            Transform trans = transCon.transform;
            for (int i = 0; i < transCon.GetChildCount(); i++)
            {
                GameObject.Destroy(transCon.GetChild(i).gameObject);
            }
        }

        private int itemnum;
        public void refresh(List<SXML> list)
        {
            clear();
            if (list.Count == 0)
                return;

            itemnum = 0;


            //   int mapid = list[0].getInt("mapid");
            // int lv_up = (int)PlayerModel.getInstance().up_lvl;
            //int lv = (int)PlayerModel.getInstance().lvl;
            for (int i = 0; i < list.Count; i++)
            {
                SXML xml = list[i];

                if (xml.getInt("type") != type)
                    continue;
                itemnum++;
                GameObject go = GameObject.Instantiate(tempItem) as GameObject;
                go.SetActive(true);
                go.transform.SetParent(transCon, false);
                go.transform.FindChild("Text").GetComponent<Text>().text = xml.getString("name");
                go.name = xml.getString(xml.getString("id"));

                Button bt = go.GetComponent<Button>();
                //bt.interactable = (lv_up > xml.getInt("lv_up")) || (lv_up == xml.getInt("lv_up") && lv >= xml.getInt("lv"));
                bt.onClick.AddListener(() =>
                {
                    if (worldmap.mapid == GRMap.instance.m_nCurMapID)
                    {
                        Vector3 vec = new Vector3(xml.getFloat("ux"), 0f, xml.getFloat("uy"));
                        NavMeshHit hit;
                        NavMesh.SamplePosition(vec, out hit, 20f, NavmeshUtils.allARE);
                        SelfRole.moveto(hit.position, null);
                    }
                    else
                    {
                        Vector3 vec = new Vector3(xml.getFloat("ux"), 0f, xml.getFloat("uy"));
                        SelfRole.moveto(worldmap.mapid, vec);
                    }

                    //这里处理点小地图切地图会乱寻路的bug，暂时没有好的办法
                    TransMapPoint.is_need_stop_fsm = true;

                    //worldmapsubwin.mapid = worldmap.mapid;
                    //worldmapsubwin.id = xml.getInt("id");
                    //InterfaceMgr.getInstance().open(InterfaceMgr.WORLD_MAP_SUB);
                });
            }
            refreshScrollRect();
        }

        void onMonsterClick()
        {
            monHandle();
        }
        void onNpcClick()
        {
            npcHandle();
        }

        void onWayClick()
        {
            wayHandle();
        }

    }




    class MiniMapItem : Skin
    {
        public int id = 0;
        Transform map;
        List<GameObject> lNpc = new List<GameObject>();
        List<GameObject> lMonster = new List<GameObject>();
        Quaternion mapRotation;
        GameObject goCon;

        public float mapScale
        {
            get { return map.localScale.x; }
        }

        public MiniMapItem(Transform trans, Quaternion rotation)
            : base(trans)
        {
            mapRotation = rotation;
            SXML xml = XMLMgr.instance.GetSXML("mappoint");
            int mapid = worldmap.mapid * 100;
            Transform tNpc = getTransformByPath("npc");
            tNpc.localRotation = rotation;
            //int lv_up = (int)PlayerModel.getInstance().up_lvl;
            //int lv = (int)PlayerModel.getInstance().lvl;

            for (int i = 0; i < tNpc.childCount; i++)
            {
                Transform t = tNpc.GetChild(i);
                t.localRotation = Quaternion.Inverse(rotation);
                lNpc.Add(t.gameObject);
                //BaseButton bt = new BaseButton(t);
                //bt.onClick = onclick;


                //Transform transTxt = t.FindChild("lv");
                //if (transTxt == null)
                //    continue;
                //Text txt = transTxt.GetComponent<Text>();
                //SXML tempxml = xml.GetNode("p", "id==" + (mapid + int.Parse(t.gameObject.name)));
                //int itemLv_up = tempxml.getInt("lv_up");
                //int itemLv = tempxml.getInt("lv");
                //txt.text = ContMgr.getCont("worldmap_lv", itemLv_up.ToString(), itemLv.ToString());

                //t.GetComponent<Button>().interactable = (lv_up > itemLv_up) || (lv_up == itemLv_up && lv >= itemLv);
            }

            Transform tWay = getTransformByPath("way");
            tWay.localRotation = rotation;
            for (int i = 0; i < tWay.childCount; i++)
            {
                Transform t = tWay.GetChild(i);
                t.localRotation = Quaternion.Inverse(rotation);
            }

            Transform tMonster = getTransformByPath("monster");
            tMonster.localRotation = rotation;
            for (int i = 0; i < tMonster.childCount; i++)
            {
                Transform t = tMonster.GetChild(i);
                t.localRotation = Quaternion.Inverse(rotation);
                lMonster.Add(t.gameObject);
                //BaseButton bt = new BaseButton(t);
                //bt.onClick = onclick;



                Transform transTxt = t.FindChild("lv");
                if (transTxt == null)
                    continue;
                Text txt = transTxt.GetComponent<Text>();
                SXML tempxml = xml.GetNode("p", "id==" + (mapid + int.Parse(t.gameObject.name)));
                int itemLv_up = tempxml.getInt("lv_up");
                int itemLv = tempxml.getInt("lv");
                txt.text = ContMgr.getCont("worldmap_lv", itemLv_up.ToString(), itemLv.ToString());

                //   t.GetComponent<Button>().interactable = (lv_up > itemLv_up) || (lv_up == itemLv_up && lv >= itemLv);
            }
            map = getTransformByPath("map");
            map.localRotation = rotation;
            EventTriggerListener.Get(gameObject).onPointClick = onMapClick;
            Transform main = transform.FindChild("namebt");
            if (main != null)
            {
                Transform con = main.FindChild("con");
                if (con == null)
                    return;

                goCon = con.gameObject;
                //for (int i = 0; i < con.GetChildCount(); i++)
                //{
                //    BaseButton bt = new BaseButton(con.GetChild(i));
                //    bt.onClick = onmapClick;
                //}


                BaseButton mainbt = new BaseButton(main.FindChild("mainbt"));
                mainbt.onClick = (GameObject go) =>
                {
                    goCon.SetActive(!goCon.active);
                };
                goCon.SetActive(false);
            }

        }

        void onMapClick(GameObject go, Vector2 mappos)
        {

            Vector2 pos2;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(worldmap.instance.m_goMapcon.GetComponent<RectTransform>(), mappos, InterfaceMgr.ui_Camera_cam, out pos2))
            {

                pos2 = Quaternion.Inverse(mapRotation) * pos2;
                Vector3 pos = SceneCamera.getPosByMiniMap(pos2, mapScale);
                if (pos.x == Mathf.Infinity)
                    return;

                if (SelfRole.fsm.Autofighting)
                    SelfRole.fsm.Stop();
                SelfRole.moveto(pos, null);


            }


            //Vector3 offset = go.transform.position;


            //mappos.x = mappos.x - offset.x;
            //mappos.y = mappos.y - offset.y;
            //Vector3 pos = SceneCamera.getPosByMiniMap(mappos, mapScale);
            //if (pos.x == Mathf.Infinity)
            //    return;

            //SelfRole.moveto(pos, null);
        }

        void onmapClick(GameObject go)
        {

            worldmap.mapid = int.Parse(go.name);
            worldmap.instance.refreshMiniMap();
            worldmap.instance.refreshPos();
        }

        //void onclick(GameObject go)
        //{
        //    worldmapsubwin.mapid = worldmap.mapid;
        //    worldmapsubwin.id = worldmap.mapid * 100 + int.Parse(go.name);
        //    InterfaceMgr.getInstance().open(InterfaceMgr.WORLD_MAP_SUB);
        //}


        //public void gotoPoint(GameObject go)
        //{
        //    Vector3 vec = go.transform.localPosition / map.localScale.x;
        //    vec = new Vector3(512f - vec.x, 0f, 512f - vec.y);
        //    vec = SceneCamera.getPosByMiniMap(vec);
        //    NavMeshHit hit;
        //    NavMesh.SamplePosition(vec, out hit, 20f, NavmeshUtils.allARE);
        //    SelfRole.moveto(hit.position, null);
        //    InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
        //}

        public void dispose()
        {
            destoryGo();
        }

    }

    class MapBt : Skin
    {
        GameObject ntxt1;
        GameObject ntxt2;
        Text txtLv1;
        Text txtLv2;
        Button bt;
        Animator ani;
        Transform m_transMap;
        Transform translv;
        string strlv;
        Image map_image;
        public MapBt(Transform transBt, Transform transMap)
            : base(transBt)
        {
            m_transMap = transMap;
            initUI();
        }

        void initUI()
        {
            translv = transform.FindChild("lv");

            bt = transform.GetChild(1).GetComponent<Button>();
            bt.onClick.AddListener(onClick);

            //EventTriggerListener evt = EventTriggerListener.Get(bt.gameObject);
            //evt.onDown = onDrag;
            //evt.onUp = evt.onDragOut = onOut;

            txtLv1 = translv.FindChild("txt1").GetComponent<Text>();
            txtLv2 = translv.FindChild("txt2").GetComponent<Text>();

            ntxt1 = translv.FindChild("ntxt1").gameObject;
            ntxt2 = translv.FindChild("ntxt2").gameObject;
            map_image = m_transMap.GetComponent<Image>();
            ani = m_transMap.GetComponent<Animator>();
            ani.enabled = false;

            // refresh();
        }

        void onDrag(GameObject go)
        {
            ani.SetBool("ondrag", true);
        }

        void onOut(GameObject go)
        {
            ani.SetBool("ondrag", false);
        }

        public void refresh()
        {
            
            //ani.SetBool("unable", false);

            //Variant v = SvrMapConfig.instance.getSingleMapConf((uint)paramInts[0]);


            string[] str = translv.GetChild(0).gameObject.name.Split('_');
            if (str.Length != 2)
            {
                setUnEnable();
            }
            else
            {
                strlv = ContMgr.getCont("worldmap_lv", str[0], str[1]);
                int lv_up = (int)PlayerModel.getInstance().up_lvl;
                int lv = (int)PlayerModel.getInstance().lvl;
                if ((lv_up > int.Parse(str[0])) || (lv_up == int.Parse(str[0]) && lv >= int.Parse(str[1])))
                {
                    txtLv1.text = strlv;
                    txtLv2.text = "";
                    ntxt1.SetActive(true);
                    ntxt2.SetActive(false);

                    bt.interactable = true;
                    map_image.color = Color.clear;
                }
                else 
                {
                   
                    ntxt2.SetActive(true);
                    ntxt1.SetActive(false);
                    txtLv1.text = "";
                    txtLv2.text = strlv;
                    setUnEnable();
                }
            }

            //if (this.gameObject .name == "24") {
            //    if (A3_LegionModel.getInstance().myLegion != null && A3_LegionModel.getInstance().myLegion.id == A3_cityOfWarModel.getInstance().def_clanid && A3_cityOfWarModel.getInstance().def_clanid != 0)
            //    {
            //        bt.interactable = true;
            //        map_image.color = Color.clear;
            //    }
            //    else {
            //        setUnEnable();
            //    }
            //}
        }

        void onClick()
        {
            string n = bt.gameObject.name.Substring(0, 1);

            if (n == "p")
            {
                /*worldmapsubwin.*/int id = int.Parse(bt.gameObject.name.Substring(1, bt.gameObject.name.Length - 1));
                //InterfaceMgr.getInstance().open(InterfaceMgr.WORLD_MAP_SUB);                
                SelfRole.Transmit(toid:id);
                InterfaceMgr.getInstance().close(InterfaceMgr.WORLD_MAP);
            }
            else
            {
                worldmap.mapid = int.Parse(bt.gameObject.name);
                worldmap.instance.tab.setSelectedIndex(1);
            }
        }

        public void setUnEnable()
        {
         
            bt.interactable = false;
            // mapImg.color = Globle.COLOR_GLAY;
            // ani.SetBool("unable", true);
            map_image.color = Color.gray;
        }

    }
}
