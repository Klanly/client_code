
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{



    public class muCLientConfig : ClientConfig
    {
        public muCLientConfig(gameMain m)
            : base(m)
        {
        }

        static public muCLientConfig instance;

        override protected void onInit()
        {
            instance = this;
         //   RoleMgr.init();

            regCreator(OBJECT_NAME.CONF_LOCAL_GENERAL,   ClientGeneralConf.create);
            regCreator(OBJECT_NAME.CONF_LOCAL_GRD,       ClientGrdConfig.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_GUIDE,     ClientGuideConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_IMGRES,    ClientImgResConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_ITEMS,     ClientItemsConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_MARKET,    ClientMarketConf.create);
            regCreator(OBJECT_NAME.CONF_LOCAL_OUTGAME,   ClientOutGameConf.create); //这里记录了随机名字
            //////regCreator(OBJECT_NAME.CONF_LOCAL_SHIELD,    ClientShieldConf.create);
            //regCreator(OBJECT_NAME.CONF_LOCAL_SKILL,     ClientSkillConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_SYSTEM,    ClientSystemConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_SYSTEMOPEN,ClientSystemOpenConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_TRIGGER,   ClientTriggerConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_VIP,       ClientVipConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_FWGAME, ClientFWGameConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_ACTIVITYVIEWCONF, ClientActivityViewConf.create);
            //////regCreator(OBJECT_NAME.CONF_LOCAL_AICONF, ClientAIConf.create);
            //regCreator(OBJECT_NAME.CONF_LOCAL_EQUIP, EquipConf.create);
            //regCreator(OBJECT_NAME.CONF_LOCAL_ITEM, ItemConf.create);
            //regCreator(OBJECT_NAME.CONF_LOCAL_TASK, TaskConf.create);
            //regCreator(OBJECT_NAME.CONF_LOCAL_HERO, HeroConf.create);

            regCreator(OBJECT_NAME.CONF_LOCAL_MONSTER, MonsterConfig.create);
            regCreator(OBJECT_NAME.CONF_LOCAL_SKILL, SkillConf.create);
            

            regCreator(OBJECT_NAME.CONF_SERVER_MAP, SvrMapConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_MARKET, SvrMarketConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_MERI, SvrMeriConfig.create);
           // regCreator(OBJECT_NAME.CONF_SERVER_MONSTER, SvrMonsterConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_MISSION, SvrMissionConfig.create);
            regCreator(OBJECT_NAME.CONF_SERVER_NPC, SvrNPCConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_ITEM, SvrItemConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_GENERAL, SvrGeneralConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_SKILL, SvrSkillConfig.create);
            regCreator(OBJECT_NAME.CONF_SERVER_LEVEL, SvrLevelConfig.create);
            //////regCreator(OBJECT_NAME.CONF_SERVER_CARRLVL, SvrCarrLvlConfig.create);

            createInst( OBJECT_NAME.CONF_LOCAL_GRD, true );
        }

        public ClientAIConf localAI
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_AICONF) as ClientAIConf; }

        }
        public ClientGeneralConf localGeneral
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_GENERAL) as ClientGeneralConf; }

        }
        public ClientGrdConfig localGrd
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_GRD) as ClientGrdConfig; }
        }
        public ClientGuideConf localGuild
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_GUIDE) as ClientGuideConf; }
        }
        public ClientImgResConf localImgRes
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_IMGRES) as ClientImgResConf; }
        }
        public ClientItemsConf localItems
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_ITEMS) as ClientItemsConf; }
        }
        public ClientMarketConf localMarket
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_MARKET) as ClientMarketConf; }
        }
        public ClientOutGameConf localOutGame
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_OUTGAME) as ClientOutGameConf; }
        }
        public ClientShieldConf localShield
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_SHIELD) as ClientShieldConf; }
        }
        public ClientSkillConf localSkill
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_SKILL) as ClientSkillConf; }
        }
        public ClientSystemConf localSystem
         {
             get { return getObject(OBJECT_NAME.CONF_LOCAL_SYSTEM) as ClientSystemConf; }
         }
        public ClientSystemOpenConf localSystemOpen
         {
             get { return getObject(OBJECT_NAME.CONF_LOCAL_SYSTEMOPEN) as ClientSystemOpenConf; }
         }
        public ClientTriggerConf localTrigger
         {
             get { return getObject(OBJECT_NAME.CONF_LOCAL_TRIGGER) as ClientTriggerConf; }
         }
        public ClientVipConf localVip
         {
             get { return getObject(OBJECT_NAME.CONF_LOCAL_VIP) as ClientVipConf; }
         }
        public ClientFWGameConf localFWGame
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_FWGAME) as ClientFWGameConf; }
        }
        public ClientActivityViewConf localActivityView
        {
            get { return getObject(OBJECT_NAME.CONF_LOCAL_ACTIVITYVIEWCONF) as ClientActivityViewConf; }
        }



        //-------------------------sever------------------------------------------------------
        public SvrMapConfig svrMapsConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_MAP) as SvrMapConfig; }
        }
        public SvrMissionConfig svrMisConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_MISSION) as SvrMissionConfig; }
        }

        public SvrNPCConfig svrNpcConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_NPC) as SvrNPCConfig; }
        }

        public SvrGeneralConfig svrGeneralConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_GENERAL) as SvrGeneralConfig; }
        }

        public SvrMonsterConfig svrMonsterConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_MONSTER) as SvrMonsterConfig; }
        }

        public SvrItemConfig svrItemConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_ITEM) as SvrItemConfig; }
        }

        public SvrSkillConfig svrSkillConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_SKILL) as SvrSkillConfig; }
        }

        public SvrMarketConfig svrMarketConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_MARKET) as SvrMarketConfig; }
        }

        public SvrMeriConfig svrMeriConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_MERI) as SvrMeriConfig; }
        }

        public SvrLevelConfig svrLevelConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_LEVEL) as SvrLevelConfig; }
        }
        public SvrCarrLvlConfig svrCarrLvlConf
        {
            get { return getObject(OBJECT_NAME.CONF_SERVER_CARRLVL) as SvrCarrLvlConfig; }
        }
    }
}