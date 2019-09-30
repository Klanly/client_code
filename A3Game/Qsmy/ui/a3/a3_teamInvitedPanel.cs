using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;

namespace MuGame
{
    class a3_teamInvitedPanel : Window//只显示一个,如果已经加入队伍了,就不再显示,没加入则弹第二个
    {
        Text txtInfor;
        ItemTeamData m_itd;
        Text txtCD;
        string strCD = "({0})";
        float cdTime = 30;
        TickItem showTime;
        public static a3_teamInvitedPanel mInstance;
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
            txtInfor = transform.FindChild("body/body/Text").GetComponent<Text>();
            txtCD = transform.FindChild("body/body/txtCD").GetComponent<Text>();
            BaseButton btnClose = new BaseButton(transform.FindChild("body/title/btnClose"));
            BaseButton btnOk = new BaseButton(transform.FindChild("body/bottom/btnOk"));
            BaseButton btnCancle = new BaseButton(transform.FindChild("body/bottom/btnCancle"));
            btnCancle.onClick = onBtnCancleClick;
            btnClose.onClick = onBtnCloseClick;
            btnOk.onClick = onBtnOKClick;


            getComponentByPath<Text>("body/title/Text").text = ContMgr.getCont("a3_teamInvitedPanel_0");
            getComponentByPath<Text>("body/body/Text").text = ContMgr.getCont("a3_teamInvitedPanel_1");
            getComponentByPath<Text>("body/bottom/btnOk/Text").text = ContMgr.getCont("a3_teamInvitedPanel_2");
            getComponentByPath<Text>("body/bottom/btnCancle/Text").text = ContMgr.getCont("a3_teamInvitedPanel_3");


        }


        override public void onShowed()
        {
            if (uiData != null)
            {
                cdTime = 30;

                txtCD.text = string.Format(strCD, cdTime);
                //showTime = new TickItem(onUpdates);
                //TickMgr.instance.addTick(showTime);
                CancelInvoke("showtime");
                InvokeRepeating("showtime", 0, 1);
                ItemTeamData itd = (ItemTeamData)uiData[0];
                m_itd = itd;
                string name = itd.name;
                string carr = A3_TeamModel.getInstance().getProfessional(itd.carr);
                uint zhuan = itd.zhuan;
                uint lvl = itd.lvl;
                //txtInfor.text = string.Format(txtInfor.text, name, carr, zhuan, lvl);
                txtInfor.text = ContMgr.getCont("a3_teamInvitedPanel_1", new List<string> { name, carr, zhuan.ToString(), lvl.ToString() });
            }
            transform.SetAsLastSibling();
            InterfaceMgr.getInstance().closeAllWin(new List<string> {this.uiName  });
        }
        override public void onClosed()
        {
            CancelInvoke("showtime");
            cdTime = 30;
            //if (showTime!=null)
            //{
            //    TickMgr.instance.removeTick(showTime);
            //}
        }
        void showtime()
        {
            cdTime -= 1;
            if (cdTime <= 0)
            {
           
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMINVITEDPANEL);
                CancelInvoke("showtime");
            }
            else
            {
                txtCD.text = string.Format(strCD, (int)cdTime);
            }
        }
        //void onUpdates(float s)
        //{
        //    print("s是多少："+s);
        //    print("cdTime是多少:" + cdTime);
        //    cdTime -=s;
        //    if (cdTime <= 0)
        //    {
        //        TickMgr.instance.removeTick(showTime);
        //        InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMINVITEDPANEL);
        //    }
        //    else
        //    {
        //        txtCD.text = string.Format(strCD, (int)cdTime);
        //    }
        //}
        void onBtnCloseClick(GameObject go)
        {
            //TickMgr.instance.removeTick(showTime);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMINVITEDPANEL);
        }
        void onBtnCancleClick(GameObject go)
        {
            TeamProxy.getInstance().SendAffirmInvite(m_itd.cid, m_itd.teamId, false);
           // TickMgr.instance.removeTick(showTime);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMINVITEDPANEL);
        }
        void onBtnOKClick(GameObject go)
        {
            a3_expbar.instance.showBtnTeamTips(false);
           // TickMgr.instance.removeTick(showTime);
            TeamProxy.getInstance().SendAffirmInvite(m_itd.cid, m_itd.teamId, true);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMINVITEDPANEL);
        }


    }
}
