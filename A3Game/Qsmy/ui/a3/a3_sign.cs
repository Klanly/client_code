using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Cross;
using DG.Tweening;

namespace MuGame
{
    class a3_sign : Window
    {
        int thisday = DateTime.Now.Day;
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int weekinone;//1号星期几
        int thismonthcout;
        int repairsigncount = 0;//补签的天数
        Text timeover; //到期时间
        List<int> list = new List<int>();//今天之前没有签的
        List<int> lst;//累计奖励

        GameObject[] risen_rewards;//5个累计奖励
        GameObject risen_reward;

        GameObject thisday_panel;//当日奖励容器

        public static a3_sign instan;

        Text repairsignglod;
        Text yesallglod;
        GameObject signbtn_panel;
        Button allrepairsignbtn;
        Button signbtn;
        GameObject buy_cardbtn;
        GameObject busign_bg;

        Dictionary<int, GameObject> dic_obj;
        Dictionary<int, GameObject> robj;
        GameObject image_day;
        GameObject contain;
        Text thismonth;
        Text addsigns;
        int addsigns_num;
        bool thisdayissign = false;
        Transform libg;
        bool canbuqian = false;
        public bool returnthis =false;
        public override void init()
        {



            getComponentByPath<Text>("bg/Text/Text").text = ContMgr.getCont("a3_sign_0");
            getComponentByPath<Text>("bg/total/day").text = ContMgr.getCont("a3_sign_1");
            getComponentByPath<Text>("panel_down/signbtn/allrepairsign/Text").text = ContMgr.getCont("a3_sign_2");
            getComponentByPath<Text>("panel_down/signbtn/sign/Text").text = ContMgr.getCont("a3_sign_3");
            getComponentByPath<Text>("panel_down/signbtn/time/time").text = ContMgr.getCont("a3_sign_4");
            getComponentByPath<Text>("panel_down/buy_cardbtn/Text").text = ContMgr.getCont("a3_sign_5");
            getComponentByPath<Text>("panel_down/title/Text").text = ContMgr.getCont("a3_sign_0");
            getComponentByPath<Text>("busign_bg/Image/Text").text = ContMgr.getCont("a3_sign_6");
            getComponentByPath<Text>("busign_bg/Image/Text1").text = ContMgr.getCont("a3_sign_7");
            getComponentByPath<Text>("busign_bg/Image/no/Text").text = ContMgr.getCont("a3_sign_8");
            getComponentByPath<Text>("busign_bg/Image/yes/Text").text = ContMgr.getCont("a3_sign_6");
            getComponentByPath<Text>("desc/bg/name/has").text = ContMgr.getCont("a3_sign_9");
            getComponentByPath<Text>("desc/bg/name/lite").text = ContMgr.getCont("a3_sign_10");
            getComponentByPath<Text>("rewInfo/Text").text = ContMgr.getCont("a3_sign_11");




            instan = this;
            libg = transform.FindChild("panel_top/libg");
            dic_obj = new Dictionary<int, GameObject>();
            robj = new Dictionary<int, GameObject>();
            image_day = transform.FindChild("panel_centre/image").gameObject;
            contain = transform.FindChild("panel_centre/contain").gameObject;
            signbtn_panel = transform.FindChild("panel_down/signbtn").gameObject;
            buy_cardbtn = transform.FindChild("panel_down/buy_cardbtn").gameObject;
            thisday_panel = transform.FindChild("panel_down/thisday_reward/group").gameObject;
            timeover = transform.FindChild("panel_down/signbtn/time").GetComponent<Text>();
            allrepairsignbtn = transform.FindChild("panel_down/signbtn/allrepairsign").GetComponent<Button>();
            repairsignglod = transform.FindChild("panel_down/signbtn/allrepairsign/gold").GetComponent<Text>();
            yesallglod = transform.FindChild("busign_bg/Image/yes/gold").GetComponent<Text>();
            signbtn = transform.FindChild("panel_down/signbtn/sign").GetComponent<Button>();
            busign_bg = transform.FindChild("busign_bg").gameObject;

            risen_reward = transform.FindChild("panel_top/contain").gameObject;
            risen_rewards = new GameObject[5];
            for (int i = 0; i < risen_reward.transform.childCount; i++)
            {
                risen_rewards[i] = risen_reward.transform.GetChild(i).gameObject;
                risen_rewards[i].name = (i + 1).ToString();
                EventTriggerListener.Get(risen_rewards[i]).onClick = (GameObject go) => { onenter(go, go.GetComponent<RectTransform>().position); };
            }
            new BaseButton(transform.FindChild("cumulative_reward/touch")).onClick = onexit;
            lst = new List<int>();
            List<SXML> addup = XMLMgr.instance.GetSXMLList("signup_a3.total");
            foreach (SXML x in addup)
            {
                lst.Add(x.getInt("total"));
            }
            thismonth = transform.FindChild("bg/month").GetComponent<Text>();
            addsigns = transform.FindChild("bg/Text").GetComponent<Text>();
            //addsigns.text = string.Format("<color=#B3EE3A>{0}</color> 天", 0);
            addsigns.text = ContMgr.getCont("a3_sign_txt2", new List<string>() { 0.ToString()});
            BaseButton bt_close = new BaseButton(transform.FindChild("btn_close"));
            bt_close.onClick = onClose;
            new BaseButton(getTransformByPath("panel_down/signbtn/allrepairsign")).onClick = onAllrepairsign;

            BaseButton bt_sign = new BaseButton(transform.FindChild("panel_down/signbtn/sign"));
            bt_sign.onClick = onsign;
            BaseButton bu_sign = new BaseButton(transform.FindChild("busign_bg/Image/yes"));
            bu_sign.onClick = onbusign;
            BaseButton bu_sign1 = new BaseButton(transform.FindChild("busign_bg/Image/no"));
            bu_sign1.onClick = onbusigns;
            new BaseButton(getTransformByPath("panel_down/buy_cardbtn")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_SIGN);
                  returnthis = true;
                  InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                  
              };
            new BaseButton(getTransformByPath("desc/touch")).onClick = (GameObject go) =>
              {
                  getGameObjectByPath("desc").SetActive(false);
              };

