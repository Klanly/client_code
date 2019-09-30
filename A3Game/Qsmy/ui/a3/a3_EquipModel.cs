using System;
using System.Linq;
using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;
using System.Collections;

namespace MuGame
{
    class a3_EquipModel : ModelBase<a3_EquipModel>
    {
        private Dictionary<uint, a3_BagItemData> Equips;
        private Dictionary<int, a3_BagItemData> Equips_byType;

        //装备的染色剂的初始颜色
        static public Dictionary<uint, Color> EQUIP_COLOR = new Dictionary<uint, Color>();
        public Dictionary<int, a3_BagItemData> active_eqp = new Dictionary<int, a3_BagItemData>();//int 表示部位
        public Dictionary<int, int> eqp_type_act = new Dictionary<int, int>();//值对应部位 激活 键对应部位
        public Dictionary<int, int> eqp_type_act_fanxiang = new Dictionary<int, int>(); //键对应部位 激活 值对应部位
        public Dictionary<int, int> eqp_att_act = new Dictionary<int, int>();


        private int honorpow_lvl = 0;
        public int getHonorPowlvl() {
            return honorpow_lvl;
        }
        public void setHonorPowlvl(int v) {
            if(honorpow_lvl != v)
            {
                if (honorpow_lvl < v) {
                    if (!GRMap.grmap_loading)
                    {
                        honorpow_lvl = v;
                        a3_EquipModel.getInstance().Attchange_wite = true;
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HOMORPOW_UP);
                    }
                }
                honorpow_lvl = v;
            }
        }
        public a3_EquipModel()
            : base()
        {
            Equips = new Dictionary<uint, a3_BagItemData>();
            Equips_byType = new Dictionary<int, a3_BagItemData>();

            SXML xml = XMLMgr.instance.GetSXML("item.equip_color");
            List<SXML> xml_list = xml.GetNodeList("color", null);
            if (xml_list != null)
            {
                foreach (SXML x in xml_list)
                {
                    string[] color = x.getString("color").Split(',');
                    Color col = new Color(float.Parse(color[0]) / 255, float.Parse(color[1]) / 255, float.Parse(color[2]) / 255);
                    EQUIP_COLOR[x.getUint("id")] = col;
                }
            }

            SXML xml_eqp_type = XMLMgr.instance.GetSXML("activate_fun.activate_rule");
            List<SXML> eqp_type = xml_eqp_type.GetNodeList("eqp_t");
            if (eqp_type != null)
            {
                foreach (SXML x in eqp_type)
                {
                    eqp_type_act[x.getInt("equip_type")] = x.getInt("need_type");
                    eqp_type_act_fanxiang[x.getInt("need_type")] = x.getInt("equip_type");
                }
            }
            SXML xml_eqp_att = XMLMgr.instance.GetSXML("activate_fun.attribute_rule");
            List<SXML> eqp_att = xml_eqp_att.GetNodeList("attribute");
            if (eqp_att != null)
            {
                foreach (SXML x in eqp_att)
                {
                    eqp_att_act[x.getInt("attribute_type")] = x.getInt("need_attribute");
                }
            }

        }

        public Dictionary<uint, a3_BagItemData> getEquips()
        {
            return Equips;
        }

        public Dictionary<int, a3_BagItemData> getEquipsByType()
        {
            return Equips_byType;
        }

