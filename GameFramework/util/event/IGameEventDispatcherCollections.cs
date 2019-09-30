using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public interface IGameEventDispatcherCollections
    {		
		bool dispatchEventCL( string objectName, GameEvent evt);
		void addEventListenerCL( string objectName, uint type, Action<GameEvent> listener);
		bool hasEventListenerCL( string objectName, uint type);
		void removeEventListenerCL( string objectName, uint type, Action<GameEvent> listener);
    }
}
