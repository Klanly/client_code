using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
    class a3_friend : BaseShejiao
    {

        public static Transform Mytransform;
        public static a3_friend instance;
        public uint cidd;
        public Transform rotb;

        public Transform rotba;
        Action _acc = null;
        Text Heitex;
        Text FirToHei;
        public a3_friend(Transform trans) : base(trans) { init(); }
        public void init()
        {
            instance = this;
            inText();
            toggleGropFriendBase.mTransform = transform;
            toggleGropFriendBase.init();
            //BaseButton btnClose = new BaseButton(transform.FindChild("btnClose"));
            //btnClose.onClick = onBtnClose;
        }


        void inText()
        {
            this.transform.FindChild("mainBody/left/toggleGroup/togFriend/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_1");//好友
            this.transform.FindChild("mainBody/left/toggleGroup/togBlacklist/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_2");//黑名单
            this.transform.FindChild("mainBody/left/toggleGroup/togEnemy/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_3");//仇人
            this.transform.FindChild("mainBody/left/toggleGroup/togNearby/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_4");//附近

            this.transform.FindChild("mainBody/myFriendsPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_5");//昵称
            this.transform.FindChild("mainBody/myFriendsPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_6");//等级
            this.transform.FindChild("mainBody/myFriendsPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_7");//战力
            this.transform.FindChild("mainBody/myFriendsPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_8");//位置
            this.transform.FindChild("mainBody/myFriendsPanel/right/main/body/firendsCount/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_9");//好友数量 :
            this.transform.FindChild("mainBody/myFriendsPanel/right/bottom/Toggle/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_10");//隐藏离线
            this.transform.FindChild("mainBody/myFriendsPanel/right/bottom/btnClickOnceAddFriend/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_11");//征友
            this.transform.FindChild("mainBody/myFriendsPanel/right/bottom/btnAddFriend/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_12");//添加

            this.transform.FindChild("mainBody/blackListPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_5");//昵称
            this.transform.FindChild("mainBody/blackListPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_6");//等级
            this.transform.FindChild("mainBody/blackListPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_7");//战力
            this.transform.FindChild("mainBody/blackListPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_8");//位置
            this.transform.FindChild("mainBody/blackListPanel/right/bottom/btnAddBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_12");//添加
            this.transform.FindChild("mainBody/blackListPanel/right/main/body/blackListCount/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_13");//黑名单数量:

            this.transform.FindChild("mainBody/enemyPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_5");//昵称
            this.transform.FindChild("mainBody/enemyPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_6");//等级
            this.transform.FindChild("mainBody/enemyPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_7");//战力
            this.transform.FindChild("mainBody/enemyPanel/right/main/title/texts/txt_combat1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_14");//仇恨值
            this.transform.FindChild("mainBody/enemyPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_8");//位置
            this.transform.FindChild("mainBody/enemyPanel/right/main/body/enemyListCount/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_15");//仇人数量:
            this.transform.FindChild("mainBody/blackListPanel/right/bottom/btnAddBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_16");//查询

            this.transform.FindChild("mainBody/neighborPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_5");//昵称
            this.transform.FindChild("mainBody/neighborPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_6");//等级
            this.transform.FindChild("mainBody/neighborPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_7");//战力
            this.transform.FindChild("mainBody/neighborPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_8");//位置
            this.transform.FindChild("mainBody/neighborPanel/right/bottom/btnRefresh/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_17");//刷新
            this.transform.FindChild("mainBody/neighborPanel/right/main/body/nearbyListCount/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_18");//好友数量:

            this.transform.FindChild("mainBody/HidePanel/AddToBalckListWaring/body/Text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_friend_19"));//玩家{color="#ff0000"}{0},等级:{1}转{2}级{/color}当前是您的好友,{n}若将其加入黑名单,会自动解除好友关系,{n}是否确定将其加入黑名单 ?
            this.transform.FindChild("mainBody/HidePanel/AddToBalckListWaring/btnCancel/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_20");//取消
            this.transform.FindChild("mainBody/HidePanel/AddToBalckListWaring/btnOK/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_21");//添加

            this.transform.FindChild("itemPrefabs/itemFriend/btnAction/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_22");//操作
            this.transform.FindChild("itemPrefabs/itemBlackList/btnAction/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_22");//操作
            this.transform.FindChild("itemPrefabs/itemEnemy/Toggle/containts/btnAction/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_22");//操作
            this.transform.FindChild("itemPrefabs/itemNearbyFirend/btnAction/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_22");//操作
            this.transform.FindChild("itemPrefabs/itemAddFriend/btnAction/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_23");//查看装备

            this.transform.FindChild("itemPrefabs/actionPanel/buttons/btnChat/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_24");//私聊
            this.transform.FindChild("itemPrefabs/actionPanel/buttons/btnWatch/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_25");//查看
            this.transform.FindChild("itemPrefabs/actionPanel/buttons/btnTeam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_26");//邀请组队
            this.transform.FindChild("itemPrefabs/actionPanel/buttons/btnDelete/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_27");//删除好友
            this.transform.FindChild("itemPrefabs/actionPanel/buttons/btnBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_28");//黑名单

            this.transform.FindChild("itemPrefabs/actionNearybyPanel/buttons/btnChat/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_24");//私聊
            this.transform.FindChild("itemPrefabs/actionNearybyPanel/buttons/btnWatch/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_25");//查看
            this.transform.FindChild("itemPrefabs/actionNearybyPanel/buttons/btnTeam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_26");//邀请组队
            this.transform.FindChild("itemPrefabs/actionNearybyPanel/buttons/btnAdd/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_29");//添加好友
            this.transform.FindChild("itemPrefabs/actionNearybyPanel/buttons/btnBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_28");//黑名单

            this.transform.FindChild("itemPrefabs/actionBlackListPanel/buttons/btnAddFriend/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_30");//加为好友
            this.transform.FindChild("itemPrefabs/actionBlackListPanel/buttons/btnCancleBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_31");//取消黑名单

            this.transform.FindChild("itemPrefabs/actionEnemyPanel/buttons/btnAdd/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_29");//添加好友
            this.transform.FindChild("itemPrefabs/actionEnemyPanel/buttons/btnBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_28");//黑名单
            this.transform.FindChild("itemPrefabs/actionEnemyPanel/buttons/btnDelEnemy/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_32");//删除仇人


            this.transform.FindChild("mainBody/myFriendsPanel/hidPanels/personalsPanel/main/title/texts/txtNiceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_33");//暱稱
            this.transform.FindChild("mainBody/myFriendsPanel/hidPanels/personalsPanel/main/title/texts/txtLevel").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_34");//等級
            this.transform.FindChild("mainBody/myFriendsPanel/hidPanels/personalsPanel/main/title/texts/txtCareer").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_35");//職業
            this.transform.FindChild("mainBody/myFriendsPanel/hidPanels/addFriendsPanel/bg/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_36");//輸入玩家暱稱
            this.transform.FindChild("mainBody/enemyPanel/right/bottom/btnSearchPos/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_friend_37");//查詢
            getComponentByPath<Text>("mainBody/myFriendsPanel/hidPanels/personalsPanel/bottom/btnSelectAll/Text").text = ContMgr.getCont("quanxuan");

            ScrollControler scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("mainBody/myFriendsPanel/right/main/body/scroll"));
            ScrollControler scrollControer1 = new ScrollControler();
            scrollControer1.create(getComponentByPath<ScrollRect>("mainBody/blackListPanel/right/main/body/scroll"));
            ScrollControler scrollControer2 = new ScrollControler();
            scrollControer2.create(getComponentByPath<ScrollRect>("mainBody/enemyPanel/right/main/body/scroll"));
            ScrollControler scrollControer3 = new ScrollControler();
            scrollControer3.create(getComponentByPath<ScrollRect>("mainBody/neighborPanel/right/main/body/scroll"));

            rotb = this.transform.FindChild("mainBody/blackListPanel/hidPanels/addMyfiendPlane");
            Heitex = rotb.FindChild("bg/text").GetComponent<Text>();

            rotba = this.transform.FindChild("mainBody/myFriendsPanel/hidPanels/addMyfiendPToHei");
            FirToHei = rotba.FindChild("bg/text").GetComponent<Text>();

            BaseButton btn = new BaseButton(rotb.FindChild("bottom/btnCancle"));
            btn.onClick = HeiFlase;
            BaseButton btnn = new BaseButton(rotb.FindChild("bottom/btnAdd"));
            btnn.onClick = HeiTrue;
            BaseButton btnOneClick = new BaseButton(rotb.FindChild("title/btnClose"));
            btnOneClick.onClick = HeiFlase;
            BaseButton btan = new BaseButton(rotba.FindChild("bottom/btnCancle"));
            btan.onClick = Nobtn;
            BaseButton btad = new BaseButton(rotba.FindChild("bottom/btnAdd"));
            btad.onClick = Gohei;
            BaseButton btnClick = new BaseButton(rotba.FindChild("title/btnClose"));
            btnClick.onClick = Nobtn;
        }

        public override void onShowed()
        {
            toggleGropFriendBase.mFriendList.onShow();

            //toggleGropFriend._instance.togFriend.isOn = true;
        }

        void HeiFlase(GameObject go)
        {
            if (rotb.gameObject.activeSelf) rotb.gameObject.SetActive(false);

        }
        void HeiTrue(GameObject go)
        {
            string name = blackList._instance.txtAddBlackListName.text;
            if (!string.IsNullOrEmpty(name))
            {

                if (!friendList._instance.BlackListDataDic.ContainsKey(cidd))
                {
                    FriendProxy.getInstance().sendAddBlackList(0, name);
                    rotb.gameObject.SetActive(false);
                }

            }

        }
        public void ShowHei(itemFriendData itemData, Action ac = null)
        {
            if (blackList._instance.addBlackListPanel.gameObject.activeSelf) blackList._instance.addBlackListPanel.gameObject.SetActive(false);
            if (!rotb.gameObject.activeSelf) rotb.gameObject.SetActive(true);
            Heitex.text = ContMgr.getCont("uilayer_a3_friend_38", itemData.name);
            cidd = itemData.cid;
            if (_acc != null)
            {
                _acc = ac;
            }
        }

        public void MaxIns(string name)
        {
            if (!rotb.gameObject.activeSelf) rotb.gameObject.SetActive(true);
            Heitex.text = ContMgr.getCont("uilayer_a3_friend_38", name);
        }
        public void FiendToHei(itemFriendData itemData, Action ac = null)
        {
            if (actionPanelPrefab._instance.root.gameObject.activeSelf) actionPanelPrefab._instance.root.gameObject.SetActive(false);
            if (!rotba.gameObject.activeSelf) rotba.gameObject.SetActive(true);
            FirToHei.text = ContMgr.getCont("uilayer_a3_friend_38", itemData.name);
            cidd = itemData.cid;
            if (_acc != null)
            {
                _acc = ac;
            }
        }
        void Gohei(GameObject go)
        {
            itemFriendData ifd = new itemFriendData();
            uint cidTemp = actionPanelPrefab._instance.cid;
            ifd.cid = cidTemp;
            FriendProxy.getInstance().sendAddBlackList(cidTemp);
            if (rotba.gameObject.activeSelf) rotba.gameObject.SetActive(false);
        }
        void Nobtn(GameObject go)
        {
            if (rotba.gameObject.activeSelf) rotba.gameObject.SetActive(false);
        }
      
        public override void onClose()
        {
            toggleGropFriendBase.mFriendList.onClose();
        }
                
        void onBtnClose(GameObject gob)
        {
            foreach (KeyValuePair<uint, itemFriendData> item in friendList._instance.friendDataDic)
                item.Value.root.gameObject.SetActive(false);

            foreach (KeyValuePair<uint, itemFriendData> item in friendList._instance.BlackListDataDic)
                item.Value.root.gameObject.SetActive(false);

            foreach (KeyValuePair<uint, itemFriendData> item in friendList._instance.EnemyListDataDic)
                item.Value.root.gameObject.SetActive(false);

            foreach (KeyValuePair<uint, itemFriendData> item in friendList._instance.NearbyListDataDic)
                item.Value.root.gameObject.SetActive(false);
            foreach (KeyValuePair<uint, itemFriendData> item in friendList._instance.RecommendListDataDic)
                item.Value.root.gameObject.SetActive(false);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
        }

    }
    class friendList : toggleGropFriendBase//好友列表
    {
        public static friendList _instance;
        public Transform containt;

        public Transform addFriend;

        Transform addFriendsPanel;
        Toggle showOnlineFriends;
        //addFriendPanel
        InputField iptfAFPName;
        //personalsPanel
        Transform personalsPanel;
        public BaseButton btnAdd;
        Text friendLimit;
        public RectTransform contains;
        public Dictionary<uint, itemFriendData> friendDataDic = new Dictionary<uint, itemFriendData>();
        public Dictionary<uint, itemFriendData> BlackListDataDic = new Dictionary<uint, itemFriendData>();
        public Dictionary<uint, itemFriendData> EnemyListDataDic = new Dictionary<uint, itemFriendData>();
        public Dictionary<uint, itemFriendData> NearbyListDataDic = new Dictionary<uint, itemFriendData>();
        public Dictionary<uint, itemFriendData> RecommendListDataDic = new Dictionary<uint, itemFriendData>();
        // public Dictionary<uint, itemFriendData> RecommendListDataDic1 = new Dictionary<uint, itemFriendData>();
        public Dictionary<uint, Time> cdPostionTimer = new Dictionary<uint, Time>();

        public new void init()
        {
            _instance = this;
            root = mTransform.FindChild("mainBody/myFriendsPanel");
            BaseButton btnClickOnceAddFriend = new BaseButton(root.FindChild("right/bottom/btnClickOnceAddFriend"));
            BaseButton btnAddFriend = new BaseButton(root.FindChild("right/bottom/btnAddFriend"));
            showOnlineFriends = root.FindChild("right/bottom/Toggle").GetComponent<Toggle>();
            showOnlineFriends.onValueChanged.AddListener(onShowOnlineFriendsChanged);
            //personalsPanel
            personalsPanel = root.FindChild("hidPanels/personalsPanel");
            BaseButton btnSelectAll = new BaseButton(personalsPanel.FindChild("bottom/btnSelectAll"));
            btnAdd = new BaseButton(personalsPanel.FindChild("bottom/btnAdd"));
            BaseButton btnPersonalClose = new BaseButton(personalsPanel.FindChild("title/btnClose"));
            btnPersonalClose.onClick = onPersonalPanelClose;
            btnAdd.onClick = onAddAllSelectClick;
            btnSelectAll.onClick = onSelectAllClick;

            addFriendsPanel = root.FindChild("hidPanels/addFriendsPanel");
            BaseButton btnAFPAdd = new BaseButton(addFriendsPanel.transform.FindChild("bottom/btnAdd"));
            iptfAFPName = addFriendsPanel.transform.FindChild("main/InputField").GetComponent<InputField>();
            BaseButton btnAFPCancel = new BaseButton(addFriendsPanel.transform.FindChild("bottom/btnCancel"));
            BaseButton btnAFPClose = new BaseButton(addFriendsPanel.transform.FindChild("title/btnClose"));
            friendLimit = root.FindChild("right/main/body/firendsCount/count").GetComponent<Text>();
            containt = root.FindChild("right/main/body/scroll/contains");
            contains = containt.GetComponent<RectTransform>();

            addFriend = root.FindChild("hidPanels/personalsPanel/main/scroll/containts");

            btnClickOnceAddFriend.onClick = onBtnClickOnceAddFriendClick;
            btnAddFriend.onClick = onBtnAddFriendClick;
            btnAFPAdd.onClick = onBtnAdd;
            btnAFPCancel.onClick = onBtnAFPClose;
            btnAFPClose.onClick = onBtnAFPClose;

        }
        public void onShow()
        {
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_FRIENDLIST, onFriendList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_BLACKLIST, onBlackList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_ENEMYLIST, onEnemyList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_RECOMMEND, onRecommendList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_DELETEFRIEND, onDeleteFriend);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_RECEIVEADDBLACKLIST, onReceiveAddBlackList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_AGREEAPLYRFRIEND, onAgreeAplyFriend);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_DELETEENEMY, onDeleteEnemy);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_ENEMYPOSTION, onEnemyPostion);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_DELETEBLACKLIST, onDeleteBlackList);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_REMOVENEARYBY, onRemoveNearybyLeave);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_ADDNEARYBY, onAddNearbyPeople);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_REFRESHNEARYBY, onFreshNearby);

            FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);

            Dictionary<uint, itemFriendData> friendDataList = FriendProxy.getInstance().FriendDataList;
            friendLimit.text = friendDataList.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            //contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (60) * friendDataList.Count);
            //Debug.LogError(friendDataList.Count);
            //Debug.LogError((60) * friendDataList.Count);

        }
        public void onClose()
        {
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_FRIENDLIST, onFriendList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_BLACKLIST, onBlackList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_ENEMYLIST, onEnemyList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_RECOMMEND, onRecommendList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_DELETEFRIEND, onDeleteFriend);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_RECEIVEADDBLACKLIST, onReceiveAddBlackList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_AGREEAPLYRFRIEND, onAgreeAplyFriend);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_DELETEENEMY, onDeleteEnemy);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_ENEMYPOSTION, onEnemyPostion);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_DELETEBLACKLIST, onDeleteBlackList);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_REMOVENEARYBY, onRemoveNearybyLeave);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_ADDNEARYBY, onAddNearbyPeople);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_REFRESHNEARYBY, onFreshNearby);
        }
        void onBtnClickOnceAddFriendClick(GameObject go)
        {
            if (!personalsPanel.gameObject.activeSelf)
            {
                personalsPanel.gameObject.SetActive(true);
            }
            if (personalsPanel.transform.FindChild("main/scroll/containts").childCount > 0)
            {
                for (int i = 0; i < personalsPanel.transform.FindChild("main/scroll/containts").childCount; i++)
                {
                    GameObject.Destroy(personalsPanel.transform.FindChild("main/scroll/containts").GetChild(i).gameObject);
                }

            }
            FriendProxy.getInstance().sendOnlineRecommend();

        }
        void onBtnAddFriendClick(GameObject go)
        {
            if (!addFriendsPanel.gameObject.activeSelf) addFriendsPanel.gameObject.SetActive(true);
        }
        void onBtnAdd(GameObject go)
        {
            string name = iptfAFPName.text;
            if (!string.IsNullOrEmpty(name))
            {
                bool checkAddFriend = FriendProxy.getInstance().checkAddFriend(name);
                if (!checkAddFriend)
                {
                    FriendProxy.getInstance().sendAddFriend(0, name);
                }
                addFriendsPanel.gameObject.SetActive(false);
            }

        }
        void onBtnAFPClose(GameObject go)
        {
            addFriendsPanel.gameObject.SetActive(false);
        }
        void onPersonalPanelClose(GameObject go)
        {
            personalsPanel.gameObject.SetActive(false);
        }
        public void onShowOnlineFriendsChanged(bool b)
        {
            if (showOnlineFriends.isOn)
            {
                foreach (KeyValuePair<uint, itemFriendData> item in friendDataDic)
                {
                    if (!item.Value.online)
                    {
                        item.Value.root.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<uint, itemFriendData> item in friendDataDic)
                {
                    if (!item.Value.online)
                    {
                        item.Value.root.gameObject.SetActive(true);
                        item.Value.root.SetAsLastSibling();
                    }
                }
            }
        }
        void onSelectAllClick(GameObject go)
        {

            foreach (KeyValuePair<uint, itemFriendData> item in RecommendListDataDic)
            {
                item.Value.root.FindChild("Toggle").GetComponent<Toggle>().isOn = true;
            }
        }
        void onAddAllSelectClick(GameObject go)
        {
            personalsPanel.gameObject.SetActive(false);
            Dictionary<uint, itemFriendData> recommendDicTemp = new Dictionary<uint, itemFriendData>();
            recommendDicTemp.Clear();
            foreach (KeyValuePair<uint, itemFriendData> item in RecommendListDataDic)
            {
                if (item.Value.root.FindChild("Toggle").GetComponent<Toggle>().isOn)
                {
                    recommendDicTemp[item.Key] = item.Value;
                }
            }
            foreach (KeyValuePair<uint, itemFriendData> item in recommendDicTemp)
            {
                bool checkAddFriend = FriendProxy.getInstance().checkAddFriend(item.Value.name);
                if (!checkAddFriend)
                {
                    FriendProxy.getInstance().sendAddFriend(item.Key, item.Value.name);

                }
                if (friendList._instance.friendDataDic.ContainsKey(item.Key))
                {
                    continue;
                }
                if (!FriendProxy.getInstance().requestFriendListNoAgree.Contains(item.Value.name))
                {
                    FriendProxy.getInstance().requestFriendListNoAgree.Add(item.Value.name);
                }
            }

        }
        void onFriendList(GameEvent e)
        {
            float itemFDataHeight = 60.0f;
            Dictionary<uint, itemFriendData> friendDataList = FriendProxy.getInstance().FriendDataList;


            //List<uint> FriendCid = new List<uint>(FriendDataList.Keys);
            //for (int i = 0; i < FriendCid.Count; i++)
            //{
            //    for (int j = 0; j < FriendCid.Count; j++)
            //    {
            //        // FriendDataList[FriendCid[i]]
            //        if (FriendDataList[FriendCid[i]].online == false && FriendDataList[FriendCid[j]].online == true)
            //        {
            //            itemFriendData temp = FriendDataList[FriendCid[i]];
            //            FriendDataList[FriendCid[i]] = FriendDataList[FriendCid[j]];
            //            FriendDataList[FriendCid[j]] = temp;
            //        }
            //    }

            //}
            foreach (KeyValuePair<uint, itemFriendData> item in friendDataList)
            {
                if (friendDataDic.ContainsKey(item.Key))
                {
                    itemFriendData itemFData = friendDataDic[item.Key];
                    itemFData.itemFPrefab.set(item.Value);
                }
                else
                {
                    itemFriendData itemFData = item.Value;
                    itemFData.itemFPrefab = new itemFriendPrefab();
                    itemFData.itemFPrefab.init();
                    Transform rootT;
                    itemFData.itemFPrefab.show(item.Value, out rootT);
                    itemFData.root = rootT;
                    friendDataDic[item.Key] = itemFData;
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemFPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }
                }
            }
            onShowOnlineFriendsChanged(true);//如果选中只显示在线好友的情况下
            friendLimit.text = friendDataList.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight) * friendDataList.Count);
        }
        void onBlackList(GameEvent e)
        {
            float itemFDataHeight = 60;
            Dictionary<uint, itemFriendData> blackDataList = FriendProxy.getInstance().BlackDataList;
            foreach (var item in blackDataList)
            {
                if (BlackListDataDic.ContainsKey(item.Key))
                {
                    itemFriendData itemFData = BlackListDataDic[item.Key];
                    itemFData.itemBListPrefab.set(item.Value);
                }
                else
                {
                    itemFriendData itemFData = item.Value;
                    itemFData.itemBListPrefab = new itemBlackListPrefab();
                    itemFData.itemBListPrefab.init();
                    Transform rootT;
                    itemFData.itemBListPrefab.show(item.Value, out rootT);
                    itemFData.root = rootT;

                    BlackListDataDic[item.Key] = itemFData;
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemBListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }
                }
            }
            blackList._instance.blackListCount.text = blackDataList.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            blackList._instance.contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight) * blackDataList.Count);
        }

        void onEnemyList(GameEvent e)
        {
            float itemFDataHeight = 60.0f;
            Dictionary<uint, itemFriendData> enemyDataList = FriendProxy.getInstance().EnemyDataList;
            foreach (var item in enemyDataList)
            {
                if (EnemyListDataDic.ContainsKey(item.Key) && enemyList._instance.containt.transform.childCount!=0)
                {
                    itemFriendData itemFData = EnemyListDataDic[item.Key];
                    itemFData.onlinename = item.Value.onlinename;
                    itemFData.online = item.Value.online;
                    itemFData.limttm = item.Value.limttm;
                    long curTimer = muNetCleint.instance.CurServerTimeStamp;
                    if (itemFData.itemEListPrefab.isCdNow) //刷新位置坐标
                    {
                        if (itemFData.limttm >= curTimer)
                        {
                            FriendProxy.getInstance().sendEnemyPostion(item.Key);
                        }
                       
                    }
                }
                else
                {
                    itemFriendData itemFData = item.Value;
                    itemFData.itemEListPrefab = new itemEnemyListPrefab();
                    itemFData.itemEListPrefab.init();
                    Transform rootT;
                    itemFData.itemEListPrefab.show(item.Value, out rootT);
                    itemFData.root = rootT;
                    EnemyListDataDic[item.Key] = itemFData;
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemEListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }
                }
            }

            enemyList._instance.enemyListCount.text = enemyDataList.Count.ToString() + "/" +100/* (50 + 50 * PlayerModel.getInstance().up_lvl)*/;
            enemyList._instance.contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight) * enemyDataList.Count);
        }

        void onRecommendList(GameEvent e)
        {
            int i = 0;
            RecommendListDataDic.Clear();
            float itemFDataHeight = 60.0f;
            List<itemFriendData> recommendDataList = FriendProxy.getInstance().RecommendDataList;
            recommendDataList.Sort(SortItemFriendData);
            // Debug.LogError(RecommendListDataDic.Count + "cccc");          
            foreach (var item in recommendDataList)
            {
                //if (RecommendListDataDic.ContainsKey(item.cid))
                //{
                //    i++;
                //    itemFriendData itemFData = RecommendListDataDic[item.cid];
                //    itemFData.itemECListPrefab.set(item);
                //    if (i % 2 == 0)
                //    {
                //        itemFData.itemECListPrefab.root.transform.FindChild("bg2").gameObject.SetActive(false);
                //    }
                //}
                //else
                {
                    i++;

                    //if (!FriendProxy.getInstance().FriendDataList.ContainsKey(item.cid))
                    //{
                    itemFriendData itemFData = item;
                    itemFData.itemECListPrefab = new itemRecommendListPrefab();
                    itemFData.itemECListPrefab.init();
                    Transform rootT;
                    itemFData.itemECListPrefab.show(item, out rootT);
                    itemFData.root = rootT;
                    RecommendListDataDic[item.cid] = itemFData;
                    if (i % 2 == 0)
                    {
                        itemFData.itemECListPrefab.root.transform.FindChild("bg2").gameObject.SetActive(false);
                    }
                    //Debug.LogError(item.name);
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemECListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }

                    //}
                }

            }
            btnAdd.transform.GetComponent<Button>().interactable = true;
            RectTransform containtRect = recommendList._instance.containt.GetComponent<RectTransform>();
            containtRect.sizeDelta = new Vector2(containtRect.sizeDelta.x, itemFDataHeight * RecommendListDataDic.Count);
            // Debug.LogError(RecommendListDataDic.Count + "--" + FriendProxy.getInstance().RecommendDataList.Count + "--" + recommendDataList.Count);
        }
        void onAgreeAplyFriend(GameEvent e)
        {

            Dictionary<uint, itemFriendData> friendDataList = FriendProxy.getInstance().FriendDataList;
            foreach (KeyValuePair<uint, itemFriendData> item in friendDataList)
            {
                if (friendDataDic.ContainsKey(item.Key))
                {
                    itemFriendData itemFData = friendDataDic[item.Key];
                    itemFData.itemFPrefab.set(item.Value);
                }
                else
                {
                    itemFriendData itemFData = item.Value;
                    itemFData.itemFPrefab = new itemFriendPrefab();
                    itemFData.itemFPrefab.init();
                    Transform rootT;
                    itemFData.itemFPrefab.show(item.Value, out rootT);
                    itemFData.root = rootT;
                    friendDataDic[item.Key] = itemFData;
                }
                if (EnemyListDataDic.ContainsKey(item.Key))
                {
                    EnemyListDataDic[item.Key].itemEListPrefab.UpdatePos(item.Value.map_id);
                }
            }
            onShowOnlineFriendsChanged(true);//如果选中只显示在线好友的情况下
            friendLimit.text = friendDataList.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (60) * friendDataList.Count);
        }
        void onDeleteEnemy(GameEvent e)
        {
            uint cidTemp = e.data["cid"]._uint;
            if (friendList._instance.EnemyListDataDic.ContainsKey(cidTemp))
            {
                GameObject.Destroy(friendList._instance.EnemyListDataDic[cidTemp].root.gameObject);

                friendList._instance.EnemyListDataDic.Remove(cidTemp);
                enemyList._instance.enemyListCount.text = friendList._instance.EnemyListDataDic.Count.ToString() + "/" + 100/*(50 + 50 * PlayerModel.getInstance().up_lvl)*/;
            }
        }

        void onEnemyPostion(GameEvent e)
        {
            uint cidTemp = e.data["cid"]._uint;
            if (friendList._instance.EnemyListDataDic.ContainsKey(cidTemp))
            {
                string pos = getMapNameById((int)FriendProxy.getInstance().EnemyDataList[cidTemp].map_id);
                friendList._instance.EnemyListDataDic[cidTemp].root.FindChild("Toggle/containts/txtpos").GetComponent<Text>().text = pos;
                string linerod= "("+(FriendProxy.getInstance().EnemyDataList[cidTemp].onlinename + 1) + ContMgr.getCont("line")+")";
                friendList._instance.EnemyListDataDic[cidTemp].root.FindChild("Toggle/containts/txtpos/findline").GetComponent<Text>().text = linerod;
                itemFriendData ifd = friendList._instance.EnemyListDataDic[cidTemp];
                ifd.timer = 300.0f;
                friendList._instance.EnemyListDataDic[cidTemp] = ifd;
                if (friendList._instance.EnemyListDataDic[cidTemp].itemEListPrefab.isCdNow)
                {
                    friendList._instance.EnemyListDataDic[cidTemp].root.gameObject.SetActive(true);

                }
                else
                {
                    ifd.itemEListPrefab.refresShowPostion(300);
                }
            }
        }
        void onDeleteBlackList(GameEvent e)
        {
            uint cid = e.data["cid"]._uint;
            if (friendList._instance.BlackListDataDic.ContainsKey(cid))
            {
                GameObject.Destroy(friendList._instance.BlackListDataDic[cid].root.gameObject);
                friendList._instance.BlackListDataDic.Remove(cid);
                blackList._instance.blackListCount.text = friendList._instance.BlackListDataDic.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            }
        }
        void onRemoveNearybyLeave(GameEvent e)
        {
            uint cid = e.data["cid"]._uint;
            if (NearbyListDataDic.ContainsKey(cid))
            {
                NearbyListDataDic[cid].itemNListPrefab.root.gameObject.SetActive(false);
                GameObject.Destroy(NearbyListDataDic[cid].itemNListPrefab.root.gameObject);
                NearbyListDataDic.Remove(cid);
            }
        }

        void onAddNearbyPeople(GameEvent e)
        {
            float itemFDataHeight = 60.0f;
            uint iid = e.data["iid"]._uint;
            uint cid = 0;
            Dictionary<uint, ProfessionRole> m_mapOtherPlayerSee = OtherPlayerMgr._inst.m_mapOtherPlayerSee;
            if (m_mapOtherPlayerSee.ContainsKey(iid))
            {
                cid = m_mapOtherPlayerSee[iid].m_unCID;
            }

            ProfessionRole item = m_mapOtherPlayerSee[iid];
            itemFriendData itemFDTemp = new itemFriendData();
            itemFDTemp.cid = item.m_unCID;
            itemFDTemp.name = item.roleName;
            itemFDTemp.carr = (uint)item.m_roleDta.carr;
            itemFDTemp.lvl = item.lvl;
            itemFDTemp.zhuan = (uint)item.zhuan;
            itemFDTemp.clan_name = string.IsNullOrEmpty(item.clanName) ? ContMgr.getCont("FriendProxy_wu") : item.clanName;
            itemFDTemp.combpt = (uint)item.combpt;
            itemFDTemp.map_id = (int)PlayerModel.getInstance().mapid;

            if (NearbyListDataDic.ContainsKey(item.m_unCID))
            {
                itemFriendData itemFData = NearbyListDataDic[item.m_unCID];
                itemFData.itemNListPrefab.set(itemFDTemp);
                if (itemFDataHeight == 0)
                {
                    itemFDataHeight = itemFData.itemNListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                }
            }
            else
            {
                itemFriendData itemFData = itemFDTemp;
                itemFData.itemNListPrefab = new itemNearbyListPrefab();
                itemFData.itemNListPrefab.init();
                Transform rootT;
                itemFData.itemNListPrefab.show(itemFDTemp, out rootT);
                itemFData.root = rootT;
                NearbyListDataDic[item.m_unCID] = itemFData;
                if (itemFDataHeight == 0)
                {
                    itemFDataHeight = itemFData.itemNListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                }
                neighborList._instance.nearbyListCount.text = OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
                neighborList._instance.contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight) * OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count);
            }

        }
        void onFreshNearby(GameEvent e)
        {

            uint cid = e.data["cid"]._uint;
            uint combpt = e.data["combpt"]._uint;
            if (NearbyListDataDic.ContainsKey(cid))
            {
                itemFriendData itemFDTemp = NearbyListDataDic[cid];
                itemFDTemp.combpt = combpt;
                itemFDTemp.itemNListPrefab.set(itemFDTemp);
            }

        }
        void onDeleteFriend(GameEvent e)
        {
            uint cidTemp = e.data["cid"]._uint;
            if (friendList._instance.friendDataDic.ContainsKey(cidTemp))
            {
                GameObject.Destroy(friendList._instance.friendDataDic[cidTemp].root.gameObject);
                if (FriendProxy.getInstance().requestFriendListNoAgree.Contains(friendList._instance.friendDataDic[cidTemp].root.name))
                {
                    FriendProxy.getInstance().requestFriendListNoAgree.Remove(friendList._instance.friendDataDic[cidTemp].root.name);
                }
                friendList._instance.friendDataDic.Remove(cidTemp);
                friendLimit.text = friendList._instance.friendDataDic.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
                contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (60) * friendList._instance.friendDataDic.Count);
            }
            if (friendList._instance.EnemyListDataDic.ContainsKey(cidTemp))
            {
                friendList._instance.EnemyListDataDic[cidTemp].itemEListPrefab.UpdatePos();
            }
        }
        void onReceiveAddBlackList(GameEvent e)
        {

            uint cidTemp = e.data["cid"]._uint;
            if (friendList._instance.friendDataDic.ContainsKey(cidTemp))
            {
                GameObject.Destroy(friendList._instance.friendDataDic[cidTemp].root.gameObject);
                friendList._instance.friendDataDic.Remove(cidTemp);
            }

        }

        private int SortItemFriendData(itemFriendData a1, itemFriendData a2)
        {
            if (a1.online.CompareTo(a2.online) != 0)
                return -(a1.online.CompareTo(a2.online));
            else if (a1.zhuan.CompareTo(a2.zhuan) != 0)
                return -(a1.zhuan.CompareTo(a2.zhuan));
            else if (a1.lvl.CompareTo(a2.lvl) != 0)
                return -(a1.lvl.CompareTo(a2.lvl));
            else if (a1.combpt.CompareTo(a2.combpt) != 0)
                return -(a1.combpt.CompareTo(a2.combpt));
            else
                return 1;
        }
        public string getMapNameById(int id)
        {
            if (id == -1)
            {
                return ContMgr.getCont("a3_friend_lx");
            }
            if (SvrMapConfig.instance.getSingleMapConf((uint)id) == null)
            {
                return ContMgr.getCont("a3_friend_wz");
            }
            return SvrMapConfig.instance.getSingleMapConf((uint)id)["map_name"]._str;

        }
        public void hidActionPanel()
        {
            if (actionPanelPrefab._instance.root.gameObject.activeSelf)
            {
                actionPanelPrefab._instance.root.gameObject.SetActive(false);
            }
            if (actionNearybyPanelPrefab._instance.root.gameObject.activeSelf)
            {
                actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
            }
        }
    }

    class itemFriendData
    {
        public uint cid;
        public string name;
        public uint carr;
        public int lvl;
        public uint zhuan;
        public string clan_name;
        public bool online;
        public int map_id;
        public uint llid;
        public string team;
        public uint combpt;
        public uint hatred;//仇恨
        public uint kill_tm;//击杀次数
        public string pos;
        public bool isNew;
        public float timer;
        public int mlzd_lv;//磨炼之地所在层
        public uint limttm;
        public uint onlinename;

        public Transform root;
        public itemFriendPrefab itemFPrefab;
        public itemBlackListPrefab itemBListPrefab;
        public itemNearbyListPrefab itemNListPrefab;
        public itemEnemyListPrefab itemEListPrefab;
        public itemRecommendListPrefab itemECListPrefab;
    }
    class actionPanelPrefab : toggleGropFriendBase //
    {
        public static actionPanelPrefab _instance;

        public BaseButton btnChat;
        public BaseButton btnWatch;
        public BaseButton btnTeam;
        public BaseButton btnDelete;
       public BaseButton btnBlackList;
        public BaseButton btnCloseBg;
        public uint cid;
        public string name;
        public uint zhuan;
        public int lvl;
        public void init()
        {
            _instance = this;
            root = mTransform.FindChild("itemPrefabs/actionPanel");
            btnChat = new BaseButton(root.FindChild("buttons/btnChat"));
            btnWatch = new BaseButton(root.FindChild("buttons/btnWatch"));
            btnTeam = new BaseButton(root.FindChild("buttons/btnTeam"));
            btnDelete = new BaseButton(root.FindChild("buttons/btnDelete"));
            btnBlackList = new BaseButton(root.FindChild("buttons/btnBlackList"));
            btnCloseBg = new BaseButton(root.FindChild("btnCloseBg"));
          
        }
     
        void CloseRoot()
        {

            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }
    }
    class actionBlackListPanelPrefab : toggleGropFriendBase //
    {
        public static actionBlackListPanelPrefab _instance;
        uint cid;
        string name;
        uint zhuan;
        int lvl;
        public void init()
        {
            _instance = this;
            root = mTransform.FindChild("itemPrefabs/actionBlackListPanel");
            BaseButton btnAddFriend = new BaseButton(root.FindChild("buttons/btnAddFriend"));
            BaseButton btnCancleBlackList = new BaseButton(root.FindChild("buttons/btnCancleBlackList"));
            BaseButton btnCloseBg = new BaseButton(root.FindChild("btnCloseBg"));
            btnAddFriend.onClick = onAddFriendClick;
            btnCancleBlackList.onClick = onBtnCancleBlackListClick;
            btnCloseBg.onClick = onBtnCloseBgClick;
        }
        public void Show(itemFriendData ifd)
        {
            cid = ifd.cid;
            name = ifd.name;
            zhuan = ifd.zhuan;
            lvl = ifd.lvl;
            if (!root.gameObject.activeSelf) root.gameObject.SetActive(true);

        }
        void onAddFriendClick(GameObject go)
        {
            FriendProxy.getInstance().sendAddFriend(cid, name);
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }
        void onBtnCancleBlackListClick(GameObject go)
        {
            FriendProxy.getInstance().sendRemoveBlackList(cid);
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }
        void onBtnCloseBgClick(GameObject go)
        {
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);

        }

    }
    class actionEnemyPanel : toggleGropFriendBase //
    {
        public static actionEnemyPanel _instance;
        uint cid;
        string name;
        uint zhuan;
        int lvl;
        public void init()
        {
            _instance = this;
            root = mTransform.FindChild("itemPrefabs/actionEnemyPanel");
            BaseButton btnDelEnemy = new BaseButton(root.FindChild("buttons/btnDelEnemy"));
            BaseButton btnAdd = new BaseButton(root.FindChild("buttons/btnAdd"));
            BaseButton btnBlackList = new BaseButton(root.FindChild("buttons/btnBlackList"));
            BaseButton btnCloseBg = new BaseButton(root.FindChild("btnCloseBg"));
            btnDelEnemy.onClick = onBtnDelEnemyClick;
            btnAdd.onClick = onBtnAddClick;
            btnBlackList.onClick = onBtnBlackListClick;
            btnCloseBg.onClick = onBtnCloseBgClick;
        }
        public void Show(itemFriendData ifd)
        {
            cid = ifd.cid;
            name = ifd.name;
            zhuan = ifd.zhuan;
            lvl = ifd.lvl;
            if (!root.gameObject.activeSelf) { root.gameObject.SetActive(true); }
        }
        void onBtnDelEnemyClick(GameObject go)
        {
            enemyList._instance.deleteEnemyPanel.gameObject.SetActive(true);
            enemyList._instance.txtDInfo.text =ContMgr.getCont("a3_friend_txt", new List<string>() { name });
            enemyList._instance.cid = cid;
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }
        void onBtnAddClick(GameObject go)
        {
            bool checkAddFriend = FriendProxy.getInstance().checkAddFriend(name);
            if (!checkAddFriend)
            {
                FriendProxy.getInstance().sendAddFriend(cid, name);
            }
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }
        void onBtnBlackListClick(GameObject go)
        {
            {
                if (friendList._instance.BlackListDataDic.ContainsKey(cid))
                {
                    flytxt.instance.fly(name + ContMgr.getCont("a3_friend_oldhave"));
                }
                else
                {
                    FriendProxy.getInstance().sendAddBlackList(cid, name);
                }
                if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
            }
        }
       
        void onBtnCloseBgClick(GameObject go)
        {
            if (root.gameObject.activeSelf) root.gameObject.SetActive(false);
        }

    }
    class actionNearybyPanelPrefab : toggleGropFriendBase //
    {
        public static actionNearybyPanelPrefab _instance;

        public BaseButton btnChat;
        public BaseButton btnWatch;
        public BaseButton btnTeam;
        public BaseButton btnAdd;
        public BaseButton btnBlackList;
        public BaseButton btnCloseBg;
        public uint cid;
        public string name;
        public void init()
        {
            _instance = this;
            root = mTransform.FindChild("itemPrefabs/actionNearybyPanel");
            btnChat = new BaseButton(root.FindChild("buttons/btnChat"));
            btnWatch = new BaseButton(root.FindChild("buttons/btnWatch"));
            btnTeam = new BaseButton(root.FindChild("buttons/btnTeam"));
            btnAdd = new BaseButton(root.FindChild("buttons/btnAdd"));
            btnBlackList = new BaseButton(root.FindChild("buttons/btnBlackList"));
            btnCloseBg = new BaseButton(root.FindChild("btnCloseBg"));
        }
    }
    class blackList : toggleGropFriendBase//黑名单
    {
        public static blackList _instance;
        public Transform containt;
        public Text blackListCount;
        public RectTransform contains;
        public  Transform addBlackListPanel;
        public  InputField txtAddBlackListName;

        uint cide;
      

        public new void init()
        {

            _instance = this;
            root = mTransform.FindChild("mainBody/blackListPanel");
            containt = root.FindChild("right/main/body/scroll/contains");
            contains = containt.GetComponent<RectTransform>();
            blackListCount = root.FindChild("right/main/body/blackListCount/count").GetComponent<Text>();
            BaseButton btnAddBlackList = new BaseButton(root.FindChild("right/bottom/btnAddBlackList"));
            btnAddBlackList.onClick = onBtnAddBlackListClick;
            BaseButton btnAddToBlackList = new BaseButton(root.FindChild("hidPanels/addBlackListPanel/bottom/btnAdd"));
            btnAddToBlackList.onClick = onBtnAddToBlackListClick;
            BaseButton btnCancle = new BaseButton(root.FindChild("hidPanels/addBlackListPanel/bottom/btnCancle"));
            btnCancle.onClick = onBtnCancleClick;
            BaseButton btnAddBlackListClose = new BaseButton(root.FindChild("hidPanels/addBlackListPanel/title/btnClose"));
            btnAddBlackListClose.onClick = onBtnCancleClick;
            addBlackListPanel = root.FindChild("hidPanels/addBlackListPanel");
            txtAddBlackListName = root.FindChild("hidPanels/addBlackListPanel/main/InputField").GetComponent<InputField>();
        }
        void onBtnAddBlackListClick(GameObject go)
        {
            if (!addBlackListPanel.gameObject.activeSelf) addBlackListPanel.gameObject.SetActive(true);
        }
        void onBtnAddToBlackListClick(GameObject go)
        {
            string name = txtAddBlackListName.text;
            if (!string.IsNullOrEmpty(name))
            {
                bool checkAddFriend = FriendProxy.getInstance().checkAddheiFriend(name);
                if (!checkAddFriend)
                {
                    FriendProxy.getInstance().joinHeiList(0, name);
                }
                else
                {
                    if (!FriendProxy.getInstance().FriendDataList.ContainsKey(cide))
                    {
                        a3_friend.instance.MaxIns(name);
                    }
                }
               
                addBlackListPanel.gameObject.SetActive(false);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_friend_peoplename"));
            }
        }
        void onBtnCancleClick(GameObject go)
        {
            if (addBlackListPanel.gameObject.activeSelf) addBlackListPanel.gameObject.SetActive(false);
        }
        void closeAddBlackListPanel()
        {
            if (addBlackListPanel.gameObject.activeSelf) addBlackListPanel.gameObject.SetActive(false);
        }
    }
    class enemyList : toggleGropFriendBase//仇人列表
    {
        public static enemyList _instance;
        public Transform containt;
        public Text enemyListCount;
        public RectTransform contains;

        //searchEnemyPanel
        Transform searchEnemyPanel;
        Transform norepeatpanel;
        Text txtInifo;
        //deleteEnemyPanel
        public Transform deleteEnemyPanel;
        public Text txtDInfo;
        public uint cid;
        public new void init()
        {
            _instance = this;
            root = mTransform.FindChild("mainBody/enemyPanel");
            containt = root.FindChild("right/main/body/scroll/contains");
            contains = containt.GetComponent<RectTransform>();
            enemyListCount = root.FindChild("right/main/body/enemyListCount/count").GetComponent<Text>();


            BaseButton btnSearchPos = new BaseButton(root.FindChild("right/bottom/btnSearchPos"));

            btnSearchPos.onClick = onBtnSearchPos;
            //searchEnemyPanel
            searchEnemyPanel = root.FindChild("hidPanels/searchEnemyPanel");
            norepeatpanel = root.FindChild("hidPanels/norepeatpanel");
            txtInifo = searchEnemyPanel.FindChild("main/Text").GetComponent<Text>();
            BaseButton btnSEPClose = new BaseButton(searchEnemyPanel.FindChild("title/btnClose"));
            BaseButton btnSEPCancel = new BaseButton(searchEnemyPanel.FindChild("bottom/btnCancel"));
            BaseButton btnSEPSearch = new BaseButton(searchEnemyPanel.FindChild("bottom/btnSearch"));

            BaseButton btnREPClose = new BaseButton(norepeatpanel.FindChild("title/btnClose"));
            BaseButton btnREPCancel = new BaseButton(norepeatpanel.FindChild("bottom/btnCancel"));
            BaseButton btnREPSearch = new BaseButton(norepeatpanel.FindChild("bottom/btnSearch"));

            btnSEPClose.onClick = onBtnSEPClose;
            btnSEPCancel.onClick = onBtnSEPClose;
            btnSEPSearch.onClick = onBtnSEPSearch;

            btnREPClose.onClick = onBtnREPClose;
            btnREPCancel.onClick = onBtnREPClose;
            btnREPSearch.onClick = onBtnREPSearch;


            //deleteEnemyPanel
            deleteEnemyPanel = root.FindChild("hidPanels/deleteEnemyPanel");
            txtDInfo = deleteEnemyPanel.FindChild("main/Text").GetComponent<Text>();
            BaseButton btnDEPClose = new BaseButton(deleteEnemyPanel.FindChild("title/btnClose"));
            BaseButton btnDEPCancel = new BaseButton(deleteEnemyPanel.FindChild("bottom/btnCancel"));
            BaseButton btnDEPOk = new BaseButton(deleteEnemyPanel.FindChild("bottom/btnOK"));
            btnDEPClose.onClick = onBtnDEPClose;
            btnDEPCancel.onClick = onBtnDEPClose;
            btnDEPOk.onClick = onBtnDEPOk;

        }
        Dictionary<uint, itemFriendData> enemyDicTemp = new Dictionary<uint, itemFriendData>();
        public Toggle cur_Toggle = null;
        private void onBtnSearchPos(GameObject obj)
        {
            enemyDicTemp.Clear();
            Dictionary<uint, itemFriendData> enemyDic = friendList._instance.EnemyListDataDic;
            foreach (KeyValuePair<uint, itemFriendData> item in enemyDic)
            {
                Toggle tg = item.Value.root.FindChild("Toggle").GetComponent<Toggle>();
                if (tg.isOn)
                {
                    enemyDicTemp.Add(item.Key, item.Value);
                }
            }
            if (enemyDicTemp.Count > 1)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_friend_justone"));
            }
            if (enemyDicTemp.Count == 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_friend_whoone"));
            }
            if (enemyDicTemp.Count == 1)
            {
                foreach (KeyValuePair<uint, itemFriendData> item in enemyDicTemp)
                {
                    if (item.Value.itemEListPrefab.isCdNow)
                    {
                        flytxt.instance.fly(ContMgr.getCont("nochaxun"));
                    }
                   else if (!item.Value.online)
                    {
                        flytxt.instance.fly(ContMgr.getCont("playnoline"));
                    }
                    else
                    {
                        txtInifo.text = ContMgr.getCont("chaxunchouren", item.Value.name, (ContMgr.getCont("goldnum")));
                        searchEnemyPanel.gameObject.SetActive(true);
                    }
                }
            }
        }
        Dictionary<uint, itemFriendData> enemyDeleteDicTemp = new Dictionary<uint, itemFriendData>();
        Dictionary<uint, itemFriendData> enemyRevDicTemp = new Dictionary<uint, itemFriendData>();

        //
        private void onBtnREPClose(GameObject obj)
        {
            norepeatpanel.gameObject.SetActive(false);
        }

        private void onBtnSEPClose(GameObject obj)
        {
            searchEnemyPanel.gameObject.SetActive(false);
        }
       
        private void onBtnSEPSearch(GameObject obj)
        {
            if (enemyDicTemp.Count != 1) return;
            foreach (KeyValuePair<uint, itemFriendData> itemm in friendList._instance.EnemyListDataDic)
            {
                Text texv = itemm.Value.root.FindChild("Toggle/containts/txtpos/txtTimer").GetComponent<Text>();
                if (texv.text != "")
                {
                    searchEnemyPanel.gameObject.SetActive(false);
                    norepeatpanel.gameObject.SetActive(true);
                    return;
                }         
            }
            foreach (KeyValuePair<uint, itemFriendData> item in enemyDicTemp)
            {
                enemyRevDicTemp.Clear();
                enemyRevDicTemp.Add(item.Key, item.Value);//第一次进来的时候添加上
                FriendProxy.getInstance().sendEnemyPostion(item.Key);
                searchEnemyPanel.gameObject.SetActive(false);
            }

        }
        private void onBtnREPSearch(GameObject obj)
        {      
            if (enemyDicTemp.Count != 1) return;
                foreach (KeyValuePair<uint, itemFriendData> itdm in enemyRevDicTemp)//会重置第一次添加上的
            {
                Text teii = itdm.Value.root.FindChild("Toggle/containts/txtpos/txtTimer").GetComponent<Text>();
                Text teps = itdm.Value.root.FindChild("Toggle/containts/txtpos").GetComponent<Text>();
                Text teds = itdm.Value.root.FindChild("Toggle/containts/txtpos/findline").GetComponent<Text>();
                teii.text = "";
                teds.text = "";
                teps.text = ContMgr.getCont("a3_friend_wz");
                itemFriendData ifdd = friendList._instance.EnemyListDataDic[itdm.Key];
                if (ifdd.itemEListPrefab.isCdNow) ifdd.itemEListPrefab.isCdNow = false;
                TickMgr.instance.removeTick(ifdd.itemEListPrefab.postionTime);
            }

            foreach (KeyValuePair<uint, itemFriendData> item in enemyDicTemp)
            {
                enemyRevDicTemp.Clear();
                enemyRevDicTemp.Add(item.Key, item.Value);
                FriendProxy.getInstance().sendEnemyPostion(item.Key);
                norepeatpanel.gameObject.SetActive(false);

            }
           
        }
        //deleteEnemyPanel
        void onBtnDEPClose(GameObject go)
        {
            deleteEnemyPanel.gameObject.SetActive(false);
        }
        void onBtnDEPOk(GameObject go)
        {
            deleteEnemyPanel.gameObject.SetActive(false);
            FriendProxy.getInstance().sendDeleteEnemy(cid);
        }

    }
    class neighborList : toggleGropFriendBase//附近的人
    {
        public static neighborList _instance;
        public Transform containt;
        public Text nearbyListCount;
        public RectTransform contains;
        public new void init()
        {
            _instance = this;
            root = mTransform.FindChild("mainBody/neighborPanel");
            containt = root.FindChild("right/main/body/scroll/contains");
            contains = containt.GetComponent<RectTransform>();
            nearbyListCount = root.FindChild("right/main/body/nearbyListCount/count").GetComponent<Text>();
            BaseButton btnRefresh = new BaseButton(root.FindChild("right/bottom/btnRefresh"));
            BaseButton btnAddFriend = new BaseButton(root.FindChild("right/bottom/btnAddFriend"));
            BaseButton btnTeam = new BaseButton(root.FindChild("right/bottom/btnTeam"));
            btnRefresh.onClick = onBtnRefresh;
            btnAddFriend.onClick = onBtnAddFriend;
            btnTeam.onClick = onBtnTeam;
        }

        private void onBtnTeam(GameObject obj)
        {
            //TODO:组队
        }

        private void onBtnAddFriend(GameObject obj)
        {
            Dictionary<uint, itemFriendData> nearbyDic = friendList._instance.NearbyListDataDic;
            foreach (KeyValuePair<uint, itemFriendData> item in nearbyDic)
            {
                Toggle tg = item.Value.root.FindChild("Toggle").GetComponent<Toggle>();
                if (tg.isOn)
                {
                    bool checkAddFriend = FriendProxy.getInstance().checkAddFriend(item.Value.name);
                    if (!checkAddFriend)
                    {
                        FriendProxy.getInstance().sendAddFriend(item.Key, item.Value.name);
                    }
                }
            }
        }

        private void onBtnRefresh(GameObject obj)
        {
            float itemFDataHeight = 0;
            Dictionary<uint, itemFriendData> nearbyDic = friendList._instance.NearbyListDataDic;
            foreach (KeyValuePair<uint, itemFriendData> item in nearbyDic)
            {
                item.Value.root.gameObject.SetActive(false);
            }
            foreach (KeyValuePair<uint, ProfessionRole> item in OtherPlayerMgr._inst.m_mapOtherPlayerSee)
            {
                itemFriendData itemFDTemp = new itemFriendData();
                itemFDTemp.cid = item.Value.m_unCID;
                itemFDTemp.name = item.Value.roleName;
                itemFDTemp.carr = (uint)item.Value.m_roleDta.carr;
                itemFDTemp.lvl = item.Value.lvl;
                itemFDTemp.zhuan = (uint)item.Value.zhuan;
                itemFDTemp.clan_name = string.IsNullOrEmpty(item.Value.clanName) ? ContMgr.getCont("FriendProxy_wu") : item.Value.clanName;
                itemFDTemp.combpt = (uint)item.Value.combpt;
                itemFDTemp.map_id = (int)PlayerModel.getInstance().mapid;

                if (nearbyDic.ContainsKey(item.Value.m_unCID))
                {
                    itemFriendData itemFData = nearbyDic[item.Value.m_unCID];
                    itemFData.itemNListPrefab.set(itemFDTemp);
                }
                else
                {
                    itemFriendData itemFData = itemFDTemp;
                    itemFData.itemNListPrefab = new itemNearbyListPrefab();
                    itemFData.itemNListPrefab.init();
                    Transform rootT;
                    itemFData.itemNListPrefab.show(itemFDTemp, out rootT);
                    itemFData.root = rootT;
                    friendList._instance.NearbyListDataDic[item.Value.m_unCID] = itemFData;
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemNListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }
                }
                neighborList._instance.nearbyListCount.text = OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
                neighborList._instance.contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight + 10.0f) * OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count);
            }
        }

    }
    class recommendList : toggleGropFriendBase//一键征友
    {
        public static recommendList _instance;
        public Transform containt;

        public new void init()
        {
            _instance = this;
            root = mTransform.FindChild("mainBody/myFriendsPanel/hidPanels/personalsPanel");
            containt = root.FindChild("main/scroll/containts");

        }
    }
    class toggleGropFriend : toggleGropFriendBase
    {

        public Toggle togFriend;
        public static toggleGropFriend _instance;
        Toggle togBlackList;
        Toggle togEnemy;
        Toggle togNearby;
        public new void init()
        {
            _instance = this;
            togFriend = mTransform.FindChild("mainBody/left/toggleGroup/togFriend").GetComponent<Toggle>();
            togBlackList = mTransform.FindChild("mainBody/left/toggleGroup/togBlacklist").GetComponent<Toggle>();
            togEnemy = mTransform.FindChild("mainBody/left/toggleGroup/togEnemy").GetComponent<Toggle>();
            togNearby = mTransform.FindChild("mainBody/left/toggleGroup/togNearby").GetComponent<Toggle>();
            togFriend.onValueChanged.AddListener(onTogFriendClick);
            togBlackList.onValueChanged.AddListener(onTogBlackListClick);
            togEnemy.onValueChanged.AddListener(onTogEnemyListClick);
            togNearby.onValueChanged.AddListener(onTogNearbyListClick);
        }
        void onTogFriendClick(bool b)//好友列表
        {
            if (!b) return;
            if (togFriendType == ToggleFriend.Friend) return;
            togFriendType = ToggleFriend.Friend;
            mFriendList.show();
            if (mBlackList.isActive) mBlackList.close();
            if (mEnemyList.isActive) mEnemyList.close();
            if (mNeighborList.isActive) mNeighborList.close();
            friendList._instance.hidActionPanel();

        }
        void onTogBlackListClick(bool b)//黑名单
        {
            if (!b) return;
            if (togFriendType == ToggleFriend.BlackList) return;
            togFriendType = ToggleFriend.BlackList;
            mBlackList.show();
            if (mFriendList.isActive) mFriendList.close();
            if (mEnemyList.isActive) mEnemyList.close();
            if (mNeighborList.isActive) mNeighborList.close();
            friendList._instance.hidActionPanel();

        }
        void onTogEnemyListClick(bool b)//仇人列表
        {
            if (!b) return;
            if (togFriendType == ToggleFriend.Enemy) return;
            togFriendType = ToggleFriend.Enemy;
            mEnemyList.show();
            if (mFriendList.isActive) mFriendList.close();
            if (mBlackList.isActive) mBlackList.close();
            if (mNeighborList.isActive) mNeighborList.close();
            friendList._instance.hidActionPanel();

        }
        public void onTogNearbyListClick(bool b)//附近的人
        {
            if (!b) return;
            if (togFriendType == ToggleFriend.Nearby) return;
            togFriendType = ToggleFriend.Nearby;
            mNeighborList.show();
            if (mFriendList.isActive) mFriendList.close();
            if (mBlackList.isActive) mBlackList.close();
            if (mEnemyList.isActive) mEnemyList.close();
            friendList._instance.hidActionPanel();
        }
    }
    class toggleGropFriendBase// : MonoBehaviour
    {

        public static friendList mFriendList;
        public static blackList mBlackList;
        public static enemyList mEnemyList;
        public static neighborList mNeighborList;
        static toggleGropFriend mToggleGropFriend;
        public ToggleFriend togFriendType = ToggleFriend.Friend;
        static actionPanelPrefab app = new actionPanelPrefab();
        static actionNearybyPanelPrefab anpp = new actionNearybyPanelPrefab();
        static recommendList mRecommendList;
        static AddToBalckListWaring addToBalckListWaring;
        static actionBlackListPanelPrefab actionBlackListPanel;
        static actionEnemyPanel _actionEnemyPanel;
        public static void init()
        {

            mFriendList = new friendList();
            mBlackList = new blackList();
            mEnemyList = new enemyList();
            mNeighborList = new neighborList();
            mToggleGropFriend = new toggleGropFriend();
            mRecommendList = new recommendList();
            addToBalckListWaring = new AddToBalckListWaring();
            actionBlackListPanel = new actionBlackListPanelPrefab();
            _actionEnemyPanel = new actionEnemyPanel();

            _actionEnemyPanel.init();
            actionBlackListPanel.init();
            mRecommendList.init();
            mFriendList.init();
            mBlackList.init();
            mEnemyList.init();
            mNeighborList.init();
            mToggleGropFriend.init();
            app.init();
            anpp.init();
            addToBalckListWaring.init();
        }
        public Transform root;
        public bool isActive { get { return root.gameObject.activeSelf; } }

        public static Transform mTransform;
        virtual public void show()
        {
            if (!root.gameObject.activeSelf)
                root.gameObject.SetActive(true);
            float itemFDataHeight = 0;
            Dictionary<uint, itemFriendData> nearbyDic = friendList._instance.NearbyListDataDic;
            foreach (KeyValuePair<uint, itemFriendData> item in nearbyDic)
            {
                item.Value.root.gameObject.SetActive(false);
            }
            foreach (KeyValuePair<uint, ProfessionRole> item in OtherPlayerMgr._inst.m_mapOtherPlayerSee)
            {
                itemFriendData itemFDTemp = new itemFriendData();
                itemFDTemp.cid = item.Value.m_unCID;
                itemFDTemp.name = item.Value.roleName;
                itemFDTemp.carr = (uint)item.Value.m_roleDta.carr;
                itemFDTemp.lvl = item.Value.lvl;
                itemFDTemp.zhuan = (uint)item.Value.zhuan;
                itemFDTemp.clan_name = string.IsNullOrEmpty(item.Value.clanName) ?ContMgr.getCont("FriendProxy_wu") : item.Value.clanName;
                itemFDTemp.combpt = (uint)item.Value.combpt;
                itemFDTemp.map_id = (int)PlayerModel.getInstance().mapid;

                if (nearbyDic.ContainsKey(item.Value.m_unCID))
                {
                    itemFriendData itemFData = nearbyDic[item.Value.m_unCID];
                    itemFData.itemNListPrefab.set(itemFDTemp);
                }
                else
                {
                    itemFriendData itemFData = itemFDTemp;
                    itemFData.itemNListPrefab = new itemNearbyListPrefab();
                    itemFData.itemNListPrefab.init();
                    Transform rootT;
                    itemFData.itemNListPrefab.show(itemFDTemp, out rootT);
                    itemFData.root = rootT;
                    friendList._instance.NearbyListDataDic[item.Value.m_unCID] = itemFData;
                    if (itemFDataHeight == 0)
                    {
                        itemFDataHeight = itemFData.itemNListPrefab.root.transform.FindChild("Toggle/Background").GetComponent<RectTransform>().sizeDelta.y;
                    }
                }

            }
            neighborList._instance.nearbyListCount.text = OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count.ToString() + "/" + (50 + 10 * PlayerModel.getInstance().up_lvl);
            neighborList._instance.contains.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemFDataHeight + 10.0f) * OtherPlayerMgr._inst.m_mapOtherPlayerSee.Count);
        }
        virtual public void close()
        {
            if (root.gameObject.activeSelf)
                root.gameObject.SetActive(false);
        }

    }
    enum ToggleFriend
    {
        Friend,
        BlackList,
        Enemy,
        Nearby
    }
    class itemFriendPrefab : toggleGropFriendBase//列表Prefab
    {
        
        static public itemFriendPrefab instance;
        public Transform root;
        public Toggle toggle;
        public Text txtNickName;
        public Text txtLvl;
        public Text txtTeam;
        public Text txtCombat;
        public Text txtPos;

        public BaseButton btnAction;
        uint cid;
        string name;
        uint zhuan;
        int lvl;
        public bool watch_avt;
        Transform actionPanelPos;
        public new void init()
        {
            instance = this;
            Transform tf = mTransform.FindChild("itemPrefabs/itemFriend");
            GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
            root = gob.transform;
            toggle = root.FindChild("Toggle").GetComponent<Toggle>();
            txtNickName = toggle.transform.FindChild("containts/txtName").GetComponent<Text>();
            txtLvl = toggle.transform.FindChild("containts/txtLevel").GetComponent<Text>();
            txtTeam = toggle.transform.FindChild("containts/txtTeam").GetComponent<Text>();
            txtCombat = toggle.transform.FindChild("containts/txtcombat").GetComponent<Text>();
            txtPos = toggle.transform.FindChild("containts/txtpos").GetComponent<Text>();
            actionPanelPos = root.FindChild("btnAction/actionPanelPos");
            btnAction = new BaseButton(root.transform.FindChild("btnAction"));
            gob.SetActive(true);
            root.SetParent(friendList._instance.containt, false);
        }

        public void show(itemFriendData data, out Transform rootT)
        {
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            name = data.name;
            zhuan = data.zhuan;
            lvl = data.lvl;
            data.root = root;
            rootT = root;
            btnAction.onClick = onBtnActionClick;


            actionPanelPrefab._instance.btnBlackList.onClick = onBtnBlackList;
            actionPanelPrefab._instance.btnDelete.onClick = onBtnDelete;
            actionPanelPrefab._instance.btnChat.onClick = onBtnChat;
            actionPanelPrefab._instance.btnWatch.onClick = onBtnWatch;
            actionPanelPrefab._instance.btnTeam.onClick = onBtnTeamClick;
            actionPanelPrefab._instance.btnCloseBg.onClick = onBtnCloseBg;

        }
        public void set(itemFriendData data)
        {
            root.gameObject.SetActive(true);
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            name = data.name;
            zhuan = data.zhuan;
            lvl = data.lvl;

            data.root = root;

        }
        void onBtnActionClick(GameObject go)
        {
            actionPanelPrefab._instance.cid = cid;
            actionPanelPrefab._instance.name = name;
            actionPanelPrefab._instance.zhuan = zhuan;
            actionPanelPrefab._instance.lvl = lvl;
            if (!actionPanelPrefab._instance.root.gameObject.activeSelf)
            {
                actionPanelPrefab._instance.root.position = actionPanelPos.position;
                actionPanelPrefab._instance.root.gameObject.SetActive(true);
            }
            else
            {
                if (actionPanelPrefab._instance.root.position.y != actionPanelPos.position.y)
                {
                    actionPanelPrefab._instance.root.position = actionPanelPos.position;
                }
                else
                {
                    actionPanelPrefab._instance.root.gameObject.SetActive(false);
                }
            }
        }
        void onBtnBlackList(GameObject go)
        {
            itemFriendData ifd = new itemFriendData();
            uint cidTemp = actionPanelPrefab._instance.cid;
            string name = actionPanelPrefab._instance.name;
            uint zhuan = actionPanelPrefab._instance.zhuan;
            int lvl = actionPanelPrefab._instance.lvl;

            ifd.cid = cidTemp;
            ifd.name = name;
            ifd.zhuan = zhuan;
            ifd.lvl = lvl;
            if (friendList._instance.BlackListDataDic.ContainsKey(cidTemp))
            {
                flytxt.instance.fly(name + ContMgr.getCont("a3_friend_oldhave"));
            }
            else
            {
                foreach (KeyValuePair<uint, itemFriendData> itemFriend in FriendProxy.getInstance().FriendDataList)
                {
                    if (itemFriend.Value.name.Equals(name))
                    {
                        a3_friend.instance.FiendToHei(ifd);
                        break;
                    }
                }
            }
            // AddToBalckListWaring._instance.Show(ifd);
          //  FriendProxy.getInstance().sendAddBlackList(cidTemp);
            actionPanelPrefab._instance.root.gameObject.SetActive(false);

        }
        void onBtnDelete(GameObject go)
        {
            uint cidTemp = actionPanelPrefab._instance.cid;
            FriendProxy.getInstance().sendDeleteFriend(cidTemp);
            actionPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnChat(GameObject go)
        {
            uint cidTemp = actionPanelPrefab._instance.cid;
            string name = actionPanelPrefab._instance.name;
            actionPanelPrefab._instance.root.gameObject.SetActive(false);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
            a3_chatroom._instance.privateChat(name);
        }
        void onBtnWatch(GameObject go)
        {
            watch_avt = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);

            uint cidTemp = actionPanelPrefab._instance.cid;
            ArrayList arr = new ArrayList();
            arr.Add(cidTemp);

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);

            actionPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnTeamClick(GameObject go)
        {
            uint cidTemp = actionPanelPrefab._instance.cid;
            TeamProxy.getInstance().SendInvite(cidTemp);
            actionPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnCloseBg(GameObject go)
        {
            actionPanelPrefab._instance.root.gameObject.SetActive(false);
        }


    }
    class itemBlackListPrefab : toggleGropFriendBase//列表Prefab
    {
        public Transform root;
        public Toggle toggle;
        public Text txtNickName;
        public Text txtLvl;
        public Text txtTeam;
        public Text txtCombat;
        public Text txtPos;
        public BaseButton btnAction;
        Transform actionPanelPos;
        uint cid;
        string name;
        uint zhuan;
        int lvl;
        public new void init()
        {
            Transform tf = mTransform.FindChild("itemPrefabs/itemBlackList");
            GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
            root = gob.transform;
            toggle = root.FindChild("Toggle").GetComponent<Toggle>();
            txtNickName = toggle.transform.FindChild("containts/txtName").GetComponent<Text>();
            txtLvl = toggle.transform.FindChild("containts/txtLevel").GetComponent<Text>();
            txtTeam = toggle.transform.FindChild("containts/txtTeam").GetComponent<Text>();
            txtCombat = toggle.transform.FindChild("containts/txtcombat").GetComponent<Text>();
            txtPos = toggle.transform.FindChild("containts/txtpos").GetComponent<Text>();
            btnAction = new BaseButton(root.transform.FindChild("btnAction"));
            actionPanelPos = root.FindChild("btnAction/actionPanelPos");
            btnAction.onClick = onBtnActionClick;
            gob.SetActive(true);
            root.SetParent(blackList._instance.containt, false);
        }

        public void show(itemFriendData data, out Transform rootT)
        {
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            data.root = root;
            rootT = root;

        }
        public void set(itemFriendData data)
        {
            root.gameObject.SetActive(true);
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            data.root = root;
        }
        void onBtnActionClick(GameObject go)
        {
            itemFriendData ifd = new itemFriendData();
            ifd.cid = cid;

            if (!actionBlackListPanelPrefab._instance.root.gameObject.activeSelf)
            {
                actionBlackListPanelPrefab._instance.root.position = actionPanelPos.position;
                actionBlackListPanelPrefab._instance.Show(ifd);
            }
            else
            {
                if (actionBlackListPanelPrefab._instance.root.position.y != actionPanelPos.position.y)
                {
                    actionBlackListPanelPrefab._instance.root.position = actionPanelPos.position;
                }
                else
                {
                    actionBlackListPanelPrefab._instance.root.gameObject.SetActive(false);
                }
            }
        }

    }
    class itemEnemyListPrefab : toggleGropFriendBase//列表Prefab
    {
        public Transform root;
        public Toggle toggle;
        public Text txtNickName;
        public Text txtLvl;
        public Text txtTeam;
        public Text txtCombat;
        public Text txthatred;
        public Text txtfinline;
        public static Text txtPos;
        public static Text txtTimer;
        public uint cid;


        string name;
        uint zhuan;
        int lvl;
        public bool isCdNow = false;
        Transform posAction;
        public static itemEnemyListPrefab instance;
        public new void init()
        {
            instance = this;
            Transform tf = mTransform.FindChild("itemPrefabs/itemEnemy");
            GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
            root = gob.transform;
            toggle = root.FindChild("Toggle").GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(FindKuan);
            toggle.interactable = true;
            toggle.targetGraphic = root.FindChild("Toggle/Background").GetComponent<Graphic>();
            root.FindChild("Toggle/Background").gameObject.SetActive(true);
            root.FindChild("Toggle/Background/Checkmark").gameObject.SetActive(false);
            txtNickName = toggle.transform.FindChild("containts/txtName").GetComponent<Text>();
            txtLvl = toggle.transform.FindChild("containts/txtLevel").GetComponent<Text>();
            txtTeam = toggle.transform.FindChild("containts/txtTeam").GetComponent<Text>();
            txtCombat = toggle.transform.FindChild("containts/txtcombat").GetComponent<Text>();
            if (toggle.transform.FindChild("containts/txthatred") != null)
            {
                txthatred = toggle.transform.FindChild("containts/txthatred").GetComponent<Text>();
            }
            txtPos = toggle.transform.FindChild("containts/txtpos").GetComponent<Text>();
            txtTimer = toggle.transform.FindChild("containts/txtpos/txtTimer").GetComponent<Text>();
            txtfinline= toggle.transform.FindChild("containts/txtpos/findline").GetComponent<Text>();
            BaseButton btnAction = new BaseButton(root.FindChild("Toggle/containts/btnAction"));
            btnAction.onClick = onBtnActionClick;
            gob.SetActive(true);
            root.SetParent(enemyList._instance.containt, false);
        }

        public void show(itemFriendData data, out Transform rootT)
        {
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txthatred.text = data.hatred.ToString();
            string posStr = string.Empty;
            posStr = friendList._instance.getMapNameById(data.map_id);
            txtPos.text = posStr;
            cid = data.cid;
            name = data.name;
            zhuan = data.zhuan;
            lvl = data.lvl;
            txtPos = root.FindChild("Toggle/containts/txtpos").GetComponent<Text>();
            txtTimer = root.FindChild("Toggle/containts/txtpos/txtTimer").GetComponent<Text>();
            posAction = root.FindChild("Toggle/containts/btnAction/actionPanelPos");
            data.root = root;
            rootT = root;

        }

        void hideCheck()
        {
            root.FindChild("Toggle/Background/Checkmark").gameObject.SetActive(false);
        }
        void FindKuan(bool a)
        {
            if (enemyList._instance != null && enemyList._instance.cur_Toggle != toggle)
            {
                if (a)
                {
                    if(enemyList._instance.cur_Toggle != null)
                    {
                        enemyList._instance.cur_Toggle.isOn = false;
                        enemyList._instance.cur_Toggle.transform.FindChild("Background/Checkmark").gameObject.SetActive(false);
                    }
                        
                    enemyList._instance.cur_Toggle = toggle;
                    toggle.transform.FindChild("Background/Checkmark").gameObject.SetActive(true);
                }
            }
        }
        public void set(itemFriendData data)
        {
            root.gameObject.SetActive(true);
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txthatred.text = data.hatred.ToString();
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            name = data.name;
            zhuan = data.zhuan;
            lvl = data.lvl;
            txtPos = root.FindChild("Toggle/containts/txtpos").GetComponent<Text>();
            txtTimer = root.FindChild("Toggle/containts/txtpos/txtTimer").GetComponent<Text>();
            posAction = root.FindChild("Toggle/containts/btnAction/actionPanelPos");
            data.root = root;
        }
        public void UpdatePos(int mapId = -1)
        {
            if (mapId == -1)
            {
                txtPos.text = ContMgr.getCont("weizhi");
            }
            else
            {
                txtPos.text = friendList._instance.getMapNameById(mapId);
            }
        }

        public TickItem postionTime;
        public float times = 0;
        public int i;
        void onUpdateEnmeyPostion(float s)
        {
            if (txtPos == null || txtTimer == null) return;
            times += s;
                if (times >= 1)
                {
                    i--;
                    if (i <= 0)
                    {
                        i = 0;
                        if (!FriendProxy.getInstance().FriendDataList.ContainsKey(cid))
                        {
                            txtPos.text = ContMgr.getCont("weizhi");
                        }
                          txtTimer.text = "";
                         txtfinline.text = "";
                        isCdNow = false;
                        TickMgr.instance.removeTick(postionTime);
                        postionTime = null;
                    }
                    else
                    {
                        isCdNow = true;
                        txtTimer.text = Globle.formatTime((int)i); ;
                    }
                    times = 0;
                }
        }
        public void refresShowPostion(int time)
        {
            txtPos = txtPos = root.FindChild("Toggle/containts/txtpos").GetComponent<Text>();
            txtTimer = root.FindChild("Toggle/containts/txtpos/txtTimer").GetComponent<Text>();
            if (time <= 0)
            {
                txtPos.text = ContMgr.getCont("weizhi");
                txtTimer.text = "";
                txtfinline.text = "";
                return;
            }
            else
            {

                postionTime = new TickItem(onUpdateEnmeyPostion);
                TickMgr.instance.addTick(postionTime);
                i = time;
            }
        }
        void onBtnActionClick(GameObject go)
        {
            itemFriendData ifd = new itemFriendData();
            ifd.cid = cid;
            ifd.name = name;
            ifd.zhuan = zhuan;
            ifd.lvl = lvl;
            actionEnemyPanel._instance.Show(ifd);
            actionEnemyPanel._instance.root.position = posAction.position;

        }
    }
    class itemNearbyListPrefab : toggleGropFriendBase//列表Prefab
    {
        static public itemNearbyListPrefab instance;
        public Transform root;
        public Toggle toggle;
        public Text txtNickName;
        public Text txtLvl;
        public Text txtTeam;
        public Text txtCombat;
        public Text txtPos;
        public uint cid;
        public string name;
        Transform actionPanelPos;
        BaseButton btnAction;
        public bool watch_avt;
        public new void init()
        {
            instance = this;
            Transform tf = mTransform.FindChild("itemPrefabs/itemNearbyFirend");
            GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
            root = gob.transform;
            toggle = root.FindChild("Toggle").GetComponent<Toggle>();
            txtNickName = toggle.transform.FindChild("containts/txtName").GetComponent<Text>();
            txtLvl = toggle.transform.FindChild("containts/txtLevel").GetComponent<Text>();
            txtTeam = toggle.transform.FindChild("containts/txtTeam").GetComponent<Text>();
            txtCombat = toggle.transform.FindChild("containts/txtcombat").GetComponent<Text>();
            txtPos = toggle.transform.FindChild("containts/txtpos").GetComponent<Text>();
            btnAction = new BaseButton(root.FindChild("btnAction"));
            actionPanelPos = root.FindChild("btnAction/actionPanelPos");
            btnAction.onClick = onBtnAction;
            gob.SetActive(true);
            root.SetParent(neighborList._instance.containt, false);
            actionNearybyPanelPrefab._instance.btnChat.onClick = onBtnChat;
            actionNearybyPanelPrefab._instance.btnWatch.onClick = onBtnWatch;
            actionNearybyPanelPrefab._instance.btnTeam.onClick = onBtnTeam;
            actionNearybyPanelPrefab._instance.btnAdd.onClick = onBtnAdd;
            actionNearybyPanelPrefab._instance.btnBlackList.onClick = onBtnBalckList;
            actionNearybyPanelPrefab._instance.btnCloseBg.onClick = onBtnCloseBg;
        }

        public void show(itemFriendData data, out Transform rootT)
        {
            txtNickName.text = data.name;
            name = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            data.root = root;
            rootT = root;

        }
        public void set(itemFriendData data)
        {
            root.gameObject.SetActive(true);
            txtNickName.text = data.name;
            name = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtTeam.text = data.clan_name;
            txtCombat.text = data.combpt.ToString();
            txtPos.text = friendList._instance.getMapNameById(data.map_id);
            cid = data.cid;
            data.root = root;
        }
        void onBtnAction(GameObject go)
        {

            actionNearybyPanelPrefab._instance.cid = cid;
            actionNearybyPanelPrefab._instance.name = name;
            if (!actionNearybyPanelPrefab._instance.root.gameObject.activeSelf)
            {
                actionNearybyPanelPrefab._instance.root.position = actionPanelPos.position;
                actionNearybyPanelPrefab._instance.root.gameObject.SetActive(true);
            }
            else
            {
                if (actionNearybyPanelPrefab._instance.root.position.y != actionPanelPos.position.y)
                {
                    actionNearybyPanelPrefab._instance.root.position = actionPanelPos.position;
                }
                else
                {
                    actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
                }
            }
        }
        void onBtnChat(GameObject go)
        {
            uint cidTemp = actionNearybyPanelPrefab._instance.cid;
            string name = actionNearybyPanelPrefab._instance.name;
            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
            a3_chatroom._instance.privateChat(name);

        }
        void onBtnWatch(GameObject go)
        {

            watch_avt = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);

            uint cidTemp = actionNearybyPanelPrefab._instance.cid;
            ArrayList arr = new ArrayList();
            arr.Add(cidTemp);
            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);




        }
        void onBtnTeam(GameObject go)
        {
            uint cidTemp = actionNearybyPanelPrefab._instance.cid;
            TeamProxy.getInstance().SendInvite(cidTemp);
            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnAdd(GameObject go)
        {
            uint cidTemp = actionNearybyPanelPrefab._instance.cid;
            string name = actionNearybyPanelPrefab._instance.name;
            FriendProxy.getInstance().sendAddFriend(cidTemp, name);
            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnBalckList(GameObject go)
        {
            uint cidTemp = actionNearybyPanelPrefab._instance.cid;

            if (friendList._instance.BlackListDataDic.ContainsKey(cidTemp))
            {
                string name = friendList._instance.BlackListDataDic[cidTemp].name;
                flytxt.instance.fly(name + ContMgr.getCont("a3_friend_havelblack"));
            }
            else
            {
                FriendProxy.getInstance().sendAddBlackList(cidTemp);
            }

            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
        }
        void onBtnCloseBg(GameObject go)
        {
            actionNearybyPanelPrefab._instance.root.gameObject.SetActive(false);
        }


    }

    class itemRecommendListPrefab : toggleGropFriendBase//列表Prefab
    {
        public Transform root;
        public Toggle toggle;
        public Text txtNickName;
        public string name;
        public Text txtLvl;
        public Text txtProfessional;
        public BaseButton btnAction;
        public uint cid;
        Transform actionPanelPos;
        public new void init()
        {
            Transform tf = mTransform.FindChild("itemPrefabs/itemAddFriend");
            GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
            root = gob.transform;
            toggle = root.FindChild("Toggle").GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(onToggleClick);

            txtNickName = root.transform.FindChild("texts/txtNickName").GetComponent<Text>();
            txtLvl = root.transform.FindChild("texts/txtLevel").GetComponent<Text>();
            txtProfessional = root.transform.FindChild("texts/txtProfessional").GetComponent<Text>();

            actionPanelPos = root.FindChild("btnAction/actionPanelPos");
            btnAction = new BaseButton(root.transform.FindChild("btnAction"));

            gob.SetActive(true);
            root.SetParent(recommendList._instance.containt, false);
            toggle.isOn = true;
        }

        public void show(itemFriendData data, out Transform rootT)
        {
            name = data.name;
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtProfessional.text = getProfessional(data.carr);
            cid = data.cid;
            data.root = root;
            rootT = root;
            toggle.isOn = true;
            btnAction.onClick = onBtnActionClick;
        }

        void onBtnActionClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
            uint cidTemp = cid;
            ArrayList arr = new ArrayList();
            arr.Add(cidTemp);
            PlayerModel.getInstance().showFriend = true;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);
            a3_targetinfo.instan?.transform.SetAsLastSibling();
        }
        string getProfessional(uint carr)
        {
            string str = string.Empty;
            str = ContMgr.getCont("profession" + carr);
            //switch (carr)
            //{
            //    case 1:
            //        str = "全职业";
            //        break;
            //    case 2:
            //        str = "战士";
            //        break;
            //    case 3:
            //        str = "法师";
            //        break;
            //    case 5:
            //        str = "暗影";
            //        break;
            //}
            return str;
        }
        public void set(itemFriendData data)
        {
            root.gameObject.SetActive(true);
            txtNickName.text = data.name;
            txtLvl.text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            txtProfessional.text = getProfessional(data.carr);
            cid = data.cid;
            data.root = root;
            toggle.isOn = true;
        }
        void onToggleClick(bool b)
        {
            friendList._instance.btnAdd.transform.GetComponent<Button>().interactable = true;
            //         foreach (KeyValuePair<uint,itemFriendData> item in friendList._instance.RecommendListDataDic)
            //{
            //	if (item.Value.itemECListPrefab.root.transform.FindChild("Toggle").GetComponent<Toggle>().isOn)
            //	{

            //		break;
            //	}
            //	//else
            //	//{
            //	//	friendList._instance.btnAdd.transform.GetComponent<Button>().interactable = false;
            //	//}
            //}


        }
    }
    class AddToBalckListWaring : toggleGropFriendBase
    {
        static public AddToBalckListWaring _instance;
        public Transform root;
        Text m_txtInfo;
        public uint cid;
        Action _ac = null;
        public new void init()
        {
            _instance = this;
            root = mTransform.FindChild("mainBody/HidePanel/AddToBalckListWaring");
            BaseButton btnClose = new BaseButton(root.FindChild("title/btnClose"));
            BaseButton btnOk = new BaseButton(root.FindChild("btnOK"));
            BaseButton btnCancle = new BaseButton(root.FindChild("btnCancel"));
            btnClose.onClick = onBtnCloseClick;
            btnCancle.onClick = onBtnCloseClick;
            btnOk.onClick = onBtnOkClick;
            m_txtInfo = root.FindChild("body/Text").GetComponent<Text>();
        }
        public void Show(itemFriendData itemData, Action ac = null)
        {
            if (root.gameObject.activeSelf==false) root.gameObject.SetActive(true);
            m_txtInfo.text = string.Format(ContMgr.getCont("uilayer_a3_friend_19"), itemData.name, itemData.zhuan, itemData.lvl);
            cid = itemData.cid;
            if (ac != null)
            {
                _ac = ac;
            }
        }
        void onBtnCloseClick(GameObject go)
        {
            root.gameObject.SetActive(false);
        }
        void onBtnOkClick(GameObject go)
        {
            FriendProxy.getInstance().sendAddBlackList(cid);
            root.gameObject.SetActive(false);
            if (_ac != null)
            {
                _ac.Invoke();
                _ac = null;
            }
        }
    }
}
