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
    class a3_getJewelryWay : Window
    {
        BaseButton btn_close;//关闭按钮
        Text cs_mwlr;
        Text cs_jjc;
        Text cs_dgsl;
        BaseButton mwlr_bt;
        BaseButton jjc_bt;
        BaseButton dgsl_bt;
        public static a3_getJewelryWay instance;
        public override void init()
        {
            instance = this;
            inText();
            btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onBtnCloseClick;

            mwlr_bt = new BaseButton(getTransformByPath("cells/scroll/content/mwlr/go"));
            mwlr_bt.onClick = mwlr_go;
            jjc_bt = new BaseButton(getTransformByPath("cells/scroll/content/jjc/go"));
            jjc_bt.onClick = jjc_go;
            dgsl_bt = new BaseButton(getTransformByPath("cells/scroll/content/dgsl/go"));
            dgsl_bt.onClick = dgsl_go;
            cs_mwlr = getTransformByPath("cells/scroll/content/mwlr/name/dj").GetComponent<Text>();
            cs_jjc = getTransformByPath("cells/scroll/content/jjc/name/dj").GetComponent<Text>();
            cs_dgsl = getTransformByPath("cells/scroll/content/dgsl/name/dj").GetComponent<Text>();

        }

        void inText()
        {
            this.transform.FindChild("bg/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_1");//首饰获取
            this.transform.FindChild("cells/scroll/content/mwlr/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_2");//魔物猎人
            this.transform.FindChild("cells/scroll/content/mwlr/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_3");// 完成猎杀任务可获得奖励箱子。
            this.transform.FindChild("cells/scroll/content/mwlr/go/text").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_getJewelryWay_4"));//立即{n}前往

            this.transform.FindChild("cells/scroll/content/jjc/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_5");//竞技场
            this.transform.FindChild("cells/scroll/content/jjc/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_6");//  赢得竞技场战斗可获得奖励箱子。
            this.transform.FindChild("cells/scroll/content/jjc/go/text").GetComponent<Text>().text = StringUtils.formatText( ContMgr.getCont("uilayer_a3_getJewelryWay_4"));//立即{n}前往

            this.transform.FindChild("cells/scroll/content/dgsl/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_7");//地宫首领
            this.transform.FindChild("cells/scroll/content/dgsl/des").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_getJewelryWay_8");//  击杀地宫首领可获得奖励。
            this.transform.FindChild("cells/scroll/content/dgsl/go/text").GetComponent<Text>().text = StringUtils.formatText(ContMgr.getCont("uilayer_a3_getJewelryWay_4"));//立即{n}前往
        }

        public string closeWin = null;
        public override void onShowed()
        {
            towin = false;    
            if (uiData!=null&&uiData.Count > 0)
            {
                closeWin = (string)uiData[0];
            }
            this.transform.SetAsLastSibling();
            refresh();
        }

        bool towin = false;
        public override void onClosed()
        {
            if (!towin)
            {
                closeWin = null;
            }
        }
        void refresh()
        {
            //mwlr
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DEVIL_HUNTER))
            {
                if (A3_ActiveModel.getInstance().mwlr_charges <= 0 )
                    mwlr_bt.gameObject.GetComponent<Button>().interactable = false;
                else
                    mwlr_bt.gameObject.GetComponent<Button>().interactable = true;
                cs_mwlr.text = "(" + A3_ActiveModel.getInstance().mwlr_charges + "/" + 10 + ")";
            }
            else
                this.transform.FindChild("cells/scroll/content/mwlr").gameObject.SetActive(false);
            //jjc
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PVP_DUNGEON))
            //A3_ActiveProxy.getInstance().SendPVP(6);
            {
                cs_jjc.text = "(" + (a3_sportsModel.getInstance().callenge_cnt - a3_sportsModel.getInstance().pvpCount + a3_sportsModel.getInstance().buyCount) + "/" + a3_sportsModel.getInstance().callenge_cnt + ")";
                if ((a3_sportsModel.getInstance().callenge_cnt - a3_sportsModel.getInstance().pvpCount + a3_sportsModel.getInstance().buyCount) <= 0)
                    jjc_bt.gameObject.GetComponent<Button>().interactable = false;
                else
                    jjc_bt.gameObject.GetComponent<Button>().interactable = true;
            }
            else
                this.transform.FindChild("cells/scroll/content/jjc").gameObject.SetActive(false);
            //dgsl
            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
            {
                cs_dgsl.text = "";
            }
            else
                this.transform.FindChild("cells/scroll/content/dgsl").gameObject.SetActive(false);
        }

        public void setTxt_jjc(bool b)
        {
            cs_jjc.text = "(" + (A3_ActiveModel.getInstance().callenge_cnt - A3_ActiveModel.getInstance().pvpCount + A3_ActiveModel.getInstance().buyCount) + "/" + A3_ActiveModel.getInstance().callenge_cnt + ")";
            if (b)
            {
                if ((A3_ActiveModel.getInstance().callenge_cnt - A3_ActiveModel.getInstance().pvpCount + A3_ActiveModel.getInstance().buyCount) <= 0)
                    jjc_bt.gameObject.GetComponent<Button>().interactable = false; 
                else
                    jjc_bt.gameObject.GetComponent<Button>().interactable = true;     
            }
            else {
                jjc_bt.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        public void onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETJEWELRYWAY);
        }
        void mwlr_go(GameObject go)
        {
            towin = true;
            ArrayList dl = new ArrayList();
            dl.Add("mwlr");
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETJEWELRYWAY);
            InterfaceMgr.getInstance().close(closeWin);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
        }
        void jjc_go(GameObject go)
        {
            towin = true;
            ArrayList dl = new ArrayList();
            dl.Add("sports_jjc");
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETJEWELRYWAY);
            InterfaceMgr.getInstance().close(closeWin);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SPORTS , dl);
        }

        void dgsl_go(GameObject go)
        {
            towin = true;
            ArrayList arr1 = new ArrayList();
            arr1.Add(ELITE_MONSTER_PAGE_IDX.BOSSPAGE);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_GETJEWELRYWAY);
            InterfaceMgr.getInstance().close(closeWin);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON, arr1);
        }
    }
}
