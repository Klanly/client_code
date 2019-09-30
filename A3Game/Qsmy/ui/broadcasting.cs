using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cross;
using MuGame;
namespace MuGame
{
    class broadcasting : LoadingUI
    {
        public static List<BroadcastingData> talkList = new List<BroadcastingData>();
        private List<BroadcastingData> system_msg = new List<BroadcastingData>();
        private List<BroadcastingData> system_gonggao_msg = new List<BroadcastingData>();
        BroadcastingData _data = null;
        private float hight = 31;

        private GameObject Text;
        private GameObject openBtn;

        private GameObject currentText;
        private RectTransform currentRect;
        private RectTransform bg;
        bool isBuilded = false;

        private GameObject pastText = null;
        private RectTransform pastRect;
        private float timer = 0;

        private Text numMark;
        private int num = 0;

        bool canGetNext = true;
        bool canGetNext_Sys = true;
        private float _length = 0;
        public static float OutTimer;

        public static bool isOn;

        public static broadcasting instance;

        BaseButton bsBtnOpen;

        private GameObject child;
        private GameObject mask;     //普通公告
        private GameObject mask_sys; //系统公告
        private Text mask_sys_txt;
        private List<iconActionData> iconAction = new List<iconActionData>();
        public override void init()
        {
            base.init();
            instance = this;
            ChatProxy.getInstance();
            Text = transform.FindChild("Text").gameObject;
            bg = transform.FindChild("bg").GetComponent<RectTransform>();
            InterfaceMgr.setUntouchable(bg.gameObject);
            bg.GetComponent<CanvasGroup>().blocksRaycasts = false;

            numMark = transform.FindChild("openBtn/mark/Text").GetComponent<Text>();
            openBtn = transform.FindChild("openBtn").gameObject;
            bsBtnOpen = new BaseButton(openBtn.transform, 0);
            bsBtnOpen.onClick = onClickOpen;

            mask = getGameObjectByPath("mask");
            mask.SetActive(false);
            mask_sys = getGameObjectByPath("mask_sys");
            mask_sys.SetActive(false);
            mask_sys_txt = mask_sys.transform.FindChild("txt").GetComponent<Text>();
            showMark();
            ChatProxy.getInstance().addEventListener(ChatProxy.lis_sys_notice, getNotice);

            RectTransform cv = cemaraRectTran;

            RectTransform thisRect = gameObject.GetComponent<RectTransform>();


            thisRect.sizeDelta = new Vector2(cv.rect.width, cv.rect.height);

            child = getGameObjectByPath("child");


            this.transform.SetAsFirstSibling();

        }

        public bool panel_open()
        {
            var xml = XMLMgr.instance.GetSXML("func_open.func", "id==32");
            string[] param = xml.getString("param1").Split(',');
            int zhuan = int.Parse(param[0]);
            int lv = int.Parse(param[1]);

            bool v = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl >= zhuan * 100 + lv;
            return v;
        }

        public override void onShowed()
        {
            base.onShowed();
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, 0);
        }

