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
using DG.Tweening;
namespace MuGame
{
    class a3_fb_finish : FloatUi
    {
        public static a3_fb_finish instance;
        Text closeTime;     //关闭副本时间
        Text finishTiem;    //完成时间
        Text kmNum;         //杀敌数
        Text getNum;        //获得经验数量
        Text goldNum;       //获得金币数量
        Text text1;
        Text text2;


        double closetime;
        bool _NewOne;
        double finishtime;
        int kmnum;
        uint getnum;
        uint goldnum;
        int getach;
        int getmoney;
        int shengwu;//圣物
        public double close_time = 0;
        string icon;
        string icon1;
        bool closefb_way;


        private GameObject m_SelfObj;//角色的avatar
        private GameObject m_Self_Camera;//拍摄avatar的摄像机
        private ProfessionAvatar m_proAvatar;
        Transform model;
        //public static double allEXP;
        //评价
        GameObject bgwin;
        GameObject bgdefet;
        GameObject image_S;
        GameObject image_A;
        GameObject image_B;
        GameObject reward;
        GameObject contain;
        GameObject pic_icon;
        GameObject ar_result;
        GameObject itempicc;//道具奖励
        GameObject jjc;//斗技场
        GameObject jdzc;//据点战场
        GameObject cityWAr;//城战
        GameObject yiwufb_defet;
        GameObject icon_star;
        public List<int> pos_i = new List<int>();
        List<string> pos_i_name = new List<string>();
        GameObject tip_text;
        
        int score;
        uint ltpid;
        public static BaseRoomItem room;

        public static bool ismlzd = false;//是不是从磨炼之地出来的

