using System;
using System.Collections.Generic;
 
using Cross;

namespace GameFramework
{
 
 
	public class BaseLGUI : GameEventDispatcher, IObjectPlugin
	{
		public UIClient g_mgr;		 
		protected string m_uiName = "";
		protected IUI m_uiF;
		protected bool m_initFlag = false;
		protected bool m_bindFlag = false;
		protected bool m_openFlag = false;
		public bool g_regEventListenerFlag = false;
		private string _controlId;
		private Variant _singleInfo;
		
		public BaseLGUI( UIClient m )
		{
			g_mgr = m;			
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
		 
		public string uiName
		{
			get
			{
				if (m_uiName == "") m_uiName = controlId;
				return m_uiName;
			}
			set
			{				 
				m_uiName = value;
			}
		}

 
		public IUI uiF
		{
			get 
			{
				return m_uiF;
			}
		}	

		 public Variant singleInfo
		 {
			get{
				return _singleInfo;
			}
			set{
				_singleInfo = value;
			}
		 }
        protected virtual void _regEventListener()
		{ 
		}

		protected virtual void _unRegEventListener()
		{			 				
		}


		public void regEventListener()
		{ 
			g_regEventListenerFlag = true;
			_regEventListener();
		}

		public void unRegEventListener()
		{		
			g_regEventListenerFlag = false;	
			_unRegEventListener();				
		}


        virtual protected  void _open_(Variant data)
		{
            m_uiF.addEventListener( UI_EVENT_DEFAULT.UI_CLOSE,  onClickClose );
			regEventListener(); 
			onOpen( data );
		}
		virtual public void init()
		{//加载完界面 调用一次
		
		}
		virtual protected  void onOpen(Variant data)
		{
			g_mgr.addUiBase( m_uiF );
            m_uiF.onOpen(data);
		}
		virtual protected  void onClose()
		{
            m_uiF.onClose();
			g_mgr.rmUiBase( m_uiF );
		}
		public Boolean isOpen
		{
			get{
				return m_openFlag;
			}
		}
		public void open(Variant data)
		{
			if( m_openFlag ) return;
			m_openFlag = true;
			
			if( m_uiF == null ) 
			{
                if( g_mgr.showLoading( this ) ) onLoading();
				initOpen( data );
				return;
			}
			_open_(data);
		}
		
		
		
		public void close()
		{		
			unRegEventListener();
			if( m_uiF == null ) return;
            m_uiF.removeEventListener(UI_EVENT_DEFAULT.UI_CLOSE, onClickClose);
			onClose();
			m_openFlag = false;
		}
		virtual public void dispose()
		{ 
			if( m_uiF != null ) m_uiF.dispose();
			g_mgr = null;
			m_uiF = null;
		}

        //通知界面
		public void containerEvent(uint uiEventId, Variant data)
		{
			//m_uiF.actFun( key, data);
			dispatchEvent(
				GameEvent.Create(uiEventId, this, data)
			);
		}

        //向外通知
        public void broadCastEvent(uint uiEventId, object data)
		{			
			g_mgr.dispatchEvent(
				GameEvent.Create(uiEventId, this, data)
			);
		}

		private string _loadingUI = "";
        virtual public void onLoading()
        {
			_loadingUI = this.uiName;
            g_mgr.onLoadingUI( this );
        }
        virtual public void onLoadingEnd()
        {
			if( _loadingUI != this.uiName ) return;
			_loadingUI = "";
            g_mgr.onLoadingUIEnd( this );            
        }
		private void initOpen(Variant data)
		{			 
			bindui(_initOpen, data);
		}

		private void _initOpen(IUI ui, Variant info)
		{        
			onLoadingEnd();     
			if (!m_openFlag) return;
            _open_(info);
		}
		public void bindui( Action<IUI, Variant> cb, Variant data )
		{
            if( m_bindFlag ) return;

			if( g_mgr.showLoading( this ) ) onLoading();

			BaseLGUI thisptr = this;
            m_bindFlag = true;
			g_mgr.getUI(
				this.uiName,
				(IUI u, Variant info) =>
				{
					onLoadingEnd();
					if( u == null )
					{
						DebugTrace.add(Define.DebugTrace.DTT_ERR, " bindui ["+ uiName +"] Err!  ");
						return;
					}
					m_uiF = u;
					m_uiF.setCtrl(thisptr);
                    init();
					cb( u, info );
				},
				data
			);
		}
        private void onClickClose( GameEvent e )
        {
            close();
        }
	}
}