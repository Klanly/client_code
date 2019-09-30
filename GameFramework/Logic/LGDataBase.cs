
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace GameFramework
{
	 
	public abstract class LGDataBase : GameEventDispatcher, IObjectPlugin
	{	 
		private IClientBase _mgr;
		private Variant _data;
		private string _controlId;
		public LGDataBase( IClientBase m ):base()
		{
			_mgr = m;
		}
		abstract public void init();
		
		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}
		


		public NetClient g_mgr
		{
			get{
				return _mgr as NetClient;
			}
		}

		public Variant m_data
		{
			get{
				if( _data == null ) _data = new Variant();
				return _data;
			}
			set{
				_data = value;
			}
		}
	}
}