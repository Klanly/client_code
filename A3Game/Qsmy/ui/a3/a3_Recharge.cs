using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace MuGame
{
    class a3_Recharge : Window
    {
        private BaseButton btnClose;
        private BaseButton btnVipPri;
        public static a3_Recharge Instance;
        private Text Vip_lvl;
        private Text NeedCount;
        private Text ToNextVip;
        private Text bindDiamond;
        private Text Diamond;
        private GameObject toptext;
        private GameObject isManLvl;
        Dictionary<int, GameObject> retra = new Dictionary<int, GameObject>();

        GameObject left;
        GameObject right;
        Transform con;

        public static a3_Recharge isshow;

        private Image ExpImg;
        public override void init()
        {


            getComponentByPath<Text>("topCon/Text_bg_1").text = ContMgr.getCont("a3_Recharge_0");
            getComponentByPath<Text>("topCon/isMaxLvl").text = ContMgr.getCont("a3_Recharge_1");
            getComponentByPath<Text>("buy_bg/item/name").text = ContMgr.getCont("a3_Recharge_2");
            getComponentByPath<Text>("buy_bg/item/item_text").text = ContMgr.getCont("a3_Recharge_3");







            Instance = this;
            isManLvl = this.transform.FindChild("topCon/isMaxLvl").gameObject;

            btnClose = new BaseButton(this.getTransformByPath("btn_close"));
            btnClose.onClick = OnCLoseClick;

            btnVipPri = new BaseButton(this.getTransformByPath("btn_VipPri"));
            btnVipPri.onClick = OnVipPri;

            left = this.transform.FindChild("buy_bg/Image_tq_left").gameObject;
            right = this.transform.FindChild("buy_bg/Image_suolian_right").gameObject;
            con = this.transform.FindChild("buy_bg/scrollview/con");
            this.transform.FindChild("buy_bg/scrollview").GetComponent<ScrollRect>().onValueChanged.AddListener((any) => CheckArrow());

            Vip_lvl = this.getComponentByPath<Text>("topCon/Image_level/Text");
            NeedCount = this.getComponentByPath<Text>("topCon/Text_bg_1/Text_num");
            ToNextVip = this.getComponentByPath<Text>("topCon/Text_level");
            toptext = this.transform.FindChild("topCon/Text_bg_1").gameObject;

            bindDiamond = this.getComponentByPath<Text>("bindDiamond/Text");
            Diamond = this.getComponentByPath<Text>("Diamond/Text");

            ExpImg = this.getComponentByPath<Image>("topCon/Image_exp");
            
            left.SetActive(false);
            right.SetActive(true);
            transform.SetAsLastSibling();
        }
        public override void onShowed()
        {
            isshow = this;
            GRMap.GAME_CAMERA.SetActive(false);
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, Refresh_Diamond);
            A3_VipModel.getInstance().OnExpChange += OnExpChange;
            A3_VipModel.getInstance().OnLevelChange += OnVipLevelChange;
            OnVipLevelChange();
            OnExpChange();
            recharge_Refresh();
            Refresh_Diamond(null);
            toTach_close = false;
            if (a3_relive.instans)
            {
                transform.SetAsLastSibling();
                a3_relive.instans.FX.SetActive(false);
            }
        }
        public override void onClosed()
        {
            isshow = null;
            GRMap.GAME_CAMERA.SetActive(true);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, Refresh_Diamond);
            A3_VipModel.getInstance().OnExpChange -= OnExpChange;
            A3_VipModel.getInstance().OnLevelChange -= OnVipLevelChange;
            foreach (GameObject o in retra.Values)
            {
                Destroy(o);
            }
            retra.Clear();

            //Instance = null;
            if (a3_relive.instans)
            {
                a3_relive.instans.FX.SetActive(true);
            }
            if (a3_lottery.mInstance?.toRecharge == true && toTach_close) {
                a3_lottery.mInstance.toRecharge = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY);
            }

            if (a3_sign.instan != null && a3_sign.instan.returnthis && toTach_close) {
                a3_sign.instan.returnthis = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SIGN);
            }

            if (a3_new_pet .instance != null && a3_new_pet.instance.toback && toTach_close)
            {
                a3_new_pet.instance.toback = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEW_PET );
            }
        }

        private void CheckArrow()
        {
            if (con.GetChild(0).transform.position.x >= left.transform.position.x)
            {
                left.SetActive(false);
            }
            else { left.SetActive(true); }

            if (con.GetChild(con.childCount - 1).transform.position.x <= right.transform.position.x)
            {
                right.SetActive(false);
            }
            else
            {
                right.SetActive(true);
            }
        }

        //刷新充值种类列表
        public  void recharge_Refresh() 
        {
            if (retra.Count > 0) return;
            GameObject inem = this.transform.FindChild("buy_bg/item").gameObject;
            RectTransform con = this.transform.FindChild("buy_bg/scrollview/con").GetComponent<RectTransform>();
            Dictionary<int, rechargeData> data = new Dictionary<int,rechargeData> ();
            data = RechargeModel.getInstance().rechargeMenu;
            int num = 0;
            foreach (int i in data.Keys)
            {
                if (data[i].ka > 0) {
                    if (A3_signProxy.getInstance().yueka == 1) {
                        if (data[i].ka == 1) { continue; }
                    }
                    else if (A3_signProxy.getInstance().yueka == 2) {
                        if (data[i].ka == 1 || data[i].ka == 2) {
                            continue;
                        }
                    }
                }

                GameObject clon = (GameObject)Instantiate(inem);
                clon.SetActive(true);
                clon.transform.SetParent(con, false);
                Text name = clon.transform.FindChild("name").GetComponent<Text>();
                name.text = data[i].name;
                Text money = clon.transform.FindChild("money/Text").GetComponent<Text>();
                money.text = "￥" + data[i].golden;
                Text item_text = clon.transform.FindChild("item_text").GetComponent<Text>();

                if (data[i].first_double > 0)
                {
                    clon.transform.FindChild("double").gameObject.SetActive(true);
                }
                else clon.transform.FindChild("double").gameObject.SetActive(false);

                if (RechargeModel.getInstance().firsted.Contains(i))
                {
                    item_text.text = StringUtils.formatText(data[i].desc2);
                    clon.transform.FindChild("double").gameObject.SetActive(false);
                }
                else {
                    item_text.text = StringUtils.formatText(data[i].desc);
                }

                clon.transform.FindChild("icon_di/icon_Img").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_recharge_"+data[i].id.ToString());

                rechargeData dta = data[i];
                BaseButton btn_monet = new BaseButton(clon.transform.FindChild("money"));
                btn_monet.onClick = delegate (GameObject go) {
                    onEnsure(dta);
                };
                num++;
                retra[i] = clon;
            }
            float childSizeX = con.GetComponent<GridLayoutGroup>().cellSize.x;
            Vector2 newSize = new Vector2(num * childSizeX, con.sizeDelta.y);
            con.sizeDelta = newSize;
        }

        public void refre_recharge() {
            
            foreach (GameObject o in retra.Values ) {
                Destroy(o);
            }
            retra.Clear();

            recharge_Refresh();
        }

        void onEnsure(rechargeData dta)
        {
            GameSdkMgr.Pay(dta);
        }

        //经验值变化
        private void OnExpChange()
        {
            int maxExp = A3_VipModel.getInstance().GetNextLvlMaxExp();
            int leftExp = maxExp - A3_VipModel.getInstance().Exp;

            NeedCount.text = leftExp.ToString();
            if (maxExp > 0)
            {
                ExpImg.fillAmount = (float)A3_VipModel.getInstance().Exp / maxExp;
            }
            else
            {
                ExpImg.fillAmount = 1;
            }
        }
        //等级变化
        private void OnVipLevelChange()
        {//
            int level = A3_VipModel.getInstance().Level;

            Vip_lvl.text = level.ToString();

            if (level >= A3_VipModel.getInstance().GetMaxVipLevel())
            {
                ToNextVip.gameObject.SetActive(false);
                toptext.SetActive(false);
                isManLvl.SetActive(true);
            }
            else
            {
                ToNextVip.text = "VIP" + (level + 1);
                ToNextVip.gameObject.SetActive(true);
                toptext.SetActive(true);
                isManLvl.SetActive(false);
            }
        }
        private void Refresh_Diamond(GameEvent e) 
        {
            bindDiamond.text = PlayerModel.getInstance().gift.ToString();
            Diamond.text = PlayerModel.getInstance().gold.ToString();
        }

        bool toTach_close = false;
        void OnCLoseClick(GameObject go) 
        {

            toTach_close = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RECHARGE);
            CreateModelOnClose();
        }
        void OnVipPri(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RECHARGE);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_VIP);
        }
        void CreateModelOnClose()
        {
            if (InterfaceMgr.getInstance().checkWinOpened(InterfaceMgr.A3_LOTTERY))
            {
                a3_lottery.mInstance.CreateModel();
                return;
            }
        }
    }

}
