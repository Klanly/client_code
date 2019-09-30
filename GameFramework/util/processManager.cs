using System;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	
	public class processManager : BaseCrossPlugin
	{
		private processQueue _processQMaster = null;
		private processQueue _processQ = null;
		private processQueue _renderQ = null;
		private processQueue _renderQMaster = null;
        public static processManager instance;

		public processManager()
		{
            instance = this;
			_processQMaster = new processQueue();
			_processQ = new processQueue();
			_renderQ = new processQueue();
			_renderQMaster = new processQueue();
			_processQMaster.setDebug( "processManager master proc " );
			_processQ.setDebug( "processManager proc" );
			_renderQMaster.setDebug( "processManager master _render " );
			_renderQ.setDebug( "processManager _render" );
		}
		 
		override public String pluginName
		{
			get{
				return "processManager";
			}
		}
		
		override public void onRender( float tmSlice )
		{ 
			_renderQMaster.process( tmSlice );
			_renderQ.process( tmSlice );
			//_renderQ.process( 0.05f );
		}
		override public void onProcess( float tmSlice )
		{
			_processQMaster.process( tmSlice );
			_processQ.process( tmSlice );
			//_processQ.process( 0.05f );
		}
			
		public void addProcess( IProcess p, bool master=false )
		{
			if( p == null )
			{				
				return;
			}
			if( master )
			{
				_processQMaster.addProcess( p );
			}
			else
			{
				_processQ.addProcess( p );
			}
		}	 

		public void removeProcess( IProcess p, bool master=false )
		{			 
			if( master )
			{
				_processQMaster.removeProcess( p );
			}
			else
			{
				_processQ.removeProcess( p );
			}
		}
		
		public void addRender( IProcess p, bool master=false )
		{
			if( p == null )
			{				
				return;
			}
		 
			if( master )
			{
				_renderQMaster.addProcess( p );
			}
			else
			{
				_renderQ.addProcess( p );
			}
			 
		}

		public void removeRender( IProcess p, bool master=false )
		{
			if( master )
			{
				_renderQMaster.removeProcess( p );
			}
			else
			{
				_renderQ.removeProcess( p );
			}
		}
		
	}
}