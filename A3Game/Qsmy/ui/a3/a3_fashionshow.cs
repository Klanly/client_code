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
using DG.Tweening;

namespace MuGame
{
    class a3_fashionshow : Window
    {

        BaseButton btn_close,
                   btn_retutn,
                   btn_buy_clsoe,
                   btn_buy,
                   btn_dress,
                   btn_buy_no,
                   btn_buy_yes,
                   btn_tips,
                   btn_tips_close;

        TabControl _tabControl;

        GameObject m_SelfObj, scene_Camera, scene_Obj;//角色的avatar，场景和摄像机
        ProfessionAvatar m_proAvatar;

        GameObject contain, image, tips;
        Toggle szToggle1,
               fyToggle1;
        /*info*/
        GameObject c_jihuo_obj;
        Text c_name,
             c_shuxing,
             c_info,
             c_time;
        GameObject jihuo_info;
        List<GameObject> lst_jihuo_infos = new List<GameObject>();
        /*buy*/
        GameObject jihuo_bg;
        GameObject btn_buy_obj,btn_dress_obj, buy_bg, contain1,panel,txt;
        GameObject[] buy_objs = new GameObject[3];

        /*self*/
        GameObject xiongjia_obj, wuqi_obj;
        int  nowchooseid_eqp=0;    //当前选中的武器
        int nowchooseid_body = 0;  //当前选中的胸甲
        int nowchooseid = 0;       //当前选中的id

