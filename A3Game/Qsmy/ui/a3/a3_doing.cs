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
using MuGame.Qsmy.model;
namespace MuGame
{
	class a3_doing : FloatUi
	{
		BaseButton btn;
		public Action btn_event;
		Text text;
		string str = default(string);

		public override void init()
		{
			btn = new BaseButton(getTransformByPath("msg/text/btn_bg/btn"));
			btn.onClick = (GameObject g) => {
				if (btn_event != null) {
					btn_event();
				}
			};
			text = getComponentByPath<Text>("msg/text");
		}

		public override void onShowed() {
			this.transform.SetAsFirstSibling();
			if (uiData != null) {
				if (uiData.Count > 0)
					btn_event = (Action)uiData[0];
				if (uiData.Count > 1)
					str = (string)uiData[1];
			}
			Refresh();
		}

		public override void onClosed() {
			base.onClosed();
		}

		void Refresh() {
			text.text = str;
		}
	}
}
