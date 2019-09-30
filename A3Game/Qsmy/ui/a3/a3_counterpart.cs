using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using DG.Tweening;
using static MuGame.a3_currentTeamPanel;
using System.Collections;

namespace MuGame
{

    class a3_counterpart : Window
    {

        Dictionary<string, a3BaseActive> _activies = new Dictionary<string, a3BaseActive>();

        public static a3_counterpart instance;
        public GameObject choicePanel;
        public GameObject yesNo;
        public GameObject less5;
        public GameObject yaoQing;

        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        BaseButton enterbtn4;
        BaseButton enterbtn5;
        BaseButton enterbtn6;
        BaseButton enterbtn7;

        BaseButton left;
        BaseButton right;
        BaseButton left1;
        BaseButton right1;        
        
        public Text txtName;
        public Text txtLvl;
        public Text txtPro;
        Toggle toggle;
        static public int nummeb;
        public Dictionary<uint, itemFriendData> RecommendListDic = new Dictionary<uint, itemFriendData>();

        public TabControl tab;
        GameObject single;
        GameObject multiPlayer;
        public static int lvl = 0;

        public uint begin_index = 0;
        public uint end_index = 20;
        public List<uint> kout = new List<uint>();//组队经验副本不符合等级的同队玩家cid
        public List<uint> canin = new List<uint>();//全队都能进入副本的cid
        public Dictionary<uint, bool> readyGofb = new Dictionary<uint, bool>();//队伍中的cid和ready;

        int mode = 0; //组队副本的类型 1,金币 2,经验 3,材料 4,幽灵

        int diff = 0;
        Transform tran;
        Transform tran1;
        public Transform currentTeam;
        public Transform peopleLess;
        Transform yao;
        Transform friends;
        Transform objects;
        Transform tenSlider;

        Sprite icon2;
        Sprite icon3;
        Sprite icon5;

        public uint readyLtpid;
        public uint readyLdiff;
        bool needTrues = false;

        List<ItemMemberObj> itemMemberObjList;
        Dictionary<uint, string> ltemObjList = new Dictionary<uint, string>();
        public static a3_counterpart _instance; 
        public override void init()
        {
            instance = this;
            inText();
            _activies["exp"] = new a3_counterpart_exp(this, "contents/exp");
            _activies["gold"] = new a3_counterpart_gold(this, "contents/gold");
            _activies["material"] = new a3_counterpart_mate(this, "contents/material");

            _activies["mexp"] = new a3_counterpart_multi_exp(this, "multiConDiff/exp");
            _activies["mgold"] = new a3_counterpart_multi_gold(this, "multiConDiff/gold");
            _activies["mmaterial"] = new a3_counterpart_multi_mate(this, "multiConDiff/material");
            _activies["ghost"] = new a3_counterpart_multi_ghost(this, "multiConDiff/ghost");


            enterbtn1 = new BaseButton(getTransformByPath("scroll_view_fuben/contain/gold/enter"));
            enterbtn2 = new BaseButton(getTransformByPath("scroll_view_fuben/contain/exp/enter"));
            enterbtn3 = new BaseButton(getTransformByPath("scroll_view_fuben/contain/material/enter"));

            enterbtn4 = new BaseButton(getTransformByPath("multiPlayer/contain/gold/enter"));
            enterbtn5 = new BaseButton(getTransformByPath("multiPlayer/contain/exp/enter"));
            enterbtn6 = new BaseButton(getTransformByPath("multiPlayer/contain/material/enter"));
            enterbtn7 = new BaseButton(getTransformByPath("multiPlayer/contain/ghost/enter"));

            icon2 = GAMEAPI.ABUI_LoadSprite("icon_team_warrior_team");
            icon3 = GAMEAPI.ABUI_LoadSprite("icon_team_mage_team");
            icon5 = GAMEAPI.ABUI_LoadSprite("icon_team_assassin_team");

            right = new MuGame.BaseButton(getTransformByPath("right"));
            left = new BaseButton(getTransformByPath("left"));
            right1 = new MuGame.BaseButton(getTransformByPath("right1"));
            left1 = new BaseButton(getTransformByPath("left1"));

            currentTeam = getTransformByPath("currentTeam/currentTeamPanel");
            tenSlider = getTransformByPath("ready/yesorno/slider/fill");
            peopleLess = getTransformByPath("peopelLess");
            itemMemberObjList = new List<ItemMemberObj>();
            ltemObjList = new Dictionary<uint, string>();
            objects = currentTeam.FindChild("right/contains");
            tenSlider.localScale = new Vector3(0, 1, 1);
            #region====五个人界面的邀请按钮
            for (int i = 0; i < objects.childCount; i++)
            {
                Transform child = objects.GetChild(i);
                ItemMemberObj itemMemberObj = new ItemMemberObj(child);
                itemMemberObjList.Add(itemMemberObj);
                new BaseButton(getTransformByPath("currentTeam/currentTeamPanel/right/contains/" + i + "/empty/btnInvite")).onClick = (GameObject go) =>
                      {
                          FriendProxy.getInstance().sendOnlineRecommend();
                          yaoQing.SetActive(true);
                      };
            }
            #endregion

            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);
            single = getGameObjectByPath("scroll_view_fuben");
            multiPlayer = getGameObjectByPath("multiPlayer");

            yesNo = getGameObjectByPath("team");
            less5 = getGameObjectByPath("teamLess5");
            yaoQing = getGameObjectByPath("yaoqing");
            yao = getTransformByPath("yaoqing/main/scroll/containts");
            friends = getTransformByPath("haoyou/main/scroll/containts");
            #region====向左向右点击按钮
            tran = getTransformByPath("scroll_view_fuben/contain");
            Tween tween;
            right.onClick = (GameObject go) =>
              {
                  if (tran.localPosition.x < (getTransformByPath("scroll_view_fuben/contain").childCount - 3) * 300)
                  {

                  }
                  else
                      tween = tran.DOLocalMoveX(tran.localPosition.x - 10f, 0.5f);
              };
            left.onClick = (GameObject go) =>
            {
                if (tran.localPosition.x > (getTransformByPath("scroll_view_fuben/contain").childCount - 3) * 300)
                {

                }
                else
                    tween = tran.DOLocalMoveX(tran.localPosition.x + 10f, 0.5f);
            };

