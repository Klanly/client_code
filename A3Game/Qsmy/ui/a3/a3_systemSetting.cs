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

namespace MuGame
{
    class a3_systemSetting : Window
    {
        private TabControl tab;
        Transform con;


        BaseSystemSetting m_current;
        BaseSystemSetting m_system;
        BaseSystemSetting m_game;
        BaseSystemSetting m_giftBag;

        //panels
        Transform tfSystemPanel;
        Transform tfGamePanel;
        Transform tfGiftBagPanel;

        override public void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("main/btns/btnSys/Text").text = ContMgr.getCont("a3_systemSetting_0");
            getComponentByPath<Text>("main/btns/btnGame/Text").text = ContMgr.getCont("a3_systemSetting_1");
            getComponentByPath<Text>("main/btns/btnGiftBag/Text").text = ContMgr.getCont("a3_systemSetting_2");
            getComponentByPath<Text>("main/btns/btnsetting/Text").text = ContMgr.getCont("a3_systemSetting_3");
            getComponentByPath<Text>("main/btnQuitGame/Text").text = ContMgr.getCont("a3_systemSetting_4");
            getComponentByPath<Text>("main/btnOutGame/Text").text = ContMgr.getCont("a3_systemSetting_5");
            getComponentByPath<Text>("main/panels/systemPanel/music/Text").text = ContMgr.getCont("a3_systemSetting_6");
            getComponentByPath<Text>("main/panels/systemPanel/soundEffect/Text").text = ContMgr.getCont("a3_systemSetting_7");
            getComponentByPath<Text>("main/panels/systemPanel/Resolution/Text").text = ContMgr.getCont("a3_systemSetting_8");
            getComponentByPath<Text>("main/panels/systemPanel/Resolution/toggles/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_9");
            getComponentByPath<Text>("main/panels/systemPanel/Resolution/toggles/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_10");
            getComponentByPath<Text>("main/panels/systemPanel/Resolution/toggles/Toggle2/Label").text = ContMgr.getCont("a3_systemSetting_11");
            getComponentByPath<Text>("main/panels/systemPanel/skillDetail/Text").text = ContMgr.getCont("a3_systemSetting_12");
            getComponentByPath<Text>("main/panels/systemPanel/skillDetail/toggles/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_13");
            getComponentByPath<Text>("main/panels/systemPanel/skillDetail/toggles/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_14");
            getComponentByPath<Text>("main/panels/systemPanel/modelDetail/Text").text = ContMgr.getCont("a3_systemSetting_15");
            getComponentByPath<Text>("main/panels/systemPanel/modelDetail/toggles/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_13");
            getComponentByPath<Text>("main/panels/systemPanel/modelDetail/toggles/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_14");
            getComponentByPath<Text>("main/panels/systemPanel/FPSlimit/Text").text = ContMgr.getCont("a3_systemSetting_16");
            getComponentByPath<Text>("main/panels/systemPanel/roleShadow/Text").text = ContMgr.getCont("a3_systemSetting_17");
            getComponentByPath<Text>("main/panels/systemPanel/roleShadow/toggles/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_18");
            getComponentByPath<Text>("main/panels/systemPanel/roleShadow/toggles/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_19");
            getComponentByPath<Text>("main/panels/systemPanel/sceneDetail/Text").text = ContMgr.getCont("a3_systemSetting_20");
            getComponentByPath<Text>("main/panels/systemPanel/sceneDetail/toggles/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_9");
            getComponentByPath<Text>("main/panels/systemPanel/sceneDetail/toggles/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_10");
            getComponentByPath<Text>("main/panels/systemPanel/sceneDetail/toggles/Toggle2/Label").text = ContMgr.getCont("a3_systemSetting_11");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle/Label").text = ContMgr.getCont("a3_systemSetting_21");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle1/Label").text = ContMgr.getCont("a3_systemSetting_22");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle2/Label").text = ContMgr.getCont("a3_systemSetting_23");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle3/Label").text = ContMgr.getCont("a3_systemSetting_24");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle4/Label").text = ContMgr.getCont("a3_systemSetting_25");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle5/Label").text = ContMgr.getCont("a3_systemSetting_26");
            getComponentByPath<Text>("main/panels/gamePanel/Toggle6/Label").text = ContMgr.getCont("a3_systemSetting_27");
            getComponentByPath<Text>("main/panels/giftBagPanel/hint").text = ContMgr.getCont("a3_systemSetting_28");
            getComponentByPath<Text>("main/panels/giftBagPanel/hint/Text").text = ContMgr.getCont("a3_systemSetting_29");
            getComponentByPath<Text>("main/panels/giftBagPanel/hint/Text1").text = ContMgr.getCont("a3_systemSetting_30");
            getComponentByPath<Text>("main/panels/giftBagPanel/Text").text = ContMgr.getCont("a3_systemSetting_31");
            getComponentByPath<Text>("main/panels/giftBagPanel/but_convert/Text").text = ContMgr.getCont("a3_systemSetting_32");
            getComponentByPath<Text>("main/btnBackRoleList/Text").text = ContMgr.getCont("a3_systemSetting_33");
            getComponentByPath<Text>("main/btnBackLogin/Text").text = ContMgr.getCont("a3_systemSetting_34");
            #endregion

            con = transform.FindChild("main/panels");
            //panels
            tfSystemPanel = transform.FindChild("main/panels/systemPanel");
            tfGamePanel = transform.FindChild("main/panels/gamePanel");
            tfGiftBagPanel = transform.FindChild("main/panels/giftBagPanel");

            //if (Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    transform.FindChild("main/btns/btnGiftBag").gameObject.SetActive(false);
            //}

            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                transform.FindChild("main/btns/btnGiftBag").gameObject.SetActive(A3_HallowsModel.type_duihuan == 0 ? false : true);
            }
             


            tab = new TabControl();
            tab.onClickHanle = OnSwitch;
            tab.create(getGameObjectByPath("main/btns"), this.gameObject);
            new BaseButton(transform.FindChild("title/btnClose")).onClick = onBtnClose;
            new BaseButton(transform.FindChild("main/btnBackRoleList")).onClick = onBtnBackRoleList;
            new BaseButton(transform.FindChild("main/btnBackLogin")).onClick = onBtnBackLogin;
            new BaseButton(transform.FindChild("main/btnQuitGame")).onClick = onBtnQuitGame;
            new BaseButton(transform.FindChild("main/btnOutGame")).onClick = onBtnOutGame;
        }
        void  onBtnBackRoleList(GameObject go)
        {
            MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("a3_systemSetting0"), backRoleListHandle);
        }
        void backRoleListHandle()
        {

        }
        void onBtnBackLogin(GameObject go)
        {
            MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("a3_systemSetting1"), backLoginHandle);

        }
        void backLoginHandle()
        {
          
        }
        void onBtnQuitGame(GameObject go)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                AnyPlotformSDK.Call_Cmd("close");
            }
            else
            {
                GameSdkMgr.record_quit();
            }
        }
        void onBtnOutGame(GameObject go)
        {
            AnyPlotformSDK.Call_Cmd("loginout");
        }

        void quitGameHandle()
        {
            AnyPlotformSDK.Call_Cmd("close");
        }

        void onBtnClose(GameObject go)
        {
            //设置系统数据配置之后关闭当前UI
            SystemPanel sp= SystemPanel.mInstance.GetValue();
            //SceneCamera.SetGameScreenPow(sp.videoQualityValue);
            SceneCamera.SetGameLight((int)sp.roleShadowType);
            SceneCamera.SetGameShadow((int)sp.roleShadowType);
            SceneCamera.SetGameScene((int)sp.sceneDetailType);
            SceneCamera.SetSikillEff((int)sp.skillDetailType);
            //SceneCamera.SetResolution((int)sp.resolutionType);
            SceneCamera.SetFPSLimit((int)sp.fpsLimitType);
            SceneCamera.SetModelDetail((int)sp.modelDetailType);


            PlayeLocalInfo.saveInt(PlayeLocalInfo.SYS_MUSIC, sp.musicValue);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.SYS_SOUND, sp.musicEffect);
            //PlayeLocalInfo.saveString(PlayeLocalInfo.VIDEO_QUALITY, sp.videoQualityValue.ToString());
            PlayeLocalInfo.saveInt(PlayeLocalInfo.SKILL_EFFECT, (int)sp.skillDetailType);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.FPS_LIMIT, (int)sp.fpsLimitType);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.ROLE_SHADOW, (int)sp.roleShadowType);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.SCENE_DETAIL, (int)sp.sceneDetailType);
            PlayeLocalInfo.saveInt(PlayeLocalInfo.MODEL_DETAIL, (int)sp.modelDetailType);
            //PlayeLocalInfo.saveInt(PlayeLocalInfo.SCREEN_RESOLUTION, (int)sp.resolutionType);
            //gameSetting
            if (GamePanel.mInstance!=null)
            {
                GamePanel gp = GamePanel.mInstance.GetValue();
                GlobleSetting.REFUSE_TEAM_INVITE = gp.refuseTeamInvite;
                GlobleSetting.IGNORE_PRIVATE_INFO = gp.ignorePrivateInfo;
                GlobleSetting.IGNORE_KNIGHTAGE_INVITE = gp.ignorePaladinInvite;
                GlobleSetting.IGNORE_FRIEND_ADD_REMINDER = gp.ignoreAddFirendHint;
                GlobleSetting.IGNORE_OTHER_EFFECT = gp.ignoreOtherEffect;
                GlobleSetting.IGNORE_OTHER_PLAYER = gp.ignoreOther;
                GlobleSetting.IGNORE_OTHER_PET = gp.ignoreOtherPet;

                PlayeLocalInfo.saveInt(PlayeLocalInfo.REFUSE_TEAM_INVITE, GlobleSetting.REFUSE_TEAM_INVITE ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_PRIVATE_INFO, GlobleSetting.IGNORE_PRIVATE_INFO ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_KNIGHTAGE_INVITE, GlobleSetting.IGNORE_KNIGHTAGE_INVITE ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_FRIEND_ADD_REMINDER, GlobleSetting.IGNORE_FRIEND_ADD_REMINDER ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_OTHER_EFFECT, GlobleSetting.IGNORE_OTHER_EFFECT ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_OTHER_PLAYER, GlobleSetting.IGNORE_OTHER_PLAYER ? 1 : 0);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.IGNORE_OTHER_PET, GlobleSetting.IGNORE_OTHER_PET ? 1 : 0);
            }
           

            //PlayerPrefs.DeleteAll();//test default value
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SYSTEM_SETTING); 
        }

        override public void onShowed()
        {
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.zero;
            if (m_current != null)
            {
                m_current.onShowed();
            }
            else
            {
                tab.setSelectedIndex(0);
                OnSwitch(tab);
            }
           // GRMap.GAME_CAMERA.SetActive(false);
        }

        override public void onClosed()
        {
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.one;

            if (m_current != null)
            {
                m_current.onClose();
            }
            //  GRMap.GAME_CAMERA.SetActive(true);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY2);
        }
        void OnSwitch(TabControl tb)
        {
            int index = tb.getSeletedIndex();
            if (m_current!=null) { m_current.onClose(); m_current.gameObject.SetActive(false); }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY2);
            switch (index)
            {

                case 0:
                    ShowSystemPanel();
                    break;
                case 1:
                    ShowGamePanel();
                    break;
                case 2:
                    ShowGiftBagPanel();
                    //ShowSetting();
                    break;
                case 3:
                    ShowSetting();
                    break;
                default:
                    break;
            }
            if (m_current != null)
            {
                m_current.onShowed();
                m_current.visiable = true;
            }
        }
        #region showPanel
       
        void ShowSystemPanel()
        {
            if (m_system==null)
            {
                m_system = new SystemPanel(tfSystemPanel);
                m_system.setPerent(con);
            }
            m_current = m_system;
        }
        void ShowGamePanel()
        {
            if (m_game == null)
            {
                m_game = new GamePanel(tfGamePanel);
                m_game.setPerent(con);
            }
            m_current = m_game;
        }
        void ShowGiftBagPanel()
        {
            if (m_giftBag == null)
            {
                m_giftBag = new GifBagPane(tfGiftBagPanel);
                m_giftBag.setPerent(con);
            }
            m_current = m_giftBag;
        }
        void ShowSetting()
        {
            tfSystemPanel.gameObject.SetActive(false);
            tfGamePanel.gameObject.SetActive(false);
            tfGiftBagPanel.gameObject.SetActive(false);
            if (m_game != null)
            {
                m_game = null;
            }
            m_current = null;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUTOPLAY2);
        }

        #endregion
        #region  SystemPanel
        class SystemPanel:BaseSystemSetting
        {
            public static SystemPanel mInstance;
            Slider scrbMusic;
            Slider scrbMusicEffect;
            //Transform tfVideoQualit;

            //Transform tfResolutionToggles;
            Transform tfFPSlimitToggles;
            Transform tfRoleShadowToggles;
            Transform tfSceneDetailToggles;
            Transform tfSkillDetailToggles;
            Transform tfModelDetailToggles;

            public int musicValue;
            public int musicEffect;
            //public float videoQualityValue;
            //public SystemItemConfigLvL resolutionType = SystemItemConfigLvL.high;
            public SystemItemConfigLvL fpsLimitType = SystemItemConfigLvL.high;
            public SystemItemConfigLvL roleShadowType = SystemItemConfigLvL.off;
            public SystemItemConfigLvL sceneDetailType = SystemItemConfigLvL.high;
            public SystemItemConfigLvL skillDetailType = SystemItemConfigLvL.off;
            public SystemItemConfigLvL modelDetailType = SystemItemConfigLvL.on;

            Text txtMusicValue;
            Text txtMusicEffectValue;

            //List<Toggle> videoQualityValueToggles;
            public SystemPanel(Transform trans)
                : base(trans)
            {
                //videoQualityValueToggles = new List<Toggle>();
                mInstance             = this;
                scrbMusic = getGameObjectByPath("music/Slider").GetComponent<Slider>();
                txtMusicValue = scrbMusic.transform.FindChild("txtValue").GetComponent<Text>();
                scrbMusicEffect = getGameObjectByPath("soundEffect/Slider").GetComponent<Slider>();
                txtMusicEffectValue = scrbMusicEffect.transform.FindChild("txtValue").GetComponent<Text>();
                //tfVideoQualit = getGameObjectByPath("videoQuality/toggles").transform;
                //tfResolutionToggles  = getGameObjectByPath("Resolution/toggles").transform;
                tfModelDetailToggles = getGameObjectByPath("modelDetail/toggles").transform;
                tfFPSlimitToggles = getGameObjectByPath("FPSlimit/toggles").transform;
                tfRoleShadowToggles   = getGameObjectByPath("roleShadow/toggles").transform;
                tfSceneDetailToggles  = getGameObjectByPath("sceneDetail/toggles").transform;
                tfSkillDetailToggles = getGameObjectByPath("skillDetail/toggles").transform;
                scrbMusic.onValueChanged.AddListener(onScrbMusicValueChange);
                scrbMusicEffect.onValueChanged.AddListener(onScrbMusicEffectValueChange);
                //for (int i = 0; i < tfVideoQualit.childCount;i++ )
                //{
                //   Toggle tg= tfVideoQualit.GetChild(i).GetComponent<Toggle>();
                //   videoQualityValueToggles.Add(tg);
                //}
             
            }
            override public void onShowed()
            {
                initData();
            }
           void initData()
           {
                scrbMusic.value           = MediaClient.instance.getMusicVolume();
                txtMusicValue.text        = ((int)(scrbMusic.value * 100)).ToString();
                scrbMusicEffect.value     = MediaClient.instance.getSoundVolume();
                txtMusicEffectValue.text  = ((int)(scrbMusicEffect.value * 100)).ToString();
                //int fScreenGQ_Level = (int)(SceneCamera.m_fScreenGQ_Level*10);

                //int screen_lv = SceneCamera.m_nScreenResolution_Level;
                int fps_limit = SceneCamera.m_nFPSLimit_Level;//动态光影
                int roleShadow   = SceneCamera.m_nShadowAndLightGQ_Level;//人物影子
                int sceneDetail  = SceneCamera.m_nSceneGQ_Level;//场景的丰富程度
                int skillDetail = SceneCamera.m_nSkillEff_Level;
                int modelDetail = SceneCamera.m_nModelDetail_Level;

                //for (int i = 1; i <= tfResolutionToggles.childCount; i++)
                //{
                //    if (screen_lv == i)
                //    {
                //        tfResolutionToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = true;
                //    }
                //    else
                //    {
                //        tfResolutionToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = false;
                //    }
                //}
                //switch (fScreenGQ_Level)
                //{
                //    case 0:
                //        videoQualityValueToggles[0].isOn = true;
                //        break;
                //    case 5:
                //        videoQualityValueToggles[1].isOn = true;
                //        break;
                //    case 10:
                //        videoQualityValueToggles[2].isOn = true;
                //        break;
                //    default:
                //        break;
                //}
                for (int i = 1; i <= tfFPSlimitToggles.childCount; i++)
                {
                    if (fps_limit == i)
                    {
                        tfFPSlimitToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        tfFPSlimitToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = false;

                    }
                }
                for (int i = 1; i <= tfRoleShadowToggles.childCount; i++)
                {
                    if (roleShadow == i)
                    {
                        tfRoleShadowToggles.GetChild(i-1).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        tfRoleShadowToggles.GetChild(i-1).GetComponent<Toggle>().isOn = false;

                    }
                }
                for (int i = 1; i <= tfSceneDetailToggles.childCount; i++)
                {
                    if (sceneDetail == i)
                    {
                        tfSceneDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        tfSceneDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = false;

                    }
                }
                for (int i = 1; i <= tfSkillDetailToggles.childCount; i++)
                {
                    if (skillDetail == i)
                    {
                        tfSkillDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        tfSkillDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = false;
                    }
                }
                for (int i = 1; i <= tfModelDetailToggles.childCount; i++)
                {
                    if (modelDetail == i)
                    {
                        tfModelDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        tfModelDetailToggles.GetChild(i - 1).GetComponent<Toggle>().isOn = false;
                    }
                }
            }
            void onScrbMusicValueChange(float f)//显示的声音最大值100
            {
                MediaClient.instance.setMusicVolume(f);
                int value = (int)(float.Parse(f.ToString("F2")) * 100);
                txtMusicValue.text = value.ToString();
                musicValue = value;
            }
            void onScrbMusicEffectValueChange(float f)
            {
                SceneCamera.Set_Sound_Effect(f);
                MediaClient.instance.setSoundVolume(f);
                int value = (int)(float.Parse(f.ToString("F2")) * 100);
                txtMusicEffectValue.text = value.ToString();
                musicEffect = value;
            }
         

            public SystemPanel GetValue()// get user sett
            {
                //getVideoQualityValue();
                //getScreenResolutionValue();
                getFPSLimitValue();
                getRoleShadowValue();
                getSceneDetailValue();
                getSkillDetailValue();
                getModelDetailValue();

                return this;
            }
            //void getVideoQualityValue()
            //{
            //    for (int i = 0; i < videoQualityValueToggles.Count;i++ )
            //    {
            //        if (videoQualityValueToggles[i].isOn)
            //        {
            //            switch (i)
            //            {
            //                case 0://high
            //                    videoQualityValue = 0f;
            //                    break;
            //                case 1://middle
            //                    videoQualityValue = .5f;
            //                    break;
            //                case 2://low
            //                    videoQualityValue = 1f;
            //                    break;

            //                default:
            //                    break;
            //            }
            //            break;
            //        }
            //    }
            //}
            void getFPSLimitValue()
            {
                for (int i = 0; i < tfFPSlimitToggles.transform.childCount; i++)
                {
                    Toggle tg = tfFPSlimitToggles.GetChild(i).GetComponent<Toggle>();
                    if (tg.isOn)
                    {
                        switch (i)
                        {
                            case 0:
                                fpsLimitType = SystemItemConfigLvL.high;
                                break;
                            case 1:
                                fpsLimitType = SystemItemConfigLvL.middle;
                                break;
                            case 2:
                                fpsLimitType = SystemItemConfigLvL.low;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //void getScreenResolutionValue()
            //{
            //    for (int i = 0; i < tfResolutionToggles.transform.childCount; i++)
            //    {
            //        Toggle tg = tfResolutionToggles.GetChild(i).GetComponent<Toggle>();
            //        if (tg.isOn)
            //        {
            //            switch (i)
            //            {
            //                case 0:
            //                    resolutionType = SystemItemConfigLvL.high;
            //                    break;
            //                case 1:
            //                    resolutionType = SystemItemConfigLvL.middle;
            //                    break;
            //                case 2:
            //                    resolutionType = SystemItemConfigLvL.low;
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //    }
            //}

            void getRoleShadowValue()
            {
                for (int i = 0; i < tfRoleShadowToggles.transform.childCount; i++)
                {
                    Toggle tg = tfRoleShadowToggles.GetChild(i).GetComponent<Toggle>();
                    if (tg.isOn)
                    {
                        switch (i)
                        {
                            case 0:
                                roleShadowType = SystemItemConfigLvL.on;
                                break;
                            case 1:
                                roleShadowType = SystemItemConfigLvL.off;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            void getSceneDetailValue()
            {
                for (int i = 0; i < tfSceneDetailToggles.transform.childCount; i++)
                {
                    Toggle tg = tfSceneDetailToggles.GetChild(i).GetComponent<Toggle>();
                    if (tg.isOn)
                    {
                        switch (i)
                        {
                            case 0:
                                sceneDetailType = SystemItemConfigLvL.high;
                                break;
                            case 1:
                                sceneDetailType = SystemItemConfigLvL.middle;
                                break;
                            case 2:
                                sceneDetailType = SystemItemConfigLvL.low;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            void getSkillDetailValue()
            {
                for (int i = 0; i < tfSkillDetailToggles.transform.childCount; i++)
                {
                    Toggle tg = tfSkillDetailToggles.GetChild(i).GetComponent<Toggle>();
                    if (tg.isOn)
                    {
                        switch (i)
                        {
                            case 0:
                                skillDetailType = SystemItemConfigLvL.on;
                                break;
                            case 1:
                                skillDetailType = SystemItemConfigLvL.off;
                                break;
                            default:
                                skillDetailType = SystemItemConfigLvL.off;
                                break;
                        }
                    }
                }
            }

            void getModelDetailValue()
            {
                for (int i = 0; i < tfModelDetailToggles.transform.childCount; i++)
                {
                    Toggle tg = tfModelDetailToggles.GetChild(i).GetComponent<Toggle>();
                    if (tg.isOn)
                    {
                        switch (i)
                        {
                            case 0:
                                modelDetailType = SystemItemConfigLvL.on;
                                break;
                            case 1:
                                modelDetailType = SystemItemConfigLvL.off;
                                break;
                            default:
                                modelDetailType = SystemItemConfigLvL.on;
                                break;
                        }
                    }
                }
            }

            public  enum  SystemItemConfigLvL
            {
              high=1,
              middle = 2,
              low = 3,
              on = 1,
              off = 2,
            }
         
        }
        #endregion
        #region GamePanel
        class GamePanel : BaseSystemSetting
        {
            public static GamePanel mInstance;
            Transform root;
            public bool refuseTeamInvite;//屏蔽组队邀请
            public bool ignorePrivateInfo;//屏蔽私聊信息
            public bool ignorePaladinInvite;//屏蔽骑士团邀请
            public bool ignoreAddFirendHint;//屏蔽好友添加提示
            public bool ignoreOtherEffect;//屏蔽他人特效
            public bool ignoreOther;//屏蔽其他玩家
            public bool ignoreOtherPet;//屏蔽其它玩家宠物

          

            public GamePanel(Transform trans)
                : base(trans)
            {
                root = trans;
                mInstance = this;
            }
            public GamePanel GetValue()
            {
                for (int i = 0; i < root.childCount; i++)
                {
                    switch (i)
                    {
                        case 0:
                            refuseTeamInvite = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 1:
                            ignorePrivateInfo = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 2:
                            ignorePaladinInvite = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 3:
                            ignoreAddFirendHint = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 4:
                            ignoreOtherEffect = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 5:
                            ignoreOther = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        case 6:
                            ignoreOtherPet = root.GetChild(i).GetComponent<Toggle>().isOn;
                            break;
                        default:
                            break;
                    }
                }
                return this;
            }

            override public void onShowed()
            {
                for (int i = 0; i < root.childCount; i++)
                {
                    switch (i)
                    {
                        case 0:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.REFUSE_TEAM_INVITE;
                            break;
                        case 1:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_PRIVATE_INFO;
                            break;
                        case 2:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_KNIGHTAGE_INVITE;
                            break;
                        case 3:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_FRIEND_ADD_REMINDER;
                            break;
                        case 4:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_OTHER_EFFECT;
                            break;
                        case 5:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_OTHER_PLAYER;
                            break;
                        case 6:
                            root.GetChild(i).GetComponent<Toggle>().isOn = GlobleSetting.IGNORE_OTHER_PET;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion
        #region GiftBagPanel
        class GifBagPane : BaseSystemSetting
        {
            InputField iptGift;
            public GifBagPane(Transform trans)
                : base(trans)
            {
                iptGift = getGameObjectByPath("InputField").GetComponent<InputField>();
                new BaseButton(getGameObjectByPath("but_convert").transform).onClick = (GameObject go) => {
                    HttpAppMgr.instance.sendInputGiftCode(iptGift.text);
                    iptGift.text = "";
                };//发送cmd
            }
        }
        #endregion
       
    }
    class BaseSystemSetting : Skin
    {
        public BaseSystemSetting(Transform trans) : base(trans) { }
        virtual public void onShowed() { }
        virtual public void onClose() { }
        
    }
}
