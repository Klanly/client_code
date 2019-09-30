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
using DG.Tweening;

namespace MuGame
{
    class a3_ranking : Window
    {
        TabControl tabCtrl1;
        int tabIdx;
        private GameObject[] rank_tabs;
        List<GameObject> tab = new List<GameObject>();
        Transform zhanliCon;
        Transform lvlCon;
        Transform chiBangCon;
        Transform junTuanCon;
        Transform summonCon;
        Transform spostCon;
        Text time_text;
        //GameObject isthis;

        public GameObject Toback = null;
        public GameObject showAvt = null;

        GameObject tip1;

        public static a3_ranking instan;
        public static a3_ranking isshow;
        GameObject wait;


        ScrollControler scrollControler;
        ScrollControler scrollControler1;
        ScrollControler scrollControler2;
        ScrollControler scrollControler3;
        public override void init()
        {

            #region 初始化汉字
            getComponentByPath<Text>("txt").text = ContMgr.getCont("a3_ranking_0");
            getComponentByPath<Text>("rank_tab1/topText/Text").text = ContMgr.getCont("a3_ranking_1");//排名
            getComponentByPath<Text>("rank_tab1/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");//昵称
            getComponentByPath<Text>("rank_tab1/topText/Text (2)").text = ContMgr.getCont("a3_ranking_3");//等级
            getComponentByPath<Text>("rank_tab1/topText/Text (3)").text = ContMgr.getCont("a3_ranking_4");//战力
            getComponentByPath<Text>("rank_tab2/topText/Text").text = ContMgr.getCont("a3_ranking_1");
            getComponentByPath<Text>("rank_tab2/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");
            getComponentByPath<Text>("rank_tab2/topText/Text (2)").text = ContMgr.getCont("a3_ranking_3");
            getComponentByPath<Text>("rank_tab2/topText/Text (3)").text = ContMgr.getCont("a3_ranking_4");
            getComponentByPath<Text>("rank_tab3/topText/Text").text = ContMgr.getCont("a3_ranking_1");
            getComponentByPath<Text>("rank_tab3/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");
            getComponentByPath<Text>("rank_tab3/topText/Text (2)").text = ContMgr.getCont("a3_ranking_15");//名称
            getComponentByPath<Text>("rank_tab3/topText/Text (3)").text = ContMgr.getCont("a3_ranking_16");//品阶
            getComponentByPath<Text>("rank_tab4/topText/Text").text = ContMgr.getCont("a3_ranking_1");
            getComponentByPath<Text>("rank_tab4/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");
            getComponentByPath<Text>("rank_tab4/topText/Text (2)").text = ContMgr.getCont("a3_ranking_3");
            getComponentByPath<Text>("rank_tab4/topText/Text (3)").text = ContMgr.getCont("a3_ranking_4");
            getComponentByPath<Text>("rank_tab5/topText/Text").text = ContMgr.getCont("a3_ranking_1");
            getComponentByPath<Text>("rank_tab5/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");
            getComponentByPath<Text>("rank_tab5/topText/Text (2)").text = ContMgr.getCont("a3_ranking_17");//种类
            getComponentByPath<Text>("rank_tab5/topText/Text (3)").text = ContMgr.getCont("a3_ranking_3");
            getComponentByPath<Text>("rank_tab6/topText/Text").text = ContMgr.getCont("a3_ranking_1");
            getComponentByPath<Text>("rank_tab6/topText/Text (1)").text = ContMgr.getCont("a3_ranking_2");
            getComponentByPath<Text>("rank_tab6/topText/Text (2)").text = ContMgr.getCont("a3_ranking_18");//积分
            getComponentByPath<Text>("rank_tab6/topText/Text (3)").text = ContMgr.getCont("a3_ranking_4");
            getComponentByPath<Text>("panelTab2/con/0/Text").text = ContMgr.getCont("a3_ranking_4");
            getComponentByPath<Text>("panelTab2/con/1/Text").text = ContMgr.getCont("a3_ranking_3");
            getComponentByPath<Text>("panelTab2/con/2/Text").text = ContMgr.getCont("a3_ranking_6");
            getComponentByPath<Text>("panelTab2/con/3/Text").text = ContMgr.getCont("a3_ranking_7");
            getComponentByPath<Text>("panelTab2/con/4/Text").text = ContMgr.getCont("a3_ranking_8");
            getComponentByPath<Text>("panelTab2/con/5/Text").text = ContMgr.getCont("a3_ranking_9");
            getComponentByPath<Text>("tip/look/Text").text = ContMgr.getCont("a3_ranking_10");
            getComponentByPath<Text>("tip/lookWing/Text").text = ContMgr.getCont("a3_ranking_11");
            getComponentByPath<Text>("tip/looksum/Text").text = ContMgr.getCont("a3_ranking_12");
            getComponentByPath<Text>("tip/talk/Text").text = ContMgr.getCont("a3_ranking_13");
            getComponentByPath<Text>("tip/add/Text").text = ContMgr.getCont("a3_ranking_14");
            #endregion








            instan = this;          
            rank_tabs = new GameObject[6];
            rank_tabs[0] = transform.FindChild("rank_tab1").gameObject;
            rank_tabs[1] = transform.FindChild("rank_tab2").gameObject;
            rank_tabs[2] = transform.FindChild("rank_tab3").gameObject;
            rank_tabs[3] = transform.FindChild("rank_tab4").gameObject;
            rank_tabs[4] = transform.FindChild("rank_tab5").gameObject;
            rank_tabs[5] = transform.FindChild("rank_tab6").gameObject;
            wait = transform.FindChild("wait").gameObject;

            zhanliCon = rank_tabs[0].transform.FindChild("panel/scroll_rect/contain");
            lvlCon = rank_tabs[1].transform.FindChild("panel/scroll_rect/contain");
            chiBangCon = rank_tabs[2].transform.FindChild("panel/scroll_rect/contain");
            junTuanCon = rank_tabs[3].transform.FindChild("panel/scroll_rect/contain");
            summonCon = rank_tabs[4].transform.FindChild("panel/scroll_rect/contain");
            spostCon = rank_tabs[5].transform.FindChild("panel/scroll_rect/contain");
            tip1 = this.transform.FindChild("tip").gameObject;
            new BaseButton(tip1.transform.FindChild("close")).onClick = (GameObject go) => 
            {
                tip1.SetActive(false);
            };

            time_text = this.transform.FindChild("time").GetComponent<Text>();
           // isthis = this.transform.FindChild("this").gameObject;

            for (int i =0;i<transform.FindChild ("panelTab2/con").childCount;i++)
            {
                tab.Add(transform.FindChild("panelTab2/con").GetChild(i).gameObject);
            }

            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("rank_tab1/panel/scroll_rect").GetComponent<ScrollRect>();
            scrollControler.create(scroll);

            scrollControler1 = new ScrollControler();
            ScrollRect scroll1 = transform.FindChild("rank_tab2/panel/scroll_rect").GetComponent<ScrollRect>();
            scrollControler1.create(scroll1);

            scrollControler2 = new ScrollControler();
            ScrollRect scroll2 = transform.FindChild("rank_tab3/panel/scroll_rect").GetComponent<ScrollRect>();
            scrollControler2.create(scroll2);

            scrollControler3 = new ScrollControler();
            ScrollRect scroll3 = transform.FindChild("rank_tab5/panel/scroll_rect").GetComponent<ScrollRect>();
            scrollControler3.create(scroll3);


            for (int i = 0; i < tab.Count; i++)
            {
                int tag = i;
                new BaseButton(tab[i].transform).onClick = delegate (GameObject go)
                {
                    onTab(tag);
                };
            }
            new BaseButton(transform.FindChild("btn_close")).onClick = onClose;

            setTip_1_btn();
        }

