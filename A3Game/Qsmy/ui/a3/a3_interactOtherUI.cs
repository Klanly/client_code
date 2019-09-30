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
    class a3_interactOtherUI:FloatUi
    {
        string playerName;
        uint cid;
        Text txt_name;
        public override void init()
        {
            inText();
            BaseButton bg = new BaseButton(transform.FindChild("bg"));
            bg.onClick = onBtnCloseBgClick;
            BaseButton btnClose = new BaseButton(transform.FindChild("btnClose"));
            btnClose.onClick = onBtnCloseBgClick;
            txt_name = transform.FindChild("txt_name").GetComponent<Text>();
            BaseButton btn_see = new BaseButton(transform.FindChild("buttons/btn_see"));
            btn_see.onClick = onBtnSeePlayerInfo;
            BaseButton btn_addFriend = new BaseButton(transform.FindChild("buttons/btn_addFriend"));
             btn_addFriend.onClick = onBtnAddFriendClick;
            BaseButton btn_pinvite = new BaseButton(transform.FindChild("buttons/btn_pinvite"));
            BaseButton btn_privateChat = new BaseButton(transform.FindChild("buttons/btn_privateChat"));
             btn_privateChat.onClick = onPrivateChatClick;
        }

        void inText()
        {
            this.transform.FindChild("buttons/btn_see/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_interactOtherUI_1");//查看
            this.transform.FindChild("buttons/btn_addFriend/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_interactOtherUI_2");//加为好友
            this.transform.FindChild("buttons/btn_pinvite/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_interactOtherUI_3");//组队邀请
            this.transform.FindChild("buttons/btn_privateChat/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_interactOtherUI_4");//私聊
        }
        public override void onShowed()
        {
            if (uiData != null) 
            {
                txt_name.text = uiData[0].ToString();
                playerName = uiData[0].ToString();
                cid = (uint)uiData[1];
                if (!transform.gameObject.activeSelf)
                    transform.gameObject.SetActive(true);
            }
        }
        void onBtnCloseBgClick(GameObject go)
        {
            if (transform.gameObject.activeSelf)
                transform.gameObject.SetActive(false);
        }
        void onBtnSeePlayerInfo(GameObject go)
        {
            if (transform.gameObject.activeSelf)
               transform.gameObject.SetActive(false);
            ArrayList arr = new ArrayList();
            arr.Add(cid);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);
        }
        void onBtnAddFriendClick(GameObject go)
        {
            if (transform.gameObject.activeSelf)
                transform.gameObject.SetActive(false);
                FriendProxy.getInstance().sendAddFriend(cid, playerName);
        }
        void onPrivateChatClick(GameObject go)
        {
            if (transform.gameObject.activeSelf)
                transform.gameObject.SetActive(false);
            a3_chatroom._instance.privateChat(playerName);
        }
    

    }
}
