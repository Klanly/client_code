//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameFramework;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using MuGame;
//using Cross;
//namespace MuGame
//{
//    class a3_equipsell : Window
//    {
//        GameObject quick_image;
//        bool isquicklyChoose = false;
//        GameObject contain_left;
//        GameObject contain;
//        GameObject grid;
//        GameObject prompt;

//        Toggle white;
//        Toggle green;
//        Toggle blue;
//        Toggle purple;

//        Dictionary<uint, a3_BagItemData> dic_right = new Dictionary<uint, a3_BagItemData>();
//        Dictionary<uint, GameObject> dic_rightObj = new Dictionary<uint, GameObject>();
//        Dictionary<uint, a3_BagItemData> dic_left = new Dictionary<uint, a3_BagItemData>();
//        Dictionary<uint, GameObject> dic_leftObj = new Dictionary<uint, GameObject>();

//        Dictionary<uint, a3_BagItemData> dic_change = new Dictionary<uint, a3_BagItemData>();

//        Text mojing;
//        Text shengguanghuiji;
//        Text mifageli;
//        int mojing_num;
//        int shengguanghuiji_num;
//        int mifageli_num;

//        ScrollControler scrollControer;
//        ScrollControler scrollcontroers;
//        static public a3_equipsell _instance;
//        public override void init()
//        {
//            _instance = this;
//            scrollControer = new ScrollControler();
//            ScrollRect scroll = transform.FindChild("panel_right/bg/Scroll_rect").GetComponent<ScrollRect>();
//            scrollControer.create(scroll);
//            scrollcontroers = new ScrollControler();
//            ScrollRect scrolls = transform.FindChild("panel_left/bg/Scrollrect").GetComponent<ScrollRect>();
//            scrollcontroers.create(scrolls);
//            contain = transform.FindChild("panel_right/bg/Scroll_rect/contain").gameObject;
//            grid = transform.FindChild("panel_right/bg/Scroll_rect/grid").gameObject;
//            contain_left = transform.FindChild("panel_left/bg/Scrollrect/contain").gameObject;
//            white = getComponentByPath<Toggle>("panel_left/panel/Toggle_white");
//            mojing = getComponentByPath<Text>("panel_left/panel/mojing/num");
//            shengguanghuiji = getComponentByPath<Text>("panel_left/panel/shenguang/num");
//            mifageli = getComponentByPath<Text>("panel_left/panel/mifa/num");
//            prompt=getGameObjectByPath("prompt");
//            quick_image = getGameObjectByPath("panel_right/bg/Image");
//            white.onValueChanged.AddListener(delegate(bool ison)
//            {
//                if (ison)
//                    EquipsSureSell(0,1);
//                else
//                    EquipsNoSell(0,1);
//            });
//            green = getComponentByPath<Toggle>("panel_left/panel/Toggle_green");
//            green.onValueChanged.AddListener(delegate(bool ison)
//            {
//                if (ison)
//                    EquipsSureSell(0,2);
//                else
//                    EquipsNoSell(0,2);

//            });
//            blue = getComponentByPath<Toggle>("panel_left/panel/Toggle_blue");
//            blue.onValueChanged.AddListener(delegate(bool ison)
//            {
//                if (ison)
//                    EquipsSureSell(0,3);
//                else
//                    EquipsNoSell(0,3);

//            });
//            purple = getComponentByPath<Toggle>("panel_left/panel/Toggle_puple");
//            purple.onValueChanged.AddListener(delegate(bool ison)
//            {
//                if (ison)
//                    EquipsSureSell(0,4);
//                else
//                    EquipsNoSell(0,4);

//            });

//            BaseButton btn_close = new BaseButton(transform.FindChild("btn_close"));
//            btn_close.onClick = onclose;

//            BaseButton btn_quicklychoose= new BaseButton(transform.FindChild("panel_right/Button"));
//            btn_quicklychoose.onClick = onQuicklyChoose;

//            BaseButton btn_decompose = new BaseButton(transform.FindChild("panel_left/Button"));
//            btn_decompose.onClick = onDecompose;


//            BaseButton prompt_sure = new BaseButton(prompt.transform.FindChild("Button"));
//            prompt_sure.onClick = Sendproxy;

