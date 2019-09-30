using System;
using System.Collections;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
	abstract public class LGGRBaseImpls : GameEventDispatcher , IProcess, IObjectPlugin
	{
		private string _controlId;
		protected lgGDBase m_gameCtrl;
		protected GRBaseImpls m_drawBase;
		
		protected GRClient m_mgr;
		//protected Variant m_pos;// = new Variant();// = {x:0, y:0};

		protected float m_x = 0;
		protected float m_y = 0;
		protected float m_z = 0;
		 
		public float m_xMoveto = 0;
        public float m_yMoveto = 0;
        public float m_zMoveto = 0;
		 
		protected float xReal = 0;
		protected float yReal = 0;

		public LGGRBaseImpls(GRClient m )
		{
			m_mgr = m;
		}
		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}
		public void setDrawBase( GRBaseImpls d )
		{
			m_drawBase = d;
			//_drawBase.setEventFun( drawSpriteEventBack );
			onSetDrawBase();
		}
		abstract protected void onSetDrawBase();
		 
		public GRClient g_mgr
		{
			get{
				return m_mgr;
			}
		}
		virtual public void init()
		{	
			
		}
		public GRBaseImpls g_GRbase
		{
			get{
				return m_drawBase;
			}
		}
 
		public void setGameCtrl( lgGDBase gct )
		{
			m_gameCtrl = gct;
			onSetGameCtrl();
		}
		abstract protected void onSetGameCtrl();
		 
		
 
		public void dispose()
		{			 
			onDispose();
			m_drawBase.dispose();
			m_mgr = null;
			this.destroy = true;
		}
 
		virtual protected void onDispose()
		{
		} 

		public void setPosX( float val )
		{
			m_x = val;
		}
		public void setPosY( float val )
		{
			m_y = val;
		}
		public void setPosZ( float val )
		{
			m_z = val;
		}

		private bool _initPos = false;
		virtual public void setPos( float x, float y )
		{

			if( x == xReal && y == yReal ) return ;

			Vec3 v = g_mgr.trans3DPos( x, y ); 
			 	
			m_xMoveto = v.x;
			m_yMoveto = v.y;
			m_zMoveto = v.z;

			xReal = x;
			yReal = y;
		 
			if( !_initPos )
			{
				_initPos = true;
				m_x = v.x;
				m_y = v.y;
				m_z = v.z;
			}

		}

		public Vec3 getPoss()
		{
			return new Vec3( m_x, m_z, m_y );
		}

		//=== Iprocess ====
		private bool _pause = false;
		private bool _destory = false;
		private string _processName = "";
		virtual public void updateProcess(float tmSlice)
		{			 
		}

		public bool destroy
		{
			get
			{
				return _destory;
			}
			set
			{

				_destory = value;
			}
		}
		public bool pause
		{
			get
			{
				return _pause;
			}
			set
			{

				_pause = value;
			}
		}
		public string processName
		{
			get
			{
				return controlId;
			}
			set{
				_processName = value;
			}
		} 
		
	}
}