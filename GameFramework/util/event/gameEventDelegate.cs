using System;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
 
   // TickItem 
	public class gameEventDelegate : BaseCrossPlugin
	{
     
		public gameEventDelegate()
		{			
			
		}

        //static private gameEventDelegate _singleton;		
        //static public gameEventDelegate singleton
        //{
        //    get	{
        //        if( _singleton  == null ) _singleton = new gameEventDelegate();
        //        return _singleton;
        //    }

        //}
		override public String  pluginName
		{
			get{
				return "gameEventDelegate";
			}
		}



		private List<taskEvent> _taskList = new List<taskEvent>();
		public void addEventTask( GameEvent evt, Action<GameEvent> listenerFun )
		{
			taskEvent t = taskEvent.alloc();
			t.evt = evt;
			t.listenerFun = listenerFun;
			_taskList.Add( t );
		}
		
		override public void onProcess( float tmSlice )
		{
			_process_updata( tmSlice );
		}
		private void _process_updata(  float tmSlice )
		{//  todo time limit and profile
			
			while( _taskList.Count > 0 )
			{

				taskEvent t = _taskList[0];
                _taskList.RemoveAt( 0 );

#if DEBUG
				// 统计消息处理时间，profiler分析			
				double beginTm = CCTime.getTickMillisec();
#endif

				t.listenerFun( t.evt );

#if DEBUG

				double procTm = CCTime.getTickMillisec() - beginTm;
				 
				ProfilerManager.inst.profilerMark( procTm, "gameEventDelegate", t.evt.type.ToString() );
#endif

                taskEvent.free(t);
			}	
		}
	}
	
}
 


