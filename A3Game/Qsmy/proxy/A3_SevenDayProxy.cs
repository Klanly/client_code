using Cross;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class A3_SevenDayProxy : BaseProxy<A3_SevenDayProxy>
    {

        public static uint SEVENDAYINFO = 1;
        public A3_SevenDayProxy()
        {
            addProxyListener(PKG_NAME.SEVENDAYS, GetInfos);
        }

        public void SendProcy(int res, int id = -1, int day = 0)
        {
            Variant msg = new Variant();
            msg["op"] = res;
            switch(res)
            {
                case 1:
                    break;
                case 2:
                    msg["day"] = day;
                    break;
                case 3:
                    break;
                case 4:
                    msg["awd_id"] = id;
                    break;
                case 5:
                    msg["awd_id"] = id;
                    break;                   
            }
            sendRPC(PKG_NAME.SEVENDAYS, msg);
        }


        private void GetInfos(Variant data)
        {
            debug.Log("七日目标：" + data.dump());


            int res = data["res"]._int;


            switch(res)
            {

                case 1:
                    A3_SevendayModel.getInstance().can_num = 0;
                    A3_SevendayModel.getInstance().thisday = data["total_day"];
                    A3_SevendayModel.getInstance().have_point = data["point"];
                    A3_SevendayModel.getInstance().today_cost = data["today_cost"];

                    //登陆奖励的state是0和1
                    if(data["today_awd"].Count>0)
                    {
                        for (int halflogin = 0; halflogin < data["today_awd"].Count; halflogin++)
                        {
                            A3_SevendayModel.getInstance().RefreshLg(data["today_awd"][halflogin]["day"], data["today_awd"][halflogin]["state"]);
                            if (data["today_awd"][halflogin]["state"] == 0)
                                A3_SevendayModel.getInstance().can_num += 1;
                        }
                    }
                    //任务奖励
                    A3_SevendayModel.getInstance().RefreshHb(data["today_buy"]);
                    List<Variant> lst = data["every_day_awd"]._arr;
                    if (lst.Count > 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            A3_SevendayModel.getInstance().Refreshs(lst[i]["id"], lst[i]["state"],lst[i]["reach_num"]);
                            if (lst[i]["state"] == 1&&(lst[i]["id"]._int/100== int.Parse(data["total_day"]))/*（发的是整个七天的）*/)
                                A3_SevendayModel.getInstance().can_num += 1;
                        }
                    }
                    //点数奖励
                    List<Variant> lsts = data["point_awd"]._arr;
                    if(lsts.Count>0)
                    {
                        for (int i = 0; i < lsts.Count; i++)
                        {
                            A3_SevendayModel.getInstance().Refresh_fourbox(lsts[i]["id"], lsts[i]["state"]);
                            A3_SevendayModel.getInstance().pointshow[lsts[i]["id"] - 1] = lsts[i]["state"] == 1 ? true : false;
                            if (lsts[i]["state"] == 1)
                                A3_SevendayModel.getInstance().can_num += 1;
                        }
                    }

                    //任务进度
                    List<Variant> lst_reach_num = data["reach_list"]._arr;
                    if (lst_reach_num.Count > 0)
                    {
                        for (int i = 0; i < lst_reach_num.Count; i++)
                        {
                            A3_SevendayModel.getInstance().RefreshRach_num(lst_reach_num[i]["awd_type"], lst_reach_num[i]["reach_num"]);
                        }
                    }


                    dispatchEvent(GameEvent.Create(SEVENDAYINFO, this, data));
                    A3_SevendayModel.getInstance().showOrHideFire();
                    break;
                case 2:
                    A3_SevendayModel.getInstance().can_num -= 1;
                    A3_SevendayModel.getInstance().RefreshPoint(0);
                    A3_SevendayModel.getInstance().RefreshLg(data["day"],1);
                    if (a3_sevenday._instance)
                        a3_sevenday._instance.RefreshData_lgAndbuy(0,/*A3_SevendayModel.getInstance().thisday*/data["day"]);

                    A3_SevendayModel.getInstance().showOrHideFire();
                    break;

                case 3:
                    A3_SevendayModel.getInstance().RefreshPoint(1);
                    A3_SevendayModel.getInstance().RefreshHb(data["today_buy"]);
                    if (a3_sevenday._instance)
                        a3_sevenday._instance.RefreshData_lgAndbuy(1, A3_SevendayModel.getInstance().thisday);
                    break;
                case 4:
                    A3_SevendayModel.getInstance().can_num -= 1;
                    A3_SevendayModel.getInstance().have_point = data["point"];
                    A3_SevendayModel.getInstance().Refreshs(data["awd_id"],2);
                    if (a3_sevenday._instance)
                        a3_sevenday._instance.Refresh_other(data["awd_id"], /*A3_SevendayModel.getInstance().thisday*/(int)(data["awd_id"]._int/100));

                    A3_SevendayModel.getInstance().showOrHideFire();
                    break;
                case 5:
                    A3_SevendayModel.getInstance().can_num -= 1;
                    A3_SevendayModel.getInstance().Refresh_fourbox(data["awd_id"], 2);
                    A3_SevendayModel.getInstance().pointshow[data["awd_id"] - 1] =false;
                    
                    if (a3_sevenday._instance)
                    {
                        a3_sevenday._instance.Refresh_FourBox(); a3_sevenday._instance.RefreshPointLight();
                    }

                        A3_SevendayModel.getInstance().showOrHideFire();
                    break;
                case 7:
                    if(data.ContainsKey("changed"))
                    {
                        for (int i = 0; i < data["changed"].Count; i++)
                        {
                           
                            A3_SevendayModel.getInstance().Refreshs(data["changed"][i]["id"], data["changed"][i]["state"], data["changed"][i]["reach_num"]);
                            int thisays=data["changed"][i]["id"]._int /100;
                            int thisday = A3_SevendayModel.getInstance().thisday;
                            if (data["changed"][i]["state"] == 1 && thisday==thisays/*（其他天的也发）*/)
                                A3_SevendayModel.getInstance().can_num += 1;
                            if (a3_sevenday._instance)
                                a3_sevenday._instance.Refresh_other(data["changed"][i]["id"], /*A3_SevendayModel.getInstance().thisday*/thisays);
                        }
                        A3_SevendayModel.getInstance().showOrHideFire();
                    }
                    break;
                case 8:
                    if(data["changed"]!=null)
                    {
                        for(int i =0;i< data["changed"].Count;i++)
                        {
                            A3_SevendayModel.getInstance().pointshow[data["changed"][i]["id"] - 1] = data["changed"][i]["state"] == 1 ? true : false;
                        }
                    }
                    if (a3_sevenday._instance)
                        a3_sevenday._instance.RefreshPointLight();

                    break;
                case 9:
                    if (data["today_cost"]!=null)
                        A3_SevendayModel.getInstance().today_cost = data["today_cost"];
                    if(data["awd_type"] !=null)
                    {
                       A3_SevendayModel.getInstance().RefreshRach_num(data["awd_type"], data["reach_num"]);
                       
                        
                    }

                    if (a3_sevenday._instance)
                        a3_sevenday._instance.RefreshData(A3_SevendayModel.getInstance().thisday);
                    break;
                case -6904:
                    tinmesover = true;
                    //活动过期，隐藏图标
                    if (a3_sevenday._instance)
                        a3_sevenday._instance.Refresh_time();
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.hideSevendays" , "ui/interfaces/low/a1_low_fightgame", 0);
                    return; 
                default:                 
                    Globle.err_output(res);
                    return;
            }

            if (res != -6904&&a3_timegifs.showover==false)
            {
                //是不是买过了
                foreach (shopDatas item in Shop_a3Model.getInstance().itemsdic.Values)
                {
                    if (item.day == A3_SevendayModel.getInstance().thisday)
                    {
                        if (item.limiteD == 0)
                        {
                            return;
                        }
                    }
                }
                
                tinmesover = false;
                /*暂时关闭*/
                //InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TIMEGIFS);
                a3_timegifs.instance?.transform.SetAsLastSibling();

            }

        }

        public static bool  tinmesover=false;//过期

    }
}
