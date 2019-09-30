using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable CheckNamespace
namespace MuGame
{
    public class A3_WingModel : ModelBase<A3_WingModel>
    {
        public A3_WingModel()
            : base()
        {
            GetAllWingsXMLData();
        }
        //配置
        private SXML wingXML;
        public SXML WingXML
        {
            get
            {
                return wingXML ??
                    (wingXML = XMLMgr.instance.GetSXML("wings.wing",
                    "career==" + PlayerModel.getInstance().profession));
            }
        }

        ////当前翅膀阶级//-----目前的翅膀ID就是stage 2016-03-22
        public int lastStage = 0;
        private int stage = 0;
        public int Stage
        {
            get { return stage; }
            set
            {
                if (value == stage)
                    return;
                stage = value;
            }
        }
        public bool stageUp = false;

        //当前选择显示的翅膀阶级 //------当前玩家要显示的翅膀就是选择的翅膀
        public int ShowStage = 0;
        public int LastShowState = 0;

        public int lastLevel = 0;
        private int level = 0;
        public int Level
        {
            get { return level; }
            set
            {
                if (value == level)
                    return;
                level = value;
            }
        }
        public int lastExp = 0;
        public int Exp = 0;

        //所有翅膀的data
        public Dictionary<int, WingsData> dicWingsData;
        public void GetAllWingsXMLData()
        {
            dicWingsData = new Dictionary<int, WingsData>();
            if (wingUpItem==null)
                wingUpItem = new Dictionary<string,uint>
                {
                    ["levelItemId"] = XMLMgr.instance.GetSXML("wings.level_item").getUint("item_id"),
                    ["stageItemId"] = XMLMgr.instance.GetSXML("wings.stage_item").getUint("item_id")
                };      
            List <SXML> wings = WingXML.GetNodeList("wing_stage");
            for (int i = 0; i < wings.Count; i++)
            {
                WingsData _data = new WingsData();

                _data.stage = wings[i].getInt("stage_id");
                _data.spriteFile = "icon_wing_" + wings[i].getString("icon");
                _data.stageCostGold = wings[i].getUint("cost_gold");
                _data.stageCrystalMin = wings[i].getUint("crystal_min");
                _data.stageCrystalMax = wings[i].getUint("crystal_max");
                _data.stageCrystalStep = wings[i].getUint("crystal_step");
                _data.stageRateMin = wings[i].getUint("rate_min");
                _data.stageRateMax = wings[i].getUint("rate_max");
                _data.wingName = wings[i].getString("name");

                dicWingsData[_data.stage] = _data;
            }
        }

        //是否装备翅膀 //2016-03-22
        //public bool hasEquipWing = false;
        private Dictionary<string /*event*/ ,uint /*item id*/ > wingUpItem;       
            
        private SXML GetLevelXML(int stage, int level) => WingXML.GetNode("wing_stage", "stage_id==" + stage)?.GetNode("wing_level", "level_id==" + level);
        //{
        //    SXML xml = null;
        //    SXML stageNode = WingXML.GetNode("wing_stage", "stage_id==" + stage);
        //    if (stageNode != null)
        //        xml = stageNode.GetNode("wing_level", "level_id==" + level);

        //    return xml;
        //}

        //获取升级最大经验
        public uint GetLevelUpMaxExp(int stage, int level) => GetLevelXML(stage, level)?.getUint("exp") ?? 0;
        //{

        //    SXML xml = GetLevelXML(stage, level);
        //    if (xml == null)
        //        return 0;

        //    uint maxExp = xml.getUint("exp");
        //    return maxExp;
        //}

        //获得升阶需要材料的数量
        public uint GetStageUpCostItemSum(int stage) => WingXML.GetNode("wing_stage", "stage_id==" + stage)?.getUint("crystal_min") ?? 0;
        //获得升级需要材料的数量
        public uint GetLevelUpCostItemSum(int stage, int level) => GetLevelXML(stage, level)?.getUint("cost_item_num") ?? 0;
        //{
        //    SXML xml = GetLevelXML(stage, level);
        //    if (xml == null)
        //        return 0;

