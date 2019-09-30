using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using MuGame.Qsmy.model;

namespace MuGame
{
    class a3_RuneStoneProxy : BaseProxy<a3_RuneStoneProxy>
    {

        

        public static uint WEARORUNLOAD= 1;            //更换符石（穿上卸下）
        public static uint COMPOSE = 2;                //合成符石
        public static uint DECOMPOSE=3;                //分解符石
        public static uint DECOMPOSES = 4;             //批量分解符石
        public static uint INFOS = 5;                  //获取信息（等级，经验，经验值）
        public static uint ADDRUNESTONEST = 6;         //添加符石体力

        public a3_RuneStoneProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_RUNESTONE,onLoadRunestone);
        }

        public void sendporxy(int res,int id=0,int num=0,List<uint> lst_runestones=null)
        {
            Variant msg= new Variant();
            msg["op"] = res;
            switch (res)
            {
                //更换符石
                case 1:
                    msg["id"] = id;
                    break;
                //合成符石
                case 2:
                    msg["stone_tpid"] = id;
                    msg["num"] = num;
                    break; 
                case 3:
                    msg["id"] = id;
                //分解符石
                    break;
                case 4:
                    //批量分解符石
                    List<uint> a = lst_runestones;
                    msg["ids"] = new Variant();
                    for (int i = 0; i < a.Count; i++)
                    {
                        msg["ids"].pushBack(a[i]);
                        debug.Log(lst_runestones[i]+"");
                    }
                   break;
                case 5:
                //获取角色合成符石等级体力经验
                    break;
                case 6:
                    msg["id"] = id;
                    break;
                case 7:
                 //加体力
                    msg["id"] = id;
                    break;
                default:
                    break;

            }
            sendRPC(PKG_NAME.C2S_A3_RUNESTONE, msg);
        }





        public void onLoadRunestone(Variant data)
        {
            debug.Log("a3符石信息：" + data.dump());
            int res = data["res"];
            switch (res)
            {
                case 1:
                    if(data.ContainsKey("fushi"))//穿上
                    {
                        if (a3_runestone._instance != null)
                        {
                            a3_BagItemData dat = A3_RuneStoneModel.getInstance().DressupInfos(data);
                            a3_runestone._instance.DressUp(dat,dat.id);
                        }
                    }
                    else                         //脱下
                    {
                        if (a3_runestone._instance != null)
                            a3_runestone._instance.DressDown(data["part_id"]);
                    }
                                          
                    break;
                case 2:
                    if(data.ContainsKey("fushi_stamina"))
                        A3_RuneStoneModel.getInstance().nowStamina(data["fushi_stamina"]);
                    if (data.ContainsKey("fushi_level"))
                    {                       
                        if (a3_runestone._instance != null&& data["fushi_level"]!=A3_RuneStoneModel.getInstance().nowlv)
                            a3_runestone._instance.RefreScrollLv(data["fushi_level"]);
                        A3_RuneStoneModel.getInstance().nowLv(data["fushi_level"]);
                    }
                    if(data.ContainsKey("fushi_exp"))
                        A3_RuneStoneModel.getInstance().nowExp(data["fushi_exp"]);
                    dispatchEvent(GameEvent.Create(INFOS, this, data));
                    break;
                case 3:                 
                    if (a3_runestonetip._instance != null)
                    {
                        if (a3_BagModel.getInstance().getItems().ContainsKey(data["id"]))
                        {
                            a3_BagItemData b = a3_BagModel.getInstance().getItems()[data["id"]];
                            b.ismark = data["mark"];
                            a3_BagModel.getInstance().addItem(b);
                        }
                        if (a3_bag.isshow)
                            a3_bag.isshow.refreshMarkRuneStones(data["id"]);
                        if (a3_runestone._instance != null)
                            a3_runestone._instance.RefreshMark(data["id"], data["mark"]);
                    }
                    break;
                default:
                    Globle.err_output(data["res"]);
                    break;
            }
        }
    }
}
