using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;

namespace MuGame.Qsmy.model
{
    enum Quality
    {
        QNone = 0,
        QWhite = 1,
        QGreen = 2,
        QBlue = 4,
        QPurple = 8,
        QGold = 16,
        qRed = 32,
        QAll = 63,
    }

    /*
	 * 挂机本地文件格式定义:
	 * 所有数据以字符串保存,中间分割 '|'
	 * 
	 * 0. 预留
	 * - NHpLower           血量低到该值时自动喝HP药
	 * - NMpLower           蓝量低到该值时自动喝MP药
	 * - MHpLower           血量低到该值时自动喝商城药
	 * - BuyDrug            自动购买普通药品
	 * - PickEqp            拾取装备的品质，按位
	 * - PickMat            拾取材料的品质，按位
	 * - EqpProc            装备处理的品质，按位
	 * - PickEqp_cailiao		拾取装备材料
	 * - PickPet_cailiao		拾取宠物材料
	 * - PickWing_cailiao	拾取翅膀材料
	 * - PickSummon_cailiao 拾取召唤兽材料
	 * - PickDrugs			拾取药品
	 * - PickGold			拾取金币
	 * - PickOther			拾取其它
	 * - EqpType            装备处理的方式，0:不处理,1:出售,2:回收
	 * - Skill 0            技能0的ID
	 * - Skill 1            技能1的ID
	 * - Skill 2            技能2的ID
	 * - Skill 3            技能3的ID
	 * - Scope              挂机范围，0:定点,1:小范围,2:全地图
	 * - Avoid              是否避开精英怪和BOSS
	 * - AutoPK             是否自动PK
	 * - StoneRespawn       是否使用复活石复活
	 * - GoldRespawn        是否使用元宝复活
	 * - RespawnLimit       是否开启自动复活限制
	 * - RespawnUpBound     道具复活上限
	 */

    class AutoPlayModel : ModelBase<AutoPlayModel>
    {
        private readonly int version = 3;

        public Action<int> onHpLowerChange = null;
        public Action<int> onMpLowerChange = null;
        public List<AutoPlayConfig4FB> autoplayCfg4FB = new List<AutoPlayConfig4FB>();
        private int nHpLower;
        public int NHpLower
        {
            get { return nHpLower; }
            set
            {
                if (nHpLower == value)
                    return;

                nHpLower = value;

                if (onHpLowerChange != null)
                    onHpLowerChange(nHpLower);

                InterfaceMgr.doCommandByLua("AutoPlayModel:getInstance().onHpLowerChange", "model/AutoPlayModel", nHpLower);
            }
        }

        private int nMpLower;
        public int NMpLower
        {
            get { return nMpLower; }
            set
            {
                if (nMpLower == value)
                    return;

                nMpLower = value;

                if (onMpLowerChange != null)
                    onMpLowerChange(nMpLower);

                InterfaceMgr.doCommandByLua("AutoPlayModel:getInstance().onMpLowerChange", "model/AutoPlayModel", nMpLower);
            }
        }

        //public int MHpLower { get; set; }
        public int BuyDrug { get; set; }
        public int PickEqp { get; set; }
        public int PickMat { get; set; }
        public int PickEqp_cailiao { get; set; }
        public int PickPet_cailiao { get; set; }
        public int PickWing_cailiao { get; set; }
        public int PickSummon_cailiao { get; set; }
        public int PickDrugs { get; set; }
        public int PickGold { get; set; }
        public int PickOther { get; set; }
        public int EqpProc { get; set; }
        public int EqpType { get; set; }

        private int[] skills = null;
        public int[] Skills
        {
            get { return skills ?? (skills = new int[4]); }
        }

        // public int Scope { get; set; } // Modified
        public int Avoid { get; set; }
        public int AutoPK { get; set; }
        public int StoneRespawn { get; set; }
        public int GoldRespawn { get; set; }
        public int RespawnLimit { get; set; }
        public int RespawnUpBound { get; set; }

