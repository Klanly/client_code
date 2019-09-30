using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;

namespace MuGame
{
    class A3_HallowsProxy : BaseProxy<A3_HallowsProxy>
    {
        public A3_HallowsProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_HALLOWS, onLoadHallows);
        }


        public  void SendHallowsProxy(int res,int type=-1, int tpid=-1,List<Variant> lst=null)
        {
            Variant msg = new Variant();
            msg["op"] = res;
            switch (res)
            {
                case 1:
                    break;
                case 2:
                    msg["ware_type"] = type;
                    msg["add_exp"] =A3_HallowsModel.getInstance().soul_num;
                    break;
                case 3:
                    if (lst.Count <= 0)
                        return;
                    msg["items"] = new Variant();
                    for (int i = 0; i < lst.Count;i++)
                    {
                        msg["items"].pushBack(lst[i]);
                    }
                    break;
                case 4:
                    msg["tpid"] = tpid;
                    msg["ware_type"] = type;
                    break;
                case 10:
                    break;
                default:
                    break;

            }
            sendRPC(PKG_NAME.C2S_A3_HALLOWS, msg);
        }


        void onLoadHallows(Variant data)
        {
            debug.Log("受到圣器的协议：" + data.dump());
            int res = data["res"];
            switch (res)
            {
                case 1://其实是发了九个位置的信息，没发的就是默认等级（item_id是不是为0判断身上有没有穿东西）
                    A3_HallowsModel.getInstance().soul_num = data["soul_num"];
                    if (data["ware_lvl"].Count > 0)
                    {
                        for (int i = 0; i < data["ware_lvl"].Count; i++)
                        {
                            hallowsData hd = new hallowsData();
                            hd.id = data["ware_lvl"][i]["soul_type"]._int;
                            hd.item_id = data["ware_lvl"][i]["ware_tpid"]._int;
                            hd.lvl = data["ware_lvl"][i]["soul_lvl"]._int;
                            hd.exp = data["ware_lvl"][i]["soul_exp"]._int;
                            hallows_skill_data hsd = new hallows_skill_data();
                            if (hd.item_id != 0)
                            {
                                hsd = A3_HallowsModel.getInstance().GetHallowsSkillData(hd.id, hd.item_id);
                                hd.h_s_d = hsd;
                            }                          
                            A3_HallowsModel.getInstance().now_hallows_dic[hd.id] = hd;
                        }
                    }
                    break;
                case 2:
                    A3_HallowsModel.getInstance().soul_num = data["soul_num"];
                    hallowsData hds = new hallowsData();
                    hds.id = data["soul_info"]["soul_type"]._int;
                    hds.item_id = data["soul_info"]["ware_tpid"]._int;
                    hds.lvl = data["soul_info"]["soul_lvl"]._int;
                    hds.exp = data["soul_info"]["soul_exp"]._int;
                    hallows_skill_data hsds = new hallows_skill_data();
                    hsds = A3_HallowsModel.getInstance().GetHallowsSkillData(hds.id, hds.item_id);
                    hds.h_s_d = hsds;
                    A3_HallowsModel.getInstance().now_hallows_dic[hds.id] = hds;
                    if (a3_hallows.instance)
                        a3_hallows.instance.UpgradeHallows(hds.id,hds);
                    break;
                case 3:
                    A3_HallowsModel.getInstance().soul_num = data["soul_num"];
                    if (a3_hallows.instance.AllCompose)
                        a3_hallows.instance.DecomposeHallows();
                    else
                        a3_hallows.instance.DecomposeHallows(a3_hallows.instance.this_tpid);
                    break;
                case 4:                       
                    if (a3_hallows.instance)
                    {
                        if (a3_hallows.instance.PutOrDown)
                        {
                            A3_HallowsModel.getInstance().now_hallows_dic[data["soul_type"]].item_id = data["ware_tpid"];                            
                            A3_HallowsModel.getInstance().now_hallows_dic[data["soul_type"]].h_s_d = A3_HallowsModel.getInstance().GetHallowsSkillData(data["soul_type"], data["ware_tpid"]);
                            a3_hallows.instance.PutHallows(data["soul_type"], data["ware_tpid"]);
                        }
                        else
                        {
                            A3_HallowsModel.getInstance().now_hallows_dic[data["soul_type"]].item_id = 0;
                            a3_hallows.instance.DownHallows(data["soul_type"]);
                        }
                    } 
                    

                    break;
                case 10:
                    if(data["tf"]._int==0)
                   {
                        //不显示
                        A3_HallowsModel.type_duihuan = 0;
                    }
                    else if(data["tf"]._int == 1)
                    {
                        //显示
                        A3_HallowsModel.type_duihuan = 1;
                    }
                    break;

                default:
                    Globle.err_output(res);
                    return;
            }

        }
    }
}