            tran1 = getTransformByPath("multiPlayer/contain");
            Tween tween1;
            right1.onClick = (GameObject go) =>
            {
                if (tran1.localPosition.x < (getTransformByPath("multiPlayer/contain").childCount - 3) * 300)
                {

                }
                else
                    tween1 = tran1.DOLocalMoveX(tran1.localPosition.x - 10f, 0.5f);
            };
            left1.onClick = (GameObject go) =>
            {
                if (tran1.localPosition.x > (getTransformByPath("multiPlayer/contain").childCount - 3) * 300)
                {

                }
                else
                    tween1 = tran1.DOLocalMoveX(tran1.localPosition.x + 10f, 0.5f);
            };
            #endregion
            this.transform.FindChild("multiPlayer/contain/exp/bg_awd").gameObject.SetActive(false);
            this.transform.FindChild("multiPlayer/contain/material/bg_awd").gameObject.SetActive(false);
            this.transform.FindChild("multiPlayer/contain/gold/bg_awd").gameObject.SetActive(false);
            SetAwd();
            #region====button
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) =>
            {
                Toclose = true;
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                leave = false;
            };
            //金币钻石绑钻按钮
            new BaseButton(getTransformByPath("jingbi/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                if (a3_exchange.Instance != null)
                    a3_exchange.Instance.transform.SetAsLastSibling();

            };
            new BaseButton(getTransformByPath("zuanshi/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                if (a3_Recharge.Instance != null)
                    a3_Recharge.Instance.transform.SetAsLastSibling();
            };
            new BaseButton(getTransformByPath("bangzuan/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HONOR);
                if (a3_honor.instan  != null)
                    a3_honor.instan.transform.SetAsLastSibling();
            };
            //===================

            //单人模式的进入和关闭
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/gold/enter")).onClick = (GameObject go) =>
            {                
                choicePanel = getGameObjectByPath("contents/gold");
                choicePanel.SetActive(true);
            };
            new BaseButton(getTransformByPath("contents/gold/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            //--------------------
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/exp/enter")).onClick = (GameObject go) =>
            {
                choicePanel = getGameObjectByPath("contents/exp");
                choicePanel.SetActive(true);
            };
            new BaseButton(getTransformByPath("contents/exp/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            //--------------------
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/material/enter")).onClick = (GameObject go) =>
            {
                choicePanel = getGameObjectByPath("contents/material");
                choicePanel.SetActive(true);
            };
            new BaseButton(getTransformByPath("contents/material/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            ///===================

            //进入组队
            new BaseButton(getTransformByPath("multiPlayer/contain/exp/enter")).onClick = (GameObject go) =>
            {
                if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(108),a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(108)))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_counterpart_lvl"));
                    return;
                }
                if(a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.multi_exp);
                    ast.Add(108);
                    ast.Add("multiConDiff/exp");
                    ast.Add(2);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE,ast);
                }
                else
                {
                    TreamFb(108, "multiConDiff/exp",2);
                }
                #region  旧的
                //Vector3 entPos = a3_counterpartModel.getInstance().GetPosByLevelId(108);
                //int mapId = a3_counterpartModel.getInstance().GetMapIdByLevelId(108);
                //if (entPos != Vector3.zero && mapId != 0)
                //{
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                //    if (MapModel.getInstance().dicMappoint[mapId] != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] && mapId != GRMap.instance.m_nCurMapID)
                //        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => { SelfRole.WalkToMap(mapId, entPos); });
                //    else SelfRole.WalkToMap(mapId, entPos);
                //}
                //else
                //{
                //    if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid != 108)
                //    {
                //        if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                //        {
                //            flytxt.instance.fly(ContMgr.getCont("counterpart_1")); return;
                //        }
                //        TeamProxy.getInstance().sendobject_change(108);
                //    }
                //    choicePanel = getGameObjectByPath("multiConDiff/exp");
                //    mode = 2;//组队副本的类型
                //    Variant data = SvrLevelConfig.instacne.get_level_data(108);
                //    currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                //    cTeam();
                //}
                #endregion
            };
            new BaseButton(getTransformByPath("multiPlayer/contain/gold/enter")).onClick = (GameObject go) =>
            {
                if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(109), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(109)))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_counterpart_lvl"));
                    return;
                }
                if (a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.multi_gold);
                    ast.Add(109);
                    ast.Add("multiConDiff/gold");
                    ast.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ast);
                }
                else
                {
                    TreamFb(109, "multiConDiff/gold", 1);
                }
                #region 旧的
                //Vector3 entPos = a3_counterpartModel.getInstance().GetPosByLevelId(109);
                //int mapId = a3_counterpartModel.getInstance().GetMapIdByLevelId(109);
                //if (entPos != Vector3.zero && mapId != 0)
                //{
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                //    if (MapModel.getInstance().dicMappoint[mapId] != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] && mapId != GRMap.instance.m_nCurMapID)
                //        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => { SelfRole.WalkToMap(mapId, entPos); });
                //    else SelfRole.WalkToMap(mapId, entPos);
                //}
                //else
                //{
                //    if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid != 109)
                //    {
                //        if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                //        {
                //            flytxt.instance.fly(ContMgr.getCont("counterpart_1")); return;
                //        }
                //        TeamProxy.getInstance().sendobject_change(109);
                //    }
                //    choicePanel = getGameObjectByPath("multiConDiff/gold");
                //    mode = 1;//组队副本的类型
                //    Variant data = SvrLevelConfig.instacne.get_level_data(109);
                //    currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                //    cTeam();
                //}
                #endregion
            };
            new BaseButton(getTransformByPath("multiPlayer/contain/material/enter")).onClick = (GameObject go) =>
            {
                if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(110), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(110)))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_counterpart_lvl"));
                    return;
                }
                if (a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.multi_mate);
                    ast.Add(110);
                    ast.Add("multiConDiff/material");
                    ast.Add(3);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ast);
                }
                else
                {
                    TreamFb(110, "multiConDiff/material", 3);
                }
                #region 旧的
                //Vector3 entPos = a3_counterpartModel.getInstance().GetPosByLevelId(110);
                //int mapId = a3_counterpartModel.getInstance().GetMapIdByLevelId(110);
                //if (entPos != Vector3.zero && mapId != 0)
                //{
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                //    if (MapModel.getInstance().dicMappoint[mapId] != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] && mapId != GRMap.instance.m_nCurMapID)
                //        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => { SelfRole.WalkToMap(mapId, entPos); });
                //    else SelfRole.WalkToMap(mapId, entPos);
                //}
                //else
                //{
                //    if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid != 110)
                //    {
                //        if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                //        {
                //            flytxt.instance.fly(ContMgr.getCont("counterpart_1")); return;
                //        }
                //        TeamProxy.getInstance().sendobject_change(110);
                //    }
                //    choicePanel = getGameObjectByPath("multiConDiff/material");
                //    mode = 3;//组队副本的类型
                //    Variant data = SvrLevelConfig.instacne.get_level_data(110);
                //    currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                //    cTeam();
                //}
                #endregion
            };
            new BaseButton(getTransformByPath("multiPlayer/contain/ghost/enter")).onClick = (GameObject go) =>
            {
                if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(111), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(111)))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_counterpart_lvl"));
                    return;
                }
                if (a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.multi_ghost);
                    ast.Add(111);
                    ast.Add("multiConDiff/ghost");
                    ast.Add(4);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ast);
                }
                else
                {
                    TreamFb(111, "multiConDiff/ghost", 4);
                }
                #region 旧的
                //Vector3 entPos = a3_counterpartModel.getInstance().GetPosByLevelId(111);
                //int mapId = a3_counterpartModel.getInstance().GetMapIdByLevelId(111);
                //if (entPos != Vector3.zero && mapId != 0)
                //{
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                //    if (MapModel.getInstance().dicMappoint[mapId] != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] && mapId != GRMap.instance.m_nCurMapID)
                //        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => { SelfRole.WalkToMap(mapId, entPos); });
                //    else SelfRole.WalkToMap(mapId, entPos);
                //}
                //else
                //{
                //    if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid != 111)
                //    {
                //        if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                //        {
                //            flytxt.instance.fly(ContMgr.getCont("counterpart_1")); return;
                //        }
                //        TeamProxy.getInstance().sendobject_change(111);
                //    }
                //    choicePanel = getGameObjectByPath("multiConDiff/ghost");
                //    mode = 4;//组队副本的类型
                //    Variant data = SvrLevelConfig.instacne.get_level_data(111);
                //    currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                //    cTeam();
                //}
                #endregion
            };

            new BaseButton(this.transform.FindChild("multiPlayer/contain/exp/title_bgbg2")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/exp/bg_awd").gameObject.SetActive(true);
            };
            new BaseButton(this.transform.FindChild("multiPlayer/contain/material/title_bgbg2")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/material/bg_awd").gameObject.SetActive(true);
            };
            new BaseButton(this.transform.FindChild("multiPlayer/contain/gold/title_bgbg2")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/gold/bg_awd").gameObject.SetActive(true);
            };

            #region

            //===================

            //组队模式的难度选择关闭按钮
            new BaseButton(getTransformByPath("multiConDiff/gold/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            new BaseButton(getTransformByPath("multiConDiff/exp/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            new BaseButton(getTransformByPath("multiConDiff/material/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            new BaseButton(getTransformByPath("multiConDiff/ghost/btn_close")).onClick = (GameObject go) =>
            {
                choicePanel.SetActive(false);
            };
            //===================

            //弹框==手动组队和自动组队
            new BaseButton(getTransformByPath("team/yesorno/shd")).onClick = (GameObject go) =>
            {
                onBtnCreateClick(go);
                //getGameObjectByPath("currentTeam").SetActive(true);
                getGameObjectByPath("team").SetActive(false);
            };
            new BaseButton(getTransformByPath("team/yesorno/zid")).onClick = (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add(0);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPEEDTEAM, arr);
                getGameObjectByPath("team").SetActive(false);
            };
            //===================

            //队伍人数小于5
            new BaseButton(getTransformByPath("teamLess5/yesorno/yes")).onClick = (GameObject go) =>
            {
                less5.SetActive(false);
                FriendProxy.getInstance().sendOnlineRecommend();
                yaoQing.SetActive(true);
            };
            //===================

            //弹框和一键邀请的关闭
            new BaseButton(getTransformByPath("team/yesorno/no")).onClick = (GameObject go) =>
            {
                yesNo.SetActive(false);
            };
            new BaseButton(getTransformByPath("teamLess5/yesorno/no")).onClick = (GameObject go) =>
            {
                less5.SetActive(false);
            };
            new BaseButton(getTransformByPath("yaoqing/main/title/title/close")).onClick = (GameObject go) =>
            {
                yaoQing.SetActive(false);
                for (int i = 0; i < yao.childCount; i++)
                {
                    Destroy(yao.GetChild(i).gameObject);
                }
            };
            //===================

            //全选和邀请组队、好友
            bool isons = false;
            bool isonline = false;
            new BaseButton(getTransformByPath("yaoqing/bottom/btnSelectAll")).onClick = (GameObject go) =>
              {
                  if (yao != null && yao.childCount > 0 && isonline == false)
                  {
                      for (int i = 0; i < yao.childCount; i++)
                      {
                          Transform child = yao.GetChild(i);
                          child.FindChild("Toggle").GetComponent<Toggle>().isOn = true;
                      }
                      isonline = true;
                      getTransformByPath("yaoqing/bottom/btnSelectAll/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_quxiao");
                      return;
                  }
                  if (yao != null && yao.childCount > 0 && isonline == true)
                  {
                      for (int i = 0; i < yao.childCount; i++)
                      {
                          Transform child = yao.GetChild(i);
                          child.FindChild("Toggle").GetComponent<Toggle>().isOn = false;
                      }
                      isonline = false;
                      getTransformByPath("yaoqing/bottom/btnSelectAll/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_quanxuan");
                      return;
                  }

              };
            new BaseButton(getTransformByPath("yaoqing/bottom/btnInvite")).onClick = (GameObject go) =>
            {
                for (int i = 0; i < yao.childCount; i++)
                {
                    Transform child = yao.GetChild(i);
                    foreach (var item in ltemObjList)
                    {
                        item.Value.Equals(child.FindChild("texts/txtNickName").GetComponent<Text>().text);
                        if (item.Value == child.FindChild("texts/txtNickName").GetComponent<Text>().text && child.FindChild("Toggle").GetComponent<Toggle>().isOn == true)
                        {
                            uint cidTemp = item.Key;
                            TeamProxy.getInstance().SendInvite(cidTemp);
                        }
                    }
                }
                yaoQing.SetActive(false);
                for (int i = 0; i < yao.childCount; i++)
                {
                    Destroy(yao.GetChild(i).gameObject);
                }
            };
            new BaseButton(getTransformByPath("haoyou/bottom/btnSelectAll")).onClick = (GameObject go) =>
            {
                if (friends != null && friends.childCount > 0 && isons == false)
                {
                    for (int i = 0; i < friends.childCount; i++)
                    {
                        Transform child = friends.GetChild(i);
                        child.FindChild("Toggle").GetComponent<Toggle>().isOn = true;
                    }
                    isons = true;
                    getTransformByPath("haoyou/bottom/btnSelectAll/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_quxiao");
                    return;
                }
                if (friends != null && friends.childCount > 0 && isons == true)
                {
                    for (int i = 0; i < friends.childCount; i++)
                    {
                        Transform child = friends.GetChild(i);
                        child.FindChild("Toggle").GetComponent<Toggle>().isOn = false;
                    }
                    isons = false;
                    getTransformByPath("haoyou/bottom/btnSelectAll/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_quanxuan");
                    return;
                }
            };
            new BaseButton(getTransformByPath("haoyou/bottom/btnInvite")).onClick = (GameObject go) =>
            {
                for (int i = 0; i < friends.childCount; i++)
                {
                    Transform child = friends.GetChild(i);
                    foreach (var item in ltemObjList)
                    {
                        item.Value.Equals(child.FindChild("texts/txtNickName").GetComponent<Text>().text);
                        if (item.Value == child.FindChild("texts/txtNickName").GetComponent<Text>().text && child.FindChild("Toggle").GetComponent<Toggle>().isOn == true)
                        {
                            uint cidTemp = item.Key;
                            TeamProxy.getInstance().SendInvite(cidTemp);
                        }
                    }
                }
                getGameObjectByPath("haoyou").SetActive(false);
                for (int i = 0; i < friends.childCount; i++)
                {
                    Destroy(friends.GetChild(i).gameObject);
                }
            };
            //==================

            //当前队伍列表中的好友、开始和关闭
            new BaseButton(getTransformByPath("currentTeam/currentTeamPanel/right/bottom/btnStart")).onClick = (GameObject go) =>
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                {//开始
                    if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 0)
                    {
                        getGameObjectByPath("teamLess5").SetActive(true);
                    }
                    else
                    {
                        getGameObjectByPath("teamLess5").SetActive(false);
                        getGameObjectByPath("currentTeam").SetActive(false);
                        switch (mode)
                        {
                            case 1: getGameObjectByPath("multiConDiff/gold").SetActive(true); break;
                            case 2: getGameObjectByPath("multiConDiff/exp").SetActive(true); break;
                            case 3: getGameObjectByPath("multiConDiff/material").SetActive(true); break;
                            case 4: getGameObjectByPath("multiConDiff/ghost").SetActive(true); break;
                            default:
                                break;
                        }
                    }
                }
                else//准备
                {
                    flytxt.instance.fly("等待中");
                }


            };
            new BaseButton(getTransformByPath("currentTeam/currentTeamPanel/right/bottom/friend")).onClick = (GameObject go) =>
            {
                FriendProxy.getInstance().sendfriendlist(FriendProxy.FriendType.FRIEND);
                getGameObjectByPath("haoyou").SetActive(true);
            };
            new BaseButton(getTransformByPath("haoyou/main/title/title/close")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("haoyou").SetActive(false);
                for (int i = 0; i < friends.childCount; i++)
                {
                    Destroy(friends.GetChild(i).gameObject);
                }
            };
            new BaseButton(getTransformByPath("currentTeam/currentTeamPanel/right/bottom/btnQuitTeam")).onClick = (GameObject go) =>
            {
                onBtnQuitClick(go);
                getGameObjectByPath("currentTeam").SetActive(false);
            };
            new BaseButton(getTransformByPath("currentTeam/currentTeamPanel/close")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("currentTeam").SetActive(false);
            };
            //队伍有玩家不满足条件后踢人
            new BaseButton(getTransformByPath("peopelLess/yesorno/yes")).onClick = (GameObject go) =>
              {
                  for (int i = 0; i < kout.Count; i++)
                  {
                      TeamProxy.getInstance().SendKickOut(kout[i]);
                  }
                  peopleLess.gameObject.SetActive(false);
                  //getGameObjectByPath("currentTeam").SetActive(true);
              };
            new BaseButton(getTransformByPath("peopelLess/yesorno/no")).onClick = (GameObject go) =>
            {
                peopleLess.gameObject.SetActive(false);
                //getGameObjectByPath("currentTeam").SetActive(true);
            };
            new BaseButton(getTransformByPath("ready/yesorno/yes")).onClick = (GameObject go) =>
            {
                yesReady = true;
                TeamProxy.getInstance().SendReady(true, readyLtpid, readyLdiff);
                //getGameObjectByPath("currentTeam").SetActive(true);
                Transform show = getTransformByPath("ready/yesorno/show");
                if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                {
                    getButtonByPath("ready/yesorno/yes").interactable = false;
                    for (int i = 0; i < show.childCount; i++)
                    {
                        if (show.GetChild(i).transform.FindChild("yes").gameObject.activeSelf == false && needTrues == false)
                        {
                            show.GetChild(i).transform.FindChild("yes").gameObject.SetActive(true);
                            show.GetChild(i).transform.FindChild("name").GetComponent<Text>().text =ContMgr.getCont("a3_counterpart_mine");
                            needTrues = true;
                            return;
                        }
                    }
                }
            };
            new BaseButton(getTransformByPath("ready/yesorno/no")).onClick = (GameObject go) =>
            {
                TeamProxy.getInstance().SendReady(false, readyLtpid, readyLdiff);
                sendfalse(true);
                tenSlider.DOKill();
                tenSeconed(false);
            };
            if (A3_TeamModel.getInstance().bein)
            {
                Variant data = SvrLevelConfig.instacne.get_level_data(A3_TeamModel.getInstance().ltpids);
                currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                //getGameObjectByPath("currentTeam").SetActive(true);
            }
            //===================
            tab = new TabControl();
            tab.onClickHanle = ontab;
            tab.create(getGameObjectByPath("tabs"), gameObject, 0, 0);
            //====进入副本的权限
            CheckLock();
         
            changePos();
            butoText();
            #endregion
            #endregion
        }

        void inText()
        {
            this.transform.FindChild("tabs/single/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_1");
            this.transform.FindChild("tabs/multiplayer/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_2");
            this.transform.FindChild("multiPlayer/contain/exp/title_bgbg2/reword").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_3");
            //this.transform.FindChild("multiPlayer/contain/exp/enter/TextGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_4");
            this.transform.FindChild("multiPlayer/contain/exp/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_5");
            this.transform.FindChild("multiPlayer/contain/material/title_bgbg2/reword").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_6");
           // this.transform.FindChild("multiPlayer/contain/material/enter/TextGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_7");
            this.transform.FindChild("multiPlayer/contain/material/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_8");
            this.transform.FindChild("multiPlayer/contain/gold/title_bgbg2/reword").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_9");
           // this.transform.FindChild("multiPlayer/contain/gold/enter/TextGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_10");
            this.transform.FindChild("multiPlayer/contain/gold/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_11");

            this.transform.FindChild("scroll_view_fuben/contain/exp/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_12");
            this.transform.FindChild("scroll_view_fuben/contain/exp/enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_13");

            this.transform.FindChild("scroll_view_fuben/contain/gold/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_14");
            this.transform.FindChild("scroll_view_fuben/contain/gold/enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_15");

            this.transform.FindChild("scroll_view_fuben/contain/material/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_16");
            this.transform.FindChild("scroll_view_fuben/contain/material/enter/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_counterpart_17");
        }

        //组队副本              
        public void TreamFb(uint id,string path,int mod)
        {
            Vector3 entPos = a3_counterpartModel.getInstance().GetPosByLevelId(id);
            int mapId = a3_counterpartModel.getInstance().GetMapIdByLevelId(id);
            if (entPos != Vector3.zero && mapId != 0)
            { 
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                if (MapModel.getInstance().dicMappoint[mapId] != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] && mapId != GRMap.instance.m_nCurMapID)
                    SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => { SelfRole.WalkToMap(mapId, entPos); });
                else SelfRole.WalkToMap(mapId, entPos);
            }
            else
            {
                if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid != id)
                {
                    if (!TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        flytxt.instance.fly(ContMgr.getCont("counterpart_1")); return;
                    }
                    TeamProxy.getInstance().sendobject_change((int)id);
                }
                choicePanel = getGameObjectByPath(path);
                mode = mod;//组队副本的类型
                Variant data = SvrLevelConfig.instacne.get_level_data(id);
                currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
                cTeam();
            }

        }

        void SetAwd() {
            Transform con1 = this.transform.FindChild("multiPlayer/contain/exp/bg_awd/scrollview/con");
            Transform con2 = this.transform.FindChild("multiPlayer/contain/material/bg_awd/scrollview/con");
            Transform con3 = this.transform.FindChild("multiPlayer/contain/gold/bg_awd/scrollview/con");
            new BaseButton(this.transform.FindChild("multiPlayer/contain/exp/bg_awd/close")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/exp/bg_awd").gameObject.SetActive(false);
            };
            new BaseButton(this.transform.FindChild("multiPlayer/contain/material/bg_awd/close")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/material/bg_awd").gameObject.SetActive(false);
            };
            new BaseButton(this.transform.FindChild("multiPlayer/contain/gold/bg_awd/close")).onClick = (GameObject go) => {
                this.transform.FindChild("multiPlayer/contain/gold/bg_awd").gameObject.SetActive(false);
            };

            for (int i = 0; i < con1.childCount; i++)
            {
                Destroy(con1.GetChild (i).gameObject);
            }
            for (int i = 0; i < con2.childCount; i++)
            {
                Destroy(con2.GetChild(i).gameObject);
            }
            for (int i = 0; i < con3.childCount; i++)
            {
                Destroy(con3.GetChild(i).gameObject);
            }
            
            GameObject item = this.transform.FindChild("multiPlayer/icon").gameObject;
          
            SXML xml1 = XMLMgr.instance.GetSXML("worldboss.mdrop", "mid==" + 108);
            List<SXML> awdlist1 = xml1.GetNodeList("item");

            foreach (SXML x in awdlist1) {
                GameObject prefabItemIcon = GameObject.Instantiate(item) as GameObject;
                prefabItemIcon.SetActive(true);
                GameObject goItemReward;
                RewardDescText? rewardDescText = null;
                if (x.getInt ("type")== 3) {
                    rewardDescText = new RewardDescText
                    {
                        strItemName = x.GetNode("item_name").getString("tip"),
                        strTipDesc = "",
                        strCarrLimit = x.GetNode("carr_limit").getString("tip"),
                        strBaseAttr = x.GetNode("desc1").getString("tip"),
                        strAddAttr = x.GetNode("desc2").getString("tip"),
                        strExtraDesc1 = x.GetNode("random_tip1").getString("tip"),
                        strExtraDesc2 = x.GetNode("random_tip2").getString("tip"),
                        strExtraDesc3 = x.GetNode("random_tip3").getString("tip")
                    };
                }
                //else if (x.getInt("type") == 1) {
                //    goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                //    goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                //}
                goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                prefabItemIcon.transform.SetParent(con1,false);
                ArrayList paraList = new ArrayList();
                uint itemId = x.getUint("id");
                paraList.Add(itemId);
                int showType = x.getInt("type");
                paraList.Add(showType);
                paraList.Add(rewardDescText);
                new BaseButton(goItemReward.transform).onClick = (GameObject go) => {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
                };
            }

            SXML xml2 = XMLMgr.instance.GetSXML("worldboss.mdrop", "mid==" + 109);
            List<SXML> awdlist2 = xml1.GetNodeList("item");

            foreach (SXML x in awdlist2)
            {
                GameObject prefabItemIcon = GameObject.Instantiate(item) as GameObject;
                prefabItemIcon.SetActive(true);
                RewardDescText? rewardDescText = null;
                if (x.getInt("type") == 3)
                {

                    rewardDescText = new RewardDescText
                    {
                        strItemName = x.GetNode("item_name").getString("tip"),
                        strTipDesc = "",
                        strCarrLimit = x.GetNode("carr_limit").getString("tip"),
                        strBaseAttr = x.GetNode("desc1").getString("tip"),
                        strAddAttr = x.GetNode("desc2").getString("tip"),
                        strExtraDesc1 = x.GetNode("random_tip1").getString("tip"),
                        strExtraDesc2 = x.GetNode("random_tip2").getString("tip"),
                        strExtraDesc3 = x.GetNode("random_tip3").getString("tip")
                    };
                }
                //else if (x.getInt("type") == 1)
                //{
                //    GameObject goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                //    goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                //}
                GameObject goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                prefabItemIcon.transform.SetParent(con2, false);
                ArrayList paraList = new ArrayList();
                uint itemId = x.getUint("id");
                paraList.Add(itemId);
                int showType = x.getInt("type");
                paraList.Add(showType);
                paraList.Add(rewardDescText);
                new BaseButton(goItemReward.transform).onClick = (GameObject go) => {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
                };
            }

            SXML xml3 = XMLMgr.instance.GetSXML("worldboss.mdrop", "mid==" + 109);
            List<SXML> awdlist3 = xml1.GetNodeList("item");

            foreach (SXML x in awdlist3)
            {
                GameObject prefabItemIcon = GameObject.Instantiate(item) as GameObject;
                prefabItemIcon.SetActive(true);
                RewardDescText? rewardDescText = null;
                if (x.getInt("type") == 3)
                {

                    rewardDescText = new RewardDescText
                    {
                        strItemName = x.GetNode("item_name").getString("tip"),
                        strTipDesc = "",
                        strCarrLimit = x.GetNode("carr_limit").getString("tip"),
                        strBaseAttr = x.GetNode("desc1").getString("tip"),
                        strAddAttr = x.GetNode("desc2").getString("tip"),
                        strExtraDesc1 = x.GetNode("random_tip1").getString("tip"),
                        strExtraDesc2 = x.GetNode("random_tip2").getString("tip"),
                        strExtraDesc3 = x.GetNode("random_tip3").getString("tip")
                    };
                }
                //else if (x.getInt("type") == 1)
                //{
                //    GameObject goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                //    goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                //}
                GameObject goItemReward = IconImageMgr.getInstance().createA3ItemIcon(x.getUint("id"), istouch: true, ignoreLimit: true);
                goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                prefabItemIcon.transform.SetParent(con3, false);
                ArrayList paraList = new ArrayList();
                uint itemId = x.getUint("id");
                paraList.Add(itemId);
                int showType = x.getInt("type");
                paraList.Add(showType);
                paraList.Add(rewardDescText);
                new BaseButton(goItemReward.transform).onClick = (GameObject go) => {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
                };
            }
        }



        Tween tween = null;
        void tenSeconed(bool comp)
        {
            if (comp)
            {
                tween = tenSlider.DOScaleX(1, 9f);
                tween.OnComplete(() => OnComplete());
            }
            else
            {
                tween = tenSlider.DOScaleX(0, 0.01f);
                tween.OnComplete(() => OnComp());
            }

        }
        void OnComplete()
        {
        }
        void OnComp()
        {

        }
        void sendfalse(bool fly)
        {

            tenSlider.localScale = new Vector3(0, 1, 1);
            //getGameObjectByPath("ready").SetActive(false);
            //getGameObjectByPath("currentTeam").SetActive(true);
            Transform show = getTransformByPath("ready/yesorno/show");
            for (int i = 1; i < show.childCount; i++)
                if (show.GetChild(i).transform.FindChild("yes").gameObject.activeSelf == true)
                {
                    show.GetChild(i).transform.FindChild("yes").gameObject.SetActive(false);
                    show.GetChild(i).transform.FindChild("name").GetComponent<Text>().text = string.Empty;
                }
            yesReady = false;
            readyLtpid = 0;
            readyLdiff = 0;
            diffStr = "";
            readyGofb.Clear();
            needTrues = false;
            if (fly)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_counterpart_nopeoplego"));
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
            }
        }
        public void tenSen()//队长开始副本
        {
            tenSlider.DOKill();
            tenSeconed(true);
        }
        Sprite icon;
        #region 事件监听函数
        void onLeaveTeam(GameEvent e)
        {
            if (gameObject == null) return;
            getButtonByPath("ready/yesorno/yes").interactable = true;
            getGameObjectByPath("ready").SetActive(false);
            getGameObjectByPath("currentTeam").SetActive(false);
            yesReady = false;
            A3_TeamModel.getInstance().cidName.Clear();
            tenSlider.DOKill();
            tenSeconed(false);
            butoText();
            changePos();
        }
        void onCreatTeam(GameEvent e)
        {
            A3_TeamModel.getInstance().cidName.Clear();
            if (e.data["ltpid"] == 108 || e.data["ltpid"] == 109 || e.data["ltpid"] == 110 || e.data["ltpid"] == 111)
            {
                butoText();
                changePos();
                if (!A3_TeamModel.getInstance().cidName.ContainsKey(PlayerModel.getInstance().cid))
                    A3_TeamModel.getInstance().cidName.Add(PlayerModel.getInstance().cid, PlayerModel.getInstance().name);
                tenSlider.DOKill();
                tenSeconed(false);
            }
        }
        void changeOBJ(GameEvent e)
        {
            switch (e.data["ltpid"]._int)
            {
                case 108:
                    TeamProxy.getInstance().MyTeamData.ltpid = 108;
                    butoText();
                    changePos(); break;
                case 109:
                    TeamProxy.getInstance().MyTeamData.ltpid = 109;
                    butoText();
                    changePos(); break;
                case 110:
                    TeamProxy.getInstance().MyTeamData.ltpid = 110;
                    butoText();
                    changePos(); break;
                case 111:
                    TeamProxy.getInstance().MyTeamData.ltpid = 111;
                    butoText();
                    changePos(); break;
                default:
                    butoText();
                    changePos();
                    break;
            }
            Variant data = SvrLevelConfig.instacne.get_level_data(e.data["ltpid"]);
            if (data != null)
                currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
        }
        string diffStr = "";
        bool yesReady = false;
        void readyGo(GameEvent e)
        {            
            changePos();
            butoText();
            Transform show = getTransformByPath("ready/yesorno/show");
            if (e.data["ready"])
            {
                //getGameObjectByPath("ready").SetActive(true);
                //getGameObjectByPath("currentTeam").SetActive(true);
                readyLdiff = e.data["ldiff"];
                readyLtpid = e.data["ltpid"];
                uint cid = e.data["cid"];
                getGameObjectByPath("ready").SetActive(true);
                transform.SetAsLastSibling();
                InterfaceMgr.getInstance().closeAllWin(new List<string> { InterfaceMgr.A3_COUNTERPART ,InterfaceMgr.A3_FB_TEAM });
                bool needTrue = false;
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                {
                    yesReady = true;
                    show.FindChild("0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                }
                else
                {
                    show.FindChild("0/name").GetComponent<Text>().text = A3_TeamModel.getInstance().cidName[TeamProxy.getInstance().MyTeamData.leaderCid];

                }
                if (cid == TeamProxy.getInstance().MyTeamData.leaderCid)//自己收不到自己的ready
                {
                    tenSlider.DOKill();
                    tenSeconed(true);
                }
                if (yesReady)
                {
                    getButtonByPath("ready/yesorno/yes").interactable = false;
                }
                else
                    getButtonByPath("ready/yesorno/yes").interactable = true;
                if (cid != TeamProxy.getInstance().MyTeamData.leaderCid && !readyGofb.ContainsKey(cid))
                {
                    for (int i = 0; i < show.childCount; i++)
                    {                        
                        if (A3_TeamModel.getInstance().cidName.ContainsKey(cid) && show.GetChild(i).transform.FindChild("yes").gameObject.activeSelf == false && needTrue == false)
                        {
                            show.GetChild(i).transform.FindChild("yes").gameObject.SetActive(true);
                            show.GetChild(i).transform.FindChild("name").GetComponent<Text>().text = A3_TeamModel.getInstance().cidName[cid];
                            needTrue = true;
                            if (!readyGofb.ContainsKey(cid))
                                readyGofb.Add(cid, e.data["ready"]);
                        }
                    }
                }
                switch (readyLdiff)
                {
                    case 1: diffStr = ContMgr.getCont("a3_counterpart_type0"); break;
                    case 2: diffStr = ContMgr.getCont("a3_counterpart_type1"); break;
                    case 3: diffStr = ContMgr.getCont("a3_counterpart_type2"); break;
                    case 4: diffStr = ContMgr.getCont("a3_counterpart_type3"); break;
                    default:
                        break;
                }
                Variant data = SvrLevelConfig.instacne.get_level_data(readyLtpid);
                getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = data["name"] + "--" + diffStr;
                if (readyGofb.Count == TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count - 1)
                {
                    tenSlider.DOKill();
                    tenSeconed(false);
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = readyLtpid;
                    sendData["diff_lvl"] = readyLdiff;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData);
                    getGameObjectByPath("ready").SetActive(false);
                    getGameObjectByPath("currentTeam").SetActive(false);
                    for (int i = 1; i < show.childCount; i++)
                    {
                        if (show.GetChild(i).transform.FindChild("yes").gameObject.activeSelf == true)
                        {
                            show.GetChild(i).transform.FindChild("yes").gameObject.SetActive(false);
                            show.GetChild(i).transform.FindChild("name").GetComponent<Text>().text = string.Empty;
                        }
                    }
                    readyLtpid = 0;
                    readyLdiff = 0;
                    diffStr = "";
                    readyGofb.Clear();
                    needTrues = false;
                }
            }
            else
            {
                sendfalse(true);
                tenSlider.DOKill();
                tenSeconed(false);
            }

        }
        void onKickOut(GameEvent e)
        {
            changePos();
            butoText();
            A3_TeamModel.getInstance().cidName.Remove(e.data["cid"]);
            getGameObjectByPath("ready").SetActive(false);
            getGameObjectByPath("currentTeam").SetActive(false);
        }
        void changeCap(GameEvent e)
        {
            changePos();
            butoText();
        }
        void noticeHaveLeave(GameEvent e)
        {
            butoText();
            changePos();
            A3_TeamModel.getInstance().cidName.Remove(e.data["cid"]);
        }
        uint leader_cid;
        void noticeInvite(GameEvent e)
        {
            if (e.data["ltpid"] == 108 || e.data["ltpid"] == 109 || e.data["ltpid"] == 110 || e.data["ltpid"] == 111)
            {
                leader_cid = e.data["leader_cid"]._uint;
                changePos();
                butoText();
                yesReady = false;
                foreach (var item in TeamProxy.getInstance().MyTeamData.itemTeamDataList)
                {
                    if (!A3_TeamModel.getInstance().cidName.ContainsKey(item.cid))
                    {
                        A3_TeamModel.getInstance().cidName.Add(item.cid, item.name);
                    }
                }
                if (!A3_TeamModel.getInstance().cidName.ContainsKey(PlayerModel.getInstance().cid))
                    A3_TeamModel.getInstance().cidName.Add(PlayerModel.getInstance().cid, PlayerModel.getInstance().name);
                Variant data = SvrLevelConfig.instacne.get_level_data(e.data["ltpid"]);
                if (data != null)
                    currentTeam.FindChild("title").GetComponent<Text>().text = data["name"];
            }

        }
        void onFriendList(GameEvent e)
        {
            Dictionary<uint, itemFriendData> friendDataList = FriendProxy.getInstance().FriendDataList;
            listFriend();
        }
        void onGetTeamListInfo(GameEvent e)
        {
            ItemTeamMemberData itm = TeamProxy.getInstance().mapItemTeamData;
            changePos();
            butoText();
        }
        void onNewMemberJoin(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            string name = data["name"];
            uint lvl = data["lvl"];
            uint zhuan = data["zhuan"];
            uint carr = data["carr"];
            uint combpt = data["combpt"];
            ItemTeamData itd = new ItemTeamData();
            itd.cid = cid;
            itd.name = name;
            itd.lvl = lvl;
            itd.zhuan = zhuan;
            itd.carr = carr;
            itd.combpt = (int)combpt;
            itd.isCaptain = false;
            itd.online = true;
            itd.teamId = TeamProxy.getInstance().MyTeamData.teamId;
            changePos();
            butoText();
            if (!A3_TeamModel.getInstance().cidName.ContainsKey(data["cid"]))
            {
                A3_TeamModel.getInstance().cidName.Add(cid, name);
            }
        }
        void commandList(GameEvent e)
        {
            List<itemFriendData> recommandDataList = FriendProxy.getInstance().RecommendDataList;
            recommandDataList.Sort(SortItemFriendData);
            listOnline();
            changePos();
            butoText();
        }
        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshMoney();
            }
            if (info.ContainsKey("yb"))
            {
                refreshGold();
            }
            if (info.ContainsKey("bndyb"))
            {
                refreshGift();
            }
        }
        #endregion
        void teamList()
        {
            ItemTeamMemberData itm = TeamProxy.getInstance().mapItemTeamData;
            foreach (ItemTeamData item in itm.itemTeamDataList)
            {
                if (TeamProxy.getInstance().MyTeamData == null)
                {
                    TeamProxy.getInstance().SendApplyJoinTeam(item.teamId);
                }
            }
        }
        void listFriend()
        {
            Transform tf = transform.FindChild("haoyou/itemReserFriend");
            Dictionary<uint, itemFriendData> re = FriendProxy.getInstance().FriendDataList;
            Transform root;
            RectTransform cont;
            cont = getTransformByPath("haoyou/main/scroll/containts").GetComponent<RectTransform>();
            cont.sizeDelta = new Vector2(cont.sizeDelta.x, re.Count * 60f);
            foreach (KeyValuePair<uint, itemFriendData> item in re)
            {
                GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
                root = gob.transform;
                gob.SetActive(true);
                root.SetParent(getTransformByPath("haoyou/main/scroll/containts"));
                txtName = root.transform.FindChild("texts/txtNickName").GetComponent<Text>();
                txtLvl = root.transform.FindChild("texts/txtLevel").GetComponent<Text>();
                txtPro = root.transform.FindChild("texts/txtProfessional").GetComponent<Text>();
                set(item.Value);
                gob.transform.localScale = Vector3.one;
                toggle = root.transform.FindChild("Toggle").GetComponent<Toggle>();
                if (ltemObjList.ContainsKey(item.Value.cid))
                {

                }
                else
                    ltemObjList.Add(item.Value.cid, item.Value.name);
            }
        }
        #region  常用函数
        public void changePos()//改变在当前队伍界面位置
        {
            if (TeamProxy.getInstance().MyTeamData != null && (TeamProxy.getInstance().MyTeamData.ltpid == 108 || TeamProxy.getInstance().MyTeamData.ltpid == 109 ||
                TeamProxy.getInstance().MyTeamData.ltpid == 110 || TeamProxy.getInstance().MyTeamData.ltpid == 111))
            {
                nummeb = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
                int t = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
                int i = 1;
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                {
                    switch (PlayerModel.getInstance().profession)
                    {
                        case 2: icon = icon2; break;
                        case 3: icon = icon3; break;
                        case 5: icon = icon5; break;
                        default:
                            break;
                    }
                    objects.FindChild("0/noEmpty/texts/icon/leader").GetComponent<Image>().sprite = icon;
                    objects.FindChild("0/noEmpty/btnRemoveTeam").gameObject.SetActive(false);
                    for (int k = 1; k < t; k++)
                    {
                        objects.FindChild(k + "/noEmpty/btnRemoveTeam").gameObject.SetActive(true);
                    }
                    for (int k = t; k < 5; k++)
                    {
                        objects.FindChild(k + "/empty/btnInvite").gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int k = 0; k < t; k++)
                    {
                        objects.FindChild(k + "/noEmpty/btnRemoveTeam").gameObject.SetActive(false);
                    }
                    for (int k = t; k < 5; k++)
                    {
                        objects.FindChild(k + "/empty/btnInvite").gameObject.SetActive(false);
                    }
                }
                if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count > 0)//当前队伍
                {
                    int t1 = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;

                    for (int x = 0; x < t1; x++)
                    {
                        objects.FindChild(x + "/noEmpty").gameObject.SetActive(true);
                        objects.FindChild(x + "/empty").gameObject.SetActive(false);
                    }
                    for (int x = t1; x < 5; x++)
                    {
                        objects.FindChild(x + "/noEmpty").gameObject.SetActive(false);
                        objects.FindChild(x + "/empty").gameObject.SetActive(true);
                    }
                }
                foreach (var item in TeamProxy.getInstance().MyTeamData.itemTeamDataList)
                {
                    switch (item.carr)
                    {
                        case 2: icon = icon2; break;
                        case 3: icon = icon3; break;
                        case 5: icon = icon5; break;
                        default:
                            break;
                    }
                    if (item.isCaptain)
                    {
                        objects.FindChild("0/noEmpty/iconCaptain").gameObject.SetActive(true);
                        objects.FindChild("0/noEmpty/texts/icon/leader").GetComponent<Image>().sprite = icon;
                        objects.FindChild("0/noEmpty/texts/txtName/Text").GetComponent<Text>().text = item.name;
                        objects.FindChild("0/noEmpty/texts/txtLvl/Text").GetComponent<Text>().text = item.zhuan + ContMgr.getCont("zhuan") + item.lvl + ContMgr.getCont("ji");
                        objects.FindChild("0/noEmpty/texts/txtCombat/Text").GetComponent<Text>().text = item.combpt.ToString();
                    }
                    else
                    {
                        if (i < t)
                        {
                            objects.FindChild(i + "/noEmpty/iconCaptain").gameObject.SetActive(false);
                            objects.FindChild(i + "/noEmpty/texts/icon/leader").GetComponent<Image>().sprite = icon;
                            objects.FindChild(i + "/noEmpty/texts/txtName/Text").GetComponent<Text>().text = item.name;
                            objects.FindChild(i + "/noEmpty/texts/txtLvl/Text").GetComponent<Text>().text = item.zhuan + ContMgr.getCont("zhuan") + item.lvl + ContMgr.getCont("ji");
                            objects.FindChild(i + "/noEmpty/texts/txtCombat/Text").GetComponent<Text>().text = item.combpt.ToString();
                            new BaseButton(objects.FindChild(i + "/noEmpty/btnRemoveTeam")).onClick = (GameObject go) =>
                              {
                                  TeamProxy.getInstance().SendKickOut(item.cid);
                                  flytxt.instance.fly(item.name + ContMgr.getCont("a3_counterpart_pleaseout"));
                              };
                            i++;
                        }
                    }
                }
            }
        }
        void cTeam()
        {
            changePos();
            if (TeamProxy.getInstance().MyTeamData != null)//有队伍
            {
                getGameObjectByPath("team").SetActive(false);
                //getGameObjectByPath("currentTeam").SetActive(true);
            }
            else//没有队伍
            {
                //getGameObjectByPath("currentTeam").SetActive(false);
                getGameObjectByPath("multiConDiff/gold").SetActive(false);
                getGameObjectByPath("team").SetActive(true);
            }
        }
        string str = "currentTeam/currentTeamPanel/right/bottom/btnStart";
        string str1 = "multiPlayer/contain/exp/enter/TextGo";
        string str2 = "multiPlayer/contain/gold/enter/TextGo";
        string str3 = "multiPlayer/contain/material/enter/TextGo";
        string str4 = "multiPlayer/contain/ghost/enter/Text";
        void butoText()
        {//暂时不改成统一写一起的，以防后面要分开不通用
            #region
            /*
            if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid == 108)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "开始";
                else
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "准备";
                getTransformByPath(str1).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str2).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str3).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str4).GetComponent<Text>().text = "我的队伍";
            }
            else if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid == 109)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "开始";
                else
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "准备";
                getTransformByPath(str2).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str1).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str3).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str4).GetComponent<Text>().text = "我的队伍";
            }
            else if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid == 110)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "开始";
                else
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "准备";
                getTransformByPath(str3).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str2).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str1).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str4).GetComponent<Text>().text = "我的队伍";
            }
            else if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.ltpid == 111)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "开始";
                else
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = "准备";
                getTransformByPath(str4).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str2).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str3).GetComponent<Text>().text = "我的队伍";
                getTransformByPath(str1).GetComponent<Text>().text = "我的队伍";
            }
            else
            {
                getTransformByPath(str1).GetComponent<Text>().text = "立即组队";
                getTransformByPath(str2).GetComponent<Text>().text = "立即组队";
                getTransformByPath(str3).GetComponent<Text>().text = "立即组队";
                getTransformByPath(str4).GetComponent<Text>().text = "立即组队";
            }*/
            #endregion
            if (TeamProxy.getInstance().MyTeamData != null)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_start");
                else
                    getTransformByPath(str).FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_zhunbei");
                //getTransformByPath(str4).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_myrank");
                //getTransformByPath(str2).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_myrank");
                //getTransformByPath(str3).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_myrank");
                //getTransformByPath(str1).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_myrank");
            }
            //else
            //{
            //    getTransformByPath(str1).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_zudui");
            //    getTransformByPath(str2).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_zudui");
            //    getTransformByPath(str3).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_zudui");
            //    getTransformByPath(str4).GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_zudui");
            //}
            getTransformByPath(str1).GetComponent<Text>().text = ContMgr.getCont("a3_couterpart_go");
            getTransformByPath(str2).GetComponent<Text>().text = ContMgr.getCont("a3_couterpart_go");
            getTransformByPath(str3).GetComponent<Text>().text = ContMgr.getCont("a3_couterpart_go");
            getTransformByPath(str4).GetComponent<Text>().text = ContMgr.getCont("a3_couterpart_go");
        }

        public override void onShowed()
        {
            _instance = this;
            Toclose = false;
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_RECOMMEND, commandList);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAMLISTINFO, onGetTeamListInfo);
            FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_FRIENDLIST, onFriendList);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);

            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, onNewMemberJoin);//新队员加入
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_KICKOUT, onKickOut);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CHANGECAPTAIN, changeCap);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, onCreatTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEHAVEMEMBERLEAVE, noticeHaveLeave);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, noticeInvite);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAMOBJECT_CHANGE, changeOBJ);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAM_READY, readyGo);

            this.transform.FindChild("multiPlayer/contain/exp/bg_awd").gameObject.SetActive(false);
            this.transform.FindChild("multiPlayer/contain/material/bg_awd").gameObject.SetActive(false);
            this.transform.FindChild("multiPlayer/contain/gold/bg_awd").gameObject.SetActive(false);

            refreshMoney();
            refreshGold();
            refreshGift();
            refreshGoldTimes();            
            refreshExpTimes();
            refreshGhostTimes();
            refreshMateTimes();
            CheckLock();
            OpenGold();
            OpenFSWZ();
            if (TeamProxy.getInstance().MyTeamData != null)//&& TeamProxy.getInstance().MyTeamData.ltpid == 108
            {
                butoText();
                changePos();
            }
            else if (TeamProxy.getInstance().MyTeamData == null)
            {
                changePos();
                butoText();
            }
            if (uiData != null)
            {
                if (uiData.Count == 1)
                {
                    int id = (int)uiData[0];
                    changetab(id);
                }
                else if (uiData.Count == 2)
                {
                    uint type = (uint)uiData[0];
                    object edata = uiData[1];
                    TeamProxy.getInstance().dispatchEvent(GameEvent.Create(type, TeamProxy.getInstance(), edata));
                }

            }
            if (TeamProxy.getInstance().MyTeamData != null)
            {
                nummeb = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
                //getGameObjectByPath("ready").SetActive(false);
            }
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        bool Toclose = false;
        public override void onClosed()
        {
            refreshGoldTimes();
            refreshExpTimes();
            refreshGhostTimes();
            refreshMateTimes();
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_RECOMMEND, commandList);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_TEAMLISTINFO, onGetTeamListInfo);
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_FRIENDLIST, onFriendList);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, onNewMemberJoin);//新队员加入
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_KICKOUT, onKickOut);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CHANGECAPTAIN, changeCap);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CREATETEAM, onCreatTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NOTICEHAVEMEMBERLEAVE, noticeHaveLeave);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_AFFIRMINVITE, noticeInvite);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_TEAMOBJECT_CHANGE, changeOBJ);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_TEAM_READY, readyGo);


            Transform show = getTransformByPath("ready/yesorno/show");
            for (int i = 1; i < show.childCount; i++)
            {
                if (show.GetChild(i).transform.FindChild("yes").gameObject.activeSelf == true)
                {
                    show.GetChild(i).transform.FindChild("yes").gameObject.SetActive(false);
                    show.GetChild(i).transform.FindChild("name").GetComponent<Text>().text = string.Empty;
                }
            }
            getGameObjectByPath("currentTeam").SetActive(false);
            needTrues = false;
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
            getTransformByPath("ready").gameObject.SetActive(false);
            instance = null;
        }

        #endregion
        void Update()
        {          
            if (choicePanel != null && choicePanel.activeSelf == true)
            {
                if (a3_counterpart_gold.open == true || a3_counterpart_exp.open == true || a3_counterpart_mate.open == true ||
                    a3_counterpart_multi_gold.open == true || a3_counterpart_multi_exp.open == true ||
                    a3_counterpart_multi_mate.open == true || a3_counterpart_multi_ghost.open == true)
                {
                    choicePanel.SetActive(false);
                }
            }
            if (choicePanel != null && choicePanel.activeSelf == false)
            {
                a3_counterpart_gold.open = false;
                a3_counterpart_exp.open = false;
                a3_counterpart_mate.open = false;
                a3_counterpart_multi_gold.open = false;
                a3_counterpart_multi_exp.open = false;
                a3_counterpart_multi_mate.open = false;
                a3_counterpart_multi_ghost.open = false;
            }
            if (tenSlider != null && tenSlider.localScale.x == 1)
            {
                tenSlider.localScale = new Vector3(0, 1, 1);
                if (getTransformByPath("ready").gameObject.activeSelf)
                {
                    TeamProxy.getInstance().SendReady(false, readyLtpid, readyLdiff);
                    sendfalse(false);
                }
            }
        }
        #region  调用少
        public void ontab(TabControl t)
        {
            int idx = t.getSeletedIndex();
            changetab(idx);
        }
        void changetab(int idx)
        {
            if (idx == 0)
            {
                getGameObjectByPath("tabs/single").GetComponent<Button>().interactable = true;
                getGameObjectByPath("tabs/multiplayer").GetComponent<Button>().interactable = false;
                multiPlayer.SetActive(false);
                single.SetActive(true);
                right1.gameObject.SetActive(false);
                left1.gameObject.SetActive(false);
                right.gameObject.SetActive(true);
                left.gameObject.SetActive(true);
                //flytxt.instance.fly("单人副本模式");
            }
            else
            {
                getGameObjectByPath("tabs/single").GetComponent<Button>().interactable = false;
                getGameObjectByPath("tabs/multiplayer").GetComponent<Button>().interactable = true;
                this.transform.FindChild("multiPlayer/contain/exp/bg_awd").gameObject.SetActive(false);
                this.transform.FindChild("multiPlayer/contain/material/bg_awd").gameObject.SetActive(false);
                this.transform.FindChild("multiPlayer/contain/gold/bg_awd").gameObject.SetActive(false);
                single.SetActive(false);
                multiPlayer.SetActive(true);
                right.gameObject.SetActive(false);
                left.gameObject.SetActive(false);
                right1.gameObject.SetActive(true);
                left1.gameObject.SetActive(true);
                //flytxt.instance.fly("组队副本模式");
            }
        }
        //金币副本
        public void refreshGoldTimes()
        {

            #region 单人


            //vip额外可购买次数
            int vip_count_g= A3_VipModel.getInstance().goldFb_count;


            //当日固定次数：
            Variant data = SvrLevelConfig.instacne.get_level_data(102);
            int thisday_count_g = data["daily_cnt"];


            //vip购买的次数
            int vip_buy_g =0;
            if (MapModel.getInstance().dFbDta.ContainsKey(102))
            {
                vip_buy_g = MapModel.getInstance().dFbDta[102].vip_buycount;
            }

            //vip额外可购买剩余次数
            int vip_count_over_g = vip_count_g - vip_buy_g >= 0 ? vip_count_g - vip_buy_g : 0;

            //总次数包括购买的
            int all_count_g = thisday_count_g + vip_buy_g;

            //进入次数：
            int goin_count_g = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(102))
            {
                goin_count_g = Mathf.Min(MapModel.getInstance().dFbDta[102].cycleCount, all_count_g/*+ A3_VipModel.getInstance().goldFb_count*/);
            }

            getTransformByPath("scroll_view_fuben/contain/gold/cue/limit").GetComponent<Text>().text 
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { ((all_count_g - goin_count_g)).ToString(), (all_count_g).ToString() });          
            getTransformByPath("scroll_view_fuben/contain/gold/cue/limit_vip").GetComponent<Text>().text 
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_g.ToString() });


            int price = SvrLevelConfig.instacne.get_level_data(102)["vip_cost"];
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/gold/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_g, price,102); };

            enterbtn1.interactable = all_count_g - goin_count_g<=0?false:true;
            #endregion



            #region 组队
            //vip额外可购买次数：
            int vip_count_g1 = A3_VipModel.getInstance().team_fb_1;

            //vip购买的次数
            int vip_buy_g1 = 0;
            if(MapModel.getInstance().dFbDta.ContainsKey(109))
            {
                vip_buy_g1 = MapModel.getInstance().dFbDta[109].vip_buycount;
            }

            //当日固定次数
            Variant data1 = SvrLevelConfig.instacne.get_level_data(109);
            int thisday_count_g1 = data1["daily_cnt"];

            //vip额外可购买剩余次数
            int vip_count_over_g1 = vip_count_g1 - vip_buy_g1 >= 0 ? vip_count_g1 - vip_buy_g1 : 0;

            //总次数包括购买的
            int all_count_g1 = thisday_count_g1 + vip_buy_g1;

            //进入次数：
            int goin_count_g1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(109))
            {
                goin_count_g1 = Mathf.Min(MapModel.getInstance().dFbDta[109].cycleCount, all_count_g1);
            }

            getTransformByPath("multiPlayer/contain/gold/cue/limit").GetComponent<Text>().text 
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (all_count_g1 - goin_count_g1).ToString(), all_count_g1.ToString() });
            getTransformByPath("multiPlayer/contain/gold/cue/limit_vip").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_g1.ToString() });

            int price1 = SvrLevelConfig.instacne.get_level_data(109)["vip_cost"];
            new BaseButton(getTransformByPath("multiPlayer/contain/gold/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_g1, price1,109); };

            enterbtn4.interactable = all_count_g1 - goin_count_g1 <= 0? false:true;
            #endregion

        }
        //经验副本
        public void refreshExpTimes()
        {

            #region 单人
            //vip额外可购买次数
            int vip_count_e = A3_VipModel.getInstance().goldFb_count;

            //vip购买的次数
            int vip_buy_e = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(101))
            {
                vip_buy_e = MapModel.getInstance().dFbDta[101].vip_buycount;
            }

            //当日固定次数：
            Variant data = SvrLevelConfig.instacne.get_level_data(101);
            int thisday_count_e = data["daily_cnt"];

            //vip额外可购买剩余次数
            int vip_count_over_e = vip_count_e - vip_buy_e >= 0 ? vip_count_e - vip_buy_e : 0;

            //总次数包括购买的
            int all_count_e = thisday_count_e + vip_buy_e;


            //进入次数：
            int goin_count_e = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(101))
            {
                goin_count_e = Mathf.Min(MapModel.getInstance().dFbDta[101].cycleCount, all_count_e/*+ A3_VipModel.getInstance().goldFb_count*/);
            }

            getTransformByPath("scroll_view_fuben/contain/exp/cue/limit").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { ((all_count_e - goin_count_e)).ToString(), (all_count_e).ToString() });
            getTransformByPath("scroll_view_fuben/contain/exp/cue/limit_vip").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_e.ToString() });

            int price = SvrLevelConfig.instacne.get_level_data(101)["vip_cost"];
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/exp/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_e, price,101); };

            enterbtn2.interactable = all_count_e - goin_count_e <= 0 ? false : true;
            #endregion

            #region 组队
            //vip额外可购买次数：
            int vip_count_g1 = A3_VipModel.getInstance().team_fb_1;

            //vip购买的次数
            int vip_buy_g1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(108))
            {
                vip_buy_g1 = MapModel.getInstance().dFbDta[108].vip_buycount;
            }

            //当日固定次数
            Variant data1 = SvrLevelConfig.instacne.get_level_data(108);
            int thisday_count_g1 = data1["daily_cnt"];


            //vip额外可购买剩余次数
            int vip_count_over_g1 = vip_count_g1 - vip_buy_g1 >= 0 ? vip_count_g1 - vip_buy_g1 : 0;

            //总次数包括购买的
            int all_count_g1 = thisday_count_g1 + vip_buy_g1;

            //进入次数：
            int goin_count_g1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(108))
            {
                goin_count_g1 = Mathf.Min(MapModel.getInstance().dFbDta[108].cycleCount, all_count_g1);
            }

            getTransformByPath("multiPlayer/contain/exp/cue/limit").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (all_count_g1 - goin_count_g1).ToString(), all_count_g1.ToString() });
            getTransformByPath("multiPlayer/contain/exp/cue/limit_vip").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_g1.ToString() });

            int price1 = SvrLevelConfig.instacne.get_level_data(108)["vip_cost"];
            new BaseButton(getTransformByPath("multiPlayer/contain/exp/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_g1, price1,108); };

            enterbtn5.interactable = all_count_g1 - goin_count_g1 <= 0 ? false : true;
            #endregion
        }
        //材料副本
        public void refreshMateTimes()
        {

            #region 单人
            //vip额外可购买次数
            int vip_count_e = A3_VipModel.getInstance().goldFb_count;

            //vip购买的次数
            int vip_buy_e = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(103))
            {
                vip_buy_e = MapModel.getInstance().dFbDta[103].vip_buycount;
            }

            //当日固定次数：
            Variant data = SvrLevelConfig.instacne.get_level_data(103);
            int thisday_count_e = data["daily_cnt"];

            //vip额外可购买剩余次数
            int vip_count_over_e = vip_count_e - vip_buy_e >= 0 ? vip_count_e - vip_buy_e : 0;

            //总次数包括购买的
            int all_count_e = thisday_count_e + vip_buy_e;

            //进入次数：
            int goin_count_e = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(103))
            {
                goin_count_e = Mathf.Min(MapModel.getInstance().dFbDta[103].cycleCount, all_count_e/*+ A3_VipModel.getInstance().goldFb_count*/);
            }

            getTransformByPath("scroll_view_fuben/contain/material/cue/limit").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { ((all_count_e - goin_count_e)).ToString(), (all_count_e).ToString() });
            getTransformByPath("scroll_view_fuben/contain/material/cue/limit_vip").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_e.ToString() });

            int price = SvrLevelConfig.instacne.get_level_data(103)["vip_cost"];
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/material/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_e, price,103); };

            enterbtn3.interactable = all_count_e - goin_count_e <= 0 ? false : true;
            #endregion

            #region 组队
            //vip额外可购买次数：
            int vip_count_g1 = A3_VipModel.getInstance().team_fb_1;

            //vip购买的次数
            int vip_buy_g1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(110))
            {
                vip_buy_g1 = MapModel.getInstance().dFbDta[110].vip_buycount;
            }

            //当日固定次数
            Variant data1 = SvrLevelConfig.instacne.get_level_data(110);
            int thisday_count_g1 = data1["daily_cnt"];


            //vip额外可购买剩余次数
            int vip_count_over_g1 = vip_count_g1 - vip_buy_g1 >= 0 ? vip_count_g1 - vip_buy_g1 : 0;

            //总次数包括购买的
            int all_count_g1 = thisday_count_g1 + vip_buy_g1;

            //进入次数：
            int goin_count_g1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(110))
            {
                goin_count_g1 = Mathf.Min(MapModel.getInstance().dFbDta[110].cycleCount, all_count_g1);
            }

            getTransformByPath("multiPlayer/contain/material/cue/limit").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (all_count_g1 - goin_count_g1).ToString(), all_count_g1.ToString() });
            getTransformByPath("multiPlayer/contain/material/cue/limit_vip").GetComponent<Text>().text
                = ContMgr.getCont("a3_counterpart_shengyu_vip", new List<string> { vip_count_over_g1.ToString() });

            int price1 = SvrLevelConfig.instacne.get_level_data(110)["vip_cost"];
            new BaseButton(getTransformByPath("multiPlayer/contain/material/addnum")).onClick = (GameObject go) => { buycountBtn(vip_count_over_g1, price1,110); };

            enterbtn6.interactable = all_count_g1 - goin_count_g1 <= 0 ? false : true;
            #endregion

        }


        //购买次数按钮
        void buycountBtn(int shownum,int price,int map_id )
        {
            if(shownum<=0)
            {

                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_COUNTERPART);
                return;
            }
            else
            {
                MsgBoxMgr.getInstance().showConfirm(price+ContMgr.getCont("zuanshi_ci"), () =>
                     {
                         LevelProxy.getInstance().sendGet_lvl_cnt_info(5,map_id);
                     });
                    
            }
        }



        public void refreshGhostTimes()
        {
            Variant data1 = SvrLevelConfig.instacne.get_level_data(111);
            int max_times1 = data1["daily_cnt"];
            int use_times1 = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(111))
            {
                use_times1 = Mathf.Min(MapModel.getInstance().dFbDta[111].cycleCount, max_times1);
            }
            //getTransformByPath("multiConDiff/ghost/choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times1 - use_times1) + "/" + max_times1 + "</color>";
            getTransformByPath("multiConDiff/ghost/choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times1 - use_times1).ToString(), max_times1.ToString() });
            // getTransformByPath("multiPlayer/contain/ghost/cue/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times1 - use_times1) + "/" + max_times1 + "</color>";
            getTransformByPath("multiPlayer/contain/ghost/cue/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times1 - use_times1).ToString(), max_times1.ToString() });

            if (max_times1 - use_times1 <= 0)
            {
                enterbtn7.interactable = false;
            }
            else
                enterbtn7.interactable = true;
        }
        public void refreshMoney()
        {
            Text money = transform.FindChild("jingbi/image/num").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("zuanshi/image/num").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("bangzuan/image/num").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
        }
        public void CheckLock()
        {
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.TEAMPART))
            {
                getGameObjectByPath("lock").SetActive(false);
            }
            else
                OpenFB();
            if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(108), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(108)))
            {
                transform.Find("multiPlayer/contain/exp/enter/TextGo").gameObject.SetActive(false);
                transform.Find("multiPlayer/contain/exp/enter/Level").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/exp/enter/Level").GetComponent<Text>().text
                    = a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(108) +
                    ContMgr.getCont("zhuan") +
                    a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(108) +
                    ContMgr.getCont("ji");
            }
            else
            {
                transform.Find("multiPlayer/contain/exp/enter/TextGo").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/exp/enter/Level").gameObject.SetActive(false);
            }
            if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(109), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(109)))
            {
                transform.Find("multiPlayer/contain/gold/enter/TextGo").gameObject.SetActive(false);
                transform.Find("multiPlayer/contain/gold/enter/Level").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/gold/enter/Level").GetComponent<Text>().text
                    = a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(109) +
                    ContMgr.getCont("zhuan") +
                    a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(109) +
                    ContMgr.getCont("ji");
            }
            else
            {
                transform.Find("multiPlayer/contain/gold/enter/TextGo").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/gold/enter/Level").gameObject.SetActive(false);
            }
            if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(110), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(110)))
            {
                transform.Find("multiPlayer/contain/material/enter/TextGo").gameObject.SetActive(false);
                transform.Find("multiPlayer/contain/material/enter/Level").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/material/enter/Level").GetComponent<Text>().text
                    = a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(110) +
                    ContMgr.getCont("zhuan") +
                    a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(110) +
                    ContMgr.getCont("ji");
            }
            else
            {
                transform.Find("multiPlayer/contain/material/enter/TextGo").gameObject.SetActive(true);
                transform.Find("multiPlayer/contain/material/enter/Level").gameObject.SetActive(false);
            }
        }
        public void OpenFB()
        {
            getGameObjectByPath("lock").SetActive(true);
            getGameObjectByPath("scroll_view_fuben/contain/material/lock").SetActive(true);
            new BaseButton(getTransformByPath("lock")).onClick = (GameObject go) =>
            {
                flytxt.instance.fly(ContMgr.getCont("func_limit_32"));
            };
            new BaseButton(getTransformByPath("scroll_view_fuben/contain/material/lock")).onClick = (GameObject go) =>
            {
                flytxt.instance.fly(ContMgr.getCont("func_limit_11"));
            };
        }
        public void OpenFSWZ()
        {
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.WIND_THRONE))
            {
                getGameObjectByPath("scroll_view_fuben/contain/material/lock").SetActive(true);
                new BaseButton(getTransformByPath("scroll_view_fuben/contain/material/lock")).onClick = (GameObject go) =>
                {
                    flytxt.instance.fly(ContMgr.getCont("func_limit_11"));
                };
            }
            else
                getGameObjectByPath("scroll_view_fuben/contain/material/lock").SetActive(false);
        }
        public void OpenGold()
        {
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.GOLD_DUNGEON))
            {
                getGameObjectByPath("scroll_view_fuben/contain/gold/lock").SetActive(true);
                new BaseButton(getTransformByPath("scroll_view_fuben/contain/gold/lock")).onClick = (GameObject go) =>
                {
                    flytxt.instance.fly(ContMgr.getCont("func_limit_13"));
                };
            }
            else
                getGameObjectByPath("scroll_view_fuben/contain/gold/lock").SetActive(false);
        }
        #endregion

        void onBtnCreateClick(GameObject go)
        {
            int m_tpid = 0;
            switch (mode)
            {
                case 1: m_tpid = 109; break;
                case 2: m_tpid = 108; break;
                case 3: m_tpid = 110; break;
                case 4: m_tpid = 111; break;
                default:
                    break;
            }
            if (TeamProxy.getInstance().MyTeamData != null)
                TeamProxy.getInstance().sendobject_change(m_tpid);
            else
                TeamProxy.getInstance().SendCreateTeam(m_tpid);
        }
        void onBtnQuitClick(GameObject go)
        {
            for (int i = 0; i < 5; i++)
            {
                objects.FindChild(i + "/noEmpty").gameObject.SetActive(false);
                objects.FindChild(i + "/empty").gameObject.SetActive(true);
            }
            leave = true;
            if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count == 1)
            {

                uint teamId = TeamProxy.getInstance().MyTeamData.teamId;
                TeamProxy.getInstance().SendDissolve(teamId);
            }
            else
            {
                TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().cid);
            }
            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);
        }

        void listOnline()
        {
            Transform tf = transform.FindChild("yaoqing/itemReserFriend");
            List<itemFriendData> re = FriendProxy.getInstance().RecommendDataList;
            Transform root;
            RectTransform cont;
            cont = getTransformByPath("yaoqing/main/scroll/containts").GetComponent<RectTransform>();
            cont.sizeDelta = new Vector2(cont.sizeDelta.x, re.Count * 60f);
            foreach (var item in re)
            {
                GameObject gob = GameObject.Instantiate(tf.gameObject) as GameObject;
                root = gob.transform;
                gob.SetActive(true);
                root.SetParent(getTransformByPath("yaoqing/main/scroll/containts"));
                txtName = root.transform.FindChild("texts/txtNickName").GetComponent<Text>();
                txtLvl = root.transform.FindChild("texts/txtLevel").GetComponent<Text>();
                txtPro = root.transform.FindChild("texts/txtProfessional").GetComponent<Text>();
                set(item);
                gob.transform.localScale = Vector3.one;
                toggle = root.transform.FindChild("Toggle").GetComponent<Toggle>();
                if (ltemObjList.ContainsKey(item.cid))
                {

                }
                else
                    ltemObjList.Add(item.cid, item.name);
            }
        }
        void set(itemFriendData item)
        {
            txtName.text = item.name;
            txtLvl.text = item.zhuan + ContMgr.getCont("zhuan") + item.lvl + ContMgr.getCont("ji");
            switch (item.carr)
            {
                case 2: txtPro.text = ContMgr.getCont("comm_job2"); break;
                case 3: txtPro.text = ContMgr.getCont("comm_job3"); break;
                case 5: txtPro.text = ContMgr.getCont("comm_job5"); break;
                default:
                    break;
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
    }
    #region  class
    class a3_counterpart_gold : a3BaseActive
    {
        public static bool open = false;
        public static a3_counterpart_gold insatnce;
        public a3_counterpart_gold(Window win, string pathStr) : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;
        public static int diff;
        //int difflvls = 1;  ??
        public override void init()
        {
            refreshGold();
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            //========================
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();




            //进入副本
            enterbtn.onClick = (GameObject go) =>
            {
                difficulty_change(1);


            };
            enterbtn1.onClick = (GameObject go) =>
            {
                difficulty_change(2);

            };
            enterbtn2.onClick = (GameObject go) =>
            {
                difficulty_change(3);
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                difficulty_change(4);
            };

        }

        void difficulty_change(int difflvlss)
        {
            //int in_count = MapModel.getInstance().dFbDta.ContainsKey(102) ? MapModel.getInstance().dFbDta[102].cycleCount : 0;//进入次数
            //int thiday_count = SvrLevelConfig.instacne.get_level_data(102)["daily_cnt"];//当日固定次数
            //int vip_count = A3_VipModel.getInstance().goldFb_count;//vip额外次数
            //int max_count = thiday_count + A3_VipModel.getInstance().goldFb_count;//总次数
            //difflvls = difflvlss;
            //if(thiday_count-in_count>0)
            //{
                openvip_cnt_gold(difflvlss);
               // return;
            //}
            //if (A3_VipModel.getInstance().goldFb_count <= 0)
            //{
            //    flytxt.instance.fly(ContMgr.getCont("vipfbaddcount0"));
            //}
            //else
            //{
            //    if (max_count - in_count <= 0)
            //    {
            //        flytxt.instance.fly(ContMgr.getCont("vipfbaddcount1"));
            //    }
            //    else
            //    {
            //        MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("vipfbaddcount2") + SvrLevelConfig.instacne.get_level_data(102)["vip_cost"] + ContMgr.getCont("vipfbaddcount3") + "\n" + ContMgr.getCont("vipfbaddcount4") + "(" + (vip_count - (in_count- thiday_count))+"/" + vip_count + ")", () =>
            //        {
            //            openvip_cnt_gold(difflvls);
            //        });
            //    }

            //}
        }


        void openvip_cnt_gold(int difflvl)
        {
           

            if (a3_active.MwlrIsDoing)
            {
                ArrayList ar = new ArrayList();
                ar.Add(fb_type.gold);
                ar.Add(difflvl);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ar);
            }
            else
            {
                BtnsOnclick(difflvl);
            }
        }

       

       public static  void BtnsOnclick(int difflvl)
        {

            open = true;
            if (true)
            {
                Variant sendData = new Variant();
                sendData["mapid"] = 3335;
                sendData["npcid"] = 0;
                sendData["ltpid"] = 102;
                sendData["diff_lvl"] = difflvl;
                diff = sendData["diff_lvl"];
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }


        }

        void refreshGold()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(102);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(102))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[102].cycleCount, max_times);
            }
            //getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string>{ (max_times - use_times).ToString(), max_times.ToString() });
        }
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(102);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text =ContMgr.getCont("a3_counterpart_openforlock",new List<string>{ zhaun.ToString(),ji.ToString() });
                enterbtn1.interactable = false;

            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        public override void onShowed()
        {
            insatnce = this;
            base.onShowed();
            changeSth();
        }
    }

    class a3_counterpart_exp : a3BaseActive
    {
        public static bool open = false;
        public static int diff;
        public static a3_counterpart_exp instance;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;
        public a3_counterpart_exp(Window win, string pathStr) : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        public static int diff_lvl = 1;//困难度
        //int difflvls = 1;
        public override void init()
        {
            refreshExp();
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;

            //进入副本
            changeSth();

            enterbtn.onClick = (GameObject go) =>
            {

                difficulty_change(1);
            };
            enterbtn1.onClick = (GameObject go) =>
            {

                difficulty_change(2);
            };
            enterbtn2.onClick = (GameObject go) =>
            {

                difficulty_change(3);
            };
            enterbtn3.onClick = (GameObject go) =>
            {

                difficulty_change(4);
            };

        }
        void difficulty_change(int difflvlss)
        {
            //int in_count =MapModel.getInstance().dFbDta.ContainsKey(101) ? MapModel.getInstance().dFbDta[101].cycleCount : 0;//进入次数
            //int thiday_count = SvrLevelConfig.instacne.get_level_data(101)["daily_cnt"];//当日固定次数
            //int vip_count = A3_VipModel.getInstance().expFb_count;//vip额外次数
            //int max_count = thiday_count + A3_VipModel.getInstance().expFb_count;//总次数
            //difflvls = difflvlss;
            //if (thiday_count - in_count > 0)
            //{
                openvip_cnt_exp(difflvlss);
            //    return;
            //}
            //if (A3_VipModel.getInstance().expFb_count <= 0)
            //{
            //    flytxt.instance.fly(ContMgr.getCont("vipfbaddcount0"));
            //}
            //else
            //{
            //    if (max_count - in_count <= 0)
            //    {
            //        flytxt.instance.fly(ContMgr.getCont("vipfbaddcount1"));
            //    }
            //    else
            //    {
            //        MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("vipfbaddcount2") + SvrLevelConfig.instacne.get_level_data(101)["vip_cost"] + ContMgr.getCont("vipfbaddcount3") + "\n" + ContMgr.getCont("vipfbaddcount4") + "(" + (vip_count - (in_count - thiday_count)) + "/" + vip_count + ")", () =>
            //        {
            //            openvip_cnt_exp(difflvls);
            //        });
            //    }

            //}
        }
        void openvip_cnt_exp(int difflvl)
        {

            if (a3_active.MwlrIsDoing)
            {
                ArrayList ar = new ArrayList();
                ar.Add(fb_type.exp);
                ar.Add(difflvl);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ar);
            }
            else
            {
                BtnsOnclick(difflvl);
            }
        }
        public  static  void BtnsOnclick(int difflvl)
        {

            open = true;
            if (true)
            {
                Variant sendData = new Variant();
                sendData["mapid"] = 3334;
                sendData["npcid"] = 0;
                sendData["ltpid"] = 101;
                sendData["diff_lvl"] = difflvl;
                diff_lvl = difflvl;
                diff = sendData["diff_lvl"];
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }

        }




        void refreshExp()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(101);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(101))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[101].cycleCount, max_times);
            }
            //getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        }
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(101);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        public override void onShowed()
        {
            instance = this;
            base.onShowed();
            changeSth();
        }
    }

    class a3_counterpart_mate : a3BaseActive
    {
        public static bool open = false;
        public a3_counterpart_mate(Window win, string pathStr) : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;
        public static int diff;
        public static a3_counterpart_mate instance;
        //int difflvls = 1;
        public override void init()
        {
            refreshMate();
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            //==============
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();
            //进入副本
            enterbtn.onClick = (GameObject go) =>
            {
                difficulty_change(1);
            };
            enterbtn1.onClick = (GameObject go) =>
            {
                difficulty_change(2);
            };
            enterbtn2.onClick = (GameObject go) =>
            {
                difficulty_change(3);
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                difficulty_change(4);
            };
        }

        void difficulty_change(int difflvlss)
        {
            //int in_count = MapModel.getInstance().dFbDta.ContainsKey(103) ? MapModel.getInstance().dFbDta[103].cycleCount : 0;//进入次数
            //int thiday_count = SvrLevelConfig.instacne.get_level_data(103)["daily_cnt"];//当日固定次数
            //int vip_count= A3_VipModel.getInstance().materialFb_count;//vip额外次数
            //int max_count = thiday_count + A3_VipModel.getInstance().materialFb_count;//总次数
            //difflvls = difflvlss;
            //if (thiday_count - in_count > 0)
            //{
                openvip_cnt_matep(difflvlss);
            //    return;
            //}
            //if (A3_VipModel.getInstance().materialFb_count <= 0)
            //{
            //    flytxt.instance.fly(ContMgr.getCont("vipfbaddcount0"));
            //}
            //else
            //{
            //    if (max_count - in_count <= 0)
            //    {
            //        flytxt.instance.fly(ContMgr.getCont("vipfbaddcount1"));
            //    }
            //    else
            //    {
            //        MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("vipfbaddcount2") + SvrLevelConfig.instacne.get_level_data(103)["vip_cost"] + ContMgr.getCont("vipfbaddcount3") + "\n" + ContMgr.getCont("vipfbaddcount4") + "(" + (vip_count - (in_count - thiday_count)) + "/" + vip_count + ")", () =>
            //        {
            //            openvip_cnt_matep(difflvls);
            //        });
            //    }

            //}
        }
        void openvip_cnt_matep(int difflvl)
        {

            if (a3_active.MwlrIsDoing)
            {
                ArrayList ar = new ArrayList();
                ar.Add(fb_type.mate);
                ar.Add(difflvl);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ar);
            }
            else
            {
                BtnsOnclick(difflvl);
            }
        }
        public static void BtnsOnclick(int difflvl)
        {
            open = true;
            if (true)
            {
                Variant sendData = new Variant();
                sendData["mapid"] = 3339;
                sendData["npcid"] = 0;
                sendData["ltpid"] = 103;
                sendData["diff_lvl"] = difflvl;
                diff = sendData["diff_lvl"];
                LevelProxy.getInstance().sendCreate_lvl(sendData);
            }


        }

        void refreshMate()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(103);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(103))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[103].cycleCount, max_times);
            }
            //getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        }
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(103);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        public override void onShowed()
        {
            instance = this;
            changeSth();
        }
    }

    /// <summary>
    /// 组队109副本
    /// </summary>
    class a3_counterpart_multi_gold : a3BaseActive
    {
        public static bool open = false;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;
        public a3_counterpart_multi_gold(Window win, string pathStr) : base(win, pathStr)
        {
        }
        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        public override void init()
        {
            refreshExp();
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();
            //=======================================
            enterbtn.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 109;
                    sendData["diff_lvl"] = 1;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(1);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 109, 1);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text =ContMgr.getCont("a3_counterpart_xtype0");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                   a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true);InterfaceMgr.getInstance().closeAllWin(new List<string> { InterfaceMgr.A3_COUNTERPART ,InterfaceMgr.A3_FB_TEAM });
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn1.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 109;
                    sendData["diff_lvl"] = 2;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(2);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 109, 2);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtyp1");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;

                }
            };
            enterbtn2.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 109;
                    sendData["diff_lvl"] = 3;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(3);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 109, 3);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype2");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 109;
                    sendData["diff_lvl"] = 4;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(4);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 109, 4);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype3");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
        }
        void refreshExp()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(109);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(109))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[109].cycleCount, max_times);
            }
           // getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        }
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(109);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        bool canINfb()
        {
            int haveNumCant = 0;
            a3_counterpart.instance.kout.Clear();
            a3_counterpart.instance.canin.Clear();
            for (int i = 0; i < a3_counterpart.nummeb; i++)
            {
                int curlvl = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].lvl;
                int curzhuan = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].zhuan;
                if (zhaun > curzhuan || (zhaun == curzhuan && curlvl < ji))
                {
                    haveNumCant++;
                    a3_counterpart.instance.kout.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                    a3_counterpart.instance.canin.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                }
            }
            if (haveNumCant > 0)
            {
                //a3_counterpart.instance.peopleLess.FindChild("yesorno/Text").GetComponent<Text>().text = "队伍中有" + haveNumCant + "个玩家等级不足，是否移除他们。";
                a3_counterpart.instance.peopleLess.FindChild("yesorno/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_depeople", new List<string> { haveNumCant.ToString()});
                a3_counterpart.instance.peopleLess.gameObject.SetActive(true);
                a3_counterpart.instance.canin.Clear();
                return false;
            }
            else
                return true;
        }
        public override void onShowed()
        {
            changeSth();
        }
    }

    /// <summary>
    /// 组队108副本
    /// </summary>
    class a3_counterpart_multi_exp : a3BaseActive
    {
        public a3_counterpart_multi_exp(Window win, string pathStr) : base(win, pathStr) { }
        public static bool open = false;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;

        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        public override void init()
        {
            refreshExp();
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();
            //=======================================
            enterbtn.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 108;
                    sendData["diff_lvl"] = 1;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData);
                    return;
                }
                zhaunJi(1);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 108, 1);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype4");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn1.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 108;
                    sendData["diff_lvl"] = 2;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData);
                    return;
                }
                zhaunJi(2);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 108, 2);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype5");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;

                }
            };
            enterbtn2.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 108;
                    sendData["diff_lvl"] = 3;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(3);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 108, 3);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype6");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 108;
                    sendData["diff_lvl"] = 4;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(4);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 108, 4);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype7");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
        }
        void refreshExp()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(108);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(108))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[108].cycleCount, max_times);
            }
           // getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        
      }
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(108);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        bool canINfb()
        {
            int haveNumCant = 0;
            a3_counterpart.instance.kout.Clear();
            a3_counterpart.instance.canin.Clear();
            for (int i = 0; i < a3_counterpart.nummeb; i++)
            {
                int curlvl = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].lvl;
                int curzhuan = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].zhuan;
                if (zhaun > curzhuan || (zhaun == curzhuan && curlvl < ji))
                {
                    haveNumCant++;
                    a3_counterpart.instance.kout.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                    a3_counterpart.instance.canin.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                }
            }
            if (haveNumCant > 0)
            {
                a3_counterpart.instance.peopleLess.FindChild("yesorno/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_depeople", new List<string> { haveNumCant.ToString() });
                a3_counterpart.instance.peopleLess.gameObject.SetActive(true);
                a3_counterpart.instance.canin.Clear();
                return false;
            }
            else
                return true;
        }
        public override void onShowed()
        {
            changeSth();
        }
    }

    /// <summary>
    /// 组队110副本
    /// </summary>
    class a3_counterpart_multi_mate : a3BaseActive
    {
        public a3_counterpart_multi_mate(Window win, string pathStr) : base(win, pathStr)
        {
        }
        public static bool open = false;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;

        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        public override void init()
        {
            refreshExp();
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();
            //=======================================
            enterbtn.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 110;
                    sendData["diff_lvl"] = 1;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(1);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 110, 1);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype8");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn1.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 110;
                    sendData["diff_lvl"] = 2;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(2);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 110, 2);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype9");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;

                }
            };
            enterbtn2.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 110;
                    sendData["diff_lvl"] = 3;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(3);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 110, 3);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype10");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 110;
                    sendData["diff_lvl"] = 4;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(4);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 110, 4);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype11");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
        }
        void refreshExp()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(110);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(110))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[110].cycleCount, max_times);
            }
           // getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        }
  
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(110);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        bool canINfb()
        {
            int haveNumCant = 0;
            a3_counterpart.instance.kout.Clear();
            a3_counterpart.instance.canin.Clear();
            for (int i = 0; i < a3_counterpart.nummeb; i++)
            {
                int curlvl = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].lvl;
                int curzhuan = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].zhuan;
                if (zhaun > curzhuan || (zhaun == curzhuan && curlvl < ji))
                {
                    haveNumCant++;
                    a3_counterpart.instance.kout.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                    a3_counterpart.instance.canin.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                }
            }
            if (haveNumCant > 0)
            {
                a3_counterpart.instance.peopleLess.FindChild("yesorno/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_depeople", new List<string> { haveNumCant.ToString() });
                a3_counterpart.instance.peopleLess.gameObject.SetActive(true);
                a3_counterpart.instance.canin.Clear();
                return false;
            }
            else
                return true;
        }
        public override void onShowed()
        {
            changeSth();
        }
    }
    /// <summary>
    /// 组队111副本
    /// </summary>
    class a3_counterpart_multi_ghost : a3BaseActive
    {
        public a3_counterpart_multi_ghost(Window win, string pathStr) : base(win, pathStr)
        {
        }
        public static bool open = false;
        int zhaun = 0;
        int ji = 0;
        int pzhuan = 0;
        int pji = 0;

        BaseButton enterbtn;
        BaseButton enterbtn1;
        BaseButton enterbtn2;
        BaseButton enterbtn3;
        public override void init()
        {
            refreshExp();
            //进入副本
            enterbtn = new BaseButton(getTransformByPath("choiceDef/easy"));
            enterbtn1 = new BaseButton(getTransformByPath("choiceDef/normal"));
            enterbtn2 = new BaseButton(getTransformByPath("choiceDef/deffi"));
            enterbtn3 = new BaseButton(getTransformByPath("choiceDef/god"));
            pzhuan = (int)PlayerModel.getInstance().up_lvl;
            pji = (int)PlayerModel.getInstance().lvl;
            changeSth();
            //=======================================
            enterbtn.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 111;
                    sendData["diff_lvl"] = 1;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(1);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 111, 1);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype12");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn1.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 111;
                    sendData["diff_lvl"] = 2;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(2);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 111, 2);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype13");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;

                }
            };
            enterbtn2.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 111;
                    sendData["diff_lvl"] = 3;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(3);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 111, 3);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype14");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
            enterbtn3.onClick = (GameObject go) =>
            {
                gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 2)
                {
                    //flytxt.instance.fly("队伍人数少于2人"); return;
                    Variant sendData = new Variant();
                    sendData["npcid"] = 0;
                    sendData["ltpid"] = 111;
                    sendData["diff_lvl"] = 4;
                    a3_counterpart.lvl = sendData["diff_lvl"];
                    LevelProxy.getInstance().sendCreate_lvl(sendData); return;
                }
                zhaunJi(4);
                open = true;
                if (canINfb())
                {
                    TeamProxy.getInstance().SendReady(true, 111, 4);
                    a3_counterpart.instance.getTransformByPath("ready/yesorno/Text/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_xtype15");
                    a3_counterpart.instance.tenSen();
                    if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                    {
                        a3_counterpart.instance.getTransformByPath("ready/yesorno/show/0/name").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_mine");
                    }
                    //a3_counterpart.instance.getGameObjectByPath("currentTeam").SetActive(true);
                    a3_counterpart.instance.getGameObjectByPath("ready").SetActive(true); InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.A3_COUNTERPART);
                    a3_counterpart.instance.getButtonByPath("ready/yesorno/yes").interactable = false;
                }
            };
        }
        void refreshExp()
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(111);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(111))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[111].cycleCount, max_times);
            }
           // getTransformByPath("choiceDef/limit").GetComponent<Text>().text = "剩余次数： <color=#00ff00>" + (max_times - use_times) + "/" + max_times + "</color>";
            getTransformByPath("choiceDef/limit").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_shengyu", new List<string> { (max_times - use_times).ToString(), max_times.ToString() });
        }
    
        void zhaunJi(int difs)
        {
            Variant data = SvrLevelConfig.instacne.get_level_data(111);
            zhaun = data["diff_lvl"][difs]["open_zhuan"];
            ji = data["diff_lvl"][difs]["open_level"];
        }
        void changeSth()
        {
            zhaunJi(2);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(true);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(false);
                getTransformByPath("choiceDef/normal/normalText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn1.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/normal/normalText").SetActive(false);
                getGameObjectByPath("choiceDef/normal/enter").SetActive(true);
                enterbtn1.interactable = true;
            }
            zhaunJi(3);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(true);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(false);
                getTransformByPath("choiceDef/deffi/deffiText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn2.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/deffi/deffiText").SetActive(false);
                getGameObjectByPath("choiceDef/deffi/enter").SetActive(true);
                enterbtn2.interactable = true;
            }
            zhaunJi(4);
            if (zhaun > pzhuan || (zhaun == pzhuan && pji < ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(true);
                getGameObjectByPath("choiceDef/god/enter").SetActive(false);
                getTransformByPath("choiceDef/god/godText").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_openforlock", new List<string> { zhaun.ToString(), ji.ToString() });
                enterbtn3.interactable = false;
            }
            else if (zhaun < pzhuan || (zhaun == pzhuan && pji >= ji))
            {
                getGameObjectByPath("choiceDef/god/godText").SetActive(false);
                getGameObjectByPath("choiceDef/god/enter").SetActive(true);
                enterbtn3.interactable = true;
            }
        }
        bool canINfb()
        {
            int haveNumCant = 0;
            a3_counterpart.instance.kout.Clear();
            a3_counterpart.instance.canin.Clear();
            for (int i = 0; i < a3_counterpart.nummeb; i++)
            {
                int curlvl = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].lvl;
                int curzhuan = (int)TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].zhuan;
                if (zhaun > curzhuan || (zhaun == curzhuan && curlvl < ji))
                {
                    haveNumCant++;
                    a3_counterpart.instance.kout.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                    a3_counterpart.instance.canin.Add(TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid);
                }
            }
            if (haveNumCant > 0)
            {
                a3_counterpart.instance.peopleLess.FindChild("yesorno/Text").GetComponent<Text>().text = ContMgr.getCont("a3_counterpart_depeople", new List<string> { haveNumCant.ToString() });
                a3_counterpart.instance.peopleLess.gameObject.SetActive(true);
                a3_counterpart.instance.canin.Clear();
                return false;
            }
            else
                return true;
        }
        public override void onShowed()
        {
            changeSth();
        }
    }

    #endregion
}