        //!--道具复活剩余次数
        public int RespawnLeft { get; set; }

        //!--挂机的配置
        private SXML autoplayXml = null;
        public SXML AutoplayXml
        {
            get { return autoplayXml ?? (autoplayXml = XMLMgr.instance.GetSXML("autoplay")); }
        }

        //!--挂机点
        public Dictionary<int, List<Vector3>> mapWayPoint = null;

        //!--挂机中可以使用的普通HP药列表
        private int[] nhpdrugs = null;
        public int[] Nhpdrugs
        {
            get
            {
                if (nhpdrugs == null)
                {
                    List<SXML> xml = AutoplayXml.GetNodeList("nhp");
                    if (xml == null || xml.Count == 0)
                        return null;

                    nhpdrugs = new int[xml.Count];
                    for (int i = 0; i < xml.Count; i++)
                    {
                        nhpdrugs[i] = xml[i].getInt("id");
                    }
                }
                return nhpdrugs;
            }
        }

        //!--挂机中可以使用的普通MP药列表
        private int[] nmpdrugs = null;
        public int[] Nmpdrugs
        {
            get
            {
                if (nmpdrugs == null)
                {
                    List<SXML> xml = AutoplayXml.GetNodeList("nmp");
                    if (xml == null || xml.Count == 0)
                        return null;

                    nmpdrugs = new int[xml.Count];
                    for (int i = 0; i < xml.Count; i++)
                    {
                        nmpdrugs[i] = xml[i].getInt("id");
                    }
                }
                return nmpdrugs;
            }
        }

        public void Init()
        {
            //Scope = 1; // Modified
            ReadLocalCfg();
            ReadLocalData();
            InitMapWayPoint();
        }
        private void ReadLocalCfg()
        {
            List<SXML> conf = AutoplayXml.GetNodeList("conf_fb");
            for (int i = 0; i < conf.Count; i++)
            {
                float mdis_fb = conf[i].getFloat("mdis_fb");
                float pickdis_fb = conf[i].getFloat("pickdis_fb");
                float pkdis_fb = conf[i].getFloat("pkdis_fb");
                List<int> mapList = new List<int>();
                List<SXML> maps = conf[i].GetNodeList("map");
                for (int j = 0; j < maps.Count; j++)
                    mapList.Add(maps[j].getInt("map_id"));
                autoplayCfg4FB.Add(new AutoPlayConfig4FB { Distance = mdis_fb, DistancePick = pickdis_fb, DistancePK = pkdis_fb, map = mapList });
            }        
        }
        private void InitMapWayPoint()
        {
            if (mapWayPoint == null)
                mapWayPoint = new Dictionary<int, List<Vector3>>();

            List<SXML> list = AutoplayXml.GetNodeList("map");
            for (int i = 0; i < list.Count; i++)
            {
                List<SXML> xml = list[i].GetNodeList("pos");
                int mapid = list[i].getInt("id");
                List<Vector3> poss = new List<Vector3>();
                for (int j = 0; j < xml.Count; j++)
                {
                    Vector3 p = new Vector3();
                    p.x = xml[j].getInt("x");
                    p.y = xml[j].getInt("y");
                    p.z = xml[j].getInt("z");
                    poss.Add(p);
                }
                mapWayPoint[mapid] = poss;
            }
        }

        private void SetDefault()
        {
            NHpLower = 80;
            NMpLower = 50;
            //MHpLower = 20;
            BuyDrug = 1;
            PickEqp = (int)Quality.QAll;
            PickMat = (int)Quality.QAll;
            EqpProc = (int)Quality.QWhite;
            EqpType = 0;
            PickEqp_cailiao = 1;
            PickPet_cailiao = 1;
            PickWing_cailiao = 1;
            PickSummon_cailiao = 1;
            PickDrugs = 1;
            PickGold = 1;
            PickOther = 1;
            Skills[0] = 0;
            Skills[1] = 0;
            Skills[2] = 0;
            Skills[3] = 0;
            //Scope = 0; // Modified
            Avoid = 0;
            AutoPK = 0;
            StoneRespawn = 0;
            GoldRespawn = 0;
            RespawnLimit = 1;
            RespawnUpBound = 10;
        }

