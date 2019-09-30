using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
using UnityEngine.SceneManagement;
using MuGame.Qsmy.model;
namespace MuGame
{
    //public class Plot_SkAniBehaviour : MonoBehaviour
    //{
    //    public void onEventAnim()
    //    {
    //        //Debug.Log("剧情里的 onEventAnim");
    //    }
    //}

    public class GRMap : GRBaseImpls
    {
        public static GRMap instance;
        public static bool haveListener = false;

        public static bool grmap_loading = true;




        public GRMap(muGRClient m)
            : base(m)
        {
            instance = this;
        }

        public static IObjectPlugin create(IClientBase m)
        {
            //if(instance != null)
            //{
            //    return instance;
            //}

            return new GRMap(m as muGRClient);
        }

        private Variant fly_array = new Variant();

        override public void init()
        {
            SceneManager.sceneLoaded += this.StartGame;
        }

        override protected void onSetSceneCtrl()
        {
            if (haveListener)
                return;

            m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_DATA, setFirstMapData);
            //  m_ctrl.addEventListener(GAME_EVENT.SPRITE_SIZE_CHANGE, onViewSizeChange);
            m_ctrl.addEventListener(GAME_EVENT.MAP_CHANGE, onChangeMap);
            //m_ctrl.addEventListener(GAME_EVENT.MAP_LINK_ADD, onLinkAdd);
            //m_ctrl.addEventListener(GAME_EVENT.MAP_ADD_SHOW_EFF, onAddEff);
            //m_ctrl.addEventListener(GAME_EVENT.MAP_ADD_FLY_EFF, onAddFlyEff);

            haveListener = true;
        }

        override protected void onSetGraphImpl()
        {

        }

        public IGRMap m_map
        {
            get
            {

                return m_gr as IGRMap;
            }
            set
            {

                m_gr = value;
            }
        }

        //data { conf:, map: }
        private void setFirstMapData(GameEvent e)
        {
            if (TriggerHanldePoint.lGo != null)
            {
                TriggerHanldePoint.lGo.Clear();
                TriggerHanldePoint.lGo = null;
            }

            grmap_loading = true;

            Cache_MapChangeData = e.data;
            string an = Cache_MapChangeData["conf"]["name"];
            GAMEAPI.LoadAsset_Async(an + ".assetbundle", an);

            //Variant data = e.data;
            //Variant svrConf = data["conf"];
            //setChangeMapData(data["mapid"]._uint, svrConf, data["localConf"]);

            ////addLinkEffs(data["tmpLinks"]);
            ////if (svrConf != null && svrConf.ContainsKey("l")) addLinkEffs(svrConf["l"]);
        }

        //private void addLinkEffs(Variant links)
        //{
        //    if (links == null || links._arr == null || links._arr.Count <= 0) return;
        //    //
        //    foreach (Variant ln in links._arr)
        //    {
        //        float lx = 0;
        //        float ly = 0;
        //        if (ln.ContainsKey("data"))
        //        {//temp link
        //            lx = ln["data"]["x"]._int;
        //            ly = ln["data"]["y"]._int;
        //        }
        //        else
        //        {
        //            lx = ln["x"]._int;
        //            ly = ln["y"]._int;
        //        }
        //        float z = (float)this.g_mgr.getZ(lx * GameConstant.GEZI, ly * GameConstant.GEZI);


        //        IGREffectParticles eff = addEffect(
        //            GameConstant.LINK_EFFECT_ID,
        //            (float)GameTools.inst.pixelToUnit((double)(lx * GameConstant.GEZI)),
        //            z,
        //            (float)GameTools.inst.pixelToUnit((double)(ly * GameConstant.GEZI)),
        //            false
        //        );
        //        if (eff != null)
        //        {
        //            eff.loop = true;
        //            eff.play();
        //        }
        //    }
        //}

        static public GameObject GAME_CAMERA;



        static public Camera GAME_CAM_CAMERA;

        static public float M_FToCameraNearStep = 0.0f;
        static public Vector3 M_VGame_Cam_FARpos = new Vector3();
        static public Vector3 M_VGame_Cam_FARrot = new Vector3();
        static public GameObject GAME_CAM_CUR = new GameObject();
        //static public GameObject GAME_CAM_NEAR = new GameObject();

        static public int LEVEL_PLOT_ID = 0; //副本中的剧情ID

        //private uint m_nCurMapSceneSettingID;
        public int m_nCurMapID;
        //private Variant m_CurLocalConf;

        private AssetBundle m_curAssetBundlePlot = null;
        private AssetBundle m_curAssetBundleScene = null;
        //static private List<AudioClip> CUR_PLOT_SOUNDS = new List<AudioClip>();

