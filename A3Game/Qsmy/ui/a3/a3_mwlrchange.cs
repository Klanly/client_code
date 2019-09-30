using Cross;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{


    public enum fb_type
    {
        gold,       //金币副本
        exp,        //经验副本
        mate,       //材料副本
        mlzd,       //魔炼之地
        slmj,       //兽灵秘境
        jjc,        //竞技场
        multi_gold, //109副本
        multi_exp,  //108副本
        multi_mate, //110副本
        multi_ghost //111副本


    }

    class a3_mwlrchange : Window
    {

        fb_type typse = new fb_type();
        public override void init()
        {
            inText();

            new BaseButton(getTransformByPath("yes")).onClick = (GameObject go) => { yesOnClick(go); };
            new BaseButton(getTransformByPath("no")).onClick = (GameObject go) => { noOnClick(go); };
        }

        void inText()
        {
            this.transform.FindChild("Image/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mwlrchange_1");//您现在正在进行魔物猎杀,该操作会放弃本次猎杀,是否确定?
            this.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mwlrchange_2");// 确定
            this.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mwlrchange_3");// 取消
        }

        public override void onShowed()
        {
            if (uiData != null)
            {
                typse = (fb_type)uiData[0];
            }
        }
        public override void onClosed()
        {
            base.onClosed();
        }


        void yesOnClick(GameObject go)
        {
            Relievemwlr();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_MWLRCHANGE);
            switch (typse)
            {
                case fb_type.gold:
                    a3_counterpart_gold.BtnsOnclick((int)uiData[1]);
                    break;
                case fb_type.exp:
                    a3_counterpart_exp.BtnsOnclick((int)uiData[1]);
                    break;
                case fb_type.mate:
                    a3_counterpart_mate.BtnsOnclick((int)uiData[1]);
                    break;
                case fb_type.mlzd:
                    a3_active_mlzd.instans.EnterOnClick();
                    break;
                case fb_type.slmj:
                    a3_active_zhsly.instance.EnterOnClick();
                    break;
                case fb_type.jjc:
                     a3_sports_jjc .instance.EnterOnClick();
                    break;
                case fb_type.multi_gold:
                    if (a3_counterpart._instance != null)
                        a3_counterpart._instance.TreamFb(Convert.ToUInt32(uiData[1]), Convert.ToString(uiData[2]), Convert.ToInt32(uiData[3]));
                    break;
                case fb_type.multi_exp:
                    if(a3_counterpart._instance!=null)
                       a3_counterpart._instance.TreamFb(Convert.ToUInt32(uiData[1]), Convert.ToString(uiData[2]), Convert.ToInt32(uiData[3]));
                    break;
                case fb_type.multi_mate:
                    if (a3_counterpart._instance != null)
                        a3_counterpart._instance.TreamFb(Convert.ToUInt32(uiData[1]), Convert.ToString(uiData[2]), Convert.ToInt32(uiData[3]));
                    break;
                case fb_type.multi_ghost:
                    if (a3_counterpart._instance != null)
                        a3_counterpart._instance.TreamFb(Convert.ToUInt32(uiData[1]), Convert.ToString(uiData[2]), Convert.ToInt32(uiData[3]));
                    break;
            }
        }

        void noOnClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_MWLRCHANGE);
        }

        void Relievemwlr()
        {
            A3_ActiveProxy.getInstance().SendGiveUpHunt();
        }
    }
}
