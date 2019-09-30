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
using DG.Tweening;

namespace MuGame
{
    class off_line_exp : Window
    {
        static public off_line_exp instance;
        private BaseButton btn1;
        private BaseButton btn2;
        private BaseButton btn3;
        private BaseButton btn4;
        private BaseButton closeBtn1;
       // private Slider timeSlider;
        private BaseButton left_click;
        private BaseButton right_click;
        //private Text baseExpState;
        private Text Text_time;
        private Text Text_exp;
        private GameObject recharge;
        private int currentType = 0;
        private List<BaseButton> lBtn = new List<BaseButton>();
        private List<BaseButton> lBtn2 = new List<BaseButton>();
        public List<a3_BagItemData> offline_item = new List<a3_BagItemData>();
        public bool offline;
        private OffLineModel offLineModel;
        GameObject eqp_icon;
        Transform contain;
        public  Toggle fenjie;
        public int mojing_num;
        public int shengguanghuiji_num;
        public int mifageli_num;
        private Image timeSlider;
        Transform TipOne;
        public override void init()
        {
            base.init();
            instance = this;
            contain = getGameObjectByPath("equp/scroll/contain").transform;
            eqp_icon = getGameObjectByPath("icon");
            left_click =new BaseButton(transform.FindChild("r_l_btn/left")); 
            right_click = new BaseButton(transform.FindChild("r_l_btn/right"));//getGameObjectByPath("r_l_btn/right").GetComponent<BaseButton>();

            Text_exp = transform.FindChild("exp_bg/exp").gameObject.GetComponent<Text>();
            fenjie = getComponentByPath<Toggle>("Toggle_fenjie");
            //fenjie.onValueChanged.AddListener(delegate (bool ison)
            //{
            //    if (ison) { EquipsSureSell();/* OnLoadItem_fenjie();*/ }
            //    else { outItemCon_fenjie(); /*EquipsNoSell(1);*/ }
            //});

        
            btn1 = new BaseButton(transform.FindChild("exp/btn1/btn"));
            btn2 = new BaseButton( transform.FindChild("exp/btn2/btn"));
            btn3 = new BaseButton(transform.FindChild("exp/btn3/btn"));
            btn4 =new BaseButton( transform.FindChild("exp/btn4/btn"));
            closeBtn1 = new BaseButton( transform.FindChild("closeBtn"));

            Text_time = this.getComponentByPath<Text>("state/Text_time");
            //timeSlider = this.getComponentByPath<Slider>("state/time_Slider");
            timeSlider = transform.FindChild("state/time_Slider/Fill Area/Fill").GetComponent<Image>();
            timeSlider.type = Image.Type.Filled;
            timeSlider.fillMethod = Image.FillMethod.Vertical;
            timeSlider.fillOrigin = (int) Image.OriginVertical.Bottom;

            vip_getexp_btn();
            btn1.onClick = delegate(GameObject go) { OnClickToGetExp(1); };
            btn2.onClick = delegate(GameObject go) { OnClickToGetExp(2); };
            btn3.onClick = delegate(GameObject go) { OnClickToGetExp(3); };
            btn4.onClick = delegate(GameObject go) { OnClickToGetExp(4); };
            left_click.onClick = delegate (GameObject go) { OnClick_left(); };
            right_click.onClick = delegate (GameObject go) { OnClick_right(); };
            closeBtn1.onClick = delegate(GameObject go) { OnClickToClose(); };
            recharge = transform.FindChild("recharge").gameObject;
            lBtn.Add(btn1);
            lBtn.Add(btn2);
            lBtn.Add(btn3);
            lBtn.Add(btn4);
            lBtn2.Add(closeBtn1);
            lBtn2.Add(left_click);
            lBtn2.Add(right_click);
            offLineModel = OffLineModel.getInstance();

            //btn3.transform.FindChild("Text").GetComponent<Text>().text = "VIP" + vip_lite(3) + "领取";
            //btn4.transform.FindChild("Text").GetComponent<Text>().text = "VIP" + vip_lite(4) + "领取";
            //if (PlayerModel.getInstance().last_time == 0)
            //{
            //    transform.FindChild("equp/image_con").gameObject.SetActive(true);
            //}
            //Debug.LogError(PlayerModel.getInstance().havePet + "sss" + PlayerModel.getInstance().last_time);
            //OffLineExpProxy.getInstance().Send_Off_Line(0);
            //OffLineExpProxy.getInstance().sendType(0);

            #region 初始化汉字提取
            getComponentByPath<Text>("exp/btn1/Text").text = ContMgr.getCont("off_line_exp_0");
            getComponentByPath<Text>("exp/btn2/btn/TextOnee").text = ContMgr.getCont("off_line_exp_1");
            getComponentByPath<Text>("state/Text_des").text = ContMgr.getCont("off_line_exp_2");
            getComponentByPath<Text>("state/Text_desc_1").text = ContMgr.getCont("off_line_exp_3");
            getComponentByPath<Text>("exp_bg/text").text = ContMgr.getCont("off_line_exp_4");
            getComponentByPath<Text>("recharge/Text").text = ContMgr.getCont("off_line_exp_5");
            getComponentByPath<Text>("recharge/yes/Text").text = ContMgr.getCont("off_line_exp_6");
            getComponentByPath<Text>("Toggle_fenjie/Label").text = ContMgr.getCont("piliang_fenjie_0");
            getComponentByPath<Text>("equp/image_con/text").text = ContMgr.getCont("off_line_exp_7");
            getComponentByPath<Text>("btn/btn1/text").text = ContMgr.getCont("off_line_exp_8");
            getComponentByPath<Text>("btn/btn2/text").text = ContMgr.getCont("off_line_exp_9");

            getComponentByPath<Text>("exp/btn1/btn/Text").text = ContMgr.getCont("off_line_exp_1");
            getComponentByPath<Text>("exp/btn3/btn/image1/Text").text = ContMgr.getCont("off_line_exp_1");
            getComponentByPath<Text>("exp/btn4/btn/image1/Text").text = ContMgr.getCont("off_line_exp_1");

            #endregion
        }

