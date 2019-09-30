using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;

namespace MuGame
{
    class a3_teamApplyPanel:Window//有可能会有多个申请,如果队伍已满则不可以继续同意,否则
    {
        Transform itemApplyPanelFrefab;
        public static a3_teamApplyPanel mInstance;
        public override bool showBG
        {
            get
            {
                return false;
            }
        }
        override public void init()
        {
            mInstance = this;
            this.gameObject.SetActive(false);
            itemApplyPanelFrefab = transform.FindChild("body");





            getComponentByPath<Text>("body/title/Text").text = ContMgr.getCont("a3_teamInvitedPanel_0");
            getComponentByPath<Text>("body/body/Text").text = ContMgr.getCont("a3_teamInvitedPanel_1");
            getComponentByPath<Text>("body/bottom/btnOk/Text").text = ContMgr.getCont("a3_teamInvitedPanel_2");
            getComponentByPath<Text>("body/bottom/btnCancle/Text").text = ContMgr.getCont("a3_teamInvitedPanel_3");



        }
        public void Show(ItemTeamData itd)
        {
            this.gameObject.SetActive(true);
            this.transform.SetAsLastSibling();
            new itemTeamApplyPanel(itemApplyPanelFrefab, itd);
        }

        override public void onClosed()
        {
           
        }
     
        class itemTeamApplyPanel
        {
            Text txtInfor;
            ItemTeamData m_itd;
            GameObject m_go;
            Text txtCD;
            string strCD = "({0})";
            float cdTime = 8;
            TickItem showTime;
            public itemTeamApplyPanel(Transform trans, ItemTeamData itd)
            {
                m_go = GameObject.Instantiate(trans.gameObject) as GameObject;
                m_go.SetActive(true);
                m_go.transform.SetParent(trans.parent);
                m_go.transform.localScale = Vector3.one;
                m_go.transform.localPosition = Vector3.zero;
                m_go.transform.SetAsFirstSibling();
                txtInfor = m_go.transform.FindChild("body/Text").GetComponent<Text>();
                txtCD = m_go.transform.FindChild("body/txtCD").GetComponent<Text>();
                txtCD.text = string.Format(strCD, cdTime);
                showTime = new TickItem(onUpdates);
                TickMgr.instance.addTick(showTime);
                BaseButton btnClose = new BaseButton(m_go.transform.FindChild("title/btnClose"));
                BaseButton btnOk = new BaseButton(m_go.transform.FindChild("bottom/btnOk"));
                BaseButton btnCancle = new BaseButton(m_go.transform.FindChild("bottom/btnCancle"));
                btnCancle.onClick = onBtnCancleClick;
                btnClose.onClick = onBtnCloseClick;
                btnOk.onClick = onBtnOKClick;
                m_itd = itd;
                string name = itd.name;
                string carrName = A3_TeamModel.getInstance().getProfessional(itd.carr);
                uint zhuan = itd.zhuan;
                uint lvl = itd.lvl;
                txtInfor.text = string.Format(txtInfor.text, itd.name, carrName, zhuan, lvl);
                trans.transform.SetAsLastSibling();
            }
            void onUpdates(float s)
            {
                cdTime -= s;
                if (cdTime <= 0)
                {
                    TickMgr.instance.removeTick(showTime);
                    Destroy(m_go);
                }
                else
                {
                    txtCD.text = string.Format(strCD, (int)cdTime);
                }
            }
            void onBtnCloseClick(GameObject go)
            {
                TickMgr.instance.removeTick(showTime);
                TeamProxy.getInstance().SendAffirmApply(m_itd.cid, false);
                Destroy(m_go);
            }
            void onBtnCancleClick(GameObject go)
            {
                TickMgr.instance.removeTick(showTime);
                TeamProxy.getInstance().SendAffirmApply(m_itd.cid, false);
                Destroy(m_go);
            }
            void onBtnOKClick(GameObject go)
            {
                TickMgr.instance.removeTick(showTime);
                TeamProxy.getInstance().SendAffirmApply(m_itd.cid, true);
                Destroy(m_go);
            }
        }
    }
}
