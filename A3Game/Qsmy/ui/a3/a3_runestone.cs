using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Cross;
using DG.Tweening;
namespace MuGame
{
    class a3_runestone : Window
    {
        Dictionary<int, a3_RunestoneData> dic_allrune_data = a3_BagModel.getInstance().allRunestoneData();
        //合成
        Dictionary<int, GameObject> dic_compose_showobjs = new Dictionary<int, GameObject>();
        GameObject compose_obj;
        string path_str = "compose/left/bg/top/";
        GameObject image_lv;
        GameObject contain_lv,
                   contain_quality,
                   contain_name;
        List<GameObject> list_compose_contain;
        GameObject contain_compose_obj,
                   image_compose_obj;
        Text compose_lv_txt,
             compose_quality_txt,
             compose_name_txt;
        Slider exp_obj,
              stamin_obj;
        Text exp,
             stamin;
        Text money,
             buy_num;
        BaseButton compose_bymoney_btn;
        //列表
        Dictionary<int, GameObject> dic_list_showobjs = new Dictionary<int, GameObject>();
        GameObject list_obj;
        GameObject list_contain_lv,
                   list_contain_quality,
                   list_contain_name;
        List<GameObject> list_list_contain;
        GameObject image_list_obj,
                   contain_list_obj;
        Text list_lv_txt,
             list_quality_txt,
             list_name_txt;
        //显示符石
        Dictionary<uint, GameObject> dic_runestones_obj = new Dictionary<uint, GameObject>();
        //穿戴
        GameObject dress_obj,
                   dress_bg;
        GameObject decomposeBg;
        List<GameObject> lst_dresspos = new List<GameObject>();

        //批量分解
        GameObject decompose_obj;
        Toggle white,
               green,
               blue,
               purple,
               orange;
        GameObject contain_decompose,
                   image_decompose;
        BaseButton decompose_btn;


        //panel
        GameObject composebtn_image;
        GameObject dressbtn_image;
        GameObject contain_haveRunestones,
                   image_hanvRunestones;
        RectTransform rct_contain_haveRunestones,
                      rct_scrollview_haveRunestones;
        BaseButton compose_btn,
                   dress_btn;
        public static a3_runestone _instance;

