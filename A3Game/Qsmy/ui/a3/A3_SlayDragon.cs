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
    class A3_SlayDragon : Window
    {
        public static A3_SlayDragon Instance;
        public Transform rootDragonList;
        public Transform rootDragonOpt;
        public GameObject goDragonHelpTxt;
        private bool isOnMoveOpt = false;
        private string currentSelectedDragonName;
        private Text txtTimer;
        private GameObject goTimer;
        private bool isFrstdrgnInit = false;
        public override void init()
        {


            #region 初始化汉字
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1201").text = ContMgr.getCont("A3_SlayDragon_0");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1201/title_text").text = ContMgr.getCont("A3_SlayDragon_1");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1202").text = ContMgr.getCont("A3_SlayDragon_2");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1202/title_text").text = ContMgr.getCont("A3_SlayDragon_3");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1203").text = ContMgr.getCont("A3_SlayDragon_4");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1203/title_text").text = ContMgr.getCont("A3_SlayDragon_5");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1204").text = ContMgr.getCont("A3_SlayDragon_6");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1204/title_text").text = ContMgr.getCont("A3_SlayDragon_7");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1205").text = ContMgr.getCont("A3_SlayDragon_8");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1205/title_text").text = ContMgr.getCont("A3_SlayDragon_9");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1206").text = ContMgr.getCont("A3_SlayDragon_10");
            getComponentByPath<Text>("bg/dragon_opt/descBg/desc/1206/title_text").text = ContMgr.getCont("A3_SlayDragon_11");
            getComponentByPath<Text>("bg/dragon_opt/proc_unlock/proc_desc").text = ContMgr.getCont("A3_SlayDragon_12");
            getComponentByPath<Text>("bg/dragon_opt/timer/desc").text = ContMgr.getCont("A3_SlayDragon_13");
            getComponentByPath<Text>("bg/dragon_opt/reward/text").text = ContMgr.getCont("A3_SlayDragon_14");
            getComponentByPath<Text>("bg/dragon_opt/btn_do/Go/text_img/Text").text = ContMgr.getCont("A3_SlayDragon_15");
            getComponentByPath<Text>("bg/dragon_opt/btn_do/Create/text_img/Text").text = ContMgr.getCont("A3_SlayDragon_15");
            getComponentByPath<Text>("bg/dragon_opt/btn_do/Unlock/text_img/Text").text = ContMgr.getCont("A3_SlayDragon_16");
            getComponentByPath<Text>("hp/help_txt/desc/title").text = ContMgr.getCont("A3_SlayDragon_17");
            getComponentByPath<Text>("hp/help_txt/desc/dc").text = ContMgr.getCont("A3_SlayDragon_18");
            #endregion


            Instance = this;
            rootDragonList = transform.FindChild("bg/dragon_list/rect_mask/rect_scroll");
            for (int i = 0; i < rootDragonList.childCount; i++)
            {
                Transform tfDragonLine = rootDragonList.GetChild(i);
                A3_SlayDragonModel.getInstance().dicDragonName[i] = tfDragonLine.name;
                new BaseButton(tfDragonLine).onClick = OnDragonLineClick;
            }
            goDragonHelpTxt = transform.FindChild("hp/help_txt").gameObject;
            goTimer = transform.FindChild("bg/dragon_opt/timer").gameObject;
            txtTimer = goTimer.transform.FindChild("time").GetComponent<Text>();
            new BaseButton(transform.FindChild("bg/dragon_opt/btn_do/Go")).onClick = OnGoToSlayDragon;
            new BaseButton(transform.FindChild("bg/dragon_opt/btn_do/Unlock")).onClick = OnUnlockDragon;
            new BaseButton(transform.FindChild("bg/dragon_opt/btn_do/Create")).onClick = (go) => { OnCreateDragon(go); OnGoToSlayDragon(go); };
            new BaseButton(transform.FindChild("bg/dragon_opt/proc_unlock/btn_give")).onClick = OnGive;
            new BaseButton(transform.FindChild("close")).onClick = (go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_SLAY_DRAGON);
            new BaseButton(transform.FindChild("hp")).onClick = (go) => goDragonHelpTxt.SetActive(true);
            new BaseButton(goDragonHelpTxt.transform.FindChild("close_area")).onClick = (go) => goDragonHelpTxt.SetActive(false);
            rootDragonOpt = transform.FindChild("bg/dragon_opt");
            new BaseButton(rootDragonOpt.FindChild("reward/reward_icon")).onClick = (go) => 
            {
                uint dragonId = A3_SlayDragonModel.getInstance().dicDragonData[currentSelectedDragonName].dragonId;
                uint itemId = A3_SlayDragonModel.getInstance().GetRewardIdByDragonId(dragonId);
                if (itemId == 0) return;
                ArrayList arr = new ArrayList();
                arr.Add(itemId);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            };

            A3_SlayDragonProxy.getInstance().addEventListener(A3_SlayDragonProxy.REFRESH, OnRefresh);
            A3_SlayDragonProxy.getInstance().addEventListener(A3_SlayDragonProxy.OPEN_LVL, OnOpenLvl);
        }

        private void ShowFirstDragon()
        {
            OnDragonLineClick(rootDragonList.GetChild(0).gameObject);
            isFrstdrgnInit = true;
        }

        public override void onShowed()
        {
            Instance = this;
            A3_SlayDragonProxy.getInstance().addEventListener(A3_SlayDragonProxy.END_TIME, OnRefreshTime);
            if (!IsInvoking("RunTimer"))
                InvokeRepeating("RunTimer", 0f, 1f);
            // if (!PlayerModel.getInstance().inFb) //副本中不发送协议
            Invoke("ShowFirstDragon", 0.2f);
            A3_SlayDragonProxy.getInstance().SendGetData();
        }

        public override void onClosed()
        {
            A3_SlayDragonProxy.getInstance().removeEventListener(A3_SlayDragonProxy.END_TIME, OnRefreshTime);
            OnDragonLineClick(rootDragonList.GetChild(0).gameObject);
            CancelInvoke("RunTimer");
            isFrstdrgnInit = false;
            Instance = null;
        }

        private void OnRefreshTime(GameEvent e)
        {
            if (!e.data.ContainsKey("end_time") || e.data["end_time"] == 0)
                return;
            else
            {
                A3_SlayDragonModel.getInstance().GetUnlockedDragonData().endTimeStamp = e.data["end_time"];
                if (!IsInvoking("RunTimer"))
                    InvokeRepeating("RunTimer", 0f, 1f);
            }
        }
        private void RunTimer()
        {
            DragonData unlockedDrgnData = A3_SlayDragonModel.getInstance()?.GetUnlockedDragonData();
            if (goTimer.activeSelf && unlockedDrgnData != null && unlockedDrgnData.endTimeStamp != 0)
            {
                long currentTime = muNetCleint.instance.CurServerTimeStamp;
                long span = unlockedDrgnData.endTimeStamp - currentTime;
                if (span > 0)
                    txtTimer.text = string.Format("{0:D2}:{1:D2}", span % 3600 / 60, span % 60);
                else
                    txtTimer.text = "00:00";
            }
        }
        private void OnRefresh(GameEvent e)
        {
            if (!e.data.ContainsKey("tulong_lvl_ary")) return;
            A3_SlayDragonModel.getInstance().SyncData(e.data);
            if (Instance != null && currentSelectedDragonName == null)
                OnDragonLineClick(rootDragonList.GetChild(0).gameObject);
            List<Variant> listDragonData = e.data["tulong_lvl_ary"]._arr;
            uint dragonId = A3_SlayDragonModel.getInstance().GetIdByName(currentSelectedDragonName);
            for (int i = 0; i < listDragonData.Count; i++)
            {
                uint curDrgnId = listDragonData[i]["lvl_id"];
                bool isUnlocked = listDragonData[i]["zhaohuan"];
                if (dragonId != 0 && curDrgnId == dragonId && isUnlocked)
                    RefreshDragonInfo(currentSelectedDragonName);
                string drgnName = A3_SlayDragonModel.getInstance().GetNameById(curDrgnId);
                Transform curDragLn = rootDragonList.FindChild(drgnName);
                bool isDead = listDragonData[i]["death"], isOpened = listDragonData[i]["open"], isCreated = listDragonData[i]["create_tm"];
                curDragLn.FindChild("state/killed").gameObject.SetActive(isDead);
                curDragLn.FindChild("state/unlocked").gameObject.SetActive(isUnlocked && !isDead);
                curDragLn.FindChild("state/locked").gameObject.SetActive(!isUnlocked && !isDead);
            }
            if (!isFrstdrgnInit && InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_SLAY_DRAGON))
                ShowFirstDragon();
        }

        private void OnOpenLvl(GameEvent e)
        {
            if (!isOnMoveOpt && !PlayerModel.getInstance().inFb)
                A3_TaskOpt.Instance.ShowDragonWin();
            isOnMoveOpt = false;
        }

        private void OnGoToSlayDragon(GameObject go)
        {
            if (A3_LegionModel.getInstance().myLegion.clanc < 3 && !A3_SlayDragonModel.getInstance().GetUnlockedDragonData().isOpened)
            {
                flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_noopen"));
                return;
            }
            if (0 != A3_SlayDragonModel.getInstance().GetUnlockedDragonId())
            {
                isOnMoveOpt = true;
                A3_SlayDragonProxy.getInstance().SendGo();
            }
            else
                flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_jf"));
        }

        private void OnUnlockDragon(GameObject go)
        {
            if (A3_LegionModel.getInstance().myLegion.clanc >= 3)
            {
                if (currentSelectedDragonName != null)
                {
                    DragonData dragonData = A3_SlayDragonModel.getInstance().dicDragonData[currentSelectedDragonName];
                    if (!dragonData.isUnlcoked && 0 == A3_SlayDragonModel.getInstance().GetUnlockedDragonId())
                        A3_SlayDragonProxy.getInstance().SendUnlock(dragonData.dragonId);
                    else
                        flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_nomany"));
                }
            }
            else
                flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_kustllx"));
        }

        private void OnCreateDragon(GameObject go)
        {
            uint drgnId = A3_SlayDragonModel.getInstance().dicDragonData[currentSelectedDragonName].dragonId;
            int diff_lvl = A3_SlayDragonModel.getInstance().GetUnlockedDiffLv();
            A3_SlayDragonProxy.getInstance().SendCreate(drgnId, diff_lvl);
        }

        private void OnDragonLineClick(GameObject go)
        {
            if (!A3_SlayDragonModel.getInstance().dicDragonData.ContainsKey(go.name)) return;
            for (int i = 0; i < rootDragonList.childCount; i++)
                rootDragonList.GetChild(i).FindChild("select").gameObject.SetActive(false);
            currentSelectedDragonName = go.name;
            bool isUnlocked = A3_SlayDragonModel.getInstance().dicDragonData[currentSelectedDragonName].isUnlcoked;
            go.transform.FindChild("select").gameObject.SetActive(true);
            if (isUnlocked)
            {
                go.transform.FindChild("select/unlocked").gameObject.SetActive(true);
                go.transform.FindChild("select/locked").gameObject.SetActive(false);
            }
            else
            {
                go.transform.FindChild("select/unlocked").gameObject.SetActive(false);
                go.transform.FindChild("select/locked").gameObject.SetActive(true);
            }
            RefreshDragonInfo(currentSelectedDragonName);
        }

        private void RefreshDragonInfo(string dragonName)
        {
            DragonData dragonData = A3_SlayDragonModel.getInstance().dicDragonData[dragonName];
            int cost = A3_SlayDragonModel.getInstance().GetCost();
            uint dragonId = dragonData.dragonId;
            uint proc = dragonData.proc;
            bool isUnlocked = dragonData.isUnlcoked,
                isOpened = dragonData.isOpened,
                isDead = dragonData.isDead,
                isCreated = dragonData.isCreated && !isDead;
            if (isCreated || isDead)
            {
                goTimer.SetActive(isCreated);
                rootDragonOpt.FindChild("proc_unlock").gameObject.SetActive(false);
            }
            else
            {
                goTimer.SetActive(false);
                rootDragonOpt.FindChild("proc_unlock").gameObject.SetActive(true);
                rootDragonOpt.FindChild("proc_unlock/proc_text").GetComponent<Text>().text = string.Format("{0}/{1}", proc, cost);
                rootDragonOpt.FindChild("proc_unlock/Slider").GetComponent<Slider>().value = proc / (float)cost;
            }

            string strDragonId = dragonId.ToString();
            Transform tfDesc = rootDragonOpt.FindChild("descBg/desc");
            for (int i = 0; i < tfDesc.childCount; i++)
            {
                GameObject goDesc = tfDesc.GetChild(i).gameObject;
                if (!goDesc.name.Equals(strDragonId))
                    goDesc.SetActive(false);
                else
                    goDesc.SetActive(true);
            }
            if (isDead)
            {
                rootDragonOpt.FindChild("btn_do/Create").gameObject.SetActive(false);
                rootDragonOpt.FindChild("btn_do/Unlock").gameObject.SetActive(false);
                rootDragonOpt.FindChild("btn_do/Go").GetComponent<Button>().interactable = false;
            }
            else
            {
                if (A3_LegionModel.getInstance().myLegion.clanc < 3)
                    rootDragonOpt.FindChild("btn_do/Create").gameObject.SetActive(false);
                rootDragonOpt.FindChild("btn_do/Go").GetComponent<Button>().interactable = true;
                if (isUnlocked)
                {
                    if (!isOpened)
                    {
                        rootDragonOpt.FindChild("btn_do/Unlock").gameObject.SetActive(false);
                        rootDragonOpt.FindChild("btn_do/Create").gameObject.SetActive(!isCreated);
                        rootDragonOpt.FindChild("btn_do/Create").GetComponent<Button>().interactable = proc >= cost;
                    }
                    else
                    {
                        rootDragonOpt.FindChild("btn_do/Create").gameObject.SetActive(false);
                        rootDragonOpt.FindChild("btn_do/Unlock").gameObject.SetActive(false);
                        rootDragonOpt.FindChild("btn_do/Go").gameObject.SetActive(isCreated);
                    }
                }
                else
                {
                    if (!isCreated)
                    {
                        rootDragonOpt.FindChild("btn_do/Unlock").gameObject.SetActive(true);
                        rootDragonOpt.FindChild("btn_do/Unlock").GetComponent<Button>().interactable = A3_SlayDragonModel.getInstance().IsAbleToUnlock();
                        rootDragonOpt.FindChild("btn_do/Go").gameObject.SetActive(false);
                    }
                }
            }
        }


        private void OnGive(GameObject go)
        {
            uint unlockedDragonId = 0;
            DragonData curDrgnData = A3_SlayDragonModel.getInstance().dicDragonData[currentSelectedDragonName];
            if (0 != (unlockedDragonId = A3_SlayDragonModel.getInstance().GetUnlockedDragonId()) && unlockedDragonId == curDrgnData.dragonId)
            {
                uint itemId = A3_SlayDragonModel.getInstance().GetDragonKeyId();
                if (0 != itemId)
                    if (0 < a3_BagModel.getInstance().getItemNumByTpid(itemId))
                    {
                        if (curDrgnData.proc < A3_SlayDragonModel.getInstance().GetCost())
                            A3_SlayDragonProxy.getInstance().SendGive();
                        else
                            flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_can"));
                    }
                    else
                    {
                        ArrayList data = new ArrayList();
                        data.Add(a3_BagModel.getInstance().getItemDataById(itemId));
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data);
                    }
            }
            else
            {
                if(A3_LegionModel.getInstance().myLegion.clanc < 3)
                    flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_jf"));
                else
                    flytxt.instance.fly(ContMgr.getCont("A3_SlayDragon_please"));
            }
        }
    }
}
