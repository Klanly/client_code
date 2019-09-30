using System;
using System.Linq;
using System.Collections.Generic;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;
using System.Collections;
using UnityEngine;
namespace MuGame
{
    class a3_BagModel : ModelBase<a3_BagModel>
    {
        public static uint EVENT_EQUIP_ADD = 0;

        SXML itemsXMl;
        public int curi = 0;
        public int house_curi = 0;
        public int m_all_curi = 150; //满格
        public  Dictionary<uint, a3_BagItemData> Items;
        private Dictionary<uint, a3_BagItemData> RunestoneItems;//背包中的符石
        private Dictionary<uint, a3_BagItemData> hallows_items;//背包中的圣器
        private Dictionary<uint, a3_BagItemData> UnEquips; //没有穿身上的装备
        private Dictionary<uint, a3_BagItemData> HouseItems; //仓库
        private Dictionary<uint /*item id*/ , EqpIntensifyLvInfo /*level info*/ > eqpIntensifyLevelInfo;
        public Dictionary<uint, EqpIntensifyLvInfo> EqpIntensifyLevelInfo => eqpIntensifyLevelInfo;
        private List<Dictionary<uint /*item id*/ , EqpStageLvInfo /*stage info*/ >> eqpStageInfo;
        public List<Dictionary<uint, EqpStageLvInfo>> EqpStageInfo => eqpStageInfo;
        private Dictionary<uint /*add lv*/ , EqpAddConfInfo /*add info*/ > eqpAddInfo;
        public Dictionary<uint, EqpAddConfInfo> EqpAddInfo => eqpAddInfo;
        private List<Dictionary<uint /*item id*/ , List<EqpGemConfInfo>> /* gem config*/ > eqpGemInfo;
        public List<Dictionary<uint, List<EqpGemConfInfo>>> EqpGemInfo => eqpGemInfo;
        TickItem process_cd;
        private Dictionary<int, float> item_cds;
        private List<int> item_remove_cds;
        private List<int> item_reduce_cds;
        public bool isFirstMark = true;
        public Dictionary<uint, a3_BagItemData> neweqp = new Dictionary<uint, a3_BagItemData>();
        public Dictionary<uint, a3_BagItemData> newshow_item = new Dictionary<uint, a3_BagItemData>();
        public Dictionary<uint, a3_BagItemData> newshow_summon = new Dictionary<uint, a3_BagItemData>();

        public List<int> jilu = new List<int>();

        //public Dictionary<uint, a3_BagItemData> item_num = new Dictionary<uint, a3_BagItemData>();

        public a3_BagModel() : base()
        {
            itemsXMl = XMLMgr.instance.GetSXML("item");
            if (itemsXMl == null) return;

            Items = new Dictionary<uint, a3_BagItemData>();
            RunestoneItems = new Dictionary<uint, a3_BagItemData>();
            hallows_items = new Dictionary<uint, a3_BagItemData>();
            UnEquips = new Dictionary<uint, a3_BagItemData>();
            HouseItems = new Dictionary<uint, a3_BagItemData>();
            item_cds = new Dictionary<int, float>();
            item_remove_cds = new List<int>();
            item_reduce_cds = new List<int>();
            eqpIntensifyLevelInfo = new Dictionary<uint, EqpIntensifyLvInfo>();
            eqpStageInfo = new List<Dictionary<uint, EqpStageLvInfo>>();
            eqpAddInfo = new Dictionary<uint, EqpAddConfInfo>();
            eqpGemInfo = new List<Dictionary<uint, List<EqpGemConfInfo>>>();
            InitEqpIntensifyLv();
            InitEqpStageInfo();
            InitEqpAddInfo();
            InitEqpGemInfo();
            allRunestoneData();
        }

