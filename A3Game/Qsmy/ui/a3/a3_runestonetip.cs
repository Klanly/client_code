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
    enum runestone_tipstype
    {
        bag_tip=0,               //背包界面 
        compose_tips=1,          //合成界面
        dress_tip=2,             //穿戴界面
        decompose_tip=3,         //分解界面
        dressdown_tip=4          //穿好卸下


    }
    class a3_runestonetip : Window
    {

        BaseButton btn_sell;
        BaseButton btn_dress;
        BaseButton btn_out;
        BaseButton btn_decompose;
        BaseButton btn_down;
        BaseButton btn_close;


        BaseButton mask_btn,
                   nomask_btn;
        GameObject mask,
                   nomask,
                   icon_obj;


        Text name_txt;

        GameObject image,
                   contain;

        a3_BagItemData itemdata = new a3_BagItemData();
        runestone_tipstype tip_type = runestone_tipstype.bag_tip;
        public static a3_runestonetip _instance;
        public override void init()
        {


            getComponentByPath<Text>("info/btns/contain/fenjie/Text").text = ContMgr.getCont("a3_runestonetip_0");
            getComponentByPath<Text>("info/btns/contain/sell/Text").text = ContMgr.getCont("a3_runestonetip_1");
            getComponentByPath<Text>("info/btns/contain/output/Text").text = ContMgr.getCont("a3_runestonetip_2");
            getComponentByPath<Text>("info/btns/contain/do/Text").text = ContMgr.getCont("a3_runestonetip_3");
            getComponentByPath<Text>("info/btns/contain/down/Text").text = ContMgr.getCont("a3_runestonetip_4");



            _instance = this;
            btn_close = new BaseButton(getTransformByPath("bg"));
            btn_close.onClick = closeOnclick;
            btn_sell = new BaseButton(getTransformByPath("info/btns/contain/sell"));
            btn_sell.onClick = btn_sell_Onclick;
            btn_dress = new BaseButton(getTransformByPath("info/btns/contain/do"));
            btn_dress.onClick = btn_dress_Onclick;
            btn_out = new BaseButton(getTransformByPath("info/btns/contain/output"));
            btn_out.onClick = btn_out_Onclick;
            btn_decompose = new BaseButton(getTransformByPath("info/btns/contain/fenjie"));
            btn_decompose.onClick = btn_decompose_Onclick;
            btn_down = new BaseButton(getTransformByPath("info/btns/contain/down"));
            btn_down.onClick = btn_down_Onclick;

            name_txt = getComponentByPath<Text>("info/name");
            image = getGameObjectByPath("info/scrollview/Image");
            contain = getGameObjectByPath("info/scrollview/contain");
            icon_obj = getGameObjectByPath("info/icon");
            mask_btn = new BaseButton(getTransformByPath("info/Mask"));
            mask_btn.onClick = maskOnclick;
            nomask_btn = new BaseButton(getTransformByPath("info/noMask"));
            nomask_btn.onClick = nomaskOnclick;
            mask = mask_btn.gameObject;
            nomask = nomask_btn.gameObject;
        }
        public override void onShowed()
        {
            if (uiData != null)
            {
                itemdata = (a3_BagItemData)uiData[0];
                tip_type = (runestone_tipstype)uiData[1];
            }
            btn_sell.gameObject.SetActive(false);
            btn_dress.gameObject.SetActive(false);
            btn_out.gameObject.SetActive(false);
            btn_decompose.gameObject.SetActive(false);
            btn_down.gameObject.SetActive(false);
            getGameObjectByPath("info/noMask").SetActive(true);
            getGameObjectByPath("info/Mask").SetActive(true);
            switch (tip_type)
            {
                case runestone_tipstype.bag_tip:
                    btn_sell.gameObject.SetActive(true);
                    btn_dress.gameObject.SetActive(true);
                    btn_decompose.gameObject.SetActive(true);
                    break;
                case runestone_tipstype.compose_tips:
                    btn_decompose.gameObject.SetActive(true);
                    break;
                case runestone_tipstype.dress_tip:
                    btn_dress.gameObject.SetActive(true);
                    btn_decompose.gameObject.SetActive(true);
                    break;
                case runestone_tipstype.decompose_tip:
                    btn_out.gameObject.SetActive(true);
                    break;
                case runestone_tipstype.dressdown_tip:
                    btn_down.gameObject.SetActive(true);
                    getGameObjectByPath("info/noMask").SetActive(false);
                    getGameObjectByPath("info/Mask").SetActive(false);

                    break;
                default:
                    break;
            }
            infos();
            this.gameObject.transform.SetAsLastSibling();
        }
        public override void onClosed()
        {
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                Destroy(contain.transform.GetChild(i).gameObject);
            }
        }

        void closeOnclick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RUNESTONETIP);
        }
        void infos()
        {
            if(icon_obj.transform.childCount>0)
            {
                for(int i=0; i< icon_obj.transform.childCount;i++)
                {
                    Destroy(icon_obj.transform.GetChild(i).gameObject);
                }
                
            }

            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata, false, -1, 1);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            icon.transform.SetParent(icon_obj.transform);
            icon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            name_txt.text = a3_BagModel.getInstance().getRunestoneDataByid((int)itemdata.tpid).item_name;
            Dictionary<int, int> dic = itemdata.runestonedata.runeston_att;
            foreach (int i in dic.Keys)
            {
                GameObject image_clone = GameObject.Instantiate(image) as GameObject;
                image_clone.SetActive(true);
                image_clone.transform.SetParent(contain.transform);
                image_clone.GetComponent<Text>().text = Globle.getAttrNameById(i) + ":" + dic[i];
            }
            RectTransform tsm = contain.GetComponent<RectTransform>();
            RectTransform ts = image.GetComponent<RectTransform>();
            tsm.sizeDelta = new Vector2(tsm.sizeDelta.x, ts.sizeDelta.y * dic.Count);

            nomask.SetActive(itemdata.ismark ? false : true);
            mask.SetActive(itemdata.ismark ? true : false);
        }
        void maskOnclick(GameObject go)
        {
            a3_RuneStoneProxy.getInstance().sendporxy(7, (int)itemdata.id);
            mask.SetActive(false);
            nomask.SetActive(true);

        }
        void nomaskOnclick(GameObject go)
        {
            a3_RuneStoneProxy.getInstance().sendporxy(7, (int)itemdata.id);
            mask.SetActive(true);
            nomask.SetActive(false);
            flytxt.instance.fly(ContMgr.getCont("a3_equiptip_bsddzb"));

        }
        //出售
        void btn_sell_Onclick(GameObject go)
        {       
            BagProxy.getInstance().sendSellItems(itemdata.id,1);
            closeOnclick(go);
        }
        //穿上
        //a.背包界面(跳穿戴)
        //b.穿戴界面
        void btn_dress_Onclick(GameObject go)
        {
            ArrayList data = new ArrayList();
            data.Add(itemdata);
            closeOnclick(go);
            if (tip_type== runestone_tipstype.bag_tip)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RUNESTONE);
            }
            else if(tip_type == runestone_tipstype.dress_tip)
                a3_RuneStoneProxy.getInstance().sendporxy(1, (int)itemdata.id);
           

        }
        //取出
        void btn_out_Onclick(GameObject go)
        {
            closeOnclick(go);
            if (a3_runestone._instance != null)
                a3_runestone._instance.destoryIcon(itemdata.id);
        }
        //分解
        void btn_decompose_Onclick(GameObject go)
        {
            a3_RuneStoneProxy.getInstance().sendporxy(3, (int)itemdata.id);
            closeOnclick(go);
        }
        //脱下
        void btn_down_Onclick(GameObject go)
        {
            a3_RuneStoneProxy.getInstance().sendporxy(1, (int)itemdata.id);
            closeOnclick(go);
        }

        
    }
}
