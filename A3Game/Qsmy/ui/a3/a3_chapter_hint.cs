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
namespace MuGame
{
    class a3_chapter_hint : Window
    {
        float animatime = 6f;
        static int chapid = -1;
        //Text desc;
        GameObject gg;
        Animator ant;
        Image title;
        Image nameI;
        Transform tfReward;
        GameObject tfPrefabIconBg;
        public static a3_chapter_hint instance;
        public static void ShowChapterHint(int id) {
            chapid = id;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHAPTERHINT);
        }

        public override void init()
        {
            inText();
            //desc = getComponentByPath<Text>("show/desc/Text");
            title = getComponentByPath<Image>("show/title/chap");
            nameI = getComponentByPath<Image>("show/desc/name");
            tfReward = transform.FindChild("show/reward");
            tfPrefabIconBg = transform.FindChild("show/template/rewardIconBg").gameObject;
            gg = getGameObjectByPath("show");
            ant = gg.GetComponent<Animator>();
            gg.SetActive(false);
        }


        void inText() {
            this.transform.FindChild("show/items_title/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_chapter_hint_1");
        }
        public override void onShowed()
        {
            instance = this;
            var vv = getTransformByPath("ig_bg_bg");
            if (vv != null) vv.gameObject.SetActive(false);
            CancelInvoke("CloseM");
            SetShow();
            InvokeRepeating("CloseM", 1, 0.1f);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_SHOW_ONLYWIN);
        }

        public override void onClosed()
        {
            instance = null;
            gg.SetActive(false);
            if (InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.NPC_TASK_TALK))
            {
                InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_STORY);
                if (npctalk.instance != null)
                    npctalk.instance.MinOrMax();
            }
            else
            {
                InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            }

            CancelInvoke("CloseM");
            a3_liteMinimap.instance.TaskBtn(null,false);
        }

        void OnDisable()
        {
            instance = null;
        }

        void SetShow()
        {
            if (chapid < 0) return;
            gg.SetActive(true);
            var cd = A3_TaskModel.getInstance().GetChapterInfosById(chapid);
            //desc.text = cd.description;
            title.sprite = GAMEAPI.ABUI_LoadSprite("icon_chapter_no" + chapid);
            title.SetNativeSize();
            nameI.sprite = GAMEAPI.ABUI_LoadSprite("icon_chapter_" + chapid);
            nameI.SetNativeSize();
            List<SXML> rewardItemList = new List<SXML>();
            rewardItemList.Add(XMLMgr.instance.GetSXML("task.Cha_gift","id=="+chapid).GetNode("RewardEqp", "carr==" + PlayerModel.getInstance().profession));
            rewardItemList.AddRange(XMLMgr.instance.GetSXML("task.Cha_gift", "id==" + chapid).GetNodeList("RewardItem"));
            for (int i = tfReward.childCount - 1; i > -1; i--)
            {
                Destroy(tfReward.GetChild(i).gameObject);
            }
            for (int i = 0; i < rewardItemList.Count; i++)
            {
                if (rewardItemList[i] == null) continue;
                uint item_id = rewardItemList[i].getUint("item_id");
                if (item_id == 0) item_id = rewardItemList[i].getUint("id");
                int num = rewardItemList[i].getInt("value");
                //if (num == -1) num = 1;
                GameObject bg = Instantiate(tfPrefabIconBg);
                Transform bgImg = bg.transform.FindChild("bg");
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_id, num: num,ignoreLimit: true);
                icon.transform.SetParent(bgImg, false);
                bg.transform.SetParent(tfReward, false);
                bgImg.GetComponent<RectTransform>().localPosition = Vector2.zero;
            }
            ii = 0;
        }

        float ii = 0;
        void CloseM()
        {
            ii += 0.1f;
            if (ant.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f || ii> animatime)
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHAPTERHINT);
        }
    }
}
