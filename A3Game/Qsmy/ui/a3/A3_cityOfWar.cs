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
using System.Collections;

namespace MuGame
{
    class A3_cityOfWar: Window
    {
        GameObject inputTip;
        Transform Con_view;

        Transform applyview;
        Transform Prepareview;
        Transform stageCon;
        GameObject apply_clan_item;
        Transform apply_clan_Con;
        GameObject helpCon;
        Text inputCost;
        public override void init() {

            Con_view = this.transform.FindChild("citylist/scroll/content");
            stageCon = this.transform.FindChild("cityinfo/citystage");
            inputTip = transform.FindChild("tip").gameObject;
            apply_clan_item = this.transform.FindChild("viewbg/applyview/scroll/item").gameObject;
            helpCon = this.transform.FindChild("helpCon").gameObject;
            apply_clan_Con = this.transform.FindChild("viewbg/applyview/scroll/content");
            inputCost = inputTip.transform.FindChild("bug/InputField/Text").GetComponent<Text>();
            new BaseButton(transform.FindChild("close")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().close(this.uiName );
            };

            applyview = this.transform.FindChild("viewbg/applyview");
            Prepareview = this.transform.FindChild("viewbg/Prepareview");

            for (int i = 0; i < Con_view.childCount; i++)
            {
                new BaseButton(Con_view.GetChild(i)).onClick = (GameObject go) =>
                {
                    onTab(go.name);  
                };
            }

            new BaseButton (this.transform .FindChild ("letters/apply")).onClick =(GameObject go)=>{
                SetLetter_btn(1);
            };

            new BaseButton(this.transform.FindChild("letters/Prepare")).onClick = (GameObject go) => {
                SetLetter_btn(2);
                change_Id(1);
            };

            new BaseButton(this.transform.FindChild("letters/Prepare/local")).onClick = (GameObject go) => {
                flytxt.instance.fly(ContMgr.getCont("uilayer_A3_cityOfWar_nllopen"));//"备战阶段才可以此操作"
            };

            new BaseButton (this.transform .FindChild ("viewbg/applyview/apply_btn")).onClick = (GameObject go) =>{
                if (A3_LegionModel.getInstance().myLegion.clanc < 3)
                {
                    flytxt.flyUseContId("clan_8");
                    return;
                }
                inputTip.SetActive(true);
                SetApply_Info_Tip();
            };
            new BaseButton(this.transform.FindChild("viewbg/applyview/infb_btn")).onClick = (GameObject go) => {
                //进入副本
                A3_cityOfWarProxy.getInstance().sendInfb();
            };

            new BaseButton(inputTip.transform.FindChild("btn_add")).onClick = (GameObject go) => {
                mineCost = mineCost + A3_cityOfWarModel.getInstance().one_change_cost;
                if (mineCost > A3_LegionModel.getInstance().myLegion.gold)
                {
                    mineCost = mineCost - A3_cityOfWarModel.getInstance().one_change_cost;
                    flytxt.instance.fly(ContMgr.getCont("A3_cityOfWar_nullMoney"));
                    return;
                }
                if (mineCost >= Minimum+ A3_cityOfWarModel .getInstance ().max_cost) {
                        mineCost = Minimum + A3_cityOfWarModel.getInstance().max_cost;
                }
                inputCost.text = mineCost.ToString();
            };

            new BaseButton (inputTip.transform.FindChild("btn_reduce")).onClick=(GameObject go)=>{
                mineCost = mineCost - A3_cityOfWarModel.getInstance().one_change_cost;
                if (mineCost <= Minimum + A3_cityOfWarModel.getInstance().min_cost)
                {
                    if (mineCost <= 0) {
                        mineCost = 0;
                        flytxt.instance.fly(ContMgr.getCont("A3_cityOfWar_nullMoney"));
                    }
                    else 
                        mineCost = Minimum + A3_cityOfWarModel.getInstance().min_cost;
                }
                inputCost.text = mineCost.ToString();
            };

            new BaseButton(inputTip.transform.FindChild("toup")).onClick = (GameObject go) => {
                A3_cityOfWarProxy.getInstance().sendApply((uint)mineCost);
                inputTip.SetActive(false);
            };

            new BaseButton(inputTip.transform.FindChild("close")).onClick = (GameObject go) => {
                inputTip.SetActive(false);
            };

            new BaseButton(this.transform.FindChild("viewbg/applyview/Refresh")).onClick = (GameObject go) => {
                A3_cityOfWarProxy.getInstance().sendProxy(1);
            };


            new BaseButton(this.transform.FindChild("help")).onClick = (GameObject go) => {
                helpCon.SetActive(true);
            };

            new BaseButton(this.transform.FindChild("helpCon/close")).onClick = (GameObject go) => {
                helpCon.SetActive(false);
            };

            new BaseButton(this.transform.FindChild("viewbg/Prepareview/apply_btn")).onClick = (GameObject go) =>
            {
                if (A3_LegionModel.getInstance().myLegion.clanc < 3)
                {
                    flytxt.flyUseContId("clan_8");
                    return;
                }
                if (curSelectId != 0) {
                    A3_cityOfWarProxy.getInstance().sendPrepare((uint)curSelectId);
                }
            };
            inText();
        }