        static private Action S_PLOT_PLAYOVER_CB = null;
        static public void SetPoltOver_EnterLevel(Action plot_over_callback)
        {
            S_PLOT_PLAYOVER_CB = plot_over_callback;
        }

        public void refreshLightMap()
        {
            //try
            //{
            //    (m_map as GRMap3D).refreshLightMap();
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogWarning(ex.ToString());
            //}

        }
        public static Variant curSvrMsg;
        public static Variant curSvrConf;
        //public static Variant curLocalConf;
        public static bool playingPlot = false;
        private static bool sdk_sendroleLogin = true;
        public static int changeMapTimeSt = 0;
        private void setChangeMapData(uint scene_setting_mapid, Variant svrConf, Variant localConf_TO_DEL)
        {
            changeMapTimeSt = NetClient.instance.CurServerTimeStamp;
            curSvrConf = svrConf;
            InterfaceMgr.doCommandByLua("MapModel:getInstance().getmapinfo", "model/MapModel", curSvrConf);
            debug.Log("C#1::::" + svrConf.dump());
            MediaClient.instance.StopSounds(); //清理所有的音效缓存
            GAMEAPI.ClearAllOneAsset();
            Resources.UnloadUnusedAssets();
            System.GC.Collect(0, System.GCCollectionMode.Forced);

            if (sdk_sendroleLogin)
            {
                sdk_sendroleLogin = false;
                //     LGPlatInfo.inst.logSDKAP("roleLogin");
            }

            MouseClickMgr.init();


            int local_mapid = svrConf["id"]._int;
            //m_nCurMapSceneSettingID = scene_setting_mapid;
            m_nCurMapID = local_mapid;

            bool isFb = false;
            int idx = -1;
            for (int i = 0; i < AutoPlayModel.getInstance().autoplayCfg4FB.Count; i++)
                if (AutoPlayModel.getInstance().autoplayCfg4FB[i].map.Contains(m_nCurMapID))
                {
                    isFb = true;
                    idx = i;
                    break;
                }
            if (isFb)
            {
                if (idx != -1)
                {
                    StateInit.Instance.Distance = AutoPlayModel.getInstance().autoplayCfg4FB[idx].Distance;
                    StateInit.Instance.PickDistance = AutoPlayModel.getInstance().autoplayCfg4FB[idx].DistancePick;
                }
            }
            else
            {
                StateInit.Instance.Distance = StateInit.Instance.DistanceNormal;
                StateInit.Instance.PickDistance = StateInit.Instance.PickDistanceNormal;
            }
            //m_CurLocalConf = localConf;
            InterfaceMgr.getInstance().closeAllWin();

            if (MapProxy.getInstance().openWin != null && MapProxy.getInstance().openWin != "")
            {
                if (MapProxy.getInstance().Win_uiData != null && MapProxy.getInstance().Win_uiData != "")
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(MapProxy.getInstance().Win_uiData);
                    InterfaceMgr.getInstance().ui_async_open(MapProxy.getInstance().openWin, arr);
                }
                else
                {
                    InterfaceMgr.getInstance().ui_async_open(MapProxy.getInstance().openWin);
                }
                MapProxy.getInstance().openWin = null;
                MapProxy.getInstance().Win_uiData = null;
            }
            //combo_txt.clear();