        public SXML getItemXml(int tpid)
        {
            return itemsXMl.GetNode("item", "id==" + tpid);
        }
        private void InitEqpIntensifyLv()
        {
            List<SXML> intensifyInfo = itemsXMl.GetNodeList("intensify");
            for (int i = 0; i < intensifyInfo.Count; i++)
            {
                Dictionary<uint, int> matDic = new Dictionary<uint, int>();
                string[] materialsInfo = {
                    intensifyInfo[i].getString("intensify_material1"),
                    intensifyInfo[i].getString("intensify_material2"),
                    intensifyInfo[i].getString("intensify_material3")
                };

                for (int j = 0; j < materialsInfo.Length; j++)
                {
                    string[] str_matInfo = materialsInfo[j].Split(',');
                    if (!matDic.ContainsKey(uint.Parse(str_matInfo[0])))
                        matDic.Add(
                           key: /*item id*/  uint.Parse(str_matInfo[0]),
                           value: /*item num*/ int.Parse(str_matInfo[1])
                        );
                    else
                        Debug.LogError("item.xml:配置表信息错误,intensify_material字段格式不正确!");
                }

                eqpIntensifyLevelInfo.Add
                (
                    key: intensifyInfo[i].getUint("intensify_level"),
                    value: new EqpIntensifyLvInfo
                    {
                        intensifyCharge = intensifyInfo[i].getInt("intensify_money"),
                        intensifyMaterials = matDic
                    }
                );
            }
        }
        private void InitEqpStageInfo()
        {
            int chatAttCount = Enum.GetNames(typeof(A3_CharacterAttribute)).Length;
            List<SXML> stageLevelInfoList = itemsXMl.GetNodeList("stage");
            List<SXML> itemList = itemsXMl.GetNodeList("item");
            Dictionary<uint /*item id*/, int /* basic level*/> basicLvAddDic = new Dictionary<uint, int>();
            for (int i = 0; i < itemList.Count; i++)
            {
                uint itemId = itemList[i].getUint("id");
                if (!basicLvAddDic.ContainsKey(itemId))
                    basicLvAddDic.Add(itemId, itemList[i].getInt("add_basiclevel"));
                else
                    Debug.LogError(string.Format("item.xml:配置表信息错误,物品id重复,重复的id为{0}!", itemId));
            }
            for (int stageLv = 0; stageLv < stageLevelInfoList.Count; stageLv++)
            {
                eqpStageInfo.Add(new Dictionary<uint, EqpStageLvInfo>());
                List<SXML> stageItemInfoList = stageLevelInfoList[stageLv].GetNodeList("stage_info");
                for (int i = 0; i < stageItemInfoList.Count; i++)
                {
                    Dictionary<A3_CharacterAttribute, int> limitDic = new Dictionary<A3_CharacterAttribute, int>();
                    string[] strInfo;
                    for (int j = 1; j < chatAttCount /* 角色属性总类别数 */; j++)
                    {
                        if ((strInfo = stageItemInfoList[i].getString("equip_limit" + j)?.Split(',')).Length == 2)
                            limitDic.Add
                            (
                                key: /*人物属性*/ (A3_CharacterAttribute)Enum.Parse(typeof(A3_CharacterAttribute), strInfo?[0]),
                                value: /*所需属性点数*/ int.Parse(strInfo?[1])
                            );
                        else
                            break;
                    }
                    Dictionary<int, int> upstageMatDic = new Dictionary<int, int>();
                    List<SXML> matList = stageItemInfoList[i].GetNodeList("stage_material");
                    for (int j = 0; j < matList.Count; j++)
                    {
                        int _itemId = matList[j].getInt("item");
                        if (_itemId > 0)
                            if (!upstageMatDic.ContainsKey(_itemId))
                                upstageMatDic.Add(_itemId, matList[j].getInt("num"));
                            else
                                upstageMatDic[_itemId] += matList[j].getInt("num");
                    }
                    uint itemId = stageItemInfoList[i].getUint("itemid");
                    if (!eqpStageInfo[stageLv].ContainsKey(itemId))
                        eqpStageInfo[stageLv].Add
                        (
                            key: itemId,
                            value: new EqpStageLvInfo
                            {
                                reincarnation = stageItemInfoList[i].getUint("zhuan"),
                                lv = stageItemInfoList[i].getUint("level"),
                                upstageCharge = stageItemInfoList[i].getUint("stage_money"),
                                equipLimit = limitDic,
                                upstageMaterials = upstageMatDic,
                                maxAddLv = basicLvAddDic[itemId] * stageLv
                            }
                        );
                    else
                        Debug.LogError(string.Format("item.xml:配置表信息不正确,装备等阶信息重复,重复id:{0}", itemId));
                }
            }
        }
        private void InitEqpAddInfo()
        {
            uint addLv = default(uint);
            List<SXML> addInfoList = itemsXMl.GetNodeList("add_att");
            for (int i = 0; i < addInfoList.Count; i++)
            {
                addLv = addInfoList[i].getUint("add_level");
                if (!eqpAddInfo.ContainsKey(addLv))
                    eqpAddInfo.Add
                    (
                        key: addLv,
                        value: new EqpAddConfInfo
                        {
                            addCharge = addInfoList[i].getInt("money"),
                            matId = addInfoList[i].getUint("material_id"),
                            matNum = addInfoList[i].getUint("material_num")
                        }
                    );
                else
                    Debug.LogError(string.Format("item.xml:配置表信息错误,装备追加等级信息重复,重复的等级：{0}", addLv));
            }

        }
        private void InitEqpGemInfo()
        {
            List<SXML> gemInfoList = itemsXMl.GetNodeList("gem");
            for (int stageLv = 0; stageLv < eqpStageInfo.Count/* 0 ~ 15 */; stageLv++)
            {
                List<EqpGemConfInfo> _eqpGemConfInfo = new List<EqpGemConfInfo>();
                Dictionary<uint, List<EqpGemConfInfo>> _eqpGemConfInfoDic = new Dictionary<uint, List<EqpGemConfInfo>>();
                for (int i = 0; i < gemInfoList.Count; i++)
                {
                    uint itemId = gemInfoList[i].getUint("item_id"); // 1.item id of each stage
                    List<SXML> _gemConfInfo = gemInfoList[i].GetNodeList("gem_info.gem_att");
                    for (int j = 0; j < _gemConfInfo.Count/* = 3 */; j++)
                        _eqpGemConfInfo.Add
                        (
                            item: new EqpGemConfInfo
                            {
                                attType = _gemConfInfo[j].getInt("att_type"),
                                attMax = _gemConfInfo[j].getUint("att_max"),
                                gemId = _gemConfInfo[j].getUint("need_itemid"),
                                gemNeedNum = _gemConfInfo[j].getUint("need_value")
                            }
                        ); // 2.gem config for each item id
                    _eqpGemConfInfoDic.Add(key: itemId, value: _eqpGemConfInfo); // 3.item config of item id
                }
                eqpGemInfo.Add(_eqpGemConfInfoDic); // 4.all item configs of each stage
            }
        }
        public void initItemList(List<Variant> arr)
        {
            if (Items == null)
                Items = new Dictionary<uint, a3_BagItemData>();
            foreach (Variant data in arr)
                initItemOne(data);
        }

        public void addItemCd(int type, float time)
        {
            if (process_cd == null)
            {
                process_cd = new TickItem(onUpdateCd);
                TickMgr.instance.addTick(process_cd);
            }
            item_cds[type] = time;
        }

        public void gheqpData(a3_BagItemData add, a3_BagItemData rem)
        {
            Items[add.id] = add;
            UnEquips[add.id] = add;
            removeItem(rem.id);
        }
        public Dictionary<int, float> getItemCds() => item_cds;
        void onUpdateCd(float s)
        {
            foreach (int type in item_cds.Keys)
            {
                item_reduce_cds.Add(type);
                if (item_cds[type] <= 0)
                    item_remove_cds.Add(type);
            }
            foreach (int type in item_reduce_cds)
            {
                item_cds[type] = item_cds[type] - s;
            }
            foreach (int type in item_remove_cds)
            {
                item_cds.Remove(type);
            }
            item_reduce_cds.Clear();
            item_remove_cds.Clear();
            if (item_cds.Count == 0)
            {
                TickMgr.instance.removeTick(process_cd);
                process_cd = null;
            }
        }

        public void initItemOne(Variant data)
        {
            a3_BagItemData itemData = new a3_BagItemData();
            itemData.id = data["id"];
            itemData.tpid = data["tpid"];
            itemData.num = data["cnt"];
            itemData.bnd = data["bnd"];
            itemData.isEquip = false;
            itemData.isNew = false;
            if (data.ContainsKey("intensify_lv"))
            {
                a3_EquipModel.getInstance().equipData_read(itemData, data);
            }
            a3_BagModel.getInstance().addItem(itemData);
        }

        public Dictionary<uint, a3_BagItemData> getItems(bool sort = false)
        {
            if (!sort)
                return Items;
            else
            {
                List<a3_BagItemData> itemList = new List<a3_BagItemData>();
                itemList.AddRange(Items.Values);
                itemList.Sort();
                Dictionary<uint, a3_BagItemData> SortItems = new Dictionary<uint, a3_BagItemData>();
                foreach (a3_BagItemData data in itemList)
                {
                    SortItems[data.id] = data;
                }
                return SortItems;
            }
        }

        public bool isMine(uint id ) {
            if (Items.ContainsKey(id)) return true;
            if (a3_EquipModel.getInstance().getEquips().ContainsKey(id)) return true;
            if (getHouseItems().ContainsKey(id)) return true;
            return false;
        }


        public Dictionary<uint, a3_BagItemData> getHouseItems() => HouseItems;
        public Dictionary<uint, a3_BagItemData> getUnEquips() => UnEquips;
        public Dictionary<uint, a3_BagItemData> getRunestonrs() => RunestoneItems;
        public Dictionary<uint, a3_BagItemData> getHallows() => hallows_items;

