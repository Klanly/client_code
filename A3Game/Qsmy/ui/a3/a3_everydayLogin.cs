using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

namespace MuGame
{
    class a3_everydayLogin : Window
    {
        private Transform _content;
        //private Transform _itemAwardPrefab;
        //private BaseButton _btnGet;
        uint day_count;
        //Transform _toggleImage;
        Transform _canget;
        public static a3_everydayLogin instans;
        public static a3_everydayLogin _instans;
        public GameObject thisobj = null;
        public GameObject thisGetbtn = null;
        public bool open = true;
        override public void init()
        {
            instans = this;
            inText();
            new BaseButton(transform.FindChild("title/bgClose")).onClick = onBtnClose;
            _content = transform.FindChild("body/awardItems/content");
            BaseButton btnClose = new BaseButton(transform.FindChild("title/btnClose"));
            btnClose.onClick = onBtnClose;
            //welfareProxy.getInstance().addEventListener(welfareProxy.SHOWDAILYRECHARGE, onShowDaily); 
            if (a3_pet_renew.instance != null)
            {
                transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            }
        }

        void inText()
        {
            this.transform.FindChild("body/awardItems/content/itemAward1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_1");//第一天
            this.transform.FindChild("body/awardItems/content/itemAward1/Text (1)").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_everydayLogin_2"));//升级宝石100个{n}金币50万
            this.transform.FindChild("body/awardItems/content/itemAward2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_3");//第二天
            this.transform.FindChild("body/awardItems/content/itemAward2/Text (1)").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_everydayLogin_4"));//烈焰火种100个{n}金币100万
            this.transform.FindChild("body/awardItems/content/itemAward3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_5");//第三天
            this.transform.FindChild("body/awardItems/content/itemAward3/Text (1)").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_everydayLogin_6"));//魔晶100个{n}金币150万
            this.transform.FindChild("body/awardItems/content/itemAward4/Text (1)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_7");//第四天
            this.transform.FindChild("body/awardItems/content/itemAward4/Text (1)").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_everydayLogin_8"));//火狮召唤兽{n}金币200万
            this.transform.FindChild("body/awardItems/content/itemAward5/Text (1)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_9");//第五天
            this.transform.FindChild("body/awardItems/content/itemAward5/Text (1)").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_everydayLogin_10"));//升级宝石500个{n}金币250万
            this.transform.FindChild("body/awardItems/content/itemAward6/Text (1)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_11");//第六天
            this.transform.FindChild("body/awardItems/content/itemAward6/Text (1)").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_everydayLogin_12"));//魔晶500个{n}金币300万
            this.transform.FindChild("body/awardItems/content/itemAward7/Text (1)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_13");//第七天
            this.transform.FindChild("body/awardItems/content/itemAward7/Text (1)").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_everydayLogin_14"));//金色装备{n}金币350万
            this.transform.FindChild("body/awardItems/content/itemAward1/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");//领取
            this.transform.FindChild("body/awardItems/content/itemAward2/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
            this.transform.FindChild("body/awardItems/content/itemAward3/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
            this.transform.FindChild("body/awardItems/content/itemAward4/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
            this.transform.FindChild("body/awardItems/content/itemAward5/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
            this.transform.FindChild("body/awardItems/content/itemAward6/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
            this.transform.FindChild("body/awardItems/content/itemAward7/btnGet/canget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_everydayLogin_15");
        }
        void onBtnClose(GameObject go)
        {
            if (a3_new_pet.instance != null)
                a3_new_pet.instance.openEveryLogin = false;
            open = false;
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.one;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EVERYDAYLOGIN);
        }
        override public void onShowed()
        {
            welfareProxy.getInstance().addEventListener(welfareProxy.SHOWDAILYRECHARGE, onShowDaily);
            _instans = this;
            if (uiData != null)
            {
                InterfaceMgr.getInstance().floatUI.localScale = Vector3.zero;
                uint last_day = (uint)uiData[0];//最后领取的时间
                day_count = (uint)uiData[1];//领奖的天数位置
                createAward(day_count);
            }
            //if (day_count == 6)
            //{
            //    _canget.gameObject.SetActive(true);
            //}
        }

        override public void onClosed()
        {
            welfareProxy.getInstance().removeEventListener(welfareProxy.SHOWDAILYRECHARGE, onShowDaily);
            InterfaceMgr.getInstance().floatUI.localScale = Vector3.one;
        }

        // List<itemAward> objlist = new List<itemAward>();
        Dictionary<int, itemAwardData> awdlist = new Dictionary<int, itemAwardData>();
        void createAward(uint dayCount)
        {
            //for (int n=0;n< objlist.Count;n++) {
            //    objlist[n].des();
            //}
            //objlist.Clear();
            awdlist.Clear();
            List<WelfareModel.itemWelfareData> iwdList = WelfareModel.getInstance().getDailyLogin();
            for (int i = 0; i < iwdList.Count; i++)
            {
                if (i < 7)
                {

                    WelfareModel.itemWelfareData iwd = iwdList[i];
                    a3_ItemData itemData = a3_BagModel.getInstance().getItemDataById(iwd.itemId);
                    bool claim = false;
                    bool canget = false;
                    if (i < dayCount) claim = true;

                    if (i == dayCount) canget = true;

                    GameObject goItemAward = transform.FindChild("body/awardItems/content/itemAward" + (i + 1)).gameObject;

                    itemAwardData iad = new itemAwardData();
                    iad.parent = _content;
                    iad.trSelf = goItemAward.transform;
                    //iad.trAward = goIcon.transform;./seven_day 1
                    iad.dayNum = (uint)i;
                    iad.dayCount = dayCount;
                    iad.isChecked = claim;
                    iad.canGet = canget;
                    iad.name = itemData.item_name;
                    iad.awardnum = iwd.num;
                    // itemAward ad = new itemAward(iad);
                    // objlist.Add(ad);
                    SetAwd(iad);
                    awdlist[i + 1] = iad;
                }
            }

        }
        //bool _isChecked = false;
        void SetAwd(itemAwardData iad)
        {
            Transform trans = iad.trSelf;
            //Transform _root = iad.trSelf;
            //_root.SetParent(iad.parent);
           // _root.localScale = Vector3.one;
            // BaseButton _btnGet = new BaseButton(trans.FindChild("btnGet"));
            // _btnGet.onClick = onThisClick;
            Transform _toggleImage = trans.transform.FindChild("btnGet/toggleImage");
            Transform _trAwardParent = trans.transform.FindChild("btnGet/parent");
            Transform canget_obj = trans.transform.FindChild("btnGet/canget");

            uint _dayNum = iad.dayNum;
            uint _dayCount = iad.dayCount;
            _toggleImage.gameObject.SetActive(iad.isChecked);


            if (_dayNum == _dayCount)
            {
                canget_obj.gameObject.SetActive(true);
                trans.transform.FindChild("btnGet/this").gameObject.SetActive(true);
                new BaseButton(canget_obj.transform).onClick = (GameObject go) => {
                    if (_dayNum < _dayCount) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_oldlq")); return; }
                    if (_dayNum > _dayCount) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_nolq")); return; }
                   // if (_isChecked) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_oldlq")); return; }
                    if (a3_new_pet.instance != null)
                        a3_new_pet.instance.openEveryLogin = false;
                    a3_everydayLogin.instans.open = false;
                    welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.accumulateLogin);
                    a3_expbar.instance.getGameObjectByPath("operator/LightTips/everyDayLogin").SetActive(false);

                };
                trans.transform.FindChild("icon").gameObject.SetActive(true);
                trans.transform.FindChild("iconh").gameObject.SetActive(false);
            }
            else
            {
                canget_obj.gameObject.SetActive(false);
                trans.transform.FindChild("btnGet/this").gameObject.SetActive(false);
                trans.transform.FindChild("icon").gameObject.SetActive(false);
                trans.transform.FindChild("iconh").gameObject.SetActive(true);
            }

            string _name = iad.name;
            uint _num = iad.awardnum;

        }

        public void onBtnGetClick(GameObject go)
        {

            if (7 != day_count + 1) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_nolq")); return; }
            if (7 == day_count) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_oldlq")); return; }
            if (a3_new_pet.instance != null)
                a3_new_pet.instance.openEveryLogin = false;
            open = false;
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.accumulateLogin);
            
        }
        void onShowDaily(GameEvent e)
        {
            Variant data = e.data;
            int day_count = data["day_count"];
            if (awdlist.ContainsKey (day_count))
            {
               // _isChecked = true;
                string strFly = ContMgr.getCont("everDayLogin_flytxt0", new List<string>() { awdlist[day_count].name });
                flytxt.instance.fly(strFly + awdlist[day_count].awardnum);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EVERYDAYLOGIN);
            }
        }
        //class itemAward : Skin
        //{
        //    public uint _dayNum;
        //    public uint _dayCount;
        //    Transform _root;
        //    Transform _toggleImage;
        //    Transform canget_obj;
        //    Text _txtDay;
        //    BaseButton _btnGet;
        //    bool _isChecked = false;
        //    string _name;
        //    uint _num;
        //    List<string> strDay = new List<string> { ContMgr.getCont("one"), ContMgr.getCont("two"), ContMgr.getCont("three"), ContMgr.getCont("four"), ContMgr.getCont("five"), ContMgr.getCont("six"), ContMgr.getCont("seven") };
        //    public itemAward(itemAwardData iad)
        //        : base(iad.trSelf)
        //    {
        //        Transform trans = iad.trSelf;
        //        _root = iad.trSelf;
        //        _root.SetParent(iad.parent);
        //        _root.localScale = Vector3.one;
        //         _btnGet = new BaseButton(trans.FindChild("btnGet"));
        //        // _btnGet.onClick = onThisClick;
        //        _toggleImage = getTransformByPath("btnGet/toggleImage");
        //        Transform _trAwardParent = getTransformByPath("btnGet/parent");
        //        canget_obj = getTransformByPath("btnGet/canget");

        //        _dayNum = iad.dayNum;
        //        _dayCount = iad.dayCount;
        //        _toggleImage.gameObject.SetActive(iad.isChecked);


        //        if (_dayNum == _dayCount)
        //        {
        //            canget_obj.gameObject.SetActive(true);
        //            transform.FindChild("btnGet/this").gameObject.SetActive(true);
        //            new BaseButton(canget_obj.transform).onClick = onBtnGetClick;
        //            transform.FindChild("icon").gameObject.SetActive(true);
        //            transform.FindChild("iconh").gameObject.SetActive(false);
        //            welfareProxy.getInstance().addEventListener(welfareProxy.SHOWDAILYRECHARGE, onShowDaily);
        //        }
        //        else
        //        {
        //            canget_obj.gameObject.SetActive(false);
        //            transform.FindChild("btnGet/this").gameObject.SetActive(false);
        //            transform.FindChild("icon").gameObject.SetActive(false);
        //            transform.FindChild("iconh").gameObject.SetActive(true);
        //        }

        //        _name = iad.name;
        //        _num = iad.awardnum;
        //    }

        //    public void des() {
        //        welfareProxy.getInstance().removeEventListener (welfareProxy.SHOWDAILYRECHARGE, onShowDaily);
        //    }
        //    void onShowDaily(GameEvent e)
        //    {
        //        Variant data = e.data;
        //        uint day_count = data["day_count"];
        //        if (day_count == _dayNum + 1)
        //        {
        //            _isChecked = true;
        //            string strFly = ContMgr.getCont("everDayLogin_flytxt0", new List<string>() { _name });
        //            flytxt.instance.fly(strFly + _num);
        //            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EVERYDAYLOGIN);
        //        }

        //    }
        //    void onBtnGetClick(GameObject go)
        //    {
        //        if (_dayNum < _dayCount) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_oldlq")); return; }
        //        if (_dayNum > _dayCount) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_nolq")); return; }
        //        if (_isChecked) { flytxt.instance.fly(ContMgr.getCont("a3_everydayLogin_oldlq")); return; }
        //        if (a3_new_pet.instance != null)
        //            a3_new_pet.instance.openEveryLogin = false;
        //        a3_everydayLogin.instans.open = false;
        //        welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.accumulateLogin);
        //        a3_expbar.instance.getGameObjectByPath("operator/LightTips/everyDayLogin").SetActive(false);
        //    }

        //    void onThisClick(GameObject go)
        //    {
        //        if (a3_everydayLogin.instans.thisobj != null)
        //        {
        //            a3_everydayLogin.instans.thisobj.SetActive(false);
        //        }
        //        if (a3_everydayLogin.instans.thisGetbtn != null)
        //        {
        //            a3_everydayLogin.instans.thisGetbtn.SetActive(false);
        //        }

        //        else
        //        {
        //            canget_obj.gameObject.SetActive(false);
        //        }

        //        a3_everydayLogin.instans.thisobj = go.transform.FindChild("this").gameObject;

        //    }

        //}
        class itemAwardData
        {
            public Transform parent;
            public Transform trSelf;
            public Transform trAward;
            public uint dayNum;
            public uint dayCount;
            public bool isChecked;
            public bool canGet;
            public string name;
            public uint awardnum;
        }
    }
}