//            BaseButton prompt_close = new BaseButton(prompt.transform.FindChild("btn_close"));
//            prompt_close.onClick = delegate(GameObject go)
//            {
//                prompt.SetActive(false);
//            };
//        }
//        public override void onShowed()
//        {

//            showInfos();
//            CreateGrids();
//            CreateEquipIcon();
//        }
//        public override void onClosed()
//        {
//            DestroyGrids();
//            removeAllinfos();
//        }

//        void CreateGrids()
//        {
//            int num = a3_BagModel.getInstance().getUnEquips().Keys.Count;
//            if (num <= 30)
//                AddGrids(30);
//            else
//            {
//                int nub = 5-(num % 5);
//                num += nub;
//                AddGrids(num);
//            }
//        }
//        void AddGrids(int num)
//        {
//            for (int i = 0; i < num; i++)
//            {
//                GameObject objclone = GameObject.Instantiate(grid) as GameObject;
//                objclone.SetActive(true);
//                objclone.transform.SetParent(contain.transform, false);
//            }
//            float width = contain.GetComponent<RectTransform>().sizeDelta.x;
//            float hight= (float)(num / 5 * grid.GetComponent<RectTransform>().sizeDelta.y + 2.35 * (num / 5 - 1));
//            contain.GetComponent<RectTransform>().sizeDelta = new Vector2(width, hight);
//        }
//        void DestroyGrids()
//        {
//            for (int i = 0; i < contain.transform.childCount; i++)
//            {
//                Destroy(contain.transform.GetChild(i).gameObject);
//            }
//            for (int i = 0; i < contain_left.transform.childCount; i++)
//            {
//                if (contain_left.transform.GetChild(i).childCount > 0)
//                {
//                    Destroy(contain_left.transform.GetChild(i).GetChild(0).gameObject);
//                }
//            }

//        }
//        int right_count = 0;
//        void CreateEquipIcon()
//        {
//            foreach (uint i in a3_BagModel.getInstance().getUnEquips().Keys)
//            {
//                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getUnEquips()[i], true);
//                icon.transform.SetParent(contain.transform.GetChild(right_count).transform, false);
//                icon.name = a3_BagModel.getInstance().getUnEquips()[i].id.ToString();
//                right_count++;
//                BaseButton bs_bt = new BaseButton(icon.transform);
//                bs_bt.onClick = delegate(GameObject go) { this.onEquipClickShowTips(icon, uint.Parse(icon.name), false); };

//                dic_rightObj[a3_BagModel.getInstance().getUnEquips()[i].id] = icon;
//                dic_right[a3_BagModel.getInstance().getUnEquips()[i].id] = a3_BagModel.getInstance().getUnEquips()[i];
//            }
//        }
//        void onEquipClickShowTips(GameObject go, uint id, bool isselltrue)
//        {
//            if (isquicklyChoose)
//            {
//                if (!isselltrue)
//                {
//                    //uint tpid = a3_BagModel.getInstance().getUnEquips()[id].tpid;
//                    //if (a3_BagModel.getInstance().getItemDataById(tpid).job_limit == 1 || a3_BagModel.getInstance().getItemDataById(tpid).job_limit == PlayerModel.getInstance().profession)
//                        EquipsSureSell(id);
//                    //else
//                       // flytxt.instance.fly("非相同职业装备不可分解！", 1);
//                }
//                else
//                {
//                    EquipsNoSell(id);
//                }
//            }
//            else
//            {
//                if (isselltrue)
//                {
//                    ArrayList data = new ArrayList();
//                    data.Add(a3_BagModel.getInstance().getUnEquips()[id]);
//                    data.Add(equip_tip_type.SellOut_tip);
//                    InterfaceMgr.getInstance().open(InterfaceMgr.A3_EQUIPTIP, data);
//                }
//                else
//                {
//                    ArrayList data = new ArrayList();
//                    data.Add(a3_BagModel.getInstance().getUnEquips()[id]);
//                    //uint tpid = a3_BagModel.getInstance().getUnEquips()[id].tpid;
//                    //if (a3_BagModel.getInstance().getItemDataById(tpid).job_limit == 1 || a3_BagModel.getInstance().getItemDataById(tpid).job_limit == PlayerModel.getInstance().profession)
//                        data.Add(equip_tip_type.SellIn_tip);
//                    //else
//                    //    data.Add(equip_tip_type.SellNo_tip);
//                    InterfaceMgr.getInstance().open(InterfaceMgr.A3_EQUIPTIP, data);
//                }
//            }
//        }


