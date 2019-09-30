
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace MuGame
{
    class a3_rank : Window 
    {
        Image nowtitle;
        Slider expslider;
        Text exptext;
        Toggle showontoggle;
        Text showontext;
        int nowshow;
        BaseButton addlv;
        BaseButton addlv1;
        Transform content;
        //public a3_rank(Window win, Transform tran)
        //    : base(win, tran)
        //{
        //}

        public override void init()
        {

            #region 初始化汉字
            getComponentByPath<Text>("background/att_bg_right/now_rankbg/Text").text = ContMgr.getCont("a3_rank_0");
            getComponentByPath<Text>("panel_centre/now_rankbg/Text").text = ContMgr.getCont("a3_rank_0");
            getComponentByPath<Text>("panel_centre/now_attribute /Text").text = ContMgr.getCont("a3_rank_1");
            getComponentByPath<Text>("panel_centre/next_attribute /Text").text = ContMgr.getCont("a3_rank_2");
           // getComponentByPath<Text>("exp/exp_point/exp").text = ContMgr.getCont("a3_rank_3");
            getComponentByPath<Text>("panel_down/rule_btn/Text").text = ContMgr.getCont("a3_rank_4");
            getComponentByPath<Text>("panel_down/showorhide_btn/Text").text = ContMgr.getCont("a3_rank_5");
            getComponentByPath<Text>("panel_down/upgrade_btn1/Text").text = ContMgr.getCont("a3_rank_6");
            getComponentByPath<Text>("panel_down/upgrade_btn2/Text").text = ContMgr.getCont("a3_rank_7");
            getComponentByPath<Text>("panel_down/showorhide_btn/Text").text = ContMgr.getCont("a3_rank_5");
            getComponentByPath<Text>("bg/title_img/title").text = ContMgr.getCont("a3_rank_8");
            getComponentByPath<Text>("ruledes_bg/ruledes/title").text = ContMgr.getCont("a3_rank_9");
            getComponentByPath<Text>("ruledes_bg/ruledes/des").text = ContMgr.getCont("a3_rank_10");
            getComponentByPath<Text>("con/upgrade_btn1/Text").text = ContMgr.getCont("a3_rank_11");
            getComponentByPath<Text>("con/upgrade_btn2/Text").text = ContMgr.getCont("a3_rank_12");
            getComponentByPath<Text>("con/ranklist/0/0/1").text = ContMgr.getCont("a3_rank_13");
            getComponentByPath<Text>("con/exp/exp_point/hint").text = ContMgr.getCont("a3_rank_14");
            getComponentByPath<Text>("con/exp/exp_point/exp").text = ContMgr.getCont("a3_rank_15");
            getComponentByPath<Text>("con/showon/Label").text = ContMgr.getCont("a3_rank_16");
            #endregion
            Debug.LogError("a3_rank");
            new BaseButton(transform.FindChild("close")).onClick = (GameObject g) => {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RANK );
                if (a3_sports_jdzc._instan != null && a3_sports_jdzc._instan.goBack)
                {
                    ArrayList arrs = new ArrayList();
                    arrs.Add("sports_jdzc");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS, arrs);
                }
            };
            addlv = new BaseButton(transform.FindChild("con/upgrade_btn1"));
            addlv.onClick = onAddLv;
            addlv1 = new BaseButton(transform.FindChild("con/upgrade_btn2"));
            addlv1.onClick = onAddLv;
            BaseButton showon = new BaseButton(transform.FindChild("con/showon"));
            showon.onClick = onShoeorHideTitile;
            BaseButton left = new BaseButton(transform.FindChild("con/left"));
            left.onClick = GoLeft;
            BaseButton right = new BaseButton(transform.FindChild("con/right"));
            right.onClick = GoRight;

            nowtitle = getComponentByPath<Image>("con/header/Image");
            expslider = getComponentByPath<Slider>("con/exp");
            exptext = getComponentByPath<Text>("con/exp/Text");
            showontoggle = getComponentByPath<Toggle>("con/showon");
            showontext = getComponentByPath<Text>("con/showon/Label");
            CreateList();
            nowshow = a3_RankModel.now_id;
        }

        public override void onShowed()
        {
            Refresh();
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.RANKADDLV, Refresh);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.RANKREFRESH, Refresh);
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        public override void onClosed()
        {
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.RANKADDLV, Refresh);
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.RANKREFRESH, Refresh);

 
        }

        void CreateList()
        {
            content = transform.FindChild("con/ranklist/content");
            GameObject temp = transform.FindChild("con/ranklist/0").gameObject;
            GameObject att = transform.FindChild("con/ranklist/0/0").gameObject;

            foreach (var v in a3_RankModel.getInstance().dicrankinfo.Values)
            {
                GameObject go = GameObject.Instantiate(temp) as GameObject;
                go.transform.SetParent(content.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                go.name = v.title_id.ToString();
                string file = "icon_achievement_title_ui_" + v.title_id;
                go.transform.FindChild("title/Image").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                go.transform.FindChild("title/Image").GetComponent<Image>().SetNativeSize();
                Transform iconRoot = go.transform.FindChild("title/icon");                
                go.transform.FindChild("title/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_achievement_title_ui_t" + v.title_id);
                go.transform.FindChild("title/icon").GetComponent<Image>().SetNativeSize();
                var ud = go.AddComponent<UIDark>();
                if (v.title_id > a3_RankModel.now_id) ud.ADDO();
                Transform attcontent = go.transform.FindChild("att");
                foreach (var a in v.nature.Keys)
                {
                    GameObject nk = GameObject.Instantiate(att) as GameObject;
                    nk.transform.SetParent(attcontent.transform);
                    nk.transform.localScale = Vector3.one;
                    nk.transform.localPosition = Vector3.zero;
                    nk.SetActive(true);
                    nk.transform.FindChild("1").GetComponent<Text>().text = /*LRAlign((*/Globle.getRankAttrNameById((int)a)/*))*/ + ":";
                    if (a == 33)
                        nk.transform.FindChild("1/2").GetComponent<Text>().text = (((float)v.nature[a])/10).ToString()+@"%";
                    else
                        nk.transform.FindChild("1/2").GetComponent<Text>().text = v.nature[a].ToString();
                }
            }

            var glg = content.GetComponent<GridLayoutGroup>();
            content.GetComponent<RectTransform>().sizeDelta = new Vector2((glg.cellSize.x + glg.spacing.x + 0.1f) * a3_RankModel.getInstance().dicrankinfo.Count, 0);
        }

        void Refresh(GameEvent gg = null)
        {
            //==1
            string file = "icon_achievement_title_ui_" + a3_RankModel.now_id;
            nowtitle.sprite = GAMEAPI.ABUI_LoadSprite(file);
            nowtitle.SetNativeSize();

            //==2
            SetExpValue();

            //==3
            SetShowonText();

            //==4
            nowshow = Mathf.Max(1, a3_RankModel.now_id);
            if (a3_RankModel.now_id == 0) SetFocus(1);
            else SetFocus(a3_RankModel.now_id);

            //==5
            SetDark();

        }


        string LRAlign(string str)
        {
            string lra = default(string);
            if (str.Length == 4)
            {
                lra = str;
            }
            else if (str.Length == 3)
            {
                lra = str[0] + "   " + str[1] + "   " + str[2];
            }
            else if (str.Length == 2)
            {
                lra = str[0] + "    " + "    " + str[1];
            }
            else if (str.Length == 1)
            {
                lra = "   " + "   " + str[0] + "   " + "   ";
            }
            return lra;
        }

        void SetExpValue()
        {
            if (!a3_RankModel.getInstance().dicrankinfo.ContainsKey(a3_RankModel.now_id + 1))
            {
                expslider.value = 0;
                exptext.text = ContMgr.getCont("a3_rank_maxlv");
                addlv.interactable = false;
                addlv1.interactable = false;
                return;
            }
            float a = a3_RankModel.nowexp > 0 ? (a3_RankModel.nowexp / (float)a3_RankModel.getInstance().dicrankinfo[a3_RankModel.now_id + 1].rankexp) : 0;
            DOTween.To(() => expslider.value, (float s) =>
            {
                expslider.value = s;
            }, a, 0.2f).SetEase(Ease.InExpo);
            DOTween.To(() => (int)float.Parse(exptext.text.Split('/')[0]), (int s) =>
            {
                exptext.text = s.ToString() + "/" + a3_RankModel.getInstance().dicrankinfo[a3_RankModel.now_id + 1].rankexp;
            }, a3_RankModel.nowexp, 0.2f).SetEase(Ease.InExpo);
        }

        void onAddLv(GameObject go)
        {
            if (PlayerModel.getInstance().ach_point >= 100)
            {
                if (go.name == "upgrade_btn1")
                    A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKADDLV, 1);
                else if (go.name == "upgrade_btn2")
                    A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKADDLV, 2);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_rank_nomw"));
            }
        }

        void SetShowonText()
        {
            if (a3_RankModel.now_id > 0)
            {
                if (!a3_RankModel.nowisactive)
                {
                    showontoggle.isOn = false;
                    showontext.text = ContMgr.getCont("a3_rank_showch");
                }
                else
                {
                    showontoggle.isOn = true;
                    showontext.text = ContMgr.getCont("a3_rank_showch");
                }
            }
        }

        void onShoeorHideTitile(GameObject go)
        {
            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKREFRESH, -1, showontoggle.isOn);
            a3_RankModel.nowisactive = showontoggle.isOn;
            Refresh();
        }

        void SetFocus(int i)
        {
            RectTransform rectent = transform.FindChild("con/ranklist/content").GetComponent<RectTransform>();
            var glg = transform.FindChild("con/ranklist/content").GetComponent<GridLayoutGroup>();
            if (a3_RankModel.getInstance().dicrankinfo.ContainsKey(i + 3))
                rectent.DOLocalMoveX((-i - 1) * (glg.cellSize.x + glg.spacing.x), 0.2f);
            else rectent.DOLocalMoveX((-a3_RankModel.getInstance().dicrankinfo.Count + 2) * (glg.cellSize.x + glg.spacing.x), 0.2f);
        }

        void GoLeft(GameObject go = null)
        {
            nowshow--;
            if (a3_RankModel.getInstance().dicrankinfo.ContainsKey(nowshow - 1))
                SetFocus(Mathf.Max(nowshow, 0));
            if (nowshow <= 1)
            {
                nowshow++;
            }
        }

        void GoRight(GameObject go = null)
        {
            nowshow++;
            if (a3_RankModel.getInstance().dicrankinfo.ContainsKey(nowshow))
                SetFocus(Mathf.Max(nowshow, 0));
            else nowshow--;
        }

        void SetDark()
        {
            var t = content.FindChild(a3_RankModel.now_id.ToString());
            if (t != null)
            {
                var ud = t.GetComponent<UIDark>();
                if (ud != null && ud.IsDark)
                {
                    ud.REMO();
                }
            }
            foreach (var v in content.GetComponentsInChildren<Transform>())
            {
                if (v.parent == content.transform)
                {
                    if (int.Parse(v.name) <= a3_RankModel.now_id)
                    {
                        var ud = v.GetComponent<UIDark>();
                        if (ud != null && ud.IsDark)
                        {
                            ud.REMO();
                        }
                    }
                }
            }
        }
    }

    class a3_rank2 : AchiveSkin
    {
        Dictionary<int, rankinfos> dic = a3_RankModel.getInstance().dicrankinfo;
        Dictionary<int, GameObject> dicGo = new Dictionary<int, GameObject>();
        GameObject nowgo = null;
        GameObject conatin;
        GameObject image;
        GameObject contain_nowinfo;
        GameObject image_nowinfo;
        GameObject contain_nextinfo;
        GameObject image_nextinfo;

        Image now_image;
        Image next_image;
        GameObject panel;
        Image last_Image;

        Text onShoeorHideTitileTxt;

        Text ach_num;
        Text exp;
        Slider expimg;
        GameObject ruledes;
        int runeid;               //升级之前的id
        int runeexp;              //升级之前的经验值
        public static a3_rank2 _instance;
        ScrollControler scrollControer0;
        ScrollControler scrollControer1;
        ScrollControler scrollControer2;

        public a3_rank2(Window win, Transform tran)
            : base(win, tran)
        {
        }
        public override void init()
        {
            Debug.LogError("a3_rank2");
            _instance = this;
            scrollControer0 = new ScrollControler();
            ScrollRect scroll = getComponentByPath<ScrollRect>("panel_top/ScrollRect");
            scrollControer0.create(scroll);
            scrollControer1 = new ScrollControler();
            ScrollRect scroll1 = getComponentByPath<ScrollRect>("panel_centre/now_attributeinfos/Scroll_rect");
            scrollControer1.create(scroll1);
            scrollControer2 = new ScrollControler();
            ScrollRect scroll2 = getComponentByPath<ScrollRect>("panel_centre/next_attributeinfos/Scroll_rect");
            scrollControer2.create(scroll2);
            ach_num = getComponentByPath<Text>("panel_down/point");
            ruledes = transform.FindChild("ruledes_bg").gameObject;
            now_image = getComponentByPath<Image>("panel_centre/now_rankinfos/panel/now/now");
            next_image = getComponentByPath<Image>("panel_centre/now_rankinfos/panel/next/next");
            panel = transform.FindChild("panel_centre/now_rankinfos/panel/next").gameObject;
            //last_Image = getComponentByPath<Image>("panel_centre/now_rankinfos/Image");
            exp = getComponentByPath<Text>("panel_centre/exp/Text");
            expimg = getComponentByPath<Slider>("panel_centre/exp");
            onShoeorHideTitileTxt = getComponentByPath<Text>("panel_down/showorhide_btn/Text");

            conatin = transform.FindChild("panel_top/ScrollRect/Contain").gameObject;
            image = transform.FindChild("panel_top/ScrollRect/Image").gameObject;
            contain_nowinfo = transform.FindChild("panel_centre/now_attributeinfos/Scroll_rect/contain").gameObject;
            image_nowinfo = transform.FindChild("panel_centre/now_attributeinfos/Scroll_rect/Image").gameObject;
            contain_nextinfo = transform.FindChild("panel_centre/next_attributeinfos/Scroll_rect/contain").gameObject;
            image_nextinfo = transform.FindChild("panel_centre/next_attributeinfos/Scroll_rect/Image").gameObject;
            BaseButton btn_des = new BaseButton(transform.FindChild("panel_down/rule_btn"));
            btn_des.onClick = onShowdes;
            BaseButton btn_desclose = new BaseButton(transform.FindChild("ruledes_bg/ruledes/close"));
            btn_desclose.onClick = onClosedes;
            BaseButton btn_close = new BaseButton(transform.FindChild("close"));
            btn_close.onClick = onClose;
            BaseButton addlv = new BaseButton(transform.FindChild("panel_down/upgrade_btn1"));
            addlv.onClick = onAddLv;
            BaseButton addlv1 = new BaseButton(transform.FindChild("panel_down/upgrade_btn2"));
            addlv1.onClick = onAddLv;
            BaseButton showorhide = new BaseButton(transform.FindChild("panel_down/showorhide_btn"));
            showorhide.onClick = onShoeorHideTitile;
            cteatrve();
            SetSliderValue(0);

        }
        public override void onShowed()
        {
            RefreshInfos(a3_RankModel.now_id);
            refreshAch_point();
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.RANKADDLV, onAddLvOver);
            A3_RankProxy.getInstance().addEventListener(A3_RankProxy.RANKREFRESH, OnRefresh);
        }
        public override void onClosed()
        {
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.RANKADDLV, onAddLvOver);
            A3_RankProxy.getInstance().removeEventListener(A3_RankProxy.RANKREFRESH, OnRefresh);

        }
        bool isaddlv = false;//是不是要加动画
        void onAddLvOver(GameEvent e)
        {
            isaddlv = true;
            RefreshInfos(e.data["title"]);




        }

        void OnRefresh(GameEvent e)
        {
            refreshAch_point();
        }

        void Update()
        {

        }

        public void RefreshInfos(int now_id)
        {

            onShoeorHideTitileTxt.text = a3_RankModel.nowisactive == true ? ContMgr.getCont("a3_rank_noshowch") : ContMgr.getCont("a3_rank_showch");

            int count = dic.Keys.Count;

            if (now_id == 0)
            {
                GetImage(now_id, now_image);
                GetImage(now_id + 1, next_image);
                exp.text = a3_RankModel.nowexp + "/" + a3_RankModel.getInstance().dicrankinfo[now_id + 1].rankexp;
                SetSliderValue(a3_RankModel.nowexp > 0 ? (a3_RankModel.nowexp / (float)a3_RankModel.getInstance().dicrankinfo[now_id + 1].rankexp) : 0);
                GetAttr(now_id + 1, contain_nextinfo, image_nextinfo);
            }
            else if (now_id == count)
            {
                //最大
                panel.SetActive(false);
                transform.FindChild("background/att_bg_right").gameObject.SetActive(false);
                transform.FindChild("panel_centre/exp").gameObject.SetActive(false);
                transform.FindChild("background/exp_point").gameObject.SetActive(false);
                //last_Image.gameObject.SetActive(true);
                GetImage(now_id, now_image);
                GetAttr(now_id, contain_nowinfo, image_nowinfo);
                deleteAttr(contain_nextinfo, image_nextinfo);
                SetSliderValue(1);
                exp.text = "max";/*a3_RankModel.nowexp + "/" + a3_RankModel.getInstance().dicrankinfo[now_id].rankexp*/;
            }
            else
            {
                GetImage(now_id, now_image);
                GetAttr(now_id, contain_nowinfo, image_nowinfo);
                GetImage(now_id + 1, next_image);
                GetAttr(now_id + 1, contain_nextinfo, image_nextinfo);
                exp.text = a3_RankModel.nowexp + "/" + a3_RankModel.getInstance().dicrankinfo[now_id + 1].rankexp;
                SetSliderValue(a3_RankModel.nowexp > 0 ? a3_RankModel.nowexp / (float)a3_RankModel.getInstance().dicrankinfo[now_id + 1].rankexp : 0);
            }
            if (nowgo != null) nowgo.transform.FindChild("bg").GetComponent<Image>().enabled = false;
            if (dicGo.ContainsKey(now_id))
            {
                dicGo[now_id].transform.FindChild("bg").GetComponent<Image>().enabled = true;
                nowgo = dicGo[now_id];
            }

        }

        void SetSliderValue(float a)
        {

            DOTween.To(() => expimg.value, (float s) =>
            {
                expimg.value = s;
            }, a, 0.5f);
        }
        //图片
        void GetImage(int id, Image image)
        {
            string file = "icon_achievement_title_ui_" + id;
            image.sprite = GAMEAPI.ABUI_LoadSprite(file);
            image.SetNativeSize();
        }
        //属性
        void GetAttr(int id, GameObject conatin, GameObject image)
        {
            deleteAttr(conatin, image);
            //print("收到的id是：" + id);
            SXML xml = XMLMgr.instance.GetSXML("achievement.title", "title_id==" + id);
            List<SXML> xmls = xml.GetNodeList("nature");
            for (int i = 0; i < xmls.Count; i++)
            {
                GameObject objclone = GameObject.Instantiate(image) as GameObject;
                objclone.SetActive(true);
                objclone.transform.SetParent(conatin.transform, false);
                objclone.transform.FindChild("Text").GetComponent<Text>().text = Globle.getRankAttrNameById(xmls[i].getInt("att_type")) + ":" + xmls[i].getInt("att_value");
            }
            refreshs(conatin, image);
        }
        void deleteAttr(GameObject conatin, GameObject image)
        {
            if (conatin.transform.childCount > 0)
            {
                for (int i = 0; i < conatin.transform.childCount; i++)
                {
                    GameObject.Destroy(conatin.transform.GetChild(i).gameObject);
                }
            }
        }
        private void Destory(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        void cteatrve()
        {
            foreach (int i in dic.Keys)
            {
                GameObject objClone = GameObject.Instantiate(image) as GameObject;
                objClone.SetActive(true);
                objClone.transform.SetParent(conatin.transform, false);
                Image picture_nodone = objClone.transform.FindChild("Image").GetComponent<Image>();
                string file = "icon_achievement_title_ui_" + i;
                picture_nodone.sprite = GAMEAPI.ABUI_LoadSprite(file);
                picture_nodone.SetNativeSize();
                dicGo[i] = objClone;
            }
            refresh();
        }
        void refresh()
        {
            int num = conatin.transform.childCount;
            RectTransform rts = conatin.GetComponent<RectTransform>();
            RectTransform rts_image = image.GetComponent<RectTransform>();
            rts.sizeDelta = new Vector2(rts_image.sizeDelta.x * num, rts_image.sizeDelta.y); ;
        }
        void refreshs(GameObject contains, GameObject images)
        {
            int num = contains.transform.childCount;
            RectTransform rts = contains.GetComponent<RectTransform>();
            RectTransform rts_image = images.GetComponent<RectTransform>();
            rts.sizeDelta = new Vector2(rts_image.sizeDelta.x, rts_image.sizeDelta.y * num); ;
        }
        void onShowdes(GameObject go)
        {
            ruledes.SetActive(true);
        }
        void onClosedes(GameObject go)
        {
            ruledes.SetActive(false);
        }
        //升级
        void onAddLv(GameObject go)
        {

            runeid = a3_RankModel.now_id;
            runeexp = a3_RankModel.nowexp;
            if (PlayerModel.getInstance().ach_point >= 500)
            {
                if (go.name == "upgrade_btn1")
                    A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKADDLV, 1);
                else if (go.name == "upgrade_btn2")
                    A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKADDLV, 2);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_rank_nomw"));
            }
        }

        void onShoeorHideTitile(GameObject go)
        {
            if (a3_RankModel.now_id > 0)
            {
                if (a3_RankModel.nowisactive)
                {
                    a3_RankModel.nowisactive = false;
                    onShoeorHideTitileTxt.text = ContMgr.getCont("a3_rank_showch");
                }
                else
                {
                    a3_RankModel.nowisactive = true;
                    onShoeorHideTitileTxt.text = ContMgr.getCont("a3_rank_noshowch");
                }
                A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKREFRESH, -1, a3_RankModel.nowisactive);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_rank_nohave"), 1);
            }

        }
        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACHIEVEMENT);

        }
        public void refreshAch_point()
        {
            A3_RankProxy.getInstance().sendProxy(A3_RankProxy.RANKREFRESH);
            ach_num.text = "" + Globle.getBigText((uint)PlayerModel.getInstance().ach_point);
        }
    }
}

