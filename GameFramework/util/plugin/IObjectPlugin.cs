using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
	public interface IObjectPlugin
	{
		string controlId
		{
			get;
			set;
		}
		void init();
	}
}