        public override void onShowed()
        {
            wait.SetActive(false);
            a3_rankingProxy.getInstance().addEventListener(a3_rankingProxy.EVENT_INFO ,OnRank);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);

            if (uiData != null && uiData.Count > 0)
            {
                onTab((int)uiData[0]);
            }
            else
            {
                onTab(0);
            }
            isshow = this;
            a3_rankingProxy.getInstance().getTime();
            isOpen();
        }

        void isOpen()
        {
            this.transform.FindChild("panelTab2/con/4").gameObject.SetActive(false);

            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
            {
                this.transform.FindChild("panelTab2/con/4").gameObject.SetActive(true);
            }
       }


        public override void onClosed() 
        {
            a3_rankingProxy.getInstance().removeEventListener(a3_rankingProxy.EVENT_INFO, OnRank);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            isshow = null;
            disposeAvatar();
        }

        void OnRank(GameEvent v)
        {
            Variant data = v.data;
            int res = data["res"];
            if (data.ContainsKey("ranks")) {        
                switch (tabIdx)
                {
                    case 0:
                        addrank_zhanli(a3_rankingModel.getInstance().zhanli);
                        break;
                    case 1:
                        addrank_lvl(a3_rankingModel.getInstance().lvl);
                        break;
                    case 2:
                        addrank_chibang(a3_rankingModel.getInstance().wing);
                        break;
                    case 3:
                        addrank_juntuan(a3_rankingModel.getInstance().juntuan);
                        break;
                    case 4:
                        addrank_summon(a3_rankingModel.getInstance().summon);
                        break;
                    case 5:
                        addrank_spost(a3_rankingModel .getInstance ().spost);
                        break;
                }
            }
        }

        

