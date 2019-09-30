using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable CheckNamespace
namespace MuGame
{
    /// <summary>
    /// 宠物数据管理类
    /// 通过PetProxy与服务器进行交互
    /// 驱动Pet UI脚本和Pet形象管理类
    /// </summary>
    public class A3_PetModel : ModelBase<A3_PetModel>
    {
        public A3_PetModel():base()
        {
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_USE_PETFEED, OnMonitorFeed);
        }

        //配置
        private SXML petXML = null;
        public SXML PetXML
        {
            get
            {
                if (petXML == null)
                {
                    petXML = XMLMgr.instance.GetSXML("pet");
                }
                return petXML;
            }
        }
        public static bool showrenew=false;
        //宠物配置id
        private uint tpid = 0;
        public List<Variant> petId;
        public Action OnTpidChange = null;
        public uint Tpid 
        { 
            get { return tpid; }
            set
            {
                if (tpid == value)
                    return;

                tpid = value;

                if (OnTpidChange != null)
                    OnTpidChange();
            }
        }
        public static bool showbuy = false;
        public static uint curPetid=0;
        //宠物阶数
        private int stage = -1;
        public Action OnStageChange = null;
        public int Stage 
        { 
            get { return stage; }
            set
            {
                if (stage == value)
                    return;

                stage = value;

                if (OnStageChange != null)
                    OnStageChange();
            }
        }
        #region
        //等级
        private int level = 0;
        public Action OnLevelChange = null;
        public int Level
        {
            get { return level; }
            set
            {
                if (level == value)
                    return;

                level = value;

                if (OnLevelChange != null)
                    OnLevelChange();
            }
        }
        
        //经验
        private int exp = 0;
        public Action OnExpChange = null;
        public int Exp
        {
            get { return exp; }
            set
            {
                if (exp == value)
                    return;

                exp = value;

                if (OnExpChange != null)
                    OnExpChange();
            }
        }

        //饥饿度
        private int starvation = 0;
        public Action OnStarvationChange = null;
        public int Starvation
        {
            get { return starvation; }
            set
            {
                if (value == 0)
                {
                    String hint = ContMgr.getCont("pet_invalid");
                    if(flytxt.instance != null)
                        flytxt.instance.fly(hint);
                }

                if (starvation == value)
                    return;

                starvation = value;

                if (OnStarvationChange != null)
                    OnStarvationChange();
            }
        }
        //是否自动购买
        private bool auto_buy = false;
        public Action Onauto_buyChange = null;
        public bool Auto_buy 
        {
            get { return auto_buy; }
            set
            {
                if (auto_buy == value)
                    return;

                auto_buy = value;

                if (Onauto_buyChange != null)
                    Onauto_buyChange();
            }
        }
        //是否自动喂养
        private bool auto_feed = false;
        public Action OnAutoFeedChange = null;
        public bool first = false;
        public long getTime=0;
        public bool Auto_feed
        {
            get { return auto_feed; }
            set
            {
                if (auto_feed == value)
                    return;

                auto_feed = value;

                if (OnAutoFeedChange != null)
                    OnAutoFeedChange();
            }
        }

        public bool hasPet()
        {
            return Tpid > 0 ? true : false;
        }
        

        //!--获取升级所需经验
        public int GetMaxExp()
        {
            return CurrentLevelConf().getInt("exp");
        }

        //!--获取祝福消耗的精灵祝福
        public int GetBlessCost()
        {
            return CurrentLevelConf().getInt("cost_item_num");
        }

        //!--获取喂养道具的配置id
        public uint GetFeedItemTpid()
        {
            SXML feeditm = PetXML.GetNode("feed_item");
            return feeditm.getUint("item_id");
        }

        //!--获取升级道具的配置id
        public uint GetLevelItemTpid()
        {
            SXML levelitm = PetXML.GetNode("level_item");
            return levelitm.getUint("item_id");
        }

