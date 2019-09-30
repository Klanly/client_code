using System;
namespace GameFramework
{
	public interface ITimer : IProcess
	{
		 
		long id
		{
			get;
		}
		void start();
 
	}
}

