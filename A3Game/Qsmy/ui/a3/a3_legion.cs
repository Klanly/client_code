using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;

namespace MuGame
{
    class a3_legion : BaseShejiao
    {

        //模糊搜索
        InputField inputfield;
        GameObject findname_obj,
                   root_obj;
        GameObject iamge_fn,
                   contain_fn;
        Transform select_fn;
        public GameObject addpanels;

        public static a3_legion mInstance;
        public static a3_legion Instance;
        public a3_legion(Transform trans) : base(trans) { init(); }
        a3BaseLegion _currentAuction = null;
        Dictionary<string, a3BaseLegion> _legions = new Dictionary<string, a3BaseLegion>();
        TabControl _tabCtl;
        BaseButton create_legion;
        BaseButton join_legion;
        Transform[] s;
        Transform select;
        Transform toggle;
        Transform toggle0;
        A3_LegionData selectData = new A3_LegionData();
        Transform content;
        Text Avalable;
        Text _txtNameKeyWord;
        Text _txtBoardKeyWord;
        string enterName = default(string);
        public Text gxdtext;
        Toggle dic;
        bool can_use;
        bool can_jx;//捐献
        private SXML itemsXMl;
        public Toggle dic0;
        public bool movetoNPCdart = false;
        ScrollControler scrollControer0;
        ScrollControler scrollControer1;
        ScrollControler scrollControer2;

        public void Linit()
        {

            //== 配置(where [$name] -> tabs/$name == contents/$name)		
            _legions["info"] = new a3_legion_info(this, "contents/info");
            _legions["member"] = new a3_legion_member(this, "contents/member");
            _legions["active"] = new a3_legion_active(this, "contents/active");
            _legions["application"] = new a3_legion_application(this, "contents/application");
            _legions["diary"] = new a3_legion_diary(this, "contents/diary");
            //== 布局
            _tabCtl = InitLayout();

            //== 关闭窗口

        }

        TabControl InitLayout()
        {
            TabControl tab = new TabControl();
            List<Transform> contents = new List<Transform>();
            Transform contentsRoot = getGameObjectByPath("s4/contents").transform;
            foreach (var v in contentsRoot.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == contentsRoot)
                {
                    v.gameObject.SetActive(false);
                    contents.Add(v);
                }
            }
            tab.onClickHanle += (TabControl tb) =>
            {
                if (!_legions.ContainsKey(contents[tb.getSeletedIndex()].name))
                {
                    debug.Log("没有界面配置");
                    if (_legions.Count > 0)
                        tb.setSelectedIndex(tb.getIndexByName(new List<a3BaseLegion>(_legions.Values)[0].pathStrName));
                    return;
                }
                for (int i = 0; i < contents.Count; i++)
                {
                    if (i != tb.getSeletedIndex()) contents[i].gameObject.SetActive(false);
                    else contents[i].gameObject.SetActive(true);
                }
                if (_currentAuction != null)
                {
                    _currentAuction.onClose();
                }
                if (_currentAuction != null && _legions.ContainsKey(contents[tb.getSeletedIndex()].name))
                {
                    _currentAuction = _legions[contents[tb.getSeletedIndex()].name];
                    _currentAuction.onShowed();
                }
                else _currentAuction = _legions[contents[tb.getSeletedIndex()].name];
            };
            tab.create(transform.FindChild("s4/tabs").gameObject, transform.FindChild("s4").gameObject);
            return tab;
        }

