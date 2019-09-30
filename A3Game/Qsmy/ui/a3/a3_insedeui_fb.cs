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
    class a3_insideui_fb : FloatUi
    {
        public enum e_room
        {
            Normal,     //默认
            Exp,        //经验
            Money,      //金币
            Cailiao,    //材料
            MLZD,       //魔炼之地
            ZHSLY,      //召唤兽乐园
            PVP,        //竞技场
            WDSY,       //无底深渊
            DRAGON,     //军团屠龙
            TLFB109,    //109副本
            TLFB110,    //110副本
            TLFB111,    //111副本
            JDZC,       //据点战场
            CITYWAR,    //城战

        }
        e_room eroom = e_room.Normal;
        public Transform exp_tra;
        public Transform money_tra;
        public Transform cailiao_tra;
        public Transform dragon_tra;

        public Transform jdzc_tra;
        public Transform cityWar_tra;
        public Text dragon_txt;
        public Transform mlzd_tra;
        public Transform zhsly_tra;
        public Transform wdsy_tra;
        public Transform tlfb109_tra;
        public Transform tlfb110_tra;
        public Transform tlfb111_tra;
        public Transform pvp_tra;

        //public Transform zhanji;
        //Transform pvp_tra;
        Variant enterdata;              //进入数据
        Text normalExitTime;            //共用倒计时
        Text exitTime;                  //独立倒计时字
        Image exitTime_bar;             //独立倒计时条
        Text fin_exittime;           //90s倒计时

        public double close_time;
        float TotalSec;                 //副本总时间
        float blesstime = 0;            //祝福时间
        int addexp = 0;
        public int addmoney = 0;
        Transform fb_cast;              //副本广播
        Transform broad;                //副本广播父物体
        public Transform normal;
        public Transform btn_quit;
        public Transform exittime;
        public Transform light_biu;//指示光标
        TabControl tabCtrl1;
        TabControl tabCtrl109;
        TabControl tabCtrl110;
        TabControl tabCtrl111;
        TabControl tabCtrlzhs;
        Transform teamPanel;
        Transform teamPanel109;
        Transform teamPanel110;
        Transform teamPanel111;
        Transform teamPanelzhs;
        public static a3_insideui_fb instance;

        public bool close_pic;
        bool close_jl_pic;
        public GameObject enter_pic1;
        public GameObject enter_pic2;
        public GameObject enter_jl_pic2;
        public GameObject enter_jl_pic3;
        Transform bg1;
        Transform bg2;
        GameObject pic_icon;
        GameObject pic_icon1;
        Text text1;
        Text text2;
        GameObject pic_icon_jl;
        GameObject pic_icon1_jl;
        Text text1_jl;
        Text text2_jl;
        Variant data;
        Variant data109;
        Variant data110;
        Variant data111;
        Text text11;
        Text text21;
        Text text11_jl;
        Text text21_jl;

        Text MyRecord_kill;
        Text MyRecord_die;
        Text MyRecord_helpkill;

        Text jdzc_time;
        Text cityWar_time;
        public int doors = 0;
        public int needkill = 0;

        public bool fb_bgset;
        public static void ShowInUI(e_room room)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_INSIDEUI_FB, new ArrayList { room });
        }

        public static void CloseInUI()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_INSIDEUI_FB);

        }



        //Old Code
        public static BaseRoomItem room;
        Text closeTime;
        //关卡开始时刻
        public static double begintime;
        //关卡结束时刻
        double endtime;
        //关卡完成时刻
        double closetime;

        public override void init()
        {
            inText();
            enter_pic1 = transform.FindChild("enter_pic").gameObject;
            enter_pic2 = transform.FindChild("enter_pic1").gameObject;
            bg1 = enter_pic1.transform.FindChild("bg1");
            pic_icon = enter_pic1.transform.FindChild("ar_result/icon").gameObject;
            pic_icon1 = enter_pic2.transform.FindChild("ar_result/icon").gameObject;
            text1 = enter_pic1.transform.FindChild("ar_result/Text1").GetComponent<Text>();
            text2 = enter_pic1.transform.FindChild("ar_result/Text2").GetComponent<Text>();

            text11 = enter_pic2.transform.FindChild("ar_result/Text1").GetComponent<Text>();
            text21 = enter_pic2.transform.FindChild("ar_result/Text2").GetComponent<Text>();

            enter_jl_pic2 = transform.FindChild("enter_jl_pic2").gameObject;
            enter_jl_pic3 = transform.FindChild("enter_jl_pic3").gameObject;
            bg2 = enter_jl_pic2.transform.FindChild("bg1");
            pic_icon_jl = enter_jl_pic2.transform.FindChild("ar_result/icon").gameObject;
            pic_icon1_jl = enter_jl_pic3.transform.FindChild("ar_result/icon").gameObject;
            text1_jl = enter_jl_pic2.transform.FindChild("ar_result/Text1").GetComponent<Text>();
            text2_jl = enter_jl_pic2.transform.FindChild("ar_result/Text2").GetComponent<Text>();

            text11_jl = enter_jl_pic3.transform.FindChild("ar_result/Text1").GetComponent<Text>();
            text21_jl = enter_jl_pic3.transform.FindChild("ar_result/Text2").GetComponent<Text>();

            light_biu = getTransformByPath("guide_task_info");
            exittime = getTransformByPath("time");
            fin_exittime = transform.FindChild("time/Text").GetComponent<Text>();
            btn_quit = getTransformByPath("btn_quit");
            normal = getTransformByPath("normal");
            broad = getTransformByPath("normal/broadcast/broad");
            fb_cast = getTransformByPath("normal/broadcast/cast");
            Transform info = transform.FindChild("normal/info");
            exp_tra = transform.FindChild("exp");
            money_tra = transform.FindChild("money");
            cailiao_tra = transform.FindChild("cailiao");
            mlzd_tra = transform.FindChild("mlzd");
            zhsly_tra = transform.FindChild("zhsly");
            wdsy_tra = transform.FindChild("wdsy");
            tlfb109_tra= transform.FindChild("tlfb109");
            tlfb110_tra = transform.FindChild("tlfb110");
            tlfb111_tra = transform.FindChild("tlfb111");
            pvp_tra = transform.FindChild("pvp");
            dragon_tra = transform.FindChild("dragon");
            dragon_txt = dragon_tra.FindChild("info/info_desc/Text").GetComponent<Text>();
        

            jdzc_tra = transform.FindChild("jdzc");
            cityWar_tra = transform.FindChild("citywar");
            //zhanji = jdzc_tra.FindChild("playerinfo");
            jdzc_time = jdzc_tra.FindChild("gameinfo/time").GetComponent<Text>();
            cityWar_time = cityWar_tra.FindChild ("gameinfo/time/tm").GetComponent<Text>();
            enter_pic1.SetActive(false);
            enter_jl_pic2.SetActive(false);
            fb_bgset = false;
            enter_pic2.SetActive(false);
            normal.gameObject.SetActive(true);
            btn_quit.gameObject.SetActive(false);
            exittime.gameObject.SetActive(false);
            light_biu.gameObject.SetActive(false);
            transform.FindChild("btn").gameObject.SetActive(false);
            data = SvrLevelConfig.instacne.get_level_data(108);
            data109 = SvrLevelConfig.instacne.get_level_data(109);
            data110 = SvrLevelConfig.instacne.get_level_data(110);
            data111 = SvrLevelConfig.instacne.get_level_data(111);

            MyRecord_kill = transform.FindChild("jdzc/zhanji/info_kill").GetComponent<Text>();
            MyRecord_die = transform.FindChild("jdzc/zhanji/info_die").GetComponent<Text>();
            MyRecord_helpkill = transform.FindChild("jdzc/zhanji/info_helpkill").GetComponent<Text>();
            //new BaseButton(pvp_tra.FindChild("time/tach")).onClick = (GameObject go) => {
            //    flytxt.instance.fly(ContMgr.getCont("a3_insideui_fb_wait"));
            //};

            new BaseButton(jdzc_tra.FindChild("zhanji")).onClick = look_zhanji;
            //new BaseButton(jdzc_tra.FindChild("playerinfo/close")).onClick = close_zhanli;
           

            new BaseButton(bg1).onClick = (GameObject go) =>
            {
                enter_pic1.SetActive(false);
                fb_bgset = false;
                enter_pic2.SetActive(true);
                transform.FindChild("btn").gameObject.SetActive(false);
                transform.FindChild("normal").gameObject.SetActive(true);
                sett_f(true);
            };
            new BaseButton(bg2).onClick = (GameObject go) =>
            {
                enter_jl_pic2.SetActive(false);
                fb_bgset = false;
                enter_jl_pic3.SetActive(true);
                transform.FindChild("btn").gameObject.SetActive(false);
                transform.FindChild("normal").gameObject.SetActive(true);
                sett_f(true);
            };
            new BaseButton(transform.FindChild("btn")).onClick = (GameObject go) =>
            {
                if (close_pic)
                {
                    enter_pic1.SetActive(false);
                    fb_bgset = false;
                    enter_pic2.SetActive(true);
                    close_pic = false;
                }
                else if (close_jl_pic)
                {
                    enter_jl_pic2.SetActive(false);
                    fb_bgset = false;
                    enter_jl_pic3.SetActive(true);
                    close_jl_pic = false;
                }
                transform.FindChild("btn").gameObject.SetActive(false);
                transform.FindChild("normal").gameObject.SetActive(true);
                sett_f(true);
            };


            //new BaseButton(jdzc_tra.FindChild("playerinfo/close_fb")).onClick =
            new BaseButton (cityWar_tra.transform .FindChild ("back")).onClick =
            new BaseButton(transform.FindChild("normal/btn_quitfb")).onClick = outfb;

            new BaseButton(btn_quit).onClick = (GameObject go) =>
            {
                LevelProxy.getInstance().sendLeave_lvl();
                if (TeamProxy.getInstance().MyTeamData != null)
                {
                    a3_liteMinimap.instance.getGameObjectByPath("taskinfo/bar").SetActive(false);
                    TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().cid);
                    if (TeamProxy.getInstance().hasEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES))
                    {
                        TeamProxy.getInstance().removeEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, WdsyOpenDoor.instance.killNum);
                    }
                    doors = 0; needkill = 0;
                }
            };


            new BaseButton(this.transform.FindChild("zhsly/info/jiangli")).onClick = (GameObject go) => 
            {
                //召唤兽乐园领取奖励
                LevelProxy.getInstance().getAwd_zhs(2);
            };
            new BaseButton(this.transform.FindChild("zhsly/tishi/close")).onClick = (GameObject go) => {
                this.transform.FindChild("zhsly/tishi").gameObject.SetActive(false);
            };

            normalExitTime = transform.FindChild("normal/btn_quitfb/Text").GetComponent<Text>();

            foreach (var v in GetComponentsInChildren<Transform>(true))
            {
                if (v.name == "btn_blessing")
                {
                    new BaseButton(v).onClick = (GameObject go) =>
                    {
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BLESSING);
                        //if (blesstime == 0)
                        //{
                        //    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BLESSING);
                        //}
                        //else
                        //{
                        //    flytxt.instance.fly(ContMgr.getCont("a3_insideui_fb_gw"));
                        //}
                    };
                }
                if (v.name == "blesson") v.gameObject.SetActive(false);
            }
            //TeamProxy.getInstance().addEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, killNum);
            teamPanel = transform.FindChild("wdsy/team");
            tabCtrl1 = new TabControl();
            tabCtrl1.onClickHanle = onTab108;
            tabCtrl1.create(this.getGameObjectByPath("wdsy/title/panelTab1"), this.gameObject, 0, 0, true);
            teamPanel109 = transform.FindChild("tlfb109/team");
            tabCtrl109 = new TabControl();
            tabCtrl109.onClickHanle = onTab109;
            tabCtrl109.create(this.getGameObjectByPath("tlfb109/title/panelTab1"), this.gameObject, 0, 0, true);
            teamPanel110 = transform.FindChild("tlfb110/team");
            tabCtrl110 = new TabControl();
            tabCtrl110.onClickHanle = onTab110;
            tabCtrl110.create(this.getGameObjectByPath("tlfb110/title/panelTab1"), this.gameObject, 0, 0, true);
            teamPanel111 = transform.FindChild("tlfb111/team");
            tabCtrl111 = new TabControl();  
            tabCtrl111.onClickHanle = onTab111;
            tabCtrl111.create(this.getGameObjectByPath("tlfb111/title/panelTab1"), this.gameObject, 0, 0, true);

            teamPanelzhs = transform.FindChild("zhsly/team");
            tabCtrlzhs = new TabControl();
            tabCtrlzhs.onClickHanle = onTabzhs;
            tabCtrlzhs.create(this.getGameObjectByPath("zhsly/title/panelTab1"), this.gameObject, 0, 0, true);
            new BaseButton(this.transform.FindChild("exp/btn_double")).onClick = (GameObject go) =>
            {
                if (this.transform.FindChild("exp/btn_double/blesson").gameObject .activeSelf ) {
                    flytxt.instance.fly(ContMgr .getCont ("have_doubleExp"));
                }
                else 
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_DOUBLEEXP);
            };

        }


        void inText() {
            this.transform.FindChild("exp/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("money/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("cailiao/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("mlzd/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("zhsly/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("wdsy/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("tlfb109/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("tlfb110/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("tlfb111/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息
            this.transform.FindChild("dragon/info_title/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_1");//副本信息

            this.transform.FindChild("wdsy/title/panelTab1/kill/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_2");//击杀
            this.transform.FindChild("wdsy/title/panelTab1/team/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_3");//队伍

            this.transform.FindChild("tlfb109/title/panelTab1/kill/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_2");//击杀
            this.transform.FindChild("tlfb109/title/panelTab1/team/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_3");//队伍

            this.transform.FindChild("tlfb110/title/panelTab1/kill/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_2");//击杀
            this.transform.FindChild("tlfb110/title/panelTab1/team/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_3");//队伍

            this.transform.FindChild("tlfb111/title/panelTab1/kill/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_2");//击杀
            this.transform.FindChild("tlfb111/title/panelTab1/team/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_3");//队伍
            this.transform.FindChild("jdzc/kill_new/info/kill").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_4");//击杀

            this.transform.FindChild("enter_pic/ar_result/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_10");//你正在进行一项神王的挑战，成功后你将会获得一个新的技能
            this.transform.FindChild("enter_pic1/ar_result/info").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_11");//通关后将会获得一个新的技能
            this.transform.FindChild("enter_jl_pic2/ar_result/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_12");//你正在进行一项精灵试炼，成功后你将会解锁一个新的符文
            this.transform.FindChild("enter_jl_pic3/ar_result/info").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_13");//通关后将会解锁一个新的符文

            this.transform.FindChild("exp/info/info_2*exp/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_14");//經驗捲軸未使用
            this.transform.FindChild("exp/info/info_bless/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_15");//傷害鼓舞未使用

            this.transform.FindChild("citywar/gameinfo/time/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_16");//距离攻城结束
            this.transform.FindChild("citywar/gameinfo/playerNum/attack").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_17");//进攻方：
            this.transform.FindChild("citywar/gameinfo/playerNum/Defense").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_insideui_fb_18");//防守方：
        }

        void onTab108(TabControl t)
        {
            if (t.getSeletedIndex() == 0)
            {
                getGameObjectByPath("wdsy/info_title").SetActive(true);
                getGameObjectByPath("wdsy/info").SetActive(true);
                getGameObjectByPath("wdsy/icon").SetActive(true);
                getGameObjectByPath("wdsy/team").SetActive(false);
            }
            else
            {
                ArrayList array = new ArrayList();
                array.Add(teamPanel);
                getGameObjectByPath("wdsy/info_title").SetActive(false);
                getGameObjectByPath("wdsy/info").SetActive(false);
                getGameObjectByPath("wdsy/icon").SetActive(false);
                getGameObjectByPath("wdsy/team").SetActive(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            }
        }
        void onTab109(TabControl t)
        {
            if (t.getSeletedIndex() == 0)
            {
                getGameObjectByPath("tlfb109/info_title").SetActive(true);
                getGameObjectByPath("tlfb109/info").SetActive(true);
                getGameObjectByPath("tlfb109/icon").SetActive(true);
                getGameObjectByPath("tlfb109/team").SetActive(false);
            }
            else
            {
                ArrayList array = new ArrayList();
                array.Add(teamPanel109);
                getGameObjectByPath("tlfb109/info_title").SetActive(false);
                getGameObjectByPath("tlfb109/info").SetActive(false);
                getGameObjectByPath("tlfb109/icon").SetActive(false);
                getGameObjectByPath("tlfb109/team").SetActive(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            }
        }
        void onTab110(TabControl t)
        {
            if (t.getSeletedIndex() == 0)
            {
                getGameObjectByPath("tlfb110/info_title").SetActive(true);
                getGameObjectByPath("tlfb110/info").SetActive(true);
                getGameObjectByPath("tlfb110/icon").SetActive(true);
                getGameObjectByPath("tlfb110/team").SetActive(false);
            }
            else
            {
                ArrayList array = new ArrayList();
                array.Add(teamPanel110);
                getGameObjectByPath("tlfb110/info_title").SetActive(false);
                getGameObjectByPath("tlfb110/info").SetActive(false);
                getGameObjectByPath("tlfb110/icon").SetActive(false);
                getGameObjectByPath("tlfb110/team").SetActive(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            }
        }
        void onTab111(TabControl t)
        {
            if (t.getSeletedIndex() == 0)
            {
                getGameObjectByPath("tlfb111/info_title").SetActive(true);
                getGameObjectByPath("tlfb111/info").SetActive(true);
                getGameObjectByPath("tlfb111/icon").SetActive(true);
                getGameObjectByPath("tlfb111/team").SetActive(false);
            }
            else
            {
                ArrayList array = new ArrayList();
                array.Add(teamPanel111);
                getGameObjectByPath("tlfb111/info_title").SetActive(false);
                getGameObjectByPath("tlfb111/info").SetActive(false);
                getGameObjectByPath("tlfb111/icon").SetActive(false);
                getGameObjectByPath("tlfb111/team").SetActive(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            }
        }

        void onTabzhs(TabControl t) {
            if (t.getSeletedIndex() == 0)
            {
                getGameObjectByPath("zhsly/info_title").SetActive(true);
                getGameObjectByPath("zhsly/info").SetActive(true);
                getGameObjectByPath("zhsly/team").SetActive(false);
            }
            else
            {
                ArrayList array = new ArrayList();
                array.Add(teamPanelzhs);
                getGameObjectByPath("zhsly/info_title").SetActive(false);
                getGameObjectByPath("zhsly/info").SetActive(false);
                getGameObjectByPath("zhsly/team").SetActive(true);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CURRENTTEAMINFO, array);
            }
        }
        public override void onShowed()
        {
            //初始化
            instance = this;
            enterdata = muLGClient.instance.g_levelsCT.get_curr_lvl_info();
            if (enterdata == null || !enterdata.ContainsKey("end_tm")) throw new Exception(ContMgr.getCont("a3_insideui_fb_sbgofb"));
            if (enterdata != null)
            {
                endtime = enterdata["end_tm"];
                // close_time = endtime + 100;
            }
            enter_jl_pic3.SetActive(false);
            enter_pic2.SetActive(false);


            this.transform.FindChild("zhsly/tishi").gameObject.SetActive(false);
            transform.FindChild("normal/btn_quitfb").gameObject.SetActive(true);
            normal.gameObject.SetActive(true);
            exp_tra.gameObject.SetActive(false);
            money_tra.gameObject.SetActive(false);
            cailiao_tra.gameObject.SetActive(false);
            mlzd_tra.gameObject.SetActive(false);
            zhsly_tra.gameObject.SetActive(false);
            wdsy_tra.gameObject.SetActive(false);
            tlfb109_tra.gameObject.SetActive(false);
            tlfb110_tra.gameObject.SetActive(false);
            tlfb111_tra.gameObject.SetActive(false);
            btn_quit.gameObject.SetActive(false);
            exittime.gameObject.SetActive(false);
            light_biu.gameObject.SetActive(false);
            pvp_tra.gameObject.SetActive(false);
            dragon_tra.gameObject.SetActive(false);
            pvp_tra.FindChild("stear").gameObject.SetActive(false);
            pvp_tra.FindChild("time").gameObject.SetActive(false);
            jdzc_tra.FindChild("stear").gameObject.SetActive(false);
            jdzc_tra.FindChild("time").gameObject.SetActive(false);
            jdzc_tra.gameObject.SetActive(false);
            cityWar_tra.gameObject.SetActive(false);
            jdzc_tra.FindChild("kill_new").gameObject.SetActive(false);
            cityWar_tra.FindChild("kill_new").gameObject.SetActive(false);
            eroom = e_room.Normal;
            if (uiData == null || uiData.Count < 1) throw new Exception(ContMgr.getCont("a3_insideui_fb_txt"));
            eroom = (e_room)uiData[0];
            if (MapModel.getInstance().curLevelId == 108)
                readLevel(0,data);
            if (MapModel.getInstance().curLevelId == 109)
                readLevel(0, data109);
            if (MapModel.getInstance().curLevelId == 110)
                readLevel(0, data110);
            if (MapModel.getInstance().curLevelId == 111)
                readLevel(0, data111);
            //close_zhanli(null);
            SetKillNum(0);
            //SetInfKill(0,101);
            //显示对应房间界面
            switch (eroom)
            {
                case e_room.Exp:
                    SetInfKill(0, 101);
                    exp_tra.gameObject.SetActive(true);
                    break;
                case e_room.Money:
                    SetInfKill(0, 102);
                    money_tra.gameObject.SetActive(true);
                    break;
                case e_room.Cailiao:
                    SetInfKill(0, 103);
                    cailiao_tra.gameObject.SetActive(true);
                    break;
                case e_room.MLZD:
                    SetKillNum(0,10);
                    mlzd_tra.gameObject.SetActive(true);
                    break;
                case e_room.ZHSLY:
                    zhsly_tra.gameObject.SetActive(true);
                    ZHSLY_info(LevelProxy.getInstance().count, LevelProxy.getInstance().cishu);
                    break;
                case e_room.PVP:
                    pvp_tra.gameObject.SetActive(true);
                    waitTime();
                    break;
                case e_room.WDSY:
                    wdsy_tra.gameObject.SetActive(true);
                    break;
                case e_room.TLFB109:
                    tlfb109_tra.gameObject.SetActive(true);
                    break;
                case e_room.TLFB110:
                    tlfb110_tra.gameObject.SetActive(true);
                    break;
                case e_room.TLFB111:
                    tlfb111_tra.gameObject.SetActive(true);
                    show111info();
                    break;
                case e_room.DRAGON:
                    dragon_tra.gameObject.SetActive(true);
                    break;
                case e_room.JDZC:
                    normal.gameObject.SetActive(false);
                    jdzc_tra.gameObject.SetActive(true);
                    waitTime_jdzc();
                    JDZC_setMyRecord();
                    CancelInvoke("Jdzc_GameTime");
                    InvokeRepeating("Jdzc_GameTime", 0, 1);
                    break;
                case e_room.CITYWAR:
                    SetFbInfo();
                    normal.gameObject.SetActive(false);
                    cityWar_tra.gameObject.SetActive(true);
                    CancelInvoke("citywar_GameTime");
                    InvokeRepeating("citywar_GameTime", 0, 1);
                    break;
            }
            //设置副本倒计时

            if (GetNowTran() != null && eroom !=  e_room.JDZC && eroom!= e_room.CITYWAR)
            {
                exitTime = GetNowTran().FindChild("info/info_time/time_text").GetComponent<Text>();
                exitTime_bar = GetNowTran().FindChild("info/info_time/time_bar").GetComponent<Image>();
            }
            //精灵副本倒计时
            if (LevelProxy.getInstance().open_pic1 == true)
            {
                exitTime =getTransformByPath("enter_jl_pic3/info/info_time/time_text").GetComponent<Text>();
                exitTime_bar = getTransformByPath("enter_jl_pic3/info/info_time/time_bar").GetComponent<Image>();
            }
            //神王挑战
            if (LevelProxy.getInstance().open_pic == true)
            {
                exitTime = getTransformByPath("enter_pic1/info/info_time/time_text").GetComponent<Text>();
                exitTime_bar = getTransformByPath("enter_pic1/info/info_time/time_bar").GetComponent<Image>();
            }
            normalExitTime.text = ContMgr.getCont("a3_insideui_nostart");
            if (exitTime != null) exitTime.text = ContMgr.getCont("a3_insideui_nostart");
            if (exitTime_bar != null) exitTime_bar.fillAmount = 1;
            RefreshExitTime();
            TotalSec = (int)Mathf.Max((int)((double)endtime - (double)muNetCleint.instance.CurServerTimeStamp), 0);
            CancelInvoke("RefreshExitTime");
            if (TotalSec > 0)
            {
                InvokeRepeating("RefreshExitTime", 0, 1);
            }
            //信息初始数据
            addexp = 0;
            addmoney = 0;
            blesstime = 0;
          
            SetInfExp(0);
            SetInfBo(0);
           
            SetInfMoney(0);
            Using_jc();
            if (enterdata.ContainsKey("energy")) SetInfBaozang(enterdata["energy"]);
            else SetInfBaozang(0);
            SetInfBoss(0);
            //事件
            A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_ONBLESS, OnBless);
            //置底
            this.transform.SetAsFirstSibling();


            if (LevelProxy.getInstance().open_pic == true)
            {
                enter_pic1.SetActive(true);
                fb_bgset = true;
                transform.FindChild("btn").gameObject.SetActive(true);
                if (PlayerModel.getInstance().profession == 2)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[0]);
                if (PlayerModel.getInstance().profession == 3)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[1]);
                if (PlayerModel.getInstance().profession == 5)
                    pic_icon.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[2]);
                enter_pic();
                close_pic = true;
                enter_pic2_show();
                a3_liteMinimap.instance.taskinfo.SetActive(false);

                transform.FindChild("normal").gameObject.SetActive(false);

                sett_f(false);
                LevelProxy.getInstance().open_pic = false;
            }
            if (LevelProxy.getInstance().open_pic1 == true)
            {
                enter_jl_pic2.SetActive(true);
                fb_bgset = true;
                transform.FindChild("btn").gameObject.SetActive(true);
                if (PlayerModel.getInstance().profession == 2)
                    pic_icon_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[0]);
                if (PlayerModel.getInstance().profession == 3)
                    pic_icon_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[1]);
                if (PlayerModel.getInstance().profession == 5)
                    pic_icon_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[2]);
                enter_pic_jl();
                close_jl_pic = true;
                enter_pic2_jl_show();
                a3_liteMinimap.instance.taskinfo.SetActive(false); 

                transform.FindChild("normal").gameObject.SetActive(false);

                sett_f(false);
                LevelProxy.getInstance().open_pic1 = false;
            }
        }
        void show111info()
        {
            GameObject idtext = GetNowTran().FindChild("info/info_killnums/mid").gameObject;
           var d=  SvrLevelConfig.instacne.get_level_data(MapModel.getInstance().curLevelId);
            int ids = -1;
            foreach (var a in d["fin_check"]._arr)
            {
                foreach (var v in a["km"]._arr)
                {
                    ids = v["mid"];
                   
                }
            }
             string name= XMLMgr.instance.GetSXML("monsters.monsters", "id==" + ids).getString("name");
            if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_1", name);
            GameObject tns = GetNowTran().FindChild("info/info_killnums/Text").gameObject;//队伍击杀

            if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", "1");
        }

        public void JDZC_setMyRecord() {
            MyRecord_kill.text = a3_sportsModel.getInstance().kill_count.ToString (); // + "/" + a3_sportsModel.getInstance().die_count + "/" + a3_sportsModel.getInstance().helpkill_count;
            MyRecord_die.text = a3_sportsModel.getInstance().die_count.ToString();
            MyRecord_helpkill.text = a3_sportsModel.getInstance().helpkill_count.ToString ();

        }
        void Jdzc_GameTime() {
            int tm;
            tm = (int)(endtime - muNetCleint.instance.CurServerTimeStamp);
            if (tm >= 0)
            {
                string time = string.Format("{0:D2}:{1:D2}", tm / 60, tm % 60);
                jdzc_time.text = time;
            }
        }

        void citywar_GameTime()
        {
            int tm;
            tm = (int)(A3_cityOfWarModel.getInstance().endTime - muNetCleint.instance.CurServerTimeStamp);
            string time = string.Format("{0:D2}:{1:D2}", tm / 60, tm % 60);
            cityWar_time.text = time;

        }
        

        void look_zhanji(GameObject go) {
            a3_sportsProxy.getInstance().getAll_info();
            open_zhanji = true;
        }


        //void close_zhanli(GameObject go) {
        //    zhanji.gameObject.SetActive(false);
        //    Transform Con = zhanji.FindChild("scroll_rect/contain");
        //    if (Con.childCount > 0) {
        //        for (int i = 0; i< Con.childCount;i++) {
        //            Destroy(Con.GetChild(i).gameObject);
        //        }
        //    }
        //}

        public bool open_zhanji = false;
        public void openzhanji( Dictionary <int, info_teamPlayer>  data) {
            //zhanji.gameObject.SetActive(true);
            //Transform Con = zhanji.FindChild("scroll_rect/contain");
            //if (Con.childCount > 0)
            //{
            //    for (int i = 0; i < Con.childCount; i++)
            //    {
            //        Destroy(Con.GetChild(i).gameObject);
            //    }
            //}
            //GameObject one = zhanji.FindChild("scroll_rect/item_zhanli").gameObject;
            //foreach (int id in data.Keys) {
            //    GameObject clon = Instantiate(one) as GameObject ;
            //    clon.SetActive(true);
            //    clon.transform.SetParent(Con,false);
            //    clon.transform.FindChild("carricon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_" + data[id].carr);
            //    clon.transform.FindChild("name").GetComponent<Text>().text = data[id].name;
            //    clon.transform.FindChild("lvl").GetComponent <Text>().text = ContMgr.getCont("worldmap_lv", new List<string> { data[id].zhuan.ToString(), data[id].lvl.ToString() });
            //    clon.transform.FindChild("zhanji").GetComponent<Text>().text = data[id].kill_cnt + "/" + data[id].die_cnt + "/" + data[id].assists_cnt;
            //    clon.transform.FindChild("dmg").GetComponent<Text>().text = data[id].dmg.ToString();
            //    clon.transform.FindChild("getpiont").GetComponent<Text>().text = data[id].ach_point.ToString();
            //}
            ArrayList array = new ArrayList();
            array.Add(data);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr .A3_JDZC_ZHANJI, array);
            open_zhanji = false;
            JDZC_setMyRecord();
        }

        public void  outfb(GameObject go) {
            if (a3_buff.instance.bless_ad) a3_buff.instance.bless_ad = false;
            uint cueLevelId = MapModel.getInstance().curLevelId;
            string str = string.Empty;
            switch (cueLevelId)
            {
                case 101: str = ContMgr.getCont("fb_quit_hint_13"); break;
                case 102: str = ContMgr.getCont("fb_quit_hint_8"); break;
                case 103: str = ContMgr.getCont("fb_quit_hint_12"); break;
                case 104: str = ContMgr.getCont("fb_quit_hint_10"); break;
                case 105: str = ContMgr.getCont("fb_quit_hint_11"); break;
                case 106: str = ContMgr.getCont("fb_quit_hint_14"); break;
                case 107: str = ContMgr.getCont("fb_quit_hint_5"); break;
                case 108:
                case 109:
                case 110:
                case 111: str = ContMgr.getCont("fb_quit_hint_2"); break;

                default: str = ContMgr.getCont("fb_quit_hint_1"); break;
            }

            MsgBoxMgr.getInstance().showConfirm(str, () =>
            {
                LevelProxy.getInstance().sendLeave_lvl();
                if (enter_pic2.activeInHierarchy == true) enter_pic2.gameObject.SetActive(false);
                if (TeamProxy.getInstance().MyTeamData != null)
                {
                    TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().cid);
                    if (TeamProxy.getInstance().hasEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES))
                    {
                        TeamProxy.getInstance().removeEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, WdsyOpenDoor.instance.killNum);
                    }
                    doors = 0;
                    needkill = 0;
                    a3_liteMinimap.instance.getGameObjectByPath("taskinfo/bar").SetActive(false);
                }

            });
        }

        public int wait_time = 0;

        void waitTime_jdzc() {
            wait_time = 10;
            wait_time -= (int)(muNetCleint.instance.CurServerTimeStamp - LevelProxy.getInstance().starTime);
            if (wait_time > 0)
            {
                SelfRole._inst.can_buff_move = false;
                SelfRole._inst.can_buff_skill = false;
                SelfRole._inst.can_buff_ani = false;
                if (a1_gamejoy.inst_joystick)
                {
                    a1_gamejoy.skill_wait = true;
                }
                CancelInvoke("JdzcTimeGo");
                InvokeRepeating("JdzcTimeGo", 0, 1);
            }
        }
        void JdzcTimeGo()
        {
            if (wait_time <= -1)
            {
                jdzc_tra.FindChild("stear").gameObject.SetActive(false);
                jdzc_tra.FindChild("time").gameObject.SetActive(false);
                //SelfRole.fsm.StartAutofight();
                //SelfRole.fsm.ChangeState(StateAttack.Instance);
                CancelInvoke("JdzcTimeGo");
                return;
            }
            if (wait_time <= 0)
            {
                SelfRole._inst.can_buff_move = true;
                SelfRole._inst.can_buff_skill = true;
                SelfRole._inst.can_buff_ani = true;
                if (a1_gamejoy.inst_joystick)
                {
                    a1_gamejoy.skill_wait = false;
                }
                jdzc_tra.FindChild("stear").gameObject.SetActive(true);
                jdzc_tra.FindChild("time").gameObject.SetActive(false);
                if (!SelfRole._inst.isDead)
                {
                    MoveProxy.getInstance().sendVec();
                }
            }
            else if (wait_time <= 9)
            {
                jdzc_tra.FindChild("stear").gameObject.SetActive(false);
                jdzc_tra.FindChild("time").gameObject.SetActive(true);
                //pvp_tra.FindChild("time").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_countdown_countdown" + wait_time);
                jdzc_tra.FindChild("time").GetComponent<Text>().text = wait_time.ToString();
            }
            else if (wait_time > 9)
            {
                jdzc_tra.FindChild("stear").gameObject.SetActive(false);
                jdzc_tra.FindChild("time").gameObject.SetActive(false);
            }
            wait_time--;
        }

        void waitTime()
        {
            wait_time = 10;
            wait_time -= (int)(muNetCleint.instance.CurServerTimeStamp - LevelProxy.getInstance().starTime);
            if (wait_time > 0)
            {
                SelfRole._inst.can_buff_move = false;
                SelfRole._inst.can_buff_skill = false;
                SelfRole._inst.can_buff_ani = false;
                if (a1_gamejoy.inst_joystick)
                {
                    a1_gamejoy.skill_wait = true;
                }
                CancelInvoke("pvpTimeGo");
                InvokeRepeating("pvpTimeGo", 0, 1);
            }
        }

        public void  Cancel() {

            if (wait_time <= 0) { return; }
            wait_time = -1;
            CancelInvoke("pvpTimeGo");
            pvp_tra.gameObject.SetActive(false);
            SelfRole.fsm.StartAutofight();
            SelfRole.fsm.ChangeState(StateAttack.Instance);
            SelfRole._inst.can_buff_move = true;
            SelfRole._inst.can_buff_skill = true;
            SelfRole._inst.can_buff_ani = true;
            if (a1_gamejoy.inst_joystick)
            {
                a1_gamejoy.skill_wait = false;
            }
        }

        void pvpTimeGo()
        {
            if (wait_time <= -1)
            {
                pvp_tra.gameObject.SetActive(false);
                SelfRole.fsm.StartAutofight();
                SelfRole.fsm.ChangeState(StateAttack.Instance);
                CancelInvoke("pvpTimeGo");
                return;
            }
            if (wait_time <= 0)
            {
                SelfRole._inst.can_buff_move = true;
                SelfRole._inst.can_buff_skill = true;
                SelfRole._inst.can_buff_ani = true;
                if (a1_gamejoy.inst_joystick)
                {
                    a1_gamejoy.skill_wait = false;
                }
                pvp_tra.FindChild("stear").gameObject.SetActive(true);
                pvp_tra.FindChild("time").gameObject.SetActive(false);
                if (!SelfRole._inst.isDead)
                {
                    MoveProxy.getInstance().sendVec();
                }
            }
            else if (wait_time <= 9)
            {
                pvp_tra.FindChild("stear").gameObject.SetActive(false);
                pvp_tra.FindChild("time").gameObject.SetActive(true);
                //pvp_tra.FindChild("time").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_countdown_countdown" + wait_time);
                pvp_tra.FindChild("time").GetComponent<Text>().text = wait_time.ToString ();
            }
            else if (wait_time > 9)
            {
                pvp_tra.FindChild("stear").gameObject.SetActive(false);
                pvp_tra.FindChild("time").gameObject.SetActive(false);
            }
            wait_time--;
        }

        void sett_f(bool b)
        {
            a1_gamejoy.inst_skillbar.transform.FindChild("skillbar/combat").gameObject.SetActive(b);
            a1_gamejoy.inst_joystick.transform.FindChild("joystick/Image").GetComponent<Image>().enabled = b;
            a1_gamejoy.inst_joystick.transform.FindChild("joystick/stick").GetComponent<Image>().enabled = b;
            a3_expbar.instance.transform.FindChild("lt_temp").gameObject.SetActive(b);
            a3_expbar.instance.transform.FindChild("operator").gameObject.SetActive(b);
            a3_liteMinimap.instance.transform.FindChild("fun_open").gameObject.SetActive(b);
        }
        private void enter_pic2_show()
        {
            if (PlayerModel.getInstance().profession == 2)
                pic_icon1.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[0]);
            if (PlayerModel.getInstance().profession == 3)
                pic_icon1.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[1]);
            if (PlayerModel.getInstance().profession == 5)
                pic_icon1.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[2]);

        }
        private void enter_pic2_jl_show()
        {
            if (PlayerModel.getInstance().profession == 2)
                pic_icon1_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[0]);
            if (PlayerModel.getInstance().profession == 3)
                pic_icon1_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[1]);
            if (PlayerModel.getInstance().profession == 5)
                pic_icon1_jl.transform.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_ar_" + LevelProxy.getInstance().codes[2]);

        }
        public void enter_pic()
        {
            List<SXML> listSxml = null;
            if (listSxml == null)
            {
                listSxml = XMLMgr.instance.GetSXMLList("accent_relic.relic");
                for (int i = 0; i < listSxml.Count; i++)
                {
                    if (listSxml[i].getInt("carr") == PlayerModel.getInstance().profession && listSxml[i].getString("type") == LevelProxy.getInstance().codess[0].ToString())
                    {
                        List<SXML> god_id = listSxml[i].GetNodeList("relic_god", "id==" + LevelProxy.getInstance().codess[1].ToString());
                        {
                            foreach (SXML x in god_id)
                            {
                                text1.text = x.getString("des1");
                                text2.text = x.getString("des2");
                                text11.text = x.getString("des1");
                                text21.text = x.getString("des2");
                            }
                        }
                    }
                }
            }
        }
        public void enter_pic_jl()
        {
            List<SXML> listSxml = null;
            if (listSxml == null)
            {
                listSxml = XMLMgr.instance.GetSXMLList("accent_relic.relic");
                for (int i = 0; i < listSxml.Count; i++)
                {
                    if (listSxml[i].getInt("carr") == PlayerModel.getInstance().profession && listSxml[i].getString("type") == LevelProxy.getInstance().codess[0].ToString())
                    {
                        List<SXML> god_id = listSxml[i].GetNodeList("relic_god", "id==" + LevelProxy.getInstance().codess[1].ToString());
                        {
                            foreach (SXML x in god_id)
                            {
                                text1_jl.text = x.getString("des1");
                                text2_jl.text = x.getString("des2");
                                text11_jl.text = x.getString("des1");
                                text21_jl.text = x.getString("des2");
                            }
                        }
                    }
                }
            }
        }
        public override void onClosed()
        {
            if (a3_buff.instance.bless_ad) a3_buff.instance.bless_ad = false;
            EndBless();
            CancelInvoke("Jdzc_GameTime");
            CancelInvoke("citywar_GameTime");
            
            instance = null;
            room = null;
            eroom = e_room.Normal;
            A3_ActiveProxy.getInstance().removeEventListener(A3_ActiveProxy.EVENT_ONBLESS, OnBless);
        }
        void Update()
        {
            if (exittime.gameObject.activeInHierarchy)
            {
                double ct = close_time - (double)muNetCleint.instance.CurServerTimeStamp;
                fin_exittime.text = "<color=#ff0000>(" + (int)ct + ")</color>";
                if (ct <= 0)
                {
                    LevelProxy.getInstance().sendLeave_lvl();
                    if (TeamProxy.getInstance().MyTeamData != null)
                    {
                        TeamProxy.getInstance().SendLeaveTeam(PlayerModel.getInstance().cid);
                        doors = 0; needkill = 0;
                        
                        if (TeamProxy.getInstance().hasEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES))
                        {
                            TeamProxy.getInstance().removeEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES,WdsyOpenDoor.instance.killNum);
                        }
                    }
                }
            }



        }
        //队伍击杀

        public int km_count = 0;
        int mid;
        //刷新离开时间
        void RefreshExitTime()
        {            
            // 公共
            normalExitTime.text = Globle.formatTime((int)Mathf.Max((int)((double)endtime - (double)muNetCleint.instance.CurServerTimeStamp), 0));
            //单独
            if (exitTime != null) exitTime.text = Globle.formatTime((int)Mathf.Max((int)((double)endtime - (double)muNetCleint.instance.CurServerTimeStamp), 0));
            //debug.Log("PPP"+ (int)Mathf.Max((int)((double)endtime - (double)muNetCleint.instance.CurServerTimeStamp)));
            if (exitTime_bar != null) exitTime_bar.fillAmount = (float)Mathf.Max((float)((double)endtime - (double)muNetCleint.instance.CurServerTimeStamp), 0) / (float)TotalSec;

            if (eroom == e_room.DRAGON)
            {
                int curMin = (int)(endtime - muNetCleint.instance.CurServerTimeStamp) / 60;
                int curSec;
                string curDragonName = A3_SlayDragonModel.getInstance().GetCurrentDragonName();
                if (curMin >= A3_SlayDragonModel.getInstance().GetKillingTime())
                {
                    curSec =(int) (endtime - muNetCleint.instance.CurServerTimeStamp - 60 * A3_SlayDragonModel.getInstance().GetKillingTime());
                    if (curSec > 0)
                        dragon_txt.text = ContMgr.getCont("a3_insideui_refre0",new List<string>() { curDragonName, curSec.ToString() });
                }
                else
                {
                    curSec = (int)(endtime - muNetCleint.instance.CurServerTimeStamp);
                    if (curSec > 0)
                    {
                        string time = string.Format("{0:D2}:{1:D2}", curSec / 60, curSec % 60);
                        dragon_txt.text = ContMgr.getCont("a3_insideui_refre1", new List<string>() { curDragonName, time.ToString() });
                    }
                }
            }

        }
        Dictionary<int, Variant> phase = new Dictionary<int, Variant>();
        Dictionary<int, Dictionary<string, string>> phaseChild = new Dictionary<int, Dictionary<string, string>>();
        void readLevel(int door,Variant datas=null)
        {
            int doorkill = a3_counterpart.lvl;//副本难度
            phase.Clear();
            phaseChild.Clear();
            for (int i = 0; i < datas["diff_lvl"][doorkill]["phase"]._arr.Count; i++)
            {
                if (!phase.ContainsKey(i))
                    phase.Add(i, datas["diff_lvl"][doorkill]["phase"][i]);
            }
            foreach (var item in phase)
            {
                Dictionary<string, string> itemvalue = new Dictionary<string, string>();
                itemvalue.Add("p", item.Value["p"]._str);
                itemvalue.Add("des", item.Value["des"]._str);
                itemvalue.Add("target", item.Value["target"]._str);
                itemvalue.Add("num", item.Value["num"]._str);

                if (!phaseChild.ContainsKey(item.Key))
                    phaseChild.Add(item.Key, itemvalue);

            }

            var idtext = GetNowTran().FindChild("info/info_killnums/mid");
            if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_9", phaseChild[door]["des"]);
            var tns = GetNowTran().FindChild("info/info_killnums/Text");//队伍击杀
            
            if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", km_count.ToString()) + "/" + phaseChild[door]["num"];

        }
        public void Using_jc()
        {
          
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_2*exp/Text");
            if (tn == null) return;
            var using_tn= GetNowTran().FindChild("info/info_2*exp/using_Text");
            var tn2= GetNowTran().FindChild("info/info_bless/Text");
            var using_tn2= GetNowTran().FindChild("info/info_bless/using_Text");

            if (a3_buff.instance.bless_ad == true)
            {
                using_tn2.gameObject.SetActive(true);
                using_tn2.GetComponent<Text>().text = "伤害鼓舞：+" + (5 * A3_ActiveModel.getInstance().blessnum_ybbd + 15 * A3_ActiveModel.getInstance().blessnum_yb) + "%";
                tn2.gameObject.SetActive(false);
            }
            else
            {
                using_tn2.gameObject.SetActive(false);
                tn2.gameObject.SetActive(true);
            }


            if (a3_buff.instance.dou_exp == true)
            {
                using_tn.gameObject.SetActive(true);
                tn.gameObject.SetActive(false);
                this.transform.FindChild("exp/btn_double/blesson").gameObject.SetActive(true);

            }
            else
            {
                using_tn.gameObject.SetActive(false);
                tn.gameObject.SetActive(true);
                this.transform.FindChild("exp/btn_double/blesson").gameObject.SetActive(false);
            }
        }
        //设置击杀数
        public void SetInfKill(int i,int tpid)      
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_killnum/Text");
            if (tn == null) return;
            if (tpid == -1) return;
            string str = string.Empty;
            switch (tpid)
            {
                case 101:
                    str = ContMgr.getCont("fb_info_1", new List<string> { i.ToString() }) + "/" + SvrLevelConfig.instacne.get_level_data(101)["diff_lvl"][a3_counterpart_exp.diff]["m_num"]._int;
                    break;
                case 102:
                    str = ContMgr.getCont("fb_info_1", new List<string> { i.ToString() }) + "/" + SvrLevelConfig.instacne.get_level_data(102)["diff_lvl"][a3_counterpart_gold.diff]["m_num"]._int;
                    break;
                case 103:
                  string s1= SvrLevelConfig.instacne.get_level_data(103)["diff_lvl"][a3_counterpart_mate.diff]["phase"][0]["des"]._str;
                  string s2 = SvrLevelConfig.instacne.get_level_data(103)["diff_lvl"][a3_counterpart_mate.diff]["phase"][1]["des"]._str;
                    if (i < 4)
                        str = s1+":"+i+"/"+/* ContMgr.getCont("fb_info_1", new List<string> { i.ToString() }) + "/" + */SvrLevelConfig.instacne.get_level_data(103)["diff_lvl"][a3_counterpart_mate.diff]["phase"][0]["num"]._int;
                    else
                       str = s2+":"+(i-4)+ "/"+/* ContMgr.getCont("fb_info_1", new List<string> { i.ToString() }) + "/" +*/ SvrLevelConfig.instacne.get_level_data(103)["diff_lvl"][a3_counterpart_mate.diff]["phase"][1]["num"]._int;
                    break;

            }

            tn.GetComponent<Text>().text =str;
            // tn.GetComponent<Text>().text= ContMgr.getCont("fb_info_1", new List<string> { i.ToString() })+"/" + SvrLevelConfig.instacne.get_level_data(101)["diff_lvl"][a3_counterpart_exp.diff_lvl]["m_num"]._int;


        }

        //设置挑战用时等级
        public void SetLvl(int lvl)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("icon_rank");
            if (tn == null) return;
            switch (lvl)
            {
                case 1: tn.GetComponent<Text>().text = ""; break;
            }
        }
        void bjm()
        {

        }

        public void changesignal(int i)
        {
            GetNowTran().FindChild("signal").gameObject.SetActive(true);
            for (int j = 0; j < GetNowTran().FindChild("signal").childCount; j++ )
            {
                GetNowTran().FindChild("signal").GetChild(j).gameObject.SetActive(false);
            }
            if(i > 0)
                GetNowTran().FindChild("signal/" + i).gameObject.SetActive(true);
            else
                GetNowTran().FindChild("signal").gameObject.SetActive(false);
        }

        //城战
        Dictionary<int, GameObject> monobjs = new Dictionary<int, GameObject>();
        public void SetFbInfo()
        {
            Transform infocon = GetNowTran().FindChild("gameinfo");
            GameObject item =GetNowTran().FindChild("gameinfo/info_fb/scrollview/npc").gameObject;
            Transform con = infocon.FindChild("info_fb/scrollview/con");

            infocon.FindChild("playerNum/attack/Text").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().atk_num + ContMgr.getCont("ren");
            infocon.FindChild("playerNum/Defense/Text").GetComponent<Text>().text = A3_cityOfWarModel.getInstance().def_num + ContMgr.getCont("ren");

            foreach (MonInfo info in A3_cityOfWarModel.getInstance().moninfos.Values)
            {
                if (monobjs.ContainsKey(info.mid))
                {
                    monobjs[info.mid].transform.FindChild("Text").GetComponent<Text>().text = info.per + "%";
                }
                else
                {
                    GameObject clon = Instantiate(item) as GameObject;
                    clon.SetActive(true);
                    clon.transform.SetParent(con, false);
                    clon.GetComponent<Text>().text = info.name;
                    clon.transform.FindChild("Text").GetComponent<Text>().text = info.per + "%";
                    monobjs[info.mid] = clon;
                }
            }

            foreach (int i in monobjs.Keys)
            {
                if (!A3_cityOfWarModel.getInstance().moninfos.ContainsKey(i))
                {
                    Destroy(monobjs[i]);
                    monobjs.Remove(i);
                }
            }
        }





        //召唤兽乐园信息（召唤兽乐园专用）
        public void showTs_zhs()
        {
            this.transform.FindChild("zhsly/tishi").gameObject.SetActive(true);
        }
        public void ZHSLY_info(int kill, int cishu = -1) {
            if (GetNowTran() == null) return;
            //vip已购买次数
            int vip_buycount = LevelProxy.getInstance().vip_buycount;
            int Zcount =  SvrLevelConfig.instacne.get_level_data(105)["lvl_target"][0]["single_kill_amounts"];
            int Zcishu = SvrLevelConfig.instacne.get_level_data(105)["lvl_target"][0]["daily_times"]+ vip_buycount;
            if (cishu > 0)
            {
                if (cishu < Zcishu)
                {
                    GetNowTran().FindChild("info/jindu").GetComponent<Text>().text = ContMgr.getCont("fb_info_2", new List<string> { kill.ToString(), Zcount.ToString() });
                    GetNowTran().FindChild("info/cishu").GetComponent<Text>().text = ContMgr.getCont("fb_info_11", new List<string> { cishu.ToString(), Zcishu.ToString() });
                }
                else if (cishu >= Zcishu)
                {
                    GetNowTran().FindChild("info/jindu").GetComponent<Text>().text = ContMgr.getCont("fb_info_10");
                    GetNowTran().FindChild("info/cishu").GetComponent<Text>().text = "";
                }
            }
            else {
                GetNowTran().FindChild("info/jindu").GetComponent<Text>().text = ContMgr.getCont("fb_info_2", new List<string> { kill.ToString(), Zcount.ToString() });
                GetNowTran().FindChild("info/cishu").GetComponent<Text>().text = ContMgr.getCont("fb_info_11", new List<string> { "0", Zcishu.ToString() });
            }
            if(kill >= Zcount)
                GetNowTran().FindChild("info/jiangli/canget").gameObject.SetActive(true);
            else GetNowTran().FindChild("info/jiangli/canget").gameObject.SetActive(false);
        }

        //设置击杀进度
        public void SetInfKillPgs(int i, int max)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_killnum/Text");
            if (tn == null) return;
            tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_2", new List<string> { i.ToString(), max.ToString() });
        }

        //设置宝藏(金币副本专用)
        public void SetInfBaozang(int i)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_baozang/Text");
            if (tn == null) return;
            tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_6", new List<string> { i.ToString() });     
        }
        public void refre_lvl()
        {
            for (int i = 0; i < 10; i++) { }
        }
        //设置经验
        public void SetInfExp(int i)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_exp/Text");
            if (tn == null) return;
            addexp += i;
            tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_3", new List<string> { (addexp/10000).ToString() })+ContMgr.getCont("globle_money");
        }

        //设置波数(经验副本专用)
        public void SetInfBo(int i)
        {
            int bos = 0;
            // int bo = 0;
            if (GetNowTran() == null) return;
            Variant data = SvrLevelConfig.instacne.get_level_data(101);
            int diffs = a3_counterpart_exp.diff;
            //int diff = a3_active_exp.diff;
            //if (bo > 0)
            //    bo = data["diff_lvl"][diff]["bo_all"];
            if (diffs > 0)
                bos = data["diff_lvl"][diffs]["bo_all"];
            var tn = GetNowTran().FindChild("info/info_boshu/Text");
            if (tn == null) return;
            //if (bo > 0)
            //{
            //    tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_4", new List<string> { i.ToString(), bo.ToString() });
            //    bo = 0;
            //}

            if (bos > 0)
            {
                tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_4", new List<string> { i.ToString(), bos.ToString() });
                bos = 0;
            }
        }

        //设置金币
        public static uint AllMoneynum=0;
        public void SetInfMoney(int i)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_money/Text");
            if (tn == null) return;
            addmoney += i;
            tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_5", new List<string> { addmoney.ToString() });
            AllMoneynum = (uint)addmoney;
           // debug.Log("我获得的金币"+addmoney);
        }

        //设置BOSS(副本结束则为1/1)
        public void SetInfBoss(int i)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("info/info_boss/Text");
            if (tn == null) return;
            tn.GetComponent<Text>().text = ContMgr.getCont("fb_info_7", new List<string> { i.ToString() });
        }

        //添加副本广播
        public void SetBroadCast(Variant data)
        {
            var txt = ContMgr.getCont(data["msg"]);
            var go = Instantiate(fb_cast.gameObject) as GameObject;
            go.transform.FindChild("Text").GetComponent<Text>().text = txt;
            go.transform.SetParent(broad);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            Destroy(go, 4);
        }


        public Transform GetNowTran()
        {       
            switch (eroom)
            {
                case e_room.Exp:
                    return exp_tra;
                case e_room.Money:
                    return money_tra;
                case e_room.Cailiao:
                    return cailiao_tra;
                case e_room.MLZD:
                    return mlzd_tra;
                case e_room.ZHSLY:
                    return zhsly_tra;
                case e_room.WDSY:
                    return wdsy_tra;
                case e_room.TLFB109:
                    return tlfb109_tra;
                case e_room.TLFB110:
                    return tlfb110_tra;
                case e_room.TLFB111:
                    return tlfb111_tra;
                case e_room.DRAGON:
                    return dragon_tra;
                case e_room.JDZC:
                    return jdzc_tra;
                //case e_room.PVP:
                //return pvp_tra;
                case e_room.CITYWAR:
                    return cityWar_tra;
            }
            return null;
        }
        public void setAct()
        {
            transform.FindChild("normal/btn_quitfb").gameObject.SetActive(false);
            exp_tra.gameObject.SetActive(false);
            money_tra.gameObject.SetActive(false);
            cailiao_tra.gameObject.SetActive(false);
            mlzd_tra.gameObject.SetActive(false);
            zhsly_tra.gameObject.SetActive(false);
            wdsy_tra.gameObject.SetActive(false);
            tlfb109_tra.gameObject.SetActive(false);
            tlfb110_tra.gameObject.SetActive(false);
            tlfb111_tra.gameObject.SetActive(false);
            normal.gameObject.SetActive(false);
            btn_quit.gameObject.SetActive(true);
        }
        public void OnLvFinish(Variant data)
        {
            if (data.ContainsKey("close_tm"))
            {
                double ct = data["close_tm"];
                closetime = ct;
            }
            transform.FindChild("normal/level_finish").gameObject.SetActive(true);
            new BaseButton(transform.FindChild("normal/level_finish/bt")).onClick = (GameObject go) =>
            {
                LevelProxy.getInstance().sendLeave_lvl();
            };
        }

        void ShowBless()
        {

        }

        public void SetKillNum(int i, int max = -1,int tpid=-1)
        {
            if (max > 0)
            {
                SetInfKillPgs(i, max);
            }
            else
            {
                SetInfKill(i,tpid);
            }

        }

        public void SetGoldLeft(int i)
        {
            SetInfBaozang(i);
        }

        //祝福成功
        public void OnBless(GameEvent e)
        {
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("btn_blessing/blesson");
            if (tn == null) return;
            tn.gameObject.SetActive(true);
            blesstime = 100;

            GetNowTran().FindChild("info/info_bless/using_Text").GetComponent<Text>().text =
                "伤害鼓舞：+" + (5 * A3_ActiveModel.getInstance().blessnum_ybbd + 15 * A3_ActiveModel.getInstance().blessnum_yb) + "%";
            //鼓舞buff
            //a3_herohead.instance.isclear = true;
            //a3_herohead.instance.cheer_bf = true;
            //if (a3_herohead.instance != null)
            //    a3_herohead.instance.refresBuff();

        }

        public void EndBless()
        {
           
            if (GetNowTran() == null) return;
            var tn = GetNowTran().FindChild("btn_blessing/blesson");
            if (tn == null) return;
            tn.gameObject.SetActive(false);
            //鼓舞buff
            //a3_herohead.instance.isclear = true;
            //a3_herohead.instance.cheer_bf = false;
            //if (a3_herohead.instance != null)
            //    a3_herohead.instance.refresBuff();
        }
    }
}
