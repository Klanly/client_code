using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cross;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections;
namespace MuGame
{
    class a3_fb_team : Window
    {
        public FB_Invite invitePanel = new FB_Invite();
        public static a3_fb_team Instance;

        #region button
        private BaseButton btnStart;
        private BaseButton btnCreate;
        private BaseButton btnInvite;
        private BaseButton btnClose;
        private BaseButton btnOpLookup;
        private BaseButton btnOpKick;
        private BaseButton btnOpClose;
        private BaseButton btnRefresh;
        private BaseButton btnQuickJoin;
        private BaseButton btncall;
        #endregion
        #region toggle
        private Toggle togDiffEasy;
        private Toggle togDiffNormal;
        private Toggle togDiffHard;
        private Toggle togDiffCrazy;
        #endregion
        #region uiData
        private uint levelId;
        #endregion
        #region Text
        private Text txtLevelName;
        #endregion
        private GameObject prefabTeamInfo;
        private Transform tfOpRoot;
        private Transform tfTeamPanel;
        private Transform contTeamPanel;
        private ScrollRect srctTeamPanel;
        private VerticalLayoutGroup vlayoutTeamPanel;
        private RectTransform rectContent;
        private LayoutElement leContent;
        private Toggle toggleApply;
        private Text leftTimes;
        private Text needZDL;
        private uint curSelectedCid = 0;
        private uint curSelectedDiff = 1;
        private float expandHeight = 0f;
        private float baseHeight = 60f;
        public override void init()
        {
            inText();
            a3_counterpartModel.getInstance();
            //保存一些预制件
            #region init prefab 
            #endregion
            Instance = this;
            togDiffEasy = getComponentByPath<Toggle>("select_diff/diff_easy");
            togDiffNormal = getComponentByPath<Toggle>("select_diff/diff_normal");
            togDiffHard = getComponentByPath<Toggle>("select_diff/diff_hard");
            togDiffCrazy = getComponentByPath<Toggle>("select_diff/diff_crazy");
            togDiffEasy.onValueChanged.AddListener((bool selected) =>
            {
                if (selected)
                {
                    curSelectedDiff = 1;
                    TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
                }
            });
            togDiffNormal.onValueChanged.AddListener((bool selected) =>
            {
                if (selected)
                {
                    curSelectedDiff = 2;
                    TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
                }
            });
            togDiffHard.onValueChanged.AddListener((bool selected) =>
            {
                if (selected)
                {
                    curSelectedDiff = 3;
                    TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
                }
            });
            togDiffCrazy.onValueChanged.AddListener((bool selected) =>
            {
                if (selected)
                {
                    curSelectedDiff = 4;
                    TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
                }
            });

            toggleApply = getGameObjectByPath("apply").GetComponent<Toggle>();
            toggleApply.onValueChanged.AddListener((b) => { TeamProxy.getInstance().SendEditorInfoDirJoin(b); });
            btnStart = new BaseButton(transform.Find("btn_start"));
            //btnCreate = new BaseButton(transform.Find("btn_create"));
            //btnQuickJoin = new BaseButton(transform.Find("btn_quickJoin"));
            btnCreate = new BaseButton(transform.Find("btn_quickJoin"));
            btnQuickJoin = new BaseButton(transform.Find("btn_create"));
            btncall = new BaseButton(getTransformByPath("btn_call"));
            btnInvite = new BaseButton(transform.Find("btn_invite"));
            tfOpRoot = transform.Find("op");
            btnOpClose = new BaseButton(transform.Find("op/close"));
            btnOpLookup = new BaseButton(transform.Find("op/lookup"));
            btnOpKick = new BaseButton(transform.Find("op/kick"));
            btnOpClose.onClick = (g) => { tfOpRoot.gameObject.SetActive(false); };
            //btnOpClose = new MuGame.BaseButton(transform.Find("op/close"));
            //btnOpClose.onClick = (go) => { tfOpRoot.gameObject.SetActive(false); };
            btnClose = new BaseButton(transform.Find("btn_close"));
            btnRefresh = new BaseButton(transform.Find("btn_refresh"));            
            btnStart.onClick = OnClickStartFB;
            btnCreate.onClick = OnClickCreateTeam;
            btnQuickJoin.onClick = OnClickQuickJoin;
            btnInvite.onClick = OnClickInvite;
            btnOpLookup.onClick = OnClickOpLookUp;
            btnOpKick.onClick = OnClickOpKick;
            btncall.onClick = OnClickMsg;
            btnRefresh.onClick = (go) => TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
            btnClose.onClick = (go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_FB_TEAM);

            txtLevelName = getComponentByPath<Text>("fb_name");
            prefabTeamInfo = getGameObjectByPath("template/team_pre");
            tfTeamPanel = getTransformByPath("team_panel");
            contTeamPanel = tfTeamPanel.Find("scroll");
            rectContent = contTeamPanel.Find("content").GetComponent<RectTransform>();
            leContent = contTeamPanel.Find("content").GetComponent<LayoutElement>();
            leftTimes = getComponentByPath<Text>("text_leftTime/times");
            needZDL = getComponentByPath<Text>("text_needZDL/zdl");
            srctTeamPanel = tfTeamPanel.GetComponent<ScrollRect>();
            vlayoutTeamPanel = contTeamPanel.GetComponent<VerticalLayoutGroup>();
            expandHeight = Mathf.Abs(vlayoutTeamPanel.padding.top);
            baseHeight = prefabTeamInfo.GetComponent<LayoutElement>().minHeight;
            invitePanel.Init();
        }