        //    uint cost = xml.getUint("cost_item_num");
        //    return cost;
        //}

        //获得升级需要的花费
        public int GetLevelUpCost(int stage, int level) => GetLevelXML(stage, level)?.getInt("cost_gold") ?? 0;
        //获得最大阶级
        public int GetXmlMaxStage() => dicWingsData.Count;
        //获得对应阶级的最大等级
        public int GetStageMaxLevel(int stage) => (WingXML.GetNode("wing_stage", "stage_id==" + stage)?.GetNodeList("wing_level").Count ?? 1) - 1;
        //获得对应阶级所需的进阶费用
        public int GetStageUpCost(int stage) => WingXML.GetNode("wing_stage", "stage_id==" + stage)?.getInt("cost_gold") ?? -1;
        //{
        //    int maxLevel = 0;
        //    SXML levelNode = WingXML.GetNode("wing_stage", "stage_id==" + stage);

        //    if (levelNode != null)
        //        maxLevel = levelNode.GetNodeList("wing_level").Count - 1;

        //    return maxLevel;
        //}

        public bool CheckLevelupAvailable() =>
            stage < GetXmlMaxStage() && level == GetStageMaxLevel(stage) ?
                 (PlayerModel.getInstance().money >= GetStageUpCost(stage + 1)
                    && a3_BagModel.getInstance().getItemNumByTpid(wingUpItem["stageItemId"]) >= GetStageUpCostItemSum(stage + 1)) :
            level < GetStageMaxLevel(stage) ?
                (PlayerModel.getInstance().money >= GetLevelUpCost(stage, level + 1)
                    && a3_BagModel.getInstance().getItemNumByTpid(wingUpItem["levelItemId"]) >= GetLevelUpCostItemSum(stage, level + 1)) :
            false;
        /// <summary>
        /// 玩家模型的变化:装备/卸下 翅膀
        /// </summary>
        /// <param name="data"></param>
        public void OnEquipModelChange()
        {
            int wingID = this.ShowStage;
            int wingFxID = this.ShowStage;

            SelfRole._inst.m_roleDta.m_WindID = wingID;
            SelfRole._inst.m_roleDta.m_WingFXID = wingFxID;
            SelfRole._inst.set_wing(wingID, wingFxID);
        }


        //初始化翅膀信息
        public void InitWingInfo(Variant data)
        {
            Stage = data["stage"];

            lastExp = Exp = data["exp"];
            lastLevel = level = data["level"];
        }

        //经验等级变化
        public void SetLevelExp(Variant data)
        {
            lastExp = Exp;
            Exp = data["exp"];

            lastLevel = Level;
            Level = data["level"];
        }

        //阶级变化
        public void SetStageInfo(Variant data)
        {
            if (data.ContainsKey("stage"))
            {
                lastLevel = Level;
                Level = 0;

                lastStage = Stage;
                Stage = data["stage"];
                ShowStage = data["show_stage"];

                stageUp = true;
            }
            else
            {
                stageUp = false;
            }
        }

        //设置显示的翅膀
        public void SetShowStage(Variant data)
        {
            this.ShowStage = data["show_stage"];

            OnEquipModelChange();
            //if (show == 0)
            //{
            //    hasEquipWing = false;
            //}
            //else
            //{
            //    this.ShowStage = show;
            //    hasEquipWing = true;
            //}
            if (a3_wing_skin.instance != null)
            {
                a3_wing_skin.instance.OnSetIconBGImage(this.ShowStage);
            }
        }
    }

    public class WingsData
    {
        public int stage;//翅膀阶级//avatar 路径
        public uint IconID;//iconid
        public string spriteFile;//精灵位置
        public string wingName;//翅膀名字
        public uint stageCostGold;//消耗金币
        public uint stageCrystalMin;//最小进阶材料
        public uint stageCrystalMax;//最大进阶材料
        public uint stageCrystalStep;//每步添加的数量
        public uint stageRateMin;//最小概率
        public uint stageRateMax;//最大进阶概率

        public bool isUnlock(int curStage)
        {
            bool b = false;
            if (curStage >= stage)
                b = true;

            return b;
        }
    }
}
