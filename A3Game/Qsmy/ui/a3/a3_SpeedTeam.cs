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
    class a3_SpeedTeam:Window
    {
        List<GameObject> tab = new List<GameObject>();
        public static a3_SpeedTeam instance;
        int tabIdx;
        Transform team_infopanel;
        Transform pre;
        public uint begin_index = 0;
        public uint end_index = 10;
        public List<ItemTeamData> itdList = new List<ItemTeamData>();
        public int limited_dj = 0;
        int lvl, up_lvl;
        GameObject change_obj;
        BaseButton change_left,
                   change_right;
        Text change_txt;
        public int allnum = 0;
        public int begion_idx = 0;
        public override void init()
        {

            getComponentByPath<Text>("panelTab2/con/0/Text").text = ContMgr.getCont("a3_SpeedTeam_0");
            getComponentByPath<Text>("panelTab2/con/1/Text").text = ContMgr.getCont("a3_SpeedTeam_1");
            getComponentByPath<Text>("panelTab2/con/2/Text").text = ContMgr.getCont("a3_SpeedTeam_2");
            getComponentByPath<Text>("panelTab2/con/3/Text").text = ContMgr.getCont("a3_SpeedTeam_3");
            getComponentByPath<Text>("panelTab2/con/4/Text").text = ContMgr.getCont("a3_SpeedTeam_4");
            getComponentByPath<Text>("btnRefresh/Text").text = ContMgr.getCont("a3_SpeedTeam_5");
            getComponentByPath<Text>("createrteam/Text").text = ContMgr.getCont("a3_SpeedTeam_6");
            getComponentByPath<Text>("btncreat/Text").text = ContMgr.getCont("a3_SpeedTeam_6");

            instance = this;
            change_obj = getGameObjectByPath("change");
            change_left = new BaseButton(getTransformByPath("change/left"));
            change_right = new BaseButton(getTransformByPath("change/right"));
            change_left.onClick = ChangeBtnOnClickLeft;
            change_right.onClick =ChangeBtnOnClickRight; 
            change_txt = getComponentByPath<Text>("change/Text");
            change_txt.text = "1/1";
            for (int i = 0; i < transform.FindChild("panelTab2/con").childCount; i++)
            {
                tab.Add(transform.FindChild("panelTab2/con").GetChild(i).gameObject);
            }
            for (int i = 0; i < tab.Count; i++)
            {
                int tag = i;
                new BaseButton(tab[i].transform).onClick = delegate(GameObject go)
                  {
                      onTab(tag);
                  };
            }
            team_infopanel = transform.FindChild("team_tab1/panel/scroll_rect/contain");
            pre = transform.FindChild("team_tab1/panel/scroll_rect/team_pre");
           // pre.transform.FindChild("4/apply/Text").gameObject.GetComponent<Text>().text = ContMgr.getCont("a3_teamPanel_22");
            //BaseButton btnRefresh = new BaseButton(transform.FindChild("btnRefresh"));
            //btnRefresh.onClick = onBtnRefreshClick;



            TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);

           
                new BaseButton(transform.FindChild("btn_close")).onClick = (GameObject go)=> {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPEEDTEAM);
            };
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, getTeamPanel);
            // TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CURPAGE_TEAM, GetTeam_info);
            new BaseButton(getTransformByPath("createrteam")).onClick = (GameObject go) => { creatrveTremOnClick(go); };

            new BaseButton(getTransformByPath("btnRefresh")).onClick = (GameObject go) => { RefreshOnClick(go); };
        }

     
        public override void onShowed()
        {
            if (uiData == null)
                onTab(0);
            else
            {
                int index = (int)uiData[0];
                onTab(index);
            }
            transform.SetAsLastSibling();
          
        }
        //void onBtnRefreshClick(GameObject go)
        //{
        //    TeamProxy.getInstance().SendGetPageTeam(0,begin_index, end_index);
           
        //}
        private void getTeamPanel(GameEvent obj)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPEEDTEAM);
            flytxt.instance.fly(ContMgr.getCont("TeamProxy_add"));
            //ArrayList arr = new ArrayList();
            //arr.Add(2);
            //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, arr);

        }
        
       
        private void onTab(int tag)
        {

            tabIdx = tag;
            for (int i = 0; i < tab.Count; i++)
            {
                tab[i].GetComponent<Button>().interactable = true;
            }
            tab[tabIdx].GetComponent<Button>().interactable = false;
            Clear_con();


            ids = tabIdx;
            now_tab = 1;
            switch (tabIdx)
            {
                case 0:
                    //服务器发请求，组队目的为：自定义，所有队伍数据
                    TeamProxy.getInstance().SendGetPageTeam(0, begin_index, end_index);
                    //GetTeam_info(/*0*/);
                    limited_dj = Set_Limited_dj(5);
                    now_Tpid = 0;
                    break;
                case 1:
                    //服务器发请求，组队目的为：挂机，所有队伍数据
                    TeamProxy.getInstance().SendGetPageTeam(1, begin_index, end_index);
                    //GetTeam_info(/*1*/);
                    limited_dj = Set_Limited_dj(4);
                    
                    now_Tpid = 1;
                    break;
                case 2:
                    //服务器发请求，组队目的为：魔物猎人，所有队伍数据
                    TeamProxy.getInstance().SendGetPageTeam(2, begin_index, end_index);
                    //GetTeam_info(/*2*/);
                    limited_dj = Set_Limited_dj(3);
                    now_Tpid = 2;
                    break;
                case 3:
                    //组队副本墓穴
                    TeamProxy.getInstance().SendGetPageTeam(108, begin_index, end_index);
                    //GetTeam_info(/*108*/);
                    limited_dj = Set_Limited_dj(1);
                    now_Tpid = 108;
                    break;
                case 4:
                    //兽灵秘境
                    TeamProxy.getInstance().SendGetPageTeam(105, begin_index, end_index);
                    //GetTeam_info(/*105*/);
                    now_Tpid = 105;
                     limited_dj = Set_Limited_dj(2);
                    break;
                default:
                    break;
            }
            RefreshChangeTxt();

        }


        //刷新按钮
        void RefreshOnClick(GameObject go)
        {
            Clear_con();
            //print("tabIdx:" + tabIdx + "begin_index:" + begin_index + "end_index:" + end_index);
            TeamProxy.getInstance().SendGetPageTeam((uint)now_Tpid, (uint)(now_tab - 1) * 10, (uint)(now_tab - 1) * 10 + 10);
            //switch (tabIdx)
            //{
            //    case 0:
            //        //服务器发请求，组队目的为：自定义，所有队伍数据
            //        TeamProxy.getInstance().SendGetPageTeam(0, begin_index, end_index);
            //        break;
            //    case 1:
            //        //服务器发请求，组队目的为：挂机，所有队伍数据
            //        TeamProxy.getInstance().SendGetPageTeam(1, begin_index, end_index);
            //        break;
            //    case 2:
            //        //服务器发请求，组队目的为：魔物猎人，所有队伍数据
            //        TeamProxy.getInstance().SendGetPageTeam(2, begin_index, end_index);
            //        break;
            //    case 3:
            //        //组队副本墓穴
            //        TeamProxy.getInstance().SendGetPageTeam(108, begin_index, end_index);
            //        break;
            //    case 4:
            //        //兽灵秘境
            //        TeamProxy.getInstance().SendGetPageTeam(105, begin_index, end_index);
            //        break;
            //    default:
            //        break;
            //}
        }


        //删除obj
        private void Clear_con()
        {
            if (team_infopanel.childCount > 0)
            {
                for (int i = 0; i < team_infopanel.childCount; i++)
                {
                    Destroy(team_infopanel.GetChild(i).gameObject);
                }
            }
            limited_dj = 0;
        }

        //创建刷新数据
        public void GetTeam_info(ItemTeamMemberData itm)
        {

            // ItemTeamMemberData itm = TeamProxy.getInstance()?.pageItemTeamData;           
            itdList.Clear();
            for (int i = 0; i < itm.itemTeamDataList.Count; i++)
            {
                //if (itm.itemTeamDataList[i].ltpid == v)
                //{
                itdList.Add(itm.itemTeamDataList[i]);
                //}
            }
            Set_teaminfo();
        }
        public void Set_teaminfo()
        {

           
            int itmCount = itdList.Count;
            for (int i = 0; i < itmCount; i++)
            {
                var go = GameObject.Instantiate(pre.gameObject) as GameObject;
                go.transform.SetParent(team_infopanel);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                Set_Line(go.transform, itdList[i]);

                Set_apply(go.transform, itdList[i]);
               

            }
            RefreshChangeTxt();
           // team_infopanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (team_infopanel.GetComponent<GridLayoutGroup>().cellSize.y) * itmCount);
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
                        flytxt.instance.fly(ContMgr.getCont("a3_SpeedTeamtxt"));
                    }
                };
            }
        }
        private void Set_Line(Transform go, ItemTeamData data)
        {
        
            go.transform.FindChild("1/" + data.carr).gameObject.SetActive(true);
            go.transform.FindChild("2/name").GetComponent<Text>().text = data.name;
            go.transform.FindChild("2/dj").GetComponent<Text>().text = data.zhuan + ContMgr.getCont("zhuan") + data.lvl + ContMgr.getCont("ji");
            go.transform.FindChild("2/zy/" + data.carr).gameObject.SetActive(true);
            //队员的职业图标
            for (int i = 0; i < data.members.Count; i++)
            {
                go.transform.FindChild("3/zy/teamer/"+(i+1)+"/" + data.members[i]).gameObject.SetActive(true);
            }
            //go.transform.FindChild("3/zy/teamer/1/"+data.members[0]).gameObject.SetActive(true);
            //go.transform.FindChild("3/zy/teamer/2/2").gameObject.SetActive(true);
            //go.transform.FindChild("3/zy/teamer/3/2"/*+首领的职业2、3、5*/).gameObject.SetActive(true);
            //go.transform.FindChild("3/zy/teamer/4/2"/*+首领的职业2、3、5*/).gameObject.SetActive(true);
            //go.transform.FindChild("3/zy/teamer/5/2"/*+首领的职业2、3、5*/).gameObject.SetActive(true);
            //要求等级
            if (limited_dj == 0)
                go.transform.FindChild("3/dj").GetComponent<Text>().text = ContMgr.getCont("a3_SpeedTeamtxt1");
            else
            {
                go.transform.FindChild("3/dj").GetComponent<Text>().text = up_lvl+ ContMgr.getCont("zhuan")+lvl+ ContMgr.getCont("ji");
            }
          


           
        }
        //左右按钮刷新
        int now_tab = 1;
        int now_Tpid = 0;
        void ChangeBtnOnClickLeft(GameObject go)
        {
            if (allnum <= 10)
                return;
            if (now_tab <= 1)

            {
                flytxt.instance.fly("最小了");
                return;
            }
            else
            {
                Clear_con();
                now_tab -= 1;
                TeamProxy.getInstance().SendGetPageTeam((uint)now_Tpid, (uint)(now_tab - 1) * 10, (uint)(now_tab - 1) * 10 + 10);

                flytxt.instance.fly("-1");
                
                RefreshChangeTxt();
            }
        }
        void ChangeBtnOnClickRight(GameObject go)
        {
            if (allnum <= 10)
                return;
            int num = allnum % 10 > 0 ? allnum / 10 + 1 : allnum / 10;
            if (now_tab >= num)
            {
                flytxt.instance.fly("最大了");
                return;
            }
            else
            {
                Clear_con();
                now_tab += 1;
                TeamProxy.getInstance().SendGetPageTeam((uint)now_Tpid, (uint)(now_tab - 1) * 10, (uint)(now_tab - 1) * 10 + 10);

                flytxt.instance.fly("+1");
                
                RefreshChangeTxt();
            }
        }
        //页签数据刷新
        public  void RefreshChangeTxt()
        {


            if(allnum>10)
            {
                int a = allnum % 10;
                change_txt.text = a > 0 ? now_tab + "/" + (int)(allnum/10)+1 : now_tab + "/" + (int)(allnum / 10);
            }
            else
            {
                change_txt.text = "1/1";
            }




        }




        int ids = 0;
        int mapid = 0;
        void creatrveTremOnClick(GameObject go)
        {

            switch (ids)
            {
                case 0:
                    mapid = 0;
                    break;
                case 1:
                    mapid = 1;
                    break;
                case 2:
                    mapid = 2;
                    break;
                case 3:
                    mapid = 108;
                    break;
                case 4:
                    mapid = 105;
                    break;

            }
            if (!A3_TeamModel.getInstance().Limit_Change_Teammubiao(ids))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_teamPanel_noopen"));
                return;
            }
            else
            {
                TeamProxy.getInstance().SendGetMapTeam(begin_index, end_index);
                TeamProxy.getInstance().SendCreateTeam(mapid);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPEEDTEAM);
            }

        }
        public int Set_Limited_dj(int i)//1-组队副本，2-兽灵秘境，3-魔物猎人，4-挂机，5-自定义
        {
            lvl = 0; up_lvl = 0;
            var xml = XMLMgr.instance.GetSXML("func_open.team_lv_limit", "id==" + i);
            {
                //Debug.LogError(xml.getInt("lv"));
                lvl = xml.getInt("lv");
                up_lvl = xml.getInt("zhuan");

            }

            return up_lvl * 100 + lvl;

        }



    }
}
