using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	abstract public class UIClient : clientBase
	{
        static public UIClient instance;

		public UIClient( gameMain m):base(m)
		{
			
		}

        protected Dictionary<string, BaseLGUI> m_uiCtrlMap = new Dictionary<string, BaseLGUI>();
        protected Dictionary<string, IUI> m_uiMap = new Dictionary<string, IUI>();
        protected Dictionary<string, string> m_backgroundClickUIS = new Dictionary<string, string>();
				
		
		public timersManager timers = new timersManager();
		override public void init()
		{
            new AttAniManager(this);
            instance = this;

			this.addEventListener( UI_EVENT_DEFAULT.UI_OPEN, openUI );
			this.addEventListener( UI_EVENT_DEFAULT.UI_CLOSE, closeUI );
            this.addEventListener(UI_EVENT_DEFAULT.UI_OPEN_SWITCH, openSwitch);

            this.addEventListener(GAME_EVENT_DEFAULT.LG_MEDIA_PLAY, onPlaySound);	
			 
			this.g_processM.addProcess(  new processStruct( _getUIProcess, "UIClient_getUIProcess" ) );
			 
			this.g_processM.addProcess(  new processStruct( AttAniManager.singleton.process, "UIClient_AttAniManagerProcess" ) );

		 

			onInit();
		}
  
		abstract protected void onInit();


		protected void regCreatorLGUI( string name, Func<IClientBase, IObjectPlugin> creator )
		{
			regCreator( tracsLguiName( name ), creator);
		}		

		protected void regCreatorUI( string name, Func<IClientBase, IObjectPlugin> creator )
		{
			regCreator( name, creator);
		}

		protected string tracsLguiName( string name )
		{
			return name+"LG";
		}
		
		private void openUI( GameEvent e )
		{
			Variant d = e.data;
			string name = d[ "name" ];
			Variant info = null;
			if( d.ContainsKey( "data" ) )  info = d["data"] as Variant;
			_openUI( name, info );
		}
        private void openSwitch( GameEvent e )
		{
            Variant d = e.data;
			string name = d[ "name" ];
			Variant info = null;
			if( d.ContainsKey( "data" ) )  info = d["data"] as Variant;
            if( !( m_uiCtrlMap.ContainsKey( name ) ) )
			{
                _openUI( name, info );
                return;
            }
            BaseLGUI lgui = m_uiCtrlMap[ name ] as BaseLGUI;
            if( lgui.isOpen )
            {
                lgui.close();
            }
            else
            {
                lgui.open( info );
            }
        }

		protected void _openUI( string uiname, Variant data )
		{
			//if( !( _LGUIClass.ContainsKey( uiname ) ) )
			//{
			//	DebugTrace.add( Define.DebugTrace.DTT_ERR, "LGUIClass[ "+uiname+" ] not exsit!" );
			//	return;
			//}
			BaseLGUI lgui;

			if( !( m_uiCtrlMap.ContainsKey( uiname ) ) )
			{
				lgui = createInst( tracsLguiName( uiname ), true ) as BaseLGUI ;
				if( lgui == null )
				{
					GameTools.PrintNotice( " createInst LGUIClass[ "+uiname+" ] err!" );
				}
				lgui.uiName = uiname;
				m_uiCtrlMap[ uiname ] = lgui;

			} 

			lgui = m_uiCtrlMap[ uiname ] as BaseLGUI;
			if( !lgui.isOpen )
			{
				lgui.open( data );
			}
		}	
		
		
 
		private void closeUI( GameEvent e )
		{
			Variant d = e.data;
			string name =  d["name"];
			_closeUI( name );
		}
		protected void _closeUI( string uiname )
		{
			if( !( m_uiCtrlMap.ContainsKey( uiname  ) ) )
			{
				return;
			}
			m_uiCtrlMap[ uiname ].close();
		}
		
		public bool isUiOpened( string uiname )
		{
			if( !( m_uiCtrlMap.ContainsKey( uiname  ) ) )
			{
				return false;
			}
			return m_uiCtrlMap[ uiname ].isOpen;
		}


		public BaseLGUI getLGTUIInst(string lguiName, string uiName, Action<IUI, Variant> cb, Variant data)
		{
            BaseLGUI lgui = null;
            if( lguiName != "UI_BASE" )
            {
			    lgui =   createInst( tracsLguiName( lguiName ), false ) as BaseLGUI ; 

				if ( lgui == null )
			    {//todo err
					GameTools.PrintNotice(" getLGTUIInst LGUIClass[" + lguiName + "]  err!!");
				    return null;
			    }
            }
            else
            {
                lgui = new BaseLGUI( this );
            }

			lgui.uiName = uiName;
            lgui.bindui(cb, data);
			return lgui;
		}

        //获取逻辑UI
        public BaseLGUI getLGUI(string lguiName)
        {
            BaseLGUI lgui = null;
            if (!(m_uiCtrlMap.ContainsKey(lguiName)))
            {
                lgui = createInst(tracsLguiName(lguiName), true) as BaseLGUI;
                if (lgui == null)
                {
                    GameTools.PrintNotice(" createInst LGUIClass[ " + lguiName + " ] err!");
                    return null;
                }
                lgui.uiName = lguiName;
                m_uiCtrlMap[lguiName] = lgui;
            }
            lgui = m_uiCtrlMap[lguiName] as BaseLGUI;
            return lgui;
        }
		 
		private List<loadUIStruct> _uiLoads = new List<loadUIStruct>();
		public void getUI(string uiname, Action<IUI, Variant> cb, Variant data)
		{//获得界面

			if (!( hasCreator( uiname ) ) )
			{
				GameTools.PrintNotice("getUI[" + uiname + "]  err!!");
				return;
			}

			loadUIStruct st = new loadUIStruct();
			st.data = data;
			st.uiname = uiname;
			st.onLoadedCallback = cb;
			_uiLoads.Add( st );
		}
		private void _getUIProcess(float tmSlice)
		{//获得界面
            //return;
            //if (_uiLoads.Count <= 0) return;
            //loadUIStruct st = _uiLoads[0];
            //_uiLoads.RemoveAt( 0 );

            //string uiname = st.uiname;

            //if (!( hasCreator( uiname ) ) )
            //{
            //    GameTools.PrintNotice("getUI[" + uiname + "]  err!!");
            //    return;
            //} 

            //IUI uf = createInst( uiname, false ) as IUI;
			
            //GameTools.PrintNotice( " %%%%%%%%%%% GET UI ["+ uiname +"] %%%%%%%%%%% " );

            //cuiM.createUIControl(
            //    uiname,
            //    uf.eventReceiver,
            //    (IUIBaseControl ui) =>
            //    {
            //        if (ui == null)
            //        {
            //            GameTools.PrintNotice(" %%%%%%%%%%% GET UI Err [" + uiname + "] %%%%%%%%%%% ");
            //        }
            //        m_uiMap[uiname] = uf;
                    
            //        uf.setBaseCtrl(ui, m_backgroundClickUIS.ContainsKey(uiname));
            //        st.onLoadedCallback(uf, st.data);
            //    }
            //);
		}
		public void regBackgroundClick( string uiname )
		{
			m_backgroundClickUIS[ uiname ] = uiname;
		}

        public IUI getUIF( string name )
        {
            if (!m_uiMap.ContainsKey(name)) return null;
            return m_uiMap[ name ];            
        }


		
		virtual public bool showLoading(BaseLGUI ui)
        {
              return false;
        }

        virtual public void onLoadingUI(BaseLGUI ui)
        {
                
        }
        virtual public void onLoadingUIEnd(BaseLGUI ui)
        {
        }
		public void addUiBase( IUI uif )
		{
            //IUIBaseControl uic = uif.getBaseCtrl();
            //uic.visible = true;
            //this.cuiM.addUIControl( uic );
		}
		public void rmUiBase( IUI uif )
		{
            //IUIBaseControl uic = uif.getBaseCtrl();
            //uic.visible = false;
            //this.cuiM.removeUIControl( uic );
		}

        public IUIBaseControl getHirachyCreatedCtrlByID(string ids)
        {
            List<string> idAry = StringUtil.splitStr(ids, ".");
            IUI ui = getUIF(idAry[0]);
            if (ui == null)
                return null;

            IUIBaseControl ctrl = ui.getBaseCtrl();
			if(idAry.Count > 1)
			{
				if(!(ctrl is IUIContainer))
				{
					return null;
				}
                return getHirachyUIByID(ctrl, idAry[1]);
			}
			
			return ctrl;
        }

        public IUIBaseControl getHirachyUIByID(IUIBaseControl baseCtrl, string ids)
        {
            if (baseCtrl == null)
                return null;
            List<string> idAry = StringUtil.splitStr(ids, ".");
            IUIBaseControl ctrl = (baseCtrl as IUIContainer).getChild(idAry[0]);
            if(idAry.Count > 1)
            {
                if(!(ctrl is IUIContainer))
				{
					return null;
				}
                return getHirachyUIByID(ctrl, idAry[1]);
            }
            return ctrl;
        }

        virtual protected void onPlaySound(GameEvent e)
        {

        }

        //private UIManager cuiM
        //{
        //    get{
        //        return  CrossApp.singleton.getPlugin("ui") as UIManager;
        //    }
        //}

        //public void loadPreloadUIConfig( Action onfin )
        //{
        //    this.cuiM.loadPreloadUIConfig( onfin );
        //} 


			 
		public void removeProcess( IProcess p )
		{	
			this.g_processM.removeProcess( p );		 
		}
			
		public void addProcess( IProcess p )
		{
			this.g_processM.addProcess( p );
		}
		 
	}
	public class loadUIStruct
	{
		public Action<IUI, Variant> onLoadedCallback;
		public string uiname;
		public Variant data;
	}
}
