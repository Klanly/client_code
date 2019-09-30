using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
using MuGame.Qsmy.model;

namespace MuGame
{

    class Basesport : Skin
    {
        public Basesport(Transform trans,string name) : base(trans)
        {
            sport_Name = name;
            sport_Obj = trans.gameObject;
            init();
        }
        public string sport_Name = null;
        public GameObject sport_Obj = null;
        public virtual void init() { }
        public virtual void onShowed() { }
        public virtual void onClose() { }
        virtual public void _updata() { }
    }
    class a3_sports : Window
    {
        Transform Con_view;
        private Basesport CurSport = null;
        private Transform contents;

        public static a3_sports  _instantiate ;
        private Dictionary<string, Basesport> sport_Dic = new Dictionary<string, Basesport>();
        public bool goback = false;
        public override void init()
        {
            getComponentByPath<Text>("Get_tab_shop/Text").text = ContMgr.getCont("a3_sports_0");
            getComponentByPath<Text>("scroll_view/contain/sports_jjc/name").text = ContMgr.getCont("a3_sports_1");
            getComponentByPath<Text>("scroll_view/contain/sports_jdzc/name").text = ContMgr.getCont("a3_sports_2");


            _instantiate = this;
            Con_view = this.transform.FindChild("scroll_view/contain");
            contents = this.transform.FindChild("contents");
            CheckLock();
            new BaseButton(this.transform.FindChild("btn_close")).onClick = (GameObject go) => {
                Toclose = true;
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPORTS);
   
            };
            for (int i = 0; i < Con_view.childCount; i++)
            {       
                new BaseButton(Con_view.GetChild(i)).onClick = (GameObject go) =>
                {
                    onTab(go.name);
                };
            }
            new BaseButton(transform.FindChild("Get_tab_shop")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPORTS);
                goback = true;
                Shop_a3Model.getInstance().selectType = 6;
                Shop_a3Model.getInstance().toSelect = true;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3);
            };
        }
        public override void onShowed()
        {
            Toclose = false;
            goback = false;
            if (uiData != null && uiData.Count > 0)
            {
                string n = (string)uiData[0];
                onTab(n);
            }
            else {
                for (int i = 0; i < Con_view.transform.childCount; i++)
                {
                    if (Con_view.transform.GetChild(i).gameObject.activeSelf)
                    {
                        onTab(Con_view.transform.GetChild(i).gameObject.name);
                        break;
                    }
                }
            }

            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        void onTab( string name)
        {
            for (int i = 0; i < Con_view.childCount; i++)
            {
                Con_view.GetChild(i).GetComponent<Button>().interactable = true;    
            }
            Con_view.FindChild(name).GetComponent<Button>().interactable = false;

            if (CurSport != null && CurSport.sport_Name == name) return;
                    
            foreach (Basesport sp in sport_Dic.Values) {
                if (sp != null)                 
                    sp.sport_Obj.SetActive(false);  
            }
            if (!sport_Dic.ContainsKey (name) || sport_Dic[name] == null)
            {   
                GameObject prefab = null;
                GameObject panel = null;                        
                switch (name)                   
                {
                    case "sports_jdzc":                 
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_sports_jdzc");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        sport_Dic[name] = new a3_sports_jdzc(panel.transform ,name);
                        break;

                    case "sports_jjc":
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_sports_jjc");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        sport_Dic[name] = new a3_sports_jjc(panel.transform, name);
                        break;
                }
                panel.transform.SetParent(contents, false);
            }
            CurSport?.onClose();
            CurSport = sport_Dic[name];
            CurSport?.onShowed();               
            CurSport?.gameObject.SetActive(true);       
        }
        bool Toclose = false;

        public override void onClosed()
        {
            if (CurSport != null)
            {
                CurSport.onClose();
                CurSport = null;
            }
            if (a3_getJewelryWay.instance && a3_getJewelryWay.instance.closeWin != null && Toclose)
            {
                InterfaceMgr.getInstance().ui_async_open(a3_getJewelryWay.instance.closeWin);
                a3_getJewelryWay.instance.closeWin = null;
            }
        }
        void Update()
        {
            if (CurSport != null) CurSport._updata();
        }

        public void CheckLock()
        {
            this.transform.FindChild("scroll_view/contain/sports_jjc").gameObject.SetActive(false) ;
            this.transform.FindChild("scroll_view/contain/sports_jdzc").gameObject.SetActive(false);
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PVP_DUNGEON))
            {
                OpenPVP();
            }

            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SPORT_JDZC))
            {
                OpenJDZC();
            }
        }


        public void OpenPVP()
        {
            this.transform.FindChild("scroll_view/contain/sports_jjc").gameObject.SetActive(true);
        }
        public void OpenJDZC() {
            this.transform.FindChild("scroll_view/contain/sports_jdzc").gameObject.SetActive(true);
        }

    }

    class a3_sports_jdzc : Basesport{


        Image header_img;
        Image header_icon;
        public static a3_sports_jdzc  _instan;
        public bool goBack = false;
        GameObject tip;
        GameObject findCon;

        GameObject tosureCon;
        Text MyScore;
        Text MyRanking;
        Text TimeRun;
        Text Find_Time;
        Text EstimatedTime;

        public a3_sports_jdzc(Transform trans, string name) : base(trans, name)
        {
        }


        public override  void init() {


            sport_Obj.transform.FindChild("myinfo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_0");
            sport_Obj.transform.FindChild("myinfo/Text1").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_1");
            sport_Obj.transform.FindChild("find/text1").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_2");
            sport_Obj.transform.FindChild("find/text2").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_3");
            sport_Obj.transform.FindChild("Text 2").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_4");
            sport_Obj.transform.FindChild("GetInfo_tab/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_5");
            sport_Obj.transform.FindChild("GetInfo_tab/shuoming/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_6");
            sport_Obj.transform.FindChild("GetInfo_tab/shuoming/Image/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_7");
            sport_Obj.transform.FindChild("GetInfo_tab/tab_top/1/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_14");
            sport_Obj.transform.FindChild("GetInfo_tab/tab_top/2/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_15");
            sport_Obj.transform.FindChild("GetInfo_tab/tab_top/3/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_16");
            sport_Obj.transform.FindChild("GetInfo_tab/tab_top/4/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_17");
            sport_Obj.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_7");
            sport_Obj.transform.FindChild("tip/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_20");
            sport_Obj.transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_21");
            sport_Obj.transform.FindChild("Finding/back/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_19");
            sport_Obj.transform.FindChild("Finding/timetxt").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_9");
            sport_Obj.transform.FindChild("Finding/EstimatedTimetxt").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_10");
            sport_Obj.transform.FindChild("ToSure/timeGo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_11");
            sport_Obj.transform.FindChild("ToSure/true/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jdzc_12");




            _instan = this;
            header_img = sport_Obj.transform.FindChild("m_rank/rank_text").GetComponent<Image>();
            header_icon = sport_Obj.transform.FindChild("m_rank/rank_icon").GetComponent<Image>();

            MyScore = sport_Obj.transform.FindChild("myinfo/Score").GetComponent<Text>();
            MyRanking = sport_Obj.transform.FindChild("myinfo/ranking").GetComponent<Text>();
            tip = sport_Obj.transform.FindChild("tip").gameObject;
            tosureCon = sport_Obj.transform.FindChild("ToSure").gameObject;
            findCon = sport_Obj.transform.FindChild("Finding").gameObject;
            TimeRun = tosureCon.transform.FindChild("timeGo/time").GetComponent<Text>();

            Find_Time = findCon.transform.FindChild("Time").GetComponent<Text>();
            EstimatedTime = findCon.transform.FindChild("EstimatedTime").GetComponent<Text>();

            new BaseButton(this.transform.FindChild("btn_rank")).onClick = (GameObject go) =>   
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPORTS);
                goBack = true;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RANK);
            };      
                                
            new BaseButton(sport_Obj.transform.FindChild("Get_tab_bt")).onClick = (GameObject go) => {
                sport_Obj.transform.FindChild("GetInfo_tab").gameObject.SetActive(true);
                setRewTab();
            };

            new BaseButton(sport_Obj.transform.FindChild("GetInfo_tab/back")).onClick = (GameObject go) =>
            {
                sport_Obj.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            };


            new BaseButton(sport_Obj.transform.FindChild("find")).onClick = (GameObject go) => {
                a3_sportsProxy.getInstance().find_game();
            };

            new BaseButton(sport_Obj.transform.FindChild("Finding/back")).onClick = (GameObject go) => {
                a3_sportsProxy.getInstance().cancel_game();
            };

            new BaseButton(sport_Obj.transform.FindChild("ToSure/yes")).onClick = (GameObject go) => {
                a3_sportsProxy.getInstance().toSure_game(true);
            };

            new BaseButton(sport_Obj.transform.FindChild("ToSure/no")).onClick = (GameObject go) => {
                a3_sportsProxy.getInstance().toSure_game(false);
            };

            new BaseButton(sport_Obj.transform.FindChild("Get_tab_rank")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SPORTS);
                goBack = true;
                ArrayList l = new ArrayList();
                l.Add(5);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RANKING, l);
            };

        }

        public override void onShowed()
        {
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_FINDING, Onfinding);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_FINDNOT, OnFindOver);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_FINDSCE, OnFindSec);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_INFB_LOS, Oninfb_los);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_TOSURE, onTosure);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_MYZC_INFO, setInfo);
            findCon.SetActive(false);
            tip.SetActive(false);
            tosureCon.SetActive(false);
            goBack = false;
            base.onShowed();
            setHeader_info();

            a3_sportsProxy.getInstance().getPrestige_info();
        }
        public override void onClose()
        {
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_FINDING, Onfinding);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_FINDNOT, OnFindOver);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_FINDSCE, OnFindSec);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_INFB_LOS, Oninfb_los);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_TOSURE, onTosure);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_MYZC_INFO, setInfo);
            sport_Obj.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            base.onClose();

            if (a3_sportsModel.getInstance().sport_stage == Game_stage.find)
            {
                a3_sportsProxy.getInstance().cancel_game();
            } else if (a3_sportsModel.getInstance().sport_stage == Game_stage.sure)
            {
                a3_sportsProxy.getInstance().toSure_game(false);
            }

        }
        void setHeader_info() {             
            string file = "icon_achievement_title_ui_" + a3_RankModel.now_id;
            header_img.sprite = GAMEAPI.ABUI_LoadSprite(file);
            header_img.SetNativeSize();

            if (a3_RankModel.now_id > 0)
            {
                header_icon.gameObject.SetActive(true);
                string file_icon = "icon_achievement_title_ui_t" + a3_RankModel.now_id;
                header_icon.sprite = GAMEAPI.ABUI_LoadSprite(file_icon);
            }
            else {
                header_icon.gameObject.SetActive(false);
            }
        }

        bool frist = true;
        //初始化奖励列表
        void setRewTab() {
            if (frist) {
                Transform View = this.transform.FindChild("GetInfo_tab/scrollview");
                GameObject item = View.FindChild("item").gameObject;
                Transform con = View.FindChild("con");
                if (con.childCount > 0)
                {
                    for (int i = 0; i < con.childCount; i++)
                    {
                        GameObject.Destroy(con.GetChild(i).gameObject);
                    }
                }
                List<SXML> Xml = XMLMgr.instance.GetSXMLList("pointarena.rank_reward");
                foreach (SXML sx in Xml)
                {
                    GameObject clon = (GameObject)GameObject.Instantiate(item);
                    int min = sx.getInt("min");
                    int max = sx.getInt("max");
                    string str_tal;
                    if (min >= max)
                    {
                        str_tal = min.ToString();
                    }
                    else {
                        str_tal = min + "-" + max;
                    }       
                    clon.transform.FindChild("1/Text").GetComponent<Text>().text = str_tal;

                    List<SXML> Reward = sx.GetNodeList("RewardValue");
                    foreach (SXML rew in Reward) {      
                        if (rew.getInt("type") == 4) {
                            clon.transform.FindChild("2/Text").GetComponent<Text>().text = rew.getInt("value").ToString();
                        } else if (rew.getInt("type") == 5) {
                            clon.transform.FindChild("3/Text").GetComponent<Text>().text = rew.getInt("value").ToString();
                        }
                    }
                    SXML item_rew = sx.GetNode("RewardItem");
                    if (item_rew != null) {
                        int item_id = item_rew.getInt("item_id");
                        int item_num = item_rew.getInt("value");
                        clon.transform.FindChild("4/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_"  + item_id);
                        //var xml = XMLMgr.instance.GetSXML("item.item", "id==" + item_id);
                        //string bx = xml.getString("item_name");
                        clon.transform.FindChild("4/Text").GetComponent<Text>().text = item_num.ToString ();

                        new BaseButton(clon.transform.FindChild("4")).onClick = (GameObject go) =>
                        {
                            showtip((uint)item_id);
                        };
                    }
                    clon.transform.SetParent(con,false);
                    clon.SetActive(true);
                }
                frist = false;
            }
        }


        void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }

        void setRank() {

        }


        bool runFind_time = false;
        float find_tm = 0;
        void Onfinding(GameEvent e){
            findCon.SetActive(true);
            find_tm = 0;
            runFind_time = true;
            Find_Time.text = "00:00";
            EstimatedTime.text = "00:00";
        }

        void OnFindOver(GameEvent e) {
            findCon.SetActive(false);
            runFind_time = false;
        }

        Dictionary<int, playerinfo> allPlayer = new Dictionary<int, playerinfo>();
        float _time;

        void OnFindSec(GameEvent e) {
            findCon.SetActive(false);
            runFind_time = false;
            tosureCon.SetActive(true);
            tosureCon.transform.FindChild("true").gameObject.SetActive(false);
            tosureCon.transform.FindChild("yes").gameObject.SetActive(true);
            tosureCon.transform.FindChild("no").gameObject.SetActive(true);
            Variant data = e.data;
            if (data.ContainsKey("matched"))
            {
                allPlayer.Clear();
                Transform con_view = tosureCon.transform.FindChild("conView/con");
                for (int i = 0; i < con_view.childCount;i++) {
                    con_view.GetChild(i).gameObject.SetActive(false);
                    con_view.GetChild(i).FindChild("b").gameObject.SetActive(true); 
                }
                Variant info = data["matched"];
                foreach (Variant one in info._arr)
                {
                    playerinfo p = new playerinfo();
                    p.cid = one["cid"];
                    p.carr = one["carr"];
                    for (int i = 0; i < con_view.childCount; i++)
                    {
                        if (con_view.GetChild(i).gameObject.activeSelf == false)        
                        {
                            con_view.GetChild(i).gameObject.SetActive(true);
                            con_view.GetChild(i).transform.FindChild ("icon").GetComponent <Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_"+ one["carr"]);
                            p.conobj = con_view.GetChild(i).gameObject;
                            break;
                        }
                        else continue;
                    }
                    allPlayer[one["cid"]] = p;
                }
                _time = a3_sportsModel.getInstance().tosureTime;
                toSure_runTime = true;
            }
        }

        bool toSure_runTime = false;
        float t = 1;
        public override void _updata()
        {
            if (tosureCon.activeSelf && toSure_runTime) {
                t -= Time.deltaTime;
                if (t <= 0) {
                    t = 1;
                    _time--;
                    TimeRun.text = _time.ToString();
                    if (_time <= 0)
                    {
                        toSure_runTime = false;
                        if (a3_sportsModel .getInstance ().sport_stage != Game_stage.ture_game) {
                            a3_sportsProxy.getInstance().toSure_game(false);
                        }
                    }
                }
            }

            if (findCon.activeSelf && runFind_time) {
                t -= Time.deltaTime;
                EstimatedTime.text = GetTm((int)(((int)(find_tm / 30) + 1) * 30));
                if (t <= 0) {
                    t = 1;
                    find_tm++;
                    Find_Time.text = GetTm((int)find_tm);
                }

            }
        }

        string GetTm(int tm)
        {
            string m = (tm / 60 % 60).ToString().Length > 1 ? (tm / 60 % 60).ToString() : "0" + (tm / 60 % 60);
            string s = (tm % 60).ToString().Length > 1 ? (tm % 60).ToString() : "0" + (tm % 60);
            string t =  m + ":" + s ;
            return t;
        }


        void onTosure(GameEvent e) {

            Variant data = e.data;
            int _cid = data["cid"];
            if (allPlayer.ContainsKey (_cid)) {
                if (data["ready"] == true) {
                    allPlayer[_cid].conobj.transform.FindChild("b").gameObject.SetActive(false);
                    if (_cid == PlayerModel.getInstance().cid)
                    {
                        tosureCon.transform.FindChild("true").gameObject.SetActive(true);
                        tosureCon.transform.FindChild("yes").gameObject.SetActive(false);
                        tosureCon.transform.FindChild("no").gameObject.SetActive(false);
                    }

                }
            }
        }

        void Oninfb_los(GameEvent e) {

            tosureCon.SetActive(false);
            Variant data = e.data;
            if (data.ContainsKey("cid") && data["cid"] == PlayerModel.getInstance().cid)
            {

            }
            else if(data.ContainsKey("cid") && data["cid"] != PlayerModel.getInstance().cid) {
                findCon.SetActive(true);
            }
        }
        void setInfo(GameEvent e) {
            MyScore.text = a3_sportsModel.getInstance().Score_jdzc.ToString();
            MyRanking.text = a3_sportsModel.getInstance().Ranking_jdzc.ToString();
        }
                




    }
    class a3_sports_jjc : Basesport
    {
        public static a3_sports_jjc instance;
        public a3_sports_jjc(Transform trans, string name) : base(trans, name)
        {
        }
        Text findCount;
        Text buyCount;
        Text duanwei;
        Text top_saiji;
        int findid = 0;
        Image rank;
        Text buy_Count_zuan;
        GameObject tip;
        public GameObject no_open;
        public GameObject yes_open;

        public override void init()
        {



            #region 初始化汉字
            this.transform.FindChild("info/Button/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_0");
            this.transform.FindChild("find_info/find/text1").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_1");
            this.transform.FindChild("find_info/find/text2").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_2");
            this.transform.FindChild("find_info/no_open/text1").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_3");
            this.transform.FindChild("find_info/day_count").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_4");
            this.transform.FindChild("find_info/buy_count").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_5");
            this.transform.FindChild("rank/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_6");
            this.transform.FindChild("Gift/last_rank/text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_7");
            this.transform.FindChild("Gift/geted").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_8");
            this.transform.FindChild("Gift/nullrew").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_9");
            this.transform.FindChild("reward/tet").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_10");
            this.transform.FindChild("GetInfo_tab/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_11");
            this.transform.FindChild("GetInfo_tab/shuoming/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_12");
            this.transform.FindChild("GetInfo_tab/shuoming/Image/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_13");
            this.transform.FindChild("GetInfo_tab/tab_top/1/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_14");
            this.transform.FindChild("GetInfo_tab/tab_top/2/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_15");
            this.transform.FindChild("GetInfo_tab/tab_top/3/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_16");
            this.transform.FindChild("GetInfo_tab/tab_top/4/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_17");
            this.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_18");
            this.transform.FindChild("Finding/back/Text").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_19");
            this.transform.FindChild("tip/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_20");
            this.transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("a3_sports_jjc_21");
            #endregion


            tip = this.transform.FindChild("tip").gameObject;
            findCount = this.transform.FindChild("find_info/day_count/text").GetComponent<Text>();
            buyCount = this.transform.FindChild("find_info/buy_count/text").GetComponent<Text>();
            // giftzuanshi = this.transform.FindChild("GetReward/tem1/zuan/bg_text/tex").GetComponent<Text>();
            // giftmingwang = this.transform.FindChild("GetReward/tem1/mingwang/bg_text/tex").GetComponent<Text>();
            // box = this.transform.FindChild("GetReward/tem1/box_name/tex").GetComponent<Text>();
            buy_Count_zuan = this.transform.FindChild("find_info/find/text2/text").GetComponent<Text>();
            rank = this.transform.FindChild("info/icon").GetComponent<Image>();
            duanwei = this.transform.FindChild("info/duanwei").GetComponent<Text>();
            top_saiji = this.transform.FindChild("top_text").GetComponent<Text>();
            no_open = this.transform.FindChild("find_info/no_open").gameObject;
            yes_open = this.transform.FindChild("find_info/find").gameObject;
            new BaseButton(getTransformByPath("reward")).onClick = (GameObject go) =>
            {
                a3_sportsProxy.getInstance().SendPVP(5);

            };
            new BaseButton(getTransformByPath("rank")).onClick = (GameObject go) =>
            {
                //排行
            };
            new BaseButton(getTransformByPath("find_info/find")).onClick = (GameObject go) =>
            {
                if (a3_active.MwlrIsDoing)
                {
                    ArrayList ast = new ArrayList();
                    ast.Add(fb_type.jjc);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MWLRCHANGE, ast);
                }
                else
                {
                    EnterOnClick();
                }

            };
            new BaseButton(getTransformByPath("Get_tab_bt")).onClick = (GameObject go) =>
            {
                //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(false);
                this.transform.FindChild("GetInfo_tab").gameObject.SetActive(true);
                intoTab();
            };
            new BaseButton(getTransformByPath("GetInfo_tab/back")).onClick = (GameObject go) =>
            {
                //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(true);
                this.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            };
            new BaseButton(getTransformByPath("Finding/back")).onClick = (GameObject go) =>
            {
                //取消匹配
                a3_sportsProxy.getInstance().SendPVP(3);
            };
            ref_zuan_count();
        }


        public void EnterOnClick()
        {

            //搜索对手
            if (findid == 0)
            {
                a3_sportsProxy.getInstance().SendPVP(2);
            }
            else
            {
                if (a3_sportsModel.getInstance().buy_cnt <= a3_sportsModel.getInstance().buyCount)
                    flytxt.instance.fly(ContMgr.getCont("pvp_nobuycount"));
                else
                {
                    a3_sportsProxy.getInstance().SendPVP(4);
                }
            }
        }


        bool b = true;
        public override void onShowed()
        {
            tip.SetActive(false);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_PVPSITE_INFO, Refresh);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_PVPGETREW, ReGet);
            if (b)
            {
                a3_sportsProxy.getInstance().SendPVP(1);
                b = false;
            }
            instance = this;
            refro_score();
            a3_sportsProxy.getInstance().SendPVP(6);
            refCount();
        }
        public override void onClose()
        {
            this.transform.FindChild("GetInfo_tab").gameObject.SetActive(false);
            refInto();
            instance = null;
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_PVPSITE_INFO, Refresh);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_PVPGETREW, ReGet);
        }
        private void refresh_opentime()
        {
            List<SXML> opentimeList = XMLMgr.instance.GetSXML("jjc").GetNodeList("t");
            int i = 0;
            int timezoneSpan = (DateTime.Now - DateTime.UtcNow).Hours;
            if (timezoneSpan < 0) timezoneSpan = timezoneSpan + 12;
            int cur_h = (muNetCleint.instance.CurServerTimeStamp / 3600 + timezoneSpan) % 24;
            for (; i < opentimeList.Count; i++)
            {
                int open_h = int.Parse(opentimeList[i].getString("opnetime").Split(',')[0]);
                if (cur_h < open_h)
                {
                  no_open.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_open_time", new List<string> { open_h.ToString() });
                    break;
                }
            }
            if (opentimeList.Count > 0 && i == opentimeList.Count)
                no_open.transform.FindChild("text1").GetComponent<Text>().text = ContMgr.getCont("a3_active_opentmmo_time", new List<string> { opentimeList[0].getString("opnetime").Split(',')[0] });

        }
        public void setbtn(bool open)
        {
            no_open.SetActive(!open);
            yes_open.SetActive(open);
            refresh_opentime();
        }

        bool frist = true;
        //载入竞技奖励说明表
        void intoTab()
        {
            if (frist)
            {
                Transform con = this.transform.FindChild("GetInfo_tab/scrollview/con");
                if (con.childCount > 0)
                {
                    for (int i = 0; i < con.childCount; i++)
                    {
                        GameObject.Destroy(con.GetChild(i).gameObject);
                    }
                }
                GameObject item = this.transform.FindChild("GetInfo_tab/scrollview/item").gameObject;
                List<SXML> Xml = XMLMgr.instance.GetSXMLList("jjc.reward");
                int length = Xml.Count;
                for (int i = length; i > 0; i--)
                {
                    GameObject clon = (GameObject)GameObject.Instantiate(item);
                    SXML itXml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + i);
                    string name = itXml.getString("name");
                    Image icon = clon.transform.FindChild("icon").GetComponent<Image>();
                    if (i < 10)
                    {
                        icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + i);
                    }
                    else
                    {
                        icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + i);
                    }
                    string color = "";
                    if (i >= 10)
                    {
                        color = "<color=#FFA500>";
                    }
                    else if (i < 10 && i >= 7)
                    {
                        color = "<color=#FF00FF>";
                    }
                    else if (i < 7)
                    {
                        color = "<color=#00BFFF>";
                    }
                    if (i == 1)
                    {
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_bangzuan") + "</color>" + 0;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_mingwang") + "</color>" + 0;
                        clon.transform.FindChild("4/Text").GetComponent<Text>().text = color + ContMgr.getCont("FriendProxy_wu") + "</color>";
                        clon.transform.FindChild("4/icon").gameObject.SetActive(false);
                    }
                    else
                    {
                        int zhuanshi = itXml.GetNode("gem").getInt("num");
                        int mingwang = itXml.GetNode("rep").getInt("num");
                        int box = itXml.GetNode("box").getInt("id");
                        var xml = XMLMgr.instance.GetSXML("item.item", "id==" + box);
                        string bx = xml.getString("item_name");
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_bangzuan") + "</color>" + zhuanshi;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = "<color=#00BFFF>" + ContMgr.getCont("a3_active_mingwang") + "</color>" + mingwang;
                        clon.transform.FindChild("4/Text").GetComponent<Text>().text = color + bx + "</color>";
                        clon.transform.FindChild("4/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + xml.getString("icon_file"));
                        new BaseButton(clon.transform.FindChild("4")).onClick = (GameObject go) =>
                        {
                            showtip((uint)box);
                        };
                    }
                    clon.transform.FindChild("1/Text").GetComponent<Text>().text = color + name + "</color>";
                    clon.transform.SetParent(con, false);
                    clon.SetActive(true);
                }
            }
            frist = false;
        }

        void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }
        public void openFind()
        {
            this.transform.FindChild("find_info").gameObject.SetActive(false);
            this.transform.FindChild("Finding").gameObject.SetActive(true);
        }
        public void CloseFind()
        {
            this.transform.FindChild("find_info").gameObject.SetActive(true);
            this.transform.FindChild("Finding").gameObject.SetActive(false);
        }
        public void refCount_buy(int Buy_Count)
        {
            refCount();
            reffind();
        }

        void ref_zuan_count()
        {
            buy_Count_zuan.text = a3_sportsModel.getInstance().buy_zuan_count.ToString();
        }
        public void refStar(int Count)
        {
            if (a3_sportsModel.getInstance().grade <= 0)
                return;
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + a3_sportsModel.getInstance().grade);
            int pointCount = Xml.getInt("star");
            if (pointCount <= 0)
                return;
            Transform star = this.transform.FindChild("info/star");
            for (int i = 0; i < star.childCount; i++)
            {
                star.GetChild(i).FindChild("this").gameObject.SetActive(false);
                star.GetChild(i).gameObject.SetActive(false);
            }
            for (int m = pointCount; m > 0; m--)
            {
                star.GetChild(m - 1).gameObject.SetActive(true);
            }
            for (int j = 0; j < Count; j++)
            {
                star.GetChild(j).FindChild("this").gameObject.SetActive(true);
            }
        }
        public void refCount(int Count)
        {
            findCount.text = (a3_sportsModel.getInstance().callenge_cnt - Count + a3_sportsModel.getInstance().buyCount) + "/" + a3_sportsModel.getInstance().callenge_cnt;
            reffind();
        }
        //初始化界面
        public void refInto()
        {
            //this.transform.FindChild("GetReward/tem1").gameObject.SetActive(true);
            //this.transform.FindChild("GetReward/tem2").gameObject.SetActive(false);
            //this.transform.FindChild("GetReward").gameObject.SetActive(false);
            this.transform.FindChild("find_info").gameObject.SetActive(true);
            this.transform.FindChild("Finding").gameObject.SetActive(false);
        }
        void Refresh(GameEvent e = null)
        {
            Variant v = e.data;
            if (v["grade"] <= 0)
                return;
            top_saiji.text = ContMgr.getCont("di") + v["tour_time"] + ContMgr.getCont("saiji");
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + v["grade"]);
            duanwei.text = ContMgr.getCont("duanwei") + Xml.getString("name");
            if (v["grade"] < 10)
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + v["grade"]);
            else
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + v["grade"]);

            setGift();
            refCount();
            refStar(v["score"]);
            reffind();
        }

        void ReGet(GameEvent e)
        {
            setGift();
        }

        bool toGet = false;
        void setGift()
        {
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + a3_sportsModel.getInstance().lastgrage);
            this.transform.FindChild("Gift/last_rank").GetComponent<Text>().text = Xml.getString("name");
            transform.FindChild("Gift/rew").gameObject.SetActive(false);
            transform.FindChild("Gift/geted").gameObject.SetActive(false);
            transform.FindChild("Gift/nullrew").gameObject.SetActive(false);
            this.transform.FindChild("reward").gameObject.SetActive(true);
            if (a3_sportsModel.getInstance().lastgrage > 1)
            {
                if (a3_sportsModel.getInstance().Canget <= 0)
                {
                    //有奖励
                    this.transform.FindChild("reward/tet").GetComponent<Text>().text = ContMgr.getCont("lingqu");
                    this.transform.FindChild("reward").GetComponent<Button>().interactable = true;
                    transform.FindChild("Gift/rew").gameObject.SetActive(true);
                    uint boxid = (uint)Xml.GetNode("box").getInt("id");
                    var xml = XMLMgr.instance.GetSXML("item.item", "id==" + boxid);
                    int zhuanshi = Xml.GetNode("gem").getInt("num");
                    int mingwang = Xml.GetNode("rep").getInt("num");
                    transform.FindChild("Gift/rew/gem/count").GetComponent<Text>().text = zhuanshi.ToString();
                    transform.FindChild("Gift/rew/rep/count").GetComponent<Text>().text = mingwang.ToString();
                    Image icon = transform.FindChild("Gift/rew/box").GetComponent<Image>();
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + xml.getString("icon_file"));
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        showtip(boxid);
                    };
                }
                else
                {
                    this.transform.FindChild("reward").GetComponent<Button>().interactable = false;

                    this.transform.FindChild("reward/tet").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                    transform.FindChild("Gift/geted").gameObject.SetActive(true);
                }
            }
            else
            {
                //无奖励
                this.transform.FindChild("reward").gameObject.SetActive(false);
                transform.FindChild("Gift/nullrew").gameObject.SetActive(true);
            }
            //Image icon = clon.transform.FindChild("icon").GetComponent<Image>();

        }
        public void refCount()
        {
            findCount.text = (a3_sportsModel.getInstance().callenge_cnt - a3_sportsModel.getInstance().pvpCount + a3_sportsModel.getInstance().buyCount) + "/" + a3_sportsModel.getInstance().callenge_cnt;
            buyCount.text = a3_sportsModel.getInstance().buyCount + "/ " + a3_sportsModel.getInstance().buy_cnt;
        }
        void reffind()
        {
            if (a3_sportsModel.getInstance().callenge_cnt - a3_sportsModel.getInstance().pvpCount + a3_sportsModel.getInstance().buyCount <= 0)
            {
                findid = 1;
                this.transform.FindChild("find_info/find/text1").gameObject.SetActive(false);
                this.transform.FindChild("find_info/find/text2").gameObject.SetActive(true);
            }
            else
            {
                this.transform.FindChild("find_info/find/text1").gameObject.SetActive(true);
                this.transform.FindChild("find_info/find/text2").gameObject.SetActive(false);
                findid = 0;
            }
        }
        public void refro_score()
        {
            if (a3_sportsModel.getInstance().grade <= 0)
                return;
            SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + a3_sportsModel.getInstance().grade);
            duanwei.text = "段位：" + Xml.getString("name");
            if (a3_sportsModel.getInstance().grade < 10)
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + a3_sportsModel.getInstance().grade);
            else
                rank.sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + a3_sportsModel.getInstance().grade);
            refStar(a3_sportsModel.getInstance().score);
        }
    }
}
