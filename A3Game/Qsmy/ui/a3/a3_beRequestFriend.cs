using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class a3_beRequestFriend :Window//可能存在多个申请需要弹出对应个数的UI面板
    {
        public static a3_beRequestFriend mInstance;
        GameObject itemPrefab;
        Dictionary<uint,itemBeRequestFriend> beRequestFriendList;
        RectTransform contentParent;
        public override bool showBG
        {
            get
            {
                return false;
            }
        }
        public override void init()
        {
            inText();
            beRequestFriendList = new Dictionary<uint, itemBeRequestFriend>();
            mInstance = this;
            contentParent = transform.FindChild("body/main/content").GetComponent<RectTransform>();
            itemPrefab = transform.FindChild("prefabs/itemBeRequestFirend").gameObject;
            BaseButton btnClose = new BaseButton(transform.FindChild("body/title/btnClose"));
            btnClose.onClick = onBtnCloseClick;
            BaseButton btnRefuse = new BaseButton(transform.FindChild("body/bottom/btnRefuse"));
            btnRefuse.onClick = onBtnRefuseClick;
            BaseButton btnAgreen = new BaseButton(transform.FindChild("body/bottom/btnAgreen"));
            btnAgreen.onClick = onBtnAgreenClick;
            

        }


        void inText()
        {
            this.transform.FindChild("body/bg/texts/txtNiceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_1");
            this.transform.FindChild("body/bg/texts/txtLevel").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_2");
            this.transform.FindChild("body/title/txtTitle").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_3");
            this.transform.FindChild("body/bottom/btnRefuse/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_4");
            this.transform.FindChild("body/bottom/btnAgreen/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_5");
            this.transform.FindChild("prefabs/itemBeRequestFirend/btnAgreen/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_6");
            this.transform.FindChild("prefabs/itemBeRequestFirend/btnRefuse/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_beRequestFriend_7");
        }
        public override void onClosed()
        {
            foreach (KeyValuePair<uint, itemBeRequestFriend> item in beRequestFriendList)
            {
                GameObject.Destroy(beRequestFriendList[item.Key].root.gameObject);
            }
            beRequestFriendList.Clear();
            FriendProxy.getInstance().requestFirendList.Clear();
        }
        void  onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BEREQUESTFRIEND);
        }
        void  onBtnRefuseClick(GameObject go)
        {
            Dictionary<uint ,itemFriendData> requestFriendList = FriendProxy.getInstance().requestFirendList;
            foreach (KeyValuePair<uint, itemFriendData> item in requestFriendList)
            {
                FriendProxy.getInstance().sendRefuseAddFriend(item.Key);
            
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BEREQUESTFRIEND);
        }

        void onBtnAgreenClick(GameObject go)
        {
            Dictionary<uint ,itemFriendData> requestFriendList = FriendProxy.getInstance().requestFirendList;
            foreach (KeyValuePair<uint, itemFriendData> item in requestFriendList)
            {
                FriendProxy.getInstance().sendAgreeApplyFriend(item.Key);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BEREQUESTFRIEND);
        }

        override public void onShowed()
        {
            Dictionary<uint ,itemFriendData> requestFriendList = FriendProxy.getInstance().requestFirendList;
            foreach (KeyValuePair<uint,itemFriendData> item in requestFriendList)
            {
                itemBeRequestFriend ibrf = new itemBeRequestFriend(itemPrefab.transform, requestFriendList[item.Key]);
                ibrf.root.SetParent(contentParent.transform);
                ibrf.root.localScale = Vector3.one;
                beRequestFriendList.Add(requestFriendList[item.Key].cid, ibrf);
            }
            contentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70 * requestFriendList.Count);
        }
        class itemBeRequestFriend
        {
           public Transform root;
            Text txtName;
            Text txtZhuan;
            uint cid = 0;
            string mName = string.Empty;
            GameObject  _go;
            public itemBeRequestFriend(Transform trans, itemFriendData data)
            {
                _go = GameObject.Instantiate(trans.gameObject) as GameObject;
                root = _go.transform;
                txtName = _go.transform.FindChild("txtName").GetComponent<Text>();
                txtZhuan = _go.transform.FindChild("txtZhuan").GetComponent<Text>();
       
                BaseButton btnRefuse = new BaseButton(_go.transform.FindChild("btnRefuse"));
                btnRefuse.onClick = onBtnRefuse;
                BaseButton btnAgreen = new BaseButton(_go.transform.FindChild("btnAgreen"));
                btnAgreen.onClick = onBtnAgreen;


                cid = (uint)data.cid;
                string name = data.name;
                mName = name;
                string zhuan = data.zhuan.ToString();
                string lvl = data.lvl.ToString();
                txtName.text = name;
                txtZhuan.text = string.Format(txtZhuan.text, zhuan, lvl);
              
            }
          
            void onBtnRefuse(GameObject go)//拒绝好友请求
            {
                FriendProxy.getInstance().sendRefuseAddFriend(cid);
                Destroy(a3_beRequestFriend.mInstance.beRequestFriendList[cid].root.gameObject);
                a3_beRequestFriend.mInstance.beRequestFriendList.Remove(cid);

            }
            void onBtnAgreen(GameObject go)//同意被添加好友
            {
                FriendProxy.getInstance().sendAgreeApplyFriend(cid);
                if (FriendProxy.getInstance().requestFriendListNoAgree.Contains(mName))
                {
                    FriendProxy.getInstance().requestFriendListNoAgree.Remove(mName);
                }
                Destroy(a3_beRequestFriend.mInstance.beRequestFriendList[cid].root.gameObject);
                a3_beRequestFriend.mInstance.beRequestFriendList.Remove(cid);
            }
        }
    }
}