//        //放入
//        public void EquipsSureSell(uint id=0,int quality=0)
//        {
//            if (quality == 0)
//            {
//                if (dic_rightObj.ContainsKey(id))
//                {
//                    if (dic_left.Keys.Count < 20)
//                    {
//                        dic_left[id] = a3_BagModel.getInstance().getUnEquips()[id];
//                        dic_change[id] = a3_BagModel.getInstance().getUnEquips()[id];
//                        showItemNum(a3_BagModel.getInstance().getUnEquips()[id].tpid, true);
//                        DestroyObj(id, true, dic_right, dic_rightObj, contain);
//                        ShowObj(dic_left, dic_leftObj, contain_left);
//                    }
//                    else
//                        flytxt.instance.fly(ContMgr.getCont("a3_equipsell_maxgrid"), 1);

//                }
//            }
//            else
//            {
//                if (dic_rightObj.Keys.Count > 0)
//                {
//                    foreach (uint key_id in dic_rightObj.Keys)
//                    {
//                        uint tpid = a3_BagModel.getInstance().getUnEquips()[key_id].tpid;
//                        if (a3_BagModel.getInstance().getItemDataById(tpid).quality == quality)
//                        {
//                            //if (a3_BagModel.getInstance().getItemDataById(tpid).job_limit == 1 || a3_BagModel.getInstance().getItemDataById(tpid).job_limit == PlayerModel.getInstance().profession)
//                            //{
//                                if (dic_left.Keys.Count < 20)
//                                {
//                                    if (dic_leftObj.ContainsKey(key_id))
//                                    {
//                                        Destroy(dic_leftObj[key_id]);
//                                        dic_leftObj.Remove(key_id);
//                                    }
//                                    dic_left[key_id] = a3_BagModel.getInstance().getUnEquips()[key_id];
//                                    dic_change[key_id] = a3_BagModel.getInstance().getUnEquips()[key_id];
//                                    showItemNum(a3_BagModel.getInstance().getUnEquips()[key_id].tpid, true);
//                                    DestroyObj(key_id, false, dic_right, dic_rightObj, contain);
//                                    ShowObj(dic_left, dic_leftObj, contain_left);
//                                }
//                                else
//                                {
//                                    flytxt.instance.fly(ContMgr.getCont("a3_equipsell_maxgrid"), 1);
//                                    break;
//                                }
                              
//                            //}
//                        }
//                    }
//                    Refresh(dic_left, dic_right, dic_rightObj);
//                }

//            }
            
//        }
//        //取出
//        public void EquipsNoSell(uint id = 0, int quality = 0)
//        {
//            if (quality == 0)
//            {
//                if (dic_leftObj.ContainsKey(id))
//                {
//                    dic_right[id] = a3_BagModel.getInstance().getUnEquips()[id];
//                    dic_change[id] = a3_BagModel.getInstance().getUnEquips()[id];
                    
//                    DestroyObj(id, true,dic_left,dic_leftObj,contain_left);
//                    ShowObj(dic_right, dic_rightObj, contain);
//                }                
//            }
//            else
//            {
//                if (dic_leftObj.Keys.Count > 0)
//                {
//                    foreach (uint key_id in dic_leftObj.Keys)
//                    {
//                        uint tpid = a3_BagModel.getInstance().getUnEquips()[key_id].tpid;
//                        if (a3_BagModel.getInstance().getItemDataById(tpid).quality == quality)
//                        {
//                            //if (a3_BagModel.getInstance().getItemDataById(tpid).job_limit == 1 || a3_BagModel.getInstance().getItemDataById(tpid).job_limit == PlayerModel.getInstance().profession)
//                            //{
//                                if (dic_rightObj.ContainsKey(key_id))
//                                {
//                                    Destroy(dic_rightObj[key_id]);
//                                    dic_rightObj.Remove(key_id);
//                                }
//                                dic_right[key_id] = a3_BagModel.getInstance().getUnEquips()[key_id];
//                                dic_change[key_id] = a3_BagModel.getInstance().getUnEquips()[key_id];
                                
//                                DestroyObj(key_id, false, dic_left, dic_leftObj, contain_left);
//                                ShowObj(dic_right, dic_rightObj, contain);
//                          //  }
//                        }
//                    }
//                    Refresh(dic_right, dic_left, dic_leftObj);
//                }               

