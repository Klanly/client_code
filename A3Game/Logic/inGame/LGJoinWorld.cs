using System;
using System.Collections.Generic;

using GameFramework;
using Cross;

namespace MuGame
{

    public class LGJoinWorld : lgGDBase
    {
        public LGJoinWorld(gameManager m)
            : base(m)
        {
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new LGJoinWorld(m as gameManager);
        }

        private bool _mapChangeFlag = false;
        private bool _enterFlag = false;
        private bool _firstFlag = false;
        private bool _first_join = true;

        override public void init()
        {

            this.g_mgr.g_netM.addEventListenerCL(
                OBJECT_NAME.DATA_JOIN_WORLD,
                PKG_NAME.S2C_JOIN_WORLD_RES,
                onJoinWorld
            );

			this.g_mgr.g_gameM.addEventListenerCL(
				OBJECT_NAME.LG_LOAD_RESOURCE,
				GAME_EVENT.ON_LOAD_MAP,
				onMapChange
			); 

			//this.g_mgr.g_sceneM.addEventListener(
   //             GAME_EVENT.MAP_LOAD_READY,
   //             onMapLoadReady
   //         );
        }

		//private void onMapLoadReady( GameEvent e )
  //      {       
		////	ogLoading.clearAll();  
  //      }

        private void onMapChange(GameEvent e)
        {
            _mapChangeFlag = true;
            tryBroadCastEnterGame();
        }
        private void onJoinWorld(GameEvent e)
        {
            _enterFlag = true;
            tryBroadCastEnterGame();
        }

        private void tryBroadCastEnterGame()
        {
            if (!_mapChangeFlag || !_enterFlag) return;
            _mapChangeFlag = false;

            if (!_firstFlag)
            {
                _firstFlag = true;
                //_joinWorld((this.g_mgr.g_netM as muNetCleint).joinWorldInfoInst.m_data);
                //lgGD_Dmis.GetDmis();
                //    (this.g_mgr.g_uiM.getLGUI(UIName.UI_MDLG_SYSTEM) as LGIUISystem).InitSystemSet();
                if (login.instance == null)
                    LGUIMainUIImpl_NEED_REMOVE.getInstance().show_all();
                //this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_OPEN, this, take_va(UIName.UI_MAIN)));
                //  this.g_mgr.g_uiM.dispatchEvent(GameEvent.Create(UI_EVENT.UI_DIPOSE_CREAT, this, null));
                //  (this.g_mgr.g_uiM.getLGUI(UIName.LGUIMission) as LGIUIMission).showMisTrack();
                // (this.g_mgr.g_gameM as muLGClient).lgGD_Award.SendGetAwardInfo();

                dispatchEvent(
                    GameEvent.Create(GAME_EVENT.ON_ENTER_GAME, this, null)
                );
            }
            else
            {				
			//	ogLoading.show();
                dispatchEvent(
                    GameEvent.Create(GAME_EVENT.ON_MAP_CHANGE, this, null)
                );

                //DoAfterMgr.instacne.addAfterRender(() =>
                //{
                //    MapEffMgr.getInstance().clearAll();

                //});
            }
            debug.Log("!!tryBroadCastEnterGame!! " + debug.count);
        }
    }
}
