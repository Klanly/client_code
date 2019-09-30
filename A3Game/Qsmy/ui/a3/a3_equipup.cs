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
    class a3_equipup : FloatUi
    {
        static public a3_equipup instance;
        Transform con;
        GameObject item;
        public uint nowShow;
        public override void init()
        {
            instance = this;
            inText();
            showUse();
            //this.gameObject.SetActive(false);
            //con = this.transform.FindChild("scrollview/con");
            //item = this.transform.FindChild("Item").gameObject;
            //BaseButton btn_equip = new BaseButton(transform.FindChild("btn_equip"));
            //btn_equip.onClick = onEquip;
        }

        void inText()
        {
            this.transform.FindChild("bg/do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equipup_1");
        }
        public override void onShowed()
        {

            base.onShowed();
        }

        //public void checkShow(a3_BagItemData data)
        //{
        //    if (!data.isEquip)
        //        return;
        //    if (!data.isNew)
        //        return;

        //    if (a3_EquipModel.getInstance().checkisSelfEquip(data.confdata)
        //        && a3_EquipModel.getInstance().checkCanEquip(data))
        //    {
        //        if (!SelfRole.fsm.Autofighting)
        //        {
        //            if (!a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type))
        //            {
        //                refreshInfo(data);
        //            }
        //            else
        //            {
        //                a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
        //                if (data.equipdata.combpt > have_one.equipdata.combpt)
        //                {
        //                    refreshInfo(data);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            int eqpproc = AutoPlayModel.getInstance().EqpProc;
        //            for (int i = 0; i < 5; i++)
        //            {
        //                if ((eqpproc & (1 << i)) == 0 && data.confdata.equip_type == i+1)
        //                {
        //                    if (!a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.quality))
        //                    {
        //                        refreshInfo(data);
        //                    }
        //                    else
        //                    {
        //                        a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
        //                        if (data.equipdata.combpt > have_one.equipdata.combpt)
        //                        {
        //                            refreshInfo(data);
        //                        }
        //                    }
        //                }
        //                else 
        //                {

        //                }
        //            }
        //        }
        //    }
        //}

        //void refreshInfo(a3_BagItemData data)
        //{
        //    this.gameObject.SetActive(true);
        //    GameObject clon = (GameObject)Instantiate(item);
        //    clon.SetActive(true);
        //    clon.transform.SetParent(con,false);
        //    clon.transform.localRotation = con.transform.localRotation;
        //    uint curid = data.id;
        //    Transform Image = clon.transform.FindChild("icon");
        //    if (Image.childCount > 0)
        //    {
        //        Destroy(Image.GetChild(0).gameObject);
        //    }
        //    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
        //    icon.transform.SetParent(Image, false);

        //    clon.transform.FindChild("name").GetComponent<Text>().text =
        //        a3_BagModel.getInstance().getEquipNameInfo(data);

        //    int addcompt = 0;
        //    if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type))
        //    {
        //        a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
        //        addcompt = data.equipdata.combpt - have_one.equipdata.combpt;
        //    }
        //    else
        //    {
        //        addcompt = data.equipdata.combpt;
        //    }

        //    BaseButton btn_equip = new BaseButton(clon.transform.FindChild("btn_equip"));
        //    btn_equip.onClick = (GameObject go) => { 
        //        EquipProxy.getInstance().sendChangeEquip(curid);
        //        Destroy(clon);
        //    };
        //    BaseButton btn_close = new BaseButton(clon.transform);
        //    btn_close.onClick = (GameObject go) => 
        //    {
        //        Destroy(clon);
        //    };
        //    clon.transform.FindChild("compt").GetComponent<Text>().text = "+" + addcompt;
        //    Destroy(clon,15);
        //}

        public void showUse()
        {
            if (a3_BagModel.getInstance().newshow_summon .Count > 0)
            {
                this.gameObject.SetActive(true);
                foreach (uint i in a3_BagModel.getInstance().newshow_summon.Keys)
                {
                    refreshShow(a3_BagModel.getInstance().newshow_summon[i]);
                    nowShow = i;
                    return;
                }
            }
            else if (a3_BagModel.getInstance().newshow_item.Count > 0)
            {
                this.gameObject.SetActive(true);
                foreach (uint i in a3_BagModel.getInstance().newshow_item.Keys)
                {
                    refreshShow(a3_BagModel.getInstance().newshow_item[i]);
                    nowShow = i;
                    return;
                }
            }
            else if (a3_BagModel.getInstance().newshow_item.Count <= 0 && a3_BagModel.getInstance().neweqp.Count > 0)
            {
                this.gameObject.SetActive(true);
                foreach (uint i in a3_BagModel.getInstance().neweqp.Keys)
                {
                    refreshShow(a3_BagModel.getInstance().neweqp[i]);
                    nowShow = i;
                    return;
                }
            }
            else if (a3_BagModel.getInstance().newshow_item.Count <= 0 && a3_BagModel.getInstance().neweqp.Count <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void showbtnIcom(bool show)
        {
            if (show)
            {
                this.transform.FindChild("bg").localScale = Vector3.one;
            }else
                this.transform.FindChild("bg").localScale = Vector3.zero ;
        }

        void refreshShow(a3_BagItemData data)
        {
            Transform Image = this.transform.FindChild("bg/icon");
            if (Image.childCount > 0)
            {
                for (int i =0;i< Image.childCount;i++)
                {
                    Destroy(Image.GetChild(i).gameObject);
                }
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
            icon.transform.SetParent(Image, false);
            new BaseButton(this.transform.FindChild("bg/do")).onClick = (GameObject go) =>
            {
                if (data.isEquip)
                {
                    EquipProxy.getInstance().sendChangeEquip(data.id);
                }
                else if (data.confdata.use_type == 13 )
                {
                    a3_BagModel.getInstance().useItemByTpid(data.confdata.tpid, 1);
                }
                else if(data.confdata.use_type == 20)//宠物在开启功能之前获得
                {
                    if(FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
                    {

                        a3_BagModel.getInstance().useItemByTpid(data.confdata.tpid, 1);
                    }
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("func_limit_8"));
                    }
                }
                else if (data.isSummon)
                {
                    A3_SummonProxy.getInstance().sendUseSummon((uint)data.id);
                }
            };
            new BaseButton(this.transform.FindChild("bg/close")).onClick = (GameObject go) => 
            {
                if (data.isEquip)
                {
                    a3_BagModel.getInstance().neweqp.Remove(data.id);
                }
                else if (data.confdata.use_type == 13 || data.confdata.use_type == 20)
                {
                    a3_BagModel.getInstance().newshow_item.Remove(data.id);
                }
                else if (data.isSummon) {
                    a3_BagModel.getInstance().newshow_summon .Remove(data.id);
                }
                showUse();
            };
        }
        void Update() 
        {
            //if (con.childCount <= 0)
            //{
            //    this.gameObject.SetActive(false);
            //}
            //else
            //{
            //    RectTransform com = this.transform.FindChild("scrollview").GetComponent<RectTransform>();
            //    float childSizeY = con.GetComponent<GridLayoutGroup>().cellSize.y;
            //    //Vector2 newSize = new Vector2(com.position.x, -childSizeX*con.childCount); 
            //    com.offsetMax = new Vector2(com.offsetMax.x, childSizeY * (con.childCount)- 120);
            //    if (con.childCount >= 3)
            //    {
            //        com.offsetMax = new Vector2(com.offsetMax.x, childSizeY * (3)- 120);
            //    }
            //    //com.offsetMin = new Vector2(com.offsetMax.x, com.offsetMax.y);
            //    //con.sizeDelta = newSize;
            //}
        } 
        void timeGo()
        {
            this.gameObject.SetActive(false);
        }
         
        //void onEquip(GameObject go)
        //{
        //    CancelInvoke("timeGo");
        //    this.gameObject.SetActive(false);
        //    EquipProxy.getInstance().sendChangeEquip(curid);
        //}
    }
}