        public void ReadLocalData()
        {
            string localInfo = FileMgr.loadString(FileMgr.TYPE_AUTO, "setting");
            if (string.IsNullOrEmpty(localInfo))
            {
                SetDefault();
            }
            else
            {
                try
                {
                    string[] sstr = localInfo.Split('|');
                    int i = 0;
                    int localversion = int.Parse(sstr[i++]);
                    if (localversion != version)
                    {
                        throw new Exception("Autoplay local data version is not match!");
                    }
                    NHpLower = int.Parse(sstr[i++]);
                    NMpLower = int.Parse(sstr[i++]);
                    i++;
                    //MHpLower = int.Parse(sstr[i++]);
                    BuyDrug = int.Parse(sstr[i++]);
                    PickEqp = int.Parse(sstr[i++]);
                    PickMat = int.Parse(sstr[i++]);
                    EqpProc = int.Parse(sstr[i++]);

                    PickEqp_cailiao = int.Parse(sstr[i++]);
                    PickPet_cailiao = int.Parse(sstr[i++]);
                    PickWing_cailiao = int.Parse(sstr[i++]);
                    PickSummon_cailiao = int.Parse(sstr[i++]);
                    PickDrugs = int.Parse(sstr[i++]);
                    PickGold = int.Parse(sstr[i++]);
                    PickOther = int.Parse(sstr[i++]);

                    EqpType = int.Parse(sstr[i++]);
                    Skills[0] = int.Parse(sstr[i++]);
                    Skills[1] = int.Parse(sstr[i++]);
                    Skills[2] = int.Parse(sstr[i++]);
                    Skills[3] = int.Parse(sstr[i++]);
                    for (int j = 0; j < 4; j++) // validate skills
                    {
                        skill_a3Data skdata = null;
                        Skill_a3Model.getInstance().skilldic.TryGetValue(Skills[j], out skdata);
                        if (skdata != null && skdata.now_lv == 0)
                            Skills[j] = 0;
                    }
                    i++;
                    //Scope =  int.Parse(sstr[i++]);
                    Avoid = int.Parse(sstr[i++]);
                    AutoPK = int.Parse(sstr[i++]);
                    StoneRespawn = int.Parse(sstr[i++]);
                    GoldRespawn = int.Parse(sstr[i++]);
                    RespawnLimit = int.Parse(sstr[i++]);
                    RespawnUpBound = int.Parse(sstr[i++]);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                    FileMgr.removeFile(FileMgr.TYPE_AUTO, "setting");
                    SetDefault();
                }
            }
        }

        public void WriteLocalData()
        {
            string ret = "";
            ret += version.ToString() + '|';
            ret += NHpLower.ToString() + '|';
            ret += NMpLower.ToString() + '|';
            ret += (0).ToString() + '|'; //MHPLower
            ret += BuyDrug.ToString() + '|';
            ret += PickEqp.ToString() + '|';
            ret += PickMat.ToString() + '|';
            ret += EqpProc.ToString() + '|';

            ret += PickEqp_cailiao.ToString() + '|';
            ret += PickPet_cailiao.ToString() + '|';
            ret += PickWing_cailiao.ToString() + '|';
            ret += PickSummon_cailiao.ToString() + '|';
            ret += PickDrugs.ToString() + '|';
            ret += PickGold.ToString() + '|';
            ret += PickOther.ToString() + '|';

            ret += EqpType.ToString() + '|';
            ret += Skills[0].ToString() + '|';
            ret += Skills[1].ToString() + '|';
            ret += Skills[2].ToString() + '|';
            ret += Skills[3].ToString() + '|';
            ret += (1).ToString() + '|'; //Scope  // Modified
            ret += Avoid.ToString() + '|';
            ret += AutoPK.ToString() + '|';
            ret += StoneRespawn.ToString() + '|';
            ret += GoldRespawn.ToString() + '|';
            ret += RespawnLimit.ToString() + '|';
            ret += RespawnUpBound.ToString();

            FileMgr.saveString(FileMgr.TYPE_AUTO, "setting", ret);
        }

