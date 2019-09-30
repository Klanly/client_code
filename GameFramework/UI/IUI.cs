using System;
using System.Collections.Generic;
using Cross;
 

namespace GameFramework
{


	public interface IUI :IGameEventDispatcher,IUIBase
	{

        void onOpen( Variant data );// 
        void onClose();
        void setCtrl( BaseLGUI uc );//管理器
        BaseLGUI getCtrl();
		
		void setBaseCtrl( IUIBaseControl ui, bool clickBack ); //界面窗口控件
		IUIBaseControl getBaseCtrl(); 
		void dispose();

	}
}

 