        public static a3_fashionshow _insatnce;
        public override void init()
        {
            inittext();
            this.getEventTrigerByPath("left_bg/hero").onDrag = OnDrag;
            #region getobj
            contain = getGameObjectByPath("right_bg/top/scroll_view/contain");
            image = getGameObjectByPath("right_bg/top/scroll_view/image");
            c_jihuo_obj = getGameObjectByPath("right_bg/com/Panel/jihuo");
            c_name = getComponentByPath<Text>("right_bg/com/Panel/name");
            c_shuxing = getComponentByPath<Text>("right_bg/com/Panel/shuxing");
            c_info = getComponentByPath<Text>("right_bg/com/Panel/info");
            c_time = getComponentByPath<Text>("right_bg/com/Panel/time");
            btn_buy_obj = getGameObjectByPath("right_bg/down/buy");
            btn_dress_obj = getGameObjectByPath("right_bg/down/wear");
            buy_bg = getGameObjectByPath("buy_bg");
            contain1 = getGameObjectByPath("buy_bg/scollBuy/content");
            panel = getGameObjectByPath("right_bg/com/Panel");
            txt = getGameObjectByPath("right_bg/com/txt");
            xiongjia_obj = getGameObjectByPath("left_bg/xj");
            wuqi_obj = getGameObjectByPath("left_bg/wq");
            jihuo_info = getGameObjectByPath("right_bg/com/Panel/Panel");
            jihuo_bg = getGameObjectByPath("jihuo_bg");
            tips = getGameObjectByPath("tip");
            #endregion
            #region Btn
            btn_close = new BaseButton(getTransformByPath("btn_close"));
            btn_close.onClick = (GameObject go) => { InterfaceMgr.getInstance().close(InterfaceMgr.A3_FASHIONSHOW); };
            btn_retutn = new BaseButton(getTransformByPath("left_bg/btn_retutn"));
            btn_retutn.onClick = (GameObject go) => { returnOnclick(); };
            btn_buy_clsoe= new BaseButton(getTransformByPath("buy_bg/touchClose"));
            btn_buy_clsoe.onClick = (GameObject go) => { buy_bg.SetActive(false); };
            btn_buy = new BaseButton(getTransformByPath("right_bg/down/buy"));
            btn_buy.onClick =(GameObject go) => { /*buy_bg.SetActive(true);*/ openbuyOnClick(go); };
            btn_dress = new BaseButton(getTransformByPath("right_bg/down/wear"));
            btn_dress.onClick = (GameObject go) => { dressinOnClick(go); };
            btn_buy_no = new BaseButton(getTransformByPath("jihuo_bg/no"));
            btn_buy_no.onClick = (GameObject go) => { jihuo_bg.SetActive(false); };
            btn_buy_yes= new BaseButton(getTransformByPath("jihuo_bg/yes"));
            btn_buy_yes.onClick = (GameObject go) => {

                if (enough_item)
                {
                    jihuo_bg.SetActive(false);
                    a3_fashionshowProxy.getInstance().SendProxys(3, null, nowchooseid, jihuo_type);
                }
                else
                {
                    jihuo_bg.SetActive(false);
                    ArrayList data1 = new ArrayList();
                    data1.Add(a3_BagModel.getInstance().getItemDataById(need_id));
                    data1.Add(InterfaceMgr.A3_FASHIONSHOW);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                }
                    //flytxt.instance.fly(ContMgr.getCont("a3_fashionshow22"));
               
            };
            btn_tips = new BaseButton(getTransformByPath("left_bg/btn_tip"));
            btn_tips.onClick = (GameObject go) => { tips.SetActive(true);};
            btn_tips_close = new BaseButton(getTransformByPath("tip/close"));
            btn_tips_close.onClick = (GameObject go) => { tips.SetActive(false); };


            szToggle1 = getComponentByPath<Toggle>("right_bg/down/shizhuang");
            szToggle1.onValueChanged.AddListener((bool v) =>
            {
                bool shows = szToggle1.isOn ? false : true;
                a3_fashionshowProxy.getInstance().SendProxys(4, null, show: shows);
            });
            fyToggle1 = getComponentByPath<Toggle>("right_bg/down/feiyi");
            fyToggle1.onValueChanged.AddListener((bool v) =>
            {

                if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING))
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_fashionshow16"));
                    fyToggle1.isOn = true;
                }
                else
                {
                    if (!v)
                        m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
                    else
                        m_proAvatar.set_wing(0, 0);


                }
               // bool shows = szToggle1.isOn ? false : true;
               //a3_fashionshowProxy.getInstance().SendProxys(4, null, show: shows);
            });
            #endregion
            #region TabControl
            _tabControl = new TabControl();
            _tabControl.onClickHanle = OnSwitch;
            _tabControl.create(getGameObjectByPath("right_bg/top/Panel"), this.gameObject);
            #endregion

            creatrveObj();

           
        }

        void inittext()
        {
            getComponentByPath<Text>("right_bg/top/Panel/btn_xj/Text").text = ContMgr.getCont("a3_fashionshow0");
            getComponentByPath<Text>("right_bg/top/Panel/btn_wq/Text").text = ContMgr.getCont("a3_fashionshow1");
            getComponentByPath<Text>("right_bg/com/Panel/txt").text = ContMgr.getCont("a3_fashionshow2");
            getComponentByPath<Text>("right_bg/com/Panel/jihuo/Text").text = ContMgr.getCont("a3_fashionshow3");
            getComponentByPath<Text>("right_bg/com/txt").text = ContMgr.getCont("a3_fashionshow4");
            getComponentByPath<Text>("right_bg/down/buy/Text").text = ContMgr.getCont("a3_fashionshow5");
            getComponentByPath<Text>("right_bg/down/wear/Text").text = ContMgr.getCont("a3_fashionshow6");
            getComponentByPath<Text>("right_bg/down/shizhuang/Label").text = ContMgr.getCont("a3_fashionshow7");
            getComponentByPath<Text>("right_bg/down/feiyi/Label").text = ContMgr.getCont("a3_fashionshow8");
            getComponentByPath<Text>("buy_bg/scollBuy/content/0/btn/buy").text = ContMgr.getCont("a3_fashionshow5");
            getComponentByPath<Text>("buy_bg/scollBuy/content/1/btn/buy").text = ContMgr.getCont("a3_fashionshow5");
            getComponentByPath<Text>("jihuo_bg/no/Text").text = ContMgr.getCont("a3_fashionshow20");
            getComponentByPath<Text>("jihuo_bg/yes/Text").text = ContMgr.getCont("a3_fashionshow21");
            getComponentByPath<Text>("tip/Text").text = ContMgr.getCont("a3_fashionshow23")+"\n"+ ContMgr.getCont("a3_fashionshow24")+
               "\n" + ContMgr.getCont("a3_fashionshow25")+
                "\n" + ContMgr.getCont("a3_fashionshow26")+
               "\n" + ContMgr.getCont("a3_fashionshow27")+
               "\n" + ContMgr.getCont("a3_fashionshow28")+
               "\n" + ContMgr.getCont("a3_fashionshow29");
        }
        public override void onShowed()
        {
            _insatnce = this;
            nowchooseid_eqp = 0;
            nowchooseid_body = 0;
            nowchooseid = 0;
            #region 场景模型加载
            createAvatar();
            createAvatar_body();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            #endregion

            //_tabControl.setSelectedIndex(0, true);

            a3_fashionshowProxy.getInstance().SendProxys(1, null);
            refreshObj();
            RefreshSelfObj();
        }

        public override void onClosed()
        {
          #region 场景模型加载
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            disposeAvatar();
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            #endregion
            nowchooseid_eqp = 0;
            nowchooseid_body = 0;
            nowchooseid = 0;
            something();
            ShowHide_SelefFashion();
        }
        void Update()
        {
            if (m_proAvatar != null) m_proAvatar.FrameMove();
        }


        #region btnOnclick
        void OnSwitch(TabControl tabc)
        {
            int index = tabc.getSeletedIndex();
            switch (index)
            {
                //胸甲 dress_type="1"
                case 0:
                    tabOnclick(1);

                    break;
                //武器 dress_type="0"
                case 1:
                    tabOnclick(0);

                    break;
            };
        }
        void tabOnclick(int dresstype)
        {
            something();
            if (contain == null)
                return;       
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                int id = int.Parse(contain.transform.GetChild(i).gameObject.name);
                bool iddresstype= A3_FashionShowModel.getInstance().Dic_AllData[id].dress_type == dresstype ? true : false;
                int carr_id = -1;
                if(SelfRole._inst is P2Warrior)
                    carr_id = 2;
                else if(SelfRole._inst is P3Mage)
                    carr_id = 3;
                else if(SelfRole._inst is P5Assassin)
                    carr_id = 5;
                else
                    carr_id = -1;
                bool iscarr = A3_FashionShowModel.getInstance().Dic_AllData[id].carr == carr_id ? true : false;
                bool show = iddresstype && iscarr ? true : false;
                contain.transform.GetChild(i).gameObject.SetActive(show);
            }
            //默认选择第一个


        }

        void showthisOnclick(int id)
        {
            nowchooseid = id;
            panel.SetActive(true);
            txt.SetActive(false);
            Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;
            if(dic[id].dress_type==0)
            {
                nowchooseid_eqp = id;
            }else if(dic[id].dress_type == 1)
            {
                nowchooseid_body = id;
            }

            #region this_icon
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                contain.transform.GetChild(i).transform.FindChild("this").gameObject.SetActive(false);
            }
            dic_objs[id].transform.FindChild("this").gameObject.SetActive(true);
            #endregion
            #region info
            c_name.text = dic[id].name;
            c_shuxing.text = "";
            for (int i = 0; i < dic[id].lst_fa.Count; i++)
            {
                string nub_str = string.Empty;
                int nub = dic[id].lst_fa[i].type;
                if (nub >= 100)
                {
                    nub_str = dic[id].lst_fa[i].value + "%";
                    c_shuxing.text += Globle.getAttrNameById(dic[id].lst_fa[i].type - 100) + "+" + nub_str + "      ";
                }

                else
                {
                    nub_str = dic[id].lst_fa[i].value.ToString();
                    c_shuxing.text += Globle.getAttrNameById(dic[id].lst_fa[i].type) + "+" + nub_str + "      ";
                }
            }
            c_info.text = dic[id].des;
            if (A3_FashionShowModel.getInstance().dic_have_fs.ContainsKey(id))
            {
              //  long now_time = muNetCleint.instance.CurServerTimeStamp;
                //long over_time = long.Parse(ConvertStringToDateTime((long)A3_FashionShowModel.getInstance().dic_have_fs[id]));

                c_time.text =ContMgr.getCont("a3_fashionshow9") + Globle.getStrTime((int)A3_FashionShowModel.getInstance().dic_have_fs[id],true);
                c_jihuo_obj.SetActive(true);
                jihuo_info.SetActive(false);
                c_info.gameObject.SetActive(true);
                c_time.gameObject.SetActive(true);
                if (A3_FashionShowModel.getInstance().nowfs[0]== id|| A3_FashionShowModel.getInstance().nowfs[1] == id)
                {
                    btn_buy_obj.SetActive(false);
                    btn_dress_obj.SetActive(false);
                }
                    else
                {
                    btn_buy_obj.SetActive(false);
                    btn_dress_obj.SetActive(true);
                }                         
            }
            else
            {
                c_time.text = ContMgr.getCont("a3_fashionshow10");
                c_jihuo_obj.SetActive(false);
                btn_buy_obj.SetActive(true);
                btn_dress_obj.SetActive(false);
                jihuo_info.SetActive(true);
                inti_jihuo_panel();
                c_info.gameObject.SetActive(false);
                c_time.gameObject.SetActive(false);
            }

            #endregion
            #region  模型更改
            if (A3_FashionShowModel.getInstance().dress_show)
            {
                Show_this_Fashion();
            }
            #endregion


        }

        void returnOnclick()
        {
            nowchooseid_eqp = 0;
            nowchooseid_body = 0;
            btn_dress.gameObject.SetActive(false);
            btn_buy.gameObject.SetActive(false);
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                contain.transform.GetChild(i).transform.FindChild("this").gameObject.SetActive(false);
            }

            returnFashion();

        }


        bool enough_item = false;
        uint need_id = 0;
        void openbuyOnClick(GameObject go)
        {
            #region 没用的
            /* Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;
             if (nowchooseid == 0)
                 return;


             if(dic.ContainsKey(nowchooseid))
             {
                 string file = "icon_equip_" + (uint)dic[nowchooseid].lst_fu[0].need_id;
                 Image icon = getComponentByPath<Image>("buy_bg/icon");
                 icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
                 int needitem_num = a3_BagModel.getInstance().getItemNumByTpid ((uint)dic[nowchooseid].lst_fu[0].need_id);
                 getComponentByPath<Text>("buy_bg/icon/Text").text = "x" + needitem_num;
                 for (int i=0;i< dic[nowchooseid].lst_fu.Count-1;i++)
                 {
                     int j = i;
                     buy_objs[i].transform.FindChild("btn").GetComponent<Button>().interactable = needitem_num < dic[nowchooseid].lst_fu[i].need_num?false:true;                   
                     buy_objs[i].transform.FindChild("des").GetComponent<Text>().text = ContMgr.getCont("a3_fashionshow11") + dic[nowchooseid].lst_fu[i].need_num + ContMgr.getCont("a3_fashionshow12") +
                                                                                        a3_BagModel.getInstance().getItemDataById((uint)dic[nowchooseid].lst_fu[i].need_id).item_name +
                                                                                        ContMgr.getCont("a3_fashionshow13") + dic[nowchooseid].lst_fu[i].limit_day + ContMgr.getCont("a3_fashionshow14");
                     new BaseButton(buy_objs[i].transform.FindChild("btn")).onClick = (GameObject gos) =>
                    {

                        a3_fashionshowProxy.getInstance().SendProxys(3, null, nowchooseid, dic[nowchooseid].lst_fu[j].type);
                        buy_bg.SetActive(false);
                    };
                 }

             }*/
            #endregion
            if (jihuo_type != -1)
                jihuo_bg.SetActive(true);
            Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;

            int need_id_num = dic[nowchooseid].lst_fu[jihuo_type-1].need_num;
            need_id= (uint)dic[nowchooseid].lst_fu[jihuo_type - 1].need_id;
            int have_id_num = a3_BagModel.getInstance().getItemNumByTpid((uint)dic[nowchooseid].lst_fu[jihuo_type-1].need_id);
            string name = a3_BagModel.getInstance().getItemDataById((uint)dic[nowchooseid].lst_fu[jihuo_type-1].need_id).item_name;
            int day = dic[nowchooseid].lst_fu[jihuo_type-1].limit_day;

            getComponentByPath<Text>("jihuo_bg/Text").text = ContMgr.getCont("a3_fashionshow17") + need_id_num + ContMgr.getCont("a3_fashionshow12") + name + ContMgr.getCont("a3_fashionshow5") + day + ContMgr.getCont("a3_fashionshow18") + "\n"
                + ContMgr.getCont("a3_fashionshow19") + name+":"+ have_id_num;

            enough_item = have_id_num < need_id_num? false:true;



        }

       
        void dressinOnClick(GameObject go)
        {
            go.SetActive(false);
            //  0:武器  1:胸甲
            List<int> lst = new List<int>();
            Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;

            if (dic[nowchooseid].dress_type == 0)
                A3_FashionShowModel.getInstance().nowfs[0] = nowchooseid_eqp;
            else if (dic[nowchooseid].dress_type == 1)
                A3_FashionShowModel.getInstance().nowfs[1] = nowchooseid_body;
        

            //A3_FashionShowModel.getInstance().nowfs[0]= nowchooseid_eqp;
            //A3_FashionShowModel.getInstance().nowfs[1] = nowchooseid_body;
            for (int i=0; i < A3_FashionShowModel.getInstance().nowfs.Length;i++)
            {
                lst.Add(A3_FashionShowModel.getInstance().nowfs[i]);
            }
            
            a3_fashionshowProxy.getInstance().SendProxys(2, lst);
        }

        #endregion

        #region 场景模型加载
        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
                scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;

                foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta")
                        tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                    else
                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
            }
        }
        void createAvatar_body()
        {
            GameObject obj_prefab;
            A3_PROFESSION eprofession = A3_PROFESSION.None;
            if (SelfRole._inst is P2Warrior)
            {
                eprofession = A3_PROFESSION.Warrior;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");//-213.44f, 0.1f, 0.8f
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P3Mage)
            {
                eprofession = A3_PROFESSION.Mage;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");//-213.48f, 0.19f, 0.5f
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 167, 0, 0)) as GameObject;
            }
            else if (SelfRole._inst is P5Assassin)
            {
                eprofession = A3_PROFESSION.Assassin;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");//-213.46f, 0.12f, 1.2f
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
            }
            else
            {
                return;
            }
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
            }
            Transform cur_model = m_SelfObj.transform.FindChild("model");
            if (SelfRole._inst is P3Mage)
            {
                Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                light_fire.transform.SetParent(cur_r_finger1, false);
            }
            m_proAvatar = new ProfessionAvatar();
            m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
            seteqp_eff();
            seteqp_eff_new();
            m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
            m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
            m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
            m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
            m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());
        }
        void seteqp_eff()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.clear_oldeff();
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
            }
        }
        void seteqp_eff_new()
        {
            if (m_proAvatar != null)
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
        }
        public void disposeAvatar()
        {
            if (m_proAvatar != null)
            {
                m_proAvatar.dispose();
                m_proAvatar = null;
            }

            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        #endregion

        #region 时装模型
        /*场景中自己模型更改*/
        public static void ShowHide_SelefFashion()
        {
            if (a3_EquipModel.getInstance().getEquips() != null && a3_EquipModel.getInstance().getEquips().Count > 0)
            {
                foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                {
                    a3_EquipModel.getInstance().equipModel_on(equip);
                }
            }
            else
                a3_EquipModel.getInstance().equipModel_on(null);

        }
        /*界面中自己模型更改*/
        public void Show_this_Fashion()
        {
            int body_id = 0;
            int eqp_id = 0;
            /* SelfRole._inst*/
            Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;

            if(nowchooseid_body==0)
            {
                if(A3_FashionShowModel.getInstance().nowfs[1]==0)
                    body_id = 0;
                else
                    body_id = A3_FashionShowModel.getInstance().nowfs[1];
            }
            else
                body_id = nowchooseid_body;
            if(nowchooseid_eqp==0)
            {
                if (A3_FashionShowModel.getInstance().nowfs[0] == 0)
                    eqp_id = 0;
                else
                    eqp_id = A3_FashionShowModel.getInstance().nowfs[0];
            }
            else
                eqp_id = nowchooseid_eqp;

                          
            if(body_id!=0)
                m_proAvatar.set_body(body_id, 0);
            if(eqp_id!=0)
            {
                switch (PlayerModel.getInstance().profession)
                {
                    case 2:
                    case 3:
                        m_proAvatar.set_weaponr(eqp_id, 0);
                        break;
                    case 5:
                        m_proAvatar.set_weaponl(eqp_id, 0);
                        m_proAvatar.set_weaponr(eqp_id, 0);
                        break;
                }                         
            }
            

        }
        public void Hide_this_Fashion()
        {
            //ShowHide_SelefFashion();
            if (a3_EquipModel.getInstance().getEquips() != null && a3_EquipModel.getInstance().getEquips().Count > 0)
            {

                foreach (a3_BagItemData data in a3_EquipModel.getInstance().getEquips().Values)
                {
                    int bodyid = -1;
                    int bodyFxid = -1;
                    bool ishavebody = false;
                    //穿戴装备model变化
                    foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                    {
                        if (equip.confdata.equip_type == 3)
                        {
                            ishavebody = true;
                            break;
                        }
                    }
                    if(ishavebody)
                    {
                        if (data.confdata.equip_type == 3 && SelfRole._inst != null)
                        {
                            bodyid = (int)data.tpid;
                            bodyFxid = data.equipdata.stage;
                            m_proAvatar.set_body(bodyid, bodyFxid);
                        }
                    }
                    else
                    {
                        if (bodyid == -1)
                            m_proAvatar.set_body(0, 0);
                    }


                    int weaponid = -1;
                    int weaponFxid = -1;
                    bool ishaveequ = false;
                    foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                    {
                        if (equip.confdata.equip_type == 6)
                        {
                            ishaveequ = true;
                            break;
                        }
                    }
                    if(ishaveequ)
                    {
                        if (data.confdata.equip_type == 6)
                        {
                            weaponid = (int)data.tpid;
                            weaponFxid = data.equipdata.stage;

                            switch (PlayerModel.getInstance().profession)
                            {
                                case 2:
                                case 3:
                                    m_proAvatar.set_weaponr(weaponid, weaponFxid);
                                    break;
                                case 5:
                                    m_proAvatar.set_weaponl(weaponid, weaponFxid);
                                    m_proAvatar.set_weaponr(weaponid, weaponFxid);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (weaponid == -1)
                        {

                            switch (PlayerModel.getInstance().profession)
                            {
                                case 2:
                                case 3:
                                    m_proAvatar.set_weaponr(0, 0);
                                    break;
                                case 5:
                                    m_proAvatar.set_weaponl(0, 0);
                                    m_proAvatar.set_weaponr(0, 0);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                m_proAvatar.set_body(0, 0);
                switch (PlayerModel.getInstance().profession)
                {
                    case 2:
                    case 3:
                        m_proAvatar.set_weaponr(0, 0);
                        break;
                    case 5:
                        m_proAvatar.set_weaponl(0, 0);
                        m_proAvatar.set_weaponr(0, 0);
                        break;
                }
            }

        }

        /*返回刷新*/
        void returnFashion()
        {
            panel.SetActive(false);
            txt.SetActive(true);

            if (a3_EquipModel.getInstance().getEquips() != null && a3_EquipModel.getInstance().getEquips().Count > 0)
            {
                foreach (a3_BagItemData data in a3_EquipModel.getInstance().getEquips().Values)
                {
                    #region body
                    int bodyid = -1;
                    int bodyFxid = -1;
                    bool ishavebody = false;
                    foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                    {
                        if (equip.confdata.equip_type == 3)
                        {
                            ishavebody = true;
                            break;
                        }
                    }
                    if (ishavebody)
                    {
                        if(data.confdata.equip_type == 3)
                        {
                            bodyid = (int)data.tpid;
                            bodyFxid = data.equipdata.stage;
                            if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                                m_proAvatar.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                            else
                                m_proAvatar.set_body(bodyid, bodyFxid);
                        }

                    }
                    else
                    {
                        if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                            m_proAvatar.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                        else
                            m_proAvatar.set_body(0, 0);
                    }
                    #endregion
                    
                    #region wepon
                    int weaponid = -1;
                    int weaponFxid = -1;
                    bool ishaveequ = false;
                    foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                    {
                        if (equip.confdata.equip_type == 6)
                        {
                            ishaveequ = true;
                            break;
                        }
                    }
                    if (ishaveequ)
                    {
                        if(data.confdata.equip_type == 6)
                        {
                            weaponid = (int)data.tpid;
                            weaponFxid = data.equipdata.stage;
                            if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                            {
                                m_proAvatar.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                if (PlayerModel.getInstance().profession == 5)
                                    m_proAvatar.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            }
                            else
                            {
                                m_proAvatar.set_weaponr(weaponid, weaponFxid);
                                if (PlayerModel.getInstance().profession == 5)
                                    m_proAvatar.set_weaponl(weaponid, weaponFxid);
                            }
                        }                                  
                    }
                    else
                    {
                        if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                        {
                            m_proAvatar.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            if (PlayerModel.getInstance().profession == 5)
                                m_proAvatar.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                        }
                        else
                        {
                            m_proAvatar.set_weaponr(0, 0);
                            if (PlayerModel.getInstance().profession == 5)
                                m_proAvatar.set_weaponl(0, 0);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                    m_proAvatar.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                else
                    m_proAvatar.set_body(0, 0);
                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                {
                    m_proAvatar.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                    if (PlayerModel.getInstance().profession==5)
                          m_proAvatar.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                }
                else
                {
                    m_proAvatar.set_weaponr(0, 0);
                    if (PlayerModel.getInstance().profession==5)
                        m_proAvatar.set_weaponl(0, 0);                 
                }
            }


        }


        #endregion

        Dictionary<int, GameObject> dic_objs = new Dictionary<int, GameObject>();
        void creatrveObj()
        {
            Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;
            foreach (int id in dic.Keys)
            {

                GameObject cloneobj = GameObject.Instantiate(image) as GameObject;
                cloneobj.SetActive(true);
                cloneobj.transform.SetParent(contain.transform, false);
                cloneobj.name = id.ToString();

                string file = "icon_item_" + (uint)dic[id].icon_file;
                Image icon = cloneobj.transform.FindChild("icon").GetComponent<Image>();
                icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
                //GameObject gso = IconImageMgr.getInstance().createA3ItemIcon((uint)dic[id].icon_file, scale: 1f);
                dic_objs[id] = cloneobj;
                int ids = id;
                new BaseButton(cloneobj.transform).onClick = (GameObject go) => { showthisOnclick(ids); };
            }
            tabOnclick(1);

            //
            for (int i = 0; i < contain1.transform.childCount; i++)
            {
                buy_objs[i] = contain1.transform.GetChild(i).gameObject;
            }
            //
            for(int i=0;i<jihuo_info.transform.childCount; i++)
            {
                if (i == 0)
                    continue;
                lst_jihuo_infos.Add(jihuo_info.transform.GetChild(i).gameObject);
            }


        }
        /*锁,隐藏时装，隐藏飞翼按钮刷新*/
        void refreshObj()
        {
            Dictionary<int, uint> dic = A3_FashionShowModel.getInstance().dic_have_fs;
            foreach(int id in dic_objs.Keys)
            {
                if(dic.ContainsKey(id))
                    dic_objs[id].transform.FindChild("lock").gameObject.SetActive(false);
                else
                    dic_objs[id].transform.FindChild("lock").gameObject.SetActive(true);
            }


            bool show = A3_FashionShowModel.getInstance().dress_show;
            getComponentByPath<Toggle>("right_bg/down/shizhuang").isOn = !show;


            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET_SWING))
            {
                bool showwind = A3_WingModel.getInstance()?.ShowStage > 0 ? false : true;
                getComponentByPath<Toggle>("right_bg/down/feiyi").isOn = showwind;
            }

        }     
        /*身上穿的刷新*/
        public void RefreshSelfObj()
        {
            int id = A3_FashionShowModel.getInstance().nowfs[0];
            int id1 = A3_FashionShowModel.getInstance().nowfs[1];
            if (id!=0)
            {
                xiongjia_obj.transform.FindChild("icon").gameObject.SetActive(true);
                string file = "icon_item_" + (uint)id;
                          
                Image icon = xiongjia_obj.transform.FindChild("icon").GetComponent<Image>();
                icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            }
            else
            {
                xiongjia_obj.transform.FindChild("icon").gameObject.SetActive(false);
            }
            if (id1!=0)
            {
                wuqi_obj.transform.FindChild("icon").gameObject.SetActive(true);
                string file = "icon_item_" + (uint)id1;
                Image icon = wuqi_obj.transform.FindChild("icon").GetComponent<Image>();
                icon.sprite = GAMEAPI.ABUI_LoadSprite(file);
            }else
            {
                wuqi_obj.transform.FindChild("icon").gameObject.SetActive(false);
            }
            /* */
            if(A3_FashionShowModel.getInstance().dress_show)
                  Show_this_Fashion();
        }
        /*激活后界面刷新*/
        public  void RefreshObjs(int id)
        {
            refreshObj();
            showthisOnclick(id);
        }
        /*激活界面刷新*/

        int jihuo_type = -1;/*当前选择的激活type*/
        void inti_jihuo_panel()
        {
            for (int i = 0; i < lst_jihuo_infos.Count; i++)
            {
                lst_jihuo_infos[i].transform.FindChild("yes").gameObject.SetActive(i == 0 ? true : false);
            }
             Dictionary<int, fashionshow_date> dic = A3_FashionShowModel.getInstance().Dic_AllData;
            if (nowchooseid == 0)
                return;


            if (dic.ContainsKey(nowchooseid))
            {
                jihuo_type = dic[nowchooseid].lst_fu[0].type;
                int needitem_num = a3_BagModel.getInstance().getItemNumByTpid((uint)dic[nowchooseid].lst_fu[0].need_id);
                for (int i = 0; i < lst_jihuo_infos.Count; i++)
                {
                    string des = ContMgr.getCont("a3_fashionshow5") + dic[nowchooseid].lst_fu[i].limit_day + ContMgr.getCont("a3_fashionshow14");
                    lst_jihuo_infos[i].transform.FindChild("day").GetComponent<Text>().text = des;


                    Image icon = lst_jihuo_infos[i].transform.FindChild("Image").GetComponent<Image>();
                    debug.Log("IIIIIIIIIIIIII:" + a3_BagModel.getInstance().getItemDataById((uint)dic[nowchooseid].lst_fu[0].need_id).file);
                    icon.sprite = GAMEAPI.ABUI_LoadSprite(a3_BagModel.getInstance().getItemDataById((uint)dic[nowchooseid].lst_fu[0].need_id).file);


                    lst_jihuo_infos[i].transform.FindChild("num").GetComponent<Text>().text = "x" + dic[nowchooseid].lst_fu[i].need_num;

                    int num = i;
                    new BaseButton(lst_jihuo_infos[num].transform).onClick = (GameObject gos) =>
                      {
;
                          jihuo_type = dic[nowchooseid].lst_fu[num].type;
                          for(int j = 0; j < lst_jihuo_infos.Count; j++)
                          {
                              lst_jihuo_infos[j].transform.FindChild("yes").gameObject.SetActive(j == num ? true : false);
                          }
                      };

                }

            }
        }


   

        void something()
        {
            if (btn_buy_obj == null)
                return;
            btn_buy_obj.SetActive(false);
            btn_dress_obj.SetActive(false);
            panel.SetActive(false);
            txt.SetActive(true);
            for (int i = 0; i < contain.transform.childCount; i++)
            {
                contain.transform.GetChild(i).transform.FindChild("this").gameObject.SetActive(false);
            }
        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        public string ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string st = dtStart.AddSeconds(timeStamp).ToString("yyyy-MM-dd");
            return st;


        }
    }
}
