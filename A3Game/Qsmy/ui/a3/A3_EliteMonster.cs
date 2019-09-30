using DG.Tweening;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cross;
using System.Collections;

namespace MuGame
{
    public enum ELITE_MONSTER_PAGE_IDX { ELITEMONSTER = 0, BOSSPAGE = 1, REPORT = 2 };
    public static partial class Extension
    {
        public static void BroadCast(this Dictionary<ELITE_MONSTER_PAGE_IDX, Action<bool>> fn, ELITE_MONSTER_PAGE_IDX idx)
        {
            int i = 0;
            for (List<ELITE_MONSTER_PAGE_IDX> allIdx = new List<ELITE_MONSTER_PAGE_IDX>(fn.Keys); i < fn.Count; i++)
                fn[allIdx[i]](idx == allIdx[i]);
        }
    }
    /// <summary>
    /// 首领主页面
    /// </summary>
    class A3_EliteMonster : Window
    {
        public uint CurrentSelectedMonsterId { get; set; }
        private Transform tabWild,
                          tabBoss,
                          tabReport;
        private static A3_EliteMonster instance;
        public static A3_EliteMonster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new A3_EliteMonster();
                    instance.init();
                }
                return instance;
            }
            set { instance = value; }
        }

        private Toggle toggleWild,
                       toggleCave,
                       toggleReport;
        private Toggle[] toggle;
        //public RewardItemTip itemTip;
        //public RewardEquipTip equipTip;
        private GameObject ig_block,
                           background,
                           prefabRewardItemIcon;
        private bool switchStatu;
        private bool switchEltmonRspnTmn
        {
            set
            {
                if (switchStatu == value)
                    return;
                if (switchStatu = value)
                    InvokeRepeating("TimingEltMonRspnTm", 0, 1);
                else
                    CancelInvoke("TimingEltMonRspnTm");
            }
        }

        public void TurnOnEliteMonTimer() => switchEltmonRspnTmn = true;
        public void TurnOffEliteMonTimer() => switchEltmonRspnTmn = false;
        private Transform tfRewardShow,
                          tfRewardParent;


        Transform contain_ranking,
                  image_rakinig,
                   contain_last;
        public override void init()
        {
            Instance = this;
            inText();
            contain_ranking = getTransformByPath("ranking/Panel/contain");
            image_rakinig = getTransformByPath("ranking/Panel/Image");
            contain_last = getTransformByPath("ranking/down/items/contain");
            new BaseButton(transform.FindChild("btn_close")).onClick = (GameObject go) =>
            {
                Toclose = true;
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
            };

            ReportMessagePage reportPage = ReportMessagePage.Instance;
            EliteMonsterPage elitemonPage = EliteMonsterPage.Instance;
            BossPage bossPage = BossPage.Instance;
            //itemTip = new RewardItemTip(transform.FindChild("con_page/container/rewardItemTip").gameObject);
            //equipTip = new RewardEquipTip(transform.FindChild("con_page/container/rewardEqpTip").gameObject);
            (ig_block = transform.FindChild("ig_bg_bg").gameObject).SetActive(false);
            background = transform.FindChild("background").gameObject;
            prefabRewardItemIcon = transform.FindChild("Template/rewardItemIcon").gameObject;
            tfRewardParent = transform.FindChild("rewardItem");
            tfRewardShow = transform.FindChild("rewardItem/itemMask/items"); // #### todo:在预制件中进行修改,然后在代码中对其路径进行读取,将EliteMonsterPage类下的逻辑移到A3_EliteMonster上
            #region init toggle

            (toggleWild = (tabWild = transform.FindChild("con_tab/view/con/wild")).transform.GetComponent<Toggle>()).onValueChanged.AddListener(
                delegate (bool isOn)
                {
                    //if (isOn)
                    //InvokeRepeating("GetFirstEliteSelected", 0f, 0.2f);
                    fnShow[(int)ELITE_MONSTER_PAGE_IDX.ELITEMONSTER](isOn);
                    nowid = 30001;
                    //background?.SetActive(false);
                    //tfRewardParent?.gameObject.SetActive(true);
                }
            );

            (toggleCave = (tabBoss = transform.FindChild("con_tab/view/con/cave")).transform.GetComponent<Toggle>()).onValueChanged.AddListener(
                delegate (bool isOn)
                {
                    //if(isOn)
                    //    InvokeRepeating("GetFirstBossSelected", 0f, 0.2f);
                    fnShow[(int)ELITE_MONSTER_PAGE_IDX.BOSSPAGE](isOn);
                    nowid = 2012;
                    //background?.SetActive(false);
                    //tfRewardParent?.gameObject.SetActive(true);
                }
            );

            (toggleReport = (tabReport = transform.FindChild("con_tab/view/con/report")).transform.GetComponent<Toggle>()).onValueChanged.AddListener(
                delegate (bool isOn)
                {
                    fnShow[(int)ELITE_MONSTER_PAGE_IDX.REPORT](isOn);
                    //background?.SetActive(true);
                    //tfRewardParent?.gameObject.SetActive(false);
                }
            );
            toggle = new Toggle[] { toggleWild, toggleCave, toggleReport };
            #endregion
            new BaseButton(getTransformByPath("ranking_btn")).onClick = rangkingOnclick_yewai;
            new BaseButton(getTransformByPath("ranking/close")).onClick = (GameObject go) => 
            {
                getGameObjectByPath("ranking").SetActive(false);
                for (int j = 0; j < contain_last.childCount; j++)
                {
                    Destroy(contain_last.GetChild(j).gameObject);
                }
                for (int i=0;i< contain_ranking.transform.childCount;i++)
                {
                    Destroy(contain_ranking.transform.GetChild(i).gameObject);
                }
                getComponentByPath<Text>("ranking/down/name").text = "";

            };


         
        }

        void inText()
        {
            this.transform.FindChild("con_tab/view/con/wild/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_1"); // 野外精英
            this.transform.FindChild("con_tab/view/con/cave/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_2");//地宫首领
            this.transform.FindChild("rewardItem/items_title /title").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_3");//物品预览

            this.transform.FindChild("con_page/container/bossPage/boss_1/text_nameMon").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_4");//尤格索圖斯
            this.transform.FindChild("con_page/container/bossPage/boss_1/text_lvGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_5");//一轉90級
            this.transform.FindChild("con_page/container/bossPage/boss_3/text_nameMon").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_8");//尤格索圖斯
            this.transform.FindChild("con_page/container/bossPage/boss_2/text_lvGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_7");//三轉
            this.transform.FindChild("con_page/container/bossPage/boss_3/text_lvGo").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_9");//五轉



        }


        //当前选中怪物类型
         public static int nowid =30001;
        /*排行界面*/
        void rangkingOnclick_yewai(GameObject go)
        {
            



                getGameObjectByPath("ranking").SetActive(true);
            Dictionary<int, List<dmg_list>> dic = A3_EliteMonsterModel.getInstance().dic_dmg_lst;
            if(dic.Keys.Count<=0)
                return;
            string name = "";
            bool havrlast = false;
            if (dic.ContainsKey(nowid))
            {

                havrlast = true;
                for (int id=0; id< dic[nowid].Count;id++)
                {

                    
                    //  int id = nowid;
                    name = dic[nowid][id].lat_name;
                    GameObject clone = GameObject.Instantiate(image_rakinig.gameObject) as GameObject;
                    clone.transform.SetParent(contain_ranking.transform, false);
                    clone.SetActive(true);
                    clone.transform.FindChild("ranking").GetComponent<Text>().text = dic[nowid][id].rank.ToString();
                    clone.transform.FindChild("name").GetComponent<Text>().text = dic[nowid][id].name.ToString();
                    int ranlk = dic[nowid][id].rank;
                    List<SXML> x= A3_EliteMonsterModel.getInstance().getxml_jingbi(nowid, ranlk);
                    foreach(SXML xx in x )
                    {
                        string  type = xx.getString("type");
                        int num = xx.getInt("value");
                        GameObject gso= IconImageMgr.getInstance().createMoneyIcon(type, 0.8f, num);
                        gso.transform.SetParent(clone.transform.FindChild("items/contain"), false);

                    }
                    List<SXML> y = A3_EliteMonsterModel.getInstance().getxml_item(nowid, ranlk);
                    foreach (SXML yy in y)
                    {
                        uint  item_id = yy.getUint("item_id");
                        int num = yy.getInt("value");
                        GameObject gso = IconImageMgr.getInstance().createA3ItemIcon(item_id,scale:0.8f, num: num);
                        gso.transform.SetParent(clone.transform.FindChild("items/contain"), false);
                    }
                    a3_runestone.commonScroview(clone.transform.FindChild("items/contain").gameObject, x.Count+y.Count);
                }
             }

            /*最后一击*/
            if (havrlast)
            {
                getComponentByPath<Text>("ranking/down/name").text = name;
                List<SXML> s = A3_EliteMonsterModel.getInstance().getxml_jingbi(nowid, 0);
                foreach (SXML xx in s)
                {
                    string type = xx.getString("type");
                    int num = xx.getInt("value");
                    GameObject gso = IconImageMgr.getInstance().createMoneyIcon(type, 0.8f, num);
                    gso.transform.SetParent(getTransformByPath("ranking/down/items/contain"), false);

                }
                List<SXML> z = A3_EliteMonsterModel.getInstance().getxml_item(nowid, 0);
                foreach (SXML yy in z)
                {
                    uint item_id = yy.getUint("item_id");
                    int num = yy.getInt("value");
                    GameObject gso = IconImageMgr.getInstance().createA3ItemIcon(item_id, scale: 0.8f, num: num);
                    gso.transform.SetParent(getTransformByPath("ranking/down/items/contain"), false);
                }
                a3_runestone.commonScroview(getTransformByPath("ranking/down/items/contain").gameObject, s.Count + z.Count);
            }
        }
        /*vip显示*/
        public void RefreshVipCount()
        {
            /*每日可击杀次数*/
            int todaycount = XMLMgr.instance.GetSXML("worldboss.kill_boss_cnt").getInt("cnt");
            /*已购买次数*/
            int buy_over = A3_EliteMonsterModel.getInstance().vip_buy_cnt;
            /*可购买次数*/
            int canbuy_num = A3_VipModel.getInstance().vip_exchange_num(26);
            /*剩余可购买次数*/
            int havecanbuy_num = canbuy_num - buy_over;
            /*已有总次数*/
            int all_have_num = todaycount + buy_over;
            /*已使用次数*/
            int over_count= A3_EliteMonsterModel.getInstance().kill_cnt;
            /*可购买次数*/
            getComponentByPath<Text>("vipbuy/bg/num").text = havecanbuy_num.ToString();
            /*可参与击杀次数*/
            getComponentByPath<Text>("numshow/bg/num").text =(all_have_num-over_count) + "/"+all_have_num;

            new BaseButton(getTransformByPath("vipbuy/Button")).onClick = (GameObject go) => { buy_vip(havecanbuy_num); };
        }
        /*vip购买*/
        void buy_vip(int can_buy)
        {
            if (can_buy <= 0)
            {

                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                return;
            }
            else
            {
                int price = XMLMgr.instance.GetSXML("worldboss.kill_boss_cnt").getInt("vip_cost");
                MsgBoxMgr.getInstance().showConfirm(price + ContMgr.getCont("zuanshi_ci"), () =>
                {
                    EliteMonsterProxy.getInstance().SendbuyvipProxy();
                });

            }
        }

        #region Fn:Show
        Action<bool>[] fnShow = new Action<bool>[]{
            (bool show) =>
            {
                if(show)
                    if (!Instance.IsInvoking("GetFirstEliteSelected"))
                       Instance.InvokeRepeating("GetFirstEliteSelected", 0f, 0.2f);
                //↓打开面板时的初始化显示
                List<uint> monId = A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList();
                if (monId.Count > 0)
                {
                    uint firstMonId = monId[0];
                    EliteMonsterPage.Instance.CreateModel(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + A3_EliteMonsterModel.getInstance().dicEMonInfo[firstMonId].MonId).getString("elite_obj"), firstMonId);
                }
                Instance.tabWild.FindChild("isOn").gameObject.SetActive(show);
                EliteMonsterPage.Instance.owner.SetActive(show);
                if(!show)  EliteMonsterPage.Instance.HideOrShowModel(show);
                Instance.background?.SetActive(false);
                Instance.tfRewardParent?.gameObject.SetActive(true);
            },
            (bool show) =>
            {
                if(show)
                    if (!Instance.IsInvoking("GetFirstBossSelected"))
                        Instance.InvokeRepeating("GetFirstBossSelected", 0f, 0.2f);
                Instance.tabBoss.FindChild("isOn").gameObject.SetActive(show);
                BossPage.Instance.OnShowed();
                BossPage.Instance.owner.SetActive(show);
                BossPage.Instance.InitModel(show);
                BossPage.Instance.HideOrShowModel(show);
                Instance.background?.SetActive(false);
                Instance.tfRewardParent?.gameObject.SetActive(true);
            },
            (bool show) =>
            {
                Instance.tabReport.FindChild("isOn").gameObject.SetActive(show);
                ReportMessagePage.Instance.owner.SetActive(show);
                Instance.background?.SetActive(true);
                Instance.tfRewardParent?.gameObject.SetActive(false);
            }
        };
        #endregion 
        private void OnBossStatuRefresh(GameEvent e)
        {
            List<Variant> data = e.data["boss_status"]?._arr;
            for (int i = 0; i < data.Count; i++)
                if (data[i].ContainsKey("index") && data[i].ContainsKey("status"))
                {
                    int index = data[i]["index"];
                    int statu = data[i]["status"];
                    if (BossPage.Instance.btnBoss1_transmit.gameObject && BossPage.Instance.textBoss1_RspnLftTm.gameObject)
                        switch (index)
                        {
                            case 1:
                                if (statu == 1/*monster alive*/)
                                {
                                    BossPage.Instance.btnBoss1_transmit.gameObject.SetActive(true);
                                    BossPage.Instance.textBoss1_RspnLftTm.gameObject.SetActive(false);
                                }
                                else
                                {
                                    BossPage.Instance.btnBoss1_transmit.gameObject.SetActive(false);
                                    BossPage.Instance.textBoss1_RspnLftTm.gameObject.SetActive(true);
                                }
                                break;
                            case 2:
                                if (statu == 1/*monster alive*/)
                                {
                                    BossPage.Instance.btnBoss2_transmit.gameObject.SetActive(true);
                                    BossPage.Instance.textBoss2_RspnLftTm.gameObject.SetActive(false);
                                }
                                else
                                {
                                    BossPage.Instance.btnBoss2_transmit.gameObject.SetActive(false);
                                    BossPage.Instance.textBoss2_RspnLftTm.gameObject.SetActive(true);
                                }
                                break;
                            case 3:
                                if (statu == 1/*monster alive*/)
                                {
                                    BossPage.Instance.btnBoss3_transmit.gameObject.SetActive(true);
                                    BossPage.Instance.textBoss3_RspnLftTm.gameObject.SetActive(false);
                                }
                                else
                                {
                                    BossPage.Instance.btnBoss3_transmit.gameObject.SetActive(false);
                                    BossPage.Instance.textBoss3_RspnLftTm.gameObject.SetActive(true);
                                }
                                break;
                            default: break;
                        }
                }
        }
        public override void onShowed()
        {
            Toclose = false;
            Instance = this;
            EliteMonsterProxy.getInstance().addEventListener(EliteMonsterProxy.EVENT_ELITEMONSTER, OnEliteMonInfoRefresh);
            EliteMonsterProxy.getInstance().SendProxy();
            A3_ActiveProxy.getInstance().addEventListener(EliteMonsterProxy.EVENT_BOSSOPSUCCESS, OnBossStatuRefresh);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);
            //↓世界BOSS的初始化
            //BossPage.Instance.OnShowed();

            if (uiData != null)
            {
                fnShow[(int)uiData[0]](toggle[(int)uiData[0]].isOn = true);
                uiData = null;
            }
            else
            {
                //↓打开面板时的初始化显示                
                fnShow[(int)ELITE_MONSTER_PAGE_IDX.ELITEMONSTER](toggleWild.isOn = true); //默认第一页响应
                nowid = 30001;
                //if (A3_EliteMonsterModel.getInstance().dicEMonInfo.Count > 0)
                //{
                //    if(IsInvoking("GetFirstEliteSelected"))
                //        InvokeRepeating("GetFirstEliteSelected", 0f, 0.2f);
                //    //uint firstMonId = A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList()[0];
                //    //EliteMonsterPage.Instance.CreateModel(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + A3_EliteMonsterModel.getInstance().dicEMonInfo[firstMonId].MonId).getString("elite_obj"), firstMonId);
                //}
            }
            RefreshVipCount();
        }
        //private void ShowFirstEliteSeleteced()
        //{
        //    if (EliteMonsterPage.Instance.goMonInfoScroll.transform.childCount > 0)
        //    {

        //        EliteMonsterPage.Instance.curSelected?.SetActive(false);
        //        (EliteMonsterPage.Instance.curSelected = EliteMonsterPage.Instance.goMonInfoScroll.transform.GetChild(0).FindChild("bg/selected").gameObject).SetActive(true);
        //        if (IsInvoking("GetFirstEliteSelected"))
        //        {
        //            CancelInvoke("GetFirstEliteSelected");
        //        }
        //        CancelInvoke("ShowFirstEliteSeleteced");
        //    }
        //}
        private void GetFirstEliteSelected()
        {
            if (EliteMonsterPage.Instance.goMonInfoScroll.transform.childCount > 0)
            {
                EliteMonsterPage.Instance.curSelected?.SetActive(false);
                (EliteMonsterPage.Instance.curSelected = EliteMonsterPage.Instance.goMonInfoScroll.transform.GetChild(0).FindChild("bg/selected").gameObject).SetActive(true);
                var rewardItems = A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList();
                if (rewardItems.Count > 0)
                    EliteMonsterPage.Instance.ShowReward(CurrentSelectedMonsterId = rewardItems[0]);
                CancelInvoke("GetFirstEliteSelected");
            }
        }

        private void GetFirstBossSelected()
        {
            if (BossPage.Instance.pboss1?.activeSelf ?? false)
            {
                BossPage.Instance.curSelected?.SetActive(false);
                (BossPage.Instance.curSelected = BossPage.Instance.btnBoss[0].transform.FindChild("selected").gameObject).SetActive(true);
                EliteMonsterPage.Instance.ShowReward(CurrentSelectedMonsterId);
                CancelInvoke("GetFirstBossSelected");
            }
        }

        public void Update()
        {
            BossPage.Instance.Update();
        }
        bool Toclose = false;
        public override void onClosed()
        {
            EliteMonsterProxy.getInstance().removeEventListener(EliteMonsterProxy.EVENT_ELITEMONSTER, OnEliteMonInfoRefresh);
            TurnOffEliteMonTimer();
            EliteMonsterPage.Instance.DestroyModel();
            BossPage.Instance.DestroyModel();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            Instance = null;
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
            if (a3_getJewelryWay.instance && a3_getJewelryWay.instance.closeWin != null && Toclose)
            {
                InterfaceMgr.getInstance().ui_async_open(a3_getJewelryWay.instance.closeWin);
                a3_getJewelryWay.instance.closeWin = null;
            }
            A3_ActiveProxy.getInstance().removeEventListener(EliteMonsterProxy.EVENT_BOSSOPSUCCESS, OnBossStatuRefresh);
            BossPage.Instance.OnClosed();
        }

        public override void dispose()
        {
            ReportMessagePage.Instance = null;
            EliteMonsterPage.Instance = null;
            BossPage.Instance = null;
            base.dispose();
        }

        private void OnEliteMonInfoRefresh(GameEvent e)
        {
            RefreshVipCount();
            EliteMonsterInfo monInfo;
            List<Variant> listData;
            if (e.data.ContainsKey("elite_mon"))
            {
                listData = e.data["elite_mon"]._arr;
                for (int i = 0; i < listData.Count; i++)
                {
                    uint monId = listData[i]["mid"]._uint;
                    monInfo = A3_EliteMonsterModel.getInstance().AddData(listData[i]);
                    if (listData[i].ContainsKey("killer_name"))
                    {
                        if (listData[i]["killer_name"]._str.Length == 0 /* ←怪物复活时发送的消息 */)
                        {
                            EliteMonsterPage.Instance.RefreshMonInfo(monId);
                        }
                        else
                        {
                            ReportMessagePage.Instance.AddReportMessage(
                                date: monInfo.date,
                                playerName: monInfo.killerName,
                                monsterName: XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("name"),
                                infoSyncHandler: delegate () { return ReportMessagePage.Instance.AddReportInfo(monId, monInfo); },
                                desc: true
                            );

                            // ↓刷新精英Boss信息(计时器)
                            EliteMonsterPage.Instance.RefreshMonInfo(monId, monInfo.unRespawnTime);
                        }
                    }
                }
            }
        }

        private void TimingEltMonRspnTm() => EliteMonsterPage.Instance?.ShowLeftTime();

        public void ShowRewardItemIcon(uint? monId = null)
        {
            if (tfRewardShow != null)
            {
                if (!monId.HasValue)
                    monId = CurrentSelectedMonsterId;
                //每次显示之前销毁原先已经显示的道具图标
                ClearRewardItemIcon();
                for (int i = 0; i < EliteMonsterInfo.poolItemReward[monId.Value].Count; i++)
                {
                    //获取奖励物品池中对应怪物id的奖励道具列表
                    var listReward = EliteMonsterInfo.poolItemReward[monId.Value];
                    //实例化一个物品图标
                    GameObject prefabItemIcon = GameObject.Instantiate(prefabRewardItemIcon);
                    GameObject goItemReward = IconImageMgr.getInstance().createA3ItemIcon(listReward[i], istouch: true, ignoreLimit: true);
                    AddFnShowRewardTip(listReward[i], prefabItemIcon.transform.FindChild("btn").gameObject);
                    //添加到奖励道具显示列表中
                    goItemReward.transform.SetParent(prefabItemIcon.transform, false);
                    goItemReward.transform.SetSiblingIndex(prefabItemIcon.transform.FindChild("btn").GetSiblingIndex());
                    prefabItemIcon.transform.SetParent(tfRewardShow, false);
                    prefabItemIcon.SetActive(true);
                }
            }
            //else
            //{
            //    UnityEngine.Debug.LogError("预制件不存在或已经更改");
            //}
        }

        private void AddFnShowRewardTip(uint itemId, GameObject btn)
        {
            new BaseButton(btn.transform).onClick = (GameObject go) =>
            {
                {
                    SXML xmlEliteMonster = XMLMgr.instance.GetSXML("worldboss.mdrop", "mid==" + CurrentSelectedMonsterId);
                    if (xmlEliteMonster != null)
                    {
                        SXML xmlEliteMonsterDrop = xmlEliteMonster.GetNode("item", "id==" + itemId);
                        //在xml中获取id为itemId的节点,并得到节点的type
                        int showType = xmlEliteMonsterDrop.getInt("type");
                        RewardDescText? rewardDescText = null;
                        if (showType == 3)
                        {
                            if (xmlEliteMonster != null)
                            {
                                SXML itemEliteMonster = xmlEliteMonster.GetNode("item", "id==" + itemId);
                                rewardDescText = new RewardDescText
                                {
                                    strItemName = itemEliteMonster.GetNode("item_name").getString("tip"),
                                    strTipDesc = "",
                                    strCarrLimit = itemEliteMonster.GetNode("carr_limit").getString("tip"),
                                    strBaseAttr = itemEliteMonster.GetNode("desc1").getString("tip"),
                                    strAddAttr = itemEliteMonster.GetNode("desc2").getString("tip"),
                                    strExtraDesc1 = itemEliteMonster.GetNode("random_tip1").getString("tip"),
                                    strExtraDesc2 = itemEliteMonster.GetNode("random_tip2").getString("tip"),
                                    strExtraDesc3 = itemEliteMonster.GetNode("random_tip3").getString("tip")
                                };
                            } // endif xmlEliteMonster != null

                        } // endif type == 3
                        ArrayList paraList = new ArrayList();
                        paraList.Add(itemId);
                        paraList.Add(showType);
                        paraList.Add(rewardDescText);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, paraList);
                    } // endif xmlEliteMonster != null
                };
            };
        }

        private void ClearRewardItemIcon()
        {
            if (tfRewardShow != null)
                for (int i = tfRewardShow.childCount - 1; i >= 0; i--) //销毁已经显示的道具图标
                    GameObject.Destroy(tfRewardShow.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 击杀报道页面
    /// </summary>
    class ReportMessagePage
    {
        private float ReportMoveDisY;
        public GameObject owner;

        private static ReportMessagePage instance;
        public static ReportMessagePage Instance
        {
            set { instance = value; }
            get { return instance ?? (instance = new ReportMessagePage()); }
        }
        private Queue<GameObject> qReport;
        private readonly int DisplayNum = 11;

        public Dictionary<uint /* mon id */, EliteMonsterInfo /* message info */> dicMessageInfo;
        public GameObject prefabReportMessage;
        public GameObject goMessageScroll;
        private ReportMessagePage()
        {
            Instance = this;
            qReport = new Queue<GameObject>();
            dicMessageInfo = new Dictionary<uint, EliteMonsterInfo>();
            owner = A3_EliteMonster.Instance.transform.FindChild("con_page/container/report/").gameObject;
            prefabReportMessage = owner.transform.FindChild("Template/reportMessage").gameObject;
            goMessageScroll = owner.transform.FindChild("scroll/scrollview").gameObject;
            ReportMoveDisY = prefabReportMessage.GetComponent<RectTransform>().sizeDelta.y;
        }

        /// <summary>
        /// 添加怪物击杀消息到击杀报道面板中
        /// </summary>
        /// <param name="date">怪物被击杀的最后时刻</param>
        /// <param name="playerName">最后击杀怪物的玩家</param>
        /// <param name="monsterName">被击杀怪物的名称</param>
        /// <param name="infoSyncHandler">同步句柄-当同步失败时,不添加此条击杀消息</param>
        /// <param name="desc">是否将最新消息显示在最上方,默认添加到最下方</param>
        public void AddReportMessage(string date, string playerName, string monsterName, Func<bool> infoSyncHandler = null, bool desc = false)
        {
            if (date == null || (!infoSyncHandler?.Invoke() ?? false))
                return;
            GameObject newReportMessage = UnityEngine.Object.Instantiate(prefabReportMessage);
            newReportMessage.transform.FindChild("textLayout/DateText").GetComponent<Text>().text = date;
            newReportMessage.transform.FindChild("textLayout/PlayerName").GetComponent<Text>().text = playerName;
            newReportMessage.transform.FindChild("textLayout/BossName").GetComponent<Text>().text = monsterName;
            newReportMessage.transform.SetParent(goMessageScroll.transform, false);
            qReport.Enqueue(newReportMessage);
            if (qReport.Count >= DisplayNum)
                UnityEngine.Object.Destroy(qReport.Dequeue());

            if (desc)
                newReportMessage.transform.SetAsFirstSibling();
        }

        public bool AddReportInfo(uint monId, EliteMonsterInfo message)
        {
            if (dicMessageInfo.ContainsKey(monId))
            {
                if (dicMessageInfo[monId].lastKilledTime == message.lastKilledTime)
                    return false;
                dicMessageInfo[monId] = message;
            }
            else
            {
                dicMessageInfo.Add(monId, message);
            }
            return true;
        }

    }
    /// <summary>
    /// 精英怪物页面
    /// </summary>
    class EliteMonsterPage
    {
        #region variable
        public GameObject prefabItemMonInfo;
        //public GameObject prefabRewardIcon;
        public GameObject goMonInfoScroll;
        public GameObject goRewardScroll;
        //private GameObject prefabRewardItemIcon;
        private GameObject curMonAvatar;
        private GameObject curMonScene;
        private GameObject camObj;
        public GameObject curSelected;
        //private Transform tfReward;
        private Dictionary<uint /* monId */, List<uint /* tpid */ > /* rewards */ > dicReward;
        private Dictionary<uint /*monId*/, KeyValuePair<Text /* timer */, uint /* timer value */>> dicTimerText;
        private Dictionary<uint /*monId*/, bool /* transmit available*/ > dicGoAvailable;
        private Dictionary<uint /*monId*/, GameObject> dicEMonItem;
        private Queue<GameObject> qGoReward;
        //public uint currentSelectedMonsterId;
        public GameObject owner;
        private static EliteMonsterPage instance;
        public static EliteMonsterPage Instance
        {
            set { instance = value; }
            get { return instance ?? (instance = new EliteMonsterPage()); }
        }
        #endregion
        private EliteMonsterPage()
        {
            //↓保存各游戏对象
            Instance = this;
            dicReward = new Dictionary<uint, List<uint>>();
            qGoReward = new Queue<GameObject>();
            dicTimerText = new Dictionary<uint, KeyValuePair<Text, uint>>();
            dicGoAvailable = new Dictionary<uint, bool>();
            dicEMonItem = new Dictionary<uint, GameObject>();
            owner = A3_EliteMonster.Instance.transform.FindChild("con_page/container/monPage/").gameObject;
            //prefabRewardIcon = owner.transform.FindChild("Template/rewardIcon").gameObject;
            //prefabRewardItemIcon = owner.transform.FindChild("Template/rewardItemIcon").gameObject;
            prefabItemMonInfo = owner.transform.FindChild("Template/item_elitemon").gameObject;
            //goRewardScroll = owner.transform.FindChild("scrollreward/mask/scrollview").gameObject;
            goMonInfoScroll = owner.transform.FindChild("scrollrect/scrollview").gameObject;
            //tfReward = owner.transform.FindChild("itemMask/items");


            //↓初始化显示界面
            //ShowReward(0);
        }
        private bool CheckGoAvaiable(uint monId)
        {
            int lv_up = 0, lv = 0;
            Variant mapInfo = SvrMapConfig.instance.getSingleMapConf((uint)A3_EliteMonsterModel.getInstance().dicEMonInfo[monId].mapId);
            if (mapInfo.ContainsKey("lv_up")) lv_up = mapInfo["lv_up"]._int;
            if (mapInfo.ContainsKey("lv")) lv = mapInfo["lv"]._int;
            return lv_up < PlayerModel.getInstance().up_lvl || lv_up == PlayerModel.getInstance().up_lvl && lv < PlayerModel.getInstance().lvl;
        }
        private bool CheckGoAvaiable(uint monId, ref int? lv_up, ref int? lv)
        {
            Variant mapInfo = SvrMapConfig.instance.getSingleMapConf((uint)A3_EliteMonsterModel.getInstance().dicEMonInfo[monId].mapId);
            if (mapInfo.ContainsKey("lv_up")) lv_up = mapInfo["lv_up"]._int;
            if (mapInfo.ContainsKey("lv")) lv = mapInfo["lv"]._int;
            return lv_up < PlayerModel.getInstance().up_lvl || lv_up == PlayerModel.getInstance().up_lvl && lv < PlayerModel.getInstance().lvl;
        }
        /// <summary>
        /// 刷新怪物复活信息
        /// </summary>
        /// <param name="monId">怪物id</param>
        /// <param name="sec">怪物重生的时刻</param>
        public void RefreshMonInfo(uint monId, uint sec = 0)
        {
            //↓刷新精英怪物信息(名称、等级)
            if (!dicTimerText.ContainsKey(monId))
            {
                GameObject go = GameObject.Instantiate(prefabItemMonInfo);
                go.transform.SetParent(goMonInfoScroll.transform, false);
                dicTimerText.Add(monId, new KeyValuePair<Text, uint>(
                    key: go.transform.FindChild("info_leftTm/Time").GetComponent<Text>(),
                    value: sec)
                );
                go.transform.FindChild("info_leftTm/Desc").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_EliteMonster_6");
                dicEMonItem.Add(monId, go);
            }
            else
                dicTimerText[monId] = new KeyValuePair<Text, uint>(dicTimerText[monId].Key, sec);
            dicTimerText[monId].Key.text = GetTimeValueBySec(dicTimerText[monId].Value); //←刚显示时的计时器值
            dicTimerText[monId].Key.transform.parent.parent.FindChild("text_nameMon").GetComponent<Text>().text =
                XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("name"); //←怪物的名字
            dicTimerText[monId].Key.transform.parent.parent.FindChild("text_lvGo").GetComponent<Text>().text =
                //XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("zhuan")
                A3_EliteMonsterModel.getInstance().dicEMonInfo[monId].upLv + ContMgr.getCont("uilayer_A3_EliteMonster_20")
                //XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("lv")
                + A3_EliteMonsterModel.getInstance().dicEMonInfo[monId].lv + ContMgr.getCont("uilayer_A3_EliteMonster_30"); //←怪物的等级                
            //↓初始化"立即前往"按钮
            GameObject btnGo = dicTimerText[monId].Key.transform.parent.parent.FindChild("btn_go").gameObject;
            int? uplvGo = null, lvGo = null;
            bool goAvaiable = CheckGoAvaiable(monId, ref uplvGo, ref lvGo);
            if (dicGoAvailable.ContainsKey(monId))
                dicGoAvailable[monId] = goAvaiable;
            else
                dicGoAvailable.Add(monId, goAvaiable);
            Transform btnLock;
            (btnLock = btnGo.transform.parent.FindChild("lock")).gameObject.SetActive(!dicGoAvailable[monId]);
            new BaseButton(btnLock).onClick = (GameObject go) =>
            {
                if (!dicGoAvailable[monId])
                {
                    flytxt.instance.fly(ContMgr.getCont("A3_EliteMonster_lvlock"));
                    return;
                }

            };
            //btnGo.gameObject.SetActive(dicGoAvailable[monId]);
            Text lockText;
            (lockText = btnGo.transform.parent.FindChild("lock/openText").GetComponent<Text>()).text = ContMgr.getCont("A3_EliteMonster_openlock", new List<string>() { uplvGo.ToString(), lvGo.ToString() });
            if (lockText != null)
                lockText.gameObject.SetActive(!goAvaiable);
            dicTimerText[monId].Key.transform.parent.gameObject.SetActive(goAvaiable);
            btnGo.gameObject.SetActive(goAvaiable);
            new BaseButton(btnGo.transform).onClick = (GameObject go) =>
            {
                EliteMonsterInfo monInfo = A3_EliteMonsterModel.getInstance().dicEMonInfo[monId];
                //TransmitPanel.Instance.TargetMapId = monInfo.mapId;
                //TransmitPanel.Instance.SetTargetPosition(monInfo.pos);
                if (PlayerModel.getInstance().line != 0)
                {//当支线时要切换到主线
                    string str = ContMgr.getCont("changeline0");
                    MsgBoxMgr.getInstance().showConfirm(str, () =>
                    {
                        int mapId = monInfo.mapId;
                        Vector3 pos = new Vector3(monInfo.pos.x / GameConstant.GEZI_TRANS_UNITYPOS, 0, monInfo.pos.y / GameConstant.GEZI_TRANS_UNITYPOS);
                        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => SelfRole.WalkToMap(mapId, pos), false, false, 0);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    });
                }
                else
                {
                    if (monInfo.mapId == (int)PlayerModel.getInstance().mapid)
                    {
                        SelfRole.fsm.Stop();
                        SelfRole.WalkToMap(monInfo.mapId, new Vector3(monInfo.pos.x / GameConstant.GEZI_TRANS_UNITYPOS, 0, monInfo.pos.y / GameConstant.GEZI_TRANS_UNITYPOS));
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    }
                    else
                    {
                        //HideOrShowModel(false);
                        //TransmitPanel.Instance.ShowTransmit(monInfo.mapId);
                        //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                        //{
                        //    after_clickBtnWalk = () => InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON),
                        //    after_clickBtnTransmit = () => InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON),
                        //    targetPosition = new Vector3(monInfo.pos.x / GameConstant.GEZI_TRANS_UNITYPOS, 0, monInfo.pos.y / GameConstant.GEZI_TRANS_UNITYPOS),
                        //    mapId = monInfo.mapId
                        //});
                        int mapId = monInfo.mapId;
                        Vector3 pos = new Vector3(monInfo.pos.x / GameConstant.GEZI_TRANS_UNITYPOS, 0, monInfo.pos.y / GameConstant.GEZI_TRANS_UNITYPOS);
                        if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                            SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.WalkToMap(mapId, pos));
                        else
                            SelfRole.WalkToMap(mapId, pos);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    }
                }
            };
            //↓初始化显示怪物avatar的按钮
            GameObject btnShow = dicTimerText[monId].Key.transform.parent.parent.FindChild("btn_show").gameObject;
            new BaseButton(btnShow.transform).onClick = (GameObject go) =>
            {
                curSelected?.SetActive(false);
                curSelected = go.transform.parent.FindChild("bg/selected").gameObject;
                curSelected.SetActive(true);
                A3_EliteMonster.Instance.CurrentSelectedMonsterId = monId;
                A3_EliteMonster.nowid = (int)monId;
                CreateModel(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("elite_obj"), monId);
                ShowReward(monId);
            };
            //↓计时
            if (sec > muNetCleint.instance.CurServerTimeStamp)
            {
                if (CheckGoAvaiable(monId))
                {
                    A3_EliteMonster.Instance.TurnOnEliteMonTimer();
                    dicTimerText[monId].Key.transform.parent.gameObject.SetActive(true);
                    btnGo.transform.parent.FindChild("lock/openText").gameObject.SetActive(false);
                    btnGo.SetActive(false);
                }
            }
            else
            {
                if (CheckGoAvaiable(monId))
                {
                    dicTimerText[monId].Key.transform.parent.gameObject.SetActive(false);
                    btnGo.transform.parent.FindChild("lock/openText").gameObject.SetActive(false);
                    btnGo.SetActive(true);
                }
            }
            var listSorted = A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList();
            for (int i = 0; i < listSorted.Count; i++)
            {
                if (dicEMonItem.ContainsKey(listSorted[i]))
                    dicEMonItem[listSorted[i]].transform.SetAsLastSibling();
            }
        }

        public void ShowLeftTime()
        {
            int i = 0;
            for (List<uint> idx = new List<uint>(dicTimerText.Keys); i < idx.Count; i++)
            {
                if ((dicTimerText[idx[i]].Key.text = GetTimeValueBySec(dicTimerText[idx[i]].Value)).Equals(""))
                {
                    dicTimerText[idx[i]].Key.transform.parent.parent.FindChild("btn_go").gameObject.SetActive(CheckGoAvaiable(idx[i]));
                    dicTimerText[idx[i]].Key.transform.parent.gameObject.SetActive(false);
                }
            }
        }

        private string GetTimeValueBySec(uint sec)
        {
            long currentTime = muNetCleint.instance.CurServerTimeStamp;
            long span = sec - currentTime;
            if (span > 0)
                return string.Format("{0:D2}:{1:D2}:{2:D2}", span / 3600, span % 3600 / 60, span % 60);
            else
                return "";
        }

        public void AddReward(uint monId, uint tpid)
        {
            if (!dicReward.ContainsKey(monId))
                dicReward.Add(monId, new List<uint>());
            dicReward[monId].Add(tpid);
        }

        public void ShowReward(uint monId) => A3_EliteMonster.Instance.ShowRewardItemIcon(monId);


        //private void Clear()
        //{
        //    for (int i = 0; i < qGoReward.Count; i++)
        //        UnityEngine.Object.Destroy(qGoReward.Dequeue());
        //}        

        //private void TempShowReward()
        //{
        //    GameObject itemIcon = IconImageMgr.getInstance().createA3ItemIcon(1540, istouch: true);
        //    GameObject itemGo = UnityEngine.Object.Instantiate(prefabRewardIcon);
        //    itemGo.transform.SetParent(goRewardScroll.transform, false);
        //    itemIcon.transform.SetParent(itemGo.transform, false);
        //    itemGo.transform.FindChild("Text").GetComponent<Text>().text = "稀有道具";
        //    new BaseButton(itemIcon.transform).onClick = (GameObject go) =>
        //    {
        //        Text _text = itemIcon.transform.parent.FindChild("Text").GetComponent<Text>();
        //    };

        //    GameObject itemIcon2 = IconImageMgr.getInstance().createA3ItemIcon(396, istouch: true);
        //    GameObject itemGo2 = UnityEngine.Object.Instantiate(prefabRewardIcon);
        //    itemGo2.transform.SetParent(goRewardScroll.transform, false);
        //    itemIcon2.transform.SetParent(itemGo2.transform, false);
        //    itemGo2.transform.FindChild("Text").GetComponent<Text>().text = "稀有装备";
        //    new BaseButton(itemIcon2.transform).onClick = (GameObject go) =>
        //    {
        //        Text _text = itemIcon2.transform.parent.FindChild("Text").GetComponent<Text>();
        //    };
        //}

        public GameObject CreateModel(string objName, uint monId)
        {
            if (curMonAvatar != null)
            {
                GameObject.DestroyImmediate(curMonAvatar);
                GameObject.DestroyImmediate(camObj);
                GameObject.DestroyImmediate(curMonScene);
            }
            var obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objName);
            var obj_scene = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_emonShow_scene");
            //var xml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId);
            //float camZ = 0;
            //float.TryParse(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("avatar_dist"), out camZ);
            float avatarY = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getFloat("avatar_height");
            float avatarScale = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getFloat("avatar_scale");
            if (obj_prefab == null)
            {
                Debug.LogError("monsters.xml:elite_obj字段配置错误");
                return null;
            }
            curMonAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-153.4f, avatarY, 0f), Quaternion.identity) as GameObject;
            curMonScene = GameObject.Instantiate(obj_scene/*,new Vector3(-153.4f, avatarY, 0f), Quaternion.identity*/) as GameObject;
            foreach (Transform tran in curMonAvatar.GetComponentsInChildren<Transform>())
                tran.gameObject.layer = EnumLayer.LM_FX;
            foreach (Transform tran in curMonScene.GetComponentsInChildren<Transform>())
                if (tran.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;
            Transform cur_model = curMonAvatar.transform.FindChild("model");
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera_emon");
            camObj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = camObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
            cur_model.Rotate(Vector3.up, 180f);
            cur_model.transform.localScale = (avatarScale > 0 ? avatarScale : 1) * Vector3.one;
            //foreach (Transform tran in obj_prefab.GetComponentsInChildren<Transform>(true))
            //{
            //    tran.gameObject.layer = EnumLayer.LM_FX;
            //}

            //控制相机位置
            //camObj.transform.position = new Vector3(camObj.transform.position.x, 5/* + camObj.transform.position.y*/, -10 + camZ);
            return curMonAvatar;
        }

        public void DestroyModel()
        {
            if (curMonAvatar != null)
                GameObject.DestroyImmediate(curMonAvatar);
            if (camObj != null)
                GameObject.DestroyImmediate(camObj);
            if (curMonScene != null)
                GameObject.DestroyImmediate(curMonScene);
        }

        /// <summary>
        /// 隐藏怪物的avatar
        /// </summary>
        /// <param name="showOrHide">显示(true)/隐藏(false)</param>
        public void HideOrShowModel(bool showOrHide = false)
        {
            if (curMonAvatar != null && curMonAvatar.activeSelf != showOrHide)
                curMonAvatar.SetActive(showOrHide);
            if (camObj != null && camObj.activeSelf != showOrHide)
                camObj.SetActive(showOrHide);
            if (curMonScene != null && !showOrHide)
                GameObject.DestroyImmediate(curMonScene);
        }
    }

    /// <summary>
    /// 世界BOSS页面
    /// </summary>
    class BossPage
    {
        public GameObject prefabItemBossInfo;
        public GameObject goBossInfoScroll;
        public GameObject goBossRewardScroll;

        private GameObject curMonAvatar, curMonScene;
        private GameObject camObj;
        public GameObject pboss1, pboss2;
        public BaseButton btnBoss1_transmit, btnBoss2_transmit, btnBoss3_transmit;
        public Text textBoss1_RspnLftTm, textBoss2_RspnLftTm, textBoss3_RspnLftTm;
        public GameObject curSelected;
        public GameObject owner;
        private Dictionary<uint /* boss Id */, List<uint /* tpid */> /* reward */> dicReward;
        private Queue<GameObject> qGoReward;
        private Dictionary<uint /*mon id*/ , GameObject> dicBossItem;
        private static BossPage instance;
        public static BossPage Instance
        {
            set { instance = value; }
            get { return instance ?? (instance = new BossPage()); }
        }
        public List<BaseButton> btnBoss;
        private BossPage()
        {
            Instance = this;
            dicReward = new Dictionary<uint, List<uint>>();
            qGoReward = new Queue<GameObject>();
            dicBossItem = new Dictionary<uint, GameObject>();
            btnBoss = new List<BaseButton>();
            owner = A3_EliteMonster.Instance.transform.FindChild("con_page/container/bossPage").gameObject;
            prefabItemBossInfo = owner.transform.FindChild("Template/item_boss").gameObject;
            goBossInfoScroll = owner.transform.FindChild("scrollrect/scrollview").gameObject;
            goBossRewardScroll = owner.transform.FindChild("scrollmask_reward/scrollview").gameObject;
            Init();
            var nodeList = XMLMgr.instance.GetSXML("worldboss").GetNodeList("boss");
            for (int i = 0; i < nodeList.Count; i++)
            {
                int id = nodeList[i].getInt("id");
                uint monId = nodeList[i].getUint("boss_id");
                string path = "boss_" + id;
                Transform btnParent = owner.transform.FindChild(path);

                if (btnParent != null)
                {
                    dicBossItem.Add(monId, btnParent.gameObject);
                    BaseButton btn = new BaseButton(btnParent.FindChild("btn"));
                    btn.onClick = (GameObject go) =>
                    {
                        curSelected?.SetActive(false);
                        curSelected = go.transform.FindChild("selected").gameObject;
                        curSelected.SetActive(true);
                        CreateModel(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("elite_obj"), monId);
                        A3_EliteMonster.Instance.CurrentSelectedMonsterId = monId;
                        A3_EliteMonster.nowid = (int)monId;
                        A3_EliteMonster.Instance.ShowRewardItemIcon();
                    };
                    btnBoss.Add(btn);
                }
                A3_EliteMonsterModel.getInstance().LoadReward(monId);
            }
        }

        public void Init()
        {
            btnBoss1_transmit = new BaseButton(owner.transform.FindChild("boss_1/btn_transmit"));
            btnBoss1_transmit.onClick = (GameObject go) =>
            {
                var xm = XMLMgr.instance.GetSXML("worldboss");
                var bx = xm.GetNode("boss", "id==1");
                //int pid = bx.getInt("point_id");                
                int mapId = bx.getInt("target_map_id");
                Vector3 pos = new Vector3(bx.getFloat("target_x"), 0, bx.getFloat("target_y"));
                //MapProxy.getInstance().sendBeginChangeMap(pid, true);
                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = mapId,
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.WalkToMap(mapId,
                //    pos),
                //    closeWinName = new string[] { InterfaceMgr.A3_ELITEMON }
                //});
                if (PlayerModel.getInstance().line != 0)
                {//当支线时要切换到主线
                    string str = ContMgr.getCont("changeline0");
                    MsgBoxMgr.getInstance().showConfirm(str, () =>
                    {
                        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => SelfRole.WalkToMap(mapId, pos), false, false, 0);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    });
                }
                else
                {
                    if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                        SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.WalkToMap(mapId, pos));
                    else
                        SelfRole.WalkToMap(mapId, pos);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                }
            };
            btnBoss1_transmit.onClickFalse = (GameObject g) =>
            {
                flytxt.instance.fly(ContMgr.getCont("A3_EliteMonster_lvlock"));
            };
            btnBoss2_transmit = new BaseButton(owner.transform.FindChild("boss_2/btn_transmit"));
            btnBoss2_transmit.onClick = (GameObject go) =>
            {
                var xm = XMLMgr.instance.GetSXML("worldboss");
                var bx = xm.GetNode("boss", "id==2");
                //int pid = bx.getInt("point_id");                
                int mapId = bx.getInt("target_map_id");
                Vector3 pos = new Vector3(bx.getFloat("target_x"), 0, bx.getFloat("target_y"));
                //MapProxy.getInstance().sendBeginChangeMap(pid, true);
                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = mapId,
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.WalkToMap(mapId,
                //    pos),
                //    closeWinName = new string[] { InterfaceMgr.A3_ELITEMON }
                //});
                if (PlayerModel.getInstance().line != 0)
                {//当支线时要切换到主线
                    string str = ContMgr.getCont("changeline0");
                    MsgBoxMgr.getInstance().showConfirm(str, () =>
                    {
                        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => SelfRole.WalkToMap(mapId, pos), false, false, 0);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    });
                }
                else
                {
                    if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                        SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.WalkToMap(mapId, pos));
                    else
                        SelfRole.WalkToMap(mapId, pos);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                }
            };
            btnBoss2_transmit.onClickFalse = (GameObject g) =>
            {
                flytxt.instance.fly(ContMgr.getCont("A3_EliteMonster_lvlock"));
            };
            btnBoss3_transmit = new BaseButton(owner.transform.FindChild("boss_3/btn_transmit"));
            btnBoss3_transmit.onClick = (GameObject go) =>
            {
                var xm = XMLMgr.instance.GetSXML("worldboss");
                var bx = xm.GetNode("boss", "id==3");
                //int pid = bx.getInt("point_id");                
                int mapId = bx.getInt("target_map_id");
                Vector3 pos = new Vector3(bx.getFloat("target_x"), 0, bx.getFloat("target_y"));
                //MapProxy.getInstance().sendBeginChangeMap(pid, true);
                //InterfaceMgr.getInstance().open(InterfaceMgr.TRANSMIT_PANEL, (ArrayList)new TransmitData
                //{
                //    mapId = mapId,
                //    check_beforeShow = true,
                //    handle_customized_afterTransmit = () => SelfRole.WalkToMap(mapId,
                //    pos),
                //    closeWinName = new string[] { InterfaceMgr.A3_ELITEMON }
                //});
                if (PlayerModel.getInstance().line != 0)
                {//当支线时要切换到主线
                    string str = ContMgr.getCont("changeline0");
                    MsgBoxMgr.getInstance().showConfirm(str, () =>
                    {
                        SelfRole.Transmit(MapModel.getInstance().dicMappoint[mapId], () => SelfRole.WalkToMap(mapId, pos), false, false, 0);
                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                    });
                }
                else
                {
                    if (!PlayerModel.getInstance().inFb && mapId != GRMap.instance.m_nCurMapID && MapModel.getInstance().dicMappoint.ContainsKey(mapId) && MapModel.getInstance().dicMappoint[mapId] != GRMap.instance.m_nCurMapID)
                        SelfRole.Transmit(toid: MapModel.getInstance().dicMappoint[mapId], after: () => SelfRole.WalkToMap(mapId, pos));
                    else
                        SelfRole.WalkToMap(mapId, pos);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
                }
            };
            btnBoss3_transmit.onClickFalse = (GameObject g) =>
            {
                flytxt.instance.fly(ContMgr.getCont("A3_EliteMonster_lvlock"));
            };
            new BaseButton(owner.transform.FindChild("boss_1/help")).onClick = Help1;
            new BaseButton(owner.transform.FindChild("boss_2/help")).onClick = Help2;
            new BaseButton(owner.transform.FindChild("boss_3/help")).onClick = Help3;
            new BaseButton(owner.transform.FindChild("pHelp/panel_help/bg/closeBtn")).onClick = CloseHelp;
            new BaseButton(owner.transform.FindChild("pHelp/panel_help/bg_0")).onClick = CloseHelp;
            textBoss1_RspnLftTm = owner.transform.FindChild("boss_1/time").GetComponent<Text>();
            textBoss2_RspnLftTm = owner.transform.FindChild("boss_2/time").GetComponent<Text>();
            textBoss3_RspnLftTm = owner.transform.FindChild("boss_3/time").GetComponent<Text>();

            pboss1 = owner.transform.FindChild("boss_1").gameObject;
            pboss2 = owner.transform.FindChild("boss_2").gameObject;
        }


        void Help1(GameObject go)
        {
            string s0 = A3_EliteMonsterModel.getInstance().s10;
            string s1 = A3_EliteMonsterModel.getInstance().s11;
            string s2 = A3_EliteMonsterModel.getInstance().s12;
            List<int> ids = A3_EliteMonsterModel.getInstance().s13;
            ShowHelp(s0, s1, s2, ids);
        }
        void Help2(GameObject go)
        {
            string s0 = A3_EliteMonsterModel.getInstance().s20;
            string s1 = A3_EliteMonsterModel.getInstance().s21;
            string s2 = A3_EliteMonsterModel.getInstance().s22;
            List<int> ids = A3_EliteMonsterModel.getInstance().s23;
            ShowHelp(s0, s1, s2, ids);
        }
        void Help3(GameObject go)
        {
            string s0 = A3_EliteMonsterModel.getInstance().s30;
            string s1 = A3_EliteMonsterModel.getInstance().s31;
            string s2 = A3_EliteMonsterModel.getInstance().s32;
            List<int> ids = A3_EliteMonsterModel.getInstance().s33;
            ShowHelp(s0, s1, s2, ids);
        }

        void ShowHelp(string tittle, string a, string b, List<int> ids)
        {
            var hp = owner.transform.FindChild("pHelp");
            Text dt1 = hp.FindChild("panel_help/bg/descTxt1").GetComponent<Text>();
            Text dt2 = hp.FindChild("panel_help/bg/descTxt2").GetComponent<Text>();
            dt1.text = "	" + a;
            dt2.text = "	" + b;
            Transform root = hp.FindChild("panel_help/bg/items/scroll/content");
            foreach (var v in root.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == root) GameObject.Destroy(v.gameObject);
            }

            foreach (var v in ids)
            {
                var item = a3_BagModel.getInstance().getItemDataById((uint)v);
                SetIcon(item, root);
            }
            hp.gameObject.SetActive(true);
            var _glg = root.GetComponent<GridLayoutGroup>();
            var rtc = root.GetComponent<RectTransform>();
            rtc.sizeDelta = new Vector2((_glg.cellSize.y + _glg.spacing.x) * ids.Count, 0);
            hp.FindChild("panel_help/bg/title_bg/title").GetComponent<Text>().text = tittle;
            rtc.anchoredPosition3D = new Vector3(0, rtc.anchoredPosition3D.y, 0);
        }

        GameObject SetIcon(a3_ItemData data, Transform parent, int num = -1)
        {
            data.borderfile = "icon_itemborder_b039_0" + (data.quality);
            data.item_type = 0;
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, num, 1, false, -1, 0, false, false);
            icon.transform.SetParent(parent, false);
            return icon;
        }

        public void OnShowed()
        {
            Refresh(null);
            CloseHelp(null);
            for (int i = 1; i < 25; i++)
            {
                GetNextTime(i);
            }
            A3_ActiveProxy.getInstance().addEventListener(EliteMonsterProxy.EVENT_BOSSOPSUCCESS, RefreshTextRspnTime);
            if (btnBoss.Count > 0)
                btnBoss[0].onClick(btnBoss[0].gameObject);
            A3_ActiveProxy.getInstance().SendLoadActivies();
        }

        public void OnClosed()
        {
            A3_ActiveProxy.getInstance().removeEventListener(EliteMonsterProxy.EVENT_BOSSOPSUCCESS, RefreshTextRspnTime);
        }

        void RefreshTextRspnTime(GameEvent e = null)
        {
            TimeSpan ts = new TimeSpan(0, 0, muNetCleint.instance.CurServerTimeStamp);
            //int a1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i11, (A3_EliteMonsterModel.getInstance().i12));
            //if (a1 >= 0) textBoss1_RspnLftTm.text = a1 + ContMgr.getCont("A3_EliteMonster_refre");
            //a1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i21, (A3_EliteMonsterModel.getInstance().i22));
            //if (a1 >= 0) textBoss2_RspnLftTm.text = a1 + ContMgr.getCont("A3_EliteMonster_refre");
            //a1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i31, (A3_EliteMonsterModel.getInstance().i32));
            //if (a1 >= 0) textBoss3_RspnLftTm.text = a1 + ContMgr.getCont("A3_EliteMonster_refre");
            textBoss1_RspnLftTm.text = A3_EliteMonsterModel.getInstance().i11 + "、" + A3_EliteMonsterModel.getInstance().i12 + ContMgr.getCont("A3_EliteMonster_refre");
            textBoss2_RspnLftTm.text = A3_EliteMonsterModel.getInstance().i21 + "、" + A3_EliteMonsterModel.getInstance().i22 + ContMgr.getCont("A3_EliteMonster_refre");
            textBoss3_RspnLftTm.text = A3_EliteMonsterModel.getInstance().i31 + "、" + A3_EliteMonsterModel.getInstance().i32 + ContMgr.getCont("A3_EliteMonster_refre");
        }

        int GetRespawnTime(int a, int b)
        {
            int r = a;
            var vg = GetNextTime(0);
            int n = vg.Hour;
            if (n >= a && n < b)
            {
                r = b;
            }
            if (r - n <= 1 && r - n > 0) return (int)(-1 * (GetNextTime(1) - vg).TotalSeconds);
            return r;
        }

        DateTime GetNextTime(int h)
        {
            int tt = (muNetCleint.instance.CurServerTimeStamp / 3600) * 3600 + h * 60 * 60;
            if (h == 0)
            {
                tt = muNetCleint.instance.CurServerTimeStamp + h * 60 * 60;
            }
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(tt + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        void Refresh(GameEvent e = null)
        {
            RefreshPanel(1);
            RefreshPanel(2);
            RefreshPanel(3);
            RefreshTextRspnTime();
        }

        void RefreshPanel(int i)
        {
            var xm = XMLMgr.instance.GetSXML("worldboss");
            var bx = xm.GetNode("boss", "id==" + i);
            string[] lls = bx.getString("level_limit").Split(',');
            int zhuan = int.Parse(lls[0]);
            int lv = int.Parse(lls[1]);
            bool canTransmit = PlayerModel.getInstance().up_lvl > zhuan ||
                PlayerModel.getInstance().up_lvl == zhuan && PlayerModel.getInstance().lvl > lv;
            if (canTransmit)
            {
                if (i == 1) btnBoss1_transmit.interactable = true;
                else if (i == 2) btnBoss2_transmit.interactable = true;
                else if (i == 3) btnBoss3_transmit.interactable = true;
            }
            else
            {
                if (i == 1) btnBoss1_transmit.interactable = false;
                else if (i == 2) btnBoss2_transmit.interactable = false;
                else if (i == 3) btnBoss3_transmit.interactable = false;
            }
        }

        public void Update()
        {
            //int a1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i11, (A3_EliteMonsterModel.getInstance().i12));
            //int t1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i11 + 1, (A3_EliteMonsterModel.getInstance().i12 + 1));
            //if (t1 < 0)
            //{
            //    TimeSpan ts = new TimeSpan(0, 0, a1 * -1);
            //    TimeSpan tss = new TimeSpan(0, 0, -1 * t1);
            //    if (A3_EliteMonsterModel.getInstance().boss_status[0] == 1)
            //        textBoss1_RspnLftTm.text = tss.Minutes + "分" + tss.Seconds + "秒" + "结束";
            //    if (A3_EliteMonsterModel.getInstance().boss_status[1] == 1)
            //        textBoss2_RspnLftTm.text = tss.Minutes + "分" + tss.Seconds + "秒" + "结束";
            //    if (A3_EliteMonsterModel.getInstance().boss_status[2] == 1)
            //        textBoss3_RspnLftTm.text = tss.Minutes + "分" + tss.Seconds + "秒" + "结束";
            //    //t1为负时为boss刷新后的一小时之内
            //    if (A3_EliteMonsterModel.getInstance().boss_status[0] == 2)
            //        textBoss1_RspnLftTm.text = "暗天军团已灭亡";
            //    if (A3_EliteMonsterModel.getInstance().boss_status[1] == 2)
            //        textBoss2_RspnLftTm.text = "赤血军团已灭亡";
            //    if (A3_EliteMonsterModel.getInstance().boss_status[2] == 2)
            //        textBoss3_RspnLftTm.text = "堕落军团已灭亡";
            //}
            //if (t1 > 0)
            //{
            //}
        }

        private void RefreshTime(GameEvent e)
        {
            Variant data = e.data;
            ChooseTime(A3_EliteMonsterModel.getInstance().i11, A3_EliteMonsterModel.getInstance().i12);
        }

        public void ChooseTime(int a, int b)//boss没刷新之前
        {
            int a1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i11, (A3_EliteMonsterModel.getInstance().i12));
            int t1 = GetRespawnTime(A3_EliteMonsterModel.getInstance().i11 + 1, (A3_EliteMonsterModel.getInstance().i12 + 1));
            var vg = GetNextTime(0);
            int n = vg.Hour;
            //Debug.LogWarning("A3_ActiveModel.getInstance().bossid   " + A3_ActiveModel.getInstance().bossid + "n:" + n + "a" + a);
            if (n > a && n < b)//刷新时间都一样，只用第一个
            {
                textBoss1_RspnLftTm.text = b + ContMgr.getCont("A3_EliteMonster_refre");
                textBoss2_RspnLftTm.text = b + ContMgr.getCont("A3_EliteMonster_refre");
                textBoss3_RspnLftTm.text = b + ContMgr.getCont("A3_EliteMonster_refre");
            }
            else if (n < a || n > b)
            {
                textBoss1_RspnLftTm.text = a + ContMgr.getCont("A3_EliteMonster_refre");
                textBoss2_RspnLftTm.text = a + ContMgr.getCont("A3_EliteMonster_refre");
                textBoss3_RspnLftTm.text = a + ContMgr.getCont("A3_EliteMonster_refre");
            }
        }

        void CloseHelp(GameObject go)
        {
            var hp = owner.transform.FindChild("pHelp");
            hp.gameObject.SetActive(false);
        }

        public void Clear()
        {
            for (int i = 0; i < qGoReward.Count; i++)
            {
                UnityEngine.Object.Destroy(qGoReward.Dequeue());
            }
        }

        public GameObject CreateModel(string objName, uint monId)
        {
            if (curMonAvatar != null)
            {
                GameObject.DestroyImmediate(curMonAvatar);
                GameObject.DestroyImmediate(camObj);
                GameObject.DestroyImmediate(curMonScene);
            }
            var obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objName);
            var obj_scene = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_emonShow_scene");
            if (obj_prefab == null)
            {
                Debug.LogError("monsters.xml:elite_obj字段配置错误");
                return null;
            }
            //float camZ = 0;
            //float.TryParse(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getString("avatar_dist"), out camZ);
            float avatarY = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getFloat("avatar_height");
            float avatarScale = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + monId).getFloat("avatar_scale");

            curMonAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-153.3f, avatarY, 0f), Quaternion.identity) as GameObject;
            curMonScene = GameObject.Instantiate(obj_scene) as GameObject;

            foreach (Transform tran in curMonAvatar.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;
            }
            foreach (Transform tran in curMonScene.GetComponentsInChildren<Transform>())
                if (tran.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;

            Transform cur_model = curMonAvatar.transform.FindChild("model");
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera_worldboss");
            camObj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = camObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
            cur_model.Rotate(Vector3.up, 180f);
            cur_model.transform.localScale = (avatarScale > 0 ? avatarScale : 1) * Vector3.one;
            //cur_model.transform.localScale = Vector3.one;

            foreach (Transform tran in obj_prefab.GetComponentsInChildren<Transform>(true))
            {
                tran.gameObject.layer = EnumLayer.LM_FX;
            }

            //camObj.transform.position = new Vector3(camObj.transform.position.x, 5, -10 + camZ);
            return curMonAvatar;
        }

        public void HideOrShowModel(bool showOrHide = false)
        {
            if (curMonAvatar != null && curMonAvatar.activeSelf != showOrHide)
                curMonAvatar.SetActive(showOrHide);
            if (camObj != null && camObj.activeSelf != showOrHide)
                camObj.SetActive(showOrHide);
        }

        public void DestroyModel()
        {
            if (curMonAvatar != null)
                GameObject.DestroyImmediate(curMonAvatar);
            if (camObj != null)
                GameObject.DestroyImmediate(camObj);
            if (curMonScene != null)
                GameObject.DestroyImmediate(curMonScene);
        }
        private uint? firstBossId;
        public uint FirstBossId
        {
            get
            {
                if (!firstBossId.HasValue)
                    if (A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList().Count > 0)
                        firstBossId = A3_EliteMonsterModel.getInstance().GetSortedMonInfoIdList()[0];// new List<uint>(dicBossItem.Keys)[0];
                    else
                        firstBossId = new List<uint>(dicBossItem.Keys)[0];
                return firstBossId.Value;
            }
        }
        public void InitModel(bool isThisPageShow = false)
        {
            if (isThisPageShow && curMonAvatar == null)
            {
                A3_EliteMonster.Instance.CurrentSelectedMonsterId = FirstBossId;
                CreateModel(XMLMgr.instance.GetSXML("monsters.monsters", "id==" + FirstBossId).getString("elite_obj"), FirstBossId);
            }
        }
    }
    ///// <summary>
    ///// 传送面板
    ///// </summary>
    //class TransmitPanel:Window
    //{
    //    //public GameObject owner;
    //    private static TransmitPanel instance;
    //    public static TransmitPanel Instance
    //    {
    //        get { return instance ?? (instance = new TransmitPanel()); }
    //        set { instance = value; }
    //    }
    //    public int curNeedMoney;
    //    private Text textCostMoney;
    //    private Text textTargetDesc;
    //    private int transmitMapPoint;
    //    private int targetMapId;
    //    public int TargetMapId
    //    {
    //        get
    //        {
    //            return targetMapId;
    //        }
    //        set
    //        {
    //            targetMapId = value;
    //            transmitMapPoint = targetMapId * 100 + 1;
    //        }
    //    }
    //    public Vector3 currentTargetPosition { get; set; }

    //    private TransmitPanel()
    //    {
    //        Instance = this;
    //        //owner = A3_EliteMonster.Instance.transform.FindChild("transmit").gameObject;
    //        textCostMoney = transform.FindChild("bt1/cost").GetComponent<Text>();
    //        textTargetDesc = transform.FindChild("desc").GetComponent<Text>();
    //        Init();
    //    }

    //    public void ShowTransmit(int mapId)
    //    {
    //        bool isfree = PlayerModel.getInstance().vip >= 3;
    //        TargetMapId = mapId;
    //        SXML xml = XMLMgr.instance.GetSXML("mappoint.p", "id==" + transmitMapPoint);
    //        Variant vmap = SvrMapConfig.instance.getSingleMapConf(xml.getUint("mapid"));
    //        string name = vmap.ContainsKey("map_name") ? vmap["map_name"]._str : "--";
    //        int basecost = xml.getInt("cost");
    //        curNeedMoney = basecost / 10 * (int)((float)PlayerModel.getInstance().lvl / 10) + basecost;
    //        textCostMoney.text = curNeedMoney.ToString();
    //        textTargetDesc.text = name;
    //        gameObject.SetActive(true);
    //    }

    //    public Vector3 SetTargetPosition(Vector2 v2targetPosition) => currentTargetPosition = new Vector3(v2targetPosition.x / GameConstant.GEZI_TRANS_UNITYPOS, 0, v2targetPosition.y / GameConstant.GEZI_TRANS_UNITYPOS);

    //    private void Init()
    //    {
    //        new BaseButton(transform.FindChild("bt0")).onClick = (GameObject go) =>
    //        {
    //            SelfRole.WalkToMap(Instance.targetMapId, Instance.currentTargetPosition);
    //            gameObject.SetActive(false);
    //            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
    //        };
    //        new BaseButton(transform.FindChild("bt1")).onClick = (GameObject go) =>
    //        {
    //            if (PlayerModel.getInstance().vip < 3 && PlayerModel.getInstance().money < curNeedMoney)
    //            {
    //                flytxt.instance.fly(ContMgr.getCont("comm_nomoney"));
    //                return;
    //            }
    //            SelfRole.Transmit(toid: Instance.transmitMapPoint, after: delegate ()
    //            {
    //                SelfRole.WalkToMap(Instance.targetMapId, Instance.currentTargetPosition);
    //            });
    //            gameObject.SetActive(false);
    //            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ELITEMON);
    //        };
    //        new BaseButton(transform.FindChild("btclose")).onClick = (GameObject go) =>
    //        {
    //            gameObject.SetActive(false);
    //        };
    //    }
    //}

    //class RewardItemTip
    //{
    //    public GameObject owner;
    //    public GameObject itemIcon;
    //    public Text textDesc;
    //    //public Text textLv;
    //    public GameObject closeBtn;
    //    public Text textName;

    //    public RewardItemTip(GameObject owner)
    //    {
    //        this.owner = owner;
    //        itemIcon = owner.transform.FindChild("text_bg/iconbg/icon").gameObject;
    //        textDesc = owner.transform.FindChild("text_bg/text").GetComponent<Text>();
    //        //textLv = owner.transform.FindChild("text_bg/nameBg/lv").GetComponent<Text>();
    //        textName = owner.transform.FindChild("text_bg/nameBg/itemName").GetComponent<Text>();
    //        new BaseButton(owner.transform.FindChild("close_btn")).onClick = (GameObject go) => owner.SetActive(false);
    //    }

    //    public void ShowItemTip(uint itemId)
    //    {
    //        if (itemIcon.transform.childCount > 0)
    //            for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
    //                GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
    //        IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
    //        SXML item = a3_BagModel.getInstance().getItemXml((int)itemId);
    //        textDesc.text = item.getString("desc");
    //        textName.text = item.getString("item_name");
    //    }
    //}

    //class RewardEquipTip
    //{
    //    public GameObject owner;
    //    public GameObject itemIcon;

    //    //道具名称:Text
    //    Text textItemName,
    //    //Tip描述:Text
    //         textTipDesc,
    //    //职业限制:Text
    //         textCarrLimit,
    //    //基本属性:Text
    //         textBaseHead,
    //         textBaseAttr,
    //    //追加属性:Text
    //         textAddAttr,
    //    //附加描述1:Text
    //         textExtraDesc1,
    //    //附加描述2:Text
    //         textExtraDesc2,
    //    //附加描述3:Text
    //         textExtraDesc3;
    //    List<Text> listTextExtraDesc;
    //    //背景颜色组:List<GameObject>
    //    List<GameObject> listGoBgImg;

    //    public RewardEquipTip(GameObject owner)
    //    {
    //        this.owner = owner;
    //        listGoBgImg = new List<GameObject>();
    //        listTextExtraDesc = new List<Text>();
    //        Transform tfBgImgParent = owner.transform.FindChild("bgImg");
    //        for (int i = 0, max = tfBgImgParent?.childCount ?? 0; i < max; i++)
    //            listGoBgImg.Add(tfBgImgParent.GetChild(i).gameObject);

    //        itemIcon = owner.transform.FindChild("text_bg/iconbg/icon").gameObject;
    //        textItemName = owner.transform.FindChild("text_bg/nameBg/itemName").GetComponent<Text>();
    //        textTipDesc = owner.transform.FindChild("text_bg/nameBg/tipDesc/dispText").GetComponent<Text>();
    //        textCarrLimit = owner.transform.FindChild("text_bg/nameBg/carrReq/dispText").GetComponent<Text>();
    //        textBaseHead = owner.transform.FindChild("text_bg/textBase/baseDisc").GetComponent<Text>();
    //        textBaseAttr = owner.transform.FindChild("text_bg/textBase/dispText").GetComponent<Text>();
    //        textAddAttr = owner.transform.FindChild("text_bg/textAddBase/dispText").GetComponent<Text>();
    //        listTextExtraDesc.Add(textExtraDesc1 = owner.transform.FindChild("text_bg/textTip1").GetComponent<Text>());
    //        listTextExtraDesc.Add(textExtraDesc2 = owner.transform.FindChild("text_bg/textTip2").GetComponent<Text>());
    //        listTextExtraDesc.Add(textExtraDesc3 = owner.transform.FindChild("text_bg/textTip3").GetComponent<Text>());

    //        new BaseButton(owner.transform.FindChild("close_btn")).onClick = (GameObject go) => owner.SetActive(false);
    //    }
    //    /// <summary>
    //    /// 根据道具品质显示对应的背景图片
    //    /// </summary>
    //    /// <param name="quality"> 道具品质 </param>
    //    /// <param name="beginWith"> 最低道具品质对应数值 </param>
    //    public void ShowBgImgByQuality(int quality, int beginWith = 0)
    //    {
    //        if (listGoBgImg.Count < quality - beginWith || quality < beginWith)
    //            return;
    //        else
    //        {
    //            for (int i = 0; i < listGoBgImg.Count; i++)
    //                listGoBgImg[i]?.SetActive(false);
    //            listGoBgImg[quality - beginWith]?.SetActive(true);
    //        }
    //    }

    //    public void ShowEquipTip(uint itemId)
    //    {
    //        if (itemIcon.transform.childCount > 0)
    //            for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
    //                GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
    //        textBaseHead.gameObject.SetActive(false);
    //        IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
    //        SXML item = a3_BagModel.getInstance().getItemXml((int)itemId);            
    //        textItemName.text = item.getString("item_name");

    //        //职业
    //        int carr = item.getInt("job_limit");
    //        switch (carr)
    //        {
    //            default:
    //            case -1:
    //                textCarrLimit.text = "无限制";
    //                break;
    //            case 2:
    //                textCarrLimit.text = "战士";
    //                break;
    //            case 3:
    //                textCarrLimit.text = "法师";
    //                break;
    //            case 5:
    //                textCarrLimit.text = "刺客";
    //                break;
    //        }

    //        //基础属性
    //        int attType = item.getInt("att_type");
    //        SXML itemInfo = XMLMgr.instance.GetSXML("item.stage", "stage_level==0");
    //        SXML eqpInfo = itemInfo.GetNode("stage_info", "itemid==" + itemId);
    //        if (eqpInfo != null)
    //        {
    //            switch (attType)
    //            {
    //                case 5:
    //                    string[] attackValue = eqpInfo.getString("basic_att").Split(',');
    //                    int minVal, maxVal;
    //                    if (attackValue.Length == 1)
    //                        maxVal = minVal = int.Parse(attackValue[0]);
    //                    else if (attackValue.Length == 2)
    //                    {
    //                        minVal = int.Parse(attackValue[0]);
    //                        maxVal = int.Parse(attackValue[1]);
    //                    }
    //                    else
    //                        return;
    //                    textBaseAttr.text = string.Format("攻击:{0}-{1}", minVal, maxVal);//"攻击"属性存在一个范围
    //                    break;
    //                default:
    //                    int val = eqpInfo.getInt("basic_att");
    //                    textBaseAttr.text = string.Format("{0}:{1}", Globle.getAttrNameById(attType), val);//其它属性是没有范围的
    //                    break;
    //            }
    //        }

    //        //基础追加属性
    //        textAddAttr.text = Globle.getAttrNameById(item.getInt("att_type"));

    //        //其它Tips说明
    //        SXML itemTip = item.GetNode("default_tip");
    //        if (itemTip == null)
    //        {
    //            Debug.LogError("未配置default_tip");
    //            return;
    //        }
    //        textTipDesc.text = itemTip.getString("equip_desc");
    //        List<SXML> listItemTipExtraDesc = itemTip.GetNodeList("random_tip");
    //        for (int i = 0; i < listItemTipExtraDesc.Count; i++)
    //        {
    //            if (i >= listTextExtraDesc.Count)
    //                break;
    //            listTextExtraDesc[i].text = listItemTipExtraDesc[i].getString("tip");
    //        }

    //        ShowBgImgByQuality(item.getInt("quality"), beginWith: 1);
    //    }
    //    /// <summary>
    //    /// 完全根据配置表来显示
    //    /// </summary>
    //    /// <param name="itemId">物品的id</param>
    //    public void ShowXMLCustomizedEquipTip(uint itemId)
    //    {
    //        SXML xmlEliteMonster = XMLMgr.instance.GetSXML("worldboss.mdrop", "mid==" + A3_EliteMonster.Instance.CurrentSelectedMonsterId);
    //        if (xmlEliteMonster != null)
    //        {
    //            if (itemIcon.transform.childCount > 0)
    //                for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
    //                    GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
    //            IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
    //            SXML itemEliteMonster = xmlEliteMonster.GetNode("item", "id==" + itemId);
    //            //道具名称
    //            textItemName.text = itemEliteMonster.GetNode("item_name").getString("tip");
    //            //Tip描述
    //            textTipDesc.text = "";
    //            //职业限制
    //            textCarrLimit.text = itemEliteMonster.GetNode("carr_limit").getString("tip");
    //            //基本属性
    //            textBaseHead.gameObject.SetActive(true);
    //            textBaseAttr.text = itemEliteMonster.GetNode("desc1").getString("tip");
    //            //追加属性
    //            textAddAttr.text = itemEliteMonster.GetNode("desc2").getString("tip");
    //            //附加描述1
    //            textExtraDesc1.text = itemEliteMonster.GetNode("random_tip1").getString("tip");
    //            //附加描述2
    //            textExtraDesc2.text = itemEliteMonster.GetNode("random_tip2").getString("tip");
    //            //附加描述3
    //            textExtraDesc3.text = itemEliteMonster.GetNode("random_tip3").getString("tip");
    //        }
    //    }
    //}
}
