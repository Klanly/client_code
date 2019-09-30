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
using DG.Tweening;

namespace MuGame
{
    class a3_vip : Window
    {
        private BaseButton btnClose;

        //top
        private Image imageCurLevel;
        //private Slider sliderExp;
        private Image ExpImg;
        private Text textBuyNum;
        private Text textNextLevel;
        private BaseButton btnRecharge;
        private GameObject isManLvl;

        private List<GameObject> tableList;
        
        //tab_1
        private Transform conBtnTable;
        private Transform conGiftTable;
        private Transform conPermissionTable;
        private BaseButton btnGetGift;
        private Transform giftlvl;
        private Text lvl;
        private BaseButton up_btn, dow_btn;
        private Transform viplnl_view;
        private BaseButton vipGiftBtn, vipPriBtn;
        GameObject tab1, tab2;
        Vector2 v;
        Vector2 v1;
        Vector2 v2;


        private GameObject tip;
        //tab_2
        private Transform conState;
       // private Text textVipState;

        private A3_VipModel vipModel;
        private List<SXML> vipXml;
        public static a3_vip instan;
        RectTransform con;
        Transform conBtn;
        RectTransform gftCon;

        GameObject up;
        GameObject down;
        Transform v_con;

        int nowlook_viplvl = 1;
        ScrollControler scrollControer0;

        public override void init()
        {
            instan = this;
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("bottomCon/tab_2/scrollview"));
            if (uiData != null)
            {
                int type = (int)uiData[0];
                setopen(type);
            }

            tip = this.transform.FindChild("tip").gameObject;
            btnClose = new BaseButton(this.getTransformByPath("btn_close"));
            btnClose.onClick = OnCLoseClick;

            v_con = this.transform.FindChild("bottomCon/tab_1/con_btn/scrollview/con");
            up = this.transform.FindChild("bottomCon/tab_1/con_btn/Image_shang").gameObject;
            down = this.transform.FindChild("bottomCon/tab_1/con_btn/Image_xia").gameObject;
            isManLvl = this.transform.FindChild("topCon/isMaxLvl").gameObject;
            imageCurLevel = this.getComponentByPath<Image>("topCon/Image_level");
            //sliderExp = this.getComponentByPath<Slider>("topCon/expSlider");
            ExpImg = this.getComponentByPath<Image>("topCon/Image_exp");
            textBuyNum = this.getComponentByPath<Text>("topCon/Text_bg_1/Text_num");
            textNextLevel = this.getComponentByPath<Text>("topCon/Text_level");
            btnRecharge = new BaseButton(this.getTransformByPath("btn_recharge"));
            btnRecharge.onClick = OnRechargeBtnClick;
            vipGiftBtn = new BaseButton(this.getTransformByPath("bottomCon/top_btn/btn_1"));
            vipGiftBtn.onClick = OpenVipGift;
            vipPriBtn = new BaseButton(this.getTransformByPath("bottomCon/top_btn/btn_2"));
            vipPriBtn.onClick = OpenVipPri;

            //tab
            tab1 = this.getGameObjectByPath("bottomCon/tab_1");
            tab2 = this.getGameObjectByPath("bottomCon/tab_2");
            //tab_1
            conBtnTable = this.getTransformByPath("bottomCon/tab_1/con_btn");
            viplnl_view = conBtnTable.FindChild("scrollview/con");
            conGiftTable = this.getTransformByPath("bottomCon/tab_1/con_gift");
            conPermissionTable = this.getTransformByPath("bottomCon/tab_1/con_permission");
            btnGetGift = new BaseButton(this.getTransformByPath("bottomCon/tab_1/con_gift/btn_get"));
            giftlvl = this.getTransformByPath("bottomCon/tab_1/con_gift/Text_level");
            lvl = giftlvl.GetComponent<Text>();
            btnGetGift.onClick = OnGetBtnClick;
            this.transform.FindChild("bottomCon/tab_1/con_btn/scrollview").GetComponent<ScrollRect>().onValueChanged.AddListener((any) => CheckArrow());
            con = conPermissionTable.FindChild("view/con").GetComponent<RectTransform>();
            conBtn = conBtnTable.FindChild("scrollview/con");
            gftCon = conGiftTable.FindChild("view/con").GetComponent<RectTransform>();