        private void outItemCon_fenjie()
        {
            
        }
        void vip_getexp_btn()
        {
            if (A3_VipModel.getInstance().Level < vip_lite(3))
            {
                btn3.transform.FindChild("image1").gameObject.SetActive(false);
                btn3.transform.FindChild("image2").gameObject.SetActive(true);
                btn3.transform.FindChild("image2/Text").GetComponent<Text>().text = "VIP" + vip_lite(3) + ContMgr.getCont("off_line_open");
            }
            else
            {
                btn3.transform.FindChild("image2").gameObject.SetActive(false);
                btn3.transform.FindChild("image1").gameObject.SetActive(true);
                btn3.transform.FindChild("image1/Text").GetComponent<Text>().text = ContMgr.getCont("off_line_lq");
            }
            if (A3_VipModel.getInstance().Level < vip_lite(4))
            {
                btn4.transform.FindChild("image1").gameObject.SetActive(false);
                btn4.transform.FindChild("image2").gameObject.SetActive(true);
                btn4.transform.FindChild("image2/Text").GetComponent<Text>().text = "VIP" + vip_lite(4) + ContMgr.getCont("off_line_open");
            }
            else
            {
                btn4.transform.FindChild("image2").gameObject.SetActive(false);
                btn4.transform.FindChild("image1").gameObject.SetActive(true);
                btn4.transform.FindChild("image1/Text").GetComponent<Text>().text = ContMgr.getCont("off_line_lq");
            }
        }
       
        private void OnClick_right()
        {
            float xx = eqp_icon.GetComponent<RectTransform>().sizeDelta.x;
            float y = contain.GetComponent<RectTransform>().anchoredPosition.y;
            float x = contain.GetComponent<RectTransform>().anchoredPosition.x;
            float r_po = (eqp_icon.GetComponent<RectTransform>().sizeDelta.x) * Mathf.CeilToInt(OffLineExpProxy.getInstance().eqp.Count / 4.0f);
            //Debug.LogError(r_po+"ss"+ eqp_icon.GetComponent<RectTransform>().sizeDelta.x+"ss"+ (OffLineExpProxy.getInstance().eqp.Count));
            float ss = -(r_po - eqp_icon.GetComponent<RectTransform>().sizeDelta.x * 4) + 10;
            //Debug.LogError(ss);
            if (contain.GetComponent<RectTransform>().anchoredPosition.x <= ss)
                return;         
            contain.GetComponent<RectTransform>().anchoredPosition = new Vector2(x - xx * 8, y);
            
        }

        private void OnClick_left()
        {
            float xx = eqp_icon.GetComponent<RectTransform>().sizeDelta.x;
            float y = contain.GetComponent<RectTransform>().anchoredPosition.y;
            float x = contain.GetComponent<RectTransform>().anchoredPosition.x;
            float r_po = (eqp_icon.GetComponent<RectTransform>().sizeDelta.x) * Mathf.CeilToInt(OffLineExpProxy.getInstance().eqp.Count / 4.0f);
            if (contain.GetComponent<RectTransform>().anchoredPosition.x >= r_po- eqp_icon.GetComponent<RectTransform>().sizeDelta.x * 4-10)
                return;
            contain.GetComponent<RectTransform>().anchoredPosition = new Vector2(x + xx * 8, y);
            
        }

