using System;
using System.Collections.Generic; 
using Cross;
namespace GameFramework
{

	interface IMessageSend
	{
		void sendRPC( uint cmd, Variant msg );
		 
		void sendTpkg(  uint cmd, Variant msg );
		 
	}
}
