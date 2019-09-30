using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

namespace MuGame
{
    class a3_resetlvl : Window
    {
        public static a3_resetlvl _instance;

        private GameObject m_SelfObj; //主角的avatar
        private ProfessionAvatar m_proAvatar;

        BaseButton btn_close;//关闭按钮
        BaseButton btn_description;//转生说明
        BaseButton btn_reincarnation;//转生
        BaseButton btn_closeDesc;//关闭转生描述按钮
        GameObject resetlvlDesc;//转生描述
        BaseButton touchBG;
        //left panel
        GameObject roleModle;//3D玩家模型对象
        Text lab_fightingCapacityValue;//玩家战斗力文本信息
        Text nextZhuanLvl;//下一转等级
        //right panel
        Text lab_level;//等级
        Text lab_experience;//经验
        string statusPointStr;//属性点
        Text lab_consumeGolds;//消耗金币
        Image txt_currentZhuan;//当前转生
        Image txt_targetZhuan;//目标转生
        Text lab_waradEquip;//x阶装备
        uint zhuan;//转生次数
        int profession;//职业
        //属性点对应的数字图片
        Image image1;
        Image image10;
        Image image100;
        Slider sliderExperience;
        public override void init()
        {


            getComponentByPath<Text>("body/rightbody/title/Text").text = ContMgr.getCont("a3_resetlvl_0");
            getComponentByPath<Text>("body/rightbody/body/unlock/Text").text = ContMgr.getCont("a3_resetlvl_2");
            getComponentByPath<Text>("body/leftbody/title/level/leveltext").text = ContMgr.getCont("a3_resetlvl_3");
            getComponentByPath<Text>("bottom/btn_reincarnation/Text").text = ContMgr.getCont("a3_resetlvl_4");
            getComponentByPath<Text>("resetlvlDesc/title/label").text = ContMgr.getCont("a3_resetlvl_5");
            getComponentByPath<Text>("resetlvlDesc/Text").text = ContMgr.getCont("a3_resetlvl_6");








            _instance = this;
            this.getEventTrigerByPath("body/leftbody/role/TouchDrag").onDrag = OnDrag;
            resetlvlDesc = transform.FindChild("resetlvlDesc").gameObject;
            touchBG = new BaseButton(transform.FindChild("resetlvlDesc/Touch"));
            btn_closeDesc = new BaseButton(transform.FindChild("resetlvlDesc/btn_close"));
            btn_closeDesc.onClick = onTouchBGClick;
            touchBG.onClick = onTouchBGClick;

            #region left panel
            roleModle = transform.FindChild("roledummy").gameObject;
            lab_fightingCapacityValue = transform.FindChild("body/leftbody/bottom/value").gameObject.GetComponent<Text>();
            
            #endregion left panel
            #region right panel
            lab_level = transform.FindChild("body/leftbody/title/level/value").gameObject.GetComponent<Text>();

            lab_experience = transform.FindChild("body/rightbody/body/experience/value").gameObject.GetComponent<Text>();

            image1 = transform.FindChild("body/rightbody/body/ScrollRectPanel/GridLayoutPanel/waradProperty/num/1").GetComponent<Image>();
            image10 = transform.FindChild("body/rightbody/body/ScrollRectPanel/GridLayoutPanel/waradProperty/num/10").GetComponent<Image>();
            image100 = transform.FindChild("body/rightbody/body/ScrollRectPanel/GridLayoutPanel/waradProperty/num/100").GetComponent<Image>();

            lab_consumeGolds = transform.FindChild("bottom/btn_reincarnation/consumeGolds/value").gameObject.GetComponent<Text>();

            lab_waradEquip = transform.FindChild("body/rightbody/body/ScrollRectPanel/GridLayoutPanel/waradEquip/desc").GetComponent<Text>();
            sliderExperience = transform.FindChild("body/rightbody/body/experience").GetComponent<Slider>();

            txt_currentZhuan = transform.FindChild("body/rightbody/title/currentZhuan/txtZhuan").GetComponent<Image>();
            txt_targetZhuan = transform.FindChild("body/rightbody/title/targetZhuan/txtZhuan").GetComponent<Image>();
            nextZhuanLvl = transform.FindChild("body/rightbody/title/txtLvl").GetComponent<Text>();
            #endregion right panel
            #region button click
            btn_close = new BaseButton(transform.FindChild("btn_close"));
            btn_close.onClick = onBtnCloseClick;

            btn_description = new BaseButton(transform.FindChild("title/btn_description"));
            btn_description.onClick = onDescriptionClick;

            btn_reincarnation = new BaseButton(transform.FindChild("bottom/btn_reincarnation"));
            btn_reincarnation.onClick = onReincarnationClick;
            #endregion button
          
        }

