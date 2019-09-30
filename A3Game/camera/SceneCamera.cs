using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MuGame;
using GameFramework;
using Cross;

static public class SceneCamera
{

    static public GameObject m_curGameObj;
    static public GameObject m_curSceneObj;

    //第一次注册的入场动画
    static public Transform m_curLoginCamObj;
    static public bool m_isFirstLogin = false;
    //触发器的场景动画
    static public Transform m_curTrrigerCamObj;
    //转生的场景动画
    static public Transform m_curZhuanShengCamObj;

    static public GameObject m_curCamGo;
    static public Camera m_curCamera;
    static public Vector2 m_forward;
    static public Vector2 m_right;

    static private Vector3 beginRoate;
    static private Vector3 beginPos;

    static public readonly int m_nGameScreen_High_w = Screen.width;
    static public readonly int m_nGameScreen_Mid_w = Screen.width / 2;
    static public readonly int m_nGameScreen_Low_w = Screen.width / 4;
    static public readonly int m_nGameScreen_High_h = Screen.height;
    static public readonly int m_nGameScreen_Mid_h = Screen.height / 2;
    static public readonly int m_nGameScreen_Low_h = Screen.height / 4;

    static public int m_nResetResolutionDelay = 0;
    //static public int m_nScreenResolution_Level = 1; //画面的分辨率
    static public int m_nFPSLimit_Level = 1; //游戏帧数的限制
    static public int m_nShadowAndLightGQ_Level = 1; //实时影子的图像品质
    static public int m_nSceneGQ_Level = 1; //场景的丰富程度
    //static public float m_fScreenGQ_Level = 0f;
    static public int m_nSkillEff_Level = 2;   //技能特效
    static public int m_nModelDetail_Level = 1;  //其他玩家模型

    //static public float m_fGameScreenPow = 1f;
    //static private RenderTexture m_curGameScreenTX;


    static public int lockType = 1;


    static public void Init()
    {
        m_curGameObj = GameObject.Find("GAME_CAMERA");
        m_curSceneObj = GameObject.Find("game_scene");

        if (m_curGameObj == null) return;
        if (m_curSceneObj == null)
            Debug.LogError("不存在对象--->" + "game_scene");
        else
        {
            m_curLoginCamObj = m_curSceneObj.transform.FindChild("camera_zc_ani");
            if (m_curLoginCamObj != null && m_curLoginCamObj.GetComponent<GameAniLoginCamera>() == null)
            {
                m_curLoginCamObj.gameObject.AddComponent<GameAniLoginCamera>();
                m_curLoginCamObj.gameObject.SetActive(false);
            }
        }

        Transform tfmyCamera = m_curGameObj.transform.FindChild("myCamera");
        m_curCamGo = tfmyCamera.gameObject;

        beginRoate = tfmyCamera.localEulerAngles;
        beginPos = tfmyCamera.localPosition;

        Vector3 fd = tfmyCamera.forward;
        m_forward = new Vector2(fd.x, fd.z);
        m_forward.Normalize();

        Vector3 rd = tfmyCamera.right;
        m_right = new Vector2(rd.x, rd.z);
        m_right.Normalize();

        m_curCamera = tfmyCamera.GetComponent<Camera>();

        SetGameLight(m_nShadowAndLightGQ_Level);
        SetGameShadow(m_nShadowAndLightGQ_Level);
        SetGameScene(m_nSceneGQ_Level);
        SetSikillEff(m_nSkillEff_Level);
        SetModelDetail(m_nModelDetail_Level);

        SetFPSLimit(m_nFPSLimit_Level);
        //SetResolution(m_nScreenResolution_Level);
        //SetGameScreenPow(m_fScreenGQ_Level);

        SceneTFX.InitScene();

        Set_Sound_Effect(MediaClient.instance.getSoundVolume());
    }


    public static void Set_Sound_Effect(float v = 1)
    {
        if (v > 1) v = 1;
        if (v < 0) v = 0;
        if (GRMap.instance.m_nCurMapID == 10)
        {
            m_curSceneObj.transform.FindChild("lv3/Reverb Zone").GetComponent<AudioSource>().volume = v;
        }
    }
    public static void SetGameShadow(int lv)
    {
        m_nShadowAndLightGQ_Level = lv;

        bool bopen = true;
        if (lv > 1) bopen = false;

        Transform Shadow_Camera = m_curGameObj.transform.FindChild("Shadow_Camera");
        if (Shadow_Camera != null) Shadow_Camera.gameObject.SetActive(bopen);
    }