        void onOfflineEXP()
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.OFFLINEEXP);
        }

        void getNotice(GameEvent e)
        {
            debug.Log("广播信息:" + e.data.dump());
            #region switch it
            if (e.data.ContainsKey("tp"))
            {
                BroadcastingData _data = new BroadcastingData();
                iconActionData _iconData = new iconActionData();
                _data.msg = e.data["msg"];
                string[] arr = _data.msg.Split(':');
                if (e.data["tp"] == 1)
                {
                    a3_ItemData dta = new a3_ItemData();
                    dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[1]));
                    string strMsg = ContMgr.getCont("lottery_roll", new List<string> { arr[0], Globle.getColorStrByQuality(dta.item_name, dta.quality) + "x" + arr[2] });
                    Variant v = new Variant();
                    v["tp"] = 6;
                    v["msg"] = strMsg;
                    _data.msg = strMsg;
                    system_msg.Add(_data);
                    _iconData.iconOpen = false;
                    _iconData.action = null;
                    iconAction.Add(_iconData);
                    //if (a3_chatroom._instance != null)
                    //a3_chatroom._instance.otherSays(v);
                }
                else if (e.data["tp"] == 2)
                {
                    EquipConf dta = new EquipConf();
                    dta = EquipModel.getInstance().getEquipDataById(int.Parse(arr[1]));
                    EquipConf dta2 = new EquipConf();
                    dta2 = EquipModel.getInstance().getEquipDataById(int.Parse(arr[2]));
                    _data.msg = ContMgr.getCont("equip_strengthen", new List<string> { arr[0], Globle.getColorStrByQuality(dta.name, dta.quality), Globle.getColorStrByQuality(dta2.name, dta2.quality) });
                    system_msg.Add(_data);
                    _iconData.iconOpen = false;
                    _iconData.action = null;
                    iconAction.Add(_iconData);
                }
                else if (e.data["tp"] == 3)
                {
                    a3_ItemData dta = new a3_ItemData();
                    dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[1]));
                    _data.msg = ContMgr.getCont("active_buying", new List<string> { arr[0], Globle.getColorStrByQuality(dta.item_name, dta.quality) });
                    system_msg.Add(_data);
                    _iconData.iconOpen = false;
                    _iconData.action = null;
                    iconAction.Add(_iconData);
                }
                else if (e.data["tp"] == 4)
                {
                    string str;
                    if (int.Parse(arr[2]) > 1)
                    {
                        str = arr[0] + ContMgr.getCont("broadcasting0") + arr[1] + " (" + arr[2] + ContMgr.getCont("broadcasting1");
                    }
                    else
                    {
                        str = arr[0] + ContMgr.getCont("broadcasting0") + arr[1];
                    }
                    _data.msg = str;
                    system_msg.Add(_data);
                    _iconData.iconOpen = false;
                    _iconData.action = null;
                    iconAction.Add(_iconData);
                }
                else if (e.data["tp"] == 5)//装备进阶成功
                {
                    // Debug.LogError("广播"+ e.data.dump());
                    a3_ItemData dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[1]));
                    #region broadcast
                    //_data.msg = ContMgr.getCont("equip_strengthen", new List<string> { arr[0], Globle.getColorStrByQuality(equipname, dta.quality), arr[2] });
                    //system_msg.Add(_data);
                    //_iconData.iconOpen = false;
                    //_iconData.action = null;
                    //iconAction.Add(_iconData);
                    #endregion
                    string playerName = arr[0], equipName = dta.item_name, equipLevel = arr[2];
                    List<Variant> listMsgEdit = new List<Variant>();
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("equip_strengthen1") });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = playerName, ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("equip_strengthen2") });
                    Color equipNameColor = Globle.getColorForChatMsgByQuality(dta.quality);
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = equipName, ["r"] = equipNameColor.r, ["g"] = equipNameColor.g, ["b"] = equipNameColor.b });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("equip_strengthen3") });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = equipLevel, ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("equip_strengthen4") });
                    string str = ContMgr.getCont("equip_strengthen1") + playerName + ContMgr.getCont("equip_strengthen2") + equipName + ContMgr.getCont("equip_strengthen3") + equipLevel + ContMgr.getCont("equip_strengthen4");
                    Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                    //if (a3_chatroom._instance != null)
                    //    a3_chatroom._instance.otherSays(v, listMsgEdit);
                }
                else if (e.data["tp"] == 6)//抽奖
                {
                    a3_ItemData dta = new a3_ItemData();
                    dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[1]));
                    string str = "<color=#FF0000>" + arr[0] + "</color>" + ContMgr.getCont("broadcasting2");
                    str += Globle.getColorStrByQuality(dta.item_name, dta.quality);
                    _data.msg = str;
                    system_msg.Add(_data);
                    Variant data = new Variant();
                    data["name"] = arr[0];
                    data["itemId"] = arr[1];
                    data["cnt"] = arr[2];
                    data["stage"] = arr.Length > 3 ? arr[3] : "0";
                    data["intensify"] = arr.Length > 4 ? arr[4] : "0";
                    _iconData.iconOpen = true;
                    _iconData.action = (GameObject o) => {
                        if (MapModel.getInstance().curLevelId > 0)
                        {
                            flytxt.instance.fly(ContMgr.getCont("infb_null"));
                            return;
                        }
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY); };
                    iconAction.Add(_iconData);
                    if (str == null) isOn = false;
                    else isOn = true;
                    UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_GETLOTTERYDRAWINFO, this, data));
                    str = arr[0] + ContMgr.getCont("broadcasting2") + dta.item_name;
                    Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                    List<Variant> listMsgEdit = new List<Variant>();
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = arr[0], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting2") });
                    Color equipColor;
                    if (dta.quality == 4) equipColor = new Color(0xFF, 0x00, 0xFF);
                    else if (dta.quality == 5) equipColor = new Color(0xFF, 0x7F, 0x0A);
                    else equipColor = new Color(0x00, 0x00, 0x00);
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = dta.item_name, ["r"] = equipColor.r, ["g"] = equipColor.g, ["b"] = equipColor.b });
                    //if (a3_chatroom._instance != null)
                    //    a3_chatroom._instance.otherSays(v, listMsgEdit);
                }
                else if (e.data["tp"] == 7)//世界boss
                {
                    int r = -1;
                    if (!int.TryParse(arr[0], out r)) return;
                    switch (r)
                    {
                        case 1:
                            _data.msg = ContMgr.getCont("mwsl_bc_" + arr[1], new List<string> { arr[2] });
                            system_msg.Add(_data);
                            _iconData.iconOpen = true;
                            _iconData.action = (GameObject) =>
                            {
                                if (MapModel.getInstance().curLevelId > 0)
                                {
                                    flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                    return;
                                }
                                ArrayList arr1 = new ArrayList();
                                arr1.Add(ELITE_MONSTER_PAGE_IDX.BOSSPAGE);
                                if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
                                {
                                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON, arr1);
                                }
                                else
                                    flytxt.instance.fly(ContMgr.getCont("func_limit_16"));

                            };
                            iconAction.Add(_iconData);
                            Variant v = new Variant { ["tp"] = 6, ["msg"] = _data.msg };
                            if (a3_chatroom._instance != null)
                                a3_chatroom._instance.otherSays(v);

                            break;
                        case 2:
                            _data.msg = ContMgr.getCont("mwsl_ltbc_" + arr[1]);
                            system_msg.Add(_data);
                            _iconData.iconOpen = true;
                            _iconData.action = (GameObject) =>
                            {
                                if (MapModel.getInstance().curLevelId > 0)
                                {
                                    flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                    return;
                                }
                                ArrayList arr1 = new ArrayList();
                                arr1.Add(ELITE_MONSTER_PAGE_IDX.BOSSPAGE);
                                if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
                                {
                                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON, arr1);
                                }
                                else
                                    flytxt.instance.fly(ContMgr.getCont("func_limit_16"));

                            };
                            iconAction.Add(_iconData);

                            break;
                        case 3://击杀
                            var xmm = XMLMgr.instance.GetSXML("monsters");
                            var xx = xmm.GetNode("monsters", "id==" + arr[1]);
                            string name = "";
                            if (xx != null)
                                name = xx.getString("name");
                            string str = name + ContMgr.getCont("mwsl_killed") + arr[2];
                            _data.msg = str; //, new List<string> { name, arr[2] });
                            system_msg.Add(_data);
                            _iconData.iconOpen = false;
                            _iconData.action = null;
                            iconAction.Add(_iconData);
                            A3_ActiveProxy.getInstance().SendGetBossInfo();

                            Variant _v = new Variant { ["tp"] = 6, ["msg"] = str };
                            if (a3_chatroom._instance != null)
                                a3_chatroom._instance.otherSays(_v);
                            break;
                    }
                }
                else if (e.data["tp"] == 8)
                {
                    _data.msg = arr[0] + " " + ContMgr.getCont("broadcasting17") + " "/* " <color=#FF00FF>" */+ arr[1]/* + "</color>"*/;
                    system_msg.Add(_data);
                    Variant v = new Variant { ["tp"] = 6, ["msg"] = _data.msg };
                    if (a3_chatroom._instance != null)
                        a3_chatroom._instance.otherSays(v);
                }
                else if (e.data["tp"] == 10)
                {
                    if (a3_insideui_fb.instance != null) a3_insideui_fb.instance.SetBroadCast(e.data);
                    //_data.msg = ContMgr.getCont(e.data["msg"]);
                    //system_msg.Add(_data);
                }
                else if (e.data["tp"] == 11) //盗宝妖
                {
                    string mag = e.data["msg"];
                    if (mag == "2:")
                    {
                        Variant v = new Variant();
                        v["tp"] = 6;
                        v["msg"] = ContMgr.getCont("sjbt_over");
                        _data.msg = v["msg"];
                        system_msg.Add(_data);
                        _iconData.iconOpen = false;
                        _iconData.action = null;
                        iconAction.Add(_iconData);
                        if (a3_chatroom._instance != null)
                            a3_chatroom._instance.otherSays(v);
                    }
                    else
                    {
                        List<string> listMapName = new List<string>();
                        for (int i = 1; i < arr.Length; i++)
                            listMapName.Add(arr[i]);
                        if (listMapName.Count == 0) return;

                        string sjbtMsg = ContMgr.getCont("sjbt_start1"), stbjBroadcasting = ContMgr.getCont("sjbt_start1");
                        List<Variant> listMsgEdit = new List<Variant>();
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("sjbt_start1") });
                        for (int i = 0; i < listMapName.Count; i++)
                        {
                            sjbtMsg = sjbtMsg + listMapName[i];
                            stbjBroadcasting = stbjBroadcasting + "<color=#FF00FF>" + listMapName[i] + "</color>";
                            listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = listMapName[i], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0xFF });
                            if (i + 1 < listMapName.Count)
                            {
                                sjbtMsg = sjbtMsg + ContMgr.getCont("sjbt_start2");
                                stbjBroadcasting = stbjBroadcasting + ContMgr.getCont("sjbt_start2");
                                listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("sjbt_start2") });
                            }
                        }
                        sjbtMsg = sjbtMsg + ContMgr.getCont("sjbt_start3");
                        stbjBroadcasting = stbjBroadcasting + ContMgr.getCont("sjbt_start3");
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("sjbt_start3") });

                        Variant v = new Variant();
                        v["tp"] = 6;
                        v["msg"] = sjbtMsg;
                        _data.msg = stbjBroadcasting;
                        system_msg.Add(_data);
                        _iconData.iconOpen = false;
                        _iconData.action = null;
                        iconAction.Add(_iconData);

                        if (a3_chatroom._instance != null)
                        {
                            v["custom"] = true;
                            a3_chatroom._instance.otherSays(v, listMsgEdit);
                        }
                    }
                }
                else if (e.data["tp"] == 12)//捡到金色以上品质装备
                {
                    if (arr.Length != 3) return;
                    a3_ItemData dta = new a3_ItemData();
                    string mdata = "";
                    dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[2]));
                    //string str;
                    //string color = "";
                    //switch (dta.quality)
                    //{
                    //    //case 4: color = "<color=#FF00FF>" + dta.item_name + "</color>"; break;
                    //    case 5: color = "<color=#FF7F0A>" + dta.item_name + "</color>"; break;
                    //    default:
                    //        break;
                    //}
                    Variant datas = SvrMapConfig.instance.getSingleMapConf(uint.Parse(arr[1]));
                    mdata = datas["map_name"];
                    if (dta.quality >= 5)
                    {
                        #region broadcast
                        //_data.msg = str;
                        //system_msg.Add(_data);
                        //if (int.Parse(arr[1]) > 3338 && int.Parse(arr[1]) < 3341)//待改
                        //{
                        //    _iconData.iconOpen = true;
                        //    _iconData.action = (GameObject) =>
                        //    {
                        //        if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DEVIL_HUNTER))
                        //        {
                        //            ArrayList arr1 = new ArrayList();
                        //            arr1.Add("mlzd");
                        //            InterfaceMgr.getInstance().open(InterfaceMgr.A3_ACTIVE, arr1);
                        //        }
                        //        else
                        //            flytxt.instance.fly(ContMgr.getCont("func_limit_12"));
                        //    };
                        //    iconAction.Add(_iconData);

                        //}
                        //else if (int.Parse(arr[1]) > 3341 && int.Parse(arr[1]) < 3348)
                        //{
                        //    _iconData.iconOpen = true;
                        //    _iconData.action = (GameObject) =>
                        //    {
                        //        if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.COUNTERPART))
                        //        {
                        //            InterfaceMgr.getInstance().open(InterfaceMgr.A3_COUNTERPART);
                        //        }
                        //        else
                        //        {
                        //            flytxt.instance.fly(ContMgr.getCont("func_limit_32"));
                        //        }

                        //    };
                        //    iconAction.Add(_iconData);
                        //}
                        #endregion
                        string playerName = arr[0], newUpLevel = arr[1];
                        List<Variant> listMsgEdit = new List<Variant>();
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = arr[0], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting3") });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = mdata, ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting4") });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = dta.item_name, ["r"] = 0xFF, ["g"] = 0x7F, ["b"] = 0x0A });
                        string str = arr[0] + ContMgr.getCont("broadcasting3") + mdata + ContMgr.getCont("broadcasting4") + dta.item_name;
                        Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                        //if (a3_chatroom._instance != null)
                        //    a3_chatroom._instance.otherSays(v, listMsgEdit);
                    }
                }
                else if (e.data["tp"] == 13)//刷新宝箱广播
                {
                    string str;
                    Debug.Log("宝箱信息::::" + e.data.dump());
                    string mdata1 = "";
                    string mdata2 = "";
                    string mdata3 = "";
                    int time;


                    if (int.Parse(arr[0]) == 1)
                    {
                        Variant data1 = SvrMapConfig.instance.getSingleMapConf(uint.Parse(arr[1]));
                        mdata1 = data1["map_name"];
                        time = int.Parse(arr[2]);
                        str = ContMgr.getCont("broadcasting5") + "<color=#FF0000>" + time + "</color>" + ContMgr.getCont("broadcasting6");
                        str += "<color=#E3B36B>" + mdata1 + "</color>";
                        str += ContMgr.getCont("broadcasting7");
                        _data.msg = str;
                        system_msg.Add(_data);
                        _iconData.iconOpen = true;
                        _iconData.action = (GameObject) =>
                        {
                            if (MapModel.getInstance().curLevelId > 0) {
                                flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                return;
                            }
                            if (!panel_open())
                            {
                                flytxt.instance.fly(ContMgr.getCont("comm_lv"));
                                return;
                            }
                            ArrayList dl = new ArrayList();
                            dl.Add("forchest");
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                        };
                        iconAction.Add(_iconData);
                    }
                    if (int.Parse(arr[0]) == 2)
                    {

                        time = int.Parse(arr[1]);
                        string mapName = "";
                        if (e.data.ContainsKey("maps"))
                        {
                            List<Variant> listMapId = e.data["maps"]._arr;
                            for (int i = 0; i < listMapId.Count;)
                            {
                                int mapId = listMapId[i];
                                Variant mapConfig = SvrMapConfig.instance.mapConfs[(uint)mapId];
                                if (mapConfig != null && mapConfig.ContainsKey("map_name"))
                                {
                                    mapName += mapConfig["map_name"];
                                    i++;
                                    if (i < listMapId.Count)
                                        mapName += ",";
                                }
                            }
                        }
                        if (time < 5)
                        {
                            str = ContMgr.getCont("broadcasting8") + "<color=#FF0000>" + time + "</color>" + ContMgr.getCont("broadcasting9") + "<color=#E3B36B>" + mapName + "</color>";
                        }
                        else
                        {
                            str = ContMgr.getCont("broadcasting10") + "<color=#E3B36B>" + mapName + "</color>";
                        }
                        _data.msg = str;
                        system_msg.Add(_data);
                        _iconData.iconOpen = true;
                        _iconData.action = (GameObject) =>
                        {
                            if (MapModel.getInstance().curLevelId > 0)
                            {
                                flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                return;
                            }
                            if (!panel_open())
                            {
                                flytxt.instance.fly(ContMgr.getCont("comm_lv"));
                                return;
                            }
                            ArrayList dl = new ArrayList();
                            dl.Add("forchest");
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                        };
                        iconAction.Add(_iconData);
                    }



                    if (int.Parse(arr[0]) == 3)
                    {
                        a3_ItemData dta = new a3_ItemData();
                        dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[2]));
                        string color = "";
                        switch (dta.quality)
                        {
                            case 4: color = "<color=#FF00FF>" + dta.item_name + "</color>"; break;
                            case 5: color = "<color=#FF7F0A>" + dta.item_name + "</color>"; break;
                            default:
                                break;
                        }

                        if (dta.quality >= 4)
                        {

                            str = "<color=#FF0000>" + arr[1] + "</color>" + ContMgr.getCont("broadcasting11");
                            // str += "<color=#E3B36B>" + mdata + "</color>";
                            str += ContMgr.getCont("broadcasting12") + color;
                            _data.msg = str;
                            system_msg.Add(_data);
                            _iconData.iconOpen = true;
                            _iconData.action = (GameObject) =>
                            {
                                if (MapModel.getInstance().curLevelId > 0)
                                {
                                    flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                    return;
                                }
                                if (!panel_open())
                                {
                                    flytxt.instance.fly(ContMgr.getCont("comm_lv"));
                                    return;
                                }
                                ArrayList dl = new ArrayList();
                                dl.Add("forchest");
                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ACTIVE, dl);
                            };
                            iconAction.Add(_iconData);
                        }
                    }
                    //icon.gameObject.SetActive(true);
                    //Enter_Btn = new BaseButton(icon);
                    //Enter_Btn.onClick = (GameObject) => { InterfaceMgr.getInstance().open(InterfaceMgr.WORLD_MAP); };
                }
                else if (e.data["tp"] == 14) //转生广播
                {
                    string playerName = arr[0], newUpLevel = arr[1];
                    List<Variant> listMsgEdit = new List<Variant>();
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = playerName, ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("comm_zhuan_up1") });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = newUpLevel, ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("comm_zhuan_up2") });
                    string str = playerName + ContMgr.getCont("comm_zhuan_up1") + newUpLevel + ContMgr.getCont("comm_zhuan_up2");
                    Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                    //if (a3_chatroom._instance != null)
                    //    a3_chatroom._instance.otherSays(v, listMsgEdit);

                }
                else if (e.data["tp"] == 15) //创建副本组队广播
                {
                    string str, broadcastingMsg;
                    int levelId = 0, teamId = 0;
                    int.TryParse(arr[1], out levelId);
                    int.TryParse(arr[2], out teamId);
                    if (SvrLevelConfig.instacne.isLevel(levelId))
                    {
                        List<Variant> listMsgEdit = new List<Variant>();
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting13") });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = arr[0], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting14") });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = SvrLevelConfig.instacne.get_level_data((uint)levelId)["name"], ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting15") });
                        listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("fb_msg_start4"), ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B, ["xtp"] = 1, ["tid"] = teamId });
                        str = ContMgr.getCont("broadcasting13") + arr[0] + ContMgr.getCont("broadcasting14") + SvrLevelConfig.instacne.get_level_data((uint)levelId)["name"] + ContMgr.getCont("broadcasting15") + ContMgr.getCont("fb_msg_start4");
                        broadcastingMsg = ContMgr.getCont("broadcasting13") +
                            "<color=#FF0000>" + arr[0] + "</color>"
                            + ContMgr.getCont("broadcasting14") +
                            "<color=#E3B36B>" + SvrLevelConfig.instacne.get_level_data((uint)levelId)["name"] + "</color>"
                            + ContMgr.getCont("broadcasting15");
                        _data.msg = broadcastingMsg;
                        system_msg.Add(_data);
                        Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                        if (a3_chatroom._instance != null)
                            a3_chatroom._instance.otherSays(v, listMsgEdit);
                    }
                }
                else if (e.data["tp"] == 16)//装备上架拍卖行
                {
                    //这里特殊处理下，转换成聊天的msg
                    uint tpid = e.data["items"]._arr[0]["tpid"];
                    a3_ItemData dta = new a3_ItemData();
                    dta = a3_BagModel.getInstance().getItemDataById(tpid);

                    string msg = "[" + arr[0] + "/" + arr[1] + "]" + ContMgr.getCont("broadcasting16") + "[" + dta.item_name + "#" + "]";
                    e.data["msg"] = msg;
                    e.data["tp"] = 6;
                    e.data["cid"] = arr[0];
                    //if (a3_chatroom._instance != null)
                    //    a3_chatroom._instance.otherSays(e.data);
                }

                else if (e.data["tp"] == 17)
                {
                    var xmm = XMLMgr.instance.GetSXML("monsters");
                    var xx = xmm.GetNode("monsters", "id==" + arr[0]);
                    string name = "";
                    if (xx != null) {

                        int zhuan = xx.getInt("zhuan");
                        int lv = xx.getInt("lv");
                        if (PlayerModel.getInstance().up_lvl < zhuan) {
                            return;
                        } else if (PlayerModel.getInstance().up_lvl == zhuan) {
                            if (PlayerModel.getInstance().lvl < lv) {
                                return;
                            }
                        }
                        name = xx.getString("name");
                        Variant v = new Variant();
                        v["tp"] = 6;
                        v["msg"] = ContMgr.getCont("boos_go", name);
                        _data.msg = v["msg"];
                        system_msg.Add(_data);
                        _iconData.iconOpen = true;
                        _iconData.action = (GameObject) =>
                        {
                            if (MapModel.getInstance().curLevelId > 0)
                            {
                                flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                return;
                            }
                            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
                            {
                                flytxt.instance.fly(ContMgr.getCont("comm_lv"));
                                return;
                            }
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON);
                        };
                        iconAction.Add(_iconData);
                        if (a3_chatroom._instance != null)
                            a3_chatroom._instance.otherSays(v);

                    }
                }
                else if (e.data["tp"] == 18) {
                    var xmm = XMLMgr.instance.GetSXML("monsters");
                    var xx = xmm.GetNode("monsters", "id==" + arr[0]);
                    string name = "";
                    if (xx != null)
                    {

                        int zhuan = xx.getInt("zhuan");
                        int lv = xx.getInt("lv");
                        if (PlayerModel.getInstance().up_lvl < zhuan)
                        {
                            return;
                        }
                        else if (PlayerModel.getInstance().up_lvl == zhuan)
                        {
                            if (PlayerModel.getInstance().lvl < lv)
                            {
                                return;
                            }
                        }
                        name = xx.getString("name");
                        Variant v = new Variant();
                        v["tp"] = 6;
                        v["msg"] = ContMgr.getCont("boos_go_1", name);
                        _data.msg = v["msg"];
                        system_msg.Add(_data);
                        _iconData.iconOpen = true;
                        _iconData.action = (GameObject) =>
                        {
                            if (MapModel.getInstance().curLevelId > 0)
                            {
                                flytxt.instance.fly(ContMgr.getCont("infb_null"));
                                return;
                            }
                            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.GLOBA_BOSS))
                            {
                                flytxt.instance.fly(ContMgr.getCont("comm_lv"));
                                return;
                            }
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ELITEMON);
                        };
                        iconAction.Add(_iconData);
                        if (a3_chatroom._instance != null)
                            a3_chatroom._instance.otherSays(v);

                    }

                }
                else if (e.data["tp"] == 19) {

                    a3_ItemData dta = new a3_ItemData();
                    dta = a3_BagModel.getInstance().getItemDataById(uint.Parse(arr[1]));
                    string str = "<color=#FF0000>" + arr[0] + "</color>" + ContMgr.getCont("broadcasting2_vip");
                    str += Globle.getColorStrByQuality(dta.item_name, dta.quality);
                    _data.msg = str;
                    system_msg.Add(_data);
                    Variant data = new Variant();
                    data["name"] = arr[0];
                    data["itemId"] = arr[1];
                    data["cnt"] = arr[2];
                    data["stage"] = arr.Length > 3 ? arr[3] : "0";
                    data["intensify"] = arr.Length > 4 ? arr[4] : "0";
                    _iconData.iconOpen = true;
                    _iconData.action = (GameObject o) => {
                        if (MapModel.getInstance().curLevelId > 0)
                        {
                            flytxt.instance.fly(ContMgr.getCont("infb_null"));
                            return;
                        }
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_LOTTERY);
                    };
                    iconAction.Add(_iconData);
                    if (str == null) isOn = false;
                    else isOn = true;
                    UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.ON_GETLOTTERYDRAWINFO, this, data));
                    str = arr[0] + ContMgr.getCont("broadcasting2_vip") + dta.item_name;
                    Variant v = new Variant { ["tp"] = 6, ["msg"] = str, ["custom"] = true };
                    List<Variant> listMsgEdit = new List<Variant>();
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = arr[0], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = ContMgr.getCont("broadcasting2_vip") });
                    Color equipColor;
                    if (dta.quality == 4) equipColor = new Color(0xFF, 0x00, 0xFF);
                    else if (dta.quality == 5) equipColor = new Color(0xFF, 0x7F, 0x0A);
                    else equipColor = new Color(0x00, 0x00, 0x00);
                    listMsgEdit.Add(new Variant { ["tp"] = 6, ["msg"] = dta.item_name, ["r"] = equipColor.r, ["g"] = equipColor.g, ["b"] = equipColor.b });

                }
            }

            #endregion
        }

        public void addMsg(string msg)
        {
            BroadcastingData _data = new BroadcastingData();
            _data.msg = msg;
            system_msg.Add(_data);
            iconActionData _iconData = new iconActionData();
            _iconData.iconOpen = false;
            _iconData.action = null;
            iconAction.Add(_iconData);
        }

        public void addGonggaoMsg(string msg)
        {
            BroadcastingData _data = new BroadcastingData();
            _data.msg = msg;
            system_gonggao_msg.Add(_data);
        }

        public void on_off(bool isOn)
        {
            if (isOn)
            {
            }
            else
            {
                openBtn.SetActive(false);
                talkList.Clear();
            }
        }

        void onClickOpen(GameObject go)
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CHATROOM);//CHATROOM
            num = 0;
            showMark();
        }

        void showMark()//显示聊天条数
        {
            if (num == 0)
            {
                numMark.transform.parent.gameObject.SetActive(false);
                return;
            }
            else
            {
                numMark.text = num.ToString();
                numMark.transform.parent.gameObject.SetActive(true);

                InterfaceMgr.setUntouchable(numMark.transform.parent.gameObject);
                numMark.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            if (num > 99)
            {
                num = 99;
            }
        }

        void Update()
        {
            if (mask_sys == null) return;

            if (talkList.Count != 0 && _data == null)
            {
                _data = talkList[0];
            }
            if (_data != null && !isBuilded)
            {
                bg.sizeDelta = new Vector2(bg.sizeDelta.x, 40);
                currentText = GameObject.Instantiate(Text) as GameObject;
                currentText.transform.SetParent(bg);
                currentText.transform.localScale = Vector3.one;
                currentText.SetActive(true);
                currentText.GetComponent<Text>().text = _data.name + ":" + KeyWord.filter(_data.msg);
                currentRect = currentText.GetComponent<RectTransform>();
                currentRect.anchoredPosition = new Vector3(10f, 31f, 0f);
                isBuilded = true;
            }

            if (currentText != null)
            {
                if (hight > 0)
                {
                    hight -= Time.deltaTime * 31;
                    currentRect.anchoredPosition = new Vector3(10, currentRect.anchoredPosition.y - Time.deltaTime * 31, 0);
                }
                else
                {
                    if (talkList.Count != 0)
                    {
                        talkList.Remove(talkList[0]);
                    }
                    _data = null;
                    pastText = currentText;
                    pastRect = currentRect;
                    currentText = null;
                    currentRect = null;
                    hight = 31;
                    isBuilded = false;
                }
            }

            if (pastText != null)
            {
                if (talkList.Count == 0)
                {
                    timer += Time.deltaTime;
                    if (timer >= 3)
                    {
                        bg.sizeDelta = new Vector2(bg.sizeDelta.x, bg.sizeDelta.y - Time.deltaTime * 46);
                        if (bg.sizeDelta.y <= 0)
                        {
                            pastText.SetActive(false);
                        }
                        if (bg.sizeDelta.y <= 0)
                        {
                            GameObject.Destroy(pastText);
                            pastText = null;
                            timer = 0;
                        }
                    }
                }
                else
                {
                    timer = 0;
                    pastRect.anchoredPosition = new Vector3(10, pastRect.anchoredPosition.y - Time.deltaTime * 46, 0);
                    if (pastRect.anchoredPosition.y <= -20)
                    {
                        GameObject.Destroy(pastText);
                        pastText = null;
                    }
                }
            }
            //
            if (system_msg.Count != 0 && canGetNext)
            {
                if (!mask.activeSelf)
                    mask.SetActive(true);
                GameObject go = Instantiate(child);
                go.transform.FindChild("msg_system/Text").GetComponent<Text>().text = system_msg[0].msg;
                go.transform.SetParent(getTransformByPath("mask"));
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                if (iconAction.Count > 0 && iconAction[0].iconOpen)
                {
                    go.transform.FindChild("icon").gameObject.SetActive(true);
                    new BaseButton(go.transform.FindChild("icon")).onClick = iconAction[0].action;
                }
                else
                    go.transform.FindChild("icon").gameObject.SetActive(false);
                canGetNext = false;
                Destroy(go, 4f);
                StartCoroutine(waitShowNextChild());
            }
            if (system_msg.Count <= 0 && mask.activeSelf)
            {
                mask.SetActive(false);
            }
            if (system_gonggao_msg.Count > 0 && canGetNext_Sys)
            {
                if (!mask_sys.activeSelf)
                    mask_sys.SetActive(true);

                mask_sys_txt.text = system_gonggao_msg[0].msg;

                canGetNext_Sys = false;
                StartCoroutine(waitShowNextChild_Sys());
            }
            if (system_gonggao_msg.Count <= 0 && mask_sys.activeSelf)
            {
                mask_sys.SetActive(false);
            }
            //
            if (OutTimer > 0)
            {
                OutTimer -= Time.deltaTime;
            }

        }
        IEnumerator waitShowNextChild()
        {
            yield return new WaitForSeconds(4f);
            system_msg.Remove(system_msg[0]);
            if (iconAction.Count > 0)
                iconAction.Remove(iconAction[0]);
            canGetNext = true;
        }

        IEnumerator waitShowNextChild_Sys()
        {
            yield return new WaitForSeconds(6f);
            system_gonggao_msg.Remove(system_gonggao_msg[0]);

            canGetNext_Sys = true;
        }
    }


    class BroadcastingData
    {
        public string name;
        public string msg;
        public int id;
        public int count;
        public uint cid;
    }
    class iconActionData
    {
        public bool iconOpen;
        public Action<GameObject> action;
    }

}