        public override void init()
        {
            inText();
            closefb_way = false;
            yiwufb_defet = transform.FindChild("yiwufb_defet").gameObject;
            bgwin = transform.FindChild("result_bg/bgwin").gameObject;
            bgdefet = transform.FindChild("result_bg/bgdefet").gameObject;
            image_S = transform.FindChild("win/success/icon/Images").gameObject;
            image_A = transform.FindChild("win/success/icon/Imagea").gameObject;
            image_B = transform.FindChild("win/success/icon/Imageb").gameObject;
            reward = transform.FindChild("win/success/gift/reward").gameObject;
            contain = transform.FindChild("win/success/gift/contain").gameObject;
            pic_icon = transform.FindChild("ar_result/icon").gameObject;
            ar_result = transform.FindChild("ar_result").gameObject;
            text1 = ar_result.transform.FindChild("Text1").GetComponent<Text>();
            text2 = ar_result.transform.FindChild("Text2").GetComponent<Text>();
            jjc = transform.FindChild("jjc").gameObject;
            icon_star = jjc.transform.FindChild("iocn").gameObject;
            close_time = 0;
            jdzc = transform.FindChild("jdzc").gameObject;
            cityWAr = transform.FindChild("cityWar").gameObject;
            tip_text = this.transform.FindChild("Text_tip").gameObject;

            model = transform.FindChild("model");
            new BaseButton(transform.FindChild("btn_close")).onClick = (GameObject go) =>
            {
                if (closefb_way == false)
                {
                    a3_insideui_fb.instance.light_biu.gameObject.SetActive(true);
                    a3_insideui_fb.instance.exittime.gameObject.SetActive(true);
                    a3_insideui_fb.instance.close_time = (double)closetime;
                    //a3_insideui_fb.instance.close_time = 80 + (double)muNetCleint.instance.CurServerTimeStamp;
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_FB_FINISH);
              
            };


            closeTime = transform.FindChild("btn_close/closeTime").GetComponent<Text>();
            transform.FindChild("btn_close/closeTime").gameObject.SetActive(false);

            new BaseButton(transform.FindChild("win/fail/icon/bag")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BAG);
            };
            new BaseButton(transform.FindChild("win/fail/icon/fl")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AWARDCENTER);
            };
            new BaseButton(transform.FindChild("win/fail/icon/skill")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SKILL_A3);
            };
            new BaseButton(transform.FindChild("win/fail/icon/zhs")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW);
            };
            new BaseButton(transform.FindChild("win/fail/icon/dz")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP);
            };


            jjc.SetActive(false);
            jdzc.SetActive(false);
            cityWAr.SetActive(false);
        }

        void inText()
        {
            this.transform.FindChild("yiwufb_defet/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_1");//不要气馁，提升战斗力再来挑战
            this.transform.FindChild("btn_close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_2");//确认
            this.transform.FindChild("Text_tip").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_3");//单人副本掉落的物品会被自动拾取

            this.transform.FindChild("win/fail/TextOne").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_4");//失敗了也不要氣餒，提升戰鬥力后再來挑戰
            this.transform.FindChild("win/fail/icon/dz/TextSix").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_8");//失敗了也不要氣餒，提升戰鬥力后再來挑戰
            this.transform.FindChild("win/fail/icon/zhs/TextFive").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_7");//失敗了也不要氣餒，提升戰鬥力后再來挑戰
            this.transform.FindChild("win/fail/icon/skill/TextFour").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_6");//失敗了也不要氣餒，提升戰鬥力后再來挑戰
            this.transform.FindChild("win/fail/icon/bag/TextTwo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_5");//失敗了也不要氣餒，提升戰鬥力后再來挑戰
        }
        public override void onShowed()
        {
            if (room is MLZDRoom)
            {
                //打开活动界面
                ismlzd = true;
            }
            
            InterfaceMgr.getInstance().closeAllWin();
            a3_insideui_fb.instance?.enter_pic2.SetActive(false);
            jjc.SetActive(false);
            jdzc.SetActive(false);
            cityWAr.SetActive(false);
            ar_result.SetActive(false);
            getGameObjectByPath("win/success").SetActive(false);
            getGameObjectByPath("state_successed").SetActive(false);
            transform.FindChild("state_successed/bg/goldNum").gameObject.SetActive(false);
            transform.FindChild("state_successed/bg/getNum").gameObject.SetActive(false);
            tip_text.SetActive(true);
            bgdefet.SetActive(false);
            bgwin.SetActive(false);
            yiwufb_defet.SetActive(false);
            //  closefb_way = false;
            transform.FindChild("btn_close/closeTime").gameObject.SetActive(false);
            //createAvatar();
            closetime = 0;
            close_time = 0;
            Variant data = (Variant)uiData[0];
            if (data.ContainsKey("ltpid"))
            {
                ltpid = data["ltpid"];
            }
            if (data.ContainsKey("score"))
            {
                score = data["score"];
            }
            else score = 0;

            if (data.ContainsKey("close_tm"))
            {

                double ct = data["close_tm"];
                closetime = ct;
            }
            if (data.ContainsKey("win"))
            {

                int ct = data["win"];

                Transform wint = getTransformByPath("win");
                wint.gameObject.SetActive(true);
                if (ct > 0 && wint != null)
                {

                    wint.FindChild("success").gameObject.SetActive(true);
                    wint.FindChild("fail").gameObject.SetActive(false);
                }
                else if (wint != null)
                {
                    closeWindow();
                    // InterfaceMgr.getInstance().floatUI.localScale = Vector3.zero;
                    if (GameObject.Find("GAME_CAMERA/myCamera"))
                    {
                        GameObject cameraOBJ = GameObject.Find("GAME_CAMERA/myCamera");
                        if (!cameraOBJ.GetComponent<DeathShader>())
                        {
                            cameraOBJ.AddComponent<DeathShader>();
                        }
                        else
                        {
                            cameraOBJ.GetComponent<DeathShader>().enabled = true;
                        }
                    }
                    wint.FindChild("success").gameObject.SetActive(false);
                    wint.FindChild("fail").gameObject.SetActive(true);


                    getGameObjectByPath("state_successed").SetActive(false);
                }
                if (data.ContainsKey("item_drop"))
                {

                    ct = data["item_drop"]._arr.Count;
                }
                else ct = 0;
                if (ct >= 0)
                {


                    getGameObjectByPath("state_successed").SetActive(true);
                    //getGameObjectByPath("state_failed").SetActive(false);

                    finishTiem = getComponentByPath<Text>("state_successed/bg/fnTime/time");
                    kmNum = getComponentByPath<Text>("state_successed/bg/kmNum/num");
                    getNum = getComponentByPath<Text>("state_successed/bg/getNum/num");
                    goldNum = getComponentByPath<Text>("state_successed/bg/goldNum/num");
                }
                else
                {
                    getGameObjectByPath("state_successed").SetActive(false);
                    //getGameObjectByPath("state_failed").SetActive(true);
                    //finishTiem = getComponentByPath<Text>("state_failed/bg/fnTime/time");
                    //kmNum = getComponentByPath<Text>("state_failed/bg/kmNum/num");
                    //getNum = getComponentByPath<Text>("state_failed/bg/getNum/num");
                    //goldNum = getComponentByPath<Text>("state_failed/bg/goldNum/num");
                }
            }

            finishtime = 0;
            if (uiData.Count > 1)
            {
                finishtime = (double)uiData[1];
            }
            //TimeSpan ts = new TimeSpan(0, 0, (int)finishtime);

            float tss = 0;
            uint tkn = 0, ten = 0, tgn = 0;
            int ach = 0, mon = 0;
            DOTween.To(() => tss, (float s) =>
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)s);
                finishTiem.text = (int)ts.TotalHours + ":" + (int)ts.Minutes + ":" + (int)ts.Seconds;


            }, (float)finishtime, 1f);

            evaluation(score);

            kmnum = 0;
            if (uiData.Count > 2)
            {
                kmnum = (int)uiData[2];
            }
            DOTween.To(() => (int)tkn, (int s) =>
            {
                tkn = (uint)s;
                kmNum.text = tkn.ToString();
            }, kmnum, 1f);


            if (room is MoneyRoom)
            {
                if (room != null)
                {
                    transform.FindChild("state_successed/bg/goldNum").gameObject.SetActive(true);

                }
            }


            if (room is ExpRoom)
            {
                if (room != null)
                {
                    transform.FindChild("state_successed/bg/getNum").gameObject.SetActive(true);

                }
            }
            //if (room != null)
            //{
            //    goldnum = room.goldnum;
            //}
            //DOTween.To(() => tgn, (uint s) =>
            //{
            //    tgn = s;
            //    goldNum.text = tgn.ToString();
            //}, goldnum, 1f);

            //}


            if (room is PVPRoom)
            {
                if (room != null)
                {
                    getach = room.getach;
                }
                DOTween.To(() => ach, (int s) =>
                {
                    ach = s;
                    getNum.text = ach.ToString();
                }, getach, 1f);

                if (room != null)
                {
                    getmoney = room.getExp;
                }
                DOTween.To(() => mon, (int s) =>
                {
                    mon = s;
                    goldNum.text = mon.ToString();
                }, getmoney, 1f);

                MapProxy.getInstance().Win_uiData = "sports_jjc";
                MapProxy.getInstance().openWin = InterfaceMgr.A3_SPORTS;
            }
            else
            {
                if (room != null)
                {
                    getnum = room.expnum;
                }
                DOTween.To(() => ten, (uint s) =>
                {
                    ten = s;
                    getNum.text = ten.ToString();
                }, getnum, 1f);
                if (room != null)
                {
                    //goldnum = room.goldnum;
                    goldnum = a3_insideui_fb.AllMoneynum;
                }
                DOTween.To(() => tgn, (uint s) =>
                {
                    tgn = s;
                    goldNum.text = tgn.ToString();
                }, goldnum, 1f);
            }
            //if (data.ContainsKey("ltpid")) {
            //    int tid = data["ltpid"];
            //    if (tid == 101) {
            //        getNum.text = "获得经验量：" + getnum;
            //    }
            //    else if (tid == 102) {
            //        getNum.text = "获得金币量：" + getnum;
            //    }
            //}
            _NewOne = true;
            instance = this;
            room.getExp = 0;
            room.getach = 0;

            Variant d = SvrLevelConfig.instacne.get_level_data(ltpid);
            if (data["win"] == 0 || room is PVPRoom || d.ContainsKey("shengwu") || room is PlotRoom ||room is ExpRoom||room is MoneyRoom ||room is FSWZRoom||room is MLZDRoom || room is JDZCRoom || room is CityWarRoom)
            {
                a3_liteMinimap.instance?.taskinfo?.SetActive(true);
                a3_insideui_fb.instance?.enter_pic2?.SetActive(false);

                LevelProxy.getInstance().open_pic = false;
                closefb_way = true;
                //InterfaceMgr.getInstance().close(InterfaceMgr.TARGET_HEAD);
            }
            else
            {
                closefb_way = false;
            }


            if (closefb_way == true)
            {
                close_time = 0;
                transform.FindChild("btn_close/closeTime").gameObject.SetActive(true);
            }
            if (closefb_way == false)
            {
                close_time = (double)closetime - (double)muNetCleint.instance.CurServerTimeStamp - 3;
                transform.FindChild("btn_close/closeTime").gameObject.SetActive(false);
            }

            if (d.ContainsKey("shengwu") && d.ContainsKey("icon"))
            {
                shengwu = d["shengwu"];
                icon = d["icon"];
                if (data["win"] == 0)
                {
                    jjc.SetActive(false);
                    jdzc.SetActive(false);
                    cityWAr.SetActive(false);
                    ar_result.SetActive(false);
                    getGameObjectByPath("win").SetActive(false);
                    getGameObjectByPath("win/success").SetActive(false);
                    getGameObjectByPath("state_successed").SetActive(false);
                    transform.FindChild("state_successed/bg/goldNum").gameObject.SetActive(false);
                    bgdefet.SetActive(false);
                    bgwin.SetActive(false);
                    yiwufb_defet.SetActive(true);
                    return;
                }
            }
            else shengwu = 0;
            if (data["win"] == 1 && d.ContainsKey("shengwu") && d.ContainsKey("des"))
            {
                icon1 = d["des"];
                string[] codess = icon1.Split(',');
                // SXML xml = XMLMgr.instance.GetSXML("accent_relic.relic", "type=="+codess[0].ToString());
                //mid = xml.getUint("obj");
                List<SXML> listSxml = null;
                if (listSxml == null)
                {
                    listSxml = XMLMgr.instance.GetSXMLList("accent_relic.relic");
                    //List<SXML> xml2 = XMLMgr.instance.GetSXMLList("accent_relic.relic", "carr=="+ PlayerModel.getInstance().profession);     , "carr==" + PlayerModel.getInstance().profession              
                    for (int i = 0; i < listSxml.Count; i++)
                    {

                        if (listSxml[i].getInt("carr") == PlayerModel.getInstance().profession && listSxml[i].getString("type") == codess[0].ToString())
                        {

                            List<SXML> god_id = listSxml[i].GetNodeList("relic_god", "id==" + codess[1].ToString());
                            {

                                foreach (SXML x in god_id)
                                {

                                    text1.text = x.getString("des1");
                                    text2.text = x.getString("des2");
                                }
                            }
                        }
                    }


                }




                //foreach (SXML x in xml)
                //{                
                //    if (x.getString("id") == codess[1])
                //    {                       
                //        text1.text = x.getString("des1");
                //        text2.text = x.getString("des2");
                //    }
                // }


                //    if (xml.getInt("carr") == PlayerModel.getInstance().profession)
                //{

                //    text1.text = xml.getString("des1");
                //    text2.text = xml.getString("des2");

                //}
            }







            if (shengwu == 1)
            {
                ar_result.SetActive(true);
                string[] codes = icon.Split(',');
                if (PlayerModel.getInstance().profession == 2)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + codes[0]);
                if (PlayerModel.getInstance().profession == 3)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + codes[1]);
                if (PlayerModel.getInstance().profession == 5)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + codes[2]);

                bgwin.SetActive(false);
                getGameObjectByPath("win").SetActive(false);
                getGameObjectByPath("win/success").SetActive(false);
                transform.FindChild("state_successed").gameObject.SetActive(false);
                jjc.SetActive(false);
                jdzc.SetActive(false);
                cityWAr.SetActive(false);
            }
            jjc.SetActive(false);
            jdzc.SetActive(false);
            cityWAr.SetActive(false);
            if (room is PVPRoom)
            {
                jjc.SetActive(false);
                ar_result.SetActive(false);
                getGameObjectByPath("win").SetActive(false);
                getGameObjectByPath("state_successed").SetActive(false);
                bgdefet.SetActive(false);
                bgwin.SetActive(false);
                if (data.ContainsKey("win"))
                {

                    int cct = data["win"];
                    if (cct == 0)
                    {
                        jjc.SetActive(true);
                        jjc.transform.FindChild("vector").gameObject.SetActive(false);
                        jjc.transform.FindChild("defet").gameObject.SetActive(true);
                    }
                    else
                    {
                        jjc.SetActive(true);
                        jjc.transform.FindChild("vector").gameObject.SetActive(true);
                        jjc.transform.FindChild("defet").gameObject.SetActive(false);
                    }
                    int aa = a3_sportsModel.getInstance().grade;
                    if (aa < 10)
                        icon_star.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_00" + aa);
                    else
                        icon_star.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_rank_0" + aa);
                    if (a3_sportsModel.getInstance().grade <= 0)
                        return;
                    SXML Xml = XMLMgr.instance.GetSXML("jjc.reward", "grade==" + a3_sportsModel.getInstance().grade);
                    int pointCount = Xml.getInt("star");
                    if (pointCount <= 0)
                        return;
                    Transform star = this.transform.FindChild("jjc/star");
                    for (int i = 0; i < star.childCount; i++)
                    {
                        star.GetChild(i).FindChild("this").gameObject.SetActive(false);
                        star.GetChild(i).gameObject.SetActive(false);
                    }
                    for (int m = pointCount; m > 0; m--)
                    {
                        star.GetChild(m - 1).gameObject.SetActive(true);
                    }
                    for (int j = 0; j < a3_sportsModel .getInstance().score; j++)
                    {
                        star.GetChild(j).FindChild("this").gameObject.SetActive(true);
                    }
                }

            }

            if (room is JDZCRoom) {
                jjc.SetActive(false);
                ar_result.SetActive(false);
                jdzc.SetActive(false);
                cityWAr.SetActive(false);
                tip_text.SetActive(false);
                getGameObjectByPath("win").SetActive(false);
                getGameObjectByPath("state_successed").SetActive(false);
                bgdefet.SetActive(false);
                bgwin.SetActive(false);
                if (data.ContainsKey("win"))
                {
                    int cct = data["win"];
                    jdzc.SetActive(true);

                    if (cct == 0)
                    {
                        GameObject item = jdzc.transform.FindChild("fail/gift/reward").gameObject;
                        Transform con = jdzc.transform.FindChild("fail/gift/contain");
                        jdzc.transform.FindChild("win").gameObject.SetActive(false);
                        jdzc.transform.FindChild("fail").gameObject.SetActive(true);
                        SXML Xml_lose = XMLMgr.instance.GetSXML("pointarena.lose_reward");
                        List<SXML> rew = Xml_lose.GetNodeList("RewardItem");
                        foreach (SXML one in rew)
                        {
                            GameObject clon = Instantiate(item) as GameObject;
                            clon.SetActive(true);
                            clon.transform.SetParent(con,false);
                            uint id = (uint)one.getInt("item_id");
                            int num = one.getInt("value");
                            GameObject icon =  IconImageMgr.getInstance().createA3ItemIcon(id);
                            icon.transform.SetParent(clon.transform.FindChild("pic/icon"), false);
                            clon.transform.FindChild("pic/num").GetComponent<Text>().text = num.ToString();
                        }
                    }
                    else 
                    {
                        GameObject item = jdzc.transform.FindChild("win/gift/reward").gameObject;
                        Transform con = jdzc.transform.FindChild("win/gift/contain");
                        jdzc.transform.FindChild("win").gameObject.SetActive(true);
                        jdzc.transform.FindChild("fail").gameObject.SetActive(false);
                        SXML Xml_win = XMLMgr.instance.GetSXML("pointarena.win_reward");
                        List<SXML> rew = Xml_win.GetNodeList("RewardItem");
                        foreach (SXML one in rew)
                        {
                            GameObject clon = Instantiate(item) as GameObject;
                            clon.SetActive(true);
                            clon.transform.SetParent(con, false);
                            uint id = (uint)one.getInt("item_id");
                            int num = one.getInt("value");
                            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(id);
                            icon.transform.SetParent(clon.transform.FindChild("pic/icon"), false);
                            clon.transform.FindChild("pic/num").GetComponent<Text>().text = num.ToString();
                        }
                    }
                }
            }

            if (room is CityWarRoom ) {
                jjc.SetActive(false);
                ar_result.SetActive(false);
                jdzc.SetActive(false);
                tip_text.SetActive(false);
                cityWAr.SetActive(false);
                getGameObjectByPath("win").SetActive(false);
                getGameObjectByPath("state_successed").SetActive(false);
                bgdefet.SetActive(false);
                bgwin.SetActive(false);
                cityWAr.transform.FindChild("win/warinfo").gameObject.SetActive(false);
                if (data.ContainsKey("win"))
                {

                    int cct = data["win"];

                    cityWAr.SetActive(true);
                    if (cct == 0)
                    {
                        GameObject item = cityWAr.transform.FindChild("fail/gift/reward").gameObject;
                        Transform con = cityWAr.transform.FindChild("fail/gift/contain");
                        cityWAr.transform.FindChild("win").gameObject.SetActive(false);
                        cityWAr.transform.FindChild("fail").gameObject.SetActive(true);
                        if (PlayerModel.getInstance().lvlsideid == 1)
                        {//攻城
                            SXML Xml_win = XMLMgr.instance.GetSXML("clanwar.atk_awd", "min=="+ 0);
                            List<SXML> rew = Xml_win.GetNodeList("RewardItem");
                            List<SXML> awds_coin = Xml_win.GetNodeList("RewardValue");
                            setAwd(con, item, rew, awds_coin);
                        }
                        else
                        {
                            //守城        
                            SXML Xml_win = XMLMgr.instance.GetSXML("clanwar.def_awd", "min=="+ 0);
                            List<SXML> rew = Xml_win.GetNodeList("RewardItem");
                            List<SXML> awds_coin = Xml_win.GetNodeList("RewardValue");
                            setAwd(con, item, rew, awds_coin);
                        }
                    }

                    else {
                        GameObject item = cityWAr.transform.FindChild("win/gift/reward").gameObject;
                        Transform con = cityWAr.transform.FindChild("win/gift/contain");
                        cityWAr.transform.FindChild("win").gameObject.SetActive(true);
                        cityWAr.transform.FindChild("fail").gameObject.SetActive(false);
                        if (PlayerModel.getInstance().lvlsideid == 1)
                        {//攻城
                            int i = data["rank"];
                            if (data["win_clanid"])
                            {
                                cityWAr.transform.FindChild("win/warinfo").gameObject.SetActive(true);
                                if (A3_LegionModel .getInstance ().myLegion != null && A3_LegionModel.getInstance().myLegion.id == data["win_clanid"])
                                {
                                    cityWAr.transform.FindChild("win/warinfo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_11");
                                }else
                                    cityWAr.transform.FindChild("win/warinfo").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_12");
                            }
                            cityWAr.transform.FindChild("win/info").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_9", new List<string> { i.ToString() });
                            SXML Xml_win = XMLMgr.instance.GetSXML("clanwar");
                            List<SXML> awd = Xml_win.GetNodeList("atk_awd");
                            foreach (SXML s in awd)
                            {
                                if (s.getInt("min") <= i && s.getInt("max") >= i)
                                {
                                    List<SXML> rew = Xml_win.GetNodeList("RewardItem");
                                    List<SXML> awds_coin = Xml_win.GetNodeList("RewardValue");
                                    setAwd(con, item, rew, awds_coin);
                                    break;
                                }
                            }
                        }
                        else {
                            //守城
                            int i = data["hpper"];
                            cityWAr.transform.FindChild("win/info").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_fb_finish_10", new List<string> { i.ToString() });
                            SXML Xml_win = XMLMgr.instance.GetSXML ("clanwar");    
                            List<SXML> awd = Xml_win.GetNodeList("def_awd");
                            foreach (SXML s in awd) {
                                if (s.getInt ("min") <=i && s.getInt("max") >= i)
                                {
                                    List<SXML> rew = s.GetNodeList("RewardItem");
                                    List<SXML> awds_coin = s.GetNodeList("RewardValue");
                                    setAwd(con, item, rew, awds_coin);
                                    break;
                                }
                            }

                        }
                    }
                }

            }

        }

        void setAwd( Transform con, GameObject item, List<SXML> rew, List<SXML> awds_coin)
        {
            foreach (SXML one in rew)
            {
                GameObject clon = Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(con, false);
                uint id = (uint)one.getInt("item_id");
                int num = one.getInt("value");
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(id, scale:0.8f);
                icon.transform.SetParent(clon.transform.FindChild("pic/icon"), false);
                clon.transform.FindChild("pic/num").GetComponent<Text>().text = num.ToString();
            }

            foreach (SXML x in awds_coin)
            {
                GameObject clon = Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(con, false);
                a3_ItemData itemdata = new a3_ItemData();
                itemdata.borderfile = "icon_itemborder_b039_05";
                if (x.getInt("type") == 2)
                    itemdata.file = "icon_comm_0x2";
                else if (x.getInt("type") == 3)
                    itemdata.file = "icon_coin_coin2";
                else if (x.getInt("type") == 4)
                    itemdata.file = "icon_coin_coin3";
                else if (x.getInt("type") == 5)
                    itemdata.file = "icon_coin_coin4";
                else
                    itemdata.file = "icon_comm_0x2";
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata, false, -1, 0.8f);
                int num = x.getInt("value");
                icon.transform.SetParent(clon.transform.FindChild("pic/icon"), false);
                clon.transform.FindChild("pic/num").GetComponent<Text>().text = num.ToString();
            }
        }

        void clearAwd()
        {
            Transform con1 = cityWAr.transform.FindChild("fail/gift/contain");
            Transform con2 = cityWAr.transform.FindChild("win/gift/contain");
            Transform con3 = jdzc.transform.FindChild("fail/gift/contain");
            Transform con4 = jdzc.transform.FindChild("win/gift/contain");
            for (int i = 0; i < con1.childCount;i++) {
                Destroy(con1.GetChild (i).gameObject);
            }
            for (int i = 0; i < con2.childCount; i++)
            {
                Destroy(con2.GetChild(i).gameObject);
            }
            for (int i = 0; i < con3.childCount; i++)
            {
                Destroy(con3.GetChild(i).gameObject);
            }
            for (int i = 0; i < con4.childCount; i++)
            {
                Destroy(con4.GetChild(i).gameObject);
            }

        }


        //public void flyawd(int id, int num)
        //{
        //    BagProxy.getInstance().addEventListener(BagProxy.EVENT_ITEM_CHANGE, onItemChange);
        //    string str = "你获得了" + a3_BagModel.getInstance().getItemDataById((uint)limitedinmfo[id].item_id).item_name + "*" + limitedinmfo[id].buy_num;
        //    flytxt.instance.fly(str);
        //}
        public void destorycontain()
        {
            if (contain.transform.childCount > 0)
            {
                for (int i = 0; i < contain.transform.childCount; i++)
                {
                    Destroy(contain.transform.GetChild(i).gameObject);
                }
            }
        }

        void closeWindow()
        {

            for (int i = 0; i < this.transform.parent.childCount; i++)
            {
                if (this.transform.parent.GetChild(i).gameObject.activeSelf == true)
                {
                    pos_i.Add(i);
                    pos_i_name.Add(this.transform.parent.GetChild(i).gameObject.name);
                    if (this.transform.parent.GetChild(i).gameObject == this.gameObject)
                    {
                        continue;
                    }
                    this.transform.parent.GetChild(i).gameObject.SetActive(false);
                    //   GRMap.GAME_CAMERA.SetActive(true);
                }
            }
        }
        void openWindow()
        {
            if(pos_i_name.Count>0 && this.transform != null && transform.parent != null)
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                   if(pos_i_name.Contains(transform.parent.GetChild(i).gameObject.name))
                    {
                        if (transform.parent.GetChild(i).gameObject.activeSelf == false)
                        {
                            transform.parent.GetChild(i).gameObject.SetActive(true);

                        }
                    }
                }
            }


            //if (pos_i.Count > 0)
            //{

            //    for (int i = 0; i < pos_i.Count; i++)
            //    {
            //        if (this.transform != null && transform.parent != null)
            //        {
            //            if (pos_i[i] < transform.parent.childCount && transform.parent.GetChild(pos_i[i]) != null)
            //            {
            //                if (this.transform.parent.GetChild(pos_i[i]).gameObject.activeSelf == false)
            //                {
            //                    this.transform.parent.GetChild(pos_i[i]).gameObject.SetActive(true);

            //                }
            //            }
            //        }
            //    }
            //}



            pos_i_name.Clear();
            pos_i.Clear();
        }


        public override void onClosed()
        {
            //  InterfaceMgr.getInstance().floatUI.localScale = Vector3.one;
            destorycontain();
            openWindow();
            disposeAvatar();

            room = null;
            _NewOne = false;
            instance = null;
            clearAwd();
            if (closefb_way == true)
            {
                LevelProxy.getInstance().sendLeave_lvl();
            }
            if (GameObject.Find("GAME_CAMERA/myCamera"))
            {
                GameObject cameraOBJ = GameObject.Find("GAME_CAMERA/myCamera");
                if (cameraOBJ.GetComponent<DeathShader>())
                {
                    cameraOBJ.GetComponent<DeathShader>().enabled = false;
                }
            }
        }

        public void SetGetGold(int num)
        {
            getNum.text = (num + getnum).ToString();
        }

        void Update()
        {
            if (!_NewOne) return;
            double ct = ((double)closetime - (double)muNetCleint.instance.CurServerTimeStamp);

            closeTime.text = "<color=#ff0000>(" + (int)ct + ")</color>";
            // if (close_time != closetime-3) ;//ct -= 60;
            if (closefb_way == true)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);
            }


            if (ct < close_time)
            {
                _NewOne = false;
                if (closefb_way == false)
                {

                    a3_insideui_fb.instance.light_biu.gameObject.SetActive(true);
                    a3_insideui_fb.instance.exittime.gameObject.SetActive(true);
                    a3_insideui_fb.instance.close_time = (double)closetime;

                    // a3_insideui_fb.instance.close_time = 80 + (double)muNetCleint.instance.CurServerTimeStamp;
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_FB_FINISH);

            }
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        public void OnLvFinish(Variant data)
        {

        }
        public void OnAwd()
        {

        }
        public void itempic()
        {
            destorycontain();
            if (LevelProxy.getInstance().is_open == false)
            {
                if (LevelProxy.getInstance().reward != null)
                {
                    for (int i = 0; i < LevelProxy.getInstance().reward.Count; i++)
                    {
                        GameObject objClone = GameObject.Instantiate(reward) as GameObject;
                        objClone.SetActive(true);
                        objClone.transform.SetParent(contain.transform, false);
                        itempicc = objClone.transform.FindChild("pic").gameObject;


                       
                        int cc = LevelProxy.getInstance().reward[i].tpid;
                        // int d= LevelProxy.getInstance().reward[i].confdata.equip_level;
                        

                        a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)cc);
                        int color = item.quality;
                        if (item.equip_type != -1)
                        {
                            objClone.transform.FindChild("pic/bg").gameObject.SetActive(false);
                            objClone.transform.FindChild("pic/num").gameObject.SetActive(false);
                        }
                        else
                            objClone.transform.FindChild("pic/num").GetComponent<Text>().text = LevelProxy.getInstance().reward[i].cnt.ToString();
                        objClone.transform.FindChild("quality_bg/" + color).gameObject.SetActive(true);
                        itempicc.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

                    }
                }
            }
            if (LevelProxy.getInstance().is_open == true)
            {
                Dictionary<int, Rewards> rewardlist = new Dictionary<int, Rewards>();
                if (LevelProxy.getInstance().reward != null)
                {
                    foreach (Rewards it in LevelProxy.getInstance().reward) {
                        if (rewardlist.ContainsKey(it.tpid))
                        {
                            rewardlist[it.tpid].cnt += it.cnt;
                        }
                        else {
                            rewardlist[it.tpid] = it;
                        }
                    }
                    LevelProxy.getInstance().reward.Clear();
                }
                if (LevelProxy.getInstance().fbDrogward != null) {
                    foreach (Rewards it in LevelProxy.getInstance().fbDrogward)
                    {
                        if (rewardlist.ContainsKey(it.tpid))
                        {
                            rewardlist[it.tpid].cnt += it.cnt;
                        }
                        else
                        {
                            rewardlist[it.tpid] = it;
                        }
                    }
                    LevelProxy.getInstance().fbDrogward.Clear();
                }
                //if (BaseRoomItem.instance.list2.Count != 0)
                //{
                //    foreach (DropItemdta it in BaseRoomItem.instance.list2) {
                //        if (rewardlist.ContainsKey(it.tpid))
                //        {
                //            rewardlist[it.tpid].cnt += it.num;
                //        }
                //        else {
                //            Rewards ra = new Rewards();
                //            ra.tpid = it.tpid;
                //            ra.cnt = it.num;
                //            rewardlist[ra.tpid] = ra;
                //        }
                //    }
                //    BaseRoomItem.instance.list2.Clear();
                //}

                foreach (int id in rewardlist.Keys) {
                    GameObject objClone = GameObject.Instantiate(reward) as GameObject;
                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    itempicc = objClone.transform.FindChild("pic").gameObject;
                    itempicc.transform.FindChild("num").gameObject.SetActive(true);
                    itempicc.transform.FindChild("num").gameObject.GetComponent<Text>().text = rewardlist[id].cnt.ToString();
                    a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)id);
                    if (item.equip_type != -1)
                    {
                        objClone.transform.FindChild("pic/bg").gameObject.SetActive(false);
                        objClone.transform.FindChild("pic/num").gameObject.SetActive(false);
                    }
                    objClone.transform.FindChild("quality_bg/" + item.quality).gameObject.SetActive(true);
                    itempicc.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);
                }

                LevelProxy.getInstance().is_open = false;
            }
        }

        List<GameObject> objlist = new List<GameObject>();

        public void change_sprite()
        {
            bgdefet.SetActive(false);
            bgwin.SetActive(true);
            image_S.SetActive(false);
            image_B.SetActive(false);
            image_A.SetActive(false);
            getGameObjectByPath("win/success").SetActive(true);
            getGameObjectByPath("state_successed").SetActive(true);
        }

        public void evaluation(int score)
        {

            if (score == 3)
            {
                change_sprite();
                image_S.SetActive(true);
                itempic();


            }
            if (score == 2)
            {
                change_sprite();
                image_A.SetActive(true);
                itempic();

            }
            if (score == 1)
            {
                change_sprite();
                image_B.SetActive(true);
                itempic();

            }
            if (score == 0)
            {
                bgwin.SetActive(false);
                bgdefet.SetActive(true);
                getGameObjectByPath("win/success").SetActive(false);
                getGameObjectByPath("win/fail").SetActive(true);
                getGameObjectByPath("state_successed").SetActive(false);
            }

        }


        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(model.localPosition.x, model.localPosition.y, model.localPosition.z), model.localRotation) as GameObject;
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(model.localPosition.x, model.localPosition.y, model.localPosition.z), model.localRotation) as GameObject;
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(model.localPosition.x, model.localPosition.y, model.localPosition.z), model.localRotation) as GameObject;
                }
                else
                {
                    return;
                }

                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }

                Transform cur_model = m_SelfObj.transform.FindChild("model");

                //手上的小火球
                if (SelfRole._inst is P3Mage)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                m_proAvatar = new ProfessionAvatar();
                m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_FX, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
                m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
                m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
                m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
                m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
                m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());

                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
                m_Self_Camera = GameObject.Instantiate(obj_prefab) as GameObject;

                var lg = m_Self_Camera.GetComponentInChildren<Light>();
                if (lg != null)
                    lg.color = Color.white;
            }
        }
        // SelfRole._inst.onDead(true);
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (m_Self_Camera != null) GameObject.Destroy(m_Self_Camera);
        }
    }
}