    public static void SetGameLight(int lv)
    {
        bool bopen = true;
        if (lv > 1) bopen = false;

        Transform SL_scene = m_curGameObj.transform.FindChild("SL_scene");
        if (SL_scene != null) SL_scene.gameObject.SetActive(bopen);

        Transform SL_monster = m_curGameObj.transform.FindChild("SL_monster");
        if (SL_monster != null) SL_monster.gameObject.SetActive(bopen);

        //Transform DL_self = m_curGameObj.transform.FindChild("DL_self");
        //if (DL_self != null) DL_self.gameObject.SetActive(bopen);

        Transform DL_other = m_curGameObj.transform.FindChild("DL_other");
        if (DL_other != null) DL_other.gameObject.SetActive(bopen);

        ProfessionRole.isShowyingzi = bopen;

        if ( ProfessionRole.systemCallBack != null )
        {
            ProfessionRole.systemCallBack(SelfRole._inst);
        }

    }

    public static void SetGameScene(int lv)
    {
        m_nSceneGQ_Level = lv;

        if (m_curSceneObj != null)
        {
            bool blv2 = true;
            bool blv3 = true;
            if (lv == 2) blv3 = false;
            if (lv == 3)
            {
                blv2 = false;
                blv3 = false;
            }

            Transform lv2 = m_curSceneObj.transform.FindChild("lv2");
            if (lv2 != null) lv2.gameObject.SetActive(blv2);

            Transform lv3 = m_curSceneObj.transform.FindChild("lv3");
            if (lv3 != null) lv3.gameObject.SetActive(blv3);
        }
    }
    public static void SetSikillEff(int lv)
    {
        m_nSkillEff_Level = lv;
    }
    public static void SetModelDetail(int lv)
    {
        m_nModelDetail_Level = lv;
        OtherPlayerMgr._inst.refreshOtherModel();
    }

    //public static void SetResolution(int lv)
    //{
    //    m_nScreenResolution_Level = lv;
    //    switch (lv)
    //    {
    //        case 1: Screen.SetResolution(m_nGameScreen_High_w, m_nGameScreen_High_h, true); break;
    //        case 2: Screen.SetResolution(m_nGameScreen_Mid_w, m_nGameScreen_Mid_h, true); break;
    //        case 3: Screen.SetResolution(m_nGameScreen_Low_w, m_nGameScreen_Low_h, true); break;
    //        default:
    //            Screen.SetResolution(m_nGameScreen_Mid_w, m_nGameScreen_Mid_h, true); break;
    //    }

    //    m_nResetResolutionDelay = 4;        
    //}

    public static void SetFPSLimit(int lv)
    {
        m_nFPSLimit_Level = lv;
        switch (lv)
        {
            case 1: Application.targetFrameRate = 30; break;
            case 2: Application.targetFrameRate = 20; break;
            case 3: Application.targetFrameRate = 15; break;
            default:
                Application.targetFrameRate = 20; break;
        }
    }

