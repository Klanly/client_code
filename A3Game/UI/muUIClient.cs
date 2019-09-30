
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;
namespace MuGame
{

    public class muUIClient : UIClient
    {
        public muUIClient(gameMain m)
            : base(m)
        {

        }

        override protected void onInit()
        {
            //regCreatorLGUI(UIName.LGUIMainUIImpl, LGUIMainUIImpl_NEED_REMOVE.create);

            //this.addEventListener(UI_EVENT.UI_ON_TRY_ENTER_GAME, onTryEnterGame);
            (this.g_gameM.getObject(OBJECT_NAME.LG_JOIN_WORLD)
                as LGJoinWorld
            ).addEventListener(GAME_EVENT.ON_ENTER_GAME, onEnterGame);
            new DisplayUtil(this);


            //backgroundClick();
        }

        private void backgroundClick()
        {
          
            regBackgroundClick(UIName.UIMdlgClan);
            regBackgroundClick(UIName.UIMdlgClanBigDialog);

            regBackgroundClick(UIName.UIMdlgClanJoinList);
            regBackgroundClick(UIName.UIMdlgClanMemberList);
            regBackgroundClick(UIName.UIMdlgClanSkill);
            regBackgroundClick(UIName.UIMdlgClanSmallDialog);
            regBackgroundClick(UIName.UIMdlgClanViewList);
        }


        override public bool showLoading(BaseLGUI ui)
        {
            return m_backgroundClickUIS.ContainsKey(ui.uiName);
        }
        override public void onLoadingUI(BaseLGUI ui)
        {
            //UILoading.loading.showLoading = true;
            //Variant d = new Variant();
            //d["name"] = "Load ui id " + ui.uiName;
            //UILoading.loading.setText(d);
            ////LGUIOGLoadingImpl lguiload = this.getLGUI(UIName.LGUIOGLoadingImpl) as LGUIOGLoadingImpl;
            ////lguiload.setTipInfo("Load ui  " + ui.uiName);
  

        }

        override public void onLoadingUIEnd(BaseLGUI ui)
        {
            //UILoading.loading.showLoading = false;
        }

        public void onTryEnterGame()//(GameEvent e)
        {
            //if (isUiOpened(UIName.UI_SELECT))
            //{
            //    _closeUI(UIName.UI_SELECT);
            //    //UILoading.loading.showLoading = true;
            //}
            //LGIUIOGLoading ogLoading = this.getLGUI(UIName.LGUIOGLoadingImpl) as LGIUIOGLoading;
            //ogLoading.show();
            if(login.instance == null)
                debug.Log("打开loading ！！！ login==null");
            else
                debug.Log("打开loading ！！！ login!=null");

            if(!LGPlatInfo.relogined)
                debug.Log("打开loading ！！！ LGPlatInfo.relogined==false");
            else
                debug.Log("打开loading ！！！ LGPlatInfo.relogined==true");

            //资源会再前面加载，这里必须使用异步加载 map_loading
            if (login.instance==null && !LGPlatInfo.relogined)InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.MAP_LOADING);

            //if (debug.instance != null)
            //    debug.instance.showMsg(ContMgr.getOutGameCont("debug4"), 3);
        }
        private void onEnterGame(GameEvent e)
        {
            //_openUI(UIName.UI_TEST_BUTTON, e.data);
            //UILoading.loading.showLoading = false;
        }
        protected override void onPlaySound(GameEvent e)
        {
          //  MediaClient.getInstance();
          //  m_media.dispatchEvent(GameEvent.Create(GAME_EVENT.LG_MEDIA_PLAY, this, e.data));
        }

        //public LGUIMainUIImpl_NEED_REMOVE lguiMainUIImpl
        //{
        //    get
        //    {
        //        return this.getLGUI(UIName.LGUIMainUIImpl) as LGUIMainUIImpl_NEED_REMOVE;
        //    }
        //}
        //private MediaClient m_media
        //{
        //    get
        //    {
        //        return MediaClient.getInstance();
        //    }
        //} 
    }
}