        public override void onShowed()
        {
            if (PlayerModel.getInstance().last_time == 0&& OffLineExpProxy.getInstance().eqp.Count==0)
            {
                transform.FindChild("equp/image_con").gameObject.SetActive(true);
            }
            else
                transform.FindChild("equp/image_con").gameObject.SetActive(false);

            show_eqp();
            for (int i = 0; i < lBtn.Count; i++)
            {
                lBtn[i].addEvent();

                Text textExp = lBtn[i].transform.FindChild("Text_exp").GetComponent<Text>();
                if (offLineModel.ismaxlvl) { textExp.text = ContMgr.getCont("off_line_exp") + 0; }
                else
                {
                    textExp.text = ContMgr.getCont("off_line_exp") + (offLineModel.BaseExp * (i + 1));
                }
            }
            base.onShowed();
            vip_getexp_btn();
            OffLineExpProxy.getInstance().addEventListener(OffLineExpProxy.EVENT_OFFLINE_EXP_GET, doGetExp);
           // OffLineExpProxy.getInstance().addEventListener(OffLineExpProxy.EVENT_OFFLINE_EXP_GET, doGetExp);

            OnCostTextChange();
            OnTitleChange();
            offLineModel.OnOffLineTimeChange += OnTitleChange;
            offLineModel.OnBaseExpChange += OnCostTextChange;
        }
        private void show_eqp()
        {
            // Debug.LogError(OffLineExpProxy.getInstance().eqp.Count+"ssssssss");
            if (OffLineExpProxy.getInstance().eqp.Count <= 16)
            {
                contain.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                contain.GetComponent<GridLayoutGroup>().constraintCount = 8;
            }
            else
            {
                contain.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
                contain.GetComponent<GridLayoutGroup>().constraintCount = 2;
            }
            foreach (var d in OffLineExpProxy.getInstance().eqp)
            {
                
                GameObject objClone = GameObject.Instantiate(eqp_icon) as GameObject;
                objClone.SetActive(true);
                objClone.transform.SetParent(contain.transform, false);
                objClone.transform.FindChild("equp").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + d.tpid);
               
                objClone.transform.FindChild("quality_bg/" + d.confdata.equip_level).gameObject.SetActive(true);
                BaseButton btn = new BaseButton(objClone.transform.FindChild("equp").transform);
                btn.onClick = delegate (GameObject goo)
                {
                    ArrayList data1 = new ArrayList();
                    a3_BagItemData one = d;
                    data1.Add(one);
                    data1.Add(equip_tip_type.Comon_tip);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data1);
                };

            }
            if (OffLineExpProxy.getInstance().eqp.Count < 16)//每页16个，不足16个补上格子。
            {
                for (int i = 0; i < 16 - OffLineExpProxy.getInstance().eqp.Count; i++)
                {
                    GameObject objClone = GameObject.Instantiate(eqp_icon) as GameObject;
                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("equp").gameObject.SetActive(false);
                }
            }
            else//奇数变为偶数并且补上格子
            {
                if (OffLineExpProxy.getInstance().eqp.Count % 2 != 0)
                {
                    GameObject objClone = GameObject.Instantiate(eqp_icon) as GameObject;
                    objClone.SetActive(true);
                    objClone.transform.SetParent(contain.transform, false);
                    objClone.transform.FindChild("equp").gameObject.SetActive(false);
                }
            }
           // contain.GetComponent<RectTransform>().anchoredPosition = new Vector2((eqp_icon.GetComponent<RectTransform>().sizeDelta.x) * Mathf.CeilToInt(OffLineExpProxy.getInstance().eqp.Count / 2.0f), 0);
            contain.GetComponent<RectTransform>().sizeDelta = new Vector2((eqp_icon.GetComponent<RectTransform>().sizeDelta.x+2.5f) * Mathf.CeilToInt(OffLineExpProxy.getInstance().eqp.Count / 2.0f), (eqp_icon.GetComponent<RectTransform>().sizeDelta.x) * 2);


           
        }

        public override void onClosed()
        {
            foreach (BaseButton bs in lBtn)
            {
                bs.removeAllListener();
            }
            base.onClosed();
            OffLineExpProxy.getInstance().removeEventListener(OffLineExpProxy.EVENT_OFFLINE_EXP_GET, doGetExp);

            offLineModel.OnOffLineTimeChange -= OnTitleChange;
            offLineModel.OnBaseExpChange -= OnCostTextChange;
        }

        //关闭窗口
        void OnClickToClose()
        {
            if (contain.transform.childCount > 0)
            {
                for (int i = 0; i < contain.transform.childCount; i++)
                {
                    Destroy(contain.transform.GetChild(i).gameObject);
                }
            }
            InterfaceMgr.getInstance().close(InterfaceMgr.OFFLINEEXP);
        }

        //标题变化
        private void OnTitleChange()
        {

            string min = ((offLineModel.OffLineTime % 3600) / 60).ToString();
            string hour = (offLineModel.OffLineTime / 3600).ToString();

            Text_time.text = ContMgr.getCont("off_line_exp_time", new List<string> { hour, min });
            // timeSlider.value = (float)OffLineModel.getInstance().OffLineTime / (24 * 3600);
            timeSlider.fillAmount= (float)OffLineModel.getInstance().OffLineTime / (24 * 3600);
            if (offLineModel.ismaxlvl) { Text_exp.text = "EXP " + 0; }
            else
            {
                Text_exp.text = "EXP " + offLineModel.BaseExp;
            }
        }

        //初始化文字
        private void OnCostTextChange()
        {
            for (int i = 1; i < lBtn.Count; i++)
            {
                Text cost = lBtn[i].transform.FindChild("Text_cost").GetComponent<Text>();
                cost.text = ((int)offLineModel.GetCost(i + 1)).ToString();
            }
        }
        //像服务器发送请求
        int vip_lite(int type)
        {
            SXML _xml = OffLineModel.getInstance().OffLineXML.GetNode("rate_type", "type==" + type.ToString());
            int vip = _xml.getInt("vip_level");
            return vip;

        }
        private void OnClickToGetExp(int type)
        {
            A3_VipModel vipModel = A3_VipModel.getInstance();


            if (!offLineModel.CanGetExp)
            {
                flytxt.instance.fly(ContMgr.getCont("off_line_empty"));
                InterfaceMgr.getInstance().close(InterfaceMgr.OFFLINEEXP);
                a3_expbar.instance.getGameObjectByPath("operator/LightTips/btnAuto_off_line_exp").SetActive(false);
                return;
            }
            offline_item.Clear();
            offline = true;
            switch (type)
            {
                case 1:
                  
                    if (fenjie.isOn == true)
                        OffLineExpProxy.getInstance().sendType(1,true);
                    else
                    {
                        OffLineExpProxy.getInstance().sendType(1,false);
                      
                    }
                    currentType = 1;
                    break;
                case 2:
                   
                    if (PlayerModel.getInstance().money < OffLineModel.getInstance().GetCost(2))
                    {
                        flytxt.instance.fly(ContMgr.getCont("off_line_exp_money"));
                    }
                    else
                    {
                        if (fenjie.isOn == true)
                            OffLineExpProxy.getInstance().sendType(2, true);
                        else
                        {
                            OffLineExpProxy.getInstance().sendType(2, false);
                           

                              
                           
                        }
                    }
                    currentType = 2;
                    break;
                case 3:
                    if (PlayerModel.getInstance().gold < OffLineModel.getInstance().GetCost(3))
                    {

                        flytxt.instance.fly(ContMgr.getCont("off_line_exp_gold"));
                    }
                    //else if (vipModel.Level < vip_lite(3))
                    //    flytxt.instance.fly(ContMgr.getCont("off_line_exp_vip"));
                    else
                    {
                        if (fenjie.isOn == true)
                            OffLineExpProxy.getInstance().sendType(3, true);
                        else
                        {
                            OffLineExpProxy.getInstance().sendType(3, false);
                        }
                    }
                    currentType = 3;
                    break;
                case 4:
                    if (PlayerModel.getInstance().gold < OffLineModel.getInstance().GetCost(4))
                    {
                        flytxt.instance.fly(ContMgr.getCont("off_line_exp_gold"));
                    }
                    //else if (vipModel.Level < vip_lite(4))
                    //    flytxt.instance.fly(ContMgr.getCont("off_line_exp_vip"));
                    else
                    {
                        if (fenjie.isOn == true)
                            OffLineExpProxy.getInstance().sendType(4, true);
                        else
                        {
                            OffLineExpProxy.getInstance().sendType(4, false);
                        }
                    }
                    currentType = 4;
                    break;
                default:
                    break;
            }

        }

      

        //获得服务器反馈
        void doGetExp(GameEvent e)
        {

            if (off_line_exp.instance?.offline == true)
            {
                if (off_line_exp.instance.offline_item != null)
                {
                    foreach (var v in off_line_exp.instance.offline_item)
                    {
                        a3_ItemData item1 = a3_BagModel.getInstance().getItemDataById((uint)v.tpid);
                        GameObject go = IconImageMgr.getInstance().createA3ItemIconTip(itemid: (uint)v.tpid, num: v.num);
                        flytxt.instance.fly(null, 6, showIcon: go);
                    }
                }
                off_line_exp.instance.offline = false;
                off_line_exp.instance.offline_item.Clear();
            }




            Variant data = e.data;
            int res = data["res"];

            offLineModel.OffLineTime = 0;
            offLineModel.BaseExp = 0;
            debug.Log("离线经验的服务器反馈"+e.data.dump());
            InterfaceMgr.getInstance().close(InterfaceMgr.OFFLINEEXP);
        }
    }

}