        public int get_item_num(uint id)
        {
            int num = 0;
            //if (item_num.ContainsKey(id))
            //{
            //    num = item_num[id].num;
            //}
            return num;
        }
        public void addItem(a3_BagItemData data)
        {
            if (data.isSummon)
            {
                SXML sumXml = XMLMgr.instance.GetSXML("callbeast.callbeast", "id==" + data.summondata.tpid);
                SXML attxml = sumXml.GetNode("star", "star_sum==" + data.summondata.star);
                int tpid = attxml.getInt("info_itm");
                data.confdata = getItemDataById((uint)tpid);
            }
            else
                data.confdata = getItemDataById(data.tpid);

            //if (Items.ContainsKey(data.id))
            //{
            //    if (data.num == 0)
            //    {
            //        Items.Remove(data.id);
            //    }
            //    else
            //    {
            //        Items[data.id] = data;
            //    }
            //}
            //else
            //{
            //    Items.Add(data.id, data);
            //}
            Items[data.id] = data;

            if (data.isEquip)
            {
                UnEquips[data.id] = data;
                if (SelfRole.fsm.Autofighting)
                {
                    StatePick.Instance.AutoEquipProcess(data);
                }

                //if (a3_equipup.instance != null)
                //    a3_equipup.instance.checkShow(data);
                isgoodeqp(data);
            }
            if (data.confdata.use_type == 13 || data.confdata.use_type == 20)
            {
                addlibao(data);
            }
            if (data.isSummon && (A3_TaskModel.getInstance().main_task_id == 704 || A3_TaskModel.getInstance().main_task_id == 703))
            { 
                addSummon(data);
            }

            if (a3_expbar.instance != null)
            {
                a3_expbar.instance.bag_Count();
            }
            if (data.confdata.use_type == 22)
            {
                RunestoneItems[data.id] = data;
                if(a3_runestone._instance != null)
                    a3_runestone._instance.addHaveRunestones(data);
            }
            if (XMLMgr.instance.GetSXML("item.item", "id==" + data.confdata.tpid).getInt("isholic") != -1)
            {
                data.ishallows = true;
                hallows_items[data.id] = data;

            }
            //符石的材料
            if(data.tpid>=1633&&data.tpid<=1637)
            {
                if(a3_runestone._instance!=null)
                    a3_runestone._instance.refreshHvaeMaterialNum();
            }
            A3_BeStronger.Instance?.CheckUpItem();
        }