    public static void CheckLoginCam()
    {//首次登入动画
        if (m_curLoginCamObj != null && m_isFirstLogin)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.RETURN_BT);
            m_curLoginCamObj.gameObject.SetActive(true);
            m_curGameObj.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_HIDE_ALL);
            NpcMgr.instance.can_touch = false;
        }
        else
        {
            if (m_curLoginCamObj != null)
                GameObject.Destroy(m_curLoginCamObj.gameObject);
            m_curLoginCamObj = null;
        }
    }
    public static void ResetAfterLoginCam()
    {
        InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.RETURN_BT);
        GameObject.Destroy(m_curLoginCamObj.gameObject);

        m_curGameObj.SetActive(true);
        InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        NpcMgr.instance.can_touch = true;

        //用于新手指引第一个任务
        if (GameObject.Find("a3_everydayLogin(Clone)") == null)
            UiEventCenter.getInstance().onWinClosed("a3_everydayLogin");
    }

    public static void CheckTrrigerCam(int camid)
    {//副本剧情动画
        
        int trriger_id = camid;
        DoAfterMgr.instacne.addAfterRender(() =>
        {
            if (m_curSceneObj != null)
            {
                m_curTrrigerCamObj = m_curSceneObj.transform.FindChild("cam_" + trriger_id);
                if (m_curTrrigerCamObj == null)
                    return;
                if (m_curTrrigerCamObj.GetComponent<GameAniTrrigerCamera>() == null)
                    m_curTrrigerCamObj.gameObject.AddComponent<GameAniTrrigerCamera>();
                m_curTrrigerCamObj.gameObject.SetActive(true);
                //needautofighting = camid;
                m_curGameObj.SetActive(false);
                InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_HIDE_ALL);
                NpcMgr.instance.can_touch = false;

                if (a1_gamejoy.inst_joystick != null)
                    a1_gamejoy.inst_joystick.OnDragOut(null);
                SelfRole.fsm.Stop();
            }
        });

    }
   // static   int needautofighting = 0;
    public static void ResetAfterTrrigerCam()
    {
        //台湾版本需求
        //if(needautofighting!=0)
        //{
        //    if(needautofighting==1051|| needautofighting == 1052||needautofighting == 1053 ||needautofighting == 1054 ||needautofighting == 1055 ||needautofighting == 1056 || needautofighting == 1057 || needautofighting == 1058 ||needautofighting == 1059
        //        || needautofighting == 1071 || needautofighting == 1072 || needautofighting == 1073 || needautofighting == 1074 || needautofighting == 1075 || needautofighting == 1076 || needautofighting == 1077 || needautofighting == 1078 || needautofighting == 1079
        //        || needautofighting == 105)
        //    {
        //        SelfRole.fsm.StartAutofight();
        //        needautofighting = 0;
        //    }
                
        //}
        if (m_curTrrigerCamObj != null)
            GameObject.Destroy(m_curTrrigerCamObj.gameObject);
        m_curGameObj.SetActive(true);
        InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        NpcMgr.instance.can_touch = true;
        m_curTrrigerCamObj = null;
    }

    public static void CheckZhuanShengCam()
    {//转生动画
        if (m_curSceneObj != null)
        {
            m_curZhuanShengCamObj = m_curSceneObj.transform.FindChild("cam_1000");
            if (m_curZhuanShengCamObj == null)
                return;
            foreach (ProfessionRole role in OtherPlayerMgr._inst.m_mapOtherPlayerSee.Values)
            {
                role.M_curGameObj.SetActive(false);
            }
            if (m_curZhuanShengCamObj.GetComponent<GameAniZhuanShengCamera>() == null)
                m_curZhuanShengCamObj.gameObject.AddComponent<GameAniZhuanShengCamera>();
            m_curZhuanShengCamObj.gameObject.SetActive(true);
            m_curGameObj.SetActive(false);
            
            
            NpcMgr.instance.can_touch = false;

            if (a1_gamejoy.inst_joystick != null && SelfRole._inst != null)
                a1_gamejoy.inst_joystick.OnDragOut(null);
            SelfRole._inst.setPos(new Vector3(53f, 17.2f, 29f));
            SelfRole._inst.TurnToPos(new Vector3(54f, 17.2f, 24f));
            a3_task_auto.instance.stopAuto = true;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_ZHUANSHENG_ANI);

            if ( SelfRole._inst.zuoji_Ani != null )
            {
                SelfRole._inst.GetAvatar.rideGo.SetActive( false );
                SelfRole._inst.GetAvatar.SetResetTransform();
                SelfRole._inst.m_curAni.SetFloat( EnumAni.ANI_F_FLY , SelfRole._inst.GetAvatar.m_WindID > 0 ? 1f : 0f );
            }

        }
    }

    public static void ResetAfterZhuanShengCam()
    {
        foreach (ProfessionRole role in OtherPlayerMgr._inst.m_mapOtherPlayerSee.Values)
        {
            role.M_curGameObj.SetActive(true);
        }
        if (m_curZhuanShengCamObj != null)
            m_curZhuanShengCamObj.gameObject.SetActive(false);

        m_curGameObj.SetActive(true);
        InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
        NpcMgr.instance.can_touch = true;
        a3_task_auto.instance.stopAuto = false;
        m_curZhuanShengCamObj = null;

        if ( SelfRole._inst.zuoji_Ani != null )
        {
            SelfRole._inst.GetAvatar.rideGo.SetActive( true );
            SelfRole._inst.m_curAni.SetFloat( EnumAni.ANI_F_FLY , 2f );
            SelfRole._inst.GetAvatar.SetPlayerWeaponTransform( SelfRole._inst.GetAvatar.GetCarrer());
        }

        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RESETLVLSUCCESS);

    }

    //public static void SetGameScreenPow(float pow)
    //{
    //    //m_fScreenGQ_Level = pow;

    //    //这里先屏蔽掉场景设置
    //    //if (pow > 0f)
    //    //{
    //    //    m_fGameScreenPow = pow + 1f;//pow * 3f +2f;
    //    //}
    //    //else
    //    //{
    //    m_fGameScreenPow = 1f;
    //    //}

    //    if (m_curGameScreenTX != null)
    //    {
    //        m_curGameScreenTX.Release();
    //        m_curGameScreenTX = null;
    //        m_curCamera.targetTexture = null;
    //        InterfaceMgr.getInstance().linkGameScreen(null);
    //        InterfaceMgr.getInstance().hideGameScreen();
    //    }

    //    //if (m_fGameScreenPow > 1f)
    //    //{
    //    //    if (m_curGameScreenTX == null)
    //    //    {
    //    //        m_curGameScreenTX = new RenderTexture((int)(Screen.width / m_fGameScreenPow), (int)(Screen.height / m_fGameScreenPow), 16, RenderTextureFormat.RGB565);
    //    //        m_curCamera.targetTexture = m_curGameScreenTX;
    //    //        string name = m_curCamera.name;

    //    //        InterfaceMgr.getInstance().linkGameScreen(m_curGameScreenTX);
    //    //    }
    //    //    else
    //    //    {
    //    //        m_curCamera.targetTexture = m_curGameScreenTX;
    //    //    }
    //    //}
    //}

    private static Action toMainFinHandle;
    private static GameObject curCam;
    public static void setCamChangeToMainCam(float speed = 10f, Action Fin = null)
    {
        if (curCam == null)
            return;
        Animator curcamani = curCam.transform.parent.GetComponent<Animator>();
        if (curcamani)
            curcamani.enabled = false;

        toMainFinHandle = Fin;
        if (speed <= 0)
        {
            lockType = 101;
        }
        else
        {
            float sec = Vector3.Distance(m_curGameObj.transform.position, curCam.transform.position) / speed;

            // curCam = cam;
            curCam.transform.DOMove(SelfRole._inst.m_curModel.position + beginPos, sec);
            curCam.transform.DOLocalRotate(beginRoate, sec).OnComplete(() =>
            {
                lockType = 101;
            });
        }

    }

    public static void endStory(float speed)
    {
        if (curCam == null)
            return;

        GameAniCamera ani = curCam.transform.parent.GetComponent<GameAniCamera>();
        if (ani == null)
            return;

        ani.onSpeedEnd(speed);
    }


    public static void changeAniCamera(GameObject cam, float speed = 10f)
    {

        if (curCam != null)
            GameObject.Destroy(curCam.gameObject);


        PlayerNameUIMgr.getInstance().hideAll();

        curCam = cam;
        lockType = 0;
        Animator curcamani = curCam.transform.parent.GetComponent<Animator>();



        m_curGameObj.SetActive(false);
        curCam.transform.parent.gameObject.SetActive(true);



        float sec = Vector3.Distance(m_curGameObj.transform.position, curCam.transform.position) / speed;

        curCam.transform.DOMove(SelfRole._inst.m_curModel.position + beginPos, sec).From().SetEase(Ease.Linear);
        curCam.transform.DOLocalRotate(beginRoate, sec).From().SetEase(Ease.Linear).OnComplete(() =>
       {
           curcamani.enabled = true;
       });
    }




    public static void cameraShake(float time, int count, float str)
    {



        if (curCam != null)
        {
            Transform trans = curCam.transform.FindChild("cam");
            if (trans != null)
                trans.DOShakePosition(time, str, count);

        }
        else if (m_curGameObj != null)
        {

            m_curCamGo.transform.DOShakePosition(time, str, count).OnComplete(resetMainCamPos);
        }
    }

    public static void resetMainCamPos()
    {
        m_curCamGo.transform.localPosition = beginPos;
    }

    public static void lockTOPlayer()
    {
        m_curGameObj.transform.DOMove(SelfRole._inst.m_curModel.position, 3);
        m_curCamGo.transform.DOLocalRotate(beginRoate, 3);
        m_curCamGo.transform.DOLocalMove(beginPos, 3);
    }

    static public void FrameMove()
    {
        if (m_curGameObj == null) return;

        if(m_nResetResolutionDelay > 0)
        {
            --m_nResetResolutionDelay;
            if(m_nResetResolutionDelay <= 0)
            {
                InterfaceMgr.getInstance().ResetUI_Main_Camera();
            }
        }

        //if (m_curGameScreenTX != null)
        //{
        //    if (m_curGameObj.active == true)
        //    {
        //        InterfaceMgr.getInstance().showGameScreen();
        //    }
        //    else
        //    {
        //        InterfaceMgr.getInstance().hideGameScreen();
        //    }
        //}


        if (lockType == 1)
        {
            Vector3 offpos = (m_curGameObj.transform.position - SelfRole._inst.m_curModel.position) * 0.125f;
            m_curGameObj.transform.position = SelfRole._inst.m_curModel.position + offpos;
        }
        else if (lockType == 101)//切换成住摄像机
        {
            if (toMainFinHandle != null)
                toMainFinHandle();

            //    GameObject.Destroy(curCam.gameObject);
            curCam = null;
            m_curGameObj.SetActive(true);
            lockType = 1;
            PlayerNameUIMgr.getInstance().showAll();
        }
    }

    private static GameObject curMiniMapCanvas;
    private static UnityEngine.Rect CutRect = new UnityEngine.Rect(0, 0, 1, 1);
    public static void refreshMiniMapCanvas()
    {
        if (a3_liteMiniBaseMap.instance == null) return;

        GameObject minicam = a3_liteMiniBaseMap.camGo;

        //if (minicam == null)
        //    return;
        curMiniMapCanvas = GameObject.Find("MINIMAP_CANVAS");
        if (curMiniMapCanvas == null)
            return;

        Shader sd = Shader.Find("UI/Unlit/Transparent");

        //Texture2D sp1 = GAMEAPI.ABUI_LoadSprite("map_map" + GRMap.curSvrConf["id"]).texture;
        //if (sp1 != null)
        //{
        //    curMiniMapCanvas.GetComponent<Renderer>().material.SetTexture("_MainTex", sp1);
        //    curMiniMapCanvas.GetComponent<Renderer>().material.shader = sd;
        //    return;
        //}

        Transform transminiMap = minicam?.transform.FindChild("camera");
        GameObject go = new GameObject();
        Camera mCam = go.AddComponent<Camera>();
        mCam.backgroundColor = new Color(0f, 0f, 0f);
        mCam.orthographic = true;
        mCam.orthographicSize = curMiniMapCanvas.transform.localScale.x * 0.5f;
        Vector3 vec = curMiniMapCanvas.transform.position;
        vec.y = 50;
        go.transform.position = vec;

        if (transminiMap != null)
        {
            go.transform.rotation = transminiMap.rotation;
        }
        // mCam.cullingMask = 1 << LayerMask.NameToLayer("scene_shadow");
        mCam.cullingMask = (1 << EnumLayer.LM_SCENE_NORMAL) + (1 << EnumLayer.LM_SCENE_SHADOW) + (1 << EnumLayer.LM_FX);
        RenderTexture rt = new RenderTexture(512, 512, 2);
        mCam.pixelRect = new UnityEngine.Rect(0, 0, Screen.width, Screen.height);
        mCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D((int)(512 * CutRect.width), (int)(512 * CutRect.height),
                                                 TextureFormat.RGB24, false);
        mCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new UnityEngine.Rect(512 * CutRect.x, 512 * CutRect.y, 512 * CutRect.width, 512 * CutRect.height), 0, 0);
        screenShot.Apply();

        curMiniMapCanvas.GetComponent<Renderer>().material.SetTexture("_MainTex", screenShot);

        sd = Shader.Find("UI/Unlit/Transparent");

        curMiniMapCanvas.GetComponent<Renderer>().material.shader = sd;
        mCam.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.Destroy(rt);

        GameObject.Destroy(go);
    }

    public static Vector2 getPosOnMiniMap(float picscale = 1)
    {
        Vector3 pos = SelfRole._inst.m_curModel.position;

        if (curMiniMapCanvas == null) { curMiniMapCanvas = GameObject.Find("MINIMAP_CANVAS"); }
        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;

        Vector3 tempos = curMiniMapCanvas.transform.localPosition;
        // Vector2 picpos = new Vector2(512f-(pos.x / scale)-tempos.x,512f-( pos.z / scale)-tempos.z);
        Vector2 picpos = new Vector2(-((pos.x - tempos.x) / scale), -((pos.z - tempos.z) / scale));
        Vector2 vec = picpos * picscale;
        //vec.x = vec.x - 512f * picscale;
        //vec.y = vec.y - 512f * picscale;

        return vec;
    }

    public static Vector2 getTeamPosOnMinMap(int x, int y, float picscale = 1)
    {

        Vector3 pos = new Vector3(x, 0, y);
        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;
        Vector3 tempos = curMiniMapCanvas.transform.localPosition;
        Vector2 picpos = new Vector2(-((pos.x - tempos.x) / scale), -((pos.z - tempos.z) / scale));
        Vector2 vec = picpos * picscale;
        return vec;
    }










    public static Vector2 getPosOnMiniMapNMA(Vector3 v3, float picscale = 0.45f)
    {

        //  Vector3 pos = SelfRole._inst.m_curModel.position;
        Vector3 pos = v3;
        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;
        Vector3 tempos = curMiniMapCanvas.transform.localPosition;
        // Vector2 picpos = new Vector2(512f-(pos.x / scale)-tempos.x,512f-( pos.z / scale)-tempos.z);
        Vector2 picpos = new Vector2(-((pos.x - tempos.x) / scale), -((pos.z - tempos.z) / scale));
        Vector2 vec = picpos * picscale;
        //vec.x = vec.x - 512f * picscale;
        //vec.y = vec.y - 512f * picscale;
        return vec;
    }


    public static Vector2 getPosOnMiniMap(Vector3 worldpos)
    {
        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;
        Vector3 tempos = curMiniMapCanvas.transform.localPosition;
        Vector2 picpos = new Vector2(-((worldpos.x - tempos.x) / scale), -((worldpos.z - tempos.z) / scale));
        return picpos;
    }



    public static float getLengthOnMinMap(float worldLength)
    {
        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;
        return worldLength / scale;
    }

    public static Vector3 getPosByMiniMap(Vector2 pos, float picscale, float distenc = 20f)
    {
        if (curMiniMapCanvas == null)
            return Vector3.zero;
        Vector3 tempos = curMiniMapCanvas.transform.localPosition;
        Vector3 vec = pos / picscale;

        float scale = curMiniMapCanvas.transform.localScale.x / 1024f;

        vec = new Vector3(-vec.x * scale + tempos.x, 10f, -vec.y * scale + tempos.z);


        //  vec=scale * vec;
        NavMeshHit hit;
        NavMesh.SamplePosition(vec, out hit, distenc, NavmeshUtils.allARE);
        return hit.position;
    }
}


