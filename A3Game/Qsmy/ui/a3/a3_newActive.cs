using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using Cross;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace MuGame
{
    class a3_newActive : Window
    {
        public static a3_newActive instans;
        Transform tab;
        Transform contents;
        RectTransform leftPos;
        RectTransform rightPos;
        RectTransform nowPos;
        List<int> setted = new List<int>();
        SXML xml;

        Dictionary<int, List<rewInfo>> allrew = new Dictionary<int, List<rewInfo>>();

        public override void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("contents/1/Text").text = ContMgr.getCont("a3_newActive_0");
            getComponentByPath<Text>("contents/1/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/2/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/3/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/4/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/5/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/6/rews/item_list/scroll/0/buttn/Text").text = ContMgr.getCont("nolingqu");
            getComponentByPath<Text>("contents/1/info").text = ContMgr.getCont("a3_newActive_1");
            getComponentByPath<Text>("contents/1/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/1/Text1").text = ContMgr.getCont("a3_newActive_3");
            getComponentByPath<Text>("contents/2/Text").text = ContMgr.getCont("a3_newActive_4");
            getComponentByPath<Text>("contents/2/info").text = ContMgr.getCont("a3_newActive_5");
            getComponentByPath<Text>("contents/2/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/2/Text1").text = ContMgr.getCont("a3_newActive_3");
            getComponentByPath<Text>("contents/3/Text").text = ContMgr.getCont("a3_newActive_6");
            getComponentByPath<Text>("contents/3/info").text = ContMgr.getCont("a3_newActive_7");
            getComponentByPath<Text>("contents/3/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/3/Text1").text = ContMgr.getCont("a3_newActive_3");
            getComponentByPath<Text>("contents/4/Text").text = ContMgr.getCont("a3_newActive_8");
            getComponentByPath<Text>("contents/4/info").text = ContMgr.getCont("a3_newActive_9");
            getComponentByPath<Text>("contents/4/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/4/Text1").text = ContMgr.getCont("a3_newActive_3");
            getComponentByPath<Text>("contents/5/Text").text = ContMgr.getCont("a3_newActive_10");
            getComponentByPath<Text>("contents/5/info").text = ContMgr.getCont("a3_newActive_11");
            getComponentByPath<Text>("contents/5/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/5/Text1").text = ContMgr.getCont("a3_newActive_3");
            getComponentByPath<Text>("contents/6/Text").text = ContMgr.getCont("a3_newActive_12");
            getComponentByPath<Text>("contents/5/info").text = ContMgr.getCont("a3_newActive_13");
            getComponentByPath<Text>("contents/6/Text2").text = ContMgr.getCont("a3_newActive_2");
            getComponentByPath<Text>("contents/6/Text1").text = ContMgr.getCont("a3_newActive_3");

            getComponentByPath<Text>("contents/1/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");
            getComponentByPath<Text>("contents/2/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");
            getComponentByPath<Text>("contents/3/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");
            getComponentByPath<Text>("contents/4/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");
            getComponentByPath<Text>("contents/5/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");
            getComponentByPath<Text>("contents/6/rank_btn/Text").text = ContMgr.getCont("a3_newActive_14");

            getComponentByPath<Text>("rank/top/1/Text").text = ContMgr.getCont("a3_newActive_15"); //排名
            getComponentByPath<Text>("rank/top/1/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/1/Text2").text = ContMgr.getCont("a3_newActive_17");//等级

            getComponentByPath<Text>("rank/top/2/Text").text = ContMgr.getCont("a3_newActive_15");//排名
            getComponentByPath<Text>("rank/top/2/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/2/Text2").text = ContMgr.getCont("a3_newActive_18");//战力

            getComponentByPath<Text>("rank/top/3/Text").text = ContMgr.getCont("a3_newActive_15");//排名
            getComponentByPath<Text>("rank/top/3/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/3/Text2").text = ContMgr.getCont("a3_newActive_19");//胜场

            getComponentByPath<Text>("rank/top/4/Text").text = ContMgr.getCont("a3_newActive_15");//排名
            getComponentByPath<Text>("rank/top/4/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/4/Text2").text = ContMgr.getCont("a3_newActive_20");//品阶

            getComponentByPath<Text>("rank/top/5/Text").text = ContMgr.getCont("a3_newActive_15");//排名
            getComponentByPath<Text>("rank/top/5/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/5/Text2").text = ContMgr.getCont("a3_newActive_21");//充值

            getComponentByPath<Text>("rank/top/6/Text").text = ContMgr.getCont("a3_newActive_15");//排名
            getComponentByPath<Text>("rank/top/6/Text1").text = ContMgr.getCont("a3_newActive_16");//名称
            getComponentByPath<Text>("rank/top/6/Text2").text = ContMgr.getCont("a3_newActive_22");//消费
            #endregion




            instans = this;
            xml = XMLMgr.instance.GetSXML("new_server_activity");
            new BaseButton(this.transform.FindChild("btn_close")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().close(this.uiName);
            };
            new BaseButton(this.transform.FindChild("left")).onClick = onLeft;
            new BaseButton(this.transform.FindChild("right")).onClick = onRight;

            leftPos = this.transform.FindChild("leftPos").GetComponent<RectTransform>();
            rightPos = this.transform.FindChild("rightPos").GetComponent<RectTransform>();
            nowPos = this.transform.FindChild("nowPos").GetComponent<RectTransform>();

            tab = this.transform.FindChild("tab");
            contents = this.transform.FindChild("contents");

            for (int i = 0; i < tab.childCount; i++) {
                new BaseButton(tab.GetChild(i)).onClick = (GameObject go) => {
                    if (!torun) return;
                    int lastid = idx;
                    idx = int.Parse(go.name);
                    if (lastid != idx)
                    {
                        setTab(idx, lastid);
                    }
                };
            }

            new BaseButton(this.transform.FindChild("contents/1/rank_btn")).onClick = (GameObject go) => { openRank(1); };
            new BaseButton(this.transform.FindChild("contents/2/rank_btn")).onClick = (GameObject go) => { openRank(2); };
            new BaseButton(this.transform.FindChild("contents/3/rank_btn")).onClick = (GameObject go) => { openRank(3); };
            new BaseButton(this.transform.FindChild("contents/4/rank_btn")).onClick = (GameObject go) => { openRank(4); };
            new BaseButton(this.transform.FindChild("contents/5/rank_btn")).onClick = (GameObject go) => { openRank(5); };
            new BaseButton(this.transform.FindChild("contents/6/rank_btn")).onClick = (GameObject go) => { openRank(6); };
            new BaseButton(this.transform.FindChild("rank/close")).onClick = (GameObject go) => { closeRank(); };
        }
        public override void onShowed()
        {
            idx = 1;
            setTab(idx);
            torun = true;
            a3_newActiveProxy.getInstance().addEventListener(a3_newActiveProxy.EVENT_RANK_INFO, OnSetRank);
            a3_newActiveProxy.getInstance().addEventListener(a3_newActiveProxy.EVENT_GET_REW, Ongetrew);
            a3_newActiveProxy.getInstance().SendProxy(1);
            a3_newActiveProxy.getInstance().SendProxy(2);
            if ( !a3_newActiveModel.getInstance().Show_active) {
                InterfaceMgr.getInstance().close(this.uiName);
            }
        }
        public override void onClosed()
        {
            a3_newActiveProxy.getInstance().removeEventListener(a3_newActiveProxy.EVENT_RANK_INFO, OnSetRank);
            a3_newActiveProxy.getInstance().removeEventListener(a3_newActiveProxy.EVENT_GET_REW, Ongetrew);
        }

        int idx = 1;
        bool torun = true;
        void onRight(GameObject go) {
            if (idx < contents.childCount && torun)
            {
                int lastid = idx;
                idx ++;
                //if (idx == 5) idx++;
                setTab(idx , lastid);
            }
        }
        void onLeft(GameObject go) {
            if (idx > 1 && torun)
            {
                int lastid = idx;
                idx -- ;
                //if (idx == 5) idx--;
                setTab(idx , lastid);
            }
        }

        void setTab(int id, int lastid = -1)
        {
            if (!allrew.ContainsKey (id)) {
                setView(id);
            }
            refreshView(id);
            for (int i = 0; i < tab.childCount; i++)
            {
                tab.GetChild(i).transform.FindChild("b").gameObject.SetActive(false);
            }
            tab.FindChild(id.ToString() + "/b").gameObject.SetActive(true);
            for (int i = 0; i < contents.childCount; i++)
            {
                contents.GetChild(i).gameObject.SetActive(false);
            }
            contents.FindChild(id.ToString()).gameObject.SetActive(true);
            changeTpye type = changeTpye.nul;
            if (lastid < 0) {
                type = changeTpye.nul;
            }
            else if (lastid> 0 && lastid > id) {
                type = changeTpye.toleft;
            }
            else if (lastid > 0 && lastid < id) {
                type = changeTpye.toright;
            }
            Do_Tween(id, lastid, type);
        }

        void Do_Tween(int id ,int lastid, changeTpye type) {
            if (type == changeTpye.toright)
            {
                Transform lastT = contents.FindChild(id.ToString());
                lastT.position = rightPos.position;
                Transform nowT = contents.FindChild(lastid.ToString());
                nowT.position = nowPos.position;
                lastT.gameObject.SetActive(true);
                nowT.gameObject.SetActive(true);
                Tween tween = null;
                tween = nowT.transform.DOMoveX(leftPos.transform.position.x, 0.5f);
                tween = lastT.transform.DOMoveX(nowPos.transform.position.x, 0.5f);
                tween.OnStart(() =>
                {
                    torun = false;
                });
                tween.OnComplete(() =>
                {
                    nowT.gameObject.SetActive(false);
                    torun = true;
                });
            }
            else if (type == changeTpye.toleft)
            {
                Transform lastT = contents.FindChild(id.ToString());
                lastT.position = leftPos.position;
                Transform nowT = contents.FindChild(lastid.ToString());
                nowT.position = nowPos.position;
                lastT.gameObject.SetActive(true);
                nowT.gameObject.SetActive(true);
                Tween tween = null;
                tween = nowT.transform.DOMoveX(rightPos.transform.position.x, 0.5f);
                tween = lastT.transform.DOMoveX(nowPos.transform.position.x, 0.5f);
                tween.OnStart(() => { torun = false; });
                tween.OnComplete(() =>
                {
                    nowT.gameObject.SetActive(false);
                    torun = true;
                });
            }
            else if (type == changeTpye.nul)
            {
                contents.FindChild(id.ToString()).position = nowPos.position;
            }
            //for (int i = 0;i< contents.childCount;i++) {
            //    contents.GetChild(i).gameObject.SetActive(false);
            //}
            //contents.FindChild(id.ToString()).gameObject.SetActive(true);
        }


        void setView(int id)
        {
            Transform ViewCon = contents.FindChild(id.ToString());
            GameObject lvl_one = ViewCon.FindChild("rews/item_list/scroll/0").gameObject;
            GameObject item_one = ViewCon.FindChild("rews/item_list/scroll/rew_item").gameObject;
            Transform lvl_con = ViewCon.FindChild("rews/item_list/scroll/content");
            List<SXML> listL = xml.GetNodeList("activity", "type==" + id);
            GameObject jinbi = this.transform.FindChild("jinbi").gameObject;
            GameObject zuanshi = this.transform.FindChild("zuanshi").gameObject;
            GameObject type_4 = this.transform.FindChild("type_4").gameObject;

            Transform act_tm = ViewCon.FindChild("time");
            if (act_tm != null)
            {
                Text t = act_tm.GetComponent<Text>();
                t.text = ConvertStringToDateTime(a3_newActiveModel.getInstance().kaifu_tm);
            }

            Transform act_tm_over = ViewCon.FindChild("time_over");
            if (act_tm_over != null)
            {
                Text t = act_tm_over.GetComponent<Text>();
                t.text = ConvertStringToDateTime(a3_newActiveModel.getInstance().kaifu_tm_over );
            }

            Transform get_time = ViewCon.FindChild("get_time");
            if (get_time != null) {
                Text t = get_time.GetComponent<Text>();
                t.text = ConvertStringToDateTime(a3_newActiveModel.getInstance().s_get_tm);
            }
            Transform get_time_over = ViewCon.FindChild("get_time_over");
            if (get_time_over != null)
            {
                Text t = get_time_over.GetComponent<Text>();
                t.text = ConvertStringToDateTime(a3_newActiveModel.getInstance().e_get_tm);
            }

            for (int i = 0;i< lvl_con.childCount;i++) {
                Destroy(lvl_con.GetChild(i).gameObject);
            }

            List<rewInfo> rew = new List<rewInfo>();
            int ids = 1;
            foreach (SXML x in listL)
            {
                uint rewid = x.getUint("id");
                GameObject clon = GameObject.Instantiate(lvl_one) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(lvl_con, false);
                List<SXML> itemlist = x.GetNodeList("RewardItem");
                if (itemlist != null && itemlist.Count > 0)
                {
                    foreach (SXML s in itemlist)
                    {
                        GameObject item_clon = GameObject.Instantiate(item_one) as GameObject;
                        item_clon.SetActive(true);
                        item_clon.transform.SetParent(clon.transform.FindChild("scrollview/con"), false);
                        uint intemid = (uint)s.getInt("item_id");
                        int num = s.getInt("value");
                        GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(intemid, false, num, 1, false);
                        icon.transform.SetParent(item_clon.transform, false);
                        new BaseButton(icon.transform).onClick = (GameObject go) =>
                        {
                            ArrayList paraList = new ArrayList();
                            paraList.Add(intemid);
                            paraList.Add(1);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
                        };
                    }
                }
                List<SXML> RewardValue = x.GetNodeList("RewardValue");
                if (RewardValue != null && itemlist.Count > 0)
                {
                    foreach (SXML s in RewardValue) {
                        GameObject item_clon = GameObject.Instantiate(item_one) as GameObject;
                        item_clon.SetActive(true);
                        item_clon.transform.SetParent(clon.transform.FindChild("scrollview/con"), false);
                        int type = s.getInt("type");
                        int count = s.getInt("value");
                        GameObject icon = null;
                        if (type == 2) {
                            icon = GameObject.Instantiate(jinbi) as GameObject;
                        }
                        else if (type == 3) {
                            icon = GameObject.Instantiate(zuanshi) as GameObject;
                        } else if (type == 4)
                        {
                            icon = GameObject.Instantiate(type_4) as GameObject;
                        }
                        icon.SetActive(true);
                        icon.transform.FindChild("num").GetComponent<Text>().text = count.ToString();
                        icon.transform.SetParent(item_clon.transform, false);
                    }
                }
                clon.transform.FindChild("rank/txt_rank").gameObject.SetActive(false );
                clon.transform.FindChild("rank/txt_self").gameObject.SetActive(false);
                rewInfo info = new rewInfo();
                if (x.getInt("order") > 0)
                {
                    if (x.getString("rank_limit") != "null")
                    {
                        clon.transform.FindChild("rank/txt_rank").gameObject.SetActive(true);
                        string[] rank = x.getString("rank_limit").Split(',');

                        string[] param = x.getString("param_limit").Split(',');

                        if (int.Parse(rank[0]) == int.Parse(rank[1]))
                        {
                            clon.transform.FindChild("rank/txt_rank/text").GetComponent<Text>().text = ContMgr.getCont("a3_newActive_text_"+ id, new List<string>() { rank[0], param[0] });
                        }
                        else
                        {
                            clon.transform.FindChild("rank/txt_rank/text").GetComponent<Text>().text = ContMgr.getCont("a3_newActive_text_" + id, new List<string>() { rank[0] + "-" + rank[1], param[0] });
                        }
                        info.minRank = int.Parse(rank[0]);
                        info.maxRank = int.Parse(rank[1]);
                        if (x.getString("param_limit") != null)
                        {
                            string[] param1 = x.getString("param_limit").Split(',');
                            if (param1.Length > 1)
                            {
                                info.maxValue = int.Parse(param1[1]);
                            }
                            info.minValue = int.Parse(param1[0]);
                        }
                        info.type = 1;
                    }
                    else if (x.getString("param_limit") != null)
                    {
                        clon.transform.FindChild("rank/txt_self").gameObject.SetActive(true);
                        string[] param = x.getString("param_limit").Split(',');
                        if (param.Length > 1)
                        {
                            clon.transform.FindChild("rank/txt_self/text").GetComponent<Text>().text = ContMgr.getCont("a3_newActive_slef_" + id, new List<string>() { ContMgr.getCont("a3_resetlvl_lv", new List<string>() { param[0], param[1] }) });
                            info.maxValue = int.Parse(param[1]);
                        }       
                        else {
                            clon.transform.FindChild("rank/txt_self/text").GetComponent<Text>().text = ContMgr.getCont("a3_newActive_slef_" + id, new List<string>() { param[0] }); 
                        }

                        info.minValue = int.Parse(param[0]);
                       
                        info.type = 2;
                    }
                    new BaseButton(clon.transform.FindChild("buttn")).onClick = (GameObject go) => {
                        a3_newActiveProxy.getInstance().getRew(3, (uint)id, rewid);
                    };
                }
                info.rank = ids;
                info.obj = clon;
                info.rewid = rewid;
                rew.Add(info);
                ids++;
            }
            allrew[id] = rew;
        }

        public string ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string st = dtStart.AddSeconds(timeStamp).ToString("yyyy"+ ContMgr.getCont("nian")+"MM"+ContMgr.getCont("yue")+"dd"+ ContMgr.getCont("ri"));
            return st;
        }
        void OnSetRank(GameEvent e)
        {
            Variant data = e.data;
            if (data.ContainsKey("level_rank"))
            {
                refreshView(1);
            }
            if (data.ContainsKey("combpt_rank"))
            {
                refreshView(2);
            }
            if (data.ContainsKey("pk_rank"))
            {
                refreshView(3);
            }
            if (data.ContainsKey("wing_rank"))
            {
                refreshView(4);
            }
            if (data.ContainsKey("recharge_rank"))
            {
                refreshView(5);
            }
            if (data.ContainsKey("pay_rank"))
            {
                refreshView(6);
            }
        }


        void closeRank() {
            Transform rank = this.transform.FindChild("rank");
            rank.gameObject.SetActive(false);
        }

        void openRank(int id)
        {
             Dictionary<int, Rankinfo> info = new Dictionary<int, Rankinfo>();
            Transform rank = this.transform.FindChild("rank");
            rank.gameObject.SetActive(true);
            Transform con_top = rank.transform.FindChild("top");
            Transform Con = rank.FindChild("scroll_rect/contain");
            GameObject item = rank.FindChild("scroll_rect/item").gameObject;
            for (int i = 0; i< con_top.childCount;i++) {
                con_top.GetChild(i).gameObject.SetActive(false);
            }
            rank.FindChild("scroll_rect").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            con_top.FindChild(id.ToString()).gameObject.SetActive(true);
            for (int i = 0; i < Con.childCount;i++) {
                Destroy(Con.GetChild (i).gameObject);
            }

            switch (id) {
                case 1:
                    info = a3_newActiveModel.getInstance().level_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con,false);
                        clon.transform.SetAsFirstSibling ();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = ContMgr.getCont("a3_resetlvl_lv", new List<string>() { one.zhuan.ToString (), one.lvl.ToString() });
                    }
                    break;
                case 2:
                    info = a3_newActiveModel.getInstance().combpt_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con, false);
                        clon.transform.SetAsFirstSibling();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.combpt.ToString();
                    }
                    break;
                case 3:
                    info = a3_newActiveModel.getInstance().pk_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con, false);
                        clon.transform.SetAsFirstSibling();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.total_win.ToString();
                    }
                    break;
                case 4:
                    info = a3_newActiveModel.getInstance().wing_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con, false);
                        clon.transform.SetAsFirstSibling();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.stage_wing.ToString ();
                    }
                    break;
                case 5:
                    info = a3_newActiveModel.getInstance().recharge_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con, false);
                        clon.transform.SetAsFirstSibling();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.recharge_num.ToString();
                    }
                    break;
                case 6:
                    info = a3_newActiveModel.getInstance().pay_rank;
                    foreach (Rankinfo one in info.Values)
                    {
                        GameObject clon = Instantiate(item) as GameObject;
                        clon.SetActive(true);
                        clon.transform.SetParent(Con, false);
                        clon.transform.SetAsFirstSibling();
                        clon.transform.FindChild("1/Text").GetComponent<Text>().text = one.rank.ToString();
                        clon.transform.FindChild("2/Text").GetComponent<Text>().text = one.name;
                        clon.transform.FindChild("3/Text").GetComponent<Text>().text = one.pay_num.ToString();
                    }
                    break;
            }

        }


        void refreshView( int id) {

            Transform ViewCon = contents.FindChild(id.ToString());
            if (ViewCon != null)
            {
                RectTransform lvl_con = ViewCon.FindChild("rews/item_list/scroll/content").GetComponent<RectTransform>();
                lvl_con.anchoredPosition = Vector3.zero;
            }
            if (allrew.ContainsKey(id))
            {
                List<rewInfo> rew_l = allrew[id];
                switch (id)
                {
                    case 1:
                        bool HaveGet_lvl = false;
                        int myRank_lvl = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().level_rank.Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_lvl = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {
                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_lvl > 0)
                                {
                                    if (a3_newActiveModel.getInstance().zhuan >= r.minValue)
                                    {
                                        if (myRank_lvl >= r.minRank && myRank_lvl <= r.maxRank)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().level_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_lvl = true;
                                        }
                                        else
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                        }
                                    }else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_lvl)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else {
                                    if (r.maxValue > 0 )
                                    {
                                        if (a3_newActiveModel.getInstance().zhuan > r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_lvl = true;
                                            if (a3_newActiveModel.getInstance().level_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        } else if (a3_newActiveModel.getInstance().zhuan == r.minValue && a3_newActiveModel.getInstance().lvl >= r.maxValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_lvl = true;
                                            if (a3_newActiveModel.getInstance().level_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                            }
                            //int rank = r.rank;
                            //GameObject obj = r.obj;
                            //if (a3_newActiveModel.getInstance().level_rank.ContainsKey(rank))
                            //{
                            //    Rankinfo info = a3_newActiveModel.getInstance().level_rank[rank];
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = info.name;
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(true);
                            //    obj.transform.FindChild("lvl").gameObject.SetActive(true);
                            //    obj.transform.FindChild("carr_bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr_bg(info.carr));
                            //    obj.transform.FindChild("carr_bg/carr").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr(info.carr));
                            //    obj.transform.FindChild("lvl").GetComponent<Text>().text = ContMgr.getCont("a3_resetlvl_lv", new List<string>() { info.zhuan.ToString(), info.lvl.ToString() });
                            //    if (info.cid == PlayerModel.getInstance().cid && !a3_newActiveModel.getInstance().level_awd && a3_newActiveModel.getInstance().CanGetREW)
                            //    {
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                            //    }           
                            //    else {
                            //        if (info.cid == PlayerModel.getInstance().cid && a3_newActiveModel.getInstance().level_awd) {
                            //            obj.transform.FindChild("buttn/Text").GetComponent <Text>().text = ContMgr.getCont("yilingqu");
                            //        }
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    }
                            //}
                            //else {
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(false);
                            //    obj.transform.FindChild("lvl").gameObject.SetActive(false);
                            //}
                        }
                        break;
                    case 2:
                        bool HaveGet_combpt = false;
                        int myRank_combpt = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().combpt_rank.Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_combpt = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {
                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_combpt > 0)
                                {
                                    if (myRank_combpt >= r.minRank && myRank_combpt <= r.maxRank)
                                    {
                                        if (a3_newActiveModel.getInstance().combpt >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().combpt_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_combpt = true;
                                        }
                                        else
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                        }
                                    }else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_combpt)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    if (r.minValue  > 0)
                                    {
                                        if (a3_newActiveModel .getInstance ().combpt >= r.minValue )
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_combpt = true;
                                            if (a3_newActiveModel.getInstance().combpt_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                        //if (a3_newActiveModel.getInstance().zhuan > r.maxValue)
                                        //{
                                        //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                        //    HaveGet_combpt = true;
                                        //    if (a3_newActiveModel.getInstance().combpt_awd)
                                        //    {
                                        //        obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                        //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;       
                                        //    }
                                        //}
                                        //else if (a3_newActiveModel.getInstance().zhuan == r.maxValue && a3_newActiveModel.getInstance().lvl >= r.maxValue)
                                        //{
                                        //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                        //    HaveGet_combpt = true;
                                        //    if (a3_newActiveModel.getInstance().combpt_awd)
                                        //    {
                                        //        obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                        //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                        //    }
                                        //}
                                        //else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                            }
                            //int rank = r.rank;
                            //GameObject obj = r.obj;
                            //if (a3_newActiveModel.getInstance().combpt_rank.ContainsKey(rank))
                            //{
                            //    Rankinfo info = a3_newActiveModel.getInstance().combpt_rank[rank];
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = info.name;
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(true);
                            //    obj.transform.FindChild("combpt").gameObject.SetActive(true);
                            //    obj.transform.FindChild("carr_bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr_bg(info.carr));
                            //    obj.transform.FindChild("carr_bg/carr").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr(info.carr));
                            //    obj.transform.FindChild("combpt").GetComponent<Text>().text = info.combpt.ToString();
                            //    if (info.cid == PlayerModel.getInstance().cid && !a3_newActiveModel.getInstance().combpt_awd && a3_newActiveModel.getInstance().CanGetREW)
                            //    {
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                            //    }
                            //    else {
                            //        if (info.cid == PlayerModel.getInstance().cid && a3_newActiveModel.getInstance().combpt_awd)
                            //        { obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu"); }
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    }
                            //}
                            //else
                            //{
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(false);
                            //    obj.transform.FindChild("combpt").gameObject.SetActive(false);
                            //}
                        }
                        break;
                    case 3:
                        bool HaveGet_pk = false;
                        int myRank_pk = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().pk_rank .Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_pk = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {
                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_pk > 0)
                                {
                                    if (myRank_pk >= r.minRank && myRank_pk <= r.maxRank)
                                    {
                                        if (a3_newActiveModel.getInstance().pk >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().pk_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_pk = true;
                                        }
                                        else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                                    }
                                    else
                                    {
                                        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_pk)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    if (r.minValue > 0)
                                    {
                                        if (a3_newActiveModel.getInstance().pk >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_pk = true;
                                            if (a3_newActiveModel.getInstance().pk_awd )
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;

                                    }
                                }
                            }

                            //int rank = r.rank;
                            //GameObject obj = r.obj;
                            //if (a3_newActiveModel.getInstance().pk_rank.ContainsKey(rank))
                            //{
                            //    Rankinfo info = a3_newActiveModel.getInstance().pk_rank[rank];
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = info.name;
                            //    obj.transform.FindChild("pvplv").gameObject.SetActive(true);
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(true);
                            //    obj.transform.FindChild("carr_bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr_bg(info.carr));
                            //    obj.transform.FindChild("carr_bg/carr").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr(info.carr));
                            //    obj.transform.FindChild("pvplv").GetComponent<Text>().text = info.total_win.ToString();
                            //    if (info.cid == PlayerModel.getInstance().cid && !a3_newActiveModel.getInstance().pk_awd && a3_newActiveModel.getInstance().CanGetREW)
                            //    {
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                            //    }
                            //    else {
                            //        if (info.cid == PlayerModel.getInstance().cid && a3_newActiveModel.getInstance().pk_awd)
                            //        { obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu"); }
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            //}
                            //else {
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    obj.transform.FindChild("pvplv").gameObject.SetActive(false);
                            //    obj.transform.FindChild("carr_bg").gameObject.SetActive(false);
                            //}
                        }
                        break;
                    case 4:
                        bool HaveGet_wing= false;
                        int myRank_wing = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().wing_rank .Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_wing = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {
                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_wing > 0)
                                {
                                    if (myRank_wing >= r.minRank && myRank_wing <= r.maxRank)
                                    {
                                        if (a3_newActiveModel.getInstance().wing >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().wing_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_wing = true;
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                    else
                                    {
                                        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_wing)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    if (r.minValue > 0)
                                    {
                                        if (a3_newActiveModel.getInstance().wing  >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_wing = true;
                                            if (a3_newActiveModel.getInstance().wing_awd )
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;

                                    }else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }

                            }

                                //int rank = r.rank;
                                //int mid = r.mid;
                                //GameObject obj = r.obj;
                                //if (a3_newActiveModel.getInstance().boss_kill_rank.ContainsKey(mid))
                                //{
                                //    Rankinfo info = a3_newActiveModel.getInstance().boss_kill_rank[mid];
                                //    if (info.name != null)
                                //        obj.transform.FindChild("name").GetComponent<Text>().text = info.name;
                                //    else
                                //        obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");

                                //    obj.transform.FindChild("carr_bg").gameObject.SetActive(true);
                                //    obj.transform.FindChild("carr_bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr_bg(info.carr));
                                //    obj.transform.FindChild("carr_bg/carr").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(get_Carr(info.carr));
                                //    obj.transform.FindChild("num").GetComponent<Text>().text = info.num.ToString();
                                //    if (info.cid == PlayerModel.getInstance().cid && !a3_newActiveModel.getInstance().boss_awd.Contains(mid) && a3_newActiveModel.getInstance().CanGetREW)
                                //    {
                                //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                //    }
                                //    else {
                                //        if (info.cid == PlayerModel.getInstance().cid && a3_newActiveModel.getInstance().boss_awd.Contains(mid))
                                //        { obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu"); }
                                //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                //    }
                                //}
                                //else {
                                //    obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");
                                //    obj.transform.FindChild("num").GetComponent<Text>().text = 0.ToString();
                                //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                //    obj.transform.FindChild("carr_bg").gameObject.SetActive(false);
                                //}
                            }
                        break;
                    case 5:
                        bool HaveGet_recharge = false;
                        int myRank_recharge = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().recharge_rank.Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_recharge = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {

                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_recharge > 0)
                                {
                                    if (myRank_recharge >= r.minRank && myRank_recharge <= r.maxRank)
                                    {
                                        if (a3_newActiveModel.getInstance().recharge >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().recharge1_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_recharge = true;
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                    else
                                    {
                                        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_recharge)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    if (r.minValue > 0)
                                    {
                                        if (a3_newActiveModel.getInstance().recharge  >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_recharge = true;
                                            if (a3_newActiveModel.getInstance().recharge1_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;

                                    }
                                }

                            }
                            //GameObject obj = r.obj;
                            //if (r.pay_value <= a3_newActiveModel.getInstance().recharge && !a3_newActiveModel.getInstance().recharge_awd.Contains ((int)r.rewid ))
                            //{
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                            //}
                            //else {
                            //    if (a3_newActiveModel.getInstance().recharge_awd.Contains((int)r.rewid))        
                            //    { obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu"); }
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //}

                            //if (a3_newActiveModel.getInstance().recharge >= r.pay_value)
                            //{
                            //    obj.transform.FindChild("bar/slider").GetComponent<Image>().fillAmount = 1;
                            //}
                            //else
                            //{
                            //    obj.transform.FindChild("bar/slider").GetComponent<Image>().fillAmount = (float)a3_newActiveModel.getInstance().recharge / r.pay_value;
                            //}
                            //obj.transform.FindChild("bar/value").GetComponent<Text>().text = a3_newActiveModel.getInstance().recharge + "/"+ r.pay_value;
                        }
                        break;
                    case 6:
                        bool HaveGet_pay = false;
                        int myRank_pay = 0;
                        foreach (Rankinfo info in a3_newActiveModel.getInstance().pay_rank .Values)
                        {
                            if (info.cid == PlayerModel.getInstance().cid)
                            {
                                myRank_pay = info.rank;
                                break;
                            }
                        }
                        foreach (rewInfo r in rew_l)
                        {
                            GameObject obj = r.obj;
                            if (!a3_newActiveModel.getInstance().CanGetREW)
                            {
                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                continue;
                            }
                            if (r.type == 1)
                            {
                                if (myRank_pay > 0)
                                {
                                    if (myRank_pay >= r.minRank && myRank_pay <= r.maxRank)
                                    {
                                        if (a3_newActiveModel.getInstance().pay >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            if (a3_newActiveModel.getInstance().pay_awd)
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                            HaveGet_pay = true;
                                        }else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                    else
                                    {
                                        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                    }
                                }
                                else { obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false; }
                            }
                            else if (r.type == 2)
                            {
                                if (HaveGet_pay)
                                {
                                    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                }
                                else
                                {
                                    if (r.minValue > 0)
                                    {
                                        if (a3_newActiveModel.getInstance().pay  >= r.minValue)
                                        {
                                            obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                                            HaveGet_pay = true;
                                            if (a3_newActiveModel.getInstance().pay_awd )
                                            {
                                                obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu");
                                                obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                                            }
                                        }
                                        else obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;

                                    }
                                }

                            }
                            //int rank = r.rank;
                            //GameObject obj = r.obj;
                            //if (a3_newActiveModel.getInstance().pay_rank.ContainsKey(rank))
                            //{
                            //    Rankinfo info = a3_newActiveModel.getInstance().pay_rank[rank];
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = info.name;
                            //    if (info.cid == PlayerModel.getInstance().cid && !a3_newActiveModel.getInstance().pay_awd && a3_newActiveModel.getInstance().CanGetREW)
                            //    {
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = true;
                            //    }
                            //    else {
                            //        if (info.cid == PlayerModel.getInstance().cid && a3_newActiveModel.getInstance().pay_awd)
                            //        { obj.transform.FindChild("buttn/Text").GetComponent<Text>().text = ContMgr.getCont("yilingqu"); }
                            //        obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //    }
                            //    obj.transform.FindChild("info").gameObject.SetActive(true);
                            //    obj.transform.FindChild("info").GetComponent<Text>().text = info.pay_num.ToString();
                            //}
                            //else {
                            //    obj.transform.FindChild("info").gameObject.SetActive(false);
                            //    obj.transform.FindChild("name").GetComponent<Text>().text = ContMgr.getCont("wu");
                            //    obj.transform.FindChild("buttn").GetComponent<Button>().interactable = false;
                            //}
                        }
                        break;
                }
            }
        }

        string get_Carr(int carr)
        {
            string file = "";
            switch (carr)
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
        string get_Carr_bg(int carr)
        {
            string file = "";
            switch (carr)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_bg2";
                    break;
                case 3:
                    file = "icon_job_icon_bg3";
                    break;
                case 4:
                    file = "icon_job_icon_bg4";
                    break;
                case 5:
                    file = "icon_job_icon_bg5";
                    break;
            }
            return file;
        }

        void Ongetrew(GameEvent e)
        {
            Variant data = e.data;
            int act_type = data["act_type"];
            int award_id = data["award_id"];
            switch (act_type)
            {
                case 1:
                    a3_newActiveModel.getInstance().level_awd = true;
                    break;
                case 2:
                    a3_newActiveModel.getInstance().combpt_awd = true;
                    break;
                case 3:
                    a3_newActiveModel.getInstance().pk_awd = true;
                    break;
                case 4:
                    a3_newActiveModel.getInstance().wing_awd = true;
                    //SXML x = xml.GetNode("activity", "id==" + award_id);
                    //int mid = x.getInt("m_id");
                    //if (!a3_newActiveModel.getInstance().boss_awd.Contains (mid))
                    //{
                    //    a3_newActiveModel.getInstance().boss_awd.Add(mid);
                    //}
                    break;
                case 5:
                    a3_newActiveModel.getInstance().recharge1_awd = true;
                    //if (!a3_newActiveModel.getInstance().recharge_awd.Contains(award_id))
                    //{
                    //    a3_newActiveModel.getInstance().recharge_awd.Add(award_id);
                    //}
                    break;
                case 6:
                    a3_newActiveModel.getInstance().pay_awd = true;
                    break;
            }
            refreshView(act_type);
            fly_text(award_id);
            InterfaceMgr.doCommandByLua("a1_low_fightgame.canget_newact", "ui/interfaces/low/a1_low_fightgame", a3_newActiveModel.getInstance().setGet());
        }

        void fly_text(int id)
        {
            SXML x = xml.GetNode("activity", "id==" + id);
            List<SXML> list = x.GetNodeList("RewardItem");
            List<SXML> RewardValue = x.GetNodeList("RewardValue");
            if ((list != null && list.Count > 0) || (RewardValue != null && RewardValue.Count > 0))
            {
                if (list != null && list.Count > 0)
                {
                    foreach (SXML item in list)
                    {
                        int item_id = item.getInt("item_id");
                        int num = item.getInt("value");
                        SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + item_id);
                        string name = item_xml.getString("item_name");
                        flytxt.instance.fly(ContMgr.getCont("huodeitem", new List<string>() { name, num.ToString() }));
                    }
                }
                if (RewardValue != null && RewardValue.Count > 0) {
                    foreach (SXML item in list)
                    {
                        int type = item.getInt("type");
                        int num = item.getInt("value");
                        if (type == 2) {
                            flytxt.instance.fly(ContMgr.getCont("getjinbi") + num);
                        } else if (type == 3)
                        {
                            flytxt.instance.fly(ContMgr.getCont("getzuanshi") + num);
                        }
                        else if (type == 4) {
                            flytxt.instance.fly(ContMgr.getCont("getbangzuan") + num);
                        }
                    }
                }
            }
            else {
                flytxt.instance.fly(ContMgr.getCont("a3_newAvctive_fanli"));
            }
        }

    }


    
    enum changeTpye {
        nul = 0,
        toleft =1,
        toright =2
    }
}