            this.transform.FindChild("Image_left").GetComponent<CanvasGroup>().blocksRaycasts = false ;
            this.transform.FindChild("Image_right").GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            //tab_2
            conState = this.getTransformByPath("bottomCon/tab_2");
            //textVipState = this.getComponentByPath<Text>("bottomCon/tab_2/scrollview/con");

            vipModel = A3_VipModel.getInstance();
            vipXml = vipModel.VipLevelXML;

            v = con.position;
            v1 = conBtn.position;
            v2 = gftCon.position;
            InitBtnList();
            InitVip_priList();
            //textVipState.text = vipModel.GetVipState();
            vipGiftBtn.interactable = false;
            base.init();


            getComponentByPath<Text>("bottomCon/tab_1/con_gift/btn_get/text").text = ContMgr.getCont("a3_vip_0");
            getComponentByPath<Text>("bottomCon/tab_1/con_gift/Text_level/Text").text = ContMgr.getCont("a3_vip_1");
            getComponentByPath<Text>("bottomCon/tab_1/con_gift/ImageTemp/pri_text").text = ContMgr.getCont("a3_vip_2");
            getComponentByPath<Text>("bottomCon/tab_1/con_permission/ImageTemp/pri_text").text = ContMgr.getCont("a3_vip_2");
            getComponentByPath<Text>("bottomCon/tab_1/con_permission/Text").text = ContMgr.getCont("a3_vip_3");
            getComponentByPath<Text>("bottomCon/tab_2/top_text/per_tip/Text").text = ContMgr.getCont("a3_vip_4");
            getComponentByPath<Text>("bottomCon/tab_2/scrollview/con/item/per_tip/Text").text = ContMgr.getCont("a3_vip_4");
            getComponentByPath<Text>("bottomCon/top_btn/btn_1/Text").text = ContMgr.getCont("a3_vip_5");
            getComponentByPath<Text>("bottomCon/top_btn/btn_2/Text").text = ContMgr.getCont("a3_vip_6");
            getComponentByPath<Text>("topCon/isMaxLvl").text = ContMgr.getCont("a3_vip_7");
            getComponentByPath<Text>("btn_recharge/text").text = ContMgr.getCont("a3_vip_8");
            getComponentByPath<Text>("topCon/Text_bg_1").text = ContMgr.getCont("Text_bg_1");


            getComponentByPath<Text>("tip/text_bg/name/lite").text = ContMgr.getCont("a3_vip_9");
            getComponentByPath<Text>("tip/text_bg/name/has").text = ContMgr.getCont("a3_vip_10");
        }

        public override void onShowed()
        {
            Toclose = false;
            //isthisImage();
            //textVipState.text = vipModel.GetVipState();
            OnVipTabHanle(1);
            //tab1.SetActive(true);
            vipModel.OnLevelChange += OnVipLevelChange;
            vipModel.OnExpChange += OnExpChange;
            A3_VipProxy.getInstance().GetVip();
            OnVipLevelChange();
            OnExpChange();
            OnGiftBtnRefresh();
            if (VipBtnList[0] != null)
            {
                BtnList();
                VipBtnList[0].GetComponent<Button>().interactable = false;
            }
            //vipGiftBtn.interactable = false;
            con.position = v;
            conBtn.position = v1;
            gftCon.position = v2;
            lvl.text = "1";
            nowlook_viplvl = 1;
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            base.onShowed();
            tip.SetActive(false);
            GRMap.GAME_CAMERA.SetActive(false);
            if (a3_relive.instans)
            {
                transform.SetAsLastSibling();
                a3_relive.instans.FX.SetActive(false);
            }

            CheckArrow();

            up.SetActive(false);
            down.SetActive(true);

        }
        private void CheckArrow()
        {
            if (v_con.GetChild(0).transform.position.y >= up.transform.position.y)
            {
                up.SetActive(true);
            }
            else { up.SetActive(false); }

            if (v_con.GetChild(v_con.childCount - 1).transform.position.y <= down.transform.position.y)
            {
                down.SetActive(true);
            }
            else
            {
                down.SetActive(false);
            }
        }
        public override void onClosed()
        {
            vipModel.OnLevelChange -= OnVipLevelChange;

            vipModel.OnExpChange -= OnExpChange;
            GRMap.GAME_CAMERA.SetActive(true);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            if (a3_relive.instans)
            {
                a3_relive.instans.FX.SetActive(true);
            }

            base.onClosed();
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
            if (a3_lottery.mInstance?.toRecharge == true && Toclose)
            {
                a3_lottery.mInstance.toRecharge = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY);
            }