        void inText()
        {
            this.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_1");//在队长点击进入副本后，需要全员确认后，副本才会开始。
            this.transform.FindChild("fb_name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_2");//副本名称
            this.transform.FindChild("text_needZDL").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_3");//推荐战力:
            this.transform.FindChild("text_leftTime").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_4");//剩余次数:
            this.transform.FindChild("btn_start/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_5");//进入副本
            //this.transform.FindChild("btn_create/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_6");//创建队伍
            this.transform.FindChild("btn_invite/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_7");//邀请
            //this.transform.FindChild("btn_quickJoin/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_8");//快速加入
            this.transform.FindChild("btn_quickJoin/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_6");//创建队伍
            this.transform.FindChild("btn_create/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_25");//快速加入
            this.transform.FindChild("btn_refresh/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_9");//刷新列表
            this.transform.FindChild("apply/desc").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_10");//允许其他玩家直接进入队伍
            this.transform.FindChild("invite_panel/btn_refresh/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_11");//刷新列表
            this.transform.FindChild("invite_panel/mainBody/left/toggleGroup/togFriend/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_12");//好友
            this.transform.FindChild("invite_panel/mainBody/left/toggleGroup/togNearby/Background/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_13");//附近

            this.transform.FindChild("invite_panel/mainBody/myFriendsPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_14");//昵称
            this.transform.FindChild("invite_panel/mainBody/myFriendsPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_15");//等级
            this.transform.FindChild("invite_panel/mainBody/myFriendsPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_16");//战力
            this.transform.FindChild("invite_panel/mainBody/myFriendsPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_17");//位置
            
            this.transform.FindChild("invite_panel/mainBody/neighborPanel/right/main/title/texts/txt_niceName").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_14");//昵称
            this.transform.FindChild("invite_panel/mainBody/neighborPanel/right/main/title/texts/txt_level").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_15");//等级
            this.transform.FindChild("invite_panel/mainBody/neighborPanel/right/main/title/texts/txt_combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_16");//战力
            this.transform.FindChild("invite_panel/mainBody/neighborPanel/right/main/title/texts/txt_pos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_17");//位置


            this.transform.FindChild("invite_panel/itemPrefabs/itemFriend/btn_invite/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_18");//邀请
            this.transform.FindChild("invite_panel/itemPrefabs/itemNearby/btn_invite/actionPanelPos").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_18");//邀请
            this.transform.FindChild("invite_panel/itemPrefabs/actionPanel/buttons/btnChat/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_19");//私聊
            this.transform.FindChild("invite_panel/itemPrefabs/actionPanel/buttons/btnWatch/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_20");//查看
            this.transform.FindChild("invite_panel/itemPrefabs/actionPanel/buttons/btnTeam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_21");//邀请组队
            this.transform.FindChild("invite_panel/itemPrefabs/actionPanel/buttons/btnDelete/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_22");//删除好友
            this.transform.FindChild("invite_panel/itemPrefabs/actionPanel/buttons/btnBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_23");//黑名单


            this.transform.FindChild("invite_panel/itemPrefabs/actionNearybyPanel/buttons/btnChat/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_19");//私聊
            this.transform.FindChild("invite_panel/itemPrefabs/actionNearybyPanel/buttons/btnWatch/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_20");//查看
            this.transform.FindChild("invite_panel/itemPrefabs/actionNearybyPanel/buttons/btnTeam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_21");//邀请组队
            this.transform.FindChild("invite_panel/itemPrefabs/actionNearybyPanel/buttons/btnAdd/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_24");//添加好友
            this.transform.FindChild("invite_panel/itemPrefabs/actionNearybyPanel/buttons/btnBlackList/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_team_23");//黑名单
        }
        ArrayList bufferData = null;
        public override void onShowed()
        {
            if (uiData != null && uiData.Count == 2 && (bool)uiData[1])
                InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_FB_TEAM);
            Instance = this;
            if (bufferData != null && (uiData == null || uiData.Count == 0))
                uiData = bufferData;
            if (uiData.Count > 0)
            {
                if (uiData[0] == null)
                    uiData = bufferData;
                levelId = (uint)uiData[0];
                Variant _levelInfo = SvrLevelConfig.instacne.get_level_data(levelId);
                if (_levelInfo != null && _levelInfo.ContainsKey("name"))
                    txtLevelName.text = _levelInfo["name"];
                Variant data = SvrLevelConfig.instacne.get_level_data(levelId);


                Variant datasss = SvrLevelConfig.instacne.get_level_data(levelId);
                int thisday_count_g = data["daily_cnt"];

                int vip_buy_g = 0;
                if (MapModel.getInstance().dFbDta.ContainsKey((int)levelId))
                {
                    vip_buy_g = MapModel.getInstance().dFbDta[(int)levelId].vip_buycount;
                }
                //总次数包括购买的
                int all_count_g = thisday_count_g + vip_buy_g;
                //进入次数：
                int goin_count_g = 0;
                if (MapModel.getInstance().dFbDta.ContainsKey((int)levelId))
                {
                    goin_count_g = Mathf.Min(MapModel.getInstance().dFbDta[(int)levelId].cycleCount, all_count_g/*+ A3_VipModel.getInstance().goldFb_count*/);
                }
               // int max_times = data["daily_cnt"];
               // int use_times = 0;
                if (MapModel.getInstance().dFbDta.ContainsKey((int)levelId))
                {
                    goin_count_g = Mathf.Min(MapModel.getInstance().dFbDta[(int)levelId].cycleCount, all_count_g);
                    leftTimes.text = (all_count_g - goin_count_g).ToString();
                }
                else leftTimes.text = all_count_g.ToString(); /*"0";*/
            }
            else
                return;