        public void initEquipList(List<Variant> arr)
        {
            foreach (Variant data in arr)
            {
                initEquipOne(data);
            }
        }
        public void initEquipOne(Variant data)
        {
            a3_BagItemData itemData = new a3_BagItemData();
            itemData.id = data["eqpinfo"]["id"];
            itemData.tpid = data["eqpinfo"]["tpid"];
            itemData.num = data["eqpinfo"]["cnt"];
            itemData.bnd = data["eqpinfo"]["bnd"];
            itemData.isEquip = true;
            if (data["eqpinfo"].ContainsKey("mark"))
                itemData.ismark = data["eqpinfo"]["mark"];

            a3_EquipModel.getInstance().equipData_read(itemData, data["eqpinfo"]);
            itemData.confdata = a3_BagModel.getInstance().getItemDataById(itemData.tpid);
            playAudio(itemData);
            addEquip(itemData);

           setHonorPowlvl (getHonor_pow());
        }
        public void unEquipOneByPart(int part)
        {
            a3_BagItemData one = Equips_byType[part];
            Equips.Remove(one.id);
            Equips_byType.Remove(part);
            playAudio(one);
            
            if (active_eqp.ContainsKey(one.confdata.equip_type))
            {
                active_eqp.Remove(one.confdata.equip_type);
                //a3_BagItemData one1 = getEquipByAll(one.id);
                //one1.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one1));
                //if (getEquips().ContainsKey(one1.id))
                //{
                //    Equips[one1.id] = one1;
                //    //Equips_byType.Remove(part);
                //}
                //if (a3_BagModel.getInstance().getItems().ContainsKey(one1.id))
                //{
                //    a3_BagModel.getInstance().addItem(one1);
                //}
            }
            if ( eqp_type_act_fanxiang.ContainsKey( one.confdata.equip_type ) && Equips_byType.ContainsKey(eqp_type_act_fanxiang[one.confdata.equip_type]))
            {
                if (active_eqp.ContainsKey(Equips_byType[eqp_type_act_fanxiang[one.confdata.equip_type]].confdata.equip_type))
                {
                    active_eqp.Remove(Equips_byType[eqp_type_act_fanxiang[one.confdata.equip_type]].confdata.equip_type);

                    //a3_BagItemData one2 = getEquipByAll(Equips_byType[eqp_type_act_fanxiang[one.confdata.equip_type]].id);
                    //one2.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one2));
                    //if (getEquips().ContainsKey(one2.id))
                    //{
                    //    Equips[one2.id] = one2;
                    //    Equips_byType[one2.confdata.equip_type] = one2;
                    //}
                    //if (a3_BagModel.getInstance().getItems().ContainsKey(one2.id))
                    //{
                    //    a3_BagModel.getInstance().addItem(one2);
                    //}
                }
            }
            equipModel_down(one);

            setHonorPowlvl(getHonor_pow());
        }
        void playAudio(a3_BagItemData itemData)
        {
            switch (itemData.confdata.job_limit)
            {
                case 1: break;
                case 2: //战士
                    if (itemData.confdata.equip_type == 6)//武器
                    {
                        MediaClient.instance.PlaySoundUrl("audio_common_equip_aex", false, null);
                    }
                    break;
                case 3://法师
                    if (itemData.confdata.equip_type == 6)
                    {
                        MediaClient.instance.PlaySoundUrl("audio_common_equip_staff", false, null);
                    }
                    break;
                case 4: break;
                case 5://刺客
                    if (itemData.confdata.equip_type == 6)
                    {
                        MediaClient.instance.PlaySoundUrl("audio_common_equip_dagger", false, null);
                    }
                    break;
                case 6: break;
            }
            if (itemData.confdata.equip_type == 3) //铠甲
            {
                MediaClient.instance.PlaySoundUrl("audio_common_equip_armour", false, null);
            }
        }
        public void equipData_read(a3_BagItemData itemData, Variant item)
        {
            try
            {
                itemData.isEquip = true;

                if (item.ContainsKey("colour"))
                    itemData.equipdata.color = item["colour"];
                if (item.ContainsKey("intensify_lv"))
                    itemData.equipdata.intensify_lv = item["intensify_lv"];
                if (item.ContainsKey("add_level"))
                    itemData.equipdata.add_level = item["add_level"];
                if (item.ContainsKey("add_exp"))
                    itemData.equipdata.add_exp = item["add_exp"];
                if (item.ContainsKey("stage"))
                    itemData.equipdata.stage = item["stage"];
                if (item.ContainsKey("blessing_lv"))
                    itemData.equipdata.blessing_lv = item["blessing_lv"];
                if (item.ContainsKey("combpt"))
                    itemData.equipdata.baseCombpt = item["combpt"];
                if (item.ContainsKey("equip_level"))
                    itemData.equipdata.eqp_level = item["equip_level"];
                if (item.ContainsKey("equip_type"))
                    itemData.equipdata.eqp_type = item["equip_type"];
                //itemData.ismark = item["mark"];
                if (item.ContainsKey("subjoin_att"))
                {
                    itemData.equipdata.subjoin_att = new Dictionary<int, int>();
                    Variant att = item["subjoin_att"];
                    foreach (Variant one in att._arr)
                    {
                        int type = one["att_type"];
                        int value = one["att_value"];
                        itemData.equipdata.subjoin_att[type] = value;
                    }
                }
                if (item.ContainsKey("gem_att"))
                {
                    itemData.equipdata.gem_att = new Dictionary<int, int>();
                    Variant att = item["gem_att"];
                    foreach (Variant one in att._arr)
                    {
                        int type = one["att_type"];
                        int value = one["att_value"];
                        itemData.equipdata.gem_att[type] = value;
                    }

                    //itemData.equipdata.gem_att = new Dictionary<int, a3_gem_att>();
                    //Variant att = item["gem_att"];
                    //foreach (Variant one in att._arr)
                    //{
                    //    a3_gem_att gem_att = new a3_gem_att();
                    //    gem_att.att_type = one["att_type"];
                    //    gem_att.att_value = one["att_value"];
                    //    gem_att.gem_lv = one["gem_lv"];
                    //    itemData.equipdata.gem_att[gem_att.att_type] = gem_att;
                    //}
                }
                if (item.ContainsKey("honor_num"))
                {
                    itemData.equipdata.honor_num = item["honor_num"];
                }

                if (item.ContainsKey("gem_att2"))
                {
                    itemData.equipdata.baoshi = new Dictionary<int, int>();
                    Variant att = item["gem_att2"];
                    int i = 0;
                    foreach (Variant one in att._arr)
                    {
                        int type = one["tpid"];
                        itemData.equipdata.baoshi[i] = type;
                        i++;
                    }
                }
                if (item.ContainsKey("prefix_name"))
                {
                    //itemData.equipdata.attribute = item["prefix_name"];
                    SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                    SXML s_xml = itemsXMl.GetNode("item", "id==" + itemData.tpid);

                    SXML xml = XMLMgr.instance.GetSXML("activate_fun.eqp", "eqp_type==" + s_xml.getInt("equip_type"));
                    SXML x = xml.GetNode("prefix_fun", "id==" + item["prefix_name"]);
                    //itemData.equipdata.attribute = x.getInt("id");
                    itemData.equipdata.attribute = item[ "prefix_name" ];
                    itemData.equipdata.att_type = x.getInt("funtype");
                    itemData.equipdata.att_value = item["att_value"];
                }
                //if (item.ContainsKey("extra_att"))
                //{
                //    itemData.equipdata.extra_att = new Dictionary<int, int>();
                //    Variant att = item["extra_att"];
                //    foreach (Variant one in att._arr)
                //    {
                //        int type = one["att_type"];
                //        int value = one["att_value"];
                //        itemData.equipdata.extra_att[type] = value;
                //    }
                //}
                itemData.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(itemData));
            }
            catch(Exception e)
            {
                Debug.LogError("Message=" + e.Message);
                Debug.LogError("StackTrace=" + e.StackTrace);
            }

