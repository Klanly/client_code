using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using UnityEngine.EventSystems;
using System.Collections;

namespace MuGame
{
	class a3_blessing : Window
	{
        Text m_left_num1, m_left_num2, m_addAll;
        int m_ybnum, m_ybbdnum, m_ybcost, m_ybbdcost;

        public override void init() {

            SXML xml = XMLMgr.instance.GetSXML("acution.buy_state");
            m_ybnum = xml.getInt("yb_cnt");
            m_ybbdnum = xml.getInt("bndyb_cnt");
            m_ybcost = xml.getInt("yb_cost");
            m_ybbdcost = xml.getInt("bndyb_cost");

            m_left_num1 = this.transform.FindChild("info/addatk/left_num1").GetComponent<Text>();
            m_left_num2 = this.transform.FindChild("info/addatk/left_num2").GetComponent<Text>();
            m_addAll = this.transform.FindChild("info/atk/all_txt").GetComponent<Text>();

            inText();

			new BaseButton(transform.FindChild("touch")).onClick = (GameObject go) => {
				InterfaceMgr.getInstance().close(InterfaceMgr.A3_BLESSING);
			};
			new BaseButton(transform.FindChild("do")).onClick = (GameObject go) => {
                if (m_ybbdcost > PlayerModel.getInstance().gift)
                {
                    flytxt.instance.fly(ContMgr.getError("-1006"));
                    return;
                }
				A3_ActiveProxy.getInstance().SendGetBlessing(4);
			};
            new BaseButton(transform.FindChild("do_high")).onClick = (GameObject go) => {
                if (m_ybcost > PlayerModel.getInstance().gold)
                {
                    flytxt.instance.fly(ContMgr.getError("-1001"));
                    return;
                }
                A3_ActiveProxy.getInstance().SendGetBlessing(3);
            };
        }

        void inText()
        {
            this.transform.FindChild("info/atk/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_blessing_1");
            this.transform.FindChild("do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_blessing_3");
            this.transform.FindChild("do_high/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_blessing_4");
            this.transform.FindChild("info/addatk/txt1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_blessing_6");
            this.transform.FindChild("info/addatk/txt2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_blessing_6");
        }

		public override void onShowed() {
			A3_ActiveProxy.getInstance().addEventListener(A3_ActiveProxy.EVENT_ONBLESS, OnBless);

            OnBless(null);
        }

		public override void onClosed() {
			A3_ActiveProxy.getInstance().removeEventListener(A3_ActiveProxy.EVENT_ONBLESS, OnBless);
		}

		public void OnBless(GameEvent e) {
            m_left_num1.text = ContMgr.getCont("uilayer_a3_blessing_5") +
               //"<color=#00FF00>" + (m_ybbdnum - A3_ActiveModel.getInstance().blessnum_ybbd) + "</color>"
              "(" + (m_ybbdnum - A3_ActiveModel.getInstance().blessnum_ybbd) + "/" + m_ybbdnum + ")";
            m_left_num2.text = ContMgr.getCont("uilayer_a3_blessing_5") +
               //"<color=#00FF00>" + (m_ybnum - A3_ActiveModel.getInstance().blessnum_yb) + "</color>"
              "(" + (m_ybnum - A3_ActiveModel.getInstance().blessnum_yb) + "/" + m_ybnum + ")";
            m_addAll.text = "+" + (5 * A3_ActiveModel.getInstance().blessnum_ybbd + 15 * A3_ActiveModel.getInstance().blessnum_yb) + "%";
        }
	}
}
