using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lui;
using System.Text.RegularExpressions;
using Cross;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections;

namespace MuGame
{
    class a3_chatroom : FloatUi
    {
        public static a3_chatroom _instance;
        ChatToType chatToType = ChatToType.All;
        public float dummyTop = float.NaN, dummyEnd = float.NaN,
                     expandHeightMsgMe, expandWidthMsgMe,
                     expandHeightMsgOthers, expandWidthMsgOthers,
                     beginMsgOthersOffsetX, beginMsgMeOffsetX,
                     msgLineSpace, firstMsgYInit;
        #region chatMain
        Text txtCurrentChat;
        RectTransform ipt_msg;
        InputField iptf_msg;
        List<Transform> AllItemMsg = new List<Transform>();
        //msgShow
        Toggle btn_all;//综合
        Toggle btn_world;
        Toggle btn_nearby;//附近
        Toggle btn_legion;//军团
        Toggle btn_team;
        Toggle btn_secretlanguage;//密语
        //msgPanel
        Transform panelAll;
        Transform panelWorld;
        Transform panelNearby;
        Transform panelLegion;
        Transform panelTeam;
        Transform panelScretlanguage;
        Panel4Player panel4Player = new Panel4Player();
        BaseButton panel4PlayerTouchBg;

        #endregion
        BaseButton btn_close;
        Transform plusItems;
        Transform quickMsg;
        //minBag
        Transform minBag;
        BaseButton btn_inBody;
        BaseButton btn_inBag;
        Transform bodyPanel;
        LScrollPage bodyPanelLScrollPage;
        Transform bodyPanelContent;
        Transform bodyToggle;//作为toogleGroup的父物体
        Transform bagPanel;
        Transform bagPanelContent;
        Transform toggleGroup;
        Transform bagToggle;//作为toogleGroup的父物体
        Transform prefabPageItem;
        LScrollPage bagPanelLScrollPage;
        GameObject itemEPrefab;//用于显示物品的预设体        
        //私聊好友
        Transform PanelFriends;
        //template
        GameObject itemChatCharName;
        Text itemChatMsgConfig;
        Transform prefabWhisperPanel;
        Dictionary<ChatToType, Transform> chatToButtons;
        Dictionary<ChatToType, Transform> chatToPanels;
        Dictionary<ChatToType, ItemChatMsgObj> itemChatMsgObjs;
        Dictionary<ChatToType, Stack<RectTransform>> chatToTypeMsgPostions;//聊天信息坐标位置
        Dictionary<ChatToType, float> yLastMessage = new Dictionary<ChatToType, float>();
        Dictionary<string, Stack<RectTransform>> dicPrivateChatToTypeMsgPostions;//私聊聊天信息坐标位置
        private Dictionary<uint, GameObject> itemicon = new Dictionary<uint, GameObject>();
        private Dictionary<uint, GameObject> houseicon = new Dictionary<uint, GameObject>();
        Queue<msgItemInfo> msgDic = new Queue<msgItemInfo>();//聊天输入的内容
        whisper[] stackWhisper = new whisper[5];//密聊对象
        Dictionary<string, Transform> dicWhisper = new Dictionary<string, Transform>();
        Dictionary<ulong, Variant> dicEquip = new Dictionary<ulong, Variant>();
        List<ulong> equipIndexLst = new List<ulong>();
        Transform endMsgPosition;
        RectTransform endMsgPosition2D;
        ulong dicEqpIndex = 0,rEqpIndex , r2EqpIndex = 0;
        string mWhisperName;
        public Text textItemCurrentPage;
        Transform itemChatMsg, itemChatMsgMe, itemSysMsg,
            panelSettings, panelBag,
            btn_tagSettings, btn_tagBag,
            btn_tagSettingsActive, btn_tagSettingsInactive,
            btn_tagBagActive, btn_tagBagInactive;
        Text textItemName, textItemInfo;
        enum IgnoreVoice
        {
            World = ChatToType.World,
            Legion = ChatToType.Legion,
            Team = ChatToType.Team,
            Whisper = ChatToType.Whisper,
        }
        enum IgnoreChat
        {
            Nearby = ChatToType.Nearby,
            Legion = ChatToType.Legion,
            Team = ChatToType.Team,
            Whisper = ChatToType.Whisper,
        }
        Dictionary<ChatToType, bool> ignoreVoiceStat = new Dictionary<ChatToType, bool>
        {
            [(ChatToType)IgnoreVoice.World] = false,
            [(ChatToType)IgnoreVoice.Legion] = false,
            [(ChatToType)IgnoreVoice.Team] = false,
            [(ChatToType)IgnoreVoice.Whisper] = false,
        },
        ignoreChatStat = new Dictionary<ChatToType, bool>
        {
            [(ChatToType)IgnoreChat.Nearby] = false,
            [(ChatToType)IgnoreChat.Legion] = false,
            [(ChatToType)IgnoreChat.Team] = false,
            [(ChatToType)IgnoreChat.Whisper] = false,
        };
        Text text_ignoreLegion, text_ignoreTeam, text_ignoreWhisper, text_ignoreNearby,
            text_ignoreVoiceLegion, text_ignoreVoiceTeam, text_ignoreVoiceWhisper, text_ignoreVoiceWorld;
        Transform iconChatMsg;
        Transform iconChatMsgMe;

        public int lengthLimit;
        private int currentDisplayEquipCount = 0;
        private float OffsetY; // 聊天窗口复位上弹高度
        public override void init()
        {
            inText();
            //GameSdkMgr.m_sdk.voiceRecordHanlde = onVoiceRecordedHandle;
            //GameSdkMgr.m_sdk.voicePlayedHanlde = onVoicePlayedHandle;
            _instance = this;
            AllItemMsg.Clear();
            chatToTypeMsgPostions = new Dictionary<ChatToType, Stack<RectTransform>>();
            dicPrivateChatToTypeMsgPostions = new Dictionary<string, Stack<RectTransform>>();
            chatToButtons = new Dictionary<ChatToType, Transform>();
            chatToPanels = new Dictionary<ChatToType, Transform>();
            btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onCloseClick;
            //bottom
            ipt_msg = transform.FindChild("bottom/InputField").GetComponent<RectTransform>();
            iptf_msg = ipt_msg.GetComponent<InputField>();
            minBag = transform.FindChild("bottom/minBag");
            quickMsg = transform.FindChild("bottom/quickMsg");
            BaseButton btn_bag = new BaseButton(transform.FindChild("bottom/btn_Funcs/btn_bag"));
            txtCurrentChat = getComponentByPath<Text>("bottom/txtCurrentChat/Text");
            BaseButton btn_pos = new BaseButton(transform.FindChild("bottom/btn_Funcs/btn_pos"));
            BaseButton btn_quickTalk = new BaseButton(transform.FindChild("bottom/btn_Funcs/btn_quickTalk"));
            BaseButton btn_sendMsg = new BaseButton(transform.FindChild("bottom/btn_sendMsg"));
            BaseButton btn_quickTalk_sl = new BaseButton(transform.FindChild("panels/PanelQucikTalk/btn_quickTalk_sl"));
            EventTriggerListener btn_voice = EventTriggerListener.Get(transform.FindChild("bottom/btn_Funcs/btn_voice").gameObject);

            btn_inBody = new BaseButton(transform.FindChild("bottom/minBag/BagPanel/btn_inBody"));
            btn_inBag = new BaseButton(transform.FindChild("bottom/minBag/BagPanel/btn_inBag"));
            Toggle tgPlus = getComponentByPath<Toggle>("bottom/tgPlus");
            tgPlus.onValueChanged.AddListener(onTgPlusClick);
            plusItems = transform.FindChild("bottom/btn_Funcs");
            //plusItems.gameObject.SetActive(false);
            lengthLimit = iptf_msg.transform.FindChild("limit").GetComponent<InputField>().characterLimit;
            iptf_msg.onValueChanged.AddListener((str) =>
            {
                if (str.Contains("\r\n"))
                {
                    str = str.Replace("\r\n", " ");

                    iptf_msg.text = str;

                }  // 回车 换成 " " 可能复制信息会造成该问题

                int expandLength = 0;
                int splitCnt = 0, lengthOnCnt = 0;
                if (str.Contains("[") && str.Contains("]"))
                {
                    string inputStr = str.Replace(']', '[');
                    string[] str_arr = inputStr.Split('[');
                    for (int i = 0; i < str_arr.Length; i++)
                    {
                        int cnt = 0;
                        for (int j = 0; j < str_arr[i].Length; j++)
                            if (str_arr[i][j] == '#') cnt++;
                        if (cnt == 1)
                        {
                            splitCnt++;
                            expandLength = 2 + expandLength + str_arr[i].Length;
                        }
                        else
                        {
                            if (lengthOnCnt + str_arr[i].Length <= lengthLimit)
                                lengthOnCnt = lengthOnCnt + str_arr[i].Length;
                        }
                        if (splitCnt == 3)
                            break;
                    }
                    currentDisplayEquipCount = splitCnt;
                }
                else currentDisplayEquipCount = 0;
                try
                {
                    if (str.Length > Mathf.Max(lengthOnCnt + expandLength, lengthLimit))
                        iptf_msg.text = str.Substring(0, expandLength + lengthLimit);
                }
                catch (Exception) { iptf_msg.text = str.Substring(0, lengthLimit); }
            });


            btn_voice.onDown = onVoiceDrag;
            btn_voice.onExit = onVoiceout;
            btn_voice.onUp = onVoiceUp;
            btn_bag.onClick = onBagClick;
            btn_pos.onClick = onPosClick;
            btn_quickTalk.onClick = onQuickTalkClick;
            btn_sendMsg.onClick = onSendMsgClick;
            btn_quickTalk_sl.onClick = (GameObject go) => { PanelQuickTalk.root.gameObject.SetActive(false); };
            btn_inBody.onClick = onBtnInBodyClick;
            btn_inBag.onClick = onBtnInBagClick;
            //msgShow
            btn_all = transform.FindChild("msgShow/left/btn_all").GetComponent<Toggle>();
            btn_world = transform.FindChild("msgShow/left/btn_world").GetComponent<Toggle>();
            btn_nearby = transform.FindChild("msgShow/left/btn_nearby").GetComponent<Toggle>();
            btn_legion = transform.FindChild("msgShow/left/btn_legion").GetComponent<Toggle>();
            btn_team = transform.FindChild("msgShow/left/btn_team").GetComponent<Toggle>();
            btn_secretlanguage = transform.FindChild("msgShow/left/btn_secretlanguage").GetComponent<Toggle>();
            btn_all.onValueChanged.AddListener(onAllClick);
            btn_world.onValueChanged.AddListener(onWorldClick);
            btn_nearby.onValueChanged.AddListener(onNearbyClick);
            btn_legion.onValueChanged.AddListener(onLegionClick);
            btn_team.onValueChanged.AddListener(onTeamClick);

            chatToButtons.Add(ChatToType.All, btn_all.transform);
            chatToButtons.Add(ChatToType.World, btn_world.transform);
            chatToButtons.Add(ChatToType.Nearby, btn_nearby.transform);
            chatToButtons.Add(ChatToType.Legion, btn_legion.transform);
            chatToButtons.Add(ChatToType.Team, btn_team.transform);
            chatToButtons.Add(ChatToType.Whisper, btn_secretlanguage.transform);
            //msgPanels
            panelAll = transform.FindChild("msgShow/right/PanelAll");
            panelWorld = transform.FindChild("msgShow/right/PanelWorld");
            panelNearby = transform.FindChild("msgShow/right/PanelNearby");
            panelLegion = transform.FindChild("msgShow/right/PanelLegion");
            panelTeam = transform.FindChild("msgShow/right/PanelTeam");
            panelScretlanguage = transform.FindChild("msgShow/right/PanelSecretlanguage");
            panel4Player.transform = transform.FindChild("panels/Panel4Player");
            dummyTop = panelAll.FindChild("viewMask/scroll/dummyTop").transform.position.y;
            dummyEnd = panelAll.FindChild("viewMask/scroll/dummyEnd").transform.position.y;
            panel4PlayerTouchBg = new BaseButton(transform.FindChild("panels/Panel4Player/bg"));
            endMsgPosition = transform.FindChild("msgShow/right/endPos");
            endMsgPosition2D = endMsgPosition.GetComponent<RectTransform>();
            // playerName Tip
            panel4Player.txt_name = transform.FindChild("panels/Panel4Player/txt_name").GetComponent<Text>();
            panel4Player.btn_see = new BaseButton(transform.FindChild("panels/Panel4Player/buttons/btn_see"));
            panel4Player.btn_see.onClick = onBtnSeePlayerInfo;
            panel4Player.btn_addFriend = new BaseButton(transform.FindChild("panels/Panel4Player/buttons/btn_addFriend"));
            panel4Player.btn_addFriend.onClick = onBtnAddFriendClick;
            panel4Player.btn_pinvite = new BaseButton(transform.FindChild("panels/Panel4Player/buttons/btn_pinvite"));
            panel4Player.btn_privateChat = new BaseButton(transform.FindChild("panels/Panel4Player/buttons/btn_privateChat"));
            panel4Player.btn_privateChat.onClick = onPrivateChatClick;
            chatToPanels.Add(ChatToType.All, panelAll);
            chatToPanels.Add(ChatToType.World, panelWorld);
            chatToPanels.Add(ChatToType.Nearby, panelNearby);
            chatToPanels.Add(ChatToType.Legion, panelLegion);
            chatToPanels.Add(ChatToType.Team, panelTeam);
            chatToPanels.Add(ChatToType.Whisper, panelScretlanguage);
            //chatToPanels.Add(ChatToType.PrivateSecretlanguage, panelScretlanguage);
            panel4PlayerTouchBg.onClick = onPanel4PlayerTouchBgClick;
            initItemChatMsgObjs();
            //minBag
            bodyPanel = transform.FindChild("bottom/minBag/BagPanel/bodyPanel");
            bodyPanelLScrollPage = bodyPanel.FindChild("ScrollView").gameObject.AddComponent<LScrollPage>();
            bodyPanelLScrollPage.smooting = 4.0f;
            bodyPanelContent = bodyPanel.FindChild("ScrollView/Content");
            bodyToggle = bodyPanel.FindChild("bodyToggle");

            bagPanel = transform.FindChild("bottom/minBag/BagPanel/bagPanel");
            bagPanelLScrollPage = bagPanel.FindChild("ScrollView").gameObject.AddComponent<LScrollPage>();
            bagPanelLScrollPage.smooting = 4.0f;
            bagPanelContent = bagPanel.FindChild("ScrollView/Content");
            bagToggle = bagPanel.FindChild("bagToggle");

            new BaseButton(transform.FindChild("bottom/minBag/btn_close")).onClick = (GameObject go) =>
            {
                go.transform.parent.gameObject.SetActive(false);
                onTgPlusClick(tgPlus.isOn = false);
            };
            //PanelFriends
            PanelFriends = transform.FindChild("panels/PanelFriends");
            whisper.mainBody = PanelFriends.FindChild("main");
            whisper.panelsSecretlanguage = transform.FindChild("msgShow/right/PanelsSecretlanguage");
            whisper.btn_Whisper = new BaseButton(PanelFriends.FindChild("btnWhisper"));
            whisper.btn_Whisper.onClick = onWhisperbtnClick;
            Transform tfFriends = PanelFriends.FindChild("main/friends");
            for (int i = 0; i < tfFriends.childCount; i++)
            {
                whisper wp = new whisper();
                wp.btn_Player = new BaseButton(tfFriends.GetChild(i));
                stackWhisper[i] = wp;
                tfFriends.GetChild(i).gameObject.SetActive(false);
            }

            //panelQuickTalk
            PanelQuickTalk.root = transform.FindChild("panels/PanelQucikTalk");
            PanelQuickTalk.btn01 = new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button01"));
            PanelQuickTalk.btn02 = new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button02"));
            PanelQuickTalk.btn03 = new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button03"));
            PanelQuickTalk.btn01.onClick = onPanelQuickTalkClick;
            PanelQuickTalk.btn02.onClick = onPanelQuickTalkClick;
            PanelQuickTalk.btn03.onClick = onPanelQuickTalkClick;
            //template
            prefabPageItem = transform.FindChild("template/PageItem");
            itemChatMsg = transform.FindChild("template/itemChatMsg");
            expandWidthMsgOthers = itemChatMsg.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta.x;
            expandHeightMsgOthers = itemChatMsg.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta.y;
            beginMsgOthersOffsetX = itemChatMsg.GetComponent<RectTransform>().anchoredPosition.y;
            itemChatMsgMe = transform.FindChild("template/itemChatMsgMe");
            expandWidthMsgMe = itemChatMsgMe.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta.x;
            expandHeightMsgMe = itemChatMsgMe.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta.y;
            beginMsgMeOffsetX = itemChatMsgMe.GetComponent<RectTransform>().anchoredPosition.y;
            itemSysMsg = transform.FindChild("template/itemSysMsg");
            itemChatCharName = transform.FindChild("template/itemChatMsg/itemChatCharName").gameObject;
            itemChatMsgConfig = transform.FindChild("template/itemChatMsgConfig").GetComponent<Text>();
            toggleGroup = transform.FindChild("template/toggleGroup");
            prefabWhisperPanel = transform.FindChild("template/PanelSecretlanguage");

            Transform tfConfig = transform.FindChild("template/config");
            msgLineSpace = tfConfig.GetComponent<Text>().lineSpacing * -1;
            firstMsgYInit = tfConfig.GetComponent<RectTransform>().anchoredPosition.y;
            //
            btn_all.onValueChanged.AddListener(onAllClick);
            btn_world.onValueChanged.AddListener(onWorldClick);
            btn_nearby.onValueChanged.AddListener(onNearbyClick);
            btn_legion.onValueChanged.AddListener(onLegionClick);
            btn_team.onValueChanged.AddListener(onTeamClick);
            btn_secretlanguage.onValueChanged.AddListener(onSecretLanguageClick);

            panel4Player.btn_addFriend.addEvent();
            //  UIClient.instance.addEventListener(UI_EVENT.GET_PUBLISH, otherSays);

            gameObject.SetActive(false);
            itemEPrefab = transform.FindChild("template/ItemE").gameObject;
            textItemCurrentPage = transform.FindChild("bottom/minBag/BagPanel/PageInfo/Text").GetComponent<Text>();
            (new BaseButton(transform.FindChild("bottom/minBag/BagPanel/Left"))).onClick =
            (GameObject go) =>
            {
                if (bodyPanel.gameObject.activeSelf)
                    bodyPanelLScrollPage.OnHardDrag(isLeftOrRight: true);
                else if (bagPanel.gameObject.activeSelf)
                    bagPanelLScrollPage.OnHardDrag(isLeftOrRight: true);
            };
            (new BaseButton(transform.FindChild("bottom/minBag/BagPanel/Right"))).onClick =
                (GameObject go) =>
                {
                    if (bodyPanel.gameObject.activeSelf)
                        bodyPanelLScrollPage.OnHardDrag(isLeftOrRight: false);
                    else if (bagPanel.gameObject.activeSelf)
                        bagPanelLScrollPage.OnHardDrag(isLeftOrRight: false);
                };
            textItemName = transform.Find("template/ItemE/ItemName").GetComponent<Text>();
            textItemInfo = transform.Find("template/ItemE/ItemInfo").GetComponent<Text>();
            new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button01/Text/Button01_Edit")).onClick =
            new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button02/Text/Button02_Edit")).onClick =
            new BaseButton(PanelQuickTalk.root.FindChild("buttons/Button03/Text/Button03_Edit")).onClick = onQuickTalkMsgEditClick;
            panelBag = transform.FindChild("bottom/minBag/BagPanel");
            panelSettings = transform.FindChild("bottom/minBag/SettingPanel");
            btn_tagSettings = transform.FindChild("bottom/minBag/btn_tagSettings");
            btn_tagSettingsInactive = btn_tagSettings.FindChild("btn_release");
            btn_tagSettingsActive = btn_tagSettings.FindChild("btn_down");
            btn_tagBag = transform.FindChild("bottom/minBag/btn_tagBag");
            btn_tagBagInactive = btn_tagBag.FindChild("btn_release");
            btn_tagBagActive = btn_tagBag.FindChild("btn_down");
            new BaseButton(btn_tagSettings).onClick = (GameObject go) =>
            {
                panelBag.gameObject.SetActive(false);
                btn_tagSettingsActive.gameObject.SetActive(true);
                btn_tagSettingsInactive.gameObject.SetActive(false);
                btn_tagBagActive.gameObject.SetActive(false);
                btn_tagBagInactive.gameObject.SetActive(true);
                panelSettings.gameObject.SetActive(true);
            };

            new BaseButton(btn_tagBag).onClick = (GameObject go) =>
            {
                panelBag.gameObject.SetActive(true);
                btn_tagSettingsActive.gameObject.SetActive(false);
                btn_tagSettingsInactive.gameObject.SetActive(true);
                btn_tagBagActive.gameObject.SetActive(true);
                btn_tagBagInactive.gameObject.SetActive(false);
                panelSettings.gameObject.SetActive(false);
            };
            AddListenerForToggles();
            OffsetY = GetIntValueFromMsgConfig("offset_y", itemChatMsgConfig);
        }


