using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class OffLineExpProxy : BaseProxy<OffLineExpProxy>
    {
        public static uint EVENT_OFFLINE_EXP_GET =118;

        private const uint C2S_OFFLINE_EXP = 118;
        private const uint S2C_OFFLINE_EXP = 118;
       public  List<a3_BagItemData> eqp = new List<a3_BagItemData>();
        /*
         * cmd = 
         * 0 获得信息
         * 1 普通经验
         * 2 双倍
         * 3 三倍
         * 4 四倍
        */
        #region client to sever msg

        //
        public OffLineExpProxy()
        {
            addProxyListener(S2C_OFFLINE_EXP, OnGetRes);
            sendType(0,false);
        }
        public void Send_Off_Line(int type)
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = 118;
            msg["option"] = (uint)type;
            sendRPC(S2C_OFFLINE_EXP, msg);
        }
        public void sendType(int type,bool fenjie)
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = 118;
            msg["option"] = (uint)type;
            msg["decompose"] = fenjie?1:0;
            sendRPC(S2C_OFFLINE_EXP, msg);
        }
       
        #endregion

        #region sever to client msg

        private void OnGetRes(Variant data)
        {
            debug.Log("获得离线经验信息::" + data.dump());
            OffLineModel expModel = OffLineModel.getInstance(); 
            int res = data["res"];            
            if (data["eqp"] != null)
            {

                OffLineModel.getInstance().IsHaveEQP = data["eqp"].Count > 0 ? true : false;
                
                foreach (Variant v in data["eqp"]._arr)
                {
                    a3_BagItemData item = new a3_BagItemData();
                    item.equipdata.gem_att = new Dictionary<int, int>();
                    item.equipdata.subjoin_att = new Dictionary<int, int>();
                    item.tpid = v["tpid"];
                    item.bnd = v["bnd"];
                    //item.isEquip = v["isEquip"];
                    item.id = v["id"];
                    item.equipdata.intensify_lv = v["intensify_lv"];
                    //item.equipdata.combpt = v["combpt"];

                    Variant att = v["gem_att"];
                   
                    foreach (Variant one in att._arr)
                    {
                        int type = one["att_type"];
                        int value = one["att_value"];                        
                        item.equipdata.gem_att[type] = value;
                    }
                    item.equipdata.add_exp = v["add_exp"];
                    item.equipdata.add_level = v["add_level"];
                    item.equipdata.blessing_lv = v["blessing_lv"];
                    Variant sub = v["subjoin_att"];
                    
                    foreach (Variant one in sub._arr)
                    {
                        int type = one["att_type"];
                        int value = one["att_value"];
                        item.equipdata.subjoin_att[type] = value;
                    }
                    item.confdata = a3_BagModel.getInstance().getItemDataById(item.tpid);
                    item.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(item));
                    eqp.Add(item);
                }
            }
            if (res < 0)
            {
                debug.Log("off_line_exp_erro::" + res);
                Globle.err_output(res);
                return;
            }

            if (data.ContainsKey("offline_tm")&&res>4)
            {
                expModel.OffLineTime = data["offline_tm"];
            }

            switch (res)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    eqp.Clear();
                    dispatchEvent(GameEvent.Create(EVENT_OFFLINE_EXP_GET, this, data));
                    break;
                default:
                    if (res == 5) { expModel.ismaxlvl = true; }
                    else { expModel.ismaxlvl = false; }
                    expModel.BaseExp = res;
                    break;
            }
        }

#endregion

       
    }
}
