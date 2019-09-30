using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    class a3_newActiveProxy : BaseProxy<a3_newActiveProxy>
    {

        public static uint EVENT_RANK_INFO = 0;
        public static uint EVENT_GET_REW = 1;
        public a3_newActiveProxy()
        {
            addProxyListener(PKG_NAME.C2S_NEW_ACTIVE,onInfo);
        }

        public void SendProxy(uint val)
        {
            Variant msg = new Variant();
            msg["op"] = val;
            sendRPC(PKG_NAME.C2S_NEW_ACTIVE, msg);
        }

        public void getRew(uint val,uint type,uint id)
        {
            Variant msg = new Variant();
            msg["op"] = val;
            msg["act_type"] = type;
            msg["award_id"] = id;
            sendRPC(PKG_NAME.C2S_NEW_ACTIVE, msg);
        }


        void onInfo(Variant data)
        {
            int res = data["res"];
            debug.Log("PuuPP"+data.dump ());
            switch (res)
            {
                case 1:
                    a3_newActiveModel.getInstance().recharge = data["total_recharge"];
                    a3_newActiveModel.getInstance().pay = data["total_pay"];
                    a3_newActiveModel.getInstance().pay_awd = data["pay_awd"];
                    a3_newActiveModel.getInstance().level_awd = data["level_awd"];
                    a3_newActiveModel.getInstance().combpt_awd = data["combpt_awd"];
                    a3_newActiveModel.getInstance().pk_awd = data["pk_awd"];
                    a3_newActiveModel.getInstance().recharge1_awd = data["recharge_awd"];
                    a3_newActiveModel.getInstance().wing_awd = data["wing_awd"];
                    a3_newActiveModel.getInstance().zhuan = data["zhuan"];
                    a3_newActiveModel.getInstance().lvl = data["lvl"];
                    a3_newActiveModel.getInstance().pk = data["pk"];
                    a3_newActiveModel.getInstance().wing = data["wing"];
                    a3_newActiveModel.getInstance().combpt = data["combpt"];
                    //if (data.ContainsKey ("recharge_awd")) {
                    //    a3_newActiveModel.getInstance().recharge_awd.Clear();
                    //    foreach (Variant rec in data["recharge_awd"]._arr)
                    //    {
                    //        if (!a3_newActiveModel.getInstance().recharge_awd.Contains(rec._int))
                    //        {
                    //            a3_newActiveModel.getInstance().recharge_awd.Add(rec._int);
                    //        }
                    //    }
                    //}

                    if (data.ContainsKey ("boss_kill"))
                    {
                        a3_newActiveModel.getInstance().boosKill.Clear();
                        foreach (Variant kill in data["boss_kill"]._arr)
                        {
                            a3_newActiveModel.getInstance().boosKill[kill["mid"]] = kill["num"];
                        }
                    }
                    if (data.ContainsKey("boss_award"))
                    {       
                        a3_newActiveModel.getInstance().boss_awd.Clear();
                        foreach (Variant kill in data["boss_award"]._arr)
                        {
                            if (!a3_newActiveModel.getInstance().boss_awd.Contains(kill._int))
                            {
                                a3_newActiveModel.getInstance().boss_awd.Add(kill._int);
                            }
                        }
                    }
                    break;
                case 2:
                    if (data.ContainsKey ("kaifu_tm"))
                    {
                        a3_newActiveModel.getInstance().setTime(data["kaifu_tm"]);
                    }
                    if (data.ContainsKey ("level_rank"))
                    {
                        Variant info = data["level_rank"];
                        a3_newActiveModel.getInstance().level_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.zhuan = one["zhuan"];
                            rank.lvl = one["lvl"];
                            a3_newActiveModel.getInstance().level_rank[rank.rank] = rank;
                        }
                    }
                    if (data.ContainsKey("combpt_rank"))
                    {
                        Variant info = data["combpt_rank"];
                        a3_newActiveModel.getInstance().combpt_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.combpt = one["combpt"];
                            a3_newActiveModel.getInstance().combpt_rank[rank.rank] = rank;
                        }
                    }
                    if (data.ContainsKey("pk_rank"))
                    {
                        Variant info = data["pk_rank"];
                        a3_newActiveModel.getInstance().pk_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.total_win = one["total_win"];
                            a3_newActiveModel.getInstance().pk_rank[rank.rank] = rank;
                        }
                    }
                    //if (data.ContainsKey("boss_kill_rank"))
                    //{
                    //    Variant info = data["boss_kill_rank"];
                    //    a3_newActiveModel.getInstance().boss_kill_rank.Clear();
                    //    foreach (Variant one in info._arr)
                    //    {
                    //        Rankinfo rank = new Rankinfo();
                    //        rank.cid = one["cid"];
                    //        rank.mid = one["mid"];
                    //        rank.name = one["name"];
                    //        rank.carr = one["carr"];
                    //        rank.num = one["num"];
                    //        a3_newActiveModel.getInstance().boss_kill_rank[rank.mid] = rank;
                    //    }
                    //}

                    if (data.ContainsKey("wing_rank"))
                    {
                        Variant info = data["wing_rank"];
                        a3_newActiveModel.getInstance().wing_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank  = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.stage_wing = one["stage"];
                            rank.level_wing = one["level"];
                            a3_newActiveModel.getInstance().wing_rank[rank.rank] = rank;
                        }
                    }
                    if (data.ContainsKey("pay_rank"))
                    {
                        Variant info = data["pay_rank"];
                        a3_newActiveModel.getInstance().pay_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.pay_num = one["num"];
                            a3_newActiveModel.getInstance().pay_rank[rank.rank] = rank;
                        }
                    }

                    if (data.ContainsKey("recharge_rank"))
                    {
                        Variant info = data["recharge_rank"];
                        a3_newActiveModel.getInstance().recharge_rank.Clear();
                        foreach (Variant one in info._arr)
                        {
                            Rankinfo rank = new Rankinfo();
                            rank.rank = one["rank"];
                            rank.cid = one["cid"];
                            rank.name = one["name"];
                            rank.carr = one["carr"];
                            rank.recharge_num = one["num"];
                            a3_newActiveModel.getInstance().recharge_rank[rank.rank] = rank;
                        }
                    }


                    dispatchEvent(GameEvent.Create(EVENT_RANK_INFO, this, data));
                    break;
                case 3:
                    dispatchEvent(GameEvent.Create(EVENT_GET_REW, this, data));
                    break;
                case 5:
                    if (data.ContainsKey("new_recharge"))
                    {
                        a3_newActiveModel.getInstance().recharge = data["total_recharge"];
                    }
                    if (data.ContainsKey("new_pay"))
                    {
                        a3_newActiveModel.getInstance().pay = data["total_pay"];
                    }
                    if (data.ContainsKey("boss_kill"))
                    {
                        if (data.ContainsKey("boss_kill"))
                        {
                            a3_newActiveModel.getInstance().boosKill.Clear();
                            foreach (Variant kill in data["boss_kill"]._arr)
                            {
                                a3_newActiveModel.getInstance().boosKill[kill["mid"]] = kill["num"];
                            }
                        }
                    }
                    break;

            }

            InterfaceMgr.doCommandByLua("a1_low_fightgame.canget_newact", "ui/interfaces/low/a1_low_fightgame", a3_newActiveModel.getInstance().setGet());
           
        }

    }
}
