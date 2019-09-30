
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 
	
	public class lgGDBase : GameEventDispatcher, IObjectPlugin
	{
        virtual public void initGr(GRBaseImpls grBase, LGGRBaseImpls lggrbase)
        {
            
        }

       
		private gameManager _mgr;
		public lgGDBase( gameManager m):base()
		{
			_mgr = m;
		}
		virtual public void init()
		{
		}

		private string _controlId;
		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}

		virtual public gameManager g_mgr
		{
			get{
				return  _mgr;
			}			
		}
	}
}