            //return itemData;
        }
        public bool checkisSelfEquip(a3_ItemData data)
        {
            bool res = false;
            //检测是否可以穿戴
            if (PlayerModel.getInstance().profession == data.job_limit || data.job_limit == 1)
            {
                res = true;
            }
            return res;
        }
        public bool checkCanEquip(a3_BagItemData data)
        {
            return checkCanEquip(data.confdata, data.equipdata.stage, data.equipdata.blessing_lv);
        }
        public bool checkCanEquip(a3_ItemData data, int stage, int blessing_lv)
        {
            bool res = false;
            SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + stage);

            if (stage_xml == null)
                return false;
            else
                stage_xml = stage_xml.GetNode("stage_info", "itemid==" + data.tpid);
            SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + blessing_lv);
            string[] list_need1 = stage_xml.getString("equip_limit1").Split(',');
            string[] list_need2 = stage_xml.getString("equip_limit2").Split(',');

            int need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            int need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;

            if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])]
                && need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])]
                && stage <= PlayerModel.getInstance().up_lvl)
            {
                res = true;
            }
            return res;
        }
        public bool CanInherit(uint eqp1,uint eqp2)
        {
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_INHERITANCE)) { return false; }
            a3_BagItemData data1 = getEquipByAll(eqp1);
            a3_BagItemData data2 = getEquipByAll(eqp2);
            //string[] list_need1;
            //string[] list_need2;
            //int need1;
            //int need2;

            //SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + (data1.equipdata.stage)).GetNode("stage_info", "itemid==" + data1.tpid);
            //SXML blessing = XMLMgr.instance.GetSXML("item.blessing", "blessing_level==" + data1.equipdata.blessing_lv);
            //list_need1 = stage_xml.getString("equip_limit1").Split(',');
            //list_need2 = stage_xml.getString("equip_limit2").Split(',');
            //need1 = int.Parse(list_need1[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            //need2 = int.Parse(list_need2[1]) * (100 - blessing.getInt("blessing_att")) / 100;
            //if (need1 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need1[0])]) {  } else {  }
            //if (need2 <= PlayerModel.getInstance().attr_list[uint.Parse(list_need2[0])]) {} else {  }



            if (data1.confdata.equip_type != data2.confdata.equip_type)
            return false;
            else if (data1.equipdata.stage * 15 + data1.equipdata.intensify_lv >= data2.equipdata.stage * 15 + data2.equipdata.intensify_lv)
                return false;
            else if (data1.equipdata.add_level > data2.equipdata.add_level)
                return false;
            else return true;
        }

        public bool CanInherit_baoshi(uint eqp1, uint eqp2)
        {
            if (eqp1 == eqp2)
                return false;
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.EQP_INHERITANCE)) { return false; }
            a3_BagItemData data1 = getEquipByAll(eqp1);
            a3_BagItemData data2 = getEquipByAll(eqp2);
            if (data1.confdata.equip_type != data2.confdata.equip_type)
                return false;
            else if (data2.equipdata.baoshi.Count <= 0)
                return false;
            else if (data1.equipdata.baoshi.Count <= 0)
                return false;
            else if (data1.equipdata.baoshi.Count > 0 && data2.equipdata.baoshi.Count > 0)
            {
                bool have_bs = false;
                foreach (int cell in data2.equipdata.baoshi.Keys)
                {
                    if (data2.equipdata.baoshi [cell] > 0)
                    { have_bs = true;
                        break; }
                }
                foreach (int cell in data1.equipdata.baoshi.Keys)
                {
                    if (data1.equipdata.baoshi[cell] <= 0 && have_bs) return true;
                }
                return false;
            }
            return true;
        }


        public bool Attchange_wite = false;
        public void addEquip(a3_BagItemData data)
        {
            int change_type = 0;
            if (Equips_byType.ContainsKey(data.confdata.equip_type))
            {//如果同一部位有装备了，应当移除掉之前的装备
                if (CanInherit(data.id, Equips_byType[data.confdata.equip_type].id))
                {
                    ArrayList l = new ArrayList();
                    l.Add(Equips_byType[data.confdata.equip_type].id);
                    l.Add(data.id);
                    l.Add(inheritType.eqp);
                    a3_EquipModel.getInstance().Attchange_wite = true;
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQPINHERIT, l);
                }
                else if (CanInherit_baoshi(data.id, Equips_byType[data.confdata.equip_type].id))
                {
                    ArrayList l = new ArrayList();
                    l.Add(Equips_byType[data.confdata.equip_type].id);
                    l.Add(data.id);
                    l.Add(inheritType.baoshi);
                    a3_EquipModel.getInstance().Attchange_wite = true;
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQPINHERIT, l);
                }
                change_type = 2;
                if (Equips_byType[data.confdata.equip_type].id != data.id)
                {
                    a3_BagModel.getInstance().gheqpData(Equips_byType[data.confdata.equip_type], data);
                    if (a3_bag.isshow)
                    {
                        a3_bag.indtans.ghuaneqp(Equips_byType[data.confdata.equip_type], data);
                    }
                }
                Equips.Remove(Equips_byType[data.confdata.equip_type].id);
                if (active_eqp.ContainsKey(data.confdata.equip_type))
                {
                    active_eqp.Remove(data.confdata.equip_type);
                    //a3_BagItemData one = data;
                    //one.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one));
                    //if (getEquips().ContainsKey(one.id))
                    //{
                    //    Equips[one.id] = one;
                    //    Equips_byType[one.confdata.equip_type] = one;
                    //}
                    //if (a3_BagModel.getInstance().getItems().ContainsKey(one.id))
                    //{
                    //    a3_BagModel.getInstance().addItem(one);
                    //}
                }
                if ( eqp_type_act_fanxiang.ContainsKey( data.confdata.equip_type ) && Equips_byType.ContainsKey(eqp_type_act_fanxiang[data.confdata.equip_type]))
                {
                    if (active_eqp.ContainsKey(Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]].confdata.equip_type))
                    {
                        active_eqp.Remove(Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]].confdata.equip_type);
                        //a3_BagItemData one1 = getEquipByAll(Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]].id);
                        //one1.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one1));
                        //if (getEquips().ContainsKey(one1.id))
                        //{
                        //    Equips[one1.id] = one1;
                        //    Equips_byType[one1.confdata.equip_type] = one1;
                        //}
                        //if (a3_BagModel.getInstance().getItems().ContainsKey(one1.id))
                        //{
                        //    a3_BagModel.getInstance().addItem(one1);
                        //}
                    }
                }
            }
            Equips[data.id] = data;
            Equips_byType[data.confdata.equip_type] = data;

            
            A3_BeStronger.Instance?.CheckUpItem();

            if (isActive_eqp(data))
            {
                active_eqp[data.confdata.equip_type] = data;
                //a3_BagItemData one2 = getEquipByAll(data.id);
                //one2.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one2));
                //if (getEquips().ContainsKey(one2.id))
                //{
                //    Equips[one2.id] = one2;
                //    Equips_byType[one2.confdata.equip_type] = one2;
                //}
                //if (a3_BagModel.getInstance().getItems().ContainsKey(one2.id))
                //{
                //    a3_BagModel.getInstance().addItem(one2);
                //}
            }
            if ( eqp_type_act_fanxiang.ContainsKey( data.confdata.equip_type ) && Equips_byType.ContainsKey(eqp_type_act_fanxiang[data.confdata.equip_type]))
            {
                if (isActive_eqp(Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]]))
                {
                    active_eqp[eqp_type_act_fanxiang[data.confdata.equip_type]] = Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]];
                    //a3_BagItemData one3 = getEquipByAll(Equips_byType[eqp_type_act_fanxiang[data.confdata.equip_type]].id);
                    //one3.equipdata.combpt = a3_BagModel.getInstance().Getcombpt(a3_BagModel.getInstance().Getequip_att(one3));
                    //if (getEquips().ContainsKey(one3.id))
                    //{
                    //    Equips[one3.id] = one3;
                    //    Equips_byType[one3.confdata.equip_type] = one3;
                    //}
                    //if (a3_BagModel.getInstance().getItems().ContainsKey(one3.id))
                    //{
                    //    a3_BagModel.getInstance().addItem(one3);
                    //}
                }
            }

            equipModel_on(data);
        }


        public int getHonor_pow() {
            int lvl = 0;
            int honor_mun = 0;
            foreach (a3_BagItemData it in getEquips().Values)
            {
                if (a3_EquipModel.getInstance().checkCanEquip(it))
                    honor_mun = it.equipdata.honor_num + honor_mun;
            }
            SXML xml = XMLMgr.instance.GetSXML("strength_of_honor");
            List<SXML> list = xml.GetNodeList("strength");
            for (int i = 1; i <= list.Count; i++)
            {
                if (honor_mun < list[i - 1].getInt("value"))
                {
                    lvl = i - 1;
                    break;
                }
            }
            return lvl;

        }

        public bool isActive_eqp(a3_BagItemData data) //检测该装备是否被激活
        {
            if (data.equipdata.attribute == 0)
                return false;
            int needTpye_act = eqp_type_act[data.confdata.equip_type];
            if (!Equips_byType.ContainsKey(needTpye_act))
                return false;
            int needatt = eqp_att_act[data.equipdata.attribute];
            if (Equips_byType[needTpye_act].equipdata.attribute == needatt)
                return true;
            else
                return false;
        }
        public void equipModel_on(a3_BagItemData data)
        {
            //没装备有时装
           // debug.Log("dsefsdffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
            if (data == null)
            {
                if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                    SelfRole._inst.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                else
                    SelfRole._inst.set_body(0, 0);
                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                {
                    switch (PlayerModel.getInstance().profession)
                    {
                        case 2:
                        case 3:
                            SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            break;
                        case 5:
                            SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            break;
                    }
                }
                else
                {
                    switch (PlayerModel.getInstance().profession)
                    {
                        case 2:
                        case 3:
                            SelfRole._inst.set_weaponr(0, 0);
                            break;
                        case 5:
                            SelfRole._inst.set_weaponl(0, 0);
                            SelfRole._inst.set_weaponr(0, 0);
                            break;
                    }
                }
            }
            else
            {
                int bodyid = -1;
                int bodyFxid = -1;
                bool ishavebody = false;
                //穿戴装备model变化
                foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                {
                    if(equip.confdata.equip_type == 3)
                    {
                        ishavebody = true;
                        break;
                    }
                }
                if (ishavebody /*|| data.confdata.equip_type == 11*/ )
                {
                    if(  SelfRole._inst != null && data.confdata.equip_type == 3)
                    {
                         bodyid = (int)data.tpid;
                         bodyFxid = data.equipdata.stage;
                        if (data.confdata.equip_type == 3 && a3_bag.F_equipInfo != null)
                        {
                            bodyid = (int)a3_bag.F_equipInfo.tpid;
                            bodyFxid = a3_bag.F_equipInfo.equipdata.stage;
                        }
                        SelfRole._inst.m_roleDta.m_BodyID = bodyid;
                        SelfRole._inst.m_roleDta.m_BodyFXID = bodyFxid;
                        if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                            SelfRole._inst.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                        else
                            SelfRole._inst.set_body(bodyid, bodyFxid);


                        //添加动画重绑
                        SelfRole._inst.rebind_ani();

                        uint colorid = data.equipdata.color;
                        SelfRole._inst.m_roleDta.m_EquipColorID = colorid;
                        SelfRole._inst.set_equip_color(colorid);
                    }

                }
                else
                {
                    if (SelfRole._inst != null)
                    {

                        if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                            SelfRole._inst.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                        else
                            SelfRole._inst.set_body(0, 0);

                    }
                }




                int weaponid = -1;
                int weaponFxid = -1;
                bool ishaveequ = false;
                foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
                {
                    if (equip.confdata.equip_type == 6)
                    {
                        ishaveequ = true;
                        break;
                    }
                }
                if (ishaveequ/*|| data.confdata.equip_type == 12 */)
                {

                    if (SelfRole._inst != null && data.confdata.equip_type == 6)
                    {


                         weaponid = (int)data.tpid;
                         weaponFxid = data.equipdata.stage;

                        if (data.confdata.equip_type == 6 && a3_bag.F_weaponInfo != null)
                        {
                            weaponid = (int)a3_bag.F_weaponInfo.tpid;
                            weaponFxid = a3_bag.F_weaponInfo.equipdata.stage;
                        }

                        switch (PlayerModel.getInstance().profession)
                        {
                            case 2:
                                SelfRole._inst.m_roleDta.m_Weapon_RID = weaponid;
                                SelfRole._inst.m_roleDta.m_Weapon_RFXID = weaponFxid;
                                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0 )
                                    SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                else
                                    SelfRole._inst.set_weaponr(weaponid, weaponFxid);
                                break;
                            case 3:
                                SelfRole._inst.m_roleDta.m_Weapon_LID = weaponid;
                                SelfRole._inst.m_roleDta.m_Weapon_LFXID = weaponFxid;
                                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                                    SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                else
                                    SelfRole._inst.set_weaponl(weaponid, weaponFxid);
                                break;
                            case 5:
                                SelfRole._inst.m_roleDta.m_Weapon_LID = weaponid;
                                SelfRole._inst.m_roleDta.m_Weapon_LFXID = weaponFxid;
                                SelfRole._inst.m_roleDta.m_Weapon_RID = weaponid;
                                SelfRole._inst.m_roleDta.m_Weapon_RFXID = weaponFxid;
                                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                                {
                                    SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                    SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                }
                                else
                                {
                                    SelfRole._inst.set_weaponl(weaponid, weaponFxid);
                                    SelfRole._inst.set_weaponr(weaponid, weaponFxid);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if(SelfRole._inst!=null)
                    {
                        if (weaponid == -1)
                        {
                            switch (PlayerModel.getInstance().profession)
                            {
                                case 2:
                                case 3:
                                    if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                                        SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                    else
                                       SelfRole._inst.set_weaponr(0, 0);
                                    break;
                                case 5:
                                    if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                                    {
                                        SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                        SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                                    }
                                    else
                                    {
                                        SelfRole._inst.set_weaponl(0, 0);
                                        SelfRole._inst.set_weaponr(0, 0);
                                    }

                                    break;
                            }
                        }
                    }

                }
                if (SelfRole._inst != null)
                    SelfRole._inst.clear_eff();
                if (active_eqp.Count >= 10 && SelfRole._inst != null)
                {
                    SelfRole._inst.set_equip_eff(GetEqpIdbyType(3), false);
                }
                if (SelfRole._inst != null)
                    SelfRole._inst.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(active_eqp.Count));
            }
        }

        public int GetEqpIdbyType(int type)//根据部位获取身上该部位装备id
        {
            if (!Equips_byType.ContainsKey(type))
            {
                return -1;
            }
            else {
                return (int)Equips_byType[type].tpid;
            }
        }

        //public int Getlvl_up(a3_BagItemData data) 
        //{
        //    int lvl_up = 0;
        //    SXML sxml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + data.equipdata.stage);
        //    List<SXML> xml_list = sxml.GetNodeList("stage_info", null);
        //    if (xml_list != null)
        //    {
        //        foreach (SXML x in xml_list)
        //        {
        //            if (data.tpid == x.getInt("itemid")) 
        //            {
        //                lvl_up = x.getInt("zhuan");
        //            }
        //        }
        //    }
        //    return lvl_up;
        //}
        public int Getlvl_up(a3_ItemData data, int stage)
        {
            int lvl_up = 0;
            SXML sxml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + stage);
            List<SXML> xml_list = sxml.GetNodeList("stage_info", null);
            if (xml_list != null)
            {
                foreach (SXML x in xml_list)
                {
                    if (data.tpid == x.getInt("itemid"))
                    {
                        lvl_up = x.getInt("zhuan");
                    }
                }
            }
            return lvl_up;
        }
        public void equipModel_down(a3_BagItemData data)
        {
            //卸下装备model变化
            if ( data.confdata.equip_type == 3 && SelfRole._inst != null && a3_bag.F_equipInfo == null )
            {
                int bodyid = a3_bag.F_equipInfo != null ? (int) a3_bag.F_equipInfo.tpid :0;
                int bodyFxid = a3_bag.F_equipInfo != null ? a3_bag.F_equipInfo.equipdata.stage:0;

                SelfRole._inst.m_roleDta.m_BodyID = bodyid;
                SelfRole._inst.m_roleDta.m_BodyFXID = bodyFxid;
                if (A3_FashionShowModel.getInstance().first_nowfs[1] == 0)
                    SelfRole._inst.set_body(bodyid, bodyFxid);
                else
                    SelfRole._inst.set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);

                //添加动画重绑
                SelfRole._inst.rebind_ani();

                uint colorid = a3_bag.F_equipInfo != null ?  a3_bag.F_equipInfo.equipdata.color : 0;

                if ( colorid != 0 )
                    SelfRole._inst.m_roleDta.m_EquipColorID = colorid;

                SelfRole._inst.set_equip_color( colorid );
            }/* else if ( data.confdata.equip_type == 11 && SelfRole._inst != null ) {
              
                int bodyid = a3_bag._equipInfo != null ? (int) a3_bag._equipInfo.tpid :0;
                int bodyFxid = a3_bag._equipInfo != null ? a3_bag._equipInfo.equipdata.stage:0;
                SelfRole._inst.m_roleDta.m_BodyID = bodyid;
                SelfRole._inst.m_roleDta.m_BodyFXID = bodyFxid;
                SelfRole._inst.set_body( bodyid , bodyFxid );

                //添加动画重绑
                SelfRole._inst.rebind_ani();

                uint colorid = a3_bag._equipInfo != null ?  a3_bag._equipInfo.equipdata.color : 0;

                if (colorid != 0)
                SelfRole._inst.m_roleDta.m_EquipColorID = colorid;

                SelfRole._inst.set_equip_color( colorid );

            }*/
            if (data.confdata.equip_type == 6 && SelfRole._inst != null && a3_bag.F_weaponInfo == null )
            {
                switch (PlayerModel.getInstance().profession)
                {
                    case 2:
                        SelfRole._inst.m_roleDta.m_Weapon_RID = 0;
                        SelfRole._inst.m_roleDta.m_Weapon_RFXID = 0;
                        if (A3_FashionShowModel.getInstance().first_nowfs[0] == 0)
                            SelfRole._inst.set_weaponr(0, 0);
                        else
                            SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                        break;
                    case 3:
                        SelfRole._inst.m_roleDta.m_Weapon_LID = 0;
                        SelfRole._inst.m_roleDta.m_Weapon_LID = 0;
                        if (A3_FashionShowModel.getInstance().first_nowfs[0] == 0)
                            SelfRole._inst.set_weaponl(0, 0);
                        else
                            SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                        break;
                    case 5:
                        SelfRole._inst.m_roleDta.m_Weapon_LID = 0;
                        SelfRole._inst.m_roleDta.m_Weapon_LFXID = 0;
                        SelfRole._inst.m_roleDta.m_Weapon_RID = 0;
                        SelfRole._inst.m_roleDta.m_Weapon_RFXID = 0;
                        if (A3_FashionShowModel.getInstance().first_nowfs[0] == 0)
                        {
                            SelfRole._inst.set_weaponl(0, 0);
                            SelfRole._inst.set_weaponr(0, 0);
                        }
                        else
                            SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                        {
                            SelfRole._inst.set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                            SelfRole._inst.set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                        }
                        break;
                }
            }
          /*  else if( data.confdata.equip_type == 12 && SelfRole._inst != null )
            {
                int weaponid =a3_bag._weaponInfo != null ? (int)a3_bag._weaponInfo.tpid : 0;
                int weaponFxid =a3_bag._weaponInfo != null? a3_bag._weaponInfo.equipdata.stage : 0;
                switch ( PlayerModel.getInstance().profession )
                {
                    case 2:
                    SelfRole._inst.m_roleDta.m_Weapon_RID = weaponid;
                    SelfRole._inst.m_roleDta.m_Weapon_RFXID = weaponFxid;
                    SelfRole._inst.set_weaponr( weaponid , weaponFxid );
                    break;
                    case 3:
                    SelfRole._inst.m_roleDta.m_Weapon_LID = weaponid;
                    SelfRole._inst.m_roleDta.m_Weapon_LFXID = weaponFxid;
                    SelfRole._inst.set_weaponl( weaponid , weaponFxid );
                    break;
                    case 5:
                    SelfRole._inst.m_roleDta.m_Weapon_LID = weaponid;
                    SelfRole._inst.m_roleDta.m_Weapon_LFXID = weaponFxid;
                    SelfRole._inst.m_roleDta.m_Weapon_RID = weaponid;
                    SelfRole._inst.m_roleDta.m_Weapon_RFXID = weaponFxid;
                    SelfRole._inst.set_weaponl( weaponid , weaponFxid );
                    SelfRole._inst.set_weaponr( weaponid , weaponFxid );
                    break;
                }
            }*/
            if (SelfRole._inst != null)
                SelfRole._inst.clear_eff();
            if (active_eqp.Count >= 10 && SelfRole._inst != null)
            {
                SelfRole._inst.set_equip_eff(GetEqpIdbyType(3), false);
            }
            SelfRole._inst.set_equip_eff(a3_EquipModel .getInstance ().GetEff_lvl (active_eqp.Count));
        }

        public int GetEff_lvl( int activecount)
        {
            int lvl = 0;
            SXML active_fun = XMLMgr.instance.GetSXML("activate_fun.eff");
            List<SXML> eff = active_fun.GetNodeList("effect");
            for ( int i=0;i<eff.Count;i++ )
            {
                if (activecount < eff[i].getInt("e"))
                {
                    lvl = i;
                    break;
                }
                else if (activecount == eff[i].getInt("e"))
                {
                    lvl = i + 1;
                    break;
                }
                else {
                    continue;
                }
            }
            return lvl;
        }

        public void equipColor_on(uint id)
        {
            SelfRole._inst.m_roleDta.m_EquipColorID = id;
            SelfRole._inst.set_equip_color(id);
        }

        public a3_BagItemData getEquipByAll(uint id)
        {
            if (Equips.ContainsKey(id))
            {
                return Equips[id];
            }
            else if (a3_BagModel.getInstance().getItems().ContainsKey(id))
            {
                return a3_BagModel.getInstance().getItems()[id];
            }
            else
            {
                return new a3_BagItemData();
            }
        }


        public a3_EquipData getEquipByItemId(uint id)
        {
            a3_EquipData ed = new a3_EquipData();
            SXML sxml = XMLMgr.instance.GetSXML("item.item", "id==" + id);

            ed.tpid = id;
            ed.color = sxml.getUint("colour");
            ed.intensify_lv = sxml.getInt("intensify_lv");
            ed.add_level = sxml.getInt("add_level");
            ed.add_exp = sxml.getInt("add_exp");
            ed.stage = sxml.getInt("stage");
            ed.blessing_lv = sxml.getInt("blessing_lv");
           // ed.combpt = sxml.getInt("combpt");
            ed.eqp_level = sxml.getInt("equip_level");
            ed.eqp_type = sxml.getInt("equip_type");
            ed.subjoin_att = new Dictionary<int, int>();
            //Variant att = sxml.m_dAtttr["subjoin_att"];
            //foreach (Variant one in att._arr)
            //{
            //    int type = one["att_type"];
            //    int value = one["att_value"];
            //    ed.subjoin_att[type] = value;
            //}
            ed.gem_att = new Dictionary<int, int>();
            //att = sxml.getInt("gem_att");
            //foreach (Variant one in att._arr)
            //{
            //    int type = one["att_type"];
            //    int value = one["att_value"];
            //    ed.gem_att[type] = value;

            //}
            ed.attribute = sxml.getInt("prefix_name");
            SXML itemsXMl = XMLMgr.instance.GetSXML("item");
            SXML s_xml = itemsXMl.GetNode("item", "id==" + id);
            SXML xml = XMLMgr.instance.GetSXML("activate_fun.eqp", "eqp_type==" + s_xml.getInt("equip_type"));
            SXML x = xml?.GetNode("prefix_fun", "id==" + sxml.getInt("prefix_name"));
            ed.att_type = x?.getInt("funtype") ?? 0;
            ed.att_value = sxml.getInt("att_value");
            return ed;
        }

        public List<a3_EquipData> GetEquipListByEquipType(int equipType)
        {
            List<a3_EquipData> equipList = new List<a3_EquipData>();
            List<SXML> nodeList = null;
            if (equipType != 0)
                nodeList = XMLMgr.instance.GetSXML("item").GetNodeList("item", "equip_type==" + equipType);
            else
            {
                nodeList?.Clear();
                nodeList = new List<SXML>();
                List<int> listPart = A3_Smithy.Instance.listPartIdx;
                for (int i=0; i < listPart.Count; i++)
                    nodeList.AddRange(XMLMgr.instance.GetSXML("item").GetNodeList("item", "equip_type=="+ listPart[i]));
            }
            for (int i = 0; i < nodeList?.Count; i++)
            {
                uint eqpId = nodeList[i].getUint("id");
                equipList.Add(getEquipByItemId(eqpId));
            }
            return equipList;
        }

        public Dictionary<bool, EquipStrengthOption> CheckEquipStrengthAvailable()
        {
            Dictionary<bool, EquipStrengthOption> result = new Dictionary<bool, EquipStrengthOption>
            {
                [true] = EquipStrengthOption.None,
                [false] = EquipStrengthOption.None
            };
            result[CheckUp(Equips, EquipStrengthOption.Intensify)] |= EquipStrengthOption.Intensify;
            result[CheckUp(Equips, EquipStrengthOption.Stage)] |= EquipStrengthOption.Stage;
            result[CheckUp(Equips, EquipStrengthOption.Add)] |= EquipStrengthOption.Add;
            //result[CheckUp(Equips, EquipStrengthOption.Gem)] |= EquipStrengthOption.Gem;
            return result;
        }

        private bool CheckUp(Dictionary<uint, a3_BagItemData> equips, EquipStrengthOption checkOption)
        {
            List<uint> itemsId = new List<uint>(equips.Keys);
            switch (checkOption)
            {
                case EquipStrengthOption.None:
                default:
                    //Debug.LogError("不匹配的类型,请检查你的代码,此处将跳过检测");
                    break;
                #region 检查装备是否满足强化条件
                case EquipStrengthOption.Intensify:
                    for (int i = 0; i < itemsId.Count; i++)
                    {
                        uint targetIntensifyLv = (uint)equips[itemsId[i]].equipdata.intensify_lv + 1;
                        if (!(a3_BagModel.getInstance().EqpIntensifyLevelInfo.ContainsKey(targetIntensifyLv)))
                            continue;
                        int moneyRequired = a3_BagModel.getInstance().EqpIntensifyLevelInfo[targetIntensifyLv].intensifyCharge;
                        Dictionary<uint, int> dicMat = a3_BagModel.getInstance().EqpIntensifyLevelInfo[targetIntensifyLv].intensifyMaterials;
                        List<uint> listMatId = new List<uint>(dicMat.Keys);
                        if (!(PlayerModel.getInstance().money < moneyRequired))
                        {
                            int j;
                            for (j = 0; j < listMatId.Count; j++)
                            {
                                if (a3_BagModel.getInstance().getItemNumByTpid(listMatId[j]) < dicMat[listMatId[j]])
                                {
                                    break;
                                }
                            }
                            if (listMatId.Count == j)
                                return true;
                        }
                    }
                    break;
                #endregion
                #region 检查装备是否满足进阶条件
                case EquipStrengthOption.Stage:
                    for (int i = 0; i < itemsId.Count; i++)
                    {
                        int targetEquipStageLv = equips[itemsId[i]].equipdata.stage + 1;
                        List<Dictionary<uint, EqpStageLvInfo>> eqpstageinfo = a3_BagModel.getInstance().EqpStageInfo;
                        if (eqpstageinfo.Count < targetEquipStageLv + 1) // 装备是否已经到达最大等阶
                            continue;
                        EqpStageLvInfo eqpStageLvInfo = eqpstageinfo[equips[itemsId[i]].equipdata.stage][equips[itemsId[i]].tpid];
                        if (!(PlayerModel.getInstance().up_lvl < eqpStageLvInfo.reincarnation
                            || PlayerModel.getInstance().lvl < eqpStageLvInfo.lv
                            || PlayerModel.getInstance().money < eqpStageLvInfo.upstageCharge)) // 人物转生、等级能否进阶装备
                        {
                            List<A3_CharacterAttribute> listAtt = new List<A3_CharacterAttribute>(eqpStageLvInfo.equipLimit.Keys);
                            bool canEquip = true;
                            for (int j = 0; j < listAtt.Count; j++) // 进阶后人物能否穿戴
                            {
                                A3_CharacterAttribute limitAttType = listAtt[j];
                                switch (limitAttType)
                                {
                                    default:
                                        Debug.LogError(string.Format("配置表信息错误<装备进阶>:未知的人物属性Id:{0}", (int)limitAttType));
                                        canEquip = false;
                                        break;
                                    case A3_CharacterAttribute.Agility:
                                        canEquip &= PlayerModel.getInstance().agility >= eqpStageLvInfo.equipLimit[limitAttType]; ;
                                        break;
                                    case A3_CharacterAttribute.Strength:
                                        canEquip &= PlayerModel.getInstance().strength >= eqpStageLvInfo.equipLimit[limitAttType]; ;
                                        break;
                                    case A3_CharacterAttribute.Constitution:
                                        canEquip &= PlayerModel.getInstance().constitution >= eqpStageLvInfo.equipLimit[limitAttType]; ;
                                        break;
                                    case A3_CharacterAttribute.Intelligence:
                                        canEquip &= PlayerModel.getInstance().intelligence >= eqpStageLvInfo.equipLimit[limitAttType];
                                        break;
                                    case A3_CharacterAttribute.Wisdom:
                                        canEquip &= PlayerModel.getInstance().wisdom >= eqpStageLvInfo.equipLimit[limitAttType]; ;
                                        break;
                                }
                            }
                            if (canEquip)
                            {
                                int j;
                                List<int> listMat = new List<int>(eqpStageLvInfo.upstageMaterials.Keys);
                                for (j = 0; j < listMat.Count; j++)
                                    if (a3_BagModel.getInstance().getItemNumByTpid((uint)listMat[j]) < eqpStageLvInfo.upstageMaterials[listMat[j]])
                                        break;
                                if (listMat.Count == j)
                                    return true;
                            }
                        }
                    }
                    break;
                #endregion
                #region 检查装备是否满足追加条件
                case EquipStrengthOption.Add:
                    for (int i = 0; i < itemsId.Count; i++)
                    {
                        EqpStageLvInfo eqpStgInfo = a3_BagModel.getInstance().EqpStageInfo
                            [equips[itemsId[i]].equipdata.stage][equips[itemsId[i]].tpid];
                        if (!(equips[itemsId[i]].equipdata.add_level < eqpStgInfo.maxAddLv))
                            continue; // 装备是否可以继续追加
                        uint eqpAddLv = (uint)equips[itemsId[i]].equipdata.add_level;
                        if (!a3_BagModel.getInstance().EqpAddInfo.ContainsKey(eqpAddLv))
                            continue;
                        EqpAddConfInfo dicAdd = a3_BagModel.getInstance().EqpAddInfo[eqpAddLv];
                        if (PlayerModel.getInstance().money <= dicAdd.addCharge)
                            continue; // 玩家能否支付追加费用
                        if (a3_BagModel.getInstance().getItemNumByTpid(dicAdd.matId) <= dicAdd.matNum)
                            continue; // 玩家能否提供足够的追加所需材料
                        return true;
                    }
                    break;
                #endregion
                #region 检查装备是否可以镶嵌宝石
                case EquipStrengthOption.Gem:
                    for (int i = 0; i < itemsId.Count; i++)
                    {
                        a3_EquipData equipData = equips[itemsId[i]].equipdata;
                        List<EqpGemConfInfo> listGem = a3_BagModel.getInstance().EqpGemInfo[equips[itemsId[i]].equipdata.stage][equips[itemsId[i]].tpid];
                        int j = default(int);
                        try
                        {
                            for (j = 0; j < listGem.Count; j++)
                                if (equipData.gem_att[listGem[j].attType] < listGem[j].attMax
                                    && a3_BagModel.getInstance().getItemNumByTpid(listGem[j].gemId) >= listGem[j].gemNeedNum)
                                    return true;
                        }
                        catch (Exception)
                        {
                            Debug.LogError(string.Format("item.xml<宝石镶嵌>信息有误:物品id:{0}", equips[itemsId[i]].tpid));
                        }
                    }
                    break;
                    #endregion
            }
            return false;
        }

        [Flags]
        public enum EquipStrengthOption
        {
            None = 0x00,
            Intensify = 0x01,
            Stage = 0x02,
            Add = 0x04,
            Gem = 0x08
        }
    }
}
