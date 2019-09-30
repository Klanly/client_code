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
    class a3_itemtip : Window
    {
        uint curid;
        a3_BagItemData item_data;
        Scrollbar num_bar;
        int cur_num;
        bool is_put_in = false;
        equip_tip_type tiptype = equip_tip_type.Bag_tip;
        Transform bodyNum;

        public static string closeWin = null;
        float recycle_price;//回收价格百分比（相对于该道具npcshop的商品价格）
        public override void init()
        {
            inText();
            BaseButton btn_close = new BaseButton(transform.FindChild("touch"));
            btn_close.onClick = onclose;

            BaseButton btn_do = new BaseButton(transform.FindChild("info/use"));
            btn_do.onClick = ondo;
            BaseButton btn_sell = new BaseButton(transform.FindChild("info/sell"));
            btn_sell.onClick = onsell;
            BaseButton btn_add = new BaseButton(transform.FindChild("info/bodyNum/btn_add"));
            btn_add.onClick = onadd;
            BaseButton btn_reduce = new BaseButton(transform.FindChild("info/bodyNum/btn_reduce"));
            btn_reduce.onClick = onreduce;
            BaseButton btn_put = new BaseButton(transform.FindChild("info/put"));
            btn_put.onClick = onput;
            new BaseButton(transform.FindChild("info/out")).onClick = onOut_chushou;



            BaseButton max = new BaseButton(transform.FindChild("info/bodyNum/max"));
            max.onClick = onmax;
            BaseButton min = new BaseButton(transform.FindChild("info/bodyNum/min"));
            min.onClick = onmin;


            num_bar = transform.FindChild("info/bodyNum/Scrollbar").GetComponent<Scrollbar>();
            num_bar.onValueChanged.AddListener(onNumChange);
            recycle_price = XMLMgr.instance.GetSXMLList("npc_shop.float_change")[0].getFloat("recycle_price")/100;
            bodyNum = transform.FindChild("info/bodyNum");
        }


        void inText()
        {
            this.transform.FindChild("info/use/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_1");//使用
            this.transform.FindChild("info/sell/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_2");//出售
            this.transform.FindChild("info/out/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_3");//取出
            this.transform.FindChild("info/put/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_4");//存入
            this.transform.FindChild("info/text_lv").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_5");//使用等级：
            this.transform.FindChild("info/bodyNum/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_6");//价值：
            this.transform.FindChild("info/bodyNum/min/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_7");//最小
            this.transform.FindChild("info/bodyNum/max/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_itemtip_8");//最大
        }
        private void onmin(GameObject obj)
        {
            cur_num = 1;
            num_bar.value = (float)cur_num / item_data.num;
            transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();
            int value;
            if (A3_NPCShopModel.getInstance().all_float.ContainsKey(item_data.tpid))
            {
                value = (int)(A3_NPCShopModel.getInstance().all_float[item_data.tpid] * recycle_price);
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (value * cur_num).ToString();
            }
            else
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (item_data.confdata.value * cur_num).ToString();
            needEvent = false;
        }

        private void onmax(GameObject obj)
        {
            cur_num = item_data.num;
            num_bar.value = (float)cur_num / item_data.num;
            transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();
            int value;
            if (A3_NPCShopModel.getInstance().all_float.ContainsKey(item_data.tpid))
            {
                value = (int)(A3_NPCShopModel.getInstance().all_float[item_data.tpid] * recycle_price);
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (value * cur_num).ToString();
            }
            else
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (item_data.confdata.value * cur_num).ToString();
            needEvent = false;
        }

        public override void onShowed()
        {
            tiptype = equip_tip_type.Bag_tip;

            transform.SetAsLastSibling();

            if (uiData == null)
                return;
            if (uiData.Count != 0)
            {
                item_data = (a3_BagItemData)uiData[0];
                curid = item_data.id;
                tiptype = (equip_tip_type)uiData[1];
            }


            transform.FindChild("info/use").gameObject.SetActive(false);
            transform.FindChild("info/sell").gameObject.SetActive(false);
            transform.FindChild("info/put").gameObject.SetActive(false);
            transform.FindChild("info/out").gameObject.SetActive(false);
            getGameObjectByPath("info/bodyNum").SetActive(true);
            getGameObjectByPath("info/bodyNum/Text").SetActive(true);
            getGameObjectByPath("info/bodyNum/value").SetActive(true);
            getGameObjectByPath("info/bodyNum/coin").SetActive(true);
            transform.FindChild("info/use/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_sy");
            transform.FindChild("info/sell/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_cs");
            transform.FindChild("info/out/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qc");
            bodyNum.gameObject.SetActive(true);
            if (tiptype == equip_tip_type.HouseOut_tip)
            {
                is_put_in = false;
                transform.FindChild("info/put/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qc");
                transform.FindChild("info/put").gameObject.SetActive(true);
            }
            else if (tiptype == equip_tip_type.tip_forchushou) {
                transform.FindChild("info/out").gameObject.SetActive(true);

            }
            else if (tiptype == equip_tip_type.HouseIn_tip)
            {
                is_put_in = true;
                transform.FindChild("info/put/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_fr");
                transform.FindChild("info/put").gameObject.SetActive(true);
            }
            else if (tiptype == equip_tip_type.Bag_tip)
            {
                transform.FindChild("info/sell").gameObject.SetActive(true);
                transform.FindChild("info/use").gameObject.SetActive(true);
                if (item_data.confdata.use_type == 21)
                {
                    transform.FindChild("info/use/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_dh");
                }
                else
                {
                    transform.FindChild("info/use/Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_sy");
                }
            }
            else if (tiptype == equip_tip_type.Chat_tip)
            {
                bodyNum.gameObject.SetActive(false);
            }
            else if (tiptype == equip_tip_type.hallowtips)
            {
                //1:背包里  2：圣器背包里  3：穿在身上
                if (uiData[2] == null)
                    return;
                switch ((int)uiData[2])
                {
                    case 1:
                        getGameObjectByPath("info/sell").SetActive(true);
                        getGameObjectByPath("info/use").SetActive(true);
                        getComponentByPath<Text>("info/use/Text").text = ContMgr.getCont("a3_equiptip_zb");
                        break;
                    case 2:
                        getGameObjectByPath("info/sell").SetActive(true);
                        getComponentByPath<Text>("info/sell/Text").text = ContMgr.getCont("a3_equiptip_fj");


                        getGameObjectByPath("info/use").SetActive(true);
                        getComponentByPath<Text>("info/use/Text").text = ContMgr.getCont("a3_equiptip_zb");

                        getGameObjectByPath("info/bodyNum/Text").SetActive(false);
                        getGameObjectByPath("info/bodyNum/value").SetActive(false);
                        getGameObjectByPath("info/bodyNum/coin").SetActive(false);
                        break;
                    case 3:
                        getGameObjectByPath("info/sell").SetActive(false);
                        getGameObjectByPath("info/use").SetActive(false);
                        getGameObjectByPath("info/out").SetActive(true);
                        getComponentByPath<Text>("info/out/Text").text = ContMgr.getCont("a3_equiptip_xx");
                        getGameObjectByPath("info/bodyNum").SetActive(false);
                        break;
                    default:
                        break;
                }

            }
            UiEventCenter.getInstance().onWinOpen(uiName);

            initItemInfo();
        }
        public override void onClosed()
        {
            if (!togo && closeWin !=null)
            {
                closeWin = null;
            }
        }
        void initItemInfo()
        {
            Transform info = transform.FindChild("info");

            for (int i = 1; i <= 6; i++)
            {
                if (i == item_data.confdata.quality)
                {
                    info.FindChild("ig_bg/" + i).gameObject.SetActive(true);
                }
                else
                {
                    info.FindChild("ig_bg/" + i).gameObject.SetActive(false);
                }
            }

            info.FindChild("name").GetComponent<Text>().text = item_data.confdata.item_name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(item_data.confdata.quality);

            info.FindChild("desc").GetComponent<Text>().text = StringUtils.formatText(item_data.confdata.desc);

            //info.FindChild("num").GetComponent<Text>().text = item_data.num.ToString();

            if (item_data.confdata.use_limit > 0)
            {
                //info.FindChild("lv").gameObject.SetActive(true);
                //info.FindChild("text_lv").gameObject.SetActive(true);
                info.FindChild("lv").GetComponent<Text>().text = item_data.confdata.use_limit + ContMgr.getCont("zhuan") + item_data.confdata.use_lv + ContMgr.getCont("ji");
            }
            else
            {
                //info.FindChild("lv").gameObject.SetActive(false);
                //info.FindChild("text_lv").gameObject.SetActive(false);
                info.FindChild("lv").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi");
            }

            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_data);
            icon.transform.SetParent(Image, false);

            num_bar.value = 0;
            cur_num = 1;

            if (item_data.confdata.use_type > 0)
            {
                if (item_data.confdata.use_type == 19)
                    transform.FindChild("info/use").GetComponent<Button>().interactable = false;
                else
                {
                    //if (item_data.confdata.use_type != 21)
                        transform.FindChild("info/use").GetComponent<Button>().interactable = true;
                    //else
                    //{
                    //    if (item_data.confdata.use_sum_require <= a3_BagModel.getInstance().getItemNumByTpid(item_data.confdata.tpid))
                    //    {
                    //        transform.FindChild("info/use").GetComponent<Button>().interactable = true;
                    //    }
                    //    else
                    //        transform.FindChild("info/use").GetComponent<Button>().interactable = false;
                    //}
                }
            }
            else
            {
                transform.FindChild("info/use").GetComponent<Button>().interactable = false;
                ///是不是圣器
                if (item_data.ishallows)
                    if ((int)uiData[2] == 2)
                    {
                        int type = A3_HallowsModel.getInstance().GetTypeByItemid((int)item_data.tpid);//位置
                        if (A3_HallowsModel.getInstance().now_hallows_dic.ContainsKey(type) && A3_HallowsModel.getInstance().now_hallows_dic[type].item_id == (int)item_data.tpid)
                            getComponentByPath<Button>("info/use").interactable = false;
                        else
                            getComponentByPath<Button>("info/use").interactable = true;
                    }
                    else
                    {
                        transform.FindChild("info/use").GetComponent<Button>().interactable = true;
                    }

            }

            onNumChange(0);


            if (item_data.confdata.use_type == 13)
            {
                transform.FindChild("info/bodyNum/btn_reduce").gameObject.SetActive(false);
                transform.FindChild("info/bodyNum/btn_add").gameObject.SetActive(false);
                transform.FindChild("info/bodyNum/bug").gameObject.SetActive(false);
                transform.FindChild("info/bodyNum/min").gameObject.SetActive(false);
                transform.FindChild("info/bodyNum/max").gameObject.SetActive(false);
                transform.FindChild("info/bodyNum/donum").gameObject.SetActive(false);
            }
            else {
                transform.FindChild("info/bodyNum/btn_reduce").gameObject.SetActive(true);
                transform.FindChild("info/bodyNum/btn_add").gameObject.SetActive(true);
                transform.FindChild("info/bodyNum/bug").gameObject.SetActive(true);
                transform.FindChild("info/bodyNum/min").gameObject.SetActive(true);
                transform.FindChild("info/bodyNum/max").gameObject.SetActive(true);
                transform.FindChild("info/bodyNum/donum").gameObject.SetActive(true);

            }
        }

        bool needEvent = true;
        void onNumChange(float rate)
        {
            if (!needEvent)
            {
                needEvent = true;
                return;
            }

            cur_num = (int)Math.Floor(rate * item_data.num);
            if (cur_num == 0)
                cur_num = 1;
            transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();
            int value;
            if (A3_NPCShopModel.getInstance().all_float.ContainsKey(item_data.tpid))
            {
                value = (int)(A3_NPCShopModel.getInstance().all_float[item_data.tpid] * recycle_price);
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (value * cur_num).ToString();
            }
            else
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (item_data.confdata.value * cur_num).ToString();
        }

        void onclose(GameObject go)
        {
            if (item_data.ishallows)
            {
                if ((int)uiData[2] == 3)
                {
                    if (a3_hallows.instance)
                        a3_hallows.instance.ShoworHideModel(true);
                }
            }
          InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
        }
        void onadd(GameObject go)
        {
            cur_num++;
            if (cur_num >= item_data.num)
                cur_num = item_data.num;
            num_bar.value = (float)cur_num / item_data.num;
            transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();
            int value;
            if (A3_NPCShopModel.getInstance().all_float.ContainsKey(item_data.tpid))
            {
                value = (int)(A3_NPCShopModel.getInstance().all_float[item_data.tpid] * recycle_price);
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (value * cur_num).ToString();
            }
            else
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (item_data.confdata.value * cur_num).ToString();
            needEvent = false;
        }
        void onreduce(GameObject go)
        {
            cur_num--;
            if (cur_num < 1)
                cur_num = 1;
            num_bar.value = (float)cur_num / item_data.num;
            transform.FindChild("info/bodyNum/donum").GetComponent<Text>().text = cur_num.ToString();
            int value;
            if (A3_NPCShopModel.getInstance().all_float.ContainsKey(item_data.tpid))
            {
                value = (int)(A3_NPCShopModel.getInstance().all_float[item_data.tpid] * recycle_price);
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (value * cur_num).ToString();
            }
            else
                transform.FindChild("info/bodyNum/value").GetComponent<Text>().text = (item_data.confdata.value * cur_num).ToString();
            needEvent = false;
        }
        void onput(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
            if (is_put_in)
            {
                BagProxy.getInstance().sendRoomItems(true, curid, cur_num);
            }
            else
            {
                BagProxy.getInstance().sendRoomItems(false, curid, cur_num);
            }
        }

        void onOut_chushou(GameObject go)
        {
            //圣器脱下
            if (item_data.ishallows)
            {
                if ((int)uiData[2] == 3)
                {
                    int type = A3_HallowsModel.getInstance().GetTypeByItemid((int)item_data.tpid);//位置
                    a3_hallows.instance.PutOrDown = false;
                    A3_HallowsProxy.getInstance().SendHallowsProxy(4, type,0);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
                    if (a3_hallows.instance)
                        a3_hallows.instance.ShoworHideModel(true);
                    return;
                }
            }
            if (piliang_chushou.instance)
            {
                piliang_chushou.instance.outItem_chushou(curid, cur_num);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
        }
        void onsell(GameObject go)
        {
            //圣器分解
            if (item_data.ishallows)
            {
                if ((int)uiData[2] == 2)
                {
                    a3_hallows.instance.AllCompose = false;
                    a3_hallows.instance.this_tpid = item_data.tpid;
                    List<Variant> lst = new List<Variant>();
                    Variant hcj = new Variant();
                    hcj["item_id"] = item_data.tpid;
                    hcj["item_num"] = cur_num;
                    lst.Add(hcj);
                    A3_HallowsProxy.getInstance().SendHallowsProxy(3, -1, -1, lst);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
                    return;
                }
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
            BagProxy.getInstance().sendSellItems(curid, cur_num);
        }

        bool togo = false;
        void ondo(GameObject go)
        {
            if (item_data.confdata.use_type == 21)
            {
                if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_itemtip_lock"));
                    return;
                }

                ArrayList v = new ArrayList();
                v.Add("tujian");
                v.Add(item_data.confdata.zhSummon);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW, v);
                if (closeWin != null)
                {
                    InterfaceMgr.getInstance().close(closeWin);
                    closeWin = null;
                    togo = true;
                }
            }
            else if (item_data.confdata.use_type == 23)
            {
                if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_itemtip_lock"));
                    return;
                }
                if (A3_SummonModel.getInstance().GetSummons().Count <= 0) {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_unllSummon"));
                    return;
                }
                ArrayList v = new ArrayList();
                v.Add("shuxing");
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW, v);
                if (closeWin != null)
                {
                    InterfaceMgr.getInstance().close(closeWin);
                    closeWin = null;
                    togo = true;
                }
            }
            else if (item_data.confdata.use_type == 24)
            {
                if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_itemtip_lock"));
                    return;
                }
                if (A3_SummonModel.getInstance().GetSummons().Count <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_unllSummon"));
                    return;
                }
                ArrayList v = new ArrayList();
                v.Add("xilian");
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW, v);
                if (closeWin != null)
                {
                    InterfaceMgr.getInstance().close(closeWin);
                    closeWin = null;
                    togo = true;
                }
            }
            else if (item_data.confdata.use_type == 25) {
                if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.SUMMON_MONSTER))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_itemtip_lock"));
                    return;
                }
                if (A3_SummonModel.getInstance().GetSummons().Count <= 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_unllSummon"));
                    return;
                }
                ArrayList v = new ArrayList();
                v.Add("shouhun");
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW, v);
                if (closeWin != null)
                {
                    InterfaceMgr.getInstance().close(closeWin);
                    closeWin = null;
                    togo = true;
                }
            }
            else if (item_data.confdata.use_type == 28)
            {
                ArrayList v = new ArrayList();
                v.Add(1); // type    1 表示 人物修改   2   表示军团修改
                v.Add(curid); // 道具id
                v.Add(cur_num);//道具使用数量
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHANGE_NAME,v);
            }
            //圣器装备
            else if (item_data.ishallows)
            {
                if ((int)uiData[2] == 2)
                {

                    int type = A3_HallowsModel.getInstance().GetTypeByItemid((int)item_data.tpid);//位置
                    if (a3_hallows.instance)
                        a3_hallows.instance.OldType_tpid = a3_hallows.instance.haveOrnoHalllow(type) ? (uint)A3_HallowsModel.getInstance().now_hallows()[type].item_id : 0;
                    a3_hallows.instance.PutOrDown = true;
                    A3_HallowsProxy.getInstance().SendHallowsProxy(4, type, (int)item_data.tpid);
                }
                else
                {
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HALLOWS);
                    return;
                }

            }
            else
            {
                BagProxy.getInstance().sendUseItems(curid, cur_num);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_ITEMTIP);
        }
    }
}
