using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Cross;


namespace MuGame
{
    class a3_washredname : Window
    {
        GameObject rule;
        Text now_point;
        Text des;
        Text needMoney;
        public static a3_washredname _instance;
        public override void init()
        {
            _instance = this;
            rule = transform.FindChild("rule_bg").gameObject;
            now_point = getComponentByPath<Text>("bg_top/txt/now_point");
            des = getComponentByPath<Text>("bg_top/des");
            needMoney = getComponentByPath<Text>("bg_downleft/num");
            BaseButton colse = new BaseButton(transform.FindChild("closeBtn"));
            colse.onClick = delegate(GameObject go)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_WASHREDNAME);
            };
            BaseButton showrule = new BaseButton(transform.FindChild("bg_top/rule_btn"));
            showrule.onClick = delegate(GameObject go)
            {
                rule.SetActive(true);
            };
            BaseButton colserule = new BaseButton(transform.FindChild("rule_bg/rule/close"));
            colserule.onClick = delegate(GameObject go)
            {
                rule.SetActive(false);
            };
            BaseButton cleanpointbtn = new BaseButton(transform.FindChild("bg_downleft/Button"));
            cleanpointbtn.onClick = cleanpoint;
            BaseButton cleanpointbtn1 = new BaseButton(transform.FindChild("bg_downright/Button"));
            cleanpointbtn1.onClick = cleanpoint1;

            getComponentByPath<Text>("bg_top/txt").text = ContMgr.getCont("a3_washredname_0");
            getComponentByPath<Text>("bg_top/des").text = ContMgr.getCont("a3_washredname_1");
            getComponentByPath<Text>("bg_downleft/Text").text = ContMgr.getCont("a3_washredname_2");
            getComponentByPath<Text>("bg_downleft/Button/Text").text = ContMgr.getCont("a3_washredname_3");
            getComponentByPath<Text>("bg_downright/Text").text = ContMgr.getCont("a3_washredname_4");
            getComponentByPath<Text>("bg_downright/redure").text = ContMgr.getCont("a3_washredname_5");
            getComponentByPath<Text>("bg_downright/Button/Text").text = ContMgr.getCont("a3_washredname_6");
            getComponentByPath<Text>("title/titleTxt").text = ContMgr.getCont("a3_washredname_7");
            getComponentByPath<Text>("rule_bg/rule/Text").text = ContMgr.getCont("a3_washredname_8");
  


        }
        public override void onShowed()
        {
            point();
        }
         public override void onClosed()
        {

        }
        public void point()
         {
             now_point.text = PlayerModel.getInstance().sinsNub.ToString();
             refreshpoint(PlayerModel.getInstance().sinsNub);
             needMoney.text = ContMgr.getCont("a3_washredname0") + PlayerModel.getInstance().sinsNub * 2 + ContMgr.getCont("a3_washredname1");
         }
        void refreshpoint(uint point)
        {
            if (point <= 15)
            {
                des.text = ContMgr.getCont("a3_washredname2");

            }
            else if(point>15&&point<=90)
            {
                des.text = ContMgr.getCont("a3_washredname3");
            }
            else if (point > 90 && point <= 150)
            {
                des.text = ContMgr.getCont("a3_washredname4");
            }
            else if (point > 150)
            {
                des.text = ContMgr.getCont("a3_washredname5");
            }
        
        }
        void cleanpoint(GameObject go)
        {
            a3_PkmodelProxy.getInstance().sendWashredname(1);
        }
        void cleanpoint1(GameObject go)
        {
            a3_PkmodelProxy.getInstance().sendWashredname(2);
        }
    }
    
}
