using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;

namespace MuGame
{
    class a3_firstRechargeAward : Window
    {
        BaseButton btnRecharge;
        Transform content2d;
        GameObject m_EquipObj;
        GameObject m_Self_Camera;

        override public void init()
        {
            inText();
            btnRecharge = new BaseButton(transform.FindChild("body/btnRecharge"));
            content2d = transform.FindChild("body/awardItems/content");

            btnRecharge.onClick = onBtnRechargeClick;

            BaseButton btnClose = new BaseButton(transform.FindChild("title/btnClose"));
            btnClose.onClick = onBtnCloseClick;
            createItem();
            createEquip();
            this.getEventTrigerByPath("body/left/avatar_touch").onDrag = OnDrag;
            createPic();
        }

        void inText()
        {
            this.transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_firstRechargeAward_1");//使用等级：
            this.transform.FindChild("tip/text_bg/name/has").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_firstRechargeAward_2");//已拥有：
        }
        override public void onShowed()
        {
            welfareProxy.getInstance().addEventListener(welfareProxy.SUCCESSGETFIRSTRECHARGEAWARD, onSuccessGetFirstRechargeAward);

            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            Toclose = false;
            transform.FindChild("tip").gameObject.SetActive(false);
            if (welfareProxy.totalRecharge <= 0)
            {
                btnRecharge.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_1");
            }
            else
            {
                if (welfareProxy.firstRecharge <= 0)
                {
                    btnRecharge.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_2");
                    btnRecharge.removeAllListener();
                    btnRecharge.addEvent();
                    btnRecharge.onClick = onBtnDrawClick;

                }
                else
                {
                    btnRecharge.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_3");
                    btnRecharge.removeAllListener();

                }
            }
            //createEquip();

        }
        bool Toclose = false;
        override public void onClosed()
        {
            welfareProxy.getInstance().removeEventListener(welfareProxy.SUCCESSGETFIRSTRECHARGEAWARD, onSuccessGetFirstRechargeAward);

            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            disposeAvatar();
            InterfaceMgr.getInstance().itemToWin(Toclose, this.uiName);
        }
        void onBtnRechargeClick(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_FIRESTRECHARGEAWARD);
        }
        void onBtnDrawClick(GameObject go)
        {
            welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.firstRechange);
            InterfaceMgr.getInstance().close(this.uiName);
        }
        void onSuccessGetFirstRechargeAward(GameEvent e)
        {
            btnRecharge.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_3");
            btnRecharge.removeAllListener();
        }
        void onBtnCloseClick(GameObject go)
        {
            Toclose = true;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_FIRESTRECHARGEAWARD);
        }

        void createItem()
        {
            Dictionary<a3_ItemData, uint> itemDataList = WelfareModel.getInstance().getFirstChargeDataList();
            foreach (KeyValuePair<a3_ItemData, uint> item in itemDataList)
            {
                a3_ItemData itemData = item.Key;
                if (item.Value == 0)
                {
                    GameObject con = this.transform.FindChild("body/awardItems/item").gameObject;
                    GameObject clon = (GameObject)Instantiate(con);
                    clon.SetActive(true);
                    clon.transform.SetParent(content2d, false);
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemData, true, -1, 0.9f);
                    icon.transform.SetParent(clon.transform, false);
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                     {
                         setTip(item.Key);
                     };
                    //LayoutElement le = icon.AddComponent<LayoutElement>();
                    //le.minHeight = 100.0f;
                    //le.minWidth = 100.0f;
                    //le.preferredHeight = 100.0f;
                    //le.preferredWidth = 100.0f;
                    icon.gameObject.SetActive(true);
                    //icon.transform.localScale = Vector3.one;
                }
            }
        }
        void createEquip()
        {
            Dictionary<a3_ItemData, uint> itemDataList = WelfareModel.getInstance().getFirstChargeDataList();
            foreach (KeyValuePair<a3_ItemData, uint> item in itemDataList)
            {
                a3_ItemData itemData = item.Key;
                if (item.Value != 0)
                {
                    if (item.Value == (uint)PlayerModel.getInstance().profession)
                    {
                        GameObject con = this.transform.FindChild("body/awardItems/equip").gameObject;
                        GameObject clon = (GameObject)Instantiate(con);
                        clon.SetActive(true);
                        clon.transform.SetParent(content2d, false);
                        GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemData, true);
                        icon.transform.FindChild("iconborder").gameObject.SetActive(false);
                        icon.transform.SetParent(clon.transform, false);
                        icon.gameObject.SetActive(true);
                        new BaseButton(icon.transform).onClick = (GameObject go) =>
                        {
                            setTip(item.Key, true);
                        };
                    }

                }
            }
        }

        void setTip(a3_ItemData item, bool Eqp = false)
        {
            transform.FindChild("tip").gameObject.SetActive(true);
            transform.FindChild("tip/text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            transform.FindChild("tip/text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(item.tpid) + ContMgr.getCont("ge");
            transform.FindChild("tip/text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            if (Eqp)
            {
                transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_4");
                switch (item.job_limit)
                {
                    case 1:
                        transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi");
                        break;
                    case 2:
                        transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_p1");
                        break;
                    case 3:
                        transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_p2");
                        break;
                    case 5:
                        transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_p3");
                        break;
                }
            }
            else
            {
                transform.FindChild("tip/text_bg/name/lite").GetComponent<Text>().text = ContMgr.getCont("a3_firstRechargeAward_5");
                if (item.use_limit <= 0) { transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
                else { transform.FindChild("tip/text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            }
            transform.FindChild("tip/text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            transform.FindChild("tip/text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);
            new BaseButton(transform.FindChild("tip/close_btn")).onClick = (GameObject oo) => { transform.FindChild("tip").gameObject.SetActive(false); };
        }

        //创建角色
        public void createAvatar(int id, int profession)
        {
            if (m_EquipObj == null)
            {
                GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + getProfession(profession) + "_weaponl_l_" + id.ToString());
                if (obj_prefab == null) { obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + getProfession(profession) + "_weaponr_l_" + id.ToString()); }
                if (obj_prefab == null) { Debug.Log("无法找到模型文件."); return; }

                m_EquipObj = GameObject.Instantiate(obj_prefab, new Vector3(-128f, 0f, 0f), Quaternion.identity) as GameObject;


                foreach (Transform tran in m_EquipObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                m_EquipObj.transform.localPosition = new Vector3(0, 10, 0);
                m_EquipObj.transform.localScale = new Vector3(1, 1, 1);
                m_EquipObj.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                m_EquipObj.name = "UIEquip";

                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_ui_FirstRechargeCamera");
                m_Self_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
                Camera cam = m_Self_Camera.GetComponentInChildren<Camera>();
            }

        }

        void createPic()
        {
            if (SelfRole._inst is P5Assassin)
            {
                transform.FindChild("body/eqp").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_eqp_cw_assa");
            }
            else if (SelfRole._inst is P2Warrior)
            {
                transform.FindChild("body/eqp").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_eqp_cw_warrior");
            }
            else if (SelfRole._inst is P3Mage)
            {
                transform.FindChild("body/eqp").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_eqp_cw_mage");
            }
        }

        string getProfession(int profession)
        {
            string strProfession = string.Empty;
            switch (profession)
            {
                case 2:
                    strProfession = "warrior";
                    break;
                case 3:
                    strProfession = "mage";
                    break;
                case 5:
                    strProfession = "assa";
                    break;
                default:
                    break;
            }
            return strProfession;
        }
        //删除角色
        public void disposeAvatar()
        {
            if (m_EquipObj != null) GameObject.Destroy(m_EquipObj);
            if (m_Self_Camera != null) GameObject.Destroy(m_Self_Camera);
        }

        void OnDrag(GameObject go, Vector2 delta)
        {

            if (m_EquipObj != null)
            {
                m_EquipObj.transform.Rotate(Vector3.forward, -delta.x);
            }
        }
    }
}