public class GameAniLoginCamera : MonoBehaviour
{
    public void onLoginEnd()
    {
        SceneCamera.ResetAfterLoginCam();
    }
}

public class GameAniTrrigerCamera : MonoBehaviour
{
    public void onEnd()
    {
        SceneCamera.ResetAfterTrrigerCam();
    }
}

public class GameAniZhuanShengCamera : MonoBehaviour
{
    public void onEnd()
    {
        SceneCamera.ResetAfterZhuanShengCam();
    }

    public void onZhuanSheng1()
    {
        NpcMgr.instance.getRole(1013).playSkill();
    }

    public void onZhuanSheng2()
    {
        SelfRole._inst.m_curAni.SetTrigger("zhuansheng");
    }
}

public class GameAniCamera : MonoBehaviour
{
    private bool hasAddEndEvent = false;
    public float speed = 10f;
    public bool uiactive = false;
    public Animator ani;


    void Start()
    {
        ani = gameObject.GetComponent<Animator>();


        ani.enabled = false;
        gameObject.SetActive(false);
    }

    public void stopAni()
    {
        NbDoItems.cacheCameraAni = ani;
        ani.speed = 0f;
    }

    public void onSpeedEnd(float endSpeed)
    {
        SceneCamera.setCamChangeToMainCam(endSpeed, dispose);
    }

    public void onEnd()
    {
        SceneCamera.setCamChangeToMainCam(speed, dispose);
    }

    public void onLoginEnd()
    {
        SceneCamera.ResetAfterLoginCam();
    }

    void dispose()
    {
        ani = null;
        if (uiactive)
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);

        NpcRole[] arr = transform.GetComponentsInChildren<NpcRole>();
        foreach (NpcRole role in arr)
        {
            role.dispose();
        }

        Destroy(transform.gameObject);
    }

    public Dictionary<string, GameObject> dEvt = new Dictionary<string, GameObject>();
    public void doit(string id)
    {

        if (dEvt.ContainsKey(id))
        {
            GameObject go = dEvt[id];
            TriggerHanldePoint trigger = go.GetComponent<TriggerHanldePoint>();
            dEvt.Remove(id);
            if (trigger != null)
            {
                trigger.onTriggerHanlde();
            }
            Destroy(go);
        }
    }


    public void shake(string pram)
    {
        string[] arr = pram.Split(new char[] { ',' });
        if (arr.Length < 3)
            return;
        SceneCamera.cameraShake(float.Parse(arr[0]), int.Parse(arr[1]), float.Parse(arr[2]));
    }
}