        void inText() {
            this.transform .FindChild ("citylist/scroll/content/skycity/Text").GetComponent <Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_1");//天启城堡
            this.transform.FindChild("citylist/scroll/content/expect/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_2");//敬请期待
            this.transform.FindChild("cityinfo/LegionName").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_3");//所占军团
            this.transform.FindChild("cityinfo/LegionName/null/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_4");//暂无
            this.transform.FindChild("cityinfo/Santoinfo").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_5");//城主
            this.transform.FindChild("cityinfo/Santoinfo/have/combat").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_6");//战力
            this.transform.FindChild("cityinfo/Santoinfo/null/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_7");//暂无
            this.transform.FindChild("cityinfo/citystage").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_8");//当前状态
            this.transform.FindChild("cityinfo/citystage/1/tet1").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_9");//申请攻城
            this.transform.FindChild("cityinfo/citystage/1/tet2").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_10");//消耗军团币进行竞标
            this.transform.FindChild("cityinfo/citystage/1/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_11");//剩余时间：
            this.transform.FindChild("cityinfo/citystage/2/tet1").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_12");//攻城备战阶段
            this.transform.FindChild("cityinfo/citystage/2/tet2").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_13");//可进行战斗设施和属性购买
            this.transform.FindChild("cityinfo/citystage/2/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_14");//攻城时间：
            this.transform.FindChild("cityinfo/citystage/3/tet2").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_15");//正在进行攻城战！
            this.transform.FindChild("cityinfo/citystage/3/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_16");//结束时间：
            this.transform.FindChild("cityinfo/citystage/4/tet2").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_17");//休战期
            this.transform.FindChild("cityinfo/citystage/4/time").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_18");//上周结果
            this.transform.FindChild("cityinfo/reward").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_19");//攻城奖励

            this.transform.FindChild("letters/apply/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_20");//申请
            this.transform.FindChild("letters/Prepare/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_21");//备战
            this.transform.FindChild("viewbg/applyview/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_22");//进攻申请列表
            this.transform.FindChild("viewbg/applyview/Refresh/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_23");//刷新
            this.transform.FindChild("viewbg/applyview/apply_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_24");//申请投标
            this.transform.FindChild("viewbg/applyview/infb_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_25");//进入战场
            this.transform.FindChild("viewbg/Prepareview/apply_btn/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_26");//升级
            this.transform.FindChild("viewbg/Prepareview/apply_btn/1/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_27");//货币
            this.transform.FindChild("viewbg/Prepareview/apply_btn/2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_28");//已满级
            this.transform.FindChild("tip/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_29");//投标
            this.transform.FindChild("tip/toup/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_30");//提交
            this.transform.FindChild("viewbg/applyview/scroll/item/txt22").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_31");//出价：
            this.transform.FindChild("helpCon/scroll/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_cityOfWar_33");

        }
        public override void onShowed()
        {
            A3_cityOfWarProxy.getInstance().addEventListener(A3_cityOfWarProxy.REFRESHINFO, Refresh);
            A3_cityOfWarProxy.getInstance().addEventListener(A3_cityOfWarProxy.REFRESHAPPLY, RefreshApply);
            A3_cityOfWarProxy.getInstance().addEventListener(A3_cityOfWarProxy.REFRESHPREPARE, RefreshPrepare);
            this.transform.SetAsLastSibling();
            A3_cityOfWarProxy.getInstance().sendProxy(1);
            inputTip.SetActive(false);
            helpCon.SetActive(false);

            refreshTime_Stage();
            onTab("skycity");
        }
        public override void onClosed()
        {
            A3_cityOfWarProxy.getInstance().removeEventListener(A3_cityOfWarProxy.REFRESHINFO, Refresh);
            A3_cityOfWarProxy.getInstance().removeEventListener(A3_cityOfWarProxy.REFRESHAPPLY, RefreshApply);
            A3_cityOfWarProxy.getInstance().removeEventListener(A3_cityOfWarProxy.REFRESHPREPARE, RefreshPrepare);
            CancelInvoke("refreshTime_Apply");
            CancelInvoke("refreshTime_Waring");
            CancelInvoke("refreshTime_WarOver");
            CancelInvoke("refreshTime_Prepare");
        }

