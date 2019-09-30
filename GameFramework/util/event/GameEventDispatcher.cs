
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
	public class GameEventDispatcher : IGameEventDispatcher
	{
		//public delegate void listenerFun(GameEvent e);

		private Dictionary<uint, List<Action<GameEvent>>> _dispatcherMap;
		private Dictionary<uint, List<Action<GameEvent>>> _removeMap;
		private bool _processFlag = false;
		public GameEventDispatcher()
		{
			_dispatcherMap = new Dictionary<uint, List<Action<GameEvent>>>();
			_removeMap = new Dictionary<uint, List<Action<GameEvent>>>();
		}
		
		//if (m_plugins.ContainsKey (name))
		public bool dispatchEvent(GameEvent evt)
		{// 			
			if( !_dispatcherMap.ContainsKey( evt.type ) )
			{
				return false;
			}
			uint type = evt.type;

			List<Action<GameEvent>> removeListenerFuns = null; 
			if( _removeMap.Count > 0 && _removeMap.ContainsKey( type ) )
			{
				removeListenerFuns = _removeMap[ type ];
			}

			_processFlag = true;

			//DebugTrace.print( "dispatchEvent:" + evt.type );

			List<Action<GameEvent>> actFuns = _dispatcherMap[ type ];
			
			for( int i=0; i<actFuns.Count; i++ )
			{
				//DebugTrace.print( "process idx["+i+"]" );
				Action<GameEvent> func = actFuns[i]  as Action<GameEvent>;
				if( removeListenerFuns!=null && removeListenerFuns.IndexOf( func ) >= 0 )
				{
					continue;
				}

				if( evt.immediately )
				{					
					func(evt);					
				}
				else
				{	 
					this.eventDelegate.addEventTask(evt, func );
				}
			}

			if( evt.GC_FLAG ) GameEvent.free( evt );

			_processFlag = false;

			if( removeListenerFuns!=null &&  removeListenerFuns.Count > 0 )
			{ 				
				foreach( Action<GameEvent> func in actFuns )
				{
					removeListener( removeListenerFuns, func );
				}
				 
				_removeMap.Clear();
			}

			return true;
		}
		//注:type应该是全属唯一编号  
		public void addEventListener(uint type, Action<GameEvent> listener)
		{			 
			if( !_dispatcherMap.ContainsKey( type ) )
			{
				_dispatcherMap[type] = new List<Action<GameEvent>>();
			}
			List<Action<GameEvent>> listenerFuns = _dispatcherMap[type]; 
			listenerFuns.Add( listener );
		}
		public bool hasEventListener( uint type )
		{
			return _dispatcherMap.ContainsKey( type );
		}
		//public void removeEventListener(uint type, Action<GameEvent> listener)
		//{
		//	if( !_dispatcherMap.ContainsKey( type ) ) return;

		//	List<Action<GameEvent>> listenerFuns = _dispatcherMap[type]; 
			
		//	if( listenerFuns.Count <= 0 ) return;
		//	for( int i=listenerFuns.Count-1; i>=0; i-- )
		//	{
		//		 if( listenerFuns[i] != listener ) continue;
		//		 listenerFuns.RemoveAt(i);	
		//	}
		//}

		public void removeEventListener(uint type, Action<GameEvent> listener)
		{
			if( !_dispatcherMap.ContainsKey( type ) ) return;

			if( _processFlag )
			{
				
				if( !_removeMap.ContainsKey( type ) )
				{
					_removeMap[type] = new List<Action<GameEvent>>();
				}
				List<Action<GameEvent>> removeListenerFuns = _removeMap[type]; 
				removeListenerFuns.Add( listener );
			}
			else{			
				List<Action<GameEvent>> listenerFuns = _dispatcherMap[type]; 
				removeListener( listenerFuns, listener );				 	 
			}
		}


		public void removeListener( List<Action<GameEvent>> listenerFuns, Action<GameEvent> listener)
		{
			//List<Action<GameEvent>> listenerFuns = _dispatcherMap[type];
			if( listenerFuns.Count <= 0 ) return;
			for( int i=listenerFuns.Count-1; i>=0; i-- )
			{
				 if( listenerFuns[i] != listener ) continue;
				 listenerFuns.RemoveAt(i);	
			}
		}
		public void removeAllListener()
		{
			_removeMap.Clear();
			_dispatcherMap.Clear();
		}


		private gameEventDelegate eventDelegate
		{
			get {
                return CrossApp.singleton.getPlugin("gameEventDelegate") as gameEventDelegate;
			}
		}
	}
}