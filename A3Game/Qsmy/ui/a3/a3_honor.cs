using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MuGame
{
    class a3_honor : Window 
    {
        private Transform conTypeTab;

        private Transform conPage;
        private GameObject tempPage;

        private Text textPoint;
        public static a3_honor instan;

        private BaseButton btnUpgrade;

        private BaseButton btnClose;
        private BaseButton btnLeft;
        private BaseButton btnRight;

        private Text textPageIndex;

        private ScrollRect scrollPage;

        private A3_AchievementModel achModel;

        //存放page的字典
        private Dictionary<uint, GameObject> dicPage = new Dictionary<uint, GameObject>();

        //public a3_honor(Window win, Transform tran)
        //    : base(win, tran)
        //{
        //}
        ScrollControler scrollControer0;


        public override void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("con_page/view"));


            instan = this;
            inText();
            achModel = A3_AchievementModel.getInstance();

            conTypeTab = this.getTransformByPath("con_tab");

            conPage = this.getTransformByPath("con_page/view/con");
            tempPage = this.getGameObjectByPath("con_page/tempPage");

            textPoint = this.getComponentByPath<Text>("Text_point/Text_point");

            // btnUpgrade = new BaseButton(this.getTransformByPath("btn_upgrade"));
            //btnUpgrade.onClick = OnUpgradeClick;

            btnLeft = new BaseButton(this.getTransformByPath("btn_select/btn_left"));
            btnLeft.onClick = OnLeftClick;
            btnRight = new BaseButton(this.getTransformByPath("btn_select/btn_right"));
            btnRight.onClick = OnRightClick;
            textPageIndex = this.getComponentByPath<Text>("btn_select/bg/Text");

            btnClose = new BaseButton(this.getTransformByPath("btn_close"));
            btnClose.onClick = OnCloseClick;

            scrollPage = this.getComponentByPath<ScrollRect>("con_page/view");
            //TODO 初始化页面内容
            InitBtnTab();
            base.init();
        }

        void inText()
        {
            this.transform.FindChild("achi_reward/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_1");//成就
            this.transform.FindChild("con_page/tempPage/con_prize/panel_0/inage/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_2");//未达成
            this.transform.FindChild("con_page/tempPage/btn_get/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_honor_3");//领取
        }

        public void RefreshPage(GameEvent e)
        {
            OnShowAchievementPage(this.selectType, this.pageIndex);
        }

        //打开                
        public override void onShowed()
        {

            UIClient.instance.addEventListener( UI_EVENT.ON_MONEY_CHANGE , RefreshBdyb );

            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE);//获取玩家成就信息

            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.RANKREFRESH, RefreshPage);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.ON_ACHIEVEMENT_CHANGE, OnRefreshPageEvent);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE, OnShowGetPrize);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.ON_REACH_ACHIEVEMENT, OnRefreshPageEvent);
            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE);
            achModel.OnAchievementChange += OnPointChange;
            OnPointChange();                   
            base.onShowed();
        }

        //关闭
        public override void onClosed()
        {
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.RANKREFRESH, RefreshPage);
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.ON_ACHIEVEMENT_CHANGE, OnRefreshPageEvent);
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.ON_GET_ACHIEVEMENT_PRIZE, OnShowGetPrize);
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.ON_REACH_ACHIEVEMENT, OnRefreshPageEvent);
            UIClient.instance.removeEventListener( UI_EVENT.ON_MONEY_CHANGE , RefreshBdyb );

            achModel.OnAchievementChange -= OnPointChange;

            ClearContainer();
            scrollPage.StopMovement();

            base.onClosed();
        }

        //每页最大显示数量
        private int maxNum = 8;
        //当前显示的页面索引
        private int pageIndex = 0;
        //当前的页面类型
        private uint selectType = 0;        
        //最大页数
        private int maxPageNum = 0;

        //显示成就页面
        private void OnShowAchievementPage(uint showType, int pageIndex)
        {
            List<AchievementData> listData = achModel.GetAchievenementDataByType(showType);
            int curIndex = maxNum * pageIndex;
            if (curIndex >= listData.Count)
                return;

            OnShowSelect(showType, pageIndex);
            ClearContainer();

            for (int i = 0; i < maxNum; i++)
            {
                int index = curIndex + i;
                if (index >= listData.Count)
                    return;
                GameObject clone = (GameObject)GameObject.Instantiate(tempPage);

                AchievementData data = listData[index];

                clone.name = data.id.ToString();

                Text textPoint = clone.transform.FindChild("Image_point/text_point").GetComponent<Text>();
                Text textTitle = clone.transform.FindChild("text_1").GetComponent<Text>();
                Text textDesc = clone.transform.FindChild("text_state").GetComponent<Text>();   

                textPoint.text = data.point.ToString();
                textTitle.text = data.name;
                textDesc.text = data.desc;

                OnRefreshPageState(clone, data);

                clone.transform.SetParent(conPage, false);
                clone.SetActive(true);

                dicPage[data.id] = clone;
            }

            //初始化容器位置
            RectTransform rect = conPage.GetComponent<RectTransform>();
            Vector2 newPos = new Vector2(rect.anchoredPosition.x, 0);
            rect.anchoredPosition = newPos;
            scrollPage.StopMovement();
        }

        private void ClearContainer()
        {
            foreach (GameObject go in dicPage.Values)
            {
                GameObject.Destroy(go);
            }
            dicPage.Clear();
        }

        private void OnShowSelect(uint showType, int pageIndex)
        {
            int count = achModel.GetAchievenementDataByType(showType).Count;

            if (count % maxNum != 0)
                this.maxPageNum = count / maxNum + 1;
            else
                this.maxPageNum = count / maxNum;

            textPageIndex.text = (pageIndex + 1) + "/" + this.maxPageNum;
        }

        //初始化tab
        private void InitBtnTab()
        {
            GameObject tempBtn = conTypeTab.FindChild("btnTemp").gameObject;
            Transform container = conTypeTab.FindChild("view/con");
            List<uint> listType = achModel.listCategory;
            for (int i = 0; i < listType.Count; i++)
            {
                GameObject clone = (GameObject)GameObject.Instantiate(tempBtn);
                clone.name = listType[i].ToString();

                Text textName = clone.transform.FindChild("Text").GetComponent<Text>();
                textName.text = achModel.GetCategoryName(listType[i]);

                clone.transform.SetParent(container, false);
                clone.SetActive(true);

            }
            TabControl tc = new TabControl();
            tc.create(container.gameObject, this.gameObject);
            tc.onClickHanle = (TabControl t) =>
            {
                int i = t.getSeletedIndex();
                this.selectType = (uint)i;
                OnShowTypeChange(selectType);
            };
        }

        //变更页面类型时变化页面内容
        private void OnShowTypeChange(uint showType)
        {
            pageIndex = 0;
            ClearContainer();

            OnShowAchievementPage(selectType, pageIndex);

            //uint type = achModel.ShowType;
            //Dictionary<uint, AchievementData> dicData = achModel.GetAchievenementDataByType(type);

            ////初始化容器位置
            //RectTransform rect = conPage.GetComponent<RectTransform>();
            //Vector2 newPos = new Vector2(rect.anchoredPosition.x, 0);
            //rect.anchoredPosition = newPos;


        }

        //刷新指定成就的状态
        private void OnRefreshPageState(GameObject page, AchievementData data)
        {
            //TODO 刷新指定的page
            Text textDrgee = page.transform.FindChild("text_state/text_plan").GetComponent<Text>();
            Slider sliderState = page.transform.FindChild("expbar/slider").GetComponent<Slider>();
            BaseButton btnGet = new BaseButton(page.transform.FindChild("btn_get"));
            Transform conPrize = page.transform.FindChild("con_prize");
            Text numtext = null;
            var numtra = page.transform.FindChild("con_prize/panel_0/icon/Text");
            if (numtra != null) numtext = numtra.GetComponent<Text>();

            switch (data.state)
            {
                case AchievementState.UNREACHED:
                    btnGet.gameObject.SetActive(false);
                    conPrize.GetChild(0).gameObject.SetActive(true);
                    conPrize.GetChild(1).gameObject.SetActive(false);
                    if (data.bndyb == 0)
                    {
                        numtext.transform.parent.gameObject.SetActive(false);
                        conPrize.GetComponent<Image>().enabled = false;
                    }
                    numtext.text = data.bndyb.ToString();
                    break;
                case AchievementState.REACHED:
                    btnGet.gameObject.SetActive(true);
                    btnGet.onClick = OnGetPrzieClick;
                    conPrize.GetChild(0).gameObject.SetActive(true);
                    conPrize.GetChild(1).gameObject.SetActive(false);
                    if (data.bndyb == 0)
                    {
                        numtext.transform.parent.gameObject.SetActive(false);
                        conPrize.GetComponent<Image>().enabled = false;
                    }
                    numtext.text = data.bndyb.ToString();
                    break;
                case AchievementState.RECEIVED:
                    btnGet.gameObject.SetActive(false);
                    conPrize.GetChild(0).gameObject.SetActive(false);
                    conPrize.GetChild(1).gameObject.SetActive(true);
                    if (data.bndyb == 0)
                        numtext.transform.parent.gameObject.SetActive(false);
                    numtext.text = "";
                    break;
                default:
                    break;
            }

            textDrgee.text = "(" + (data.degree > data.condition ? data.condition : data.degree) + "/" + data.condition + ")";

            sliderState.maxValue = data.condition;
            sliderState.value = data.degree;
        }

        //成就点数变化
        private void OnPointChange()
        {
            textPoint.text = achModel.AchievementPoint.ToString();
            var zstt = transform.FindChild("zhuanshi/text");
            if (zstt != null)
            {
                Text tt = zstt.GetComponent<Text>();
                if (tt != null) tt.text = PlayerModel.getInstance().gift.ToString();
            }
        }

        //刷新成就面板事件
        public void OnRefreshPageEvent(GameEvent e)
        {
            //成就的变化
            List<uint> list = achModel.listAchievementChange;

            foreach (uint id in list)
            {
                if (dicPage.ContainsKey(id))
                {
                    GameObject tempPage = dicPage[id];
                    AchievementData data = achModel.GetAchievementDataByID(id);

                    OnRefreshPageState(tempPage, data);
                }
            }

            achModel.listAchievementChange.Clear();
        }

        //领取奖励
        private void OnShowGetPrize(GameEvent e)
        {
            uint id = achModel.GetAchievementID;
            AchievementData data = achModel.GetAchievementDataByID(id);

            uint bndyb = data.bndyb;
            if (bndyb > 0)      
                flytxt.instance.fly(ContMgr.getCont("a3_honor_getbz") + bndyb);
            if (dicPage.ContainsKey(id))
            {
                GameObject page = dicPage[id];

                OnRefreshPageState(page, data);
                SortPage(page, data);
            }
        }

        //对页面进行排序,已领取放在最后, 未领取最前
        private void SortPage(GameObject page, AchievementData data)
        {
            switch (data.state)
            {
                case AchievementState.UNREACHED:
                    break;
                case AchievementState.REACHED:
                    page.transform.SetAsFirstSibling();
                    break;
                case AchievementState.RECEIVED:
                    page.transform.SetAsLastSibling();
                    ClearContainer();
                    scrollPage.StopMovement();
                    OnPointChange();
                    OnShowAchievementPage(this.selectType, this.pageIndex);
                    break;
                default:
                    break;
            }
        }

        //升级称号
        private void OnUpgradeClick(GameObject go)
        {
            //TODO 升级称号
            //A3_RankProxy.getInstance().sendProxy();
        }

        //关闭
        private void OnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_HONOR );
        }

        private void OnLeftClick(GameObject go)
        {
            if (this.pageIndex <= 0)
                return;
            this.pageIndex--;

            OnShowAchievementPage(this.selectType, this.pageIndex);
        }

        private void OnRightClick(GameObject go)
        {
            if (this.pageIndex + 1 >= this.maxPageNum)
                return;
            this.pageIndex++;

            OnShowAchievementPage(this.selectType, this.pageIndex);
        }

        private void OnGetPrzieClick(GameObject go)
        {
            uint id = uint.Parse(go.transform.parent.name);
            A3_RankProxy.getInstance().GetAchievementPrize(id);
        }

        private void RefreshBdyb( GameEvent e ) {

                Variant info = e.data;

                if ( info.ContainsKey( "bndyb" ) )
                {

                transform.FindChild( "zhuanshi/text" ).GetComponent<Text>().text = PlayerModel.getInstance().gift.ToString();
                }

       }

    }
}
