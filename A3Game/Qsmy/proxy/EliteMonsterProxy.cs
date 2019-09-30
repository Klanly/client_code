using GameFramework;
using Cross;
using UnityEngine;
using System.Collections.Generic;

namespace MuGame
{
    class EliteMonsterProxy : BaseProxy<EliteMonsterProxy>
    {
        public static readonly uint EVENT_ELITEMONSTER = 1;
        public static readonly uint EVENT_BOSSOPSUCCESS = 3;					//世界Boss操作完成
        public static readonly uint EVENT_SHOW = 5;                                //伤害显示

        public EliteMonsterProxy()
        {
            addProxyListener(PKG_NAME.S2C_ELITE_MONSTER, EltMon_OnEMOp);
        }

        public void SendProxy()
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.S2C_ELITE_MONSTER, msg);
        }
        public void SendbuyvipProxy()
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.S2C_ELITE_MONSTER, msg);
        }
        public void EltMon_OnEMOp(Variant data)
        {
            debug.Log("收到156协议!!!!!!!!!!!!!!!!!!!!!!!" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
            }
            switch(res)
            {

                case 1:
                    A3_EliteMonsterModel.getInstance().kill_cnt = data["kill_cnt"]._int;
                    A3_EliteMonsterModel.getInstance().vip_buy_cnt = data["vip_cnt"]._int;
                    //a.死亡时发
                    if (!data["elite_mon"]._arr[0].ContainsKey("mon_x"))
                        SendProxy();
                    //b.复活时发
                    else if (!data["elite_mon"]._arr[0].ContainsKey("killer_name"))
                        SendProxy();
                    //IconAddLightMgr.getInstance().showOrHideFires("o_Light_enterElite", null);
                    //c.请求回复，都发
                    else
                        RefreshEliteMonInfo(data);
                    dispatchEvent(GameEvent.alloc(EVENT_ELITEMONSTER, this, data));
                    break;
                case 2:
                    A3_EliteMonsterModel.getInstance().kill_cnt = data["kill_cnt"]._int;
                    A3_EliteMonsterModel.getInstance().vip_buy_cnt = data["vip_cnt"]._int;
                    A3_EliteMonster.Instance?.RefreshVipCount();
                    break;
                case 4:
                    break;

                case 5:
                    for(int i=0;i<data["dmg_list"].Count;i++)
                    {
                        bossrankingdata d= new bossrankingdata();
                        d.cid = data["dmg_list"][i]["cid"]._int;
                        d.name= data["dmg_list"][i]["name"]._str;
                        d.dmg = data["dmg_list"][i]["dmg"]._uint;
                        A3_EliteMonsterModel.getInstance().dic_bsk[d.cid] = d;
                    }


                    if(!isfristover)
                    {
                        isfristover = true;
                        string path = "ui/interfaces/low/a1_low_fightgame";
                
                        InterfaceMgr.doCommandByLua("a1_low_fightgame.bossrkOp", path, null);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_BOSSRANKING);
                        //a3_expbar.instance?.BossRankingBtn.SetActive(true);
                    }
                   
                    dispatchEvent(GameEvent.alloc(EVENT_SHOW, this, data));
                    break;
                default:
                    break;


            }


             
            


        }
        public static bool isfristover = false;
        private void RefreshEliteMonInfo(Variant data)
        {
            var emonList = A3_EliteMonsterModel.getInstance().dicEMonInfo;
            List<Variant> listData;
            if (data.ContainsKey("elite_mon"))
            {
                listData = data["elite_mon"]._arr;
                for (int i = 0; i < listData.Count; i++)
                {
                    uint monId = listData[i]["mid"]._uint;

                    if (!emonList.ContainsKey(monId))
                    {
                        //emonList.Add(monId, new EliteMonsterInfo(
                        //    lastKilledDate: listData[i].ContainsKey("kill_tm") ? listData[i]["kill_tm"]._uint : 0,
                        //    respawnTime: listData[i].ContainsKey("respawntm") ? listData[i]["respawntm"]._uint : 0,
                        //    killerName: listData[i].ContainsKey("killer_name") ? listData[i]["killer_name"]._str : "",
                        //    mapId: listData[i].ContainsKey("mapid") ? listData[i]["mapid"]._int : 0,
                        //    pos: listData[i].ContainsKey("mon_x") && listData[i].ContainsKey("mon_y") ? new Vector2(listData[i]["mon_x"]._int, listData[i]["mon_y"]._int) : default(Vector2),
                        //    monId: listData[i]["mid"]._uint
                        //));
                        A3_EliteMonsterModel.getInstance().AddData(listData[i]);
                    }
                    else
                    {
                        emonList[monId] = new EliteMonsterInfo(
                            lastKilledDate: listData[i].ContainsKey("kill_tm") ? listData[i]["kill_tm"]._uint : 0,
                            respawnTime: listData[i].ContainsKey("respawntm") ? listData[i]["respawntm"]._uint : 0,
                            killerName: listData[i].ContainsKey("killer_name") ? listData[i]["killer_name"]._str : null,
                            mapId: listData[i].ContainsKey("mapid") ? listData[i]["mapid"]._int : 0,
                            pos: listData[i].ContainsKey("mon_x") && listData[i].ContainsKey("mon_y") ? new Vector2(listData[i]["mon_x"]._int, listData[i]["mon_y"]._int) : Vector2.zero,
                            monId: listData[i]["mid"]._uint
                        );
                    }
                    string name = "";
                    if (listData[i].ContainsKey("killer_name"))
                        name = listData[i]["killer_name"];
                    if (listData[i].ContainsKey("dmg_list")&& listData[i]["dmg_list"].Count>0)
                    {
                        debug.Log("伤害排行");
                        List<dmg_list> lst = new List<dmg_list>();
                        for (int j=0;j< listData[i]["dmg_list"].Count;j++)
                        {
                           
                           int ranks = j;
                            dmg_list sl = new dmg_list();
                            sl.mid= listData[i]["mid"]._int;
                            sl.cid = listData[i]["dmg_list"][j]["cid"]._int;
                            sl.name = listData[i]["dmg_list"][j]["name"]._str;
                            sl.dmg = listData[i]["dmg_list"][j]["dmg"]._int;
                            sl.rank = ranks+1;
                            sl.lat_name = name;
                            lst.Add(sl);
                            A3_EliteMonsterModel.getInstance().dic_dmg_lst[sl.mid] = lst;
                        }

                    }

                }
                //iconlight
                uint zhuan = PlayerModel.getInstance().up_lvl;
                uint lv = PlayerModel.getInstance().lvl;
                int mosid=30001;//最后一个开启的怪物id
                List<int> mids = new List<int>();
                if (data.ContainsKey("elite_mon"))
                {
                    listData = data["elite_mon"]._arr;
                    for (int i = 0; i < listData.Count; i++)
                    {
                        mids.Add(listData[i]["mid"]._int);
                    }
                   
                }
                mids.Sort();
                for (int i =0;i<mids.Count;i++)
                {
                    if(zhuan > XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mids[i]).getUint("zhuan"))
                        mosid = mids[i];
                    else if(zhuan == XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mids[i]).getUint("zhuan"))
                    {
                        if(lv >=XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mids[i]).getUint("zhuan"))
                            mosid = mids[i];
                        else{ mosid = mids[i]; break;}
                    }
                    else
                        break;
                }
                List<uint> lsts = new List<uint>();
                for(int i=0;i< mids.Count; i++)
                {
                    if (mids[i]<= mosid)
                    {
                        foreach(Variant v in listData)
                        {
                            if(v["mid"]==mids[i])
                            {
                                if (v["kill_tm"] == 0)
                                {
                                    //亮
                                    IconAddLightMgr.getInstance().showOrHideFires("Open_Light_enterElite", null);
                                    break;
                                }
                                else
                                    IconAddLightMgr.getInstance().showOrHideFires("jingyingguai_Light_enterElite", null);
                                //不亮
                            }
                        }

                    }
                }





            }
        }
    }
}