        public override void onShowed()
        {
            ResetLvLProxy.getInstance().addEventListener(ResetLvLProxy.EVENT_RESETLVL,onResetLvLSucc);

            lab_fightingCapacityValue.text = PlayerModel.getInstance().combpt.ToString();
            lab_level.text = ContMgr.getCont("a3_resetlvl_lv", new List<string>() { PlayerModel.getInstance().up_lvl.ToString(), PlayerModel.getInstance().lvl.ToString() });
            profession = PlayerModel.getInstance().profession;
            zhuan = PlayerModel.getInstance().up_lvl;
            uint lvl = PlayerModel.getInstance().lvl;
            uint exp = ResetLvLModel.getInstance().getExpByResetLvL(profession, zhuan, lvl);
            uint currentExp=PlayerModel.getInstance().exp>ResetLvLModel.getInstance().getAllExpByZhuan(profession,zhuan)?ResetLvLModel.getInstance().getAllExpByZhuan(profession,zhuan):PlayerModel.getInstance().exp;
            lab_experience.text = string.Format("{0}/{1}", currentExp, ResetLvLModel.getInstance().getAllExpByZhuan(profession, zhuan));// 玩家经验/当前转等级经验
            sliderExperience.maxValue = ResetLvLModel.getInstance().getAllExpByZhuan(profession, zhuan);
            sliderExperience.value = PlayerModel.getInstance().exp;
            statusPointStr = ResetLvLModel.getInstance().getAwardAttrPointByZhuan(profession, zhuan).ToString();
            createNum(uint.Parse(statusPointStr));
            lab_waradEquip.text = getAwardDescStr(zhuan);
            lab_consumeGolds.text = ResetLvLModel.getInstance().getNeedGoldsByZhuan(profession, zhuan).ToString();
            lab_consumeGolds.color = getGoldsColor();
            txt_currentZhuan.sprite = GAMEAPI.ABUI_LoadSprite("icon_resetlvl_" + zhuan);// zhuan.ToString()+"转";
            txt_targetZhuan.sprite = GAMEAPI.ABUI_LoadSprite("icon_resetlvl_" + (zhuan + 1));//(zhuan + 1).ToString() + "转";
            nextZhuanLvl.text =ContMgr.getCont("a3_resetlvl_lv", new List<string>() { (zhuan + 1).ToString(), ResetLvLModel.getInstance().getNextLvLByZhuan(profession, zhuan, currentExp).ToString() });
            createAvatar();
            btn_description.addEvent();
            btn_reincarnation.addEvent();
            btn_close.addEvent();
            touchBG.addEvent();
        }

        public override void onClosed()
        {
            ResetLvLProxy.getInstance().removeEventListener(ResetLvLProxy.EVENT_RESETLVL, onResetLvLSucc);
            btn_close.removeAllListener();
            btn_description.removeAllListener();
            btn_reincarnation.removeAllListener();
            touchBG.removeAllListener();
            disposeAvatar();
        }

        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }

