using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
    class a3_teamPanel : BaseShejiao
    {
        public Dictionary<uint, itemTeamInfoPrefab> itemTeamInfoPrefabDic;
        public static a3_teamPanel _instance;
        public Dictionary<uint, Sprite> iconSpriteDic;
       
        public a3_teamPanel(Transform trans)
            : base(trans)
        {
            Init();
        }
        public uint begin_index = 0;
        public uint end_index = 20;
        public GameObject team_object;
        public int object_num;
        Dropdown team_objectPanel;
           
        public void Init()
        {
            _instance = this;
            itemTeamInfoPrefabDic = new Dictionary<uint, itemTeamInfoPrefab>();
            BaseButton btnJoinTeam = new BaseButton(transform.FindChild("right/bottom/btnJoinTeam"));
            btnJoinTeam.onClick = onBtnJoinTeamClick;
            BaseButton btnCreateTeam = new BaseButton(transform.FindChild("right/bottom/btnCreateTeam"));
            btnCreateTeam.onClick = onBtnCreateClick;
            team_object = transform.FindChild("team_object").gameObject;

            team_objectPanel = team_object.transform.FindChild("Dropdown").gameObject.GetComponent<Dropdown>();

            team_objectPanel.captionText.text = ContMgr.getCont("a3_teamPanel_16");
            for (int i = 0; i < team_objectPanel.options.Count; i++)
            {
                team_objectPanel.options[i].text = ContMgr.getCont("a3_teamPanel_" + (i + 16));
            }

            team_object.SetActive(false);

            BaseButton btn_1_object = new BaseButton(transform.FindChild("team_object/btn_1"));
            btn_1_object.onClick = onbtn_1_Click;
            BaseButton btn_0_object = new BaseButton(transform.FindChild("team_object/btn_0"));
            btn_0_object.onClick = onbtn_0_Click;

            BaseButton btnRefresh = new BaseButton(transform.FindChild("right/bottom/btnRefresh"));
            btnRefresh.onClick = onBtnRefreshClick;
            new BaseButton(transform.FindChild("right/bottom/speedteam")).onClick = (GameObject go) =>
            {
                
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPEEDTEAM);

            };


            Toggle togShowNearbyPanel = transform.FindChild("right/main/body/showNearby").GetComponent<Toggle>();
            togShowNearbyPanel.onValueChanged.AddListener(onShowNearby);
            getProfessionSprite();

            #region 初始化汉字
            getComponentByPath<Text>("title/txts/txtCaptain").text = ContMgr.getCont("a3_teamPanel_0");
            getComponentByPath<Text>("title/txts/txtLvl").text = ContMgr.getCont("a3_teamPanel_1");
            getComponentByPath<Text>("title/txts/txtKnightage").text = ContMgr.getCont("a3_teamPanel_2");
            getComponentByPath<Text>("title/txts/txtMap").text = ContMgr.getCont("a3_teamPanel_3");
            getComponentByPath<Text>("title/txts/txtMembCount").text = ContMgr.getCont("a3_teamPanel_4");
            getComponentByPath<Text>("right/main/body/showNearby/Label").text = ContMgr.getCont("a3_teamPanel_5");
            getComponentByPath<Text>("right/bottom/btnRefresh/Text").text = ContMgr.getCont("a3_teamPanel_6");
            getComponentByPath<Text>("right/bottom/btnCreateTeam/Text").text = ContMgr.getCont("a3_teamPanel_7");
            getComponentByPath<Text>("right/bottom/btnJoinTeam/Text").text = ContMgr.getCont("a3_teamPanel_8");
            getComponentByPath<Text>("right/bottom/speedteam/Text").text = ContMgr.getCont("a3_teamPanel_9");
            getComponentByPath<Text>("itemPrefabs/itemTeamInfo/team_pre/4/apply/Text").text = ContMgr.getCont("a3_teamPanel_15");
            getComponentByPath<Text>("itemPrefabs/itemTeamInfo/btnWatch/Text").text = ContMgr.getCont("a3_teamPanel_10");
            getComponentByPath<Text>("itemPrefabs/itemTeamInfo/txtLvl").text = ContMgr.getCont("a3_teamPanel_1");
            getComponentByPath<Text>("itemPrefabs/itemTeamInfo/txtKnightage").text = ContMgr.getCont("a3_teamPanel_2");
            getComponentByPath<Text>("itemPrefabs/itemTeamInfo/txtMap").text = ContMgr.getCont("a3_teamPanel_11");
            getComponentByPath<Text>("team_object/text").text = ContMgr.getCont("a3_teamPanel_12");
            getComponentByPath<Text>("team_object/btn_0/text").text = ContMgr.getCont("a3_teamPanel_13");
            getComponentByPath<Text>("team_object/btn_1/text").text = ContMgr.getCont("a3_teamPanel_14");
            getComponentByPath<Text>("team_object/Dropdown/Label").text = ContMgr.getCont("a3_teamPanel_16");

            ScrollControler scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("right/main/body/Panel"));
            #endregion

        }

       
        private void onbtn_1_Click(GameObject go)
        {
            int v = 0;
            object_num = team_object.transform.FindChild("Dropdown").GetComponent<Dropdown>().value;
         
            if (!A3_TeamModel.getInstance().Limit_Change_Teammubiao(object_num))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_teamPanel_noopen"));
                team_object.transform.FindChild("Dropdown").GetComponent<Dropdown>().value = 0;
                return;
            }
            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);
            //switch (object_num)
            //{
            //    case 0:
            //        v = 0;//自定义
            //        break;
            //    case 1:
            //        v = 1;//挂机
            //        break;
            //    case 2://托维尔墓穴
            //        v = 108;
            //        break;
            //    case 3://兽灵秘境
            //        v = 105;
            //        break;
            //    case 4://魔物猎人
            //        v = 2;
            //        break;
            //}
            TeamProxy.getInstance().SendCreateTeam(a3_currentTeamPanel._instance.change_v(object_num,true));
            team_object.SetActive(false);
            if (a3_currentTeamPanel._instance != null)
                a3_currentTeamPanel._instance.team_object.GetComponent<Dropdown>().value = object_num;
        }

        private void onbtn_0_Click(GameObject obj)
        {
            team_object.SetActive(false);
        }
        public override void onShowed()
        {

            base.onShowed();
            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);

            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CREATETEAM, onCreateTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_DISSOLVETEAM, onDissolveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_TEAMLISTINFO, onGetTeamListInfo);
        }

        public override void onClose()
        {
            base.onClose();

            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CREATETEAM, onCreateTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_DISSOLVETEAM, onDissolveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_TEAMLISTINFO, onGetTeamListInfo);
        }

        void onShowNearby(bool b)//是否只显示附近的队伍
        {
            ItemTeamMemberData itm = TeamProxy.getInstance().mapItemTeamData;
            List<ItemTeamData> itdList = new List<ItemTeamData>();
            itdList.Clear();
            for (int i = 0; i < itm.itemTeamDataList.Count;i++ )
            {
                if (itm.itemTeamDataList[i].mapId!=PlayerModel.getInstance().mapid)
                {
                    itdList.Add(itm.itemTeamDataList[i]);
                }
            }
            for (int i = 0; i < itdList.Count;i++ )
            {
                if (itemTeamInfoPrefabDic.ContainsKey(itdList[i].teamId))
                {
                    itemTeamInfoPrefabDic[itdList[i].teamId].root.gameObject.SetActive(!b);
                }
            }
        }
        void onBtnJoinTeamClick(GameObject go)
        { 
            foreach (KeyValuePair<uint, itemTeamInfoPrefab> itip in itemTeamInfoPrefabDic)
            {
                bool isON = itip.Value.root.GetComponent<Toggle>().isOn;
                if (isON)
                {
                    TeamProxy.getInstance().SendApplyJoinTeam(itip.Key);
                    break;
                }
            }
        }
        void onBtnCreateClick(GameObject go)
        {
            team_object.SetActive(true);
            team_object.transform.FindChild("Dropdown").GetComponent<Dropdown>().value = 0;        
        }
        void onBtnRefreshClick(GameObject go)
        {
            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);
        }
        void getProfessionSprite()
        {
            iconSpriteDic = new Dictionary<uint, Sprite>();
            for (int i = 2; i <= 5; i++)
            {
                iconSpriteDic.Add((uint)i, GAMEAPI.ABUI_LoadSprite(getProfession(i)));
            }
        }
        string getProfession(int profession)
        {
            string file = string.Empty;
            switch (profession)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_h2";
                    break;
                case 3:
                    file = "icon_job_icon_h3";
                    break;
                case 4:
                    file = "icon_job_icon_h4";
                    break;
                case 5:
                    file = "icon_job_icon_h5";
                    break;
            }
            return file;
        }
        #region event
        void onCreateTeam(GameEvent e)
        {
            Variant data = e.data;
            uint teamId = data["teamid"];
            if (!itemTeamInfoPrefabDic.ContainsKey(teamId))
            {
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
                itd.isCaptain = true;

                itemTeamInfoPrefab itip = new itemTeamInfoPrefab(transform);
                itip.Set(itd);
                itemTeamInfoPrefabDic[teamId] = itip;
                this.gameObject.SetActive(false);
            }
        }
        void onGetTeamListInfo(GameEvent e)
        {
            ItemTeamMemberData itm = TeamProxy.getInstance().mapItemTeamData;
            List<uint> newTeams = new List<uint>();
            newTeams.Clear();
            for (int i = 0; i < itm.itemTeamDataList.Count; i++)
            {
                newTeams.Add(itm.itemTeamDataList[i].teamId);
            }
            Dictionary<uint, itemTeamInfoPrefab> needRemoveDic = new Dictionary<uint, itemTeamInfoPrefab>();
            needRemoveDic.Clear();
            foreach (KeyValuePair<uint, itemTeamInfoPrefab> itip in itemTeamInfoPrefabDic)
            {
                if (!newTeams.Contains(itip.Key))
                {
                    needRemoveDic[itip.Key] = itip.Value;
                }
            }
            foreach (KeyValuePair<uint, itemTeamInfoPrefab> itip in needRemoveDic)
            {
                if(itemTeamInfoPrefabDic[itip.Key].root != null)
                    GameObject.Destroy(itemTeamInfoPrefabDic[itip.Key].root.gameObject);
                itemTeamInfoPrefabDic.Remove(itip.Key);
            }

            uint totalCount = itm.totalCount;
            uint indexBegin = itm.idxBegin;
            foreach (ItemTeamData tlid in itm.itemTeamDataList)
            {
                if (itemTeamInfoPrefabDic.ContainsKey(tlid.teamId))
                {
                    itemTeamInfoPrefabDic[tlid.teamId].Set(tlid);

                }
                else
                {
                    itemTeamInfoPrefab itip = new itemTeamInfoPrefab(transform);
                    itip.Set(tlid);
                    itemTeamInfoPrefabDic[tlid.teamId] = itip;
                }
            }

        }
        void onDissolveTeam(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("teamid"))
            {
                uint teamId = data["teamid"];
                if ((itemTeamInfoPrefabDic.ContainsKey(teamId)))
                {
                    if (itemTeamInfoPrefabDic[teamId].root != null)
                    {
                        GameObject go = itemTeamInfoPrefabDic[teamId].root.gameObject;
                        GameObject.Destroy(go);
                    }
                    itemTeamInfoPrefabDic.Remove(teamId);
                }
            }
        }
        #endregion

        public class itemTeamInfoPrefab
        {
            public itemTeamInfoPrefab(Transform trans) { Init(trans); }

            GameObject prefab;
            GameObject iconCarr_head;
            GameObject iconCarr_zy;
            Text txtCapatain;
            Text txtLvl;
            Text txtKnightage;
            Text txtMap;
            Text txtMembCount;
            const string strMembCount = "{0}/{1}";
            public uint tid;
           
            public Transform root;
            public int limited_dj = 0;
            int lvl, up_lvl;
            void Init(Transform trans)
            {
                if (prefab == null)
                {
                    prefab = GameObject.Instantiate(trans.FindChild("itemPrefabs/itemTeamInfo").gameObject) as GameObject;
                }
                else
                {
                    prefab = GameObject.Instantiate(prefab) as GameObject;
                }
                root = prefab.transform;
                iconCarr_zy = root.FindChild("team_pre/2/zy").gameObject;
                iconCarr_head = root.FindChild("team_pre/1").gameObject;
                prefab.transform.SetParent(trans.FindChild("right/main/body/Panel/contains"), false);
              
            
                root.GetComponent<Toggle>().group = trans.FindChild("right/main/body/Panel/contains").GetComponent<ToggleGroup>();
                prefab.transform.localScale = Vector3.one;
                prefab.transform.SetAsLastSibling();
             
                if (!prefab.activeSelf) prefab.SetActive(true);
                txtCapatain = prefab.transform.FindChild("team_pre/2/name").GetComponent<Text>();
                txtLvl = prefab.transform.FindChild("team_pre/2/dj").GetComponent<Text>();
                txtKnightage = prefab.transform.FindChild("txtKnightage").GetComponent<Text>();
                txtMap = prefab.transform.FindChild("team_pre/textMap").GetComponent<Text>();
                txtMembCount = prefab.transform.FindChild("txtMembCount").GetComponent<Text>();
                BaseButton btnWatch = new BaseButton(prefab.transform.FindChild("btnWatch"));
                btnWatch.onClick = onBtnWatchClick;
            }
            public int Set_Limited_dj(int i)//1-组队副本，2-兽灵秘境，3-魔物猎人，4-挂机，5-自定义 6-驯龙者的末日 7-血之盛宴
            {
                lvl = 0; up_lvl = 0;
                var xml = XMLMgr.instance.GetSXML("func_open.team_lv_limit", "id==" + i);
                {                 
                    lvl = xml.getInt("lv");
                    up_lvl = xml.getInt("zhuan");

                }

                return up_lvl * 100 + lvl;

            }

            public void Set(ItemTeamData itd)
            {
                tid = itd.teamId;
                uint lv = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl;
                txtCapatain.text = itd.name;
                txtLvl.text = itd.zhuan + ContMgr.getCont("zhuan") + itd.lvl.ToString() + ContMgr.getCont("ji");
                txtKnightage.text = itd.knightage;
                if (a3_teamPanel._instance.iconSpriteDic.ContainsKey(itd.carr))
                {
                    //iconCarr.sprite = a3_teamPanel._instance.iconSpriteDic[itd.carr];
                    iconCarr_zy.transform.FindChild(""+itd.carr).gameObject.SetActive(true);
                    iconCarr_head.transform.FindChild("" + itd.carr).gameObject.SetActive(true);
                }
                txtMap.text = SvrMapConfig.instance.getSingleMapConf((uint)itd.mapId) == null ? "" : SvrMapConfig.instance.getSingleMapConf((uint)itd.mapId)["map_name"]._str; ;
                txtMembCount.text = string.Format(strMembCount, itd.curcnt, 5);


                if (itd.members != null)
                {
                    for (int i = 0; i < itd.members.Count; i++)
                    {
                        prefab.transform.FindChild("team_pre/3/zy/teamer/" + (i + 1) + "/" + itd.members[i]).gameObject.SetActive(true);
                    }
                }
               
                int v = 0;
                switch (itd.ltpid)
                {
                    case 0:
                        v = 5;//自定义
                        break;
                    case 1:
                        v = 4;//挂机
                        break;
                    case 2:
                        v = 3;
                        break;
                    case 105:
                        v = 2;
                        break;
                    case 108:
                        v = 1;
                        break;
                    case 109:
                        v = 6;
                        break;
                    case 110:
                        v = 7;
                        break;
                }
               
                limited_dj = Set_Limited_dj(v);


                if (limited_dj == 0)
                    prefab.transform.FindChild("team_pre/3/dj").GetComponent<Text>().text = ContMgr.getCont("a3_SpeedTeamtxt1");
                else
                {
                    prefab.transform.FindChild("team_pre/3/dj").GetComponent<Text>().text = up_lvl + ContMgr.getCont("zhuan") + lvl + ContMgr.getCont("ji");
                }
                         
                if (itd.curcnt >= 5)
                {
                    prefab.transform.FindChild("team_pre/4/full").gameObject.SetActive(true);
                    prefab.transform.FindChild("team_pre/4/apply").gameObject.SetActive(false);
                    prefab.transform.FindChild("team_pre/4/applyed").gameObject.SetActive(false);
                }
                if (TeamProxy.getInstance().MyTeamData == null&&itd.curcnt<5)
                {
                    prefab.transform.FindChild("team_pre/4/full").gameObject.SetActive(false);
                    prefab.transform.FindChild("team_pre/4/apply").gameObject.SetActive(true);
                    prefab.transform.FindChild("team_pre/4/applyed").gameObject.SetActive(false);
                }
                if (itd.curcnt < 5 && prefab.transform.FindChild("team_pre/4/applyed").gameObject.activeInHierarchy == false && lv >= limited_dj)
                {
                    prefab.transform.FindChild("team_pre/4/full").gameObject.SetActive(false);
                    prefab.transform.FindChild("team_pre/4/apply").gameObject.SetActive(true);
                    prefab.transform.FindChild("team_pre/4/applyed").gameObject.SetActive(false);

                    new BaseButton(prefab.transform.FindChild("team_pre/4/apply")).onClick = (GameObject oo) =>
                    {
                        if (TeamProxy.getInstance().MyTeamData == null && lv >= limited_dj)
                        {
                            //Debug.LogError(data.teamId);
                            TeamProxy.getInstance().SendApplyJoinTeam(itd.teamId);
                            prefab.transform.FindChild("team_pre/4/apply").gameObject.SetActive(false);
                            prefab.transform.FindChild("team_pre/4/applyed").gameObject.SetActive(true);
                        }
                        else if (TeamProxy.getInstance().MyTeamData != null && lv >= limited_dj)
                             {
                            flytxt.instance.fly(ContMgr.getCont("a3_teamPanel_goout"));
                            }
                            else
                          {
                            flytxt.instance.fly(ContMgr.getCont("a3_teamPanel_lvlock"));
                          }
                       
                    };
                }
            }

            private void Set_apply(Transform go, ItemTeamData data)
            {
                uint lv = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl;
                if (data.curcnt >= 5)
                {
                    go.transform.FindChild("4/full").gameObject.SetActive(true);
                    go.transform.FindChild("4/apply").gameObject.SetActive(false);
                    go.transform.FindChild("4/applyed").gameObject.SetActive(false);
                }
                if (data.curcnt < 5 && go.transform.FindChild("4/applyed").gameObject.activeInHierarchy == false && lv >= limited_dj)
                {
                    go.transform.FindChild("4/full").gameObject.SetActive(false);
                    go.transform.FindChild("4/apply").gameObject.SetActive(true);
                    go.transform.FindChild("4/applyed").gameObject.SetActive(false);

                    new BaseButton(go.transform.FindChild("4/apply")).onClick = (GameObject oo) =>
                    {
                        if (TeamProxy.getInstance().MyTeamData == null)
                        {
                            //Debug.LogError(data.teamId);
                            TeamProxy.getInstance().SendApplyJoinTeam(data.teamId);
                            go.transform.FindChild("4/apply").gameObject.SetActive(false);
                            go.transform.FindChild("4/applyed").gameObject.SetActive(true);
                        }
                        else
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_teamPanel_goout"));
                        }
                    };
                }
            }
            void onBtnWatchClick(GameObject go)
            {
                worldmap.getmapid = false;
                TeamProxy.getInstance().SendWatchTeamInfo(tid);
                TeamProxy.WatchTeamId_limited = (uint)limited_dj;

            }
        }
    }
}