        void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("s2/root/cells/scroll"));
            scrollControer1 = new ScrollControler();
            scrollControer1.create(getComponentByPath<ScrollRect>("s2/addPanelResult/cells/scroll"));
            scrollControer2 = new ScrollControler();
            scrollControer2.create(getComponentByPath<ScrollRect>("s4/contents/member/cells/scroll"));
            inText();
            inputfield = getComponentByPath<InputField>("s2/addPanel/main/InputField");
            findname_obj = getGameObjectByPath("s2/addPanelResult");
            root_obj = getGameObjectByPath("s2/root");
            iamge_fn = getGameObjectByPath("s2/addPanelResult/cells/scroll/0");
            contain_fn = getGameObjectByPath("s2/addPanelResult/cells/scroll/content");
            select_fn = getTransformByPath("s2/addPanelResult/cells/scroll/select");
            addpanels = getGameObjectByPath("s2/addPanel");
            mInstance = this;
            Linit();
            gxdtext = getTransformByPath("s5/gxd/text").GetComponent<Text>();
            Avalable = getTransformByPath("s3/Avalable").GetComponent<Text>();
            _txtNameKeyWord = getComponentByPath<Text>("s3/txtKeyWord");
            _txtNameKeyWord.gameObject.SetActive(false);
            _txtBoardKeyWord = getComponentByPath<Text>("s4/contents/info/rt/txtKeyWord");
            _txtBoardKeyWord.gameObject.SetActive(false);
            content = getTransformByPath("s2/root/cells/scroll/content");
            select = getTransformByPath("s2/root/cells/scroll/select");
            toggle = getTransformByPath("s2/root/Toggle");

            dic = toggle.GetComponent<Toggle>();
            dic.onValueChanged.AddListener((bool b) =>
            {
                A3_LegionProxy.getInstance().SendGetList();
                RefreshAllList(null);
            });
            toggle0 = getTransformByPath("s1/Toggle");
            dic0 = toggle0.GetComponent<Toggle>();
            dic0.onValueChanged.AddListener((bool b) =>
            {
                dic0.isOn = b;
                A3_LegionProxy.getInstance().SendChangeToggleMode(b);
            });

            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_REPAIR, Repair);
            s = new Transform[] { getTransformByPath("s1"), getTransformByPath("s2"), getTransformByPath("s3"), getTransformByPath("s4"), getTransformByPath("s5"), getTransformByPath("s6") };
            join_legion = new BaseButton(getTransformByPath("s1/bt1"));
            join_legion.onClick = (GameObject g) =>
            {
                bool b = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl >= A3_LegionModel.getInstance().create_needzhuan * 100 + A3_LegionModel.getInstance().create_needlv;
                if (b)
                {
                    findname_obj.SetActive(false);
                    if (root_obj.activeSelf == false)
                        root_obj.SetActive(true);
                    ShowS(1);
                    A3_LegionProxy.getInstance().SendGetList();
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_legion_txt"));
                }
            };
            create_legion = new BaseButton(getTransformByPath("s1/bt2"));
            create_legion.onClick = (GameObject g) =>
            {
                bool b = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl >= A3_LegionModel.getInstance().create_needzhuan * 100 + A3_LegionModel.getInstance().create_needlv;
                bool c = PlayerModel.getInstance().money >= A3_LegionModel.getInstance().create_needmoney;
                if (b && c)
                {
                    OpenCreatePanel();
                }
                else flytxt.instance.fly(ContMgr.getCont("a3_legion_txt1"));
            };
            new BaseButton(getTransformByPath("s2/root/bt1")).onClick = (GameObject g) =>
            {
                transform.FindChild("s2").gameObject.SetActive(false);
                transform.FindChild("s1").gameObject.SetActive(true);
               

            };
            new BaseButton(getTransformByPath("s2/root/bt2")).onClick = (GameObject g) =>
            {

                if (selectData.id > 0)
                {

                    A3_LegionProxy.getInstance().SendApplyFor((uint)selectData.id);
                }
            };
            new BaseButton(getTransformByPath("s2/root/bt3")).onClick = (GameObject g) =>
            {

                A3_LegionProxy.getInstance().SendGetList();
                RefreshAllList(null);
                int a = UnityEngine.Random.Range(0, A3_LegionModel.getInstance().list.Count);
                if (A3_LegionModel.getInstance().list[a].id > 0)
                {
                    A3_LegionProxy.getInstance().SendApplyFor((uint)A3_LegionModel.getInstance().list[a].id);
                }
            };
            new BaseButton(getTransformByPath("s2/root/bt4")).onClick = (GameObject g) =>
              {
                  getGameObjectByPath("s2/addPanel").SetActive(true);
                  inputfield.text = "";
              };
            new BaseButton(getTransformByPath("s2/addPanel/bottom/btnCancel")).onClick = (GameObject g) =>
              {
                  getGameObjectByPath("s2/addPanel").SetActive(false);
              };
            new BaseButton(getTransformByPath("s2/addPanel/bottom/btnAdd")).onClick = (GameObject g) =>
            {
                //确认搜索
                string names = inputfield.text;
                if (names == "")
                {
                    flytxt.instance.fly(ContMgr.getCont("finname"));
                }
                else
                {

                    A3_LegionProxy.getInstance().sendfindname(inputfield.text);
                }

            };
            new BaseButton(getTransformByPath("s2/addPanelResult/bt1")).onClick = (GameObject g) =>
            {
                findname_obj.SetActive(false);
                root_obj.SetActive(true);
                selecr_fnTsm();
            };
            new BaseButton(getTransformByPath("s2/addPanelResult/bt2")).onClick = (GameObject g) =>
            {
                selecr_fnTsm();
                if (findid > 0)
                    A3_LegionProxy.getInstance().SendApplyFor((uint)findid);
            };
            new BaseButton(getTransformByPath("s4/contents/info/btn_shop")).onClick = (GameObject go) =>
            {
                if (FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
                {
                    ArrayList arr = new ArrayList();
                    Shop_a3Model.getInstance().selectnum = 4;
                    arr.Add(4);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.SHOP_A3, arr);
                    shop_a3._instance?.transform.SetAsLastSibling();
                    shop_a3._instance?.tab.setSelectedIndex(4);
                }
                else
                    flytxt.instance.fly(ContMgr.getCont("func_limit_38"));
            };
            new BaseButton(getTransformByPath("s3/btn_close")).onClick = (GameObject g) =>
            {
                CloseCreatePanel();
            };
            new BaseButton(getTransformByPath("s7/bg")).onClick = (GameObject g) =>
            {
                transform.FindChild("s7").gameObject.SetActive(false);
            };
            getTransformByPath("s3/InputName").GetComponent<InputField>().onEndEdit.AddListener((string ss) =>
            {
                debug.Log("Enter====Enter:   " + ss);

                if (checkKeyWord(ss, 3)) return;

                A3_LegionProxy.getInstance().SendCheckName(ss);
                enterName = ss;
                can_use = true;
            });

            new BaseButton(getTransformByPath("s3/btn_create")).onClick = (GameObject g) =>
            {
                bool b = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl >= A3_LegionModel.getInstance().create_needzhuan * 100 + A3_LegionModel.getInstance().create_needlv;
                bool c = PlayerModel.getInstance().money >= A3_LegionModel.getInstance().create_needmoney;

                if (enterName != string.Empty && can_use == true)
                {
                    if (b && c)
                    {
                        A3_LegionProxy.getInstance().SendCreateLegion(enterName);
                    }
                    else
                    {
                        flytxt.instance.fly(ContMgr.getCont("a3_legion_txt2"));
                    }
                }
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_legion_txt3"));
                }
            };
            //A3_LegionProxy.getInstance().SendGetMember();

            new BaseButton(getTransformByPath("s4/contents/info/btn_build")).onClick = (go) => { InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LEGION_BUILD); };

            new BaseButton(getTransformByPath("s4/contents/active/rect_mask/rect_scroll/slayDragon")).onClick = (go) => { InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SLAY_DRAGON); };

            new BaseButton(getTransformByPath("s4/contents/info/lt/info/clan_name/Edit")).onClick = (go) => {
                if (A3_LegionModel.getInstance().change_needUser != A3_LegionModel.getInstance().myLegion.clanc)
                {
                    flytxt.instance.fly(ContMgr.getCont("changeName_LengionErrUser"));
                    return;
                }
                  
                ArrayList v = new ArrayList();
                v.Add(2); // type    1 表示 人物修改   2   表示军团修改
                v.Add(A3_LegionModel.getInstance().change_needId);
                v.Add(A3_LegionModel.getInstance().change_needNumber);

                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHANGE_NAME , v);

                if (a3_changeName.changeLegionName == null)
                {
                    A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CHANGE_NAME, ChangeNameSuccess);
                    A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CHANGE_NAME, ChangeNameSuccess);

                    a3_changeName.changeLegionName = ( str ) =>
                    {
                        if (A3_LegionModel.getInstance().change_needUser != A3_LegionModel.getInstance().myLegion.clanc)
                        {
                            flytxt.instance.fly(ContMgr.getCont("changeName_LengionErrUser"));

                            return;
                        }

                        int havaNum =  a3_BagModel.getInstance().getItemNumByTpid((uint)A3_LegionModel.getInstance().change_needId);

                        if (havaNum < A3_LegionModel.getInstance().change_needNumber)
                        {
                            flytxt.instance.fly(ContMgr.getCont("a3_summon_shouhun_noitem"));

                            return;
                        }

                        if (str == null  || str.Equals(string.Empty))
                        {
                            return;
                        }

                        A3_LegionProxy.getInstance().ChangeLegionName(str);

                    };
                }
            };
        }

        void inText()
        {
            this.transform.FindChild("s1/1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_1");//建团条件
            this.transform.FindChild("s1/2/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_2");//角色到达1转1级
            this.transform.FindChild("s1/2/Text2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_3");//需要消耗100万金币
            this.transform.FindChild("s1/2/Text3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_4");//独一无二的军团名称
            this.transform.FindChild("s1/3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_5");//军团福
            this.transform.FindChild("s1/4/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_6");//学习军团技能，提升角色属性
            this.transform.FindChild("s1/4/Text2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_7");//完成军团任务，获得丰厚奖励
            this.transform.FindChild("s1/4/Text3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_8");//参与军团运镖，开启激情厮杀
            this.transform.FindChild("s1/4/Text4").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_9");//挑战远古巨龙，赢取极品装备
            this.transform.FindChild("s1/bt1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_10");//加团
            this.transform.FindChild("s1/bt2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_11");//建团
            this.transform.FindChild("s1/Toggle/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_12");//当有军团邀请加入时,自动加入。


            //this.transform.FindChild("s2/root/bt1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_13");//返回
            this.transform.FindChild("s2/root/bt2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_14");//申请加入
            this.transform.FindChild("s2/root/bt3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_15");//快速加入
            this.transform.FindChild("s2/root/bt4/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_16");//搜索军团
            this.transform.FindChild("s2/root/Image1/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_17");//军团名称
            this.transform.FindChild("s2/root/Image1/Text2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_18");//首领
            this.transform.FindChild("s2/root/Image1/Text3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_19");//人数
            this.transform.FindChild("s2/root/Image1/Text4").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_20");//战斗力
            this.transform.FindChild("s2/root/Image1/Text6").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_21");//活跃度
            this.transform.FindChild("s2/addPanel/bg1/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_23");//输入军团名称或关键字:
            this.transform.FindChild("s2/addPanel/bottom/btnAdd/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_24");//查找
            this.transform.FindChild("s2/addPanel/bottom/btnCancel/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_25");//取消

            this.transform.FindChild("s3/Image1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_26");//建立军团
            this.transform.FindChild("s3/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_27");//请输入军团的名称：
            this.transform.FindChild("s3/Avalable").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_28");//(可用）
            this.transform.FindChild("s3/btn_create/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_26");//建立军团
            this.transform.FindChild("s3/txtKeyWord").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_29");//含有屏蔽字符,请重新输入

            this.transform.FindChild("s4/tabs/info/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_30");//信息
            this.transform.FindChild("s4/tabs/member/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_31");//成员
            this.transform.FindChild("s4/tabs/active/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_32");//活动
            this.transform.FindChild("s4/tabs/application/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_33");//申请表
            this.transform.FindChild("s4/tabs/diary/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_34");//日志
            this.transform.FindChild("s4/contents/info/rt/msg/Placeholder").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_35");//点击此处输入内容....
            this.transform.FindChild("s4/contents/info/rt/Image1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_36");//公告
            this.transform.FindChild("s4/contents/info/rt/buff/Image/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_37");//荣耀与共
            this.transform.FindChild("s4/contents/info/rt/txtKeyWord").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_38");//含有屏蔽字符,请重新输入
            this.transform.FindChild("s4/contents/info/lt/btn_lvup/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_39");//升级
            this.transform.FindChild("s4/contents/info/lt/info/clan_name/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_40");//名字：
            this.transform.FindChild("s4/contents/info/lt/info/legion_name/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_41");//军团信息
            this.transform.FindChild("s4/contents/info/lt/info/name/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_42");//首领：
            this.transform.FindChild("s4/contents/info/lt/info/lvl/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_43");//等级：
            this.transform.FindChild("s4/contents/info/lt/info/exp/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_44");//经验：
            this.transform.FindChild("s4/contents/info/lt/info/member_num/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_45");//人数：
            this.transform.FindChild("s4/contents/info/lt/info/funds/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_46");//资金：
            this.transform.FindChild("s4/contents/info/lt/info/zdl/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_47");//战斗力：
            this.transform.FindChild("s4/contents/info/btn_donation/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_48");//捐献
            this.transform.FindChild("s4/contents/info/btn_quit/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_49");//退团
            this.transform.FindChild("s4/contents/info/help/panel_help/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_50");//说  明
            this.transform.FindChild("s4/contents/info/help/panel_help/descTxt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_51");//军团等级影响技能学习等级上限{n}离开军团后，依然可以获得技能属性加成
            this.transform.FindChild("s4/contents/info/help2/panel_help/title").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_52");//说  明
            this.transform.FindChild("s4/contents/info/help2/panel_help/cells/scroll/content/descTxt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_53");// 军团管理{n}  领袖、元老及精英成员有处理入会申请的权限{n}  领袖及元老有对其他团员委任职位的权利（不可委任高于或与{n}  自身平级的职位）{n}  领袖及元老可以对军团公告进行修改{n}  领袖及元老可以对军团进行升级{n}  军团等级影响军团成员人数上限，以及军团技能等级上限{n}军团任务{n}  一周可完成70个军团任务，完成军团任务可以获得一定数量的{n}  军团资金、个人贡献以及活跃度奖励
            this.transform.FindChild("s4/contents/member/0/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_54");//名称
            this.transform.FindChild("s4/contents/member/0/jj").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_55");//阶级
            this.transform.FindChild("s4/contents/member/0/zt").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_56");//状态
            this.transform.FindChild("s4/contents/member/0/dj").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_57");//等级
            this.transform.FindChild("s4/contents/member/0/zdl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_58");//战斗力
            this.transform.FindChild("s4/contents/member/0/gxd").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_59");//贡献度
            this.transform.FindChild("s4/contents/member/0/hyd").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_60");//活跃度
            this.transform.FindChild("s4/contents/member/btn_leader/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_61");//晋升为领袖
            this.transform.FindChild("s4/contents/member/btn_promotion/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_62");//升职
            this.transform.FindChild("s4/contents/member/btn_demotion/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_63");//降职
            this.transform.FindChild("s4/contents/member/btn_remove/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_64");//请离
            this.transform.FindChild("s4/contents/application/0/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_65");//名称
            this.transform.FindChild("s4/contents/application/0/zy").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_66");//职业
            this.transform.FindChild("s4/contents/application/0/dj").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_67");//等级
            this.transform.FindChild("s4/contents/application/0/zdl").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_68");//战斗力
            this.transform.FindChild("s4/contents/application/0/gxd").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_69");//操作
            this.transform.FindChild("s4/contents/application/btn_promotion/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_70");//允许全部
            this.transform.FindChild("s4/contents/application/btn_demotion/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_71");//拒绝全部

            this.transform.FindChild("s4/contents/application/Toggle/Label").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_72");//允许所有人直接加入军团


            this.transform.FindChild("s5/Image1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_73");//捐献金币
            this.transform.FindChild("s5/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_74");//为军团捐献：
            this.transform.FindChild("s5/btn_create/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_73");//捐献金币
            this.transform.FindChild("s5/gxd/text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_75");//可获得贡献：

            this.transform.FindChild("s6/Image1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_76");//邀请加入军团
            this.transform.FindChild("s6/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_77");//填写角色名字
            this.transform.FindChild("s6/btn_invite/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_78");//邀请
            this.transform.FindChild("s6/btn_cancel/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_79");//取消
            this.transform.FindChild("s6/sa/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_80");//没找到对应角色

            this.transform.FindChild("s7/raise_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_81");//提升
            this.transform.FindChild("s7/Image/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_legion_82");//“英勇印章”可通过参与军团运镖活动获得
        }

        public bool checkKeyWord(string s, int sNum)
        {
            bool b = KeyWord.isContain(s);
            switch (sNum)
            {
                case 3:
                    _txtNameKeyWord.gameObject.SetActive(b);
                    break;
                case 4:
                    _txtBoardKeyWord.gameObject.SetActive(b);
                    break;
                default:
                    break;
            }

            return b;
        }



        //public void application_false()
        //{
        //    Debug.LogError(A3_LegionModel.getInstance().members.Count);      
        //    if (A3_LegionModel.getInstance().members.Count!=0)
        //    {
        //        foreach (int key in A3_LegionModel.getInstance().members.Keys)
        //        {
        //            Debug.LogError(key);
        //            if (PlayerModel.getInstance().cid == A3_LegionModel.getInstance().members[key].cid)
        //            {
        //                Debug.LogError(PlayerModel.getInstance().cid + "ss" + A3_LegionModel.getInstance().members[key].cid);
        //                if (A3_LegionModel.getInstance().members[key].clanc > 1)
        //                {
        //                    Debug.LogError(A3_LegionModel.getInstance().members[key].clanc);
        //                    transform.FindChild("s4/tabs/application").gameObject.SetActive(true);
        //                }
        //                else
        //                {
        //                    transform.FindChild("s4/tabs/application").gameObject.SetActive(false);

        //                }
        //            }
        //        }
        //    }
        //}
        public static int lastPage = -1;
        void RefreshLegionPage(GameEvent e)
        {
            if (A3_LegionModel.getInstance().myLegion.id <= 0)
            {
                ShowS(0);
            }
            //
            _legions[ "application" ].transform.FindChild( "Toggle" ).GetComponent<Toggle>().isOn = A3_LegionModel.getInstance().CanAutoApply;
        }
        public override void onShowed()
        {
            Instance = this;
            base.onShowed();
            enterName = string.Empty;
            getTransformByPath("s3/InputName").GetComponent<InputField>().text = string.Empty;
            select.gameObject.SetActive(false);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_LOADLIST, RefreshAllList);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CREATE, OnCreate);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_APPLYSUCCESSFUL, OnApplySuccessful);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshLegionPage);


            if (lastPage > 0)
            {
                A3_LegionProxy.getInstance().SendGetInfo();
                if (lastPage == 1/*member*/)
                {
                    lastPage = 0;
                    if (A3_LegionModel.getInstance().myLegion != null)
                    {
                        ShowS(3);
                        _tabCtl.setSelectedIndex(1);
                    }
                    else
                        goto NOT_IN_LEGION;
                }
                return;
            }
        NOT_IN_LEGION:
            if (A3_LegionModel.getInstance().showtype >= 0)
            {
                _tabCtl.setSelectedIndex(A3_LegionModel.getInstance().showtype);
                A3_LegionModel.getInstance().showtype = -1;
            }
            else
            {
                var test = A3_LegionModel.getInstance();
                if (A3_LegionModel.getInstance().myLegion.id <= 0)
                {
                    ShowS(0);
                }
                else
                {

                    ShowS(3);
                    if (_tabCtl.getSeletedIndex() > 0) _tabCtl.setSelectedIndex(0);
                    else if (_currentAuction != null) _currentAuction.onShowed();
                }

                if (A3_LegionProxy.getInstance().join_legion)
                {

                    ShowS(3);
                    if (_tabCtl.getSeletedIndex() > 0) _tabCtl.setSelectedIndex(0);
                    else if (_currentAuction != null) _currentAuction.onShowed();
                    A3_LegionProxy.getInstance().join_legion = false;
                }
            }

        }

        private void Repair(GameEvent e)
        {
            var data = e.data;
            if (data.ContainsKey("clname") && !data.ContainsKey("clan_lvl"))
            {

                flytxt.instance.fly(ContMgr.getCont("clan_log_15")/*"很遗憾，您所在的军团因长期缺乏军团资金被解散了。"*/);
            }
            if (data.ContainsKey("clname") && data.ContainsKey("clan_lvl"))
            {
                uint lvl = data["clan_lvl"];

                flytxt.instance.fly(ContMgr.getCont("clan_log_14", new List<string> { lvl.ToString() }));
                // flytxt.instance.fly("很遗憾，您所在的军团因长期缺乏军团资金，导致军团等级降低至" + "<color=#00FF00>" + lvl + "</color>" + "级。");
            }
        }

        public override void onClose()
        {
            base.onClose();
            selectData = new A3_LegionData();
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_LOADLIST, RefreshAllList);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CREATE, OnCreate);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_APPLYSUCCESSFUL, OnApplySuccessful);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshLegionPage);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CHANGE_NAME, ChangeNameSuccess);
            if (_currentAuction != null) _currentAuction.onClose();
            if (lastPage == 0) lastPage = -1;
        }

        void RefreshAllList(GameEvent e)
        {

            select.gameObject.SetActive(false);
            selectData = new A3_LegionData();
            select.SetParent(content.parent);
            Transform pre = getTransformByPath("s2/root/cells/scroll/0");
            foreach (var v in content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            int icc = 0;



            if (dic.isOn == true)
            {
                //A3_LegionModel.getInstance().list = A3_LegionModel.getInstance().list2;
                for (int i = 0; i < A3_LegionModel.getInstance().list.Count; i++)
                {
                    if (A3_LegionModel.getInstance().list[i].direct_join == 0)
                    {
                        A3_LegionModel.getInstance().list.Remove(A3_LegionModel.getInstance().list[i]);
                    }
                }
            }
            if (dic.isOn == false)
            {
                A3_LegionModel.getInstance().list.Clear();
                foreach (var v in A3_LegionModel.getInstance().list2)
                {
                    A3_LegionModel.getInstance().list.Add(v);
                }


            }



            //for (int i = 0; i < A3_LegionModel.getInstance().list.Count; i++)
            //{
            //    for (int j = 0; j < A3_LegionModel.getInstance().list.Count; j++)
            //    {
            //        if (A3_LegionModel.getInstance().list[i].id > A3_LegionModel.getInstance().list[j].id)
            //        {
            //            A3_LegionData temp = A3_LegionModel.getInstance().list[i];
            //            A3_LegionModel.getInstance().list[i] = A3_LegionModel.getInstance().list[j];
            //            A3_LegionModel.getInstance().list[j] = temp;

            //        }
            //    }
            //}

            A3_LegionModel.getInstance().list.Sort(new myComparer());
            //for (int i = 0; i < A3_LegionModel.getInstance().list.Count; i++)
            //{
            //    debug.Log(A3_LegionModel.getInstance().list[i].name + A3_LegionModel.getInstance().list[i].direct_join);
            //}

            foreach (var v in A3_LegionModel.getInstance().list)
            {
                var go = GameObject.Instantiate(pre.gameObject) as GameObject;
                go.transform.SetParent(content);
                go.transform.FindChild("name").GetComponent<Text>().text = v.clname;
                go.transform.FindChild("sl").GetComponent<Text>().text = v.name;
                go.transform.FindChild("zdl").GetComponent<Text>().text = v.combpt.ToString();
                go.transform.FindChild("dj").GetComponent<Text>().text = v.lvl.ToString();
                go.transform.FindChild("hy").GetComponent<Text>().text = v.huoyue.ToString();
                int maxnum = XMLMgr.instance.GetSXML("clan.clan", "clan_lvl==" + v.lvl).getInt("member");
                go.transform.FindChild("rs").GetComponent<Text>().text = v.plycnt + "/" + maxnum;

                if (icc % 2 == 0)
                {
                    go.transform.FindChild("bg1").gameObject.SetActive(true);
                    go.transform.FindChild("bg2").gameObject.SetActive(false);
                }
                else
                {
                    go.transform.FindChild("bg1").gameObject.SetActive(false);
                    go.transform.FindChild("bg2").gameObject.SetActive(true);
                }
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                var temp = new List<A3_LegionData>();
                temp.Add(v);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    select.SetParent(go.transform);
                    select.localPosition = Vector3.zero;
                    select.gameObject.SetActive(true);
                    selectData = temp[0];
                };
                icc++;

            }
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (content.GetComponent<GridLayoutGroup>().cellSize.y) * A3_LegionModel.getInstance().list.Count);

        }

        void OnCreate(GameEvent e)
        {
            ShowS(3);
            if (_tabCtl.getSeletedIndex() > 0) _tabCtl.setSelectedIndex(0);
            else if (_currentAuction != null) _currentAuction.onShowed();
            A3_LegionProxy.getInstance().SendGetMember();
        }

        void OpenCreatePanel()
        {
            s[2].gameObject.SetActive(true);
            can_use = false;
            // if (!A3_LegionProxy.getInstance().hasEventListener(A3_LegionProxy.EVENT_CHECKNAME))
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CHECKNAME, SetCheckName);
            Avalable.text = ContMgr.getCont("a3_legion_txt4");
        }

        void CloseCreatePanel()
        {
            s[2].gameObject.SetActive(false);
            //if (A3_LegionProxy.getInstance().hasEventListener(A3_LegionProxy.EVENT_CHECKNAME))
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CHECKNAME, SetCheckName);
        }

        public void SetCheckName(GameEvent e)
        {
            int res = e.data["err"];
            if (res == -107 || res == -103)
            {
                can_use = false;
                Avalable.text = ContMgr.getCont("a3_legion_txt5");
            }
            else if (res == -1420)
            {
                can_use = false;
                Avalable.text = ContMgr.getCont("a3_legion_txt6");
            }
            else if (res > 0)
            {
                can_use = true;
                Avalable.text = ContMgr.getCont("a3_legion_txt7");
            }
        }

        public void ChangeNameSuccess( GameEvent e) {

            InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHANGE_NAME);

            this.transform.FindChild("s4/contents/info/lt/info/clan_name/Text/Text").GetComponent<Text>().text = A3_LegionModel.getInstance().myLegion.clname;

            flytxt.instance.fly(ContMgr.getCont("changeName_LengionSucess"));

        }

        void Refresh()
        {

        }

        void OnApplySuccessful(GameEvent e)
        {
            Variant data = e.data;
            bool approved = data["approved"];
            if (approved)
            {
                ShowS(3);
                if (_tabCtl.getSeletedIndex() > 0) _tabCtl.setSelectedIndex(0);
                else if (_currentAuction != null) _currentAuction.onShowed();
            }
        }

        public void ShowS(int index)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i].gameObject.SetActive(false);
            }
            if (index >= 0 && index < s.Length)
            {
                s[index].gameObject.SetActive(true);
            }
            if (A3_LegionProxy.getInstance().hasEventListener(A3_LegionProxy.EVENT_CHECKNAME))
                A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CHECKNAME, SetCheckName);
        }

        public void OpenSub(Transform t)
        {
            t.SetParent(transform.parent);
            t.SetAsLastSibling();
            t.gameObject.SetActive(true);
        }

        public void CloseSub(Transform t, Transform p)
        {
            t.SetParent(p);
            t.SetAsLastSibling();
            t.gameObject.SetActive(false);
        }


        //模糊查找军团名字
        int findid = 0;
        public void Findnames()
        {
            if (contain_fn.transform.childCount > 0)
            {
                for (int i = 0; i < contain_fn.transform.childCount; i++)
                {
                    GameObject.Destroy(contain_fn.transform.GetChild(i).gameObject);

                }
            }

            root_obj.SetActive(false);
            findname_obj.SetActive(true);
            int iccs = 0;
            foreach (var v in A3_LegionModel.getInstance().finfname)
            {
                var go = GameObject.Instantiate(iamge_fn) as GameObject;
                go.transform.SetParent(contain_fn.transform, false);
                go.name = v.id.ToString();
                go.transform.FindChild("name").GetComponent<Text>().text = v.clname;
                go.transform.FindChild("sl").GetComponent<Text>().text = v.name;
                go.transform.FindChild("zdl").GetComponent<Text>().text = v.combpt.ToString();
                go.transform.FindChild("dj").GetComponent<Text>().text = v.lvl.ToString();
                go.transform.FindChild("hy").GetComponent<Text>().text = v.huoyue.ToString();
                int maxnum = XMLMgr.instance.GetSXML("clan.clan", "clan_lvl==" + v.lvl).getInt("member");
                go.transform.FindChild("rs").GetComponent<Text>().text = v.plycnt + "/" + maxnum;

                if (iccs % 2 == 0)
                {
                    go.transform.FindChild("bg1").gameObject.SetActive(true);
                    go.transform.FindChild("bg2").gameObject.SetActive(false);
                }
                else
                {
                    go.transform.FindChild("bg1").gameObject.SetActive(false);
                    go.transform.FindChild("bg2").gameObject.SetActive(true);
                }
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                new BaseButton(go.transform).onClick = (GameObject g) =>
                {
                    findid = int.Parse(g.name);
                    select_fn.SetParent(g.transform);
                    select_fn.localPosition = Vector3.zero;
                    select_fn.gameObject.SetActive(true);
                };
                iccs++;

            }
            contain_fn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (contain_fn.GetComponent<GridLayoutGroup>().cellSize.y) * A3_LegionModel.getInstance().finfname.Count);
        }

        void selecr_fnTsm()
        {
            select_fn.SetParent(getTransformByPath("s2/addPanelResult/cells/scroll"),false);
            select_fn.gameObject.SetActive(false);
        }
    }

    class a3BaseLegion : Skin
    {
        public a3_legion main { get; set; }
        public string pathStrName { get; set; }
        public a3BaseLegion(BaseShejiao win, string pathStr)
            : base(win.getTransformByPath("s4/" + pathStr))
        {
            var ss = pathStr.Split('/');
            this.pathStrName = ss[Mathf.Max(0, ss.Length - 1)];
            main = win as a3_legion;
            init();
        }
        public virtual void init() { }
        public virtual void onShowed() { }
        public virtual void onClose()
        {
            
        }
    }

    class a3_legion_info : a3BaseLegion
    {
        public a3_legion_info(BaseShejiao win, string pathStr)
            : base(win, pathStr)
        {
        }
        public static a3_legion_info mInstance;
        InputField edittext;
        BaseButton edit;
       
        Transform s5;
        Transform s7;
        BaseButton btn_quit;
        BaseButton btn_lvup;
        //BaseButton jn_btn;//icon技能提升页面跳转
        //BaseButton raise_lvl;//团队技能提升
        Transform helpParent;
        Transform help;
        Transform help2;
        Transform jn_add;//技能
        bool can_jx;
        public override void init()
        {
            mInstance = this;
            jn_add = transform.FindChild("rt/buff/icon/Image_add");
            helpParent = transform.FindChild("help");
            help = transform.FindChild("help/panel_help");
            help2 = transform.FindChild("help2");
            edittext = transform.FindChild("rt/msg").GetComponent<InputField>();
            edittext.enabled = false;
            btn_quit = new BaseButton(transform.FindChild("btn_quit"));
            btn_quit.onClick = (GameObject) =>
            {
                if (PlayerModel.getInstance().name == A3_LegionModel.getInstance().myLegion.name)
                {
                    if (A3_LegionModel.getInstance().myLegion.plycnt <= 1)
                        MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("clan_0"), () => A3_LegionProxy.getInstance().SendDeleteLegion());
                    else flytxt.instance.fly(ContMgr.getCont("clan_1"));
                }
                else
                {
                    MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("clan_2"), () => A3_LegionProxy.getInstance().SendQuit());
                }
            };
          
            edit = new BaseButton(transform.FindChild("rt/Edit"));
            edit.onClick = (GameObject) =>
            {

                if (!edittext.interactable)
                {
                   
                    if (A3_LegionModel.getInstance().myLegion.clanc < 3)
                    {
                        flytxt.flyUseContId("clan_8");
                        return;
                    }
                    edittext.enabled = true;
                    transform.FindChild("rt/text_bg").gameObject.SetActive(true);
                    edit.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_legion_txt8");
                   
                   
                    edittext.text = "";
                    transform.FindChild("rt/msg/Placeholder").GetComponent<Text>().text = ContMgr.getCont("clan_17");
                    flytxt.instance.fly(ContMgr.getCont("clan_3"));
                }
                else
                {
                   
                    edittext.enabled = false;
                    transform.FindChild("rt/text_bg").gameObject.SetActive(false);
                    edit.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_legion_txt9");
                    bool isContainKeyWord = a3_legion.mInstance.checkKeyWord(edittext.text, 4);
                    if (isContainKeyWord) return;

                    A3_LegionProxy.getInstance().SendChangeNotice(edittext.text);
                    if (edittext.text == "")
                    {
                        transform.FindChild("rt/msg/Placeholder").GetComponent<Text>().text = "";
                    }
                    flytxt.instance.fly(ContMgr.getCont("clan_4"));
                }
                edittext.interactable = !edittext.interactable;
            };
            s5 = main.__mainTrans.FindChild("s5");

            new BaseButton(transform.FindChild("btn_donation")).onClick = (GameObject) =>
            {
                s5.SetParent(s5.parent.parent.parent);
                s5.SetAsLastSibling();
                s5.gameObject.SetActive(true);
            };


            new BaseButton(transform.FindChild("help/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                main.CloseSub(help, helpParent);
            };
            new BaseButton(transform.FindChild("help/panel_help/bg_0")).onClick = (GameObject) =>
            {
                main.CloseSub(help, helpParent);
            };
            new BaseButton(transform.FindChild("help2/panel_help/closeBtn")).onClick = (GameObject) =>
            {
                help2.gameObject.SetActive(false);
            };
            new BaseButton(transform.FindChild("help2/panel_help/bg_0")).onClick = (GameObject) =>
            {
                help2.gameObject.SetActive(false);
            };
            new BaseButton(main.__mainTrans.FindChild("s5/btn_close")).onClick = (GameObject) =>
            {
                s5.SetParent(main.__mainTrans);
                s5.SetAsLastSibling();
                s5.gameObject.SetActive(false);
            };
            InputField df = main.__mainTrans.FindChild("s5/InputName").GetComponent<InputField>();
            new BaseButton(main.__mainTrans.FindChild("s5/btn_create")).onClick = (GameObject) =>
            {
                // jx_up(A3_LegionProxy.getInstance().gold,A3_LegionProxy.getInstance().lvl);
                var xml = XMLMgr.instance.GetSXML("clan.clan", "clan_lvl==" + A3_LegionProxy.getInstance().lvl);
                int num = xml.getInt("money_limit");

                if (A3_LegionProxy.getInstance().gold > num || (uint)int.Parse(df.text) + A3_LegionProxy.getInstance().gold > num)
                {
                    can_jx = false;
                }
                else
                {
                    can_jx = true;
                }

                if (can_jx == false)
                {
                    flytxt.instance.fly("帮派资金已经达到上限");

                }
                if (can_jx == true)
                {
                    A3_LegionProxy.getInstance().SendDonate((uint)int.Parse(df.text));
                    s5.SetParent(main.__mainTrans);
                    s5.SetAsLastSibling();
                    df.text = "0";
                    s5.gameObject.SetActive(false);
                }
            };
            df.onValueChange.AddListener((string str) =>
            {
                if (str == "") str = "0";
                main.gxdtext.text = ContMgr.getCont("clan_tip_0", (int.Parse(str) / 1000).ToString());
            });
            btn_lvup = new BaseButton(transform.FindChild("lt/btn_lvup"));
            btn_lvup.onClick = LvUpGroup;
            new BaseButton(transform.FindChild("btn_description")).onClick = Desc;
            new BaseButton(transform.FindChild("btn_description2")).onClick = (GameObject g) => help2.gameObject.SetActive(true);


            s7 = main.__mainTrans.FindChild("s7");

            new BaseButton(s7.transform.FindChild("raise_btn")).onClick = (GameObject) =>
            {

                s7.gameObject.SetActive(false);
            };

            new BaseButton(s7.transform.FindChild("close")).onClick = (GameObject) =>
            {
                s7.gameObject.SetActive(false);
            };
        }

        public override void onShowed()
        {

            main.CloseSub(help, helpParent);
            edittext.interactable = false;
            edit.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_legion_txt9");
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshInfo);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_QUIT, Quit);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_DONATE, OnDonateSuccess);
            A3_LegionProxy.getInstance().SendGetInfo();
        }

        public override void onClose()
        {
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETINFO, RefreshInfo);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_QUIT, Quit);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_DONATE, OnDonateSuccess);
            s7.gameObject.SetActive(false);
        }

        public void RefreshInfo(GameEvent e)
        {
            Variant data = e.data;
            // Debug.LogError(data.dump());
            if (data.ContainsKey("direct_join_clan"))
            {
                a3_legion.mInstance.dic0.isOn = data["direct_join_clan"]._bool;
            }
            if (A3_LegionModel.getInstance().myLegion.id <= 0)
            {
                main.ShowS(0);
                return;
            }
            if (A3_LegionModel.getInstance().myLegion.clanc < 3)
            {
                btn_lvup.gameObject.SetActive(false);
            }
            else
            {
                btn_lvup.gameObject.SetActive(true);
            }
            if (PlayerModel.getInstance().name == A3_LegionModel.getInstance().myLegion.name)
            {
                btn_quit.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_legion_txt10");
            }
            else btn_quit.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_legion_txt11");
            A3_LegionData ald = A3_LegionModel.getInstance().myLegion;
            transform.FindChild("lt/info/clan_name/Text/Text").GetComponent<Text>().text = ald.clname;
            transform.FindChild("lt/info/name/Text/Text").GetComponent<Text>().text = ald.name;
            transform.FindChild("lt/info/lvl/Text/Text").GetComponent<Text>().text = ald.lvl + ContMgr.getCont("ji");
            transform.FindChild("lt/info/exp/Text/Text").GetComponent<Text>().text = ald.exp + "/" + ald.exp_cost;
            transform.FindChild("lt/info/member_num/Text/Text").GetComponent<Text>().text = ald.plycnt + "/" + ald.member_max + "（" + ald.ol_cnt + ContMgr.getCont("clan_18")+")";
            transform.FindChild("lt/info/funds/Text/Text").GetComponent<Text>().text = ald.gold.ToString() + "/" + ald.gold_cost;
            transform.FindChild("lt/info/zdl/Text/Text").GetComponent<Text>().text = ald.combpt.ToString("N0");
            edittext.text = A3_LegionModel.getInstance().myLegion.notice.ToString();
            edittext.textComponent.text = A3_LegionModel.getInstance().myLegion.notice.ToString();
            //-------------------------------------------------------------------------------------------------------------
            Transform info = transform.FindChild("rt/buff/info");
            Transform pi = transform.FindChild("rt/buff/0");
            if (FunctionOpenMgr.instance.checkLegion(FunctionOpenMgr.LEGION))
            {
                getGameObjectByPath("btn_shop/dontopen").SetActive(false);
            }
            else
                getGameObjectByPath("btn_shop/dontopen").SetActive(true);

            foreach (var v in info.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == info.transform)
                    GameObject.Destroy(v.gameObject);

            }

          //  if (PlayerModel.getInstance().clan_buff_lvl == 0) PlayerModel.getInstance().clan_buff_lvl = 1;
            A3_LegionModel.getInstance().SetLegionBuff(PlayerModel.getInstance().clan_buff_lvl);
            foreach (var v in A3_LegionModel.getInstance().myLegionbuff.buffs.Keys)
            {
                var go = GameObject.Instantiate(pi.gameObject) as GameObject;
                go.transform.SetParent(info);
                go.transform.localScale = Vector3.one;
                go.GetComponent<Text>().text = Globle.getAttrNameById(v) + " + " + A3_LegionModel.getInstance().myLegionbuff.buffs[v];
                go.SetActive(true);
            }
            transform.FindChild("rt/buff/icon/Text").GetComponent<Text>().text = "LV " + PlayerModel.getInstance().clan_buff_lvl;

            if (PlayerModel.getInstance().clan_buff_lvl >= ald.lvl)
            {
                jn_add.gameObject.SetActive(false);

            }
            else
            {
                jn_add.gameObject.SetActive(true);
            }



            new BaseButton(transform.FindChild("rt/buff/icon")).onClick = (GameObject oo) =>
            {
                if (jn_add.gameObject.activeSelf)
                {
                    s7.gameObject.SetActive(true);
                    A3_LegionProxy.getInstance().SendGetMember();
                    BagProxy.getInstance().sendLoadItems(0);
                    buff_up();
                }
            };

        }

        public void buff_up()
        {



            Transform pi = transform.FindChild("rt/buff/0");

            foreach (var v in s7.transform.FindChild("newlvl/info").GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == s7.transform.FindChild("newlvl/info").transform)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            foreach (var v in s7.transform.FindChild("oldlvl/info").GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == s7.transform.FindChild("oldlvl/info").transform)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
            A3_LegionModel.getInstance().SetLegionBuff(PlayerModel.getInstance().clan_buff_lvl);
            foreach (var v in A3_LegionModel.getInstance().myLegionbuff.buffs.Keys)
            {
                var go = GameObject.Instantiate(pi.gameObject) as GameObject;
                go.transform.SetParent(s7.transform.FindChild("oldlvl/info"));
                go.transform.localScale = Vector3.one;
                go.GetComponent<Text>().text = Globle.getAttrNameById(v) + " + " + A3_LegionModel.getInstance().myLegionbuff.buffs[v];
                go.SetActive(true);
            }

            //s7.transform.FindChild("coin_change/coin/Text").GetComponent<Text>().text = A3_LegionModel.getInstance().donate+"/"+ A3_LegionModel.getInstance().myLegionbuff.cost_donate;
            //s7.transform.FindChild("coin_change/item/Text").GetComponent<Text>().text = a3_BagModel.getInstance().get_item_num((uint)A3_LegionModel.getInstance().myLegionbuff.cost_item)+"/"+ A3_LegionModel.getInstance().myLegionbuff.cost_num;
            s7.transform.FindChild("coin_change/item/haditem").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_item_" + A3_LegionModel.getInstance().myLegionbuff.cost_item.ToString());
            s7.transform.FindChild("oldlvl/icon/Text").GetComponent<Text>().text = "LV " + PlayerModel.getInstance().clan_buff_lvl;
            if (A3_LegionModel.getInstance().donate >= A3_LegionModel.getInstance().myLegionbuff_cost.cost_donate)
                s7.transform.FindChild("coin_change/coin/Text").GetComponent<Text>().text = "<color=#68FB2EFF>" + A3_LegionModel.getInstance().donate + "</color>" + "/" + "<color=#FFFFFFFF>" + A3_LegionModel.getInstance().myLegionbuff_cost.cost_donate + "</color>";
            else
                s7.transform.FindChild("coin_change/coin/Text").GetComponent<Text>().text = "<color=#F32C2CFF>" + A3_LegionModel.getInstance().donate + "</color>" + "/" + "<color=#FFFFFFFF>" + A3_LegionModel.getInstance().myLegionbuff_cost.cost_donate + "</color>";

            if (a3_BagModel.getInstance().getItemNumByTpid((uint)A3_LegionModel.getInstance().myLegionbuff_cost.cost_item) >= A3_LegionModel.getInstance().myLegionbuff_cost.cost_num)
                s7.transform.FindChild("coin_change/item/Text").GetComponent<Text>().text = "<color=#68FB2EFF>" + a3_BagModel.getInstance().getItemNumByTpid((uint)A3_LegionModel.getInstance().myLegionbuff_cost.cost_item) + "</color>" + "/" + "<color=#FFFFFFFF>" + A3_LegionModel.getInstance().myLegionbuff_cost.cost_num + "</color>";
            else
                s7.transform.FindChild("coin_change/item/Text").GetComponent<Text>().text = "<color=#F32C2CFF>" + a3_BagModel.getInstance().getItemNumByTpid((uint)A3_LegionModel.getInstance().myLegionbuff_cost.cost_item) + "</color>" + "/" + "<color=#FFFFFFFF>" + A3_LegionModel.getInstance().myLegionbuff_cost.cost_num + "</color>";

            if (PlayerModel.getInstance().clan_buff_lvl + 1 <= 12) 
            A3_LegionModel.getInstance().SetLegionBuff(PlayerModel.getInstance().clan_buff_lvl + 1);
            else
             A3_LegionModel.getInstance().SetLegionBuff(PlayerModel.getInstance().clan_buff_lvl);

            foreach (var v in A3_LegionModel.getInstance().myLegionbuff.buffs.Keys)
            {
                var go = GameObject.Instantiate(pi.gameObject) as GameObject;
                go.transform.SetParent(s7.transform.FindChild("newlvl/info"));
                go.transform.localScale = Vector3.one;
                go.GetComponent<Text>().text = Globle.getAttrNameById(v) + " + " + A3_LegionModel.getInstance().myLegionbuff.buffs[v];
                go.SetActive(true);
            }
            if (PlayerModel.getInstance().clan_buff_lvl + 1 <= 12)
                s7.transform.FindChild("newlvl/icon/Text").GetComponent<Text>().text = "LV " + (PlayerModel.getInstance().clan_buff_lvl + 1);
            else
                s7.transform.FindChild("newlvl/icon/Text").GetComponent<Text>().text = "LV " + (PlayerModel.getInstance().clan_buff_lvl );
            //s7.transform.FindChild("coin_change/need/coin/Text").GetComponent<Text>().text = A3_LegionModel.getInstance().myLegionbuff.cost_donate.ToString();
            //s7.transform.FindChild("coin_change/need/item/Text").GetComponent<Text>().text = A3_LegionModel.getInstance().myLegionbuff.cost_num.ToString();



            new BaseButton(s7.transform.FindChild("raise_btn")).onClick = (GameObject o) =>
            {
                if (/*A3_LegionModel.getInstance().myLegionbuff.cost_num <= a3_BagModel.getInstance().get_item_num((uint)A3_LegionModel.getInstance().myLegionbuff.cost_item)
                && */A3_LegionModel.getInstance().donate >= A3_LegionModel.getInstance().myLegionbuff.cost_donate)
                {
                    A3_LegionProxy.getInstance().SendUp_Buff();
                    s7.gameObject.SetActive(false);
                    if(PlayerModel.getInstance().clan_buff_lvl<12)
                    PlayerModel.getInstance().clan_buff_lvl += 1;
                    if (PlayerModel.getInstance().clan_buff_lvl == 1)
                        a3_buff.instance.legion_buf();
                    else
                        a3_buff.instance.resh_buff();

                    A3_LegionProxy.getInstance().SendGetInfo();
                }
                //else if (A3_LegionModel.getInstance().myLegionbuff.cost_num > a3_BagModel.getInstance().get_item_num((uint)A3_LegionModel.getInstance().myLegionbuff.cost_item))
                //    //&& A3_LegionModel.getInstance().donate >= A3_LegionModel.getInstance().myLegionbuff.cost_donate)
                //{
                //    if (XMLMgr.instance.GetSXML("item.item", "id==" + A3_LegionModel.getInstance().myLegionbuff.cost_item).GetNode("drop_info") == null)
                //        return;
                //    ArrayList data1 = new ArrayList();
                //    data1.Add(a3_BagModel.getInstance ().getItemDataById((uint)A3_LegionModel.getInstance().myLegionbuff.cost_item));
                //    data1.Add(InterfaceMgr.A3_SHEJIAO);
                //    InterfaceMgr.getInstance().open(InterfaceMgr.A3_ITEMLACK, data1);
                //}
                else
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_legion_txt12"));
                }

            };
        }

        void Quit(GameEvent e)
        {
            main.ShowS(0);
        }

        //升级骑士团
        void LvUpGroup(GameObject g)
        {
            if (A3_LegionModel.getInstance().myLegion.lvl >= A3_LegionModel.getInstance().myLegion.max_lvl)
            {
                flytxt.instance.fly(ContMgr.getCont("clan_5"));
            }
            else if (A3_LegionModel.getInstance().myLegion.exp >= A3_LegionModel.getInstance().myLegion.exp_cost)
            {
                A3_LegionProxy.getInstance().SendLVUP();
            }
            else flytxt.instance.fly(ContMgr.getCont("clan_6"));
        }

        //描述界面
        void Desc(GameObject g)
        {
            main.OpenSub(help);
        }

        //捐赠成功
        void OnDonateSuccess(GameEvent e)
        {
            Variant data = e.data;
            int money = data["money"];
            s5.SetParent(main.__mainTrans);
            s5.SetAsLastSibling();
            s5.gameObject.SetActive(false);
            RefreshInfo(null);
        }
    }

    class a3_legion_member : a3BaseLegion
    {
        public a3_legion_member(BaseShejiao win, string pathStr)
            : base(win, pathStr)
        {
        }

        Dictionary<GameObject, A3_LegionMember> gos = new Dictionary<GameObject, A3_LegionMember>();
        Dictionary<GameObject, BaseButton> gbtn = new Dictionary<GameObject, BaseButton>();
        Transform select, memOp, closeOp;
        Vector3 selectPosInit;
        Transform content;
        GameObject selectedMember;
        BaseButton btn_leader;
        BaseButton btn_promotion;
        BaseButton btn_demotion;
        BaseButton btn_remove;
        BaseButton btn_invitation;
        Transform s6;
        InputField df;
        public    uint inviteNum;
        public static a3_legion_member instance;
        public override void init()
        {
            instance = this;
            content = transform.FindChild("cells/scroll/content");
            select = transform.FindChild("cells/scroll/select");
            selectPosInit = select.GetComponent<RectTransform>().anchoredPosition;
            selectPosInit = new Vector3(selectPosInit.x, 0, selectPosInit.z);
            memOp = select.Find("menu");
            closeOp = memOp.Find("close_area");
            new BaseButton(closeOp).onClick = (g) => memOp.gameObject.SetActive(false);
            new BaseButton(memOp.Find("lookup")).onClick = (g) => 
            {
                if (curCid > 0)
                {
                    ArrayList arr = new ArrayList();
                    ArrayList arrP = new ArrayList();
                    arr.Add((uint)curCid);
                    arr.Add(InterfaceMgr.A3_SHEJIAO);                    
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO, arr);
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
                    a3_legion.lastPage = 1;
                }
            };
            btn_leader = new BaseButton(transform.FindChild("btn_leader"));
            btn_promotion = new BaseButton(transform.FindChild("btn_promotion"));
            btn_demotion = new BaseButton(transform.FindChild("btn_demotion"));
            btn_remove = new BaseButton(transform.FindChild("btn_remove"));
            btn_invitation = new BaseButton(transform.FindChild("btn_invitation"));
            btn_leader.onClick = PromoteToBeLeader;
            btn_promotion.onClick = Promotion;
            btn_demotion.onClick = Demotion;
            btn_remove.onClick = RemoveOne;
            btn_invitation.onClick = InviteMember;
            s6 = main.__mainTrans.FindChild("s6");
            new BaseButton(main.__mainTrans.FindChild("s6/btn_close")).onClick = (GameObject g) =>
            {
                df.text = "";
                //s6.SetParent(main.__mainTrans);
                //s6.SetAsLastSibling();
                s6.gameObject.SetActive(false);
                inviteNum = 0;
            };
            new BaseButton(main.__mainTrans.FindChild("s6/btn_cancel")).onClick = (GameObject g) =>
            {
                df.text = "";
                //s6.SetParent(main.__mainTrans);
                //s6.SetAsLastSibling();
                s6.gameObject.SetActive(false);
                inviteNum = 0;
            };
            df = main.__mainTrans.FindChild("s6/InputName").GetComponent<InputField>();

            new BaseButton(main.__mainTrans.FindChild("s6/btn_invite")).onClick = (GameObject g) =>
            {
                PlayerInfoProxy.getInstance().SendGetPlayerFromName(df.text);
                IsInvite = true;
                //if (inviteNum != 0)
                //{
                //    A3_LegionProxy.getInstance().SendInvite(inviteNum);

                //}

                //s6.gameObject.SetActive(false);
            };
            new BaseButton(main.__mainTrans.FindChild("s6/btn_search")).onClick = (GameObject g) =>
            {
                PlayerInfoProxy.getInstance().SendGetPlayerFromName(df.text);
            };
            main.__mainTrans.FindChild("s6/sa").gameObject.SetActive(false);
        }
        public  bool IsInvite = false;
       public    void InviteBtn()
        {
            IsInvite = false;
            if (inviteNum != 0)
            {
                A3_LegionProxy.getInstance().SendInvite(inviteNum);

            }

            s6.gameObject.SetActive(false);
        }
        public override void onShowed()
        {
            inviteNum = 0;            
            select.gameObject.SetActive(false);
            selectedMember = null;
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETMEMBER, RefreshMembersList);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_INVITE, OnInviteSuccess);
            PlayerInfoProxy.getInstance().addEventListener(PlayerInfoProxy.EVENT_ONGETPLAYERINFO, OnSearchMember);
            A3_LegionProxy.getInstance().SendGetMember();
            main.__mainTrans.FindChild("s6/sa").gameObject.SetActive(false);

            if (A3_LegionModel.getInstance().members != null)
            {
                foreach (int key in A3_LegionModel.getInstance().members.Keys)
                {
                    if (PlayerModel.getInstance().cid == A3_LegionModel.getInstance().members[key].cid)
                    {
                        if (A3_LegionModel.getInstance().members[key].clanc > 2)
                        {
                            btn_leader.gameObject.SetActive(true);
                            btn_promotion.gameObject.SetActive(true);
                            btn_demotion.gameObject.SetActive(true);
                            btn_remove.gameObject.SetActive(true);
                        }
                        else
                        {
                            btn_leader.gameObject.SetActive(false);
                            btn_promotion.gameObject.SetActive(false);
                            btn_demotion.gameObject.SetActive(false);
                            btn_remove.gameObject.SetActive(false);
                        }
                    }
                }
            }


            //if (A3_LegionProxy.getInstance().cacheProxyData != null)
            //    RefreshMembersList(GameEvent.Create(0, null, A3_LegionProxy.getInstance().cacheProxyData));
        }

        public override void onClose()
        {
            inviteNum = 0;
            select.gameObject.SetActive(false);
            selectedMember = null;
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETMEMBER, RefreshMembersList);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_INVITE, OnInviteSuccess);
            PlayerInfoProxy.getInstance().removeEventListener(PlayerInfoProxy.EVENT_ONGETPLAYERINFO, OnSearchMember);            
        }
        int curCid;
        void RefreshMembersList(GameEvent e)
        {
            selectedMember = null;
            select.gameObject.SetActive(false);
            select.SetParent(content.parent);
            Transform pre = transform.FindChild("cells/scroll/0");
            int i = 0;
            List<A3_LegionMember> mm = new List<A3_LegionMember>(A3_LegionModel.getInstance().members.Values);
            int onlinenum = mm.Count;
            if (mm.Count >= gos.Count)
            {
                for (int jj = gos.Count; jj < mm.Count; jj++)
                {
                    var go = GameObject.Instantiate(pre.gameObject) as GameObject;
                    go.transform.SetParent(content);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    SetLine(go.transform, mm[jj]);
                    if (mm[i].lastlogoff > 0) onlinenum--;
                }
            }
            foreach (var v in content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == content.transform)
                {
                    if (i < mm.Count)
                    {
                        SetLine(v, mm[i]);
                        if (mm[i].lastlogoff > 0) onlinenum--;
                        var tp = new List<A3_LegionMember>();
                        tp.Add(mm[i]);
                        var btn = new BaseButton(v.transform);
                        btn.onClick = (GameObject g) =>
                        {
                            debug.Log(tp[0].name);
                            select.SetParent(g.transform,false);
                            select.localPosition = selectPosInit;
                            select.localScale = Vector3.one;
                            select.gameObject.SetActive(true);
                            memOp.gameObject.SetActive(true);
                            selectedMember = g;
                            curCid = gos[g].cid;
                        };
                        gbtn[v.gameObject] = btn;
                        v.gameObject.SetActive(true);
                        i++;
                    }
                    else
                    {
                        if (gbtn.ContainsKey(v.gameObject))
                        {
                            gbtn[v.gameObject] = null;
                        }
                        v.gameObject.SetActive(false);
                    }
                }
            }
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (content.GetComponent<GridLayoutGroup>().cellSize.y) * A3_LegionModel.getInstance().members.Count);
            transform.FindChild("zx").GetComponent<Text>().text = mm.Count + "/" + A3_LegionModel.getInstance().myLegion.member_max + "(" + onlinenum + ContMgr.getCont("a3_legion_zx") +")";
        }

        void SetLine(Transform go, A3_LegionMember alm)
        {
            go.name = alm.cid.ToString();
            go.FindChild("zy/job2").gameObject.SetActive(false);
            go.FindChild("zy/job3").gameObject.SetActive(false);
            go.FindChild("zy/job5").gameObject.SetActive(false);
            string jj = "";
            switch (alm.clanc)
            {
                case 0: jj = ContMgr.getCont("a3_legion_xr"); break;
                case 1: jj = ContMgr.getCont("a3_legion_hy"); break;
                case 2: jj = ContMgr.getCont("a3_legion_jy"); break;
                case 3: jj = ContMgr.getCont("a3_legion_yl"); break;
                case 4: jj = ContMgr.getCont("a3_legion_lx"); break;
            }
            string zt = "";
            switch (alm.lastlogoff)
            {
                case 0: zt = ContMgr.getCont("a3_legion_zx"); break;
                default: zt = ContMgr.getCont("a3_legion_xx"); break;

            }

            go.FindChild("hyd").GetComponent<Text>().text = alm.huoyue.ToString();
            go.FindChild("name").GetComponent<Text>().text = alm.name;
            go.FindChild("jj").GetComponent<Text>().text = jj;
            // go.FindChild("zy/job1").gameObject.SetActive(false);
            go.FindChild("zy/job" + alm.carr).gameObject.SetActive(true);

            //Debug.LogError(alm.lastlogoff);
            go.FindChild("zt").GetComponent<Text>().text = zt;
            //go.FindChild("zy").GetComponent<Text>().text = A3_LegionModel.GetCarr(alm.carr);
            go.FindChild("dj").GetComponent<Text>().text = alm.zhuan.ToString() + ContMgr.getCont("zhuan") + alm.lvl.ToString() + ContMgr.getCont("ji");
            go.FindChild("zdl").GetComponent<Text>().text = alm.combpt.ToString();
            go.FindChild("gxd").GetComponent<Text>().text = alm.donate.ToString();
            gos[go.gameObject] = alm;
        }



        //晋升为领袖
        void PromoteToBeLeader(GameObject go)
        {

            if (selectedMember != null && gos.ContainsKey(selectedMember))
            {
                var v = gos[selectedMember];
                if ((uint)v.cid == PlayerModel.getInstance().cid)
                {
                    flytxt.flyUseContId("clan_7");
                    return;
                }
                if (v.clanc < 3)
                {
                    flytxt.flyUseContId("clan_15");
                    return;
                }
                A3_LegionProxy.getInstance().SendBeLeader((uint)v.cid);
               // flytxt.flyUseContId("clan_14");
            }
        }

        //晋升
        void Promotion(GameObject go)
        {
            if (selectedMember != null && gos.ContainsKey(selectedMember))
            {
                var v = gos[selectedMember];
                if ((uint)v.cid == PlayerModel.getInstance().cid)
                {
                    flytxt.flyUseContId("clan_7");
                    return;
                }
                if (v.clanc == 3)
                {
                    flytxt.flyUseContId("clan_16");
                    return;
                }
                A3_LegionProxy.getInstance().PromotionOrDemotion((uint)v.cid, (uint)1);
               // flytxt.flyUseContId("clan_14");
            }

        }

        //降职
        void Demotion(GameObject go)
        {
            if (selectedMember != null && gos.ContainsKey(selectedMember))
            {
                var v = gos[selectedMember];
                if ((uint)v.cid == PlayerModel.getInstance().cid)
                {
                    flytxt.flyUseContId("clan_7");
                    return;
                }
                if (v.clanc == 1)
                {
                    flytxt.flyUseContId("clan_13");
                    return;
                }
                A3_LegionProxy.getInstance().PromotionOrDemotion((uint)v.cid, (uint)0);
               // flytxt.flyUseContId("clan_14");
            }
        }

        //移除
        void RemoveOne(GameObject go)
        {
            if (selectedMember != null && gos.ContainsKey(selectedMember))
            {
                var v = gos[selectedMember];
                if ((uint)v.cid == PlayerModel.getInstance().cid)
                {
                    flytxt.flyUseContId("clan_7");
                    return;
                }
               
                A3_LegionProxy.getInstance().SendRemove((uint)v.cid);
            }

        }

        //邀请成员
        void InviteMember(GameObject go)
        {
            //s6.SetParent(s6.parent.parent.parent);
            //s6.SetAsLastSibling();
            s6.gameObject.SetActive(true);
            s6.FindChild("sa").gameObject.SetActive(false);
        }

        void OnSearchMember(GameEvent e)
        {
            Variant da = e.data;
            if (da.ContainsKey("res"))
            {
                if (da["res"] < 0)
                {
                    return;
                }
            }
            uint cid = da["cid"];
            inviteNum = cid;
            int combpt = da["combpt"];
            int carr = da["carr"];
            int zhuan = da["zhuan"];
            int lvl = da["lvl"];
            string name = da["name"];
            int clid = da["clid"];
            bool online = da["online"];
            s6.FindChild("sa").gameObject.SetActive(true);
            s6.FindChild("sa/Text").GetComponent<Text>().text = "ID:" + cid + ContMgr.getCont("a3_legion_nc") + name + ContMgr.getCont("a3_legion_zy") + A3_LegionModel.GetCarr(carr) + ContMgr.getCont("a3_legion_dj") + zhuan + ContMgr.getCont("zhuan") + lvl + ContMgr.getCont("ji");

            if (clid != 0)
                flytxt.instance.fly(ContMgr.getCont("a3_legion_havejt"));
        }

        void OnInviteSuccess(GameEvent e)
        {
            //s6.SetParent(main.__mainTrans);
            //s6.SetAsLastSibling();
            df.text = "";
            s6.gameObject.SetActive(false);
        }
    }

    class a3_legion_active : a3BaseLegion
    {

        public a3_legion_active(BaseShejiao win, string pathStr)
            : base(win, pathStr)
        {
        }
        public override void init()
        {
            base.init();
            new BaseButton(getTransformByPath("rect_mask/rect_scroll/clanDart")).onClick = (GameObject go) =>
             {
                 getGameObjectByPath("close").SetActive(true);
             };
            new BaseButton(getTransformByPath("close/dart/Button")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("close").SetActive(false);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SHEJIAO);
                if ((int)PlayerModel.getInstance().mapid == 10)
                {
                    SelfRole.moveToNPc(10, 1003);
                }
                else
                {
                    SelfRole.Transmit(10*100+1, after:()=>SelfRole.moveToNPc(10, 1003));
                }

            };
            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("close").SetActive(false);
            };

            new BaseButton(getTransformByPath("rect_mask/rect_scroll/cityOfwar")).onClick = (GameObject go) => {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CITYOFWAR );

            };

        }
        public override void onShowed()
        {
            base.onShowed();

            if (A3_LegionModel .getInstance ().myLegion.lvl < 2) {
                getTransformByPath("rect_mask/rect_scroll/cityOfwar").gameObject.SetActive(false);
            }
            else getTransformByPath("rect_mask/rect_scroll/cityOfwar").gameObject.SetActive(true);

        }

        public override void onClose()
        {
            base.onClose();
        }
    }

    class a3_legion_application : a3BaseLegion
    {
        public a3_legion_application(BaseShejiao win, string pathStr)
            : base(win, pathStr)
        {
        }
        Transform content;
        Toggle tt;
        public override void init()
        {
            content = getTransformByPath("cells/scroll/content");
            tt = transform.FindChild("Toggle").GetComponent<Toggle>();
            tt.isOn = A3_LegionModel.getInstance().CanAutoApply;
            new BaseButton(transform.FindChild("touch_toggle")).onClick = (GameObject) =>
            {
                if (A3_LegionModel.getInstance().members != null)
                {
                    foreach (int key in A3_LegionModel.getInstance().members.Keys)
                    {
                        if (PlayerModel.getInstance().cid == A3_LegionModel.getInstance().members[key].cid)
                        {
                            if (A3_LegionModel.getInstance().members[key].clanc > 1)
                            {
                                A3_LegionProxy.getInstance().SendChangeApplyMode(!tt.isOn);
                            }
                            else
                            {
                                flytxt.flyUseContId("clan_8");
                                return;
                            }
                        }
                    }
                }

            };
            //tt.onValueChanged.AddListener((bool b) =>
            //{
            //    A3_LegionProxy.getInstance().SendChangeApplyMode(b);
            //});
        }

        public override void onShowed()
        {

            foreach (int key in A3_LegionModel.getInstance().members.Keys)
            {
                if (PlayerModel.getInstance().cid == A3_LegionModel.getInstance().members[key].cid)
                {
                    if (A3_LegionModel.getInstance().members[key].clanc > 1)
                    {
                        A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_APPLYMODE, OnChangeApplyMode);
                        A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETAPPLICANT, OnGetApplicant);
                        A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_APPROVEORREJECT, OnApproveOrReject);
                        A3_LegionProxy.getInstance().SendGetApplicant();
                    }
                    else
                    {
                        return;
                    }
                }
            }

        }

        public override void onClose()
        {
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_APPLYMODE, OnChangeApplyMode);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETAPPLICANT, OnGetApplicant);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_APPROVEORREJECT, OnApproveOrReject);
            Transform content = getTransformByPath("cells/scroll/content");
            Transform pre = getTransformByPath("cells/scroll/0");
            foreach (var v in content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }
        }

        void OnChangeApplyMode(GameEvent e)
        {
            tt.isOn = A3_LegionModel.getInstance().CanAutoApply;
        }

        void OnGetApplicant(GameEvent e)
        {
            Transform pre = getTransformByPath("cells/scroll/0");
            foreach (var v in content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }

            foreach (var v in A3_LegionModel.getInstance().applicant.Values)
            {
                var go = GameObject.Instantiate(pre.gameObject) as GameObject;
                go.transform.SetParent(content);
                go.transform.FindChild("name").GetComponent<Text>().text = v.name;
                go.transform.FindChild("zy").GetComponent<Text>().text = A3_LegionModel.GetCarr(v.carr);
                go.transform.FindChild("dj").GetComponent<Text>().text = v.lvl.ToString();
                go.transform.FindChild("zdl").GetComponent<Text>().text = v.combpt.ToString();
                go.transform.localScale = Vector3.one;
                go.name = v.cid.ToString();
                go.SetActive(true);
                var temp = new List<A3_LegionMember>();
                temp.Add(v);
                new BaseButton(go.transform.FindChild("yes")).onClick = (GameObject g) =>
                {
                    A3_LegionProxy.getInstance().SendYN((uint)v.cid, true);
                };
                new BaseButton(go.transform.FindChild("no")).onClick = (GameObject g) =>
                {
                    A3_LegionProxy.getInstance().SendYN((uint)v.cid, false);
                };
            }
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (content.GetComponent<GridLayoutGroup>().cellSize.y) * A3_LegionModel.getInstance().list.Count);
        }

        void OnOpApplicant(GameEvent e)
        {
            Variant d = e.data;
            uint cid = d["cid"];
            bool b = d["approved"];
            var v = content.FindChild(cid.ToString());
            if (v != null)
            {
                GameObject.Destroy(v.gameObject);
            }
        }
        void OnApproveOrReject(GameEvent e)
        {
            Variant da = e.data;
            uint cid = da["cid"];
            bool ar = da["approved"];

            var gc = content.FindChild(cid.ToString());
            if (gc != null) GameObject.Destroy(gc.gameObject);
        }
    }

    class a3_legion_diary : a3BaseLegion
    {
        public a3_legion_diary(BaseShejiao win, string pathStr)
            : base(win, pathStr)
        {
        }

        public override void onShowed()
        {
            base.onShowed();
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_GETDIARY, RefreshDiary);
            A3_LegionProxy.getInstance().SendGetDiary();
        }

        public override void onClose()
        {
            base.onClose();
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_GETDIARY, RefreshDiary);
        }

        //1职务变动;2等级提升；3捐献；4新成员加入；5离开；6踢出；7创建工会；8修改公告；9批准加入；10转让会长,11,军团维护
        void RefreshDiary(GameEvent e)
        {
            GameObject pr = getGameObjectByPath("cells/scroll/0");
            Transform content = getTransformByPath("cells/scroll/content");

            foreach (var v in content.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent == content)
                {
                    GameObject.Destroy(v.gameObject);
                }
            }

            Variant data = A3_LegionModel.getInstance().logdata;
            if (data == null || !data.ContainsKey("clanlog_list")) return;
            Variant list = data["clanlog_list"];
            List<Variant> temp = new List<Variant>(list._arr);
            temp.Reverse();
            foreach (var v in temp)
            {
                var go = GameObject.Instantiate(pr) as GameObject;
                go.SetActive(true);
                go.transform.SetParent(content);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                Text tt = go.transform.FindChild("text").GetComponent<Text>();

                int logtp = v["logtp"];
                Variant log = v["log"];
                string oname = string.Empty;
                string name = string.Empty;
                string tname = string.Empty;
                string oldName = string.Empty;
                string newName = string.Empty;
                int guard_time = 0;
                int old_clanc = 0;
                int clanc = 0;
                int money = 0;
                int clan_lvl = 0;
                int repair_cost = 0;
                if (log.ContainsKey("name")) oname = log["name"];
                if (log.ContainsKey("tar_name")) tname = log["tar_name"];
                if (log.ContainsKey("clanc")) clanc = log["clanc"];
                if (log.ContainsKey("oldclanc")) old_clanc = log["oldclanc"];
                if (log.ContainsKey("money")) money = log["money"];
                if (log.ContainsKey("name")) name = log["name"];
                if (log.ContainsKey("guard_time")) guard_time = log["guard_time"];
                if (log.ContainsKey("repair_cost")) repair_cost = log["repair_cost"];
                if (log.ContainsKey("clan_lvl")) clan_lvl = log["clan_lvl"];
                if (log.ContainsKey("old_clname")) oldName = log["old_clname"];
                if (log.ContainsKey("new_clname")) newName = log["new_clname"];
                switch (logtp)
                {
                    case 1:
                        string ss = string.Empty;
                        if (clanc < old_clanc) ss = ContMgr.getCont("a3_legion_jj");
                        else ss = ContMgr.getCont("a3_legion_sj");
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { tname, oname, ss, A3_LegionModel.getInstance().GetClancToName(clanc) });
                        break;
                    case 2:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { oname, A3_LegionModel.getInstance().myLegion.lvl.ToString() });
                        break;
                    case 3:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { A3_LegionModel.GetCarr(PlayerModel.getInstance().profession), oname, money.ToString() });
                        break;
                    case 4:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { name });
                        break;
                    case 5:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { name });
                        break;
                    case 6:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { tname, oname });
                        break;
                    case 7:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { oname });
                        break;
                    case 8:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { oname });
                        break;
                    case 9:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { tname, oname });
                        break;
                    case 10:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { oname, tname });
                        break;
                    case 11:
                        if (clan_lvl <= 1)
                        {

                            tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { (4 - guard_time).ToString() });
                        }
                        else
                        {                          
                             tt.text = ContMgr.getCont("clan_log_12", new List<string> { (4 - guard_time).ToString(), (clan_lvl - 1).ToString() });
                        }
                        break;
                    case 12:
                        tt.text = ContMgr.getCont("clan_log_14" , new List<string> { (clan_lvl).ToString() });
                        break;
                    case 13:
                        tt.text = ContMgr.getCont("clan_log_" + logtp, new List<string> { repair_cost.ToString() });
                        break;
                    case 15:
                        tt.text = ContMgr.getCont("clan_log_ChangeName", new List<string> { name ,oldName ,newName });
                        break;
                }
            }
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (content.GetComponent<GridLayoutGroup>().cellSize.y) * list._arr.Count);
        }
    }
    public class myComparer : IComparer<A3_LegionData>
    {
        public int Compare(A3_LegionData y, A3_LegionData x)
        {
            var ss = (x.lvl.CompareTo(y.lvl));
            if (ss == 0)
            {
                ss = (x.combpt.CompareTo(y.combpt));
                if (ss == 0)
                {
                    ss = (x.huoyue.CompareTo(y.huoyue));
                    if (ss == 0)
                    {
                        ss = (x.plycnt.CompareTo(y.plycnt));
                    }
                }
            }
            return ss;
        }
    }
}
