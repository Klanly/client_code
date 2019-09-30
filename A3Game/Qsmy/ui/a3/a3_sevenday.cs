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
    class a3_sevenday:Window
    {
        //有三种类型
        //1：登陆,等级，消费 2：半价 3：其他
        //tab读表
        int get_id;
        Text point;
        Transform btns,
                  btn_contains,
                  four_box;


        GameObject day_this_obj,
                   btns_this_obj;


        GameObject grid,
                   contain;


        GameObject exp_obj;


        Text txt_overtime;

        List<GameObject> lst_fourbox = new List<GameObject>();
        List<RectTransform> obj_sevenday = new List<RectTransform>();
        Dictionary<int, int> dic_three_info = new Dictionary<int, int>();//第三种根据award_id找出他obj的位置
        List<GameObject> lst_obj = new List<GameObject>();//所有的。按个数顺序分
       // Dictionary<int, GameObject> dic_obj_others = new Dictionary<int, GameObject>();//第三种，按id分
        public static a3_sevenday _instance;


        GameObject[] point_obj_this;
        public override void init()
        {


            getComponentByPath<Text>("down/btns/0/Text").text = ContMgr.getCont("a3_sevenday_0");
            getComponentByPath<Text>("down/btns/1/Text (1)").text = ContMgr.getCont("a3_sevenday_1");
            getComponentByPath<Text>("down/btns/2/Text (2)").text = ContMgr.getCont("a3_sevenday_2");
            getComponentByPath<Text>("top/Text/Text (1)").text = ContMgr.getCont("a3_sevenday_3");
            getComponentByPath<Text>("top/Text/Text (2)").text = ContMgr.getCont("a3_sevenday_4");
            getComponentByPath<Text>("top/Text/Text (3)").text = ContMgr.getCont("a3_sevenday_5");
            getComponentByPath<Text>("top/Text/Text (4)").text = ContMgr.getCont("a3_sevenday_6");
            getComponentByPath<Text>("top/over_time").text = ContMgr.getCont("a3_sevenday_7");


            point_obj_this = new GameObject[4]{getGameObjectByPath("top/bg1/this"), getGameObjectByPath("top/bg2/this"), getGameObjectByPath("top/bg3/this"), getGameObjectByPath("top/bg4/this") };
            point = getComponentByPath<Text>("top/point/num");
            txt_overtime = getComponentByPath<Text>("top/over_time/over_time");
            exp_obj = getGameObjectByPath("top/exp/exp");
            grid =getGameObjectByPath("down/contain/scrollview/grid");
            contain = getGameObjectByPath("down/contain/scrollview/contain");
            btns = getTransformByPath("down/btns");
            btn_contains = getTransformByPath("down/contain");
            day_this_obj = getGameObjectByPath("down/days/0/that");
            btns_this_obj = getGameObjectByPath("down/btns/0/this");
            four_box = getTransformByPath("top/box");
            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) => { InterfaceMgr.getInstance().close(InterfaceMgr.A3_SEVENDAY); };
            initsomething();
        }
        public override void onShowed()
        {
            RefreshData(A3_SevendayModel.getInstance().thisday);
            _instance = this;
           
            A3_SevenDayProxy.getInstance().addEventListener(A3_SevenDayProxy.SEVENDAYINFO, RefreshData);
            btnOnClick(btns.GetChild(0).gameObject, 0);
            RefreshPointLight();

        }
        public override void onClosed()
        {
            A3_SevenDayProxy.getInstance().removeEventListener(A3_SevenDayProxy.SEVENDAYINFO, RefreshData);
        }
        //void cleansomething()
        //{
        //    for(int i=0;i<getTransformByPath("down/days").childCount;i++)
        //    {
        //        getTransformByPath("down/days").GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        //    }
        //}


        void RefreshData(GameEvent e)
        {
            RefreshData(A3_SevendayModel.getInstance().thisday);
        }

        int choseday = A3_SevendayModel.getInstance().thisday;
        public   void  RefreshData(int day)
        {

            RefreshOnjs(day);
            btnOnClick(btns.GetChild(0).gameObject, 0);
            choseday =day;
            Refresh_day_show(day);
           // cleansomething();
            Refresh_point();
            Refresh_FourBox();
            int thisday = day; 
            txt_overtime.text = ConvertStringToDateTime(muNetCleint.instance.CurServerTimeStamp + (7 - A3_SevendayModel.getInstance().thisday) * 86400);


           // getTransformByPath("down/days").GetChild(thisday - 1).transform.GetChild(0).gameObject.SetActive(true);
            Dictionary<int, sevendayData> dic = A3_SevendayModel.getInstance().dic_data;
            for (int i = 0; i < lst_obj.Count; i++)
            {
                int z = i;
               // 按钮(要刷新)
                //GameAniCamera  btn =lst_obj[i].transform.FindChild("Button").gameObject;   
                //Text btn_txt= lst_obj[i].transform.FindChild("Button/Text").GetComponent<Text>();
               // GameObject over_obj = lst_obj[i].transform.FindChild("over").gameObject;

                //名字（第三种要刷新）
                Text name=lst_obj[i].transform.FindChild("name").GetComponent<Text>();
                //点数
                Text point = lst_obj[i].transform.FindChild("jifen/num").GetComponent<Text>();             
                //奖励              
                GameObject contain= lst_obj[i].transform.FindChild("items").gameObject;

                lst_obj[i].transform.FindChild("zuanshi").gameObject.SetActive(false);
                if (i == 0)
                {

                    name.text = dic[thisday].loginaed.des;
                    point.text = dic[thisday].loginaed.point.ToString();
                    
                    if (contain.transform.childCount > 0)
                    {
                        DestroyImmediate(contain.transform.GetChild(0).gameObject);
                    }
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint)dic[thisday].loginaed.item_id, true, dic[thisday].loginaed.item_num, 0.7f);
                    icon.transform.SetParent(contain.transform, false);
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        itemtipOnclick(go, (uint)dic[thisday].loginaed.item_id, dic[thisday].loginaed.item_num);
                    };

                    RefreshData_lgAndbuy(0,thisday);
                }
                else if (i == 1)
                {
                    lst_obj[i].transform.FindChild("zuanshi/Text").GetComponent<Text>().text = dic[thisday].halfbuy.cost.ToString();
                    lst_obj[i].transform.FindChild("zuanshi").gameObject.SetActive(true);
                    name.text = dic[thisday].halfbuy.des;
                    point.text = dic[thisday].halfbuy.point.ToString();
                    if (contain.transform.childCount >0)
                    {
                        if (contain.transform.childCount > 0)
                        {
                            DestroyImmediate(contain.transform.GetChild(0).gameObject);
                        }

                    }
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint)dic[thisday].halfbuy.shop_item, true, dic[thisday].halfbuy.shop_num, 0.7f);
                    icon.transform.SetParent(contain.transform, false);
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        itemtipOnclick(go, (uint)dic[thisday].halfbuy.shop_item, dic[thisday].halfbuy.shop_num);
                    };
                    RefreshData_lgAndbuy(1,thisday);
                }
                else
                {
                    //get_id= dic[thisday].task_award.ElementAt(z - 2).Key;
                    dic_three_info[dic[thisday].task_award.ElementAt(i - 2).Key] = i;
                    point.text = dic[thisday].task_award.ElementAt(i - 2).Value.task_point.ToString();
                    if (contain.transform.childCount > 0)
                    {
                        for (int numc = 0; numc <contain.transform.childCount; numc++)
                        {
                            DestroyImmediate(contain.transform.GetChild(numc).gameObject);
                        }
                    }
                    for (int j = 0; j < dic[thisday].task_award.ElementAt(i - 2).Value.lst_ta.Count; j++)
                    {
                        uint id = (uint)dic[thisday].task_award.ElementAt(i - 2).Value.lst_ta[j].id;
                        int num = dic[thisday].task_award.ElementAt(i - 2).Value.lst_ta[j].value;
                        GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(id, true, num, 0.7f);
                        icon.transform.SetParent(contain.transform, false);
                        new BaseButton(icon.transform).onClick = (GameObject go) =>
                        {

                            itemtipOnclick(go, id, num);
                        };

                    }
                    Refresh_other(dic[thisday].task_award.ElementAt(i - 2).Key,thisday);

                }

            }





        }
        public string ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string st = dtStart.AddSeconds(timeStamp).ToString("yyyy-MM-dd");
            return st;
            // long lTime = long.Parse(timeStamp + "0000");
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);

        }

        // 0:登陆,1:半价刷新
        public void RefreshData_lgAndbuy(int i,int nowday)
        {
            //i =0登陆=1半价
            Refresh_point();
            BaseButton btn = new BaseButton(lst_obj[i].transform.FindChild("Button").transform);
            Text btn_txt = lst_obj[i].transform.FindChild("Button/Text").GetComponent<Text>();
            GameObject over_obj = lst_obj[i].transform.FindChild("over").gameObject;
            GameObject old_obj = lst_obj[i].transform.FindChild("old").gameObject;
            GameObject new_obj = lst_obj[i].transform.FindChild("new").gameObject;

            int thisday = A3_SevendayModel.getInstance().thisday;
            Dictionary<int, sevendayData> dic = A3_SevendayModel.getInstance().dic_data;

            btn_txt.text = i == 0 ? ContMgr.getCont("off_line_lq") : ContMgr.getCont("a3_sevenday_buy");
            if(i==0)
            {
                btn.interactable = false;
                btn.gameObject.SetActive(false);
                old_obj.SetActive(false);
                new_obj.SetActive(false);
                over_obj.SetActive(false);
                if(nowday<=thisday)
                {
                    switch (dic[nowday].loginaed.state)
                    {
                        case 0:/*可领没领*/
                            btn.interactable = true;
                            btn.gameObject.SetActive(true);
                            lst_obj[i].transform.SetAsFirstSibling();
                            break;
                        case 1:/*领过*/
                            over_obj.SetActive(true);
                            lst_obj[i].transform.SetAsLastSibling();
                            break;
                        case 2:/*不能领*/
                            btn.interactable = false;
                            btn.gameObject.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    new_obj.SetActive(true);
                }


            }
            else
            {
                btn.gameObject.SetActive(false);
                old_obj.SetActive(false);
                new_obj.SetActive(false);
                over_obj.SetActive(false);
                if (thisday == nowday)
                {
                    if (dic[nowday].halfbuy.isReceive)
                    {
                        over_obj.SetActive(true);
                        lst_obj[i].transform.SetAsLastSibling();
                    }
                    else
                    {
                        btn.gameObject.SetActive(true);
                    }
                }
                else if(nowday < thisday)
                {
                    old_obj.SetActive(true);
                }
                else if(nowday > thisday)
                {
                    new_obj.SetActive(true);
                }

                //btn_txt.text = i == 0 ? ContMgr.getCont("off_line_lq") : ContMgr.getCont("a3_sevenday_buy");
               
            }

                


            
        }
        // 其他刷新
        public void Refresh_other(int award_id, int nowday)
        {
            //nowday;当前选中天数 thisday:当天
            Refresh_point();
            int i;
            if(dic_three_info.ContainsKey(award_id))
            {
                i = dic_three_info[award_id];
            }
            else
                return;

            BaseButton btn = new BaseButton(lst_obj[i].transform.FindChild("Button").transform);
            Text btn_txt = lst_obj[i].transform.FindChild("Button/Text").GetComponent<Text>();
            GameObject over_obj = lst_obj[i].transform.FindChild("over").gameObject;
            GameObject old_obj = lst_obj[i].transform.FindChild("old").gameObject;
            GameObject new_obj = lst_obj[i].transform.FindChild("new").gameObject;
            Text name = lst_obj[i].transform.FindChild("name").GetComponent<Text>();

            int thisday = A3_SevendayModel.getInstance().thisday;
            Dictionary<int, sevendayData> dic = A3_SevendayModel.getInstance().dic_data;

            string txt = dic[nowday].task_award[award_id].task_des;
            btn_txt.text = ContMgr.getCont("a3_sevenday_get1");

            btn.gameObject.SetActive(false);
            btn.interactable = false;
            old_obj.SetActive(false);
            new_obj.SetActive(false);
            over_obj.SetActive(false);

            //des
            switch (dic[nowday].task_award[award_id].task_type)
            {
                //1:消费
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:

                    if (dic[nowday].task_award[award_id].task_have >=int.Parse(dic[nowday].task_award[award_id].task_need))
                        name.text = txt + ContMgr.getCont("canover");
                    else
                        name.text = txt + "(" +dic[nowday].task_award[award_id].task_have + "/" + dic[nowday].task_award[award_id].task_need + ")";
                    break;

                    //name.text = txt + "(" + dic[nowday].task_award[award_id].task_have + "/" + dic[nowday].task_award[award_id].task_need + ")";
                    //break;
                //8:等级奖励
                case 8:
                    name.text = txt /*+ "(" + "当前：" + PlayerModel.getInstance().up_lvl + "转" + PlayerModel.getInstance().lvl + "级" + ")";*/;
                    break;
            }
            //可领状态
            switch (dic[nowday].task_award[award_id].task_type)
            {
                case 1:case 2: case 3:case 4:case 5:case 6: case 7:case 9: case 10: case 11: case 12:case 13:case 14:case 15:case 16:
                    //if (nowday<thisday)
                    //{
                    //    old_obj.SetActive(true);
                    //}
                    if(nowday==thisday|| nowday < thisday)
                    {
                        if (dic[nowday].task_award[award_id].state == 0)
                        {
                            btn.gameObject.SetActive(true);
                            btn.interactable = false;
                        }
                        else if (dic[nowday].task_award[award_id].state == 1)
                        {
                            btn.gameObject.SetActive(true);
                            btn.interactable = true;
                            lst_obj[i].transform.SetAsFirstSibling();
                        }
                        else
                        {
                            over_obj.SetActive(true);
                            lst_obj[i].transform.SetAsLastSibling();
                        }

                    }
                    else
                    {
                        new_obj.SetActive(true);
                    }

                    break;
                case 8:            /*等级是可以领的其他不行*/
                    if(nowday<=thisday)
                    {
                        if (dic[nowday].task_award[award_id].state == 0)
                        {
                            btn.gameObject.SetActive(true);
                            btn.interactable = false;
                        }
                        else if (dic[nowday].task_award[award_id].state == 1)
                        {
                            btn.gameObject.SetActive(true);
                            btn.interactable = true;
                            lst_obj[i].transform.SetAsFirstSibling();
                        }
                        else
                        {
                            over_obj.SetActive(true);
                            lst_obj[i].transform.SetAsLastSibling();
                        }
                    }
                    else
                    {
                        new_obj.SetActive(true);
                    }

                    break;
            }


        }
        //刷新点数
        void Refresh_point()
        {
            int num = A3_SevendayModel.getInstance().have_point;

                float nub = (float)num/ 100;
                exp_obj.transform.localScale = num == 100 ? new Vector3(1, 1, 1) : new Vector3(nub, 1, 1);

            point.text = num.ToString();
        }
        //4个box刷新
        public void Refresh_FourBox()
        {

            List<four_box> lst = A3_SevendayModel.getInstance().jifen_box;
            for (int j = 0; j < lst_fourbox.Count; j++)
            {
                switch (lst[j].state)
                {
                    //不能领
                    case 0:
                        lst_fourbox[j].transform.GetChild(0).gameObject.SetActive(true);
                        lst_fourbox[j].transform.GetChild(1).gameObject.SetActive(false);
                        break;
                    //能领未领
                    case 1:
                        lst_fourbox[j].transform.GetChild(0).gameObject.SetActive(true);
                        lst_fourbox[j].transform.GetChild(1).gameObject.SetActive(false);
                        break;
                    //领过
                    case 2:
                        lst_fourbox[j].transform.GetChild(0).gameObject.SetActive(false);
                        lst_fourbox[j].transform.GetChild(1).gameObject.SetActive(true);
                        break;
                }

            }
        }
        //界面打开时过期
        public void Refresh_time()
        {
            txt_overtime.text = ContMgr.getCont("a3_sevenday_old");
        }
        //刷新天数显示
        void Refresh_day_show(int day)
        {


            int thisday = A3_SevendayModel.getInstance().thisday;
            //初始化的样子
            for (int i = 0; i < obj_sevenday.Count; i++)
            {
                obj_sevenday[i].GetComponent<LayoutElement>().minWidth = 0;


                obj_sevenday[i].FindChild("that").gameObject.SetActive(day-1==i?true:false);
                
                obj_sevenday[i].FindChild("over").gameObject.SetActive(false);
               
                string str = string.Empty;
                switch (i)
                {
                    case 0:
                        str = ContMgr.getCont("active_day1");
                        break;
                    case 1:
                        str = ContMgr.getCont("active_day2");
                        break;
                    case 2:
                        str = ContMgr.getCont("active_day3");
                        break;
                    case 3:
                        str = ContMgr.getCont("active_day4");
                        break;
                    case 4:
                        str = ContMgr.getCont("active_day5");
                        break;
                    case 5:
                        str = ContMgr.getCont("active_day6");
                        break;
                    case 6:
                        str = ContMgr.getCont("active_day7");
                        break;

                }
                obj_sevenday[i].FindChild("Text").GetComponent<Text>().text = str;
            }
            if (thisday < 7)
            {
                obj_sevenday[thisday-1].FindChild("Text").GetComponent<Text>().text=ContMgr.getCont("a3_sevenday_today");
                obj_sevenday[thisday].FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_sevenday_toww");
                obj_sevenday[thisday].GetComponent<LayoutElement>().minWidth = 250;
                string res = XMLMgr.instance.GetSXML("seven_days.seven_day", "day==" + (thisday + 1)).getString("item");
                string days = obj_sevenday[thisday].FindChild("Text").GetComponent<Text>().text;
                obj_sevenday[thisday].FindChild("Text").GetComponent<Text>().text = days + "<color=#FF0000>" + "（" + res + "）" + "</color>";
            }
            else
            {
                obj_sevenday[thisday - 1].FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_sevenday_today");
                obj_sevenday[6].GetComponent<LayoutElement>().minWidth = 250;
                string res = XMLMgr.instance.GetSXML("seven_days.seven_day", "day==" + thisday).getString("item");
                string days = obj_sevenday[thisday - 1].FindChild("Text").GetComponent<Text>().text;
                obj_sevenday[thisday - 1].FindChild("Text").GetComponent<Text>().text = days + "<color=#FF0000>" + "（" + res + "）" + "</color>";
            }


                


            //  for (int i = 0; i < obj_sevenday.Count; i++)
            //{
            //    if (i >= (thisday - 1))
            //    {
            //        obj_sevenday[i].FindChild("over").gameObject.SetActive(false);
            //    }

            //}

        }
        //积分奖励高亮显示
        public void RefreshPointLight()
        {
            for(int i=0;i< point_obj_this.Length;i++)
            {
                point_obj_this[i].SetActive(A3_SevendayModel.getInstance().pointshow[i] == true ? true : false);
               
            }
        }

        //三种类型obj的个数

        int one_num = 0;
        int two_num = 0;
        int three_num = 0;
        void initsomething()
        {
            one_num = 0;
            two_num = 0;
            three_num = 0;
            //天数obj
            for (int day = 0; day < 7; day++)
            {
                obj_sevenday.Add(getTransformByPath("down/days").GetChild(day).GetComponent<RectTransform>());
            }
            for(int j=0;j<obj_sevenday.Count;j++)
            {
                int clickday = j + 1;
                new BaseButton(obj_sevenday[j]).onClick =(GameObject go) => { RefreshData(clickday); };
            }

            ////每天的任务数量是一样的（先把obj创建出来，再刷新数据）
            //lst_obj.Clear();
            //int num = A3_SevendayModel.getInstance().dic_data[1].task_award.Keys.Count + 2;
            //for(int k=0;k<num;k++)
            //{
            //    int idx = k;
            //    GameObject objclone = GameObject.Instantiate(grid) as GameObject;
            //    objclone.SetActive(true);
               
            //    objclone.transform.SetParent(contain.transform, false);
            //    lst_obj.Add(objclone);

            //    objclone.transform.FindChild("Button").transform.GetComponent<Button>().onClick.AddListener(delegate() 
            //    {
            //        this.getOnClick(idx);
            //    });
            //    if (k ==0 )
            //    {
            //        objclone.name = 1.ToString();
            //        one_num += 1;
            //    }
            //   else  if(k==1)
            //    {
            //        objclone.name = 2.ToString();
            //        two_num += 1;
            //    }
            //   else if (k >= 2)
            //    {

            //            objclone.name = A3_SevendayModel.getInstance().dic_data[1].task_award.ElementAt(k-2).Value.tab.ToString();
            //            if (int.Parse(objclone.name)==1)
            //                one_num += 1;
            //            else if(int.Parse(objclone.name) == 2)
            //                two_num += 1;
            //            else if(int.Parse(objclone.name) == 3)
            //                three_num += 1;                    
            //        }
            //    }          
            //a3_runestone.commonScroview(contain, num);
            
            ////tabbtn
            //for (int i = 0; i < btns.childCount; i++)
            //{
            //    int j = i;
            //    new BaseButton(btns.GetChild(i)).onClick = (GameObject go) =>{ btnOnClick(go, j);};
            //}
            //4个点数宝箱obj

            for(int i=0;i< four_box.childCount;i++)
            {
                int j = i;
                lst_fourbox.Add(four_box.GetChild(i).gameObject);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon((uint) A3_SevendayModel.getInstance().jifen_box[j].lst_ta.id,true,A3_SevendayModel.getInstance().jifen_box[j].lst_ta.value,1);
                icon.transform.SetParent(four_box.GetChild(i).transform.GetChild(0).transform, false);
                icon.transform.FindChild("iconbor").gameObject.SetActive(false);
                icon.transform.FindChild("wk").gameObject.SetActive(false);
                new BaseButton(icon.transform).onClick = (GameObject go) =>         
                {
                    if(A3_SevendayModel.getInstance().have_point >= A3_SevendayModel.getInstance().jifen_box[j].param1)
                         A3_SevenDayProxy.getInstance().SendProcy(5, A3_SevendayModel.getInstance().jifen_box[j].id);
                    else
                         itemtipOnclick(go, (uint)A3_SevendayModel.getInstance().jifen_box[j].lst_ta.id, A3_SevendayModel.getInstance().jifen_box[j].lst_ta.value);
                };
            }
            
        }

        void RefreshOnjs(int day)
        {
          for(int i =0;i<contain.transform.childCount;i++)

            {

                Destroy(contain.transform.GetChild(i).gameObject);
            }
            one_num = 0;
            two_num = 0;
            three_num = 0;
            //每天的任务数量是一样的（先把obj创建出来，再刷新数据）//变成不一样了
            lst_obj.Clear();
            int num = A3_SevendayModel.getInstance().dic_data[day].task_award.Keys.Count + 2;
            for (int k = 0; k < num; k++)
            {
                int idx = k;
                GameObject objclone = GameObject.Instantiate(grid) as GameObject;
                objclone.SetActive(true);

                objclone.transform.SetParent(contain.transform, false);
                lst_obj.Add(objclone);

                objclone.transform.FindChild("Button").transform.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    this.getOnClick(idx);
                });
                if (k == 0)
                {
                    objclone.name = 1.ToString();
                    one_num += 1;
                }
                else if (k == 1)
                {
                    objclone.name = 2.ToString();
                    two_num += 1;
                }
                else if (k >= 2)
                {

                    objclone.name = A3_SevendayModel.getInstance().dic_data[day].task_award.ElementAt(k - 2).Value.tab.ToString();
                    if (int.Parse(objclone.name) == 1)
                        one_num += 1;
                    else if (int.Parse(objclone.name) == 2)
                        two_num += 1;
                    else if (int.Parse(objclone.name) == 3)
                        three_num += 1;
                }
            }
            a3_runestone.commonScroview(contain, num);

            //tabbtn
            for (int i = 0; i < btns.childCount; i++)
            {
                int j = i;
                new BaseButton(btns.GetChild(i)).onClick = (GameObject go) => { btnOnClick(go, j); };
            }
        }
        //tabbtnOnClock
        int old_i = 0;
        void btnOnClick(GameObject go,int i)
        {
            //if (old_i==i)          
            //    return;
           // else
           // {
                //btn_contains.GetChild(i).gameObject.SetActive(true);
                //btn_contains.GetChild(old_i).gameObject.SetActive(false);
                btns_this_obj.transform.SetParent(go.transform, false);
                btns_this_obj.transform.SetSiblingIndex(0);
                for(int j=0;j< lst_obj.Count;j++)
                {
                    if(int.Parse(lst_obj[j].name)==(i+1))
                    {
                        lst_obj[j].SetActive(true);
                    }
                    else
                    {
                        lst_obj[j].SetActive(false);
                    }
                }
                if (i == 0)
                {
                    a3_runestone.commonScroview(contain, one_num);
                }
                else if (i == 1)
                {
                    a3_runestone.commonScroview(contain, two_num);
                }
                else if (i == 2)
                {
                    a3_runestone.commonScroview(contain, three_num);
                }
                old_i = i;
         //   }

           
        }
        //tip显示
        void itemtipOnclick(GameObject go,uint id,int num)
        {
            ArrayList arr = new ArrayList();
            arr.Add(id);
            arr.Add(num);
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            a3_miniTip.Instance?.transform.SetAsLastSibling();
        }






        //领取按钮
        void getOnClick(int ides)
        {
            switch(ides)
            {
                case 0:
                    A3_SevenDayProxy.getInstance().SendProcy(2,day:choseday);
                    break;
                case 1:
                    A3_SevenDayProxy.getInstance().SendProcy(3);
                    break;
                default:
                    get_id = A3_SevendayModel.getInstance().dic_data[choseday].task_award.ElementAt(ides - 2).Key;
                    A3_SevenDayProxy.getInstance().SendProcy(4, get_id);
                    break;
            }
        }
    }
}
