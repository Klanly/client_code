
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	abstract public class gameManager : clientBase 
	{
		public timersManager timers = new timersManager();
		public gameManager( gameMain m ):base(m)
		{
		}
		override public void init()
		{
			GMCommand.inst.setNetManager( g_netM as NetClient );

			this.g_processM.addRender( new processStruct( update, "timersManager" ) );

			onInit(); 

		}
		abstract protected void onInit();

		private void update(float tmSlice)
		{ 
			 
		}
	}

}