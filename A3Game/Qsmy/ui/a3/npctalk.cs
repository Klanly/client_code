using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using Cross;

namespace MuGame
{

    class npctalk : StoryUI
    {
       // private RectTransform tranMain;
        protected Text txtName;
        protected Text txtDesc;
        protected GameObject fg;
        public  List<Transform> lUiPos = new List<Transform>();
        Transform transTalk;
        GameObject teach;
		Transform bg;
        Transform name;
        Animator aniTalk;
        public static npctalk instance;
        public override void init()
        {
            transTalk = getTransformByPath("talk");
            aniTalk = transTalk.GetComponent<Animator>();
            teach = getGameObjectByPath("talk/teach");
            txtName = getComponentByPath<Text>("talk/txtname");
            txtDesc = getComponentByPath<Text>("talk/txtdesc");
            fg = getGameObjectByPath("talk/fg");
			bg = getTransformByPath("talk/bg");
            name = getTransformByPath("talk/namebg");
            fg.GetComponent<RectTransform>().sizeDelta = new Vector2(Baselayer.uiWidth * 1.5f, Baselayer.uiHeight * 1.5f);
            lUiPos.Add(getTransformByPath("con1"));
            lUiPos.Add(getTransformByPath("con0"));
            OnInit();

            //解决有的手机上第一次npc对话不显示的bug
            this.gameObject.SetActive(false);

            CancelInvoke("showui_phone");
            Invoke("showui_phone", 0.1f);
        }

        public override void onShowed()
        {
            instance = this;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_STORY);
            //teach.SetActive(SelfRole.s_bStandaloneScene);
            EventTriggerListener.Get(fg).onClick = onClick;            
            showDesc();
            transTalk.gameObject.SetActive(true);
        }
        public void MinOrMax(bool b = true)
        {
            if (b) this.transform.localScale = Vector3.one;
            else this.transform.localScale = Vector3.zero;
        }

        public void showui_phone()
        {
            this.transform.localScale = Vector3.one;
            this.gameObject.SetActive(true);
            refreshPos();
        }

        public override void onClosed()
        {
            instance = null;
            //  EventTriggerListener.Get(fg).clearAllListener();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            OnClosedProcess();
        }

        public virtual void OnClosedProcess()
        {

        }

        public virtual void OnInit()
        {
 
        }

        public virtual void onClick(GameObject go)
        {
            dialog.next();
        }

        public virtual void refreshPos()
        {
            if (dialog.curType == "0")
            {
                txtName.text = PlayerModel.getInstance().name;
                dialog.instance.GetPlayerCamRdy();
                dialog.instance.showRole(true);
                transTalk.position = lUiPos[0].position;
				bg.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                name.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                txtName.alignment = TextAnchor.LowerLeft;
                var tt = transform.FindChild("talk/txtdesc");
				tt.position = new Vector3(lUiPos[0].position.x, tt.position.y, 0);
                aniTalk?.SetTrigger("left");

            }
            else if (dialog.curType == "1")
            {
                txtName.text = dialog.m_npc.name;
                dialog.instance.GetNPCCamRdy();
                dialog.instance.showRole(false);
                transTalk.position = lUiPos[1].position;
				bg.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                name.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                txtName.alignment = TextAnchor.LowerRight;
                var tt = transform.FindChild("talk/txtdesc");
				tt.position = new Vector3(lUiPos[1].position.x, tt.position.y, 0);
                aniTalk?.SetTrigger("right");
            }

         
        }

        public  void showDesc()
        {
            refreshPos();
            refreshView();
        }


        public virtual void refreshView()
        {
            txtDesc.text = dialog.curDesc;
        }
    }
}
