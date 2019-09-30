using System;
using System.Collections.Generic;
using Cross; 

namespace GameFramework
{
	public class UIbase : GameEventDispatcher, IUI, IObjectPlugin
	{
		private string _controlId;
        public  Variant userdata;
        protected Variant _openData;
		protected BaseLGUI _ctrl;
		
 
		protected IUIBaseControl _win;
 
		public UIClient g_mgr;
	
		protected Dictionary<string, Action<IUIBaseControl, Event>> 
			_eventReceiver = new Dictionary<string,Action<IUIBaseControl,Event>>();

		public UIbase(UIClient m)
		{
			g_mgr = m;
            _eventReceiver["onMouseClkBtnClose"] = onMouseClkBtnClose;
			 
		}
		virtual public void init()
		{
		
		}

        public void onOpen(Variant data)
        {
            _openData = data;
            _onOpen();
        }
        public void onClose()
        {
            _onClose();
        }
        virtual protected void _onOpen()
        {

        }
        virtual protected void _onClose()
        {

        }
		public Dictionary<string, Action<IUIBaseControl, Event>> eventReceiver
		{
			get{
				return _eventReceiver;
			}
		}

		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}

 

		virtual public void dispose()
		{ 
			if( _win != null ) _win.dispose();
			_eventReceiver = null;
			g_mgr = null;
			_ctrl = null;
		}
  
		public void setCtrl(BaseLGUI ctrl)
		{
			_ctrl = ctrl;
			onSetCtrl();
		}

		public BaseLGUI getCtrl()
		{
			return _ctrl;
		}

		protected void eventAct(uint uiEventId, Variant data)
		{// 
			dispatchEvent(
				GameEvent.Create(uiEventId, this, data)
			);
		}

		virtual protected void onSetCtrl()
		{//子类重写
		}
 
		//外部设置界面窗口控件
		public void setBaseCtrl( IUIBaseControl ui, bool clickBack = false )
		{
			_win = ui;
			if( clickBack ) 
			{
				ui.addEventListener(Define.EventType.MOUSE_DOWN, clickBackground);
			}
			_bindControl(_win );
			onSetBaseCtrl();
            init();
		}
		private void clickBackground( IUIBaseControl ui, Event e )
		{
			onClickBackground();
		}
		virtual protected void onClickBackground()
		{//子类重写
		}

		virtual protected void onSetBaseCtrl()
		{//子类重写
		}

		virtual protected void _initControl(Dictionary<string, IUIBaseControl> host)
		{
            if (host.ContainsKey("mainframe"))
            {
                (host["mainframe"] as IUIImageBox).mouseEnabled = true;;
            }
		}

		protected void _bindControl(IUIBaseControl ctrl)
		{

			Dictionary<string, IUIBaseControl> 
				ctrlHost = new Dictionary<string, IUIBaseControl>(); //child controls dictionary

			if (ctrl != null && ctrl is IUIContainer)
			{
				this._bindChildControls(ctrlHost, ctrl);
				//ctrl.bindUI = this;
			}

			_initControl(ctrlHost);
		}

 

		protected void _bindChildControls( Dictionary<string, IUIBaseControl> ctrlHost, IUIBaseControl ctrl )
		{
			IUIContainer container = ctrl as IUIContainer;
			if (container == null)
			{
				return;
			}

			for (int i = 0; i < container.numChildren; ++i)
			{
				IUIBaseControl child = container.getChild(i);

				if (ctrlHost.ContainsKey(child.id))
				{
					continue;
				}

				ctrlHost[child.id] = child;
				//child.bindUI = this;

				this._bindChildControls(ctrlHost, child);
			}
		}

		public IUIBaseControl getBaseCtrl()
		{
			return _win;
		}

        public IUIBaseControl control
        {
            get
            {
                return _win;
            }
        }
        virtual public void onMouseClkBtnClose(IUIBaseControl ui, Event evt)
        {
            this.getCtrl().close();
        }
	}
}