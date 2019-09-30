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
    enum equip_tip_type
    {
        Comon_tip = 0,
        Bag_tip = 1,
        BagPick_tip = 2,
        HouseIn_tip = 3,
        HouseOut_tip = 4,
        SellIn_tip = 5,
        SellOut_tip = 6,
        SellNo_tip = 7,
        Chat_tip = 8,
        Equip_tip = 9,
        tip_ForLook = 10,
        tip_forfenjie = 11,
        tip_forAuc_buy = 12,
        tip_forAuc_put = 13,
        tip_forchushou = 14,
        hallowtips = 15,
        chatroom_display = 16,

    }

    class a3_equiptip : Window
    {
        bool is_sell_sure = false;
        uint curid;
        bool is_put_in = false;
        Vector3 local_pos;
        GameObject infoclone = null;
        equip_tip_type tiptype = equip_tip_type.Bag_tip;
        List<uint> dic_leftAllid = new List<uint>();
        private Toggle isMark;
        a3_BagItemData equip_data1;
        public static a3_equiptip instans;
        GameObject fristMark;
        uint inputeqp_id = 0;
        uint output_id = 0;

        private bool isFashion = false;

        List<Transform> txetClon = new List<Transform>();
        Transform AttCon;


        BaseButton btn_do;
        BaseButton btn_back;
        BaseButton btn_sell;
        BaseButton btn_put;
        BaseButton btn_fenjie;
        BaseButton btn_duanzao;
        BaseButton btn_out;
        BaseButton btn_out_bag;
        BaseButton btn_out_chushou;
        BaseButton btn_buy;
        BaseButton btn_buy_out;
        BaseButton btn_close;
        public override void init()
        {
            instans = this;
            inText();
            BaseButton btn_yes = new BaseButton(transform.FindChild("yes_or_no/yes"));
            btn_yes.onClick = onYes;

            BaseButton btn_no = new BaseButton(transform.FindChild("yes_or_no/no"));
            btn_no.onClick = onNo;

            BaseButton auto_AddPoint = new BaseButton(transform.FindChild("auto_addPiont/yes"));
            auto_AddPoint.onClick = onAddYes;
            BaseButton auto_NO = new BaseButton(transform.FindChild("auto_addPiont/no"));
            auto_NO.onClick = onAddNo;

            BaseButton btn_close_tach = new BaseButton(transform.FindChild("touch"));
            btn_close_tach.onClick = onclose;

            btn_do = new BaseButton(transform.FindChild("info/btns/contain/do"));
            btn_do.onClick = ondo;

            btn_back = new BaseButton(transform.FindChild("info/btns/contain/back"));
            btn_back.onClick = onback;

            btn_sell = new BaseButton(transform.FindChild("info/btns/contain/sell"));
            btn_sell.onClick = onsell;

            btn_put = new BaseButton(transform.FindChild("info/btns/contain/put"));
            btn_put.onClick = onput;

            btn_fenjie = new BaseButton(transform.FindChild("info/btns/contain/fenjie"));
            btn_fenjie.onClick = onequipsell;

            btn_duanzao = new BaseButton(transform.FindChild("info/btns/contain/input"));
            btn_duanzao.onClick = onduanzao;

            btn_out = new BaseButton(transform.FindChild("info/btns/contain/output"));
            btn_out.onClick = onOutput;

            btn_out_bag = new BaseButton(transform.FindChild("info/btns/contain/output_bagfenjie"));
            btn_out_bag.onClick = onOutput_bag;

            btn_out_chushou = new BaseButton(transform.FindChild("info/btns/contain/output_bagchushou"));
            btn_out_chushou.onClick = onOutchushou;

            BaseButton is_Mark = new BaseButton(transform.FindChild("info/isMark"));
            is_Mark.onClick = onIsMark;

            btn_buy = new BaseButton(transform.FindChild("info/btns/contain/buy"));
            btn_buy.onClick = onBuy;
            btn_buy_out = new BaseButton(transform.FindChild("info/btns/contain/putitem"));
            btn_buy_out.onClick = onputitem;

            btn_close = new BaseButton(transform.FindChild("info/btns/contain/closetip"));
            btn_close.onClick = onclose;
            //isMark = getComponentByPath<Toggle>("info/isMark");
            //isMark.onValueChanged.AddListener(sendMark);
            fristMark = this.transform.FindChild("info/isFirstMark").gameObject;

            local_pos = transform.FindChild("info").localPosition;

            for (int i = 0; i <= 7; i++)
            {
                txetClon.Add(transform.FindChild("info/attr_scroll/scroll/text" + i));
            }


        }

        void inText()
        {
            this.transform.FindChild("info/btns/contain/fenjie/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_1");//分解
            this.transform.FindChild("info/btns/contain/putitem/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_2");//放入
            this.transform.FindChild("info/btns/contain/closetip/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_3");//取消
            this.transform.FindChild("info/btns/contain/buy/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_4");//购买
            this.transform.FindChild("info/btns/contain/put/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_5");//存入
            this.transform.FindChild("info/btns/contain/sell/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_6");//出售
            this.transform.FindChild("info/btns/contain/output/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_7");//取出
            this.transform.FindChild("info/btns/contain/output_bagfenjie/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_8");//取出
            this.transform.FindChild("info/btns/contain/output_bagchushou/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_9");//取出
            this.transform.FindChild("info/btns/contain/input/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_10");//放入
            this.transform.FindChild("info/btns/contain/back/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_11");//锻造
            this.transform.FindChild("info/btns/contain/do/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_12");//装备
            this.transform.FindChild("info/face").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_13");//评分
            this.transform.FindChild("info/lvl_need").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_14");//等级
            this.transform.FindChild("info/job_need").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_15");//职业
            this.transform.FindChild("info/Refine_text/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_16");//精炼
            this.transform.FindChild("yes_or_no/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_17");//确定
            this.transform.FindChild("yes_or_no/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_18");//取消
            this.transform.FindChild("auto_addPiont/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_19");//是否自动加点以满足穿戴要求
            this.transform.FindChild("auto_addPiont/yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_20");//确定
            this.transform.FindChild("auto_addPiont/no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_equiptip_21");//取消
        }

        public string winName = null;
        public override void onShowed()
        {
            tiptype = equip_tip_type.Bag_tip;
            transform.SetAsLastSibling();
            transform.FindChild("yes_or_no").gameObject.SetActive(false);
            transform.FindChild("auto_addPiont").gameObject.SetActive(false);
            if (uiData == null)
                return;
            if (uiData.Count != 0)
            {
                equip_data1 = (a3_BagItemData)uiData[0];
                tiptype = (equip_tip_type)uiData[1];

                isFashion = equip_data1.confdata.equip_type == 11 || equip_data1.confdata.equip_type == 12 ? true : false;

                curid = equip_data1.id;
                inputeqp_id = curid;

                if (uiData.Count > 2)
                {
                    winName = (string)uiData[2];
                }
            }
            a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(equip_data1));
            mark();
            IsfirstMark();
            transform.FindChild("info/ig_up").gameObject.SetActive(false);
            transform.FindChild("info/ig_down").gameObject.SetActive(false);
            transform.FindChild("info/ig_equal").gameObject.SetActive(false);
            transform.FindChild("info/isequiped").gameObject.SetActive(false);
            btn_sell.gameObject.SetActive(false);
            btn_do.gameObject.SetActive(false);
            btn_back.gameObject.SetActive(false);
            btn_put.gameObject.SetActive(false);
            btn_fenjie.gameObject.SetActive(false);
            btn_duanzao.gameObject.SetActive(false);
            btn_out.gameObject.SetActive(false);
            btn_out_bag.gameObject.SetActive(false);
            btn_out_chushou.gameObject.SetActive(false);
            btn_close.gameObject.SetActive(false);
            btn_buy.gameObject.SetActive(false);
            btn_buy_out.gameObject.SetActive(false);
            if (tiptype == equip_tip_type.HouseOut_tip)
            {
                btn_put.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qc");
                btn_put.gameObject.SetActive(true);
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                //transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
                is_put_in = false;
            }
            else if (tiptype == equip_tip_type.HouseIn_tip)
            {
                btn_put.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_fr");
                btn_put.gameObject.SetActive(true);
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                //transform.FindChild("info/isFirstMark").gameObject.SetActive(false);

                    is_put_in = true;            
            }
            else if (tiptype == equip_tip_type.Bag_tip)
            {
                transform.FindChild("info/isequiped").gameObject.SetActive(true);
                btn_fenjie.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_xx");
                btn_put.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_dz");
                btn_fenjie.gameObject.SetActive(true);
                if (!isFashion)
                    btn_put.gameObject.SetActive(true);

            }
            else if (tiptype == equip_tip_type.BagPick_tip)
            {
                btn_do.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_zb");

                btn_do.gameObject.SetActive(true);

                if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP) && !isFashion)
                {
                    btn_back.gameObject.SetActive(true);
                }
                btn_fenjie.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_fj");

                if (!isFashion)
                {
                    btn_sell.gameObject.SetActive(true);
                    btn_fenjie.gameObject.SetActive(true);
                }
                transform.FindChild("info/isMark").gameObject.SetActive(true);
                if (a3_BagModel.getInstance().isFirstMark)
                {
                    transform.FindChild("info/isFirstMark").gameObject.SetActive(true);
                }

                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(equip_data1.confdata.equip_type))
                {
                    a3_BagItemData equip_data2 = a3_EquipModel.getInstance().getEquipsByType()[equip_data1.confdata.equip_type];
                    output_id = equip_data2.id;

                    GameObject go = transform.FindChild("info").gameObject;
                    infoclone = ((GameObject)GameObject.Instantiate(go));
                    infoclone.transform.SetParent(transform.FindChild("info_clon"), false);
                    infoclone.transform.localPosition = new Vector3(local_pos.x - 450, local_pos.y, local_pos.z);
                    infoclone.transform.FindChild("isequiped").gameObject.SetActive(true);

                    if (equip_data1.equipdata.combpt > equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_up").gameObject.SetActive(true);
                    else if (equip_data1.equipdata.combpt < equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_down").gameObject.SetActive(true);
                    else
                        go.transform.FindChild("ig_equal").gameObject.SetActive(true);
                    go.transform.localPosition = new Vector3(local_pos.x + 50, local_pos.y, local_pos.z);

                    infoclone.transform.FindChild("btns").gameObject.SetActive(false);
                    infoclone.transform.FindChild("isFirstMark").gameObject.SetActive(false);
                    infoclone.transform.FindChild("isMark").gameObject.SetActive(false);
                    initEquipInfo(infoclone.transform, equip_data2);
                }
            }
            else if (tiptype == equip_tip_type.SellIn_tip)
            {
                btn_fenjie.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_fj");
                btn_fenjie.gameObject.SetActive(true);
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                //transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
                is_sell_sure = true;
            }
            else if (tiptype == equip_tip_type.SellOut_tip)
            {
                btn_fenjie.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qc");
                btn_fenjie.gameObject.SetActive(true);
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                //transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
                is_sell_sure = false;
            }
            else if (tiptype == equip_tip_type.SellNo_tip)
            {
                btn_fenjie.transform.FindChild("Text").GetComponent<Text>().text = "";
                btn_fenjie.gameObject.SetActive(false);
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                //transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
            }
            else if (tiptype == equip_tip_type.Equip_tip)
            {
                if (a3_equip.instance)
                {
                    if (a3_equip.instance.tabIndex == 7)
                    {
                        if (a3_equip.instance.curInheritId3 == curid)
                        {
                            btn_out.gameObject.SetActive(true);
                        }
                        else
                        {
                            btn_duanzao.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        btn_duanzao.gameObject.SetActive(true);
                    }
                }
                //transform.FindChild("info/isMark").gameObject.SetActive(false);
                // transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
            }
            else if (tiptype == equip_tip_type.tip_ForLook)
            {
                btn_close.gameObject.SetActive(true);
            }
            else if (tiptype == equip_tip_type.tip_forfenjie)
            {
                btn_out_bag.gameObject.SetActive(true);
            }
            else if (tiptype == equip_tip_type.tip_forchushou)
            {
                btn_out_chushou.gameObject.SetActive(true);
            }
            else if (tiptype == equip_tip_type.tip_forAuc_buy)
            {
                btn_buy.gameObject.SetActive(true);
                btn_close.gameObject.SetActive(true);

                btn_do.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_zb");

                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(equip_data1.confdata.equip_type))
                {
                    a3_BagItemData equip_data2 = a3_EquipModel.getInstance().getEquipsByType()[equip_data1.confdata.equip_type];
                    output_id = equip_data2.id;

                    GameObject go = transform.FindChild("info").gameObject;
                    infoclone = ((GameObject)GameObject.Instantiate(go));
                    infoclone.transform.SetParent(transform.FindChild("info_clon"), false);
                    infoclone.transform.localPosition = new Vector3(local_pos.x - 450, local_pos.y, local_pos.z);
                    infoclone.transform.FindChild("isequiped").gameObject.SetActive(true);

                    if (equip_data1.equipdata.combpt > equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_up").gameObject.SetActive(true);
                    else if (equip_data1.equipdata.combpt < equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_down").gameObject.SetActive(true);
                    else
                        go.transform.FindChild("ig_equal").gameObject.SetActive(true);
                    go.transform.localPosition = new Vector3(local_pos.x + 50, local_pos.y, local_pos.z);

                    infoclone.transform.FindChild("btns").gameObject.SetActive(false);
                    initEquipInfo(infoclone.transform, equip_data2);
                }
            }
            else if (tiptype == equip_tip_type.tip_forAuc_put)
            {
                btn_buy_out.gameObject.SetActive(true);
                btn_close.gameObject.SetActive(true);
                btn_do.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_zb");

                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(equip_data1.confdata.equip_type))
                {
                    a3_BagItemData equip_data2 = a3_EquipModel.getInstance().getEquipsByType()[equip_data1.confdata.equip_type];
                    output_id = equip_data2.id;

                    GameObject go = transform.FindChild("info").gameObject;
                    infoclone = ((GameObject)GameObject.Instantiate(go));
                    infoclone.transform.SetParent(transform.FindChild("info_clon"), false);
                    infoclone.transform.localPosition = new Vector3(local_pos.x - 450, local_pos.y, local_pos.z);
                    infoclone.transform.FindChild("isequiped").gameObject.SetActive(true);

                    if (equip_data1.equipdata.combpt > equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_up").gameObject.SetActive(true);
                    else if (equip_data1.equipdata.combpt < equip_data2.equipdata.combpt)
                        go.transform.FindChild("ig_down").gameObject.SetActive(true);
                    else
                        go.transform.FindChild("ig_equal").gameObject.SetActive(true);
                    go.transform.localPosition = new Vector3(local_pos.x + 50, local_pos.y, local_pos.z);

                    infoclone.transform.FindChild("btns").gameObject.SetActive(false);
                    initEquipInfo(infoclone.transform, equip_data2);
                }
            }
            else if (tiptype == equip_tip_type.chatroom_display)
            {
                btn_back.gameObject.SetActive(false);
                btn_do.gameObject.SetActive(false);
                btn_sell.gameObject.SetActive(false);
                btn_fenjie.gameObject.SetActive(false);
            }


            Transform info = transform.FindChild("info");
            initEquipInfo(info, equip_data1);


            //EquipProxy.getInstance().send_Changebaoshi(equip_data1.id,1507);

            if (a3_bag.isshow)
            {
                if (a3_bag.isshow.eqpView.activeSelf == true)
                {
                    a3_bag.isshow.eqpView.SetActive(false);
                }
            }

        }
        public override void onClosed()
        {
            if (infoclone != null)
                Destroy(infoclone);
            infoclone = null;
            transform.FindChild("info/isMark").gameObject.SetActive(false);
            transform.FindChild("info/isFirstMark").gameObject.SetActive(false);
            transform.FindChild("info").localPosition = local_pos;

            if (a3_bag.isshow)
            {
                if (a3_bag.isshow.eqpView.activeSelf == false)
                {
                    a3_bag.isshow.eqpView.SetActive(true);
                }
            }
            winName = null;
        }

        void initEquipInfo(Transform info, a3_BagItemData equip_data)
        {

            info.FindChild("name").GetComponent<Text>().text =
                a3_BagModel.getInstance().getEquipNameInfo(equip_data);
            for (int i = 1; i <= 6; i++)
            {
                if (i == equip_data.confdata.quality)
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(true);
                }
                else
                {
                    info.FindChild("ig_bg/ig_" + i).gameObject.SetActive(false);
                }
            }

            info.FindChild("money").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_sj") + equip_data.confdata.value.ToString();
            info.FindChild("txt_value").GetComponent<Text>().text = equip_data.equipdata.combpt.ToString();
            info.FindChild("lv").GetComponent<Text>().text = equip_data.confdata.equip_level + ContMgr.getCont("a3_equiptip_jie") + Globle.getEquipTextByType(equip_data.confdata.equip_type);
            if (a3_BagModel.getInstance().isWorked(equip_data))
            {
                info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_can");
            }
            else
            {
                info.FindChild("transaction").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_nocan");
            }

            info.FindChild("Refine_text").GetComponent<Text>().text = equip_data.equipdata.stage + "/10";

            string str = "";
            switch (equip_data.confdata.job_limit)
            {
                case 1:
                    // str = "全职业";
                    str = ContMgr.getCont("profession1");
                    break;
                case 2:
                    //str = "狂战士";
                    str = ContMgr.getCont("profession2");
                    break;
                case 3:
                    //str = "法师";
                    str = ContMgr.getCont("profession3");
                    break;
                case 5:
                    //str = "暗影";;
                    str = ContMgr.getCont("profession5");
                    break;
            }
            if (!a3_EquipModel.getInstance().checkisSelfEquip(equip_data.confdata))
            {
                info.FindChild("profession").GetComponent<Text>().text = "<color=#f90e0e>" + str + "</color>";
            }
            else
            {
                info.FindChild("profession").GetComponent<Text>().text = str;
            }
            int lvl_up = equip_data.equipdata.stage;//a3_EquipModel.getInstance().Getlvl_up(equip_data);
            if (PlayerModel.getInstance().up_lvl >= lvl_up)
            {
                info.FindChild("lvl_need/text").GetComponent<Text>().text = lvl_up + ContMgr.getCont("zhuan");
            }
            else
            {
                info.FindChild("lvl_need/text").GetComponent<Text>().text = "<color=#f90e0e>" + lvl_up + ContMgr.getCont("zhuan") + "</color>";
            }

            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            if (a3_BagModel.getInstance().isMine(equip_data.id))
            {
                info.FindChild("show").gameObject.SetActive(true);
                new BaseButton(info.FindChild("show")).onClick = (GameObject go) =>
               {
                   chuli();//特殊处理一些界面
                   a3_chatroom._instance?.onItemClick(info.FindChild("show").gameObject, equip_data.id, true);
                   if (winName != null) InterfaceMgr.getInstance().close(winName);
                   InterfaceMgr.getInstance().close(this.uiName);
                   InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHATROOM);
               };
            }
            else
            {
                info.FindChild("show").gameObject.SetActive(false);
            }

            GameObject icon = IconImageMgr.getInstance().createA3EquipIcon(equip_data);
            icon.transform.FindChild("iconborder/equip_self").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/equip_canequip").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/is_upequip").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/is_new").gameObject.SetActive(false);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            GameObject taoNum_text = icon.transform.FindChild("iconborder/taoshu").gameObject;
            taoNum_text.SetActive(true);
            //string id = equip_data.tpid.ToString();
            //char[] idchar = id.ToCharArray();
            //int idtp = int.Parse(idchar[1].ToString()) + 1;
            int idtp = equip_data.confdata.equip_level;
            string str_text = ContMgr.getCont("di") + idtp + ContMgr.getCont("tao");
            switch (equip_data.confdata.quality)
            {
                case 1: str_text = "<color=#FFFFFF>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + "</color>"; break;
                case 2: str_text = "<color=#00FF00>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + "</color>"; break;
                case 3: str_text = "<color=#00BFFF>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + "</color>"; break;
                case 4: str_text = "<color=#FF00FF>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + " </color>"; break;
                case 5: str_text = "<color=#FFA500>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + "</color>"; break;
                case 6: str_text = "<color=#f90e0e>" + ContMgr.getCont("di") + idtp + ContMgr.getCont("tao") + "</color>"; break;
            }
            taoNum_text.GetComponent<Text>().text = str_text;
            icon.transform.SetParent(Image, false);

            for (int i = 1; i <= 10; i++)
            {
                if (i <= equip_data.equipdata.stage)
                    info.FindChild("stars/star" + i).gameObject.SetActive(true);
                else
                    info.FindChild("stars/star" + i).gameObject.SetActive(false);
            }








            initAtt(info, equip_data);

            if (isFashion)
            {
                info.FindChild("stars_bg").gameObject.SetActive(false);
                info.FindChild("stars").gameObject.SetActive(false);

                info.FindChild("Refine_text").gameObject.SetActive(false);

                info.FindChild("face").gameObject.SetActive(false);
                info.FindChild("txt_value").gameObject.SetActive(false);

                info.FindChild("lv").gameObject.SetActive(false);

                info.FindChild("zhufu/title").gameObject.SetActive(false);

                info.FindChild("transaction").gameObject.SetActive(false);

                info.FindChild("ig_up").gameObject.SetActive(false);
                info.FindChild("ig_down").gameObject.SetActive(false);
                info.FindChild("ig_equal").gameObject.SetActive(false);
                //详情
                //GameObject attr_contain  = info.FindChild( "attr_scroll/scroll/contain" ).gameObject;
                //bool b = false;
                //for ( int i = 0 ; i <  attr_contain.transform.childCount ; i++ )
                //{
                //    Transform item = attr_contain.transform.GetChild( i );
                //    if ( i != 0 && item.name == "text0(Clone)" )
                //    {
                //        b=true;
                //    }
                //    if ( b )
                //    {
                //        item.gameObject.SetActive( false );
                //    }
                //    else
                //    {
                //        item.gameObject.SetActive( true );
                //    }
                //}

            }
            else
            {
                info.FindChild("stars_bg").gameObject.SetActive(true);
                info.FindChild("stars").gameObject.SetActive(true);

                info.FindChild("Refine_text").gameObject.SetActive(true);

                info.FindChild("face").gameObject.SetActive(true);
                info.FindChild("txt_value").gameObject.SetActive(true);

                info.FindChild("lv").gameObject.SetActive(true);

                info.FindChild("zhufu/title").gameObject.SetActive(true);

                info.FindChild("transaction").gameObject.SetActive(true);

            }
        }
        int need1;
        int need2;
        string[] list_need1;
        string[] list_need2;
        void initAtt(Transform info, a3_BagItemData equip_data)
        {
            int conMon = 0;
            AttCon = info.transform.FindChild("attr_scroll/scroll/contain");
            for (int i = 0; i < AttCon.transform.childCount; i++)
            {
                Destroy(AttCon.GetChild(i).gameObject);
            }
            SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
            //Text text_basic_att = info.FindChild("attr_scroll/scroll/contain/attr1/text1").GetComponent<Text>();
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).GetNode("stage_info", "itemid==" + equip_data.tpid);

            //祝福
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + equip_data.equipdata.blessing_lv);
            info.FindChild("zhufu/title").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_zf") + equip_data.equipdata.blessing_lv
            + ContMgr.getCont("a3_equiptip_xq") + blessing.getInt("blessing_att") + "%）";
            list_need1 = stage_xml.getString("equip_limit1").Split(',');
            list_need2 = stage_xml.getString("equip_limit2").Split(',');
            need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            string text_need1, text_need2;
            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])])
            {
                //if (need1 <= 0)
                //{
                //    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + "-";
                //}
                //else
                //{
                //    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
                //}
                if ((int.Parse(list_need1[0]) == 1 && (PlayerModel.getInstance().profession == 3 && (equip_data.confdata.job_limit == 2 || equip_data.confdata.job_limit == 5))) ||
                    (int.Parse(list_need1[0]) == 1 && ((PlayerModel.getInstance().profession == 2 || PlayerModel.getInstance().profession == 5) && equip_data.confdata.job_limit == 3)))
                {
                    text_need1 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need1[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + 0 + "）" + "</color>";
                }
                //   text_need1 = "<color=#00FF56FF>" + ContMgr.getCont("a3_equiptip_need")+ Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") +PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
                else
                {
                    text_need1 = "<color=#00FF56FF>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need1[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
                }
            }
            else
            {
                //if (need1 <= 0) 
                //{
                //    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + "-";
                //}
                //else
                //{
                //    text_need1 = Globle.getAttrNameById(int.Parse(list_need1[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "</color>/" + need1;
                //}

                // text_need1 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getAttrNameById(int.Parse(list_need1[0])) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
                if ((int.Parse(list_need1[0]) == 1 && (PlayerModel.getInstance().profession == 3 && (equip_data.confdata.job_limit == 2 || equip_data.confdata.job_limit == 5))) || (int.Parse(list_need1[0]) == 1 && ((PlayerModel.getInstance().profession == 2 || PlayerModel.getInstance().profession == 5) && equip_data.confdata.job_limit == 3)))
                {
                    text_need1 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need1[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + 0 + "）" + "</color>";
                }
                else
                {
                    text_need1 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need1[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need1 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])] + "）" + "</color>";
                }

            }
            if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])])
            {
                //if (need2<=0) 
                //{
                //    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + "-";
                //}
                //else
                //{
                //    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#00FF00>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
                //}
                //     text_need2 = "<color=#00FF56FF>" + ContMgr.getCont("a3_equiptip_need") + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
                // text_need2 = "<color=#00FF56FF>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need2[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
                if ((int.Parse(list_need2[0]) == 1 && (PlayerModel.getInstance().profession == 3 && (equip_data.confdata.job_limit == 2 || equip_data.confdata.job_limit == 5))) || (int.Parse(list_need2[0]) == 1 && ((PlayerModel.getInstance().profession == 2 || PlayerModel.getInstance().profession == 5) && equip_data.confdata.job_limit == 3)))
                {
                    text_need2 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need2[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + 0 + "）" + "</color>";
                }
                else
                {
                    text_need2 = "<color=#00FF56FF>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need2[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
                }
            }
            else
            {
                //if (need2<=0)
                //{
                //    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + "-";
                //}
                //else
                //{
                //    text_need2 = Globle.getAttrNameById(int.Parse(list_need2[0])) + " <color=#f90e0e>" + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "</color>/" + need2;
                //}
                //  text_need2 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getAttrNameById(int.Parse(list_need2[0])) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";
                if ((int.Parse(list_need2[0]) == 1 && (PlayerModel.getInstance().profession == 3 && (equip_data.confdata.job_limit == 2 || equip_data.confdata.job_limit == 5))) || (int.Parse(list_need2[0]) == 1 && ((PlayerModel.getInstance().profession == 2 || PlayerModel.getInstance().profession == 5) && equip_data.confdata.job_limit == 3)))
                {
                    text_need2 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need2[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + 0 + "）" + "</color>";
                }
                else
                {
                    text_need2 = "<color=#f90e0e>" + ContMgr.getCont("a3_equiptip_need") + Globle.getString(int.Parse(list_need2[0]) + "_" + equip_data.confdata.job_limit + "_" + PlayerModel.getInstance().profession) + need2 + "（" + ContMgr.getCont("a3_equiptip_dangqian") + PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])] + "）" + "</color>";

                }
            }

            info.FindChild("zhufu/text1").GetComponent<Text>().text = text_need1;
            info.FindChild("zhufu/text2").GetComponent<Text>().text = text_need2;
            //荣耀之力
            SXML itemXMl = XMLMgr.instance.GetSXML("item");
            SXML xml = itemXMl.GetNode("item", "id==" + equip_data.tpid);
            if (xml != null)
            {
                //Vector3 pos1 = info.transform.FindChild("attr_scroll/pos1").GetComponent<RectTransform>().anchoredPosition;
                //Vector3 pos2 = info.transform.FindChild("attr_scroll/pos2").GetComponent<RectTransform>().anchoredPosition;
                SXML s_xml_hon = xml.GetNode("strength_of_honor");
                if (s_xml_hon != null && equip_data.equipdata.honor_num > 0)
                {
                    int maxmun = 0;
                    maxmun = s_xml_hon.getInt("max");
                    Transform AttName7 = Instantiate(txetClon[7]).transform;
                    AttName7.SetParent(AttCon, false);
                    AttName7.gameObject.SetActive(true);
                    conMon++;
                    AttName7.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_honor") + " + " + equip_data.equipdata.honor_num + "  (" + "Max + " + maxmun + ")";
                    //info.FindChild("attr_scroll/honor").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_honor") + " + " + equip_data.equipdata.honor_num + "  (" + "Max + " + maxmun + ")";
                    //info.FindChild("attr_scroll/honor").gameObject.SetActive(true);
                    //info.transform.FindChild("attr_scroll/scroll").GetComponent<RectTransform>().anchoredPosition = pos2;
                }
                //else
                //{
                //    info.FindChild("attr_scroll/honor").gameObject.SetActive(false);
                //    //info.transform.FindChild("attr_scroll/scroll").GetComponent<RectTransform>().anchoredPosition = pos1;
                //}
            }

            //基础属性
            string[] list_att0 = XMLMgr.instance.GetSXML("item.stage", "stage_level==0").GetNode("stage_info", "itemid==" + equip_data.tpid).getString("basic_att").Split(',');
            string[] list_att2 = stage_xml.getString("basic_att").Split(',');
            Transform AttName1 = Instantiate(txetClon[0]).transform;
            AttName1.SetParent(AttCon, false);
            AttName1.gameObject.SetActive(true);
            AttName1.GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_jcsx");
            conMon++;

            Transform att_basic = Instantiate(txetClon[1]).transform;
            att_basic.SetParent(AttCon, false);
            att_basic.gameObject.SetActive(true);
            conMon++;
            if (list_att0.Length > 1)
            {
                att_basic.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + " " + list_att0[0]
                    + "-" + list_att0[1];
            }
            else
            {
                att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0];
            }

            if (equip_data.equipdata.intensify_lv > 0)
            {
                Transform att_basic1 = Instantiate(txetClon[1]).transform;
                att_basic1.SetParent(AttCon, false);
                att_basic1.gameObject.SetActive(true);
                conMon++;
                SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + equip_data.equipdata.intensify_lv).GetNode("intensify_info", "itemid==" + equip_data.tpid);
                string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');
                int lv = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).getInt("extra");
                int add = 0;
                int add2 = 0;
                for (int i = 1; i <= equip_data.equipdata.intensify_lv; i++)
                {
                    //SXML _xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + i).GetNode("intensify_info", "itemid==" + equip_data.tpid);
                    if (list_att1.Length > 1)
                    {
                        add += int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                        add2 += int.Parse(intensify_xml.getString("intensify_att").Split(',')[1]) * lv / 100;
                    }
                    else
                    {
                        add += int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                    }
                }

                if (list_att1.Length > 1)
                {
                    att_basic1.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qhjc") + (add) + "-" + (add2);
                }
                else
                {
                    att_basic1.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_qhjc") + (add);
                }
            }

            if (equip_data.equipdata.stage > 0)
            {
                Transform att_basic2 = Instantiate(txetClon[1]).transform;
                att_basic2.SetParent(AttCon, false);
                att_basic2.gameObject.SetActive(true);
                conMon++;
                if (list_att0.Length > 1)
                {
                    int add1 = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                    int add2 = int.Parse(list_att2[1]) - int.Parse(list_att0[1]);

                    att_basic2.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_jljc") + add1 + "-" + add2;
                }
                else
                {
                    int add = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                    att_basic2.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_jljc") + add;
                }
            }




            //if (equip_data.equipdata.intensify_lv > 0)
            //{
            //    SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + equip_data.equipdata.intensify_lv).GetNode("intensify_info", "itemid==" + equip_data.tpid);
            //    string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');

            //    if (list_att1.Length > 1)
            //    {
            //        int add1 = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));
            //        int add2 = int.Parse(list_att1[1]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[1]) - int.Parse(list_att0[1]));

            //        att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
            //            + "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
            //    }
            //    else
            //    {
            //        int add = int.Parse(list_att1[0]) * equip_data.equipdata.intensify_lv + (int.Parse(list_att2[0]) - int.Parse(list_att0[0]));

            //        att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
            //    }
            //}
            //else if (equip_data.equipdata.stage > 0)
            //{
            //    if (list_att0.Length > 1)
            //    {
            //        int add1 = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
            //        int add2 = int.Parse(list_att2[1]) - int.Parse(list_att0[1]);

            //        att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add1 + "）</color>"
            //            + "-" + list_att0[1] + "<color=#00FF00>（+" + add2 + "）</color>";
            //    }
            //    else
            //    {
            //        int add = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
            //        att_basic.FindChild("Text").GetComponent<Text>().text = Globle.getAttrNameById(item_xml.getInt("att_type")) + " " + list_att0[0] + "<color=#00FF00>（+" + add + "）</color>";
            //    }
            //}
            //else
            //{

            //}

            //附加属性
            Transform AttName2 = Instantiate(txetClon[0]).transform;
            AttName2.SetParent(AttCon, false);
            AttName2.gameObject.SetActive(true);
            AttName2.GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_fjsx");
            AttName2.FindChild("count").GetComponent<Text>().text = equip_data.equipdata.subjoin_att.Count + "/5";
            conMon++;



            int subjoin_num = 0;
            SXML subjoin_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + equip_data.confdata.equip_level);
            foreach (int type in equip_data.equipdata.subjoin_att.Keys)
            {
                subjoin_num++;
                Transform att_Add = Instantiate(txetClon[2]).transform;
                att_Add.SetParent(AttCon, false);
                att_Add.gameObject.SetActive(true);
                conMon++;
                Text subjoin_text = att_Add.FindChild("Text").GetComponent<Text>();
                subjoin_text.text = Globle.getAttrAddById(type, equip_data.equipdata.subjoin_att[type]);
                SXML subjoin_xml_node = subjoin_xml.GetNode("subjoin_att_info", "att_type==" + type);
                if (equip_data.equipdata.subjoin_att[type] >= subjoin_xml_node.getInt("max"))
                {
                    att_Add.FindChild("max").gameObject.SetActive(true);
                }
                else
                {
                    att_Add.FindChild("max").gameObject.SetActive(false);
                }
            }

            if (isFashion)
            {
                AttName2.gameObject.SetActive(false);
            }

            //for (int i = subjoin_num + 1; i <= 5; i++)
            //{
            //    GameObject subjoin_go = info.FindChild("attr_scroll/scroll/contain/attr2/texts/scroll/contain/text" + i).gameObject;
            //    subjoin_go.transform.FindChild("max").gameObject.SetActive(false);
            //    subjoin_go.GetComponent<Text>().text = "无";
            //    subjoin_go.SetActive(false);
            //}

            ////if (equip_data.confdata.quality == 5 && equip_data.equipdata.extra_att!= null)
            ////{
            ////    foreach (int type in equip_data.equipdata.extra_att.Keys)
            ////    {
            ////        debug.Log("神器:" + Globle.getAttrAddById(type, equip_data.equipdata.extra_att[type]));
            ////    }
            ////}

            //宝石属性


            //Transform AttName4 = Instantiate(txetClon[0]).transform;
            //AttName4.SetParent(AttCon, false);
            //AttName4.gameObject.SetActive(true);
            //AttName4.GetComponent<Text>().text = "[宝石属性]";
            //conMon++;

            //int gem_num = 0;
            //foreach (int type in equip_data.equipdata.gem_att.Keys)
            //{
            //    gem_num++;
            //    Transform att_baoshi = Instantiate(txetClon[gem_num+2]).transform;
            //    att_baoshi.SetParent(AttCon, false);
            //    att_baoshi.gameObject.SetActive(true);
            //    conMon++;
            //    Text gem_text = att_baoshi.FindChild("Text").GetComponent<Text>();
            //    gem_text.text = Globle.getAttrAddById(type, equip_data.equipdata.gem_att[type]);
            //}
            ////info.FindChild("attr_scroll/scroll/contain/attr3").localPosition = new Vector3(0, -200 + (5 - subjoin_num) * 20, 0);

            //foreach (int i in equip_data.equipdata.baoshi.Keys)
            //{
            //    debug.Log("第" + i + "孔" + "的宝石id是" + equip_data.equipdata.baoshi[i]);
            //}
            //debug.Log("id:" + equip_data.tpid);

            if (equip_data.equipdata.baoshi != null)
            {

                Transform AttName6 = Instantiate(txetClon[0]).transform;
                AttName6.SetParent(AttCon, false);
                AttName6.gameObject.SetActive(true);
                AttName6.GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_bsxq");
                AttName6.FindChild("count").GetComponent<Text>().text = equip_data.equipdata.baoshi.Count + ContMgr.getCont("kong");
                conMon++;

                foreach (int i in equip_data.equipdata.baoshi.Keys)
                {
                    Transform att_baoshi = Instantiate(txetClon[3]).transform;
                    att_baoshi.SetParent(AttCon, false);
                    att_baoshi.gameObject.SetActive(true);
                    Image icon = att_baoshi.transform.FindChild("icon").GetComponent<Image>();
                    Text gem_text = att_baoshi.FindChild("Text").GetComponent<Text>();
                    if (equip_data.equipdata.baoshi[i] <= 0)
                    {
                        icon.gameObject.SetActive(false);
                        gem_text.text = ContMgr.getCont("a3_equiptip_kxq");
                    }
                    else
                    {
                        SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                        SXML s_xml = itemsXMl.GetNode("item", "id==" + equip_data.equipdata.baoshi[i]);
                        string File = "icon_item_" + s_xml.getString("icon_file");
                        icon.sprite = GAMEAPI.ABUI_LoadSprite(File);
                        SXML s_xml1 = itemsXMl.GetNode("gem_info", "item_id==" + equip_data.equipdata.baoshi[i]);
                        List<SXML> gem_add = s_xml1.GetNodeList("gem_add");
                        int add_type = 0;
                        int add_vaule = 0;
                        //bool ok = false;
                        foreach (SXML x in gem_add)
                        {
                            if (x.getInt("equip_type") == equip_data.confdata.equip_type)
                            {
                                add_type = x.getInt("att_type");
                                add_vaule = x.getInt("att_value");
                                break;
                            }
                        }
                        gem_text.text = s_xml.getString("item_name") + " " + Globle.getAttrAddById(add_type, add_vaule);
                    }
                    conMon++;
                }

                if (isFashion)
                {
                    AttName6.gameObject.SetActive(false);
                }
            }


            //追加属性
            Transform AttName5 = Instantiate(txetClon[0]).transform;
            AttName5.SetParent(AttCon, false);
            AttName5.gameObject.SetActive(true);
            AttName5.GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_zjsx");
            AttName5.FindChild("count").GetComponent<Text>().text = equip_data.equipdata.add_level + "/" + 20 * equip_data.equipdata.stage;
            conMon++;

            Transform att_outadd = Instantiate(txetClon[1]).transform;
            att_outadd.SetParent(AttCon, false);
            att_outadd.gameObject.SetActive(true);
            conMon++;
            //SXML add_xml = XMLMgr.instance.GetSXML("item.add_att", "add_level==" + (equip_data.equipdata.add_level + 1));
            //if (add_xml != null)
            //{
            //    float need_exp = add_xml.getFloat("add_exp");
            //    info.FindChild("bar/value").GetComponent<Image>().fillAmount = equip_data.equipdata.add_exp / need_exp;
            //}
            //else
            //{
            //    info.FindChild("bar/value").GetComponent<Image>().fillAmount = 1;
            //}

            SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
            int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
            int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * equip_data.equipdata.add_level;
            att_outadd.FindChild("Text").GetComponent<Text>().text = Globle.getAttrAddById(attType, attValue);

            if (isFashion)
            {
                AttName5.gameObject.SetActive(false);
                att_outadd.gameObject.SetActive(false);
            }

            //词缀属性
            if (equip_data.equipdata.att_type > 0)
            {
                Transform AttName3 = Instantiate(txetClon[0]).transform;
                AttName3.SetParent(AttCon, false);
                AttName3.gameObject.SetActive(true);
                AttName3.GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_lhsx");
                conMon++;

                Transform att_cizhui = Instantiate(txetClon[6]).transform;
                att_cizhui.SetParent(AttCon, false);
                att_cizhui.gameObject.SetActive(true);
                conMon++;
                Dictionary<int, a3_BagItemData> active_eqp;
                if (a3_targetinfo.isshow)
                {
                    active_eqp = a3_targetinfo.isshow.active_eqp;
                }
                else
                {
                    active_eqp = a3_EquipModel.getInstance().active_eqp;
                }
                Image icon = att_cizhui.transform.FindChild("Image").GetComponent<Image>();
                string File = "icon_shuxing_" + equip_data.equipdata.attribute;
                icon.sprite = GAMEAPI.ABUI_LoadSprite(File);
                string str = "";
                //if (equip_data.equipdata.att_type > 0)
                //{
                str = Globle.getAttrAddById(equip_data.equipdata.att_type, equip_data.equipdata.att_value);
                if (active_eqp.ContainsKey(equip_data.confdata.equip_type))
                {
                    if (active_eqp[equip_data.confdata.equip_type].id == equip_data.id)
                    {
                        str = getcolor(equip_data.equipdata.attribute) + str + ContMgr.getCont("a3_equiptip_yiyou") + Getcizhui(a3_EquipModel.getInstance().eqp_att_act[equip_data.equipdata.attribute]) + ContMgr.getCont("a3_equiptip_shuxing") + Globle.getEquipTextByType(a3_EquipModel.getInstance().eqp_type_act[equip_data.confdata.equip_type]) + ContMgr.getCont("a3_equiptip_jihuo") + "</color>";
                    }
                    else
                    {

                        str = "<color=#B3B1ACFF>" + str + ContMgr.getCont("a3_equiptip_dress") + Getcizhui(a3_EquipModel.getInstance().eqp_att_act[equip_data.equipdata.attribute]) + ContMgr.getCont("a3_equiptip_shuxing") + Globle.getEquipTextByType(a3_EquipModel.getInstance().eqp_type_act[equip_data.confdata.equip_type]) + ContMgr.getCont("a3_equiptip_jihuo") + "</color>";
                    }
                }
                else
                {
                    str = "<color=#B3B1ACFF>" + str + ContMgr.getCont("a3_equiptip_dress") + Getcizhui(a3_EquipModel.getInstance().eqp_att_act[equip_data.equipdata.attribute]) + ContMgr.getCont("a3_equiptip_shuxing") + Globle.getEquipTextByType(a3_EquipModel.getInstance().eqp_type_act[equip_data.confdata.equip_type]) + ContMgr.getCont("a3_equiptip_jihuo") + "</color>";
                }
                //}
                //else
                //{
                //    str = "<color=#4d3d3d>" + ContMgr.getCont("FriendProxy_wu") + "</color>";
                //}
                att_cizhui.FindChild("Text").GetComponent<Text>().text = str;

            }

            float childSizey = AttCon.GetComponent<GridLayoutGroup>().cellSize.y;
            float childSpacing = AttCon.GetComponent<GridLayoutGroup>().spacing.y;
            Vector2 newSize = new Vector2(AttCon.GetComponent<RectTransform>().sizeDelta.x, conMon * (childSizey + childSpacing) + childSpacing);
            AttCon.GetComponent<RectTransform>().sizeDelta = newSize;
        }

        //void inattForbag(uint thiseqp, int type, int value)
        //{
        //    if (a3_bag.indtans = null)
        //    {
        //        return;
        //    }
        //    if (thiseqp == inputeqp_id)
        //    {
        //        a3_bag.indtans.eqpatt[type] = value;
        //    }
        //    else if (thiseqp == output_id)
        //    {
        //        a3_bag.indtans.Uneqpatt[type] = value;
        //    }
        //}

        string getcolor(int tp)
        {

            string color = "<color=#FFFFFFFF>";
            switch (tp)
            {
                case 1: color = "<color=#58F731FF>"; break;
                case 2: color = "<color=#F77F31FF>"; break;
                case 3: color = "<color=#E9FF5FFF>"; break;
                case 4: color = "<color=#FF00CDFF>"; break;
                case 5: color = "<color=#5FC3FFFF>"; break;
                case 6: color = "<color=#f90e0e>"; break;
            }
            return color;
        }
        void onBuy(GameObject go)
        {
            if (curid != 0 && A3_AuctionModel.getInstance().GetItems()[curid].confdata.equip_type > 0)
            {
                uint cid = A3_AuctionModel.getInstance().GetItems()[curid].auctiondata.cid;
                int maxNum = A3_AuctionModel.getInstance().GetItems()[curid].num;
                A3_AuctionProxy.getInstance().SendBuyMsg(curid, cid, (uint)maxNum);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
        void onputitem(GameObject go)
        {
            if (a3_BagModel.getInstance().getItems()[curid].confdata.equip_type > 0)
            {
                if (curid != 0 && a3_BagModel.getInstance().isWorked(a3_BagModel.getInstance().getItems()[curid]))
                {
                    var bd = a3_BagModel.getInstance().getItems()[curid];
                    if (a3_auction_sell.instans != null)
                    {
                        a3_auction_sell.instans.SetInfo(bd);
                    }
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_equiptip_nojiaoyi"));
                }
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }

        void zhufu()
        {
        }
        void onAddYes(GameObject go)
        {
            int point2 = PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])];
            int point1 = PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])];
            int type_id1 = GetType_id(Globle.getAttrNameById(int.Parse(list_need1[0])));
            int type_id2 = GetType_id(Globle.getAttrNameById(int.Parse(list_need2[0])));
            if (need1 > point1 && need2 <= point2)
            {
                int count = need1 - PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])];
                Dictionary<int, int> add_pt = new Dictionary<int, int>();

                if (count <= PlayerModel.getInstance().pt_att)
                    add_pt.Add(type_id1, count);
                else
                    add_pt.Add(type_id1, PlayerModel.getInstance().pt_att);
                PlayerInfoProxy.getInstance().sendAddPoint(0, add_pt);
            }
            if (need2 > point2 && need1 <= point1)
            {
                int count = need2 - PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])];
                Dictionary<int, int> add_pt = new Dictionary<int, int>();
                if (count <= PlayerModel.getInstance().pt_att)
                    add_pt.Add(type_id2, count);
                else
                    add_pt.Add(type_id2, PlayerModel.getInstance().pt_att);
                PlayerInfoProxy.getInstance().sendAddPoint(0, add_pt);
            }
            if (need2 > point2 && need1 > point1)
            {
                int count1 = need1 - PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])];
                int count2 = need2 - PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])];
                Dictionary<int, int> add_pt = new Dictionary<int, int>();

                if (PlayerModel.getInstance().pt_att >= (count1 + count2))
                {
                    add_pt.Add(type_id1, count1);
                    add_pt.Add(type_id2, count2);
                }
                if (PlayerModel.getInstance().pt_att >= count1 && PlayerModel.getInstance().pt_att < (count1 + count2))
                {
                    int newCount = PlayerModel.getInstance().pt_att - count1;
                    add_pt.Add(type_id1, count1);
                    if (newCount != 0)
                        add_pt.Add(type_id2, newCount);
                }
                if (PlayerModel.getInstance().pt_att < count1)
                {
                    add_pt.Add(type_id1, count1);
                }
                PlayerInfoProxy.getInstance().sendAddPoint(0, add_pt);
            }
            EquipProxy.getInstance().sendChangeEquip(curid);
            transform.FindChild("auto_addPiont").gameObject.SetActive(false);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
        int GetType_id(string tpye)
        {
            int type_id = 0;
            if (tpye == ContMgr.getCont("globle_attr1"))
                type_id = 1;
            else if (tpye == ContMgr.getCont("globle_attr2"))
                type_id = 3;
            else if (tpye == ContMgr.getCont("globle_attr3"))
                type_id = 4;
            else if (tpye == ContMgr.getCont("globle_attr4"))
                type_id = 2;
            else if (tpye == ContMgr.getCont("globle_attr34"))
                type_id = 5;
            //switch (tpye)
            //{
            //    case "力量": type_id = 1; break;
            //    case "敏捷": type_id = 3; break;
            //    case "体力": type_id = 4; break;
            //    case "魔力": type_id = 2; break;
            //    case "智慧": type_id = 5; break;
            //}
            return type_id;
        }

        string Getcizhui(int type)
        {
            string str = "";
            switch (type)
            {
                case 1: str = ContMgr.getCont("a3_equiptip_f"); break;
                case 2: str = ContMgr.getCont("a3_equiptip_h"); break;
                case 3: str = ContMgr.getCont("a3_equiptip_g"); break;
                case 4: str = ContMgr.getCont("a3_equiptip_l"); break;
                case 5: str = ContMgr.getCont("a3_equiptip_b"); break;
            }
            return str;
        }

        void onAddNo(GameObject go)
        {
            transform.FindChild("auto_addPiont").gameObject.SetActive(false);
        }
        public void mark()
        {
            if (equip_data1.ismark)
            {
                getComponentByPath<Image>("info/isMark/Image").gameObject.SetActive(true);

            }
            else
            {
                getComponentByPath<Image>("info/isMark").GetComponent<Image>().enabled = true;
                getComponentByPath<Image>("info/isMark/Image").gameObject.SetActive(false);
            }
        }


        void chuli()
        {
            if (a3_bag.indtans && a3_bag.indtans.isbagToCK)
            {
                a3_bag.indtans.isbagToCK = false;
            }
        }
        void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
        void ondo(GameObject go)
        {
            if (go.transform.FindChild("Text").GetComponent<Text>().text == ContMgr.getCont("a3_equiptip_zb"))
            {
                //if (a3_BagModel.getInstance().getItems().Count >= a3_BagModel.getInstance().curi )
                //{
                // flytxt.instance.fly("背包空间不足！");
                // return;
                // InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                //}
                // else
                // {
                if (!a3_EquipModel.getInstance().checkCanEquip(equip_data1.confdata, equip_data1.equipdata.stage, equip_data1.equipdata.blessing_lv) && a3_EquipModel.getInstance().checkisSelfEquip(equip_data1.confdata))
                {
                    if (PlayerModel.getInstance().pt_att > 0)
                    {
                        transform.FindChild("auto_addPiont").gameObject.SetActive(true);
                    }
                }

                //}
            }
            EquipProxy.getInstance().sendChangeEquip(curid);
            //if (go.transform.FindChild("Text").GetComponent<Text>().text == "装备")
            //{
            //    if (!a3_EquipModel.getInstance().checkCanEquip(equip_data1.confdata, equip_data1.equipdata.stage, equip_data1.equipdata.blessing_lv) && a3_EquipModel.getInstance().checkisSelfEquip(equip_data1.confdata))
            //    {
            //        if (PlayerModel.getInstance().pt_att > 0)
            //        {
            //            transform.FindChild("auto_addPiont").gameObject.SetActive(true);
            //        }
            //    }
            //}
            //EquipProxy.getInstance().sendChangeEquip(curid);
        }

        void onback(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BAG);

            ArrayList data = new ArrayList();
            data.Add(curid);
            data.Add(true);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIP, data);
        }
        void onsell(GameObject go)
        {
            //InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
            //if (a3_BagModel.getInstance().HasBaoshi(equip_data1))
            //{
            //    flytxt.instance.fly("请先摘下宝石！");
            //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
            //}
            //else
            //{
            //    BagProxy.getInstance().sendSellItems(curid, 1);
            //}
            if (equip_data1.confdata.quality < 5 && a3_BagModel.getInstance().isWorked(equip_data1))
            {
                do_type = 1;
                onYes(go);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
            }
            else
            {
                do_type = 1;
                transform.FindChild("yes_or_no").gameObject.SetActive(true);
                setGetmoney();

            }
        }

        void onOutput(GameObject go)
        {
            if (a3_equip.instance)
            {
                a3_equip.instance.onOutEqp();
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
        void onOutput_bag(GameObject go)
        {
            if (piliang_fenjie.instance)
            {
                piliang_fenjie.instance.outItemCon_fenjie(-1, curid);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
        void onOutchushou(GameObject go)
        {
            if (piliang_chushou.instance)
            {
                piliang_chushou.instance.outItem_chushou(curid, 1, true);
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }

        void onput(GameObject go)
        {
            if (go.transform.FindChild("Text").GetComponent<Text>().text == ContMgr.getCont("a3_equiptip_dz"))
            {
                onback(go);
            }
            else
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                if (is_put_in && (a3_BagModel.getInstance().house_curi- a3_BagModel.getInstance().getHouseItems().Count )>= 1)
                {
                    BagProxy.getInstance().sendRoomItems(true, equip_data1.id, equip_data1.num);
                }
                else if (!is_put_in)
                {
                    BagProxy.getInstance().sendRoomItems(false, equip_data1.id, equip_data1.num);
                }
                else if ((a3_BagModel.getInstance().house_curi - a3_BagModel.getInstance().getHouseItems().Count) < 1)
                {
                    flytxt.instance.fly(ContMgr.getCont("cangkuyiman"));
                }
            }
        }

        public void IsfirstMark()
        {
            if (transform.FindChild("info/isMark").gameObject.activeSelf == true)
            {
                if (a3_BagModel.getInstance().isFirstMark)
                    fristMark.SetActive(true);
                else
                    fristMark.SetActive(false);
            }
        }
        int do_type = 0;
        void onYes(GameObject go)
        {
            if (do_type == 0)//分解确认
            {
                if (getComponentByPath<Image>("info/isMark/Image").gameObject.activeSelf == true)
                {
                    transform.FindChild("yes_or_no").gameObject.SetActive(false);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                    flytxt.instance.fly(ContMgr.getCont("a3_equiptip_lock"));
                }
                //else if (a3_BagModel.getInstance().HasBaoshi(equip_data1))
                //{
                //    flytxt.instance.fly("请先摘下宝石！");
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                //}
                else
                {
                    EquipProxy.getInstance().sendsell_one(curid);
                    getItemNum();
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                }
            }
            else if (do_type == 1)//出售确认
            {
                if (getComponentByPath<Image>("info/isMark/Image").gameObject.activeSelf == true)
                {
                    transform.FindChild("yes_or_no").gameObject.SetActive(false);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                    flytxt.instance.fly(ContMgr.getCont("a3_equiptip_lock"));
                }
                //else if (a3_BagModel.getInstance().HasBaoshi(equip_data1))
                //{
                //    flytxt.instance.fly("请先摘下宝石！");
                //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                //}
                else
                {
                    BagProxy.getInstance().sendSellItems(curid, 1);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                }
            }
        }
        void getItemNum()
        {
            if (a3_bag.indtans != null)
            {
                SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data1.tpid);
                List<SXML> xmls = xml.GetNodeList("decompose");
                foreach (SXML x in xmls)
                {
                    switch (x.getInt("item"))
                    {
                        case 1540:
                            a3_bag.isshow.mojing_num = x.getInt("num");
                            break;
                        case 1541:
                            a3_bag.isshow.shengguanghuiji_num = x.getInt("num");
                            break;
                        case 1542:
                            a3_bag.isshow.mifageli_num = x.getInt("num");
                            break;
                    }
                }
            }
        }
        void onNo(GameObject go)
        {
            transform.FindChild("yes_or_no").gameObject.SetActive(false);
        }
        void onequipsell(GameObject go)
        {
            if (go.transform.FindChild("Text").GetComponent<Text>().text == ContMgr.getCont("a3_equiptip_xx"))
            {
                ondo(go);
            }
            else
            {
                //dic_leftAllid.Clear();
                //dic_leftAllid.Add(equip_data1.id);
                if (equip_data1.confdata.quality < 5 && a3_BagModel.getInstance().isWorked(equip_data1))
                {
                    do_type = 0;
                    onYes(go);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
                }
                else
                {
                    do_type = 0;
                    transform.FindChild("yes_or_no").gameObject.SetActive(true);
                    setfjts();
                }
            }
        }


        void setfjts()
        {
            transform.FindChild("yes_or_no/toptext").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_sffj") + a3_BagModel.getInstance().getEquipNameInfo(equip_data1);
            string str = ContMgr.getCont("a3_equiptip_fjkhd");
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data1.tpid);
            List<SXML> xmls = xml.GetNodeList("decompose");
            bool f = true;
            foreach (SXML x in xmls)
            {
                SXML xx = XMLMgr.instance.GetSXML("item.item", "id==" + x.getInt("item"));
                if (f)
                {
                    str = str + "<color=#00FF00>" + x.getInt("num") + "</color>" + xx.getString("item_name");
                    f = false;
                }
                else
                {
                    str = str + "和" + "<color=#00FF00>" + x.getInt("num") + "</color>" + xx.getString("item_name");
                }
            }
            transform.FindChild("yes_or_no/canget").GetComponent<Text>().text = str;
        }

        void setGetmoney()
        {
            transform.FindChild("yes_or_no/toptext").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_sfcs") + a3_BagModel.getInstance().getEquipNameInfo(equip_data1);
            string str = ContMgr.getCont("a3_equiptip_cskhd");
            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data1.tpid);
            str = str + "<color=#FFD700>" + xml.getInt("value") + "</color>" + ContMgr.getCont("a3_equiptip_gold");
            transform.FindChild("yes_or_no/canget").GetComponent<Text>().text = str;
        }

        // bool canMark = true;
        void onIsMark(GameObject go)
        {
            //if (canMark)
            //{
            BagProxy.getInstance().sendMark(equip_data1.id);
            //CancelInvoke("wait_Time");
            //InvokeRepeating("wait_Time",0,1);
            if (getComponentByPath<Image>("info/isMark/Image").gameObject.activeSelf == true)
            {
                go.GetComponent<Image>().enabled = true;
                getComponentByPath<Image>("info/isMark/Image").gameObject.SetActive(false);
            }
            else
            {
                go.GetComponent<Image>().enabled = false;
                getComponentByPath<Image>("info/isMark/Image").gameObject.SetActive(true);
                flytxt.instance.fly(ContMgr.getCont("a3_equiptip_bsddzb"));
            }
            //}
            //else 
            //{
            //    if (getComponentByPath<Image>("info/isMark/Image").gameObject.activeSelf == true)
            //    {
            //        flytxt.instance.fly("您需要等一会才能取消标记");
            //    }
            //    else 
            //    {
            //        flytxt.instance.fly("您需要等一会才能进行标记操作");
            //    }
            //}
        }

        //void wait_Time() 
        //{
        //    waitTime--;
        //    if (waitTime <= 0)
        //    {
        //        waitTime = 0;
        //        CancelInvoke("wait_Time");
        //        canMark = true;
        //    }
        //}



        void onduanzao(GameObject go)
        {
            if (a3_equip.instance != null)
            {
                if (uiData != null)
                {
                    uint id = curid;
                    a3_equip.instance.onClickEquip(go, id);
                }
                else
                {
                    if (a3_equip.instance.equipicon.Count > 0)
                    {
                        a3_equip.instance.onClickEquip(go, a3_equip.instance.equipicon.Keys.First());
                    }
                }
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPTIP);
        }
    }
}
