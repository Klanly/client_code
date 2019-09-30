using System;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
    public class GameEventDispatcherCollections : GameEventDispatcher, IGameEventDispatcherCollections
    {
		private Dictionary< string, IGameEventDispatcher > 
			_dispatcherMap = new Dictionary< string, IGameEventDispatcher >();
		public void regEventDispatcher( string objectName, IGameEventDispatcher ed )
		{
			if( _dispatcherMap.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR regEventDispatcher ["+ objectName +"] exsit!" );
				return;
			}

			if( ed == null )
			{
				DebugTrace.print( "ERR regEventDispatcher ["+ objectName +"] null!" );
				return;
			}

			_dispatcherMap[ objectName ] = ed;
		}

		public bool dispatchEventCL( string objectName, GameEvent evt )
		{
			if( !_dispatcherMap.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR dispatchEventCL ["+ objectName +"] not exsit!" );
				return false;
			}

			IGameEventDispatcher ged = _dispatcherMap[ objectName ];

			ged.dispatchEvent( evt );

			return true;
		}
		public void addEventListenerCL( string objectName, uint type, Action<GameEvent> listener )
		{
			if( !_dispatcherMap.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR addEventListenerCL ["+ objectName +"] not exsit!" );
				return;
			}

			IGameEventDispatcher ged = _dispatcherMap[ objectName ];

			ged.addEventListener( type, listener );
		}
		public bool hasEventListenerCL( string objectName, uint type )
		{
			if( !_dispatcherMap.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR hasEventListenerCL ["+ objectName +"] not exsit!" );
				return false;
			}
			IGameEventDispatcher ged = _dispatcherMap[ objectName ];
						 
			return ged.hasEventListener( type );;
		}

		public void removeEventListenerCL( string objectName, uint type, Action<GameEvent> listener )
		{
			if( !_dispatcherMap.ContainsKey( objectName ) )
			{
				DebugTrace.print( "ERR removeEventListenerCL ["+ objectName +"] not exsit!" );
				return;
			}
			IGameEventDispatcher ged = _dispatcherMap[ objectName ];
						 
			ged.removeEventListener( type, listener );;

		}

    }
}