            if (a3_sign.instan != null && a3_sign.instan.returnthis && Toclose)
            {
                a3_sign.instan.returnthis = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SIGN);
            }

            if (a3_new_pet.instance != null && a3_new_pet.instance.toback && Toclose)
            {
                a3_new_pet.instance.toback = false;
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_NEW_PET);
            }
        }

        void showtip() { }

        //等级变化
        private void OnVipLevelChange()
        {
            Text textLevel = imageCurLevel.transform.FindChild("Text").GetComponent<Text>();
            int level = vipModel.Level;

            textLevel.text =level.ToString();

            if (level >= vipModel.GetMaxVipLevel())
            {
                textNextLevel.gameObject.SetActive(false);
                textBuyNum.gameObject.transform.parent.gameObject.SetActive(false);
                isManLvl.SetActive(true);
            }
            else
            {
                textNextLevel.text = "VIP" + (level + 1);
                isManLvl.SetActive(false);
                textNextLevel.gameObject.SetActive(true);
                textBuyNum.gameObject.transform.parent.gameObject.SetActive(true);
            }
        }

        //经验值变化
        private void OnExpChange()
        {
            int maxExp = vipModel.GetNextLvlMaxExp();
            int leftExp = maxExp - vipModel.Exp;

            textBuyNum.text = leftExp.ToString();
            if (maxExp > 0)
            {
                ExpImg.fillAmount = (float)vipModel.Exp / maxExp;
            }
            else 
            {
                ExpImg.fillAmount = 1;
            }
            //float val = (float)vipModel.Exp / maxExp;
            //sliderExp.value = val;
        }

        private Dictionary<int,GameObject> VipBtnList = new Dictionary<int,GameObject>();
        //初始化tab_1按钮列表
        private void InitBtnList()
        {
            GameObject btnTemp = conBtnTable.FindChild("btnTemp").gameObject;
            Transform conBtn = conBtnTable.FindChild("scrollview/con");

            int length = vipModel.GetMaxVipLevel();
            lvl.text = "1";
            for (int i = 0; i < length; i++)
            {
                GameObject clon = (GameObject)Instantiate(btnTemp);

                Text name = clon.transform.FindChild("Text").GetComponent<Text>();
                name.text = "VIP" + (i + 1);
                clon.name = (i + 1).ToString();
                VipBtnList[i] = clon;
                BaseButton btn = new BaseButton(clon.transform);
                btn.onClick = delegate(GameObject go)
                {
                    int level = Int32.Parse(go.name);
                    nowlook_viplvl = level;
                    OnGiftBtnRefresh();
                    OnVipTabHanle(level);
                    //isthisImage();
                    BtnList();
                    go.GetComponent<Button>().interactable = false;
                    lvl.text = level.ToString();
                };
                clon.transform.SetParent(conBtn, false);
                clon.SetActive(true);
            }
        }
        private void BtnList()
        {
            Transform conBtn = conBtnTable.FindChild("scrollview/con");
            for (int i = 0; i < conBtn.childCount;i++ ) 
            {
                conBtn.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
        //void isthisImage()
        //{
        //    foreach (int i in VipBtnList.Keys)
        //    {
        //        VipBtnList[i].transform.FindChild("isthis").gameObject.SetActive(false);
        //    }
        //}
        //初始化tab_2特权表
        private void InitVip_priList() 
        {
            GameObject btnTemp = conState.FindChild("scrollview/con/item").gameObject;
            Transform conBtn = conState.FindChild("scrollview/con");
            int length = vipModel.GetPriMum();
            for (int i = 0; i < length;i++ ) 
            {
                GameObject clon = (GameObject)Instantiate(btnTemp);
                clon.SetActive(true);
                Text tip_text = clon.transform.FindChild("per_tip/Text").GetComponent<Text>();
                tip_text.text = vipModel.Gettype_Name(0,i);
                for (int j = 1; j <= clon.transform.FindChild("con").childCount; j++)
                {
                    int showType = vipModel.GetShowType(i);
                    if (showType == 1)
                    {
                        GameObject value_text = clon.transform.FindChild("con/" + j + "/Text").gameObject;
                        value_text.SetActive(false);
                        string value = vipModel.GetValue(j, i);
                        switch (value)
                        {
                            case "0":
                                clon.transform.FindChild("con/" + j + "/No").gameObject.SetActive(true);
                                break;
                            case "1":
                                clon.transform.FindChild("con/" + j + "/Yes").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (showType == 2)
                    {
                        Text value_text = clon.transform.FindChild("con/" + j + "/Text").GetComponent<Text>();
                        string value = vipModel.GetValue(j, i);
                        value_text.text = value;
                    }
                    else if (showType == 3)
                    {
                        Text value_text = clon.transform.FindChild("con/" + j + "/Text").GetComponent<Text>();

                        value_text.text = double.Parse(vipModel.GetValue(j, i)) * 100 + "%";

                    }

                }
                clon.transform.SetParent(conBtn,false);
            }
        }
        //vip界面table
        private void OnVipTabHanle(int level)
        {
            //List<int> list1 = vipModel.GetVipAttByLevel(level);
            //List<int> list2 = vipModel.GetVipGiftListByLevel(level);
            OnContainerRefresh(conPermissionTable,level);
            OnVipGiftRefresh(conGiftTable, level);
            //conGiftTable
            //OnContainerRefresh(conPermissionTable, list1);
            //OnContainerRefresh(conGiftTable, list2);
        }

        //不同vip的 刷新container
        private void OnContainerRefresh(Transform conTab,List<int> list)
        {
            GameObject tempImage = conTab.FindChild("ImageTemp").gameObject;
            RectTransform con = conTab.FindChild("view/con").GetComponent<RectTransform>();

            float childSizeX = tempImage.transform.GetComponent<RectTransform>().sizeDelta.x;
            Vector2 newSize = new Vector2(list.Count * childSizeX, con.sizeDelta.y);
            con.sizeDelta = newSize;

            int conLength = con.childCount;
            int i, k;

            for (i = 0; i < list.Count; i++)
            {
                int id = list[i];

                if (i >= conLength)
                {
                    GameObject clon = (GameObject)Instantiate(tempImage);

                    clon.transform.SetParent(con, false);
                    OnRefreshObjByID(clon, id);
                }
                else
                {
                    Transform tran = con.GetChild(i);
                    OnRefreshObjByID(tran.gameObject, id);
                }
            }

            for (k = i; k < conLength; k++)
            {
                con.GetChild(k).gameObject.SetActive(false);
            }
        }
        //刷新新增的vip特权
        private void OnContainerRefresh(Transform conTab ,int lvl) 
        {
            if (lvl <= 0)
                return;
            GameObject tempImage = conTab.FindChild("ImageTemp").gameObject;
            RectTransform con = conTab.FindChild("view/con").GetComponent<RectTransform>();
            for (int j = 0; j < con.childCount; j++)
            {
                Destroy(con.GetChild(j).gameObject);
            }
            int mun = 0;
            for (int i = 0; i < vipModel.GetPriMum(); i++)
            {
                if(vipModel.GetValue(lvl,i) != vipModel.GetValue(lvl-1,i))
                {
                    GameObject clon = (GameObject)Instantiate(tempImage);
                    clon.transform.SetParent(con, false);
                    Sprite icon_Image = GAMEAPI.ABUI_LoadSprite("icon_type_" + vipModel.GetType(lvl,i));
                    if (icon_Image != null)
                    clon.transform.FindChild("icon").GetComponent<Image>().sprite = icon_Image;
                    Text pri_text = clon.transform.FindChild("pri_text").GetComponent<Text>();
                    pri_text.text = vipModel.Gettype_Name(lvl,i);
                    clon.SetActive(true);
                    mun++;
                }
            }
            float childSizeX = tempImage.transform.GetComponent<RectTransform>().sizeDelta.x;
            Vector2 newSize = new Vector2(mun * childSizeX, con.sizeDelta.y);
            con.sizeDelta = newSize;
            con.anchoredPosition = new Vector2(0,con.anchoredPosition.y);
        }
        //刷新对应vip礼包数据显示
        private void OnVipGiftRefresh(Transform conTab, int lvl) 
        {
            if (lvl <= 0)
                return;
            GameObject tempImage = conTab.FindChild("ImageTemp").gameObject;
            RectTransform con = conTab.FindChild("view/con").GetComponent<RectTransform>();
            for (int j = 0; j < con.childCount; j++)
            {
                Destroy(con.GetChild(j).gameObject);
            }
            int mun = 0;
            Dictionary<int ,int> dic = new Dictionary<int,int> ();
            dic = vipModel.giftdata[vipModel.GetVipGiftListByLevel(lvl)];
            foreach (int it in dic.Keys) 
            {
                GameObject clon = (GameObject)Instantiate(tempImage);
                clon.transform.SetParent(con, false);
                Text item_text = clon.transform.FindChild("pri_text").GetComponent<Text>();
                uint id = (uint)it;
                item_text.text = a3_BagModel.getInstance().getItemDataById(id).item_name +"x"+dic[it];
                GameObject  con_item = clon.transform.FindChild("icon/icon_Img").gameObject;
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById(id), false , -1, 0.8f,false,-1,0,false ,false  );
                if (a3_BagModel.getInstance().getItemDataById(id).item_type ==2 )
                {
                    icon.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
                    icon.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
                }
                icon.transform.SetParent(con_item.transform, false);
                clon.SetActive(true);
                new BaseButton(clon.transform).onClick = (GameObject go) =>
                {
                    tip.SetActive(true);
                    a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
                    tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
                    tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
                    tip.transform.FindChild("text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
                    if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
                    else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
                    tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
                    tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

                    new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
                };
                mun++;
            } 
            
            float childSizeX = tempImage.transform.GetComponent<RectTransform>().sizeDelta.x;
            Vector2 newSize = new Vector2(mun * childSizeX, con.sizeDelta.y);
            con.sizeDelta = newSize;
        }
        //刷新领取礼包按钮
        public void OnGiftBtnRefresh() 
        {
            foreach (uint i in vipModel.isGetVipGift)
            {
                if (nowlook_viplvl == i)
                {
                    btnGetGift.interactable = false;
                    btnGetGift.transform.FindChild("text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_3");
                    break;
                }
                else 
                {
                    btnGetGift.interactable = true;
                    btnGetGift.transform.FindChild("text").GetComponent<Text>().text = ContMgr.getCont("active_recharge_l");
                }
            }
            
        }
        //点击领取礼包
        private void OnGetBtnClick(GameObject go)
        {
            debug.Log("On_Get_Btn_Click");
            //for (int i = 0; i <= vipModel.GetMaxVipLevel();i++ ) 
            //{
            //    foreach (int it in vipModel.GetVipAttByLevel(i)) 
            //    {
            //        print("vip等级:"+i+"数据"+it);
            //    }
            //}
            A3_VipProxy.getInstance().GetVipGift(nowlook_viplvl);
            A3_VipProxy.getInstance().GetVip();
            if (vipModel.Level < nowlook_viplvl)
            {
                flytxt.instance.fly(ContMgr.getCont("off_line_exp_vip"), 1);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("active_getSucc"), 1);
            }
        }
        //刷新icon
        private void OnRefreshObjByID(GameObject obj , int id)
        {
            //TODO 刷新ICON
            debug.Log("OnRefreshObjByID");
            obj.SetActive(true);
        }

        //点击充值
        private void OnRechargeBtnClick(GameObject go)
        {
            debug.Log("On_Recharge_Btn_Click");
            //flytxt.instance.fly("vip！！", 1);
            //vipModel.Exp += 50;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_VIP);
        }


        bool Toclose = false;
        //点击关闭
        private void OnCLoseClick(GameObject go)
        {
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_VIP);
        }
        private int iconIndex = 0;

        private void OpenVipGift(GameObject go) 
        {
            tab1.SetActive(true);
            tab2.SetActive(false);
            vipGiftBtn.interactable = false;
            vipPriBtn.interactable = true;
        }
        private void OpenVipPri(GameObject go)
        {
            tab1.SetActive(false);
            tab2.SetActive(true);
            vipGiftBtn.interactable = true;
            vipPriBtn.interactable = false;
        }

        public void setopen(int type)
        {
            if (type == 1)
                OpenVipGift(null);
            else if (type == 2)
                OpenVipPri(null);
        }

    }
}
