using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
	 
	public class clientBase : GameEventDispatcherCollections, IClientBase
	{
		private gameMain _main;
		private Dictionary< string, Func<IClientBase, IObjectPlugin> > 
			m_creators = new Dictionary< string, Func<IClientBase, IObjectPlugin> >();
		protected Dictionary< string, IObjectPlugin > m_objectPlugins = new Dictionary<string,IObjectPlugin>();
		public clientBase(gameMain m):base()
		{
			_main = m;			 
		}
 
		virtual public void init()
		{
			 
		}
		
		public void regCreator( string name, Func<IClientBase, IObjectPlugin> creator )
		{
			if( m_creators.ContainsKey( name ) )
			{
				DebugTrace.print( "gameManagerBase regCreator ["+ name +"] exsit!" );
				return;
			}
			m_creators[ name ] = creator;
		}

		public IObjectPlugin createInst( string name, bool single )
		{
			if( !m_creators.ContainsKey( name ) )
			{
				DebugTrace.print( "err gameManagerBase createInst ["+ name +"] notExsit!" );
				return null;
			}
			if( single && m_objectPlugins.ContainsKey( name) )
			{
				DebugTrace.print( "err gameManagerBase createInst single ["+ name +"] repeated!" );
				return m_objectPlugins[ name ] as IObjectPlugin;
			}

			Func<IClientBase, IObjectPlugin> create = m_creators[ name ];
			IObjectPlugin obj = create( this );


			obj.controlId = name;

			if( single ) 
			{
				regEventDispatcher( name, obj as IGameEventDispatcher );
				m_objectPlugins[ name ] = obj;
			}

			return obj;
		}
		public bool hasCreator( string name )
		{ 
			return m_creators.ContainsKey( name );
		}
		public void createAllSingleInst( /*Action<IObjectPlugin> cbfun */)
		{  
			foreach( string key in m_creators.Keys )
			{
				Func<IClientBase, IObjectPlugin> create = m_creators[ key ];

				IObjectPlugin obj = create( this );
				obj.controlId = key;

				m_objectPlugins[ key ] = obj;

				//if( cbfun != null ) cbfun( obj );

				regEventDispatcher( key, obj as IGameEventDispatcher );
			}
		}
		public IObjectPlugin getObject( string objectName )
		{
			if( !m_objectPlugins.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR gameManagerBase getObject ["+ objectName +"] not exsit!" );
				return null;
			}
			return  m_objectPlugins[ objectName ];
		}

		public NetClient g_netM
		{
			get{
				return _main.g_netM as NetClient;
			}
		} 

		public gameManager g_gameM
		{
			get{
				return _main.g_gameM as gameManager;
			}
		} 

		public GRClient g_sceneM
		{
			get{
				return _main.g_sceneM as GRClient;
			}
		}
 
 		public UIClient g_uiM
		{
			get{
				return _main.g_uiM as UIClient;
			}
		}

		public ClientConfig g_gameConfM
		{
			get{
				return _main.g_gameConfM as ClientConfig;
			}
		} 

		public processManager g_processM
		{
			get
			{
				return CrossApp.singleton.getPlugin("processManager") as processManager ;
			}
		}

	 
		public LocalizeManager g_localizeM
		{
			get{
				return  CrossApp.singleton.getPlugin("localize") as LocalizeManager; 
			}
		}
        
	}
}