        #region 使用提示窗口
        void isgoodeqp(a3_BagItemData data)
        {
            if (!data.isEquip)
                return;
            if (!data.isNew)
                return;
            int EqpType = AutoPlayModel.getInstance().EqpType;
            if (a3_EquipModel.getInstance().checkisSelfEquip(data.confdata)
            && a3_EquipModel.getInstance().checkCanEquip(data))
            {
                if (!SelfRole.fsm.Autofighting || EqpType <= 0)
                {
                    if (!a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type))
                    {
                        foreach (a3_BagItemData it in neweqp.Values)
                        {
                            if (data.confdata.equip_type == it.confdata.equip_type)
                            {
                                if (data.equipdata.combpt > it.equipdata.combpt)
                                {
                                    neweqp[data.id] = data;
                                    neweqp.Remove(it.id);
                                    //if (it.id == a3_equipup.instance.nowShow)
                                    {
                                        a3_equipup.instance.showUse();
                                    }
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        neweqp[data.id] = data;
                        a3_equipup.instance.showUse();
                    }
                    else
                    {
                        a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
                        if (data.equipdata.combpt > have_one.equipdata.combpt)
                        {
                            foreach (a3_BagItemData it in neweqp.Values)
                            {
                                if (data.confdata.equip_type == it.confdata.equip_type)
                                {
                                    if (data.equipdata.combpt > it.equipdata.combpt)
                                    {
                                        neweqp[data.id] = data;
                                        neweqp.Remove(it.id);
                                        //if (it.id == a3_equipup.instance.nowShow)
                                        {
                                            a3_equipup.instance.showUse();
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                            neweqp[data.id] = data;
                            a3_equipup.instance.showUse();
                        }
                    }
                }
                else if (SelfRole.fsm.Autofighting && EqpType > 0)
                {
                    int eqpproc = AutoPlayModel.getInstance().EqpProc;

                        for (int i = 0; i < 5; i++)
                        {
                            if ((eqpproc & (1 << i)) == 0 && data.confdata.quality == i + 1)
                            {
                                if (!a3_EquipModel.getInstance().getEquipsByType().ContainsKey(data.confdata.equip_type))
                                {
                                    foreach (a3_BagItemData it in neweqp.Values)
                                    {
                                        if (data.confdata.equip_type == it.confdata.equip_type)
                                        {
                                            if (data.equipdata.combpt > it.equipdata.combpt)
                                            {
                                                neweqp[data.id] = data;
                                                neweqp.Remove(it.id);
                                                //if (it.id == a3_equipup.instance.nowShow)
                                                {
                                                    a3_equipup.instance.showUse();
                                                }
                                                return;
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    neweqp[data.id] = data;
                                    a3_equipup.instance.showUse();
                                }
                                else
                                {
                                    a3_BagItemData have_one = a3_EquipModel.getInstance().getEquipsByType()[data.confdata.equip_type];
                                    if (data.equipdata.combpt > have_one.equipdata.combpt)
                                    {
                                        foreach (a3_BagItemData it in neweqp.Values)
                                        {
                                            if (data.confdata.equip_type == it.confdata.equip_type)
                                            {
                                                if (data.equipdata.combpt > it.equipdata.combpt)
                                                {
                                                    neweqp[data.id] = data;
                                                    neweqp.Remove(it.id);
                                                    //if (it.id == a3_equipup.instance.nowShow)
                                                    {
                                                        a3_equipup.instance.showUse();
                                                    }
                                                    return;
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                        neweqp[data.id] = data;
                                        a3_equipup.instance.showUse();
                                    }
                                }
                            }
                        }
                }
            }
        }

        void addlibao(a3_BagItemData data)
        {
            if (PlayerModel.getInstance().up_lvl > data.confdata.use_limit)
            {
                newshow_item[data.id] = data;
            }
            if (PlayerModel.getInstance().up_lvl == data.confdata.use_limit)
            {
                if (PlayerModel.getInstance().lvl >= data.confdata.use_lv)
                {
                    newshow_item[data.id] = data;
                }
            }
            if (a3_equipup.instance != null)
            {
                a3_equipup.instance.showUse();
            }
        }

        void addSummon(a3_BagItemData data)
        {
            newshow_summon[data.id] = data;
            if (a3_equipup.instance != null)
            {
                a3_equipup.instance.showUse();
            }
        }
        #endregion

        public void addHouseItem(a3_BagItemData data)
        {
            data.confdata = getItemDataById(data.tpid);
            HouseItems[data.id] = data;
        }

        public void removeItem(uint id)
        {
            if (Items.ContainsKey(id))
            {
                Items.Remove(id);
            }

            if (UnEquips.ContainsKey(id))
            {
                UnEquips.Remove(id);
            }
            if (neweqp.ContainsKey(id))
            {
                neweqp.Remove(id);
                if (id == a3_equipup.instance.nowShow)
                {
                    a3_equipup.instance.showUse();
                }
            }

            if (newshow_item.ContainsKey(id))
            {
                newshow_item.Remove(id);
                if (id == a3_equipup.instance.nowShow)
                {
                    a3_equipup.instance.showUse();
                }
            }
            if (newshow_summon.ContainsKey(id)) {
                newshow_summon.Remove(id);
                if (id == a3_equipup.instance.nowShow)
                {
                    a3_equipup.instance.showUse();
                }
            }

            if (a3_expbar.instance != null)
            {
                a3_expbar.instance.bag_Count();
            }
            if (RunestoneItems.ContainsKey(id))
            {
                RunestoneItems.Remove(id);
                if (a3_runestone._instance != null)
                    a3_runestone._instance.removeHaveRunestones(id);
            }
            if(hallows_items.ContainsKey(id))
            {
                hallows_items.Remove(id);
            }
            if (getItemDataById((uint)id).tpid >= 1633 && getItemDataById((uint)id).tpid <= 1637)
            {
                if (a3_runestone._instance!=null)
                {
                    a3_runestone._instance.refreshHvaeMaterialNum();
                }
            }
        }

        public void removeHouseItem(uint id)
        {
            if (HouseItems.ContainsKey(id))
            {
                HouseItems.Remove(id);
            }
        }

        public a3_BagItemData getequipToUp(int type)
        {
            a3_BagItemData maxGoodEqp = null;
            foreach (a3_BagItemData item in UnEquips.Values)
            {
                if (item.confdata.equip_type == type)
                {
                    if (a3_EquipModel.getInstance().checkisSelfEquip(item.confdata)
                            && a3_EquipModel.getInstance().checkCanEquip(item))
                    {
                        if (maxGoodEqp == null) {
                            maxGoodEqp = item;
                        }
                        else
                        {
                            if (item.equipdata.combpt > maxGoodEqp.equipdata.combpt)
                            {
                                maxGoodEqp = item;
                            }
                        }
                    }
                }
            }

            if (maxGoodEqp == null) return null;
            else
            {
                if (a3_EquipModel.getInstance().getEquipsByType().ContainsKey(type))
                {
                    if (maxGoodEqp.equipdata.combpt > a3_EquipModel.getInstance().getEquipsByType()[type].equipdata.combpt)
                    {
                        return maxGoodEqp;
                    }
                    else
                    {
                        return null; 
                    }
                }
                else {
                    return maxGoodEqp;
                }
            }
            return null;
        }


        public a3_ItemData getItemDataById(uint tpid)
        {
            a3_ItemData item = new a3_ItemData();
            item.tpid = tpid;
            SXML s_xml = itemsXMl.GetNode("item", "id==" + tpid);
            if (s_xml != null)
            {
                item.file = "icon_item_" + s_xml.getString("icon_file");
                item.borderfile = "icon_itemborder_b039_0" + s_xml.getString("quality");
                item.item_name = s_xml.getString("item_name");
                item.quality = s_xml.getInt("quality");
                item.desc = s_xml.getString("desc");
                item.desc2 = s_xml.getString("desc2");
                item.maxnum = s_xml.getInt("maxnum");
                item.value = s_xml.getInt("value");
                item.use_lv = s_xml.getInt("use_lv");
                item.use_limit = s_xml.getInt("use_limit");
                item.use_type = s_xml.getInt("use_type");
                int score = s_xml.getInt("intensify_score");
                item.intensify_score = score;
                item.item_type = s_xml.getInt("item_type");
                if (s_xml.getInt("sort_type") < 0)
                {
                    item.sortType = 9999;
                }
                else
                {
                    item.sortType = s_xml.getInt("sort_type");
                }
                item.equip_type = s_xml.getInt("equip_type");
                item.equip_level = s_xml.getInt("equip_level");
                item.job_limit = s_xml.getInt("job_limit");
                item.modelId = s_xml.getInt("model_id");
                item.on_sale = s_xml.getInt("on_sale");
                item.cd_type = s_xml.getInt("cd_type");
                item.cd_time = s_xml.getFloat("cd");
                item.main_effect = s_xml.getInt("main_effect");
                item.add_basiclevel = s_xml.getInt("add_basiclevel");
                item.use_sum_require = s_xml.getInt("use_sum_require");
                item.zhSummon = s_xml.getInt("summon");
            }
            return item;
        }


        public int Getcombpt(Dictionary <int,int> att)
        {
            int combft = 0;
            SXML score_xml = XMLMgr.instance.GetSXML("score");
            List<SXML> per_list = score_xml.GetNodeList("grade");
            foreach (int type in att.Keys) {
                foreach (SXML node in per_list)
                {
                    if (node.getInt("att_id") == type) {
                        combft = combft + (att[type]* node.getInt ("per")/ 10000);
                        //Debug.LogError("评分" + combft);
                        break;
                    }
                }

            }

          // Debug.LogError("评分"+combft);
            return combft;
        }

        public Dictionary<int,int> Getequip_att(a3_BagItemData equip_data) {
            Dictionary<int, int> att = new Dictionary<int, int>();

            try
            {
                SXML item_xml = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
                SXML stage_xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).GetNode("stage_info", "itemid==" + equip_data.tpid);
                string[] list_att0 = XMLMgr.instance.GetSXML("item.stage", "stage_level==0").GetNode("stage_info", "itemid==" + equip_data.tpid).getString("basic_att").Split(',');
                string[] list_att2 = stage_xml.getString("basic_att").Split(',');
                if (equip_data.confdata.equip_type <= 0)
                {
                    equip_data.confdata.equip_type = item_xml.getInt("equip_type");
                }
                equip_data.confdata.equip_level = item_xml.getInt("equip_level");
                if (!att.ContainsKey(100)) att[100] = equip_data.confdata.equip_level;
                if (list_att0.Length > 1)//基础
                {
                    if (att.ContainsKey(38))
                        att[38] += int.Parse(list_att0[0]);
                    else
                        att[38] = int.Parse(list_att0[0]);
                    if (att.ContainsKey(5))
                        att[5] += int.Parse(list_att0[1]);
                    else
                        att[5] = int.Parse(list_att0[1]);
                }
                else
                {
                    if (att.ContainsKey(item_xml.getInt("att_type")))
                        att[item_xml.getInt("att_type")] += int.Parse(list_att0[0]);
                    else
                        att[item_xml.getInt("att_type")] = int.Parse(list_att0[0]);
                }
                //if (equip_data.equipdata.intensify_lv > 0)//强化
                //{
                //    SXML intensify_xml = XMLMgr.instance.GetSXML("item.intensify", "intensify_level==" + equip_data.equipdata.intensify_lv).GetNode("intensify_info", "itemid==" + equip_data.tpid);
                //    string[] list_att1 = intensify_xml.getString("intensify_att").Split(',');
                //    int lv = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + equip_data.equipdata.stage).getInt("extra");
                //    for (int i = 1; i <= equip_data.equipdata.intensify_lv; i++) {
                //        if (list_att1.Length > 1)
                //        {
                //            if (att.ContainsKey(38)) att[38] += int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                //            else att[38] = int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                //            if (att.ContainsKey(5)) att[5] += int.Parse(intensify_xml.getString("intensify_att").Split(',')[1]) * lv / 100;
                //            else att[5] = int.Parse(intensify_xml.getString("intensify_att").Split(',')[1]) * lv / 100;
                //        }
                //        else {
                //            if (att.ContainsKey(item_xml.getInt("att_type"))) att[item_xml.getInt("att_type")] += int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                //            else att[item_xml.getInt("att_type")] = int.Parse(intensify_xml.getString("intensify_att").Split(',')[0]) * lv / 100;
                //        }
                //    }
                //}

                //if (equip_data.equipdata.stage > 0)//精炼
                //{
                //    if (list_att0.Length > 1)
                //    {
                //        if (att.ContainsKey(38)) att[38] += int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                //        else att[38] = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);

                //        if (att.ContainsKey(5)) att[5] += int.Parse(list_att2[1]) - int.Parse(list_att0[1]);
                //        else att[5] = int.Parse(list_att2[1]) - int.Parse(list_att0[1]);
                //    }
                //    else
                //    {
                //        if (att.ContainsKey(item_xml.getInt("att_type"))) att[item_xml.getInt("att_type")] += int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                //        else att[item_xml.getInt("att_type")] = int.Parse(list_att2[0]) - int.Parse(list_att0[0]);
                //    }
                //}

                //附加
                SXML subjoin_xml = XMLMgr.instance.GetSXML("item.subjoin_att", "equip_level==" + equip_data.confdata.equip_level);
                foreach (int type in equip_data.equipdata.subjoin_att.Keys)
                {
                    if (att.ContainsKey(type)) att[type] += equip_data.equipdata.subjoin_att[type];
                    else att[type] = equip_data.equipdata.subjoin_att[type];
                }
                //宝石
                if (equip_data.equipdata.baoshi != null)
                {
                    if (!att.ContainsKey(97)) att[97] = equip_data.equipdata.baoshi.Count;
                    //foreach (int i in equip_data.equipdata.baoshi.Keys)
                    //{
                    //    if (equip_data.equipdata.baoshi[i] > 0)
                    //    {
                    //        SXML _itemsXMl = XMLMgr.instance.GetSXML("item");
                    //        SXML s_xml1 = _itemsXMl.GetNode("gem_info", "item_id==" + equip_data.equipdata.baoshi[i]);
                    //        List<SXML> gem_add = s_xml1.GetNodeList("gem_add");
                    //        foreach (SXML x in gem_add)
                    //        {
                    //            if (x.getInt("equip_type") == equip_data.confdata.equip_type)
                    //            {
                    //                if (att.ContainsKey(x.getInt("att_type"))) att[x.getInt("att_type")] += x.getInt("att_value");
                    //                else att[x.getInt("att_type")] = x.getInt("att_value");
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                }
                //追加
                //SXML add_xml2 = XMLMgr.instance.GetSXML("item.item", "id==" + equip_data.tpid);
                //int attType = int.Parse(add_xml2.getString("add_atttype").Split(',')[0]);
                //int attValue = int.Parse(add_xml2.getString("add_atttype").Split(',')[1]) * equip_data.equipdata.add_level;
                //if (att.ContainsKey(attType)) att[attType] += attValue;
                //else att[attType] = attValue;

                //灵魂
                //if((a3_EquipModel.getInstance()?.active_eqp != null 
                //    && a3_EquipModel.getInstance()?.active_eqp.Count > 0)
                //    ||( a3_targetinfo.isshow?.active_eqp != null 
                //    && a3_targetinfo.isshow?.active_eqp .Count > 0))
                //{
                //    Dictionary<int, a3_BagItemData> active_eqp;
                //    if (a3_targetinfo.isshow)
                //    {
                //        active_eqp = a3_targetinfo.isshow.active_eqp;
                //    }
                //    else
                //    {
                //        active_eqp = a3_EquipModel.getInstance().active_eqp;
                //    }
                if (equip_data.equipdata.att_type > 0)//&& active_eqp.ContainsKey (equip_data.confdata.equip_type))
                {
                    //if (active_eqp[equip_data.confdata.equip_type].id == equip_data.id)
                    //{
                    if (att.ContainsKey(equip_data.equipdata.att_type)) att[equip_data.equipdata.att_type] += equip_data.equipdata.att_value;
                    else att[equip_data.equipdata.att_type] = equip_data.equipdata.att_value;
                    // }
                }
                //}
                //荣耀
                if (equip_data.equipdata.honor_num > 0)
                {
                    if (!att.ContainsKey(99))
                        att[99] = equip_data.equipdata.honor_num;
                }

                //foreach (int i in att.Keys)
                //{
                //    Debug.LogError
                //    (Globle.getAttrAddById(i, att[i]) + "KKK" + att[i]);
                //}
            }
            catch (Exception e)
            {
                Debug.LogError("Message=" + e.Message);
                Debug.LogError("StackTrace=" + e.StackTrace);
            }

            return att;
        }
        public a3_ItemData getItemDataByName(string name)
        {
            a3_ItemData item = new a3_ItemData();

            SXML s_xml = itemsXMl.GetNode("item", "item_name==" + name);
            if (s_xml != null)
            {
                item.file = "icon_item_" + s_xml.getString("icon_file");
                item.borderfile = "icon_itemborder_b039_0" + s_xml.getString("quality");
                item.item_name = s_xml.getString("item_name");
                item.quality = s_xml.getInt("quality");
                item.desc = s_xml.getString("desc");
                item.desc2 = s_xml.getString("desc2");
                item.value = s_xml.getInt("value");
                item.use_lv = s_xml.getInt("use_lv");
                item.use_limit = s_xml.getInt("use_limit");
                item.use_type = s_xml.getInt("use_type");
                int score = s_xml.getInt("intensify_score");
                item.intensify_score = score;
                if (s_xml.getInt("sort_type") < 0)
                {
                    item.sortType = 9999;
                }
                else
                {
                    item.sortType = s_xml.getInt("sort_type");
                }
                item.item_type = s_xml.getInt("item_type");
                item.equip_type = s_xml.getInt("equip_type");
                item.equip_level = s_xml.getInt("equip_level");
                item.job_limit = s_xml.getInt("job_limit");
                item.modelId = s_xml.getInt("model_id");
                item.on_sale = s_xml.getInt("on_sale");
                item.cd_type = s_xml.getInt("cd_type");
                item.cd_time = s_xml.getFloat("cd");
                item.tpid = s_xml.getUint("id");
                item.main_effect = s_xml.getInt("main_effect");
                item.add_basiclevel = s_xml.getInt("add_basiclevel");
                item.use_sum_require = s_xml.getInt("use_sum_require");
                item.zhSummon = s_xml.getInt("summon");
            }
            return item;

        }
        public bool isContainItem(uint tpid)
        {

            SXML s_xml = itemsXMl.GetNode("item", "id==" + tpid);
            if (s_xml != null)
            {
                return true;
            }
            return false;
        }

        public int getItemNumByTpid(uint tpid)
        {
            int num = 0;
            foreach (a3_BagItemData one in Items.Values)
            {
                if (one.isSummon) {
                    if (one.confdata.tpid == tpid)
                    {
                        num += one.num;
                    }
                }
                else 
                if (one.tpid == tpid)
                {
                    num += one.num;
                }
            }

            return num;
        }
        public bool getHaveRoom()
        {
            //这里以后扩展叠加的判断
            if (curi > Items.Count)
                return true;
            else
                return false;
        }

        public bool getHaveOverLayRoom(uint tpid, int num)
        {
            int have_num = 0;
            int room_num = 0;
            foreach (a3_BagItemData one in Items.Values)
            {
                if (one.tpid == tpid)
                {
                    have_num += one.num;
                    room_num++;
                }
            }

            SXML s_xml = itemsXMl.GetNode("item", "id==" + tpid);
            int max_num = s_xml.getInt("maxnum");

            int have_room = curi - Items.Count + room_num;

            if (have_room * max_num >= have_num + num)
                return true;
            else
                return false;
        }


        public bool hasItem(uint tpid)
        {
            foreach (a3_BagItemData one in Items.Values)
            {
                if (one.tpid == tpid)
                    return true;
            }
            return false;
        }


        public string getEquipNameInfo(a3_BagItemData data)
        {
            return getEquipNameInfo(data.tpid, data.equipdata.stage, data.equipdata.intensify_lv, data.equipdata.add_level, data.confdata.equip_level, data.equipdata.attribute);
        }
        public string getEquipNameInfo(uint tpid, int stage_lv, int intensify_ly, int add_level, int equip_level, int attribute)
        {
            string name = "";
            int quality = 1;
            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            if (s_xml != null)
            {
                name = s_xml.getString("item_name");
                quality = s_xml.getInt("quality");
            }

            //string pro = "";
            //switch (stage_lv)
            //{
            //    case 0:
            //        //pro = "普通的";
            //        pro = ContMgr.getCont("a3_equip_0");
            //        break;
            //    case 1:
            //        //pro = "强化的";
            //        pro = ContMgr.getCont("a3_equip_1");
            //        break;
            //    case 2:
            //        //pro = "打磨的";
            //        pro = ContMgr.getCont("a3_equip_2");
            //        break;
            //    case 3:
            //       // pro = "优良的";
            //        pro = ContMgr.getCont("a3_equip_3");
            //        break;
            //    case 4:
            //       // pro = "珍惜的";
            //        pro = ContMgr.getCont("a3_equip_4");
            //        break;
            //    case 5:
            //       // pro = "祝福的";
            //        pro = ContMgr.getCont("a3_equip_5");
            //        break;
            //    case 6:
            //        //pro = "完美的";
            //        pro = ContMgr.getCont("a3_equip_6");
            //        break;
            //    case 7:
            //        //pro = "卓越的";
            //        pro = ContMgr.getCont("a3_equip_7");
            //        break;
            //    case 8:
            //       // pro = "传说的";
            //        pro = ContMgr.getCont("a3_equip_8");
            //        break;
            //    case 9:
            //       // pro = "神话的";
            //        pro = ContMgr.getCont("a3_equip_9");
            //        break;
            //    case 10:
            //      //  pro = "创世的";
            //        pro = ContMgr.getCont("a3_equip_10");
            //        break;
            //}

            //name = pro + name;
            string cizhui = "";
            switch (attribute)
            {
                case 0: break;
                case 1:
                    //cizhui = "[风]";
                    cizhui = ContMgr.getCont("a3_equip_11");
                    break;
                case 2:
                    //cizhui = "[火]";
                    cizhui = ContMgr.getCont("a3_equip_12");
                    break;
                case 3:
                    cizhui = ContMgr.getCont("a3_equip_13");
                    //cizhui = "[光]";
                    break;
                case 4:
                    cizhui = ContMgr.getCont("a3_equip_14");
                    //cizhui = "[雷]";
                    break;
                case 5:
                    cizhui = ContMgr.getCont("a3_equip_15");
                   // cizhui = "[冰]";
                    break;
            }
            name = name + cizhui;
            //name = addGodStr(equip_level) + name;
            string zhui = intensify_ly > 0 ? "+" + intensify_ly : string.Empty;
            string level = add_level > 0 ? ContMgr.getCont("a3_auction_zhui") + add_level.ToString() : string.Empty;
            zhui = "<color=#ff5555ff>" + zhui + "</color>";
            level = "<color=#ff9500ff>" + level + "</color>";
            name = Globle.getColorStrByQuality(name, quality) + zhui + level;

            return name;
        }
        //public void  addGodStr(int equip_level)
        //{
        //string strGod = string.Empty;
        //if (equip_level == 4 || equip_level == 7 || equip_level == 10) strGod = "神·";
        //return strGod;
        //}
        public string getEquipName(a3_BagItemData data)
        {
            string nameStr = string.Empty;

            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + data.tpid);
            if (s_xml != null)
            {
                nameStr = s_xml.getString("item_name");

            }
            return nameStr;
        }

        public string getEquipName(uint tpid)
        {
            string nameStr = string.Empty;

            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            if (s_xml != null)
            {
                nameStr = s_xml.getString("item_name");

            }
            return nameStr;
        }
        public int getEquipTypeBytpId( int tpid )
        {
            string typeSrt = string.Empty;

            SXML s_xml = XMLMgr.instance.GetSXML("item.item", "id==" + tpid);
            if ( s_xml != null )
            {
                typeSrt = s_xml.getString( "equip_type" );

            }
            debug.Log("1111——————：" + typeSrt + "::" + tpid );
            return int.Parse(typeSrt);
        }

        public void useItemByTpid(uint tpid, int num)
        {
            a3_ItemData data = getItemDataById(tpid);

            if (data.use_type < 0)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_BagModel__notodo"));
                return;
            }

            int have_num = getItemNumByTpid(tpid);

            if (have_num < num)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_BagModel__nonum"));
                return;
            }

            //按物品数量少的先使用
            List<a3_BagItemData> ids = new List<a3_BagItemData>();
            int low_num = 999999;
            foreach (a3_BagItemData one in Items.Values)
            {
                if (one.tpid == tpid)
                {
                    if (one.num < low_num)
                    {
                        low_num = one.num;
                        ids.Insert(0, one);
                    }
                    else
                    {
                        ids.Add(one);
                    }
                }
            }

            int curneed_num = num;
            foreach (a3_BagItemData one in ids)
            {
                if (one.num > curneed_num)
                {
                    BagProxy.getInstance().sendUseItems(one.id, curneed_num);
                    break;
                }
                else
                {
                    BagProxy.getInstance().sendUseItems(one.id, one.num);
                    curneed_num = num - one.num;
                }
            }
            A3_BeStronger.Instance?.CheckUpItem();
        }
        public bool isWorked(a3_BagItemData data)
        {
            bool c = false;
            if (data.equipdata.baoshi != null)
            {
                if (data.equipdata.baoshi.Count <= 0) { c = true; }
                foreach (int i in data.equipdata.baoshi.Keys)
                {
                    if (data.equipdata.baoshi[i] > 0)
                    {
                        c = false;
                        break;
                    }
                    else { c = true; }
                }
            }
            else { c = true; }

           // bool a = false;
            //if (data.confdata.equip_type < 1)
            //{
            //    return true;
            //}
            //if (data.equipdata.gem_att != null)
            //{
            //    foreach (int type in data.equipdata.gem_att.Keys)
            //    {
            //        bool b = data.equipdata.gem_att[type] == 0;
            //        if (b == true)
            //        {
            //            a = true;
            //        }
            //        else if (b == false)
            //        {
            //            a = false;
            //            break;
            //        }
            //    }
            //}

            //debug.Log("强化" + data.equipdata.intensify_lv + "进阶" + data.equipdata.stage + "追加" + data.equipdata.add_level + "宝石" + a);
            return data.equipdata.intensify_lv == 0 && data.equipdata.stage == 0 && data.equipdata.add_level == 0  && c;
        }


        public bool HasBaoshi(a3_BagItemData data)
        {
            bool a = false;
            if (data.equipdata.baoshi != null)
            {
                if (data.equipdata.baoshi.Count <= 0) { return false; };
                foreach (int i in data.equipdata.baoshi.Keys)
                {
                    if (data.equipdata.baoshi[i] > 0)
                    {
                        a = true;
                        break;
                    }
                }
            }
            return a;
        }
        public void OnMoneyChange() => A3_BeStronger.Instance?.CheckUpItem();
        public void SellItem() => A3_BeStronger.Instance?.CheckUpItem();
        #region //符石
        public Dictionary<int, a3_RunestoneData> allRunestoneData()
        {
            Dictionary<int, a3_RunestoneData> allrunestones = new Dictionary<int, a3_RunestoneData>();

            List<SXML> s_xml = XMLMgr.instance.GetSXMLList("item.item", "use_type==22");
            if (s_xml != null)
            {
                foreach (SXML x in s_xml)
                {
             
                    a3_RunestoneData runestone = new a3_RunestoneData();
                    runestone.id = x.getInt("id");
                    runestone.item_name = x.getString("item_name");
                    runestone.icon_file = x.getInt("icon_file");
                    runestone.desc = x.getString("desc");
                    runestone.quality = x.getInt("quality");
                    runestone.position = x.getInt("position");
                    runestone.stone_level = x.getInt("stone_level");
                    runestone.name_type = x.getInt("name_type");
                    runestone.compose_data = NeedorGetMaterial(runestone.id,1);
                    runestone.decompose_data = NeedorGetMaterial(runestone.id, 2);
                    allrunestones[runestone.id] = runestone;
                }
            }
            return allrunestones;
        }
        public a3_RunestoneData getRunestoneDataByid(int stone_id)
        {
            return allRunestoneData()[stone_id];
        }
        public string getRunestoneLvByid(int stone_id)
        {
            return allRunestoneData()[stone_id].stone_level.ToString();
        }
        /*  type:         
          1:所需材料
          2:获得材料
        */
        public List<a3_RunestonrnMaterial> NeedorGetMaterial(int stone_id,int type)
        {
            List<a3_RunestonrnMaterial> list_needMaterials= new List<a3_RunestonrnMaterial>();
            SXML xml = XMLMgr.instance.GetSXML("item.rune_stone", "id==" + stone_id);
            List<SXML> s_xml = xml.GetNodeList(type==1?"compose": "decompose");
            if(xml!=null&&s_xml!=null)
            {
                foreach (SXML x in s_xml)
                {
                    a3_RunestonrnMaterial needdata = new a3_RunestonrnMaterial();
                    needdata.id =x.getInt("item");
                    needdata.num = x.getInt("num");
                    list_needMaterials.Add(needdata);
                }
            }
            return list_needMaterials;
        }

        #endregion

    }

    public class a3_BagItemData:IComparable<a3_BagItemData>
    {
        public uint id;
        public uint tpid;
        public int num;
        public bool bnd;
        public bool isEquip;
        public bool isNew;
        public bool isSummon;
        public bool isLabeled;
        public bool ismark;
        public bool isrunestone;
        public bool ishallows = false;
        public a3_EquipData equipdata = new a3_EquipData();
        public a3_ItemData confdata = new a3_ItemData();
        public a3_SummonData summondata = new a3_SummonData();
        public a3_AuctionData auctiondata = new a3_AuctionData();
        public a3_RunestoneData runestonedata = new a3_RunestoneData();

        public int CompareTo(a3_BagItemData other)
        {
            if (confdata.sortType > other.confdata.sortType)
                return 1;
            else if (confdata.sortType < other.confdata.sortType)
                return -1;
            else {
                if (confdata.quality > other.confdata.quality)
                    return -1;
                else if (confdata.quality < other.confdata.quality)
                    return 1;
                else
                {
                    if (tpid > other.tpid)
                        return 1;
                    else if (tpid < other.tpid)
                        return -1;
                    else {
                        if (equipdata.stage > other.equipdata.stage)
                            return -1;
                        else if (equipdata.stage < other.equipdata.stage)
                            return 1;
                        else {
                            if (equipdata.intensify_lv > other.equipdata.intensify_lv)
                                return -1;
                            else if (equipdata.intensify_lv < other.equipdata.intensify_lv)
                                return 1;
                            else {
                                if (equipdata.add_level > other.equipdata.add_level)
                                    return -1;
                                else if (equipdata.add_level < other.equipdata.add_level)
                                    return 1;
                                else {
                                    if (equipdata.baoshi?.Count > other.equipdata.baoshi?.Count)
                                        return -1;
                                    else if (equipdata.baoshi?.Count < other.equipdata.baoshi?.Count)
                                        return 1;
                                    else
                                        return 0;
                                }
                            }
                        }         
                    }
                }
            }
        }
    }
    //符石数据
     public class a3_RunestoneData
    {
        public int id;
        public string item_name;
        public int icon_file;
        public string desc;
        public int quality;
        public int use_type;
        public int stonelevel;
        public int name_type;
        public int stone_level
        {
            get { return stonelevel < 1 ? 1 : stonelevel;}
            set { stonelevel = value;}
        }
        public int position;

        public Dictionary<int, int> runeston_att;                           //属性  
        public List<a3_RunestonrnMaterial> compose_data;                    //合成所需要的材料
        public List<a3_RunestonrnMaterial> decompose_data;                  //分解所获得的材料

    }
    public class a3_RunestonrnMaterial
    {
        public int id;          //所需材料id
        public int num;         //所需材料数量
    }
    //召唤兽物品数据
    public class a3_SummonData : IComparable<a3_SummonData>
    {
        public int id;                                 //唯一id
        public int tpid;                               //模板id
        public string name;                            //名字
        public int currentexp;                         //当前经验
        public int currenthp;                          //当前hp
        public int maxhp;                              //最大hp
        public int grade;                              //品级（1普通、2精英、3变异）
        public int naturaltype;                        //资质类型（1攻、2防、3体、4敏）
        public int level;                              //等级
        public int lifespan;                           //寿命
        public int blood;                              //血脉(1光、2暗)
        public int luck;                               //幸运
        public int power;                              //战斗力
        public int talent_type;                        //天赋类型
        public int attNatural;                         //攻击资质
        public int defNatural;                         //防御资质
        public int agiNatural;                         //敏捷资质
        public int conNatural;                         //体质资质
        public int resetluck;                          //保存洗练幸运
        public int resetatt;                    //保存洗练攻击资质
        public int resetdef;                     //保存洗练防御资质
        public int resetagi;                       //保存洗练敏捷资质
        public int resetcon;                     //保存洗练体质资质
        public bool haveReset;                        //是否有保存资质
        public int star;                               //资质星数
        public int max_attack;                         //最大攻
        public int min_attack;                         //最小攻 
        public int physics_def;                        //物理防御
        public int magic_def;                          //魔法防御
        public int physics_dmg_red;                    //物理伤害减免
        public int magic_dmg_red;                       //魔法伤害减免
        public int dodge;                               //闪避
        public int hit;                                  //命中
        public int double_damage_rate;                 //暴击率
        public int fatal_damage;                       //暴击伤害       
        public int reflect_crit_rate;                  //致命闪避率
        public int skillNum;                           //技能数量
        public Dictionary<int, summonskill> skills;                        //拥有技能
        public Dictionary<int, summonshouhun> shouhun; //兽魂
        public Dictionary<int, int> equips;            //拥有装备
        public bool isSpecial;                         //是否变异
        public int objid;                              //模型编号
        public int status;                             //0闲置，1出战

        public Dictionary<int, link_data> linkdata;   // 连携属性
        public int linkCombpt;         //连携战力


        public int CompareTo(a3_SummonData other)
        {
            if (tpid > other.tpid)
                return 1;
            else if (tpid < other.tpid)
                return -1;
            else
            {
                if (star > other.star) return -1;
                else if (star < other.star)
                    return 1;
                else {
                    if (level > other.level) return -1;
                    else if (level < other.level) return 1;
                    else return 0;
                }
            }
        }
    }

    //拍卖物品数据
    public class a3_AuctionData
    {
        public uint cid;            //所有者id
        public string seller;       //上架者名字
        public int tm;              //上架时间
        public int pro_tm;          //上架时长（12h,24h,48h）
        public int cost;            //购买花费
        public int get_type;        //领取类型（1:下架  2:购买  3:卖出）
        public int get_tm;          //领取物品的时间
    }

    public class a3_EquipData
    {
        public uint color;
        public int intensify_lv;
        public int add_level;
        public int add_exp;
        public int stage;
        public int blessing_lv;
        public int baseCombpt;
        public int combpt;
        public int attribute;
        public int att_type;
        public int att_value;
        public int eqp_level;
        public int eqp_type;
        public Dictionary<int, int> subjoin_att;
        public Dictionary<int, int> gem_att;
        public Dictionary<int, int> baoshi;
        public uint? tpid;
        public int honor_num;
        //public Dictionary<int, a3_gem_att> gem_att;
    }

    //public class a3_gem_att {
    //    public int att_type;
    //    public int att_value;
    //    public int gem_lv;
    //}

    public class a3_ItemData
    {
        public uint tpid;
        public string file;
        public string borderfile;
        public string item_name;
        public string desc;
        public string desc2;
        public int maxnum;
        public int quality;
        public int value;
        public int use_type;
        public int sortType;
        public int use_lv;
        public int use_limit;
        public int intensify_score;
        public int item_type;
        public int equip_type;
        private int equipLevel;
        public int equip_level { get { return equipLevel <= 0 ? 1 : equipLevel; } set { equipLevel = value; } }
        public int job_limit;
        public int modelId;
        public int on_sale;
        public int cd_type;
        public float cd_time;
        public int main_effect;
        public int add_basiclevel;
        public int use_sum_require;
        public int zhSummon;
    }
    /// <summary> 装备的强化信息 </summary>
    class EqpIntensifyLvInfo
    {
        /// <summary> 所需强化费用 </summary>
        public int intensifyCharge;
        /// <summary> 所需强化材料和数量 </summary>
        public Dictionary<uint /* item_id */ , int /* num */ > intensifyMaterials;
    }
    /// <summary> 装备的进阶信息 </summary>
    class EqpStageLvInfo
    {
        /// <summary> 穿戴需求 </summary>
        public Dictionary<A3_CharacterAttribute /* att id */, int /* att num */> equipLimit;
        /// <summary> 所需人物等级 </summary>
        public uint lv;
        /// <summary> 所需人物转生等级 </summary> 
        public uint reincarnation;
        /// <summary> 进阶所需材料和数量 </summary>
        public Dictionary<int /* item id */, int /* num */> upstageMaterials;
        /// <summary> 进阶所需费用 </summary>
        public uint upstageCharge;
        /// <summary> 当前最大可追加的等级 </summary>
        public int maxAddLv;
    }
    /// <summary> 装备的追加信息 </summary>
    class EqpAddConfInfo
    {
        /// <summary> 本次追加的费用 </summary>
        public int addCharge;
        /// <summary> 追加材料的id </summary>
        public uint matId;
        /// <summary> 追加材料的数量 </summary>
        public uint matNum;
    }
    /// <summary> 装备镶嵌的宝石信息 </summary>
    class EqpGemConfInfo
    {
        /// <summary> 影响的属性类型 </summary>
        public int attType;
        /// <summary> 影响的属性最大值 </summary>
        public uint attMax;
        /// <summary> 宝石的id </summary>
        public uint gemId;
        /// <summary> 宝石的数量 </summary>
        public uint gemNeedNum;
    }
    /// <summary> 人物属性 </summary>
    public enum A3_CharacterAttribute
    {
        Strength = 1,
        Agility = 2,
        Constitution = 3,
        Intelligence = 4,
        Wisdom = 34
    }
}