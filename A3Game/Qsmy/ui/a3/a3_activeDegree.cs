using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
    class a3_activeDegree:Window
    {

        public static a3_activeDegree instance;
        public Image loadslider;
        public int active_num;
       
        Transform icon,iconi;
        Text coin, diamond, bangzuan;
        private GameObject pre;
        private Transform contain;

        
        Dictionary<int, int> reward_geted = new Dictionary<int, int>();
        public override void init()
        {
            instance = this;
            inText();
            loadslider = transform.FindChild("reward/load/load_fill").GetComponent<Image>();
            icon = transform.FindChild("reward/icon_reward");
            pre = transform.FindChild("icon_go/scroll_rect/temprefab").gameObject;

            contain = transform.FindChild("icon_go/scroll_rect/contain");

            coin=transform.FindChild("coin/image/num").GetComponent<Text>();
            diamond = transform.FindChild("diamond/image/num").GetComponent<Text>();
            bangzuan = transform.FindChild("bangzuan/image/num").GetComponent<Text>();

            new BaseButton(transform.FindChild("btn_close")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVEDEGREE);
            };
            new BaseButton(transform.FindChild("coin/add")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                a3_exchange.Instance?.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
            };
            new BaseButton(transform.FindChild("diamond/add")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                a3_Recharge.Instance?.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
            };
            new BaseButton(transform.FindChild("bangzuan/add")).onClick=(GameObject go)=>{
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                a3_Recharge.Instance.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
            };
            // a3_activeDegreeProxy.getInstance().addEventListener(a3_activeDegreeProxy.EVENT_GET_ALLPOINT, onLoad_Change);
           






        }
        void inText()
        {
            this.transform.FindChild("icon_go/scroll_rect/temprefab/go_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeDegree_1");
            this.transform.FindChild("icon_go/scroll_rect/temprefab/get_reward/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_activeDegree_2");
        }
        override public void onShowed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);
            a3_activeDegreeProxy.getInstance()?.SendGetPoint(1);
            onActive_Load();
           
            refreshMoney();
            do_Active();
            onLoad_Change();
            getComponentByPath<ScrollRect>("icon_go/scroll_rect").verticalNormalizedPosition = 1;
        }
        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
        }
        private void refreshMoney()
        {
            coin.text = Globle.getBigText(PlayerModel.getInstance().money);
            diamond.text= PlayerModel.getInstance().gold.ToString();
            bangzuan.text = PlayerModel.getInstance().gift.ToString();
        }

        public  void onActive_Load()
        {
                      
            List<SXML> xmlreward = XMLMgr.instance.GetSXMLList("huoyue.reward");
            for (int i = 0; i < xmlreward.Count; i++)
            {
                //active_XML = XMLMgr.instance.GetSXML("huoyue.reward", "id==" + i);
                //int icon_s = active_XML.getInt("item");
                icon.transform.FindChild(i + "/icon_over").gameObject.SetActive(false);
                icon.transform.FindChild(i + "/finsh").gameObject.SetActive(false);
                iconi = icon.transform.FindChild(i + "/icon");
                iconi.GetComponent<Image>().sprite =
                    GAMEAPI.ABUI_LoadSprite(a3_BagModel.getInstance().getItemDataById((uint)xmlreward[i].getInt("item")).file);

                int index = i;
                if (a3_activeDegreeProxy.getInstance().huoyue_point >= xmlreward[index].getInt("ac"))
                {
                    if (!a3_activeDegreeProxy.getInstance().point.Contains(xmlreward[index].getInt("ac")))
                    {
                        icon.transform.FindChild(i + "/icon_over").gameObject.SetActive(true);
                    }
                    else
                    {
                        icon.transform.FindChild(i + "/icon_over").gameObject.SetActive(false);
                        icon.transform.FindChild(i + "/finsh").gameObject.SetActive(true);
                    }
                }
                 


                new BaseButton(transform.FindChild("reward/icon_reward/" + i)).onClick = (GameObject oo) => {
                    if (a3_activeDegreeProxy.getInstance().huoyue_point >= xmlreward[index].getInt("ac"))
                    {
                        if (!a3_activeDegreeProxy.getInstance().point.Contains(xmlreward[index].getInt("ac")))
                        {
                            a3_activeDegreeProxy.getInstance().SendGetReward(2, (uint)xmlreward[index].getInt("ac"));
                            flytxt.instance.fly(ContMgr.getCont("a3_activeDegree"));
                            a3_activeDegreeProxy.getInstance().SendGetPoint(1);
                        }
                        else
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_activeDegree_over"));
                        }
                    }
                    else
                    {
                         ArrayList arr = new ArrayList();
                         arr.Add((uint)xmlreward[index].getInt("item"));//第一项为物品id
                         arr.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                        a3_miniTip.Instance?.transform.SetAsLastSibling();
                    }

                };


            }




        }

        public void do_Active()
        {
            Clear_con();
             
            List<SXML> xmlItems = XMLMgr.instance.GetSXMLList("huoyue.active");
            for (int i = 0; i < xmlItems.Count; i++)
            {
                var go = GameObject.Instantiate(pre) as GameObject;
                go.transform.SetParent(contain);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
             
                Set_Line(go.transform, xmlItems[i]);
              
            }
         

        }
        private void Clear_con()
        {
            if (contain.childCount == 0) return;
            else
            {
                for (int i = 0; i < contain.childCount; i++)
                {
                    Destroy(contain.GetChild(i).gameObject);
                }
            }          
        }
        private void Set_Line(Transform go, SXML xml)
        {
            new BaseButton(go.transform.FindChild("go_btn/Text")).onClick = (GameObject oo) => {
                btn_go(xml.getInt("id"));
            };
            // int usetime = 0;
            int  maxtime = xml.getInt("times");
            //if (MapModel.getInstance().dFbDta.ContainsKey(xml.getInt("id")))
            //{
            //    usetime = Mathf.Min(MapModel.getInstance().dFbDta[xml.getInt("id")].cycleCount, xml.getInt("times"));
            //}
            go.transform.FindChild("active_num").GetComponent<Text>().text = ContMgr.getCont("a3_activeDegree_huoyuedu") +xml.getInt("ac_num");


            if (xml.getInt("type") == 1)
            {
                int lv = (int)PlayerModel.getInstance().lvl;
                int level2 = (int)PlayerModel.getInstance().up_lvl;

                string lvl = xml.getString("pram");
                string[] dj = lvl.Split(',');
                int a = int.Parse(dj[0]);
                int b = int.Parse(dj[1]);
                if ((level2 * 100 + lv) >= (a * 100 + b))
                {
                    go.transform.FindChild("go_btn/lock").gameObject.SetActive(false);
                }
                else
                {
                    go.transform.FindChild("go_btn/lock").gameObject.SetActive(true);
                }
            }
            else if (xml.getInt("type") == 2)
            {
                int maintaskid = xml.getInt("pram");
             
                if (maintaskid < A3_TaskModel.getInstance().main_task_id)
                {
                    go.transform.FindChild("go_btn/lock").gameObject.SetActive(false);
                }
                else
                {
                    go.transform.FindChild("go_btn/lock").gameObject.SetActive(true);
                }


                //if (FunctionOpenMgr.instance.dItem.Keys.Contains(func_id) && FunctionOpenMgr.instance.dItem[func_id].opened)
                //{
                //    Debug.LogError(FunctionOpenMgr.instance.dItem[func_id].opened+"s"+ func_id);
                //    go.transform.FindChild("go_btn/lock").gameObject.SetActive(false);
                //}
                //else
                //{
                //    go.transform.FindChild("go_btn/lock").gameObject.SetActive(true);
                //}
            }









            if (a3_activeDegreeProxy.getInstance().itd.ContainsKey((uint)xml.getInt("id")))
            {
                go.transform.FindChild("name_num").GetComponent<Text>().text =
                    xml.getString("des") + "(" + a3_activeDegreeProxy.getInstance().itd[(uint)xml.getInt("id")].count + "/" + xml.getInt("times") + ")";
                if (maxtime - a3_activeDegreeProxy.getInstance().itd[(uint)xml.getInt("id")].count <= 0)
                {

                    go.transform.FindChild("finsh").gameObject.SetActive(true);
                    go.transform.FindChild("get_reward").gameObject.SetActive(false);
                    go.transform.FindChild("go_btn").gameObject.SetActive(false);
                 
                }
                else
                {
                    go.transform.FindChild("finsh").gameObject.SetActive(false);
                    go.transform.FindChild("get_reward").gameObject.SetActive(false);
                    go.transform.FindChild("go_btn").gameObject.SetActive(true);
                }
                //FunctionOpenMgr.instance.dItem.Keys.Contains(func_id) && func_id != 0
                                         
            }
            else
            {
                go.transform.FindChild("name_num").GetComponent<Text>().text = xml.getString("des") + "(0/" + xml.getInt("times") + ")";
                go.transform.FindChild("finsh").gameObject.SetActive(false);
                go.transform.FindChild("get_reward").gameObject.SetActive(false);
                go.transform.FindChild("go_btn").gameObject.SetActive(true);
            }
           
            //通过xml中的id。调用协议里的方法得到此项完成的状态
            
            //new BaseButton(go.transform.FindChild("get_reward")).onClick = (GameObject oo) =>
            //{
            //    go.transform.FindChild("get_reward").gameObject.SetActive(false);
            //    go.transform.FindChild("go_btn").gameObject.SetActive(false);
            //    go.transform.FindChild("finsh").gameObject.SetActive(true);
            //    //a3_activeDegreeProxy.getInstance().SendGetPoint(/*xml.getInt("id")*/);
            //    //向服务器发送请求 领取点数，并根据点数判断是否获得物品

            //};

        }
        public void btn_go(int i)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ACTIVEDEGREE);
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
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUCTION,dlt);
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
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE,dl);
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
                    ArrayList dat10= new ArrayList();
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
                    
            }        
        }


        //通过协议里的数据传到model中。得到此时的总共活跃点数.通过监听得到总共获得的点数
        public void onLoad_Change(/*GameEvent e*/)
        {
            //Variant data = e.data;
            //uint id = 0;
            //if (data.ContainsKey("huoyue_point"))
            //    id = data["huoyue_point"];
            transform.FindChild("reward/current/Text").GetComponent<Text>().text = a3_activeDegreeProxy.getInstance().huoyue_point.ToString();
            loadslider.fillAmount = (float)a3_activeDegreeProxy.getInstance().huoyue_point/ 100;
        }
      
    }
}
