using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MuGame
{
    class BaseShejiao : Skin
    {
        public BaseShejiao(Transform trans) : base(trans) { }
        virtual public void onShowed(){}
        virtual public void onClose(){}
    }

    class a3_shejiao : Window
    {
        private TabControl tab;
        public static a3_shejiao instance;
        public TabControl Tab => tab;
        private Transform con;
        private BaseShejiao current = null;
		private BaseShejiao friend = null;
		private BaseShejiao legion = null;
        private BaseShejiao teamList = null;
        private BaseShejiao currentTeam = null;

        public override void init()
        {

            getComponentByPath<Text>("tab/knightage/Text").text = ContMgr.getCont("a3_shejiao_0");
            getComponentByPath<Text>("tab/friend/Text").text = ContMgr.getCont("a3_shejiao_1");
            getComponentByPath<Text>("tab/team/Text").text = ContMgr.getCont("a3_shejiao_2");



            con = getTransformByPath("con");
            instance = this;
            tab = new TabControl();
            tab.onClickHanle = OnSwitch;
            tab.create(getGameObjectByPath("tab"), this.gameObject);

            BaseButton closeBtn = new BaseButton(getTransformByPath("close"));
            closeBtn.onClick = OnClose;

            UIClient.instance.addEventListener(UI_EVENT.ON_OPENFRIENDPANEL,OnOpenPanel);
        }

        public override void onShowed()
        {
            if (uiData==null)
            {
                if (current != null)
                {
                    current.onShowed();
                }
                else
                {
                    tab.setSelectedIndex(0);
                    OnSwitch(tab);
                }
            }
            else
            {
                int index = (int)uiData[0];
                if (uiData.Count > 1)
                {
                    A3_LegionModel.getInstance().showtype = (int)uiData[1];
                }
                tab.setSelectedIndex(index);
                OnSwitch(tab);
            }
            if (teamList != null && (teamList==current||currentTeam==current))
            {
                TeamProxy.getInstance().SetTeamPanelInfo();
            }
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);
            Toclose = false;
            UiEventCenter.getInstance().onWinOpen(uiName);
        }

        public override void onClosed()
        {
            if (current != null)
            {
                current.onClose();
            }
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            //if (a3_itemLack.intans && a3_itemLack.intans.closewindow != null )
            //{
            //    if (Toclose)
            //    {
            //        if (a3_itemLack.intans.closewindow != this.uiName)
            //            InterfaceMgr.getInstance().open(a3_itemLack.intans.closewindow);
            //        a3_itemLack.intans.closewindow = null;
            //        Toclose = false;
            //    }
            //    else {
            //        if (!a3_itemLack.intans.noclear)
            //        {
            //            a3_itemLack.intans.closewindow = null;
            //            a3_itemLack.intans.noclear = false;
            //        }
            //    }
            //}
            InterfaceMgr.getInstance().itemToWin(Toclose,this.uiName);
        }

        public void SetIndexToFriend()
        {
            if (friend == null)
            {
                GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_friend");
                GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                friend = new a3_friend(panel.transform);
                friend.setPerent(con);
            }
            current = friend;
        }
        private void OnSwitch(TabControl t)
        {
            int index = t.getSeletedIndex();
            if (current != null)
            {
                if (currentTeam != null && teamList!=null)
                {
                    currentTeam.onClose();
                    currentTeam.gameObject.SetActive(false);
                    teamList.onClose();
                    teamList.gameObject.SetActive(false);
                }
              
                current.onClose();
                current.gameObject.SetActive(false);
            }

            if (index == 0)
            {
				if (legion == null) {
                    GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_legion");
                    GameObject panel = GameObject.Instantiate(prefab) as GameObject;
					legion = new a3_legion(panel.transform);
					legion.setPerent(con);
                    legion.transform.localPosition = Vector3.zero;
				}
				current = legion;
            }
            else if (index == 1)
            {
                SetIndexToFriend();
            }
			else {

                if (currentTeam == null)
                {
                    GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_currentTeamPanel");
                    GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                    currentTeam = new a3_currentTeamPanel(panel.transform);
                    currentTeam.setPerent(con);
                }
                if (teamList == null)
                {
                    GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_teamPanel");
                    GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                    teamList = new a3_teamPanel(panel.transform);
                    teamList.setPerent(con);
                }
                if (TeamProxy.getInstance().joinedTeam && TeamProxy.getInstance().MyTeamData!=null)
                {
                    current = currentTeam;
                    currentTeam.gameObject.SetActive(true);
                    teamList.gameObject.SetActive(false);
                }
                else
                {
                    current = teamList;
                    currentTeam.gameObject.SetActive(false);
                    teamList.gameObject.SetActive(true);
                }
                TeamProxy.getInstance().SetTeamPanelInfo();
              
			}

            if (current != null)
            {
                current.onShowed();
                current.visiable = true;
            }
        }

        bool Toclose = false;//是否是点击关闭按钮关闭
        void OnClose(GameObject go)
        {
            if (currentTeam != null && teamList != null)
            {
                currentTeam.onClose();
                currentTeam.gameObject.SetActive(false);
                teamList.onClose();
                teamList.gameObject.SetActive(false);
            }
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);

        }
        public void ShowFriend(ArrayList dat)
        {


        }
        void OnOpenPanel(GameEvent e)
        {
            int index = e.data["index"];
            tab.setSelectedIndex(index);
            OnSwitch(tab);
        }
    }
}