        #region ScrollControler
        ScrollControler scrollControler,
                        scrollControler1,
                        scrollControler2,
                        scrollControler3,
                        scrollControler4,
                        scrollControler5,
                        scrollControler6,
                        scrollControler7,
                        scrollControler8,
                        scrollControler9;
        #endregion
        public override void init()
        {
            
            new BaseButton(getTransformByPath("Button")).onClick = (GameObject go) => bttttt();
            
            compose_obj = getGameObjectByPath("compose");
            image_lv = getGameObjectByPath(path_str + "lv/scrollview/Image");
            contain_lv = getGameObjectByPath(path_str + "lv/scrollview/grid");
            contain_quality = getGameObjectByPath(path_str + "quality/scrollview/grid");
            contain_name = getGameObjectByPath(path_str + "name/scrollview/grid");
            list_compose_contain = new List<GameObject> { contain_lv, contain_quality, contain_name };
            BaseButton choselv_btn = new BaseButton(getTransformByPath(path_str + "lv/Button"));
            BaseButton chosequality_btn = new BaseButton(getTransformByPath(path_str + "quality/Button"));
            BaseButton chosename_btn = new BaseButton(getTransformByPath(path_str + "name/Button"));
            choselv_btn.onClick = chosequality_btn.onClick = chosename_btn.onClick = (GameObject go) => OpenchoseOnClick(go, list_compose_contain);
            image_compose_obj = getGameObjectByPath("compose/left/bg/down/scrollview/Image");
            contain_compose_obj = getGameObjectByPath("compose/left/bg/down/scrollview/contain");
            compose_lv_txt = getComponentByPath<Text>("compose/left/bg/top/lv/chose");
            compose_quality_txt = getComponentByPath<Text>("compose/left/bg/top/quality/chose");
            compose_name_txt = getComponentByPath<Text>("compose/left/bg/top/name/chose");
            exp_obj = getComponentByPath<Slider>("compose/left/exp/exp");
            stamin_obj = getComponentByPath<Slider>("compose/left/strength/strength");
            exp = getComponentByPath<Text>("compose/left/exp/Text");
            stamin = getComponentByPath<Text>("compose/left/strength/Text");
            money = getComponentByPath<Text>("compose/left/need_money/num");
            buy_num = getComponentByPath<Text>("compose/left/nub/num");
            new BaseButton(getTransformByPath("compose/left/nub/right_btn")).onClick = (GameObject go) => addnum();
            new BaseButton(getTransformByPath("compose/left/nub/left_btn")).onClick = (GameObject go) => reducenum();
            compose_bymoney_btn = new BaseButton(getTransformByPath("compose/left/need_money"));
            compose_bymoney_btn.onClick = (GameObject go) => compose_bymonry();
            



            list_obj = getGameObjectByPath("list");
            new BaseButton(getTransformByPath("list/close_btn")).onClick = (GameObject go) => openorcloselistOnClock(false);
            new BaseButton(getTransformByPath("close/list_btn")).onClick = (GameObject go) => openorcloselistOnClock(true);
            list_contain_lv = getGameObjectByPath("list/top/lv/scrollview/grid");
            list_contain_quality = getGameObjectByPath("list/top/quality/scrollview/grid");
            list_contain_name = getGameObjectByPath("list/top/name/scrollview/grid");
            list_list_contain = new List<GameObject> { list_contain_lv, list_contain_quality, list_contain_name };
            BaseButton list_choselv_btn = new BaseButton(getTransformByPath("list/top/lv/Button"));
            BaseButton list_chosequality_btn = new BaseButton(getTransformByPath("list/top/quality/Button"));
            BaseButton list_chosename_btn = new BaseButton(getTransformByPath("list/top/name/Button"));
            list_choselv_btn.onClick = list_chosequality_btn.onClick = list_chosename_btn.onClick = (GameObject go) => OpenchoseOnClick(go, list_list_contain);
            image_list_obj = getGameObjectByPath("list/down/scrollview/Image");
            contain_list_obj = getGameObjectByPath("list/down/scrollview/grid");
            list_lv_txt = getComponentByPath<Text>("list/top/lv/chose");
            list_quality_txt = getComponentByPath<Text>("list/top/quality/chose");
            list_name_txt = getComponentByPath<Text>("list/top/name/chose");



            dress_obj = getGameObjectByPath("dress");
            dress_bg = getGameObjectByPath("dress/bg");
            decomposeBg = getGameObjectByPath("haverunestones/Image");
            


            decompose_obj = getGameObjectByPath("decompose");
            new BaseButton(getTransformByPath("haverunestones/Image/btn")).onClick = (GameObject go) => openorclsoeDecompose(true);
            new BaseButton(getTransformByPath("decompose/close_btn")).onClick = (GameObject go) => openorclsoeDecompose(false);
            contain_decompose = getGameObjectByPath("decompose/scroll_view/contain");
            image_decompose = getGameObjectByPath("decompose/scroll_view/icon");
            decompose_btn = new BaseButton(getTransformByPath("decompose/info_bg/btn"));
            decompose_btn.onClick = decomposesOnclick;
            white = getComponentByPath<Toggle>("decompose/info_bg/Toggle_all/Toggle_white");
            white.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    ShowComposeRuneston(1);
                }
                else
                    DesComposeRuneston(1);
            });
            green = getComponentByPath<Toggle>("decompose/info_bg/Toggle_all/Toggle_green");
            green.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    ShowComposeRuneston(2);
                    if (white.isOn == false)
                        white.isOn = true;
                }
                else
                    DesComposeRuneston(2);
            });
            blue = getComponentByPath<Toggle>("decompose/info_bg/Toggle_all/Toggle_blue");
            blue.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    ShowComposeRuneston(3);
                    if (green.isOn == false)
                        green.isOn = true;
                }
                else
                    DesComposeRuneston(3);
            });
            purple = getComponentByPath<Toggle>("decompose/info_bg/Toggle_all/Toggle_puple");
            purple.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    ShowComposeRuneston(4);
                    if (blue.isOn == false)
                        blue.isOn = true;
                }
                else
                    DesComposeRuneston(4);
            });
            orange = getComponentByPath<Toggle>("decompose/info_bg/Toggle_all/Toggle_orange");
            orange.onValueChanged.AddListener(delegate (bool ison)
            {
                if (ison)
                {
                    ShowComposeRuneston(5);
                    if (purple.isOn == false)
                        purple.isOn = true;
                }
                else
                    DesComposeRuneston(5);
            });







            image_hanvRunestones = getGameObjectByPath("haverunestones/scrollview/icon");
            contain_haveRunestones = getGameObjectByPath("haverunestones/scrollview/contain");
            


            composebtn_image = getGameObjectByPath("Panel/compose_btn/Image");
            dressbtn_image = getGameObjectByPath("Panel/dress_btn/Image");
            rct_contain_haveRunestones = contain_compose_obj.GetComponent<RectTransform>();
            rct_scrollview_haveRunestones = getGameObjectByPath("haverunestones/scrollview").GetComponent<RectTransform>();
            new BaseButton(getTransformByPath("close/close_btn")).onClick = (GameObject go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_RUNESTONE);
            compose_btn = new BaseButton(getTransformByPath("Panel/compose_btn"));
            dress_btn = new BaseButton(getTransformByPath("Panel/dress_btn"));
            dress_btn.onClick = compose_btn.onClick = btnSwitch;

            //初始化
            ScrollControler();
            //
            InitchosebtnOnclick(list_compose_contain, 1);
            InitchosebtnOnclick(list_list_contain, 2);
            LvStaminaInfos();
            //
            InitSccrollHaveRunestonsGrids();
            InitBagHaveRunestones();
            //
            InitDress();
        }

        public override void onShowed()
        {
            _instance = this;
            if (uiData!=null)
            {
                btnSwitch(dress_btn.gameObject);
            }

            ClosechoseOnClick(null, 1);
            a3_RuneStoneProxy.getInstance().addEventListener(a3_RuneStoneProxy.INFOS, LvExpInfos);
            
        }
        public override void onClosed()
        {
            a3_RuneStoneProxy.getInstance().removeEventListener(a3_RuneStoneProxy.INFOS, LvExpInfos);

            OpenchoseOnClick(null, list_compose_contain);


            btnSwitch(getGameObjectByPath("Panel/compose_btn"));
            switchbl = false;
            oldswitchbl = false;
        }
        #region 穿戴
        void  InitDress()
        {
            for (int i = 0; i < dress_bg.transform.childCount; i++)
            {
                lst_dresspos.Add(dress_bg.transform.GetChild(i).gameObject);
            }
            Dictionary<uint, a3_BagItemData> dic = A3_RuneStoneModel.getInstance().dressup_runestones;
            foreach (uint i in dic.Keys)
            {
                DressUp(dic[i], i);
            }
        }
        /*卸下符石*/
        public  void DressDown(int pos)
        {

            Destroy(lst_dresspos[pos-1].transform.GetChild(0).gameObject);
        }
        /*穿上符石*/
       public   void DressUp(a3_BagItemData data,uint id)
        {
            if (lst_dresspos[data.runestonedata.position].transform.childCount > 0)
                DressDown(data.runestonedata.position + 1);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, -1, 1f);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            icon.transform.SetParent(lst_dresspos[a3_BagModel.getInstance().getRunestoneDataByid((int)data.tpid).position-1].transform, false);
            icon.name = id.ToString();
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) { this.onItemClick(data,icon, id, 2); };
        }


        #endregion
        #region 批量分解   
        void openorclsoeDecompose(bool isopen)
        {
            //decompose_obj.SetActive(isopen ? true : false);
            if(isopen)
            {
                decompose_obj.SetActive(true);
                creatrvegrids();
            }
            else
            {
                decompose_obj.SetActive(false);
                destorygrids();
                orange.isOn = false;
                purple.isOn = false;
                blue.isOn = false;
                green.isOn = false;
                white.isOn = false;
            }
        }
        void creatrvegrids()
        {
            int gridnum = contain_haveRunestones.transform.childCount;
            gridnum += (7-gridnum % 7);
            for (int i=0;i<gridnum;i++)
            {
                commonCreatrveImages(contain_decompose,image_decompose);
            }
            commonScroview(contain_decompose, gridnum);
        }
        void destorygrids()
        {
            for (int i = 0; i < contain_decompose.transform.childCount; i++)
            {
                Destroy(contain_decompose.transform.GetChild(i).gameObject);
            }
        }
        void ShowComposeRuneston(int quality)
        {
            Dictionary<uint, a3_BagItemData> dics = a3_BagModel.getInstance().getRunestonrs();
            int indes = 0;
            foreach(uint i in dics.Keys)
            {              
                if (a3_BagModel.getInstance().getRunestoneDataByid((int)(dics[i].tpid)).quality == quality&& dics[i].ismark == false)
                {
                    creatrveIcons(dics[i], indes);
                    indes++;
                }                                
            }
        }
        void DesComposeRuneston(int qualit)
        { 
            Dictionary<uint, a3_BagItemData> dics = a3_BagModel.getInstance().getRunestonrs();
            for (int i = 0; i < lst_decompose.Count; i++)
            {
                if(a3_BagModel.getInstance().getRunestoneDataByid((int)(dics[lst_decompose[i]].tpid)).quality==qualit)
                {
                    Destroy(decompose_runestones[lst_decompose[i]].transform.parent.gameObject);
                    decompose_runestones.Remove(lst_decompose[i]);
                    commonScroview(contain_decompose, 1);
                }
            }
            lst_decompose.Clear();
            foreach(uint i in decompose_runestones.Keys)
            {
                lst_decompose.Add(i);
            }
        }             
        List<uint> lst_decompose = new List<uint>();//需要分解的符石
        Dictionary<uint, GameObject> decompose_runestones = new Dictionary<uint, GameObject>();
        private void creatrveIcons(a3_BagItemData data, int index)
        {
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, -1, 0.8f);
            icon.transform.SetParent(contain_decompose.transform.GetChild(index).transform, false);
            icon.transform.FindChild("iconborder/ismark").gameObject.SetActive(false);
            icon.name = data.id.ToString();
            lst_decompose.Add(data.id);
            decompose_runestones[data.id] = icon;
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) { this.onItemClick(data, icon, data.id, 3); };
        }
        public void destoryIcon(uint id)
        {
            if (decompose_runestones.ContainsKey(id))
            {
                Destroy(decompose_runestones[id].transform.parent.gameObject);
                decompose_runestones.Remove(id);
                lst_decompose.Remove(id);
            }
            commonCreatrveImages(contain_decompose, image_decompose);
        }
        void decomposesOnclick(GameObject go)
        {
            if(lst_decompose.Count<=0)
                flytxt.instance.fly(ContMgr.getCont("a3_runestone_nostone"));
            else
            { 
              a3_RuneStoneProxy.getInstance().sendporxy(4, 0, 0, lst_decompose);
              RefreshDecompose();
            }
        }
        void RefreshDecompose()
        {
            for (int i = 0; i < lst_decompose.Count; i++)
            {
                Destroy(decompose_runestones[lst_decompose[i]].transform.parent.gameObject);
                decompose_runestones.Remove(lst_decompose[i]);
            }
            lst_decompose.Clear();
            decompose_runestones.Clear();
        }
        #endregion
        #region  合成及列表查询
            /*3种选择界面刷新*/
        uint imageInstanceid = 0;
        uint old_imageInstanceid = 0;
        int lv_type = 0;
        int quality_type = 0;
        int name_type = 0;
        void OpenchoseOnClick(GameObject go, List<GameObject> list_contain)
        {
            for (int i = 0; i < list_contain.Count; i++)
            {
                if (go != null)
                {
                    if (list_contain[i].transform.parent.transform.parent == go.transform.parent)
                        list_contain[i].transform.DOScaleY(1, 0.1f);
                    else
                        list_contain[i].transform.DOScaleY(0, 0.1f);
                }
                else
                    list_contain[i].transform.DOScaleY(0, 0.1f);
            }
        }
        void ClosechoseOnClick(GameObject go, int type)
        {
            if (go == null)
            {
                lv_type = 0;
                quality_type = 0;
                name_type = 0;
                /*type
                 * 1:合成
                 * 2：list
                 */
                string quan = ContMgr.getCont("quan");
                if (type == 1)
                {
                    compose_lv_txt.text = quan;
                    compose_quality_txt.text = quan;
                    compose_name_txt.text = quan;
                }
                else if (type == 2)
                {
                    list_lv_txt.text = quan;
                    list_quality_txt.text = quan;
                    list_name_txt.text = quan;
                }
            }
            else
            {
                go.transform.parent.transform.DOScaleY(0, 0.1f);
                imageInstanceid = (uint)go.GetInstanceID();
                if (imageInstanceid == old_imageInstanceid)
                    return;
                old_imageInstanceid = imageInstanceid;
                string str = go.transform.FindChild("Text").GetComponent<Text>().text;
                GameObject obj = go.transform.parent.parent.parent.gameObject;
                obj.transform.FindChild("chose").GetComponent<Text>().text = str;
                switch (obj.name)
                {
                    case "lv":
                        lv_type = int.Parse(go.name);
                        break;
                    case "quality":
                        quality_type = int.Parse(go.name);
                        break;
                    case "name":
                        name_type = int.Parse(go.name);
                        break;
                    default:
                        break;
                }
            }
            if (type == 1)
                RefreshRunes(contain_compose_obj, image_compose_obj, dic_compose_showobjs, A3_RuneStoneModel.getInstance().nowlv);
            else if (type == 2)
                RefreshRunes(contain_list_obj, image_list_obj, dic_list_showobjs);
        }
        void RefreshRunes(GameObject contain, GameObject image, Dictionary<int, GameObject> dic, int lv = 0)
        {
            /*
             * lv=0   列表查询
             * lv!=0  合成
            */
            foreach (int i in dic.Keys) { Destroy(dic[i]); }
            list_dic.Clear();
            dic.Clear();
            Dictionary<int, a3_RunestoneData> dics = new Dictionary<int, a3_RunestoneData>();
            if (lv != 0)
            {
                foreach (int i in dic_allrune_data.Keys)
                    if (dic_allrune_data[i].stone_level <= lv) dics[i] = dic_allrune_data[i];
            }
            else { dics = dic_allrune_data; }
            //有空把方法换一下
            #region
            if (lv_type == 0)
            {
                if (quality_type == 0)
                {
                    if (name_type == 0)
                    {
                        foreach (int i in dics.Keys)
                        {
                            commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                    else
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].name_type == name_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                }
                else
                {
                    if (name_type == 0)
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].quality == quality_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                    else
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].quality == quality_type && dics[i].name_type == name_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                }
            }
            else
            {
                if (quality_type == 0)
                {
                    if (name_type == 0)
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].stone_level == lv_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                    else
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].stone_level == lv_type && dics[i].name_type == name_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                }
                else
                {
                    if (name_type == 0)
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].stone_level == lv_type && dics[i].quality == quality_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                    else
                    {
                        foreach (int i in dics.Keys)
                        {
                            if (dics[i].stone_level == lv_type && dics[i].quality == quality_type && dics[i].name_type == name_type)
                                commonCreatrveImages(contain, image, i, dic, lv != 0 ? true : false);
                        }
                    }
                }
            }
            #endregion
            showInfos(contain, dic);
            //默认选第一个
            if (lv != 0)
            {
                if (list_dic.Count > 0)
                    thisbtnOclick(list_dic[0], int.Parse(list_dic[0].name));
                else
                {
                    nub = 0;
                    needmoney_num = 0;
                    buy_num.text = nub.ToString();
                    money.text = needmoney_num.ToString();
                }
            }

            //commomScrollRectPosition(getComponentByPath<ScrollRect>("compose/left/bg/down/scrollview"));
            //commomScrollRectPosition(getComponentByPath<ScrollRect>("list/down/scrollview"));
            //print("lv:" + lv + "quality:" + quality + "name:" + name);
        }
        void showInfos(GameObject contain, Dictionary<int, GameObject> dic)
        {
            if (dic == null)
                return;
            List<GameObject> list_item;
            foreach (int id in dic.Keys)
            {
                //等级锁
                if (dic[id].transform.FindChild("lock") != null)
                {
                    Text txt_lock = dic[id].transform.FindChild("lock").GetComponent<Text>();
                    txt_lock.text = ContMgr.getCont("a3_runestone_lv") + dic_allrune_data[id].stone_level + ContMgr.getCont("a3_runestone_oplock");
                    txt_lock.gameObject.SetActive(dic_allrune_data[id].stone_level >= A3_RuneStoneModel.getInstance().nowlv ? true : false);
                }
                //选中图片
                if (dic[id].transform.FindChild("this") != null)
                {

                }
                //icon
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(a3_BagModel.getInstance().getItemDataById((uint)id), false,-1,0.6f);
                icon.transform.SetParent(dic[id].transform.FindChild("icon").transform, false);
                //name
                Text txt_name = dic[id].transform.FindChild("name").GetComponent<Text>();
                txt_name.text = dic_allrune_data[id].item_name;
                //材料显示
                GameObject item1 = dic[id].transform.FindChild("1").gameObject;
                GameObject item2 = dic[id].transform.FindChild("2").gameObject;
                list_item = new List<GameObject> { item1, item2 };
                item2.SetActive(dic_allrune_data[id].compose_data.Count > 1 ? true : false);
                for (int i = 0; i < dic_allrune_data[id].compose_data.Count; i++)
                {
                    string file = a3_BagModel.getInstance().getItemDataById((uint)dic_allrune_data[id].compose_data[i].id).file;
                    list_item[i].transform.FindChild("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                    list_item[i].transform.FindChild("num").GetComponent<Text>().text =
                       a3_BagModel.getInstance().getItemNumByTpid((uint)dic_allrune_data[id].compose_data[i].id) + "/" + dic_allrune_data[id].compose_data[i].num;
                }
            }
            commonScroview(contain, dic.Keys.Count);

        }
        void compose_bymonry()
        {
            if (this_id == -1)
                return;
            else
            {
                SXML xml = XMLMgr.instance.GetSXML("item.item", "id==" + this_id);
                int neednum = xml.getInt("stamina_use");
                if (A3_RuneStoneModel.getInstance().nowstamina < neednum) { flytxt.instance.fly(ContMgr.getCont("a3_runestone_nolive"), 1); }
                else { a3_RuneStoneProxy.getInstance().sendporxy(2, this_id, nub); }
            }

            
            
        }
        /*材料变动（分解合成）*/
        public void refreshHvaeMaterialNum()
        {
            if (dic_compose_showobjs != null)
            {
                List<GameObject> list_item = new List<GameObject>();
                foreach (int id in dic_compose_showobjs.Keys)
                {
                    GameObject item1 = dic_compose_showobjs[id].transform.FindChild("1").gameObject;
                    GameObject item2 = dic_compose_showobjs[id].transform.FindChild("2").gameObject;
                    list_item = new List<GameObject> { item1, item2 };
                    for (int i = 0; i < dic_allrune_data[id].compose_data.Count; i++)
                    {
                        list_item[i].transform.FindChild("num").GetComponent<Text>().text =
                            a3_BagModel.getInstance().getItemNumByTpid( (uint)dic_allrune_data[id].compose_data[i].id) + "/" + dic_allrune_data[id].compose_data[i].num;
                    }
                }
            }
        }
        /*等级选择刷新(合成界面）*/
        public   void RefreScrollLv(int lv)
        {
            //print(objclone.GetInstanceID());//唯一id
            GameObject objclone = GameObject.Instantiate(image_lv) as GameObject;
            objclone.SetActive(true);
            objclone.transform.SetParent(contain_lv.transform, false);
            objclone.name = lv.ToString();
            objclone.transform.FindChild("Text").GetComponent<Text>().text = lv + ContMgr.getCont("ji");
            new BaseButton(objclone.transform).onClick = (GameObject go) => ClosechoseOnClick(go, 1);
            commonScroview(contain_lv, lv +1);
          
        }
        /*左右按钮*/
        void addnum()
        {
            if (min_num == 0)
                return;
            nub++;
            zzzz();
        }
        void reducenum()
        {
            if (min_num == 0)
                return;
            nub--;
            zzzz();
        }
        void zzzz()
        {
            if (nub <= 1)
                nub = 1;
            else if (nub >= max_num)
                nub = max_num;
            buy_num.text = nub.ToString();
            money.text = (needmoney_num * nub).ToString();
        }
        /*选中要合成的符石，合成按钮*/
        void thisbtnOclick(GameObject go, int id)
        {
            foreach (int i in dic_compose_showobjs.Keys)
            {
                dic_compose_showobjs[i].transform.FindChild("this").gameObject.SetActive(false);
            }
            go.transform.FindChild("this").gameObject.SetActive(true);
            refreBynumAndMoney(id);
        }
        int min_num = 0;
        int max_num = 0;
        int needmoney_num = 0;       //单个购买所需的钱
        int nub = 0;                 //购买数量
        int this_id = -1;            //当前所选物品id
        void refreBynumAndMoney(int id)
        {
            List<int> lst = new List<int>();
            int num;
            bool canby = false;
            for (int i = 0; i < dic_allrune_data[id].compose_data.Count; i++)
            {
                if (a3_BagModel.getInstance().getItemNumByTpid((uint)dic_allrune_data[id].compose_data[i].id) > 0)
                {
                    num = (int)Mathf.Floor(a3_BagModel.getInstance().getItemNumByTpid((uint)dic_allrune_data[id].compose_data[i].id) / dic_allrune_data[id].compose_data[i].num);
                    if (num <= 0)
                    {
                        canby = false;
                        break;
                    }
                    else
                    {
                        canby = true;
                        lst.Add(num);
                    }
                }
                else
                    break;
            }
            lst.Sort();
            if (canby)
            {
                min_num = 1;
                max_num = lst[0] > 1 ? lst[0] : 1;
                nub = 1;
                this_id = id;
                needmoney_num = A3_RuneStoneModel.getInstance().getNeedMoney(id);
                getComponentByPath<Button>("compose/left/need_money").interactable = true;
            }
            else
            {
                min_num = 0;
                max_num = 0;
                nub = 0;
                this_id = -1;
                needmoney_num = 0;
                getComponentByPath<Button>("compose/left/need_money").interactable = false;
            }
            buy_num.text = nub.ToString();
            money.text = needmoney_num.ToString();
        }
        /*列表界面切换*/
        void openorcloselistOnClock(bool isopen)
        {
            list_obj.SetActive(isopen ? true : false);
            if (!isopen)
            {
                OpenchoseOnClick(null, list_list_contain);

            }
            else
            {
                ClosechoseOnClick(null, 2);
            }
            commomScrollRectPosition(getComponentByPath<ScrollRect>("list/down/scrollview"));
        }    
        //初始化的一些数据
        /*等级体力*/
        void LvStaminaInfos()
        {
            int Stamina = A3_RuneStoneModel.getInstance().nowstamina;
            stamin.text = ContMgr.getCont("a3_runestone_live") + "    "+ Stamina;
            stamin_obj.value = Stamina / 50;
            int lv= A3_RuneStoneModel.getInstance().nowlv;
            int nowexp= A3_RuneStoneModel.getInstance().nowexp;
            uint nextexp;
            SXML xml = XMLMgr.instance.GetSXML("rune_stone.compose", "level==" + lv);
            if (xml.getUint("exp") != 0)
                nextexp = xml.getUint("exp");
            else
                nextexp = XMLMgr.instance.GetSXML("rune_stone.compose", "level==" + (lv - 1)).getUint("exp");
            exp.text = lv+ ContMgr.getCont("ji") + "      " + nowexp + "/" + nextexp;
            exp_obj.value = (float)nowexp / nextexp;
        }
        /*选择条件的按钮*/
        void InitchosebtnOnclick(List<GameObject> list_contain, int type)
        {
            for (int i = 0; i < list_contain.Count; i++)
            {
                for (int j = 0; j < list_contain[i].transform.childCount; j++)
                {
                    new BaseButton(list_contain[i].transform.GetChild(j).transform).onClick = (GameObject go) => ClosechoseOnClick(go, type);

                }
            }
            int lv = A3_RuneStoneModel.getInstance().nowlv;
            for(int i =2;i<lv+1;i++)
            {
                RefreScrollLv(i);
            }
        }
        //监听
        void LvExpInfos(GameEvent e)
        {
            LvStaminaInfos();

        }
        #endregion
        #region 已有符石显示刷新
        //（rect.height402   528）
        /*初始化40个格子，增减物品刷新格子数量*/
        int grids_nub = 40;
        void InitSccrollHaveRunestonsGrids()
        {
            for (int i = 0; i < grids_nub; i++)
            {
                commonCreatrveImages(contain_haveRunestones, image_hanvRunestones/*, i*/);
            }
        }
        void RefreshSccrollHaveRunestonsGrid()
        {
            int count = A3_RuneStoneModel.getInstance().getHaveRunestones().Keys.Count;
            if (count <= grids_nub)
            {
                if (contain_haveRunestones.transform.childCount <= grids_nub)
                    return;
                else
                    RefreshSccrollHaveRunestonsGrid(false, contain_haveRunestones.transform.childCount - grids_nub);
            }
            else
            {
                int x = (count - grids_nub) % 5;
                int num = x % 5 == 0 ? x : (int)Mathf.Ceil(x / 5);
                RefreshSccrollHaveRunestonsGrid(true,x<=5?5: num * 5);
            }
        }
        void RefreshSccrollHaveRunestonsGrid(bool add, int num)
        {
            if (add)
            {
                for (int i = 0; i < num ; i++)
                {
                    commonCreatrveImages(contain_haveRunestones, image_hanvRunestones/*, grids_nub + i*/);
                }
                commonScroview(contain_haveRunestones, grids_nub + num);
            }
            else
            {
                int nub = contain_haveRunestones.transform.childCount; 
                int dnub = nub - num;
                for (int i = dnub ; i < nub; i++)
                {
                     Destroy(contain_haveRunestones.transform.GetChild(i).gameObject);
                   
                }
                commonScroview(contain_haveRunestones, nub - num);
            }

        }
        /*创建符石*/
        void InitBagHaveRunestones()
        {
            Dictionary<uint, a3_BagItemData> dic = new Dictionary<uint, a3_BagItemData>();
            if (A3_RuneStoneModel.getInstance().getHaveRunestones() == null|| dic == A3_RuneStoneModel.getInstance().getHaveRunestones())
                return;
            dic = A3_RuneStoneModel.getInstance().getHaveRunestones();
            int index = 0;
            RefreshSccrollHaveRunestonsGrid();
            foreach (a3_BagItemData data in A3_RuneStoneModel.getInstance().getHaveRunestones().Values)
            {
                CreatrveIcons(data, index);
                index++;
            }
        }
        /*刷新符石*/
        public   void addHaveRunestones(a3_BagItemData data)
        {
            if (!dic_runestones_obj.ContainsKey(data.id))
            {
                RefreshSccrollHaveRunestonsGrid();
                CreatrveIcons(data, dic_runestones_obj.Count);
            }
        }
        public   void removeHaveRunestones(uint id)
        {
            RefreshSccrollHaveRunestonsGrid();
            Destroy(dic_runestones_obj[id].transform.parent.gameObject);
            dic_runestones_obj.Remove(id);
            commonCreatrveImages(contain_haveRunestones, image_hanvRunestones);
            
        }
        /*刷新mark*/
        public void RefreshMark(uint id,bool ismark)
        {
            if(dic_runestones_obj.ContainsKey(id))
            {
                dic_runestones_obj[id].transform.FindChild("iconborder/ismark").gameObject.SetActive(ismark?true:false);
            }
        }
        void CreatrveIcons(a3_BagItemData data, int i)
        {
            GameObject icon= IconImageMgr.getInstance().createA3ItemIcon(data, true, -1, 0.8f);
            icon.transform.SetParent(contain_haveRunestones.transform.GetChild(i).transform, false);
            icon.name = data.id.ToString();
            dic_runestones_obj[data.id] = icon;
            BaseButton bs_bt = new BaseButton(icon.transform);
            bs_bt.onClick = delegate (GameObject go) { this.onItemClick(data,icon, data.id,1); };
        }

        void onItemClick(a3_BagItemData one,GameObject icon,uint id,int type)
        {
            /*type:
             *1:已有符石显示界面时*
             * a.合成界面
             * b.穿戴界面（未穿身上）
             *2:已穿身上
             *3:分解取出
             */           
            ArrayList data = new ArrayList();
            
            if(type==1)
            {
                a3_BagItemData d = a3_BagModel.getInstance().getItems()[id];
                data.Add(d);
                if (compose_obj.activeSelf)
                    data.Add(runestone_tipstype.compose_tips);
                else
                    data.Add(runestone_tipstype.dress_tip);
            }
            else if (type == 2)
            {
                data.Add(one);
                data.Add(runestone_tipstype.dressdown_tip);
            }
            else if(type==3)
            {
                a3_BagItemData d = a3_BagModel.getInstance().getItems()[id];
                data.Add(d);
                data.Add(runestone_tipstype.decompose_tip);
            }
                
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RUNESTONETIP, data);
        }

        #endregion
        #region 合成穿戴切换界面

        bool switchbl = false;
        bool oldswitchbl = false;
        void btnSwitch(GameObject go)
        {
            switchbl = go.name == "compose_btn" ? false : true;
            if (oldswitchbl == switchbl)
                return;
            compose_obj.SetActive(switchbl ? false : true);
            composebtn_image.SetActive(switchbl ? false : true);
            dress_obj.SetActive(switchbl ? true : false);
            dressbtn_image.SetActive(switchbl ? true : false);
            decomposeBg.SetActive(switchbl ? true : false);
            // rct_contain_haveRunestones.sizeDelta = new Vector2(400, switchbl?402:528);
            rct_scrollview_haveRunestones.sizeDelta = new Vector2(400, switchbl ? 402 : 528);
            commomScrollRectPosition(rct_scrollview_haveRunestones.GetComponent<ScrollRect>());
            oldswitchbl = switchbl;
        }




        #endregion
        List<GameObject> list_dic = new List<GameObject>();//
        public void commonCreatrveImages(GameObject contain, GameObject image, int num=-1, Dictionary<int, GameObject> dic = null, bool isbtn = false)
        {
            GameObject objclone = GameObject.Instantiate(image) as GameObject;
            objclone.SetActive(true);
            objclone.transform.SetParent(contain.transform, false);
            if( num!= -1)
              objclone.name = num.ToString();
            if (dic != null) { dic[num] = objclone; }
            if (isbtn)
            {
                list_dic.Add(objclone);
                new BaseButton(objclone.transform).onClick = (GameObject go) => thisbtnOclick(go, int.Parse(go.name));
            }
        }

        /*滑动条长度*/
        //有时候destory会刷新，不能用findchild找         
        public  static void commonScroview(GameObject contain, int childcount)
        {
            /*有个问题tsm.sizeDelta.x必须>=glg.cellSize.x*/
            RectTransform tsm = contain.GetComponent<RectTransform>();
            GridLayoutGroup glg = contain.GetComponent<GridLayoutGroup>();
            int width_num = (int)Mathf.Floor((tsm.sizeDelta.x + glg.spacing.x) / (glg.cellSize.x + glg.spacing.x));
            int height_num = (int)Mathf.Ceil(childcount / (float)width_num);
            tsm.sizeDelta = new Vector2(tsm.sizeDelta.x, height_num * glg.cellSize.y + (height_num - 1) * glg.spacing.y);
        }
        public void commomScrollRectPosition(ScrollRect sr)
        {
            if (sr != null)
                sr.verticalNormalizedPosition = 1;
        }
        void ScrollControler()
        {
            scrollControler = new ScrollControler();
            scrollControler1 = new ScrollControler();
            scrollControler2 = new ScrollControler();
            scrollControler3 = new ScrollControler();
            scrollControler4 = new ScrollControler();
            scrollControler5 = new ScrollControler();
            scrollControler6 = new ScrollControler();
            scrollControler7 = new ScrollControler();
            scrollControler8 = new ScrollControler();
            scrollControler9 = new ScrollControler();
            scrollControler.create(getComponentByPath<ScrollRect>("compose/left/bg/down/scrollview"));
            scrollControler1.create(getComponentByPath<ScrollRect>("compose/left/bg/top/lv/scrollview"));
            scrollControler2.create(getComponentByPath<ScrollRect>("compose/left/bg/top/quality/scrollview"));
            scrollControler3.create(getComponentByPath<ScrollRect>("compose/left/bg/top/name/scrollview"));
            scrollControler4.create(getComponentByPath<ScrollRect>("haverunestones/scrollview"));
            scrollControler5.create(getComponentByPath<ScrollRect>("decompose/scroll_view"));
            scrollControler6.create(getComponentByPath<ScrollRect>("list/down/scrollview"));
            scrollControler7.create(getComponentByPath<ScrollRect>("list/top/lv/scrollview"));
            scrollControler8.create(getComponentByPath<ScrollRect>("list/top/quality/scrollview"));
            scrollControler9.create(getComponentByPath<ScrollRect>("list/top/name/scrollview"));

        }
        void bttttt()
        {
            a3_RuneStoneProxy.getInstance().sendporxy(6, 1638);
        }
    }
}