        /// <summary>
        /// 检查玩家是否有意愿捡取该品质物品
        /// </summary>
        /// <param name="tpid">物品配置ID</param>
        /// <returns></returns>
        public bool WillPick(uint tpid)
        {
            a3_ItemData itmData = a3_BagModel.getInstance().getItemDataById(tpid);
            int quality = itmData.quality;
            int bitQual = (1 << (quality - 1));
            bool will = true;
            if (itmData.item_type == 1)
            {//道具
             //will = ((bitQual & PickMat) != 0);
                var ap = XMLMgr.instance.GetSXML("autoplay");
                string[] eqpcl = ap.GetNode("eqp_cailiao").getString("list").Split(',');
                string[] petcl = ap.GetNode("pet_cailiao").getString("list").Split(',');
                string[] wingcl = ap.GetNode("wing_cailiao").getString("list").Split(',');
                string[] summoncl = ap.GetNode("summon_cailiao").getString("list").Split(',');
                string[] drugscl = ap.GetNode("drugs").getString("list").Split(',');
                if (eqpcl.Contains(tpid.ToString()) && PickEqp_cailiao != 1) { will = false; }
                else if (petcl.Contains(tpid.ToString()) && PickPet_cailiao != 1) { will = false; }
                else if (wingcl.Contains(tpid.ToString()) && PickWing_cailiao != 1) { will = false; }
                else if (summoncl.Contains(tpid.ToString()) && PickSummon_cailiao != 1) { will = false; }
                else if (drugscl.Contains(tpid.ToString()) && PickDrugs != 1) { will = false; }
                else if (PickOther != 1) { will = false; }
            }
            else if (itmData.item_type == 2)
            {//装备
                will = ((bitQual & PickEqp) != 0);
            }
            else if (tpid == 0)
            {//金币
                if (PickGold != 1) { will = false; }
            }


            return will;
        }

        public int GetSkillForUse()
        {
            for (int i = 0; i < 5; i++)
            {
                //!--该技能未设置
                if (Skills[i] == 0)
                    continue;
                int skid = Skills[i];
                skill_a3Data skdata = null;
                Skill_a3Model.getInstance().skilldic.TryGetValue(skid, out skdata);
                if (skdata == null)
                    continue;
                if (skdata.cdTime > 0)
                    continue;
                if (skdata.now_lv == 0)
                    continue;
                long tempCD = muNetCleint.instance.CurServerTimeStampMS + skdata.cd;
                if (skdata.endCD < tempCD)
                    skdata.endCD = tempCD;
                return skid;
            }
            return a1_gamejoy.NORNAL_SKILL_ID;
        }

        public List<int> GetAllHpDrugID()
        {
            List<SXML> nhps = AutoplayXml.GetNodeList("nhp");
            List<int> ids = new List<int>();
            for (int i = 0; i < nhps.Count; i++)
            {
                int id = nhps[i].getInt("id");
                ids.Add(id);
            }
            nhps = null;
            return ids;
        }

        public List<int> GetAllMpDrugID()
        {
            List<SXML> nmps = AutoplayXml.GetNodeList("nmp");
            List<int> ids = new List<int>();
            for (int i = 0; i < nmps.Count; i++)
            {
                int id = nmps[i].getInt("id");
                ids.Add(id);
            }
            nmps = null;
            return ids;
        }

        public List<Vector3> GetMapAutoPlayPosition(int mapid)
        {
            List<Vector3> mappos = new List<Vector3>();

            return mappos;
        }
    }
    class AutoPlayConfig4FB
    {
        public float Distance;
        public float DistancePick;
        public float DistancePK;
        public List<int> map = new List<int>();
    }
}
