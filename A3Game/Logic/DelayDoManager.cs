using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    class DelayDoManager
    {
        private muLGClient g_mgr;
        static public DelayDoManager singleton;

        public DelayDoManager(muLGClient m)
        {
            singleton = this;
			g_mgr = m;
        }

        private Variant _delayDos = new Variant();

		//消除标记
		public const uint CF_NONE = 0;
		public const uint CF_CHANGE_MAP = 1;
		public const uint CF_SETSYSTEM = 2;
		public void AddDelayDo( Action fun, int tm, uint clearFlag = CF_NONE )
		{
			long id= g_mgr.timers.addTimer(
				tm,
				(object data )=>{
					fun();				
				 },
				 1,
				null
			);

			_delayDos.pushBack(GameTools.createGroup("fun",fun,"id",id,"cf",clearFlag));
		}
		
		public void RmvDelayDo(Action fun )
		{
			for( int i = _delayDos.Count - 1; i >= 0; --i )
			{
				Variant delayDo = _delayDos[i];
				if( (delayDo["fun"]._val as Action) == fun )
				{
                    long id = delayDo["id"];
					_delayDos._arr.RemoveAt(i);
                    g_mgr.timers.removeTimer(id);
				}
			}
		}
		
		public void ClearDelayDoByFlag( uint cf )
		{
			if( _delayDos.Count == 0 ) return;
			
			if( cf == 0 )
			{
				_delayDos = new Variant();
				return;
			}
			
			for( int i = _delayDos.Count - 1; i >= 0; --i )
			{
				Variant delayDo = _delayDos[i];
				if( (delayDo["cf"] & cf) != 0 )
				{
                    long id = delayDo["id"];
					_delayDos._arr.RemoveAt(i);
                    g_mgr.timers.removeTimer(id);
				}
			}
		}
    }
}