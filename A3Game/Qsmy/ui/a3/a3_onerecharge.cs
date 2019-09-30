using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace MuGame
{
    class a3_onerecharge : Window
    {

        TabControl _tabControl;
        GameObject contain, contain1, image;

        GameObject tip;

        Dictionary<string, Variant> dic = new Dictionary<string, Variant>();
        Dictionary<int, Dictionary<int,GameObject>> dic_obj = new Dictionary<int, Dictionary<int,GameObject>>();
        public static Dictionary<int, Dictionary<int, int>> dics = new Dictionary<int, Dictionary<int, int>>();

        public static a3_onerecharge _instance;


        public override void init()
        {

            _tabControl = new TabControl();
            _tabControl.onClickHanle = OnSwitch;
            _tabControl.create(getGameObjectByPath("btns"), this.gameObject);
            contain = getGameObjectByPath("Panel/contain");
            contain1 = getGameObjectByPath("Panel1/contain");
            image = getGameObjectByPath("Panel/Image");
            tip = getGameObjectByPath("tip");

            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) =>
              {
                  InterfaceMgr.getInstance().close(InterfaceMgr.A3_ONERECHARGE);
              };


            new BaseButton(getTransformByPath("Button")).onClick = (GameObject go) =>
            {

                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ONERECHARGE);
                ArrayList a = new ArrayList();
                a.Add(indexx);

                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AWARDCENTER,a);

            };


        }
        public int indexx= 0;
        void OnSwitch(TabControl tabc)
        {
            int index = tabc.getSeletedIndex();
            switch (index)
            {
                case 0:
                    indexx = 0;
                    getGameObjectByPath("Panel").SetActive(true);
                    getGameObjectByPath("Panel1").SetActive(false);
                    getGameObjectByPath("Panel").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                    break;
                case 1:
                    indexx = 1;
                    getGameObjectByPath("Panel").SetActive(false);
                    getGameObjectByPath("Panel1").SetActive(true);
                    getGameObjectByPath("Panel1").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                    break;
            };

        }
        public override void onShowed()
        {
            _instance = this;
           

            if (uiData != null)
            {
                getGameObjectByPath("btns").transform.GetChild((int)uiData[0]).gameObject.SetActive(false);             
                _tabControl.setSelectedIndex((int)uiData[0]==0?1:0, true);
            }
            else
            {
                _tabControl.setSelectedIndex(0, true);
            }

            initdata();
           // DataRefresh();

            getGameObjectByPath("Panel").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        }

        int num_cz = 0;
        int num_db = 0;
        void initdata()
        {
            GameEvent e = A3_AwardCenterServer.getInstance().Alldata;
            Variant data = e.data;
            Variant awardLst = data["actonline_conf"];
            if (awardLst != null)
            {
                for (int v = 0; v < awardLst._arr.Count; v++)
                {

                    string name = awardLst[v]["name"]._str;
                    int tp= awardLst[v]["tp"]._int;
                    if (tp == 1 || tp ==3)
                    {
                        Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();
                        int op = awardLst[v]["tp"]._int;

                        if (awardLst[v]["action"]["list"] != null)
                        {

                            for (int va = 0; va < awardLst[v]["action"]["list"]._arr.Count; va++)
                            {
                                int id = awardLst[v]["action"]["list"][va]["id"]._int;
                                GameObject clone = GameObject.Instantiate(image) as GameObject;
                                clone.name = id.ToString();
                                clone.SetActive(true);
                                if (tp == 3)
                                {
                                    num_cz++;
                                    clone.transform.SetParent(contain.transform, false);
                                    clone.transform.FindChild("Text").GetComponent<Text>().text = "充值有奖";
                                    clone.transform.FindChild("value").GetComponent<Text>().text = awardLst[v]["action"]["list"][va]["param"]._int.ToString(); ;
                                }
                                if(tp==1)
                                {
                                    num_db++;
                                    clone.transform.FindChild("Text").GetComponent<Text>().text = "单笔充值";
                                    clone.transform.SetParent(contain1.transform, false);
                                    clone.transform.FindChild("value").GetComponent<Text>().text = awardLst[v]["action"]["list"][va]["param"]._int.ToString();
                                }
                                foreach (var var in awardLst[v]["action"]["list"][va]["RewardValue"]._arr)
                                {
                                    string type = awardLst[v]["action"]["list"][va]["RewardValue"][var]["type"]._str;
                                    int num = awardLst[v]["action"]["list"][va]["RewardValue"][var]["value"]._int;
                                    GameObject gso = IconImageMgr.getInstance().createMoneyIcon(type, 0.8f, num);
                                    gso.transform.SetParent(clone.transform.FindChild("Panel").transform, false);
                                }
                                for (int var = 0; var < awardLst[v]["action"]["list"][va]["RewardItem"]._arr.Count; var++)
                                {
                                    uint item_id = awardLst[v]["action"]["list"][va]["RewardItem"][var]["item_id"]._uint;
                                    int num = awardLst[v]["action"]["list"][va]["RewardItem"][var]["value"]._int;
                                    GameObject gso = IconImageMgr.getInstance().createA3ItemIcon(item_id, scale: 0.8f, num: num);
                                    gso.transform.SetParent(clone.transform.FindChild("Panel").transform, false);
                                    new BaseButton(gso.transform).onClick = (GameObject go) =>
                                      {
                                          showtip(item_id);
                                      };
                                }

                                if (dics.ContainsKey(op)&&dics[op].ContainsKey(id))
                                {
                                    string shoe = string.Empty;
                                    //  print("op是多少:"+op+"id是多少：" + id + "状态是多少:" + dics[op][id]);
                                    switch (dics[op][id])
                                    {
                                        default:
                                        case 0:
                                            shoe = "前往";
                                            clone.transform.FindChild("Button").gameObject.SetActive(true);
                                            clone.transform.FindChild("over").gameObject.SetActive(false);
                                            break;
                                        case 1:
                                            shoe = "领取";
                                            clone.transform.FindChild("Button").gameObject.SetActive(true);
                                            clone.transform.FindChild("over").gameObject.SetActive(false);
                                            break;
                                        case 2:
                                            clone.transform.FindChild("Button").gameObject.SetActive(false);
                                            clone.transform.FindChild("over").gameObject.SetActive(true);
                                            shoe = "已领取";
                                            break;
                                    }

                                    clone.transform.FindChild("Button/Text").GetComponent<Text>().text = shoe;
                                }
                                new BaseButton(clone.transform.FindChild("Button").transform).onClick = (GameObject go) =>
                                {
                                    if(!dics[op].ContainsKey(id))
                                    {
                                        InterfaceMgr.getInstance().close(InterfaceMgr.A3_ONERECHARGE);
                                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                                    }
                                    else
                                    {
                                        switch (dics[op][id])
                                        {
                                            default:
                                            case 0:
                                                InterfaceMgr.getInstance().close(InterfaceMgr.A3_ONERECHARGE);
                                                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
                                                break;
                                            case 1:
                                                Variant Itemdata = new Variant();
                                                Itemdata["act_type"] = op;
                                                Itemdata["award_id"] = id;

                                                A3_AwardCenterServer.getInstance().SendMsg(A3_AwardCenterServer.EVENT_GETAWARD, Itemdata);
                                                break;
                                            case 2:
                                                break;
                                        }
                                    }

                                };
                                




                                dic[id] = clone;
                            }
                        }
                        dic_obj[op] = dic;
                    }


                }
            }


            a3_runestone.commonScroview(contain, num_cz);
            a3_runestone.commonScroview(contain1, num_db);
        }

        public void DataRefresh(int op,int id)
        {
            if(dic_obj.ContainsKey(op))
            {
                if(dic_obj[op].ContainsKey(id) && dic_obj[op][id]!=null)
                {
                    dic_obj[op][id].transform.FindChild("Button/Text").GetComponent<Text>().text = "已领取";
                    dic_obj[op][id].transform.FindChild("Button").gameObject.SetActive(false);
                    dic_obj[op][id].transform.FindChild("over").gameObject.SetActive(true);
                    dic_obj[op][id].transform.SetAsLastSibling();
                }
            }
     

                
            



        }
        public void showtip(uint id)
        {
            tip.SetActive(true);
            a3_ItemData item = a3_BagModel.getInstance().getItemDataById(id);
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().text = item.item_name;
            transform.FindChild("tip/text_bg/name/hasnum").GetComponent<Text>().text = a3_BagModel.getInstance().getItemNumByTpid(id) + ContMgr.getCont("ge");
            tip.transform.FindChild("text_bg/name/namebg").GetComponent<Text>().color = Globle.getColorByQuality(item.quality);
            if (item.use_limit <= 0) { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = ContMgr.getCont("a3_active_wuxianzhi"); }
            else { tip.transform.FindChild("text_bg/name/dengji").GetComponent<Text>().text = item.use_limit + ContMgr.getCont("zhuan"); }
            tip.transform.FindChild("text_bg/text").GetComponent<Text>().text = StringUtils.formatText(item.desc);
            tip.transform.FindChild("text_bg/iconbg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(item.file);

            new BaseButton(tip.transform.FindChild("close_btn")).onClick = (GameObject oo) => { tip.SetActive(false); };
        }
        public override void onClosed()
        {

        }

    }
}
