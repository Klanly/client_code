using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_store : Window
    {
        static public uint itm_tpid = 0;
        static private uint min_num = 1;
        static private uint max_num = 10000;

        private Slider slider;
        private a3_ItemData itmdata = new a3_ItemData();

        public override void init()
        {

            getComponentByPath<Text>("title/Text").text = ContMgr.getCont("a3_store_0");
            getComponentByPath<Text>("num").text = ContMgr.getCont("a3_store_1");
            getComponentByPath<Text>("text").text = ContMgr.getCont("a3_store_2");
            getComponentByPath<Text>("buyBtn/Text").text = ContMgr.getCont("a3_store_0");



            BaseButton addBtn = new BaseButton(transform.FindChild("addBtn")) {onClick = OnAddButton};
            BaseButton subBtn = new BaseButton(transform.FindChild("subBtn")) {onClick = OnSubButton};
            BaseButton buyBtn = new BaseButton(transform.FindChild("buyBtn")) {onClick = OnBuy};

            slider = getComponentByPath<Slider>("slider");
            slider.onValueChanged.AddListener(OnSliderChange);

            this.getEventTrigerByPath("ig_bg_bg").onClick = onClose;
        }

        public override void onShowed()
        {
            a3_BagModel bag = a3_BagModel.getInstance();
            itmdata = bag.getItemDataById(itm_tpid);

            Image icon = getComponentByPath<Image>("iconimage/icon");
            icon.sprite = GAMEAPI.ABUI_LoadSprite(itmdata.file);

            Image iconborder = getComponentByPath<Image>("iconimage/iconborder");
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(itmdata.borderfile);
            
            getComponentByPath<Text>("nimg/name").text = itmdata.item_name;
            getComponentByPath<Text>("desc").text = itmdata.desc;

            slider.minValue = min_num;
            slider.maxValue = max_num;

            uint num = (uint)slider.value;
            getComponentByPath<Text>("num").text = num.ToString();

            long money = num * itmdata.on_sale;
            getComponentByPath<Text>("money").text = money.ToString();
        }

        private void OnSubButton(GameObject go)
        {
            uint num = (uint) slider.value;
            if (num > min_num)
                num -= 1;
            slider.value = num;
        }

        private void OnAddButton(GameObject go)
        {
            uint num = (uint) slider.value;
            if (num < max_num)
                num += 1;
            slider.value = num;
        }

        private void OnBuy(GameObject go)
        {
            if (itmdata.on_sale <= 0)
                return;

            PlayerModel player = PlayerModel.getInstance();
            if (player.money < slider.value * itmdata.on_sale)
            {
                flytxt.instance.fly(ContMgr.getCont("comm_nomoney")+"!");
                return;
            }

            //TODO 检查背包容量是否足够

            Shop_a3Proxy.getInstance().BuyStoreItems(itmdata.tpid, (uint)slider.value);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_STORE);
        }

        private void OnSliderChange(float v)
        {
            uint num = (uint)v;
            getComponentByPath<Text>("num").text = num.ToString();

            long money = num * itmdata.on_sale;
            getComponentByPath<Text>("money").text = money.ToString();
        }

        private void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_STORE);
        }
    }
}
