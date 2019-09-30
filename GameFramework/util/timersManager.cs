using System;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	public class timersManager 
	{
	 
		private Dictionary<long, ITimer > _timers = new Dictionary<long, ITimer >();
		public timersManager()
		{
			 	 
		}
		
		private processManager g_processM
		{
			get
			{
				return CrossApp.singleton.getPlugin("processManager") as processManager ;
			}
		} 
			
		public long addTimer( int delayTime, Action<object> callBack, int cnt=1, object data=null )
		{
			ITimer t = timer.create( 
				this,
				delayTime,
				callBack,
				cnt,
				data
			);

			g_processM.addProcess( t );

			long id = t.id;
			_timers[ id ] = t;
			t.start();
			return id;
		}
		
		public void removeTimer( long id )
		{		
			if( !_timers.ContainsKey( id ) )
			{
				GameTools.PrintError( "removeTimer id["+ id +"] err!"  );
				return ;
			}
			ITimer t = _timers[ id ];
				 
			g_processM.removeProcess( t );

		}
	}
	class timer : ITimer
	{
		private timersManager _mgr = null;

		private bool _destroy = false;
		private bool _pause = true;
		private string _processName ;

		private int _delayTime = 0;//ms
		private int _cnt = 0;
		 
		private Action<object> _callBack = null;
		private object _userData = null;
		private float _overTime = 0;
		private float _overCnt = 0;

		private long _id= 0;
		static private long _idCount = 0;
		static public timer create( timersManager mgr, int delayTime, Action<object> callBack, int cnt=1, object data=null )
		{
			return new timer( mgr, delayTime, callBack, cnt, data );
		}

		public timer( timersManager mgr, int delayTime, Action<object>callBack, int cnt, object data )
		{
			_mgr = mgr;
			_idCount++;
			_id = _idCount;
			_delayTime = delayTime;
			_cnt = cnt;
			_userData = data;
			_callBack = callBack;
		}
		public void start()
		{
			 this.pause = false;
		}

		public void updateProcess( float tmSlice )
		{
			_overTime += tmSlice;
			if( _overTime >= _delayTime )
			{
				_overCnt ++;
				_overTime = 0;
				_callBack( _userData );
			}

			if( _cnt!=0 && _overCnt >= _cnt )
			{
				_mgr.removeTimer( this.id );
			}
		}
		public long id
		{
			get
			{
				return _id;
			}
		}

		public bool destroy
		{
			get
			{
				return _destroy;
			}
			set
			{
				_destroy = value;
			}

		}
		public bool pause
		{
			get
			{
				return _pause;
			}
			set
			{
				_pause = value;
			}
		}
		
		public string processName
		{
			get
			{
				return _processName;
			}
			set{
				_processName = value;
			}
			 
		}
	}
}
