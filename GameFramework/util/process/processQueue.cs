using System;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	
	public class processQueue 
	{
		private  List<IProcess> _processVec  = new  List<IProcess>();
		private  string _debugFlag = "";
		private int _limittmMS = GameConstantDef.PROCESS_DEF_LIMIT_MS_TM;
		public processQueue()
		{
			 
		}
		public void setDebug( string val )
		{
			_debugFlag = val;
		}

		public void setLimittm( int ms )
		{
			_limittmMS = ms;
		}

		private void _onProcess( float tmSlice )
		{//todo profile
			List<IProcess> _remove = new List<IProcess>();
			int i=0;
			IProcess p;

			// 统计消息处理时间，profiler分析			
			double startTm = CCTime.getTickMillisec();

			for( i=0; i<_processVec.Count; i++ )
			{
				p = _processVec[i];
				if( p == null )
				{	
					GameTools.PrintError( "processQueue onProcess null!");				 
					continue;
				}

				if( p.pause ) continue;
				if( p.destroy )
				{
					_remove.Add( p );
					continue;
				}
 
				
				double beginTm = CCTime.getTickMillisec();

				p.updateProcess( tmSlice );
 
				double procTm = CCTime.getTickMillisec() - beginTm;				 
				ProfilerManager.inst.profilerMark( procTm, _debugFlag, p.processName );

				if( startTm - beginTm > _limittmMS ) 
				{
					break;	
				}

			}
			
			foreach( IProcess match in _remove )
			{	
				for( i=_processVec.Count-1; i>=0; i-- )
				{
					p = _processVec[i];
					if( match != p ) continue;
					_processVec.RemoveAt( i );
					break;
				} 
			}
		}
  
		public void process( float tmSlice)
		{
			_onProcess( tmSlice );
		}
			
		public void addProcess( IProcess p )
		{
			if( p == null )
			{				 
				GameTools.PrintError( "processQueue addProcess null!");
				return;
			}
			if( p.processName == "")
			{
				GameTools.PrintError( "processQueue addProcess processName null!");
			}

            if (p.destroy)
            {
                p.destroy = false;
            }
			_processVec.Add( p );
		}
		
		public void removeProcess( IProcess p )
		{			 
			p.destroy = true;
		} 
 
	}
}