        void onTab( int tag)
        {
            tabIdx = tag;
            rank_tabs[0].SetActive(false);
            rank_tabs[1].SetActive(false);
            rank_tabs[2].SetActive(false);
            rank_tabs[3].SetActive(false);
            rank_tabs[4].SetActive(false);
            rank_tabs[5].SetActive(false);
            tip1.SetActive(false);
            tip1.transform.FindChild("look").gameObject.SetActive(false);
            tip1.transform.FindChild("looksum").gameObject.SetActive(false);
            tip1.transform.FindChild("lookWing").gameObject.SetActive(false);
            //disposeAvatar();
            for (int i = 0; i < tab.Count; i++)                                                 
            {
                tab[i].GetComponent<Button>().interactable = true;
            }
            tab[tabIdx].GetComponent<Button>().interactable = false;
            rank_tabs[tabIdx].SetActive(true);
            switch (tabIdx)
            {
                case 0:
                    tip1.SetActive(true);
                    tip1.transform.FindChild("look").gameObject.SetActive(true);
                    if (a3_rankingModel.getInstance().zhanli_frist)
                    {
                        for (int i = 0; i < zhanliCon.childCount; i++)
                        {
                            Destroy(zhanliCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(1, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().zhanli_frist = false;
                    }
                    else
                    {
                        zhanliCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(zhanliCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if (zhanliCon.childCount <= 0)
                            addrank_zhanli(a3_rankingModel.getInstance().zhanli);
                        else {
                            setPlayer_Avt(a3_rankingModel.getInstance().zhanli[1].cid);
                            setSelect(zhanliObj, 1);
                            setPow_zhanli(1);
                        }
                    }
                    break;
                case 1:
                    tip1.SetActive(true);
                    tip1.transform.FindChild("look").gameObject.SetActive(true);
                    if (a3_rankingModel.getInstance().lvl_frist)
                    {
                        for (int i = 0; i < lvlCon.childCount; i++)
                        {
                            Destroy(lvlCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(2, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().lvl_frist = false;
                    }
                    else
                    {
                        lvlCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if (lvlCon.childCount <= 0)
                            addrank_lvl(a3_rankingModel.getInstance().lvl);
                        else {
                            setPlayer_Avt(a3_rankingModel.getInstance().lvl[1].cid);
                            setSelect(lvlObj, 1);
                            setPow_lvl(1);
                        }
                    }
                    break;
                case 2:
                    tip1.SetActive(true);
                    tip1.transform.FindChild("lookWing").gameObject.SetActive(true);
                    if (a3_rankingModel.getInstance().chibang_frist)
                    {
                        for (int i = 0; i < chiBangCon.childCount; i++)
                        {
                            Destroy(chiBangCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(3, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().chibang_frist = false;
                    }
                    else
                    {
                        chiBangCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(chiBangCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if (chiBangCon.childCount <= 0)
                            addrank_chibang(a3_rankingModel.getInstance().wing);
                        else {
                            setPlayer_Avt(a3_rankingModel.getInstance().wing[1].cid, a3_rankingModel.getInstance().wing[1].stage);
                            setSelect(wingObj, 1);
                            cuicarr = (int)a3_rankingModel.getInstance().wing[1].carr;
                            cuiwingstage = a3_rankingModel.getInstance().wing[1].stage;
                            cuiwinglvl = a3_rankingModel.getInstance().wing[1].flylvl;
                        }
                    }
                    break;
                case 3:
                    if (a3_rankingModel.getInstance().juntuan_frist)
                    {
                        for (int i = 0; i < junTuanCon.childCount; i++)
                        {
                            Destroy(junTuanCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(4, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().juntuan_frist = false;
                    }
                    else
                    {
                        junTuanCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(junTuanCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if(junTuanCon.childCount <= 0)
                            addrank_juntuan(a3_rankingModel.getInstance().juntuan);
                        else
                            setSelect(juntuanObj, 1);
                    }
                    break;
                case 4:
                    tip1.SetActive(true);
                    tip1.transform.FindChild("looksum").gameObject.SetActive(true);
                    if (a3_rankingModel.getInstance().summon_frist)
                    {
                        for (int i = 0; i < summonCon.childCount; i++)
                        {
                            Destroy(summonCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(5, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().summon_frist = false;
                    }
                    else
                    {
                        summonCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(summonCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if (summonCon.childCount <= 0)
                            addrank_summon(a3_rankingModel.getInstance().summon);
                        else {
                            setSummon_Avt(a3_rankingModel.getInstance().summon[1].zhs_tpid);
                            setSelect(summonObj, 1);
                            setPow_sum(1);
                        }
                    }
                    break;

                case 5:
                    tip1.SetActive(true);
                    tip1.transform.FindChild("look").gameObject.SetActive(true);
                    if (a3_rankingModel.getInstance().spost_frist)
                    {
                        for (int i = 0; i < spostCon.childCount; i++)
                        {
                            Destroy(spostCon.GetChild(i).gameObject);
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            a3_rankingProxy.getInstance().send_Getinfo(7, (uint)i, 1);
                        }
                        a3_rankingModel.getInstance().spost_frist = false;
                    }
                    else {

                        spostCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(spostCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
                        if (spostCon.childCount <= 0)
                            addrank_spost(a3_rankingModel.getInstance().spost);
                        else
                        {
                            setPlayer_Avt(a3_rankingModel.getInstance().spost[1].cid);
                            setSelect(spostObj, 1);
                            setPow_spost(1);
                        }
                    }
                    break;
            }
        }

      //  Vector3 v = new Vector3();
        public void Getinfo_panel(List<RankingData> l,int type)
        {
            switch (type)
            {
                case 1:
                    //addrank_zhanli(l);
                    break;
                case 2:
                    //addrank_lvl(l);
                    break;
                case 3:
                   // addrank_chibang(l);
                    break;
                case 4:
                  //  addrank_juntuan(l);
                    break;
                case 5:
                   // addrank_summon(l);
                    break;
            }  
        }
        public void refresh_myRank(int type,int rank)
        {
            //if (rank <= 0)
            //{
            //    rank_tabs[type - 1].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_no");
            //}
            //else
            {
                switch (type)
                {
                    case 1:
                        rank_tabs[type - 1].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_my") + rank;
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        if (rank <= 0 || rank > 5000)
                        {
                            rank_tabs[type - 1].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_no");
                        }
                        else
                        {
                            rank_tabs[type - 1].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_my") + rank;
                        }
                        break;

                    case 7:
                        if (rank <= 0 || rank > 5000)
                        {
                            rank_tabs[5].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_no");
                        }
                        else {
                            rank_tabs[5].transform.FindChild("myrank").GetComponent<Text>().text = ContMgr.getCont("a3_ranking_my") + rank;
                        }
                        break;
                }
            }

        }


        Dictionary<uint, GameObject> zhanliObj = new Dictionary<uint, GameObject>();

        void addrank_zhanli(Dictionary <uint,RankingData> data)
        {
            if (zhanliCon.childCount > 0)
            {
                for (int i = 0; i < zhanliCon.childCount; i++)
                {
                    Destroy(zhanliCon.GetChild(i).gameObject);
                }
            }
            zhanliObj.Clear();
            GameObject item = rank_tabs[0].transform.FindChild("panel/scroll_rect/item_zhanli").gameObject;
            for (uint i = 1;i<=data.Count;i++)
            {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(zhanliCon, false);

                clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString ();
                if (one.rank <= 3)
                {
                    Vector2 size = new Vector2();
                    switch (one.rank)
                    {
                        case 1:
                            size = new Vector2(88f, 48);
                            break;
                        case 2:
                            size = new Vector2(72f, 48);
                            break;
                        case 3:
                            size = new Vector2(60f, 48);
                            break;
                    }
                    clon.transform.FindChild("1/rankbg").GetComponent<RectTransform>().sizeDelta = size;
                   string file = "icon_ranking_rank_" + one.rank;
                    clon.transform.FindChild("1/rankbg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(true);
                    string file_bg = "icon_ranking_rank_di_"+ one.rank;
                    clon.transform.FindChild ("bg").GetComponent <Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                    clon.transform.FindChild("bg").gameObject.SetActive(true);
                }
                else
                {
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(false);
                    clon.transform.FindChild("bg").gameObject.SetActive(false);
                }

                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                if (one.viplvl <= 0)
                    clon.transform.FindChild("2/vip").gameObject.SetActive(false);
                else
                {
                    clon.transform.FindChild("2/vip").gameObject.SetActive(true);
                    clon.transform.FindChild("2/vip").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_vip_" + one.viplvl);
                }
                clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.zhuan+ ContMgr.getCont("zhuan") + one.lvl + ContMgr.getCont("ji");
               // clon.transform.FindChild("4/Text").GetComponent<Text>().text = one.combpt.ToString();
                clon.transform.SetAsFirstSibling();

                zhanliObj[i] = clon;
                uint cid = one.cid;
                string n = one.name;
                uint ide = i;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    cuiId = cid;
                    cuiName = n;
                    //saveAvt(cuiId);
                    if (m_SelfObj != null) m_SelfObj.SetActive(false);
                    setPlayer_Avt(cuiId);
                    setSelect(zhanliObj, ide);
                    setPow_zhanli(one.rank);
                };
            }
            zhanliCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(zhanliCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            //saveAvt(data[1].cid);
            if (m_SelfObj != null) m_SelfObj.SetActive(false);
            setPlayer_Avt(data[1].cid);
            setSelect(zhanliObj,1);
            setPow_zhanli(1);
        }

        Dictionary<uint, GameObject> spostObj = new Dictionary<uint, GameObject>();
        void addrank_spost(Dictionary<uint, RankingData> data)
        {
            if (spostCon.childCount > 0)
            {
                for (int i = 0; i < spostCon.childCount; i++)
                {
                    Destroy(spostCon.GetChild(i).gameObject);
                }
            }
            spostObj.Clear();
            GameObject item = rank_tabs[5].transform.FindChild("panel/scroll_rect/item_spost").gameObject;
            for (uint i = 1; i <= data.Count; i++) {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(spostCon, false);
                clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                if (one.rank <= 3)
                {
                    Vector2 size = new Vector2();
                    switch (one.rank)
                    {
                        case 1:
                            size = new Vector2(88f, 48);
                            break;
                        case 2:
                            size = new Vector2(72f, 48);
                            break;
                        case 3:
                            size = new Vector2(60f, 48);
                            break;
                    }
                    clon.transform.FindChild("1/rankbg").GetComponent<RectTransform>().sizeDelta = size;
                    string file = "icon_ranking_rank_" + one.rank;
                    clon.transform.FindChild("1/rankbg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(true);
                    string file_bg = "icon_ranking_rank_di_" + one.rank;
                    clon.transform.FindChild("bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                    clon.transform.FindChild("bg").gameObject.SetActive(true);
                }
                else
                {
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(false);
                    clon.transform.FindChild("bg").gameObject.SetActive(false);
                }
                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                if (one.viplvl <= 0)
                    clon.transform.FindChild("2/vip").gameObject.SetActive(false);
                else
                {
                    clon.transform.FindChild("2/vip").gameObject.SetActive(true);
                    clon.transform.FindChild("2/vip").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_vip_" + one.viplvl);
                }

                clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.piont .ToString() ;
                clon.transform.SetAsFirstSibling();
                spostObj[i] = clon;
                uint cid = one.cid;
                string n = one.name;
                uint ide = i;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    cuiId = cid;
                    cuiName = n;
                    if (m_SelfObj != null) m_SelfObj.SetActive(false);
                    setPlayer_Avt(cuiId);
                    setSelect(spostObj, ide);
                    setPow_spost(one.rank);
                };
            }
            spostCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(spostCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            if (m_SelfObj != null) m_SelfObj.SetActive(false);
            setPlayer_Avt(data[1].cid);
            setSelect(spostObj, 1);
            setPow_spost(1);
        }

        Dictionary<uint, GameObject> lvlObj = new Dictionary<uint, GameObject>();
        void addrank_lvl(Dictionary<uint, RankingData> data)
        {
            if (lvlCon.childCount > 0)
            {
                for (int i = 0; i < lvlCon.childCount; i++)
                {
                    Destroy(lvlCon.GetChild(i).gameObject);
                }
            }
            lvlObj.Clear();
            GameObject item = rank_tabs[1].transform.FindChild("panel/scroll_rect/item_lvl").gameObject;
            for (uint i = 1; i <= data.Count; i++)
            {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(lvlCon, false);
                clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString() ;
                if (one.rank <= 3)
                {
                    Vector2 size = new Vector2();
                    switch (one.rank)
                    {
                        case 1:
                            size = new Vector2(88f, 48);
                            break;
                        case 2:
                            size = new Vector2(72f, 48);
                            break;
                        case 3:
                            size = new Vector2(60f, 48);
                            break;
                    }
                    clon.transform.FindChild("1/rankbg").GetComponent<RectTransform>().sizeDelta = size;
                    string file = "icon_ranking_rank_" + one.rank;
                    clon.transform.FindChild("1/rankbg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(true);
                    string file_bg = "icon_ranking_rank_di_" + one.rank;
                    clon.transform.FindChild("bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                    clon.transform.FindChild("bg").gameObject.SetActive(true);
                }
                else
                {
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(false);
                    clon.transform.FindChild("bg").gameObject.SetActive(false);
                }
                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                if (one.viplvl <= 0)
                    clon.transform.FindChild("2/vip").gameObject.SetActive(false);
                else
                {
                    clon.transform.FindChild("2/vip").gameObject.SetActive(true);
                    clon.transform.FindChild("2/vip").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_vip_" + one.viplvl);
                }
                clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.zhuan + ContMgr.getCont("zhuan") + one.lvl + ContMgr.getCont("ji");
                //clon.transform.FindChild("4/Text").GetComponent<Text>().text = one.combpt.ToString();
                clon.transform.SetAsFirstSibling();
                lvlObj[i] = clon;
                uint cid = one.cid;
                string n = one.name;
                uint ide = i;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    cuiId = cid;
                    cuiName = n;
                    //saveAvt(cuiId);
                    if (m_SelfObj != null) m_SelfObj.SetActive(false);
                    setPlayer_Avt(cuiId);
                    setSelect(lvlObj, ide);
                    setPow_lvl(one.rank);
                };
            }
            lvlCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(lvlCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            //saveAvt(data[1].cid);
            if (m_SelfObj != null) m_SelfObj.SetActive(false);
            setPlayer_Avt(data[1].cid);
            setSelect(lvlObj,1);
            setPow_lvl(1);
        }
        Dictionary<uint, GameObject> wingObj = new Dictionary<uint, GameObject>();
        void addrank_chibang(Dictionary<uint, RankingData> data)
        {
            if (chiBangCon.childCount > 0)
            {
                for (int i = 0; i < chiBangCon.childCount; i++)
                {
                    Destroy(chiBangCon.GetChild(i).gameObject);
                }
            }
            wingObj.Clear();
            GameObject item = rank_tabs[2].transform.FindChild("panel/scroll_rect/item_chibang").gameObject;
            for (uint i = 1; i <= data.Count; i++)
            {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(chiBangCon, false);
                clon.transform.FindChild("1/Text").GetComponent<Text>().text =  one.rank .ToString ();
                if (one.rank <= 3)
                {
                    Vector2 size = new Vector2();
                    switch (one.rank)
                    {
                        case 1:
                            size = new Vector2(88f, 48);
                            break;
                        case 2:
                            size = new Vector2(72f, 48);
                            break;
                        case 3:
                            size = new Vector2(60f, 48);
                            break;
                    }
                    clon.transform.FindChild("1/rankbg").GetComponent<RectTransform>().sizeDelta = size;
                    string file = "icon_ranking_rank_" + one.rank;
                    clon.transform.FindChild("1/rankbg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(true);
                    string file_bg = "icon_ranking_rank_di_" + one.rank;
                    clon.transform.FindChild("bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                    clon.transform.FindChild("bg").gameObject.SetActive(true);
                }
                else
                {
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(false);
                    clon.transform.FindChild("bg").gameObject.SetActive(false);
                }
                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                if (one.viplvl <= 0)
                    clon.transform.FindChild("2/vip").gameObject.SetActive(false);
                else
                {
                    clon.transform.FindChild("2/vip").gameObject.SetActive(true);
                    clon.transform.FindChild("2/vip").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_vip_" + one.viplvl);
                }
                SXML xml = XMLMgr.instance.GetSXML("wings.wing", "career==" + one.carr);
                SXML x = xml.GetNode("wing_stage", "stage_id=="+one.stage);
                if (x == null) {
                    Destroy(clon);
                    continue;
                }
                clon.transform.FindChild("3/Text").GetComponent<Text>().text = x.getString("name");
                clon.transform.FindChild("4/Text").GetComponent<Text>().text = one.stage+ ContMgr.getCont("a3_equip_jie") + one.flylvl + ContMgr.getCont("xing");
                clon.transform.SetAsFirstSibling();
                wingObj[i] = clon;
                uint carr = one.carr;
                int wingstage = one.stage;
                int winglvl = one.flylvl;
                uint ide = i;
                uint cuid = one.cid;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    cuicarr = (int)carr;
                    cuiwingstage = wingstage;
                    cuiwinglvl = winglvl;
                    setPlayer_Avt(cuid, cuiwingstage);
                    setSelect(wingObj, ide);
                };
            }
            chiBangCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(chiBangCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            setPlayer_Avt(data[1].cid, data[1].stage);
            setSelect(wingObj,1);

            cuicarr = (int)data[1].carr;
            cuiwingstage = data[1].stage;
            cuiwinglvl = data[1].flylvl;
        }

        Dictionary<uint, GameObject> juntuanObj = new Dictionary<uint, GameObject>();
        void addrank_juntuan(Dictionary<uint, RankingData> data)
        {
            if (junTuanCon.childCount > 0)
            {
                for (int i = 0; i < junTuanCon.childCount; i++)
                {
                    Destroy(junTuanCon.GetChild(i).gameObject);
                }
            }
            juntuanObj.Clear();
            GameObject item = rank_tabs[3].transform.FindChild("panel/scroll_rect/item_juntuan").gameObject;
            for (uint i = 1; i <= data.Count; i++)
            {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(junTuanCon, false);
                clon.transform.FindChild("1/Text").GetComponent<Text>().text = ContMgr.getCont("di") + one.rank + ContMgr.getCont("ming");
                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.jt_name;
                clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.jt_combpt.ToString ();
                clon.transform.FindChild("4/Text").GetComponent<Text>().text = one.jt_lvl + ContMgr.getCont("ji");
                clon.transform.SetAsFirstSibling();
                juntuanObj[i] = clon;
                uint ide = i;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    setSelect(juntuanObj, ide);
                    // isthis.SetActive(true);
                    // isthis.transform.SetParent(clon.transform, false);
                };
            }
            junTuanCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(junTuanCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            setSelect(juntuanObj,1);
        }
        Dictionary<uint, GameObject> summonObj = new Dictionary<uint, GameObject>();
        void addrank_summon(Dictionary<uint, RankingData> data)
        {
            if (summonCon.childCount > 0)
            {
                for (int i = 0; i < summonCon.childCount; i++)
                {
                    Destroy(summonCon.GetChild(i).gameObject);
                }
            }
            summonObj.Clear();
            GameObject item = rank_tabs[4].transform.FindChild("panel/scroll_rect/item_summon").gameObject;
            for (uint i = 1; i <= data.Count; i++)
            {
                RankingData one = data[i];
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                clon.transform.SetParent(summonCon, false);
                clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString ();
                if (one.rank <= 3)
                {
                    Vector2 size = new Vector2();
                    switch (one.rank)
                    {
                        case 1:
                            size = new Vector2(88f, 48);
                            break;
                        case 2:
                            size = new Vector2(72f, 48);
                            break;
                        case 3:
                            size = new Vector2(60f, 48);
                            break;
                    }
                    clon.transform.FindChild("1/rankbg").GetComponent<RectTransform>().sizeDelta = size;
                    string file = "icon_ranking_rank_" + one.rank;
                    clon.transform.FindChild("1/rankbg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(true);
                    string file_bg = "icon_ranking_rank_di_" + one.rank;
                    clon.transform.FindChild("bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                    clon.transform.FindChild("bg").gameObject.SetActive(true);
                }
                else
                {
                    clon.transform.FindChild("1/rankbg").gameObject.SetActive(false);
                    clon.transform.FindChild("bg").gameObject.SetActive(false);
                }
                clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                if (one.viplvl <= 0)
                    clon.transform.FindChild("2/vip").gameObject.SetActive(false);
                else
                {
                    clon.transform.FindChild("2/vip").gameObject.SetActive(true);
                    clon.transform.FindChild("2/vip").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_vip_" + one.viplvl);
                }
                SXML sumxml = XMLMgr.instance.GetSXML("callbeast.callbeast", "id==" + one.zhs_tpid);
                SXML attxml = sumxml.GetNode("star", "star_sum==" + one.talent);
                int tpid_m = attxml.getInt("info_itm");
                SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid_m);
                clon.transform.FindChild("3/Text").GetComponent<Text>().text = xml.getString("item_name");
                clon.transform.FindChild("4/Text").GetComponent<Text>().text = one.zhs_lvl+ ContMgr.getCont("ji");
               // clon.transform.FindChild("5/Text").GetComponent<Text>().text = one.zhs_combpt.ToString();
                clon.transform.SetAsFirstSibling();
                summonObj[i] = clon;
                uint cid = (uint)one.zhs_id;
                string n = one.name;
                uint ide = i;
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    cuiId = cid;
                    cuiName = n;
                    setSummon_Avt(one.zhs_tpid);
                    setSelect(summonObj, ide);
                    setPow_sum(one.rank );
                };
            }
            summonCon.GetComponent<RectTransform>().anchoredPosition = new Vector2(summonCon.GetComponent<RectTransform>().anchoredPosition.x, 0);
            setSummon_Avt(data[1].zhs_tpid);
            setSelect(summonObj,1);
            setPow_sum(1);
        }

        void setPow_zhanli(uint rank)
        {
            uint pow = a3_rankingModel.getInstance().zhanli[rank].combpt;
            rank_tabs[0].transform.FindChild("power/num").GetComponent<Text>().text = pow.ToString ();
            cuiId = a3_rankingModel.getInstance().zhanli[rank].cid;
        }


        void setPow_spost(uint rank) {
            uint pow = a3_rankingModel.getInstance().spost[rank].combpt;
            rank_tabs[5].transform .FindChild ("power/num").GetComponent <Text>().text = pow.ToString();
            cuiId = a3_rankingModel.getInstance().spost[rank].cid;
        }
        void setPow_lvl(uint rank)
        {
            uint pow = a3_rankingModel.getInstance().lvl[rank].combpt;
            rank_tabs[1].transform.FindChild("power/num").GetComponent<Text>().text = pow.ToString();
            cuiId = a3_rankingModel.getInstance().lvl[rank].cid;
        }
        void setPow_sum(uint rank)
        {
            int pow = a3_rankingModel.getInstance().summon[rank].zhs_combpt;
            rank_tabs[4].transform.FindChild("power/num").GetComponent<Text>().text = pow.ToString();
            cuiId = (uint)a3_rankingModel.getInstance().summon[rank].zhs_id;
        }

        void setSelect(Dictionary<uint, GameObject> objCon , uint ide)
        {
            foreach (uint i in objCon.Keys)
            {
                if(i != ide)
                    objCon[i].transform.FindChild("this").gameObject.SetActive(false);
                else
                    objCon[i].transform.FindChild("this").gameObject.SetActive(true);
            }

        }

         
        uint cuiId = 0;
        string cuiName = "";
        int cuicarr = 0;
        int cuiwingstage = 0;
        int cuiwinglvl = 0;
        void setTip_1_btn()
        {
            new BaseButton(tip1.transform.FindChild("looksum")).onClick = (GameObject go) =>
            {
                ArrayList list = new ArrayList();
                list.Add(cuiId);
                Toback = this.gameObject;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMONINFO, list);
                this.gameObject.SetActive(false);
                if (m_SelfObj.activeSelf) showAvt = m_SelfObj;
                m_SelfObj.SetActive(false);
                scene_Camera.SetActive(false);
            };
            new BaseButton(tip1.transform.FindChild("look")).onClick = (GameObject go) =>
            {
                ArrayList list = new ArrayList();
                list.Add(cuiId);
                Toback = this.gameObject;
                FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, list);
                this.gameObject.SetActive(false);
                if (m_SelfObj.activeSelf) showAvt = m_SelfObj;
                m_SelfObj.SetActive(false);


                scene_Camera.SetActive(false);
            };

            new BaseButton(tip1.transform.FindChild("lookWing")).onClick = (GameObject go) => 
            {
                ArrayList list = new ArrayList();
                list.Add(cuicarr);
                list.Add(cuiwingstage);
                list.Add(cuiwinglvl);
                Toback = this.gameObject;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_WINGINFO, list);
                this.gameObject.SetActive(false);
                if (m_SelfObj.activeSelf) showAvt = m_SelfObj;
                m_SelfObj.SetActive(false);
                scene_Camera.SetActive(false);
            };
        }



        int WingStage = -1;

        void setPlayer_Avt(uint tid,int stage = -1) {
            WingStage = stage;
            disposeAvatar();
            wait.SetActive(true);
            if (data_Eve != null) {
                Variant data = data_Eve.data;
                if (data["cid"] == tid)
                {
                    GetInfo(data_Eve);
                }
                else {
                    FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
                    FriendProxy.getInstance().sendgetplayerinfo(tid);
                }
            }
            else
            {
                FriendProxy.getInstance().addEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
                FriendProxy.getInstance().sendgetplayerinfo(tid);
            }
        }
        void setSummon_Avt(int sumid)
        {
            disposeAvatar();
            wait.SetActive(true);
            createAvatar_sum(sumid);
        }
        void setWing_Avt(int carr, int stage)
        {
            disposeAvatar();
            wait.SetActive(true);
            createAvatar_wing(carr, stage);
        }

        public void setTime(float time )
        {
            TimeSpan ts = new TimeSpan(0, 0, (int)time);
            time_text.text = ts.Hours +ContMgr.getCont("hour") + ts.Minutes + ContMgr.getCont("mine") + ts.Seconds + ContMgr.getCont("miao");
        }

        void onClose(GameObject go)
        {
            if (m_SelfObj != null)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RANKING);

                if (a3_sports_jdzc._instan != null && a3_sports_jdzc._instan.goBack)
                {
                    ArrayList arrs = new ArrayList();
                    arrs.Add("sports_jdzc");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS, arrs);
                }
            }
        }

        //================Avator================

        public  GameObject m_SelfObj;//角色的avatar
        public  ProfessionAvatar m_proAvatar;
        public  GameObject scene_Camera;

        #region 人物

        int carr = 0;
        int show_wing = 0;
        uint cuid = 0;

        Dictionary<int, a3_BagItemData> Equips = new Dictionary<int, a3_BagItemData>();
        int[] fashionsshows = new int[2];//时装

        public Dictionary<int, a3_BagItemData> active_eqp = new Dictionary<int, a3_BagItemData>();

        GameEvent data_Eve = null;
        void GetInfo(GameEvent e)
        {
            data_Eve = e;
            Variant data = e.data;

            cuid = data["cid"];
            carr = data["carr"];
            if (data.ContainsKey("show_wing"))
                show_wing = data["show_wing"];

            if (WingStage > 0)
                show_wing = WingStage;
            Variant equips = data["equipments"];
            Equips.Clear();
            active_eqp.Clear();
            foreach (var v in equips._arr)
            {
                a3_BagItemData item = new a3_BagItemData();
                item.confdata.equip_type = v["part_id"];
                Variant info = v["eqpinfo"];
                item.id = info["id"];
                item.tpid = info["tpid"];
                item.confdata = a3_BagModel.getInstance().getItemDataById(item.tpid);
                a3_EquipModel.getInstance().equipData_read(item, info);
                Equips[item.confdata.equip_type] = item;
            }
            if(data["dress_list"]!=null&& data["dress_list"].Count>0)
            {
                fashionsshows[0] = data["dress_list"][0]._int;
                fashionsshows[1] = data["dress_list"][1]._int;
            }
            else
            {
                fashionsshows[0] = fashionsshows[1] = 0;
            }
            foreach (a3_BagItemData item in Equips.Values)
            {
                if (isactive_eqp(item))
                {
                    active_eqp[item.confdata.equip_type] = item;
                }
            }

            createAvatar();
            FriendProxy.getInstance().removeEventListener(FriendProxy.EVENT_LOOKFRIEND, GetInfo);
        }
        
        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }
        public bool isactive_eqp(a3_BagItemData data)
        {
            if (data.equipdata.attribute == 0)
                return false;
            int needTpye_act = a3_EquipModel.getInstance().eqp_type_act[data.confdata.equip_type];
            if (!Equips.ContainsKey(needTpye_act))
                return false;
            int needatt = a3_EquipModel.getInstance().eqp_att_act[data.equipdata.attribute];
            if (Equips[needTpye_act].equipdata.attribute == needatt)
                return true;
            else
                return false;
        }

        public void createAvatar()
        {
            GameObject obj_prefab_cam;
            if (scene_Camera == null) {
                obj_prefab_cam = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab_cam) as GameObject;
            }

            if (m_SelfObj==null )
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                string m_strAvatarPath = "";
                string equipEff_path = "";
                if (carr == 2)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-150.01f, -0.45f, 2.82f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_warrior_";
                    equipEff_path = "Fx_armourFX_warrior_";
                    m_SelfObj.transform.eulerAngles = new Vector3(3.13f, 34.793f, 5.908f);

                }
                else if (carr == 3)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-150.27f, -0.33f, 2.44f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_mage_";
                    equipEff_path = "Fx_armourFX_mage_";
                    m_SelfObj.transform.eulerAngles = new Vector3(3.197f,29.584f,6.8f);
                }
                else if (carr == 5)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-150.16f, -0.43f, 2.83f), Quaternion.identity) as GameObject;
                    m_strAvatarPath = "profession_assa_";
                    equipEff_path = "Fx_armourFX_assa_";
                    m_SelfObj.transform.eulerAngles = new Vector3(1.497f, 32.282f, 5.623f);
                }
                else
                {
                    return;
                }


                Transform cur_model = m_SelfObj.transform.FindChild("model");
                cur_model.Rotate(Vector3.up, 200f);
                //cur_model.rotation = new Quaternion(-3.135f,-130f,-8.074f,1);
                //手上的小火球
                if (carr == 3)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                int bodyid = 0;
                int bodyFxid = 0;
                uint colorid = 0;
                if (Equips.ContainsKey(3))
                {
                    bodyid = (int)Equips[3].tpid;
                    bodyFxid = Equips[3].equipdata.stage;
                    colorid = Equips[3].equipdata.color;
                }
                int m_Weapon_LID = 0;
                int m_Weapon_LFXID = 0;
                int m_Weapon_RID = 0;
                int m_Weapon_RFXID = 0;

                if (Equips.ContainsKey(6))
                {
                    switch (carr)
                    {
                        case 2:
                            m_Weapon_RID = (int)Equips[6].tpid;
                            m_Weapon_RFXID = Equips[6].equipdata.stage;
                            break;
                        case 3:
                            m_Weapon_LID = (int)Equips[6].tpid;
                            m_Weapon_LFXID = Equips[6].equipdata.stage;
                            break;
                        case 5:
                            m_Weapon_LID = (int)Equips[6].tpid;
                            m_Weapon_LFXID = Equips[6].equipdata.stage;
                            m_Weapon_RID = (int)Equips[6].tpid;
                            m_Weapon_RFXID = Equips[6].equipdata.stage;
                            break;
                    }
                }
                m_proAvatar = new ProfessionAvatar();
                m_proAvatar.Init_PA(eprofession, m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, equipEff_path);
                if (active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(bodyid, true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(active_eqp.Count));

                if(fashionsshows[1]!=0)
                    m_proAvatar.set_body(fashionsshows[1], 0);
                else
                   m_proAvatar.set_body(bodyid, bodyFxid);
                if(fashionsshows[0]!=0)
                {
                    m_proAvatar.set_weaponl(fashionsshows[0], 0);
                    m_proAvatar.set_weaponr(fashionsshows[0], 0);
                }
                else
                {
                    m_proAvatar.set_weaponl(m_Weapon_LID, m_Weapon_LFXID);
                    m_proAvatar.set_weaponr(m_Weapon_RID, m_Weapon_RFXID);
                }

                m_proAvatar.set_wing(show_wing, show_wing);
                m_proAvatar.set_equip_color(colorid);
                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                if (m_proAvatar != null) m_proAvatar.FrameMove();
                wait.SetActive(false);
            }
        }

        #endregion

        #region 召唤兽

        public void createAvatar_sum( int Avator_Sum_id)
        {
            GameObject obj_prefab;
            if (scene_Camera == null)
            {
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            }
            int objid = 0;
            SXML itemsXMl = XMLMgr.instance.GetSXML("callbeast");
            var xml = itemsXMl.GetNode("callbeast", "id==" + Avator_Sum_id);
            var mid = xml.getInt("mid");
            var mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            objid = mxml.getInt("obj");

            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-151.43f, 0.56f, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            m_SelfObj.transform.eulerAngles = new Vector3(0f, 230f, 0f);
            Transform cur_model = m_SelfObj.transform.FindChild("model");
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 90 - mxml.getInt("smshow_face"));
            float scale = mxml.getFloat("smshow_scale");
            if (scale < 0) { scale = 0.7f; }
            cur_model.transform.localScale = new Vector3(scale, scale, scale);
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            wait.SetActive(false);

        }


        #endregion

        #region 飞翼
        public void createAvatar_wing(int carr, int stage)
        {
            //if (m_swobj != null) Destroy(m_swobj);
            GameObject obj_prefab;
            if (scene_Camera == null)
            {
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            }
            string path = "";
            switch (carr)
            {
                case 2:
                    path = "profession_warrior_wing_l_" + stage;
                    break;
                case 3:
                    path = "profession_mage_wing_l_" + stage;
                    break;
                case 5:
                    path = "profession_assa_wing_l_" + stage;
                    break;
            }
            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject(path);
            if (obj_prefab != null)
            {
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-151.74f, 1.17f, -0.03f), Quaternion.identity) as GameObject;
                m_SelfObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                m_SelfObj.transform.eulerAngles = new Vector3(0f,-95f, 0f);
                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
            }
            wait.SetActive(false);

        }
        #endregion
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
            scene_Camera = null;
            m_SelfObj = null;
        }
    }
}
