using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class LGPlatInfo
    {

        //----------------------------------------手机版本实现，网页版无需调用---------------------------------------------	
        private string _query = "http://10.1.8.45/ifacec/sdk_srvlists.php";// 外网手机研发测试服
        private Variant _srv_lists = null;
        private Variant _curSvrPlyInfo = null;
        private Action<Variant> _notifyFun = null;
        private int _curSid = 0;//U3D服务器
        private static LGPlatInfo _inst;
        private string _platId = "";
        public static LGPlatInfo inst
        {
            get
            {
                if (_inst == null) _inst = new LGPlatInfo();
                return _inst;
            }
        }
        public Variant svlist
        {
            get
            {
                return _srv_lists;
            }
        }
        public int sid
        {
            get
            {
                return _curSid;
            }
            set
            {
                _curSid = value;
            }
        }
        public void setServerListReqUrl(string url)
        {
            _query = url;
        }

        public void regNotifiFun(Action<Variant> cbfun)
        {
            _notifyFun = cbfun;
        }
        public int GetRecommend()
        {
            return 5;
        }

        //public void onSdkCallBack(Variant v)
        //{
        //    DebugTrace.print("onSdkCallBack:");
        //    DebugTrace.dumpObj(v);
        //    string cmd = v["cmd"]._str;
        //    if (AndroidSDKManager.SDKType.login.ToString() == cmd)
        //    {
        //        //_platId= "";
        //        onSelectPlatuid(v);
        //    }
        //    else
        //    {

        //    }
        //}

        private void onSelectPlatuid(Variant v)
        {
            if (!v.ContainsKey("data") || !v["data"].ContainsKey("pid") || !v["data"].ContainsKey("avatar") || !v["data"].ContainsKey("uid"))
            {
                DebugTrace.print("Erorr: Variant no`t Find <data>");
                DebugTrace.dumpObj(v);
                return;
            }

            string str1 = "platform=" + v["data"]["pid"]._str + "&sign=" + v["data"]["avatar"]._str + "&platuid=" + v["data"]["uid"]._str;
            //  str1 = "platform=qihooandroid&sign=7d065c0b871a4f280404070bc1aaa125|1439812387" + "&platuid=" + "2534758785";
            HttpAppMgr.POSTSvr(_query, str1, _getSeverListBack);
        }
        //获取服务器列表
        private void _getSeverListBack(Variant data)
        {
            //DebugTrace.print("_getSeverListBack:");

            debug.Log("收到服务器列表信息： " + data.dump());
            //DebugTrace.dumpObj( data );

            if (data["r"]._int == 0)
            {//获取服务器列表失败
                //Variant v = new Variant();
                //v["list_err"] = data;
                //notify( v );


                if (data.ContainsKey("errmsg"))
                    debug.Log("SeverListError::" + StringUtils.unicodeToStr(data["errmsg"]._str));

                retryLoadServerList();
            }
            else if (data["r"]._int == 1)
            {
                if (data.ContainsKey("data"))
                {
                    _srv_lists = data["data"]["srv_lists"];
                    Variant v = new Variant();
                    v["svrList"] = _srv_lists;
                    notify(v);
                    Globle.initServer(_srv_lists._arr);
                    login.instance.refresh();
                }
                else
                {
                    //notify( {key:"list_err", data:data} );	
                }

                //int sid = GetSrvSid(_srv_lists.Count - 1);
                //if (os.sys.loscalStorage.readInt("sever_id") == 0)
                //    _curSid = GetRecommend();
                //else
                //    _curSid = os.sys.loscalStorage.readInt("sever_id");
                //GetPlyInfo(_curSid);
            }
        }

        int serverListErrorCount = 0;
        void retryLoadServerList()
        {
            serverListErrorCount++;
            if (serverListErrorCount > 3)
            {
                if (Globle.DebugMode == 0)
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.DISCONECT);
                return;
            }
            loadServerList();
        }

        private int GetSrvSid(int inedx)
        {
            if (inedx < 0)
                return _srv_lists.Count - 1;

            if (_srv_lists[inedx]["close"]._int == 0)
                return _srv_lists[inedx]["sid"]._int;
            else
                return GetSrvSid(inedx - 1);
        }

        public static bool relogined = false;
        public static bool relogin_again = false;
        public void relogin()
        {
            relogined = true;
            relogin_again = false;
            if (Globle.DebugMode == 2 )
            {
                NetClient.instance.reConnect();
               // this.g_mgr.g_netM.reConnect();
            }
            else 
            { 
                HttpAppMgr.POSTSvr(Globle.curServerD.login_url, "", _getPlyInfoBack, true, "GET");
                //解决手机上断线重连的问题
                DelayDoManager.singleton.AddDelayDo(() =>
                {
                    if(relogin_again == false)
                    {
                        if (disconect.instance != null)
                            disconect.instance.setErrorType(disconect.ERROR_TYPE_NETWORK);
                    } 
                }, 3);
            }
        }


        //public function test():void
        //{
        //    notify( {key:"test"} );	
        //}
        public void GetPlyInfo(int sid)
        {
            os.sys.loscalStorage.writeInt("sever_id", sid);
            for (int i = 0; i < _srv_lists.Count; i++)
            {
                if (_srv_lists[i]["sid"]._int == sid)
                {
                    //string requrl = _srv_lists[i]["login_url"]._str;
                    //string[] strs = requrl.Split('?');
                    //POSTSvr( strs[0], strs[1], _getPlyInfoBack );
                    HttpAppMgr.POSTSvr(_srv_lists[i]["login_url"]._str, "", _getPlyInfoBack, true, "GET");
                    break;
                }
            }
        }


        /*
        * 
            adv_url	"http://10.1.59.138/dkclient/loading.jpg"	
            allow_atf	"0"	
            clterr	0	
            config_url	"http://10.1.59.138/"	
            gm_url	""	
            home_url	"http://10.1.59.138/views/main/"	
            isnew	1	
            mini	"0"	
            mini_url	"http://ayzs.8090yxs.com/other/0523/index.html"	
            msg	0	
            newuser	0	
            paygift_url	"http://10.1.59.138/paygift.php"	
            pay_url	"http://10.1.59.138/views/main/?wmd=pay&sid=1"	
            phonegiven	0	
            phone_url	"http://ayzs.8090yxs.com/other/0523/index.html"	
            platuid	"1"	
            safe	"1"	
            server_ip	"10.1.59.138"	
            server_port	"64999"	
            sid	"1"	
            skey	"da4d0282de00ecab396a88c0de47dc31_2015-04-29 16:37:50-900000078"	
                "http://10.1.59.138/dkclient/"	
            uid	900000078 [0x35a4e94e]	
            wallowIn_url	""	
        */
        //通过url获取角色详细信息
        private bool isfirst = true;
        private void _getPlyInfoBack(Variant data)
        {
            relogin_again = true;
            //DebugTrace.print("_getPlyInfoBack:"); 
            //DebugTrace.dumpObj( data );

            if (!data.ContainsKey("r"))
            {
                if (disconect.instance != null)
                    disconect.instance.setErrorType(disconect.ERROR_TYPE_NETWORK);
                return;
            }

            if (data["r"]._int == 1)
            {
                _curSvrPlyInfo = data["data"];
                Variant v = new Variant();
                v["paramters"] = _curSvrPlyInfo;
                notify(v);
                if (isfirst)
                {

                    initGame(_curSvrPlyInfo);
                    //GRClient.instance.getGraphCamera().visible = false;
                    isfirst = false;


                }
                else
                {
                    //第二次重连服务器，得到的数据
                    Variant msg = data["data"];
                    Variant value = new Variant();
                    Variant outgame = new Variant();
                    outgame["server_ip"] = msg["server_ip"];
                    outgame["uid"] = msg["uid"];
                    outgame["server_port"] = msg["server_port"];
                    outgame["token"] = msg["skey"];
                    outgame["clnt"] = 0;
                    value["outgamevar"] = outgame;
                    value["server_id"] = msg["sid"];
                    value["server_config_url"] = msg["config_url"];
                    value["mainConfig"] = "main";
                    conninfo.setInfo(value);


                }

            }
            else
            {//获取失败:封IP/账号或者区服关闭，会导致登录失败
                debug.Log(" GetPlyInfo failed :" + StringUtils.unicodeToStr(data["r"]._str));
                debug.Log(" GetPlyInfo failed :" + data.dump());
                //notify( {key:"pinfo_err", data:data} );	
                //if (login.instance!=null)
                //    login.instance.msg.show(true, "服务器维护中..");
                if (debug.instance != null)
                    debug.instance.showMsg(StringUtils.unicodeToStr(data["errmsg"]._str), 100);

                if (disconect.instance != null)
                    disconect.instance.setErrorType(disconect.ERROR_TYPE_SERVER);
            }

        }

        //private gameST _game;
        private void initGame(Variant data)
        {
            //Globle.game_CrossMono = new gameST();
            HttpAppMgr.init();
            Globle.game_CrossMono.init(
                data["config_url"]._str,//"http://10.1.8.76/do.php",	/*server_config_url,*/
                data["server_ip"]._str,//"10.1.59.138", /*server_ip, */
                data["sid"]._uint,//3, /*server_id, */
                data["server_port"]._uint,//62999,/*port, */
                data["uid"]._uint,//1,/*uid, */
                0,//data["clnt"]._uint,//0,/*clnt, */
                data["skey"]._str,//"123",/*token, */
                "main"//"main" /*mainConfig */
            );
        }


        //["static"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["skey"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["uid"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["config_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["server_ip"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["server_port"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["sid"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["newuser"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["isnew"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["clterr"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["adv_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["pay_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["paygift_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["home_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["mini"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["mini_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["gm_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["wallowIn_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["platuid"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["safe"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["allow_atf"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["msg"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["phone_url"]	{Cross.Variant}	System.Collections.DictionaryEntry
        //["phonegiven"]	{Cross.Variant}	System.Collections.DictionaryEntry


        //info{ key:, data:}
        private void notify(Variant info)
        {
            debug.Log("================================>");
            debug.Log(info.dump());
            if (_notifyFun == null) return;
            _notifyFun(info);
        }

        public void sendLogin(string login_url)
        {
            HttpAppMgr.POSTSvr(login_url, "", _getPlyInfoBack, true, "GET");
        }

        private string SID_UID = "";
        private int m_lastLogAPTime = 0;
        public void firstAnalysisPoint(uint server_id, uint uid)
        {
            SID_UID = "sid=" + server_id.ToString() + "&uid=" + uid.ToString();

            //POST参数（以下参数必须要有，值可以为空，但是参数变量需要传递）
            //platid：玩家平台id
            //sid：登录服务器sid
            //uid：游戏uid
            //platuid：平台uid
            //ver：第一次登录的游戏版本号
            //deviceid：手机的唯一标识符(例如 c4a9ce8d0aa06b29cecad665f0598791)
            //model：手机型号（例如 Xiaomi MI 4W）
            //opsys：操作系统(例如 Android OS 4.4.4 / API-19 (KTU84P/5.10.22))
            //resolution：分辨率(例如 1920x1080)
            //video：显卡设备 (例如 Adreno (TM) 320)
            //v_changshang：显卡厂商(例如 ARM)
            //v_drivers：显卡驱动版本(例如 OpenGL ES 3.0 V@66.0 AU@04.04.04.090.052 (CL@))
            //v_memory：显存(例如 1024m)
            //cpu：CPU类型(例如 ARMv7 VFPv3 NEON)
            //c_core：CPU处理核数(例如 4)

            string strmsg = "platid=" + Globle.YR_srvlists__platform;  //????????????
            strmsg += "&sid=" + server_id.ToString();
            strmsg += "&uid=" + uid.ToString();
            strmsg += "&platuid=" + Globle.YR_srvlists__platuid;
            strmsg += "&ver=" + Globle.QSMY_game_ver;

            strmsg += "&deviceid=" + SystemInfo.deviceUniqueIdentifier;
            strmsg += "&model=" + SystemInfo.deviceModel;
            strmsg += "&opsys=" + SystemInfo.operatingSystem;
            strmsg += "&resolution=" + Screen.width.ToString() + "x" + Screen.height.ToString();
            strmsg += "&video=" + SystemInfo.graphicsDeviceName;
            strmsg += "&v_changshang=" + SystemInfo.graphicsDeviceVendor;
            strmsg += "&v_drivers=" + SystemInfo.graphicsDeviceVersion;
            strmsg += "&v_memory=" + SystemInfo.graphicsMemorySize;
            strmsg += "&cpu=" + SystemInfo.processorType;
            strmsg += "&c_core=" + SystemInfo.processorCount;

            //debug.Log("上传设备数据 " + strmsg);
            // POSTSvr("http://10.1.8.60/do.php?device", strmsg, _getLogAPBack);

            if (Globle.curServerD != null && Globle.curServerD.login_url != null)
            {
              //  HttpAppMgr.POSTSvr(Globle.curServerD.login_url + "?device", strmsg, _getLogAPBack);
            }

            strmsg = null;
        }

        //public void logSDKCustomAP(string id)
        //{
        //    //新的埋点策略
        //    string event_name = "qsmy_" + id;
        //    string gatajsonString = "{\"portname\":\"setEvent\",\"identifier\":\"" + event_name + "\"}";
        //    AnyPlotformSDK.Call_Cmd("gataroleplatform", "lanGaiya", gatajsonString);
        //}

        ////设置SDK的埋点
        //public void logSDKAP(string pname)
        //{
        //    //在埋点中更新相关的角色信息
        //    //string exitRoleInfoJsonString = "{\"roleId\":\"1\",\"roleName\":\"chasname\",\"roleLevel\":\"101\",\"roleGold\":\"300\",\"roleYb\":\"200\",\"roleServerId\":\"1\",\"roleServerName\":\"servername\"}";
        //    //AnyPlotformSDK.Add_moreCmdInfo("lanRole", exitRoleInfoJsonString);

        //    try
        //    {
        //        Variant v = new Variant();
        //        v["roleId"] = PlayerModel.getInstance().cid;
        //        v["roleName"] = PlayerModel.getInstance().name;
        //        v["roleLevel"] = PlayerModel.getInstance().lvl;
        //        v["roleGold"] = PlayerModel.getInstance().money;
        //        v["roleYb"] = PlayerModel.getInstance().gold;
        //        v["roleServerId"] = Globle.curServerD.sid;
        //        v["roleServerName"] = Globle.curServerD.server_name;
        //        string exitRoleInfoJsonString = JsonManager.VariantToString(v);
        //        AnyPlotformSDK.Add_moreCmdInfo("lanRole", exitRoleInfoJsonString);

        //        string gatajsonString = "{\"portname\":\"" + pname + "\"}";
        //        //string gatajsonString = "{\"portname\":\"" + pname + "\",\"identifier\":\"create-role-begin\"}";
        //        AnyPlotformSDK.Call_Cmd("gataroleplatform", "lanGaiya", gatajsonString);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogWarning(pname);
        //    }
        //}

        ////获取埋点的返回
        //private void _getLogAPBack(Variant data)
        //{
        //    debug.Log("http = " + data.dump());
        //}
        public void loadServerList()
        {
            //AndroidSDKManager.actFlag = true;
            //DebugTrace.print("============= test act ===================");

            if (Globle.QSMY_Platform_Index == ENUM_QSMY_PLATFORM.QSPF_None)
            {

                //内网服务器的列表地址
              //  HttpAppMgr.POSTSvr("http://10.1.8.76/ifacec/sdk_srvlists.php", "platform=" + Globle.YR_srvlists__platform + "&sign=&platuid=" + Globle.YR_srvlists__platuid, _getSeverListBack);
            }
            else
            {
                //外网平台的列表地址
                // POSTSvr("http://qsmy-mobile.8090mt.com/qsmy_mobile/ifacec/sdk_srvlists.php", "platform=" + Globle.YR_srvlists__platform + "&sign=&platuid=" + Globle.YR_srvlists__platuid, _getSeverListBack);

                //HttpAppMgr.POSTSvr("http://123.59.51.193/ifacec/sdk_srvlists.php", "platform=" + Globle.YR_srvlists__platform + "&sign=" + Globle.YR_srvlists__sign + "&platuid=" + Globle.YR_srvlists__platuid, _getSeverListBack);
                //debug.Log("http://123.59.51.193/ifacec/sdk_srvlists.php?platform=" + Globle.YR_srvlists__platform + "&sign=" + Globle.YR_srvlists__sign + "&platuid=" + Globle.YR_srvlists__platuid);

                HttpAppMgr.POSTSvr(Globle.YR_srvlists__slurl, "platform=" + Globle.YR_srvlists__platform + "&sign=" + Globle.YR_srvlists__sign + "&platuid=" + Globle.YR_srvlists__platuid + "&ver=1.0.1", _getSeverListBack);
                debug.Log("sl_url = " + Globle.YR_srvlists__slurl);
                //debug.Log(Globle.YR_srvlists__slurl + Globle.YR_srvlists__platform + "&sign=" + Globle.YR_srvlists__sign + "&platuid=" + Globle.YR_srvlists__platuid);
            }
        }

        ////进入游戏之前发送PHP请求
        //private  void POSTSvr(string query, string param, Action<Variant> cb, bool rcvJSONHandler = true, string method="POST")
        //{
        //    debug.Log("http = " + query);
        //    ////这里要改成异步的才能继续
        //    //return;
        //    if( query == null || query == "" || cb == null) return;
        //    IURLReq  urlReq =  os.net.CreateURLReq(null);
        //    urlReq.url = query;
        //    urlReq.contentType = NetConst.URL_CONTENT_TYPE_URLENCODE;
        //    urlReq.dataFormat = NetConst.URL_DATA_FORMAT_TEXT;

        //    string data = "";
        //    data += param;
        //    DebugTrace.print(" POSTSvr query:" + query + "\n param:" + param);
        //    urlReq.data = data;

        //    //DebugTrace.dumpObj("POSTSvr data:" + data);

        //    urlReq.method = method;


        //    urlReq.load(
        //        //delegate(IURLReq r, byte[] vari)
        //        delegate(IURLReq r, object vari)
        //        {
        //            if( vari == null )
        //            {
        //                 DebugTrace.print(" POSTSvr urlReq.load vari Null!"  );
        //            } 
        //            string str = vari as string; 

        //            DebugTrace.print(" POSTSvr urlReq.loaded str["+str+"]!"  );

        //            Variant t = JsonManager.StringToVariant(str);

        //            if (cb != null)
        //                cb(JsonManager.StringToVariant(str)); 

        //        }, 
        //        null, 
        //        null
        //    );

        //}
        // ==== objs ====
        protected connInfo conninfo
        {
            get
            {
                return Globle.game_CrossMono.m.g_netM.getObject(OBJECT_NAME.DATA_CONN) as connInfo;
            }
        }
    }
}
