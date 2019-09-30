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
    class EquipProxy : BaseProxy<EquipProxy>
    {
        public static uint EVENT_EQUIP_PUTON = 1;
        public static uint EVENT_EQUIP_PUTDOWN = 2;
        public static uint EVENT_EQUIP_STRENGTH = 3;
        public static uint EVENT_EQUIP_ADVANCE = 4;
        public static uint EVENT_EQUIP_ADDATTR = 7;
        public static uint EVENT_EQUIP_INHERIT = 6;
        public static uint EVENT_EQUIP_GEM_UP = 8;
        public static uint EVENT_CHANGE_ATT = 9;
        public static uint EVENT_DO_CHANGE_ATT = 10;
        public static uint EVENT_BAOSHI = 13;
        public static uint EVENT_SMITHY = 14;        
        public static uint ONEQUIP = 11;
        public static uint EVENT_BAOSHI_HC = 12;

        public EquipProxy()
        {
            addProxyListener(PKG_NAME.S2C_DO_EQUIP_RES, onEquip);
        }

        public void sendChangeEquip(uint id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 1;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendAdvance(uint id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 2;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendsell(List<uint> id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 5;
            msg["ids"] = new Variant();
            for (int i = 0; i < id.Count; i++)
            {
                msg["ids"].pushBack(id[i]);

            }

            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void sendsell_one(uint id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 4;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);

            debug.Log("HHH"+msg.dump ());
        }

        public void send_Changebaoshi(uint id,uint type)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 13;
            msg["eqp_id"] = id;
            msg["gem_id"] = type;
            debug.Log("eqpid"+id+"gemid"+type);
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void send_outBaoshi(uint id,uint key)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 12;
            msg["id"] = id;
            msg["idx"] = key;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void send_hcBaoshi(uint id,uint num)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 11;
            msg["tar_id"] = id;
            msg["num"] = num;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void sendAddAttr(uint id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 7;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendInherit(uint frm_id, uint to_id, int type, bool cost_yb)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 6;
            msg["frm_id"] = frm_id;
            msg["to_id"] = to_id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendInherit_baoshi(uint frm_id, uint to_id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 14;
            msg["frm_id"] = frm_id;
            msg["to_id"] = to_id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void sendAddAttr(uint id , uint num)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 15;
            msg["id"] = id;
            msg["loop"] = num;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }

        public void sendGemUp(uint id, int type)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 8;
            msg["id"] = id;
            msg["type"] = type;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendStrengthEquip(uint id)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 3;
            msg["id"] = id;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendChangeAtt(uint id, int type)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 9;
            msg["id"] = id;
            msg["type"] = type;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void sendDoChangeAtt(uint id, bool replace)
        {
            Variant msg = new Variant();
            msg["eqp_cmd"] = 10;
            msg["id"] = id;
            msg["replace"] = replace;
            sendRPC(PKG_NAME.S2C_DO_EQUIP_RES, msg);
        }
        public void onEquip(Variant data)
        {
            debug.Log("装"+data.dump());
            int res = data["res"];
            if (res <= 0)
            {
                debug.Log("错误码：" + res);
                Globle.err_output(res);
                //flytxt.instance.fly("道具不足！！！");
                return;
            }

            switch (res)
            {
                case 1:
                    if (data.ContainsKey("eqpinfo"))
                    {//穿戴
                        a3_EquipModel.getInstance().initEquipOne(data);
                        dispatchEvent(GameEvent.Create(EVENT_EQUIP_PUTON, this, data));
                    }
                    else
                    {//卸下
                        int part_id = data["part_id"];
                        a3_EquipModel.getInstance().unEquipOneByPart(data["part_id"]);
                       // a3_EquipModel.getInstance().outEquipOne(data);
                        dispatchEvent(GameEvent.Create(EVENT_EQUIP_PUTDOWN, this, data));
                        dispatchEvent(GameEvent.Create(ONEQUIP, this, data));
                    }
                    if (a3_bag.indtans)
                    {
                        a3_bag.indtans.refreshQHdashi();
                        a3_bag.indtans.refreshLHlianjie();
                    }
                    break;

                case 2://进阶
                    if (data.ContainsKey("id"))
                    {
                        uint id0 = data["id"];
                        a3_BagItemData one0 = a3_EquipModel.getInstance().getEquipByAll(id0);
                        one0.equipdata.stage = data["stage"];
                        one0.equipdata.intensify_lv = data["intensify_lv"];
                        //one0.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att (one0));
                        //one0.equipdata.combpt = data["combpt"];
                        if (a3_EquipModel.getInstance().getEquips().ContainsKey(id0))
                        {
                            a3_EquipModel.getInstance().addEquip(one0);
                        }
                        if (a3_BagModel.getInstance().getItems().ContainsKey(id0))
                        {
                            a3_BagModel.getInstance().addItem(one0);
                        }
                        //进阶时模型变化
                        if (a3_EquipModel.getInstance().getEquips().ContainsKey(one0.id))
                            a3_EquipModel.getInstance().equipModel_on(one0);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_ADVANCE, this, data));
                    break;

                case 3://强化
                    uint id1 = data["id"];
                    a3_BagItemData one1 = a3_EquipModel.getInstance().getEquipByAll(id1);
                    one1.equipdata.intensify_lv = data["intensify_lv"];
                    //one1.equipdata.combpt = data["combpt"];
                    //one1.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one1));
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id1))
                    {
                        a3_EquipModel.getInstance().addEquip(one1);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id1))
                    {
                        a3_BagModel.getInstance().addItem(one1);
                    }

                    //强化时模型变化
                    //if(a3_EquipModel.getInstance().getEquips().ContainsKey(one1.id))
                    //    a3_EquipModel.getInstance().equipModel_on(one1);

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_STRENGTH, this, data));
                    break;
                case 4:
                    if (a3_bag.indtans != null)
                        a3_bag.indtans.refresh();
                    break;
                case 5://回收
                    //if (a3_equipsell._instance != null)
                    //    a3_equipsell._instance.refresh();
                    if(piliang_fenjie.instance != null)
                        piliang_fenjie.instance.refresh();

                    break;
                case 6://传承
                    Variant frm_data = data["frm_eqpinfo"];
                    uint frm_id = frm_data["id"];
                    a3_BagItemData frm_one = a3_EquipModel.getInstance().getEquipByAll(frm_id);
                    frm_one.id = frm_data["id"];
                    frm_one.tpid = frm_data["tpid"];
                    a3_EquipModel.getInstance().equipData_read(frm_one, frm_data);
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(frm_id))
                    {
                        a3_EquipModel.getInstance().addEquip(frm_one);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(frm_id))
                    {  
                        a3_BagModel.getInstance().addItem(frm_one);
                    }

                    Variant to_data = data["to_eqpinfo"];
                    uint to_id = to_data["id"];
                    a3_BagItemData to_one = a3_EquipModel.getInstance().getEquipByAll(to_id);
                    to_one.id = to_data["id"];
                    to_one.tpid = to_data["tpid"];
                    a3_EquipModel.getInstance().equipData_read(to_one, to_data);
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(to_id))
                    {
                        a3_EquipModel.getInstance().addEquip(to_one);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(to_id))
                    {
                        a3_BagModel.getInstance().addItem(to_one);
                    }

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_INHERIT, this, data));
                    break;

                case 7:
                    uint id7 = data["id"];
                    a3_BagItemData one7 = a3_EquipModel.getInstance().getEquipByAll(id7);
                    one7.equipdata.add_exp = data["add_exp"];

                    if (one7.equipdata.add_level != data["add_level"])
                        data["do_add_level_up"] = true;
                    else
                        data["do_add_level_up"] = false;

                    one7.equipdata.add_level = data["add_level"];
                    //one7.equipdata.combpt = data["combpt"];
                   // one7.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one7));
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id7))
                    {
                        a3_EquipModel.getInstance().addEquip(one7);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id7))
                    {
                        a3_BagModel.getInstance().addItem(one7);
                    }

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_ADDATTR, this, data));
                    break;

                case 8://宝石属性
                    uint id3 = data["id"];
                    a3_BagItemData one3 = a3_EquipModel.getInstance().getEquipByAll(id3);
                    //one3.equipdata.combpt = data["combpt"];
                    if (data.ContainsKey("gem_att"))
                    {
                        int value_att = 0;
                        int type_att = 0;
                        Dictionary<int, int> old_gem = new Dictionary<int,int>();
                       // int oldvalue = one3.equipdata.gem_att[type];
                        foreach (int type in one3.equipdata.gem_att.Keys)
                        {
                            old_gem[type] = one3.equipdata.gem_att[type];
                        }
                        one3.equipdata.gem_att = new Dictionary<int, int>();
                        Variant att = data["gem_att"];
                        foreach (Variant one in att._arr)
                        {
                            int type = one["att_type"];
                            int value = one["att_value"];
                            if (value != old_gem[type])
                            {
                                value_att = value - old_gem[type];
                                type_att = type;
                            }
                            one3.equipdata.gem_att[type] = value;
                        }

                        data["att_type"] = type_att;
                        data["att_value"] = value_att;

                        //one3.equipdata.gem_att = new Dictionary<int, a3_gem_att>();
                        //Variant att = data["gem_att"];
                        //foreach (Variant one in att._arr)
                        //{
                        //    a3_gem_att gem_att = new a3_gem_att();
                        //    gem_att.att_type = one["att_type"];
                        //    gem_att.att_value = one["att_value"];
                        //    gem_att.gem_lv = one["gem_lv"];
                        //    one3.equipdata.gem_att[gem_att.att_type] = gem_att;
                        //}
                    }
                    //one3.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one3));

                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id3))
                    {
                        a3_EquipModel.getInstance().addEquip(one3);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id3))
                    {
                        a3_BagModel.getInstance().addItem(one3);
                    }

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_GEM_UP, this, data));
                    break;

                case 9://重铸
                    dispatchEvent(GameEvent.Create(EVENT_CHANGE_ATT, this, data));
                    break;

                case 10://确认重铸
                    uint id2 = data["id"];
                    a3_BagItemData one2 = a3_EquipModel.getInstance().getEquipByAll(id2);
                    //one2.equipdata.combpt = data["combpt"];
                    if (data.ContainsKey("subjoin_att"))
                    {
                        one2.equipdata.subjoin_att = new Dictionary<int, int>();
                        Variant att = data["subjoin_att"];
                        foreach (Variant one in att._arr)
                        {
                            int type = one["att_type"];
                            int value = one["att_value"];
                            one2.equipdata.subjoin_att[type] = value;
                        }
                    }
                    one2.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one2));

                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id2))
                    {
                        a3_EquipModel.getInstance().addEquip(one2);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id2))
                    {
                        a3_BagModel.getInstance().addItem(one2);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_DO_CHANGE_ATT, this, data));
                    break;

                case 11:
                    
                    break;
                case 12:
                    dispatchEvent(GameEvent.Create(EVENT_BAOSHI_HC, this, data));
                    break;
                case 13:
                    uint id13 = data["id"];
                    a3_BagItemData one13 = a3_EquipModel.getInstance().getEquipByAll(id13);
                    //one13.equipdata.combpt = data["combpt"];
                    if (data.ContainsKey("gem_att2"))
                    {
                        one13.equipdata.baoshi = new Dictionary<int, int>();
                        Variant att = data["gem_att2"];
                        int i = 0;
                        foreach (Variant one in att._arr)
                        {
                            int tpye = one["tpid"];
                            one13.equipdata.baoshi[i] = tpye;
                            i++;
                        }
                    }
                    one13.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel .getInstance ().Getequip_att (one13));

                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id13))
                    {
                        a3_EquipModel.getInstance().addEquip(one13);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id13))
                    {
                        a3_BagModel.getInstance().addItem(one13);
                    }
                    dispatchEvent(GameEvent.Create(EVENT_BAOSHI, this, data));
                    break;
                case 14:
                    Variant frm_data1 = data["frm_eqpinfo"];
                    uint frm_id1 = frm_data1["id"];
                    a3_BagItemData frm_one1 = a3_EquipModel.getInstance().getEquipByAll(frm_id1);
                    frm_one1.id = frm_data1["id"];
                    frm_one1.tpid = frm_data1["tpid"];
                    a3_EquipModel.getInstance().equipData_read(frm_one1, frm_data1);
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(frm_id1))
                    {
                        a3_EquipModel.getInstance().addEquip(frm_one1);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(frm_id1))
                    {
                        a3_BagModel.getInstance().addItem(frm_one1);
                    }

                    Variant to_data1 = data["to_eqpinfo"];
                    uint to_id1 = to_data1["id"];
                    a3_BagItemData to_one1 = a3_EquipModel.getInstance().getEquipByAll(to_id1);
                    to_one1.id = to_data1["id"];
                    to_one1.tpid = to_data1["tpid"];
                    a3_EquipModel.getInstance().equipData_read(to_one1, to_data1);
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(to_id1))
                    {
                        a3_EquipModel.getInstance().addEquip(to_one1);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(to_id1))
                    {
                        a3_BagModel.getInstance().addItem(to_one1);
                    }

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_INHERIT, this, data));
                    break;
                case 15:
                    uint id15 = data["id"];
                    a3_BagItemData one15 = a3_EquipModel.getInstance().getEquipByAll(id15);
                    one15.equipdata.add_exp = data["add_exp"];

                    if (one15.equipdata.add_level != data["add_level"])
                        data["do_add_level_up"] = true;
                    else
                        data["do_add_level_up"] = false;

                    one15.equipdata.add_level = data["add_level"];
                    if (a3_EquipModel.getInstance().getEquips().ContainsKey(id15))
                    {
                        a3_EquipModel.getInstance().addEquip(one15);
                    }
                    if (a3_BagModel.getInstance().getItems().ContainsKey(id15))
                    {
                        a3_BagModel.getInstance().addItem(one15);
                    }

                    dispatchEvent(GameEvent.Create(EVENT_EQUIP_ADDATTR, this, data));

                    break;

                //case 14: //铁匠铺打造
                //    dispatchEvent(GameEvent.Create(EVENT_SMITHY, this, data));
                //    break;
                //case 15: //铁匠铺打造学习
                //    A3_SmithyModel.AvaiablePart = data["id"]._uint;
                //    break;
            }
        }
    }
}
