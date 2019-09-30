using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MuGame
{


    class LGOutGame : lgGDBase, IObjectPlugin
    {
        public LGOutGame(muLGClient m)
            : base(m)
        {

        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new LGOutGame(m as muLGClient);
        }

        //private switchParam _param = new switchParam();
        private uint _selectCharCid = 0;

        private bool _setConnFlag = false;
        private bool _loadMinResourceFlag = false;
        private bool _selectSidFalg = false;

        static public LGOutGame instance;
        override public void init()
        {
            addEventListener(GAME_EVENT.GAME_INIT_START, onStart);

            (g_mgr.g_netM as muNetCleint).addEventListenerCL(OBJECT_NAME.DATA_CONN, GAME_EVENT.CONN_ED, onConnected);
            g_mgr.g_netM.addEventListenerCL(OBJECT_NAME.DATA_CONN, GAME_EVENT.CONN_SET, onConnSet);
            g_mgr.g_netM.addEventListenerCL(OBJECT_NAME.DATA_CHARS, GAME_EVENT.ON_LOGIN, onLogin);

            g_mgr.g_gameM.addEventListenerCL(OBJECT_NAME.LG_LOAD_RESOURCE, GAME_EVENT.ON_LOAD_MIN, onLoadMin);


            g_mgr.g_netM.addEventListener(GAME_EVENT_DEFAULT.CONN_CLOSE, onConnectLost);

            g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_CREATE_CHAR, createChar);
            g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_DELETE_CHAR, deleteChar);
            g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_SELECT_CHAR, selectChar);
            g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ACT_ENTER_GAME, actEnterGame);

            g_mgr.g_uiM.addEventListener(UI_EVENT.UI_ON_SELECT_SID, onSelectSid);

            instance = this;
        }


        private void onSelectSid(GameEvent e)
        {
            _selectSidFalg = true;
        }

        public void onConnectLost(GameEvent e)
        {
            //断开链接了
            _setConnFlag = false;
            _selectSidFalg = false;

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.DISCONECT);
        }

        public void onStart(GameEvent e)
        {
            if (!_setConnFlag) _connect();
        }

        public void reStart()
        {
            _connect();
        }

        private void removeListener()
        {
            addEventListener(GAME_EVENT.GAME_INIT_START, onStart);

            g_mgr.g_netM.removeEventListenerCL(OBJECT_NAME.DATA_CONN, GAME_EVENT.CONN_ED, onConnected);
            g_mgr.g_netM.removeEventListenerCL(OBJECT_NAME.DATA_CONN, GAME_EVENT.CONN_SET, onConnSet);
            g_mgr.g_netM.removeEventListenerCL(OBJECT_NAME.DATA_CHARS, GAME_EVENT.ON_LOGIN, onLogin);




            g_mgr.g_gameM.removeEventListenerCL(OBJECT_NAME.LG_LOAD_RESOURCE, GAME_EVENT.ON_LOAD_MIN, onLoadMin);


            g_mgr.g_uiM.removeEventListener(UI_EVENT.UI_ACT_CREATE_CHAR, createChar);
            g_mgr.g_uiM.removeEventListener(UI_EVENT.UI_ACT_DELETE_CHAR, deleteChar);
            g_mgr.g_uiM.removeEventListener(UI_EVENT.UI_ACT_SELECT_CHAR, selectChar);
            g_mgr.g_uiM.removeEventListener(UI_EVENT.UI_ACT_ENTER_GAME, actEnterGame);

            g_mgr.g_uiM.removeEventListener(UI_EVENT.UI_ON_SELECT_SID, onSelectSid);

        }
        private void createChar(GameEvent e)
        {
            //sendTpkg(PKG_CMD.TYPG_CRATE_CHAR, data);
            (this.g_mgr.g_netM as muNetCleint).outGameMsgsInst.createCha(
                e.data["name"]._str,
                e.data["carr"]._uint,
                e.data["sex"]._uint
            );
        }
        private void deleteChar(GameEvent e)
        {
            //sendTpkg(PKG_CMD.TYPG_DELETE_CHAR, data);
            (this.g_mgr.g_netM as muNetCleint).outGameMsgsInst.deleteCha(e.data["cid"]);
        }
        private void selectChar(GameEvent e)
        {
            Variant data = e.data;
            _selectCharCid = data["cid"];
        }

        private void actEnterGame(GameEvent e)
        {
            if (_selectCharCid <= 0) return;

            //sendTpkg( PKG_NAME.TYPG_SELECT_CID, data );
            (this.g_mgr.g_netM as muNetCleint).outGameMsgsInst.selectCha(_selectCharCid);

            (this.g_mgr.g_uiM as muUIClient).onTryEnterGame();
            //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ON_TRY_ENTER_GAME, this, null));
        }

        private bool _open_sec_flag = false;
        private void onLoadMin(GameEvent e)
        {
            _loadMinResourceFlag = true;
            //UILoading.loading.showLoading = false;
            //UILoading.loading.showring(false);

            //InterfaceMgr.getInstance().destory(InterfaceMgr.BEGIN_LOADING);
            //debug.Log("关闭开始的加载界面");

            //if (AndroidSDKManager.actFlag && !_open_sec_flag)
            //{
            //    _open_sec_flag = true;
            //    LGUISelectArea lgsec = this.g_mgr.g_uiM.getLGUI(UIName.UI_SelectArea) as LGUISelectArea;
            //    lgsec.sid = LGPlatInfo.inst.sid;
            //    lgsec.sv_list = LGPlatInfo.inst.svlist;
            //    lgsec.open_sec();
            //}
            //else
            //{
            //    tryOpenUI();
            //}

            tryOpenUI();
        }

        private void onLogin(GameEvent e)
        {
            //if (!AndroidSDKManager.actFlag)
            //{
            //    tryOpenUI();
            //}
            //else
            //{
            //    if (_selectSidFalg) tryOpenUI();
            //}

            tryOpenUI();
        }


        private void tryOpenUI()
        {
            if (_selectCharCid > 0)
            {//重新连接、直接进去
                (this.g_mgr.g_netM as muNetCleint).outGameMsgsInst.selectCha(_selectCharCid);
                (this.g_mgr.g_uiM as muUIClient).onTryEnterGame();
                //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ON_TRY_ENTER_GAME, this, null));
                return;
            }
            if (!_loadMinResourceFlag) return;

            if ((g_mgr.g_netM as muNetCleint).charsInfoInst.getChas() == null)
            {
                return;
            }

            //a3为多角色
            if (login.instance != null)
            {
                login.instance.onBeginLoading((Action)(() =>
                {
                    InterfaceMgr.getInstance().ui_async_open((string)InterfaceMgr.A3_SELECTCHA);
                }));
            }
            else
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SELECTCHA);
            }

            //Variant data = new Variant();
            //if ((this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_CHARS) as charsInfo).getChas().Count <= 0)
            //{//creat
            //    //data["name"] = UIName.UI_CREATE_CHAR;
            //    //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT_DEFAULT.UI_OPEN, this, data));

            //    if (login.instance != null)
            //        login.instance.onBeginLoading(() =>
            //        {
            //            InterfaceMgr.getInstance().open(InterfaceMgr.A3_CREATECHA);
            //        });
            //    else
            //        InterfaceMgr.getInstance().open(InterfaceMgr.A3_CREATECHA);
            //}
            //else
            //{//select

            //    if (login.instance != null && Globle.DebugMode == 0)
            //    {
            //        Variant chas = muNetCleint.instance.charsInfoInst.getChas();
            //        uint cid = chas[0]["cid"]._uint;
            //        UIClient.instance.dispatchEvent(
            //        GameEvent.Create(UI_EVENT.UI_ACT_SELECT_CHAR, this, GameTools.createGroup("cid", cid)));
            //        UIClient.instance.dispatchEvent(
            //        GameEvent.Create(UI_EVENT.UI_ACT_ENTER_GAME, this, GameTools.createGroup("cid", cid)));

            //        // InterfaceMgr.getInstance().open(InterfaceMgr.SELECT_CHAR);
            //    }
            //    else
            //    {
            //        if (login.instance != null)
            //        {
            //            login.instance.onBeginLoading(() =>
            //            {
            //                InterfaceMgr.getInstance().open(InterfaceMgr.A3_SELECTCHA);
            //            });
            //        }
            //        else
            //        {
            //            InterfaceMgr.getInstance().open(InterfaceMgr.A3_SELECTCHA);
            //        }
            //    }
                   
            //    //data["name"] = UIName.UI_SELECT;
            //    //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT_DEFAULT.UI_OPEN, this, data));
            //}
        }


        private void onConnSet(GameEvent e)
        {
            _connect();
        }

        private void onConnected(GameEvent e)
        {
            this.g_mgr.g_netM.reqServerVersion();
        }
        private void _connect()
        {
            _setConnFlag = true;
            connInfo info = this.g_mgr.g_netM.getObject(OBJECT_NAME.DATA_CONN) as connInfo;

            bool ipv6 = false;
            IPAddress[] address = Dns.GetHostAddresses(info.server_ip);
            if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipv6 = true;
            }

            this.g_mgr.g_netM.connect(
                           info.server_ip,
                           info.server_port,
                           info.uid,
                           info.token,
                           info.clnt,
                           info.keyt,
                           ipv6
                       );

            debug.Log("链接服务器" + "server_id=" + info.server_ip + " server_port=" + info.server_port + " uid=" + info.uid + " ipv6" + ipv6);
        }

    }
}
