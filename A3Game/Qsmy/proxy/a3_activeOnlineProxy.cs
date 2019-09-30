using Cross;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class a3_activeOnlineProxy:BaseProxy<a3_activeOnlineProxy>
    {

        public static uint ACTIVELOTTERY=1;
        public static uint ACTIVELOTTERYOVER= 2;
        public a3_activeOnlineProxy()
        {
            addProxyListener(PKG_NAME.S2C_ACTIVEONLINE, getinfos);
        }



        public void SendProxy(int res,int point=-1, int fund_type = -1,int awd_id=-1)
        {
            Variant msg = new Variant();
            msg["op"] = res;
            switch (res)
            {

                case 1:
                    break;

                case 2:
                    msg["point"] = point;
                    break;
                case 3:
                    msg["fund_type"] = fund_type;
                    break;
                case 4:
                    msg["awd_id"] = awd_id;
                    break;
                case 5:
                    break;

            }
            sendRPC(PKG_NAME.S2C_ACTIVEONLINE, msg);
        }


        void getinfos(Variant data)
        {
            debug.Log("受到新的活跃的协议：" + data.dump());
            if (data.ContainsKey("res"))
            {
                int res = data["res"];
                switch (res)
                {
                    case 1:

                        //活跃任务
                        if (data["huoyues"].Length > 0)
                        {
                            for (int i = 0; i < data["huoyues"].Length; i++)
                            {
                                a3_ActiveOnlineModel.getInstance().Refresh_huoyue(data["huoyues"][i]["active_id"], data["huoyues"][i]["count"]);
                            }
                        }
                        //活跃点数基金是否够买
                        a3_ActiveOnlineModel.getInstance().nowpoint = data["huoyue_point"];
                        a3_ActiveOnlineModel.getInstance().zhuansheng_fund = data["zhuan_fund"];
                        a3_ActiveOnlineModel.getInstance().zhanli_fund = data["combpt_fund"];
                        a3_ActiveOnlineModel.getInstance().zuanshi_fund = data["login_fund"];
                        //活跃点数奖励
                        if (data["huoyue_reward"].Count > 0)
                        {
                            for (int i = 0; i < data["huoyue_reward"].Count; i++)
                            {
                                a3_ActiveOnlineModel.getInstance().Refresh_reward(data["huoyue_reward"][i]);
                            }
                        }
                        //基金状态
                        if (data["fund_list"].Count > 0)
                        {
                            for (int i = 0; i < data["fund_list"].Count; i++)
                            {
                                a3_ActiveOnlineModel.getInstance().Refresh_fund(data["fund_list"][i]["id"], data["fund_list"][i]["state"]);
                            }
                        }
                        if (data["login_day"] != null)
                        {
                            a3_ActiveOnlineModel.getInstance().RefreshZuanshi_fundnow(data["login_day"]);
                        }
                        //12点刷新界面
                        if (a3_activeOnline.instance && !a3_activeOnline.instance.ischoujiang)
                        {
                            a3_activeOnline.instance.Refresh();
                        }
                        //抽奖单独
                        if (a3_activeOnline.instance && a3_activeOnline.instance.ischoujiang)
                        {
                            a3_activeOnline.instance.ischoujiang = false;
                            dispatchEvent(GameEvent.Create(ACTIVELOTTERY, this, data));
                        }
                        //提示
                        /*有新奖励可以领*/
                        Dictionary<int, reward_info> dic = a3_ActiveOnlineModel.getInstance().dic_reward;
                        foreach (int i in dic.Keys)
                        {
                            if(data["huoyue_point"] >= dic[i].ac && dic[i].receive_type != 2)
                            {
                                a3_ActiveOnlineModel.getInstance().hintreward = true;
                                InterfaceMgr.doCommandByLua("a1_low_fightgame.open_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                                return;
                            }
                            else
                                a3_ActiveOnlineModel.getInstance().hintreward = false;
                        }
                        /*有新抽奖可以抽*/
                        if (data["lottery_count"] <= 6&& data["online_tm"] >= a3_ActiveOnlineModel.getInstance().dic_online[data["lottery_count"]].time)
                        {
                            a3_ActiveOnlineModel.getInstance().hintlottery = true;
                            InterfaceMgr.doCommandByLua("a1_low_fightgame.open_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                            if (a3_activeOnline.instance)
                                a3_activeOnline.instance.RefreshHint();
                            return;
                        }
                        else
                            a3_ActiveOnlineModel.getInstance().hintlottery = false;

                        if(a3_ActiveOnlineModel.getInstance().hintreward != true && a3_ActiveOnlineModel.getInstance().hintlottery != true)
                            InterfaceMgr.doCommandByLua("a1_low_fightgame.close_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.RefreshHint();


                        break;
                    case 2:
                        if (data["huoyue_reward"].Count > 0)
                        {
                            for (int i = 0; i < data["huoyue_reward"].Count; i++)
                            {
                                a3_ActiveOnlineModel.getInstance().Refresh_reward(data["huoyue_reward"][i]);
                            }
                        }
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.Resresh_reward();
                        /*还有奖励没领取*/
                        Dictionary<int, reward_info> dics = a3_ActiveOnlineModel.getInstance().dic_reward;
                        foreach (int i in dics.Keys)
                        {
                            if (a3_ActiveOnlineModel.getInstance().nowpoint >= dics[i].ac && dics[i].receive_type != 2)
                            {
                                a3_ActiveOnlineModel.getInstance().hintreward = true;
                                InterfaceMgr.doCommandByLua("a1_low_fightgame.open_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                                if (a3_activeOnline.instance)
                                    a3_activeOnline.instance.RefreshHint();
                                return;
                            }
                            else
                                a3_ActiveOnlineModel.getInstance().hintreward = false;
                        }
                        if (a3_ActiveOnlineModel.getInstance().hintreward != true && a3_ActiveOnlineModel.getInstance().hintlottery != true)
                            InterfaceMgr.doCommandByLua("a1_low_fightgame.close_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.RefreshHint();
                        break;
                    case 3:
                        switch(data["fund_type"]._int)
                        {
                            case 1:
                                a3_ActiveOnlineModel.getInstance().zuanshi_fund = true;
                                break;
                            case 2:
                                a3_ActiveOnlineModel.getInstance().zhuansheng_fund = true;
                                break;
                            case 3:
                                a3_ActiveOnlineModel.getInstance().zhanli_fund = true;
                                break;
                        }
                        //if (a3_activeOnline.instance)
                       //     a3_activeOnline.instance.Refreshbuy_fund(data["fund_type"]);
                        if (a3_awardCenter._instance)
                            a3_awardCenter._instance.Refreshbuy_fund(data["fund_type"]);
                        break;
                    case 4:
                        a3_ActiveOnlineModel.getInstance().Refresh_fund(data["awd_id"],2);
                      //  if (a3_activeOnline.instance)
                         //   a3_activeOnline.instance.Refreshfun(data["awd_id"]);
                        if (a3_awardCenter._instance)
                            a3_awardCenter._instance.Refreshfun(data["awd_id"]);
                        break;
                    case 5:
                        a3_ActiveOnlineModel.getInstance().hintlottery = false;
                        if (a3_ActiveOnlineModel.getInstance().hintreward != true && a3_ActiveOnlineModel.getInstance().hintlottery != true)
                            InterfaceMgr.doCommandByLua("a1_low_fightgame.close_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.RefreshHint();
                        dispatchEvent(GameEvent.Create(ACTIVELOTTERYOVER, this, data));
                        break;
                    case 6:/*抽奖可以抽的时候主动发*/
                        /*有新抽奖可以抽*/
                        a3_ActiveOnlineModel.getInstance().hintlottery = true;
                        InterfaceMgr.doCommandByLua("a1_low_fightgame.open_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.RefreshHint();
                        break;
                    case 7:
                        switch(data["fund_type"]._int)
                        {

                            case 1:
                                a3_ActiveOnlineModel.getInstance().zuanshi_fundnow = data["num"];
                                break;
                            case 2:
                                a3_ActiveOnlineModel.getInstance().zhuansheng_fundnow = data["num"];
                                break;
                            case 3:
                                a3_ActiveOnlineModel.getInstance().zhanli_fundnow = data["num"];
                                break;

                        }
                        if (data.ContainsKey("changed")&&data["changed"].Count > 0)
                        {
                            for (int i = 0; i < data["changed"].Count; i++)
                            {
                                a3_ActiveOnlineModel.getInstance().Refresh_fund(data["changed"][i]["id"], data["changed"][i]["state"]);
                            }
                        }
                        //if (a3_activeOnline.instance)
                        //    a3_activeOnline.instance.RefreshFund_all();
                        if (a3_awardCenter._instance)
                            a3_awardCenter._instance.RefreshFund_all();
                        break;
                    case 8:/*点数变动的时候主动发*/
                        a3_ActiveOnlineModel.getInstance().nowpoint = data["huoyue_point"];
                        a3_ActiveOnlineModel.getInstance().Refresh_huoyue(data["active_id"], data["count"]);
                        /*有新奖励可以领*/
                        Dictionary<int, reward_info> dicss = a3_ActiveOnlineModel.getInstance().dic_reward;
                        foreach (int i in dicss.Keys)
                        {
                            if ( data["huoyue_point"] >= dicss[i].ac && dicss[i].receive_type != 2)
                            {
                                a3_ActiveOnlineModel.getInstance().hintreward = true;
                                InterfaceMgr.doCommandByLua("a1_low_fightgame.open_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                                if (a3_activeOnline.instance)
                                    a3_activeOnline.instance.RefreshHint();
                                return;
                            }
                            else
                                a3_ActiveOnlineModel.getInstance().hintreward = false;
                        }
                        if (a3_ActiveOnlineModel.getInstance().hintreward != true && a3_ActiveOnlineModel.getInstance().hintlottery != true)
                            InterfaceMgr.doCommandByLua("a1_low_fightgame.close_light_huoyue", "ui/interfaces/low/a1_low_fightgame");
                        if (a3_activeOnline.instance)
                            a3_activeOnline.instance.RefreshHint();
                        break;
                    default:
                        Globle.err_output(res);
                        return;

                }
            }
          }

    }

}
