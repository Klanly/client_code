using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;
using DG.Tweening;
namespace MuGame
{
    public class login : LoadingUI
    {
        Button bt;
        Button btChoose;
        Text txtServer;

        public static string m_QSMY_Update_url;

        GameObject loadingserver;

        void CheckUpdateVer()
        {

            changeState(0);
        }

        private string errorString = "";
        public void onUpdateError(string str)
        {
            if (errorString == "")
                errorString = str;
        }


        public ServerData curServerData;

        private Animation aniCam;

        public GameObject scene;

        private Image imageBg;

        private GameObject goUpdate;
        private Transform updateline;
        private Text updatTxt;

        private Text uidTxt;

        private Text vetTxt;

        private GameObject noticePanel;
        private Text noticeTxt;
        //private Text titleTxt;
        //GameObject titleTxt;
        private GameObject btn_look;
        private Text waitTxt;

        public LoginMsgBox msg;

        public static login instance;
        public bool canEnter = false;
        public override void init()
        {
            getGameObjectByPath("login").SetActive(true);
            getGameObjectByPath("loading").SetActive(true);
            
           
            instance = this;
            txtServer = getComponentByPath<Text>("login/server/Text");
            txtServer.text = "";
            bt = getComponentByPath<Button>("login/bt");
            btChoose = getComponentByPath<Button>("login/server");

            waitTxt = getComponentByPath<Text>("loading/txt_wait");
            waitTxt.gameObject.SetActive(false);

            imageBg = this.getComponentByPath<Image>("mask");
            imageBg.gameObject.SetActive(false);
            goUpdate = getGameObjectByPath("update");
            updateline = goUpdate.transform.FindChild("line");
            updatTxt = goUpdate.transform.FindChild("Text").GetComponent<Text>();
            uidTxt = getComponentByPath<Text>("login/uid/txt");
            vetTxt = getComponentByPath<Text>("login/ver_txt");

            noticePanel = getGameObjectByPath("notice");
            noticeTxt = noticePanel.transform.FindChild("scroll_view/str").GetComponent<Text>();
            //titleTxt = noticePanel.transform.FindChild("title").GetComponent<Text>();
            //titleTxt = noticePanel.transform.FindChild("title").gameObject;
            btn_look = getGameObjectByPath("btn_look");
            btn_look.GetComponent<Button>().onClick.AddListener(onOpenNotice);
            noticePanel.transform.FindChild("close").GetComponent<Button>().onClick.AddListener(onCloseNotice);

            GameObject go = getGameObjectByPath("msg");
            go.SetActive(false);
            msg = new LoginMsgBox(go.transform);

            bt.onClick.AddListener(onEnterClick);
            btChoose.onClick.AddListener(onChoose);

            loadingserver = getGameObjectByPath("loading_update");

            bt.interactable = btChoose.interactable = false;
            goUpdate.SetActive(false);
            GlobleSetting.initSystemSetting();

            getGameObjectByPath("fg").SetActive(false);

            initScence();

            if (Globle.QSMY_Platform_Index == ENUM_QSMY_PLATFORM.QSPF_LINKSDK)
            {
               
                AndroidPlotformSDK.LOGIN_ACTION = platform_Login;
                IOSPlatformSDK.LOGIN_ACTION = platform_Login;
                AnyPlotformSDK.Call_Cmd("login");


                ////内网链接测试
                //Globle.YR_srvlists__slurl = "http://10.1.8.76/ifacec/sdk_srvlists.php?";
                //Globle.YR_srvlists__platform = "lanandroid";
                //Globle.YR_srvlists__sign = "123";
                //Globle.YR_srvlists__platuid = "171039691";
                //LGPlatInfo.inst.loadServerList();
            }
            else
            {
                changeState(-1);
            }
        }

        void initScence()
        {
            GameObject temp = U3DAPI.U3DResLoad<GameObject>("ui_mesh/login/loginscene");
            scene = GameObject.Instantiate(temp) as GameObject;
          
            aniCam = scene.transform.FindChild("camera").GetComponent<Animation>();


            //LightmapData lightmapData = new LightmapData();
            //lightmapData.lightmapFar = U3DAPI.U3DResLoad<Texture2D>("ui_mesh/login/scene/LightmapFar-0") as Texture2D;
            //LightmapSettings.lightmaps = new LightmapData[] { lightmapData };


            InterfaceMgr.ui_Camera_cam.gameObject.SetActive(false);
        }

