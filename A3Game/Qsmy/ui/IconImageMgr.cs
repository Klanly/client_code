using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    /***********
     *用来获取各种icon的公用类
     * *********/
    enum EQUIP_SHOW_TYPE
    {
        SHOW_COMMON = 0,
        SHOW_INTENSIFY = 1,
        SHOW_ADDLV = 2,
        SHOW_STAGE = 3,
        SHOW_INTENSIFYANDSTAGE = 4

    };

    class IconImageMgr
    {
        void init()
        {

        }

        //private void checkItemLoaded(ItemData item, Action<ItemData, GameObject> loaded, bool istouch, int num, float scale, GameObject icon_prefab, Sprite s_icon, Sprite s_border)
        //{
        //    if (icon_prefab == null || s_icon == null || s_border == null)
        //    {
        //        loaded(item, null);
        //        return;
        //    }

        //    GameObject iconPrefab = icon_prefab;
        //    GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
        //    Image icon = root.transform.FindChild("icon").GetComponent<Image>();
        //    icon.sprite = s_icon;
        //    Image iconborder = root.transform.FindChild("iconborder").GetComponent<Image>();
        //    iconborder.sprite = s_border;
        //    Text numText = root.transform.FindChild("num").GetComponent<Text>();
        //    if (istouch)
        //    {
        //        root.transform.GetComponent<Button>().enabled = true;
        //    }
        //    else
        //    {
        //        root.transform.GetComponent<Button>().enabled = false;
        //    }
        //    if (num != -1)
        //    {
        //        numText.text = num.ToString();
        //        numText.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        numText.gameObject.SetActive(false);
        //    }

        //    root.name = "icon";
        //    root.transform.localScale = new Vector3(scale, scale, 1.0f);

        //    loaded(item, root);
        //}

        //public void waitItemIcon(ItemData item, Action<ItemData, GameObject> loaded, bool istouch = false, int num = -1, float scale = 1.0f)
        //{
        //    GameObject icon_prefab = null;
        //    Sprite s_icon = null;
        //    Sprite s_border = null;

        //    int res_count = 3;
        //    IAsset res_icon = os.asset.getAsset<IAssetMesh>("prefab/iconimage", (IAsset ast) =>
        //    {
        //        icon_prefab = (ast as AssetMeshImpl).assetObj;
        //        res_count--;
        //        if( res_count == 0 ) checkItemLoaded(item, loaded, istouch, num, scale, icon_prefab, s_icon, s_border);
        //    }, null,
        //    (IAsset ast, string err) =>
        //    {
        //        //加载失败
        //        debug.Log("加载UI失败prefab/iconimage");
        //        res_count--;
        //        if (res_count == 0) loaded(item, null);
        //    });
        //    (res_icon as AssetImpl).loadImpl(false);

        //    //////////////////////////////////////////////////////////////////////////////////////
        //    IAsset res_pic1 = os.asset.getAsset<IAssetBitmap>(item.file, (IAsset ast) =>
        //    {
        //        s_icon = (ast as AssetBitmapImpl).sprite;
        //        res_count--;
        //        if (res_count == 0) checkItemLoaded(item, loaded, istouch, num, scale, icon_prefab, s_icon, s_border);
        //    }, null,
        //    (IAsset ast, string err) =>
        //    {
        //        debug.Log("加载图片失败：" + item.file);
        //        res_count--;
        //        if (res_count == 0) loaded(item, null);
        //    });
        //    (res_pic1 as AssetImpl).loadImpl(false);

        //    //////////////////////////////////////////////////////////////////////////////////////
        //    IAsset res_pic2 = os.asset.getAsset<IAssetBitmap>(item.borderfile, (IAsset ast) =>
        //    {
        //        s_border = (ast as AssetBitmapImpl).sprite;
        //        res_count--;
        //        if (res_count == 0) checkItemLoaded(item, loaded, istouch, num, scale, icon_prefab, s_icon, s_border);
        //    }, null,
        //    (IAsset ast, string err) =>
        //    {
        //        debug.Log("加载图片失败：" + item.borderfile);
        //        res_count--;
        //        if (res_count == 0) loaded(item, null);
        //    });
        //    (res_pic2 as AssetImpl).loadImpl(false);
        //}

        public GameObject createA3ItemIcon(a3_BagItemData data, bool istouch = false, int num = -1, float scale = 1.0f, bool tip = false)
        {
            bool isUpEquip = false;
            if (data.isEquip)
            {//装备战斗力更高标识
                if (a3_EquipModel.getInstance().getEquips().ContainsKey(data.id))
                {

                }
                else if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type) && a3_EquipModel.getInstance().checkisSelfEquip(data.confdata))
                {
                    a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
                    if (data.equipdata.combpt > have_one.equipdata.combpt)
                    {
                        isUpEquip = true;
                    }
                }
                else
                {
                    isUpEquip = true;
                }
            }

            return createA3ItemIcon(data.confdata, istouch, num, scale, tip, data.equipdata.stage, data.equipdata.blessing_lv, data.isNew, isUpEquip, data.ismark, data.equipdata.attribute);
        }
        public GameObject createA3ItemIcon(uint itemid, bool istouch = false, int num = -1, float scale = 1.0f, bool tip = false, int stage = -1, int blessing_lv = 0, bool isNew = false, bool isUpEquip = false, bool ignoreLimit = false, bool isicon = false)
        {
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(itemid);
            return createA3ItemIcon(item, istouch, num, scale, tip, stage, blessing_lv, isNew, isUpEquip, ignoreAllEquipTip: ignoreLimit, isicon: isicon);
        }
        public GameObject createA3ItemIconTip(uint itemid, bool istouch = false, int num = -1, float scale = 1.0f, bool tip = false, int stage = -1, int blessing_lv = 0, bool isNew = false, bool isUpEquip = false, bool isMark = false)
        {
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(itemid);
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimageTip");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(item.file);
            Image iconborder = root.transform.FindChild("iconbor").GetComponent<Image>();
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(item.borderfile);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            if (istouch)
            {
                root.transform.GetComponent<Button>().enabled = true;
            }
            else
            {
                root.transform.GetComponent<Button>().enabled = false;
            }
            if (num != -1)
            {
                numText.text = num.ToString();
                numText.gameObject.SetActive(true);
            }
            else
            {
                numText.gameObject.SetActive(false);
            }

            if (item.item_type == 2)
            {//装备
                if (!a3_EquipModel.getInstance().checkisSelfEquip(item))
                {
                    root.transform.FindChild("iconborder/equip_self").gameObject.SetActive(true);
                }
                else
                {
                    if (!a3_EquipModel.getInstance().checkCanEquip(item, stage, blessing_lv))
                    {
                        root.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                    }

                    if (isUpEquip)
                        root.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(true);
                }
            }
            if (isNew)
                root.transform.FindChild("iconborder/is_new").gameObject.SetActive(true);
            if (isMark)
                root.transform.FindChild("iconborder/ismark").gameObject.SetActive(true);
            else
                root.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            root.name = "icon";
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }
        public GameObject createA3ItemIcon(a3_ItemData item, bool istouch = false, int num = -1, float scale = 1.0f, bool tip = false, int stage = -1, int blessing_lv = 0, bool isNew = false, bool isUpEquip = false, bool isMark = false, int shuxing = -1, bool ignoreAllEquipTip = false, bool isicon = false)
        {
            if (item.borderfile == "" || item.file == "")
            {
                Debug.LogError("图片路径错误__道具： id=" + item.item_name + "   name=" + item.item_name);
            }

            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimage");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(item.file);
            Image iconborder = root.transform.FindChild("iconbor").GetComponent<Image>();
            if(item.borderfile != "")
                iconborder.sprite = GAMEAPI.ABUI_LoadSprite(item.borderfile);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            root.transform.FindChild("shuxing").gameObject.SetActive(false);
            if (isicon)
            {
                Image Bicon = root.transform.FindChild("bicon").GetComponent<Image>();
                Bicon.gameObject.SetActive(true);
            }
            if (istouch)
            {
                root.transform.GetComponent<Button>().enabled = true;
            }
            else
            {
                root.transform.GetComponent<Button>().enabled = false;
            }
            if (num != -1)
            {
                numText.text = num.ToString();
                numText.gameObject.SetActive(true);
            }
            else
            {
                numText.gameObject.SetActive(false);
            }

            if (item.item_type == 2)
            {//装备
                if (!ignoreAllEquipTip)
                {
                    if (!a3_EquipModel.getInstance().checkisSelfEquip(item))
                    {

                        root.transform.FindChild("iconborder/equip_self").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (!a3_EquipModel.getInstance().checkCanEquip(item, stage, blessing_lv))
                            root.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                        if (isUpEquip)
                            root.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(true);
                    }
                    if (shuxing > 0)
                    {
                        string file = "icon_shuxing_" + shuxing;
                        root.transform.FindChild("shuxing").gameObject.SetActive(true);
                        root.transform.FindChild("shuxing").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    }
                    else
                    {
                        root.transform.FindChild("shuxing").gameObject.SetActive(false);
                    }
                }
            }
            if (isNew)
                root.transform.FindChild("iconborder/is_new").gameObject.SetActive(true);
            if (isMark)
                root.transform.FindChild("iconborder/ismark").gameObject.SetActive(true);
            else
                root.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            root.name = "icon";
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }

        public GameObject createA3EquipIcon(a3_BagItemData data, float scale = 1.0f, bool istouch = false)
        {
            return createA3EquipIcon(data.confdata, data.equipdata.stage, data.equipdata.blessing_lv, scale, istouch);
        }
        public void refreshA3EquipIcon_byType(GameObject root, a3_BagItemData data, EQUIP_SHOW_TYPE show_type = EQUIP_SHOW_TYPE.SHOW_COMMON)
        {
            Text numText = root.transform.FindChild("inlvl").GetComponent<Text>();
            numText.color = new Color(1, 0.7f, 0);
            Text tt = root.transform.FindChild("num").GetComponent<Text>();
            tt.gameObject.SetActive(false);
            //Text lvl = root.transform.FindChild("lvl").GetComponent<Text>();
            //lvl.color = new Color(1, 0.7f, 0);
            Transform stars = root.transform.FindChild("stars");

            //if (data.confdata.equip_type != 8 && data.confdata.equip_type != 9 && data.confdata.equip_type != 10)
            //    numText.transform.localPosition = new Vector3(-4,-68,0);

            string str = "";
            switch (data.equipdata.stage)
            {
                case 0: break;
                case 1: str = ContMgr.getCont("IconImageMgr0"); break;
                case 2: str = ContMgr.getCont("IconImageMgr1"); break;
                case 3: str = ContMgr.getCont("IconImageMgr2"); break;
                case 4: str = ContMgr.getCont("IconImageMgr3"); break;
                case 5: str = ContMgr.getCont("IconImageMgr4"); break;
                case 6: str = ContMgr.getCont("IconImageMgr5"); break;
                case 7: str = ContMgr.getCont("IconImageMgr6"); break;
                case 8: str = ContMgr.getCont("IconImageMgr7"); break;
                case 9: str = ContMgr.getCont("IconImageMgr8"); break;
                case 10: str = ContMgr.getCont("IconImageMgr9"); break;
            }
            for (int i = 0; i < stars.childCount; i++)
            {
                stars.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < data.equipdata.stage; i++)
            {
                stars.GetChild(i).gameObject.SetActive(true);
            }

            switch (show_type)
            {
                case EQUIP_SHOW_TYPE.SHOW_INTENSIFY://显示强化
                    stars.gameObject.SetActive(false);

                    numText.gameObject.SetActive(true);
                    numText.text = "+" + data.equipdata.intensify_lv;
                    break;
                case EQUIP_SHOW_TYPE.SHOW_ADDLV://显示追加
                    stars.gameObject.SetActive(false);
                    numText.gameObject.SetActive(true);
                    numText.text = ContMgr.getCont("a3_auction_zhui") + data.equipdata.add_level;
                    break;
                case EQUIP_SHOW_TYPE.SHOW_STAGE://显示进阶
                    numText.gameObject.SetActive(false);
                    stars.gameObject.SetActive(true);
                    // lvl.text = str;
                    break;
                case EQUIP_SHOW_TYPE.SHOW_INTENSIFYANDSTAGE://显示强化和显示进阶
                    stars.gameObject.SetActive(true);
                    // lvl.text = str;
                    numText.gameObject.SetActive(true);
                    numText.text = "+" + data.equipdata.intensify_lv;
                    break;
                default:
                    stars.gameObject.SetActive(false);
                    numText.gameObject.SetActive(false);
                    break;
            }
        }
        public GameObject createA3EquipIcon(a3_ItemData data, int stage = -1, int blessing_lv = 0, float scale = 1.0f, bool istouch = false)
        {
            string file = "icon_equip_" + data.tpid;
            string borderfile = "icon_itemborder_b039_0" + data.quality;
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimage");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            Image iconborder = root.transform.FindChild("iconbor").GetComponent<Image>();
            Transform war = root.transform.FindChild("wk");
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(borderfile);

            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            numText.gameObject.SetActive(false);
            root.transform.GetComponent<Button>().enabled = false;

            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            root.name = "icon";

            if (istouch)
                root.transform.GetComponent<Button>().enabled = true;
            else
                root.transform.GetComponent<Button>().enabled = false;

            if (!a3_EquipModel.getInstance().checkisSelfEquip(data))
            {
                root.transform.FindChild("iconborder/equip_self").gameObject.SetActive(true);
            }
            else
            {
                if (!a3_EquipModel.getInstance().checkCanEquip(data, stage, blessing_lv))
                {
                    root.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                }
            }

            icon.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 160);
            iconborder.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 160);
            war.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 160);
            root.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(92, 166);

            return root;
        }

        public GameObject createItemIcon4Lottery(a3_ItemData item, bool istouch = false, int num = -1, bool isicon = false, float scale = 1.0f, bool tip = false)
        {
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimage");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image borderIcon = root.GetComponent<Image>();
            borderIcon.enabled = false;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            Image Qicon = root.transform.FindChild("qicon").GetComponent<Image>();
            Qicon.sprite = GAMEAPI.ABUI_LoadSprite(item.borderfile);
            Qicon.gameObject.SetActive(true);

            if (isicon)
            {
                Image Bicon = root.transform.FindChild("bicon").GetComponent<Image>();
                Bicon.gameObject.SetActive(true);
            }

            Transform iconborder = root.transform.FindChild("iconbor");
            iconborder.gameObject.SetActive(false);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            numText.enabled = false;
            if (istouch)
            {
                root.transform.GetComponent<Button>().enabled = true;
            }
            else
            {
                root.transform.GetComponent<Button>().enabled = false;
            }
            if (num != -1)
            {
                numText.text = num.ToString();
                numText.gameObject.SetActive(true);
            }
            else
            {
                numText.gameObject.SetActive(false);
            }


            //if (tip)
            //{
            //    EventTriggerListener.Get(root).onDown = (GameObject go) => { TipMgr._instacne.show(item, go.GetComponent<RectTransform>().position); };
            //    EventTriggerListener.Get(root).onExit = (GameObject go) => { TipMgr._instacne.hide(); };
            //    EventTriggerListener.Get(root).onUp = (GameObject go) => { TipMgr._instacne.hide(); };
            //}

            root.name = "icon";
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }

        public GameObject createLotteryInfo(itemLotteryAwardInfoData data, bool isTouch = false, int num = -1, float scale = 1.0f)//创建玩家抽奖信息
        {
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_lotteryItemAwardInfo");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Text txtInfo = root.transform.FindChild("txt_info").GetComponent<Text>();
            var item = a3_BagModel.getInstance().getItemDataById(data.tpid);
            txtInfo.text = string.Format("<color=#ff0000>{0}</color> <color=#ffffff>获得了</color> {1}", data.name,/* data.cnt 
                +LotteryModel.getInstance().getAwardTypeId(data.tpid), data.stage == 0 ? "" : data.stage + "阶", */a3_lottery.mInstance.GetLotteryItemNameColor(item.item_name, item.quality));
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }

        public GameObject creatEeverydayWelfareItemIcon(a3_ItemData item, bool istouch = false, int dayCount = -1, int num = -1,
            float scale = 1.0f, bool tip = false, int stage = -1, int blessing_lv = 0, bool isNew = false,//每日奖励
            bool isUpEquip = false)
        {
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_everydayWelfare");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(item.file);
            Image iconborder = root.transform.FindChild("iconborder").GetComponent<Image>();
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(item.borderfile);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            if (istouch)
            {
                root.transform.GetComponent<Button>().enabled = true;
            }
            else
            {
                root.transform.GetComponent<Button>().enabled = false;
            }

            if (num != -1)
            {
                numText.text = num.ToString();
                numText.gameObject.SetActive(true);
            }
            else
            {
                numText.gameObject.SetActive(false);
            }

            if (item.item_type == 2)
            {//装备
                if (!a3_EquipModel.getInstance().checkisSelfEquip(item))
                {
                    root.transform.FindChild("iconborder/equip_self").gameObject.SetActive(true);
                }
                else
                {
                    if (!a3_EquipModel.getInstance().checkCanEquip(item, stage, blessing_lv))
                    {
                        root.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(true);
                    }

                    if (isUpEquip)
                        root.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(true);
                }
            }
            if (isNew)
                root.transform.FindChild("iconborder/is_new").gameObject.SetActive(true);

            root.name = "icon";
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }
        public GameObject creatItemAwardCenterIcon(a3_ItemData item)//奖励中心)
        {
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_itemAwardCenter");
            iconPrefab.transform.FindChild("Common/Text_One/txtInfor").GetComponent<Text>().text = ContMgr.getCont("itemAwardCenter_0");
            iconPrefab.transform.FindChild("Common/Text_Two/txtInfor").GetComponent<Text>().text = ContMgr.getCont("itemAwardCenter_0");
            iconPrefab.transform.FindChild("Common/Text_Three/txtInfor").GetComponent<Text>().text = ContMgr.getCont("itemAwardCenter_1");
            iconPrefab.transform.FindChild("Common/state/btnGet/Text").GetComponent<Text>().text = ContMgr.getCont("itemAwardCenter_2");
            iconPrefab.transform.FindChild("Specific/state/btnGet/Text").GetComponent<Text>().text = ContMgr.getCont("itemAwardCenter_2");

            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            //Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            //icon.sprite = GAMEAPI.ABUI_LoadSprite(item.file);
            //Text txtInfo = root.transform.FindChild("txtInfor").GetComponent<Text>();

            //root.name = "icon";

            return root;
        }
        public GameObject createEquipIcon(EquipConf data, float scale = 1.0f, bool istouch = false)
        {
            string file = "icon_equip_" + data.tpid;
            string borderfile = "icon_itemborder_b039_0" + data.quality;
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimage");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            Image iconborder = root.transform.FindChild("iconbor").GetComponent<Image>();
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(borderfile);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            numText.gameObject.SetActive(false);
            root.transform.GetComponent<Button>().enabled = false;

            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            root.name = "icon";

            if (istouch)
                root.transform.GetComponent<Button>().enabled = true;
            else
                root.transform.GetComponent<Button>().enabled = false;

            return root;
        }
        public GameObject createMoneyIcon(string type, float scale = 1.0f, int num = -1)
        {
            string file = "icon_comm_" + type;
            string borderfile = "icon_itemborder_b039_02";
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_iconimage");
            GameObject root = GameObject.Instantiate(iconPrefab) as GameObject;
            Image icon = root.transform.FindChild("icon").GetComponent<Image>();
            icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            Image iconborder = root.transform.FindChild("iconbor").GetComponent<Image>();
            iconborder.sprite = GAMEAPI.ABUI_LoadSprite(borderfile);
            Text numText = root.transform.FindChild("num").GetComponent<Text>();
            if (num != -1)
            {
                numText.text = num.ToString();
                numText.gameObject.SetActive(true);
            }
            else
            {
                numText.gameObject.SetActive(false);
            }
            root.transform.GetComponent<Button>().enabled = false;
            root.transform.localScale = new Vector3(scale, scale, 1.0f);
            return root;
        }
        static private IconImageMgr _instance;
        static public IconImageMgr getInstance()
        {
            if (_instance == null)
            {
                _instance = new IconImageMgr();
                _instance.init();
            }
            return _instance;

        }
    }
}
