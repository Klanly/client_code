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
	class a3_dyetip : Window
	{
		public enum eType
		{
			info,
			buy
		}
		eType _type = eType.info;
		a3_BagItemData item_data;
		Action backEvent = null;
		Text totalMoney = null;
		int buynum;
		int maxnum;
		BaseButton bs_bt1;
		BaseButton bs_bt2;
		BaseButton bs_buy;
		Scrollbar bar = null;
		shopDatas sd = null;
		InputField Inputbuy_num ;
		Text buy_text;

		public override void init() {
			BaseButton btn_close = new BaseButton(transform.FindChild("touch"));
			btn_close.onClick = onclose;
			Inputbuy_num = transform.FindChild("buy/bg/contain/bug/InputField").GetComponent<InputField>();
			buy_text = Inputbuy_num.transform.FindChild("Text").GetComponent<Text>();
			totalMoney = transform.FindChild("buy/bg/contain/paymoney/money").GetComponent<Text>();
			bar = transform.FindChild("buy/bg/contain/Scrollbar").GetComponent<Scrollbar>();
			bs_bt1 = new BaseButton(transform.FindChild("buy/bg/contain/btn_reduce"));
			bs_bt2 = new BaseButton(transform.FindChild("buy/bg/contain/btn_add"));
			bs_buy = new BaseButton(transform.FindChild("buy/bg/Button").transform);
			bs_bt1.__listener.onDown = (GameObject go) => {
				buynum = Mathf.Max(--buynum, 0);
				bar.value = (float)buynum / (float)maxnum;
				buy_text.text = buynum.ToString();
				if (buynum == 0) totalMoney.text = "0";
			};
			bs_bt2.__listener.onDown = (GameObject go) => {
				buynum = Mathf.Max(++buynum, 0);
				bar.value =  (float)buynum / (float)maxnum ;
				buy_text.text = buynum.ToString();
			};
			bar.onValueChanged.AddListener((float f)=>{
				buynum = (int)(maxnum * bar.value);
				totalMoney.text = (buynum * sd.value).ToString();
				buy_text.text = buynum.ToString();
				Inputbuy_num.text = buynum.ToString();
			}
			);
			Inputbuy_num.onValueChange.AddListener((string s) => {

				int.TryParse(s,out buynum);
				bar.value = (float)buynum / (float)maxnum;
				totalMoney.text = (buynum * sd.value).ToString();
				buy_text.text = buynum.ToString();
			});
		}

		public override void onShowed() {
			buynum = 0; 
			bar.value = 0;
			backEvent = null;
			transform.SetAsLastSibling();
			item_data = (a3_BagItemData)uiData[0];
			_type = (eType)uiData[1];
			backEvent = (Action)uiData[2];
			Transform info = transform.FindChild("info");
			Transform buy = transform.FindChild("buy");
			if (_type == eType.info) {
				info.gameObject.SetActive(true);
				buy.gameObject.SetActive(false);
				initItemInfo();
			}
			else if (_type == eType.buy) {
				info.gameObject.SetActive(false);
				buy.gameObject.SetActive(true);
				initItemBuy();
			}
			bs_bt1.interactable = true;
			bs_bt2.interactable = true;
			bs_buy.interactable = true;
			buy_text.text = "1";
		}
		public override void onClosed() {
			if (backEvent != null) backEvent();
		}

		void initItemInfo() {
			Transform info = transform.FindChild("info");
			info.FindChild("name").GetComponent<Text>().text = item_data.confdata.item_name;
			info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(item_data.confdata.quality);
			info.FindChild("desc").GetComponent<Text>().text = item_data.confdata.desc;

			if (item_data.confdata.use_limit > 0) {
				info.FindChild("lv").GetComponent<Text>().text = item_data.confdata.use_limit + ContMgr.getCont("zhuan") + item_data.confdata.use_lv + ContMgr.getCont("ji");
			}
			else {
				info.FindChild("lv").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi");
			}
			Transform Image = info.FindChild("icon");
			if (Image.childCount > 0) {
				Destroy(Image.GetChild(0).gameObject);
			}
			GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_data);
			icon.transform.SetParent(Image, false);
			transform.FindChild("info/donum").GetComponent<Text>().text = item_data.num.ToString();
		}

		void initItemBuy() {
			var objsurebuy = transform.FindChild("buy");
			var surebuy_name = objsurebuy.transform.FindChild("bg/contain/name").GetComponent<Text>();
			var surebuy_des = objsurebuy.transform.FindChild("bg/contain/des_bg/Text").GetComponent<Text>();
			GameObject icon = objsurebuy.transform.FindChild("bg/contain/icon").gameObject;
			if (icon.transform.childCount > 0) {
				for (int i = 0; i < icon.transform.childCount; i++) {
					Destroy(icon.transform.GetChild(i).gameObject);
				}
			}
			GameObject item = IconImageMgr.getInstance().createA3ItemIcon(item_data);
			item.transform.SetParent(icon.transform, false);
			surebuy_name.text = a3_BagModel.getInstance().getItemDataById((uint)item_data.confdata.tpid).item_name;
			int color = a3_BagModel.getInstance().getItemDataById((uint)item_data.confdata.tpid).quality;
			surebuy_name.color = Globle.getColorByQuality(color);
			surebuy_des.text = a3_BagModel.getInstance().getItemDataById((uint)item_data.confdata.tpid).desc;
			foreach (var v in Shop_a3Model.getInstance().itemsdic.Values) {
				if (v.itemid == (int)item_data.confdata.tpid) {
					sd = v;
				}
			}
			bar.numberOfSteps = (int)PlayerModel.getInstance().gold / sd.value;
			maxnum = bar.numberOfSteps;
			
			if (maxnum <= 0) {
				bs_bt1.interactable = false;
				bs_bt2.interactable = false;
				bs_buy.interactable = false;
				bs_buy.onClick = (GameObject go) => flytxt.instance.fly(ContMgr.getCont("a3_dyetip"));
				return;
			}
			else {
				buynum = 1;
				totalMoney.text = (buynum * sd.value).ToString();
				bar.value = (float)buynum / (float)maxnum;
			}

			bs_buy.onClick = delegate(GameObject goo) {
				if (sd == null) return;
				if(buynum>=1)
					Shop_a3Proxy.getInstance().sendinfo(2, sd.id, buynum);
			};
		}

		void onclose(GameObject go) {
			InterfaceMgr.getInstance().close(InterfaceMgr.A3_DYETIP);
		}
	}
}
