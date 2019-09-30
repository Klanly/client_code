using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace MuGame
{
    //金币获取方式
    class a3_getGoldWay:Window
    {
        public static a3_getGoldWay _instance;
        BaseButton btn_close;//关闭按钮
        Text cs_rotine;
        Text cs_goldfb;
        Text cs_getMoney;
        public override void init()
        {
            inText();
            btn_close = new BaseButton(transform.FindChild("btn_close"));
            cs_rotine = getTransformByPath("cells/scroll/content/routine/name/dj").GetComponent<Text>();
            cs_goldfb = getTransformByPath("cells/scroll/content/goldfb/name/dj").GetComponent<Text>();
            cs_getMoney = getTransformByPath("cells/scroll/content/dianjin/name/dj").GetComponent<Text>();
            btn_close.onClick += onBtnCloseClick;
            new BaseButton(getTransformByPath("cells/scroll/content/routine/go")).onClick = routine_go;
            new BaseButton(getTransformByPath("cells/scroll/content/goldfb/go")).onClick = goldfb_go;
            new BaseButton(getTransformByPath("cells/scroll/content/guaji/go")).onClick = guaji_go;
            new BaseButton(getTransformByPath("cells/scroll/content/dianjin/go")).onClick = dianjin_go;
        }

        void inText()
        {
            this.transform.FindChild("bg/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_1");//金币获取
            this.transform.FindChild("cells/scroll/content/goldfb/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_2");//黄金王的宝藏
            this.transform.FindChild("cells/scroll/content/goldfb/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_3");//通关黄金王的宝藏可获得大量金币。
            this.transform.FindChild("cells/scroll/content/goldfb/go/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_4");//立即前往
            this.transform.FindChild("cells/scroll/content/guaji/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_5");//野外挂机
            this.transform.FindChild("cells/scroll/content/guaji/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_6");//击杀野外怪物可获得少量金币。
            this.transform.FindChild("cells/scroll/content/guaji/go/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_4");//立即前往
            this.transform.FindChild("cells/scroll/content/dianjin/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_7");//金币兑换
            this.transform.FindChild("cells/scroll/content/dianjin/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_8");//花费少量钻石兑换大量金币。
            this.transform.FindChild("cells/scroll/content/dianjin/go/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getGoldWay_4");//立即前往

        }
        public override void onShowed()
        {
            btn_close.addEvent();
            want_to();
            refresh();
        }
        public override void onClosed()
        {
            btn_close.removeAllListener();
        }
        public void onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETGOLDWAY);
        }

        void refresh()
        {
            var dd = A3_TaskModel.getInstance().GetDailyTask();
            if (dd != null)
            {
                cs_rotine.text = "(" + (A3_TaskModel.getInstance().GetTaskMaxCount(dd.taskId) - dd.taskCount) + "/" + A3_TaskModel.getInstance().GetTaskMaxCount(dd.taskId) + ")";
                this.transform.FindChild("cells/scroll/content/routine").gameObject.SetActive(true);
            }
            else
            {
                //cs_rotine.text = "(未开启)";
                this.transform.FindChild("cells/scroll/content/routine").gameObject.SetActive(false);
            }
            Variant data = SvrLevelConfig.instacne.get_level_data(102);
            int max_times = data["daily_cnt"];
            int use_times = 0;
            if (MapModel.getInstance().dFbDta.ContainsKey(102))
            {
                use_times = Mathf.Min(MapModel.getInstance().dFbDta[102].cycleCount, max_times);
            }
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GOLD_DUNGEON) && max_times != use_times)
            {
                cs_goldfb.text = "(" + (max_times - use_times) + "/" + max_times + ")";
                this.transform.FindChild("cells/scroll/content/goldfb").gameObject.SetActive(true);
            }
            else
            {
                //cs_expfb.text = "(未开启）";
                this.transform.FindChild("cells/scroll/content/goldfb").gameObject.SetActive(false);
            }

            //if (ExchangeModel.getInstance().Count >= 10)
            //{
                
            //}
            //else
            //{
            //    cs_getMoney.text = "(" + (10 - ExchangeModel.getInstance().Count) + "/10)";
            //    this.transform.FindChild("cells/scroll/content/dianjin").gameObject.SetActive(true);
            //}
            int num;
            if (A3_VipModel.getInstance().Level > 0)
                num = A3_VipModel.getInstance().vip_exchange_num(3);
            else
                num = 10;
            ExchangeModel exModel = ExchangeModel.getInstance();
            if (num - exModel.Count > 0)
            {
                cs_getMoney.text = "(" + (num - exModel.Count) + "/" + num + ")";
                this.transform.FindChild("cells/scroll/content/dianjin").gameObject.SetActive(true);
            }
            else
                this.transform.FindChild("cells/scroll/content/dianjin").gameObject.SetActive(false);


        }



        int map_x;
        int map_y;
        int map_id;
        void want_to()
        {
            map_x = 0;
            map_y = 0;
            map_id = 1;

            SXML s_xml = XMLMgr.instance.GetSXML("god_light");
            List<SXML> s_xml_info = s_xml.GetNodeList("player_info");
            if (s_xml_info != null)
            {
                foreach (SXML x in s_xml_info)
                {
                    if (x.getInt("zhuan") < PlayerModel.getInstance().up_lvl)
                    {

                    }
                    else if (x.getInt("zhuan") == PlayerModel.getInstance().up_lvl)
                    {
                        if (x.getInt("lv") > PlayerModel.getInstance().lvl)
                            break;
                    }
                    else
                    {
                        break;
                    }
                    map_id = x.getInt("map_id");
                    map_x = x.getInt("map_x");
                    map_y = x.getInt("map_y");
                }
            }

        }
        void routine_go(GameObject go)
        {
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DAILY_TASK, true))
            {
                ArrayList arr = new ArrayList();
                arr.Add(20005);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK, arr);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETGOLDWAY);
            }
        }
        void goldfb_go(GameObject go)
        {
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GOLD_DUNGEON, true))
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETGOLDWAY);
            }
        }
        void guaji_go(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETGOLDWAY);
            SelfRole.moveto(map_id, new Vector3(map_x, 0, map_y));
        }
        void dianjin_go(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETGOLDWAY);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);

        }
    }


}