            #region add listeners to events
            //TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, OnTeamStatuChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, OnCreateTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CHANGECAPTAIN, OnTeamStatuChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_KICKOUT, OnMemberInfoChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, OnMemberInfoChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, OnMemberInfoChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, OnMemberInfoChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CURPAGE_TEAM, OnRefreshTeamInfo);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_DISSOLVETEAM, OnTeamDissolve);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAM_READY, OnReady);
            #endregion
            #region send msg to server
            TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
            #endregion
            if (PlayerModel.getInstance().IsInATeam)
            {
                btnCreate.gameObject.SetActive(false);
                btnQuickJoin.gameObject.SetActive(false);
                if (PlayerModel.getInstance().IsCaptain)
                {
                    btnStart.gameObject.SetActive(true);
                    btnStart.interactable = true;
                    btnInvite.gameObject.SetActive(true);
                    btncall.gameObject.SetActive(true);
                }
                else
                {
                    btnStart.interactable = false;
                    btnInvite.gameObject.SetActive(false);
                    btncall.gameObject.SetActive(false);
                }
            }
            else
            {
                btncall.gameObject.SetActive(false);
                btnCreate.gameObject.SetActive(true);
                btnQuickJoin.gameObject.SetActive(true);
            }
            Variant levelInfo = SvrLevelConfig.instacne.get_level_data(levelId);
            if (levelInfo != null && levelInfo.ContainsKey("zdl"))
            {
                needZDL.transform.parent.gameObject.SetActive(true);
                int zdl = levelInfo["zdl"];
                if(zdl != 0) needZDL.text = zdl.ToString();
                else needZDL.transform.parent.gameObject.SetActive(false);
            }
            else needZDL.transform.parent.gameObject.SetActive(false);
        }
        void OnTeamDissolve(GameEvent e)
        {
            TeamProxy.getInstance().SendGetPageTeam(levelId, 0, 20);
        }
        public override void onClosed()
        {
            #region remove listeners from events
            //TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CREATETEAM, OnTeamStatuChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CREATETEAM, OnCreateTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CHANGECAPTAIN, OnTeamStatuChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_KICKOUT, OnMemberInfoChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, OnMemberInfoChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_LEAVETEAM, OnMemberInfoChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CURPAGE_TEAM, OnRefreshTeamInfo);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_AFFIRMINVITE, OnTeamStatuChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_DISSOLVETEAM, OnTeamDissolve);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_TEAM_READY, OnReady);
            #endregion
            levelId = 0;
            tfOpRoot.SetParent(transform, false);
            tfOpRoot.gameObject.SetActive(false);
            if (uiData != null && uiData.Count == 1)
                bufferData = uiData;
            for (int i = rectContent.transform.childCount; i > 0; i--)
                Destroy(rectContent.transform.GetChild(i - 1).gameObject);
            Instance = null;
        }
        public void OnReady(GameEvent e)
        {
            if (e.data.ContainsKey("ready"))
                if (e.data.ContainsKey("cid") && !e.data["ready"])
                {
                    uint cid = e.data["cid"];
                    if (TeamProxy.getInstance().MyTeamData != null && cid != TeamProxy.getInstance().MyTeamData.leaderCid)
                    {
                        int index;
                        if (TeamProxy.getInstance().MyTeamData.GetIndexOfMember(cid, out index))
                            if (goMyTeamInfo != null)
                                goMyTeamInfo.transform.Find("member/carr").GetChild(index).Find("notready").gameObject.SetActive(true);
                    }
                }
        }
        #region button recall
        private void OnClickStartFB(GameObject go)
        {
            if (!isMyTeamReady)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_fb_team_no_chance"));
                return;
            }
            if (levelId != 0)
                GoFb();
        }
        private void OnClickCreateTeam(GameObject go)
        {
            TeamProxy.getInstance().SendCreateTeam((int)levelId);
        }
        private void OnClickQuickJoin(GameObject go)
        {
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CURPAGE_TEAM, OnFastJoin);
            TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
        }
        private void OnFastJoin(GameEvent e)
        {
            uint tid = 0;
            if (e.data != null && e.data.ContainsKey("info"))
            {
                List<Variant> teamInfoList = e.data["info"]._arr;
                for (int i = 0; i < teamInfoList.Count; i++)
                    if (!teamInfoList[i].ContainsKey("dir_join") || !teamInfoList[i]["dir_join"]) continue;
                    else { tid = teamInfoList[i]["tid"]; break; }
                if (tid != 0) TeamProxy.getInstance().SendApplyJoinTeam(tid);
                else               
                {
                    TeamProxy.getInstance().SendCreateTeam((int)levelId);
                    //flytxt.instance.fly(ContMgr.getCont("a3_fb_team_noteam"));
                }
            }
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CURPAGE_TEAM, OnFastJoin);
        }
        private void OnClickMsg(GameObject go)
        {
            int v=0;
            switch (levelId)
            {
                case 0:
                    v = 0;//自定义
                    break;
                case 1:
                    v = 1;//挂机
                    break;
                case 2:
                    v = 6;
                    break;
                case 105:
                    v = 5;
                    break;
                case 108:
                    v = 2;
                    break;
                case 109:
                    v = 3;
                    break;
                case 110:
                    v = 4;
                    break;
            }
            if (!PlayerModel.getInstance().IsCaptain) { flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_not_captain")); return; }
            if (PlayerModel.getInstance().inFb) { flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_in_fb")); return; }
            a3_chatroom._instance.SendMsg(string.Format("{0}:{1}:{2}", TeamProxy.getInstance().MyTeamData.teamId, v, TeamProxy.getInstance().mapItemTeamData == null ? 0 : TeamProxy.getInstance().mapItemTeamData.ldiff), chatType: ChatToType.World, xtp: 1);
        }
        private void OnClickInvite(GameObject go)
        {
            invitePanel.root.gameObject.SetActive(true);
            invitePanel.OnShowed();
        }
        private void OnClickOpLookUp(GameObject go)
        {
            if (curSelectedCid != 0)
            {
                ArrayList arr = new ArrayList();
                arr.Add(curSelectedCid);
                arr.Add(InterfaceMgr.A3_FB_TEAM);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_FB_TEAM);
            }
        }
        private void OnClickOpKick(GameObject go)
        {
            if (curSelectedCid != 0 && TeamProxy.getInstance().MyTeamData.IsInMyTeam(curSelectedCid))
            {
                TeamProxy.getInstance().SendKickOut(curSelectedCid);
                TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
            }
        }
        #endregion
        #region Team Op Recall
        //创建/离开队伍/成为队长
        private void OnTeamStatuChange(GameEvent e = null)
        {
            if (PlayerModel.getInstance().IsInATeam)
            {
                btnCreate.gameObject.SetActive(false);
                btnQuickJoin.gameObject.SetActive(false);
                if (PlayerModel.getInstance().IsCaptain)
                {
                    btnStart.gameObject.SetActive(true);
                    btnStart.interactable = true;
                    btnInvite.gameObject.SetActive(true);
                    btncall.gameObject.SetActive(true);
                    if (TeamProxy.getInstance().MyTeamData != null)
                    {
                        toggleApply.gameObject.SetActive(true);
                        toggleApply.isOn = TeamProxy.getInstance().MyTeamData.dirJoin;
                    }
                }
                else
                {
                    btnStart.interactable = false;
                    btnInvite.gameObject.SetActive(false);
                    btncall.gameObject.SetActive(false);
                    toggleApply.gameObject.SetActive(false);
                }
            }
            else
            {
                btnCreate.gameObject.SetActive(true);
                btnQuickJoin.gameObject.SetActive(true);
                toggleApply.gameObject.SetActive(false);
                btncall.gameObject.SetActive(false);
            }         
        }
        private void OnCreateTeam(GameEvent e)
        {
            TeamProxy.getInstance().SendGetPageTeam(levelId, 0, 20);
            OnTeamStatuChange();
        }
        //队员离开或加入
        private void OnMemberInfoChange(GameEvent e)
        {
            TeamProxy.getInstance().SendGetPageTeam(levelId, TeamProxy.BEGININDEX, TeamProxy.ENDINDEX);
        }
        //获取队伍信息
        Dictionary<uint, TeamInfoObject> dicList = new Dictionary<uint, TeamInfoObject>();
        GameObject goMyTeamInfo = null;
        bool isMyTeamReady;
        private void OnRefreshTeamInfo(GameEvent e)
        {
            Debug.Log(e.data.dump()); isMyTeamReady = true;
            tfOpRoot.SetParent(transform, false);
            tfOpRoot.gameObject.SetActive(false);
            dicList.Clear();
            for (int i = rectContent.transform.childCount; i > 0; i--)
                Destroy(rectContent.transform.GetChild(i - 1).gameObject);
            if (e.data != null)
            {
                List<Variant> info = e.data["info"]._arr;                
                for (int i = 0; i < info.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(prefabTeamInfo);
                    //if (i == 0)
                    //{
                    string carr_captain = info[i]["lcarr"],
                        name_captain = info[i]["lname"],
                        level_captain = info[i]["llevel"],
                        zhuan_captain = info[i]["lzhuan"];
                    go.transform.Find("captain_head_icon/" + carr_captain).gameObject.SetActive(true);
                    go.transform.Find("captain_info/carr/" + carr_captain).gameObject.SetActive(true);
                    go.transform.Find("captain_info/name").GetComponent<Text>().text = name_captain;
                    go.transform.Find("captain_info/level").GetComponent<Text>().text = zhuan_captain + ContMgr.getCont("zhuan") + level_captain + ContMgr.getCont("ji");
                    //}
                    uint teamId = info[i]["tid"];
                    go.name = teamId.ToString();
                   
                    if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.teamId == teamId)
                    {
                        goMyTeamInfo = go;
                        if (PlayerModel.getInstance().IsCaptain)
                        {
                            Transform btn_dissolve = go.transform.Find("dissolve");
                            new BaseButton(btn_dissolve).onClick = (btn) => { TeamProxy.getInstance().SendDissolve(PlayerModel.getInstance().teamid); };
                            btn_dissolve.gameObject.SetActive(true);
                            toggleApply.gameObject.SetActive(true);
                        }
                        else
                        {
                            Transform btn_dissolve = go.transform.Find("leave");
                            new BaseButton(btn_dissolve).onClick = (btn) => { TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().teamid); };
                            btn_dissolve.gameObject.SetActive(true);
                            toggleApply.gameObject.SetActive(false);
                        }
                    }
                    int cur_index = i;
                    uint cur_cid = info[cur_index]["members"][0]["cid"];
                    Transform cptnHead = go.transform.Find("captain_head_icon");
                    new BaseButton(cptnHead).onClick = (g) =>
                    {
                        if (info[cur_index].ContainsKey("members"))
                            curSelectedCid = cur_cid;
                        ShowOp(g.transform);
                    };
                    if (info[i].ContainsKey("members"))
                    {
                        List<Variant> listMember = info[i]["members"]._arr;
                        for (int j = 1; j < listMember.Count; j++)
                        {
                            //OtherPlayerMgr._inst.
                            string memLv = listMember[j]["lvl"], memZhuan = listMember[j]["zhuan"];
                            bool canJoin = listMember[j]["can_join"];
                            uint cid = listMember[j]["cid"];
                            string carr = listMember[j]["carr"];
                            bool isOffline = false;
                            if (listMember[j].ContainsKey("online")) isOffline = !listMember[j]["online"];
                            go.transform.Find("member/carr").GetChild(j).Find("offline").gameObject.SetActive(isOffline);
                            go.transform.Find("member/carr").GetChild(j).Find("lvinfo").gameObject.SetActive(true);
                            go.transform.Find("member/carr").GetChild(j).Find("lvinfo").GetComponent<Text>().text = memZhuan + ContMgr.getCont("zhuan") + memLv + ContMgr.getCont("ji");
                            if (!canJoin)
                            {
                                if (TeamProxy.getInstance().MyTeamData?.IsInMyTeam(cid) ?? false)
                                    isMyTeamReady = false;
                                go.transform.Find("member/carr").GetChild(j).Find("notready").gameObject.SetActive(true);
                            }
                            Transform tfHead = go.transform.Find("member/carr").GetChild(j).Find(carr);
                            new BaseButton(tfHead).onClick = (g) =>
                            {
                                curSelectedCid = cid;
                                ShowOp(g.transform);
                            };
                            tfHead.gameObject.SetActive(true);
                        }
                    }
                    go.transform.SetParent(rectContent.transform, false);
                    dicList[teamId] = new TeamInfoObject(teamId, go, go.transform.Find("btn_join/apply"));
                }
                if (goMyTeamInfo != null)
                {
                    if (TeamProxy.getInstance().MyTeamData != null && dicList.ContainsKey(TeamProxy.getInstance().MyTeamData.teamId))
                    {
                        dicList[TeamProxy.getInstance().MyTeamData.teamId].goMain.transform.SetAsFirstSibling();
                        dicList[TeamProxy.getInstance().MyTeamData.teamId].goBtnJoin.gameObject.SetActive(false);
                    }
                    else goMyTeamInfo = null;
                }
                srctTeamPanel.vertical = info.Count > 3;
                leContent.minHeight = baseHeight * rectContent.transform.childCount + expandHeight;
            }
            OnTeamStatuChange();
        }

        private void GoFb()
        {
            if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count == 1)
            {
                Variant sendData = new Variant();
                sendData["npcid"] = 0;
                sendData["ltpid"] = levelId;
                sendData["diff_lvl"] = curSelectedDiff;
                a3_counterpart.lvl = sendData["diff_lvl"];
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }
            else
                TeamProxy.getInstance().SendReady(true, levelId, curSelectedDiff);
        }

        private void ShowOp(Transform tf)
        {
            tfOpRoot.SetParent(tf.parent.Find("ready"), false);
            tfOpRoot.gameObject.SetActive(true);
            if (!PlayerModel.getInstance().IsCaptain)
                btnOpKick.gameObject.SetActive(false);
            else if (TeamProxy.getInstance().MyTeamData.IsInMyTeam(curSelectedCid) && curSelectedCid != PlayerModel.getInstance().cid)
                btnOpKick.gameObject.SetActive(true);
            else
                btnOpKick.gameObject.SetActive(false);
        }
        #endregion
    }
    class TeamInfoObject
    {
        uint tid;
        public GameObject goMain;
        public Transform goBtnJoin;
        public TeamInfoObject() { }
        public TeamInfoObject(uint teamId, GameObject main, Transform goBtn)
        {
            tid = teamId;
            goMain = main;
            goBtnJoin = goBtn;
            goBtn.transform.parent.gameObject.SetActive(!PlayerModel.getInstance().IsInATeam);
            new BaseButton(goBtnJoin).onClick = (go) =>
            {
                TeamProxy.getInstance().SendApplyJoinTeam(tid);
                main.transform.Find("btn_join/applyed").gameObject.SetActive(true);
            };
        }
    }
    class FB_Invite
    {
        public Transform root;
        #region data
        #endregion

        #region prefab
        GameObject prefabFriendInfo;
        GameObject prefabNearbyInfo;

        Transform tfFriendContainer;
        Transform tfNearbyContainer;

        GameObject goFriendPanel;
        GameObject goNearbyPanel;

        BaseButton btn_refresh;
        BaseButton btn_close;

        Toggle toggleFriend;
        Toggle toggleNearby;
        #endregion

        #region tfroot
        #endregion

        public void Init()
        {
            //保存一些游戏对象
            root = a3_fb_team.Instance.transform.Find("invite_panel");
            prefabFriendInfo = root.transform.Find("itemPrefabs/itemFriend").gameObject;
            prefabNearbyInfo = root.transform.Find("itemPrefabs/itemNearby").gameObject;

            goFriendPanel = root.transform.Find("mainBody/myFriendsPanel").gameObject;
            goNearbyPanel = root.transform.Find("mainBody/neighborPanel").gameObject;

            tfFriendContainer = root.transform.Find("mainBody/myFriendsPanel/right/main/body/scroll/contains");
            tfNearbyContainer = root.transform.Find("mainBody/neighborPanel/right/main/body/scroll/contains");

            toggleFriend = root.transform.Find("mainBody/left/toggleGroup/togFriend").GetComponent<Toggle>();
            toggleNearby = root.transform.Find("mainBody/left/toggleGroup/togNearby").GetComponent<Toggle>();

            btn_refresh = new BaseButton(root.transform.Find("btn_refresh"));
            btn_refresh.onClick = (go) =>
            {
                if (toggleFriend.isOn) FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);
                else RefreshNearby();
            };

            btn_close = new BaseButton(root.transform.Find("btn_close"));
            btn_close.onClick = (go) => { OnClosed(); };

            toggleFriend.onValueChanged.AddListener((b) =>
            {
                if (b)
                {
                    goFriendPanel.SetActive(true);
                    goNearbyPanel.SetActive(false);
                }
            });
            toggleNearby.onValueChanged.AddListener((b) =>
            {
                if (b)
                {
                    goFriendPanel.SetActive(false);
                    goNearbyPanel.SetActive(true);
                }
            });
        }

        public void OnShowed()
        {
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_FRIENDLIST, OnFriendRefresh);
            RefreshNearby();
            FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);
        }

        public void OnClosed()
        {
            root.gameObject.SetActive(false);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_FRIENDLIST, OnFriendRefresh);
            for (int i = tfFriendContainer.childCount; i > 0; i--)
                GameObject.Destroy(tfFriendContainer.GetChild(i - 1).gameObject);
            for (int i = tfFriendContainer.childCount; i > 0; i--)
                GameObject.Destroy(tfFriendContainer.GetChild(i - 1).gameObject);
        }

        void OnFriendRefresh(GameEvent e)
        {
            for (int i = tfFriendContainer.childCount; i > 0; i--)
                GameObject.Destroy(tfFriendContainer.GetChild(i - 1).gameObject);
            Variant data = e.data;
            if (data.ContainsKey("buddy"))
            {
                List<Variant> listFriend = data["buddy"]._arr;
                for (int i = 0; i < listFriend.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(prefabFriendInfo);
                    string name = listFriend[i]["name"];
                    string level = listFriend[i]["zhuan"] + ContMgr.getCont("zhuan") + listFriend[i]["lvl"] + ContMgr.getCont("ji");
                    string combat = listFriend[i]["combpt"];
                    uint cid = listFriend[i]["cid"]._uint;
                    bool online = listFriend[i]["online"];
                    go.transform.Find("Toggle/containts/txtName").GetComponent<Text>().text = name;
                    go.transform.Find("Toggle/containts/txtLevel").GetComponent<Text>().text = level;
                    go.transform.Find("Toggle/containts/txtcombat").GetComponent<Text>().text = combat;
                    if (online)
                    {
                        uint mapId = (uint)FriendProxy.getInstance().FriendDataList[cid].map_id;
                        go.transform.Find("Toggle/containts/txtpos").GetComponent<Text>().text = SvrMapConfig.instance.getSingleMapConf((uint)mapId)["map_name"]._str;
                    }
                    else
                        go.transform.Find("Toggle/containts/txtpos").GetComponent<Text>().text = ContMgr.getCont("a3_friend_lx");
                    go.transform.SetParent(tfFriendContainer, false);
                    new BaseButton(go.transform.Find("btn_invite")).onClick = (_go) =>
                    {
                        TeamProxy.getInstance().SendInvite(cid);
                    };
                }
            }
        }

        void RefreshNearby()
        {
            for (int i = tfNearbyContainer.childCount; i > 0; i--)
                GameObject.Destroy(tfNearbyContainer.GetChild(i - 1).gameObject);
            foreach (KeyValuePair<uint, ProfessionRole> item in OtherPlayerMgr._inst.m_mapOtherPlayerSee)
            {
                GameObject go = GameObject.Instantiate(prefabFriendInfo);
                string name = item.Value.roleName;
                string level = item.Value.zhuan + ContMgr.getCont("zhuan") + item.Value.lvl + ContMgr.getCont("ji");
                int combat = item.Value.combpt;
                uint cid = item.Value.m_unCID;
                uint map_id = PlayerModel.getInstance().mapid;
                go.transform.Find("Toggle/containts/txtName").GetComponent<Text>().text = name;
                go.transform.Find("Toggle/containts/txtLevel").GetComponent<Text>().text = level;
                go.transform.Find("Toggle/containts/txtcombat").GetComponent<Text>().text = combat.ToString();
                go.transform.Find("Toggle/containts/txtpos").GetComponent<Text>().text = SvrMapConfig.instance.getSingleMapConf(map_id)["map_name"]._str;
                go.transform.SetParent(tfNearbyContainer, false);
                new BaseButton(go.transform.Find("btn_invite")).onClick = (_go) =>
                 {
                     TeamProxy.getInstance().SendInvite(cid);
                 };
            }
        }

        void OnRefresh()
        {
            FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);
        }
    }
}
