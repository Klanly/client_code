using System;
namespace GameFramework
{
	public interface IProcess
	{
		void updateProcess( float tmSlice );

		bool destroy
		{
			get;
			set;

		}
		bool pause
		{
			get;
			set;
		}
		string processName
		{
			get;
			set;
		}
		 
	}
}