        void SetLetter_btn(int type) {
            if (type == 1) {
                this.transform.FindChild("letters/apply").GetComponent<Button>().interactable = false;
                this.transform.FindChild("letters/Prepare").GetComponent<Button>().interactable = true;
                applyview.gameObject.SetActive(true);
                Prepareview.gameObject.SetActive(false);
            }
            else if (type == 2) {
                this.transform.FindChild("letters/apply").GetComponent<Button>().interactable = true;
                this.transform.FindChild("letters/Prepare").GetComponent<Button>().interactable = false;
                applyview.gameObject.SetActive(false);
                Prepareview.gameObject.SetActive(true);
            }
        }

        void RefreshApply(GameEvent e) {
            for (int i = 0; i < apply_clan_Con.childCount;i++ ) {
                Destroy(apply_clan_Con.GetChild (i).gameObject);
            }
            for (int i = 0; i < A3_cityOfWarModel.getInstance ().apply_list.Count; i++) {
                GameObject clon = Instantiate(apply_clan_item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(apply_clan_Con);
                clon.transform.FindChild("idx").GetComponent<Text>().text = (i + 1).ToString();
                Apply_Info info = A3_cityOfWarModel.getInstance().GetApplyInfo_One(i+1);
                clon.transform.FindChild("lvl/tet").GetComponent<Text>().text = info.clan_lvl.ToString();
                clon.transform.FindChild("name").GetComponent<Text>().text = info.clan_name;
                clon.transform.FindChild("money").GetComponent<Text>().text = info.apply_num.ToString() ;
            }
        }


        void RefreshPrepare(GameEvent e) { 
     
            foreach (int id in def_objList.Keys) {
                if (A3_cityOfWarModel.getInstance().deflist.ContainsKey(id))
                { 
                    SXML Xml = XMLMgr.instance.GetSXML("clanwar.def_info", "id==" + id);
                    def_objList[id].transform.FindChild("lvl/tet").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().deflist[id]._lvl.ToString();
                    SXML Xml_lvl = Xml.GetNode("lvl", "lv==" + A3_cityOfWarModel.getInstance().deflist[id]._lvl);
                    def_objList[id].transform.FindChild("att").GetComponent<Text>().text = Xml.getString ("value_name")+ "+" + Xml_lvl.getInt("value");
                }
                else
                {
                    SXML Xml = XMLMgr.instance.GetSXML("clanwar.def_info", "id==" + id);
                    def_objList[id].transform.FindChild("lvl/tet").GetComponent<Text>().text = "0";
                    def_objList[id].transform.FindChild("att").GetComponent<Text>().text = Xml.getString("value_name") + "+" +"0";
                }
            }
            SetMoney();
        }

        Dictionary<int, GameObject> def_objList = new Dictionary<int, GameObject>();

        int curSelectId = 0;

        void SetPrepareCon() {
            SXML Xml = XMLMgr.instance.GetSXML("clanwar");
            List<SXML> def_list = Xml.GetNodeList("def_info");
            GameObject item = this.transform.FindChild("viewbg/Prepareview/scroll/item").gameObject;
            Transform defCon = this.transform.FindChild("viewbg/Prepareview/scroll/content");

            for (int i = 0; i < defCon.childCount; i++ ) {
                Destroy(defCon.GetChild(i).gameObject);
            }
            def_objList.Clear();
            foreach (SXML one in def_list) {
                GameObject clon = Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(defCon,false);
                clon.transform.FindChild("name").GetComponent<Text>().text = one.getString("name");
                clon.transform.FindChild("this").gameObject.SetActive(false);
                def_objList[one.getInt("id")] = clon;
                int id = one.getInt("id");
                new BaseButton(clon.transform).onClick = (GameObject go) => {
                    change_Id(id);
                };
            }
        }

        void change_Id(int id) {
            foreach (GameObject obj in def_objList.Values)
            {
                obj.transform.FindChild("this").gameObject.SetActive(false);
            }
            def_objList[id].transform.FindChild("this").gameObject.SetActive(true);
            curSelectId = id;
            SetMoney();
        }

        void SetMoney()
        {
            if (A3_cityOfWarModel.getInstance().deflist.ContainsKey(curSelectId))
            {
                int lvl = A3_cityOfWarModel.getInstance().deflist[curSelectId]._lvl + 1;
                SXML Xml = XMLMgr.instance.GetSXML("clanwar.def_info", "id==" + curSelectId);
                if (Xml == null) return;
                SXML Xml_lvl = Xml.GetNode("lvl", "lv==" + lvl);
                if (Xml_lvl != null)
                {
                    this.transform.FindChild("viewbg/Prepareview/apply_btn/1").gameObject.SetActive(true);
                    this.transform.FindChild("viewbg/Prepareview/apply_btn/2").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/Prepareview/apply_btn/1/money").GetComponent<Text>().text = Xml_lvl.getInt("cost").ToString();
                }
                else
                {
                    this.transform.FindChild("viewbg/Prepareview/apply_btn/1").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/Prepareview/apply_btn/2").gameObject.SetActive(true);
                }
            }
            else
            {
                this.transform.FindChild("viewbg/Prepareview/apply_btn/1").gameObject.SetActive(true);
                this.transform.FindChild("viewbg/Prepareview/apply_btn/2").gameObject.SetActive(false);
                int lvl = 1;
                SXML Xml = XMLMgr.instance.GetSXML("clanwar.def_info", "id==" + curSelectId);
                if (Xml == null) return;
                SXML Xml_lvl = Xml.GetNode("lvl", "lv==" + lvl);
                this.transform.FindChild("viewbg/Prepareview/apply_btn/1/money").GetComponent<Text>().text = Xml_lvl.getInt("cost").ToString();
            }
        }

        void onTab(string name)
        {
            if (name == "expect") { return; }
                
            for (int i = 0; i < Con_view.childCount; i++)
            {
                if (Con_view.GetChild(i).gameObject.name == "expect") continue;
                Con_view.GetChild(i).GetComponent<Button>().interactable = true;
            }

            Con_view.FindChild (name).GetComponent<Button>().interactable = false;

            switch (name) {
                case "skycity":
                    //天启城堡
                    SetFor_SkyCity();
                    setAwd();
                    SetPrepareCon();
                    RefreshApply(null);
                    RefreshPrepare(null);
                    break;
            }
            SetLetter_btn(1);
        }

        void refreshTime_Stage() {

            TimeType a = A3_cityOfWarModel.getInstance().checkTime();
            SetWar_stage(a);
        }

        void setAwd() {
            RectTransform con = this.transform.FindChild("cityinfo/reward/scroll/content").GetComponent<RectTransform>();
            for (int i = 0; i < con.childCount;i++) {
                Destroy(con.GetChild (i).gameObject);
            }
            GameObject itembg = this.transform.FindChild("cityinfo/reward/scroll/icon").gameObject;
            int count = 0;
            SXML Xml = XMLMgr.instance.GetSXML("clanwar.awd_all");
            List<SXML> awds_coin = Xml.GetNodeList("RewardValue");
            List<SXML> awds_item = Xml.GetNodeList("RewardItem");
            if (awds_coin.Count > 0 ) {
                foreach (SXML x in awds_coin) {
                    a3_ItemData item = new a3_ItemData();
                    item.borderfile = "icon_itemborder_b039_05";
                    if (x.getInt("type") == 2)
                        item.file = "icon_comm_0x2";
                    else if (x.getInt("type") == 3)
                        item.file = "icon_coin_coin2";
                    else if (x.getInt("type") == 4)
                        item.file = "icon_coin_coin3";
                    else if (x.getInt("type") == 5)
                        item.file = "icon_coin_coin4";
                    else
                        item.file = "icon_comm_0x2";
                    GameObject clon = Instantiate(itembg) as GameObject;
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item, false, -1, 0.8f);
                    icon.transform.SetParent(clon.transform,false);
                    clon.SetActive(true);
                    clon.transform.SetParent(con, false);
                    count++;
                }
            }

            if (awds_item.Count > 0) {
                foreach (SXML x in awds_item) {
                    int iconid = x.getInt("item_id");
                    a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)iconid);
                    GameObject clon = Instantiate(itembg) as GameObject;
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item, false, -1, 0.8f);
                    icon.transform.SetParent(clon.transform, false);
                    clon.SetActive(true);
                    clon.transform.SetParent(con, false);
                    new BaseButton(clon.transform).onClick = (GameObject go) => {
                        ArrayList arr = new ArrayList();
                        arr.Add((uint)iconid);//第一项为物品id
                        arr.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                    };
                    count++;
                }
            }
            float childSizeX = con.GetComponent<GridLayoutGroup>().cellSize .x;
            float spaceX = con.GetComponent<GridLayoutGroup>().spacing.x;
            Vector2 newSize = new Vector2(count * childSizeX + (count - 1)* spaceX, con.sizeDelta.y);
            con.sizeDelta = newSize;
            con.anchoredPosition = Vector2.zero;
        }

        int Minimum = 0;
        int mineCost = 0;
        void SetApply_Info_Tip() {
            Apply_Info info;
            if (A3_cityOfWarModel.getInstance().checkMineClan() != null) {
                info = A3_cityOfWarModel.getInstance().checkMineClan();
            }
            else
                info = A3_cityOfWarModel.getInstance().GetApplyInfo_One(3);
            if (info == null) { Minimum = 0; }
            else Minimum = info.apply_num;
            mineCost = Minimum + A3_cityOfWarModel.getInstance().min_cost;
            if (mineCost > A3_LegionModel.getInstance().myLegion.gold)
            {
                mineCost = 0;
            }
            inputTip.transform.FindChild("lastcount").GetComponent<Text>().text = Minimum.ToString ();
            inputCost.text = mineCost.ToString() ;
        }

        void SetWar_stage(TimeType type) {
            for (int i = 0; i < stageCon.childCount;i++ ) {
                stageCon.GetChild(i).gameObject.SetActive(false) ;
            }
            switch (type) {
                case TimeType.ApplyTime:
                    stageCon.FindChild("1").gameObject.SetActive(true);
                    InvokeRepeating("refreshTime_Apply", 0, 1f);
                    SetLetter_btn(1);
                    break;
                case TimeType.PrepareTime:
                    stageCon.FindChild("2").gameObject.SetActive(true);
                    SetTime_Prepare();
                    SetLetter_btn(2);
                    InvokeRepeating("refreshTime_Prepare", 0, 1f);
                    break;
                case TimeType.WarStart:
                    stageCon.FindChild("3").gameObject.SetActive(true);
                    InvokeRepeating("refreshTime_Waring", 0, 1f);
                    SetLetter_btn(1);
                    break;
                case TimeType.WarOver:
                    stageCon.FindChild("4").gameObject.SetActive(true);
                    InvokeRepeating("refreshTime_WarOver", 0, 1f);
                    setWar_Result();
                    SetLetter_btn(1);
                    break;
            }
            SetWinBtn(type);
        }
         
        void refreshTime_Apply() { 
            DateTime dateTime = DateTime.Now;
            int hh = A3_cityOfWarModel.getInstance().apply_tm_end_h; 
            DateTime overTime = DateTime.Parse(dateTime.ToString ("yyyy/MM/dd") +" "+ hh.ToString() + ":00:00");
            TimeSpan span = overTime - dateTime;
            if (span.Hours > 0)
                stageCon.FindChild("1/time/tet").GetComponent<Text>().text = span.Hours + ContMgr.getCont("hour") + span.Minutes + ContMgr.getCont("mine") + span.Seconds + ContMgr.getCont("miao");
            else if (span.Hours <= 0 && span.Minutes > 0)
                stageCon.FindChild("1/time/tet").GetComponent<Text>().text = span.Minutes + ContMgr.getCont("mine") + span.Seconds + ContMgr.getCont("miao");
            else if (span.Hours <= 0 && span.Minutes <= 0)
                stageCon.FindChild("1/time/tet").GetComponent<Text>().text = span.Seconds + ContMgr.getCont("miao");
            if (span.Hours <= 0 && span.Minutes <=0 && span.Seconds < 0) {
                //时间到，刷新界面
               Debug.LogError(dateTime);
               refreshTime_Stage();
               CancelInvoke("refreshTime_Apply");
            }
        }

        void refreshTime_Waring() {
            DateTime dateTime = DateTime.Now;
            int hh = A3_cityOfWarModel.getInstance().open_tm_end_h;
            int mm = A3_cityOfWarModel.getInstance().open_tm_end_m;
            DateTime overTime = DateTime.Parse(dateTime.ToString("yyyy/MM/dd") + " " + hh.ToString() + ":"+mm.ToString ()+":00");
            TimeSpan span = overTime - dateTime;
            stageCon.FindChild("3/time/tet").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().GetTimestr(span.Hours) + ":" + A3_cityOfWarModel.getInstance().GetTimestr(span.Minutes)+ ":" + A3_cityOfWarModel .getInstance ().GetTimestr (span.Seconds) ;
            if (span.Hours <= 0 && span.Minutes <= 0 && span.Seconds < 0)
            {
                //时间到，刷新界面
                refreshTime_Stage();
                CancelInvoke("refreshTime_Waring");
            }
        }

        void refreshTime_WarOver() {
            DateTime dateTime = DateTime.Now;
            if (dateTime.DayOfWeek == A3_cityOfWarModel.getInstance().GetWeek(A3_cityOfWarModel.getInstance().apply_tm_day)) {
                int hh = A3_cityOfWarModel.getInstance().apply_tm_start_h;
                DateTime overTime = DateTime.Parse(dateTime.ToString("yyyy/MM/dd") + " " + hh.ToString() + ":00:00");
                TimeSpan span = overTime - dateTime;
                if (span.Hours <= 0 && span.Minutes <= 0 && span.Seconds < 0)
                {
                    //时间到，刷新界面
                    refreshTime_Stage();
                    CancelInvoke("refreshTime_WarOver");
                }
            }
        }

        void refreshTime_Prepare() {
            DateTime dateTime = DateTime.Now;
            if (dateTime.DayOfWeek == A3_cityOfWarModel.getInstance().GetWeek(A3_cityOfWarModel.getInstance().open_tm_day))
            {
                int hh = A3_cityOfWarModel.getInstance().open_tm_start_h;
                int mm = A3_cityOfWarModel.getInstance().open_tm_start_m;
                DateTime overTime = DateTime.Parse(dateTime.ToString("yyyy/MM/dd") + " " + hh.ToString() + ":" + mm.ToString() + ":00");
                TimeSpan span = overTime - dateTime;
                if (span.Hours <= 0 && span.Minutes <= 0 && span.Seconds < 0)
                {
                    //时间到，刷新界面
                    refreshTime_Stage();
                    CancelInvoke("refreshTime_Prepare");
                }
            }
        }

        void SetTime_Prepare() {
            string str = "";
            switch (A3_cityOfWarModel .getInstance ().open_tm_day) {
                case 1:
                    str = ContMgr.getCont("week1");
                    break;
                case 2:
                    str = ContMgr.getCont("week2");
                    break;
                case 3:
                    str = ContMgr.getCont("week3");
                    break;
                case 4:
                    str = ContMgr.getCont("week4");
                    break;
                case 5:
                    str = ContMgr.getCont("week5");
                    break;
                case 6:
                    str = ContMgr.getCont("week6");
                    break;
                case 7:
                    str = ContMgr.getCont("week7");
                    break;
            }
            str += A3_cityOfWarModel.getInstance().open_tm_start_h+ ContMgr.getCont("shi");
            if (A3_cityOfWarModel .getInstance ().open_tm_start_m > 0) {
                str += A3_cityOfWarModel.getInstance().GetTimestr(A3_cityOfWarModel.getInstance().open_tm_start_m)+ ContMgr.getCont("mine");
            }

            str += ContMgr.getCont("kaishi");

            stageCon.FindChild("2/time/tet").GetComponent<Text>().text = str;
        }


        void setWar_Result() {
            stageCon.FindChild("4/time/tet").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().GetStage_Str();
        }

        void SetWinBtn(TimeType type) {
            switch (type) {
                case TimeType.ApplyTime:
                    this.transform.FindChild("letters/apply/local").gameObject.SetActive(false);
                    this.transform.FindChild("letters/Prepare/local").gameObject.SetActive(true);
                    if ( A3_LegionModel.getInstance().myLegion.id != A3_cityOfWarModel.getInstance().def_clanid)
                        this.transform.FindChild("viewbg/applyview/apply_btn").gameObject.SetActive(true);
                    else
                        this.transform.FindChild("viewbg/applyview/apply_btn").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/applyview/infb_btn").gameObject.SetActive(false);
                    break;
                case TimeType.PrepareTime:
                    this.transform.FindChild("letters/apply/local").gameObject.SetActive(false);
                    this.transform.FindChild("letters/Prepare/local").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/applyview/apply_btn").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/applyview/infb_btn").gameObject.SetActive(false);
                    break;
                case TimeType.WarStart:
                    this.transform.FindChild("letters/apply/local").gameObject.SetActive(false);
                    this.transform.FindChild("letters/Prepare/local").gameObject.SetActive(true);
                    this.transform.FindChild("viewbg/applyview/apply_btn").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/applyview/infb_btn").gameObject.SetActive(true);
                    break;
                case TimeType.WarOver:
                    this.transform.FindChild("letters/apply/local").gameObject.SetActive(false);
                    this.transform.FindChild("letters/Prepare/local").gameObject.SetActive(true);
                    this.transform.FindChild("viewbg/applyview/apply_btn").gameObject.SetActive(false);
                    this.transform.FindChild("viewbg/applyview/infb_btn").gameObject.SetActive(false);
                    break;
            }

        }


        void SetFor_SkyCity() {
            if (A3_cityOfWarModel.getInstance().def_clanid == 0)
            {
                this.transform.FindChild("cityinfo/LegionName/null").gameObject.SetActive(true);
                this.transform.FindChild("cityinfo/LegionName/have").gameObject.SetActive(false);
                this.transform.FindChild("cityinfo/Santoinfo/null").gameObject.SetActive(true);
                this.transform.FindChild("cityinfo/Santoinfo/have").gameObject.SetActive(false);
            }
            else {
                this.transform.FindChild("cityinfo/LegionName/null").gameObject.SetActive(false);
                this.transform.FindChild("cityinfo/LegionName/have").gameObject.SetActive(true);
                this.transform.FindChild("cityinfo/Santoinfo/null").gameObject.SetActive(false);
                this.transform.FindChild("cityinfo/Santoinfo/have").gameObject.SetActive(true);
                this.transform.FindChild("cityinfo/LegionName/have/lvl").GetComponent<Text>().text = "LV" + A3_cityOfWarModel.getInstance().clan_lvl;
                this.transform.FindChild("cityinfo/LegionName/have/tet").GetComponent<Text>().text =  A3_cityOfWarModel.getInstance().clan_name;
                this.transform .FindChild ("cityinfo/Santoinfo/have/lvl").GetComponent <Text>().text = ContMgr.getCont("worldmap_lv", new List<string> { A3_cityOfWarModel .getInstance ().Castellan_zhuan.ToString(), A3_cityOfWarModel.getInstance().Castellan_lvl.ToString() });
                this.transform.FindChild("cityinfo/Santoinfo/have/name").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().Castellan_name;
                this.transform.FindChild("cityinfo/Santoinfo/have/combat/tet").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().Castellan_combpt.ToString();
                this.transform.FindChild("cityinfo/Santoinfo/have/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_" + A3_cityOfWarModel.getInstance().Castellan_carr);
            }
        }


        void Refresh(GameEvent e) {
            //暂时只有天启城，先不做区分判断
            SetFor_SkyCity();
            Variant data = e.data;

            if (data.ContainsKey ("llid")) {
                if (A3_cityOfWarModel .getInstance ().llid  != data["llid"]) {
                    refreshTime_Stage();
                }
            }
            if (data.ContainsKey("apply_list"))
            {
                RefreshApply(null);
            }
        }
    }
}
