using Cross;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_activeOnline : Window
    {
        GameObject bigbg;


        GameObject obj,
                   obj1;
        BaseButton brn;

        //活跃+奖励
        GameObject huoyue_image;
        Transform huoyue_contain;
        Text point_txt;

        GameObject reward_panel;
        List<GameObject> rewards = new List<GameObject>();

        GameObject exp_obj;

        //抽奖
        GameObject roll_in,
                   roll_over;
        Text cj_txt;
        BaseButton btns_lottery;
        GameObject item_panel;
        List<GameObject> lst_lottery_obj = new List<GameObject>();
        bool rotationing = false;
        GameObject cricleob;

       //钻石基金,转生基金,战力基金tranjiu mang shihagn dshu mag ndshimsan hiajia nhsima nshui

       GameObject zuanshi_image,
                   zhuansheng_image,
                   zhanli_image;
        Transform zuanshi_contain,
                  zhuansheng_contain,
                  zhanli_contain;
        GameObject zuanshi_btn,
                   zhuansheng_btn,
                   zhanli_btn;


        GameObject contains;
        List<GameObject> lst_contain = new List<GameObject>();
        TabControl tab = new TabControl();
        public static a3_activeOnline _instance;
        public static a3_activeOnline instance;
        ScrollControler scrollControer0;
        public override void init()
        {
            _instance = this;
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("contain_panel/0/down/Scrollview"));
            inText();
            bigbg = getGameObjectByPath("bigbg");
            //活跃
            huoyue_image = getGameObjectByPath("contain_panel/0/down/Scrollview/Image");
            huoyue_contain = getTransformByPath("contain_panel/0/down/Scrollview/contain");
            point_txt = getComponentByPath<Text>("contain_panel/0/top/point/Text");
            reward_panel = getGameObjectByPath("contain_panel/0/top/items");
            exp_obj = getGameObjectByPath("contain_panel/0/top/image/exp");
            //抽奖
            roll_in = getGameObjectByPath("contain_panel/1/roll_in");
            roll_over = getGameObjectByPath("contain_panel/1/roll_over");
            cj_txt = getComponentByPath<Text>("contain_panel/1/btn/Text");
            btns_lottery = new BaseButton(getTransformByPath("contain_panel/1/btn"));
            btns_lottery.onClick = (GameObject go) => { btnOnClick(); };
            item_panel = getGameObjectByPath("contain_panel/1/cricle/image");
            for (int i = 0; i < item_panel.transform.childCount; i++)
            {
                lst_lottery_obj.Add(item_panel.transform.GetChild(i).gameObject);
            }
            cricleob = getGameObjectByPath("contain_panel/1/cricle");
            //基金
            zuanshi_image = getGameObjectByPath("contain_panel/2/down/Scrollview/Image");
            zhuansheng_image = getGameObjectByPath("contain_panel/3/down/Scrollview/Image");
            zhanli_image = getGameObjectByPath("contain_panel/4/down/Scrollview/Image");
            zuanshi_contain = getTransformByPath("contain_panel/2/down/Scrollview/contain");
            zhuansheng_contain = getTransformByPath("contain_panel/3/down/Scrollview/contain");
            zhanli_contain = getTransformByPath("contain_panel/4/down/Scrollview/contain");
            zuanshi_btn = getGameObjectByPath("contain_panel/2/top/Button");
            zhuansheng_btn = getGameObjectByPath("contain_panel/3/top/Button");
            zhanli_btn = getGameObjectByPath("contain_panel/4/top/Button");
            new BaseButton(zuanshi_btn.transform).onClick = (GameObject go) => { btn_buyfund(1); };
            new BaseButton(zhuansheng_btn.transform).onClick = (GameObject go) => { btn_buyfund(2); };
            new BaseButton(zhanli_btn.transform).onClick = (GameObject go) => { btn_buyfund(3); };


            contains = getGameObjectByPath("contain_panel");
            initsomething();
            tab.onClickHanle = tabhandel;
            tab.create(getGameObjectByPath("btn_panel"), this.gameObject);
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) => { closBtnOnclick(); };
            getGameObjectByPath("btn_panel/0/Image").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hint_tips");
            getGameObjectByPath("btn_panel/1/Image").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hint_tips");

        }


        void inText()
        {
            this.transform.FindChild("btn_panel/0/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeOnline_1");
            this.transform.FindChild("btn_panel/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeOnline_2");
            this.transform.FindChild("contain_panel/0/down/Scrollview/Image/txt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeOnline_3");
            this.transform.FindChild("contain_panel/0/down/Scrollview/Image/Button/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeOnline_4");
        }


        void initsomething()
        {
            for (int i = 0; i < contains.transform.childCount; i++)
            {
                lst_contain.Add(contains.transform.GetChild(i).gameObject);
            }
            //
            for (int j = 0; j < reward_panel.transform.childCount; j++)
            {
                rewards.Add(reward_panel.transform.GetChild(j).gameObject);
            }
            creatrve(huoyue_image, huoyue_contain);
            creatrve();
            //
            //
            creatrve_fund(zuanshi_image, zuanshi_contain, 1);
            creatrve_fund(zhuansheng_image, zhuansheng_contain, 2);
            creatrve_fund(zhanli_image, zhanli_contain, 3);
        }




        public override void onShowed()
        {

           

            instance = this;
            RefreshHint();
            tab.setSelectedIndex(0);
            Refresh_huoyue_infos();
            Resresh_reward();
            a3_activeOnlineProxy.getInstance().addEventListener(a3_activeOnlineProxy.ACTIVELOTTERY, RefreshLottery);
            a3_activeOnlineProxy.getInstance().addEventListener(a3_activeOnlineProxy.ACTIVELOTTERYOVER, lotteryoner);
            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public override void onClosed()
        {


            a3_activeOnlineProxy.getInstance().removeEventListener(a3_activeOnlineProxy.ACTIVELOTTERY, RefreshLottery);
            a3_activeOnlineProxy.getInstance().removeEventListener(a3_activeOnlineProxy.ACTIVELOTTERYOVER, lotteryoner);


        }
        public void RefreshHint()
        {
            getGameObjectByPath("btn_panel/0/Image").SetActive(a3_ActiveOnlineModel.getInstance().hintreward ? true : false);
            getGameObjectByPath("btn_panel/1/Image").SetActive(a3_ActiveOnlineModel.getInstance().hintlottery ? true : false);
        }

        public bool ischoujiang = false;
        void tabhandel(TabControl t)
        {
            for (int i = 0; i < lst_contain.Count; i++)
            {
                lst_contain[i].SetActive(i == t.getSeletedIndex() ? true : false);
            }
            if (t.getSeletedIndex() != 1)
                getComponentByPath<ScrollRect>("contain_panel/" + t.getSeletedIndex() + "/down/Scrollview").verticalNormalizedPosition = 1;
            switch (t.getSeletedIndex())
            {
                case 0:
                    Refresh_huoyue_infos();
                    Resresh_reward();
                    break;
                case 1:
                    ischoujiang = true;
                    a3_activeOnlineProxy.getInstance().SendProxy(1);
                    break;
                case 2:
                case 3:
                case 4:
                    RefreshFund_all();
                    break;
                default:
                    return;
            }

        }


        #region //活跃
        #region/*活跃任务*/
        Dictionary<int, GameObject> huoyue_obj = new Dictionary<int, GameObject>();
        private void creatrve(GameObject image, Transform contain)
        {
            Dictionary<int, huoyue_infos> dic = a3_ActiveOnlineModel.getInstance().dic_huoyue;
            foreach (int i in dic.Keys)
            {
                GameObject objclone = GameObject.Instantiate(image) as GameObject;
                objclone.SetActive(true);
                objclone.transform.SetParent(contain.transform, false);
                objclone.name = dic[i].id.ToString();
                huoyue_obj[i] = objclone;
                GameObject icon = objclone.transform.FindChild("icon").gameObject;
                icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(dic[i].icon_frie);

                Text des = objclone.transform.FindChild("des").GetComponent<Text>();
                des.text = dic[i].des + "(0/" + dic[i].need_num + ")";

                Text des1 = objclone.transform.FindChild("des1").GetComponent<Text>();
                des1.text = "";

                Text point = objclone.transform.FindChild("point").GetComponent<Text>();
                point.text = "+" + dic[i].point;

                objclone.transform.FindChild("Button").gameObject.SetActive(true);
                int j = i;
                new BaseButton(objclone.transform.FindChild("Button").transform).onClick = (GameObject go) => { btn_go(j); };
                objclone.transform.FindChild("over").gameObject.SetActive(false);

            }
            a3_runestone.commonScroview(contain.gameObject, huoyue_obj.Keys.Count);
        }

        public void btn_go(int i)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVEONLINE);
            switch (i)
            {
                //拍卖上架商品
                case 1:
                    // Variant conf = SvrMapConfig.instance.getSingleMapConf((uint)GRMap.instance.m_nCurMapID);

                    //原来只在主城拍卖，现在改掉了
                    //if (GRMap.instance.m_nCurMapID != 10)
                    //{
                    //    flytxt.instance.fly(ContMgr.getCont("a3_activeDegree_please"));
                    //}
                    //else
                    //{
                    SelfRole.fsm.ChangeState(StateIdle.Instance);


                    ArrayList dlt = new ArrayList();
                    dlt.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION, dlt);
                    // }                                 
                    break;
                //材料副本
                case 2:
                    ArrayList dat1 = new ArrayList();
                    dat1.Add(0);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat1);
                    break;
                //魔炼之地
                case 3:
                    ArrayList dl = new ArrayList();
                    dl.Add("mlzd");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                    break;
                //金币副本
                case 4:
                    ArrayList dat2 = new ArrayList();
                    dat2.Add(0);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat2);
                    break;
                //经验副本     
                case 5:
                    ArrayList dat3 = new ArrayList();
                    dat3.Add(0);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat3);
                    break;
                //击杀野外boss
                case 6:
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON);
                    break;
                //强化装备
                case 7:
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
                    break;
                //托维尔墓穴
                case 8:
                    ArrayList dat = new ArrayList();
                    dat.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat);
                    break;
                //占卜
                case 9:
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY);
                    break;
                //驯龙者的末日
                case 10:
                    ArrayList dat10 = new ArrayList();
                    dat10.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat10);
                    break;
                //血色丛林
                case 11:
                    ArrayList dat11 = new ArrayList();
                    dat11.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART, dat11);
                    break;
                //兽灵秘境
                case 12:
                    ArrayList dl12 = new ArrayList();
                    dl12.Add("summonpark");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl12);
                    break;
                //完成魔物猎人
                case 13:
                    ArrayList dl13 = new ArrayList();
                    dl13.Add("mwlr");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl13);
                    break;
                 //参与竞技场
                case 14:
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS);
                    break;
                //参与战场
                case 15:
                    ArrayList dl14 = new ArrayList();
                    dl14.Add("sports_jdzc");
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS, dl14);
                    break;

            }
        }
        public void Refresh_huoyue_infos()
        {
            Dictionary<int, huoyue_infos> dic = a3_ActiveOnlineModel.getInstance().dic_huoyue;

            foreach (int id in dic.Keys)
            {


                //添加功能限制
                if (dic[id].type == 1)
                {
                    int lv = (int)PlayerModel.getInstance().lvl;
                    int level2 = (int)PlayerModel.getInstance().up_lvl;

                    string lvl = dic[id].pram;
                    string[] dj = lvl.Split(',');
                    int a = int.Parse(dj[0]);
                    int b = int.Parse(dj[1]);
                    if ((level2 * 100 + lv) >= (a * 100 + b))
                    {
                        huoyue_obj[id].transform.FindChild("Button").gameObject.SetActive(dic[id].receive_type == 2 ? false : true);

                    }
                    else
                    {
                        huoyue_obj[id].transform.FindChild("Button").gameObject.SetActive(false);
                    }
                }
                else if (dic[id].type == 2)
                {
                    int maintaskid = int.Parse(dic[id].pram);

                    if (maintaskid < A3_TaskModel.getInstance().main_task_id)
                    {
                        huoyue_obj[id].transform.FindChild("Button").gameObject.SetActive(dic[id].receive_type == 2 ? false : true);
                    }
                    else
                    {
                        huoyue_obj[id].transform.FindChild("Button").gameObject.SetActive(false);
                    }
                }
                huoyue_obj[id].transform.FindChild("des").GetComponent<Text>().text = dic[id].des + "(" + dic[id].have_num + "/" + dic[id].need_num + ")";
                huoyue_obj[id].transform.FindChild("over").gameObject.SetActive(dic[id].receive_type == 2 ? true : false);
                //huoyue_obj[id].transform.FindChild("Button").gameObject.SetActive(dic[id].receive_type == 2 ? false : true);
                if (dic[id].receive_type == 2)
                    huoyue_obj[id].transform.SetAsLastSibling();
            }
        }
        #endregion
        #region/*活跃奖励*/
        Dictionary<int, GameObject> reward_obj = new Dictionary<int, GameObject>();
        void creatrve()
        {
            Dictionary<int, reward_info> dic = a3_ActiveOnlineModel.getInstance().dic_reward;
            Dictionary<int, reward_info>.Enumerator d = dic.GetEnumerator();
            for (int i = 0; i < dic.Count; i++)
            {
                if (d.MoveNext())
                {
                    int item_id = d.Current.Value.item_id;
                    GameObject icon = rewards[i].transform.FindChild("item").gameObject;
                    GameObject icons = IconImageMgr.getInstance().createA3ItemIcon((uint)d.Current.Value.item_id, true, d.Current.Value.num, 1, true);
                    int id = d.Current.Key;
                    new BaseButton(icons.transform).onClick = (GameObject go) =>
                    {
                        getBtnOnclick(item_id, id);
                    };
                    icons.transform.SetParent(icon.transform, false);
                    rewards[i].transform.FindChild("over").gameObject.SetActive(false);
                    rewards[i].transform.FindChild("this").gameObject.SetActive(false);
                    reward_obj[d.Current.Key] = rewards[i];
                }
            }
        }

        void getBtnOnclick(int item_id, int id)
        {
            Dictionary<int, reward_info> dic = a3_ActiveOnlineModel.getInstance().dic_reward;
             int needata= dic[id].ac;
            int num = a3_ActiveOnlineModel.getInstance().nowpoint;
            if (num >= needata)
            {
                a3_activeOnlineProxy.getInstance().SendProxy(2, needata);
            }
            else
            {
                ArrayList arr = new ArrayList();
                arr.Add((uint)item_id);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                a3_miniTip.Instance?.transform.SetAsLastSibling();
            }

        }
        public void Resresh_reward()
        {
            point_txt.text = a3_ActiveOnlineModel.getInstance().nowpoint.ToString();
            int num = a3_ActiveOnlineModel.getInstance().nowpoint;
            Dictionary<int, reward_info> dic = a3_ActiveOnlineModel.getInstance().dic_reward;


            foreach (int i in dic.Keys)
            {

                if (dic[i].receive_type == 2)
                {
                    reward_obj[i].transform.FindChild("over").gameObject.SetActive(true);
                    reward_obj[i].transform.FindChild("this").gameObject.SetActive(false);
                }
                else {
                    reward_obj[i].transform.FindChild("over").gameObject.SetActive(false);
                }
                if (num >= dic[i].ac&& dic[i].receive_type != 2)
                {
                    reward_obj[i].transform.FindChild("this").gameObject.SetActive(true);
                    reward_obj[i].transform.FindChild("over").gameObject.SetActive(false);
                }
            }

            
            point_txt.text = num.ToString();

            float x = num / (float)100;
            exp_obj.GetComponent<Transform>().localScale = num >= 100 ? new Vector3(1, 1, 1) : new Vector3(x, 1, 1);
        }
        public void Refresh()
        {
            Refresh_huoyue_infos();
            Resresh_reward();
        }
        #endregion
        #endregion
        #region//抽奖
        uint time = 0;
        /*刷新信息*/
        void RefreshLottery(GameEvent e)
        {
            refresh_items(e.data["lottery_count"]);
            if (e.data["lottery_count"] >6)
            {
                //cj_txt.text = "次数用尽";
                cj_txt.text = ContMgr.getCont("a3_huoyueonline0");
                btns_lottery.interactable = false;
                return;
            }
            else
            {
                Dictionary<int, online_time> dic = a3_ActiveOnlineModel.getInstance().dic_online;
                if(e.data["online_tm"]>= dic[e.data["lottery_count"]].time)
                {
                    btns_lottery.interactable = true;
                    cj_txt.text = ContMgr.getCont("a3_huoyueonline1");
                }
                else
                {
                    btns_lottery.interactable = false;
                    time = dic[e.data["lottery_count"]].time - e.data["online_tm"];
                    CancelInvoke("timechange");
                    InvokeRepeating("timechange", 0, 1);
                }

            }

        }
        /*刷新奖励*/
        int ids = -1;
        void refresh_items(int id)
        {
            ids = id;
            if (id > 6)
                id = 6;
            if (lst_lottery_obj[0].transform.childCount > 0 && btns_lottery.interactable == false)
                return;
            DesitemObj();

            for (int i = 0; i < lst_lottery_obj.Count; i++)
            {
                GameObject icons = IconImageMgr.getInstance().createA3ItemIcon(
                    (uint)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(id)[i+1].item_id,false,(int)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(id)[i + 1].num);
                icons.transform.SetParent(lst_lottery_obj[i].transform,false);
             

            }
        }
        /*删除*/
         void DesitemObj()
        {
            for (int i = 0; i < lst_lottery_obj.Count; i++)
            {
                if (lst_lottery_obj[i].transform.childCount > 0)
                {
                    DestroyImmediate(lst_lottery_obj[i].transform.GetChild(0).gameObject);
                }
            }
        }

        void timechange()
        {
            time -= 1;

            cj_txt.text = Globle.formatTime((int)time, false);/* time.ToString();*/
            if (time <= 0)
            {
                refresh_items(ids);
                btns_lottery.interactable = true;
                cj_txt.text = ContMgr.getCont("a3_huoyueonline1");
                CancelInvoke("timechange");
            }
        }
        /*按钮功能*/
        void btnOnClick()
        {
            a3_activeOnlineProxy.getInstance().SendProxy(5);
            rotationing = true;
            btns_lottery.interactable = false;
            roll_in.SetActive(true);
            roll_over.SetActive(false);
            bigbg.SetActive(true);
        }

        int getid = -1;
        void lotteryoner(GameEvent e)
        {
            int num = e.data["lottery_count"]-1;
            getid = e.data["lotter_id"];
        }

        /*动画显示获得的奖励*/
        void getItems()
        {
            DesitemObj();
            roll_in.SetActive(false);
            roll_over.SetActive(true);
            for (int i = 0; i < lst_lottery_obj.Count; i++)
            {
                GameObject icons = IconImageMgr.getInstance().createA3ItemIcon(
                    (uint)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(ids)[i + 1].item_id,false,(int)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(ids)[i + 1].num);
                icons.name = a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(ids)[i + 1].item_id.ToString();


                if ((i + 1) == getid)
                    icons.transform.SetParent(lst_lottery_obj[0].transform, false);
                else if (i + 1 > getid)
                {
                    int num = i + 1 - getid;
                    icons.transform.SetParent(lst_lottery_obj[num].transform, false);
                }
                else
                {
                    int num = getid - (i + 1);
                    icons.transform.SetParent(lst_lottery_obj[8 - num].transform, false);
                }
            }
            flytxt.instance.fly(ContMgr.getCont("a3_bag_get") + a3_BagModel.getInstance().getItemDataById((uint)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(ids)[getid].item_id).item_name+
                "*"+ (int)a3_ActiveOnlineModel.getInstance().GetActivelotteryItems(ids)[getid].num);

            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr .NEWBIE);
            ischoujiang = true;
            a3_activeOnlineProxy.getInstance().SendProxy(1);
            bigbg.SetActive(false);


        }

        #endregion
        #region //钻石，转生，战力基金
        /*购买基金*/
        void btn_buyfund(int type)
        {
            a3_activeOnlineProxy.getInstance().SendProxy(3, fund_type: type);
        }
        /*购买后刷新*/
        public void Refreshbuy_fund(int type)
        {
            switch (type)
            {
                case 1:
                    zuanshi_btn.SetActive(false);
                    break;
                case 2:
                    zhuansheng_btn.SetActive(false);
                    break;
                case 3:
                    zhanli_btn.SetActive(false);
                    break;
            }
            RefreshFund_all();
        }
        Dictionary<int, GameObject> fund_obj = new Dictionary<int, GameObject>();
        void creatrve_fund(GameObject image,Transform contain,int type)
        {
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            int num = 0;
            foreach (int i in dic.Keys)
            {
                if (dic[i].type != type)
                    continue;
                GameObject objclone = GameObject.Instantiate(image) as GameObject;
                objclone.SetActive(true);
                objclone.transform.SetParent(contain.transform, false);
                objclone.name = dic[i].id.ToString();
                fund_obj[i] = objclone;
                GameObject icon = objclone.transform.FindChild("icon").gameObject;
                icon.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(dic[i].file);

               Text needmoney = objclone.transform.FindChild("zuanshi/Text").GetComponent<Text>();
                needmoney.text = dic[i].zuanshi_num.ToString();

                Text needmoney1 = objclone.transform.FindChild("bangzuan/Text").GetComponent<Text>();
                needmoney1.text = dic[i].bangzuan_num.ToString();

                Text des = objclone.transform.FindChild("des").GetComponent<Text>();
                des.text = dic[i].des;

                GameObject exp = objclone.transform.FindChild("exp/exp").gameObject;

                int id = dic[i].id;
                new BaseButton(objclone.transform.FindChild("Button")).onClick = (GameObject go) => { buybtnOnclick(id); };

                Text exp_txt= objclone.transform.FindChild("exp/Text").GetComponent<Text>();
                exp_txt.text = "0/" + dic[i].need_paraml;

                objclone.transform.FindChild("Button").gameObject.SetActive(false);

                objclone.transform.FindChild("over").gameObject.SetActive(false);
                num++;
            }
            a3_runestone.commonScroview(contain.gameObject,num);
        }
        /*整体刷新*/
        public void RefreshFund_all()
        {
            zuanshi_btn.SetActive(a3_ActiveOnlineModel.getInstance().zuanshi_fund ? false : true);
            zhuansheng_btn.SetActive(a3_ActiveOnlineModel.getInstance().zhuansheng_fund ? false : true);
            zhanli_btn.SetActive(a3_ActiveOnlineModel.getInstance().zhanli_fund ? false : true);
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            foreach(int i in dic.Keys)
            {
                Refreshfun(i);
            }
        }
        /*单个领取或购买刷新*/
        public void Refreshfun(int id)
        {
            
            //进度,exp，按钮状态
            Dictionary<int, fund_infso> dic = a3_ActiveOnlineModel.getInstance().dic_funds;
            int nub = 0;
            switch (dic[id].type)
            {
                case 1:
                    nub = a3_ActiveOnlineModel.getInstance().zuanshi_fund ? a3_ActiveOnlineModel.getInstance().zuanshi_fundnow:0;
                    break;
                case 2:
                    nub = a3_ActiveOnlineModel.getInstance().zhuansheng_fund ? a3_ActiveOnlineModel.getInstance().zhuansheng_fundnow:0;
                    break;
                case 3:
                    nub = a3_ActiveOnlineModel.getInstance().zhanli_fund ? a3_ActiveOnlineModel.getInstance().zhanli_fundnow:0;
                    break;
            }

            fund_obj[id].transform.FindChild("exp/Text").GetComponent<Text>().text=nub+ "/"+dic[id].need_paraml;
            float x = nub / (float)dic[id].need_paraml;
            if (x >= 1)
                x = 1;
            fund_obj[id].transform.FindChild("exp/exp").GetComponent<Transform>().localScale = new Vector3(x, 1, 1);
            GameObject btn_obj = fund_obj[id].transform.FindChild("Button").gameObject;
            GameObject over_obj= fund_obj[id].transform.FindChild("over").gameObject;
            switch (dic[id].receive_type)
            {
                case 0:
                    btn_obj.SetActive(false);
                    over_obj.SetActive(false);
                    break;
                case 1:
                    btn_obj.SetActive(true);
                    over_obj.SetActive(false);
                    break;
                case 2:
                    btn_obj.SetActive(false);
                    over_obj.SetActive(true);
                    fund_obj[id].transform.SetAsLastSibling();
                    break;

            }




        }
        /*领取奖励*/
        void buybtnOnclick(int id)
        {
            //print("我发送的ID是：" + id);
            a3_activeOnlineProxy.getInstance().SendProxy(4, awd_id: id);

        }
        #endregion




        void closBtnOnclick()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVEONLINE);
        }

        float num = 40;
        void Update()
        {
            if(rotationing)
            {
                num -= 1f;
                cricleob.transform.Rotate(Vector3.forward*40);
                float z = cricleob.GetComponent<Transform>().localRotation.z;

                if (num <= 0 && getid != -1)
                {
                    getItems();
                    cricleob.GetComponent<Transform>().localRotation = new Quaternion(0, 0,0,0);
                    rotationing = false;
                    num = 40;
                }

            }
        }
    }
}