            new BaseButton(getTransformByPath("top/jingbi/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                if (a3_exchange.Instance != null)
                    a3_exchange.Instance.transform.SetAsLastSibling();
            };
            new BaseButton(getTransformByPath("top/zuanshi/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SIGN);
                returnthis = true;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            };
            new BaseButton(getTransformByPath("top/bangzuan/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HONOR);
                if (a3_honor.instan != null)
                    a3_honor.instan.transform.SetAsLastSibling();
            };
            new BaseButton(transform.FindChild("panel_top/help")).onClick = (GameObject go) => {
                transform.FindChild("rewInfo").gameObject.SetActive(true); 
            };
            new BaseButton(transform.FindChild("rewInfo/close")).onClick = (GameObject go) => {
                transform.FindChild("rewInfo").gameObject.SetActive(false); 
            };

            new BaseButton(transform.FindChild("panel_top/toget")).onClick = (GameObject go) => {
                A3_signProxy.getInstance().sendproxy(5, 0,(uint)idx);
            };
            setRew_info();

        }
        public override void onShowed()
        {
            
            thismonth.text = month + ContMgr.getCont("yue");
            A3_signProxy.getInstance().sendproxy(1, 0);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.SIGNINFO, refreshSign);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.AllREPARISIGN, allrepairsign);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.SIGNorREPAIR, singorrepair);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.ACCUMULATE, accumulate);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.TOTAL_SIGNUP, Toget_res);
            A3_signProxy.getInstance().addEventListener(A3_signProxy.SIGNINFO_YUEKA, timeovers);
            GRMap.GAME_CAMERA.SetActive(false);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            refreshMoney();
            refreshGold();
            refreshGift();
            createSign();
            transform.FindChild("rewInfo").gameObject.SetActive(false);
        }
        public override void onClosed()
        {
            addsigns_num = 0;
            thisdayissign = false;
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            SignProxy.getInstance().removeEventListener(A3_signProxy.SIGNINFO, refreshSign);
            A3_signProxy.getInstance().removeEventListener(A3_signProxy.AllREPARISIGN, allrepairsign);
            A3_signProxy.getInstance().removeEventListener(A3_signProxy.SIGNorREPAIR, singorrepair);
            A3_signProxy.getInstance().removeEventListener(A3_signProxy.TOTAL_SIGNUP, Toget_res);
            A3_signProxy.getInstance().removeEventListener(A3_signProxy.ACCUMULATE, accumulate);
            A3_signProxy.getInstance().removeEventListener(A3_signProxy.SIGNINFO_YUEKA, timeovers);
            GRMap.GAME_CAMERA.SetActive(true);
            A3_signProxy.getInstance().sendproxy(1, 0);
        }
        int years;
        int months;
        int days;


        public void refreshMoney()
        {
            Text money = transform.FindChild("top/jingbi/image/num").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("top/zuanshi/image/num").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("top/bangzuan/image/num").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
        }
        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshMoney();
            }
            if (info.ContainsKey("yb"))
            {
                refreshGold();
            }
            if (info.ContainsKey("bndyb"))
            {
                refreshGift();
            }
        }
        public  string ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string st = dtStart.AddSeconds(timeStamp).ToString("yyyy-MM-dd");     
            return st;
            // long lTime = long.Parse(timeStamp + "0000");
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);
           
        }

       
        void timeovers(GameEvent e)
        {
            timeoverss(e.data["yueka"]);
            if(e.data["yueka"] == 0)
                setNull_yueka(e.data);
        }

        bool have_yueka = false;
        void  timeoverss(int num)
        {

            switch(num)
            {
                case 0://没卡
                    timeover.text = ContMgr.getCont("a3_sign_txt");
                    have_yueka = false;
                    break;
                case 1://有卡
                    timeover.text = timeover_str;//月卡到期时间
                    have_yueka = true;
                    break;                   
                case 2:
                    timeover.text = ContMgr.getCont("a3_sign_txt1");
                    have_yueka = true;
                    break;

            }
        }

        void setNull_yueka(Variant v)
        {
            if (v.ContainsKey("qd_days"))
            {
                addsigns_num = v["qd_days"].Length;
                addsigns.text = ContMgr.getCont("a3_sign_txt2", new List<string>() { addsigns_num.ToString() });
            }
            if (v.ContainsKey ("count_type"))
            {
                if (v["count_type"].Length > 0)
                {
                    int idx = 0;
                    for (int i = 0; i < v["count_type"].Length; i++)
                    {
                        if (idx < v["count_type"][i])
                            idx = v["count_type"][i];
                    }
                    totalal_idx = idx;
                }
                else
                {
                    totalal_idx = 0;
                }
                set_tolal_rew();
            }

            if (v.ContainsKey ("qd_days"))
            {
                int count = v["qd_days"].Length;
                for (int i = 0; i < count; i++)
                {
                    last_day.Add(v["qd_days"]._arr[i]);//签过的日期
                }
                if (v["qd_days"].Length > 0)
                {
                    dic_obj[thisday + weekinone - 1].transform.FindChild("thisday").gameObject.SetActive(true);

                    foreach (int j in v["qd_days"]._arr)
                    {
                        dic_obj[j + weekinone - 1].transform.FindChild("over").gameObject.SetActive(true);
                        dic_obj[j + weekinone - 1].transform.FindChild(j + weekinone - 1 + "").gameObject.SetActive(false);//补
                        if (j == thisday)
                        {
                            thisdayissign = true;
                            repairsigncount = thisday - count;
                            dic_obj[thisday + weekinone - 1].transform.FindChild("button").gameObject.SetActive(false);

                        }
                        if (list.Contains(j + weekinone - 1))
                        {
                            list.Remove(j + weekinone - 1);
                        }

                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == thisday + weekinone - 1)
                        {
                            continue;
                        }
                        dic_obj[list[i]].transform.FindChild("over").gameObject.SetActive(false);
                        dic_obj[list[i]].transform.FindChild(list[i] + "").gameObject.SetActive(true);

                    }
                    if (repairsigncount == 0)
                    {
                        canbuqian = false;
                        repairsignglod.text = 0 + "";
                        yesallglod.text = 0 + "";
                    }
                    else
                    {
                        canbuqian = true;
                        repairsignglod.text = (repairsigncount * 5).ToString();
                        yesallglod.text = (repairsigncount * 5).ToString();
                    }
                }
                else
                {
                    //signbtn.gameObject.SetActive(true);
                    //if (v.ContainsKey("yueka_tm"))
                    //{
                    //    if (days == thisday)
                    //    {
                    //        //signbtn.interactable = true;
                    //        signbtn.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("qiandao");
                    //    }
                    //}

                    foreach (int i in dic_obj.Keys)
                    {
                        dic_obj[i].transform.FindChild("over").gameObject.SetActive(false);
                    }
                    //allrepairsignbtn.interactable = true;
                    canbuqian = true;
                    repairsignglod.text = (repairsigncount * 5).ToString();
                    yesallglod.text = (repairsigncount * 5).ToString();
                    foreach (int i in dic_obj.Keys)
                    {
                        if (i < thisday + weekinone - 1)
                            dic_obj[i].transform.FindChild(i + "").gameObject.SetActive(true);
                        else if (i == thisday + weekinone - 1)
                            dic_obj[i].transform.FindChild("thisday").gameObject.SetActive(true);
                    }
                }
            }
        }

        string timeover_str = "";
        List<int> last_day = new List<int>();
        void refreshSign(GameEvent e)
        {

            if (e.data.ContainsKey("yueka")) {
                if (e.data["yueka"] == 0)
                    return;
            }
            int count = e.data["qd_days"].Length;
            for (int i = 0; i < count; i++)
            {
                last_day.Add(e.data["qd_days"]._arr[i]);//签过的日期
            }
            if (buy_cardbtn != null)
            {
                buy_cardbtn.SetActive(false);
            }
            signbtn_panel.SetActive(true);
            transform.FindChild("panel_top/toget").gameObject.SetActive(true);
            repairsigncount = thisday - count - 1;
            timeover.text=ConvertStringToDateTime(e.data["yueka_tm"]);
            timeover_str = timeover.text;
            timeoverss(e.data["yueka"]._int);           
            addsigns_num = e.data["qd_days"].Length;
            addsigns.text = ContMgr.getCont("a3_sign_txt2", new List<string>() { addsigns_num.ToString() });
            dic_obj[thisday + weekinone - 1].transform.FindChild("button").gameObject.SetActive(true);
            BaseButton btn = new BaseButton(dic_obj[thisday + weekinone - 1].transform.FindChild("button"));
            btn.onClick = (GameObject go)=> {
                if (go.transform.parent.FindChild(go.transform.parent.name).gameObject.activeSelf == false
                && go.transform.parent.FindChild("over").gameObject.activeSelf == false)
                    A3_signProxy.getInstance().sendproxy(2, 0);
                else {
                    setTip(int.Parse (go.transform.parent.FindChild("nub").GetChild (0).gameObject.name));
                }
            };
            if (e.data["qd_days"].Length > 0)
            {
                dic_obj[thisday + weekinone - 1].transform.FindChild("thisday").gameObject.SetActive(true);
                //signbtn.interactable = true;
                signbtn.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("qiandao");

                foreach (int j in e.data["qd_days"]._arr)
                {
                    dic_obj[j + weekinone - 1].transform.FindChild("over").gameObject.SetActive(true);
                    dic_obj[j + weekinone - 1].transform.FindChild(j + weekinone - 1 + "").gameObject.SetActive(false);//补
                    if (j == thisday)
                    {
                        thisdayissign = true;
                        //signbtn.interactable = false;
                        signbtn.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("yiqiandao");
                        repairsigncount = thisday - count;
                        dic_obj[thisday + weekinone - 1].transform.FindChild("button").gameObject.SetActive(false);

                    }
                    if (list.Contains(j + weekinone - 1))
                    {
                        list.Remove(j + weekinone - 1);
                    }

                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == thisday + weekinone - 1)
                    {
                        continue;
                    }
                    dic_obj[list[i]].transform.FindChild("over").gameObject.SetActive(false);
                    dic_obj[list[i]].transform.FindChild(list[i] + "").gameObject.SetActive(true);

                }
                if (repairsigncount == 0)
                {
                    //allrepairsignbtn.interactable = false;
                    canbuqian = false;
                    repairsignglod.text = 0 + "";
                    yesallglod.text = 0 + "";
                }
                else
                {
                    //allrepairsignbtn.interactable = true;
                    canbuqian = true;
                    repairsignglod.text = (repairsigncount * 5).ToString();
                    yesallglod.text = (repairsigncount * 5).ToString();
                }
            }
            else
            {
                signbtn.gameObject.SetActive(true);
                if (e.data.ContainsKey("yueka_tm"))
                {
                    if (days == thisday)
                    {
                        //signbtn.interactable = true;
                        signbtn.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("qiandao");
                    }
                }

                foreach (int i in dic_obj.Keys)
                {
                    dic_obj[i].transform.FindChild("over").gameObject.SetActive(false);
                }
                //allrepairsignbtn.interactable = true;
                canbuqian = true;
                repairsignglod.text = (repairsigncount * 5).ToString();
                yesallglod.text = (repairsigncount * 5).ToString();
                foreach (int i in dic_obj.Keys)
                {
                    if (i < thisday + weekinone - 1)
                        dic_obj[i].transform.FindChild(i + "").gameObject.SetActive(true);
                    else if (i == thisday + weekinone - 1)
                        dic_obj[i].transform.FindChild("thisday").gameObject.SetActive(true);
                }
            }

            //累计
            //for (int i = 0; i < risen_rewards.Length; i++)
            //{
            //    risen_rewards[i].transform.FindChild("over").gameObject.SetActive(false);
            //    risen_rewards[i].transform.FindChild("can").gameObject.SetActive(false);
            //    risen_rewards[i].transform.FindChild("image").gameObject.SetActive(true);

            //    if (i < 4)
            //    {
            //        transform.FindChild("bg/line/" + i + "/" + i).gameObject.SetActive(false);
            //    }

            //}
            //if (e.data["count_type"].Length > 0)
            //{
            //    foreach (int i in e.data["count_type"]._arr)
            //    {
            //        risen_rewards[i / 5 - 1].transform.FindChild("over").gameObject.SetActive(true);
            //        risen_rewards[i / 5 - 1].transform.FindChild("image").gameObject.SetActive(false);
            //        risen_rewards[i / 5 - 1].transform.FindChild("can").gameObject.SetActive(false);

            //        if ((i / 5 - 1) < 4)
            //        {
            //            transform.FindChild("bg/line/" + (i / 5 - 1) + "/" + (i / 5 - 1)).gameObject.SetActive(true);
            //        }

            //    }
            //}
            if (e.data.ContainsKey ("count_type"))
            {
                if (e.data["count_type"].Length > 0)
                {
                    int idx = 0;
                    for (int i = 0;i< e.data["count_type"].Length;i++)
                    {
                        if(idx < e.data["count_type"][i])
                            idx = e.data["count_type"][i];
                    }
                    totalal_idx = idx;
                }
                else {
                    totalal_idx = 0;
                }

                set_tolal_rew();
                setGetBut(); 
            }
            if (!thisdayissign)
            {
                leijicanImahe(addsigns_num);
            }
        }

        void set_tolal_rew() {
            SXML x = XMLMgr.instance.GetSXML("signup_a3");
            SXML item_list = x.GetNode("total", "total==" + totalal_idx);
            GameObject item = transform.FindChild("panel_top/item").gameObject;
            Transform Con = transform.FindChild("panel_top/group");
            for (int i =0;i< Con.childCount;i++) {
                Destroy(Con.GetChild(i).gameObject);
            }
            if (item_list == null) {
                idx = 1;
            }
            else {
                int lastidx = item_list.getInt("id");
                idx = lastidx + 1;
            }
            int max_idx = x.GetNodeList("total").Count;
            if (idx > 0 ) {
                SXML _item;
                if (idx <= max_idx)
                {
                    _item = x.GetNode("total", "id==" + idx);
                }
                else {
                    _item = x.GetNode("total", "id==" + max_idx);
                }
                List<SXML> itemList = _item.GetNodeList("item");
                foreach (SXML it in itemList)
                {
                    GameObject clon = GameObject.Instantiate(item) as GameObject;
                    clon.SetActive(true);
                    clon.transform.SetParent(Con, false);
                    int id = it.getInt("item_id");
                    int num = it.getInt("num");
                    GameObject go = IconImageMgr.getInstance().createA3ItemIcon((uint)id, false, num, 1, true);
                    go.transform.SetParent(clon.transform, false);
                    new BaseButton(go.transform).onClick = (GameObject goo) => {
                        setTip(id);
                    };
                }
                transform.FindChild("bg/total").gameObject.SetActive(true);
                transform.FindChild("bg/total").GetComponent<Text>().text = _item.getInt("total").ToString();
            }
            else if (idx > max_idx) { //已领完
                //transform.FindChild("bg/total").gameObject.SetActive(false);
            }
        }

        void setGetBut() {
            SXML x = XMLMgr.instance.GetSXML("signup_a3");
            int max_idx = x.GetNodeList("total").Count;
            SXML _item;
            if (idx > max_idx) {
                _item = x.GetNode("total", "id==" + max_idx);
            } else
            {
                _item = x.GetNode("total", "id==" + idx);
            }
            int days = _item.getInt("total");
            if (idx > 0 && idx <= max_idx) {
                if (days > addsigns_num)
                {
                    //transform.FindChild("panel_top/toget").GetComponent<Button>().interactable = false;
                    transform.FindChild("panel_top/toget/Text").GetComponent <Text>().text = ContMgr.getCont("nolingqu");
                }
                else
                {
                    //transform.FindChild("panel_top/toget").GetComponent<Button>().interactable = true;
                    transform.FindChild("panel_top/toget/Text").GetComponent<Text>().text = ContMgr.getCont("lingqu");
                }
            }
            else if (idx > max_idx) {
                //transform.FindChild("panel_top/toget").GetComponent<Button>().interactable = false;
                transform.FindChild("panel_top/toget/Text").GetComponent<Text>().text = ContMgr.getCont("nolingqu");
            }
        }

        int totalal_idx = 0;
        int idx = 0;
        void setRew_info() {

            GameObject item_con = this.transform.FindChild("rewInfo/scrollview/item").gameObject;
            GameObject item_icon = this.transform.FindChild("rewInfo/scrollview/rew_item").gameObject;
            Transform Con = this.transform.FindChild("rewInfo/scrollview/con");
            for (int i = 0; i < Con.transform.childCount;i++) {
                Destroy(Con.transform.GetChild (i).gameObject);
            }
            SXML x = XMLMgr.instance.GetSXML("signup_a3");
            List<SXML> lists = x.GetNodeList("total");
            foreach (SXML xml in lists) {
                GameObject clon_item_con = GameObject.Instantiate(item_con) as GameObject;
                clon_item_con.transform.SetParent(Con,false);
                clon_item_con.SetActive(true);
                int day_num = xml.getInt("total");
                clon_item_con.transform.FindChild ("toptext").GetComponent <Text>().text = ContMgr.getCont("a3_sign_txt_top", new List<string>() { day_num.ToString() });
                List<SXML> itemList = xml.GetNodeList("item");
                foreach (SXML item_xml in itemList)
                {
                    GameObject clon_item_icon = GameObject.Instantiate(item_icon) as GameObject;
                    clon_item_icon.transform.SetParent(clon_item_con.transform.FindChild ("group"),false);
                    clon_item_icon.SetActive(true);
                    int id = item_xml.getInt("item_id");
                    int num = item_xml.getInt("num");
                    GameObject go = IconImageMgr.getInstance().createA3ItemIcon((uint)id, false, num, 1, true);
                    go.transform.SetParent(clon_item_icon.transform,false);
                }
            }
            float childSizeX = Con.GetComponent<GridLayoutGroup>().cellSize.x + Con.GetComponent<GridLayoutGroup>().spacing.x;
            Vector2 newSize = new Vector2(lists.Count * childSizeX, Con.GetComponent<RectTransform>().sizeDelta.y);
            Con.GetComponent <RectTransform>().sizeDelta = newSize;
        }

        void allrepairsign(GameEvent e)
        {

            if (e.data["fillsign_all"].Length > 0)
            {
                foreach (Variant v in e.data["fillsign_all"]._arr)
                {
                    dic_obj[v + weekinone - 1].transform.FindChild("over").gameObject.SetActive(true);
                    dic_obj[v + weekinone - 1].transform.FindChild(v + weekinone - 1 + "").gameObject.SetActive(false);
                    List<SXML> lists = XMLMgr.instance.GetSXMLList("signup_a3.signup", "signup_times==" + v._int);
                    if (lists.Count > 0)
                    {
                        int item_id = lists[0].GetNodeList("item")[0].getInt("item_id");
                        int num = lists[0].GetNodeList("item")[0].getInt("num");
                        List<SXML> listItem = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
                        string nameItem = listItem[0].getString("item_name");
                        flytxt.instance.fly(ContMgr.getCont("a3_sign_txt3") + nameItem + "*" + num);
                    }
                }

            }
            addsigns_num += e.data["fillsign_all"].Length;
            addsigns.text = ContMgr.getCont("a3_sign_txt2", new List<string>() { addsigns_num.ToString() });
            if (busign_bg.activeSelf)
                busign_bg.SetActive(false);
            canbuqian = false;
            repairsignglod.text = 0 + "";
            yesallglod.text = 0 + "";
            if (!thisdayissign)
            {
                leijicanImahe(addsigns_num);
            }
            setGetBut();
        }
        void singorrepair(GameEvent e)
        {
            dic_obj[e.data["daysign"] + weekinone - 1].transform.FindChild(e.data["daysign"] + weekinone - 1 + "").gameObject.SetActive(false);
            // dic_obj[e.data["daysign"]].transform.FindChild("thisday").gameObject.SetActive(false);
            dic_obj[e.data["daysign"] + weekinone - 1].transform.FindChild("over").gameObject.SetActive(true);
            if (e.data["daysign"] == thisday)
            {

                //signbtn.interactable = false;
                signbtn.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("yiqiandao");
                thisdayissign = true;
            }
            else
            {
                if (list.Contains(e.data["daysign"] + weekinone - 1))
                {
                    list.Remove(e.data["daysign"] + weekinone - 1);
                }
                repairsigncount -= 1;
                repairsignglod.text = (repairsigncount * 5).ToString();
                yesallglod.text = (repairsigncount * 5).ToString();
            }
            if (repairsigncount == 0)
            {
                //allrepairsignbtn.interactable = false;
                canbuqian = false;
                repairsignglod.text = 0 + "";
                yesallglod.text = 0 + "";
            }
            if (busign_bg.activeSelf)
            {
                busign_bg.SetActive(false);
            }
            day = -1;
            addsigns_num += 1;
            addsigns.text =ContMgr.getCont("a3_sign_txt2", new List<string>() { addsigns_num.ToString() });
            if (!thisdayissign)
            {
                leijicanImahe(addsigns_num);
            }
            List<SXML> lists = XMLMgr.instance.GetSXMLList("signup_a3.signup", "signup_times==" + e.data["daysign"]);
            if (lists.Count > 0)
            {
                int item_id = lists[0].GetNodeList("item")[0].getInt("item_id");
                int num = lists[0].GetNodeList("item")[0].getInt("num");
                List<SXML> listItem = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
                string nameItem = listItem[0].getString("item_name");
                flytxt.instance.fly(ContMgr.getCont("a3_sign_txt3") + nameItem + "*" + num);
            }
            setGetBut();
        }

        void Toget_res(GameEvent e) {
            if (e.data.ContainsKey("total_signup"))
            {
                if (e.data["total_signup"].Length > 0)
                {
                    totalal_idx = e.data["total_signup"][e.data["total_signup"].Length - 1];
                }
                else
                {
                    totalal_idx = 0;
                }
                fly_text(totalal_idx);
                set_tolal_rew();
                setGetBut();
            }
        }

        void fly_text(int ids)
        {
            SXML s = XMLMgr.instance.GetSXML("signup_a3.total", "total==" + ids);
            if (s != null) {
                List<SXML> l = s.GetNodeList("item");
                foreach (SXML xml in l)
                {
                    int item = xml.getInt("item_id");
                    int num = xml.getInt("num");
                    SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + item);
                    string name = item_xml.getString("item_name");
                    flytxt.instance.fly(ContMgr.getCont("huodeitem", new List<string>() { name , num.ToString () }));
                }
            }
        }
        void accumulate(GameEvent e)
        {
            if (e.data["total_signup"].Length > 0)
            {
                foreach (int i in e.data["total_signup"]._arr)
                {
                    risen_rewards[i / 5 - 1].transform.FindChild("over").gameObject.SetActive(true);
                    risen_rewards[i / 5 - 1].transform.FindChild("image").gameObject.SetActive(false);
                    risen_rewards[i / 5 - 1].transform.FindChild("can").gameObject.SetActive(false);

                    if ((i / 5 - 1) < 4)
                    {
                        transform.FindChild("bg/line/" + (i / 5 - 1) + "/" + (i / 5 - 1)).gameObject.SetActive(true);
                    }
                    List<SXML> lists = XMLMgr.instance.GetSXMLList("signup_a3.total", "total==" + i);
                    for (int j = 0; j < lists[0].GetNodeList("item").Count; j++)
                    {
                        int item_id = lists[0].GetNodeList("item")[j].getInt("item_id");
                        int num = lists[0].GetNodeList("item")[j].getInt("num");
                        List<SXML> listItem = XMLMgr.instance.GetSXMLList("item.item", "id==" + item_id);
                        string nameItem = listItem[0].getString("item_name");
                        flytxt.instance.fly(ContMgr.getCont("a3_sign_txt4") + nameItem + "*" + num);
                    }
                }
            }
        }

        void leijicanImahe(int nums)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] == (nums + 1))
                {
                    risen_rewards[i].transform.FindChild("can").gameObject.SetActive(true);
                    risen_rewards[i].transform.FindChild("over").gameObject.SetActive(false);
                    risen_rewards[i].transform.FindChild("image").gameObject.SetActive(false);
                    BaseButton btn = new BaseButton(risen_rewards[i].transform.FindChild("can").transform);
                    btn.onClick = onsign;

                    if (i < 4)
                    {
                        transform.FindChild("bg/line/" + i + "/" + i).gameObject.SetActive(false);
                    }

                    break;
                }
            }

        }

        int day = -1;
        void createSign()
        {
            onexit(null);
            buy_cardbtn.SetActive(true);
            signbtn_panel.SetActive(false);
            transform.FindChild("bg/total").gameObject.SetActive(false);
            transform.FindChild("panel_top/toget").gameObject.SetActive(false); 
            thismonthcout = DateTime.DaysInMonth(year, month);
            int week = (thisday + 2 * month + 3 * (month + 1) / 5 + year + year / 4 - year / 100 + year / 400) % 7 + 1;//星期几
            if (transform.FindChild("panel_down/thisday_reward/group").transform.childCount > 0)
            {
                for (int j = 0; j < thisday_panel.transform.childCount; j++)
                {
                    Destroy(thisday_panel.transform.GetChild(j).gameObject);
                }
            }
            
            showreward(thisday, thisday_panel);

            for (int i = 0; i < risen_rewards.Length; i++) 
            {
                risen_rewards[i].transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_sign_txt5") + "<color=#f5ff55>" + lst[i] + "</color>" + ContMgr.getCont("a3_sign_txt6");
            }
            //1号星期几
            int dayinone = 2;
            DateTime date = new DateTime();
            date = DateTime.Parse(year + "/" + month + "/" + 1);
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dayinone = 7;
                    break;
                case DayOfWeek.Monday:
                    dayinone = 1;
                    break;
                case DayOfWeek.Tuesday:
                    dayinone = 2;
                    break;
                case DayOfWeek.Wednesday:
                    dayinone = 3;
                    break;
                case DayOfWeek.Thursday:
                    dayinone = 4;
                    break;
                case DayOfWeek.Friday:
                    dayinone = 5;
                    break;
                case DayOfWeek.Saturday:
                    dayinone = 6;
                    break;
                default:
                    break;
            }
            weekinone = dayinone;

            //上个月
            int lastday;
            if (month - 1 == 0)
            {
                lastday = DateTime.DaysInMonth(year - 1, 12);
            }
            else
            {
                lastday = DateTime.DaysInMonth(year, month - 1);
            }
            for (int i = dayinone; i <= (thismonthcout + dayinone - 1); i++)
            {
                int num = i;
                GameObject objClone = null;
                if (robj.ContainsKey(i)) { objClone = robj[i]; }
                else
                {
                    objClone = GameObject.Instantiate(image_day) as GameObject;
                }
                objClone.SetActive(true);
                objClone.transform.SetParent(contain.transform, false);
                objClone.name = i + "";
                var bu = objClone.transform.FindChild("bu");
                if (bu != null) bu.name = i.ToString();

                if (i < dayinone)
                {
                    for (int i_1 = 0; i_1 < objClone.transform.FindChild("old_nub").childCount;i_1++) {
                        Destroy(objClone.transform.FindChild("old_nub").GetChild(i_1).gameObject);
                    }
                    objClone.transform.FindChild("nub").gameObject.SetActive(false);
                    objClone.transform.FindChild("old_nub").gameObject.SetActive(true);
                    objClone.transform.FindChild("old_nub").GetComponent<Text>().text = (lastday - dayinone + i + 1).ToString();
                    GameObject go = getItemCon(lastday - dayinone + i + 1);
                    go.transform.SetParent(objClone.transform.FindChild("old_nub"),false);
                }
                if (i >= dayinone && i <= thismonthcout + dayinone - 1)
                {
                    for (int i_1 = 0; i_1 < objClone.transform.FindChild("nub").childCount; i_1++)
                    {
                        Destroy(objClone.transform.FindChild("nub").GetChild(i_1).gameObject);
                    }
                    objClone.transform.FindChild("nub").gameObject.SetActive(true);
                    objClone.transform.FindChild("old_nub").gameObject.SetActive(false);
                    objClone.transform.FindChild("nub").GetComponent<Text>().text = (i - dayinone + 1).ToString();
                    GameObject go = getItemCon(i - dayinone + 1);
                    go.transform.SetParent(objClone.transform.FindChild("nub"), false);
                }
                if (i > thismonthcout + dayinone - 1)
                {
                    for (int i_1 = 0; i_1 < objClone.transform.FindChild("old_nub").childCount; i_1++)
                    {
                        Destroy(objClone.transform.FindChild("old_nub").GetChild(i_1).gameObject);
                    }
                    objClone.transform.FindChild("nub").gameObject.SetActive(false);
                    objClone.transform.FindChild("old_nub").gameObject.SetActive(true);
                    objClone.transform.FindChild("old_nub").GetComponent<Text>().text = (i - (thismonthcout + dayinone - 1)).ToString();
                    GameObject go = getItemCon(i - (thismonthcout + dayinone - 1));
                    go.transform.SetParent(objClone.transform.FindChild("old_nub"), false);
                }

                if (i < thisday + weekinone - 1)
                    objClone.transform.FindChild("old").gameObject.SetActive(true);
                if (i == thisday + weekinone - 1)
                    objClone.transform.FindChild("thisday").gameObject.SetActive(true);
                if (i > thisday + weekinone - 1)
                    objClone.transform.FindChild("new").gameObject.SetActive(true);
                
                BaseButton btn_bu = new BaseButton(objClone.transform.FindChild(i + ""));
                btn_bu.onClick = delegate (GameObject go)
                {
                    if (have_yueka == true)
                    {
                        GameObject obj = busign_bg.transform.FindChild("Image/hscr/scroll/content").gameObject;
                        GridLayoutGroup gglg= obj.GetComponent<GridLayoutGroup>();
                        gglg.childAlignment = TextAnchor.MiddleCenter;
                       // print("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz:" + obj.transform.localPosition.x);
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2((gglg.cellSize.x + 0.1f) * 6, 102);                                        
                        day = num - weekinone + 1;//补签的几号
                        deldereward();
                        showreward(day, busign_bg.transform.FindChild("Image/hscr/scroll/content").gameObject);
                        yesallglod.text = 5 + "";
                        busign_bg.transform.FindChild("left").gameObject.SetActive(false);
                        busign_bg.transform.FindChild("right").gameObject.SetActive(false);
                        busign_bg.SetActive(true);

                        obj.transform.localPosition = new Vector3(-298f, obj.transform.localPosition.y, obj.transform.localPosition.z);

                    }
                    else {
                        flytxt.instance.fly(ContMgr.getCont("toget_yueka"));
                    }
                };
                robj[i] = objClone;
                if (i >= dayinone && i <= thismonthcout + dayinone - 1)
                    dic_obj[i] = objClone;
                if (i >= dayinone && i < thisday + dayinone - 1)
                {
                    list.Add(i);
                }
            }
        }

        GameObject getItemCon(int days)
        {
            SXML xml = XMLMgr.instance.GetSXML("signup_a3.signup", "signup_times==" + days);
            SXML item_xml = xml.GetNode("item");
            int itemid = item_xml.getInt("item_id");
            int num = item_xml.getInt("num");
            GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)itemid, false, num, 1, true);
            item.name = itemid.ToString();
            new BaseButton(item.transform).onClick = (GameObject go) => { 
            setTip(itemid);
        };
            return item;
        }
        //显示补签或者当天的奖励
        int itemid;
        void showreward(int days, GameObject obj)
        {
            SXML xmlitem;
            string limit;
            Transform iconbg = transform.FindChild("cumulative_reward/iconbg");
            SXML xml = XMLMgr.instance.GetSXML("signup_a3.signup", "signup_times==" + days);
            int moneytype = xml.getInt("gemnum");
            // obj.transform.FindChild("Image/num").GetComponent<Text>().text = moneytype.ToString();
            List<SXML> xmls = xml.GetNodeList("item");
            foreach (SXML x in xmls)
            {
                GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)x.getInt("item_id"), false, x.getInt("num"), 1, true);
                if (x.getInt("num") <= 1)
                {
                    item.transform.FindChild("num").gameObject.SetActive(false);
                }
                itemid = x.getInt("item_id");
                item.transform.FindChild("iconborder").localScale = new Vector3(1, 1, 0);
                var imt = item.GetComponent<Image>();
                if (imt != null) imt.enabled = false;
                item.transform.SetParent(obj.transform, false);
                var gg = GameObject.Instantiate(iconbg.gameObject) as GameObject;
                gg.transform.SetParent(item.transform);
                gg.transform.localPosition = Vector3.zero;
                gg.transform.localScale = Vector3.one;
                gg.SetActive(true);
                gg.transform.SetAsFirstSibling();
                new BaseButton(item.transform).onClick = (GameObject go) =>
                  {
                      xmlitem = XMLMgr.instance.GetSXML("item.item", "id==" + itemid);
                      getTransformByPath("desc/bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid) + ContMgr.getCont("ge");
                      getTransformByPath("desc/bg/itemdesc").GetComponent<Text>().text = StringUtils.formatText(xmlitem.getString("desc"));
                      getTransformByPath("desc/bg/name/name").GetComponent<Text>().text = xmlitem.getString("item_name");
                      getTransformByPath("desc/bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + itemid);
                      if (xmlitem.getInt("use_limit") > 0)
                      {
                          limit = xmlitem.getInt("use_limit") + ContMgr.getCont("zhuan");
                      }
                      else
                          limit = ContMgr.getCont("a3_active_wuxianzhi");
                      getTransformByPath("desc/bg/name/dengji").GetComponent<Text>().text = limit;
                      getGameObjectByPath("desc").SetActive(true);
                  };
            }
            var gglg = obj.GetComponent<GridLayoutGroup>();
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2((gglg.cellSize.x + 0.1f) * Mathf.Max(xmls.Count, 6), 102);
            if (xmls.Count > 6) gglg.childAlignment = TextAnchor.MiddleLeft;
            else gglg.childAlignment = TextAnchor.MiddleCenter;
        }
        //清除数据
        void deldereward()
        {
            if (busign_bg.transform.FindChild("Image/hscr/scroll/content").transform.childCount > 0)
            {
                int num = busign_bg.transform.FindChild("Image/hscr/scroll/content").transform.childCount;
                for (int j = num; j > 0; j--)
                {
                    DestroyImmediate(busign_bg.transform.FindChild("Image/hscr/scroll/content").transform.GetChild(j - 1).gameObject);
                }
            }
        }

        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SIGN);
        }
        void onAllrepairsign(GameObject go)
        {
            if (!canbuqian) {
                flytxt.instance.fly(ContMgr.getCont("null_repairsign"));
                return;
            }
            yesallglod.text = (repairsigncount * 5).ToString();
            deldereward();
            int num = 0;
            Dictionary<int, int> dicAllrepairsign = new Dictionary<int, int>();
            for (int i = 0; i < list.Count; i++)
            {
                SXML xml = XMLMgr.instance.GetSXML("signup_a3.signup", "signup_times==" + (list[i]/*-2*/));
                num += xml.getInt("gemnum");
                List<SXML> xmls = xml.GetNodeList("item");
                foreach (SXML x in xmls)
                {
                    if (dicAllrepairsign.ContainsKey(x.getInt("item_id")))
                        dicAllrepairsign[x.getInt("item_id")] = x.getInt("num");
                    else
                        dicAllrepairsign[x.getInt("item_id")] = x.getInt("num");
                }
            }
            busign_bg.transform.FindChild("Image/panel/Image/num").GetComponent<Text>().text = num.ToString();
            Transform iconbg = transform.FindChild("cumulative_reward/iconbg");
            foreach (int i in dicAllrepairsign.Keys)
            {
                GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)i, false, dicAllrepairsign[i], 1.0f, true);
                if (dicAllrepairsign[i] <= 1)
                {
                    item.transform.FindChild("num").gameObject.SetActive(false);
                }
                item.transform.FindChild("iconborder").localScale = new Vector3(1, 1, 0);
                var imt = item.GetComponent<Image>();
                if (imt != null) imt.enabled = false;
                item.transform.SetParent(busign_bg.transform.FindChild("Image/hscr/scroll/content").transform, false);
                var gg = GameObject.Instantiate(iconbg.gameObject) as GameObject;
                gg.transform.SetParent(item.transform);
                gg.transform.localPosition = Vector3.zero;
                gg.transform.localScale = Vector3.one;
                gg.SetActive(true);
                gg.transform.SetAsFirstSibling();
            }

            GameObject obj = busign_bg.transform.FindChild("Image/hscr/scroll/content").gameObject;
            if (list.Count <= 5)
            {
                busign_bg.transform.FindChild("left").gameObject.SetActive(false);
                busign_bg.transform.FindChild("right").gameObject.SetActive(false);
            }
            else
            {
                busign_bg.transform.FindChild("left").gameObject.SetActive(true);
                busign_bg.transform.FindChild("right").gameObject.SetActive(true);
            }
            var gglg = obj.GetComponent<GridLayoutGroup>();
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2((gglg.cellSize.x + 0.1f) * Mathf.Max(dicAllrepairsign.Count, 6), 102);
            if (dicAllrepairsign.Count > 6) gglg.childAlignment = TextAnchor.MiddleLeft;
            else gglg.childAlignment = TextAnchor.MiddleCenter;
            busign_bg.SetActive(true);
        }
        void onsign(GameObject go)
        {
            A3_signProxy.getInstance().sendproxy(2, 0);
        }

        void onbusign(GameObject go)
        {
            if (day == -1)
                A3_signProxy.getInstance().sendproxy(4, 0);
            else
                A3_signProxy.getInstance().sendproxy(3, day);
        }
        void onbusigns(GameObject go)
        {
            busign_bg.SetActive(false);       
        }
        void onenter(GameObject go, Vector3 pos)
        {
            libg.gameObject.SetActive(true);
            libg.SetParent(go.transform);
            libg.localPosition = new Vector3(-4.5f, 4f, 0);
            libg.SetAsFirstSibling();

            Transform crr = transform.FindChild("cumulative_reward/bg/cumulative_reward");
            Transform iconbg = transform.FindChild("cumulative_reward/iconbg");
            GridLayoutGroup glg = crr.GetComponent<GridLayoutGroup>();
            var recbg = getComponentByPath<RectTransform>("cumulative_reward/bg");
            if (crr.transform.childCount > 0)
            {
                for (int i = 0; i < crr.transform.childCount; i++)
                {
                    Destroy(crr.transform.GetChild(i).gameObject);
                }
            }
            SXML xml = XMLMgr.instance.GetSXML("signup_a3.total", "total==" + int.Parse(go.name) * 5);
            SXML xmlitem;
            string limit;
            List<SXML> sxml = xml.GetNodeList("item");
            int itemid2;

            foreach (SXML x in sxml)
            {
                GameObject item = IconImageMgr.getInstance().createA3ItemIcon((uint)x.getInt("item_id"), false, x.getInt("num"), 1f, true);
                if (x.getInt("num") <= 1)
                {
                    item.transform.FindChild("num").gameObject.SetActive(false);
                }
                item.transform.FindChild("iconborder").localScale = new Vector3(1f, 1, 0);
                var imt = item.GetComponent<Image>();
                if (imt != null) imt.enabled = false;
                item.transform.SetParent(crr.transform, false);
                var gg = GameObject.Instantiate(iconbg.gameObject) as GameObject;
                gg.transform.SetParent(item.transform);
                gg.transform.localPosition = Vector3.zero;
                gg.transform.localScale = Vector3.one;
                gg.SetActive(true);
                gg.transform.SetAsFirstSibling();
                uint id = (uint)x.getInt("item_id");
                new BaseButton(item.transform).onClick = (GameObject gos) =>
                {
                    itemid2 = (int)id;

                    xmlitem = XMLMgr.instance.GetSXML("item.item", "id==" + itemid2);
                    getTransformByPath("desc/bg/itemdesc").GetComponent<Text>().text = xmlitem.getString("desc");
                    getTransformByPath("desc/bg/name/name").GetComponent<Text>().text = xmlitem.getString("item_name");
                    getTransformByPath("desc/bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
                    getTransformByPath("desc/bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + itemid2);
                    if (xmlitem.getInt("use_limit") > 0)
                    {
                        limit = xmlitem.getInt("use_limit") + ContMgr.getCont("zhuan");
                    }
                    else
                        limit = ContMgr.getCont("a3_active_wuxianzhi");
                    getTransformByPath("desc/bg/name/dengji").GetComponent<Text>().text = limit;
                    getGameObjectByPath("desc").SetActive(true);
                };
            }
            transform.FindChild("cumulative_reward").gameObject.SetActive(true);

            recbg.sizeDelta = new Vector2(recbg.sizeDelta.x, (glg.cellSize.y + glg.spacing.y + 10) * (Mathf.Max(1, sxml.Count - 1) / glg.constraintCount + 1) + 20);

            Vector3 vec = pos;
            if (vec.y > Screen.height / 2)
            {
                pos.y -= glg.cellSize.y * (Mathf.Max(1, sxml.Count - 1) / glg.constraintCount) * Baselayer.uiRatio;
            }
            else
            {
                pos.y += glg.cellSize.y * (Mathf.Max(1, sxml.Count - 1) / glg.constraintCount) * Baselayer.uiRatio;
            }
            if (vec.x > Screen.width / 2)
                pos.x -= 230 * Baselayer.uiRatio;
            else
                pos.x += 230 * Baselayer.uiRatio;
            transform.FindChild("cumulative_reward").transform.position = (pos + new Vector3(30, 0, 0));

        }

        void setTip(int itemid) {
            SXML xmlitem = XMLMgr.instance.GetSXML("item.item", "id==" + itemid);
            getTransformByPath("desc/bg/itemdesc").GetComponent<Text>().text = StringUtils.formatText(xmlitem.getString("desc"));
            getTransformByPath("desc/bg/name/name").GetComponent<Text>().text = xmlitem.getString("item_name");
            getTransformByPath("desc/bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid) + ContMgr.getCont("ge");
            getTransformByPath("desc/bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + itemid);
            string limit;
            if (xmlitem.getInt("use_limit") > 0)
            {
                limit = xmlitem.getInt("use_limit") + ContMgr.getCont("zhuan");
            }
            else
                limit = ContMgr.getCont("a3_active_wuxianzhi");
            getTransformByPath("desc/bg/name/dengji").GetComponent<Text>().text = limit;
            getGameObjectByPath("desc").SetActive(true);
        }
        void onexit(GameObject go)
        {
            transform.FindChild("cumulative_reward").gameObject.SetActive(false);
            var libgp = transform.FindChild("panel_top");
            libg.SetParent(libgp);
            libg.gameObject.SetActive(false);
        }
    }
}
