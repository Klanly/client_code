
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{

	public class muNetCleint : NetClient
	{ 
        static public muNetCleint instance;
		public muNetCleint(gameMain m):base(m)
		{			 
		}
		override protected void onInit()
		{
            instance = this;
			regCreator( OBJECT_NAME.DATA_CONN, connInfo.create );
			regCreator( OBJECT_NAME.DATA_JOIN_WORLD, joinWorldInfo.create );
			regCreator( OBJECT_NAME.DATA_CHARS, charsInfo.create );
			regCreator( OBJECT_NAME.DATA_ITEMS, itemsInfo.create );
            //regCreator( OBJECT_NAME.DATA_MISSION, missionInfo.create);
            regCreator(OBJECT_NAME.MSG_LEVEL, InGameLevelMsgs.create);
            regCreator(OBJECT_NAME.DATA_CHAT, chatInfo.create);
			regCreator(OBJECT_NAME.DATA_SMITHY, smithyInfo.create);
            //regCreator( OBJECT_NAME.DATA_MAIL, LGGDMails.create);
            regCreator(OBJECT_NAME.DATA_SHOP, shopInfo.create);
			regCreator( OBJECT_NAME.MSG_OUT_GAME, outGameMsgs.create );
			regCreator( OBJECT_NAME.MSG_GENERAL,  InGameGeneralMsgs.create );

			createAllSingleInst();
        //    LGDropItemManager.into(this);
			foreach ( IObjectPlugin val in m_objectPlugins.Values )
            {
				val.init();
            }

            PlayerInfoProxy.getInstance();
		} 
		



		public outGameMsgs outGameMsgsInst
		{
			get{
				return getObject( OBJECT_NAME.MSG_OUT_GAME ) as outGameMsgs;
			}
		}
    
        public InGameGeneralMsgs igGenMsg
        {
            get
            {
                return getObject(OBJECT_NAME.MSG_GENERAL) as InGameGeneralMsgs;
            }
        }
      
       
		public connInfo connInfoInst
		{
			get{
				return getObject( OBJECT_NAME.DATA_CONN ) as connInfo;
			}
		}
		public charsInfo charsInfoInst
		{
			get{
				return getObject( OBJECT_NAME.DATA_CHARS ) as charsInfo;
			}
		}
		public joinWorldInfo joinWorldInfoInst
		{
			get{
				return getObject( OBJECT_NAME.DATA_JOIN_WORLD ) as joinWorldInfo;
			}
		}
		public itemsInfo itemsInfoInst
		{
			get{
				return getObject( OBJECT_NAME.DATA_ITEMS ) as itemsInfo;
			}
		}

        //public missionInfo missionInfoInst
        //{
        //    get
        //    {
        //        return getObject(OBJECT_NAME.DATA_MISSION) as missionInfo;
        //    }
        //}

        public shopInfo shopInfoInst
        {
            get
            {
                return getObject(OBJECT_NAME.DATA_SHOP) as shopInfo;
            }
        }

        public  chatInfo chatInfoInst
        {
            get
            {
                return getObject(OBJECT_NAME.DATA_CHAT) as chatInfo;
            }
        }

        public smithyInfo smithyInfoInst
        {
            get
            {
                return getObject(OBJECT_NAME.DATA_SMITHY) as smithyInfo;
            }
        }
     
	}

}