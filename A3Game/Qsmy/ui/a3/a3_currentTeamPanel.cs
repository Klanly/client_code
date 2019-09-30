using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class a3_currentTeamPanel : BaseShejiao
    {
        public static a3_currentTeamPanel _instance;
        public a3_currentTeamPanel(Transform trans) : base(trans) { init(trans); }
        uint cid;
        List<ItemMemberObj> itemMemberObjList;

        Toggle togInvite;
        Toggle togJoin;
        public Material materialGrey;
        public Text txtTeambuff;
        public Dictionary<uint, Sprite> carrSpriteDic;
        public uint begin_index = 0;
        public uint end_index = 20;
        public static bool leave = false;
        public GameObject team_object;
        public Dropdown team_object_change;
        public int curTeamType = 0,curTeamDiff = 0;
        private ChatToType ctp;
        public void init(Transform trans)
        {
            _instance = this;
            inText();
            itemMemberObjList = new List<ItemMemberObj>();
            txtTeambuff = trans.FindChild("right/bottom/teambuff").GetComponent<Text>();
            Transform objects = trans.FindChild("right/main/body/contains");
            team_object = transform.FindChild("right/bottom/team_object/Dropdown").gameObject;
            team_object_change = team_object.GetComponent<Dropdown>();
            team_object_change.onValueChanged.AddListener(team_object_dropdownClick);


            team_object_change.captionText.text = ContMgr.getCont("a3_teamPanel_16");
            for (int i = 0; i < team_object_change.options.Count; i++)
            {
                team_object_change.options[i].text = ContMgr.getCont("a3_teamPanel_"+(i+16));
            }

            for (int i = 0; i < objects.childCount; i++)
            {
                Transform child = objects.GetChild(i);
                ItemMemberObj itemMemberObj = new ItemMemberObj(child);
                itemMemberObjList.Add(itemMemberObj);
            }
            togInvite = trans.FindChild("right/bottom/togInvite").GetComponent<Toggle>();
           /*暂时先屏蔽掉*/ //togJoin = trans.FindChild("right/bottom/togJoin").GetComponent<Toggle>();
            togInvite.onValueChanged.AddListener(onTogAgreenAddOtherClick);
            //togJoin.onValueChanged.AddListener(onTogAgreeOtherApplyClick);
            materialGrey = U3DAPI.U3DResLoad<Material>("uifx/uiGray");
            carrSpriteDic = new Dictionary<uint, Sprite>();

            for (int i = 0; i < 3; i++)
            {
                if (i == 0) { carrSpriteDic.Add(2, GAMEAPI.ABUI_LoadSprite("icon_team_warrior_team")); }
                if (i == 1) { carrSpriteDic.Add(3, GAMEAPI.ABUI_LoadSprite("icon_team_mage_team")); }
                if (i == 2) { carrSpriteDic.Add(5, GAMEAPI.ABUI_LoadSprite("icon_team_assassin_team")); }

            }
            BaseButton btnQuitTeam = new BaseButton(trans.FindChild("right/bottom/btnQuitTeam"));
            btnQuitTeam.onClick = onBtnQuitTeamClick;
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, onCreateTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, onAffirminvite);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, onNewMemberJoin);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_KICKOUT, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CHANGETEAMINFO, onChangeTeamInfo);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEHAVEMEMBERLEAVE, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEONLINESTATECHANGE, onNoticeOnlineStateChange);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CHANGECAPTAIN, onChangeCaptain);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAMOBJECT_CHANGE, onChangeTeamObject);
            if (TeamProxy.getInstance().MyTeamData != null)
                team_object_change.value = change_v((int)TeamProxy.getInstance().MyTeamData.ltpid, false);

            new BaseButton(transform.Find("right/main/body/btn_call")).onClick = OnClickMsg;
        }
        void inText()
        {
            this.transform.FindChild("right/bottom/inforTips").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_1");//*同仇敌忾:每位成员将为周围队友提供6%的攻击加成!
            this.transform.FindChild("right/bottom/btnQuitTeam/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_2");//退出队伍
            this.transform.FindChild("right/bottom/togInvite/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_3");//允许队友进行邀请
            this.transform.FindChild("right/bottom/togJoin/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_4");//申请直接进队
            this.transform.FindChild("right/bottom/team_object/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_5");//队伍目标：
            this.transform.FindChild("right/main/body/btn_call/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamPanel_6");//喊话
        }

        private void OnClickMsg(GameObject go)
        {
            if (!PlayerModel.getInstance().IsCaptain) { flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_not_captain")); return; }
            if (PlayerModel.getInstance().inFb) { flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_in_fb")); return; }
            a3_chatroom._instance.SendMsg(string.Format("{0}:{1}:{2}", TeamProxy.getInstance().MyTeamData.teamId, curTeamType,TeamProxy.getInstance().mapItemTeamData==null?0: TeamProxy.getInstance().mapItemTeamData.ldiff), chatType: ChatToType.World, xtp: 1);
        } 
        private void team_object_dropdownClick(int i)
        {

            if (!A3_TeamModel.getInstance().Limit_Change_Teammubiao(i))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_nofunction"));
                team_object_change.GetComponent<Dropdown>().value = 0;
                return;
            }
            if (TeamProxy.getInstance().MyTeamData != null)
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain == true)
                {
                    TeamProxy.getInstance().sendobject_change(change_v(i,true));
                }
                else
                {
                    string v="";
                    v = ContMgr.getCont("a3_currentTeamPanel_type" + i);
                    //switch (i)
                    //{
                    //    case 0:
                    //        v = "队伍目标为：自定义";
                    //        break;
                    //    case 1:
                    //        v = "队伍目标为：挂机";
                    //        break;
                    //    case 2:
                    //        v = "队伍目标为：托维尔墓穴";
                    //        break;
                    //    case 3:
                    //        v = "队伍目标为：兽灵秘境";
                    //        break;
                    //    case 4:
                    //        v = "队伍目标为：魔物猎人";
                    //        break;

                    //}
                    flytxt.instance.fly(v);
                }
            }
        }
        /// <summary>
        /// for case 10  确认申请 申请者会推送队伍的信息res=10 不会单独推确认信息 拒绝申请会推res=21
        /// </summary>
        /// <param name="itm"></param>
        public void Show(ItemTeamMemberData itm)
        {
           
            int itmCount = itm.itemTeamDataList.Count;
            for (int i = 0,j = 0; i < itmCount; i++,j++)
            {
                if (itemMemberObjList.Exists((item) => item.cid == itm.itemTeamDataList[i].cid))
                {
                    j--;
                    continue;
                }                
                itemMemberObjList[j].SetInfo(itm.itemTeamDataList[i], itm.meIsCaptain);
            }
            setTeamBuffTxt();
            for (int i = itmCount; i < 5; i++)//将剩余的位置设置为空,即只显示邀请按钮
            {
                if (itm.meIsCaptain || itm.membInv)
                {
                    itemMemberObjList[i].ClearInfo(true);
                }
                else
                {
                    itemMemberObjList[i].ClearInfo();
                }
            }

            this.gameObject.SetActive(true);
            a3_teamPanel._instance.gameObject.SetActive(false);
            if (itm.leaderCid == PlayerModel.getInstance().cid)
            {
                if (togInvite.isOn != itm.membInv) togInvite.isOn = itm.membInv;
                //if (togJoin.isOn != itm.dirJoin) togJoin.isOn = itm.dirJoin;

            }
            else
            {
                togInvite.gameObject.SetActive(false);
                //togJoin.gameObject.SetActive(false);
            }
            cid = itm.leaderCid;

          // team_object_change.value = change_v((int)itm.ltpid, false);
           // Debug.LogError(change_v((int)itm.ltpid, false));
            //Debug.LogError(itm.ltpid + "ssss" + itm.leaderCid + "ss" + itm.meIsCaptain + "ss" + itm.membInv);

        }

        public int change_v(int i,bool b)
        {
            int v = 0;
            if (b == true)
            {
                switch (i)
                {
                    case 0:
                        v = 0;//自定义
                        break;
                    case 1:
                        v = 1;//挂机
                        break;
                    case 2://托维尔墓穴
                        v = 108;
                        break;
                    case 3://驯龙者的末日
                        v = 109;
                        break;
                    case 4://托维尔墓穴
                        v = 110;
                        break;
                    case 5://兽灵秘境
                        v = 105;
                        break;
                    case 6://魔物猎人
                        v = 2;
                        break;
                }
            }
            else
            {
                switch (i)
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
            }
            return curTeamType = v;
        }
        private void onChangeTeamObject(GameEvent e)
        {
            Variant data = e.data;
            uint ltpid = data["ltpid"];
            team_object_change.value = change_v((int)ltpid,false);
        }
        private void setTeamBuffTxt()
        {
          
            int itmCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            if (itmCount >= 2)
            {
                //txtTeambuff.text = "同仇敌忾:攻击+" + itmCount * 6 + "%";
                txtTeambuff.text = ContMgr.getCont("a3_currentTeamPanel_add", new List<string>() { (itmCount * 6).ToString() });
            }
            else
            {
                txtTeambuff.text = ContMgr.getCont("a3_currentTeamPanel_add1");
            }

            if (TeamProxy.getInstance().MyTeamData != null)
            {
                               
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain == false)
                {
                    team_object_change.enabled = false;
                    team_object.transform.FindChild("Arrow").gameObject.SetActive(false);
                }
                else
                {
                    team_object_change.enabled = true;
                    team_object.transform.FindChild("Arrow").gameObject.SetActive(true);
                }
            }
        }

        void onCreateTeam(GameEvent e)
        {
            Variant data = e.data;
            uint teamId = data["teamid"];
            uint ltpid = data["ltpid"];

            ItemTeamData itd = new ItemTeamData();
            itd.name = PlayerModel.getInstance().name;
            itd.lvl = PlayerModel.getInstance().lvl;
            itd.knightage = PlayerModel.getInstance().clanid.ToString();
            itd.mapId = PlayerModel.getInstance().mapid;
            itd.MembCount = 1;
            itd.cid = PlayerModel.getInstance().cid;
            itd.zhuan = PlayerModel.getInstance().up_lvl;
            itd.combpt = PlayerModel.getInstance().combpt;
            itd.teamId = teamId;
            itd.ltpid = ltpid;
            itd.isCaptain = true;
            itd.showRemoveMemberBtn = false;
            itd.online = true;
            itd.carr = (uint)PlayerModel.getInstance().profession;
            cid = itd.cid;
            this.gameObject.SetActive(true);
            bool isCaptain = itd.isCaptain;
            for (int i = 0; i < itemMemberObjList.Count; i++)
            {
                if (i == 0)
                {
                    itemMemberObjList[i].SetInfo(itd, true);//将第一个设置为自己的位置,自己就是队长
                }
                else
                {
                    itemMemberObjList[i].ClearInfo(true);//将除了第一个的位置,其他位置都设置为空,用于邀请加入
                }
            }
            setTeamBuffTxt();
            team_object_change.value = change_v((int)ltpid, false);
        }
        void onTogAgreenAddOtherClick(bool b)
        {
            TeamProxy.getInstance().SendEditorInfoMembInv(b);
        }
        void onTogAgreeOtherApplyClick(bool b)
        {
            TeamProxy.getInstance().SendEditorInfoDirJoin(b);
        }
        void onAffirminvite(GameEvent e)
        {
            Variant data = e.data;
            ItemTeamMemberData itm = A3_TeamModel.getInstance().AffirmInviteData;
            team_object_change.value = change_v((int)itm.ltpid, false);
            Show(itm);
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
       
           
            if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
            {
                itd.showRemoveMemberBtn = true;
            }
            else
            {
                itd.showRemoveMemberBtn = false;
            }

            itd.teamId = TeamProxy.getInstance().MyTeamData.teamId;
            int index = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            itemMemberObjList[index - 1].SetInfo(itd, TeamProxy.getInstance().MyTeamData.meIsCaptain);
            setTeamBuffTxt();
           

        }
        void onKickOut(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            //不用删除后边的,直接用set,然后将信息擦除,放到last
            int indexRemove = (int)TeamProxy.getInstance().MyTeamData.removedIndex;
            ItemMemberObj imo = itemMemberObjList[indexRemove];
            itemMemberObjList.RemoveAt(indexRemove);
          //  itemMemberObjList.Add(new ItemMemberObj(imo.root));
            itemMemberObjList.Add(imo);
            imo.ClearInfo();
            for (int i = indexRemove; i < itemMemberObjList.Count; i++)
            {
                itemMemberObjList[i].gameObject.transform.SetSiblingIndex(i);
            }         
            setTeamBuffTxt();
        }
        void onChangeTeamInfo(GameEvent e)
        {
            if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
            {
                togInvite.gameObject.SetActive(true);
               // togJoin.gameObject.SetActive(true);
                if (e.data.ContainsKey("memb_inv") && togInvite.isOn != TeamProxy.getInstance().MyTeamData.membInv)
                {
                    togInvite.isOn = TeamProxy.getInstance().MyTeamData.membInv;
                }

                //if (e.data.ContainsKey("dir_join") && togJoin.isOn != TeamProxy.getInstance().MyTeamData.dirJoin)
               // {
                    //togJoin.isOn = TeamProxy.getInstance().MyTeamData.dirJoin;
               // }
            }
            else
            {
                bool isEnable = TeamProxy.getInstance().MyTeamData.membInv;
                int startCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
                for (int i = startCount; i < itemMemberObjList.Count; i++)
                {
                    itemMemberObjList[i].ClearInfo(isEnable);
                }

            }

        }
        void onNoticeHaveMemberLeave(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            //不用删除后边的,直接用set,然后将信息擦除,放到last
            int indexRemove = (int)TeamProxy.getInstance().MyTeamData.removedIndex;
            ItemMemberObj imo = itemMemberObjList[indexRemove];
            itemMemberObjList.RemoveAt(indexRemove);
            itemMemberObjList.Add(imo);
            imo.ClearInfo();
            for (int i = indexRemove; i < itemMemberObjList.Count; i++)
            {
                itemMemberObjList[i].root.SetSiblingIndex(i);
            }
            int itemMemberCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            for (int i = indexRemove; i < itemMemberCount; i++)
            {
                ItemTeamData itd = TeamProxy.getInstance().MyTeamData.itemTeamDataList[i];
                itemMemberObjList[i].SetInfo(itd, TeamProxy.getInstance().MyTeamData.meIsCaptain);
            }
            for (int i = itemMemberCount; i < itemMemberObjList.Count; i++)//对空位置的操作
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain || TeamProxy.getInstance().MyTeamData.membInv)
                {
                    itemMemberObjList[i].ClearInfo(true);
                }
                else
                {
                    itemMemberObjList[i].ClearInfo();
                }
            }
            if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
            {
                togInvite.gameObject.SetActive(true);
              //  togJoin.gameObject.SetActive(true);
                if (togInvite.isOn != TeamProxy.getInstance().MyTeamData.membInv)
                {
                    togInvite.isOn = TeamProxy.getInstance().MyTeamData.membInv;
                }
               // if (togJoin.isOn != TeamProxy.getInstance().MyTeamData.dirJoin)
                //{
               //     togJoin.isOn = TeamProxy.getInstance().MyTeamData.dirJoin;
               // }
            }
            else
            {
                togInvite.gameObject.SetActive(false);
              //  togJoin.gameObject.SetActive(false);
            }
            setTeamBuffTxt();
            ItemTeamMemberData itm = TeamProxy.getInstance().MyTeamData;
            Show(itm);
        }
        void onLeaveTeam(GameEvent e)
        {
            //初始化数据
            _instance.gameObject.SetActive(false);
            a3_teamPanel._instance.gameObject.SetActive(true);
            TeamProxy.getInstance().SendGetMapTeam(a3_teamPanel._instance.begin_index, a3_teamPanel._instance.end_index);
        }
        void onChangeCaptain(GameEvent e)
        {
            changeCaptain();//换队长
            setTeamBuffTxt();
        }
        void onNoticeOnlineStateChange(GameEvent e)//在线状态变更 重连在线&离线 两种状态
        {
            NoticeOnlineStateChange();//切换在线状态
        }

        void changeCaptain()
        {
            int memberCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            ItemTeamMemberData itmd = TeamProxy.getInstance().MyTeamData;
            List<ItemTeamData> itemTeamDataList = TeamProxy.getInstance().MyTeamData.itemTeamDataList;
            int imoCount = itemMemberObjList.Count;
            for (int i = 0; i < memberCount; i++)
            {
                for (int j = 0; j < itemMemberObjList.Count; j++)
                {
                    if (itemTeamDataList[i].cid == itemMemberObjList[j].cid)
                    {
                        if (itmd.meIsCaptain)
                        {
                            itemMemberObjList[j].ChangeToMeCaptain(itemTeamDataList[i]);
                        }
                        else
                        {
                            itemMemberObjList[j].ChangeToMeCustom();
                        }
                    }
                }
            }
            if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
            {
                for (int i = memberCount; i < imoCount; i++)
                {
                    itemMemberObjList[i].ChangeToMeCaptain(null, itmd);
                }
            }

            if (itmd.meIsCaptain)
            {
                togInvite.gameObject.SetActive(true);
               // togJoin.gameObject.SetActive(true);
                bool isCanInv = TeamProxy.getInstance().MyTeamData.membInv;
                togInvite.GetComponent<Toggle>().isOn = isCanInv;
                bool isCanjoin = TeamProxy.getInstance().MyTeamData.dirJoin;
               // togJoin.GetComponent<Toggle>().isOn = isCanjoin;
            }
            changeCaptainPos();
        }
        void changeCaptainPos()
        {
            int memberCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            ItemTeamMemberData itmd = TeamProxy.getInstance().MyTeamData;
            List<ItemTeamData> itemTeamDataList = TeamProxy.getInstance().MyTeamData.itemTeamDataList;
            int imoCount = itemMemberObjList.Count;
            for (int i = 0; i < memberCount; i++)
            {
                for (int j = 0; j < itemMemberObjList.Count; j++)
                {
                    if (itemTeamDataList[i].cid == itemMemberObjList[j].cid)
                    {
                        if (itemTeamDataList[i].isCaptain)
                        {
                            ItemMemberObj imo = itemMemberObjList[j];
                            itemMemberObjList.RemoveAt(j);
                            itemMemberObjList.Insert(0, imo);

                        }
                    }
                }
            }
            for (int i = 0; i < itemMemberObjList.Count; i++)
            {
                itemMemberObjList[i].root.SetSiblingIndex(i);
            }


        }
        void NoticeOnlineStateChange()
        {
            int memberCount = TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count;
            List<ItemTeamData> itemTeamDataList = TeamProxy.getInstance().MyTeamData.itemTeamDataList;
            int imoCount = itemMemberObjList.Count;
            for (int i = 0; i < memberCount; i++)
            {
                for (int j = 0; j < imoCount; j++)
                {
                    if (itemTeamDataList[i].cid == itemMemberObjList[j].cid)
                    {
                        itemMemberObjList[j].SetInfo(itemTeamDataList[i], TeamProxy.getInstance().MyTeamData.meIsCaptain);
                        ItemMemberObj imo = itemMemberObjList[j];
                        itemMemberObjList.RemoveAt(j);
                        itemMemberObjList.Insert(i, imo);
                        itemMemberObjList[i].root.SetSiblingIndex(i);
                    }
                }
            }
            setTeamBuffTxt();
          
        }
        void onBtnQuitTeamClick(GameObject go)
        {
            if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count == 1)//队伍没人了,只有自己一个还要离队时候
            {
                uint teamId = TeamProxy.getInstance().MyTeamData.teamId;
                TeamProxy.getInstance().SendDissolve(teamId);
            }
            else
            {
                TeamProxy.getInstance().SendLeaveTeam(cid);
            }
            Variant data = SvrLevelConfig.instacne.get_level_data(MapModel.getInstance().curLevelId);//取得相应id的level信息           
            if (data!=null &&data["team"]._int == 1)//在组队副本中不能退出队伍
                return;
            a3_currentTeamPanel._instance.gameObject.SetActive(false);
            a3_teamPanel._instance.gameObject.SetActive(true);
            A3_ActiveProxy.getInstance().SendGiveUpHunt();
        }

        public class ItemMemberObj : Skin
        {
            public Transform root;
            public Image iconHead;
            public Image iconCaptain;
            public Text txtName;
            public Text txtLvl;
            public Text txtCombat;
            public uint cid;
            BaseButton btnRemoveTeam;
            Transform noEmpty;
            Transform empty;
            ItemTeamData itemTeamData;
            BaseButton btnInvite;
            public ItemMemberObj(Transform tran) : base(tran) { Init(tran); }

            void Init(Transform trans)
            {
                root = trans;
                noEmpty = root.FindChild("noEmpty");
                empty = root.FindChild("empty");
                iconHead = root.FindChild("noEmpty/texts/icon/leader").GetComponent<Image>();
                iconCaptain = root.FindChild("noEmpty/iconCaptain").GetComponent<Image>();
                txtName = root.FindChild("noEmpty/texts/txtName/Text").GetComponent<Text>();
                txtLvl = root.FindChild("noEmpty/texts/txtLvl/Text").GetComponent<Text>();
                txtCombat = root.FindChild("noEmpty/texts/txtCombat/Text").GetComponent<Text>();
                btnRemoveTeam = new BaseButton(root.FindChild("noEmpty/btnRemoveTeam"));
                btnRemoveTeam.onClick = onBtnRemoveTeam;
                btnInvite = new BaseButton(root.FindChild("empty/btnInvite"));
                btnInvite.onClick = onBtnInviteClick;
            }

            public void SetInfo(ItemTeamData itd = null, bool meIsCaptain = false)
            {
               
                if (itd != null)
                {
                  
                    itemTeamData = itd;
                    cid = itd.cid;
                    txtName.text = itd.name;
                    txtLvl.text = itd.zhuan + ContMgr.getCont("zhuan") + itd.lvl + ContMgr.getCont("ji");
                    txtCombat.text = itd.combpt.ToString();
                    uint carr = itd.carr;
                  
                    if(carr==0) carr = (uint)PlayerModel.getInstance().profession;
                    if (a3_currentTeamPanel._instance.carrSpriteDic.ContainsKey(carr))
                    {
                   
                        iconHead.sprite = a3_currentTeamPanel._instance.carrSpriteDic[carr];
                        iconHead.SetNativeSize();
                    }
                    if (itd.online)
                    {
                        iconHead.gameObject.GetComponent<Image>().material = null;
                        iconCaptain.gameObject.GetComponent<Image>().material = null;
                        txtName.transform.parent.GetComponent<Image>().material = null;
                        txtLvl.transform.parent.GetComponent<Image>().material = null;
                        txtCombat.transform.parent.GetComponent<Image>().material = null;
                    }
                    else
                    {
                        iconHead.gameObject.GetComponent<Image>().material = a3_currentTeamPanel._instance.materialGrey;
                        iconCaptain.gameObject.GetComponent<Image>().material = a3_currentTeamPanel._instance.materialGrey;
                        txtName.transform.parent.GetComponent<Image>().material = a3_currentTeamPanel._instance.materialGrey;
                        txtLvl.transform.parent.GetComponent<Image>().material = a3_currentTeamPanel._instance.materialGrey;
                        txtCombat.transform.parent.GetComponent<Image>().material = a3_currentTeamPanel._instance.materialGrey;
                    }
                    noEmpty.gameObject.SetActive(true);
                    empty.gameObject.SetActive(false);
                    iconCaptain.gameObject.SetActive(itd.isCaptain);
                    if (meIsCaptain && cid != PlayerModel.getInstance().cid)
                    {
                        btnRemoveTeam.gameObject.SetActive(true);
                    }
                    else
                    {
                        btnRemoveTeam.gameObject.SetActive(false);
                    }

                }
                else
                {
                    empty.gameObject.SetActive(true);
                    noEmpty.gameObject.SetActive(false);
                }
            }
            public void ClearInfo(bool showInvite = false)
            {
                empty.gameObject.SetActive(true);
                noEmpty.gameObject.SetActive(false);
                cid = 0;
                iconHead.material = null;
                iconCaptain.material = null;
                txtName.text = string.Empty;
                txtLvl.text = string.Empty;
                txtCombat.text = string.Empty;
                btnInvite.gameObject.SetActive(showInvite);
            }
            public void ChangeToMeCaptain(ItemTeamData itd = null, ItemTeamMemberData itmd = null)//原队长离队,自己变成队长时候
            {
                if (itd != null)
                {
                    cid = itd.cid;
                    if (itd.cid != PlayerModel.getInstance().cid)
                    {
                        btnRemoveTeam.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (itmd != null)
                    {
                        btnInvite.gameObject.SetActive(true);
                    }
                }
            }
            //------------------------------------------
            void meIsCaptain()
            {
                if (cid == PlayerModel.getInstance().cid)//设置自己
                {
                    showCaptainIcon(true);
                    showBtnInvite(false);
                    showBtnRemoveTeam(false);
                }
                else//设置别人
                {
                    showCaptainIcon(false);
                    showBtnInvite(true);
                    showBtnRemoveTeam(true);
                }
            }
            void meIsCustom()
            {
                bool isMe = cid == PlayerModel.getInstance().cid ? false : true;
                showCaptainIcon(false);
                showBtnInvite(isMe);
            }
            void showContentInfo(bool b)//显示隐藏是否是空位置
            {
                noEmpty.gameObject.SetActive(b);
                empty.gameObject.SetActive(!b);
            }
            void showCaptainIcon(bool b)//显示隐藏队长标志
            {
                iconCaptain.gameObject.SetActive(b);
            }
            void showBtnInvite(bool b)//显示隐藏邀请按钮
            {
                btnInvite.gameObject.SetActive(b);
            }
            void showBtnRemoveTeam(bool b)//显示隐藏移除队伍按钮
            {
                btnRemoveTeam.gameObject.SetActive(b);
            }
            public void ChangeToMeCustom()
            {
                btnRemoveTeam.gameObject.SetActive(false);
            }
            void onBtnInviteClick(GameObject go)
            {
                Variant data = new Variant();
                data["index"] = 1;//open friendPanel
                UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_OPENFRIENDPANEL, this, data));
            }
            void onBtnRemoveTeam(GameObject go)
            {
                TeamProxy.getInstance().SendKickOut(cid);
            }

        }

    }
}