//            }
            
//        }
//        //点击删除obj
//        void DestroyObj(uint id,bool isOneRemove,Dictionary<uint,a3_BagItemData> dic=null,Dictionary<uint,GameObject> dic_obj=null,GameObject contain=null)
//        {
//            GameObject go = dic_obj[id].transform.parent.gameObject;
//            Destroy(go);
//            if (isOneRemove)
//            {
//                dic_obj.Remove(id);
//                dic.Remove(id);
//            }
//            GameObject objclone = GameObject.Instantiate(grid) as GameObject; 
//            objclone.SetActive(true);
//            objclone.transform.SetParent(contain.transform, false);
//            objclone.transform.SetSiblingIndex(dic_obj.Count + 1);
//            if (dic == dic_left)
//            {
//                showItemNum(a3_BagModel.getInstance().getUnEquips()[id].tpid, false);
//                left_count--;
//            }
//            else
//                right_count--;
//        }
//        //点击添加obj
//        int left_count = 0;
//        void ShowObj(Dictionary<uint,a3_BagItemData> dic,Dictionary<uint,GameObject> dic_obj,GameObject contain)
//        {
//            foreach (uint i in dic_change.Keys)
//            {
//                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(dic[i], true);
//                if (dic == dic_left)
//                {
//                    icon.transform.SetParent(contain.transform.GetChild(left_count).transform, false);
//                    left_count++;
//                }
//                else
//                {
//                    icon.transform.SetParent(contain.transform.GetChild(right_count).transform, false);
//                    right_count++;
//                }
//                icon.name = dic[i].id.ToString();
//                BaseButton bs_bt = new BaseButton(icon.transform);
//                bs_bt.onClick = delegate(GameObject go) 
//                {
//                    if (dic == dic_left)
//                        this.onEquipClickShowTips(icon, uint.Parse(icon.name), true);
//                    else
//                        this.onEquipClickShowTips(icon, uint.Parse(icon.name), false);
                    
//                };
//                dic_obj[i] = icon;          
//            }
//            dic_change.Clear();


//        }
//        //刷新字典信息
//        void Refresh(Dictionary<uint, a3_BagItemData> dic, Dictionary<uint, a3_BagItemData> refresh_dic, Dictionary<uint, GameObject> refresh_dicobj)
//        {
//            foreach (uint ids in dic.Keys)
//            {
//                if (refresh_dicobj.ContainsKey(ids))
//                    refresh_dicobj.Remove(ids);
//            }
//            foreach (uint ids in dic.Keys)
//            {
//                if (refresh_dic.ContainsKey(ids))
//                    refresh_dic.Remove(ids);
//            }
//        }   
//        //显示分解获得物品数量
//        Dictionary<int, int> Itemnum = new Dictionary<int, int>();
//        void showItemNum(uint tpid,bool add)
//        {

//            print("左边字典的个数：" + dic_left.Keys.Count);
//            SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
//            List<SXML> xmls = xml.GetNodeList("decompose");
//            foreach (SXML x in xmls)
//            {
//                sellItems iteminfos = new sellItems();
//                iteminfos.item_id = x.getInt("item");
//                iteminfos.item_num = x.getInt("num");
//                Itemnum[iteminfos.item_id] = iteminfos.item_num;
//            }
            
//            foreach (int i in Itemnum.Keys)
//            {
//                switch (i)
//                {
//                    case 1540:
//                        if (add)
//                            mojing_num += Itemnum[i];
//                        else
//                        {
//                            //print("1:" + i);
//                            //print("巫婆吗个的数量：" + Itemnum[i]);
//                            mojing_num -= Itemnum[i];
//                            //print("墨镜的数量：" + mojing_num);
//                        }
                           
//                        mojing.text = mojing_num.ToString();
//                        break;
//                    case 1541:
//                        if (add)
//                            shengguanghuiji_num += Itemnum[i];
//                        else
//                            shengguanghuiji_num -= Itemnum[i];
//                        shengguanghuiji.text = shengguanghuiji_num.ToString();
//                        break;
//                    case 1542:
//                        if (add)
//                            mifageli_num += Itemnum[i];
//                        else
//                            mifageli_num -= Itemnum[i];
//                        mifageli.text = mifageli_num.ToString();
//                        break;
//                }
//            }
//        }
        


