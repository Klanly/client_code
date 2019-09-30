using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;
namespace MuGame
{
    class choose_server : LoadingUI
    {
        private GameObject goTemp;
        private List<ServerItem> lserver;
        private Transform transCon;

        private List<AreaItem> lArea;

        private GameObject last_Temp;
        private ServerItem last_server;

        Button btClose;

        private float w;
        private float h;
        public override void init()
        {
            lArea = new List<AreaItem>();
            goTemp = this.getGameObjectByPath("tempAreaBt");
            last_Temp = this.getGameObjectByPath("s_go");
            btClose = this.getComponentByPath<Button>("imgclose");
            btClose.onClick.AddListener(onClose);
            RectTransform rect = goTemp.GetComponent<RectTransform>();
            w = rect.sizeDelta.x;
            h = rect.sizeDelta.y;
            goTemp.SetActive(false);
            transCon = transform.FindChild("info/con");
            lserver = new List<ServerItem>();
            for (int i = 0; i < 10; i++)
            {
                ServerItem si = new ServerItem(transform.FindChild("s/s" + i), onServerClick);
                lserver.Add(si);
                si.visiable = false;
            }
            last_server = new ServerItem(last_Temp.transform, onServerClick);

            initList();
            //changeArea(0);
        }

        public override void onShowed()
        {
            //默认选中最新的记录
            List<ServerData> l = Globle.lServer;
            if(l.Count > 0)
                changeArea((l.Count - 1) / 10);

            int sid = 0;
            if (PlayeLocalInfo.checkKey(PlayeLocalInfo.LOGIN_SERVER_SID))
                sid = PlayeLocalInfo.loadInt(PlayeLocalInfo.LOGIN_SERVER_SID);
            last_server.visiable = false;
            if(sid != 0 && Globle.dServer.ContainsKey(sid))
            {
                last_server.setData(Globle.dServer[sid]);
                last_server.visiable = true;
            }
        }

        public void initList()
        {
            lArea.Clear();
            for (int i = 0; i < transCon.childCount; i++)
            {
                GameObject.Destroy(transCon.GetChild(i).gameObject);
            }
            int len = 1;
            List<ServerData> l = Globle.lServer;
            if (l.Count % 10 == 0)
                len = l.Count / 10;
            else
                len = l.Count / 10 + 1;
            for (int i = 0; i < len; i++)
            {
                GameObject go = GameObject.Instantiate(goTemp) as GameObject;
                go.transform.SetParent(transCon, false);
                go.SetActive(true);
                AreaItem item = new AreaItem(go.transform, onAreaClick, i);
                lArea.Add(item);
            }

            RectTransform rect = transCon.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, len * h);
        }

        private AreaItem lastArea;
        public void changeArea(int idx)
        {
            if (lastArea != null)
                lastArea.bt.interactable = true;

            lastArea = lArea[idx];
            lastArea.bt.interactable = false;
            refeshServers(idx * 10);
        }

        public void refeshServers(int begin)
        {
            List<ServerData> l = Globle.lServer;
            if (begin >= l.Count)
                return;

            for (int i = 0; i < 10; i++)
            {
                if (begin + i >= l.Count)
                    lserver[i].visiable = false;
                else
                    lserver[i].setData(l[begin + i]);
            }
        }

        private void onClose()
        {

            InterfaceMgr.getInstance().close(InterfaceMgr.SERVE_CHOOSE);
        }

        public void onAreaClick(int idx)
        {
            changeArea(idx);
        }

        public void onServerClick(ServerData d)
        {
            login.instance.setServer(d);
            InterfaceMgr.getInstance().close(InterfaceMgr.SERVE_CHOOSE);
        }

        public override void dispose()
        {
            btClose.onClick.RemoveAllListeners();
            btClose = null;
            transCon = null;
            goTemp = null;
            foreach (ServerItem item in lserver)
            {
                item.dispose();
            }
            lserver.Clear();
            foreach (AreaItem item in lArea)
            {
                item.dispose();
            }
            lArea.Clear();
            base.dispose();
        }
    }

    class AreaItem : Skin
    {
        public int _idx;
        public Action<int> _handle;

        public Button bt;
        public AreaItem(Transform trans, Action<int> handle, int idx)
            : base(trans)
        {
            _idx = idx;
            _handle = handle;
            init();
        }

        void init()
        {
            bt = __mainTrans.GetComponent<Button>();
            bt.onClick.AddListener(onClick);

            this.getComponentByPath<Text>("Text").text = _idx * 10 + 1 + "-" + (_idx*10+10);
        }

        void onClick()
        {
            if (_handle != null)
                _handle(_idx);
        }

        public void dispose()
        {
            _handle = null;
            bt.onClick.RemoveAllListeners();
        }
    }

    class ServerItem : Skin
    {
        public GameObject iconClose;
        public GameObject iconTj;
        public GameObject iconNew;

        public ServerData serverData;
        public Action<ServerData> _handle;

        public Text txt;
        public Button _bt;
        public ServerItem(Transform trans, Action<ServerData> handle)
            : base(trans)
        {
            _handle = handle;
            init();
        }

        void init()
        {
            _bt = __mainTrans.GetComponent<Button>();
            _bt.onClick.AddListener(onClick);
            txt = this.getComponentByPath<Text>("Text");

            iconClose = this.getGameObjectByPath("iconClose");
            iconTj = this.getGameObjectByPath("iconTj");
            iconNew = this.getGameObjectByPath("iconNew");
            hideIcon();
        }

        public void hideIcon()
        {
            iconClose.SetActive(false);
            iconTj.SetActive(false);
            iconNew.SetActive(false);
        }

        public void setData(ServerData d)
        {
            if (d == null)
            {
                visiable = false;
                return;
            }

            visiable = true;
            serverData = d;
            txt.text = d.server_name;

            iconNew.SetActive(false);
            iconClose.SetActive(false);
            iconTj.SetActive(false);
            _bt.interactable = true;
            if (d.close)
            {
                iconClose.SetActive(true);
                _bt.interactable = false;
            }
            else if (d.srvnew)
            {
                iconNew.SetActive(true);
            }
            else if (d.recomm)
            {
                iconTj.SetActive(true);
            }

            for (int i = 1; i <= 5; i++)
            {
                if(i == d.srv_status)
                    this.transform.FindChild("sevstate/" + i).gameObject.SetActive(true);
                else
                    this.transform.FindChild("sevstate/" + i).gameObject.SetActive(false);
            }

        }

        void onClick()
        {
            if (_handle != null && serverData != null)
                _handle(serverData);
        }

        public void dispose()
        {
            _bt.onClick.RemoveAllListeners();
            _bt = null;
            _handle = null;
        }

        public override bool visiable
        {
            get
            {
                return base.visiable;
            }
            set
            {
                base.visiable = value;
                if (base.visiable == false)
                    serverData = null;
            }
        }

    }
}