        int GetIntValueFromMsgConfig(string filter, Text textPrefab)
        {
            string str = textPrefab.text;
            string filterEnd = string.Format("</{0}>", filter);
            filter = string.Format("<{0}>", filter);

            int startIndex = str.IndexOf(filter) + filter.Length;
            string strVal = str.Substring(
                 startIndex: startIndex,
                 length: str.IndexOf(filterEnd) - startIndex
            );
            int nVal = 0;
            int.TryParse(strVal, out nVal);
            return nVal;
        }


        void inText()
        {
            this.transform.FindChild ("msgShow/left/btn_all/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_1");
            this.transform.FindChild("msgShow/left/btn_world/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_2");
            this.transform.FindChild("msgShow/left/btn_nearby/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_3");
            this.transform.FindChild("msgShow/left/btn_legion/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_4");
            this.transform.FindChild("msgShow/left/btn_team/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_5");
            this.transform.FindChild("msgShow/left/btn_secretlanguage/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_6");

            this.transform.FindChild("bottom/btn_sendMsg/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_7");
            this.transform.FindChild("bottom/minBag/btn_tagBag/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_8");
            this.transform.FindChild("bottom/minBag/btn_tagSettings/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_9");
            //this.transform.FindChild("bottom/minBag/btn_tagSettings/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_10");
            this.transform.FindChild("bottom/minBag/BagPanel/btn_inBody/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_11");
            this.transform.FindChild("bottom/minBag/BagPanel/btn_inBag/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_12");

            this.transform.FindChild("template/ClickArea/btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_13");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_14");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/Text2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_15");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWorld/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_16");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleLegion/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_17");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleTeam/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_18");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWhisper/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_19");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleLegion/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_20");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleTeam/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_21");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleWhisper/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_22");
            this.transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleNearby/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chatroom_23");
        }
        void AddListenerForToggles()
        {
            text_ignoreLegion = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleLegion/Label").GetComponent<Text>();
            text_ignoreTeam = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleTeam/Label").GetComponent<Text>();
            text_ignoreWhisper = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleWhisper/Label").GetComponent<Text>();
            text_ignoreNearby = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleNearby/Label").GetComponent<Text>();

            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleLegion")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreLegion.color = (ignoreChatStat[(ChatToType)IgnoreChat.Legion] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleTeam")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreTeam.color = (ignoreChatStat[(ChatToType)IgnoreChat.Team] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleWhisper")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreWhisper.color = (ignoreChatStat[(ChatToType)IgnoreChat.Whisper] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreChat/ToggleNearby")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreNearby.color = (ignoreChatStat[(ChatToType)IgnoreChat.Nearby] = isSelected) ? Color.green : Color.white);

            text_ignoreVoiceWorld = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWorld/Label").GetComponent<Text>();
            text_ignoreVoiceLegion = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleLegion/Label").GetComponent<Text>();
            text_ignoreVoiceTeam = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleTeam/Label").GetComponent<Text>();
            text_ignoreVoiceWhisper = transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWhisper/Label").GetComponent<Text>();

            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWorld")
                 .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreVoiceWorld.color = (ignoreVoiceStat[(ChatToType)IgnoreVoice.World] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleLegion")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreVoiceLegion.color = (ignoreVoiceStat[(ChatToType)IgnoreVoice.Legion] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleTeam")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreVoiceTeam.color = (ignoreVoiceStat[(ChatToType)IgnoreVoice.Team] = isSelected) ? Color.green : Color.white);
            transform.FindChild("bottom/minBag/SettingPanel/Panel/ToggleGrp_IgnoreVoice/ToggleWhisper")
                .GetComponent<Toggle>().onValueChanged.AddListener
                    ((bool isSelected) => text_ignoreVoiceWhisper.color = (ignoreVoiceStat[(ChatToType)IgnoreVoice.Whisper] = isSelected) ? Color.green : Color.white);
        }
        private void onQuickTalkMsgEditClick(GameObject go)
        {
            GameObject goEdit = go.transform.FindChild("Edit").gameObject;
            go.transform.FindChild("Save").gameObject.SetActive(goEdit.activeSelf);
            goEdit.SetActive(!goEdit.activeSelf);

            GameObject respondGo_inputField = go.transform.parent.parent.GetChild(1).gameObject;
            if (respondGo_inputField.activeSelf)
            {
                string content_inputField = respondGo_inputField.transform.GetChild(2).GetComponent<Text>().text;
                Text btnText = go.transform.GetComponentInParent<Text>();
                if (content_inputField.Length > 0)
                    btnText.text = new StringBuilder().AppendFormat("   {0}", content_inputField).ToString();
            }
            respondGo_inputField.SetActive(!respondGo_inputField.activeSelf);
        }
        public override void onShowed() {
            transform.SetAsLastSibling();
        }
        public override void onClosed()
        {
            foreach (var item in itemsGameObject)
                Destroy(item.gameObject);
            for (int i = bodyPanelContent.childCount - 1; i >= 0; i--)
                Destroy(bodyPanelContent.GetChild(i).gameObject);
            for (int i = bagPanelContent.childCount - 1; i >= 0; i--)
                Destroy(bagPanelContent.GetChild(i).gameObject);
            isLoaded = false;
            minBag.gameObject.SetActive(false);
            iptf_msg.text = "";
        }
        public void privateChat(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            if (!this.gameObject.activeSelf)
            {
                //chatToPanels[ChatToType.Secretlanguage].gameObject.SetActive(true);
                this.gameObject.SetActive(true);
            }
            this.gameObject.transform.localPosition = Vector3.zero;
            chatToType = ChatToType.Whisper;
            setChatToButtonGroup();
            hideMinChatTo();
            //chatToType = ChatToType.PrivateSecretlanguage;a
            chatToType = ChatToType.Whisper;
            setChatToPanel();
            //chatToType = ChatToType.Secretlanguage;
            if (!PanelFriends.gameObject.activeSelf)
                PanelFriends.gameObject.SetActive(true);
            createWhisperPanel(name);
            mWhisperName = name;
            iptf_msg.text = mWhisperName + "/";
        }
        private void DestroyItem()
        {
            if (bagPanelContent != null)
            {
                for (int i = 0; i < bagPanelContent.childCount; i++)
                {
                    Destroy(bagPanelContent.GetChild(i).gameObject);
                }
            }
            if (bodyPanelContent != null)
            {
                for (int i = 0; i < bodyPanelContent.childCount; i++)
                {
                    Destroy(bodyPanelContent.GetChild(i).gameObject);
                }
            }
            if (bodyToggle != null)
            {
                for (int i = 0; i < bodyToggle.childCount; i++)
                {
                    Destroy(bodyToggle.GetChild(i).gameObject);
                }
            }

        }
        void initItemChatMsgObjs()
        {
            itemChatMsgObjs = new Dictionary<ChatToType, ItemChatMsgObj>();
            foreach (KeyValuePair<ChatToType, Transform> item in chatToPanels)
            {
                ItemChatMsgObj tmp = new ItemChatMsgObj();
                RectTransform msgContainer = (RectTransform)item.Value.FindChild("viewMask/scroll/msgContainer");
                tmp.parent = msgContainer;
                itemChatMsgObjs.Add(item.Key, tmp);
            }


        }
        void onCloseClick(GameObject go)
        {
            //暂时屏蔽掉sdk关语音
            //GameSdkMgr.clearVoices();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHATROOM);
        }
        #region chatMain
        //语音
        long voicetimer = 0;
        void onVoiceDrag(GameObject go)
        {
            if (!checkCanSendMsg())
                return;
            voicetimer = NetClient.instance.CurServerTimeStampMS;
            Invoke("voiceNeedEnd", 30f);

            GameSdkMgr.beginVoiceRecord();
        }
        void onVoiceout(GameObject go)
        {
            CancelInvoke("voiceNeedEnd");
            GameSdkMgr.cancelVoiceRecord();
        }
        void onVoiceUp(GameObject go)
        {
            CancelInvoke("voiceNeedEnd");

            if (NetClient.instance.CurServerTimeStampMS - voicetimer < 600)
            {
                GameSdkMgr.cancelVoiceRecord();
            }
            else
            {
                GameSdkMgr.endVoiceRecord();
            }


        }
        void voiceNeedEnd()
        {

            GameSdkMgr.endVoiceRecord();
        }
        void onVoiceRecordedHandle(string state, string path, int sec)
        {
            if (state == "end")
            {
                if (!checkCanSendMsg())
                    return;
                sendMsg(path + "," + sec, true);
            }
            else if (state == "error") debug.Log("::voiceError::" + path);
            else if (state == "begin") { }
        }
        void onVoicePlayedHandle(string state)
        {
            debug.Log(".....endVoiceRecord loadHanlde:" + state);
            if (state == "played")
            {
            }
            else if (state == "error")
            {
            }
        }
        bool isLoaded = false;
        void onTgPlusClick(bool b)
        {
            if (!isLoaded)
            {
                loadItem();
                isLoaded = true;
            }
            minBag.gameObject.SetActive(!minBag.gameObject.activeSelf);
        }
        void onBagClick(GameObject g)
        {
            minBag.gameObject.SetActive(!minBag.gameObject.activeSelf);
        }
        void onPosClick(GameObject g)
        {
            string name = PlayerModel.getInstance().name;
            string mapName = GRMap.curSvrConf.ContainsKey("map_name") ? GRMap.curSvrConf["map_name"]._str : "--";
            iptf_msg.text += "[" + mapName + "(" + (int)SelfRole._inst.m_curModel.position.x + ","
                                 + (int)SelfRole._inst.m_curModel.position.z + ")]";
        }
        void onQuickTalkClick(GameObject g)
        {
            if (PanelQuickTalk.root.gameObject.activeSelf)
            {
                PanelQuickTalk.root.gameObject.SetActive(false);
            }
            else
            {
                PanelQuickTalk.root.gameObject.SetActive(true);
            }
        }
        string iptf_msgStr = string.Empty;
        float OutTimer = 0;
        void onSendMsgClick(GameObject g)//发送聊天信息
        {
            if (!checkCanSendMsg())
                return;
            string strMsg = iptf_msg.text;
            strMsg = KeyWord.filter(strMsg);
            if (string.IsNullOrEmpty(strMsg.Trim()))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_chatroom_nonull"));
                return;
            }

            //打印日志的开关
            if (strMsg == "debugopen")
            {
                debug.show_debug = true;
                PlayeLocalInfo.saveInt(PlayeLocalInfo.DEBUG_SHOW, 1);
            }
            if (strMsg == "debugclose")
            {
                debug.show_debug = false;
                PlayeLocalInfo.saveInt(PlayeLocalInfo.DEBUG_SHOW, 0);
            }

            sendMsg(strMsg, false);
        }
        bool checkCanSendMsg()
        {
            uint lvlUp = PlayerModel.getInstance().up_lvl;
            if ((chatToType == ChatToType.All || chatToType == ChatToType.World) && OutTimer > 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_chatroom_maxtell"));
                return false;
            }
            switch (chatToType)
            {
                case ChatToType.All:
                    if (lvlUp < 1)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_chatroom_openword"));                        
                        return false;
                    }                    
                    break;
                case ChatToType.Nearby:
                    break;
                case ChatToType.World:

                    if (lvlUp < 1)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_chatroom_openword"));
                        return false;
                    }
                    break;
                case ChatToType.Legion:
                    break;
                case ChatToType.Team:
                    break;
                case ChatToType.Whisper:
                    if (lvlUp < 1)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_chatroom_openmiyu"));
                        return false;
                    }
                    break;
                case ChatToType.PrivateSecretlanguage:
                    if (lvlUp < 1)
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_chatroom_openmiyu"));
                        return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        public void SendMsg(string strMsg, ChatToType chatType = ChatToType.Nearby,uint xtp = 0)
        {
            chatToButtons[chatToType].GetComponent<Toggle>().isOn = false;
            switch (chatType)
            {
                case ChatToType.All:
                    onAllClick(true);
                    break;
                case ChatToType.World:
                    onWorldClick(true);
                    break;
                case ChatToType.Legion:
                    onLegionClick(true);
                    break;
                case ChatToType.Nearby:
                    onNearbyClick(true);
                    break;
                case ChatToType.Team:
                    onTeamClick(true);
                    break;
                    //case ChatToType.PrivateSecretlanguage:
                    //    onPrivateChatClick();
                    //    break;                
            }
            sendMsg(strMsg, false, xtp);
        }

        void sendMsg(string strMsg, bool isvoice,uint xtp = 0)
        {
            if (strMsg == "")
                return;

            uint tp = (uint)chatToType;
            if (chatToType == ChatToType.All)
                tp = (uint)ChatToType.World;
            string name = PlayerModel.getInstance().name;
            iptf_msgStr = strMsg;
            if (chatToType == ChatToType.Whisper || chatToType == ChatToType.PrivateSecretlanguage)
            {
                string strMsgTemp = strMsg;
                //mWhisperName = strMsgTemp.Split('/')[0];
                //strMsg = strMsgTemp.Split('/')[1];
                //if (string.IsNullOrEmpty(mWhisperName))
                //{
                //    flytxt.instance.fly("请选择密语对象", 3);
                //    return;
                //}
                //else
                //{
                //    name = mWhisperName;
                //    strMsg = strMsg.Substring(strMsg.IndexOf('/') + 1);
                //    ChatProxy.getInstance().sendMsg(strMsg, name, (int)ChatToType.Whisper, isvoice);
                //    //meSays();
                //}
                if (!strMsgTemp.Contains(@"/"))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_chatroom_miyupeople"));
                    return;
                }
                else
                {
                    string[] msg = strMsgTemp.Split('/');
                    mWhisperName = msg[0];
                    strMsg = msg[1];
                    name = mWhisperName;
                    strMsg = strMsg.Substring(strMsg.IndexOf('/') + 1);
                    ChatProxy.getInstance().sendMsg(strMsg, name, (int)ChatToType.Whisper, isvoice ,xtp);
                    //meSays();
                }



            }
            else
            {
                if (chatToType == ChatToType.Team && !PlayerModel.getInstance().IsInATeam)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_chatroom_notall0"));
                    return;
                }
                if (chatToType == ChatToType.Legion && A3_LegionModel.getInstance().myLegion.id <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_chatroom_notall1"));
                    return;
                }
                ChatProxy.getInstance().sendMsg(strMsg, name, tp, isvoice, xtp);
                if (xtp == 0) meSays(isvoice);
            }

        }
        void onBtnInBodyClick(GameObject go)
        {
            if (bodyPanel == null || bagPanel == null)
                return;
            bodyPanel.gameObject.SetActive(true);
            bagPanel.gameObject.SetActive(false);
        }
        void onBtnInBagClick(GameObject go)
        {
            if (bodyPanel == null || bagPanel == null)
                return;
            bodyPanel.gameObject.SetActive(false);
            bagPanel.gameObject.SetActive(true);
        }
        bool isMeSay = false;
        public void meSays(bool isvoice)
        {
            isMeSay = true;
            chatData data = new chatData();
            if (chatToType == ChatToType.PrivateSecretlanguage || chatToType == ChatToType.Whisper)
            {
                data.cid = panel4Player.cid;
                data.name = mWhisperName;
                if (isvoice)
                    data.msg =
                    data.url = iptf_msgStr;
                else
                    data.msg = iptf_msgStr.Substring(iptf_msgStr.IndexOf('/') + 1);
                setChatToPanel();
            }
            else
            {
                data.cid = PlayerModel.getInstance().cid;
                data.name = PlayerModel.getInstance().name;
                if (isvoice)
                    data.msg =
                    data.url = iptf_msgStr;
                else
                    data.msg = iptf_msgStr;
            }
            data.tp = 0;
            data.vip = A3_VipModel.getInstance().Level;
            data.mapId = PlayerModel.getInstance().mapid;
            iptf_msg.text = string.Empty;
            switch (chatToType)
            {
                case ChatToType.All:
                    data.tp = 1;
                    OutTimer = 5.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.World:
                    data.tp = 1;
                    OutTimer = 5.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.Nearby:
                    PlayerChatUIMgr.getInstance().show(SelfRole._inst, ChatProxy.getInstance().analysisStrName(data.msg));
                    data.tp = 2;
                    OutTimer = 3.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.Legion:
                    data.tp = 3;
                    OutTimer = 3.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.Team:
                    data.tp = 4;
                    OutTimer = 3.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.Whisper:
                    data.tp = 5;
                    OutTimer = 3.0f;
                    showMsgInPanel(data);
                    break;
                case ChatToType.PrivateSecretlanguage:
                    data.tp = 6;
                    OutTimer = 3.0f;
                    showMsgInPanel(data);
                    break;
                default:
                    break;
            }
            //TOOO
        }
        public void otherSays(Variant v,List<Variant> lp = null)
        {
            isMeSay = false;
            chatData data = new chatData();
            if (v.ContainsKey("tp")) data.tp = v["tp"];
            if ((ChatToType)data.tp != ChatToType.SystemMsg && (ChatToType)data.tp != ChatToType.LegionSystemMsg)
            {
                data.cid = v["cid"];
                data.name = v["name"];
            }
            data.msg = v["msg"];
            if (v.ContainsKey("vip")) data.vip = v["vip"];
            if (v.ContainsKey("items")) data.items = v["items"]._arr;

            if (v.ContainsKey("mpid"))
            {
                data.mapId = v["mpid"]._uint;
                data.x = v["x"]._int;
                data.y = v["y"]._int;
            }

            if (v.ContainsKey("url"))
            {
                data.msg = "--";
                data.url = v["url"];
            }
            if (v.ContainsKey("carr"))
            {
                data.carr = v["carr"];
            }
            bool custom = v.ContainsKey("custom") && lp != null;
            int removeCount = 0;
            if (data.items != null)
            {                
                for (int i = 0; i < data.items.Count; i++)
                {
                    equipIndexLst.Add(dicEqpIndex);
                    dicEquip[dicEqpIndex++] = data.items[i];

                    if (dicEquip.Count > 100) {
                        dicEquip.Remove(equipIndexLst[removeCount]);
                        removeCount++;
                    } 

                }

               if( removeCount > 0 ) equipIndexLst.RemoveRange(0, removeCount);

            }

            switch ((ChatToType)data.tp)
            {
                case ChatToType.All:
                    if (custom)
                        ShowSysMsgRedefn(data, lp);
                    else
                        showMsgInPanel(data);
                    break;
                case ChatToType.World:
                    if (custom)
                        ShowSysMsgRedefn(data, lp);
                    else
                        showMsgInPanel(data);
                    break;
                case ChatToType.Nearby:
                    if (custom)
                        ShowSysMsgRedefn(data, lp);
                    else
                        showMsgInPanel(data);
                    break;
                case ChatToType.Legion:
                    if (custom)
                        ShowSysMsgRedefn(data, lp);
                    else
                        showMsgInPanel(data);
                    break;
                case ChatToType.Team:
                    showMsgInPanel(data);
                    break;
                case ChatToType.Whisper:
                    showMsgInPanel(data);
                    break;
                case ChatToType.SystemMsg:
                case ChatToType.LegionSystemMsg:
                    if (custom)
                        ShowSysMsgRedefn(data, lp);
                    else
                        showMsgInPanel(data);
                    break;
                case ChatToType.PrivateSecretlanguage:
                    showMsgInPanel(data);
                    break;
                default:
                    break;
            }
            //for (int i = 0; i < removeCount; i++)
            //{
            //    dicEquip.Remove(dicEquip.Keys.ToList()[0]);
            //}
       
        }
        bool createOnceInExpbar = true;

        public void ShowSysMsgRedefn(chatData data,List<Variant> lp)
        {
            listPara = lp;
            createOnceInExpbar = true;
            createMsgObjInPanel( data , ChatToType.All , MsgDeal );
        }

        List<Variant> listPara;
        public void MsgDeal()
        {
            if (listPara == null) return;
            msgDic.Clear();
            for (int i = 0; i < listPara.Count; i++)
            {
                msgItemInfo msgII = null;
                if (listPara[i].ContainsKey("msg"))
                {
                    int r, g, b;
                    if (!listPara[i].ContainsKey("r")) r = 255;
                    else r = listPara[i]["r"];
                    if (!listPara[i].ContainsKey("g")) g = 255;
                    else g = listPara[i]["g"];
                    if (!listPara[i].ContainsKey("b")) b = 255;
                    else b = listPara[i]["b"];
                    msgII = new msgItemInfo
                    {
                        msgtype = msgType.customx,
                        color = new Color(r, g, b),
                        msg = listPara[i]["msg"]._str
                    };
                    if (listPara[i].ContainsKey("tid"))
                    {
                        msgII.xtp = 1;
                        msgII.data = new Variant { ["tid"] = listPara[i]["tid"] };
                    }
                    msgDic.Enqueue(msgII);
                }
              
            }
            listPara.Clear();
        }
       
        void showMsgInPanel(chatData data, ChatToType? chatType = null)
        {
            ChatToType _chatType = chatType.GetValueOrDefault(chatToType);
            ChatToType prevChatToType = chatToType;
            createOnceInExpbar = true;
            if (string.IsNullOrEmpty(data.msg)) return;            
            if (isMeSay)
            {
                if (_chatType == ChatToType.All)
                {
                    createMsgObjInPanel(data, ChatToType.World);
                }
                else if (_chatType != ChatToType.PrivateSecretlanguage)
                {
                    createMsgObjInPanel(data, _chatType);

                }
                if (_chatType == ChatToType.PrivateSecretlanguage)
                {

                    createMsgObjInPanel(data, _chatType);
                    createMsgObjInPanel(data, ChatToType.Whisper);
                }
                if (_chatType == ChatToType.Whisper)
                {
                    createMsgObjInPanel(data, ChatToType.PrivateSecretlanguage);
                }
            }
            else
            {
                //ChatToType prevChatToType = chatToType;
                chatToType = (ChatToType)data.tp;
                if (chatToType == ChatToType.SystemMsg)
                    chatToType = ChatToType.All;
                else if (chatToType == ChatToType.LegionSystemMsg)
                    chatToType = ChatToType.Legion;
                if ((ChatToType)data.tp == ChatToType.Whisper)
                {
                    chatToType = ChatToType.PrivateSecretlanguage;
                    createWhisperPanel(data.name);
                    if (chatToType != ChatToType.PrivateSecretlanguage)
                    {
                        dicWhisper[data.name].gameObject.SetActive(false);
                    }
                    createMsgObjInPanel(data, ChatToType.Whisper);
                    createMsgObjInPanel(data, ChatToType.PrivateSecretlanguage);
                }
                else
                {
                    if ((ChatToType)data.tp != ChatToType.SystemMsg && (ChatToType)data.tp != ChatToType.LegionSystemMsg)
                        createMsgObjInPanel(data, chatToType);
                }
                //chatToType = prevChatToType;  显示错误标签的bug
            }
            ChatToType ctp;
            if (chatToType == ChatToType.LegionSystemMsg)
                ctp = ChatToType.Legion;
            else
                ctp = ChatToType.All;
            if ( !ignoreChatStat.ContainsKey( ( ChatToType ) data.tp ) || !ignoreChatStat[ ( ChatToType ) data.tp ] ) {

                if ( data.items != null && data.items.Count > 0 )
                {
                    rEqpIndex  =  rEqpIndex - (ulong) data.items.Count;

                    r2EqpIndex = rEqpIndex;
                }

                createMsgObjInPanel( data , ctp );
            }
                

            chatToType = prevChatToType;  //显示错误标签的bug 修改

        }
        List<msg4roll> rollWordStrList = new List<msg4roll>();
        int LRichTextWidth = 0;
        public Dictionary<ChatToType, bool> IsMsgOutOfRange = new Dictionary<ChatToType, bool>
        {
            [ChatToType.All] = false,
            [ChatToType.Legion] = false,
            [ChatToType.Nearby] = false,
            [ChatToType.Team] = false,
            [ChatToType.World] = false,
            [ChatToType.Whisper] = false,
            [ChatToType.PrivateSecretlanguage] = false
        };
        void createMsgObjInPanel(chatData data, ChatToType ctp,Action dataHandle = null)
        {
            if (ctp == ChatToType.PrivateSecretlanguage)
                return;
            rollWordStrList.Clear();
            bool isInWhisper = true;
            GameObject itemMsg;
            Transform headIcon = null;
            Text chatMsg = null, channelMsg = null, vipText = null;
            bool isSysMsg = (ChatToType)data.tp == ChatToType.SystemMsg || (ChatToType)data.tp == ChatToType.LegionSystemMsg;
            #region init_prfab
            if (isSysMsg)
                itemMsg = Instantiate(itemSysMsg.gameObject) as GameObject;
            else
            {
                if (isMeSay)
                {
                    itemMsg = Instantiate(itemChatMsgMe.gameObject) as GameObject;
                    chatMsg = itemMsg.transform.FindChild("itemChatCharName").GetComponent<Text>();
                    chatMsg.text = PlayerModel.getInstance().name;
                    headIcon = itemMsg.transform.FindChild("HeadIconMan");
                    Instantiate(transform.FindChild(string.Format("template/HeadIcon/{0}", PlayerModel.getInstance().profession)).gameObject).transform.SetParent(headIcon, false);
                }
                else
                {
                    itemMsg = Instantiate(itemChatMsg.gameObject) as GameObject;
                    chatMsg = itemMsg.transform.FindChild("itemChatCharName").GetComponent<Text>();
                    chatMsg.text = data.name;
                    headIcon = itemMsg.transform.FindChild("HeadIconMan");
                    Instantiate(transform.FindChild(string.Format("template/HeadIcon/{0}", data.carr.ToString())).gameObject).transform.SetParent(headIcon, false);
                }

                channelMsg = itemMsg.transform.FindChild("Channel").GetComponent<Text>();
                vipText = itemMsg.transform.FindChild("VipLevel/Text").GetComponent<Text>();
                chatMsg.color = Color.white;
                chatMsg.fontSize = itemChatMsgConfig.fontSize;
            }
            #endregion
            #region adjust_xy
            if (ctp != ChatToType.PrivateSecretlanguage)
            {
                if (ctp == ChatToType.SystemMsg)
                    itemMsg.transform.SetParent(itemChatMsgObjs[ChatToType.All].parent, false);
                else if(ctp == ChatToType.LegionSystemMsg)
                    itemMsg.transform.SetParent(itemChatMsgObjs[ChatToType.Legion].parent, false);
                else
                    itemMsg.transform.SetParent(itemChatMsgObjs[ctp].parent, false);
                LRichTextWidth = 450;//(int)itemChatMsgObjs[ctp].parent.GetComponent<RectTransform>().sizeDelta.x - 100;
            }
            else
            {
                if (!dicWhisper.ContainsKey(data.name))
                    createWhisperPanel(data.name);
                dicWhisper[data.name].transform.gameObject.SetActive(true);
                Transform rtf = dicWhisper[data.name].FindChild("viewMask/scroll/msgContainer");
                itemMsg.transform.SetParent(rtf, false);
                LRichTextWidth = 450;//(int)rtf.GetComponent<RectTransform>().sizeDelta.x - 100;
            }
            //itemMsg.transform.localScale = Vector3.one;
            LRichText lrt_msg;
            if (ctp == ChatToType.SystemMsg || ctp == ChatToType.LegionSystemMsg)
                lrt_msg = itemMsg.AddComponent<LRichText>();
            else
            {
                Transform msgBdy = itemMsg.transform.FindChild("rect/msgBody");
                if (msgBdy == null)
                    return;
                lrt_msg = msgBdy.gameObject.AddComponent<LRichText>();
            }
            AllItemMsg.Add(itemMsg.transform);
            lrt_msg.font = itemChatMsgConfig.font;
            lrt_msg.maxLineWidth = LRichTextWidth;
            #endregion
            if (data.tp != (uint)ChatToType.Whisper && data.tp != (uint)ChatToType.PrivateSecretlanguage)
            {
                if (data.tp == (uint)ChatToType.SystemMsg || data.tp == (uint)ChatToType.LegionSystemMsg)
                {
                    string chatToTypeStr = getChatToStr(ChatToType.SystemMsg);
                    msg4roll msr = new msg4roll();
                    msr.msgT = msgType.chatTo;
                    msr.msgStr = chatToTypeStr;
                    msr.color = Color.red;
                    rollWordStrList.Add(msr);
                    lrt_msg.insertElement(chatToTypeStr, /*getWordColor()*/Color.red, itemChatMsgConfig.fontSize, false, false, getWordColor(), "");
                    string dataName = data.cid == PlayerModel.getInstance().cid ? "" : "name:" + data.name + ":" + data.cid;
                    lrt_msg.insertElement(data.name + ":", Color.white, itemChatMsgConfig.fontSize, false, false, Color.blue, "");
                    msg4roll msrName = new msg4roll();
                    msrName.msgT = msgType.non;
                    msrName.msgStr = data.name + ":";
                    msrName.color = Color.white;
                    rollWordStrList.Add(msrName);
                }
                else
                {
                    string chatToTypeStr = getChatToStr(ctp);
                    msg4roll msr = new msg4roll();
                    msr.msgT = msgType.chatTo;
                    msr.msgStr = chatToTypeStr;
                    msr.color = getWordColor();
                    rollWordStrList.Add(msr);
                    channelMsg.text = chatToTypeStr;
                    channelMsg.color = getWordColor();
                    channelMsg.fontSize = itemChatMsgConfig.fontSize;
                    if (data.vip > 0)
                        vipText.text = data.vip.ToString();
                    else
                        vipText.text = "0";
                    //{
                    //    lrt_msg.insertElement("V" + data.vip, getWordColor(), itemChatMsgConfig.fontSize, false, false, Color.blue, "");
                    //    msg4roll msrV = new msg4roll();
                    //    msrV.msgT = msgType.vip;
                    //    msrV.msgStr = "V";
                    //    msrV.color = getWordColor();
                    //    rollWordStrList.Add(msrV);


                    //}

                    string dataName = data.cid == PlayerModel.getInstance().cid ? "" : "name:" + data.name + ":" + data.cid;
                    //charName_msg.insertElement(data.name + "：", Color.white, itemChatMsgConfig.fontSize, false, false, Color.blue, dataName);

                    msg4roll msrName = new msg4roll();
                    msrName.msgT = msgType.uname;
                    msrName.msgStr = data.name + ":";
                    msrName.color = Color.white;
                    msrName.data = dataName;
                    rollWordStrList.Add(msrName);
                }

            }
            else
            {
                if (isMeSay)
                {
                    if (panel4Player.name == null) panel4Player.name = mWhisperName;
                    lrt_msg.insertElement(ContMgr.getCont("wo"), Color.white, itemChatMsgConfig.fontSize, false, false, Color.white, "");
                    msg4roll msrSc = new msg4roll();
                    msrSc.msgT = msgType.non;
                    msrSc.msgStr = ContMgr.getCont("wo");
                    msrSc.color = Color.white;
                    rollWordStrList.Add(msrSc);

                    lrt_msg.insertElement(ContMgr.getCont("mi"), Color.magenta, itemChatMsgConfig.fontSize, false, false, Color.magenta, "");
                    msg4roll msr01 = new msg4roll();
                    msr01.msgT = msgType.non;
                    msr01.msgStr = ContMgr.getCont("mi");
                    msr01.color = Color.white;
                    rollWordStrList.Add(msr01);

                    lrt_msg.insertElement(data.name + ":", getWordColor(), itemChatMsgConfig.fontSize, false, false, getWordColor(), "name:" + data.name + ":" + data.cid);
                    msg4roll msr02 = new msg4roll();
                    msr02.msgT = msgType.uname;
                    msr02.msgStr = data.name + ":";
                    msr02.color = getWordColor();
                    msr02.data = "name:" + data.name + ":" + data.cid;
                    rollWordStrList.Add(msr02);

                    // lrt_msg.insertElement(1);

                    for (int i = 0; i < stackWhisper.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(stackWhisper[i].name))
                        {
                            if (stackWhisper[i].name.Equals(data.name))
                            {
                                isInWhisper = true;
                                break;
                            }
                        }

                        isInWhisper = false;
                    }
                    if (!isInWhisper && string.IsNullOrEmpty(stackWhisper[4].name))
                    {
                        for (int i = 0; i < stackWhisper.Length; i++)
                        {
                            if (stackWhisper[i].name == null)
                            {
                                whisper wp = new whisper();
                                wp.btn_Player = stackWhisper[i].btn_Player;
                                wp.name = data.name;
                                wp.cid = data.cid;
                                wp.btn_Player.gameObject.SetActive(true);
                                wp.btn_Player.gameObject.transform.FindChild("Text").GetComponent<Text>().text = data.name;
                                wp.btn_Player.onClick = delegate (GameObject go) { onBtnPlayerWhisperClick(wp.cid, wp.name); };
                                stackWhisper[i] = wp;
                                break;
                            }
                        }
                    }
                    else if (!isInWhisper && !string.IsNullOrEmpty(stackWhisper[4].name))
                    {
                        whisper tempWhisper = new whisper();
                        tempWhisper = stackWhisper[0];
                        for (int i = 0; i < stackWhisper.Length; i++)
                        {
                            if (i < stackWhisper.Length - 1)
                            {
                                stackWhisper[i] = stackWhisper[i + 1];
                                tempWhisper = stackWhisper[i + 1];
                            }
                            else
                            {
                                whisper wp = new whisper();
                                wp.btn_Player = stackWhisper[i].btn_Player;
                                wp.cid = data.cid;
                                wp.name = data.name;
                                wp.btn_Player.gameObject.SetActive(true);
                                wp.btn_Player.gameObject.transform.FindChild("Text").GetComponent<Text>().text = data.name;
                                wp.btn_Player.onClick = delegate (GameObject go) { onBtnPlayerWhisperClick(wp.cid, wp.name); };
                                stackWhisper[i] = wp;
                            }
                        }
                    }
                }
                else
                {
                    lrt_msg.insertElement(data.name, getWordColor(), itemChatMsgConfig.fontSize, false, false, Color.white, "name:" + data.name + ":" + data.cid);
                    msg4roll msr0 = new msg4roll();
                    msr0.msgT = msgType.uname;
                    msr0.msgStr = data.name;
                    msr0.color = getWordColor();
                    msr0.data = "name:" + data.name + ":" + data.cid;
                    rollWordStrList.Add(msr0);
                    lrt_msg.insertElement(ContMgr.getCont("mi"), Color.magenta, itemChatMsgConfig.fontSize, false, false, Color.magenta, "");
                    msg4roll msr01 = new msg4roll();
                    msr01.msgT = msgType.non;
                    msr01.msgStr = ContMgr.getCont("mi");
                    msr01.color = getWordColor();
                    rollWordStrList.Add(msr01);

                    lrt_msg.insertElement(ContMgr.getCont("wo"), Color.white, itemChatMsgConfig.fontSize, false, false, Color.white, "");
                    msg4roll msr02 = new msg4roll();
                    msr02.msgT = msgType.non;
                    msr02.msgStr = ContMgr.getCont("wo");
                    msr02.color = Color.white;
                    rollWordStrList.Add(msr02);

                    lrt_msg.insertElement(":", getWordColor(), itemChatMsgConfig.fontSize, false, false, getWordColor(), "");

                    msg4roll msr03 = new msg4roll();
                    msr03.msgT = msgType.non;
                    msr03.msgStr = ":";
                    msr03.color = getWordColor();
                    rollWordStrList.Add(msr03);



                    // lrt_msg.insertElement(1);

                    for (int i = 0; i < stackWhisper.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(stackWhisper[i].name))
                        {
                            if (stackWhisper[i].name.Equals(data.name))
                            {
                                isInWhisper = true;
                                break;
                            }
                        }
                        isInWhisper = false;
                    }
                    if (!isInWhisper && string.IsNullOrEmpty(stackWhisper[4].name))
                    {
                        for (int i = 0; i < stackWhisper.Length; i++)
                        {
                            if (stackWhisper[i].name == null)
                            {
                                whisper wp = new whisper();
                                wp.btn_Player = stackWhisper[i].btn_Player;
                                wp.cid = data.cid;
                                wp.name = data.name;
                                wp.btn_Player.gameObject.SetActive(true);
                                wp.btn_Player.gameObject.transform.FindChild("Text").GetComponent<Text>().text = data.name;
                                wp.btn_Player.onClick = delegate (GameObject go) { onBtnPlayerWhisperClick(wp.cid, wp.name); };
                                stackWhisper[i] = wp;
                                break;
                            }
                        }
                    }
                    else if (!isInWhisper && !string.IsNullOrEmpty(stackWhisper[4].name))
                    {
                        whisper tempWhisper = new whisper();
                        tempWhisper = stackWhisper[0];
                        for (int i = 0; i < stackWhisper.Length; i++)
                        {
                            if (i < stackWhisper.Length - 1)
                            {
                                stackWhisper[i] = stackWhisper[i + 1];
                                tempWhisper = stackWhisper[i + 1];
                            }
                            else
                            {
                                whisper wp = new whisper();
                                wp.btn_Player = stackWhisper[i].btn_Player;
                                wp.name = data.name;
                                wp.btn_Player.gameObject.SetActive(true);
                                wp.btn_Player.gameObject.transform.FindChild("Text").GetComponent<Text>().text = data.name;
                                wp.btn_Player.onClick = delegate (GameObject go) { onBtnPlayerWhisperClick(wp.cid, wp.name); };
                                stackWhisper[i] = wp;
                            }
                        }
                    }
                }
            }



            if (data.url != null)
            {
                string[] arr = data.url.Split(',');
                string strV = ContMgr.getCont("chat_voice", arr[1]);
                lrt_msg.insertElement(strV, Color.gray, itemChatMsgConfig.fontSize, false, false, Color.gray, "voice:" + arr[0]);

                msg4roll msr = new msg4roll();
                msr.msgT = msgType.voice;
                msr.msgStr = strV;
                msr.color = Color.gray;
                msr.data = "voice:" + arr[0];
                rollWordStrList.Add(msr);

            }
            else
            {
                if (dataHandle == null)
                    analysisStr(data);
                else
                    dataHandle();
                foreach (var item in msgDic)
                {
                    Color color = Globle.getColorByQuality(item.quality);
                    switch (item.msgtype)
                    {
                        case msgType.non:
                            lrt_msg.insertElement(item.msg, Color.white, itemChatMsgConfig.fontSize, false, false, Color.white, "");

                            msg4roll msr03 = new msg4roll();
                            msr03.msgT = msgType.non;
                            msr03.msgStr = item.msg;
                            msr03.color = Color.white;
                            rollWordStrList.Add(msr03);

                            break;
                        case msgType.item:
                            lrt_msg.insertElement("【" + item.msg + "】", color, itemChatMsgConfig.fontSize, false, false, color, "item:" + (isMeSay ? item.tpid : rEqpIndex++) );
                            msg4roll msr04 = new msg4roll();
                            msr04.msgT = msgType.item;
                            msr04.msgStr = "【" + item.msg + "】";
                            msr04.color = color;
                            msr04.data = "item:" + item.tpid;
                            rollWordStrList.Add(msr04);

                            break;
                        case msgType.equip:
                            if(!isMeSay)
                                color = Globle.getColorByQuality(a3_BagModel.getInstance().getItemDataById(dicEquip[rEqpIndex]["tpid"])?.quality ?? 0);
                            lrt_msg.insertElement("【" + item.msg + "】", color, itemChatMsgConfig.fontSize, false, false, color, "equip:" + (isMeSay ? item.id : rEqpIndex++ ));
                            msg4roll msr05 = new msg4roll();
                            msr05.msgT = msgType.equip;
                            msr05.msgStr = "【" + item.msg + "】";
                            msr05.color = color;
                            msr05.data = "equip:" + item.id;// item.id;
                            rollWordStrList.Add(msr05);
                            break;
                        case msgType.pos:
                            lrt_msg.insertElement("【", getWordColor(), 20, false, false, getWordColor(), "");
                            lrt_msg.insertElement(item.msg, getWordColor(), itemChatMsgConfig.fontSize, false, false, Color.blue, "position:" + item.msg + data.mapId);
                            msg4roll msr06 = new msg4roll();
                            msr06.msgT = msgType.pos;
                            msr06.msgStr = "【" + item.msg + "】";
                            msr06.color = getWordColor();
                            msr06.data = "position:" + item.msg + data.mapId;
                            rollWordStrList.Add(msr06);

                            lrt_msg.insertElement("】", getWordColor(), 20, false, false, getWordColor(), "");
                            break;
                        case msgType.uname:
                            lrt_msg.insertElement(" " + item.uname, new Color(254, 255, 255, 1.0f), itemChatMsgConfig.fontSize, false, false, Color.grey, "name:" + item.uname + ":" + item.cid);
                            msg4roll msr07 = new msg4roll();
                            msr07.msgT = msgType.uname;
                            msr07.msgStr = item.uname;
                            msr07.color = new Color(254, 255, 255, 1.0f);
                            msr07.data = "name:" + item.uname + ":" + item.cid;
                            rollWordStrList.Add(msr07);

                            break;
                        case msgType.custom:
                            color = Globle.getColorById(item.colorId);
                            lrt_msg.insertElement(item.msg, color, itemChatMsgConfig.fontSize, false, false, color, "");
                            msg4roll msr08 = new msg4roll();
                            msr08.msgT = msgType.non;
                            msr08.msgStr = item.msg;
                            msr08.color = color;
                            rollWordStrList.Add(msr08);
                            break;
                        case msgType.customx:
                            color = item.color;
                            msg4roll msr09 = new msg4roll();
                            msr09.msgT = msgType.non;
                            msr09.msgStr = item.msg;
                            msr09.color = color;
                            if (item.xtp == 1) msr09.data = "team:" + item +":" + item.data["tid"];
                            rollWordStrList.Add(msr09);
                            lrt_msg.insertElement(item.msg, color, itemChatMsgConfig.fontSize, false, false, color, msr09.data);
                            break;
                        default:
                            break;

                    }
                }

            }
            if (createOnceInExpbar)
            {
                a3_expbar.instance.setRollWord(rollWordStrList);
                createOnceInExpbar = false;
            }
            setLRichTextAction(lrt_msg);
            lrt_msg.reloadData();
            float recY = firstMsgYInit;//-25
            Stack<RectTransform> stack;
            if (chatToTypeMsgPostions.TryGetValue(ctp, out stack))
            {
                Vector2 sizeD = stack.Peek().sizeDelta;
                Vector2 localScale = stack.Peek().localScale;
                itemMsg.transform.localPosition = new Vector2(beginMsgOthersOffsetX, recY = -sizeD.y * localScale.y + stack.Peek().localPosition.y - 25);
                chatToTypeMsgPostions[ctp].Push(itemMsg.GetComponent<RectTransform>());
                Vector2 sizeItem = itemMsg.GetComponent<RectTransform>().sizeDelta;
                Vector2 localScalItem = itemMsg.GetComponent<RectTransform>().localScale;
                Vector2 posItem = itemMsg.GetComponent<RectTransform>().anchoredPosition;
                float durationY = itemMsg.transform.position.y - ((sizeItem.y * localScalItem.y));
                if (IsMsgOutOfRange[ctp] || (IsMsgOutOfRange[ctp] = durationY < endMsgPosition.position.y))
                {
                    if (itemChatMsgObjs.ContainsKey(ctp))
                    {
                        Vector2 sdCom = itemChatMsgObjs[ctp].parent.GetComponent<RectTransform>().sizeDelta;
                        itemChatMsgObjs[ctp].parent.GetComponent<RectTransform>().sizeDelta = new Vector2
                            (sdCom.x, sdCom.y + Mathf.Abs(yLastMessage[ctp] - (Mathf.Abs(posItem.y) + sizeItem.y)));
                    }
                }
                if (durationY < endMsgPosition.position.y)
                {
                    var rectObj = itemChatMsgObjs[ctp].parent.GetComponent<RectTransform>();
                    rectObj.DOKill();
                    float offsetY = OffsetY;
                    float posY = itemChatMsgObjs[ctp].parent.GetComponent<RectTransform>().anchoredPosition.y +
                        Mathf.Abs(yLastMessage[ctp] - (Mathf.Abs(posItem.y) + sizeItem.y)) + msgLineSpace + offsetY;
                    rectObj.DOLocalMoveY(posY, 1.0f).OnKill(delegate ()
                    {
                        rectObj.anchoredPosition = new Vector2(rectObj.anchoredPosition.x, posY);
                    });
                }
            }
            else
            {
                itemMsg.transform.localPosition = new Vector2(beginMsgOthersOffsetX, -25);
                Stack<RectTransform> qrt = new Stack<RectTransform>();
                qrt.Push(itemMsg.GetComponent<RectTransform>());
                chatToTypeMsgPostions.Add(ctp, qrt);
            }
           

            if (chatToType == ChatToType.SystemMsg) chatToType = ChatToType.All;

            itemMsg.transform.localScale = Vector3.one;
            RectTransform rect = itemMsg.GetComponent<RectTransform>();

            RectTransform container=null;
            if ( itemChatMsgObjs.ContainsKey( ctp ) )
            {
                container = itemChatMsgObjs[ ctp ].parent.GetComponent<RectTransform>();
            }
            //rectBG = itemMsg.transform.FindChild("rect").GetComponent<RectTransform>();
            //if (ctp != ChatToType.SystemMsg)
            if (isMeSay)
            {
                rect.pivot = Vector2.one;
                rect.anchoredPosition = new Vector2(-50, recY + msgLineSpace);
                rect.sizeDelta = rect.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta + new Vector2(expandWidthMsgMe, expandHeightMsgMe);
            }
            else
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, recY + msgLineSpace);
                rect.sizeDelta = rect.transform.FindChild("rect/msgBody").GetComponent<RectTransform>().sizeDelta + new Vector2(expandWidthMsgOthers, expandHeightMsgOthers);
            }

            if ( yLastMessage.ContainsKey( ctp ) )
            {
                yLastMessage[ ctp ] = Mathf.Abs( itemMsg.GetComponent<RectTransform>().anchoredPosition.y ) + itemMsg.GetComponent<RectTransform>().sizeDelta.y;
                if ( container ) container.sizeDelta = new Vector2( container.sizeDelta.x , yLastMessage[ ctp ] );
            }
            else
            {
                yLastMessage.Add( ctp , Mathf.Abs( itemMsg.GetComponent<RectTransform>().anchoredPosition.y ) + itemMsg.GetComponent<RectTransform>().sizeDelta.y );
                if ( container ) container.sizeDelta = new Vector2( container.sizeDelta.x , yLastMessage[ ctp ] );
            }

            if (itemMsg != null)
            {
                Transform bodyMsg = itemMsg.transform.FindChild("rect/msgBody");
                if (bodyMsg != null)
                    for (int i = bodyMsg.childCount; i > 0; i--)
                        bodyMsg.GetChild(i - 1).localScale = Vector3.one;
            }
               
        }
        //uint itemIndex = 0;
        void analysisStr(chatData data)
        {
            //itemIndex = 0;
            msgDic.Clear();
            string str = data.msg;
            if (str == null) return;

            if (str.Contains('[') && str.Contains(']'))
            {

                string newStr = str.Replace("]", "[");
                string[] strMsgArr = newStr.Split('[');//Regex.Split(newStr, @"\[.*?\]");                
                for (int i = 0; i < strMsgArr.Length; i++)
                {
                    if (string.IsNullOrEmpty(strMsgArr[i])) continue;

                    string posOrItemStr = strMsgArr[i];
                    Match match = Regex.Match(posOrItemStr, @"\(.*?,.*?\)");

                    if (posOrItemStr[0].Equals('@'))
                    {                        
                        posOrItemStr = posOrItemStr.Remove(0, 1);
                        uint colorId;
                        msgItemInfo mItemInfo = new msgItemInfo();
                        if (posOrItemStr.Length > 1 && uint.TryParse(posOrItemStr[0].ToString(), out colorId))
                        {
                            mItemInfo.msgtype = msgType.custom;                            
                            mItemInfo.colorId = colorId;
                            posOrItemStr = posOrItemStr.Remove(0, 1);
                        }
                        else
                            mItemInfo.msgtype = msgType.non;
                        mItemInfo.msg = posOrItemStr;
                        msgDic.Enqueue(mItemInfo);
                    }
                    else if (match.Success)//大体可以确认是个坐标位置
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.pos;
                        mItemInfo.msg = posOrItemStr;
                        msgDic.Enqueue(mItemInfo);
                    }
                    else if (posOrItemStr.Contains("#"))
                    {
                        setItemStr(posOrItemStr, data, (uint)r2EqpIndex );
                        if ( !isMeSay )
                        {
                            r2EqpIndex++;
                        }
                    }
                    else if (posOrItemStr.Contains("/"))
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.uname;
                        uint cid;
                        string[] strArr = posOrItemStr.Split('/');
                        uint.TryParse(strArr[0], out cid);
                        string uname = strArr[1];
                        mItemInfo.cid = cid;
                        mItemInfo.uname = uname;
                        msgDic.Enqueue(mItemInfo);

                        msgItemInfo mItemInfo1 = new msgItemInfo();
                        mItemInfo1.msgtype = msgType.non;
                        mItemInfo1.msg = "  ";
                        msgDic.Enqueue(mItemInfo1);
                    }
                    else
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.non;
                        mItemInfo.msg = posOrItemStr;
                        msgDic.Enqueue(mItemInfo);
                    }
                }

            }
            else
            {
                msgItemInfo mItemInfo = new msgItemInfo();
                mItemInfo.msgtype = msgType.non;
                mItemInfo.msg = str;
                msgDic.Enqueue(mItemInfo);
            }

        }
        private void setItemStr(string posOrItemStr, chatData data, uint index)
        {
            #region me_say
            if (isMeSay)
            {
                uint id;
                if (posOrItemStr.Contains("#"))
                {
                    bool parseed = uint.TryParse(posOrItemStr.Split('#')[1], out id);
                    if (!parseed)
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.non;
                        mItemInfo.msg = posOrItemStr;
                        msgDic.Enqueue(mItemInfo);
                        return;
                    }
                    string itemName = posOrItemStr.Split('#')[0];
                    itemName = itemName.Contains(ContMgr.getCont("shen")) ? itemName.Remove(0, 2) : itemName;

                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataByName(itemName);

                    ///针对装备名字肯能被屏蔽给屏蔽成 * 导致 itemData 拿不到配置表的数据   下面是根据id查找下  确定玩家 是否是分享装备

                    if (itemData.tpid == 0)
                    {
                        uint tpid = 0;

                        if (a3_EquipModel.getInstance().getEquips().ContainsKey(id))
                        {
                            tpid = a3_EquipModel.getInstance().getEquips()[id].tpid;
                        }
                        else if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                        {
                            tpid = a3_BagModel.getInstance().getItems()[id].tpid;
                        }
                        else if (a3_BagModel.getInstance().getHouseItems().ContainsKey(id))
                        {
                            tpid = a3_BagModel.getInstance().getHouseItems()[id].tpid;
                        }

                        itemData = a3_BagModel.getInstance().getItemDataById(tpid);  
                    }

                    /////

                    if (itemData.tpid == 0)
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.non;
                        mItemInfo.msg = "[" + posOrItemStr.Split('#')[0] + "]";
                        msgDic.Enqueue(mItemInfo);
                        return;
                    }
                    if (itemData.item_type == 1)
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.item;
                        mItemInfo.msg = posOrItemStr.Split('#')[0];
                        mItemInfo.tpid = itemData.tpid;
                        mItemInfo.quality = itemData.quality;
                        msgDic.Enqueue(mItemInfo);
                    }
                    else if (itemData.item_type == 2)
                    {
                        if (a3_EquipModel.getInstance().getEquips().ContainsKey(id))
                        {
                            a3_BagItemData bid = a3_EquipModel.getInstance().getEquips()[id];
                            msgItemInfo mItemInfo = new msgItemInfo();
                            mItemInfo.msgtype = msgType.equip;
                            mItemInfo.msg = posOrItemStr.Split('#')[0];
                            mItemInfo.id = id;
                            mItemInfo.quality = (int)bid.confdata.quality;
                            msgDic.Enqueue(mItemInfo);
                        }
                        else
                        {
                            if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                            {
                                a3_BagItemData bagItemData = a3_BagModel.getInstance().getItems()[id];
                                msgItemInfo mItemInfo = new msgItemInfo();
                                mItemInfo.msgtype = msgType.equip;
                                mItemInfo.msg = posOrItemStr.Split('#')[0];
                                mItemInfo.id = id;
                                mItemInfo.quality = bagItemData.confdata.quality;
                                msgDic.Enqueue(mItemInfo);
                            }
                            else
                            {
                                msgItemInfo mItemInfo = new msgItemInfo();
                                mItemInfo.msgtype = msgType.non;
                                mItemInfo.msg = "[" + posOrItemStr.Split('#')[0] + "]";
                                msgDic.Enqueue(mItemInfo);
                            }
                        }
                    }
                }
            }
            #endregion
            else
            {

                if (posOrItemStr.Contains("#"))
                {
                    if ( dicEquip[ index ].ContainsKey("intensify_lv"))//装备
                    {
                        uint tpid = dicEquip[index]["tpid"]._uint;
                        a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(tpid);
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.equip;
                        mItemInfo.msg = posOrItemStr.Split('#')[0];
                        mItemInfo.id = (uint)index;
                        mItemInfo.quality = itemData.quality;
                        msgDic.Enqueue(mItemInfo);
                    }
                    else
                    {
                        msgItemInfo mItemInfo = new msgItemInfo();
                        mItemInfo.msgtype = msgType.item;
                        mItemInfo.msg = posOrItemStr.Split('#')[0];
                        mItemInfo.tpid = (uint)index;
                        a3_ItemData itemData = a3_BagModel.getInstance().getItemDataByName(mItemInfo.msg);
                        mItemInfo.quality = itemData.quality;
                        msgDic.Enqueue(mItemInfo);
                    }
                    // mItemInfo.tpid = itemData.tpid;


                }

            }

        }
        // private void msgClickHandle()
        public void setLRichTextAction(LRichText lrt_msg, bool singlePanel = false)
        {
            lrt_msg.onClickHandler = (string dataTmp) =>
            {
                if (string.IsNullOrEmpty(dataTmp))
                    return;


                string[] dataTmpArry = dataTmp.Split(':');

                string msgTp = dataTmpArry[0];
                uint id;
                uint tpid;
                a3_ItemData itemData = null;
                switch (msgTp)
                {
                    case "name"://显示加好友信息
                        if (singlePanel)
                        {
                            ArrayList playerData = new ArrayList();
                            playerData.Add(dataTmpArry[1].ToString());
                            playerData.Add(uint.Parse(dataTmpArry[2]));

                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_INTERACTOTHERUI, playerData);
                        }
                        else
                        {
                            ///lrt_msg.gameObject.transform.position;
                            panel4Player.transform.gameObject.SetActive(true);
                            panel4Player.txt_name.text = dataTmpArry[1].ToString();
                            panel4Player.name = dataTmpArry[1].ToString();
                            panel4Player.cid = uint.Parse(dataTmpArry[2]);
                        }
                        break;

                    case "item":

                        uint.TryParse(dataTmpArry[1], out id);
                        itemData = a3_BagModel.getInstance().getItemDataById(id);
                        if (itemData.item_type == 1)
                        {
                            a3_BagItemData bagItemData = new a3_BagItemData();
                            bagItemData.num = 0;
                            bagItemData.confdata = itemData;

                            ArrayList data = new ArrayList();
                            data.Add(bagItemData);
                            data.Add(equip_tip_type.Chat_tip);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                            return;
                        }
                        if (!dicEquip.ContainsKey(id))
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_chatroom_item_invalid"));
                            return;
                        }
                        tpid = dicEquip[id]["tpid"]._uint;
                        a3_ItemData itemData0 = a3_BagModel.getInstance().getItemDataById(tpid);
                        if (itemData0.item_type == 1)
                        {
                            a3_BagItemData bagItemData = new a3_BagItemData();
                            bagItemData.num = 0;
                            bagItemData.confdata = itemData0;

                            ArrayList data = new ArrayList();
                            data.Add(bagItemData);
                            data.Add(equip_tip_type.Chat_tip);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
                        }
                        break;


                    case "equip":
                        uint.TryParse(dataTmpArry[1], out id);
                        if (a3_EquipModel.getInstance().getEquips().ContainsKey(id))
                        {
                            a3_BagItemData oneItem = a3_EquipModel.getInstance().getEquips()[id];
                            ArrayList data1 = new ArrayList();
                            data1.Add(oneItem);
                            data1.Add(equip_tip_type.chatroom_display);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
                            return;
                        }
                        if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                        {
                            a3_BagItemData oneItem = a3_BagModel.getInstance().getItems()[id];
                            ArrayList data1 = new ArrayList();
                            data1.Add(oneItem);
                            data1.Add(equip_tip_type.Comon_tip);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
                            return;
                        }
                        a3_BagItemData bagData;
                        if (!dicEquip.ContainsKey(id))
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_chatroom_item_invalid"));
                            return;
                        }
                        tpid = dicEquip[id]["tpid"]._uint;
                        a3_BagItemData one = new a3_BagItemData();
                        itemData = a3_BagModel.getInstance().getItemDataById(tpid);
                        one.tpid = tpid;
                        one.confdata = itemData;
                        one.equipdata = new a3_EquipData();
                       
                        if (dicEquip[id].ContainsKey("intensify_lv")) one.equipdata.intensify_lv = dicEquip[id]["intensify_lv"]._int;
                        if (dicEquip[id].ContainsKey("colour")) one.equipdata.color = dicEquip[id]["colour"]._uint;
                        if (dicEquip[id].ContainsKey("add_level")) one.equipdata.add_level = dicEquip[id]["add_level"]._int;
                        if (dicEquip[id].ContainsKey("add_exp")) one.equipdata.add_exp = dicEquip[id]["add_exp"]._int;
                        if (dicEquip[id].ContainsKey("stage")) one.equipdata.stage = dicEquip[id]["stage"]._int;
                        if (dicEquip[id].ContainsKey("blessing_lv")) one.equipdata.blessing_lv = dicEquip[id]["blessing_lv"]._int;
                        //if (dicEquip[id].ContainsKey("combpt")) one.equipdata.combpt = dicEquip[id]["combpt"]._int;
                        one.equipdata.subjoin_att = new Dictionary<int, int>();
                        one.equipdata.subjoin_att.Clear();
                        if (dicEquip[id].ContainsKey("subjoin_att"))
                            foreach (Variant item in dicEquip[id]["subjoin_att"]._arr)
                            {
                                int type = item["att_type"];
                                int value = item["att_value"];
                                one.equipdata.subjoin_att[type] = value;
                            }
                        one.equipdata.gem_att = new Dictionary<int, int>();
                        one.equipdata.gem_att.Clear();

                        if (dicEquip[id].ContainsKey("honor_num"))
                        {
                            one.equipdata.honor_num = dicEquip[id]["honor_num"];
                        }
                        if (dicEquip[id].ContainsKey("gem_att"))
                        {
                            Variant att = dicEquip[id]["gem_att"];
                            foreach (Variant item in att._arr)
                            {
                                int type = item["att_type"];
                                int value = item["att_value"];
                                one.equipdata.gem_att[type] = value;
                            }
                        }
                        if (dicEquip[id].ContainsKey("gem_att2"))
                        {
                            Variant att = dicEquip[id]["gem_att2"];
                            one.equipdata.baoshi = new Dictionary<int, int>();
                            for (int i = 0; i < att._arr.Count; i++)
                                one.equipdata.baoshi[i] = att._arr[i]["tpid"];
                        }
                        if (dicEquip[id].ContainsKey("prefix_name"))
                        {
                            SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                            SXML s_xml = itemsXMl.GetNode("item", "id==" + one.tpid);
                            SXML xml = XMLMgr.instance.GetSXML("activate_fun.eqp", "eqp_type==" + s_xml.getInt("equip_type"));
                            SXML _x = xml.GetNode("prefix_fun", "num==" + dicEquip[id]["prefix_name"]);
                            one.equipdata.attribute = _x.getInt("id");
                            one.equipdata.att_type = _x.getInt("funtype");
                            one.equipdata.att_value = dicEquip[id]["att_value"];
                        }
                        //if (dicEquip[id].ContainsKey("att_value"))
                        //    one.equipdata.att_value = dicEquip[id]["att_value"];
                        one.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one));
                        ArrayList dataArr = new ArrayList();
                        dataArr.Add(one);
                        dataArr.Add(equip_tip_type.chatroom_display);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, dataArr);
                        break;
                    case "position":  //文字坐标自动寻路
                        float x;
                        float y;
                        string posStr = dataTmpArry[1].Split('(')[1];
                        string posStrX = posStr.Split(',')[0];
                        string posStrY = posStr.Split(',')[1].Split(')')[0];
                        int mapId = int.Parse(posStr.Split(',')[1].Split(')')[1]);
                        float.TryParse(posStrX, out x);
                        float.TryParse(posStrY, out y);
                        Vector3 pos = new Vector3(x, 0, y);
                        SelfRole.fsm.Stop(); // 自动战斗 下 无法移动到指定目标bug
                    if (MapModel.getInstance().dicMappoint != null && MapModel.getInstance().dicMappoint.ContainsKey(mapId))
                            SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => SelfRole.moveto(mapId, pos, stopDis:2f));
                        else
                            SelfRole.moveto(mapId, pos, stopDis: 2f);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHATROOM);
                    break;
                    case "voice":
                        GameSdkMgr.playVoice(dataTmpArry[1] + ":" + dataTmpArry[2]);
                        break;
                    case "team":
                        uint tid = 0;
                        if(uint.TryParse(dataTmpArry[2],out tid))
                            TeamProxy.getInstance().SendApplyJoinTeam(tid);
                        break;
                }
            };
        }
        Color getWordColor()
        {
            Color co = Color.white;
            switch (chatToType)
            {
                case ChatToType.All:
                    co = Color.yellow;
                    break;

                case ChatToType.Nearby:
                    co = new Color(254, 254, 254);
                    break;
                case ChatToType.World:
                    co = Color.yellow;
                    break;
                case ChatToType.Legion:
                    co = Color.green;
                    break;
                case ChatToType.Team:
                    co = new Color(r: 0, g: 220, b: 255);
                    break;
                case ChatToType.Whisper:
                    co = Color.magenta;
                    break;
                //case ChatToType.SystemMsg:
                //    co = Color.red;
                //    break;
                case ChatToType.PrivateSecretlanguage:
                    co = Color.magenta;
                    break;
                default:
                    break;
            }
            return co;

        }

        void onBtnPlayerWhisperClick(uint cid, string name)
        {
            panel4Player.name = name;
            panel4Player.cid = cid;
            mWhisperName = name;
            iptf_msg.text = name + "/";
            whisper.mainBody.gameObject.SetActive(false);
            chatToType = ChatToType.PrivateSecretlanguage;
            setChatToPanel();
            chatToType = ChatToType.Whisper;
            createWhisperPanel(name);
        }

        private void createWhisperPanel(string name)
        {
            if (!dicWhisper.ContainsKey(name))
            {
                GameObject go = GameObject.Instantiate(prefabWhisperPanel.gameObject) as GameObject;
                go.transform.SetParent(whisper.panelsSecretlanguage, false);
                go.SetActive(true);
                go.transform.localPosition = Vector3.zero;
                dicWhisper.Add(name, go.transform);
            }
            else
            {
                dicWhisper[name].gameObject.SetActive(true);
            }
        }
        void onBtnNewInfo(GameObject go)
        {
            //itemChatMsgObjs[chatToType].parent.parent.fin
        }
        void onAllClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.All;
                setCurrentChatTo();
            }


        }
        void onWorldClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.World;
                setCurrentChatTo();
            }
        }
        bool ipt_msgChanged = false;
        bool isWhisper = false;
        private void hideMinChatTo()
        {
            txtCurrentChat.text = getNameByChatTo(chatToType);

            if (chatToType != ChatToType.Whisper && chatToType != ChatToType.PrivateSecretlanguage)
            {
                if (!ipt_msgChanged)
                {
                    Vector2 sizeDelta = ipt_msg.GetComponent<RectTransform>().sizeDelta;

                    ipt_msgChanged = true;
                }
            }
            else
            {

                if (ipt_msgChanged)
                {
                    Vector2 sizeDelta = ipt_msg.GetComponent<RectTransform>().sizeDelta;

                    ipt_msgChanged = false;

                }


            }
            if (chatToType != ChatToType.Whisper)
            {
                if (PanelFriends.gameObject.activeSelf) PanelFriends.gameObject.SetActive(false);
                if (isWhisper)
                {

                    isWhisper = false;
                }

            }
            else
            {

                if (!isWhisper)
                {
                    if (chatToType == ChatToType.Whisper)
                    {
                        if (!PanelFriends.gameObject.activeSelf) PanelFriends.gameObject.SetActive(true);
                        isWhisper = true;
                    }
                }

            }
        }
        void onNearbyClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.Nearby;
                setCurrentChatTo();
            }
        }
        void onLegionClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.Legion;
                setCurrentChatTo();
            }
        }
        void onTeamClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.Team;
                setCurrentChatTo();
            }
        }
        void onSecretLanguageClick(bool b)
        {
            if (b)
            {
                chatToType = ChatToType.Whisper;
                setCurrentChatTo();
            }
        }
        void onMinWorldClick(bool b)
        {
            chatToType = ChatToType.World;
            hideMinChatTo();
        }
        void onMinSecretLanguageClick(bool b)
        {
            chatToType = ChatToType.Whisper;
            hideMinChatTo();
        }
        void onMinTeamClick(bool b)
        {
            chatToType = ChatToType.Team;
            hideMinChatTo();
        }
        void onMinLegionClick(bool b)
        {
            chatToType = ChatToType.Legion;
            hideMinChatTo();
        }
        void onMinNearbyClick(bool b)
        {
            chatToType = ChatToType.Nearby;
            hideMinChatTo();
        }
        void onPanel4PlayerTouchBgClick(GameObject go)
        {
            if (panel4Player.transform.gameObject.activeSelf)
            {
                panel4Player.transform.gameObject.SetActive(false);
            }
            else
            {
                panel4Player.transform.gameObject.SetActive(true);
            }
        }
        void onBtnSeePlayerInfo(GameObject go)
        {
            if (panel4Player.transform.gameObject.activeSelf)
                panel4Player.transform.gameObject.SetActive(false);
            ArrayList arr = new ArrayList();
            arr.Add(panel4Player.cid);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);
        }
        void onBtnAddFriendClick(GameObject go)
        {
            if (panel4Player.transform.gameObject.activeSelf)
                panel4Player.transform.gameObject.SetActive(false);

            FriendProxy.getInstance().sendAddFriend(panel4Player.cid, panel4Player.name);
        }
        void onPrivateChatClick(GameObject go)
        {
            if (panel4Player.transform.gameObject.activeSelf)
                panel4Player.transform.gameObject.SetActive(false);
            chatToType = ChatToType.Whisper;
            setChatToButtonGroup();
            hideMinChatTo();
            //chatToType = ChatToType.PrivateSecretlanguage;
            setChatToPanel();

            if (!PanelFriends.gameObject.activeSelf)
                PanelFriends.gameObject.SetActive(true);
            chatToType = ChatToType.Whisper;
            createWhisperPanel(panel4Player.name);
            mWhisperName = panel4Player.name;
            iptf_msg.text = mWhisperName + "/";

        }
        void onWhisperbtnClick(GameObject go)
        {
            if (!whisper.mainBody.gameObject.activeSelf)
                whisper.mainBody.gameObject.SetActive(true);
            else
                whisper.mainBody.gameObject.SetActive(false);

        }
        void onPanelQuickTalkClick(GameObject go)
        {
            PanelQuickTalk.root.gameObject.SetActive(false);
            iptf_msg.text += go.transform.FindChild("Text").GetComponent<Text>().text;
        }
        private void setCurrentChatTo()
        {
            setChatToButtonGroup();
            setChatToPanel();
            hideMinChatTo();
        }
        void setChatToButtonGroup()
        {
            foreach (KeyValuePair<ChatToType, Transform> item in chatToButtons)
            {
                if (item.Key == chatToType)
                    item.Value.GetComponent<Toggle>().isOn = true;
            }
            txtCurrentChat.text = getNameByChatTo(chatToType);
        }
        string getNameByChatTo(ChatToType chatTo)
        {
            string str = string.Empty;
            switch (chatTo)
            {
                case ChatToType.Nearby:
                    //str = "附近";
                    str=ContMgr.getCont("a3_chatroom_str0");
                    break;
                case ChatToType.All:
                case ChatToType.World:
                    //str = "世界";
                    str=ContMgr.getCont("a3_chatroom_str1");
                    break;
                case ChatToType.Legion:
                    //str = "军团";
                    str=ContMgr.getCont("a3_chatroom_str2");
                    break;
                case ChatToType.Team:
                   // str = "队伍";
                    str=ContMgr.getCont("a3_chatroom_str3");
                    break;
                case ChatToType.Whisper:
                   // str = "密语";
                    str=ContMgr.getCont("a3_chatroom_str4");
                    break;
                case ChatToType.SystemMsg:
                case ChatToType.LegionSystemMsg:
                    break;
                case ChatToType.PrivateSecretlanguage:
                   // str = "密语";
                    str=ContMgr.getCont("a3_chatroom_str4");
                    break;
                default:
                    break;
            }

            return str;
        }
        void setChatToPanel()
        {
            foreach (KeyValuePair<ChatToType, Transform> item in chatToPanels)
            {
                if (item.Key == chatToType)
                {
                    if (!item.Value.gameObject.activeSelf)
                        item.Value.gameObject.SetActive(true);
                }
                else
                {
                    if (item.Value.gameObject.activeSelf)
                        item.Value.gameObject.SetActive(false);
                }
            }
            foreach (KeyValuePair<string, Transform> item in dicWhisper)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
        #endregion endChatMain
        class ItemChatMsgObj
        {
            public Transform parent;//is msgContainer
            public Transform itemChatMsg;
            public Transform txt_msg;
        }
        string getChatToStr(ChatToType ctp)
        {
            string schatTo = string.Empty;
            if (ctp != ChatToType.SystemMsg && ctp != ChatToType.LegionSystemMsg && chatToType != ctp)
            {
                ctp = chatToType;
            }
            switch (ctp)
            {
                case ChatToType.All:
                    //schatTo = "[世界]";
                    schatTo=ContMgr.getCont("a3_chatroom_str5");
                    break;
                case ChatToType.Nearby:
                   // schatTo = "[附近]";
                    schatTo = ContMgr.getCont("a3_chatroom_str6");
                    break;
                case ChatToType.World:
                   // schatTo = "[世界]";
                    schatTo = ContMgr.getCont("a3_chatroom_str5");
                    break;
                case ChatToType.Legion:
                  //  schatTo = "[军团]";
                    schatTo = ContMgr.getCont("a3_chatroom_str7");
                    break;
                case ChatToType.Team:
                  //  schatTo = "[队伍]";
                    schatTo = ContMgr.getCont("a3_chatroom_str8");
                    break;
                case ChatToType.Whisper:
                  //  schatTo = "[密语]";
                    schatTo = ContMgr.getCont("a3_chatroom_str9");
                    break;
                case ChatToType.SystemMsg:
                case ChatToType.LegionSystemMsg:
                default:
                  //  schatTo = "[系统]";
                    schatTo = ContMgr.getCont("a3_chatroom_str10");
                    break;                    
            }
            return schatTo;
        }

        //class minBagItem
        //{
        //    BaseButton btn_Item;
        //    string name;
        //    uint id;
        //    Sprite sprite;
        //}

        List<LPageItem> listPageItem = new List<LPageItem>();
        List<GameObject> itemsGameObject = new List<GameObject>();
        int itemsCount = 0;
        int onePageItemCount = 6;//一页显示6个物品
        int pageCount = 0;//页数
        public void LoadBagItems()
        {
            itemsGameObject.Clear();
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            itemsCount = items.Count;
            listPageItem.Clear();
            List<a3_BagItemData> a3_BagItemDataList = new List<a3_BagItemData>();
            a3_BagItemDataList.Clear();
            foreach (a3_BagItemData item in items.Values)
                a3_BagItemDataList.Add(item);
            pageCount = Mathf.CeilToInt(itemsCount / (float)onePageItemCount);
            for (int i = 0; i < pageCount; i++)
                listPageItem.Add(CreatePageItem(bagPanelContent));
            string[,] itemNameAndValue = new string[2, itemsCount];
            for (int i = 0; i < pageCount; i++)
            {
                List<GameObject> datas = new List<GameObject>();
                datas.Clear();
                for (int j = i * onePageItemCount; j < i * onePageItemCount + onePageItemCount && j < itemsCount; j++)
                {
                    datas.Add(CreateItemIcon(a3_BagItemDataList[j], false));
                    itemNameAndValue[0, j] = a3_BagItemDataList[j].confdata.item_name;
                    if (a3_BagItemDataList[j].isEquip) itemNameAndValue[1, j] = ContMgr.getCont("a3_chatroom_str11", new List<string>() { a3_BagItemDataList[j].equipdata.combpt.ToString() });
                    else if (a3_BagItemDataList[j].isSummon) { }
                    else itemNameAndValue[1, j] =ContMgr.getCont("a3_chatroom_str12", new List<string>() { (a3_BagItemDataList[j].confdata.use_lv).ToString() });
                }
                //listPageItem[i].Init(datas);
                listPageItem[i].Init(datas, prefab: itemEPrefab, path: "ItemBorder/Item", itemInfo: itemNameAndValue);
            }
            LPageItem.ResetPage();
            LScrollPageMark lScrollPageMark = toggleGroup.transform.GetComponent<LScrollPageMark>() == null ?
                toggleGroup.gameObject.AddComponent<LScrollPageMark>() :
            toggleGroup.transform.GetComponent<LScrollPageMark>();
            lScrollPageMark.scrollPage = bagPanelLScrollPage;
            lScrollPageMark.toggleGroup = lScrollPageMark.gameObject.GetComponent<ToggleGroup>();
            lScrollPageMark.togglePrefab = lScrollPageMark.gameObject.transform.FindChild("Toggle").GetComponent<Toggle>();
            GameObject toggleGroupTmp = GameObject.Instantiate(lScrollPageMark.gameObject) as GameObject;
            toggleGroupTmp.gameObject.transform.localPosition = Vector3.zero;
            toggleGroupTmp.transform.SetParent(bagToggle, false);
            itemsGameObject.Add(toggleGroupTmp.gameObject);
        }
        public void LoadBodyItems()
        {
            itemsGameObject.Clear();
            Dictionary<int, a3_BagItemData> equips = a3_EquipModel.getInstance().getEquipsByType();
            itemsCount = equips.Count;
            listPageItem.Clear();
            List<a3_BagItemData> a3_bodyItemDataList = new List<a3_BagItemData>();
            a3_bodyItemDataList.Clear();
            foreach (a3_BagItemData item in equips.Values)
                a3_bodyItemDataList.Add(item);
            pageCount = Mathf.CeilToInt(itemsCount / (float)onePageItemCount);
            for (int i = 0; i < pageCount; i++)
                listPageItem.Add(CreatePageItem(bodyPanelContent));
            for (int i = 0; i < pageCount; i++)
            {
                List<GameObject> datas = new List<GameObject>();
                string[,] itemNameAndValue = new string[2, itemsCount];
                datas.Clear();
                for (int j = i * onePageItemCount; j < i * onePageItemCount + onePageItemCount && j < itemsCount; j++)
                {
                    datas.Add(CreateItemIcon(a3_bodyItemDataList[j], true));
                    itemNameAndValue[0, j] = a3_bodyItemDataList[j].confdata.item_name;
                    if (a3_bodyItemDataList[j].isEquip) itemNameAndValue[1, j] =ContMgr.getCont("a3_chatroom_str11", new List<string>() { a3_bodyItemDataList[j].equipdata.combpt.ToString() });
                    else if (a3_bodyItemDataList[j].isSummon) { }
                    else itemNameAndValue[1, j] =ContMgr.getCont(("a3_chatroom_str12"), new List<string>(){ (a3_bodyItemDataList[j].confdata.use_lv).ToString() });
                }
                listPageItem[i].Init(datas, prefab: itemEPrefab, path: "ItemBorder/Item", itemInfo: itemNameAndValue);
            }
            LPageItem.ResetPage();
            LScrollPageMark lScrollPageMarkBody = toggleGroup.gameObject.GetComponent<LScrollPageMark>() == null ?
                toggleGroup.gameObject.AddComponent<LScrollPageMark>() :
            toggleGroup.gameObject.GetComponent<LScrollPageMark>();
            lScrollPageMarkBody.scrollPage = bodyPanelLScrollPage;
            lScrollPageMarkBody.toggleGroup = lScrollPageMarkBody.gameObject.GetComponent<ToggleGroup>();
            lScrollPageMarkBody.togglePrefab = lScrollPageMarkBody.gameObject.transform.FindChild("Toggle").GetComponent<Toggle>();
            GameObject toggleGroupBodyTmp = Instantiate(lScrollPageMarkBody.gameObject) as GameObject;
            toggleGroupBodyTmp.gameObject.transform.localPosition = Vector3.zero;
            toggleGroupBodyTmp.transform.SetParent(bodyToggle, false);
            itemsGameObject.Add(toggleGroupBodyTmp.gameObject);
        }
        public void LoadPetItems()
        {
            //TODO:再加个召唤兽的 
            /*
            Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
            int i = 0;
            foreach (a3_BagItemData item in items.Values)
            {
                CreateItemIcon(item, bagPanelContent, false);
                i++;
            }

            Dictionary<uint, a3_BagItemData> houseitems = a3_BagModel.getInstance().getHouseItems();
            int j = 0;
            foreach (a3_BagItemData item in houseitems.Values)
            {
                CreateItemIcon(item, bodyPanelContent, true);
                j++;
            }
           */
        }
        bool isFirstTimeOpen = true;
        public void loadItem()
        {
            LoadBodyItems();
            LoadBagItems();
            LoadPetItems();
            if (isFirstTimeOpen)
            {
                onBtnInBodyClick(null);
                isFirstTimeOpen = false;
            }

        }
        //private Dictionary<int, setFriend> FriendDic = new Dictionary<int, setFriend>();
        LPageItem CreatePageItem(Transform parent)
        {
            LPageItem lpageItemTemp = prefabPageItem.gameObject.GetComponent<LPageItem>() ?? prefabPageItem.gameObject.AddComponent<LPageItem>();
            LPageItem t = Instantiate(lpageItemTemp) as LPageItem;
            t.gameObject.SetActive(true);
            t.transform.SetParent(parent, false);
            t.transform.localScale = Vector3.one;
            t.transform.localPosition = Vector3.zero;
            t.prefabItemParent = t.transform.FindChild("Panel");
            itemsGameObject.Add(t.gameObject);
            return t;
        }
        GameObject CreateItemIcon(a3_BagItemData data, bool ishouse = false)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
            bool _isEquip = data.isEquip;
            new BaseButton(icon.transform).onClick = (go) => 
            {
                if (_isEquip)
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(data);
                    arr.Add(equip_tip_type.chatroom_display);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, arr);
                }
                else
                {
                    ArrayList arr = new ArrayList();
                    arr.Add(data.tpid);
                    arr.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP,arr);
                }
            };
            icon.GetComponent<Image>().raycastTarget = false;
            // icon.transform.SetParent(parent, false);
            icon.transform.localScale = new Vector3(icon.transform.localScale.x * 0.8f, icon.transform.localScale.y * 0.8f, icon.transform.localScale.z * 0.8f);

            //if (ishouse)
            //    houseicon[data.id] = icon;
            //else
            //    itemicon[data.id] = icon;

            if (data.num <= 1)
                icon.transform.FindChild("num").gameObject.SetActive(false);

            GameObject iconClick = Instantiate(transform.FindChild("template/ClickArea").gameObject);
            iconClick.gameObject.SetActive(true);
            iconClick.name = "ClickArea";
            iconClick.transform.SetParent(icon.transform, false);
            BaseButton bs_bt = new BaseButton(iconClick.transform.Find("btn"));
            bs_bt.onClick = delegate (GameObject go) { this.onItemClick(iconClick, data.id, ishouse); };
            itemsGameObject.Add(icon);
            return icon;
        }
        public void onItemClick(GameObject go, uint id, bool ishouse)
        {
            if (currentDisplayEquipCount == 3)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_chatroom_str13"));
                return;
            }
            if (ishouse)//装备
            {
                a3_BagItemData equip_data;
                string equipName = null;
                if (a3_EquipModel.getInstance().getEquips().ContainsKey(id))
                {
                    equip_data = a3_EquipModel.getInstance().getEquips()[id];
                    equipName = a3_BagModel.getInstance().getEquipName(equip_data);
                }
                else if (a3_BagModel.getInstance().getItems().ContainsKey(id))
                {
                    equip_data = a3_BagModel.getInstance().getItems()[id];
                    equipName = a3_BagModel.getInstance().getEquipName(equip_data);
                }
                else if (a3_BagModel.getInstance().getHouseItems().ContainsKey (id))
                {
                    equip_data = a3_BagModel.getInstance().getHouseItems()[id];
                    equipName = a3_BagModel.getInstance().getEquipName(equip_data);
                }
                if (equipName == null) return;
                //string strGod = a3_BagModel.getInstance().addGodStr(equip_data.confdata.equip_level);
                iptf_msg.text += "[" + equipName + "#" + id + "]";
            }
            else//道具
            {
                uint itemDataId = a3_BagModel.getInstance().getItems()[id].tpid;
                uint tempId = a3_BagModel.getInstance().getItems()[id].id;
                iptf_msg.text += "[" + a3_BagModel.getInstance().getItemDataById(itemDataId).item_name + "#" + id + "]";
            }
            //minBag.gameObject.SetActive(false);
        }
        public class msgItemInfo
        {
            public msgType msgtype;
            public string msg;
            public uint tpid;
            public uint id;
            public int quality;
            //
            public uint intensify_lv;
            public uint add_level;
            public uint add_exp;
            public uint stage;
            public uint blessing_lv;
            public uint combpt;
            public List<uint> subjoin_att;
            public List<uint> gem_att;
            //
            public uint cid;
            public string uname;
            //
            public uint colorId;
            //
            public Color color;
            //
            public uint xtp;
            public Variant data;
        }
        float delayExcuteShowTime = 0.3f;
        [SerializeField]
        float BorderMaxValue = 600f;
        void Update()
        {

            if (OutTimer > 0)
            {
                OutTimer -= Time.deltaTime;
            }
            if (delayExcuteShowTime > 0)
            {
                delayExcuteShowTime -= Time.deltaTime;
                return;
            }
            for (int i = 0; i < AllItemMsg.Count; i++)
            {
                if (AllItemMsg[i].position.y < dummyTop + BorderMaxValue && AllItemMsg[i].position.y > dummyEnd - BorderMaxValue)
                {
                    if (!AllItemMsg[i].gameObject.activeSelf)
                    {
                        AllItemMsg[i].gameObject.SetActive(true);
                    }

                }
                else
                {
                    if (AllItemMsg[i].gameObject.activeSelf)
                    {
                        AllItemMsg[i].gameObject.SetActive(false);
                    }
                }
            }
            delayExcuteShowTime = 0.3f;
        }
    }
    enum ChatToType
    {
        All,//综合
        Nearby,//附近
        World,
        Legion,//军团
        Team,//队伍
        Whisper,//密语
        SystemMsg,//系统消息
        PrivateSecretlanguage,//单人私聊
        LegionSystemMsg,//军团系统消息

    }
    enum BgType
    {
        Body,
        Bag
    }
    enum msgType//聊天内容对应的三种形式
    {
        non,//普通类型,玩家自己输入的
        item,//物品,插入的物品
        equip,
        pos, //位置
        uname,//玩家姓名
        chatTo,//显示位置
        vip,
        voice,
        custom,
        customx,//不采用RichText的标签写法,而是由自己定义颜色
    }
    class msg4roll
    {
        public msgType msgT = msgType.non;
        public string msgStr = string.Empty;
        public Color color = Color.white;
        public string data = string.Empty;
    }
    class Panel4Player
    {
        public Transform transform;//panel4Player面板
        public Text txt_name;//显示的姓名
        public BaseButton btn_see;//查看
        public BaseButton btn_addFriend;//加为好友
        public BaseButton btn_pinvite;//组队邀请
        public BaseButton btn_privateChat;//私聊
        public uint cid;
        public string name;

    }
    struct whisper
    {
        public BaseButton btn_Player;
        public static BaseButton btn_Whisper;//密语按钮
        public static Transform mainBody;
        public uint cid;
        public string name;
        public static Transform panelsSecretlanguage;//单独密语面板父节点


    }
    class PanelQuickTalk
    {
        public static Transform root;
        public static BaseButton btn01;
        public static BaseButton btn02;
        public static BaseButton btn03;
    }
    class chatData
    {
        public uint tp;
        public uint cid;
        public string name;
        public string msg;
        public int vip;
        public bool isButton;
        public List<Variant> items;
        public uint mapId;
        public int x;
        public int y;
        public string url;
        public int carr;
    }
}
