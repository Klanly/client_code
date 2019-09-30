using System;
using System.Collections.Generic;
using Cross;
using GameFramework;
namespace MuGame
{
    class EquipModel : ModelBase<EquipModel>
    {
        Dictionary<int, EquipData> equips;

        public EquipModel()
            : base()
        {
            if (equips == null)
                equips = new Dictionary<int, EquipData>();
        }
        public Dictionary<int, EquipData> getEquip()
        {
            return equips;
        }

        public void addEquip(EquipData data)
        {
            data.equipConf = getEquipDataById(data.tpid);
            equips[data.id] = data;
        }

        public EquipConf getEquipDataById(int id)
        {
            EquipConf conf = new EquipConf();
            conf.tpid = id;
            conf.quality = 1;
            //SXML s_xml = XMLMgr.instance.GetSXML("equip.equip_info", "id==" + id.ToString());
            //EquipConf conf = new EquipConf();
            //if (s_xml != null)
            //{
            //    conf.id = id;
            //    conf.name = s_xml.getString("equip_name");
            //    conf.equipType = s_xml.getInt("equip_type");
            //    conf.quality = s_xml.getInt("quality");
            //    conf.needLv = s_xml.getInt("level_need");
            //    SXML e_xml = s_xml.GetNode("maineffect", null);
            //    if (e_xml != null)
            //    {
            //        conf.effect = e_xml.getInt("index");
            //        conf.value = e_xml.getInt("value");
            //    }
            //}
            return conf;
        }


        public void initEquipList(List<Variant> arr)
        {
            if (equips == null)
                equips = new Dictionary<int, EquipData>();
            foreach (Variant data in arr)
            {
                EquipData equipData = new EquipData();
                equipData.id = data["equipid"];
                equipData.lv = data["level"];
                refreshEquip(equipData);
            }
        }
       
        public void refreshEquip(EquipData data)
        {
            data.equipConf = getEquipDataById(data.id);
            if (equips.ContainsKey(data.equipConf.equipType))
            {
                equips[data.equipConf.equipType] = data;
            }
            else
            {
                equips.Add(data.equipConf.equipType, data);
            }
        }
        public EquipData getNextLvEquip(int curId)
        {
            EquipData data = new EquipData();
            SXML s_xml = XMLMgr.instance.GetSXML("equip.equip_upgrade", "item_id==" + curId);
            if (s_xml != null)
            {
                SXML aa_xml = XMLMgr.instance.GetSXML("equip.equip_info", "id==" + curId);
                int id = s_xml.getInt("target_item_id");
                data.id = id;
                data.lv = aa_xml.getInt("max_strength");
                data.equipConf = getEquipDataById(data.id);
            }
            return data;
        }
      
        public EquipStengthConf getEquipStengthConf(EquipData data)
        {
            SXML s_xml = XMLMgr.instance.GetSXML("equip.equip_strengthen", "id==" + data.id.ToString());
            s_xml = s_xml.GetNode("strengthen", "level==" + (data.lv+1).ToString());
            EquipStengthConf conf = new EquipStengthConf();
            conf.id = data.id;
            conf.lv = data.lv;
            if (s_xml != null)
            {  
                conf.exp = s_xml.getInt("strengthen_exp");

                SXML bb = XMLMgr.instance.GetSXML("equip.equip_strengthen", "id==" + data.id.ToString());
                bb = bb.GetNode("strengthen", "level==" + (data.lv).ToString());
                if (data.lv != 0)
                {
                    conf.add = bb.getInt("addition");
                }
                else
                {
                    conf.add = 0;
                } 
            }

            SXML all_xml = XMLMgr.instance.GetSXML("equip.equip_strengthen", "id==" + data.id.ToString());
            all_xml = all_xml.GetNode("strengthen", null);
            if (all_xml != null)
            {
                int all = 0;
                do
                {
                    int needexp = all_xml.getInt("strengthen_exp");
                    if (all_xml.getInt("level") > data.lv)
                    {
                        all += needexp;
                    }
                    conf.maxlv = all_xml.getInt("level");
                } while (all_xml.nextOne());
                conf.maxexp = all;
            }
            return conf;
        }

        
    }
    public class EquipData
    {
        public int id; //实例id
        public int tpid; //配置id
        public bool equiped; //是否装备上了
        public int lv;
        public int addexp;
        public int addlv;
        public int stage;
        public int blessing_lv;
        public int combpt;
        public Dictionary<int, int> aad_att;
        public EquipConf equipConf = new EquipConf();
    }
    public class EquipConf
    {
        public int tpid;
        public int equipType;
        public string name;
        public int quality;
        public int needLv;
        public int effect;
        public int value;
    }
    public class EquipStengthConf
    {
        public int id;
        public int lv;
        public int exp;
        public int add;
        public int maxlv;
        public int maxexp;
    }
}
