using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  
using Cross;
namespace MuGame
{
 
	class muLGClient :gameManager 
	{
		 
        static public muLGClient instance;
		public muLGClient( gameMain m ):base(m)
		{
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
		override protected void onInit()
		{
            instance = this;
         
			regCreator( OBJECT_NAME.LG_OUT_GAME, LGOutGame.create );
			regCreator( OBJECT_NAME.LG_MAIN_PLAY, lgSelfPlayer.create );
			regCreator( OBJECT_NAME.LG_CAMERA, LGCamera.create );
			regCreator( OBJECT_NAME.LG_MAP, LGMap.create );
			regCreator( OBJECT_NAME.LG_NPCS, LGNpcs.create );
			
			regCreator( OBJECT_NAME.LG_JOIN_WORLD, LGJoinWorld.create );
			regCreator( OBJECT_NAME.LG_LOAD_RESOURCE, LGLoadResource.create );
            //regCreator( OBJECT_NAME.LG_SKILL, LGSkill.create);
			regCreator( OBJECT_NAME.LG_MONSTERS, LGMonsters.create );
            regCreator( OBJECT_NAME.LG_HEROS, LGHeros.create); 
			regCreator( OBJECT_NAME.LG_OTHER_PLAYERS,  LGOthers.create );

            regCreator(OBJECT_NAME.LG_LEVEL, LGGDLevels.create);

            new DelayDoManager(this);

			createAllSingleInst();

			foreach ( IObjectPlugin val in m_objectPlugins.Values )
            {				 
				val.init();
            }


			this.g_processM.addRender(this.g_selfPlayer, true ); 
		}

		

     

		public LGMap g_mapCT
		{
			get{
				return getObject( OBJECT_NAME.LG_MAP ) as LGMap;
			}
		}
		public lgSelfPlayer g_selfPlayer
		{
			get{
				return getObject( OBJECT_NAME.LG_MAIN_PLAY ) as lgSelfPlayer;
			}
		}

        public LGGDLevels g_levelsCT
        {
            get
            {
                return getObject(OBJECT_NAME.LG_LEVEL) as LGGDLevels;
            }
        }

    }
}