        void onTouchBGClick(GameObject go)
        {
            if (resetlvlDesc.activeSelf) resetlvlDesc.SetActive(false);
     
        }

        void onBtnCloseClick(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RESETLVL);
        }

        void onDescriptionClick(GameObject go)
        {
            if (!resetlvlDesc.activeSelf) resetlvlDesc.SetActive(true);
        }

        void onReincarnationClick(GameObject go)
        {
            uint needGolds = ResetLvLModel.getInstance().getNeedGoldsByZhuan(profession,zhuan);
            if (needGolds > PlayerModel.getInstance().money)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RESETLVL);
                flytxt.instance.fly(ContMgr.getCont("comm_nomoney"));
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_GETGOLDWAY);
            }
            else
            {
                //向服务器发转生消息
                ResetLvLProxy.getInstance().sendResetLvL();
            }
        }

        void onResetLvLSucc(GameEvent e)
        {
            SceneCamera.CheckZhuanShengCam();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_RESETLVL);
        }

        //创建角色
        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                }
                else
                {
                    return;
                }


                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-128f, 0f, 0f), Quaternion.identity) as GameObject;

                Transform cur_model = m_SelfObj.transform.FindChild("model");
                m_SelfObj.transform.SetParent(roleModle.transform, true);
                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
                m_SelfObj.transform.localPosition = Vector3.zero;
                m_SelfObj.transform.localScale = new Vector3(1, 1, 1);
                m_SelfObj.transform.localRotation = Quaternion.Euler(0, 90, 0);
                m_SelfObj.name = "UIAvatar";
                //手上的小火球
                if (SelfRole._inst is P3Mage)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                m_proAvatar = new ProfessionAvatar();
                m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_FX, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);

                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
                m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
                m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
                m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
                m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());

                cur_model.Rotate(Vector3.up, 90f);
            }

        }
        //删除角色
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);

        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }

        void createNum(uint num)
        { 
         uint unm;
         uint.TryParse(statusPointStr, out unm);
         int counNum = num.ToString().Length;
         switch (counNum)
         {
             case 1:
                 image100.gameObject.SetActive(false);
                 image10.gameObject.SetActive(false);
                 image1.gameObject.SetActive(true);
                Sprite sp= GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString());
                image1.sprite = sp;
                 break;
             case 2:
                 image100.gameObject.SetActive(false);
                 image10.gameObject.SetActive(true);
                 image1.gameObject.SetActive(true);
                 Sprite sp1 = GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString().ToCharArray()[1]);
                 image1.sprite = sp1;
                  Sprite sp10 = GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString().ToCharArray()[0]);
                 image10.sprite = sp10;
                 break;
             case 3:
                 image100.gameObject.SetActive(true);
                 image10.gameObject.SetActive(true);
                 image1.gameObject.SetActive(true);
                 Sprite sp31 = GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString().ToCharArray()[1]);
                 image10.sprite = sp31;
                 Sprite sp310 = GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString().ToCharArray()[0]);
                 image100.sprite = sp310;
                  Sprite sp3100 = GAMEAPI.ABUI_LoadSprite("icon_uplvl_num_" + unm.ToString().ToCharArray()[2]);
                 image1.sprite = sp3100;
                 break;
             default:
                 break;
         }
        }
        string getAwardDescStr(uint carr) 
        {
           string str = string.Empty;
           List<ResetLvLAwardData> rrladList= ResetLvLAwardModel.getInstance().getAwardListById(carr);
            char[] charArr=  rrladList[3].name.ToArray();
            for (int i = 0; i < charArr.Length; i++)
            {
                str += charArr[i].ToString() + "\n";
            }
            return str;
        
        }
        Color getGoldsColor()
        {
            uint needGolds = ResetLvLModel.getInstance().getNeedGoldsByZhuan(profession,zhuan);
            return needGolds > PlayerModel.getInstance().money ?  Color.red:Color.green ;

        }
        
    }
}
