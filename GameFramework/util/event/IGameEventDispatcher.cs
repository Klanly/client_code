using System;
using System.Collections.Generic;
 

namespace GameFramework
{
    public interface IGameEventDispatcher
    {

		bool dispatchEvent( GameEvent evt);
		void addEventListener( uint type, Action<GameEvent> listener);
		bool hasEventListener( uint type);
		void removeEventListener( uint type, Action<GameEvent> listener);
		void removeAllListener();
		

    }
}
