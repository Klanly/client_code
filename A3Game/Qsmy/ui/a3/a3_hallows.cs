using Cross;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_hallows : Window
    {
        //所定义的id是他的位置1-9=ware_type
        List<GameObject> lst_avatars = new List<GameObject>();
        List<Vector3> avatars_old_position = new List<Vector3>();
        GameObject camera_prefab;

        BaseButton close_h,
                   open_h_bag,
                   close_h_bag,
                   devour_h,
                   decompose_h,
                   attribute_btn,
                   info_btn,
                   toallwear_btn;

        GameObject obj_h_bag,
                   obj_have_panel,
                   obj_nohave_panel,
                   obj_h_attribute,
                   obj_h_infos,

                   scrollview_h_name,
                   contain_h_name,

                   scrollview_h_bag,
                   contain_h_bag,
                   grid_h_bag;
        Text mySoulNum,
             mySoulNumAndNeednum,
             mySoulNum_bag;


        List<GameObject> lsts_nine_region = new List<GameObject>();
        List<GameObject> lsts_attributes = new List<GameObject>();
        public uint OldType_tpid = 0;    //换装之前的位置上的装备的tpid
        public bool PutOrDown = false;   //false脱下，true穿上
        public bool AllCompose = false;  //true群分解
        public uint this_tpid = 0;           //分解是选择的item
        public int now_id = 1;           //当前选择的位置
        public static a3_hallows instance;

        ScrollControler scrollControer0;
        GameObject helpcon;
        public override void init()
        {
            scrollControer0 = new ScrollControler();
            scrollControer0.create(getComponentByPath<ScrollRect>("left_bg/ScrollView"));
            inText();
            obj_h_bag = getGameObjectByPath("hallows_bag");
            scrollview_h_name = getGameObjectByPath("left_bg/ScrollView");
            contain_h_name = getGameObjectByPath("left_bg/ScrollView/contain");
            scrollview_h_bag = getGameObjectByPath("hallows_bag/hallows_bg/scrollview");
            contain_h_bag = getGameObjectByPath("hallows_bag/hallows_bg/scrollview/contain");
            grid_h_bag = getGameObjectByPath("hallows_bag/hallows_bg/scrollview/grid");
            obj_h_attribute = getGameObjectByPath("right_bg/have/panels/attribute");
            obj_h_infos = getGameObjectByPath("right_bg/have/panels/info");
            obj_have_panel = getGameObjectByPath("right_bg/have");
            obj_nohave_panel = getGameObjectByPath("right_bg/nohave");

            mySoulNum = getComponentByPath<Text>("right_bg/have/devour_btn/num");
            mySoulNumAndNeednum = getComponentByPath<Text>("right_bg/have/panels/exp/num");
            mySoulNum_bag = getComponentByPath<Text>("hallows_bag/hallows_bg/num_sh");
            helpcon = this.transform.FindChild("help").gameObject;
            new BaseButton(this.transform.FindChild("help_btn")).onClick = (GameObject go) =>
            {
                helpcon.SetActive(true);
                ShoworHideModel(false);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) =>
            {
                helpcon.SetActive(false);
                ShoworHideModel(true);
            };
            close_h = new BaseButton(getTransformByPath("close"));
            open_h_bag = new BaseButton(getTransformByPath("openbag_btn"));
            close_h_bag = new BaseButton(getTransformByPath("hallows_bag/hallows_bg/close_bag"));
            devour_h = new BaseButton(getTransformByPath("right_bg/have/devour_btn"));
            decompose_h = new BaseButton(getTransformByPath("hallows_bag/hallows_bg/decompose_btn"));
            toallwear_btn = new BaseButton(getTransformByPath("hallows_bag/hallows_bg/toallwear_btn"));
            attribute_btn = new BaseButton(getTransformByPath("right_bg/have/btns/attribute_btn"));
            info_btn = new BaseButton(getTransformByPath("right_bg/have/btns/info_btn"));
            close_h.onClick = open_h_bag.onClick = close_h_bag.onClick = devour_h.onClick = decompose_h.onClick = attribute_btn.onClick = info_btn.onClick = toallwear_btn.onClick = btnsOnClick;

            initsomething();

        }

        void inText()
        {
            this.transform.FindChild("right_bg/have/btns/attribute_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_1");//属性
            this.transform.FindChild("right_bg/have/btns/info_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_2");//信息
            this.transform.FindChild("right_bg/have/devour_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_3");//吞噬圣魂
            this.transform.FindChild("right_bg/nohave/attribute/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_4");//未装备圣器
            this.transform.FindChild("hallows_bag/name").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_5");//圣器背包
            this.transform.FindChild("hallows_bag/hallows_bg/num_sh (1)").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_6");//圣魂X
            this.transform.FindChild("hallows_bag/hallows_bg/decompose_btn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_7");//分解多余圣器
            this.transform.FindChild("help/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_8");//help提示
            this.transform.FindChild("help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_hallows_9");//知道了

        }
        public override void onShowed()
        {
            instance = this;
            helpcon.SetActive(false);
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(false);

            scrollview_h_name.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            divine_soul_nums();

            createAvatar();
            showAvatar(1);//默认第一个
            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public override void onClosed()
        {
            if (GRMap.GAME_CAMERA != null)
                GRMap.GAME_CAMERA.SetActive(true);
            disposeAvatar();
            model = null;
        }
        void createAvatar()
        {
            //Vector3 pos = new Vector3(-152.89f, 1.652f, -1.472f);
            //Vector3 roa = new Vector3(-71.72f, 90, 0);
            Vector3 pos = Vector3.zero;
            Vector3 roa = Vector3.zero;
            for (int i = 1; i < 10; i++)
            {

                string position = XMLMgr.instance.GetSXML("holicware.holic", "id==" + i).getString("pos");
                string rotation = XMLMgr.instance.GetSXML("holicware.holic", "id==" + i).getString("roa");
                string[] s_pos = position.Split(',');
                string[] s_rot = rotation.Split(',');
                GameObject avater = GAMEAPI.ABModel_LoadNow_GameObject("Item_sq_0" + i);
                avater.SetActive(false);
                pos = new Vector3(float.Parse(s_pos[0]), float.Parse(s_pos[1]), float.Parse(s_pos[2]));
                roa = new Vector3(float.Parse(s_rot[0]), float.Parse(s_rot[1]), float.Parse(s_rot[2]));
                //switch (i)
                //{
                //    case 1:
                //        break;
                //    case 2:
                //        break;
                //    case 3:
                //        pos = new Vector3(-152.681f, 1.6359f, 0.51f);
                //        roa = new Vector3(-107.394f, 270, -90);
                //        break;
                //    case 4:
                //        pos = new Vector3(-152.91f, 1.6f, -1.3f);
                //        roa = new Vector3(-90.3f, -6.5f, 85.8f);
                //        break;
                //    case 5:
                //        pos = new Vector3(-152.8f, 1.56f, -0.4f);
                //        roa = new Vector3(-80.45f, 90, 0);
                //        break;
                //    case 6:
                //        pos = new Vector3(-152.78f, 1.4f, 0.9f);
                //        roa = new Vector3(-71.242f, 90, -90);
                //        break;
                //    case 7:
                //        pos = new Vector3(-153f, 1.652f, -1.4f);
                //        roa = new Vector3(90f, -90, 0);
                //        break;
                //    case 8:
                //        pos = new Vector3(-152.74f, 1.45f, 0.31f);
                //        break;
                //    case 9:
                //        pos = new Vector3(-152.89f, 1.45f, 1.1f);
                //        break;
                //}

                avatars_old_position.Add(pos);
                GameObject avater_prefab = GameObject.Instantiate(avater, pos, Quaternion.Euler(roa)) as GameObject;
                lst_avatars.Add(avater_prefab);
                avater_prefab.layer = EnumLayer.LM_FX;
            }
            GameObject camera = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            camera_prefab = GameObject.Instantiate(camera) as GameObject;

        }
        void disposeAvatar()
        {
            if (lst_avatars != null)
            {
                for (int i = 0; i < lst_avatars.Count; i++)
                {
                    Destroy(lst_avatars[i]);
                }
                lst_avatars.Clear();
            }
            if (camera_prefab != null) Destroy(camera_prefab);

        }




        //刷新模型
        void showAvatar(int id, int quality = 0)
        {
            now_id = id;
            for (int j = 0; j < lsts_nine_region.Count; j++)
            {
                lst_avatars[j].SetActive(false);
                lsts_nine_region[j].transform.FindChild("icon/this").gameObject.SetActive(false);
            }
            lsts_nine_region[id - 1].transform.FindChild("icon/this").gameObject.SetActive(true);

            if (haveOrnoHalllow(id))
            {
                lst_avatars[id - 1].SetActive(true);
                model = lst_avatars[id - 1];
                model_old_pos = avatars_old_position[id - 1];
                show_infos(id, A3_HallowsModel.getInstance().now_hallows()[id]);
                show_attribute(id, A3_HallowsModel.getInstance().now_hallows()[id]);
                divine_soul_nums(A3_HallowsModel.getInstance().now_hallows()[id].lvl, A3_HallowsModel.getInstance().now_hallows()[id].exp);
            }
            else
            {
                model = null;
                // model_old_pos = Vector3.zero;
            }
            obj_have_panel.SetActive(haveOrnoHalllow(id) ? true : false);
            obj_nohave_panel.SetActive(haveOrnoHalllow(id) ? false : true);

        }
        //刷新信息
        void show_infos(int id, hallowsData data)
        {
            Dictionary<int, hallowsData> dic = A3_HallowsModel.getInstance().now_hallows();

            Text nameandlv = getComponentByPath<Text>("right_bg/have/panels/info/nameandlv");
            nameandlv.text = a3_BagModel.getInstance().getItemDataById((uint)data.item_id).item_name + "LV:" + data.lvl;


            GameObject iconf = getGameObjectByPath("right_bg/have/panels/info/icon/icon");
            iconf.GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_holic_skill_" + data.h_s_d.skill_id);
            //GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint)data.h_s_d.skill_id);
            //icon.transform.SetParent(iconf.transform, false);


            Text skill_name = getComponentByPath<Text>("right_bg/have/panels/info/skill_name");
            skill_name.text = data.h_s_d.skill_name;

            Text des = getComponentByPath<Text>("right_bg/have/panels/info/skill_des");
            des.text = data.h_s_d.skill_des;
        }
        //刷新属性
        void show_attribute(int id, hallowsData data)
        {

            for (int i = 0; i < lsts_attributes.Count; i++)
            {
                int attribute_name = A3_HallowsModel.getInstance().GetAttributeForQuality(data.h_s_d.quality)[i].Keys.ElementAt(0);//字典key的下标
                float attribute_value = A3_HallowsModel.getInstance().GetAttributeForQuality(data.h_s_d.quality)[i][attribute_name];
                lsts_attributes[i].GetComponent<Text>().text = Globle.getAttrAddById(attribute_name, (int)((attribute_value + attribute_value * data.lvl * 0.1f)) + 1);
            }
        }
        //刷新圣魂数量       
        void divine_soul_nums()
        {
            string num = A3_HallowsModel.getInstance().GetSoulNum();
            mySoulNum.text = num;
            mySoulNum_bag.text = num;
            if (int.Parse(num) <= 0)
                devour_h.gameObject.GetComponent<Button>().interactable = false;
            else
                devour_h.gameObject.GetComponent<Button>().interactable = true;


        }/*(背包界面，吞噬圣魂按钮)*/
        void divine_soul_nums(int lvl, int exp)
        {
            int xx = XMLMgr.instance.GetSXML("holicware.lvlup").getInt("basic_exp");
            int yy = XMLMgr.instance.GetSXML("holicware.lvlup").getInt("add_exp");
            //string num = A3_HallowsModel.getInstance().GetSoulNum();
            mySoulNumAndNeednum.text = lvl == 999 ? "max" : exp + "/" + (xx + lvl * yy);
            float x = exp / (float)(xx + lvl * yy);
            // print("百分比：" + x);
            getComponentByPath<Transform>("right_bg/have/panels/exp/exp").localScale = lvl == 999 ? new Vector3(1, 0.9f, 1) : new Vector3(x, 0.9f, 1);


        } /*(信息界面)*/


        //装备后刷新
        public void PutHallows(int id, uint tpid)
        {

            DownHallows(id);
            creatrveicon(id);
            showAvatar(id);
            refreshHallowsbag(tpid);
            ShoworHideModel(false);
        }
        //卸下后刷新
        public void DownHallows(int id)
        {
            if (lsts_nine_region[id - 1].transform.FindChild("icon/icon").childCount > 0)
            {
                Destroy(lsts_nine_region[id - 1].transform.FindChild("icon/icon").GetChild(0).gameObject);
                lsts_nine_region[id - 1].transform.FindChild("name").GetComponent<Text>().text = "";
            }

            showAvatar(id);
        }
        //升级后刷新
        public void UpgradeHallows(int id, hallowsData data)
        {
            Text nameandlv = getComponentByPath<Text>("right_bg/have/panels/info/nameandlv");
            nameandlv.text = a3_BagModel.getInstance().getItemDataById((uint)data.item_id).item_name + "LV:" + data.lvl;
            show_attribute(id, data);
            divine_soul_nums();
            divine_soul_nums(data.lvl, data.exp);
        }
        //分解后刷新     
        public void DecomposeHallows(uint tpid)
        {
            refreshHallowsbag(tpid);
            divine_soul_nums();

        } /*单分解*/
        public void DecomposeHallows()
        {
            closeHaveHallows();
            showHaveHallows();
            divine_soul_nums();

        } /*群分解*/


        #region 圣器背包
        Dictionary<uint, GameObject> dic_havehallows = new Dictionary<uint, GameObject>();
        void showHaveHallows()
        {
            scrollview_h_bag.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            Dictionary<uint, a3_BagItemData> dic = a3_BagModel.getInstance().getHallows();
            if (dic.Count <= 0)
            {
                decompose_h.gameObject.GetComponent<Button>().interactable = false;
                return;
            }
            decompose_h.gameObject.GetComponent<Button>().interactable = true;
            int i = 0;
            foreach (uint id in dic.Keys)
            {
                creatrveicon_bag(i, dic[id]);
                i++;
            }
        }
        //显示tips
        void itemOnclick(GameObject go, a3_BagItemData one, int type)
        {

            ArrayList data = new ArrayList();
            if (type == 2)
                data.Add(a3_BagModel.getInstance().getHallows()[one.id]);
            else
                data.Add(one);
            data.Add(equip_tip_type.hallowtips);
            data.Add(type);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
        }
        void closeHaveHallows()
        {
            lst_bag.Clear();
            lst_bid.Clear();
            l.Clear();
            foreach (GameObject go in dic_havehallows.Values)
            {
                Destroy(go);
            }
            dic_havehallows.Clear();
        }
        //刷新圣器背包
        void refreshHallowsbag(uint tpid)
        {
            Dictionary<uint, a3_BagItemData> dic = a3_BagModel.getInstance().getHallows();
            if (a3_BagModel.getInstance().hasItem(tpid))
            {
                int num = a3_BagModel.getInstance().getItemNumByTpid(tpid);
                dic_havehallows[tpid].transform.FindChild("num").GetComponent<Text>().text = num.ToString();
            }
            else
            {
                dic_havehallows[tpid].transform.parent.transform.SetAsLastSibling();
                Destroy(dic_havehallows[tpid]);
                dic_havehallows.Remove(tpid);
            }
            if (OldType_tpid != 0)
            {
                int num = a3_BagModel.getInstance().getItemNumByTpid(OldType_tpid);
                if (num > 1)
                {
                    dic_havehallows[OldType_tpid].transform.FindChild("num").GetComponent<Text>().text = num.ToString();
                }
                else if (num == 1)
                {
                    foreach (a3_BagItemData one in dic.Values)
                    {
                        if (one.tpid == OldType_tpid)
                        {
                            creatrveicon_bag(dic_havehallows.Count, one);
                        }
                    }
                }
                else
                {
                    //bug;
                }
                OldType_tpid = 0;
            }

        }
        //背包里要分解的圣器
        void composeallhallows()
        {



            Dictionary<uint, a3_BagItemData> dic_bag = a3_BagModel.getInstance().getHallows();
            Dictionary<int, hallowsData> dic_self = A3_HallowsModel.getInstance().now_hallows();

            if (dic_bag.Count <= 0)
            {
                return;
            }

            for (int i = 1; i <= dic_self.Keys.Count; i++)
            {
                if (dic_self[i].item_id != 0)
                {
                    //有穿
                    compose_halloews(i, true);
                }
                else
                {
                    compose_halloews(i, false);
                    //身上没穿
                }
            }

        }
        List<Variant> lst_bag = new List<Variant>();
        List<a3_BagItemData> lst_bid = new List<a3_BagItemData>();
        void compose_halloews(int id, bool havehallows)
        {
            Dictionary<uint, a3_BagItemData> dic_bag = a3_BagModel.getInstance().getHallows();
            Dictionary<int, hallowsData> dic_self = A3_HallowsModel.getInstance().now_hallows();

            foreach (a3_BagItemData data in dic_bag.Values)
            {
               
                if (A3_HallowsModel.getInstance().GetTypeByItemid((int)data.confdata.tpid) == id)
                {
                    if (havehallows)
                    {
                        if (data.confdata.quality <= dic_self[id].h_s_d.quality)
                        {
                            Variant hci = new Variant();
                            hci["item_id"] = data.tpid;
                            hci["item_num"] = data.num;
                            lst_bag.Add(hci);

                        }
                        else
                        {
                            lst_bid.Add(data);
                        }
                       
                    }
                    else
                    {

                        lst_bid.Add(data);
                      
                    }

                }
            }
            if (lst_bid.Count > 0)
            {
                //lst_bid.OrderBy(c=>c.confdata.quality);
                lst_bid.Sort((a, b) => a.confdata.quality.CompareTo(b.confdata.quality));


                Lst_bag(lst_bid);
            }


        }
        void Lst_bag(List<a3_BagItemData> lst_bid)
        {
            //lst_bid.OrderBy(c => c.confdata.quality);
            for (int i = 0; i < lst_bid.Count; i++)
            {
                // print("i:" + lst_bid[i].id+"item_id:" + lst_bid[i].tpid+ "num:"+lst_bid[i].num+"\n");
                if (i < lst_bid.Count - 1)
                {
                    Variant hci = new Variant();
                    hci["item_id"] = lst_bid[i].tpid;
                    hci["item_num"] = lst_bid[i].num;
                    lst_bag.Add(hci);
                }
                else
                {
                    if (lst_bid[i].num > 1)
                    {
                        Variant hci = new Variant();
                        hci["item_id"] = lst_bid[i].tpid;
                        hci["item_num"] = lst_bid[i].num - 1;
                        lst_bag.Add(hci);

                    }
                    else
                    {
                        lst_bid.Clear();
                        return;
                    }
                }

            }
            lst_bid.Clear();
        }

        #endregion

        //一件穿搭

        List<a3_BagItemData> l = new List<a3_BagItemData>();
        void toallwear()
        {
            l.Clear();
            Dictionary<uint, a3_BagItemData> dic_bag = a3_BagModel.getInstance().getHallows();
            foreach (a3_BagItemData data in dic_bag.Values)
            {
                l.Add(data);
            }


            if (l.Count <= 0)
                return;

            l.Sort((a, b) => a.confdata.quality.CompareTo(b.confdata.quality));

            l.Sort((a, b) => a.confdata.tpid.CompareTo(b.confdata.tpid));

            int nowtype = 1;
            for (int i = 0; i < l.Count; i++)
            {
                int qua = l[i].confdata.quality;
                uint tpid = l[i].tpid;
                int type = A3_HallowsModel.getInstance().GetTypeByItemid((int)tpid);

                print("位置：" + type + "id是：" + tpid + "品质是:" + qua);

                if (i < l.Count - 1)
                {
                    if (A3_HallowsModel.getInstance().GetTypeByItemid((int)l[i + 1].tpid) == nowtype)
                    {
                        continue;
                    }
                    else
                    {
                        onecompare(type, l[i].tpid,qua);
                        nowtype = A3_HallowsModel.getInstance().GetTypeByItemid((int)l[i + 1].tpid);
                    }
                }
                else
                {
                    onecompare(type, l[i].tpid,qua);

                }

            }
            if(!ischange)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_hallowallwear"));
            }
            ischange = false;
        }
        bool ischange = false;
        void onecompare(int type, uint tpid,int qua)
        {
           
           // Debug.LogError("我当前的位置是：" + type + "我的id是：" + tpid);
            Dictionary<int, hallowsData> dic_selfs = A3_HallowsModel.getInstance().now_hallows();

           
            if (dic_selfs != null)
            {

                if (dic_selfs.ContainsKey(type))
                {

                    if(dic_selfs[type].item_id==0)
                    {
                        PutOrDown = true;
                        A3_HallowsProxy.getInstance().SendHallowsProxy(4, type, (int)tpid);
                        ischange = true;
                    }
                    else
                    {
                        if(dic_selfs[type].h_s_d.quality<qua)
                        {
                            PutOrDown = true;
                            A3_HallowsProxy.getInstance().SendHallowsProxy(4, type, (int)tpid);
                            ischange = true;
                        }

                    }
                }

            }
            



        }

        #region init
        void initsomething()
        {
            lsts_nine_region.Clear();
            lsts_attributes.Clear();
            for (int i = 0; i < 9; i++)
            {
                int j = i;
                BaseButton bt = new BaseButton(getTransformByPath("left_bg/ScrollView/contain/" + i));
                bt.onClick = (GameObject go) => { showAvatar(j + 1); };
                lsts_nine_region.Add(bt.gameObject);
            }
            for (int j = 0; j < obj_h_attribute.transform.FindChild("Panel").transform.childCount; j++)
            {
                lsts_attributes.Add(obj_h_attribute.transform.FindChild("Panel").GetChild(j).gameObject);
            }
            init_bag_grids();
            init_region();
        }
        void init_region()
        {
            Dictionary<int, hallowsData> dic = A3_HallowsModel.getInstance().now_hallows();
            if (dic == null)
                return;

            foreach (int i in dic.Keys)
            {
                if (dic[i].item_id == 0)
                    continue;
                else
                    creatrveicon(dic[i].id);
            }
        }
        void init_bag_grids()
        {
            //45 个格子
            for (int i = 0; i < 45; i++)
            {
                GameObject iconClone = Instantiate(grid_h_bag) as GameObject;
                iconClone.SetActive(true);
                iconClone.name = i.ToString();
                iconClone.transform.SetParent(contain_h_bag.transform, false);
            }
        }

        #endregion


        //9个位置icon的创建
        void creatrveicon(int id)
        {
            Dictionary<int, hallowsData> dic = A3_HallowsModel.getInstance().now_hallows();

            GameObject iconf = lsts_nine_region[id - 1].transform.FindChild("icon/icon").gameObject;
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint)dic[id].item_id, true, -1, 1, true);
            icon.transform.SetParent(iconf.transform, false);

            Text name = lsts_nine_region[id - 1].transform.FindChild("name").GetComponent<Text>();
            string item_name = XMLMgr.instance.GetSXML("item.item", "id==" + (uint)dic[id].item_id).getString("item_name");
            name.text = Globle.getColorStrByQuality(item_name, dic[id].h_s_d.quality);

            a3_BagItemData data = new a3_BagItemData();
            data.tpid = (uint)dic[id].item_id;
            data.num = 1;
            a3_ItemData da = new a3_ItemData();
            da = a3_BagModel.getInstance().getItemDataById(data.tpid);
            data.confdata = da;
            data.ishallows = true;
            new BaseButton(icon.transform).onClick = delegate (GameObject go) { this.itemOnclick(icon, data, 3); ShoworHideModel(false); };
        }
        //背包里面icon的创建
        void creatrveicon_bag(int i, a3_BagItemData data)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
            dic_havehallows[data.confdata.tpid] = icon;
            icon.transform.SetParent(contain_h_bag.transform.GetChild(i).transform, false);
            new BaseButton(icon.transform).onClick = delegate (GameObject go) { this.itemOnclick(icon, data, 2); ShoworHideModel(false); };
        }




        //btns
        void btnsOnClick(GameObject go)
        {
            switch (go.name)
            {

                case "close"://主界面关闭
                    InterfaceMgr.getInstance().close(InterfaceMgr.A3_HALLOWS);
                    obj_h_bag.SetActive(false);
                    break;
                case "openbag_btn"://打开圣器宝箱
                    ShoworHideModel(false);
                    obj_h_bag.SetActive(true);
                    showHaveHallows();
                    composeallhallows();
                    break;
                case "close_bag"://关闭圣器宝箱
                    ShoworHideModel(true);
                    obj_h_bag.SetActive(false);
                    closeHaveHallows();
                    break;
                case "devour_btn"://吞噬圣魂
                    A3_HallowsProxy.getInstance().SendHallowsProxy(2, now_id);
                    break;
                case "decompose_btn"://分解多余圣器

                    if (lst_bag.Count <= 0)
                        return;
                    else
                    {
                        AllCompose = true;
                        A3_HallowsProxy.getInstance().SendHallowsProxy(3, -1, -1, lst_bag);
                    }
                    break;
                case "attribute_btn"://打开属性界面
                    go.transform.FindChild("this").gameObject.SetActive(true);
                    info_btn.transform.FindChild("this").gameObject.SetActive(false);
                    obj_h_attribute.SetActive(true);
                    obj_h_infos.SetActive(false);
                    break;
                case "info_btn"://打开信息界面
                    go.transform.FindChild("this").gameObject.SetActive(true);
                    attribute_btn.transform.FindChild("this").gameObject.SetActive(false);
                    obj_h_attribute.SetActive(false);
                    obj_h_infos.SetActive(true);
                    break;
                case "toallwear_btn":
                    toallwear();
                    break;
            }

        }
        //现在的身上是否有穿戴
        public bool haveOrnoHalllow(int id)
        {
            if (A3_HallowsModel.getInstance().now_hallows() != null && A3_HallowsModel.getInstance().now_hallows().ContainsKey(id) && A3_HallowsModel.getInstance().now_hallows()[id].item_id != 0)
                return true;
            else
                return false;
        }








        public void ShoworHideModel(bool show)
        {
            if (model != null)
                model.SetActive(show);
        }

        Vector3 model_old_pos;/*= Vector3.zero;*/
        GameObject model = null;
        float a = 0;
        float b = 0.02f;
        float c = 0.1f;
        void Update()
        {
            if (model != null)
            {
                a += b;
                float f = Mathf.Sin(a) * c;
                model.transform.position = model_old_pos + new Vector3(0, f, 0);
            }
        }
    }

      class hall
        {
           public int type;
        public int tpid;
        public int qua; 
        }
}