            //TaskModel.getInstance().isSubTask = false;
            LEVEL_PLOT_ID = 0;
            REV_RES_LIST_OK();
            REV_PLOT_PLAY_OVER();

        }

        static private void REV_CHARRES_LINKER_GO(GameObject[] linker, PLOT_CHARRES_TYPE res_type, string str_id, Dictionary<string, AnimationClip> anim_loop, string first_anim)
        {
            string res_url = "null";
            switch (res_type)
            {
                case PLOT_CHARRES_TYPE.PCRT_HERO: res_url = "QSMY/character/hero/" + str_id + "/" + str_id + ".res"; break;
                case PLOT_CHARRES_TYPE.PCRT_MONSTER: res_url = "QSMY/character/monster/" + str_id + "/" + str_id + ".res"; break;
                case PLOT_CHARRES_TYPE.PCRT_NPC: res_url = "QSMY/character/npc/" + str_id + "/" + str_id + ".res"; break;
                case PLOT_CHARRES_TYPE.PCRT_MOUNT: res_url = "QSMY/character/mount/" + str_id + "/" + str_id + ".res"; break;
                case PLOT_CHARRES_TYPE.PCRT_AVATAR: res_url = "QSMY/character/avatar/" + str_id + ".res"; break;
            }

            IAsset res_mesh = os.asset.getAsset<IAssetMesh>(res_url, (IAsset ast) =>
            {
                for (int i = 0; i < linker.Length; i++)
                {
                    GameObject one_link = linker[i];
                    if (one_link == null) continue;

                    GameObject res_obj = (ast as AssetMeshImpl).assetObj;
                    GameObject the_obj = GameObject.Instantiate(res_obj) as GameObject;
                    the_obj.transform.SetParent(one_link.transform, false);

                    //SkAniMeshBehaviour behaviour = the_obj.AddComponent<SkAniMeshBehaviour>();
                    //behaviour.obj = ast as SkAniMeshImpl;
                    //the_obj.AddComponent<Plot_SkAniBehaviour>();

                    Animation animation = the_obj.GetComponent<Animation>();
                    if (animation == null)
                    {
                        animation = the_obj.AddComponent<Animation>();
                    }

                    if (animation != null && anim_loop != null)
                    {
                        foreach (string key in anim_loop.Keys)
                        {
                            //anim_loop[key].wrapMode = WrapMode.Loop;
                            animation.AddClip(anim_loop[key], key);
                        }

                        //animation[first_anim].speed = 1.0f;
                        animation.Play(first_anim);
                        animation.wrapMode = WrapMode.Loop;
                    }
                }
            }, null,
            (IAsset ast, string err) =>
            {
                //加载失败
                debug.Log("加载剧情Res失败" + res_url);
            });

            (res_mesh as AssetImpl).loadImpl(false);
        }

        //处理剧情层的资源加载请求
        static public void REV_CHARRES_LINKER(GameObject[] linker, PLOT_CHARRES_TYPE res_type, int id, string[] anim_list)
        {
            //debug.Log("加载场景剧情模型");
            string str_id = id.ToString();
            int nanim_count = anim_list.Length;
            if (0 == nanim_count)
            {
                REV_CHARRES_LINKER_GO(linker, res_type, str_id, null, null);
                return;
            }

            Dictionary<string, AnimationClip> anim_loop = new Dictionary<string, AnimationClip>();
            //先加载相关的动作，最后加入模型
            string strfirst_anim = null;
            for (int i = 0; i < anim_list.Length; i++)
            {
                string curanim = anim_list[i];
                if (0 == i)
                {
                    strfirst_anim = curanim;
                }

                string anim_url = "null";
                switch (res_type)
                {
                    case PLOT_CHARRES_TYPE.PCRT_HERO: anim_url = "QSMY/character/hero/" + str_id + "/" + curanim; break;
                    case PLOT_CHARRES_TYPE.PCRT_MONSTER: anim_url = "QSMY/character/monster/" + str_id + "/" + curanim; break;
                    case PLOT_CHARRES_TYPE.PCRT_NPC: anim_url = "QSMY/character/npc/" + str_id + "/" + curanim; break;
                    case PLOT_CHARRES_TYPE.PCRT_MOUNT: anim_url = "QSMY/character/mount/" + str_id + "/" + curanim; break;
                        //case PLOT_CHARRES_TYPE.PCRT_AVATAR: anim_url = "QSMY/character/avatar/" + str_id + "/" + curanim; break;
                }

                IAsset sk_anim = os.asset.getAsset<IAssetSkAnimation>(anim_url, (IAsset ast) =>
                                {
                                    nanim_count--;
                                    anim_loop[curanim] = (ast as AssetSkAnimationImpl).anim;

                                    Debug.Log(nanim_count + "成功加载了动作 " + anim_url);
                                    if (0 == nanim_count)
                                    {
                                        REV_CHARRES_LINKER_GO(linker, res_type, str_id, anim_loop, strfirst_anim);
                                    }
                                },
                                null,
                                (IAsset ast, string err) =>
                                {
                                    //加载失败
                                    Debug.Log("加载动作失败 " + anim_url);
                                    nanim_count--;
                                });

                (sk_anim as AssetImpl).loadImpl(false);
            }
        }


        //处理剧情层的特效播放
        static public void REV_FXRES_LINKER(GameObject[] linker, string fx_file)
        {
            //debug.Log("----------------------------------- 加载剧情的特效 " + fx_file);
            IAsset res_mesh = os.asset.getAsset<IAssetParticles>(fx_file, (IAsset ast) =>
            {
                for (int i = 0; i < linker.Length; i++)
                {
                    GameObject one_link = linker[i];
                    GameObject res_obj = (ast as AssetParticlesImpl).assetObj;
                    GameObject the_obj = GameObject.Instantiate(res_obj) as GameObject;
                    the_obj.transform.SetParent(one_link.transform, false);

                    one_link.SetActive(false);
                }
            }, null,
            (IAsset ast, string err) =>
            {
                //加载失败
                debug.Log("加载特效失败" + fx_file);
            });

            (res_mesh as AssetImpl).loadImpl(false);
        }

        //static public void ClearWaitPlotSound()
        //{
        //    //CUR_PLOT_SOUNDS.Clear();
        //}

        //static public bool HasNoWaitPlotSound()
        //{
        //    bool allready = true;
        //    //debug.Log("音乐准备测试 " + CUR_PLOT_SOUNDS.Count);
        //    for (int i = 0; i < CUR_PLOT_SOUNDS.Count; i++)
        //    {
        //        if (false == CUR_PLOT_SOUNDS[i].isReadyToPlay)
        //        {
        //            //debug.Log("音乐没有准备好");
        //            allready = false;
        //            break;
        //        }
        //    }
        //    return allready;
        //}

        //处理剧情层的声音
        static public void REV_SOUNDRES_LINKER(GameObject linker, int id)
        {
            //debug.Log("加载剧情声音");
            //string str_id = id.ToString();

            //URLReqImpl urlreq_res = new URLReqImpl();
            //urlreq_res.dataFormat = "assetbundle";
            //urlreq_res.url = "media/plot/" + str_id + ".snd";
            //urlreq_res.load((IURLReq req, object ret) =>
            //{
            //    //debug.Log("成功挂接声音");
            //    AudioSource audio_src = linker.GetComponent<AudioSource>();
            //    if (audio_src == null)
            //        audio_src = linker.AddComponent<AudioSource>();

            //    AudioClip ac = ret as AudioClip;
            //    audio_src.clip = ac;

            //    CUR_PLOT_SOUNDS.Add(ac);


            //    //debug.Log("ac音乐准备测试 " + ac.isReadyToPlay);
            //},
            //null,
            //(IURLReq req, string err) =>
            //{
            //    //加载失败
            //    debug.Log("加载剧情声音失败 " + req.url);
            //});
        }

        //处理剧情层的字幕
        static public void REV_ZIMU_TEXT(string zimu)
        {
            //if (zimu != null)
            //    storydialog.show(zimu);
            //else
            //    storydialog.show("");
        }

        static public void REV_PLOT_UI(string plot_ui)
        {
            //plot_linkui.show(plot_ui);
        }


        private int m_nScene_Loaded_CallNextFrame = 0;
        private static bool initedDontDestory = false;

        static public void DontDestroyBaseGameObject()
        {
            if (!initedDontDestory)
            {
                UnityEngine.Object[] initsObjects = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (UnityEngine.Object go in initsObjects)
                {
                    Application.DontDestroyOnLoad(go);
                }

                initedDontDestory = true;
            }
        }

        public void REV_RES_LIST_OK()
        {
            if (GameRoomMgr.getInstance().curRoom != null)
                GameRoomMgr.getInstance().curRoom.clear();
            //MediaClient.instance.clearMusic();
            NpcMgr.instance.clear();
            MonsterMgr._inst.clear();
            OtherPlayerMgr._inst.clear();
            if (tragethead.instance != null)
                tragethead.instance.resetTragethead();

            
            //LoaderBehavior.ms_HasAllLoaded = false;

            //if (m_map == null)
            //{
            //    setGraphImpl(g_mgr.createGraphMap(m_nCurMapSceneSettingID.ToString()));
            //    //_map = m_mgr.createGraphMap( mapid ) as IGRMap;
            //}
            //  m_map.loadConfig(m_CurLocalConf);

            //  Application.LoadLevelAdditive("p" + 101);


            if (m_curAssetBundleScene != null)
            {
                m_curAssetBundleScene.Unload(false);
                m_curAssetBundleScene = null;
            }


            //Application.LoadLevel(curSvrConf["name"]._str);


            //URLReqImpl urlreq_localver = new URLReqImpl();
            //urlreq_localver.dataFormat = "assetbundle";
            //urlreq_localver.url = "scene/" + curLocalConf["Mesh"]._arr[0]["asset"]._arr[0]["file"] + ".unity";
            //urlreq_localver.load((IURLReq local_r, object local_ret) =>
            //{
            //    //debug.Log("加载A3地图成功:" + local_r.url);
            //    //m_curAssetBundleScene = local_ret as AssetBundle;
            //    //if (initedDontDestory == false)
            //    //{
            //    //    UnityEngine.Object[] initsObjects = GameObject.FindObjectsOfType(typeof(GameObject));
            //    //    foreach (UnityEngine.Object go in initsObjects)
            //    //    {
            //    //        Application.DontDestroyOnLoad(go);
            //    //    }
            //    //    initedDontDestory = true;
            //    //}

            //    //Application.LoadLevel(m_nCurMapID.ToString());
            //},
            //  null,
            //  (IURLReq local_r, string err) =>
            //  {
            //    debug.Log("加载A3地图失败:" + err);
            //MediaClient.instance.clearMusic();
            DontDestroyBaseGameObject();
            GameEventTrigger.clear();
            debug.Log("开始加载地图");

            SceneFXMgr.StartLoadScene(curSvrConf["name"]._str);

            //SceneManager.LoadScene(curSvrConf["name"]._str);
            //GAMEAPI.Unload_Asset(curSvrConf["name"] + ".assetbundle");

            //if (debug.instance != null)
            //{
            //    debug.instance.LoadScene(curSvrConf["name"]._str);
            //}

            if (BaseRoomItem.instance != null)
                BaseRoomItem.instance.clear();

            if (a3_RollItem.single != null)
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ROLL_ITEM);

            //   });
            //if (login.instance == null)
            //    refreshLightMap();

            //m_nCurMapSceneSettingID = 0;
            //m_CurLocalConf = null;
        }

        void StartGame(Scene scene, LoadSceneMode sceneMode)
        {
            m_nScene_Loaded_CallNextFrame = 2;
            LGUIMainUIImpl_NEED_REMOVE.CHECK_MAPLOADING_LAYER = true;
        }


        List<int> showid = new List<int>();
        bool fristloaded = true;
        private void a3_scene_loaded()
        {
            MapProxy.getInstance().changingMap = false;

            if (!grmap_loading)
                return;

            //   PlayerNameUIMgr.getInstance().Clear();

            //清空音效
            //MediaClient.instance.clearMusic();

            SceneCamera.Init();
            if (SelfRole._inst != null)
                SelfRole._inst.dispose();

            debug.Log("初始化角色");
            SelfRole.Init();

            SelfRole._inst.m_unIID = PlayerModel.getInstance().iid;
            SelfRole._inst.m_curModel.position = PlayerModel.getInstance().enter_map_pos;
            SelfRole._inst.setNavLay(NavmeshUtils.listARE[1]);
            //debug.Log("地图加载后的坐标为" + SelfRole._inst.m_curModel.position);


            Time.fixedDeltaTime = 0.02f;

            GameObject svr = GameObject.Find("SVR_DATA");
            if (svr != null)
                GameObject.Destroy(svr);

            //主城添加一个z轴的18高度，防止初始化判断在阻挡外面。加了高度后虽然在阻挡外面，但hit返回的xy值正常，z值调整为正常
            float z = 0f;
            if (curSvrConf["id"] == 10 || GRMap.curSvrConf["id"] == 24)
                z = 18f;
            SelfRole._inst.setPos(new Vector3(PlayerModel.getInstance().mapBeginX, z, PlayerModel.getInstance().mapBeginY));

            if (PlayerModel.getInstance().mapBeginroatate > 0f)
            {
                SelfRole._inst.setRoleRoatate(PlayerModel.getInstance().mapBeginroatate);
                PlayerModel.getInstance().mapBeginroatate = 0f;
            }

            DoAfterMgr.instacne.addAfterRender(() =>
            {
                if (MapModel.getInstance().CheckAutoPlay())
                    SelfRole.fsm.StartAutofight();
            });


            GameRoomMgr.getInstance().onChangeLevel(curSvrConf, curSvrMsg);
            grmap_loading = false;

            if (!fristloaded)
            {
                if (showid.Contains(curSvrConf["id"]))
                {
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMAPUI);
                }
                else
                {
                    if (curSvrConf.ContainsKey("pk_hint") && curSvrConf["pk_hint"] == 1)
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_PKMAPUI);
                        showid.Add(curSvrConf["id"]);
                    }
                    else
                    {
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMAPUI);
                    }
                }
            }
            //打开魔炼之地
            if (a3_fb_finish.ismlzd)
            {
                ArrayList dl = new ArrayList();
                dl.Add("mlzd");
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                a3_fb_finish.ismlzd = false;

            }
            if (curSvrConf.ContainsKey("pk_lock"))
            {
                InterfaceMgr.doCommandByLua("a1_low_fightgame.refreskCanPk", "ui/interfaces/low/a1_low_fightgame", false);
            }
            else
            {
                InterfaceMgr.doCommandByLua("a1_low_fightgame.refreskCanPk", "ui/interfaces/low/a1_low_fightgame", true);
            }

            if (a1_gamejoy.inst_skillbar != null)
            {
                if (GRMap.curSvrConf["id"] == 10 || GRMap.curSvrConf["id"] == 24)
                    a1_gamejoy.inst_skillbar.ShowCombatUI(false);
                else
                    a1_gamejoy.inst_skillbar.ShowCombatUI(true);
            }

            if (a3_liteMinimap.instance)
                a3_liteMinimap.instance.refreshMapname();
            InterfaceMgr.doCommandByLua("a1_high_fightgame.refreshMapname", "ui/interfaces/high/a1_high_fightgame", null);

            if (a1_gamejoy.inst_skillbar != null)
                a1_gamejoy.inst_skillbar.refreshAllSkills(SelfRole.s_bStandaloneScene ? 0 : -1);

            if (curSvrConf.ContainsKey("music"))
                MediaClient.instance.PlayMusicUrl("audio_map_" + curSvrConf["music"], null, true);

            fristloaded = false;

            OtherPlayerMgr._inst.onMapLoaded();
            MonsterMgr._inst.onMapLoaded();
            NpcMgr.instance.onMapLoaded();
            BattleProxy.getInstance().onMapLoaded();
        }



        private void doLogin()
        {
            login.instance.onBeginLoading(() =>
            {
                DoPlotPlayOver();
                //     LGPlatInfo.inst.logSDKCustomAP("load");
            });
        }


        private void DoPlotPlayOver()
        {
            //      (m_map as GRMap3D).SetMapFogAndAnimbent(); //这里赋值场景的雾效和全局光

            Globle.setTimeScale(1f); //重新设置当前的播放速度

            //这里要做还原，去除场景中不要的物件，或者上层剧情脚本自己去除
            //GRClient.instance.getGraphCamera().visible = true;
            //if (lgSelfPlayer.instance != null && lgSelfPlayer.instance.lggrAvatar != null)
            //    lgSelfPlayer.instance.lggrAvatar.updateToTerrainZ();

            //剧情高级字幕效果的挂接点
            //plot_linkui.ClearAll();
            //InterfaceMgr.getInstance().delclose(InterfaceMgr.PLOT_LINKUI);

            //Dictionary<uint, LGAvatarMonster> mons = LGMonsters.instacne.getMons();
            //foreach (LGAvatarMonster m in mons.Values)
            //{
            //    m.grAvatar.m_char.visible = true;
            //}
            //Vector3 oldpos = GAME_CAMERA.transform.position;
            //  GameObject.Destroy(GAME_CAMERA);

            // debug.Log("加载当前地图的摄像机 " + m_nCurMapID);

            // string game_cam_id = null;
            //     GameObject game_cam_prefab = null;
            //   Variant map_gconf = this.g_mgr.g_sceneM.getGMapInfo(m_nCurMapID.ToString());
            //debug.Log(map_gconf.dump());
            //if (map_gconf != null && map_gconf.ContainsKey("gamecamera"))
            //{
            //    game_cam_id = map_gconf["gamecamera"];
            //    string curmap_game_cam = "GAME_CAMERA_" + game_cam_id.ToString();
            //    game_cam_prefab = U3DAPI.U3DResLoad<GameObject>(curmap_game_cam);
            //}

            //if (game_cam_prefab == null)
            //{
            //    game_cam_prefab = U3DAPI.U3DResLoad<GameObject>("GAME_CAMERA");
            //}

            // GAME_CAMERA = GameObject.Instantiate(game_cam_prefab) as GameObject;


            //GAME_CAM_CAMERA = GAME_CAMERA.transform.GetChild(0).GetComponent<Camera>();
            //GAME_CAM_CUR = GAME_CAMERA.transform.GetChild(0).gameObject;

            //if (GAME_CAMERA.transform.childCount >= 2) GAME_CAM_NEAR = GAME_CAMERA.transform.GetChild(1).gameObject;

            M_VGame_Cam_FARpos = GAME_CAM_CUR.transform.localPosition;
            M_VGame_Cam_FARrot = GAME_CAM_CUR.transform.localEulerAngles;

            //     GameRoomMgr.getInstance().onChangeLevel();
            UiEventCenter.getInstance().onMapChanged();
            LGMap lgmap = GRClient.instance.g_gameM.getObject(OBJECT_NAME.LG_MAP) as LGMap;
            lgmap.playMapMusic(disconect.needResetMusic);
            disconect.needResetMusic = false;


            //if (skillbar.instance != null)
            //    skillbar.setAutoFightType(0);

            //if (lgSelfPlayer.instance != null && lgSelfPlayer.instance.lggrAvatar != null)
            //{
            //    Vector3 my_newmap_pos = lgSelfPlayer.instance.pGameobject.transform.position;
            //    my_newmap_pos.x = lgSelfPlayer.instance.lggrAvatar.m_xMoveto;
            //    my_newmap_pos.y = lgSelfPlayer.instance.lggrAvatar.m_zMoveto;
            //    my_newmap_pos.z = lgSelfPlayer.instance.lggrAvatar.m_yMoveto;

            //    lgSelfPlayer.instance.pGameobject.transform.position = my_newmap_pos;

            //    (GRClient.instance.g_gameM.getObject(OBJECT_NAME.LG_CAMERA) as LGCamera).updateMainPlayerPos(true);
            //}
        }

        static public Action CUR_POLTOVER_CB = null;
        private void REV_PLOT_PLAY_OVER()
        {
            playingPlot = false;
            //请求处理剧情播放完的指令
            if (S_PLOT_PLAYOVER_CB != null)
            {
                if (login.instance)
                    CUR_POLTOVER_CB = doLogin;
                else
                    CUR_POLTOVER_CB = DoPlotPlayOver;
                S_PLOT_PLAYOVER_CB();
                S_PLOT_PLAYOVER_CB = null;
            }
            else
            {
                if (login.instance)
                    doLogin();
                else
                    DoPlotPlayOver();
            }
        }



        private void onChangeMap(GameEvent e)
        {
            deleteEffects();
            //if (m_map == null)
            //{
            //    //DebugTrace.print( "m_map null Err!");
            //    return;
            //}            
            if (changeMapData != null && loading_cloud.instance != null)
                return;

            debug.Log("onChangeMap:e.data::::::" + e.data);

            changeMapData = e.data;

            if (loading_cloud.instance == null)
            {
                loading_cloud.showhandle = doChangeMap;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.LOADING_CLOUD);
            }
            else
            {
                doChangeMap();
            }
        }



        Variant changeMapData;

        private Variant Cache_MapChangeData = null;

        private void doChangeMap()
        {
            lgSelfPlayer.instance.doSkillPreload();
            GameCameraMgr.getInstance().clearCurCamera();
            //m_map.dispose();
            //m_map = null;
            grmap_loading = true;

            if (changeMapData != null)
                debug.Log(changeMapData.dump());

            Cache_MapChangeData = changeMapData;
            string an = Cache_MapChangeData["conf"]["name"];
            GAMEAPI.LoadAsset_Async(an + ".assetbundle", an);
            //Debug.Log("开始加载场景，准备进入游戏---------------------------------------------------------");
            //setChangeMapData(changeMapData["mapid"]._uint, changeMapData["conf"], changeMapData["localConf"]);

            //addLinkEffs(changeMapData["tmpLinks"]);
            //addLinkEffs(changeMapData["conf"]["l"]);

            //m_ctrl.dispatchEvent(GameEvent.Create(GAME_EVENT.MAP_LOAD_READY, null, null));
            changeMapData = null;
        }

        private void onViewSizeChange(GameEvent e)
        {
            Variant data = e.data;
            //_map.onViewSizeChange( data.w, data.h );
        }

        private void onLinkAdd(GameEvent e)
        {
            Variant data = e.data;
            //  addLinkEffs(data);

        }
        //data { x:, y: }
        private void upDateView(GameEvent e)
        {
            //var data:Object = e.data;
            //_map.upDateView( data );
        }
        override public void dispose()
        {
            //this.m_mgr.deleteEntity( this.m_gr );
        }

        private void onAddEff(GameEvent e)
        {
            Variant d = e.data;
            string effid = d["effid"]._str;
            float lx = d["x"]._float;
            float ly = d["y"]._float;
            bool loop = false;
            float angle = 0;
            if (d.ContainsKey("angle"))
            {
                angle = d["angle"]._float;
            }
            if (d.ContainsKey("loop"))
            {
                loop = d["loop"]._bool;
            }

            float z = (float)this.g_mgr.getZ(lx, ly);

            IGREffectParticles eff = addEffect(
                effid,
                (float)GameTools.inst.pixelToUnit(lx),
                z,
                (float)GameTools.inst.pixelToUnit(ly),
                false
            );
            if (eff != null)
            {
                if (angle != 0)
                    eff.rotY = (float)(angle * 180 / Math.PI);

                eff.loop = loop;
                eff.play();
            }
        }

        private void onAddFlyEff(GameEvent e)
        {
            Variant d = e.data;
            if (!d.ContainsKey("fly_eff"))
            {
                return;
            }
            Vec2 effPoint = d["effPoint"]._val as Vec2;
            string effid = d["fly_eff"]._str;
            float lx = effPoint.x;
            float ly = effPoint.y;
            bool loop = false;
            float angle = 0;
            if (d.ContainsKey("angle"))
            {
                angle = d["angle"]._float;
            }
            if (d.ContainsKey("loop"))
            {
                loop = d["loop"]._bool;
            }

            float z = (float)this.g_mgr.getZ(lx, ly);

            IGREffectParticles eff = addEffect(
                effid,
                (float)GameTools.inst.pixelToUnit(lx),
                z,
                (float)GameTools.inst.pixelToUnit(ly),
                false
            );
            if (eff != null)
            {
                if (angle != 0)
                    eff.rotY = (float)(angle * 180 / Math.PI);

                eff.loop = loop;
                eff.play();
            }
            Variant flyData = d.clone();
            flyData["fly_eff"]._val = eff;

            fly_array._arr.Add(flyData);
        }

        public override void updateProcess(float tmSlice)
        {
            if (Cache_MapChangeData != null)
            {
                //只要场景加载好了，就可以进游戏了
                if (GAMEAPI.Res_Async_Loaded())
                {
                    AnyPlotformSDK.LoadAB_Res();
                    //Debug.Log("加载完毕，进入游戏---------------------------------------------------------");

                    setChangeMapData(Cache_MapChangeData["mapid"]._uint, Cache_MapChangeData["conf"], Cache_MapChangeData["localConf"]);
                    Cache_MapChangeData = null;
                }
            }

            if (m_nScene_Loaded_CallNextFrame > 0)
            {
                m_nScene_Loaded_CallNextFrame--;
                if (0 == m_nScene_Loaded_CallNextFrame)
                {//场景加载结束
                    a3_scene_loaded();
                }
            }


            if (GAME_CAMERA == null)
            {
                GAME_CAMERA = GameObject.Find("GAME_CAMERA");
                if (GAME_CAMERA == null)
                    return;
                GAME_CAM_CUR = GAME_CAMERA.transform.GetChild(0).gameObject;
                GAME_CAM_CAMERA = GAME_CAM_CUR.GetComponent<Camera>();
                LGCamera.instance.updateMainPlayerPos(true);

                LGNpcs.instance.onMapchg();
            }

            //if (fly_array.Count <= 0)
            //    return;

            //float cur_tm = this.g_mgr.g_netM.CurServerTimeStampMS;
            //for (int i = 0; i < fly_array.Count; i++)
            //{
            //    Variant fly = fly_array[i];
            //    if (fly == null)
            //        continue;
            //    float passtm = cur_tm - fly["lasttm"]._float;
            //    IGREffectParticles fly_eff = fly["fly_eff"]._val as IGREffectParticles;
            //    float flytm = fly["flytm"]._float;
            //    if (passtm >= flytm)
            //    {

            //        //到了
            //        //停止飞行特效，播放end特效
            //        if (fly_eff != null)
            //            fly_eff.stop();

            //        if (lgMainPlayer.can_play_skill_effect(0))
            //        {
            //            if (fly.ContainsKey("to_lc") && fly["end_eff"] != null && fly["to_lc"]._val != null)
            //            {
            //                LGAvatarGameInst to_lc = fly["to_lc"]._val as LGAvatarGameInst;
            //                this.dispatchEvent(
            //                    GameEvent.Create(GAME_EVENT.MAP_ADD_SHOW_EFF, this,
            //                    GameTools.createGroup("effid", fly["end_eff"]._str, "x", to_lc.x, "y", to_lc.y))
            //                );
            //            }
            //            else if (fly.ContainsKey("tar_pos") && fly["end_eff"] != null)
            //            {
            //                this.dispatchEvent(
            //                    GameEvent.Create(GAME_EVENT.MAP_ADD_SHOW_EFF, this,
            //                    GameTools.createGroup("effid", fly["end_eff"]._str, "x", (fly["tar_pos"]._val as Vec2).x, "y", (fly["tar_pos"]._val as Vec2).y))
            //                );
            //            }
            //        }
            //        fly_array._arr.RemoveAt(i);
            //        i--;
            //    }
            //    else
            //    {	//没到
            //        //更新飞行特效位置，（和方向）
            //        if (fly_eff == null)
            //            continue;
            //        float f_x = 0;
            //        float f_y = 0;

            //        if (fly.ContainsKey("to_lc"))
            //        {
            //            LGAvatarGameInst to_lc = fly["to_lc"]._val as LGAvatarGameInst;
            //            if (to_lc != null)
            //            {
            //                f_x = to_lc.x;
            //                f_y = to_lc.y;
            //            }
            //        }
            //        else if (fly.ContainsKey("tar_pos"))
            //        {
            //            Vec2 tar_pos = fly["tar_pos"]._val as Vec2;
            //            f_x = tar_pos.x;
            //            f_y = tar_pos.y;
            //        }

            //        float lx = (f_x - fly_eff.x) * (passtm / flytm);
            //        float ly = (f_y - fly_eff.y) * (passtm / flytm);
            //        fly_eff.x += lx;
            //        fly_eff.y += ly;
            //        fly["lasttm"] = cur_tm;
            //        fly["flytm"] -= passtm;
            //    }
            //}
        }


        private lgSelfPlayer lgMainPlayer
        {
            get
            {
                return (this.g_mgr.g_gameM as muLGClient).getObject(OBJECT_NAME.LG_MAIN_PLAY) as lgSelfPlayer;
            }
        }

    }
}