        //第三方登入
        private void platform_Login(Variant v_login)
        {
            debug.Log("收到平台的登入数据 " + v_login.dump());

  //          收到平台的登入数据 < Dct:3 >
 
  //   [cmd]:"login"
  //   [result]:"11"
  //   [data]:< Dct:10 >
  
  //        [avatar]:"73e164638990031b2da510626cc73dc5|1496221894|quickandroid|0"
  //        [resource_url]:"http://cdn.qsmy.hulai.com/qsmy/android/"
  //       [srvlists_url]:"http://120.92.16.209/ifacec/sdk_srvlists.php"
  //      [token]:"@171@91@163@82@111@88@103@110@146@156@152@109@109@106@100@148@86@96@87@172@159@149@90@114@83@107@102@105@104@106@109@104@105@113@82@92@87@170@160@163@150@89@111@110@102@108@96@85@151@86@111@89@111@106@113@113@83@178"
  //     [uid]:"604276198"
  //    [username]:"quicktest"
  //   [sign]:"2a991c2c702012ebcde716b5fd2e937b"
  //  [pid]:"quickandroid"
  // [content]:"null"
  //[titles]:""


            bool b_success = false;
            if (v_login.ContainsKey("result"))
            {
                Variant data = v_login["data"];

                b_success = true;
                if (data.ContainsKey("pid"))
                    Globle.YR_srvlists__platform = data["pid"]._str;
                else
                {
                    debug.Log("解析失败 pid----------");
                    b_success = false;
                }

                if (data.ContainsKey("uid"))
                {
                    Globle.YR_srvlists__platuid = data["uid"]._str;
                    uidTxt.text = data["uid"]._str;
                } 
                else
                {
                    debug.Log("解析失败 uid----------");
                    b_success = false;
                }

                if (data.ContainsKey("avatar"))
                    Globle.YR_srvlists__sign = data["avatar"]._str;
                else
                {
                    debug.Log("解析失败 avatar----------");
                    b_success = false;
                }

                if (data.ContainsKey("srvlists_url"))
                    Globle.YR_srvlists__slurl = data["srvlists_url"]._str;
                else
                {
                    debug.Log("解析失败 srvlists_url----------");
                    b_success = false;
                }

                if (data.ContainsKey("titles") && data["titles"] != "")
                {
                    string cnotent = data["content"];
                    string titles = data["titles"];
                    cnotent = cnotent.Replace('n', '\n');
                    noticePanel.SetActive(true);
                    btn_look.SetActive(true);
                    noticeTxt.text = cnotent;
                    //titleTxt.text = titles;
                }

                vetTxt.text = Globle.CLIENT_VER + "_" + Globle.VER;
            }
            else
            {
                debug.Log("没有结果数据----------");
            }

            Variant.free(v_login);
            v_login = null;

            if (b_success)
            {
                debug.Log("登入成功----------");

                AndroidPlotformSDK.m_bLogined = true;
                IOSPlatformSDK.m_bLogined = true;

                //进入选服页面，选服页面就是现在的login，好好优化下
                //CG_PlayOver();
                changeState(-1);
            }
            else
            {
                debug.Log("登入失败 30帧后 再登入");
                m_nSDK_ReloginCount = 30;
            }
        }

        float toLineScale = 1f;
        float curlineScale = 0f;
        private Color maskColor = new Color(1f, 1f, 1f, 1f);
        private int curState = 0;
        private int m_nSDK_ReloginCount = 0; //因为不知的原因，平台登入取消了，或者是登入错误了，需要重新打开登入界面
        void Update()
        {
            if (m_nSDK_ReloginCount > 0)
            {
                debug.Log("平台的登入取消 " + m_nSDK_ReloginCount);
                m_nSDK_ReloginCount--;

                if (m_nSDK_ReloginCount == 0)
                {
                    debug.Log("平台的登入 " + m_nSDK_ReloginCount);
                    AnyPlotformSDK.Call_Cmd("login");
                }
            }

            if (goUpdate.active == true && curlineScale < 1f)
            {

                if (curlineScale < toLineScale)
                {
                    curlineScale += 0.01f;
                    if (curlineScale > 0.95f)
                        curlineScale = 1f;

                    updateline.localScale = new Vector3(curlineScale, 1, 1);

                    if (curlineScale == 1f)
                    {
                        if (canEnter)
                            changeState(0);
                        else
                            canEnter = true;
                    }

                }

            }

            if (curState == 0)
                return;

            if (curState == 1)
            {
                curState = 2;
            }
            else if (curState == 2)
            {
                aniCam.Play("cam_move");
                imageBg.gameObject.SetActive(true);
                maskColor.a = 0f;
                imageBg.color = maskColor;
                curState = 3;
            }
            else if (curState == 3)
            {


                float anilen = aniCam["cam_move"].normalizedTime;
                //  aniCam.
                if (anilen >= 1f )
                {
                    if (_overhandle != null)
                        _overhandle();
                    _overhandle = null;
                    curState = 4;
                    maskColor.a = 1;
                    if (scene != null)
                        Destroy(scene);
                    scene = null;
                    if (GRMap.instance != null)
                    {
                        GRMap.instance.refreshLightMap();
                        LGUIMainUIImpl_NEED_REMOVE.getInstance().show_all();
                    }
                }
                else
                {
                    maskColor.a = anilen;
                    imageBg.color = maskColor;

                }
            }
            else if (curState == 4)
            {
                if (maskColor.a <= 0.1f )
                {
                    curState = 5;
                    InterfaceMgr.ui_Camera_cam.gameObject.SetActive(true);
                    InterfaceMgr.ui_Camera_cam.gameObject.SetActive(true);
                    InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.SERVE_CHOOSE);
                    InterfaceMgr.getInstance().DisposeUI(InterfaceMgr.LOGIN);
                }
                else
                {
                    maskColor.a -= 0.05f;
                    imageBg.color = maskColor;
                }
            }
        }