        //!--获取进阶道具的配置id
        public uint GetStageItemTpid()
        {
            SXML stageitm = PetXML.GetNode("stage_item");
            return stageitm.getUint("item_id");
        }
        #endregion
        //!--获取宠物avatar
        public string GetPetAvatar(int id, int stageid)
        {
            //try//暂时增加容错 by lucisa
            //{
            //    SXML idXML = PetXML.GetNode("pet", "id==" + id);
            //    SXML stageXML = idXML.GetNode("stage", "stage==" + stageid);
            //    return stageXML.getString("avatar");
            //}
            //catch (System.Exception ex)
            //{
            //    return "";
            //}
            //return "";
            if (XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + id) != null)
                return XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + id).getString("mod");
            else
                return "";
           
        }

        //!--获取宠物图标
        public string GetPetIcon()
        {
            return CurrentStageConf().getString("icon");;
        }
        //!--获取当前阶段配置
        public SXML CurrentStageConf()
        {
            SXML idXML = PetXML.GetNode("pet", "id==" + Tpid);
            SXML stageXML = idXML?.GetNode("stage", "stage==" + Stage);
            return stageXML;
        }
        //!--获取下一阶段配置
        public SXML NextStageConf() {
            SXML idXML = PetXML.GetNode("pet", "id==" + Tpid);
            SXML nextStageXML = idXML.GetNode("stage", "stage==" + (Stage+1));
            if (nextStageXML == null)
                return null;
            return nextStageXML;
        }
        //!--获取下一阶段配置
        public int StageMaxLvl => CurrentStageConf().getInt("to_next_stage_level");

        //!--获取当前等级配置
        public SXML CurrentLevelConf()
        {
            SXML lvlxml = CurrentStageConf()?.GetNode("lvl", "level==" + Level);
            return lvlxml;
        }

        //!--获取下一等级配置
        public SXML NextLevelConf()
        {
            if (Level == StageMaxLvl)
            {
                SXML nextStageXML = NextStageConf();
                if (nextStageXML == null)
                    return null;
                return nextStageXML.GetNode("lvl", "level==1");
            }
            return CurrentStageConf().GetNode("lvl", "level==" + (Level + 1));
        }

        //!--监控宠物饲料是否用尽,用尽后提示一次
        //private bool isreset = false;
        private bool needHint = true;
        private uint feedid = 0;
        private void OnMonitorFeed(GameEvent e)
        {
            if (feedid == 0)
                feedid = GetFeedItemTpid();

            int num = a3_BagModel.getInstance().getItemNumByTpid(feedid);
            if (num > 0)
            {
                needHint = true;
            }
            else
            {
                if (needHint == true && Auto_buy == true)
                {
                    AutoBuy();
                }
                needHint = false;
            }
        }
        public void AutoBuy() 
        {
            uint num = 99;
            uint itm_tpid = GetFeedItemTpid();
             a3_BagModel bag = a3_BagModel.getInstance();
             a3_ItemData itmdata = bag.getItemDataById(itm_tpid);
             long money = num * itmdata.on_sale;
             if (PlayerModel.getInstance().money < money)
             {
                 int i = (int)PlayerModel.getInstance().money / itmdata.on_sale;
                 if (i > 0)
                 {
                     Shop_a3Proxy.getInstance().BuyStoreItems(itm_tpid, (uint)i);
                     flytxt.instance.fly(ContMgr.getCont("petmodel_addfood", new List<string> {i.ToString(),itmdata.item_name.ToString() }));
                     // flytxt.instance.fly("成功补充了" + i + "个" + itmdata.item_name);
                 }
                 else if (i <= 0)
                 {
                    flytxt.instance.fly(ContMgr.getCont("petmodel_nofood"));
                    //flytxt.instance.fly("金币不足，补充宠物饲料失败");
                 }
             }
             else
             {
                 Shop_a3Proxy.getInstance().BuyStoreItems(itm_tpid, num);
                flytxt.instance.fly(ContMgr.getCont("petmodel_addfood", new List<string> { num.ToString(), itmdata.item_name.ToString() }));
                //flytxt.instance.fly("成功补充了" + num + "个" + itmdata.item_name);
             }
        }
        public bool CheckLevelupAvaiable()
        {
            if (!FunctionOpenMgr.instance.Check(FunctionOpenMgr.PET))
                //|| CurrentLevelConf() == null 
                //|| Stage > PlayerModel.getInstance().up_lvl 
                //|| Stage == PlayerModel.getInstance().up_lvl 
                //    && Level >= PlayerModel.getInstance().lvl)
                return false;
            else if (CurrentLevelConf().getInt("level") >= StageMaxLvl)
                return NextStageConf() != null && a3_BagModel.getInstance().getItemNumByTpid(GetStageItemTpid()) >= NextStageConf().getInt("crystal");
            else
                return a3_BagModel.getInstance().getItemNumByTpid(GetLevelItemTpid()) >= NextLevelConf().getInt("cost_item_num");
        }
    }
}