//        //分解后刷新信息
//        public void refresh()
//        {
//            for (int i = 0; i < dic_leftAllid.Count;i++ )
//            {
//                GameObject go = dic_leftObj[dic_leftAllid[i]].transform.parent.gameObject;
//                Destroy(go);
//                GameObject objclone = GameObject.Instantiate(grid) as GameObject;
//                objclone.SetActive(true);
//                objclone.transform.SetParent(contain_left.transform, false);
//                objclone.transform.SetSiblingIndex(dic_leftObj.Count + 1);
//            }
//            dic_left.Clear();
//            dic_leftObj.Clear();
//            dic_leftAllid.Clear();
//            left_count = 0;
//            mojing_num = 0;
//            shengguanghuiji_num = 0;
//            mifageli_num = 0;
//            mojing.text = 0 + "";
//            shengguanghuiji.text = 0 + "";
//            mifageli.text = 0 + "";
//            purple.isOn = false;
//            blue.isOn = false;
//            green.isOn = false;
//            white.isOn = false;
//        }

        
//        List<uint> dic_leftAllid = new List<uint>();
//        void onDecompose(GameObject go)
//        {
//            dic_leftAllid.Clear();
//            if (dic_left.Keys.Count > 0)
//            {
//                foreach (uint i in dic_left.Keys)
//                {
//                    dic_leftAllid.Add(dic_left[i].id);
//                }
//                if (haveYullowEquip())
//                {
//                    prompt.SetActive(true);
//                }
//                else
//                {
//                    Sendproxy(go);
//                }
//            }
//            else
//                flytxt.instance.fly(ContMgr.getCont("a3_equipsell_pleaseadd"), 1);
//        }
//        void Sendproxy(GameObject go)
//        {
//            EquipProxy.getInstance().sendsell(dic_leftAllid);
//            if (prompt.activeSelf)
//                prompt.SetActive(false);
//        }
//        bool ishanve = false;
//        bool haveYullowEquip()
//        {
            
//            for (int i = 0; i < dic_leftAllid.Count; i++)
//            {
//                uint tpid = a3_BagModel.getInstance().getUnEquips()[dic_leftAllid[i]].tpid;
//                if (a3_BagModel.getInstance().getItemDataById(tpid).quality == 5)
//                {
//                    ishanve = true;
//                    break;
//                }
//                else
//                    ishanve = false;
//            }
//            return ishanve;
//        }
//        void onclose(GameObject go)
//        {
//            InterfaceMgr.getInstance().close(InterfaceMgr.A3_EQUIPSELL);
//            InterfaceMgr.getInstance().open(InterfaceMgr.A3_BAG);
//        }
//        void onQuicklyChoose(GameObject go)
//        {
//            if (isquicklyChoose == false)
//            {
//                quick_image.SetActive(true);
//                getComponentByPath<Text>("panel_right/Button/Text").text =ContMgr.getCont("a3_equipsell_changeone");
//                isquicklyChoose = true;
//            }
//            else
//            {
//                quick_image.SetActive(false);
//                getComponentByPath<Text>("panel_right/Button/Text").text = ContMgr.getCont("a3_equipsell_qualick");
//                isquicklyChoose = false;
//            }
//        }
//        //初始化数据
//        void showInfos()
//        {
//            contain.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
//            purple.isOn = false;
//            blue.isOn = false;
//            green.isOn = false;
//            white.isOn = false;
//            isquicklyChoose = false;
//            getComponentByPath<Text>("panel_right/Button/Text").text = ContMgr.getCont("a3_equipsell_qualick");
//        }
//        //清楚所有数据
//        void removeAllinfos()
//        {
//            dic_left.Clear();
//            dic_right.Clear();
//            dic_leftObj.Clear();
//            dic_rightObj.Clear();
//            dic_change.Clear();
//            Itemnum.Clear();
//            left_count = 0;
//            right_count = 0;
//            mojing_num = 0;
//            shengguanghuiji_num = 0;
//            mifageli_num = 0;
//            mojing.text = 0 + "";
//            shengguanghuiji.text = 0 + "";
//            mifageli.text = 0 + "";

//        }
//    }
//    class sellItems
//    {
//        public int item_id;
//        public int item_num;
//    }
//}