        public void showUILoading()
        {
            getGameObjectByPath("login").SetActive(false);
            getGameObjectByPath("loading").SetActive(true);
        }

        private bool serverChanged = false;
        public void setServer(ServerData d)
        {
            serverChanged = true;
            Globle.curServerD = curServerData = d;
            txtServer.text = d.server_name;
            bt.interactable = true;
            for (int i = 1; i <= 5; i++)
            {
                if (i == d.srv_status)
                    getGameObjectByPath("login/server/sevstate/" + i).SetActive(true);
                else
                    getGameObjectByPath("login/server/sevstate/" + i).SetActive(false);
            }
        }

        public void setServer(int sid)
        {
            if (Globle.dServer.ContainsKey(sid) == false)
                return;

            setServer(Globle.dServer[sid]);
        }

        private Action _overhandle;

        public void onBeginLoading(Action overhandle)
        {
            if (curState != 0)
                return;
            _overhandle = overhandle;
            changeState(1);
        }


        public void setWaitingTxt(string txt)
        {
            waitTxt.text = txt;
            waitTxt.gameObject.SetActive(true);
        }

        private int lastIdx = -999;
        public void changeState(int idx, int parm = 0)
        {
            if (curState != 0)
                return;

            lastIdx = idx;
            if (idx == -1)
            {
                goUpdate.SetActive(false);
                getGameObjectByPath("login").SetActive(false);
                getGameObjectByPath("loading").SetActive(false);

                CheckUpdateVer();
            }
          
            else if (idx == 0)
            {
                LGPlatInfo.inst.loadServerList();
                if (goUpdate.active)
                {
                    goUpdate.transform.DOKill();
                    goUpdate.transform.DOScale(0, 0.3f);
                }
                else
                {
                    goUpdate.SetActive(false);
                }

            }
            else if (idx == 1)
            {
                getGameObjectByPath("login").SetActive(false);
                getGameObjectByPath("loading").SetActive(false);

              
                curState = 1;
            }


        }


        void onLoadSid(int sid)
        {
            if (serverChanged)
                return;

            setServer(sid);
        }

        public void refresh()
        {
            if (goUpdate.active)
            {
                getGameObjectByPath("login").SetActive(true);
                getGameObjectByPath("login").transform.DOScale(0, 0.3f).From();
            }
            else
            {
                getGameObjectByPath("login").SetActive(true);
            }


            btChoose.interactable = true;
            int sid = 0;
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.LOGIN_SERVER_SID))
                sid = PlayeLocalInfo.loadInt(PlayeLocalInfo.LOGIN_SERVER_SID);

            if (sid == 0)
            {


                //发送给服务器读取服务器缓存sid
                if (Globle.defServerId != 0)
                    sid = Globle.defServerId;
                else if (Globle.lServer.Count > 0)
                    sid = Globle.lServer[0].sid;
            }


            setServer(sid);
            serverChanged = false;
        }

        void onEnterClick()
        {
            if (curServerData == null)
                return;

            if (curServerData.srv_status == 5)
            {
                //服务器维护中
                msg.show(true, ContMgr.getOutGameCont("debug3"));
            }
            else
            {
                bt.interactable = false;
                btn_look.SetActive(false);
                PlayeLocalInfo.saveInt(PlayeLocalInfo.LOGIN_SERVER_SID, curServerData.sid);
                LGPlatInfo.inst.sendLogin(curServerData.login_url);
                //if (debug.instance != null)
                //    debug.instance.showMsg(ContMgr.getOutGameCont("debug1"));

                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidPlotformSDK.ANDROID_HIDE_STATUSBAR();
                }
            }
        }

        void onCloseNotice()
        {
            noticePanel.SetActive(false);
        }

        void onOpenNotice()
        {
            noticePanel.SetActive(true);
        }

        void onChoose()
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SERVE_CHOOSE);
        }

        public override void dispose()
        {
            if (scene != null)
                Destroy(scene);
            scene = null;
            bt.onClick.RemoveAllListeners();
            btChoose.onClick.RemoveAllListeners();
            bt = null;
            btChoose = null;
            txtServer = null;
            _overhandle = null;
            instance = null;
            base.dispose();
        }
    }

   public class LoginMsgBox : Skin
    {
        private Text txt;

        public LoginMsgBox(Transform trans)
            : base(trans)
        {
            txt = getComponentByPath<Text>("Text");
            getComponentByPath<Button>("bt").onClick.AddListener(onClick);
            visiable = false;
        }
        private Action _handle;
        public void show(bool b, string str, Action handle = null)
        {
            _handle = handle;
            if (b)
            {
                txt.text = str;
                visiable = true;
                __mainTrans.localScale = Vector3.one;
                __mainTrans.DOScale(1, 0.3f).From();
            }
            else
            {
                txt.text = "";
                _handle = null;
                visiable = false;
            }
        }

        public void onClick()
        {
            if (_handle != null)
                _handle();
            visiable = false;
        }
    }

}
